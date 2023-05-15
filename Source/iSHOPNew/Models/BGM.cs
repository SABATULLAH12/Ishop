using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using iSHOPNew.DAL;

namespace iSHOPNew.Models
{
    public class BGM
    {
        string selectedProduct = null;
        bool NonBeverageItem = false;
        string tbltext = string.Empty;
        StringBuilder sampleSizeHeaderTable = null;
        StringBuilder sampleSizeBodyTable = null;

        StringBuilder leftheader = null;
        StringBuilder leftbody = null;
        StringBuilder rightheader = null;
        StringBuilder rightbody = null;

        public List<string> CompList = new List<string>();
        public string StatPositive = string.Empty;
        public string StatNegative = string.Empty;
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public double PercentStat;

        public string significanceColor;

        string BenchmarkOrComparison;
        string SelectedStatTest = string.Empty;
        string Benchmark = string.Empty;
        string MetricItem = string.Empty;

        string ChageBasis_FontWeight = "normal";
        string ImpactToSales_BackgroundColor = "#ededee";//"lightgrey";

        CommonFunctions objcomFunc = new CommonFunctions();
        public Dictionary<string, string> metricListValues = new Dictionary<string, string>();

        //public string PlotData(string _BenchMark, string _Compare, string _timePeriod, string _ShopperFrequency, string _BeverageType, string _selectionBevorNonBev, string _filter, string _timeType, string _BevorNonBevShortName, string _AdvnedFltrTxtForExlBGM, int _MaxHeight, int _MaxWidth, int _HeaderMaxHeightSecondRow, int _HeaderMaxHeightThirdRow,string Selected_StatTest)
        //{

        //    string Benchmark = _BenchMark.Replace("~", "`");
        //    _BenchMark = _BenchMark.Replace("~", "`").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "");
        //    string Compare = _Compare.Replace("~", "`");
        //    _Compare = _Compare.Replace("~", "`").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "");
        //    Compare = Compare.Replace("'", "`").Replace("RetailerNet|", "").Replace("Retailers|","");
        //    CompList = Compare.Split(',').ToList<string>();
        //    string Frequency = _ShopperFrequency;

        //    Benchmark = _BenchMark;
        //    List<int> samplesizeList = new List<int>();
        //    DataAccess da1 = new DataAccess();
        //    List<string> distinctColumnValues = new List<string>();

        //    StringBuilder leftTable1 = new StringBuilder();
        //    StringBuilder leftTable2 = new StringBuilder();
        //    StringBuilder rightTable1 = new StringBuilder();
        //    StringBuilder rightTable2 = new StringBuilder();

        //    metricListValues=new  Dictionary<string, string>();
        //    metricListValues.Clear();
        //    metricListValues.Add("Shoppers (000)", ",");
        //    metricListValues.Add("Trips (000)", ",");
        //    metricListValues.Add("Trips within Channel (000)", ",");
        //    metricListValues.Add("Trips to Retailer (000)", ",");
        //    metricListValues.Add("Share of Total Trips", "%");//"Total Trip Conversion", "%");
        //    metricListValues.Add("Share of Channel Trips", "%");//"Trip Conversion within Channel", "%");
        //    metricListValues.Add("Total Trips Per Shopper", "d");
        //    metricListValues.Add("Trips Per Shopper within Channel", "d");
        //    metricListValues.Add("Trips per Shopper within Retailer", "d");

        //    StringBuilder table=new StringBuilder();
        //    StringBuilder mainTable = new StringBuilder();

        //    SelectedStatTest = Selected_StatTest;

        //    string htmltext = String.Empty;
        //    try
        //    {
        //        StatPositive = Convert.ToString(HttpContext.Current.Session["StatSessionPosi"]);
        //        StatNegative = Convert.ToString(HttpContext.Current.Session["StatSessionNega"]);
        //        object[] paramvalues = new object[] { _BenchMark.Replace("~", "`"), _Compare.Replace(",", "|").Replace("~", "`"), _timePeriod, _ShopperFrequency, _BeverageType.Replace(" ", "").Replace(":", ""), _selectionBevorNonBev, _filter, Selected_StatTest };
        //        object[] paramvaluesSession = new object[] { _BenchMark.Replace("~", "`"), _Compare.Replace(",", "|").Replace("~", "`"), _timePeriod, _ShopperFrequency, _BeverageType.Replace(" ", "").Replace(":", ""), _selectionBevorNonBev, _filter, _timeType, _BevorNonBevShortName, _AdvnedFltrTxtForExlBGM, Benchmark, Compare, Selected_StatTest };

        //        DataSet ds = da1.GetData(paramvalues, "USP_iSHOPBGM");

        //        HttpContext.Current.Session["ISHOPBGMparameters"] = paramvaluesSession;
        //        HttpContext.Current.Session["ISHOPBGMDataset"] = ds;

        //        if (ds != null && ds.Tables.Count > 0)
        //        {

        //                if (ds.Tables.Count == 3)
        //                {
        //                    DataTable bgmTable2 = ds.Tables[0];
        //                    DataTable bgmTable = ds.Tables[1];
        //                    DataTable bgmTable1 = ds.Tables[2];

        //                    List<string> col1 = new List<string>();
        //                    var col1List = from r in bgmTable.AsEnumerable() select r.Field<string>(0);
        //                    col1 = col1List.Distinct().ToList();
        //                    List<string> col2 = new List<string>();
        //                    var col2List = from r in bgmTable.AsEnumerable() select r.Field<string>(1);
        //                    col2 = col2List.Distinct().ToList();
        //                    List<string> col3 = new List<string>();
        //                    var col3List = from r in bgmTable.AsEnumerable() select r.Field<string>(2);
        //                    col3 = col3List.Distinct().ToList();
        //                    List<string> col4 = new List<string>();
        //                    var col4List = from r in bgmTable.AsEnumerable() select r.Field<string>(3);
        //                    col4 = col4List.Distinct().ToList();

        //                    List<string> col3Sample = new List<string>();
        //                    var col3SampleList = from r in bgmTable1.AsEnumerable() select r.Field<string>(2);
        //                    col3Sample = col3SampleList.Distinct().ToList();

        //                    List<string> FrstColNames = new List<string>();
        //                    var FrstColNames1 = from r in bgmTable2.AsEnumerable() select r.Field<string>(0);
        //                    FrstColNames = FrstColNames1.Distinct().ToList();
        //                    FrstColNames.Remove("Sample Size");

        //                    List<double> valuesLists = new List<double>();

        //                    #region Left Table Start
        //                    leftTable1.Append("<table class=\"lefttableheader\" style=\"width:100%;\">");

        //                    #region Left Table Header Start
        //                    leftTable1.Append("<thead class=\"BenchComp\" style=\"background-color: #E41E2A;\">");
        //                    leftTable1.Append("<tr>");

        //                    //leftTable1.Append("<th class=\"BenchComp\" style=\"background-color: #E41E2A;width:87px;border-right:0px;\">");
        //                    //leftTable1.Append("</th>");

        //                    leftTable1.Append("<td colspan='2' style=\"background-color: #E41E2A;border-right:0;height:20px;border-bottom:1px solid #E41E2A;\" class=\"BenchComp\">");
        //                    //leftTable1.Append("SHOPPING FREQUENCY");
        //                    leftTable1.Append("</td>");
        //                    leftTable1.Append("</tr>");

        //                    leftTable1.Append("<tr class=\"ltRow2\">");//120px
        //                    //leftTable1.Append("<th style=\"background-color: #E41E2A;color:white;width:87px;\" class=\"ShoppingFrequencyheader\">");
        //                    //leftTable1.Append("</th>");
        //                    //leftTable1.Append("<th style=\"background-color: #E41E2A;color:white;text-align: center;\" class=\"ShoppingFrequencyheader\">");
        //                    ////leftTable1.Append(_ShopperFrequency);
        //                    //leftTable1.Append("</th>");
        //                    //leftTable1.Append("</tr>");
        //                    //leftTable1.Append("<tr class=\"ltRow2\" style=\"height:63px\">");
        //                    leftTable1.Append("<td colspan='2' style=\"background-color: #E41E2A;color:white;text-align: center;display:block;border-right:1px solid #fffff; min-height:30px; max-height:" + _HeaderMaxHeightSecondRow + "px;\" class=\"ShoppingFrequencyheader\">");
        //                    leftTable1.Append(" ");
        //                    leftTable1.Append("</td>");
        //                    leftTable1.Append("</tr>");
        //                    leftTable1.Append("<tr class=\"ltRow2\">");
        //                    leftTable1.Append("<td colspan='2' style=\"background-color: #E41E2A;color:white;text-align: center;display:block;border-right:1px solid #fffff;border-bottom:1px solid #fffff;min-height:40px;max-height:" + _HeaderMaxHeightThirdRow + "px;\" class=\"ShoppingFrequencyheader\">");
        //                    leftTable1.Append(" ");
        //                    leftTable1.Append("</td>");
        //                    leftTable1.Append("</tr>");
        //                    leftTable1.Append("</thead>");

