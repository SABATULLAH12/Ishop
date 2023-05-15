using iSHOPNew.CommonFilters;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace iSHOPNew.Controllers
{
     [AuthenticateUser]
    public class ChartsController : Controller
    {
        Chart chart = new Chart();
        iSHOPParams param = new iSHOPParams();
        iSHOPParams Subparam = new iSHOPParams();
        Models.Chart Chart = new Models.Chart();
        Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        List<string> CompList = new List<string>();
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        List<string> selectedMetrics = new List<string>();
        List<string> ChartXValues = new List<string>();
        List<string> BCFullNames = new List<string>();
        CommonTable objCommonTable = new CommonTable();

        ProfilerChartParams profilerchartparams = null;
        public ActionResult CompareRetailers()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult RetailerDeepDive()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult CompareBeverages()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult BeverageDeepDive()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public void ExportToPPT()
        {
            AsposeDownload ppt = new AsposeDownload();
            ppt.GeneratePPT(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
        }

        #region Get Chart Data
        [HttpPost]
        public JsonResult GetChartData(ProfilerChartParams _profilerparams)
        {
            
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });
            //log selection
            LogEntries.LogSelection("CHARTS");
            HighChartData HData = new HighChartData();            
            try
            {
                _profilerparams.ChartType = _profilerparams.ChartType;
                _profilerparams.ActiveTab = _profilerparams.ActiveTab;
                if (_profilerparams.ActiveTab == "usp_profilerWithinRetailerTrip")
                {
                    _profilerparams.Benchmark = _profilerparams.Benchmark.ToMyString();
                    _profilerparams.Comparisonlist = string.Join("|", _profilerparams.Comparisonlist);
                }
                else
                {
                    _profilerparams.Benchmark = _profilerparams.Comparison_DBNames[0].ToMyString();
                    _profilerparams.Comparison_DBNames.RemoveAt(0);
                    _profilerparams.Comparisonlist = string.Join("|", _profilerparams.Comparison_DBNames);
                }
                _profilerparams.TimePeriod = _profilerparams.TimePeriod.ToMyString();//"3MMT|Sep 2016";
                _profilerparams.ShortTimePeriod = _profilerparams.ShortTimePeriod.ToMyString();//"Sep 2016 3MMT";
                _profilerparams.Filters = _profilerparams.Filters.ToMyString();
                _profilerparams.FrequencyTitle = _profilerparams.FrequencyTitle.ToMyString();
                _profilerparams.ShopperFrequency = _profilerparams.ShopperFrequency.ToMyString();//"Store In Trade Area";
                _profilerparams.ShopperFrequencyShortName = _profilerparams.ShopperFrequency.ToMyString();
                _profilerparams.Metric = _profilerparams.Metric.ToMyString();//"Gender";
                _profilerparams.MetricShortName = _profilerparams.Metric.ToMyString();
                _profilerparams.ModuleBlock = _profilerparams.ModuleBlock.ToMyString();
                _profilerparams.FilterShortNames = _profilerparams.FilterShortNames.ToMyString(); //string.Empty;             
                _profilerparams.ShopperSegment = _profilerparams.ShopperSegment.ToMyString();
                _profilerparams.View = _profilerparams.View;
                _profilerparams.View = _profilerparams.View.ToMyString();
                _profilerparams.SelectedMetrics = _profilerparams.SelectedMetrics.ToMyString();//"Male|Female";
                _profilerparams.IsSelectionChange = true;

                _profilerparams.SelectedStatTest = _profilerparams.SelectedStatTest.ToMyString();//"BENCHMARK";

                _profilerparams.TrendType = _profilerparams.TrendType.ToMyString();//"BENCHMARK";

                //New Id Params

                if (_profilerparams.Tab_Id_mapping == "true")
                {
                    string benchmark_UID = string.Empty;
                    string comp_UID = string.Empty;
                    if (_profilerparams.SelectedStatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase) && !_profilerparams.ModuleBlock.Equals("TREND", StringComparison.OrdinalIgnoreCase))
                    {
                        benchmark_UID = _profilerparams.CustomBase_UniqueId;
                        List<string> comp_list = (from r in _profilerparams.Comparison_UniqueIds where r != benchmark_UID select r).ToList();
                        comp_UID = string.Join("|", comp_list);
                        _profilerparams.Benchmark = benchmark_UID;
                        _profilerparams.Comparisonlist = comp_UID;
                    }
                    else
                    {
                        _profilerparams.Benchmark = _profilerparams.Comparison_UniqueIds[0].ToMyString();
                        _profilerparams.Comparison_UniqueIds.RemoveAt(0);
                        _profilerparams.Comparisonlist = string.Join("|", _profilerparams.Comparison_UniqueIds);
                    }

                    _profilerparams.TimePeriod = _profilerparams.TimePeriod_UniqueId;
                    _profilerparams.Filters = _profilerparams.ShopperSegment_UniqueId;
                    _profilerparams.ShopperFrequency = _profilerparams.ShopperFrequency_UniqueId;
                    _profilerparams.Metric = _profilerparams.SelectedMetricsIds.ToMyString();//_profilerparams.TabIndexId;

                    _profilerparams.SelectedStatTest = _profilerparams.SelectedStatTest;

                    _profilerparams.Sigtype_UniqueId = _profilerparams.Sigtype_UniqueId;
                    _profilerparams.Filtertypeid_UniqueId = _profilerparams.Filtertypeid_UniqueId;
                    _profilerparams.selectedMetrics = _profilerparams.SelectedMetricsIds.Split('|').ToList();
                    _profilerparams.TimePeriodFrom_UniqueId = _profilerparams.TimePeriodFrom_UniqueId;
                    _profilerparams.TimePeriodTo_UniqueId = _profilerparams.TimePeriodTo_UniqueId;
                }

                //paramvalues = new object[] { profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod_UniqueId, profilerparams.Filters, profilerparams.ShopperFrequency_UniqueId, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric, profilerparams.Sigtype_UniqueId };

                Session["ChartInputSelection"] = _profilerparams;
                //HData.StatPositive = Convert.ToString(Session["StatSessionPosi"]);
                //HData.StatNegative = Convert.ToString(Session["StatSessionNega"]);
                HData = chart.HighChartGenerator();

                var jsonResult = Json(HData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
            catch(Exception ex)
            {
                throw ex;
            }           
              
        }


        public JsonResult GenerateTable(ProfilerChartParams _profilerparams)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });
            //log selection
            LogEntries.LogSelection("CHARTS");

            string tbltext = string.Empty;
            string xmltext = string.Empty;
            List<string> BenchComp = new List<string>();
            try
            {
                ProfilerChartParams pcparams = null;
                int width = 918;
                int height = 313;

                string StatPositive = Convert.ToString(Session["StatSessionPosi"]).ToMyString();
                string StatNegative = Convert.ToString(Session["StatSessionNega"]).ToMyString();

                //_profilerparams.Benchmark = _profilerparams.Comparison_DBNames[0].ToMyString();
                _profilerparams.BCFullNames = _profilerparams.Comparison_ShortNames;
                _profilerparams.Comparison_DBNames.RemoveAt(0);
                //_profilerparams.Comparison_ShortNames.RemoveAt(0);
                _profilerparams.Comparisonlist = string.Join("|", _profilerparams.Comparison_DBNames);

                //_profilerparams.selectedMetrics = _profilerparams.SelectedMetrics.Split('|').ToList();
                _profilerparams.FrequencyTitle = _profilerparams.FrequencyTitle.ToMyString();
                //_profilerparams.ShopperFrequency = _profilerparams.ShopperFrequency.ToMyString();//"Store In Trade Area";
                //_profilerparams.ShopperFrequencyShortName = _profilerparams.ShopperFrequency.ToMyString();
                _profilerparams.ChartDataSet = GetData(_profilerparams);
                DataColumn[] stringColumns = _profilerparams.ChartDataSet.Tables[0].Columns.Cast<DataColumn>()
                                                .Where(c => c.DataType == typeof(string))
                                                .ToArray();

                foreach (DataRow row in _profilerparams.ChartDataSet.Tables[0].Rows)
                    foreach (DataColumn col in stringColumns)
                        row.SetField<string>(col, row.Field<string>(col).Trim());

                List<string> datacolumns = _profilerparams.ChartDataSet.Tables[0].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                List<string> colomns = new List<string>();
                foreach (string col in _profilerparams.BCFullNames)
                {
                    if (datacolumns.Contains(col, StringComparer.OrdinalIgnoreCase))
                        colomns.Add(col);
                }
                _profilerparams.BCFullNames = colomns;
                param = Chart.BindTabs(_profilerparams, out tbltext, out xmltext, "false", _profilerparams.ChartXValues.ToArray(), width, height);
                if ((_profilerparams.ActiveTab == "usp_profilerTrendRetailerTrip_TRENDCHANGE" || _profilerparams.ActiveTab == "usp_profilerTrendRetailerShopper_TRENDCHANGE") && _profilerparams.TrendType == "2")
                    Subparam = objCommonTable.BindTabsTrends(_profilerparams.Benchmark, _profilerparams.Comparisonlist.Split('|').ToArray(), _profilerparams.TimePeriod, _profilerparams.Filters.ToMyString(), _profilerparams.Filters.ToMyString(), _profilerparams.ShopperFrequency, _profilerparams.BCFullNames.ToArray(), StatPositive.ToMyString(), StatNegative.ToMyString(), _profilerparams.ShortTimePeriod, "381", "381", _profilerparams.SelectedStatTest, _profilerparams.CustomBase_ShortName, _profilerparams.ChartDataSet, "2");
                else
                    Subparam = objCommonTable.BindTabs(_profilerparams.Benchmark, _profilerparams.Comparisonlist.Split('|').ToArray(), _profilerparams.TimePeriod, _profilerparams.Filters.ToMyString(), _profilerparams.Filters.ToMyString(), _profilerparams.ShopperFrequency, _profilerparams.BCFullNames.ToArray(), StatPositive.ToMyString(), StatNegative.ToMyString(), _profilerparams.ShortTimePeriod, "381", "381", _profilerparams.SelectedStatTest, _profilerparams.CustomBase_ShortName, _profilerparams.ChartDataSet, "1");

                param.LeftBody = Subparam.LeftBody;
                param.RightBody = Subparam.RightBody;
                param.LeftHeader = Subparam.LeftHeader;
                param.RightHeader = Subparam.RightHeader;
                //TableContent.InnerHtml = tbltext;

            }

            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            var jsonResult = Json(param, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;

        }
        private DataSet GetData(ProfilerChartParams _profilerparams)
        {
            char[] PipeSeparator = new char[] { '|' };
            ProfilerChartParams profilerchartparams = null;
            DataSet ds = null;
            DataAccess dal = new DataAccess();
            //selectedMetrics = Convert.ToString(_profilerparams.SelectedMetrics.Replace("Top 10|", "")).Trim().TrimEnd().Split('|').ToList();
            if (_profilerparams.Tab_Id_mapping == "true")
                selectedMetrics = Convert.ToString(_profilerparams.SelectedMetricsIds.Replace("Top 10|", "")).Trim().TrimEnd().Split('|').ToList();
            else
                selectedMetrics = Convert.ToString(_profilerparams.SelectedMetrics.Replace("Top 10|", "")).Trim().TrimEnd().Split('|').ToList();
            try
            {
                ProfilerChartParams profilerparams = new ProfilerChartParams();
                profilerparams.ViewType = _profilerparams.ViewType;
                if (_profilerparams.ActiveTab == "usp_profilerWithinRetailerTrip")
                {
                    profilerparams.Benchmark = _profilerparams.Benchmark.ToMyString();
                    profilerparams.Comparisonlist = string.Join("|", _profilerparams.Comparisonlist);
                }
                else
                {
                    profilerparams.Benchmark = _profilerparams.Benchmark;
                    profilerparams.Comparisonlist = _profilerparams.Comparisonlist;
                }

                //profilerparams.Benchmark = _profilerparams.Benchmark;
                //profilerparams.Comparisonlist = _profilerparams.Comparisonlist;
                profilerparams.CustomBase_ShortName = _profilerparams.CustomBase_ShortName.ToMyString();
                profilerparams.CustomBase_UniqueId = _profilerparams.CustomBase_UniqueId.ToMyString();
                profilerparams.CustomBase_DBName = _profilerparams.CustomBase_DBName.ToMyString();

                profilerparams.TimePeriod = _profilerparams.TimePeriod;
                profilerparams.Filters = _profilerparams.Filters.ToMyString();
                profilerparams.ShopperFrequency = _profilerparams.ShopperFrequency.ToMyString();
                profilerparams.Metric = _profilerparams.Metric;
                profilerparams.ChartType = _profilerparams.ChartType;
                profilerparams.ActiveTab = _profilerparams.ActiveTab;
                profilerparams.FilterShortNames = _profilerparams.FilterShortNames;
                profilerparams.ShopperSegment = _profilerparams.ShopperSegment;
                profilerparams.View = _profilerparams.View;

                profilerparams.FrequencyTitle = _profilerparams.FrequencyTitle;
                profilerparams.ShortTimePeriod = _profilerparams.ShortTimePeriod;
                profilerparams.ShopperFrequencyShortName = _profilerparams.ShopperFrequencyShortName;
                profilerparams.MetricShortName = _profilerparams.MetricShortName;
                profilerparams.selectedMetrics = selectedMetrics;
                profilerparams.ChartWidth = _profilerparams.ChartWidth;
                profilerparams.ChartHeight = _profilerparams.ChartHeight;
                profilerparams.ModuleBlock = _profilerparams.ModuleBlock;
                profilerparams.SelectedStatTest = _profilerparams.SelectedStatTest;
                profilerparams.Comparison_DBNames = _profilerparams.Comparison_DBNames;

                profilerparams.View = _profilerparams.View;
                profilerparams.CompetitorFrequency_UniqueId = _profilerparams.CompetitorFrequency_UniqueId.ToMyString();
                profilerparams.CompetitorFrequency_Name = _profilerparams.CompetitorFrequency_Name.ToMyString();

                profilerparams.CompetitorRetailer_UniqueId = _profilerparams.CompetitorRetailer_UniqueId.ToMyString();
                profilerparams.CompetitorRetailer_Name = _profilerparams.CompetitorRetailer_Name.ToMyString();

                if (_profilerparams.Tab_Id_mapping == "true")
                {

                    profilerparams.Benchmark = _profilerparams.Comparison_UniqueIds[0].ToMyString();
                    _profilerparams.Comparison_UniqueIds.RemoveAt(0);
                    profilerparams.Comparisonlist = string.Join("|", _profilerparams.Comparison_UniqueIds);

                    profilerparams.TimePeriod = _profilerparams.TimePeriod_UniqueId;
                    profilerparams.Filters = _profilerparams.ShopperSegment_UniqueId;
                    profilerparams.ShopperFrequency = _profilerparams.ShopperFrequency_UniqueId;
                    profilerparams.Metric = _profilerparams.SelectedMetricsIds.ToMyString();//_profilerparams.TabIndexId;
                    profilerparams.SelectedStatTest = _profilerparams.Sigtype_UniqueId;
                    profilerparams.Sigtype_UniqueId = _profilerparams.Sigtype_UniqueId;
                    profilerparams.Filtertypeid_UniqueId = _profilerparams.Filtertypeid_UniqueId;
                    profilerparams.selectedMetrics = _profilerparams.SelectedMetricsIds.Split('|').ToList();

                    profilerparams.Filtertypeid_UniqueId = _profilerparams.Filtertypeid_UniqueId;

                    profilerparams.Comparison_UniqueIds = _profilerparams.Comparison_UniqueIds;
                    profilerparams.SingleSelection = _profilerparams.SingleSelection;

                    profilerparams.TimePeriodFrom_UniqueId = _profilerparams.TimePeriodFrom_UniqueId;
                    profilerparams.TimePeriodTo_UniqueId = _profilerparams.TimePeriodTo_UniqueId;
                    profilerparams.beverageSelectionType_UniqueId = _profilerparams.beverageSelectionType_UniqueId;

                }

                object[] paramvalues = null;
                if (_profilerparams.Tab_Id_mapping == "true")
                {
                    if (Convert.ToString(_profilerparams.ModuleBlock).IndexOf("TREND") == -1)
                    {
                        if (profilerparams.SelectedStatTest == "1")
                        {
                            List<string> comp = (profilerparams.Benchmark + "|" + profilerparams.Comparisonlist).Split('|').ToList();
                            comp = (from r in comp where r != _profilerparams.CustomBase_UniqueId select r).Distinct().ToList();
                            profilerparams.Comparisonlist = String.Join("|", comp);
                            profilerparams.Benchmark = _profilerparams.CustomBase_UniqueId;

                        }
                    }


                    if (Convert.ToString(profilerparams.ModuleBlock).Contains("PIT"))
                    {
                        if (profilerparams.ActiveTab.ToLower().IndexOf("retailer") > -1)
                            paramvalues = new object[] { null, null, profilerparams.SingleSelection, profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.SelectedStatTest, profilerparams.CompetitorFrequency_UniqueId.ToMyString(), profilerparams.CompetitorRetailer_UniqueId.ToMyString() };
                        else
                            paramvalues = new object[] { null, null, profilerparams.SingleSelection, profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.SelectedStatTest };
                    }
                    else if (Convert.ToString(profilerparams.ModuleBlock).Contains("TREND"))
                    {
                        if (_profilerparams.TrendType == "1")
                        {
                            if (profilerparams.ActiveTab.ToLower().IndexOf("retailer") > -1)
                                paramvalues = new object[] { "", profilerparams.beverageSelectionType_UniqueId.ToMyString(), profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString(), profilerparams.TimePeriodFrom_UniqueId.ToMyString(), profilerparams.TimePeriodTo_UniqueId.ToMyString(), profilerparams.Filters.ToMyString(), profilerparams.ShopperFrequency.ToMyString(), profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<").ToMyString() : profilerparams.Metric.Replace("&lt;", "<").ToMyString(), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CustomBase_UniqueId, profilerparams.CompetitorFrequency_UniqueId.ToMyString(), profilerparams.CompetitorRetailer_UniqueId.ToMyString() };
                            else
                                paramvalues = new object[] { "", profilerparams.beverageSelectionType_UniqueId.ToMyString(), profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", "").ToMyString(), profilerparams.TimePeriodFrom_UniqueId.ToMyString(), profilerparams.TimePeriodTo_UniqueId.ToMyString(), profilerparams.Filters.ToMyString(), profilerparams.ShopperFrequency.ToMyString(), profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<").ToMyString() : profilerparams.Metric.Replace("&lt;", "<").ToMyString(), profilerparams.Sigtype_UniqueId.ToMyString(), profilerparams.CustomBase_UniqueId };
                        }
                        else if (_profilerparams.TrendType == "2")
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
                            paramvalues = new object[] { null, null, profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.SelectedStatTest, profilerparams.CompetitorFrequency_UniqueId.ToMyString(), profilerparams.CompetitorRetailer_UniqueId.ToMyString() };
                        else
                            paramvalues = new object[] { null, null, profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric.Replace("&lt;", "<"), profilerparams.SelectedStatTest };
                    }

                    //if (profilerchartparams.IsSelectionChange || HttpContext.Current.Session["HighChartDataSet"] == null)
                    if (Convert.ToString(profilerparams.ActiveTab).ToLower().IndexOf("ecom") > -1)
                    {
                        List<object> dbparam = paramvalues.ToList();
                        dbparam.Add(_profilerparams.Add_ShopperFrequency_UniqueId.ToMyString());
                        paramvalues = dbparam.ToArray();
                    }
                    ds = dal.GetData_WithIdMapping(paramvalues, profilerparams.ActiveTab);
                    //else
                    profilerparams.SelectedMetrics = Convert.ToString(_profilerparams.SelectedMetrics.Replace("Top 10|", "")).Trim().TrimEnd();
                    if (profilerparams.SelectedMetrics.ToUpper().Split('|').ToList()[0].Trim() == "TOP 10")
                    {
                        List<string> str = (from r in ds.Tables[1].AsEnumerable() orderby r.Field<string>("MetricItem") select r.Field<string>("MetricItem")).Distinct().ToList();
                        profilerparams.SelectedMetrics = string.Join("|", str.ToArray());
                        profilerparams.Top_10 = true;
                    }

                    //ds = HttpContext.Current.Session["HighChartDataSet"] as DataSet;
                    profilerparams.Benchmark = _profilerparams.Benchmark.ToMyString();
                    profilerparams.Comparisonlist = string.Join("|", _profilerparams.Comparisonlist);
                    profilerparams.TimePeriod = _profilerparams.TimePeriod;
                    profilerparams.Filters = _profilerparams.Filters.ToMyString();
                    profilerparams.ShopperFrequency = _profilerparams.ShopperFrequency.ToMyString();
                    profilerparams.Metric = _profilerparams.Metric;
                    profilerparams.ChartType = _profilerparams.ChartType;
                    profilerparams.ActiveTab = _profilerparams.ActiveTab;
                    profilerparams.FilterShortNames = _profilerparams.FilterShortNames;
                    profilerparams.ShopperSegment = _profilerparams.ShopperSegment;


                    profilerparams.FrequencyTitle = _profilerparams.FrequencyTitle;
                    profilerparams.ShortTimePeriod = _profilerparams.ShortTimePeriod;
                    profilerparams.ShopperFrequencyShortName = _profilerparams.ShopperFrequencyShortName;
                    profilerparams.MetricShortName = _profilerparams.MetricShortName;
                    profilerparams.selectedMetrics = _profilerparams.SelectedMetrics.Replace("&lt;", "<").Replace("`", "'").Split('|').ToList();
                    profilerparams.ChartWidth = _profilerparams.ChartWidth;
                    profilerparams.ChartHeight = _profilerparams.ChartHeight;
                    profilerparams.ModuleBlock = _profilerparams.ModuleBlock;
                    profilerparams.SelectedStatTest = _profilerparams.SelectedStatTest;
                    selectedMetrics = _profilerparams.SelectedMetrics.Replace("&lt;", "<").Replace("`", "'").ToLower().Split('|').ToList();
                }
                else
                {
                    if (Convert.ToString(_profilerparams.ModuleBlock).IndexOf("TREND") == -1)
                    {
                        if (profilerparams.SelectedStatTest.Trim().ToLower() == "custom base")
                        {
                            List<string> comp = new List<string>();
                            comp.Add(profilerparams.Benchmark);
                            comp.AddRange(profilerparams.Comparison_DBNames);

                            comp = (from r in comp where r != _profilerparams.CustomBase_DBName select r).Distinct().ToList();
                            profilerparams.Comparisonlist = String.Join("|", comp);
                            profilerparams.Benchmark = _profilerparams.CustomBase_DBName;
                        }
                    }
                    if (Convert.ToString(_profilerparams.ModuleBlock).Contains("PIT"))
                        paramvalues = new object[] { profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.ShopperSegment.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.ShopperFrequency, profilerparams.Filters, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric, profilerparams.SelectedStatTest };

                    else if (Convert.ToString(_profilerparams.ModuleBlock).Contains("TREND"))
                        paramvalues = new object[] { profilerparams.Benchmark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.Comparisonlist.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), profilerparams.ShopperSegment, profilerparams.ShopperFrequency, profilerparams.Filters, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric, profilerparams.SelectedStatTest };

                    else
                        paramvalues = new object[] { profilerparams.Benchmark, profilerparams.Comparisonlist, profilerparams.TimePeriod, profilerparams.Filters, profilerparams.ShopperFrequency, profilerparams.Metric.IndexOf("Favourite") > -1 ? profilerparams.Metric.Replace("&lt;", "<") : profilerparams.Metric, profilerparams.SelectedStatTest };

                    //if (profilerchartparams.IsSelectionChange || HttpContext.Current.Session["HighChartDataSet"] == null)
                    ds = dal.GetData(paramvalues, profilerparams.ActiveTab);

                    DataColumn[] stringColumns = ds.Tables[1].Columns.Cast<DataColumn>()
                                                .Where(c => c.DataType == typeof(string))
                                                .ToArray();

                    foreach (DataRow row in ds.Tables[1].Rows)
                        foreach (DataColumn col in stringColumns)
                            row.SetField<string>(col, row.Field<string>(col).Trim());
                    //else
                    //    ds = HttpContext.Current.Session["HighChartDataSet"] as DataSet;
                }
                System.Web.HttpContext.Current.Session["HighChartDataSet"] = ds.Copy();

                if (!string.IsNullOrEmpty(profilerparams.Benchmark))
                {
                    if (Convert.ToString(_profilerparams.ModuleBlock).Contains("Time"))
                    {
                        //Nagaraju 25-06-14
                        string timeperiod = string.Empty;
                        if (!string.IsNullOrEmpty(_profilerparams.Benchmark))
                        {
                            string[] timeperiodlist = _profilerparams.Benchmark.Split('|');
                            if (timeperiodlist != null && timeperiodlist.Count() > 0)
                            {
                                timeperiod = " " + timeperiodlist[0];
                            }
                        }
                        var query2 = from r in ds.Tables[1].AsEnumerable() select r.Field<string>("Objective");
                        BCFullNames = query2.Distinct().ToList();

                        var query = from r in ds.Tables[1].AsEnumerable() select r.Field<string>("Objective") + timeperiod;
                        ChartXValues = query.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                        //End
                    }

                    else
                    {
                        if (ds != null && ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                        {
                            List<string> inputCRlist = new List<string>();
                            // inputCRlist.RemoveAt(0);
                            ChartXValues = new List<string>();
                            var query = from r in ds.Tables[1].AsEnumerable()
                                        select r.Field<string>("Objective").Trim();
                            List<string> tblCRlist = query.Distinct().ToList();
                            inputCRlist = tblCRlist;
                            foreach (string cr in inputCRlist)
                            {
                                if (tblCRlist.Contains(cr.Trim(), StringComparer.CurrentCultureIgnoreCase))
                                {
                                    BCFullNames.Add(cr.Trim());
                                    ChartXValues.Add(Get_ShortNames(cr.Trim()));
                                }
                            }
                        }
                    }
                }

                if (ds != null && ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                {
                    if (_profilerparams.TrendType == "2")
                        ds = FilterDataTable(_profilerparams, ds, _profilerparams.ComparisonNames, "MetricItem");
                    else
                        ds = FilterDataTable(_profilerparams, ds, selectedMetrics, "MetricItem");
                    //if (ChartXValues.Contains("Total") && _profilerparams.View.IndexOf("Within") == -1)
                    //{
                    //    int index = 0;
                    //    index = ChartXValues.IndexOf("Total");
                    //    if (index >= 0)
                    //    {
                    //        ChartXValues.RemoveAt(index);
                    //        ChartXValues.Insert(index, Get_ShortNames(_profilerparams.View));
                    //    }

                    //}
                    if (profilerparams.Top_10)//(_profilerparams.SelectedMetrics.IndexOf("Top 10") > -1)
                    {
                        ds = MakeTopTenTable(ds);
                    }
                }

                if (ChartXValues != null && ChartXValues.Count > 0)
                {
                    ChartXValues = ChartXValues.OrderBy(x => _profilerparams.ChartXValues.IndexOf(x)).ToList();
                }
                profilerparams.ChartXValues = ChartXValues;
                profilerparams.BCFullNames = BCFullNames;
                if (_profilerparams.TrendType == "2")
                    ds = CreateTablesTrend(ds);
                else
                    ds = CreateTables(ds);

                profilerparams.ChartDataSet = ds;
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    profilerparams.Metric = Get_ShortNames(profilerparams.Metric);
                    Session["ProfilerTableData"] = profilerparams;
                }
                else
                {
                    Session["ProfilerTableData"] = null;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }
        public DataSet FilterDataTable(ProfilerChartParams profilerchartparams, DataSet ds, List<string> metriclist, string spColName)
        {
            DataSet dsRet = new DataSet();
            //Nagaraju 25-06-14
            string timeperiod = string.Empty;
            if (Convert.ToString(profilerchartparams.ModuleBlock).Contains("Time"))
            {
                if (!string.IsNullOrEmpty(profilerchartparams.Benchmark))
                {
                    string[] timeperiodlist = profilerchartparams.Benchmark.Split('|');
                    if (timeperiodlist != null && timeperiodlist.Count() > 0)
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
                                where Convert.ToString(r.Field<string>("Objective")).ToLower().Trim() == obj.ToLower()
                                select r;
                    List<DataRow> Rows = query.Distinct().ToList();
                    foreach (DataRow row in Rows)
                    {
                        //if (metriclist.Contains(Get_ShortNames(row[spColName].ToString().Trim()), StringComparer.CurrentCultureIgnoreCase) &&
                        //    !Convert.ToString(row[spColName]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                        //    && !Convert.ToString(row["MetricItem"]).Equals("Number of Trips", StringComparison.OrdinalIgnoreCase)) //&&
                        if ((metriclist.Contains(row[spColName].ToString().ToLower().Trim(), StringComparer.CurrentCultureIgnoreCase) || metriclist.Contains("Top 10", StringComparer.CurrentCultureIgnoreCase)) &&
                          !Convert.ToString(row[spColName]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                          && !Convert.ToString(row["MetricItem"]).Equals("Number of Trips", StringComparison.OrdinalIgnoreCase))
                        //!Convert.ToString(row["MetricItem"]).ToUpper().Contains("SAMPLESIZE"))
                        {
                            if (System.DBNull.Value != row["Volume"] && Convert.ToDouble(row["Volume"]) > 0)
                            {
                                row["Volume"] = Convert.ToDouble(row["Volume"]);
                            }
                            else
                            {
                                row["Volume"] = 0; // Change this to dbnull later
                            }
                            //if (Convert.ToString(row["Objective"]).ToLower() == "total" && profilerchartparams.View.IndexOf("Within") == -1)
                            //{
                            //    row["Objective"] = Get_ShortNames(profilerchartparams.View);
                            //}
                            //else
                            //{
                            //    row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"])) + timeperiod;
                            //}
                            row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"]).Trim()) + timeperiod;

                            row["Metric"] = Get_ShortNames(Convert.ToString(profilerchartparams.MetricShortName));
                            //row["Metric"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Get_ShortNames(Convert.ToString(profilerchartparams.MetricShortName)).ToLower());
                            row["MetricItem"] = Get_ShortNames(Convert.ToString(row["MetricItem"])).Replace("&lt;", "<");
                            outputTable.ImportRow(row);
                        }
                        //Sample Size
                        else if (Convert.ToString(row[spColName]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                            || Convert.ToString(row["MetricItem"]).ToUpper().Contains("SAMPLESIZE") || Convert.ToString(row["MetricItem"]).ToUpper().Contains("SAMPLE SIZE") ||
                            Convert.ToString(row["MetricItem"]).Equals("Number of Trips", StringComparison.OrdinalIgnoreCase)
                            || Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase))
                        {
                            //if (Convert.ToString(row["Objective"]).ToLower() == "total" && profilerchartparams.View.IndexOf("Within") == -1)
                            //{
                            //    row["Objective"] = Get_ShortNames(profilerchartparams.View);
                            //}
                            //else
                            //{
                            //    row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"])) + timeperiod;
                            //}
                            row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"]).Trim()) + timeperiod;
                            row["Metric"] = Get_ShortNames(Convert.ToString(profilerchartparams.MetricShortName));
                            //row["Metric"] = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Get_ShortNames(Convert.ToString(profilerchartparams.MetricShortName)).ToLower());
                            row["MetricItem"] = Get_ShortNames(Convert.ToString(row["MetricItem"])).Replace("&lt;", "<");
                            outputTable.ImportRow(row);
                        }
                        else
                        {
                            outputTable.ImportRow(row);
                        }
                    }
                }

                dsRet.Tables.Add(outputTable);
            }

            return dsRet;
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

        public DataSet MakeTopTenTable(DataSet ds)
        {
            DataTable outputTable = ds.Tables[1].Clone();


            var query = (from r in ds.Tables[1].AsEnumerable()
                         where Convert.ToString(r["Objective"]) == ChartXValues[0]
                         && !string.IsNullOrEmpty(Convert.ToString(r["Volume"]))
                         orderby (r["Volume"]) descending
                         select Convert.ToString(r["MetricItem"])).Take(10);

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
        private DataSet CreateTables(DataSet ds)
        {
            DataSet das = new DataSet();
            DataTable tbl = new DataTable();
            List<string> Benchmark_Comparisonlist = null;
            if (ds != null && ds.Tables.Count >= 2)
            {
                #region Create Columns
                tbl.Columns.Add("Metric", typeof(string));
                tbl.Columns.Add("MetricItem", typeof(string));
                #region Add Benchmark and Comparison Columns
                var query = from r in ds.Tables[0].AsEnumerable()
                            select r.Field<string>("Objective");
                Benchmark_Comparisonlist = query.Distinct().ToList();
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    tbl.Columns.Add(bc, typeof(double));
                }
                #endregion

                #endregion

                #region Create Rows
                #region Add Sample Size
                List<object> Metricrowitems = new List<object>();
                List<object> Significancerowitems = new List<object>();
                Metricrowitems.Add(ds.Tables[0].Rows[0]["Metric"]);
                Metricrowitems.Add("Sample Size");
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    var query2 = (from r in ds.Tables[0].AsEnumerable()
                                 where r.Field<string>("Objective") == bc
                                 && !Convert.ToString(r.Field<string>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                  select r).Distinct();
                    List<DataRow> Rows = query2.ToList();
                    foreach (DataRow row in Rows)
                    {
                        Metricrowitems.Add(row["Volume"]);
                    }
                }
                tbl.Rows.Add(Metricrowitems.ToArray());

                var nofres = from r in ds.Tables[0].AsEnumerable()
                             where Convert.ToString(r.Field<string>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                             select r;
                if (nofres != null && nofres.Count() > 0)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(ds.Tables[0].Rows[0]["Metric"]);
                    Metricrowitems.Add("Number of Responses");
                    foreach (string bc in Benchmark_Comparisonlist)
                    {
                        var query2 = (from r in ds.Tables[0].AsEnumerable()
                                     where r.Field<string>("Objective") == bc
                                     && Convert.ToString(r.Field<string>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                     select r).Distinct();
                        List<DataRow> Rows = query2.ToList();
                        foreach (DataRow row in Rows)
                        {
                            Metricrowitems.Add(row["Volume"]);
                        }
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                }
                #endregion

                #region Add Metric Items
                var query4 = from r in ds.Tables[1].AsEnumerable()
                             select r.Field<string>("MetricItem");
                List<string> MetricItems = query4.Distinct().ToList();
                foreach (string metric in MetricItems)
                {
                    //foreach (string bc in Benchmark_Comparisonlist)
                    //{
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(ds.Tables[1].Rows[0]["Metric"]);
                    Metricrowitems.Add(metric);

                    Significancerowitems = new List<object>();
                    Significancerowitems.Add(ds.Tables[1].Rows[0]["Metric"]);
                    Significancerowitems.Add(metric + "Significance");
                    var query3 = (from r in ds.Tables[1].AsEnumerable()
                                 where r.Field<string>("MetricItem") == metric
                                 //where r.Field<string>("Objective") == bc
                                 select r).Distinct();
                    List<DataRow> Rows = query3.ToList();
                    foreach (DataRow row in Rows)
                    {
                        Metricrowitems.Add(row["Volume"]);
                        Significancerowitems.Add(row["Significance"]);
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                    tbl.Rows.Add(Significancerowitems.ToArray());
                    // }
                }

                #endregion
                #endregion
                das.Tables.Add(tbl);
            }
            return das;
        }

        private DataSet CreateTablesTrend(DataSet ds)
        {
            DataSet das = new DataSet();
            DataTable tbl = new DataTable();
            List<string> Benchmark_Comparisonlist = null;
            if (ds != null && ds.Tables.Count >= 2)
            {
                #region Create Columns
                tbl.Columns.Add("Metric", typeof(string));
                tbl.Columns.Add("MetricItem", typeof(string));
                #region Add Benchmark and Comparison Columns
                var query = from r in ds.Tables[0].AsEnumerable()
                            select r.Field<string>("Objective");
                Benchmark_Comparisonlist = query.Distinct().ToList();
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    tbl.Columns.Add(bc, typeof(double));
                }
                tbl.Columns.Add("SortOrder", typeof(int));
                #endregion

                #endregion

                #region Create Rows
                #region Add Sample Size
                List<object> Metricrowitems = new List<object>();
                List<object> Significancerowitems = new List<object>();

                var query44 = from r in ds.Tables[1].AsEnumerable()
                              select r.Field<string>("MetricItem");
                List<string> MetricList = query44.Distinct().ToList();
                var i = 0;
                foreach (string metric in MetricList)
                {
                    Metricrowitems = new List<object>();

                    Metricrowitems.Add("RETAILER");
                    Metricrowitems.Add("Sample Size");
                    foreach (string bc in Benchmark_Comparisonlist)
                    {

                        var query2 = (from r in ds.Tables[0].AsEnumerable()
                                     where r.Field<string>("Objective") == bc
                                     && r.Field<string>("Retailer") == metric
                                     && !Convert.ToString(r.Field<string>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                      select r).Distinct();
                        List<DataRow> Rows = query2.ToList();
                        foreach (DataRow row in Rows)
                        {
                            Metricrowitems.Add(row["Volume"]);
                        }

                    }
                    Metricrowitems.Add(Convert.ToInt16(i));
                    tbl.Rows.Add(Metricrowitems.ToArray());
                    i = i + 2;
                }


                var nofres = from r in ds.Tables[0].AsEnumerable()
                             where Convert.ToString(r.Field<string>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                             select r;
                //if (nofres != null && nofres.Count() > 0)
                //{
                //    Metricrowitems = new List<object>();


                //    foreach (string metric in MetricList)
                //    {
                //        Metricrowitems = new List<object>();
                //        Metricrowitems.Add("Number of Responses");
                //        Metricrowitems.Add(metric.ToString());
                //        foreach (string bc in Benchmark_Comparisonlist)
                //        {
                //            var query2 = from r in ds.Tables[0].AsEnumerable()
                //                         where r.Field<string>("Objective") == bc
                //                         && r.Field<string>("Retailer") == metric
                //                         && Convert.ToString(r.Field<string>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                //                         select r;
                //            List<DataRow> Rows = query2.ToList();
                //            foreach (DataRow row in Rows)
                //            {
                //                Metricrowitems.Add(row["Volume"]);
                //            }
                //        }
                //        tbl.Rows.Add(Metricrowitems.ToArray());
                //    }

                //}
                #endregion

                #region Add Metric Items
                var query4 = from r in ds.Tables[1].AsEnumerable()
                             select r.Field<string>("MetricItem");
                List<string> MetricItems = query4.Distinct().ToList();
                i = 1;
                foreach (string metric in MetricItems)
                {
                    //foreach (string bc in Benchmark_Comparisonlist)
                    //{
                    Metricrowitems = new List<object>();
                    //Metricrowitems.Add(ds.Tables[1].Rows[0]["Metric"]);
                    Metricrowitems.Add("RETAILER");
                    Metricrowitems.Add(metric);

                    Significancerowitems = new List<object>();
                    //Significancerowitems.Add(ds.Tables[1].Rows[0]["Metric"]);
                    Significancerowitems.Add("RETAILER");
                    Significancerowitems.Add(metric + "Significance");
                    var query3 = (from r in ds.Tables[1].AsEnumerable()
                                 where r.Field<string>("MetricItem") == metric
                                 //where r.Field<string>("Objective") == bc
                                  select r).Distinct();
                    List<DataRow> Rows = query3.ToList();
                    foreach (DataRow row in Rows)
                    {
                        Metricrowitems.Add(row["Volume"]);
                        Significancerowitems.Add(row["Significance"]);

                    }
                    Metricrowitems.Add(Convert.ToInt16(i));
                    Significancerowitems.Add(Convert.ToInt16(i));
                    tbl.Rows.Add(Metricrowitems.ToArray());
                    tbl.Rows.Add(Significancerowitems.ToArray());
                    i = i + 2;
                    //}
                }

                #endregion
                #endregion
                tbl.DefaultView.Sort = "[SortOrder] ASC";
                DataView dv = tbl.DefaultView;
                dv.Sort = "[SortOrder] ASC";
                tbl = dv.ToTable();
                das.Tables.Add(tbl);
            }
            return das;
        }
        #endregion
        public JsonResult AddChartToExport(string ChartType)
        {
            if (ModelState.IsValid)
            {
                string status = chart.AddChartToExport(ChartType);
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });
            }
        }

        public JsonResult ShowExportChartList()
        {
           if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                string status = chart.ShowExportChartList();
                return Json(status, JsonRequestBehavior.AllowGet);           
        }

        public JsonResult DeleteChartFromExportList(string ChartID)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                string status = chart.DeleteChartFromExportList(ChartID);
                return Json(status, JsonRequestBehavior.AllowGet);           
        }
        public JsonResult CheckExportChartList()
        {
           if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                bool status = chart.CheckExportChartList();
                return Json(status, JsonRequestBehavior.AllowGet);           
        }
        public JsonResult CheckChartReports()
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                bool status = chart.CheckChartReports();
                return Json(status, JsonRequestBehavior.AllowGet);           
        }
        public JsonResult ClearAllChartsFromExportList()
        {
             if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                string status = chart.ClearAllChartsFromExportList();
                return Json(status, JsonRequestBehavior.AllowGet);           
        }
        public void DownloadTable(ProfilerChartParams profilerchartparams)
        {
            try
            {
                string sheetname = string.Empty;
                Dictionary<string, ProfilerChartParams> chartlist = System.Web.HttpContext.Current.Session["ProfilerTableExportList"] as Dictionary<string, ProfilerChartParams>;
                Dictionary<string, string> exportfiles = new Dictionary<string, string>();
                Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
                List<string> ExportSheetNames = new List<string>();
                List<string> DuplicateSheetNames = new List<string>();
                int sheerNo = 1;
                ProfilerChartParams pparams = null;
                pparams = System.Web.HttpContext.Current.Session["ProfilerTableData"] as ProfilerChartParams;
                if ((System.Web.HttpContext.Current.Session["ProfilerTableExportList"] == null && chartlist == null) || chartlist.Count == 0)
                {
                    chartlist = new Dictionary<string, ProfilerChartParams>();
                    chartlist.Add("Chart_0", pparams);
                }
                foreach (ProfilerChartParams tableparams in chartlist.Values)
                {
                    string tbltext = string.Empty;
                    string xmlstring = string.Empty;
                    Chart.BindTabs(tableparams, out tbltext, out xmlstring, "true", tableparams.ChartXValues.ToArray(), pparams.ChartWidth, pparams.ChartHeight);
                    exportfiles.Add("tab" + sheerNo, xmlstring);
                    String ret = Regex.Replace(tableparams.Metric.Trim(), "[^A-Za-z0-9_. ]+", "");
                    if (!ExportSheetNames.Contains(ret.Replace(" ", String.Empty)))
                    {
                        sheetname = ret.Replace(" ", String.Empty);
                        if (sheetname.Length > 20)
                            sheetname = sheetname.Substring(0, 20);

                        ExportSheetNames.Add(sheetname);
                    }
                    else
                    {
                        DuplicateSheetNames.Add(ret.Replace(" ", String.Empty));
                        var query = from metric in DuplicateSheetNames
                                    where metric == tableparams.Metric
                                    select metric.ToList();

                        sheetname = (tableparams.Metric + "(" + query.Count() + ")").Replace(" ", String.Empty);
                        if (sheetname.Length > 20)
                            sheetname = sheetname.Substring(0, 20);

                        ExportSheetNames.Add(sheetname);
                    }
                    sheerNo += 1;
                }
                List<string> sheetlist = ExportSheetNames.ToList();
                System.Web.HttpContext.Current.Session["TableExportfiles"] = exportfiles;
                System.Web.HttpContext.Current.Session["TableExportSheetNames"] = sheetlist;
            }
            catch (Exception ex)
            {

            }
        }

        public void DownloadChartsExcel()
        {
            DownloadExcel download = new Models.DownloadExcel();
            download.DownloadChartsReport();
        }
    }
}
