using System;
using Aspose.Slides.Charts;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iSHOPNew.DAL;
using Aspose.Slides;
using System.Drawing;
using System.Data;

namespace iSHOPNew.Models
{
    public abstract class BaseBeverageReport
    {
        public ProfilerChartParams profilerparams;
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
        public ISlide sld = null;
        public DataSet ds = new DataSet();
        public List<string> objectivelist = null;
        public List<double> objectivelistTripSummary = null;
        public List<string> metriclist = null;

        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public double StatTesting;

        public float chart_x_position = 0;
        public float chart_y_position = 0;
        public float chart_width = 0;
        public float chart_height = 0;
        public CommonFunctions commonfunctions = new CommonFunctions();
        public ReportGeneratorParams reportparams = null;

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

        public float chart_legend_width
        {
            get
            {
                return (float)130;
            }
        }
        public float chart_legend_height
        {
            get
            {
                return (float)45.36;
            }
        }
        public int SlideNumber = 0;

        public Color backgroundcolor = new Color();
        public Color fontcolor = new Color();
        public string textboxvalue = string.Empty;
       
        private float lagend_fontsize
        {
            get
            {
                return 12;
            }

        }

        public double chart_Min_Axis_Value = 0.0;
        public double chart_Max_Axis_Value = 0.0;

        public string chart_Top_MetriItem = "";
        public double chart_Top_MetriItemVolume = 0.0;

        Dictionary<string, string> bench_Comp_ShortNames = new Dictionary<string, string>();

        public float table_width
        {
            get
            {
                return (float)919.27;
            }
        }
        int seriesnumber = 0;
        IEnumerable<double> dblCols = null;
        IEnumerable<double> dblRows = null;

        FontData fontfamily = new FontData("Arial (Body)");

        public abstract void GenerateBeverageReport(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds);