        //                    #endregion Left Table Header Start
        //                    leftTable1.Append("</table>");
        //                    leftTable2.Append("<table class=\"lefttablebody\" style=\"width:100%;height:357px;\">");

        //                    #region left Table Body Start

        //                    int z = 0;

        //                    foreach (DataRow FrstColName in bgmTable2.Rows)
        //                    {
        //                        string superScript = string.Empty;


        //                        #region left table row content
        //                        int i = Convert.ToInt32(FrstColName[1]);
        //                        bool b = true;
        //                        int j = 0;
        //                        int c = 0;
        //                        if (z > 0)
        //                        {
        //                            for (int k = 0; k < bgmTable2.Rows.Count; k++)
        //                            {
        //                                if (k < z)
        //                                {
        //                                    c = c + Convert.ToInt32(bgmTable2.Rows[k][1]);
        //                                }
        //                            }
        //                        }

        //                        int f = 0;
        //                        foreach (var c3 in col3)
        //                        {
        //                            switch (j)
        //                            {
        //                                case 0:
        //                                    superScript = "1";
        //                                    break;

        //                                case 1:
        //                                case 4:
        //                                case 6:
        //                                case 14:
        //                                    superScript = "2";
        //                                    break;

        //                                case 2:
        //                                case 5:
        //                                case 7:
        //                                case 16:
        //                                    superScript = "3";
        //                                    break;

        //                                case 3:
        //                                case 8:
        //                                case 18:
        //                                    superScript = "4";
        //                                    break;

        //                                case 9:
        //                                case 12:
        //                                case 15:
        //                                    superScript = "5";
        //                                    break;

        //                                case 10:
        //                                case 13:
        //                                case 17:
        //                                    superScript = "6";
        //                                    break;

        //                                case 11:
        //                                case 19:
        //                                    superScript = "7";
        //                                    break;

        //                            }
        //                            if (j >= c)
        //                            {
        //                                if (f < i)
        //                                {
        //                                    leftTable2.Append("<tr>");
        //                                    if (b)
        //                                    {
        //                                        leftTable2.Append("<td style=\"height:20px;border:1px solid lightgrey;width:87px;font-size:12px;\" rowspan='" + FrstColName[1] + "'> " + FrstColName[0] + "</td>");
        //                                        b = false;
        //                                    }
        //                                    leftTable2.Append("<td style=\"height:20px;border:1px solid lightgrey;width:200px;font-size:12px;\">");
        //                                    leftTable2.Append(c3 + "<sup>" + superScript + "</sup>");

        //                                    leftTable2.Append("</td>");
        //                                    leftTable2.Append("</tr>");
        //                                }
        //                                f++;
        //                                //if (c == f)
        //                                //    break;
        //                            }
        //                            j++;
        //                        }
        //                        #endregion left table row content
        //                        z++;
        //                    }

        //                    #region left Table Empty Row Start
        //                    leftTable2.Append("<tr>");
        //                    leftTable2.Append("<td style=\"height:20px;border:1px solid lightgrey;width:87px;\"></td>");
        //                    leftTable2.Append("<td style=\"height:20px;border:1px solid lightgrey;width:200px;\">");
        //                    leftTable2.Append("</td>");
        //                    leftTable2.Append("</tr>");
        //                    #endregion left Table Empty Row End



        //                    #endregion left Table Body End
        //                    #region left Sample Table Start

        //                    bool rowspan = true;
        //                    int sampleRowCount = 1;
        //                    foreach (var c3sample in col3Sample)
        //                    {

        //                        #region left table row content
        //                        leftTable2.Append("<tr>");
        //                        if (rowspan)
        //                        {
        //                            leftTable2.Append("<td style=\"height:20px;border:1px solid lightgrey;width:87px;font-size:12px; color: #878787;font-weight:bold;\" rowspan=\"7\">" + " Sample Size " + "</td>");
        //                            rowspan = false;
        //                        }
        //                        leftTable2.Append("<td style=\"height:20px;border:1px solid lightgrey;width:200px;font-size:12px;color: #878787;font-weight:bold;\">");
        //                        leftTable2.Append(c3sample.Replace("Sample", "") + "<sup>" + sampleRowCount + "</sup>");
        //                        leftTable2.Append("</td>");
        //                        leftTable2.Append("</tr>");
        //                        #endregion left table row content
        //                        sampleRowCount++;
        //                    }

        //                    #endregion left Sample Table End
        //                    leftTable2.Append("</table>");
        //                    #endregion Left Table End

        //                    #region Right Table Start
        //                    int widthrightTable = col1.Count * 700;
        //                    rightTable1.Append("<table class=\"righttableheader\" style=\"width:" + widthrightTable + "px;\">");

        //                    #region right Table Header Start

        //                    //rightTable1.Append("<thead>");

        //                    #region Row 1

        //                    //bool BenchCheck = true;
        //                    rightTable1.Append("<tr>");
        //                    for (int i = 0; i < 5; i++)
        //                    {
        //                        //rightTable1.Append("<td colspan=\"5\" style=\"background-color: #E41E2A;border: 1px solid white; color: white; font-size: 16px; height: auto; text-align: center; font-weight: bold;\" class=\"Benchmarktitle\">");
        //                        if (i == 2)
        //                        {
        //                            rightTable1.Append("<td  style=\"background-color: #E41E2A; color: white; font-size: 16px; height: 20px; text-align: center; font-weight: bold;\" class=\"Benchmarktitle\">");
        //                            rightTable1.Append("BENCHMARK");
        //                            rightTable1.Append("</td>");
        //                        }
        //                        else
        //                        {
        //                            rightTable1.Append("<td  style=\"background-color: #E41E2A; color: white; font-size: 16px; height: 20px; text-align: center; font-weight: bold;\" class=\"Benchmarktitle\"></td>");
        //                        }
        //                    }
        //                    //rightTable1.Append("BENCHMARK");
        //                    //rightTable1.Append("</td>");

        //                    int colsPan = (col1.Count - 1) * 5;
        //                    //rightTable1.Append("<td  colspan='" + colsPan + "' style=\"background-color: #E41E2A;border: 0; color: white; font-size: 16px; height: auto; text-align: center; font-weight: bold;\" class=\"comparisonheader\">");
        //                    //rightTable1.Append("COMPARISION AREAS");
        //                    //rightTable1.Append("</td>");
        //                    bool compAreaCheck = true;
        //                    foreach (var c1 in CompList)
        //                    {
        //                        for (int i = 0; i < 5; i++)
        //                        {
        //                            if (compAreaCheck)
        //                            {
        //                                rightTable1.Append("<td  style=\"background-color: #E41E2A;border: 0;border-left:1px solid white; color: white; font-size: 15px; height: auto; text-align: center; font-weight: bold;\" class=\"comparisonheader " + CleanClass(objcomFunc.Get_ShortNames(Convert.ToString(c1.Replace("~", "`").Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "")))).Replace("Grocery", "SupermarketGrocery") + "header" + "\">COMPARISION AREAS</td>");
        //                            }
        //                            else
        //                            {
        //                                rightTable1.Append("<td  style=\"background-color: #E41E2A;border: 0; color: white; font-size: 15px; height: auto; text-align: center; font-weight: bold;\" class=\"comparisonheader " + CleanClass(objcomFunc.Get_ShortNames(Convert.ToString(c1.Replace("~", "`").Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "")))).Replace("Grocery", "SupermarketGrocery") + "header" + "\"></td>");
        //                            }
        //                            compAreaCheck = false;
        //                        }

        //                    }


        //                    rightTable1.Append("</tr>");

        //                    #endregion Row 1

        //                    #region Row 2
        //                    rightTable1.Append("<tr class=\"rtRow2\" style=\"min-height:30px;max-height:" + _HeaderMaxHeightSecondRow + "px;\">");

        //                    int AdBenchMrk = 0;
        //                    foreach (var c1 in col1)
        //                    {
        //                        if (AdBenchMrk == 0)
        //                        {

        //                            rightTable1.Append("<td colspan='5' style=\"background-color: #808080;font-size:15px;border:1px solid white;border-left:0; font-weight: normal;width:700px;\" class=\"benchmarkheader\">" + objcomFunc.Get_ShortNames(c1).Replace("~", "`") + "</td>");
        //                        }
        //                        else
        //                        {
        //                            rightTable1.Append("<td colspan='5' style=\"background-color: #808080;border: 1px solid lightgrey;color:white;text-align:center;font-size:15px;font-weight: normal;width:700px;\" class=\"comparisonheader " + CleanClass(objcomFunc.Get_ShortNames(c1)).Replace("Grocery", "SupermarketGrocery") + "header" + "\">" + objcomFunc.Get_ShortNames(c1).Replace("Grocery", "SupermarketGrocery") + "</td>");

