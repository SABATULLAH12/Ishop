using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Aspose.Slides.Charts;
using Aspose.Slides;
using System.Drawing;
using Svg;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Office.Core;
using interop = Microsoft.Office.Interop.PowerPoint;

namespace iSHOPNew.Models
{
    public class PathToPurchase
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
        public PathToPurchase()
        {
            accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
            accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
        }
        public string cacheString(object[] obj)
        {
            string output = "";

            for(var i = 0; i < obj.Length; i++)
            {
                output += ((obj[i] != null ? obj[i].ToString() : string.Empty) + ",");
            }
            return output;
        }
        public PathToPurchaseMetrics GetData(PathToPurchaseParams pathToPurchaseParams)
        {
            PathToPurchaseMetrics pathToPurchaseMetrics = null;
            string cachename = string.Empty;
            List<string> metrictypes = null;
            dal = new DataAccess();
            object[] paramvalues = null;
            try
            {
                paramvalues = new object[] { pathToPurchaseParams.CustomBase_UniqueId, pathToPurchaseParams.Comparison_UniqueIds, pathToPurchaseParams.TimePeriod_UniqueId,
                pathToPurchaseParams.ShopperSegment_UniqueId,pathToPurchaseParams.CustomBaseAdvancedFilters_UniqueId, pathToPurchaseParams.ShopperFrequency_UniqueId, pathToPurchaseParams.CustomBaseShopperFrequency_UniqueId,pathToPurchaseParams.Sigtype_UniqueId, pathToPurchaseParams.Sort,
                pathToPurchaseParams.CompetitorFrequency_UniqueId,pathToPurchaseParams.CompetitorRetailer_UniqueId,pathToPurchaseParams.CustomBaseCompetitorFrequency_UniqueId,pathToPurchaseParams.CustomBaseCompetitorRetailer_UniqueId};

                cachename = "usp_IshopP2PDashboard" + cacheString(paramvalues);
                var dashbrdInfo = HttpContext.Current.Application[cachename] as PathToPurchaseMetrics;
                if (dashbrdInfo != null)
                    return dashbrdInfo;

                ds = dal.GetData_WithIdMapping(paramvalues, "usp_IshopP2PDashboard");
                if (ds != null && ds.Tables.Count > 0)
                {
                    pathToPurchaseMetrics = new PathToPurchaseMetrics();
                    pathToPurchaseMetrics.SampleSize = Convert.ToString(ds.Tables[0].Rows[0]["respcnt"]).FormateSampleSizeNumber();
                    pathToPurchaseMetrics.StatTestSampleSize = Convert.ToString(ds.Tables[0].Rows[1]["respcnt"]).FormateSampleSizeNumber();
                    pathToPurchaseMetrics.pathToPurchaseMetricEntitylist = new List<PathToPurchaseMetricEntity>();
                    metrictypes = (from r in ds.Tables[1].AsEnumerable()
                                   select Convert.ToString(r["MetricType"])).Distinct().ToList();
                    foreach (string metrictype in metrictypes)
                    {
                        pathToPurchaseMetrics.pathToPurchaseMetricEntitylist.Add(new PathToPurchaseMetricEntity()
                        {
                            MetricType = metrictype
                        });
                    }

                    if (pathToPurchaseMetrics.pathToPurchaseMetricEntitylist != null && pathToPurchaseMetrics.pathToPurchaseMetricEntitylist.Count > 0)
                    {
                        foreach (PathToPurchaseMetricEntity ppm in pathToPurchaseMetrics.pathToPurchaseMetricEntitylist)
                        {
                            if (ppm.MetricType.Equals("TRIP MISSION", StringComparison.OrdinalIgnoreCase))
                            {
                                ppm.MetricData = (from r in ds.Tables[1].AsEnumerable()
                                                  where Convert.ToString(r["MetricType"]).Equals(ppm.MetricType)
                                                  && Convert.ToInt32(r["Flag"]) != 2
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

                                var query = (from r in ds.Tables[1].AsEnumerable()
                                             where Convert.ToString(r["MetricType"]).Equals(ppm.MetricType)
                                             && Convert.ToInt32(r["Flag"]) == 2
                                             select new PathToPurchaseEntity()
                                             {
                                                 Retailer = Convert.ToString(r["Retailer"]),
                                                 MetricType = Convert.ToString(r["MetricType"]),
                                                 Metric = Convert.ToString(r["Metric"]),
                                                 Volume = string.IsNullOrEmpty(Convert.ToString(r["Volume"])) ? 0 : Convert.ToDouble(r["Volume"]),
                                                 Significance = string.IsNullOrEmpty(Convert.ToString(r["Significance"])) ? 0 : Convert.ToDouble(r["Significance"]),
                                                 ChangeVolume = string.IsNullOrEmpty(Convert.ToString(r["ChangeVolume"])) ? 0 : Convert.ToDouble(r["ChangeVolume"]),
                                                 Flag = Convert.ToInt32(r["Flag"])
                                             }).Distinct().FirstOrDefault();
                                ppm.MetricData.Insert(3, query);
                            }
                            else
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
                            }
                            if (ppm.MetricData != null && ppm.MetricData.Count > 0)
                            {
                                if (ppm.MetricType.Equals("ORDER SUMMARY", StringComparison.OrdinalIgnoreCase) ||
                                    ppm.MetricType.Equals("DESTINATION ITEM", StringComparison.OrdinalIgnoreCase) || ppm.MetricType.Equals("TIME SPENT", StringComparison.OrdinalIgnoreCase)
                                    || ppm.MetricType.Equals("DESTINATION ITEM-Nets", StringComparison.OrdinalIgnoreCase) || ppm.MetricType.Equals("ORDER SUMMARY-Nets", StringComparison.OrdinalIgnoreCase))
                                {
                                    for (var i = 0; i < ppm.MetricData.Count; i++)
                                    {
                                        if (ppm.MetricData[i].Flag == 1)
                                        {
                                            ppm.MetricData[i].Selected_Popup_Metric_Item = true;
                                            //ppm.MetricData[1].Selected_Popup_Metric_Item = true;
                                            //ppm.MetricData[2].Selected_Popup_Metric_Item = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ppm.MetricData[0].Selected_Popup_Metric_Item = true;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pathToPurchaseMetrics = null;
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            if (pathToPurchaseMetrics != null)
            {
                HttpContext.Current.Application.Add(cachename, pathToPurchaseMetrics);
            }
            return pathToPurchaseMetrics;
        }
        #region create clustered bar chart
        private void Create_ClusteredBarChart(ISlide sld, P2PPopupDashboardData filter)
        {
            float data_label_x = 0.94f;
            float data_label_y = 0f;
            try
            {
                objectivelist = (from r in filter.OutputData select Convert.ToString(r.MetricType)).Distinct().ToList();
                metriclist = (from r in filter.OutputData where r.Flag != 2 select Convert.ToString(r.Metric)).Distinct().ToList();
                objectivelist.Reverse();
                metriclist.Reverse();
                IChart chart = sld.Shapes.AddChart(ChartType.ClusteredBar, chart_x_position, chart_y_position, chart_width, chart_height);
                chart.HasTitle = false;
                chart.Name = "chart";

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
                    chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i, objectivelist[i - 1]), chart.Type);
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
                        var query = (from r in filter.OutputData
                                     where Convert.ToString(r.MetricType).Equals(_objective, StringComparison.OrdinalIgnoreCase)
                                     && Convert.ToString(r.Metric).Equals(series, StringComparison.OrdinalIgnoreCase)
                                     select new
                                     {
                                         value = Convert.ToString(r.Volume),
                                         significance = Convert.ToString(r.Significance),
                                         changevalue = Convert.ToString(r.ChangeVolume)
                                     }).FirstOrDefault();

                        Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? (Convert.ToDouble(query.value) / 100) : 0)));

                        Series.Labels.DefaultDataLabelFormat.NumberFormat = "0%"; ;
                        Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0%";

                        Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                        Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;
                        Series.Labels.DefaultDataLabelFormat.ShowLeaderLines = true;

                        Series.Format.Fill.FillType = FillType.Solid;
                        Series.Format.Fill.SolidFillColor.Color = GetSerirsColour(chart.ChartData.Series.Count - serCount);

                        //Set Data Point Label Style
                        lbl = Series.DataPoints[catcount - 1].Label;
                        lbl.DataLabelFormat.Position = LegendDataLabelPosition.OutsideEnd;
                        lbl.DataLabelFormat.ShowValue = true;
                        lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                        lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;

                        ////Modify the Change value for dataLabels                      
                        lbl.AsIOverridableText.AddTextFrameForOverriding("");
                        lbl.TextFrameForOverriding.Paragraphs[0].Portions[0].Text = Convert.ToDouble(Convert.ToDouble(query.value) / 100).ToString("0%");

                        var tempDP_Label = Series.DataPoints[catcount - 1].Label.TextFrameForOverriding.Paragraphs[0].Portions;
                        tempDP_Label.Add(new Portion() { Text = " | " });
                        tempDP_Label.Add(new Portion() { Text = getChangeValue(Convert.ToDouble(query.changevalue)) });
                        tempDP_Label.Add(new Portion());
                        tempDP_Label[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                        tempDP_Label[1].PortionFormat.FillFormat.FillType = FillType.Solid;
                        tempDP_Label[2].PortionFormat.FillFormat.FillType = FillType.Solid;
                        tempDP_Label[2].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(Convert.ToDouble(query.significance), filter.ss);
                        LabelFontSize(lbl.TextFrameForOverriding.Paragraphs[0].Portions[0]);
                        LabelFontSize(lbl.TextFrameForOverriding.Paragraphs[0].Portions[1]);
                        LabelFontSize(lbl.TextFrameForOverriding.Paragraphs[0].Portions[2]);

                        if (filter.pptOrPdf == "pdf")
                        {
                            Series.DataPoints[catcount - 1].Label.X = data_label_x;
                            Series.DataPoints[catcount - 1].Label.Y = data_label_y;
                        }
                        else
                        {
                            Series.DataPoints[catcount - 1].Label.X = data_label_x;
                        }
                        catcount++;
                    }
                    catcount = 1;
                    serCount++;
                }
                chart.Axes.VerticalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
                chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

                chart.Axes.HorizontalAxis.MajorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;
                chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.FillFormat.FillType = FillType.NoFill;

                chart.Axes.HorizontalAxis.MinorGridLinesFormat.Line.Width = 0;
                chart.Axes.VerticalAxis.MinorGridLinesFormat.Line.Width = 0;

                chart.Axes.VerticalAxis.MajorTickMark = TickMarkType.None;
                chart.Axes.VerticalAxis.MinorTickMark = TickMarkType.None;

                chart.Axes.VerticalAxis.IsVisible = true;
                chart.Axes.HorizontalAxis.IsVisible = false;
                #region chart axis
                chart.Axes.VerticalAxis.TextFormat.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
                chart.Axes.VerticalAxis.TextFormat.PortionFormat.FontHeight = 11;

                chart.Axes.HorizontalAxis.TextFormat.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
                chart.Axes.HorizontalAxis.TextFormat.PortionFormat.FontHeight = 11;
                #endregion
                #region Set Legend
                //Set Legend Style
                chart.HasLegend = false;
                #endregion               
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
        #endregion
        #region create table
        private void Create_Table(ISlide sld, List<PathToPurchaseEntity> filter, double? samplesize)
        {
            filter = filter.Where(x => x.Flag != 2).Select(x => x).ToList();
            dblCols = new List<double> { 300, 69, 10, 69, 300, 69, 10, 69 };
            dblRows = filter.Select(x => Convert.ToDouble(35)).Take(filter.Count / 2);

            if (filter.Count % 2 == 1)
                dblRows = filter.Select(x => Convert.ToDouble(35)).Take((filter.Count / 2) + 1);
            else
                dblRows = filter.Select(x => Convert.ToDouble(35)).Take(filter.Count / 2);

            //Add table shape to slide
            ITable tbl = sld.Shapes.AddTable((float)31.74, (float)110.55, dblCols.ToArray(), dblRows.ToArray());
            int altrownumber = 0;
            Color colr = new Color();
            foreach (IRow row in tbl.Rows)
            {
                if (altrownumber == 0)
                    colr = Color.FromArgb(242, 242, 242);
                else
                    colr = Color.White;

                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.NoFill;
                    cell.BorderLeft.FillFormat.FillType = FillType.NoFill;
                    cell.BorderRight.FillFormat.FillType = FillType.NoFill;
                    cell.BorderBottom.FillFormat.FillType = FillType.NoFill;

                    cell.FillFormat.FillType = FillType.Solid;
                    cell.FillFormat.SolidFillColor.Color = colr;

                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(89, 89, 89);
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 16;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
                }
                altrownumber++;
                if (altrownumber == 2)
                    altrownumber = 0;
            }
            //************end table desain**********
            int filter_indx = 0;
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                //first metric
                tbl.Rows[i][0].TextFrame.Text = filter[filter_indx].Metric.Replace("&lt;", "<");
                tbl.Rows[i][0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

                tbl.Rows[i][1].TextFrame.Text = Convert.ToDouble(filter[filter_indx].Volume).ToString("0") + "%";
                tbl.Rows[i][1].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;

                tbl.Rows[i][2].TextFrame.Text = "|";
                tbl.Rows[i][2].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

                tbl.Rows[i][3].TextFrame.Text = getChangeValue(Convert.ToDouble(filter[filter_indx].ChangeVolume));
                tbl.Rows[i][3].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter[filter_indx].Significance, samplesize);
                tbl.Rows[i][3].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

                //second metric
                filter_indx++;
                if (filter.Count > filter_indx)
                {
                    tbl.Rows[i][4].TextFrame.Text = filter[filter_indx].Metric.Replace("&lt;", "<");
                    tbl.Rows[i][4].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

                    tbl.Rows[i][5].TextFrame.Text = Convert.ToDouble(filter[filter_indx].Volume).ToString("0") + "%";
                    tbl.Rows[i][5].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;

                    tbl.Rows[i][6].TextFrame.Text = "|";
                    tbl.Rows[i][6].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

                    tbl.Rows[i][7].TextFrame.Text = getChangeValue(Convert.ToDouble(filter[filter_indx].ChangeVolume));
                    tbl.Rows[i][7].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter[filter_indx].Significance, samplesize);
                    tbl.Rows[i][7].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;
                    filter_indx++;
                }
            }
        }
        private void Create_Sorted_Table(ISlide sld, List<PathToPurchaseEntity> filter, double? samplesize)
        {
            filter = filter.Where(x => x.Flag != 2).Select(x => x).ToList();
            dblCols = new List<double> { 300, 69, 10, 69, 300, 69, 10, 69 };
            dblRows = filter.Select(x => Convert.ToDouble(35)).Take(filter.Count / 2);

            if (filter.Count % 2 == 1)
                dblRows = filter.Select(x => Convert.ToDouble(35)).Take((filter.Count / 2) + 1);
            else
                dblRows = filter.Select(x => Convert.ToDouble(35)).Take(filter.Count / 2);

            //Add table shape to slide
            ITable tbl = sld.Shapes.AddTable((float)31.74, (float)110.55, dblCols.ToArray(), dblRows.ToArray());
            int altrownumber = 0;
            Color colr = new Color();
            foreach (IRow row in tbl.Rows)
            {
                if (altrownumber == 0)
                    colr = Color.FromArgb(242, 242, 242);
                else
                    colr = Color.White;

                foreach (ICell cell in row)
                {
                    cell.BorderTop.FillFormat.FillType = FillType.NoFill;
                    cell.BorderLeft.FillFormat.FillType = FillType.NoFill;
                    cell.BorderRight.FillFormat.FillType = FillType.NoFill;
                    cell.BorderBottom.FillFormat.FillType = FillType.NoFill;

                    cell.FillFormat.FillType = FillType.Solid;
                    cell.FillFormat.SolidFillColor.Color = colr;

                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(89, 89, 89);
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 16;
                    cell.TextFrame.Paragraphs[0].Portions[0].PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
                }
                altrownumber++;
                if (altrownumber == 2)
                    altrownumber = 0;
            }
            //************end table desain**********
            int filter_indx = 0;
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                //first metric
                tbl.Rows[i][0].TextFrame.Text = filter[filter_indx].Metric.Replace("&lt;", "<");
                tbl.Rows[i][0].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

                tbl.Rows[i][1].TextFrame.Text = Convert.ToDouble(filter[filter_indx].Volume).ToString("0") + "%";
                tbl.Rows[i][1].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;

                tbl.Rows[i][2].TextFrame.Text = "|";
                tbl.Rows[i][2].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

                tbl.Rows[i][3].TextFrame.Text = getChangeValue(Convert.ToDouble(filter[filter_indx].ChangeVolume));
                tbl.Rows[i][3].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter[filter_indx].Significance, samplesize);
                tbl.Rows[i][3].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;
                filter_indx++;
            }
            //for next column
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                //second metric             
                if (filter.Count > filter_indx)
                {
                    tbl.Rows[i][4].TextFrame.Text = filter[filter_indx].Metric.Replace("&lt;", "<");
                    tbl.Rows[i][4].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;

                    tbl.Rows[i][5].TextFrame.Text = Convert.ToDouble(filter[filter_indx].Volume).ToString("0") + "%";
                    tbl.Rows[i][5].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Right;

                    tbl.Rows[i][6].TextFrame.Text = "|";
                    tbl.Rows[i][6].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Center;

                    tbl.Rows[i][7].TextFrame.Text = getChangeValue(Convert.ToDouble(filter[filter_indx].ChangeVolume));
                    tbl.Rows[i][7].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter[filter_indx].Significance, samplesize);
                    tbl.Rows[i][7].TextFrame.Paragraphs[0].ParagraphFormat.Alignment = TextAlignment.Left;
                    filter_indx++;
                }
            }
        }

        #endregion
        private void LabelFontSize(IPortion lbl)
        {
            lbl.PortionFormat.FontHeight = 11;
            lbl.PortionFormat.LatinFont = new FontData("Franklin Gothic Book");
        }
        private System.Drawing.Color GetSignificanceColor(string significancevalue, double samplesize)
        {
            System.Drawing.Color color = System.Drawing.Color.Black;
            if (!string.IsNullOrEmpty(significancevalue))
            {
                if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
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
        public System.Drawing.Color GetSerirsColour(int SeriesIndex)
        {
            Random rnd = new Random();
            System.Drawing.Color color = new System.Drawing.Color();
            switch (SeriesIndex)
            {
                case 0:
                    {
                        color = System.Drawing.Color.FromArgb(241, 95, 46);
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
                    color =
                        System.Drawing.Color.FromArgb(rnd.Next(0, 112), rnd.Next(0, 150), rnd.Next(0, 100));
                    break;

            }
            return color;
        }
        public string PopupExportDashboard(string filepath, string destFile, P2PPopupDashboardData filter, HttpContextBase context)
        {
            int i = 0, j = 0;
            string suffix = ((filter.DemofilterName == "Food Item" || filter.DemofilterName == "Beverage Item") ? "" : "Bar");
            suffix = "Bar";
            string chartTitle = (filter.DemoTitle == "" || filter.DemoTitle == null ? filter.DemofilterName : filter.DemoTitle);
            switch (chartTitle.ToLower())
            {
                case "destination item":
                    chartTitle = "DESTINATION ITEM";
                    break;
                case "weekday or weekend":
                    chartTitle = "DAY OF THE WEEK";
                    break;
                case "consideration":
                    chartTitle = "STORES CONSIDERED";
                    break;
                case "immediate consumption":
                    chartTitle = "IC ITEMS IN BASKET - (CONSUMED WITHIN 1 HOUR)";
                    break;
                case "order summary":
                    chartTitle = "ITEMS PURCHASED";
                    break;
                default:
                    chartTitle = chartTitle;
                    break;
            }
            //chartTitle = chartTitle == "DESTINATION ITEM" ? "DESTINATION ITEMS" : chartTitle;
            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            if (filter.DemofilterName.Trim().ToLower() == "table")
            {
                suffix = "";
                filepath = HttpContext.Current.Server.MapPath("~/Templates/P2PDashboard_Popup_Table.pptx");
            }
            else
            {
                filepath = HttpContext.Current.Server.MapPath("~/Templates/P2PDashboard_PopupBar.pptx");
            }

            try
            {
                using (Presentation pres = new Presentation(filepath))
                {
                    //Master Slide data filling
                    //StatTestAgainst
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "chartTitle").FirstOrDefault()).TextFrame.Text = chartTitle;
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = filter.statTest;
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "leftPane").FirstOrDefault()).TextFrame.Text = filter.LeftpanelData.ToUpper();
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "ss").FirstOrDefault()).TextFrame.Text = "Sample Size – " + filter.ss.ToString("#,##,##0");
                    if (filter.ShopperFrequency_UniqueId == "10")
                    {
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips, Sorted by: Largest "  + filter.Sort + (", Time Period: " + filter.TimePeriod ) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                    }
                    else
                    {
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips (" + filter.ShopperFrequency + "), Sorted by: Largest " + filter.Sort + (", Time Period: " + filter.TimePeriod) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                    }
                    //Delete extra slides
                    if (filter.DemofilterName.Trim().ToLower() == "table")
                    {
                        //Presentation pres_table_appendix = new Presentation(HttpContext.Current.Server.MapPath("~/Templates/P2PDashboard_PopupBar-clone.pptx"));
                        List<PathToPurchaseEntity> filterlist = filter.OutputData.Where(x => x.Flag != 2).Select(x => x).ToList();
                        List<PathToPurchaseEntity> filterlistSecondary = new List<PathToPurchaseEntity>();
                        if (filter.OutputDataSecondary!=null)
                        {
                            filterlistSecondary=filter.OutputDataSecondary.Where(x => x.Flag != 2).Select(x => x).ToList();
                        }
                        List<PathToPurchaseEntity> _tblfilters = null;
                        int sldnum = 0;
                        if (filterlist.Count <= 20)
                        {
                            //Create_Table(pres.Slides[0], filterlist, filter.ss);
                            pres.Slides.AddClone(pres.Slides[0]);
                            Create_Sorted_Table(pres.Slides[1], filterlist, filter.ss);
                        }
                        else
                        {
                            for (int k = 0; k < filterlist.Count; k++)
                            {
                                if (_tblfilters == null)
                                    _tblfilters = new List<PathToPurchaseEntity>();

                                _tblfilters.Add(filterlist[k]);
                                if (_tblfilters.Count == 20 || filterlist.Count == (k + 1))
                                {
                                    pres.Slides.AddClone(pres.Slides[0]);
                                    ISlide sld = pres.Slides[pres.Slides.Count - 1];
                                    if (sldnum > 0)
                                    {
                                        ((IAutoShape)sld.Shapes.Where(x => x.Name == "chartTitle").FirstOrDefault()).TextFrame.Text = chartTitle;
                                        ((IAutoShape)sld.Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = filter.statTest;
                                        ((IAutoShape)sld.Shapes.Where(x => x.Name == "leftPane").FirstOrDefault()).TextFrame.Text = filter.LeftpanelData.ToUpper();
                                        ((IAutoShape)sld.Shapes.Where(x => x.Name == "ss").FirstOrDefault()).TextFrame.Text = "Sample Size – " + filter.ss.ToString("#,##,##0");
                                        if (filter.ShopperFrequency_UniqueId == "10")
                                        {
                                            ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips, Sorted by: Largest "  + filter.Sort + (", Time Period: " + filter.TimePeriod ) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                                        }
                                        else
                                        {
                                            ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips (" + filter.ShopperFrequency + "), Sorted by: Largest " + filter.Sort + (", Time Period: " + filter.TimePeriod) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                                        }
                                    }
                                    //Create_Table(sld, _tblfilters, filter.ss);
                                    Create_Sorted_Table(sld, _tblfilters, filter.ss);
                                    _tblfilters = null;
                                    sldnum++;
                                }
                            }
                           
                        }
                        if (filterlistSecondary.Count <= 20 && filterlistSecondary.Count>0)
                        {
                            pres.Slides.AddClone(pres.Slides[0]);
                            ((IAutoShape)pres.Slides[pres.Slides.Count - 1].Shapes.Where(x => x.Name == "chartTitle").FirstOrDefault()).TextFrame.Text = filter.DemoTitleSecondary;
                            Create_Sorted_Table(pres.Slides[pres.Slides.Count-1], filterlistSecondary, filter.ss);
                        }
                        pres.Slides.RemoveAt(0);
                        //pres_table_appendix                       

                        //pres.Slides.AddClone(pres_table_appendix.Slides[0]);
                        //pres.Slides.AddClone(pres_table_appendix.Slides[1]);                       

                        //((IAutoShape)pres.Slides[pres.Slides.Count - 1].Shapes.Where(x => x.Name == "chartTitle").FirstOrDefault()).TextFrame.Text = chartTitle;
                        //((IAutoShape)pres.Masters[1].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = filter.statTest;
                        //((IAutoShape)pres.Masters[1].Shapes.Where(x => x.Name == "leftPane").FirstOrDefault()).TextFrame.Text = filter.LeftpanelData.ToUpper();
                        //((IAutoShape)pres.Masters[1].Shapes.Where(x => x.Name == "ss").FirstOrDefault()).TextFrame.Text = "Sample Size – " + filter.ss.ToString("#,##,##0");
                    }
                    else
                    {
                        //for (i = pres.Slides.Count - 1; i >= 0; i--)
                        //{
                        //    if (i != filter.OutputData.Count - 1)
                        //    {
                        //        pres.Slides.RemoveAt(i);
                        //    }
                        //}
                        //Update the chart
                        //Create_ClusteredBarChart(pres.Slides[0], filter);
                        //Update the chart
                        IChart chrt = (IChart)pres.Slides[0].Shapes.Where(x => x.Name == "chart").FirstOrDefault();
                        //chrt.PlotArea.Width = 0.92f;
                        IChartDataWorkbook fact = chrt.ChartData.ChartDataWorkbook;
                        List<PathToPurchaseEntity> filterlist = filter.OutputData.Where(x => x.Flag != 2).Select(x => x).ToList();
                        int datapoints_count = chrt.ChartData.Series[0].DataPoints.Count;
                        for (int d = datapoints_count - 1; d > filterlist.Count - 1; d--)
                        {
                            chrt.ChartData.Series[0].DataPoints.RemoveAt(d);
                        }
                        //Update categories
                        foreach (var x in filterlist)
                        {
                            fact.GetCell(0, 1 + i, 0, x.Metric);
                            i++;
                        }
                        i = 0;
                        //Fill the values
                        foreach (var ser in chrt.ChartData.Series)
                        {
                            j = 0;
                            foreach (var x in filterlist)
                            {
                                fact.GetCell(0, j + 1, i + 1, Math.Round(x.Volume));
                                //Modify the Change value for dataLabels
                                var tempDP_Label = ser.DataPoints[j].Label.TextFrameForOverriding.Paragraphs[0].Portions;
                                //Clear All Portion except 1st
                                for (int idx = tempDP_Label.Count - 1; idx > 0; idx--)
                                {
                                    tempDP_Label.RemoveAt(idx);
                                }
                                //PDF special Case
                                if (filter.pptOrPdf == "pdf")
                                {
                                    tempDP_Label[0].Text = x.Volume.ToString("#0");
                                }
                                tempDP_Label.Add(new Portion() { Text = "% | " });
                                tempDP_Label.Add(new Portion() { Text = getChangeValue(x.ChangeVolume) });
                                tempDP_Label[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tempDP_Label[1].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tempDP_Label[2].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tempDP_Label[2].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(x.Significance, filter.ss);
                                ser.DataPoints[j].Label.X = (float)0.94;
                                ser.DataPoints[j].Label.Y = (float)0.0;
                                j++;
                            }
                            i++;
                        }
                        chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowLeaderLines = true;
                        chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;
                        chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.NumberFormat = "##0";
                        chrt.Axes.HorizontalAxis.IsAutomaticMaxValue = false;
                        chrt.Axes.HorizontalAxis.MaxValue = filterlist.Max(x => x.Volume) + 10;// 100;
                        //chrt.ChartData.Series[0].Labels.DefaultDataLabelFormat.ShowLeaderLines = true;
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "chartTitle").FirstOrDefault()).TextFrame.Text = chartTitle;
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = filter.statTest;
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "leftPane").FirstOrDefault()).TextFrame.Text = filter.LeftpanelData.ToUpper();
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "ss").FirstOrDefault()).TextFrame.Text = "Sample Size – " + filter.ss.ToString("#,##,##0");
                        if (filter.ShopperFrequency_UniqueId == "10")
                        {
                            ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips, Sorted by: Largest "  + filter.Sort + (", Time Period: " + filter.TimePeriod ) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                        }
                        else
                        {
                            ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips (" + filter.ShopperFrequency + "), Sorted by: Largest " + filter.Sort + (", Time Period: " + filter.TimePeriod) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                        }
                    }
                    if (filter.pptOrPdf == "pdf")
                    {
                        //Check if office is installed
                        if (isPPT_Installed())
                        {
                            pres.Save(context.Server.MapPath("~/Temp/P2P_PDF"), Aspose.Slides.Export.SaveFormat.Pptx);
                        }
                        else
                        {
                            pres.Save(destFile, Aspose.Slides.Export.SaveFormat.Pptx);
                            Presentation pres_new = new Presentation(destFile);
                            //PdfOptions pdfop = new PdfOptions();
                            ////pdfop.JpegQuality = 100;
                            //pdfop.SufficientResolution = 10000;
                            //pdfop.EmbedFullFonts = true;
                            //pdfop.EmbedTrueTypeFontsForASCII = true;
                            //pdfop.TextCompression = PdfTextCompression.Flate;
                            //pdfop.JpegQuality = 100;
                            //pdfop.SaveMetafilesAsPng = true;
                            //pdfop.Compliance = PdfCompliance.Pdf15;
                            ////pdfOp.EmbedFullFonts = true;
                            pres_new.Save(destFile, Aspose.Slides.Export.SaveFormat.Pdf);
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
                        var file = ppt.Open(context.Server.MapPath("~/Temp/P2P_PDF"), MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoFalse);
                        file.SaveCopyAs(destFile, interop.PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoTrue);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            return "";
        }
        public void setValuePerSlide(P2PPopupDashboardData filter, ITable tbl, int startInd, int endInd)
        {
            int i = 1, j;
            for (i = startInd - 1, j = 0; i < endInd; i++, j++)
            {
                if (filter.OutputData.Count >= i + 1)
                {
                    //1st half
                    tbl[0, j].TextFrame.Text = filter.OutputData[i].Metric;
                    tbl[1, j].TextFrame.Text = filter.OutputData[i].Volume.ToString("#0") + "%";
                    tbl[2, j].TextFrame.Text = "|";
                    tbl[3, j].TextFrame.Text = getChangeValue(filter.OutputData[i].ChangeVolume); //(filter.OutputData[i].Change > 0 ? "+" + filter.OutputData[i].Change.ToString("#0.0") : filter.OutputData[i].Change.ToString("#0.0")) + "%";
                    tbl[3, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                    tbl[1, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                    tbl[2, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                    tbl[3, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter.OutputData[i].Significance, filter.ss);
                    tbl[1, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter.OutputData[i].Significance, filter.ss);
                    tbl[2, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter.OutputData[i].Significance, filter.ss);
                    i++;
                    if (filter.OutputData.Count >= i + 1)
                    {
                        //2nd half
                        tbl[4, j].TextFrame.Text = filter.OutputData[i].Metric;
                        tbl[5, j].TextFrame.Text = filter.OutputData[i].Volume.ToString("#0") + "%";
                        tbl[6, j].TextFrame.Text = "|";
                        tbl[7, j].TextFrame.Text = getChangeValue(filter.OutputData[i].ChangeVolume); //(filter.OutputData[i].Change > 0 ? "+" + filter.OutputData[i].Change.ToString("#0.0") : filter.OutputData[i].Change.ToString("#0.0")) + "%";
                        tbl[7, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                        tbl[6, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                        tbl[5, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                        tbl[7, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter.OutputData[i].Significance, filter.ss);
                        tbl[6, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter.OutputData[i].Significance, filter.ss);
                        tbl[5, j].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(filter.OutputData[i].Significance, filter.ss);
                    }
                }
            }
        }
        string getChangeValue(double ch)
        {
            double nCh = Math.Round(ch, 1);
            if (nCh > 0) { return "+" + nCh.ToString("##0.0"); }
            return nCh.ToString("##0.0");
        }
        public Color getColorBasedOnSignificanceForPopup(double? i, double? samplesize)
        {
            if (i == null)
                return Color.FromArgb(0, 0, 0);
            else if (i < accuratestatvaluenega)
                return Color.Red;
            else if (i > accuratestatvalueposi)
                return Color.Green;
            else if (samplesize >= GlobalVariables.LowSample && samplesize < 100)
                return Color.Gray;

            return Color.FromArgb(0, 0, 0);
        }
        ////For BackUp
        //public void imageReplace(PictureFrame tempImg, string loc, HttpContextBase context, int indx, int fact)
        //{
        //    string pathToSvg = context.Server.MapPath(loc);
        //    //Create a directory
        //    string subPath = "~/Images/temp/" + context.Session.SessionID; // your code goes here
        //    bool exists = System.IO.Directory.Exists(context.Server.MapPath(subPath));
        //    if (!exists)
        //        System.IO.Directory.CreateDirectory(context.Server.MapPath(subPath));
        //    string tempPath = context.Server.MapPath(subPath + "/img" + indx + ".emf");
        //    try
        //    {
        //        SvgDocument.Open(pathToSvg);
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //    var xyz = SvgDocument.Open(pathToSvg);
        //    if (File.Exists(tempPath))
        //    {
        //        File.Delete(tempPath);
        //    }
        //    if (fact == -1) //For bevearge and food items
        //    {
        //        xyz.Height = 300;
        //        xyz.Width = 300;
        //    }
        //    else
        //    {
        //        if (fact == -2) //For Establishment logo
        //        {
        //            xyz.Height = 400;// 4 * xyz.Bounds.Height;
        //            xyz.Width = 400;// * xyz.Bounds.Width;
        //        }
        //        else
        //        {
        //            xyz.Height = 4 * xyz.Bounds.Height;
        //            xyz.Width = 4 * xyz.Bounds.Width;
        //        }
        //    }
        //    xyz.Draw().Save(tempPath, System.Drawing.Imaging.ImageFormat.Emf);
        //    using (Image img = Image.FromFile(tempPath, true))
        //    {
        //        tempImg.PictureFormat.Picture.Image.ReplaceImage(img);
        //    }
        //}
        ////End For BackUp

        public void imageReplace(PictureFrame tempImg, string loc, HttpContextBase context, int indx, int fact)
        {
            string pathToSvg = context.Server.MapPath(loc);
            string subPath = "~/Images/temp/" + context.Session.SessionID; // your code goes here
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
                bool isBoundsExist = true;
                try
                {
                    var rectangleF = xyz.Bounds;
                }
                catch(Exception ex)
                {
                    isBoundsExist = false;
                }
                if (isBoundsExist)
                {
                    if (xyz.Bounds.Width > xyz.Bounds.Height)
                    {
                        ratio = xyz.Bounds.Width / xyz.Bounds.Height;
                        xyz.Height = (int)((4 * ratio) * xyz.Bounds.Height);
                        xyz.Width = (int)((4 * widthRatio) * xyz.Bounds.Width);
                    }
                    else
                    {
                        if (xyz.Bounds.Width < xyz.Bounds.Height)
                        {
                            ratio = xyz.Bounds.Height / xyz.Bounds.Width;
                            xyz.Width = (int)((2 * ratio * widthRatio) * xyz.Bounds.Width);
                            xyz.Height = 2 * xyz.Bounds.Height;
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
        public string ExportToFullDashboardPPT(string filepath, string destFile, P2PDashboardData filter, HttpContextBase context)
        {

            try { 
            #region Variables
            int indx = 0;
            List<PathToPurchaseMetricEntity> planningType = filter.OutputData.Where(x => x.MetricType == "PLANNING").ToList();
            #endregion Variables

            #region select slide
            int slideNo = 1;
            int isSpur = planningType == null ? -1 : (planningType.FirstOrDefault().MetricData.FirstOrDefault().Metric.ToLower() == "spur of the moment" ? 1 : 0);

            if (filter.changedData.Where(x => x.name == "PLANNING").FirstOrDefault().value.ToUpper() != "NONE")
            {
                isSpur = filter.changedData.Where(x => x.name == "PLANNING").FirstOrDefault().value.ToLower() == "spur of the moment" ? 1 : 0;
            }
            switch (isSpur)
            {
                case -1: return "error";
                case 0: slideNo = 1; break;
                case 1: slideNo = 2; break;
            }

            #endregion select slide

            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            using (Presentation pres = new Presentation(filepath))
            {
                #region Variable
                List<PathToPurchaseMetricEntity> tempList = new List<PathToPurchaseMetricEntity>();
                PathToPurchaseEntity finalList = new PathToPurchaseEntity();
                List<PathToPurchaseEntity> finalData = new List<PathToPurchaseEntity>();
                PathToPurchaseEntity average = new PathToPurchaseEntity();
                IGroupShape tempGroup;
                IAutoShape tmpShape;
                #endregion Variables
                indx = pres.Slides.Count - 1;
                foreach (var item in pres.Slides.Reverse())
                {
                    if (indx == slideNo - 1)
                    {
                        indx--; continue;
                    }
                    else
                    {
                        item.Remove();
                    }
                    indx--;
                }

                PictureFrame tempImg; string loc; changedData tempStore;
                #region Master Slide
                //StatTestAgainst
                ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "StatTestAgainst").FirstOrDefault()).TextFrame.Text = filter.statTest;
                ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "leftPane").FirstOrDefault()).TextFrame.Text = filter.LeftpanelData.ToUpper();
                ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "ss").FirstOrDefault()).TextFrame.Text = "Sample Size – " + filter.ss.ToString("#,##,##0");
                if (filter.ShopperFrequency_UniqueId == "10")
                {
                    ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips, Sorted by: Largest "  + filter.Sort + (", Time Period: " + filter.TimePeriod ) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                }
                else
                {
                        ((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker, Base - Total Trips (" + filter.ShopperFrequency + "), Sorted by: Largest " + filter.Sort + (", Time Period: " + filter.TimePeriod) + ("\nFilters: " + (!string.IsNullOrEmpty(filter.Filters) ? filter.Filters : "NONE"));
                        //((IAutoShape)pres.Slides[0].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = "Source: CCNA iSHOP Tracker\n" + "Total Trips (" + filter.ShopperFrequency + ")" + "\nSorted by: Largest " + filter.Sort;
                    }
                #endregion Master Slide
                #region Restraunt name updation
                loc = "~/Images/P2PDashboardEsthmtImages/" + replace_file_special_characters(filter.OutputData.FirstOrDefault().MetricData.FirstOrDefault().Retailer) + ".svg";
                imageReplace((PictureFrame)pres.Slides[0].Shapes.Where(x => x.Name == "RetailerImg").FirstOrDefault(), loc, context, 8, -2);
                #endregion

                #region Weekday Or Weekend
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "Weekday").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "WEEKDAY OR WEEKEND").ToList();
                setValuesForMetricsWithVM(tempGroup, (tempList[0]).MetricData, 2, true, false, filter.ss);

                #endregion


                #region Time Of Day
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "tod").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "TIME OF DAY").ToList();
                setValuesForMetrics(tempGroup, (tempList[0]).MetricData, tempList.FirstOrDefault().MetricData.Count > 1 ? tempList.FirstOrDefault().MetricData.Count : -1, true, true, filter.ss);

                #endregion

                #region locationPrior
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "locationPrior").FirstOrDefault();
                tempStore = filter.changedData.Where(x => x.name == "LOCATION PRIOR TO TRIP" && x.value != "none").FirstOrDefault();
                if (tempStore == null)
                {
                    tempList = filter.OutputData.Where(x => x.MetricType == "LOCATION PRIOR TO TRIP").ToList();
                    setValuesForMetricsWithVM(tempGroup, (tempList[0]).MetricData, -1, false, true, filter.ss);
                }
                else
                {
                    finalList = (from o in filter.OutputData
                                 where o.MetricType == "LOCATION PRIOR TO TRIP"
                                 select o.MetricData into temp
                                 from m in temp
                                 where m.Metric == tempStore.value
                                 select m).FirstOrDefault();
                    setValuesForMetricsWithVMSDynamic(tempGroup, finalList, -1, false, true, filter.ss);

                }
                //Replace Images
                loc = "~/Images/ishop-P2P-Icons/Location Prior To trip/" + (tempStore != null ? replace_file_special_characters(finalList.Metric) : replace_file_special_characters((tempList[0]).MetricData.FirstOrDefault().Metric)) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "img").FirstOrDefault(), loc, context, 1, 0);
                #endregion

                #region planningType
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "planningType").FirstOrDefault();
                tempStore = filter.changedData.Where(x => x.name == "PLANNING" && x.value != "none").FirstOrDefault();
                if (tempStore == null)
                {
                    tempList = filter.OutputData.Where(x => x.MetricType == "PLANNING").ToList();
                    setValuesForMetricsWithVM(tempGroup, (tempList[0]).MetricData, -1, false, true, filter.ss);
                }
                else
                {
                    finalList = (from o in filter.OutputData
                                 where o.MetricType == "PLANNING"
                                 select o.MetricData into temp
                                 from m in temp
                                 where m.Metric == tempStore.value
                                 select m).FirstOrDefault();
                    setValuesForMetricsWithVMSDynamic(tempGroup, finalList, -1, false, true, filter.ss);

                }
                    if (isSpur == 1)
                    {
                        //Fill the time pie chart
                        plotBalloonPieChart(tempGroup, (tempStore != null ? finalList : ((tempList[0]).MetricData.FirstOrDefault())));
                    }
                    else
                    {
                        //Replace Images
                        int indexPlanning = (int)Math.Ceiling(tempStore != null ? finalList.Volume / 20 : ((tempList[0]).MetricData.FirstOrDefault().Volume / 20));
                        var planingtype = (from m in filter.OutputData where m.MetricType.Equals("PLANNING", StringComparison.OrdinalIgnoreCase) select m).FirstOrDefault();
                        var metric = (from m in planingtype.MetricData where m.Selected_Popup_Metric_Item == true select m.Metric).FirstOrDefault();                                        
                        loc = "~/Images/ishop-P2P-Icons/Planning/" + metric + ".svg";
                        imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "img").FirstOrDefault(), loc, context, 1, 0);
                    }
                #endregion

                #region Preparation Types
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "Preparation Types").FirstOrDefault();
                tempStore = filter.changedData.Where(x => x.name == "Preparation Types" && x.value != "none").FirstOrDefault();
                if (tempStore == null)
                {
                    tempList = filter.OutputData.Where(x => x.MetricType == "Preparation Types").ToList();
                    setValuesForMetricsWithVM(tempGroup, (tempList[0]).MetricData, -1, false, true, filter.ss);
                }
                else
                {
                    finalList = (from o in filter.OutputData
                                 where o.MetricType == "Preparation Types"
                                 select o.MetricData into temp
                                 from m in temp
                                 where m.Metric == tempStore.value
                                 select m).FirstOrDefault();
                    setValuesForMetricsWithVMSDynamic(tempGroup, finalList, -1, false, true, filter.ss);

                }
                #endregion

                #region whoWith
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "whoWith").FirstOrDefault();
                tempStore = filter.changedData.Where(x => x.name == "WHO WITH" && x.value != "none").FirstOrDefault();
                if (tempStore == null)
                {
                    tempList = filter.OutputData.Where(x => x.MetricType == "WHO WITH").ToList();
                    setValuesForMetricsWithVM(tempGroup, (tempList[0]).MetricData, -1, false, true, filter.ss);
                }
                else
                {
                    finalList = (from o in filter.OutputData
                                 where o.MetricType == "WHO WITH"
                                 select o.MetricData into temp
                                 from m in temp
                                 where m.Metric == tempStore.value
                                 select m).FirstOrDefault();
                    setValuesForMetricsWithVMSDynamic(tempGroup, finalList, -1, false, true, filter.ss);

                }
                //Replace Images
                loc = "~/Images/ishop-P2P-Icons/Who With/" + (tempStore != null ? replace_file_special_characters(finalList.Metric) : replace_file_special_characters((tempList[0]).MetricData.FirstOrDefault().Metric)) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "img").FirstOrDefault(), loc, context, 1, 0);
                #endregion

                #region REASON FOR STORE CHOICE
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "Reason for store choice").FirstOrDefault();
                tempStore = filter.changedData.Where(x => x.name == "REASON FOR STORE CHOICE" && x.value != "none").FirstOrDefault();
                if (tempStore == null)
                {
                    tempList = filter.OutputData.Where(x => x.MetricType == "REASON FOR STORE CHOICE").ToList();
                    setValuesForMetricsWithVM(tempGroup, (tempList[0]).MetricData, -1, false, true, filter.ss);
                }
                else
                {
                    finalList = (from o in filter.OutputData
                                 where o.MetricType == "REASON FOR STORE CHOICE"
                                 select o.MetricData into temp
                                 from m in temp
                                 where m.Metric == tempStore.value
                                 select m).FirstOrDefault();
                    setValuesForMetricsWithVMSDynamic(tempGroup, finalList, -1, false, true, filter.ss);

                }
                #endregion

                #region Consideration
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "Consideration").FirstOrDefault();
                tempStore = filter.changedData.Where(x => x.name == "CONSIDERATION" && x.value != "none").FirstOrDefault();
                if (tempStore == null)
                {
                    //tempList = filter.OutputData.Where(x => x.MetricType == "CONSIDERATION").ToList();
                    average = (from o in filter.OutputData
                               where o.MetricType == "CONSIDERATION"
                               select o.MetricData into temp
                               from m in temp
                               where m.Flag == 2
                               select m).FirstOrDefault();

                    setValuesForMetricsWithVMSDynamic(tempGroup, average, -1, false, true, filter.ss);
                }
                else
                {
                    finalList = (from o in filter.OutputData
                                 where o.MetricType == "CONSIDERATION"
                                 select o.MetricData into temp
                                 from m in temp
                                 where m.Metric == tempStore.value
                                 select m).FirstOrDefault();
                    setValuesForMetricsWithVMSDynamic(tempGroup, finalList, -1, false, true, filter.ss);

                }
                plotBalloonPieChart(tempGroup, (tempStore != null ? finalList : average));

                #endregion

                #region balloon
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "balloon").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "TRIP MISSION").ToList();
                //setValuesForMetrics(tempGroup, (tempList[0]).MetricData, tempList.FirstOrDefault().MetricData.Count > 1 ? tempList.Count : -1, true, false);
                setValuesForMetrics(tempGroup, (tempList[0]).MetricData, 4, true, true, filter.ss);
                //Plot balloon charts
                indx = 1;
                for (var i = 0; i < (tempList[0]).MetricData.Count - 3; i++)
                {
                    plotBalloonPieChart((IGroupShape)tempGroup.Shapes.Where(x => x.Name == "balloon" + indx).FirstOrDefault(), (tempList[0]).MetricData[i]);
                    //Replace Images
                    loc = "~/Images/ishop-P2P-Icons/Trip Mission/" + (tempStore != null ? replace_file_special_characters(finalList.Metric) : replace_file_special_characters((tempList[0]).MetricData[i].Metric)) + ".svg";
                    imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "img" + indx).FirstOrDefault(), loc, context, 1, 0);
                    indx++;
                }
                #endregion


                /*Harsha*/
                #region destinationItems
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "destinationItems").FirstOrDefault();
                tempList = filter.OutputData.Where(x => (x.MetricType == "DESTINATION ITEM" || x.MetricType == "DESTINATION ITEM-Nets")).ToList();
                finalData=((tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).ToList());
                finalData.AddRange((tempList[1]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).ToList());
                setValuesForindMetricsWithVM(tempGroup, finalData[0], 1, false, true, filter.ss);
                loc = "~/Images/ishop-P2P-Icons/Destination Item/" + replace_file_special_characters(finalData[0].Metric) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "Picture1").FirstOrDefault(), loc, context, 1, 0);
                setValuesForindMetricsWithVM(tempGroup, finalData[1], 2, false, true, filter.ss);
                loc = "~/Images/ishop-P2P-Icons/Destination Item/" + replace_file_special_characters(finalData[1].Metric) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "Picture2").FirstOrDefault(), loc, context, 1, 0);
                setValuesForindMetricsWithVM(tempGroup, finalData[2], 3, false, true, filter.ss);
                loc = "~/Images/ishop-P2P-Icons/Destination Item/" + replace_file_special_characters(finalData[2].Metric) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "Picture3").FirstOrDefault(), loc, context, 1, 0);
                #endregion destinationItems
                #region purchasedNartd
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "purchasedNartd").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "PURCHASED NARTD").ToList();
                finalList = (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).FirstOrDefault();
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "vm").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = finalList.Volume.ToString("#0") + "%";
                //((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "vm").FirstOrDefault()).TextFrame.Paragraphs[1].Portions[0].Text = finalList.Metric;
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = getChangeValue(finalList.ChangeVolume);
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(finalList.Significance, filter.ss);
                //Fill pie chart
                plotBalloonPieChart(tempGroup, finalList);
                #endregion purchasedNartd
                #region immediateConsumption
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "immediateConsumption").FirstOrDefault();
                //tempList = filter.OutputData.Where(x => x.MetricType == "IMMEDIATE CONSUMPTION").ToList();

                finalList = (from o in filter.OutputData
                             where o.MetricType == "IMMEDIATE CONSUMPTION"
                             select o.MetricData into temp
                             from m in temp
                             where m.Flag == 2
                             select m).FirstOrDefault();

                //finalList = (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).FirstOrDefault();
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "vm").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = finalList.Volume.ToString("#0") + "%";
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = getChangeValue(finalList.ChangeVolume);
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(finalList.Significance, filter.ss);
                //Fill pie chart
                plotBalloonPieChart(tempGroup, finalList);
                #endregion immediateConsumption
                #region timeSpent
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "timeSpent").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "TIME SPENT").ToList();
                finalList = (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).FirstOrDefault();
                tmpShape = ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault());
                tmpShape.TextFrame.Text = getChangeValue(finalList.ChangeVolume);
                tmpShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                tmpShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(finalList.Significance, filter.ss);
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "vm").FirstOrDefault()).TextFrame.Text = finalList.Volume.ToString("##0") + "%";
                tmpShape = ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "m").FirstOrDefault());
                setStylesTimeSpent(tmpShape, finalList, 7, -1, -1, 10, -1, -1);
                #endregion timeSpent

                #region DollarsSpent
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "DollarsSpent").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "DOLLARS SPENT").ToList();
                finalList = (from o in filter.OutputData
                             where o.MetricType == "DOLLARS SPENT"
                             select o.MetricData into temp
                             from m in temp
                             where m.Flag == 2
                             select m).FirstOrDefault();

                //finalList = (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).FirstOrDefault();
                tmpShape = ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault());
                tmpShape.TextFrame.Text = getChangeValue(finalList.ChangeVolume);
                tmpShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                tmpShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(finalList.Significance, filter.ss);
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "v").FirstOrDefault()).TextFrame.Text = "$" + finalList.Volume.ToString("##0");
                tmpShape = ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "vm").FirstOrDefault());
                setStyles(tmpShape, finalList.Metric, 6, 5, -1, -1, -1, -1);

                #endregion DollarsSpent
                #region NumberOfItems
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "NumberOfItems").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "NUMBER OF ITEMS").ToList();
                finalList = (from o in filter.OutputData
                             where o.MetricType == "NUMBER OF ITEMS"
                             select o.MetricData into temp
                             from m in temp
                             where m.Flag == 2
                             select m).FirstOrDefault();
                //finalList = (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).FirstOrDefault();
                setValuesForindMetricsWithVM(tempGroup, finalList, 0, false, true, filter.ss);
                #endregion NumberOfItems
                #region orderSummary
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "orderSummary").FirstOrDefault();
                tempList = filter.OutputData.Where(x => (x.MetricType == "ORDER SUMMARY" || x.MetricType == "ORDER SUMMARY-Nets")).ToList();
                finalData = (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).ToList();
                finalData.AddRange((tempList[1]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).ToList());
                setValuesForindMetricsWithVM(tempGroup, finalData[0], 1, false, true, filter.ss);
                loc = "~/Images/ishop-P2P-Icons/Top Items/" + replace_file_special_characters(finalData[0].Metric) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "Picture1").FirstOrDefault(), loc, context, 1, 0);
                setValuesForindMetricsWithVM(tempGroup, finalData[1], 2, false, true, filter.ss);
                loc = "~/Images/ishop-P2P-Icons/Top Items/" + replace_file_special_characters(finalData[1].Metric) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "Picture2").FirstOrDefault(), loc, context, 1, 0);
                setValuesForindMetricsWithVM(tempGroup, finalData[2], 3, false, true, filter.ss);
                loc = "~/Images/ishop-P2P-Icons/Top Items/" + replace_file_special_characters(finalData[2].Metric) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "Picture3").FirstOrDefault(), loc, context, 1, 0);
                #endregion orderSummary
                #region overallSatisfaction
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "overallSatisfaction").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "OVERALL SATISFACTION").ToList();
                finalList = (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).FirstOrDefault();
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "vm").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = finalList.Volume.ToString("#0") + "%";
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].Text = getChangeValue(finalList.ChangeVolume);
                ((IAutoShape)tempGroup.Shapes.Where(x => x.Name == "c").FirstOrDefault()).TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(finalList.Significance, filter.ss);
                //Fill pie chart
                plotBalloonPieChart(tempGroup, finalList);
                #endregion overallSatisfaction
                #region satisfaction
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "satisfaction").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "SATISFACTION DRIVERS").ToList();
                setValuesForSatisfaction(tempGroup, (tempList[0]).MetricData.Where(x => x.Metric == "Product Selection").FirstOrDefault(), 1, false, false, filter.ss);
                setValuesForSatisfaction(tempGroup, (tempList[0]).MetricData.Where(x => x.Metric == "Product Pricing").FirstOrDefault(), 2, false, false, filter.ss);
                setValuesForSatisfaction(tempGroup, (tempList[0]).MetricData.Where(x => x.Metric == "Service by Employees").FirstOrDefault(), 3, false, false, filter.ss);
                setValuesForSatisfaction(tempGroup, (tempList[0]).MetricData.Where(x => x.Metric == "Checkout Experience").FirstOrDefault(), 4, false, false, filter.ss);
                setValuesForSatisfaction(tempGroup, (tempList[0]).MetricData.Where(x => x.Metric == "Overall Store Atmosphere").FirstOrDefault(), 5, false, false, filter.ss);
                setValuesForSatisfaction(tempGroup, (tempList[0]).MetricData.Where(x => x.Metric == "Ease of Shopping").FirstOrDefault(), 6, false, false, filter.ss);
                #endregion satisfaction
                #region postvisit
                tempGroup = (IGroupShape)pres.Slides[0].Shapes.Where(x => x.Name == "postvist").FirstOrDefault();
                tempList = filter.OutputData.Where(x => x.MetricType == "LOCATION AFTER TRIP").ToList();
                setValuesForMetricsWithVM(tempGroup, (tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).ToList(), -1, false, true, filter.ss);
                //Replace Images
                loc = "~/Images/ishop-P2P-Icons/Location After Trip/" + replace_file_special_characters((tempList[0]).MetricData.Where(x => x.Selected_Popup_Metric_Item == true).FirstOrDefault().Metric) + ".svg";
                imageReplace((PictureFrame)tempGroup.Shapes.Where(x => x.Name == "img").FirstOrDefault(), loc, context, 1, 0);
                #endregion postvisit
                /*End*/

                if (filter.pptOrPdf == "pdf")
                {
                    //Check if office is installed
                    if (isPPT_Installed())
                    {
                        pres.Save(context.Server.MapPath("~/Temp/P2P_PDF"), Aspose.Slides.Export.SaveFormat.Pptx);
                    }
                    else
                    {
                        /*Delete ppt logo*/
                        pres.Masters[0].Shapes.Remove(pres.Masters[0].Shapes.Where(x => x.Name == "logo_for_ppt").FirstOrDefault());
                        //PdfOptions pdfop = new PdfOptions();
                        //pdfop.SufficientResolution = 10000;
                        //pdfop.EmbedFullFonts = true;
                        //pdfop.EmbedTrueTypeFontsForASCII = true;
                        //pdfop.TextCompression = PdfTextCompression.Flate;
                        //pdfop.JpegQuality = 100;
                        //pdfop.SaveMetafilesAsPng = true;
                        //pdfop.Compliance = PdfCompliance.Pdf15;

                        //pres.Save(destFile, Aspose.Slides.Export.SaveFormat.Pdf, pdfop);
                        pres.Save(destFile, Aspose.Slides.Export.SaveFormat.Pdf);
                    }
                }
                else
                {
                    pres.Masters[0].Shapes.Remove(pres.Masters[0].Shapes.Where(x => x.Name == "logo_for_pdf").FirstOrDefault());
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
                    var file = ppt.Open(context.Server.MapPath("~/Temp/P2P_PDF"), MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoFalse);
                    file.SaveCopyAs(destFile, interop.PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoTrue);
                }
            }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return "";
        }
        public void setValuesForMetrics(IGroupShape shp, List<PathToPurchaseEntity> data, int len, bool isUpper, bool updateMetrics, double? sampleSize)
        {
            if (len == -1)
            {
                setValuesForindMetrics(shp, data[0], 0, isUpper, updateMetrics, sampleSize);
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    setValuesForindMetrics(shp, data[i], i + 1, isUpper, updateMetrics, sampleSize);
                }
            }
        }
        public void setValuesForindMetrics(IGroupShape shp, PathToPurchaseEntity data, int indx, bool isUpper, bool updateMetrics, double? sampleSize)
        {
            IAutoShape tmp = ((IAutoShape)shp.Shapes.Where(x => x.Name == "c" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault());
            tmp.TextFrame.Text = getChangeValue(data.ChangeVolume);
            tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(data.Significance, sampleSize);
            if (updateMetrics == true)
            {
                ((IAutoShape)shp.Shapes.Where(x => x.Name == "m" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault()).TextFrame.Text = data.Metric.ToUpper();
            }
            else
            {
                ((IAutoShape)shp.Shapes.Where(x => x.Name == "m" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault()).TextFrame.Text = data.Metric;
            }
            ((IAutoShape)shp.Shapes.Where(x => x.Name == "v" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault()).TextFrame.Text = data.Volume.ToString("##0") + "%";
        }

        public void setValuesForMetricsWithVM(IGroupShape shp, List<PathToPurchaseEntity> data, int len, bool isUpper, bool updateMetrics, double? sampleSize)
        {
            if (len == -1)
            {
                setValuesForindMetricsWithVM(shp, data[0], 0, isUpper, updateMetrics, sampleSize);
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    setValuesForindMetricsWithVM(shp, data[i], i + 1, isUpper, updateMetrics, sampleSize);
                }
            }
        }
        public void setValuesForMetricsWithVMSDynamic(IGroupShape shp, PathToPurchaseEntity data, int len, bool isUpper, bool updateMetrics, double? sampleSize)
        {
            if (len == -1)
            {
                setValuesForindMetricsWithVM(shp, data, 0, isUpper, updateMetrics, sampleSize);
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    setValuesForindMetricsWithVM(shp, data, i + 1, isUpper, updateMetrics, sampleSize);
                }
            }
        }
        public void setValuesForindMetricsWithVM(IGroupShape shp, PathToPurchaseEntity data, int indx, bool isUpper, bool updateMetrics, double? sampleSize)
        {
            IAutoShape tmp = ((IAutoShape)shp.Shapes.Where(x => x.Name == "c" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault());
            tmp.TextFrame.Text = getChangeValue(data.ChangeVolume);
            tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(data.Significance, sampleSize);
            tmp = ((IAutoShape)shp.Shapes.Where(x => x.Name == "vm" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault());
            tmp.TextFrame.Paragraphs[0].Portions[0].Text = data.MetricType == "NUMBER OF ITEMS" ? data.Volume.ToString("##0.0") : (data.Volume.ToString("##0") + "%");
            tmp.TextFrame.Paragraphs[1].Portions[0].Text = data.Metric;
        }
        public Color getColorBasedOnSignificance(double? i)
        {
            if (i == null) return Color.Black;
            if (i < -1.96) return Color.Red;
            if (i > 1.96) return Color.Green;
            return Color.Black;
        }

        #region plotBalloonPieChart
        public void plotBalloonPieChart(IGroupShape shp, PathToPurchaseEntity data)
        {
            IChart tmp = ((IChart)shp.Shapes.Where(x => x.Name == "chart").FirstOrDefault());
            tmp.ChartData.Series[0].DataPoints[0].Value.Data = Math.Round(data.Volume);
            tmp.ChartData.Series[0].DataPoints[1].Value.Data = 100 - Math.Round(data.Volume);
        }
        #endregion plotBalloonPieChart

        public void setValuesForSatisfaction(IGroupShape shp, PathToPurchaseEntity data, int indx, bool isUpper, bool updateMetrics, double? sampleSize)
        {
            IAutoShape tmp = ((IAutoShape)shp.Shapes.Where(x => x.Name == "c" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault());
            tmp.TextFrame.Text = getChangeValue(data.ChangeVolume);
            tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
            tmp.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = getColorBasedOnSignificanceForPopup(data.Significance, sampleSize);
            tmp = ((IAutoShape)shp.Shapes.Where(x => x.Name == "vm" + (indx == 0 ? "" : indx.ToString())).FirstOrDefault());
            tmp.TextFrame.Paragraphs[2].Portions[0].Text = data.Volume.ToString("##0") + "%";
        }
        public void setStyles(IAutoShape tmpShape, string text, int fontSize, int marginTop, int marginRight, int marginBottom, int marginLeft, int width)
        {
            tmpShape.TextFrame.Text = text;
            if (width != -1) tmpShape.Width = width;
            if (fontSize != -1) tmpShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = fontSize;
            if (marginTop != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginTop;
            if (marginLeft != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginLeft;
            if (marginBottom != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginBottom;
            if (marginRight != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginRight;

        }
        public void setStylesTimeSpent(IAutoShape tmpShape, PathToPurchaseEntity filter, int fontSize, int marginTop, int marginRight, int marginBottom, int marginLeft, int width)
        {
            if (filter.Metric.ToLower().IndexOf("or") > -1)
            {
                tmpShape.TextFrame.Text = filter.Metric.ToUpper();
            }
            else
            {
                string min = filter.Metric.ToUpper().ToString().Replace("MIN", "").Trim();
                tmpShape.TextFrame.Paragraphs[0].Portions[0].Text = min + "\n MIN";
                //tmpShape.TextFrame.Paragraphs[0].Portions[1].Text = "MIN";
            }
            if (width != -1) tmpShape.Width = width;
            if (fontSize != -1) tmpShape.TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = fontSize;
            if (marginTop != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginTop;
            if (marginLeft != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginLeft;
            if (marginBottom != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginBottom;
            if (marginRight != -1) tmpShape.TextFrame.TextFrameFormat.MarginBottom = marginRight;

        }
        public string replace_file_special_characters(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
                filename = Regex.Replace(filename, "[&/\\#,+()$~%.':*?<>{}]+", "-");
            return filename;
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
