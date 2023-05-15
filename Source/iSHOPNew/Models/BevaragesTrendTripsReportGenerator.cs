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
    public class BevaragesTrendTripsReportGenerator : BaseBeverageReport, IBevaragesShopper, IBevaragesTrips
    {
        public void PrepareSlides()
        {
            try
            {
                reportparams = HttpContext.Current.Session["GenerateReportParams"] as ReportGeneratorParams;

                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "line"
                };

                InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverageTrend\Trips.pptx"));

                Benchlist = reportparams.Benchmark.Replace("||", "|").Split('|');

                filt = reportparams.Filters.Split('|');

                ShopperSegment = Convert.ToString(reportparams.ShopperSegmentShortName);
                List<string> list = new List<string>();
                list = Convert.ToString(reportparams.Benchmark).Split('|').ToList();
                if (list != null && list.Count > 0)
                    ComparisonPointsBanner = commonfunctions.Get_ShortNames(list[0]);

                ds = GetChartData();

                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    shopperBenchValue = (from r in ds.Tables[0].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

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
                                case 9:
                                    {
                                        Slide_9(sld, ds);
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

                //profilerparams = new  ProfilerChartParams()
                //{
                //    ChartType = "line"
                //};

                //InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverageTrend\Trips.pptx"));

                //Benchlist = reportparams.Benchmark.Replace("||", "|").Split('|');

                //filt = reportparams.Filters.Split('|');

                //ShopperSegment = Convert.ToString(reportparams.ShopperSegmentShortName);
                //List<string> list = new List<string>();
                //list = Convert.ToString(reportparams.Benchmark).Split('|').ToList();
                //if (list != null && list.Count > 0)
                //    ComparisonPointsBanner = commonfunctions.Get_ShortNames(list[0]);

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
                //                case 9:
                //                    {
                //                        Slide_9(sld, ds);
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
            if (Convert.ToString(reportparams.ShopperFrequency).Equals("Total",StringComparison.OrdinalIgnoreCase))
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

            texboxvalue = "Beverage: " + ShopperSegment + "\n" +
                          "Where Purchased: " + ChannelRetailersVisited + "\n" +
                          "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames) + "\n" +
                          "Time period: " + Convert.ToString(reportparams.ShortTimePeriod);

            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(255, 255, 255), (float)65.19, (float)267.87, (float)865.98, (float)217.70, (float)33.3);
        }

        public void Slide_2(ISlide slide, DataSet Ds)
        {
            //TextChanges 
            string texboxvalue = "This report compares trips that included " + ShopperSegment + " in " + Benchlist1 + " to the trips that included " + ShopperSegment + " in " + complistNames + " across the key metrics in the iSHOP survey.";

            base.Create_TextBox(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)22.32, (float)54, (float)921.6, (float)79.92, 11);

            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0.0%") + " of total trips included " + ShopperSegment;
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)525.6, (float)160.56, (float)415.44, (float)28.08, 9);

            texboxvalue = ShopperSegment + " Trips";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(89, 89, 89), (float)25.51, (float)132.94, (float)399.96, (float)39.96, 12, true);

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)37.44;
                chart_y_position = (float)193.68;
                chart_width = (float)889.92;
                chart_height = (float)276.09;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[1], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart_Trips_Slide2(slide, ds.Tables[0], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 12, 8);
                //         
            }
        }

        public void Slide_3(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[1].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            //Text Changes

            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of " + ShopperSegment + " trips included a chilled " + ShopperSegment;

            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)721.44, (float)85.68, (float)215.28, (float)28.8, 9);

            //texboxvalue = "Chilled and room temperature proportions may aggregate to more than 100% due to more than one beverage being purchased on the trip";
            //base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)496.06, (float)92.40, (float)215.43, (float)28.2, 7);

            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[2].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of " + ShopperSegment + " trips included " + ShopperSegment + " that was picked up in " + base.GetBeverageTemperatureMetricShortName(Convert.ToString(ds.Tables[3].Rows[0]["MetricItem"]));
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)721.44, (float)240.48, (float)222.48, (float)28.08, 9);

            //add title

            texboxvalue = "Temperature and Location for " + ShopperSegment + " Trips";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.2, (float)0.85, (float)925.2, (float)52.56, 24, true);

            texboxvalue = ShopperSegment + " Temperature";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 12, true);

            texboxvalue = ShopperSegment + " Location";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)203.04, (float)572.4, (float)24.48, 12, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)20.16;
                chart_y_position = (float)87.12;
                chart_width = (float)690.48;
                chart_height = (float)114.48;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[1], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[1], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 9, 6, true, false, 9);
                //
            }

            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
            {
                //plot chart 2
                chart_x_position = (float)20.16;
                chart_y_position = (float)239.76;
                chart_width = (float)690.48;
                chart_height = (float)114.48;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[2], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[2], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 9, 6, true, false, 9);
                //
            }
        }

        public void Slide_4(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[3].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[3].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            //Text Changes
            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of " + ShopperSegment + " purchases were intended to be consumed fully or in part by the purchaser";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)721.44, (float)85.68, (float)215.28, (float)28.08, 9);

            if (ds != null && ds.Tables != null && ds.Tables[4].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[4].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of " + ShopperSegment + " purchases were consumed fully or in part within 10 minutes of purchase";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)721.44, (float)240.48, (float)222.48, (float)28.08, 9);

            //add title

            texboxvalue = ShopperSegment + " Consumption in " + ShopperSegment + " Trips";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

            texboxvalue = ShopperSegment + " Intended Consumer";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)86.74, (float)572.4, (float)24.48, 12, true);

            texboxvalue = ShopperSegment + " Consumption Timing";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)234.14, (float)572.4, (float)24.48, 12, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            if (ds != null && ds.Tables != null && ds.Tables[3].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)20.16;
                chart_y_position = (float)110.55;
                chart_width = (float)690.48;
                chart_height = (float)114.48;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[3], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[3], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 9, 6, false, false, 9);
                //
            }

            if (ds != null && ds.Tables != null && ds.Tables[4].Rows.Count > 0)
            {
                //plot chart 2
                chart_x_position = (float)20.16;
                chart_y_position = (float)269.29;
                chart_width = (float)690.48;
                chart_height = (float)141.16;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[4], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[4], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 9, 6, false, false, 9);
                //               
            }
        }

        public void Slide_5(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;
            List<string> _objectivelist = new List<string> { };

            if (ds != null && ds.Tables != null && ds.Tables[5].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[5].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();


            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            if (ds != null && ds.Tables != null && ds.Tables[6].Rows.Count > 0)
            {
                Get_Chart_MetricItemVolume(ds.Tables[6], ref chart_Top_MetriItem, ref chart_Top_MetriItemVolume, Benchlist1);

                //Text Changes
                texboxvalue = "Read as: In " + Benchlist1 + ", " + ShopperSegment + " trips contained " + (shopperBenchValue * 100).ToString("0.0") + " items on average and " + chart_Top_MetriItemVolume.ToString("0%") + " of these also included " + chart_Top_MetriItem;
                base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)423.36, (float)470.16, (float)526.32, (float)34.56, 9);

                //add title

                texboxvalue = "Basket Size & Contents of " + ShopperSegment + " Trips";
                base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

                texboxvalue = ShopperSegment + " Trips: Average Basket Size";
                base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 12, true);

                texboxvalue = ShopperSegment + " Trips: Basket Contents";
                base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)203.04, (float)572.4, (float)24.48, 12, true);

                //end              
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                //1st chart
                chart_x_position = (float)20.16;
                chart_y_position = (float)87.12;
                chart_width = (float)926.64;
                chart_height = (float)114.48;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[5], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart_Trips_Slide8(slide, ds.Tables[5], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 9, false, false, 9, "0.0");

                //2nd chart
                chart_x_position = (float)20.16;
                chart_y_position = (float)239.76;
                chart_width = (float)926.64;
                chart_height = (float)230.4;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[6], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[6], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 9, false, true, 9);
            }
        }

        public void Slide_6(ISlide slide, DataSet Ds)
        {
            List<string> _objectivelist = new List<string> { };
            string texboxvalue = string.Empty;
            string categoryName = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[7].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[7].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            if (ds != null && ds.Tables != null && ds.Tables[7].Rows.Count > 0)
                categoryName = (from r in ds.Tables[7].AsEnumerable() select r.Field<string>("MetricItem")).FirstOrDefault();


            //Text Changes
            texboxvalue = "Read as: In " + Benchlist1 + ", " + Convert.ToDouble(shopperBenchValue).ToString("0%") + " of " + ShopperSegment + " trips had " + categoryName + " as the destination item; destination items are at the category level";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)423.36, (float)470.16, (float)526.32, (float)34.56, 9);

            //add title

            texboxvalue = "Destination Item – " + ShopperSegment + " Trips";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

            texboxvalue = ShopperSegment + " Trips: CSD Destination Item";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 12, true);

            texboxvalue = ShopperSegment + " Trips: Top 10 Destination Item";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)203.04, (float)572.4, (float)24.48, 12, true);

            //end
            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
            {
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                //plot 1st chart
                chart_x_position = (float)20.16;
                chart_y_position = (float)87.12;
                chart_width = (float)926.64;
                chart_height = (float)114.48;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[7], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[7], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 9, false, false, 9);

                //plot 2nd chart
                chart_x_position = (float)20.16;
                chart_y_position = (float)239.76;
                chart_width = (float)926.64;
                chart_height = (float)230.4;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[8], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[8], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 9, false, true, 9);
            }
        }

        public void Slide_7(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            if (ds != null && ds.Tables != null && ds.Tables[9].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[9].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            //Text Changes
            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of " + ShopperSegment + " trips were stock up trips";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)728.64, (float)87.84, (float)173.52, (float)28.08, 9);

            //add title

            texboxvalue = "Trip Details: Trip Mission for " + ShopperSegment + " Trips";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

            texboxvalue = ShopperSegment + " Trip Mission";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 12, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            if (ds != null && ds.Tables != null && ds.Tables[9].Rows.Count > 0)
            {
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                //plot chart 1
                chart_x_position = (float)25.92;
                chart_y_position = (float)95.04;
                chart_width = (float)686.88;
                chart_height = (float)376.56;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[9], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[9], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 9, (float)6.5, false, false, 9);
                //               
            }
        }

        public void Slide_8(ISlide slide, DataSet Ds)
        {
            string texboxvalue = string.Empty;

            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[10].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[10].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            objectivelistTripSummary = GetTripSummarylist(ds.Tables[10], benchMarkActualValue);

            texboxvalue = "Read as: In " + Benchlist1 + " On average, " + ShopperSegment + " trip had a basket price of $" + (objectivelistTripSummary.ElementAtOrDefault(1) != null ? (objectivelistTripSummary[1] * 100).ToString("0") : string.Empty) + ", contained " + (objectivelistTripSummary.ElementAtOrDefault(0) != null ? (objectivelistTripSummary[0] * 100).ToString("0.0") : string.Empty) + " items and lasted " + (objectivelistTripSummary.ElementAtOrDefault(2) != null ? (objectivelistTripSummary[2] * 100).ToString("0") : string.Empty) + " minutes";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)701.00, (float)468.56, (float)240.66, (float)28.2, 9);

            //add title

            texboxvalue = "Trip Summary for " + ShopperSegment + " Trips";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //
            System.Data.DataTable tbl = null;
            if (ds != null && ds.Tables != null && ds.Tables[10].Rows.Count > 0)
            {
                List<string> _metriclist = new List<string>();
                _metriclist = (from r in ds.Tables[10].AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

                for (int i = 0; i < _metriclist.Count; i++)
                {
                    var query = (from row in ds.Tables[10].AsEnumerable()
                                 where Convert.ToString(row.Field<object>("MetricItem")).Equals(_metriclist[i], StringComparison.OrdinalIgnoreCase)
                                 select row).ToList();
                    tbl = query.CopyToDataTable();

                    chart_Min_Axis_Value = 0;
                    chart_Max_Axis_Value = 0;
                    Get_Chart_maxAndMinValue(tbl, ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                    if (i == 0)
                    {
                        //plot chart 1
                        chart_x_position = (float)185.04;
                        chart_y_position = (float)128.16;
                        chart_width = (float)743.04;
                        chart_height = (float)113.76;

                        base.Line_Chart_Trips_Slide8(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, (float)10.5, 7, false, false, 9, "0.0");
                    }
                    else if (i == 1)
                    {
                        //plot chart 1
                        chart_x_position = (float)185.04;
                        chart_y_position = (float)241.92;
                        chart_width = (float)743.04;
                        chart_height = (float)113.76;
                        base.Line_Chart_Trips_Slide8(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, (float)10.5, 7, false, false, 9, "0");
                    }
                    else if (i == 2)
                    {
                        //plot chart 1
                        chart_x_position = (float)185.04;
                        chart_y_position = (float)354.96;
                        chart_width = (float)743.04;
                        chart_height = (float)113.76;
                        base.Line_Chart_Trips_Slide8(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 10, 7, false, false, 9, "0");
                    }
                }
            }
        }
        //Sample size table
        public void Slide_9(ISlide slide, DataSet Ds)
        {
            //add title
            texboxvalue = "Sample Sizes for " + ChannelRetailersVisited + " " + ShopperSegment + " Trips";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)51.02, 24, true);
            //
            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            Create_SampleSize_Table(slide, Ds.Tables[11]);
        }
    }
}