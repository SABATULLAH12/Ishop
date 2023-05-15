using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using iSHOPNew.DAL;
using Aspose.Slides;
using Aspose.Slides.Export;
using Svg;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using Aspose.Slides.Charts;
using Microsoft.Office.Core;
using interop = Microsoft.Office.Interop.PowerPoint;

namespace iSHOPNew.Models
{
    public class Demographic
    {
        DataAccess dal = null;
        DataSet ds = null;
        List<string> objectivelist = null;
        List<string> metriclist = null;

        public static Random rnd = new Random();

        public double accuratestatvalueposi;
        public double accuratestatvaluenega;

        float chart_x_position = (float)11.05;
        float chart_y_position = (float)100.34;
        float chart_width = (float)937.13;
        float chart_height = (float)387.21;
        public float table_width
        {
            get
            {
                return (float)896.88;
            }
        }
        IEnumerable<double> dblCols = null;
        IEnumerable<double> dblRows = null;
        public Demographic()
        {
            accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
            accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
        }

        public string cacheString(object[] obj)
        {
            string output = "";

            for (var i = 0; i < obj.Length; i++)
            {
                output += ((obj[i] != null ? obj[i].ToString() : string.Empty) + ",");
            }
            return output;
        }

        public PathToPurchaseMetrics GetData(PathToPurchaseParams demogDashboardData)
        {
            PathToPurchaseMetrics demographicMetrics = null;
            string cachename = string.Empty;
            List<string> metrictypes = null;
            dal = new DataAccess();
            object[] paramvalues = null;
            var spName = "";
            try
            {
                paramvalues = new object[] { demogDashboardData.CustomBase_UniqueId, demogDashboardData.Comparison_UniqueIds, demogDashboardData.TimePeriod_UniqueId,
                demogDashboardData.ShopperSegment_UniqueId, demogDashboardData.ShopperFrequency_UniqueId,demogDashboardData.CustomBaseAdvancedFilters_UniqueId,demogDashboardData.CustomBaseShopperFrequency_UniqueId,demogDashboardData.Sigtype_UniqueId, demogDashboardData.Sort,demogDashboardData.TabType,
                demogDashboardData.CompetitorFrequency_UniqueId,demogDashboardData.CompetitorRetailer_UniqueId,demogDashboardData.CustomBaseCompetitorFrequency_UniqueId,demogDashboardData.CustomBaseCompetitorRetailer_UniqueId,demogDashboardData.IsOnlineSelected,demogDashboardData.IsOnlineSelectedAsBase};
                //if(demogDashboardData.TabType.ToLower() == "trips")
                //{
                //    spName = "usp_IshopDemographicDashboardTrips";
                //}
                //else
                //{
                //    spName = "usp_IshopDemographicDashboardShopper";
                //}
                cachename = "usp_IshopDemographicDashboard" + cacheString(paramvalues);
                var dashbrdInfo = HttpContext.Current.Application[cachename] as PathToPurchaseMetrics;
                if (dashbrdInfo != null)
                    return dashbrdInfo;
                spName = "usp_IshopDemographicDashboard";
                ds = dal.GetData_WithIdMapping(paramvalues, spName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    demographicMetrics = new PathToPurchaseMetrics();
                    demographicMetrics.SampleSize = Convert.ToString(ds.Tables[0].Rows[0]["respcnt"]).FormateSampleSizeNumber();
                    demographicMetrics.StatTestSampleSize = Convert.ToString(ds.Tables[0].Rows[1]["respcnt"]).FormateSampleSizeNumber();
                    demographicMetrics.pathToPurchaseMetricEntitylist = new List<PathToPurchaseMetricEntity>();
                    metrictypes = (from r in ds.Tables[1].AsEnumerable()
                                   select Convert.ToString(r["MetricType"])).Distinct().ToList();
                    foreach (string metrictype in metrictypes)
                    {
                        demographicMetrics.pathToPurchaseMetricEntitylist.Add(new PathToPurchaseMetricEntity()
                        {
                            MetricType = metrictype
                        });
                    }

                    if (demographicMetrics.pathToPurchaseMetricEntitylist != null && demographicMetrics.pathToPurchaseMetricEntitylist.Count > 0)
                    {
                        foreach (PathToPurchaseMetricEntity ppm in demographicMetrics.pathToPurchaseMetricEntitylist)
                        {
                            ppm.MetricData = (from r in ds.Tables[1].AsEnumerable()
                                              where Convert.ToString(r["MetricType"]).Equals(ppm.MetricType)
                                              select new PathToPurchaseEntity()
                                              {
                                                  Retailer = Convert.ToString(r["Retailer"]),
                                                  MetricType = Convert.ToString(r["MetricType"]),
                                                  Metric = Convert.ToString(r["Metric"]),
                                                  Volume = string.IsNullOrEmpty(Convert.ToString(r["Volume"])) ? 0 : Convert.ToDouble(r["Volume"]),
                                                  Significance = string.IsNullOrEmpty(Convert.ToString(r["Significance"])) ? 0 : Convert.ToDouble(r["Significance"]),
                                                  ChangeVolume = string.IsNullOrEmpty(Convert.ToString(r["ChangeVolume"])) ? 0 : Convert.ToDouble(r["ChangeVolume"]),
                                                  Flag = Convert.ToInt32(r["Flag"])
                                              }).Distinct().ToList();

                            if (ppm.MetricData != null && ppm.MetricData.Count > 0)
                            {
                                ppm.MetricData[0].Selected_Popup_Metric_Item = true;

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                demographicMetrics = null;
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            if (demographicMetrics != null)
            {
                HttpContext.Current.Application.Add(cachename, demographicMetrics);
            }
            return demographicMetrics;
        }

        public string ExportToDemogDashboardPPT(string filepath, string destFile, P2PDashboardData filter, HttpContextBase context)
        {
            try
            {
                Aspose.Slides.License license = new Aspose.Slides.License();
                //Pass only the name of the license file embedded in the assembly
                license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
                using (Presentation pres = new Presentation(filepath))
                {
                    //Master Slide : Stat test, Sample size
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "ss").FirstOrDefault()).TextFrame.Text = "Sample Size – " + filter.ss.ToString("#,##,##0");
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = "* Stat tested at 95% CL against : " + filter.statTest.ToUpper();
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "leftPane").FirstOrDefault()).TextFrame.Text = filter.LeftpanelData.ToUpper();
                    if (filter.TabType == "Trips")
                    {
                        if (filter.ShopperFrequency == "TOTAL VISITS")
                        {                            
                            ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips, Sorted by: Largest " + filter.Sort + (", Time Period: " + filter.TimePeriod ) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                        }
                        else
                        {
                            var frequency = (filter.ShopperFrequency != "" && filter.ShopperFrequency != null) ? "(" + filter.ShopperFrequency + ")" : "";                           
                            ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips " + frequency + ", Sorted by: Largest " + filter.Sort +(", Time Period: " + filter.TimePeriod )+ ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                        }
                    }
                    else
                    {
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - " + filter.ShopperFrequency + " Shopper" + ", Sorted by: Largest " + filter.Sort + (", Time Period: " + filter.TimePeriod ) +("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                    }
                    //Replace Establishment logo
                    string loc = "~/Images/P2PDashboardEsthmtImages/" + replace_file_special_characters(filter.OutputData.FirstOrDefault().MetricData.FirstOrDefault().Retailer) + ".svg";
                    imageReplace((PictureFrame)pres.Slides[0].Shapes.Where(x => x.Name == "logo").FirstOrDefault(), loc, context, 8, -2);

                    //Gender
                    fillGender(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "gender").ToList()[0].MetricData.ToList(), filter.ss);
                    //Age
                    fillAge(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "age").ToList()[0].MetricData.ToList(), filter.ss);
                    //Ethnicity
                    fillEthnicity(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "ethnicity").ToList()[0].MetricData.ToList(), filter.ss);
                    //Density
                    fillOcc(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "density").ToList()[0].MetricData.ToList(), filter.pptOrPdf == "pdf", filter.ss);
                    //DHS
                    fillSHS(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "hh size - total").ToList()[0].MetricData.ToList(), filter.ss);
                    //MS
                    fillMS(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "marital status").ToList()[0].MetricData.ToList(), filter.ss);
                    //PS
                    fillPS(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "parental identification").ToList()[0].MetricData.ToList(), filter.ss);
                    //HH
                    fillHH(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "hh income").ToList()[0].MetricData.ToList(), filter.ss);
                    //SE
                    fillSE(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "socio economic").ToList()[0].MetricData.ToList(), filter.ss);
                    //Diner Attitude
                    fillDA(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "attitudinal statements - top 2 box").ToList()[0].MetricData.ToList(), filter.pptOrPdf == "pdf", filter.ss);
                    //Avg Monthly Channel Visit
                    fillAMCV(pres.Slides[0], filter.OutputData.Where(x => x.MetricType.ToLower() == "average monthy channel visit").ToList()[0].MetricData.ToList(), filter.pptOrPdf == "pdf", filter.ss);
                    if (filter.pptOrPdf == "pdf")
                    {
                        //Check if office is installed
                        if (isPPT_Installed())
                        {

                            pres.Save(context.Server.MapPath("~/Temp/DemogPDF"), Aspose.Slides.Export.SaveFormat.Pptx);
                        }
                        else
                        {
                            PdfOptions pdfop = new PdfOptions();
                            pdfop.SufficientResolution = 10000;
                            pdfop.EmbedFullFonts = true;
                            pdfop.EmbedTrueTypeFontsForASCII = true;
                            pdfop.TextCompression = PdfTextCompression.Flate;
                            pdfop.JpegQuality = 100;
                            pdfop.SaveMetafilesAsPng = true;
                            pdfop.Compliance = PdfCompliance.Pdf15;
                            pres.Save(destFile, Aspose.Slides.Export.SaveFormat.Pdf, pdfop);
                        }
                    }
                    else
                    {
                        pres.Save(destFile, Aspose.Slides.Export.SaveFormat.Pptx);
                    }
                }
                if (filter.pptOrPdf == "pdf")
                {
                    if (isPPT_Installed())
                    {
                        //Download the interop way
                        var app = new interop.Application();
                        var ppt = app.Presentations;
                        var file = ppt.Open(context.Server.MapPath("~/Temp/DemogPDF"), MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoFalse);
                        file.SaveCopyAs(destFile, interop.PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoTrue);
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return "";
        }
        public void imageReplace(PictureFrame tempImg, string loc, HttpContextBase context, int indx, int fact)
        {
            string pathToSvg = context.Server.MapPath(loc);
            //Create a directory
            string subPath = "~/Images/temp/" + context.Session.SessionID; // your code goes here
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
                if (fact == -1) //For bevearge and food items
                {
                    xyz.Height = 300;
                    xyz.Width = 300;
                }
                else
                {
                    double ratio = 1;
                    double widthRatio = 1;
                    if (fact == -2) //For Establishment logo
                    {
                        //xyz.Height = 200;// 4 * xyz.Bounds.Height;
                        //xyz.Width = 200;// * xyz.Bounds.Width;
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
                    else
                    {
                        xyz.Height = 4 * xyz.Bounds.Height;
                        xyz.Width = 4 * xyz.Bounds.Width;
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

        public string replace_file_special_characters(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
                filename = Regex.Replace(filename, "[&/\\#,+()$~%.':*?<>{}]+", "-");
            return filename;
        }

        public void fillGender(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            setMCvalue(((IAutoShape)((IGroupShape)sld.Shapes.Where(x => x.Name == "gender1").FirstOrDefault()).Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[0], TextAlignment.Center, FontAlignment.Center, sampleSize);//Male
            ((IAutoShape)((IGroupShape)sld.Shapes.Where(x => x.Name == "gender1").FirstOrDefault()).Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[0].Metric;
            setColorForElement(((IAutoShape)((IGroupShape)sld.Shapes.Where(x => x.Name == "gender1").FirstOrDefault()).Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[0].Flag == 1);
            setMCvalue(((IAutoShape)((IGroupShape)sld.Shapes.Where(x => x.Name == "gender2").FirstOrDefault()).Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[1], TextAlignment.Center, FontAlignment.Center, sampleSize);//Male
            ((IAutoShape)((IGroupShape)sld.Shapes.Where(x => x.Name == "gender2").FirstOrDefault()).Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[1].Metric;
            setColorForElement(((IAutoShape)((IGroupShape)sld.Shapes.Where(x => x.Name == "gender2").FirstOrDefault()).Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[1].Flag == 1);
            //Fill Chart
            IChart chrt = ((IChart)((IGroupShape)sld.Shapes.Where(x => x.Name == "genderChart").FirstOrDefault()).Shapes.Where(x => x.Name == "chart").FirstOrDefault());
            chrt.ChartData.Series[0].DataPoints[0].Value.Data = data[1].Volume / 100;
            chrt.ChartData.Series[0].DataPoints[1].Value.Data = 1 - data[1].Volume / 100;
        }

        public void fillAge(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            int i = 0;
            //Fill Chart
            IChart chrt = ((IChart)((IGroupShape)sld.Shapes.Where(x => x.Name == "ageChart").FirstOrDefault()).Shapes.Where(x => x.Name == "chart").FirstOrDefault());
            for (i = 0; i < data.Count; i++)
            {
                IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "age" + (i + 1)).FirstOrDefault();
                setMCvalue(((IAutoShape)gp.Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[i], TextAlignment.Left, FontAlignment.Center, sampleSize);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[i].Metric;
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[i].Flag == 1);
                chrt.ChartData.Series[0].DataPoints[i].Value.Data = data[data.Count - 1 - i].Volume / 100;
            }
        }

        public void fillEthnicity(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            int i = 0;
            for (i = 0; i < data.Count; i++)
            {
                IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "eth" + (i + 1)).FirstOrDefault();
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "m").FirstOrDefault()).TextFrame.Text = data[i].Volume.ToString("##,##0") + "%";
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Text = getFormattedChangeVal(data[i].ChangeVolume);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data[i].Significance, sampleSize);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[i].Metric;
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[i].Flag == 1);
            }
        }

        public void fillOcc(ISlide sld, List<PathToPurchaseEntity> data, bool isPdf, double? sampleSize)
        {
            IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "occ").FirstOrDefault();
            IChart chrt = (IChart)gp.Shapes.Where(x => x.Name == "occChart").FirstOrDefault();
            for (int i = 0; i < data.Count; i++)
            {
                chrt.ChartData.ChartDataWorkbook.GetCell(0, 1 + i, 1, data[data.Count - 1 - i].Volume < 0 ? 0.0 : Math.Round(data[data.Count - 1 - i].Volume));
                chrt.ChartData.ChartDataWorkbook.GetCell(0, 1 + i, 2, data[data.Count - 1 - i].Volume < 0 ? 100.0 : Math.Round(100 - data[data.Count - 1 - i].Volume));
                if (isPdf)
                {
                    chrt.ChartData.Series[0].DataPoints[i].Label.TextFrameForOverriding.Paragraphs[0].Portions[0].Text = data[data.Count - 1 - i].Volume < 0 ? "0%" : data[data.Count - 1 - i].Volume.ToString("##,##0");
                }
                chrt.ChartData.Series[0].DataPoints[i].Label.TextFrameForOverriding.Paragraphs[0].Portions[1].Text = "% | ";
                chrt.ChartData.Series[0].DataPoints[i].Label.TextFrameForOverriding.Paragraphs[0].Portions[2].Text = data[data.Count - 1 - i].Volume < 0 ? "0.0" : getFormattedChangeVal(data[data.Count - 1 - i].ChangeVolume);
                var tempDP_Label = chrt.ChartData.Series[0].DataPoints[i].Label.TextFrameForOverriding.Paragraphs[0].Portions;
                tempDP_Label[2].PortionFormat.FillFormat.FillType = FillType.Solid;
                tempDP_Label[2].PortionFormat.FillFormat.SolidFillColor.Color = data[data.Count - 1 - i].Volume < 0 ? Color.Black : getColorBasedOnSignificance(data[data.Count - 1 - i].Significance, sampleSize);
                //Set Label position
                chrt.ChartData.Series[0].DataPoints[i].Label.X = (float)0.68;
                chrt.ChartData.Series[0].DataPoints[i].Label.Y = (float)0.0;
                chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.TextFormat.ParagraphFormat.FontAlignment = FontAlignment.Center;
                chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.TextFormat.ParagraphFormat.Alignment = TextAlignment.Center;
                //Update the active widget
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r" + (i + 1)).FirstOrDefault()), data[i].Flag == 1);
            }
        }

        public void fillSHS(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            int i = 0;
            //Fill first one
            IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "dhs1").FirstOrDefault();
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "m").FirstOrDefault()).TextFrame.Text = data[0].Volume.ToString("##,##0.0");
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Text = getFormattedChangeVal(data[0].ChangeVolume);
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data[0].Significance, sampleSize);
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[0].Metric;
            for (i = 1; i < data.Count; i++)
            {
                gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "dhs" + (i + 1)).FirstOrDefault();
                setMCvalue(((IAutoShape)gp.Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[i], TextAlignment.Center, FontAlignment.Center, sampleSize);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[i].Metric;
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[i].Flag == 1);
                //Update the Chart/Metric Value
                IChart cht = (IChart)gp.Shapes.Where(x => x.Name == "chart").FirstOrDefault();
                cht.ChartData.Series[0].DataPoints[0].Value.Data = data[i].Volume;
                cht.ChartData.Series[1].DataPoints[0].Value.Data = 100 - data[i].Volume;
                //Update the picture frame color
                if (data[i].Flag == 1)
                {
                    PictureFrame pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgR").FirstOrDefault());
                    pf.Hidden = true;
                    pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgRA").FirstOrDefault());
                    pf.Hidden = false;
                }
                else
                {
                    PictureFrame pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgRA").FirstOrDefault());
                    pf.Hidden = true;
                    pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgR").FirstOrDefault());
                    pf.Hidden = false;
                }
            }
        }

        public void fillMS(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            int i = 0;
            for (i = 0; i < data.Count; i++)
            {
                IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "ms" + (i + 1)).FirstOrDefault();
                setMCvalue(((IAutoShape)gp.Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[i], TextAlignment.Center, FontAlignment.Center, sampleSize);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[i].Metric;
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[i].Flag == 1);
                //Update the Chart/Metric Value
                IChart cht = (IChart)gp.Shapes.Where(x => x.Name == "chart").FirstOrDefault();
                cht.ChartData.Series[0].DataPoints[0].Value.Data = data[i].Volume;
                cht.ChartData.Series[1].DataPoints[0].Value.Data = 100 - data[i].Volume;
                //Update the picture frame color
                if (data[i].Flag == 1)
                {
                    PictureFrame pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgR").FirstOrDefault());
                    pf.Hidden = true;
                    pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgRA").FirstOrDefault());
                    pf.Hidden = false;
                }
                else
                {
                    PictureFrame pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgRA").FirstOrDefault());
                    pf.Hidden = true;
                    pf = ((PictureFrame)gp.Shapes.Where(x => x.Name == "imgR").FirstOrDefault());
                    pf.Hidden = false;
                }
            }
        }

        public void fillPS(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "ps").FirstOrDefault();
            setMCvalue(((IAutoShape)gp.Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[0], TextAlignment.Center, FontAlignment.Center, sampleSize);
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[0].Metric;
            //Update the Chart/Metric Value
            IChart cht = (IChart)gp.Shapes.Where(x => x.Name == "chart").FirstOrDefault();
            cht.ChartData.Series[0].DataPoints[0].Value.Data = data[0].Volume;
            cht.ChartData.Series[1].DataPoints[0].Value.Data = 100 - data[0].Volume;
        }

        public void fillHH(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            int i = 0;
            //Fill first one
            IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "hi1").FirstOrDefault();
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "m").FirstOrDefault()).TextFrame.Text = "$" + data[0].Volume.ToString("##,##0") + "K";
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Text = getFormattedChangeVal(data[0].ChangeVolume) + "K";
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data[0].Significance, sampleSize);
            ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[0].Metric;
            for (i = 1; i < data.Count; i++)
            {
                gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "hi" + (i + 1)).FirstOrDefault();
                setMCvalue(((IAutoShape)gp.Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[i], TextAlignment.Right, FontAlignment.Center, sampleSize);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[i].Metric;
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[i].Flag == 1);
            }
        }

        public void fillSE(ISlide sld, List<PathToPurchaseEntity> data, double? sampleSize)
        {
            int i = 0;
            for (i = 0; i < data.Count; i++)
            {
                IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "se" + (i + 1)).FirstOrDefault();
                setMCvalue(((IAutoShape)gp.Shapes.Where(x => x.Name == "mc").FirstOrDefault()), data[i], TextAlignment.Center, FontAlignment.Center, sampleSize);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[i].Metric;
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[i].Flag == 1);
            }
        }

        public void fillDA(ISlide sld, List<PathToPurchaseEntity> data, bool isPdf, double? sampleSize)
        {
            IChart chrt = (IChart)sld.Shapes.Where(x => x.Name == "daChart").FirstOrDefault();
            IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "da").FirstOrDefault();
            IGroupShape gpTxt = (IGroupShape)sld.Shapes.Where(x => x.Name == "daTxt").FirstOrDefault();
            for (int i = 0; i < data.Count; i++)
            {
                chrt.ChartData.ChartDataWorkbook.GetCell(0, 1 + i, 1, Math.Round(data[data.Count - 1 - i].Volume));
                chrt.ChartData.ChartDataWorkbook.GetCell(0, 1 + i, 2, Math.Round(100 - data[data.Count - 1 - i].Volume));
                //if (isPdf)
                //{
                //    chrt.ChartData.Series[0].DataPoints[i].Label.TextFrameForOverriding.Paragraphs[0].Portions[0].Text = data[data.Length - 1 - i].MetricValue.ToString("##,##0");
                //}
                chrt.ChartData.Series[0].DataPoints[i].Label.TextFrameForOverriding.Paragraphs[0].Portions[2].Text = getFormattedChangeVal(data[data.Count - 1 - i].ChangeVolume);
                var tempDP_Label = chrt.ChartData.Series[0].DataPoints[i].Label.TextFrameForOverriding.Paragraphs[0].Portions;
                //tempDP_Label[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                //tempDP_Label[1].PortionFormat.FillFormat.FillType = FillType.Solid;
                tempDP_Label[2].PortionFormat.FillFormat.FillType = FillType.Solid;
                //tempDP_Label[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data[data.Length - 1 - i].Significancevalue);
                //tempDP_Label[1].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data[data.Length - 1 - i].Significancevalue);
                tempDP_Label[2].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data[data.Count - 1 - i].Significance, sampleSize);
                //Set Label position
                chrt.ChartData.Series[0].DataPoints[i].Label.X = (float)0.78;
                chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.TextFormat.ParagraphFormat.FontAlignment = FontAlignment.Center;
                chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.TextFormat.ParagraphFormat.Alignment = TextAlignment.Center;
                //Update text
                ((IAutoShape)gpTxt.Shapes.Where(x => x.Name == "txt" + (i + 1)).FirstOrDefault()).TextFrame.Text = data[i].Metric;
            }
        }

        public void fillAMCV(ISlide sld, List<PathToPurchaseEntity> data, bool isPdf, double? sampleSize)
        {
            int i = 0;
            for (i = 0; i < data.Count; i++)
            {
                IGroupShape gp = (IGroupShape)sld.Shapes.Where(x => x.Name == "amc" + (i + 1)).FirstOrDefault();
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "t").FirstOrDefault()).TextFrame.Text = data[i].Metric;
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "m").FirstOrDefault()).TextFrame.Text = data[i].Volume.ToString("##,##0.0");// + "%";// data[i].Volume.ToString("##0.0");
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Text = getFormattedChangeVal(data[i].ChangeVolume);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data[i].Significance, sampleSize);
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "Rectangle").FirstOrDefault()).FillFormat.FillType = FillType.Solid;
                ((IAutoShape)gp.Shapes.Where(x => x.Name == "Rectangle").FirstOrDefault()).FillFormat.SolidFillColor.Color = getColorForChannnel(data[i].Metric);
                setColorForElement(((IAutoShape)gp.Shapes.Where(x => x.Name == "r").FirstOrDefault()), data[i].Flag == 1);
            }
        }
        public void setMCvalue(IAutoShape shp, PathToPurchaseEntity data, TextAlignment ta, FontAlignment fa, double? sampleSize)
        {
            shp.TextFrame.Paragraphs.Clear();
            shp.TextFrame.Paragraphs.Add(new Paragraph());
            shp.TextFrame.Paragraphs[0].Portions.Clear();
            IPortion prt = new Portion() { Text = data.Volume.ToString("##,##0") + "% | " };
            prt.PortionFormat.FillFormat.FillType = FillType.Solid;
            prt.PortionFormat.FontHeight = 12;
            prt.PortionFormat.FontBold = NullableBool.True;
            prt.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
            prt.PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
            shp.TextFrame.Paragraphs[0].Portions.Add(prt);
            prt = new Portion() { Text = getFormattedChangeVal(data.ChangeVolume) };
            prt.PortionFormat.FillFormat.FillType = FillType.Solid;
            prt.PortionFormat.FontHeight = 10;
            prt.PortionFormat.FontBold = NullableBool.True;
            prt.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
            prt.PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificance(data.Significance, sampleSize);
            shp.TextFrame.Paragraphs[0].Portions.Add(prt);
            //Alignment
            shp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = ta;
            shp.TextFrame.Paragraphs[0].ParagraphFormat.FontAlignment = fa;
        }

        public Color getColorBasedOnSignificance(double? i, double? samplesize)
        {
            if (i == null) return Color.Black;
            if (i < accuratestatvaluenega) return Color.Red;
            if (i > accuratestatvalueposi) return Color.Green;
            if (samplesize >= GlobalVariables.LowSample && samplesize < 100)
                return Color.Gray;
            return Color.Black;
        }

        public System.Drawing.Color getColorForChannnel(string ch)
        {
            System.Drawing.Color color = new System.Drawing.Color();
            switch (ch.ToLower())
            {
                case "grocery":
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#fbcf51");
                        break;
                    }
                case "conv.":
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#01a7d9");
                        break;
                    }
                case "super centers":
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#ff5212");
                        break;
                    }
                case "mass merch":
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#a8a8a5");
                        break;
                    }
                case "drug":
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#bb2c2b");
                        break;
                    }
                case "dollar":
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#658781");
                        break;
                    }
                case "club":
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#72984B");
                        break;
                    }
            }
            return color;
        }
        public string getFormattedChangeVal(double ch)
        {
            if (ch > 0) { return "+" + ch.ToString("##,##0.0"); }
            if (Math.Round(ch, 1) == 0)
            {
                if (ch < 0)
                {
                    return "-" + ch.ToString("##,##0.0");
                }
            }
            return ch.ToString("##,##0.0");
        }

        public void setColorForElement(IAutoShape shp, bool flag)
        {
            shp.FillFormat.FillType = FillType.Solid;
            shp.FillFormat.SolidFillColor.Color = (flag == true ? Color.FromArgb(221, 221, 221) : Color.FromArgb(247, 247, 247));
        }
        public static bool isPPT_Installed()
        {
            Type officeType = Type.GetTypeFromProgID("Powerpoint.Application");

            if (officeType == null)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}







