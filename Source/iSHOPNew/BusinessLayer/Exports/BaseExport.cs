using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Slides;
using System.Data;
using iSHOPNew.DAL;
using Aspose.Slides.Charts;
//using DocumentFormat.OpenXml.Office2010.Excel;
//using DocumentFormat.OpenXml.Drawing;
using System.Drawing;

namespace iSHOPNew.BusinessLayer.Exports
{
    public abstract class BaseExport
    {
        public Presentation pres = null;
        public ISlideCollection slds = null;
        public int SlideNumber = 0;
        public string filename = string.Empty;
        IChart chart = null;
        IChartSeries Series = null;
        IDataLabel lbl = null;
        public System.Data.DataTable chart_table = null;
        public List<string> seriesList = new List<string>();
        public List<string> metrics = null;
        public List<string> metriclist = null;
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public string benchmark = string.Empty;
        public double StatTesting;
        int serCount = 1, catcount = 1;

        #region abstract functions
        public abstract string PrepareSlides(DataSet ds, EstablishmentDeepDiveParams param);
        #endregion
        public void BaseReport()
        {
            StatTesting = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);
            accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
            accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
        }
        public void LoadSlides(string filePath)
        {
            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            pres = new Presentation(filePath);
            slds = pres.Slides;
        }
        public string SaveFile()
        {
            filename = "iSHOP_PPTGenerator_" + GlobalVariables.GetRandomNumber;
            pres.Save(HttpContext.Current.Server.MapPath("~/Temp/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);
            return "~/Temp/" + filename + ".pptx";//HttpContext.Current.Server.MapPath("~/Temp/" + filename + ".pptx");
        }

        public void ReplaceFooterTable(ISlide sld, System.Data.DataTable tbl,string tableName)
        {
            IShape _shp = sld.Shapes.Where(x => x.Name == tableName).FirstOrDefault();
            if (_shp != null)
            {
                Aspose.Slides.ITable footerTable = (Aspose.Slides.ITable)_shp;
                var metricList = (from r in tbl.AsEnumerable() select Convert.ToString(r["Metric"])).Distinct().ToList();
                var values = (from r in tbl.AsEnumerable() select Convert.ToString(r["Volume"])).ToList();
                var share = (from r in tbl.AsEnumerable() select Convert.ToString(r["Share"])).ToList();
                var changePercentage = (from r in tbl.AsEnumerable() select Convert.ToString(r["ChangePercentage"])).ToList();

                var count = 0;
                var width = footerTable.Width/metricList.Count;   

                foreach (var metric in metricList)
                {
                    footerTable[count, 0].TextFrame.Text = metric;
                    if(count==0 || count == metricList.Count - 1)
                    {
                        footerTable[count, 0].TextFrame.Text += "\n(" + String.Format("{0:#,##0}", Convert.ToDouble(values[count])) + ")";
                    }
                    if (count != 0 )
                    {
                        if(count == metricList.Count - 1)
                        {
                            footerTable[count, 1].TextFrame.Text = "";
                            footerTable[count, 2].TextFrame.Text = "";
                        }
                        else
                        {
                            footerTable[count, 1].TextFrame.Text = String.Format("{0:0.0%}", Convert.ToDouble(changePercentage[count]) / 100);
                            footerTable[count, 2].TextFrame.Text = String.Format("{0:0.0%}", Convert.ToDouble(share[count]) / 100);
                        }
                    }
                    footerTable.Columns[count].Width = width;
                    count = count + 1;
                }

                for (var i = count; i < footerTable.Columns.Count;)
                {
                    footerTable.Columns.RemoveAt(i, false);
                }   
            }
        }
        public void ReplaceWaterFallChart(ISlide sld, System.Data.DataTable tbl)
        {
            chart = (IChart)sld.Shapes.Where(x => x.Name == "Chart").FirstOrDefault();
            chart.ChartTitle.Overlay = false;
            chart.HasTitle = false;
            
            chart.Axes.HorizontalAxis.IsVisible = false;
            chart.Axes.VerticalAxis.IsVisible = true;

            chart.Axes.VerticalAxis.MaxValue = GetAxisMaxValue(tbl);
            chart.Axes.VerticalAxis.MinValue = 0;

            chart.Axes.VerticalAxis.NumberFormat = "0%";
            chart.Axes.VerticalAxis.IsNumberFormatLinkedToSource = true;

            int defaultWorksheetIndex = 0;

            //Getting the chart data worksheet
            IChartDataWorkbook fact = chart.ChartData.ChartDataWorkbook;

            //Delete default generated series and categories
            chart.ChartData.Series.Clear();
            chart.ChartData.Categories.Clear();
            chart.ChartData.ChartDataWorkbook.Clear(0);

            int s = chart.ChartData.Series.Count;
            metriclist = (from r in tbl.AsEnumerable() select Convert.ToString(r["Metric"])).Distinct().ToList();
            seriesList = (from r in tbl.AsEnumerable() select Convert.ToString(r["Retailer"])).Distinct().ToList();

            for (int i = 0; i < seriesList.Count; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i + 1, seriesList[i]), chart.Type);
                chart.ChartData.Series[i].ParentSeriesGroup.Overlap = 100;
            }

            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));
            }

            serCount = 1;
            catcount = 1;

            foreach (string item in seriesList)
            {
                Series = chart.ChartData.Series[serCount - 1];

                foreach (string series in metriclist)
                {
                    var query = (from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Retailer")).Equals(item, StringComparison.OrdinalIgnoreCase)
                                 where Convert.ToString(r.Field<object>("Metric")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = (catcount == metriclist.Count ? 1.0 : 0.0) + ((string.IsNullOrEmpty(r["DisplayValue"].ToString()) ? (double?)null : Convert.ToDouble(r["DisplayValue"]))/100),
                                     retailer = r["Retailer"].ToString(),
                                     sampleSize = string.IsNullOrEmpty(r["SampleSize"].ToString()) ? (double?)null : Convert.ToDouble(r["SampleSize"]),
                                     displayValue = string.IsNullOrEmpty(r["DisplayValue"].ToString()) ? (double?)null : Convert.ToDouble(r["DisplayValue"]),
                                 }).FirstOrDefault();

                    Series.DataPoints.AddDataPointForWaterfallSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? (Convert.ToDouble(query.value)) : 0)));

                    Series.DataPoints[catcount - 1].Format.Fill.FillType = FillType.Solid;
                    Series.DataPoints[catcount - 1].Format.Fill.SolidFillColor.Color = (catcount == 1 || catcount == metriclist.Count) ? Color.Orange : (query.value >= 0 ? Color.Green : Color.Red);

                    if(catcount == 1 || catcount == metriclist.Count)
                    {
                        Series.DataPoints[catcount - 1].SetAsTotal = true;
                    }

                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0%";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0%";


                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.ShowPercentage = true;
                    lbl.DataLabelFormat.ShowValue = true;

                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(string.Empty, query.sampleSize, string.Empty);

                    catcount++;
                    //Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                    //Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = true;
                }
                catcount = 1;
                serCount++;
            }
            ReplaceFooterTable(sld, tbl, "HAxisTable");
            lbl = chart.ChartData.Series[0].DataPoints[0].Label;
        }
        public void ReplaceCustomWaterFallChart(ISlide sld, System.Data.DataTable tbl)
        {
            chart = (IChart)sld.Shapes.Where(x => x.Name == "Chart").FirstOrDefault();
            chart.ChartTitle.Overlay = false;
            chart.HasTitle = false;
            System.Data.DataTable tb = GetWaterFallChartTypeData(tbl);


            chart.Axes.HorizontalAxis.IsVisible = true;
            chart.Axes.VerticalAxis.IsVisible = true;
    
            chart.Axes.VerticalAxis.MaxValue = GetAxisMaxValue(tbl);
            chart.Axes.VerticalAxis.MinValue = 0;

            chart.Axes.VerticalAxis.NumberFormat = "0";
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

            int s = chart.ChartData.Series.Count;
            metriclist = (from r in tb.AsEnumerable() select Convert.ToString(r["Metric"])).Distinct().ToList();
            seriesList = (from r in tb.AsEnumerable() select Convert.ToString(r["Objective"])).Distinct().ToList();

            for (int i = 0; i < seriesList.Count; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, i + 1, seriesList[i]), chart.Type);
                chart.ChartData.Series[i].ParentSeriesGroup.Overlap = 100;
            }

            for (int i = 0; i < metriclist.Count - 1; i++)
            {
                chart.ChartData.Series.Add(fact.GetCell(defaultWorksheetIndex, 0, seriesList.Count + i + 1, "Line"+ (i+1).ToString()), Aspose.Slides.Charts.ChartType.Line);
            }

            //chart.ChartData.Series.Add(fact.GetCell(0, 0, 1, "Series 1"), chart.Type);

            for (int i = 0; i < metriclist.Count; i++)
            {
                //Setting Category Name
                chart.ChartData.Categories.Add(fact.GetCell(defaultWorksheetIndex, i + 1, 0, metriclist[i]));

            }


            serCount = 1;
            catcount = 1;

            foreach (string item in seriesList)
            {
                Series = chart.ChartData.Series[serCount - 1];
                //Series.Labels.DefaultDataLabelFormat.ShowValue = true;
                //Series.Labels.DefaultDataLabelFormat.IsNumberFormatLinkedToSource = false;

                if(serCount == 1)
                {
                    Series.Format.Fill.FillType = FillType.NoFill;
                }
                //Series.Format.Fill.SolidFillColor.Color = GetSeriesColor(serCount - 1);

                //Series.Format.Fill.FillType = FillType.Gradient;
                //Series.Format.Fill.GradientFormat.GradientShape = GradientShape.Linear;
                //Series.Format.Fill.GradientFormat.GradientDirection = GradientDirection.FromCenter;
                //Series.Format.Fill.GradientFormat.LinearGradientAngle = 90;

               

                foreach (string series in metriclist)
                {
                    var query = (from r in tb.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")).Equals(item, StringComparison.OrdinalIgnoreCase)
                                 where Convert.ToString(r.Field<object>("Metric")).Equals(series, StringComparison.OrdinalIgnoreCase)
                                 select new
                                 {
                                     value = r["Volume"],
                                     colorSelector = r["IsNegative"].ToString(),
                                     sampleSize = string.IsNullOrEmpty(r["SampleSize"].ToString()) ? (double?)null : Convert.ToDouble(r["SampleSize"]),
                                 }).FirstOrDefault();
                   
                    
                    Series.DataPoints.AddDataPointForBarSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? (Convert.ToDouble(query.value)) : 0)));

                    if (serCount !=1)
                    {
                        Series.DataPoints[catcount-1].Format.Fill.FillType = FillType.Solid;
                        Series.DataPoints[catcount-1].Format.Fill.SolidFillColor.Color = query.colorSelector == "0" ? Color.Green : (query.colorSelector == "1" ? Color.Red : Color.Orange);
                    }

                    Series.Labels.DefaultDataLabelFormat.NumberFormat = "0.0";
                    Series.DataPoints[catcount - 1].Value.AsCell.CustomNumberFormat = "0.0";

                    ////Set Data Point Label Style
                    lbl = Series.DataPoints[catcount - 1].Label;
                    lbl.DataLabelFormat.Position = LegendDataLabelPosition.InsideEnd;
                    var k = lbl.Y;
                    //lbl.DataLabelFormat.Position = query.colorSelector == "0" ? LegendDataLabelPosition.Top : (query.colorSelector == "1" ? LegendDataLabelPosition.Bottom : LegendDataLabelPosition.Top);
                    lbl.DataLabelFormat.ShowValue = serCount != 1 ? true : false;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.FillType = FillType.Solid;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FontBold = NullableBool.True;
                    lbl.DataLabelFormat.TextFormat.PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(string.Empty, query.sampleSize, string.Empty);
                    //LabelFontSize(lbl);
                    catcount++;
                }
                catcount = 1;
                serCount++;
            }
            

            for (int i = 0; i < metriclist.Count - 1; i++)
            {
                Series = chart.ChartData.Series[serCount - 1];

                var query = (from r in tb.AsEnumerable()
                             where Convert.ToString(r.Field<object>("Objective")).Equals(seriesList[0], StringComparison.OrdinalIgnoreCase)
                             where Convert.ToString(r.Field<object>("Metric")).Equals(metriclist[i], StringComparison.OrdinalIgnoreCase)
                             select new
                             {
                                 value = r["Cumulative"],
                             }).FirstOrDefault();

                for (int j=0;j < metriclist.Count; j++)
                {
                   
                    if(catcount - i - 1 <= 1 && catcount - i - 1 >= 0)
                    {
                        Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, (!string.IsNullOrEmpty(Convert.ToString(query.value)) ? (Convert.ToDouble(query.value)) : 0)));
                    }
                    else
                    {
                        Series.DataPoints.AddDataPointForLineSeries(fact.GetCell(defaultWorksheetIndex, catcount, serCount, DBNull.Value));
                    }
                    if(catcount - i - 1 == 1)
                    {
                        Series.DataPoints[j].Format.Line.FillFormat.FillType = FillType.Solid;
                        Series.DataPoints[j].Format.Line.FillFormat.SolidFillColor.Color = Color.Black;
                        //Series.DataPoints[j].Format.Line.DashStyle = LineDashStyle.Dot;
                        Series.DataPoints[j].Format.Line.Width = 0.2;
                    }
                    else
                    {
                        Series.DataPoints[j].Format.Line.FillFormat.FillType = FillType.Solid;
                        Series.DataPoints[j].Format.Line.FillFormat.SolidFillColor.Color = Color.Transparent;
                    }
                    catcount++;
                }

                catcount = 1;
                serCount++;
            }
        }
        private System.Data.DataTable GetWaterFallChartTypeData(System.Data.DataTable tbl)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Objective");
            dt.Columns.Add("Metric");
            dt.Columns.Add("Volume");
            dt.Columns.Add("IsNegative");
            dt.Columns.Add("SampleSize");
            dt.Columns.Add("Cumulative");
            var cumulative = 0.0;
            int cnt = 0;
            int cntRows = tbl.Rows.Count;
            foreach (DataRow row in tbl.Rows)
            {
                cnt++;
                if(cnt == cntRows)
                {
                    break;
                }
                string name = row["Metric"].ToString();
                double value = Convert.ToDouble((row["Volume"]));
                if(value > 0)
                {
                    if(cnt == 1)
                    {
                        dt.Rows.Add("Series 1", name, cumulative, 2, row["SampleSize"], cumulative + value);
                        dt.Rows.Add("Series 2", name, value, row["SampleSize"], cumulative + value);
                    }
                    else
                    {
                        dt.Rows.Add("Series 1", name, cumulative, 0, row["SampleSize"], cumulative + value);
                        dt.Rows.Add("Series 2", name, value, 0, row["SampleSize"], cumulative + value);
                    }      
                    cumulative += value;
                }else
                {
                    cumulative += value;
                    if (cnt == 1)
                    {
                        dt.Rows.Add("Series 1", name, cumulative, 2, row["SampleSize"], cumulative);
                        dt.Rows.Add("Series 2", name, -value, 2, row["SampleSize"], cumulative );
                    }
                    else
                    {
                        dt.Rows.Add("Series 1", name, cumulative, 1, row["SampleSize"], cumulative);
                        dt.Rows.Add("Series 2", name, -value, 1, row["SampleSize"], cumulative);
                    }
                }
            }
            DataRow lastRow = tbl.Rows[tbl.Rows.Count - 1];
            dt.Rows.Add("Series 1",lastRow["Metric"],0,2, lastRow["SampleSize"], cumulative + 0);
            dt.Rows.Add("Series 2", lastRow["Metric"], lastRow["Volume"],2, lastRow["SampleSize"], cumulative + Convert.ToDouble(lastRow["Volume"]));
            return dt;
        }
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
        private System.Drawing.Color GetSignificanceColor(string significancevalue, double? samplesize, string objective)
        {
            System.Drawing.Color color = System.Drawing.Color.Black;
            if (!string.IsNullOrEmpty(significancevalue))
            {
                if (!string.IsNullOrEmpty(benchmark) && benchmark.Equals(objective, StringComparison.OrdinalIgnoreCase))
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
        private System.Drawing.Color GetSeriesColor(int SeriesIndex)
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
                    //color = Color.FromArgb(Red.Next(0, 112), rnd.Next(0, 150), rnd.Next(0, 100));
                    break;

            }
            return color;
        }

    }
}