        //                        }
        //                        AdBenchMrk++;
        //                    }
        //                    rightTable1.Append("</tr>");
        //                    #endregion Row 2

        //                    #region Row 3
        //                    rightTable1.Append("<tr class=\"rtRow2\" style=\"min-height:40px;max-height:" + _HeaderMaxHeightThirdRow + "px;\">");
        //                    for (int i = 0; i < col1.Count; i++)
        //                    {
        //                        foreach (var c4 in col4)
        //                        {
        //                            if (i == 0)
        //                            {
        //                                rightTable1.Append("<td style=\"background-color: #808080;font-size:12px;border:1px solid white;border-left:0;font-weight: normal;width:140px;\" class=\"benchmarkheader\">" + c4 + "</td>");
        //                            }
        //                            else
        //                            {
        //                                rightTable1.Append("<td style=\"background-color: #808080;font-size:12px;border:1px solid white;color:white;text-align:center;font-weight: normal;width:140px;\" class=\"comparisonheader " + CleanClass(objcomFunc.Get_ShortNames(Convert.ToString(col1[i]))).Replace("Grocery", "SupermarketGrocery") + "header" + "\">" + c4 + "</td>");

        //                            }
        //                        }
        //                    }
        //                    rightTable1.Append("</tr>");
        //                    #endregion Row 3

        //                    //rightTable1.Append("</thead>");

        //                    #endregion right Table Header Start
        //                    rightTable1.Append("</table>");

        //                    rightTable2.Append("<table class=\"righttablebody\" style=\"width:" + widthrightTable + "px;\">");
        //                    rightTable2.Append("<tbody>");


        //                    #region right Table Body Start


        //                    int rowcount = 0;

        //                    //List<Int32> valuesListsSample = new List<Int32>();
        //                    List<Int32> valuesListsSample1 = new List<Int32>();
        //                    List<Int32> valuesListsSample2 = new List<Int32>();
        //                    List<Int32> valuesListsSample3 = new List<Int32>();
        //                    List<Int32> valuesListsSample4 = new List<Int32>();
        //                    List<Int32> valuesListsSample5 = new List<Int32>();
        //                    List<Int32> valuesListsSample6 = new List<Int32>();
        //                    List<Int32> valuesListsSample7 = new List<Int32>();

        //                    int ca = 0;
        //                    foreach (var c3sample in col3Sample)
        //                    {
        //                        var valList1 = from r in bgmTable1.AsEnumerable() where r.Field<string>("ColumnValue") == c3sample select r.Field<Int32>(4);
        //                        //if (ca == 0)
        //                        //    valuesListsSample1 = valList1.ToList();
        //                        //else 
        //                        //    valuesListsSample2  = valList1.ToList();

        //                        switch (ca)
        //                        {
        //                            case 0:
        //                                valuesListsSample1 = valList1.ToList();
        //                                break;
        //                            case 1:
        //                                valuesListsSample2 = valList1.ToList();
        //                                break;
        //                            case 2:
        //                                valuesListsSample3 = valList1.ToList();
        //                                break;
        //                            case 3:
        //                                valuesListsSample4 = valList1.ToList();
        //                                break;
        //                            case 4:
        //                                valuesListsSample5 = valList1.ToList();
        //                                break;
        //                            case 5:
        //                                valuesListsSample6 = valList1.ToList();
        //                                break;
        //                            case 6:
        //                                valuesListsSample7 = valList1.ToList();
        //                                break;

        //                        }


        //                        ca++;
        //                    }

        //                    foreach (var c3 in col3)
        //                    {
        //                        List<Int32> valuesListsSampleChecking = new List<Int32>();
        //                        var valList = from r in bgmTable.AsEnumerable() where r.Field<string>("ColumnValue") == c3 select r.Field<double>(4);
        //                        valuesLists = valList.ToList();

        //                        List<string> valueListSignificance = new List<string>();
        //                        var valSignfiList = from r in bgmTable.AsEnumerable() where r.Field<string>("ColumnValue") == c3 select Convert.ToString(r.Field<object>(5)) == string.Empty ? "0" : Convert.ToString(r.Field<object>(5));
        //                        valueListSignificance = valSignfiList.ToList();

        //                        string separatedValue = string.Empty;
        //                        rightTable2.Append("<tr style=\"border-right:1px solid lightgrey;border-top:1px solid lightgrey;\">");
        //                        significanceColor = "color:black";
        //                      string applybluecolor = string.Empty;

        //                        int colIndex_ = 0;
        //                        string backGrdCelClr = string.Empty;
        //                        backGrdCelClr = "background-color:white;";
        //                        string checkSamplesize = string.Empty;



        //                        foreach (var l in valuesLists)
        //                        {
        //                            backGrdCelClr = "background-color:white;";
        //                            string shopperValue = Convert.ToString(valuesListsSample1[colIndex_]);
        //                            string tripsValue = Convert.ToString(valuesListsSample2[colIndex_]);
        //                            //string fValue = CommonFunctions.CheckLowSampleSize(shopperValue, tripsValue);
        //                            switch(rowcount)
        //                            {
        //                                case 0:
        //                                checkSamplesize = Convert.ToString(valuesListsSample1[colIndex_]);
        //                                    break;

        //                                case 1:
        //                                case 4:
        //                                case 6:
        //                                case 14:
        //                                     checkSamplesize = Convert.ToString(valuesListsSample2[colIndex_]);
        //                                    break;

        //                                case 2:
        //                                case 5:
        //                                case 7:
        //                                case 16:
        //                                     checkSamplesize = Convert.ToString(valuesListsSample3[colIndex_]);
        //                                    break;

        //                                case 3:
        //                                case 8:
        //                                case 18:
        //                                     checkSamplesize = Convert.ToString(valuesListsSample4[colIndex_]);
        //                                    break;

        //                                case 9:
        //                                case 12:
        //                                case 15:
        //                                     checkSamplesize = Convert.ToString(valuesListsSample5[colIndex_]);
        //                                    break;

        //                                case 10:
        //                                case 13:
        //                                case 17:
        //                                     checkSamplesize = Convert.ToString(valuesListsSample6[colIndex_]);
        //                                    break;

        //                                case 11:
        //                                case 19:
        //                                    checkSamplesize = Convert.ToString(valuesListsSample7[colIndex_]);
        //                                    break;

        //                            }

        //                            string fValue = objcomFunc.CommonFunctions.CheckLowSampleSize(checkSamplesize);

        //                            if (c3.Contains("Average Basket"))
        //                            {
        //                                separatedValue = "$" + l.ToString("0");
        //                            }
        //                            else if (rowcount == 9 || rowcount == 10 || rowcount == 11)
        //                            {
        //                                separatedValue = l.ToString("#,#", CultureInfo.InvariantCulture);//(String.Format("{0:#,###,###}", l));
        //                                if (l == 0)
        //                                {
        //                                    separatedValue = "0";
        //                                }
        //                            }
        //                            else if (rowcount == 12 || rowcount == 13)
        //                            {
        //                                BenchmarkOrComparison = Benchmark;
        //                                significanceColor = GetCellColor(Convert.ToDouble(valueListSignificance[colIndex_]));

        //                                separatedValue = l.ToString("0.0") + "%";
        //                            }
        //                            else
        //                            {
        //                                separatedValue = CommaSeparatedValues(l, c3, Convert.ToDouble(valueListSignificance[colIndex_]));
        //                            }

        //                            if (colIndex_ <= 4)
        //                            {
        //                                if ((_BeverageType.Replace(" ", "") == "NonBeverages" && Benchmark.Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (_BeverageType.Replace(" ", "") == "Beverages" && Benchmark.Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
        //                                {
        //                                    separatedValue = "<span style=\"color:#969696;\">NA*</span>";
        //                                }
        //                                else if (_BeverageType.Replace(" ", "") == "NonBeverages"  && shopperValue == "0" && tripsValue == "0")
        //                                {
        //                                    separatedValue = "<span style=\"color:#969696;\">NA*</span>";
        //                                }
        //                                else if (fValue == GlobalVariables.LowSampleSize)
        //                                {
        //                                    separatedValue = "";
        //                                    backGrdCelClr = "background-color:white;";

        //                                }
        //                                else if (fValue == GlobalVariables.UseDirectionally)
        //                                {
        //                                    backGrdCelClr = "background-color: rgb(230, 230, 230);";
        //                                }
        //                                else
        //                                {
        //                                    //separatedValue = separatedValue;
        //                                    backGrdCelClr = "background-color:white;";
        //                                }
        //                                //applybluecolor= GetCellColor(Convert.ToDouble(valueListSignificance[colIndex_]));

        //                                if (separatedValue.Contains("%") && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
        //                                {

