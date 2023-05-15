using Aspose.Slides;
using Aspose.Slides.Charts;
using iSHOP.BLL;
using iSHOPNew.DAL;
using Svg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AsposeSlide = Aspose.Slides;

namespace iSHOPNew.Models
{
    public class ErrorLogSar
    {
        public static void LogError(string error_message, string stack_trace,HttpContextBase context)
        {
            error_message = Convert.ToString(error_message).Replace("'", "");
            stack_trace = Convert.ToString(stack_trace).Replace("'", "");
            if (context.Session[SessionVariables.USERID] == null)
            {
                if (!context.Response.IsRequestBeingRedirected)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                    {
                        context.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["SSOLogoutPageUrl"].ToString()));
                    }
                    else
                    {
                        context.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                    }
                }
            }

            UserParams userparam = context.Session[SessionVariables.USERID] as UserParams;
            if (userparam != null)
            {
                string username = userparam.UserName;
                string name = context.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

                string comments = "";
                comments += context.Request.Browser.Platform;
                comments += "Type=" + context.Request.Browser.Type + ", ";
                comments += "Name=" + context.Request.Browser.Browser + ", ";
                comments += "Version=" + context.Request.Browser.Version + ", ";
                comments += "Major Version=" + context.Request.Browser.MajorVersion + ", ";
                comments += "Minor Version=" + context.Request.Browser.MinorVersion + ", ";
                comments += "Platform=" + context.Request.Browser.Platform + ", ";
                comments += "Is Beta=" + context.Request.Browser.Beta + ", ";
                comments += "Is Crawler=" + context.Request.Browser.Crawler + ", ";
                comments += "Is AOL=" + context.Request.Browser.AOL + ", ";
                comments += "Is Win16=" + context.Request.Browser.Win16 + ", ";
                comments += "Is Win32=" + context.Request.Browser.Win32 + ", ";
                comments += "Supports Frames=" + context.Request.Browser.Frames + ", ";
                comments += "Supports Tables=" + context.Request.Browser.Tables + ", ";
                comments += "Supports Cookies=" + context.Request.Browser.Cookies + ", ";
                comments += "Supports VB Script=" + context.Request.Browser.VBScript + ", ";
                comments += "Supports JavaScript=" + context.Request.Browser.JScriptVersion + ", ";
                comments += "Supports Java Applets=" + context.Request.Browser.JavaApplets + ", ";
                comments += "CDF=" + context.Request.Browser.CDF;

                string strHostName = System.Net.Dns.GetHostName();
                string strAddress = "";
                try
                {
                    strAddress = GetIP4Address(context);

                    strHostName = (System.Net.Dns.GetHostEntry(context.Request.ServerVariables["remote_addr"]).HostName);
                }
                catch
                {

                }

                Random rnd = new Random();
                string strSessionId = username + rnd.Next(0, 100000).ToString();
                strSessionId += "." + (rnd.NextDouble() * 1000).ToString("0.00000");
                string qry = "insert into tblUserErrorLog(error_message, stack_trace, session_id, username, login_time, active, machine_ip, hostname, comments, browser) values ('" +
                             error_message + "', '" +
                             stack_trace + "', " +
                            cf.getQ(strSessionId) + ", " +
                            cf.getQ(username) + ", GETDATE(), 1, " + cf.getQ(strAddress) + ", " + cf.getQ(strHostName) + ", " +
                            cf.getQ(comments) + ", " + cf.getQ(name) + ")";

                //string Connectionstring = ConfigurationManager.ConnectionStrings["Sql_Connectionstring_IdMapping"].ToString();
                string Connectionstring = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Sql_Connectionstring_IdMapping"]].ConnectionString;
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(Connectionstring);
                System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(qry, conn);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                sqlCmd.ExecuteNonQuery();
                conn.Close();
                sqlCmd.Dispose();
                conn.Dispose();
                //HttpContext.Current.Response.Flush();     

                var httpRequestBase = new HttpRequestWrapper(context.ApplicationInstance.Request);
                if (!context.Response.IsRequestBeingRedirected && !httpRequestBase.IsAjaxRequest())
                    context.Response.Redirect("~/Error/Error");
                //ErrorController error = new ErrorController();
                //error.Error();
            }
        }

        public static string GetIP4Address(HttpContextBase context)
        {
            string IP4Address = "";
            IP4Address = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (IP4Address == "" || IP4Address == null)
                IP4Address = context.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(IP4Address) && IP4Address.Trim().ToLower() == "::1")
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }
            }
            return IP4Address;
        }
    }

    public class SARReport
    {
        public string UserExportFileName = string.Empty;
        public string Source = string.Empty;
        public string filename = string.Empty;
        public string sPowerPointTemplatePath = string.Empty;
        public static Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public static Random rnd = new Random();
        public object SignificanceValue, PositiveValue, NegativeValue;
        public string volume = string.Empty;
        public string significance = string.Empty;
        public List<string> lstHeaderText = new List<string>();
        string xmlpath = string.Empty;
        string rowheight = string.Empty;
        CommonFunctions _commonfunctions = new CommonFunctions();
        List<string> ChannelNets = new List<string>();
        public Presentation pres = new Presentation(); // creates a blank presentation with one blank slide.  must be done first
        public ISlideCollection slds = null;
        public ISlide cur_Slide = null;
        public DataSet dset = new DataSet();
        public System.Data.DataTable scatterTable = new System.Data.DataTable();
        public List<string> objectivelist = null;
        public List<double> objectivelistTripSummary = null;
        public List<string> metriclist = null;
        public int deviationValue;
        public int rangeValue;

        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public double StatTesting;

        public float chart_x_position = 0;
        public float chart_y_position = 0;
        public float chart_width = 0;
        public float chart_height = 0;
        public CommonFunctions commonfunctions = new CommonFunctions();

        public string Benchlist1 = string.Empty;
        public string benchMarkActualValue = string.Empty;
        public string[] complist, filt, Benchlist;
        public string complist1 = string.Empty;
        public double shopperBenchValue;
        public double tripsBenchValue;
        public string texboxvalue;
        public List<object> sampleSizelist;
        public List<string> beveragelist;
        public string samplesizeNames;
        public string complistNames;
        public string ChannelRetailersVisited = string.Empty;
        public string ShopperSegment = string.Empty;
        public string ComparisonPointsBanner = string.Empty;

        public List<int> ExceptionSlideList = new List<int>();
        public void InitializeAsposePresentationFile(string _fileName)
        {
            //pres = new Presentation(_fileName);
            Aspose.Slides.License license = new Aspose.Slides.License();
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            //slds = pres.Slides;
        }
        public async Task<string> PrepareSARReport(string filepath, List<string> spNames, HttpContextBase context, FilterPanelInfo filter)
        {
            string destinationtemplate = "~/Temp/PPT/ExportedReportPPT" + context.Session.SessionID + ".pptx";
            int slideId = 5;
            DataSet ds = new DataSet();
            //DataTable result;
            System.Data.DataTable copytable;
            int spIncrmt = 0;
            InitializeAsposePresentationFile(filepath);
            #region using name custom or custom1 and custom2
            Dictionary<int, bool> isCustomName = new Dictionary<int, bool>();
            for(int i = 1; i <= 50; i++)
            {
                isCustomName.Add(i, true);
            }
            isCustomName[10] = false;
            isCustomName[11] = false;
            isCustomName[12] = false;
            isCustomName[13] = false;
            isCustomName[15] = false;
            isCustomName[16] = false;
            isCustomName[17] = false;
            isCustomName[18] = false;
            isCustomName[19] = false;
            isCustomName[21] = false;
            isCustomName[22] = false;
            isCustomName[23] = false;
            isCustomName[24] = false;
            isCustomName[25] = false;
            isCustomName[26] = false;
            isCustomName[27] = false;
            isCustomName[28] = false;
            isCustomName[29] = false;
            isCustomName[30] = false;
            isCustomName[31] = false;
            isCustomName[32] = false;
            isCustomName[33] = false;
            isCustomName[34] = false;
            isCustomName[35] = false;
            isCustomName[36] = false;
            isCustomName[37] = false;
            isCustomName[38] = false;
            isCustomName[39] = false;
            isCustomName[40] = false;
            isCustomName[41] = false;
            #endregion

            //Slides
            using (Presentation pres = new Presentation(filepath))
            {
                await Task.Run(async () =>
                {
                    foreach (var spName in spNames)
                    {
                        slideId++;
                        if (!string.IsNullOrEmpty(spName))
                        {

                            var result = await Task.Run(() => GetTableOutputPPT(filter, spName,isCustomName[slideId],slideId,context));
                            if (result != null)
                            {
                                result.TableName = result.TableName + spIncrmt;
                                copytable = result.Copy();
                                ds.Tables.Add(copytable);
                            }

                            await Task.Run(() =>
                            {
                                if (result != null)
                                {
                                    try
                                    {
                                        prepareSARportPPT(pres, filter, result, filepath, destinationtemplate, context, slideId);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorLogSar.LogError(ex.Message + "   when calling slide   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                                    }
                                }

                                spIncrmt++;
                            });
                            //    prepareSARportPPT(pres, filter, ds.Tables[spIncrmt], filepath, destinationtemplate, context, slideId);
                            //    spIncrmt++;
                            //});
                        }
                    }
                });
                SortedSet<int> slidesToDelete = new SortedSet<int>();
                //for(int i = 0; i < ExceptionSlideList.Count; i++)
                //{
                //    slidesToDelete.Add(ExceptionSlideList[i]);
                //}

                for(int i= pres.Slides.Count - 1; i >= 41; i--)
                {
                    pres.Slides.Remove(pres.Slides[i]);
                }
                if ((bool)filter.IsRetailerMSS==true)
                {
                    for(int i = 37; i >= 31; i--)
                    {
                        slidesToDelete.Add(i);
                    }
                    slidesToDelete.Add(22);
                    for(int i = 19; i >= 14; i--)
                    {
                        slidesToDelete.Add(i);
                    }
                    if ((int)filter.IsTripsOrShopper == 1)
                    {
                        for(int i = 13; i >= 10; i--)
                        {
                            slidesToDelete.Add(i);
                        }
                    }
                }
                if((bool)filter.IsChannelSelected == true|| (bool)filter.IsTripFilter == true || (bool)filter.IsNonPrioritySelected == true || (bool)filter.corporateOrChannelNetSelected == true)
                {
                    for(int i = 41; i >= 39; i--)
                    {
                        slidesToDelete.Add(i);
                    }
                    for(int i = 30; i >= 23; i--)
                    {
                        slidesToDelete.Add(i);
                    }
                    if((bool)filter.IsTripFilter == true || (bool)filter.IsNonPrioritySelected == true)
                    {
                        for (int i = 22; i >= 20; i--)
                        {
                            slidesToDelete.Add(i);
                        }
                        slidesToDelete.Add(13);
                    }

                }
                for (int i = pres.Slides.Count - 1; i >= 0; i--)
                {
                    if (slidesToDelete.Contains(i + 1))
                    {
                        pres.Slides.Remove(pres.Slides[i]);
                    }
                }
                //if (!selectionList[0].IsImageries)
                //{
                //    for (int i = pres.Slides.Count - 1; i >= 22; i--)
                //    {
                //        pres.Slides.Remove(pres.Slides[i]);
                //    }
                //}

                //if (selectionList[0].IsChannelSelected)
                //{
                //    for (int i = pres.Slides.Count - 1; i >= 20; i--)
                //    {
                //        pres.Slides.Remove(pres.Slides[i]);
                //    }
                //}
                //else
                //{
                //    //for (int i = pres.Slides.Count - 1; i >= 23; i--)
                //    //{
                //    //    pres.Slides.Remove(pres.Slides[i]);
                //    //}
                //}
                ////Added By SABAT ULLAH
                //if (selectionList[0].IsFromMSS)
                //{
                //    pres.Slides[19].Remove();
                //    pres.Slides[18].Remove();
                //    pres.Slides[17].Remove();
                //    pres.Slides[16].Remove();
                //    pres.Slides[15].Remove();
                //}

                //if (selectionList[0].DeleteTripsandGuestsSlide == true || selectionList[0].IsFromMSS == true)
                //{
                //    pres.Slides[14].Remove();
                //}

                ////Added By SABAT ULLAH
                //if (selectionList[0].IsFromMSS && selectionList[0].IsVisits)
                //{
                //    pres.Slides[9].Remove();
                //    pres.Slides[8].Remove();
                //    pres.Slides[7].Remove();
                //    pres.Slides[6].Remove();
                //    pres.Slides[5].Remove();
                //    pres.Slides[4].Remove();
                //}
                pres.Save(context.Server.MapPath(destinationtemplate), AsposeSlide.Export.SaveFormat.Pptx);
            }
            return "";
        }

        private void prepareSARportPPT(Presentation pres, FilterPanelInfo filter, System.Data.DataTable dataTable, string filepath, string destinationtemplate, HttpContextBase context, int slideId)
        {



            string subPath = "~/Images/temp/" + context.Session.SessionID + getUniqueConst();
            IGroupShape tempGroup;
            Aspose.Slides.Charts.IChart tempChart;
            PictureFrame tempImg;
            IAutoShape tempShape;
            string loc="";
            //local variables
            double point1 = 0.0, point2 = 0.0, point3 = 0.0, point4 = 0.0;
            double metricPercentage = 0.0;
            dynamic tempRow = null;
            string footerFreqName = "";
            int frequencyIdIndex = 5;
            string[] custombase = new string[1];
            System.Data.DataTable tbl;

            string cleanedBenchmark = getCleanedRetailerForImage(filter.BenchMark.Name, "-");



            #region selection Summary
            string selectedEstName = "";
            string selectedTime = "";
            //Added by Sabat Ullah
            //string timePeriod = (from r in result.AsEnumerable()
            //                     select r.Field<string>("TimePeriod")).FirstOrDefault();
            //string footerNote = (from r in result.AsEnumerable()
            //                     select r.Field<string>("FooterNote")).FirstOrDefault();


            //string footerNote2 = (from r in result.AsEnumerable()
            //                      select r.Field<string>("ReadAsText")).FirstOrDefault();

            #endregion
            //string Filters = (selectionList[0].Filters == null) ? "NONE" : selectionList[0].Filters;
            switch (slideId)
            {
                case 6:
                    try
                    {
                        #region slide 1
                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[0];
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "retailerTextBox")).TextFrame.Text = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "retailerTextBox")).TextFrame.Text, "_retailer_", filter.BenchMark.Name);
                        imageReplace((PictureFrame)pres.Masters[3].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, 100, subPath);
                        updateConnector((IConnector)pres.Masters[3].Shapes.Where(x => x.Name == "upperBorder").FirstOrDefault(), filter.ColorCode, "solid");
                        updateConnector((IConnector)pres.Masters[3].Shapes.Where(x => x.Name == "lowerBorder").FirstOrDefault(), filter.ColorCode, "gradient");
                        #endregion
                        #region slide 2
                        //imageReplace((PictureFrame)pres.Slides[1].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                        #region slide 3
                        //imageReplace((PictureFrame)pres.Slides[2].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                        #region slide 4
                        cur_Slide = pres.Slides[3];
                        IGroupShape globe1 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe1");
                        globe1.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        IGroupShape globe2 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe2");
                        globe2.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[3].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                        #region slide 5
                        cur_Slide = pres.Slides[4];
                        globe1 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe1");
                        globe1.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[4].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                        #region slide 6
                        cur_Slide = pres.Slides[5];
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingRectangleTextbox")).TextFrame.Text = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingRectangleTextbox")).TextFrame.Text, "_retailer_", filter.BenchMark.Name);
                        IGroupShape grpShape =(IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headerGroup");
                        ((IAutoShape)grpShape.Shapes.FirstOrDefault(x => x.Name == "headerText")).TextFrame.Text = filter.BenchMark.Name;
                        prepareSlideSix(dataTable, cur_Slide,filter.BenchMark.Name);
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //imageReplace((PictureFrame)pres.Slides[5].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message+"   --- Inside Slide "+slideId+" SARReport", ex.StackTrace,context);
                    }
                    break;
                case 7:
                    try
                    {

                        #region slide 7

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[6];
                        barWithLineChart(dataTable, cur_Slide, "barChart", false,filter.ColorCode, context, slideId, subPath, true);
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gainsIcon"), filter.ColorCode);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "lossIcon"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[6].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 8:
                    try
                    {

                        #region slide 8

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[7];
                        barWithLineChart(dataTable, cur_Slide, "barChart", true, filter.ColorCode, context, slideId, subPath, true);
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(((IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gains")).Shapes.FirstOrDefault(x => x.Name == "retailerImage"), filter.ColorCode);
                        //updateIconColor(((IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "loss")).Shapes.FirstOrDefault(x => x.Name == "retailerImage"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[7].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 10:
                    try
                    {
                        #region slide 9
                        cur_Slide = pres.Slides[8];
                        var globe1 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe1");
                        globe1.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        #endregion

                        #region slide 10

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[9];

                        #region gender
                        IGroupShape grpGender = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gender");
                        updateConnector((IConnector)grpGender.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart_gender")), filter.ColorCode);
                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Gender", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        tempRow = (from r in dataTable.AsEnumerable()
                                   where Convert.ToString(r.Field<object>("MetricType")).Equals("Gender", StringComparison.OrdinalIgnoreCase)
                                   orderby r["MetricPercentage"] descending
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).FirstOrDefault();
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "genderHeading")).TextFrame.Text = tempRow.MetricName;
                        var gender_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "genderTable").FirstOrDefault();
                        gender_tbl[0, 0].TextFrame.Text = gender_tbl[0, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        gender_tbl[1, 0].TextFrame.Text = gender_tbl[1, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        tempChart = (IChart)grpGender.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                        if (!double.TryParse(tempRow.MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                        point1 = metricPercentage;
                        point2 = 1 - point1;
                        ((IAutoShape)grpGender.Shapes.FirstOrDefault(x => x.Name == "donutLegend")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                        updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);
                        tbl = query.CopyToDataTable();

                        updateSARportIndexTable(cur_Slide, tbl, "genderTable", "Metric", -1, -1, 0, 1, 1, 0);
                        #endregion

                        #region age
                        IGroupShape grpAge = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ageGroup");
                        updateConnector((IConnector)grpAge.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart_age")), filter.ColorCode);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ageIndex1")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ageIndex1")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ageIndex2")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ageIndex2")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Age", StringComparison.OrdinalIgnoreCase));

                        tbl = query.CopyToDataTable();

                        updateSARportIndexTable(cur_Slide, tbl, "ageTable", "Metric", 1, 0, 2, 3, 0, 0);
                        #endregion

                        #region ethnicity
                        IGroupShape grpEthnicity = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ethnicity");
                        updateConnector((IConnector)grpEthnicity.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart_ethnicity")), filter.ColorCode);

                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ethnicityIndex1")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ethnicityIndex1")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ethnicityIndex2")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ethnicityIndex2")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);

                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Ethnicity", StringComparison.OrdinalIgnoreCase));
                        tbl = query.CopyToDataTable();

                        updateSARportIndexTable(cur_Slide, tbl, "ethnicityTable", "Metric", 1, 0, 2, 3, 0, 0);
                        #endregion

                        #region hhincome
                        IGroupShape grpHh = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "hhIncome");
                        updateConnector((IConnector)grpHh.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart_hhincome")), filter.ColorCode);
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("HH income", StringComparison.OrdinalIgnoreCase));
                        tempRow = (from r in dataTable.AsEnumerable()
                                   where Convert.ToString(r.Field<object>("MetricType")).Equals("HH income", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("Less than $25,000", StringComparison.OrdinalIgnoreCase)
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).FirstOrDefault();
                        tempChart = (IChart)grpHh.Shapes.Where(x => x.Name == "donutChart1").FirstOrDefault();
                        if (!double.TryParse(tempRow.MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                        point1 = metricPercentage;
                        point2 = 1 - point1;
                        ((IAutoShape)grpHh.Shapes.FirstOrDefault(x => x.Name == "donutLegend1")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                        updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);
                        tempRow = (from r in dataTable.AsEnumerable()
                                   where Convert.ToString(r.Field<object>("MetricType")).Equals("HH income", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("$75,000+", StringComparison.OrdinalIgnoreCase)
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).FirstOrDefault();
                        var hhp_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "hhTable").FirstOrDefault();
                        hhp_tbl[0, 0].TextFrame.Text = hhp_tbl[0, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        hhp_tbl[1, 0].TextFrame.Text = hhp_tbl[1, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        tempChart = (IChart)grpHh.Shapes.Where(x => x.Name == "donutChart2").FirstOrDefault();
                        if (!double.TryParse(tempRow.MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                        point1 = metricPercentage;
                        point2 = 1 - point1;
                        ((IAutoShape)grpHh.Shapes.FirstOrDefault(x => x.Name == "donutLegend2")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                        updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);
                        tbl = query.CopyToDataTable();

                        updateSARportIndexTable(cur_Slide, tbl, "hhTable", "Metric", -1, -1, 0, 1, 1, 0);
                        #endregion

                        #region parentalidentification
                        IGroupShape grpParent = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "parentOfChild");
                        updateConnector((IConnector)grpParent.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart_parent")), filter.ColorCode);
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Parental Identification", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        tempRow = (from r in dataTable.AsEnumerable()
                                   where Convert.ToString(r.Field<object>("MetricType")).Equals("Parental Identification", StringComparison.OrdinalIgnoreCase)
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).FirstOrDefault();
                        var pc_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "pcTable").FirstOrDefault();
                        pc_tbl[0, 0].TextFrame.Text = pc_tbl[0, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        pc_tbl[1, 0].TextFrame.Text = pc_tbl[1, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        tempChart = (IChart)grpParent.Shapes.Where(x => x.Name == "donutChart2").FirstOrDefault();
                        if (!double.TryParse(tempRow.MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                        point1 = metricPercentage;
                        point2 = 1 - point1;


                        ((IAutoShape)grpParent.Shapes.FirstOrDefault(x => x.Name == "donutLegend2")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                        updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);
                        tbl = query.CopyToDataTable();
                        updateSARportIndexTable(cur_Slide, tbl, "pcTable", "Metric", -1, -1, 0, 1, 1, 0);

                        #endregion

                        #region employement
                        IGroupShape grpstudents = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "students");
                        updateConnector((IConnector)grpstudents.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart_student")), filter.ColorCode);

                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "studentIndex1")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "studentIndex1")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "studentIndex2")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "studentIndex2")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);

                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Employment Status", StringComparison.OrdinalIgnoreCase));
                        tbl = query.CopyToDataTable();

                        updateSARportIndexTable(cur_Slide, tbl, "studentTable", "Metric", 1, 0, 2, 3, 0, 0);
                        #endregion

                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 11:
                    try
                    {

                        #region slide 11

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[10];


                        #region shopperSegment
                        IGroupShape grpShopper = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "shopperSegment");
                        updateConnector((IConnector)grpShopper.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Attitudinal Segment", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(6);
                        var tempNew = (from r in query.AsEnumerable()
                                       select new
                                       {
                                           MetricName = r["Metric"].ToString(),
                                           CustomBaseName1 = r["CB1"].ToString(),
                                           CustomBaseName2 = r["CB2"].ToString(),
                                           CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                           CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                           MetricPercentage = r["MetricPercentage"].ToString(),
                                           CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                           CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                       }).ToList();
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "segmentIndex1")).TextFrame.Text = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "segmentIndex1")).TextFrame.Text, "_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "segmentIndex2")).TextFrame.Text = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "segmentIndex2")).TextFrame.Text, "_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        for (int i = 0; i < tempNew.Count; i++)
                        {
                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart" + (i + 1))), filter.ColorCode);
                            metricPercentage = 0.0;
                            double sign1 = 0.0, sign2 = 0;

                            string tempMetricName = getCleanedRetailerForImage(tempNew[i].MetricName, "-");
                            string tempLoc = "~/Images/P2PSarReportIcons/" + tempMetricName + ".svg";

                            IGroupShape shopperItem = (IGroupShape)grpShopper.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());
                            ((IAutoShape)shopperItem.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempNew[i].MetricName;
                            IGroupShape chart = (IGroupShape)shopperItem.Shapes.FirstOrDefault(x => x.Name == "chart");
                            tempChart = (IChart)chart.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            if (!double.TryParse(tempNew[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
                            point1 = metricPercentage;
                            point2 = 1 - point1;
                            ((IAutoShape)chart.Shapes.FirstOrDefault(x => x.Name == "legend")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart" + (i + 1))), filter.ColorCode);
                            imageReplace((PictureFrame)shopperItem.Shapes.Where(x => x.Name == "icon").FirstOrDefault(), tempLoc, context, 400 + slideId + (200 + i), subPath);

                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);
                            if (!double.TryParse(Convert.ToString(tempNew[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempNew[i].CB2Sig), out sign2)) sign2 = 0.0;
                            //attitudeIndexTable1[j, k].TextFrame.Text = query.CustomBaseIndex2;
                            //aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            //aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }
                        updateSARportIndexTableRow(cur_Slide, query.CopyToDataTable(), "segmentIndexTable1", "Metric", -1, -1, 0, -1, 0, 0);
                        updateSARportIndexTableRow(cur_Slide, query.CopyToDataTable(), "segmentIndexTable2", "Metric", -1, -1, -1, 0, 0, 0);
                        #endregion

                        #region primaryShopper
                        /*IGroupShape grpPrimary = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "primaryShopper");
                        updateConnector((IConnector)grpPrimary.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Primary Shopper", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("Yes", StringComparison.OrdinalIgnoreCase));
                        tempRow = (from r in query.AsEnumerable()
                                       select new
                                       {
                                           MetricName = r["Metric"].ToString(),
                                           CustomBaseName1 = r["CB1"].ToString(),
                                           CustomBaseName2 = r["CB2"].ToString(),
                                           CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                           CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                           MetricPercentage = r["MetricPercentage"].ToString(),
                                           CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                           CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                       }).ToList();
                        var primary_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "primaryShopperTable").FirstOrDefault();
                        primary_tbl[0, 0].TextFrame.Text = primary_tbl[0, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        primary_tbl[1, 0].TextFrame.Text = primary_tbl[1, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        var primaryChart = (IGroupShape)grpPrimary.Shapes.Where(x => x.Name == "chart").FirstOrDefault();
                        tempChart = (IChart)primaryChart.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                        metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
                        point1 = metricPercentage;
                        point2 = 1 - point1;
                        ((IAutoShape)primaryChart.Shapes.FirstOrDefault(x => x.Name == "legend")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                        updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);
                        tbl = query.CopyToDataTable();

                        updateSARportIndexTable(cur_Slide, tbl, "primaryShopperTable", "Metric", -1, -1, 0, 1, 1, 0);*/
                        #endregion

                        #region hh shopper
                        IGroupShape grpPrimary = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "primaryShopper");
                        updateConnector((IConnector)grpPrimary.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        var sortOrder = new Dictionary<string, int>{
                                                                      { "0%-25%", 1 },
                                                                      { "26%-50%", 2 },
                                                                      { "51%-70%", 3 },
                                                                      { "71%-100%", 4 }
                                                                    };
                        var defaultOrder = sortOrder.Max(x => x.Value) + 1;
                        var order = 1;
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("% HH Shopping Personally Responsible For", StringComparison.OrdinalIgnoreCase)).OrderBy(r => sortOrder.TryGetValue(Convert.ToString(r.Field<object>("Metric")), out order) ? order : defaultOrder);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        var primary_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "primaryShopperTable").FirstOrDefault();
                        primary_tbl[1, 0].TextFrame.Text = primary_tbl[1, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        primary_tbl[2, 0].TextFrame.Text = primary_tbl[2, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        //var primaryChart = (IGroupShape)grpPrimary.Shapes.Where(x => x.Name == "chart").FirstOrDefault();
                        tempChart = (IChart)grpPrimary.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                        //if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                        //metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
                        //point1 = metricPercentage;
                        //point2 = 1 - point1;
                        tbl = query.CopyToDataTable();
                        int alphaValue = ColorTranslator.FromHtml(filter.ColorCode).A;
                        List<Color> colorList = new List<Color>();
                        colorList.Add(ColorTranslator.FromHtml(filter.ColorCode));
                        colorList.Add(Color.FromArgb(alphaValue - 75, ColorTranslator.FromHtml(filter.ColorCode)));
                        colorList.Add(Color.FromArgb(alphaValue - 150, ColorTranslator.FromHtml(filter.ColorCode)));
                        colorList.Add(Color.FromArgb(alphaValue - 225, ColorTranslator.FromHtml(filter.ColorCode)));
                        grpPrimary.Shapes.Where(x => x.Name == "legend1").FirstOrDefault().FillFormat.SolidFillColor.Color = colorList[0];
                        grpPrimary.Shapes.Where(x => x.Name == "legend2").FirstOrDefault().FillFormat.SolidFillColor.Color = colorList[1];
                        grpPrimary.Shapes.Where(x => x.Name == "legend3").FirstOrDefault().FillFormat.SolidFillColor.Color = colorList[2];
                        grpPrimary.Shapes.Where(x => x.Name == "legend4").FirstOrDefault().FillFormat.SolidFillColor.Color = colorList[3];
                        updateDonutfourPointsWithLabel(tempChart, tbl, colorList);


                        updateSARportIndexTable(cur_Slide, tbl, "primaryShopperTable", "Metric", -1, -1, 1, 2, 1, 1);
                        #endregion



                        #region Device Ownership
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "deviceConnector").FirstOrDefault(), filter.ColorCode, "gradient");

                        query = dataTable
                                       .AsEnumerable()
                                       .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Device Ownership", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(6);
                        tempNew = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        var device_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "deviceTable").FirstOrDefault();
                        device_tbl[0, 1].TextFrame.Text = filter.BenchMark.Name;
                        device_tbl[0, 2].TextFrame.Text = device_tbl[0, 2].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        device_tbl[0, 3].TextFrame.Text = device_tbl[0, 3].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);

                        for (int i = 0; i < tempNew.Count; i++)
                        {
                            device_tbl[(i + 1), 0].TextFrame.Text = tempNew[i].MetricName;
                        }

                        tbl = query.CopyToDataTable();
                        updateSARportIndexTableRow(cur_Slide, tbl, "deviceTable", "Metric", -1, 1, 2, 3, 1, 1);



                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        #endregion

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 12:
                    try
                    {
                        #region slide 12

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[11];

                        #region attitudenal
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "attitudeConnector").FirstOrDefault(), filter.ColorCode, "gradient");
                        var attitude_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "attitudeDataTable").FirstOrDefault();
                        attitude_tbl[0, 0].TextFrame.Text = filter.BenchMark.Name;
                        attitude_tbl[1, 0].TextFrame.Text = attitude_tbl[1, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        attitude_tbl[2, 0].TextFrame.Text = attitude_tbl[2, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Attitudinal Statements - Top 2 Box", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(10);
                        updateStackBarChartData(query.CopyToDataTable(), cur_Slide, "attitudeBarChart", filter.ColorCode);
                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "attitudeDataTable", "Metric", -1, 0, 1, 2, 1, 0);
                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "attitudeNameTable", "Metric", 0, -1, -1, -1, 0, 0);
                        #endregion

                        #region kohorts
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "kohortsConnector").FirstOrDefault(), filter.ColorCode, "gradient");
                        var kohorts_tbl = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "kohortsDataTable").FirstOrDefault();
                        kohorts_tbl[0, 0].TextFrame.Text = filter.BenchMark.Name;
                        kohorts_tbl[1, 0].TextFrame.Text = kohorts_tbl[1, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        kohorts_tbl[2, 0].TextFrame.Text = kohorts_tbl[2, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("KOHORTS", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage"));
                        updateStackBarChartData(query.CopyToDataTable(), cur_Slide, "kohortsBarChart", filter.ColorCode);
                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "kohortsDataTable", "Metric", -1, 0, 1, 2, 1, 0);
                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "kohortsNameTable", "Metric", 0, -1, -1, -1, 0, 0);
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        #endregion

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 13:
                    try
                    {

                        #region slide 13

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[12];

                        #region channels
                        ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "Title Slide").FirstOrDefault()).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "Title Slide").FirstOrDefault()).TextFrame.Text.Replace("_frequency_", filter.Frequency[0].Name.ToUpper());
                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("IsChannel")).Equals("1", StringComparison.OrdinalIgnoreCase) && !Convert.ToString(r.Field<object>("Metric")).Equals("Online Grocery", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5);
                        var onlineGrocery = dataTable
                                          .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("IsChannel")).Equals("1", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("Online Grocery", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        var onlineGroceryItems = new { MetricName = onlineGrocery["Metric"].ToString(), MetricPercentage = onlineGrocery["MetricPercentage"].ToString() };
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                   }).ToList();
                        tempRow.Add(onlineGroceryItems);
                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            string tempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string tempLoc = "~/Images/P2PSarReportIcons/" + tempMetricName + ".svg"; ;
                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("header_chart" + (i + 1))), filter.ColorCode);

                            var channelGroup = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == ("channel" + (i + 1))).FirstOrDefault();
                            channelGroup.Shapes.Where(x => x.Name == "icon").FirstOrDefault().Hidden = false;
                            if (tempRow[i].MetricName != "Online Grocery")
                            {
                                imageReplace((PictureFrame)channelGroup.Shapes.Where(x => x.Name == "icon").FirstOrDefault(), tempLoc, context, 1000 + (i + 1), subPath);
                            }

                            double metricPercent = 0.0;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercent))
                                metricPercent = 0.0;
                            ((IAutoShape)channelGroup.Shapes.Where(x => x.Name == "metricPercentage").FirstOrDefault()).TextFrame.Text = Convert.ToDouble(metricPercent * 100).ToString("#0.0") + "%";
                            ((IAutoShape)channelGroup.Shapes.Where(x => x.Name == "text").FirstOrDefault()).TextFrame.Text = tempRow[i].MetricName;
                        }
                        var highestMetricPercentage = (Convert.ToDouble(tempRow[1].MetricPercentage) * 100).ToString("#0.0") + "%";
                        var mostVisitedMetric = tempRow[1].MetricName;
                        var channelHeader = ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "channelTextbox").FirstOrDefault()).TextFrame.Text;
                        channelHeader = channelHeader.Replace("_metric_", highestMetricPercentage).Replace("_selRetailer_", filter.BenchMark.Name).Replace("_frequency_", filter.Frequency[0].Name).Replace("_channel_", mostVisitedMetric);
                        ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "channelTextbox").FirstOrDefault()).TextFrame.Text = channelHeader;



                        #endregion

                        #region retailer
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("IsChannel")).Equals("0", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(10);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                   }).ToList();
                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            var retailerGroup = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == ("retailer" + (i + 1))).FirstOrDefault();
                            double metricPercent = 0.0;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercent))
                                metricPercent = 0.0;
                            ((IAutoShape)retailerGroup.Shapes.Where(x => x.Name == "metricPercentage").FirstOrDefault()).TextFrame.Text = Convert.ToDouble(metricPercent * 100).ToString("#0.0") + "%";
                            ((IAutoShape)retailerGroup.Shapes.Where(x => x.Name == "text").FirstOrDefault()).TextFrame.Text = tempRow[i].MetricName;
                            string tempIconName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            loc = "~/Images/P2PSarReport/" + tempIconName + ".svg";

                            imageReplace((PictureFrame)retailerGroup.Shapes.Where(x => x.Name == "icon").FirstOrDefault(), loc, context, slideId + (1000 + i), subPath);

                        }
                        highestMetricPercentage = (Convert.ToDouble(tempRow[1].MetricPercentage) * 100).ToString("#0.0") + "%";
                        mostVisitedMetric = tempRow[1].MetricName;
                        var retailerHeader = ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "retailerTextbox").FirstOrDefault()).TextFrame.Text;
                        retailerHeader = retailerHeader.Replace("_metric_", highestMetricPercentage).Replace("_selRetailer_", filter.BenchMark.Name).Replace("_frequency_", filter.Frequency[0].Name).Replace("_retailer_", mostVisitedMetric);
                        ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "retailerTextbox").FirstOrDefault()).TextFrame.Text = retailerHeader;

                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        #endregion

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 15:
                    try
                    {
                        #region slide 14
                        cur_Slide = pres.Slides[13];
                        var globe1 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe1");
                        globe1.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        #endregion


                        #region slide 15

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";

                        cur_Slide = pres.Slides[14];
                        string headerSlide = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "leftPane")).TextFrame.Text;
                        headerSlide = replaceText(headerSlide, "_retailer_", filter.BenchMark.Name.ToUpper());
                        headerSlide = replaceText(headerSlide, "_timeperiod_", filter.TimeperiodType);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "leftPane")).TextFrame.Text = headerSlide;
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        imageReplace((PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "RetailerImg").FirstOrDefault(), loc, context, slideId + (500), subPath);


                        prepareSlide15(dataTable, cur_Slide, filter, context, subPath);
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        string statTestString = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "StatTestAgainst")).TextFrame.Text;
                        statTestString = replaceText(statTestString, "_custom1_", filter.CustomBase[0].Name.ToUpper());
                        statTestString = replaceText(statTestString, "_filter_", filterText);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "StatTestAgainst")).TextFrame.Text = statTestString;

                        string timeperiodAndFilter = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "TPandFilters")).TextFrame.Text;
                        timeperiodAndFilter = replaceText(timeperiodAndFilter, "_timeperiod_", filter.TimeperiodType);
                        timeperiodAndFilter = replaceText(timeperiodAndFilter, "_filter_", filterText);

                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "TPandFilters")).TextFrame.Text = timeperiodAndFilter;

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 16:
                    try
                    {

                        #region slide 16

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[15];

                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "topConnector").FirstOrDefault(), filter.ColorCode, "gradient");

                        #region location Prior
                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Location Prior", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;

                        var locationCustomTable1 = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "locationPriorCustom1").FirstOrDefault();
                        locationCustomTable1[0, 0].TextFrame.Text = locationCustomTable1[0, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                        var locationCustomTable2 = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "locationPriorCustom2").FirstOrDefault();
                        locationCustomTable2[0, 0].TextFrame.Text = locationCustomTable2[0, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);

                        var locationGroup = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == ("locationPriorGrp")).FirstOrDefault();
                        ((IAutoShape)locationGroup.Shapes.FirstOrDefault(x => x.Name == "metric")).TextFrame.Text = tempRow[0].MetricName;

                        string LoctempMetricName = getCleanedRetailerForImage(tempRow[0].MetricName, "-");
                        string LoctempLoc = "~/Images/P2PSarReportIcons/" + LoctempMetricName + ".svg";
                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("locationPriorChart")), filter.ColorCode);
                        imageReplace((PictureFrame)locationGroup.Shapes.Where(x => x.Name == "img").FirstOrDefault(), LoctempLoc, context, 2000 + slideId + (100), subPath);

                        ((IAutoShape)locationGroup.Shapes.FirstOrDefault(x => x.Name == "metricPercentage")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "locationPriorCustom1", "Metric", -1, -1, 0, -1, 1, 0);
                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "locationPriorCustom2", "Metric", -1, -1, -1, 0, 1, 0);



                        #endregion


                        #region TripMission
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("TripMission", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(3);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;

                            string tempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string tempLoc = "~/Images/P2PSarReportIcons/" + tempMetricName + ".svg";


                            var tripMissionCustomTable1 = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "tripMission" + (i + 1) + "Custom1").FirstOrDefault();
                            tripMissionCustomTable1[0, 0].TextFrame.Text = tripMissionCustomTable1[0, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            var tripMissionCustomTable2 = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "tripMission" + (i + 1) + "Custom2").FirstOrDefault();
                            tripMissionCustomTable2[0, 0].TextFrame.Text = tripMissionCustomTable2[0, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);

                            var tripMissionGroup = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == ("tripMission" + (i + 1))).FirstOrDefault();
                            ((IAutoShape)tripMissionGroup.Shapes.FirstOrDefault(x => x.Name == "metric")).TextFrame.Text = tempRow[i].MetricName;
                            ((IAutoShape)tripMissionGroup.Shapes.FirstOrDefault(x => x.Name == "metricPercentage")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("tripMissionChart" + (i + 1))), filter.ColorCode);
                            imageReplace((PictureFrame)tripMissionGroup.Shapes.Where(x => x.Name == "img").FirstOrDefault(), tempLoc, context, 2000 + slideId + (200 + i), subPath);

                            var tempQuery = query
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("Metric")).Equals(tempRow[i].MetricName, StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                            updateSARportIndexTable(cur_Slide, tempQuery.CopyToDataTable(), "tripMission" + (i + 1) + "Custom1", "Metric", -1, -1, 0, -1, 1, 0);
                            updateSARportIndexTable(cur_Slide, tempQuery.CopyToDataTable(), "tripMission" + (i + 1) + "Custom2", "Metric", -1, -1, -1, 0, 1, 0);
                        }



                        #endregion



                        #region Planning
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Planning", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                        point1 = metricPercentage;
                        point2 = 1 - point1;

                        var planningCustomTable1 = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "planningCustom1").FirstOrDefault();
                        planningCustomTable1[0, 0].TextFrame.Text = planningCustomTable1[0, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                        var planningCustomTable2 = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "planningCustom2").FirstOrDefault();
                        planningCustomTable2[0, 0].TextFrame.Text = planningCustomTable2[0, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);

                        var planningGroup = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == ("planningGrp")).FirstOrDefault();
                        ((IAutoShape)planningGroup.Shapes.FirstOrDefault(x => x.Name == "metric")).TextFrame.Text = tempRow[0].MetricName;
                        ((IAutoShape)planningGroup.Shapes.FirstOrDefault(x => x.Name == "metricPercentage")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                        tempChart = (IChart)planningGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                        updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "planningCustom1", "Metric", -1, -1, 0, -1, 1, 0);
                        updateSARportIndexTable(cur_Slide, query.CopyToDataTable(), "planningCustom2", "Metric", -1, -1, -1, 0, 1, 0);



                        #endregion


                        #region Preparation Types
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "preparationConnector").FirstOrDefault(), filter.ColorCode, "gradient");

                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Preparation Types", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(3);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;

                        var preparationItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "preparationItems");

                        ((IAutoShape)preparationItems.Shapes.FirstOrDefault(x => x.Name == "preparationCustom1")).TextFrame.Text = ((IAutoShape)preparationItems.Shapes.FirstOrDefault(x => x.Name == "preparationCustom1")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        ((IAutoShape)preparationItems.Shapes.FirstOrDefault(x => x.Name == "preparationCustom2")).TextFrame.Text = ((IAutoShape)preparationItems.Shapes.FirstOrDefault(x => x.Name == "preparationCustom2")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;

                            var preparationItem = (IGroupShape)preparationItems.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());
                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "metric")).TextFrame.Text = tempRow[i].MetricName;
                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "metricPercentage")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)preparationItem.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion



                        #region Destination Items
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "destinationConnector").FirstOrDefault(), filter.ColorCode, "gradient");

                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("All Destination Items", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(3);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();

                        var destinationItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "destinationItems");

                        ((IAutoShape)destinationItems.Shapes.FirstOrDefault(x => x.Name == "destinationCustom1")).TextFrame.Text = ((IAutoShape)destinationItems.Shapes.FirstOrDefault(x => x.Name == "destinationCustom1")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        ((IAutoShape)destinationItems.Shapes.FirstOrDefault(x => x.Name == "destinationCustom2")).TextFrame.Text = ((IAutoShape)destinationItems.Shapes.FirstOrDefault(x => x.Name == "destinationCustom2")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;

                            var destinationItem = (IGroupShape)destinationItems.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());
                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "metric")).TextFrame.Text = tempRow[i].MetricName;
                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "metricPercentage")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[1].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 17:
                    try
                    {

                        #region slide 17

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[16];



                        #region Reason Store Segments
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "reasonSegmentConnector2").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "reasonSegmentConnector1").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Reasons For Store Choice Segment", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;

                        var reasonSegments = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "reasonSegments");



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            var reasonSegm = (IGroupShape)reasonSegments.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());

                            string tempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string tempLoc = "~/Images/P2PSarReportIcons/" + tempMetricName + ".svg";


                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "custom1Index")).TextFrame.Text = ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "custom1Index")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "custom2Index")).TextFrame.Text = ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "custom2Index")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "metric")).TextFrame.Text = tempRow[i].MetricName;

                            var chartGroup = (IGroupShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";


                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("reasonSegmchart" + (i + 1))), filter.ColorCode);
                            imageReplace((PictureFrame)reasonSegm.Shapes.Where(x => x.Name == "img").FirstOrDefault(), tempLoc, context, 2000 + slideId + (200 + i), subPath);


                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)reasonSegm.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion



                        #region Reason Store 
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "reasonStoreConnector1").FirstOrDefault(), filter.ColorCode, "gradient");

                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Top 5 Reasons For Store Choice", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;

                        var reasonStore = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "reasonStore");



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            var reasonSt = (IGroupShape)reasonStore.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());

                            string tempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string tempLoc = "~/Images/P2PSarReportIcons/" + tempMetricName + ".svg";


                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "custom1Index")).TextFrame.Text = ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "custom1Index")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "custom2Index")).TextFrame.Text = ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "custom2Index")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "metric")).TextFrame.Text = tempRow[i].MetricName;

                            var chartGroup = (IGroupShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("reasonStorechart" + (i + 1))), filter.ColorCode);
                            imageReplace((PictureFrame)reasonSt.Shapes.Where(x => x.Name == "img").FirstOrDefault(), tempLoc, context, 2000 + slideId + (250 + i), subPath);

                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "index1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)reasonSt.Shapes.FirstOrDefault(x => x.Name == "index2")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion



                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[1].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 18:
                    try
                    {

                        #region slide 18

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[17];



                        #region Destination Items
                        var destinationItem = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "destinationItem");
                        updateConnector((IConnector)destinationItem.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Items Purchased", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("NARTD", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        var nartdRow = (from r in query.AsEnumerable()
                                        select new
                                        {
                                            MetricName = r["Metric"].ToString(),
                                            CustomBaseName1 = r["CB1"].ToString(),
                                            CustomBaseName2 = r["CB2"].ToString(),
                                            CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                            CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                            MetricPercentage = r["MetricPercentage"].ToString(),
                                            CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                            CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                        }).ToList();
                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Items Purchased", StringComparison.OrdinalIgnoreCase) && !(Convert.ToString(r.Field<object>("Metric")).Equals("NARTD", StringComparison.OrdinalIgnoreCase))).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(4);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        tempRow.Insert(0, nartdRow[0]);

                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            var destItem = (IGroupShape)destinationItem.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());


                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text = ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text = ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempRow[i].MetricName;

                            var chartGroup = (IGroupShape)destItem.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)destItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion

                        #region weekend-Weekday
                        var weekendWeekday = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "week");
                        updateConnector((IConnector)weekendWeekday.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");


                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Day of Week", StringComparison.OrdinalIgnoreCase)).OrderBy(r => r.Field<object>("Metric")).Take(2);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            var weekItem = (IGroupShape)weekendWeekday.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());


                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text = ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text = ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempRow[i].MetricName;

                            var chartGroup = (IGroupShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)weekItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion

                        #region Impulse Item
                        var impulseItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "impulse");
                        updateConnector((IConnector)impulseItems.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");




                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Impulse Item", StringComparison.OrdinalIgnoreCase) && !(Convert.ToString(r.Field<object>("Metric")).Equals("NARTD", StringComparison.OrdinalIgnoreCase))).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(4);



                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();

                        var queryNartd = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Impulse Item", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("NARTD", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);

                        if (queryNartd != null && queryNartd.Count() > 0)
                        {
                            var nartdRowImpulse = (from r in queryNartd.AsEnumerable()
                                                   select new
                                                   {
                                                       MetricName = r["Metric"].ToString(),
                                                       CustomBaseName1 = r["CB1"].ToString(),
                                                       CustomBaseName2 = r["CB2"].ToString(),
                                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                                   }).ToList();
                            tempRow.Insert(0, nartdRowImpulse[0]);
                        }
                        else
                        {
                            query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Impulse Item", StringComparison.OrdinalIgnoreCase) && !(Convert.ToString(r.Field<object>("Metric")).Equals("NARTD", StringComparison.OrdinalIgnoreCase))).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5).OrderBy(r => r.Field<object>("MetricPercentage")).Take(1);



                            var tempRowNew = (from r in query.AsEnumerable()
                                              select new
                                              {
                                                  MetricName = r["Metric"].ToString(),
                                                  CustomBaseName1 = r["CB1"].ToString(),
                                                  CustomBaseName2 = r["CB2"].ToString(),
                                                  CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                                  CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                                  MetricPercentage = r["MetricPercentage"].ToString(),
                                                  CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                                  CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                              }).ToList();

                            tempRow.Add(tempRowNew[0]);
                        }



                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            var impItem = (IGroupShape)impulseItems.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());


                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text = ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text = ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempRow[i].MetricName;

                            var chartGroup = (IGroupShape)impItem.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)impItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion

                        #region Device
                        var deviceItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "device");
                        //updateConnector((IConnector)deviceItems.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");


                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Smartphone/Tablet Used to Influence Purchase", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("Yes", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            //var impItem = (IGroupShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());


                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text = ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text = ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            // ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempRow[i].MetricName;

                            var chartGroup = (IGroupShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)deviceItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion

                        #region Checkout Method
                        var checkItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "checkout");
                        updateConnector((IConnector)checkItems.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");


                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Checkout Method", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(2);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            var chkItem = (IGroupShape)checkItems.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());


                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text = ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text = ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempRow[i].MetricName;

                            var chartGroup = (IGroupShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)chkItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion

                        #region Average Check Size
                        var avgCheckItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "averageCheck");
                        // updateConnector((IConnector)impulseItems.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");


                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Trip Summary", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Percentage = r["CB1Percentage"].ToString(),
                                       CB2Percentage = r["CB2Percentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Percentage), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Percentage), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;








                            string custom1Text = ((IAutoShape)avgCheckItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)avgCheckItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = custom1Text.Replace("_val1_", "$" + Convert.ToDouble(sign1).ToString("#0.0"));

                            string custom2Text = ((IAutoShape)avgCheckItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);

                            ((IAutoShape)avgCheckItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = custom2Text.Replace("_val2_", "$" + Convert.ToDouble(sign2).ToString("#0.0"));



                            ((IAutoShape)avgCheckItems.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = "$" + Convert.ToDouble(metricPercentage * 100).ToString("#0.0");

                            loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";

                            imageReplace((PictureFrame)avgCheckItems.Shapes.Where(x => x.Name == "img").FirstOrDefault(), loc, context, 100 + slideId, subPath);

                        }



                        #endregion



                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[1].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 19:
                    try
                    {

                        #region slide 19

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[18];

                        #region Overall Satisfaction
                        var ovrSatisItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "overallSatisfaction");
                        updateConnector((IConnector)ovrSatisItems.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");


                        var query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("TripSatisfaction", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            // var chkItem = (IGroupShape)checkItems.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());


                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text = ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text = ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempRow[i].MetricName;

                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("overallchart")), filter.ColorCode);

                            var chartGroup = (IGroupShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "chart");

                            ((IAutoShape)chartGroup.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            tempChart = (IChart)chartGroup.Shapes.Where(x => x.Name == "donutChart").FirstOrDefault();
                            updateDonutTwoPointsWithLabel(tempChart, point1, point2, filter.ColorCode);

                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)ovrSatisItems.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion

                        #region Satisfaction Drivers
                        var satisfactionDrivers = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "satisfaction");
                        updateConnector((IConnector)satisfactionDrivers.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");


                        query = dataTable
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Trip Attribute Satisfaction - Top 2 Box", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(6);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            double sign1, sign2;
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB1Sig), out sign1)) sign1 = 0.0;
                            if (!double.TryParse(Convert.ToString(tempRow[i].CB2Sig), out sign2)) sign2 = 0.0;
                            point1 = metricPercentage;
                            point2 = 1 - point1;





                            var satisItem = (IGroupShape)satisfactionDrivers.Shapes.FirstOrDefault(x => x.Name == (i + 1).ToString());


                            string tempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string tempLoc = "~/Images/P2PSarReportIcons/" + tempMetricName + ".svg";


                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text = ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom1Text")).TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);

                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text = ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom2Text")).TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);


                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = tempRow[i].MetricName;


                            updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("satisfactionChart" + (i + 1))), filter.ColorCode);
                            imageReplace((PictureFrame)satisItem.Shapes.Where(x => x.Name == "img").FirstOrDefault(), tempLoc, context, 2000 + slideId + (200 + i), subPath);



                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "label")).TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";

                            var tempQuery = query
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Trip Attribute Satisfaction - Top 2 Box", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals(tempRow[i].MetricName, StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);

                            updateStackBarChartSingleSeriesData(tempQuery.CopyToDataTable(), satisItem, "custom1Chart", "custom2Chart", filter.ColorCode);


                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Text = tempRow[i].CustomBaseIndex1 ?? "";
                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom1Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);

                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Text = tempRow[i].CustomBaseIndex2 ?? "";
                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            ((IAutoShape)satisItem.Shapes.FirstOrDefault(x => x.Name == "custom2Value")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);

                        }



                        #endregion




                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[1].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 21:
                    try
                    {
                        #region slide 20
                        cur_Slide = pres.Slides[19];
                        var globe1 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe1");
                        globe1.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        #endregion


                        #region slide 21

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[20];

                        #region Frequency Funnel
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "retailer")).TextFrame.Text = filter.BenchMark.Name;

                        UpdatePyramidSeriesDataSAR(dset.Tables[1], dset.Tables[2], filter.ColorCode);

                        var pyramidGroup = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "pyramidCurve");
                        updatePyamidCurveColor(pyramidGroup, filter.ColorCode);

                        //Update Competitor Image
                        var competitorList = filter.Competitors.Select(x => x.Name).Distinct().ToList();
                        for (int i = 10; i > competitorList.Count; i--)
                        {
                            cur_Slide.Shapes.Remove(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "comp" + i));
                        }
                        for (int i = 1; i <= competitorList.Count; i++)
                        {
                            var retName = getCleanedRetailerForImage(competitorList[i - 1], "-");
                            var tempPath = "~/Images/P2PSarReport/" + retName + ".svg";
                            imageReplace((PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "comp" + i).FirstOrDefault(), tempPath, context, 3000 + slideId + i, subPath);
                        }


                        //end


                        #endregion


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[1].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 22:
                    try
                    {

                        #region slide 22

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[21];

                        #region retailer and competitorChart
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "Title Slide").FirstOrDefault()).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "Title Slide").FirstOrDefault()).TextFrame.Text.Replace("_retailer_", filter.BenchMark.Name.ToUpper());

                        //retailer
                        var query = dset.Tables[1]
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("SelectionType")).Equals("BenchMark", StringComparison.OrdinalIgnoreCase)).OrderBy(r => r.Field<object>("L2SortId")).Take(4);

                        updateStackBarChartTwoSeriesData(query.CopyToDataTable(), cur_Slide, "retailerChart", filter.ColorCode, true);
                        //end

                        //competitor
                        query = dset.Tables[1]
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("SelectionType")).Equals("Competitors", StringComparison.OrdinalIgnoreCase)).OrderBy(r => r.Field<object>("L2SortId")).Take(4);

                        updateStackBarChartTwoSeriesData(query.CopyToDataTable(), cur_Slide, "competitorChart", filter.ColorCode, false);
                        //end


                        //Update Competitor Image
                        var competitorList = filter.Competitors.Select(x => x.Name).Distinct().ToList();
                        for (int i = 10; i > competitorList.Count; i--)
                        {
                            cur_Slide.Shapes.Remove(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "comp" + i));
                        }
                        for (int i = 1; i <= competitorList.Count; i++)
                        {
                            var retName = getCleanedRetailerForImage(competitorList[i - 1], "-");
                            var tempPath = "~/Images/P2PSarReport/" + retName + ".svg";
                            imageReplace((PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "comp" + i).FirstOrDefault(), tempPath, context, 3000 + slideId + i, subPath);
                        }


                        //end


                        #endregion


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[1].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 25:
                    try
                    {
                        #region slide 23
                        cur_Slide = pres.Slides[22];
                        var globe1 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe1");
                        globe1.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        #endregion

                        #region slide 24
                        cur_Slide = pres.Slides[23];
                        var benchmark = dataTable.AsEnumerable().Where(r => Convert.ToString(r.Field<object>("SelectionType")).Equals("BenchMark", StringComparison.OrdinalIgnoreCase)).Select(r => r.Field<string>("Retailer")).FirstOrDefault();

                        var competitorList = dataTable.AsEnumerable().Where(r => Convert.ToString(r.Field<object>("SelectionType")).Equals("Competitors", StringComparison.OrdinalIgnoreCase)).OrderBy(r => r.Field<object>("Retailer")).Select(r => r.Field<string>("Retailer")).Distinct().Take(10).ToList();

                        cur_Slide.Shapes.FirstOrDefault(x => x.Name == "rectangle").FillFormat.SolidFillColor.Color = Color.FromArgb(140, ColorTranslator.FromHtml(filter.ColorCode));



                        var retName = getCleanedRetailerForImage(benchmark, "-");
                        var tempPath = "~/Images/P2PSarReport/" + retName + ".svg";
                        imageReplace((PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "retailer").FirstOrDefault(), tempPath, context, 5000 + slideId, subPath);

                        int compStart = ((10 - competitorList.Count) / 2) + 1;

                        PictureFrame first = (PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "comp1").FirstOrDefault();
                        PictureFrame second = (PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "comp2").FirstOrDefault();
                        float changeValue = (second.X - first.X) / 2;

                        for (int i = 1; i < compStart; i++)
                        {
                            cur_Slide.Shapes.Remove(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "comp" + i));
                        }
                        for (int i = compStart + competitorList.Count; i <= 10; i++)
                        {
                            cur_Slide.Shapes.Remove(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "comp" + i));
                        }
                        int compIndex = 0;

                        if (competitorList.Count % 2 == 1)
                        {
                            for (int i = compStart; i < (compStart + competitorList.Count); i++, compIndex++)
                            {
                                PictureFrame f = (PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "comp" + i).FirstOrDefault();
                                f.X = f.X + changeValue;
                                retName = getCleanedRetailerForImage(competitorList[compIndex], "-");
                                tempPath = "~/Images/P2PSarReport/" + retName + ".svg";
                                imageReplace((PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "comp" + i).FirstOrDefault(), tempPath, context, 5000 + slideId + 100 + i, subPath);
                            }
                        }
                        else
                        {
                            for (int i = compStart; i < (compStart + competitorList.Count); i++, compIndex++)
                            {
                                retName = getCleanedRetailerForImage(competitorList[compIndex], "-");
                                tempPath = "~/Images/P2PSarReport/" + retName + ".svg";
                                imageReplace((PictureFrame)cur_Slide.Shapes.Where(x => x.Name == "comp" + i).FirstOrDefault(), tempPath, context, 5000 + slideId + 100 + i, subPath);
                            }
                        }




                        #endregion


                        #region slide 25

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[24];
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dataTable
                                        .AsEnumerable()
                                        .OrderBy(r => r.Field<object>("SelectionType")).ThenBy(r => r.Field<object>("Retailer")).ThenBy(r => r.Field<object>("L2SortId"));

                        createDinerImageryTableSAR(cur_Slide, query.CopyToDataTable(), slideId, filter.ColorCode, 0.0);


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[2].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 26:
                    try
                    {

                        #region slide 26

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[25];
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dset.Tables[0]
                                        .AsEnumerable()
                                        .OrderBy(r => r.Field<object>("SelectionType")).ThenBy(r => r.Field<object>("Retailer")).ThenBy(r => r.Field<object>("L2SortId"));
                        var stdD = dset.Tables[1]
                                        .AsEnumerable().Select(r => r.Field<object>("Standard Deviation")).FirstOrDefault();

                        scatterTable = (dset.Tables[0]
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("SelectionType")).Equals("BenchMark", StringComparison.OrdinalIgnoreCase) && !Convert.ToString(r.Field<object>("Metric")).Equals("None of these", StringComparison.OrdinalIgnoreCase))).CopyToDataTable();

                        double stdDev = 0.0;
                        if (!double.TryParse(stdD.ToString(), out stdDev)) stdDev = 0.0;



                        createDinerImageryTableSAR(cur_Slide, query.CopyToDataTable(), slideId, filter.ColorCode, stdDev);


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);


                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[2].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 27:
                    try
                    {

                        #region slide 27

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[26];
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dataTable
                                        .AsEnumerable()
                                        .OrderBy(r => r.Field<object>("SelectionType")).ThenBy(r => r.Field<object>("Retailer")).ThenBy(r => r.Field<object>("L2SortId"));




                        createDinerImageryTableSAR(cur_Slide, query.CopyToDataTable(), slideId, filter.ColorCode, 0.0);


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[2].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 28:
                    try
                    {

                        #region slide 28

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[27];
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dset.Tables[0]
                                        .AsEnumerable()
                                        .OrderBy(r => r.Field<object>("SelectionType")).ThenBy(r => r.Field<object>("Retailer")).ThenBy(r => r.Field<object>("L2SortId"));
                        var stdD = dset.Tables[1]
                                        .AsEnumerable().Select(r => r.Field<object>("Standard Deviation")).FirstOrDefault();

                        double stdDev = 0.0;
                        if (!double.TryParse(stdD.ToString(), out stdDev)) stdDev = 0.0;



                        createDinerImageryTableSAR(cur_Slide, query.CopyToDataTable(), slideId, filter.ColorCode, stdDev);


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[2].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 30:
                    try
                    {

                        #region slide 29

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[28];
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dataTable
                                        .AsEnumerable()
                                        .OrderByDescending(r => r.Field<object>("values"));

                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricName = r["Metric"].ToString(),
                                       Values = r["values"] == DBNull.Value ? 0 : Convert.ToInt32(r["values"])
                                   }).ToList();

                        IAutoShape conn = (IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "arrow").FirstOrDefault();

                        conn.FillFormat.GradientFormat.GradientStops.Clear();
                        conn.FillFormat.GradientFormat.GradientStops.Add(0, ColorTranslator.FromHtml(filter.ColorCode));
                        conn.FillFormat.GradientFormat.GradientStops.Add(0.18f, ColorTranslator.FromHtml(filter.ColorCode));
                        conn.FillFormat.GradientFormat.GradientStops.Add(0.35f, ColorTranslator.FromHtml(filter.ColorCode));
                        conn.FillFormat.GradientFormat.GradientStops.Add(0.65f, ColorTranslator.FromHtml(filter.ColorCode));
                        conn.FillFormat.GradientFormat.GradientStops.Add(0.82f, ColorTranslator.FromHtml(filter.ColorCode));
                        conn.FillFormat.GradientFormat.GradientStops.Add(1, ColorTranslator.FromHtml(filter.ColorCode));
                        conn.FillFormat.GradientFormat.GradientStops.Add(0.5f, Color.White);

                        ITable table = (ITable)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "tbl");



                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            table[0, i + 1].TextFrame.Text = tempRow[i].MetricName;
                            table[1, i + 1].TextFrame.Text = tempRow[i].Values.ToString();
                        }

                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[2].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                        #region slide 30

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[29];
                        imageReplace((PictureFrame)pres.Masters[3].LayoutSlides[5].Shapes.Where(x => x.Name == "SelectedEstablish_Img").FirstOrDefault(), loc, context, 100, subPath);
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");


                        query = dataTable
                                        .AsEnumerable()
                                        .OrderBy(r => r.Field<object>("values"));

                        //tempRow = (from r in query.AsEnumerable()
                        //           select new
                        //           {
                        //               MetricName = r["Metric"].ToString(),
                        //               Values = r["values"] == DBNull.Value ? 0 : Convert.ToInt32(r["values"])
                        //           }).ToList();

                        updateScatterSeries(cur_Slide, "chart", scatterTable, query.CopyToDataTable(), filter.ColorCode, 30, filter.CustomBase[0].Name, filter.BenchMark.Name);





                        ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "txt_devtn_1").FirstOrDefault()).TextFrame.Text = Convert.ToString(deviationValue) + "%";
                        ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "txt_devtn_2").FirstOrDefault()).TextFrame.Text = "- " + Convert.ToString(deviationValue) + "%";
                        setStrengthWeekPosition(cur_Slide);
                        timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArrayNew = { };
                        if (filter.Filters != null)
                        {
                            filterArrayNew = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterTextNew = filterArrayNew.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterTextNew + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 33:
                    try
                    {
                        #region slide 31
                        cur_Slide = pres.Slides[30];
                        var globe1 = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "globe1");
                        globe1.Shapes.FirstOrDefault(x => x.Name == "oval").FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        #endregion

                        #region slide 33

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[32];
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        barWithLineChart(dataTable, cur_Slide, "chart", false, filter.ColorCode, context, slideId, subPath, true);
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[3].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;
                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gainsIcon"), filter.ColorCode);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "lossIcon"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[6].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 34:
                    try
                    {

                        #region slide 34

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        //loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[33];

                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text.Replace("_retailer_", filter.BenchMark.Name);


                        #region SSD Categories 1
                        var query = dataTable
                                         .AsEnumerable()
                                         .Where(r => Convert.ToString(r.Field<object>("sort")).Equals("1", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricType = r["MetricType"].ToString(),
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        ITable table = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "regularTable").FirstOrDefault();

                        IGroupShape group = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "regularGroup").FirstOrDefault();

                        updateConnector((IConnector)group.Shapes.Where(x => x.Name == "DownConnector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateConnector((IConnector)group.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateConnector((IConnector)group.Shapes.Where(x => x.Name == "headerConnector").FirstOrDefault(), filter.ColorCode, "gradient");

                        ((IAutoShape)group.Shapes.Where(x => x.Name == "category").FirstOrDefault()).TextFrame.Text = tempRow[0].MetricType;

                        string beverageMetricName = getCleanedRetailerForImage(tempRow[0].MetricType, "-");
                        string beverageMetricLocation = "~/Images/P2PSarReportIcons/" + beverageMetricName + ".svg";
                        imageReplace((PictureFrame)group.Shapes.Where(x => x.Name == "img").FirstOrDefault(), beverageMetricLocation, context, 2000 + 100 + slideId, subPath);



                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("regularIconChart")), filter.ColorCode);

                        string tempMetricName = getCleanedRetailerForImage(filter.BenchMark.Name, "-");
                        string tempLoc = "~/Images/P2PSarReport/" + tempMetricName + ".svg";
                        imageReplace((PictureFrame)group.Shapes.Where(x => x.Name == "retailerImage").FirstOrDefault(), tempLoc, context, 2000 + 200 + slideId, subPath);

                        IChart chart = (IChart)group.Shapes.FirstOrDefault(x => x.Name == "chart");
                        chart.ChartData.Series[0].Format.Fill.FillType = FillType.Pattern;
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.PatternStyle = PatternStyle.Trellis;
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.ForeColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.BackColor.Color = Color.White;
                        chart.ChartData.Series[0].Format.Line.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);

                        table[2, 0].TextFrame.Text = table[2, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        table[3, 0].TextFrame.Text = table[3, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        chart.ChartData.ChartDataWorkbook.GetCell(0, 0, 3, tempRow[0].MetricType);
                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            chart.ChartData.ChartDataWorkbook.GetCell(0, 5 - i, 3, tempRow[i].MetricName);
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            chart.ChartData.ChartDataWorkbook.GetCell(0, 5 - i, 2, metricPercentage);

                            table[1, i + 1].TextFrame.Text = table[1, i + 1].TextFrame.Text.Replace("_metric_", Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%");

                            var tempQuery = query
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("Metric")).Equals(tempRow[i].MetricName, StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);

                            updateSARportIndexTable(cur_Slide, tempQuery.CopyToDataTable(), "regularTable", "Metric", -1, -1, 2, 3, i + 1, 2);

                            string locTempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string locTempLoc = "~/Images/P2PSarReportBeverageLogos/" + locTempMetricName + ".svg";
                            imageReplaceV2((PictureFrame)group.Shapes.Where(x => x.Name == "brand" + (i + 1) + "Image").FirstOrDefault(), locTempLoc, context, 3000 + slideId + i, subPath, (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "regularTable").FirstOrDefault(), i + 1, 0);
                        }



                        #endregion


                        #region SSD Categories 2
                        query = dataTable
                                         .AsEnumerable()
                                         .Where(r => Convert.ToString(r.Field<object>("sort")).Equals("2", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricType = r["MetricType"].ToString(),
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        table = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "dietTable").FirstOrDefault();

                        IGroupShape group2 = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "dietGroup").FirstOrDefault();

                        updateConnector((IConnector)group2.Shapes.Where(x => x.Name == "DownConnector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateConnector((IConnector)group2.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateConnector((IConnector)group2.Shapes.Where(x => x.Name == "headerConnector").FirstOrDefault(), filter.ColorCode, "gradient");

                        ((IAutoShape)group2.Shapes.Where(x => x.Name == "category").FirstOrDefault()).TextFrame.Text = tempRow[0].MetricType;

                        string beverageMetricName2 = getCleanedRetailerForImage(tempRow[0].MetricType, "-");
                        string beverageMetricLocation2 = "~/Images/P2PSarReportIcons/" + beverageMetricName2 + ".svg";
                        imageReplace((PictureFrame)group2.Shapes.Where(x => x.Name == "img").FirstOrDefault(), beverageMetricLocation2, context, 2000 + 300 + slideId, subPath);

                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("dietIconChart")), filter.ColorCode);

                        tempMetricName = getCleanedRetailerForImage(filter.BenchMark.Name, "-");
                        tempLoc = "~/Images/P2PSarReport/" + tempMetricName + ".svg";
                        imageReplace((PictureFrame)group2.Shapes.Where(x => x.Name == "retailerImage").FirstOrDefault(), tempLoc, context, 2000 + 400 + slideId, subPath);

                        chart = (IChart)group2.Shapes.FirstOrDefault(x => x.Name == "chart");
                        chart.ChartData.Series[0].Format.Fill.FillType = FillType.Pattern;
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.PatternStyle = PatternStyle.Trellis;
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.ForeColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.BackColor.Color = Color.White;
                        chart.ChartData.Series[0].Format.Line.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);

                        table[2, 0].TextFrame.Text = table[2, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        table[3, 0].TextFrame.Text = table[3, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        chart.ChartData.ChartDataWorkbook.GetCell(0, 0, 3, tempRow[0].MetricType);

                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            chart.ChartData.ChartDataWorkbook.GetCell(0, 5 - i, 3, tempRow[i].MetricName);
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            chart.ChartData.ChartDataWorkbook.GetCell(0, 5 - i, 2, metricPercentage);

                            table[1, i + 1].TextFrame.Text = table[1, i + 1].TextFrame.Text.Replace("_metric_", Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%");

                            var tempQuery = query
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("Metric")).Equals(tempRow[i].MetricName, StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);

                            updateSARportIndexTable(cur_Slide, tempQuery.CopyToDataTable(), "dietTable", "Metric", -1, -1, 2, 3, i + 1, 2);

                            string locTempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string locTempLoc = "~/Images/P2PSarReportBeverageLogos/" + locTempMetricName + ".svg";
                            imageReplaceV2((PictureFrame)group2.Shapes.Where(x => x.Name == "brand" + (i + 1) + "Image").FirstOrDefault(), locTempLoc, context, 4000 + slideId + i, subPath, (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "dietTable").FirstOrDefault(), i + 1, 0);
                        }



                        #endregion


                        #region SSD Categories 3
                        query = dataTable
                                         .AsEnumerable()
                                         .Where(r => Convert.ToString(r.Field<object>("sort")).Equals("3", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5);
                        tempRow = (from r in query.AsEnumerable()
                                   select new
                                   {
                                       MetricType = r["MetricType"].ToString(),
                                       MetricName = r["Metric"].ToString(),
                                       CustomBaseName1 = r["CB1"].ToString(),
                                       CustomBaseName2 = r["CB2"].ToString(),
                                       CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? null : r["CB1Index"].ToString(),
                                       CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? null : r["CB2Index"].ToString(),
                                       MetricPercentage = r["MetricPercentage"].ToString(),
                                       CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                       CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                   }).ToList();
                        table = (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "coffeeTable").FirstOrDefault();

                        IGroupShape group3 = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "coffeeGroup").FirstOrDefault();

                        updateConnector((IConnector)group3.Shapes.Where(x => x.Name == "DownConnector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateConnector((IConnector)group3.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");
                        updateConnector((IConnector)group3.Shapes.Where(x => x.Name == "headerConnector").FirstOrDefault(), filter.ColorCode, "gradient");

                        ((IAutoShape)group3.Shapes.Where(x => x.Name == "category").FirstOrDefault()).TextFrame.Text = tempRow[0].MetricType;

                        string beverageMetricName3 = getCleanedRetailerForImage(tempRow[0].MetricType, "-");
                        string beverageMetricLocation3 = "~/Images/P2PSarReportIcons/" + beverageMetricName3 + ".svg";
                        imageReplace((PictureFrame)group3.Shapes.Where(x => x.Name == "img").FirstOrDefault(), beverageMetricLocation3, context, 2000 + 500 + slideId, subPath);

                        updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == ("coffeeIconChart")), filter.ColorCode);

                        tempMetricName = getCleanedRetailerForImage(filter.BenchMark.Name, "-");
                        tempLoc = "~/Images/P2PSarReport/" + tempMetricName + ".svg";
                        imageReplace((PictureFrame)group3.Shapes.Where(x => x.Name == "retailerImage").FirstOrDefault(), tempLoc, context, 2000 + 600 + slideId, subPath);

                        chart = (IChart)group3.Shapes.FirstOrDefault(x => x.Name == "chart");
                        chart.ChartData.Series[0].Format.Fill.FillType = FillType.Pattern;
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.PatternStyle = PatternStyle.Trellis;
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.ForeColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        chart.ChartData.Series[0].Format.Fill.PatternFormat.BackColor.Color = Color.White;
                        chart.ChartData.Series[0].Format.Line.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);

                        table[2, 0].TextFrame.Text = table[2, 0].TextFrame.Text.Replace("_base1_", filter.CustomBase[0].Name == "Previous Period" ? "PP" : filter.CustomBase[0].Name);
                        table[3, 0].TextFrame.Text = table[3, 0].TextFrame.Text.Replace("_base2_", filter.CustomBase[1].Name == "Previous Period" ? "PP" : filter.CustomBase[1].Name);
                        chart.ChartData.ChartDataWorkbook.GetCell(0, 0, 3, tempRow[0].MetricType);

                        for (int i = 0; i < tempRow.Count; i++)
                        {
                            chart.ChartData.ChartDataWorkbook.GetCell(0, 5 - i, 3, tempRow[i].MetricName);
                            if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                            chart.ChartData.ChartDataWorkbook.GetCell(0, 5 - i, 2, metricPercentage);

                            table[1, i + 1].TextFrame.Text = table[1, i + 1].TextFrame.Text.Replace("_metric_", Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%");

                            var tempQuery = query
                                        .AsEnumerable()
                                        .Where(r => Convert.ToString(r.Field<object>("Metric")).Equals(tempRow[i].MetricName, StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);

                            updateSARportIndexTable(cur_Slide, tempQuery.CopyToDataTable(), "coffeeTable", "Metric", -1, -1, 2, 3, i + 1, 2);

                            string locTempMetricName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                            string locTempLoc = "~/Images/P2PSarReportBeverageLogos/" + locTempMetricName + ".svg";
                            imageReplaceV2((PictureFrame)group3.Shapes.Where(x => x.Name == "brand" + (i + 1) + "Image").FirstOrDefault(), locTempLoc, context, 5000 + slideId + i, subPath, (AsposeSlide.ITable)cur_Slide.Shapes.Where(x => x.Name == "coffeeTable").FirstOrDefault(), i + 1, 0);
                        }



                        #endregion


                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[3].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);

                        #endregion


                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 35:
                    try
                    {

                        #region slide 35

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[34];

                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text.Replace("_retailer_", filter.BenchMark.Name);
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        barWithLineChart(dataTable, cur_Slide, "chart", false, filter.ColorCode, context, slideId, subPath, true);
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[3].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gainsIcon"), filter.ColorCode);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "lossIcon"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[6].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 36:
                    try
                    {

                        #region slide 36

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[35];

                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text.Replace("_retailer_", filter.BenchMark.Name);
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        barWithLineChart(dataTable, cur_Slide, "chart", false, filter.ColorCode, context, slideId, subPath, true);
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[3].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gainsIcon"), filter.ColorCode);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "lossIcon"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[6].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 37:
                    try
                    {

                        #region slide 37

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[36];

                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "headingTextbox")).TextFrame.Text.Replace("_retailer_", filter.BenchMark.Name);
                        updateConnector((IConnector)cur_Slide.Shapes.Where(x => x.Name == "connector").FirstOrDefault(), filter.ColorCode, "gradient");

                        var query = dset.Tables[0].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("ChilledOrAmbient")).Equals("1", StringComparison.OrdinalIgnoreCase)).CopyToDataTable();

                        ITable barTable1 = (ITable)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "barChartTable1");

                        int ind = 0;
                        barTable1[ind, 0].TextFrame.Text = "Index vs " + query.Rows[0].Field<string>(11);
                        foreach (DataRow row in query.Rows)
                        {
                            barTable1[ind + 1, 0].TextFrame.Text = row["CB1Index"] == DBNull.Value ? "NA" : row["CB1Index"].ToString();
                            ind++;
                        }


                        barWithLineChart(query, cur_Slide, "barChart1", false, filter.ColorCode, context, slideId, subPath, true);

                        query = dset.Tables[0].AsEnumerable().Where(x => Convert.ToString(x.Field<object>("ChilledOrAmbient")).Equals("0", StringComparison.OrdinalIgnoreCase)).CopyToDataTable();

                        ITable barTable2 = (ITable)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "barChartTable2");

                        ind = 0;
                        barTable2[ind, 0].TextFrame.Text = "Index vs " + query.Rows[0].Field<string>(11);
                        foreach (DataRow row in query.Rows)
                        {
                            barTable2[ind + 1, 0].TextFrame.Text = row["CB1Index"] == DBNull.Value ? "NA" : row["CB1Index"].ToString();
                            ind++;
                        }

                        barWithLineChart(query, cur_Slide, "barChart2", false, filter.ColorCode, context, slideId, subPath, true);

                        #region clustered Bar Chart
                        //var barData = dset.Tables[1].AsEnumerable().OrderBy(x => x.Field<object>("Metric")).ThenBy(x => x.Field<object>("MetricType")).CopyToDataTable();
                        var barData = dset.Tables[1].AsEnumerable().CopyToDataTable();
                        var distinctSeries = barData.AsEnumerable().Select(x => x.Field<string>("Metric")).Distinct().ToList();
                        //distinctSeries.Sort();
                        var distinctCategories = barData.AsEnumerable().Select(x => x.Field<string>("MetricType")).Distinct().ToList();
                        //distinctCategories.Sort();
                        IChart barChart = (IChart)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "stackedChart");
                        IChartDataWorkbook barWorkbook = barChart.ChartData.ChartDataWorkbook;

                        for (int i = 0; i < distinctSeries.Count; i++)
                        {
                            barWorkbook.GetCell(0, i + 1, 0, distinctSeries[i]);
                            ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Series" + (i + 1) + "Text")).TextFrame.Text = distinctSeries[i];
                            IShape legendShape = (IShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Series" + (i + 1) + "Legend");
                            if (i == 0)
                            {
                                legendShape.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                            }
                        }
                        for (int j = 0; j < distinctCategories.Count; j++)
                        {
                            barWorkbook.GetCell(0, 0, j + 1, distinctCategories[j]);
                            ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Categories" + (j + 1) + "Text")).TextFrame.Text = distinctCategories[j];
                        }
                        ITable categories1 = (ITable)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "chilledTable");
                        ITable categories2 = (ITable)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ambientTable");
                        barChart.ChartData.Series[0].Format.Fill.FillType = FillType.Solid;
                        barChart.ChartData.Series[0].Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(filter.ColorCode);
                        for (int i = 0; i < distinctSeries.Count; i++)
                        {
                            for (int j = 0; j < distinctCategories.Count; j++)
                            {
                                DataRow row = barData.AsEnumerable().Where(x => Convert.ToString(x.Field<object>("Metric")).Equals(distinctSeries[i], StringComparison.OrdinalIgnoreCase) && Convert.ToString(x.Field<object>("MetricType")).Equals(distinctCategories[j], StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                                barChart.ChartData.Series[i].DataPoints[j].Value.Data = row["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(row["MetricPercentage"]);
                                if (j == 0)
                                    categories1[0, i + 1].TextFrame.Text = row["Change"] == DBNull.Value ? "0.0" : Convert.ToDouble(row["Change"]).ToString("#0.0") + "%";
                                else
                                    categories2[0, i + 1].TextFrame.Text = row["Change"] == DBNull.Value ? "0.0" : Convert.ToDouble(row["Change"]).ToString("#0.0") + "%";
                            }
                        }
                        #endregion
                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[3].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gainsIcon"), filter.ColorCode);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "lossIcon"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[6].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;
                case 40:
                    try
                    {

                        #region slide 40

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[39];


                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Title Slide")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Title Slide")).TextFrame.Text.Replace("_retailer_", filter.BenchMark.Name.ToUpper());


                        var query = dataTable
                                         .AsEnumerable()
                                         .Where(r => Convert.ToInt32(r.Field<object>("sortID")) <= 2).OrderBy(r => r.Field<object>("sortID")).OrderByDescending(r => r.Field<object>("MetricPercentage"));

                        updateStackedBarChart(cur_Slide, query.CopyToDataTable(), "chart", filter.ColorCode, context, subPath, slideId);






                        string timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        //timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArray = { };
                        if (filter.Filters != null)
                        {
                            filterArray = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterText = filterArray.Length == 0 ? "NONE" : string.Join(",", filterArray).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterText + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gainsIcon"), filter.ColorCode);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "lossIcon"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[6].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion


                        #region slide 41

                        //loc = "~/Images/P2PDashboardEsthmtImages/" + cleanedBenchmark + ".svg";
                        loc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                        cur_Slide = pres.Slides[40];


                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Title Slide")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Title Slide")).TextFrame.Text.Replace("_retailer_", filter.BenchMark.Name.ToUpper());


                        query = dataTable
                                         .AsEnumerable()
                                         .Where(r => Convert.ToInt32(r.Field<object>("sortID")) > 2).OrderBy(r => r.Field<object>("sortID")).OrderByDescending(r => r.Field<object>("MetricPercentage"));

                        updateStackedBarChart(cur_Slide, query.CopyToDataTable(), "chart", filter.ColorCode, context, subPath, slideId);






                        timeperiodText = replaceText(((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text, "_timeperiod_", filter.TimeperiodType);
                        //timeperiodText = replaceText(timeperiodText, "_frequency_", filter.Frequency[0].Name);
                        ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeperiodTextbox")).TextFrame.Text = timeperiodText;

                        string[] filterArrayNew = { };
                        if (filter.Filters != null)
                        {
                            filterArrayNew = filter.Filters.Select(filt => filt.Name).ToArray();
                        }
                        string filterTextNew = filterArrayNew.Length == 0 ? "NONE" : string.Join(",", filterArrayNew).ToUpper();
                        notesSection(cur_Slide, "Filter - " + filterTextNew + "\n\n" + ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)cur_Slide).NotesSlideManager).NotesSlide).NotesTextFrame).Text);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "gainsIcon"), filter.ColorCode);
                        //updateIconColor(cur_Slide.Shapes.FirstOrDefault(x => x.Name == "lossIcon"), filter.ColorCode);
                        //imageReplace((PictureFrame)pres.Slides[6].Shapes.Where(x => x.Name == "retailerLogo").FirstOrDefault(), loc, context, slideId, subPath);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ExceptionSlideList.Add(slideId);
                        ErrorLogSar.LogError(ex.Message + "   --- Inside Slide " + slideId + " SARReport", ex.StackTrace, context);
                    }
                    break;

            }
        }

        private void barWithLineChart(System.Data.DataTable tbl, ISlide sld, string chart_shape_name, bool isImage, string colorCode, HttpContextBase context, int slideId, string subPath, bool useLineOnSecondary, string loc = "")
        {
            IChart chart;
            if (tbl.Rows.Count != 0)
            {
                chart = (IChart)sld.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();

                int defaultWorksheetIndex = 0;

                int gainIndex = -1, lossIndex = -1;

                string gainMetric = ""; string lossMetric = "";

                double gainChange = 0; double lossChange = 0;
                double gainPercentage = 0; double lossPercentage = 0;

                bool hasSignificanceFlag = false;

                if (tbl.Columns.Contains("Significance"))
                {
                    hasSignificanceFlag = true;
                }


                //Getting the chart data worksheet
                IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

                //Delete default generated series and categories
                chart.ChartData.Series.Clear();
                chart.ChartData.Categories.Clear();
                chart.ChartData.ChartDataWorkbook.Clear(0);

                //add chart title in work sheet
                fact.GetCell(0, 49, 0, "title");
                fact.GetCell(0, 49, 1, Convert.ToString(tbl.Rows[0]["metric"]));

                int s = chart.ChartData.Series.Count;
                //objectivelist = (from r in tbl.AsEnumerable() select Convert.ToString(r["Objective"])).Distinct().ToList();
                objectivelist = new List<string>() { "Trip Share", "Series 2", "Gap vs. YAGO" };
                metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r["metric"])).Distinct().ToList();

                //for (int i = 1; i < objectivelist.Count + 1; i++)
                //{
                //    chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);
                //}
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, 1, objectivelist[0]), ChartType.ClusteredColumn);
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, 2, objectivelist[1]), ChartType.ClusteredColumn);
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, 3, objectivelist[2]), ChartType.LineWithMarkers);
                //Adding new categories
                for (int i = 0; i < metriclist.Count; i++)
                {
                    //Setting Category Name
                    chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
                }

                int serCount = 1;
                int catcount = 1;
                IChartSeries Series;


                Series = chart.ChartData.Series[serCount - 1];
                IChartSeries LineSeries = chart.ChartData.Series[serCount + 1];
                if (useLineOnSecondary)
                {
                    LineSeries.PlotOnSecondAxis = true;
                }

                LineSeries.Marker.Symbol = MarkerStyleType.Square;
                LineSeries.Marker.Size = 7;
                LineSeries.Marker.Format.Fill.FillType = FillType.Solid;
                LineSeries.Marker.Format.Fill.SolidFillColor.Color = Color.Black;
                LineSeries.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                LineSeries.Marker.Format.Line.FillFormat.SolidFillColor.Color = Color.Black;

                Series.ParentSeriesGroup.GapWidth = 47;
                Series.ParentSeriesGroup.Overlap = 100;
                Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.ShowLeaderLines = true;
                Series.Labels.DefaultDataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                Series.Labels.DefaultDataLabelFormat.Format.Fill.FillType = FillType.Solid;
                Series.Labels.DefaultDataLabelFormat.Format.Fill.SolidFillColor.Color = Color.White;
                Series.Format.Fill.FillType = FillType.Solid;
                Series.Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(colorCode);
                LineSeries.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                LineSeries.Labels.DefaultDataLabelFormat.ShowValue = true;
                LineSeries.Labels.DefaultDataLabelFormat.ShowLeaderLines = true;
                LineSeries.Labels.DefaultDataLabelFormat.Position = LegendDataLabelPosition.Top;
                LineSeries.Labels.DefaultDataLabelFormat.Format.Fill.FillType = FillType.Solid;
                LineSeries.Labels.DefaultDataLabelFormat.Format.Fill.SolidFillColor.Color = Color.White;
                chart.Legend.Entries[1].Hide = true;
                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("metric")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     percentage = Math.Round(double.Parse(r["CYValue"].ToString()), 1),
                                     change = Math.Round(double.Parse(r["DIffValue"].ToString()), 1),
                                     MetricName = r["metric"].ToString(),
                                     Significance = hasSignificanceFlag ? GetSigColorNormalVal(double.Parse(r["Significance"].ToString())) : 0
                                 }).FirstOrDefault();

                    if (query.Significance == 1)
                    {
                        if (gainIndex == -1)
                        {
                            gainIndex = catcount;
                            gainChange = query.change;
                            gainPercentage = query.percentage;
                            gainMetric = query.MetricName;
                        }
                        else if (query.change > gainChange)
                        {
                            gainIndex = catcount;
                            gainChange = query.change;
                            gainPercentage = query.percentage;
                            gainMetric = query.MetricName;
                        }
                    }
                    else if (query.Significance == -1)
                    {
                        if (lossIndex == -1)
                        {
                            lossIndex = catcount;
                            lossChange = query.change;
                            lossPercentage = query.percentage;
                            lossMetric = query.MetricName;
                        }
                        else if (query.change > lossChange)
                        {
                            lossIndex = catcount;
                            lossChange = query.change;
                            lossPercentage = query.percentage;
                            lossMetric = query.MetricName;
                        }
                    }




                    catcount++;
                }
                catcount = 1;

                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("metric")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     percentage = Math.Round(double.Parse(r["CYValue"].ToString()), 1),
                                     change = Math.Round(double.Parse(r["DIffValue"].ToString()), 1),
                                     MetricName = r["metric"].ToString(),
                                     Significance = hasSignificanceFlag ? GetSigColorNormalVal(double.Parse(r["Significance"].ToString())) : 0
                                 }).FirstOrDefault();



                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.percentage)) ? (Convert.ToDouble(query.percentage / 100)) : 0)));
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";
                    IDataLabel lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.Width = (float)15.5;

                    lbl.TextFormat.TextBlockFormat.WrapText = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = (float)10.5;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;


                    LineSeries.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount + 2, (!string.IsNullOrEmpty(Convert.ToString(query.change)) ? (Convert.ToDouble(query.change / 100)) : 0)));
                    LineSeries.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    LineSeries.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";
                    lbl = LineSeries.DataPoints[catcount - 1].Label;
                    lbl.Width = (float)15.5;
                    lbl.TextFormat.TextBlockFormat.WrapText = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = (float)10.5;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    //float barWidth = chart.UserShapes.Width / metriclist.Count;
                    //float actualBarwidth = chart.UserShapes.Width / ((1.47f * (metriclist.Count - 1)) + 1);
                    //float barWidth = 1.47f * actualBarwidth;
                    if (catcount == gainIndex)
                    {
                        //IShape shapeTriangle = sld.Shapes.FirstOrDefault(x => x.Name == "greenTriangle");
                        //shapeTriangle.X = chart.UserShapes.X ;
                        //shapeTriangle.X = chart.UserShapes.X+(barWidth/1.47f);
                        //shapeTriangle.Y = Series.DataPoints[catcount - 1].ActualY;
                        lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = Color.Green;
                    }
                    if (catcount == lossIndex)
                    {
                        //IShape shapeTriangle = sld.Shapes.FirstOrDefault(x => x.Name == "redTriangle");
                        ////shapeTriangle.X = chart.UserShapes.X + (barWidth*(metriclist.Count-1+(1 / 1.47f)));
                        //shapeTriangle.X = chart.UserShapes.X + chart.UserShapes.Width;
                        //shapeTriangle.X = chart.UserShapes.X + (barWidth * (metriclist.Count - 2)) + (actualBarwidth/2f);
                        //shapeTriangle.Y = Series.DataPoints[catcount - 1].ActualY;
                        lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = Color.Red;
                    }


                    if ((catcount == gainIndex || catcount == lossIndex) && (slideId == 7 || slideId == 8))
                    {
                        //loc = "~/Images/P2PDashboardEsthmtImages/" + getCleanedRetailerForImage(query.MetricName,"-") + ".svg";
                        loc = "~/Images/P2PSarReport/" + getCleanedRetailerForImage(query.MetricName, "-") + ".svg";
                        IGroupShape grpShape;
                        if (catcount == gainIndex)
                            grpShape = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "gains").FirstOrDefault();
                        else
                            grpShape = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "loss").FirstOrDefault();

                        ((IAutoShape)grpShape.Shapes.FirstOrDefault(x => x.Name == "retailerText")).TextFrame.Text = query.MetricName;

                        if (isImage)
                        {
                            imageReplace((PictureFrame)grpShape.Shapes.Where(x => x.Name == "retailerImage").FirstOrDefault(), loc, context, slideId, subPath);
                        }
                        IAutoShape changeShape = (IAutoShape)grpShape.Shapes.FirstOrDefault(x => x.Name == "changeText");
                        string result1 = replaceText(changeShape.TextFrame.Text, "_shareChange_", (query.change > 0) && catcount == 1 ? string.Format("+{0}%", query.change) : string.Format("{0}%", query.change));
                        changeShape.TextFrame.Text = replaceText(result1, "_visitChange_", string.Format("{0}%", query.percentage));
                    }


                    //Set Data Point Label Style
                    //IDataLabel lbl = Series.DataPoints[catcount - 1].Label;
                    //lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    //lbl.DataLabelFormat.ShowValue = true;
                    //lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    //lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    //lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, query.sampleSize, _objective);
                    //LabelFontSize(lbl);
                    catcount++;
                }
                if (slideId == 7 || slideId == 8)
                {
                    chart.ValidateChartLayout();
                    float actualBarwidth = chart.PlotArea.ActualWidth / (metriclist.Count);
                    if (gainIndex != -1)
                    {
                        IShape shapeTriangle = sld.Shapes.FirstOrDefault(x => x.Name == "greenTriangle");
                        //shapeTriangle.X = chart.UserShapes.X + chart.PlotArea.ActualX + (actualBarwidth / 2) - (shapeTriangle.Width / 2);
                        shapeTriangle.X = chart.UserShapes.X + chart.PlotArea.ActualX + (actualBarwidth * ((float)gainIndex - 0.5f)) - (shapeTriangle.Width / 2);
                        shapeTriangle.Y = chart.UserShapes.Y + chart.PlotArea.ActualY + chart.PlotArea.ActualHeight - 50;
                    }
                    else
                    {
                        //if (slideId == 7)
                        //{
                        //    cur_Slide.Shapes.Where(x => x.Name == "separator").FirstOrDefault().Hidden = true;
                        //}
                        IGroupShape grp = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "gains").FirstOrDefault();
                        grp.Shapes.FirstOrDefault(x => x.Name == "retailerText").Hidden = true;
                        //grp.Shapes.FirstOrDefault(x => x.Name == "gainHeading").Hidden = true;
                        grp.Shapes.FirstOrDefault(x => x.Name == "changeText").Hidden = true;
                        if (slideId == 7)
                        {
                            grp.Shapes.FirstOrDefault(x => x.Name == "gainsIcon").Hidden = true;
                        }
                        else
                        {
                            grp.Shapes.FirstOrDefault(x => x.Name == "retailerImage").Hidden = true;
                        }
                        //cur_Slide.Shapes.Where(x => x.Name == "gains").FirstOrDefault().Hidden = true;
                        sld.Shapes.FirstOrDefault(x => x.Name == "greenTriangle").Hidden = true;
                    }
                    if (lossIndex != -1)
                    {
                        IShape shapeTriangle2 = sld.Shapes.FirstOrDefault(x => x.Name == "redTriangle");
                        shapeTriangle2.X = chart.UserShapes.X + chart.PlotArea.ActualX + (actualBarwidth * ((float)lossIndex - 0.5f)) - (shapeTriangle2.Width / 2);
                        shapeTriangle2.Y = chart.UserShapes.Y + chart.PlotArea.ActualY + chart.PlotArea.ActualHeight - 50;
                    }
                    else
                    {
                        //if (slideId == 7)
                        //{
                        //    cur_Slide.Shapes.Where(x => x.Name == "separator").FirstOrDefault().Hidden = true;
                        //}
                        IGroupShape grp = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "loss").FirstOrDefault();
                        grp.Shapes.FirstOrDefault(x => x.Name == "retailerText").Hidden = true;
                        //grp.Shapes.FirstOrDefault(x => x.Name == "lossHeading").Hidden = true;
                        grp.Shapes.FirstOrDefault(x => x.Name == "changeText").Hidden = true;
                        if (slideId == 7)
                        {
                            grp.Shapes.FirstOrDefault(x => x.Name == "lossIcon").Hidden = true;
                        }
                        else
                        {
                            grp.Shapes.FirstOrDefault(x => x.Name == "retailerImage").Hidden = true;
                        }
                        //cur_Slide.Shapes.Where(x => x.Name == "loss").FirstOrDefault().Hidden = true;
                        sld.Shapes.FirstOrDefault(x => x.Name == "redTriangle").Hidden = true;
                    }
                    /*chart.ValidateChartLayout();

                    float actualBarwidth = chart.PlotArea.ActualWidth / (metriclist.Count);
                    float barWidth = actualBarwidth / 1.47f;
                    float gapWidth = actualBarwidth - barWidth;
                    IShape shapeTriangle = sld.Shapes.FirstOrDefault(x => x.Name == "greenTriangle");
                    //shapeTriangle.X = chart.UserShapes.X + chart.PlotArea.ActualX + (actualBarwidth / 2) - (shapeTriangle.Width / 2);
                    shapeTriangle.X = chart.UserShapes.X + chart.PlotArea.ActualX + (actualBarwidth *((float)4-0.5f)) - (shapeTriangle.Width / 2);
                    shapeTriangle.Y = chart.UserShapes.Y + chart.PlotArea.ActualY + chart.PlotArea.ActualHeight - 50;
                    IShape shapeTriangle2 = sld.Shapes.FirstOrDefault(x => x.Name == "redTriangle");
                    shapeTriangle2.X = chart.UserShapes.X + chart.PlotArea.ActualX + (actualBarwidth * ((float)metriclist.Count - 0.5f)) - (shapeTriangle2.Width / 2);
                    shapeTriangle2.Y = chart.UserShapes.Y + chart.PlotArea.ActualY + chart.PlotArea.ActualHeight - 50;*/
                }



            }
        }

        private void prepareSlideSix(System.Data.DataTable dataTable, ISlide cur_Slide, string retailer)
        {
            if (dataTable.Rows.Count != 0)
            {
                IAutoShape channelShp = (IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "channelTextBox");
                channelShp.TextFrame.Text = channelShp.TextFrame.Text.Replace("Convenience", dataTable.Rows[0]["channel"].ToString());
                foreach (DataRow row in dataTable.Rows)
                {

                    switch (row["TimePeriodType"].ToString())
                    {
                        case "monthly+":
                            try
                            {
                                prepareSlideSixItem((IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "monthlyPlus"), row, retailer);
                            }
                            catch (Exception ex) { }
                            break;
                        case "quarterly+":
                            try
                            {
                                prepareSlideSixItem((IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "quarterlyPlus"), row, retailer);
                            }
                            catch (Exception ex) { }
                            break;
                        case "yearly+":
                            try
                            {
                                prepareSlideSixItem((IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "yearlyPlus"), row, retailer);
                            }
                            catch (Exception ex) { }
                            break;
                        case "Visits":
                            try
                            {
                                prepareSlideSixItem((IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "visit"), row, retailer);
                            }
                            catch (Exception ex) { }
                            break;
                        default:
                            try
                            {
                                IAutoShape txtFrame = (IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "channel");
                                txtFrame.TextFrame.Text = txtFrame.TextFrame.Text.Replace("_channel_", row["TimePeriodType"].ToString());
                            }
                            catch (Exception ex)
                            {

                            }
                            break;
                    }
                }
            }
        }
        private void prepareSlideSixItem(IGroupShape currShape, DataRow row, string retailer)
        {
            int samplesize = 0;
            decimal percentage = 1.5M;
            int.TryParse(row["RetailerSample"].ToString(), out samplesize);
            decimal.TryParse(row["Percentage"].ToString(), out percentage);
            ((IAutoShape)currShape.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text = replaceText(((IAutoShape)currShape.Shapes.FirstOrDefault(x => x.Name == "text")).TextFrame.Text, "_retailer_", retailer);
            ((IAutoShape)currShape.Shapes.FirstOrDefault(x => x.Name == "samplesize")).TextFrame.Text = String.Format("{0:n0}", samplesize);
            ((IAutoShape)currShape.Shapes.FirstOrDefault(x => x.Name == "percentage")).TextFrame.Text = Math.Round(percentage, 1) + "%";
        }

        private System.Data.DataTable GetTableOutputPPT(FilterPanelInfo leftPanelData, string spName, bool isUseCustomName, int slideId, HttpContextBase context)
        {
            object[] param = new object[6];
            string[] paramId = new string[6];
            SqlDbType[] paramType = new SqlDbType[6];

            //spName = "USP_BrefingBook_SampleSizeValidation";
            param[0] = leftPanelData.TimeperiodID;
            paramId[0] = "@TimePeriod";
            paramType[0] = SqlDbType.VarChar;

            #region datatable for @selections parameter
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("UniqueId", typeof(Int32));
            dt.Columns.Add("ChannelId", typeof(Int32));
            dt.Columns.Add("Channel", typeof(string));
            dt.Columns.Add("SelectionType", typeof(string));
            DataRow benchmarkRow = dt.NewRow();
            benchmarkRow[0] = leftPanelData.BenchMark.UniqueFilterId;
            benchmarkRow[1] = leftPanelData.BenchMark.ID;
            benchmarkRow[2] = leftPanelData.BenchMark.Name;
            benchmarkRow[3] = leftPanelData.BenchMark.selectionType;
            dt.Rows.Add(benchmarkRow);

            for (int i = 0; i < leftPanelData.CustomBase.Count; i++)
            {
                DataRow dRow = dt.NewRow();
                dRow[0] = leftPanelData.CustomBase[i].UniqueFilterId;
                dRow[1] = leftPanelData.CustomBase[i].ID;
                dRow[2] = leftPanelData.CustomBase[i].Name;
                dRow[3] = leftPanelData.CustomBase[i].selectionType + (isUseCustomName ? "" : (i + 1).ToString());
                dt.Rows.Add(dRow);
            }
            param[1] = dt;
            paramId[1] = "@Selections";
            paramType[1] = SqlDbType.Structured;
            #endregion

            if (leftPanelData.Filters != null && leftPanelData.Filters.Count != 0)
            {
                var filterArray = (from filter in leftPanelData.Filters select filter.UniqueFilterId.ToString()).ToArray<String>();
                param[2] = String.Join("|", filterArray);

            }
            paramId[2] = "@AdvancedFilters";
            paramType[2] = SqlDbType.VarChar;
            //var competitorArray = (from com in leftPanelData.Competitors select com.ID.ToString()).ToArray<String>();
            var competitorArray = (from com in leftPanelData.Competitors select com.UniqueFilterId.ToString()).ToArray<String>();
            param[3] = String.Join("|", competitorArray);
            paramId[3] = "@CompetitorsId";
            paramType[3] = SqlDbType.VarChar;

            param[4] = leftPanelData.IsTripsOrShopper;
            paramId[4] = "@IsTripsorShopper";
            paramType[4] = SqlDbType.Bit;

            if (leftPanelData.Frequency.Count != 0)
            {
                if (slideId <= 13)
                {
                    param[5] = leftPanelData.Frequency.Where(x => x.selectionType == "Who is my Core shopper?").FirstOrDefault().FrequencyId.Equals("1000") ? DBNull.Value : leftPanelData.Frequency.Where(x => x.selectionType == "Who is my Core shopper?").FirstOrDefault().FrequencyId;
                }
                else if (slideId <= 22)
                {
                    param[5] = leftPanelData.Frequency.Where(x => x.selectionType == "path to purchase and trip details").FirstOrDefault().FrequencyId.Equals("1000") ? DBNull.Value : leftPanelData.Frequency.Where(x => x.selectionType == "path to purchase and trip details").FirstOrDefault().FrequencyId;
                }
                else if (slideId <= 30)
                {
                    param[5] = leftPanelData.Frequency.Where(x => x.selectionType == "Strength and oppurtunities").FirstOrDefault().FrequencyId.Equals("1000") ? DBNull.Value : leftPanelData.Frequency.Where(x => x.selectionType == "Strength and oppurtunities").FirstOrDefault().FrequencyId;
                }
                else
                {
                    param[5] = leftPanelData.Frequency.Where(x => x.selectionType == "Beverege section").FirstOrDefault().FrequencyId.Equals("1000") ? DBNull.Value : leftPanelData.Frequency.Where(x => x.selectionType == "Beverege section").FirstOrDefault().FrequencyId;
                }
                //param[5] = leftPanelData.Frequency.FirstOrDefault<FilterPanelData>().FrequencyId.Equals("1000") ? DBNull.Value : leftPanelData.Frequency.FirstOrDefault<FilterPanelData>().FrequencyId;
                paramId[5] = "@FrequencyId";
                paramType[5] = SqlDbType.VarChar;
            }
            DataAccess dal = new DataAccess();
            System.Data.DataTable tbl = null;
            //DataSet ds = dal.GetData_WithIdMapping(param, spName);
            try
            {
                Exception ex = null;
                DataSet ds = dal.GetData(param, paramId, paramType, spName, out ex);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dset = ds;
                    }
                    tbl = ds.Tables[0];
                }
                else if (ex != null)
                {
                    ErrorLogSar.LogError(ex.Message + "   --- Error in fetching Data  spName=" + spName, ex.StackTrace, context);
                }
            }
            catch (Exception ex)
            {
                ErrorLogSar.LogError(ex.Message + "   --- Error in fetching Data  spName=" + spName, ex.StackTrace, context);
                //ErrorLog.LogError(ex.Message + "   --- Error in fetching Data  spName="+spName, ex.StackTrace);
            }

            return tbl;
        }
        public string getUniqueConst()
        {
            return (DateTime.Now.Ticks.ToString());
        }
        public static string FileNamingConventn(string filename)
        {
            string fileNamingConventn = "";
            fileNamingConventn = "Ishop " + filename + "_" + System.DateTime.Now.DayOfWeek.ToString().Substring(0, 3) + " " + System.DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + System.DateTime.Today.Day + " " + System.DateTime.Today.Year + "" + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
            return fileNamingConventn;
        }
        public void imageReplace(PictureFrame tempImg, string loc, HttpContextBase context, int indx, string subPath)
        {
            string pathToSvg = context.Server.MapPath(loc);
            double ratio = 1;
            double widthRatio = tempImg.Width / tempImg.Height;
            //Create a directory            
            bool exists = System.IO.Directory.Exists(context.Server.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(context.Server.MapPath(subPath));
            string tempPath = context.Server.MapPath(subPath + "/img" + indx + ".emf");
            if (System.IO.File.Exists(pathToSvg))
            {
                var xyz = SvgDocument.Open(pathToSvg);
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                var svgBounds = true;
                try
                {
                    var svgBounds1 = xyz.Bounds;
                }
                catch (Exception ex)
                {
                    svgBounds = false;
                }
                if (svgBounds)
                {
                    if (xyz.ViewBox.Width > xyz.ViewBox.Height)
                    {
                        ratio = xyz.ViewBox.Width / xyz.ViewBox.Height;
                        xyz.Height = (int)((4 * ratio) * xyz.Bounds.Height);
                        xyz.Width = (int)((4 * widthRatio) * xyz.Bounds.Width);
                    }
                    else
                    {
                        if (xyz.ViewBox.Width < xyz.ViewBox.Height)
                        {
                            ratio = xyz.ViewBox.Height / xyz.ViewBox.Width;
                            xyz.Width = (int)((4 * ratio * widthRatio) * xyz.Bounds.Width);
                            xyz.Height = 4 * xyz.Bounds.Height;
                        }
                        else
                        {
                            xyz.Height = 4 * xyz.Bounds.Height;
                            xyz.Width = (int)((4 * widthRatio) * xyz.Bounds.Width);
                        }

                    }

                }
                xyz.Draw().Save(tempPath, System.Drawing.Imaging.ImageFormat.Emf);
                using (Image img = Image.FromFile(tempPath, true))
                {
                    tempImg.PictureFormat.Picture.Image.ReplaceImage(img);
                }
            }
            else
            {
                //Remove the Image holder
                tempImg.Hidden = true;
            }
        }

        public void imageReplaceV2(PictureFrame tempImg, string loc, HttpContextBase context, int indx, string subPath, Aspose.Slides.ITable tbl, int row, int column)
        {
            string pathToSvg = context.Server.MapPath(loc);
            double ratio = 1;
            double widthRatio = tempImg.Width / tempImg.Height;
            double x1Pos = tbl[column, row].OffsetX + tbl.X;
            double y1Pos = tbl[column, row].OffsetY + tbl.Y;
            double x2Pos = tbl[column, row].OffsetX + tbl[column, row].Width + tbl.X;
            double y2Pos = tbl[column, row].OffsetY + tbl[column, row].Height + tbl.Y;
            //Create a directory            
            bool exists = System.IO.Directory.Exists(context.Server.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(context.Server.MapPath(subPath));
            string tempPath = context.Server.MapPath(subPath + "/img" + indx + ".emf");
            if (System.IO.File.Exists(pathToSvg))
            {
                var xyz = SvgDocument.Open(pathToSvg);
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                var svgBounds = true;
                try
                {
                    var svgBounds1 = xyz.Bounds;
                }
                catch (Exception ex)
                {
                    svgBounds = false;
                }
                if (svgBounds)
                {

                    /* if (xyz.Bounds.Width > xyz.Bounds.Height)
                     {
                         double oldWidth = xyz.Bounds.Width;
                         xyz.Width = (int)tempImg.Width;
                         xyz.Height = (int)(((float)tempImg.Width / (float)oldWidth) * xyz.Bounds.Height);
                         //ratio = xyz.ViewBox.Width / xyz.ViewBox.Height;
                         //xyz.Height = (int)((4 * ratio) * xyz.Bounds.Height);
                         //xyz.Width = (int)((4 * widthRatio) * xyz.Bounds.Width);

                     }
                     else
                     {
                         if (xyz.Bounds.Width < xyz.Bounds.Height)
                         {
                             double oldHeight = xyz.Bounds.Height;
                             xyz.Height = (int)tempImg.Height;
                             xyz.Width = (int)(((float)tempImg.Height / (float)oldHeight) * xyz.Bounds.Width);
                             //ratio = xyz.ViewBox.Height / xyz.ViewBox.Width;
                             //xyz.Width = (int)((4 * ratio * widthRatio) * xyz.Bounds.Width);
                             //xyz.Height = 4 * xyz.Bounds.Height;
                         }
                         else
                         {
                             xyz.Height = (int)tempImg.Height;
                             xyz.Width = (int)tempImg.Width;
                             //xyz.Height = 4 * xyz.Bounds.Height;
                             //xyz.Width = (int)((4 * widthRatio) * xyz.Bounds.Width);
                         }

                     }
                     tempImg.Width = xyz.Width.Value;
                     tempImg.Height = xyz.Height.Value;
                     double width = tempImg.Width;
                     double height = tempImg.Height;

                     if (width < height)
                     {
                         tempImg.X = (int)(x1Pos + ((x2Pos - x1Pos - width) / 2));
                     }
                     else if (width > height)
                     {
                         tempImg.Y = (int)(y1Pos + ((y2Pos - y1Pos - height) / 2));
                     }*/
                }
                xyz.Draw().Save(tempPath, System.Drawing.Imaging.ImageFormat.Emf);
                using (Image img = Image.FromFile(tempPath, true))
                {
                    tempImg.PictureFormat.Picture.Image.ReplaceImage(img);

                }
            }
            else
            {
                //Remove the Image holder
                tempImg.Hidden = true;
            }
        }

        public void imageReplaceNew(PictureFrame tempImg, string loc, HttpContextBase context, int indx, string subPath)
        {
            string path = context.Server.MapPath(loc);
            double ratio = 1;
            double widthRatio = tempImg.Width / tempImg.Height;
            //Create a directory            
            bool exists = System.IO.Directory.Exists(context.Server.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(context.Server.MapPath(subPath));
            string tempPath = context.Server.MapPath(subPath + "/img" + indx + ".emf");
            if (System.IO.File.Exists(path))
            {
                var xyz = Image.FromFile(path);
                Bitmap myImage = new Bitmap(xyz);
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                var svgBounds = true;
                try
                {
                    var unitGraphics = GraphicsUnit.Pixel;
                    var svgBounds1 = xyz.GetBounds(ref unitGraphics);
                }
                catch (Exception ex)
                {
                    svgBounds = false;
                }
                float newWidth = 0.0f, newHeight = 0.0f;
                if (svgBounds)
                {
                    if (myImage.Width > myImage.Height)
                    {
                        ratio = myImage.Width / myImage.Height;
                        newHeight = (int)((4 * ratio) * myImage.Height);
                        newWidth = (int)((4 * widthRatio) * myImage.Width);
                    }
                    else
                    {
                        if (myImage.Width < myImage.Height)
                        {
                            ratio = myImage.Height / myImage.Width;
                            newWidth = (int)((4 * ratio * widthRatio) * myImage.Width);
                            newHeight = 4 * myImage.Height;
                        }
                        else
                        {
                            newHeight = 4 * myImage.Height;
                            newWidth = (int)((4 * widthRatio) * myImage.Width);
                        }

                    }

                }
                Bitmap b = new Bitmap((int)newWidth, (int)newHeight);
                Graphics g = Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // Draw image with new width and height  
                g.DrawImage(myImage, 0, 0, newWidth, newHeight);
                b.Save(tempPath, System.Drawing.Imaging.ImageFormat.Emf);
                using (Image img = Image.FromFile(tempPath, true))
                {
                    tempImg.PictureFormat.Picture.Image.ReplaceImage(img);
                }
                myImage.Dispose();
                g.Dispose();
                xyz.Dispose();
            }
            else
            {
                //Remove the Image holder
                tempImg.Hidden = true;
            }
        }
        public string getCleanedRetailerForImage(string name, string replace)
        {
            Regex regex = new Regex("[&/\\#,+()$~%.':*?<>{}]", RegexOptions.IgnoreCase);
            return regex.Replace(name, replace);
        }
        public string replaceText(string text, string find, string replace)
        {
            return text.Replace(find, replace);
        }
        public void updateConnector(IConnector conn, string color, string type)
        {
            if (type == "solid")
            {
                conn.LineFormat.FillFormat.FillType = FillType.Solid;
                conn.LineFormat.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(color);
            }
            else if (type == "gradient")
            {
                conn.LineFormat.FillFormat.FillType = FillType.Gradient;
                conn.LineFormat.FillFormat.GradientFormat.GradientStops.Clear();
                conn.LineFormat.FillFormat.GradientFormat.GradientStops.Add(0, ColorTranslator.FromHtml(color));
                conn.LineFormat.FillFormat.GradientFormat.GradientStops.Add(1, ColorTranslator.FromHtml(color));
                conn.LineFormat.FillFormat.GradientFormat.GradientStops.Add(0.5f, Color.White);
            }
        }
        public void updateIconColor(IShape shape, string ColorCode)
        {
            //shape.FillFormat.FillType = FillType.Solid;
            IChart chart = (IChart)shape;
            IChartSeries Series = chart.ChartData.Series[0];
            Series.Format.Fill.FillType = FillType.Solid;
            Series.Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(ColorCode);
            //shape.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(ColorCode);
        }
        public void updateDonutTwoPointsWithLabel(IChart donutChart1, double point1, double point2, string selectedColorCode)
        {
            donutChart1.ChartData.Series[0].DataPoints[0].Value.Data = point1;

            donutChart1.ChartData.Series[0].DataPoints[0].Format.Fill.FillType = FillType.Solid;
            donutChart1.ChartData.Series[0].DataPoints[0].Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(selectedColorCode);
            donutChart1.ChartData.Series[0].DataPoints[1].Value.Data = point2;
        }
        private void updateDonutfourPointsWithLabel(IChart donutChart1, System.Data.DataTable dt, List<Color> colorCode)
        {
            for (int row = 0; row < 4; row++)
            {
                donutChart1.ChartData.Series[0].DataPoints[row].Value.Data = dt.Rows[row]["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(dt.Rows[row]["MetricPercentage"]);
                donutChart1.ChartData.Categories[row].Value = dt.Rows[row]["Metric"];
            }

            donutChart1.ChartData.Series[0].DataPoints[0].Format.Fill.FillType = FillType.Solid;
            donutChart1.ChartData.Series[0].DataPoints[0].Format.Fill.SolidFillColor.Color = (colorCode[0]);
            donutChart1.ChartData.Series[0].DataPoints[1].Format.Fill.FillType = FillType.Solid;
            donutChart1.ChartData.Series[0].DataPoints[1].Format.Fill.SolidFillColor.Color = (colorCode[1]);
            donutChart1.ChartData.Series[0].DataPoints[2].Format.Fill.FillType = FillType.Solid;
            donutChart1.ChartData.Series[0].DataPoints[2].Format.Fill.SolidFillColor.Color = (colorCode[2]);
            donutChart1.ChartData.Series[0].DataPoints[3].Format.Fill.FillType = FillType.Solid;
            donutChart1.ChartData.Series[0].DataPoints[3].Format.Fill.SolidFillColor.Color = (colorCode[3]);


        }
        //public Color getSigcolor(double? cbsig)
        //{
        //    if (cbsig == 0.0) return Color.Black;
        //    if (cbsig < -1.96) return Color.Green;
        //    if (cbsig > 1.96) return Color.Red;
        //    return Color.Black;
        //}
        public Color getSigcolorNormal(double? cbsig)
        {
            if (cbsig == 0.0) return Color.Black;
            if (cbsig < -1.96) return Color.Red;
            if (cbsig > 1.96) return Color.Green;
            return Color.Black;
        }
        public int GetSigColorNormalVal(double? cbsig)
        {
            if (cbsig == 0.0) return 0;
            if (cbsig < -1.96) return -1;
            if (cbsig > 1.96) return 1;
            return 0;
        }
        public Color getSigcolor(double? cbsig)
        {
            if (cbsig == 0.0) return Color.Black;
            if (cbsig < -1.96) return Color.Green;
            if (cbsig > 1.96) return Color.Red;
            return Color.Black;
        }
        public void updateSARportIndexTable(ISlide sld, System.Data.DataTable tbl, string tableName, string metricName, int textCol, int valueCol, int index1Col, int index2Col, int rowstart, int colStart)
        {
            var aspose_tbl = (AsposeSlide.ITable)sld.Shapes.Where(x => x.Name == tableName).FirstOrDefault();
            var metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r[metricName])).Distinct().ToList();
            int rowCount = rowstart;
            double metricPercentage = 0.0;
            double sign1 = 0.0, sign2 = 0.0;

            if (aspose_tbl != null)
            {

                foreach (string metric in metriclist)
                {
                    int k = rowCount;
                    rowCount++;
                    for (int j = colStart; j < aspose_tbl.Columns.Count; j++)
                    {
                        if (!(j == textCol || j == valueCol || j == index1Col | j == index2Col))
                            continue;
                        //aspose_tbl[j, k].FillFormat.FillType = Aspose.Slides.FillType.Solid;
                        //aspose_tbl[j, k].FillFormat.SolidFillColor.Color = Color.White;

                        var query = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(r.Field<object>(metricName)).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                     select new
                                     {
                                         MetricPercentage = Convert.ToString(r.Field<object>("MetricPercentage")),
                                         CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? "NA" : r["CB1Index"].ToString(),
                                         CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? "NA" : r["CB2Index"].ToString(),
                                         MetricName = r["Metric"].ToString(),
                                         CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                         CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                     }).FirstOrDefault();
                        if (!double.TryParse(Convert.ToString(query.MetricPercentage), out metricPercentage)) metricPercentage = 0.0;
                        if (!double.TryParse(Convert.ToString(query.CB1Sig), out sign1)) sign1 = 0.0;
                        if (!double.TryParse(Convert.ToString(query.CB2Sig), out sign2)) sign2 = 0.0;
                        if (j == textCol)
                        {
                            aspose_tbl[j, k].TextFrame.Text = query.MetricName;
                        }
                        else if (j == valueCol)
                        {
                            aspose_tbl[j, k].TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                            aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
                        }
                        else if (j == index1Col)
                        {
                            aspose_tbl[j, k].TextFrame.Text = query.CustomBaseIndex1;
                            aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);
                        }
                        else if (j == index2Col)
                        {
                            aspose_tbl[j, k].TextFrame.Text = query.CustomBaseIndex2;
                            aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[j, k].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);
                        }

                    }
                }
            }
        }

        public void updateSARportIndexTableRow(ISlide sld, System.Data.DataTable tbl, string tableName, string metricName, int textRow, int valueRow, int index1Row, int index2Row, int rowStart, int colStart)
        {
            var aspose_tbl = (AsposeSlide.ITable)sld.Shapes.Where(x => x.Name == tableName).FirstOrDefault();
            var metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r[metricName])).Distinct().ToList();
            int colCount = colStart;
            double metricPercentage = 0.0;
            double sign1 = 0.0, sign2 = 0.0;

            if (aspose_tbl != null)
            {

                foreach (string metric in metriclist)
                {
                    int k = colCount;
                    colCount++;
                    for (int j = rowStart; j < aspose_tbl.Rows.Count; j++)
                    {
                        if (!(j == textRow || j == valueRow || j == index1Row | j == index2Row))
                            continue;
                        //aspose_tbl[j, k].FillFormat.FillType = Aspose.Slides.FillType.Solid;
                        //aspose_tbl[j, k].FillFormat.SolidFillColor.Color = Color.White;

                        var query = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(r.Field<object>(metricName)).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                     select new
                                     {
                                         MetricPercentage = Convert.ToString(r.Field<object>("MetricPercentage")),
                                         CustomBaseIndex1 = r["CB1Index"] == DBNull.Value ? "NA" : r["CB1Index"].ToString(),
                                         CustomBaseIndex2 = r["CB2Index"] == DBNull.Value ? "NA" : r["CB2Index"].ToString(),
                                         MetricName = r["Metric"].ToString(),
                                         CB1Sig = r["CB1Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB1Significance"]),
                                         CB2Sig = r["CB2Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["CB2Significance"])
                                     }).FirstOrDefault();
                        if (!double.TryParse(Convert.ToString(query.MetricPercentage), out metricPercentage)) metricPercentage = 0.0;
                        if (!double.TryParse(Convert.ToString(query.CB1Sig), out sign1)) sign1 = 0.0;
                        if (!double.TryParse(Convert.ToString(query.CB2Sig), out sign2)) sign2 = 0.0;
                        if (j == textRow)
                        {
                            aspose_tbl[k, j].TextFrame.Text = query.MetricName;
                        }
                        else if (j == valueRow)
                        {
                            aspose_tbl[k, j].TextFrame.Text = Convert.ToDouble(metricPercentage * 100).ToString("#0.0") + "%";
                            aspose_tbl[k, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[k, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
                        }
                        else if (j == index1Row)
                        {
                            aspose_tbl[k, j].TextFrame.Text = query.CustomBaseIndex1;
                            aspose_tbl[k, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[k, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign1);
                        }
                        else if (j == index2Row)
                        {
                            aspose_tbl[k, j].TextFrame.Text = query.CustomBaseIndex2;
                            aspose_tbl[k, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[k, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigcolor(sign2);
                        }

                    }
                }
            }
        }
        public void updateStackBarChartData(System.Data.DataTable tbl, ISlide sld, string chart_shape_name, string colorCode)
        {
            if (tbl.Rows.Count > 0)
            {
                IChart chart;
                IChartSeries Series;
                IDataLabel lbl;
                double metricPercentage = 0.0, sign = 0.0;
                double xAxis_fact = 1, xAxis_fact1 = 1, additionalFact = 0.2;
                int serCount = 1;

                chart = (IChart)sld.Shapes.FirstOrDefault(x => x.Name == chart_shape_name);
                int defaultWorksheetIndex = 0;

                //Getting the chart data worksheet
                IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;
                int seriesCount = chart.ChartData.Series.Count;
                int categoryCount = tbl.Rows.Count;
                fact.GetCell(0, 0, 1, tbl.Rows[0]["Retailer"]);

                for (int k = 0; k < tbl.Rows.Count; k++)
                {
                    metricPercentage = 0.0;
                    DataRow currentRow = tbl.Rows[k];
                    chart.ChartData.Categories[k].Value = Convert.ToString(Convert.ToString(currentRow.Field<object>("Metric")));
                    if (!double.TryParse(Convert.ToString(Convert.ToString(currentRow.Field<object>("MetricPercentage"))), out metricPercentage)) metricPercentage = 0.0;
                    double tempMetricPercentage = Convert.ToDouble(metricPercentage > 1 ? metricPercentage / 100 : metricPercentage);
                    chart.ChartData.Series[0].DataPoints[k].Value.Data = tempMetricPercentage;
                    chart.ChartData.Series[1].DataPoints[k].Value.Data = 1 - tempMetricPercentage;
                    Series = chart.ChartData.Series[0];

                    //if (serCount == 0)
                    //{
                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(colorCode);
                    //}
                }

            }

        }
        public void updateStackBarChartSingleSeriesData(System.Data.DataTable tbl, IGroupShape sld, string chart_shape_name1, string chart_shape_name2, string colorCode)
        {
            if (tbl.Rows.Count > 0)
            {
                IChart chart1, chart2;
                IChartSeries Series1, Series2;
                IDataLabel lbl;
                double metricPercentage = 0.0, sign = 0.0;
                int CB1Index, CB2Index;
                double xAxis_fact = 1, xAxis_fact1 = 1, additionalFact = 0.2;
                int serCount = 1;

                chart1 = (IChart)sld.Shapes.FirstOrDefault(x => x.Name == chart_shape_name1);
                chart2 = (IChart)sld.Shapes.FirstOrDefault(x => x.Name == chart_shape_name2);
                IChartDataWorkbook fact1 = chart1.ChartData.ChartDataWorkbook;
                IChartDataWorkbook fact2 = chart2.ChartData.ChartDataWorkbook;
                chart1.ChartData.Series[0].GetAutomaticSeriesColor();
                int defaultWorksheetIndex = 0;


                for (int k = 0; k < tbl.Rows.Count; k++)
                {
                    sign = 0.0;
                    DataRow currentRow = tbl.Rows[k];
                    CB1Index = currentRow.Field<object>("CB1Index") == DBNull.Value ? 0 : (int)currentRow.Field<object>("CB1Index");

                    //CB2Index = Convert.ToString(currentRow.Field<object>("CB2Index") == DBNull.Value ? "0" : (currentRow.Field<object>("CB2Index").ToString()));

                    CB2Index = currentRow.Field<object>("CB2Index") == DBNull.Value ? 0 : (int)currentRow.Field<object>("CB2Index");




                    chart1.ChartData.Series[0].DataPoints[k].Value.Data = CB1Index;
                    //fact1.GetCell(0, 1, 2, 100 - CB1Index);
                    chart1.ChartData.Series[1].DataPoints[k].Value.Data = 100 - CB1Index;

                    chart2.ChartData.Series[0].DataPoints[k].Value.Data = CB2Index;
                    //fact2.GetCell(0, 1, 2, 100 - CB2Index);
                    chart2.ChartData.Series[1].DataPoints[k].Value.Data = 100 - CB2Index;
                    Series1 = chart1.ChartData.Series[0];
                    Series1.Format.Fill.FillType = FillType.Solid;
                    Series1.Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(colorCode);
                    Series1.DataPoints[0].Format.Fill.FillType = FillType.Solid;
                    Series1.DataPoints[0].Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(colorCode);
                    Series1.Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(colorCode);


                    //if (serCount == 0)
                    //{

                    //}
                }

            }

        }

        public void updateStackBarChartTwoSeriesData(System.Data.DataTable tbl, ISlide sld, string chart_shape_name, string colorCode, bool editTitle)
        {
            if (tbl.Rows.Count > 0)
            {
                IChart chart1;
                IChartSeries Series1, Series2;
                IDataLabel lbl;
                double metricPercentage = 0.0, sign = 0.0;
                double CB1Index, CB2Index;
                double xAxis_fact = 1, xAxis_fact1 = 1, additionalFact = 0.2;
                int serCount = 1;

                chart1 = (IChart)sld.Shapes.FirstOrDefault(x => x.Name == chart_shape_name);
                IChartDataWorkbook fact1 = chart1.ChartData.ChartDataWorkbook;
                chart1.ChartData.Series[0].GetAutomaticSeriesColor();
                int defaultWorksheetIndex = 0;
                if (editTitle)
                    chart1.ChartTitle.TextFrameForOverriding.Text = tbl.AsEnumerable().Select(x => x.Field<string>("Retailer")).FirstOrDefault();


                for (int k = 0; k < tbl.Rows.Count; k++)
                {
                    sign = 0.0;
                    DataRow currentRow = tbl.Rows[k];
                    chart1.ChartData.Categories[k].Value = Convert.ToString(currentRow.Field<object>("Metric"));
                    CB1Index = currentRow.Field<object>("ShopperPercentage") == DBNull.Value ? 0.0 : (double)currentRow.Field<object>("ShopperPercentage");

                    //CB2Index = Convert.ToString(currentRow.Field<object>("CB2Index") == DBNull.Value ? "0" : (currentRow.Field<object>("CB2Index").ToString()));

                    CB2Index = currentRow.Field<object>("TripsPercentage") == DBNull.Value ? 0.0 : (double)currentRow.Field<object>("TripsPercentage");




                    chart1.ChartData.Series[0].DataPoints[k].Value.Data = CB1Index;
                    chart1.ChartData.Series[1].DataPoints[k].Value.Data = CB2Index;

                    Series1 = chart1.ChartData.Series[0];
                    Series1.Format.Fill.FillType = FillType.Solid;
                    Series1.Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(colorCode);
                    Series1.DataPoints[k].Format.Fill.FillType = FillType.Solid;
                    Series1.DataPoints[k].Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(colorCode);
                }

            }

        }
        public void updateStackedBarChart(ISlide sld, System.Data.DataTable tbl, string chart_shape_name, string selectedColorCode, HttpContextBase context, string subPath, int slideId)
        {
            IChart chart;
            IDataLabel lbl;
            IGroupShape tempGroup;
            IAutoShape tempShape;
            double metricPercentage = 0.0, gap = 0.0;
            chart = (IChart)sld.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();
            //for (var i = 0; i < tbl.Rows.Count; i++)
            //{
            int i = 0;
            int defaultWorksheetIndex = 0;

            chart.ChartData.Categories.Clear();
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;
            //chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, 0, 0, ""));
            var metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r["Metric"])).Distinct().ToList();
            foreach (string metric in metriclist)
            {
                tempGroup = (IGroupShape)cur_Slide.Shapes.Where(x => x.Name == "Group" + (i + 1)).FirstOrDefault();
                var query = (from r in tbl.AsEnumerable()
                             where Convert.ToString(r.Field<object>("Metric")).Equals(metric, StringComparison.OrdinalIgnoreCase)
                             select new
                             {
                                 MetricPercentage = Convert.ToString(r.Field<object>("MetricPercentage")),
                                 BICGap = Convert.ToString(r.Field<object>("BICGap")),
                                 BICRating = Convert.ToString(r.Field<object>("BICPercentage")),
                                 BICEstablishmentName = Convert.ToString(r.Field<object>("BICRetailer")),
                                 //SignificanceColorFlag = Convert.ToString(r.Field<object>("SignificanceColorFlag"))
                             }).FirstOrDefault();
                if (!double.TryParse(Convert.ToString(query.MetricPercentage), out metricPercentage)) metricPercentage = 0.0;
                if (!double.TryParse(Convert.ToString(query.BICGap), out gap)) gap = 0.0;

                chart.ChartData.ChartDataWorkbook.GetCell(0, i + 1, 1, query.BICEstablishmentName);



                chart.ChartData.Series[0].DataPoints[i].Value.Data = metricPercentage;
                chart.ChartData.Series[1].DataPoints[i].Value.Data = gap;

                chart.ChartData.Series[0].Name.AsCells[0].Value = Convert.ToString(tbl.Rows[0]["Retailer"]);
                //chart.ChartData.Series[i].DataPoints[0].Value.Data = "Metric";
                chart.ChartData.Series[0].Format.Fill.FillType = FillType.Solid;
                chart.ChartData.Series[0].Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(selectedColorCode);
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
                tempShape = (IAutoShape)tempGroup.Shapes.Where(x => x.Name == "Rectangle").FirstOrDefault();
                tempShape.TextFrame.Text = (query.BICRating == "") ? "" : (Convert.ToDouble(query.BICRating) * 100).ToString("#0.0") + "%";
                chart.ChartData.ChartDataWorkbook.GetCell(0, i + 1, 4, (query.BICRating == "") ? 0 : (Convert.ToDouble(query.BICRating)));
                tempShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                //tempShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getSigColorRating(query.SignificanceColorFlag);
                string cleanedBenchmark = getCleanedRetailerForImage(query.BICEstablishmentName, "-");
                var imgLoc = "~/Images/P2PSarReport/" + cleanedBenchmark + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "img").FirstOrDefault(), imgLoc, context, 8000 + slideId + i + 1, subPath);
                i++;
            }
        }
        public Color getSigColorRating(string significanceColorFlag)
        {
            if (significanceColorFlag == "1")
                return Color.Green;
            else if (significanceColorFlag == "2")
                return Color.Blue;
            else if (significanceColorFlag == "3")
                return Color.Black;
            return Color.Black;
        }
        public void prepareSlide15(System.Data.DataTable dataTable, ISlide cur_Slide, FilterPanelInfo filter, HttpContextBase context, string subPath)
        {
            double metricPercentage = 0.0, point1 = 0.0, point2 = 0.0;
            #region weekday or weekend
            IGroupShape grpWeekday = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Weekday");
            var query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("WEEKDAY OR WEEKEND", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Flag")).Equals("2", StringComparison.OrdinalIgnoreCase)).OrderBy(r => r.Field<object>("Metric")).Take(2);
            var tempRow = (from r in query.AsEnumerable()
                           select new
                           {
                               MetricName = r["Metric"].ToString(),
                               MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                               change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                               Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                           }).ToList();
            slide15ChangeData(tempRow, grpWeekday, "vm", "vm", "c", "", true, true, false, true, false, "", context, subPath);
            //if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            //int weekMetricPercentage = (int)Math.Round(metricPercentage, 2)*100;
            //if (!double.TryParse(tempRow[1].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            //int weekendMetricPercentage = (int)Math.Round(metricPercentage, 2) * 100;
            //((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text= ((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text.Replace("_mp_",weekMetricPercentage+"%");
            //((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text = tempRow[0].change;
            //((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm1")).FillFormat.SolidFillColor.Color = getSigcolor(tempRow[0].Significance);
            //((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm2")).TextFrame.Text = ((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text.Replace("_mp_", weekendMetricPercentage + "%");
            //((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm2")).TextFrame.Text = tempRow[1].change;
            //((IAutoShape)grpWeekday.Shapes.FirstOrDefault(x => x.Name == "vm2")).FillFormat.SolidFillColor.Color = getSigcolor(tempRow[1].Significance);
            #endregion

            #region times of the day
            IGroupShape grpTod = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "tod");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("TIME OF DAY", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(5);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            slide15ChangeData(tempRow, grpTod, "m", "v", "c", "", false, true, true, true, false, "", context, subPath);
            //for(int i = 0; i < tempRow.Count; i++)
            //{
            //    if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            //    int tempMetricPercentage = (int)Math.Round(metricPercentage, 2) * 100;
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "m" + (i + 1))).TextFrame.Text = tempRow[i].MetricName;
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "v" + (i + 1))).TextFrame.Text = tempMetricPercentage+"%";
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "c" + (i + 1))).TextFrame.Text = tempRow[i].change;
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "c" + (i + 1))).FillFormat.SolidFillColor.Color = getSigcolor(tempRow[i].Significance);
            //}

            #endregion

            #region location prior to home
            IGroupShape grpLocPrior = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "locationPrior");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("LOCATION PRIOR TO TRIP", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            string loc = "~/Images/ishop-P2P-Icons/Location Prior To Trip/";
            slide15ChangeData(tempRow, grpLocPrior, "vm", "vm", "c", "img", true, true, true, true, true, loc, context, subPath);
            //for (int i = 0; i < tempRow.Count; i++)
            //{
            //    if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            //    int tempMetricPercentage = (int)Math.Round(metricPercentage, 2) * 100;
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "m" + (i + 1))).TextFrame.Text = tempRow[i].MetricName;
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "v" + (i + 1))).TextFrame.Text = tempMetricPercentage + "%";
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "c" + (i + 1))).TextFrame.Text = tempRow[i].change;
            //    ((IAutoShape)grpTod.Shapes.FirstOrDefault(x => x.Name == "c" + (i + 1))).FillFormat.SolidFillColor.Color = getSigcolor(tempRow[i].Significance);
            //}

            #endregion


            #region planningType
            IGroupShape grpPlanning = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "planningType");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("PLANNING", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Planning/";
            slide15ChangeData(tempRow, grpPlanning, "vm", "vm", "c", "img", true, true, true, true, true, loc, context, subPath);

            #endregion

            #region Preparation Types
            IGroupShape grpreparation = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Preparation Types");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("Preparation Types", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Planning/";
            slide15ChangeData(tempRow, grpreparation, "vm", "vm", "c", "img", true, true, true, true, false, loc, context, subPath);

            #endregion


            #region Consideration
            IGroupShape grpConsideration = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Consideration");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("CONSIDERATION", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Planning/";
            slide15ChangeData(tempRow, grpConsideration, "vm", "vm", "c", "img", true, true, true, true, false, loc, context, subPath);
            if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
            point1 = metricPercentage;
            point2 = 1 - point1;
            updateDonutTwoPointsWithLabel((IChart)grpConsideration.Shapes.FirstOrDefault(x => x.Name == "chart"), point1, point2, filter.ColorCode);


            #endregion


            #region Reason for store choice
            IGroupShape grpReasonStore = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "Reason for store choice");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("REASON FOR STORE CHOICE", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Planning/";
            slide15ChangeData(tempRow, grpReasonStore, "vm", "vm", "c", "img", true, true, true, true, false, loc, context, subPath);



            #endregion

            #region whoWith
            IGroupShape grpWho = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "whoWith");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("WHO WITH", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Who With/";
            slide15ChangeData(tempRow, grpWho, "vm", "vm", "c", "img", true, true, true, true, true, loc, context, subPath);



            #endregion

            #region destinationItems
            IGroupShape grpDestItem = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "destinationItems");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("DESTINATION ITEM", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(3);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Destination Item/";
            slide15ChangeData(tempRow, grpDestItem, "vm", "vm", "c", "img", true, true, true, true, true, loc, context, subPath);



            #endregion



            #region tripMission
            IGroupShape grptripMission = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "tripMission");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("TRIP MISSION", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(4);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Trip Mission/";
            slide15ChangeData(tempRow, grptripMission, "m", "v", "c", "img", false, true, true, true, true, loc, context, subPath);
            for (int i = 0; i < tempRow.Count; i++)
            {
                IGroupShape grpBallon = (IGroupShape)grptripMission.Shapes.FirstOrDefault(x => x.Name == "balloon" + (i + 1));
                if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
                point1 = metricPercentage;
                point2 = 1 - point1;
                updateDonutTwoPointsWithLabel((IChart)grpBallon.Shapes.FirstOrDefault(x => x.Name == "chart"), point1, point2, filter.ColorCode);
            }


            #endregion

            #region orderSummary
            IGroupShape grpOrderSummary = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "orderSummary");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("ORDER SUMMARY", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(3);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Top Items/";
            slide15ChangeData(tempRow, grpOrderSummary, "vm", "vm", "c", "img", true, true, true, true, true, loc, context, subPath);

            #endregion


            #region NumberOfItems
            IGroupShape numberOfItems = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "NumberOfItems");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("NUMBER OF ITEMS", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("Average Per Basket", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            double curMetricPercentage = Math.Round(metricPercentage, 3) * 100;
            IPortionFormat Numberformat1 = ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat;
            IPortionFormat Numberformat2 = ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Paragraphs[1].Portions[0].PortionFormat;

            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text = ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text.Replace("_mp_", curMetricPercentage.ToString());
            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text = ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Text.Replace("_metric_", tempRow[0].MetricName);
            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "c1")).TextFrame.Text = (Convert.ToDouble(tempRow[0].change)).ToString("#0.0");
            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "c1")).FillFormat.SolidFillColor.Color = getSigcolorNormal(tempRow[0].Significance);
            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = Numberformat1.FontBold;
            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = Numberformat1.FontHeight;
            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Paragraphs[1].Portions[0].PortionFormat.FontBold = Numberformat2.FontBold;
            ((IAutoShape)numberOfItems.Shapes.FirstOrDefault(x => x.Name == "vm1")).TextFrame.Paragraphs[1].Portions[0].PortionFormat.FontHeight = Numberformat2.FontHeight;





            #endregion

            #region DollarsSpent
            IGroupShape DollarsSpent = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "DollarsSpent");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("DOLLARS SPENT", StringComparison.OrdinalIgnoreCase) && Convert.ToString(r.Field<object>("Metric")).Equals("Average Per Basket", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();

            if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            int dollarMetricPercentage = (int)(Math.Round(metricPercentage, 2) * 100);
            ((IAutoShape)DollarsSpent.Shapes.FirstOrDefault(x => x.Name == "v1")).TextFrame.Text = "$" + dollarMetricPercentage;
            ((IAutoShape)DollarsSpent.Shapes.FirstOrDefault(x => x.Name == "c1")).TextFrame.Text = (Convert.ToDouble(tempRow[0].change)).ToString("#0.0");
            ((IAutoShape)DollarsSpent.Shapes.FirstOrDefault(x => x.Name == "c1")).FillFormat.SolidFillColor.Color = getSigcolorNormal(tempRow[0].Significance);


            #endregion

            #region purchasedNartd
            IGroupShape grpPurchaseNartd = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "purchasedNartd");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("PURCHASED NARTD", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Trip Mission/";
            slide15ChangeData(tempRow, grpPurchaseNartd, "vm", "vm", "c", "img", false, true, false, true, false, loc, context, subPath);
            if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
            point1 = metricPercentage;
            point2 = 1 - point1;
            updateDonutTwoPointsWithLabel((IChart)grpPurchaseNartd.Shapes.FirstOrDefault(x => x.Name == "chart"), point1, point2, filter.ColorCode);


            #endregion

            #region immediateConsumption
            IGroupShape grpImmediateConsump = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "immediateConsumption");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("IMMEDIATE CONSUMPTION-Nets", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Trip Mission/";
            slide15ChangeData(tempRow, grpImmediateConsump, "m", "vm", "c", "img", false, true, true, true, false, loc, context, subPath);
            if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
            point1 = metricPercentage;
            point2 = 1 - point1;
            updateDonutTwoPointsWithLabel((IChart)grpImmediateConsump.Shapes.FirstOrDefault(x => x.Name == "chart"), point1, point2, filter.ColorCode);


            #endregion


            #region timeSpent
            IGroupShape grpTimeSpent = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "timeSpent");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("TIME SPENT", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString().ToUpper(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Trip Mission/";
            slide15ChangeData(tempRow, grpTimeSpent, "m", "vm", "c", "img", false, true, true, true, false, loc, context, subPath);


            #endregion


            #region overallSatisfaction
            IGroupShape grpoverallSatisfaction = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "overallSatisfaction");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("OVERALL SATISFACTION", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Trip Mission/";
            slide15ChangeData(tempRow, grpoverallSatisfaction, "vm", "vm", "c", "img", true, true, true, true, false, loc, context, subPath);
            if (!double.TryParse(tempRow[0].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
            metricPercentage = metricPercentage > 1 ? metricPercentage / 100 : metricPercentage;
            point1 = metricPercentage;
            point2 = 1 - point1;
            updateDonutTwoPointsWithLabel((IChart)grpoverallSatisfaction.Shapes.FirstOrDefault(x => x.Name == "chart"), point1, point2, filter.ColorCode);


            #endregion


            #region satisfaction driver
            IGroupShape grpsatisfaction = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "satisfaction");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("SATISFACTION DRIVERS", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(6);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/Satisfaction Drivers/";
            slide15ChangeData(tempRow, grpsatisfaction, "vm", "vm", "c", "img", true, true, true, true, true, loc, context, subPath);


            #endregion


            #region location after visit
            IGroupShape grpPostVisit = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "postvist");
            query = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("LOCATION AFTER TRIP", StringComparison.OrdinalIgnoreCase)).OrderByDescending(r => r.Field<object>("MetricPercentage")).Take(1);
            tempRow = (from r in query.AsEnumerable()
                       select new
                       {
                           MetricName = r["Metric"].ToString(),
                           MetricPercentage = (r["MetricPercentage"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["MetricPercentage"])).ToString(),
                           change = (r["Change"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Change"])).ToString(),
                           Significance = r["Significance"] == DBNull.Value ? 0.0 : Convert.ToDouble(r["Significance"]),
                       }).ToList();
            loc = "~/Images/ishop-P2P-Icons/LOCATION AFTER TRIP/";
            slide15ChangeData(tempRow, grpPostVisit, "vm", "vm", "c", "img", true, true, true, true, true, loc, context, subPath);


            #endregion

            DataRow sampleSize = dataTable
                            .AsEnumerable()
                            .Where(r => Convert.ToString(r.Field<object>("MetricType")).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (sampleSize != null)
            {
                ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ss")).TextFrame.Text = ((IAutoShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "ss")).TextFrame.Text.Replace("_ss_", String.Format("{0:n0}", sampleSize["MetricPercentage"].ToString()));
            }
        }
        public void slide15ChangeData(IEnumerable<dynamic> tempRowP, IGroupShape grp, string metric, string value, string change, string imgId, bool isVM, bool updateMetricVal, bool updateMetricText, bool updateChange, bool updateImage, string imageLoc, HttpContextBase context, string subPath)
        {
            List<dynamic> tempRow = tempRowP.ToList();
            double metricPercentage = 0.0;
            IPortionFormat format1 = null, format2 = null;
            string suffix = imageLoc.Contains("Satisfaction Drivers") ? "\n\n" : "";
            for (int i = 0; i < tempRow.Count; i++)
            {
                format1 = null; format2 = null;
                if (isVM)
                {
                    format1 = ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == value + (i + 1))).TextFrame.Paragraphs[0].Portions[0].PortionFormat;
                    format2 = ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == metric + (i + 1))).TextFrame.Paragraphs[1].Portions[0].PortionFormat;
                }
                if (!double.TryParse(tempRow[i].MetricPercentage, out metricPercentage)) metricPercentage = 0.0;
                int tempMetricPercentage = (int)(Math.Round(metricPercentage, 2) * 100);
                if (updateMetricText)
                {
                    if (!isVM)
                    {
                        ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == metric + (i + 1))).TextFrame.Text = tempRow[i].MetricName;
                    }
                    else
                    {
                        ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == metric + (i + 1))).TextFrame.Text = ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == metric + (i + 1))).TextFrame.Text.Replace("_metric_", tempRow[i].MetricName + suffix);
                    }
                }
                if (updateMetricVal)
                {

                    if (!isVM)
                    {
                        ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == value + (i + 1))).TextFrame.Text = tempMetricPercentage + "%";
                    }
                    else
                    {
                        ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == value + (i + 1))).TextFrame.Text = ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == value + (i + 1))).TextFrame.Text.Replace("_mp_", tempMetricPercentage + "%");
                    }
                }
                if (updateChange)
                {
                    ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == change + (i + 1))).TextFrame.Text = (Convert.ToDouble(tempRow[i].change)).ToString("#0.0");
                    ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == change + (i + 1))).FillFormat.SolidFillColor.Color = getSigcolorNormal(tempRow[i].Significance);
                }
                if (updateImage)
                {
                    string imageName = getCleanedRetailerForImage(tempRow[i].MetricName, "-");
                    string path = imageLoc + imageName + ".svg";
                    imageReplace((PictureFrame)grp.Shapes.Where(x => x.Name == imgId + (i + 1)).FirstOrDefault(), path, context, 2000 + (i + 1), subPath);
                }
                if (isVM)
                {
                    ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == value + (i + 1))).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = format1.FontBold;
                    ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == value + (i + 1))).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = format1.FontHeight;
                    ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == metric + (i + 1))).TextFrame.Paragraphs[1].Portions[0].PortionFormat.FontBold = format2.FontBold;
                    ((IAutoShape)grp.Shapes.FirstOrDefault(x => x.Name == metric + (i + 1))).TextFrame.Paragraphs[1].Portions[0].PortionFormat.FontHeight = format2.FontHeight;
                }
            }
        }
        public void UpdatePyramidSeriesDataSAR(System.Data.DataTable dt, System.Data.DataTable dt2, string selectedColorCode)
        {
            int defaultIndex = 0;
            List<string> ser = dt.AsEnumerable().OrderByDescending(x => x.Field<object>("L2SortId")).Select(x => x.Field<string>("Metric")).Distinct().ToList();

            List<string> metricList = dt2.AsEnumerable().OrderByDescending(x => x.Field<object>("L2SortId")).Select(x => x.Field<string>("conversion")).Distinct().ToList();

            List<string> type = dt.AsEnumerable().OrderBy(x => x.Field<string>("SelectionType")).Select(x => x.Field<string>("Retailer")).Distinct().ToList();
            IChart chart_to_change_dataLabelColors;
            int series_ind = 1; int dp_index = 1;
            int pyramid_Bars_ind = 4;
            int pyramid_ind = 1;
            defaultIndex = 0;
            foreach (var item in type)
            {
                if (pyramid_ind == 3)
                    break;

                chart_to_change_dataLabelColors = (IChart)cur_Slide.Shapes.Where(x => x.Name == "pyramid" + pyramid_ind).FirstOrDefault();

                //Set length of Vetical Axis
                chart_to_change_dataLabelColors.Axes.HorizontalAxis.IsAutomaticMaxValue = false;
                chart_to_change_dataLabelColors.Axes.HorizontalAxis.IsAutomaticMinValue = false;
                chart_to_change_dataLabelColors.Axes.HorizontalAxis.MinValue = 0;
                //chart_to_change_dataLabelColors.Axes.HorizontalAxis.MaxValue = ser.Count + (ser.Count - 1) * 0.05;
                chart_to_change_dataLabelColors.Axes.HorizontalAxis.NumberFormat = @"##0%";
                IChartDataWorkbook fact = chart_to_change_dataLabelColors.ChartData.ChartDataWorkbook;
                //Add Series

                List<string> xCol = dt.AsEnumerable().Select(x => x.Field<string>("Metric")).Distinct().ToList();

                //fact.GetCell(defaultIndex, 0, 1 + dp_index, item);
                series_ind = 1;

                //Update the color

                chart_to_change_dataLabelColors.ChartData.Series[dp_index].Format.Fill.FillType = FillType.Solid;
                chart_to_change_dataLabelColors.ChartData.Series[dp_index].Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(selectedColorCode);
                series_ind = 1;
                int gap_ser_ind = 0;
                IGroupShape gapGroup = (IGroupShape)cur_Slide.Shapes.FirstOrDefault(x => x.Name == "pyramidGap" + pyramid_ind);
                foreach (var x in ser)
                {
                    if (series_ind > pyramid_Bars_ind)
                    {
                        break;
                    }
                    //if (dp_index == 1)
                    //{
                    //    fact.GetCell(defaultIndex, series_ind, 0, x);
                    //}
                    var val = dt.AsEnumerable().Where(y => y.Field<string>("Metric") == x && y.Field<string>("Retailer") == item).FirstOrDefault();
                    double mv = 0.0;
                    mv = val["MetricPercentage"] == DBNull.Value ? 0 : Convert.ToDouble(val["MetricPercentage"]);
                    fact.GetCell(defaultIndex, series_ind, 1 + dp_index, mv);
                    fact.GetCell(defaultIndex, series_ind, dp_index, (1 - mv) / 2);
                    fact.GetCell(defaultIndex, series_ind, 2 + dp_index, (1 - mv) / 2);

                    if (pyramid_ind == 1)
                    {
                        chart_to_change_dataLabelColors.ChartData.Series[1].Name.AsCells[0].Value = item;
                        chart_to_change_dataLabelColors.ChartData.Categories[series_ind - 1].Value = x;
                    }
                    //Set the labels
                    chart_to_change_dataLabelColors.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.NumberFormat = "#0%";
                    chart_to_change_dataLabelColors.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    if (pyramid_ind == 2) chart_to_change_dataLabelColors.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;

                    //pyramid Gap
                    if (gap_ser_ind < 3)
                    {
                        var gapRow = dt2.AsEnumerable().Where(y => y.Field<string>("retailer") == item && y.Field<string>("conversion") == metricList[gap_ser_ind]).FirstOrDefault();
                        double gapValue = 0.0;
                        gapValue = gapRow["convPercentage"] == DBNull.Value ? 0 : Convert.ToDouble(gapRow["convPercentage"]);
                        gapValue = Math.Round(gapValue * 100, 1);
                        IAutoShape tempShape = ((IAutoShape)gapGroup.Shapes.Where((y) => y.Name == "gap" + gap_ser_ind).FirstOrDefault());
                        tempShape.TextFrame.Text = Convert.ToString(gapValue.ToString("#0.0")) + "%";
                    }

                    //end

                    series_ind++;
                    gap_ser_ind++;
                }


                //dp_index = dp_index + 4;
                //pyramid_ind++;
                pyramid_ind++;
            }
        }
        public void updatePyamidCurveColor(IGroupShape group, string selectedColorCode)
        {
            IAutoShape tempShape;
            for (int i = 1; i <= group.Shapes.Count(); i++)
            {
                tempShape = ((IAutoShape)group.Shapes.Where(x => x.Name == "curve_" + i).FirstOrDefault());
                tempShape.FillFormat.FillType = FillType.Solid;
                tempShape.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(selectedColorCode);

            }
        }

        //Format table cells
        public void formatTableCellSAR(ICell x, LineDashStyle bdr_bottom, LineDashStyle bdr_right, double bdr_bottom_width, double bdr_right_width, Color bdr_clr, Color bdrright_clr, Color font_clr, Aspose.Slides.FillType filltype, Color bg_clr, float fontH, TextAlignment align, TextCapType tcp, float top, float left, float bottom, float right, NullableBool isFB)
        {
            //Border
            //x.BorderTop.Width = 0;
            //x.BorderLeft.Width = 0;

            x.BorderBottom.Width = bdr_bottom_width;
            x.BorderBottom.DashStyle = bdr_bottom;
            x.BorderBottom.FillFormat.FillType = FillType.Solid;
            x.BorderBottom.FillFormat.SolidFillColor.Color = bdr_clr;

            x.BorderRight.Width = bdr_right_width;
            x.BorderRight.DashStyle = bdr_right;
            x.BorderRight.FillFormat.FillType = FillType.Solid;
            x.BorderRight.FillFormat.SolidFillColor.Color = bdrright_clr;


            x.CellFormat.BorderBottom.Width = bdr_bottom_width;
            x.CellFormat.BorderBottom.DashStyle = bdr_bottom;
            x.CellFormat.BorderBottom.FillFormat.FillType = FillType.Solid;
            x.CellFormat.BorderBottom.FillFormat.SolidFillColor.Color = bdr_clr;

            x.CellFormat.BorderRight.Width = bdr_right_width;
            x.CellFormat.BorderRight.DashStyle = bdr_right;
            x.CellFormat.BorderRight.FillFormat.FillType = FillType.Solid;
            x.CellFormat.BorderRight.FillFormat.SolidFillColor.Color = bdrright_clr;


            //Background color, font color, font size, alignment
            if (filltype == FillType.Solid)
            {
                //x.FillFormat.FillType = FillType.Solid;
                //x.FillFormat.SolidFillColor.Color = bg_clr;

                x.CellFormat.FillFormat.FillType = FillType.Solid;
                x.CellFormat.FillFormat.SolidFillColor.Color = bg_clr;
            }
            else if (filltype == FillType.Gradient)
            {
                x.CellFormat.FillFormat.FillType = FillType.Gradient;
                x.CellFormat.FillFormat.GradientFormat.GradientDirection = GradientDirection.FromCorner1;
                x.CellFormat.FillFormat.GradientFormat.GradientShape = GradientShape.Linear;
                x.CellFormat.FillFormat.GradientFormat.LinearGradientAngle = 90;
                x.CellFormat.FillFormat.GradientFormat.GradientStops.Clear();
                x.CellFormat.FillFormat.GradientFormat.GradientStops.Add(0, Color.White);
                x.CellFormat.FillFormat.GradientFormat.GradientStops.Add(1, bg_clr);
                x.CellFormat.FillFormat.GradientFormat.GradientStops.Add(0.91f, bg_clr);
            }

            x.TextFrame.Paragraphs[0].ParagraphFormat.DefaultPortionFormat.FillFormat.FillType = FillType.Solid;
            x.TextFrame.Paragraphs[0].ParagraphFormat.DefaultPortionFormat.FillFormat.SolidFillColor.Color = font_clr;
            x.TextFrame.Paragraphs[0].ParagraphFormat.FontAlignment = FontAlignment.Center;
            x.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = align;
            x.TextFrame.Paragraphs[0].ParagraphFormat.DefaultPortionFormat.FontHeight = fontH;

            //Case
            x.TextFrame.Paragraphs[0].ParagraphFormat.DefaultPortionFormat.TextCapType = tcp;

            //Margins top,bottom : 0.1cm
            x.MarginTop = top; x.MarginBottom = bottom;
            x.MarginLeft = left;
            x.MarginRight = right;

            //Vertical Alignment Center
            x.TextAnchorType = TextAnchorType.Center;

            //Font-family Franklin Gothic Book
            x.TextFrame.Paragraphs[0].ParagraphFormat.DefaultPortionFormat.LatinFont = new FontData("Franklin Gothic Book");
            //Font-bold
            x.TextFrame.Paragraphs[0].ParagraphFormat.DefaultPortionFormat.FontBold = isFB;
        }

        public string getSigTableColor(double val, double std)
        {
            if (val >= std)
            {
                return "Green";
            }
            else if (val <= (-std))
            {
                return "Red";
            }
            else
                return "Black";
        }
        public void createDinerImageryTableSAR(ISlide sld, System.Data.DataTable dt, int slideId, string selectedColor, double stdValue)
        {
            ITable oldTable = (ITable)sld.Shapes.FirstOrDefault(x => x.Name == "tbl");
            int index = 0, jInd = 0, kInd = 0, cur_row_ind = 0;

            double tbl_width = oldTable.Width, tbl_height = oldTable.Height, tbl_x = oldTable.X, tbl_y = oldTable.Y, first_col_w = oldTable[0, 0].FirstColumn.Width, first_row_height = oldTable[0, 0].FirstRow.Height, padd_w = 2;
            List<string> allestList = dt.AsEnumerable().Select(x => x.Field<string>("Retailer")).Distinct().ToList();
            List<string> estList = new List<string>();
            if (allestList.Count > 11)
            {
                for (int i = 0; i < 11; i++)
                {
                    estList.Add(allestList[i]);
                }
            }
            else
            {
                estList = allestList;
            }
            List<string> metricList = dt.AsEnumerable().Select(x => x.Field<string>("Metric")).Distinct().ToList();
            double[] cols = new double[estList.Count + 1];
            double[] rows = new double[metricList.Count + 1];
            //Initialize cols
            double leftoutwidth = (tbl_width - first_col_w) / estList.Count;
            double leftoutHeight = (tbl_height - first_row_height) / (oldTable.Rows.Count);
            cols[0] = first_col_w;
            rows[0] = first_row_height;
            for (int i = 1; i < cols.Length; i++)
            {
                cols[i] = leftoutwidth;
            }
            for (int i = 1; i < rows.Length; i++)
            {
                rows[i] = leftoutHeight;
            }
            if (slideId == 26)
            {
                deviationValue = Convert.ToInt32(stdValue);
            }
            //if (slideId == 28)
            //{
            //    tbl_width = 700;
            //    cols[0] = 580;
            //    cols[1] = 160;
            //}
            //else
            //{
            //    for (index = 1; index < cols.Length; index++)
            //    {
            //        cols[index] = leftoutwidth;
            //    }

            //}

            //Create table
            AsposeSlide.ITable tbl = (Aspose.Slides.ITable)sld.Shapes.AddTable((float)tbl_x, (float)tbl_y, cols, rows);
            tbl.FirstRow = true;
            tbl.StylePreset = TableStylePreset.NoStyleTableGrid;
            //formatTableCellSAR(tbl[0, 0], LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.Transparent, 11, TextAlignment.Center, TextCapType.All, 6, 1, NullableBool.True);
            //formatTableCellSAR(tbl[1, 0], LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.Transparent, 1, TextAlignment.Center, TextCapType.All, 3, 0, NullableBool.False);
            //formatTableCellSAR(tbl[0, 0], LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.FromArgb(242, 242, 242), 11, TextAlignment.Center, TextCapType.None, 6, 1, NullableBool.True);
            //formatTableCellSAR(tbl[1, 0], LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.FromArgb(242, 242, 242), 11, TextAlignment.Center, TextCapType.None, 6, 1, NullableBool.False);

            //if (slideId == 28)
            //{
            //    tbl[0, 0].TextFrame.Text = "Correlation to Dining Frequency for " + channelParentName + " (Index vs Average Correlation)";
            //    tbl.MergeCells(tbl[0, 0], tbl[1, 0], true);
            //    formatTableCellSAR(tbl.MergeCells(tbl[0, 0], tbl[1, 0], true), LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.FromArgb(242, 242, 242), 11, TextAlignment.Center, TextCapType.None, 6, 1, NullableBool.True);
            //    formatTableCellSAR(tbl[1, 0], LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.FromArgb(242, 242, 242), 11, TextAlignment.Center, TextCapType.None, 6, 1, NullableBool.True);

            //}
            //else
            //    tbl[0, 0].TextFrame.Text = "Establishment Imageries";
            if (slideId == 25 || slideId == 27)
            {
                tbl[0, 0].BorderLeft.Width = 0;
                tbl[0, 0].BorderTop.Width = 0;
                formatTableCellSAR(tbl[0, 0], LineDashStyle.Solid, LineDashStyle.SystemDash, 0.5, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(51, 62, 72), FillType.Solid, Color.FromArgb(255, 204, 204, 204), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.True);
            }
            else if (slideId == 26 || slideId == 28)
            {
                tbl[0, 0].BorderLeft.Width = 0;
                tbl[0, 0].BorderTop.Width = 0;
                formatTableCellSAR(tbl[0, 0], LineDashStyle.Solid, LineDashStyle.SystemDash, 0.5, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(51, 62, 72), FillType.Gradient, Color.FromArgb(242, 242, 242), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.True);
                IGroupShape strngthGrp = (IGroupShape)sld.Shapes.FirstOrDefault((x => x.Name == "strengthGroup"));
                string textVal = ((IAutoShape)strngthGrp.Shapes.FirstOrDefault(x => x.Name == "strengthText")).TextFrame.Text;
                string strngthVal = stdValue.ToString("#0.0") + "%";
                textVal = textVal.Replace("_m_", strngthVal).Replace("_mn_", "-" + strngthVal);
                ((IAutoShape)strngthGrp.Shapes.FirstOrDefault(x => x.Name == "strengthText")).TextFrame.Text = textVal;
            }
            //Fill table
            for (index = 0; index < estList.Count; index++)
            {
                cur_row_ind = 0;
                //Add column header
                if (slideId == 25 || slideId == 27)
                {
                    tbl[1 + index, 0].TextFrame.Text = estList[index];
                    //Format Column Headers
                    tbl[1 + index, 0].BorderTop.Width = 0;
                    formatTableCellSAR(tbl[1 + index, 0], LineDashStyle.Solid, LineDashStyle.SystemDash, 0.5, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(51, 62, 72), FillType.Solid, Color.FromArgb(255, 204, 204, 204), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.True);
                    //formatTableCellSAR(tbl[1 + index, 0], LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.FromArgb(242, 242, 242), 11, TextAlignment.Center, TextCapType.None, 6, 0, NullableBool.True);
                }
                else if (slideId == 26 || slideId == 28)
                {
                    tbl[1 + index, 0].TextFrame.Text = estList[index];
                    //Format Column Headers
                    tbl[1 + index, 0].BorderTop.Width = 0;
                    formatTableCellSAR(tbl[1 + index, 0], LineDashStyle.Solid, LineDashStyle.SystemDash, 0.5, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(51, 62, 72), FillType.Gradient, Color.FromArgb(242, 242, 242), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.True);
                    //formatTableCellSAR(tbl[1 + index, 0], LineDashStyle.Solid, ColorTranslator.FromHtml(selectedColor), Color.LightGray, Color.Black, Color.FromArgb(242, 242, 242), 11, TextAlignment.Center, TextCapType.None, 6, 0, NullableBool.True);
                }
                var dt_rows = dt.AsEnumerable().Where(x => x.Field<string>("Retailer") == estList[index]);
                kInd = 0;
                //Format the group-column header
                //formatTableCellSAR(tbl[1+ 1 * index, cur_row_ind], LineDashStyle.Solid, Color.FromArgb(100, 100, 100), Color.FromArgb(64, 64, 64), Color.FromArgb(230, 230, 230), 11, TextAlignment.Left, TextCapType.All, 5, 0);
                //formatTableCellSAR(tbl[1 + 2 * index, cur_row_ind], LineDashStyle.Solid, Color.Transparent, Color.Transparent, Color.Transparent, 11, TextAlignment.Center, TextCapType.All, 5, 0);
                cur_row_ind++;
                foreach (DataRow dr in dt_rows)
                {
                    if (index == 0)
                    {
                        //Update the 1st Column
                        tbl[0, cur_row_ind].TextFrame.Text = Convert.ToString(dr["Metric"]);
                        tbl[0, cur_row_ind].BorderLeft.Width = 0;
                        formatTableCellSAR(tbl[0, cur_row_ind], LineDashStyle.SystemDash, LineDashStyle.SystemDash, 0.25, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(51, 62, 72), FillType.NoFill, Color.FromArgb(255, 0, 0, 0), 10, TextAlignment.Left, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.False);
                        //formatTableCellSAR(tbl[0, cur_row_ind], LineDashStyle.Solid, Color.LightGray, Color.LightGray, Color.Black, Color.Transparent, 8, TextAlignment.Left, TextCapType.None, -30, 1, NullableBool.False);
                    }
                    if (slideId == 25 || slideId == 27)
                    {
                        tbl[1 + index, cur_row_ind].TextFrame.Text = (Convert.ToDouble(dr["MetricPercentage"]) * 100).ToString("#0.0") + "%";
                        formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.SystemDash, LineDashStyle.SystemDash, 0.25, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(51, 62, 72), FillType.NoFill, Color.FromArgb(255, 0, 0, 0), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.False);
                    }
                    else if (slideId == 26 || slideId == 28)
                    {

                        tbl[1 + index, cur_row_ind].TextFrame.Text = (Convert.ToDouble(dr["MetricPercentage"]) * 100).ToString("#0.0") + "%";
                        switch (getSigTableColor((Convert.ToDouble(dr["MetricPercentage"]) * 100), stdValue))
                        {
                            case "Green": formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.SystemDash, LineDashStyle.SystemDash, 0.25, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(32, 178, 80), FillType.Solid, Color.FromArgb(214, 253, 207), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.False); break;
                            case "Red": formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.SystemDash, LineDashStyle.SystemDash, 0.25, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(192, 0, 0), FillType.Solid, Color.FromArgb(250, 210, 213), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.False); break;
                            case "Black": formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.SystemDash, LineDashStyle.SystemDash, 0.25, 0.25, Color.FromArgb(255, 128, 128, 128), Color.FromArgb(255, 128, 128, 128), Color.FromArgb(51, 62, 72), FillType.NoFill, Color.FromArgb(255, 0, 0, 0), 10, TextAlignment.Center, TextCapType.None, 0.75f, 0.75f, 0, 0.75f, NullableBool.False); break;
                        }

                    }
                    //Update the values
                    //if (slideId == 28)
                    //{
                    //    if (dr["DeviationValue"].ToString() == "999")
                    //        tbl[1 + index, cur_row_ind].TextFrame.Text = "NA";
                    //    else
                    //        tbl[1 + index, cur_row_ind].TextFrame.Text = dr["DeviationValue"] == DBNull.Value ? "NA" : (Convert.ToDouble(dr["DeviationValue"])).ToString();
                    //}
                    //else
                    //{

                    //    if (dr["DeviationValue"].ToString() == "999")
                    //        tbl[1 + index, cur_row_ind].TextFrame.Text = "NA";
                    //    else
                    //        tbl[1 + index, cur_row_ind].TextFrame.Text = dr["DeviationValue"] == DBNull.Value ? "NA" : (Convert.ToDouble(dr["DeviationValue"])).ToString("#0.0") + "%";
                    //}
                    //if (slideId == 27)
                    //{
                    //    formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.Solid, Color.LightGray, Color.LightGray, returnStatTestColorSARBG(dr, true), returnStatTestColorSARBG(dr, false), 8, TextAlignment.Center, TextCapType.None, 3, 0, NullableBool.False);
                    //    ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "txt_devtn_1").FirstOrDefault()).TextFrame.Text = dr["SigRangeValue"].ToString();
                    //    ((IAutoShape)cur_Slide.Shapes.Where(x => x.Name == "txt_devtn_2").FirstOrDefault()).TextFrame.Text = "- " + dr["SigRangeValue"].ToString();
                    //    deviationValue = dr["SigRangeValue"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SigRangeValue"]);
                    //}
                    //else if (slideId == 26)
                    //{
                    //    formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.Solid, Color.LightGray, Color.LightGray, getSigColor(dr["SignificanceColorFlag"].ToString()), Color.Transparent, 8, TextAlignment.Center, TextCapType.None, 3, 0, NullableBool.False);
                    //}
                    //else
                    //    formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.Solid, Color.LightGray, Color.LightGray, Color.Black, Color.Transparent, 8, TextAlignment.Center, TextCapType.None, 3, 0, NullableBool.False);
                    //formatTableCellSAR(tbl[1 + index, cur_row_ind], LineDashStyle.Solid, Color.Transparent, Color.Transparent, Color.Transparent, 9, TextAlignment.Center, TextCapType.All, 3, 0);
                    kInd++; cur_row_ind++;
                }



                //}
            }
            //Remove existing table
            sld.Shapes.Remove(sld.Shapes.Where(x => x.Name == "tbl").FirstOrDefault());
        }

        public double getNearestValue(double val, bool isVertical, bool isMaxVal)
        {

            double reslt = 0.0;
            double valcoeff = 0;

            if (val > 0 && isMaxVal)
            {
                if (isVertical)
                {
                    if (val % 10 == 0)

                    {
                        valcoeff = (Math.Floor(val / 10)) * 10;
                    }
                    else
                    {
                        valcoeff = ((Math.Floor(val / 10)) * 10) + 10;
                    }

                }
                else
                {
                    double Xaxisval = 0;
                    if (val < deviationValue)
                    {
                        Xaxisval = deviationValue;
                    }
                    else
                        Xaxisval = val;
                    if (Xaxisval % 5 == 0)

                    {
                        valcoeff = (Math.Floor(Xaxisval / 5)) * 5;
                    }
                    else
                    {
                        valcoeff = ((Math.Floor(Xaxisval / 5)) * 5) + 5;
                    }
                }


                reslt = valcoeff;
            }
            else
            {


                if (isVertical)
                {

                    if (val % 10 == 0)
                    {
                        valcoeff = (Math.Floor(Math.Abs(val) / 10)) * 10;
                    }
                    else
                    {
                        valcoeff = Math.Abs((Math.Floor(Math.Abs(val) / 10)) * 10);
                    }
                    reslt = valcoeff;
                }
                else
                {


                    double XaxisMinval = 0;
                    if (val > (-deviationValue))
                    {
                        XaxisMinval = deviationValue;
                    }
                    else
                        XaxisMinval = val;
                    if (val % 5 == 0)
                    {
                        valcoeff = (Math.Floor(Math.Abs(XaxisMinval) / 5)) * 5;
                    }
                    else
                    {
                        valcoeff = ((Math.Floor(Math.Abs(XaxisMinval) / 5)) * 5) + 5;
                    }
                    reslt = -valcoeff;
                }
            }

            return reslt;
        }
        public void setStrengthWeekPosition(ISlide sld)
        {

            IChart chart;
            chart = (IChart)sld.Shapes.Where(x => x.Name == "chart").FirstOrDefault();
            float markValue = chart.Width / rangeValue;
            float devatnNegative = (rangeValue / 2) - deviationValue;
            float devatnPostive = (rangeValue + deviationValue + deviationValue) / 2;
            IConnector connectorShape = (IConnector)cur_Slide.Shapes.Where(x => x.Name == "weekness").FirstOrDefault();
            connectorShape.X = chart.X + 22 + markValue * devatnNegative;
            connectorShape = (IConnector)cur_Slide.Shapes.Where(x => x.Name == "strength").FirstOrDefault();
            connectorShape.X = chart.X + markValue * devatnPostive;

        }
        public void notesSection(ISlide sld, string notes)
        {
            ((Aspose.Slides.TextFrame)((Aspose.Slides.NotesSlide)((Aspose.Slides.NotesSlideManager)((Aspose.Slides.Slide)sld).NotesSlideManager).NotesSlide).NotesTextFrame).Text = notes;
        }
        public void updateScatterSeries(ISlide sld, string shapeName, System.Data.DataTable metricTable, System.Data.DataTable indexTable, string selectedColor, int slideId, string channelName, string establishmentName)
        {
            //IChart chart = (IChart)sld.Shapes.FirstOrDefault(x => x.Name == shapeName);
            //IChartDataWorkbook wbook = chart.ChartData.ChartDataWorkbook;
            //int i = 1;
            //foreach(DataRow row in indexTable.Rows)
            //{
            //    var metricPercentage = metricTable.AsEnumerable().Where(r => Convert.ToString((r.Field<object>("Metric"))).Equals(row["Metric"].ToString(), StringComparison.OrdinalIgnoreCase)).Select(r => r.Field<object>("MetricPercentage")).FirstOrDefault();
            //    double metricVal = 0.0;
            //    if (!double.TryParse(metricPercentage.ToString(), out metricVal)) metricVal = 0.0;
            //    wbook.GetCell(0, i, 0, metricVal);
            //    wbook.GetCell(0, i, 1, Convert.ToInt32(row["values"]));
            //    wbook.GetCell(0, i, 2, row["Metric"].ToString());
            //    i++;
            //}

            IChart chart;
            IChartSeries Series;
            IDataLabel lbl;

            chart = (IChart)sld.Shapes.Where(x => x.Name == "chart").FirstOrDefault();
            int i = 0, j = 0;
            //Aspose.Slides.Charts.IChart chart = null;
            chart.ChartData.Series.Clear();
            int defaultWorksheetIndex = 0;
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;
            //chart.ChartData.Series.Add(fact.GetCell(0, 1, 1, ""), chart.Type);
            chart.ChartData.Series.Add(fact.GetCell(0, 0, 1, "Category Lift with Monthly+ Dining"), chart.Type);
            //first chart series
            IChartSeries series = chart.ChartData.Series[0];
            series.Marker.Size = 10;
            series.Marker.Format.Fill.FillType = FillType.Solid;

            series.Marker.Format.Fill.SolidFillColor.Color = ColorTranslator.FromHtml(selectedColor);
            series.Marker.Symbol = MarkerStyleType.Circle;
            series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;

            series.Marker.Format.Line.FillFormat.SolidFillColor.Color = ColorTranslator.FromHtml(selectedColor);
            //Adding data to series 1
            i = 0; IChartDataPoint pointIndex; int tempCount = 0;
            double _vminxAxis = 0.0; double _vmaxxAxis = 0.0;
            double _hminxAxis = 0.0; double _hmaxxAxis = 0.0; double xvalue = 0.0;
            _vmaxxAxis = (Convert.ToDouble(indexTable.Compute("max([values])", string.Empty)));
            _vminxAxis = (Convert.ToDouble(indexTable.Compute("min([values])", string.Empty)));

            var d24reslt1 = (from r in metricTable.AsEnumerable()
                             select new
                             {
                                 value = Convert.ToDouble(r.Field<object>("MetricPercentage")) * 100,
                             }).ToList();
            //            var sortD24reslt11 = d24reslt1;

            var d24reslt = (from r in metricTable.AsEnumerable()

                            select new
                            {
                                value = Convert.ToDouble(r.Field<object>("MetricPercentage")) * 100,
                                metricName = Convert.ToString(r.Field<object>("Metric"))
                            }).ToList();
            var sortD24reslt11 = d24reslt.OrderBy(d => d.value).ToList();

            _hmaxxAxis = sortD24reslt11[sortD24reslt11.Count - 1].value;
            _hminxAxis = sortD24reslt11[0].value;

            chart.Axes.HorizontalAxis.IsAutomaticMaxValue = false;
            chart.Axes.HorizontalAxis.IsAutomaticMinValue = false;

            double maxvalue = (getNearestValue(_hmaxxAxis, false, true)) / 100;

            double minvalue = (getNearestValue(_hminxAxis, false, false)) / 100;

            //double maxvalue = (getNearestValue(_hmaxxAxis, false, true)) / 100;
            //double minvalue = (getNearestValue(_hminxAxis, false, false)) / 100;
            //double mxvalue = maxvalue * 0.35;
            //double mnvalue = minvalue * 0.35;
            //maxvalue += 0.10;
            //minvalue += 0.10;

            if (maxvalue > Math.Abs(minvalue))
            {
                chart.Axes.HorizontalAxis.MaxValue = maxvalue;
                chart.Axes.HorizontalAxis.MinValue = -maxvalue;
                rangeValue = Math.Abs(Convert.ToInt32((maxvalue * 2) * 100));
            }
            else
            {
                chart.Axes.HorizontalAxis.MaxValue = -minvalue;
                chart.Axes.HorizontalAxis.MinValue = minvalue;
                rangeValue = Math.Abs(Convert.ToInt32((minvalue * 2) * 100));
            }

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            //double V_maxvalue = ((Math.Round(getNearestValue(_vmaxxAxis, true, true) / 5d) * 5) + 10);
            //double V_minvalue = ((Math.Round(getNearestValue(_vminxAxis, true, false) / 5d) * 5) - 10);
            chart.Axes.VerticalAxis.MaxValue = getNearestValue(_vmaxxAxis, true, true);
            chart.Axes.VerticalAxis.MinValue = getNearestValue(_vminxAxis, true, false);
            //chart.Axes.VerticalAxis.MaxValue = V_maxvalue;
            //chart.Axes.VerticalAxis.MinValue = V_minvalue;
            dynamic d25reslt = (from r in indexTable.AsEnumerable()

                                select new
                                {
                                    value = Convert.ToString(r.Field<object>("values")),
                                    metricName = Convert.ToString(r.Field<object>("Metric"))
                                }).ToList();

            foreach (var r in d25reslt)
            {
                foreach (var d in d24reslt)
                {
                    if ((r.metricName).Equals(d.metricName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        if (!double.TryParse(Convert.ToString(d.value), out xvalue)) xvalue = 0.0;
                        series.DataPoints.AddDataPointForScatterSeries(fact.GetCell(defaultWorksheetIndex, 1 + tempCount, 0, xvalue == 0.0 ? 0.0 : xvalue / 100), fact.GetCell(defaultWorksheetIndex, 1 + tempCount, 1, Convert.ToDouble(r.value)));
                        pointIndex = series.DataPoints[tempCount];
                        pointIndex.Label.TextFrameForOverriding.Text = r.metricName;
                        series.DataPoints[tempCount].Label.TextFrameForOverriding.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
                        series.DataPoints[tempCount].Label.DataLabelFormat.TextFormat.TextBlockFormat.WrapText = NullableBool.True;
                        //series.Labels.DefaultDataLabelFormat.ShowLeaderLines = true;
                        //series.Labels.DefaultDataLabelFormat.ShowSeriesName = true;
                        //series.Labels.DefaultDataLabelFormat.ShowCategoryName = true;
                        //series.Labels.DefaultDataLabelFormat.TextFormat.TextBlockFormat.AutofitType = TextAutofitType.Normal;
                        //series.Labels.DefaultDataLabelFormat.Position = AsposeChart.LegendDataLabelPosition.Right;
                        series.Marker.Size = 10;
                        chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 1 + tempCount, 2, r.metricName), chart.Type);
                        tempCount++;
                        i++;
                    }
                }
            }
            chart.HasLegend = false;
            chart.HasTitle = false;
            chart.Axes.VerticalAxis.Title.AddTextFrameForOverriding(channelName + " Correlation to Shopping Frequency");
            chart.Axes.HorizontalAxis.Title.AddTextFrameForOverriding(establishmentName + " Imagery Weaknesses/ Strengths");
            chart.Axes.HorizontalAxis.MajorTickMark = Aspose.Slides.Charts.TickMarkType.None;
            chart.Axes.HorizontalAxis.MinorTickMark = Aspose.Slides.Charts.TickMarkType.None;
            chart.Axes.VerticalAxis.MajorTickMark = Aspose.Slides.Charts.TickMarkType.None;
            chart.Axes.VerticalAxis.MinorTickMark = Aspose.Slides.Charts.TickMarkType.None;
            chart.Axes.HorizontalAxis.TickLabelPosition = Aspose.Slides.Charts.TickLabelPositionType.Low;
            chart.Axes.VerticalAxis.TickLabelPosition = Aspose.Slides.Charts.TickLabelPositionType.Low;
        }
    }
}