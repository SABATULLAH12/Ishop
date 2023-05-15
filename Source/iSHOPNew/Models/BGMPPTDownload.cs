using Aspose.Slides;
using Aspose.Slides.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Globalization;
using iSHOPNew.DAL;

namespace iSHOPNew.Models
{

    public class BGMPPTDownload
    {
        string selectedProduct = null;
        bool NonBeverageItem = false;
        CommonFunctions commonfunctions = new CommonFunctions();
        DataSet ds = null;
        System.Data.DataTable tbl = null;
        Presentation pres = new Presentation(HttpContext.Current.Server.MapPath("~/ISHOPBGM PPT Export Files/ISHOPBGMPPTTemplate.pptx")); // creates a blank presentation with one blank slide.  must be done first
        ISlideCollection slds;
        BGMParams bgmParams = null;
        int rowNumber = 1;
        int colNumber = 1;
        List<string> Products = null;
        List<string> Metrics = new List<string>() { "MARKET STATS", "RETAILER STATS", "PRODUCT STATS" };
        string ChangeBasis = string.Empty;
        string ImpactToSales = string.Empty;
        string ShopperSampleSize = string.Empty;
        string TripsSampleSize = string.Empty;
        string Benchmark = string.Empty;
        string MetricItem = string.Empty;
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;

        private string FormateDateAndTime(string month)
        {
            if (month.Length == 1)
            {
                return "0" + month;
            }
            else
                return month;
        }
        public void GeneratePPT(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {          
            if (HttpContext.Current.Session["BGMData"] != null)
            {
                bgmParams = HttpContext.Current.Session["BGMData"] as BGMParams;
            }
            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            if (!string.IsNullOrEmpty(bgmParams.statpositive))
                accuratestatvalueposi = Convert.ToDouble(bgmParams.statpositive);
            if (!string.IsNullOrEmpty(bgmParams.statnegative))
                accuratestatvaluenega = Convert.ToDouble(bgmParams.statnegative);

            ds = bgmParams.Data_Set;
            Benchmark = bgmParams.BenchMark;
            slds = pres.Slides;
            ISlide sld = pres.Slides[0];
          
            if (ds != null && ds.Tables.Count > 0)
            {
                rowNumber = 0;
                Products = (from row in ds.Tables[1].AsEnumerable()
                            select Convert.ToString(row.Field<object>("Product"))).Distinct().ToList();
                if (Products != null && Products.Count > 0)
                {
                    for (int i = 0; i < Products.Count;i++ )
                    {
                        UpdateInputSelectionTable(sld, Products[i]);
                        UpdateSampleSizeTable(sld, Products[i]);
                        UpdateProductTable(sld, Products[i]);
                        if (Products.Count - 1 > i)
                        {
                            slds.AddClone(pres.Slides[0]);
                            sld = slds[i + 1];
                        }
                    }
                }
            }

            string filename = "Shopper_Metrics" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds));