        //                                    rightTable2.Append("<td  class=\"benchmarkcell\" style=\"height:20px;border:1px solid lightgrey;font-size:12px;text-align:center;width:140px;color:blue;"  + backGrdCelClr + "\">");

        //                                }
        //                                else
        //                                {
        //                                    rightTable2.Append("<td  class=\"benchmarkcell\" style=\"height:20px;border:1px solid lightgrey;font-size:12px;text-align:center;width:140px;color:black;" +   backGrdCelClr + "\">");

        //                                }
        //                                rightTable2.Append(separatedValue);

        //                                rightTable2.Append("</td>");
        //                            }
        //                            else
        //                            {
        //                                int compIndex = 0;
        //                                compIndex= objcomFunc.GetCompIndex(colIndex_);

        //                                if ((_BeverageType.Replace(" ", "") == "NonBeverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (_BeverageType.Replace(" ", "") == "Beverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
        //                                {
        //                                    separatedValue = "<span style=\"color:#969696;\">NA*</span>";
        //                                }
        //                                else if (_BeverageType.Replace(" ", "") == "NonBeverages" && shopperValue == "0" && tripsValue == "0")
        //                                {
        //                                    separatedValue = "<span style=\"color:#969696;\">NA*</span>";
        //                                }
        //                                else if (fValue == GlobalVariables.LowSampleSize)
        //                                {
        //                                    separatedValue = "";
        //                                    backGrdCelClr = "background-color:white;";

        //                                }
        //                                else if (fValue == GlobalVariables.UseDirectionally)
        //                                {
        //                                    backGrdCelClr = "background-color: rgb(230, 230, 230);";
        //                                }
        //                                else
        //                                {
        //                                    backGrdCelClr = "background-color:white;";
        //                                }

        //                                rightTable2.Append("<td  class=\"" + GetClassNames(colIndex_, col1) + "cell\" style=\"height:20px;border:1px solid lightgrey;" + backGrdCelClr + "font-size:12px;text-align:center;width:140px;" + significanceColor + " \">");
        //                                rightTable2.Append(separatedValue);

        //                                rightTable2.Append("</td>");
        //                            }


        //                            colIndex_++;
        //                        }
        //                        rightTable2.Append("</tr>");
        //                        rowcount++;
        //                    }


        //                    rightTable2.Append("</tbody>");

        //                    #endregion right Table Body End
        //                    #region right Table Body SampleSize Start
        //                    List<Int32> valuesListsSample = new List<Int32>();

        //                    #region right Table Empty Row Start

        //                    foreach (var c3sample in col3Sample)
        //                    {
        //                        var valList = from r in bgmTable1.AsEnumerable() where r.Field<string>("ColumnValue") == c3sample select r.Field<Int32>(4);
        //                        valuesListsSample = valList.ToList();
        //                        rightTable2.Append("<tr style=\"height:20px;border-right:1px solid lightgrey;border-top:1px solid lightgrey;\">");
        //                        int colIndexSample_ = 0;
        //                        foreach (var l in valuesListsSample)
        //                        {
        //                            if (colIndexSample_ <= 4)
        //                            {
        //                                rightTable2.Append("<td class=\"benchmarkcell\" style=\"border-right:1px solid lightgrey;font-size:13px;width:140px;\">");
        //                            }
        //                            else
        //                            {
        //                                rightTable2.Append("<td class=\"" + GetClassNames(colIndexSample_, col1) + "cell\" style=\"border-right:1px solid lightgrey;font-size:13px;width:140px;\">");
        //                            }
        //                            rightTable2.Append("");
        //                            rightTable2.Append("</td>");
        //                            colIndexSample_++;
        //                        }
        //                        rightTable2.Append("</tr>");
        //                        break;
        //                    }
        //                    #endregion right Table Empty Row End

        //                    List<Int32> valuesListsSampleSize1 = new List<Int32>();
        //                    List<Int32> valuesListsSampleSize2 = new List<Int32>();

        //                    int ca1 = 0;
        //                    foreach (var c3sample in col3Sample)
        //                    {
        //                        var valList1 = from r in bgmTable1.AsEnumerable() where r.Field<string>("ColumnValue") == c3sample select r.Field<Int32>(4);
        //                        if (ca1 == 0)
        //                            valuesListsSampleSize1 = valList1.ToList();
        //                        else
        //                            valuesListsSampleSize2 = valList1.ToList();

        //                        ca1++;
        //                    }

        //                    foreach (var c3sample in col3Sample)
        //                    {
        //                        var valList = from r in bgmTable1.AsEnumerable() where r.Field<string>("ColumnValue") == c3sample select r.Field<Int32>(4);
        //                        valuesListsSample = valList.ToList();

        //                        rightTable2.Append("<tr style=\"border-right:1px solid lightgrey;border-top:1px solid lightgrey;border-bottom:1px solid lightgrey;\">");
        //                        int colIndexSampleVal_ = 0;
        //                        foreach (var l in valuesListsSample)
        //                        {
        //                            string shopperValue = Convert.ToString(valuesListsSampleSize1[colIndexSampleVal_]);
        //                            string tripsValue = Convert.ToString(valuesListsSampleSize2[colIndexSampleVal_]);


        //                            string showVal = string.Empty;
        //                            string SampleSizeText = objcomFunc.CommonFunctions.CheckLowSampleSize(Convert.ToString(l));
        //                            string backgrndClr = string.Empty;
        //                            backgrndClr = "background-color:white;";
        //                            showVal = Convert.ToString(l);

        //                            if (colIndexSampleVal_ <= 4)
        //                            {
        //                                if ((_BeverageType.Replace(" ", "") == "NonBeverages" && Benchmark.Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (_BeverageType.Replace(" ", "") == "Beverages" && Benchmark.Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
        //                                {
        //                                    showVal = "NA*";
        //                                }
        //                                else if (_BeverageType.Replace(" ", "") == "NonBeverages" && shopperValue == "0" && tripsValue == "0")
        //                                {
        //                                    showVal = "NA*";
        //                                }
        //                                else
        //                                {
        //                                    if (l >= 50 && l <= 100)
        //                                    {
        //                                        showVal = Convert.ToString(l.ToString("#,#", CultureInfo.InvariantCulture));
        //                                        backgrndClr = "background-color: rgb(230, 230, 230);";
        //                                    }
        //                                    else
        //                                    {
        //                                        showVal = Convert.ToString(l.ToString("#,#", CultureInfo.InvariantCulture));
        //                                        backgrndClr = "background-color:white;";
        //                                    }

        //                                }
        //                                rightTable2.Append("<td class=\"benchmarkcell\" style=\"border-right:1px solid lightgrey;font-size:13px;border-bottom:1px solid lightgrey;text-align:center;width:140px;color:#878787;font-weight:bold;" + backgrndClr + "\">");
        //                                //if (l == 0)
        //                                //    rightTable2.Append(l + " " + SampleSizeText);
        //                                if (showVal == "NA*")
        //                                {
        //                                    showVal = "<span style=\"color:#969696;font-weight:normal;\">NA*</span>";
        //                                    rightTable2.Append(showVal);
        //                                }
        //                                else if (showVal == "")
        //                                {
        //                                    rightTable2.Append("0" + " " + SampleSizeText);
        //                                }
        //                                else
        //                                    rightTable2.Append(showVal + " " + SampleSizeText);

        //                                rightTable2.Append("</td>");
        //                            }
        //                            else
        //                            {
        //                                int compIndex = 0;
        //                                compIndex = objcomFunc.GetCompIndex(colIndexSampleVal_);

        //                                if ((_BeverageType.Replace(" ", "") == "NonBeverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0") || (_BeverageType.Replace(" ", "") == "Beverages" && CompList[compIndex].Contains("|Non Priority Retailers") && shopperValue == "0" && tripsValue == "0"))
        //                                {
        //                                    showVal = "NA*";
        //                                }
        //                                else if (_BeverageType.Replace(" ", "") == "NonBeverages" && shopperValue == "0" && tripsValue == "0")
        //                                {
        //                                    showVal = "NA*";
        //                                }
        //                                else
        //                                {
        //                                    if (l >= 50 && l <= 100)
        //                                    {
        //                                        showVal = Convert.ToString(l.ToString("#,#", CultureInfo.InvariantCulture));
        //                                        backgrndClr = "background-color: rgb(230, 230, 230);";
        //                                    }
        //                                    else
        //                                    {
        //                                        showVal = Convert.ToString(l.ToString("#,#", CultureInfo.InvariantCulture));
        //                                        backgrndClr = "background-color:white;";
        //                                    }

        //                                }

