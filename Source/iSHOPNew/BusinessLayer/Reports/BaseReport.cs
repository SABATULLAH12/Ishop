using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Slides;
using System.Data;
using Aspose.Slides.Charts;
using iSHOPNew.DAL;
using System.Drawing;
using iSHOP.BLL;
using iSHOPNew.Models;
using System.IO;
using Newtonsoft.Json;


namespace iSHOPNew.Reports.BusinessLayer
{
    public abstract class BaseReport
    {
        public Presentation pres = null;
        public ISlideCollection slds = null;
        public int SlideNumber = 0;
        IChart chart = null;
        IChartSeries Series = null;
        IDataLabel lbl = null;
        int serCount = 1, catcount = 1;
        public bool isTrend = false;
        public List<string> objectivelist = null;
        public List<string> metrics = null;
        public List<string> metriclist = null;
        public DataSet ds = null;
        public System.Data.DataTable chart_table = null;
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public double StatTesting;
        public string benchmark = string.Empty;
        public string shopperFrequency = string.Empty;
        public string readAsText = string.Empty;
        private static Random rnd = new Random();
        public string filename = string.Empty;
        private ITable aspose_tbl = null;
        private List<double> aspose_tbl_Cols = null;
        private List<double> aspose_tbl_Rows = null;
        public List<string> table_metrics = null;
        private double table_width = 744.7;
        private double table_column_width = 0.0;
        public string readAsText_4_5th_Slide = string.Empty;

        #region abstract functions
        public abstract void PrepareReport(ReportGeneratorParams param);
        #endregion