            pres.Save(HttpContext.Current.Server.MapPath("~/ISHOPBGM PPT Export Files/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);

            FileStream fs1 = null;
            fs1 = System.IO.File.Open(HttpContext.Current.Server.MapPath("~/ISHOPBGM PPT Export Files/Downloads/" + filename + ".pptx"), System.IO.FileMode.Open);

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

        public void GenerateReportGeneratorPPT(string timePeriod, string retailerChannel, string geography, string shopperGroup, string ShopperFrequency, string filter)
        {
            if (HttpContext.Current.Session["ReportGeneratorData"] != null)
            {
                bgmParams = HttpContext.Current.Session["ReportGeneratorData"] as BGMParams;
            }
            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            if (!string.IsNullOrEmpty(bgmParams.statpositive))
                accuratestatvalueposi = Convert.ToDouble(bgmParams.statpositive);
            if (!string.IsNullOrEmpty(bgmParams.statnegative))
                accuratestatvaluenega = Convert.ToDouble(bgmParams.statnegative);
            
            slds = pres.Slides;
            ISlide sld = pres.Slides[0];
            ds = bgmParams.Data_Set;
            if (ds != null && ds.Tables.Count > 0)
            {
                rowNumber = 0;
                Products = (from row in ds.Tables[1].AsEnumerable()
                            select Convert.ToString(row.Field<object>("Product"))).Distinct().ToList();
                if (Products != null && Products.Count > 0)
                {
                    for (int i = 0; i < Products.Count; i++)
                    {
                        UpdateInputSelectionTable(sld, Products[i]);
                        UpdateSampleSizeTable(sld, Products[i]);
                        UpdateProductTable(sld, Products[i]);
                        if (Products.Count - 1 > i)
                        {
                            slds.AddClone(pres.Slides[0]);
                            sld = slds[i + 1];
                        }
                    }
                }
            }

            //string filename = "BGM" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds));
            string filename = "ReportGenerator" + timePeriod+System.DateTime.Now;

            pres.Save(HttpContext.Current.Server.MapPath("~/ISHOPBGM PPT Export Files/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);

            FileStream fs1 = null;
            fs1 = System.IO.File.Open(HttpContext.Current.Server.MapPath("~/ISHOPBGM PPT Export Files/Downloads/" + filename + ".pptx"), System.IO.FileMode.Open);

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

        #region Update Sample Size
        private void UpdateSampleSizeTable(ISlide slide, string Product)
        {
            ITable datatbl = null;
            foreach (IShape shp in slide.Shapes)
            {
                if (shp is ITable)
                {
                    if (shp.Name == "Table 7")
                    {
                        datatbl = (ITable)shp;
                        ShopperSampleSize = (from row in ds.Tables[0].AsEnumerable()
                                             where
                                                 Convert.ToString(row.Field<object>("Objective")).Equals(Product, StringComparison.OrdinalIgnoreCase)
                                                   && Convert.ToString(row.Field<object>("Section")).Equals("Shopper", StringComparison.OrdinalIgnoreCase)
                                             select Convert.ToString(row.Field<object>("SampleSize"))).FirstOrDefault();

                        TripsSampleSize = (from row in ds.Tables[0].AsEnumerable()
                                           where
                                               Convert.ToString(row.Field<object>("Objective")).Equals(Product, StringComparison.OrdinalIgnoreCase)
                                                 && Convert.ToString(row.Field<object>("Section")).Equals("Product Trips", StringComparison.OrdinalIgnoreCase)
                                           select Convert.ToString(row.Field<object>("SampleSize"))).FirstOrDefault();

                        if (Convert.ToString(Convert.ToString(ds.Tables[1].Rows[0]["flag"])).Equals("Non-Priority", StringComparison.OrdinalIgnoreCase))
                            datatbl[1, 1].TextFrame.Text = GlobalVariables.NA;
                        else if (!string.IsNullOrEmpty(ShopperSampleSize) && ShopperSampleSize == "0")
                            datatbl[1, 1].TextFrame.Text = "0";
                        else
                        datatbl[1, 1].TextFrame.Text = !string.IsNullOrEmpty(ShopperSampleSize) ? Convert.ToDouble(ShopperSampleSize).ToString("#,#", CultureInfo.InvariantCulture):string.Empty;

                        if (!string.IsNullOrEmpty(TripsSampleSize) && TripsSampleSize == "0")
                            datatbl[1, 2].TextFrame.Text = "0";
                        else
                        datatbl[1, 2].TextFrame.Text = !string.IsNullOrEmpty(TripsSampleSize) ? Convert.ToDouble(TripsSampleSize).ToString("#,#", CultureInfo.InvariantCulture) : string.Empty; 
                    }
                }
            }
        }
        #endregion
        #region Update Input Selection
        private void UpdateInputSelectionTable(ISlide slide, string Product)
        {
            ITable datatbl = null;
            foreach (IShape shp in slide.Shapes)
            {
                if (shp is ITable)
                {
                    if (shp.Name == "Table 8")
                    {
                        datatbl = (ITable)shp;


                        datatbl[1, 0].TextFrame.Text = bgmParams.TimePeriod;
                        datatbl[1, 1].TextFrame.Text = bgmParams.BenchMarkShortName;
                        datatbl[1, 2].TextFrame.Text = Product;
                        datatbl[1, 3].TextFrame.Text = bgmParams.ShopperFrequency;
                        datatbl[1, 4].TextFrame.Text = string.IsNullOrEmpty(bgmParams.FilterShortNames) ? "NONE" : bgmParams.FilterShortNames;
                    }
                }
                else if (shp is IAutoShape)
                {
                    if (shp.Name == "New shape")
                    {
                        IAutoShape ashp = (IAutoShape)shp;
                        ITextFrame txtFrame = ashp.TextFrame;
                        foreach (IParagraph para in txtFrame.Paragraphs)
                        {
                            foreach (IPortion portion in para.Portions)
                            {
                                if(Convert.ToString(portion.Text).IndexOf("95") > -1)
                                    portion.Text = portion.Text.Replace("95",bgmParams.PercentStat);
                            }
                        }
                    }
                }
            }
        }
        #endregion
        private void UpdateProductTable(ISlide slide,string Product)
        {
            try
            {
                selectedProduct = Product;
                NonBeverageItem = false;
                ITable datatbl = null;
                foreach (IShape shp in slide.Shapes)
                {
                    if (shp is ITable)
                    {
                        if (shp.Name == "Table 3")
                        {
                            datatbl = (ITable)shp;
                            #region add header
                            rowNumber = 0;
                            colNumber = 3;
                            datatbl[colNumber, 0].TextFrame.Text = bgmParams.PreviousTimePeriod;
                            colNumber++;
                            datatbl[colNumber, 0].TextFrame.Text = bgmParams.TimePeriod;
                            colNumber++;
                            datatbl[colNumber, 0].TextFrame.Text = "Change vs. YAGO";
                            colNumber++;
                            datatbl[colNumber, 0].TextFrame.Text = "Change Basis";
                            colNumber++;
                            datatbl[colNumber, 0].TextFrame.Text = "Impact to Sales";
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                rowNumber = 0;
                                if (!string.IsNullOrEmpty(Product))
                                {
                                    foreach (string Metric in Metrics)
                                    {
                                        rowNumber++;
                                        datatbl[0, rowNumber].TextFrame.Text = Metric.Replace(" ", "\n");
                                        var querystring = (from row in ds.Tables[1].AsEnumerable()
                                                           where
                                                               Convert.ToString(row.Field<object>("Product")).Equals(Product, StringComparison.OrdinalIgnoreCase)
                                                               && Convert.ToString(row.Field<object>("Metric")).Equals(Metric, StringComparison.OrdinalIgnoreCase)
                                                           select row);

                                        tbl = querystring.CopyToDataTable();
                                        if (tbl != null && tbl.Rows.Count > 0)
                                        {
                                            if (Convert.ToInt16(tbl.Rows[0]["IsBeverage"]) == 0)
                                            {
                                                NonBeverageItem = true;
                                            }
                                            for (int i = 0; i < tbl.Rows.Count; i++)
                                            {
                                                colNumber = 2;
                                                MetricItem = Convert.ToString(tbl.Rows[i]["MetricItem"]);
                                                ChangeBasis = GetChangeBasis(Convert.ToInt32(tbl.Rows[i]["rowNum"]));
                                                ImpactToSales = GetImpactToSalesValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Impact to Sales"]));

                                                datatbl[colNumber, rowNumber].TextFrame.Text = AddMetricItem(Convert.ToString(tbl.Rows[i]["MetricItem"]));
                                                colNumber++;

                                                datatbl[colNumber, rowNumber].TextFrame.Text = GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Previous Year"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]), Convert.ToString(tbl.Rows[i]["Previous Year Sample"]));
                                                datatbl[colNumber, rowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                                                datatbl[colNumber, rowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), true, Convert.ToInt32(tbl.Rows[i]["rowNum"]));

                                                datatbl[colNumber, rowNumber].FillFormat.SolidFillColor.Color = ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SamplePY"]));
                                                colNumber++;

                                                datatbl[colNumber, rowNumber].TextFrame.Text = GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Current Year"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]), Convert.ToString(tbl.Rows[i]["Previous Year Sample"]));
                                                datatbl[colNumber, rowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                                                datatbl[colNumber, rowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), false, Convert.ToInt32(tbl.Rows[i]["rowNum"]));
                                                datatbl[colNumber, rowNumber].FillFormat.SolidFillColor.Color = ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SampleCY"]));
                                                colNumber++;

                                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                                    datatbl[colNumber, rowNumber].TextFrame.Text = "-";
                                                else
                                                    datatbl[colNumber, rowNumber].TextFrame.Text = AddPlusSymble(Convert.ToString(tbl.Rows[i]["Change vs. YAGO"])) + Convert.ToString(tbl.Rows[i]["Change vs. YAGO"]) + (ChangeBasis.Equals("Percentage", StringComparison.OrdinalIgnoreCase) ? "%" : string.Empty);

                                                colNumber++;

                                                datatbl[colNumber, rowNumber].TextFrame.Text = ChangeBasis;
                                                colNumber++;
                                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                                    datatbl[colNumber, rowNumber].TextFrame.Text = "-";
                                                else
                                                    datatbl[colNumber, rowNumber].TextFrame.Text = ImpactToSales;

                                                colNumber++;
                                                rowNumber++;
                                            }
                                        }
                                    }
                                }
                                datatbl[1, 16].TextFrame.Text = "Source:  Coca-Cola iSHOP Shopper Metrics, " + bgmParams.TimePeriod + " vs. YAGO, " + bgmParams.ShopperFrequency + ", " + (string.IsNullOrEmpty(bgmParams.FilterShortNames) ? "All Demographics" : bgmParams.FilterShortNames) + ", " + bgmParams.BenchMarkShortName + " Visits";
                            }
                            #endregion
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
        private string AddMetricItem(string MetricItem)
        {
            switch (MetricItem.Trim().ToLower())
            {
                case "% of shoppers, monthly+ product buyers":
                    {
                        //MetricItem = MetricItem + "*";
                        break;
                    }
            }
            return MetricItem;
        }
        public string AddPlusSymble(string value)
        {
            string _symble = string.Empty;
            if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) > 0)
                _symble = "+";
            return _symble;
        }
        public bool IsNonPriorityMetric(string PriorityType, string Metric)
        {
            bool NonPriorityMetric = false;
            if (!string.IsNullOrEmpty(PriorityType) && PriorityType.Equals("Non-Priority", StringComparison.OrdinalIgnoreCase))
            {
                switch (Convert.ToString(Metric).Trim().ToLower())
                {
                    case "% of shopper population in trade area":
                    case "shopper reach of retailer (% of trade area)":
                    case "retailer trips per shopper, any item":
                    case "number of retailer shoppers":
                    case "product trips per shopper":
                    case "% of shoppers, monthly+ product buyers":
                        {
                            NonPriorityMetric = true;
                            break;
                        }
                }
            }
            return NonPriorityMetric;
        }
        public string GetTimePeriodValue(int Rownumber, string value, string samplesize, string PriorityType, string Metric, string sampleSizePy)
        {
            string TimePeriodValue = value;
            //if ((NonBeverageItem && Metric.Equals("% of Shoppers, Monthly+ Product Buyers", StringComparison.OrdinalIgnoreCase))
            //     || (selectedProduct.Equals("Total Beverage Trips", StringComparison.OrdinalIgnoreCase)
            //     && Metric.Equals("% of Shoppers, Monthly+ Product Buyers", StringComparison.OrdinalIgnoreCase)))
            if ((NonBeverageItem && Metric.Equals("% of Shoppers, Monthly+ Product Buyers", StringComparison.OrdinalIgnoreCase))
               || (!string.IsNullOrEmpty(sampleSizePy) && Convert.ToDouble(sampleSizePy) == GlobalVariables.NANumber)
               && Metric.Equals("% of Shoppers, Monthly+ Product Buyers", StringComparison.OrdinalIgnoreCase))
            {
                TimePeriodValue = GlobalVariables.NA;
            }
            else if (IsNonPriorityMetric(PriorityType, Metric))
            {
                TimePeriodValue = GlobalVariables.NA;
            }
            else if (IsChannelMetric(Benchmark, Metric))
            {
                TimePeriodValue = GlobalVariables.NA;
            }  
            else if (IsLowSampleSize(samplesize))
            {
                TimePeriodValue = "(LOW SAMPLE)";
            }
            else
            {
                switch (Rownumber)
                {
                    case 7:
                    case 12:
                    case 13:
                        {
                            if (!string.IsNullOrEmpty(value))
                                TimePeriodValue = Convert.ToDouble(value).ToString("#,#", CultureInfo.InvariantCulture);//Convert.ToString(String.Format("{0:#,###,###}", value));//{0:#,###0}
                            break;
                        }
                    case 8:
                    case 10:
                    case 14:
                    case 15:
                    case 17:
                    case 20:
                        {
                            TimePeriodValue = value + "%";
                            break;
                        }
                    case 18:
                    case 21:
                        {
                            TimePeriodValue = "$" + value;
                            break;
                        }
                }
            }
            return TimePeriodValue;
        }
        public string GetImpactToSalesValue(int Rownumber, string value)
        {
            string ImpactToSales = value;           
            switch (Rownumber)
            {
                case 12:
                case 13:
                case 14:
                case 15:
                case 19:
                case 20:
                case 21:
                    {
                        ImpactToSales = "-";                       
                        break;
                    }
                default:
                    {
                        if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) > 0)
                        {
                            ImpactToSales = "+" + Convert.ToString(value) + "%";
                        }
                        else
                        {
                            ImpactToSales = Convert.ToString(value) + "%";
                        }
                        break;
                    }
            }
            return ImpactToSales;
        }
        public string GetChangeBasis(int Rownumber)
        {
            string _ChangeBasis = string.Empty;           
            switch (Rownumber)
            {
                case 7:
                case 12:
                case 13:
                case 18:
                case 21:
                    {
                        _ChangeBasis = "Percentage";                     
                        break;
                    }
                case 8:
                case 10:
                case 14:
                case 15:
                case 17:
                case 20:
                    {
                        _ChangeBasis = "Pct. Points";                      
                        break;
                    }
                case 11:
                case 19:
                    {
                        _ChangeBasis = "Trips";                      
                        break;
                    }
            }
            return _ChangeBasis;
        }
        #region Check Channel
        public bool IsChannelMetric(string Channel, string Metric)
        {
            bool iChannel = false;
            if (!string.IsNullOrEmpty(Channel) && Channel.IndexOf("Channels") > -1)
            {
                switch (Convert.ToString(Metric).Trim().ToLower())
                {
                    case "% of shopper population in trade area":
                        {
                            iChannel = true;
                            break;
                        }
                }
            }
            return iChannel;
        }
        #endregion
        public bool IsLowSampleSize(string samplesize)
        {
            bool isSampleSize = false;
            if (IsChannelMetric(Benchmark, MetricItem))
                isSampleSize = true;
            else if (!string.IsNullOrEmpty(samplesize))
            {
                //atul new
                if (Convert.ToDouble(samplesize) < GlobalVariables.MinSampleSize)
                {
                    isSampleSize = true;
                }
            }
            return isSampleSize;
        }
        private Color ApplyMediumSampleSizeBackgroundColor(string samplesize)
        {
            Color SampleSizeBackgroundColor = Color.FromArgb(217, 217, 217);
            if (!string.IsNullOrEmpty(samplesize))
            {
                //atul new
                if (Convert.ToDouble(samplesize) >= GlobalVariables.MinSampleSize && Convert.ToDouble(samplesize) < GlobalVariables.MaxSampleSize)
                {
                    SampleSizeBackgroundColor = Color.Gray;
                }
            }
            return SampleSizeBackgroundColor;
        }
        private Color ApplySignificanceColor(string value, string samplesize, bool isBenchmark, int Rownumber)
        {
            Color color = Color.FromArgb(0,0,0);
            if (IsLowSampleSize(samplesize))
            {
                return color;
            }

            if (isBenchmark)
            {
                switch (Rownumber)
                {
                    case 8:
                    case 10:
                    case 14:
                    case 15:
                    case 17:
                    case 20:
                        {
                            color = Color.FromArgb(0, 0, 255);
                            break;
                        }
                    default:
                        {
                            color = Color.FromArgb(0, 0, 0);
                            break;
                        }
                }
               
            }
            else
            {
                if (value != "")
                {
                    if (Convert.ToDouble(value) > accuratestatvalueposi)
                    {
                        color = Color.FromArgb(0,128,0);
                    }
                    else if (Convert.ToDouble(value) < accuratestatvaluenega)
                    {
                        color = Color.FromArgb(255,0,0);
                    }
                    else if (Convert.ToDouble(value) <= accuratestatvalueposi && Convert.ToDouble(value) >= accuratestatvaluenega)
                    {
                        color = Color.FromArgb(0,0,0);
                    }
                }
            }
            return color;
        }    
    }
}