        public BaseBeverageReport()
        {          
            if (HttpContext.Current.Session["PercentStat"] == null)
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
            else
            {
                StatTesting = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);
                accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
                accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
            }
        }
        public void InitializeAsposePresentationFile(string _fileName)
        {
            pres = new Presentation(_fileName);
            Aspose.Slides.License license = new Aspose.Slides.License();
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            slds = pres.Slides;
        }
        //public void LabelFontSize(IDataLabel lbl)
        //{
        //    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = data_label_fontsize;
        //    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
        //}
        public ChartType GetChartType(string chartType)
        {
            ChartType chart_Type = new ChartType();
            switch (chartType.ToLower())
            {
                case "stacked column":
                    {
                        chart_Type = ChartType.StackedColumn;
                        break;
                    }
                case "stacked bar":
                    {
                        chart_Type = ChartType.StackedBar;
                        break;
                    }
                case "clustered column":
                    {
                        chart_Type = ChartType.ClusteredColumn;
                        break;
                    }
                case "bar with change":
                case "clustered bar":
                    {
                        chart_Type = ChartType.ClusteredBar;
                        break;
                    }
                case "line":
                case "stacked line":
                    {
                        chart_Type = ChartType.Line;
                        break;
                    }
                case "stacked area":
                    {
                        chart_Type = ChartType.StackedArea;
                        break;
                    }
                case "pyramid with change":
                case "pyramid":
                    {
                        chart_Type = ChartType.PercentsStackedBar;
                        break;
                    }
            }
            return chart_Type;
        }
        //Get Series Colour
        public System.Drawing.Color GetSerirsColour(int SeriesIndex)
        {
            System.Drawing.Color color = new System.Drawing.Color();
            switch (SeriesIndex)
            {
                case 0:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#E41E2B");
                        break;
                    }
                case 1:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#31859C");
                        break;
                    }
                case 2:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#FFC000");
                        break;
                    }
                case 3:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#00B050");
                        break;
                    }
                case 4:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#7030A0");
                        break;
                    }
                case 5:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#7F7F7F");
                        break;
                    }
                case 6:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#C00000");
                        break;
                    }
                case 7:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#0070C0");
                        break;
                    }
                case 8:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#FF9900");
                        break;
                    }
                case 9:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#D2D9DF");
                        break;
                    }
                case 10:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#000000");
                        break;
                    }
                case 11:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#838C87");
                        break;
                    }
                case 12:
                    {
                        color = System.Drawing.ColorTranslator.FromHtml("#83E5BB");
                        break;
                    }

                default:
                    color = Color.FromArgb(rnd.Next(0, 112), rnd.Next(0, 150), rnd.Next(0, 100));
                    break;

            }
            return color;
        }

        //public System.Drawing.Color GetSignificanceColor(string significancevalue)
        //{
        //    System.Drawing.Color color = System.Drawing.Color.Black;

        //    if (!string.IsNullOrEmpty(significancevalue))
        //    {
        //        if (Convert.ToDouble(significancevalue) == 1000)
        //            color = System.Drawing.Color.Blue;
        //        else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
        //            color = System.Drawing.Color.Green;
        //        else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
        //            color = System.Drawing.Color.Red;
        //        else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
        //            color = System.Drawing.Color.Black;

        //    }
        //    return color;
        //}

        public System.Drawing.Color GetSignificanceColor(string significancevalue, string objective, string samplesize)
        {
            System.Drawing.Color color = System.Drawing.Color.Black;
            double _sampleSize = 0;
            if (!string.IsNullOrEmpty(samplesize))
            {
                _sampleSize = Convert.ToDouble(samplesize);
            }

            if (Benchlist1 == objective || benchMarkActualValue == objective)
            {
                color = System.Drawing.Color.FromArgb(52, 2, 152);
            }
            else
            {
                if (!string.IsNullOrEmpty(significancevalue))
                {
                    if (Convert.ToDouble(significancevalue) == 1000)
                        color = System.Drawing.Color.Blue;
                    else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                        color = System.Drawing.Color.Green;
                    else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                        color = System.Drawing.Color.Red;
                    else if (_sampleSize >= GlobalVariables.LowSample && _sampleSize < 100)
                    {
                        color = System.Drawing.Color.Gray;
                    }
                    else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                        color = System.Drawing.Color.Black;
                }
            }
            return color;
        }
        public DataSet GetChartData()
        {
            ds = new DataSet();
            if (HttpContext.Current.Session["GenerateReportParams"] != null)
            {
                reportparams = HttpContext.Current.Session["GenerateReportParams"] as ReportGeneratorParams;
                if (reportparams != null)
                {
                    foreach (string key in reportparams.ChartDataSet.Keys)
                    {
                        ds = reportparams.ChartDataSet[key];
                        ds = PrepareSlideTables(ds);
                        break;
                    }
                }
            }
            return FilterDataTable(ds);
        }
        public DataSet PrepareSlideTables(DataSet ds)
        {
            DataSet sDs = new DataSet();
            List<int> slideNumbers = (from row in ds.Tables[0].AsEnumerable()
                                      select Convert.ToInt32(row["SlideNumber"])).Distinct().ToList();
            foreach(int slidenum in slideNumbers)
            {
                List<int> slidetablenum = (from row in ds.Tables[0].AsEnumerable()
                                 where Convert.ToInt32(row["SlideNumber"]) == slidenum
                                 select Convert.ToInt32(row["TableNumber"])).Distinct().ToList();
                foreach (int tablenum in slidetablenum)
                {
                    var sliderows = (from row in ds.Tables[0].AsEnumerable()
                                               where Convert.ToInt32(row["SlideNumber"]) == slidenum
                                               && Convert.ToInt32(row["TableNumber"]) == tablenum
                                     select row).ToList();
                    sDs.Tables.Add(sliderows.CopyToDataTable());
                }
            }
            return sDs;
        }
        public DataSet FilterDataTable(DataSet ds)
        {
            DataSet dsRet = ds;
            if (ds != null && ds.Tables != null)
            {
                bench_Comp_ShortNames = new Dictionary<string, string>();
                List<string> _objective = (from row in dsRet.Tables[1].AsEnumerable() select Convert.ToString(row.Field<object>("Objective"))).Distinct().ToList();

                if (reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase)
                    || reportparams.ModuleBlock.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
                {
                    bench_Comp_ShortNames.Add(_objective[0], _objective[0]);
                    _objective.RemoveAt(0);
                    for (int i = 0; i < _objective.Count; i++)
                    {
                        bench_Comp_ShortNames.Add(_objective[i], _objective[i]);
                    }
                }
                else
                {
                    //bench_Comp_ShortNames.Add(_objective[0], reportparams.BenchmarkShortName);
                    //_objective.RemoveAt(0);
                    for (int i = 0; i < _objective.Count; i++)
                    {
                        bench_Comp_ShortNames.Add(_objective[i], reportparams.ComparisonShortNamelist[i]);
                    }
                }

                string BaseTimePeriod = string.Empty;
                if (reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase)
                  || reportparams.ModuleBlock.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
                {
                    if (reportparams.Benchmark.IndexOf("48MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("48MMT") == -1)
                        BaseTimePeriod = " 48MMT";
                    else if (reportparams.Benchmark.IndexOf("36MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("36MMT") == -1)
                        BaseTimePeriod = " 36MMT";
                    else if (reportparams.Benchmark.IndexOf("30MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("30MMT") == -1)
                        BaseTimePeriod = " 30MMT";
                    else if (reportparams.Benchmark.IndexOf("24MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("24MMT") == -1)
                        BaseTimePeriod = " 24MMT";
                    else if (reportparams.Benchmark.IndexOf("18MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("18MMT") == -1)
                        BaseTimePeriod = " 18MMT";
                    else if (reportparams.Benchmark.IndexOf("3MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("3MMT") == -1)
                        BaseTimePeriod = " 3MMT";
                    else if (reportparams.Benchmark.IndexOf("6MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("6MMT") == -1)
                        BaseTimePeriod = " 6MMT";
                    else if (reportparams.Benchmark.IndexOf("12MMT") > -1 && Convert.ToString(dsRet.Tables[0].Rows[0]["Objective"]).IndexOf("12MMT") == -1)
                        BaseTimePeriod = " 12MMT";

                }


                //System.Data.DataTable inputSampleSizeTable = dsRet.Tables[0];
                //foreach (DataRow row in inputSampleSizeTable.Rows)
                //{
                //    if (reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase)
                //   || reportparams.ModuleBlock.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
                //    {
                //        row["Objective"] = UppercaseFirst((commonfunctions.Get_ShortNames(GetShortName(Convert.ToString(row["Objective"])))).Trim().Replace("&LT;", "<").Replace("&GT;", ">").Replace("&APOS;", "`")) + BaseTimePeriod;
                //    }
                //    else
                //        row["Objective"] = (commonfunctions.Get_ShortNames(GetShortName(Convert.ToString(row["Objective"])))).Trim().Replace("&LT;", "<").Replace("&GT;", ">").Replace("&APOS;", "`");
                //}
                int tbl_count_to = dsRet.Tables.Count;
                int tbl_count_from = 0;

                if (reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase))
                    tbl_count_to = dsRet.Tables.Count - 1;
                if (reportparams.ModuleBlock.Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase))
                    tbl_count_from = 1;

                for (int i = tbl_count_from; i < tbl_count_to; i++)
                {
                    System.Data.DataTable inputTable = dsRet.Tables[i];
                    foreach (DataRow row in inputTable.Rows)
                    {
                        if (System.DBNull.Value != row["Volume"] && Convert.ToDouble(row["Volume"]) > 0)
                            row["Volume"] = Convert.ToDouble(row["Volume"]) / 100;
                        else
                            row["Volume"] = 0;
                        if (reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase)
                          || reportparams.ModuleBlock.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
                        {
                            row["Objective"] = UppercaseFirst((commonfunctions.Get_ShortNames(GetShortName(Convert.ToString(row["Objective"])))).Trim().Replace("&LT;", "<").Replace("&GT;", ">").Replace("&APOS;", "`")) + BaseTimePeriod;
                        }
                        else
                            row["Objective"] = (commonfunctions.Get_ShortNames(GetShortName(Convert.ToString(row["Objective"])))).Trim().Replace("&LT;", "<").Replace("&GT;", ">").Replace("&APOS;", "`");

                        row["Metric"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["Metric"])));
                        row["MetricItem"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["MetricItem"]))).Trim().Replace("&LT;", "<").Replace("&GT;", ">").Replace("&APOS;", "`");
                    }
                }
            }
            return dsRet;
        }
        public string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        private string GetShortName(string name)
        {
            if (name != null && bench_Comp_ShortNames != null && bench_Comp_ShortNames.ContainsKey(name))
                return bench_Comp_ShortNames[name];
            return name;
        }
        //Get Legend List
        public List<string> GetLegendList(System.Data.DataTable tbl)
        {
            objectivelist = new List<string>();
            if (tbl != null && tbl.Rows.Count > 0)
                objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            return objectivelist;
        }
        #region Charts
        //Clustered_Chart only for 2nd Slide
        public void Trips_Clustered_Chart_Slide2(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height)
        {
            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);// + "(" + samplesize.FormateSampleSizeNumber() + ")"
            }

            //Adding new categories
            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
            }

            //Take first chart series

            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string _objective in objectivelist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);
                    Series.ParentSeriesGroup.Overlap = -100;
                    Series.ParentSeriesGroup.GapWidth = 245;

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    //LabelFontSize(lbl);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = 12;
                    catcount++;
                }
                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;


            chart.Axes.HorizontalAxis.IsAutomaticTickLabelSpacing = false;
            chart.Axes.HorizontalAxis.MajorTickMark = TickMarkType.None;
            chart.Axes.HorizontalAxis.MinorTickMark = TickMarkType.None;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = 9;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            //chart.Axes.HorizontalAxis.IsVisible = true;
            //chart.Axes.HorizontalAxis.HasTitle = false;
            chart.Axes.HorizontalAxis.TickLabelPosition = TickLabelPositionType.None;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = true;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = true;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            foreach (IChartSeries series in chart.ChartData.Series)
            {
                float xpos = series.Chart.X;
            }

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }
        //Clustered_Chart only for 2nd Slide
        public void Clustered_Chart_Slide2(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height)
        {
            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);// + "(" + samplesize.FormateSampleSizeNumber() + ")"
            }

            //Adding new categories
            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
            }

            //Take first chart series

            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string _objective in objectivelist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);
                    Series.ParentSeriesGroup.Overlap = -100;
                    Series.ParentSeriesGroup.GapWidth = 474;
                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    //LabelFontSize(lbl);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = 12;
                    catcount++;
                }
                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;


            chart.Axes.HorizontalAxis.IsAutomaticTickLabelSpacing = false;
            chart.Axes.HorizontalAxis.MajorTickMark = TickMarkType.None;
            chart.Axes.HorizontalAxis.MinorTickMark = TickMarkType.None;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = 9;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            //chart.Axes.HorizontalAxis.IsVisible = true;
            //chart.Axes.HorizontalAxis.HasTitle = false;
            chart.Axes.HorizontalAxis.TickLabelPosition = TickLabelPositionType.None;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = true;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = true;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }

        //Trend only for 2nd Slide
        public void Line_Chart_Slide2(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, double _chart_maxValue, double _chart_minValue, float _data_label_fontheight, float _axis_Label_fontheight)
        {
            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < metriclist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, metriclist[i - 1]), chart.Type);
            }

            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i]));
            }

            //Take first chart series
            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string series in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string _objective in objectivelist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    if (query == null)
                    {
                        query = new
                        {
                            value = "0",
                            significance = "0",
                            samplesize="0"
                        };
                    }
                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Size = 5;
                SetMarketStyle(Series, serCount - 1);
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = _chart_maxValue;
            chart.Axes.VerticalAxis.MinValue = _chart_minValue;
            chart.Axes.VerticalAxis.NumberFormat = "0%";

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }

        //Trend only for 2nd Slide
        public void Line_Chart_Trips_Slide2(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, double _chart_maxValue, double _chart_minValue, float _data_label_fontheight, float _axis_Label_fontheight)
        {
            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < metriclist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, metriclist[i - 1]), chart.Type);
            }

            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i]));
            }

            //Take first chart series
            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string series in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string _objective in objectivelist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    if (query == null)
                    {
                        query = new
                        {
                            value = "0",
                            significance = "0",
                            samplesize = "0"
                        };
                    }
                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Size = 5;
                SetMarketStyle(Series, serCount - 1);
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = _chart_maxValue;
            chart.Axes.VerticalAxis.MinValue = _chart_minValue;
            chart.Axes.VerticalAxis.NumberFormat = "0.0%";

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }

        private string GetSampleSize(DataRow r, System.Data.DataTable tbl)
        {
            string samplesize = string.Empty;
            if(r != null && tbl.Columns.Contains("NoOfRespondents"))           
                samplesize = Convert.ToString(r["NoOfRespondents"]);
            else
                samplesize = Convert.ToString(r["SampleSize"]);
            return samplesize;
        }


        //Clustered_Chart
        public void Clustered_Chart(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, bool _chart_CheckOverlapNGapWidth, float _data_label_fontheight, float _axis_Label_fontheight, bool _checkBold)
        {
            objectivelist = (from r in tbl.AsEnumerable() select (Convert.ToString(r.Field<string>("Objective")).First().ToString().ToUpper() + String.Join("", Convert.ToString(r.Field<string>("Objective")).Skip(1)))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (Convert.ToString(r.Field<string>("MetricItem")).First().ToString().ToUpper() + String.Join("", Convert.ToString(r.Field<string>("MetricItem")).Skip(1)))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);// + "(" + samplesize.FormateSampleSizeNumber() + ")"
            }

            //Adding new categories
            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
            }

            //Take first chart series

            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string _objective in objectivelist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);
                    if (_chart_CheckOverlapNGapWidth)
                    {
                        Series.ParentSeriesGroup.Overlap = -27;
                        Series.ParentSeriesGroup.GapWidth = 219;
                    }

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    //LabelFontSize(lbl);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;                    
                    catcount++;
                }
                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorTickMark = TickMarkType.None;
            chart.Axes.HorizontalAxis.MinorTickMark = TickMarkType.None;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;
            if (_checkBold)
                chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontBold = NullableBool.True;
            else
                chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontBold = NullableBool.True;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(89, 89, 89);

            chart.Axes.HorizontalAxis.IsVisible = true;
            //chart.Axes.HorizontalAxis.Title = false;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = true;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = true;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.HorizontalAxis.IsAutomaticMajorUnit = true;
            chart.Axes.HorizontalAxis.IsAutomaticMinorUnit = true;
            chart.Axes.HorizontalAxis.IsAutomaticTickLabelSpacing = false;
            chart.Axes.HorizontalAxis.LabelOffset = 100;
            chart.Axes.HorizontalAxis.TickLabelSpacing = 1;           
            chart.Axes.HorizontalAxis.TickLabelPosition = TickLabelPositionType.NextTo;
            chart.Axes.HorizontalAxis.TickLabelRotationAngle = 0;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }

        //Line_Chart
        public void Line_Chart(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, double _chart_maxValue, double _chart_minValue, bool _chart_CheckOverlapNGapWidth, float _data_label_fontheight, float _axis_Label_fontheight, bool _checkBold)
        {
            objectivelist = (from r in tbl.AsEnumerable() select (Convert.ToString(r.Field<string>("Objective")).First().ToString().ToUpper() + String.Join("", Convert.ToString(r.Field<string>("Objective")).Skip(1)))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (Convert.ToString(r.Field<string>("MetricItem")).First().ToString().ToUpper() + String.Join("", Convert.ToString(r.Field<string>("MetricItem")).Skip(1)))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < metriclist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, metriclist[i - 1]), chart.Type);
            }

            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i]));
            }

            //Take first chart series
            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string series in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string _objective in objectivelist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    if (query == null)
                    {
                        query = new
                        {
                            value = "0",
                            significance = "0",
                            samplesize="0"
                        };
                    }
                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Size = 5;
                SetMarketStyle(Series, serCount - 1);
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = _chart_maxValue;
            chart.Axes.VerticalAxis.MinValue = _chart_minValue;
            chart.Axes.VerticalAxis.NumberFormat = "0%";

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }

        //Clustered_Bar_Chart
        public void Clustered_Bar_Chart(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, float _data_label_fontheight, float _axis_Label_fontheight, bool _checkBold,bool labelOffset = false)
        {
            //var maxValue = (from r in tbl.AsEnumerable() orderby r.Field<string>("Volume") descending select r).Take(1);
            //double maxValue = (from r in tbl.AsEnumerable() select r.Field<double>("Volume")).Max();

            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);// + "(" + samplesize.FormateSampleSizeNumber() + ")"
            }

            //Adding new categories
            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
            }

            //Take first chart series
            IChartSeries Series;

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            int series_color_indx = objectivelist.Count;
            foreach (string _objective in objectivelist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));//maxValueCalculation((!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0), maxValue)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(series_color_indx - 1);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    //LabelFontSize(lbl);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    catcount++;
                }
                catcount = 1;
                serCount++;
                series_color_indx--;
            }
            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;
            chart.Axes.VerticalAxis.MajorTickMark = TickMarkType.None;
            chart.Axes.VerticalAxis.MinorTickMark = TickMarkType.None;

            chart.Axes.VerticalAxis.IsVisible = true;
            chart.Axes.HorizontalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;
            if (_checkBold)
            {
                chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontBold = NullableBool.True;
            }
            else
            {
                chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontBold = NullableBool.False;
            }


            chart.Axes.VerticalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.VerticalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = true;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = true;
            if (labelOffset)
                chart.Axes.VerticalAxis.LabelOffset = 350;

            #region Set Legend
                //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }

        //Line Chart
        public void Line_Chart(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, double _chart_maxValue, double _chart_minValue, float _data_label_fontheight, float _axis_Label_fontheight, bool _checkBold, bool _show_legends, float _legend_font_height)
        {
            //var maxValue = (from r in tbl.AsEnumerable() orderby r.Field<string>("Volume") descending select r).Take(1);
            //double maxValue = (from r in tbl.AsEnumerable() select r.Field<double>("Volume")).Max();

            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < metriclist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, metriclist[i - 1]), chart.Type);
            }

            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i]));
            }

            //Take first chart series
            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string series in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string _objective in objectivelist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    if (query == null)
                    {
                        query = new
                        {
                            value = "0",
                            significance = "0",
                            samplesize="0"
                        };
                    }
                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Size = 5;
                SetMarketStyle(Series, serCount - 1);
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = _chart_maxValue;
            chart.Axes.VerticalAxis.MinValue = _chart_minValue;
            chart.Axes.VerticalAxis.NumberFormat = "0%";

            #region Set Legend
            //Set Legend Style
            if (_show_legends)
            {
                chart.HasLegend = true;
                chart.Legend.Position = LegendPositionType.Bottom;
                chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
                chart.Legend.TextFormat.PortionFormat.FontHeight = _legend_font_height;
            }
            else
                chart.HasLegend = false;
            #endregion
        }

        //Line Chart
        public void Line_Chart_Trips_Slide8(ISlide slide, System.Data.DataTable tbl, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, double _chart_maxValue, double _chart_minValue, float _data_label_fontheight, float _axis_Label_fontheight, bool _checkBold, bool _show_legends, float _legend_font_height, string decimalvalue)
        {
            //var maxValue = (from r in tbl.AsEnumerable() orderby r.Field<string>("Volume") descending select r).Take(1);
            //double maxValue = (from r in tbl.AsEnumerable() select r.Field<double>("Volume")).Max();

            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < metriclist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, metriclist[i - 1]), chart.Type);
            }

            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i]));
            }

            //Take first chart series
            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string series in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string _objective in objectivelist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    if (query == null)
                    {
                        query = new
                        {
                            value = "0",
                            significance = "0",
                            samplesize="0"
                        };
                    }
                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? (Convert.ToDouble(query.value) * 100) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = decimalvalue;
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = decimalvalue;

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Size = 5;
                SetMarketStyle(Series, serCount - 1);
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = _chart_maxValue * 100;
            chart.Axes.VerticalAxis.MinValue = _chart_minValue;
            chart.Axes.VerticalAxis.NumberFormat = decimalvalue;

            #region Set Legend
            //Set Legend Style
            if (_show_legends)
            {
                chart.HasLegend = true;
                chart.Legend.Position = LegendPositionType.Bottom;
                chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
                chart.Legend.TextFormat.PortionFormat.FontHeight = _legend_font_height;
            }
            else
                chart.HasLegend = false;
            #endregion
        }
        //Clustered_Chart for top 10
        public void Clustered_Bar_Chart(ISlide slide, System.Data.DataTable tbl, int SeriesIndexNumber, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, double _chart_maxValue, double _chart_minValue, float _data_label_fontheight, float _axis_Label_fontheight, bool _checkBold)
        {
            //var maxValue = (from r in tbl.AsEnumerable() orderby r.Field<string>("Volume") descending select r).Take(1);
            //double maxValue = (from r in tbl.AsEnumerable() select r.Field<double>("Volume")).Max();

            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);// + "(" + samplesize.FormateSampleSizeNumber() + ")"
            }

            //Adding new categories
            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
            }

            //Take first chart series
            IChartSeries Series;

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string _objective in objectivelist)
            {

                Series = chart.ChartData.Series[serCount - 1];
                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));//maxValueCalculation((!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0), maxValue)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(SeriesIndexNumber);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    if (_checkBold)
                        lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    else
                        lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    catcount++;
                }
                catcount = 1;
                serCount++;
            }
            chart.Axes.HorizontalAxis.IsAutomaticMaxValue = false;
            chart.Axes.HorizontalAxis.IsAutomaticMinValue = false;

            chart.Axes.HorizontalAxis.MaxValue = _chart_maxValue;
            chart.Axes.HorizontalAxis.MinValue = _chart_minValue;
            chart.Axes.HorizontalAxis.NumberFormat = "0%";
            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;
            chart.Axes.VerticalAxis.MajorTickMark = TickMarkType.None;
            chart.Axes.VerticalAxis.MinorTickMark = TickMarkType.None;

            chart.Axes.VerticalAxis.IsVisible = true;
            chart.Axes.HorizontalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.VerticalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = false;
            #endregion
        }

        //Clustered_Chart for top 10
        public void Line_Chart(ISlide slide, System.Data.DataTable tbl, int SeriesIndexNumber, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, double _chart_maxValue, double _chart_minValue, float _data_label_fontheight, float _axis_Label_fontheight, bool _checkBold, bool _show_legends, float _legend_font_height)
        {
            //var maxValue = (from r in tbl.AsEnumerable() orderby r.Field<string>("Volume") descending select r).Take(1);
            //double maxValue = (from r in tbl.AsEnumerable() select r.Field<double>("Volume")).Max();

            objectivelist = (from r in tbl.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select (r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = slide.Shapes.AddChart(GetChartType(profilerparams.ChartType), _chart_x_position, _chart_y_position, _chart_width, _chart_height);
            chart.HasTitle = false;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;

            //Adding new series
            for (int i = 1; i < metriclist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, metriclist[i - 1]), chart.Type);
            }

            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in tbl.AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("SampleSize"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i]));
            }

            //Take first chart series
            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string series in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                foreach (string _objective in objectivelist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                      samplesize = GetSampleSize(r,tbl)
                                 }).FirstOrDefault();
                    if (query == null)
                    {
                        query = new
                        {
                            value = "0",
                            significance = "0",
                            samplesize="0"
                        };
                    }
                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, _objective, query.samplesize);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Size = 5;
                SetMarketStyle(Series, serCount - 1);
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = _axis_Label_fontheight;
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = fontfamily;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = _chart_maxValue;
            chart.Axes.VerticalAxis.MinValue = _chart_minValue;
            chart.Axes.VerticalAxis.NumberFormat = "0%";

            #region Set Legend
            //Set Legend Style
            if (_show_legends)
            {
                chart.HasLegend = true;
                chart.Legend.Position = LegendPositionType.Bottom;
                chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
                chart.Legend.TextFormat.PortionFormat.FontHeight = _legend_font_height;
            }
            else
                chart.HasLegend = false;
            #endregion
        }
        //set marker
        public void SetMarketStyle(IChartSeries Series, int serCount)
        {
            switch (serCount)
            {
                case 0:
                    Series.Marker.Symbol = MarkerStyleType.Square;
                    break;
                case 1:
                    Series.Marker.Symbol = MarkerStyleType.Triangle;
                    break;
                case 2:
                    Series.Marker.Symbol = MarkerStyleType.Diamond;
                    break;
                case 3:
                    Series.Marker.Symbol = MarkerStyleType.Circle;
                    break;
                case 4:
                    Series.Marker.Symbol = MarkerStyleType.Star;
                    break;
                case 5:
                    Series.Marker.Symbol = MarkerStyleType.X;
                    break;
                case 6:
                    Series.Marker.Symbol = MarkerStyleType.Dot;
                    break;
                case 7:
                    Series.Marker.Symbol = MarkerStyleType.Plus;
                    break;
                case 8:
                    Series.Marker.Symbol = MarkerStyleType.Star;
                    break;
                case 9:
                    Series.Marker.Symbol = MarkerStyleType.X;
                    break;
                case 10:
                    Series.Marker.Symbol = MarkerStyleType.Dot;
                    break;
            };

        }
        // Piec_hart_Image
        public void Create_PieImage(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, float _fontheight)
        {
            // Add chart with default data
            IChart chart = slide.Shapes.AddChart(ChartType.Pie, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = false;

            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories

            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            //Adding new categories
            chart.ChartData.Categories.Add(fact.GetCell(0, 1, 0, "1st Qtr"));

            chart.ChartData.Categories.Add(fact.GetCell(0, 2, 0, "2nd Qtr"));
            //chart.ChartData.Categories.Add(fact.GetCell(0, 3, 0, "3rd Qtr"));

            //Adding new series
            IChartSeries series = chart.ChartData.Series.Add(fact.GetCell(0, 0, 1, "Series1"), chart.Type);

            //Now populating series data
            series.DataPoints.AddDataPointForPieSeries(fact.GetCell(defaultWorksheetIndex, 1, 1, Convert.ToDouble(_textboxvalue)));
            series.DataPoints.AddDataPointForPieSeries(fact.GetCell(defaultWorksheetIndex, 2, 1, !string.IsNullOrEmpty(_textboxvalue) ? (1.00 - Convert.ToDouble(_textboxvalue)) : 1.00));
            //series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";


            //series.Labels.DefaultDataLabelFormat.ShowValue = true;         
            series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;
            series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
            foreach (IChartSeries ser in chart.ChartData.Series)
            {
                //Traverse through every data cell in series
                foreach (IChartDataPoint cell in ser.DataPoints)
                {
                    cell.Value.AsCell.CustomNumberFormat = "0%";
                }
            }

            //Not working in new version
            //Adding new points and setting sector color
            //series.IsColorVaried = true;
            chart.ChartData.SeriesGroups[0].IsColorVaried = false;

            IChartDataPoint point = series.DataPoints[0];
            point.Format.Fill.FillType = FillType.Solid;
            point.Format.Fill.SolidFillColor.Color = Color.Red;

            //Setting Sector border
            point.Format.Line.FillFormat.FillType = FillType.Solid;
            point.Format.Line.FillFormat.SolidFillColor.Color = Color.White;
            point.Format.Line.Width = 1.0;
            point.Format.Line.Style = LineStyle.NotDefined;
            point.Format.Line.DashStyle = LineDashStyle.NotDefined;


            IChartDataPoint point1 = series.DataPoints[1];
            point1.Format.Fill.FillType = FillType.Solid;
            point1.Format.Fill.SolidFillColor.Color = Color.FromArgb(127, 127, 127);

            //Setting Sector border
            point1.Format.Line.FillFormat.FillType = FillType.Solid;
            point1.Format.Line.FillFormat.SolidFillColor.Color = Color.White;
            point1.Format.Line.Width = 1.0;
            point1.Format.Line.Style = LineStyle.NotDefined;
            point1.Format.Line.DashStyle = LineDashStyle.NotDefined;


            //Create custom labels for each of categories for new series

            IDataLabel lbl1 = series.DataPoints[0].Label;
            lbl1.DataLabelFormat.ShowValue = false;

            //Showing Leader Lines for Chart
            series.Labels.DefaultDataLabelFormat.ShowLeaderLines = false;

            //Setting Rotation Angle for Pie Chart Sectors
            chart.ChartData.SeriesGroups[0].FirstSliceAngle = 360;
            chart.HasLegend = false;
            chart.HasTitle = false;
        }
        //
        #endregion

        #region Create TextBox
        public void Create_Lagend(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height)
        {
            //Add an AutoShape of Rectangle type
            IAutoShape ashp = slide.Shapes.AddAutoShape(ShapeType.RoundCornerRectangle, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Add TextFrame to the Rectangle
            ashp.AddTextFrame(" ");

            //Accessing the text frame
            ITextFrame txtFrame = ashp.TextFrame;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = _fontcolor;
            ashp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = lagend_fontsize;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = fontfamily;

            ashp.FillFormat.FillType = FillType.Solid;
            ashp.FillFormat.SolidFillColor.Color = _backgroundcolor;

            //Apply some formatting on the line of the rectangle
            ashp.LineFormat.Style = LineStyle.NotDefined;
            ashp.LineFormat.Width = 0;

            //set the color of the line of rectangle
            ashp.LineFormat.FillFormat.FillType = FillType.NoFill;

            //Create the Paragraph object for text frame
            IParagraph para = txtFrame.Paragraphs[0];

            //Create Portion object for paragraph
            IPortion portion = para.Portions[0];

            //Set Text
            string[] LegendsList;
            if (_textboxvalue != "" && _textboxvalue != null)
            {
                LegendsList = _textboxvalue.Split('(');
                _textboxvalue = LegendsList[0].Trim();
            }
            portion.Text = _textboxvalue;
        }
        public void Create_LagendForSlide2(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height)
        {
            //Add an AutoShape of Rectangle type
            IAutoShape ashp = slide.Shapes.AddAutoShape(ShapeType.RoundCornerRectangle, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Add TextFrame to the Rectangle
            ashp.AddTextFrame(" ");

            //Accessing the text frame
            ITextFrame txtFrame = ashp.TextFrame;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = _fontcolor;
            ashp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 11;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = fontfamily;

            ashp.FillFormat.FillType = FillType.Solid;
            ashp.FillFormat.SolidFillColor.Color = _backgroundcolor;

            //Apply some formatting on the line of the rectangle
            ashp.LineFormat.Style = LineStyle.NotDefined;
            ashp.LineFormat.Width = 0;

            //set the color of the line of rectangle
            ashp.LineFormat.FillFormat.FillType = FillType.NoFill;

            //Create the Paragraph object for text frame
            IParagraph para = txtFrame.Paragraphs[0];

            //Create Portion object for paragraph
            IPortion portion = para.Portions[0];

            //Set Text
            string[] LegendsList;
            if (_textboxvalue != "" && _textboxvalue != null)
            {
                LegendsList = _textboxvalue.Split('(');
                _textboxvalue = LegendsList[0].Trim();
            }
            portion.Text = _textboxvalue;
        }
        #endregion
        public void Create_TextBox(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, float _fontheight)
        {
            //Add an AutoShape of Rectangle type
            IAutoShape ashp = slide.Shapes.AddAutoShape(ShapeType.Rectangle, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Add TextFrame to the Rectangle
            ashp.AddTextFrame(" ");

            //Accessing the text frame
            ITextFrame txtFrame = ashp.TextFrame;

            txtFrame.TextFrameFormat.MarginTop = 3.68;
            txtFrame.TextFrameFormat.MarginBottom = 3.68;
            txtFrame.TextFrameFormat.MarginRight = 7.08;
            txtFrame.TextFrameFormat.MarginLeft = 7.08;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = _fontcolor;
            ashp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = _fontheight;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = fontfamily;

            ashp.FillFormat.FillType = FillType.NoFill;
            ashp.FillFormat.SolidFillColor.Color = _backgroundcolor;

            //Apply some formatting on the line of the rectangle
            ashp.LineFormat.Style = LineStyle.Single;
            ashp.LineFormat.Width = 0;

            //set the color of the line of rectangle
            ashp.LineFormat.FillFormat.FillType = FillType.NoFill;

            //Create the Paragraph object for text frame
            IParagraph para = txtFrame.Paragraphs[0];

            //Create Portion object for paragraph
            IPortion portion = para.Portions[0];

            //Set Text
            portion.Text = _textboxvalue;

        }

        public void Create_Trend_TextBox(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, float _fontheight, bool _checkBold)
        {
            //Add an AutoShape of Rectangle type
            IAutoShape ashp = slide.Shapes.AddAutoShape(ShapeType.Rectangle, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Add TextFrame to the Rectangle
            ashp.AddTextFrame(" ");
            slide.Shapes.Reorder(0, ashp);
            //Accessing the text frame
            ITextFrame txtFrame = ashp.TextFrame;

            txtFrame.TextFrameFormat.MarginTop = 3.68;
            txtFrame.TextFrameFormat.MarginBottom = 3.68;
            txtFrame.TextFrameFormat.MarginRight = 7.08;
            txtFrame.TextFrameFormat.MarginLeft = 7.08;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = _fontcolor;
            ashp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = _fontheight;
            if (_checkBold)
                ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            else
                ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = fontfamily;

            ashp.FillFormat.FillType = FillType.Solid;
            ashp.FillFormat.SolidFillColor.Color = _backgroundcolor;

            //Apply some formatting on the line of the rectangle
            ashp.LineFormat.Style = LineStyle.Single;
            ashp.LineFormat.Width = 0;

            //set the color of the line of rectangle
            ashp.LineFormat.FillFormat.FillType = FillType.NoFill;

            //Create the Paragraph object for text frame
            IParagraph para = txtFrame.Paragraphs[0];

            //Create Portion object for paragraph
            IPortion portion = para.Portions[0];

            //Set Text
            portion.Text = _textboxvalue;

        }
        public void Create_PieLabel(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, float _fontheight)
        {
            //Add an AutoShape of Rectangle type
            IAutoShape ashp = slide.Shapes.AddAutoShape(ShapeType.Rectangle, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Add TextFrame to the Rectangle
            ashp.AddTextFrame(" ");

            //Accessing the text frame
            ITextFrame txtFrame = ashp.TextFrame;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = _fontcolor;
            ashp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = _fontheight;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = fontfamily;

            ashp.FillFormat.FillType = FillType.NoFill;

            //Apply some formatting on the line of the rectangle
            ashp.LineFormat.Style = LineStyle.NotDefined;
            ashp.LineFormat.Width = 0;

            //set the color of the line of rectangle
            ashp.LineFormat.FillFormat.FillType = FillType.NoFill;

            //textframe vertical align
            txtFrame.TextFrameFormat.AnchoringType = TextAnchorType.Center;
            //textframe margin
            txtFrame.TextFrameFormat.MarginTop = 0;
            txtFrame.TextFrameFormat.MarginBottom = 0;
            txtFrame.TextFrameFormat.MarginRight = 0;
            txtFrame.TextFrameFormat.MarginLeft = 0;

            //Create the Paragraph object for text frame
            IParagraph para = txtFrame.Paragraphs[0];

            //Create Portion object for paragraph
            IPortion portion = para.Portions[0];

            //Set Text
            if (_textboxvalue == "0%")
            {
                portion.Text = GlobalVariables.NA;
            }
            else
            {
                portion.Text = _textboxvalue;
            }

        }
        public void Create_Label(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, float _fontheight)
        {
            //Add an AutoShape of Rectangle type
            IAutoShape ashp = slide.Shapes.AddAutoShape(ShapeType.Rectangle, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Add TextFrame to the Rectangle
            ashp.AddTextFrame(" ");

            //Accessing the text frame
            ITextFrame txtFrame = ashp.TextFrame;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = _fontcolor;
            ashp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = _fontheight;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = fontfamily;

            ashp.FillFormat.FillType = FillType.NoFill;

            //Apply some formatting on the line of the rectangle
            ashp.LineFormat.Style = LineStyle.NotDefined;
            ashp.LineFormat.Width = 0;

            //set the color of the line of rectangle
            ashp.LineFormat.FillFormat.FillType = FillType.NoFill;

            //textframe vertical align
            txtFrame.TextFrameFormat.AnchoringType = TextAnchorType.Top;
            //textframe margin
            txtFrame.TextFrameFormat.MarginTop = 0;
            txtFrame.TextFrameFormat.MarginBottom = 0;
            txtFrame.TextFrameFormat.MarginRight = 0;
            txtFrame.TextFrameFormat.MarginLeft = 0;

            //Create the Paragraph object for text frame
            IParagraph para = txtFrame.Paragraphs[0];

            //Create Portion object for paragraph
            IPortion portion = para.Portions[0];

            //Set Text
            if (_textboxvalue == "0%")
            {
                portion.Text = GlobalVariables.NA;
            }
            else
            {
                portion.Text = _textboxvalue;
            }

        }

        public void Create_Title(ISlide slide, string _textboxvalue, Color _backgroundcolor, Color _fontcolor, float _chart_x_position, float _chart_y_position, float _chart_width, float _chart_height, float _fontheight, bool _fontweight)
        {
            //Add an AutoShape of Rectangle type
            IAutoShape ashp = slide.Shapes.AddAutoShape(ShapeType.Rectangle, _chart_x_position, _chart_y_position, _chart_width, _chart_height);

            //Add TextFrame to the Rectangle
            ashp.AddTextFrame(" ");
            if (_backgroundcolor == Color.FromArgb(89, 89, 89))
                slide.Shapes.Reorder(slide.Shapes.Count - 1, ashp);
            else
                slide.Shapes.Reorder(slide.Shapes.Count - 2, ashp);
            //Accessing the text frame
            ITextFrame txtFrame = ashp.TextFrame;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = _fontcolor;
            ashp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = _fontheight;
            if (_fontweight)
                ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            else
                ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;

            ashp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = fontfamily;

            ashp.FillFormat.FillType = FillType.NoFill;

            //Apply some formatting on the line of the rectangle
            ashp.LineFormat.Style = LineStyle.NotDefined;
            ashp.LineFormat.Width = 0;

            //set the color of the line of rectangle
            ashp.LineFormat.FillFormat.FillType = FillType.NoFill;

            //textframe vertical align
            txtFrame.TextFrameFormat.AnchoringType = TextAnchorType.Top;
            //textframe margin
            txtFrame.TextFrameFormat.MarginTop = 0;
            txtFrame.TextFrameFormat.MarginBottom = 0;
            txtFrame.TextFrameFormat.MarginRight = 0;
            txtFrame.TextFrameFormat.MarginLeft = 0;

            //Create the Paragraph object for text frame
            IParagraph para = txtFrame.Paragraphs[0];

            //Create Portion object for paragraph
            IPortion portion = para.Portions[0];

            //Set Text
            if (_textboxvalue == "0%")
            {
                portion.Text = GlobalVariables.NA;
            }
            else
            {
                portion.Text = _textboxvalue;
            }

        }
        #region Create Table
        public void Create_Shopper_Table(ISlide slide, List<string> legendvalues, string shoppingFrequency)
        {
            //Define columns with widths and rows with heights
            dblCols = legendvalues.Select(x => Convert.ToDouble(table_width / legendvalues.Count));
            dblRows = new List<double>() { 10, 50, 335 };

            //Add table shape to slide
            ITable tbl = slide.Shapes.AddTable(20, (float)54.42, dblCols.ToArray(), dblRows.ToArray());
            //Set border format for each cell
            foreach (IRow row in tbl.Rows)
            {
                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.Solid;
                    cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderTop.Width = 1;
                    cell.BorderTop.Style = LineStyle.Single;

                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;

                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;
                    cell.BorderRight.Style = LineStyle.Single;

                    cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                    cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderBottom.Width = 1;
                    cell.BorderBottom.Style = LineStyle.Single;
                }
            }
            foreach (ICell cell in tbl.Rows[0])
            {

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1;
                cell.BorderBottom.DashStyle = LineDashStyle.Dot;

            }

            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[1])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = GetSerirsColour(seriesnumber);

                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                cell.BorderTop.DashStyle = LineDashStyle.Dot;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1.875;
                cell.BorderBottom.Style = LineStyle.Single;

                if (legendvalues.Count >= seriesnumber)
                    cell.TextFrame.Text = legendvalues[seriesnumber];
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 12;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
                cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                cell.TextAnchorType = TextAnchorType.Center;
                seriesnumber += 1;
            }

            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[2])
            {
                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                cell.BorderTop.Style = LineStyle.Single;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = Color.Transparent;
                seriesnumber += 1;
            }

            //Merge cells 1 & 2 of row 1
            tbl.MergeCells(tbl[0, 0], tbl[legendvalues.Count - 1, 0], false);
            //tbl.MergeCells(tbl[0, 2], tbl[legendvalues.Count - 1, 2], false);
            tbl[0, 0].FillFormat.FillType = FillType.Solid;
            tbl[0, 0].FillFormat.SolidFillColor.Color = Color.FromArgb(89, 89, 89);
            //Add text to the merged cell

            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 12;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            tbl[0, 0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            tbl[0, 0].TextFrame.Text = shoppingFrequency + " Shoppers of Retailer";
            tbl[0, 0].BorderBottom.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 0].BorderBottom.Width = 1;
            tbl[0, 0].BorderBottom.DashStyle = LineDashStyle.Dot;

            //tbl[0, 2].FillFormat.FillType = FillType.Solid;
            //tbl[0, 2].FillFormat.SolidFillColor.Color = Color.Transparent;
            //Set table border
            SetTableBorder(tbl);
        }

        //Sample Size Table
        public void Create_SampleSize_Table(ISlide slide, System.Data.DataTable _table)
        {
            //Define columns with widths and rows with heights           
            dblCols = new List<double> { 226.08, 226.08 };
            objectivelist = new List<string>();
            objectivelist = (from r in _table.AsEnumerable() select (r.Field<string>("Objective"))).Distinct().ToList();
            List<double> _dblRows = new List<double>();
            _dblRows.Add(25);
            double samplesizerowheight = 13;
            _dblRows.AddRange((from obj in objectivelist select samplesizerowheight).ToList());

            //Add table shape to slide
            ITable tbl = slide.Shapes.AddTable((float)230.4, (float)54.72, dblCols.ToArray(), _dblRows.ToArray());

            int cellnumber = 0;
            int rownumber = 0;
            foreach (IRow row in tbl.Rows)
            {
                cellnumber = 0;
                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.Solid;
                    cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderTop.Width = 1;
                    cell.BorderTop.Style = LineStyle.Single;

                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;

                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;
                    cell.BorderRight.Style = LineStyle.Single;

                    cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                    cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderBottom.Width = 1;
                    cell.BorderBottom.Style = LineStyle.Single;

                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 12;

                    if (rownumber > 0)
                    {
                        cell.MarginTop = 0.85;
                        cell.MarginBottom = 0;
                        cell.MarginLeft = 7.08;
                        cell.MarginRight = 0.85;
                    }

                    if (cellnumber == 0)
                    {
                        cell.FillFormat.FillType = FillType.Solid;
                        cell.FillFormat.SolidFillColor.Color = Color.FromArgb(192, 192, 192);
                    }
                    else
                    {
                        cell.FillFormat.FillType = FillType.NoFill;
                    }
                    cellnumber++;
                }
                rownumber++;
            }

            foreach (ICell cell in tbl.Rows[0])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = Color.FromArgb(89, 89, 89);
            }

            //add header
            tbl[0, 0].TextFrame.Text = "Time Period";
            tbl[1, 0].TextFrame.Text = "Sample Size";

            //first header column
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 14;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            tbl[0, 0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            //second header column
            tbl[1, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 14;
            tbl[1, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tbl[1, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
            tbl[1, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            tbl[1, 0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            rownumber = 1;
            string samplesize = string.Empty;
            foreach (string objective in objectivelist)
            {
                samplesize = (from r in _table.AsEnumerable()
                              where Convert.ToString(r.Field<object>("Objective")).Equals(objective, StringComparison.OrdinalIgnoreCase)
                              select Convert.ToString(r.Field<object>("SampleSize"))).FirstOrDefault();

                tbl[0, rownumber].TextFrame.Text = Convert.ToString(objective).ToUpper();
                tbl[1, rownumber].TextFrame.Text = samplesize.FormateSampleSizeNumber();

                tbl[0, rownumber].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                tbl[1, rownumber].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                rownumber++;
            }
        }

        public void Create_Within_Shopper_Table(ISlide slide, List<string> legendvalues, string shoppingFrequency)
        {
            //Define columns with widths and rows with heights
            dblCols = legendvalues.Select(x => Convert.ToDouble(table_width / legendvalues.Count));
            dblRows = new List<double>() { 10, 50, 325 };

            //Add table shape to slide
            ITable tbl = slide.Shapes.AddTable(20, (float)54.42, dblCols.ToArray(), dblRows.ToArray());
            slide.Shapes.Reorder(0, tbl);
            //Set border format for each cell
            foreach (IRow row in tbl.Rows)
            {
                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.Solid;
                    cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderTop.Width = 1;
                    cell.BorderTop.Style = LineStyle.Single;

                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;

                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;
                    cell.BorderRight.Style = LineStyle.Single;

                    cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                    cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderBottom.Width = 1;
                    cell.BorderBottom.Style = LineStyle.Single;
                }
            }
            foreach (ICell cell in tbl.Rows[0])
            {
                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1;
                cell.BorderBottom.DashStyle = LineDashStyle.Dot;
            }

            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[1])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = GetSerirsColour(seriesnumber);

                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                cell.BorderTop.DashStyle = LineDashStyle.Dot;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1.875;
                cell.BorderBottom.Style = LineStyle.Single;

                if (legendvalues.Count >= seriesnumber)
                    cell.TextFrame.Text = legendvalues[seriesnumber];
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 12;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
                cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                cell.TextAnchorType = TextAnchorType.Center;
                seriesnumber += 1;
            }

            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[2])
            {
                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                cell.BorderTop.Style = LineStyle.Single;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = Color.Transparent;
                seriesnumber += 1;
            }

            //Merge cells 1 & 2 of row 1
            tbl.MergeCells(tbl[0, 0], tbl[legendvalues.Count - 1, 0], false);
            //tbl.MergeCells(tbl[0, 2], tbl[legendvalues.Count - 1, 2], false);
            tbl[0, 0].FillFormat.FillType = FillType.Solid;
            tbl[0, 0].FillFormat.SolidFillColor.Color = Color.FromArgb(89, 89, 89);
            //Add text to the merged cell

            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 12;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
            tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            tbl[0, 0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            tbl[0, 0].TextFrame.Text = shoppingFrequency + " Purchasers of " + ShopperSegment + " who are Monthly+ Shoppers of Retailer";
            tbl[0, 0].BorderBottom.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 0].BorderBottom.Width = 1;
            tbl[0, 0].BorderBottom.DashStyle = LineDashStyle.Dot;

            //tbl[0, 2].FillFormat.FillType = FillType.Solid;
            //tbl[0, 2].FillFormat.SolidFillColor.Color = Color.Transparent;
            //Set table border
            SetTableBorder(tbl);
        }
        #endregion

        #region Create Trips Table
        public void Create_Trips_Table(ISlide slide, List<string> legendvalues, string shoppingFrequency, System.Data.DataTable tble)
        {
            string imagepath = HttpContext.Current.Server.MapPath("~/Images/cart.png");
            //Define columns with widths and rows with heights
            dblCols = legendvalues.Select(x => Convert.ToDouble(table_width / legendvalues.Count));
            dblRows = new List<double>() { 65, 25, 30, 284.78 };

            //dynamic Fontsize
            int dynamicefontSize = dynamicFontSize(legendvalues);
            //

            //Add table shape to slide
            ITable tbl = slide.Shapes.AddTable((float)20.12, (float)48.75, dblCols.ToArray(), dblRows.ToArray());
            //Set border format for each cell
            foreach (IRow row in tbl.Rows)
            {
                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.Solid;
                    cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderTop.Width = 1;
                    cell.BorderTop.Style = LineStyle.Single;

                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;

                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;
                    cell.BorderRight.Style = LineStyle.Single;

                    cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                    cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderBottom.Width = 1;
                    cell.BorderBottom.Style = LineStyle.Single;
                }
            }
            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[0])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = GetSerirsColour(seriesnumber);

                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1;
                cell.BorderBottom.DashStyle = LineDashStyle.Dot;

                string[] legendSplitValues;

                if (legendvalues.Count >= seriesnumber)
                {
                    legendSplitValues = legendvalues[seriesnumber].Split('(');
                    cell.TextFrame.Text = legendSplitValues[0].Trim();
                }

                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = dynamicefontSize;
                //}
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
                cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                cell.TextAnchorType = TextAnchorType.Center;
                seriesnumber += 1;
            }

            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[1])
            {

                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.White;
                cell.BorderTop.Width = 1;
                cell.BorderLeft.Style = LineStyle.Single;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                cell.BorderRight.Style = LineStyle.Single;

                cell.BorderLeft.FillFormat.FillType = FillType.NoFill;
                cell.BorderLeft.Width = 0;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1;
                //cell.BorderBottom.Style = LineStyle.Single;
                cell.BorderBottom.DashStyle = LineDashStyle.Dot;

                if (seriesnumber == 0)
                {
                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;
                }
                else if ((legendvalues.Count - 1) == seriesnumber)
                {
                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;
                    cell.BorderRight.Style = LineStyle.Single;

                    cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                    cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderBottom.Width = 1;
                    //cell.BorderBottom.Style = LineStyle.Single;
                    cell.BorderBottom.DashStyle = LineDashStyle.Dot;
                }
                seriesnumber += 1;
            }

            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[2])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = GetSerirsColour(seriesnumber);

                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                cell.BorderTop.DashStyle = LineDashStyle.Dot;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1.875;
                cell.BorderBottom.Style = LineStyle.Single;

                if (legendvalues.Count >= seriesnumber)
                    cell.TextFrame.Text = (Convert.ToDouble(tble.Rows[seriesnumber]["Volume"]) * 100).ToString("0.0");


                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 14;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
                cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                cell.TextAnchorType = TextAnchorType.Center;
                seriesnumber += 1;
            }
            float image_x_position = 0;
            if (objectivelist.Count == 2)
                image_x_position = (float)213.44;
            else if (objectivelist.Count == 3)
                image_x_position = (float)134.07;
            else if (objectivelist.Count == 4)
                image_x_position = (float)98.92;
            else if (objectivelist.Count == 5)
                image_x_position = (float)75.40;

            float image_y_position = (float)146.55;
            float chart_x_Position = (table_width / objectivelist.Count);
            for (int i = 0; i < objectivelist.Count; i++)
            {
                Create_Image(slide, imagepath, (image_x_position), image_y_position, 19, 17);
                image_x_position += chart_x_Position;
            }
            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[3])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = Color.Transparent;


                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                cell.BorderTop.DashStyle = LineDashStyle.Dot;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.NoFill;
                cell.BorderLeft.Width = 0;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1;
                cell.BorderBottom.Style = LineStyle.Single;
                if (seriesnumber == 0)
                {
                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;
                }
                else if ((legendvalues.Count - 1) == seriesnumber)
                {
                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;
                    cell.BorderRight.Style = LineStyle.Single;
                }
                seriesnumber += 1;
            }
            //Merge cells 1 & 2 of row 1
            tbl.MergeCells(tbl[0, 1], tbl[legendvalues.Count - 1, 1], false);
            //tbl.MergeCells(tbl[0, 3], tbl[legendvalues.Count - 1, 3], false);
            tbl[0, 1].FillFormat.FillType = FillType.Solid;
            tbl[0, 1].FillFormat.SolidFillColor.Color = Color.FromArgb(127, 127, 127);
            //Add text to the merged cell

            tbl[0, 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 11;
            tbl[0, 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tbl[0, 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
            tbl[0, 1].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
            tbl[0, 1].TextFrame.Paragraphs[0].ParagraphFormat.FontAlignment = FontAlignment.Center;                

            tbl[0, 1].TextFrame.Text = "Average Basket Size (Items)";

            tbl[0, 0].BorderTop.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 0].BorderTop.Width = 1;
            tbl[0, 0].BorderTop.Style = LineStyle.Single;

            tbl[0, 1].BorderBottom.FillFormat.FillType = FillType.Solid;
            tbl[0, 1].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 1].BorderBottom.Width = 1;
            //tbl[0, 1].BorderBottom.Style = LineStyle.Single;
            tbl[0, 1].BorderBottom.DashStyle = LineDashStyle.Dot;

            tbl[0, 1].BorderRight.FillFormat.FillType = FillType.Solid;
            tbl[0, 1].BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 1].BorderRight.Width = 1;
            tbl[0, 1].BorderRight.Style = LineStyle.Single;

            tbl[0, 2].BorderTop.FillFormat.FillType = FillType.Solid;
            tbl[0, 2].BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 2].BorderTop.Width = 1;
            //tbl[0, 2].BorderTop.Style = LineStyle.Single;
            tbl[0, 2].BorderTop.DashStyle = LineDashStyle.Dot;

            tbl[0, 2].BorderBottom.FillFormat.FillType = FillType.Solid;
            tbl[0, 2].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 2].BorderBottom.Width = 1.875;
            tbl[0, 2].BorderBottom.Style = LineStyle.Single;

            //tbl[0, 3].FillFormat.FillType = FillType.Solid;
            //tbl[0, 3].FillFormat.SolidFillColor.Color = Color.Transparent;
            //set table border
            SetTableBorder(tbl);
        }
        #endregion
        private void SetTableBorder(ITable tbl)
        {
            int columns = 0;
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                columns = 0;
                foreach (ICell cell in tbl.Rows[i])
                {
                    if (i == 0)
                    {
                        cell.BorderTop.FillFormat.FillType = FillType.Solid;
                        cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                        cell.BorderTop.Width = 1.875;
                        cell.BorderTop.Style = LineStyle.Single;
                        if (columns == 0)
                        {
                            cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                            cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                            cell.BorderLeft.Width = 1.875;
                            cell.BorderLeft.Style = LineStyle.Single;
                        }
                        if (columns == tbl.Columns.Count - 1 || cell.ColSpan == tbl.Columns.Count)
                        {
                            cell.BorderRight.FillFormat.FillType = FillType.Solid;
                            cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                            cell.BorderRight.Width = 1.875;
                            cell.BorderRight.Style = LineStyle.Single;
                        }

                    }
                    else if (i == tbl.Rows.Count - 1)
                    {
                        cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                        cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                        cell.BorderBottom.Width = 1.875;
                        cell.BorderBottom.Style = LineStyle.Single;
                        if (columns == 0)
                        {
                            cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                            cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                            cell.BorderLeft.Width = 1.875;
                            cell.BorderLeft.Style = LineStyle.Single;
                        }
                        if (columns == tbl.Columns.Count - 1 || cell.ColSpan == tbl.Columns.Count)
                        {
                            cell.BorderRight.FillFormat.FillType = FillType.Solid;
                            cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                            cell.BorderRight.Width = 1.875;
                            cell.BorderRight.Style = LineStyle.Single;
                        }

                    }
                    else
                    {
                        if (columns == 0)
                        {
                            cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                            cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                            cell.BorderLeft.Width = 1.875;
                            cell.BorderLeft.Style = LineStyle.Single;
                        }
                        if (columns == tbl.Columns.Count - 1 || cell.ColSpan == tbl.Columns.Count)
                        {
                            cell.BorderRight.FillFormat.FillType = FillType.Solid;
                            cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                            cell.BorderRight.Width = 1.875;
                            cell.BorderRight.Style = LineStyle.Single;
                        }
                    }
                    columns++;
                }
            }
        }
        #region Create Trips Table1
        public void Create_Trips_Table1(ISlide slide, List<string> legendvalues, string shoppingFrequency, System.Data.DataTable tble)
        {
            //string imagepath = HttpContext.Current.Server.MapPath("~/iSHOPExplorer/Pics/cart.png");

            //Define columns with widths and rows with heights
            dblCols = legendvalues.Select(x => Convert.ToDouble(table_width / legendvalues.Count));
            double height = 260;
            dblRows = new List<double>() { 60, 20, 25, 30, height };

            //dynamic Fontsize
            int dynamicefontSize = dynamicFontSize(legendvalues);
            int dynamicefontSizeMetricItem = dynamicFontSize((from r in tble.AsEnumerable() select r.Field<string>("MetricItem")).ToList());
            //

            //Add table shape to slide
            ITable tbl = slide.Shapes.AddTable((float)20.12, (float)48.75, dblCols.ToArray(), dblRows.ToArray());
            slide.Shapes.Reorder(0, tbl);
            //Set border format for each cell
            foreach (IRow row in tbl.Rows)
            {
                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.Solid;
                    cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderTop.Width = 1;
                    cell.BorderTop.Style = LineStyle.Single;

                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;

                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;
                    cell.BorderRight.Style = LineStyle.Single;

                    cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                    cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderBottom.Width = 1;
                    cell.BorderBottom.Style = LineStyle.Single;
                }
            }
            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[0])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = GetSerirsColour(seriesnumber);

                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1;
                cell.BorderBottom.DashStyle = LineDashStyle.Dot;

                string[] legendSplitValues;

                if (legendvalues.Count >= seriesnumber)
                {
                    legendSplitValues = legendvalues[seriesnumber].Split('(');
                    cell.TextFrame.Text = legendSplitValues[0].Trim();
                }

                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = dynamicefontSize;
                //}
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
                cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                cell.TextAnchorType = TextAnchorType.Center;
                seriesnumber += 1;
            }
            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[3])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = GetSerirsColour(seriesnumber);

                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                //cell.BorderTop.Style = LineStyle.Single;
                cell.BorderTop.DashStyle = LineDashStyle.Dot;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1.875;
                cell.BorderBottom.Style = LineStyle.Single;


                if (legendvalues.Count >= seriesnumber)
                    cell.TextFrame.Text = tble.Rows[seriesnumber]["MetricItem"].ToString();

                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = dynamicefontSizeMetricItem;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
                cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                cell.TextAnchorType = TextAnchorType.Center;
                seriesnumber += 1;
            }

            float image_x_position = 0;
            if (objectivelist.Count == 2)
                image_x_position = (float)213.44;
            else if (objectivelist.Count == 3)
                image_x_position = (float)145.41;
            else if (objectivelist.Count == 4)
                image_x_position = (float)100.06;
            else if (objectivelist.Count == 5)
                image_x_position = (float)83.05;

            float image_y_position = (float)130.11;
            float chart_x_Position = (table_width / objectivelist.Count);
            for (int i = 0; i < objectivelist.Count; i++)
            {
                Create_PieImage(slide, Convert.ToDouble(tble.Rows[i]["Volume"]).ToString(), Color.FromArgb(89, 89, 89), Color.FromArgb(89, 89, 89), image_x_position, image_y_position, 35, 35, 10);
                Create_PieLabel(slide, Convert.ToDouble(tble.Rows[i]["Volume"]).ToString("0%"), Color.FromArgb(242, 242, 242), Color.White, (image_x_position + 30), image_y_position + 1, 70, 32, 16);
                image_x_position += chart_x_Position;

            }
            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[4])
            {
                cell.FillFormat.FillType = FillType.Solid;
                cell.FillFormat.SolidFillColor.Color = Color.Transparent;


                cell.BorderTop.FillFormat.FillType = FillType.Solid;
                cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderTop.Width = 1;
                cell.BorderTop.Style = LineStyle.Single;

                cell.BorderRight.FillFormat.FillType = FillType.Solid;
                cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderRight.Width = 1;
                //cell.BorderRight.Style = LineStyle.Single;
                if ((legendvalues.Count - 1) == seriesnumber)
                    cell.BorderRight.Style = LineStyle.Single;
                else
                    cell.BorderRight.DashStyle = LineDashStyle.Dot;

                cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderLeft.Width = 1;
                if ((seriesnumber == 0))
                    cell.BorderLeft.Style = LineStyle.Single;
                else
                    cell.BorderLeft.DashStyle = LineDashStyle.Dot;

                cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                cell.BorderBottom.Width = 1;
                cell.BorderBottom.Style = LineStyle.Single;

                seriesnumber += 1;
            }

            //Merge cells 1 & 2 of row 1
            tbl.MergeCells(tbl[0, 1], tbl[legendvalues.Count - 1, 1], false);
            tbl.MergeCells(tbl[0, 2], tbl[legendvalues.Count - 1, 2], false);
            //tbl.MergeCells(tbl[0, 4], tbl[legendvalues.Count - 1, 4], false);
            tbl[0, 1].FillFormat.FillType = FillType.Solid;
            tbl[0, 1].FillFormat.SolidFillColor.Color = Color.FromArgb(127, 127, 127);

            tbl[0, 2].FillFormat.FillType = FillType.Solid;
            tbl[0, 2].FillFormat.SolidFillColor.Color = Color.FromArgb(127, 127, 127);
            //Add text to the merged cell

            tbl[0, 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 12;
            tbl[0, 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tbl[0, 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
            tbl[0, 1].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

            tbl[0, 1].TextFrame.Text = "Destination Item Categories";

            tbl[0, 0].BorderTop.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 0].BorderTop.Width = 1;
            tbl[0, 0].BorderTop.Style = LineStyle.Single;

            tbl[0, 1].BorderBottom.FillFormat.FillType = FillType.Solid;
            tbl[0, 1].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 1].BorderBottom.Width = 1;
            tbl[0, 1].BorderBottom.DashStyle = LineDashStyle.Dot;

            tbl[0, 2].BorderBottom.FillFormat.FillType = FillType.Solid;
            tbl[0, 2].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 2].BorderBottom.Width = 1;
            tbl[0, 2].BorderBottom.DashStyle = LineDashStyle.Dot;

            tbl[0, 3].BorderTop.FillFormat.FillType = FillType.Solid;
            tbl[0, 3].BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 3].BorderTop.Width = 1;
            tbl[0, 3].BorderTop.DashStyle = LineDashStyle.Dot;

            tbl[0, 3].BorderBottom.FillFormat.FillType = FillType.Solid;
            tbl[0, 3].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
            tbl[0, 3].BorderBottom.Width = 1.875;
            tbl[0, 3].BorderBottom.Style = LineStyle.Single;

            //tbl[0, 4].FillFormat.FillType = FillType.Solid;31.74
            //tbl[0, 4].FillFormat.SolidFillColor.Color = Color.Transparent;
            //Set table border
            SetTableBorder(tbl);
        }
        #endregion

        #region Create Trip Summary Table
        public void Create_Trip_Summary(ISlide slide, List<string> legendvalues, string shoppingFrequency, System.Data.DataTable tble)
        {
            //Define columns with widths and rows with heights

            legendvalues.Insert(0, "");
            dblCols = legendvalues.Select(x => Convert.ToDouble(table_width / (legendvalues.Count - 1)));
            dblRows = new List<double>() { 90, (float)104.74, (float)104.74, (float)104.74 };

            //dynamic Fontsize
            int dynamicefontSize = 16;
            dynamicefontSize = dynamicFontSize(legendvalues);
            //
            legendvalues.Insert(0, "");
            //Add table shape to slide
            ITable tbl = slide.Shapes.AddTable((float)20.12, (float)48.75, dblCols.ToArray(), dblRows.ToArray());
            tbl.Columns[0].Width = 170;
            float table_widthmodified = table_width - Convert.ToSingle(tbl.Columns[0].Width);
            double width = Convert.ToDouble(table_widthmodified / (legendvalues.Count - 1));
            for (int i = 1; i < legendvalues.Count; i++)
            {

                tbl.Columns[i].Width = width;
            }

            slide.Shapes.Reorder(0, tbl);
            //Set border format for each cell
            foreach (IRow row in tbl.Rows)
            {
                seriesnumber = 0;
                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.Solid;
                    cell.BorderTop.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderTop.Width = 1;
                    cell.BorderTop.Style = LineStyle.Single;

                    cell.BorderLeft.FillFormat.FillType = FillType.Solid;
                    cell.BorderLeft.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderLeft.Width = 1;
                    cell.BorderLeft.Style = LineStyle.Single;

                    cell.BorderRight.FillFormat.FillType = FillType.Solid;
                    cell.BorderRight.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderRight.Width = 1;

                    if (seriesnumber > 0 && (seriesnumber + 1 != row.AsICellCollection.Count))
                        cell.BorderRight.DashStyle = LineDashStyle.Dot;
                    else
                    {
                        cell.BorderRight.Width = 1.875;
                        cell.BorderRight.Style = LineStyle.Single;
                    }

                    cell.BorderBottom.FillFormat.FillType = FillType.Solid;
                    cell.BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                    cell.BorderBottom.Width = 1.875;
                    cell.BorderBottom.Style = LineStyle.Single;
                    seriesnumber += 1;
                }
            }
            seriesnumber = 0;
            foreach (ICell cell in tbl.Rows[0])
            {
                if (seriesnumber > 0)
                {
                    string[] legendSplitValues;

                    if (legendvalues.Count >= seriesnumber)
                    {
                        legendSplitValues = legendvalues[seriesnumber].Split('(');
                        cell.TextFrame.Text = legendSplitValues[0].Trim();
                    }
                    cell.FillFormat.FillType = FillType.Solid;
                    cell.FillFormat.SolidFillColor.Color = GetSerirsColour(seriesnumber - 1);
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = dynamicefontSize;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.White;
                    cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                    cell.TextAnchorType = TextAnchorType.Center;
                }
                if (seriesnumber == 0)
                {

                    cell.FillFormat.FillType = FillType.Solid;
                    cell.FillFormat.SolidFillColor.Color = System.Drawing.ColorTranslator.FromHtml("#595959");
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;

                }
                seriesnumber += 1;
            }
            int cellindex = 0;
            System.Data.DataTable _tbl = null;
            objectivelist = GetLegendList(tble);
            for (int i = 1; i < tbl.Rows.Count; i++)
            {
                seriesnumber = 0;
                cellindex = 0;
                foreach (ICell cell in tbl.Rows[i])
                {
                    var query = (from row in tble.AsEnumerable()
                                 where Convert.ToString(row.Field<object>("Objective")).Equals(objectivelist[cellindex], StringComparison.OrdinalIgnoreCase)
                                 select row);
                    _tbl = query.CopyToDataTable();

                    if (seriesnumber > 0)
                    {
                        if (legendvalues.Count >= seriesnumber)
                            cell.TextFrame.Text = (Convert.ToString(_tbl.Rows[i - 1]["MetricItem"]).Equals("Average Amount Spent on Basket", StringComparison.OrdinalIgnoreCase) ? "$" : string.Empty) + (i == 1 ? (Convert.ToDouble(_tbl.Rows[i - 1]["Volume"]) * 100).ToString("0.0") : (Convert.ToDouble(_tbl.Rows[i - 1]["Volume"]) * 100).ToString("0"));
                        cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 24;
                        cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                        cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(Convert.ToString(_tbl.Rows[i - 1]["Significance"]), objectivelist[cellindex], Convert.ToString(_tbl.Rows[i - 1]["SampleSize"]));
                        cell.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                        cell.FillFormat.FillType = FillType.NoFill;
                        cell.TextAnchorType = TextAnchorType.Center;
                        cellindex += 1;
                    }
                    if (seriesnumber == 0)
                    {

                        cell.FillFormat.FillType = FillType.Solid;
                        cell.FillFormat.SolidFillColor.Color = System.Drawing.Color.FromArgb(192, 192, 192);
                        cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                        cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = System.Drawing.Color.FromArgb(89, 89, 89);
                        //Create_Image(slide, imagepath, (image_x_position + 25), image_y_position, 20, 20);
                    }
                    seriesnumber += 1;
                }
            }
            //Set table border
            SetTableBorder(tbl);
        }

        #endregion
        #region Create Image
        public void Create_Image(ISlide slide, string imagepath, float _image_x_position, float _image_y_position, float _image_width, float _image_height)
        {
            System.Drawing.Image img = (System.Drawing.Image)new Bitmap(imagepath);
            IPPImage imgx = pres.Images.AddImage(img);

            //Add Picture Frame with height and width equivalent of Picture
            sld.Shapes.AddPictureFrame(ShapeType.Rectangle, _image_x_position, _image_y_position, _image_width, _image_height, imgx);
        }
        #endregion

        //MaxValue Calucation only for Clustered_Bar_Chart
        public void Get_Chart_maxAndMinValue(System.Data.DataTable tbl, ref double minvalue, ref double maxvalue)
        {
            if (tbl != null && tbl.Rows.Count > 0)
            {
                maxvalue = (from r in tbl.AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).Max() + 0.05;
                minvalue = 0.0;
            }
        }

        public void Get_Chart_MetricItemVolume(System.Data.DataTable tbl, ref string topmetricItem, ref double topmetricItemVolume, string Benchlist)
        {
            if (tbl != null && tbl.Rows.Count > 0)
            {
                topmetricItem = (from r in tbl.AsEnumerable() select r.Field<string>("MetricItem")).FirstOrDefault();
                topmetricItemVolume = (from r in tbl.AsEnumerable() select Convert.ToDouble(r.Field<object>("Volume"))).FirstOrDefault();
            }
        }
        //Get TripSummary List for 8 th slide
        public List<double> GetTripSummarylist(System.Data.DataTable tbl, string Benchlist)
        {
            objectivelistTripSummary = new List<double>();
            if (tbl != null && tbl.Rows.Count > 0)
                objectivelistTripSummary = (from r in tbl.AsEnumerable()
                                            where r.Field<string>("objective").Equals(Benchlist, StringComparison.OrdinalIgnoreCase)
                                            select Convert.ToDouble(r.Field<object>("Volume"))).ToList();
            return objectivelistTripSummary;
        }
        public string Get_ShortNames(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (_commonfunctions.HeaderTabs.ContainsKey(spVal))
                    slRetVal = _commonfunctions.HeaderTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }

        //Source Names 
        public void SourceStatSampleDynamicText(ISlide slide, ReportGeneratorParams reportparams, string texboxvalue, string samplesizeNames, string Benchlist1)
        {

            texboxvalue = "Sample Size: " + samplesizeNames;
            Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)23.76, (float)462.89, (float)639.36, (float)20.16, 10);

            texboxvalue = "Source: CCNA iSHOP Tracker- Time Period : " + Convert.ToString(reportparams.ShortTimePeriod) + ";\n" +
                          "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            if (Convert.ToString(reportparams.ModuleBlock).Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase))
            {
                texboxvalue += "; Where Purchased: " + ChannelRetailersVisited;
            }
            ((IAutoShape)pres.Masters[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Text = texboxvalue;

            texboxvalue = " * Stat tested at 95% CL against "+ Benchlist1;

            ((IAutoShape)pres.Masters[0].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = texboxvalue;
            
            texboxvalue = "Benchmark – " + Benchlist1;

            var tempGroup = ((IGroupShape)pres.Masters[0].Shapes.Where(x => x.Name == "benchmarkGroup").FirstOrDefault());
            ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "benchmark").FirstOrDefault()).TextFrame.Text = texboxvalue;
            //texboxvalue = "Source: iSHOP; " + Convert.ToString(reportparams.ShortTimePeriod) + "; " +
            //              "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            //if (Convert.ToString(reportparams.ModuleBlock).Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase))
            //{
            //    texboxvalue += "; Where Purchased: " + ChannelRetailersVisited;
            //}

            //Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(127, 127, 127), (float)23.81, (float)486.99, (float)416.69, (float)9.92, 8);


            //texboxvalue = "Stat test: " + Convert.ToString(StatTesting) + "% confidence interval vs, " + Benchlist1;
            //Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(127, 127, 127), (float)438.23, (float)485.57, (float)238.11, (float)24.94, 8);
        }
        //
        //Within Source Names 
        public void WithinSourceStatSampleDynamicText(ISlide slide, ReportGeneratorParams reportparams, string texboxvalue, string samplesizeNames, string Benchlist1)
        {
            texboxvalue = "Source: CCNA iSHOP Tracker- Time Period : " + Convert.ToString(reportparams.ShortTimePeriod) + "; Base - " + ShopperSegment + "\n" +
              "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            if (Convert.ToString(reportparams.ModuleBlock).Equals("WithinBeverageTrips", StringComparison.OrdinalIgnoreCase))
            {
                texboxvalue += "; Where Purchased: " + ChannelRetailersVisited;
            }
            ((IAutoShape)pres.Masters[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Text = texboxvalue;

            texboxvalue = " * Stat tested at 95% CL against " + Benchlist1;

            ((IAutoShape)pres.Masters[0].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = texboxvalue;

            texboxvalue = "Benchmark – " + Benchlist1;

            var tempGroup = ((IGroupShape)pres.Masters[0].Shapes.Where(x => x.Name == "benchmarkGroup").FirstOrDefault());
            ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "benchmark").FirstOrDefault()).TextFrame.Text = texboxvalue;

            texboxvalue = "Sample Size: " + samplesizeNames;
            Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(89, 89, 89), (float)21.54, (float)457.22, 430, (float)39.96, 10);

            //    texboxvalue = "Source: iSHOP; " + Convert.ToString(reportparams.ShortTimePeriod) + "; " + ShopperSegment + "\n" +
            //                  "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            //    if (Convert.ToString(reportparams.ModuleBlock).Equals("WithinBeverageTrips", StringComparison.OrdinalIgnoreCase))
            //    {
            //        texboxvalue += "\nWhere Purchased: " + ChannelRetailersVisited;
            //    }

            //    Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(127, 127, 127), (float)20.69, (float)467.71, (float)416.69, (float)9.92, 8);


            //    texboxvalue = "Stat test: " + Convert.ToString(StatTesting) + "% confidence interval vs, " + Benchlist1;
            //    Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(127, 127, 127), (float)438.23, (float)485.57, (float)238.11, (float)24.94, 8);
        }

            //Trend Source Names 
        public void TrendSourceStatSampleDynamicText(ISlide slide, ReportGeneratorParams reportparams, string texboxvalue, string samplesizeNames, string Benchlist1)
        {
            texboxvalue = "Source: CCNA iSHOP Tracker- Time Period : " + Convert.ToString(reportparams.ShortTimePeriod) + "; Base - " + ShopperSegment + "\n" +
                  "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            if (Convert.ToString(reportparams.ModuleBlock).Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
            {
                texboxvalue += "; Where Purchased: " + ChannelRetailersVisited;
            }
            ((IAutoShape)pres.Masters[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Text = texboxvalue;

            texboxvalue = " * Stat tested at 95% CL against " + Benchlist1;

            ((IAutoShape)pres.Masters[0].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = texboxvalue;

            texboxvalue = "Benchmark – " + Benchlist1;

            var tempGroup = ((IGroupShape)pres.Masters[0].Shapes.Where(x => x.Name == "benchmarkGroup").FirstOrDefault());
            ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "benchmark").FirstOrDefault()).TextFrame.Text = texboxvalue;

            //texboxvalue = "Source: iSHOP; " + Convert.ToString(reportparams.ShortTimePeriod) + "; " + ShopperSegment + "\n" +
            //              "Advanced Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            //if (Convert.ToString(reportparams.ModuleBlock).Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
            //{
            //    texboxvalue += "\nWhere Purchased: " + ChannelRetailersVisited;
            //}

            //Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(127, 127, 127), (float)23.81, (float)469.98, (float)416.69, (float)9.92, 8);


            //texboxvalue = "Stat test: " + Convert.ToString(StatTesting) + "% confidence interval vs, " + Benchlist1;
            //Create_Label(slide, texboxvalue, Color.FromArgb(242, 242, 242), Color.FromArgb(127, 127, 127), (float)491.24, (float)485.57, (float)238.11, (float)24.94, 8);
        }
        // get beverage temparature metric short name
        public string GetBeverageTemperatureMetricShortName(string metric)
        {
            switch (metric)
            {
                case "Bev Aisle; Room Temp":
                    {
                        metric = "the Beverage Aisle at Room Temp";
                        break;
                    }
                case "Refrigerated Case/Cooler, Back of Store; Chilled":
                    {
                        metric = "the Refrigerated Case/Cooler in the back of store and was chilled";
                        break;
                    }
                case "Refrigerated Case/Cooler, Aisle of Store; Chilled":
                    {
                        metric = "the Refrigerated Case/Cooler in the aisle of store and was chilled";
                        break;
                    }
                case "Refrigerated Case/Cooler, Near Checkout; Chilled":
                    {
                        metric = "the Refrigerated Case/Cooler near checkout and was chilled";
                        break;
                    }
                case "Fountain Dispenser; Chilled":
                    {
                        metric = "the Fountain Dispenser and was chilled";
                        break;
                    }
                case "Display; Room Temp":
                    {
                        metric = "the Display at room temperature";
                        break;
                    }
                case "Refrigerated Case/Cooler, Deli/Bakery/Café; chilled":
                    {
                        metric = "the Refrigerated Case/Cooler, Deli/Bakery/Café and was chilled";
                        break;
                    }
                case "Other Chilled":
                    {
                        metric = "Other places and was chilled";
                        break;
                    }
                case "Barrel Cooler/Bin with Ice, Front of Store; chilled":
                    {
                        metric = "the Barrel Cooler/Bin with Ice in Front of Store and was chilled";
                        break;
                    }
                case "Other Room Temp":
                    {
                        metric = "Other places at room temperature";
                        break;
                    }
            }
            return metric;
        }
        public int dynamicFontSize(List<string> legendvalues)
        {
            int fontSize = 16;
            int checkLength = 0;
            string[] legendSplitValues;
            int[] lens = new int[legendvalues.Count];
            foreach (string lgs in legendvalues)
            {
                legendSplitValues = lgs.Split('(');
                checkLength = legendSplitValues[0].Length;
                lens[legendvalues.IndexOf(lgs)] = checkLength;
            }

            int maxValue = lens.Max();

            if (legendvalues[0] == "")
            {
                legendvalues.RemoveAt(0);
            }

            if (legendvalues.Count == 3)
            {
                if (maxValue >= 51)
                {
                    fontSize = 13;

                }
            }
            else if (legendvalues.Count == 4)
            {

                if (maxValue >= 40 && maxValue <= 50)
                {
                    fontSize = 14;
                }
                else if (maxValue >= 51)
                {
                    fontSize = 12;

                }

            }
            else if (legendvalues.Count == 5)
            {
                if ((maxValue > 25 && maxValue <= 35) || maxValue <= 25)
                {
                    fontSize = 16;
                }
                else
                {
                    fontSize = 12;
                }

            }
            return fontSize;
        }

    }
}