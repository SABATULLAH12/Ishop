using iSHOPNew.DAL;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;

namespace iSHOPNew.Models
{
    public class LoadLeftPanelFilters
    {
        CommonFunctions commonFunctions = new CommonFunctions();
        DataAccess da = new DataAccess();
        DataSet ds = null;
        DataSet dsCR = null;
        #region Load Geography Filters
        List<Filter> LoadBeverages(int tableNo, string groupName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();

                filter.Levels.Add(new Level() { Id = 1 });

                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["UniqueFilterId"]),
                                                   ParentId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Name = Convert.ToString(row["Selection"]),
                                                   searchName = Convert.ToString(row["Selection"]),
                                                   ParentName = "BEVERAGE PURCHASER LEVEL",
                                                   IsSelectable = true,
                                                   HasSubLevel = false
                                               }).ToList();

                filters.Add(filter);
            }
            return filters;
        }
        public List<Filter> LoadGeography(string TagName, string TimePeriod, string TimePeriodType, string CheckModule, string filterName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (HttpContext.Current.Session["CustomRegions"] == null)
            {
                dsCR = da.GetData("CustomRegions");
                HttpContext.Current.Session["CustomRegions"] = dsCR;
            }
            if (HttpContext.Current.Session["CustomRegions"] != null)
            {
                dsCR = HttpContext.Current.Session["CustomRegions"] as DataSet;

                if (TimePeriodType.Equals("Total Time", StringComparison.OrdinalIgnoreCase) || TimePeriodType.Equals("Year", StringComparison.OrdinalIgnoreCase))
                    TimePeriod = TimePeriodType;

                //load levels
                filter = new Filter();
                filter.Name = filterName;
                filter.Levels = new List<Level>();
                filter.Levels.AddRange((from row in dsCR.Tables[4].AsEnumerable()
                                        orderby row["LevelId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }                
                    //filter.Levels[0].LevelItems.Add(new LevelItems()
                    //{
                    //    Id = 4000,
                    //    ParentId = 100,
                    //    ParentName = "GEOGRAPHY",
                    //    Name = "Total",
                    //    searchName = "Total",
                    //    FilterType = "Demographics",
                    //    FilterTypeId = 0,
                    //    PrimeFilterType = "",
                    //    PrimeFilterTypeId = 0,
                    //    UniqueId = 4000,
                    //    IsSelectable = true,
                    //    HasSubLevel = false,
                    //    IsGeography = true
                    //});
                //add level filters
                foreach (Level level in filter.Levels)
                {
                    level.LevelItems.AddRange((from row in dsCR.Tables[4].AsEnumerable()
                                               where level.Id == Convert.ToInt32(row["LevelId"])
                                               select new LevelItems()
                                               {
                                                   ParentId = string.IsNullOrEmpty(Convert.ToString(row["ParentId"])) ? 0 : Convert.ToInt32(row["ParentId"]),
                                                   Id = string.IsNullOrEmpty(Convert.ToString(row["RegionId"])) ? 0 : Convert.ToInt32(row["RegionId"]),
                                                   Name = Convert.ToString(row["Regions"]),
                                                   searchName = Convert.ToString(row["Regions"]),
                                                   UniqueId = string.IsNullOrEmpty(Convert.ToString(row["UniqueFilterId"])) ? 0 : Convert.ToInt32(row["UniqueFilterId"]),
                                                   ParentName = Convert.ToString(row["DispParentName"]),
                                                   IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                   HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                   ToolTip = GetRegionToolTip(Convert.ToBoolean(row["HasSubLevel"]), Convert.ToString(row["Regions"]), TimePeriodType, TimePeriod, false),
                                                   TrendToolTip = GetRegionToolTip(Convert.ToBoolean(row["HasSubLevel"]), Convert.ToString(row["Regions"]), TimePeriodType, TimePeriod, true),
                                                   GeoTimePeriods = GetGeoTimePeriods(Convert.ToString(row["Regions"]), false),
                                                   TrendGeoTimePeriods = GetGeoTimePeriods(Convert.ToString(row["Regions"]), true),
                                                   IsGeography = true
                                               }).Distinct().ToList());

                }
                filters.Add(filter);
            }
            return filters;
        }
        #endregion
        List<string> GetGeoTimePeriods(string Region, bool IsTrend = false)
        {
            List<string> geoTimePeriods = new List<string>();
            if (!IsTrend)
            {
                geoTimePeriods.AddRange((from r in dsCR.Tables[6].AsEnumerable()
                                         where Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                                         select Convert.ToString(r["TimePeriodType"]) + "|" + Convert.ToString(r["TimePeriod"])));

                geoTimePeriods.AddRange((from r in dsCR.Tables[5].AsEnumerable()
                                         where Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                                         select Convert.ToString(r["TimePeriodType"]) + "|" + Convert.ToString(r["TimePeriod"])));
            }
            else
            {
                geoTimePeriods.AddRange((from r in dsCR.Tables[8].AsEnumerable()
                                         where Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                                         select Convert.ToString(r["TimePeriodType"])));

                geoTimePeriods.AddRange((from r in dsCR.Tables[7].AsEnumerable()
                                         where Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                                         select Convert.ToString(r["TimePeriodType"])));
            }
            geoTimePeriods = geoTimePeriods.Distinct().ToList();
            return geoTimePeriods;
        }
        string GetRegionToolTip(bool HasSubLevel, string Region, string TimePeriodType, string TimePeriod, bool IsTrend = false)
        {
            string Tooltip = string.Empty;
            if (1==1)
            {
                if (!IsTrend)
                {
                    Tooltip = (from r in dsCR.Tables[6].AsEnumerable()
                               where Convert.ToString(r["TimePeriodType"]).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                               && Convert.ToString(r["TimePeriod"]).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                               && Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                               select Convert.ToString(r["ToolTip"])).FirstOrDefault();

                    if (string.IsNullOrEmpty(Tooltip))
                    {
                        Tooltip = (from r in dsCR.Tables[6].AsEnumerable()
                                   where Convert.ToString(r["Regions"]).Equals(Convert.ToString(Region), StringComparison.OrdinalIgnoreCase)
                                   select Convert.ToString(r["ToolTip"])).FirstOrDefault();
                    }
                    //if (string.IsNullOrEmpty(Tooltip))
                    //{
                    //    Tooltip = (from r in dsCR.Tables[5].AsEnumerable()
                    //               where Convert.ToString(r["TimePeriodType"]).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                    //               && Convert.ToString(r["TimePeriod"]).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                    //               && Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                    //               select Convert.ToString(r["ToolTip"])).FirstOrDefault();
                    //}
                }
                else
                {
                    Tooltip = (from r in dsCR.Tables[8].AsEnumerable()
                               where Convert.ToString(r["TimePeriodType"]).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                               && Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                               select Convert.ToString(r["ToolTip"])).FirstOrDefault();

                    if (string.IsNullOrEmpty(Tooltip))
                    {
                        Tooltip = (from r in dsCR.Tables[8].AsEnumerable()
                                   where Convert.ToString(r["Regions"]).Equals(Convert.ToString(Region), StringComparison.OrdinalIgnoreCase)
                                   select Convert.ToString(r["ToolTip"])).FirstOrDefault();
                    }
                    //if (string.IsNullOrEmpty(Tooltip))
                    //{
                    //    Tooltip = (from r in dsCR.Tables[7].AsEnumerable()
                    //               where Convert.ToString(r["TimePeriodType"]).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                    //               && Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                    //               select Convert.ToString(r["ToolTip"])).FirstOrDefault();
                    //}

                }
            }
            return Tooltip;
        }
        bool GetRegionAcctiveStatus(bool HasSubLevel, string Region, string TimePeriodType, string TimePeriod)
        {
            bool IsActive = false;
            string Tooltip = string.Empty;
            if (HasSubLevel)
            {
                Tooltip = (from r in dsCR.Tables[6].AsEnumerable()
                           where Convert.ToString(r["TimePeriodType"]).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                           && Convert.ToString(r["TimePeriod"]).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                           && Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                           select Convert.ToString(r["ToolTip"])).FirstOrDefault();
            }
            else
            {
                Tooltip = (from r in dsCR.Tables[6].AsEnumerable()
                           where Convert.ToString(r["TimePeriodType"]).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                           && Convert.ToString(r["TimePeriod"]).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                           && Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                           select Convert.ToString(r["ToolTip"])).FirstOrDefault();

                if (string.IsNullOrEmpty(Tooltip))
                {
                    Tooltip = (from r in dsCR.Tables[6].AsEnumerable()
                               where Convert.ToString(r["Regions"]).Equals(Convert.ToString(Region), StringComparison.OrdinalIgnoreCase)
                               select Convert.ToString(r["ToolTip"])).FirstOrDefault();
                }
            }
            if (string.IsNullOrEmpty(Tooltip))
            {
                Tooltip = (from r in dsCR.Tables[5].AsEnumerable()
                           where Convert.ToString(r["TimePeriodType"]).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                           && Convert.ToString(r["TimePeriod"]).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                           && Convert.ToString(r["Regions"]).Equals(Region, StringComparison.OrdinalIgnoreCase)
                           select Convert.ToString(r["ToolTip"])).FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(Tooltip))
            {
                IsActive = true;
            }
            return IsActive;
        }
        #region Load Time Period Filters
        List<TimePeriod> LoadTimePeriod(int TableNumber)
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
            }
            return TimePeriodlist;
        }
        #endregion
        #region Load Frequency Filters
        List<Filter> LoadFrequency(int tableNo, string groupName, List<string> frequencyTypes)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();

                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });

                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where frequencyTypes.Contains(Convert.ToString(row["Frequency"]), StringComparer.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["FrequencyId"]),
                                                   ParentId = Convert.ToInt32(row["FrequencyId"]),
                                                   Name = Convert.ToString(row["Frequency"]),
                                                   searchName = Convert.ToString(row["Frequency"]),
                                                   FilterType = Convert.ToString(row["Frequency"]),
                                                   IsSelectable = Convert.ToString(row["Frequency"]).Equals("MAIN STORE/FAVORITE STORE", StringComparison.OrdinalIgnoreCase) ? false : true,
                                                   HasSubLevel = Convert.ToString(row["Frequency"]).Equals("MAIN STORE/FAVORITE STORE", StringComparison.OrdinalIgnoreCase) ? true : false,
                                                   IsShowInSearch = Convert.ToString(row["Frequency"]).Equals("MAIN STORE/FAVORITE STORE", StringComparison.OrdinalIgnoreCase) ? false : true,
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();


                filter.Levels[1].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where frequencyTypes.Contains(Convert.ToString(row["Frequency"]), StringComparer.OrdinalIgnoreCase)
                                               && !string.IsNullOrEmpty(Convert.ToString(row["SubFrequency"]))
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["FrequencyId"]),
                                                   ParentId = Convert.ToInt32(row["FrequencyId"]),
                                                   Name = Convert.ToString(row["SubFrequency"]),
                                                   searchName = Convert.ToString(row["SubFrequency"]),
                                                   ParentName = Convert.ToString(row["Frequency"]),
                                                   FilterType = Convert.ToString(row["SubFrequency"]),
                                                   IsSelectable = true,
                                                   HasSubLevel = false,
                                                   IsShowInSearch = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                filters.Add(filter);
            }
            return filters;
        }

        List<Filter> LoadCompetitorFrequency(int tableNo, string groupName, List<string> frequencyTypes, bool IsAddOnlineFrequency = false)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();

                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });

                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1 
                                               && frequencyTypes.Contains(Convert.ToString(row["Frequency"]), StringComparer.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),                                               
                                                   Id = Convert.ToInt32(row["UniqueFilterId"]),
                                                   FrequencyId = Convert.ToInt32(row["FrequencyId"]),
                                                   ParentId = Convert.ToInt32(row["ParentId"]),
                                                   Name = Convert.ToString(row["Frequency"]),
                                                   searchName = Convert.ToString(row["Frequency"]),
                                                   FilterType = "FREQUENCY",
                                                   IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                   HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                   IsShowInSearch = false
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();


                filter.Levels[1].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 2
                                               && (Convert.ToString(row["Frequency"]) != "Online" || IsAddOnlineFrequency == true)
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["UniqueFilterId"]),
                                                   FrequencyId = Convert.ToInt32(row["FrequencyId"]),
                                                   ParentName = Convert.ToString(row["ParentName"]),
                                                   ParentId = Convert.ToInt32(row["ParentId"]),
                                                   Name = Convert.ToString(row["Frequency"]),
                                                   searchName = Convert.ToString(row["Frequency"])== "SELECTION" ? Convert.ToString(row["ParentName"]) : Convert.ToString(row["Frequency"]),
                                                   FilterType = "FREQUENCY",
                                                   IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                   HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                   IsShowInSearch = Convert.ToString(row["Frequency"]).ToLower() != "cross-retailer shopper"
                                               }).ToList();

               

                filters.Add(filter);
            }
            return filters;
        }

        void LoadComptitrFrequencyInFilters(int tableNo, List<string> frequencyTypes, Filter filter, string ParentName, int LevelId, int ParentID, bool IsAddOnlineFrequency = false)
        {
            //List<Filter> filters = new List<Models.Filter>();

            if (ds != null && ds.Tables.Count > 0)
            {
                filter.Levels[LevelId].LevelItems.Add(new LevelItems()
                {
                    Id = -1001,
                    ParentId = ParentID,
                    ParentName = ParentName,
                    Name = "FREQUENCY",
                    FilterType = "FREQUENCY",
                    IsSelectable = false,
                    HasSubLevel = true,
                });

                filter.Levels[LevelId + 1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               && frequencyTypes.Contains(Convert.ToString(row["Frequency"]), StringComparer.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["UniqueFilterId"]),
                                                   FrequencyId = Convert.ToInt32(row["FrequencyId"]),
                                                   ParentId = -1001,
                                                   Name = Convert.ToString(row["Frequency"]),
                                                   ParentName = "FREQUENCY",
                                                   searchName = Convert.ToString(row["Frequency"]),
                                                   FilterType = "FREQUENCY",
                                                   IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                   HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                   IsShowInSearch = false
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());


                filter.Levels[LevelId + 2].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 2
                                               && (Convert.ToString(row["Frequency"]) != "Online" || IsAddOnlineFrequency==true)
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["UniqueFilterId"]),
                                                   FrequencyId = Convert.ToInt32(row["FrequencyId"]),
                                                   ParentName = Convert.ToString(row["ParentName"]),
                                                   ParentId = Convert.ToInt32(row["ParentId"]),
                                                   Name = Convert.ToString(row["Frequency"]),
                                                   searchName = Convert.ToString(row["Frequency"]) == "SELECTION" ? Convert.ToString(row["ParentName"]) : Convert.ToString(row["Frequency"]),
                                                   FilterType = "FREQUENCY",
                                                   IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                   HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                   IsShowInSearch = Convert.ToString(row["Frequency"]).ToLower() != "cross-retailer shopper"
                                               }).ToList());
            }
        }
        
        List<Filter> LoadBeverageFrequency(int tableNo, string groupName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelId"])
                                        }
                                ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());
                filter.Levels.Add(new Level() { Id = 3 });

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }
                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["MetricId"]),
                                                   searchName = Convert.ToString(row["Metric"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   IsSelectable = Convert.ToString(row["Metric"]).Equals("FAVORITE BRAND", StringComparison.OrdinalIgnoreCase) ? false : true,
                                                   HasSubLevel = Convert.ToString(row["Metric"]).Equals("FAVORITE BRAND", StringComparison.OrdinalIgnoreCase) ? true : false,
                                                   IsShowInSearch = Convert.ToString(row["Metric"]).Equals("FAVORITE BRAND", StringComparison.OrdinalIgnoreCase) ? false : true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                filter.Levels[1].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               && !string.IsNullOrEmpty(Convert.ToString(row["MetricItem"]))
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["MetricItemId"]),
                                                   ParentId = Convert.ToInt32(row["MetricId"]),
                                                   searchName = Convert.ToString(row["MetricItem"]),
                                                   Name = Convert.ToString(row["MetricItem"]),
                                                   ParentName = "FAVORITE BRAND",
                                                   IsSelectable = false,
                                                   HasSubLevel = true,
                                                   IsShowInSearch = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                   where (level.Id - 1) == Convert.ToInt32(row["LevelId"])
                                                   select new LevelItems()
                                                   {
                                                       UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                       Id = Convert.ToInt32(row["MetricItemId"]),
                                                       ParentId = Convert.ToInt32(row["ParentId"]),
                                                       Name = Convert.ToString(row["MetricItem"]),
                                                       searchName = Convert.ToString(row["MetricItem"]),
                                                       ParentName = "FAVOURITE " + Convert.ToString(row["Metric"]).Replace("/"," "),
                                                       IsSelectable = true,
                                                       HasSubLevel = false,
                                                       IsShowInSearch = true
                                                   }).Distinct().ToList());
                    }
                }

                filters.Add(filter);
            }
            return filters;
        }

        void LoadGroupFrequency(int tableNo, List<string> frequencyTypes, Filter filter, string ParentName, int LevelId, int ParentID)
        {
            List<Filter> filters = new List<Models.Filter>();

            if (ds != null && ds.Tables.Count > 0)
            {
                filter.Levels[LevelId].LevelItems.Add(new LevelItems()
                {
                    Id = -1000,
                    ParentId = ParentID,
                    ParentName = ParentName,
                    Name = "FREQUENCY",
                    FilterType = "FREQUENCY",
                    IsSelectable = false,
                    HasSubLevel = true,
                });

                filter.Levels[LevelId + 1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                                where frequencyTypes.Contains(Convert.ToString(row["Frequency"]), StringComparer.OrdinalIgnoreCase)
                                                                select new LevelItems()
                                                                {
                                                                    UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                                    Id = Convert.ToInt32(row["FrequencyId"]),
                                                                    ParentId = -1000,
                                                                    ParentName = "FREQUENCY",
                                                                    Name = Convert.ToString(row["Frequency"]),
                                                                    searchName = Convert.ToString(row["Frequency"]),
                                                                    FilterType = "FREQUENCY",
                                                                    IsSelectable = Convert.ToString(row["Frequency"]).Equals("MAIN STORE/FAVORITE STORE", StringComparison.OrdinalIgnoreCase) ? false : true,
                                                                    HasSubLevel = Convert.ToString(row["Frequency"]).Equals("MAIN STORE/FAVORITE STORE", StringComparison.OrdinalIgnoreCase) ? true : false,
                                                                    IsFrequency = true,
                                                                    IsShowInSearch = Convert.ToString(row["Frequency"]).Equals("MAIN STORE/FAVORITE STORE", StringComparison.OrdinalIgnoreCase) ? false : true
                                                                }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());


            }
        }
        #endregion
        List<Filter> LoadBGMFrequency(int tableNo, string frequencyName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = frequencyName;
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                filter.Levels[0].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                      select new LevelItems()
                                                      {
                                                          ParentId = Convert.ToInt32(row["FrequencyId"]),
                                                          Name = Convert.ToString(row["Frequency"]),
                                                          searchName = Convert.ToString(row["Frequency"]),
                                                          ParentName = Convert.ToString(row["Frequency"]),
                                                          UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                          IsSelectable = true,
                                                          HasSubLevel = false,
                                                          IsShowInSearch = true
                                                      }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());

                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadChannelRetailer(int tableNo)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = "Retailers";
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where Convert.ToInt32(row["LevelDispId"]) != 0 && Convert.ToInt32(row["LevelDispId"]) != 1
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    filter.Levels[0].LevelItems.Add((from row in ds.Tables[tableNo].AsEnumerable()
                                                     where Convert.ToString(row["Channel"]).Equals(channel,StringComparison.OrdinalIgnoreCase)
                                                     && Convert.ToString(row["Metric"]).Equals(channel, StringComparison.OrdinalIgnoreCase)
                                                     select new LevelItems()
                                                     {
                                                         Id = Convert.ToInt32(row["ChannelId"]),
                                                         ParentId = Convert.ToInt32(row["ChannelId"]),
                                                         Name = Convert.ToString(row["Channel"]),
                                                         ParentName = Convert.ToString(row["Channel"]),
                                                         UniqueId = (Convert.ToString(row["Channel"]).Equals("Total",StringComparison.OrdinalIgnoreCase) ? 1 : Convert.ToInt32(row["UniqueFilterId"])),
                                                         IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                         IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                         LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                         searchName = Convert.ToString(row["Channel"]),
                                                         IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                         HasSubLevel = true,
                                                         IsShowImage = true,
                                                         ShowAll = true
                                                     }).FirstOrDefault());
                }
                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id != 1)
                        {
                            //add priority header item    
                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 2)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                var priorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                        where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                        && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt32(row["MetricId"]) != 0
                                                        && Convert.ToInt16(row["IsPriority"]) == 1
                                                        select row).FirstOrDefault();

                                if (priorityRetailer != null && Convert.ToBoolean(priorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(priorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(priorityRetailer["ChannelId"]),
                                        Name = "PRIORITY",
                                        ParentName = Convert.ToString(priorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(priorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                            }
                            level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                       where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                       && Convert.ToInt32(row["MetricId"]) != 0
                                                       && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt16(row["IsPriority"]) == 1
                                                       orderby row["IsPriority"] descending
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["MetricId"]),
                                                           ParentId = Convert.ToInt32(row["ChannelId"]),
                                                           Name = Convert.ToString(row["Metric"]),
                                                           searchName = Convert.ToString(row["Metric"]),
                                                           ParentName = Convert.ToString(row["Channel"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                           IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                       }).Distinct().ToList());

                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 2)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                //add non priority header item      
                                var nonpriorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                           where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                           && channel == Convert.ToString(row["Channel"])
                                                           && Convert.ToInt32(row["MetricId"]) != 0
                                                           && Convert.ToInt16(row["IsPriority"]) == 0
                                                           select row).FirstOrDefault();

                                if (nonpriorityRetailer != null && !Convert.ToBoolean(nonpriorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(nonpriorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(nonpriorityRetailer["ChannelId"]),
                                        Name = "RETAILERS",
                                        ParentName = Convert.ToString(nonpriorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(nonpriorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                            }
                            level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                       where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                       && Convert.ToInt32(row["MetricId"]) != 0
                                                       && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt16(row["IsPriority"]) == 0
                                                       orderby row["IsPriority"] descending
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["MetricId"]),
                                                           ParentId = Convert.ToInt32(row["ChannelId"]),
                                                           searchName = Convert.ToString(row["Metric"]),
                                                           Name = Convert.ToString(row["Metric"]),
                                                           ParentName = Convert.ToString(row["Channel"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                           IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                       }).Distinct().ToList());
                        }
                    }
                }
                filters.Add(filter);
            }
            return filters;
        }
        #region Load Priority Retailers
        List<Filter> LoadPriorityChannelRetailer(int tableNo)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = "Priority Retailers";
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where Convert.ToInt32(row["LevelDispId"]) != 0 && Convert.ToInt32(row["LevelDispId"]) != 1
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    filter.Levels[0].LevelItems.Add((from row in ds.Tables[tableNo].AsEnumerable()
                                                     where Convert.ToString(row["Channel"]).Equals(channel, StringComparison.OrdinalIgnoreCase)
                                                     && Convert.ToString(row["Metric"]).Equals(channel, StringComparison.OrdinalIgnoreCase)
                                                     select new LevelItems()
                                                     {
                                                         Id = Convert.ToInt32(row["ChannelId"]),
                                                         ParentId = Convert.ToInt32(row["ChannelId"]),
                                                         Name = Convert.ToString(row["Channel"]),
                                                         ParentName = Convert.ToString(row["Channel"]),
                                                         UniqueId = (Convert.ToString(row["Channel"]).Equals("Total", StringComparison.OrdinalIgnoreCase) ? 1 : Convert.ToInt32(row["UniqueFilterId"])),
                                                         IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                         IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                         LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                         searchName = Convert.ToString(row["Channel"]),
                                                         IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                         HasSubLevel = true,
                                                         IsShowImage = true,
                                                         ShowAll = true
                                                     }).FirstOrDefault());
                }
                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id != 1)
                        {
                            //add priority header item    
                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 2)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                var priorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                        where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                        && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt32(row["MetricId"]) != 0
                                                        && Convert.ToInt16(row["IsPriority"]) == 1
                                                        select row).FirstOrDefault();

                                if (priorityRetailer != null && Convert.ToBoolean(priorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(priorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(priorityRetailer["ChannelId"]),
                                        Name = "PRIORITY",
                                        ParentName = Convert.ToString(priorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(priorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                            }
                            level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                       where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                       && Convert.ToInt32(row["MetricId"]) != 0
                                                       && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt16(row["IsPriority"]) == 1
                                                       orderby row["IsPriority"] descending
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["MetricId"]),
                                                           ParentId = Convert.ToInt32(row["ChannelId"]),
                                                           Name = Convert.ToString(row["Metric"]),
                                                           searchName = Convert.ToString(row["Metric"]),
                                                           ParentName = Convert.ToString(row["Channel"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                           IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                       }).Distinct().ToList());

                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 2)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                //add non priority header item      
                                var nonpriorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                           where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                           && channel == Convert.ToString(row["Channel"])
                                                           && Convert.ToInt32(row["MetricId"]) != 0
                                                           && Convert.ToInt16(row["IsPriority"]) == 0
                                                           select row).FirstOrDefault();

                                if (nonpriorityRetailer != null && !Convert.ToBoolean(nonpriorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(nonpriorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(nonpriorityRetailer["ChannelId"]),
                                        Name = "RETAILERS",
                                        ParentName = Convert.ToString(nonpriorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(nonpriorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                            }
                            level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                       where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                       && Convert.ToInt32(row["MetricId"]) != 0
                                                       && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt16(row["IsPriority"]) == 0
                                                       orderby row["IsPriority"] descending
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["MetricId"]),
                                                           ParentId = Convert.ToInt32(row["ChannelId"]),
                                                           searchName = Convert.ToString(row["Metric"]),
                                                           Name = Convert.ToString(row["Metric"]),
                                                           ParentName = Convert.ToString(row["Channel"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                           IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                       }).Distinct().ToList());
                        }
                    }
                }
                filters.Add(filter);
            }
            return filters;
        }
        #endregion
        List<Filter> LoadBGMChannelRetailer(int tableNo)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = "Retailers";
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where Convert.ToInt32(row["LevelDispId"]) != 0 && Convert.ToInt32(row["LevelDispId"]) != 1
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable()
                                            where !Convert.ToString(row["Channel"]).Equals("Channel Nets", StringComparison.OrdinalIgnoreCase)
                                            select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    filter.Levels[0].LevelItems.Add((from row in ds.Tables[tableNo].AsEnumerable()
                                                     where Convert.ToString(row["Channel"]).Equals(channel, StringComparison.OrdinalIgnoreCase)
                                                     && Convert.ToString(row["Metric"]).Equals(channel, StringComparison.OrdinalIgnoreCase)
                                                     select new LevelItems()
                                                     {
                                                         Id = Convert.ToInt32(row["ChannelId"]),
                                                         ParentId = Convert.ToInt32(row["ChannelId"]),
                                                         Name = Convert.ToString(row["Channel"]),
                                                         ParentName = Convert.ToString(row["Channel"]),
                                                         UniqueId = (Convert.ToString(row["Channel"]).Equals("Total", StringComparison.OrdinalIgnoreCase) ? 1 : Convert.ToInt32(row["UniqueFilterId"])),
                                                         IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                         IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                         LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                         searchName = Convert.ToString(row["Channel"]),
                                                         IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                         HasSubLevel = true,
                                                         IsShowImage = true,
                                                         ShowAll = true
                                                     }).FirstOrDefault());
                }
                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable()
                                            where !Convert.ToString(row["Channel"]).Equals("Channel Nets", StringComparison.OrdinalIgnoreCase)
                                            select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id != 1)
                        {
                            //add priority header item    
                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 2)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                var priorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                        where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                        && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt32(row["MetricId"]) != 0
                                                        && Convert.ToInt16(row["IsPriority"]) == 1
                                                        select row).FirstOrDefault();

                                if (priorityRetailer != null && Convert.ToBoolean(priorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(priorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(priorityRetailer["ChannelId"]),
                                        Name = "PRIORITY",
                                        ParentName = Convert.ToString(priorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(priorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                            }
                            level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                       where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                       && Convert.ToInt32(row["MetricId"]) != 0
                                                       && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt16(row["IsPriority"]) == 1
                                                       orderby row["IsPriority"] descending
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["MetricId"]),
                                                           ParentId = Convert.ToInt32(row["ChannelId"]),
                                                           Name = Convert.ToString(row["Metric"]),
                                                           searchName = Convert.ToString(row["Metric"]),
                                                           ParentName = Convert.ToString(row["Channel"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                           IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                       }).Distinct().ToList());

                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 2)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                //add non priority header item      
                                var nonpriorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                           where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                           && channel == Convert.ToString(row["Channel"])
                                                           && Convert.ToInt32(row["MetricId"]) != 0
                                                           && Convert.ToInt16(row["IsPriority"]) == 0
                                                           select row).FirstOrDefault();

                                if (nonpriorityRetailer != null && !Convert.ToBoolean(nonpriorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(nonpriorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(nonpriorityRetailer["ChannelId"]),
                                        Name = "RETAILERS",
                                        ParentName = Convert.ToString(nonpriorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(nonpriorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                            }
                            level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                       where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                       && Convert.ToInt32(row["MetricId"]) != 0
                                                       && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt16(row["IsPriority"]) == 0
                                                       orderby row["IsPriority"] descending
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["MetricId"]),
                                                           ParentId = Convert.ToInt32(row["ChannelId"]),
                                                           searchName = Convert.ToString(row["Metric"]),
                                                           Name = Convert.ToString(row["Metric"]),
                                                           ParentName = Convert.ToString(row["Channel"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                           IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                       }).Distinct().ToList());
                        }
                    }
                }
                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadGroup(int tableNo, string groupName, bool IsAddGeography = true)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();


                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }
                //add prime filter type
                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                   ParentId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                   Name = Convert.ToString(row["PrimeFilterType"]),
                                                   PrimeFilterType= Convert.ToString(row["PrimeFilterType"]),
                                                   FilterType = Convert.ToString(row["DispFilterType"]),
                                                   IsSelectable = false,
                                                   HasSubLevel = true,
                                                   IsShowInSearch = false
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                //add metric
                filter.Levels[1].LevelItems = new List<LevelItems>();
                foreach (LevelItems item in filter.Levels[0].LevelItems)
                {
                    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                          where Convert.ToInt32(row["LevelId"]) == 1
                                                          && !Convert.ToString(row["FilterType"]).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                                           && item.Name == Convert.ToString(row["PrimeFilterType"])
                                                          select new LevelItems()
                                                          {
                                                              Id = Convert.ToInt32(row["DispMetricId"]),
                                                              ParentId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                              ParentName = Convert.ToString(row["PrimeFilterType"]),
                                                              Name = Convert.ToString(row["Metric"]).Trim(),
                                                              searchName = Convert.ToString(row["Metric"]),
                                                              FilterType = Convert.ToString(row["FilterType"]),
                                                              FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                              UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                              PrimeFilterType = Convert.ToString(row["PrimeFilterType"]),
                                                              PrimeFilterTypeId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                              IsSelectable = HasSubLevel(Convert.ToString(row["Metric"])) ? false : true,
                                                              IsShowInSearch = HasSubLevel(Convert.ToString(row["Metric"])) ? false : true,
                                                              HasSubLevel = HasSubLevel(Convert.ToString(row["Metric"]))
                                                          }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());
                }
                //add Geography filter
                if (IsAddGeography)
                {
                    filter.Levels[1].LevelItems.Add(new LevelItems()
                    {
                        Id = 100,
                        ParentId = 1,
                        Name = "GEOGRAPHY",
                        ParentName = "Demographics",
                        FilterType = "Demographics",
                        PrimeFilterType = "Demographics",
                        IsSelectable = false,                        
                        HasSubLevel = true                       
                    });
                }                
                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                            && !Convert.ToString(row["FilterType"]).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["DispMetricId"]),
                                                ParentId = Convert.ToInt32(row["DispMetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToString(row["MetricItem"]).Trim(),
                                                searchName = Convert.ToString(row["searchName"]).Trim(),
                                                FilterType = Convert.ToString(row["FilterType"]),
                                                FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                PrimeFilterType = Convert.ToString(row["PrimeFilterType"]),
                                                PrimeFilterTypeId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader = row.Table.Columns.Contains("IsHeader") ? Convert.ToBoolean(row["IsHeader"]) : false
                                            }).ToList());
                    }

                }
                filters.Add(filter);
            }
            return filters;
        }

        List<Filter> LoadGroup(int tableNo, string groupName, List<string> groupTypes, bool IsAddFrequency = false, bool IsAddGeography = true, bool IsAddCrossRetailerShopper = false, bool IsAddOnlineFrequency=false)
        {
            //IsAddCrossRetailerShopper = false;
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();


                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where groupTypes.Contains(Convert.ToString(row["PrimeFilterType"]), StringComparer.OrdinalIgnoreCase)
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                //add prime filter type
                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               && groupTypes.Contains(Convert.ToString(row["PrimeFilterType"]), StringComparer.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                   ParentId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                   Name = Convert.ToString(row["PrimeFilterType"]),
                                                   searchName = Convert.ToString(row["PrimeFilterType"]),
                                                   PrimeFilterType= Convert.ToString(row["PrimeFilterType"]),
                                                   FilterType = Convert.ToString(row["DispFilterType"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                //add metric
                filter.Levels[1].LevelItems = new List<LevelItems>();
                foreach (LevelItems item in filter.Levels[0].LevelItems)
                {
                    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                          where Convert.ToInt32(row["LevelId"]) == 1
                                                          && !Convert.ToString(row["FilterType"]).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                                           && item.Name == Convert.ToString(row["PrimeFilterType"])
                                                           && groupTypes.Contains(Convert.ToString(row["PrimeFilterType"]), StringComparer.OrdinalIgnoreCase)
                                                          select new LevelItems()
                                                          {
                                                              Id = Convert.ToInt32(row["DispMetricId"]),
                                                              ParentId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                              ParentName = Convert.ToString(row["PrimeFilterType"]),
                                                              Name = Convert.ToString(row["Metric"]).Trim(),
                                                              searchName = Convert.ToString(row["Metric"]).Trim(),
                                                              FilterType = Convert.ToString(row["FilterType"]),
                                                              FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                              PrimeFilterType = Convert.ToString(row["PrimeFilterType"]),
                                                              PrimeFilterTypeId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                              IsSelectable = HasSubLevel(Convert.ToString(row["Metric"])) ? false : true,
                                                              IsShowInSearch = HasSubLevel(Convert.ToString(row["Metric"])) ? false : true,
                                                              HasSubLevel = HasSubLevel(Convert.ToString(row["Metric"]))
                                                          }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());
                }
                //add Geography filter
                if (IsAddGeography)
                {
                    filter.Levels[1].LevelItems.Add(new LevelItems()
                    {
                        Id = 100,
                        ParentId = 1,
                        Name = "GEOGRAPHY",
                        ParentName = "Demographics",
                        FilterType = "Demographics",
                        PrimeFilterType = "Demographics",
                        IsSelectable = false,
                        HasSubLevel = true,
                    });
                }
                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                            && groupTypes.Contains(Convert.ToString(row["PrimeFilterType"]), StringComparer.OrdinalIgnoreCase)
                                            && !Convert.ToString(row["FilterType"]).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["DispMetricId"]),
                                                ParentId = Convert.ToInt32(row["DispMetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToString(row["MetricItem"]).Trim(),
                                                searchName = Convert.ToString(row["searchName"]).Trim(),
                                                FilterType = Convert.ToString(row["FilterType"]),
                                                FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                PrimeFilterType = Convert.ToString(row["PrimeFilterType"]),
                                                PrimeFilterTypeId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader = row.Table.Columns.Contains("IsHeader") ?Convert.ToBoolean(row["IsHeader"]) : false
                                            }).ToList();
                    }

                }
                if (IsAddFrequency && !IsAddCrossRetailerShopper)
                    LoadGroupFrequency(3, new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA", "TOTAL VISITS" }, filter, "FREQUENCY", 0, -1000);
                if (IsAddFrequency && IsAddCrossRetailerShopper)
                    LoadComptitrFrequencyInFilters(34, new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA", "TOTAL VISITS" }, filter, "FREQUENCY", 0, -1000, IsAddOnlineFrequency);

                //if (IsAddCrossRetailerShopper)
                //{
                //    filter.Levels[0].LevelItems.Add(new LevelItems()
                //    {
                //        Id = 101,
                //        ParentId = 101,
                //        Name = "Cross-Retailer Shopper",
                //        FilterType = "Cross-Retailer Shopper",
                //        IsSelectable = false,
                //        HasSubLevel = true,
                //    });
                //}

                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadCorrespondenceMeasure(int tableNo, string groupName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();


                filter.Levels.Add(new Level() { Id = 1 });
                //filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                //add prime filter type
                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["MetricId"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   MeasureType = Convert.ToString(row["Metric"]),
                                                   FilterType = Convert.ToString(row["DispFilterType"]),
                                                   FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                   SelID = Convert.ToInt32(row["SelID"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).GroupBy(l => new { l.Name, l.SelID }).Select(g => g.FirstOrDefault()).ToList();

                //add metric
                //filter.Levels[1].LevelItems = new List<LevelItems>();
                //foreach (LevelItems item in filter.Levels[0].LevelItems)
                //{
                //    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                //                                          where Convert.ToInt32(row["SelID"]) == 1
                //                                           && item.Name == Convert.ToString(row["FilterType"])                                       
                //                                          select new LevelItems()
                //                                          {
                //                                              Id = Convert.ToInt32(row["DispMetricId"]),
                //                                              ParentId = Convert.ToInt32(row["DispMetricId"]),
                //                                              ParentName = Convert.ToString(row["FilterType"]),
                //                                              Name = Convert.ToString(row["Metric"]),
                //                                              FilterType = Convert.ToString(row["FilterType"]),
                //                                              FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                //                                              IsSelectable = false,
                //                                              HasSubLevel = HasSubLevel(Convert.ToString(row["Metric"]))
                //                                          }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());
                //}
                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1)
                    {
                        level.LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["MetricItemId"]),
                                                ParentId = Convert.ToInt32(row["MetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToString(row["MetricItem"]),
                                                MeasureType = Convert.ToString(row["Metric"]),
                                                searchName = Convert.ToString(row["MetricItem"]),
                                                FilterType = Convert.ToString(row["FilterType"]),
                                                FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = true,
                                                IsShowInSearch = true,
                                                HasSubLevel = false
                                                //IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                //HasSubLevel = Convert.ToBoolean(row["HasSubLevel"])
                                            }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();
                    }

                }
                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadMeasure(int tableNo, string groupName, List<string> meaureTypes)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();


                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                //add prime filter type
                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               && meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["DispFiltertypeid"]),
                                                   ParentId = Convert.ToInt32(row["DispFiltertypeid"]),
                                                   Name = Convert.ToString(row["FilterType"]),
                                                   MeasureType = Convert.ToString(row["FilterType"]),
                                                   searchName = Convert.ToString(row["FilterType"]),
                                                   FilterType = Convert.ToString(row["DispFilterType"]),
                                                   DispFilterType = Convert.ToString(row["DispFilterType"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                //add metric
                filter.Levels[1].LevelItems = new List<LevelItems>();
                foreach (LevelItems item in filter.Levels[0].LevelItems)
                {
                    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                          where Convert.ToInt32(row["LevelId"]) == 1
                                                          && !Convert.ToString(row["FilterType"]).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                                           && item.Name == Convert.ToString(row["FilterType"])
                                                           && meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                                          select new LevelItems()
                                                          {
                                                              Id = Convert.ToInt32(row["DispMetricid"]),
                                                              ParentId = Convert.ToInt32(row["DispFiltertypeid"]),
                                                              MetricSortId = (string.IsNullOrEmpty(Convert.ToString(row["MetricSortId"])) ? 0 : Convert.ToInt32(row["MetricSortId"])),
                                                              ParentName = Convert.ToString(row["FilterType"]),
                                                              Name = Convert.ToString(row["Metric"]),
                                                              MeasureType = Convert.ToString(row["FilterType"]),
                                                              searchName = Convert.ToString(row["Metric"]),
                                                              FilterType = Convert.ToString(row["FilterType"]),
                                                              FilterTypeId = Convert.ToInt32(row["DispFiltertypeid"]),
                                                              ChartTypePIT = Convert.ToString(row["ChartTypePIT"]),
                                                              ChartTypeTrend = Convert.ToString(row["ChartTypeTrend"]),
                                                              DispFilterType = Convert.ToString(row["DispFilterType"]),
                                                              IsSelectable = false,
                                                              IsShowInSearch = false,
                                                              HasSubLevel = HasSubLevel(Convert.ToString(row["Metric"])),
                                                              IsHeader= Convert.ToBoolean(row["IsHeader"])
                                                          }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());
                }
                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                            && meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["DispMetricid"]), 
                                                ParentId = Convert.ToInt32(row["DispMetricid"]),
                                                MetricSortId = (string.IsNullOrEmpty(Convert.ToString(row["MetricSortId"])) ? 0 : Convert.ToInt32(row["MetricSortId"])),
                                                HasMetricSortId = false,
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToString(row["MetricItem"]),
                                                MeasureType = Convert.ToString(row["FilterType"]),
                                                searchName = Convert.ToString(row["MetricItem"]),
                                                FilterType = Convert.ToString(row["FilterType"]),
                                                FilterTypeId = Convert.ToInt32(row["DispFiltertypeid"]),
                                                ChartTypePIT = Convert.ToString(row["ChartTypePIT"]),
                                                ChartTypeTrend = Convert.ToString(row["ChartTypeTrend"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                DispFilterType = Convert.ToString(row["DispFilterType"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader = Convert.ToBoolean(row["IsHeader"])
                                            }).ToList();
                    }

                }
                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadDemographic(int tableNo, string filterType, bool IsAddGeography, bool IsAddCrossRetailerShopper = false)
        {
            IsAddCrossRetailerShopper = false;
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = filterType;
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where Convert.ToInt32(row["LevelDispId"]) != 0
                                        && Convert.ToString(row["FilterType"]).Equals(filterType, StringComparison.OrdinalIgnoreCase)
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToString(row["FilterType"]).Equals(filterType, StringComparison.OrdinalIgnoreCase)
                                               && Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["MetricId"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch=false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList();
                if (filterType == "Demographic" && IsAddCrossRetailerShopper)
                {
                    filter.Levels[0].LevelItems.Add(new LevelItems()
                    {
                        Id = 101,
                        ParentId = 101,
                        Name = "Cross-Retailer Shopper",
                        FilterType = "Cross-Retailer Shopper",
                        IsSelectable = false,
                        HasSubLevel = true,
                    });
                }

                if (IsAddGeography)
                {
                    filter.Levels[0].LevelItems.Add(new LevelItems()
                    {
                        Id = 100,
                        ParentId = 100,
                        Name = "GEOGRAPHY",
                        FilterType = "GEOGRAPHY",
                        IsSelectable = false,
                        HasSubLevel = true,
                    });
                }

                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1)
                    {
                        level.LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                             && Convert.ToString(row["FilterType"]).Equals(filterType, StringComparison.OrdinalIgnoreCase)
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["MetricId"]),
                                                ParentId = Convert.ToInt32(row["MetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToString(row["MetricItem"]),
                                                searchName = Convert.ToString(row["MetricItem"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader= Convert.ToBoolean(row["IsHeader"])
                                            }).ToList();
                    }

                }
                filters.Add(filter);
            }
            return filters;
        }

        List<Filter> LoadPathToPurchaseFilters(int tableNo, string filterType, bool IsAddGeography, bool IsAddFrequency, bool IsAddCrossRetailerShopper = false)
        {
            //IsAddCrossRetailerShopper = false;
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = filterType;
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where Convert.ToInt32(row["LevelDispId"]) != 0
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"]) + 1
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["FilterTypeId"]),
                                                   ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                   Name = GetShortNames(Convert.ToString(row["FilterType"])),
                                                   ParentName = GetShortNames(Convert.ToString(row["FilterType"])),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true,
                                                   
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                filter.Levels[1].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   ParentName = GetShortNames(Convert.ToString(row["FilterType"])),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                //add Geography filter
                if (IsAddGeography)
                {
                    filter.Levels[1].LevelItems.Add(new LevelItems()
                    {
                        Id = 100,
                        ParentId = 1,
                        Name = "GEOGRAPHY",
                        ParentName = "DEMOGRAPHIC FILTERS",
                        FilterType = "Demographics",
                        PrimeFilterType = "Demographics",//"DEMOGRAPHIC FILTERS",
                        IsSelectable = false,
                        HasSubLevel = true,
                    });
                }

                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where (level.Id - 1) == Convert.ToInt32(row["LevelDispId"])
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["MetricId"]),
                                                ParentId = Convert.ToInt32(row["MetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToBoolean(row["IsHeader"]) == true ? Convert.ToString(row["MetricItem"]) : Convert.ToString(row["searchName"]),//Convert.ToString(row["MetricItem"])
                                                searchName = Convert.ToString(row["searchName"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader = Convert.ToBoolean(row["IsHeader"]),
                                                IsTripAddnFilter = (row["isAdditionalFilter"] == DBNull.Value ? false : Convert.ToBoolean(row["isAdditionalFilter"]))
                                            }).ToList();
                    }

                }

                if (IsAddFrequency && !IsAddCrossRetailerShopper)
                    LoadGroupFrequency(3, new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "TOTAL VISITS" }, filter, "ADDITIONAL FILTERS", 1,3);
                if (IsAddFrequency && IsAddCrossRetailerShopper)
                    LoadComptitrFrequencyInFilters(34, new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "TOTAL VISITS" }, filter, "ADDITIONAL FILTERS", 1, 3);

                //if (IsAddCrossRetailerShopper)
                //{
                //    filter.Levels[0].LevelItems.Add(new LevelItems()
                //    {
                //        Id = 101,
                //        ParentId = 101,
                //        Name = "Cross-Retailer Shopper",
                //        FilterType = "Cross-Retailer Shopper",
                //        IsSelectable = false,
                //        HasSubLevel = true,
                //    });
                //}

                filters.Add(filter);
            }
            return filters;
        }

        List<Filter> LoadPathToPurchaseFiltersSar(int tableNo, string filterType, bool IsAddGeography, bool IsAddFrequency, bool IsAddCrossRetailerShopper = false)
        {
            //IsAddCrossRetailerShopper = false;
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = filterType;
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where Convert.ToInt32(row["LevelDispId"]) != 0
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"]) + 1
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());
                if (IsAddGeography)
                {
                    List<int> geographyLevel = (from row in ds.Tables[tableNo].AsEnumerable()
                     where Convert.ToInt32(row["LevelDispId"]) != 0 && Convert.ToInt32(row["FilterTypeId"]) == 2
                     orderby row["LevelDispId"]
                     select Convert.ToInt32(row["LevelDispId"]) + 2
                                 ).Distinct().ToList();
                    foreach(var item in geographyLevel)
                    {
                        if (filter.Levels.Where(l => l.Id == item).Count() == 0)
                        {
                            filter.Levels.Add(new Level() { Id = item });
                        }
                    }
                }

                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1 && Convert.ToInt32(row["FilterTypeId"])!=2
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["FilterTypeId"]),
                                                   ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                   Name = GetShortNames(Convert.ToString(row["FilterType"])),
                                                   ParentName = GetShortNames(Convert.ToString(row["FilterType"])),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                filter.Levels[1].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   ParentName = GetShortNames(Convert.ToString(row["FilterType"])),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                //add Geography filter
                if (IsAddGeography)
                {
                    filter.Levels[1].LevelItems.Add(new LevelItems()
                    {
                        Id = 100,
                        ParentId = 1,
                        Name = "GEOGRAPHY",
                        ParentName = "DEMOGRAPHIC FILTERS",
                        FilterType = "Demographics",
                        PrimeFilterType = "Demographics",//"DEMOGRAPHIC FILTERS",
                        IsSelectable = false,
                        HasSubLevel = true,
                    });
                }

                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where (level.Id - 1) == Convert.ToInt32(row["LevelDispId"])
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["MetricId"]),
                                                ParentId = Convert.ToInt32(row["MetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToBoolean(row["IsHeader"]) == true ? Convert.ToString(row["MetricItem"]) : Convert.ToString(row["searchName"]),//Convert.ToString(row["MetricItem"])
                                                searchName = Convert.ToString(row["searchName"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader = Convert.ToBoolean(row["IsHeader"])
                                            }).ToList();
                    }

                }

                if (IsAddFrequency && !IsAddCrossRetailerShopper)
                    LoadGroupFrequency(3, new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "TOTAL VISITS" }, filter, "ADDITIONAL FILTERS", 1, 3);
                if (IsAddFrequency && IsAddCrossRetailerShopper)
                    LoadComptitrFrequencyInFilters(34, new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "TOTAL VISITS" }, filter, "ADDITIONAL FILTERS", 1, 3);

                //if (IsAddCrossRetailerShopper)
                //{
                //    filter.Levels[0].LevelItems.Add(new LevelItems()
                //    {
                //        Id = 101,
                //        ParentId = 101,
                //        Name = "Cross-Retailer Shopper",
                //        FilterType = "Cross-Retailer Shopper",
                //        IsSelectable = false,
                //        HasSubLevel = true,
                //    });
                //}

                filters.Add(filter);
            }
            return filters;
        }
        string GetShortNames(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            switch (name.Trim().ToUpper())
            {
                case "DEMOGRAPHIC":
                    {
                        name = "DEMOGRAPHIC FILTERS";
                        break;
                    }
                case "VISITS":
                    {
                        name = "ADDITIONAL FILTERS";
                        break;
                    }
            }
            return name;
        }
        List<Filter> LoadBeverages(int tableNo)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = "Beverages";
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelDisplayId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDisplayId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                foreach (string metric in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Metric"])).Distinct().ToList())
                {
                    var bevCategory = (from row in ds.Tables[tableNo].AsEnumerable()
                                       where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                       && Convert.ToString(row["MetricItem"]).Equals(metric,StringComparison.OrdinalIgnoreCase)
                                       select new LevelItems()
                                       {
                                           Id = Convert.ToInt32(row["DispMetricId"]),
                                           ParentId = Convert.ToInt32(row["DispMetricId"]),
                                           Name = Convert.ToString(row["Metric"]),
                                           searchName = Convert.ToString(row["Metric"]),
                                           ParentName = Convert.ToString(row["Metric"]),
                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                           IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                           HasSubLevel = true,
                                           IsShowImage = true,
                                           ShowAll = true,
                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                       }).FirstOrDefault();
                    if (bevCategory != null)
                    {
                        filter.Levels[0].LevelItems.Add(bevCategory);
                    }
                    else
                    {
                        filter.Levels[0].LevelItems.Add((from row in ds.Tables[tableNo].AsEnumerable()
                                                         where Convert.ToString(row["Metric"]).Equals(metric)
                                                         select new LevelItems()
                                                         {
                                                             Id = Convert.ToInt32(row["DispMetricId"]),
                                                             ParentId = Convert.ToInt32(row["DispMetricId"]),
                                                             Name = Convert.ToString(row["Metric"]),
                                                             searchName = Convert.ToString(row["Metric"]),
                                                             ParentName = Convert.ToString(row["Metric"]),
                                                             UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                             IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                             LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                             HasSubLevel = true,
                                                             IsShowImage = true,
                                                             ShowAll = true,
                                                             IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                         }).FirstOrDefault());
                    }
                }
                foreach (string metric in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Metric"])).Distinct().ToList())
                {
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id != 1)
                        {
                            level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                       where level.Id == Convert.ToInt32(row["LevelDisplayId"])
                                                       && Convert.ToInt32(row["DispMetricId"]) != 0
                                                       && metric == Convert.ToString(row["Metric"])
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["DispMetricId"]),
                                                           ParentId = Convert.ToInt32(row["DispMetricId"]),                                                          
                                                           Name = Convert.ToString(row["MetricItem"]),
                                                           searchName = Convert.ToString(row["MetricItem"]),
                                                           ParentName = Convert.ToString(row["Metric"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = true,
                                                           LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                           IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                       }).Distinct().ToList());
                        }
                    }
                }
                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadBGMBeveragAndNonBeverageItems()
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = "BGM Beverag And NonBeverage Items";
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[18].AsEnumerable()
                                        orderby row["LevelDisplayId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDisplayId"]) + 1
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                filter.Levels[0].LevelItems = (from row in ds.Tables[18].AsEnumerable()
                                               where !Convert.ToString(row["DispFilterType"]).Equals("Beverage Item", StringComparison.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["FilterTypeId"]),
                                                   ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                   Name = Convert.ToString(row["DispFilterType"]),
                                                   searchName = Convert.ToString(row["DispFilterType"]),
                                                   ParentName = Convert.ToString(row["DispFilterType"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                filter.Levels[1].LevelItems = new List<LevelItems>();

                filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[18].AsEnumerable()
                                                      where Convert.ToString(row["FilterType"]).Equals("Beverage Item", StringComparison.OrdinalIgnoreCase)
                                                      && Convert.ToString(row["Metric"]).Equals(Convert.ToString(row["MetricItem"]), StringComparison.OrdinalIgnoreCase)
                                                      select new LevelItems()
                                                      {
                                                          Id = Convert.ToInt32(row["DispMetricId"]),
                                                          ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                          Name = Convert.ToString(row["Metric"]),
                                                          searchName = Convert.ToString(row["Metric"]),
                                                          ParentName = Convert.ToString(row["DispFilterType"]),
                                                          IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                          IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                          UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                          LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                          HasSubLevel = true,
                                                          IsShowImage = true,
                                                          ShowAll = true,
                                                      }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());

                filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[18].AsEnumerable()
                                                      where Convert.ToString(row["FilterType"]).Equals("Non Beverage Item", StringComparison.OrdinalIgnoreCase)
                                                      select new LevelItems()
                                                      {
                                                          Id = Convert.ToInt32(row["DispMetricId"]),
                                                          ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                          Name = GetShortNames(Convert.ToString(row["MetricItem"])),
                                                          searchName = GetShortNames(Convert.ToString(row["MetricItem"])),
                                                          ParentName = Convert.ToString(row["DispFilterType"]),
                                                          UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                          LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                          IsSelectable = true,
                                                          IsShowInSearch = true,
                                                          HasSubLevel = false
                                                      }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());

                foreach (string metric in (from row in ds.Tables[18].AsEnumerable() select Convert.ToString(row["Metric"])).Distinct().ToList())
                {
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id != 1 && level.Id != 2)
                        {
                            level.LevelItems.AddRange((from row in ds.Tables[18].AsEnumerable()
                                                       where (level.Id) == (Convert.ToInt32(row["LevelDisplayId"]))
                                                       && Convert.ToInt32(row["DispMetricId"]) != 0
                                                       && metric == Convert.ToString(row["Metric"])
                                                       select new LevelItems()
                                                       {
                                                           Id = Convert.ToInt32(row["DispMetricId"]),
                                                           ParentId = Convert.ToInt32(row["DispMetricId"]),
                                                           Name = Convert.ToString(row["MetricItem"]),
                                                           searchName = Convert.ToString(row["MetricItem"]),
                                                           ParentName = Convert.ToString(row["Metric"]),
                                                           UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                           IsSelectable = true,
                                                           IsShowInSearch = true,
                                                           LevelDesc = Convert.ToString(row["LevelDesc"])
                                                       }).Distinct().ToList());
                        }
                    }
                }
                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadBeverageWherePurchased(int tableNo , bool isCorpNet = false)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = "Beverage Where Purchased";
                filter.Levels = new List<Level>();
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelDisplayId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDisplayId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                filter.Levels[0].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                      where 1 == Convert.ToInt32(row["LevelDisplayId"])
                                                      select new LevelItems()
                                                      {
                                                          Id = Convert.ToInt32(row["MetricId"]),
                                                          ParentId = Convert.ToInt32(row["MetricId"]),
                                                          Name = Convert.ToString(row["Metric"]).ToUpper(),
                                                          searchName = Convert.ToString(row["Metric"]).ToUpper(),
                                                          ParentName = Convert.ToString(row["Metric"]).ToUpper(),
                                                          UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                          IsSelectable = Convert.ToString(row["Metric"]).Equals("Total", StringComparison.OrdinalIgnoreCase) ? true : false,
                                                          IsShowInSearch = Convert.ToString(row["Metric"]).Equals("Total", StringComparison.OrdinalIgnoreCase) ? true : false,
                                                          HasSubLevel = Convert.ToString(row["Metric"]).Equals("Total", StringComparison.OrdinalIgnoreCase) ? false : true,
                                                      }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());


                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1)
                    {
                        level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                   where level.Id == Convert.ToInt32(row["LevelDisplayId"])
                                                 && !string.IsNullOrEmpty(Convert.ToString(row["MetricItem"]))
                                                   select new LevelItems()
                                                   {
                                                       ParentId = Convert.ToInt32(row["MetricId"]),
                                                       Name = Convert.ToString(row["MetricItem"]).ToUpper(),
                                                       searchName = Convert.ToString(row["MetricItem"]),
                                                       ParentName = Convert.ToString(row["Metric"]).ToUpper(),
                                                       UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                       IsSelectable = true,
                                                       IsShowInSearch = true
                                                   }).Distinct().ToList());
                    }
                }
                //add retailer visited items
                filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                      where Convert.ToInt32(row["LevelDisplayId"]) == 3
                                                      select new LevelItems()
                                                      {
                                                          Id = Convert.ToInt32(row["MetricId"]),
                                                          ParentId = Convert.ToInt32(row["MetricId"]),
                                                          searchName = Convert.ToString(row["Metric"]),
                                                          Name = Convert.ToString(row["Metric"]).ToUpper(),
                                                          ParentName = "RETAILER VISITED",                                                         
                                                          IsSelectable = false,
                                                          IsShowInSearch = false,
                                                          HasSubLevel = true,
                                                      }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());
                if (!isCorpNet)
                {
                    filter.Levels[1].LevelItems.RemoveAll(r => r.Name == "Corporate Nets");
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id != 1 && level.Id != 2)
                        {
                            level.LevelItems.RemoveAll(r => r.ParentName == "Corporate Nets");
                        }
                    }
                   
                }
                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadTotalMeasure()
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = "Total Measure";
                filter.Levels = new List<Level>();

                DataTable tbl = ds.Tables[22].Copy();
                tbl.Merge(ds.Tables[21], true, MissingSchemaAction.Ignore);

                filter.Levels.Add(new Level() { Id = 1 });

                filter.Levels.AddRange((from row in tbl.AsEnumerable()
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                //add prime filter type
                filter.Levels[0].LevelItems = (from row in tbl.AsEnumerable()
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["MetricTypeId"]),
                                                   ParentId = Convert.ToInt32(row["MetricTypeId"]),
                                                   Name = Convert.ToString(row["MetricType"]),
                                                   searchName = Convert.ToString(row["MetricType"]),
                                                   FilterType = Convert.ToString(row["DispFilterType"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1)
                    {
                        level.LevelItems = (from row in tbl.AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["MetricId"]),
                                                ParentId = Convert.ToInt32(row["MetricTypeId"]),
                                                ParentName = Convert.ToString(row["MetricType"]),
                                                Name = Convert.ToString(row["Metric"]),
                                                searchName = Convert.ToString(row["Metric"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = true,
                                                IsShowInSearch = true,
                                                HasSubLevel = false
                                            }).ToList();
                    }

                }
                filters.Add(filter);
            }
            return filters;
        }

        List<Filter> LoadEstablishmentMeasure(int tableNo, string filterName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = filterName;
                filter.Levels = new List<Level>();
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }


                foreach (Level level in filter.Levels)
                {
                    level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                               where level.Id == Convert.ToInt32(row["LevelDispId"])
                                               select new LevelItems()
                                               {
                                                   ParentId = Convert.ToInt32(row["ParentId"]),
                                                   Id = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   searchName = Convert.ToString(row["Metric"]),
                                                   ParentName = Convert.ToString(row["Metric"]),
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                   IsShowInSearch = Convert.ToBoolean(row["IsShowSearch"]),
                                                   HasSubLevel = Convert.ToBoolean(row["hasSubLevel"]),                                                   
                                               }).Distinct().ToList());
                }

                filters.Add(filter);
            }
            return filters;
        }

        List<Filter> LoadRetailerOrCompetitorSar(int tableNo, string groupName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                bool isChannelColumnExist = ds.Tables[tableNo].Columns.Contains("IsChannel") ? true : false;
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        where Convert.ToInt32(row["LevelDispId"]) != 1 && Convert.ToInt32(row["LevelDispId"]) != 2
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                foreach(int metricId in (from row in ds.Tables[tableNo].AsEnumerable() where Convert.ToInt32(row["LevelDispId"]) == 1 select Convert.ToInt32(row["MetricId"])).Distinct().ToList())
                {
                    filter.Levels[0].LevelItems.Add((from row in ds.Tables[tableNo].AsEnumerable()
                                                     where Convert.ToInt32(row["metricId"]) == (metricId)
                                                     select new LevelItems()
                                                     {
                                                         Id = Convert.ToInt32(row["MetricId"]),
                                                         ParentId = Convert.ToInt32(row["ChannelId"]),
                                                         Name = Convert.ToString(row["Metric"]),
                                                         ParentName = Convert.ToString(row["Channel"]),
                                                         UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                         IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                         IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                         LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                         searchName = Convert.ToString(row["Metric"]),
                                                         IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                         HasSubLevel = true,
                                                         IsShowImage = false,
                                                         ShowAll = true,
                                                         ParentOfParent = Convert.ToString(row["ParentOfParent"]),
                                                         IsChannel = isChannelColumnExist ? (row["IsChannel"] == DBNull.Value?false: Convert.ToBoolean(row["IsChannel"])) : false
                                                     }).FirstOrDefault());
                }

                //foreach (int channelId in (from row in ds.Tables[tableNo].AsEnumerable() where Convert.ToInt32(row["LevelDispId"])==1 select Convert.ToInt32(row["MetricId"])).Distinct().ToList())
                //{
                //    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                //                                     where Convert.ToInt32(row["ChannelId"]) == (channelId)
                //                                           && Convert.ToInt32(row["LevelDispId"])!=1
                //                                     select new LevelItems()
                //                                     {
                //                                         Id = Convert.ToInt32(row["MetricId"]),
                //                                         ParentId = Convert.ToInt32(row["ChannelId"]),
                //                                         Name = Convert.ToString(row["Metric"]),
                //                                         ParentName = Convert.ToString(row["Channel"]),
                //                                         UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                //                                         IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                //                                         IsPriority = Convert.ToBoolean(row["IsPriority"]),
                //                                         LevelDesc = Convert.ToString(row["LevelDesc"]),
                //                                         searchName = Convert.ToString(row["Metric"]),
                //                                         IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                //                                         HasSubLevel = true,
                //                                         IsShowImage = true,
                //                                         ShowAll = true,
                //                                         ParentOfParent = Convert.ToString(row["ParentOfParent"])
                //                                     }).Distinct().ToList());
                //}
                foreach (int channelId in (from row in ds.Tables[tableNo].AsEnumerable() where Convert.ToInt32(row["LevelDispId"]) == 1 select Convert.ToInt32(row["MetricId"])).Distinct().ToList())
                {
                    
                    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                          where Convert.ToInt32(row["ChannelId"]) == (channelId)
                                                                && Convert.ToInt32(row["LevelDispId"]) != 1
                                                                && (Convert.ToInt32(row["isTimePeriodCustomBase"]) != 1)
                                                          select new LevelItems()
                                                          {
                                                              Id = Convert.ToInt32(row["MetricId"]),
                                                              ParentId = Convert.ToInt32(row["ChannelId"]),
                                                              Name = Convert.ToString(row["Metric"]),
                                                              ParentName = Convert.ToString(row["Channel"]),
                                                              UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                              IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                              IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                              LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                              searchName = Convert.ToString(row["Metric"]),
                                                              IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                              HasSubLevel = row["ParentOfParent"].ToString().Trim()=="Custom" && row["Metric"].ToString().Trim().ToUpper() != "CHANNEL NETS" ? false:true,
                                                              IsShowImage = true,
                                                              ShowAll = true,
                                                              ParentOfParent = Convert.ToString(row["ParentOfParent"]),
                                                              IsChannel = isChannelColumnExist ? (row["IsChannel"] == DBNull.Value ? false : Convert.ToBoolean(row["IsChannel"])) : false
                                                          }).Distinct().ToList());

                    var statCustomBase = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where Convert.ToInt32(row["ChannelId"]) == (channelId)
                                                                && Convert.ToInt32(row["LevelDispId"]) != 1
                                                                && Convert.ToInt32(row["isTimePeriodCustomBase"]) == 1
                                            select row).FirstOrDefault();

                    if (statCustomBase != null && Convert.ToBoolean(statCustomBase["isTimePeriodCustomBase"]))
                    {
                        filter.Levels[1].LevelItems.Add(new LevelItems()
                        {
                            Id = Convert.ToInt32(statCustomBase["MetricId"]),
                            ParentId = Convert.ToInt32(statCustomBase["ChannelId"]),
                            Name = "Time Period Custom Base",
                            ParentName = Convert.ToString(statCustomBase["Channel"]),
                            IsSelectable = false,
                            ParentOfParent = Convert.ToString(statCustomBase["ParentOfParent"]),
                            IsHeader = true
                        });
                    }
                    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                          where Convert.ToInt32(row["ChannelId"]) == (channelId)
                                                                && Convert.ToInt32(row["LevelDispId"]) != 1
                                                                && Convert.ToInt32(row["isTimePeriodCustomBase"]) == 1
                                                          select new LevelItems()
                                                          {
                                                              Id = Convert.ToInt32(row["MetricId"]),
                                                              ParentId = Convert.ToInt32(row["ChannelId"]),
                                                              Name = Convert.ToString(row["Metric"]),
                                                              ParentName = Convert.ToString(row["Channel"]),
                                                              UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                              IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                              IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                              LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                              searchName = Convert.ToString(row["Metric"]),
                                                              IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                              ParentOfParent = Convert.ToString(row["ParentOfParent"]),
                                                              IsChannel = isChannelColumnExist ? (row["IsChannel"] == DBNull.Value ? false : Convert.ToBoolean(row["IsChannel"])) : false
                                                          }).Distinct().ToList());
                }
                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id == 3)
                        {
                            if (channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               || (channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase))
                               || channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               || channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                           where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                           && Convert.ToInt32(row["MetricId"]) != 0
                                                           && channel == Convert.ToString(row["Channel"])
                                                           select new LevelItems()
                                                           {
                                                               Id = Convert.ToInt32(row["MetricId"]),
                                                               ParentId = Convert.ToInt32(row["ChannelId"]),
                                                               Name = Convert.ToString(row["Metric"]),
                                                               searchName = Convert.ToString(row["Metric"]),
                                                               ParentName = Convert.ToString(row["Channel"]),
                                                               UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                               IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                               IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                               LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                               IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                               ParentOfParent = Convert.ToString(row["ParentOfParent"]),
                                                               IsChannel = isChannelColumnExist ? (row["IsChannel"] == DBNull.Value ? false : Convert.ToBoolean(row["IsChannel"])) : false
                                                           }).Distinct().ToList());
                            }
                                
                        }
                    }
                
                }
                foreach (string channel in (from row in ds.Tables[tableNo].AsEnumerable() select Convert.ToString(row["Channel"])).Distinct().ToList())
                {
                    foreach (Level level in filter.Levels)
                    {
                        if (level.Id != 1 && level.Id != 2)
                        {
                            //add priority header item    
                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 3)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                var priorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                        where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                        && channel == Convert.ToString(row["Channel"])
                                                        && Convert.ToInt32(row["MetricId"]) != 0
                                                        && Convert.ToInt16(row["IsPriority"]) == 1
                                                        select row).FirstOrDefault();

                                if (priorityRetailer != null && Convert.ToBoolean(priorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(priorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(priorityRetailer["ChannelId"]),
                                        Name = "PRIORITY",
                                        ParentName = Convert.ToString(priorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(priorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                                level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                           where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                           && Convert.ToInt32(row["MetricId"]) != 0
                                                           && channel == Convert.ToString(row["Channel"])
                                                            && Convert.ToInt16(row["IsPriority"]) == 1
                                                           orderby row["IsPriority"] descending
                                                           select new LevelItems()
                                                           {
                                                               Id = Convert.ToInt32(row["MetricId"]),
                                                               ParentId = Convert.ToInt32(row["ChannelId"]),
                                                               Name = Convert.ToString(row["Metric"]),
                                                               searchName = Convert.ToString(row["Metric"]),
                                                               ParentName = Convert.ToString(row["Channel"]),
                                                               UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                               IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                               IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                               LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                               IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                               ParentOfParent = Convert.ToString(row["ParentOfParent"]),
                                                               IsChannel = isChannelColumnExist ? (row["IsChannel"] == DBNull.Value ? false : Convert.ToBoolean(row["IsChannel"])) : false
                                                           }).Distinct().ToList());
                            }
                            

                            if (!channel.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)
                               && !(channel.Equals("SUPERMARKET/GROCERY", StringComparison.OrdinalIgnoreCase) && level.Id == 3)
                               && !channel.Equals("CORPORATE NETS", StringComparison.OrdinalIgnoreCase)
                               && !channel.Equals("CHANNEL NETS", StringComparison.OrdinalIgnoreCase))
                            {
                                //add non priority header item      
                                var nonpriorityRetailer = (from row in ds.Tables[tableNo].AsEnumerable()
                                                           where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                           && channel == Convert.ToString(row["Channel"])
                                                           && Convert.ToInt32(row["MetricId"]) != 0
                                                           && Convert.ToInt16(row["IsPriority"]) == 0
                                                           select row).FirstOrDefault();

                                if (nonpriorityRetailer != null && !Convert.ToBoolean(nonpriorityRetailer["IsPriority"]))
                                {
                                    level.LevelItems.Add(new LevelItems()
                                    {
                                        Id = Convert.ToInt32(nonpriorityRetailer["MetricId"]),
                                        ParentId = Convert.ToInt32(nonpriorityRetailer["ChannelId"]),
                                        Name = "RETAILERS",
                                        ParentName = Convert.ToString(nonpriorityRetailer["Channel"]),
                                        IsSelectable = false,
                                        IsPriority = Convert.ToBoolean(nonpriorityRetailer["IsPriority"]),
                                        IsHeader = true
                                    });
                                }
                                level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                           where level.Id == Convert.ToInt32(row["LevelDispId"])
                                                           && Convert.ToInt32(row["MetricId"]) != 0
                                                           && channel == Convert.ToString(row["Channel"])
                                                            && Convert.ToInt16(row["IsPriority"]) == 0
                                                           orderby row["IsPriority"] descending
                                                           select new LevelItems()
                                                           {
                                                               Id = Convert.ToInt32(row["MetricId"]),
                                                               ParentId = Convert.ToInt32(row["ChannelId"]),
                                                               searchName = Convert.ToString(row["Metric"]),
                                                               Name = Convert.ToString(row["Metric"]),
                                                               ParentName = Convert.ToString(row["Channel"]),
                                                               UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                               IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                               IsPriority = Convert.ToBoolean(row["IsPriority"]),
                                                               LevelDesc = Convert.ToString(row["LevelDesc"]),
                                                               IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                               ParentOfParent = Convert.ToString(row["ParentOfParent"]),
                                                               IsChannel = isChannelColumnExist ? (row["IsChannel"] == DBNull.Value ? false : Convert.ToBoolean(row["IsChannel"])) : false
                                                           }).Distinct().ToList());
                            }
                            
                        }
                    }
                }
                filters.Add(filter);
            }
            return filters;
        }

        string getSarFrequencyShortName(string name)
        {
            if (name.Contains("shopper"))
            {
                return "Core Shopper";
            }
            else if (name.Contains("purchase and trip"))
            {
                return "P2P";
            }
            else if (name.Contains("Strength"))
            {
                return "Strength";
            }
            else
            {
                return "Beverage";
            }
        }

        List<Filter> LoadFrequencySar(int tableNo, string groupName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }

                foreach (int metricId in (from row in ds.Tables[tableNo].AsEnumerable() where Convert.ToInt32(row["LevelDispId"]) == 1 select Convert.ToInt32(row["MetricId"])).Distinct().ToList())
                {
                    filter.Levels[0].LevelItems.Add((from row in ds.Tables[tableNo].AsEnumerable()
                                                     where Convert.ToInt32(row["metricId"]) == (metricId)
                                                     select new LevelItems()
                                                     {
                                                         Id = Convert.ToInt32(row["MetricId"]),
                                                         ParentId = Convert.ToInt32(row["MetricId"]),
                                                         Name = Convert.ToString(row["Metric"]),
                                                         ParentName = Convert.ToString(row["Metric"]),
                                                         UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                         IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                         searchName = Convert.ToString(row["Metric"]),
                                                         IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                         HasSubLevel = true,
                                                         IsShowImage = false,
                                                         ShowAll = true
                                                     }).FirstOrDefault());
                }

                foreach (int channelId in (from row in ds.Tables[tableNo].AsEnumerable() where Convert.ToInt32(row["LevelDispId"]) == 1 select Convert.ToInt32(row["MetricId"])).Distinct().ToList())
                {
                    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                     where row["ChannelId"]!= System.DBNull.Value
                                                     && Convert.ToInt32(row["ChannelId"]) == (channelId)
                                                           && Convert.ToInt32(row["LevelDispId"]) != 1
                                                          select new LevelItems()
                                                     {
                                                         Id = Convert.ToInt32(row["MetricId"]),
                                                         ParentId = Convert.ToInt32(row["ChannelId"]),
                                                         Name = Convert.ToString(row["Metric"]),
                                                         ParentName = Convert.ToString(row["Channel"]),
                                                         UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                         IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                         searchName = Convert.ToString(row["Metric"]) + "(" + getSarFrequencyShortName(Convert.ToString(row["Channel"])) + ")",
                                                         IsShowInSearch = Convert.ToBoolean(row["ShowSearchName"]),
                                                         HasSubLevel = false,
                                                         IsShowImage = false,
                                                         ShowAll = true,
                                                         FrequencyId = Convert.ToInt32(row["FrequencyID"])
                                                          }).Distinct().ToList());
                }
                
                
                filters.Add(filter);
            }
            return filters;
        }


        #region E-Com filters
        List<Filter> LoadEComSites(int tableNo, string filterName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = filterName;
                filter.Levels = new List<Level>();
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                foreach (Level level in filter.Levels)
                {
                    level.LevelItems = new List<LevelItems>();
                }


                foreach (Level level in filter.Levels)
                {
                    level.LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                               where level.Id == Convert.ToInt32(row["LevelDispId"])
                                               select new LevelItems()
                                               {
                                                   ParentId = 999,
                                                   Id = 999,
                                                   Name = Convert.ToString(row["Metric"]),
                                                   searchName = Convert.ToString(row["Metric"]),
                                                   ParentName = level.Id > 1 ? "Sites " : Convert.ToString(row["Metric"]),
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   IsSelectable = Convert.ToString(row["Metric"]).Equals("Sites ", StringComparison.OrdinalIgnoreCase) ? false : true,
                                                   IsShowInSearch = Convert.ToString(row["Metric"]).Equals("Sites ", StringComparison.OrdinalIgnoreCase) ? false : true,
                                                   HasSubLevel = Convert.ToString(row["Metric"]).Equals("Sites ", StringComparison.OrdinalIgnoreCase) ? true : false,
                                                   IsShowImage = level.Id == 1 ? true : false,
                                                   ShowAll = Convert.ToString(row["Metric"]).Equals("Sites ", StringComparison.OrdinalIgnoreCase) ? true : false,
                                               }).Distinct().ToList());
                }

                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadEComFrequency(int tableNo, string groupName, string filtertype)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();

                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToString(row["FilterType"]).Equals(filtertype, StringComparison.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   searchName = Convert.ToString(row["Metric"]),
                                                   ParentName = Convert.ToString(row["FilterType"]),
                                                   IsSelectable = true,
                                                   IsShowInSearch = true,
                                                   HasSubLevel = false
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadEComAdvancedFilters(int tableNo, string filterType)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = filterType;
                filter.Levels = new List<Level>();
                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });

                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["MetricId"]),
                                                   Name = Convert.ToString(row["Metric"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList();

                filter.Levels[1].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["MetricId"]),
                                                   ParentId = Convert.ToInt32(row["MetricId"]),
                                                   ParentName = Convert.ToString(row["Metric"]),
                                                   Name = Convert.ToString(row["MetricItem"]),
                                                   searchName = Convert.ToString(row["MetricItem"]),
                                                   UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                   IsSelectable = true,
                                                   IsShowInSearch = true,
                                                   HasSubLevel = false
                                               }).ToList();


                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadEComGroup(int tableNo, string groupName)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();


                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                //add prime filter type
                filter.Levels[0].LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                   ParentId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                   Name = Convert.ToString(row["PrimeFilterType"]),
                                                   searchName = Convert.ToString(row["PrimeFilterType"]),
                                                   FilterType = Convert.ToString(row["DispFilterType"]),
                                                   PrimeFilterType = Convert.ToString(row["PrimeFilterType"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList();

                //add metric
                filter.Levels[1].LevelItems = new List<LevelItems>();
                foreach (LevelItems item in filter.Levels[0].LevelItems)
                {
                    filter.Levels[1].LevelItems.AddRange((from row in ds.Tables[tableNo].AsEnumerable()
                                                          where Convert.ToInt32(row["LevelId"]) == 1
                                                          && !Convert.ToString(row["FilterType"]).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                                           && item.Name == Convert.ToString(row["PrimeFilterType"])
                                                          select new LevelItems()
                                                          {
                                                              Id = Convert.ToInt32(row["DispMetricId"]),
                                                              ParentId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                              ParentName = Convert.ToString(row["PrimeFilterType"]),
                                                              Name = Convert.ToString(row["Metric"]),
                                                              searchName = Convert.ToString(row["Metric"]),
                                                              FilterType = Convert.ToString(row["FilterType"]),
                                                              FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                              PrimeFilterType = Convert.ToString(row["PrimeFilterType"]),
                                                              PrimeFilterTypeId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                              IsSelectable = false,
                                                              IsShowInSearch = false,
                                                              HasSubLevel = HasSubLevel(Convert.ToString(row["Metric"]))
                                                          }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());
                }
                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems = (from row in ds.Tables[tableNo].AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["DispMetricId"]),
                                                ParentId = Convert.ToInt32(row["DispMetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToString(row["MetricItem"]),
                                                searchName = Convert.ToString(row["searchName"]),
                                                FilterType = Convert.ToString(row["FilterType"]),
                                                FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                PrimeFilterType = Convert.ToString(row["PrimeFilterType"]),
                                                PrimeFilterTypeId = Convert.ToInt32(row["PrimeFilterTypeId"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader = row.Table.Columns.Contains("IsHeader") ? Convert.ToBoolean(row["IsHeader"]) : false
                                            }).ToList();
                    }

                }
                filters.Add(filter);
            }
            return filters;
        }
        List<Filter> LoadEComMeasure(int tableNo, string groupName, List<string> meaureTypes)
        {
            List<Filter> filters = new List<Models.Filter>();
            Filter filter = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                filter = new Filter();
                filter.Name = groupName;
                filter.Levels = new List<Level>();
                DataTable tbl = ds.Tables[tableNo].Copy();
                tbl.Merge(ds.Tables[27]);

                filter.Levels.Add(new Level() { Id = 1 });
                filter.Levels.Add(new Level() { Id = 2 });
                filter.Levels.AddRange((from row in tbl.AsEnumerable()
                                        where meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                        orderby row["LevelDispId"]
                                        select new Level()
                                        {
                                            Id = Convert.ToInt32(row["LevelDispId"])
                                        }
                                 ).ToList().GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList());

                //add prime filter type
                filter.Levels[0].LevelItems = (from row in tbl.AsEnumerable()
                                               where Convert.ToInt32(row["LevelId"]) == 1
                                               && meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                               select new LevelItems()
                                               {
                                                   Id = Convert.ToInt32(row["FilterTypeId"]),
                                                   ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                   Name = Convert.ToString(row["FilterType"]),
                                                   FilterType = Convert.ToString(row["DispFilterType"]),
                                                   DispFilterType = Convert.ToString(row["DispFilterType"]),
                                                   IsSelectable = false,
                                                   IsShowInSearch = false,
                                                   HasSubLevel = true
                                               }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList();

                //add metric
                filter.Levels[1].LevelItems = new List<LevelItems>();
                foreach (LevelItems item in filter.Levels[0].LevelItems)
                {
                    filter.Levels[1].LevelItems.AddRange((from row in tbl.AsEnumerable()
                                                          where Convert.ToInt32(row["LevelId"]) == 1
                                                          && !Convert.ToString(row["FilterType"]).Equals("Geography", StringComparison.OrdinalIgnoreCase)
                                                           && item.Name == Convert.ToString(row["FilterType"])
                                                           && meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                                          select new LevelItems()
                                                          {
                                                              Id = Convert.ToInt32(row["DispMetricId"]),
                                                              ParentId = Convert.ToInt32(row["FilterTypeId"]),
                                                              ParentName = Convert.ToString(row["FilterType"]),
                                                              Name = Convert.ToString(row["Metric"]),
                                                              searchName = Convert.ToString(row["Metric"]),
                                                              FilterType = Convert.ToString(row["FilterType"]),
                                                              FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                              DispFilterType = Convert.ToString(row["DispFilterType"]),
                                                              ChartTypePIT = Convert.ToString(row["ChartTypePIT"]),
                                                              ChartTypeTrend = Convert.ToString(row["ChartTypeTrend"]),
                                                              IsSelectable = false,
                                                              IsShowInSearch = false,
                                                              HasSubLevel = HasSubLevel(Convert.ToString(row["Metric"])),
                                                              IsHeader = Convert.ToBoolean(row["IsHeader"])
                                                          }).ToList().GroupBy(l => l.Name).Select(g => g.FirstOrDefault()).ToList());
                }
                foreach (Level level in filter.Levels)
                {
                    if (level.Id != 1 && level.Id != 2)
                    {
                        level.LevelItems = (from row in tbl.AsEnumerable()
                                            where level.Id == Convert.ToInt32(row["LevelDispId"])
                                            && meaureTypes.Contains(Convert.ToString(row["SelType"]), StringComparer.OrdinalIgnoreCase)
                                            select new LevelItems()
                                            {
                                                Id = Convert.ToInt32(row["DispMetricId"]),
                                                ParentId = Convert.ToInt32(row["DispMetricId"]),
                                                ParentName = Convert.ToString(row["Metric"]),
                                                Name = Convert.ToString(row["MetricItem"]),
                                                searchName = Convert.ToString(row["MetricItem"]),
                                                FilterType = Convert.ToString(row["FilterType"]),
                                                FilterTypeId = Convert.ToInt32(row["FilterTypeId"]),
                                                DispFilterType = Convert.ToString(row["DispFilterType"]),
                                                ChartTypePIT = Convert.ToString(row["ChartTypePIT"]),
                                                ChartTypeTrend = Convert.ToString(row["ChartTypeTrend"]),
                                                UniqueId = Convert.ToInt32(row["UniqueFilterId"]),
                                                IsSelectable = Convert.ToBoolean(row["IsSelectable"]),
                                                IsShowInSearch = Convert.ToBoolean(row["IsSelectable"]),
                                                HasSubLevel = Convert.ToBoolean(row["HasSubLevel"]),
                                                IsHeader = Convert.ToBoolean(row["IsHeader"])
                                            }).ToList();
                    }

                }
                filters.Add(filter);
            }
            return filters;
        }
        #endregion
        bool HasSubLevel(string name)
        {
            bool _hasSubLevel = true;
            switch (Convert.ToString(name).ToUpper())
            {
                case "WEEKLY +":
                case "MONTHLY +":
                case "QUARTERLY +":
                case "ANNUALLY +":
                case "STORE IN TRADE AREA":
                    {
                        _hasSubLevel = false;
                        break;
                    }
            }
            return _hasSubLevel;
        }
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
        /// <summary>
        ///  load left panel filters
        /// </summary>        
        public LeftPanelFilters LoadFilters(string viewName, string TimePeriodType)
        {
            LeftPanelFilters filters = new LeftPanelFilters();
            try
            {
                if (HttpContext.Current.Session[viewName] != null)
                {
                    filters = HttpContext.Current.Session[viewName] as LeftPanelFilters;
                    return filters;
                }
                filters.filters = new List<List<Filter>>();
                if (HttpContext.Current.Session["filter-data"] != null)
                    ds = HttpContext.Current.Session["filter-data"] as DataSet;
                else
                {
                    //ds = da.GetData("usp_Ishopfilters_test");
                    //ds = da.GetData("usp_Ishopfilters_test_Sabat");
                    ds = da.GetData("usp_Ishopfilters_test");
                    HttpContext.Current.Session["filter-data"] = ds;
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    string TimePeriod = string.Empty;
                    if (viewName.ToLower().IndexOf("e-commerce") > -1)
                    {
                        TimePeriod = (from row in ds.Tables[15].AsEnumerable()
                                      where Convert.ToString(row["PeriodType"]).Equals(TimePeriodType)
                                      orderby row["PeriodId"] descending
                                      select Convert.ToString(row["Value"])).FirstOrDefault();
                    }
                    else
                    {
                        TimePeriod = (from row in ds.Tables[1].AsEnumerable()
                                      where Convert.ToString(row["PeriodType"]).Equals(TimePeriodType)
                                      orderby row["PeriodId"] descending
                                      select Convert.ToString(row["Value"])).FirstOrDefault();
                    }

                    if (!string.IsNullOrEmpty(TimePeriod))
                    {
                        TimePeriod = TimePeriod.Split(' ')[0];
                    }

                    #region Load Time Period
                    filters.TimePeriodlist = LoadTimePeriod(1);
                    #endregion


                    //#region Load Channel Retailer filters
                    //filters.filters.Add(LoadChannelRetailer(2));
                    //#endregion

                    //#region Load Beverages
                    //filters.filters.Add(LoadBeverages(5));
                    //#endregion

                    //#region Load Frequency filters
                    //filters.filters.Add(LoadFrequency(3, "Reports Retailer Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +" }));
                    //filters.filters.Add(LoadFrequency(3, "Correspondance Retailer Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));

                   
                    filters.filters.Add(LoadFrequency(3, "Trips Frequency", new List<string>() { "TOTAL VISITS", "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));
                    filters.filters.Add(LoadFrequency(3, "Shopper Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA", "MAIN STORE/FAVORITE STORE", "MAIN STORE (IN CHANNEL)", "MAIN STORE (ACROSS CHANNEL)", "FAVORITE STORE (IN CHANNEL)", "FAVORITE STORE (ACROSS CHANNEL)" }));
                    //#endregion

                    //#region Load BGM Frequency filters
                    //filters.filters.Add(LoadBGMFrequency(17, "BGM Frequency"));
                    //#endregion

                    //#region Load Groups
                    //filters.filters.Add(LoadGroup(23, "Shopper Groups"));
                    //filters.filters.Add(LoadGroup(24, "Beverage Groups"));
                    //filters.filters.Add(LoadGroup(23, "Reports Retailer Groups", new List<string>() { "DEMOGRAPHICS", "BEVERAGE PURCHASER" }));
                    //filters.filters.Add(LoadGroup(24, "Reports Beverage Groups", new List<string>() { "DEMOGRAPHICS" }));
                    //#endregion

                    //#region Load Demographic filters
                    //filters.filters.Add(LoadDemographic(4, "Demographic"));
                    //#endregion

                    #region Load Advanced filters
                    filters.filters.Add(LoadDemographic(4, "Visits",false));
                    #endregion

                    //#region Load Advanced filters
                    //filters.filters.Add(LoadCorrespondenceMeasure(14, "CorrespondenceMeasure"));
                    //#endregion

                    //#region Load Advanced filters
                    //filters.filters.Add(LoadBeverageWherePurchased(7));
                    //#endregion

                    //#region Reports PathToPurchase Filters
                    //filters.filters.Add(LoadPathToPurchaseFilters(4, "Reports PathToPurchase Filters"));
                    //#endregion

                    //#region Load Geography
                    //filters.GeographyList = LoadGeographyList();
                    //#endregion

                    //#region Measure
                    //filters.filters.Add(LoadMeasure(10, "Retailer Measure",new List<string>() { "Retailer Trip Measures", "Retailer Shopper Measures" }));
                    //filters.filters.Add(LoadMeasure(10, "Beverage Measure", new List<string>() { "Beverage Trip Measures", "Beverage Shopper Measures" }));
                    //#endregion

                    //#region Load Total Measure
                    //filters.filters.Add(LoadTotalMeasure());
                    //#endregion

                    //#region Load Total Measure
                    //filters.filters.Add(LoadBGMBeveragAndNonBeverageItems());
                    //#endregion                    
                    switch (viewName)
                    {
                        case "hdn-tbl-compareretailers":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true, true));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));                                
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Trips)", new List<string>() { "TOTAL VISITS", "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));// "MAIN STORE/FAVORITE STORE"
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Shoppers)", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" , "MAIN STORE/FAVORITE STORE" }));// "MAIN STORE/FAVORITE STORE"
                                break;
                            }
                        case "hdn-tbl-retailerdeepdive":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadGroup(23, "Shopper Groups"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true, true));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Trips)", new List<string>() { "TOTAL VISITS", "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));// "MAIN STORE/FAVORITE STORE"
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Shoppers)", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA", "MAIN STORE/FAVORITE STORE" }));// "MAIN STORE/FAVORITE STORE"
                                break;
                            }
                        case "hdn-chart-compareretailers":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadMeasure(10, "Retailer Measure", new List<string>() { "Retailer Trip Measures", "Retailer Shopper Measures" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true, true));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Trips)", new List<string>() { "TOTAL VISITS", "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));// "MAIN STORE/FAVORITE STORE"
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Shoppers)", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA", "MAIN STORE/FAVORITE STORE" }));// "MAIN STORE/FAVORITE STORE"
                                break;
                            }
                        case "hdn-chart-retailerdeepdive":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadGroup(23, "Shopper Groups"));
                                filters.filters.Add(LoadMeasure(10, "Retailer Measure", new List<string>() { "Retailer Trip Measures", "Retailer Shopper Measures" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true, true));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Trips)", new List<string>() { "TOTAL VISITS", "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));// "MAIN STORE/FAVORITE STORE"
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Shoppers)", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA", "MAIN STORE/FAVORITE STORE" }));// "MAIN STORE/FAVORITE STORE"
                                break;
                            }
                        case "hdn-tbl-comparebeverages":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadBeverageWherePurchased(7));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverageFrequency(6, "Beverage Shopper Frequency"));
                                break;
                            }
                        case "hdn-tbl-beveragedeepdive":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadGroup(24, "Beverage Groups"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadBeverageWherePurchased(7));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverageFrequency(6, "Beverage Shopper Frequency"));
                                break;
                            }
                        case "hdn-chart-comparebeverages":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadMeasure(10, "Beverage Measure", new List<string>() { "Beverage Trip Measures", "Beverage Shopper Measures" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadBeverageWherePurchased(7));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverageFrequency(6, "Beverage Shopper Frequency"));
                                break;
                            }
                        case "hdn-chart-beveragedeepdive":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadGroup(24, "Beverage Groups"));
                                filters.filters.Add(LoadMeasure(10, "Beverage Measure", new List<string>() { "Beverage Trip Measures", "Beverage Shopper Measures" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadBeverageWherePurchased(7));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverageFrequency(6, "Beverage Shopper Frequency"));
                                break;
                            }
                        case "hdn-report-compareretailersshoppers":
                            {
                                filters.filters.Add(LoadPriorityChannelRetailer(33));
                                filters.filters.Add(LoadFrequency(3, "Reports Retailer Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true, true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadCompetitorFrequency(34, "Reports Retailer Frequency-Cross-Retailer Shopper (Shoppers)", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +" }));// "MAIN STORE/FAVORITE STORE"
                                break;
                            }
                        case "hdn-report-retailersshopperdeepdive":
                            {
                                filters.filters.Add(LoadPriorityChannelRetailer(33));
                                filters.filters.Add(LoadGroup(23, "Reports Retailer Groups", new List<string>() { "DEMOGRAPHICS", "BEVERAGE PURCHASER" }));
                                filters.filters.Add(LoadFrequency(3, "Reports Retailer Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true, true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadCompetitorFrequency(34, "Reports Retailer Frequency-Cross-Retailer Shopper (Shoppers)", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +" }));// "MAIN STORE/FAVORITE STORE"
                                break;
                            }
                        case "hdn-report-compareretailerspathtopurchase":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadPathToPurchaseFilters(4, "Reports PathToPurchase Filters", true, true, true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-report-retailerspathtopurchasedeepdive":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadGroup(23, "Shopper Groups", new List<string>() { "DEMOGRAPHICS", "PRE SHOP", "IN STORE", "IN STORE - BEVERAGE DETAIL", "POST SHOP/TRIP SUMMARY" }, false));
                                filters.filters.Add(LoadPathToPurchaseFilters(4, "Reports PathToPurchase Filters", true, true, true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-report-comparebeveragesmonthlypluspurchasers":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-report-beveragemonthlypluspurchasersdeepdive":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadGroup(24, "Reports Beverage Groups", new List<string>() { "DEMOGRAPHICS" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-report-comparebeveragespurchasedetails":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadBeverageWherePurchased(7));
                                filters.filters.Add(LoadPathToPurchaseFilters(4, "Reports PathToPurchase Filters", true, false));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-report-beveragespurchasedetailsdeepdive":
                            {
                                filters.filters.Add(LoadBeverages(5));
                                filters.filters.Add(LoadBeverageWherePurchased(7,false));
                                filters.filters.Add(LoadGroup(24, "Beverage Groups"));
                                filters.filters.Add(LoadPathToPurchaseFilters(4, "Reports PathToPurchase Filters", true, false));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-crossretailer-totalrespondentstripsreport":
                            {
                                filters.filters.Add(LoadGroup(23, "Shopper Groups", new List<string>() { "DEMOGRAPHICS", "PRE SHOP", "IN STORE", "IN STORE - BEVERAGE DETAIL", "POST SHOP/TRIP SUMMARY", "BEVERAGE PURCHASER", "SHOPPER FREQUENCY" }, false));
                                filters.filters.Add(LoadTotalMeasure());
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadFrequency(3, "Correspondance Retailer Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));
                                break;
                            }
                        case "hdn-analysis-acrossshopper":
                            {
                                filters.filters.Add(LoadBGMChannelRetailer(2));
                                filters.filters.Add(LoadBGMBeveragAndNonBeverageItems());
                                filters.filters.Add(LoadBGMFrequency(17, "BGM Frequency"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-analysis-acrosstrips":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadGroup(23, "Reports Beverage Groups", new List<string>() { "DEMOGRAPHICS" }));
                                filters.filters.Add(LoadFrequency(3, "Reports Retailer Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-analysis-withinshopper":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadCorrespondenceMeasure(14, "CorrespondenceMeasure"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-analysis-withintrips":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadGroup(23, "Shopper Groups"));
                                filters.filters.Add(LoadCorrespondenceMeasure(14, "CorrespondenceMeasure"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-analysis-crossretailerfrequencies":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-analysis-crossretailerimageries":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadFrequency(3, "Correspondance Retailer Frequency", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                        case "hdn-dashboard-pathtopurchase":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadGroup(32, "Shopper Groups", new List<string>() { "DEMOGRAPHICS", "PRE SHOP", "IN STORE", "IN STORE - BEVERAGE DETAIL", "POST SHOP/TRIP SUMMARY" }, true, true, true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadFrequency(3, "Cross-Retailer Shopper", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));
                                break;
                            }
                        case "hdn-dashboard-demographic":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadGroup(32, "Shopper Groups", new List<string>() { "DEMOGRAPHICS", "PRE SHOP", "IN STORE", "IN STORE - BEVERAGE DETAIL", "POST SHOP/TRIP SUMMARY", "BEVERAGE PURCHASER" }, true, true, true,true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadFrequency(3, "Cross-Retailer Shopper", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));
                                break;
                            }
                        case "hdn-e-commerce-tbl-comparesites":
                            {
                                filters.EcommTimePeriodList = LoadTimePeriod(15);
                                filters.filters.Add(LoadEComSites(16, "E-Com Sites"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadEComAdvancedFilters(31, "E-Com Visits"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Shopper Frequency", "Online Order Frequency"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Trips Frequency", "Online Order Type"));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));
                                break;
                            }
                        case "hdn-e-commerce-tbl-sitedeepdive":
                            {
                                filters.EcommTimePeriodList = LoadTimePeriod(15);
                                filters.filters.Add(LoadEComSites(16, "E-Com Sites"));
                                filters.filters.Add(LoadEComGroup(30, "E-Com Groups"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadEComAdvancedFilters(31, "E-Com Visits"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Shopper Frequency", "Online Order Frequency"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Trips Frequency", "Online Order Type"));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));
                                break;
                            }
                        case "hdn-e-commerce-chart-comparesites":
                            {
                                filters.EcommTimePeriodList = LoadTimePeriod(15);
                                filters.filters.Add(LoadEComSites(16, "E-Com Sites"));
                                filters.filters.Add(LoadEComMeasure(26, "E-Com Measure", new List<string>() { "Shopper", "Trips" }));

                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadEComAdvancedFilters(31, "E-Com Visits"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Shopper Frequency", "Online Order Frequency"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Trips Frequency", "Online Order Type"));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));
                                break;
                            }
                        case "hdn-e-commerce-chart-sitedeepdive":
                            {
                                filters.EcommTimePeriodList = LoadTimePeriod(15);
                                filters.filters.Add(LoadEComSites(16, "E-Com Sites"));
                                filters.filters.Add(LoadEComMeasure(26, "E-Com Measure", new List<string>() { "Shopper", "Trips" }));

                                filters.filters.Add(LoadEComGroup(30, "E-Com Groups"));
                                filters.filters.Add(LoadDemographic(4, "Demographic", true));
                                filters.filters.Add(LoadEComAdvancedFilters(31, "E-Com Visits"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Shopper Frequency", "Online Order Frequency"));
                                filters.filters.Add(LoadEComFrequency(28, "E-Com Trips Frequency", "Online Order Type"));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadBeverages(12, "Beverage Frequency"));
                                break;
                            }
                        case "hdn-analysis-establishmentdeepdive":
                            {
                                filters.filters.Add(LoadChannelRetailer(2));
                                filters.filters.Add(LoadEstablishmentMeasure(35, "Retailer Measure"));
                                filters.filters.Add(LoadPathToPurchaseFilters(36, "Demographic", true, false, false));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Trips)", new List<string>() { "TOTAL VISITS", "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA" }));// "MAIN STORE/FAVORITE STORE"
                                filters.filters.Add(LoadCompetitorFrequency(34, "Cross-Retailer Shopper (Shoppers)", new List<string>() { "WEEKLY +", "MONTHLY +", "QUARTERLY +", "ANNUALLY +", "STORE IN TRADE AREA", "MAIN STORE/FAVORITE STORE" }));// "MAIN STORE/FAVORITE STORE"
                                break;
                            }
                        case "hdn-crossretailer-sarreport":
                            {
                                filters.filters.Add(LoadRetailerOrCompetitorSar(37, "Retailers"));
                                filters.filters.Add(LoadRetailerOrCompetitorSar(38, "Competitors"));
                                filters.filters.Add(LoadFrequencySar(39, "SarFrequency"));
                                filters.filters.Add(LoadPathToPurchaseFilters(40, "Reports PathToPurchase Filters", true, false, true));
                                filters.filters.Add(LoadGeography("", TimePeriod, TimePeriodType, "", "Default Geography"));
                                break;
                            }
                    }

                }
            }
            catch (Exception ex)
            {
                filters = null;
            }
            HttpContext.Current.Session[viewName] = filters;

            return filters;
        }

        public List<Competitors> GetSarCompetitors(int retailerId)
        {
            List<Competitors> _competitorsList = new List<Competitors>();
            object[] param = { (object)retailerId };
            try
            {
                ds = da.GetData(param, "USP_BreifingBook_CompetiorsList");
                if(ds!=null && ds.Tables.Count != 0)
                {
                    _competitorsList.AddRange(
                        (from row in ds.Tables[0].AsEnumerable()
                         select new Competitors
                         {
                             Id = Convert.ToInt32(row["CompetitorId"]),
                             Name = Convert.ToString(row["CompetitorName"]),
                             LevelId = Convert.ToInt32(row["LevelDispId"])
                         }
                                 ).ToList()
                        );
                }
            }
            catch(Exception ex)
            {

            }
            return _competitorsList;
        }
    }

    #region filter objects
    public class LeftPanelFilters
    {
        public List<List<Filter>> filters { get; set; }
        public List<TimePeriod> TimePeriodlist { get; set; }
        public List<TimePeriod> EcommTimePeriodList { get; set; }
        public List<Frequency> Frequencylist { get; set; }
        public List<GeographyParams> GeographyList { get; set; }
    }
    public class Filter
    {
        public string Name { get; set; }
        public List<Level> Levels { get; set; }
        public List<string> SearchItems { get; set; }
    }
    public class Level
    {
        public int Id { get; set; }
        public List<LevelItems> LevelItems { get; set; }
    }
    public class LevelItems
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int MetricSortId { get; set; }
        public string Name { get; set; }
        public string searchName { get; set; }
        public string ParentName { get; set; }
        public int UniqueId { get; set; }
        public bool IsSelectable { get; set; }
        public bool HasMetricSortId { get; set; }
        public bool HasSubLevel { get; set; }
        public bool IsPriority { get; set; }
        public string FilterType { get; set; }
        public string MeasureType { get; set; }
        public int FilterTypeId { get; set; }
        public string PrimeFilterType { get; set; }
        public int PrimeFilterTypeId { get; set; }
        public string LevelDesc { get; set; }
        public bool IsActive { get; set; }
        public string ToolTip { get; set; }
        public string TrendToolTip { get; set; }
        public int SelID { get; set; }
        public string ChartTypePIT { get; set; }
        public string ChartTypeTrend { get; set; }
        public string DispFilterType { get; set; }
        public bool IsFrequency { get; set; }
        public int FrequencyId { get; set; }
        public List<string> GeoTimePeriods { get; set; }
        public List<string> TrendGeoTimePeriods { get; set; }
        public bool IsShowImage { get; set; }
        public bool ShowAll { get; set; }
        public bool IsHeader { get; set; }
        public bool IsGeography { get; set; }
        public bool IsShowInSearch { get; set; }
        public string ParentOfParent { get; set; }
        public bool IsToggle { get; set; }
        public bool IsChannel { get; set; }

        public bool IsTripAddnFilter { get; set; }

    }
    public class Competitors
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }

    }
    #endregion
}