        //                                rightTable2.Append("<td class=\"" + GetClassNames(colIndexSampleVal_, col1) + "cell\" style=\"border-right:1px solid lightgrey;font-size:13px;border-bottom:1px solid lightgrey;text-align:center;width:140px;color:#878787;font-weight:bold;" + backgrndClr + "\">");
        //                                //if (l == 0)
        //                                //    rightTable2.Append(l + " " + SampleSizeText);
        //                                if (showVal == "NA*")
        //                                {
        //                                    showVal = "<span style=\"color:#969696;font-weight:normal;\">NA*</span>";
        //                                    rightTable2.Append(showVal);

        //                                }
        //                                else if (showVal == "")
        //                                {
        //                                    rightTable2.Append("0" + " " + SampleSizeText);
        //                                }
        //                                else
        //                                    rightTable2.Append(showVal + " " + SampleSizeText);

        //                                rightTable2.Append("</td>");

        //                            }

        //                            colIndexSampleVal_++;
        //                        }
        //                        rightTable2.Append("</tr>");
        //                    }

        //                    #endregion Right Table Body SampleSize End
        //                    rightTable2.Append("</table>");
        //                    #endregion Right Table End

        //                    #region Main Table Start

        //                    mainTable.Append("<div class=\"lefttablecontent\" style=\"float:left;width:387px;\">");
        //                    mainTable.Append("<div class=\"leftheader\" style=\"clear:both;width:387px;\">");
        //                    mainTable.Append(leftTable1);
        //                    mainTable.Append("</div>");
        //                    mainTable.Append("<div class=\"leftbody\" style=\"clear:both;width:387px;min-height:150px;max-height:" + (_MaxHeight - 18) + "px;overflow:hidden;border-bottom:2px solid grey;\">");
        //                    mainTable.Append(leftTable2);
        //                    mainTable.Append("</div>");
        //                    mainTable.Append("</div>");

        //                    mainTable.Append("<div class=\"righttablecontent\" style=\"float:left;min-width:100px; max-width:" + _MaxWidth + "px\" >");
        //                    mainTable.Append("<div class=\"rightheader\" style=\"clear:both;width:" + "537" + "px;overflow: hidden;\">");
        //                    mainTable.Append(rightTable1);
        //                    mainTable.Append("</div>");
        //                    mainTable.Append("<div onscroll=\"reposVertical(this);\" class=\"righttbody\" style=\"clear:both;width:557px;min-height:170px;max-height:" + _MaxHeight + "px; overflow:auto;\">");
        //                    mainTable.Append(rightTable2);
        //                    mainTable.Append("</div>");
        //                    mainTable.Append("</div>");
        //                    #endregion Main Table Start


        //                }
        //                else
        //                {
        //                    ErrorLog.LogError("Tables are not Equal", "592");
        //                }


        //        }
        //        else
        //        {
        //            #region NO Data Available Start
        //            //int colsPan = (CompList.Count - 1) * 5;
        //            mainTable.Append("<div class=\"lefttablecontent\" style=\"float:left;width:387px;\">");
        //            mainTable.Append("<div class=\"leftheader\" style=\"clear:both;width:387px;\">");
        //            mainTable.Append("<table class=\"lefttableheader\" style=\"width:100%;\">");
        //            mainTable.Append("<tr>");
        //            mainTable.Append("<td style=\"background-color: #E41E2A;border-right:0\" class=\"BenchComp\"></td>");
        //            mainTable.Append("</tr>");
        //            mainTable.Append("<tr>");
        //            mainTable.Append("<td style=\"background-color: #E41E2A;color:white;height: 40px;text-align: center;\" class=\"ShoppingFrequencyheader\"></td>");
        //            mainTable.Append("</tr>");
        //            mainTable.Append("</table>");
        //            mainTable.Append("</div>");
        //            mainTable.Append("<div class=\"leftbody\" style=\"clear:both;width:387px;overflow:hidden;\">");
        //            mainTable.Append("<table>");
        //            mainTable.Append("<tr>");
        //            mainTable.Append("<td>");
        //            mainTable.Append("NO Data Available");
        //            mainTable.Append("</td>");
        //            mainTable.Append("</tr>");
        //            mainTable.Append("</table>");

        //            mainTable.Append("</div>");
        //            mainTable.Append("</div>");
        //            mainTable.Append("<div class=\"righttablecontent\" style=\"float:left;width:" + "537" + "px;\">");
        //            mainTable.Append("<div class=\"rightheader\" style=\"clear:both;width:" + "537" + "px;overflow: hidden;\">");
        //            mainTable.Append("<table class=\"righttableheader\" style=\"width:" + ((1 + CompList.Count) * 200) + "px\">");
        //            mainTable.Append("<tr>");
        //            mainTable.Append("<td style=\"background-color: #E41E2A;border: 1px solid white; color: white; font-size: 16px; height: auto; text-align: center; width: 200px;font-weight: bold;\" class=\"Benchmarktitle\">Benchmark</td>");
        //            //mainTable.Append("<td style=\"background-color: #E41E2A;border: 0; color: white; font-size: 16px; height: auto; text-align: center; width: "+ (CompList.Count * 200) +"px;font-weight: bold;\" class=\"comparisonheader\" colspan='" + CompList.Count +"'>Comaprison Areas</td>");
        //            bool compAreaCheck = true;
        //            foreach (var c1 in CompList)
        //            {

        //                    if (compAreaCheck)
        //                    {
        //                        mainTable.Append("<td  style=\"background-color: #E41E2A;border: 0; color: white; font-size: 15px; height: auto; text-align: center; font-weight: bold;width:200px;\" class=\"comparisonheader " + CleanClass(objcomFunc.Get_ShortNames(Convert.ToString(c1.Replace("~", "`").Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "")))) + "header" + "\">COMPARISION AREAS</td>");
        //                    }
        //                    else
        //                    {
        //                        mainTable.Append("<td  style=\"background-color: #E41E2A;border: 0; color: white; font-size: 15px; height: auto; text-align: center; font-weight: bold;width:200px;\" class=\"comparisonheader " + CleanClass(objcomFunc.Get_ShortNames(Convert.ToString(c1.Replace("~", "`").Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "")))) + "header" + "\"></td>");
        //                    }
        //                    compAreaCheck = false;


        //            }
        //            mainTable.Append("</tr>");
        //            mainTable.Append("<tr>");
        //            mainTable.Append("<td style=\"background-color: #808080;\" class=\"benchmarkheader\">" + objcomFunc.Get_ShortNames(_BenchMark.Replace("~", "`").Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "")) + "</td>");


        //            foreach (var c3 in CompList)
        //            {
        //                mainTable.Append("<td style=\"background-color: #808080;border: 1px solid lightgrey;\" class=\"comparisonheader " + CleanClass(objcomFunc.Get_ShortNames(Convert.ToString(c3.Replace("~", "`").Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "")))) + "header" + "\">" + objcomFunc.Get_ShortNames(c3.Replace(",", "|").Replace("~", "`").Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", "")) + "</td>");
        //            }
        //            mainTable.Append("</tr>");

        //            //mainTable.Append("<tr>");
        //            //mainTable.Append("<td style=\"color:black;\">" + "No Data Available" + "</td>");
        //            //mainTable.Append("<td></td>");
        //            //mainTable.Append("</tr>");
        //            mainTable.Append("</table>");
        //            mainTable.Append("</div>");
        //            mainTable.Append("<div onscroll=\"reposVertical(this);\" class=\"righttbody\" style=\"clear:both;width:" + "554" + "px;height:" + "375" + "px;overflow:auto;\">");
        //            mainTable.Append("<table class=\"righttablebody\" style=\"width:" + ((1 + CompList.Count) * 200) + "px;border:0;\">");
        //            mainTable.Append("<tr>");
        //            mainTable.Append("<td>");
        //            mainTable.Append("<td>");
        //            mainTable.Append("</tr>");
        //            mainTable.Append("</table>");
        //            mainTable.Append("</div>");
        //            mainTable.Append("</div>");
        //            #endregion NO Data Available End
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.LogError(ex.Message, ex.StackTrace);
        //    }
        //    return mainTable.ToString();
        //}

        //writen by Nagaraju for BGM Revamp 04/04/2016
        public iSHOPParams PlotData(string _BenchMark, string _timePeriod, string _previoustimePeriod, string _ShopperFrequency, string _selectionBevorNonBev, string _filter, string _timeType, string _BevorNonBevShortName, string _SelectedBevorNonBevShortName, string _BenchmarkShortName, string FilterShortNames, string TimePeriodUniqueId, string ComparisonUniqueId, string BevUniqueIds, string AdvancedFiltersUniqueId, string FrequencyUniqueId,List<string> BeverageNonBeveragelist)
        {
            iSHOPParams ishopParams = new iSHOPParams();
            BGMParams bgmParams = new BGMParams();
            ishopParams.IsNoDataAvailable = false;
            bgmParams.BenchMark = _BenchMark;
            _BenchmarkShortName = _BenchmarkShortName.Replace("~", "`");
            bgmParams.BenchMarkShortName = _BenchmarkShortName.Replace("~", "`");
            bgmParams.TimePeriod = _timeType;
            bgmParams.PreviousTimePeriod = _previoustimePeriod;
            bgmParams.ShopperFrequency = _ShopperFrequency;
            bgmParams.CustomFilters = _filter;
            bgmParams.FilterShortNames = FilterShortNames;
            Benchmark = _BenchMark;
            bgmParams.selectedProduct = _SelectedBevorNonBevShortName;
            bgmParams.BeverageNonBeveragelist = BeverageNonBeveragelist;
            //bgmParams.

            if (HttpContext.Current.Session["StatSessionPosi"] != null)
                accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
            if (HttpContext.Current.Session["StatSessionNega"] != null)
                accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
            if (HttpContext.Current.Session["PercentStat"] != null)
                PercentStat = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);

