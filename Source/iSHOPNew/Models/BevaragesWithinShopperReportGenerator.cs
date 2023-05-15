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
    public class BevaragesWithinShopperReportGenerator : BaseBeverageReport, IBevaragesShopper
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

                InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverageWithin\Shopper.pptx"));

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

                //InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\Templates\ReportGenerator\ReportGeneratorPPTFilesBeverageWithin\Shopper.pptx"));

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
                           "Comparison Points/Banner: " + Convert.ToString(ComparisonPointsBanner) + "\n" +
                            Benchlist1 + ", " + complistNames + "\n" +
                          "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames) + "\n" +
                           "Time period: " + Convert.ToString(reportparams.ShortTimePeriod);
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(255, 255, 255), (float)65.19, (float)267.87, (float)865.98, (float)217.70, (float)33.3);
        }

        public void Slide_2(ISlide slide, DataSet Ds)
        {

            //TextChanges 
            texboxvalue = "This report compares " + ShopperSegment + " monthly+ purchasers with " + ComparisonPointsBanner + " " + Benchlist1 + " to " + ComparisonPointsBanner + " " + complistNames + " across the key metrics in the iSHOP survey.\n\n" +
                               "A monthly+ purchaser is defined as a shopper who purchases the beverage in a typical month.";

            base.Create_TextBox(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)22.32, (float)54.0, (float)921.6, (float)79.92, 12);

            texboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of " + ComparisonPointsBanner + "  " + Benchlist1 + " shoppers purchase " + ShopperSegment + " in a typical month";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)173.52, (float)184.32, (float)179.28, (float)28.08, 9);

            texboxvalue = "Read as: " + tripsBenchValue.ToString("0%") + " of " + ShopperSegment + " trips made by " + ComparisonPointsBanner + "  " + Benchlist1 + " are made by monthly+ " + ShopperSegment + " purchasers";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)567.36, (float)184.32, (float)179.28, (float)28.08, 9);

            //add %


            texboxvalue = "% of pop that are monthly+ purchasers of " + ShopperSegment;
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(89, 89, 89), (float)71.716, (float)138.61, (float)216.56, (float)36.28, 12, true);

            texboxvalue = "% of " + ShopperSegment + " trips by monthly+ purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(89, 89, 89), (float)440.22, (float)138.61, (float)223.08, (float)36.28, 12, true);

            //end

            WithinSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);


            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                //
                //plot chart 1
                chart_x_position = (float)36.0;
                chart_y_position = (float)221.04;
                chart_width = (float)353.52;
                chart_height = (float)216.72;

                base.Clustered_Chart_Slide2(slide, ds.Tables[0], chart_x_position, chart_y_position, chart_width, chart_height);
            }

            if (ds != null && ds.Tables != null && ds.Tables[1].Rows.Count > 0)
            {
                //plot chart 2
                chart_x_position = (float)401.04;
                chart_y_position = (float)221.04;
                chart_width = (float)353.52;
                chart_height = (float)216.72;

                base.Clustered_Chart_Slide2(slide, ds.Tables[1], chart_x_position, chart_y_position, chart_width, chart_height);

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[0]);
                fontcolor = Color.White;
                chart_y_position = (float)143.28;
                chart_x_position = (float)774.0;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    base.Create_LagendForSlide2(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                    chart_y_position += (float)49.68;
                }
            }


        }
        public void Slide_3(ISlide slide, DataSet Ds)
        {
            System.Data.DataTable tbl = null;
            //Text Changes
            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[2].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            textboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " in the " + ComparisonPointsBanner + "  " + Benchlist1 + " are male";
            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)391.68, (float)90.72, (float)372.24, (float)17.28, 9);

            //add title

            texboxvalue = "Demographics of Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " Demographics";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)36.85, (float)52.44, (float)572.59, (float)24.37, 14, true);

            //end

            WithinSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            chart_y_position = (float)87.12;
            chart_height = (float)110.16;
            chart_x_position = (float)121.68;
            if (ds != null && ds.Tables != null && ds.Tables[2].Rows.Count > 0)
            {

                float chart_x_Position = (table_width / objectivelist.Count);
                chart_width = (float)253.44;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered bar"
                };

                var query = (from row in ds.Tables[2].AsEnumerable()
                             select row).Reverse();
                tbl = query.CopyToDataTable();

                base.Clustered_Bar_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, 7, 10, false);
                chart_x_position += chart_x_Position;

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[0]);
                fontcolor = Color.White;
                chart_y_position = (float)-12.96;
                chart_x_position = (float)780.48;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    chart_y_position += 90;
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                }
            }
            chart_y_position = (float)188.64;
            chart_height = (float)270.0;
            chart_x_position = (float)100.08;
            if (ds != null && ds.Tables != null && ds.Tables[3].Rows.Count > 0)
            {

                float chart_x_Position = (table_width / objectivelist.Count);
                chart_width = (float)284.4;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered bar"
                };

                var query = (from row in ds.Tables[3].AsEnumerable()
                             select row).Reverse();
                tbl = query.CopyToDataTable();
                //added by Nagaraju D for appending year to age
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        row["MetricItem"] = Convert.ToString(row["MetricItem"]) + " Years";
                    }
                }
                base.Clustered_Bar_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, 7, 10, false,true);
                chart_x_position += chart_x_Position;


            }
            chart_y_position = (float)107.28;
            chart_height = (float)352.8;
            chart_x_position = (float)515.52;
            if (ds != null && ds.Tables != null && ds.Tables[4].Rows.Count > 0)
            {

                float chart_x_Position = (table_width / objectivelist.Count);
                chart_width = (float)196.8;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered bar"
                };
                var query = (from row in ds.Tables[4].AsEnumerable()
                             select row).Reverse();
                tbl = query.CopyToDataTable();
                base.Clustered_Bar_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, 7, 10, false);
                chart_x_position += chart_x_Position;

            }
        }

        public void Slide_4(ISlide slide, DataSet Ds)
        {
            System.Data.DataTable tbl = null;

            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[5].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[5].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            textboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " in the " + ComparisonPointsBanner + "  " + Benchlist1 + " are white";
            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)391.68, (float)90.72, (float)372.24, (float)17.28, 9);


            //add title

            texboxvalue = "Demographics of Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " Demographics";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)36.85, (float)52.44, (float)572.59, (float)24.37, 14, true);

            //end

            WithinSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);

            //

            chart_y_position = (float)107.28;
            chart_height = (float)352.8;
            chart_x_position = (float)121.68;
            if (ds != null && ds.Tables != null && ds.Tables[5].Rows.Count > 0)
            {

                float chart_x_Position = (table_width / objectivelist.Count);
                chart_width = (float)236.88;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered bar"
                };

                var query = (from row in ds.Tables[5].AsEnumerable()
                             select row).Reverse();
                tbl = query.CopyToDataTable();
                base.Clustered_Bar_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, 7, 10, false);
                chart_x_position += chart_x_Position;

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[0]);
                fontcolor = Color.White;
                chart_y_position = (float)-12.96;
                chart_x_position = (float)780.48;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    chart_y_position += 90;
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                }

            }
            chart_y_position = (float)107.28;
            chart_height = (float)352.8;
            chart_x_position = (float)515.52;
            if (ds != null && ds.Tables != null && ds.Tables[6].Rows.Count > 0)
            {

                float chart_x_Position = (table_width / objectivelist.Count);
                chart_width = (float)236.88;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered bar"
                };
                var query = (from row in ds.Tables[6].AsEnumerable()
                             select row).Reverse();
                tbl = query.CopyToDataTable();
                base.Clustered_Bar_Chart(slide, tbl, chart_x_position, chart_y_position, chart_width, chart_height, 7, 10, false);
                chart_x_position += chart_x_Position;

            }
        }

        public void Slide_5(ISlide slide, DataSet Ds)
        {
            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[7].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[7].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            textboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " in the " + ComparisonPointsBanner + "  " + Benchlist1 + " are Pleasure Shoppers";
            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)391.68, (float)90.72, (float)372.24, (float)17.28, 9);


            //add title

            texboxvalue = "Segmentation of Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)648, (float)52.72, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " Shopper Segment";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)36.85, (float)52.44, (float)572.59, (float)24.37, 14, true);

            //end

            WithinSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            if (ds != null && ds.Tables != null && ds.Tables[7].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)15.12;
                chart_y_position = (float)113.04;
                chart_width = (float)747.36;
                chart_height = (float)306.72;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered column"
                };

                base.Clustered_Chart(slide, ds.Tables[7], chart_x_position, chart_y_position, chart_width, chart_height, true, 7, 10, true);

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[0]);
                fontcolor = Color.White;
                chart_y_position = (float)-12.96;
                chart_x_position = (float)780.48;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    chart_y_position += 90;
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                }
            }
        }

        public void Slide_6(ISlide slide, DataSet Ds)
        {
            //Text Changes

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
                shopperBenchValue = (from r in ds.Tables[8].AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();

            texboxvalue = "Read as: " + shopperBenchValue.ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " in the " + ComparisonPointsBanner + "  " + Benchlist1 + " are also monthly+ shoppers of Grocery. This does not mean they purchased the product within this channel";
            base.Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)391.68, (float)90.72, (float)372.24, (float)17.28, 9);

            //add title

            texboxvalue = "Channel Frequency for Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)912.18, (float)52.72, 24, true);

            texboxvalue = "Monthly+ Purchasers of " + ShopperSegment + " who are Monthly+ Shoppers of Channel";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(0, 0, 0), Color.FromArgb(89, 89, 89), (float)36.85, (float)52.44, (float)590.45, (float)24.37, 14, true);

            //end

            WithinSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            if (ds != null && ds.Tables != null && ds.Tables[8].Rows.Count > 0)
            {
                //plot chart 1
                chart_x_position = (float)15.12;
                chart_y_position = (float)113.04;
                chart_width = (float)747.36;
                chart_height = (float)306.72;
                profilerparams = new ProfilerChartParams()
                {
                    ChartType = "clustered column"
                };
                base.Clustered_Chart(slide, ds.Tables[8], chart_x_position, chart_y_position, chart_width, chart_height, true, 7, 10, true);

                //plot textbox legends
                objectivelist = GetLegendList(ds.Tables[0]);
                fontcolor = Color.White;
                chart_y_position = (float)-12.96;
                chart_x_position = (float)780.48;
                for (int i = 0; i < objectivelist.Count; i++)
                {
                    chart_y_position += 90;
                    base.Create_Lagend(slide, objectivelist[i], GetSerirsColour(i), fontcolor, chart_x_position, chart_y_position, chart_legend_width, chart_legend_height);
                }
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
                textboxvalue = "Read as: " + Convert.ToDouble(tbltop.Rows[0]["Volume"]).ToString("0%") + " of monthly+ purchasers of " + ShopperSegment + " in the " + ComparisonPointsBanner + "  " + Benchlist1 + " are also monthly+ shoppers of Walmart Supercenter. This does not mean they purchased the product within this retailer.";
            }

            base.Create_Label(slide, textboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)684.0, (float)453.82, (float)260.64, (float)28.08, 9);

            //add title

            texboxvalue = "Retailer Frequency for Monthly+ " + ShopperSegment + " Purchasers";
            base.Create_Title(slide, texboxvalue, Color.FromArgb(89, 89, 89), Color.FromArgb(228, 30, 43), (float)25.22, (float)0.85, (float)912.18, (float)52.72, 24, true);

            //end

            WithinSourceStatSampleDynamicText(slide, reportparams, texboxvalue, samplesizeNames, Benchlist1);
            //

            List<string> _objectivelist = GetLegendList(ds.Tables[0]);
            Create_Within_Shopper_Table(slide, _objectivelist, "Monthly+");

            chart_y_position = (float)119.62;
            chart_height = (float)339.84;
            chart_x_position = (float)21.25;
            float chart_x_Position = (table_width / _objectivelist.Count);
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


                base.Clustered_Bar_Chart(slide, tbl, i, chart_x_position, chart_y_position, chart_width, chart_height, chart_Max_Axis_Value, chart_Min_Axis_Value, 10, 9, false);
                chart_x_position += chart_x_Position;
            }
        }
    }
}