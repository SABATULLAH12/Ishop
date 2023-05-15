using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using iSHOPNew.DAL;
using iSHOP.BLL;
using System.Globalization;

namespace iSHOPNew.Models
{
    public partial class DownloadCorrespondenceExcel
    {
        AdvancedAnalyticsParams advancedAnalyticsParams = null;
        private Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        public Dictionary<string, string> HeaderTabs = new Dictionary<string, string>();
        public Dictionary<string, string> FilterTabs = new Dictionary<string, string>();
        int rowNumber = 0;
        string UserExportFileName = string.Empty;
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["correspondenceData"] != null)
            {
                advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
                Shortnames();
                PopulateShortNames();
                ExportToExcel(Convert.ToString(HttpContext.Current.Request.QueryString["year"]), Convert.ToString(HttpContext.Current.Request.QueryString["month"]), Convert.ToString(HttpContext.Current.Request.QueryString["date"]), Convert.ToString(HttpContext.Current.Request.QueryString["hours"]), Convert.ToString(HttpContext.Current.Request.QueryString["minutes"]), Convert.ToString(HttpContext.Current.Request.QueryString["seconds"]));
            }
        }
        public void ExportToExcel(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;

            try
            {
                CopyFilesToDestination();
                writeSheet();
                writeSharedstring();

                string tempDir = HttpContext.Current.Server.MapPath(UserExportFileName + "");
                string fileName = HttpContext.Current.Server.MapPath("~/CorrespondenceExportExcelFiles/ExcelTemplate.xlsm");
                ZipDirectory(tempDir, fileName);

                if (Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
                {
                    Directory.Delete(HttpContext.Current.Server.MapPath(UserExportFileName), true);
                }

                FileStream fs1 = null;
                fs1 = System.IO.File.Open(fileName, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs1.Length];
                fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                fs1.Close();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=iShop_Explorer_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds)) + ".xlsm");
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Length", new FileInfo(fileName).Length.ToString());
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
                HttpContext.Current.Response.BinaryWrite(btFile);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
        public void Shortnames()
        {
            FilterTabs.Clear();
            FilterTabs.Add("MainStore", "Main Store (in channel)");
            FilterTabs.Add("MainStoreOverAll", "Main Store (across channel)");
            FilterTabs.Add("FavoriteStore", "Favorite Store (in channel)");
            FilterTabs.Add("FavoriteStoreOverAll", "Favorite Store (across channel)");
        }
        private string Get_ShortNamesFrequency(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (FilterTabs.ContainsKey(spVal))
                    slRetVal = FilterTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        public string WriteFilters()
        {
            string xmlstring = "* Selection";
            string xmltext = string.Empty;
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
           "r = \"1\" " +
           "spans = \"1:11\" " +
           "ht = \"15\" " +
           "thickBot = \"1\" " +
           "x14ac:dyDescent = \"0.3\">" +
           "<c " +
               "r = \"B1\" " +
               "s = \"10\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmlstring = "* Filters";
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"C1\" " +
               "s = \"10\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmltext += "</row>";

            //Time Period
            if (!string.IsNullOrEmpty(advancedAnalyticsParams.TimePeriod))
            {
                if (advancedAnalyticsParams.TimePeriod.IndexOf("3MMT") > -1)
                {
                    xmlstring = "Time Period : " + advancedAnalyticsParams.TimePeriod.Split('|')[1] + " 3MMT";
                }
                else if (advancedAnalyticsParams.TimePeriod.IndexOf("total") > -1)
                {
                    xmlstring = "Time Period : AUG 2013 TO JUN 2014";
                }
                else
                {

                    xmlstring = "Time Period : " + advancedAnalyticsParams.TimePeriod.Split('|')[1];
                }
            }
            else
            {
                xmlstring = "Time Period :";
            }

            xmlstring = cf.cleanExcelXML("Time Period : " + Convert.ToString(advancedAnalyticsParams.ShortTimePeriod.Replace(":", "")));
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
   "r = \"2\" " +
   "spans = \"1:11\" " +
   "ht = \"15\" " +
   "thickBot = \"1\" " +
   "x14ac:dyDescent = \"0.3\">" +
   "<c " +
       "r = \"B2\" " +
       "s = \"11\" " +
       "t = \"s\">" +
       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
   "</c>";

            if (advancedAnalyticsParams.Comparisonlist.IndexOf("Category") > -1 || advancedAnalyticsParams.Comparisonlist.IndexOf("Brand") > -1 || advancedAnalyticsParams.Comparisonlist.IndexOf("Category") > -1 || advancedAnalyticsParams.Comparisonlist.IndexOf("Brand") > -1)
            {
                if (advancedAnalyticsParams.ShopperFrequency.IndexOf("channels") > -1 || advancedAnalyticsParams.ShopperFrequency.IndexOf("retailers") > -1)
                {
                    string[] cr = advancedAnalyticsParams.ShopperFrequency.Split(new String[] { "|", "|" },
                                   StringSplitOptions.RemoveEmptyEntries);
                    string text = string.Empty;
                    for (int i = 1; i < cr.Length; i += 2)
                    {
                        text += Get_ShortNames(cr[i]) + ", ";
                    }
                    xmlstring = "COMPARISON POINTS : " + text;
                }
                else
                {
                    xmlstring = "Monthly Purchasing Amount : " + advancedAnalyticsParams.ShopperFrequency;
                }
            }
            else
            {
                //xmlstring = "Shopping Frequency: " + frequency;
                xmlstring = "Shopping Frequency: " + Get_ShortNamesFrequency(advancedAnalyticsParams.ShopperFrequencyShortName);
            }
            xmlstring = cf.cleanExcelXML((!string.IsNullOrEmpty(advancedAnalyticsParams.FrequencyTitle) ? advancedAnalyticsParams.FrequencyTitle : "Frequency") + ": " + Get_ShortNamesFrequency(advancedAnalyticsParams.ShopperFrequencyShortName));
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
           "r = \"C2\" " +
           "s = \"11\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ";

            xmltext += "</row>";

            //Single Selection
            xmlstring = "";
            if (advancedAnalyticsParams.ModuleBlock.Contains("Within"))
                xmlstring = "Channel/Retailer : " + Get_ShortNames(advancedAnalyticsParams.ShopperSegment.Replace("Channels|", "").Replace("Retailers|", ""));
            // xmlstring = "Single Selection";
            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
     "r = \"3\" " +
     "spans = \"1:11\" " +
     "ht = \"15\" " +
     "thickBot = \"1\" " +
     "x14ac:dyDescent = \"0.3\">" +
     "<c " +
         "r = \"B3\" " +
         "s = \"12\" " +
         "t = \"s\">" +
         "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
     "</c>";            

            string CustomFilter = cf.GetExcelSortedFilters(advancedAnalyticsParams.FilterShortNames);

            //string[] ss = advancedAnalyticsParams.FilterShortNames.Split(new String[] { "|", "|" },
            //                        StringSplitOptions.RemoveEmptyEntries);

            //for (int i = 0; i < ss.Length; i += 2)
            //{
            //    ss[i] = ss[i] + ": ";
            //}

            //for (int i = 1; i < ss.Length; i += 2)
            //{
            //    ss[i] = ss[i] + ", ";
            //}
            //foreach (string xmlfilter in ss)
            //{
            //    CustomFilter += xmlfilter;
            //}
            if (CustomFilter != "")
                xmlstring = CustomFilter;
            else
                xmlstring = " : ";

            xmlstring = cf.cleanExcelXML(xmlstring);

           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }
            xmltext += "<c " +
           "r = \"C3\" " +
           "s = \"12\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ";

            xmltext += "</row>";
            //Single Selection

            //       xmlstring = "Single Selection";
            //       xmlstring = cf.cleanExcelXML(xmlstring);
            //      if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            //       {
            //           sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            //       }

            //       xmltext += "<row " +
            //"r = \"3\" " +
            //"spans = \"1:11\" " +
            //"ht = \"15\" " +
            //"thickBot = \"1\" " +
            //"x14ac:dyDescent = \"0.3\">" +
            //"<c " +
            //    "r = \"B4\" " +
            //    "s = \"13\" " +
            //    "t = \"s\">" +
            //    "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
            //"</c></row> ";

            return xmltext;

        }
        private void writeSharedstring()
        {
            string xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/xl/sharedStrings.xml");
            XmlDocument xmlChart = new XmlDocument();
            xmlChart.Load(xmlpath);
            XmlNodeList snodelist = xmlChart.GetElementsByTagName("sst");

            string xmltest = "xmlns = \"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
                           "count = \" " + sharedStrings.Count + "\" " +
                           "uniqueCount = \" " + sharedStrings.Count + "\">";
            sharedStrings = sharedStrings.ToDictionary(pair => Convert.ToString(pair.Key.ToUpper()), pair => pair.Value);

            List<string> sSharedStrings = new List<string>();
            var sSVal ="";
            var sPrevsVal = "";
            var iCount = 0;
            foreach (string sString in sharedStrings.Keys)
            {                
                sSVal = "";
                sPrevsVal = "";
                if (HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] != null)
                {
                    AdvancedAnalyticsParams advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
                    var sSelection = advancedAnalyticsParams.Metric.ToLower();
                    if (sString.ToLower() == sSelection)
                        iCount = 1;
                    else if (sString.ToLower() == "bi-variate correspondence analysis" || sString.ToLower() == "retailers")
                        iCount = 0;
                    else if (sString.ToLower() == "comparison points" || sString.ToLower() == "variables")
                        iCount = 1;

                    if (iCount == 0)
                        sSVal = sString.ToUpper();
                    else
                        sSVal = CommonFunctions.ToTitleCase(sString);

                    sSharedStrings.Add(sSVal); 
                }
            }

            int count = 0;
            foreach (string sString in sSharedStrings)
            {
                count++;
                
                string[] headers = sString.Split(':');
                if (sString.Contains(":") && headers.Count() == 2)
                {
                    string[] sf = sString.Split(':');
                    xmltest += "<si><r><rPr><b /><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t>" + sf[0].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ": </t></r>";
                    xmltest += "<r><rPr><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t xml:space = \"preserve\">" + sf[1].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></r></si>";
                }
                else if (sString.Contains(":") && headers.Count() > 2)
                {
                    if (sString == "")
                    {
                        xmltest += "<si><t>" + sString.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></si>";
                    }
                    else
                    {
                        string[] ss = sString.Split(new String[] { ":", ", " },
                                         StringSplitOptions.RemoveEmptyEntries);

                        xmltest += "<si><r><rPr><b /> <u /><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t>" + ss[0].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ": </t></r>";
                        //Temp variables Do not use else where
                        int countss = ss.Length;
                        int i = 2;
                        int j = 1;
                        while (countss > 0)
                        {
                            while (j < ss.Length - 2)
                            {
                                xmltest += "<r><rPr><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t xml:space = \"preserve\">" + ss[j].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ", </t></r>";
                                j += 2;
                                break;
                            }
                            while (i < ss.Length)
                            {
                                xmltest += "<r><rPr><b /> <u /><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t>" + ss[i].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ": </t></r>";
                                i += 2;
                                break;
                            }

                            countss -= 2;
                        }
                        xmltest += "<r><rPr><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t xml:space = \"preserve\">" + ss[j].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></r>";
                        xmltest += "</si>";
                    }
                }
                else
                {
                    xmltest += "<si><t>" + sString.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></si>";
                }
            }

            foreach (XmlNode node in snodelist)
            {
                node.InnerXml = "";
                node.InnerXml = xmltest;
            }
            xmlChart.Save(xmlpath);
        }
        private static DataSet CreateTables(DataSet ds)
        {
            DataSet das = new DataSet();
            DataTable tbl = new DataTable();
            List<string> Benchmark_Comparisonlist = null;
            if (ds != null && ds.Tables.Count >= 3)
            {
                #region Create Columns
                tbl.Columns.Add("Metric", typeof(string));
                tbl.Columns.Add("MetricItem", typeof(string));
                #region Add Benchmark and Comparison Columns
                var query = from r in ds.Tables[2].AsEnumerable()
                            select r.Field<string>("Objective");
                Benchmark_Comparisonlist = query.Distinct().ToList();
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    tbl.Columns.Add(bc, typeof(string));
                }
                #endregion

                #endregion

                #region Create Rows
                #region Add Sample Size
                List<object> Metricrowitems = new List<object>();
                List<object> Significancerowitems = new List<object>();
                Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                Metricrowitems.Add("SampleSize");
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    var query2 = from r in ds.Tables[0].AsEnumerable()
                                 where r.Field<string>("Objective") == bc
                                 select r;
                    List<DataRow> Rows = query2.ToList();
                    if (Rows != null && Rows.Count > 0)
                    {
                        foreach (DataRow row in Rows)
                        {
                            Metricrowitems.Add(row["SampleSize"]);
                        }
                    }
                    else
                    {
                        if (bc.Equals("AVERAGE", StringComparison.OrdinalIgnoreCase))
                        {
                            string samplesize = (from r in ds.Tables[2].AsEnumerable()
                                         where r.Field<string>("Objective") == bc
                                                select Convert.ToString(r.Field<object>("SampleSize"))).FirstOrDefault();
                            Metricrowitems.Add(samplesize);
                        }
                        else
                        {
                            Metricrowitems.Add(string.Empty);
                        }
                    }
                }
                tbl.Rows.Add(Metricrowitems.ToArray());
                #endregion

                #region Add Metric Items
                var query4 = from r in ds.Tables[2].AsEnumerable()
                             select r.Field<string>("MetricItem");
                List<string> MetricItems = query4.Distinct().ToList();
                foreach (string metric in MetricItems)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Metricrowitems.Add(metric);

                    Significancerowitems = new List<object>();
                    Significancerowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Significancerowitems.Add(metric + "Significance");
                    var query3 = from r in ds.Tables[2].AsEnumerable()
                                 where r.Field<string>("MetricItem") == metric
                                 select r;
                    List<DataRow> Rows = query3.ToList();
                    foreach (DataRow row in Rows)
                    {
                        Metricrowitems.Add(row["Volume"]);
                        Significancerowitems.Add(row["Significance"]);
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                    tbl.Rows.Add(Significancerowitems.ToArray());
                }

                #endregion
                #endregion
                das.Tables.Add(tbl);
            }
            return das;
        }
        private string Generate_Contingency_Table()
        {
            AdvancedAnalyticsParams advancedAnalyticsParams = null;
            ContingencyTable contingencyTable = new ContingencyTable();
            string tbltxt = string.Empty;
            string xmlstring = string.Empty;
            try
            {
                if (HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] != null)
                {
                    advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;

                    DataSet ds = CreateTables(advancedAnalyticsParams.ChartDataSet);
                    var query = from r in advancedAnalyticsParams.ChartDataSet.Tables[2].AsEnumerable()
                                select r.Field<string>("Objective");
                    List<string> shortnames = query.Distinct().ToList();
                    contingencyTable.BindTabs(out tbltxt, out xmlstring, ds, advancedAnalyticsParams.TimePeriod, advancedAnalyticsParams.ShopperSegment,
                     advancedAnalyticsParams.FilterShortNames, advancedAnalyticsParams.ShopperFrequencyShortName, shortnames, Convert.ToString(advancedAnalyticsParams.StatPositive)
                     , Convert.ToString(advancedAnalyticsParams.StatNegative), "false", advancedAnalyticsParams.ShortTimePeriod, advancedAnalyticsParams.ViewType);
                }
            }
            catch (Exception ex)
            {

            }
            return xmlstring;
        }
        private void UpdateTableSheet()
        {
            string sheetpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/xl/worksheets/sheet2.xml");
            XmlDocument xmlChart = new XmlDocument();
            string sheetdata = Generate_Contingency_Table();
            xmlChart.Load(sheetpath);
            XmlNodeList snodelist = xmlChart.GetElementsByTagName("worksheet");
            foreach (XmlNode node in snodelist)
            {
                node.InnerXml = "";
                node.InnerXml = sheetdata;
            }
            xmlChart.Save(sheetpath);
            snodelist = xmlChart.GetElementsByTagName("sheetViews");
            string sview = "<sheetView showGridLines = \"0\" zoomScale = \"80\" zoomScaleNormal = \"80\" workbookViewId = \"0\"> " +
                       "<pane ySplit =\"7\" topLeftCell =\"A8\" activePane =\"bottomLeft\" state =\"frozen\"/>" +
                      "<selection pane = \"bottomLeft\" activeCell = \"A1\" sqref = \"A1\"/> " +
                      "</sheetView> ";
            foreach (XmlNode fsnode in snodelist)
            {

                fsnode.InnerXml = "";
                fsnode.InnerXml = sview;
            }
            xmlChart.Save(sheetpath);
        }

        private void writeSheet()
        {
            //update table sheet
            UpdateTableSheet();
            //
            if (HttpContext.Current.Session["CorrespondenceMapsSharedstrings"] != null)
            {
                sharedStrings = HttpContext.Current.Session["CorrespondenceMapsSharedstrings"] as Dictionary<string, int>;
            }

            XmlDocument xmlChart = new XmlDocument();
            string xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/xl/worksheets/sheet1.xml");
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("sheetData");
            rowNumber = 5;
            DataTable tbl = HttpContext.Current.Session["correspondenceData"] as DataTable;
            List<string> ChannelRetailerlist = new List<string>();
            List<string> Variablelist = new List<string>();
            StringBuilder sb = new StringBuilder();
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //write header
                sb.AppendLine(WriteFilters());
                sb.AppendLine("<row r = \"" + rowNumber + "\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
                if (!sharedStrings.ContainsKey("Bi-Variate Correspondence Analysis"))
                {
                    sharedStrings.Add("Bi-Variate Correspondence Analysis", sharedStrings.Count());
                }
                sb.AppendLine("<c r = \"A" + rowNumber + "\" s = \"13\" t = \"s\"><v>" + sharedStrings["Bi-Variate Correspondence Analysis"] + "</v></c>");

                if (!sharedStrings.ContainsKey("Dimension 1"))
                {
                    sharedStrings.Add("Dimension 1", sharedStrings.Count());
                }
                sb.AppendLine("<c r = \"B" + rowNumber + "\" s = \"14\" t = \"s\"><v>" + sharedStrings["Dimension 1"] + "</v></c>");

                if (!sharedStrings.ContainsKey("Dimension 2"))
                {
                    sharedStrings.Add("Dimension 2", sharedStrings.Count());
                }
                sb.AppendLine("<c r = \"C" + rowNumber + "\" s = \"15\" t = \"s\"><v>" + sharedStrings["Dimension 2"] + "</v></c>");
                sb.AppendLine("</row>");
                //end header
                var query = from r in tbl.AsEnumerable()
                            select r.Field<string>("Name");
                List<string> metricvalues = query.Distinct().ToList();
                foreach (string metric in metricvalues)
                {
                    if (advancedAnalyticsParams.Comparisonlist.Contains(Convert.ToString(metric)))
                    {
                        ChannelRetailerlist.Add(Convert.ToString(metric));
                    }
                    else
                    {
                        Variablelist.Add(Convert.ToString(metric));
                    }
                }
                //plot retailer channels
                if (ChannelRetailerlist != null && ChannelRetailerlist.Count > 0)
                {
                    rowNumber += 1;
                    sb.AppendLine("<row r = \"" + rowNumber + "\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
                    if (!sharedStrings.ContainsKey("COMPARISON POINTS"))
                    {
                        sharedStrings.Add("COMPARISON POINTS", sharedStrings.Count());
                    }
                    sb.AppendLine("<c r = \"A" + rowNumber + "\" s = \"16\" t = \"s\"><v>" + sharedStrings["COMPARISON POINTS"] + "</v></c>");

                    sb.AppendLine("<c r = \"B" + rowNumber + "\" s = \"16\"></c>");

                    sb.AppendLine("<c r = \"C" + rowNumber + "\" s = \"16\"></c>");
                    sb.AppendLine("</row>");

                    foreach (string metric in ChannelRetailerlist)
                    {
                        var query2 = from r in tbl.AsEnumerable()
                                     where r.Field<string>("Name") == metric
                                     select r;
                        List<DataRow> rows = query2.ToList();
                        foreach (DataRow row in rows)
                        {
                            rowNumber += 1;
                            sb.AppendLine("<row r = \"" + rowNumber + "\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
                            if (!sharedStrings.ContainsKey(cf.cleanExcelXML(Get_ShortNames(Convert.ToString(row["Name"])))))
                            {
                                sharedStrings.Add(cf.cleanExcelXML(Get_ShortNames(Convert.ToString(row["Name"]))), sharedStrings.Count());
                            }
                            sb.AppendLine("<c r = \"A" + rowNumber + "\" s = \"17\" t = \"s\"><v>" + sharedStrings[Get_ShortNames(Convert.ToString(row["Name"]))] + "</v></c>");

                            sb.AppendLine("<c r = \"B" + rowNumber + "\" s = \"18\"><v>" + CommonFunctions.GetRoundingValue(Convert.ToString(row["Dim1"])) + "</v></c>");

                            sb.AppendLine("<c r = \"C" + rowNumber + "\" s = \"19\"><v>" + CommonFunctions.GetRoundingValue(Convert.ToString(row["Dim2"])) + "</v></c>");
                            sb.AppendLine("</row>");
                        }
                    }
                }
                //plot variables
                if (Variablelist != null && Variablelist.Count > 0)
                {
                    rowNumber += 1;
                    sb.AppendLine("<row r = \"" + rowNumber + "\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
                    if (!sharedStrings.ContainsKey("Variables"))
                    {
                        sharedStrings.Add("Variables", sharedStrings.Count());
                    }
                    sb.AppendLine("<c r = \"A" + rowNumber + "\" s = \"16\" t = \"s\"><v>" + sharedStrings["Variables"] + "</v></c>");

                    sb.AppendLine("<c r = \"B" + rowNumber + "\" s = \"16\"></c>");

                    sb.AppendLine("<c r = \"C" + rowNumber + "\" s = \"16\"></c>");
                    sb.AppendLine("</row>");

                    foreach (string metric in Variablelist)
                    {
                        var query2 = from r in tbl.AsEnumerable()
                                     where r.Field<string>("Name") == metric
                                     select r;
                        List<DataRow> rows = query2.ToList();
                        foreach (DataRow row in rows)
                        {
                            rowNumber += 1;
                            sb.AppendLine("<row r = \"" + rowNumber + "\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");

                            if (!sharedStrings.ContainsKey(cf.cleanExcelXML(Get_ShortNames(Convert.ToString(row["Name"])))))
                            {
                                sharedStrings.Add(cf.cleanExcelXML(Get_ShortNames(Convert.ToString(row["Name"]))), sharedStrings.Count());
                            }
                            sb.AppendLine("<c r = \"A" + rowNumber + "\" s = \"17\" t = \"s\"><v>" + sharedStrings[cf.cleanExcelXML(Get_ShortNames(Convert.ToString(row["Name"])))] + "</v></c>");

                            sb.AppendLine("<c r = \"B" + rowNumber + "\" s = \"18\"><v>" + CommonFunctions.GetRoundingValue(Convert.ToString(row["Dim1"])) + "</v></c>");

                            sb.AppendLine("<c r = \"C" + rowNumber + "\" s = \"19\"><v>" + CommonFunctions.GetRoundingValue(Convert.ToString(row["Dim2"])) + "</v></c>");
                            sb.AppendLine("</row>");
                        }
                    }
                }
            }
            //Retailer 1
            sb.AppendLine("<row r = \"1002\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
            if (!sharedStrings.ContainsKey("Retailer 1"))
            {
                sharedStrings.Add("Retailer 1", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BA1002\" t = \"s\"><v>" + sharedStrings["Retailer 1"] + "</v></c>");
            sb.AppendLine("<c r = \"BB1002\"><v>-0.86180034320878396</v></c>");
            sb.AppendLine("<c r = \"BC1002\"><v>0.81269412516013395</v></c>");
            if (!sharedStrings.ContainsKey("Variable 1"))
            {
                sharedStrings.Add("Variable 1", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BD1002\" t = \"s\"><v>" + sharedStrings["Variable 1"] + "</v></c>");
            sb.AppendLine("<c r = \"BE1002\"><v>9</v></c>");
            sb.AppendLine("<c r = \"BF1002\"><v>-0.19464588724233101</v></c>");
            sb.AppendLine("</row>");
            //

            //Retailer 2
            sb.AppendLine("<row r = \"1003\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
            if (!sharedStrings.ContainsKey("Retailer 2"))
            {
                sharedStrings.Add("Retailer 2", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BA1003\" t = \"s\"><v>" + sharedStrings["Retailer 2"] + "</v></c>");
            sb.AppendLine("<c r = \"BB1003\"><v>5</v></c>");
            sb.AppendLine("<c r = \"BC1003\"><v>-1.41363060194888</v></c>");
            if (!sharedStrings.ContainsKey("Variable 2"))
            {
                sharedStrings.Add("Variable 2", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BD1003\" t = \"s\"><v>" + sharedStrings["Variable 2"] + "</v></c>");

            sb.AppendLine("<c r = \"BE1003\"><v>-0.504083112294747</v></c>");
            sb.AppendLine("<c r = \"BF1003\"><v>1.1549471657271999</v></c>");

            if (!sharedStrings.ContainsKey("X-axis"))
            {
                sharedStrings.Add("X-axis", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BH1003\" s = \"2\" t = \"s\"><v>" + sharedStrings["X-axis"] + "</v></c>");

            sb.AppendLine("<c r = \"BI1003\" s = \"3\"><f t = \"array\" ref = \"BI1003\">ROUNDUP(MAX(BB1002:BB1100,BE1002:BE1100)+0.2,0)</f><v>10</v></c>");
            sb.AppendLine("<c r = \"BJ1003\" s = \"3\"><f t = \"array\" ref = \"BJ1003\">ROUNDUP(MIN(BB1002:BB1100,BE1002:BE1100)-0.2,0)</f><v>-2</v></c>");
            sb.AppendLine("</row>");
            //

            //Retailer 3
            sb.AppendLine("<row r = \"1004\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
            if (!sharedStrings.ContainsKey("Retailer 3"))
            {
                sharedStrings.Add("Retailer 3", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BA1004\" t = \"s\"><v>" + sharedStrings["Retailer 3"] + "</v></c>");
            sb.AppendLine("<c r = \"BB1004\"><v>1.6386480918776201</v></c>");
            sb.AppendLine("<c r = \"BC1004\"><v>0.50718649885323697</v></c>");
            if (!sharedStrings.ContainsKey("Variable 3"))
            {
                sharedStrings.Add("Variable 3", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BD1004\" t = \"s\"><v>" + sharedStrings["Variable 3"] + "</v></c>");

            sb.AppendLine("<c r = \"BE1004\"><v>3</v></c>");
            sb.AppendLine("<c r = \"BF1004\"><v>3</v></c>");

            if (!sharedStrings.ContainsKey("Y-axis"))
            {
                sharedStrings.Add("Y-axis", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BH1004\" s = \"2\" t = \"s\"><v>" + sharedStrings["Y-axis"] + "</v></c>");

            sb.AppendLine("<c r = \"BI1004\" s = \"3\"><f t = \"array\" ref = \"BI1004\">ROUNDUP(MAX(BC1002:BC1100,BF1002:BF1100)+0.2,0)</f><v>6</v></c>");
            sb.AppendLine("<c r = \"BJ1004\" s = \"3\"><f t = \"array\" ref = \"BJ1004\">ROUNDUP(MIN(BC1002:BC1100,BF1002:BF1100)-0.2,0)</f><v>-2</v></c>");
            sb.AppendLine("</row>");
            //

            //Retailer 4
            sb.AppendLine("<row r = \"1005\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
            if (!sharedStrings.ContainsKey("Retailer 4"))
            {
                sharedStrings.Add("Retailer 4", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BA1005\" t = \"s\"><v>" + sharedStrings["Retailer 4"] + "</v></c>");
            sb.AppendLine("<c r = \"BB1005\"><v>-0.80428905847279897</v></c>");
            sb.AppendLine("<c r = \"BC1005\"><v>-1.21687690440339</v></c>");
            if (!sharedStrings.ContainsKey("Variable 4"))
            {
                sharedStrings.Add("Variable 4", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BD1005\" t = \"s\"><v>" + sharedStrings["Variable 4"] + "</v></c>");

            sb.AppendLine("<c r = \"BE1005\"><v>4</v></c>");
            sb.AppendLine("<c r = \"BF1005\"><v>5</v></c>");
            sb.AppendLine("</row>");
            //

            //Retailer 4
            sb.AppendLine("<row r = \"1006\" spans = \"1:60\" ht = \"12.75\" customHeight = \"1\" thickTop = \"1\" thickBot = \"1\" x14ac:dyDescent = \"0.3\">");
            if (!sharedStrings.ContainsKey("Variable 5"))
            {
                sharedStrings.Add("Variable 5", sharedStrings.Count());
            }
            sb.AppendLine("<c r = \"BD1006\" t = \"s\"><v>" + sharedStrings["Variable 5"] + "</v></c>");

            sb.AppendLine("<c r = \"BE1006\"><v>3</v></c>");
            sb.AppendLine("<c r = \"BF1006\"><v>2</v></c>");
            sb.AppendLine("</row>");
            //

            modulegraphicFrames[0].InnerXml = sb.ToString();
            xmlChart.Save(xmlpath);
        }

        public string ColumnIndexToName(int columnIndex)
        {
            char second = (char)(((int)'A') + columnIndex % 26);

            columnIndex /= 26;

            if (columnIndex == 0)
                return second.ToString();
            else
                return ((char)(((int)'A') - 1 + columnIndex)).ToString() + second.ToString();
        }
        private string Get_ShortNames(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (HeaderTabs.ContainsKey(spVal))
                    slRetVal = HeaderTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        private void PopulateShortNames()
        {
            HeaderTabs.Clear();

            HeaderTabs.Add("Store brand/private label (diet)", "Store brand/private label (DIET)");
            HeaderTabs.Add("Store brand/private label (regular)", "Store brand/private label (REGULAR)");
            HeaderTabs.Add("smartwater", "SmartWater");
            HeaderTabs.Add("vitaminwater", "Vitamin Water");
            HeaderTabs.Add("Gender ", "Gender");
            HeaderTabs.Add("FactAgeGroups", "Age");
            HeaderTabs.Add("FactAgeGender", "Age-Gender");
            HeaderTabs.Add("ItemsPurchased", "# of Items Purchased");
            HeaderTabs.Add("Ethnicity", "Race/Ethnicity");
            HeaderTabs.Add("HHTotal", "HH Size - Total");
            HeaderTabs.Add("HHAdults", "HH Size - Adults in HH");
            HeaderTabs.Add("HHChildren", "HH Size - Children in HH");
            HeaderTabs.Add("MaritalStatus", "Marital Status");
            HeaderTabs.Add("HHIncomeGroups", "HH Income");
            HeaderTabs.Add("EmployeeStatus2", "Employment Status");
            HeaderTabs.Add("Education ", "Education ");

            HeaderTabs.Add("DayParts", "Daypart of Trip");
            HeaderTabs.Add("DayofWeek", "Day of Week");
            HeaderTabs.Add("PreTripOrigin", "Pre-Trip Origin");
            HeaderTabs.Add("OtherStoreConsidered", "Consideration of Another Store");
            HeaderTabs.Add("ReasonForStoreChoice", "Reasons for Store Choice - Top 2 Box");
            HeaderTabs.Add("VisitPlans", "Pre Trip Planning");
            HeaderTabs.Add("VisitPreparation", "Preparation Types");
            HeaderTabs.Add("TechnologyUsed", "Use of Technology to Prepare");
            HeaderTabs.Add("ComputerBased", "Computer-Based Preparation Activities");
            HeaderTabs.Add("SmartPhoneBased", "Smartphone-Based Preparation Activities");
            HeaderTabs.Add("DestinationItemSummary", "Destination Item Summary");
            HeaderTabs.Add("DestinationItemDetails", "Destination Item Detail");

            HeaderTabs.Add("A supermarket or grocery store", "Supermarket / Grocery");
            HeaderTabs.Add("A convenience store or gas station food mart (excluding gas)", "Convenience");
            HeaderTabs.Add("A drug store", "Drug");
            HeaderTabs.Add("A dollar store such as Family Dollar or Dollar General", "Dollar");
            HeaderTabs.Add("A warehouse club such as Sam`s Club or Costco", "Club");
            HeaderTabs.Add("A Mass Merchandise store or super center such as walmart, target, walmart supercenter, or supertarget", "Mass Merc. with Supers");
            HeaderTabs.Add("A mass merchandise store without a full-line grocery section such as Walmart or Target", "Mass Merc.");
            HeaderTabs.Add("A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget", "Supercenter");

            HeaderTabs.Add("Shopper Attitude", "Top 2 Box Attitudinal Statements");
            HeaderTabs.Add("Attitudinal Segment", "Attitudinal Segment");

            HeaderTabs.Add("RetailerLoyaltyPyramid", "Retailer Loyalty Pyramid - Total Grocery Across Channel(<span style=\"font-size:15px;\">Applicable only for Retailers</span>)");
            HeaderTabs.Add("TopBox", "Loyalty and Satisfaction Detail(<span style=\"font-size:15px;\">Applicable only for Retailers</span>)");
            HeaderTabs.Add("MainFavoriteStore", "Main/Favorite Store for Grocery Spending(<span style=\"font-size:15px;\">Applicable only for Retailers</span>)");

            HeaderTabs.Add("shoppingpercent", "% HH Shopping Personally Responsible For");
            HeaderTabs.Add("SmartphoneTabletOwnership", "Smartphone/Tablet Ownership");
            HeaderTabs.Add("CrossDeviceOwnership", "Cross-Device Ownership");
            HeaderTabs.Add("SampleSize", "Sample Size");

            HeaderTabs.Add("TopBoxSatisfaction SampleSize", "Sample Size - Top Box Satisfaction");
            HeaderTabs.Add("TopBoxSatisfaction", "Top Box Satisfaction");
            HeaderTabs.Add("TopBoxLikeability SampleSize", "Sample Size - Top Box Likeability");
            HeaderTabs.Add("TopBoxWillingnesstoRecommend", "Top Box Willingness to Recommend");

            HeaderTabs.Add("TopBoxWillingnesstoRecommend SampleSize", "Sample Size - Top Box Willingness to Recommend");
            HeaderTabs.Add("TopBoxEarnedLoyalty", "Top Box Earned Loyalty");
            HeaderTabs.Add("TopBoxEarnedLoyalty SampleSize", "Sample Size - Top Box Earned Loyalty");
            HeaderTabs.Add("MainStore SampleSize", "Sample Size - Main Store");
            HeaderTabs.Add("MainStore", "Main Store");

            HeaderTabs.Add("MainStoreOverAll", "Main Store OverAll");
            HeaderTabs.Add("MainStoreOverAll SampleSize", "Sample Size - Main Store OverAll");
            HeaderTabs.Add("FavoriteStore", "Favorite Store");
            HeaderTabs.Add("FavoriteStore SampleSize", "Sample Size - Favorite Store");

            HeaderTabs.Add("FavoriteStoreOverAll", "Favorite Store OverAll");
            HeaderTabs.Add("FavoriteStoreOverAll SampleSize", "Sample Size - Favorite Store OverAll");

            HeaderTabs.Add("StoreAttribute", "Store Attributes");
            HeaderTabs.Add("GoodPlaceToShop", "Good Place To Shop");
            HeaderTabs.Add("InstorePurchaseInfluence", "In-Store Influencers");
            HeaderTabs.Add("Smartphone/TabletInfluencedPurchases", "Smartphone/Tablet Influenced Purchases?");

            HeaderTabs.Add("SmartPhoneUsage", "Ways Technology Used to Influence");
            HeaderTabs.Add("BeverageBrandsPurchased", "Beverage Brands Purchased: SSD");

            HeaderTabs.Add("ItemsPurchasedSummary", "Items Purchased Summary");
            HeaderTabs.Add("InStoreDestinationDetails", "Items Purchased Detail");
            HeaderTabs.Add("ImpulseItem", "Impulse Item");
            HeaderTabs.Add("TripMission", "Trip Mission");

            HeaderTabs.Add("WayTabletInfluenced", "Way Tablet Influenced");
            HeaderTabs.Add("WaySmartphoneInfluenced", "Way Smartphone Influenced");

            HeaderTabs.Add("TimeSpent", "Time Spent in Store");
            HeaderTabs.Add("TripExpenditure", "Trip Expenditure");
            HeaderTabs.Add("CheckOutType", "Checkout Method");
            HeaderTabs.Add("ConsideredStoreVisits", "Considered Store Visits");
            HeaderTabs.Add("VisitMotiviations", "Visit Motiviations");
            HeaderTabs.Add("tabletBased", "Tablet Based");
            HeaderTabs.Add("MostImportantDestinationItems", "Most Important Destination Items");
            HeaderTabs.Add("PaymentMode", "Method of Payment and Store Cards");
            HeaderTabs.Add("RetailerLoyaltyPyramid(Base:CouldShop)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)");

            HeaderTabs.Add("RedeemedCoupon", "Coupon Redemption");
            HeaderTabs.Add("RedeemedCouponTypes", "Type of Coupons Redeemed");
            HeaderTabs.Add("DestinationStoreTrip", "Destination Following Store Trip");
            HeaderTabs.Add("TripSatisfaction", "Trip Satisfaction");
            HeaderTabs.Add("TripAttributeSatisfaction", "Trip Attribute Satisfaction - Top 2 Box");
            HeaderTabs.Add("Diet carbonated soft drinks", "Diet SSD");
            HeaderTabs.Add("Regular (non-diet) carbonated soft drinks", "REG SSD");
            HeaderTabs.Add("Bottled water", "Water");
            HeaderTabs.Add("Juice or juice drinks", "Juice");
            HeaderTabs.Add("Iced tea in bottles, cans, or cartons", "Iced Tea");
            HeaderTabs.Add("Coffee in bottles or cans", "Coffee");
            HeaderTabs.Add("Sports drinks", "Sports Drinks");
            HeaderTabs.Add("Energy drinks", "Energy Drinks");
            HeaderTabs.Add("P3M+ Channel Shopping Frequency", "Quarterly+ Channel Shopping Frequency");
            HeaderTabs.Add("P3M+ Priority Store Shopping Frequency", "Quarterly+ Priority Store Shopping Frequency");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(REG SSD)", "Do not purchase(REG SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(DIET SSD)", "Do not purchase(DIET SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(WATER)", "Do not purchase(WATER)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(JUICE)", "Do not purchase(JUICE)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(SPORTS DRINKS)", "Do not purchase(SPORTS DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(ENERGY DRINKS)", "Do not purchase(ENERGY DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD TEA)", "Do not purchase(RTD TEA)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD COFFEE)", "Do not purchase(RTD COFFEE)");
        }
        private string FormateDateAndTime(string month)
        {
            if (month.Length == 1)
            {
                return "0" + month;
            }
            else
                return month;
        }
        private void CopyFilesToDestination()
        {
            string source = HttpContext.Current.Server.MapPath("~/CorrespondenceExportExcelFiles/Excel Template");

            UserParams userparam = HttpContext.Current.Session[SessionVariables.USERID] as UserParams;
            if (userparam == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");
                }
                else
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                }
            }
            UserExportFileName = "~/CorrespondenceExportExcelUsersFiles/" + userparam.UserName + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UserExportFileName));
            }

            string destination = HttpContext.Current.Server.MapPath(UserExportFileName);
            DirectoryCopy(source, destination, true);
        }
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {

            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public void ZipDirectory(string sourceDirectory, string zipFileName)
        {
            (new FastZip()).CreateZip(zipFileName, sourceDirectory, true, null);
            //ICSharpCode.SharpZipLib.GZip.GZipInputStream ff;
        }
    }
}