            bgmParams.statpositive = Convert.ToString(accuratestatvalueposi);
            bgmParams.statnegative = Convert.ToString(accuratestatvaluenega);
            bgmParams.PercentStat = Convert.ToString(PercentStat);

            selectedProduct = string.Empty;
            string ChangeBasis = string.Empty;
            string ImpactToSales = string.Empty;

            string ShopperSampleSize = string.Empty;
            string TripsSampleSize = string.Empty;

            DataTable tbl = null;
            sampleSizeHeaderTable = new StringBuilder();
            sampleSizeBodyTable = new StringBuilder();

            leftheader = new StringBuilder();
            rightheader = new StringBuilder();

            leftbody = new StringBuilder();
            rightbody = new StringBuilder();
            NonBeverageItem = false;
            try
            {
                if (!string.IsNullOrEmpty(_SelectedBevorNonBevShortName))
                {
                    _SelectedBevorNonBevShortName = _SelectedBevorNonBevShortName.Replace("~", "`");
                    selectedProduct = _SelectedBevorNonBevShortName.Split('|').ToList()[_SelectedBevorNonBevShortName.Split('|').ToList().Count - 1];
                }
                DataAccess dal = new DataAccess();
                //object[] paramvalues = new object[] { _timePeriod, _BenchMark.Replace("~", "`").Replace("|Priority Retailers", "").Replace("|Non Priority Retailers", ""), _selectionBevorNonBev, _filter, _ShopperFrequency };
                object[] paramvalues = new object[] { TimePeriodUniqueId, ComparisonUniqueId, BevUniqueIds, AdvancedFiltersUniqueId, FrequencyUniqueId };
                DataSet ds = dal.GetData_WithIdMapping(paramvalues, "usp_IshopBGMReport");
                //Plot Sample Size Header Table
                if (ds != null && ds.Tables.Count > 0)
                {
                    sampleSizeHeaderTable.Append("<table id=\"bgmsamplesizeheadertable\" style=\"clear:both;width:100%;\">");

                    //add first left table header row
                    //sampleSizeHeaderTable.Append("<tr>");

                    //sampleSizeHeaderTable.Append("<td style=\"background-color: #e41e2a;width:200px;\" rowspan=\"2\"></td>");
                    //sampleSizeHeaderTable.Append("<td style=\"background-color: #e41e2a;text-align:center;color:white;font-weight: bold;\"><span id=\"bgmretailer\" class=\"Benchmark_LabelBGM\">" + _BenchmarkShortName.ToUpper() + "</span></td>");
                    //sampleSizeHeaderTable.Append("</tr>");
                    //
                    //add second header row
                    sampleSizeHeaderTable.Append("<tr style=\"background-color: #d5d6d6; border-bottom: 1px solid #686868;color: #000000; font-weight: bold; line-height: 24px;height:25px;\">");
                    sampleSizeHeaderTable.Append("<td style=\"position:relative;\"><a class=\"table-top-title-bottom-line\"></a><span id=\"bgmretailer\" class=\"Benchmark_LabelBGM\" style=\"font-size:12px;\">" + _BenchmarkShortName.ToUpper() + "</span></td>");
                    sampleSizeHeaderTable.Append("</tr>");

                    sampleSizeHeaderTable.Append("</table>");
                    //end Sample Size header----------------------->

                    //Plot Sample Size Body Table

                    ShopperSampleSize = (from row in ds.Tables[0].AsEnumerable()
                                         where
                                             Convert.ToString(row.Field<object>("Objective")).Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)
                                               && Convert.ToString(row.Field<object>("Section")).Equals("Shopper", StringComparison.OrdinalIgnoreCase)
                                         select Convert.ToString(row.Field<object>("SampleSize"))).FirstOrDefault();

                    TripsSampleSize = (from row in ds.Tables[0].AsEnumerable()
                                       where
                                           Convert.ToString(row.Field<object>("Objective")).Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)
                                             && Convert.ToString(row.Field<object>("Section")).Equals("Product Trips", StringComparison.OrdinalIgnoreCase)
                                       select Convert.ToString(row.Field<object>("SampleSize"))).FirstOrDefault();


                    sampleSizeBodyTable.Append("<table id=\"bgmsamplesizebodytable\" style=\"clear:both;width:100%;margin-top:1px;\">");

                    //add first left table header row
                    sampleSizeBodyTable.Append("<tr>");

                    sampleSizeBodyTable.Append("<td style=\"background-color: rgb(72,99,112);color:white;text-align:center;width:11.9%;\" rowspan=\"2\"><span class=\"span_samplesize\">SAMPLE SIZE<br/><span style=\"font-size:10px;\">(CURRENT YEAR)</span></span></td>");
                    sampleSizeBodyTable.Append("<td style=\"background-color: rgb(242,242,242);font-weight:bold;width:32.95%;\"><span class=\"span_samplesize\" style=\"float:left;\">SHOPPERS</span></td>");

                    if (Convert.ToString(Convert.ToString(ds.Tables[1].Rows[0]["flag"])).Equals("Non-Priority", StringComparison.OrdinalIgnoreCase))
                        sampleSizeBodyTable.Append("<td style=\"background-color: rgb(242,242,242);font-weight:bold;text-align:center;width:55.15%;\"><span class=\"span_samplesize\">NA</span></td>");
                    else if (!string.IsNullOrEmpty(ShopperSampleSize) && ShopperSampleSize == "0")
                        sampleSizeBodyTable.Append("<td style=\"background-color: rgb(242,242,242);font-weight:bold;text-align:center;width:55.15%;\"><span class=\"span_samplesize\">0</span></td>");
                    else
                        sampleSizeBodyTable.Append("<td style=\"background-color: rgb(242,242,242);font-weight:bold;text-align:center;width:55.15%;\"><span class=\"span_samplesize\">" + Convert.ToDouble(ShopperSampleSize).ToString("#,#", CultureInfo.InvariantCulture) + "</span></td>");
                    sampleSizeBodyTable.Append("</tr>");
                    //
                    //add second left table header row
                    sampleSizeBodyTable.Append("<tr>");

                    sampleSizeBodyTable.Append("<td style=\"background-color: rgb(242,242,242);font-weight:bold;width:32.95%;\"><span class=\"span_samplesize\" style=\"float:left;\">PRODUCT TRIPS</span></td>");
                    if (!string.IsNullOrEmpty(TripsSampleSize) && TripsSampleSize == "0")
                        sampleSizeBodyTable.Append("<td style=\"background-color:rgb(242,242,242);font-weight:bold;text-align:center;width:55.15%;\"><span class=\"span_samplesize\">0</span></td>");
                    else
                        sampleSizeBodyTable.Append("<td style=\"background-color:rgb(242,242,242);font-weight:bold;text-align:center;width:55.15%;\"><span class=\"span_samplesize\">" + Convert.ToDouble(TripsSampleSize).ToString("#,#", CultureInfo.InvariantCulture) + "</span></td>");
                    sampleSizeBodyTable.Append("</tr>");

                    sampleSizeBodyTable.Append("</table>");
                    //end Sample Size header----------------------->

                    //Plot Left Table Header
                    leftheader.Append("<table id=\"leftbgmtableheader\" style=\"\">");
                    //add first left table header row
                    leftheader.Append("<tr>");

                    leftheader.Append("<td colspan=\"2\"></td>");

                    leftheader.Append("</tr>");

                    leftheader.Append("</table>");
                    //end left header

                    //Plot Right Table Header
                    rightheader.Append("<table id=\"rightbgmtabletheader\" style=\" height:100%;\">");
                    //add first Right table header row
                    rightheader.Append("<tr>");

                    rightheader.Append("<td style=\"border-bottom:1px solid #686868;position:relative;\"><span class=\"span_rightheader\">" + _previoustimePeriod.ToUpper() + "</span></td>");
                    rightheader.Append("<td style=\"border-bottom:1px solid #686868\"><span class=\"span_rightheader\">" + _timeType.ToUpper() + "</span></td>");

