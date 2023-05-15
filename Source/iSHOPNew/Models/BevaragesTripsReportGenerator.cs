using Aspose.Slides;
using iSHOP.BLL;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace iSHOPNew.Models
{
    public class BevaragesTripsReportGenerator : BaseBeverageReport, IBevaragesShopper, IBevaragesTrips
    {
        public void PrepareSlides()
        {
            try
            {
                reportparams = HttpContext.Current.Session["GenerateReportParams"] as ReportGeneratorParams;

                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered column"
                };

                InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverage\Trips.pptx"));

                Benchlist = reportparams.Benchmark.Replace("||", "|").Split('|');

                filt = reportparams.Filters.Split('|');

                ds = GetChartData();

                if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
                    shopperBenchValue = (from r in ds.Tables[1].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

                //if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
                //    tripsBenchValue = (from r in ds.Tables[2].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    Benchlist1 = (from r in ds.Tables[0].AsEnumerable() select r.Field<string>("Objective")).FirstOrDefault();
                    if (Benchlist1 != null && Benchlist1 != "")
                    {
                        benchMarkActualValue = Benchlist1.Trim();
                        Benchlist = Benchlist1.Split('(');
                        Benchlist1 = base.Get_ShortNames(Convert.ToString(Benchlist[0]).Trim());
                    }

                    sampleSizelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<object>("SampleSize"))).ToList();
                    beveragelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
                }

                List<string> Complist = new List<string>();
                string[] SplitComplist;
                List<string> _samplesize = new List<string>();
                for (int i = 0; i < beveragelist.Count(); i++)
                {
                    SplitComplist = beveragelist[i].Split('(');
                    samplesizeNames += Get_ShortNames(SplitComplist[0].Trim()) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber() + " ";
                    _samplesize.Add(Get_ShortNames(beveragelist[i]) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber());
                    if (i >= 1)
                    {
                        Complist.Add(Get_ShortNames(SplitComplist[0].Trim()));
                    }
                }
                samplesizeNames = String.Join("; ", _samplesize);
                if (Complist.Count > 1)
                {
                    complistNames = " and " + Complist[Complist.Count - 1];
                    Complist.RemoveAt(Complist.Count - 1);
                    complistNames = String.Join(", ", Complist) + complistNames;
                }
                else
                {
                    complistNames = String.Join(", ", Complist);
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (slds != null && slds.Count > 0)
                    {
                        foreach (ISlide slide in slds)
                        {
                            sld = slide;
                            SlideNumber += 1;
                            switch (SlideNumber)
                            {
                                case 1:
                                    {
                                        Slide_1(sld, ds);
                                        break;
                                    }
                                case 2:
                                    {
                                        Slide_2(sld, ds);
                                        break;
                                    }
                                case 3:
                                    {
                                        Slide_3(sld, ds);
                                        break;
                                    }
                                case 4:
                                    {
                                        Slide_4(sld, ds);
                                        break;
                                    }
                                case 5:
                                    {
                                        Slide_5(sld, ds);
                                        break;
                                    }
                                case 6:
                                    {
                                        Slide_6(sld, ds);
                                        break;
                                    }
                                case 7:
                                    {
                                        Slide_7(sld, ds);
                                        break;
                                    }
                                case 8:
                                    {
                                        Slide_8(sld, ds);
                                        break;
                                    }

                            }
                        }
                    }
                }
                filename = "iSHOP_ReportGenerator_" + GlobalVariables.GetRandomNumber;
                pres.Save(HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);
                HttpContext.Current.Session[SessionVariables.BeveragePPT] = HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx");
            }
            catch (Exception ex)
            {
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
                //UserExportFileName = "~/iSHOPExplorer/ReportGeneratorPPTFiles/UserExportFiles/" + userparam.UserName;

                //if (Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
                //{
                //    Directory.Delete(HttpContext.Current.Server.MapPath(UserExportFileName), true);
                //}
                if (ex.HResult != -2146233040)
                    ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
        public override void GenerateBeverageReport(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            try
            {
                //reportparams = HttpContext.Current.Session["GenerateReportParams"] as ReportGeneratorParams;

                //profilerparams = new ProfilerChartParams()
                //{
                //    ChartType = "clustered column"
                //};

                //InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverage\Trips.pptx"));

                //Benchlist = reportparams.Benchmark.Replace("||", "|").Split('|');

                //filt = reportparams.Filters.Split('|');

                //ds = GetChartData();

                //if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
                //    shopperBenchValue = (from r in ds.Tables[1].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

                //if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
                //    tripsBenchValue = (from r in ds.Tables[2].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

                //if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    Benchlist1 = (from r in ds.Tables[0].AsEnumerable() select r.Field<string>("Objective")).FirstOrDefault();
                //    if (Benchlist1 != null && Benchlist1 != "")
                //    {
                //        benchMarkActualValue = Benchlist1.Trim();
                //        Benchlist = Benchlist1.Split('(');
                //        Benchlist1 = base.Get_ShortNames(Convert.ToString(Benchlist[0]).Trim());
                //    }

                //    sampleSizelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<object>("SampleSize"))).ToList();
                //    beveragelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
                //}

                //List<string> Complist = new List<string>();
                //string[] SplitComplist;
                //List<string> _samplesize = new List<string>();
                //for (int i = 0; i < beveragelist.Count(); i++)
                //{
                //    SplitComplist = beveragelist[i].Split('(');
                //    samplesizeNames += Get_ShortNames(SplitComplist[0].Trim()) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber() + " ";
                //    _samplesize.Add(Get_ShortNames(beveragelist[i]) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber());
                //    if (i >= 1)
                //    {
                //        Complist.Add(Get_ShortNames(SplitComplist[0].Trim()));
                //    }
                //}
                //samplesizeNames = String.Join("; ", _samplesize);
                //if (Complist.Count > 1)
                //{
                //    complistNames = " and " + Complist[Complist.Count - 1];
                //    Complist.RemoveAt(Complist.Count - 1);
                //    complistNames = String.Join(", ", Complist) + complistNames;
                //}
                //else
                //{
                //    complistNames = String.Join(", ", Complist);
                //}

                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    if (slds != null && slds.Count > 0)
                //    {
                //        foreach (ISlide slide in slds)
                //        {
                //            sld = slide;
                //            SlideNumber += 1;
                //            switch (SlideNumber)
                //            {
                //                case 1:
                //                    {
                //                        Slide_1(sld, ds);
                //                        break;
                //                    }
                //                case 2:
                //                    {
                //                        Slide_2(sld, ds);
                //                        break;
                //                    }
                //                case 3:
                //                    {
                //                        Slide_3(sld, ds);
                //                        break;
                //                    }
                //                case 4:
                //                    {
                //                        Slide_4(sld, ds);
                //                        break;
                //                    }
                //                case 5:
                //                    {
                //                        Slide_5(sld, ds);
                //                        break;
                //                    }
                //                case 6:
                //                    {
                //                        Slide_6(sld, ds);
                //                        break;
                //                    }
                //                case 7:
                //                    {
                //                        Slide_7(sld, ds);
                //                        break;
                //                    }
                //                case 8:
                //                    {
                //                        Slide_8(sld, ds);
                //                        break;
                //                    }

                //            }
                //        }
                //    }
                //}
                //Presentation pre = HttpContext.Current.Session[SessionVariables.BeveragePPT] as Presentation;
                filename = "iSHOP_ReportGenerator_" + hdnyear + "" + Convert.ToString(hdnmonth).FormateDateTime() + "" + Convert.ToString(hdndate).FormateDateTime() + "_" + Convert.ToString(hdnhours).FormateDateTime() + "" + Convert.ToString(hdnminutes).FormateDateTime() + Convert.ToString(hdnseconds).FormateDateTime();
                //pre.Save(HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);

                FileStream fs1 = null;
                fs1 = System.IO.File.Open(HttpContext.Current.Session[SessionVariables.BeveragePPT] as string, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs1.Length];
                fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                fs1.Close();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + filename + ".pptx");

                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                HttpContext.Current.Response.AddHeader("Content-Length", btFile.Length.ToString());
                HttpContext.Current.Response.BinaryWrite(btFile);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();

            }
            catch (Exception ex)
            {
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
                //UserExportFileName = "~/iSHOPExplorer/ReportGeneratorPPTFiles/UserExportFiles/" + userparam.UserName;

                //if (Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
                //{
                //    Directory.Delete(HttpContext.Current.Server.MapPath(UserExportFileName), true);
                //}
                if (ex.HResult != -2146233040)
                    ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
        public void Slide_1(ISlide slide, DataSet Ds)
        {

            //TextChanges 
            ChannelRetailersVisited = string.Empty;
            List<string> ShopperFrequencylist = new List<string>();
            if (Convert.ToString(reportparams.ShopperFrequency).Equals("Total", StringComparison.OrdinalIgnoreCase))
            {
                ShopperFrequencylist.Add(reportparams.ShopperFrequency);
            }
            else
            {
                //if (reportparams.ShopperFrequency.IndexOf("channels") > -1 || reportparams.ShopperFrequency.IndexOf("retailers") > -1)
                //{
                string[] cr = reportparams.ShopperFrequency.Split(new String[] { "|", "|" },
                               StringSplitOptions.RemoveEmptyEntries);

                //for (int i = 1; i < cr.Length; i += 2)
                //{
                //    ShopperFrequencylist.Add(Get_ShortNames(cr[i]));
                //}
                for (int i = 0; i < cr.Length; i++)
                {
                    ShopperFrequencylist.Add(Get_ShortNames(cr[i]));
                }
                //}               
            }

            if (ShopperFrequencylist.Count > 1)
            {
                ChannelRetailersVisited = " and " + ShopperFrequencylist[ShopperFrequencylist.Count - 1];
                ShopperFrequencylist.RemoveAt(ShopperFrequencylist.Count - 1);
                ChannelRetailersVisited = String.Join(", ", ShopperFrequencylist) + ChannelRetailersVisited;
            }
            else
            {
                ChannelRetailersVisited = String.Join(", ", ShopperFrequencylist);
            }

            //texboxvalue = "Filter choices:\n" +
            //              "Time period: " + Convert.ToString(reportparams.ShortTimePeriod) + "\n" +
            //              "Benchmark: " + Benchlist1 + "\n" +
            //              "Comparison: " + complistNames + "\n" +
            //              "Where Purchased: " + ChannelRetailersVisited + "\n" +
            //              "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);

            texboxvalue = Benchlist1 + ", " + complistNames + "\n" +
                      "Where Purchased: " + ChannelRetailersVisited + "\n" +
                      "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames) + "\n" +
                        "Time period: " + Convert.ToString(reportparams.ShortTimePeriod);

            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(255, 255, 255), (float)65.19, (float)267.87, (float)865.98, (float)217.70, (float)33.3);
        }

        public void Slide_2(ISlide slide, DataSet Ds)
        {
            //TextChanges 
            string texboxvalue = "This report compares trips that included " + Benchlist1 + " to the trips that included " + complistNames + " across the key metrics in the iSHOP survey.\n\n";

            base.Create_TextBox(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)22.32, (float)54.0, (float)921.6, (float)79.92, 11);

            //texboxvalue = "% of trips that that included the beverage";
            //base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), 20, 280, 400, 40,12);

            texboxvalue = "Read as: " + shopperBenchValue.ToString("0.0%") + "  of total trips included " + Benchlist1;
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)576.72, (float)162.72, (float)329.04, (float)28.08, 9);

            SourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)48.24;
                chart_y_position = (float)178.56;
                chart_width = (float)856.8;
                chart_height = (float)211.68;
                base.Trips_Clustered_Chart_Slide2(slide, ds.Tables[1], chart_x_position, chart_y_position, chart_width, chart_height);
                //

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[1]);
                fontcolor = Color.White;
                chart_y_position = (float)389.52;

                chart_x_position = 0;
                float increamentx = 0;
                if (objectivelist.Count == 1)
                {
                    chart_x_position = (float)420.09;                  
                }
                else if (objectivelist.Count == 2)
                {
                    chart_x_position = (float)259.93;
                    increamentx = (float)307.39;
                }
                else if (objectivelist.Count == 3)
                {
                    chart_x_position = (float)188.50;
                    increamentx = (float)223.95;
                }
                else if (objectivelist.Count == 4)
                {
                    chart_x_position = (float)146.83;
                    increamentx = (float)178.58;
                }
                else if (objectivelist.Count == 5)
                {
                    chart_x_position = (float)119.33;
                    increamentx = (float)146.55;
                }

                for (int i = 0; i < objectivelist.Count; i++)
                {
                    //if (i == 2)
                    //{
                    //    chart_x_position = 450;
                    //    chart_y_position += 50;
                    //}
                    //else if (i == 4)
                    //{
                    //    chart_x_position = 500;
                    //    chart_y_position += 50;
                    //}
                    //else if (i == 0)
                    //{
                    //    chart_x_position = 450;
                    //}
                    //else
                    //{
                    //    chart_x_position += 130;
                    //}
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                    chart_x_position += increamentx;
                }
                //
            }
        }

        public void Slide_3(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[2].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            //Text Changes

            texboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of " + Benchlist1 + " trips include a chilled " + Benchlist1 + ".";

            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)564.94, (float)87.12, (float)377.00, (float)13.68, 9);

            //texboxvalue = "Chilled and room temperature proportions may aggregate to more than 100% due to more than one beverage being purchased on the trip";
            //base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)445.1, (float)92.40, (float)243.9, (float)28.2, 7);

            if (ds != null && ds.Tables != null && ds.Tables[3].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[3].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            texboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of " + Benchlist1 + " trips include an " + Benchlist1 + " that was picked up in " + base.GetBeverageTemperatureMetricShortName(Convert.ToString(ds.Tables[3].Rows[0]["MetricItem"]));
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)564.94, (float)296.64, (float)377.00, (float)28.08, 9);

            SourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)20.88;
                chart_y_position = (float)91.44;
                chart_width = (float)600.94;
                chart_height = 170;
                base.Clustered_Chart(slide, ds.Tables[2], chart_x_position, chart_y_position, chart_width, chart_height, true, 10, 10, true);
                //
            }

            if (ds != null && ds.Tables != null && ds.Tables[3].Rows.Count > 0)
            {
                //plot chart 2
                chart_x_position = (float)20.88;
                chart_y_position = (float)292.32;
                chart_width = (float)940.32;
                chart_height = 180;
                base.Clustered_Chart(slide, ds.Tables[3], chart_x_position, chart_y_position, chart_width, chart_height, false, 7, 8, true);
                //

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[3]);
                fontcolor = Color.White;
                chart_x_position = (float)624.18; 
                chart_y_position = (float)122.4;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    if (i == 2)
                    {
                        chart_x_position = (float)624.18;
                        chart_y_position = (float)181.98;
                    }
                    else if (i == 4)
                    {
                        chart_x_position = (float)706.67;
                        chart_y_position = (float)240.37;
                    }
                    else if (i == 0)
                    {
                        chart_x_position = (float)624.18;
                    }
                    else
                    {
                        chart_x_position += (float)168.94;
                    }
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                }
            }
            //
        }

        public void Slide_4(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[4].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[4].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            //Text Changes
            texboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of " + Benchlist1 + " purchases were intended to be consumed fully or in part by the purchaser";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)391.68, (float)90.72, (float)372.24, (float)17.28, 9);

            if (ds != null && ds.Tables != null && ds.Tables[5].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[5].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            texboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of " + Benchlist1 + " purchases were consumed fully or in part within 10 minutes of purchase";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)391.68, (float)296.64, (float)370.8, (float)28.08, 9);

            SourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            if (ds != null && ds.Tables != null && ds.Tables[4].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)20.16;
                chart_y_position = (float)113.04;
                chart_width = (float)726.48;
                chart_height = (float)146.88;
                base.Clustered_Chart(slide, ds.Tables[4], chart_x_position, chart_y_position, chart_width, chart_height, true, 10, 10, true);
                //
            }

            if (ds != null && ds.Tables != null && ds.Tables[5].Rows.Count > 0)
            {
                //plot chart 2
                chart_x_position = (float)20.16;
                chart_y_position = (float)308.16;
                chart_width = (float)726.48;
                chart_height = (float)129.6;
                base.Clustered_Chart(slide, ds.Tables[5], chart_x_position, chart_y_position, chart_width, chart_height, true, 10, 10, true);
                //

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[5]);
                fontcolor = Color.White;
                chart_x_position = (float)780.48;
                chart_y_position = (float)-12.96;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    chart_y_position += 90;
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                }
                //
            }
        }

        public void Slide_5(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;
            List<string> _objectivelist = new List<string> { };

            if (ds != null && ds.Tables != null && ds.Tables[6].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[6].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();


            SourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //
            if (ds != null && ds.Tables != null && ds.Tables[6].Rows.Count > 0)
            {
                _objectivelist = GetLegendList(ds.Tables[6]);
                Create_Trips_Table(slide, objectivelist, "monthly+", ds.Tables[6]);
            }
            if (ds != null && ds.Tables != null && ds.Tables[7].Rows.Count > 0)
            {
                Get_Chart_MetricItemVolume(ds.Tables[7], ref chart_Top_MetriItem, ref chart_Top_MetriItemVolume, Benchlist1);

                //Text Changes
                texboxvalue = "Read as: " + Benchlist1 + " trips contain " + (shopperBenchValue * 100).ToString("0.0") + " items on average and " + chart_Top_MetriItemVolume.ToString("0%") + " of these also include " + chart_Top_MetriItem;
                base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)714.24, (float)466.56, (float)244.08, (float)28.08, 9);


                chart_y_position = (float)168.09;
                chart_height = (float)297.07;
                chart_x_position = (float)20.12;
                float chart_x_Position = (table_width / objectivelist.Count);
                chart_width = chart_x_Position - 3;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered bar"
                };

                System.Data.DataTable tbl = null;
                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[7], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                for (int i = 0; i < _objectivelist.Count; i++)
                {
                    var query = (from row in ds.Tables[7].AsEnumerable()
                                 where Convert.ToString(row.Field<object>("Objective")).Equals(_objectivelist[i], StringComparison.OrdinalIgnoreCase)
                                 select row).Reverse();
                    tbl = query.CopyToDataTable();



                    base.Clustered_Bar_Chart(slide, tbl, i, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 8, false);
                    chart_x_position += chart_x_Position;
                }
            }
        }

        public void Slide_6(ISlide slide, DataSet Ds)
        {
            List<string> _objectivelist = new List<string> { };
            string texboxvalue = string.Empty;
            string categoryName = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[8].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
                categoryName = (from r in ds.Tables[8].AsEnumerable() select r.Field<string>("MetricItem")).FirstOrDefault();


            //Text Changes
            texboxvalue = "Read as: " + Convert.ToDouble(shopperBenchValue).ToString("0%") + " of " + Benchlist1 + " trips have " + categoryName + " as the destination item; destination items are at the category level";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)672.2, (float)466.56, (float)285.12, (float)28.08, 9);

            SourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
            {
                _objectivelist = GetLegendList(ds.Tables[8]);
                Create_Trips_Table1(slide, _objectivelist, "monthly+", ds.Tables[8]);
            }

            if (ds != null && ds.Tables != null && ds.Tables[9].Rows.Count > 0)
            {
                chart_y_position = (float)190.77;
                chart_height = (float)274.39;
                chart_x_position = (float)20.12;
                float chart_x_Position = (table_width / objectivelist.Count);
                chart_width = chart_x_Position - 3;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered bar"
                };

                System.Data.DataTable tbl = null;
                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[9], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                for (int i = 0; i < _objectivelist.Count; i++)
                {
                    var query = (from row in ds.Tables[9].AsEnumerable()
                                 where Convert.ToString(row.Field<object>("Objective")).Equals(_objectivelist[i], StringComparison.OrdinalIgnoreCase)
                                 select row).Reverse();
                    tbl = query.CopyToDataTable();

                    base.Clustered_Bar_Chart(slide, tbl, i, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 8, false);
                    chart_x_position += chart_x_Position;

                }
            }
        }

        public void Slide_7(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[10].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[10].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            //Text Changes
            texboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of " + Benchlist1 + " trips are stock up trips";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)391.68, (float)90.72, (float)372.24, (float)17.28, 9);

            SourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            if (ds != null && ds.Tables != null && ds.Tables[10].Rows.Count > 0)
            {
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered column"
                };

                //plot chart 1
                chart_x_position = (float)15.12;
                chart_y_position = (float)113.04;
                chart_width = (float)747.36;
                chart_height = (float)306.72;
                base.Clustered_Chart(slide, ds.Tables[10], chart_x_position, chart_y_position, chart_width, chart_height, true, 9, 10, true);
                //

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[10]);
                fontcolor = Color.White;
                chart_x_position = (float)780.48;
                chart_y_position = (float)-12.96;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    chart_y_position += 90;
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                }
                //
            }


        }

        public void Slide_8(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[11].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[11].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            objectivelistTripSummary = GetTripSummarylist(ds.Tables[11], benchMarkActualValue);

            texboxvalue = "Read as: On average, " + Benchlist1 + " trip has a basket price of $" + (objectivelistTripSummary.ElementAtOrDefault(1) != null ? (objectivelistTripSummary[1] * 100).ToString("0") : string.Empty) + ", contains " + (objectivelistTripSummary.ElementAtOrDefault(0) != null ? (objectivelistTripSummary[0] * 100).ToString("0.0") : string.Empty) + " items and lasts " + (objectivelistTripSummary.ElementAtOrDefault(2) != null ? (objectivelistTripSummary[2] * 100).ToString("0") : string.Empty) + " minutes";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)714.24, (float)466.56, (float)244.08, (float)28.08, 9);

            SourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //
            if (ds != null && ds.Tables != null && ds.Tables[11].Rows.Count > 0)
            {
                objectivelist = GetLegendList(ds.Tables[11]);
                Create_Trip_Summary(slide, objectivelist, "monthly+", ds.Tables[11]);
            }
        }
    }
}