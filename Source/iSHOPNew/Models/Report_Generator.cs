using AQ.Common.GenerateReport;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace iSHOPNew.Models
{
    public class Report_Generator
    {
        CommonFunctions _commonfunctions = new CommonFunctions();
        string strSelection = string.Empty;
        List<string> lowsamplesize = new List<string>();     
        public Dictionary<string, DataSet> BuildSlides(ReportGeneratorParams reportparams, out List<string> ShopperLowSampleSizelist, 
            out List<string> TripsLowSampleSizelist, out List<string> SelectedReports, string Selected_StatTest, 
            out bool GenerateReport, out bool IsUseDirectionally, out List<LowSampleSizeItems> lowSampleSizeItems,
            out List<UseDirectionallyItems> useDirectionallyItems, out bool IsAllLowSampleSizes)
        {
            GenerateReport = true;
            IsUseDirectionally = false;
            IsAllLowSampleSizes = false;
            string _report = Convert.ToString(reportparams.ModuleBlock).ToMyString();
            List<SlideDetails> slidelist = new List<SlideDetails>();
            Dictionary<string, DataSet> dsl = new Dictionary<string, DataSet>();
            DataSet ds = null;
            object[] paramvalues = null;          
            ShopperLowSampleSizelist = new List<string>();
            TripsLowSampleSizelist = new List<string>();
            lowSampleSizeItems = new List<LowSampleSizeItems>();
            useDirectionallyItems = new List<UseDirectionallyItems>();
            if (reportparams.ModuleBlock.Equals("AcrossBeverageShopper", StringComparison.OrdinalIgnoreCase) || reportparams.ModuleBlock.Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase)
                || reportparams.ModuleBlock.Equals("WithinBeverageShopper", StringComparison.OrdinalIgnoreCase) || reportparams.ModuleBlock.Equals("WithinBeverageTrips", StringComparison.OrdinalIgnoreCase)
                || reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase) || reportparams.ModuleBlock.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
            {
                reportparams.SelectedReports = new List<string>();
                reportparams.SelectedReports.Add(reportparams.ModuleBlock);
            }
            SelectedReports = reportparams.SelectedReports;
            string benchmark_UniqueId = string.Empty; 
            try
            {
                #region id mappind
                if (reportparams.Tab_Id_mapping)
                {
                    if(_report== "WithinTrips")
                    {
                        paramvalues = new object[] { reportparams.TimePeriod_UniqueId, reportparams.ShopperSegment_UniqueId.ToMyString(), reportparams.CustomBase_UniqueId, String.Join("|", reportparams.Comparison_UniqueIds), reportparams.Filter_UniqueId, reportparams.ShopperFrequency_UniqueId, reportparams.Sigtype_UniqueId, reportparams.SampleSizeCheck };
                    }
                    else if (_report == "WithinShopper")
                    {                       
                        paramvalues = new object[] { reportparams.TimePeriod_UniqueId, reportparams.ShopperSegment_UniqueId.ToMyString(), reportparams.CustomBase_UniqueId, String.Join("|", reportparams.Comparison_UniqueIds), reportparams.Filter_UniqueId, reportparams.ShopperFrequency_UniqueId, reportparams.Sigtype_UniqueId, reportparams.SampleSizeCheck };
                    }
                    else if (_report== "TimeTrips")
                    {
                        paramvalues = new object[] { reportparams.TimePeriodFrom_UniqueId, reportparams.TimePeriodTo_UniqueId, reportparams.ShopperSegment_UniqueId, reportparams.Filter_UniqueId.ToMyString(), reportparams.ShopperFrequency_UniqueId, reportparams.Sigtype_UniqueId, reportparams.CustomBase_UniqueId, reportparams.SampleSizeCheck };
                    }
                    else if (_report == "TimeShopper")
                    {
                        paramvalues = new object[] { reportparams.TimePeriodFrom_UniqueId, reportparams.TimePeriodTo_UniqueId, reportparams.ShopperSegment_UniqueId, reportparams.Filter_UniqueId.ToMyString(), reportparams.ShopperFrequency_UniqueId, reportparams.Sigtype_UniqueId, reportparams.CustomBase_UniqueId, reportparams.SampleSizeCheck };
                    }
                    else if (_report == "AcrossBeverageShopper")
                    {
                        paramvalues = new object[] { reportparams.TimePeriod_UniqueId.ToMyString(), String.Join("|", reportparams.Comparison_UniqueIds).ToMyString(), reportparams.Filter_UniqueId.ToMyString(), reportparams.SampleSizeCheck };
                    }
                    else if (_report == "AcrossBeverageTrips")
                    {
                        paramvalues = new object[] { reportparams.TimePeriod_UniqueId.ToMyString(), String.Join("|", reportparams.Comparison_UniqueIds).ToMyString(), reportparams.Filter_UniqueId.ToMyString(), reportparams.ShopperFrequency_UniqueId.ToMyString(), reportparams.SampleSizeCheck };
                    }
                    else if (_report == "WithinBeverageShopper")
                    {
                        benchmark_UniqueId = reportparams.Comparison_UniqueIds[0];
                        reportparams.Comparison_UniqueIds.RemoveAt(0);
                        paramvalues = new object[] { reportparams.ShopperSegment_UniqueId.ToMyString(), reportparams.TimePeriod_UniqueId.ToMyString(), benchmark_UniqueId, String.Join("|", reportparams.Comparison_UniqueIds).ToMyString(), reportparams.Filter_UniqueId.ToMyString(), reportparams.SampleSizeCheck };
                    }
                    else if (_report == "WithinBeverageTrips")
                    {
                        benchmark_UniqueId = reportparams.Comparison_UniqueIds[0];
                        reportparams.Comparison_UniqueIds.RemoveAt(0);
                        paramvalues = new object[] { reportparams.TimePeriod_UniqueId.ToMyString(), reportparams.ShopperSegment_UniqueId.ToMyString(), benchmark_UniqueId, String.Join("|", reportparams.Comparison_UniqueIds).ToMyString(), reportparams.Filter_UniqueId.ToMyString(), reportparams.ShopperFrequency_UniqueId.ToMyString(), reportparams.SampleSizeCheck };
                    }
                    else if (_report == "TimeBeverageShopper")
                    {
                        paramvalues = new object[] { reportparams.ShopperSegment_UniqueId.ToMyString(), reportparams.TimePeriodFrom_UniqueId, reportparams.TimePeriodTo_UniqueId, reportparams.Filter_UniqueId.ToMyString(), reportparams.SampleSizeCheck };
                    }
                    else if (_report == "TimeBeverageTrips")
                    {
                        paramvalues = new object[] { reportparams.TimePeriodFrom_UniqueId, reportparams.TimePeriodTo_UniqueId, reportparams.ShopperSegment_UniqueId.ToMyString(), reportparams.Filter_UniqueId.ToMyString(), reportparams.ShopperFrequency_UniqueId.ToMyString(), reportparams.SampleSizeCheck };
                    }
                    else if (Convert.ToString(reportparams.ModuleBlock).Contains("Within"))
                        paramvalues = new object[] { reportparams.TimePeriod_UniqueId, reportparams.ShopperSegment_UniqueId.ToMyString(), String.Join("|", reportparams.Comparison_UniqueIds), reportparams.Filter_UniqueId, reportparams.ShopperFrequency_UniqueId, reportparams.SampleSizeCheck };
                    else if (Convert.ToString(reportparams.ModuleBlock).Contains("Time"))
                        paramvalues = new object[] { reportparams.TimePeriodFrom_UniqueId, reportparams.TimePeriodTo_UniqueId, reportparams.ShopperSegment_UniqueId, reportparams.Filter_UniqueId.ToMyString(), reportparams.ShopperFrequency_UniqueId, reportparams.SampleSizeCheck };
                    else
                    {
                        paramvalues = new object[] { reportparams.TimePeriod_UniqueId, reportparams.CustomBase_UniqueId, String.Join("|", reportparams.Comparison_UniqueIds), reportparams.Filter_UniqueId, reportparams.ShopperFrequency_UniqueId,reportparams.Sigtype_UniqueId, reportparams.SampleSizeCheck };
                    }
                    if (reportparams.ModuleBlock.ToLower().IndexOf("beverage") == -1)
                    {
                        paramvalues = paramvalues.Concat(new object[] { reportparams.CompetitorFrequency_UniqueId, reportparams.CompetitorRetailer_UniqueId }).ToArray();
                    }                   
                    DataAccess dal = new DataAccess();
                    ds = dal.GetData_WithIdMapping(paramvalues, reportparams.SPName);                   
                    dsl = PrepareReports(reportparams,  out ShopperLowSampleSizelist, out TripsLowSampleSizelist,  SelectedReports, ds,out GenerateReport, out IsUseDirectionally,out lowSampleSizeItems, out useDirectionallyItems, out IsAllLowSampleSizes);                   
                }
                #endregion                
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            return dsl;
        }
        #region prepare reports
        public Dictionary<string, DataSet> PrepareReports(ReportGeneratorParams reportparams, out List<string> ShopperLowSampleSizelist,
            out List<string> TripsLowSampleSizelist, List<string> SelectedReports, DataSet report_ds, 
            out bool GenerateReport, out bool IsUseDirectionally, out List<LowSampleSizeItems> lowSampleSizeItems ,
            out List<UseDirectionallyItems> useDirectionallyItems, out bool IsAllLowSampleSizes)
        {
            lowSampleSizeItems = new List<LowSampleSizeItems>();
            ShopperLowSampleSizelist = new List<string>();
            TripsLowSampleSizelist = new List<string>();
            GenerateReport = true;
            IsUseDirectionally = false;
            IsAllLowSampleSizes = false;
            useDirectionallyItems = new List<UseDirectionallyItems>();
            Dictionary<string, DataSet> dsl = new Dictionary<string, DataSet>();
            Dictionary<string, DataSet> Reports = new Dictionary<string, DataSet>();          
            DataSet ds = null;
            DataTable tbl = null;          
            int to = 0;
            if (reportparams.ModuleBlock.Equals("AcrossBeverageShopper", StringComparison.OrdinalIgnoreCase) || reportparams.ModuleBlock.Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase)
               || reportparams.ModuleBlock.Equals("WithinBeverageShopper", StringComparison.OrdinalIgnoreCase) || reportparams.ModuleBlock.Equals("WithinBeverageTrips", StringComparison.OrdinalIgnoreCase)
               || reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase) || reportparams.ModuleBlock.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
            {
                if (report_ds != null && report_ds.Tables.Count > 0
                      && report_ds.Tables[0].Rows.Count > 0 && report_ds.Tables[0].Columns.Contains("SampleSizeStatus"))
                {                 
                    lowSampleSizeItems = (from r in report_ds.Tables[0].AsEnumerable()
                                          where Convert.ToDouble(r["SampleSize"]) < GlobalVariables.MinSampleSize
                                          && Convert.ToString(r.Field<object>("SampleSizeStatus")).ToLower() == "low"
                                          select new LowSampleSizeItems()
                                          {
                                              IsLowSampleSize = true,
                                              Name = Convert.ToString(r["Metric"])
                                          }).ToList();

                    useDirectionallyItems = (from r in report_ds.Tables[0].AsEnumerable()
                                             where Convert.ToDouble(r["SampleSize"]) < GlobalVariables.MaxSampleSize
                               && Convert.ToDouble(r["SampleSize"]) >= GlobalVariables.MinSampleSize
                               && Convert.ToString(r.Field<object>("SampleSizeStatus")).ToLower() == "low"
                                             select new UseDirectionallyItems()
                                             {
                                                 IsUseDirectionally = true,
                                                 Name = Convert.ToString(r["Metric"])
                                             }).ToList();
                    List<string> noofsections = (from r in report_ds.Tables[0].AsEnumerable()
                                                 select Convert.ToString(r.Field<object>("Metric"))).Distinct().ToList();

                    if (noofsections.Count == lowSampleSizeItems.Count)
                        IsAllLowSampleSizes = true;

                    if (lowSampleSizeItems.Count > 0 || useDirectionallyItems.Count > 0)
                        GenerateReport = false;
                }
                report_ds = CheckLowSampleSize(report_ds);
                Reports.Add(reportparams.ModuleBlock, report_ds);
            }
            else
            {
                if (report_ds != null && report_ds.Tables != null)
                {
                    to = report_ds.Tables.Count;                    
                    if (report_ds != null && report_ds.Tables.Count > 0
                     && report_ds.Tables[0].Rows.Count > 0 && report_ds.Tables[0].Columns.Contains("SampleSizeStatus"))
                    {
                        lowSampleSizeItems = (from r in report_ds.Tables[0].AsEnumerable()
                                              where Convert.ToDouble(r["SampleSize"]) < GlobalVariables.MinSampleSize
                                              && Convert.ToString(r.Field<object>("SampleSizeStatus")).ToLower() == "low"
                                              select new LowSampleSizeItems()
                                              {
                                                  IsLowSampleSize = true,
                                                  Name = Convert.ToString(r["Metric"])
                                              }).ToList();

                        useDirectionallyItems = (from r in report_ds.Tables[0].AsEnumerable()
                                                 where Convert.ToDouble(r["SampleSize"]) < GlobalVariables.MaxSampleSize
                                   && Convert.ToDouble(r["SampleSize"]) >= GlobalVariables.MinSampleSize
                                   && Convert.ToString(r.Field<object>("SampleSizeStatus")).ToLower() == "low"
                                                 select new UseDirectionallyItems()
                                                 {
                                                     IsUseDirectionally = true,
                                                     Name = Convert.ToString(r["Metric"])
                                                 }).ToList();
                        List<string> noofsections = (from r in report_ds.Tables[0].AsEnumerable()
                                                     select Convert.ToString(r.Field<object>("Metric"))).Distinct().ToList();

                        if (noofsections.Count == lowSampleSizeItems.Count)
                            IsAllLowSampleSizes = true;

                        if (lowSampleSizeItems.Count > 0 || useDirectionallyItems.Count > 0)
                            GenerateReport = false;

                    }
                    else
                    {
                        report_ds = CheckLowSampleSize(report_ds);
                        List<string> reports = (from row in report_ds.Tables[0].AsEnumerable() select Convert.ToString(row["ReportType"])).Distinct().ToList();                       
                        List<int> slideNumbers = null;
                        List<int> tableNumbers = null;
                        foreach (string _reportName in reports)
                        {
                            if (!Reports.ContainsKey(_reportName.Replace(" ", "").ToUpper()))
                            {
                                ds = new DataSet();
                                slideNumbers = (from row in report_ds.Tables[0].AsEnumerable()
                                           where Convert.ToString(row["ReportType"]).Equals(_reportName, StringComparison.OrdinalIgnoreCase)
                                           select Convert.ToInt32(row["SlideNumber"])).Distinct().ToList();
                                foreach (int slideNo in slideNumbers)
                                {
                                    tableNumbers = (from row in report_ds.Tables[0].AsEnumerable()
                                                where Convert.ToString(row["ReportType"]).Equals(_reportName, StringComparison.OrdinalIgnoreCase)
                                                && Convert.ToInt32(row["SlideNumber"]) == slideNo
                                                    select Convert.ToInt32(row["TableNumber"])).Distinct().ToList();
                                    foreach (int tableNo in tableNumbers)
                                    {
                                      var query = (from row in report_ds.Tables[0].AsEnumerable()
                                                        where Convert.ToString(row["ReportType"]).Equals(_reportName, StringComparison.OrdinalIgnoreCase)
                                                        && Convert.ToInt32(row["SlideNumber"]) == slideNo && Convert.ToInt32(row["TableNumber"]) == tableNo
                                                   select row).ToList();
                                        tbl = query.CopyToDataTable();
                                        ds.Tables.Add(tbl);
                                    }
                                }
                                Reports.Add(_reportName.Replace(" ", "").ToUpper(), ds);
                            }
                        }
                    }                    
                }
            }
            foreach (string report in Reports.Keys)
            {
                ds = new DataSet();
                if (Reports.ContainsKey(report))
                {
                    ds = Reports[report];
                }
                dsl.Add(report, ds);
            }
            return dsl;
        }
        #endregion
        #region check low sample size
        private DataSet CheckLowSampleSize(DataSet ds)
        {
            if(ds != null && ds.Tables.Count == 2)
            {
                Dictionary<string, double> samplesizelist = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase); 
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    if (!samplesizelist.ContainsKey(Convert.ToString(row["objective"])))
                        samplesizelist.Add(Convert.ToString(row["objective"]), Convert.ToDouble(row["NoOfRespondents"]));
                }           
                var lowsamplesizelist = (from row in ds.Tables[1].AsEnumerable()
                                         where Convert.ToDouble(row["NoOfRespondents"]) < GlobalVariables.MinSampleSize
                                         select new
                                         {
                                             Objective = Convert.ToString(row["objective"]),
                                             SampleSize = Convert.ToDouble(row["NoOfRespondents"])                                          
                                         }).ToList();

                if (lowsamplesizelist != null && lowsamplesizelist.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (samplesizelist.ContainsKey(Convert.ToString(row["objective"])) 
                            && samplesizelist[Convert.ToString(row["objective"])] < GlobalVariables.MinSampleSize)
                        {
                            row["volume"] = 0;
                        }
                    }
                }
            }
            return ds;
        }
        #endregion
        private string Get_ShortNames(String spVal)
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
        public List<LowSampleSizeRetailerList> GetRetailerSampleSize(FilterPanelInfo leftPanelData, HttpContextBase context)
        {
            List<LowSampleSizeRetailerList> lst = new List<LowSampleSizeRetailerList>();
            DataSet ds = GetValidRetailer(leftPanelData,context);
            if (ds == null)
                return lst;
            DataTable dt = ds.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                int ss = 0;
                int mss = 0;
                //int.TryParse(item["SampleSize"].ToString(), out ss);
                int.TryParse(item["TripSampleSize"].ToString(), out ss);
                //sabat
                //int.TryParse(item["MonthlyPlusSampleSize"].ToString(), out mss);
                int.TryParse(item["ShopperSampleSize"].ToString(), out mss);

                if (ss < 30)
                {
                    //sabat
                    if (mss < 30)
                    {
                        lst.Add(new LowSampleSizeRetailerList()
                        {
                            RetailerName = item["Metric"].ToString(),
                            RetailerId = item["MetricId"].ToString(),
                            SelectionType = item["SelectionType"].ToString(),
                            TimePeriodType = item["TimePeriodType"].ToString(),
                            UniqueFilterId = item["UniqueFilterId"].ToString(),
                            IsUseDirectional = "LSS"
                        });
                        
                    }
                    else
                    {
                        lst.Add(new LowSampleSizeRetailerList()
                        {
                            RetailerName = item["Metric"].ToString(),
                            RetailerId = item["MetricId"].ToString(),
                            SelectionType = item["SelectionType"].ToString(),
                            TimePeriodType = item["TimePeriodType"].ToString(),
                            UniqueFilterId = item["UniqueFilterId"].ToString(),
                            IsUseDirectional = "MSS"
                        });
                    }

                }
            }
            return lst;
        }
        private DataSet GetValidRetailer(FilterPanelInfo leftPanelData,HttpContextBase context)
        {
            object[] param = new object[6];
            string[] paramId = new string[6];
            SqlDbType[] paramType = new SqlDbType[6];
            string spName = "";

            spName = "USP_BrefingBook_SampleSizeValidation";
            //spName = "USP_BrefingBook_SampleSizeValidation_Sabat";
            param[0] = leftPanelData.TimeperiodID;
            paramId[0] = "@TimePeriod";
            paramType[0] = SqlDbType.VarChar;

            #region datatable for @selections parameter
            DataTable dt = new DataTable();
            dt.Columns.Add("UniqueId", typeof(Int32));
            dt.Columns.Add("ChannelId", typeof(Int32));
            dt.Columns.Add("Channel", typeof(string));
            dt.Columns.Add("SelectionType", typeof(string));
            DataRow benchmarkRow = dt.NewRow();
            benchmarkRow[0] = leftPanelData.BenchMark.UniqueFilterId;
            benchmarkRow[1] = leftPanelData.BenchMark.ID;
            benchmarkRow[2] = leftPanelData.BenchMark.Name;
            benchmarkRow[3] = leftPanelData.BenchMark.selectionType;
            dt.Rows.Add(benchmarkRow);

            for (int i = 0; i < leftPanelData.CustomBase.Count; i++)
            {
                DataRow dRow = dt.NewRow();
                dRow[0] = leftPanelData.CustomBase[i].UniqueFilterId;
                dRow[1] = leftPanelData.CustomBase[i].ID;
                dRow[2] = leftPanelData.CustomBase[i].Name;
                dRow[3] = leftPanelData.CustomBase[i].selectionType;
                dt.Rows.Add(dRow);
            }
            param[1] = dt;
            paramId[1] = "@Selections";
            paramType[1] = SqlDbType.Structured;
            #endregion

            if (leftPanelData.Filters!=null && leftPanelData.Filters.Count != 0)
            {
                var filterArray = (from filter in leftPanelData.Filters select filter.UniqueFilterId.ToString()).ToArray<String>();
                param[2] = String.Join("|", filterArray);
                
            }
            paramId[2] = "@AdvancedFilters";
            paramType[2] = SqlDbType.VarChar;
            //var competitorArray = (from com in leftPanelData.Competitors select com.ID.ToString()).ToArray<String>();
            var competitorArray = (from com in leftPanelData.Competitors select com.UniqueFilterId.ToString()).ToArray<String>();
            param[3] = String.Join("|", competitorArray);
            paramId[3] = "@CompetitorsId";
            paramType[3] = SqlDbType.VarChar;

            param[4] = leftPanelData.IsTripsOrShopper;
            paramId[4] = "@IsTripsorShopper";
            paramType[4] = SqlDbType.Bit;

            if (leftPanelData.Frequency.Count != 0)
            {
                param[5] = leftPanelData.Frequency.FirstOrDefault<FilterPanelData>().FrequencyId.Equals("1000") ? DBNull.Value : leftPanelData.Frequency.FirstOrDefault<FilterPanelData>().FrequencyId;
                paramId[5] = "@FrequencyId";
                paramType[5] = SqlDbType.VarChar;
            }
            DataAccess dal = new DataAccess();
            //DataSet ds = dal.GetData_WithIdMapping(param, spName);
            Exception ex = null;
            DataSet ds = dal.GetData(param, paramId, paramType, spName,out ex);
            if (ex != null)
            {
                ErrorLogSar.LogError(ex.Message + "   --- Error in fetching Data  spName=" + spName, ex.StackTrace, context);
            }
            return ds;
        }
    }
}