                    rightheader.Append("<td style=\"border-bottom:1px solid #686868\"><span class=\"span_rightheader\">CHANGE VS. YAGO</span></td>");
                    rightheader.Append("<td style=\"border-bottom:1px solid #686868\"><span class=\"span_rightheader\">CHANGE BASIS</span></td>");
                    rightheader.Append("<td style=\"border-bottom:1px solid #686868\"><span class=\"span_rightheader\">IMPACT TO SALES</span></td>");

                    rightheader.Append("</tr>");

                    rightheader.Append("</table>");

                    //end right header

                    //Plot body

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        //plot MARKET STATS
                        var query = (from row in ds.Tables[1].AsEnumerable()
                                     where
                                         Convert.ToString(row.Field<object>("Product")).Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)
                                         && Convert.ToString(row.Field<object>("Metric")).Equals("MARKET STATS", StringComparison.OrdinalIgnoreCase)
                                     select row).Distinct().ToList();
                        tbl = query.CopyToDataTable();

                        if (tbl != null && tbl.Rows.Count > 0)
                        {
                            if (int.Parse(Convert.ToString(tbl.Rows[0]["IsBeverage"])) == 0)
                            {
                                NonBeverageItem = true;
                            }
                            leftbody.Append("<table id=\"leftbgmtablebody\" style=\"\">");
                            rightbody.Append("<table id=\"rightbgmtablebody\" style=\"\">");

                            for (int i = 0; i < tbl.Rows.Count; i++)
                            {
                                //Plot Left Table body
                                MetricItem = Convert.ToString(tbl.Rows[i]["MetricItem"]);
                                leftbody.Append("<tr>");
                                if (i == 0)
                                    leftbody.Append("<td style=\"width:26.7%;color:white;background-color: rgb(230,30,42);font-weight: bold;text-align: center;font-size:12px;\" rowspan=\"" + tbl.Rows.Count + "\"><span class=\"span_leftbody\">MARKET</br> STATS</span></td>");

                                leftbody.Append("<td style=\"padding-left:" + AddMetricItemPadding_Left(Convert.ToString(tbl.Rows[i]["MetricItem"])) + ";\"><span class=\"span_leftbody\" style=\"float:left;text-transform:uppercase;\">" + AddMetricItem(Convert.ToString(tbl.Rows[i]["MetricItem"])).ToLower() + "</span></td>");
                                leftbody.Append("</tr>");
                                //End Left Table body

                                //Plot Right Table body
                                ChangeBasis = GetChangeBasis(Convert.ToInt32(tbl.Rows[i]["rowNum"]));
                                ImpactToSales = GetImpactToSalesValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Impact to Sales"]));

                                rightbody.Append("<tr>");
                                rightbody.Append("<td style=\"background-color:" + ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SamplePY"])) + "; color:" + ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), true, Convert.ToInt32(tbl.Rows[i]["rowNum"])) + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Previous Year"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]), Convert.ToString(tbl.Rows[i]["Previous Year Sample"])) + "</span></td>");
                                rightbody.Append("<td style=\"background-color:" + ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SampleCY"])) + "; color:" + ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), false, Convert.ToInt32(tbl.Rows[i]["rowNum"])) + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Current Year"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]), Convert.ToString(tbl.Rows[i]["Previous Year Sample"])) + "</span></td>");

                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                    rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">-</span></td>");
                                else
                                    rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + AddPlusSymble(Convert.ToString(tbl.Rows[i]["Change vs. YAGO"])) + Convert.ToString(tbl.Rows[i]["Change vs. YAGO"]) + (ChangeBasis.Equals("Percentage", StringComparison.OrdinalIgnoreCase) ? "%" : string.Empty) + "</span></td>");

                                rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + ChangeBasis.ToLower() + "</span></td>");

                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                    rightbody.Append("<td style=\"background-color:" + ImpactToSales_BackgroundColor + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">-</span></td>");
                                else
                                    rightbody.Append("<td style=\"background-color:" + ImpactToSales_BackgroundColor + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + ImpactToSales.ToUpper() + "</span></td>");

                                rightbody.Append("</tr>");
                                //End Right Table body
                            }
                            leftbody.Append("<tr><td style=\"/*background-color:white;*/\" colspan=\"2\"</tr>");
                            rightbody.Append("<tr><td style=\"/*background-color:white;*/\" colspan=\"5\"</tr>");
                        }

                        //plot RETAILER STATS
                        var query2 = (from row in ds.Tables[1].AsEnumerable()
                                      where
                                          Convert.ToString(row.Field<object>("Product")).Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(row.Field<object>("Metric")).Equals("RETAILER STATS", StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        tbl = query2.CopyToDataTable();
                        if (tbl != null && tbl.Rows.Count > 0)
                        {
                            for (int i = 0; i < tbl.Rows.Count; i++)
                            {
                                //Plot Left Table body
                                MetricItem = Convert.ToString(tbl.Rows[i]["MetricItem"]);
                                leftbody.Append("<tr>");
                                if (i == 0)
                                    leftbody.Append("<td style=\"width:26.7%;background-color: rgb(113,113,113);;color:white;font-weight: bold;text-align: center;font-size:12px;\" rowspan=\"" + tbl.Rows.Count + "\"><span class=\"span_leftbody\">RETAILER<br/> STATS</span></td>");

                                leftbody.Append("<td style=\"padding-left:" + AddMetricItemPadding_Left(Convert.ToString(tbl.Rows[i]["MetricItem"])) + ";\"><span class=\"span_leftbody\" style=\"float:left;text-transform:uppercase;\">" + AddMetricItem(Convert.ToString(tbl.Rows[i]["MetricItem"])).ToLower() + "</span></td>");
                                leftbody.Append("</tr>");
                                //End Left Table body

                                //Plot Right Table body
                                ChangeBasis = GetChangeBasis(Convert.ToInt32(tbl.Rows[i]["rowNum"]));
                                ImpactToSales = GetImpactToSalesValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Impact to Sales"]));

                                rightbody.Append("<tr>");
                                rightbody.Append("<td style=\"background-color:" + ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SamplePY"])) + "; color:" + ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), true, Convert.ToInt32(tbl.Rows[i]["rowNum"])) + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Previous Year"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]),Convert.ToString(tbl.Rows[i]["Previous Year Sample"])) + "</span></td>");
                                rightbody.Append("<td style=\"background-color:" + ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SampleCY"])) + "; color:" + ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), false, Convert.ToInt32(tbl.Rows[i]["rowNum"])) + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Current Year"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]), Convert.ToString(tbl.Rows[i]["Previous Year Sample"])) + "</span></td>");

                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                    rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">-</span></td>");
                                else
                                    rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + AddPlusSymble(Convert.ToString(tbl.Rows[i]["Change vs. YAGO"])) + Convert.ToString(tbl.Rows[i]["Change vs. YAGO"]) + (ChangeBasis.Equals("Percentage", StringComparison.OrdinalIgnoreCase) ? "%" : string.Empty).ToLower() + "</span></td>");

                                rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + ChangeBasis.ToLower() + "</span></td>");

                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                    rightbody.Append("<td style=\"background-color:" + ImpactToSales_BackgroundColor + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">-</span></td>");
                                else
                                    rightbody.Append("<td style=\"background-color:" + ImpactToSales_BackgroundColor + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + ImpactToSales.ToUpper() + "</span></td>");

                                rightbody.Append("</tr>");
                                //End Right Table body
                            }

                            leftbody.Append("<tr><td style=\"/*background-color:white;*/\" colspan=\"2\"</tr>");
                            rightbody.Append("<tr><td style=\"/*background-color:white;*/\" colspan=\"5\"</tr>");
                        }

                        //plot PRODUCT STATS
                        var query3 = (from row in ds.Tables[1].AsEnumerable()
                                      where
                                          Convert.ToString(row.Field<object>("Product")).Equals(selectedProduct, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(row.Field<object>("Metric")).Equals("PRODUCT STATS", StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        tbl = query3.CopyToDataTable();
                        if (tbl != null && tbl.Rows.Count > 0)
                        {
                            for (int i = 0; i < tbl.Rows.Count; i++)
                            {
                                //Plot Left Table body
                                MetricItem = Convert.ToString(tbl.Rows[i]["MetricItem"]);
                                leftbody.Append("<tr>");
                                if (i == 0)
                                    leftbody.Append("<td style=\"width:26.7%;background-color: rgb(51,51,51);color:white;font-weight: bold;text-align: center;font-size:12px;\" rowspan=\"" + tbl.Rows.Count + "\"><span class=\"span_leftbody\">PRODUCT<br/> STATS</span></td>");

                                leftbody.Append("<td style=\"padding-left:" + AddMetricItemPadding_Left(Convert.ToString(tbl.Rows[i]["MetricItem"])) + ";\"><span class=\"span_leftbody\" style=\"float:left;text-transform:uppercase;\">" + AddMetricItem(Convert.ToString(tbl.Rows[i]["MetricItem"])).ToLower() + "</span></td>");
                                leftbody.Append("</tr>");
                                //End Left Table body

                                //Plot Right Table body
                                ChangeBasis = GetChangeBasis(Convert.ToInt32(tbl.Rows[i]["rowNum"]));
                                ImpactToSales = GetImpactToSalesValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Impact to Sales"]));

                                rightbody.Append("<tr>");
                                rightbody.Append("<td style=\"background-color:" + ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SamplePY"])) + "; color:" + ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), true, Convert.ToInt32(tbl.Rows[i]["rowNum"])) + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Previous Year"]), Convert.ToString(tbl.Rows[i]["SamplePY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]), Convert.ToString(tbl.Rows[i]["Previous Year Sample"])) + "</span></td>");
                                rightbody.Append("<td style=\"background-color:" + ApplyMediumSampleSizeBackgroundColor(Convert.ToString(tbl.Rows[i]["SampleCY"])) + "; color:" + ApplySignificanceColor(Convert.ToString(tbl.Rows[i]["Significance"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), false, Convert.ToInt32(tbl.Rows[i]["rowNum"])) + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + GetTimePeriodValue(Convert.ToInt32(tbl.Rows[i]["rowNum"]), Convert.ToString(tbl.Rows[i]["Current Year"]), Convert.ToString(tbl.Rows[i]["SampleCY"]), Convert.ToString(tbl.Rows[i]["flag"]), Convert.ToString(tbl.Rows[i]["MetricItem"]), Convert.ToString(tbl.Rows[i]["Previous Year Sample"])) + "</span></td>");

                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                    rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">-</span></td>");
                                else
                                    rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + AddPlusSymble(Convert.ToString(tbl.Rows[i]["Change vs. YAGO"])) + Convert.ToString(tbl.Rows[i]["Change vs. YAGO"]) + (ChangeBasis.Equals("Percentage", StringComparison.OrdinalIgnoreCase) ? "%" : string.Empty) + "</span></td>");

                                rightbody.Append("<td><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + ChangeBasis.ToLower() + "</span></td>");

                                if (IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SamplePY"])) || IsLowSampleSize(Convert.ToString(tbl.Rows[i]["SampleCY"])))
                                    rightbody.Append("<td style=\"background-color:" + ImpactToSales_BackgroundColor + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">-</span></td>");
                                else
                                    rightbody.Append("<td style=\"background-color:" + ImpactToSales_BackgroundColor + ";\"><span style=\"font-weight:" + ChageBasis_FontWeight + ";\" class=\"span_righttbody\">" + ImpactToSales.ToUpper() + "</span></td>");

                                rightbody.Append("</tr>");
                                //End Right Table body
                            }
                            //end left body
                        }
                        leftbody.Append("</table>");
                        rightbody.Append("</table>");
                    }
                }
                else
                {
                    ishopParams.IsNoDataAvailable = true;
                }
                ishopParams.SampleSizeHeaderTable = sampleSizeHeaderTable.ToString();
                ishopParams.SampleSizeBodyTable = sampleSizeBodyTable.ToString();
                ishopParams.LeftHeader = leftheader.ToString();
                ishopParams.LeftBody = leftbody.ToString();
                ishopParams.RightHeader = rightheader.ToString();
                ishopParams.RightBody = rightbody.ToString();

                bgmParams.Data_Set = ds;

                HttpContext.Current.Session["BGMData"] = bgmParams;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
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

        private string ApplyMediumSampleSizeBackgroundColor(string samplesize)
        {
            string SampleSizeBackgroundColor = "#ededee";//"lightgray";
            if (!string.IsNullOrEmpty(samplesize))
            {
                //atul new
                if (Convert.ToDouble(samplesize) >= GlobalVariables.MinSampleSize && Convert.ToDouble(samplesize) < GlobalVariables.MaxSampleSize)
                {
                    SampleSizeBackgroundColor = "gray";
                }
            }
            return SampleSizeBackgroundColor;
        }
        public string AddMetricItemPadding_Left(string _metricitem)
        {
            string padding_left = "5px";
            switch (_metricitem.ToLower())
            {
                case "number of retailer shoppers":
                case "number of retailer trips":
                case "number of retailer trip":
                case "share of shopper`s trips to any retailer":
                case "share of shopper`s trips to channel":
                case "share of trips to any retailer":
                case "share of trips to channel":
                case "product trips per shopper":
                case "% of shoppers, monthly+ product buyers":
                case "avg. basket size, any item":
                    {
                        padding_left = "12px";
                        break;
                    }
            }
            return padding_left;
        }

        public string AddPlusSymble(string value)
        {
            string _symble = string.Empty;
            if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) > 0)
                _symble = "+";
            return _symble;
        }
        private string ApplySignificanceColor(string value, string samplesize, bool isBenchmark, int Rownumber)
        {
            string color = "rgb(0,0,0)";
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
                            color = "rgb(0,0,255)";
                            break;
                        }
                    default:
                        {
                            color = "rgb(0,0,0)";
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
                        color = "rgb(0,128,0)";
                    }
                    else if (Convert.ToDouble(value) < accuratestatvaluenega)
                    {
                        color = "rgb(255,0,0)";
                    }
                    else if (Convert.ToDouble(value) <= accuratestatvalueposi && Convert.ToDouble(value) >= accuratestatvaluenega)
                    {
                        color = "rgb(0,0,0)";
                    }
                }
            }
            return color;
        }

        public string GetTimePeriodValue(int Rownumber, string value, string samplesize, string PriorityType, string Metric, string sampleSizePy)
        {
            string TimePeriodValue = value;
            //if ((NonBeverageItem && Metric.Equals("% of Shoppers, Monthly+ Product Buyers", StringComparison.OrdinalIgnoreCase))
            //    || (selectedProduct.Equals("Total Beverage Trips", StringComparison.OrdinalIgnoreCase)
            //    && Metric.Equals("% of Shoppers, Monthly+ Product Buyers", StringComparison.OrdinalIgnoreCase)))
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
                TimePeriodValue = "(low sample)";
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
            ImpactToSales_BackgroundColor = "#ededee";//"lightgrey";
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
                        ImpactToSales_BackgroundColor = "rgb(201, 201, 201)";//"rgb(166,166,166)";
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
        public string CleanClass(string _class)
        {
            _class = System.Text.RegularExpressions.Regex.Replace(_class, @"[/\s,.`/@#$%;&*~()+/]", "");
            return _class;
        }
        public string CommaSeparatedValues(double value, string metricName, double valueListSignificance)
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
                        decimaval = value.ToString("#,#", CultureInfo.InvariantCulture);//Convert.ToString(String.Format("{0:#,###,###}", value));//{0:#,###0}
                        if (decimaval == "")
                        {
                            decimaval = "0";
                        }
                        break;

                    case "%":
                        significanceColor = GetCellColor(valueListSignificance);
                        decimaval = value.ToString("0.0") + "%";
                        break;
                    case "d":
                        decimaval = value.ToString("0.0");
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
            //if (BenchmarkOrComparison.Equals(Benchmark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
            if (Benchmark == BenchmarkOrComparison && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
            {
                color = "color:blue;";
                //cellfontstyle = 28;

            }
            else if (significancevalue > accuratestatvalueposi)
            {
                color = "color:#20B250;";


            }
            else if (significancevalue < accuratestatvaluenega)
            {
                color = "color:red;";

            }
            else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && significancevalue >= accuratestatvaluenega)
            {
                color = "color:black;";

            }
            return color;

        }
        private string GetClassNames(int colIndex, List<string> col1)
        {
            if (colIndex >= 0 && colIndex < 5)
            {
                return CleanClass(Convert.ToString(objcomFunc.Get_ShortNames(col1[0]).Replace("Grocery", "SupermarketGrocery")));
            }
            else if (colIndex >= 5 && colIndex < 10)
            {
                return CleanClass(Convert.ToString(objcomFunc.Get_ShortNames(col1[1]).Replace("Grocery", "SupermarketGrocery")));
            }
            else if (colIndex >= 10 && colIndex < 15)
            {
                return CleanClass(Convert.ToString(objcomFunc.Get_ShortNames(col1[2]).Replace("Grocery", "SupermarketGrocery")));
            }
            else if (colIndex >= 15 && colIndex < 20)
            {
                return CleanClass(Convert.ToString(objcomFunc.Get_ShortNames(col1[3]).Replace("Grocery", "SupermarketGrocery")));
            }
            else if (colIndex >= 20 && colIndex < 25)
            {
                return CleanClass(Convert.ToString(objcomFunc.Get_ShortNames(col1[4]).Replace("Grocery", "SupermarketGrocery")));
            }
            else
                return CleanClass(Convert.ToString(objcomFunc.Get_ShortNames(col1[0]))); ;
        }

    }
}