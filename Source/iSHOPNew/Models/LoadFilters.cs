using iSHOPNew.DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Microsoft.Ajax.Utilities;
using System.Reflection;
using System.Text;

namespace iSHOPNew.Models
{
    public class LoadFilters
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        DataAccess da = new DataAccess();
        DataSet ds = null;       
        #region Load Filters
        public Filters GetFilters()
        {
            Filters filters = new Filters();
            try
            {
                CommonFunctions common = new CommonFunctions();
                if (HttpContext.Current.Session[SessionVariables.FilterData] != null)
                {
                    filters = HttpContext.Current.Session[SessionVariables.FilterData] as Filters;
                    return filters;
                }
                ds = da.GetData("usp_Ishopfilters_test");

                HttpContext.Current.Session[SessionVariables.FilterData] = filters;
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<List<PrimaryAdvancedFilter>> AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
                    List<List<PrimaryAdvancedFilter>> VisitsAdvFilterlist = new List<List<PrimaryAdvancedFilter>>();
                    TotalMeasureFilterlist TotalTripHTMLMeasure = new TotalMeasureFilterlist();
                    TotalMeasureFilterlist TotalShopperHTMLMeasure = new TotalMeasureFilterlist();
                    GroupsFilterlist Retailers_GroupsFilterlist = new GroupsFilterlist();
                    GroupsFilterlist Beverages_GroupsFilterlist = new GroupsFilterlist();
                    SiteFilterlist SiteHTMLFilters = new SiteFilterlist();
                    GroupsFilterlist Ecomm_GroupsFilterlist = new GroupsFilterlist();

                    filters.TimePeriodlist = LoadTimePeriodFiltersFilters(1);
                    filters.Channel = LoadChannelOrCategoryFilters();
                    filters.Category = LoadCategoryOrBeverageFilters();
                    
                    filters.Frequencylist = LoadFrequencyFiltersFilters();
                    filters.BGMFrequencylist = LoadBGMFrequencyFiltersFilters();
                    filters.ReportFrequencylist = LoadReportFrequencyFiltersFilters();
                    filters.ReportTripsFrequencylist = LoadReportTripsFrequencyFiltersFilters();
                    filters.MonthlyPurchaselist = LoadMonthlyPurchase();                   

                    filters.AdvFilterlist = LoadAdvancedFiltersString(out AdvancedFilterLists);
                    filters.AdvancedFilterlist = AdvancedFilterLists[0];                   
                    filters.VisitsAdvFilterlist = LoadVisitsAdvancedFiltersString(out VisitsAdvFilterlist,4);
                    filters.VisitAdvancedFilter = VisitsAdvFilterlist[1];

                    //added by Nagaraju for E-commerce visit adv filters
                    //date: 24-04-2017
                    VisitsAdvFilterlist = new List<List<PrimaryAdvancedFilter>>();
                    LoadVisitsAdvancedFiltersString(out VisitsAdvFilterlist,31);
                    filters.EcommerceVisitAdvancedFilter = VisitsAdvFilterlist[1];               

                    filters.ChannelFilterlist = LoadChannelFilters();                   

                    filters.ShopperGroupTypelist = LoadGroupTypeFilters(23, "", out Retailers_GroupsFilterlist);
                    filters.TripGroupTypelist = LoadGroupTypeFilters(24, "", out Beverages_GroupsFilterlist);

                    //LoadGroupsHTMLFilters added by Nagaraju for  Groups HTML string
                    filters.Retailers_GroupsFilterlist = Retailers_GroupsFilterlist;
                    filters.Beverages_GroupsFilterlist = Beverages_GroupsFilterlist;

                    filters.ShopperGroupsPrimeFilterlist = GetGroupsPrimeFilters(23);
                    filters.TripsGroupsPrimeFilterlist = GetGroupsPrimeFilters(24);
                    filters.BeverageSelection = LoadBeverageSelection();

                    filters.AdvanceAnalytics = LoadAdvanceAnalyticsFilters("Shopper");
                    filters.TripsAdvanceAnalytics = LoadAdvanceAnalyticsFilters("Trips");
                    filters.NonBeverageList = LoadNonBeverageList();
                 
                    filters.TotalTripMeasure = LoadTotalMeasures(22, out TotalTripHTMLMeasure,"trips");                   
                    filters.TotalTripHTMLMeasure = TotalTripHTMLMeasure;                  
                   
                    filters.SelTypelist = GetMeasureData();
                    filters.GeographyList = LoadGeographyList();

                    //Ecommerce filters
                    filters.EcommTimePeriodList = LoadTimePeriodFiltersFilters(15);
                    filters.TripEcommerceMeasures = GetMeasureDataEcommerce(26);                                       
                    filters.SitesList = LoadSitesFilters(out SiteHTMLFilters);
                    filters.SiteHTMLFilters = SiteHTMLFilters;
                    filters.RightPanelMeasures = LoadRightFilters();
                    filters.EcommFrequencylist = LoadEcommFrequencyFilters("Online Order Frequency");
                    filters.EcommTripsFrequencylist = LoadEcommFrequencyFilters("Online Order Type");                   
                    filters.EcommShopperGroupTypeList = LoadEcommGroupTypeFilters(30, "",out Ecomm_GroupsFilterlist);
                    filters.Ecomm_GroupsFilterlist = Ecomm_GroupsFilterlist;
                    filters.EcommShopperGroupsPrimeFilterList = GetGroupsPrimeFilters(30);
                    //-------->

                    //added by Nagaraju for Geography filters
                    //Date: 13-04-2017
                    filters.DefaultGeographyFilters = common.GetWithinGeographyBenchmarkComparison("Geography", "MAR", "12mmt", "within");
                    filters.DefaultGeographyFiltersEcom = common.GetWithinGeographyBenchmarkComparison("Geography", string.Empty, "Total Time", "within");
                    
                }
                if (HttpContext.Current.Session["CustomRegions"] == null)
                {
                    DataSet dsCR = da.GetData("CustomRegions");
                    HttpContext.Current.Session["CustomRegions"] = dsCR;
                }
            }
            catch (Exception ex)
            {
                filters = null;
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            return filters;
        }
        #endregion
        #region Load Ecommerce Filters

        List<PrimaryAdvancedFilter> LoadEcommGroupTypeFilters(int TableNumber, string selecttype, out GroupsFilterlist groupsFilterlist)
        {
            groupsFilterlist = new GroupsFilterlist();
            groupsFilterlist.SearchObj = new SearchHTMLEntity();
            groupsFilterlist.SearchObj.SearchItems = new List<string>();
            StringBuilder GroupTypeHeaderContent = new StringBuilder();
            StringBuilder GroupTypeContent = new StringBuilder();
            StringBuilder GroupTypeContentSub = new StringBuilder();
            List<PrimaryAdvancedFilter> PrimaryAdvancedFilterlist = null;
            PrimaryAdvancedFilter PrimaryAdvancedFilter = null;
            if (ds.Tables[TableNumber].Rows.Count > 0)
            {
                var table_Objects = (from row in ds.Tables[TableNumber].AsEnumerable()
                                     select new
                                     {
                                         LevelId = row["LevelId"],
                                         FilterType = row["FilterType"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         FilterTypeId = row["FilterTypeId"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         PrimeFilterType = row["PrimeFilterType"],
                                         PrimeFilterTypeId = row["PrimeFilterTypeId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         ParentId = row["ParentId"]
                                     }).Distinct().ToList();

                PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                //Taking Measure Filtes             
                //End                
                //lavel 1
                GroupTypeHeaderContent.Append("<div id=\"GroupTypeHeaderContent\" class=\"Lavel\" style=\"display:none;height:94%;\">");
                GroupTypeHeaderContent.Append("<ul>");

                //lavel 2
                GroupTypeContent.Append("<div id=\"GroupTypeContent\" class=\"Lavel\" style=\"display:none;\">");

                //lavel 3
                GroupTypeContentSub.Append("<div id=\"GroupTypeContentSub\" class=\"Lavel\" style=\"display:none;\">");
                List<string> PrimaryFilterType = (from row in table_Objects
                                                  where Convert.ToString(row.LevelId) == "1"
                                                      && !Convert.ToString(row.FilterType).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                                  select (Convert.ToString(row.PrimeFilterTypeId))).Distinct().ToList();
                foreach (String PrimaryFilter in PrimaryFilterType)
                {
                    List<string> metriclist = (from row in table_Objects
                                               where Convert.ToString(row.LevelId) == "1"
                                               && Convert.ToString(row.PrimeFilterTypeId) == PrimaryFilter
                                               && !Convert.ToString(row.FilterType).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                               select (Convert.ToString(row.Metric))).Distinct().ToList();

                    foreach (String Metric in metriclist)
                    {
                        PrimaryAdvancedFilter = (from row in table_Objects
                                                 where (Convert.ToString(Metric).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase))
                                                 && Convert.ToString(row.LevelId) == "1"
                                                 && Convert.ToString(row.PrimeFilterTypeId) == PrimaryFilter
                                                  && !Convert.ToString(row.FilterType).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                                 select new PrimaryAdvancedFilter
                                                 {
                                                     Id = row.MetricId == null ? 00 : Convert.ToInt16(row.MetricId),
                                                     Name = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                     FullName = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                     Position = row.Metric == null ? "" : GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
                                                     Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                                     FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                     DBName = row.Metric == null ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)),
                                                     UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                     PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                     PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                     FilterType = Convert.ToString(row.FilterType)
                                                 }).FirstOrDefault();

                        GroupTypeHeaderContent.Append("<li style=\"display: none;\" primefiltertype=\"" + PrimaryAdvancedFilter.PrimeFilterType + "\" primefiltertypeid=\"" + PrimaryAdvancedFilter.PrimeFilterTypeId + "\" filtertype=\"" + PrimaryAdvancedFilter.FilterType + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" dbname=\"" + PrimaryAdvancedFilter.Name + "\" uniqueid=\"" + PrimaryAdvancedFilter.UniqueId + "\" shopperdbname=\"null\" tripsdbname=\"null\" class=\"gouptype\" onclick=\"SelecGroup(this);\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + PrimaryAdvancedFilter.FilterTypeId + "\" id=\"" + PrimaryAdvancedFilter.Id + "\" type=\"Main-Stub\" name=\"" + PrimaryAdvancedFilter.Name + "\">" + PrimaryAdvancedFilter.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>");
                        //foreach (PrimaryAdvancedFilter _PrimaryAdvancedFilter in PrimaryAdvancedFilterlist)
                        //{
                        GroupTypeContent.Append("<div class=\"DemographicList\" id=\"" + PrimaryAdvancedFilter.Id + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" fullname=\"" + PrimaryAdvancedFilter.Name + "\" parentname=\"" + PrimaryAdvancedFilter.PrimeFilterType + "\" parentid=\"" + PrimaryAdvancedFilter.PrimeFilterTypeId + "\" style=\"overflow-y: auto;display:none;\">");
                            GroupTypeContent.Append("<ul>");
                            PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                                 where !Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                                  && Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                                  && Convert.ToString(row.PrimeFilterTypeId) == PrimaryAdvancedFilter.PrimeFilterTypeId
                                                                                  select new SecondaryAdvancedFilter
                                                                                  {
                                                                                      Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                                      LevelId = Convert.ToString(row.LevelId),
                                                                                      Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                                      Metric = Convert.ToString(row.Metric),
                                                                                      FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                                      MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                                      ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                                                      FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                                                      DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                                      UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                                      PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                                      PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                                                      FilterType = Convert.ToString(row.FilterType)
                                                                                  }).Distinct().ToList();

                            foreach (SecondaryAdvancedFilter secfil in PrimaryAdvancedFilter.SecondaryAdvancedFilterlist)
                            {
                                var query = secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                                  where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                                  && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                                  && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
                                                                                  //&& Convert.ToString(row.PrimeFilterTypeId) == PrimaryFilter
                                                                                  select new SecondaryAdvancedFilter
                                                                                  {
                                                                                      Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                                      Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                                      Metric = Convert.ToString(row.Metric),
                                                                                      FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                                      MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                                      ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                                                      FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                                                      DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                                      UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                                      PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                                      FilterType = Convert.ToString(row.FilterType)
                                                                                  }).Distinct().ToList();
                                if (query != null && query.Count > 0)
                                {
                                    GroupTypeContent.Append("<li class=\"gouptype\" MetricId=\"" + secfil.MetricId + "\" style=\"\" PrimeFilterType=\"" + secfil.PrimeFilterType + "\" FilterType=\"" + secfil.FilterType + "\" Name=\"" + secfil.Name + "\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" class=\"gouptype\" onclick=\"DisplayThirdLevelDemoFilter(this);\">");
                                    GroupTypeContent.Append("<div  class=\"FilterStringContainerdiv\" style=\"\">");
                                    GroupTypeContent.Append("<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + secfil.FilterTypeId + "\" id=\"" + secfil.Id + "\" type=\"Main-Stub\" Name=\"" + secfil.Name + "\">" + secfil.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                                    GroupTypeContent.Append("</div>");
                                    GroupTypeContent.Append("</li>");

                                    secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                          where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                            && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
                                                                          //&& Convert.ToString(row.PrimeFilterTypeId) == PrimaryFilter
                                                                          select new SecondaryAdvancedFilter
                                                                          {
                                                                              Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                              Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                              Metric = Convert.ToString(row.Metric),
                                                                              FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                              MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                              ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                                              FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                                              DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                              UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                              PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                              PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                                              FilterType = Convert.ToString(row.FilterType)
                                                                          }).Distinct().ToList();
                                    GroupTypeContentSub.Append("<div class=\"DemographicList\" id=\"" + PrimaryAdvancedFilter.Id + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" fullname=\"" + PrimaryAdvancedFilter.Name + "\" parentname=\"" + PrimaryAdvancedFilter.PrimeFilterType + "\" parentid=\"" + PrimaryAdvancedFilter.PrimeFilterTypeId + "\" style=\"overflow-y: auto;display:none;\">");
                                    GroupTypeContentSub.Append("<ul>");
                                    foreach (SecondaryAdvancedFilter thfil in secfil.SecondaryAdvancedFilterlist)
                                    {
                                        GroupTypeContentSub.Append("<div MetricId=\"" + secfil.MetricId + "\" style=\"display: none;\" id=\"" + secfil.Id + "\" PrimeFilterType=\"" + secfil.PrimeFilterType + "\" FilterType=\"" + secfil.FilterType + "\" Name=\"" + thfil.Name + "\" MericName=\"" + secfil.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + thfil.isGeography + "\" FullName=\"" + thfil.FullName + "\" DBName=\"" + thfil.DBName + "\" UniqueId=\"" + thfil.UniqueId + "\" shopperdbname=\"" + thfil.ShopperDBName + "\" tripsdbname=\"" + thfil.TripsDBName + "\"  data-id=\"" + thfil.Id + "\" id=" + thfil.Id + "-" + thfil.MetricId + "-" + thfil.ParentId + " Name=\"" + thfil.Name + "\" parent=\"" + thfil.ParentId + "\" ParentLevelId=\" " + PrimaryAdvancedFilter.Id + " \" ParentLevelName=\" " + PrimaryAdvancedFilter.Name + " \" data-isselectable=\"true\">" + thfil.Name + "</span></div>");
                                        groupsFilterlist.SearchObj.SearchItems.Add(thfil.UniqueId + "|" + thfil.Name);
                                    }
                                    GroupTypeContentSub.Append("</ul>");
                                    GroupTypeContentSub.Append("</div>");
                                }
                                else
                                {
                                    GroupTypeContent.Append("<div PrimeFilterType=\"" + secfil.PrimeFilterType + "\" FilterType=\"" + secfil.FilterType + "\" Name=\"" + secfil.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + secfil.isGeography + "\" FullName=\"" + secfil.FullName + "\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" data-id=\"" + secfil.Id + "\" id=" + secfil.Id + "-" + secfil.MetricId + "-" + secfil.ParentId + " Name=\"" + secfil.Name + "\" parent=\"" + secfil.ParentId + "\" ParentLevelId=\" " + PrimaryAdvancedFilter.Id + " \" ParentLevelName=\" " + PrimaryAdvancedFilter.Name + " \" data-isselectable=\"true\">" + secfil.Name + "</span></div>");
                                    groupsFilterlist.SearchObj.SearchItems.Add(secfil.UniqueId + "|" + secfil.Name);
                                }
                            }
                            GroupTypeContent.Append("</ul>");
                            GroupTypeContent.Append("</div>");
                        //}
                    }
                    PrimaryAdvancedFilterlist.Add(PrimaryAdvancedFilter);
                }
            }
            GroupTypeHeaderContent.Append("</ul>");
            GroupTypeHeaderContent.Append("</div>");

            GroupTypeContent.Append("</div>");
            GroupTypeContentSub.Append("</div>");

