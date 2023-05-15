using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Aspose.Slides;
using Aspose.Slides.Charts;
using iSHOPNew.DAL;

namespace iSHOPNew.Models
{
    public class AsposeDownload
    {
        Dictionary<string, double> samplesizelist = null;
        List<Dictionary<string, double>> samplesizeArray = null;
        CommonFunctions commonfunctions = new CommonFunctions();
        List<string> objectivelist = null;
        List<string> metriclist = null;
        DataSet ds = null;
        public static Random rnd = new Random();

        public double accuratestatvalueposi;
        public double accuratestatvaluenega;

        float chart_x_position = (float)10.8;
        float chart_y_position = (float)41.10;
        float chart_width = (float)940.32;
        float chart_height = (float)448.72;
        Presentation pres = new Presentation(HttpContext.Current.Server.MapPath("~/ProfilerPPTFiles/ProfilerPPTTemplate/ProfilerTemplate.pptx")); // creates a blank presentation with one blank slide.  must be done first
        ISlideCollection slds;
        public AsposeDownload()
        {

        }

        public void GeneratePPT(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));

            slds = pres.Slides;
            Dictionary<string, ProfilerChartParams> chartlist = HttpContext.Current.Session["ProfilerChartExportList"] as Dictionary<string, ProfilerChartParams>;
            if ((HttpContext.Current.Session["ProfilerChartExportList"] == null && chartlist == null) || chartlist.Count == 0)
            {
                ProfilerChartParams pparams = HttpContext.Current.Session["ProfilerChartData"] as ProfilerChartParams;
                chartlist = new Dictionary<string, ProfilerChartParams>();
                chartlist.Add("Chart_0", pparams);
            }
            int i = 0;
            foreach (ProfilerChartParams profilerparams in chartlist.Values)
            {
                profilerparams.ChartDataSet = FormatDataSet(profilerparams.ChartDataSet);
                accuratestatvalueposi = profilerparams.StatPositive;
                accuratestatvaluenega = profilerparams.StatNegative;

                 chart_x_position = (float)10.8;
                 chart_y_position = (float)41.10;
                 chart_width = (float)940.32;
                 chart_height = (float)448.72;
                if (profilerparams.ChartType.Contains("Pyramid"))
                {
                    List<string> objlist = (from r in profilerparams.ChartDataSet.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
                    profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
                    objlist = objlist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();

                    List<string> retailerlist = new List<string>();
                    if (objlist != null && objlist.Count > 4)
                    {
                        for (int j = 0; j < objlist.Count; j++)
                        {
                            retailerlist.Add(objlist[j]);
                            if (retailerlist.Count == 4)
                            {
                                slds.AddClone(pres.Slides[0]);
                                ISlide sld = slds[i + 1];
                                Plot_PyramidChart(sld, profilerparams, profilerparams.ChartDataSet.Copy(), retailerlist);
                                //added By sharath for adding chg Vs Py on 02-11-2015
                                if (profilerparams.ChartType.Equals("Pyramid with Change", StringComparison.OrdinalIgnoreCase))
                                    PyramidChngVsPYTable(sld, profilerparams, profilerparams.ChartDataSet.Copy(), retailerlist);
                                //end   
                                retailerlist = new List<string>();
                                i += 1;
                            }
                            else if (objlist.Count == (j + 1))
                            {
                                slds.AddClone(pres.Slides[0]);
                                ISlide sld = slds[i + 1];
                                Plot_PyramidChart(sld, profilerparams, profilerparams.ChartDataSet.Copy(), retailerlist);
                                //added By sharath for adding chg Vs Py on 02-11-2015
                                if (profilerparams.ChartType.Equals("Pyramid with Change", StringComparison.OrdinalIgnoreCase))
                                    PyramidChngVsPYTable(sld, profilerparams, profilerparams.ChartDataSet.Copy(), retailerlist);
                                //end   
                                retailerlist = new List<string>();
                                i += 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        slds.AddClone(pres.Slides[0]);
                        ISlide sld = slds[i + 1];
                        Plot_PyramidChart(sld, profilerparams, profilerparams.ChartDataSet.Copy(), objlist);
                        //added By sharath for adding chg Vs Py on 02-11-2015
                        if (profilerparams.ChartType.Equals("Pyramid with Change", StringComparison.OrdinalIgnoreCase))
                            PyramidChngVsPYTable(sld, profilerparams, profilerparams.ChartDataSet.Copy(), objlist);
                        //end                      
                        i += 1;
                    }
                }
                else
                {
                    slds.AddClone(pres.Slides[0]);
                    ISlide sld = slds[i + 1];  // pres.Slides returns a list of slides in the presentation, sets sld to first slide            
                    i += 1;
                    // chart     
                    switch (profilerparams.ChartType.ToLower())
                    {
                        case "stacked column":
                            {
                                Plot_StackedColumnChart(sld, profilerparams);
                                break;
                            }
                        case "stacked bar":
                            {
                                Plot_StackedBarChart(sld, profilerparams);
                                break;
                            }
                        case "clustered column":
                            {
                                Plot_ClusteredColumnChart(sld, profilerparams);
                                break;
                            }
                        case "clustered bar":
                            {
                                Plot_ClusteredBarChart(sld, profilerparams);
                                break;
                            }
                        case "line":
                        case "stacked line":
                            {
                                if (profilerparams.Comparisonlist == "")
                                    Plot_LineChart(sld, profilerparams);
                                else
                                    Plot_LineChartNew(sld, profilerparams);
                                break;
                            }
                        case "stacked area":
                            {
                                Plot_AreaChart(sld, profilerparams);
                                break;
                            }
                        case "bar with change":
                            {
                                Plot_ClusteredBarwithChangeChart(sld, profilerparams);
                                break;
                            }
                    }
                }
            }
            pres.Slides.RemoveAt(0);

            string filename = "Profiler_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds));

            pres.Save(HttpContext.Current.Server.MapPath("~/Temp/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);

            FileStream fs1 = null;
            fs1 = System.IO.File.Open(HttpContext.Current.Server.MapPath("~/Temp/" + filename + ".pptx"), System.IO.FileMode.Open);

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

        private void Plot_LineChart(ISlide sld, ProfilerChartParams profilerparams)
        {
            ds = profilerparams.ChartDataSet.Copy();

            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();
            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[1].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
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
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i].Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "") + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")"));
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
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(_objective));
                    LabelFontSize(lbl);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                //Series.Marker.Format.Fill.FillType = FillType.Solid;
                //Series.Marker.Size = 3;
                //Series.Marker.Format.Line.Width = 3;
                ////Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = Color.FromArgb(127, 233, 4, 55);
                //SetMarketStyle(Series, serCount - 1);
                //Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;

                //Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = Color.FromArgb(127, GetSerirsColour(serCount - 1));
                //Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                //Series.Format.Line.FillFormat.FillType = FillType.Solid;
                //Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);
                //Series.Format.Line.DashStyle = LineDashStyle.Dot;

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1); ;
                Series.Format.Line.DashStyle = LineDashStyle.SystemDot;
                Series.Format.Line.Width = 3;
                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1); ;
                //Added marker outer border for line chart-starts here
                Series.Marker.Size = 9;
                Series.Marker.Symbol = MarkerStyleType.Circle;//Triangle
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Gradient;
                Series.Marker.Format.Line.Width = 3.5;
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientShape = GradientShape.Path;
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientDirection = GradientDirection.FromCenter;
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Clear();
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)0.5, Color.White);
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)0.5, Color.White);
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)0.6, GetSerirsColour(serCount - 1));
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)1.0, GetSerirsColour(serCount - 1));
                //Added marker outer border for line chart-ends here

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            //chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.VerticalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.Width = 0;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            //chart.Axes.VerticalAxis.IsVisible = false;

            //chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            //chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            //set axis max value
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.MaxValue = GetAxisMaxValueTrend(ds.Tables[1]);

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            #endregion

            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //

            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //
        }

        private void Plot_LineChartNew(ISlide sld, ProfilerChartParams profilerparams)
        {
            ds = profilerparams.ChartDataSet.Copy();

            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();
            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[0].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
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
            samplesizeArray = CommonFunctions.LoadChartSampleSizeSizeNew(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                //string samplesize = (from r in ds.Tables[0].AsEnumerable()
                //                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                //                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                //                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i].Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "") + " (NA)"));
            }

            //Take first chart series
            IChartSeries Series = chart.ChartData.Series[0];

            //Now populating series data
            IDataLabel lbl;
            int serCount = 1, catcount = 1;
            foreach (string series in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                int index = metriclist.FindIndex(x => x.StartsWith(series));
                foreach (string _objective in objectivelist)
                {
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizelessNew(Convert.ToString(r.Field<object>("Objective")), index) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSizeNew(_objective, index));
                    LabelFontSize(lbl);
                    catcount++;
                }
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                //Series.Marker.Format.Fill.FillType = FillType.Solid;
                //Series.Marker.Size = 5;
                //SetMarketStyle(Series, serCount - 1);
                //Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                //Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                //Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                //Series.Format.Line.FillFormat.FillType = FillType.Solid;
                //Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1); ;
                Series.Format.Line.DashStyle = LineDashStyle.SystemDot;
                Series.Format.Line.Width = 3;
                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1); ;
                //Added marker outer border for line chart-starts here
                Series.Marker.Size = 9;
                Series.Marker.Symbol = MarkerStyleType.Circle;//Triangle
                Series.Marker.Format.Line.FillFormat.FillType = FillType.Gradient;
                Series.Marker.Format.Line.Width = 3.5;
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientShape = GradientShape.Path;
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientDirection = GradientDirection.FromCenter;
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Clear();
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)0.5, Color.White);
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)0.5, Color.White);
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)0.6, GetSerirsColour(serCount - 1));
                Series.Marker.Format.Line.FillFormat.GradientFormat.GradientStops.Add((float)1.0, GetSerirsColour(serCount - 1));

                catcount = 1;
                serCount++;
            }

            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            //chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.VerticalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.Width = 0;

            //chart.Axes.VerticalAxis.IsVisible = false;

            //chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            //chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            //set axis max value
            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.MaxValue = GetAxisMaxValueTrend(ds.Tables[1]);

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            #endregion

            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //

            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //
        }

        private void Plot_ClusteredBarChart(ISlide sld, ProfilerChartParams profilerparams)
        {
            ds = profilerparams.ChartDataSet.Copy();

            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();
            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();
            objectivelist.Reverse();
            metriclist.Reverse();
            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[1].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1] + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")"), chart.Type);
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
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(chart.ChartData.Series.Count - serCount);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(_objective));
                    LabelFontSize(lbl);
                    catcount++;
                }
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

            chart.Axes.VerticalAxis.IsVisible = true;
            chart.Axes.HorizontalAxis.IsVisible = false;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
           
            #endregion

            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //

            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //
        }
        private void Plot_ClusteredBarwithChangeChart(ISlide sld, ProfilerChartParams profilerparams)
        {
            #region Plotting the Bar Chart
            //added by sharath on 29-10-2015 for splitting the table and chart
            chart_height = chart_height - 155;
            //end
            ds = profilerparams.ChartDataSet.Copy();
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();
            objectivelist.Reverse();
            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();
            metriclist.Reverse();
            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[1].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1] + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")"), chart.Type);
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
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(objectivelist.Count() - serCount);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(_objective));
                    LabelFontSize(lbl);
                    catcount++;
                }
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

            chart.Axes.VerticalAxis.IsVisible = true;
            chart.Axes.HorizontalAxis.IsVisible = false;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            #endregion

            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //

            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //
            #endregion

            #region Add Table to the Slide


            List<double> tblCols = new List<double>();

            ITable tbl;

            if (metriclist.Count < 3)
                tblCols.Add(939.6 / (metriclist.Count + 1));
            else
                tblCols.Add(939.6 / (metriclist.Count + 1));

            double[] tblRows = { 80 / (objectivelist.Count + 1) };

            tbl = sld.Shapes.AddTable((float)10.8, 342, tblCols.ToArray(), tblRows);

            tbl[0, 0].FillFormat.FillType = FillType.NoFill;
            tbl[0, 0].BorderTop.FillFormat.FillType = FillType.NoFill;
            tbl[0, 0].BorderBottom.FillFormat.FillType = FillType.NoFill;
            tbl[0, 0].BorderRight.FillFormat.FillType = FillType.Solid;
            tbl[0, 0].BorderRight.FillFormat.SolidFillColor.Color = Color.LightPink;
            tbl[0, 0].BorderLeft.FillFormat.FillType = FillType.NoFill;

            objectivelist.Reverse();

            for (int i = 0; i < objectivelist.Count; i++)
            {
                tbl.Rows.AddClone(tbl.Rows[0], true);
                tbl[0, i + 1].TextFrame.Text = objectivelist[i];
                tbl[0, i + 1].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;
                tbl[0, i + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
                tbl[0, i + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial (Body)");

                tbl[0, i + 1].FillFormat.FillType = FillType.NoFill;

                tbl[0, i + 1].BorderTop.FillFormat.FillType = FillType.NoFill;
                tbl[0, i + 1].BorderBottom.FillFormat.FillType = FillType.NoFill;
                tbl[0, i + 1].BorderRight.FillFormat.FillType = FillType.Solid;
                tbl[0, i + 1].BorderRight.FillFormat.SolidFillColor.Color = Color.LightPink;
                tbl[0, i + 1].BorderLeft.FillFormat.FillType = FillType.NoFill;
            }

            metriclist.Reverse();
            for (int i = 0; i < metriclist.Count; i++)
            {
                tbl.Columns.AddClone(tbl.Columns[0], true);
                tbl[i + 1, 0].TextFrame.Text = metriclist[i];
                tbl[i + 1, 0].TextAnchorType = TextAnchorType.Center;
                tbl[i + 1, 0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                tbl[i + 1, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
                tbl[i + 1, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial (Body)");

                tbl[i + 1, 0].FillFormat.FillType = FillType.Solid;
                tbl[i + 1, 0].FillFormat.SolidFillColor.Color = Color.Gray;

                tbl[i + 1, 0].FillFormat.FillType = FillType.Solid;
                tbl[i + 1, 0].FillFormat.SolidFillColor.Color = Color.Gray;

                tbl[i + 1, 0].BorderTop.FillFormat.FillType = FillType.Solid;
                tbl[i + 1, 0].BorderTop.FillFormat.SolidFillColor.Color = Color.LightPink;
                tbl[i + 1, 0].BorderTop.Width = 1;

                tbl[i + 1, 0].BorderBottom.FillFormat.FillType = FillType.Solid;
                tbl[i + 1, 0].BorderBottom.FillFormat.SolidFillColor.Color = Color.LightPink;
                tbl[i + 1, 0].BorderBottom.Width = 1;

                tbl[i + 1, 0].BorderLeft.FillFormat.FillType = FillType.Solid;
                tbl[i + 1, 0].BorderLeft.FillFormat.SolidFillColor.Color = Color.LightPink;
                tbl[i + 1, 0].BorderLeft.Width = 1;

                tbl[i + 1, 0].BorderRight.FillFormat.FillType = FillType.Solid;
                tbl[i + 1, 0].BorderRight.FillFormat.SolidFillColor.Color = Color.LightPink;
                tbl[i + 1, 0].BorderRight.Width = 1;

            }


            for (int i = 0; i < objectivelist.Count; i++)
            {
                for (int j = 0; j < metriclist.Count; j++)
                {
                    var SSCheck = ds.Tables[1].AsEnumerable().Where(x => x.Field<string>("Objective").ToLower() == objectivelist[i].ToLower() && x.Field<string>("MetricItem").ToLower() == metriclist[j].ToLower()).Select(x => x.Field<object>("SampleSize")).FirstOrDefault();
                    var significance = ds.Tables[1].AsEnumerable().Where(x => x.Field<string>("Objective").ToLower() == objectivelist[i].ToLower() && x.Field<string>("MetricItem").ToLower() == metriclist[j].ToLower()).Select(x => x.Field<object>("Significance")).FirstOrDefault();
                    var ChangeVal = ds.Tables[1].AsEnumerable().Where(x => x.Field<string>("Objective").ToLower() == objectivelist[i].ToLower() && x.Field<string>("MetricItem").ToLower() == metriclist[j].ToLower()).Select(x => x.Field<object>("ChangePY")).FirstOrDefault();
                    if (Convert.ToInt32(SSCheck) < GlobalVariables.LowSample)
                    {
                        tbl[j + 1, i + 1].TextFrame.Text = GlobalVariables.NA;
                    }
                    else
                    {
                        if (ChangeVal != null)
                        {
                            ChangeVal = Math.Round((Convert.ToDouble(ChangeVal)), 1).ToString("0.0");
                            tbl[j + 1, i + 1].TextFrame.Text = ChangeVal.ToString();
                            tbl[j + 1, i + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            tbl[j + 1, i + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(Convert.ToString(significance), GetSampleSize(objectivelist[i]));//ChangeColorOnSignificance(Convert.ToDouble(significance), SSCheck == null ? 0.0 : Convert.ToDouble(SSCheck));

                        }
                        else
                        {
                            tbl[j + 1, i + 1].TextFrame.Text = GlobalVariables.NA;

                        }
                    }
                    tbl[j + 1, i + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
                    tbl[j + 1, i + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial (Body)");
                    tbl[j + 1, i + 1].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                    tbl[j + 1, i + 1].TextAnchorType = TextAnchorType.Center;

                    tbl[j + 1, i + 1].BorderTop.FillFormat.FillType = FillType.Solid;
                    tbl[j + 1, i + 1].BorderTop.FillFormat.SolidFillColor.Color = Color.LightPink;
                    tbl[j + 1, i + 1].BorderTop.Width = 1;

                    tbl[j + 1, i + 1].BorderBottom.FillFormat.FillType = FillType.Solid;
                    tbl[j + 1, i + 1].BorderBottom.FillFormat.SolidFillColor.Color = Color.LightPink;
                    tbl[j + 1, i + 1].BorderBottom.Width = 1;

                    tbl[j + 1, i + 1].BorderLeft.FillFormat.FillType = FillType.Solid;
                    tbl[j + 1, i + 1].BorderLeft.FillFormat.SolidFillColor.Color = Color.LightPink;
                    tbl[j + 1, i + 1].BorderLeft.Width = 1;

                    tbl[j + 1, i + 1].BorderRight.FillFormat.FillType = FillType.Solid;
                    tbl[j + 1, i + 1].BorderRight.FillFormat.SolidFillColor.Color = Color.LightPink;
                    tbl[j + 1, i + 1].BorderRight.Width = 1;

                    tbl[j + 1, i + 1].FillFormat.FillType = FillType.NoFill; tbl[j + 1, i + 1].FillFormat.FillType = FillType.Solid;
                    tbl[j + 1, i + 1].FillFormat.SolidFillColor.Color = Color.White;

                    //tbl[j + 1, i + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = ChangeColorOnSignificance(significance == null ? 0.0 : Convert.ToDouble(significance), SSCheck == null ? 0.0 : Convert.ToDouble(SSCheck));

                }

            }
            #endregion

        }



        private void Plot_ClusteredColumnChart(ISlide sld, ProfilerChartParams profilerparams)
        {
            ds = profilerparams.ChartDataSet.Copy();

            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();
            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[1].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
            //Setting the index of chart data sheet
            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();

            int s = chart.ChartData.Series.Count;
            s = chart.ChartData.Categories.Count;
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new series
            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[i - 1]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1] + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")"), chart.Type);
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
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(_objective));
                    LabelFontSize(lbl);
                    catcount++;
                }
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

            chart.Axes.VerticalAxis.IsVisible = false;

            //chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            //set max value
            chart.Axes.VerticalAxis.MaxValue = GetAxisMaxValue(ds.Tables[1]);

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            #endregion

            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //

            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //
        }
        private double GetAxisMaxValue(System.Data.DataTable tbl)
        {
            double maxval = 0.0;
            if (tbl != null && tbl.Rows.Count > 0)
            {
                var query = (from row in tbl.AsEnumerable() orderby row["Volume"] descending select Convert.ToString(row["Volume"])).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                    maxval = Convert.ToDouble(query);
            }
            return maxval;
        }
        private double GetAxisMaxValueTrend(System.Data.DataTable tbl)
        {
            double maxval = 0.0;
            if (tbl != null && tbl.Rows.Count > 0)
            {
                var query = (from row in tbl.AsEnumerable() orderby row["Volume"] descending select Convert.ToString(row["Volume"])).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                    maxval = Convert.ToDouble(query);
                maxval = Convert.ToDouble(query) * 100;
                maxval = Math.Floor(maxval);
                maxval = Math.Floor(((maxval + 20) / 5.0)) * 5.0;

                maxval = (maxval) * 0.01;
            }
            return maxval;
        }
        public void Plot_StackedColumnChart(ISlide sld, ProfilerChartParams profilerparams)
        {
            ds = profilerparams.ChartDataSet.Copy();

            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();

            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();

            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[1].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
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
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                     && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i] + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")"));
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
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.ParentSeriesGroup.Overlap = 100;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.HighlightColor.Color = Color.FromArgb(178,255,255,255);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(_objective));
                    LabelFontSize(lbl);
                    catcount++;
                }
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

            chart.Axes.VerticalAxis.IsVisible = false;

            //chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            #endregion
            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //
            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //
        }

        public void Plot_StackedBarChart(ISlide sld, ProfilerChartParams profilerparams)
        {
            ds = profilerparams.ChartDataSet.Copy();

            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();
            objectivelist.Reverse();
            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[1].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
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
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i] + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")"));
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
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.ParentSeriesGroup.Overlap = 100;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Center;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.HighlightColor.Color = Color.FromArgb(76, 255, 255, 255);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(_objective));
                    LabelFontSize(lbl);
                    catcount++;
                }
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

            chart.Axes.VerticalAxis.IsVisible = true;
            chart.Axes.HorizontalAxis.IsVisible = false;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");

            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //

            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //           
            #endregion
        }

        public void Plot_AreaChart(ISlide sld, ProfilerChartParams profilerparams)
        {
            ds = profilerparams.ChartDataSet.Copy();

            objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();
            profilerparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames.Select(x => x.ToUpper()).ToList();
            objectivelist = objectivelist.OrderBy(x => profilerparams.Comparison_ShortNames.IndexOf(x)).ToList();
            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().ToList();

            IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);
            chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ds.Tables[1].Rows[0]["Metric"]));
            chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
            chart.ChartTitle.Overlay = false;
            chart.ChartTitle.Height = 20;
            chart.HasTitle = true;

            //Set first series to Show Values
            chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
            Chart_Styles(chart, profilerparams);
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
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[i]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i].Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "") + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")"));
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
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForAreaSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";

                    Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                    Series.ParentSeriesGroup.Overlap = 100;

                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Line.FillFormat.SolidFillColor.Color = Color.Transparent;

                    Series.Marker.Format.Fill.FillType = FillType.Solid;
                    Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(metriclist.Count() - serCount);
                    Series.Marker.Size = 5;
                    SetMarketStyle(Series, serCount);
                    Series.Marker.Format.Line.FillFormat.FillType = FillType.Solid;
                    Series.Marker.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(metriclist.Count() - serCount);


                    Series.Format.Fill.FillType = FillType.Solid;
                    Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(metriclist.Count() - serCount);

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.HighlightColor.Color = Color.FromArgb(76, 255, 255, 255);
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(_objective));
                    LabelFontSize(lbl);
                    catcount++;
                }
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

            chart.Axes.VerticalAxis.IsVisible = false;

            //chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;
            //chart.Axes.VerticalAxis.IsAutomaticMinValue = false;
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;

            #region Set Legend
            //Set Legend Style
            chart.HasLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            #endregion
            //PlotInputSelection
            PlotChartTitle(sld, profilerparams, chart);
            //
            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            //
        }

        public void Plot_PyramidChart(ISlide sld, ProfilerChartParams profilerparams, DataSet _ds, List<string> _objectivelist)
        {
            ds = _ds;
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();           
            objectivelist = _objectivelist;
            var query2 = from r in ds.Tables[1].AsEnumerable() select r;
            ds.Tables.RemoveAt(1);
            ds.Tables.Add(query2.CopyToDataTable());

            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().Reverse().ToList();
            //Chart Title
            IAutoShape ChartMainTitle = sld.Shapes.AddAutoShape(ShapeType.Rectangle, 5, (float)41.95, (float)950.4, (float)20.16, true);
            ChartMainTitle.AddTextFrame(" ");

            ChartMainTitle.TextFrame.Paragraphs[0].Portions[0].Text = Convert.ToString(ds.Tables[1].Rows[0]["Metric"]);
            ChartMainTitle.ShapeStyle.LineColor.Color = Color.Transparent;
            ChartMainTitle.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;


            ChartMainTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ChartMainTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;

            ChartMainTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 10;
            ChartMainTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            ChartMainTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");

            ChartMainTitle.FillFormat.FillType = FillType.Solid;
            ChartMainTitle.FillFormat.SolidFillColor.Color = Color.White;

            //

            //Filters
            IAutoShape ChartFilters = sld.Shapes.AddAutoShape(ShapeType.Rectangle, 5, (float)68.88, 710, 20, true);
            ChartFilters.AddTextFrame(" ");
            ChartFilters.TextFrame.Paragraphs[0].Portions[0].Text = Convert.ToString(profilerparams.FilterShortNames.ToUpper());
            ChartFilters.ShapeStyle.LineColor.Color = Color.Transparent;
            ChartFilters.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;


            ChartFilters.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            ChartFilters.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;

            ChartFilters.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
            ChartFilters.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
            ChartFilters.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");

            ChartFilters.FillFormat.FillType = FillType.Solid;
            ChartFilters.FillFormat.SolidFillColor.Color = Color.White;
            //

            chart_x_position = (float)10.8;
            chart_y_position = (float)63.21;
            chart_width = (float)462.96;
            chart_height = (float)198.42;

            int crindex = 1;

            for (int cr = 0; cr < objectivelist.Count; cr++)
            {
                if (crindex == 1)
                {
                    chart_x_position = (float)10.8;
                }
                else if (crindex == 2)
                {
                    crindex = 0;
                    chart_x_position = 486;
                    crindex = 0;
                }
                if (profilerparams.ChartType == "Pyramid with Change")
                    chart_width = (float)367.37;
                IChart chart = sld.Shapes.AddChart(GetChartType(profilerparams.ChartType), chart_x_position, chart_y_position, chart_width, chart_height);

                //chart.ChartTitle.AddTextFrameForOverriding(objectivelist[cr]);
                //chart.ChartTitle.TextFrameForOverriding.TextFrameFormat.CenterText = NullableBool.True;
                //chart.ChartTitle.Overlay = false;
                //chart.ChartTitle.Height = 20;
                //chart.HasTitle = false;

                //Set first series to Show Values
                chart.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowValue = true;
                Chart_Styles(chart, profilerparams);
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
                for (int i = 1; i < 4; i++)
                {
                    chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, "Series" + i), chart.Type);
                }

                //Adding new categories
                for (int i = 0; i < metriclist.Count; i++)
                {
                    //Setting Category Name
                    chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
                }

                //Take first chart series
                IChartSeries Series = chart.ChartData.Series[0];
                IDataLabel lbl;
                //Now populating series data            
                #region Plotting one pyramid chart
                for (int j = 0; j < metriclist.Count; j++)
                {
                    var query = (from r in ds.Tables[1].AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(objectivelist[cr], StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(metriclist[j], StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(Convert.ToString(r.Field<object>("Objective"))) ? "0" : Convert.ToString(r.Field<object>("Volume")),
                                     significance = Convert.ToString(r.Field<object>("Significance"))
                                 }).FirstOrDefault();

                    for (int i = 1; i < 4; i++)
                    {
                        Series = chart.ChartData.Series[i - 1];
                        if (i == 2)
                        {
                            if (query != null)
                            {
                                Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, j + 1, i, ((!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0))));
                                Series.DataPoints[j].Value.AsCell.CustomNumberFormat = "0.0%";
                                Series.DataPoints[j].Label.DataLabelFormat.NumberFormat = "0.0%";

                                Series.DataPoints[j].Label.DataLabelFormat.ShowValue = true;
                                Series.DataPoints[j].Label.DataLabelFormat.IsNumberFormatLinkedToSource = false;

                                //Series.Labels.DefaultDataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                                Series.Labels.DefaultDataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(objectivelist[cr]));

                                Series.DataPoints[j].Label.DataLabelFormat.Format.Fill.FillType = FillType.Solid;

                                Series.DataPoints[j].Label.DataLabelFormat.Format.Fill.SolidFillColor.Color = Color.White;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.FontHeight = 8;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");

                                Series.DataPoints[j].Format.Fill.FillType = FillType.Solid;
                                Series.DataPoints[j].Format.Fill.SolidFillColor.Color = GetSerirsColour(cr);

                                lbl = Series.DataPoints[j].Label;
                                lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(objectivelist[cr]));
                            }
                            else
                            {
                                Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, j + 1, i, GlobalVariables.NA));
                                Series.DataPoints[j].Label.AddTextFrameForOverriding(GlobalVariables.NA).Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
                                Series.DataPoints[j].Label.DataLabelFormat.IsNumberFormatLinkedToSource = true;
                                Series.DataPoints[j].Label.DataLabelFormat.Format.Fill.FillType = FillType.Solid;
                                Series.DataPoints[j].Label.DataLabelFormat.Format.Fill.SolidFillColor.Color = Color.White;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.FontHeight = 8;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
                                Series.Labels.DefaultDataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(objectivelist[cr]));
                            }

                        }
                        else
                        {
                            if (query != null)
                            {
                                Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, j + 1, i, (1 - ((!string.IsNullOrEmpty(Convert.ToString(query.value)) ? Convert.ToDouble(query.value) : 0))) / 2));
                                Series.DataPoints[j].Value.AsCell.CustomNumberFormat = "0.0%";
                                Series.DataPoints[j].Format.Fill.FillType = FillType.Solid;
                                Series.DataPoints[j].Format.Fill.SolidFillColor.Color = Color.Transparent;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.FontHeight = 8;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
                                Series.Labels.DefaultDataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(objectivelist[cr]));
                            }
                            else
                            {
                                Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, j + 1, i, (1 - 0.0) / 2));
                                Series.DataPoints[j].Value.AsCell.CustomNumberFormat = "0.0%";
                                Series.DataPoints[j].Format.Fill.FillType = FillType.Solid;
                                Series.DataPoints[j].Format.Fill.SolidFillColor.Color = Color.Transparent;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.FontHeight = 8;
                                Series.DataPoints[j].Label.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
                                Series.Labels.DefaultDataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, GetSampleSize(objectivelist[cr]));
                            }
                        }
                        Series.ParentSeriesGroup.GapWidth = 0;
                        Series.ParentSeriesGroup.Overlap = 100;

                    }

                }
                #endregion

                chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
                chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

                chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
                chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

                chart.Axes.HorizontalAxis.IsVisible = false;
                chart.Axes.VerticalAxis.MajorTickMark = TickMarkType.None;
                chart.Axes.VerticalAxis.Format.Line.FillFormat.FillType = FillType.Solid;
                chart.Axes.VerticalAxis.Format.Line.FillFormat.SolidFillColor.Color = Color.Transparent;

                chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

                #region Set Legend
                //Set Legend Style
                chart.HasLegend = false;
                //PlotInputSelection
                //PlotChartTitle(sld, profilerparams, chart);
                //

                #endregion
                samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
                //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
                //Set Retailer
                string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                     where Convert.ToString(objectivelist[cr]).Equals(r.Field<string>("Objective"), StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(r.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(r.Field<object>("Volume"))).Distinct().FirstOrDefault();

                //updated By sharath on 18-08-2015
                IAutoShape ChartTitle = sld.Shapes.AddAutoShape(ShapeType.Rectangle, chart_x_position + 95, chart_y_position + chart.Height - 10, chart_width - 95, 10, true);
                ChartTitle.AddTextFrame(" ");

                ChartTitle.TextFrame.Paragraphs[0].Portions[0].Text = objectivelist[cr] + " (" + CommonFunctions.CheckdecimalValue(samplesize) + ")";
                //ChartTitle.FillFormat.FillType = FillType.Solid;
                //ChartTitle.FillFormat.SolidFillColor.Color = Color.White;
                ChartTitle.ShapeStyle.LineColor.Color = Color.Transparent;
                ChartTitle.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;


                ChartTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                ChartTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;

                ChartTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
                ChartTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
                ChartTitle.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");

                //updated By sharath on 18-08-2015
                ChartTitle.FillFormat.FillType = FillType.Solid;
                ChartTitle.FillFormat.SolidFillColor.Color = Color.FromArgb(250, 250, 250);
                //               

                //Apply Chart Styles
                Chart_Styles(chart, profilerparams);
                //

                if (crindex == 0)
                {
                    crindex = 0;
                    chart_y_position += (float)222.48;
                    crindex = 0;
                }

                crindex += 1;
            }
            //PlotInputSelection
            PlotInputSelection(sld, profilerparams);
            // 
        }

        //added by sharath for chg Vs Py on 02-11-2015
        public void PyramidChngVsPYTable(ISlide sld, ProfilerChartParams profilerparams, DataSet _ds, List<string> _objectivelist)
        {
            ds = _ds;
            samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
            //samplesizeArray = CommonFunctions.LoadChartSampleSizeSize(ds);
            //objectivelist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"))).Distinct().ToList();           
            objectivelist = _objectivelist;
            var query2 = from r in ds.Tables[1].AsEnumerable() select r;
            ds.Tables.RemoveAt(1);
            ds.Tables.Add(query2.CopyToDataTable());

            metriclist = (from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("MetricItem"))).Distinct().Reverse().ToList();
            metriclist.Reverse();
            chart_x_position = 275;
            chart_y_position = (float)63.21;
            chart_width = 340;
            chart_height = (float)198.42;

            int crindex = 1;

            for (int cr = 0; cr < objectivelist.Count; cr++)
            {
                if (crindex == 1)
                {
                    chart_x_position = (float)378.14;
                }
                else if (crindex == 2)
                {
                    chart_x_position = (float)837.07;
                    crindex = 0;
                }


                List<double> dblCols = new List<double> { 80 };
                //List<double> dblRows = new List<double> { 10, 35, 35, 35, 35, 35 };
                //if (profilerparams.MetricShortName.ToUpper().StartsWith("RETAILER LOYALTY PYRAMID(BASE-COULD SHOP)(APPLICABLE ONLY FOR RETAILERS)"))
                //{
                //    dblRows = new List<double> { 10, 29, 28, 29, 29, 26, 33 };
                //}
                List<double> dblRows = new List<double> { 10 };
                for (int i = 0; i < metriclist.Count; i++)
                {
                    dblRows.Add(181.62 / metriclist.Count);
                }

                //Add table shape to slide
                ITable tbl = sld.Shapes.AddTable(chart_x_position, chart_y_position, dblCols.ToArray(), dblRows.ToArray());

                //Added by sharath on 14-08-2015 for changing the chart background color
                #region Fill the chart background
                tbl.FillFormat.FillType = FillType.Solid;
                tbl.FillFormat.SolidFillColor.Color = Color.FromArgb(250, 250, 250);
                #endregion

                foreach (IRow row in tbl.Rows)
                {
                    foreach (ICell cell in row)
                    {
                        cell.BorderTop.FillFormat.FillType = FillType.NoFill;
                        cell.BorderBottom.FillFormat.FillType = FillType.NoFill;
                        cell.BorderLeft.FillFormat.FillType = FillType.NoFill;
                        cell.BorderRight.FillFormat.FillType = FillType.NoFill;
                    }
                }
                if (profilerparams.SelectedStatTest.ToLower() == "previous year")
                    tbl[0, 0].TextFrame.Text = "Change Vs PY".ToUpper();
                else if (profilerparams.SelectedStatTest.ToLower() == "previous period")
                    tbl[0, 0].TextFrame.Text = "Change Vs PP".ToUpper();


                tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 8;
                tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");
                tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
                tbl[0, 0].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.True;
                tbl[0, 0].FillFormat.FillType = FillType.NoFill;
                tbl[0, 0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;



                for (int j = 0; j < metriclist.Count; j++)
                {
                    var SSCheck = ds.Tables[1].AsEnumerable().Where(x => x.Field<string>("Objective").ToLower() == objectivelist[cr].ToLower() && x.Field<string>("MetricItem").ToLower() == metriclist[j].ToLower()).Select(x => x.Field<object>("SampleSize")).FirstOrDefault();
                    var significance = ds.Tables[1].AsEnumerable().Where(x => x.Field<string>("Objective").ToLower() == objectivelist[cr].ToLower() && x.Field<string>("MetricItem").ToLower() == metriclist[j].ToLower()).Select(x => x.Field<object>("Significance")).FirstOrDefault();
                    var changePY = ds.Tables[1].AsEnumerable().Where(x => x.Field<string>("Objective").ToLower() == objectivelist[cr].ToLower() && x.Field<string>("MetricItem").ToLower() == metriclist[j].ToLower()).Select(x => x.Field<object>("ChangePY")).FirstOrDefault();

                    if (changePY != null && Convert.ToInt32(SSCheck) >= 30)
                    {
                        // changePY = Convert.ToDouble(changePY).ToString("0.0");
                        changePY = Math.Round(Convert.ToDouble(changePY), 1).ToString("0.0");

                        tbl[0, j + 1].TextFrame.Text = changePY.ToString();

                        tbl[0, j + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");
                        tbl[0, j + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                        tbl[0, j + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
                        tbl[0, j + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(Convert.ToString(significance), GetSampleSize(objectivelist[cr]));//ChangeColorOnSignificance(significance == null ? 0.0 : Convert.ToDouble(significance), SSCheck == null ? 0.0 : Convert.ToDouble(SSCheck));

                    }
                    else
                    {
                        tbl[0, j + 1].TextFrame.Text = GlobalVariables.NA;
                    }

                    tbl[0, j + 1].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 10;
                    tbl[0, j + 1].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;
                    tbl[0, j + 1].FillFormat.FillType = FillType.Solid;
                    tbl[0, j + 1].FillFormat.SolidFillColor.Color = Color.Transparent;
                }
                if (crindex == 0)
                {
                    crindex = 0;
                    chart_y_position = (float)285.73;
                    crindex = 0;
                }

                crindex += 1;
            }
        }
        //end
        private static void PlotChartTitle(ISlide sld, ProfilerChartParams profilerparams, IChart chart)
        {
            #region Base label
            chart.HasTitle = true;
            chart.ChartTitle.AddTextFrameForOverriding("");
            chart.ChartTitle.Overlay = false;

            List<string> ChartTitleList = new List<string>() { Convert.ToString(profilerparams.ChartDataSet.Tables[0].Rows[0]["Metric"]).ToUpper() };
            if (!string.IsNullOrEmpty(profilerparams.FilterShortNames))
            {                
                //var sFilterList = profilerparams.FilterShortNames.Split('|');
                //var sFilter = "";
                //for (var i = 0; i < sFilterList.Length; i++)
                //{
                //    if (i % 2 == 0)
                //        sFilter += sFilterList[i] + ":-";
                //    else
                //    {
                //        sFilter += sFilterList[i] + (sFilterList.Length - 1 == i ? string.Empty : ", ");
                //    }
                //}
                var sFilter = cf.GetSortedFilters(profilerparams.FilterShortNames);
                ChartTitleList.Add(sFilter.ToUpper());
            }
            //ChartTitleList.Add(profilerparams.FilterShortNames.ToUpper());

            StringBuilder ChartTitle = new StringBuilder();
            foreach (string title in ChartTitleList)
            {
                if (title.Trim() != "")
                {
                    ChartTitle.Append(title + "\r");
                }
            }
            ITextFrame textFrame = chart.ChartTitle.AddTextFrameForOverriding(Convert.ToString(ChartTitle).TrimEnd('\r'));

            int chartcount = 1;
            foreach (IParagraph Prg in textFrame.Paragraphs)
            {
                Prg.Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                Prg.Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;
                Prg.Portions[0].PortionFormat.LatinFont = new FontData("Arial (Body)");
                if (chartcount == 1)
                    Prg.Portions[0].PortionFormat.FontHeight = 12;
                else
                    Prg.Portions[0].PortionFormat.FontHeight = 8;
                chartcount++;
            }
            #endregion
        }

        public void PlotInputSelection(ISlide sld, ProfilerChartParams profilerparams)
        {
            #region Base label
            float xpos = 10;
            float ypos = 460;
            float getheight = 20;
            float getWidth = 700;

            IAutoShape StatLabel = sld.Shapes.AddAutoShape(ShapeType.Rectangle, (float)10.20, (float)481.61, (float)470.27, (float)5.1, true);

            IAutoShape SelectedStatTest = sld.Shapes.AddAutoShape(ShapeType.Rectangle, (float)555.8, (float)481.7, 350, (float)5.1, true);
            IAutoShape StatTestGreenValue = sld.Shapes.AddAutoShape(ShapeType.Rectangle, (float)580.3, (float)481.7, 350, (float)5.1, true);
            IAutoShape StatTestRedValue = sld.Shapes.AddAutoShape(ShapeType.Rectangle, (float)480.2, (float)481.7, (float)470.2, (float)5.1, true);

            StatLabel.AddTextFrame(" ");

            //added by Nagaraju for Where Purchased 27-05-2016
            string inputselection = "TIME PERIOD: " + (cf.cleanPPTXML(profilerparams.ShortTimePeriod)).ToUpper();

            //add shopper segment
            if (!string.IsNullOrEmpty(profilerparams.ShopperSegment))
            {
                string ShopperSegment = string.Empty;
                List<string> ShopperSegmentlist = profilerparams.ShopperSegment.Split('|').ToList();

                if (profilerparams.ShopperSegment.IndexOf("Channel") > -1 || profilerparams.ShopperSegment.IndexOf("Retailer") > -1)
                    ShopperSegment = "CHANNEL/RETAILER: ";
                else if (profilerparams.ShopperSegment.IndexOf("Category") > -1 || profilerparams.ShopperSegment.IndexOf("Brand") > -1)
                    ShopperSegment = "CATEGORY/BRAND: ";
                if (ShopperSegment != "")
                inputselection += "\n" + ShopperSegment + commonfunctions.Get_ShortNames(ShopperSegmentlist[ShopperSegmentlist.Count - 1]);
            }

            string ChannelRetailersVisited = string.Empty;
            if (Convert.ToString(profilerparams.ModuleBlock).Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase)
                || Convert.ToString(profilerparams.ModuleBlock).Equals("WithinBeverageTrips", StringComparison.OrdinalIgnoreCase))
            {
                List<string> ShopperFrequencylist = new List<string>();
                if (profilerparams.ShopperFrequency == "Total")
                {
                    ShopperFrequencylist.Add(profilerparams.ShopperFrequency);
                }
                else
                {
                    string[] cr = profilerparams.ShopperFrequency.Split(new String[] { "|", "|" },
                                   StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 1; i < cr.Length; i += 2)
                    {
                        ShopperFrequencylist.Add(commonfunctions.Get_ShortNames(cr[i]));
                    }
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
                if (!string.IsNullOrEmpty(ChannelRetailersVisited))
                    inputselection += "\nWHERE PURCHASED: " + ChannelRetailersVisited;
            }

            //StatLabel.TextFrame.Paragraphs[0].Portions[0].Text = "TIME PERIOD: " + (cf.cleanPPTXML(profilerparams.ShortTimePeriod)).ToUpper();
            StatLabel.TextFrame.Paragraphs[0].Portions[0].Text = inputselection.Trim().ToUpper();
            StatLabel.FillFormat.FillType = FillType.NoFill;

            StatLabel.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;
            StatLabel.ShapeStyle.LineColor.Color = Color.Transparent;


            StatLabel.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            StatLabel.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;

            StatLabel.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 6;
            StatLabel.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");

            //------------->Plot Selected Stat test label
            SelectedStatTest.AddTextFrame(" ");

            SelectedStatTest.TextFrame.Paragraphs[0].Portions[0].Text = ("Stat Testing Vs. " + UppercaseFirst(profilerparams.SelectedStatTest)).ToUpper();

            SelectedStatTest.FillFormat.FillType = FillType.NoFill;

            SelectedStatTest.ShapeStyle.LineColor.Color = Color.Transparent;

            SelectedStatTest.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;

            SelectedStatTest.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            SelectedStatTest.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Black;

            SelectedStatTest.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 6;
            SelectedStatTest.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");
            //------------>

            //------------->Plot Selected Stat test StatTestGreenValue 
            StatTestGreenValue.AddTextFrame(" ");

            StatTestGreenValue.TextFrame.Paragraphs[0].Portions[0].Text = ">" + Convert.ToString(profilerparams.StatTesting) + "%";

            StatTestGreenValue.FillFormat.FillType = FillType.NoFill;

            StatTestGreenValue.ShapeStyle.LineColor.Color = Color.Transparent;

            StatTestGreenValue.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;

            StatTestGreenValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            StatTestGreenValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Green;

            StatTestGreenValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 6;
            StatTestGreenValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");
            //------------>

            //------------->Plot Selected Stat test StatTestRedValue 
            StatTestRedValue.AddTextFrame(" ");

            StatTestRedValue.TextFrame.Paragraphs[0].Portions[0].Text = "<" + Convert.ToString(profilerparams.StatTesting) + "%";

            StatTestRedValue.FillFormat.FillType = FillType.NoFill;

            StatTestRedValue.ShapeStyle.LineColor.Color = Color.Transparent;

            StatTestRedValue.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;

            StatTestRedValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            StatTestRedValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.Red;

            StatTestRedValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 6;
            StatTestRedValue.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Arial Body");
            //------------>
            #endregion
        }

        private ChartType GetChartType(string chartType)
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
        private static void SetMarketStyle(IChartSeries Series, int serCount)
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
        public string UppercaseFirst(string s)
        {
            // Check for empty string.
            //if (string.IsNullOrEmpty(s))
            //{
            //    return string.Empty;
            //}
            //// Return char and concat substring.
            //return char.ToUpper(s[0]) + s.Substring(1);
            return s.ToUpper();
        }

        private static void Chart_Styles(IChart chart, ProfilerChartParams profilerparams)
        {
            #region chart axis
            chart.Axes.VerticalAxis.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            chart.Axes.VerticalAxis.TextFormat.PortionFormat.FontHeight = 8;

            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = 8;
            #endregion

            #region chart legend
            chart.Legend.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
            chart.Legend.TextFormat.PortionFormat.FontHeight = 8;
            #endregion

            chart.FillFormat.FillType = FillType.Solid;
            chart.FillFormat.SolidFillColor.Color = Color.FromArgb(250, 250, 250);

            #region Set chart Min and Max
            double dlMinval = (from r in profilerparams.ChartDataSet.Tables[1].AsEnumerable()
                               where !String.IsNullOrEmpty(Convert.ToString(r.Field<object>("Volume")))
                               orderby (r.Field<object>("Volume")) ascending
                               select Convert.ToDouble(r.Field<object>("Volume"))).Distinct().FirstOrDefault();

            double dlMaxval = (from r in profilerparams.ChartDataSet.Tables[1].AsEnumerable()
                               where !String.IsNullOrEmpty(Convert.ToString(r.Field<object>("Volume")))
                               orderby (r.Field<object>("Volume")) descending
                               select Convert.ToDouble(r.Field<object>("Volume"))).Distinct().FirstOrDefault();

            int nlMulval = 100;

            if (chart.Type == ChartType.Line)
            {

                dlMinval = dlMinval * nlMulval < 5 ? 0 : ((dlMinval * nlMulval) - ((dlMinval * nlMulval) % 5)) / nlMulval;
                dlMaxval = dlMaxval * nlMulval > 95 ? (1 / nlMulval) : ((dlMaxval * nlMulval) + (5 - (dlMaxval % 5))) / nlMulval;

                chart.Axes.VerticalAxis.MaxValue = dlMaxval;
                chart.Axes.VerticalAxis.MinValue = dlMinval;
                chart.Axes.VerticalAxis.NumberFormat = "0.0%";
            }
            else
            {
                chart.Axes.VerticalAxis.MinValue = 0;
                chart.Axes.VerticalAxis.MaxValue = 1;
            }



            #endregion
        }

        //Get Series Colour
        private System.Drawing.Color GetSerirsColour(int SeriesIndex)
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

        private System.Drawing.Color GetSignificanceColor(string significancevalue, double samplesize)
        {
            System.Drawing.Color color = System.Drawing.Color.Black;
            if (!string.IsNullOrEmpty(significancevalue))
            {
                if (Convert.ToDouble(significancevalue) == 1000)
                    color = System.Drawing.Color.Blue;
                else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    color = System.Drawing.Color.Green;
                else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    color = System.Drawing.Color.Red;
                else if (samplesize >= GlobalVariables.LowSample && samplesize < 100)
                    color = System.Drawing.Color.Gray;
                else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                    color = System.Drawing.Color.Black;
            }
            return color;
        }       

        private void LabelFontSize(IDataLabel lbl)
        {
            lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = 8;
            lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Arial (Body)");
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

        private DataSet FormatDataSet(DataSet _ds)
        {
            foreach (System.Data.DataTable tbl in _ds.Tables)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    row["Objective"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["Objective"]))).ToUpper();
                    row["Metric"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["Metric"]))).ToUpper();
                    row["MetricItem"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["MetricItem"]))).ToUpper();
                }
            }
            return _ds;
        }
        private double GetSampleSize(string key)
        {
            double samplesize = 0.0;
            if (samplesizelist.ContainsKey(key))
            {
                samplesize = samplesizelist[key];
            }
            return samplesize;
        }
        private double GetSampleSizeNew(string key, int index)
        {
            double samplesize = 0.0;
            if (samplesizeArray[index].ContainsKey(key))
            {
                samplesize = samplesizeArray[index][key];
            }
            return samplesize;
        }
        private bool IsSampleSizeless(string key)
        {
            double samplesize = 0.0;
            if (samplesizelist.ContainsKey(key))
            {
                samplesize = samplesizelist[key];
                if (Convert.ToDouble(samplesize) < GlobalVariables.LowSample)
                    return true;
            }
            else
            {
                return true;
            }
            return false;
        }
        private bool IsSampleSizelessNew(string key, int index)
        {
            double samplesize = 0.0;
            if (samplesizeArray[index].ContainsKey(key))
            {
                samplesize = samplesizeArray[index][key];
                if (Convert.ToDouble(samplesize) < GlobalVariables.LowSample)
                    return true;
            }
            else
            {
                return true;
            }
            return false;
        }
    }
}