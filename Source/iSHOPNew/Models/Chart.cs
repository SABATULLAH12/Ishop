using iSHOPNew.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

namespace iSHOPNew.Models
{
    public class Chart   
    {
        string View_Type = string.Empty;
        bool IsApplicable = true;
        int TabIndexId = 0;
        Hashtable LoyaltyRetailerList = new Hashtable();
        Dictionary<string, double> samplesizelist = null;
        List<Dictionary<string, double>> samplesizeArray = null;
        CommonFunctions commonFunctions = new CommonFunctions();           
        List<string> selectedMetrics = new List<string>();
        List<string> ChartXValues = new List<string>();
        List<string> BCFullNames = new List<string>();
        ProfilerChartParams profilerchartparams = null;
        iSHOPParams param = new iSHOPParams();

        public string BenchMark = string.Empty;
        public string Compare1 = string.Empty;
        public string Compare2 = string.Empty;
        public string Compare3 = string.Empty;
        public string Compare4 = string.Empty;
        public string Compare5 = string.Empty;
        public string ShopperSegment = string.Empty;
        public string ShopperFrequency = string.Empty;
        public string CheckString = string.Empty;
        public string TimePeriod = string.Empty;
        public Dictionary<string, string> HeaderTabs = new Dictionary<string, string>();
        public Dictionary<string, string> FilterTabs = new Dictionary<string, string>();
        public Dictionary<string, string> TableMappingList = new Dictionary<string, string>();
        public Dictionary<string, string> sampleSize = new Dictionary<string, string>();
        public Dictionary<string, string> exportfiles = new Dictionary<string, string>();
        private string currentTab = string.Empty;
        public string average = string.Empty;

        public iSHOPParams getvalue = new iSHOPParams();
        public string[] Retailerlist = null;
        public List<string> complist = new List<string>();
        public Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        public Dictionary<string, string> selectedsheets = new Dictionary<string, string>();
        public List<string> mergeCell = new List<string>();
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        private int colmaxwidth = 0;
        int cellfontstyle = 8;
        //Nishanth
        int cellfontstylegrey = 19;
        int samplecellstyle = 4;
        string activetab = string.Empty;
        DataSet demo = new DataSet();
        DataSet general = new DataSet();
        DataSet postshop = new DataSet();
        DataSet bdetail = new DataSet();
        public string userRole { get; set; }
        public string frequency = "";
        CommonFunctions _commonfunctions = new CommonFunctions();
        ProfilerChartParams profilerParams = new ProfilerChartParams();

        string HeaderTable = string.Empty;
        string BodyTable = string.Empty;


        int table_width = 900;
        int table_td_width = 110;

        bool CheckBeverageTripNA = false;
        Hashtable CheckBeverageTripNAhTbl = new Hashtable();
        string checkBevTotalTrips = "totaltrip||totaltrip";
        int isBevTotalTrips = 0;
        //

        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        bool isBeverageDetail = false;
        bool isLiquidFlavorEnhancer = false;
        bool LoyaltyPyramid = false;
        bool RetailerNetCheck = false;

        string LoyaltyPyramidmetric = string.Empty;

        string BenchmarkOrComparison;
        string SelectedStatTest = string.Empty;

        bool LoyaltyPyramidForRetailers = false;
        bool StoreImageryCheck = false;
        bool CheckRetailerorChannel = false;
        string NA_Text = string.Empty;
        List<string> BenchmarkorComparisionList;

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
        public DataSet FilterDataTable(DataSet ds, List<string> metriclist, string spColName)
        {
            DataSet dsRet = new DataSet();
            //Nagaraju 25-06-14
            string timeperiod = string.Empty;
            if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("TREND"))
            {
                if (!string.IsNullOrEmpty(profilerchartparams.Benchmark))
                {
                    string[] timeperiodlist = profilerchartparams.Benchmark.Split('|');
                    if (timeperiodlist != null && timeperiodlist.Count() > 0 && timeperiodlist[0].ToLower() != "total")
                    {
                        timeperiod = " " + timeperiodlist[0];
                    }
                }
            }

            foreach (DataTable inputTable in ds.Tables)
            {
                DataTable outputTable = inputTable.Clone();
                foreach (string obj in BCFullNames)
                {
                    var query = from r in inputTable.AsEnumerable()
                                where r.Field<string>("Objective").ToLower() == obj.ToLower()
                                select r;
                    List<DataRow> Rows = query.Distinct().ToList();

                    foreach (DataRow row in Rows)
                    {
                        if (!Convert.ToString(row[spColName]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                            && !Convert.ToString(row["MetricItem"]).Equals("Number of Trips", StringComparison.OrdinalIgnoreCase)
                            && !Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)) //&&
                        //!Convert.ToString(row["MetricItem"]).ToUpper().Contains("SAMPLESIZE"))
                        {
                            if (Convert.ToString(row["Objective"]).Equals(BCFullNames[0], StringComparison.OrdinalIgnoreCase)
                                && Convert.ToString(profilerchartparams.SelectedStatTest).Equals("CUSTOM BASE", StringComparison.OrdinalIgnoreCase))
                            {
                                row["Significance"] = 1000;
                            }


                            if (profilerchartparams.View.Contains("Trips"))
                            {
                                if (inputTable.Columns.Contains("SampleSize") && DBNull.Value.Equals(row["SampleSize"]))
                                {
                                    row["SampleSize"] = 0;
                                }
                                if (inputTable.Columns.Contains("SampleSize") && System.DBNull.Value != row["Volume"] && Convert.ToDouble(row["Volume"]) > 0 && Convert.ToDouble(row["SampleSize"]) >= 30)
                                {
                                    row["Volume"] = Convert.ToDouble(row["Volume"]) / 100;
                                }
                                else
                                {
                                    row["Volume"] = 0; // Change this to dbnull later
                                }
                            }
                            else
                            {
                                if (inputTable.Columns.Contains("SampleSize") && DBNull.Value.Equals(row["SampleSize"]))
                                {
                                    row["SampleSize"] = 0;
                                }
                                if (inputTable.Columns.Contains("SampleSize") && System.DBNull.Value != row["Volume"] && Convert.ToDouble(row["Volume"]) > 0 && Convert.ToDouble(row["SampleSize"]) >= 30)
                                {
                                    row["Volume"] = Convert.ToDouble(row["Volume"]) / 100;
                                }
                                else
                                {
                                    row["Volume"] = 0; // Change this to dbnull later
                                }
                            }

                            if (profilerchartparams.View.IndexOf("PIT") == -1)
                            {
                                //row["Objective"] = (UppercaseFirst(commonFunctions.Get_ShortNames(profilerchartparams.View)) + CommonFunctions.AddTradeAreaNoteforChannel(commonFunctions.Get_ShortNames(profilerchartparams.View), profilerchartparams.ShopperFrequency).ToUpper());
                                row["Objective"] = UppercaseFirst(commonFunctions.Get_ShortNames(Convert.ToString(row["Objective"])));
                            }
                            else
                            {
                                row["Objective"] = (UppercaseFirst(commonFunctions.Get_ShortNames(Convert.ToString(row["Objective"])) + timeperiod) + CommonFunctions.AddTradeAreaNoteforChannel(commonFunctions.Get_ShortNames(Convert.ToString(row["Objective"])), profilerchartparams.ShopperFrequency)).ToUpper();
                            }

                            //row["Metric"] = commonFunctions.Get_ShortNames(Convert.ToString(profilerchartparams.MetricShortName)).ToUpper();                          

                            //row["MetricItem"] = (commonFunctions.Get_ShortNames(Convert.ToString(row["MetricItem"])).Replace("&lt;", "<")).ToUpper();
                            outputTable.ImportRow(row);
                        }
                        //Sample Size
                        else if (Convert.ToString(row[spColName]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                              || Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                             || Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase) ||
                            Convert.ToString(row["MetricItem"]).Equals("Number of Trips", StringComparison.OrdinalIgnoreCase))
                        {
                            if (profilerchartparams.View.IndexOf("PIT") == -1)
                            {
                                //row["Objective"] = (UppercaseFirst(commonFunctions.Get_ShortNames(profilerchartparams.View)) + CommonFunctions.AddTradeAreaNoteforChannel(commonFunctions.Get_ShortNames(commonFunctions.Get_ShortNames(profilerchartparams.View)), profilerchartparams.ShopperFrequency)).ToUpper();
                                row["Objective"] = UppercaseFirst(commonFunctions.Get_ShortNames(Convert.ToString(row["Objective"])));
                            }
                            else
                            {
                                row["Objective"] = (UppercaseFirst(commonFunctions.Get_ShortNames(Convert.ToString(row["Objective"])) + timeperiod) + CommonFunctions.AddTradeAreaNoteforChannel(commonFunctions.Get_ShortNames(commonFunctions.Get_ShortNames(Convert.ToString(row["Objective"]))), profilerchartparams.ShopperFrequency)).ToUpper();
                            }
                            //row["Metric"] = (commonFunctions.Get_ShortNames(Convert.ToString(profilerchartparams.MetricShortName))).ToUpper();                           

                            //row["MetricItem"] = (commonFunctions.Get_ShortNames(Convert.ToString(row["MetricItem"])).Replace("&lt;", "<")).ToUpper();
                            outputTable.ImportRow(row);
                        }
                    }
                }

                dsRet.Tables.Add(outputTable);
            }

