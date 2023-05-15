using iSHOPNew.DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Microsoft.Ajax.Utilities;
using System.Text;

namespace iSHOPNew.Models
{
    public class LoadECommerceFilters
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        DataAccess da = new DataAccess();
        DataSet ds = null;
        public Filters GetFilters()
        {
            Filters filters = new Filters();
            try
            {
                if (HttpContext.Current.Session[SessionVariables.ECommerceFilterData] != null)
                {
                    filters = HttpContext.Current.Session[SessionVariables.ECommerceFilterData] as Filters;
                    return filters;
                }
                ds = da.GetData(SPVariables.USP_IshopECommercefilters);

                HttpContext.Current.Session[SessionVariables.ECommerceFilterData] = filters;
                if (ds != null && ds.Tables.Count > 0)
                {
                    filters.TimePeriodlist = LoadTimePeriodFiltersFilters(3);
                    filters.TripEcommerceMeasures = GetMeasureDataEcommerce(0);
                    filters.shopperEcommerceMeasures = GetMeasureDataEcommerce(1);
                    filters.SitesList = LoadSitesFilters();
                    filters.RightPanelMeasures = LoadRightFilters();
                    filters.Frequencylist = LoadFrequencyFiltersFilters("Online Order Frequency");
                    filters.TripsFrequencylist = LoadFrequencyFiltersFilters("Online Order Type");
                    filters.BeverageSelection = LoadBeverageSelection();
                    filters.ShopperGroupTypelist = LoadGroupTypeFilters(7, "");
                    filters.ShopperGroupsPrimeFilterlist = GetGroupsPrimeFilters(7);
                }

                //if (HttpContext.Current.Session[SessionVariables.FilterData] != null)
                //{
                //    filters = HttpContext.Current.Session[SessionVariables.FilterData] as Filters;
                //    return filters;
                //}
                ds = da.GetData(SPVariables.USP_Ishopfilters);

                //HttpContext.Current.Session[SessionVariables.FilterData] = filters;
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<List<PrimaryAdvancedFilter>> AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
                    AdvancedFilterLists = LoadAdvancedFilters();
                    filters.AdvancedFilterlist = AdvancedFilterLists[0];
                    filters.VisitAdvancedFilter = AdvancedFilterLists[1];
                    //filters.TripGroupTypelist = LoadGroupTypeFilters(13, "");
                    //filters.ShopperGroupTypelist = LoadGroupTypeFilters(13, "");
                    filters.GeographyList = LoadGeographyList();
                }

                if (HttpContext.Current.Session["CustomRegions"] == null)
                {
                    DataSet dsCR = da.GetData("CustomRegions");
                    HttpContext.Current.Session["CustomRegions"] = dsCR;
                }
            }
            catch(Exception ex)
            {
                filters = null;
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            return filters;
        }
        #region BeverageSelection
        List<BeverageSelectiontype> LoadBeverageSelection()
        {
            List<BeverageSelectiontype> BeverageSelectionlist = null;
            BeverageSelectiontype BeverageSelectiontype = null;
            if (ds.Tables[6].Rows.Count > 0)
            {
                BeverageSelectionlist = new List<BeverageSelectiontype>();
                foreach (String Selection in (from row in ds.Tables[6].AsEnumerable() select (Convert.ToString(row["Selection"]))).Distinct().ToList())
                {
                    BeverageSelectiontype = new BeverageSelectiontype();
                    BeverageSelectiontype = (from row in ds.Tables[6].AsEnumerable()
                                             where Convert.ToString(Selection).Equals(Convert.ToString(row["Selection"]), StringComparison.OrdinalIgnoreCase)
                                             select new BeverageSelectiontype
                                             {
                                                 Id = Convert.ToInt16(row["UniqueFilterId"]),
                                                 Name = Convert.ToString(row["Selection"]),
                                                 UniqueId = Convert.ToString(row["UniqueFilterId"]),
                                                 Params = Convert.ToString(row["Params"])
                                             }).FirstOrDefault();
                    if (BeverageSelectiontype != null)
                        BeverageSelectionlist.Add(BeverageSelectiontype);
                }
            }
            return BeverageSelectionlist;
        }
        #endregion
        List<Frequency> LoadFrequencyFiltersFilters(string filtertype)
        {
            List<Frequency> Frequencylist = null;
            Frequency Frequency = null;
            if (ds.Tables[4].Rows.Count > 0)
            {
                Frequencylist = new List<Frequency>();
                foreach (String frequency in (from row in ds.Tables[4].AsEnumerable()
                                              where Convert.ToString(row["FilterType"]).Equals(filtertype, StringComparison.OrdinalIgnoreCase) 
                                              select (Convert.ToString(row["Metric"]))).Distinct().ToList())
                {
                    Frequency = new Frequency();
                    Frequency = (from row in ds.Tables[4].AsEnumerable()
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
        #region Load Time Period Filters
        List<TimePeriod> LoadTimePeriodFiltersFilters(int TableNumber)
        {
            List<TimePeriod> TimePeriodlist = null;
            TimePeriod timePeriod = null;
            if (ds.Tables[TableNumber].Rows.Count > 0)
            {
                TimePeriodlist = new List<TimePeriod>();
                foreach (String period in (from row in ds.Tables[TableNumber].AsEnumerable() select (Convert.ToString(row["PeriodType"]))).Distinct().ToList())
                {
                    timePeriod = new TimePeriod();
                    timePeriod = (from row in ds.Tables[TableNumber].AsEnumerable()
                                  where Convert.ToString(period).Equals(Convert.ToString(row["PeriodType"]), StringComparison.OrdinalIgnoreCase)
                                  select new TimePeriod
                                  {
                                      Id = Convert.ToInt16(row["TypeId"]),
                                      Name = Convert.ToString(row["PeriodType"]),
                                      UniqueId = Convert.ToString(row["UniqueFilterId"])
                                  }).FirstOrDefault();
                    TimePeriodlist.Add(timePeriod);
                }
                foreach (TimePeriod _timePeriod in TimePeriodlist)
                {
                    _timePeriod.TimePeriodlist = (from row in ds.Tables[TableNumber].AsEnumerable()
                                                  where Convert.ToString(_timePeriod.Name).Equals(Convert.ToString(row["PeriodType"]), StringComparison.OrdinalIgnoreCase)
                                                  select new TimePeriod
                                                  {
                                                      Id = Convert.ToInt16(row["PeriodId"]),
                                                      Name = Convert.ToString(row["Period"]),
                                                      UniqueId = Convert.ToString(row["UniqueFilterId"])
                                                  }).ToList();

                    _timePeriod.Sliderlist = (from row in ds.Tables[TableNumber].AsEnumerable()
                                              where Convert.ToString(_timePeriod.Name).Equals(Convert.ToString(row["PeriodType"]), StringComparison.OrdinalIgnoreCase)
                                              select Convert.ToString(row["Period"])).ToList();

                }
            }
            return TimePeriodlist;
        }
        #endregion
        #region Measure
        public List<SelTypes> GetMeasureDataEcommerce(int TableNumber)
        {
            string seltype = string.Empty;
            List<SelTypes> SelTypelist = new List<DAL.SelTypes>();
            SelTypes Seltype = null;
            List<GroupType> GroupTypelist = new List<GroupType>();

            var table_Objects = (from row in ds.Tables[TableNumber].AsEnumerable()
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
                                         select Convert.ToString(row.SelType)).Distinct().ToList())
            {
                Seltype = new DAL.SelTypes();
                Seltype.SelType = Convert.ToString(_seltype);
                SelTypelist.Add(Seltype);
            }
            foreach (SelTypes _seltype in SelTypelist)
            {
                GroupTypelist = new List<GroupType>();
                foreach (string _group in (from row in table_Objects
                                           where Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                           select Convert.ToString(row.FilterType)).Distinct().ToList())
                {
                    GroupType _GroupType = new GroupType();
                    _GroupType = (from row in table_Objects
                                  where Convert.ToString(row.FilterType).Equals(_group, StringComparison.OrdinalIgnoreCase)
                                  && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                  select new GroupType
                                  {
                                      GroupName = Convert.ToString(row.FilterType),
                                      GroupId = Convert.ToString(row.FilterTypeId)
                                  }).Distinct().FirstOrDefault();
                    GroupTypelist.Add(_GroupType);
                }
                foreach (GroupType _group in GroupTypelist)
                {
                    _group.PrimaryAdvancedFilter = new List<PrimaryAdvancedFilter>();
                    foreach (string measure in (from row in table_Objects
                                                where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                                select Convert.ToString(row.Metric)).Distinct().ToList())
                    {
                        var query = (from row in table_Objects
                                     where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                     && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
                                      && Convert.ToString(row.MetricItem).Equals(measure, StringComparison.OrdinalIgnoreCase)
                                     select Convert.ToString(row.MetricItem)).FirstOrDefault();

                        if (string.IsNullOrEmpty(Convert.ToString(query)))
                        {
                            PrimaryAdvancedFilter pfl = (from row in table_Objects
                                                         where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                          && Convert.ToString(row.Metric).Equals(measure, StringComparison.OrdinalIgnoreCase)
                                                         && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                         && !Convert.ToString(row.Metric).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                         && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
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
                        }
                    }
                    foreach (PrimaryAdvancedFilter primaryAdvancedFilter in _group.PrimaryAdvancedFilter)
                    {
                        List<SecondaryAdvancedFilter> seclist = (from row in table_Objects
                                                                 where Convert.ToString(row.FilterType).Equals(_group.GroupName, StringComparison.OrdinalIgnoreCase)
                                                                && Convert.ToString(row.FilterTypeId).Equals(_group.GroupId, StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.Metric).Equals(Convert.ToString(primaryAdvancedFilter.Name), StringComparison.OrdinalIgnoreCase)
                                                                 && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
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
                                         && Convert.ToString(row.SelType).Equals(_seltype.SelType, StringComparison.OrdinalIgnoreCase)
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
                        }
                    }
                    _seltype.GroupTypelist = GroupTypelist;
                }
            }
            return SelTypelist;
        }
        #endregion
        #region Sites
        List<Sites> LoadSitesFilters()
        {
            List<Sites> Sitelist = null;          
            if (ds.Tables[2].Rows.Count > 0)
            {                
                Sitelist = (from row in ds.Tables[2].AsEnumerable()
                            where Convert.ToInt16(row["LevelId"]) == 1
                            select new Sites
                            {
                                Id = Convert.ToInt16(row["UniqueFilterId"]),
                                MetricItemId = Convert.ToInt16(row["MetricId"]),
                                Name = Convert.ToString(row["Metric"]),
                                LevelId = Convert.ToString(row["LevelId"]),
                                UniqueId = Convert.ToString(row["UniqueFilterId"]),
                            }).ToList();

                foreach (Sites _Sitelist in Sitelist)
                {
                    _Sitelist.SiteList = (from row in ds.Tables[2].AsEnumerable()
                                          where Convert.ToInt16(row["ParentId"]) == _Sitelist.MetricItemId
                                                    select new SitesMetricItem
                                                    {
                                                        Id = Convert.ToInt16(row["UniqueFilterId"]),
                                                        Name = Convert.ToString(row["Metric"]),
                                                        MetricItemId = Convert.ToInt16(row["MetricId"]),
                                                        MetricItemName = Convert.ToString(row["Metric"]),
                                                        LevelId = Convert.ToString(row["LevelId"]),
                                                        ParentId = Convert.ToString(row["ParentId"]),
                                                        UniqueId = Convert.ToString(row["UniqueFilterId"]),
                                                    }).Distinct().ToList();

                }

            }
            return Sitelist;
        }
        #endregion
        #region RightPanelMetric
        List<RightPanelMetrics> LoadRightFilters()
        {
            List<RightPanelMetrics> MetricList = null;
            RightPanelMetrics Metric = null;
            if (ds.Tables[4].Rows.Count > 0)
            {
                MetricList = new List<RightPanelMetrics>();
                foreach (String SiteMetric in (from row in ds.Tables[4].AsEnumerable() select (Convert.ToString(row["FilterType"]))).Distinct().ToList())
                {
                    Metric = new RightPanelMetrics();
                    Metric = (from row in ds.Tables[4].AsEnumerable()
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
                    _MetricList.MetricList = (from row in ds.Tables[4].AsEnumerable()
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
                AdvancedFilterLists = new List<List<PrimaryAdvancedFilter>>();
                PrimaryAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                VisitAdvancedFilterlist = new List<PrimaryAdvancedFilter>();
                foreach (String Metric in (from row in ds.Tables[4].AsEnumerable() where (Convert.ToString(row["FilterTypeId"]) == "1") select (Convert.ToString(row["Metric"]))).Distinct().ToList())
                {
                    PrimaryAdvancedFilter = new PrimaryAdvancedFilter();
                    PrimaryAdvancedFilter = (from row in ds.Tables[4].AsEnumerable()
                                             where ((Convert.ToString(Metric).Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase)) && (Convert.ToString(row["FilterTypeId"]) == "1"))
                                             select new PrimaryAdvancedFilter
                                             {
                                                 Id = Convert.ToInt16(row["MetricId"]),
                                                 Name = Convert.ToString(row["Metric"]),
                                                 FullName = Convert.ToString(row["Metric"]),
                                                 Position = GetAdvancedFilterPosition(Convert.ToString(row["Metric"])),
                                                 ParentId = Convert.ToString(row["ParentId"]),
                                                 Level = Convert.ToString(row["LevelId"]),
                                                 UniqueId = Convert.ToString(row["UniqueFilterId"])
                                             }).FirstOrDefault();
                    if (PrimaryAdvancedFilter != null)
                        PrimaryAdvancedFilterlist.Add(PrimaryAdvancedFilter);

                }

                foreach (String Metric in (from row in ds.Tables[4].AsEnumerable() where (Convert.ToString(row["FilterTypeId"]) == "3") select (Convert.ToString(row["Metric"]))).Distinct().ToList())
                {
                    VisitAdvancedFilter = new PrimaryAdvancedFilter();


                    VisitAdvancedFilter = (from row in ds.Tables[4].AsEnumerable()
                                           where ((Convert.ToString(Metric).Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase)) && (Convert.ToString(row["FilterTypeId"]) == "3"))
                                           select new PrimaryAdvancedFilter
                                           {
                                               Id = Convert.ToInt16(row["MetricId"]),
                                               Name = Convert.ToString(row["Metric"]),
                                               FullName = Convert.ToString(row["Metric"]),
                                               Position = GetAdvancedFilterPosition(Convert.ToString(row["Metric"])),
                                               ParentId = Convert.ToString(row["ParentId"]),
                                               Level = Convert.ToString(row["LevelId"]),
                                               UniqueId = Convert.ToString(row["UniqueFilterId"])
                                           }).FirstOrDefault();
                    if (VisitAdvancedFilter != null)
                        VisitAdvancedFilterlist.Add(VisitAdvancedFilter);
                }

                foreach (PrimaryAdvancedFilter _PrimaryAdvancedFilter in PrimaryAdvancedFilterlist)
                {
                    _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in ds.Tables[4].AsEnumerable()
                                                                          where !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row["MetricItem"]), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(_PrimaryAdvancedFilter.Id).Equals(Convert.ToString(row["MetricId"]), StringComparison.OrdinalIgnoreCase)
                                                                          select new SecondaryAdvancedFilter
                                                                          {
                                                                              Id = Convert.ToString(row["MetricItemId"]),
                                                                              Name = Convert.ToString(row["MetricItem"]),
                                                                              FullName = Convert.ToString(row["MetricItem"]),
                                                                              MetricId = Convert.ToString(row["MetricId"]),
                                                                              ParentId = Convert.ToString(row["ParentId"]),
                                                                              DBName = commonFunctions.GetDBMappingName(Convert.ToString(row["Metric"])) + "|" + Convert.ToString(row["MetricItem"]),
                                                                              UniqueId = Convert.ToString(row["UniqueFilterId"])
                                                                          }).ToList();

                }

                foreach (PrimaryAdvancedFilter _VisitsAdvancedFilter in VisitAdvancedFilterlist)
                {
                    _VisitsAdvancedFilter.SecondaryAdvancedFilterlist = (from row in ds.Tables[4].AsEnumerable()
                                                                         where !Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row["MetricItem"]), StringComparison.OrdinalIgnoreCase)
                                                                         && Convert.ToString(_VisitsAdvancedFilter.Name).Equals(Convert.ToString(row["Metric"]), StringComparison.OrdinalIgnoreCase)
                                                                         && Convert.ToString(_VisitsAdvancedFilter.Id).Equals(Convert.ToString(row["MetricId"]), StringComparison.OrdinalIgnoreCase)
                                                                         select new SecondaryAdvancedFilter
                                                                         {
                                                                             Id = Convert.ToString(row["MetricItemId"]),
                                                                             Name = Convert.ToString(row["MetricItem"]),
                                                                             FullName = Convert.ToString(row["MetricItem"]),
                                                                             MetricId = Convert.ToString(row["MetricId"]),
                                                                             ParentId = Convert.ToString(row["ParentId"]),
                                                                             DBName = commonFunctions.GetDBMappingName(Convert.ToString(row["Metric"])) + "|" + Convert.ToString(row["MetricItem"]),
                                                                             UniqueId = Convert.ToString(row["UniqueFilterId"])
                                                                         }).ToList();

                }
                AdvancedFilterLists.Add(PrimaryAdvancedFilterlist);
                AdvancedFilterLists.Add(VisitAdvancedFilterlist);
            }
            return AdvancedFilterLists;
        }
        #endregion
        #region Load Group Type Filters
        List<PrimaryAdvancedFilter> LoadGroupTypeFilters(int TableNumber, string selecttype)
        {
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
                    PrimaryAdvancedFilterlist.Add(PrimaryAdvancedFilter);
                }
                foreach (PrimaryAdvancedFilter _PrimaryAdvancedFilter in PrimaryAdvancedFilterlist)
                {
                    _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist = (from row in table_Objects
                                                                          where !Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.MetricItem), StringComparison.OrdinalIgnoreCase)
                                                                          && Convert.ToString(_PrimaryAdvancedFilter.Name).Equals(Convert.ToString(row.Metric), StringComparison.OrdinalIgnoreCase)
                                                                          //&& Convert.ToString(row.PrimeFilterTypeId) == PrimaryFilter
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
                                                                              DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                                                                              UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                              PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                              PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                                              FilterType = Convert.ToString(row.FilterType)
                                                                          }).Distinct().ToList();

                    foreach (SecondaryAdvancedFilter secfil in _PrimaryAdvancedFilter.SecondaryAdvancedFilterlist)
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
                                                                              DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                                                                              UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                              PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                              FilterType = Convert.ToString(row.FilterType)
                                                                          }).Distinct().ToList();
                        if (query != null && query.Count > 0)
                        {
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
                                                                      DBName = (row.Metric == null || row.MetricItem == null) ? "" : commonFunctions.GetDBMappingName(Convert.ToString(row.Metric)) + "|" + Convert.ToString(row.MetricItem),
                                                                      UniqueId = row.UniqueFilterId == null ? "" : Convert.ToString(row.UniqueFilterId),
                                                                      PrimeFilterType = Convert.ToString(row.PrimeFilterType),
                                                                      PrimeFilterTypeId = Convert.ToString(row.PrimeFilterTypeId),
                                                                      FilterType = Convert.ToString(row.FilterType)
                                                                  }).Distinct().ToList();
                        }
                    }

                }
            }
            }
            return PrimaryAdvancedFilterlist;
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