            groupsFilterlist.SearchObj.HTML_String += GroupTypeHeaderContent.ToString();
            groupsFilterlist.SearchObj.HTML_String += GroupTypeContent.ToString();
            groupsFilterlist.SearchObj.HTML_String += GroupTypeContentSub.ToString();
            //added by Nagaraju for HTML string
            //Date: 17-04-2017
            PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
            return PrimaryAdvancedFilterlist;
        }
        List<Frequency> LoadEcommFrequencyFilters(string filtertype)
        {
            List<Frequency> Frequencylist = null;
            Frequency Frequency = null;
            if (ds.Tables[28].Rows.Count > 0)
            {
                Frequencylist = new List<Frequency>();
                foreach (String frequency in (from row in ds.Tables[28].AsEnumerable()
                                              where Convert.ToString(row["FilterType"]).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                                              select (Convert.ToString(row["Metric"]))).Distinct().ToList())
                {
                    Frequency = new Frequency();
                    Frequency = (from row in ds.Tables[28].AsEnumerable()
                                 where Convert.ToString(frequency).Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase)
                                 select new Frequency
                                 {
                                     Id = Convert.ToInt16(row["UniqueFilterId"]),
                                     Name = Convert.ToString(row["Metric"]),
                                     SubFrequency = Convert.ToString(row["FilterType"]),
                                     UniqueId = Convert.ToString(row["UniqueFilterId"])
                                 }).FirstOrDefault();
                    Frequencylist.Add(Frequency);
                }
            }
            return Frequencylist;
        }

        List<RightPanelMetrics> LoadRightFilters()
        {
            List<RightPanelMetrics> MetricList = null;
            RightPanelMetrics Metric = null;
            if (ds.Tables[4].Rows.Count > 0)
            {
                MetricList = new List<RightPanelMetrics>();
                foreach (String SiteMetric in (from row in ds.Tables[28].AsEnumerable() select (Convert.ToString(row["FilterType"]))).Distinct().ToList())
                {
                    Metric = new RightPanelMetrics();
                    Metric = (from row in ds.Tables[28].AsEnumerable()
                              where Convert.ToString(SiteMetric).Equals(Convert.ToString(row["FilterType"]), StringComparison.OrdinalIgnoreCase)
                              select new RightPanelMetrics
                              {
                                  Id = Convert.ToInt16(row["FilterTypeId"]),
                                  Name = Convert.ToString(row["FilterType"]),

                              }).FirstOrDefault();
                    MetricList.Add(Metric);
                }

                foreach (RightPanelMetrics _MetricList in MetricList)
                {
                    _MetricList.MetricList = (from row in ds.Tables[28].AsEnumerable()
                                              where Convert.ToString(_MetricList.Name).Equals(Convert.ToString(row["FilterType"]), StringComparison.OrdinalIgnoreCase)
                                              && !Convert.ToString("").Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase)
                                              && !Convert.ToString(_MetricList.Name).Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase)
                                              select new RightPanelMetricItem
                                              {
                                                  MetricItemId = Convert.ToInt16(row["MetricId"]),
                                                  MetricItemName = Convert.ToString(row["Metric"]),
                                                  ParentId = Convert.ToString(row["FilterTypeId"]),
                                                  UniqueId = Convert.ToString(row["UniqueFilterId"]),
                                              }).Distinct().ToList();

                }

            }
            return MetricList;
        }

        List<Sites> LoadSitesFilters(out SiteFilterlist SiteHTMLFilters)
        {
            SiteHTMLFilters = new SiteFilterlist();
            List<string> SearchItems = new List<string>();
            List<string> AllSearchItems = new List<string>();
            StringBuilder PrimarySiteFilterContent = new StringBuilder();
            StringBuilder SecondarSiteFilterContent = new StringBuilder();
            StringBuilder Level3SiteFilterContent = new StringBuilder();
            string sImageClassName = string.Empty;
            List<Sites> Sitelist = null;
            List<int> Levels = null;
            if (ds.Tables[16].Rows.Count > 0)
            {
                var table_Objects = (from row in ds.Tables[16].AsEnumerable()
                                     select new
                                     {
                                         LevelId = row["LevelId"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         ParentId = row["ParentId"]
                                     }).Distinct().ToList();

                Levels = (from row in table_Objects
                          where Convert.ToInt16(row.LevelId) != 1
                          orderby row.LevelId ascending
                          select int.Parse(Convert.ToString(row.LevelId))).Distinct().ToList();

                PrimarySiteFilterContent.Append("<div id=\"PrimarySiteFilterContent\" class=\"DemoLevel Lavel Lavel1\" style=\"height:84%;overflow-y:auto;\">");
                PrimarySiteFilterContent.Append("<div id=\"PrimarySiteFilterList\" class=\"clsPrimaryDemoFilterList\" style=\"height:auto;overflow:hidden;\">");
                SecondarSiteFilterContent.Append("<div id=\"SecondarSiteFilterContent\" class=\"DemoMeasureTypeHeaderMainLevel DemoLevel2 Lavel2 Sub-Lavel Lavel\" style=\"display: none;height:84%;\">");
                Level3SiteFilterContent.Append("<div id=\"Level3SiteFilterContent\" class=\"DemoMeasureTypeHeaderMainLevel DemoLevel3 Lavel3 Sub-Lavel Lavel\" style=\"display: none;height:84%;\">");
                PrimarySiteFilterContent.Append("<ul>");
                Sitelist = (from row in table_Objects
                            where Convert.ToInt16(row.LevelId) == 1
                            select new Sites
                            {
                                Id = Convert.ToInt16(row.UniqueFilterId),
                                MetricItemId = Convert.ToInt16(row.MetricId),
                                Name = Convert.ToString(row.Metric),
                                LevelId = Convert.ToString(row.LevelId),
                                UniqueId = Convert.ToString(row.UniqueFilterId),
                            }).ToList();

                foreach (Sites _Sitelist in Sitelist)
                {
                   var query = (from row in table_Objects
                                          where Convert.ToInt16(row.ParentId) == _Sitelist.MetricItemId
                                          && Levels.Contains(int.Parse(Convert.ToString(row.LevelId)))                                        
                                          select new SitesMetricItem
                                          {
                                              Id = Convert.ToInt16(row.UniqueFilterId),
                                              Name = Convert.ToString(row.Metric),
                                              MetricItemId = Convert.ToInt16(row.MetricId),
                                              MetricItemName = Convert.ToString(row.Metric),
                                              LevelId = Convert.ToString(row.LevelId),
                                              ParentId = Convert.ToString(row.ParentId),
                                              UniqueId = Convert.ToString(row.UniqueFilterId),
                                          }).Distinct().ToList();
                   if (query != null && query.Count > 0)
                   {
                       int maxlevel = (from row in table_Objects
                                          where Convert.ToInt16(row.ParentId) == _Sitelist.MetricItemId
                                         orderby row.LevelId descending
                                         select int.Parse(Convert.ToString(row.LevelId))).FirstOrDefault();

                       sImageClassName = GetSitesImagePosition(_Sitelist.Name.ToLower());

                       PrimarySiteFilterContent.Append("<li style=\"display:table;\">");
                       PrimarySiteFilterContent.Append("<div lavels=\"" + maxlevel + "\" onclick=\"DisplaySecondarySiteFilter(this);\" Name=\"" + _Sitelist.Name + "\" id=\"" + _Sitelist.Id + "\" class=\"lft-popup-ele header FilterStringContainerdiv\" style=\"\">");
                       if (string.IsNullOrEmpty(sImageClassName))
                           PrimarySiteFilterContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>");
                       else
                           PrimarySiteFilterContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg');background-position:" + sImageClassName + "\"></span>");

                       PrimarySiteFilterContent.Append("<span style=\"width:79%;\" class=\"lft-popup-ele-label\" id=\"" + _Sitelist.Id + "\" data-val=" + _Sitelist.Name + " data-parent=\"\" data-isselectable=\"true\">" + _Sitelist.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>");
                       PrimarySiteFilterContent.Append("</li>");
                       foreach (int level in Levels)
                       {
                           sImageClassName = GetSitesImagePosition(_Sitelist.Name);
                           _Sitelist.SiteList = (from row in table_Objects
                                                 where Convert.ToInt16(row.ParentId) == _Sitelist.MetricItemId
                                                 && level == int.Parse(Convert.ToString(row.LevelId))
                                                 select new SitesMetricItem
                                                 {
                                                     Id = Convert.ToInt16(row.UniqueFilterId),
                                                     Name = Convert.ToString(row.Metric),
                                                     MetricItemId = Convert.ToInt16(row.MetricId),
                                                     MetricItemName = Convert.ToString(row.Metric),
                                                     LevelId = Convert.ToString(row.LevelId),
                                                     ParentId = Convert.ToString(row.ParentId),
                                                     UniqueId = Convert.ToString(row.UniqueFilterId),
                                                 }).Distinct().ToList();
                           if (_Sitelist.SiteList != null && _Sitelist.SiteList.Count > 0)
                           {                             
                               if (level == 2)
                               {
                                   SecondarSiteFilterContent.Append("<div class=\"DemographicList\" id=\"" + _Sitelist.Id + "\" Name=\"" + _Sitelist.Name + "\" FullName=\"" + _Sitelist.Name + "\" style=\"overflow-y:auto;display:none;\">");
                                   SecondarSiteFilterContent.Append("<ul>");
                                   foreach (SitesMetricItem smitem in _Sitelist.SiteList)
                                   {
                                       SecondarSiteFilterContent.Append("<div onclick=\"SelectSite(this);\" LevelId=\"" + smitem.LevelId + "\" Name=\"" + smitem.Name + "\" id=\"" + smitem.Id + "\" uniqueid=\"" + smitem.Id + "\" class=\"lft-popup-ele\" style=\"    height: 31px;\"><span style=\"height: 100%;\" class=\"lft-popup-ele-label\" FullName=\"" + smitem.MetricItemName + "\" DBName=\"" + smitem.MetricItemName + "\" UniqueId=\"" + smitem.UniqueId + "\" shopperdbname=\"\" tripsdbname=\"\" id=\"" + smitem.MetricItemId + "\"  Name=\"" + smitem.MetricItemName + "\" parent=\"" + smitem.ParentId + "\"  data-isselectable=\"true\">" + smitem.MetricItemName + "</span></div>");
                                       if (!AllSearchItems.Contains(_Sitelist.Name))
                                       {
                                           SearchItems.Add(smitem.Id + "|" + smitem.Name);
                                           AllSearchItems.Add(smitem.Name);
                                       }
                                   }
                                   SecondarSiteFilterContent.Append("</ul>");
                                   SecondarSiteFilterContent.Append("</div>");
                               }
                               else if (level == 3)
                               {
                                   Level3SiteFilterContent.Append("<div class=\"DemographicList\" id=\"" + _Sitelist.Id + "\" Name=\"" + _Sitelist.Name + "\" FullName=\"" + _Sitelist.Name + "\" style=\"overflow-y:auto;display:none;\">");
                                   Level3SiteFilterContent.Append("<ul>");
                                   foreach (SitesMetricItem smitem in _Sitelist.SiteList)
                                   {
                                       Level3SiteFilterContent.Append("<div onclick=\"SelectSite(this);\" LevelId=\"" + smitem.LevelId + "\" Name=\"" + smitem.Name + "\" id=\"" + smitem.Id + "\" uniqueid=\"" + smitem.Id + "\" class=\"lft-popup-ele\" style=\"    height: 31px;\"><span style=\"height: 100%;\" class=\"lft-popup-ele-label\" FullName=\"" + smitem.MetricItemName + "\" DBName=\"" + smitem.MetricItemName + "\" UniqueId=\"" + smitem.UniqueId + "\" shopperdbname=\"\" tripsdbname=\"\" id=\"" + smitem.MetricItemId + "\"  Name=\"" + smitem.MetricItemName + "\" parent=\"" + smitem.ParentId + "\"  data-isselectable=\"true\">" + smitem.MetricItemName + "</span></div>");
                                       if (!AllSearchItems.Contains(_Sitelist.Name))
                                       {
                                           SearchItems.Add(smitem.Id + "|" + smitem.Name);
                                           AllSearchItems.Add(smitem.Name);
                                       }
                                   }
                                   Level3SiteFilterContent.Append("</ul>");
                                   Level3SiteFilterContent.Append("</div>");
                               }
                           }                          
                       }
                   }
                   else
                   {
                       sImageClassName = GetSitesImagePosition(_Sitelist.Name.ToLower());
                       PrimarySiteFilterContent.Append("<li style=\"display:table;\">");
                       PrimarySiteFilterContent.Append("<div onclick=\"SelectSite(this);\" LevelId=\"" + _Sitelist.LevelId + "\" Name=\"" + _Sitelist.Name + "\" id=\"" + _Sitelist.Id + "\" uniqueid=\"" + _Sitelist.Id + "\" class=\"lft-popup-ele\" style=\"height:31px;\">");
                       if (string.IsNullOrEmpty(sImageClassName))
                           PrimarySiteFilterContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>");
                       else
                           PrimarySiteFilterContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg');background-position:" + sImageClassName + "\"></span>");

                       PrimarySiteFilterContent.Append("<span style=\"width:83%;height:100%;\" class=\"lft-popup-ele-label\" id=\"" + _Sitelist.Id + "\" data-val=" + _Sitelist.Name + " data-parent=\"\" data-isselectable=\"true\">" + _Sitelist.Name + "</span></div>");
                       PrimarySiteFilterContent.Append("</li>");
                       if (!AllSearchItems.Contains(_Sitelist.Name))
                       {
                           SearchItems.Add(_Sitelist.Id + "|" + _Sitelist.Name);
                           AllSearchItems.Add(_Sitelist.Name);
                       }
                   }
                }
            }
            PrimarySiteFilterContent.Append("</ul>");
            PrimarySiteFilterContent.Append("</div>");
            PrimarySiteFilterContent.Append("</div>");
            SecondarSiteFilterContent.Append("</div>");
            Level3SiteFilterContent.Append("</div>");

            SiteHTMLFilters.SearchObj = new SearchHTMLEntity();
            SiteHTMLFilters.SearchObj.HTML_String = PrimarySiteFilterContent.ToString();
            SiteHTMLFilters.SearchObj.HTML_String += SecondarSiteFilterContent.ToString();
            SiteHTMLFilters.SearchObj.HTML_String += Level3SiteFilterContent.ToString();
            SiteHTMLFilters.SearchObj.SearchItems = SearchItems;
            //added by Nagaraju for HTML string
            //Date: 17-04-2017
            Sitelist = new List<Sites>();
            return Sitelist;
        }
        //Get site image position
        string GetSitesImagePosition(string site)
        {
            string sImageClassName = string.Empty;
            if (!string.IsNullOrEmpty(site))
            {
                switch (site.ToLower().Trim())
                {
                    case "total shopper":
                        {
                            sImageClassName = "-932px -675px";
                            break;
                        }
                    case "total online shopper":
                        {
                            sImageClassName = "-1080px -672px";
                            break;
                        }
                    case "total online grocery shopper":
                        {
                            sImageClassName = "-1133px -672px";
                            break;
                        }
                    case "sites":
                        {
                            sImageClassName = "-1182px -676px";
                            break;
                        }
                }
            }
            return sImageClassName;
        }
        public List<SelTypes> GetMeasureDataEcommerce(int TableNumber)
        {
            StringBuilder html1 = new StringBuilder();
            StringBuilder html2 = new StringBuilder();
            StringBuilder html3 = new StringBuilder();
            StringBuilder html4 = new StringBuilder();
            StringBuilder html5 = new StringBuilder();
            string filtertyle = string.Empty;
            List<string> searchitems = null;
            List<string> shopperSearchItems = null;
            List<string> tripsSearchItems = null;

            //added by Nagaraju D for checking duplicate search items
            //Date: 11-29-2017
            List<string> Allsearchitems = null;
            List<string> AllshopperSearchItems = null;
            List<string> AlltripsSearchItems = null; 

            string seltype = string.Empty;
            List<SelTypes> SelTypelist = new List<DAL.SelTypes>();
            SelTypes Seltype = null;
            List<GroupType> GroupTypelist = new List<GroupType>();

            DataTable tbl = ds.Tables[26].Copy();
            tbl.Merge(ds.Tables[27]);

            //var table_Objects = (from row in ds.Tables[TableNumber].AsEnumerable()
            var table_Objects = (from row in tbl.AsEnumerable()
                                 select new
                                 {
                                     Metric = row["Metric"],
                                     MetricId = row["MetricId"],
                                     MetricItem = row["MetricItem"],
                                     MetricItemId = row["MetricItemId"],
                                     LevelId = row["LevelId"],
                                     SelTypeId = row["SelTypeId"],
                                     SelType = row["SelType"],
                                     FilterType = row["FilterType"],
                                     FilterTypeId = row["FilterTypeId"],
                                     ChartTypePIT = row["ChartTypePIT"],
                                     ChartTypeTrend = row["ChartTypeTrend"],
                                     UniqueFilterId = row["UniqueFilterId"],
                                     ParentId = row["ParentId"],
                                     SearchName = row["SearchName"]
                                 }).Distinct().ToList();

            foreach (string _seltype in (from row in table_Objects
                                         select Convert.ToString(row.SelTypeId)).Distinct().ToList())
            {
                if (Convert.ToInt32(_seltype) % 2 != 0)
                {
                    Seltype = new DAL.SelTypes();
                    Seltype.SelType = Convert.ToString(_seltype);
                    SelTypelist.Add(Seltype);
                }
            }
            foreach (SelTypes _seltype in SelTypelist)
            {
                html1 = new StringBuilder();
                html2 = new StringBuilder();
                html3 = new StringBuilder();
                html4 = new StringBuilder();
                html5 = new StringBuilder();
                searchitems = new List<string>();
                shopperSearchItems = new List<string>();
                tripsSearchItems = new List<string>();

                Allsearchitems = new List<string>();
                AllshopperSearchItems = new List<string>();
                AlltripsSearchItems = new List<string>();

                GroupTypelist = new List<GroupType>();
                html1.Append("<ul>");
                foreach (string _group in (from row in table_Objects
                                           where (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32
                                           (_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                           select Convert.ToString(row.FilterType)).Distinct().ToList())
                {
                    GroupType _GroupType = new GroupType();
                    _GroupType = (from row in table_Objects
                                  where Convert.ToString(row.FilterType).Equals(_group, StringComparison.OrdinalIgnoreCase)
                                  && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                  select new GroupType
                                  {
                                      GroupName = Convert.ToString(row.FilterType),
                                      GroupId = Convert.ToString(row.FilterTypeId),
                                      SelType = Convert.ToString(row.SelType)
                                  }).Distinct().FirstOrDefault();
                    GroupTypelist.Add(_GroupType);

                     filtertyle = "Shopper";
                    if (Convert.ToString(_GroupType.GroupName).ToLower().IndexOf("demographics") > -1)
                        filtertyle = "Demographics";
                    else if (Convert.ToString(_GroupType.SelType).ToLower().IndexOf("trip") > -1)
                        filtertyle = "Visits";

                    html1.Append("<li style=\"display:none;\" Id=\"" + _GroupType.GroupId + "\" Name=\"" + _GroupType.GroupName + "\" filtertype=\"" + filtertyle + "\" class=\"gouptype main-measure\" onclick=\"DisplayMeasureList(this);\">");
                    html1.Append("<div class=\"FilterStringContainerdiv\">");
                    html1.Append("<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" Id=\"" + _GroupType.GroupId + "\"  type=\"Main-Stub\"  Name=\"" + _GroupType.GroupName + "\">" + _GroupType.GroupName + "</div>");
                    html1.Append("<div class=\"ArrowContainerdiv measure-inactive\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                    html1.Append("</div>");
                    html1.Append("</li>");
                }
                html1.Append("</ul>");
                foreach (GroupType _group in GroupTypelist)
                {
                    filtertyle = "Shopper";
                    if (Convert.ToString(_group.GroupName).ToLower().IndexOf("demographics") > -1)
                        filtertyle = "Demographics";
                    else if (Convert.ToString(_group.SelType).ToLower().IndexOf("trip") > -1)
                        filtertyle = "Visits";

                    html2.Append("<ul>");
                    _group.PrimaryAdvancedFilter = new List<PrimaryAdvancedFilter>();
                    foreach (string measure in (from row in table_Objects
                                                where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                                select Convert.ToString(row.Metric)).Distinct().ToList())
                    {
                        var query = (from row in table_Objects
                                     where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                     && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                      && Convert.ToString(row.MetricItem).Equals(measure, StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(row.MetricItem)).FirstOrDefault();

                        if (string.IsNullOrEmpty(Convert.ToString(query)))
                        {
                            PrimaryAdvancedFilter pfl = (from row in table_Objects
                                                         where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                          && Convert.ToString(row.Metric).Equals(measure, StringComparison.OrdinalIgnoreCase)
                                                         && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                         && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                         && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                                         select new PrimaryAdvancedFilter
                                                         {
                                                             Id = row.MetricId == null ? 00 : Convert.ToInt16(row.MetricId),
                                                             Name = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                             FullName = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                             Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                                             selectType = row.SelTypeId == null ? "" : Convert.ToString(row.SelTypeId),
                                                             FilterType = row.FilterType == null ? "" : Convert.ToString(row.FilterType),
                                                             FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                             ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
                                                             ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
                                                             DBName = Convert.ToString(row.Metric),
                                                             UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                             ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                             SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
                                                         }).FirstOrDefault();
                            _group.PrimaryAdvancedFilter.Add(pfl);
                            html2.Append("<li ParentDetails=\"\" style=\"display:none;\" type=\"" + filtertyle + "\" Seltypeid=\"" + pfl.selectType + "\" FilterTypeId=\"" + pfl.FilterTypeId + "\" FilterType=\"" + pfl.FilterType + "\" parentname=\"" + _group.GroupName + "\" DBName=\"" + pfl.DBName + "\" UniqueId=\"" + pfl.UniqueId + "\" shopperdbname=\"" + pfl.ShopperDBName + "\" tripsdbname=\"" + pfl.TripsDBName + "\" Name=\"" + pfl.Name + "\" class=\"gouptype\" ChartTypePIT=\"" + pfl.ChartTypePIT + "\" ChartTypeTrend=\"" + pfl.ChartTypeTrend + "\" onclick=\"SelecMeasure(this);\">");
                            html2.Append("<div class=\"FilterStringContainerdiv\">");
                            html2.Append("<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + pfl.FilterTypeId + "\" id=\"" + pfl.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + pfl.ChartTypePIT + "\" ChartTypeTrend=\"" + pfl.ChartTypeTrend + "\" Name=\"" + pfl.Name + "\">" + pfl.Name + "</div>");
                            html2.Append("<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                            html2.Append("</div>");
                            html2.Append("</li>");
                        }
                    }
                    foreach (PrimaryAdvancedFilter primaryAdvancedFilter in _group.PrimaryAdvancedFilter)
                    {
                        List<SecondaryAdvancedFilter> seclist = (from row in table_Objects
                                                                 where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.Metric).Equals(Convert.ToString(primaryAdvancedFilter.Name), StringComparison.OrdinalIgnoreCase)
                                                                 && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                                                 select new SecondaryAdvancedFilter
                                                                 {

                                                                     Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                     Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                     FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                     MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                     Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                                                     ParentId = row.ParentId == null ? "" : Convert.ToString(primaryAdvancedFilter.Id.ToString()),
                                                                     DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
                                                                     UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                     ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
                                                                     ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
                                                                     SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName),
                                                                     FilterType = row.FilterType == null ? "" : Convert.ToString(row.FilterType)
                                                                 }).Distinct().ToList();

                        primaryAdvancedFilter.SecondaryAdvancedFilterlist = seclist;

                        foreach (SecondaryAdvancedFilter secfil in seclist)
                        {
                            var query = (from row in table_Objects
                                         where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                        && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                         && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                         && Convert.ToString(row.Metric).Equals(Convert.ToString(secfil.Name), StringComparison.OrdinalIgnoreCase)
                                         && Convert.ToString(row.ParentId).Equals(Convert.ToString(secfil.ParentId), StringComparison.OrdinalIgnoreCase)
                                         select new SecondaryAdvancedFilter
                                         {

                                             Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                             Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                             FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                             MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                             Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                             ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                             DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
                                             UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                             ParentDetails = (row.MetricItemId == null || row.MetricItem == null || row.ParentId == null) ? "" : primaryAdvancedFilter.Id + "|" + Convert.ToString(row.MetricItemId) + "|" + Convert.ToString(row.MetricItem) + "|" + Convert.ToString(row.ParentId),
                                             ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
                                             ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
                                             SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName),
                                             FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                             FilterType = row.FilterType == null ? "" : Convert.ToString(row.FilterType)
                                         }).Distinct().ToList();
                            secfil.SecondaryAdvancedFilterlist = new List<SecondaryAdvancedFilter>();
                            secfil.SecondaryAdvancedFilterlist = query.ToList();
                            if (query != null && query.Count > 0)
                            {
                                html3.Append("<ul Name=\"" + primaryAdvancedFilter.Name + "\" style=\"display:none;\">");
                                html3.Append("<li ParentDetails=\"" + secfil.ParentDetails + "\" type=\"" + filtertyle + "\" id=\"" + secfil.Id + "\" FilterTypeId=\"" + primaryAdvancedFilter.FilterTypeId + "\" Seltypeid=\"" + primaryAdvancedFilter.selectType + "\" parentname=\"" + primaryAdvancedFilter.Name + "\"  type=\"Sub-Level\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" ChartTypePIT=\"" + secfil.ChartTypePIT + "\" ChartTypeTrend=\"" + secfil.ChartTypeTrend + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" Name=\"" + secfil.Name + "\" class=\"gouptype\" onclick=\"SelecMeasure(this);\">");
                                html3.Append("<div class=\"FilterStringContainerdiv\">");
                                html3.Append("<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + primaryAdvancedFilter.FilterTypeId + "\" parentname=\"" + primaryAdvancedFilter.Name + "\"  id=\"" + secfil.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + secfil.ChartTypePIT + "\" ChartTypeTrend=\"" + secfil.ChartTypeTrend + "\" Name=\"" + secfil.Name + "\">" + secfil.Name + "</div>");
                                html3.Append("<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                                html3.Append("</div>");
                                html3.Append("</li>");
                                html3.Append("</ul>");

                                html4.Append("<ul uniqueid=\"" + secfil.UniqueId + "\" Name=\"" + secfil.Name + "\" style=\"display:none;\">");
                                foreach (SecondaryAdvancedFilter thfil in secfil.SecondaryAdvancedFilterlist)
                                {
                                    html4.Append("<li style=\"white-space:pre-wrap;\" Type=\"" + filtertyle + "\" Seltypeid=\"" + primaryAdvancedFilter.selectType + "\" mainparentname=\"" + _group.GroupName + "\"  FilterType=\"" + thfil.FilterType + "\" ParentDetails=\"" + thfil.ParentDetails + "\" UId=\"" + thfil.FilterTypeId + "|" + thfil.Id + "|" + thfil.ParentId + "\" id=\"" + thfil.Id + "|" + thfil.Id + "\" type=\"Main-Stub\" DBName=\"" + thfil.DBName + "\" UniqueId=\"" + thfil.UniqueId + "\" shopperdbname=\"" + thfil.ShopperDBName + "\" tripsdbname=\"" + thfil.TripsDBName + "\" Name=\"" + thfil.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + thfil.Name + "</li>");

                                    if (!Allsearchitems.Contains(thfil.SearchName.Trim()))
                                    {
                                        searchitems.Add(thfil.UniqueId + "|" + thfil.SearchName + "|" + thfil.DBName.Split('|')[0]);
                                        Allsearchitems.Add(thfil.SearchName.Trim());
                                    }

                                    if (filtertyle.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (!AllshopperSearchItems.Contains(thfil.SearchName.Trim()))
                                        {
                                            shopperSearchItems.Add(thfil.UniqueId + "|" + thfil.SearchName + "|" + thfil.DBName.Split('|')[0]);
                                            AllshopperSearchItems.Add(thfil.SearchName.Trim());
                                        }                                       
                                    }

                                    if (filtertyle.Equals("Visits", StringComparison.OrdinalIgnoreCase) || filtertyle.Equals("Demographics", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (!AlltripsSearchItems.Contains(thfil.SearchName.Trim()))
                                        {
                                            tripsSearchItems.Add(thfil.SearchName + "|" + thfil.DBName.Split('|')[0]);
                                            AlltripsSearchItems.Add(thfil.SearchName.Trim());
                                        }                                      
                                    }
                                }
                                html4.Append("</ul>");
                            }
                            else
                            {
                                if (seclist.IndexOf(secfil) == 0)
                                    html4.Append("<ul uniqueid=\"" + primaryAdvancedFilter.UniqueId + "\" Name=\"" + primaryAdvancedFilter.Name + "\" style=\"display:none;\">");

                                html4.Append("<li style=\"white-space:pre-wrap;\" Type=\"" + filtertyle + "\" Seltypeid=\"" + primaryAdvancedFilter.selectType + "\" mainparentname=\"" + _group.GroupName + "\"  FilterType=\"" + secfil.FilterType + "\" ParentDetails=\"" + secfil.ParentDetails + "\" UId=\"" + secfil.FilterTypeId + "|" + secfil.Id + "|" + secfil.ParentId + "\" id=\"" + secfil.Id + "|" + secfil.Id + "\" type=\"Main-Stub\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" Name=\"" + secfil.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + secfil.Name + "</li>");

                                if (!Allsearchitems.Contains(secfil.SearchName.Trim()))
                                {
                                    searchitems.Add(secfil.UniqueId + "|" + secfil.SearchName + "|" + secfil.DBName.Split('|')[0]);
                                    Allsearchitems.Add(secfil.SearchName.Trim());
                                }


                                if (filtertyle.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!AllshopperSearchItems.Contains(secfil.SearchName.Trim()))
                                    {
                                        shopperSearchItems.Add(secfil.UniqueId + "|" + secfil.SearchName + "|" + secfil.DBName.Split('|')[0]);
                                        AllshopperSearchItems.Add(secfil.SearchName.Trim());
                                    }
                                   
                                }

                                if (filtertyle.Equals("Visits", StringComparison.OrdinalIgnoreCase) || filtertyle.Equals("Demographics", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!AlltripsSearchItems.Contains(secfil.SearchName.Trim()))
                                    {
                                        tripsSearchItems.Add(secfil.UniqueId + "|" + secfil.SearchName + "|" + secfil.DBName.Split('|')[0]);
                                        AlltripsSearchItems.Add(secfil.SearchName.Trim());
                                    }                                   
                                }
                            }
                        }
                        html4.Append("</ul>");
                    }
                    html2.Append("</ul>");
                    _seltype.html1 = html1.ToString();
                    _seltype.html2 = html2.ToString();
                    _seltype.html3 = html3.ToString();
                    _seltype.html4 = html4.ToString();
                    _seltype.html5 = html5.ToString();

                    _seltype.SearchObj = new SearchHTMLEntity();
                    _seltype.SearchObj.SearchItems = searchitems;
                    _seltype.SearchObj.ShopperSearchItems = shopperSearchItems;
                    _seltype.SearchObj.TripsSearchItems = tripsSearchItems;
                    //added by Nagaraju for HTML string
                    //Date: 17-04-2017
                    //_seltype.GroupTypelist = GroupTypelist;
                }
            }
            return SelTypelist;
        }
        //public List<SelTypes> GetMeasureDataEcommerce(int TableNumber)
        //{
        //    string seltype = string.Empty;
        //    List<SelTypes> SelTypelist = new List<DAL.SelTypes>();
        //    SelTypes Seltype = null;
        //    List<GroupType> GroupTypelist = new List<GroupType>();

        //    var table_Objects = (from row in ds.Tables[TableNumber].AsEnumerable()
        //                         select new
        //                         {
        //                             Metric = row["Metric"],
        //                             MetricId = row["MetricId"],
        //                             MetricItem = row["MetricItem"],
        //                             MetricItemId = row["MetricItemId"],
        //                             LevelId = row["LevelId"],
        //                             SelTypeId = row["SelTypeId"],
        //                             SelType = row["SelType"],
        //                             FilterType = row["FilterType"],
        //                             FilterTypeId = row["FilterTypeId"],
        //                             ChartTypePIT = row["ChartTypePIT"],
        //                             ChartTypeTrend = row["ChartTypeTrend"],
        //                             UniqueFilterId = row["UniqueFilterId"],
        //                             ParentId = row["ParentId"],
        //                             SearchName = row["SearchName"]
        //                         }).Distinct().ToList();

        //    foreach (string _seltype in (from row in table_Objects
        //                                 select Convert.ToString(row.SelType)).Distinct().ToList())
        //    {
        //        Seltype = new DAL.SelTypes();
        //        Seltype.SelType = Convert.ToString(_seltype);
        //        SelTypelist.Add(Seltype);
        //    }
        //    foreach (SelTypes _seltype in SelTypelist)
        //    {
        //        GroupTypelist = new List<GroupType>();
        //        foreach (string _group in (from row in table_Objects
        //                                   where Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                   select Convert.ToString(row.FilterType)).Distinct().ToList())
        //        {
        //            GroupType _GroupType = new GroupType();
        //            _GroupType = (from row in table_Objects
        //                          where Convert.ToString(row.FilterType).Equals(_group, StringComparison.OrdinalIgnoreCase)
        //                          && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                          select new GroupType
        //                          {
        //                              GroupName = Convert.ToString(row.FilterType),
        //                              GroupId = Convert.ToString(row.FilterTypeId)
        //                          }).Distinct().FirstOrDefault();
        //            GroupTypelist.Add(_GroupType);
        //        }
        //        foreach (GroupType _group in GroupTypelist)
        //        {
        //            _group.PrimaryAdvancedFilter = new List<PrimaryAdvancedFilter>();
        //            foreach (string measure in (from row in table_Objects
        //                                        where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                        && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                        && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
        //                                        && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                        select Convert.ToString(row.Metric)).Distinct().ToList())
        //            {
        //                var query = (from row in table_Objects
        //                             where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                             && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                              && Convert.ToString(row.MetricItem).Equals(measure, StringComparison.OrdinalIgnoreCase)
        //                             select Convert.ToString(row.MetricItem)).FirstOrDefault();

        //                if (string.IsNullOrEmpty(Convert.ToString(query)))
        //                {
        //                    PrimaryAdvancedFilter pfl = (from row in table_Objects
        //                                                 where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                                  && Convert.ToString(row.Metric).Equals(measure, StringComparison.OrdinalIgnoreCase)
        //                                                 && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                                 && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
        //                                                 && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                                 select new PrimaryAdvancedFilter
        //                                                 {
        //                                                     Id = row.MetricId == null ? 00 : Convert.ToInt16(row.MetricId),
        //                                                     Name = row.Metric == null ? "" : Convert.ToString(row.Metric),
        //                                                     FullName = row.Metric == null ? "" : Convert.ToString(row.Metric),
        //                                                     Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
        //                                                     selectType = row.SelTypeId == null ? "" : Convert.ToString(row.SelTypeId),
        //                                                     FilterType = row.FilterType == null ? "" : Convert.ToString(row.FilterType),
        //                                                     FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //                                                     ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
        //                                                     ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
        //                                                     DBName = Convert.ToString(row.Metric),
        //                                                     UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                                     ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //                                                     SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
        //                                                 }).FirstOrDefault();
        //                    _group.PrimaryAdvancedFilter.Add(pfl);
        //                }
        //            }
        //            foreach (PrimaryAdvancedFilter primaryAdvancedFilter in _group.PrimaryAdvancedFilter)
        //            {
        //                List<SecondaryAdvancedFilter> seclist = (from row in table_Objects
        //                                                         where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                                        && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                                         && Convert.ToString(row.Metric).Equals(Convert.ToString(primaryAdvancedFilter.Name), StringComparison.OrdinalIgnoreCase)
        //                                                         && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                                         select new SecondaryAdvancedFilter
        //                                                         {

        //                                                             Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //                                                             Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                             FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                             MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //                                                             Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
        //                                                             ParentId = row.ParentId == null ? "" : Convert.ToString(primaryAdvancedFilter.Id.ToString()),
        //                                                             DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
        //                                                             UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                                             ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
        //                                                             ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
        //                                                             SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
        //                                                         }).Distinct().ToList();

        //                primaryAdvancedFilter.SecondaryAdvancedFilterlist = seclist;
        //                foreach (SecondaryAdvancedFilter secfil in seclist)
        //                {
        //                    var query = (from row in table_Objects
        //                                 where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                 && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                 && Convert.ToString(row.Metric).Equals(Convert.ToString(secfil.Name), StringComparison.OrdinalIgnoreCase)
        //                                 && Convert.ToString(row.ParentId).Equals(Convert.ToString(secfil.ParentId), StringComparison.OrdinalIgnoreCase)
        //                                 select new SecondaryAdvancedFilter
        //                                 {

        //                                     Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //                                     Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                     FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                     MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //                                     Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
        //                                     ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //                                     DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
        //                                     UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                     ParentDetails = (row.MetricItemId == null || row.MetricItem == null || row.ParentId == null) ? "" : primaryAdvancedFilter.Id + "|" + Convert.ToString(row.MetricItemId) + "|" + Convert.ToString(row.MetricItem) + "|" + Convert.ToString(row.ParentId),
        //                                     ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
        //                                     ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
        //                                     SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
        //                                 }).Distinct().ToList();
        //                    secfil.SecondaryAdvancedFilterlist = new List<SecondaryAdvancedFilter>();
        //                    secfil.SecondaryAdvancedFilterlist = query.ToList();
        //                }
        //            }
        //            _seltype.GroupTypelist = GroupTypelist;
        //        }
        //    }
        //    return SelTypelist;
        //}
        #endregion
        #region Load Time Period Filters
        List<TimePeriod> LoadTimePeriodFiltersFilters(int TableNumber)
        {
            List<TimePeriod> TimePeriodlist = null;
            TimePeriod timePeriod = null;
            if (ds.Tables[TableNumber].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[TableNumber].AsEnumerable()
                                     select new
                                     {
                                         TypeId = row["TypeId"],
                                         PeriodType = row["PeriodType"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         PeriodId = row["PeriodId"],
                                         Period = row["Period"]
                                     }).Distinct().ToList();

                TimePeriodlist = new List<TimePeriod>();
                foreach (String period in (from row in table_Objects select (Convert.ToString(row.PeriodType))).Distinct().ToList())
                {
                    timePeriod = new TimePeriod();
                    timePeriod = (from row in table_Objects
                                  where Convert.ToString(period).Equals(Convert.ToString(row.PeriodType), StringComparison.OrdinalIgnoreCase)
                                  select new TimePeriod
                                  {
                                      Id = Convert.ToInt16(row.TypeId),
                                      Name = Convert.ToString(row.PeriodType),
                                      UniqueId = Convert.ToString(row.UniqueFilterId)
                                  }).FirstOrDefault();

                    timePeriod.TimePeriodlist = (from row in table_Objects
                                                 where Convert.ToString(period).Equals(Convert.ToString(row.PeriodType), StringComparison.OrdinalIgnoreCase)
                                                  select new TimePeriod
                                                  {
                                                      Id = Convert.ToInt16(row.PeriodId),
                                                      Name = Convert.ToString(row.Period),
                                                      UniqueId = Convert.ToString(row.UniqueFilterId)
                                                  }).ToList();

                    timePeriod.Sliderlist = timePeriod.TimePeriodlist.Select(x => x.Name).ToList();

                    TimePeriodlist.Add(timePeriod);
                }
                //foreach (TimePeriod _timePeriod in TimePeriodlist)
                //{
                //    _timePeriod.TimePeriodlist = (from row in table_Objects
                //                                  where Convert.ToString(_timePeriod.Name).Equals(Convert.ToString(row.PeriodType), StringComparison.OrdinalIgnoreCase)
                //                                  select new TimePeriod
                //                                  {
                //                                      Id = Convert.ToInt16(row.PeriodId),
                //                                      Name = Convert.ToString(row.Period),
                //                                      UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                  }).ToList();

                //    _timePeriod.Sliderlist = (from row in table_Objects
                //                              where Convert.ToString(_timePeriod.Name).Equals(Convert.ToString(row.PeriodType), StringComparison.OrdinalIgnoreCase)
                //                              select Convert.ToString(row.Period)).ToList();

                //}
            }
            return TimePeriodlist;
        }
        #endregion
        #region Get Retailer and Beverage string mapping name
        private string Get_Retailer_Beverage_String_Mapping_Name(object tblrow, string metricName)
        {
            Type myType = tblrow.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            DataTable table = new DataTable();
            foreach (PropertyInfo prop in props)
            {
                table.Columns.Add(prop.Name);
            }
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(tblrow, null);
                table.Rows[0][prop.Name] = propValue;
            }
            row = table.Rows[0];

            string mp_name = string.Empty;
            string lavel_name = string.Empty;
            if (row != null && !string.IsNullOrEmpty(Convert.ToString(row["LevelDesc"])))
                lavel_name = Convert.ToString(row["LevelDesc"]).ToLower();

            switch (lavel_name.ToLower())
            {
                case "total shopper":
                case "total":
                case "channel":
                    {
                        if (metricName.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Convert.ToString(row["Metric"]).Equals("Total Shopper", StringComparison.OrdinalIgnoreCase)
                                || Convert.ToString(row["Metric"]).Equals("Total", StringComparison.OrdinalIgnoreCase))
                                mp_name = "Channels|Total";
                            else
                                mp_name = "Channels|" + Convert.ToString(row["Retailer_Shopper_DB_Name"]);
                        }
                        else
                        {
                            if (Convert.ToString(row["Metric"]).Equals("Total Shopper", StringComparison.OrdinalIgnoreCase)
                              || Convert.ToString(row["Metric"]).Equals("Total", StringComparison.OrdinalIgnoreCase))
                                mp_name = "Channels|Total";
                            else
                                mp_name = "Channels|" + Convert.ToString(row["Retailer_Trips_DB_Name"]);
                        }
                        break;
                    }
                case "retailer":
                    {
                        if (metricName.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                            mp_name = "Retailers|" + Convert.ToString(row["Retailer_Shopper_DB_Name"]);
                        else
                            mp_name = "Retailers|" + Convert.ToString(row["Retailer_Trips_DB_Name"]);
                        break;
                    }
                case "net":
                    {
                        if (metricName.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                            mp_name = "RetailerNet|" + Convert.ToString(row["Retailer_Shopper_DB_Name"]);
                        else
                            mp_name = "RetailerNet|" + Convert.ToString(row["Retailer_Trips_DB_Name"]);
                        break;
                    }
                case "category":
                case "category||":
                case "categorynet||":
                    {
                        if (metricName.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Convert.ToString(row["MetricItem"]).Equals("Total Trips", StringComparison.OrdinalIgnoreCase))
                                mp_name = "totalshopper||totalshopper";
                            else if (Convert.ToString(row["MetricItem"]).Equals("Total Beverage Trips", StringComparison.OrdinalIgnoreCase))
                                mp_name = "Category||Total";
                            else
                                mp_name = "Category||" + Convert.ToString(row["Beverage_Shopper_DB_Name"]);
                        }
                        else
                        {
                            if (Convert.ToString(row["MetricItem"]).Equals("Total Trips", StringComparison.OrdinalIgnoreCase))
                                mp_name = "totaltrip||totaltrip";
                            else if (Convert.ToString(row["MetricItem"]).Equals("Total Beverage Trips", StringComparison.OrdinalIgnoreCase))
                                mp_name = "Category||Total";
                            else
                                mp_name = "Category||" + Convert.ToString(row["Beverage_Trips_DB_Name"]);
                        }
                        break;
                    }
                case "categorynet":
                    {
                        if (metricName.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Convert.ToString(row["MetricItem"]).Equals("Total Beverage Trips", StringComparison.OrdinalIgnoreCase))
                                mp_name = "CategoryNet||Total";
                            else
                                mp_name = "CategoryNet||" + Convert.ToString(row["Beverage_Shopper_DB_Name"]);
                        }
                        else
                        {
                            if (Convert.ToString(row["MetricItem"]).Equals("Total Beverage Trips", StringComparison.OrdinalIgnoreCase))
                                mp_name = "CategoryNet||Total";
                            else
                                mp_name = "CategoryNet||" + Convert.ToString(row["Beverage_Trips_DB_Name"]);
                        }
                        break;
                    }
                case "brand||":
                case "brand":
                    {
                        if (metricName.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                            mp_name = "Brands|" + Convert.ToString(row["Beverage_Shopper_DB_Name_Brand"]) + "|" + Convert.ToString(row["Beverage_Shopper_DB_Name"]);
                        else
                            mp_name = "Brand|" + Convert.ToString(row["Beverage_Trips_DB_Name2"]) + "|" + Convert.ToString(row["Beverage_Trips_DB_Name"]) + " (" + Convert.ToString(row["Beverage_Trips_DB_Name2"]) + ")";
                        break;
                    }
                case "brandnet||":
                case "brandnet":
                    {
                        if (metricName.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                            mp_name = "BrandNet||" + Convert.ToString(row["Beverage_Shopper_DB_Name"]);
                        else
                            mp_name = "BrandNet||" + Convert.ToString(row["Beverage_Trips_DB_Name"]);
                        break;
                    }
            }
            return mp_name;
        }
        #endregion
        #region Load Channel Or Category Filters
        string IsItemExist(string item,List<string> array)
        {
            string status = "false";
            for(var i=0;i<array.Count;i++)
            {
        
            if (array[i].Split('|')[1].ToString() == item)
            {
               status = "true";
            }
            }
         return status;
       }
        RetailerStringList RetailerStringFunction(List<RetailerOrBrand> Retailerlist, string DBName, string Name, string Id)
        {
            RetailerStringList obj1 = new RetailerStringList();
            List<StringBuilder> sbList = new List<StringBuilder>();
            StringBuilder sb1 = null;
            sb1 = new StringBuilder();
            StringBuilder sb11 = null;
            sb11 = new StringBuilder();
            List<string> SearchList = new List<string>();
            List<string> PrioritySearchList = new List<string>();

            var index = 0; var sindex = 0; var ssindex = 0;
            for (var j = 0; j < Retailerlist.Count; j++)
            {
                var obj = Retailerlist[j];
                if (obj.LevelId == 3)
                {
                    if (obj.PriorityId == "1")
                    {
                        if (index == 0)
                        {
                            sb1.Append("<div class=\"RetailerOrBrand\" id=\"" + obj.UniqueId + "\" Name=\"" + Name + "\" DBName=\"" + DBName + "\" style=\"display:none;\"><ul>");
                            index++;
                        }
                        sb1.Append("<li class=\"Comparison\" parentLevelId=\"" + Id + "\" id=\"" + obj.UniqueId + "\" Name=\"" + obj.Name + "\" DBName=\"" + obj.DBName + "\" UniqueId=\"" + obj.UniqueId + "\" shopperdbname=\"" + obj.ShopperDBName + "\" tripsdbname=\"" + obj.TripsDBName + "\" onclick=\"SelectComparison(this);\" PriorityId=\"" + obj.PriorityId + "\">" + obj.Name + "</li>");
                        //if (!IsItemExist(object.Name, AllPriorityRetailers))
                        //    AllPriorityRetailers.push(object.UniqueId + "|" + object.Name);
                        if ((IsItemExist(obj.Name.ToString(), PrioritySearchList) == "false"))
                            PrioritySearchList.Add(obj.UniqueId + "|" + obj.Name);
                    }
                    else
                    {
                        if (sindex == 0)
                        {
                            sb11.Append("<div class=\"RetailerOrBrand\" id=\"" + obj.UniqueId + "\" Name=\"" + Name + "\" DBName=\"" + DBName + "\" style=\"display:none;\"><ul>");
                            sindex++;
                        }
                        sb11.Append("<li class=\"Comparison\" parentLevelId=\"" + Id + "\" id=\"" + obj.UniqueId + "\" Name=\"" + obj.Name + "\" DBName=\"" + obj.DBName + "\" UniqueId=\"" + obj.UniqueId + "\" shopperdbname=\"" + obj.ShopperDBName + "\" tripsdbname=\"" + obj.TripsDBName + "\" onclick=\"SelectComparison(this);\" PriorityId=\"" + obj.PriorityId + "\">" + obj.Name + "</li>");

                    }
                }
                else
                {
                    if (ssindex == 0)
                        sb1.Append("<div class=\"RetailerOrBrand\" id=\"" + obj.UniqueId + "\" Name=\"" + Name + "\" DBName=\"" + DBName + "\" style=\"display:none;\"><ul>");
                    sb1.Append("<li class=\"Comparison\" parentLevelId=\"" + Id + "\" id=\"" + obj.UniqueId + "\" Name=\"" + obj.Name + "\" DBName=\"" + obj.DBName + "\" UniqueId=\"" + obj.UniqueId + "\" shopperdbname=\"" + obj.ShopperDBName + "\" tripsdbname=\"" + obj.TripsDBName + "\" onclick=\"SelectComparison(this);\" PriorityId=\"" + (obj.TripsDBName.ToLower().IndexOf("retailernet") > -1 ? "1" : obj.PriorityId) + "\">" + obj.Name + "</li>");
                    //if (!IsItemExist(object.Name, AllPriorityRetailers))
                    //    AllPriorityRetailers.push(object.UniqueId + "|" + object.Name);
                    if ((IsItemExist(obj.Name.ToString(), PrioritySearchList) == "false"))
                            PrioritySearchList.Add(obj.UniqueId + "|" + obj.Name);
                    ssindex++;
                }
                //if (!IsItemExist(object.Name, AllRetailers) && object.isSelectable != "false")
                //    AllRetailers.push(object.UniqueId + "|" + object.Name);
                if ((IsItemExist(obj.Name.ToString(), SearchList) == "false"))
                            SearchList.Add(obj.UniqueId + "|" + obj.Name);
            }

            if (sindex > 0)
            {
                sb1.Append("</ul></div>");
                sb11.Append("</ul></div>");
                var html = "";
                html += "<div Name=\"" + Name + "\" class=\"priorityclass\" style=\"\">PRIORITY</div>";
                html += sb1.ToString();
                sb1 = new StringBuilder();
                sb1.Append(html);

                //if (currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive" && currentpage != "hdn-analysis-crossretailerfrequencies" && currentpage != "hdn-analysis-crossretailerimageries") 
                {
                    var sHtml = "";
                    sHtml += "<div Name=\"" + Name + "\"  class=\"priorityclass\" style=\"\">RETAILERS</div>";
                    sHtml += sb11.ToString();
                    sb11 = new StringBuilder();
                    sb11.Append(sHtml);
                }
            }
            else
            {
                sb1.Append("</ul></div>");
            }

            sbList.Add(sb1);
            sbList.Add(sb11);
            obj1.HTML_StringBuilder = new List<StringBuilder>();
            obj1.HTML_StringBuilder.Add(sb1);
            obj1.HTML_StringBuilder.Add(sb11);
            obj1.SearchItems = new List<string>();
            obj1.SearchItems = SearchList;
            obj1.PrioritySearchItems = new List<string>();
            obj1.PrioritySearchItems = PrioritySearchList;
            return obj1;
        }
        string RetailerImagePosition(string RetailerName) {
            switch(RetailerName){
                case "Total" : return "-932px -678px;";
                               break;
                case "Supermarket/Grocery" : return "-1414px -360px;";
                               break;
                case "Convenience" : return "-1456px -360px;";
                               break;
                case "Drug" : return "-1646px -360px;";
                               break;
                case "Dollar" : return "-1603px -360px;";
                               break;
                case "Club" : return "-1545px -360px;";
                               break;
                case "Mass Merc." : return "-1498px -360px;";
                               break;
                case "Supercenter" : return "-1692px -360px;";
                               break;
                case "Corporate Nets" : return "4px -678px;";
                               break;
                case "Channel Nets": return "-1490px -672px";
                    break;
            }
            return "";
          }
       
        Channel LoadChannelOrCategoryFilters()
        {
            Channel Channel = new Channel();
            ChannelOrCategory ChannelOrCategory = null;
            ChannelOrCategoryLavel ChannelOrCategoryLavel = null;
            RetailersFilterlist RetailersFilterlist = null;
            List<string> SearchList = new List<string>();
            List<string> PrioritySearchList = new List<string>();

            StringBuilder sb, sb1, sb11, sb2, sb22, sb3, sb33, sb4, sb44 = null;
            sb = new StringBuilder();
            sb1 = new StringBuilder();
            sb11 = new StringBuilder();
            sb2 = new StringBuilder();
            sb22 = new StringBuilder();
            sb3 = new StringBuilder();
            sb33 = new StringBuilder();
            sb4 = new StringBuilder();
            sb44 = new StringBuilder();

            if (ds.Tables[2].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[2].AsEnumerable()
                                     select new
                                     {
                                         ChannelId = row["ChannelId"],
                                         Channel = row["Channel"],
                                         IsSelectable = row["IsSelectable"],
                                         LevelId = row["LevelId"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         IsPriority = row["IsPriority"],
                                         LevelDesc = row["LevelDesc"],
                                         Retailer_Shopper_DB_Name = row["Retailer_Shopper_DB_Name"],
                                         Retailer_Trips_DB_Name = row["Retailer_Trips_DB_Name"]
                                     }).Distinct().ToList();

                Channel.Lavels = (from row in table_Objects
                                  orderby row.LevelId
                                  select Convert.ToString(row.LevelId)).Distinct().ToList();

                //sb.Append("<div id=\"ChannelOrCategoryContent\" class=\"Lavel\" style=\"height: 95%;\">");

                Channel.ChannelOrCategorylist = new List<ChannelOrCategory>();
                foreach (String channel in (from row in table_Objects select (Convert.ToString(row.Channel))).Distinct().ToList())
                {
                    ChannelOrCategory = new ChannelOrCategory();
                    ChannelOrCategory = (from row in table_Objects
                                         where Convert.ToString(channel).Equals(Convert.ToString(row.Channel), StringComparison.OrdinalIgnoreCase)
                                         select new ChannelOrCategory
                                         {
                                             Id = Convert.ToInt16(row.ChannelId),
                                             Name = Convert.ToString(row.Channel),
                                             FullName = Convert.ToString(row.Channel),
                                             ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                                             TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                                             TopPosition = GetChannelTopPosition(Convert.ToString(row.Channel)),
                                             BottomPosition = GetChannelBottomPosition(Convert.ToString(row.Channel)),
                                             IsSelectable = Convert.ToBoolean(row.IsSelectable),
                                             UniqueId = Convert.ToString(row.UniqueFilterId)
                                         }).FirstOrDefault();

                    

                    //sb.Append("</div>");

                    ChannelOrCategory.ChannelOrCategoryLavel = new List<ChannelOrCategoryLavel>();

                    var i = 0;
                    Dictionary<string, string> sLevelDictinary = new Dictionary<string, string>();
                    foreach (string lavel in (from row in table_Objects
                                              where !Convert.ToString(ChannelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(ChannelOrCategory.Name).Equals(Convert.ToString(row.Channel), StringComparison.OrdinalIgnoreCase)
                                              orderby row.LevelId
                                              select Convert.ToString(row.LevelId)).Distinct().ToList())
                    {
                        sLevelDictinary.Add(lavel.ToString(),i.ToString());
                        i++;
                    }

                    foreach (string lavel in (from row in table_Objects
                                              where !Convert.ToString(ChannelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(ChannelOrCategory.Name).Equals(Convert.ToString(row.Channel), StringComparison.OrdinalIgnoreCase)
                                              orderby row.LevelId
                                              select Convert.ToString(row.LevelId)).Distinct().ToList())
                    {
                        ChannelOrCategoryLavel = new ChannelOrCategoryLavel();
                        ChannelOrCategoryLavel.Lavel = lavel;

                        List<RetailerOrBrand> Retailerlist = (from row in table_Objects
                                                              where !Convert.ToString(ChannelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                              && Convert.ToString(ChannelOrCategory.Name).Equals(Convert.ToString(row.Channel), StringComparison.OrdinalIgnoreCase)
                                                               && ChannelOrCategoryLavel.Lavel == Convert.ToString(row.LevelId)
                                                              select new RetailerOrBrand
                                                              {
                                                                  Id = Convert.ToInt16(row.MetricId),
                                                                  LevelId = Convert.ToInt16(row.LevelId),
                                                                  Name = Convert.ToString(row.Metric),
                                                                  FullName = Convert.ToString(row.Metric),
                                                                  ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                                                                  TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                                                                  UniqueId = Convert.ToString(row.UniqueFilterId),
                                                                  PriorityId = Convert.ToString(row.IsPriority)
                                                              }).ToList();
                        ChannelOrCategoryLavel.LavelRetailerlist = Retailerlist;

                        var lev = sLevelDictinary[lavel.ToString()];
                        if (lev == "0")
                        {
                            RetailerStringList sblist = new RetailerStringList();
                            sblist = RetailerStringFunction(Retailerlist, ChannelOrCategory.DBName.ToMyString(), ChannelOrCategory.Name.ToMyString(), ChannelOrCategory.Id.ToString());
                            sb1.Append(sblist.HTML_StringBuilder[0]);
                            sb11.Append(sblist.HTML_StringBuilder[1]);
                            SearchList.AddRange(sblist.SearchItems);
                            PrioritySearchList.AddRange(sblist.PrioritySearchItems);
                        }
                        else if (lev == "1")
                        {
                            RetailerStringList sblist = new RetailerStringList();
                            sblist = RetailerStringFunction(Retailerlist, ChannelOrCategory.DBName.ToMyString(), ChannelOrCategory.Name.ToMyString(), ChannelOrCategory.Id.ToString());
                            sb2.Append(sblist.HTML_StringBuilder[0]);
                            sb22.Append(sblist.HTML_StringBuilder[1]);
                            SearchList.AddRange(sblist.SearchItems);
                            PrioritySearchList.AddRange(sblist.PrioritySearchItems);
                        }
                        else if (lev == "2")
                        {
                            RetailerStringList sblist = new RetailerStringList();
                            sblist = sblist = RetailerStringFunction(Retailerlist, ChannelOrCategory.DBName.ToMyString(), ChannelOrCategory.Name.ToMyString(), ChannelOrCategory.Id.ToString());
                            sb3.Append(sblist.HTML_StringBuilder[0]);
                            sb33.Append(sblist.HTML_StringBuilder[1]);
                            SearchList.AddRange(sblist.SearchItems);
                            PrioritySearchList.AddRange(sblist.PrioritySearchItems);
                        }
                        else if (lev == "3")
                        {
                            RetailerStringList sblist = new RetailerStringList();
                            sblist = sblist = RetailerStringFunction(Retailerlist, ChannelOrCategory.DBName.ToMyString(), ChannelOrCategory.Name.ToMyString(), ChannelOrCategory.Id.ToString());
                            sb4.Append(sblist.HTML_StringBuilder);
                            sb44.Append(sblist.HTML_StringBuilder[1]);
                            SearchList.AddRange(sblist.SearchItems);
                            PrioritySearchList.AddRange(sblist.PrioritySearchItems);
                        }
                            ChannelOrCategory.ChannelOrCategoryLavel.Add(ChannelOrCategoryLavel);
                    }
                    sb.Append("<li>");
                    sb.Append("<div class=\"FilterStringContainerdiv\">");
                    var sImageClassName = RetailerImagePosition(ChannelOrCategory.Name.ToString());
                    if (sImageClassName == "")
                        sb.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:\"></span>");
                    else
                        sb.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>");

                    //sb.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:\"></span>");


                    sb.Append("<span id=\"" + ChannelOrCategory.UniqueId + "\" type=\"Main-Stub\" priorityid=\"1\" Name=\"" + ChannelOrCategory.Name + "\" isselectable=\"" + ChannelOrCategory.IsSelectable + "\" DBName=\"" + ChannelOrCategory.DBName + "\" UniqueId=\"" + ChannelOrCategory.UniqueId + "\" shopperdbname=\"" + ChannelOrCategory.ShopperDBName + "\" tripsdbname=\"" + ChannelOrCategory.TripsDBName + "\" class=\"Comparison\" onclick=\"SelectComparison(this);\">" + ChannelOrCategory.Name + "</span>");
                    sb.Append("<div class=\"ArrowContainerdiv\"><span Name=\"" + ChannelOrCategory.Name + "\" lavels=\"" + ChannelOrCategory.ChannelOrCategoryLavel.Count + "\" class=\"sidearrw\" onclick=\"DisplayComparisonRetailer(this);\"></span></div>");
                    //if (!IsItemExist(object.Name, AllRetailers) && object.isSelectable != "false")
                    //    AllRetailers.push(object.UniqueId + "|" + object.Name);

                    if ((IsItemExist(ChannelOrCategory.Name.ToString(), SearchList) == "false") && ChannelOrCategory.IsSelectable == true)
                    SearchList.Add(ChannelOrCategory.UniqueId + "|" + ChannelOrCategory.Name);

                    sb.Append("</div>");
                    sb.Append("</li>");

                    Channel.ChannelOrCategorylist.Add(ChannelOrCategory);
                }

                var ssHtml = "";
                ssHtml += "<div id=\"ChannelOrCategoryContent\" class=\"Lavel\" style=\"\"><ul>" + sb + "</ul></div>";
                ssHtml += "<div class=\"Retailer Lavel Lavel0 Sub-Lavel\" style=\"display: none;\">" + sb1 + sb11 + "</div>";
                ssHtml += "<div class=\"Retailer Lavel Lavel1 Sub-Lavel\" style=\"display: none;\">" + sb2 + sb22 +"</ul></div></div>";
                ssHtml += "<div class=\"Retailer Lavel Lavel2 Sub-Lavel\" style=\"display: none;\">" + sb3 + sb33 +"</ul></div></div>";
                ssHtml += "<div class=\"Retailer Lavel Lavel3 Sub-Lavel\" style=\"display: none;\">" + sb4 + sb44 +"</ul></div></div>";

                RetailersFilterlist = new RetailersFilterlist();
                RetailersFilterlist.SearchObj = null;
                RetailersFilterlist.SearchObj = new SearchHTMLEntity();
                RetailersFilterlist.SearchObj.HTML_String = "";
                RetailersFilterlist.SearchObj.HTML_String = ssHtml.ToString();
                RetailersFilterlist.SearchObj.SearchItems = SearchList.ToList();
                RetailersFilterlist.SearchObj.PrioritySearchItems = PrioritySearchList.ToList();
                Channel.RetailersFilterlist = RetailersFilterlist;

                //foreach (ChannelOrCategory _channelOrCategory in Channel.ChannelOrCategorylist)
                //{
                //    _channelOrCategory.ChannelOrCategoryLavel = new List<ChannelOrCategoryLavel>();
                //    foreach (string lavel in (from row in table_Objects
                //                              where !Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                              && Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Channel), StringComparison.OrdinalIgnoreCase)
                //                              orderby row.LevelId
                //                              select Convert.ToString(row.LevelId)).Distinct().ToList())
                //    {
                //        ChannelOrCategoryLavel = new ChannelOrCategoryLavel();
                //        ChannelOrCategoryLavel.Lavel = lavel;
                //        _channelOrCategory.ChannelOrCategoryLavel.Add(ChannelOrCategoryLavel);
                //    }

                //    foreach (ChannelOrCategoryLavel _channelOrCategoryLavel in _channelOrCategory.ChannelOrCategoryLavel)
                //    {
                //        List<RetailerOrBrand> Retailerlist = (from row in table_Objects
                //                                              where !Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                              && Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Channel), StringComparison.OrdinalIgnoreCase)
                //                                               && _channelOrCategoryLavel.Lavel == Convert.ToString(row.LevelId)
                //                                              select new RetailerOrBrand
                //                                              {
                //                                                  Id = Convert.ToInt16(row.MetricId),
                //                                                  LevelId = Convert.ToInt16(row.LevelId),
                //                                                  Name = Convert.ToString(row.Metric),
                //                                                  FullName = Convert.ToString(row.Metric),
                //                                                  ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                //                                                  TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                //                                                  UniqueId = Convert.ToString(row.UniqueFilterId),
                //                                                  PriorityId = Convert.ToString(row.IsPriority)
                //                                              }).ToList();
                //        _channelOrCategoryLavel.LavelRetailerlist = Retailerlist;
                //    }
                //}
            }
            //added by Nagaraju for HTML string
            //Date: 17-04-2017
            Channel.ChannelOrCategorylist = new List<ChannelOrCategory>();
            return Channel;
        }
        #endregion
        #region Load Advanced Filters Demographics
        List<List<PrimaryAdvancedFilter>> LoadAdvancedFilters()
        {
            List<List<PrimaryAdvancedFilter>> AdvancedFilterLists = null;
            List<PrimaryAdvancedFilter> PrimaryAdvancedFilterlist = null;
            List<PrimaryAdvancedFilter> VisitAdvancedFilterlist = null;
            PrimaryAdvancedFilter PrimaryAdvancedFilter = null;
            PrimaryAdvancedFilter VisitAdvancedFilter = null;

            if (ds.Tables[4].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[4].AsEnumerable()
                                     select new
                                     {
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         LevelId = row["LevelId"],
                                         FilterTypeId = row["FilterTypeId"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         ParentId = row["ParentId"]
                                     }).Distinct().ToList();

                AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
                PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                VisitAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                foreach (String Metric in (from row in table_Objects where (Convert.ToString(row.FilterTypeId) == "1") select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    PrimaryAdvancedFilter = new PrimaryAdvancedFilter();
                    PrimaryAdvancedFilter = (from row in table_Objects
                                             where ((Convert.ToString(Metric).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)) && (Convert.ToString(row.FilterTypeId) == "1"))
                                             select new PrimaryAdvancedFilter
                                             {
                                                 Id = Convert.ToInt16(row.MetricId),
                                                 Name = Convert.ToString(row.Metric),
                                                 FullName = Convert.ToString(row.Metric),
                                                 Position = GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
                                                 ParentId = Convert.ToString(row.ParentId),
                                                 Level = Convert.ToString(row.LevelId),
                                                 UniqueId = Convert.ToString(row.UniqueFilterId)
                                             }).FirstOrDefault();
                    if (PrimaryAdvancedFilter != null)
                    {
                        PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                             where !Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                              && Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                              && Convert.ToString(PrimaryAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                              select new SecondaryAdvancedFilter
                                                                              {
                                                                                  Id = Convert.ToString(row.MetricItemId),
                                                                                  Name = Convert.ToString(row.MetricItem),
                                                                                  FullName = Convert.ToString(row.MetricItem),
                                                                                  MetricId = Convert.ToString(row.MetricId),
                                                                                  ParentId = Convert.ToString(row.ParentId),
                                                                                  DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                                  UniqueId = Convert.ToString(row.UniqueFilterId)
                                                                              }).ToList();

                        PrimaryAdvancedFilterlist.Add(PrimaryAdvancedFilter);
                    }                    

            }

                foreach (String Metric in (from row in table_Objects where (Convert.ToString(row.FilterTypeId) == "3") select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    VisitAdvancedFilter = new PrimaryAdvancedFilter();

                    VisitAdvancedFilter = (from row in table_Objects
                                           where ((Convert.ToString(Metric).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)) && (Convert.ToString(row.FilterTypeId) == "3"))
                                           select new PrimaryAdvancedFilter
                                           {
                                               Id = Convert.ToInt16(row.MetricId),
                                               Name = Convert.ToString(row.Metric),
                                               FullName = Convert.ToString(row.Metric),
                                               Position = GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
                                               ParentId = Convert.ToString(row.ParentId),
                                               Level = Convert.ToString(row.LevelId),
                                               UniqueId = Convert.ToString(row.UniqueFilterId)
                                           }).FirstOrDefault();

                    if (VisitAdvancedFilter != null)
                    {
                        VisitAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                           //where !Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                           where Convert.ToString(VisitAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                           && Convert.ToString(VisitAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                           select new SecondaryAdvancedFilter
                                                                           {
                                                                               Id = Convert.ToString(row.MetricItemId),
                                                                               Name = Convert.ToString(row.MetricItem),
                                                                               FullName = Convert.ToString(row.MetricItem),
                                                                               MetricId = Convert.ToString(row.MetricId),
                                                                               ParentId = Convert.ToString(row.ParentId),
                                                                               DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                               UniqueId = Convert.ToString(row.UniqueFilterId)
                                                                           }).ToList();

                        VisitAdvancedFilterlist.Add(VisitAdvancedFilter);
                    }
                }

                //foreach (PrimaryAdvancedFilter _PrimaryAdvancedFilter in PrimaryAdvancedFilterlist)
                //{
                //    _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                //                                                          where !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                          && Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                          && Convert.ToString(_PrimaryAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                //                                                          select new SecondaryAdvancedFilter
                //                                                          {
                //                                                              Id = Convert.ToString(row.MetricItemId),
                //                                                              Name = Convert.ToString(row.MetricItem),
                //                                                              FullName = Convert.ToString(row.MetricItem),
                //                                                              MetricId = Convert.ToString(row.MetricId),
                //                                                              ParentId = Convert.ToString(row.ParentId),
                //                                                              DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                //                                                              UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                                          }).ToList();

                //}

                //foreach (PrimaryAdvancedFilter _VisitsAdvancedFilter in VisitAdvancedFilterlist)
                //{
                //    _VisitsAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                //                                                         //where !Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                         where Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                         && Convert.ToString(_VisitsAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                //                                                         select new SecondaryAdvancedFilter
                //                                                         {
                //                                                             Id = Convert.ToString(row.MetricItemId),
                //                                                             Name = Convert.ToString(row.MetricItem),
                //                                                             FullName = Convert.ToString(row.MetricItem),
                //                                                             MetricId = Convert.ToString(row.MetricId),
                //                                                             ParentId = Convert.ToString(row.ParentId),
                //                                                             DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                //                                                             UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                                         }).ToList();

                //}

                AdvancedFilterLists.Add(PrimaryAdvancedFilterlist);
                AdvancedFilterLists.Add(VisitAdvancedFilterlist);
            }
            return AdvancedFilterLists;
        }

        AdvFilterlist LoadAdvancedFiltersString(out List<List<PrimaryAdvancedFilter>> AdvancedFilterLists)
        {
            AdvFilterlist AdvFilter = null;
            //List<List<PrimaryAdvancedFilter>> AdvancedFilterLists = null;
            AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
            List<PrimaryAdvancedFilter> PrimaryAdvancedFilterlist = null;
            List<PrimaryAdvancedFilter> VisitAdvancedFilterlist = null;
            PrimaryAdvancedFilter PrimaryAdvancedFilter = null;
            PrimaryAdvancedFilter VisitAdvancedFilter = null;
            List<string> SearchList = new List<string>();
            StringBuilder sb = null;
            var index = 0;
            sb = new StringBuilder();
            StringBuilder sbSub = null;
            var indexSub = 0;
            sbSub = new StringBuilder();

            if (ds.Tables[4].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[4].AsEnumerable()
                                     select new
                                     {
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         LevelId = row["LevelId"],
                                         FilterTypeId = row["FilterTypeId"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         ParentId = row["ParentId"]
                                     }).Distinct().ToList();

                AdvFilter = new AdvFilterlist();
                AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
                PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                VisitAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                foreach (String Metric in (from row in table_Objects where (Convert.ToString(row.FilterTypeId) == "1") select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    PrimaryAdvancedFilter = new PrimaryAdvancedFilter();
                    PrimaryAdvancedFilter = (from row in table_Objects
                                             where ((Convert.ToString(Metric).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)) && (Convert.ToString(row.FilterTypeId) == "1"))
                                             select new PrimaryAdvancedFilter
                                             {
                                                 Id = Convert.ToInt16(row.MetricId),
                                                 Name = Convert.ToString(row.Metric),
                                                 FullName = Convert.ToString(row.Metric),
                                                 Position = GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
                                                 ParentId = Convert.ToString(row.ParentId),
                                                 Level = Convert.ToString(row.LevelId),
                                                 UniqueId = Convert.ToString(row.UniqueFilterId)
                                             }).FirstOrDefault();


                    var obj = PrimaryAdvancedFilter;
                    if (index == 0)
                    {
                        sb.Append("<ul>");
                    }

                    if (obj.Level.ToString() == "1")
                    {
                        sb.Append("<li style='display:table;'>");
                        sb.Append("<div onclick='DisplaySecondaryDemoFilter(this);' Name='" + obj.Name + "' id='" + obj.Id + "' class='lft-popup-ele FilterStringContainerdiv' style=''><span class='lft-popup-ele-label' id='" + obj.Id + "' data-val='" + obj.Name + "' data-parent='' data-isselectable='true'>" + obj.Name + "</span><div class=\"ArrowContainerdiv\"><span class='lft-popup-ele-next sidearrw'></span></div></div>");

                        sb.Append("</li>");
                        index++;
                    }

                    indexSub = 0;
                    if (PrimaryAdvancedFilter != null)
                    {
                        PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                             where !Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                              && Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                              && Convert.ToString(PrimaryAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                             select new SecondaryAdvancedFilter
                                                                             {
                                                                                 Id = Convert.ToString(row.MetricItemId),
                                                                                 Name = Convert.ToString(row.MetricItem),
                                                                                 FullName = Convert.ToString(row.MetricItem),
                                                                                 MetricId = Convert.ToString(row.MetricId),
                                                                                 ParentId = Convert.ToString(row.ParentId),
                                                                                 DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                                 UniqueId = Convert.ToString(row.UniqueFilterId)
                                                                             }).ToList();

                        if (indexSub == 0)
                            sbSub.Append("<div class='DemographicList' id='" + PrimaryAdvancedFilter.Id + "' Name='" + PrimaryAdvancedFilter.Name + "' FullName='" + PrimaryAdvancedFilter.FullName + "' style='overflow-y:auto;display:none;'><ul>");

                        for (var i = 0; i < PrimaryAdvancedFilter.SecondaryAdvancedFilterlist.Count; i++)
                        {
                            var obj1 = PrimaryAdvancedFilter.SecondaryAdvancedFilterlist[i];
                            sbSub.Append("<div onclick=\"SelectDemographic(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + obj1.FullName + "\" DBName=\"" + obj1.DBName + "\" isGeography=\"" + obj1.isGeography + "\" UniqueId=\"" + obj1.UniqueId + "\" shopperdbname=\"" + obj1.ShopperDBName + "\" tripsdbname=\"" + obj1.TripsDBName + "\" data-id=\"" + obj1.Id + "\" id=\"" + obj1.Id + "-" + obj1.MetricId + "-" + obj1.ParentId + "\" Name=\"" + obj1.Name.ToLower() + "\" parent=\"" + obj1.ParentId + "\" ParentLevelId=\"" + PrimaryAdvancedFilter.Id.ToString().Trim() + "\" ParentLevelName=\"" + PrimaryAdvancedFilter.Name.ToString().Trim() + "\" data-isselectable=\"true\">" + obj1.Name + "</span></div>");
                            // AllDemographics.push(object.UniqueId + "|" + object.Name);
                            if ((IsItemExist(obj.Name.ToString(), SearchList) == "false"))
                                SearchList.Add(obj1.UniqueId + "|" + obj1.Name + "|" + PrimaryAdvancedFilter.Name.ToString().Trim());
                        }

                        PrimaryAdvancedFilterlist.Add(PrimaryAdvancedFilter);
                    }
                    sbSub.Append("</ul></div>");
                }

                sb.Append("<li style='display:table;'><div onclick='DisplaySecondaryDemoFilter(this);' name='Geography' id='100' class='lft-popup-ele FilterStringContainerdiv' style=''><span class='lft-popup-ele-label' id='100' data-val='Geography' data-parent='' data-isselectable='true'>Geography</span><div class=\"ArrowContainerdiv\"><span class='lft-popup-ele-next sidearrw'></span></div></div></li>");
                sb.Append("</ul>");
                AdvancedFilterLists.Add(PrimaryAdvancedFilterlist);
                AdvancedFilterLists.Add(VisitAdvancedFilterlist);
                AdvFilter.StringList = null;
                AdvFilter.StringList = new RetailerStringList();
                AdvFilter.StringList.HTML_String = new List<string>();
                AdvFilter.StringList.HTML_String.Add(sb.ToString());
                AdvFilter.StringList.HTML_String.Add(sbSub.ToString());
                AdvFilter.StringList.SearchItems = new List<string>();
                AdvFilter.StringList.SearchItems = SearchList.ToList();
            }           
            return AdvFilter;
        }
        VisitsAdvFilterlist LoadVisitsAdvancedFiltersString(out List<List<PrimaryAdvancedFilter>> AdvancedFilterLists,int tblno)
        {
            VisitsAdvFilterlist VisitsAdvFilter = null;
            //List<List<PrimaryAdvancedFilter>> AdvancedFilterLists = null;
            AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
            List<PrimaryAdvancedFilter> PrimaryAdvancedFilterlist = null;
            List<PrimaryAdvancedFilter> VisitAdvancedFilterlist = null;
            PrimaryAdvancedFilter PrimaryAdvancedFilter = null;
            PrimaryAdvancedFilter VisitAdvancedFilter = null;
            List<string> SearchList = new List<string>();
            StringBuilder sb = null;
            var index = 0;
            sb = new StringBuilder();
            StringBuilder sbSub = null;
            var indexSub = 0;
            sbSub = new StringBuilder();

            if (ds.Tables[tblno].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[tblno].AsEnumerable()
                                     select new
                                     {
                                         Metric = Convert.ToString(row["Metric"]).Trim(),
                                         MetricId = row["MetricId"],
                                         MetricItem = Convert.ToString(row["MetricItem"]).Trim(),
                                         MetricItemId = row["MetricItemId"],
                                         LevelId = row["LevelId"],
                                         FilterTypeId = row["FilterTypeId"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         ParentId = row["ParentId"]
                                     }).Distinct().ToList();

                VisitsAdvFilter = new VisitsAdvFilterlist();
                AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
                PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                VisitAdvancedFilterlist = new List<PrimaryAdvancedFilter>();

                foreach (String Metric in (from row in table_Objects where (Convert.ToString(row.FilterTypeId) == "3") select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    VisitAdvancedFilter = new PrimaryAdvancedFilter();

                    VisitAdvancedFilter = (from row in table_Objects
                                           where ((Convert.ToString(Metric).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)) && (Convert.ToString(row.FilterTypeId) == "3"))
                                           select new PrimaryAdvancedFilter
                                           {
                                               Id = Convert.ToInt16(row.MetricId),
                                               Name = Convert.ToString(row.Metric),
                                               FullName = Convert.ToString(row.Metric),
                                               Position = GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
                                               ParentId = Convert.ToString(row.ParentId),
                                               Level = Convert.ToString(row.LevelId),
                                               UniqueId = Convert.ToString(row.UniqueFilterId)
                                           }).FirstOrDefault();

                    var obj = VisitAdvancedFilter;
                    if (index == 0)
                    {
                        sb.Append("<ul>");
                    }

                    if (obj.Level.ToString() == "1")
                    {
                        sb.Append("<li style='display:table;'>");
                        sb.Append("<div onclick='DisplaySecondaryDemoFilter(this);' Name='" + obj.Name + "' id='" + obj.Id + "' class='lft-popup-ele FilterStringContainerdiv' style=''><span class='lft-popup-ele-label' id='" + obj.Id + "' data-val='" + obj.Name + "' data-parent='' data-isselectable='true'>" + obj.Name + "</span><div class=\"ArrowContainerdiv\"><span class='lft-popup-ele-next sidearrw'></span></div></div>");

                        sb.Append("</li>");
                        index++;
                    }

                    indexSub = 0;

                    if (VisitAdvancedFilter != null)
                    {
                        VisitAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                           //where !Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                           where Convert.ToString(VisitAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                           && Convert.ToString(VisitAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                           select new SecondaryAdvancedFilter
                                                                           {
                                                                               Id = Convert.ToString(row.MetricItemId),
                                                                               Name = Convert.ToString(row.MetricItem),
                                                                               FullName = Convert.ToString(row.MetricItem),
                                                                               MetricId = Convert.ToString(row.MetricId),
                                                                               ParentId = Convert.ToString(row.ParentId),
                                                                               DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                               UniqueId = Convert.ToString(row.UniqueFilterId)
                                                                           }).ToList();

                        if (indexSub == 0)
                            sbSub.Append("<div class='DemographicList' id='" + VisitAdvancedFilter.Id + "' Name='" + VisitAdvancedFilter.Name + "' FullName='" + VisitAdvancedFilter.FullName + "' style='overflow-y:auto;display:none;'><ul>");

                        for (var i = 0; i < VisitAdvancedFilter.SecondaryAdvancedFilterlist.Count; i++)
                        {
                            var obj1 = VisitAdvancedFilter.SecondaryAdvancedFilterlist[i];
                            sbSub.Append("<div onclick='SelectDemographic(this);' class='lft-popup-ele' style=''><span class='lft-popup-ele-label' FullName='" + obj1.FullName + "' DBName='" + obj1.DBName + "' isGeography='" + obj1.isGeography + "' UniqueId='" + obj1.UniqueId + "' shopperdbname='" + obj1.ShopperDBName + "' tripsdbname='" + obj1.TripsDBName + "' data-id='" + obj1.Id + "' id='" + obj1.Id + '-' + obj1.MetricId + '-' + obj1.ParentId + "' Name='" + obj1.Name.ToLower() + "' parent=\"" + obj1.ParentId + "\" ParentLevelId=\"" + VisitAdvancedFilter.Id.ToString().Trim() + "\" ParentLevelName=\"" + VisitAdvancedFilter.Name.ToString().Trim() + "\" data-isselectable=\"true\">" + obj1.Name + "</span></div>");
                            // AllDemographics.push(object.UniqueId + "|" + object.Name);
                            if ((IsItemExist(obj.Name.ToString(), SearchList) == "false"))
                                SearchList.Add(obj1.UniqueId + "|" + obj1.Name);
                        }

                        VisitAdvancedFilterlist.Add(VisitAdvancedFilter);
                    }
                    sbSub.Append("</ul></div>");
                }

                //foreach (PrimaryAdvancedFilter _PrimaryAdvancedFilter in PrimaryAdvancedFilterlist)
                //{
                //    _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                //                                                          where !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                          && Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                          && Convert.ToString(_PrimaryAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                //                                                          select new SecondaryAdvancedFilter
                //                                                          {
                //                                                              Id = Convert.ToString(row.MetricItemId),
                //                                                              Name = Convert.ToString(row.MetricItem),
                //                                                              FullName = Convert.ToString(row.MetricItem),
                //                                                              MetricId = Convert.ToString(row.MetricId),
                //                                                              ParentId = Convert.ToString(row.ParentId),
                //                                                              DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                //                                                              UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                                          }).ToList();

                //}

                //foreach (PrimaryAdvancedFilter _VisitsAdvancedFilter in VisitAdvancedFilterlist)
                //{
                //    _VisitsAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                //                                                         //where !Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                         where Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                         && Convert.ToString(_VisitsAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                //                                                         select new SecondaryAdvancedFilter
                //                                                         {
                //                                                             Id = Convert.ToString(row.MetricItemId),
                //                                                             Name = Convert.ToString(row.MetricItem),
                //                                                             FullName = Convert.ToString(row.MetricItem),
                //                                                             MetricId = Convert.ToString(row.MetricId),
                //                                                             ParentId = Convert.ToString(row.ParentId),
                //                                                             DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                //                                                             UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                                         }).ToList();

                //}
                sb.Append("<li style=\"display:table;\"><div onclick=\"DisplaySecondaryDemoFilter(this);\" name=\"Geography\" id=\"100\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"100\" data-val=\"Geography\" data-parent=\"\" data-isselectable=\"true\">Geography</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>");
                sb.Append("</ul>");

                AdvancedFilterLists.Add(PrimaryAdvancedFilterlist);
                AdvancedFilterLists.Add(VisitAdvancedFilterlist);
                VisitsAdvFilter.StringList = null;
                VisitsAdvFilter.StringList = new RetailerStringList();
                VisitsAdvFilter.StringList.HTML_String = new List<string>();
                VisitsAdvFilter.StringList.HTML_String.Add(sb.ToString());
                VisitsAdvFilter.StringList.HTML_String.Add(sbSub.ToString());
                VisitsAdvFilter.StringList.SearchItems = new List<string>();
                VisitsAdvFilter.StringList.SearchItems = SearchList.ToList();
            }           
            return VisitsAdvFilter;
        }
        #endregion
        #region Load Channel Filters
        List<PrimaryAdvancedFilter> LoadChannelFilters()
        {
            List<PrimaryAdvancedFilter> ChannelAdvancedFilterlist = null;
            PrimaryAdvancedFilter ChannelAdvancedFilter = null;
            if (ds.Tables[7].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[7].AsEnumerable()
                                     select new
                                     {
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         LevelId = row["LevelId"],
                                         UniqueFilterId = row["UniqueFilterId"]
                                     }).Distinct().ToList();

                ChannelAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                foreach (String Metric in (from row in table_Objects select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    ChannelAdvancedFilter = new PrimaryAdvancedFilter();
                    ChannelAdvancedFilter = (from row in table_Objects
                                             where ((Convert.ToString(Metric).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)))
                                             select new PrimaryAdvancedFilter
                                             {
                                                 Id = Convert.ToInt16(row.MetricId),
                                                 Name = Convert.ToString(row.Metric),
                                                 FullName = Convert.ToString(row.Metric),
                                                 Position = GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
                                                 Level = Convert.ToString(row.LevelId),
                                                 DBName = Convert.ToString(row.Metric).Equals("Total", StringComparison.OrdinalIgnoreCase) ? "Total" : "Channels|" + commonFunctions.Get_TableMappingNames(Convert.ToString(row.Metric)),
                                                 UniqueId = Convert.ToString(row.UniqueFilterId)
                                             }).FirstOrDefault();

                    if (ChannelAdvancedFilter != null)
                    {
                        if (ChannelAdvancedFilter.Level != "3")
                            ChannelAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                                 where !Convert.ToString(ChannelAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                                  && !Convert.ToString(ChannelAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                                  && (Convert.ToString(row.MetricItem) != null && Convert.ToString(row.MetricItem) != "" && Convert.ToString(row.MetricItem) != "NULL" && Convert.ToString(row.MetricItem) != "null")
                                                                                  && Convert.ToString(row.LevelId).Equals(Convert.ToString("3"), StringComparison.OrdinalIgnoreCase)
                                                                                  && Convert.ToString(ChannelAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                                  select new SecondaryAdvancedFilter
                                                                                  {
                                                                                      Id = Convert.ToString(row.MetricItemId),
                                                                                      Name = Convert.ToString(row.Metric),
                                                                                      FullName = Convert.ToString(row.Metric),
                                                                                      MetricId = Convert.ToString(row.MetricId),
                                                                                      DBName = "Retailers|" + commonFunctions.Get_TableMappingNames(Convert.ToString(row.Metric)),
                                                                                      UniqueId = Convert.ToString(row.UniqueFilterId)
                                                                                  }).DistinctBy(i => i.Name).ToList();


                        if (ChannelAdvancedFilter.SecondaryAdvancedFilterlist == null || ChannelAdvancedFilter.SecondaryAdvancedFilterlist.Count <= 0)
                            ChannelAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                                 where !Convert.ToString(ChannelAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                                  && (Convert.ToString(row.MetricItem) != null && Convert.ToString(row.MetricItem) != "" && Convert.ToString(row.MetricItem) != "NULL" && Convert.ToString(row.MetricItem) != "null")
                                                                                  && Convert.ToString(ChannelAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                                  && Convert.ToString(ChannelAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                                  select new SecondaryAdvancedFilter
                                                                                  {
                                                                                      Id = Convert.ToString(row.MetricItemId),
                                                                                      Name = Convert.ToString(row.MetricItem),
                                                                                      FullName = Convert.ToString(row.MetricItem),
                                                                                      MetricId = Convert.ToString(row.MetricId),
                                                                                      DBName = "Retailers|" + commonFunctions.Get_TableMappingNames(Convert.ToString(row.Metric)),
                                                                                      UniqueId = Convert.ToString(row.UniqueFilterId)
                                                                                  }).ToList();

                        ChannelAdvancedFilterlist.Add(ChannelAdvancedFilter);
                    }

                }

                //foreach (PrimaryAdvancedFilter _PrimaryAdvancedFilter in ChannelAdvancedFilterlist)
                //{
                //    if (_PrimaryAdvancedFilter.Level != "3")
                //        _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                //                                                              where !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                              && !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                              && (Convert.ToString(row.MetricItem) != null && Convert.ToString(row.MetricItem) != "" && Convert.ToString(row.MetricItem) != "NULL" && Convert.ToString(row.MetricItem) != "null")
                //                                                              && Convert.ToString(row.LevelId).Equals(Convert.ToString("3"), StringComparison.OrdinalIgnoreCase)
                //                                                              && Convert.ToString(_PrimaryAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                //                                                              select new SecondaryAdvancedFilter
                //                                                              {
                //                                                                  Id = Convert.ToString(row.MetricItemId),
                //                                                                  Name = Convert.ToString(row.Metric),
                //                                                                  FullName = Convert.ToString(row.Metric),
                //                                                                  MetricId = Convert.ToString(row.MetricId),
                //                                                                  DBName = "Retailers|" + commonFunctions.Get_TableMappingNames(Convert.ToString(row.Metric)),
                //                                                                  UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                                              }).DistinctBy(i => i.Name).ToList();


                //    if (_PrimaryAdvancedFilter.SecondaryAdvancedFilterlist == null || _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist.Count <= 0)
                //        _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                //                                                              where !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                              && (Convert.ToString(row.MetricItem) != null && Convert.ToString(row.MetricItem) != "" && Convert.ToString(row.MetricItem) != "NULL" && Convert.ToString(row.MetricItem) != "null")
                //                                                              && Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                              && Convert.ToString(_PrimaryAdvancedFilter.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                //                                                              select new SecondaryAdvancedFilter
                //                                                              {
                //                                                                  Id = Convert.ToString(row.MetricItemId),
                //                                                                  Name = Convert.ToString(row.MetricItem),
                //                                                                  FullName = Convert.ToString(row.MetricItem),
                //                                                                  MetricId = Convert.ToString(row.MetricId),
                //                                                                  DBName = "Retailers|" + commonFunctions.Get_TableMappingNames(Convert.ToString(row.Metric)),
                //                                                                  UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                                              }).ToList();

                //}
            }
            return ChannelAdvancedFilterlist;
        }
        #endregion
        #region Measure
        //public List<SelTypes> GetMeasureData()
        //{
        //    string seltype = string.Empty;
        //    List<SelTypes> SelTypelist = new List<DAL.SelTypes>();
        //    SelTypes Seltype = null;
        //    List<GroupType> GroupTypelist = new List<GroupType>();

        //    var table_Objects = (from row in ds.Tables[10].AsEnumerable()
        //                         select new
        //                         {
        //                             Metric = row["Metric"],
        //                             MetricId = row["MetricId"],
        //                             MetricItem = row["MetricItem"],
        //                             MetricItemId = row["MetricItemId"],
        //                             LevelId = row["LevelId"],
        //                             SelTypeId = row["SelTypeId"],
        //                             SelType = row["SelType"],
        //                             FilterType = row["FilterType"],
        //                             FilterTypeId = row["FilterTypeId"],
        //                             ChartTypePIT = row["ChartTypePIT"],
        //                             ChartTypeTrend = row["ChartTypeTrend"],
        //                             UniqueFilterId = row["UniqueFilterId"],
        //                             ParentId = row["ParentId"],
        //                             SearchName = row["SearchName"]
        //                         }).Distinct().ToList();

        //    foreach (string _seltype in (from row in table_Objects
        //                                 select Convert.ToString(row.SelType)).Distinct().ToList())
        //    {
        //        Seltype = new DAL.SelTypes();
        //        Seltype.SelType = Convert.ToString(_seltype);
        //        SelTypelist.Add(Seltype);
        //    }
        //    foreach (SelTypes _seltype in SelTypelist)
        //    {
        //        GroupTypelist = new List<GroupType>();
        //        foreach (string _group in (from row in table_Objects
        //                                   where Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                   select Convert.ToString(row.FilterType)).Distinct().ToList())
        //        {
        //            GroupType _GroupType = new GroupType();
        //            _GroupType = (from row in table_Objects
        //                          where Convert.ToString(row.FilterType).Equals(_group, StringComparison.OrdinalIgnoreCase)
        //                          && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                          select new GroupType
        //                          {
        //                              GroupName = Convert.ToString(row.FilterType),
        //                              GroupId = Convert.ToString(row.FilterTypeId)
        //                          }).Distinct().FirstOrDefault();
        //            GroupTypelist.Add(_GroupType);
        //        }              
        //        foreach (GroupType _group in GroupTypelist)
        //        {                 
        //            _group.PrimaryAdvancedFilter = new List<PrimaryAdvancedFilter>();
        //            foreach (string measure in (from row in table_Objects
        //                                        where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                        && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                        && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
        //                                        && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                        select Convert.ToString(row.Metric)).Distinct().ToList())
        //            {
        //                var query = (from row in table_Objects
        //                             where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                             && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                              && Convert.ToString(row.MetricItem).Equals(measure, StringComparison.OrdinalIgnoreCase)
        //                             select Convert.ToString(row.MetricItem)).FirstOrDefault();

        //                if (string.IsNullOrEmpty(Convert.ToString(query)))
        //                {
        //                    PrimaryAdvancedFilter pfl = (from row in table_Objects
        //                                                 where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                                  && Convert.ToString(row.Metric).Equals(measure, StringComparison.OrdinalIgnoreCase)
        //                                                 && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                                 && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
        //                                                 && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                                 select new PrimaryAdvancedFilter
        //                                                 {
        //                                                     Id = row.MetricId == null ? 00 : Convert.ToInt16(row.MetricId),
        //                                                     Name = row.Metric == null ? "" : Convert.ToString(row.Metric),
        //                                                     FullName = row.Metric == null ? "" : Convert.ToString(row.Metric),
        //                                                     Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
        //                                                     selectType = row.SelTypeId == null ? "" : Convert.ToString(row.SelTypeId),
        //                                                     FilterType = row.FilterType == null ? "" : Convert.ToString(row.FilterType),
        //                                                     FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //                                                     ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
        //                                                     ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
        //                                                     DBName = Convert.ToString(row.Metric),
        //                                                     UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                                     ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //                                                     SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
        //                                                 }).FirstOrDefault();
        //                    _group.PrimaryAdvancedFilter.Add(pfl);                          
        //                }
        //            }                  
        //            foreach (PrimaryAdvancedFilter primaryAdvancedFilter in _group.PrimaryAdvancedFilter)
        //            {
        //                List<SecondaryAdvancedFilter> seclist = (from row in table_Objects
        //                                                         where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                                        && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                                         && Convert.ToString(row.Metric).Equals(Convert.ToString(primaryAdvancedFilter.Name), StringComparison.OrdinalIgnoreCase)
        //                                                         && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                                         select new SecondaryAdvancedFilter
        //                                                         {

        //                                                             Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //                                                             Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                             FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                             MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //                                                             Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
        //                                                             ParentId = row.ParentId == null ? "" : Convert.ToString(primaryAdvancedFilter.Id.ToString()),
        //                                                             DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
        //                                                             UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                                             ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
        //                                                             ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
        //                                                             SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
        //                                                         }).Distinct().ToList();

        //                primaryAdvancedFilter.SecondaryAdvancedFilterlist = seclist;                        
        //                foreach (SecondaryAdvancedFilter secfil in seclist)
        //                {
        //                    var query = (from row in table_Objects
        //                                 where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
        //                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
        //                                 && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
        //                                 && Convert.ToString(row.Metric).Equals(Convert.ToString(secfil.Name), StringComparison.OrdinalIgnoreCase)
        //                                 && Convert.ToString(row.ParentId).Equals(Convert.ToString(secfil.ParentId), StringComparison.OrdinalIgnoreCase)
        //                                 select new SecondaryAdvancedFilter
        //                                 {

        //                                     Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //                                     Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                     FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                     MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //                                     Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
        //                                     ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //                                     DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
        //                                     UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                     ParentDetails = (row.MetricItemId == null || row.MetricItem == null || row.ParentId == null) ? "" : primaryAdvancedFilter.Id + "|" + Convert.ToString(row.MetricItemId) + "|" + Convert.ToString(row.MetricItem) + "|" + Convert.ToString(row.ParentId),
        //                                     ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
        //                                     ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
        //                                     SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
        //                                 }).Distinct().ToList();
        //                    secfil.SecondaryAdvancedFilterlist = new List<SecondaryAdvancedFilter>();
        //                    secfil.SecondaryAdvancedFilterlist = query.ToList();                           
        //                }                     
        //            }                 
        //            _seltype.GroupTypelist = GroupTypelist;
        //        }              
        //    }
        //    return SelTypelist;
        //}
        //added by Nagaraju for Measure HTML string        
        public List<SelTypes> GetMeasureData()
        {
            StringBuilder html1 = new StringBuilder();
            StringBuilder html2 = new StringBuilder();
            StringBuilder html3 = new StringBuilder();
            StringBuilder html4 = new StringBuilder();
            StringBuilder html5 = new StringBuilder();

            string filtertyle = string.Empty;
            List<string> searchitems = null;
            List<string> shopperSearchItems = null;
            List<string> tripsSearchItems = null;

            //added by Nagaraju D for checking duplicate search items
            //Date: 11-29-2017
            List<string> Allsearchitems = null;
            List<string> AllshopperSearchItems = null;
            List<string> AlltripsSearchItems = null;

            string seltype = string.Empty;
            List<SelTypes> SelTypelist = new List<DAL.SelTypes>();
            SelTypes Seltype = null;
            List<GroupType> GroupTypelist = new List<GroupType>();

            var table_Objects = (from row in ds.Tables[10].AsEnumerable()
                                 select new
                                 {
                                     Metric = row["Metric"],
                                     MetricId = row["MetricId"],
                                     MetricItem = row["MetricItem"],
                                     MetricItemId = row["MetricItemId"],
                                     LevelId = row["LevelId"],
                                     SelTypeId = row["SelTypeId"],
                                     SelType = row["SelType"],
                                     FilterType = row["FilterType"],
                                     FilterTypeId = row["FilterTypeId"],
                                     ChartTypePIT = row["ChartTypePIT"],
                                     ChartTypeTrend = row["ChartTypeTrend"],
                                     UniqueFilterId = row["UniqueFilterId"],
                                     ParentId = row["ParentId"],
                                     SearchName = row["SearchName"]
                                 }).Distinct().ToList();

            foreach (string _seltype in (from row in table_Objects
                                         select Convert.ToString(row.SelTypeId)).Distinct().ToList())
            {
                if (Convert.ToInt32(_seltype) % 2 != 0)
                {
                    Seltype = new DAL.SelTypes();
                    Seltype.SelType = Convert.ToString(_seltype);                
                    SelTypelist.Add(Seltype);
                }
            }

            //Seltype = new DAL.SelTypes();
            //Seltype.SelType = Convert.ToString("1");
            //SelTypelist.Add(Seltype);

            //Seltype = new DAL.SelTypes();
            //Seltype.SelType = Convert.ToString("3");
            //SelTypelist.Add(Seltype);

            foreach (SelTypes _seltype in SelTypelist)
            {
                 html1 = new StringBuilder();
                 html2 = new StringBuilder();
                 html3 = new StringBuilder();
                 html4 = new StringBuilder();
                 html5 = new StringBuilder();
                 searchitems = new List<string>();
                 shopperSearchItems = new List<string>();
                 tripsSearchItems = new List<string>();

                Allsearchitems = new List<string>();
                AllshopperSearchItems = new List<string>();
                AlltripsSearchItems = new List<string>();

                GroupTypelist = new List<GroupType>();
                 html1.Append("<ul>");
                foreach (string _group in (from row in table_Objects
                                           where Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                           || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32
                                           (_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase)
                                           select Convert.ToString(row.FilterType)).Distinct().ToList())
                {
                    GroupType _GroupType = new GroupType();
                    _GroupType = (from row in table_Objects
                                  where Convert.ToString(row.FilterType).Equals(_group, StringComparison.OrdinalIgnoreCase)
                                  && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                  || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32
                                           (_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                  select new GroupType
                                  {
                                      GroupName = Convert.ToString(row.FilterType),
                                      GroupId = Convert.ToString(row.FilterTypeId),
                                      SelType = Convert.ToString(row.SelType)
                                  }).Distinct().FirstOrDefault();
                    GroupTypelist.Add(_GroupType);

                     filtertyle = "Shopper";
                    if (Convert.ToString(_GroupType.GroupName).ToLower().IndexOf("demographics") > -1)
                        filtertyle = "Demographics";
                    else if (Convert.ToString(_GroupType.SelType).ToLower().IndexOf("trip") > -1)
                        filtertyle = "Visits";

                    html1.Append("<li style=\"display:none;\" Id=\"" + _GroupType.GroupId + "\" Name=\"" + _GroupType.GroupName + "\" filtertype=\"" + filtertyle + "\"  class=\"gouptype main-measure\" onclick=\"DisplayMeasureList(this);\">");
                    html1.Append("<div class=\"FilterStringContainerdiv\">");
                    html1.Append("<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" Id=\"" + _GroupType.GroupId + "\"  type=\"Main-Stub\"  Name=\"" + _GroupType.GroupName + "\">" + _GroupType.GroupName + "</div>");
                    html1.Append("<div class=\"ArrowContainerdiv measure-inactive\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                    html1.Append("</div>");
                    html1.Append("</li>");
                }
                html1.Append("</ul>");
                foreach (GroupType _group in GroupTypelist)
                {
                    filtertyle = "Shopper";
                    if (Convert.ToString(_group.GroupName).ToLower().IndexOf("demographics") > -1)
                        filtertyle = "Demographics";
                    else if (Convert.ToString(_group.SelType).ToLower().IndexOf("trip") > -1)
                        filtertyle = "Visits";

                    html2.Append("<ul>");
                    _group.PrimaryAdvancedFilter = new List<PrimaryAdvancedFilter>();
                    foreach (string measure in (from row in table_Objects
                                                where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                                || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                                select Convert.ToString(row.Metric)).Distinct().ToList())
                    {
                        var query = (from row in table_Objects
                                     where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                     && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                     || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                      && Convert.ToString(row.MetricItem).Equals(measure, StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(row.MetricItem)).FirstOrDefault();

                        if (string.IsNullOrEmpty(Convert.ToString(query)))
                        {
                            PrimaryAdvancedFilter pfl = (from row in table_Objects
                                                         where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                          && Convert.ToString(row.Metric).Equals(measure, StringComparison.OrdinalIgnoreCase)
                                                         && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                         && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                         && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                                         || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                                         select new PrimaryAdvancedFilter
                                                         {
                                                             Id = row.MetricId == null ? 00 : Convert.ToInt16(row.MetricId),
                                                             Name = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                             FullName = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                             Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                                             selectType = row.SelTypeId == null ? "" : Convert.ToString(row.SelTypeId),
                                                             FilterType = row.FilterType == null ? "" : Convert.ToString(row.FilterType),
                                                             FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                             ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
                                                             ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
                                                             DBName = Convert.ToString(row.Metric),
                                                             UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                             ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                             SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
                                                         }).FirstOrDefault();
                            _group.PrimaryAdvancedFilter.Add(pfl);
                            html2.Append("<li ParentDetails=\"\" style=\"display:none;\" type=\"" + filtertyle + "\" Seltypeid=\"" + pfl.selectType + "\" FilterTypeId=\"" + pfl.FilterTypeId + "\" parentname=\"" + _group.GroupName + "\" DBName=\"" + pfl.DBName + "\" UniqueId=\"" + pfl.UniqueId + "\" shopperdbname=\"" + pfl.ShopperDBName + "\" tripsdbname=\"" + pfl.TripsDBName + "\" Name=\"" + pfl.Name + "\" class=\"gouptype\" ChartTypePIT=\"" + pfl.ChartTypePIT + "\" ChartTypeTrend=\"" + pfl.ChartTypeTrend + "\" onclick=\"SelecMeasure(this);\">");
                            html2.Append("<div class=\"FilterStringContainerdiv\">");
                            html2.Append("<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + pfl.FilterTypeId + "\" id=\"" + pfl.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + pfl.ChartTypePIT + "\" ChartTypeTrend=\"" + pfl.ChartTypeTrend + "\" Name=\"" + pfl.Name + "\">" + pfl.Name + "</div>");
                            html2.Append("<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                            html2.Append("</div>");
                            html2.Append("</li>");
                        }
                    }
                    foreach (PrimaryAdvancedFilter primaryAdvancedFilter in _group.PrimaryAdvancedFilter)
                    {
                        List<SecondaryAdvancedFilter> seclist = (from row in table_Objects
                                                                 where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.Metric).Equals(Convert.ToString(primaryAdvancedFilter.Name), StringComparison.OrdinalIgnoreCase)
                                                                 && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                                                 || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                                                 select new SecondaryAdvancedFilter
                                                                 {

                                                                     Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                     Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                     FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                     MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                     Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                                                     ParentId = row.ParentId == null ? "" : Convert.ToString(primaryAdvancedFilter.Id.ToString()),
                                                                     DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
                                                                     UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                     ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
                                                                     ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
                                                                     SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
                                                                 }).Distinct().ToList();

                        primaryAdvancedFilter.SecondaryAdvancedFilterlist = seclist;
                       
                        foreach (SecondaryAdvancedFilter secfil in seclist)
                        {
                            var query = (from row in table_Objects
                                         where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                        && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                         && (Convert.ToString(row.SelTypeId).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                         || Convert.ToString(row.SelTypeId).Equals(Convert.ToString(Convert.ToInt32(_seltype.SelType) + 1), StringComparison.OrdinalIgnoreCase))
                                         && Convert.ToString(row.Metric).Equals(Convert.ToString(secfil.Name), StringComparison.OrdinalIgnoreCase)
                                         && Convert.ToString(row.ParentId).Equals(Convert.ToString(secfil.ParentId), StringComparison.OrdinalIgnoreCase)
                                         select new SecondaryAdvancedFilter
                                         {

                                             Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                             Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                             FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                             MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                             Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                             ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                             DBName = Convert.ToString(row.Metric) + "|" + Convert.ToString(row.MetricItem),
                                             UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                             ParentDetails = (row.MetricItemId == null || row.MetricItem == null || row.ParentId == null) ? "" : primaryAdvancedFilter.Id + "|" + Convert.ToString(row.MetricItemId) + "|" + Convert.ToString(row.MetricItem) + "|" + Convert.ToString(row.ParentId),
                                             ChartTypePIT = row.ChartTypePIT == null ? "" : Convert.ToString(row.ChartTypePIT),
                                             ChartTypeTrend = row.ChartTypeTrend == null ? "" : Convert.ToString(row.ChartTypeTrend),
                                             SearchName = row.SearchName == null ? "" : Convert.ToString(row.SearchName)
                                         }).Distinct().ToList();
                            secfil.SecondaryAdvancedFilterlist = new List<SecondaryAdvancedFilter>();
                            secfil.SecondaryAdvancedFilterlist = query.ToList();
                            if (query != null && query.Count > 0)
                            {
                                html3.Append("<ul Name=\"" + primaryAdvancedFilter.Name + "\" style=\"display:none;\">");
                                html3.Append("<li ParentDetails=\"" + secfil.ParentDetails + "\" type=\"" + filtertyle + "\" id=\"" + secfil.Id + "\" FilterTypeId=\"" + primaryAdvancedFilter.FilterTypeId + "\" Seltypeid=\"" + primaryAdvancedFilter.selectType + "\" parentname=\"" + primaryAdvancedFilter.Name + "\" type=\"Sub-Level\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" ChartTypePIT=\"" + secfil.ChartTypePIT + "\" ChartTypeTrend=\"" + secfil.ChartTypeTrend + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" Name=\"" + secfil.Name + "\" class=\"gouptype\" onclick=\"SelecMeasure(this);\">");
                                html3.Append("<div class=\"FilterStringContainerdiv\">");
                                html3.Append("<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + primaryAdvancedFilter.FilterTypeId + "\" parentname=\"" + primaryAdvancedFilter.Name + "\" id=\"" + secfil.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + secfil.ChartTypePIT + "\" ChartTypeTrend=\"" + secfil.ChartTypeTrend + "\" Name=\"" + secfil.Name + "\">" + secfil.Name + "</div>");
                                html3.Append("<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                                html3.Append("</div>");
                                html3.Append("</li>");
                                html3.Append("</ul>");

                                html4.Append("<ul uniqueid=\"" + secfil.UniqueId + "\" Name=\"" + secfil.Name + "\" style=\"display:none;\">");
                                foreach (SecondaryAdvancedFilter thfil in secfil.SecondaryAdvancedFilterlist)
                                {
                                    html4.Append("<li style=\"white-space:pre-wrap;\" Type=\"" + filtertyle + "\" Seltypeid=\"" + primaryAdvancedFilter.selectType + "\" mainparentname=\"" + _group.GroupName + "\" ParentDetails=\"" + thfil.ParentDetails + "\" UId=\"" + thfil.FilterTypeId + "|" + thfil.Id + "|" + thfil.ParentId + "\" id=\"" + thfil.Id + "|" + thfil.Id + "\" type=\"Main-Stub\" DBName=\"" + thfil.DBName + "\" UniqueId=\"" + thfil.UniqueId + "\" shopperdbname=\"" + thfil.ShopperDBName + "\" tripsdbname=\"" + thfil.TripsDBName + "\" Name=\"" + thfil.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + thfil.Name + "</li>");
                                    if (!Allsearchitems.Contains(thfil.Name.Trim()))
                                    {
                                        searchitems.Add(thfil.UniqueId + "|" + thfil.Name + "|" + thfil.DBName.Split('|')[0]);
                                        Allsearchitems.Add(thfil.Name.Trim());
                                    }
                                    if (filtertyle.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (!AllshopperSearchItems.Contains(thfil.Name.Trim()))
                                        {
                                            shopperSearchItems.Add(thfil.UniqueId + "|" + thfil.Name + "|" + thfil.DBName.Split('|')[0]);
                                            AllshopperSearchItems.Add(thfil.Name.Trim());
                                        }
                                    }                                       

                                    if (filtertyle.Equals("Visits", StringComparison.OrdinalIgnoreCase) || filtertyle.Equals("Demographics", StringComparison.OrdinalIgnoreCase))
                                        if (!AlltripsSearchItems.Contains(thfil.Name.Trim()))
                                        {
                                            tripsSearchItems.Add(thfil.UniqueId + "|" + thfil.Name + "|" + thfil.DBName.Split('|')[0]);
                                            AlltripsSearchItems.Add(thfil.Name.Trim());
                                        }
                                   
                                }
                                html4.Append("</ul>");
                            }
                            else
                            {
                                if (seclist.IndexOf(secfil) == 0)
                                    html4.Append("<ul uniqueid=\"" + primaryAdvancedFilter.UniqueId + "\" Name=\"" + primaryAdvancedFilter.Name + "\" style=\"display:none;\">");

                                html4.Append("<li style=\"white-space:pre-wrap;\" Type=\"" + filtertyle + "\" Seltypeid=\"" + primaryAdvancedFilter.selectType + "\" mainparentname=\"" + _group.GroupName + "\" ParentDetails=\"" + secfil.ParentDetails + "\" UId=\"" + secfil.FilterTypeId + "|" + secfil.Id + "|" + secfil.ParentId + "\" id=\"" + secfil.Id + "|" + secfil.Id + "\" type=\"Main-Stub\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" Name=\"" + secfil.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + secfil.Name + "</li>");
                                if (!Allsearchitems.Contains(secfil.Name.Trim()))
                                {
                                    searchitems.Add(secfil.UniqueId + "|" + secfil.Name + "|" + secfil.DBName.Split('|')[0]);
                                    Allsearchitems.Add(secfil.Name.Trim());
                                }

                                if (filtertyle.Equals("Shopper", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!AllshopperSearchItems.Contains(secfil.Name.Trim()))
                                    {
                                        shopperSearchItems.Add(secfil.UniqueId + "|" + secfil.Name + "|" + secfil.DBName.Split('|')[0]);
                                        AllshopperSearchItems.Add(secfil.Name.Trim());
                                    }                                  
                                }

                                if (filtertyle.Equals("Visits", StringComparison.OrdinalIgnoreCase) || filtertyle.Equals("Demographics", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (!AlltripsSearchItems.Contains(secfil.Name.Trim()))
                                    {
                                        tripsSearchItems.Add(secfil.UniqueId + "|" + secfil.Name + "|" + secfil.DBName.Split('|')[0]);
                                        AlltripsSearchItems.Add(secfil.Name.Trim());
                                    }
                                  
                                }
                            }
                        }
                        html4.Append("</ul>");      
                    }
                    html2.Append("</ul>");
                    _seltype.html1 = html1.ToString();
                    _seltype.html2 = html2.ToString();
                    _seltype.html3 = html3.ToString();
                    _seltype.html4 = html4.ToString();
                    _seltype.html5 = html5.ToString();

                    _seltype.SearchObj = new SearchHTMLEntity();
                    _seltype.SearchObj.SearchItems = searchitems;
                    _seltype.SearchObj.ShopperSearchItems = shopperSearchItems;
                    _seltype.SearchObj.TripsSearchItems = tripsSearchItems;
                    //added by Nagaraju for HTML string
                    //Date: 17-04-2017
                    //_seltype.GroupTypelist = GroupTypelist;

                }
            }            
            return SelTypelist;
        }
        #endregion
        #region Get Groups Prime Filters
        List<GroupsPrimeFilters> GetGroupsPrimeFilters(int TableNumber)
        {
            List<GroupsPrimeFilters> GroupsPrimeFilterlist = null;
            GroupsPrimeFilterlist = ds.Tables[TableNumber].AsEnumerable().Select(x => new
            {
                Id = Convert.ToString(x["PrimeFilterTypeId"]),
                PrimeFilterType = Convert.ToString(x["PrimeFilterType"]),
                FilterType = Convert.ToString(x["FilterType"])
            }).Distinct().ToList().Select(g => new GroupsPrimeFilters { Id = g.Id, PrimeFilterType = g.PrimeFilterType, FilterType = g.FilterType }).ToList()
               .Where(a => !a.FilterType.Equals("Geography", StringComparison.OrdinalIgnoreCase) && !a.FilterType.Equals("Extra Beverages", StringComparison.OrdinalIgnoreCase)).ToList();
            return GroupsPrimeFilterlist;
        }


        #endregion
        #region Load Group Type Filters
        //List<PrimaryAdvancedFilter> LoadGroupTypeFilters(int TableNumber, string selecttype)
        //{
        //    List<PrimaryAdvancedFilter> PrimaryAdvancedFilterlist = null;
        //    PrimaryAdvancedFilter PrimaryAdvancedFilter = null;
        //    if (ds.Tables[TableNumber].Rows.Count > 0)
        //    {
        //        var table_Objects = (from row in ds.Tables[TableNumber].AsEnumerable()
        //                             select new
        //                             {
        //                                 LevelId = row["LevelId"],
        //                                 FilterType = row["FilterType"],
        //                                 Metric = row["Metric"],
        //                                 MetricId = row["MetricId"],
        //                                 FilterTypeId = row["FilterTypeId"],
        //                                 UniqueFilterId = row["UniqueFilterId"],
        //                                 PrimeFilterType = row["PrimeFilterType"],
        //                                 PrimeFilterTypeId = row["PrimeFilterTypeId"],
        //                                 MetricItem = row["MetricItem"],
        //                                 MetricItemId = row["MetricItemId"],
        //                                 ParentId = row["ParentId"]
        //                             }).Distinct().ToList();

        //        PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
        //        //Taking Measure Filtes             
        //        //End                
        //        List<string> metriclist = (from row in table_Objects
        //                                   where Convert.ToString(row.LevelId) == "1"
        //                                       && !Convert.ToString(row.FilterType).Equals("Geography", StringComparison.OrdinalIgnoreCase)
        //                                   select (Convert.ToString(row.Metric))).Distinct().ToList();

        //        foreach (String Metric in metriclist)
        //        {
        //            PrimaryAdvancedFilter = (from row in table_Objects
        //                                     where (Convert.ToString(Metric).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase))
        //                                     && Convert.ToString(row.LevelId) == "1"
        //                                      && !Convert.ToString(row.FilterType).Equals("Geography", StringComparison.OrdinalIgnoreCase)
        //                                     select new PrimaryAdvancedFilter
        //                                     {
        //                                         Id = row.MetricId == null ? 00 : Convert.ToInt16(row.MetricId),
        //                                         Name = row.Metric == null ? "" : Convert.ToString(row.Metric),
        //                                         FullName = row.Metric == null ? "" : Convert.ToString(row.Metric),
        //                                         Position = row.Metric == null ? "" : GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
        //                                         Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
        //                                         FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //                                         DBName = row.Metric == null ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)),
        //                                         UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                         PrimeFilterType = Convert.ToString(row.PrimeFilterType),
        //                                         PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
        //                                         FilterType = Convert.ToString(row.FilterType)
        //                                     }).FirstOrDefault();

        //            PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
        //                                                                 where !Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
        //                                                                  && Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
        //                                                                  select new SecondaryAdvancedFilter
        //                                                                  {
        //                                                                      Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //                                                                      LevelId = Convert.ToString(row.LevelId),
        //                                                                      Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                                      Metric = Convert.ToString(row.Metric),
        //                                                                      FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                                      MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //                                                                      ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //                                                                      FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //                                                                      DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
        //                                                                      UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                                                      PrimeFilterType = Convert.ToString(row.PrimeFilterType),
        //                                                                      PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
        //                                                                      FilterType = Convert.ToString(row.FilterType)
        //                                                                  }).Distinct().ToList();

        //            foreach (SecondaryAdvancedFilter secfil in PrimaryAdvancedFilter.SecondaryAdvancedFilterlist)
        //            {
        //                var query = secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
        //                                                                  where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
        //                                                                  && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
        //                                                                  && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
        //                                                                  select new SecondaryAdvancedFilter
        //                                                                  {
        //                                                                      Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //                                                                      Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                                      Metric = Convert.ToString(row.Metric),
        //                                                                      FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                                      MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //                                                                      ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //                                                                      FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //                                                                      DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
        //                                                                      UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                                                      PrimeFilterType = Convert.ToString(row.PrimeFilterType),
        //                                                                      FilterType = Convert.ToString(row.FilterType)
        //                                                                  }).Distinct().ToList();
        //                if (query != null && query.Count > 0)
        //                {
        //                    secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
        //                                                          where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
        //                                                          && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
        //                                                           && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
        //                                                          select new SecondaryAdvancedFilter
        //                                                          {
        //                                                              Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //                                                              Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                              Metric = Convert.ToString(row.Metric),
        //                                                              FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //                                                              MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //                                                              ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //                                                              FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //                                                              DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
        //                                                              UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //                                                              PrimeFilterType = Convert.ToString(row.PrimeFilterType),
        //                                                              PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
        //                                                              FilterType = Convert.ToString(row.FilterType)
        //                                                          }).Distinct().ToList();
        //                }
        //            }

        //            PrimaryAdvancedFilterlist.Add(PrimaryAdvancedFilter);
        //        }
        //        //foreach (PrimaryAdvancedFilter _PrimaryAdvancedFilter in PrimaryAdvancedFilterlist)
        //        //{
        //        //    _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
        //        //                                                          where !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
        //        //                                                          && Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
        //        //                                                          select new SecondaryAdvancedFilter
        //        //                                                          {
        //        //                                                              Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //        //                                                              LevelId = Convert.ToString(row.LevelId),
        //        //                                                              Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //        //                                                              Metric = Convert.ToString(row.Metric),
        //        //                                                              FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //        //                                                              MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //        //                                                              ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //        //                                                              FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //        //                                                              DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
        //        //                                                              UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //        //                                                              PrimeFilterType = Convert.ToString(row.PrimeFilterType),
        //        //                                                              PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
        //        //                                                              FilterType = Convert.ToString(row.FilterType)
        //        //                                                          }).Distinct().ToList();

        //        //    foreach (SecondaryAdvancedFilter secfil in _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist)
        //        //    {
        //        //        var query = secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
        //        //                                                          where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
        //        //                                                          && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
        //        //                                                          && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
        //        //                                                          select new SecondaryAdvancedFilter
        //        //                                                          {
        //        //                                                              Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //        //                                                              Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //        //                                                              Metric = Convert.ToString(row.Metric),
        //        //                                                              FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //        //                                                              MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //        //                                                              ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //        //                                                              FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //        //                                                              DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
        //        //                                                              UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //        //                                                              PrimeFilterType = Convert.ToString(row.PrimeFilterType),
        //        //                                                              FilterType = Convert.ToString(row.FilterType)
        //        //                                                          }).Distinct().ToList();
        //        //        if (query != null && query.Count > 0)
        //        //        {
        //        //            secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
        //        //                                                  where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
        //        //                                                  && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
        //        //                                                   && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
        //        //                                                  select new SecondaryAdvancedFilter
        //        //                                                  {
        //        //                                                      Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
        //        //                                                      Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //        //                                                      Metric = Convert.ToString(row.Metric),
        //        //                                                      FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
        //        //                                                      MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
        //        //                                                      ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
        //        //                                                      FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
        //        //                                                      DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
        //        //                                                      UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
        //        //                                                      PrimeFilterType = Convert.ToString(row.PrimeFilterType),
        //        //                                                      PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
        //        //                                                      FilterType = Convert.ToString(row.FilterType)
        //        //                                                  }).Distinct().ToList();
        //        //        }
        //        //    }

        //        //}
        //    }
        //    return PrimaryAdvancedFilterlist;
        //}

        //added by Nagarau for forming Groups html string
        List<PrimaryAdvancedFilter> LoadGroupTypeFilters(int TableNumber, string selecttype, out GroupsFilterlist groupsFilterlist)
        {
            StringBuilder GroupTypeHeaderContent = new StringBuilder();
            StringBuilder GroupTypeContent = new StringBuilder();
            StringBuilder GroupTypeContentSub = new StringBuilder();

            groupsFilterlist = new DAL.GroupsFilterlist();
            groupsFilterlist.SearchObj = new SearchHTMLEntity();
            groupsFilterlist.SearchObj.SearchItems = new List<string>();
            groupsFilterlist.SearchObj.ReportsSearchItems = new List<string>();
            
            List<PrimaryAdvancedFilter> PrimaryAdvancedFilterlist = null;
            PrimaryAdvancedFilter PrimaryAdvancedFilter = null;
            if (ds.Tables[TableNumber].Rows.Count > 0)
            {
                var table_Objects = (from row in ds.Tables[TableNumber].AsEnumerable()
                                     select new
                                     {
                                         LevelId = row["LevelId"],
                                         FilterType = row["FilterType"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         FilterTypeId = row["FilterTypeId"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         PrimeFilterType = row["PrimeFilterType"],
                                         PrimeFilterTypeId = row["PrimeFilterTypeId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         ParentId = row["ParentId"]
                                     }).Distinct().ToList();

                PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                //Taking Measure Filtes             
                //End                
                List<string> metriclist = (from row in table_Objects
                                           where Convert.ToString(row.LevelId) == "1"
                                               && !Convert.ToString(row.FilterType).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                           select (Convert.ToString(row.PrimeFilterTypeId) + "_" + Convert.ToString(row.Metric))).Distinct().ToList();
                //lavel 1
                GroupTypeHeaderContent.Append("<div id=\"GroupTypeHeaderContent\" class=\"Lavel\" style=\"display:none;height:94%;\">");
                GroupTypeHeaderContent.Append("<ul>");

                //lavel 2
                GroupTypeContent.Append("<div id=\"GroupTypeContent\" class=\"Lavel\" style=\"display:none;\">");

                //lavel 3
                GroupTypeContentSub.Append("<div id=\"GroupTypeContentSub\" class=\"Lavel\" style=\"display:none;\">");   
                foreach (String Metric in metriclist)
                {
                    PrimaryAdvancedFilter = (from row in table_Objects
                                             where (Convert.ToString(Metric).Split('_')[1].Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                             && (Convert.ToString(Metric).Split('_')[0].Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase))
                                             )
                                             && Convert.ToString(row.LevelId) == "1"
                                              && !Convert.ToString(row.FilterType).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                             select new PrimaryAdvancedFilter
                                             {
                                                 Id = row.MetricId == null ? 00 : Convert.ToInt16(row.MetricId),
                                                 Name = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                 FullName = row.Metric == null ? "" : Convert.ToString(row.Metric),
                                                 Position = row.Metric == null ? "" : GetAdvancedFilterPosition(Convert.ToString(row.Metric)),
                                                 Level = row.LevelId == null ? "" : Convert.ToString(row.LevelId),
                                                 FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                 DBName = row.Metric == null ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)),
                                                 UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                 PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                 PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                 FilterType = Convert.ToString(row.FilterType)
                                             }).FirstOrDefault();
                    if (PrimaryAdvancedFilter.PrimeFilterType.ToLower() == "shopper frequency" && PrimaryAdvancedFilter.Name.ToLower() != "main store/favorite store") {
                        GroupTypeHeaderContent.Append("<li style=\"display: none;\" primefiltertype=\"" + PrimaryAdvancedFilter.PrimeFilterType + "\" filtertype=\"" + PrimaryAdvancedFilter.FilterType + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" dbname=\"" + PrimaryAdvancedFilter.Name + "\" uniqueid=\"" + PrimaryAdvancedFilter.UniqueId + "\" shopperdbname=\"null\" tripsdbname=\"null\" class=\"gouptype\" ParentLevelId=\" " + PrimaryAdvancedFilter.Id + " \" ParentLevelName=\" " + PrimaryAdvancedFilter.Name + " \" data-isselectable=\"true\" onclick=\"SelecGroupMetricName(this);\"><span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + PrimaryAdvancedFilter.FilterTypeId + "\" id=\"" + PrimaryAdvancedFilter.Id + "\" type=\"Main-Stub\" name=\"" + PrimaryAdvancedFilter.Name + "\" primefiltertype=\"" + PrimaryAdvancedFilter.PrimeFilterType + "\" filtertype=\"" + PrimaryAdvancedFilter.FilterType + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" dbname=\"" + PrimaryAdvancedFilter.Name + "\" uniqueid=\"" + PrimaryAdvancedFilter.UniqueId + "\" shopperdbname=\"null\" tripsdbname=\"null\" class=\"gouptype\" ParentLevelId=\" " + PrimaryAdvancedFilter.Id + " \" ParentLevelName=\" " + PrimaryAdvancedFilter.Name + " \" data-isselectable=\"true\">" + PrimaryAdvancedFilter.Name + "</span></li>");
                        groupsFilterlist.SearchObj.SearchItems.Add(PrimaryAdvancedFilter.UniqueId + "|" + PrimaryAdvancedFilter.Name);
                    }
                    else {
                        GroupTypeHeaderContent.Append("<li style=\"display: none;\" primefiltertype=\"" + PrimaryAdvancedFilter.PrimeFilterType + "\" filtertype=\"" + PrimaryAdvancedFilter.FilterType + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" dbname=\"" + PrimaryAdvancedFilter.Name + "\" uniqueid=\"" + PrimaryAdvancedFilter.UniqueId + "\" shopperdbname=\"null\" tripsdbname=\"null\" class=\"gouptype\" onclick=\"SelecGroup(this);\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + PrimaryAdvancedFilter.FilterTypeId + "\" id=\"" + PrimaryAdvancedFilter.Id + "\" type=\"Main-Stub\" name=\"" + PrimaryAdvancedFilter.Name + "\">" + PrimaryAdvancedFilter.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>");
                    }

                    if (PrimaryAdvancedFilter.PrimeFilterType.ToLower() == "demographics" || PrimaryAdvancedFilter.PrimeFilterType.ToLower() == "beverage purchaser")                   
                        groupsFilterlist.SearchObj.ReportsSearchItems.Add(PrimaryAdvancedFilter.UniqueId + "|" + PrimaryAdvancedFilter.Name);                  

                    GroupTypeContent.Append("<div class=\"DemographicList\" id=\"" + PrimaryAdvancedFilter.Id + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" fullname=\"" + PrimaryAdvancedFilter.Name + "\" style=\"overflow-y: auto;display:none;\">");
                    GroupTypeContent.Append("<ul>");
                    PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                         where !Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(PrimaryAdvancedFilter.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
                                                                         select new SecondaryAdvancedFilter
                                                                         {
                                                                             Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                             LevelId = Convert.ToString(row.LevelId),
                                                                             Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                             Metric = Convert.ToString(row.Metric),
                                                                             FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                             MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                             ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                                             FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                                             DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                             UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                             PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                             PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                                             FilterType = Convert.ToString(row.FilterType)
                                                                         }).Distinct().ToList();

                    foreach (SecondaryAdvancedFilter secfil in PrimaryAdvancedFilter.SecondaryAdvancedFilterlist)
                    {
                        var query = secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                          where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                          select new SecondaryAdvancedFilter
                                                                          {
                                                                              Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                              Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                              Metric = Convert.ToString(row.Metric),
                                                                              FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                              MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                              ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                                              FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                                              DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                              UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                              PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                              FilterType = Convert.ToString(row.FilterType)
                                                                          }).Distinct().ToList();
                        if (query != null && query.Count > 0)
                        {
                            GroupTypeContent.Append("<li class=\"gouptype\" MetricId=\"" + secfil.MetricId + "\" style=\"\" PrimeFilterType=\"" + secfil.PrimeFilterType + "\" FilterType=\"" + secfil.FilterType + "\" Name=\"" + secfil.Name + "\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" class=\"gouptype\" onclick=\"DisplayThirdLevelDemoFilter(this);\">");
                            GroupTypeContent.Append("<div  class=\"FilterStringContainerdiv\" style=\"\">");
                            GroupTypeContent.Append("<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + secfil.FilterTypeId + "\" id=\"" + secfil.Id + "\" type=\"Main-Stub\" Name=\"" + secfil.Name + "\">" + secfil.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>");
                            GroupTypeContent.Append("</div>");
                            GroupTypeContent.Append("</li>");

                            secfil.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                  where Convert.ToString(secfil.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                  && Convert.ToString(secfil.PrimeFilterTypeId).Equals(Convert.ToString(row.PrimeFilterTypeId), StringComparison.OrdinalIgnoreCase)
                                                                   && Convert.ToString(secfil.MetricId).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                                  select new SecondaryAdvancedFilter
                                                                  {
                                                                      Id = row.MetricItemId == null ? "" : Convert.ToString(row.MetricItemId),
                                                                      Name = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                      Metric = Convert.ToString(row.Metric),
                                                                      FullName = row.MetricItem == null ? "" : Convert.ToString(row.MetricItem),
                                                                      MetricId = row.MetricId == null ? "" : Convert.ToString(row.MetricId),
                                                                      ParentId = row.ParentId == null ? "" : Convert.ToString(row.ParentId),
                                                                      FilterTypeId = row.FilterTypeId == null ? "" : Convert.ToString(row.FilterTypeId),
                                                                      DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                      UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                      PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                      PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                                      FilterType = Convert.ToString(row.FilterType)
                                                                  }).Distinct().ToList();

                            GroupTypeContentSub.Append("<div class=\"DemographicList\" id=\"" + PrimaryAdvancedFilter.Id + "\" name=\"" + PrimaryAdvancedFilter.Name + "\" fullname=\"" + PrimaryAdvancedFilter.Name + "\" style=\"overflow-y: auto;display:none;\">");
                            GroupTypeContentSub.Append("<ul>");
                            foreach (SecondaryAdvancedFilter thfil in secfil.SecondaryAdvancedFilterlist)
                            {
                                GroupTypeContentSub.Append("<div MetricId=\"" + secfil.MetricId + "\" style=\"display: none;\" id=\"" + secfil.Id + "\" PrimeFilterType=\"" + secfil.PrimeFilterType + "\" FilterType=\"" + secfil.FilterType + "\" Name=\"" + thfil.Name + "\" MericName=\"" + secfil.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + thfil.isGeography + "\" FullName=\"" + thfil.FullName + "\" DBName=\"" + thfil.DBName + "\" UniqueId=\"" + thfil.UniqueId + "\" shopperdbname=\"" + thfil.ShopperDBName + "\" tripsdbname=\"" + thfil.TripsDBName + "\"  data-id=\"" + thfil.Id + "\" id=" + thfil.Id + "-" + thfil.MetricId + "-" + thfil.ParentId + " Name=\"" + thfil.Name + "\" parent=\"" + thfil.ParentId + "\" ParentLevelId=\" " + PrimaryAdvancedFilter.Id + " \" ParentLevelName=\" " + PrimaryAdvancedFilter.Name + " \" data-isselectable=\"true\">" + thfil.Name + "</span></div>");
                                groupsFilterlist.SearchObj.SearchItems.Add(thfil.UniqueId + "|" + thfil.Name);

                                if (PrimaryAdvancedFilter.PrimeFilterType.ToLower() == "demographics" || PrimaryAdvancedFilter.PrimeFilterType.ToLower() == "beverage purchaser")
                                    groupsFilterlist.SearchObj.ReportsSearchItems.Add(thfil.UniqueId + "|" + thfil.Name);   
                            }
                            GroupTypeContentSub.Append("</ul>");
                            GroupTypeContentSub.Append("</div>");
                        }
                        else
                        {
                            GroupTypeContent.Append("<div PrimeFilterType=\"" + secfil.PrimeFilterType + "\" FilterType=\"" + secfil.FilterType + "\" Name=\"" + secfil.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + secfil.isGeography + "\" FullName=\"" + secfil.FullName + "\" DBName=\"" + secfil.DBName + "\" UniqueId=\"" + secfil.UniqueId + "\" shopperdbname=\"" + secfil.ShopperDBName + "\" tripsdbname=\"" + secfil.TripsDBName + "\" data-id=\"" + secfil.Id + "\" id=" + secfil.Id + "-" + secfil.MetricId + "-" + secfil.ParentId + " Name=\"" + secfil.Name + "\" parent=\"" + secfil.ParentId + "\" ParentLevelId=\" " + PrimaryAdvancedFilter.Id + " \" ParentLevelName=\" " + PrimaryAdvancedFilter.Name + " \" data-isselectable=\"true\">" + secfil.Name + "</span></div>");
                            groupsFilterlist.SearchObj.SearchItems.Add(secfil.UniqueId + "|" + secfil.Name);

                            if (PrimaryAdvancedFilter.PrimeFilterType.ToLower() == "demographics" || PrimaryAdvancedFilter.PrimeFilterType.ToLower() == "beverage purchaser")
                                groupsFilterlist.SearchObj.ReportsSearchItems.Add(secfil.UniqueId + "|" + secfil.Name);
                        }
                    }
                    GroupTypeContent.Append("</ul>");
                    GroupTypeContent.Append("</div>");
                    PrimaryAdvancedFilterlist.Add(PrimaryAdvancedFilter);
                }               
            }
            GroupTypeHeaderContent.Append("</ul>");
            GroupTypeHeaderContent.Append("</div>");

            GroupTypeContent.Append("</div>");
            GroupTypeContentSub.Append("</div>");

            groupsFilterlist.SearchObj.HTML_String += GroupTypeHeaderContent.ToString();
            groupsFilterlist.SearchObj.HTML_String += GroupTypeContent.ToString();
            groupsFilterlist.SearchObj.HTML_String += GroupTypeContentSub.ToString();
            //added by Nagaraju for HTML string
            //Date: 17-04-2017
            PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
            return PrimaryAdvancedFilterlist;
        }

        #endregion
        #region Load Frequency Filters
        List<Frequency> LoadFrequencyFiltersFilters()
        {
            List<Frequency> Frequencylist = null;
            Frequency Frequency = null;
            if (ds.Tables[3].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[3].AsEnumerable()
                                     select new
                                     {
                                         UniqueFilterId = row["UniqueFilterId"],
                                         Frequency = row["Frequency"],
                                         SubFrequency = row["SubFrequency"],
                                         SubFrequencyId = row["SubFrequencyId"]
                                     }).ToList();

                Frequencylist = new List<Frequency>();
                foreach (String frequency in (from row in table_Objects select (Convert.ToString(row.Frequency))).Distinct().ToList())
                {
                    Frequency = new Frequency();
                    Frequency = (from row in table_Objects
                                 where Convert.ToString(frequency).Equals(Convert.ToString(row.Frequency), StringComparison.OrdinalIgnoreCase)
                                 select new Frequency
                                 {
                                     Id = Convert.ToInt16(row.UniqueFilterId),
                                     Name = Convert.ToString(row.Frequency),
                                     SubFrequency = Convert.ToString(row.SubFrequency),
                                     UniqueId = Convert.ToString(row.UniqueFilterId)
                                 }).FirstOrDefault();


                    Frequency.Frequencylist = (from row in table_Objects
                                               where Convert.ToString(Frequency.Name).Equals(Convert.ToString(row.Frequency), StringComparison.OrdinalIgnoreCase)
                                                    && !Convert.ToString("").Equals(Convert.ToString(row.SubFrequency), StringComparison.OrdinalIgnoreCase)
                                                    select new Frequency
                                                    {
                                                        Id = Convert.ToInt16(row.SubFrequencyId),
                                                        Name = Convert.ToString(row.SubFrequency),
                                                        UniqueId = Convert.ToString(row.UniqueFilterId)
                                                    }).ToList();

                    Frequencylist.Add(Frequency);
                }

                //foreach (Frequency _Frequencylist in Frequencylist)
                //{
                //    _Frequencylist.Frequencylist = (from row in table_Objects
                //                                    where Convert.ToString(_Frequencylist.Name).Equals(Convert.ToString(row.Frequency), StringComparison.OrdinalIgnoreCase)
                //                                    && !Convert.ToString("").Equals(Convert.ToString(row.SubFrequency), StringComparison.OrdinalIgnoreCase)
                //                                    select new Frequency
                //                                    {
                //                                        Id = Convert.ToInt16(row.SubFrequencyId),
                //                                        Name = Convert.ToString(row.SubFrequency),
                //                                        UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                    }).Distinct().ToList();

                //}
            }
            return Frequencylist;
        }
        #endregion
        #region Load Report Frequency Filters
        List<Frequency> LoadReportTripsFrequencyFiltersFilters()
        {
            List<Frequency> Frequencylist = null;
            Frequency Frequency = null;
            if (ds.Tables[3].Rows.Count > 0)
            {

                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[3].AsEnumerable()
                                     select new
                                     {
                                         UniqueFilterId = row["UniqueFilterId"],
                                         Frequency = row["Frequency"],
                                         SubFrequency = row["SubFrequency"],
                                         SubFrequencyId = row["SubFrequencyId"]
                                     }).Distinct().ToList();

                Frequencylist = new List<Frequency>();
                foreach (String frequency in (from row in table_Objects where Convert.ToString(row.Frequency).Equals("Total Visits", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.Frequency).Equals("Monthly +", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.Frequency).Equals("Weekly +", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.Frequency).Equals("quarterly +", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.Frequency).Equals("annually +", StringComparison.OrdinalIgnoreCase) select (Convert.ToString(row.Frequency))).Distinct().ToList())
                {
                    Frequency = new Frequency();
                    Frequency = (from row in table_Objects
                                 where Convert.ToString(frequency).Equals(Convert.ToString(row.Frequency), StringComparison.OrdinalIgnoreCase)
                                 select new Frequency
                                 {
                                     Id = Convert.ToInt16(row.UniqueFilterId),
                                     Name = Convert.ToString(row.Frequency),
                                     SubFrequency = Convert.ToString(row.SubFrequency),
                                     UniqueId = Convert.ToString(row.UniqueFilterId)
                                 }).FirstOrDefault();
                    Frequencylist.Add(Frequency);
                }
            }
            return Frequencylist;
        }
        List<Frequency> LoadReportFrequencyFiltersFilters()
        {
            List<Frequency> Frequencylist = null;
            Frequency Frequency = null;
            if (ds.Tables[3].Rows.Count > 0)
            {

                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[3].AsEnumerable()
                                     select new
                                     {
                                         UniqueFilterId = row["UniqueFilterId"],
                                         Frequency = row["Frequency"],
                                         SubFrequency = row["SubFrequency"],
                                         SubFrequencyId = row["SubFrequencyId"]
                                     }).Distinct().ToList();

                Frequencylist = new List<Frequency>();
                foreach (String frequency in (from row in table_Objects where Convert.ToString(row.Frequency).Equals("Monthly +", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.Frequency).Equals("Weekly +", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.Frequency).Equals("quarterly +", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row.Frequency).Equals("annually +", StringComparison.OrdinalIgnoreCase) select (Convert.ToString(row.Frequency))).Distinct().ToList())
                {
                    Frequency = new Frequency();
                    Frequency = (from row in table_Objects
                                 where Convert.ToString(frequency).Equals(Convert.ToString(row.Frequency), StringComparison.OrdinalIgnoreCase)
                                 select new Frequency
                                 {
                                     Id = Convert.ToInt16(row.UniqueFilterId),
                                     Name = Convert.ToString(row.Frequency),
                                     SubFrequency = Convert.ToString(row.SubFrequency),
                                     UniqueId = Convert.ToString(row.UniqueFilterId)
                                 }).FirstOrDefault();
                    Frequencylist.Add(Frequency);
                }
            }
            return Frequencylist;
        }
        #endregion
        #region Load BGM Frequency Filters
        List<Frequency> LoadBGMFrequencyFiltersFilters()
        {
            List<Frequency> Frequencylist = null;
            //Frequency Frequency = null;
            if (ds.Tables[17].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                //var table_Objects = (from row in ds.Tables[17].AsEnumerable()
                //                     select new
                //                     {
                //                         UniqueFilterId = row["UniqueFilterId"],
                //                         Frequency = row["Frequency"]
                //                     }).Distinct().ToList();

                //Frequencylist = new List<Frequency>();
                //foreach (String frequency in (from row in table_Objects select (Convert.ToString(row.Frequency))).Distinct().ToList())
                //{
                //    Frequency = new Frequency();
                //    Frequency = (from row in table_Objects
                //                 where Convert.ToString(frequency).Equals(Convert.ToString(row.Frequency), StringComparison.OrdinalIgnoreCase)
                //                 select new Frequency
                //                 {
                //                     Id = Convert.ToInt16(row.UniqueFilterId),
                //                     Name = Convert.ToString(row.Frequency),
                //                     UniqueId = Convert.ToString(row.UniqueFilterId)
                //                 }).FirstOrDefault();
                //    Frequencylist.Add(Frequency);
                //}

                Frequencylist = (from row in ds.Tables[17].AsEnumerable()
                                 select new Frequency()
                                 {
                                     Id = Convert.ToInt16(row["UniqueFilterId"]),
                                     Name = Convert.ToString(row["Frequency"]),
                                     UniqueId = Convert.ToString(row["UniqueFilterId"])
                                 }).Distinct().ToList();

            }
            return Frequencylist;
        }
        #endregion
        #region Load MonthlyPurchase Filters
        List<MonthlyPurchase> LoadMonthlyPurchase()
        {
            List<MonthlyPurchase> MonthlyPurchaselist = null;
            MonthlyPurchase MonthlyPurchase = null;
            if (ds.Tables[6].Rows.Count > 0)
            {

                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[6].AsEnumerable()
                                     select new
                                     {
                                         UniqueFilterId = row["UniqueFilterId"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         LevelId = row["LevelId"],
                                         ParentId = row["ParentId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"]
                                     }).Distinct().ToList();

                MonthlyPurchaselist = new List<MonthlyPurchase>();
                foreach (String Monthly in (from row in table_Objects select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    MonthlyPurchase = new MonthlyPurchase();
                    MonthlyPurchase = (from row in table_Objects
                                       where Convert.ToString(Monthly).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                       select new MonthlyPurchase
                                       {
                                           Id = Convert.ToInt16(row.MetricId),
                                           Name = Convert.ToString(row.Metric),
                                           LevelId = Convert.ToString(row.LevelId),
                                           UniqueId = Convert.ToString(row.UniqueFilterId),
                                           ParentId = Convert.ToString(row.ParentId)
                                       }).FirstOrDefault();
                    if (MonthlyPurchase != null)
                    {

                        MonthlyPurchase.MonthlyPurchseList = (from row in table_Objects
                                                              where !Convert.ToString(MonthlyPurchase.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                               && !Convert.ToString("").Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                               && Convert.ToString(MonthlyPurchase.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                               && Convert.ToString(MonthlyPurchase.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                                                               select new MonthlyPurchase
                                                               {
                                                                   Id = Convert.ToInt16(row.MetricItemId),
                                                                   Name = Convert.ToString(row.MetricItem),
                                                                   DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem).Trim(),
                                                                   LevelId = Convert.ToString(row.LevelId),
                                                                   UniqueId = Convert.ToString(row.UniqueFilterId),
                                                                   ParentId = Convert.ToString(row.ParentId)
                                                               }).Distinct().ToList();

                        MonthlyPurchaselist.Add(MonthlyPurchase);
                    }
                }

                //foreach (MonthlyPurchase _MonthlyPurchase in MonthlyPurchaselist)
                //{
                //    _MonthlyPurchase.MonthlyPurchseList = (from row in table_Objects
                //                                           where !Convert.ToString(_MonthlyPurchase.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                           && !Convert.ToString("").Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                           && Convert.ToString(_MonthlyPurchase.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                           && Convert.ToString(_MonthlyPurchase.Id).Equals(Convert.ToString(row.MetricId), StringComparison.OrdinalIgnoreCase)
                //                                           select new MonthlyPurchase
                //                                           {
                //                                               Id = Convert.ToInt16(row.MetricItemId),
                //                                               Name = Convert.ToString(row.MetricItem),
                //                                               DBName = commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                //                                               LevelId = Convert.ToString(row.LevelId),
                //                                               UniqueId = Convert.ToString(row.UniqueFilterId),
                //                                               ParentId = Convert.ToString(row.ParentId)
                //                                           }).Distinct().ToList();

                //}
            }
            return MonthlyPurchaselist;
        }
        #endregion
        #region Load Category Or Beverage Filters
        //Get Beverage Image position
        string GetBeverageImagePosition(string BeverageName)
        {
            string imageName = string.Empty;
            string imagePosition = string.Empty;
            switch (BeverageName)
            {
                case "Regular (Non-Diet) Carbonated Soft Drink": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "7px -297px;"; break; }
                case "RTD Coffee": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-72px -147px;"; break; }
                case "RTD Tea": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-112px -147px;"; break; }
                case "Protein Drinks": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-943px -147px;"; break; }
                case "Packaged 100% Orange Juice": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1156px -148px;"; break; }
                case "Sparkling Water (Unflavored)": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1614px -149px;"; break; }
                case "Flavored Sparkling Water": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1688px -149px;"; break; }
                case "Category Nets": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "10px -470px;"; break; }
                case "Detailed Categories": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-36px -470px;"; break; }
                case "SSD Regular": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "8px -149px;"; break; }
                case "SSD Diet": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-30px -149px;"; break; }
                case "RTD Smoothies in a Bottle": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1026px -149px;"; break; }
                case "Packaged 100% Fruit Juice (NON-OJ)": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1240px -147px;"; break; }
                case "Packaged 100% Grape Juice": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-647px -678px;"; break; }
                case "Packaged 100% Apple Juice": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-694px -678px;"; break; }
                case "Packaged 100% Grapefruit Juice": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-740px -678px;"; break; }
                case "Packaged 100% Cranberry Juice": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-792px -678px;"; break; }
                case "Packaged 100% Fruit Juice Blends": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-840px -678px;"; break; }
                case "Vegetable Juice/ Vegetable + Juice Blend": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-92px -471px;"; break; }
                case "Other Flavor 100% Juice": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-885px -678px;"; break; }
                case "Sports Drinks": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1751px -360px;"; break; }
                case "Energy Drink/ Shot": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1791px -360px;"; break; }
                case "Liquid Flavor Enhancer": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-619px -570px;"; break; }
                case "Enhanced Milk": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-90px -678px;"; break; }
                case "Non-Sparkling Water - Nets": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-568px -678px;"; break; }
                case "Sparkling Water - Nets": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-525px -681px;"; break; }
                case "Bottled Water - Nets": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-606px -678px;"; break; }
                case "SSD Regular/Diet": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-485px -681px;"; break; }
                case "RTD Juice Drink": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1493px -147px;"; break; }
                case "Single Serving Bottled Water (Non-Sparkling Water)": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-135px -470px;"; break; }
                case "Flavored Non-Sparkling Bottled Water": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-174px -470px;"; break; }
                case "Juice/Juice Drinks/Vege/Smoothies - Nets": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-195px -677px;"; break; }
                case "Other Trademark/Brand Groups": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-141px -678px;"; break; }
                case "Total": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-932px -678px;"; break; }
                case "Supermarket/Grocery": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1414px -360px;"; break; }
                case "Convenience": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1456px -360px;"; break; }
                case "Drug": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1646px -360px;"; break; }
                case "Dollar": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1603px -360px;"; break; }
                case "Club": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1545px -360px;"; break; }//-320px -48px;
                case "Mass Merc.": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1498px -360px;"; break; }
                case "Supercenter": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1692px -360px;"; break; }
                case "Corporate Nets": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "4px -678px;"; break; }
                case "Channel Nets": { imageName = "../../Images/sprite_filter_icons.svg?id=3"; imagePosition = "-1490px -672px;"; break; }
            }
            return imagePosition;
        }
        Category LoadCategoryOrBeverageFilters()
        {
            Category Category = new Category();
            CategoryOrBeverage CategoryOrBeverage = null;
            CategoryOrBeverageLavel CategoryOrBeverageLavel = null;
            List<string> SearchItems = new List<string>();
            List<string> AllSearchItems = new List<string>();
            StringBuilder BeverageOrCategoryContent = new StringBuilder();
            StringBuilder Lavel0 = new StringBuilder();
            StringBuilder Lavel1 = new StringBuilder();
            StringBuilder Lavel2 = new StringBuilder();
            StringBuilder Lavel3 = new StringBuilder();
            string sImageClassName = string.Empty;
            string strhtml = string.Empty;           

            BeverageOrCategoryContent.Append("<div id=\"BeverageOrCategoryContent\" class=\"Lavel\" style=\"height: 98%;\">");
            BeverageOrCategoryContent.Append("<ul>");          

            Lavel0.Append("<div class=\"Beverage Lavel Lavel0 Sub-Lavel\" style=\"display: none;height:98%;\">");
            Lavel1.Append("<div class=\"Beverage Lavel Lavel1 Sub-Lavel\" style=\"display: none;height:98%;\">");
            Lavel2.Append("<div class=\"Beverage Lavel Lavel2 Sub-Lavel\" style=\"display: none;height:98%;\">");
            Lavel3.Append("<div class=\"Beverage Lavel Lavel3 Sub-Lavel\" style=\"display: none;height:98%;\">");

            if (ds.Tables[5].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[5].AsEnumerable()
                                     select new
                                     {
                                         LevelDispId = row["LevelDispId"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         LevelDesc = row["LevelDesc"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         IsSelectable = row["IsSelectable"],
                                         LevelId = row["LevelId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         Beverage_Shopper_DB_Name = row["Beverage_Shopper_DB_Name"],
                                         Beverage_Trips_DB_Name = row["Beverage_Trips_DB_Name"],
                                         Beverage_Shopper_DB_Name_Brand = row["Beverage_Shopper_DB_Name_Brand"],
                                         Beverage_Trips_DB_Name2 = row["Beverage_Trips_DB_Name2"]
                                     }).Distinct().ToList();

                Category.Lavels = (from row in table_Objects
                                   orderby row.LevelDispId
                                   select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList();

                Category.CategoryOrBeveragelist = new List<CategoryOrBeverage>();
                foreach (String channel in (from row in table_Objects select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    CategoryOrBeverage = new CategoryOrBeverage();
                    CategoryOrBeverage = (from row in table_Objects
                                          where Convert.ToString(channel).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                          select new CategoryOrBeverage
                                          {
                                              Id = Convert.ToInt16(row.MetricId),
                                              Name = Convert.ToString(row.Metric),
                                              FullName = Convert.ToString(row.Metric),
                                              ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                                              TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                                              TopPosition = (Convert.ToString(row.Metric)),
                                              BottomPosition = GetChannelBottomPosition(Convert.ToString(row.Metric)),
                                              LevelDesc = Convert.ToString(row.LevelDesc),
                                              UniqueId = Convert.ToString(row.UniqueFilterId),
                                              isSelectable = Convert.ToString(row.IsSelectable) == "0" ? "false" : "true"
                                          }).FirstOrDefault();

                    BeverageOrCategoryContent.Append("<li>");
                    BeverageOrCategoryContent.Append("<div class=\"FilterStringContainerdiv\">");
                    sImageClassName = GetBeverageImagePosition(CategoryOrBeverage.Name);

                    if (string.IsNullOrEmpty(sImageClassName))
                         BeverageOrCategoryContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>");
                    else
                        BeverageOrCategoryContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>");

                    if (CategoryOrBeverage.isSelectable == "false")
                        BeverageOrCategoryContent.Append("<span type=\"Main-Stub\" Name=\"" + CategoryOrBeverage.Name + "\" DBName=\"" + CategoryOrBeverage.DBName + "\" shopperdbname=\"" + CategoryOrBeverage.ShopperDBName + "\" tripsdbname=\"" + CategoryOrBeverage.TripsDBName + "\" class=\"Comparison\" isselectable=\"" + CategoryOrBeverage.isSelectable + "\" onclick=\"SelectBevComparison(this);\">" + CategoryOrBeverage.Name + "</span>");
                    else
                    {
                        BeverageOrCategoryContent.Append("<span id=\"" + CategoryOrBeverage.UniqueId + "\" type=\"Main-Stub\" Name=\"" + CategoryOrBeverage.Name + "\" DBName=\"" + CategoryOrBeverage.DBName + "\" UniqueId=\"" + CategoryOrBeverage.UniqueId + "\" shopperdbname=\"" + CategoryOrBeverage.ShopperDBName + "\" tripsdbname=\"" + CategoryOrBeverage.TripsDBName + "\" class=\"Comparison\" isselectable=\"" + CategoryOrBeverage.isSelectable + "\" onclick=\"SelectBevComparison(this);\">" + CategoryOrBeverage.Name + "</span>");
                        if (!AllSearchItems.Contains(CategoryOrBeverage.Name))
                        {
                            SearchItems.Add(CategoryOrBeverage.UniqueId + "|" + CategoryOrBeverage.Name);
                            AllSearchItems.Add(CategoryOrBeverage.Name);
                        }
                    }


                    List<string> noOflevels = (from row in table_Objects
                                               where !Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                               && Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                               orderby row.LevelDispId
                                               select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList();


                    BeverageOrCategoryContent.Append("<div class=\"ArrowContainerdiv\"><span Name=\"" + CategoryOrBeverage.Name + "\" lavels=\"" + noOflevels.Count + "\" class=\"sidearrw\" onclick=\"DisplayBevComparisonRetailer(this);\"></span></div>");
                    BeverageOrCategoryContent.Append("</div>");
                    BeverageOrCategoryContent.Append("</li>");                    
                    
                    CategoryOrBeverage.CategoryOrBeverageLavel = new List<CategoryOrBeverageLavel>();
                    foreach (string lavel in (from row in table_Objects
                                              where !Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                              orderby row.LevelDispId
                                              select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList())
                    {
                        CategoryOrBeverageLavel = new CategoryOrBeverageLavel();
                        CategoryOrBeverageLavel.Lavel = lavel;

                        List<RetailerOrBrand> Retailerlist = (from row in table_Objects
                                                              where !Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                              && Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                               && CategoryOrBeverageLavel.Lavel == Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)
                                                              select new RetailerOrBrand
                                                              {
                                                                  Id = Convert.ToInt16(row.MetricItemId),
                                                                  LevelId = Convert.ToInt16(row.LevelId) - 1,
                                                                  Name = Convert.ToString(row.MetricItem),
                                                                  FullName = Convert.ToString(row.MetricItem),
                                                                  ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                                                                  TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                                                                  LevelDesc = Convert.ToString(row.LevelDesc),
                                                                  UniqueId = Convert.ToString(row.UniqueFilterId)
                                                              }).ToList();
                        CategoryOrBeverageLavel.LavelRetailerlist = Retailerlist;
                        CategoryOrBeverage.CategoryOrBeverageLavel.Add(CategoryOrBeverageLavel);
                        strhtml = "<div class=\"RetailerOrBrand\" id=\"" + CategoryOrBeverage.UniqueId + "\" Name=\"" + CategoryOrBeverage.Name + "\" DBName=\"" + CategoryOrBeverage.DBName + "\" style=\"display:none;\"><ul>";
                        foreach (RetailerOrBrand beverage in CategoryOrBeverageLavel.LavelRetailerlist)
                        {
                            strhtml += "<li class=\"Comparison\" id=\"" + beverage.UniqueId + "\" Name=\"" + beverage.Name + "\" DBName=\"" + beverage.DBName + "\" UniqueId=\"" + beverage.UniqueId + "\" shopperdbname=\"" + beverage.ShopperDBName + "\" tripsdbname=\"" + beverage.TripsDBName + "\" onclick=\"SelectBevComparison(this);\">" + beverage.Name + "</li>";

                            if (!AllSearchItems.Contains(beverage.Name))
                            {
                                SearchItems.Add(beverage.UniqueId + "|" + beverage.Name);
                                AllSearchItems.Add(beverage.Name);
                            }
                        }
                        strhtml += "</ul></div>";
                        if (lavel == "0")
                            Lavel0.Append(strhtml);
                        else if (lavel == "1")
                            Lavel1.Append(strhtml);
                        else if (lavel == "2")
                            Lavel2.Append(strhtml);
                        else if (lavel == "3")
                            Lavel3.Append(strhtml);
                    }
                    Category.CategoryOrBeveragelist.Add(CategoryOrBeverage);
                }
                BeverageOrCategoryContent.Append("</ul>");
                BeverageOrCategoryContent.Append("</div>");
                Lavel0.Append("</div>");
                Lavel1.Append("</div>");
                Lavel2.Append("</div>");
                Lavel3.Append("</div>");
                Category.SearchObj = new SearchHTMLEntity();
                Category.SearchObj.HTML_String = BeverageOrCategoryContent.ToString();
                Category.SearchObj.HTML_String += Lavel0.ToString();
                Category.SearchObj.HTML_String += Lavel1.ToString();
                Category.SearchObj.HTML_String += Lavel2.ToString();
                Category.SearchObj.HTML_String += Lavel3.ToString();
                SearchItems = SearchItems.Distinct().ToList();
                Category.SearchObj.SearchItems = SearchItems;
                //foreach (CategoryOrBeverage _channelOrCategory in Category.CategoryOrBeveragelist)
                //{
                //    _channelOrCategory.CategoryOrBeverageLavel = new List<CategoryOrBeverageLavel>();
                //    foreach (string lavel in (from row in table_Objects
                //                              where !Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                              && Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                              orderby row.LevelDispId
                //                              select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList())
                //    {
                //        CategoryOrBeverageLavel = new CategoryOrBeverageLavel();
                //        CategoryOrBeverageLavel.Lavel = lavel;
                //        _channelOrCategory.CategoryOrBeverageLavel.Add(CategoryOrBeverageLavel);
                //    }

                //    foreach (CategoryOrBeverageLavel _channelOrCategoryLavel in _channelOrCategory.CategoryOrBeverageLavel)
                //    {
                //        List<RetailerOrBrand> Retailerlist = (from row in table_Objects
                //                                              where !Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                              && Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                               && _channelOrCategoryLavel.Lavel == Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)
                //                                              select new RetailerOrBrand
                //                                              {
                //                                                  Id = Convert.ToInt16(row.MetricItemId),
                //                                                  LevelId = Convert.ToInt16(row.LevelId) - 1,
                //                                                  Name = Convert.ToString(row.MetricItem),
                //                                                  FullName = Convert.ToString(row.MetricItem),
                //                                                  ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                //                                                  TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                //                                                  LevelDesc = Convert.ToString(row.LevelDesc),
                //                                                  UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                              }).ToList();
                //        _channelOrCategoryLavel.LavelRetailerlist = Retailerlist;
                //    }
                //}
            }
            //added by Nagaraju for HTML string
            //Date: 17-04-2017
            Category.CategoryOrBeveragelist = new List<CategoryOrBeverage>();
            return Category;
        }
        #endregion
        #region BGM Beverage Items
        Category LoadBGMCategoryOrBeverageFilters()
        {
            Category Category = new Category();
            CategoryOrBeverage CategoryOrBeverage = null;
            CategoryOrBeverageLavel CategoryOrBeverageLavel = null;
            List<string> SearchItems = new List<string>();
            List<string> AllSearchItems = new List<string>();
            StringBuilder BeverageOrCategoryContent = new StringBuilder();
            StringBuilder Lavel0 = new StringBuilder();
            StringBuilder Lavel1 = new StringBuilder();
            StringBuilder Lavel2 = new StringBuilder();
            StringBuilder Lavel3 = new StringBuilder();
            string sImageClassName = string.Empty;
            string strhtml = string.Empty;

            BeverageOrCategoryContent.Append("<div id=\"BeverageOrCategoryContent\" class=\"Lavel\" style=\"height: 98%;\">");
            BeverageOrCategoryContent.Append("<ul>");

            Lavel0.Append("<div class=\"Beverage Lavel Lavel0 Sub-Lavel\" style=\"display: none;height:98%;\">");
            Lavel1.Append("<div class=\"Beverage Lavel Lavel1 Sub-Lavel\" style=\"display: none;height:98%;\">");
            Lavel2.Append("<div class=\"Beverage Lavel Lavel2 Sub-Lavel\" style=\"display: none;height:98%;\">");
            Lavel3.Append("<div class=\"Beverage Lavel Lavel3 Sub-Lavel\" style=\"display: none;height:98%;\">");

            if (ds.Tables[18].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[18].AsEnumerable()
                                     where Convert.ToInt16(row["FilterTypeId"]) == 1
                                     select new
                                     {
                                         LevelDispId = row["LevelDispId"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         LevelDesc = row["LevelDesc"],
                                         UniqueFilterId = row["UniqueFilterId"],
                                         IsSelectable = row["IsSelectable"],
                                         LevelId = row["LevelId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         Beverage_Shopper_DB_Name = row["Beverage_Shopper_DB_Name"],
                                         Beverage_Trips_DB_Name = row["Beverage_Trips_DB_Name"],
                                         Beverage_Shopper_DB_Name_Brand = row["Beverage_Shopper_DB_Name_Brand"],
                                         Beverage_Trips_DB_Name2 = row["Beverage_Trips_DB_Name2"]
                                     }).Distinct().ToList();

                Category.Lavels = (from row in table_Objects
                                   orderby row.LevelDispId
                                   select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList();

                Category.CategoryOrBeveragelist = new List<CategoryOrBeverage>();
                foreach (String channel in (from row in table_Objects select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    CategoryOrBeverage = new CategoryOrBeverage();
                    CategoryOrBeverage = (from row in table_Objects
                                          where Convert.ToString(channel).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                          select new CategoryOrBeverage
                                          {
                                              Id = Convert.ToInt16(row.MetricId),
                                              Name = Convert.ToString(row.Metric),
                                              FullName = Convert.ToString(row.Metric),
                                              ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                                              TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                                              TopPosition = (Convert.ToString(row.Metric)),
                                              BottomPosition = GetChannelBottomPosition(Convert.ToString(row.Metric)),
                                              LevelDesc = Convert.ToString(row.LevelDesc),
                                              UniqueId = Convert.ToString(row.UniqueFilterId),
                                              isSelectable = Convert.ToString(row.IsSelectable) == "0" ? "false" : "true"
                                          }).FirstOrDefault();

                    BeverageOrCategoryContent.Append("<li>");
                    BeverageOrCategoryContent.Append("<div class=\"FilterStringContainerdiv\">");
                    sImageClassName = GetBeverageImagePosition(CategoryOrBeverage.Name);

                    if (string.IsNullOrEmpty(sImageClassName))
                        BeverageOrCategoryContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>");
                    else
                        BeverageOrCategoryContent.Append("<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>");

                    if (CategoryOrBeverage.isSelectable == "false")
                        BeverageOrCategoryContent.Append("<span type=\"Main-Stub\" Name=\"" + CategoryOrBeverage.Name + "\" DBName=\"" + CategoryOrBeverage.DBName + "\" shopperdbname=\"" + CategoryOrBeverage.ShopperDBName + "\" tripsdbname=\"" + CategoryOrBeverage.TripsDBName + "\" class=\"Comparison\" isselectable=\"" + CategoryOrBeverage.isSelectable + "\" onclick=\"SelectBevComparison(this);\">" + CategoryOrBeverage.Name + "</span>");
                    else
                    {
                        BeverageOrCategoryContent.Append("<span id=\"" + CategoryOrBeverage.UniqueId + "\" type=\"Main-Stub\" Name=\"" + CategoryOrBeverage.Name + "\" DBName=\"" + CategoryOrBeverage.DBName + "\" UniqueId=\"" + CategoryOrBeverage.UniqueId + "\" shopperdbname=\"" + CategoryOrBeverage.ShopperDBName + "\" tripsdbname=\"" + CategoryOrBeverage.TripsDBName + "\" class=\"Comparison\" isselectable=\"" + CategoryOrBeverage.isSelectable + "\" onclick=\"SelectBevComparison(this);\">" + CategoryOrBeverage.Name + "</span>");
                        if (!AllSearchItems.Contains(CategoryOrBeverage.Name))
                        {
                            SearchItems.Add(CategoryOrBeverage.UniqueId + "|" + CategoryOrBeverage.Name);
                            AllSearchItems.Add(CategoryOrBeverage.Name);
                        }
                    }


                    List<string> noOflevels = (from row in table_Objects
                                               where !Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                               && Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                               orderby row.LevelDispId
                                               select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList();


                    BeverageOrCategoryContent.Append("<div class=\"ArrowContainerdiv\"><span Name=\"" + CategoryOrBeverage.Name + "\" lavels=\"" + noOflevels.Count + "\" class=\"sidearrw\" onclick=\"DisplayBevComparisonRetailer(this);\"></span></div>");
                    BeverageOrCategoryContent.Append("</div>");
                    BeverageOrCategoryContent.Append("</li>");

                    CategoryOrBeverage.CategoryOrBeverageLavel = new List<CategoryOrBeverageLavel>();
                    foreach (string lavel in (from row in table_Objects
                                              where !Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                              orderby row.LevelDispId
                                              select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList())
                    {
                        CategoryOrBeverageLavel = new CategoryOrBeverageLavel();
                        CategoryOrBeverageLavel.Lavel = lavel;

                        List<RetailerOrBrand> Retailerlist = (from row in table_Objects
                                                              where !Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                              && Convert.ToString(CategoryOrBeverage.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                               && CategoryOrBeverageLavel.Lavel == Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)
                                                              select new RetailerOrBrand
                                                              {
                                                                  Id = Convert.ToInt16(row.MetricItemId),
                                                                  LevelId = Convert.ToInt16(row.LevelId) - 1,
                                                                  Name = Convert.ToString(row.MetricItem),
                                                                  FullName = Convert.ToString(row.MetricItem),
                                                                  ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                                                                  TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                                                                  LevelDesc = Convert.ToString(row.LevelDesc),
                                                                  UniqueId = Convert.ToString(row.UniqueFilterId)
                                                              }).ToList();
                        CategoryOrBeverageLavel.LavelRetailerlist = Retailerlist;
                        CategoryOrBeverage.CategoryOrBeverageLavel.Add(CategoryOrBeverageLavel);
                        strhtml = "<div class=\"RetailerOrBrand\" id=\"" + CategoryOrBeverage.UniqueId + "\" Name=\"" + CategoryOrBeverage.Name + "\" DBName=\"" + CategoryOrBeverage.DBName + "\" style=\"display:none;\"><ul>";
                        foreach (RetailerOrBrand beverage in CategoryOrBeverageLavel.LavelRetailerlist)
                        {
                            strhtml += "<li class=\"Comparison\" id=\"" + beverage.UniqueId + "\" Name=\"" + beverage.Name + "\" DBName=\"" + beverage.DBName + "\" UniqueId=\"" + beverage.UniqueId + "\" shopperdbname=\"" + beverage.ShopperDBName + "\" tripsdbname=\"" + beverage.TripsDBName + "\" onclick=\"SelectBevComparison(this);\">" + beverage.Name + "</li>";

                            if (!AllSearchItems.Contains(beverage.Name))
                            {
                                SearchItems.Add(beverage.UniqueId + "|" + beverage.Name);
                                AllSearchItems.Add(beverage.Name);
                            }
                        }
                        strhtml += "</ul></div>";
                        if (lavel == "0")
                            Lavel0.Append(strhtml);
                        else if (lavel == "1")
                            Lavel1.Append(strhtml);
                        else if (lavel == "2")
                            Lavel2.Append(strhtml);
                        else if (lavel == "3")
                            Lavel3.Append(strhtml);
                    }
                    Category.CategoryOrBeveragelist.Add(CategoryOrBeverage);
                }
                BeverageOrCategoryContent.Append("</ul>");
                BeverageOrCategoryContent.Append("</div>");
                Lavel0.Append("</div>");
                Lavel1.Append("</div>");
                Lavel2.Append("</div>");
                Lavel3.Append("</div>");
                Category.SearchObj = new SearchHTMLEntity();
                Category.SearchObj.HTML_String = BeverageOrCategoryContent.ToString();
                Category.SearchObj.HTML_String += Lavel0.ToString();
                Category.SearchObj.HTML_String += Lavel1.ToString();
                Category.SearchObj.HTML_String += Lavel2.ToString();
                Category.SearchObj.HTML_String += Lavel3.ToString();
                SearchItems = SearchItems.Distinct().ToList();
                Category.SearchObj.SearchItems = SearchItems;
                //foreach (CategoryOrBeverage _channelOrCategory in Category.CategoryOrBeveragelist)
                //{
                //    _channelOrCategory.CategoryOrBeverageLavel = new List<CategoryOrBeverageLavel>();
                //    foreach (string lavel in (from row in table_Objects
                //                              where !Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                              && Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                              orderby row.LevelDispId
                //                              select Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)).Distinct().ToList())
                //    {
                //        CategoryOrBeverageLavel = new CategoryOrBeverageLavel();
                //        CategoryOrBeverageLavel.Lavel = lavel;
                //        _channelOrCategory.CategoryOrBeverageLavel.Add(CategoryOrBeverageLavel);
                //    }

                //    foreach (CategoryOrBeverageLavel _channelOrCategoryLavel in _channelOrCategory.CategoryOrBeverageLavel)
                //    {
                //        List<RetailerOrBrand> Retailerlist = (from row in table_Objects
                //                                              where !Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                              && Convert.ToString(_channelOrCategory.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                               && _channelOrCategoryLavel.Lavel == Convert.ToString(Convert.ToDouble(row.LevelDispId) - 1)
                //                                              select new RetailerOrBrand
                //                                              {
                //                                                  Id = Convert.ToInt16(row.MetricItemId),
                //                                                  LevelId = Convert.ToInt16(row.LevelId) - 1,
                //                                                  Name = Convert.ToString(row.MetricItem),
                //                                                  FullName = Convert.ToString(row.MetricItem),
                //                                                  ShopperDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Shopper"),
                //                                                  TripsDBName = Get_Retailer_Beverage_String_Mapping_Name(row, "Trips"),
                //                                                  LevelDesc = Convert.ToString(row.LevelDesc),
                //                                                  UniqueId = Convert.ToString(row.UniqueFilterId)
                //                                              }).ToList();
                //        _channelOrCategoryLavel.LavelRetailerlist = Retailerlist;
                //    }
                //}
            }
            //added by Nagaraju for HTML string
            //Date: 17-04-2017
            Category.CategoryOrBeveragelist = new List<CategoryOrBeverage>();
            return Category;
        }
        #endregion       
        #region BeverageSelection
        List<BeverageSelectiontype> LoadBeverageSelection()
        {
            List<BeverageSelectiontype> BeverageSelectionlist = null;
            //BeverageSelectiontype BeverageSelectiontype = null;
            if (ds.Tables[12].Rows.Count > 0)
            {

                BeverageSelectionlist = (from row in ds.Tables[12].AsEnumerable()
                                         select new BeverageSelectiontype
                                         {
                                             Id = Convert.ToInt16(row["UniqueFilterId"]),
                                             Name = Convert.ToString(row["Selection"]),
                                             UniqueId = Convert.ToString(row["UniqueFilterId"]),
                                             Params = Convert.ToString(row["Params"])
                                         }).ToList();

                //added by Nagaraju for table objects Date: 18-03-2017
                //var table_Objects = (from row in ds.Tables[12].AsEnumerable()
                //                     select new
                //                     {
                //                         UniqueFilterId = row["UniqueFilterId"],
                //                         Selection = row["Selection"],
                //                         Params = row["Params"]
                //                     }).Distinct().ToList();

                //BeverageSelectionlist = new List<BeverageSelectiontype>();
                //foreach (String Selection in (from row in table_Objects select (Convert.ToString(row.Selection))).Distinct().ToList())
                //{
                //    BeverageSelectiontype = new BeverageSelectiontype();
                //    BeverageSelectiontype = (from row in table_Objects
                //                             where Convert.ToString(Selection).Equals(Convert.ToString(row.Selection), StringComparison.OrdinalIgnoreCase)
                //                             select new BeverageSelectiontype
                //                             {
                //                                 Id = Convert.ToInt16(row.UniqueFilterId),
                //                                 Name = Convert.ToString(row.Selection),
                //                                 UniqueId = Convert.ToString(row.UniqueFilterId),
                //                                 Params = Convert.ToString(row.Params)
                //                             }).FirstOrDefault();
                //    if (BeverageSelectiontype != null)
                //        BeverageSelectionlist.Add(BeverageSelectiontype);
                //}
            }
            return BeverageSelectionlist;
        }
        #endregion
        #region Load Advance Analytics Filters
        AdvanceVariables LoadAdvanceAnalyticsFilters(string filtertype)
        {

            AdvanceVariables advanceVariables = new AdvanceVariables();
            advanceVariables.ChannelVariables = new List<AdvanceFilterSlectVariabl>();
            advanceVariables.RetailerVariables = new List<AdvanceFilterSlectVariabl>();


            List<AdvanceFilterSlectVariabl> MetricItemList = null;
            AdvanceFilterSlectVariabl advanceFilterSlectVariabl = null;
            if (ds.Tables[14].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[14].AsEnumerable()
                                     select new
                                     {
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         MetricItem = row["MetricItem"],
                                         MetricItemId = row["MetricItemId"],
                                         SelType = row["SelType"],
                                         FilterType = row["FilterType"],
                                         SelID = row["SelID"],
                                         UniqueFilterId = row["UniqueFilterId"]
                                     }).Distinct().ToList();


                MetricItemList = new List<AdvanceFilterSlectVariabl>();
                foreach (String frequency in (from row in table_Objects where Convert.ToString(row.SelType).Equals("ChannelType", StringComparison.OrdinalIgnoreCase) && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase) select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    advanceFilterSlectVariabl = new AdvanceFilterSlectVariabl();
                    advanceFilterSlectVariabl = (from row in table_Objects
                                                 where Convert.ToString(frequency).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.SelType).Equals("ChannelType", StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                                                 select new AdvanceFilterSlectVariabl
                                                 {
                                                     MetricId = Convert.ToInt16(row.MetricId),
                                                     MetricName = Convert.ToString(row.Metric)
                                                 }).FirstOrDefault();

                    advanceFilterSlectVariabl.MetricItemList = (from row in table_Objects
                                                                where Convert.ToString(advanceFilterSlectVariabl.MetricName).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                 && !Convert.ToString("").Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.SelType).Equals("ChannelType", StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                                                                 select new MetricItemList
                                                                 {
                                                                     MetricItemId = Convert.ToInt16(row.MetricItemId),
                                                                     MetricItemName = Convert.ToString(row.MetricItem),
                                                                     SelId = Convert.ToInt16(row.SelID),
                                                                     SelType = Convert.ToString(row.SelType),
                                                                     UniqueFilterId = Convert.ToInt16(row.UniqueFilterId),
                                                                     TripShopperType = Convert.ToString(filtertype)
                                                                 }).Distinct().ToList();

                    advanceVariables.ChannelVariables.Add(advanceFilterSlectVariabl);
                }

                //foreach (AdvanceFilterSlectVariabl _AdvanceFilterSlectVariabl in advanceVariables.ChannelVariables)
                //{
                //    _AdvanceFilterSlectVariabl.MetricItemList = (from row in table_Objects
                //                                                 where Convert.ToString(_AdvanceFilterSlectVariabl.MetricName).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                 && !Convert.ToString("").Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                 && Convert.ToString(row.SelType).Equals("ChannelType", StringComparison.OrdinalIgnoreCase)
                //                                                 && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                //                                                 select new MetricItemList
                //                                                 {
                //                                                     MetricItemId = Convert.ToInt16(row.MetricItemId),
                //                                                     MetricItemName = Convert.ToString(row.MetricItem),
                //                                                     SelId = Convert.ToInt16(row.SelID),
                //                                                     SelType = Convert.ToString(row.SelType),
                //                                                     UniqueFilterId = Convert.ToInt16(row.UniqueFilterId)
                //                                                 }).Distinct().ToList();

                //}

                //Retailer variables
                foreach (String frequency in (from row in table_Objects where Convert.ToString(row.SelType).Equals("RetailerType", StringComparison.OrdinalIgnoreCase) && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase) select (Convert.ToString(row.Metric))).Distinct().ToList())
                {
                    advanceFilterSlectVariabl = new AdvanceFilterSlectVariabl();
                    advanceFilterSlectVariabl = (from row in table_Objects
                                                 where Convert.ToString(frequency).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                 && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                                                 && Convert.ToString(row.SelType).Equals("RetailerType", StringComparison.OrdinalIgnoreCase)
                                                 select new AdvanceFilterSlectVariabl
                                                 {
                                                     MetricId = Convert.ToInt16(row.MetricId),
                                                     MetricName = Convert.ToString(row.Metric)
                                                 }).FirstOrDefault();

                    advanceFilterSlectVariabl.MetricItemList = (from row in table_Objects
                                                                 where Convert.ToString(advanceFilterSlectVariabl.MetricName).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                 && !Convert.ToString("").Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.SelType).Equals("RetailerType", StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                                                                 select new MetricItemList
                                                                 {
                                                                     MetricItemId = Convert.ToInt16(row.MetricItemId),
                                                                     MetricItemName = Convert.ToString(row.MetricItem),
                                                                     SelId = Convert.ToInt16(row.SelID),
                                                                     SelType = Convert.ToString(row.SelType),
                                                                     UniqueFilterId = Convert.ToInt16(row.UniqueFilterId),
                                                                     TripShopperType = Convert.ToString(filtertype)
                                                                 }).Distinct().ToList();

                    advanceVariables.RetailerVariables.Add(advanceFilterSlectVariabl);
                }

                //foreach (AdvanceFilterSlectVariabl _AdvanceFilterSlectVariabl in advanceVariables.RetailerVariables)
                //{
                //    _AdvanceFilterSlectVariabl.MetricItemList = (from row in table_Objects
                //                                                 where Convert.ToString(_AdvanceFilterSlectVariabl.MetricName).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                                 && !Convert.ToString("").Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                                 && Convert.ToString(row.SelType).Equals("RetailerType", StringComparison.OrdinalIgnoreCase)
                //                                                 && Convert.ToString(row.FilterType).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                //                                                 select new MetricItemList
                //                                                 {
                //                                                     MetricItemId = Convert.ToInt16(row.MetricItemId),
                //                                                     MetricItemName = Convert.ToString(row.MetricItem),
                //                                                     SelId = Convert.ToInt16(row.SelID),
                //                                                     SelType = Convert.ToString(row.SelType),
                //                                                     UniqueFilterId = Convert.ToInt16(row.UniqueFilterId)
                //                                                 }).Distinct().ToList();

                //}
            }
            return advanceVariables;
        }
        #endregion
        #region NonBeverageList
        List<NonBeverageList> LoadNonBeverageList()
        {
            List<NonBeverageList> NonBeverageList = null;
            //NonBeverageList NonBeverageObject = null;
            if (ds.Tables[18].Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                var table_Objects = (from row in ds.Tables[18].AsEnumerable()
                                     select new
                                     {
                                         UniqueFilterId = row["UniqueFilterId"],
                                         FilterTypeId = row["FilterTypeId"],
                                         MetricItemId = row["MetricItemId"],
                                         MetricItem = row["MetricItem"]
                                     }).Distinct().ToList();

                NonBeverageList = (from row in table_Objects where (Convert.ToString(row.FilterTypeId) == "2")
                                   select new NonBeverageList
                                         {
                                             FilterTypeId = Convert.ToString(row.FilterTypeId),
                                             MetrticItemId = Convert.ToString(row.MetricItemId),
                                             MetricItem = Convert.ToString(row.MetricItem),
                                             UniqueId = Convert.ToString(row.UniqueFilterId),
                                         }).ToList();

                //NonBeverageList = new List<NonBeverageList>();
                //NonBeverageObject = new NonBeverageList();
                //foreach (String NonBev in (from row in table_Objects where (Convert.ToString(row.FilterTypeId) == "2") select (Convert.ToString(row.MetricItem))).Distinct().ToList())
                //{
                //    NonBeverageObject = new NonBeverageList();
                //    NonBeverageObject = (from row in table_Objects
                //                         where (Convert.ToString(NonBev).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                //                                         && (Convert.ToString(row.FilterTypeId) == "2"))
                //                         select new NonBeverageList
                //                         {
                //                             FilterTypeId = Convert.ToString(row.FilterTypeId),
                //                             MetrticItemId = Convert.ToString(row.MetricItemId),
                //                             MetricItem = Convert.ToString(row.MetricItem),
                //                             UniqueId = Convert.ToString(row.UniqueFilterId),
                //                         }).FirstOrDefault();
                //    if (NonBeverageObject != null)
                //        NonBeverageList.Add(NonBeverageObject);
                //}
            }
            return NonBeverageList;
        }
        #endregion
        #region GeographyList
        List<GeographyParams> LoadGeographyList()
        {
            List<GeographyParams> GeoList = null;
            GeographyParams GeoObject = null;
            if (ds.Tables[18].Rows.Count > 0)
            {
                GeoList = new List<GeographyParams>();
                GeoObject = new GeographyParams();
                foreach (String Geo in (from row in ds.Tables[11].AsEnumerable() where (Convert.ToString(row["FilterTypeId"]) == "2") select (Convert.ToString(row["MetricItem"]))).Distinct().ToList())
                {
                    GeoObject = new GeographyParams();
                    GeoObject = (from row in ds.Tables[11].AsEnumerable()
                                 where (Convert.ToString(Geo).Equals(Convert.ToString(row["MetricItem"]), StringComparison.OrdinalIgnoreCase)
                                 && !(Convert.ToString(Geo).Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase))
                                 && (Convert.ToString(row["FilterTypeId"]) == "2"))
                                 select new GeographyParams
                                 {
                                     Metric = Convert.ToString(row["Metric"]),
                                     MetricItemId = Convert.ToString(row["MetricItemId"]),
                                     MetricItem = Convert.ToString(row["MetricItem"]),
                                     UniqueId = Convert.ToString(row["UniqueFilterId"]),
                                 }).FirstOrDefault();
                    if (GeoObject != null)
                        GeoList.Add(GeoObject);
                }
            }
            return GeoList;
        }
        #endregion
        #region Load TotalMeasures Filters
        List<PrimaryTotalMetric> LoadTotalMeasures(int TableNo, out TotalMeasureFilterlist TotalMeasure,string MeasureType)
        {
            TotalMeasure = new TotalMeasureFilterlist();
            TotalMeasure.SearchObj = new SearchHTMLEntity();
            TotalMeasure.SearchObj.SearchItems = new List<string>();

            TotalMeasure.SearchObj.ShopperSearchItems = new List<string>();
            TotalMeasure.SearchObj.TripsSearchItems = new List<string>();

            DataTable tbl = ds.Tables[22].Copy();
            tbl.Merge(ds.Tables[21], true, MissingSchemaAction.Ignore);

            StringBuilder measureheader = new StringBuilder();
            StringBuilder measureContent = new StringBuilder();

            if (MeasureType.Equals("shopper", StringComparison.OrdinalIgnoreCase))
            {
                measureheader.Append("<div id=\"TotalMeasureHeaderMainShopper\" class=\"Lavel Sub-Lavel\" style=\"height: 92%;\">");
                measureContent.Append("<div id=\"TotalMeasureHeaderContentShopper\" class=\"TotalMeasureHeaderContent Lavel Sub-Lavel\" style=\"height: 92%;\">");
            }
            else
            {
                measureheader.Append("<div id=\"TotalMeasureHeaderMainTrip\" class=\"Lavel Sub-Lavel\" style=\"height: 92%;\">");
                measureContent.Append("<div id=\"TotalMeasureHeaderContentTrip\" class=\"TotalMeasureHeaderContent Lavel Sub-Lavel\" style=\"height: 92%;\">");
            }
            measureheader.Append("<ul>");
            List<PrimaryTotalMetric> PrimaryTotalMetricList = null;
            PrimaryTotalMetric TotalMetricItem = null;
            //if (ds.Tables[TableNo].Rows.Count > 0)
            if (tbl.Rows.Count > 0)
            {
                //added by Nagaraju for table objects Date: 18-03-2017
                //var table_Objects = (from row in ds.Tables[TableNo].AsEnumerable()
                var table_Objects = (from row in tbl.AsEnumerable()
                                     select new
                                     {
                                         MetricType = row["MetricType"],
                                         MetricTypeId = row["MetricTypeId"],
                                         SelectType = row["SelectType"],
                                         Metric = row["Metric"],
                                         MetricId = row["MetricId"],
                                         UniqueFilterId = row["UniqueFilterId"]
                                     }).Distinct().ToList();

                PrimaryTotalMetricList = new List<PrimaryTotalMetric>();
                //foreach (String MetricType in (from row in ds.Tables[TableNo].AsEnumerable() select (Convert.ToString(row["MetricType"]))).Distinct().ToList())
                foreach (String MetricType in (from row in tbl.AsEnumerable() select (Convert.ToString(row["MetricType"]))).Distinct().ToList())
                {
                    TotalMetricItem = new PrimaryTotalMetric();
                    TotalMetricItem = (from row in table_Objects
                                       where Convert.ToString(MetricType).Equals(Convert.ToString(row.MetricType), StringComparison.OrdinalIgnoreCase)
                                       select new PrimaryTotalMetric
                                       {
                                           Id = Convert.ToString(row.MetricTypeId),
                                           Name = Convert.ToString(row.MetricType),
                                           FullName = Convert.ToString(row.MetricType),
                                           SelectType = Convert.ToString(row.SelectType),
                                           LevelId = Convert.ToString(1),
                                       }).FirstOrDefault();
                    if (TotalMetricItem.LevelId == "1")
                    {
                        measureheader.Append("<li class=\"gouptype\" style=\"display:table;\" filtertype=\"" + (TotalMetricItem.SelectType == "Trip" ? "Visits" : "Shopper") + "\">");
                        measureheader.Append("<div onclick=\"DisplaySecondaryTotalFilter(this);\" filtertype=\"" + (TotalMetricItem.SelectType == "Trip" ? "Visits" : "Shopper") + "\" type=\"" + TotalMetricItem.SelectType
 + "\" style=\"border-bottom: 0;display: flex;\" Name=\"" + TotalMetricItem.Name + "\" id=\"" + TotalMetricItem.Id + "\" class=\"lft-popup-ele FilterStringContainerdiv main-measure\" style=\"\"><span style=\"padding-right:3px;\" class=\"lft-popup-ele-label\" id=\"" + TotalMetricItem.Id + "\" data-val=" + TotalMetricItem.Name + " data-parent=\"\" data-isselectable=\"true\">" + TotalMetricItem.Name + "</span><div class=\"ArrowContainerdiv measure-inactive\"><span style=\"height:0;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>");
                        measureheader.Append("</li>");
                    }
                    TotalMetricItem.Metriclist = (from row in table_Objects
                                                          where Convert.ToString(TotalMetricItem.Name).Equals(Convert.ToString(row.MetricType), StringComparison.OrdinalIgnoreCase)
                                                    && !Convert.ToString("").Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                          select new SecondaryTotalMetrics
                                                          {
                                                              Id = Convert.ToString(row.MetricId),
                                                              Name = Convert.ToString(row.Metric),
                                                              FullName = Convert.ToString(row.Metric),
                                                              SelectType = Convert.ToString(row.SelectType),
                                                              UniqueId = Convert.ToString(row.UniqueFilterId),
                                                              ParentId = Convert.ToString(row.MetricTypeId),
                                                              LevelId = Convert.ToString(2),
                                                          }).Distinct().ToList();

                    PrimaryTotalMetricList.Add(TotalMetricItem);
                    measureContent.Append("<div class=\"DemographicList\" id=\"" + TotalMetricItem.Id + "\" Name=\"" + TotalMetricItem.Name + "\" FullName=\"" + TotalMetricItem.FullName + "\" style=\"overflow-y:none;display:none;position:relative;\"><ul>");
                    foreach (SecondaryTotalMetrics sm in TotalMetricItem.Metriclist)
                    {
                        measureContent.Append("<li Name=\"" + sm.Name + "\" style=\"display:table;border:none;line-height:0%;\">");
                        measureContent.Append("<div id=\"" + sm.Id + "\" onclick=\"SelectTotalMeasure(this);\" type=\"trip\" class=\"lft-popup-ele\" style=\"width:100%;height:auto;\"></span><span class=\"lft-popup-ele-label\" type=\"" + sm.SelectType + "\" FullName=\"" + sm.FullName + "\" type=\"trip\" id=" + sm.Id + "-" + sm.ParentId + " UniqueId=\"" + sm.UniqueId + "\" Name=\"" + sm.Name + "\" parent=\"" + TotalMetricItem.Name + "\" data-isselectable=\"true\">" + sm.Name + "</span></div>");
                        measureContent.Append("</li>");
                        TotalMeasure.SearchObj.SearchItems.Add(sm.UniqueId + "|" + sm.Name);

                        if(TotalMetricItem.SelectType.Equals("Shopper",StringComparison.OrdinalIgnoreCase))
                            TotalMeasure.SearchObj.ShopperSearchItems.Add(sm.UniqueId + "|" + sm.Name);
                        else if (TotalMetricItem.SelectType.Equals("Trip", StringComparison.OrdinalIgnoreCase))
                            TotalMeasure.SearchObj.TripsSearchItems.Add(sm.UniqueId + "|" + sm.Name);
                    }
                    measureContent.Append("</ul></div>");
                }
                measureheader.Append("</ul>");
                measureheader.Append("</div>");
                measureContent.Append("</div>");
                TotalMeasure.SearchObj.HTML_String = measureheader.ToString();
                TotalMeasure.SearchObj.HTML_String += measureContent.ToString();
                //foreach (PrimaryTotalMetric _PrimaryTotalMetricList in PrimaryTotalMetricList)
                //{
                //    _PrimaryTotalMetricList.Metriclist = (from row in table_Objects
                //                                          where Convert.ToString(_PrimaryTotalMetricList.Name).Equals(Convert.ToString(row.MetricType), StringComparison.OrdinalIgnoreCase)
                //                                    && !Convert.ToString("").Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                //                                          select new SecondaryTotalMetrics
                //                                          {
                //                                              Id = Convert.ToString(row.MetricId),
                //                                              Name = Convert.ToString(row.Metric),
                //                                              FullName = Convert.ToString(row.Metric),
                //                                              SelectType = Convert.ToString(row.SelectType),
                //                                              UniqueId = Convert.ToString(row.UniqueFilterId),
                //                                              ParentId = Convert.ToString(row.MetricTypeId),
                //                                              LevelId = Convert.ToString(2),
                //                                          }).Distinct().ToList();

                //}

            }
            //added by Nagaraju for HTML string
            //Date: 17-04-2017
            PrimaryTotalMetricList = new List<PrimaryTotalMetric>();
            return PrimaryTotalMetricList;
        }
        #endregion
        public string GetChannelTopPosition(string Channel)
        {
            string TopPosition = string.Empty;
            if (!string.IsNullOrEmpty(Channel))
            {
                switch (Channel.ToLower())
                {
                    case "total shopper":
                        {
                            TopPosition = "-2550px 0";
                            break;
                        }
                    case "supermarket/grocery":
                        {
                            TopPosition = "-239px -155px";
                            break;
                        }
                    case "corporate nets":
                        {
                            TopPosition = "-123px -739px";
                            break;
                        }
                    case "convenience":
                        {
                            TopPosition = "4px -155px";
                            break;
                        }
                    case "dollar":
                        {
                            TopPosition = "-119px -155px";
                            break;
                        }
                    case "mass merc.":
                        {
                            TopPosition = "-489px -155px";
                            break;
                        }
                    case "drug":
                        {
                            TopPosition = "-611px -155px";
                            break;
                        }
                    case "club":
                        {
                            TopPosition = "-366px -155px";
                            break;
                        }
                    case "supercenter":
                        {
                            TopPosition = "-733px -155px";
                            break;
                        }
                }
            }
            return TopPosition;
        }
        public string GetChannelBottomPosition(string Channel)
        {
            string BottomPosition = string.Empty;
            if (!string.IsNullOrEmpty(Channel))
            {
                switch (Channel.ToLower())
                {
                    case "total shopper":
                        {
                            BottomPosition = "-2550px -85px";
                            break;
                        }
                    case "supermarket/grocery":
                        {
                            BottomPosition = "-242px -236px";
                            break;
                        }
                    case "corporate nets":
                        {
                            BottomPosition = "-121px -821px";
                            break;
                        }
                    case "convenience":
                        {
                            BottomPosition = "3px -236px";
                            break;
                        }
                    case "dollar":
                        {
                            BottomPosition = "-109px -236px";
                            break;
                        }
                    case "mass merc.":
                        {
                            BottomPosition = "-486px -236px";
                            break;
                        }
                    case "drug":
                        {
                            BottomPosition = "-594px -236px";
                            break;
                        }
                    case "club":
                        {
                            BottomPosition = "-350px -236px";
                            break;
                        }
                    case "supercenter":
                        {
                            BottomPosition = "-732px -236px";
                            break;
                        }
                }
            }
            return BottomPosition;
        }
        public string GetAdvancedFilterPosition(string Metric)
        {
            string Position = string.Empty;
            if (!string.IsNullOrEmpty(Metric))
            {
                switch (Metric.ToLower())
                {
                    case "gender":
                        {
                            Position = "-70px -260px";
                            break;
                        }
                    case "age":
                        {
                            Position = "7px -260px";
                            break;
                        }
                    case "age gender":
                        {
                            Position = "7px -260px";
                            break;
                        }
                    case "race/ethnicity":
                        {
                            Position = "-108px -260px";
                            break;
                        }
                    case "hh size - total":
                        {
                            Position = "-260px -260px";
                            break;
                        }
                    case "hh size - adults in hh":
                        {
                            Position = "-260px -260px";
                            break;
                        }
                    case "hh size - children in hh":
                        {
                            Position = "-260px -260px";
                            break;
                        }
                    case "marital status":
                        {
                            Position = "-30px -260px";
                            break;
                        }
                    case "hh income":
                        {
                            Position = "-223px -260px";
                            break;
                        }
                    case "employment status net":
                        {
                            Position = "-300px -260px";
                            break;
                        }
                    case "employment status detailed":
                        {
                            Position = "-300px -260px";
                            break;
                        }
                    case "education":
                        {
                            Position = "5px -305px";
                            break;
                        }
                    case "socioeconomic":
                        {
                            Position = "5px -305px";
                            break;
                        }
                    case "primary shopper":
                        {
                            Position = "-147px -260px";
                            break;
                        }
                    case "other":
                        {
                            Position = "-183px -260px";
                            break;
                        }
                }
            }
            return Position;
        }
    }
}