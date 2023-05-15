using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using iSHOPNew.DAL;


namespace iSHOPNew.Models
{

    public class ExportToExcelBGM
    {
        public Dictionary<string, string> metricListValues = new Dictionary<string, string>();
        public string StatPositive = string.Empty;
        public string StatNegative = string.Empty;
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public string significanceColor;

        int samplecellstyle = 4;
        string SelectedStatTest = string.Empty;
        string Benchmark = string.Empty;
        string _SelectedBevorNonBevShortName = string.Empty;

        string ShopperSampleSize = string.Empty;
        string TripsSampleSize = string.Empty;
        string ChageBasis_FontWeight = "normal";
        string ImpactToSales_BackgroundColor = "lightgrey";
        //For BGM Revamp
        //Added by Nagaraju 05-04-2016
        public string AddPlusSymble(string value)
        {
            string _symble = string.Empty;
            if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) > 0)
                _symble = "+";
            return _symble;
        }
        public string GetTimePeriodValue(int Rownumber, string value)
        {
            string TimePeriodValue = value;
            switch (Rownumber)
            {
                case 7:
                case 12:
                case 13:
                    {
                        if (!string.IsNullOrEmpty(value))
                            TimePeriodValue = value;
                        break;
                    }
                case 8:
                case 10:
                case 14:
                case 15:
                case 17:
                case 20:
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            TimePeriodValue = Convert.ToString(Convert.ToDouble(value) / 100);
                        }
                        break;
                    }
                case 18:
                case 21:
                    {
                        TimePeriodValue = value;
                        break;
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
                        if (!string.IsNullOrEmpty(value))
                        {
                            ImpactToSales = Convert.ToString(Convert.ToDouble(value) / 100);
                        }
                        break;
                    }
            }
            return ImpactToSales;
        }
        public string GetChangeBasis(int Rownumber)
        {
            string _ChangeBasis = string.Empty;
            ChageBasis_FontWeight = "normal";
            switch (Rownumber)
            {
                case 7:
                case 12:
                case 13:
                case 18:
                case 21:
                    {
                        _ChangeBasis = "Percentage";
                        if (Rownumber == 7 || Rownumber == 18)
                            ChageBasis_FontWeight = "bold";
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
                        if (Rownumber == 8 || Rownumber == 10 || Rownumber == 17)
                            ChageBasis_FontWeight = "bold";
                        break;
                    }
                case 11:
                case 19:
                    {
                        _ChangeBasis = "Trips";
                        if (Rownumber == 11)
                            ChageBasis_FontWeight = "bold";
                        break;
                    }
            }
            return _ChangeBasis;
        }
        public string GetChangeVsYagoValue(string change_basic, string value)
        {
            string ChangeVsYago = value;
            if (change_basic.Equals("Percentage", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(value))
            {
                ChangeVsYago = Convert.ToString(Convert.ToDouble(value) / 100);
            }
            return ChangeVsYago;
        }
        public void ExportToExcel(string query, string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            BGMParams bgmParams = null;
            if (HttpContext.Current.Session["BGMData"] != null)
            {
                bgmParams = HttpContext.Current.Session["BGMData"] as BGMParams;
            }
            var fileNmae = "ShopperMetricsExportFile" + System.DateTime.Today.ToLongDateString() + ".xlsx";
            string destFile = HttpContext.Current.Server.MapPath("~/Temp/ExportedFiles/" + fileNmae);
            string sourceFile = HttpContext.Current.Request.MapPath("~/ISHOPBGM Excel Export Files/ISHOPBGMExcelTemplate.xlsx");
            File.Copy(sourceFile, destFile, true);
            XLWorkbook workbook = new XLWorkbook(destFile);
            #region File Save Section Start

            string selectedProduct = string.Empty;
            string ChangeBasis = string.Empty;
            string ImpactToSales = string.Empty;
            //bgmParams.selectedProduct = ;
            string ShopperSampleSize = string.Empty;
            string TripsSampleSize = string.Empty;
            DataSet ds = bgmParams.Data_Set;
            DataTable tbl = null;
            List<string> Products = new List<string>();
            List<string> Metrics = new List<string>() { "MARKET STATS", "RETAILER STATS", "PRODUCT STATS" };
            
            int SheetNumber = 1;
            int rowNumber = 1;
            int colNumber = 1;

            if (ds != null && ds.Tables.Count > 0)
            {
                Products = (from row in ds.Tables[1].AsEnumerable()
                            select Convert.ToString(row.Field<object>("Product"))).Distinct().ToList();
                if (Products != null && Products.Count > 0)
                {
                    foreach (string Product in bgmParams.BeverageNonBeveragelist)
                    {
                        //if (Product == bgmParams.selectedProduct)
                        //{
                        rowNumber = 1;
                        colNumber = 1;

                        IXLWorksheet ws = workbook.Worksheet("(" + SheetNumber + ")");
                        #region Add Sample Size
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
                        ws.Cell(1, 7).Value = ShopperSampleSize;
                        ws.Cell(2, 7).Value = TripsSampleSize;
                        #endregion

                        foreach (string Metric in Metrics)
                        {
                            var querystring = (from row in ds.Tables[1].AsEnumerable()
                                               where
                                                   Convert.ToString(row.Field<object>("Product")).Equals(Product, StringComparison.OrdinalIgnoreCase)
                                                   && Convert.ToString(row.Field<object>("Metric")).Equals(Metric, StringComparison.OrdinalIgnoreCase)
                                               select row);

                            tbl = querystring.CopyToDataTable();
                            if (tbl != null && tbl.Rows.Count > 0)
                            {
                                for (int i = 0; i < tbl.Rows.Count; i++)
                                {
                                    colNumber = 1;                                  
                                    ChangeBasis = GetChangeBasis(Convert.ToInt32(tbl.Rows[i]["rowNum"]));
                                    ImpactToSales = GetImpactToSalesValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Impact to Sales"]));

                                    ws.Cell(rowNumber, colNumber).Value = GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Previous Year"]));
                                    colNumber++;
                                    ws.Cell(rowNumber, colNumber).Value = GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Current Year"]));
                                    colNumber++;
                                    ws.Cell(rowNumber, colNumber).Value = GetChangeVsYagoValue(ChangeBasis, Convert.ToString(tbl.Rows[i]["Change vs. YAGO"]));
                                    colNumber++;
                                    ws.Cell(rowNumber, colNumber).Value = ImpactToSales;

                                    colNumber += 4;
                                    ws.Cell(rowNumber, colNumber).Value = Convert.ToString(tbl.Rows[i]["Previous Year Sample"]);
                                    colNumber++;
                                    ws.Cell(rowNumber, colNumber).Value = Convert.ToString(tbl.Rows[i]["Current Year Sample"]);
                                    colNumber++;
                                    ws.Cell(rowNumber, colNumber).Value = Convert.ToString(tbl.Rows[i]["Significance"]);

                                    colNumber++;
                                    ws.Cell(rowNumber, colNumber).Value = Convert.ToString(tbl.Rows[i]["SamplePY"]);
                                    colNumber++;
                                    ws.Cell(rowNumber, colNumber).Value = Convert.ToString(tbl.Rows[i]["SampleCY"]);

                                    rowNumber++;
                                }
                            }
                            rowNumber++;
                        }
                        SheetNumber++;
                        //}
                    }
                }
            }

            #region Plot Setup Sheet
            IXLWorksheet wsSetup = workbook.Worksheet("Setup");
            //Add Products
            rowNumber = 2;
            colNumber = 2;
            foreach (string Product in bgmParams.BeverageNonBeveragelist)
            {
                //if (Product == bgmParams.selectedProduct)
                //{
                    wsSetup.Cell(rowNumber, colNumber).Value = Product.ToUpper();
                    rowNumber++;
                //}
            }
            //
            //Add TimePeriod
            wsSetup.Cell(2, 5).Value = bgmParams.PreviousTimePeriod.ToUpper();
            wsSetup.Cell(3, 5).Value = bgmParams.TimePeriod.ToUpper();
            //
            //Add Retailer
            wsSetup.Cell(2, 7).Value = bgmParams.BenchMarkShortName.ToUpper();
            //
            //Add Advanced Filters
            wsSetup.Cell(2, 9).Value = string.IsNullOrEmpty(bgmParams.FilterShortNames) ? "NONE" : bgmParams.FilterShortNames.ToUpper();
            //
            //Add Frequency
            wsSetup.Cell(2, 11).Value = bgmParams.ShopperFrequency.ToUpper();
            //
            #endregion

            IXLWorksheet wsReport = workbook.Worksheet("Report");
            wsReport.Cell(1, 4).Value = bgmParams.BeverageNonBeveragelist[0].ToUpper();

            workbook.SaveAs(destFile);
            FileStream fs1 = null;
            fs1 = System.IO.File.Open(destFile, System.IO.FileMode.Open);
            byte[] btFile = new byte[fs1.Length];
            fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
            fs1.Close();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=iShop_Explorer_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds)) + ".xlsx");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Length", new FileInfo(destFile).Length.ToString());
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
            HttpContext.Current.Response.BinaryWrite(btFile);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            #endregion File Save Section End
        }

        public void ExportToExcel2(string query, string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            int row = 0, col = 0;
            try
            {
                StatPositive = Convert.ToString(HttpContext.Current.Session["StatSessionPosi"]);
                StatNegative = Convert.ToString(HttpContext.Current.Session["StatSessionNega"]);

                string sourceFile = HttpContext.Current.Request.MapPath("~/ISHOPBGM Excel Export Files/ISHOPBGMExcelTemplate.xlsx");

                var fileNmae = "BGMExportFile" + System.DateTime.Today.ToLongDateString() + ".xlsx";
                string destFile = HttpContext.Current.Server.MapPath("~/Temp/ExportedFiles/" + fileNmae);
                File.Copy(sourceFile, destFile, true);

                metricListValues.Clear();
                metricListValues.Add("Shoppers (000)", ",");
                metricListValues.Add("Trips (000)", ",");
                metricListValues.Add("Trips within Channel (000)", ",");
                metricListValues.Add("Trips to Retailer (000)", ",");
                metricListValues.Add("Share of Total Trips", "%");//"Total Trip Conversion", "%");
                metricListValues.Add("Share of Channel Trips", "%");//"Trip Conversion within Channel", "%");
                metricListValues.Add("Total Trips Per Shopper", "d");
                metricListValues.Add("Trips Per Shopper within Channel", "d");
                metricListValues.Add("Trips per Shopper within Retailer", "d");

                XLWorkbook workbook = new XLWorkbook(destFile);
                IXLWorksheet ws = workbook.Worksheet("Sheet1");
                DataAccess da1 = new DataAccess();
                if (HttpContext.Current.Session["ISHOPBGMparameters"] != null && HttpContext.Current.Session["ISHOPBGMDataset"] != null)
                {
                    object[] ISHOPBGMparameters = HttpContext.Current.Session["ISHOPBGMparameters"] as object[];
                    object[] ISHOPBGMparametersSp = new object[] { ISHOPBGMparameters[0], ISHOPBGMparameters[1], ISHOPBGMparameters[2], ISHOPBGMparameters[3], ISHOPBGMparameters[4], ISHOPBGMparameters[5], ISHOPBGMparameters[6] };
                    DataSet ds = HttpContext.Current.Session["ISHOPBGMDataset"] as DataSet;//da1.GetData(ISHOPBGMparametersSp, "USP_iSHOPBGM");

                    CommonFunctions objcomFunc = new CommonFunctions();
                    if (ds.Tables.Count == 3)
                    {
                        DataTable bgmTable2 = ds.Tables[0];
                        DataTable bgmTable = ds.Tables[1];
                        DataTable bgmTable1 = ds.Tables[2];


                        List<string> col1 = new List<string>();
                        var col1List = from r in bgmTable.AsEnumerable() select r.Field<string>(0);
                        col1 = col1List.Distinct().ToList();
                        List<string> col2 = new List<string>();
                        var col2List = from r in bgmTable.AsEnumerable() select r.Field<string>(1);
                        col2 = col2List.Distinct().ToList();
                        List<string> col3 = new List<string>();
                        var col3List = from r in bgmTable.AsEnumerable() select r.Field<string>(2);
                        col3 = col3List.Distinct().ToList();
                        List<string> col4 = new List<string>();
                        var col4List = from r in bgmTable.AsEnumerable() select r.Field<string>(3);
                        col4 = col4List.Distinct().ToList();

                        List<string> col3Sample = new List<string>();
                        var col3SampleList = from r in bgmTable1.AsEnumerable() select r.Field<string>(2);
                        col3Sample = col3SampleList.Distinct().ToList();

                        List<string> FrstColNames = new List<string>();
                        var FrstColNames1 = from r in bgmTable2.AsEnumerable() select r.Field<string>(0);
                        FrstColNames = FrstColNames1.Distinct().ToList();
                        FrstColNames.Remove("Sample Size");

                        #region Selection Part Start
                        ws.Cell(1, 3).Value = "* Selection";
                        ws.Cell(1, 3).Style.Font.Underline = XLFontUnderlineValues.Single;
                        ws.Cell(1, 3).Style.Border.InsideBorder = XLBorderStyleValues.Medium;

                        ws.Cell(1, 6).Value = "* Filters";
                        ws.Cell(1, 6).Style.Font.Underline = XLFontUnderlineValues.Single;
                        ws.Cell(1, 8).Value = "Stat Test:";
                        ws.Cell(1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        ws.Cell(2, 8).Value = ">95%";
                        ws.Cell(2, 8).Style.Font.FontColor = XLColor.Green;
                        ws.Cell(2, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        ws.Cell(3, 8).Value = "<95%";
                        ws.Cell(3, 8).Style.Font.FontColor = XLColor.Red;
                        ws.Cell(3, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        ws.Cell(2, 3).Value = "Time Period : " + ISHOPBGMparameters[7];
                        ws.Cell(2, 6).Value = "Shopping Frequency : " + ISHOPBGMparameters[3];
                        ws.Cell(3, 3).Value = "Purchase Item : " + ISHOPBGMparameters[4].ToString().Replace(":", "") + "-" + ISHOPBGMparameters[8].ToString().Replace("Category||", "").Replace("Retailers|", "").Replace("ItemPurchased|", "").Replace("~", "`"); //ISHOPBGMparameters[5].ToString().Replace("Category||", "").Replace("Retailers|", "").Replace("ItemPurchased|", "");
                        ws.Cell(3, 6).Value = ISHOPBGMparameters[9].ToString();
                        ws.Cell(4, 3).Value = "NA* - Data Not Available";

                        ws.Column(2).Width = 70;

                        #endregion Selection Part End

                        #region Header Section BenchmarK and Comparision start

                        var range1 = ws.Range(5, 3, 5, 7);
                        range1.Value = "BENCHMARK";
                        range1.Style.Font.FontColor = XLColor.White;
                        range1.Style.Fill.BackgroundColor = XLColor.FromHtml("#000001");
                        range1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        range1.Merge();

                        row = 5; col = 2;
                        int colspan = ((col1.Count - 1) * 5);
                        var range = ws.Range(5, 8, 5, colspan + 7);
                        range.Value = "COMPRISON AREAS";
                        range.Style.Fill.BackgroundColor = XLColor.FromHtml("#808080");
                        range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        range.Merge();

                        col = 3;
                        foreach (var c1 in col1)
                        {
                            int rangecol = 4;
                            ws.Cell(6, col).Value = objcomFunc.Get_ShortNames(c1);
                            ws.Range(6, col, 6, rangecol + col).Merge();
                            ws.Range(6, col, 6, rangecol + col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            col = col + 5;
                        }

                        #endregion Header Section BenchmarK and Comparision End

                        #region First Column Values Start
                        row = 8;

                        foreach (DataRow FrstColName in bgmTable2.Rows)
                        {

                            int rowcount = Convert.ToInt32(FrstColName[1]) - 1;

                            //if (FrstColName[0].ToString() == "Basket Size")
                            //    row = row + 1;

                            if (FrstColName[0].ToString() == "Sample Size")
                            {
                                row = row + 1;
                                ws.Range(row, 1, row + rowcount, 1).Merge();
                                ws.Range(row, 1, row + rowcount, 1).Value = FrstColName[0];
                                ws.Range(row, 1, row + rowcount, 1).Style.Font.Bold = true;
                                ws.Range(row, 1, row + rowcount, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(row, 1, row + rowcount, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                break;
                            }
                            else
                            {
                                ws.Range(row, 1, row + rowcount, 1).Merge();
                                ws.Range(row, 1, row + rowcount, 1).Value = FrstColName[0];
                                ws.Range(row, 1, row + rowcount, 1).Style.Font.Bold = true;
                                ws.Range(row, 1, row + rowcount, 1).Style.Alignment.WrapText = true;
                                ws.Range(row, 1, row + rowcount, 1).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                                ws.Range(row, 1, row + rowcount, 1).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                                ws.Range(row, 1, row + rowcount, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(row, 1, row + rowcount, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            }


                            #region Column Second Border Alignment start
                            int columnsCount = col1.Count * 5;
                            for (int i = 1; i < columnsCount + 3; i++)
                            {
                                int rowcountforColumns = Convert.ToInt32(FrstColName[1]);
                                ws.Cell(row, i).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                                ws.Cell(21, i).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                                ws.Cell(27, i).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                                ws.Cell(28, i).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                                ws.Cell(35, i).Style.Border.BottomBorder = XLBorderStyleValues.Medium;

                            }
                            #endregion Column Second Border Alignment End

                            row = row + rowcount;
                            row++;
                        }

                        #endregion First Column Values End

                        #region Benchmark and Comparision Header Section Start
                        row = 7; col = 3;
                        for (int i = 0; i < col1.Count; i++)
                        {
                            foreach (var c4 in col4)
                            {
                                ws.Cell(row, col).Value = c4;
                                ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(row, col).Style.Font.FontName = "Calibri";
                                ws.Cell(row, col).Style.Font.Bold = true;
                                ws.Cell(row, col).Style.Font.FontSize = 11;
                                ws.Cell(row, col).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                                ws.Cell(row, col).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                                //ws.Column(col).Width = 20;
                                col++;
                            }
                        }
                        #endregion Benchmark and Comparision Header Section End

                        #region Benchmark and Comparision Values Start
                        List<double> valuesLists = new List<double>();

                        List<Int32> valuesListsSample1 = new List<Int32>();
                        List<Int32> valuesListsSample2 = new List<Int32>();
                        List<Int32> valuesListsSample3 = new List<Int32>();
                        List<Int32> valuesListsSample4 = new List<Int32>();
                        List<Int32> valuesListsSample5 = new List<Int32>();
                        List<Int32> valuesListsSample6 = new List<Int32>();
                        List<Int32> valuesListsSample7 = new List<Int32>();

                        int ca = 0;
                        foreach (var c3sample in col3Sample)
                        {
                            var valList1 = from r in bgmTable1.AsEnumerable() where r.Field<string>("ColumnValue") == c3sample select r.Field<Int32>(4);
                            //if (ca == 0)
                            //    valuesListsSample1 = valList1.ToList();
                            //else
                            //    valuesListsSample2 = valList1.ToList();

                            switch (ca)
                            {
                                case 0:
                                    valuesListsSample1 = valList1.ToList();
                                    break;
                                case 1:
                                    valuesListsSample2 = valList1.ToList();
                                    break;
                                case 2:
                                    valuesListsSample3 = valList1.ToList();
                                    break;
                                case 3:
                                    valuesListsSample4 = valList1.ToList();
                                    break;
                                case 4:
                                    valuesListsSample5 = valList1.ToList();
                                    break;
                                case 5:
                                    valuesListsSample6 = valList1.ToList();
                                    break;
                                case 6:
                                    valuesListsSample7 = valList1.ToList();
                                    break;

                            }
                            ca++;
                        }
                        int rowcount1 = 0;
                        string superScript = string.Empty;
                        foreach (var c3 in col3)
                        {
                            string seperatedValues = string.Empty;
                            var valList = from r in bgmTable.AsEnumerable() where r.Field<string>("ColumnValue") == c3 select r.Field<double>(4);
                            col = 3;
                            //if (c3 == "Average Basket")
                            //{
                            //    row = row + 1;
                            //}
                            row++;

                            switch (rowcount1)
                            {
                                case 0:
                                    superScript = "1";
                                    break;

                                case 1:
                                case 4:
                                case 6:
                                case 14:
                                    superScript = "2";
                                    break;

                                case 2:
                                case 5:
                                case 7:
                                case 16:
                                    superScript = "3";
                                    break;

                                case 3:
                                case 8:
                                case 18:
                                    superScript = "4";
                                    break;

                                case 9:
                                case 12:
                                case 15:
                                    superScript = "5";
                                    break;

                                case 10:
                                case 13:
                                case 17:
                                    superScript = "6";
                                    break;

                                case 11:
                                case 19:
                                    superScript = "7";
                                    break;

                            }

                            ws.Cell(row, 2).Value = c3 + " [" + superScript + "]";
                            //ws.Cell(row, 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            valuesLists = valList.ToList();

                            int colIndex_ = 0;
                            string backGrndCeLLClr = string.Empty;
                            string checkSamplesize = string.Empty;

                            foreach (var val in valuesLists)
                            {

                                List<string> valueListSignificance = new List<string>();
                                var valSignfiList = from r in bgmTable.AsEnumerable() where r.Field<string>("ColumnValue") == c3 select Convert.ToString(r.Field<object>(5)) == string.Empty ? "0" : Convert.ToString(r.Field<object>(5));
                                valueListSignificance = valSignfiList.ToList();

                                string shopperValue = Convert.ToString(valuesListsSample1[colIndex_]);
                                string tripsValue = Convert.ToString(valuesListsSample2[colIndex_]);
                                //string fValue = CheckLowSampleSize(shopperValue, tripsValue);

                                switch (rowcount1)
                                {
                                    case 0:
                                        checkSamplesize = Convert.ToString(valuesListsSample1[colIndex_]);
                                        break;

                                    case 1:
                                    case 4:
                                    case 6:
                                    case 14:
                                        checkSamplesize = Convert.ToString(valuesListsSample2[colIndex_]);
                                        break;

                                    case 2:
                                    case 5:
                                    case 7:
                                    case 16:
                                        checkSamplesize = Convert.ToString(valuesListsSample3[colIndex_]);
                                        break;

                                    case 3:
                                    case 8:
                                    case 18:
                                        checkSamplesize = Convert.ToString(valuesListsSample4[colIndex_]);
                                        break;

                                    case 9:
                                    case 12:
                                    case 15:
                                        checkSamplesize = Convert.ToString(valuesListsSample5[colIndex_]);
                                        break;

                                    case 10:
                                    case 13:
                                    case 17:
                                        checkSamplesize = Convert.ToString(valuesListsSample6[colIndex_]);
                                        break;

                                    case 11:
                                    case 19:
                                        checkSamplesize = Convert.ToString(valuesListsSample7[colIndex_]);
                                        break;

                                }

                                string fValue =CommonFunctions.CheckLowSampleSize(checkSamplesize,out samplecellstyle);
                                CultureInfo cultureInfo = new CultureInfo("en-US");
                                //if (val.ToString() == fValue)
                                //{
                                seperatedValues = CommaSeparatedValues(val, c3);
                                if (colIndex_ <= 4)
                                {
                                    if ((ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && ISHOPBGMparameters[10].ToString().Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "Beverages" && ISHOPBGMparameters[10].ToString().Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                    }
                                    else if (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && shopperValue == "0" && tripsValue == "0")
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                    }
                                    else if (fValue == GlobalVariables.LowSampleSize)
                                    {
                                        ws.Cell(row, col).Value = "";
                                    }
                                    else if (fValue == GlobalVariables.UseDirectionally)
                                    {
                                        if (c3.Contains("Average Basket"))
                                        {
                                            //ws.Cell(row, col).Value = val.ToString("0");
                                            //ws.Cell(row, col).Style.NumberFormat.Format = "$0";
                                            ws.Cell(row, col).Value = Convert.ToString(val);
                                            ws.Cell(row, col).Style.NumberFormat.Format = "$0";
                                        }
                                        else if (seperatedValues == "%" || rowcount1 == 12 || rowcount1 == 13)
                                        {
                                            double valdivision;

                                            if (colIndex_ <= 4)
                                            {
                                                if ((rowcount1 == 12 || rowcount1 == 13 || rowcount1 == 4 || rowcount1 == 5) && ISHOPBGMparameters[12].ToString().Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                                                {

                                                    significanceColor = "#0000FF";
                                                }
                                                else
                                                {
                                                    significanceColor = "#000000";
                                                }
                                            }
                                            else
                                            {
                                                significanceColor = GetCellColor(Convert.ToDouble(valueListSignificance[colIndex_]));
                                            }

                                            valdivision = val / 100;
                                            ws.Cell(row, col).Value = valdivision;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#0.0%";
                                            ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml(significanceColor);

                                        }
                                        else if (seperatedValues == "," || rowcount1 == 9 || rowcount1 == 10 || rowcount1 == 11)
                                        {
                                            ws.Cell(row, col).Value = Convert.ToString(val);//.ToString("#,###,###", CultureInfo.InvariantCulture);//.ToString("#,###,###", CultureInfo.InvariantCulture); //.ToString("N0", CultureInfo.InvariantCulture);//val.ToString("#,#", CultureInfo.InvariantCulture);//Convert.ToStrivalng();
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0"; //"//0:#,###,###";//"#,###,###";

                                        }
                                        else if (seperatedValues == "d")
                                        {
                                            ws.Cell(row, col).Value = val;//val.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);//val;//val.ToString("0.0");

                                            ws.Cell(row, col).Style.NumberFormat.Format = "0.0;[Red]0.0";

                                        }
                                        ws.Cell(row, col).Style.Fill.BackgroundColor = XLColor.FromHtml("#E6E6E6");
                                    }
                                    else
                                    {
                                        if (c3.Contains("Average Basket"))
                                        {
                                            //ws.Cell(row, col).Value = val.ToString("0");
                                            //ws.Cell(row, col).Style.NumberFormat.Format = "$0";
                                            ws.Cell(row, col).Value = Convert.ToString(val);
                                            ws.Cell(row, col).Style.NumberFormat.Format = "$0";

                                        }
                                        else if (seperatedValues == "%" || rowcount1 == 12 || rowcount1 == 13)
                                        {
                                            double valdivision;

                                            if (colIndex_ <= 4)
                                            {
                                                if ((rowcount1 == 12 || rowcount1 == 13 || rowcount1 == 4 || rowcount1 == 5) && ISHOPBGMparameters[12].ToString().Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                                                {

                                                    significanceColor = "#0000FF";
                                                }
                                                else
                                                {
                                                    significanceColor = "#000000";
                                                }
                                            }
                                            else
                                            {
                                                significanceColor = GetCellColor(Convert.ToDouble(valueListSignificance[colIndex_]));
                                            }

                                            valdivision = val / 100;
                                            ws.Cell(row, col).Value = valdivision;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#0.0%";
                                            ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml(significanceColor);

                                        }
                                        else if (seperatedValues == "," || rowcount1 == 9 || rowcount1 == 10 || rowcount1 == 11)
                                        {
                                            ws.Cell(row, col).Value = Convert.ToString(val);//.ToString("#,###,###", CultureInfo.InvariantCulture);//.ToString("N0", CultureInfo.InvariantCulture);//val.ToString("#,#", CultureInfo.InvariantCulture);//Convert.ToString(val);
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0";//"0:#,##0"; //"#,###,###";
                                        }
                                        else if (seperatedValues == "d")
                                        {
                                            ws.Cell(row, col).Value = val;//.ToString("0.0;[Red]0.0"); //val.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);//val;//val.ToString("0.0");
                                            ws.Cell(row, col).Style.NumberFormat.Format = "0.0;[Red]0.0";
                                        }

                                    }

                                }
                                else
                                {
                                    int compIndex = 0;
                                    List<string> CompList = new List<string>();
                                    compIndex = objcomFunc.GetCompIndex(colIndex_);
                                    CompList = ISHOPBGMparameters[11].ToString().Split(',').ToList<string>();
                                    if ((ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "Beverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                    }
                                    else if (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && shopperValue == "0" && tripsValue == "0")
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                    }
                                    else if (fValue == GlobalVariables.LowSampleSize)
                                    {
                                        ws.Cell(row, col).Value = "";
                                    }
                                    else if (fValue == GlobalVariables.UseDirectionally)
                                    {
                                        if (c3.Contains("Average Basket"))
                                        {
                                            //ws.Cell(row, col).Value = val.ToString("0");
                                            //ws.Cell(row, col).Style.NumberFormat.Format = "$0";
                                            ws.Cell(row, col).Value = Convert.ToString(val);
                                            ws.Cell(row, col).Style.NumberFormat.Format = "$0";
                                        }
                                        else if (seperatedValues == "%" || rowcount1 == 12 || rowcount1 == 13)
                                        {
                                            double valdivision;

                                            if (colIndex_ <= 4)
                                            {
                                                significanceColor = "#000000";
                                            }
                                            else
                                            {
                                                significanceColor = GetCellColor(Convert.ToDouble(valueListSignificance[colIndex_]));
                                            }

                                            valdivision = val / 100;
                                            ws.Cell(row, col).Value = valdivision;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#0.0%";
                                            ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml(significanceColor);

                                        }
                                        else if (seperatedValues == "," || rowcount1 == 9 || rowcount1 == 10 || rowcount1 == 11)
                                        {
                                            ws.Cell(row, col).Value = Convert.ToString(val);//.ToString("#,###,###", CultureInfo.InvariantCulture);//.ToString("#,###,###", CultureInfo.InvariantCulture); //.ToString("N0", CultureInfo.InvariantCulture);//val.ToString("#,#", CultureInfo.InvariantCulture);//Convert.ToStrivalng();
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0"; //"//0:#,###,###";//"#,###,###";

                                        }
                                        else if (seperatedValues == "d")
                                        {
                                            ws.Cell(row, col).Value = val;//val.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);//val;//val.ToString("0.0");

                                            ws.Cell(row, col).Style.NumberFormat.Format = "0.0;[Red]0.0";

                                        }
                                        ws.Cell(row, col).Style.Fill.BackgroundColor = XLColor.FromHtml("#E6E6E6");
                                    }
                                    else
                                    {
                                        if (c3.Contains("Average Basket"))
                                        {
                                            //ws.Cell(row, col).Value = val.ToString("0");
                                            //ws.Cell(row, col).Style.NumberFormat.Format = "$0";
                                            ws.Cell(row, col).Value = Convert.ToString(val);
                                            ws.Cell(row, col).Style.NumberFormat.Format = "$0";

                                        }
                                        else if (seperatedValues == "%" || rowcount1 == 12 || rowcount1 == 13)
                                        {
                                            double valdivision;

                                            if (colIndex_ <= 4)
                                            {
                                                significanceColor = "#000000";
                                            }
                                            else
                                            {
                                                significanceColor = GetCellColor(Convert.ToDouble(valueListSignificance[colIndex_]));
                                            }

                                            valdivision = val / 100;
                                            ws.Cell(row, col).Value = valdivision;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#0.0%";
                                            ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml(significanceColor);

                                        }
                                        else if (seperatedValues == "," || rowcount1 == 9 || rowcount1 == 10 || rowcount1 == 11)
                                        {
                                            ws.Cell(row, col).Value = Convert.ToString(val);//.ToString("#,###,###", CultureInfo.InvariantCulture);//.ToString("N0", CultureInfo.InvariantCulture);//val.ToString("#,#", CultureInfo.InvariantCulture);//Convert.ToString(val);
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0";//"0:#,##0"; //"#,###,###";
                                        }
                                        else if (seperatedValues == "d")
                                        {
                                            ws.Cell(row, col).Value = val;//.ToString("0.0;[Red]0.0"); //val.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);//val;//val.ToString("0.0");
                                            ws.Cell(row, col).Style.NumberFormat.Format = "0.0;[Red]0.0";
                                        }

                                    }
                                }
                                ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                                col++;
                                colIndex_++;
                            }
                            rowcount1++;
                        }




                        #endregion Benchmark and Comparision Values End

                        #region Sample size Section Start
                        row = row + 2;
                        List<Int32> valuesListsSample = new List<Int32>();

                        List<Int32> valuesListsSampleSize1 = new List<Int32>();
                        List<Int32> valuesListsSampleSize2 = new List<Int32>();

                        int ca1 = 0;
                        foreach (var c3sample in col3Sample)
                        {
                            var valList1 = from r in bgmTable1.AsEnumerable() where r.Field<string>("ColumnValue") == c3sample select r.Field<Int32>(4);
                            if (ca1 == 0)
                                valuesListsSampleSize1 = valList1.ToList();
                            else
                                valuesListsSampleSize2 = valList1.ToList();

                            ca1++;
                        }

                        int rowSampleCount = 1;
                        foreach (var c3sample in col3Sample)
                        {
                            ws.Cell(row, 2).Value = c3sample.Replace("Sample", "") + " [" + rowSampleCount + "] ";

                            col = 3;
                            var valList = from r in bgmTable1.AsEnumerable() where r.Field<string>("ColumnValue") == c3sample select r.Field<Int32>(4);
                            valuesListsSample = valList.ToList();

                            int colIndexSampleVal_ = 0;
                            foreach (int l in valuesListsSample)
                            {

                                string SampleSizeText = CommonFunctions.CheckLowSampleSize(Convert.ToString(l),out samplecellstyle);
                                string backgrndClr = string.Empty;
                                string showVal = string.Empty;

                                string shopperValue = Convert.ToString(valuesListsSampleSize1[colIndexSampleVal_]);
                                string tripsValue = Convert.ToString(valuesListsSampleSize2[colIndexSampleVal_]);

                                if (colIndexSampleVal_ <= 4)
                                {
                                    if ((ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && ISHOPBGMparameters[10].ToString().Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "Beverages" && ISHOPBGMparameters[10].ToString().Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                        ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                        ws.Cell(row, col).Style.Font.Bold = false;
                                    }
                                    else if (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && shopperValue == "0" && tripsValue == "0")
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                        ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                        ws.Cell(row, col).Style.Font.Bold = false;
                                    }
                                    else
                                    {
                                        if (l >= 30 && l <= 100)
                                        {
                                            ws.Cell(row, col).Value = l + " " + SampleSizeText;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0";
                                            ws.Cell(row, col).Style.Fill.BackgroundColor = XLColor.FromHtml("#E6E6E6");

                                        }
                                        else
                                        {
                                            ws.Cell(row, col).Value = l + " " + SampleSizeText;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0";
                                        }
                                        ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#878787");
                                        ws.Cell(row, col).Style.Font.Bold = true;
                                    }
                                }
                                else
                                {
                                    int compIndex = 0;
                                    List<string> CompList = new List<string>();
                                    compIndex = objcomFunc.GetCompIndex(colIndexSampleVal_);
                                    CompList = ISHOPBGMparameters[11].ToString().Split(',').ToList<string>();
                                    if ((ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "Beverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                        ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                        ws.Cell(row, col).Style.Font.Bold = false;
                                    }
                                    else if (ISHOPBGMparameters[4].ToString().Replace(" ", "") == "NonBeverages" && shopperValue == "0" && tripsValue == "0")
                                    {
                                        ws.Cell(row, col).Value = "NA*";
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#969696");
                                        ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                        ws.Cell(row, col).Style.Font.Bold = false;
                                    }
                                    else
                                    {
                                        if (l >= 30 && l <= 100)
                                        {
                                            ws.Cell(row, col).Value = l + " " + SampleSizeText;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0";
                                            ws.Cell(row, col).Style.Fill.BackgroundColor = XLColor.FromHtml("#E6E6E6");

                                        }
                                        else
                                        {
                                            ws.Cell(row, col).Value = l + " " + SampleSizeText;
                                            ws.Cell(row, col).Style.NumberFormat.Format = "#,###,##0";
                                        }
                                        ws.Cell(row, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                        ws.Cell(row, col).Style.Font.FontColor = XLColor.FromHtml("#878787");
                                        ws.Cell(row, col).Style.Font.Bold = true;
                                    }
                                }


                                col++;
                                colIndexSampleVal_++;
                            }
                            row++;
                            rowSampleCount++;
                        }

                        #endregion Sample size Section End

                        #region Right Border After 5 Columns 
                        col = 2;
                        for (int k = 1; k <= col1.Count; k++)
                        {
                            ws.Range(5, col + k * 5, col3.Count + 7, col + k * 5).Style.Border.RightBorder = XLBorderStyleValues.Thick; ;
                        }

                        for (int k = 1; k <= col1.Count; k++)
                        {
                            ws.Range(29, col + k * 5, 35, col + k * 5).Style.Border.RightBorder = XLBorderStyleValues.Thick; ;
                        }
                        #endregion Right Border After 5 Columns
                        #region File Save Section Start
                        workbook.SaveAs(destFile);
                        FileStream fs1 = null;
                        fs1 = System.IO.File.Open(destFile, System.IO.FileMode.Open);

                        byte[] btFile = new byte[fs1.Length];
                        fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                        fs1.Close();
                        HttpContext.Current.Response.ClearHeaders();
                        HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=iShop_Explorer_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds)) + ".xlsx");
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.AddHeader("Content-Length", new FileInfo(destFile).Length.ToString());
                        HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
                        HttpContext.Current.Response.BinaryWrite(btFile);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                        #endregion File Save Section End

                    }
                    else
                    {
                        ErrorLog.LogError("Tables are not Equal", "756");
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }

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
        public string CommaSeparatedValues(double value, string metricName)
        {
            string decimaval = string.Empty;
            string getSymbol = string.Empty;
            try
            {

                if (metricListValues.ContainsKey(metricName))
                {
                    getSymbol = metricListValues[metricName];
                }
                switch (getSymbol)
                {
                    case ",":
                        //decimaval = Convert.ToString(String.Format("{0:#,###}", value));
                        //if (decimaval == "")
                        //{
                        //    decimaval = "0";
                        //}
                        decimaval = ",";
                        break;

                    case "%":
                        //decimaval = Convert.ToString(String.Format("{0:P}", value));
                        decimaval = "%";
                        break;

                    case "$":
                        //decimaval = Convert.ToString(string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}", value));
                        decimaval = "$";
                        break;

                    case "d":
                        //decimaval = Convert.ToString(string.Format("{0:0.#}", value));
                        decimaval = "d";
                        break;

                    case "":
                        //decimaval = Convert.ToString(String.Format("{0:#,###}", value));
                        //if (decimaval == "")
                        //{
                        //    decimaval = "0";
                        //}
                        decimaval = ",";
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }

            return decimaval;
        }

        private string GetCellColor(double significancevalue)
        {
            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);
            string color = string.Empty;

            if (significancevalue > accuratestatvalueposi)
            {
                color = "#20B250";
            }
            else if (significancevalue < accuratestatvaluenega)
            {
                color = "#FF0000";

            }
            else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && significancevalue >= accuratestatvaluenega)
            {
                color = "#000000";

            }
            return color;

        }
    }
}