        public BaseReport()
        {
            StatTesting = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);
            accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
            accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
        }
        #region load template
        public void LoadSlides(string filePath)
        {
            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            pres = new Presentation(filePath);
            slds = pres.Slides;
        }
        public void SaveFile()
        {
            filename = "iSHOP_ReportGenerator_" + GlobalVariables.GetRandomNumber;
            pres.Save(HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);
            HttpContext.Current.Session[SessionVariables.BeveragePPT] = HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx");
        }
        #endregion
        #region get slide chart table
        public System.Data.DataTable Get_Chart_Table(DataSet ds, int slideNumber = 0, int tableNumber = 0)
        {
            System.Data.DataTable tbl = null;
            foreach (System.Data.DataTable tb in ds.Tables)
            {
                if (tb.Rows.Count > 0 && Convert.ToInt32(tb.Rows[0]["SlideNumber"]) == slideNumber && Convert.ToInt32(tb.Rows[0]["TableNumber"]) == tableNumber)
                {
                    tbl = tb;
                    break;
                }
            }
            return tbl;
        }
        #endregion
        #region replace clustered column chart
        public void ReplaceClusteredColumnChart(ISlide sld, System.Data.DataTable tbl, String chart_shape_name)
        {
            chart = (IChart)sld.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();
            chart.ChartTitle.Overlay = false;
            chart.HasTitle = false;
            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = GetAxisMaxValue(tbl);
            chart.Axes.VerticalAxis.MinValue = 0;

            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;
            chart.HasLegend = false;
            int defaultWorksheetIndex = 0;

            chart.Axes.VerticalAxis.TickLabelRotationAngle = 0;
            chart.Axes.HorizontalAxis.TickLabelRotationAngle = 0;
            chart.Axes.HorizontalAxis.IsAutomaticTickLabelSpacing = false;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;          

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();
            chart.ChartData.ChartDataWorkbook.Clear(0);

            //add chart title in work sheet
            fact.GetCell(0, 49, 0, "title");
            fact.GetCell(0, 49, 1, Convert.ToString(tbl.Rows[0]["Metric"]));

            int s = chart.ChartData.Series.Count;
            objectivelist = (from r in tbl.AsEnumerable() select Convert.ToString(r["Objective"])).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r["MetricItem"])).Distinct().ToList();

            for (int i = 1; i < objectivelist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);
                chart.ChartData.Series[i - 1].ParentSeriesGroup.Overlap = -20;
            }
            //Adding new categories
            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
            }

            serCount = 1;
            catcount = 1;

            foreach (string _objective in objectivelist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.Format.Fill.FillType = FillType.Solid;
                Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Format.Fill.FillType = FillType.Gradient;
                Series.Format.Fill.GradientFormat.GradientShape = GradientShape.Linear;
                Series.Format.Fill.GradientFormat.GradientDirection = GradientDirection.FromCenter;
                Series.Format.Fill.GradientFormat.LinearGradientAngle = 90;

                Color _Stop2Col = GetSerirsColour(serCount - 1);
                Color _Stop1Col = Colorluminance(GetSerirsColour(serCount - 1), 0.25f);
                Series.Format.Fill.GradientFormat.GradientStops.Add(0, _Stop1Col);
                Series.Format.Fill.GradientFormat.GradientStops.Add(1, _Stop2Col);

                Series.Format.Effect.EnableOuterShadowEffect();
                Series.Format.Effect.OuterShadowEffect.BlurRadius = 2;
                Series.Format.Effect.OuterShadowEffect.Direction = 90;
                Series.Format.Effect.OuterShadowEffect.Distance = 1.5;
                Series.Format.Effect.OuterShadowEffect.ShadowColor.Color = Color.Black;
                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(double.Parse(Convert.ToString(r["SampleSize"]))) ? "0" : Convert.ToString(r["Volume"]),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                     sampleSize = double.Parse(Convert.ToString(r["SampleSize"])),
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? (Convert.ToDouble(query.value) / 100) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, query.sampleSize, _objective);
                    LabelFontSize(lbl);
                    catcount++;
                }
                catcount = 1;
                serCount++;
            }
        }
        #endregion
        #region replace Trend line chart
        public void ReplaceTrendLineChart(ISlide sld, System.Data.DataTable tbl, String chart_shape_name)
        {
            chart = (IChart)sld.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();
            chart.ChartTitle.Overlay = false;
            chart.HasTitle = false;
            chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = false;
            chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;

            chart.Axes.VerticalAxis.IsVisible = false;

            chart.Axes.VerticalAxis.IsAutomaticMajorUnit = false;
            chart.Axes.VerticalAxis.IsAutomaticMinorUnit = false;

            chart.Axes.VerticalAxis.IsAutomaticMaxValue = false;
            chart.Axes.VerticalAxis.IsAutomaticMinValue = false;

            chart.Axes.VerticalAxis.MaxValue = GetAxisMaxValue(tbl);
            chart.Axes.VerticalAxis.MinValue = 0;

            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = false;
            chart.HasLegend = false;
            int defaultWorksheetIndex = 0;

            chart.Axes.VerticalAxis.TickLabelRotationAngle = 0;
            chart.Axes.HorizontalAxis.TickLabelRotationAngle = 0;
            chart.Axes.HorizontalAxis.IsAutomaticTickLabelSpacing = false;

            chart.HasLegend = true;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();
            chart.ChartData.ChartDataWorkbook.Clear(0);

            //add chart title in work sheet
            fact.GetCell(0, 49, 0, "title");
            fact.GetCell(0, 49, 1, Convert.ToString(tbl.Rows[0]["Metric"]));

            int s = chart.ChartData.Series.Count;
            objectivelist = (from r in tbl.AsEnumerable() select Convert.ToString(r["Objective"])).Distinct().ToList();
            metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r["MetricItem"])).Distinct().ToList();

            for (int i = 1; i < metriclist.Count + 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, metriclist[i - 1]), chart.Type);
                //chart.ChartData.Series[i - 1].ParentSeriesGroup.Overlap = -20;
            }
            //Adding new categories
            for (int i = 0; i < objectivelist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, objectivelist[i]));
            }

            serCount = 1;
            catcount = 1;

            foreach (string _metric in metriclist)
            {
                Series = chart.ChartData.Series[serCount - 1];
                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;


                Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                Series.ParentSeriesGroup.Overlap = 100;

                Series.Format.Fill.FillType = FillType.Solid;
                Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);

                Series.Marker.Symbol = MarkerStyleType.Circle;
                Series.Marker.Format.Fill.FillType = FillType.Solid;
                Series.Marker.Format.Fill.SolidFillColor.Color = GetSerirsColour(serCount - 1);
                Series.Marker.Size = 7;


                Series.Format.Line.DashStyle = LineDashStyle.Dot;
                Series.Format.Line.CapStyle = LineCapStyle.Round;
                Series.Format.Line.FillFormat.FillType = FillType.Solid;
                Series.Format.Line.FillFormat.SolidFillColor.Color = GetSerirsColour(serCount - 1);
                Series.Format.Line.Width = 2.25;

                foreach (string series in objectivelist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("MetricItem")).Equals(_metric, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("Objective")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(double.Parse(Convert.ToString(r["SampleSize"]))) ? "0" : Convert.ToString(r["Volume"]),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                     sampleSize = double.Parse(Convert.ToString(r["SampleSize"])),
                                     objective = Convert.ToString(r.Field<object>("Objective")),
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? (Convert.ToDouble(query.value) / 100) : 0)));
                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";


                    //Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    //lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = _data_label_fontheight;
                    //lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = fontfamily;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.Top;
                    lbl.DataLabelFormat.ShowValue = true;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, query.sampleSize, query.objective);
                    catcount++;
                }

                catcount = 1;
                serCount++;
            }
        }
        #endregion
        #region update table
        public void Update_Table(ISlide sld, System.Data.DataTable tbl, String chart_shape_name, int current_slide_no)
        {
            List<string> metriclist = (from r in chart_table.AsEnumerable() select Convert.ToString(r["Metric"])).Distinct().ToList();
            if (metriclist != null && metriclist.Count > 0)
            {
                table_metrics = new List<string>();
                int slide_no = current_slide_no;
                if (metriclist.Count > 2)
                {
                    for (int j = 0; j < metriclist.Count; j++)
                    {
                        table_metrics.Add(metriclist[j]);
                        if (j == 1)
                        {
                            var query = (from r in chart_table.AsEnumerable()
                                         where table_metrics.Contains(Convert.ToString(r["Metric"]))
                                         select r);
                            Replace_Table(slds[slide_no], query.CopyToDataTable(), chart_shape_name);
                            table_metrics = new List<string>();
                            slide_no++;
                        }
                        else if (j == metriclist.Count - 1)
                        {
                            var query = (from r in chart_table.AsEnumerable()
                                         where table_metrics.Contains(Convert.ToString(r["Metric"]))
                                         select r);
                            Replace_Table(slds[slide_no], query.CopyToDataTable(), chart_shape_name);
                            table_metrics = new List<string>();
                            slide_no++;
                        }
                    }
                }
                else
                {
                    Replace_Table(sld, chart_table, chart_shape_name);
                }
            }
        }
        #endregion
        public void Replace_Table(ISlide sld, System.Data.DataTable tbl, String chart_shape_name)
        {
            try
            {
                //Add table shape to slide          
                aspose_tbl = (ITable)sld.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();
                objectivelist = (from r in tbl.AsEnumerable() select Convert.ToString(r["Objective"])).Distinct().ToList();
                metrics = (from r in tbl.AsEnumerable() select Convert.ToString(r["Metric"])).Distinct().ToList();
                metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r["MetricItem"])).Distinct().ToList();
                //remove extra columns
                for (int i = aspose_tbl.Columns.Count - 1; i > (((objectivelist.Count + objectivelist.Count) - 2) + 2); i--)
                {
                    aspose_tbl.Columns.RemoveAt(i, true);
                }
                //remove default rows
                for (int i = aspose_tbl.Rows.Count - 1; i > 3; i--)
                {
                    aspose_tbl.Rows.RemoveAt(i, true);
                }
                //set column width
                double margin_width = 11.65;
                table_column_width = (table_width - (68 + ((objectivelist.Count - 1) * margin_width))) / objectivelist.Count;
                int j = 0;
                double metric_column_width= 205;
                aspose_tbl.Columns[0].Width = metric_column_width;
                aspose_tbl.Columns[1].Width = margin_width;
                for (int i = 2; i < aspose_tbl.Columns.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        aspose_tbl.Columns[i].Width = table_column_width;
                        aspose_tbl[i, 0].TextFrame.Text = objectivelist[j];
                        j++;
                    }
                    else
                    {
                        aspose_tbl.Columns[i].Width = margin_width;
                    }
                }

                IPictureFrame seperator_1 = (IPictureFrame)sld.Shapes.Where(x => x.Name == "seperator_1").FirstOrDefault();
                IPictureFrame seperator_2 = (IPictureFrame)sld.Shapes.Where(x => x.Name == "seperator_2").FirstOrDefault();
                float sep_X = (float) 235.03;
                float col_sep = 0;
                seperator_1.X = sep_X;
                col_sep = sep_X + (float)table_column_width + (float)margin_width;
                seperator_2.X = col_sep;
                for (int i = 3; i <= 6; i++)
                {
                    IPictureFrame seperator = (IPictureFrame)sld.Shapes.Where(x => x.Name == "seperator_" + i.ToString()).FirstOrDefault();
                    if(seperator != null)
                    {
                        if(i > objectivelist.Count)                        
                            seperator.Hidden = true;                        
                        else
                        {
                            col_sep += (float)margin_width + (float)table_column_width;
                            seperator.X = col_sep;
                        }
                    }
                }        

                int row_indx = 2;
                int col_indx = 0;
                for (int i = 0; i < metrics.Count; i++)
                {
                    col_indx = 0;
                    //add measure
                    if (row_indx > 2)
                        aspose_tbl.Rows.AddClone(aspose_tbl.Rows[2], true);

                    aspose_tbl[col_indx, row_indx].TextFrame.Text = metrics[i];
                    row_indx++;
                    var metricitems = (from r in tbl.AsEnumerable()
                                       where Convert.ToString(r["Metric"]).Equals(metrics[i], StringComparison.OrdinalIgnoreCase)
                                       select Convert.ToString(r["MetricItem"])).Distinct().ToList();

                    foreach (string metricitem in metricitems)
                    {
                        if (row_indx > 3)
                            aspose_tbl.Rows.AddClone(aspose_tbl.Rows[3], false);

                        aspose_tbl[col_indx, row_indx].TextFrame.Text = metricitem;

                        aspose_tbl[col_indx, row_indx].BorderTop.FillFormat.FillType = FillType.NoFill;
                        aspose_tbl[col_indx, row_indx].BorderBottom.FillFormat.FillType = FillType.Solid;
                        aspose_tbl[col_indx, row_indx].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                        aspose_tbl[col_indx, row_indx].BorderBottom.Width = 0.8;
                        aspose_tbl[col_indx, row_indx].BorderBottom.DashStyle = LineDashStyle.Dot;
                        col_indx += 2;
                        foreach (string obj in objectivelist)
                        {
                            var query = (from r in tbl.AsEnumerable()
                                         where Convert.ToString(r["Metric"]).Equals(metrics[i], StringComparison.OrdinalIgnoreCase)
                                         && Convert.ToString(r["MetricItem"]).Equals(metricitem, StringComparison.OrdinalIgnoreCase)
                                         && Convert.ToString(r["Objective"]).Equals(obj, StringComparison.OrdinalIgnoreCase)
                                         select new
                                         {
                                             metric_item = Convert.ToString(r["MetricItem"]),
                                             volume = string.IsNullOrEmpty(Convert.ToString(r["Volume"])) ? 0 : Convert.ToDouble(r["Volume"]) / 100,
                                             significance = string.IsNullOrEmpty(Convert.ToString(r["Significance"])) ? 0 : Convert.ToDouble(r["Significance"]),
                                             sampleSize = string.IsNullOrEmpty(Convert.ToString(r["SampleSize"])) ? 0 : Convert.ToDouble(r["SampleSize"]),
                                         }).Distinct().FirstOrDefault();
                            aspose_tbl[col_indx, row_indx].TextFrame.Text = Convert.ToDouble(query.volume).ToString("0%");

                            aspose_tbl[col_indx, row_indx].BorderTop.FillFormat.FillType = FillType.NoFill;
                            aspose_tbl[col_indx, row_indx].BorderBottom.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[col_indx, row_indx].BorderBottom.FillFormat.SolidFillColor.Color = Color.Black;
                            aspose_tbl[col_indx, row_indx].BorderBottom.Width = 0.8;
                            aspose_tbl[col_indx, row_indx].BorderBottom.DashStyle = LineDashStyle.Dot;

                            aspose_tbl[col_indx, row_indx].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                            aspose_tbl[col_indx, row_indx].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(Convert.ToString(query.significance), query.sampleSize, obj);
                            col_indx += 2;
                        }
                        col_indx = 0;
                        row_indx++;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }

        string getText(string value, string chartType)
        {
            double newValue;
            double.TryParse(value, out newValue);
            if (chartType.Equals("Another_Store_Considered") || chartType.Equals("Immediate_Consumption") || chartType.Equals("Overall_Satisfaction"))
            {
                return Math.Round(newValue).ToString() + "%";
            }
            else if (chartType.Equals("Average_Expenditure"))
            {

                return "$" + Math.Round(newValue).ToString();
            }
            else if (chartType.Equals("Average_NumOf_Items"))
            {
                return Math.Round(newValue).ToString();
            }
            else
            {
                int hours = (int)(newValue / 60);
                return hours.ToString().PadLeft(2, '0') + ":" + Math.Round(newValue - (hours * 60)).ToString().PadLeft(2, '0');
            }
        }

        #region SampleSize Footer Update

        public void SampleSizeFooterTableUpdate(ISlide sld, System.Data.DataTable tbl, string tableName, bool IsUpperCase = false)
        {
            IShape _shp = sld.Shapes.Where(x => x.Name == tableName).FirstOrDefault();
            if (_shp != null)
            {
                Aspose.Slides.ITable footerTable = (Aspose.Slides.ITable)_shp;
                var objectiveList = (from r in tbl.AsEnumerable() select Convert.ToString(r["Objective"])).Distinct().ToList();
                var count = 0;

                foreach (var objective in objectiveList)
                {
                    count = count + 2;
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(objective, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     sampleSize = double.Parse(Convert.ToString(r["SampleSize"])),
                                 }).FirstOrDefault();

                    footerTable[count, 0].TextFrame.Text = (IsUpperCase ? objective.ToUpper() : objective) + "\r(" + CommaSeparatedValues(query.sampleSize.ToString()) + ")";
                }

                for (var i = count + 1; i < footerTable.Columns.Count;)
                {
                    footerTable.Columns.RemoveAt(i, false);
                }
            }
        }

        #endregion

        #region Trend SampleSize Footer Text

        public void SampleSizeFooterTextForTrend(ISlide sld,System.Data.DataTable tbl, string textBoxName)
        {
            string sampleSizeText = string.Empty;
            sampleSizeText += "Sample Size :";
            var objectiveList = (from r in tbl.AsEnumerable() select Convert.ToString(r["Objective"])).Distinct().ToList();

            foreach (var objective in objectiveList)
            {
                var query = (from r in tbl.AsEnumerable()
                             where Convert.ToString(r.Field<object>("Objective")).Equals(objective, StringComparison.OrdinalIgnoreCase)
                             select new
                             {
                                 sampleSize = double.Parse(Convert.ToString(r["SampleSize"])),
                             }).FirstOrDefault();

                sampleSizeText += " " + objective + " (" + CommaSeparatedValues(query.sampleSize.ToString()) + "),";
            }
            IAutoShape _shp = (IAutoShape)sld.Shapes.Where(x => x.Name == textBoxName).FirstOrDefault();
            if (_shp != null)
            {
                _shp.TextFrame.Text= sampleSizeText.Substring(0, sampleSizeText.Length - 1);
            }
        }

        #endregion

        #region replace Header and DescriptionText text
        public void ReplaceHeaderAndDescriptionText(ISlide sld, System.Data.DataTable tbl, String chart_shape_name, bool headerUpdate)
        {
            IGroupShape group = (IGroupShape)sld.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();
            IAutoShape tmp;
            double volume;

            if (headerUpdate)
            {
                tmp = (IAutoShape)group.Shapes.Where(x => x.Name == "Header").FirstOrDefault();
                tmp.TextFrame.Text = tmp.TextFrame.Text.Replace("????", tbl.Rows[0]["Objective"].ToString()).Replace("||", ToPascalCase(shopperFrequency)).Replace("_objective", tbl.Rows[0]["Objective"].ToString());
            }
            tmp = (IAutoShape)group.Shapes.Where(x => x.Name == "Description").FirstOrDefault();
            double.TryParse(tbl.Rows[0]["Volume"].ToString(), out volume);
            if (isTrend)
            {
                tmp.TextFrame.Text = tmp.TextFrame.Text.Replace("_metricItem", tbl.Rows[0]["ReadAsMetricItem"].ToString()).Replace("_objective", tbl.Rows[0]["Objective"].ToString()).Replace("_percentage", Math.Round(volume).ToString()).Replace("_retailer", tbl.Rows[0]["Retailer"].ToString()).Replace("_frequency", ToPascalCase(shopperFrequency));
            }
            else if (string.IsNullOrEmpty(readAsText))
            {
                tmp.TextFrame.Text = tmp.TextFrame.Text.Replace("???", tbl.Rows[0]["ReadAsMetricItem"].ToString()).Replace("??", tbl.Rows[0]["Retailer"].ToString()).Replace("?", Math.Round(volume).ToString()).Replace("||", ToPascalCase(shopperFrequency));
            }
            if(true)
            {
                if (string.IsNullOrEmpty(readAsText_4_5th_Slide))
                {
                    string objective = readAsText.Replace("_objective", tbl.Rows[0]["Objective"].ToString());
                    tmp.TextFrame.Text = tmp.TextFrame.Text.Replace("_metricItem", tbl.Rows[0]["ReadAsMetricItem"].ToString()).Replace("_objective", objective).Replace("_percentage", Math.Round(volume).ToString()).Replace("_retailer", tbl.Rows[0]["Retailer"].ToString()).Replace("_frequency", ToPascalCase(shopperFrequency));
                }
                else
                {
                    tmp.TextFrame.Text = readAsText_4_5th_Slide.Replace("_metricItem", tbl.Rows[0]["ReadAsMetricItem"].ToString()).Replace("_objective", tbl.Rows[0]["Objective"].ToString()).Replace("_percentage", Math.Round(volume).ToString()).Replace("_retailer", tbl.Rows[0]["Retailer"].ToString()).Replace("_frequency", ToPascalCase(shopperFrequency));
                }
            }            
        }
        #endregion

        #region replace rectangle text
        public void ReplaceRectangleText(ISlide sld, System.Data.DataTable tbl, String chart_shape_name)
        {
            string significance, volume;
            double sampleSize;
            var itemsCount = 5;

            float minX = sld.Shapes.Where(x => x.Name == chart_shape_name + tbl.Rows.Count.ToString()).FirstOrDefault().X;
            float maxX = sld.Shapes.Where(x => x.Name == chart_shape_name + itemsCount.ToString()).FirstOrDefault().X;
            for (var i = 1; i <= tbl.Rows.Count; i++)
            {
                var row = tbl.Rows[i - 1];
                IGroupShape group = (IGroupShape)sld.Shapes.Where(x => x.Name == chart_shape_name + i.ToString()).FirstOrDefault();
                IAutoShape tmp = (IAutoShape)group.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();

                significance = Convert.ToString(row["Significance"]);
                volume = Convert.ToString(row["Volume"]);
                sampleSize = double.Parse(Convert.ToString(row["SampleSize"]));

                tmp.TextFrame.Text = getText(volume, chart_shape_name);
                tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(significance, sampleSize, row["Objective"].ToString());
                tmp.TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

                group.X += i * (maxX - minX - (group.Width * (((float)(5 / 4)) * (itemsCount - tbl.Rows.Count) / (tbl.Rows.Count)))) / tbl.Rows.Count;
            }
            for (var i = tbl.Rows.Count + 1; i <= 5; i++)
            {
                sld.Shapes.Remove((IGroupShape)sld.Shapes.Where(x => x.Name == chart_shape_name + i.ToString()).FirstOrDefault());
            }
        }
        #endregion

        #region replace DoughtNut Chart
        public void ReplaceDoughtNutChart(ISlide sld, System.Data.DataTable tbl, String chart_shape_name)
        {
            ReplaceRectangleText(sld, tbl, chart_shape_name);
            double volume = 0;
            for (var i = 1; i <= tbl.Rows.Count; i++)
            {
                volume = 0;
                IGroupShape group = (IGroupShape)sld.Shapes.Where(x => x.Name == chart_shape_name + i.ToString()).FirstOrDefault();
                chart = (IChart)group.Shapes.Where(x => x.Name == "donut_chart").FirstOrDefault();
                chart.ChartTitle.Overlay = false;
                chart.HasTitle = false;

                int defaultWorksheetIndex = 0;

                //Getting the chart data worksheet
                IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

                double.TryParse(tbl.Rows[i - 1]["Volume"].ToString(), out volume);

                fact.GetCell(defaultWorksheetIndex, 0, 1, "New_Series1");//modifying series name
                //Updating Series Data
                chart.ChartData.Series[0].DataPoints[0].Value.Data = 1;
                chart.ChartData.Series[0].DataPoints[1].Value.Data = volume / 100;
                chart.ChartData.Series[0].DataPoints[2].Value.Data = 1 - (volume / 100);
            }
        }
        #endregion

        #region pyramid chart
        public void UpdatePyramidSeriesData(ISlide sld, System.Data.DataTable tbl, String chart_shape_name)
        {
            int defaultIndex = 0;
            List<string> ser = tbl.AsEnumerable().Select(x => x.Field<string>("Objective")).Distinct().ToList();
            chart = (IChart)sld.Shapes.Where(x => x.Name == chart_shape_name).FirstOrDefault();
            int series_count = chart.ChartData.Series.Count;
            int noOfObjects = (series_count + 1) / 4;
            if (ser.Count < noOfObjects)
            {
                //Remove other Pyramids
                int countToDel = 3 + 4 * (4 - ser.Count);
                for (int i = (series_count-1); i >= (series_count - countToDel); i--)
                {
                    chart.ChartData.Series.RemoveAt(i);
                }
            }
            //Set length of Vetical Axis
            chart.Axes.HorizontalAxis.IsAutomaticMaxValue = false;
            chart.Axes.HorizontalAxis.IsAutomaticMinValue = false;
            chart.Axes.HorizontalAxis.MinValue = 0;
            chart.Axes.HorizontalAxis.MaxValue = ser.Count + (ser.Count - 1) * 0.05;
            chart.Axes.HorizontalAxis.NumberFormat = @"##0%";
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;
            //chart.ChartData.ChartDataWorkbook.Clear(0);
            //Add Series

            List<string> xCol = tbl.AsEnumerable().Select(x => x.Field<string>("MetricItem")).Distinct().ToList();
            //List<string> metricvalue = dt.AsEnumerable().Select(x => x.Field<string>("MetricValue")).ToList();
            int series_ind = 1, dp_index = 1;
            int pyramid_ind = 0;
            foreach (var item in ser)
            {
                fact.GetCell(defaultIndex, 0, 1 + dp_index, item);
                series_ind = 1;

                //Update the color
                chart.ChartData.Series[dp_index].Format.Fill.SolidFillColor.Color = GetSerirsColour(pyramid_ind);
                chart.ChartData.Series[dp_index].Format.Fill.FillType = FillType.Gradient;
                chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientShape = GradientShape.Linear;
                chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientDirection = GradientDirection.FromCenter;
                chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.LinearGradientAngle = 90;
                Color _Stop2Col = GetSerirsColour(pyramid_ind);
                Color _Stop1Col = Colorluminance(GetSerirsColour(pyramid_ind), 0.25f);
                if (chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientStops != null &&
                    chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientStops.Count > 0)
                {
                    chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientStops.RemoveAt(0);
                    chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientStops.RemoveAt(0);
                }
                chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientStops.Add(0, _Stop1Col);
                chart.ChartData.Series[dp_index].Format.Fill.GradientFormat.GradientStops.Add(1, _Stop2Col);
                chart.ChartData.Series[dp_index].Format.Effect.EnableOuterShadowEffect();
                chart.ChartData.Series[dp_index].Format.Effect.OuterShadowEffect.BlurRadius = 2;
                chart.ChartData.Series[dp_index].Format.Effect.OuterShadowEffect.Direction = 90;
                chart.ChartData.Series[dp_index].Format.Effect.OuterShadowEffect.Distance = 1.5;
                chart.ChartData.Series[dp_index].Format.Effect.OuterShadowEffect.ShadowColor.Color = Color.White;

                foreach (var x in xCol)
                {
                    if (dp_index == 1)
                    {
                        fact.GetCell(defaultIndex, series_ind, 0, x);
                    }
                    var val = tbl.AsEnumerable().Where(y => y.Field<string>("Objective") == item && y.Field<string>("MetricItem") == x).FirstOrDefault();
                    double mv = val["Volume"] == DBNull.Value ? 0 : Convert.ToDouble(val["Volume"]);

                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(item, StringComparison.OrdinalIgnoreCase)
                                 && Convert.ToString(r.Field<object>("MetricItem")).Equals(x, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = IsSampleSizeless(double.Parse(Convert.ToString(r["SampleSize"]))) ? "0" : Convert.ToString(r["Volume"]),
                                     significance = Convert.ToString(r.Field<object>("Significance")),
                                     sampleSize = double.Parse(Convert.ToString(r["SampleSize"])),
                                 }).FirstOrDefault();
                    double value = Convert.ToDouble(query.value) / 100;
                    fact.GetCell(defaultIndex, series_ind, 1 + dp_index, value);
                    fact.GetCell(defaultIndex, series_ind, dp_index, (1 - value) / 2);
                    fact.GetCell(defaultIndex, series_ind, 2 + dp_index, (1 - value) / 2);
                    //Set the labels
                    chart.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.NumberFormat = "#0%";
                    chart.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    chart.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(query.significance, query.sampleSize, item);
                    chart.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.Format.Fill.FillType = FillType.Solid;
                    chart.ChartData.Series[dp_index].DataPoints[series_ind - 1].Label.DataLabelFormat.Format.Fill.SolidFillColor.Color = Color.White;
                    series_ind++;
                }
                dp_index = dp_index + 4;
                pyramid_ind++;
            }
        }
        #endregion

        #region get axis max value
        private double GetAxisMaxValue(System.Data.DataTable tbl)
        {
            double maxval = 0.0;
            if (tbl != null && tbl.Rows.Count > 0)
            {
                var query = (from row in tbl.AsEnumerable()
                             where !string.IsNullOrEmpty(Convert.ToString(row["Volume"]))
                             orderby row["Volume"] descending
                             select Convert.ToString(row["Volume"])).FirstOrDefault();
                if (!string.IsNullOrEmpty(query))
                    maxval = (Convert.ToDouble(query) / 100) + 0.05;
            }
            return maxval;
        }
        #endregion

        #region ReadAsText For PIT

        public void GetReadASTextForPIT(ReportGeneratorParams param,string fileName)
        {
            readAsText = string.Empty;
            var jsonText = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~\Templates\Reports\" + fileName));
            var readAsTextData = JsonConvert.DeserializeObject<List<TabLevelReadAsText>>(jsonText);

            if (param.Comparison_DBNames[0].Split('|')[1].Contains("(Monthly Purchaser)"))
            {
                readAsText = "Of Shoppers Who Purchased _objective At Least Once A Month And Are";
            }
            else if (param.Comparison_DBNames[0].Split('|')[1].Contains("(Favorite Brand)"))
            {
                readAsText = "Of Shoppers Who Considered _objective As Their Favorite Brand And Are";
            }
            else if (param.CustomBase_ShortName == "Total" && param.Sigtype_UniqueId == "1")
            {
                readAsText = "By _objective";
            }
            else if (param.Sigtype_UniqueId != "1" && param.Comparison_DBNames[0].Split('|')[1] == "Total")
            {
                readAsText = "By _objective";
            }

            for (var i = 0; i < readAsTextData.Count && string.IsNullOrEmpty(readAsText); i++)
            {
                for (var j = 0; j < readAsTextData[i].TabLevelData.Count && string.IsNullOrEmpty(readAsText); j++)
                {
                    if (readAsTextData[i].TabLevelData[j].Metric == param.Comparison_DBNames[0].Split('|')[0])
                    {
                        readAsText = readAsTextData[i].TabLevelData[j].Text;
                    }
                }
            }
        }

        #endregion
        public static Color Colorluminance(Color color, float factor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (factor < 0)
            {
                factor = 1 + factor;
                red *= factor;
                green *= factor;
                blue *= factor;
            }
            else
            {
                red = (255 - red) * factor + red;
                green = (255 - green) * factor + green;
                blue = (255 - blue) * factor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
        private void LabelFontSize(IDataLabel lbl)
        {
            lbl.DataLabelFormat.TextFormat.PortionFormat.FontHeight = (float)8.5;
            lbl.DataLabelFormat.TextFormat.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
            lbl.DataLabelFormat.TextFormat.TextBlockFormat.RotationAngle = 270;
            lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.False;
        }
        private bool IsSampleSizeless(double? value)
        {
            if (value != null)
            {
                if (value < GlobalVariables.LowSample)
                    return true;
            }
            return false;
        }
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
        private System.Drawing.Color GetSignificanceColor(string significancevalue, double? samplesize, string objective)
        {
            System.Drawing.Color color = System.Drawing.Color.Black;
            if (!string.IsNullOrEmpty(significancevalue))
            {
                if ( !string.IsNullOrEmpty(benchmark) && benchmark.Equals(objective, StringComparison.OrdinalIgnoreCase))
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
        public void ReplaceMainHeaderText(ISlide sld, string shapeName,string text,string replaceText)
        {
            var temp = (IAutoShape)sld.Shapes.Where(x => x.Name == shapeName).FirstOrDefault();
            if (temp != null)
            {
                temp.TextFrame.Text = temp.TextFrame.Text.Replace(text, replaceText);
            }
        }
        public void ReplaceGroupHeaderText(ISlide sld, string groupShapeName,string shapeName, string text, string replaceText)
        {
            var group = (IGroupShape)sld.Shapes.Where(x => x.Name == groupShapeName).FirstOrDefault();
            if (group != null)
            {
                IAutoShape shape = (IAutoShape)group.Shapes.Where(x => x.Name == shapeName).FirstOrDefault();
                if (shape != null)
                {
                    shape.TextFrame.Text = shape.TextFrame.Text.Replace(text, replaceText);
                }
            }
        }
        public void ReplaceBenchmarkAndStatText(ISlide sld,ReportGeneratorParams param)
        {
            #region StatTest and BenchMark Update
            IAutoShape shape = (IAutoShape)sld.Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault();
            if (shape != null)
            {
                shape.TextFrame.Text = shape.TextFrame.Text.Replace("?", param.Sigtype_UniqueId == "1" ? benchmark : param.StatTest).Replace("95%", Convert.ToString(StatTesting) + "%");
                if (param.Sigtype_UniqueId == "1")
                {
                    shape = (IAutoShape)((IGroupShape)sld.Shapes.Where(x => x.Name == "benchmarkGroup").FirstOrDefault()).Shapes.Where(x => x.Name == "benchmark").FirstOrDefault();
                    shape.TextFrame.Text = shape.TextFrame.Text.Replace("?", benchmark);
                }
                else
                {
                    sld.Shapes.Remove(sld.Shapes.Where(x => x.Name == "benchmarkGroup").FirstOrDefault());
                }
            }
            #endregion
        }

        public string CommaSeparatedValues(string value)
        {
            string decimaval = "0";
            if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) != GlobalVariables.NANumber)
            {
                decimaval = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(value)));
            }
            return decimaval;
        }

        public string ToPascalCase(string text)
        {
            if (text.Length > 0)
                return text[0].ToString().ToUpper() + text.Substring(1).ToLower();
            else
                return string.Empty;
        }

    }
}