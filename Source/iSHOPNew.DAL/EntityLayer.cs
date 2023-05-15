using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.UI.DataVisualization;

namespace iSHOPNew.DAL
{
    [Serializable]
    public class Filters
    {
        public List<TimePeriod> TimePeriodlist { get; set; }
        public Channel Channel { get; set; }
        public Category Category { get; set; }
        public Category BGMCategory { get; set; }
        public List<PrimaryMetric> PrimaryMetriclist { get; set; }
        public List<PrimaryAdvancedFilter> AdvancedFilterlist { get; set; }
        public List<PrimaryAdvancedFilter> VisitAdvancedFilter { get; set; }

        public List<PrimaryAdvancedFilter> EcommerceVisitAdvancedFilter { get; set; }

        public List<PrimaryAdvancedFilter> ChannelFilterlist { get; set; }
        public List<Frequency> Frequencylist { get; set; }
        public List<Frequency> TripsFrequencylist { get; set; }
        public List<Frequency> BGMFrequencylist { get; set; }
        public List<Frequency> ReportFrequencylist { get; set; }
        public List<Frequency> ReportTripsFrequencylist { get; set; }        
        public List<MonthlyPurchase> MonthlyPurchaselist { get; set; }
        public List<PrimaryAdvancedFilter> TripGroupTypelist { get; set; }
        public List<PrimaryAdvancedFilter> ShopperGroupTypelist { get; set; }
        public List<PrimaryAdvancedFilter> Measure { get; set; }
        public List<PrimaryTotalMetric> TotalTripMeasure { get; set; }
        public List<PrimaryTotalMetric> TotalShopperMeasure { get; set; }
        public List<BeverageSelectiontype> BeverageSelection { get; set; }
        public AdvanceVariables AdvanceAnalytics { get; set; }
        public AdvanceVariables TripsAdvanceAnalytics { get; set; }
        public List<NonBeverageList> NonBeverageList { get; set; }
        public List<SelTypes> SelTypelist { get; set; } 

        public List<GeographyParams> GeographyList {get;set;}

        public List<GroupsPrimeFilters> ShopperGroupsPrimeFilterlist { get; set; }
        public List<GroupsPrimeFilters> TripsGroupsPrimeFilterlist { get; set; }
        public List<PrimaryAdvancedFilter> DefaultGeographyFilters { get; set; }
        public List<PrimaryAdvancedFilter> DefaultGeographyFiltersEcom { get; set; }
        

        //For Ecommerce
        public List<TimePeriod> EcommTimePeriodList { get; set; }
        public List<SelTypes> TripEcommerceMeasures { get; set; }
        public List<SelTypes> shopperEcommerceMeasures { get; set; }
        public List<Sites> SitesList { get; set; }
        public List<RightPanelMetrics> RightPanelMeasures { get; set; }
        public List<Frequency> EcommFrequencylist { get; set; }
        public List<Frequency> EcommTripsFrequencylist { get; set; }
        public List<PrimaryAdvancedFilter> EcommShopperGroupTypeList { get; set; }
        public List<GroupsPrimeFilters> EcommShopperGroupsPrimeFilterList { get; set; }
        public SiteFilterlist SiteHTMLFilters { get; set; }
        public GroupsFilterlist Ecomm_GroupsFilterlist { get; set; }

        //added by Nagaraju for Groups Date: 10-04-2017

        public GroupsFilterlist GroupsFilterlist { get; set; }
        public GroupsFilterlist Retailers_GroupsFilterlist { get; set; }
        public GroupsFilterlist Beverages_GroupsFilterlist { get; set; }
        public RetailersFilterlist RetailersFilterlist { get; set; }       
       
        public AdvFilterlist AdvFilterlist { get; set; }
        public VisitsAdvFilterlist VisitsAdvFilterlist { get; set; }

        public TotalMeasureFilterlist TotalTripHTMLMeasure { get; set; }
        public TotalMeasureFilterlist TotalShopperHTMLMeasure { get; set; }

    }
    //added by Nagaraju for Groups Date: 10-04-2017
    public class TotalMeasureFilterlist
    {
        public SearchHTMLEntity SearchObj { get; set; }
    }
    public class SiteFilterlist
    {
        public SearchHTMLEntity SearchObj { get; set; }
    }
    public class GroupsFilterlist
    {
        public SearchHTMLEntity SearchObj { get; set; }
    }
    public class RetailersFilterlist
    {
        public SearchHTMLEntity SearchObj { get; set; }
    }
    public class AdvFilterlist
    {
        public SearchHTMLEntity SearchObj { get; set; }
        public RetailerStringList StringList { get; set; }
    }
    public class VisitsAdvFilterlist
    {
        public SearchHTMLEntity SearchObj { get; set; }
        public RetailerStringList StringList { get; set; }
    }
    public class SearchHTMLEntity
    {
        public string HTML_String { get; set; }     
        public List<string> SearchItems { get; set; }
        public List<string> ReportsSearchItems { get; set; }

        public List<string> ShopperSearchItems { get; set; }
        public List<string> TripsSearchItems { get; set; }
        public List<string> PrioritySearchItems { get; set; }
    }

    public class RetailerStringList
    {
        public List<StringBuilder> HTML_StringBuilder { get; set; }
        public List<string> HTML_String { get; set; }
        public List<string> SearchItems { get; set; }
        public List<string> PrioritySearchItems { get; set; }
    }