            return dsRet;
        }
        public DataSet MakeTopTenTable(DataSet ds)
        {
            DataTable outputTable = ds.Tables[1].Clone();

            var query = (from r in ds.Tables[1].AsEnumerable()
                         orderby (r.Field<double?>("Volume")) descending
                         select r.Field<string>("MetricItem")).Distinct().Take(10);

            List<string> Metric = query.Distinct().ToList();

            foreach (string item in Metric)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (Convert.ToString(dr["MetricItem"]).ToLower().Trim() == item.ToLower().Trim())
                        outputTable.ImportRow(dr);
                }
                outputTable.AcceptChanges();
            }

            ds.Tables.Remove("table1");
            ds.Tables.Add(outputTable);
            return ds;
        }
        private void CheckSessionData()
        {
            if (HttpContext.Current.Session["StatSessionPosi"] == null || HttpContext.Current.Session["StatSessionNega"] == null
                || HttpContext.Current.Session["PercentStat"] == null || HttpContext.Current.Session[SessionVariables.USERID] == null)
            {
                //HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx");
            }
        }
        public DataSet GetHighChartData()
        {
            PageLoadForHighChart();
            char[] PipeSeparator = new char[] { '|' };
            DataSet ds = null;           
            DataAccess dal = new DataAccess();
            ProfilerChartParams profilerparams = new ProfilerChartParams();

            if (profilerchartparams.Tab_Id_mapping == "true")
                selectedMetrics = Convert.ToString(profilerchartparams.SelectedMetricsIds.Replace("Top 10|", "")).Trim().TrimEnd().Split('|').ToList();
            else
                selectedMetrics = Convert.ToString(profilerchartparams.SelectedMetrics.Replace("Top 10|", "")).Trim().TrimEnd().Split('|').ToList();
            try
            {
                profilerparams.Benchmark = string.Join("||", profilerchartparams.Benchmark.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());               
                profilerparams.Comparisonlist = string.Join("||", profilerchartparams.Comparisonlist.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());               
                profilerparams.TimePeriod = profilerchartparams.TimePeriod;
                profilerparams.Filters = profilerchartparams.Filters;
                profilerparams.ShopperFrequency = profilerchartparams.ShopperFrequency;
                profilerparams.Metric = profilerchartparams.Metric;
                profilerparams.ChartType = profilerchartparams.ChartType;
                profilerparams.ActiveTab = profilerchartparams.ActiveTab;
                profilerparams.FilterShortNames = profilerchartparams.FilterShortNames;
                profilerparams.ShopperSegment = string.Join("||", profilerchartparams.ShopperSegment.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
             
                profilerparams.FrequencyTitle = profilerchartparams.FrequencyTitle;
                profilerparams.ShortTimePeriod = profilerchartparams.ShortTimePeriod;
                profilerparams.ShopperFrequencyShortName = profilerchartparams.ShopperFrequencyShortName;
                profilerparams.MetricShortName = profilerchartparams.MetricShortName;
                profilerparams.ModuleBlock = profilerchartparams.ModuleBlock;
                profilerparams.SelectedStatTest = profilerchartparams.SelectedStatTest;
                profilerparams.Filtertypeid_UniqueId = profilerchartparams.Filtertypeid_UniqueId;

                profilerparams.Comparison_UniqueIds = profilerchartparams.Comparison_UniqueIds;
                profilerparams.SingleSelection = profilerchartparams.SingleSelection;

                profilerparams.TimePeriodFrom_UniqueId = profilerchartparams.TimePeriodFrom_UniqueId;
                profilerparams.TimePeriodTo_UniqueId = profilerchartparams.TimePeriodTo_UniqueId;
                profilerparams.beverageSelectionType_UniqueId = profilerchartparams.beverageSelectionType_UniqueId;
                profilerparams.View = profilerchartparams.View;

                profilerparams.TrendType = profilerchartparams.TrendType;

                profilerparams.StatPositive = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
                profilerparams.StatNegative = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
                profilerparams.StatTesting = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);

                profilerparams.Sigtype_UniqueId = profilerchartparams.Sigtype_UniqueId.ToMyString();
                profilerparams.CustomBase_UniqueId = profilerchartparams.CustomBase_UniqueId.ToMyString();
                profilerparams.CompetitorFrequency_UniqueId = profilerchartparams.CompetitorFrequency_UniqueId.ToMyString();
                profilerparams.CompetitorFrequency_Name = profilerchartparams.CompetitorFrequency_Name.ToMyString();

                profilerparams.CompetitorRetailer_UniqueId = profilerchartparams.CompetitorRetailer_UniqueId.ToMyString();
                profilerparams.CompetitorRetailer_Name= profilerchartparams.CompetitorRetailer_Name.ToMyString();
                object[] paramvalues = null;

                if (profilerchartparams.Tab_Id_mapping == "true")
                {
                    if (Convert.ToString(profilerchartparams.ModuleBlock).IndexOf("TREND") == -1)
                    {
                        if (profilerparams.SelectedStatTest == "1")
                        {
                            List<string> comp = (profilerparams.Benchmark + "|" + profilerparams.Comparisonlist).Split('|').ToList();
                            comp = (from r in comp where r != profilerchartparams.CustomBase_UniqueId select r).Distinct().ToList();
                            profilerparams.Comparisonlist = String.Join("|", comp);
                            profilerparams.Benchmark = profilerchartparams.CustomBase_UniqueId;

                        }
                    }
                    if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("PIT"))
                    {
                     if (profilerparams.ActiveTab.ToLower().IndexOf("retailer") > -1)
                            paramvalues = new object[] { profilerparams.Filtertypeid_UniqueId, null, profilerparams.SingleSelection, profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CompetitorFrequency_UniqueId.ToMyString(), profilerparams.CompetitorRetailer_UniqueId.ToMyString() };
                        else
                            paramvalues = new object[] { profilerparams.Filtertypeid_UniqueId, null, profilerparams.SingleSelection, profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.Sigtype_UniqueId.ToMyString()};
                    }
                    else if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("TREND"))
                    {
                        if (profilerparams.TrendType == "1")
                        {
                            if (profilerparams.ActiveTab.ToLower().IndexOf("retailer") > -1)
                                paramvalues = new object[] { "", profilerparams.beverageSelectionType_UniqueId.ToMyString(), profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString(), profilerparams.TimePeriodFrom_UniqueId.ToMyString(), profilerparams.TimePeriodTo_UniqueId.ToMyString(), profilerparams.Filters.ToMyString(), profilerparams.ShopperFrequency.ToMyString(), profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<").ToMyString() : profilerparams.Metric.Replace("&lt;", "<").ToMyString(), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CustomBase_UniqueId, profilerparams.CompetitorFrequency_UniqueId.ToMyString(), profilerparams.CompetitorRetailer_UniqueId.ToMyString() };
                            else
                                paramvalues = new object[] { "", profilerparams.beverageSelectionType_UniqueId.ToMyString(), profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString(), profilerparams.TimePeriodFrom_UniqueId.ToMyString(), profilerparams.TimePeriodTo_UniqueId.ToMyString(), profilerparams.Filters.ToMyString(), profilerparams.ShopperFrequency.ToMyString(), profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<").ToMyString() : profilerparams.Metric.Replace("&lt;", "<").ToMyString(), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CustomBase_UniqueId };

                        }
                        else if (profilerparams.TrendType == "2")
                        {
                            if (profilerparams.ActiveTab.ToLower().IndexOf("retailer") > -1)
                                paramvalues = new object[] { "", profilerparams.beverageSelectionType_UniqueId.ToMyString(), profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString() + "|" + profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString(), profilerparams.TimePeriodFrom_UniqueId.ToMyString(), profilerparams.TimePeriodTo_UniqueId.ToMyString(), profilerparams.Filters.ToMyString(), profilerparams.ShopperFrequency.ToMyString(), profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<").ToMyString() : profilerparams.Metric.Replace("&lt;", "<").ToMyString(), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CustomBase_UniqueId, profilerparams.CompetitorFrequency_UniqueId.ToMyString(), profilerparams.CompetitorRetailer_UniqueId.ToMyString() };
                            else
                                paramvalues = new object[] { "", profilerparams.beverageSelectionType_UniqueId.ToMyString(), profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString() + "|" + profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString(), profilerparams.TimePeriodFrom_UniqueId.ToMyString(), profilerparams.TimePeriodTo_UniqueId.ToMyString(), profilerparams.Filters.ToMyString(), profilerparams.ShopperFrequency.ToMyString(), profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<").ToMyString() : profilerparams.Metric.Replace("&lt;", "<").ToMyString(), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CustomBase_UniqueId };
                        }
                    }
                    else
                    {
                        if (profilerparams.ActiveTab.ToLower().IndexOf("retailer") > -1)
                            paramvalues = new object[] { profilerparams.Filtertypeid_UniqueId, null, profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CompetitorFrequency_UniqueId.ToMyString(), profilerparams.CompetitorRetailer_UniqueId.ToMyString() };
                        else
                            paramvalues = new object[] { profilerparams.Filtertypeid_UniqueId, null, profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.Sigtype_UniqueId.ToMyString()};
                    }
                    if (profilerchartparams.IsSelectionChange || HttpContext.Current.Session["HighChartDataSet"] == null)
                    {
                        if (Convert.ToString(profilerchartparams.ActiveTab).ToLower().IndexOf("ecom") > -1)
                        {
                            List<object> dbparam = paramvalues.ToList();
                            dbparam.Add(profilerchartparams.Add_ShopperFrequency_UniqueId.ToMyString());
                            paramvalues = dbparam.ToArray();
                        }
                        ds = dal.GetData_WithIdMapping(paramvalues, profilerparams.ActiveTab);
                    }
                    else
                        ds = HttpContext.Current.Session["HighChartDataSet"] as DataSet;

                    DataColumn[] stringColumns = ds.Tables[1].Columns.Cast<DataColumn>()
                                                .Where(c => c.DataType == typeof(string))
                                                .ToArray();

                    foreach (DataRow row in ds.Tables[1].Rows)
                        foreach (DataColumn col in stringColumns)
                            row.SetField<string>(col, row.Field<string>(col).Trim());
                }
                else
                {
                    if (Convert.ToString(profilerchartparams.ModuleBlock).IndexOf("TREND") == -1)
                    {
                        if (profilerparams.SelectedStatTest.Trim().ToLower() == "custom base")
                        {
                            List<string> comp = new List<string>();
                            comp.Add(profilerchartparams.Benchmark);
                            comp.AddRange(profilerchartparams.Comparison_DBNames);

                            comp = (from r in comp where r != profilerchartparams.CustomBase_DBName select r).Distinct().ToList();
                            profilerparams.Comparisonlist = String.Join("|", comp);
                            profilerparams.Benchmark = profilerchartparams.CustomBase_DBName;
                        }
                    }
                    if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("PIT"))
                        paramvalues = new object[] { profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.ShopperSegment.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.ShopperFrequency, profilerparams.Filters, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.SelectedStatTest };
                    else if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("TREND"))
                        paramvalues = new object[] { profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.ShopperSegment, profilerparams.ShopperFrequency, profilerparams.Filters, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.SelectedStatTest };
                    else
                        paramvalues = new object[] { profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.SelectedStatTest };

                    if (profilerchartparams.IsSelectionChange || HttpContext.Current.Session["HighChartDataSet"] == null)
                        ds = dal.GetData(paramvalues, profilerparams.ActiveTab);
                    else
                        ds = HttpContext.Current.Session["HighChartDataSet"] as DataSet;
                }
                if (profilerchartparams.SelectedMetrics.ToUpper().Split('|').ToList()[0].Trim() == "TOP 10")
                {
                    List<string> str = (from r in ds.Tables[1].AsEnumerable() orderby r.Field<string>("MetricItem") select r.Field<string>("MetricItem")).Distinct().ToList();
                    profilerchartparams.SelectedMetrics = string.Join("|", str.ToArray());
                    profilerchartparams.Top_10 = true;
                }
                    HttpContext.Current.Session["HighChartDataSet"] = ds.Copy();

                List<string> _Benchitems = new List<string>();
                _Benchitems = profilerchartparams.Benchmark.Split('|').ToList();

                List<string> lstComplist = new List<string>();
                lstComplist = profilerchartparams.Comparisonlist.Split('|').ToList();
                ds = FormatDataSet(ds);
                GetData(ds.Copy());             

                if (((Convert.ToString(profilerchartparams.ChartType).Equals("Clustered Column", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10) ||
                       (Convert.ToString(profilerchartparams.ChartType).Equals("Clustered Bar", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10) ||
                       (Convert.ToString(profilerchartparams.ChartType).Equals("Bar with Change", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10) ||
                       (Convert.ToString(profilerchartparams.ChartType).Equals("Line", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10)) && profilerchartparams.Top_10)//&& profilerchartparams.SelectedMetrics.IndexOf("Top 10") > -1)
                {
                    ds = MakeTopTenTable(ds);
                }                
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }
        private DataSet FormatDataSet(DataSet _ds)
        {
            CommonFunctions commonfunctions = new CommonFunctions();
            DataSet dsRet = new DataSet();
            int tblnumber = 0;
            foreach (System.Data.DataTable tbl in _ds.Tables)
            {
                DataTable outputTable = tbl.Clone();
                foreach (DataRow row in tbl.Rows)
                {
                    row["Objective"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["Objective"]).Trim())).ToUpper();
                    row["Metric"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["Metric"]).Trim())).ToUpper();
                    //row["MetricItem"] = (commonfunctions.Get_ShortNames(Convert.ToString(row["MetricItem"]))).Trim().ToUpper().Replace("&LT;", "<").Replace("&GT;", ">").Replace("&APOS;", "`");
                    string metricName = Convert.ToString(row["MetricItem"]);
                    if (tblnumber == 1)
                    {
                        //if (profilerchartparams.SelectedMetrics.ToUpper().Split('|').ToList().Contains(metricName.ToUpper().Trim(), StringComparer.OrdinalIgnoreCase))
                            outputTable.ImportRow(row);
                    }
                    else
                    {
                        outputTable.ImportRow(row);
                    }
                }
                tblnumber += 1;
                dsRet.Tables.Add(outputTable);
            }
            return dsRet;
        }
        private void GetData(DataSet _ds)
        {
            BCFullNames.Clear();
            char[] PipeSeparator = new char[] { '|' };
            DataSet ds = _ds;
            DataAccess dal = new DataAccess();
            selectedMetrics = Convert.ToString(profilerchartparams.SelectedMetrics.Replace("Top 10|", "")).Trim().TrimEnd().Split('|').ToList();
            try
            {
                ProfilerChartParams profilerparams = new ProfilerChartParams();
                profilerparams.Benchmark = profilerchartparams.Benchmark;
                profilerparams.Comparisonlist = profilerchartparams.Comparisonlist;
                profilerparams.TimePeriod = profilerchartparams.TimePeriod;
                profilerparams.Filters = profilerchartparams.Filters;
                profilerparams.ShopperFrequency = profilerchartparams.ShopperFrequency;
                profilerparams.Metric = profilerchartparams.Metric;
                profilerparams.ChartType = profilerchartparams.ChartType;
                profilerparams.ActiveTab = profilerchartparams.ActiveTab;
                profilerparams.FilterShortNames = profilerchartparams.FilterShortNames;
                profilerparams.ShopperSegment = profilerchartparams.ShopperSegment;

                profilerparams.FrequencyTitle = profilerchartparams.FrequencyTitle;
                profilerparams.ShortTimePeriod = profilerchartparams.ShortTimePeriod;
                profilerparams.ShopperFrequencyShortName = profilerchartparams.ShopperFrequencyShortName;
                profilerparams.MetricShortName = profilerchartparams.MetricShortName;
                profilerparams.ModuleBlock = profilerchartparams.ModuleBlock;
                profilerparams.SelectedStatTest = profilerchartparams.SelectedStatTest;
                profilerparams.Comparison_ShortNames = profilerchartparams.Comparison_ShortNames;
                if (HttpContext.Current.Session["StatSessionPosi"] == null || HttpContext.Current.Session["StatSessionNega"] == null)
                {
                    //HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx");
                }
                profilerparams.StatPositive = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
                profilerparams.StatNegative = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
                profilerparams.StatTesting = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);
                object[] paramvalues = null;

                if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("PIT"))
                    paramvalues = new object[] { profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.ShopperSegment.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.ShopperFrequency, profilerparams.Filters, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric };

                else if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("TREND"))
                    paramvalues = new object[] { profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.ShopperSegment, profilerparams.ShopperFrequency, profilerparams.Filters, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric };

                else
                    paramvalues = new object[] { profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric };
              

                if (!string.IsNullOrEmpty(profilerparams.Benchmark) || !string.IsNullOrEmpty(profilerparams.Comparisonlist))
                {
                    if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("TREND"))
                    {                      
                        string timeperiod = string.Empty;
                        if (!string.IsNullOrEmpty(profilerchartparams.Benchmark))
                        {
                            string[] timeperiodlist = profilerchartparams.Benchmark.Split('|');
                            if (timeperiodlist != null && timeperiodlist.Count() > 0 && timeperiodlist[0].ToLower() != "total")
                            {
                                timeperiod = " " + timeperiodlist[0];
                            }
                        }
                        var query2 = from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective"));
                        BCFullNames = query2.Distinct().ToList();
                        var query = from r in ds.Tables[1].AsEnumerable() select UppercaseFirst(r.Field<string>("Objective") + timeperiod);
                        ChartXValues = query.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();                      
                    }

                    else
                    {
                        if (ds != null && ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                        {
                            CommonFunctions commonfunctions = new CommonFunctions();
                            List<string> inputCRlist = ChartXValues = (profilerparams.Benchmark + "|" + profilerparams.Comparisonlist).Split('|').ToList();
                            inputCRlist.RemoveAt(0);
                            ChartXValues = new List<string>();
                            var query = from r in ds.Tables[1].AsEnumerable()
                                        select r.Field<string>("Objective");
                            List<string> tblCRlist = query.Distinct().ToList();
                            inputCRlist = tblCRlist;
                            foreach (string cr in inputCRlist)
                            {
                                if (tblCRlist.Contains(commonfunctions.Get_ShortNames(cr).ToUpper(), StringComparer.CurrentCultureIgnoreCase))
                                {
                                    BCFullNames.Add(UppercaseFirst(commonfunctions.Get_ShortNames(cr)));
                                    if (cr.ToLower() == "total" && profilerchartparams.View.IndexOf("PIT") == -1)
                                    {
                                        ChartXValues.Add(UppercaseFirst(commonFunctions.Get_ShortNames(profilerchartparams.View)) + CommonFunctions.AddTradeAreaNoteforChannel(commonFunctions.Get_ShortNames(commonFunctions.Get_ShortNames(profilerchartparams.View)), profilerchartparams.ShopperFrequency));
                                    }
                                    else
                                    {
                                        ChartXValues.Add(UppercaseFirst(commonFunctions.Get_ShortNames(cr)) + CommonFunctions.AddTradeAreaNoteforChannel(commonFunctions.Get_ShortNames(cr.Trim()), profilerchartparams.ShopperFrequency));
                                    }
                                }
                            }
                        }
                    }
                }
                BCFullNames = BCFullNames.Distinct().ToList();
                if (Convert.ToString(profilerchartparams.ChartType).Equals("stacked bar", StringComparison.OrdinalIgnoreCase))
                {
                    ChartXValues.Reverse();
                }
                if (ds != null && ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                {
                    if (((Convert.ToString(profilerchartparams.ChartType).Equals("Clustered Column", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10) ||
                           (Convert.ToString(profilerchartparams.ChartType).Equals("Clustered Bar", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10) || (Convert.ToString(profilerchartparams.ChartType).Equals("Bar with Change", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10) ||
                           (Convert.ToString(profilerchartparams.ChartType).Equals("Line", StringComparison.OrdinalIgnoreCase) && selectedMetrics.Count >= 10)) && profilerchartparams.Top_10)
                    {
                        ds = MakeTopTenTable(ds);
                    }

                    selectedMetrics = (from row in ds.Tables[1].AsEnumerable()
                                       select Convert.ToString(row.Field<string>("MetricItem"))).Distinct().ToList();

                    ds = FilterDataTable(ds, selectedMetrics, "MetricItem");

                    if (ChartXValues.Contains("Total") && profilerchartparams.View.IndexOf("PIT") == -1)
                    {
                        int index = 0;
                        index = ChartXValues.IndexOf("Total");
                        if (index >= 0)
                        {
                            ChartXValues.RemoveAt(index);
                            ChartXValues.Insert(index,commonFunctions.Get_ShortNames(profilerchartparams.View));
                        }
                    }
                }

                profilerparams.ChartXValues = ChartXValues;
                profilerparams.BCFullNames = BCFullNames;
                profilerparams.ChartDataSet = ds;
                List<double> volumelist = null;
                if (ds != null && ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                {
                    var query = from r in ds.Tables[1].AsEnumerable()
                                where r.Field<double>("Volume") >= 0
                                select r.Field<double>("Volume");
                    volumelist = query.Distinct().ToList();
                }
                if (volumelist != null && volumelist.Count > 0)
                {
                    profilerparams.Metric = commonFunctions.Get_ShortNames(profilerparams.Metric);
                    HttpContext.Current.Session["ProfilerChartData"] = profilerparams;
                }
                else
                {
                    HttpContext.Current.Session["ProfilerChartData"] = null;
                }               

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }     
        #region Add to chart      
        public string AddChartToExport(string ChartType)
        {
            Dictionary<string, ProfilerChartParams> chartlist = HttpContext.Current.Session["ProfilerChartExportList"] as Dictionary<string, ProfilerChartParams>;
            ProfilerChartParams profilerparams = HttpContext.Current.Session["ProfilerChartData"] as ProfilerChartParams;
            ProfilerChartParams active_profilerparams = null;
            if (profilerparams == null)
            {
                return "false";
            }
            if (profilerparams != null)
            {
                active_profilerparams = profilerparams.Clone();
                active_profilerparams.ChartType = ChartType;
            }
            if (chartlist == null)
            {
                chartlist = new Dictionary<string, ProfilerChartParams>();
            }

            //chartlist.Add("Chart_" + chartlist.Count.ToString(), profilerparams);
            chartlist.Add("Chart_" + chartlist.Count.ToString(), active_profilerparams);
            HttpContext.Current.Session["ProfilerChartExportList"] = chartlist;
            return "true";
        }
        #endregion
        #region Show export chart list      
        public string ShowExportChartList()
        {
            string htmlstring = string.Empty;
            List<ProfilerChartParams> profilerparamslist = new List<ProfilerChartParams>();
            Dictionary<string, ProfilerChartParams> chartlist = HttpContext.Current.Session["ProfilerChartExportList"] as Dictionary<string, ProfilerChartParams>;
            if (chartlist != null && chartlist.Count > 0)
            {
                htmlstring = "<table>";
                foreach (string key in chartlist.Keys)
                {
                    ProfilerChartParams pcparams = chartlist[key];
                    htmlstring += "<tr>";
                    htmlstring += "<td style=\"width:90%;\"><span style=\"float: left;\">" + pcparams.MetricShortName + "</span></td>";
                    htmlstring += "<td><span id=\"" + key + "\" onclick=\"DeleteChartFromExportList('" + key + "')\" class=\"deletechart\" style=\"text-decoration:underline;cursor:pointer;\">Delete</span></td>";
                    htmlstring += "</tr>";
                }
                htmlstring += "</table>";
            }
            return htmlstring;
        }
        #endregion
        #region Delete chart from export list       
        public string DeleteChartFromExportList(string ChartID)
        {
            Dictionary<string, ProfilerChartParams> chartlist = HttpContext.Current.Session["ProfilerChartExportList"] as Dictionary<string, ProfilerChartParams>;
            if (chartlist.ContainsKey(ChartID))
            {
                chartlist.Remove(ChartID);
            }
            HttpContext.Current.Session["ProfilerChartExportList"] = chartlist;
            return ShowExportChartList();
        }
        #endregion
        #region  Clear all charts from export list     
        public string ClearAllChartsFromExportList()
        {
            Dictionary<string, ProfilerChartParams> chartlist = new Dictionary<string, ProfilerChartParams>();
            HttpContext.Current.Session["ProfilerChartExportList"] = chartlist;
            return ShowExportChartList();
        }
        #endregion
        public bool CheckExportChartList()
        {
            if (HttpContext.Current.Session["ProfilerChartExportList"] == null && HttpContext.Current.Session["ProfilerChartData"] == null)
            {
                return false;
            }
            return true;
        }
        public bool CheckChartReports()
        {
            Dictionary<string, ProfilerChartParams> chartlist = null;
            chartlist = HttpContext.Current.Session["ProfilerChartExportList"] as Dictionary<string, ProfilerChartParams>;
            if (chartlist != null && chartlist.Count > 0)
                return true;
            else
                return false;
        }
        public HighChartData HighChartGenerator()
        {
            //ProfilerChartParams profilerparams = new ProfilerChartParams();
            //profilerparams.Comparisonlist = string.Join("||", profilerchartparams.Comparisonlist.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray()); ;
            HighChartData HData = new HighChartData();
            try
            {               
                DataSet ds = null;
                ds = GetHighChartData();
                if (profilerchartparams.ActiveTab == "usp_profilerTrendRetailerTrip_TRENDCHANGE" || profilerchartparams.ActiveTab == "usp_profilerTrendRetailerShopper_TRENDCHANGE")
                {
                    if (profilerchartparams.TrendType == "1")
                        samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);
                    else if (profilerchartparams.TrendType == "2")
                        samplesizeArray = CommonFunctions.LoadChartSampleSizeSizeNew(ds);
                }
                else
                    samplesizelist = CommonFunctions.LoadChartSampleSizeSize(ds);

                var _Brands = from row in ds.Tables[0].AsEnumerable()
                              select Convert.ToString(row.Field<string>("Objective"));
                HData.BrandList = _Brands.Distinct().ToList();


                //Array.Sort(HData.BrandList, profilerparams.Comparisonlist);

                var _MetricNames = (from row in ds.Tables[1].AsEnumerable()
                                    select Convert.ToString(row.Field<string>("MetricItem"))).Distinct().ToList();

                HData.MetricList = (from row in ds.Tables[1].AsEnumerable()
                                    select Convert.ToString(row.Field<string>("MetricItem"))).Distinct().ToList();               

                var _SampleSize = from row in ds.Tables[0].AsEnumerable()
                                  where !Convert.ToString(row.Field<object>("MetricItem")).Equals("Number of Responses",StringComparison.OrdinalIgnoreCase)
                                  select string.IsNullOrEmpty(Convert.ToString(row.Field<object>("Volume"))) ? 0 : row.Field<double>("Volume");

                HData.SampleSize = _SampleSize.ToList();

                var _numberofResponses = from row in ds.Tables[0].AsEnumerable()
                                  where Convert.ToString(row.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                  select string.IsNullOrEmpty(Convert.ToString(row.Field<object>("Volume"))) ? 0 : row.Field<double>("Volume");

                if (_numberofResponses != null && _numberofResponses.Count() > 0)
                    HData.NumberOfResponses = _numberofResponses.ToList();
                else
                    HData.NumberOfResponses = _SampleSize.ToList();

                HData.ValueData = new List<List<double>>();
                HData.SignificanceData = new List<List<double>>();
                HData.ChangeVsPy = new List<List<double>>();

                foreach (var _met in _MetricNames)
                {
                    var series = new IValueData();
                    series.Values = new List<double>();

                    var _Sig = new List<double>();
                    var _Val = new List<double>();
                    var _ChgePy = new List<double>();
                    //var index = Array.FindIndex(_MetricNames, row => row.Contains("Convenience"));
                    //var index = Array.FindIndex(_MetricNames, row => row.Author == _met.ToString());
                    int index = _MetricNames.FindIndex(x => x.StartsWith(_met));
                    foreach (var _br in HData.BrandList)
                    {
                        if (profilerchartparams.ActiveTab == "usp_profilerTrendRetailerTrip_TRENDCHANGE" || profilerchartparams.ActiveTab == "usp_profilerTrendRetailerShopper_TRENDCHANGE")
                        {
                            if (profilerchartparams.TrendType == "1")
                            {
                                if (!IsSampleSizeless(_br))
                                {
                                    var _value = (from row in ds.Tables[1].AsEnumerable()
                                                  where row.Field<string>("Objective") == _br && row.Field<string>("MetricItem") == _met
                                                  select string.IsNullOrEmpty(Convert.ToString(row.Field<object>("Volume"))) ? 0 : Math.Round(row.Field<double>("Volume"), 1)).FirstOrDefault();
                                    _Val.Add(_value);
                                }
                                else
                                {
                                    _Val.Add(0);
                                }
                            }
                            else if (profilerchartparams.TrendType == "2")
                            {
                                if (!IsSampleSizelessNew(_br, index))
                                {
                                    var _value = (from row in ds.Tables[1].AsEnumerable()
                                                  where row.Field<string>("Objective") == _br && row.Field<string>("MetricItem") == _met
                                                  select string.IsNullOrEmpty(Convert.ToString(row.Field<object>("Volume"))) ? 0 : Math.Round(row.Field<double>("Volume"), 1)).FirstOrDefault();
                                    _Val.Add(_value);
                                }
                                else
                                {
                                    _Val.Add(0);
                                }
                            }
                        }
                        else {
                            if (!IsSampleSizeless(_br))
                            {
                                var _value = (from row in ds.Tables[1].AsEnumerable()
                                              where row.Field<string>("Objective") == _br && row.Field<string>("MetricItem") == _met
                                              select string.IsNullOrEmpty(Convert.ToString(row.Field<object>("Volume"))) ? 0 : Math.Round(row.Field<double>("Volume"), 1)).FirstOrDefault();
                                _Val.Add(_value);
                            }
                            else
                            {
                                _Val.Add(0);
                            }
                        }
                        
                        var _Signi = (from row in ds.Tables[1].AsEnumerable()
                                      where row.Field<string>("Objective") == _br && row.Field<string>("MetricItem") == _met
                                      select string.IsNullOrEmpty(Convert.ToString(row.Field<object>("Significance"))) ? 0 : row.Field<double>("Significance")).FirstOrDefault();
                        _Sig.Add(_Signi);

                        DataColumnCollection columns = ds.Tables[1].Columns;
                        if (columns.Contains("ChangePy"))
                        //if (profilerchartparams.ChartType == "Bar with Change" || profilerchartparams.ChartType == "Pyramid with Change")
                        {
                            var _ChangePy = (from row in ds.Tables[1].AsEnumerable()
                                             where row.Field<string>("Objective") == _br && row.Field<string>("MetricItem") == _met
                                             select string.IsNullOrEmpty(Convert.ToString(row.Field<object>("ChangePy"))) ? 0 : Math.Round(row.Field<double>("ChangePy"), 1)).FirstOrDefault();
                            _ChgePy.Add(_ChangePy);
                        }
                    }
                    HData.SignificanceData.Add(_Sig);
                    HData.ValueData.Add(_Val);
                    HData.ChangeVsPy.Add(_ChgePy);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return HData;
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
        
        public void PageLoadForHighChart()
        {
            if (HttpContext.Current.Session["ChartInputSelection"] != null)
            {
                profilerchartparams = HttpContext.Current.Session["ChartInputSelection"] as ProfilerChartParams;
            }
        }

        public iSHOPParams BindTabs(ProfilerChartParams profilerChartParams, out string tbltext, out string xmlstring, string ExportToExcel, string[] ShortNames, int width, int height)
        {
            profilerchartparams = profilerChartParams;
            View_Type = "COMPARE";
            iSHOPParams ishopParams = new iSHOPParams();
            DataSet ds = profilerChartParams.ChartDataSet;
            string _BenchMark = profilerChartParams.Benchmark;
            ShortNames = ShortNames.Select(x => x.ToLower()).ToArray();
            /**/
            //if (profilerchartparams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            //{               
            //    SelectedStatTest = "Benchmark";
            //}
            //else
            //{
            //    SelectedStatTest = profilerchartparams.CustomBase_ShortName;
            //}
            if (profilerChartParams.SelectedStatTest.ToString().Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = profilerChartParams.CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedStatTest = profilerChartParams.SelectedStatTest;
            }

            /**/
            param = new iSHOPParams();
            //SelectedStatTest = profilerChartParams.SelectedStatTest;
            //param.BenchMark = _BenchMark;
           // BenchMark = ShortNames[0];
            complist = new List<string>();
            var query = from r in ShortNames select r;
            if (Convert.ToString(profilerChartParams.ModuleBlock).IndexOf("TREND") > -1)
            {
                complist = profilerChartParams.ChartDataSet.Tables[0].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem" && x.ColumnName != "SortOrder").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
            }
            else
            {
                complist = query.ToList();
                List<string> tblcolumns = profilerChartParams.ChartDataSet.Tables[0].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem" && x.ColumnName != "SortOrder").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                //complist = tblcolumns.OrderBy(x => complist.IndexOf(x)).ToList();
                complist = tblcolumns.OrderBy(x => ShortNames.ToList().IndexOf(x.Trim().ToLower())).ToList();
                
            }
            frequency = profilerChartParams.ShopperFrequency;
            param.ShopperSegment = profilerChartParams.ShopperSegment; ;
            param.ShopperFrequency = profilerChartParams.ShopperFrequency;
            param.CustomFilters = profilerChartParams.FilterShortNames;

            table_width = width;
            //table_td_width = (width - 303) / (ShortNames.Count());
            table_td_width = 100;

            sharedStrings = new Dictionary<string, int>();
            int excelcolumnindex = 1;
            int rownumber = 6;

            accuratestatvalueposi = 0;
            accuratestatvaluenega = 0;
            if(HttpContext.Current.Session["StatSessionPosi"] != null)
            {
                accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
            }
            if (HttpContext.Current.Session["StatSessionNega"] != null)
            {
                accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
            }
            //Nagaraju 27-03-2014
            if (ExportToExcel == "true")
            {
                if (HttpContext.Current.Session["sharedstrings"] != null)
                {
                    sharedStrings = HttpContext.Current.Session["sharedstrings"] as Dictionary<string, int>;
                }
            }
            //End
            tbltext = string.Empty;
            string Significance = string.Empty;
            xmlstring = string.Empty;
            colmaxwidth = 0;
            string xmltext = string.Empty;
            mergeCell = new List<string>();

            try
            {
                xmltext += "<sheetData>";
                //write top header
                xmltext += WriteFilters(profilerChartParams);
                xmltext += AddSampleSizeNote();
                xmltext += GetTableHeader(complist.Count, Convert.ToString(profilerChartParams.ViewType));

                if (complist.Count > 1)
                {
                    mergeCell.Add("<mergeCell ref = \"B5:" + ColumnIndexToName(complist.Count) + "5\"/>");
                }
                //if (!sharedStrings.ContainsKey("BENCHMARK"))
                //{
                //    sharedStrings.Add("BENCHMARK", sharedStrings.Count());
                //}

                //if (!sharedStrings.ContainsKey("COMPARISON AREAS"))
                //{
                //    sharedStrings.Add("COMPARISON AREAS", sharedStrings.Count());
                //}

                //write second header
                excelcolumnindex = 0;
                xmltext += " <row" +
               " r = \"" + rownumber + "\" " +
                "spans = \"1:11\" " +
                "x14ac:dyDescent = \"0.25\">" +
               " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"5\"/>";
                excelcolumnindex += 1;

                tbltext = "<table class=\"tableheader\" style=\"width:" + table_width + "px;font-size:14px;text-transform:uppercase;\"><thead>";
                tbltext += CreateFirstTableHeader();
                tbltext += "<tr><td class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;padding: 1px;text-align: center;width:303px;background-color: #d5d6d6;color:black;font-weight:bold;font-size: 12px;border-bottom:1px solid black;  font-size: 12px;font-family: 'Chivo', sans-serif;letter-spacing: 0.3px;\">" + profilerChartParams.ShopperFrequencyShortName + "</td>";
                //create header
                string colNames;

                //write comparison               
                foreach (string Comparison in complist)
                {
                    colNames = Get_ShortNames(Comparison) + AddTradeAreaNoteforChannel(Get_ShortNames(Comparison));

                    tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;width:" + table_td_width + "px;background-color: #d5d6d6;color:black;font-size: 12px;height:30px;border-bottom:1px solid black;  font-size: 12px;font-family: 'Chivo', sans-serif;letter-spacing: 0.3px;\"><span title=\" " + colNames + " \">" + (colNames.Length > 30 ? colNames.Substring(0, 30) + "..." : colNames) + "</span></td>";
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                    xmlstring = cf.cleanExcelXML(colNames);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }

                    xmltext += " <c" +
                      " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                      " s = \"4\" " +
                      " t = \"s\">" +
                       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                   "</c>";
                    excelcolumnindex += 1;
                }
                tbltext += "</tr>";
                tbltext += "</thead>";
                xmltext += "</row>";
                //end header
                tbltext += "<tbody>";
                //------->
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (((BenchMark == "totaltrip" && profilerChartParams.ActiveTab == "sp_FactBookTripBevDetailsAcrossBeverageTripMainpro") && (profilerChartParams.MetricShortName == "PRODUCT TEMPERATURE" || profilerChartParams.MetricShortName == "CHILLED - LOCATION"
                || profilerChartParams.MetricShortName == "ROOM TEMPERATURE LOCATION" || profilerChartParams.MetricShortName == "INTENDED CONSUMER")) || ((param.ShopperSegment == "totaltrip||totaltrip" && profilerChartParams.ActiveTab == "sp_FactBookTripBevDetailsBeverageWithinMainPro") && (profilerChartParams.MetricShortName == "PRODUCT TEMPERATURE" || profilerChartParams.MetricShortName == "CHILLED - LOCATION"
                || profilerChartParams.MetricShortName == "ROOM TEMPERATURE LOCATION" || profilerChartParams.MetricShortName == "INTENDED CONSUMER")))
                    {
                        tbltext += "<tr><td style=\"text-align:center\">No data available</td></tr>";
                        int nodatarow = 9;

                        string metricitem = "No data available";
                        xmlstring = cf.cleanExcelXML(metricitem);

                       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                        {
                            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                        }
                        xmltext += "<row " +
                                        "r = \"" + nodatarow.ToString() + "\" " +
                                        "spans = \"1:11\" " +
                                        "ht = \"15\" " +
                                        "thickBot = \"1\" " +
                                        "x14ac:dyDescent = \"0.3\">" +
                                        "<c " +
                                            "r = \"C" + nodatarow.ToString() + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                        "</c></row> ";
                    }
                    else
                    {
                        int colms = ds.Tables[0].Columns.Count;
                        //rownumber = 7;
                        for (int tbl = 0; tbl < ds.Tables.Count; tbl++)
                        {
                            if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                            {
                                excelcolumnindex = 0;
                                sampleSize = new Dictionary<string, string>();
                                rownumber += 1;
                                tbltext += "<tr><td style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #E4EEEF") + ";color:#000000;width:303px;\"> " + Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString()) + " </td>";

                                foreach (string Comparison in complist)
                                {
                                    tbltext += "<td class=\"" + CleanClass(Comparison + "cell") + "\" style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #E4EEEF") + ";color:#000000;width:" + table_td_width + "px;\"></td>";
                                }

                                tbltext += "</tr>";

                                string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                                tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                xmlstring = cf.cleanExcelXML(tablename);

                               if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                                {
                                    sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                                }


                                ////write table name
                                //List<string> tblcolumns = new List<string>();
                                //foreach (object col in ds.Tables[tbl].Columns)
                                //{
                                //    string coln = Convert.ToString(col);
                                //    tblcolumns.Add(coln.Trim().ToLower().ToString());
                                //}

                                //write table name
                                List<string> tblcolumns = new List<string>();

                                tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem" && x.ColumnName != "SortOrder").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                                //List<string> comp = (from r in complistaArray select r.ToLower()).ToList();
                                //tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x)).ToList();

                                xmltext += "<row " +
                          "r = \"" + rownumber + "\" " +
                       "spans = \"1:11\" " +
                      " ht = \"15\" " +
                       "thickBot = \"1\" " +
                      " x14ac:dyDescent = \"0.3\">" +
                       "<c " +
                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                           "s = \"" + (xmlstring == "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)" ? 16 : 15) + "\" " +
                           "t = \"s\">" +
                           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                       "</c>";

                                for (int i = 0; i < complist.Count; i++)
                                {
                                    excelcolumnindex += 1;
                                    xmltext += "<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (xmlstring == "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)" ? 16 : 15) + "\"/>";
                                }


                                xmltext += "</row>";
                                for (int rows = 0; rows < ds.Tables[tbl].Rows.Count; rows++)
                                {
                                    excelcolumnindex = 0;
                                    DataRow dRow = ds.Tables[tbl].Rows[rows];
                                    Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                    if (!Significance.Trim().ToLower().Contains("significance") && !Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Equals("Number of Responses",StringComparison.OrdinalIgnoreCase))
                                    {
                                        rownumber += 1;
                                        //cellfontstyle = 2;
                                        tbltext += "<tr>";

                                        if (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average Number of Items" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average time in store" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Amount Spent" ||
                                     Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "AVERAGE ONLINE ORDER SIZE" ||
                                     Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "AVERAGE ONLINE BASKET SIZE")
                                        {
                                            average = "Average";
                                        }
                                        else
                                        {
                                            average = "";
                                        }
                                        //write sample size
                                        if (ds.Tables[tbl].Rows[rows]["MetricItem"].ToString() == "Number of Trips" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString() == "SampleSize" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().Contains("SampleSize") || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString() == "Sample Size")
                                        {
                                            sampleSize = new Dictionary<string, string>();
                                            tbltext += "<td style=\"overflow: hidden;padding: 1px;text-align: left;background-color: #FFFFFF; color:black;font-weight: bold;width:303px;font-size: 12px;font-family: Chivo, sans-serif;letter-spacing: 0.3px;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</td>";

                                            string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                            if (metricitem.Length > colmaxwidth)
                                            {
                                                colmaxwidth = metricitem.Length;
                                            }
                                            metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                            xmlstring = cf.cleanExcelXML(metricitem);
                                           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                                            {
                                                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                                            }

                                            xmltext += "<row " +
                                           "r = \"" + rownumber + "\" " +
                                           "spans = \"1:11\" " +
                                           "ht = \"15\" " +
                                           "thickBot = \"1\" " +
                                           "x14ac:dyDescent = \"0.3\">" +
                                           "<c " +
                                               "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                               "s = \"3\" " +
                                               "t = \"s\">" +
                                               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                           "</c> ";

                                            //plot sample size
                                            foreach (string Comparison in complist)
                                            {
                                                excelcolumnindex += 1;

                                                if (!string.IsNullOrEmpty(Comparison))
                                                {
                                                    if (tblcolumns.Contains((Comparison).Trim().ToLower()))
                                                    {
                                                        tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;background-color: #FFFFFF; color:black;font-weight: bold;width:;font-size: 12px;font-family: Chivo, sans-serif;letter-spacing: 0.3px;" + table_td_width + "px;\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]),out samplecellstyle) + "</td>";
                                                        string samplesize = (from r in ds.Tables[tbl].AsEnumerable()
                                                                             where Convert.ToString(r["MetricItem"]).Equals(metricitem, StringComparison.OrdinalIgnoreCase)
                                                                             select Convert.ToString(r[Comparison])).FirstOrDefault();

                                                        if(!string.IsNullOrEmpty(samplesize))
                                                            sampleSize.Add(Comparison, samplesize);
                                                        else
                                                        sampleSize.Add(Comparison, Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]));

                                                        if (samplecellstyle == 20)
                                                        {
                                                            //string lowsamplesize = "0";
                                                            //if(!string.IsNullOrEmpty(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]))
                                                            //    && Convert.ToDouble(ds.Tables[tbl].Rows[rows][(Comparison)]) > 0)
                                                            //lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])));

                                                            string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])));

                                                            if (!sharedStrings.ContainsKey(lowsamplesize))
                                                            {
                                                                sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                            }
                                                            xmltext += " <c" +
                                                                        " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"2\" " +
                                                                        " t = \"s\">" +
                                                                        "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                        "</c>";
                                                        }
                                                        else if (samplecellstyle == 30)
                                                        {
                                                            string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])));
                                                            if (!sharedStrings.ContainsKey(lowsamplesize))
                                                            {
                                                                sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                            }
                                                            xmltext += " <c" +
                                                                        " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"2\" " +
                                                                        " t = \"s\">" +
                                                                        "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                        "</c>";
                                                        }
                                                        else
                                                        {

                                                            xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                       " s = \"2\">" +
                                                                  "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])) + "</v>" +
                                                                        "</c> ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;background-color: #E6E6E6; color:black;font-weight: bold;\"></td>";
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                        " s = \"2\">" +
                                                          "<v></v>" +
                                                             "</c> ";
                                                    }
                                                }
                                            }
                                            tbltext += "</tr>";
                                            xmltext += "</row>";
                                            //End Sample Size
                                        }

                                        else
                                        {
                                            tbltext += "<td style=\"overflow: hidden;padding: 1px;text-align: left;width:303px;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</td>";

                                            string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));
                                            if (metricitem.Length > colmaxwidth)
                                            {
                                                colmaxwidth = metricitem.Length;
                                            }
                                            metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                            xmlstring = cf.cleanExcelXML(metricitem);

                                           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                                            {
                                                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                                            }

                                            xmltext += "<row " +
                                           "r = \"" + rownumber + "\" " +
                                           "spans = \"1:11\" " +
                                           "ht = \"15\" " +
                                           "thickBot = \"1\" " +
                                           "x14ac:dyDescent = \"0.3\">" +
                                           "<c " +
                                               "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                               "s = \"" + (average == "Average" ? "16" : "5") + "\" " +
                                               "t = \"s\">" +
                                               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                           "</c> ";
                                            //cellfontstyle = 8;

                                            foreach (string Comparison in complist)
                                            {
                                                BenchmarkOrComparison = Comparison;
                                                excelcolumnindex += 1;

                                                if (!string.IsNullOrEmpty(Comparison))
                                                {
                                                    if (tblcolumns.Contains((Comparison).Trim().ToLower()))
                                                    {
                                                        if (CheckSampleSize(Comparison))
                                                        {
                                                            if (average == "Average")
                                                            {
                                                                tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;width:" + table_td_width + "px;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " t=\"s\"  s = \"17\">" +
                                                                  "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]), tablename)] + "</v>" +
                                                                     "</c> ";
                                                            }
                                                            else
                                                            {
                                                                    tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;width:" + table_td_width + "px;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])) + "</v>" +
                                                                     "</c> ";
                                                            }

                                                        }
                                                        else if (CommonFunctions.CheckMediumSampleSize(Comparison, sampleSize))
                                                        {
                                                            if (average == "Average")
                                                            {
                                                                tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;width:" + table_td_width + "px;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: #ededee;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " t=\"s\" s = \"17\">" +
                                                                  "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]), tablename)] + "</v>" +
                                                                     "</c> ";
                                                            }
                                                            else
                                                            {
                                                                tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;width:" + table_td_width + "px;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: #ededee;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)])) + "</v>" +
                                                                     "</c> ";
                                                            }

                                                        }
                                                        else
                                                        {
                                                            string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]));
                                                            if (!string.IsNullOrEmpty(na))
                                                            {
                                                                GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[rows][(Comparison)]));
                                                                if (!sharedStrings.ContainsKey(na))
                                                                {
                                                                    sharedStrings.Add(na, sharedStrings.Count());
                                                                }
                                                                if (cellfontstyle == 30)
                                                                {
                                                                   xmltext +="<c " +
                                                "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                "s = \"" + cellfontstyle + "\" " +
                                                "t = \"s\">" +
                                                "<v>" + sharedStrings[na] + "</v>" +
                                            "</c> ";
                                                                }
                                                                else
                                                                {
                                                                    xmltext += "<c " +
                                                 "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                 "s = \"" + (average == "Average" ? "16" : cellfontstyle.ToString()) + "\" " +
                                                 "t = \"s\">" +
                                                 "<v>" + sharedStrings[na] + "</v>" +
                                             "</c> ";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;width:" + table_td_width + "px;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: #FFFFFF;") + " color: black;font-size: 12px;font-family: Chivo, sans-serif;letter-spacing: 0.3px;\"></td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                            " s = \"" + (average == "Average" ? "17" : "8") + "\">" +
                                                                        "<v></v>" +
                                                                        "</c> ";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tbltext += "<td  style=\"overflow: hidden;padding: 1px;text-align: center;width:" + table_td_width + "px;background-color: #FFFFFF; color:black;font-size: 12px;font-family: Chivo, sans-serif;letter-spacing: 0.3px;\"></td>";
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle + "\">" +
                                                                             "<v></v>" +
                                                                                "</c> ";
                                                    }
                                                }

                                            }
                                            xmltext += "</row>";
                                            tbltext += "</tr>";
                                        }
                                    }
                                }

                            }

                        }
                    }

                }
                else
                {
                    tbltext += "<tr><td style=\"text-align:center\">No data available</td></tr>";
                    int nodatarow = 9;

                    string metricitem = "No data available";
                    xmlstring = cf.cleanExcelXML(metricitem);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }
                    xmltext += "<row " +
                                    "r = \"" + nodatarow.ToString() + "\" " +
                                    "spans = \"1:11\" " +
                                    "ht = \"15\" " +
                                    "thickBot = \"1\" " +
                                    "x14ac:dyDescent = \"0.3\">" +
                                    "<c " +
                                        "r = \"C" + nodatarow.ToString() + "\" " +
                                        "t = \"s\">" +
                                        "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                    "</c></row> ";
                }

                tbltext += "</tbody></table>";

                xmltext += "</sheetData>";

                if (mergeCell.Count > 0)
                {
                    string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                    foreach (string mergrrow in mergeCell)
                    {
                        mergetext += mergrrow;

                    }
                    mergetext += "</mergeCells>";
                    xmltext += mergetext;
                }

                xmltext += GetPageMargins();
                xmltext = GetSheetHeadandColumns() + xmltext;
                //Nagaraju 27-03-2014
                xmlstring = xmltext;
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                //exportfiles = new Dictionary<string, string>();
                //exportfiles.Add("tab1", xmltext);
                //HttpContext.Current.Session["exportfiles"] = exportfiles;     

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }
        public string WriteFilters(ProfilerChartParams profilerParams)
        {

            string value = "";
            switch (accuratestatvalueposi.ToString())
            {
                case "1.2816":
                    value = "80";
                    break;
                case "1.6449":
                    value = "90";
                    break;

                case "1.96":
                    value = "95";
                    break;

                case "2.5758":
                    value = "99";
                    break;
            }
            string xmlstring = "* Selection";
            string xmltext = string.Empty;
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
           "r = \"1\" " +
           "spans = \"1:11\" " +
           "ht = \"15\" " +
           "thickBot = \"1\" " +
           "x14ac:dyDescent = \"0.3\">" +
           "<c " +
               "r = \"B1\" " +
               "s = \"9\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmlstring = "* Filters";
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"C1\" " +
               "s = \"9\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";


            xmlstring = "Stat Test:";
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"D1\" " +
               "s = \"10\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmltext += "</row>";

            //Time Period
            if (Convert.ToString(profilerParams.ModuleBlock).IndexOf("TREND") > -1)
            {
                xmlstring = "Time Period :" + profilerParams.ShortTimePeriod;
            }
            else
            {
                if (!string.IsNullOrEmpty(profilerParams.TimePeriod))
                {
                    if (profilerParams.TimePeriod.IndexOf("3MMT") > -1)
                    {
                        xmlstring = "Time Period : " + profilerParams.TimePeriod.Split('|')[1] + " 3MMT";
                    }
                    else if (profilerParams.TimePeriod.IndexOf("total") > -1)
                    {
                        xmlstring = "Time Period : AUG 2013 TO DEC 2014";
                    }
                    else
                    {
                        xmlstring = "Time Period : " + profilerParams.TimePeriod.Split('|')[1];
                    }
                }
                else
                {
                    xmlstring = "Time Period :";
                }
            }
            xmlstring = cf.cleanExcelXML("Time Period : " + Convert.ToString(profilerParams.ShortTimePeriod.Replace(":", "")));
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
   "r = \"2\" " +
   "spans = \"1:11\" " +
   "ht = \"15\" " +
   "thickBot = \"1\" " +
   "x14ac:dyDescent = \"0.3\">" +
   "<c " +
       "r = \"B2\" " +
       "s = \"11\" " +
       "t = \"s\">" +
       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
   "</c>";

            if (BenchMark.IndexOf("Category") > -1 || BenchMark.IndexOf("Brand") > -1 || CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1 || CheckString.IndexOf("Channels") > -1)
            {
                if (profilerParams.ShopperFrequency.IndexOf("channels") > -1 || profilerParams.ShopperFrequency.IndexOf("retailers") > -1)
                {
                    string[] cr = profilerParams.ShopperFrequency.Split(new String[] { "|", "|" },
                                   StringSplitOptions.RemoveEmptyEntries);
                    string text = string.Empty;
                    for (int i = 1; i < cr.Length; i += 2)
                    {
                        text += Get_ShortNames(cr[i]) + ", ";
                    }
                    xmlstring = "Channel/Retailer : " + text;
                }
                else
                {
                    xmlstring = "Monthly Purchasing Amount : " + profilerParams.ShopperFrequency;
                }
            }
            else
            {
                //xmlstring = "Shopping Frequency: " + frequency;
                xmlstring = "Shopping Frequency: " + Get_ShortNamesFrequency(profilerParams.ShopperFrequencyShortName);

            }
            if (profilerParams.ModuleBlock == "AcrossBeverageTrips" || profilerParams.ModuleBlock == "WithinBeverageTrips")
            {
                xmlstring = cf.cleanExcelXML((!string.IsNullOrEmpty(profilerParams.FrequencyTitle) ? profilerParams.FrequencyTitle : "Frequency") + ": " + "");
            }
            else
            {
                xmlstring = cf.cleanExcelXML((!string.IsNullOrEmpty(profilerParams.FrequencyTitle) ? profilerParams.FrequencyTitle : "Frequency") + ": " + Get_ShortNamesFrequency(profilerParams.ShopperFrequencyShortName));
            }
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
           "r = \"C2\" " +
           "s = \"11\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ";

            xmlstring = ">" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"D2\" " +
               "s = \"12\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmltext += "</row>";

            //Single Selection
            if (!string.IsNullOrEmpty(param.ShopperSegment))
            {
                string CategoryBrandSplit = param.ShopperSegment.Split('|').Last();
                if (CheckString.IndexOf("Channel") > -1 || CheckString.IndexOf("Retailer") > -1)
                {
                    xmlstring = "Channel/Retailer : " + param.ShopperSegment;
                }
                else if (CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1)
                {
                    xmlstring = "Category/Brand : " + param.ShopperSegment;
                }
                else
                {
                    xmlstring = "";
                }
                if (param.ShopperSegment.IndexOf("Channels") > -1 || param.ShopperSegment.IndexOf("Retailer") > -1)
                {
                    xmlstring = "Channel/Retailer : " + Get_ShortNames(CategoryBrandSplit);//param.ShopperSegment.Replace("Channels|","").Replace("Retailer|",""));
                }
                else if (param.ShopperSegment.IndexOf("Category") > -1 || param.ShopperSegment.IndexOf("Brand") > -1)
                {
                    if (profilerParams.ModuleBlock == "WithinBeverageTrips")
                    {
                        xmlstring = "Category/Brand : " + Get_ShortNames(CategoryBrandSplit) + Environment.NewLine + " :  Channel/Retailer : " + Get_ShortNamesFrequency(profilerParams.ShopperFrequencyShortName);
                        //xmlstring = "Channel/Retailer : " + Get_ShortNamesFrequency(profilerParams.ShopperFrequencyShortName);
                    }
                    else
                    {
                        xmlstring = "Category/Brand : " + Get_ShortNames(CategoryBrandSplit);//param.ShopperSegment.Replace("Category||","").Replace("Brand|",""));
                    }
                }
                else
                {
                    xmlstring = "";
                }
            }
            else
            {
                if (profilerParams.ModuleBlock == "AcrossBeverageTrips")
                {
                    xmlstring = "Channel/Retailer : " + Get_ShortNamesFrequency(profilerParams.ShopperFrequencyShortName);//param.ShopperSegment.Replace("Channels|","").Replace("Retailer|",""));
                }
                else
                {
                    xmlstring = "";
                }
            }
            // xmlstring = "Single Selection";
            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
     "r = \"3\" " +
     "spans = \"1:11\" " +
     "ht = \"15\" " +
     "thickBot = \"1\" " +
     "x14ac:dyDescent = \"0.3\">" +
     "<c " +
         "r = \"B3\" " +
         "s = \"13\" " +
         "t = \"s\">" +
         "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
     "</c>";

            string CustomFilter = string.Empty;
            if (param.CustomFilters != null)
            {
                //string[] ss = param.CustomFilters.Split(new String[] { "|", "|" },
                //                        StringSplitOptions.RemoveEmptyEntries);

                //for (int i = 0; i < ss.Length; i += 2)
                //{
                //    ss[i] = ss[i] + ": ";
                //}

                //for (int i = 1; i < ss.Length; i += 2)
                //{
                //    ss[i] = ss[i] + ", ";
                //}
                //foreach (string xmlfilter in ss)
                //{
                //    CustomFilter += xmlfilter;
                //}
                CustomFilter = cf.GetExcelSortedFilters(param.CustomFilters);
            }
            if (CustomFilter != "")
                xmlstring = CustomFilter;
            else
                xmlstring = " : ";
            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }
            xmltext += "<c " +
           "r = \"C3\" " +
           "s = \"13\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ";

            xmlstring = "<" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"D3\" " +
               "s = \"14\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmltext += "</row>";
            //Single Selection

            //       xmlstring = "Single Selection";
            //       xmlstring = cf.cleanExcelXML(xmlstring);
            //      if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            //       {
            //           sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            //       }

            //       xmltext += "<row " +
            //"r = \"3\" " +
            //"spans = \"1:11\" " +
            //"ht = \"15\" " +
            //"thickBot = \"1\" " +
            //"x14ac:dyDescent = \"0.3\">" +
            //"<c " +
            //    "r = \"B4\" " +
            //    "s = \"13\" " +
            //    "t = \"s\">" +
            //    "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
            //"</c></row> ";

            return xmltext;

        }
        #region Add sample size note
        private string AddSampleSizeNote()
        {
            string samplesizenote = cf.cleanExcelXML("NOTE : GREY FONT = LOW SAMPLE (30-99), BLANK = SAMPLE < 30; NA = NOT APPLICABLE");
            StringBuilder xmltext = new StringBuilder();
            if (!sharedStrings.ContainsKey(samplesizenote))
            {
                sharedStrings.Add(samplesizenote, sharedStrings.Count());
            }
            xmltext.Append("<row" +
                " r = \"4\" " +
                 "spans = \"1:11\" " +
                 "ht = \"16.5\" " +
                 "thickTop = \"1\" " +
                 "thickBot = \"1\" " +
                 "x14ac:dyDescent = \"0.3\">" +
                 "<c " +
                     "r = \"C4\" " +
                     "s = \"13\" " +
                     "t = \"s\">" +
                    "<v>" + sharedStrings[samplesizenote] + "</v>" +
                 "</c>");

            xmltext.Append("</row> ");
            return xmltext.ToString();
        }
        #endregion
        private string GetTableHeader(int comparisons, string viewtype)
        {
            StringBuilder xmltext = new StringBuilder();
            if (!sharedStrings.ContainsKey(viewtype))
            {
                sharedStrings.Add(viewtype, sharedStrings.Count());
            }
            xmltext.Append("<row" +
                " r = \"5\" " +
                 "spans = \"1:11\" " +
                 "ht = \"16.5\" " +
                 "thickTop = \"1\" " +
                 "thickBot = \"1\" " +
                 "x14ac:dyDescent = \"0.3\">" +
                 "<c " +
                     "r = \"B5\" " +
                     "s = \"18\" " +
                     "t = \"s\">" +
                    "<v>" + sharedStrings[viewtype] + "</v>" +
                 "</c>" +
                 "<c " +
                     "r = \"C5\" " +
                     "s = \"18\" " +
                     "t = \"s\">" +
                 "</c>");

            xmltext.Append("</row> ");
            return xmltext.ToString();
        }

        public string ColumnIndexToName(int columnIndex)
        {
            char second = (char)(((int)'A') + columnIndex % 26);

            columnIndex /= 26;

            if (columnIndex == 0)
                return second.ToString();
            else
                return ((char)(((int)'A') - 1 + columnIndex)).ToString() + second.ToString();
        }
        private string Get_ShortNames(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (HeaderTabs.ContainsKey(spVal))
                    slRetVal = HeaderTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        private string GetLoyaltyPyramidName(string value)
        {
            string Name = string.Empty;
            if (LoyaltyRetailerList.ContainsKey(value))
            {
                Name = Convert.ToString(LoyaltyRetailerList[value]);
            }
            return Name;
        }
        private string CheckNAValues(string value)
        {
            string NA = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (!IsApplicable)
            {
                return NA = GlobalVariables.NA;
            }
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                return NA = GlobalVariables.NA;
            }
            if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
            {
                return NA = GlobalVariables.NA;
            }
            if (CheckSampleSize(BenchmarkOrComparison) == false && CommonFunctions.CheckMediumSampleSize(BenchmarkOrComparison, sampleSize) == false && CheckRetailerorChannel == false)
            {
                return NA;
            }
            if (string.IsNullOrEmpty(value) && LoyaltyPyramid) //&& LoyaltyPyramid)
            {
                NA = GlobalVariables.NA;

            }
            else if ((StoreImageryCheck || CheckRetailerorChannel || LoyaltyPyramidForRetailers) || (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips)))
            {
                NA = GlobalVariables.NA;
            }
            return NA;
        }
        private bool CheckTotal_DeliveryMethodUseItem(string item)
        {
            switch (item.ToLower())
            {
                case "total shopper":
                case "total online shopper":
                case "total online grocery shopper":
                    {
                        IsApplicable = false;
                        return true;
                    }
            }
            return false;
        }
        private bool CheckSampleSize(string samplesizekey)
        {
            IsApplicable = true;
            try
            {
                if (sampleSize.ContainsKey(samplesizekey))
                {
                    string val = sampleSize[samplesizekey];
                    if (string.IsNullOrEmpty(val))
                        return false;
                    double szvalue = Convert.ToDouble(sampleSize[samplesizekey]);
                    //added by Nagaraju for DELIVERY METHOD USE
                    //date: 24-04-2017

                    if (View_Type.Equals("COMPARE", StringComparison.OrdinalIgnoreCase) && LoyaltyPyramidmetric.Equals("DELIVERY METHOD USE", StringComparison.OrdinalIgnoreCase)
                        && TabIndexId == 2)
                    {
                        CheckTotal_DeliveryMethodUseItem(samplesizekey);
                    }
                    else if ((View_Type.Equals("PIT", StringComparison.OrdinalIgnoreCase) && LoyaltyPyramidmetric.Equals("DELIVERY METHOD USE", StringComparison.OrdinalIgnoreCase)
                       && TabIndexId == 2)
                        || (View_Type.Equals("TREND", StringComparison.OrdinalIgnoreCase) && LoyaltyPyramidmetric.Equals("DELIVERY METHOD USE", StringComparison.OrdinalIgnoreCase)
                     && TabIndexId == 2))
                    {
                        CheckTotal_DeliveryMethodUseItem(param.ShopperSegment);
                    }

                    if (!IsApplicable)
                        return false;

                    if (szvalue >= 100)
                    {
                        return true;
                    }
                    else
                    {
                        //cellfontstyle = 10;
                        if (szvalue == GlobalVariables.NANumber)
                        {
                            IsApplicable = false;
                        }
                        return false;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }    
             
        private int GetRowNumber(DataTable tbl, int currentrow)
        {
            int rownum = 0;
            if (tbl.Rows.Count > currentrow + 1)
            {
                rownum = currentrow + 1;
            }
            else
            {
                rownum = currentrow;
            }

            return rownum;

        }

        private string CheckXMLBlankValues(string rowvalue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(rowvalue))
            {
                value = "";
            }
            //else if (Convert.ToDouble(rowvalue) == 0.0)
            //{
            //    value = "";
            //}
            else
            {
                double val = Convert.ToDouble(rowvalue) / 100;
                value = val.ToString();
            }
            return value;
        }

        private string CheckBlankValues(string rowvalue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(rowvalue))
            {
                value = "";
            }
            //else if (Convert.ToDouble(rowvalue) == 0.0)
            //{
            //    value = "";
            //}
            else
            {
                value = Convert.ToString(CommonFunctions.GetRoundingValue(rowvalue)) + "%";
            }
            return value;
        }

        private string CheckBlankValues(string rowvalue, string tablename)
        {
            string value = string.Empty;
            if (tablename == "# of Items Purchased")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }
            else if (tablename == "Time Spent in Store")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minute";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minutes";
                }
            }
            else if (tablename == "Trip Expenditure")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }

            return value;
        }

        private string GetCellColor(string currentrow, string significancerow, string significancevalue)
        {
            string color = string.Empty;
            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstyle = 28;
                    }
                    else if(IsApplicable == false)
                    {
                        color = "color:black";
                        cellfontstyle = 8;
                    }
                     
                    else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    {
                        color = "color:#20B250";

                        cellfontstyle = 7;
                    }
                    else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    {
                        color = "color:red";
                        cellfontstyle = 6;
                    }
                    else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                    {
                        color = "color:black";
                        cellfontstyle = 8;
                    }

                }
            }
            else
            {
                if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 28;
                }
                else
                {
                    color = "color:black";
                    cellfontstyle = 8;
                }
            }
            return color;
        }

        private string GetCellColorGrey(string currentrow, string significancerow, string significancevalue)
        {
            string color = string.Empty;
            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstylegrey = 29;
                    }                   
                    else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    {
                        color = "color:red";
                        cellfontstylegrey = 23;
                    }
                    else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    {
                        color = "color:#20B250";
                        cellfontstylegrey = 22;
                    }
                    else
                    {
                        color = "color:gray";
                        cellfontstylegrey = 19;
                    }
                }
            }
            else
            {
                if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstylegrey = 29;
                }
                else
                {
                    color = "color:black";
                    cellfontstylegrey = 19;
                }
            }
            return color;
        }

        private string GetPageMargins()
        {
            string pagem = "<pageMargins " +
                "left = \"0.7\" " +
                "right = \"0.7\" " +
                "top = \"0.75\" " +
                "bottom = \"0.75\" " +
                "header = \"0.3\" " +
                "footer = \"0.3\"/>" +
                 "<pageSetup " +
                "paperSize = \"9\" " +
                "orientation = \"portrait\" " +
                "r:id = \"rId1\"/>" +
                "<drawing r:id = \"rId2\"/>";

            return pagem;

        }

        private string CheckXMLBlankValues(string rowvalue, string tablename)
        {
            string value = string.Empty;
            string valuereturn = string.Empty;

            if (tablename == "# of Items Purchased")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }

            else if (tablename == "Time Spent in Store")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minute";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minutes";
                }
            }

            else if (tablename == "Trip Expenditure")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    double val = Convert.ToDouble(rowvalue) / 100;
                    value = val.ToString();
                }
            }

            valuereturn = cf.cleanExcelXML(value);
            if (!sharedStrings.ContainsKey(valuereturn))
            {
                sharedStrings.Add(valuereturn, sharedStrings.Count());
            }
            //else
            //{
            //    valuereturn = Convert.ToString(sharedStrings[valuereturn]);
            //}
            return valuereturn;
        }

        private string GetSheetHeadandColumns()
        {
            if (colmaxwidth <= 40)
            {
                colmaxwidth = 40;
            }

            string sheetstr = "xmlns = \"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
        "xmlns:r = \"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" " +
        "xmlns:mc = \"http://schemas.openxmlformats.org/markup-compatibility/2006\" " +
        "mc:Ignorable = \"x14ac\" " +
        "xmlns:x14ac = \"http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac\"> " +
        "<dimension ref = \"A1\"/> " +
        "<sheetViews> " +
            "<sheetView showGridLines = \"0\" tabSelected = \"1\" workbookViewId = \"0\"> " +
                "<selection activeCell = \"A1\" sqref = \"A1\"/> " +
           "</sheetView> " +
        "</sheetViews> " +
        "<sheetFormatPr defaultRowHeight = \"15\" x14ac:dyDescent = \"0.25\"/> " +
        "<cols> ";

            for (int i = 0; i < (complist.Count + 1); i++)
            {
                if (i == 0)
                {
                    sheetstr += "<col " +
                   "min = \"" + (i + 1) + "\" " +
                   "max = \"" + (i + 1) + "\" " +
                   "width = \"" + colmaxwidth.ToString() + "\" " +
                   "customWidth = \"1\"/> ";
                }
                else
                {
                    sheetstr += "<col " +
                 "min = \"" + (i + 1) + "\" " +
                "max = \"" + (i + 1) + "\" " +
                 "width = \"30\" " +
                 "customWidth = \"1\"/> ";
                }
            }
            sheetstr += "</cols>";
            return sheetstr;
        }



        public void Shortnames()
        {
            FilterTabs.Clear();
            FilterTabs.Add("MainStore", "Main Store (in channel)");
            FilterTabs.Add("MainStoreOverAll", "Main Store (across channel)");
            FilterTabs.Add("FavoriteStore", "Favorite Store (in channel)");
            FilterTabs.Add("FavoriteStoreOverAll", "Favorite Store (across channel)");
        }

        private string Get_ShortNamesFrequency(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (FilterTabs.ContainsKey(spVal))
                    slRetVal = FilterTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        private string CreateFirstTableHeader()
        {
            string table = string.Empty;
            table += "<tr>";
            table += "<td style=\"height: 32px;width:303px;background-color: #d5d6d6;color:black;font-weight:bold;font-size: 12px;border-bottom:1px solid black;  font-size: 12px;font-family: 'Chivo', sans-serif;letter-spacing: 0.3px;font-weight:bold;\" class=\"ShoppingFrequencytitle\">Shopping Frequency</td>";
            table += "<td class=\"benchmarktitle\" style=\"width:" + table_td_width + "px;background-color: #d5d6d6;color:black;font-weight:bold;border-bottom:1px solid black;  font-size: 12px;font-family: 'Chivo', sans-serif;letter-spacing: 0.3px;font-weight:bold;\"></td>";
            table += "<td colspan=\"" + (complist.Count - 1) + "\"  class=\"comparisonheader\" style=\"width:" + (table_td_width * (complist.Count - 1)) + "px;background-color: #d5d6d6;color:black;font-weight:bold;font-size: 12px;border-bottom:1px solid black;font-family:'Chivo',sans-serif;letter-spacing: 0.3px;\"></td>";
            table += "</tr>";
            return table;
        }

        private string CreateFirstTableHeaderOvertime()
        {
            HeaderTable = "<table></thead>";
            string table = string.Empty;
            table += "<tr>";
            table += "<td style=\"height: 32px;width:303px;\" class=\"ShoppingFrequencytitle\"></td>";
            table += "<td class=\"benchmarktitle\" style=\"width:" + table_td_width + "px;\"></td>";
            table += "<td class=\"comparisonheader\" style=\"\"></td>";

            HeaderTable += "<tr><td style=\"height: 32px;width:303px;\" class=\"ShoppingFrequencytitle\"></td>";
            HeaderTable += "<td class=\"benchmarktitle\" style=\"width:" + table_td_width + "px;\"></td></tr>";
            if (complist != null && complist.Count > 1)
                HeaderTable += "<tr><td class=\"" + CleanClass(Convert.ToString(complist[1])) + "header\" style=\"\"></td>";

            if (complist != null && complist.Count > 2)
            {
                for (int i = 2; i < complist.Count; i++)
                {
                    table += "<td class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"></td>";
                    HeaderTable += "<td class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"></td>";
                }
            }
            HeaderTable += "</tr>";
            table += "</tr>";
            return table;
        }
        private string AddTradeAreaNoteforChannel(string ChannelRetailer)
        {
            string TradeNode = string.Empty;
            if (frequency == "Store In Trade Area")
            {
                if (ChannelRetailer.ToLower() == "convenience" || ChannelRetailer.ToLower() == "dollar" || ChannelRetailer.ToLower() == "supermarketgrocery" ||
                  ChannelRetailer.ToLower() == "massmerc" || ChannelRetailer.ToLower() == "drug" || ChannelRetailer.ToLower() == "club" ||
                  ChannelRetailer.ToLower() == "supercenter" || ChannelRetailer.ToLower() == "total shopper" || ChannelRetailer.ToLower() == "total trips")
                {
                    TradeNode = " (Any Priority Store in Trade Area)";
                }
            }
            return TradeNode;
        }
        public string CleanClass(string _class)
        {
            _class = Regex.Replace(_class, @"[/\s,`/@#$%;&*~()+/]", "");
            return _class;
        }
    }
}

 