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
    public class BevaragesTrendShopperReportGenerator : BaseBeverageReport, IBevaragesShopper
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

                InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverageTrend\Shopper.pptx"));

                Benchlist = reportparams.Benchmark.Replace("||", "|").Split('|');
                filt = reportparams.Filters.Split('|');

                ShopperSegment = Convert.ToString(reportparams.ShopperSegmentShortName);
                List<string> list = new List<string>();
                list = Convert.ToString(reportparams.Benchmark).Split('|').ToList();
                if (list != null && list.Count > 0)
                    ComparisonPointsBanner = commonfunctions.Get_ShortNames(list[0]);

                ds = GetChartData();

                if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
                    shopperBenchValue = (from r in ds.Tables[0].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

                if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
                    tripsBenchValue = (from r in ds.Tables[1].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    Benchlist1 = (from r in ds.Tables[0].AsEnumerable() select r.Field<string>("Objective")).FirstOrDefault();
                    Benchlist1 = base.Get_ShortNames(Benchlist1.Trim());

                    sampleSizelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<object>("SampleSize"))).ToList();
                    beveragelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
                }
                List<string> Complist = new List<string>();
                List<string> _samplesize = new List<string>();
                for (int i = 0; i < beveragelist.Count(); i++)
                {
                    samplesizeNames += Get_ShortNames(beveragelist[i]) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber() + " ";
                    _samplesize.Add(Get_ShortNames(beveragelist[i]) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber());

                    if (i >= 1)
                    {
                        Complist.Add(Get_ShortNames(beveragelist[i]));
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

                //profilerparams = new  ProfilerChartParams()
                //{
                //    ChartType = "line"
                //};

                //InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverageTrend\Shopper.pptx"));

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
                //    Benchlist1 = base.Get_ShortNames(Benchlist1.Trim());

                //    sampleSizelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<object>("SampleSize"))).ToList();
                //    beveragelist = (from r in ds.Tables[0].AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
                //}
                //List<string> Complist = new List<string>();
                //List<string> _samplesize = new List<string>();
                //for (int i = 0; i < beveragelist.Count(); i++)
                //{
                //    samplesizeNames += Get_ShortNames(beveragelist[i]) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber() + " ";
                //    _samplesize.Add(Get_ShortNames(beveragelist[i]) + " " + Convert.ToString(sampleSizelist[i]).FormateSampleSizeNumber());

                //    if (i >= 1)
                //    {
                //        Complist.Add(Get_ShortNames(beveragelist[i]));
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
            //complist1 = base.Get_ShortNames(Convert.ToString(complist[1])).Trim();

            //TextChanges 

            texboxvalue = "Beverage: " + Convert.ToString(ShopperSegment) + "\n" +
                          "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames) + "\n" +
                          "Time period: " + Convert.ToString(reportparams.ShortTimePeriod);
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(255, 255, 255), (float)65.19, (float)267.87, (float)865.98, (float)217.70, (float)33.3);
        }

        public void Slide_2(ISlide slide, DataSet Ds)
        {

            //TextChanges 
            texboxvalue = "This report compares " + ShopperSegment + " monthly+ purchasers in " + Benchlist1 + " to " + ShopperSegment + " monthly+ purchasers in " + complistNames + " across the key metrics in the iSHOP survey.\n" +
                               "A monthly+ purchaser is defined as a shopper who purchases the beverage in a typical month.";

            base.Create_TextBox(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)22.32, (float)54, (float)921.6, (float)79.92, 12);

            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of shoppers purchased " + ShopperSegment + " in a typical month";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)190.77, (float)184.32, (float)246.89, (float)28.06, 9);

            texboxvalue = "Read as: In " + Benchlist1 + ", " + tripsBenchValue.ToString("0%") + " of " + ShopperSegment + " trips were made by monthly+ " + ShopperSegment + " purchasers";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)672.66, (float)184.32, (float)246.89, (float)28.06, 9);

            //add %


            texboxvalue = "% of pop that were monthly+ purchasers of " + ShopperSegment;
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(89, 89, 89), (float)64.06, (float)142.86, (float)347.81, (float)36.28, 12, true);

            texboxvalue = "% of " + ShopperSegment + " trips by monthly+ purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(89, 89, 89), (float)526.39, (float)142.86, (float)347.81, (float)36.28, 12, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);


            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                //
                //plot chart 1
                chart_x_position = (float)36;
                chart_y_position = (float)221.10;
                chart_width = (float)438.80;
                chart_height = (float)216.72;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[0], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart_Slide2(slide, ds.Tables[0], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 12, 4);
            }

            if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
            {
                //plot chart 2
                chart_x_position = (float)489.25;
                chart_y_position = (float)221.10;
                chart_width = (float)438.80;
                chart_height = (float)216.72;

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[1], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart_Slide2(slide, ds.Tables[1], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 12, 4);

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[0]);
                fontcolor = Color.White;
                chart_y_position = (float)186.80;
                chart_x_position = (float)584.78;
                //for (int i = 0; i < objectivelist.Count; i++)
                //{
                //    base.Create_LagendForSlide2(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                //    chart_y_position += 50;
                //}
            }


        }
        public void Slide_3(ISlide slide, DataSet Ds)
        {
            System.Data.DataTable tbl = null;
            //Text Changes
            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[2].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            textboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " were male";
            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)536.4, (float)91.44, (float)405.36, (float)13.68, 9);

            //add title

            texboxvalue = "Demographics of Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.2, (float)12.24, (float)925.2, (float)52.56, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " Demographics";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 14, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            chart_y_position = (float)113.76;
            chart_height = (float)113.04;
            chart_x_position = (float)110.88;
            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
            {
                chart_width = (float)817.2;
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                var query = (from row in ds.Tables[2].AsEnumerable()
                             select row).ToList();
                tbl = query.CopyToDataTable();

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(tbl, ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 7, false, true, 9);
            }
            chart_y_position = (float)226.08;
            chart_height = (float)113.04;
            chart_x_position = (float)110.88;
            if (ds != null && ds.Tables != null && ds.Tables[3].Rows.Count > 0)
            {
                chart_width = (float)817.2;
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                var query = (from row in ds.Tables[3].AsEnumerable()
                             select row).ToList();
                tbl = query.CopyToDataTable();

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(tbl, ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 7, false, true, 9);
            }
            chart_y_position = (float)360.72;
            chart_height = (float)113.04;
            chart_x_position = (float)110.88;
            if (ds != null && ds.Tables != null && ds.Tables[4].Rows.Count > 0)
            {
                chart_width = (float)817.2;
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };
                var query = (from row in ds.Tables[4].AsEnumerable()
                             select row).ToList();
                tbl = query.CopyToDataTable();
                //added by Nagaraju D for appending year to age
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        row["MetricItem"] = Convert.ToString(row["MetricItem"]) + " Years";
                    }
                }
                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(tbl, ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 7, false, true, 9);
            }
        }

        public void Slide_4(ISlide slide, DataSet Ds)
        {
            System.Data.DataTable tbl = null;

            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[5].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[5].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            textboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " were white";
            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)536.4, (float)91.44, (float)405.36, (float)13.68, 9);

            //add title

            texboxvalue = "Demographics of Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.2, (float)12.24, (float)925.2, (float)52.56, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " Demographics";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 14, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            chart_y_position = (float)113.76;
            chart_height = (float)184.32;
            chart_x_position = (float)110.88;
            if (ds != null && ds.Tables != null && ds.Tables[5].Rows.Count > 0)
            {
                chart_width = (float)817.2;
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                var query = (from row in ds.Tables[5].AsEnumerable()
                             select row).ToList();
                tbl = query.CopyToDataTable();

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(tbl, ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 8, false, true, 9);
            }

            chart_y_position = (float)298.08;
            chart_height = (float)170.36;
            chart_x_position = (float)110.88;
            if (ds != null && ds.Tables != null && ds.Tables[6].Rows.Count > 0)
            {
                chart_width = (float)817.2;
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };
                var query = (from row in ds.Tables[6].AsEnumerable()
                             select row).ToList();
                tbl = query.CopyToDataTable();

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(tbl, ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 8, false, true, 9);
            }
        }

        public void Slide_5(ISlide slide, DataSet Ds)
        {
            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[7].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[7].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            textboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " were Pleasure Shoppers";
            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)723.6, (float)88.56, (float)215.28, (float)28.08, 9);


            //add title

            texboxvalue = "Segmentation of Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.2, (float)12.24, (float)925.2, (float)52.56, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " Shopper Segment";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 14, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            if (ds != null && ds.Tables != null && ds.Tables[7].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)15.12;
                chart_y_position = (float)82.8;
                chart_width = (float)686.16;
                chart_height = (float)389.76;
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[7], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[7], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, true, 7, 6, true);
            }
        }

        public void Slide_6(ISlide slide, DataSet Ds)
        {
            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[8].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            texboxvalue = "Read as: In " + Benchlist1 + ", " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " were also monthly+ shoppers of Grocery. This does not mean they purchased the product within this channel";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)723.6, (float)88.56, (float)208.08, (float)48.96, 9);

            //add title

            texboxvalue = "Channel Frequency for Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.2, (float)12.24, (float)925.2, (float)52.56, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " who are Monthly+ Shoppers of Channel";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 14, true);

            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)15.12;
                chart_y_position = (float)82.8;
                chart_width = (float)686.16;
                chart_height = (float)389.76;
                profilerparams = new  ProfilerChartParams()
                {
                    ChartType = "line"
                };

                chart_Min_Axis_Value = 0;
                chart_Max_Axis_Value = 0;
                Get_Chart_maxAndMinValue(ds.Tables[8], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

                base.Line_Chart(slide, ds.Tables[8], chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, true, 7, 6, true);

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[0]);
                fontcolor = Color.White;
                chart_y_position = -25;
                chart_x_position = 600;
                //for (int i = 0; i < objectivelist.Count; i++)
                //{
                //    chart_y_position += 90;
                //    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                //}
            }
        }

        public void Slide_7(ISlide slide, DataSet Ds)
        {
            //Text Changes
            System.Data.DataTable tbltop = null;
            var topvalue = (from row in ds.Tables[9].AsEnumerable()
                            where Convert.ToString(row.Field<object>("Objective")).Equals(Benchlist1, StringComparison.OrdinalIgnoreCase)
                            orderby row.Field<object>("Volume") descending
                            select row).Take(1);

            tbltop = topvalue.CopyToDataTable();
            if (tbltop.Rows.Count > 0 && tbltop != null)
            {
                textboxvalue = "Read as: In " + Benchlist1 + ", " + Convert.ToDouble(tbltop.Rows[0]["Volume"]).ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " were also monthly+ shoppers of Walmart Supercenter. This does not mean they purchased the product within this retailer.";
            }

            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)695.62, (float)452.40, (float)243.77, (float)28.2, 9);

            //add title

            texboxvalue = "Retailer Frequency for Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.2, (float)12.24, (float)925.2, (float)52.56, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " who were Monthly+ Shoppers of Retailer";
            base.Create_Trend_TextBox(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(255, 255, 255), (float)38.16, (float)54.72, (float)572.4, (float)24.48, 12, true);
            //end

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //       

            chart_y_position = (float)88.56;
            chart_height = (float)368.64;
            chart_x_position = (float)23.76;
            chart_width = (float)921.6;
            profilerparams = new  ProfilerChartParams()
            {
                ChartType = "line"
            };

            chart_Min_Axis_Value = 0;
            chart_Max_Axis_Value = 0;
            Get_Chart_maxAndMinValue(ds.Tables[9], ref chart_Min_Axis_Value, ref chart_Max_Axis_Value);

            base.Line_Chart(slide, ds.Tables[9], 0, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 7, 8, false, true, 10);

        }

        //Sample size table
        public void Slide_8(ISlide slide, DataSet Ds)
        {
            //add title
            texboxvalue = "Sample Sizes for Monthly+ Purchasers of " + ShopperSegment;
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.2, (float)12.24, (float)925.2, (float)52.56, 24, true);
            //

            TrendSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            Create_SampleSize_Table(slide, Ds.Tables[10]);
        }
    }
}