    public class GroupsPrimeFilters
    {
        public string Id { get; set; }
        public string PrimeFilterType { get; set; }
        public string FilterType { get; set; }
    }  
    public class Channel
    {
        public List<string> Lavels { get; set; }
        public List<ChannelOrCategory> ChannelOrCategorylist { get; set; }
        public RetailersFilterlist RetailersFilterlist { get; set; }
    }   
    public class ChannelOrCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DBName { get; set; }
        public string TopPosition { get; set; }
        public string BottomPosition { get; set; }
        public bool IsSelectable { get; set; }
        public List<ChannelOrCategoryLavel> ChannelOrCategoryLavel { get; set; }
        public string ShopperDBName { get; set; }
        public string TripsDBName { get; set; }
        public string LevelDesc { get; set; }
        public string UniqueId { get; set; }
    }
    public class ChannelOrCategoryLavel
    {
        public string Lavel { get; set; }
        public List<RetailerOrBrand> LavelRetailerlist { get; set; }
    }

    public class Category
    {
        public List<string> Lavels { get; set; }
        public List<CategoryOrBeverage> CategoryOrBeveragelist { get; set; }
        public SearchHTMLEntity SearchObj { get; set; }
    }
    public class CategoryOrBeverage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DBName { get; set; }
        public string TopPosition { get; set; }
        public string BottomPosition { get; set; }
        public bool TopEnabled { get; set; }
        public List<CategoryOrBeverageLavel> CategoryOrBeverageLavel { get; set; }
        public string ShopperDBName { get; set; }
        public string TripsDBName { get; set; }
        public string LevelDesc { get; set; }
        public string UniqueId { get; set; }
        public string isSelectable { get; set; }
    }
    public class CategoryOrBeverageLavel
    {
        public string Lavel { get; set; }
        public List<RetailerOrBrand> LavelRetailerlist { get; set; }
    }

    public class NonBeverageList {
        public string FilterTypeId { get; set; }
        public string MetrticItemId { get; set; }
        public string MetricItem { get; set; }
        public string UniqueId { get; set; }
    }

    public class RetailerOrBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DBName { get; set; }
        public int LevelId { get; set; }
        public string ShopperDBName { get; set; }
        public string TripsDBName { get; set; }
        public string LevelDesc { get; set; }
        public string UniqueId { get; set; }
        public string PriorityId { get; set; }
    }
    public class Frequency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubFrequency { get; set; }
        public List<Frequency> Frequencylist { get; set; }
        public string UniqueId { get; set; }
    }

    public class BeverageSelectiontype
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Params { get; set; }
        public string UniqueId { get; set; }
    }
    public class TimePeriod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TimePeriod> TimePeriodlist  { get; set; }
        public List<String> Sliderlist { get; set; }
        public string UniqueId { get; set; }
    }
    public class SelTypes
    {
        public string SelType { get; set; }
        public string Name { get; set; }    
        public List<GroupType> GroupTypelist { get; set; }
        public SearchHTMLEntity SearchObj { get; set; }

        public string html1 { get; set; }
        public string html2 { get; set; }
        public string html3 { get; set; }
        public string html4 { get; set; }
        public string html5 { get; set; }      
    }
    public class GroupType
    {
        public string GroupName { get; set; }
        public string SelType { get; set; }        
        public string GroupId { get; set; }
        public string FilterType { get; set; }
        public List<PrimaryAdvancedFilter> PrimaryAdvancedFilter { get; set; }      
    }


    public class PrimaryAdvancedFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MetricItem { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Level { get; set; }
        public string ParentId { get; set; }
        //For Measure Selection Begin
        public string selectType { get; set; }
        public string SelType { get; set; }
        public string FilterType { get; set; }
        public string FilterTypeId { get; set; }
        public string ChartTypePIT { get; set; }
        public string ChartTypeTrend { get; set; }
        public string DBName { get; set; }
        public string ShopperDBName { get; set; }
        public string TripsDBName { get; set; }
        public string UniqueId { get; set; }
        public string SearchName { get; set; }
        public string ToolTip { get; set; }
        public string PrimeFilterType { get; set; }
        public string PrimeFilterTypeId { get; set; }
        //For Measure Selection End
        public List<SecondaryAdvancedFilter> SecondaryAdvancedFilterlist = null;
    }
    public class SecondaryAdvancedFilter
    {
        public string Id { get; set; }
        public string LevelId { get; set; }
        public string Name { get; set; }
        public string Metric { get; set; }
        public string FullName { get; set; }
        public string MetricId { get; set; }
        public string Level { get; set; }
        public string ParentId { get; set; }
        public string DBName { get; set; }
        public string FilterTypeId { get; set; }
        public string active { get; set; }      
        public string isGeography { get; set; }
        public string ShopperDBName { get; set; }
        public string TripsDBName { get; set; }
        public string UniqueId { get; set; }
        public string ParentDetails { get; set; }
        public string ChartTypePIT { get; set; }
        public string ChartTypeTrend { get; set; }
        public string SearchName { get; set; }
        public string ToolTip { get; set; }
        public string FilterType { get; set; }
        public string PrimeFilterType { get; set; }
        public string PrimeFilterTypeId { get; set; }
        public List<SecondaryAdvancedFilter> SecondaryAdvancedFilterlist = null;
    }
    public class PrimaryMetric
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public List<SecondaryMetrics> Metriclist  { get; set; }
        public string UniqueId { get; set; }
    }
    public class SecondaryMetrics
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int LevelId { get; set; }
        public string UniqueId { get; set; }
    }
    public class PrimaryTotalMetric
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string SelectType { get; set; }
        public string LevelId { get; set; }
        public List<SecondaryTotalMetrics> Metriclist { get; set; }
    }
    public class SecondaryTotalMetrics
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string SelectType { get; set; }
        public string ParentId { get; set; }
        public string LevelId { get; set; }
        public string UniqueId { get; set; }
    }

    public class MonthlyPurchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DBName { get; set; }
        public string LevelId { get; set; }
        public string ParentId { get; set; }
        public List<MonthlyPurchase> MonthlyPurchseList { get; set; }
        public string UniqueId { get; set; }
    }
    //iSHOP Params
    public class iSHOPParams
    {
        private string _benchMark = string.Empty;
        private string _benchMarkShortName = string.Empty;
        private string _compare1 = string.Empty;
        private string _compare2 = string.Empty;
        private string _compare3 = string.Empty;
        private string _compare4 = string.Empty;
        private string _compare5 = string.Empty;
        private string _shopperSegment = string.Empty;
        private string _shopperFrequency = string.Empty;
        private string _shopperbrand = string.Empty;
        private string _customfiltersRestoreitems = string.Empty;
        private string _filtersRestoreitems = string.Empty;
        private string _geography = string.Empty;
        private string _benchmarkprioritytype = string.Empty;
        private string _samplesize = string.Empty;
        private string _issamplesize = string.Empty;
        private string _retailer = string.Empty;

        private string _leftheader = string.Empty;
        private string _leftbody = string.Empty;
        private string _rightheader = string.Empty;
        private string _rightbody = string.Empty;

        public string Tab1 { get; set; }
        public string Tab2 { get; set; }
        public string Tab3 { get; set; }
        public string Tab4 { get; set; }

        public string benchmarklist { get; set; }

        public string channelretailer { get; set; }
        public string channelretailershortname { get; set; }
        public string type { get; set; }
        public string filterid { get; set; }
        public string select { get; set; }

        public string statpositive { get; set; }
        public string statnegative { get; set; }

        public string Compare1Shortname { get; set; }
        public string Compare2Shortname { get; set; }
        public string Compare3Shortname { get; set; }
        public string Compare4Shortname { get; set; }
        public string Compare5Shortname { get; set; }

        public bool IsNoDataAvailable { get; set; }

        public DataSet Data_Set
        {
            get;
            set;
        }
        public string TimePeriod
        {
            get;
            set;
        }
        public string PreviousTimePeriod
        {
            get;
            set;
        }
        public string SampleSizeHeaderTable
        {
            get;
            set;
        }
        public string SampleSizeBodyTable
        {
            get;
            set;
        }
        public string LeftHeader
        {
            get
            {
                return _leftheader;
            }
            set
            {
                _leftheader = value;
            }
        }


        public string LeftBody
        {
            get
            {
                return _leftbody;
            }
            set
            {
                _leftbody = value;
            }
        }

        public string RightHeader
        {
            get
            {
                return _rightheader;
            }
            set
            {
                _rightheader = value;
            }
        }
        public string RightBody
        {
            get
            {
                return _rightbody;
            }
            set
            {
                _rightbody = value;
            }
        }

        public string htmlstring
        {
            get
            {
                return _retailer;
            }
            set
            {
                _retailer = value;
            }
        }

        public string Retailer
        {
            get
            {
                return _retailer;
            }
            set
            {
                _retailer = value;
            }
        }
        public string SampleSize
        {
            get
            {
                return _samplesize;
            }
            set
            {
                _samplesize = value;
            }
        }
        public string ISSampleSize
        {
            get
            {
                return _issamplesize;
            }
            set
            {
                _issamplesize = value;
            }
        }
        public string BenchMark
        {
            get
            {
                return _benchMark;
            }
            set
            {
                _benchMark = value;
            }
        }
        public string BenchmarkPrioritytype
        {
            get
            {
                return _benchmarkprioritytype;
            }
            set
            {
                _benchmarkprioritytype = value;
            }
        }

        public string BenchMarkShortName
        {
            get
            {
                return _benchMarkShortName;
            }
            set
            {
                _benchMarkShortName = value;
            }
        }

        public string Compare1
        {
            get
            {
                return _compare1;
            }
            set
            {
                _compare1 = value;
            }
        }

        public string Compare2
        {
            get
            {
                return _compare2;
            }
            set
            {
                _compare2 = value;
            }
        }
        public string Compare3
        {
            get
            {
                return _compare3;
            }
            set
            {
                _compare3 = value;
            }
        }
        public string Compare4
        {
            get
            {
                return _compare4;
            }
            set
            {
                _compare4 = value;
            }
        }

        public string Compare5
        {
            get
            {
                return _compare5;
            }
            set
            {
                _compare5 = value;
            }
        }

        public string ShopperSegment
        {
            get
            {
                return _shopperSegment;
            }
            set
            {
                _shopperSegment = value;
            }
        }

        public string ShopperFrequency
        {
            get
            {
                return _shopperFrequency;
            }
            set
            {
                _shopperFrequency = value;
            }
        }
        public string brand
        {
            get
            {
                return _shopperbrand;
            }
            set
            {
                _shopperbrand = value;
            }
        }

        public string CustomFilters { get; set; }

        public string CustomfiltersRestoreitems
        {
            get
            {
                return _customfiltersRestoreitems;
            }
            set
            {
                _customfiltersRestoreitems = value;
            }
        }

        public string FiltersRestoreitems
        {
            get
            {
                return _filtersRestoreitems;
            }
            set
            {
                _filtersRestoreitems = value;
            }
        }

        public string Geography
        {
            get
            {
                return _geography;
            }
            set
            {
                _geography = value;
            }
        }
    }
    //
    public class BenchCompSelect
    {
        public string benchmarklist { get; set; }
        public string comparisonlist { get; set; }
        public string benchmark { get; set; }
        public string comparison { get; set; }
        public string ChannelCategory { get; set; }
        public string ChannelCategoryList { get; set; }
        public string filterlist { get; set; }
        public string filterHead { get; set; }
        public string typelist { get; set; }
        public string select { get; set; }
        public string Type { get; set; }
        public string Favorite { get; set; }
        public string BCRList { get; set; }
        public string MeasureHead { get; set; }
        public string Measurelist { get; set; }
        public string DataSelection { get; set; }
        public string SelectedData { get; set; }
    }
    public class ProfilerChartParams
    {
        public string ViewType { get; set; }
        public List<string> selectedMetrics = new List<string>();
        public List<string> BCFullNames = new List<string>();
        public DataSet ChartDataSet { get; set; }
        public string SingleSelection { get; set; }
        public string ChartType { get; set; }
        public List<string> Comparison_DBNames { get; set; }
        public List<string> Comparison_ShortNames { get; set; }
        public string Benchmark { get; set; }
        public string BenchmarkShortName { get; set; }
        public string Comparisonlist { get; set; }
        public string ComparisonShortNames { get; set; }
        public string TimePeriod { get; set; }
        public string ShortTimePeriod { get; set; }
        public string ShopperSegment { get; set; }
        public string FrequencyTitle { get; set; }
        public string ShopperFrequency { get; set; }
        public string Add_ShopperFrequency { get; set; }

        public string ShopperFrequencyShortName { get; set; }
        public string Metric { get; set; }
        public string MetricShortName { get; set; }
        public string ActiveTab { get; set; }
        public string FilterShortNames { get; set; }
        public string Filters { get; set; }
        public string ModuleBlock { get; set; }
        public string SelectedMetrics { get; set; }
        public bool Top_10 { get; set; }
        public string SelectedMetricsIds { get; set; }
        public string View { get; set; }
        public int ChartHeight { get; set; }
        public int ChartWidth { get; set; }
        public List<string> ChartXValues { get; set; }

        public string SelectedStatTest { get; set; }

        public bool IsSelectionChange { get; set; }
        public string StatTest { get; set; }
        public double StatPositive { get; set; }
        public double StatNegative { get; set; }
        public double StatTesting { get; set; }

        public string CustomBase_DBName { get; set; }
        public string CustomBase_ShortName { get; set; }
        public string CustomBase_UniqueId { get; set; }

        public List<string> Comparison_UniqueIds { get; set; }
        public string TimePeriod_UniqueId { get; set; }
        public string TimePeriodFrom_UniqueId { get; set; }
        public string TimePeriodTo_UniqueId { get; set; }
        public string ShopperSegment_UniqueId { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string Add_ShopperFrequency_UniqueId { get; set; }

        public string Sigtype_UniqueId { get; set; }
        public string Filter_UniqueId { get; set; }
        public string Beverage_UniqueId { get; set; }

        public string Metric_UniqueId { get; set; }
        public string Filtertypeid_UniqueId { get; set; }
        public string TabName { get; set; }
        public string TabIndexId { get; set; }
        public string Tab_Id_mapping { get; set; }
        public string ChartXValues_UniqueId { get; set; }
        public string beverageSelectionType_UniqueId { get; set; }
        public string TrendType { get; set; }
        public List<string> ComparisonNames { get; set; }

        public string CompetitorRetailer_Name { get; set; }
        public string CompetitorRetailer_UniqueId { get; set; }
        public string CompetitorFrequency_Name { get; set; }
        public string CompetitorFrequency_UniqueId { get; set; }
    }
  
    public class HighChartData
    {
        public List<string> BrandList { get; set; }
        public List<string> MetricList { get; set; }
        public List<double> SampleSize { get; set; }
        public List<double> NumberOfResponses { get; set; }
        public List<List<double>> ValueData { get; set; }
        public List<List<double>> SignificanceData { get; set; }
        public List<List<double>> ChangeVsPy { get; set; }

        public string StatPositive { get; set; }
        public string StatNegative { get; set; }
    }
    public class IValueData
    {
        public List<double> Values { get; set; }
    }

    public class ISignificanceData
    {
        public List<double> SignifData { get; set; }
    }
    public class TableParams
    {
        public string ViewType { get; set; }
        public string TabName { get; set; }
        public string TabIndexId { get; set; }
        public bool Tab_Id_mapping { get; set; }
        public List<string> Comparison_DBNames { get; set; }
        public List<string> Comparison_ShortNames { get; set; }
        public List<string> Comparison_UniqueIds { get; set; }

        public string CustomBase_DBName { get; set; }
        public string CustomBase_ShortName { get; set; }
        public string CustomBase_UniqueId { get; set; }

        public string TimePeriod_UniqueId { get; set; }
        public string TimePeriodFrom_UniqueId { get; set; }
        public string TimePeriodTo_UniqueId { get; set; }

        public string ShopperSegment_UniqueId { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string Sigtype_UniqueId { get; set; }
        public string Filter_UniqueId { get; set; }
        public string Beverage_UniqueId { get; set; }
        public string Add_ShopperFrequency_UniqueId { get; set; }

        public string CheckSampleSize_SPName { get; set; }
        public string Main_SPName { get; set; }
        public string BenchMark { get; set; }
        public List<string> Comparisonlist { get; set; }
        public string TimePeriod { get; set; }
        public string TimePeriodShortName { get; set; }
        public string ShopperSegment { get; set; }
        public string SingleSelection { get; set; }
        public string Filter { get; set; }        
        public string FilterShortname { get; set; }
        public string ShopperFrequency { get; set; }
        public string Add_ShopperFrequency { get; set; }
        public List<string> ShortNames { get; set; }
        public string StatPositive { get; set; }
        public string StatNegative { get; set; }
        public bool IsExportToExcel { get; set; }       
        public string StatTest { get; set; }
        public List<string> ExportSheetList { get; set; }
        public List<string> ExportSheetNames { get; set; }
        public List<string> Mainspnames { get; set; }
        public List<string> SampleSpnames { get; set; }
        public bool IsBeverages { get; set; }
        public bool IsWherePurchased { get; set; }
        public string CompetitorRetailer_Name { get; set; }
        public string CompetitorRetailer_UniqueId { get; set; }
        public string CompetitorFrequency_Name { get; set; }
        public string CompetitorFrequency_UniqueId { get; set; }
    }
    public class TimeDetails
    {
        public string year { get; set; }
        public string month { get; set; }
        public string date { get; set; }
        public string hours { get; set; }
        public string minutes { get; set; }
        public string seconds { get; set; }
    }

    public class GeoCustomDetails
    {
        public string TagName { get; set; }
        public string TimePeriod { get; set; }
        public string TimePeriodType { get; set; } 
        public string CheckModule { get; set; }
    }
    public class ReportGeneratorParams
    {
        public int SampleSizeCheck { get; set; }
        public int ReportFlag { get; set; }
        public bool Tab_Id_mapping { get; set; }
        public List<string> Comparison_DBNames { get; set; }
        public List<string> Comparison_ShortNames { get; set; }
        public string StatTest { get; set; }

        public DataSet SOAPData { get; set; }
        public Dictionary<string, DataSet> ChartDataSet { get; set; }
        public string Benchmark { get; set; }
        public string BenchmarkShortName { get; set; }
        public string Comparisonlist { get; set; }
        public string ComparisonShortNames { get; set; }

        public List<string> Comparison_UniqueIds { get; set; }

        public string CustomBase_DBName { get; set; }
        public string CustomBase_ShortName { get; set; }
        public string CustomBase_UniqueId { get; set; }

        public string TimePeriod_UniqueId { get; set; }
        public string TimePeriodFrom_UniqueId { get; set; }
        public string TimePeriodTo_UniqueId { get; set; }

        public string ShopperSegment_UniqueId { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string Sigtype_UniqueId { get; set; }
        public string Filter_UniqueId { get; set; }
        public string Beverage_UniqueId { get; set; }

        public List<string> ComparisonShortNamelist { get; set; }

        public string TimePeriod { get; set; }
        public string ShortTimePeriod { get; set; }
        public string ShopperSegment { get; set; }
        public string ShopperSegmentShortName { get; set; }
        public string Geography { get; set; }
        public string GeographyShortName { get; set; }

        public string FrequencyTitle { get; set; }
        public string ShopperFrequency { get; set; }
        public string ShopperFrequencyShortName { get; set; }
        public string SPName { get; set; }
        public string FilterShortNames { get; set; }
        public string Filters { get; set; }
        public string ModuleBlock { get; set; }
        public string SelectedMetrics { get; set; }
        public string View { get; set; }
        public int ChartHeight { get; set; }
        public int ChartWidth { get; set; }

        public List<string> ChartXValues { get; set; }
        public List<string> SelectedReports { get; set; }

        public double StatPositive { get; set; }
        public double StatNegative { get; set; }
        public double StatTesting { get; set; }

        public string Geography_UniqueId { get; set; }
        public string Group_UniqueId { get; set; }
        public string ComparisonList_UniqueIds { get; set; }
        public string CompetitorRetailer_Name { get; set; }
        public string CompetitorRetailer_UniqueId { get; set; }
        public string CompetitorFrequency_Name { get; set; }
        public string CompetitorFrequency_UniqueId { get; set; }
    }

    public class LowSampleSizeItems
    {
        public string Name { get; set; }
        public bool IsLowSampleSize { get; set; }     
    }
    public class UseDirectionallyItems
    {
        public string Name { get; set; }      
        public bool IsUseDirectionally { get; set; }
    }
    public class SampleSizeParams
    {
        public List<string> Shopperlist { get; set; }
        public List<string> Tripslist { get; set; }
        public bool GenerateReport { get; set; }
        public bool IsUseDirectionally { get; set; }
        public List<LowSampleSizeItems> LowSampleSizeItems { get; set; }
        public List<UseDirectionallyItems> UseDirectionallyItems { get; set; }
        public bool IsAllLowSampleSizes { get; set; }
    }

    public class CrossFrequenciesParams
    {
        public string Retailer { get; set; }
        public string timePeriod { get; set; }
        public string shortTimePeriod { get; set; }
        public string isChange { get; set; }
        public string width { get; set; }
        public int height { get; set; }
        public string filter { get; set; }
        public string TimePeriod_UniqueId { get; set; }
        public string Comparison_UniqueIds { get; set; }
        public string ShopperSegment_UniqueId { get; set; }
    }

    public class BGMParamsNew
    {
        public string BenchMark { get; set; }
        public string FilterShortNames { get; set; }
        public string timePeriod { get; set; }
        public string previoustimePeriod { get; set; }
        public string ShopperFrequency { get; set; }
        public string selectionBevorNonBev { get; set; }
        public string timeType { get; set; }
        public string filter { get; set; }
        public string BevorNonBevShortName { get; set; }
        public string SelectedBevorNonBevShortName { get; set; }
        public string BenchmarkShortName { get; set; }
        //Unique Id Parameters
        public string TimePeriod_UniqueId { get; set; }
        public List<string> Comparison_UniqueIds { get; set; }
        public string Beverage_UniqueId { get; set; }
        public string Filter_UniqueId { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public List<string> BeverageNonBeveragelist { get; set; }
        //End

    }

    public class CRImageries
    {
        public string BenchMark { get; set; }
        public string Compare { get; set; }
        public string timePeriod { get; set; }
        public string shortTimePeriod { get; set; }
        public string ShopperFrequencyShort { get; set; }
        public string ShopperFrequency { get; set; }
        public string isChange { get; set; }
        public string width { get; set; }
        public int height { get; set; }
        public string filter { get; set; }
        public string Selected_StatTest { get; set; }
        public string TimePeriod_UniqueId { get; set; }
        public string Benchmark_UniqueIds { get; set; }
        public string Comparison_UniqueIds { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string ShopperSegment_UniqueId { get; set; }
        public string Sigtype_UniqueId { get; set; }
        public string CustomBase_UniqueId { get; set; }
        public string CustomBase_ShortName { get; set; }
        public string Comparison_ShortNames { get; set; }
    }

    public class BGMParams
    {
        private string _benchMark = string.Empty;
        private string _benchMarkShortName = string.Empty;
        private string _compare1 = string.Empty;
        private string _compare2 = string.Empty;
        private string _compare3 = string.Empty;
        private string _compare4 = string.Empty;
        private string _compare5 = string.Empty;
        private string _shopperSegment = string.Empty;
        private string _shopperFrequency = string.Empty;
        private string _shopperbrand = string.Empty;
        private string _customfiltersRestoreitems = string.Empty;
        private string _filtersRestoreitems = string.Empty;
        private string _geography = string.Empty;
        private string _benchmarkprioritytype = string.Empty;
        private string _samplesize = string.Empty;
        private string _issamplesize = string.Empty;
        private string _retailer = string.Empty;
        
        private string _leftheader = string.Empty;
        private string _leftbody = string.Empty;
        private string _rightheader = string.Empty;
        private string _rightbody = string.Empty;

        public string selectedProduct { get; set; }
        public string Tab1 { get; set; }
        public string Tab2 { get; set; }
        public string Tab3 { get; set; }
        public string Tab4 { get; set; }

        public string benchmarklist { get; set; }

        public string channelretailer { get; set; }
        public string channelretailershortname { get; set; }
        public string type { get; set; }
        public string filterid { get; set; }
        public string select { get; set; }
        public List<string> BeverageNonBeveragelist { get; set; }

        public string statpositive { get; set; }
        public string statnegative { get; set; }

        public string PercentStat { get; set; }

        public string Compare1Shortname { get; set; }
        public string Compare2Shortname { get; set; }
        public string Compare3Shortname { get; set; }
        public string Compare4Shortname { get; set; }
        public string Compare5Shortname { get; set; }

        public DataSet Data_Set
        {
            get;
            set;
        }
        public string TimePeriod
        {
            get;
            set;
        }
        public string PreviousTimePeriod
        {
            get;
            set;
        }
        public string SampleSizeHeaderTable
        {
            get;
            set;
        }
        public string SampleSizeBodyTable
        {
            get;
            set;
        }
        public string LeftHeader
        {
            get
            {
                return _leftheader;
            }
            set
            {
                _leftheader = value;
            }
        }


        public string LeftBody
        {
            get
            {
                return _leftbody;
            }
            set
            {
                _leftbody = value;
            }
        }

        public string RightHeader
        {
            get
            {
                return _rightheader;
            }
            set
            {
                _rightheader = value;
            }
        }
        public string RightBody
        {
            get
            {
                return _rightbody;
            }
            set
            {
                _rightbody = value;
            }
        }

        public string htmlstring
        {
            get
            {
                return _retailer;
            }
            set
            {
                _retailer = value;
            }
        }

        public string Retailer
        {
            get
            {
                return _retailer;
            }
            set
            {
                _retailer = value;
            }
        }
        public string SampleSize
        {
            get
            {
                return _samplesize;
            }
            set
            {
                _samplesize = value;
            }
        }
        public string ISSampleSize
        {
            get
            {
                return _issamplesize;
            }
            set
            {
                _issamplesize = value;
            }
        }
        public string BenchMark
        {
            get
            {
                return _benchMark;
            }
            set
            {
                _benchMark = value;
            }
        }
        public string BenchmarkPrioritytype
        {
            get
            {
                return _benchmarkprioritytype;
            }
            set
            {
                _benchmarkprioritytype = value;
            }
        }

        public string BenchMarkShortName
        {
            get
            {
                return _benchMarkShortName;
            }
            set
            {
                _benchMarkShortName = value;
            }
        }

        public string Compare1
        {
            get
            {
                return _compare1;
            }
            set
            {
                _compare1 = value;
            }
        }

        public string Compare2
        {
            get
            {
                return _compare2;
            }
            set
            {
                _compare2 = value;
            }
        }
        public string Compare3
        {
            get
            {
                return _compare3;
            }
            set
            {
                _compare3 = value;
            }
        }
        public string Compare4
        {
            get
            {
                return _compare4;
            }
            set
            {
                _compare4 = value;
            }
        }

        public string Compare5
        {
            get
            {
                return _compare5;
            }
            set
            {
                _compare5 = value;
            }
        }

        public string ShopperSegment
        {
            get
            {
                return _shopperSegment;
            }
            set
            {
                _shopperSegment = value;
            }
        }

        public string ShopperFrequency
        {
            get
            {
                return _shopperFrequency;
            }
            set
            {
                _shopperFrequency = value;
            }
        }
        public string brand
        {
            get
            {
                return _shopperbrand;
            }
            set
            {
                _shopperbrand = value;
            }
        }

        public string FilterShortNames { get; set; }
        public string CustomFilters { get; set; }

        public string CustomfiltersRestoreitems
        {
            get
            {
                return _customfiltersRestoreitems;
            }
            set
            {
                _customfiltersRestoreitems = value;
            }
        }

        public string FiltersRestoreitems
        {
            get
            {
                return _filtersRestoreitems;
            }
            set
            {
                _filtersRestoreitems = value;
            }
        }

        public string Geography
        {
            get
            {
                return _geography;
            }
            set
            {
                _geography = value;
            }
        }
    }


    public class SOAPParams
    {
        public string TimePeriod { get; set; }
        public string ShortTimePeriod { get; set; }
        public string ShopperSegment { get; set; }
        public string ShopperSegmentShortName { get; set; }
        public string Geography { get; set; }
        public string GeographyShortName { get; set; }
        public string ShopperGroup { get; set; }
        public string ShopperFrequency { get; set; }
        public string ShoppingFrequencyShortname { get; set; }
        public string Filters { get; set; }
        public string FilterShortNames { get; set; }
        //Unique Id Parameters
        public string TimePeriod_UniqueId { get; set; }
        public string Comparison_UniqueIds { get; set; }
        public string Geography_UniqueId { get; set; }
        public string Group_UniqueId { get; set; }
        public string Filter_UniqueId { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        //End

    }

    public class AdvancedAnalyticsParams
    {
        public string ViewType { get; set; }
        public DataSet ChartDataSet { get; set; }
        public string ChartType { get; set; }
        public string Benchmark { get; set; }
        public string BenchmarkShortName { get; set; }
        public string Comparisonlist { get; set; }
        public List<string> ComparisonItems { get; set; }
        public List<string> ComparisonShortNameItems { get; set; }
        public string ShortNames { get; set; }
        public List<string> StoreidItems { get; set; }
        public string ComparisonShortNames { get; set; }
        public string TimePeriod { get; set; }
        public string ShortTimePeriod { get; set; }
        public string ShopperSegment { get; set; }
        public string FrequencyTitle { get; set; }
        public string ShopperFrequency { get; set; }
        public string ShopperFrequencyShortName { get; set; }
        public string Metric { get; set; }
        public string MetricShortName { get; set; }
        public string ActiveTab { get; set; }
        public string FilterShortNames { get; set; }
        public string Filters { get; set; }
        public string ModuleBlock { get; set; }
        public string SelectedMetrics { get; set; }
        public string View { get; set; }
        public int ChartHeight { get; set; }
        public int ChartWidth { get; set; }

        public List<string> ChartXValues { get; set; }

        public double StatPositive { get; set; }
        public double StatNegative { get; set; }
        public double StatTesting { get; set; }

        public string LowVolume { get; set; }
        public List<string> LowVolumelist { get; set; }
        public string Comparison_UniqueIds { get; set; }
        public string TimePeriod_UniqueId { get; set; }
        public string ShopperSegment_UniqueId { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string MetricUniqueId { get; set; }
        public string GroupUniqueId { get; set; }
        public string sNoDataRetailersUniqueId { get; set; }
    }

    public class CorrespondenceParams
    {
        public string LowSampleSize { get; set; }
        public string LowSampleSizeShortNames { get; set; }
        public string StoreidItems { get; set; }
        public string LowVariables { get; set; }
        public List<RData> Rlist { get; set; }
        public int Objective_Count { get; set; }
        public iSHOPParams ishoparams { get; set; }
        public string R_Table { get; set; }

        public string Get_SampleSize { get; set; }
      

    }
    public class RData
    {
        public string name { get; set; }
        public string x { get; set; }
        public string y { get; set; }
    }
    public class AdvanceVariables
    {
        public List<AdvanceFilterSlectVariabl> ChannelVariables { get; set; }
        public List<AdvanceFilterSlectVariabl> RetailerVariables { get; set; }
    }
    public class AdvanceFilterSlectVariabl
    {
        public int MetricId { get; set; }
        public string MetricName { get; set; }
        public List<MetricItemList> MetricItemList { get; set; }
    }

    public class MetricItemList {
        public int MetricItemId { get; set; }
        public string MetricItemName { get; set; }
        public int SelId { get; set; }
        public string SelType { get; set; }
        public int UniqueFilterId { get; set; }
        public string TripShopperType { get; set; }
    }

   public class GeographyParams
    {
        public string Metric { get; set; }
        public string MetricItemId { get; set; }
        public string MetricItem { get; set; }
        public string UniqueId { get; set; }

    }

    public class Sites
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int MetricItemId { get; set; }
        public string Name { get; set; }
        public string LevelId { get; set; }
        public List<SitesMetricItem> SiteList { get; set; }
    }

    public class SitesMetricItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MetricItemId { get; set; }
        public string MetricItemName { get; set; }
        public string UniqueId { get; set; }
        public string LevelId { get; set; }
        public string ParentId { get; set; }
    }
    public class RightPanelMetrics
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RightPanelMetricItem> MetricList { get; set; }
    }

    public class RightPanelMetricItem
    {
        public int MetricItemId { get; set; }
        public string MetricItemName { get; set; }
        public string UniqueId { get; set; }
        public string ParentId { get; set; }
    }
    public class StatTest
    {
        public string PosiValue { get; set; }
        public string NegaValue { get; set; }
        public string Percent { get; set; }
    }
    public class reportparams
    {
        public string tabid { get; set; }
        public string _BenchMark { get; set; }
        public List<string> Comparisonlist { get; set; }
        public string timePeriod { get; set; }
        public string _shortTimePeriod { get; set; }
        public string _ShopperFrequency { get; set; }
        public string _measure { get; set; }
        public string _filter { get; set; }
        public string filterShortname { get; set; }
        public List<string> ShortNames { get; set; }
        public string ExportToExcel { get; set; }
        public List<string> ExportSheetList { get; set; }
        public List<string> ExportSheetNames { get; set; }
        public string Selected_StatTest { get; set; }
        public List<string> Comparison_DBNames { get; set; }
        public List<string> Comparison_ShortNames { get; set; }

        public string TimePeriod_UniqueId { get; set; }
        public string BenchmarkUniqueId { get; set; }
        public string Comparison_UniqueIds { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string ShopperSegment_UniqueId { get; set; }
        public string MeasureUniqueIds { get; set; }
        public string Sigtype_UniqueId { get; set; }
        public string Module { get; set; }
        public string CustomBase_DBName { get; set; }
        public string CustomBase_ShortName { get; set; }
        public string CustomBase_UniqueId { get; set; }

    }
    public class PathToPurchaseMetrics
    {
        public string SampleSize { get; set; }
        public string StatTestSampleSize { get; set; }
        public List<PathToPurchaseMetricEntity> pathToPurchaseMetricEntitylist { get; set; }
    }
    public class PathToPurchaseMetricEntity
    {
        public string MetricType { get; set; }       
        public List<PathToPurchaseEntity> MetricData { get; set; }
    }
    public class PathToPurchaseEntity
    {
        public string Retailer { get; set; }
        public string MetricType { get; set; }
        public string Metric { get; set; }
        public double Volume { get; set; }
        public double Significance { get; set; }
        public double ChangeVolume { get; set; }
        public int Flag { get; set; }
        public bool Selected_Popup_Metric_Item { get; set; }       
    }
    public class PathToPurchaseParams
    {
        public string Comparison_ShortNames { get; set; }
        public string Comparison_UniqueIds { get; set; }     
          
        public string CustomBase_ShortName { get; set; }
        public string CustomBase_UniqueId { get; set; }

        public string TimePeriod { get; set; }
        public string TimePeriodShortName { get; set; }
        public string TimePeriod_UniqueId { get; set; }

        public string FilterShortname { get; set; }
        public string ShopperSegment { get; set; }
        public string ShopperSegment_UniqueId { get; set; }

        public string CustomBaseAdvancedFilters{ get; set; }
        public string CustomBaseAdvancedFilters_UniqueId { get; set; }

        public string ShopperFrequency { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string CustomBaseShopperFrequency { get; set; }
        public string CustomBaseShopperFrequency_UniqueId { get; set; }
        public string StatTest { get; set; }
        public string Sigtype_UniqueId { get; set; }
        public string Sort { get; set; }
        public string TabType { get; set; }
        public string CompetitorRetailer_Name { get; set; }
        public string CompetitorRetailer_UniqueId { get; set; }
        public string CompetitorFrequency_Name { get; set; }
        public string CompetitorFrequency_UniqueId { get; set; }
        public string CustomBaseCompetitorRetailer_Name { get; set; }
        public string CustomBaseCompetitorRetailer_UniqueId { get; set; }
        public string CustomBaseCompetitorFrequency_Name { get; set; }
        public string CustomBaseCompetitorFrequency_UniqueId { get; set; }
        public bool IsOnlineSelected { get; set; }
        public bool IsOnlineSelectedAsBase { get; set; }
    }

    public class P2PDashboardData
    {
        public List<PathToPurchaseMetricEntity> OutputData { get; set; }
        public int NoOfRoads { get; set; }
        public List<changedData> changedData { get; set; }
        public string LeftpanelData { get; set; }
        public string statTest { get; set; }
        public string pptOrPdf { get; set; }
        public int ss { get; set; }
        public string ShopperFrequency { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string Sort { get; set; }
        public string TabType { get; set; }
        public string TimePeriod { get; set; }
        public string Base { get; set; }
        public string CustomBase { get; set; }
        public string Filters { get; set; }

    }
    public class changedData
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    public class P2PPopupDashboardData
    {
        public List<PathToPurchaseEntity> OutputData { get; set; }
        public List<PathToPurchaseEntity> OutputDataSecondary { get; set; }
        public string LeftpanelData { get; set; }
        public string statTest { get; set; }
        public string pptOrPdf { get; set; }
        public string DemofilterName { get; set; }
        public string DemoTitle { get; set; }
        public string DemoTitleSecondary { get; set; }
        public int ss { get; set; }
        public string ShopperFrequency { get; set; }
        public string ShopperFrequency_UniqueId { get; set; }
        public string Sort { get; set; }
        public string TimePeriod { get; set; }
        public string Base { get; set; }
        public string CustomBase { get; set; }
        public string Filters { get; set; }
    }

    public class ReadAsText
    {
        public string Metric { get; set; }
        public string Text { get; set; }
    }
    public class TabLevelReadAsText
    {
        public string Tab { get; set; }
        public List<ReadAsText> TabLevelData { get; set; }
    }
    public class EstablishmentDeepDiveParams
    {
        public string TimePeriod { get; set; }
        public string TimePeriodShortName { get; set; }
        public string TimePeriod_UniqueId { get; set; }
        public string Comparison_ShortNames { get; set; }
        public string Comparison_UniqueIds { get; set; }
        public string Filter { get; set; }
        public string FilterShortname { get; set; }
        public string Filter_UniqueId { get; set; }
        public string MetricShortName { get; set; }
        public string SelectedMetricsNames { get; set; }
        public string SelectedMetricsIds { get; set; }
        public string selectedMetrics { get; set; }

    }
    public class EstablishmentDeepDiveMetrics
    {
        public string Retailer { get; set; }
        public string MetricType { get; set; }
        public string Metric { get; set; }
        public double Volume { get; set; }
        public double DisplayValue { get; set; }
        public double Share { get; set; }
        public double ChangePercentage { get; set; }
        public double SampleSize { get; set; }
    }
    public class FilterPanelData
    {
        public object ID { get; set; }
        public string Name { get; set; }

        public string ParentName { get; set; }
        public object UniqueFilterId { get; set; }
        public string selectionType { get; set; }
        public object FrequencyId { get; set; }
    }
    public class FilterPanelInfo
    {
        public object TimeperiodID { get; set; }
        public string TimeperiodType { get; set; }

        public FilterPanelData BenchMark { get; set; }

        public List<FilterPanelData> CustomBase { get; set; }

        public List<FilterPanelData> Competitors { get; set; }

        public List<FilterPanelData> Frequency { get; set; }

        public List<FilterPanelData> Filters { get; set; }
        public object IsTripsOrShopper { get; set; }
        public string ColorCode { get; set; }
        public object IsRetailerMSS { get; set; }
        public object IsCompetitorMSS { get; set; }

        public object IsChannelSelected { get; set; }

        public object IsTripFilter { get; set; }

        public object IsNonPrioritySelected { get; set; }
        public object corporateOrChannelNetSelected { get; set; }

    }
    public class LowSampleSizeRetailerList
    {
        public string RetailerName { get; set; }
        public string IsUseDirectional { get; set; }

        public string SelectionType { get; set; }
        public string TimePeriodType { get; set; }
        public string RetailerId { get; set; }

        public string UniqueFilterId { get; set; }
    }
}
