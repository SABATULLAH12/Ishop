/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../Slider/jquery-ui-slider-pips.js" />
/// <reference path="../AngularJS/angular.min.js" />
/// <reference path="../lodash.min.js" />
//23-11-17
var getFilterT = "";
var DemogFlag = 0;
var AllDemographicsSF = [];
var AllSubFrequencyDemo = [];
//End 23-11-17
var html = "";
var width = 1024;
var height = 520;
var postBackData = "";
var CustomBaseFlag = 0;
//stat test
var stattest_obj = new Object();
var StatPercent = "";
var PositiveValue = "";
var NegativeValue = "";
var selectedModule = new Object();
var obj_CustomBase = new Object();
var Groupfiltertype = "";
var tripsGroups = new Array();
var Grouptype = "";
var Stat_PositiveValue = "";
var Stat_NegativeValue = "";
var isPopupVisible = false;

var IsWherePurchased = false;
var ChartType = "";
var ChartTileName = "";
var TabType = "";
var Tab_Index_Id = "";
var Tab_Id_mapping = false;
var left_scroll_bgcolor = "white";
var Selected_StatTest = "";
var Sigtype_Id = "0";
var CustomBase = [];
var TimePeriod_UniqueId = "";
var TimePeriodFrom_UniqueId = "";
var TimePeriodTo_UniqueId = "";
var sCurrent_PreviousTime = "";
var ShopperSegment_UniqueId = "";
var ShopperFrequency_UniqueId = "";
var ChartType_UniqueId = "";
var ChartXValues_UniqueId = "";
var ChartModuleData = "";
var ModuleBlock = "";
var hideAdvanceFilterType = "";
var CheckTimePeriodChanged = "";
var month = ['Aug 2013', 'Sep 2013', 'Oct 2013', 'Nov 2013', 'Dec 2013', 'Jan 2014', 'Feb 2014', 'Mar 2014', 'Apr 2014', 'May 2014', 'Jun 2014', 'Jul 2014', 'Aug 2014', 'Sep 2014', 'Oct 2014', 'Nov 2014', 'Dec 2014', 'Jan 2015', 'Feb 2015', 'Mar 2015', 'Apr 2015', 'May 2015', 'Jun 2015', 'Jul 2015', 'Aug 2015', 'Sep 2015', 'Oct 2015', 'Nov 2015', 'Dec 2015', 'Jan 2016', 'Feb 2016', 'Mar 2016', 'Apr 2016', 'May 2016', 'Jun 2016', 'Jul 2016', 'Aug 2016', 'Sep 2016'];
var quarter = ['Q4 2013', 'Q1 2014', 'Q2 2014', 'Q3 2014', 'Q4 2014', 'Q1 2015', 'Q2 2015', 'Q3 2015', 'Q4 2015', 'Q1 2016', 'Q2 2016', 'Q3 2016'];
var mmt = ['Oct 2013 3MMT', 'Nov 2013 3MMT', 'Dec 2013 3MMT', 'Jan 2014 3MMT', 'Feb 2014 3MMT', 'Mar 2014 3MMT', 'Apr 2014 3MMT', 'May 2014 3MMT', 'Jun 2014 3MMT', 'Jul 2014 3MMT', 'Aug 2014 3MMT', 'Sep 2014 3MMT', 'Oct 2014 3MMT', 'Nov 2014 3MMT', 'Dec 2014 3MMT', 'Jan 2015 3MMT', 'Feb 2015 3MMT', 'Mar 2015 3MMT', 'Apr 2015 3MMT', 'May 2015 3MMT', 'Jun 2015 3MMT', 'Jul 2015 3MMT', 'Aug 2015 3MMT', 'Sep 2015 3MMT', 'Oct 2015 3MMT', 'Nov 2015 3MMT', 'Dec 2015 3MMT', 'Jan 2016 3MMT', 'Feb 2016 3MMT', 'Mar 2016 3MMT', 'Apr 2016 3MMT', 'May 2016 3MMT', 'Jun 2016 3MMT', 'Jul 2016 3MMT', 'Aug 2016 3MMT', 'Sep 2016 3MMT'];
var year = ['2014', '2015'];
var MMT_12 = ['Jul 2014 12MMT', 'Aug 2014 12MMT', 'Sep 2014 12MMT', 'Oct 2014 12MMT', 'Nov 2014 12MMT', 'Dec 2014 12MMT', 'Jan 2015 12MMT', 'Feb 2015 12MMT', 'Mar 2015 12MMT', 'Apr 2015 12MMT', 'May 2015 12MMT', 'Jun 2015 12MMT', 'Jul 2015 12MMT', 'Aug 2015 12MMT', 'Sep 2015 12MMT', 'Oct 2015 12MMT', 'Nov 2015 12MMT', 'Dec 2015 12MMT', 'Jan 2016 12MMT', 'Feb 2016 12MMT', 'Mar 2016 12MMT', 'Apr 2016 12MMT', 'May 2016 12MMT', 'Jun 2016 12MMT', 'Jul 2016 12MMT', 'Aug 2016 12MMT', 'Sep 2016 12MMT'];
var YTD = ['YTD Jan 2014', 'YTD Feb 2014', 'YTD Mar 2014', 'YTD Apr 2014', 'YTD May 2014', 'YTD Jun 2014', 'YTD Jul 2014', 'YTD Aug 2014', 'YTD Sep 2014', 'YTD Oct 2014', 'YTD Nov 2014', 'YTD Dec 2014', 'YTD Jan 2015', 'YTD Feb 2015', 'YTD Mar 2015', 'YTD Apr 2015', 'YTD May 2015', 'YTD Jun 2015', 'YTD Jul 2015', 'YTD Aug 2015', 'YTD Sep 2015', 'YTD Oct 2015', 'YTD Nov 2015', 'YTD Dec 2015', 'YTD Jan 2016', 'YTD Feb 2016', 'YTD Mar 2016', 'YTD Apr 2016', 'YTD May 2016', 'YTD Jun 2016', 'YTD Jul 2016', 'YTD Aug 2016', 'YTD Sep 2016'];//, 'YTD Apr 2015', 'YTD May 2015', 'YTD Jun 2015', 'YTD Jul 2015', 'YTD Aug 2015', 'YTD Sep 2015', 'YTD Oct 2015', 'YTD Nov 2015', 'YTD Dec 2015'
var MMT_6 = ['Jan 2014 6MMT', 'Feb 2014 6MMT', 'Mar 2014 6MMT', 'Apr 2014 6MMT', 'May 2014 6MMT', 'Jun 2014 6MMT', 'Jul 2014 6MMT', 'Aug 2014 6MMT', 'Sep 2014 6MMT', 'Oct 2014 6MMT', 'Nov 2014 6MMT', 'Dec 2014 6MMT', 'Jan 2015 6MMT', 'Feb 2015 6MMT', 'Mar 2015 6MMT', 'Apr 2015 6MMT', 'May 2015 6MMT', 'Jun 2015 6MMT', 'Jul 2015 6MMT', 'Aug 2015 6MMT', 'Sep 2015 6MMT', 'Oct 2015 6MMT', 'Nov 2015 6MMT', 'Dec 2015 6MMT', 'Jan 2016 6MMT', 'Feb 2016 6MMT', 'Mar 2016 6MMT', 'Apr 2016 6MMT', 'May 2016 6MMT', 'Jun 2016 6MMT', 'Jul 2016 6MMT', 'Aug 2016 6MMT', 'Sep 2016 6MMT'];

var compFreqViews = ["hdn-tbl-compareretailers", "hdn-tbl-retailerdeepdive", "hdn-chart-compareretailers", "hdn-chart-retailerdeepdive", "hdn-report-compareretailersshoppers", "hdn-report-retailersshopperdeepdive", "hdn-report-compareretailerspathtopurchase", "hdn-report-retailerspathtopurchasedeepdive", "hdn-dashboard-pathtopurchase", "hdn-dashboard-demographic"];

var DefaultGeolist = [];
var label = [];
var AllRetailers = [];
var AllPriorityRetailers = [];
var AllBeverages = [];
var AllNonBeverages = [];
var AllTypes = [];
var AllMeasures = [];
var ShopperMeasureSearchItems = [];
var TripsMeasureSearchItems = [];

var AllComparisonBanners = [];
var AllTimePeriods = [];
var AllDemographics = [];
var AllAdvancedFilterLeft = [];
var AllGeographics = [];
var AllFrequency = [];
var AllSubFrequency = [];
var AllTotalMeasures = [];
var ShopperTotalMeasures = [];
var TripsTotalMeasures = [];

var TripTotalMeasures = [];
var AllLeftPanelFrequency = [];
var AllAdvancedFilters = [];
var AllGeography = [];
var SelectedDempgraphicList = [];
var SelectedDempgraphicGeoList = [];
var SelectedFrequencyList = [];
var SelectedTotalMeasure = [];
var SelectedAdvFilterList = [];
var allChannels = [];
var selectedChannels = [];
var AllMonthly = [];
var Channels_DBNames = [];
var Channels_UniqueId = [];
var Metric_UniqueId = "";
var SelectedAdvancedAnalyticsList = [];
//---->Time Period variables <---//
var TimeExtension = "12MMT";
var TimePeriod = "12MMT";
var TimePeriodId = 0;
var TimePeriodName = "";
var TimePeriodShortName = "";
var CurrentTimePeriodlist = [];
//For Trend -------->
var TimePeriod_From = "";
var TimePeriod_To = "";
var TimePeriod_ShortNames = [];
var TimePeriod_Unique_ShortNames = [];
//------->

//Deep dive variables
var Grouplist = [];
var GroupGeolist = [];
var GroupId = 0;
var removeGroupStatus = 1;

var Measurelist = [];
var MeasureId = 0;

var Geographylist = [];
var GeographyId = 0;

//---->Comparison variables <---//
var Comparisonlist = [];
var ComparisonBevlist = [];
var CompCurrentId
var CompCurrentName
//------->

//---->Competitor variables <---//
var CompetitorFrequency = [];
var CompetitorCustomBaseFrequency = [];
var CompetitorRetailer = [];
var CompetitorCustomBaseRetailer = [];
var isCompetitorCustomBase = false;
//------->

var sFilterData = {};
var sGeographyData = [];
//var currentpage = "";
var dGeo = [];
var sflag = 0;

var checksamplesize_spname = "";
var main_spname = "";
var checksamplesize_spname = "";
var tabname = "";
var sVisitsOrGuests = "1";
var sRemovedLegendPosition = [];
//DB mapping Names
var Advanced_Filters_DBNames = [];
var Advanced_Filters_ShortNames = [];
var Advanced_Filters_UniqueId = [];

var CustomBaseAdvancedFilters = [];
var CustomBaseAdvancedFilters_UniqueId = [];

var CustomBaseFrequencyFilters = [];
var CustomBaseFrequency_UniqueId = [];

var identifier = 0;
//Export Excel
var ExportToExcel = "false";

var sSelectedDropDownBrand = "";
var PreviousTimePeriod = "";

var sBevarageSelctionType = [];
var isSearch = "0";
var CorrespondaceMapsLowSampleSizeVariables = "";
var sRetailer = "1";
var sTrendType = "1";
var leftpositionval = "0";
//var myApp = angular.module('myApp', []);
var P2P_Sort = 1;
var custombase_AddFilters = [];
var custombase_Canceled_AddFilters = [];
var custombase_Frequency = [];
//Reset filters

//For SAR Report
let selectedCustomBaseOrBenchMark = ""
let sarRetailerCustomBaseOrBenchMarkClicked = false
let sarRetailerBenchmarkList = []
let sarRetailerCustomList = []
let sarCompetitorList = []
let sarFrequencyList = {}
let isVisitSar = true
let isChannelSelected = false
let isColPaletCalledDirectly = false
let totaltripsList = ["Who is my Core shopper?", "path to purchase and trip details", "Beverege section"]
let sarDefaultFrequencyList = {
    "Who is my Core shopper?": "Total Trips",
    "path to purchase and trip details": "Total Trips",
    "Strength and oppurtunities": "Store in trade area",
    "Beverege section": "Total Trips",
}
var isNonPrioritySelected = false
var corporateOrChannelNetSelected = false

//Color Palette
var ctrlUp = false, intForContinuous;
var defaultchartColors = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#cccccc", "#b42c14", "#643160", "#be6e14", "#406462", "#605f4f", "#a3978b", "#c08617", "#9d270e", "#170909", "#368130", "#378574"];
//END
//End SAR Report
function ResetFilters() {
    SelectedDempgraphicList = [];
    SelectedAdvFilterList = [];
    Comparisonlist = [];
    SelectedFrequencyList = [];
    ComparisonBevlist = [];
    TabType = "";
    Grouplist = [];
    $("#groupDivId *").removeClass("Selected");
    $(".rgt-cntrl-SubContainer *").removeClass("Selected");
    $(".rgt-cntrl-frequency *").removeClass("Selected");
    HideAdvFilterOnGroupSelect();
    $("#RightPanelPartial").hide();
    $(".adv-fltr-details").hide();

    $(".Retailers .RetailerDiv .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
    $(".Retailers .RetailerDiv .Lavel li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(".AdvancedFilters .lft-popup-ele").removeClass("Selected").addClass("Not-Selected-Channel");
    $(".AdvancedFilters .lft-popup-ele").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(".GroupType .lft-popup-ele").removeClass("Selected").addClass("Not-Selected-Channel");
    $(".GroupType .lft-popup-ele").find(".ArrowContainerdiv").css("background-color", "#58554D");

    $(".Beverages .BevDiv .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
    $(".Beverages .BevDiv .Lavel li").find(".ArrowContainerdiv").css("background-color", "#58554D");

    $(".adv-fltr-top").hide();
    $(".adv-fltr-label").removeClass("adv-fltr-label-demo");
    $(".adv-fltr-label").removeClass("adv-fltr-label-visits");
    $(".adv-fltr-label").removeClass("adv-fltr-label-guests");
    ShowSelectedFilters();

    if ($("#guest-visit-toggle").is(":checked") == true)
        $("#guest-visit-toggle").trigger("click");
    //$(".adv-fltr-details").css("margin-top", "2.5%");

    //if ($(".adv-fltr-showhide-txt").text() == "SHOW LESS")
    //    $(".adv-fltr-showhide").trigger("click");
    clearOutScr();
    if (currentpage == "hdn-tbl-compareretailers") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    else if (currentpage == "hdn-tbl-retailerdeepdive" || currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-tbl-BeverageDeepDive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-chart-comparebeverages") {

    }
    else if (currentpage == "hdn-chart-compareretailers") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    else if (currentpage == "hdn-chart-retailerdeepdive" || currentpage == "hdn-e-commerce-chart-sitedeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    else if (currentpage == "hdn-analysis-establishmentdeepdive") {
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    SetStatTesting(currentpage);
}
function UpdateDeepDive() {
    clearOutScr();
    ResetFilters();
    if (sVisitsOrGuests == 1) {
        TabType = "trips";
    }
    else if (sVisitsOrGuests == 2) {
        TabType = "shopper";
    }
    if (ModuleBlock == "TREND") {
        Grouplist = [];
        $("#groupDivId *").removeClass("Selected");
        if (currentpage.indexOf("hdn-tbl") > -1) {
            TabType = "trips";
        }
        if (TabType != "")
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
        $("#GroupType").hide();
        $("#trend-tabs-block").show();
        LoadTimePeriod(filters);
        $("#TimeBlock ul li[name='TOTAL TIME']").hide();
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //            
        $("#trend-tabs-block .demo .adv-fltr-label").addClass("adv-fltr-label-demo");
        main_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("visit-spname");
        checksamplesize_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("visit-samplesize-spname");
        tabname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").html();
        //$("#trend-tabs-block .demo .adv-fltr-label").trigger("click");
        if (currentpage.indexOf("hdn-tbl") > -1) {
            ActivateTableDefaultTab($("#trend-tabs-block .demo .adv-fltr-label"));
        }
        else if (currentpage == "hdn-report-retailersshopperdeepdive") {
            TabType = "shopper";
            SelectedFrequencyList = [];
            //$(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
            //$(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");

            $(".Left-Frequency ul li[parentname='MONTHLY +'][name='SELECTION']").trigger("click");
        }
        else if (currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
            SelectedFrequencyList = [];
            //$(".Left-Frequency ul li[name='TOTAL VISITS']").removeClass("Selected");
            //$(".Left-Frequency ul li[name='TOTAL VISITS']").trigger("click");
            $("#AdvFilterDivId ul li[name='TOTAL VISITS']").trigger("click");
        }
    }
    else {
        if (sVisitsOrGuests == "2") {
            if (currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-compareretailersshoppers") {
                $("#RightPanelPartial #frequency_containerId ul li[parentname='MONTHLY +'][name='SELECTION']").trigger("click");
            }
            else
                $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
        }
        else if ((currentpage.indexOf("hdn-tbl") > -1)) {
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
        }
        else if (currentpage == "hdn-analysis-withintrips") {
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
        }
        else if ((currentpage.indexOf("hdn-chart") > -1) && TabType != "") {
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
        }
        else if (currentpage.indexOf("hdn-report") > -1 && currentpage.indexOf("purchase") == -1) {
            if (currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-compareretailersshoppers") {
                $(".Left-Frequency ul li[parentname='MONTHLY +'][name='SELECTION']").removeClass("Selected");
                $(".Left-Frequency ul li[parentname='MONTHLY +'][name='SELECTION']").trigger("click");
            }
            else {
                $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
                $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
            }
        }

        $("#GroupType").show();
        $("#pit-tabs-block").show();
        $("#TimeBlock ul li[name='TOTAL TIME']").show();
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //
        $("#pit-tabs-block .demo .adv-fltr-label").addClass("adv-fltr-label-demo");
        main_spname = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("visit-spname");
        checksamplesize_spname = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("visit-samplesize-spname");
        tabname = $("#pit-tabs-block .demo .adv-fltr-label").children("span").html();
        if (currentpage.indexOf("hdn-tbl") > -1) {
            ActivateTableDefaultTab($("#pit-tabs-block .demo .adv-fltr-label"));
        }
        else if (currentpage == "hdn-report-retailersshopperdeepdive") {
            SelectedFrequencyList = [];
            //$(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
            //$(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");

            $(".Left-Frequency ul li[parentname='MONTHLY +'][name='SELECTION']").trigger("click");
        }
        else if (currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
            SelectedFrequencyList = [];
            //$(".Left-Frequency ul li[name='TOTAL VISITS']").removeClass("Selected");
            //$(".Left-Frequency ul li[name='TOTAL VISITS']").trigger("click");
            $("#AdvFilterDivId ul li[name='TOTAL VISITS']").trigger("click");
        }
        //$("#pit-tabs-block .demo .adv-fltr-label").trigger("click");

    }
    if (currentpage.indexOf("chart") > -1 && currentpage.indexOf("deepdive") > -1) {
        $(".shoppertrip-Toggle").hide();
    }
    ResetMeasures();
    ShowSelectedFilters();
}
function ResetMeasures() {
    var measureId = "#retailer-measure div[level-id='1']";
    var measureOnclick = "DisplayMeasureList(this);";
    if (currentpage.indexOf("hdn-crossretailer-totalrespondentstripsreport") > -1) {
        measureId = "#total-measure-trip";
        measureOnclick = "DisplaySecondaryTotalFilter(this);";
    }
    else if (currentpage.indexOf("hdn-analysis-withintrips") > -1) {
        measureId = "#CorrespondenceMeasureDivId";
        measureOnclick = "DisplaySecondaryAdvancedAnalyticsTrips(this);";
    }

    $(measureId + " ul li[filtertype='Shopper']").css("cursor", "pointer");
    $(measureId + " ul li[filtertype='Shopper']").css("background-color", "");
    $(measureId + " ul li[filtertype='Shopper'] div").not(".measure-inactive").css("background-color", "");

    $(measureId + " ul li[filtertype='Visits']").css("cursor", "pointer");
    $(measureId + " ul li[filtertype='Visits']").css("background-color", "");
    $(measureId + " ul li[filtertype='Visits'] div").not(".measure-inactive").css("background-color", "");

    $(measureId + " ul li[filtertype='Shopper'] .measure-inactive").addClass("ArrowContainerdiv");
    $(measureId + " ul li[filtertype='Visits'] .measure-inactive").addClass("ArrowContainerdiv");

    if ($(measureId + " ul li[filtertype='Shopper']").eq(0).hasClass("main-measure"))
        $(measureId + " ul li[filtertype='Shopper']").attr("onclick", measureOnclick);
    else if ($(measureId + " ul li[filtertype='Shopper'] .main-measure").eq(0).hasClass("main-measure"))
        $(measureId + " ul li[filtertype='Shopper'] .main-measure").attr("onclick", measureOnclick);

    if ($(measureId + " ul li[filtertype='Visits']").eq(0).hasClass("main-measure"))
        $(measureId + " ul li[filtertype='Visits']").attr("onclick", measureOnclick);
    else if ($(measureId + " ul li[filtertype='Shopper'] .main-measure").eq(0).hasClass("main-measure"))
        $(measureId + " ul li[filtertype='Visits'] .main-measure").attr("onclick", measureOnclick);

    //enable Demographics
    $(measureId + " ul li[filtertype='Demographics']").css("cursor", "pointer");
    $(measureId + " ul li[filtertype='Demographics']").css("background-color", "");
    $(measureId + " ul li[filtertype='Demographics'] div").not(".measure-inactive").css("background-color", "");
    $(measureId + " ul li[filtertype='Demographics'] .measure-inactive").addClass("ArrowContainerdiv");

    if ($(measureId + " ul li[filtertype='Demographics']").eq(0).hasClass("main-measure"))
        $(measureId + " ul li[filtertype='Demographics']").attr("onclick", measureOnclick);
    else if ($(measureId + " ul li[filtertype='Shopper'] .main-measure").eq(0).hasClass("main-measure"))
        $(measureId + " ul li[filtertype='Demographics'] .main-measure").attr("onclick", measureOnclick);

    Measurelist = [];
    SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
    SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", AllTotalMeasures);
}
function UpdateVisitGuestFilters() {
    if (currentpage.indexOf("chart") > -1) {
        var sChange = "";
        if ($("#guest-visit-toggle").is(":checked") == false) {
            TabType = "trips";
            if (sVisitsOrGuests == 1)
                sChange = "false";
            else
                sChange = "true";
            sVisitsOrGuests = 1;
            sBevarageSelctionType = [];
        }
        else if ($("#guest-visit-toggle").is(":checked") == true) {
            TabType = "shopper";
            if (sVisitsOrGuests == 2)
                sChange = "false";
            else
                sChange = "true";
            sVisitsOrGuests = 2;
            if (currentpage.indexOf("chart") > -1) {
                $("#adv-bevselectiontype-freq").hide();
                sBevarageSelctionType = [];
            }
            else
                $("#beverage-frequency ul div[uniqueid='1']").trigger("click");
        }
        if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1))
            $("#adv-bevselectiontype-freq").show();
        else {
            $("#adv-bevselectiontype-freq").hide();
            sBevarageSelctionType = [];
        }
        GetDefaultFrequency();
    }
    else {
        if ($("#guest-visit-toggle").is(":checked")) {
            SelectedAdvFilterList = [];
            $("#RightPanelPartial .lft-popup-ele").removeClass("Selected");
            $("#RightPanelPartial .lft-popup-ele").find(".ArrowContainerdiv").css("background-color", "#58554D");
            if (currentpage.indexOf("beverage") > -1) {
                selectedChannels = [];
                $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
            }
            else {
                $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
            }

            if (tabname.toLocaleLowerCase() == "demographics" && ModuleBlock == "TREND") {
                main_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("guest-spname");
                checksamplesize_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("guest-samplesize-spname");
                TabType = "shopper";
            }
            else if (tabname.toLocaleLowerCase() == "demographics" && ModuleBlock == "PIT") {
                main_spname = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("guest-spname");
                checksamplesize_spname = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("guest-samplesize-spname");
                Tab_Id_mapping = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("guest-id-mapping");
                Tab_Index_Id = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("tabindex");
                TabType = "shopper";
            }
            else if (tabname.toLocaleLowerCase() == "demographics") {
                main_spname = $(".demo .adv-fltr-label").children("span").attr("guest-spname");
                checksamplesize_spname = $(".demo .adv-fltr-label").children("span").attr("guest-samplesize-spname");
                Tab_Id_mapping = $(".demo .adv-fltr-label").children("span").attr("guest-id-mapping");
                Tab_Index_Id = $(".demo .adv-fltr-label").children("span").attr("tabindex");
                TabType = "shopper";
            }
        }
        else {
            SelectedFrequencyList = [];
            if (currentpage.indexOf("beverage") > -1) {
                $("#RightPanelPartial #channel-content ul li[name='TOTAL']").trigger("click");
            }
            else {
                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
            }

            if (tabname.toLocaleLowerCase() == "demographics" && ModuleBlock == "TREND") {
                main_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("visit-spname");
                checksamplesize_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("visit-samplesize-spname");
                TabType = "trips";
            }
            else if (tabname.toLocaleLowerCase() == "demographics" && ModuleBlock == "PIT") {
                main_spname = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("visit-spname");
                checksamplesize_spname = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("visit-samplesize-spname");
                Tab_Id_mapping = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("visit-id-mapping");
                Tab_Index_Id = $("#pit-tabs-block .demo .adv-fltr-label").children("span").attr("tabindex");
                TabType = "trips";
            }
            else if (tabname.toLocaleLowerCase() == "demographics") {
                main_spname = $(".demo .adv-fltr-label").children("span").attr("visit-spname");
                checksamplesize_spname = $(".demo .adv-fltr-label").children("span").attr("visit-samplesize-spname");
                Tab_Id_mapping = $(".demo .adv-fltr-label").children("span").attr("visit-id-mapping");
                Tab_Index_Id = $(".demo .adv-fltr-label").children("span").attr("tabindex");
                TabType = "trips";
            }
        }
    }
    HideOrShowFilters();
    $("#adv-bevselectiontype-freq").hide();
    ShowSelectedFilters();
}
function ClearChartReports() {
    $("#btnAddToExport").attr('chart-type', 'inactive');
    $("#btnViewSelections").attr('chart-type', 'inactive');
    $("#btnClearAll").attr('chart-type', 'inactive');

    $("#btnAddToExport").css("background", "lightgray");
    $("#btnViewSelections").css("background", "lightgray");
    $("#btnClearAll").css("background", "lightgray");
}

function RetailnChartReports() {
    $("#btnAddToExport").attr('chart-type', 'active');
    $("#btnAddToExport").css("background", "");
    if (CheckChartReports()) {
        $("#btnViewSelections").attr('chart-type', 'active');
        $("#btnClearAll").attr('chart-type', 'active');
        $("#btnViewSelections").attr('chart-type', 'active');

        $("#btnViewSelections").css("background", "");
        $("#btnClearAll").css("background", "");
    }
    else {
        $("#btnViewSelections").attr('chart-type', 'inactive');
        $("#btnClearAll").attr('chart-type', 'inactive');
        $("#btnViewSelections").css("background", "#BEBEBE");
        $("#btnViewSelections").css("background", "lightgray");
        $("#btnClearAll").css("background-color", "lightgray");
    }
}
$(window).on('load', function () {
    //Color Pallet
    if ($(".jscolor")[0] != undefined || $(".jscolor")[0] != null) {
        $(".jscolor")[0].jscolor.show();
        //Clear the Input val
        $(".jscolor").val('');
        //Update the Color
        recalibrateColorfronInput($('.redVal>input'), $('.greenVal>input'), $('.blueVal>input'));
    }
});
$(document).ready(function (e) {
    //Set page resolution
    //width = $(window).width();
    //height = $(window).height();

    width = window.innerWidth;
    height = window.innerHeight;
    if (width < 1024)
        width = 1024;
    if (height < 520)
        height = 520;
    // 
    $(window).resize(function () {
        if (screen.width == window.innerWidth) {
            //$('.priorityclass').text('-----------------------PRIORITY-----------------------')
        } else if (screen.width > window.innerWidth) {
            //$('.priorityclass').text('--------------------PRIORITY--------------------')
        } else {
            var zoominpercent = Math.round((screen.width / window.innerWidth) * 100);
            if (zoominpercent <= 90 && zoominpercent > 80) {
                //$('.priorityclass').text('---------------------PRIORITY--------------------')
            }

            else if (zoominpercent <= 80 && zoominpercent > 75) {
                //$('.priorityclass').text('------------------------PRIORITY-----------------------')
            }

            else if (zoominpercent <= 75 && zoominpercent > 67) {
                //$('.priorityclass').text('-----------------------PRIORITY---------------------')
            }
            else if (zoominpercent <= 67) {
                //$('.priorityclass').text('--------------------PRIORITY---------------------')
            }
        }

        if ($(selectedModule).html() != undefined && $(selectedModule).html().trim().toLocaleLowerCase() != "reports" && $(selectedModule).html().trim().toLocaleLowerCase() != "add’l capabilities") {
            $(selectedModule).trigger("click");
        }
        if (($("#MeasureTypeHeaderContentSubLevelTrip").is(':visible') || $("#MeasureTypeHeaderContentSubLevelShopper").is(':visible')) && ($("#MeasureTypeContentTrip").is(':visible') || $("#MeasureTypeContentShopper").is(':visible'))) {
            //$(".MeasureScrollDiv").css("width", "110%");
            //$(".MeasureType").css("width", "95%");
            $(".MeasureType").css("width", "auto");
            // $(".Lavel").css("width", "20%");
        }
        else {
            $(".MeasureType").css("width", "auto");
            //$(".Lavel").css("width", "262px");
        }
        if ($(".MeasureType").width() > window.innerWidth)
            $(".MeasureType").css("width", "95%");
        else
            $(".MeasureType").css("width", "auto");
        //$(".MeasureType").getNiceScroll().remove();

        if ($(".BevScrollDiv").width() > window.innerWidth) {
            //$(".Beverages").css("width", "95%");
        }
        else
            $(".Beverages").css("width", "auto");
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

        if ($(".MeasureType").width() > window.innerWidth)
            $(".MeasureType").css("width", "95%");
        else
            $(".MeasureType").css("width", "auto");
        //GroupContentScroll
        if (typeof SetStyles === 'function') {
            SetStyles();
        }
        //var leftPos = $('#BevContainerDivId').scrollLeft() + 200;
        //$('#BevContainerDivId').scrollLeft(leftPos);

        //Color Pallet
        if ($(".jscolor")[0] != undefined || $(".jscolor")[0] != null) {
            $(".jscolor")[0].jscolor.show();
            //Clear the Input val
            $(".jscolor").val('');
            //Update the Color
            recalibrateColorfronInput($('.redVal>input'), $('.greenVal>input'), $('.blueVal>input'));
        }
    });


    $("html").css("min-width", width + "px").css("min-height", height + "px");
    $("body").css("min-width", width + "px").css("min-height", height + "px");

    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }

    $(".arrow_popup").click(function (e) {
        ClosePopups();
        if ($(".arrw").hasClass("arrw_dwn")) {
            $(".arrw").removeClass("arrw_dwn");
            $(".arrw").addClass("uparrw");
            $(this).addClass("active_arrow");
            $("#FilterHeader").css("height", "auto");
            $("#SelectedFilters").css("height", "auto");
            $("#scrollableselection").css("height", "auto");
            $("#Translucent").show();
            e.stopImmediatePropagation();
        }
        else {
            $(".arrw").removeClass("uparrw");
            $(".arrw").addClass("arrw_dwn");
            $(this).removeClass("active_arrow");
            $("#FilterHeader").css("height", "21px");
            $("#SelectedFilters").css("height", "21px");
            $("#scrollableselection").css("height", "21px");
            $("#Translucent").hide();
        }
    });
    GetStatTestValue();
    $(document).on("click", ".remove-item", function (e) {
        if (!$(".Custombase-GroupType").is(':visible'))
            ClosePopups();
        e.stopImmediatePropagation();
    });
    $(document).on("click", ".rgt-cntrl-frequency", function (e) {
        e.stopImmediatePropagation();
    });
    $(document).on("click", ".adv-fltr-applyfiltr", function () {
        if (currentpage == "hdn-analysis-withintrips" || currentpage.indexOf("tbl") > -1) {
            $(".advance-filters").css("display", "block");
            $(".adv-fltr-details").css("margin-top", "0%");
        }
        //$(".adv-fltr-showhide").trigger("click");
        prepareContentArea();
    });
    $("#Stat-Test").click(function () {
        GetStatTestValue();
        $("#Translucent").show();
        $(".StatArea").show();
    });
    $(".StatTestValue").click(function () {
        $(".StatTestValue").removeClass("selected-stattest");
        $(this).addClass("selected-stattest");
    });
    $(".stattest-sign").hover(function () {
        $(".ShowStatDetails").show();
        $(".Beverages").css("width", "auto");
        $(".BevScrollDiv").css("width", "auto");

        if ($(".BevScrollDiv").width() > window.innerWidth) {
            //$(".Beverages").css("width", "95%");
        }
        else
            $(".Beverages").css("width", "auto");
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if ($(".BevScrollDiv")[0].style.width == "auto")
            $(".Beverages").getNiceScroll().remove();
        else
            SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    });
    $(".stattest-sign").mouseleave(function () {
        $(".ShowStatDetails").hide();
        $(".Beverages").css("width", "auto");
        $(".BevScrollDiv").css("width", "auto");

        if ($(".BevScrollDiv").width() > window.innerWidth) {
            //$(".Beverages").css("width", "95%");
        }
        else
            $(".Beverages").css("width", "auto");
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if ($(".BevScrollDiv")[0].style.width == "auto")
            $(".Beverages").getNiceScroll().remove();
        else
            SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    });
    $(document).on("click", ".leftbody .table-title, .rightbody .table-title", function () {
        var indx = $(this).index() + 1;
        if ($(".leftbody .rowitem").eq(indx - 1).find(".treeview").hasClass("minusIcon")) {
            $(".leftbody .rowitem").eq(indx - 1).find(".treeview").removeClass("minusIcon");
            $(".leftbody .rowitem").eq(indx - 1).find(".treeview").removeClass("plusIcon");
            $(".leftbody .rowitem").eq(indx - 1).find(".treeview").addClass("plusIcon");
        }
        else {
            $(".leftbody .rowitem").eq(indx - 1).find(".treeview").removeClass("minusIcon");
            $(".leftbody .rowitem").eq(indx - 1).find(".treeview").removeClass("plusIcon");
            $(".leftbody .rowitem").eq(indx - 1).find(".treeview").addClass("minusIcon");
        }
        for (var i = indx; i < $(".leftbody .rowitem").length; i++) {
            if (currentpage == "hdn-analysis-crossretailerimageries") {
                if ($(".leftbody .rowitem").eq(i).hasClass("table-title")) {
                    SetScroll($("#GetCRPData .rightbody"), "#393939", 0, -8, 0, -8, 8);
                    return false;
                    break;
                }
                if ($(".leftbody .rowitem").eq(i).is(':visible')) {
                    $(".leftbody .rowitem").eq(i).hide();
                    //SetScroll($("#GetCRPData .rightbody"), "#393939", 0, -8, 0, -8, 8);
                    $(".rightbody .rowitem").eq(i).hide();
                    //SetScroll($("#GetCRPData .rightbody"), "#393939", 0, -8, 0, -8, 8);
                }
                else {
                    $(".leftbody .rowitem").eq(i).show();
                    //SetScroll($("#GetCRPData .rightbody"), "#393939", 0, -8, 0, -8, 8);
                    $(".rightbody .rowitem").eq(i).show();
                    //SetScroll($("#GetCRPData .rightbody"), "#393939", 0, -8, 0, -8, 8);
                }
            }
            else {
                if ($(".leftbody .rowitem").eq(i).hasClass("table-title")) {
                    SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
                    return false;
                    break;
                }
                if ($(".leftbody .rowitem").eq(i).is(':visible')) {
                    $(".leftbody .rowitem").eq(i).hide();
                    //SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
                    $(".rightbody .rowitem").eq(i).hide();
                    //SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
                }
                else {
                    $(".leftbody .rowitem").eq(i).show();
                    //SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
                    $(".rightbody .rowitem").eq(i).show();
                    //SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
                }
            }
        }
    });
    $("#pathtopurchase-size-skew-toggleTrip").click(function () {
        // if ($("#default-tabtype").val() == "2") {
        if ($(this).is(":checked")) {
            //P2P_Sort = 0;
            sVisitsOrGuests = "2";
            TabType = "Shopper";
            $("#lft-fltr-trendDashShopper").addClass("Active");
            $("#lft-fltr-pitDashTrip").removeClass("Active");
            //$("#AdvFilterDivId .level2 ul").find('li[parentname="ADDITIONAL FILTERS"]').hide();
            SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllSubFrequencyDemo.splice(4)));
        }
        else {
            //P2P_Sort = 1;
            sVisitsOrGuests = "1";
            TabType = "Trips";
            $("#lft-fltr-pitDashTrip").addClass("Active");
            $("#lft-fltr-trendDashShopper").removeClass("Active");
            //$("#AdvFilterDivId .level2 ul").find('li[parentname="ADDITIONAL FILTERS"]').show();
            SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllAdvancedFilterLeft).concat(AllSubFrequencyDemo));
        }
        //}
        //LoadAdvancedFilterFromString(sFilterData);
        $("#groupDivId li[filtertype=FREQUENCY]").show();
        if (DemogFlag == 0) {
            SelectedFrequencyList = [];
            SelectedDempgraphicList = [];
            custombase_Frequency = [];
            Grouplist = [];
            custombase_AddFilters = [];
            $(".FilterPopup.AdvancedFilters *").find(".Selected").removeClass("Selected");
            SetTripsDefaultFrequency();
        }
        else {
            LoadDashboardGroupTypeHeaderName(sFilterData);
            var _frequency = $("#default-shopper-frequency").val();
            $("#groupDivId span[parentname=FREQUENCY][uniqueid='" + _frequency + "']").parent("li").trigger("click");
        }
        ShowSelectedFilters();
        prepareContentArea();
        DemogFlag = 0;
    });
    $("#pathtopurchase-size-skew-toggle").click(function () {
        if ($(this).is(":checked")) {
            P2P_Sort = 0;
            $("#lft-fltr-trendDash").addClass("Active");
            $("#lft-fltr-pitDash").removeClass("Active");
        }
        else {
            P2P_Sort = 1;
            $("#lft-fltr-pitDash").addClass("Active");
            $("#lft-fltr-trendDash").removeClass("Active");
        }
        if (DemogFlag == 0) {
            //SelectedFrequencyList = [];
            SelectedDempgraphicList = [];
            ShowSelectedFilters();
            $(".FilterPopup.AdvancedFilters *").find(".Selected").removeClass("Selected");
            prepareContentArea();
        }
    });
    $("#pit-toggle").click(function (e) {
        //var timer = setTimeout(function () {
        //    document.getElementById('Translucent').style.display = "block";
        //}, 100);
        GetStatTestValue();
        ClearChartReports();
        Measurelist = [];
        TabType = "";
        if ($(this).is(":checked")) {
            ModuleBlock = "TREND";
            $(".trendText").hide();
            $("#lft-fltr-pit").removeClass("Active");
            $("#lft-fltr-trend").addClass("Active");
        }
        else {
            ModuleBlock = "PIT";
            $(".trendText").show();
            $("#lft-fltr-trend").removeClass("Active");
            $("#lft-fltr-pit").addClass("Active");
        }
        pit_trend_toggletype = ModuleBlock;
        LoadTimePeriod(filters);
        sVisitsOrGuests = "1";
        TabType = "trips";
        if ($('#guest-visit-toggle').hasClass('active')) {
            $('#guest-visit-toggle').removeClass('active');
            $(".adv-fltr-guest").css("color", "#000");
            $(".adv-fltr-visit").css("color", "#f00");
            //if (currentpage == "hdn-e-commerce-tbl-comparesites" || currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") {
            //    if ($(".adv-fltr-suboptions-list-container").is(":visible") && $(".adv-fltr-suboptions-list-container ul li:visible").length > 3) {
            //        $(".adv-filters-wraper").css("left", "1%");
            //    }
            //    else {
            //        $(".adv-filters-wraper").css("left", "27%");
            //    }
            //}
            //else if (sVisitsOrGuests == 1 && $(".adv-fltr-toggle-container.shoppertrip-Toggle").is(":visible") == false) {
            //    $(".adv-filters-wraper").css("left", "9%");
            //}
            //else {
            //    $(".adv-filters-wraper").css("left", "1%")
            //}
            sVisitsOrGuests = "1";
            TabType = "trips";
            //$("#adv-fltr-freq").css("display", "none");
            if (currentpage.indexOf("beverage") > 0) {
                $("#adv-fltr-Chnl").css("display", "block");
                $(".toggle-seperator").css("display", "none");
                $(".freq-seperator").css("display", "none");
                $(".advancedfilter-seperator").css("display", "block");
            }
        }
        //else {
        //    $('#guest-visit-toggle').addClass('active');
        //    $(".adv-fltr-visit").css("color", "#000");
        //    $(".adv-fltr-guest").css("color", "#f00");
        //    $(".adv-filters-wraper").css("left", "28%")

        //    sVisitsOrGuests = "2";
        //    $("#adv-fltr-freq").css("display", "block");
        //    if (currentpage.indexOf("beverage") > 0) {
        //        $("#adv-fltr-Chnl").css("display", "none");
        //    }
        //}

        UpdateDeepDive();
        //clearTimeout(timer);
        ShowSelectedFilters();
        if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
            $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("cursor", "pointer");
            $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("background-color", "");
            $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").attr("onclick", "DisplayTotalMeasures(this);");
            SelectedTotalMeasure = [];
            SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", AllTotalMeasures);
        }
        else if (currentpage == "hdn-chart-retailerdeepdive") {
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("cursor", "pointer");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("background-color", "");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").attr("onclick", "DisplayMeasureTripShopperList(this);");
            Measurelist = [];
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
        }
        FilterSelectionLimitText();
        $("#retailer-measure div[level-id='2'] ul li").removeClass('DNI');
        $("#AdvFilterDivId div[level-id='1'] ul li").show();
        e.stopImmediatePropagation();
    });
    $(document).on("click", "#guest-visit-toggle", function () {
        $(".rgt-cntrl-SubContainer *").removeClass("Selected");
        UpdateVisitGuestFilters();
        prepareContentArea();
    });
    if ($("#hdn-page").attr("name") == "hdn-tbl-RetailerDeepDive" || $("#hdn-page").attr("name") == "hdn-tbl-BeverageDeepDive") {
        $("#GroupType").hide();
        $("#MeasureType").hide();
        $("#trend-tabs-block").show();
        tabname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").html();
        main_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("guest-spname");
        checksamplesize_spname = $("#trend-tabs-block .demo .adv-fltr-label").children("span").attr("guest-samplesize-spname");
    }
    else if ($("#hdn-page").length > 0 && $("#hdn-page").attr("name").indexOf("chart") > 0) {
        $("#MeasureType").show();
    }

    $("#MenuHeader ul li span").click(function (e) {
        selectedModule = $(this);
        ClosePopups();
        $("#MenuHeader .bottom-line").hide();
        //HighLight sub page selection in dropdown
        $("#SubMenuHeader .SubItem .Item ul li").find(".Active").removeClass("Active");
        //_.forEach($(".SubItem .Item ul li a"), function (value, key) {
        //    if ((currentpage != "") && (currentpage != undefined))
        //    if ((value.href.toUpperCase().indexOf(currentpage.split("-")[2].toUpperCase()) > 0) && (value.href.toUpperCase().indexOf(currentpage.split("-")[1].toUpperCase() == "TBL" ? "TABLE" : currentpage.split("-")[1].toUpperCase()) > 0))
        //        value.className = "Active";
        //    else
        //        value.className = "";
        //});
        $("#SubMenuHeader .SubMenu ul li[link-name='" + currentpage + "']").children("a").addClass("Active");
        //End
        $("#MenuHeader ul li span").removeClass("Active");
        $("#MenuHeader ul li span").addClass("InActive");

        $(this).removeClass("InActive");
        $(this).addClass("Active");

        $(".dashboard-logo").css("background-position", "-482px -1px");
        $(".reports-logo").css("background-position", "-35px 0px");
        $(".tables-logo").css("background-position", "-119px 0px");
        $(".charts-logo").css("background-position", "-199px 0px");
        $(".analysis-logo").css("background-position", "-289px 0px");


        var path = $(this).parent("div").parent("li").attr("id");
        switch (path) {
            case "Dashboard": $(".dashboard-logo").css("background-position", "-437px -1px"); break;
            case "Reports": $(".reports-logo").css("background-position", "6px 0px"); break;
            case "Tables": $(".tables-logo").css("background-position", "-78px 0px"); break;
            case "Charts": $(".charts-logo").css("background-position", "-156px 0px"); break;
            case "Analysis": $(".analysis-logo").css("background-position", "-244px 0px"); break;
        }
        $("#SubMenuHeader .SubMenu").hide();

        $("#SubMenuHeader .SubMenu").css("left", "0").css("border-top", "0");
        if ($(this).html().toLocaleLowerCase() != "reports" && $(this).html().toLocaleLowerCase() != "add’l capabilities")

            var perwidth = 100 * ($(this).offset().left - ($("#SubMenuHeader div[parent-menu='" + $(this).parent("div").parent("li").attr("id") + "']").width() / 2) + 54 - 5) / $("body").width();
        $("#SubMenuHeader div[parent-menu='" + $(this).parent("div").parent("li").attr("id") + "']").css("margin-left", perwidth + "%").show();

        $("#SubMenuHeader div[parent-menu='" + $(this).parent("div").parent("li").attr("id") + "']").css("border-top", "3px solid #ea1f2a");
        //$(this).parent("div").find(".bottom-line").css("left", (($(this).width() / 2)));

        //$(this).parent("li").children(".bottom-line").css("top", ($("#MenuHeader").offset().top + $("#MenuHeader").innerHeight()) - 5 +6);
        $(this).parent("div").find(".bottom-line").show();
        $("#SubMenuHeader div[parent-menu='" + $(this).parent("div").parent("li").attr("id") + "']").show();
        e.stopImmediatePropagation();
    });
    $("#SubMenuHeader .SubMenu .SubItem").hover(function (e) {
        $("#SubMenuHeader .SubMenu .SubItem span").removeClass("Active");
        $("#SubMenuHeader .SubMenu .SubItem span").addClass("InActive");

        $("#SubMenuHeader .SubMenu .SubItem .Menu").removeClass("Menu-Active");

        $("#SubMenuHeader .SubMenu .SubItem .Menu .MenuTitle div").removeClass("downarrw_active");
        $("#SubMenuHeader .SubMenu .SubItem .Menu .MenuTitle div").addClass("downarrw");

        $(this).children(".Menu").children(".MenuTitle").children("span").removeClass("InActive");
        $(this).children(".Menu").children(".MenuTitle").children("span").addClass("Active");

        $(this).children(".Menu").addClass("Menu-Active");

        $(this).children(".Menu").children(".MenuTitle").children("div").removeClass("downarrw");
        $(this).children(".Menu").children(".MenuTitle").children("div").addClass("downarrw_active");
        $(this).children(".Item").show();
        $(".Beverages").css("width", "auto");
        $(".BevScrollDiv").css("width", "auto");

        if ($(".BevScrollDiv").width() > window.innerWidth) {
            //$(".Beverages").css("width", "95%");
        }
        else
            $(".Beverages").css("width", "auto");
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if ($(".BevScrollDiv")[0].style.width == "auto")
            $(".Beverages").getNiceScroll().remove();
        else
            SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    });
    $("#SubMenuHeader .SubMenu .SubItem").mouseleave(function (e) {
        $("#SubMenuHeader .SubMenu .SubItem .Item").hide();
        $("#SubMenuHeader .SubMenu .SubItem span").removeClass("Active");
        $("#SubMenuHeader .SubMenu .SubItem span").addClass("InActive");
        $("#SubMenuHeader .SubMenu .SubItem .Menu").removeClass("Menu-Active");
        $("#SubMenuHeader .SubMenu .SubItem .Menu .MenuTitle div").removeClass("downarrw_active");
        $("#SubMenuHeader .SubMenu .SubItem .Menu .MenuTitle div").addClass("downarrw");
        $(".Beverages").css("width", "auto");
        $(".BevScrollDiv").css("width", "auto");

        if ($(".BevScrollDiv").width() > window.innerWidth) {
            //$(".Beverages").css("width", "95%");
        }
        else
            $(".Beverages").css("width", "auto");
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if ($(".BevScrollDiv")[0].style.width == "auto")
            $(".Beverages").getNiceScroll().remove();
        else
            SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    });


    $(document).on("click", "#LeftPanel .FilterMenu", function (e) {
        if (currentpage == "hdn-crossretailer-sarreport") {
            if (sarRetailerBenchmarkList.length == 0) {
                if ($(this).attr("id") == "Competitors" || $(this).attr("id") == "Sar-Frequency" || $(this).attr("id") == "AdvancedFilters") {
                    showMessage("Please select 1 Main Retailer / Channel and 2 Comparision Retailer / Channel")
                    return false;
                }
            }
            else if (sarRetailerCustomList.length != 2) {
                if ($(this).attr("id") == "Competitors" || $(this).attr("id") == "Sar-Frequency" || $(this).attr("id") == "AdvancedFilters") {
                    showMessage("Please select 2 Comparision Retailer / Channel")
                    return false;
                }
            }
            else if (sarCompetitorList.length < 5) {
                if ($(this).attr("id") == "Sar-Frequency" || $(this).attr("id") == "AdvancedFilters") {
                    showMessage("Please select minimum 5 Competitors")
                    return false;
                }
            }
        }
        //ShowChannelRetailerVariables();     
        ClosePopups();
        if ($(this).attr("id") == "MeasureType" && (currentpage.indexOf("chart") > -1)) {
            $(".popup-menu").hide();
            $(".Sub-Lavel").hide();
            $("#MeasureTypeHeaderMainTrip").show();
            $("#MeasureTypeHeaderMainTrip ul li").show();
            $("#MeasureTypeHeaderContentTrip .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderContentShopper .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderContentSubLevelTrip .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderContentSubLevelShopper .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderContentTrip").hide();
            $("#MeasureTypeHeaderContentShopper").hide();
            $("#MeasureTypeHeaderContentSubLevelTrip").hide();
            $("#MeasureTypeHeaderContentSubLevelShopper").hide();
            $("#MeasuresHeadingLevel4").hide();
            $("#MeasureTypeContentTrip").hide();
            $("#MeasureTypeContentShopper").hide();
            $("#MeasuresHeadingLevel1").hide();
            $("#MeasuresHeadingLevel2").hide();
            $("#MeasuresHeadingLevel3").hide();
            $("#MeasuresHeadingLevel4").hide();
            $("#MeasureTypeHeaderMainTrip *").removeClass("Selected");
            $("#MeasureTypeHeaderMainTrip *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderMainShopper *").removeClass("Selected");
            $("#MeasureTypeHeaderMainShopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeShopperTripHeader *").removeClass("Selected");
            $("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContentTrip *").removeClass("Selected");
            $("#MeasureTypeHeaderContentTrip *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContentShopper *").removeClass("Selected");
            $("#MeasureTypeHeaderContentShopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContentSubLevelTrip").removeClass("Selected");
            $("#MeasureTypeHeaderContentSubLevelTrip").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContentSubLevelShopper").removeClass("Selected");
            $("#MeasureTypeHeaderContentSubLevelShopper").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeContentTrip").removeClass("Selected");
            $("#MeasureTypeContentTrip").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeContentShopper").removeClass("Selected");
            $("#MeasureTypeContentShopper").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderMainTrip .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderMainShopper .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderContentTrip .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $("#MeasureTypeHeaderContentShopper .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            $(".MeasureType").css("width", "auto");
            $(".MeasureType").css("overflow-x", "hidden");
            $(".MeasureType").css("overflow-y", "hidden");
            if (($("#MeasureTypeHeaderContentSubLevelTrip").is(':visible') || $("#MeasureTypeHeaderContentSubLevelShopper").is(':visible')) && ($("#MeasureTypeContentTrip").is(':visible') || $("#MeasureTypeContentShopper").is(':visible'))) {
                //$(".MeasureScrollDiv").css("width", "110%");
                //$(".MeasureType").css("width", "95%");
                $(".MeasureType").css("width", "auto");
                // $(".Lavel").css("width", "20%");
            }
            else {
                $(".MeasureType").css("width", "auto");
                //$(".Lavel").css("width", "262px");
            }
            if ($(".MeasureType").width() > window.innerWidth)
                $(".MeasureType").css("width", "95%");
            else
                $(".MeasureType").css("width", "auto");
            $(".MeasureType").getNiceScroll().remove();

        }

        $(".GroupType").css("width", "auto")
        if ($(".FilterPopup." + $(this).attr("id")).css('display') == 'none') {
            //$(".FilterPopup").css("left", $(this).width());
            //$(".FilterPopup").css("left", (55 / window.innerWidth) * 100 + "%");
            ClosePopups();
            $(".rgt-cntrl-SubFilter-Conatianer").hide();
            $(".rgt-cntrl-frequency").hide();
            $("#LeftPanel .FilterPopup").removeClass("ActiveFilter");
            $(this).addClass("ActiveFilter");
            //$("." + $(this).attr("id")).show();
            $("." + $(this).attr("id")).show();
            updateSearch($(this));
            if ($(this).attr("id") == "AdvancedFilters") {
                //23-11-17
                AllDemographicsSF = $.extend(true, [], AllDemographics);
                if (Grouplist.length > 0) {
                    for (var j = 0; j < Grouplist.length; j++) {
                        _.each(_.filter(AllDemographicsSF), function (item) {
                            if (item.split("|")[2] == Grouplist[j].parentName)
                                AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                        });
                    }
                }
                if (Measurelist.length > 0) {
                    for (var j = 0; j < Measurelist[0].metriclist.length; j++) {
                        _.each(_.filter(AllDemographicsSF), function (item) {
                            if (item.split("|")[2] == Measurelist[0].parentName)
                                AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                        });
                    }
                }

                if (Grouplist.length > 0 || Measurelist.length > 0) {
                    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographicsSF);
                }
                else {
                    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
                }
                //End 23-11-17
                $("#PrimaryDemoFilterList").find(".Selected").removeClass("Selected");
                $("#PrimaryDemoFilterList").find(".ArrowContainerdiv").css("background-color", "#58554D");
                if (currentpage == "hdn-dashboard-demographic") {
                    if (TabType.toLowerCase() == "trips") {
                        SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllAdvancedFilterLeft).concat(AllSubFrequencyDemo));
                    }
                    else {
                        SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllSubFrequencyDemo));

                    }
                }

            }
            if ($(this).find(".demograhicFitr_img")) { $(this).find(".demograhicFitr_img").css("background-position", "-241px -159px") }
            if ($(this).find(".establishment_img")) {
                $(this).find(".establishment_img").css("background-position", "-466px -147px");
            }
            if ($(this).find(".retailer_img")) { $(this).find(".retailer_img").css("background-position", "-663px -158px") }
            if ($(this).find(".competitor_img")) { $(this).find(".competitor_img").css("background-position", "-3090px -155px") }
            if ($(this).find(".sites_img")) { $(this).find(".sites_img").css("background-position", "-2743px -159px") }
            if ($(this).find(".comparission_img")) { $(this).find(".comparission_img").css("background-position", "-103px -149px") }
            if ($(this).find(".grouptype_img")) { $(this).find(".grouptype_img").css("background-position", "-365px -157px") }
            if ($(this).find(".timeperiod_img")) { $(this).find(".timeperiod_img").css("background-position", "-1234px -159px") }
            if ($(this).find(".measure_img")) { $(this).find(".measure_img").css("background-position", "-556px -157px") }
            if ($(this).find(".Freq_img")) { $(this).find(".Freq_img").css("background-position", "1px -160px") }
            if ($(this).find(".Geo_img")) { $(this).find(".Geo_img").css("background-position", "-2873px -158px") }
        }
        //SetScroll($("#PrimaryDemoFilterList"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#PrimaryAdvancedFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#ChannelOrCategoryContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if ($(this).attr("id") == "GeographyFilters") {
            $("#SecondaryGeographyFilterContent").parent().show();
            DisplaySecondaryGeoFilter($('#PrimaryGeographyFilterList ul li div'));
            if (currentpage == "hdn-analysis-acrosstrips" && Geographylist.length <= 0)
                $("#SecondaryGeographyFilterContent div[id='100'] ul div").eq(0).trigger("click");
            else if (currentpage == "hdn-analysis-acrosstrips" && Geographylist[0].Name == "Total")
                $("#SecondaryGeographyFilterContent div[id='100'] ul div").eq(0).addClass("Selected");
        }

        if ($(this).attr("id") == "GroupType" && currentpage == "hdn-analysis-acrosstrips") {

            if (Geographylist.length > 0 && Geographylist[0].Name != "Total") {
                $("#GroupTypeHeaderContent li[name='Geography']").hide();
                SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
            }
            else {
                $("#GroupTypeHeaderContent li[name='Geography']").show();
                SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
            }
            $("#GroupTypeHeaderContent li[name='Trip Mission']").hide();
            $("#GroupTypeHeaderContent li[name='Day Of Week']").hide();
            $("#GroupTypeHeaderContent li[name='Daypart']").hide();
            var SoapDemographicsList = [];
            if ($("#GroupTypeContent .DemographicList div[filtertype='Demographics']").length > 0) {
                for (var i = 0; i < $("#GroupTypeContent .DemographicList div[filtertype='Demographics']").length; i++) {
                    var par = $("#GroupTypeContent .DemographicList div[filtertype='Demographics']").eq(i).children("span");
                    SoapDemographicsList.push($(par).attr("uniqueid") + "|" + $(par).attr("name") + "|" + $(par).attr("parentname").trim());

                }

            }
            SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", SoapDemographicsList);
        }
        if ($(this).attr("id") == "AdvancedFilters" && currentpage == "hdn-analysis-acrosstrips") {
            if (Geographylist.length > 0 && Geographylist[0].Name != "Total") {
                $("#PrimaryAdvancedFilterContent div[name='Geography']").parent().hide();
                SetScroll($("#PrimaryAdvancedFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
            }
            else {
                $("#PrimaryAdvancedFilterContent div[name='Geography']").parent().show();
                SetScroll($("#PrimaryAdvancedFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
            }
        }
        SetFilterLayerScroll();
        e.stopImmediatePropagation();
        //if (Measurelist.length > 0) {
        //    $("#MeasureTypeHeaderContent").show();
        //    $("#MeasureTypeContent").show();
        //}
        //if (Grouplist.length > 0) {
        //    $("#GroupTypeContent").show();
        //    var obj =$("#GroupTypeContent ul[name='" + Grouplist[0].DBName.split('|')[0].trim() + "']");
        //    $("#GroupTypeContent ul[name='" + Grouplist[0].DBName.split('|')[0].trim() + "']").show();
        //    if (obj.length <= 0)
        //        $("#GroupTypeContent div[name='" + Grouplist[0].DBName.split('|')[0].trim() + "']").show();
        //    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").text(Grouplist[0].DBName.split('|')[0].trim().toUpperCase());
        //    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").show();
        //    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").css("width", "287px");
        //    _.forEach(Grouplist, function (i) { $("#GroupTypeContent div span[name='" + i.Name.trim() + "']").parent().addClass("Selected"); });
        //    if (Grouplist[0].isGeography == "true") {
        //        $("#GroupTypeContentSub").show();
        //        $("#GroupTypeContentSub ul[name='" + Grouplist[0].Name.trim() + "']").show();
        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").text(Grouplist[0].Name.split('|')[0].trim().toUpperCase());
        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").show();
        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").css("width", "287px");
        //    }
        //}
        //else {
        //    $("#GroupTypeContent").hide();
        //    $("#GroupTypeContentSub").hide();
        //    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").hide();
        //    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").hide();
        //    $("#GroupTypeHeaderContent").find(".Selected").removeClass("Selected");
        //}
        $("#GroupTypeContent").hide();
        $("#GroupTypeContentSub").hide();
        $("#GroupTypeGeoContentSub").hide();


        //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").hide();
        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").hide();
        $("#GroupTypeHeaderContent li[onclick ='SelecGroup(this);']").find(".Selected").removeClass("Selected");
        $("#GroupTypeHeaderContent li").find(".ArrowContainerdiv").css("background-color", "#58554D");

        if (currentpage == "hdn-analysis-acrossshopper") {
            $("#BGMBeverage_NonBevarageDiv").css("display", "block");
            $("#beverageHeadingLevel0").show();
            $("#BeverageOrCategoryContent").css("display", "none");
            $("#BGMNonBeverageDiv").css("display", "none");
            $(".Beverage").hide();
            $("#beverageHeadingLevel2").hide();
            $("#beverageHeadingLevel1").hide();
            $(".Beverages").css("width", "auto");
        }
        else {
            $(".Beverages").css("width", "auto");
            $("#BGMBeverage_NonBevarageDiv").css("display", "none");
            $("#BeverageOrCategoryContent").css("display", "block");
            $("#BGMNonBeverageDiv").css("display", "none");
            $("#beverageHeadingLevel2").hide();
            $("#beverageHeadingLevel0").hide();
            $("#beverageHeadingLevel1").show();
        }
        if ($(this).attr("id") == "GroupType" && (currentpage == "hdn-report-retailersshopperdeepdive")) {
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='Pre Shop']").eq(0).attr('onclick', '').unbind('click');
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='In Store']").eq(0).attr('onclick', '').unbind('click');
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='In Store - Beverage Detail']").eq(0).attr('onclick', '').unbind('click');
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='Post Shop/Trip Summary']").eq(0).attr('onclick', '').unbind('click');

        }
        else {
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='Pre Shop']").eq(0).attr('onclick', 'ShowGroup(this);').bind('click');
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='In Store']").eq(0).attr('onclick', 'ShowGroup(this);').bind('click');
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='In Store - Beverage Detail']").eq(0).attr('onclick', 'ShowGroup(this);').bind('click');
            $("#PrimeGroupTypeHeaderContent li[primefiltertype='Post Shop/Trip Summary']").eq(0).attr('onclick', 'ShowGroup(this);').bind('click');
        }
        //SetFilterLayerScroll();
        if ((currentpage.indexOf("hdn-report") > -1) && currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive" && currentpage != "hdn-report-comparebeveragesmonthlypluspurchasers" && currentpage != "hdn-report-beveragemonthlypluspurchasersdeepdive") {
            //End 29-01-18
            if (currentpage == "hdn-dashboard-demographic" && SelectedFrequencyList.length == 1) {
                var freqL = "2";
                if (TabType.toLocaleLowerCase() == "shopper") {
                    if ($("#default-shopper-frequency").val() != null && $("#default-shopper-frequency").val() != "10") {
                        freqL = $("#default-shopper-frequency").val();
                    }
                    else {
                        freqL = "2";
                    }
                    //object =  $(".AdvancedFilters .Frequency ul div>span[uniqueid='" + freqL + "']");
                    object = $("#AdvFilterDivId span[parentname=FREQUENCY][uniqueid='" + freqL + "']").parent("li");

                    SelectedFrequencyList = [];
                    SelectedFrequencyList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("uniqueid") });
                }
                else if ($("#default-shopper-frequency").val() != null) {
                    freqL = $("#default-shopper-frequency").val();
                }

                $(".AdvancedFilters .Frequency ul div>span[uniqueid='" + freqL + "']").parent().removeClass("Selected");
                $(".AdvancedFilters .Frequency ul div>span[uniqueid='" + freqL + "']").parent().addClass("Selected");
                $("#AdvFilterDivId span[parentname=FREQUENCY][uniqueid='" + freqL + "']").parent("li").addClass("Selected");

                if (SelectedDempgraphicList.length > 0) {
                    var _filters = SelectedDempgraphicList;
                    for (var i = 0; i < _filters.length; i++) {
                        //$("#AdvFilterDivId span[uniqueid='" + _filters[i].UniqueId + "']").parent("div").addClass("Selected");
                        $("#AdvFilterDivId span[parentname=FREQUENCY][uniqueid='" + _filters[i].UniqueId + "']").parent("li").addClass("Selected");

                    }
                }
                //$(".AdvancedFilters .Frequency ul div>span[name='MONTHLY +']").parent().addClass("Selected");
            }
            if (currentpage == "hdn-dashboard-demographic") {
                if (TabType.toLowerCase() == "trips") {
                    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllAdvancedFilterLeft).concat(AllSubFrequencyDemo));
                }
                else {
                    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllSubFrequencyDemo));

                }
            }

            $("#ToShowDemoAndAdvFilters").show();
            $("#DemoHeadingLevel0").show();
            $("#PrimaryAdvancedFilterContent").hide();
            $("#DemoHeadingLevel1").hide();
            ShowSelectedFilters();
        }
        else {
            //23-11-17
            AllDemographicsSF = $.extend(true, [], AllDemographics);
            if (Grouplist.length > 0) {
                for (var j = 0; j < Grouplist.length; j++) {
                    _.each(_.filter(AllDemographicsSF), function (item) {
                        if (item.split("|")[2] == Grouplist[j].parentName)
                            AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                    });
                }
            }
            if (Measurelist.length > 0) {
                for (var j = 0; j < Measurelist[0].metriclist.length; j++) {
                    _.each(_.filter(AllDemographicsSF), function (item) {
                        if (item.split("|")[2] == Measurelist[0].parentName)
                            AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                    });
                }
            }
            //End 23-11-17
            $("#PrimaryAdvancedFilterContent").css("display", "inline-block");
            $("#DemoHeadingLevel1").show();
            $("#ToShowDemoAndAdvFilters").hide();
            $("#DemoHeadingLevel0").hide();
            //23-11-17
            if (Grouplist.length > 0 || Measurelist.length > 0) {
                SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographicsSF);
            }
            else {
                SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
            }
            //End 23-11-17
        }
        if ($(this).attr("id") == "MeasureType" && (currentpage.indexOf("chart") > -1)) {
            //23-11-17
            var AllMeasuresSF = [];
            AllMeasuresSF = $.extend(true, [], AllMeasures);
            for (var j = 0; j < Grouplist.length; j++) {
                _.each(_.filter(AllMeasuresSF), function (item) {
                    if (item.split("|")[2] == Grouplist[j].parentName)
                        AllMeasuresSF.splice($.inArray(item, AllMeasuresSF), 1);
                });
            }
            //End 23-11-17
            $("#MeasureTypeHeaderMainTrip").show();
            $("#MeasureTypeHeaderMainTrip ul li").show();
            //23-11-17
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", Grouplist.length == 0 ? AllMeasures : AllMeasuresSF);
            //End 23-11-17
        }
        if ($(this).attr("id") == "Advanced-Analytics-Select-Variable" && currentpage == "hdn-analysis-withinshopper") {
            ShowChannelRetailerVariables();
        }
        if ($(this).attr("id") == "Advanced-Analytics-Select-Variable" && currentpage == "hdn-analysis-withintrips") {
            ShowChannelRetailerVariablesTrips();
        }
        if ($(this).attr("id") == "TotalMeasure" && (currentpage == "hdn-crossretailer-totalrespondentstripsreport")) {
            $("#TotalMeasureHeaderMainTrip").show();
        }
        FilterSelectionLimitText();
        //added by Nagaraju for to update filter search
        //Date: 19-12-2017
        //updateSearch($(this));
        e.stopImmediatePropagation();
    });
    function ShowChannelRetailerVariables() {
        //$("#advanced-analytics-Channel-Trips").hide();
        //$("#advanced-analytics-Retailer-Trips").hide();
        //$("#advanced-analytics-Channel-Shopper").hide();
        $("#CorrespondenceMeasureDivId .level1 ul *").show();
        for (var i = 0; i < Comparisonlist.length; i++) {
            if (Comparisonlist[i].LevelDesc.toLocaleLowerCase().indexOf("channel") > -1) {
                $("#CorrespondenceMeasureDivId .level1 ul *").show();
                _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
                    if (v.attributes.selid.value == "1") {
                        $(v).show();
                    }
                    else {
                        $(v).hide();
                    }
                })
                //$("#advanced-analytics-Retailer-Shopper").hide();
                //$("#advanced-analytics-Channel-Shopper").show();
                return true;
                break;
            }
        }
        $("#CorrespondenceMeasureDivId .level1 ul *").show();
        _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
            if (v.attributes.selid.value == "2") {
                $(v).show();
            }
            else {
                $(v).hide();
            }
        })
        //$("#advanced-analytics-Retailer-Shopper").show();
    }

    function ShowChannelRetailerVariablesTrips() {
        //$("#advanced-analytics-Channel-Shopper").hide();
        //$("#advanced-analytics-Retailer-Shopper").hide();
        //$("#advanced-analytics-Channel-Trips").hide();
        $("#CorrespondenceMeasureDivId .level1 ul *").show();
        for (var i = 0; i < Comparisonlist.length; i++) {
            if (Comparisonlist[i].LevelDesc.toLocaleLowerCase().indexOf("channel") > -1) {
                $("#CorrespondenceMeasureDivId .level1 ul *").show();
                _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
                    if (v.attributes.selid.value == "1") {
                        $(v).show();
                    }
                    else {
                        $(v).hide();
                    }
                })
                //$("#advanced-analytics-Retailer-Trips").hide();
                //$("#advanced-analytics-Channel-Trips").show();
                return true;
                break;
            }
        }
        $("#CorrespondenceMeasureDivId .level1 ul *").show();
        _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
            if (v.attributes.selid.value == "2") {
                $(v).show();
            }
            else {
                $(v).hide();
            }
        })
        //$("#advanced-analytics-Retailer-Trips").show();
    }
    $(document).click(function (e) {
        isPopupVisible = false;
        var popup = $(".FilterPopup");
        var custombase_popup = $(".Custombase-Retailers");
        var popup_menu = $(".popup-menu");
        var popup_block = $("#PopupBlock");
        if (popup_menu.is(e.target)) { return; }
        //if ((!$('.FilterPopup').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0)
        //    && (!$('.popup-menu').is(e.target) && !popup_menu.is(e.target) && popup_menu.has(e.target).length == 0)
        //    && (!$('.Custombase-Retailers').is(e.target) && !custombase_popup.is(e.target) && custombase_popup.has(e.target).length == 0) && (!$('.remove-item-selected').is(e.target) && $('.remove-item-selected').has(e.target).length == 0)) {
        //    ClosePopups();
        //}
        if ((!$('.FilterPopup').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0)
                    && (!$('.popup-menu').is(e.target) && !popup_menu.is(e.target) && popup_menu.has(e.target).length == 0)
                    && (!$('.Custombase-Retailers').is(e.target) && !custombase_popup.is(e.target) && custombase_popup.has(e.target).length == 0) && (!$('.remove-item-selected').is("." + e.target.getAttribute("class")) && $('.remove-item-selected').has("." + e.target.getAttribute("class")).length == 0)) {
            ClosePopups();
        }


        if (!$(".adv-fltr-freq-container *").is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) {
            $(".rgt-cntrl-frequency").hide();
            $(".adv-fltr-freq-container").removeClass("TileActive");
            $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").hide();
            $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel3").hide();
            $("#frequency_containerId-SubLevel1").hide();
            $("#frequency_containerId-SubLevel2").hide();
        }
        if (!$(".adv-fltr-suboptions-list-container *").is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) {
            $(".rgt-cntrl-SubFilter-Conatianer").hide();
            $(".adv-fltr-suboptions-list-container *").removeClass("TileActive");
        }
        if (!$(".adv-fltr-Chnl-container *").is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) {
            $(".rgt-cntrl-chnl").hide();
            $("#adv-fltr-Chnl").removeClass("TileActive");
            $("#rgt-cntrl-chnl-SubFilter3").hide();
            $("#rgt-cntrl-chnl-SubFilter2").hide();
            $("#channelFilterHeadingLevel2").hide();
            $("#channelFilterHeadingLevel3").hide();
        }
        if (!$(".beverageItems *").is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) {
            $(".beverageItems").hide();
            $("#adv-bevselectiontype-freq").removeClass("TileActive");
        }
        if ($(".arrow_popup").children().hasClass("uparrw")) {
            $(".arrow_popup").children().removeClass("uparrw");
            $(".arrow_popup").children().addClass("arrw_dwn");
            $(".arrow_popup").removeClass("active_arrow");
            $("#SelectedFilters").css("height", "21px");
            $("#scrollableselection").css("height", "21px");
            $("#FilterHeader").css("height", "21px");
            $("#Translucent").hide();
        }
        //e.stopImmediatePropagation();
    });

    $("#submitButton").click(function Submit() {
        //LogSelection();
        if ($(".adv-fltr-showhide-txt").text().toLowerCase().trim() == "show less" && ((currentpage.indexOf("tbl") > -1) || (currentpage.indexOf("chart") > -1))) {
            $(".adv-fltr-showhide-txt").trigger("click");
        }

        if (currentpage == "hdn-analysis-withintrips" || currentpage.indexOf("tbl") > -1 || currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
            //if (currentpage.indexOf("tbl") > -1) {
            //    if (currentpage == "hdn-tbl-compareretailers") {
            //        if (!Validate_CompareRetailers()) {
            //            return false;
            //        }
            //    }
            //    else if (currentpage == "hdn-tbl-retailerdeepdive") {
            //        if (ModuleBlock == "PIT") {
            //            if (!Validate_CompareRetailers_Charts() || !Validate_Group()) {
            //                return false;
            //            }
            //        }
            //        else {
            //            if (!Validate_CompareRetailers_Charts() || !Validate_Trend()) {
            //                return false;
            //            }
            //        }
            //    }
            //    else if (currentpage == "hdn-tbl-comparebeverages") {
            //        if (!Validate_CompareBeverages()) {
            //            return false;
            //        }
            //    }
            //    else if (currentpage == "hdn-tbl-beveragedeepdive") {
            //        if (ModuleBlock == "PIT") {
            //            if (!Validate_CompareBeverages_Charts() || !Validate_Group()) {
            //                return false;
            //            }
            //        }
            //        else {
            //            if (!Validate_CompareBeverages_Charts()) {
            //                return false;
            //            }
            //        }
            //    }
            //    else if (currentpage == "hdn-e-commerce-tbl-comparesites") {
            //        if (!Validate_Site()) {
            //            return false;
            //        }
            //    }
            //    else if (currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
            //        if (ModuleBlock == "PIT") {
            //            if (!Validate_Group_Site() || !Validate_Group()) {
            //                return false;
            //            }
            //        }
            //        else {
            //            if (!Validate_Group_Site()) {
            //                return false;
            //            }
            //        }
            //    }
            //}
            if (currentpage == "hdn-tbl-compareretailers") {
                if (!Validate_CompareRetailers()) {
                    return false;
                }
            }
            else if (currentpage == "hdn-tbl-comparebeverages") {
                if (!Validate_CompareBeverages()) {
                    return false;
                }
            }
            else if (currentpage == "hdn-tbl-retailerdeepdive") {
                if (!Validate_DeepDive_Retailer_ShopperSegment()) {
                    return false;
                }
            }
            else if (currentpage == "hdn-tbl-beveragedeepdive") {
                if (!Validate_DeepDive_Beverage_ShopperSegment()) {
                    return false;
                }
            }

            $(".advance-filters").css("display", "block");
            $(".adv-fltr-details").css("display", "block");
            $(".adv-fltr-details").css("margin-top", "0%");
        }
        if (currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
            $("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").show();
            $("#frequency_containerId ul li[name='TOTAL VISITS']").hide();

            $("#adv-fltr-freq").show();
            $(".adv-fltr-details").show();
            HideOrShowFilters();
        }
        if ((currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") && (sVisitsOrGuests == "1" || TabType == "trips")) {
            $(".adv-fltr-suboptions-list-container").show();
        }
        else if ((currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") && (sVisitsOrGuests == "2" || TabType == "shopper")) {
            $(".adv-fltr-suboptions-list-container").hide();
        }
        sRemovedLegendPosition = [];
        //$("#RightPanelPartial").show();
        $(".adv-fltr-details").css("display", "block");
        if ((document.getElementById('adv-fltr visitsId') != null) || (document.getElementById('adv-fltr visitsTrendId') != null)) {
            var navbarwidth = document.getElementById('adv-fltr visitsId').offsetWidth;
            if (currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-tbl-retailerdeepdive") {
                if (ModuleBlock.toUpperCase() == "TREND")
                    navbarwidth = document.getElementById('adv-fltr visitsTrendId').offsetWidth;
                else
                    navbarwidth = document.getElementById('adv-fltr visitsId').offsetWidth;
            }
            if (currentpage == "hdn-tbl-beveragedeepdive" || currentpage == "hdn-tbl-comparebeverages")
                var sWidth = (navbarwidth * 19.19) / 100;
            else if (currentpage.indexOf('retailer')) {
                var sWidth1 = (navbarwidth * 21.90) / 100;
                var sWidth = (navbarwidth * 22.6) / 100;
                $(".width-3").css("width", sWidth1);
            }
            else
                var sWidth = (navbarwidth * 24.6) / 100;
            $(".width-5").css("width", sWidth);


        }
        if (!($(".adv-fltr-toggle-container").is(":visible")))
            $(".toggle-seperator").hide();
        else
            $(".toggle-seperator").show();
        if ($("#adv-fltr-Chnl").is(":visible") || $("#adv-bevselectiontype-freq").is(":visible"))
            $(".advancedfilter-seperator").show();
        else
            $(".advancedfilter-seperator").hide();
    });

    $("#MenuHeader ul li").hover(function () {
        var path = $(this).attr("id");
        if (path == undefined)
            path = $(this).parent().attr("id");
        switch (path) {
            case "Dashboard": $(".dashboard-logo").css("background-position", "-437px -1px"); break;
            case "Reports": $(".reports-logo").css("background-position", "6px 0px"); break;
            case "Tables": $(".tables-logo").css("background-position", "-78px 0px"); break;
            case "Charts": $(".charts-logo").css("background-position", "-156px 0px"); break;
            case "Analysis": $(".analysis-logo").css("background-position", "-244px 0px"); break;
        }
    });
    $("#MenuHeader ul li").mouseleave(function () {
        var path = $(this).attr("id");
        if (path == undefined)
            path = $(this).parent().attr("id");
        var cur_path = $(this).find(".master_link_a.Active").parent("div").parent().attr("id");
        if (path != cur_path) {
            switch (path) {
                case "Dashboard": $(".dashboard-logo").css("background-position", "-482px -1px"); break;
                case "Reports": $(".reports-logo").css("background-position", "-35px 0px"); break;
                case "Tables": $(".tables-logo").css("background-position", "-119px 0px"); break;
                case "Charts": $(".charts-logo").css("background-position", "-199px 0px"); break;
                case "Analysis": $(".analysis-logo").css("background-position", "-288px 0px"); break;
            }
        }
    });

    ActivateCurrentPage();
    //if (sFilterData == null || sFilterData == "" || sFilterData.length <= 0 || sFilterData == {} || sFilterData.Measure == undefined)

    if ($("#hdn-page").length > 0) {
        LoadFilters();
    }
    //LoadRightFilter();]
    $("#ddlbeverageitems").on('change', function () {
        sSelectedDropDownBrand = $(this).find('option:selected').attr('id');
        prepareContentArea();
    });
    $("#SiteHeadingLevel2").hide();
    //adding extra demography and advanced filter level for reports
    if ((currentpage.indexOf("hdn-report") > -1) && currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive" && currentpage != "hdn-report-comparebeveragesmonthlypluspurchasers" && currentpage != "hdn-report-beveragemonthlypluspurchasersdeepdive") {
        $("#ToShowDemoAndAdvFilters").show();
        $("#DemoHeadingLevel0").show();
        $("#PrimaryAdvancedFilterContent").hide();
        $("#DemoHeadingLevel1").hide();
    }
    else {
        $("#PrimaryAdvancedFilterContent").css("display", "inline-block");
        $("#DemoHeadingLevel1").show();
        $("#ToShowDemoAndAdvFilters").hide();
        $("#DemoHeadingLevel0").hide();
    }
    $(".MeasureScrollDiv, #AdvFilterDivId, .AdvancedFilters").scroll(function () {
        if (sVisitsOrGuests == 1) {
            SetScroll($("#MeasureTypeHeaderContentSubLevelTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        }
        else if (sVisitsOrGuests == 2) {
            SetScroll($("#MeasureTypeHeaderContentSubLevelShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        }

        if (sVisitsOrGuests == 1) {
            SetScroll($("#MeasureTypeContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        }
        else if (sVisitsOrGuests == 2) {
            SetScroll($("#MeasureTypeContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        }
        SetFilterLayerScroll();
    });
    SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //var leftPos = $('#BevContainerDivId').scrollLeft() + 200;
    //$('#BevContainerDivId').scrollLeft(leftPos);

    //Low Sample Size Popup
    $(document).on("click", ".LowSample-clsebtn, .LowSample-cancel", function (e) {
        $(".LowSample-popup").hide();
        $("#Translucent").hide();
    });
    if ($(".LowSample-popup").is(":visible"))
        $(".TranslucentDiv").show();
    //End
    $(".Geotooltipimage, #ToShowShooperAndTrips li[name='Shopper Measures']").hover(function () {

        // Hover over code      
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
    if (currentpage == "")
        //$("#MenuHeader #Tables .master_link_a").trigger("click");
        if (currentpage.indexOf("chart") > -1) {
            $('#spChartLegend').show();
            $("#ToShowChart").css("height", "65%");
            $(".trendChartMain").css("height", "100%");
        }
    jQuery('.classMouseHover').mouseenter(function () {
        //if ($("#LeftPanel").is(":visible") == false)
        //    $("#MouseHoverBigDiv").css("background-size", "100% 100%");
        //else
        //    $("#MouseHoverBigDiv").css("background-size", "contain");

        jQuery(this).find('.play').show();
        var objs = GetMouseHoverPopUpDetails();
        var sClassName = $(this).attr('class').split(' ')[1];
        var sPopupDetails = _.filter(objs, function (i) {
            return i.cls == sClassName
        }).length > 0 ? _.filter(objs, function (i) { return i.cls == sClassName; })[0] : "";
        if (sPopupDetails != undefined && sPopupDetails != "" && sPopupDetails != null)
            MouseHoverPopupshowHide(sPopupDetails);
        //$("#MouseHoverBigDiv").show();
    });
    jQuery('.classMouseHover').mouseleave(function () {
        //if ($("#LeftPanel").is(":visible") == false)
        //    $("#MouseHoverBigDiv").css("background-size", "100% 100%");
        //else
        //    $("#MouseHoverBigDiv").css("background-size", "contain");
        jQuery(this).find('.play').hide();
        var objs = GetMouseHoverPopUpDetails();

        $("#MouseHoverBigDiv").hide();
        $("#MouseHoverSmallDiv").hide();
        $("#MouseHoverSmallerDiv").hide();
        $("#MouseHoverExtraSmallDiv").hide();
    });
    $(document).on("click", ".SubMenuli", function (e) {
        var href = $(this).find("a").eq(0).attr("href");
        window.location.href = href;
    });
    $(document).on("click", ".ArrowContainerdiv", function (e) {
        e.stopPropagation();
        if ($(this).find("span")[0].hasAttribute("onclick") && sflag == 0) {
            sflag = 1;
            $(this).find("span").eq(0).trigger("click");
        }
        sflag = 0;
    });

    //$(document).on("click", ".Lavel ul li", function (e) {
    //    $(".Lavel ul li .ArrowContainerdiv").css("color", "rgb(88, 85, 77)");
    //});
    $(document).on("click", ".Lavel ul li", function (e) {
        if ($(this).hasClass("Selected") || $(this).find("div").hasClass("Selected")) {
            //$(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
        }
        else
            $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
    });
    $(document).on("click", ".lft-popup-ele", function (e) {
        if (!($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) {
            if ($(this).hasClass("Selected") || $(this).find("div").hasClass("Selected")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
            else
                $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
        }
    });
    if (!($(".adv-fltr-toggle-container").is(":visible")))
        $(".toggle-seperator").hide();
    else
        $(".toggle-seperator").show();
    if ($("#adv-fltr-Chnl").is(":visible") || $("#adv-bevselectiontype-freq").is(":visible"))
        $(".advancedfilter-seperator").show();
    else
        $(".advancedfilter-seperator").hide();


    $(".dynpos").hover(function (e) {
        var pos = $(this).position();
        var width = $(this).outerWidth();
        var height = $(this).outerHeight();
        var widthdiv = $("#MouseHoverExtraSmallDiv").outerWidth();
        //show the menu directly over the placeholder
        $("#MouseHoverExtraSmallDiv").css({
            position: "absolute",
            top: (pos.top + height) + "px",
            left: (pos.left - widthdiv + 22) + "px",
        }).show();

    }, function () {
        // Hover out code
        $('#MouseHoverExtraSmallDiv').hide();
    });
    $(".dynpos1").hover(function (e) {
        var pos = $(this).offset();
        var width = $(this).outerWidth();
        var height = $(this).outerHeight();
        var widthdiv = $("#MouseHoverSmallerDiv").outerWidth();

        var pageWidth = $(window).width();
        var pageHeight = $(window).width();
        var elementWidth = $("#MouseHoverSmallerDiv").outerWidth();
        var elementLeft = $(this).offset().left;
        var elementHeight = $("#MouseHoverSmallerDiv").outerHeight();
        var elementTop = $(this).offset().top;
        if ($(this).hasClass("up")) {
            $("#MouseHoverSmallerDiv").css({
                position: "absolute",
                top: (pos.top - height - 50) + "px",
                left: (pos.left + 5) + "px",
            }).show();
        }
        else {
            if ($(this).hasClass("breadcrumb-open")) {
                $("#MouseHoverSmallerDiv").css({
                    position: "absolute",
                    top: (pos.top + height + 20) + "px",
                    left: (pos.left - widthdiv - 10) + "px",
                }).show();
            }
            else if ($(this).attr("id") == "btnClearAll") {
                $("#MouseHoverSmallerDiv").css({
                    position: "absolute",
                    top: (pos.top + height - 6) + "px",
                    left: (pos.left - widthdiv + $(this).width() + 15) + "px",
                }).show();
            }
            else {
                $("#MouseHoverSmallerDiv").css({
                    position: "absolute",
                    top: (pos.top + height - 6) + "px",
                    left: (pos.left) + "px",
                }).show();
            }
        }
        //show the menu directly over the placeholder


    }, function () {
        // Hover out code
        $('#MouseHoverSmallerDiv').hide();
    });

    FilterSelectionLimitText();

});
//Loader
function UnSelectArrow() {
    $(".GroupContentDiv .ArrowContainerdiv").each(function (i, j) {
        $(j).css("background-color", "#58554D")
    });
    $("#MeasureContainerDivId .ArrowContainerdiv").each(function (i, j) {
        $(j).css("background-color", "#58554D")
    });
    $(".AdvancedFilters .ArrowContainerdiv").each(function (i, j) {
        $(j).css("background-color", "#58554D")
    });


}
var numberOfAjaxRequests = 0;
$(document).ajaxSend(function () {
    numberOfAjaxRequests++;
    ShowLoader();
});
$(document).ajaxComplete(function () {
    numberOfAjaxRequests--;
    if (numberOfAjaxRequests == 0) {
        HideLoader();
        isonreadystatechange = false;
        if (isColPaletCalledDirectly == true) {
            isColPaletCalledDirectly = false
            $("#Translucent").css("z-index", "9000");
            $(".TranslucentDiv").show();
        }
    }
});
function ShowLoader() {
    $("#Translucent").css("z-index", "9000");
    $(".TranslucentDiv").show();
    $("#Loader").show();
}
function HideLoader() {
    $(".TranslucentDiv").hide();
    $("#Translucent").css("z-index", "1001");
    $("#Loader").hide();
    if ($(".LowSample-popup").is(":visible") || $(".exporttoexcelpopup").is(":visible") || $(".exportchartlistpopup").is(":visible") || $(".popup1").is(":visible")) {
        Set_zIndex();
        $(".TranslucentDiv").show();
    }
}

function ShowLoaderDB() {
    $("#TranslucentDB").css("z-index", "9100");
    $(".TranslucentDivDB").show();
    $("#Loader").show();
}
function HideLoaderDB() {
    $(".TranslucentDivDB").hide();
    $("#TranslucentDB").css("z-index", "1001");
    $("#Loader").hide();
    if ($(".popup1").is(":visible")) {
        $("#Translucent").css("z-index", "9000");
        $(".TranslucentDiv").show();
    }
}
function ClosePopups() {
    if (!$(".MeasureType").is(':visible')) {
        $(".popup-menu").hide();
        $(".Sub-Lavel").hide();
    }
    $(".adv-fltr-Chnl").removeClass("TileActive");
    $("#frequency_containerId .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#frequency_containerId-SubLevel1 .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $(".rgt-cntrl-SubFilter-Conatianer .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#rgt-cntrl-chnl-SubFilter1 .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#rgt-cntrl-chnl-SubFilter2 .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#BGMBeverage_NonBevarageDiv .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#ToShowShooperAndTrips .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#advanced-analytics-Retailer-Trips .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#advanced-analytics-Channel-Trips .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#advanced-analytics-Retailer-Shopper .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#advanced-analytics-Channel-Shopper .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#GroupTypeContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#GroupTypeContentSub .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#ToShowDemoAndAdvFilters .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#ToShowDemoAndAdvFilters *").removeClass("Selected");
    $("#ToShowDemoAndAdvFilters *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(".rgt-cntrl-SubFilter-Conatianer").hide();
    $(".beverageChannel").removeClass("TileActive");
    $(".adv-fltr-suboptions-list-container *").removeClass("TileActive");
    $(".adv-fltr-selection-container").removeClass("TileActive");
    $(".rgt-cntrl-Selection").hide();
    $(".rgt-cntrl-Selection").hide();
    $(".adv-fltr-selection-container").removeClass("TileActive");
    $(".rgt-cntrl-chnl").hide();
    $(".MonPurFreq").removeClass("TileActive");
    $(".adv-fltr-freq-container").removeClass("TileActive");
    $(".rgt-cntrl-frequency").hide();
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").hide();
    $("#TotalMeasureHeadingLevel2").hide();
    $("#ToShowShooperAndTrips *").removeClass("Selected");
    $("#ToShowShooperAndTrips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#advanced-analytics-Retailer-Trips *").removeClass("Selected");
    $("#advanced-analytics-Retailer-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#advanced-analytics-Channel-Trips *").removeClass("Selected");
    $("#advanced-analytics-Channel-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#advanced-analytics-Retailer-Shopper *").removeClass("Selected");
    $("#advanced-analytics-Retailer-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#advanced-analytics-Channel-Shopper *").removeClass("Selected");
    $("#advanced-analytics-Channel-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(".txt-search").val("");
    $(".FilterPopup").hide();
    $(".FilterMenu").removeClass("ActiveFilter");
    $("#RetailerOrBrandContent").hide();
    $("#SecondaryAdvancedFilterContent .DemographicList").hide();
    $("#ThirdLevelAdvancedFilterContent .DemographicList").hide();
    $("#SecondaryAdvancedFilterContent div[name=Other]").find(".Selected").removeClass("Selected")
    $("#SecondaryAdvancedFilterContent div[name=Other]").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#SecondaryAdvancedFilterContent").hide();
    $("#ThirdLevelAdvancedFilterContent").hide();
    $(".demograhicFitr_img").css("background-position", "-300px -159px");
    $(".Geo_img").css("background-position", "-2933px -158px");

    $(".establishment_img").css("background-position", "-507px -148px");
    $(".retailer_img").css("background-position", "-719px -157px");
    $(".competitor_img").css("background-position", "-3147px -154px");
    $(".sites_img").css("background-position", "-2808px -159px");

    $(".comparission_img").css("background-position", "-1277px -149px");
    $(".timeperiod_img").css("background-position", "-1291px -159px");
    $(".grouptype_img").css("background-position", "-421px -159px");
    $(".measure_img").css("background-position", "-611px -158px");
    $(".Freq_img").css("background-position", "-55px -160px");
    $("#SecondaryAdvancedFilterContent div[name=Other]").find(".Selected").removeClass("Selected");
    $("#SecondaryAdvancedFilterContent div[name=Other]").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(".Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").hide();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").hide();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").hide();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").hide();
    $(".AdvancedFiltersDemoHeading #beverageHeadingLevel0").hide();
    $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").hide();
    $(".AdvancedFiltersDemoHeading #beverageHeadingLevel3").hide();
    $(".GroupContentDiv .AdvancedFiltersDemoHeading #grouptypeHeadingLevel5").hide();

    $(".MeasureType").hide();
    //$(".Beverage").hide();
    //if (Measurelist.length <= 0) {
    //    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel2").hide();
    //    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel3").hide();
    //    $("#MeasureTypeHeaderContent").hide();
    //    $("#MeasureTypeContent").hide();
    //    $("#MeasureTypeHeaderContentSubLevel").hide();
    //    $("#MeasuresHeadingLevel4").hide();
    //    $("#MeasureTypeHeaderContent").hide();
    //    $("#MeasuresHeadingLevel1").hide();
    //    $("#MeasuresHeadingLevel2").hide();
    //    $("#MeasuresHeadingLevel3").hide();
    //}
    //else {
    //    $("#MeasureTypeHeaderMain").show();
    //}



    $("#ChannelOrCategoryContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#PrimaryDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#SecondaryDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#ThirdDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#GroupTypeHeaderContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#BeverageOrCategoryContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#SecondaryGeographyFilterContent").find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //$("#ThirdGeographyFilterList div[onclick='DisplayFourthLevelGeoFilter(this);']").find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //$("#ThirdGeographyFilterList div[onclick='DisplayFourthLevelGeoFilter(this);']").removeClass("Selected");
    $("#ThirdGeographyFilterList div .RemoveSelectionClass").find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    $("#ThirdGeographyFilterList div .RemoveSelectionClass").removeClass("Selected");
    $("#ThirdGeographyFilterList div .RemoveSelectionClass").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$("#rgt-cntrl-chnl-SubFilter1 div[onclick='DisplayChannelDemoFilter(this);']").removeClass("Selected");
    //$("#rgt-cntrl-SubFilter1 div[onclick='DisplayVisistThirdLevelDemoFilter(this);']").removeClass("Selected");
    $("#rgt-cntrl-chnl-SubFilter1 div .RemoveSelectionClass").removeClass("Selected");
    $("#rgt-cntrl-chnl-SubFilter1 div .RemoveSelectionClass").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#rgt-cntrl-SubFilter1 div .RemoveSelectionClass").removeClass("Selected");
    $("#rgt-cntrl-SubFilter1 div .RemoveSelectionClass").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#SiteHeadingLevel2").hide();

    if (currentpage.indexOf("hdn-report") > 0) {
        $("#PrimaryAdvancedFilterContent").hide();
        $("#DemoHeadingLevel1").hide();
    }

    $(".Beverages").css("width", "auto");
    $(".BevScrollDiv").css("width", "auto");
    SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //var leftPos = $('#BevContainerDivId').scrollLeft() + 200;
    //$('#BevContainerDivId').scrollLeft(leftPos);
    $(".Beverage").getNiceScroll().remove();

    $("#SecondaryGeographyFilterContent *").removeClass("Selected");
    $("#SecondaryGeographyFilterContent *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#ThirdLevelGeographyFilterContent").hide();
    $("#GeographyHeadingLevel2").hide();
    $("#GeographyHeadingLevel3").hide();

    $("#GroupTypeContent").hide();
    $("#GroupTypeContentSub").hide();
    $("#GroupTypeHeaderContent").hide();

    $("#grouptypeHeadingLevel4").hide();
    $("#DemoHeadingLevel4").hide();
    $("#advanced-analytics-Retailer-Trips").hide();
    $("#advanced-analytics-Retailer-Shopper").hide();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
    // $("#advancedanalyticsHeadingLevel1").hide(); 
    $("#advancedanalyticsHeadingLevel1").show();
    $("#advancedanalyticsHeadingLevel2").hide();
    $("#advanced-analytics-Channel-Shopper").hide();
    $("#advanced-analytics-Channel-Trips").hide();

    $("#TotalMeasureShopperTripHeader *").removeClass("Selected");
    $("#TotalMeasureShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#TotalMeasureShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#TotalMeasureHeadingLevel2").hide();
    $("#TotalMeasureHeaderMainTrip *").removeClass("Selected");
    $("#TotalMeasureHeaderMainTrip *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#TotalMeasureHeaderMainTrip .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#TotalMeasureHeaderMainShopper *").removeClass("Selected");
    $("#TotalMeasureHeaderMainShopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#TotalMeasureHeaderMainShopper .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#TotalMeasureHeadingLevel3").hide();
    $("#PrimeGroupTypeHeaderContent ul li").removeClass("Selected");
    $("#PrimeGroupTypeHeaderContent ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").parent().removeClass("sidearrw_OnCLick");
    $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").addClass("sidearrw");
    $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").removeClass("sidearrw_OnCLick");
    $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").addClass("sidearrw");
    $("#TotalMeasureHeadingLevel3").hide();
    $("#GroupTypeHeaderContent").hide();
    //$(".LowSample-popup").hide();
}
function GetTimePeriodSliderlist(Name) {
    CurrentTimePeriodlist = [];
    for (var i = 0; i < AllTimePeriods.length; i++) {
        if (AllTimePeriods[i].Name.toLowerCase() == Name.toLowerCase()) {
            var TimePeriodData = [];
            if (currentpage == "hdn-analysis-acrossshopper") {
                if (Name.toLowerCase() == "year") {
                    CurrentTimePeriodlist = AllTimePeriods[i].TimePeriods.slice(0, 1);
                    return AllTimePeriods[i].Sliderlist.slice(1, AllTimePeriods[i].Sliderlist.length);
                    break;
                }
                else if (Name.toLowerCase() == "quarter") {
                    CurrentTimePeriodlist = AllTimePeriods[i].TimePeriods.slice(0, 4);
                    return AllTimePeriods[i].Sliderlist.slice(4, AllTimePeriods[i].Sliderlist.length);
                    break;
                }
                else if (Name.toLowerCase() == "ytd" || Name.toLowerCase() == "12mmt" || Name.toLowerCase() == "3mmt" || Name.toLowerCase() == "6mmt") {
                    CurrentTimePeriodlist = AllTimePeriods[i].TimePeriods.slice(0, 1);
                    return AllTimePeriods[i].Sliderlist.slice(12, AllTimePeriods[i].Sliderlist.length);
                    break;
                }
                else
                    CurrentTimePeriodlist = AllTimePeriods[i].TimePeriods;
                //data.TimePeriodlist.slice(0, n).remove(); 
            }
            else
                CurrentTimePeriodlist = AllTimePeriods[i].TimePeriods;
            //CurrentTimePeriodlist = AllTimePeriods[i].TimePeriods;
            return AllTimePeriods[i].Sliderlist;
            break;
        }
    }
}
function SelectTimePeriod(obj) {
    //CustomBaseFlag = 0;
    label = [];
    $("#TimeBlock ul li").removeClass("Selected");
    $("#TimeBlock ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(obj).addClass("Selected");
    var Name = $(obj).attr("name").toLowerCase();
    TimePeriodId = $(obj).attr("id").toLowerCase();
    label = GetTimePeriodSliderlist(Name);
    $("#SliderContent").html("");
    $("#SliderContent").html("<div class=\"slider\"></div>");
    SetCustomBaseforTotalTime(obj);
    TimeExtension = Name;
    TimePeriod = Name;
    $(".ui-slider-tip").remove();
    $(".totime").val("");
    CheckTimePeriodChanged = "Yes";
    if (Name == "total time") {
        $(".timeType").val(label[0]);
        TimePeriod = "total|total";
        $('.slider').hide();
        var uislider = new Object();
        uislider.value = 0;
        if (ModuleBlock != "TREND")
            slidechange(null, uislider);
    }
    else {
        //if (Name == 'month') 
        //    label = month;
        //else if (Name == 'quarter')
        //    label = quarter;        
        //else if (Name == '3mmt') 
        //    label = mmt;        
        //else if (Name == 'year') 
        //    label = year;       
        //else if (Name == '12mmt')
        //    label = MMT_12;        
        //else if (Name == 'ytd') 
        //    label = YTD;       
        //else if (Name == '6mmt') 
        //    label = MMT_6;       

        if (ModuleBlock == "TREND") {
            $('.slider').slider({
                max: label.length - 1, min: 0,
                range: true, minRangeSize: 1,
                values: [(label.length > 1 ? label.length - 2 : 0),
                    label.length - 1],
                slide: slidechange,
            }).slider('pips', {
                rest: true,
                labels: label,
            }).slider('float', {
                labels: true,
                rest: true,
            });

            var uislider = new Object();
            uislider.values = [];
            uislider.values.push($.inArray($(".ui-state-default .ui-slider-tip").eq(0).html(), label));
            uislider.values.push($.inArray($(".ui-state-default .ui-slider-tip").eq(1).html(), label));
            slidechange(null, uislider);
        }
        else {
            $('.slider').slider({
                max: label.length - 1,
                min: 0,
                value: (label.length > 1 ? label.length - 1 : 0),
                slide: slidechange,
            }).slider('pips', {
                rest: true,
                labels: label,

            }).slider('float', {
                labels: true

            });

            var uislider = new Object();
            uislider.value = $.inArray($(".ui-state-default .ui-slider-tip").eq(0).html(), label);
            slidechange(null, uislider);
        }
    }
    SelectTimePeriodNew($(obj).attr("name").toLowerCase(), "8", $(obj));
    if (Grouplist.length > 0) {
        if (Grouplist[0].isGeography == "true") {
            Grouplist = [];
            $("#groupDivId *").removeClass("Selected");
            $("#GroupTypeContentSub *").removeClass("Selected");
            $("#GroupTypeContentSub *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#GroupTypeContent .DemographicList[name=Geography] *").removeClass("Selected");
            $("#GroupTypeContent .DemographicList[name=Geography] *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            LoadAdvancedFilters(sFilterData);
        }
    }
    if (SelectedDempgraphicList.length > 0) {

        SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
            return obj.isGeography !== 'true';
        });
    }
    $("#ThirdLevelAdvancedFilterContent *").removeClass("Selected");
    $("#ThirdLevelAdvancedFilterContent *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#SecondaryAdvancedFilterContent .DemographicList[name=Geography] *").removeClass("Selected");
    $("#SecondaryAdvancedFilterContent .DemographicList[name=Geography] *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    if (currentpage == "hdn-analysis-acrosstrips") {
        dGeo = [];
        LoadSecondaryGeographyFilters();
    }
    ShowSelectedFilters();
}
//
function custombase_defaulttime() {
    var timeli = $("#default-time-period").val().split('|');
    var obj = $("#TimeBlock li[id='" + timeli[0] + "']");
    label = [];
    $("#TimeBlock ul li").removeClass("Selected");
    $("#TimeBlock ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(obj).addClass("Selected");
    var Name = $(obj).attr("name").toLowerCase();
    TimePeriodId = $(obj).attr("id").toLowerCase();
    label = GetTimePeriodSliderlist(Name);
    $("#SliderContent").html("");
    $("#SliderContent").html("<div class=\"slider\"></div>");
    SetCustomBaseforTotalTime(obj);
    TimeExtension = Name;
    TimePeriod = Name;
    $(".ui-slider-tip").remove();
    $(".totime").val("");
    CheckTimePeriodChanged = "Yes";
    if (Name == "total time") {
        $(".timeType").val(label[0]);
        TimePeriod = "total|total";
        $('.slider').hide();
        var uislider = new Object();
        uislider.value = 0;
        if (ModuleBlock != "TREND")
            slidechange(null, uislider);
    }
    else {

        if (ModuleBlock == "TREND") {
            $('.slider').slider({ max: label.length - 1, min: 0, range: true, minRangeSize: 1, values: [(label.length > 1 ? label.length - 2 : 0), label.length - 1], slide: slidechange, }).slider('pips', {
                rest: true,
                labels: label,
            }).slider('float', {
                labels: true,
                rest: true,
            });

            var uislider = new Object();
            uislider.values = [];
            uislider.values.push($.inArray($(".ui-state-default .ui-slider-tip").eq(0).html(), label));
            uislider.values.push($.inArray($(".ui-state-default .ui-slider-tip").eq(1).html(), label));
            slidechange(null, uislider);
        }
        else {
            $('.slider').slider({
                max: label.length - 1,
                min: 0,
                value: (label.length > 1 ? label.length - 1 : 0),
                slide: slidechange,
            }).slider('pips', {
                rest: true,
                labels: label,

            }).slider('float', {
                labels: true

            });

            var uislider = new Object();
            uislider.value = $.inArray($(".ui-state-default .ui-slider-tip").eq(0).html(), label);
            slidechange(null, uislider);
        }
    }
    SelectTimePeriodNew($(obj).attr("name").toLowerCase(), "8", $(obj));
    if (Grouplist.length > 0) {
        if (Grouplist[0].isGeography == "true") {
            Grouplist = [];
            $("#groupDivId *").removeClass("Selected");
            $("#GroupTypeContentSub *").removeClass("Selected");
            $("#GroupTypeContentSub *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#GroupTypeContent .DemographicList[name=Geography] *").removeClass("Selected");
            $("#GroupTypeContent .DemographicList[name=Geography] *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            LoadAdvancedFilters(sFilterData);
        }
    }
    if (SelectedDempgraphicList.length > 0) {

        SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
            return obj.isGeography !== 'true';
        });
    }
    $("#ThirdLevelAdvancedFilterContent *").removeClass("Selected");
    $("#ThirdLevelAdvancedFilterContent *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#SecondaryAdvancedFilterContent .DemographicList[name=Geography] *").removeClass("Selected");
    $("#SecondaryAdvancedFilterContent .DemographicList[name=Geography] *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    if (currentpage == "hdn-analysis-acrosstrips") {
        dGeo = [];
        LoadSecondaryGeographyFilters();
    }
    ShowSelectedFilters();
}
function GetTimePeriodUniqueId(periodName) {
    for (var i = 0; i < CurrentTimePeriodlist.length; i++) {
        if (CurrentTimePeriodlist[i].Name.toLocaleLowerCase() == periodName.toLocaleLowerCase()) {
            return CurrentTimePeriodlist[i].UniqueId;
            break;
        }
    }
}
function slidechange(event, ui) {
    CheckTimePeriodChanged = "Yes";
    SelectGeographyFilter('', '', '', '', '', '', '', '');
    TimePeriod_ShortNames = [];
    TimePeriod_Unique_ShortNames = [];
    if (ModuleBlock == "TREND") {
        TrendCustomBaselist = [];
        $(".timeType").val(label[ui.values[0]] + " To " + label[ui.values[1]]);
        $(".totime").val(label[ui.values[1]]);
        TimePeriod_From = TimeExtension + "|" + label[ui.values[0]];
        TimePeriod_To = label[ui.values[1]];
        for (var i = parseInt(ui.values[0]) ; i <= parseInt(ui.values[1]) ; i++) {
            TimePeriod_ShortNames.push(label[i].replace(" 48MMT", "").replace(" 36MMT", "").replace(" 30MMT", "").replace(" 24MMT", "").replace(" 18MMT", "").replace(" 3MMT", "").replace(" 6MMT", "").replace(" 12MMT", ""));
            TrendCustomBaselist.push({ Name: label[i].replace(" 48MMT", "").replace(" 36MMT", "").replace(" 30MMT", "").replace(" 24MMT", "").replace(" 18MMT", "").replace(" 3MMT", "").replace(" 6MMT", "").replace(" 12MMT", ""), UniqueId: (parseInt(GetTimePeriodUniqueId(label[i]))), DBName: label[i].replace("48MMT", "").replace("36MMT", "").replace("30MMT", "").replace("24MMT", "").replace("18MMT", "").replace("3MMT", "").replace("6MMT", "").replace("12MMT", "") });
        }
        TimePeriodFrom_UniqueId = (parseInt(GetTimePeriodUniqueId(label[ui.values[0]])));
        TimePeriodTo_UniqueId = (parseInt(GetTimePeriodUniqueId(label[ui.values[1]])));
        BuildDynamicTable();
    }
    else {
        $(".timeType").val(label[ui.value]);
        if (currentpage == "hdn-analysis-acrossshopper") {
            if (TimePeriodId == "2") {
                TimePeriod_UniqueId = TimePeriodId + "|" + (parseInt(ui.value) + 1 + 1);
            }
            else if (TimePeriodId == "4") {
                TimePeriod_UniqueId = TimePeriodId + "|" + (parseInt(ui.value) + 1 + 4);
            }
            else if (TimePeriodId == "3" || TimePeriodId == "5" || TimePeriodId == "6" || TimePeriodId == "7") {
                TimePeriod_UniqueId = TimePeriodId + "|" + (parseInt(ui.value) + 1 + 12);
            }
            else
                TimePeriod_UniqueId = TimePeriodId + "|" + (parseInt(ui.value) + 1);
        }
        else
            TimePeriod_UniqueId = TimePeriodId + "|" + (parseInt(ui.value) + 1);
    }

    sCurrent_PreviousTime = "";
    var timeperiodarray = [];
    timeperiodarray = $(".timeType").val().split(" ");
    PreviousTimePeriod = "";
    for (var i = 0; i < timeperiodarray.length; i++) {
        if ($.isNumeric(timeperiodarray[i])) {
            timeperiodarray[i] = timeperiodarray[i] - 1;
        }
    }
    PreviousTimePeriod = timeperiodarray.join(" ");
    if (TimePeriod.split('|')[0].toString().toUpperCase() == "3MMT" || TimePeriod.split('|')[0].toString().toUpperCase() == "6MMT" || TimePeriod.split('|')[0].toString().toUpperCase() == "12MMT") {
        if (TimePeriod.split('|').length > 1)
            sCurrent_PreviousTime = TimePeriod.split('|')[1].toString().toUpperCase() + " " + TimePeriod.split('|')[0].toString().toUpperCase() + " V/S " + PreviousTimePeriod.toString().toUpperCase();
    }
    else {
        if (TimePeriod.split('|').length > 1)
            sCurrent_PreviousTime = TimePeriod.split('|')[1].toString().toUpperCase() + " V/S " + PreviousTimePeriod.toString().toUpperCase();
    }
    disablePrimaryShopper();
    ShowSelectedFilters();
}
function disablePrimaryShopper() {
    var year = 2020;
    var month = 6; // From July
    var filterparent = ".FilterPopup.AdvancedFilters";//.FilterPopup.GroupType
    if (currentpage.indexOf("dashboard") > 0)
        filterparent = ".FilterPopup.GroupType";
    $(filterparent).find("li[name='Primary Shopper']").removeClass("in-active-item");
    var flag = false;
    if (ModuleBlock == "TREND") {
        for (var i = 0; i < TrendCustomBaselist.length; i++) {
            var date = TrendCustomBaselist[i].Name.toString();
            date = date.replace("Q1", "Mar").replace("Q2", "Jun").replace("Q3", "Sep").replace("Q4", "Dec")
            date = date.length == 4 ? "Dec " + date : date;
            date = new Date(date.toString())
            if (date.getFullYear() > year) {
                flag = true;
                break;
            }
            else if (date.getFullYear() == year && date.getMonth() >= month)//0-11 --> 6 means from july 
            {
                flag = true;
                break;
            }

        }
    }
    else {
        if (TimePeriodId == 1)
            flag = true;
        else {
            var date = TimePeriod.split('|')[1].toString();
            date = date.replace("Q1", "Mar").replace("Q2", "Jun").replace("Q3", "Sep").replace("Q4", "Dec")
            date = date.length == 4 ? "Dec " + date : date;
            date = new Date(date.toString())
            if (date.getFullYear() > year)
                flag = true;
            else if (date.getFullYear() == year && date.getMonth() >= month)//0-11 --> 6 means from july 
                flag = true;
        }
    }
    if (flag) {
        $(filterparent).find("li[name='Primary Shopper']").addClass("in-active-item");
        $(filterparent).find("li[parentname='Primary Shopper']").removeClass("Selected");
        if (currentpage.indexOf("dashboard") > 0) {
            custombase_AddFilters = custombase_AddFilters.filter(function (x) { return x.parentName != "Primary Shopper" });
            Grouplist = Grouplist.filter(function (x) { return x.parentName != "Primary Shopper" });
        }
        else
            SelectedDempgraphicList = SelectedDempgraphicList.filter(function (x) { return x.parentName != "Primary Shopper" });
    }
}
//function LoadFilters() {  
//    ShowLoader();
//    window.app.db.get(1, function (saveData) {       
//        if (saveData !== null && saveData !== undefined) {
//            var data = JSON.parse(saveData.value);
//            PrepareFilters(data);
//            HideLoader();
//        }
//        else 
//        {
//            var module = currentpage.split("-");
//            postBackData = "{modulename:'" + module[0] + "-" + module[1] + "'}";
//            jQuery.ajax({
//                type: "POST",
//                url: $("#URLCommon").val(),
//                async: true,
//                data: postBackData,
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    var saveData = {
//                        id: 1,
//                        value: JSON.stringify(data)
//                    };
//                    window.app.db.save(saveData, function () { });
//                    //sessionStorage.setItem('filters', JSON.stringify(data));
//                    PrepareFilters(data);

//                },
//                error: function (xhr, status, error) {
//                }
//            });
//        }
//    });

//    //if (sessionStorage.getItem('filters') === null) {

//    //    var module = currentpage.split("-");
//    //    postBackData = "{modulename:'" + module[0] + "-" + module[1] + "'}";
//    //    jQuery.ajax({
//    //        type: "POST",
//    //        url: $("#URLCommon").val(),
//    //        async: true,
//    //        data: postBackData,
//    //        contentType: "application/json; charset=utf-8",
//    //        dataType: "json",
//    //        success: function (data) {
//    //            var saveData = {
//    //                id: 'IShopFilters',
//    //                value: JSON.stringify(data)
//    //            };
//    //            window.app.save(saveData, function () { });
//    //            //sessionStorage.setItem('filters', JSON.stringify(data));
//    //            PrepareFilters(data);

//    //        },
//    //        error: function (xhr, status, error) {
//    //        }
//    //    });

//    //}
//    //else {
//    //    var data = JSON.parse(sessionStorage.getItem('filters'));

//    //    PrepareFilters(data);
//    //}
//}

function PrepareFilters2(data) {
    sFilterData = data;
    AllRetailers = [];
    AllBeverages = [];
    DefaultGeolist = SelecDefaultGeography(data);
    if (currentpage == "hdn-analysis-acrossshopper") {
        //added by Nagaraju for  Beverage html string Date: 12-04-2017
        //LoadBeverageOrCategory(data);
        //LoadBeverages(data);
        $("#BevDivId").append(data.Category.SearchObj.HTML_String);
        AllBeverages = data.Category.SearchObj.SearchItems;
        SearchFilters("Beverage", "Search-Beverages", "Beverage-Search-Content", AllBeverages);
        //------->
        LoadChannelOrCategory(data);
        //LoadRetailerOrBrand(data);
        $("#Beverages").show();
    }
    else if (currentpage == "hdn-dashboard-pathtopurchase") {
        LoadChannelOrCategory(data);
        PreparePathToPurchaseCustombaseFilters(data);
    }
    else if (currentpage == "hdn-dashboard-demographic") {
        LoadChannelOrCategory(data);
        LoadAdvancedFilterFromString(data);
        PreparePathToPurchaseCustombaseFilters(data);
    }
    else if (currentpage == "hdn-analysis-acrosstrips") {
        LoadChannelOrCategory(data);
        //LoadRetailerOrBrand(data);
        LoadGeographyFilters();
        //LoadSecondaryGeographyFilters();
        LoadDefaultSecondaryGeographyFilters();
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel1").text("Retailers");
        $("#GroupType").show();
        $("#Left-Frequency").show();
        $("#GeographyFilters").show();
    }
    else if (currentpage == "hdn-analysis-withintrips") {
        LoadChannelOrCategory(data);
        //LoadRetailerOrBrand(data);
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel1").text("Retailers");
        $("#GroupType").show();
        //$("#Left-Frequency").show();
        $("#Advanced-Analytics-Select-Variable").show();
        //$("#GeographyFilters").show();
    }
    else if ((currentpage.indexOf("beverage") > 0)) {
        //added by Nagaraju for  Beverage html string Date: 12-04-2017
        //LoadBeverageOrCategory(data);
        //LoadBeverages(data);
        $("#BevDivId").append(data.Category.SearchObj.HTML_String);
        AllBeverages = data.Category.SearchObj.SearchItems;
        SearchFilters("Beverage", "Search-Beverages", "Beverage-Search-Content", AllBeverages);
        //------->

        $("#Beverages").show();
        $("#Retailers").hide();
        $(".FilterPopup.Beverages").css('display', 'none');
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel1").text("Beverages");
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        $("#Retailers").hide();
        $("#AdvancedFilters").show();
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel1").text("Retailers");
        $("#GroupType").show();
        $("#TotalMeasure").show();
        $(".LowerRightContent").show();
        //LoadTotalMeasuresFilters(data);
        //LoadSecondaryTotalMeasuresFilters(data);
        //added by Nagaraju for HTML measure
        //Date: 13-04-2017
        $("#total-measure-trip").html(data.TotalTripHTMLMeasure.SearchObj.HTML_String);
        //$("#total-measure-shopper").html(data.TotalShopperHTMLMeasure.SearchObj.HTML_String);
        AllTotalMeasures = data.TotalTripHTMLMeasure.SearchObj.SearchItems;
        ShopperTotalMeasures = data.TotalTripHTMLMeasure.SearchObj.ShopperSearchItems;
        TripsTotalMeasures = data.TotalTripHTMLMeasure.SearchObj.TripsSearchItems;
        //AllTotalMeasures = AllTotalMeasures.concat(data.TotalShopperHTMLMeasure.SearchObj.SearchItems);
        SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", AllTotalMeasures);
    }
    else {
        $("#Retailers").show();
        LoadChannelOrCategory(data);
        //LoadRetailerOrBrand(data);
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel1").text("Retailers");
    }


    LoadTimePeriod(data);
    //LoadAdvancedFilters(data);
    //LoadSecondaryAdvancedFilters(data);
    LoadAdvancedFilterFromString(data);

    //LoadTotalMeasuresFilters(data);
    //LoadSecondaryTotalMeasuresFilters(data);
    LoadGroupsPrimeFilters(data);
    //LoadGroupTypeHeaderName(data);
    //LoadGroupTypeNames(data);
    //added by Nagaraju for Groups Date: 11-04-2017
    if (currentpage.indexOf("beverage") > -1) {
        $("#GroupTypeGeoContentSub").before(data.Beverages_GroupsFilterlist.SearchObj.HTML_String);
        AllTypes = data.Beverages_GroupsFilterlist.SearchObj.SearchItems;
    }
    else {
        $("#GroupTypeGeoContentSub").before(data.Retailers_GroupsFilterlist.SearchObj.HTML_String);
        AllTypes = data.Retailers_GroupsFilterlist.SearchObj.SearchItems;
        //added by Nagaraju 
        //Date: 27-06-2017
        if (currentpage == "hdn-report-retailersshopperdeepdive")
            AllTypes = data.Retailers_GroupsFilterlist.SearchObj.ReportsSearchItems;
    }
    //add Geography
    if (currentpage != "hdn-analysis-acrosstrips") {
        $("#GroupTypeHeaderContent ul").append("<li style=\"\" primefiltertype=\"Demographics\" filtertype=\"Demographics\" name=\"Geography\" dbname=\"Geography\" uniqueid=\"null\" shopperdbname=\"null\" tripsdbname=\"null\" class=\"gouptype\" onclick=\"SelecGroup(this);\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%;\" class=\"lft-popup-ele-label\" filetrtypeid=\"1\" id=\"100\" type=\"Main-Stub\" name=\"Geography\">Geography</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>");
        $("#GroupTypeContent").append("<div class=\"DemographicList\" id=\"100\" name=\"Geography\" fullname=\"Geography\" style=\"overflow-y: auto;\"></div>");

        UpdateDefaultGeography(data);
    }
    SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);
    //----->

    //LoadMeasureTypeMain(data);
    //LoadMeasureTypeHeaderName(data);
    //LoadMeasureTypeNames(data);
    if (currentpage.indexOf("hdn-chart") > -1) {
        //LoadMeasureShopperTripLevel();
        //LoadMeasure(data);
        //added by Nagaraju for Measure Date: 11-04-2017
        if ((currentpage.indexOf("retailer") > 0)) {
            $("#MeasureTypeHeaderMainTrip").append(data.SelTypelist[0].html1);
            $("#MeasureTypeHeaderContentTrip").append(data.SelTypelist[0].html2);
            $("#MeasureTypeHeaderContentSubLevelTrip").append(data.SelTypelist[0].html3);
            $("#MeasureTypeContentTrip").append(data.SelTypelist[0].html4);

            $("#MeasureTypeHeaderMainTrip").show();
            $("#MeasureTypeHeaderMainTrip ul li").show();

            //$("#MeasureTypeHeaderMainShopper").append(data.SelTypelist[1].html1);
            //$("#MeasureTypeHeaderContentShopper").append(data.SelTypelist[1].html2);
            //$("#MeasureTypeHeaderContentSubLevelShopper").append(data.SelTypelist[1].html3);
            //$("#MeasureTypeContentShopper").append(data.SelTypelist[1].html4);

            AllMeasures = data.SelTypelist[0].SearchObj.SearchItems;
            ShopperMeasureSearchItems = data.SelTypelist[0].SearchObj.ShopperSearchItems;
            TripsMeasureSearchItems = data.SelTypelist[0].SearchObj.TripsSearchItems;
            //AllMeasures = AllMeasures.concat(data.SelTypelist[1].SearchObj.SearchItems);
        }
        else {
            $("#MeasureTypeHeaderMainTrip").append(data.SelTypelist[1].html1);
            $("#MeasureTypeHeaderContentTrip").append(data.SelTypelist[1].html2);
            $("#MeasureTypeHeaderContentSubLevelTrip").append(data.SelTypelist[1].html3);
            $("#MeasureTypeContentTrip").append(data.SelTypelist[1].html4);

            $("#MeasureTypeHeaderMainTrip").show();
            $("#MeasureTypeHeaderMainTrip ul li").show();

            //$("#MeasureTypeHeaderMainShopper").append(data.SelTypelist[3].html1);
            //$("#MeasureTypeHeaderContentShopper").append(data.SelTypelist[3].html2);
            //$("#MeasureTypeHeaderContentSubLevelShopper").append(data.SelTypelist[3].html3);
            //$("#MeasureTypeContentShopper").append(data.SelTypelist[3].html4);

            AllMeasures = data.SelTypelist[1].SearchObj.SearchItems;
            ShopperMeasureSearchItems = data.SelTypelist[1].SearchObj.ShopperSearchItems;
            TripsMeasureSearchItems = data.SelTypelist[1].SearchObj.TripsSearchItems;
            //AllMeasures = AllMeasures.concat(data.SelTypelist[3].SearchObj.SearchItems);
        }
        SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
    }
    if (currentpage == "hdn-crossretailer-totalrespondentstripsreport")
        LoadTotalMeasureShopperTripLevel();
    if (currentpage == "hdn-analysis-withintrips" || currentpage == "hdn-analysis-withinshopper")
        LoadOthersMeasureShopperTripLevel();
    LoadFrequencyFilter(data);
    LoadSecondaryChannelFilters(data);
    if (currentpage.indexOf("hdn-analysis") > -1) {
        //LoadAdvancedAnalyticsFilter(data);
        LoadAdvancedAnalyticsFilterTrips(data);
    }

    LoadRightFilter();

    LoadNonBeverageFilter(data);
    if (sFilterData == null || sFilterData == "" || sFilterData.length <= 0 || sFilterData == {} || sFilterData.Measure == undefined) {
        LoadFilterData();
        LoadRightPanel(sFilterData);
        if (currentpage == "hdn-chart-comparebeverages" || currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive" || currentpage == "hdn-chart-beveragedeepdive" || currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
            $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
        }
        if (currentpage.indexOf("beverage") > 0) {
            LoadMonthlyFilters(sFilterData);
        }
        else {
            LoadFrequencyFilter(sFilterData);
        }

        LoadChannelFilters(sFilterData);
        LoadBeverageSlectionTypeFilter(sFilterData);
    }
    else {
        LoadRightPanel(sFilterData);
        if (currentpage.indexOf("beverage") > 0) {
            LoadMonthlyFilters(sFilterData);
        }
        else {
            LoadFrequencyFilter(sFilterData);
        }

        LoadChannelFilters(sFilterData);
        LoadBeverageSlectionTypeFilter(sFilterData);
    }

    LoadLeftPanelFrequencyFilter(data);
    for (var i = 0; i < data.Channel.RetailersFilterlist.SearchObj.SearchItems.length; i++) {
        var _rowobj = data.Channel.RetailersFilterlist.SearchObj.SearchItems[i].split('|')
        if (_rowobj[1] == "Total Shopper") {
            data.Channel.RetailersFilterlist.SearchObj.SearchItems.splice(i, 1);
            break;
        }
    }

    if (currentpage.indexOf("deepdive") > -1 || currentpage.indexOf("totalrespondentstripsreport") > -1
        || currentpage.indexOf("hdn-analysis-withintrips") > -1)
        LoadMeasureShopperTripLevel();

    if (currentpage == "hdn-report-compareretailersshoppers" || currentpage == "hdn-report-retailersshopperdeepdive")
        SearchFilters("Retailer", "Search-Retailers", "Retailer-Search-Content", data.Channel.RetailersFilterlist.SearchObj.SearchItems);
    if (currentpage == "hdn-analysis-crossretailerfrequencies")
        SearchFilters("Retailer", "Search-Retailers", "Retailer-Search-Content", filterSearchListBasedOnView(data.Channel.RetailersFilterlist.SearchObj.SearchItems));
    else if (currentpage == "hdn-analysis-crossretailerimageries")
        SearchFilters("Retailer", "Search-Retailers", "Retailer-Search-Content", filterSearchListBasedOnView(data.Channel.RetailersFilterlist.SearchObj.SearchItems));
    else {
        //SearchFilters("Retailer", "Search-Retailers", "Retailer-Search-Content", AllRetailers);
        SearchFilters("Retailer", "Search-Retailers", "Retailer-Search-Content", filterSearchListBasedOnView(data.Channel.RetailersFilterlist.SearchObj.SearchItems));
        SearchFilters("Custombase-Retailers", "Search-Custombase-Retailers", "Custombase-Retailer-Search-Content", filterSearchListBasedOnView(data.Channel.RetailersFilterlist.SearchObj.SearchItems));
    }

    SearchFilters("CompetitorFrequency-Retailers", "Search-CompetitorFrequency-Retailers", "CompetitorFrequency-Retailer-Search-Content", filterSearchListBasedOnView(data.Channel.RetailersFilterlist.SearchObj.SearchItems));
    //added by Nagaraju for hiding Total Visits
    //date: 21-04-2017  
    HideTotalVisits();
    SearchFilters("Beverage", "Search-Beverages", "Beverage-Search-Content", AllBeverages);
    SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
    SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);
    SearchFilters_RightPanel("Frequency", "Search-Left-FrequencyFilters", "FreqFilter-Left-Search-Content", AllFrequency);
    SearchFilters("AdvancedAnalytics", "Search-Left-Advanced-AnalyticsFilters", "Advanced-AnalyticsFilter-Left-Search-Content", AllAdvancedAnalytics);
    SetDefaultValues();
    SetFilterLayerScroll();
}
//added by Nagaraju for adding frequency in adv filters
//date: 21-04-2017  
function AddFrequencyinAdvFilters() {
    if (currentpage == "hdn-report-compareretailerspathtopurchase" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {

        if ($(".Advmaindiv #PrimaryAdvancedFilterContentAdv #PrimaryDemoFilterListAdv li div[id='-1']").length <= 0) {
            html = "<li style=\"display:table;\"><div onclick=\"DisplaySecondaryDemoFilterAdv(this);\" name=\"FREQUENCY\" id=\"-1\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"-1\" data-val=\"FREQUENCY\" data-parent=\"\" data-isselectable=\"true\">FREQUENCY</span><div class=\"ArrowContainerdiv\" style=\"left:1%;\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
            $(".Advmaindiv #PrimaryAdvancedFilterContentAdv #PrimaryDemoFilterListAdv ul").append(html);


            html = "<div class=\"DemographicList Frequency\" id=\"-1\" name=\"FREQUENCY\" fullname=\"FREQUENCY\" style=\"overflow-y: auto; display: block;\"><ul>";

            for (var i = 0; i < sFilterData.ReportTripsFrequencylist.length; i++) {
                var object = sFilterData.ReportTripsFrequencylist[i];
                //if (object.Name.toLocaleLowerCase() != "total visits") {
                //}
                html += "<div onclick=\"SelectFrequency(this);\" class=\"lft-popup-ele lft-popup-ele-label FrequencyItem\"  id=\"" + object.UniqueId + "\" style=\"display:" + (object.Name.toLocaleLowerCase() == "total visits" ? "none" : "block") + ";\">";
                html += "<span class=\"lft-popup-ele-label\" id=\"" + object.UniqueId + "\" fullname=\"" + object.Name.toUpperCase() + "\"";
                html += "isgeography=\"null\" uniqueid=\"" + object.UniqueId + "\"";
                html += "name=\"" + object.Name.toUpperCase() + "\" parent=\"-1\" parentlevelid=\"-1\" parentlevelname=\"FREQUENCY\"";
                html += "data-isselectable=\"true\">" + object.Name.toUpperCase() + "</span></div>";
                //AllDemographics.push(object.UniqueId + "|" + object.Name);

            }
            html += "</ul></div>";
            $(".Advmaindiv #SecondaryAdvancedFilterContentAdv #SecondaryDemoFilterListAdv").append(html);
        }
    }
}
function HideTotalVisits() {
    $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").parent("li").hide();
    for (var i = 0; i < AllFrequency.length; i++) {
        if (AllFrequency[i].toLocaleLowerCase().indexOf("total visits") > -1) {
            AllFrequency.splice(i, 1);
        }
    }
}
function SetTripsDefaultFrequency() {
    var freqL = "";
    if (SelectedFrequencyList.length == 0) {
        if (TabType.toLocaleLowerCase() == "trips" || currentpage == "hdn-report-compareretailerspathtopurchase"
            || currentpage == "hdn-report-retailerspathtopurchasedeepdive" || currentpage == "hdn-dashboard-demographic") {
            var object = new Object();
            if (TabType.toLocaleLowerCase() == "shopper" && currentpage == "hdn-dashboard-demographic") {
                if ($("#default-shopper-frequency").val() != null && DemogFlag != 0 && $("#default-shopper-frequency").val() != "10") {
                    freqL = $("#default-shopper-frequency").val();
                }
                else {
                    freqL = "2";
                }
                object = $(".AdvancedFilters .Frequency ul div>span[uniqueid='" + freqL + "']");
                $(".AdvancedFilters .Frequency ul div>span[uniqueid='" + freqL + "']").parent().removeClass("Selected");
                $(".AdvancedFilters .Frequency ul div>span[uniqueid='" + freqL + "']").parent().trigger("click");
                //$("#groupDivId span[parentname=FREQUENCY][uniqueid='" + freqL + "']").parent("li").trigger("click");
                $("#groupDivId li[filtertype=FREQUENCY][parentid='" + freqL + "'][name='SELECTION']").trigger("click");
                return;
            }
            else if (currentpage == "hdn-report-compareretailerspathtopurchase" || currentpage == "hdn-report-retailerspathtopurchasedeepdive" || currentpage == "hdn-dashboard-pathtopurchase") {
                object = $(".Left-Frequency #left-panel-frequency ul li li[name='TOTAL VISITS']").find(".lft-popup-ele-label");
            }
            else {
                object = $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").find(".lft-popup-ele-label");
            }
            SelectedFrequencyList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("uniqueid") });
        }
    }
}
//
function LoadLeftPanelFrequencyFilter(data) {
    html = "";
    AllLeftPanelFrequency = [];
    var frequencyfilterlist = [];
    if (currentpage == "hdn-analysis-acrossshopper")
        frequencyfilterlist = data.BGMFrequencylist;
    else if (currentpage == "hdn-analysis-crossretailerimageries")
        frequencyfilterlist = data.Frequencylist.splice(0, 5);
    else if (currentpage == "hdn-report-compareretailerspathtopurchase"
        || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
        frequencyfilterlist = data.ReportTripsFrequencylist;
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport")
        frequencyfilterlist = data.Frequencylist.splice(0, 5);
    else if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
        frequencyfilterlist = data.Frequencylist.splice(5, 2);
    }
    else
        frequencyfilterlist = data.ReportFrequencylist;

    if (data != null) {
        html += "<ul>";
        for (var i = 0; i < frequencyfilterlist.length; i++) {
            var object = frequencyfilterlist[i];
            html += "<li Name=\"" + object.Name.toUpperCase() + "\" style=\"display:table;\">";
            html += "<div onclick=\"SelectFrequency(this);\" Name=\"" + object.Name.toUpperCase() + "\" class=\"\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.Name + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";
            html += "</li>";
            AllLeftPanelFrequency.push(object.Id + "|" + object.Name);
        }
        html += "</ul>";
        $("#left-panel-frequency").html("");
        $("#left-panel-frequency").append(html);
    }
    SearchFilters_RightPanel("Left-Panel-Frequency", "Search-Left-Panel-FrequencyFilters", "FreqFilter-Left-Search-Content", AllLeftPanelFrequency);
}

function LoadChannelOrCategory(data) {
    $("#RetailerDivId").html("");
    $("#RetailerDivId").html(data.Channel.RetailersFilterlist.SearchObj.HTML_String.toString());
    filterChannelBasedOnView();
    //html = "";
    //var index = 0;
    //var ulclose = false;
    //var ImageDetails = GetBeverageImagePosition();
    //if (data != null)
    //{       
    //    //add lavels
    //    for (var i = 0; i < data.Channel.Lavels.length; i++)
    //    {
    //        $("<div class=\"Retailer Lavel Lavel" + data.Channel.Lavels[i] + " Sub-Lavel\" style=\"display: none;\"></div>").appendTo(".Retailers .RetailerDiv");
    //    }
    //    html += "<ul>";
    //    for (var i = 0; i < data.Channel.ChannelOrCategorylist.length; i++)
    //    {         
    //        var object = data.Channel.ChannelOrCategorylist[i];
    //        var sImageClassName = "";
    //        sImageClassName = _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; })[0].imagePosition : "";

    //        html += "<li>";
    //        html += "<div>";
    //        if (sImageClassName == "")
    //            html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>";
    //        else
    //        html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>";
    //        html += "<span id=\"" + object.UniqueId + "\" type=\"Main-Stub\" priorityid=\"1\" Name=\"" + object.Name + "\" isselectable=\"" + object.IsSelectable + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"Comparison\" onclick=\"SelectComparison(this);\">" + object.Name + "</span>";
    //        html += "<span Name=\"" + object.Name + "\" lavels=\"" + object.ChannelOrCategoryLavel.length + "\" class=\"sidearrw\" onclick=\"DisplayComparisonRetailer(this);\"></span>";
    //        if (!IsItemExist(object.Name, AllRetailers) && object.isSelectable != "false")
    //            AllRetailers.push(object.UniqueId + "|" + object.Name);

    //        html += "</div>";
    //        html += "</li>";

    //    }       
    //        html += "</ul>";

    //    $("#ChannelOrCategoryContent").html("");
    //    $("#ChannelOrCategoryContent").html(html);
    //}
}

//-Abhay
function filterSearchListBasedOnView(dataList) {
    switch (currentpage) {
        case "hdn-analysis-crossretailerimageries":
            var removableitem = ["1|Total", "3|Supermarket/Grocery", "4|Convenience", "5|Drug", "6|Dollar", "7|Club", "8|Mass Merc.", "9|Supercenter", "331|Corporate Nets", "11|AUSA Corporate", "333|AUSA Corporate"];
            $.map(removableitem, function (item) {
                dataList.splice(dataList.indexOf(item), 1);
            })
            break;
        case "hdn-analysis-acrossshopper":
            var removableitem = ["1|Total", "331|Corporate Nets"];
            $.map(removableitem, function (item) {
                dataList.splice(dataList.indexOf(item), 1);
            })
            break;
        case "hdn-analysis-crossretailerfrequencies":
            var removableitem = ["331|Corporate Nets"];
            $.map(removableitem, function (item) {
                dataList.splice(dataList.indexOf(item), 1);
            })
            break;
    }
    return dataList;
}

function IsItemExist(item, array) {
    for (var i = 0; i < array.length; i++) {
        if (array[i].split("|")[1] == item) {
            return true;
            break;
        }
    }
    return false;
}
function LoadRetailerOrBrand(data) {
    html = "";
    var shtml = "";
    var index = 0; var sindex = 0; var ssindex = 0;
    var sHeadingName = "";
    if (data != null) {
        for (var i = 0; i < data.Channel.ChannelOrCategorylist.length; i++) {
            for (var lavel = 0; lavel < data.Channel.ChannelOrCategorylist[i].ChannelOrCategoryLavel.length; lavel++) {
                if (data.Channel.ChannelOrCategorylist[i].ChannelOrCategoryLavel[lavel].LavelRetailerlist.length > 0) {
                    index = 0; sindex = 0; ssindex = 0;
                    for (var j = 0; j < data.Channel.ChannelOrCategorylist[i].ChannelOrCategoryLavel[lavel].LavelRetailerlist.length; j++) {
                        var object = data.Channel.ChannelOrCategorylist[i].ChannelOrCategoryLavel[lavel].LavelRetailerlist[j];
                        if (object.LevelId == 3 || object.LevelId == "3") {
                            if (object.PriorityId == "1") {
                                if (index == 0) {
                                    html = "<div class=\"RetailerOrBrand\" id=\"" + object.UniqueId + "\" Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" DBName=\"" + data.Channel.ChannelOrCategorylist[i].DBName + "\" style=\"display:none;\"><ul>";
                                    index++;
                                }
                                html += "<li class=\"Comparison\" parentLevelId=\"" + data.Channel.ChannelOrCategorylist[i].Id + "\" id=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectComparison(this);\" PriorityId=\"" + object.PriorityId + "\">" + object.Name + "</li>";
                                if (!IsItemExist(object.Name, AllPriorityRetailers))
                                    AllPriorityRetailers.push(object.UniqueId + "|" + object.Name);
                            }
                            else {
                                if (sindex == 0) {
                                    shtml = "<div class=\"RetailerOrBrand\" id=\"" + object.UniqueId + "\" Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" DBName=\"" + data.Channel.ChannelOrCategorylist[i].DBName + "\" style=\"display:none;\"><ul>";
                                    sindex++;
                                }
                                shtml += "<li class=\"Comparison\" parentLevelId=\"" + data.Channel.ChannelOrCategorylist[i].Id + "\" id=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectComparison(this);\" PriorityId=\"" + object.PriorityId + "\">" + object.Name + "</li>";

                            }
                        }
                        else {
                            if (ssindex == 0)
                                html = "<div class=\"RetailerOrBrand\" id=\"" + object.UniqueId + "\" Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" DBName=\"" + data.Channel.ChannelOrCategorylist[i].DBName + "\" style=\"display:none;\"><ul>";
                            html += "<li class=\"Comparison\" parentLevelId=\"" + data.Channel.ChannelOrCategorylist[i].Id + "\" id=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectComparison(this);\">" + object.Name + "</li>";
                            if (!IsItemExist(object.Name, AllPriorityRetailers))
                                AllPriorityRetailers.push(object.UniqueId + "|" + object.Name);
                            ssindex++;
                        }
                        if (!IsItemExist(object.Name, AllRetailers) && object.isSelectable != "false")
                            AllRetailers.push(object.UniqueId + "|" + object.Name);
                    }

                    if (sindex > 0) {
                        html += "</ul></div>";
                        shtml += "</ul></div>";
                        $(".Retailers .RetailerDiv .Lavel" + lavel + "").append("<div Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" class=\"priorityclass\" style=\"display:inline-block;min-height: 6.5%;margin-top: 3%;font-size:13px;\">-----------------------PRIORITY-----------------------</div>");
                        $(".Retailers .RetailerDiv .Lavel" + lavel + "").append(html);
                        if (currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive" && currentpage != "hdn-analysis-crossretailerfrequencies" && currentpage != "hdn-analysis-crossretailerimageries") {
                            $(".Retailers .RetailerDiv .Lavel" + lavel + "").append("<div Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\"  class=\"priorityclass\" style=\"display:inline-block;min-height: 6.5%;margin-top: 3%;font-size:13px;\">-------------------NON-PRIORITY-------------------</div>");
                            $(".Retailers .RetailerDiv .Lavel" + lavel + "").append(shtml);
                        }
                    }
                    else {
                        html += "</ul></div>";
                        $(".Retailers .RetailerDiv .Lavel" + lavel + "").append(html);
                    }
                }

            }
        }
    }
}
function LoadBeverageOrCategory(data) {
    html = "";
    var index = 0;
    var ulclose = false;
    if (data != null) {
        var ImageDetails = GetBeverageImagePosition();
        //add lavels
        for (var i = 0; i < data.Category.Lavels.length; i++) {
            $("<div class=\"Beverage Lavel Lavel" + data.Category.Lavels[i] + " Sub-Lavel\" style=\"display: none;height:98%;\" ></div>").appendTo(".BevDiv");
        }
        html += "<ul>";
        for (var i = 0; i < data.Category.CategoryOrBeveragelist.length; i++) {
            var object = data.Category.CategoryOrBeveragelist[i];
            var sImageClassName = _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; })[0].imagePosition : "";
            html += "<li>";
            html += "<div class=\"FilterStringContainerdiv\">";
            if (sImageClassName == "")
                html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>";
            else {
                if (object.Name == "Hard Seltzers and Other Flavored Alcoholic Beverages")
                    html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/hard_seltzers_icons.svg?id=2');background-repeat: no-repeat;background-position:" + sImageClassName + "\"></span>";
                else
                    html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>";
            }
            //html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>";

            if (object.isSelectable == "false")
                html += "<span type=\"Main-Stub\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"Comparison\" isselectable=\"" + object.isSelectable + "\" onclick=\"SelectBevComparison(this);\">" + object.Name + "</span>";
            else
                html += "<span id=\"" + object.UniqueId + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"Comparison\" isselectable=\"" + object.isSelectable + "\" onclick=\"SelectBevComparison(this);\">" + object.Name + "</span>";

            html += "<div class=\"ArrowContainerdiv\"><span Name=\"" + object.Name + "\" lavels=\"" + object.CategoryOrBeverageLavel.length + "\" class=\"sidearrw\" onclick=\"DisplayBevComparisonRetailer(this);\"></span></div>";

            if (!IsItemExist(object.Name, AllBeverages) && object.isSelectable != "false")
                AllBeverages.push(object.UniqueId + "|" + object.Name);

            html += "</div>";
            html += "</li>";

        }
        html += "</ul>";

        $("#BeverageOrCategoryContent").html("");
        $("#BeverageOrCategoryContent").html(html);
        SetScroll($("#BeverageOrCategoryContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
}
function LoadBeverages(data) {
    html = "";
    if (data != null) {
        for (var i = 0; i < data.Category.CategoryOrBeveragelist.length; i++) {
            for (var lavel = 0; lavel < data.Category.CategoryOrBeveragelist[i].CategoryOrBeverageLavel.length; lavel++) {
                if (data.Category.CategoryOrBeveragelist[i].CategoryOrBeverageLavel[lavel].LavelRetailerlist.length > 0) {
                    html = "<div class=\"RetailerOrBrand\" id=\"" + data.Category.CategoryOrBeveragelist[i].UniqueId + "\" Name=\"" + data.Category.CategoryOrBeveragelist[i].Name + "\" DBName=\"" + data.Category.CategoryOrBeveragelist[i].DBName + "\" style=\"display:none;\"><ul>";
                    for (var j = 0; j < data.Category.CategoryOrBeveragelist[i].CategoryOrBeverageLavel[lavel].LavelRetailerlist.length; j++) {
                        var object = data.Category.CategoryOrBeveragelist[i].CategoryOrBeverageLavel[lavel].LavelRetailerlist[j];


                        html += "<li class=\"Comparison\" id=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectBevComparison(this);\">" + object.Name + "</li>";

                        if (!IsItemExist(object.Name, AllBeverages) && object.isSelectable != "false")
                            AllBeverages.push(object.UniqueId + "|" + object.Name);
                    }
                    html += "</ul></div>";
                    $(".Beverages .Lavel" + lavel + "").append(html);
                }
            }
        }
    }
}

//added by Nagaraju for Groups prime filters Date: 04-03-2017
function LoadGroupsPrimeFilters(data) {
    html = "";
    html += "<ul>";
    var primegroups = [];
    if (currentpage.indexOf("beverage") > -1) {
        if (currentpage == "hdn-report-retailerspathtopurchasedeepdive" || currentpage == "hdn-report-retailersshopperdeepdive")
            primegroups = _.filter(sFilterData.TripsGroupsPrimeFilterlist, function (o) {
                return (o.PrimeFilterType != 'Shopper Frequency')
            });
        else
            primegroups = data.TripsGroupsPrimeFilterlist;
    }
    else {
        if (currentpage == "hdn-report-retailerspathtopurchasedeepdive" || currentpage == "hdn-analysis-acrosstrips" || currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-crossretailer-totalrespondentstripsreport")
            primegroups = _.filter(sFilterData.ShopperGroupsPrimeFilterlist, function (o) {
                return (o.PrimeFilterType != 'Shopper Frequency')
            });
        if (currentpage == "hdn-dashboard-demographic") {
            primegroups = _.filter(sFilterData.ShopperGroupsPrimeFilterlist, function (o) {
                return (o.PrimeFilterType == 'Demographics')
            });
        }
        else
            primegroups = data.ShopperGroupsPrimeFilterlist;
    }

    if (currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive") {
        if (currentpage.indexOf("beverage") > -1) {
            primegroups = _.filter(sFilterData.TripsGroupsPrimeFilterlist, function (o) {
                return (o.PrimeFilterType != 'Pre Shop' && o.PrimeFilterType != 'In Store' && o.PrimeFilterType != 'In Store - Beverage Detail' && o.PrimeFilterType != 'Post Shop/Trip Summary')
            });
        }
        else {
            primegroups = _.filter(sFilterData.ShopperGroupsPrimeFilterlist, function (o) {
                return (o.PrimeFilterType != 'Pre Shop' && o.PrimeFilterType != 'In Store' && o.PrimeFilterType != 'In Store - Beverage Detail' && o.PrimeFilterType != 'Post Shop/Trip Summary')
            });
        }
    }
    else if (currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
        primegroups = _.filter(sFilterData.ShopperGroupsPrimeFilterlist, function (o) {
            return (o.PrimeFilterType != 'Shopper Frequency')
        });
    }
    else if (currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-analysis-acrosstrips") {
        if (currentpage.indexOf("beverage") > -1) {
            primegroups = _.filter(sFilterData.TripsGroupsPrimeFilterlist, function (o) {
                return (o.PrimeFilterType != 'Pre Shop' && o.PrimeFilterType != 'Shopper Frequency' && o.PrimeFilterType != 'In Store' && o.PrimeFilterType != 'In Store - Beverage Detail' && o.PrimeFilterType != 'Post Shop/Trip Summary')
            });
        }
        else {
            primegroups = _.filter(sFilterData.ShopperGroupsPrimeFilterlist, function (o) {
                return (o.PrimeFilterType != 'Pre Shop' && o.PrimeFilterType != 'Shopper Frequency' && o.PrimeFilterType != 'In Store' && o.PrimeFilterType != 'In Store - Beverage Detail' && o.PrimeFilterType != 'Post Shop/Trip Summary')
            });
        }
    }
    for (var i = 0; i < primegroups.length; i++) {
        var object = primegroups[i];
        if (currentpage != "hdn-dashboard-pathtopurchase") {
            if (object.PrimeFilterType == "Shopper Frequency")
                html += "<li PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" onclick=\"ShowGroup(this);\" title=\"This Group is valid only for Priority Retailers.\">";
            else
                html += "<li PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" onclick=\"ShowGroup(this);\">";
            html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
            html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\">" + object.PrimeFilterType + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
            html += "</div>";
            html += "</li>";
        }
        else {
            if (object.PrimeFilterType != "Shopper Frequency") {
                html += "<li PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" onclick=\"ShowGroup(this);\">";
                html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
                html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\">" + object.PrimeFilterType + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
                html += "</div>";
                html += "</li>";
            }
        }

    }
    if (currentpage == "hdn-dashboard-pathtopurchase") {
        html += "<li PrimeFilterType=\"Frequency\" FilterType=\"Frequency\" onclick=\"ShowGroup(this);\">";
        html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
        html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\">Frequency</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
        html += "</div>";
        html += "</li>";
    }
    html += "</ul>";
    $("#PrimeGroupTypeHeaderContent").html("");
    $("#PrimeGroupTypeHeaderContent").html(html);
    SetScroll($("#PrimeGroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

    $(".Geotooltipimage, #PrimeGroupTypeHeaderContent li[PrimeFilterType='Shopper Frequency']").hover(function () {
        // Hover over code      
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function ShowGroup(obj) {
    $("#PrimeGroupTypeHeaderContent ul li").removeClass("Selected");
    $("#PrimeGroupTypeHeaderContent ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");

    if ($(obj).attr("primefiltertype") == "Shopper Frequency") {
        Grouplist = [];
        $("#GroupTypeHeaderContent li[onclick='SelecGroupMetricName(this);']").removeClass("Selected");
    }

    $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").removeClass("sidearrw_OnCLick");
    $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").addClass("sidearrw");

    $(obj).children("div").find(".lft-popup-ele-next").removeClass("sidearrw");
    $(obj).children("div").find(".lft-popup-ele-next").addClass("sidearrw_OnCLick");

    $("#GroupTypeContent").hide();
    $("#GroupTypeContentSub").hide();
    $("#GroupTypeHeaderContent ul li").hide();
    if ($(obj).attr("PrimeFilterType").toLocaleLowerCase() == "in store - beverage detail") {
        $("#GroupTypeHeaderContent ul li[PrimeFilterType='" + $(obj).attr("PrimeFilterType") + "'][FilterType='Extra Beverages']").show();
    }
    $("#GroupTypeHeaderContent ul li[PrimeFilterType='" + $(obj).attr("PrimeFilterType") + "'][FilterType='" + $(obj).attr("FilterType") + "']").show();
    $("#GroupTypeHeaderContent").show();

    $("#grouptypeHeadingLevel3").hide();
    $("#grouptypeHeadingLevel4").hide();
    $("#GroupTypeGeoContentSub").hide();

    $("#grouptypeHeadingLevel2").html($(obj).attr("PrimeFilterType").toLowerCase());
    $("#grouptypeHeadingLevel2").show();

    ShowSelectedFilters();

    $(".GroupType").css("width", "auto")

}


function LoadGroupTypeHeaderName(data) {
    LoadGroupsPrimeFilters(data);
    if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
        html = "";
        var index = 0;
        var datalist = {};
        if (currentpage.indexOf("beverage") > -1) {
            if (currentpage != "hdn-analysis-acrosstrips") {
                dGeo = SelecGeography();
                datalist = (data.TripGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
            }
            else if (currentpage == "hdn-analysis-acrosstrips") {
                if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                    dGeo = SelecGeography();
                    datalist = (data.TripGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
                }
                else
                    datalist = datalist = (data.TripGroupTypelist).concat(dGeo);//data.TripGroupTypelist;
            }
            else
                datalist = data.TripGroupTypelist;
        }
        else {
            if (currentpage != "hdn-analysis-acrosstrips") {
                dGeo = SelecGeography();
                datalist = (data.ShopperGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
            }
            else if (currentpage == "hdn-analysis-acrosstrips") {
                if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                    dGeo = SelecGeography();
                    datalist = (data.ShopperGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
                }
                else
                    datalist = (data.ShopperGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
            }
            else
                datalist = data.ShopperGroupTypelist;
            //data.ShopperGroupTypelist//[data.ShopperGroupTypelist.length - 1] = d[0];
            //data.ShopperGroupTypelist = d.concat(data.ShopperGroupTypelist);
            //End
        }
        var ulclose = false;
        //var ImageDetails = GetDemographyImagePosition();
        if (data != null) {

            html += "<ul>";
            for (var i = 0; i < datalist.length; i++) {
                var object = datalist[i];
                var sImageClassName = "";// _.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; })[0].imagePosition : "";
                if (object.Level == "1") {
                    //    html += "<li Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"SelecGroup(this);\">";
                    //else
                    html += "<li style=\"display:none;\" PrimeFilterType=\"" + (object.Name == "Geography" ? "Demographics" : object.PrimeFilterType) + "\" FilterType=\"" + (object.Name == "Geography" ? "Demographics" : object.FilterType) + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"SelecGroup(this);\">";
                    html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
                    html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";

                    html += "</div>";
                    html += "</li>";
                }
            }


            AllLeftPanelFrequency = [];
            var frequencyfilterlist = [];
            if (currentpage == "hdn-analysis-acrossshopper")
                frequencyfilterlist = data.BGMFrequencylist;
            else if (currentpage == "hdn-analysis-crossretailerimageries")
                frequencyfilterlist = data.Frequencylist.splice(0, 5);
            else if (currentpage == "hdn-report-compareretailerspathtopurchase"
                || currentpage == "hdn-report-retailerspathtopurchasedeepdive"
                || currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
                frequencyfilterlist = data.ReportTripsFrequencylist;
            }
            else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport")
                frequencyfilterlist = data.Frequencylist.splice(0, 5);
            else
                frequencyfilterlist = data.ReportFrequencylist;

            if (data != null) {
                html += "<ul>";
                for (var i = 0; i < frequencyfilterlist.length; i++) {
                    var object = frequencyfilterlist[i];
                    html += "<li Name=\"" + object.Name.toUpperCase() + "\" style=\"display:table;\">";
                    html += "<div onclick=\"SelectFrequency(this);\" Name=\"" + object.Name.toUpperCase() + "\" class=\"\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.Name + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";
                    html += "</li>";
                    AllLeftPanelFrequency.push(object.Id + "|" + object.Name);
                }
            }

            html += "</ul>";
            $("#GroupTypeHeaderContent").html("");
            $("#GroupTypeHeaderContent").html(html);
            SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);


        }
    }
}
function LoadDashboardGroupTypeHeaderName(data) {
    if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
        html = "";
        AllLeftPanelFrequency = [];
        var frequencyfilterlist = [];
        if (currentpage == "hdn-analysis-acrossshopper")
            frequencyfilterlist = data.BGMFrequencylist;
        else if (currentpage == "hdn-analysis-crossretailerimageries")
            frequencyfilterlist = data.Frequencylist.splice(0, 5);
        else if (currentpage == "hdn-report-compareretailerspathtopurchase"
            || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
            frequencyfilterlist = data.ReportTripsFrequencylist;
        }
        else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
            frequencyfilterlist = data.Frequencylist.splice(0, 5);
        }
        else if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
            frequencyfilterlist = data.Frequencylist;
        }
        else {
            frequencyfilterlist = data.ReportFrequencylist;
        }

        //var ImageDetails = GetDemographyImagePosition();
        if (data != null && frequencyfilterlist != undefined && frequencyfilterlist != null) {

            //html += "<ul>";
            for (var i = 0; i < frequencyfilterlist.length; i++) {
                var object = frequencyfilterlist[i];
                var sImageClassName = "";// _.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; })[0].imagePosition : "";
                //if (object.Level == "1") {
                //    html += "<li Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"SelecGroup(this);\">";
                //else
                if (object.Name.toUpperCase() != "TOTAL VISITS") {
                    html += "<li style=\"display:none;\" PrimeFilterType=\"" + (object.Name == "Geography" ? "Demographics" : "Frequency") + "\" FilterType=\"" + (object.Name == "Geography" ? "Demographics" : "Frequency") + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"SelectFrequency(this);\">";
                    html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
                    html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\" FullName=\"" + object.Name + "\" UniqueId=\"" + object.UniqueId + "\">" + object.Name + "</span>";

                    html += "</div>";
                    html += "</li>";
                }
                // }
            }

            $("#GroupTypeHeaderContent ul").append(html);
            SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

        }
    }
}
//added by Nagaraju D for geography Date: 07-03-2017
function UpdateGeography() {
    var obj_geo = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var index = 0;
    var ulclose = false;
    var datalist = {};
    var indexSubLevel = 0;
    var datalist = {};
    datalist = SelecGeography();

    for (var i = 0; i < datalist.length; i++) {
        //html += "<div class=\"DemographicList\" id=\"" + datalist[i].Id + "\" Name=\"" + datalist[i].Name + "\" FullName=\"" + datalist[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
        html = "<ul>";
        thirdLevelhtml = "<div class=\"DemographicList\" id=\"" + datalist[i].Id + "\" Name=\"" + datalist[i].Name + "\" FullName=\"" + datalist[i].FullName + "\" style=\"display:none;\"><ul>";
        fourthLevelhtml = "";
        for (var j = 0; j < datalist[i].SecondaryAdvancedFilterlist.length; j++) {
            var object = datalist[i].SecondaryAdvancedFilterlist[j];
            if (datalist[i].Level == "1") {
                //if (data.AdvancedFilterlist[i].Name != "Other")               
                var k = _.filter(datalist, function (u) {
                    return u.Name.toUpperCase() == object.Name.toUpperCase();
                });
                if (k.length <= 0) {
                    // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                    if (object.isGeography == "true") {
                        if (object.active != "false") {
                            html += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            if (!IsItemExist(object.Name, AllTypes))
                                AllTypes.push(object.UniqueId + "|" + object.Name);
                        }
                        else
                            html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                    else {
                        if (object.active != "false") {
                            //added by Nagaraju for Beverage shopper
                            if (object.SecondaryAdvancedFilterlist.length > 0) {
                                html += "<ul><li class=\"gouptype\" MetricId=\"" + object.MetricId + "\" style=\"\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"DisplayThirdLevelDemoFilter(this);\">";
                                html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
                                html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";

                                html += "</div>";
                                html += "</li></ul>";
                                for (var k = 0; k < object.SecondaryAdvancedFilterlist.length; k++) {
                                    var objthirdlavel = object.SecondaryAdvancedFilterlist[k];
                                    thirdLevelhtml += "<div MetricId=\"" + object.MetricId + "\" style=\"display: none;\" id=\"" + object.Id + "\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + objthirdlavel.Name + "\" MericName=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + objthirdlavel.isGeography + "\" FullName=\"" + objthirdlavel.FullName + "\" DBName=\"" + objthirdlavel.DBName + "\" UniqueId=\"" + objthirdlavel.UniqueId + "\" shopperdbname=\"" + objthirdlavel.ShopperDBName + "\" tripsdbname=\"" + objthirdlavel.TripsDBName + "\"  data-id=\"" + objthirdlavel.Id + "\" id=" + objthirdlavel.Id + "-" + objthirdlavel.MetricId + "-" + objthirdlavel.ParentId + " Name=\"" + objthirdlavel.Name + "\" parent=\"" + objthirdlavel.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + objthirdlavel.Name + "</span></div>";
                                    if (!IsItemExist(object.Name, AllTypes))
                                        AllTypes.push(objthirdlavel.UniqueId + "|" + objthirdlavel.Name);
                                }
                            }
                            else {
                                html += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                if (!IsItemExist(object.Name, AllTypes))
                                    AllTypes.push(object.UniqueId + "|" + object.Name);
                            }
                        }
                        else
                            html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                }
                else {
                    if (object.isGeography == "true") {
                        if (object.active != "false")
                            html += "<div Uniqueid=\"" + object.Id + "\" Name=\"" + object.Name + "\" onclick=\"DisplayThirdLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        else
                            html += "<div Name=\"" + object.Name + "\" onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                    else {
                        if (object.active != "false")
                            html += "<div Uniqueid=\"" + object.Id + "\" Name=\"" + object.Name + "\" onclick=\"DisplayThirdLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        else
                            html += "<div Name=\"" + object.Name + "\" onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                }
            }
            else {
                var object = datalist[i].SecondaryAdvancedFilterlist[j];
                if (datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                    if (object.isGeography == "true")
                        thirdLevelhtml += "<div Name=\"" + object.Name + "\" onclick=\"DisplayForthLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    else
                        thirdLevelhtml += "<div Name=\"" + object.Name + "\" onclick=\"DisplayForthLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    //if (indexSubLevel <= 0) {
                    fourthLevelhtml = "<div class=\"DemographicList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                    obj_geo = object.Name;
                    //    indexSubLevel++;
                    //}
                    for (var l = 0; l < datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                        var object1 = datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                        if (object1.active != "false") {
                            if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                            else
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                            if (!IsItemExist(object.Name, AllTypes))
                                AllTypes.push(object1.UniqueId + "|" + object1.Name);
                        }
                        else {
                            if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                fourthLevelhtml += "<div Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                            else
                                fourthLevelhtml += "<div Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                        }
                    }
                    fourthLevelhtml += "</ul></div>";
                    $("#GroupTypeGeoContentSub .DemographicList[name='" + obj_geo + "']").remove();
                    $("#GroupTypeGeoContentSub").append(fourthLevelhtml);
                }
                else {
                    thirdLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                    if (!IsItemExist(object.Name, AllTypes))
                        AllTypes.push(object.UniqueId + "|" + object.Name);
                }
            }

            //html += "<li filetrtypeid=\"" + object.FilterTypeId + "\" parentLevelId=\"" + datalist[i].Id + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" Name=\"" + object.Name + "\" class=\"gouptype\" onclick=\"SelecGroupMetricName(this);\">" + object.Name + "</li>";



            //AllComparisonBanners.push(object.UniqueId + "|" + object.Name);
        }
        html += "</ul>";
        thirdLevelhtml += "</ul></div>";

        $("#GroupTypeContentSub .DemographicList[name='" + datalist[i].Name + "']").remove();
        $("#GroupTypeContentSub").append(thirdLevelhtml);

    }

    $("#GroupTypeContent .DemographicList[name='Geography']").html(html);
    SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    SetScroll($("#GroupTypeGeoContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    $(".Geotooltipimage, #MeasureTypeShopperTripHeader li[name='Shopper Measures']").hover(function () {
        // Hover over code      
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        //added by Nagaraju for last layer tool tip
        //Date: 25-05-2017
        if ((mousex + 100) > $("#RightPanel").width()) {
            $('.GeoToolTip').css({ top: mousey, left: mousex - 250 })
        }
        else {
            $('.GeoToolTip').css({ top: mousey, left: mousex })
        }
    });
    $(".lft-popup-ele").mouseover(function () {
        if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
    });
    $(".lft-popup-ele").mouseleave(function () {
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
    });
}
function UpdateDefaultGeography(data) {
    var obj_geo = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var index = 0;
    var ulclose = false;
    var datalist = {};
    var indexSubLevel = 0;
    var datalist = {};
    datalist = DefaultGeolist;

    for (var i = 0; i < datalist.length; i++) {
        //html += "<div class=\"DemographicList\" id=\"" + datalist[i].Id + "\" Name=\"" + datalist[i].Name + "\" FullName=\"" + datalist[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
        html = "<ul>";
        thirdLevelhtml = "<div class=\"DemographicList\" id=\"" + datalist[i].Id + "\" Name=\"" + datalist[i].Name + "\" FullName=\"" + datalist[i].FullName + "\" style=\"display:none;\"><ul>";
        fourthLevelhtml = "";
        for (var j = 0; j < datalist[i].SecondaryAdvancedFilterlist.length; j++) {
            var object = datalist[i].SecondaryAdvancedFilterlist[j];
            if (datalist[i].Level == "1") {
                //if (data.AdvancedFilterlist[i].Name != "Other")               
                var k = _.filter(datalist, function (u) {
                    return u.Name.toUpperCase() == object.Name.toUpperCase();
                });
                if (k.length <= 0) {
                    // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                    if (object.isGeography == "true") {
                        if (object.active != "false") {
                            html += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            if (!IsItemExist(object.Name, AllTypes))
                                AllTypes.push(object.UniqueId + "|" + object.Name);
                        }
                        else
                            html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                    else {
                        if (object.active != "false") {
                            //added by Nagaraju for Beverage shopper
                            if (object.SecondaryAdvancedFilterlist.length > 0) {
                                html += "<ul><li class=\"gouptype\" MetricId=\"" + object.MetricId + "\" style=\"\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"DisplayThirdLevelDemoFilter(this);\">";
                                html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
                                html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";

                                html += "</div>";
                                html += "</li></ul>";
                                for (var k = 0; k < object.SecondaryAdvancedFilterlist.length; k++) {
                                    var objthirdlavel = object.SecondaryAdvancedFilterlist[k];
                                    thirdLevelhtml += "<div MetricId=\"" + object.MetricId + "\" style=\"display: none;\" id=\"" + object.Id + "\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + objthirdlavel.Name + "\" MericName=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + objthirdlavel.isGeography + "\" FullName=\"" + objthirdlavel.FullName + "\" DBName=\"" + objthirdlavel.DBName + "\" UniqueId=\"" + objthirdlavel.UniqueId + "\" shopperdbname=\"" + objthirdlavel.ShopperDBName + "\" tripsdbname=\"" + objthirdlavel.TripsDBName + "\"  data-id=\"" + objthirdlavel.Id + "\" id=" + objthirdlavel.Id + "-" + objthirdlavel.MetricId + "-" + objthirdlavel.ParentId + " Name=\"" + objthirdlavel.Name + "\" parent=\"" + objthirdlavel.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + objthirdlavel.Name + "</span></div>";
                                    if (!IsItemExist(object.Name, AllTypes))
                                        AllTypes.push(objthirdlavel.UniqueId + "|" + objthirdlavel.Name);
                                }
                            }
                            else {
                                html += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                if (!IsItemExist(object.Name, AllTypes))
                                    AllTypes.push(object.UniqueId + "|" + object.Name);
                            }
                        }
                        else
                            html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                }
                else {
                    if (object.isGeography == "true") {
                        if (object.active != "false")
                            html += "<div Uniqueid=\"" + object.Id + "\" Name=\"" + object.Name + "\" onclick=\"DisplayThirdLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        else
                            html += "<div Name=\"" + object.Name + "\" onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                    else {
                        if (object.active != "false")
                            html += "<div Uniqueid=\"" + object.Id + "\" Name=\"" + object.Name + "\" onclick=\"DisplayThirdLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        else
                            html += "<div Name=\"" + object.Name + "\" onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                }
            }
            else {
                var object = datalist[i].SecondaryAdvancedFilterlist[j];
                if (datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                    if (object.isGeography == "true")
                        thirdLevelhtml += "<div Name=\"" + object.Name + "\" onclick=\"DisplayForthLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    else
                        thirdLevelhtml += "<div Name=\"" + object.Name + "\" onclick=\"DisplayForthLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    //if (indexSubLevel <= 0) {
                    fourthLevelhtml = "<div class=\"DemographicList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                    obj_geo = object.Name;
                    //    indexSubLevel++;
                    //}
                    for (var l = 0; l < datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                        var object1 = datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                        if (object1.active != "false") {
                            if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                            else
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                            if (!IsItemExist(object.Name, AllTypes))
                                AllTypes.push(object1.UniqueId + "|" + object1.Name);
                        }
                        else {
                            if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                fourthLevelhtml += "<div Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                            else
                                fourthLevelhtml += "<div Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                        }
                    }
                    fourthLevelhtml += "</ul></div>";
                    $("#GroupTypeGeoContentSub .DemographicList[name='" + obj_geo + "']").remove();
                    $("#GroupTypeGeoContentSub").append(fourthLevelhtml);
                }
                else {
                    thirdLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                    if (!IsItemExist(object.Name, AllTypes))
                        AllTypes.push(object.UniqueId + "|" + object.Name);
                }
            }

            //html += "<li filetrtypeid=\"" + object.FilterTypeId + "\" parentLevelId=\"" + datalist[i].Id + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" Name=\"" + object.Name + "\" class=\"gouptype\" onclick=\"SelecGroupMetricName(this);\">" + object.Name + "</li>";



            //AllComparisonBanners.push(object.UniqueId + "|" + object.Name);
        }
        html += "</ul>";
        thirdLevelhtml += "</ul></div>";

        $("#GroupTypeContentSub .DemographicList[name='" + datalist[i].Name + "']").remove();
        $("#GroupTypeContentSub").append(thirdLevelhtml);

    }

    $("#GroupTypeContent .DemographicList[name='Geography']").html(html);
    SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    SetScroll($("#GroupTypeGeoContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    $(".Geotooltipimage, #MeasureTypeShopperTripHeader li[name='Shopper Measures']").hover(function () {
        // Hover over code      
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
    $(".lft-popup-ele").mouseover(function () {
        if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
    });
    $(".lft-popup-ele").mouseleave(function () {
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
    });
}
function UpdateAdvancedFilterGeography() {

    html = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var DataList = [];
    var AllDemographicsSub = [];
    dGeo = SelecGeography();
    DataList = dGeo;
    if (DataList != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].SecondaryAdvancedFilterlist.length > 0) {
                html += "<div class=\"DemographicList GeoList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
                thirdLevelhtml += "<div class=\"DemographicList GeoList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"display:none;\"><ul>";
                for (var j = 0; j < DataList[i].SecondaryAdvancedFilterlist.length; j++) {
                    var object = DataList[i].SecondaryAdvancedFilterlist[j];
                    if (DataList[i].Level == "1") {
                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(DataList, function (u) {
                            return u.Name.toUpperCase() == object.Name.toUpperCase();
                        });
                        if (k.length <= 0) {
                            if (object.active != "false") {
                                html += "<div onclick=\"SelectDemographic(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                AllDemographicsSub.push(object.UniqueId + "|" + object.Name);
                            }
                            else {
                                if (object.isGeography == "true")
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            }
                        }
                        else {
                            if (object.isGeography == "true") {
                                if (object.active != "false")
                                    html += "<div onclick=\"DisplayThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            }
                            else {
                                if (object.active != "false")
                                    html += "<div onclick=\"DisplayThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            }
                        }
                    }
                    else {
                        var object = DataList[i].SecondaryAdvancedFilterlist[j];
                        if (DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                            if (object.isGeography == "true")
                                thirdLevelhtml += "<div onclick=\"DisplayFourthLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                thirdLevelhtml += "<div onclick=\"DisplayFourthLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            //if (indexSubLevel <= 0) {
                            fourthLevelhtml += "<div class=\"DemographicList GeoList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                            //    indexSubLevel++;
                            //}
                            for (var l = 0; l < DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                                var object1 = DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                                if (object1.active != "false") {
                                    if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    AllDemographicsSub.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else {
                                    if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                        fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                }
                            }
                            fourthLevelhtml += "</ul></div>";
                        }
                        else {
                            thirdLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"ThirdLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            AllDemographicsSub.push(object.UniqueId + "|" + object.Name);
                        }


                    }
                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }
    AllDemographics.push.apply(AllDemographics, AllDemographicsSub);
    //$("#SecondaryDemoFilterList").html("");
    $("#SecondaryDemoFilterList .GeoList").remove();
    $("#SecondaryDemoFilterList").append(html);
    //$("#ThirdDemoFilterList").html("");
    $("#ThirdDemoFilterList .GeoList").remove();
    $("#ThirdDemoFilterList").append(thirdLevelhtml);
    //$("#FourthDemoFilterList").html("");
    $("#FourthDemoFilterList .GeoList").remove();
    $("#FourthDemoFilterList").append(fourthLevelhtml);

    //SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
    //SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);

    $('.Geotooltipimage').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
    $(".lft-popup-ele").mouseover(function () {
        if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
    });
    $(".lft-popup-ele").mouseleave(function () {
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
    });
}
//added by Nagaraju for default advanced geography filters
//date: 13-04-2017
function UpdateDefaultAdvancedFilterGeography() {

    html = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var DataList = [];
    var AllDemographicsSub = [];
    // dGeo = SelecGeography();
    DataList = DefaultGeolist;
    //DataList = dGeo[0].SecondaryAdvancedFilterlist;
    if (DataList != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].SecondaryAdvancedFilterlist != null && DataList[i].SecondaryAdvancedFilterlist.length > 0) {
                html += "<div class=\"DemographicList GeoList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
                thirdLevelhtml += "<div class=\"DemographicList GeoList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"display:none;\"><ul>";
                for (var j = 0; j < DataList[i].SecondaryAdvancedFilterlist.length; j++) {
                    var object = DataList[i].SecondaryAdvancedFilterlist[j];
                    if (DataList[i].Level == "1") {
                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(DataList, function (u) {
                            return u.Name.toUpperCase() == object.Name.toUpperCase();
                        });
                        if (k.length <= 0) {
                            if (object.active != "false") {
                                html += "<div onclick=\"SelectDemographic(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                AllDemographicsSub.push(object.UniqueId + "|" + object.Name);
                            }
                            else {
                                if (object.isGeography == "true")
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            }
                        }
                        else {
                            if (object.isGeography == "true") {
                                if (object.active != "false")
                                    html += "<div onclick=\"DisplayThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            }
                            else {
                                if (object.active != "false")
                                    html += "<div onclick=\"DisplayThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            }
                        }
                    }
                    else {
                        var object = DataList[i].SecondaryAdvancedFilterlist[j];
                        if (DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                            if (object.isGeography == "true")
                                thirdLevelhtml += "<div onclick=\"DisplayFourthLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                thirdLevelhtml += "<div onclick=\"DisplayFourthLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            //if (indexSubLevel <= 0) {
                            fourthLevelhtml += "<div class=\"DemographicList GeoList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                            //    indexSubLevel++;
                            //}
                            for (var l = 0; l < DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                                var object1 = DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                                if (object1.active != "false") {
                                    if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    AllDemographicsSub.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else {
                                    if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                        fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                }
                            }
                            fourthLevelhtml += "</ul></div>";
                        }
                        else {
                            thirdLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"ThirdLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            AllDemographicsSub.push(object.UniqueId + "|" + object.Name);
                        }


                    }
                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }
    AllDemographics.push.apply(AllDemographics, AllDemographicsSub);
    //$("#SecondaryDemoFilterList").html("");
    $("#SecondaryDemoFilterList .GeoList").remove();
    $("#SecondaryDemoFilterList").append(html);
    //$("#ThirdDemoFilterList").html("");
    $("#ThirdDemoFilterList .GeoList").remove();
    $("#ThirdDemoFilterList").append(thirdLevelhtml);
    //$("#FourthDemoFilterList").html("");
    $("#FourthDemoFilterList .GeoList").remove();
    $("#FourthDemoFilterList").append(fourthLevelhtml);

    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
    //SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);

    $('.Geotooltipimage').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function LoadGroupTypeNames(data) {
    html = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var index = 0;
    var ulclose = false;
    var datalist = {};
    var indexSubLevel = 0;
    if (currentpage.indexOf("beverage") > -1) {
        if (currentpage != "hdn-analysis-acrosstrips") {
            dGeo = SelecGeography();
            datalist = (data.TripGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
        }
        else if (currentpage == "hdn-analysis-acrosstrips") {
            if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                dGeo = SelecGeography();
                datalist = (data.TripGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
            }
            else
                datalist = datalist = (data.TripGroupTypelist).concat(dGeo);//data.TripGroupTypelist;
        }
        else
            datalist = data.TripGroupTypelist;
    }
    else {
        if (currentpage != "hdn-analysis-acrosstrips") {
            dGeo = SelecGeography();
            datalist = (data.ShopperGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
        }
        else if (currentpage == "hdn-analysis-acrosstrips") {
            if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                dGeo = SelecGeography();
                datalist = (data.ShopperGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
            }
            else
                datalist = (data.ShopperGroupTypelist).concat(dGeo);//data.ShopperGroupTypelist;
        }
        else
            datalist = data.ShopperGroupTypelist;
        //data.ShopperGroupTypelist//[data.ShopperGroupTypelist.length - 1] = d[0];
        //data.ShopperGroupTypelist = d.concat(data.ShopperGroupTypelist);
        //End
    }

    if (currentpage == "hdn-analysis-acrosstrips" || currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive") {
        if (currentpage.indexOf("beverage") > -1) {
            if (currentpage != "hdn-analysis-acrosstrips") {
                dGeo = SelecGeography();
                var sData = _.filter(data.TripGroupTypelist, function (o) {
                    return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary' && o.FilterType != "Visits")
                });
                datalist = (sData).concat(dGeo);//data.ShopperGroupTypelist;
            }
            else if (currentpage == "hdn-analysis-acrosstrips") {
                if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                    dGeo = SelecGeography();
                    var sData = _.filter(data.TripGroupTypelist, function (o) {
                        return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary' && o.FilterType != "Visits")
                    });
                    datalist = (sData).concat(dGeo);//data.ShopperGroupTypelist;
                }
                else {
                    var sData = _.filter(data.TripGroupTypelist, function (o) {
                        return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary && o.FilterType != "Visits"')
                    });
                    datalist = datalist = (sData).concat(dGeo);//data.TripGroupTypelist;
                }
            }
            else {
                var sData = _.filter(data.TripGroupTypelist, function (o) {
                    return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary' && o.FilterType != "Visits")
                });
                datalist = sData;
            }
        }
        else {
            if (currentpage != "hdn-analysis-acrosstrips") {
                dGeo = SelecGeography();
                var sData = _.filter(data.ShopperGroupTypelist, function (o) {
                    return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary' && o.FilterType != "Visits")
                });
                datalist = (sData).concat(dGeo);//data.ShopperGroupTypelist;
            }
            else if (currentpage == "hdn-analysis-acrosstrips") {
                if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                    dGeo = SelecGeography();
                    var sData = _.filter(data.ShopperGroupTypelist, function (o) {
                        return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary' && o.FilterType != "Visits")
                    });
                    datalist = (sData).concat(dGeo);//data.ShopperGroupTypelist;
                }
                else {
                    var sData = _.filter(data.ShopperGroupTypelist, function (o) {
                        return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary' && o.FilterType != "Visits")
                    });
                    datalist = (sData).concat(dGeo);//data.ShopperGroupTypelist;
                }
            }
            else {
                var sData = _.filter(data.ShopperGroupTypelist, function (o) {
                    return (o.FilterType != 'Pre Shop' && o.FilterType != 'In Store' && o.FilterType != 'In Store - Beverage Detail' && o.FilterType != 'Post Shop/Trip Summary' && o.FilterType != "Visits")
                });
                datalist = sData;
            }
            //data.ShopperGroupTypelist//[data.ShopperGroupTypelist.length - 1] = d[0];
            //data.ShopperGroupTypelist = d.concat(data.ShopperGroupTypelist);
            //End
        }
    }


    if (datalist != null) {

        for (var i = 0; i < datalist.length; i++) {
            html += "<div class=\"DemographicList\" id=\"" + datalist[i].Id + "\" Name=\"" + datalist[i].Name + "\" FullName=\"" + datalist[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
            thirdLevelhtml += "<div class=\"DemographicList\" id=\"" + datalist[i].Id + "\" Name=\"" + datalist[i].Name + "\" FullName=\"" + datalist[i].FullName + "\" style=\"display:none;\"><ul>";

            for (var j = 0; j < datalist[i].SecondaryAdvancedFilterlist.length; j++) {
                var object = datalist[i].SecondaryAdvancedFilterlist[j];
                if (datalist[i].Level == "1") {
                    //if (data.AdvancedFilterlist[i].Name != "Other")                
                    var k = _.filter(datalist, function (u) {
                        return u.Name.toUpperCase() == object.Name.toUpperCase();
                    });
                    if (datalist[i].Level == "1") {
                        // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                        if (object.isGeography == "true") {
                            if (object.active != "false") {
                                html += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                if (!IsItemExist(object.Name, AllTypes))
                                    AllTypes.push(object.UniqueId + "|" + object.Name);
                            }
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                        else {
                            if (object.active != "false") {
                                //added by Nagaraju for Beverage shopper
                                if (object.SecondaryAdvancedFilterlist.length > 0) {
                                    html += "<ul><li class=\"gouptype\" MetricId=\"" + object.MetricId + "\" style=\"\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"DisplayThirdLevelDemoFilter(this);\">";
                                    html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
                                    html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";

                                    html += "</div>";
                                    html += "</li></ul>";
                                    for (var k = 0; k < object.SecondaryAdvancedFilterlist.length; k++) {
                                        var objthirdlavel = object.SecondaryAdvancedFilterlist[k];
                                        thirdLevelhtml += "<div MetricId=\"" + object.MetricId + "\" style=\"display: none;\" id=\"" + object.Id + "\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + objthirdlavel.Name + "\" MericName=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + objthirdlavel.isGeography + "\" FullName=\"" + objthirdlavel.FullName + "\" DBName=\"" + objthirdlavel.DBName + "\" UniqueId=\"" + objthirdlavel.UniqueId + "\" shopperdbname=\"" + objthirdlavel.ShopperDBName + "\" tripsdbname=\"" + objthirdlavel.TripsDBName + "\"  data-id=\"" + objthirdlavel.Id + "\" id=" + objthirdlavel.Id + "-" + objthirdlavel.MetricId + "-" + objthirdlavel.ParentId + " Name=\"" + objthirdlavel.Name + "\" parent=\"" + objthirdlavel.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + objthirdlavel.Name + "</span></div>";
                                        if (!IsItemExist(object.Name, AllTypes))
                                            AllTypes.push(objthirdlavel.UniqueId + "|" + objthirdlavel.Name);
                                    }
                                }
                                else {
                                    html += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                    if (!IsItemExist(object.Name, AllTypes))
                                        AllTypes.push(object.UniqueId + "|" + object.Name);
                                }
                            }
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                    }
                    else {
                        if (object.isGeography == "true") {
                            if (object.active != "false")
                                html += "<div Uniqueid=\"" + object.Id + "\" Name=\"" + object.Name + "\" onclick=\"DisplayThirdLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                html += "<div Name=\"" + object.Name + "\" onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width: 83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                        else {
                            if (object.active != "false")
                                html += "<div Uniqueid=\"" + object.Id + "\" Name=\"" + object.Name + "\" onclick=\"DisplayThirdLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                html += "<div Name=\"" + object.Name + "\" onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                    }
                }
                else {
                    var object = datalist[i].SecondaryAdvancedFilterlist[j];
                    if (datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                        if (object.isGeography == "true")
                            thirdLevelhtml += "<div Name=\"" + object.Name + "\" onclick=\"DisplayForthLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        else
                            thirdLevelhtml += "<div Name=\"" + object.Name + "\" onclick=\"DisplayForthLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        //if (indexSubLevel <= 0) {
                        fourthLevelhtml += "<div class=\"DemographicList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                        //    indexSubLevel++;
                        //}
                        for (var l = 0; l < datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                            var object1 = datalist[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                            if (object1.active != "false") {
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                if (!IsItemExist(object.Name, AllTypes))
                                    AllTypes.push(object1.UniqueId + "|" + object1.Name);
                            }
                            else
                                fourthLevelhtml += "<div Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                        }
                        fourthLevelhtml += "</ul></div>";
                    }
                    else {
                        thirdLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                        if (!IsItemExist(object.Name, AllTypes))
                            AllTypes.push(object.UniqueId + "|" + object.Name);
                    }
                }

                //html += "<li filetrtypeid=\"" + object.FilterTypeId + "\" parentLevelId=\"" + datalist[i].Id + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" Name=\"" + object.Name + "\" class=\"gouptype\" onclick=\"SelecGroupMetricName(this);\">" + object.Name + "</li>";



                //AllComparisonBanners.push(object.UniqueId + "|" + object.Name);
            }
            html += "</ul></div>";
            thirdLevelhtml += "</ul></div>";
            fourthLevelhtml += "</ul></div>";
        }

        $("#GroupTypeContent").html("");
        $("#GroupTypeContent").html(html);
        SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        $("#GroupTypeContentSub").html("");
        $("#GroupTypeContentSub").html(thirdLevelhtml);
        SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        $("#GroupTypeGeoContentSub").html("");
        $("#GroupTypeGeoContentSub").html(fourthLevelhtml);
        SetScroll($("#GroupTypeGeoContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
    SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);
    $(".Geotooltipimage, #MeasureTypeShopperTripHeader li[name='Shopper Measures']").hover(function () {
        // Hover over code      
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function SelecGeography(obj) {
    var Module = "";
    if ((currentpage.indexOf("beverage") > 0))
        Module = "beverage";
    else
        Module = "within";

    //added by Nagaraju for TREND
    //Date: 12-07-2017
    if (ModuleBlock == "TREND")
        Module = "time";

    var SplitTimePeriod = "";
    var TimperiodForGeography = "";
    if (Module == "Time") {
        //SplitTimePeriod = GetBenchmarkTimePeriod();
        //TimperiodForGeography = SplitTimePeriod.split(' ')[1];
    }
    else {
        if (TimePeriod == "total|total") {
            TimeExtension = "Total Time";
        }
        else if (TimeExtension.toLowerCase() == "ytd") {
            if (TimePeriod.indexOf('|') > -1) {
                SplitTimePeriod = TimePeriod.split('|')[1];
                TimperiodForGeography = SplitTimePeriod.split(' ')[1];
            }
        }
        else if (TimeExtension.toLowerCase() == "year") {
            if (TimePeriod.indexOf('|') > -1) {
                TimperiodForGeography = TimePeriod.split('|')[1];
            }

        }
            //else if (TimeExtension == "Quarter" || TimeExtension == "3MMT" || TimeExtension == "6MMT" || TimeExtension == "12MMT") {
        else if (TimeExtension.toLowerCase() != "year") {
            if (TimePeriod.indexOf('|') > -1) {
                SplitTimePeriod = TimePeriod.split('|')[1];
                TimperiodForGeography = SplitTimePeriod.split(' ')[0];
            }

        }
    }
    //
    var tableParams = new Object();
    tableParams.TagName = "Geography";//$(obj).attr("name");
    tableParams.TimePeriod = TimperiodForGeography;
    tableParams.TimePeriodType = TimeExtension;
    tableParams.CheckModule = Module;

    var postBackData = "{GeoCustomDetails:" + JSON.stringify(tableParams) + "}"
    var sResult = [];
    // if (sGeographyData == [] || sGeographyData == null || sGeographyData == "") {
    jQuery.ajax({
        type: "POST",
        url: $("#URLCommonRegions").val(),
        async: false,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            sResult = content;
            sGeographyData = sResult;
            //$("#GroupTypeContent").html("");
            //$("#GroupTypeContent").html(content.benchmarklist);
            //$("#GroupTypeContent").show();
            //SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
            //if (content.d != "")
            //LoadWithinBenchComp(content.d);
        },
        error: function (error) {
            showMessage(error);
        }
    });
    //}
    //else
    //    sResult = sGeographyData;
    //if (sResult.length > 0)
    //    sResult = _.forEach(sResult, function (i) {
    //        _.forEach(i.SecondaryAdvancedFilterlist, function (j) {
    //            if (j.SecondaryAdvancedFilterlist == null || j.SecondaryAdvancedFilterlist.length <= 0) {
    //                _.forEach(sFilterData.GeographyList, function (k) {
    //                    if (j.Name == "Total")
    //                        j.UniqueId = "4000";
    //                    else if (j.Name == k.MetricItem)
    //                        j.UniqueId = k.UniqueId;
    //                    j.isGeography = "true";
    //                });
    //            }
    //            else {
    //                _.forEach(j.SecondaryAdvancedFilterlist, function (l) {
    //                    _.forEach(sFilterData.GeographyList, function (k) {
    //                        if (l.Name == "Total")
    //                            l.UniqueId = "4000";
    //                        else if (l.Name == k.MetricItem)
    //                            l.UniqueId = k.UniqueId;
    //                        j.isGeography = "true";
    //                    });
    //                });
    //            }
    //        });
    //    });

    return sResult;
}
function SelecDefaultGeography(data) {
    var sResult = [];
    sResult = data.DefaultGeographyFilters;
    if (sResult.length > 0)
        sResult = _.forEach(sResult, function (i) {
            _.forEach(i.SecondaryAdvancedFilterlist, function (j) {
                if (j.SecondaryAdvancedFilterlist == null || j.SecondaryAdvancedFilterlist.length <= 0) {
                    _.forEach(sFilterData.GeographyList, function (k) {
                        if (j.Name == k.MetricItem)
                            j.UniqueId = k.UniqueId;
                        j.isGeography = "true";
                    });
                }
                else {
                    _.forEach(j.SecondaryAdvancedFilterlist, function (l) {
                        _.forEach(sFilterData.GeographyList, function (k) {
                            if (l.Name == k.MetricItem)
                                l.UniqueId = k.UniqueId;
                            j.isGeography = "true";
                        });
                    });
                }
            });
        });
    return sResult;
}
function DisplayThirdLevelGroupFilter(obj) {
    $("#GroupTypeContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    //Grouplist = [];
    //$("#GroupTypeContentSub div").removeClass("Selected");
    $("#GroupTypeGeoContentSub").hide();
    $("#grouptypeHeadingLevel4").hide();
    $("#GroupTypeContentSub .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });

    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    //$(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $("#GroupTypeContent div[onclick='DisplayThirdLevelGroupFilter(this);']").removeClass("Selected");
    $("#GroupTypeContent div[onclick='DisplayThirdLevelGroupFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    $("#GroupTypeContentSub .DemographicList").hide();
    $("#GroupTypeContentSub").show();
    $("#GroupTypeContentSub div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").text($(obj).find(".lft-popup-ele-label").attr("name"));
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").css("width", "287px");
    SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    ShowSelectedFilters();
    $(".GroupType").css("width", "auto")
}
function DisplayForthLevelGroupFilter(obj) {
    $("#GroupTypeContentSub .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    //Grouplist = [];
    //$("#GroupTypeGeoContentSub div").removeClass("Selected");
    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    //$(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $("#GroupTypeContentSub div[onclick='DisplayForthLevelGroupFilter(this);']").removeClass("Selected");
    $("#GroupTypeContentSub div[onclick='DisplayForthLevelGroupFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    $("#GroupTypeGeoContentSub .DemographicList").hide();
    $("#GroupTypeGeoContentSub").show();
    $("#GroupTypeGeoContentSub div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").text($(obj).find(".lft-popup-ele-label").attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").css("width", "287px");
    SetScroll($("#GroupTypeGeoContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    $("#GroupTypeHeaderContent").getNiceScroll().remove()
    $("#GroupTypeContent").getNiceScroll().remove()
    $("#GroupTypeContentSub").getNiceScroll().remove()
    $("#GroupTypeGeoContentSub").getNiceScroll().remove()
    SetScroll($('.FilterPopup.GroupType'), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    ShowSelectedFilters();
    setTimeout(function () {
        $(".GroupType").css("width", "95%")
    }, 100);
}
function LoadTimePeriod(data) {
    html = "";
    AllTimePeriods = [];
    var indx = 0;
    if (data != null) {
        html += "<ul>";
        for (var i = 0; i < data.TimePeriodlist.length; i++) {
            if (currentpage == "hdn-analysis-establishmentdeepdive" && !(data.TimePeriodlist[i].Name.toUpperCase() == "12MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "YEAR")) {
                continue;
            }
            if (data.TimePeriodlist[i].Name.toUpperCase() == "3MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "12MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "YTD" || data.TimePeriodlist[i].Name.toUpperCase() == "6MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "48MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "36MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "30MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "24MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "18MMT")
                html += "<li name=\"" + data.TimePeriodlist[i].Name + "\" id=\"" + data.TimePeriodlist[i].Id + "\" onclick=\"SelectTimePeriod(this);\" style=\"text-transform:uppercase;\">" + data.TimePeriodlist[i].Name.toLowerCase() + "</li>";
            else
                html += "<li name=\"" + data.TimePeriodlist[i].Name + "\" id=\"" + data.TimePeriodlist[i].Id + "\" onclick=\"SelectTimePeriod(this);\" style=\"text-transform:uppercase;\">" + data.TimePeriodlist[i].Name.toLowerCase() + "</li>";
            if (indx == 3 && ((currentpage != "hdn-analysis-acrossshopper") && (ModuleBlock != "TREND"))) {
                html += "</ul>";
                html += "<ul>";
                indx = -1;
            }
            else if (indx == 4 && ((currentpage == "hdn-analysis-acrossshopper") || (ModuleBlock == "TREND"))) {
                html += "</ul>";
                html += "<ul>";
                indx = 0;
            }
            indx++;
            AllTimePeriods.push({ Id: data.TimePeriodlist[i].Id, Name: data.TimePeriodlist[i].Name, TimePeriods: data.TimePeriodlist[i].TimePeriodlist, Sliderlist: data.TimePeriodlist[i].Sliderlist });
        }
        html += "</ul>";
    }
    $("#TimeBlock").html("");
    $("#TimeBlock").html(html);
}
function SelectGeographyFilter(Value, DatabaseName, storeid, obj, type, _parentid, parentName, SingleOrMultipleSelect) {

}
function ClearOnlyData() {

}
function GetArrayId(obj, Array) {
    for (var i = 0; i < Array.length; i++) {
        if (Array[i].UniqueId == $(obj).attr("uniqueid") || Array[i].Name == $(obj).attr("name")) {
            return i;
            break;
        }
        else if (Array[i].UniqueId == $(obj).find(".lft-popup-ele-label").attr("uniqueid") || Array[i].Name == $(obj).attr("name")) {
            return i;
            break;
        }
    }
    return -1;
}

function GetArrayName(obj, Array) {
    for (var i = 0; i < Array.length; i++) {
        if (Array[i].Name == $(obj).attr("name")) {
            return i;
            break;
        }
        else if (Array[i].Name == $(obj).attr("name")) {
            return i;
            break;
        }
    }
    return -1;
}
function SelectComparison(obj) {
    for (var i = 0; i < $(obj).length; i++) {
        if ($(obj).eq(i).parent().parent().parent().parent().attr("id") == "RetailerDivId" || $(obj).eq(i).parent().parent().parent().parent().parent().attr("id") == "RetailerDivId") {
            obj = $(obj).eq(i);
            break;
        }
    }
    if (currentpage == "hdn-analysis-crossretailerimageries" && ($(obj).attr("name").toString().trim() == "AUSA Corporate"))
        return;
    if (currentpage == "hdn-analysis-acrossshopper" && ($(obj).attr("name").toString().trim() == "Total"))
        return;
    if ($(obj).attr("isselectable") != undefined && $(obj).attr("isselectable") != "" && JSON.parse($(obj).attr("isselectable").toLocaleLowerCase()) == false)
        return false;
    if ($(obj).attr("type") == "Main-Stub" && currentpage == "hdn-analysis-crossretailerimageries") return;
    CompCurrentId = GetArrayId(obj, Comparisonlist);
    CompCurrentName = GetArrayName(obj, Comparisonlist);
    // if (((ModuleBlock == "PIT" && currentpage == "hdn-report-retailerspathtopurchasedeepdive") || (ModuleBlock == "TREND" && currentpage.indexOf("hdn-chart") == -1) || currentpage == "hdn-analysis-crossretailerfrequencies" || currentpage == "hdn-analysis-acrossshopper" || currentpage == "hdn-analysis-withintrips" || currentpage == "hdn-analysis-acrosstrips" || currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic" || (ModuleBlock.toUpperCase() == "TREND" && currentpage.indexOf("hdn-report") > -1))) {
    if ((ModuleBlock == "PIT" || (ModuleBlock == "TREND" && currentpage.indexOf("hdn-chart") == -1) || currentpage == "hdn-analysis-crossretailerfrequencies" || currentpage == "hdn-analysis-acrossshopper" || currentpage == "hdn-analysis-withintrips" || currentpage == "hdn-analysis-establishmentdeepdive" || currentpage == "hdn-analysis-acrosstrips" || currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic" || (ModuleBlock.toUpperCase() == "TREND" && currentpage.indexOf("hdn-report") > -1))) {
        if (CompCurrentId == -1 && CompCurrentName != -1) { return; }
        Comparisonlist = [];
        $(".Retailers .RetailerDiv .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
        $(".Retailers .RetailerDiv .Lavel li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    }
    else {
        if (currentpage != "hdn-analysis-withinshopper" && currentpage != "hdn-analysis-withintrips") {
            if (CompCurrentId == -1 && Comparisonlist.length == 5 && currentpage.indexOf("hdn-report") > -1) {
                showMessage("YOU CAN MAKE UPTO 5 SELECTIONS");
                return false;
            }
            else if (CompCurrentId == -1 && Comparisonlist.length == 11 && currentpage != "hdn-analysis-crossretailerimageries") {
                showMessage("YOU CAN MAKE UPTO 11 SELECTIONS");
                return false;
            }
            else if (CompCurrentId == -1 && Comparisonlist.length == 13 && currentpage == "hdn-analysis-crossretailerimageries") {
                showMessage("YOU CAN MAKE UPTO 13 SELECTIONS");
                return false;
            }
        }
    }

    if (CompCurrentId == -1 && CompCurrentName == -1) {
        var LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");

        if (LevelDescL != undefined && LevelDescL.toLowerCase() == "channel")
            LevelDescL = "Channels";
        else if (LevelDescL != undefined && LevelDescL.toLowerCase() == "retailer")
            LevelDescL = "Retailer";

        Comparisonlist.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid"), PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL });
        CustomBaseFlag = 1;
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Not-Selected-Channel").addClass("Selected");
        }
        else {
            $(obj).addClass("Selected");
        }
        if (currentpage == "hdn-analysis-acrosstrips" && $(obj).attr("priorityid") == "0") {
            $("#Left-Frequency").hide();
            SelectedFrequencyList = [];
        }
        else if (currentpage == "hdn-analysis-acrosstrips" && $(obj).attr("priorityid") == "1") {
            $("#Left-Frequency").show();
            if (SelectedFrequencyList.length <= 0) {
                $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
                $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
            }
        }
    }
    else if (CompCurrentId == -1 && CompCurrentName != -1) { return; }
    else if (CompCurrentId != -1 && CompCurrentName != -1) {
        Comparisonlist.splice(CompCurrentId, 1);
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Selected").addClass("Not-Selected-Channel");
            $(obj).parent("div").parent("li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
        else {
            $(obj).removeClass("Selected");
            $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[name='" + $(obj).attr("name") + "']").removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
    }

    if (ModuleBlock == "TREND" && Comparisonlist.length > 0 && currentpage.indexOf("hdn-report") == -1) {
        if (Comparisonlist.length == 1)
            sTrendType = "1";
        else if (Comparisonlist.length == 2) {
            sTrendType = "2";
            Measurelist = [];
        }
        else if (Comparisonlist.length > 2)
            sTrendType = "2";
    }

    if ($(obj).attr("type") == "Main-Stub") {
        $(".Retailer").hide();
        $("#retailerHeadingLevel2").hide();
        $("#retailerHeadingLevel3").hide();
        $("#ChannelOrCategoryContent .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
    }
    if (currentpage == "hdn-analysis-acrosstrips" && Comparisonlist.length > 0 && Comparisonlist[0].PriorityId == "0") {
        $("#Left-Frequency").hide();
        SelectedFrequencyList = [];
    }
    else if (currentpage == "hdn-analysis-acrosstrips" && Comparisonlist.length > 0 && Comparisonlist[0].PriorityId == "1") {
        $("#Left-Frequency").show();
        if (SelectedFrequencyList.length <= 0) {
            $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
            $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
        }
    }
    else if (currentpage == "hdn-analysis-acrosstrips" && Comparisonlist.length <= 0) {
        $("#Left-Frequency").show();
        if (SelectedFrequencyList.length <= 0) {
            $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
            $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
        }
    }

    var status = 0;
    _.each(Comparisonlist, function (j, k) {
        if (j.PriorityId != "1")
            status = 1;
    });
    if (status == 1) {
        $("#PrimeGroupTypeHeaderContent ul li[primefiltertype='Shopper Frequency']").removeAttr("onclick");
        $("#PrimeGroupTypeHeaderContent ul li[primefiltertype='Shopper Frequency']").css("background-color", "gray");
        $("#PrimeGroupTypeHeaderContent ul li[primefiltertype='Shopper Frequency']").css("cursor", "auto");
    }
    else {
        $("#PrimeGroupTypeHeaderContent ul li[primefiltertype='Shopper Frequency']").attr("onclick", "ShowGroup(this);");
        $("#PrimeGroupTypeHeaderContent ul li[primefiltertype='Shopper Frequency']").css("background-color", "");
        $("#PrimeGroupTypeHeaderContent ul li[primefiltertype='Shopper Frequency']").css("cursor", "pointer");
    }
    if (currentpage == "hdn-dashboard-pathtopurchase") {
        var name = $(obj).attr("parentname") == undefined ? $(obj).attr("name") : $(obj).attr("parentname");
        var cus_obj = $("#Custombase-RetailerDivId div[level-id='1'] ul li span[name='" + name + "']");
        if ($(cus_obj).attr("onclick") != undefined && $(cus_obj).attr("onclick") != "" && $(obj).parent().parent().parent().parent().attr("level-id") != "1") {
            SelectPathToPurchaseCustomBase(cus_obj);
        }
        else {
            SelectPathToPurchaseCustomBase($("#Custombase-RetailerDivId div[level-id='1'] ul li span[name='Total']"));
        }
    }
    ShowSelectedFilters();
    BuildDynamicTable();
}
function RemoveComparison(obj) {
    var CompObj = $(".Retailer .Comparison[UniqueId='" + $(obj).attr("UniqueId") + "']");
    if (CompObj.length <= 0)
        CompObj = $("#RetailerDivId li[uniqueid='" + $(obj).attr("UniqueId") + "']");
    if (CompObj.length <= 0)
        CompObj = $("#ChannelOrCategoryContent ul li span[UniqueId='" + $(obj).attr("UniqueId") + "']");
    if ($(obj).attr("name") == "Total") {
        //CompObj = $("#ChannelOrCategoryContent ul li span[UniqueId='" + $(obj).attr("UniqueId") + "']");
        CompObj = $("#RetailerDivId li[uniqueid='" + $(obj).attr("UniqueId") + "']");
    }
    SelectComparison(CompObj);
}
function SelectSarRetailer(obj) {
    if ($(obj).attr("type") == "Main-Stub") {
        obj = $(obj).parent("div").parent("li")
    }
    if ($(obj).attr("isselectable") != undefined && $(obj).attr("isselectable") != "" && JSON.parse($(obj).attr("isselectable").toLocaleLowerCase()) == false)
        return false;
    if ($(obj).attr("parent-of-parent").trim().toLowerCase() == "custom") {
        if (sarRetailerBenchmarkList.length == 0) {
            showMessage("Please Select BenchMark")
            return false;
        }
        let currentIdInBench = GetArrayId(obj, sarRetailerBenchmarkList);
        let currentNameInBench = GetArrayName(obj, sarRetailerBenchmarkList);
        if (currentIdInBench != -1 || currentNameInBench != -1) {
            showMessage("Already Selected")
            return false;
        }
        let currentCustomId = GetArrayId(obj, sarRetailerCustomList)
        let currentCustomName = GetArrayName(obj, sarRetailerCustomList)
        if (currentCustomId == -1 && currentCustomName == -1) {
            if (sarRetailerCustomList.length == 2) {
                showMessage("You can select upto 2 custom base")
                return false;
            }
            let LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");
            sarRetailerCustomList.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), ParentName: $(obj).attr("parentname"), ParentId: $(obj).attr("parentid"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid"), PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(obj).attr("parent-of-parent") });
            $(obj).addClass("Selected");
            let customBaseFirstHtml = "<li selectedItem='true' donothover='true' id=\"" + $(obj).attr("id") + "\" parent-of-parent=\"" + $(obj).attr("parent-of-parent") + "\" parentid=\"" + $(obj).attr("parentid") + "\" parentname=\"" + $(obj).attr("parentname") + "\" name=\"" + $(obj).attr("name") + "\" uniqueid=\"" + $(obj).attr("uniqueid") + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" id=\"" + $(obj).attr("id") + "\" type=\"Main-Stub\" name=\"" + $(obj).attr("name") + "\" uniqueid=\"" + $(obj).attr("uniqueid") + "\">" + $(obj).attr("name") + "</span><div name=\"" + $(obj).attr("name") + "\"  class=\"closeContainerDiv\"><span class=\"remove-item-selected\" uniqueid=\"" + $(obj).attr("uniqueid") + "\" name=\"" + $(obj).attr("name") + "\" id=\"" + $(obj).attr("id") + "\" parent-of-parent=\"" + $(obj).attr("parent-of-parent") + "\" onclick='RemoveSarRetailer(this);'></span></div></div></li>";
            $("#SarRetailerDivId .level1 ul").append(customBaseFirstHtml)
        }
        else {
            sarRetailerCustomList.splice(currentCustomId, 1);
            $(obj).removeClass("Selected");
            $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[parent-of-parent='" + $(obj).attr("parent-of-parent") + "'][name='" + $(obj).attr("name") + "']").removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#" + $(obj).parent().parent().parent().attr("id") + " .level1 ul li[id='" + $(obj).attr("id") + "'][uniqueid='" + $(obj).attr("uniqueid") + "'][name='" + $(obj).attr("name") + "']").remove();
        }
        ShowSelectedFilters()
        return false;
    }
    else if ($(obj).attr("parent-of-parent").trim().toLowerCase() == "benchmark") {
        let currentBenchId = GetArrayId(obj, sarRetailerBenchmarkList)
        let currentBenchName = GetArrayName(obj, sarRetailerBenchmarkList)
        $.each(sarRetailerCustomList, (index, value) => {
            $("#SarRetailerDivId" + " .Lavel ul li[parent-of-parent='" + value["ParentOfParent"] + "'][name='" + value["Name"] + "']").removeClass("Selected");
            $("#SarRetailerDivId" + " .Lavel ul li[parent-of-parent='" + value["ParentOfParent"] + "'][name='" + value["Name"] + "']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#SarRetailerDivId" + " .level1 ul li[id='" + value["Id"] + "'][uniqueid='" + value["UniqueId"] + "'][name='" + value["Name"] + "']").remove()
        })
        $.each(sarCompetitorList, (index, value) => {
            $("#SarCompetitorDivId" + " .Lavel ul li[parent-of-parent='" + value["ParentOfParent"] + "'][name='" + value["Name"] + "']").removeClass("Selected");
            $("#SarCompetitorDivId" + " .Lavel ul li[parent-of-parent='" + value["ParentOfParent"] + "'][name='" + value["Name"] + "']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#SarCompetitorDivId" + " .level1 ul li[id='" + value["Id"] + "'][uniqueid='" + value["UniqueId"] + "'][name='" + value["Name"] + "']").remove()
        })
        sarRetailerCustomList = []
        sarCompetitorList = []
        if (currentBenchId == -1 && currentBenchName == -1) {
            if (sarRetailerBenchmarkList.length != 0) {

                $("#SarRetailerDivId .Lavel ul li[parent-of-parent='" + sarRetailerBenchmarkList[0]["ParentOfParent"] + "'][name='" + sarRetailerBenchmarkList[0]["Name"] + "']").removeClass("Selected");
                $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
                $("#SarRetailerDivId" + " .level1 ul li[id='" + sarRetailerBenchmarkList[0]["Id"] + "'][uniqueid='" + sarRetailerBenchmarkList[0]["UniqueId"] + "'][name='" + sarRetailerBenchmarkList[0]["Name"] + "']").remove()
                sarRetailerBenchmarkList = []
            }

            if ($(obj).attr("parentname") == "Corporate Nets" || $(obj).attr("parentname") == "Channel Nets") {
                //isChannelSelected = true;
                corporateOrChannelNetSelected = true;
            }
            else {
                corporateOrChannelNetSelected = false;
            }
            if ($(obj).attr("isChannel") == "true") {
                isChannelSelected = true;
            }
            else {
                isChannelSelected = false;
            }
            if ($(obj).attr("priorityid") == "false") {
                isNonPrioritySelected = true
            }
            else {
                isNonPrioritySelected = false
            }
            let LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");
            sarRetailerBenchmarkList.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), ParentName: $(obj).attr("parentname"), ParentId: $(obj).attr("parentid"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid"), PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(obj).attr("parent-of-parent") });
            $(obj).addClass("Selected");
            let benchmarkHtml = "<li selectedItem='true' donothover='true' id=\"" + $(obj).attr("id") + "\" parent-of-parent=\"" + $(obj).attr("parent-of-parent") + "\" parentid=\"" + $(obj).attr("parentid") + "\" parentname=\"" + $(obj).attr("parentname") + "\" name=\"" + $(obj).attr("name") + "\" uniqueid=\"" + $(obj).attr("uniqueid") + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" id=\"" + $(obj).attr("id") + "\" type=\"Main-Stub\" name=\"" + $(obj).attr("name") + "\" uniqueid=\"" + $(obj).attr("uniqueid") + "\">" + $(obj).attr("name") + "</span><div name=\"" + $(obj).attr("name") + "\"  class=\"closeContainerDiv\"><span class=\"remove-item-selected\" uniqueid=\"" + $(obj).attr("uniqueid") + "\" name=\"" + $(obj).attr("name") + "\" id=\"" + $(obj).attr("id") + "\" parent-of-parent=\"" + $(obj).attr("parent-of-parent") + "\" onclick='RemoveSarRetailer(this);'></span></div></div></li>";
            $(benchmarkHtml).insertAfter("#SarRetailerDivId .level1 ul li[parentname='BenchMark']")
            //$(benchmarkHtml).insertAfter("#SarRetailerDivId .level1 ul li[parent-of-parent='BenchMark']")
            let currentBenchLevelSelected = Number($(obj).parent().parent().attr("level-id"))
            let parentSelectedObj = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[parent-of-parent='Custom'][name='" + $(obj).attr("parentname") + "']")
            let parentBenchLevelSelected = Number($(parentSelectedObj).parent().parent().attr("level-id"))
            let customBaseFirst = {}
            let customBaseSecond = {}
            let isTotalSelected = false
            if (currentBenchLevelSelected == 2 && $(obj).attr("name").trim().toLowerCase() == "total") {
                isTotalSelected = true
            }
            if (!isTotalSelected) {
                if (currentBenchLevelSelected == 2 || (currentBenchLevelSelected == 3 && $(obj).attr("parentname").trim().toLowerCase() == "total")) {
                    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Total']")
                }
                else {
                    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='" + $(obj).attr("parentname") + "']")
                }

                //customBaseSecond = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Total']");
            }
            else {
                customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Convenience']")
                //customBaseSecond = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Convenience']")
            }
            customBaseSecond = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Previous Period']")
            //if (currentBenchLevelSelected == 2 && $(obj).attr("name").trim().toLowerCase() == "total") {
            //    isTotalSelected = true
            //}
            //else if ((currentBenchLevelSelected == 3 && $(obj).attr("parentname").trim().toLowerCase() == "total") || currentBenchLevelSelected == 2) {
            //    if ($(obj).attr("name").trim().toLowerCase() == "convenience") {
            //        customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Supermarket/Grocery']")
            //    }
            //    else {
            //        customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Convenience']")
            //    }
            //}
            //else {
            //    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='" + $(obj).attr("parentname") + "']")
            //}
            //if (!isTotalSelected) {
            //    customBaseSecond = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Total']")
            //}
            //else {
            //    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Supermarket/Grocery']")
            //    customBaseSecond = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level2']" + " ul li[parent-of-parent='Custom'][name='Convenience']")
            //}
            //if ((currentBenchLevelSelected == 3 || currentBenchLevelSelected == 4) && $(obj).siblings("li[parent-of-parent='BenchMark']").length != 0) {
            //    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level" + (currentBenchLevelSelected) + "']" + " ul li[parent-of-parent='Custom'][name='" + $(obj).attr("name") + "']").nextAll("li[parent-of-parent='Custom']").first()
            //    customBaseSecond = $("#" + $(customBaseFirst).parent().parent().parent().attr("id") + " .Lavel[level='level" + (Number($(customBaseFirst).parent().parent().attr("level-id")) - (currentBenchLevelSelected == 4 ? 2 : 1)) + "']" + " ul li[parent-of-parent='Custom'][name='" + $(customBaseFirst).attr("parentname") + "']")
            //}
            //else if (currentBenchLevelSelected == 2) {
            //    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level" + (currentBenchLevelSelected) + "']" + " ul li[parent-of-parent='Custom'][name='" + $(obj).attr("name") + "']").nextAll("li[parent-of-parent='Custom']").first()
            //    customBaseSecond = $("#" + $(customBaseFirst).parent().parent().parent().attr("id") + " .Lavel[level='level" + (Number($(customBaseFirst).parent().parent().attr("level-id"))) + "']" + " ul li[parent-of-parent='Custom'][name='" + $(customBaseFirst).attr("name") + "']").nextAll("li[parent-of-parent='Custom']").first()
            //}
            //if (currentBenchLevelSelected > 3) {
            //    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level"+(currentBenchLevelSelected-1)+"']" + " ul li[parent-of-parent='Custom'][name='" + $(obj).attr("parentname") + "']")
            //}
            //else {
            //    customBaseFirst = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel[level='level" + (currentBenchLevelSelected) + "']" + " ul li[parent-of-parent='Custom'][name='" + $(obj).attr("name") + "']").nextAll("li[parent-of-parent='Custom']").first()
            //}
            //if (currentBenchLevelSelected > 2) {
            //    customBaseSecond = $("#" + $(customBaseFirst).parent().parent().parent().attr("id") + ".Lavel[level='level" + (Number($(customBaseFirst).parent().parent().attr("level-id")) - 1) + "']" + " ul li[parent-of-parent='Custom'][name='" + $(customBaseFirst).attr("parentname") + "']")
            //}
            //else {
            //    customBaseSecond = $("#" + $(customBaseFirst).parent().parent().parent().attr("id") + " .Lavel[level='level" + (Number($(customBaseFirst).parent().parent().attr("level-id"))) + "']" + " ul li[parent-of-parent='Custom'][name='" + $(customBaseFirst).attr("name") + "']").nextAll("li[parent-of-parent='Custom']").first()
            //}
            LevelDescL = $(customBaseFirst).parent().parent().attr("LevelDesc") != undefined ? $(customBaseFirst).parent().parent().attr("LevelDesc") : $(customBaseFirst).attr("LevelDesc");
            sarRetailerCustomList.push({ Id: $(customBaseFirst).attr("id"), Name: $(customBaseFirst).attr("name"), ParentName: $(customBaseFirst).attr("parentname"), ParentId: $(customBaseFirst).attr("parentid"), DBName: $(customBaseFirst).attr("name"), ShopperDBName: $(customBaseFirst).attr("shopperdbname"), TripsDBName: $(customBaseFirst).attr("tripsdbname"), UniqueId: $(customBaseFirst).attr("uniqueid"), PriorityId: $(customBaseFirst).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(customBaseFirst).attr("parent-of-parent") });
            $(customBaseFirst).addClass("Selected");
            let customBaseFirstHtml = "<li selectedItem='true' donothover='true' id=\"" + $(customBaseFirst).attr("id") + "\" parent-of-parent=\"" + $(customBaseFirst).attr("parent-of-parent") + "\" parentid=\"" + $(customBaseFirst).attr("parentid") + "\" parentname=\"" + $(customBaseFirst).attr("parentname") + "\" name=\"" + $(customBaseFirst).attr("name") + "\" uniqueid=\"" + $(customBaseFirst).attr("uniqueid") + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" id=\"" + $(customBaseFirst).attr("id") + "\" type=\"Main-Stub\" name=\"" + $(customBaseFirst).attr("name") + "\" uniqueid=\"" + $(customBaseFirst).attr("uniqueid") + "\">" + $(customBaseFirst).attr("name") + "</span><div name=\"" + $(customBaseFirst).attr("name") + "\"  class=\"closeContainerDiv\"><span class=\"remove-item-selected\" uniqueid=\"" + $(customBaseFirst).attr("uniqueid") + "\" name=\"" + $(customBaseFirst).attr("name") + "\" id=\"" + $(customBaseFirst).attr("id") + "\" parent-of-parent=\"" + $(customBaseFirst).attr("parent-of-parent") + "\" onclick='RemoveSarRetailer(this);'></span></div></div></li>";
            $("#SarRetailerDivId .level1 ul").append(customBaseFirstHtml)
            LevelDescL = $(customBaseSecond).parent().parent().attr("LevelDesc") != undefined ? $(customBaseSecond).parent().parent().attr("LevelDesc") : $(customBaseSecond).attr("LevelDesc");
            sarRetailerCustomList.push({ Id: $(customBaseSecond).attr("id"), Name: $(customBaseSecond).attr("name"), ParentName: $(customBaseSecond).attr("parentname"), ParentId: $(customBaseSecond).attr("parentid"), DBName: $(customBaseSecond).attr("name"), ShopperDBName: $(customBaseSecond).attr("shopperdbname"), TripsDBName: $(customBaseSecond).attr("tripsdbname"), UniqueId: $(customBaseSecond).attr("uniqueid"), PriorityId: $(customBaseSecond).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(customBaseSecond).attr("parent-of-parent") });
            $(customBaseSecond).addClass("Selected");
            let customBaseSecondHtml = "<li selectedItem='true' donothover='true' id=\"" + $(customBaseSecond).attr("id") + "\" parent-of-parent=\"" + $(customBaseSecond).attr("parent-of-parent") + "\" parentid=\"" + $(customBaseSecond).attr("parentid") + "\" parentname=\"" + $(customBaseSecond).attr("parentname") + "\" name=\"" + $(customBaseSecond).attr("name") + "\" uniqueid=\"" + $(customBaseSecond).attr("uniqueid") + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" id=\"" + $(customBaseSecond).attr("id") + "\" type=\"Main-Stub\" name=\"" + $(customBaseSecond).attr("name") + "\" uniqueid=\"" + $(customBaseSecond).attr("uniqueid") + "\">" + $(customBaseSecond).attr("name") + "</span><div name=\"" + $(customBaseSecond).attr("name") + "\"  class=\"closeContainerDiv\"><span class=\"remove-item-selected\" uniqueid=\"" + $(customBaseSecond).attr("uniqueid") + "\" name=\"" + $(customBaseSecond).attr("name") + "\" id=\"" + $(customBaseSecond).attr("id") + "\" parent-of-parent=\"" + $(customBaseSecond).attr("parent-of-parent") + "\" onclick='RemoveSarRetailer(this);'></span></div></div></li>";
            $("#SarRetailerDivId .level1 ul").append(customBaseSecondHtml)
            //let retailerPassed = "{RetailerId:'" + Number($(obj).attr("id")) + "'}";
            let retailerPassed = "{RetailerId:'" + Number($(obj).attr("uniqueid")) + "'}";
            //Guest-Visit-Toggle
            isVisitSar = true
            $('#sar-guest-visit-toggle').removeClass('active');
            $(".sar-adv-fltr-guest").css("color", "#fff");
            $(".sar-adv-fltr-visit").css("color", "#f00");
            //let retailerPassed = { RetailerId: Number($(obj).attr("id")) };
            jQuery.ajax({
                type: "POST",
                url: $("#URLGetCompetitorsList").val(),
                async: true,
                data: retailerPassed,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $.each(data, (i, v) => {
                        //let competitorObj = $("#SarCompetitorDivId .Lavel[level='level" + v.LevelId + "'] ul li[id='" + v.Id + "'][name='" + v.Name + "']")
                        let competitorObj = $("#SarCompetitorDivId .Lavel[level='level" + v.LevelId + "'] ul li[uniqueid='" + v.Id + "'][name='" + v.Name + "']")
                        LevelDescL = $(competitorObj).parent().parent().attr("LevelDesc") != undefined ? $(competitorObj).parent().parent().attr("LevelDesc") : $(competitorObj).attr("LevelDesc");
                        sarCompetitorList.push({ Id: $(competitorObj).attr("id"), Name: $(competitorObj).attr("name"), ParentName: $(competitorObj).attr("parentname"), DBName: $(competitorObj).attr("name"), ShopperDBName: $(competitorObj).attr("shopperdbname"), TripsDBName: $(competitorObj).attr("tripsdbname"), UniqueId: $(competitorObj).attr("uniqueid"), PriorityId: $(competitorObj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(competitorObj).attr("parent-of-parent") });
                        $(competitorObj).addClass("Selected");
                        if ($(competitorObj).attr("priorityid") == "false") {
                            isNonPrioritySelected = true;
                        }
                        else if (isNonPrioritySelected == false) {
                            isNonPrioritySelected = false;
                        }
                        if ($(competitorObj).attr("isChannel") == "true") {
                            isChannelSelected = true;
                        }
                        else if (isChannelSelected == false) {
                            isChannelSelected = false;
                        }
                        let competitorHtml = "<li selectedItem='true' donothover='true' id=\"" + $(competitorObj).attr("id") + "\" parent-of-parent=\"" + $(competitorObj).attr("parent-of-parent") + "\" parentid=\"" + $(competitorObj).attr("parentid") + "\" parentname=\"" + $(competitorObj).attr("parentname") + "\" name=\"" + $(competitorObj).attr("name") + "\" uniqueid=\"" + $(competitorObj).attr("uniqueid") + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" id=\"" + $(competitorObj).attr("id") + "\" type=\"Main-Stub\" name=\"" + $(competitorObj).attr("name") + "\" uniqueid=\"" + $(competitorObj).attr("uniqueid") + "\">" + $(competitorObj).attr("name") + "</span><div name=\"" + $(competitorObj).attr("name") + "\"  class=\"closeContainerDiv\"><span class=\"remove-item-selected\" uniqueid=\"" + $(competitorObj).attr("uniqueid") + "\" name=\"" + $(competitorObj).attr("name") + "\" id=\"" + $(competitorObj).attr("id") + "\" parent-of-parent=\"" + $(competitorObj).attr("parent-of-parent") + "\" onclick='RemoveSarCompetitor(this);'></span></div></div></li>";
                        $("#SarCompetitorDivId .level1 ul").append(competitorHtml)
                    })
                    if (sarCompetitorList.length >= 5) {
                        let freqParentList = []
                        $.each($("#SarFrequencyDivId .level1 ul li"), (i, v) => { freqParentList.push($(v).attr("name")) })
                        $.each(freqParentList, (i, v) => {
                            $("#SarFrequencyDivId .level2 ul li[parentname=\"" + v + "\"]").each((index, value) => { if ($(value).attr("name") != "VisitGuestToggle") $(value).removeClass("Selected") })
                            let currentSarFreqObj = null;
                            if (sarDefaultFrequencyList[v] == "Total Trips") {
                                currentSarFreqObj = $("#SarFrequencyDivId .level2 ul li[parentname=\"" + v + "\"][name='Weekly+']")
                            }
                            else {
                                currentSarFreqObj = $("#SarFrequencyDivId .level2 ul li[parentname=\"" + v + "\"][name=\"" + sarDefaultFrequencyList[v] + "\"]")
                            }
                            let LevelDescL = $(currentSarFreqObj).parent().parent().attr("LevelDesc") != undefined ? $(currentSarFreqObj).parent().parent().attr("LevelDesc") : $(currentSarFreqObj).attr("LevelDesc");

                            if (sarDefaultFrequencyList[v] == "Total Trips") {
                                sarFrequencyList[v] = { Id: "1000", Name: "Total Trips", ParentName: $(currentSarFreqObj).attr("parentname"), DBName: $(currentSarFreqObj).attr("name"), ShopperDBName: $(currentSarFreqObj).attr("shopperdbname"), TripsDBName: $(currentSarFreqObj).attr("tripsdbname"), UniqueId: "1000", PriorityId: $(currentSarFreqObj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(currentSarFreqObj).attr("parent-of-parent"), FrequencyId: "1000" }
                            }
                            else {
                                sarFrequencyList[v] = { Id: $(currentSarFreqObj).attr("id"), Name: $(currentSarFreqObj).attr("name"), ParentName: $(currentSarFreqObj).attr("parentname"), DBName: $(currentSarFreqObj).attr("name"), ShopperDBName: $(currentSarFreqObj).attr("shopperdbname"), TripsDBName: $(currentSarFreqObj).attr("tripsdbname"), UniqueId: $(currentSarFreqObj).attr("uniqueid"), PriorityId: $(currentSarFreqObj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(currentSarFreqObj).attr("parent-of-parent"), FrequencyId: $(currentSarFreqObj).attr("frequencyid") }
                                $(currentSarFreqObj).addClass("Selected");
                            }



                        })
                    }
                    ShowSelectedFilters()
                    return false;
                },
                error: function (xhr, status, error) {
                    ShowSelectedFilters()
                    return false;
                }
            });
        }
        else {
            sarRetailerBenchmarkList = []
            isChannelSelected = false
            isNonPrioritySelected = false
            corporateOrChannelNetSelected = false
            $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[parent-of-parent='" + $(obj).attr("parent-of-parent") + "'][name='" + $(obj).attr("name") + "']").removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#" + $(obj).parent().parent().parent().attr("id") + " .level1 ul li[id='" + $(obj).attr("id") + "'][uniqueid='" + $(obj).attr("uniqueid") + "'][name='" + $(obj).attr("name") + "']").remove();
            ShowSelectedFilters()
            return false;
        }
    }
}
function RemoveSarRetailer(obj) {
    let compObj = $("#SarRetailerDivId .Lavel[level!='level1'] ul li[UniqueId='" + $(obj).attr("UniqueId") + "'][parent-of-parent='" + $(obj).attr("parent-of-parent") + "']");
    if (compObj.length <= 0) {
        compObj = $("#SarRetailerDivId .Lavel[level!='level1'] ul li[UniqueId='" + $(obj).attr("UniqueId") + "']");
    }
    SelectSarRetailer(compObj)
}
function SelectSarCompetitor(obj) {
    if ($(obj).attr("type") == "Main-Stub") {
        obj = $(obj).parent("div").parent("li")
    }
    if ($(obj).attr("isselectable") != undefined && $(obj).attr("isselectable") != "" && JSON.parse($(obj).attr("isselectable").toLocaleLowerCase()) == false)
        return false;
    if (sarRetailerBenchmarkList.length == 0) {
        showMessage("Please select 1 Main Retailer / Channel and 2 Comparision Retailer / Channel")
        return false;
    }
    else if (sarRetailerCustomList.length == 0) {
        showMessage("Please select 2 Comparision Retailer / Channel")
        return false;
    }
    let currentIdInBench = GetArrayId(obj, sarRetailerBenchmarkList);
    let currentNameInBench = GetArrayName(obj, sarRetailerBenchmarkList);
    if (currentIdInBench != -1 || currentNameInBench != -1) {
        showMessage("You cannot make benchmark and competitor selection same")
        return false;
    }
    let currentCompId = GetArrayId(obj, sarCompetitorList)
    let currentCompName = GetArrayName(obj, sarCompetitorList)
    if (currentCompId == -1 && currentCompName == -1) {
        if (sarCompetitorList.length == 20) {
            showMessage("You can select min of 5 and max of 20 competitors")
            return false;
        }
        if ($(obj).attr("priorityid") == "false") {
            isNonPrioritySelected = true;
        }
        if ($(obj).attr("isChannel") == "true") {
            isChannelSelected = true;
        }
        let LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");
        sarCompetitorList.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), ParentName: $(obj).attr("parentname"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid"), PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(obj).attr("parent-of-parent") });
        $(obj).addClass("Selected");
        let competitorHtml = "<li selectedItem='true' donothover='true' id=\"" + $(obj).attr("id") + "\" parent-of-parent=\"" + $(obj).attr("parent-of-parent") + "\" parentid=\"" + $(obj).attr("parentid") + "\" parentname=\"" + $(obj).attr("parentname") + "\" name=\"" + $(obj).attr("name") + "\" uniqueid=\"" + $(obj).attr("uniqueid") + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" id=\"" + $(obj).attr("id") + "\" type=\"Main-Stub\" name=\"" + $(obj).attr("name") + "\" uniqueid=\"" + $(obj).attr("uniqueid") + "\">" + $(obj).attr("name") + "</span><div name=\"" + $(obj).attr("name") + "\"  class=\"closeContainerDiv\"><span class=\"remove-item-selected\" uniqueid=\"" + $(obj).attr("uniqueid") + "\" name=\"" + $(obj).attr("name") + "\" id=\"" + $(obj).attr("id") + "\" parent-of-parent=\"" + $(obj).attr("parent-of-parent") + "\" onclick='RemoveSarCompetitor(this);'></span></div></div></li>";
        $("#SarCompetitorDivId .level1 ul").append(competitorHtml)
    }
    else {
        sarCompetitorList.splice(currentCompId, 1);
        let locIsNonPriority = false;
        let locIsChannelSelected = false;
        for (let i = 0; i < sarRetailerBenchmarkList.length; i++) {
            let compObj = $("#SarRetailerDivId .Lavel[level!='level1'] ul li[UniqueId='" + sarRetailerBenchmarkList[i]["UniqueId"] + "'][parent-of-parent='" + sarRetailerBenchmarkList[i]["parent-of-parent"] + "']");
            if (compObj.length <= 0) {
                compObj = $("#SarRetailerDivId .Lavel[level!='level1'] ul li[UniqueId='" + sarRetailerBenchmarkList[i]["UniqueId"] + "']");
            }
            if ($(compObj).attr("priorityid") == "false") {
                locIsNonPriority = true; 
            }
            if ($(compObj).attr("isChannel") == "true") {
                locIsChannelSelected = true;
            }
        }
        for (let i = 0; i < sarCompetitorList.length ; i++) {
            let compObj = $("#SarCompetitorDivId .Lavel[level!='level1'] ul li[UniqueId='" + sarCompetitorList[i]["UniqueId"] + "'][parent-of-parent='" + sarCompetitorList[i]["parent-of-parent"] + "']");
            if (compObj.length <= 0) {
                compObj = $("#SarCompetitorDivId .Lavel[level!='level1'] ul li[UniqueId='" + sarCompetitorList[i]["UniqueId"] + "']");
            }
            if ($(compObj).attr("priorityid") == "false") {
                locIsNonPriority = true; 
            }
            if ($(compObj).attr("isChannel") == "true") {
                locIsChannelSelected = true;
            }

        }
        if (locIsNonPriority == false) {
            isNonPrioritySelected = false;
        }
        if (locIsChannelSelected == false) {
            isChannelSelected = false;
        }
        $(obj).removeClass("Selected");
        $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[parent-of-parent='" + $(obj).attr("parent-of-parent") + "'][name='" + $(obj).attr("name") + "']").removeClass("Selected");
        $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#" + $(obj).parent().parent().parent().attr("id") + " .level1 ul li[id='" + $(obj).attr("id") + "'][uniqueid='" + $(obj).attr("uniqueid") + "'][name='" + $(obj).attr("name") + "']").remove();
    }
    ShowSelectedFilters()

}
function RemoveSarCompetitor(obj) {
    let compObj = $("#SarCompetitorDivId .Lavel[level!='level1'] ul li[UniqueId='" + $(obj).attr("UniqueId") + "'][parent-of-parent='" + $(obj).attr("parent-of-parent") + "']");
    if (compObj.length <= 0) {
        compObj = $("#SarCompetitorDivId .Lavel[level!='level1'] ul li[UniqueId='" + $(obj).attr("UniqueId") + "']");
    }
    SelectSarCompetitor(compObj);
}
function SelectSarFrequency(obj) {
    if ($(obj).attr("isselectable") != undefined && $(obj).attr("isselectable") != "" && JSON.parse($(obj).attr("isselectable").toLocaleLowerCase()) == false)
        return false;
    let parentName = $(obj).attr("parentname")
    let prevSelectedObj = sarFrequencyList[parentName]
    if (totaltripsList.indexOf(parentName) != -1 && ((parentName == "Who is my Core shopper?" && isVisitSar == true) || parentName != "Who is my Core shopper?")) {
        if (prevSelectedObj["Name"] == "Total Trips") {
            let LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");
            sarFrequencyList[parentName] = { Id: $(obj).attr("id"), Name: $(obj).attr("name"), ParentName: $(obj).attr("parentname"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid"), PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(obj).attr("parent-of-parent"), FrequencyId: $(obj).attr("frequencyid") }
            $(obj).addClass("Selected");
        }
        else {
            let prevObj = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[id='" + prevSelectedObj["Id"] + "'][name='" + prevSelectedObj["Name"] + "']")

            if (prevObj != undefined) {
                $(prevObj).removeClass("Selected");
            }
            if (prevSelectedObj["Name"] == $(obj).attr("name")) {
                let LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");
                sarFrequencyList[parentName] = { Id: "1000", Name: "Total Trips", ParentName: $(obj).attr("parentname"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: "1000", PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(obj).attr("parent-of-parent"), FrequencyId: "1000" }
            }
            else {

                $(obj).siblings("[parentname=\"" + $(obj).attr("parentname") + "\"]").each((i, v) => {
                    $(v).removeClass("selected");
                })

                let LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");
                sarFrequencyList[parentName] = { Id: $(obj).attr("id"), Name: $(obj).attr("name"), ParentName: $(obj).attr("parentname"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid"), PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(obj).attr("parent-of-parent"), FrequencyId: $(obj).attr("frequencyid") }
                $(obj).addClass("Selected");
            }
        }
    }
    else {
        if (prevSelectedObj != undefined) {
            let prevObj = $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[id='" + prevSelectedObj["Id"] + "'][name='" + prevSelectedObj["Name"] + "']")
            if (prevObj != undefined) {
                $(prevObj).removeClass("Selected");
            }
        }

        let LevelDescL = $(obj).parent().parent().attr("LevelDesc") != undefined ? $(obj).parent().parent().attr("LevelDesc") : $(obj).attr("LevelDesc");
        sarFrequencyList[parentName] = { Id: $(obj).attr("id"), Name: $(obj).attr("name"), ParentName: $(obj).attr("parentname"), DBName: $(obj).attr("name"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid"), PriorityId: $(obj).attr("priorityid"), LevelDesc: LevelDescL, ParentOfParent: $(obj).attr("parent-of-parent"), FrequencyId: $(obj).attr("frequencyid") }
        $(obj).addClass("Selected");
    }

    ShowSelectedFilters()
    return false;
}
function RemoveSarFrequency(obj) {
    let compObj = $("#SarFrequencyDivId .Lavel ul li[UniqueId='" + $(obj).attr("UniqueId") + "']");
    SelectSarFrequency(CompObj);
}
function DisplayComparisonRetailer(obj) {
    if (currentpage == "hdn-analysis-crossretailerimageries" && ($(obj).attr("name").toString().trim() == "Total" || $(obj).attr("name").toString().trim() == "Corporate Nets"))
        return;
    if (currentpage == "hdn-analysis-acrossshopper" && ($(obj).attr("name").toString().trim() == "Total"))
        return;
    $("#ChannelOrCategoryContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj).parent();
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw");
    $(obj).addClass("sidearrw_OnCLick");


    //$(obj).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    $(".Retailer").hide();
    $(".Retailer div").hide();
    var lavels = parseInt($(obj).attr("lavels"));
    for (var i = 0; i < lavels ; i++) {
        $(".Retailers .RetailerDiv .Lavel" + i).show();
        $(".Retailers .RetailerDiv .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".Retailers .RetailerDiv .Lavel" + i + " .priorityclass").hide();
        $(".Retailers .RetailerDiv .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").text($(obj).attr("name").toLowerCase());
        $(".Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").show();

        if (i == 1)
            $(".Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").css("width", "574px");
        else
            $(".Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").css("width", "287px");
        SetScroll($(".Retailers .RetailerDiv .Lavel" + i), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }

    DisplayHeightDynamicCalculation("retailer");
    if ($(obj).attr("name") == "Total") {
        sRetailer = "1";
        $(".Retailers .RetailerDiv .Lavel0").hide();
        $(".Retailers #retailerHeadingLevel2").css("width", "287px");
    }
    else {
        $(".Retailers .RetailerDiv .Lavel0").show();
        sRetailer = "2";
    }
}
function SelectBevComparison(obj) {
    if ($(obj).attr("isselectable") != undefined && $(obj).attr("isselectable") != "" && JSON.parse($(obj).attr("isselectable")) == false)
        return false;
    for (var i = 0; i < $(obj).length; i++) {
        if ($(obj).eq(i).hasClass("Selected")) {
            obj = $(obj).eq(i);
            break;
        }

    }
    if ($(obj).length > 1) {
        for (var i = 0; i < $(obj).length; i++) {
            if ($(obj).children().eq(i).attr("data-isselectable")) {
                obj = $(obj).eq(i);
                break;
            }

        }
    }
    CompCurrentId = GetArrayId($(obj), ComparisonBevlist);
    CompCurrentName = GetArrayName(obj, ComparisonBevlist);
    if (ModuleBlock == "TREND" || ModuleBlock == "PIT" || currentpage == "hdn-analysis-crossretailerfrequencies" || currentpage == "hdn-chart-beveragedeepdive") {
        if (CompCurrentId == -1 && CompCurrentName != -1) { return; }
        ComparisonBevlist = [];
        $(".Beverages .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
        $(".Beverages .Lavel li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#BGMNonBeverageDiv .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
    }
    else {
        if (CompCurrentId == -1 && ComparisonBevlist.length == 5 && currentpage.indexOf("hdn-report") > -1) {
            showMessage("YOU CAN MAKE UPTO 5 SELECTIONS");
            return false;
        }
        else if (CompCurrentId == -1 && ComparisonBevlist.length == 11 && currentpage != "hdn-analysis-acrossshopper") {
            showMessage("YOU CAN MAKE UPTO 11 SELECTIONS");
            return false;
        }
        else if (CompCurrentId == -1 && ComparisonBevlist.length == 9 && currentpage == "hdn-analysis-acrossshopper") {
            showMessage("YOU CAN MAKE UPTO 9 SELECTIONS");
            return false;
        }
    }


    if (CompCurrentId == -1 && CompCurrentName == -1) {
        if ($(obj).parent("div").parent("li").parent("").parent().attr("class") == "Lavel level1") {
            var objL = $(obj).parent("div").parent("li");
        }
        else {
            var objL = $(obj);
        }

        ComparisonBevlist.push({ Id: objL.attr("id"), Name: objL.attr("name"), DBName: objL.attr("name"), ShopperDBName: objL.attr("shopperdbname"), TripsDBName: objL.attr("tripsdbname"), UniqueId: objL.attr("uniqueid"), LevelDesc: objL.attr("leveldesc") });
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Not-Selected-Channel").addClass("Selected");
        }
        else {
            $(obj).addClass("Selected");
        }
    }
    else if (CompCurrentId == -1 && CompCurrentName != -1) { return; }
    else if (CompCurrentId != -1 && CompCurrentName != -1) {
        ComparisonBevlist.splice(CompCurrentId, 1);
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Selected").addClass("Not-Selected-Channel");
            $(obj).parent("div").parent("li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
        else {
            $(obj).removeClass("Selected");
            $("#" + $(obj).parent().parent().parent().attr("id") + " .Lavel ul li[name='" + $(obj).attr("name") + "']").removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
    }
    if ($(obj).attr("type") == "Main-Stub") {
        $(".Beverage").hide();
        $("#beverageHeadingLevel2").hide();
        $("#beverageHeadingLevel3").hide();
        $(".Beverages").css("width", "auto");
        $("#BeverageOrCategoryContent .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
    }

    ShowSelectedFilters();
    if (currentpage == "hdn-analysis-acrossshopper") {
        AddToBevarageDropDown();
    }

    if ($(obj).attr("name") == "Category Nets" && currentpage == "hdn-analysis-acrossshopper") {
        //$(".Beverages").css("width", "95%");
        //$(".BevScrollDiv").css("width", "135%");
        $(".BevScrollDiv").css("width", "auto");
    }
    else if ($(obj).attr("name") == "Category Nets") {
        //$(".Beverages").css("width", "95%");
        //$(".BevScrollDiv").css("width", "111%");
        $(".BevScrollDiv").css("width", "auto");
    }
    else {
        $(".Beverages").css("width", "auto");
        $(".BevScrollDiv").css("width", "auto");
    }
    if ($(".BevScrollDiv").width() > window.innerWidth) {
        //$(".Beverages").css("width", "95%");
    }
    else
        $(".Beverages").css("width", "auto");
    SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    if ($(".BevScrollDiv")[0].style.width == "auto")
        $(".Beverages").getNiceScroll().remove();
    else
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

    BuildDynamicTable();
    //var leftPos = $('#BevContainerDivId').scrollLeft() + 200;
    //$('#BevContainerDivId').scrollLeft(leftPos);
    //SetScroll($("#MeasureTypeHeaderContentSubLevel"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}//[name='" + $(obj).attr("Name") + "' i]
function RemoveBevComparison(obj) {
    var CompObj = $(".Beverages .Comparison[name='" + $(obj).attr("name") + "'][id='" + $(obj).attr("Id") + "']");
    if (CompObj.length <= 0)
        CompObj = $("#BeverageOrCategoryContent ul li span[dbname='" + $(obj).attr("dbname") + "'][id='" + $(obj).attr("Id") + "']");
    if (CompObj.length <= 0)
        CompObj = $("#BGMNonBeverageDiv ul li span[id='" + $(obj).attr("Id") + "']").parent();
    if (CompObj.length <= 0)
        CompObj = $("#BevDivId li[uniqueid='" + $(obj).attr("UniqueId") + "']");
    SelectBevComparison(CompObj);
}
function DisplayBevComparisonRetailer(obj) {

    $("#BeverageOrCategoryContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj).parent();
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw");
    $(obj).addClass("sidearrw_OnCLick");

    $(".Beverage").hide();
    $(".Beverage div").hide();
    //if ($(obj).attr("name") == "Category Nets" && currentpage == "hdn-analysis-acrossshopper")
    //    $(".Beverages").css("width", "127%");
    //else if ($(obj).attr("name") == "Category Nets")
    //    $(".Beverages").css("width", "106%");
    //else
    //    $(".Beverages").css("width", "auto");


    var lavels = parseInt($(obj).attr("lavels"));
    for (var i = 0; i < lavels ; i++) {
        $(".Beverages .Lavel" + i).show();
        $(".Beverages .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").text($(obj).attr("name"));
        $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").show();
        if (i == 3)
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "1154px");
        else if (i == 2)
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "856px");
        else if (i == 1)
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "574px");
        else
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "287px");
        SetScroll($(".Beverages .Lavel" + i), left_scroll_bgcolor, 0, 0, 0, 0, 8);

        if ($(obj).attr("name") == "Category Nets" && currentpage == "hdn-analysis-acrossshopper") {
            //$(".Beverages").css("width", "95%");
            //$(".BevScrollDiv").css("width", "135%");
            $(".Beverages").css("width", "auto");
        }
        else if ($(obj).attr("name") == "Category Nets") {
            //$(".Beverages").css("width", "95%");
            //$(".BevScrollDiv").css("width", "111%");
            $(".Beverages").css("width", "auto");
        }
        else {
            $(".Beverages").css("width", "auto");
            $(".BevScrollDiv").css("width", "auto");
        }
        if ($(".BevScrollDiv").width() > window.innerWidth) {
            //$(".Beverages").css("width", "95%");
        }
        else
            $(".Beverages").css("width", "auto");
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    if ($(".BevScrollDiv")[0].style.width == "auto")
        $(".Beverages").getNiceScroll().remove();
    else
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

    SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

    DisplayHeightDynamicCalculation("beverage");
    //var leftPos = $('#BevContainerDivId').scrollLeft() + 200;
    //$('#BevContainerDivId').scrollLeft(leftPos);
    SetScroll($(".Beverages .Lavel"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function AddToBevarageDropDown() {
    $("#ddlbeverageitems").html("");
    $("#SelectedBGMPurchaseItems").html("");
    for (var i = 0; i < ComparisonBevlist.length; i++) {
        if (TabType.toLocaleLowerCase() == "trips") {
            var newOption = "<option id=\"" + ComparisonBevlist[i].Id + "\" BevItem=\"" + ComparisonBevlist[i].Name + "\">" + ComparisonBevlist[i].Name.replace("~", "`") + "</option>";
        }
        else {
            var newOption = "<option id=\"" + ComparisonBevlist[i].Id + "\" BevItem=\"" + ComparisonBevlist[i].Name + "\">" + ComparisonBevlist[i].Name.replace("~", "`") + "</option>";
        }
        $("#ddlbeverageitems").append(newOption);

        //var newPurchaseItem = "<div class=\"SelectedBGMPurchaseItem\">" + BevarageItems[i].ShortName.replace("~", "`") + "<a class=\"deletePurchaseItem\" onclick=\"DeleteBeverageItem('" + BevarageItems[i].ShortName + "','" + BevarageItems[i].id + "','" + BevarageItems[i].BevItem + "','" + BevarageItems[i].ItemType + "')\">X</a></div>";
        //$("#SelectedBGMPurchaseItems").append(newPurchaseItem);
    }
}//Advanced Filter Popup


function DisplayDemoOrVisitsFilters(ShowValue) {
    $("#PrimaryAdvancedFilterContent").hide();
    $("#PrimaryAdvancedFilterContentAdv").hide();
    $("#SecondaryAdvancedFilterContent").hide();
    $("#SecondaryAdvancedFilterContentAdv").hide();
    $("#ThirdLevelAdvancedFilterContent").hide();
    $("#ThirdLevelAdvancedFilterContentAdv").hide();
    $("#FourthLevelAdvancedFilterContent").hide();
    $("#FourthLevelAdvancedFilterContentAdv").hide();
    $("#DemoHeadingLevel4").hide();

    $("#ToShowDemoAndAdvFilters .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $("#" + ShowValue);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    $("#DemoHeadingLevel2").hide();
    $("#SecondaryAdvancedFilterContent").hide();
    $("#DemoHeadingLevel3").hide();
    $("#ThirdLevelAdvancedFilterContent").hide();
    $("#ToShowDemoAndAdvFilters *").removeClass("Selected");
    $("#ToShowDemoAndAdvFilters *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$("#" + ShowValue).addClass("Selected");
    if (ShowValue == "ToShowDemo") {
        $(".demomaindiv").show();
        $("#PrimaryAdvancedFilterContent").css("display", "inline-block");
        $(".Advmaindiv").hide();
        $("#PrimaryAdvancedFilterContentAdv").hide();
        //LoadAdvancedFilters(sFilterData);
        //LoadSecondaryAdvancedFilters(sFilterData);
    }
    else {
        $(".demomaindiv").hide();
        $("#PrimaryAdvancedFilterContent").hide();
        $(".Advmaindiv").css("display", "inline-block");
        $("#PrimaryAdvancedFilterContentAdv").css("display", "inline-block");
        //LoadVisitsFiltersLeftPanel(sFilterData);
        //LoadSecondaryVisitsFiltersLeftPanel(sFilterData);
        AddFrequencyinAdvFilters();
        if (currentpage == "hdn-dashboard-demographic") {
            $(".Advmaindiv #PrimaryAdvancedFilterContentAdv #PrimaryDemoFilterListAdv li div[id='-1']").parent().css("display", "block");
        }
    }
    $("#PrimaryAdvancedFilterContent").css("display", "inline-block");
    $("#DemoHeadingLevel1").html($("#" + ShowValue).attr("name").toLowerCase());
    $("#DemoHeadingLevel1").show();
    if (SelectedDempgraphicList.length > 0) {
        _.each(SelectedDempgraphicList, function (i) {
            $("#SecondaryDemoFilterList span[uniqueid='" + i.UniqueId.trim() + "']").parent().addClass('Selected');
        });
    }
    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllAdvancedFilterLeft));
}
function LoadAdvancedFilters(data) {
    html = "";
    var index = 0;
    //var ImageDetails = GetDemographyImagePosition();
    var DataList = [];
    var status = "0";
    if ((Grouplist.length > 0 && Grouplist[0].isGeography == "true")) {
        //DataList = data.AdvancedFilterlist;
        //DataList = _.filter(sFilterData.AdvancedFilterlist, function (i) { return i.Name != Grouplist[0].DBName.split('|')[0].trim() });
        status = "0";
        DataList = _.filter(sFilterData.AdvancedFilterlist, function (i) {
            status = "0";
            _.filter(i.SecondaryAdvancedFilterlist, function (j) {
                if (j.DBName.split('|')[0].trim() != Grouplist[0].DBName.split('|')[0].trim())
                    status = "0";
                else
                    status = "1";
            });
            if (status == "0")
                return i;
        });
    }
    else {
        if (currentpage != "hdn-analysis-acrosstrips") {
            dGeo = SelecGeography();
            DataList = (data.AdvancedFilterlist).concat(dGeo);
            if (Grouplist.length > 0) {
                //DataList = _.filter(DataList, function (i) { return i.Name != Grouplist[0].DBName.split('|')[0].trim() });
                status = "0";
                DataList = _.filter(DataList, function (i) {
                    status = "0";
                    _.filter(i.SecondaryAdvancedFilterlist, function (j) {
                        if (j.DBName.split('|')[0].trim() != Grouplist[0].DBName.split('|')[0].trim())
                            status = "0";
                        else
                            status = "1";
                    });
                    if (status == "0")
                        return i;
                });
            }
        }
        else if (currentpage == "hdn-analysis-acrosstrips") {
            dGeo = SelecGeography();
            DataList = (data.AdvancedFilterlist).concat(dGeo);

            if (Grouplist.length > 0) {
                //DataList = _.filter(sFilterData.AdvancedFilterlist, function (i) { return i.Name != Grouplist[0].DBName.split('|')[0].trim() });
                status = "0";
                DataList = _.filter(DataList, function (i) {
                    status = "0";
                    _.filter(i.SecondaryAdvancedFilterlist, function (j) {
                        if (j.DBName.split('|')[0].trim() != Grouplist[0].DBName.split('|')[0].trim())
                            status = "0";
                        else
                            status = "1";
                    });
                    if (status == "0")
                        return i;
                });
            }
            else
                DataList = (data.AdvancedFilterlist).concat(dGeo);//DataList = data.AdvancedFilterlist;
        }
        else {

            if (Grouplist.length > 0) {
                //DataList = _.filter(sFilterData.AdvancedFilterlist, function (i) { return i.Name != Grouplist[0].DBName.split('|')[0].trim() });
                status = "0";
                DataList = _.filter(sFilterData.AdvancedFilterlist, function (i) {
                    status = "0";
                    _.filter(i.SecondaryAdvancedFilterlist, function (j) {
                        if (j.DBName.split('|')[0].trim() != Grouplist[0].DBName.split('|')[0].trim())
                            status = "0";
                        else
                            status = "1";
                    });
                    if (status == "0")
                        return i;
                });
            }
            else
                DataList = data.AdvancedFilterlist;
        }
    }
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            var object = DataList[i];
            if (index == 0) {
                html += "<ul>";
                //ulclose = false;
            }
            var sImageClassName = "";//_.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; })[0].imagePosition : "";
            if (object.Level == "1") {
                html += "<li style=\"display:table;\">";
                //html += "<div Name=\"" + object.Name + "\" class=\"\" onclick=\"DisplaySecondaryDemoFilter(this);\">" + object.Name + "</div>";
                html += "<div onclick=\"DisplaySecondaryDemoFilter(this);\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";

                //AllDemographics.push(object.Id + "|" + object.Name);
                html += "</li>";

                //if (index == 1) {
                //html += "</ul>";
                //ulclose = true;
                //}
                index++;
            }
        }

        //if (ulclose == false)
        html += "</ul>";

        $("#PrimaryDemoFilterList").html("");
        $("#PrimaryDemoFilterList").html(html);
    }
}
function LoadSecondaryAdvancedFilters(data) {

    html = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var DataList = [];
    AllDemographics = [];
    dGeo = SelecGeography();
    DataList = (data.AdvancedFilterlist).concat(dGeo);
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].SecondaryAdvancedFilterlist.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
                thirdLevelhtml += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"display:none;\"><ul>";
                for (var j = 0; j < DataList[i].SecondaryAdvancedFilterlist.length; j++) {
                    var object = DataList[i].SecondaryAdvancedFilterlist[j];
                    if (DataList[i].Level == "1") {
                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(DataList, function (u) {
                            return u.Name.toUpperCase() == object.Name.toUpperCase();
                        });
                        if (k.length <= 0) {
                            if (object.active != "false") {
                                html += "<div onclick=\"SelectDemographic(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                AllDemographics.push(object.UniqueId + "|" + object.Name);
                            }
                            else {
                                if (object.isGeography == "true")
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            }
                        }
                        else {
                            if (object.isGeography == "true") {
                                if (object.active != "false")
                                    html += "<div onclick=\"DisplayThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            }
                            else {
                                if (object.active != "false")
                                    html += "<div onclick=\"DisplayThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                                else
                                    html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            }
                        }
                    }
                    else {
                        var object = DataList[i].SecondaryAdvancedFilterlist[j];
                        if (DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                            if (object.isGeography == "true")
                                thirdLevelhtml += "<div onclick=\"DisplayFourthLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                thirdLevelhtml += "<div onclick=\"DisplayFourthLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            //if (indexSubLevel <= 0) {
                            fourthLevelhtml += "<div class=\"DemographicList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                            //    indexSubLevel++;
                            //}
                            for (var l = 0; l < DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                                var object1 = DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                                if (object1.active != "false") {
                                    if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    AllDemographics.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else {
                                    if (object1.Name.replace("`", "") == "Albertson's/Safeway Corporate Net Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area" || object1.Name.replace("`", "") == "Kroger Trade Area" || object1.Name.replace("`", "") == "Publix Trade Area")
                                        fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                }
                            }
                            fourthLevelhtml += "</ul></div>";
                        }
                        else {
                            thirdLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"ThirdLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            AllDemographics.push(object.UniqueId + "|" + object.Name);
                        }


                    }
                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }
    $("#SecondaryDemoFilterList").html("");
    $("#SecondaryDemoFilterList").html(html);
    $("#ThirdDemoFilterList").html("");
    $("#ThirdDemoFilterList").html(thirdLevelhtml);
    $("#FourthDemoFilterList").html("");
    $("#FourthDemoFilterList").html(fourthLevelhtml);
    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
    SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);

    $('.Geotooltipimage').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function LoadAdvancedFilterFromString(data) {

    $("#PrimaryDemoFilterList").html("");
    $("#PrimaryDemoFilterList").html(sFilterData.AdvFilterlist.StringList.HTML_String[0].toString());

    $("#SecondaryDemoFilterList").html("");
    $("#SecondaryDemoFilterList").html(sFilterData.AdvFilterlist.StringList.HTML_String[1].toString());
    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", sFilterData.AdvFilterlist.StringList.SearchItems);
    AllDemographics = sFilterData.AdvFilterlist.StringList.SearchItems;
    //UpdateAdvancedFilterGeography();
    //added by Nagaraju for default geography filters
    //Date: 13-04-2017
    UpdateDefaultAdvancedFilterGeography();
    AddFrequencyinAdvFilters();
}

//total Measure Filter Laoding Start
function LoadTotalMeasuresFilters(data) {
    html = "";
    var index = 0;
    var DataList = [];
    DataList = data.TotalTripMeasure;

    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            var object = DataList[i];
            if (index == 0) {
                html += "<ul>";
            }
            var sImageClassName = "";
            if (object.LevelId == "1") {
                html += "<li style=\"display:table;\">";
                html += "<div onclick=\"DisplaySecondaryTotalFilter(this);\" style=\"border-bottom: 0;display: flex;\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"padding-right:3px;\" class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span style=\"height:0;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                html += "</li>";
                index++;
            }
        }
        html += "</ul>";
        $("#TotalMeasureHeaderMainTrip").html("");
        $("#TotalMeasureHeaderMainTrip").html(html);
    }

    var DataList = [];
    html = "";
    index = 0;
    DataList = data.TotalShopperMeasure;

    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            var object = DataList[i];
            if (index == 0) {
                html += "<ul>";
            }
            var sImageClassName = "";
            if (object.LevelId == "1") {
                html += "<li style=\"display:table;\">";
                html += "<div onclick=\"DisplaySecondaryTotalFilter(this);\" style=\"border-bottom: 0;display: flex;\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"padding-right:3px;\" class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span style=\"height:0;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                html += "</li>";
                index++;
            }
        }
        html += "</ul>";
        $("#TotalMeasureHeaderMainShopper").html("");
        $("#TotalMeasureHeaderMainShopper").html(html);
    }
}
function LoadSecondaryTotalMeasuresFilters(data) {
    //Trip
    html = "";
    var DataList = [];

    DataList = data.TotalTripMeasure;
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].Metriclist.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:none;display:none;position:relative;\"><ul>";
                for (var j = 0; j < DataList[i].Metriclist.length; j++) {
                    var object = DataList[i].Metriclist[j];
                    html += "<li Name=\"" + object.Name + "\" style=\"display:table;border:none;line-height:0%;\">";

                    html += "<div id=\"" + object.Id + "\" onclick=\"SelectTotalMeasure(this);\" type=\"trip\" class=\"lft-popup-ele\" style=\"width:100%;height:auto;\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" type=\"trip\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" parent=\"" + DataList[i].Name + "\" data-isselectable=\"true\">" + object.Name + "</span></div>";
                    html += "</li>";
                    TripTotalMeasures.push(object.UniqueId + "|" + object.Name);
                    AllTotalMeasures.push(object.UniqueId + "|" + object.Name);
                }
                html += "</ul></div>";
            }
        }
    }
    $("#TotalMeasureHeaderContentTrip").html("");
    $("#TotalMeasureHeaderContentTrip").html(html);

    //Shopper
    html = "";
    var DataList = [];

    DataList = data.TotalShopperMeasure;
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].Metriclist.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:none;display:none;position:relative;\"><ul>";
                for (var j = 0; j < DataList[i].Metriclist.length; j++) {
                    var object = DataList[i].Metriclist[j];
                    html += "<li Name=\"" + object.Name + "\" style=\"display:table;border:none;line-height:0%;\">";

                    html += "<div id=\"" + object.Id + "\" onclick=\"SelectTotalMeasure(this);\" type=\"shopper\" class=\"lft-popup-ele\" style=\"width:100%;height:auto;\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" type=\"shopper\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" parent=\"" + DataList[i].Name + "\" data-isselectable=\"true\">" + object.Name + "</span></div>";
                    html += "</li>";
                    AllTotalMeasures.push(object.UniqueId + "|" + object.Name);
                }
                html += "</ul></div>";
            }
        }
    }
    SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", AllTotalMeasures);
    $("#TotalMeasureHeaderContentShopper").html("");
    $("#TotalMeasureHeaderContentShopper").html(html);
}
function DisplayTotalMeasures(obj) {
    var sChange = "";
    //TabType = $(obj).attr("name").toLowerCase().replace("trip", "trips");
    SelectedTotalMeasure = [];
    $(".TotalMeasureHeaderContent").find(".Selected").removeClass("Selected");
    $(".TotalMeasureHeaderContent").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#TotalMeasureHeaderMainTrip *").removeClass("Selected");
    $("#TotalMeasureHeaderMainTrip *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#TotalMeasureHeaderMainTrip .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#TotalMeasureHeaderMainShopper *").removeClass("Selected");
    $("#TotalMeasureHeaderMainShopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#TotalMeasureHeaderMainShopper .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    if ($(obj).attr("id") == "ToTotalMeasureShowTrip") {
        if (sVisitsOrGuests == 1)
            sChange = "false";
        else
            sChange = "true";
        sVisitsOrGuests = 1;
        sBevarageSelctionType = [];
        $(".TotalMeasure .trip").show();
        $(".TotalMeasure .Shopper").hide();
        TabType = "trips";
        $("#Left-Frequency").hide();
        SelectedFrequencyList = [];
    }
    else {
        if (sVisitsOrGuests == 2)
            sChange = "false";
        else
            sChange = "true";
        sVisitsOrGuests = 2;
        sBevarageSelctionType = [];
        $(".TotalMeasure .trip").hide();
        $(".TotalMeasure .Shopper").show();
        TabType = "shopper";
        $("#left-panel-frequency ul div[name='STORE IN TRADE AREA']").trigger("click");
        $("#Left-Frequency").show();
    }
    HideOrShowFilters();
    //GetDefaultFrequency();
    //$("#TotalMeasure").trigger("click");
    $("#TotalMeasureShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    $(obj).parent().find(".Selected").removeClass("Selected");
    $(obj).parent().find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(obj).addClass("Selected");
    if (sVisitsOrGuests == 1) {
        $("#TotalMeasureHeaderMainTrip").show();
        $("#TotalMeasureHeaderMainTrip ul li").css("display", "block");
    }
    else if (sVisitsOrGuests == 2) {
        $("#TotalMeasureHeaderMainShopper").show();
        $("#TotalMeasureHeaderMainShopper ul li").css("display", "none");
    }
    $("#TotalMeasureHeaderContentTrip").hide();
    $("#TotalMeasureHeaderContentShopper").hide();
    $("#TotalMeasureHeadingLevel2").show();
    $("#TotalMeasureHeadingLevel3").hide();
    $("#TotalMeasureHeadingLevel4").hide();
    $("#TotalMeasureHeaderContentTrip").hide();
    $("#TotalMeasureHeaderContentShopper").hide();

    if (sVisitsOrGuests == 1) {
        $("#TotalMeasureHeaderMainTrip").show();
        $("#TotalMeasureHeaderMainTrip ul li").show();
    }
    else if (sVisitsOrGuests == 2) {
        $("#TotalMeasureHeaderMainShopper").show();
        $("#TotalMeasureHeaderMainShopper ul li").show();
    }
    //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel2").text($(obj).text().trim().toLowerCase());
    //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel2").show();
    //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel2").css("width", "287px");

    if (sVisitsOrGuests == 1) {
        SetScroll($("#TotalMeasureHeaderMainTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    else if (sVisitsOrGuests == 2) {
        SetScroll($("#TotalMeasureHeaderMainShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
}
function TotalTripShopperSelection(Type) {
    var sChange = "";
    if (Type == "trips") {
        if (sVisitsOrGuests == 1)
            sChange = "false";
        else
            sChange = "true";
        sVisitsOrGuests = 1;
        sBevarageSelctionType = [];
        $(".TotalMeasure .trip").show();
        $(".TotalMeasure .Shopper").hide();
        TabType = "trips";
        $("#Left-Frequency").hide();
        SelectedFrequencyList = [];
    }
    else {
        if (sVisitsOrGuests == 2)
            sChange = "false";
        else
            sChange = "true";
        sVisitsOrGuests = 2;
        sBevarageSelctionType = [];
        $(".TotalMeasure .trip").show();
        $(".TotalMeasure .Shopper").hide();
        TabType = "shopper";
        $("#left-panel-frequency ul li[name='STORE IN TRADE AREA']").trigger("click");
        $("#Left-Frequency").show();
    }
    HideOrShowFilters();
    //GetDefaultFrequency();
    //$("#TotalMeasure").trigger("click");

    //if (sVisitsOrGuests == 1) 
    {
        $("#TotalMeasureHeaderMainTrip").show();
        $("#TotalMeasureHeaderMainTrip ul li").css("display", "block");
    }
    //else if (sVisitsOrGuests == 2) {
    //    $("#TotalMeasureHeaderMainShopper").show();
    //    $("#TotalMeasureHeaderMainShopper ul li").css("display", "none");
    //}

    //if (sVisitsOrGuests == 1) 
    {
        $("#TotalMeasureHeaderMainTrip").show();
        $("#TotalMeasureHeaderMainTrip ul li").show();
    }
    //else if (sVisitsOrGuests == 2) {
    //    $("#TotalMeasureHeaderMainShopper").show();
    //    $("#TotalMeasureHeaderMainShopper ul li").show();
    //}
    //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel2").text($(obj).text().trim().toLowerCase());
    //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel2").show();
    //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel2").css("width", "287px");

    //if (sVisitsOrGuests == 1) 
    {
        SetScroll($("#TotalMeasureHeaderMainTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    //else if (sVisitsOrGuests == 2) {
    //    SetScroll($("#TotalMeasureHeaderMainShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //}
}
function DisplaySecondaryTotalFilter(obj) {
    var fitertype = $(obj).attr("filtertype").toLowerCase() == "visits" ? "trips" : "shopper";
    TotalTripShopperSelection(fitertype);
    $("#TotalMeasureHeaderMainTrip .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#TotalMeasureHeaderMainShopper .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    var sPrimaryDemo = $(obj).parent().parent().parent()[0];

    $("#DemoHeadingLevel4").hide();
    $("#FourthLevelAdvancedFilterContent").hide();

    $(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $(sPrimaryDemo).find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    SelectedTotalMeasure = [];
    $(".TotalMeasureHeaderContent").find(".Selected").removeClass("Selected");
    $(".TotalMeasureHeaderContent").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //if (sVisitsOrGuests == "1") 
    {
        //$("#TotalMeasureHeaderContentTrip .DemographicList").hide();
        //$("#TotalMeasureHeaderContentTrip").show();
        //$("#TotalMeasureHeaderContentTrip div[name='" + $(obj).attr("name") + "']").show();
        //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel3").text($(obj).attr("name").toLowerCase());
        //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel3").show();
        //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel3").css("width", "287px");
        //$(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel4").hide();
        //SetScroll($("#TotalMeasureHeaderContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    //else {
    //    $("#TotalMeasureHeaderContentShopper .DemographicList").hide();
    //    $("#TotalMeasureHeaderContentShopper").show();
    //    $("#TotalMeasureHeaderContentShopper div[name='" + $(obj).attr("name") + "']").show();
    //    $(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel3").text($(obj).attr("name").toLowerCase());
    //    $(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel3").show();
    //    $(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel3").css("width", "287px");
    //    $(".AdvancedFiltersDemoHeading #TotalMeasureHeadingLevel4").hide();
    //    SetScroll($("#TotalMeasureHeaderContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //}
    ShowSelectedFilters();
}
function SelectTotalMeasure(obj) {
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    for (var i = 0; i < SelectedTotalMeasure.length; i++) {
        if (SelectedTotalMeasure[i].UniqueId == $(obj).attr("UniqueId")) {
            sCurrentDemoId = i;
        }
    }

    if (sCurrentDemoId.toString() != "") {
        $(obj).removeClass("Selected");
        $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        SelectedTotalMeasure.splice(sCurrentDemoId, 1);
    }
    else {
        //SelectedTotalMeasure = [];
        //$(".TotalMeasureHeaderContent").find(".Selected").removeClass("Selected");
        //$(obj).parent().parent().find(".Selected").removeClass("Selected");
        $(obj).addClass("Selected");
        SelectedTotalMeasure.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("uniqueid"), parent: $(object).attr("parent"), type: $(object).attr("type") });
    }
    ShowSelectedFilters();
}
function RemovetotalMeasure(obj) {
    var ObjData = $("#total-measure-trip span[uniqueid='" + $(obj).attr("Uniqueid") + "']").parent();
    if (ObjData.length > 0)
        SelectTotalMeasure(ObjData);
}
//total Measure Filter Laoding End

function LoadVisitsFiltersLeftPanel(data) {
    html = "";
    var index = 0;
    //var ImageDetails = GetDemographyImagePosition();
    var DataList = [];
    var status = "0";
    if ((Grouplist.length > 0 && Grouplist[0].isGeography == "true")) {
        //DataList = data.AdvancedFilterlist;
        //DataList = _.filter(sFilterData.AdvancedFilterlist, function (i) { return i.Name != Grouplist[0].DBName.split('|')[0].trim() });
        status = "0";
        DataList = _.filter(sFilterData.VisitAdvancedFilter, function (i) {
            status = "0";
            _.filter(i.SecondaryAdvancedFilterlist, function (j) {
                if (j.DBName.split('|')[0].trim() != Grouplist[0].DBName.split('|')[0].trim())
                    status = "0";
                else
                    status = "1";
            });
            if (status == "0")
                return i;
        });
    }
    else {
        if (currentpage != "hdn-analysis-acrosstrips") {
            //dGeo = SelecGeography();
            DataList = (data.VisitAdvancedFilter);//.concat(dGeo);
            if (Grouplist.length > 0) {
                //DataList = _.filter(DataList, function (i) { return i.Name != Grouplist[0].DBName.split('|')[0].trim() });
                status = "0";
                DataList = _.filter(DataList, function (i) {
                    status = "0";
                    _.filter(i.SecondaryAdvancedFilterlist, function (j) {
                        if (j.DBName.split('|')[0].trim() != Grouplist[0].DBName.split('|')[0].trim())
                            status = "0";
                        else
                            status = "1";
                    });
                    if (status == "0")
                        return i;
                });
            }
        }
        else {

            if (Grouplist.length > 0) {
                //DataList = _.filter(sFilterData.AdvancedFilterlist, function (i) { return i.Name != Grouplist[0].DBName.split('|')[0].trim() });
                status = "0";
                DataList = _.filter(sFilterData.VisitAdvancedFilter, function (i) {
                    status = "0";
                    _.filter(i.SecondaryAdvancedFilterlist, function (j) {
                        if (j.DBName.split('|')[0].trim() != Grouplist[0].DBName.split('|')[0].trim())
                            status = "0";
                        else
                            status = "1";
                    });
                    if (status == "0")
                        return i;
                });
            }
            else
                DataList = data.VisitAdvancedFilter;
        }
    }
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            var object = DataList[i];
            if (index == 0) {
                html += "<ul>";
                //ulclose = false;
            }
            var sImageClassName = "";//_.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; })[0].imagePosition : "";
            if (object.Level == "1") {
                html += "<li style=\"display:table;\">";
                //html += "<div Name=\"" + object.Name + "\" class=\"\" onclick=\"DisplaySecondaryDemoFilter(this);\">" + object.Name + "</div>";
                html += "<div onclick=\"DisplaySecondaryDemoFilterAdv(this);\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\" style=\"left:1%;\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";

                //AllDemographics.push(object.Id + "|" + object.Name);
                html += "</li>";

                //if (index == 1) {
                //html += "</ul>";
                //ulclose = true;
                //}
                index++;
            }
        }

        //if (ulclose == false)
        html += "</ul>";

        $("#PrimaryDemoFilterListAdv").html("");
        $("#PrimaryDemoFilterListAdv").html(html);
    }
}
function LoadSecondaryVisitsFiltersLeftPanel(data) {

    html = "";
    var thirdLevelhtml = "";
    var DataList = [];
    AllAdvancedFilterLeft = [];
    //dGeo = SelecGeography();
    DataList = (data.VisitAdvancedFilter);//.concat(dGeo);
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].SecondaryAdvancedFilterlist.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
                thirdLevelhtml += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"display:none;\"><ul>";
                for (var j = 0; j < DataList[i].SecondaryAdvancedFilterlist.length; j++) {
                    var object = DataList[i].SecondaryAdvancedFilterlist[j];
                    if (DataList[i].Level == "1") {
                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(DataList, function (u) {
                            return ((u.FullName == object.Name) && (u.Id == object.MetricId) && (u.Level == object.Level));
                        });
                        if (k.length <= 0) {
                            if (object.active != "false") {
                                html += "<div onclick=\"SelectDemographic(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                AllAdvancedFilterLeft.push(object.UniqueId + "|" + object.Name);
                            } else
                                html += "<div onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                        }
                        else {
                            if (object.active != "false")
                                html += "<div onclick=\"DisplayThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                    }
                    else {
                        thirdLevelhtml += "<div onclick=\"SelectDemographic(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" isGeography=\"" + object.isGeography + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                        AllAdvancedFilterLeft.push(object.UniqueId + "|" + object.Name);
                    }
                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }
    $("#SecondaryDemoFilterListAdv").html("");
    $("#SecondaryDemoFilterListAdv").html(html);
    $("#ThirdDemoFilterListAdv").html("");
    $("#ThirdDemoFilterListAdv").html(thirdLevelhtml);

}

function DisplaySecondaryDemoFilter(obj) {
    $("#PrimaryDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    var sPrimaryDemo = $(obj).parent().parent().parent()[0];
    if ($(obj).attr("Name") == "Geography") {
        $("#SecondaryAdvancedFilterContent .DemographicList[name='Geography']").find(".Selected").removeClass("Selected");
        $("#SecondaryAdvancedFilterContent .DemographicList[name='Geography']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        LoadSecondaryAdvancedFilters(sFilterData);
        SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
            return obj.isGeography !== 'true';
        });
        _.each(SelectedDempgraphicList, function (i) {
            var obj = $("#SecondaryAdvancedFilterContent .DemographicList span[uniqueid='" + i.UniqueId + "']").parent();
            obj.addClass("Selected");
        });
        AddFrequencyinAdvFilters();
    }
    $("#DemoHeadingLevel4").hide();
    $("#FourthLevelAdvancedFilterContent").hide();

    $(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $(sPrimaryDemo).find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    $("#SecondaryAdvancedFilterContent .DemographicList").hide();
    $("#ThirdLevelAdvancedFilterContent .DemographicList").hide();
    $("#ThirdLevelAdvancedFilterContent").hide();
    $("#SecondaryAdvancedFilterContent").show();
    $("#SecondaryAdvancedFilterContent div[name='" + $(obj).attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").text($(obj).attr("name"));
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").css("width", "287px");
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").hide();
    DisplayHeightDynamicCalculation("AdvancedFilter");
    SetScroll($("#SecondaryAdvancedFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    ShowSelectedFilters();
    $(".lft-popup-ele").mouseover(function () {
        if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
    });
    $(".lft-popup-ele").mouseleave(function () {
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
    });
}
function DisplaySecondaryDemoFilterAdv(obj) {
    $("#PrimaryDemoFilterListAdv .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    var sPrimaryDemo = $(obj).parent().parent().parent()[0];
    if ($(obj).attr("Name") == "Geography") {
        $("#SecondaryAdvancedFilterContentAdv .DemographicList[name='Geography']").find(".Selected").removeClass("Selected");
        $("#SecondaryAdvancedFilterContentAdv .DemographicList[name='Geography']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        LoadSecondaryAdvancedFilters(sFilterData);
        SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
            return obj.isGeography !== 'true';
        });
        _.each(SelectedDempgraphicList, function (i) {
            var obj = $("#SecondaryAdvancedFilterContentAdv .DemographicList span[uniqueid='" + i.UniqueId + "']").parent();
            obj.addClass("Selected");
        });
    }
    $("#DemoHeadingLevel4").hide();
    $("#FourthLevelAdvancedFilterContentAdv").hide();

    $(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $(sPrimaryDemo).find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    $("#SecondaryAdvancedFilterContentAdv .DemographicList").hide();
    $("#ThirdLevelAdvancedFilterContentAdv .DemographicList").hide();
    $("#ThirdLevelAdvancedFilterContentAdv").hide();
    $("#SecondaryAdvancedFilterContentAdv").show();
    $("#SecondaryAdvancedFilterContentAdv div[name='" + $(obj).attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").text($(obj).attr("name"));
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").css("width", "287px");
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").hide();
    DisplayHeightDynamicCalculation("AdvancedFilter");
    SetScroll($("#SecondaryAdvancedFilterContentAdv"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    ShowSelectedFilters();
    $(".lft-popup-ele").mouseover(function () {
        if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
    });
    $(".lft-popup-ele").mouseleave(function () {
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
    });
}
function DisplayThirdLevelDemoFilter(obj) {
    $("#SecondaryDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    $("#DemoHeadingLevel4").hide();
    $("#FourthLevelAdvancedFilterContent").hide();
    $("#ThirdDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });

    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    //$(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $("#SecondaryDemoFilterList div[onclick='DisplayThirdLevelDemoFilter(this);']").removeClass("Selected");
    $("#SecondaryDemoFilterList div[onclick='DisplayThirdLevelDemoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    $("#ThirdLevelAdvancedFilterContent .DemographicList").hide();
    $("#ThirdLevelAdvancedFilterContent").show();
    $("#ThirdLevelAdvancedFilterContent div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").text($(obj).find(".lft-popup-ele-label").attr("name"));
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").css("width", "287px");

    //added by Nagaraju for Beverages date: 06-03-2017
    if ($(obj).hasClass("gouptype")) {
        $("#GroupTypeContent .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#GroupTypeContent ul li").removeClass("Selected");
        $("#GroupTypeContent ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$(obj).addClass("Selected");
        var sPrimaryDemo = $(obj);
        $(obj).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
        Grouplist = [];
        $("#GroupTypeContent ul .lft-popup-ele[onclick='SelecGroupMetricName(this);']").removeClass("Selected");
        $("#GroupTypeContentSub ul .lft-popup-ele").removeClass("Selected");
        $("#GroupTypeContentSub ul .lft-popup-ele").find(".ArrowContainerdiv").css("background-color", "#58554D");
    }

    $("#GroupTypeContentSub .DemographicList").hide();
    $("#GroupTypeContentSub .DemographicList ul .lft-popup-ele").hide();
    $("#GroupTypeContentSub").show();
    $("#GroupTypeContentSub div[mericname='" + $(obj).attr("name") + "'][metricid='" + $(obj).attr("metricid") + "']").show()
    $("#GroupTypeContentSub div[mericname='" + $(obj).attr("name") + "'][metricid='" + $(obj).attr("metricid") + "']").parent("ul").parent(".DemographicList").show();
    //
    DisplayHeightDynamicCalculation("AdvancedFilter");
    SetScroll($("#ThirdLevelAdvancedFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    ShowSelectedFilters();
    if ($(obj).attr("name") != undefined && $(obj).attr("name") != "" && $(obj).attr("name") != null)
        $("#grouptypeHeadingLevel4").html($(obj).attr("name").toLowerCase());
    $("#grouptypeHeadingLevel4").show();
}
function DisplayFourthLevelDemoFilter(obj) {

    $("#ThirdDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    //$(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $("#ThirdDemoFilterList div[onclick='DisplayFourthLevelDemoFilter(this);']").removeClass("Selected");
    $("#ThirdDemoFilterList div[onclick='DisplayFourthLevelDemoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    $("#FourthLevelAdvancedFilterContent .DemographicList").hide();
    $("#FourthLevelAdvancedFilterContent").show();
    $("#FourthLevelAdvancedFilterContent div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel4").text($(obj).find(".lft-popup-ele-label").attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel4").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel4").css("width", "287px");
    DisplayHeightDynamicCalculation("AdvancedFilter");
    SetScroll($("#FourthLevelAdvancedFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function SelectDemographic(obj) {
    if ($(obj).hasClass("FrequencyItem") || $(obj).attr('filtertype') == "FREQUENCY") {
        SelectFrequency(obj);
    }
    else if ($(obj).find("span").attr("isgeography") == "true") {
        var object = $(obj).find(".lft-popup-ele-label");
        if ($(obj).parent().parent().attr("level") == "level5") {
            $("#AdvFilterDivId .level3 *").removeClass("Selected");
        }
        else {
            $("#AdvFilterDivId .level5").hide();
            //$("#FourthLevelAdvancedFilterContent").hide();
            $("#DemoHeadingLevel4").hide();
            $("#AdvFilterDivId .level5 *").removeClass("Selected").addClass("Not-Selected-Channel");
            //$("#FourthLevelAdvancedFilterContent .DemographicList div[Level = 'FouthLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#FourthLevelAdvancedFilterContent .DemographicList div[Level = 'FouthLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[onclick='DisplayFourthLevelGeoFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
            $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            //$("#ThirdLevelAdvancedFilterContent .DemographicList div[onclick='DisplayFourthLevelGeoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#ThirdDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            var sPrimaryDemo = $(obj);
            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
        }
        var sCurrentDemoId = "";
        var CompCurrentName = "";
        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            if (SelectedDempgraphicList[i].UniqueId == $(object).attr("uniqueid")) {
                sCurrentDemoId = i;
            }
        }


        if (($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "") && (CompCurrentName == "" || CompCurrentName > 0)) {
            $(obj).removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
            SelectedDempgraphicList.splice(sCurrentDemoId, 1);
        }
        else {
            if (SelectedDempgraphicList.length > 0)
                CompCurrentName = GetArrayName($(obj).find('span'), SelectedDempgraphicList);

            if (CompCurrentName.toString() != "" && CompCurrentName != -1) { return; }

            if (SelectedDempgraphicList.length > 0) {
                var sGeoList = SelectedDempgraphicList.filter(function (obj) {
                    return obj.isGeography == 'true';
                });
                if (sGeoList.length > 0) {
                    if (sGeoList[0].parentName.trim() != $(obj).find("span").attr("parentname").trim()) {
                        SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                            return obj.isGeography !== 'true';
                        });
                        //Total
                        if ($(obj).find("span").text().trim() == "Total") {
                            $("#AdvFilterDivId .level4 *").removeClass("Selected");
                            //$("#ThirdLevelAdvancedFilterContent *").removeClass("Selected");
                            $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            $("#AdvFilterDivId .level5 *").removeClass("Selected");
                            $("#AdvFilterDivId .level5 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            $("#AdvFilterDivId .level4").hide();
                            $("#AdvFilterDivId .level5").hide();
                            $("#DemoHeadingLevel3").hide();
                            $("#DemoHeadingLevel4").hide();
                            $("#SecondaryDemoFilterList .sidearrw_OnCLick").each(function (i, j) {
                                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                            });
                            var sPrimaryDemo = $(obj);
                            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
                            $("#SecondaryDemoFilterList div[onclick='DisplayThirdLevelDemoFilter(this);']").removeClass("Selected");
                            $("#SecondaryDemoFilterList div[onclick='DisplayThirdLevelDemoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                        }
                        else {
                            if ($(obj).find("span").attr("parentname").trim() == "Trade Areas") {
                                $("#AdvFilterDivId .level4 *").removeClass("Selected");
                                $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                            else {
                                $("#AdvFilterDivId .level3 li[is-geography='true'][onclick='SelectDemographic(this);']").removeClass("Selected");
                                $("#AdvFilterDivId .level3 li[is-geography='true'][onclick='SelectDemographic(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                $("#AdvFilterDivId .level4 li[onclick='SelectDemographic(this);']").removeClass("Selected");
                                $("#AdvFilterDivId .level4 liv[onclick='SelectDemographic(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                        }


                        $(obj).addClass("Selected");
                        SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography"), isTripFilter: $(object).attr("isTrip") == undefined ? $(object).parent().attr("isTrip") : $(object).attr("isTrip") });
                    }
                    else {
                        var sTradeAreaSingleSelction = "0";
                        _.each(SelectedDempgraphicList, function (i, j) {
                            if (i.Name == "Albertson's/Safeway Corporate Net Trade Area" || i.Name == "HEB Trade Area" || i.Name == "Kroger Trade Area" || i.Name == "Publix Trade Area")
                                sTradeAreaSingleSelction = "1";
                            else
                                sTradeAreaSingleSelction = "0";
                        });
                        if (sTradeAreaSingleSelction == "1") {

                            //SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                            //    return obj.isGeography !== 'true';
                            //});
                            $("#AdvFilterDivId .level5 *").removeClass("Selected");
                            $("#AdvFilterDivId .level5 *").find(".ArrowContainerdiv").css("background-color", "#58554D");

                        }
                        if ($(obj).find("span").attr("parentname").trim() == "Trade Areas") {
                            SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                                return obj.isGeography !== 'true';
                            });
                            $("#AdvFilterDivId .level3 *").removeClass("Selected");
                            $("#AdvFilterDivId .level4 *").removeClass("Selected");
                            $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            $("#AdvFilterDivId .level4 .sidearrw_OnCLick").each(function (i, j) {
                                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                            });
                            var sPrimaryDemo = $(obj);
                            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
                            $("#AdvFilterDivId .level5 *").removeClass("Selected");
                            $("#AdvFilterDivId .level5 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                        }
                        if ($(obj).find("span").attr("parentname").trim() == "Albertson's/Safeway Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Circle K Trade Areas" || $(obj).find("span").attr("parentname").trim() == "HEB Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Kroger Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Publix Trade Areas") {
                            if ($(obj).find("span.Geotooltipimage").length == 1 || $(obj).find("span").attr("parentname").trim() == "Albertson's/Safeway Trade Areas") {
                                SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                                    return obj.isGeography !== 'true'
                                });
                                $("#AdvFilterDivId .level4 *").removeClass("Selected");
                                $("#AdvFilterDivId .level5 *").removeClass("Selected");
                                $("#AdvFilterDivId .level5 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                            else {
                                var parentObj = obj;
                                var topItem = SelectedDempgraphicList.filter(function (obj) {
                                    return obj.isGeography === 'true' && (obj.Name == $(parentObj).find("span").attr("parentname").trim().substring(0, $(parentObj).find("span").attr("parentname").trim().length - 1));
                                });
                                SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                                    return obj.isGeography !== 'true' || (obj.Name != $(parentObj).find("span").attr("parentname").trim().substring(0, $(parentObj).find("span").attr("parentname").trim().length - 1));
                                });

                                if (topItem.length > 0) $("span[uniqueid='" + topItem[0].UniqueId + "']").parent().removeClass("Selected");
                            }
                        }

                        $(obj).addClass("Selected");
                        SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography"), isTripFilter: $(object).attr("isTrip") == undefined ? $(object).parent().attr("isTrip") : $(object).attr("isTrip") });
                    }
                }
                else {
                    $(obj).addClass("Selected");
                    SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography"), isTripFilter: $(object).attr("isTrip") == undefined ? $(object).parent().attr("isTrip") : $(object).attr("isTrip") });
                }
            }
            else {
                $(obj).addClass("Selected");
                SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography"), isTripFilter: $(object).attr("isTrip") == undefined ? $(object).parent().attr("isTrip") : $(object).attr("isTrip") });
            }
        }
    }
    else {
        var object = $(obj).find(".lft-popup-ele-label");
        if ($(obj).parent().parent().attr("level") == "level5") {
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[Level = 'ThirdLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[Level = 'ThirdLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
        else {
            $("#AdvFilterDivId .level5").hide();
            $("#DemoHeadingLevel4").hide();
            $("#AdvFilterDivId .level5 *").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#AdvFilterDivId .level5 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#AdvFilterDivId .level4 *").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
            $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#AdvFilterDivId .level4 .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            var sPrimaryDemo = $(obj);
            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
        }
        var sCurrentDemoId = "";
        var CompCurrentName = "";
        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            if (SelectedDempgraphicList[i].UniqueId == $(object).attr("uniqueid")) {
                sCurrentDemoId = i;
            }
        }


        if (($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "") && (CompCurrentName == "" || CompCurrentName > 0)) {
            $(obj).removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
            SelectedDempgraphicList.splice(sCurrentDemoId, 1);
        }
        else {
            if (SelectedDempgraphicList.length > 0)
                CompCurrentName = GetArrayName($(obj).find('span'), SelectedDempgraphicList);

            if (CompCurrentName.toString() != "" && CompCurrentName != -1) { return; }

            $(obj).addClass("Selected");
            SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography"), isTripFilter: $(object).attr("isTrip") == undefined ? $(object).parent().attr("isTrip") : $(object).attr("isTrip") });
        }
    }
    ShowSelectedFilters();
}
function RemoveDemographic(obj) {
    var ObjData = $("#AdvFilterDivId .level2 span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $("#AdvFilterDivId .level3 span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $("#AdvFilterDivId .level4 span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();


    var ObjData = $("#AdvFilterDivId .level2 span[uniqueId='" + $(obj).attr("uniqueid") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $("#AdvFilterDivId .level3 span[uniqueId='" + $(obj).attr("uniqueid") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $("#AdvFilterDivId .level4 span[uniqueId='" + $(obj).attr("uniqueid") + "']").parent();

    if ((ObjData.length <= 0) && ((currentpage.indexOf("hdn-report") > -1) && currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive" && currentpage != "hdn-report-comparebeveragesmonthlypluspurchasers" && currentpage != "hdn-report-beveragemonthlypluspurchasersdeepdive")) {
        var ObjData = $("#AdvFilterDivId .level2 span[uniqueId='" + $(obj).attr("uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#AdvFilterDivId .level3 span[uniqueId='" + $(obj).attr("uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#AdvFilterDivId .level4 span[uniqueId='" + $(obj).attr("uniqueid") + "']").parent();
    }

    if (ObjData.length <= 0)
        ObjData = $("#AdvFilterDivId li[uniqueid='" + $(obj).attr("UniqueId") + "']");
    SelectDemographic(ObjData);
}
function SelecGroup(obj) {
    $("#GroupTypeHeaderContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    //if (Grouplist.length > 0) {
    //if (Grouplist[0].DBName.split('|')[0].trim().toUpperCase() != $(obj).attr("dbname").toUpperCase())
    //if ($(obj).attr("name").toLocaleLowerCase() != "main store/favorite store" && currentpage != "hdn-dashboard-pathtopurchase")
    //    Grouplist = [];
    //}
    //$("#grouptypeHeadingLevel4").hide();
    $("#GroupTypeGeoContentSub").hide();
    if ($(obj).attr("Name") == "Geography") {
        $("#SecondaryAdvancedFilterContent .DemographicList[name='Geography']").find(".Selected").removeClass("Selected");
        $("#SecondaryAdvancedFilterContent .DemographicList[name='Geography']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //LoadGroupTypeNames(sFilterData);
        //added by Nagaraju for Geography date: 07-03-2017
        UpdateGeography();
    }
    $("#GroupTypeHeaderContent ul li[onclick ='SelecGroup(this);']").removeClass("Selected");
    $("#GroupTypeHeaderContent ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    if (currentpage != "hdn-dashboard-pathtopurchase")
        $("#GroupTypeContent div").removeClass("Selected");

    $("#GroupTypeContent div").find(".ArrowContainerdiv").css("background-color", "#58554D");
    $("#GroupTypeContentSub div").removeClass("Selected");
    $("#GroupTypeContentSub div").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    //$("#groupDivId *").removeClass("Selected");
    $("#GroupTypeContent ul").hide();
    $("#GroupTypeContent").show();
    $("#GroupTypeContentSub").hide();
    //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").hide();
    $("#GroupTypeContent div[name='" + $(obj).attr("name") + "']").show();
    $("#GroupTypeContent div[name='" + $(obj).attr("name") + "'] ul").show();
    //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").text($(obj).attr("name").toUpperCase());
    //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").show();
    //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").css("width", "287px");
    HideAdvFilterOnGroupSelect();
    HideMeasureFilterOnGroupSelect();
    ShowSelectedFilters();
    DisplayHeightDynamicCalculation("group");
    //$("#grouptypeHeadingLevel4").hide();
    //$("#grouptypeHeadingLevel3").html($(obj).attr("name"));
    //$("#grouptypeHeadingLevel3").show();
    $(".GroupType").css("width", "auto")
}
function SelecGroupMetricName(obj) {
    if (currentpage == "hdn-dashboard-pathtopurchase")
        CustomBaseFlag = 1;
    for (var i = 0; i < $(obj).length; i++) {
        if ($(obj).eq(i).hasClass("Selected")) {
            obj = $(obj).eq(i);
            break;
        }

    }
    if ($(obj).length > 1) {
        for (var i = 0; i < $(obj).length; i++) {
            if ($(obj).children().eq(i).attr("data-isselectable")) {
                obj = $(obj).eq(i);
                break;
            }

        }
    }
    Groupfiltertype = $(obj).attr("filtertype");
    var measureId = "#retailer-measure div[level-id='1']";
    var measureOnclick = "DisplayMeasureList(this);";
    if (currentpage.indexOf("hdn-crossretailer-totalrespondentstripsreport") > -1) {
        measureId = "#total-measure-trip";
        measureOnclick = "DisplaySecondaryTotalFilter(this);";
    }
    else if (currentpage.indexOf("hdn-analysis-withintrips") > -1) {
        if (Comparisonlist.length > 0 && Comparisonlist[0].LevelDesc.toLocaleLowerCase().indexOf("channels|") > -1)
            measureId = "#CorrespondenceMeasureDivId";
        else
            measureId = "#CorrespondenceMeasureDivId";
        measureOnclick = "SelectAdvanceAnalyticsTrips(this);";
    }
    else if (currentpage == "hdn-dashboard-pathtopurchase" > -1) {
        measureId = "#" + $(obj).parent().parent().parent().attr("id");
        measureOnclick = $(obj).attr("onclick");
    }
    //$(".shoppertrip-Toggle").hide();
    if (removeGroupStatus != 2) {
        if (Groupfiltertype.toLocaleLowerCase() == "visits" || Groupfiltertype.toLocaleLowerCase() == "extra beverages" || Groupfiltertype.toLocaleLowerCase() == "online ordering process" || Groupfiltertype.toLocaleLowerCase() == "items purchased" || Groupfiltertype.toLocaleLowerCase() == "auto-replenished deliveries") {
            Grouptype = "visits";
            if ($(measureId + " ul li[filtertype='Shopper']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper']").removeAttr("onclick");
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper'] .show-level").removeAttr("onclick");

            if ($(measureId + " ul li[filtertype='Shopper']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper']").removeAttr("onclick");
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper'] .show-level").removeAttr("onclick");

            $(measureId + " ul li[filtertype='Shopper']").css("background-color", "gray");
            $(measureId + " ul li[filtertype='Shopper'] div").not(".measure-inactive").css("background-color", "gray");
            $(measureId + " ul li[filtertype='Shopper'] .ArrowContainerdiv").not(".measure-inactive").css("background-color", "#58554D");
            $(measureId + " ul li[filtertype='Shopper']").css("cursor", "auto");
            $(measureId + " ul li[filtertype='Shopper'] .measure-inactive").removeClass("ArrowContainerdiv");

            $(measureId + " ul li[filtertype='Visits']").css("cursor", "pointer");
            $(measureId + " ul li[filtertype='Visits']").css("background-color", "");
            $(measureId + " ul li[filtertype='Visits'] div").not(".measure-inactive").css("background-color", "");
            $(measureId + " ul li[filtertype='Visits'] .measure-inactive").addClass("ArrowContainerdiv");

            if ($(measureId + " ul li[filtertype='Visits']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Visits']").attr("onclick", measureOnclick);
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Visits'] .show-level").attr("onclick", measureOnclick);

            //enable Demographics
            $(measureId + " ul li[filtertype='Demographics']").css("cursor", "pointer");
            $(measureId + " ul li[filtertype='Demographics']").css("background-color", "");
            $(measureId + " ul li[filtertype='Demographics'] div").not(".measure-inactive").css("background-color", "");
            $(measureId + " ul li[filtertype='Demographics'] .measure-inactive").addClass("ArrowContainerdiv");

            if ($(measureId + " ul li[filtertype='Demographics']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Demographics']").attr("onclick", measureOnclick);
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Demographics'] .show-level").attr("onclick", measureOnclick);

            //SelectedFrequencyList = [];
            Measurelist = [];
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", TripsMeasureSearchItems);
            SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", TripsTotalMeasures);
        }
        else if (Groupfiltertype.toLowerCase().indexOf("shopper") > -1) {
            Grouptype = "shoppers";
            if ($(measureId + " ul li[filtertype='Visits']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Visits']").removeAttr("onclick");
            else if ($(measureId + " ul li[filtertype='Visits'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Visits'] .show-level").removeAttr("onclick");

            $(measureId + " ul li[filtertype='Visits']").css("background-color", "gray");
            $(measureId + " ul li[filtertype='Visits'] div").not(".measure-inactive").css("background-color", "gray");
            $(measureId + " ul li[filtertype='Visits'] .ArrowContainerdiv").not(".measure-inactive").css("background-color", "#58554D");
            $(measureId + " ul li[filtertype='Visits']").css("cursor", "auto");
            $(measureId + " ul li[filtertype='Visits'] .measure-inactive").removeClass("ArrowContainerdiv");

            $(measureId + " ul li[filtertype='Shopper']").css("cursor", "pointer");
            $(measureId + " ul li[filtertype='Shopper']").css("background-color", "");
            $(measureId + " ul li[filtertype='Shopper'] div").not(".measure-inactive").css("background-color", "");
            $(measureId + " ul li[filtertype='Shopper'] .measure-inactive").addClass("ArrowContainerdiv");

            if ($(measureId + " ul li[filtertype='Shopper']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper']").attr("onclick", measureOnclick);
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper'] .show-level").attr("onclick", measureOnclick);

            Measurelist = [];
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", ShopperMeasureSearchItems);
            SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", ShopperTotalMeasures);
        }
        else {
            Grouptype = "demographics";
            $(measureId + " ul li[filtertype='Shopper']").css("cursor", "pointer");
            $(measureId + " ul li[filtertype='Shopper']").css("background-color", "");
            $(measureId + " ul li[filtertype='Shopper'] div").not(".measure-inactive").css("background-color", "");

            $(measureId + " ul li[filtertype='Visits']").css("cursor", "pointer");
            $(measureId + " ul li[filtertype='Visits']").css("background-color", "");
            $(measureId + " ul li[filtertype='Visits'] div").not(".measure-inactive").css("background-color", "");

            $(measureId + " ul li[filtertype='Shopper'] .measure-inactive").addClass("ArrowContainerdiv");
            $(measureId + " ul li[filtertype='Visits'] .measure-inactive").addClass("ArrowContainerdiv");

            if ($(measureId + " ul li[filtertype='Shopper']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper']").attr("onclick", measureOnclick);
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Shopper'] .show-level").attr("onclick", measureOnclick);

            if ($(measureId + " ul li[filtertype='Visits']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Visits']").attr("onclick", measureOnclick);
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Visits'] .show-level").attr("onclick", measureOnclick);

            //enable Demographics
            $(measureId + " ul li[filtertype='Demographics']").css("cursor", "pointer");
            $(measureId + " ul li[filtertype='Demographics']").css("background-color", "");
            $(measureId + " ul li[filtertype='Demographics'] div").not(".measure-inactive").css("background-color", "");
            $(measureId + " ul li[filtertype='Demographics'] .measure-inactive").addClass("ArrowContainerdiv");

            if ($(measureId + " ul li[filtertype='Demographics']").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Demographics']").attr("onclick", measureOnclick);
            else if ($(measureId + " ul li[filtertype='Shopper'] .show-level").eq(0).hasClass("show-level"))
                $(measureId + " ul li[filtertype='Demographics'] .show-level").attr("onclick", measureOnclick);

            Measurelist = [];
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
            SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", AllTotalMeasures);
        }

        if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
            Groupfiltertype = $(obj).attr("filtertype");
            if (Groupfiltertype.toLocaleLowerCase() == "visits" || Groupfiltertype.toLocaleLowerCase() == "online ordering process" || Groupfiltertype.toLocaleLowerCase() == "items purchased" || Groupfiltertype.toLocaleLowerCase() == "auto-replenished deliveries") {
                $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").removeAttr("onclick");
                $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("background-color", "gray");
                $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("cursor", "auto");
                $("#Left-Frequency").hide();
                SelectedFrequencyList = [];
                SelectedTotalMeasure = [];
                //SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", TripTotalMeasures);
            }
            else {
                $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("cursor", "pointer");
                $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("background-color", "");
                $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").attr("onclick", "DisplayTotalMeasures(this);");
                SelectedTotalMeasure = [];
                //SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", AllTotalMeasures);
            }
        }
        if (currentpage == "hdn-analysis-withintrips") {
            Groupfiltertype = $(obj).attr("filtertype");
            if (Groupfiltertype.toLocaleLowerCase() == "visits" || Groupfiltertype.toLocaleLowerCase() == "online ordering process" || Groupfiltertype.toLocaleLowerCase() == "items purchased" || Groupfiltertype.toLocaleLowerCase() == "auto-replenished deliveries") {
                $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").removeAttr("onclick");
                $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").css("background-color", "gray");
                $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").css("cursor", "auto");
                $("#Left-Frequency").hide();
                SelectedFrequencyList = [];
                //if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name != "Total Visits") {
                //    SelectedFrequencyList = [];
                //    $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
                //    $("#Advanced-Analytics-Select-Variable").trigger("click");
                //    $("#ToShowTrip").trigger("click");
                //}
                SelectedAdvancedAnalyticsList = [];
                SearchFilters("AdvancedAnalytics", "Search-Left-Advanced-AnalyticsFilters", "Advanced-AnalyticsFilter-Left-Search-Content", AllTripAdvancedAnalytics);
            }
            else {
                $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").css("cursor", "pointer");
                $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").css("background-color", "");
                $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").attr("onclick", "AdvancedAnalyticsClickShopper(this);");
                //SelectedFrequencyList = [];
                SelectedAdvancedAnalyticsList = [];
                SearchFilters("AdvancedAnalytics", "Search-Left-Advanced-AnalyticsFilters", "Advanced-AnalyticsFilter-Left-Search-Content", AllAdvancedAnalytics);
            }
        }
    }

    var object = $(obj).find(".lft-popup-ele-label");
    //if ($(obj).parent().parent().attr("level") == "level4") {
    //    // $("#GroupTypeContentSub .DemographicList div[Level = 'ThirdLevel' i]").removeClass("Selected").addClass("Not-Selected-Channel");
    //}
    //else {
    $("#GroupTypeGeoContentSub").hide();
    //$("#grouptypeHeadingLevel4").hide();
    //if (!$(obj).attr("is-geography")) {
    //    $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
    //    $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //}
    $("#GroupTypeContentSub .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
    $("#GroupTypeContentSub .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$("#GroupTypeContentSub .sidearrw_OnCLick").each(function (i, j) {
    //    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //});
    var sPrimaryDemo = $(obj);
    //$(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    //}

    if (currentpage.indexOf("dashboard") == -1) {
        if ($(obj).attr("is-geography") == "true") {
            if (Grouplist.length > 0 && Grouplist[0].isGeography == "false") {
                $("#groupDivId *").removeClass("Selected");
                Grouplist = [];
                $(obj).addClass("Selected");
                Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });
            }
            else {
                var sCurrentDemoId = "";
                for (var i = 0; i < Grouplist.length; i++) {
                    if (Grouplist[i].UniqueId == $(object).attr("UniqueId")) {
                        sCurrentDemoId = i;
                    }
                }
                $(obj).removeClass("Not-Selected-Channel");
                if (($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "")) {
                    $(obj).removeClass("Selected");
                    $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
                    Grouplist.splice(sCurrentDemoId, 1);
                }
                else {
                    if (currentpage.indexOf("hdn-report") > -1 && Grouplist.length == 5)
                        showMessage("YOU CAN MAKE UPTO 5 SELECTIONS")
                    else if (currentpage == "hdn-analysis-acrosstrips" && Grouplist.length >= 4)
                        showMessage("YOU CAN MAKE UPTO 4 SELECTIONS")
                    else {
                        if (!Validate_Common_Groups()) {
                            return false;
                        }
                        $(obj).addClass("Selected");
                        Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });
                        //CustomBaseFlag = 0;
                    }
                }
            }
        }
        else if ($(obj).attr("filtertype").toLowerCase() == "shopper frequency") {
            if (Grouplist.length > 0 && $(obj).attr("filtertype").toLowerCase() != Grouplist[0].filtertype.toLowerCase()) {
                $("#groupDivId *").removeClass("Selected");
                Grouplist = [];
                $(obj).addClass("Selected");
                Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });
            }
            else {
                var sCurrentDemoId = "";
                for (var i = 0; i < Grouplist.length; i++) {
                    if (Grouplist[i].UniqueId == $(object).attr("UniqueId")) {
                        sCurrentDemoId = i;
                    }
                }
                $(obj).removeClass("Not-Selected-Channel");
                if (($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "")) {
                    $(obj).removeClass("Selected");
                    $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
                    Grouplist.splice(sCurrentDemoId, 1);
                }
                else {
                    if (currentpage.indexOf("hdn-report") > -1 && Grouplist.length == 5)
                        showMessage("YOU CAN MAKE UPTO 5 SELECTIONS")
                    else if (currentpage == "hdn-analysis-acrosstrips" && Grouplist.length >= 4)
                        showMessage("YOU CAN MAKE UPTO 4 SELECTIONS")
                    else {
                        if (!Validate_Common_Groups()) {
                            return false;
                        }
                        $(obj).addClass("Selected");
                        Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });
                        //CustomBaseFlag = 0;
                    }
                }
            }
        }
        else if (Grouplist.length > 0 && $(obj).attr("parentname").toLowerCase() != Grouplist[0].parentName.toLowerCase()) {
            $("#groupDivId *").removeClass("Selected");
            Grouplist = [];
            $(obj).addClass("Selected");
            Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });
        }
        else {
            var sCurrentDemoId = "";
            for (var i = 0; i < Grouplist.length; i++) {
                if (Grouplist[i].UniqueId == $(object).attr("UniqueId")) {
                    sCurrentDemoId = i;
                }
            }
            $(obj).removeClass("Not-Selected-Channel");
            if (($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "")) {
                $(obj).removeClass("Selected");
                $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
                Grouplist.splice(sCurrentDemoId, 1);
            }
            else {
                if (currentpage.indexOf("hdn-report") > -1 && Grouplist.length == 5)
                    showMessage("YOU CAN MAKE UPTO 5 SELECTIONS")
                else if (currentpage == "hdn-analysis-acrosstrips" && Grouplist.length >= 4)
                    showMessage("YOU CAN MAKE UPTO 4 SELECTIONS")
                else {
                    if (!Validate_Common_Groups()) {
                        return false;
                    }
                    $(obj).addClass("Selected");
                    Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });
                    //CustomBaseFlag = 0;
                }
            }
        }

    }
    else {
        if ($(obj).hasClass("FrequencyItem") || $(obj).attr("filtertype") == "FREQUENCY") {
            SelectFrequency(obj);
        }
        else if ($(obj).find("span").attr("isgeography") == "true") {
            var object = $(obj).find(".lft-popup-ele-label");
            //if ($(obj).parent().parent().attr("level") == "level5") {
            //    // $("#ThirdLevelAdvancedFilterContent .DemographicList div[Level = 'ThirdLevel' i]").removeClass("Selected").addClass("Not-Selected-Channel");
            //}
            //else {
            //$("#groupDivId .level5").hide();
            //$("#DemoHeadingLevel4").hide();
            //$("#groupDivId ul li[is-geography='true']").removeClass("Selected").addClass("Not-Selected-Channel");
            //$("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#GroupTypeContentSub .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
            $("#GroupTypeContentSub .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#GroupTypeContentSub .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            var sPrimaryDemo = $(obj);
            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
            //}
            var sCurrentDemoId = "";
            var CompCurrentName = "";
            for (var i = 0; i < Grouplist.length; i++) {
                if (Grouplist[i].UniqueId == $(object).attr("uniqueid")) {
                    sCurrentDemoId = i;
                }
            }


            if (($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "") && (CompCurrentName == "" || CompCurrentName > 0)) {
                $(obj).removeClass("Selected");
                $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
                Grouplist.splice(sCurrentDemoId, 1);
            }
            else {
                if (Grouplist.length > 0)
                    CompCurrentName = GetArrayName($(obj).find('span'), Grouplist);

                if (CompCurrentName.toString() != "" && CompCurrentName != -1) { return; }

                if (Grouplist.length > 0) {
                    var sGeoList = Grouplist.filter(function (obj) {
                        return obj.isGeography == 'true';
                    });
                    if (sGeoList.length > 0) {
                        if (sGeoList[0].parentName.trim() != $(obj).find("span").attr("parentname").trim() && $(object).attr("isgeography") == 'true') {
                            Grouplist = Grouplist.filter(function (obj) {
                                return obj.isGeography !== 'true';
                            });
                            //Total
                            if ($(obj).find("span").text().trim() == "Total") {
                                $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                $("#groupDivId ul li[is-geography='true']").hide();
                                $("#groupDivId ul li[is-geography='true']").hide();
                                $("#DemoHeadingLevel3").hide();
                                $("#DemoHeadingLevel4").hide();
                                $("#GroupTypeHeaderContent .sidearrw_OnCLick").each(function (i, j) {
                                    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                                });
                                var sPrimaryDemo = $(obj);
                                $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
                                //$("##groupDivId .level4 div[onclick='SelecGroup(this);']").removeClass("Selected");
                                //$("##groupDivId .level4 div[onclick='SelecGroup(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                            else {
                                if ($(obj).find("span").attr("parentname").trim() == "Trade Areas") {
                                    //$("##groupDivId ul li[is-geography='true']").removeClass("Selected");
                                    //$("##groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                }
                                else {
                                    //$("#groupDivId .level5 li[is-geography='true'][onclick='SelecGroupMetricName(this);']").removeClass("Selected");
                                    //$("#groupDivId .level5 li[is-geography='true'][onclick='SelecGroupMetricName(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                    $("#GroupTypeContent .DemographicList div[onclick='SelecGroupMetricName(this);']").removeClass("Selected");
                                    $("#GroupTypeContent .DemographicList div[onclick='SelecGroupMetricName(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                    $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                    $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                }
                            }


                            $(obj).addClass("Selected");
                            //    Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("ParentLevelId").trim(), parentName: $(object).attr("parentname").trim(), DBName: $(object).attr("DBName"), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
                            Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });

                        }
                        else {
                            var sTradeAreaSingleSelction = "0";
                            _.each(Grouplist, function (i, j) {
                                if (currentpage.indexOf("dashboard") == -1 && (i.Name == "Albertson's/Safeway Corporate Net Trade Area" || i.Name == "HEB Trade Area" || i.Name == "Kroger Trade Area" || i.Name == "Publix Trade Area"))
                                    sTradeAreaSingleSelction = "1";
                                else
                                    sTradeAreaSingleSelction = "0";
                            });
                            if (sTradeAreaSingleSelction == "1") {

                                Grouplist = Grouplist.filter(function (obj) {
                                    return obj.isGeography !== 'true';
                                });
                                $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");

                            }
                            if ($(obj).find("span").attr("parentname").trim() == "Trade Areas") {
                                Grouplist = Grouplist.filter(function (obj) {
                                    return obj.isGeography !== 'true';
                                });
                                $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                $("#groupDivId .level4 .sidearrw_OnCLick").each(function (i, j) {
                                    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                                });
                                var sPrimaryDemo = $(obj);
                                $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
                                //$("#groupDivId .level4 li[onclick='DisplayForthLevelGroupFilter(this);']").removeClass("Selected");
                                //$("#groupDivId .level4 li[onclick='DisplayForthLevelGroupFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }

                            if ($(obj).find("span").attr("parentname").trim() == "Albertson's/Safeway Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Circle K Trade Areas" || $(obj).find("span").attr("parentname").trim() == "HEB Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Kroger Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Publix Trade Areas") {
                                if ($(obj).find("span.Geotooltipimage").length == 1 || $(obj).find("span").attr("parentname").trim() == "Albertson's/Safeway Trade Areas") {
                                    Grouplist = Grouplist.filter(function (obj) {
                                        return obj.isGeography !== 'true'
                                    });
                                    $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                    $("#groupDivId ul li[is-geography='true']").removeClass("Selected");
                                    $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                    $("#AdvancedFilters ul li[is-geography='true']").removeClass("Selected");
                                    $("#AdvancedFilters ul li[is-geography='true']").removeClass("Selected");
                                    $("#AdvancedFilters ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                }
                                else {
                                    var parentObj = obj;
                                    var topItem = Grouplist.filter(function (obj) {
                                        return obj.isGeography === 'true' && (obj.Name == $(parentObj).find("span").attr("parentname").trim().substring(0, $(parentObj).find("span").attr("parentname").trim().length - 1));
                                    });
                                    Grouplist = Grouplist.filter(function (obj) {
                                        return obj.isGeography !== 'true' || (obj.Name != $(parentObj).find("span").attr("parentname").trim().substring(0, $(parentObj).find("span").attr("parentname").trim().length - 1));
                                    });

                                    if (topItem.length > 0) $("span[uniqueid='" + topItem[0].UniqueId + "']").parent().removeClass("Selected");
                                }
                            }
                            $(obj).addClass("Selected");
                            //    Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("ParentLevelId").trim(), parentName: $(object).attr("parentname").trim(), DBName: $(object).attr("DBName"), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
                            Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });

                        }
                    }
                    else {
                        $(obj).addClass("Selected");
                        //Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("ParentLevelId").trim(), parentName: $(object).attr("parentname").trim(), DBName: $(object).attr("DBName"), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
                        Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });

                    }
                }
                else {
                    $(obj).addClass("Selected");
                    //Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("ParentLevelId").trim(), parentName: $(object).attr("parentname").trim(), DBName: $(object).attr("DBName"), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
                    Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });

                }
            }
        }
        else {
            var object = $(obj).find(".lft-popup-ele-label");
            if ($(obj).parent().parent().parent().attr("id") == "GroupTypeGeoContentSub") {
                $("#groupDivId ul li[is-geography='true']").removeClass("Selected").addClass("Not-Selected-Channel");
                $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                $("#groupDivId ul li[is-geography='true']").removeClass("Selected").addClass("Not-Selected-Channel");
                $("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            }
            else {
                //$("#groupDivId .level5").hide();
                //$("#DemoHeadingLevel4").hide();
                //$("#groupDivId ul li[is-geography='true']").removeClass("Selected").addClass("Not-Selected-Channel");
                //$("#groupDivId ul li[is-geography='true']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                //$("#groupDivId .level3 .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
                //$("#groupDivId .level3 .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                //$("#groupDivId .level4 .sidearrw_OnCLick").each(function (i, j) {
                //    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                //});
                var sPrimaryDemo = $(obj);
                $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
            }
            var sCurrentDemoId = "";
            var CompCurrentName = "";
            for (var i = 0; i < Grouplist.length; i++) {
                if (Grouplist[i].UniqueId == $(object).attr("uniqueid")) {
                    sCurrentDemoId = i;
                }
            }


            if (($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "") && (CompCurrentName == "" || CompCurrentName > 0)) {
                $(obj).removeClass("Selected");
                $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
                Grouplist.splice(sCurrentDemoId, 1);
            }
            else {
                if (Grouplist.length > 0)
                    CompCurrentName = GetArrayName($(obj).find('span'), Grouplist);

                if (CompCurrentName.toString() != "" && CompCurrentName != -1) { return; }

                //Grouplist = Grouplist.filter(function (obj) {
                //    return obj.isGeography !== 'true';
                //});

                $(obj).addClass("Selected");
                //Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("ParentLevelId").trim(), parentName: $(object).attr("parentname").trim(), DBName: $(object).attr("DBName"), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
                Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography"), filtertype: $(object).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });

            }
        }
    }
    HideAdvFilterOnGroupSelect();
    HideMeasureFilterOnGroupSelect();
    //LoadAdvancedFilters(sFilterData);
    //LoadSecondaryAdvancedFilters(sFilterData);
    $(".GroupType").css("width", "auto");
    if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
        //update custombase filters
        custombase_AddFilters = [];
        $("#custombase-groupDivId ul li").removeClass("Selected");
        for (var i = 0; i < Grouplist.length; i++) {
            var cus_Obj = $("#custombase-groupDivId div[level-id='" + Grouplist[i].levelId + "'] ul li[parentname='" + Grouplist[i].parentName + "'][uniqueid='" + Grouplist[i].UniqueId + "']");
            if (cus_Obj.length == 0) {
                //$("#custombase-groupDivId div[level-id='2'] ul li[name='GEOGRAPHY']").trigger('click');
                cus_Obj = $("#custombase-groupDivId div[level-id='" + Grouplist[i].levelId + "'] ul li[parentname='" + Grouplist[i].parentName + "'][uniqueid='" + Grouplist[i].UniqueId + "']");
            }
            SelecCustomBaseGroupMetricName(cus_Obj);
        }
        for (var i = 0; i < custombase_Frequency.length; i++) {
            var cus_Obj = $("#custombase-groupDivId ul li[parentname='" + custombase_Frequency[i].Name + "'][parentid='" + custombase_Frequency[i].UniqueId + "'][name='SELECTION']");
            if (cus_Obj.length == 0) {
                cus_Obj = $("#custombase-groupDivId ul li[name='" + custombase_Frequency[i].Name + "'][parentid='" + custombase_Frequency[i].UniqueId + "']")
            }

            $(cus_Obj).addClass("Selected");
        }
    }
    ShowSelectedFilters();
    BuildDynamicTable();
}

function SelecCustomBaseGroupMetricName(obj) {
    if ($(obj).hasClass("FrequencyItem") || $(obj).attr("filtertype") == "FREQUENCY") {
        SelectFrequency(obj);
        return;
    }
    var itemindex = -1;
    for (var i = 0; i < custombase_AddFilters.length; i++) {
        if (custombase_AddFilters[i].UniqueId == $(obj).attr("uniqueid")) {
            itemindex = i;
            break;
        }
    }
    if (itemindex == -1) {
        if ($(obj).attr("is-geography") == 'true') {
            var sGeoList = custombase_AddFilters.filter(function (obj) {
                return obj.isGeography == 'true';
            });
            if (sGeoList.length > 0) {
                if (sGeoList[0].parentName.trim() != $(obj).find("span").attr("parentname").trim()) {
                    $("#custombase-groupDivId ul li[is-geography='true']").removeClass("Selected");
                    custombase_AddFilters = custombase_AddFilters.filter(function (obj) {
                        return obj.isGeography !== 'true';
                    });
                }
            }
        }
        if ($(obj).find("span").attr("parentname").trim() == "Trade Areas"
            || $(obj).find("span").attr("parentname").trim() == "Albertson's/Safeway Trade Areas"
            || $(obj).find("span").attr("parentname").trim() == "Circle K Trade Areas"
            || $(obj).find("span").attr("parentname").trim() == "HEB Trade Areas"
            || $(obj).find("span").attr("parentname").trim() == "Kroger Trade Areas"
            || $(obj).find("span").attr("parentname").trim() == "Publix Trade Areas") {
            if ($(obj).find("span.Geotooltipimage").length == 1
                || $(obj).find("span").attr("parentname").trim() == "Albertson's/Safeway Trade Areas"
                || $(obj).find("span").attr("parentname").trim() == "Trade Areas") {
                $("#custombase-groupDivId ul li[is-geography='true']").removeClass("Selected");
                custombase_AddFilters = custombase_AddFilters.filter(function (obj) {
                    return obj.isGeography !== 'true';
                });
            }
            else {
                var parentObj = obj;
                var topItem = custombase_AddFilters.filter(function (obj) {
                    return obj.isGeography === 'true' && (obj.Name == $(parentObj).find("span").attr("parentname").trim().substring(0, $(parentObj).find("span").attr("parentname").trim().length - 1));
                });
                custombase_AddFilters = custombase_AddFilters.filter(function (obj) {
                    return obj.isGeography !== 'true' || (obj.Name != $(parentObj).find("span").attr("parentname").trim().substring(0, $(parentObj).find("span").attr("parentname").trim().length - 1));
                });

                if (topItem.length > 0) $("span[uniqueid='" + topItem[0].UniqueId + "']").parent().removeClass("Selected");
            }
        }
        $(obj).addClass("Selected");
        custombase_AddFilters.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name").trim(), FullName: ($(obj).attr("Fullname") != undefined ? $(obj).attr("Fullname").trim() : ""), parentId: $(obj).attr("parentid").trim(), parentName: $(obj).attr("parentname").trim(), UniqueId: $(obj).attr("uniqueid"), isGeography: $(obj).attr("is-geography"), filtertype: $(obj).parent().attr("filtertype"), levelId: $(obj).parent("ul").parent(".Lavel").attr("level-id") });
    }
    else {
        custombase_AddFilters.splice(itemindex, 1);
        $(obj).removeClass("Selected");
    }
    ShowSelectedFilters();
}

function HideAdvFilterOnGroupSelect() {
    var groupname = "";
    var measurename = "";
    $("#AdvFilterDivId div[level-id='1'] ul li").show();
    if (Grouplist.length > 0 || Measurelist.length > 0) {
        if (Grouplist.length > 0)
            groupname = Grouplist[0].parentName;
        if (Measurelist.length > 0)
            measurename = Measurelist[0].parentName;

        for (var levelId = 1; levelId <= $("#AdvFilterDivId .Lavel").length; levelId++) {
            $("#AdvFilterDivId div[level-id='" + levelId + "'] ul li[name='" + groupname + "']").hide();
            $("#AdvFilterDivId div[level-id='" + levelId + "'] ul li[name='" + measurename + "']").hide();
        }

        //for measure
        for (var levelId = 1; levelId <= $("#retailer-measure .Lavel").length; levelId++) {
            $("#retailer-measure div[level-id='" + levelId + "'] ul li[name='" + groupname + "']").hide();
        }
        //$("#MeasureTypeHeaderContentShopper ul").children("li[name='" + groupname + "']").hide();

        if (Grouplist.length > 0 && Grouplist[0].isGeography == "true")
            //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='Geography']").parent("li").hide();
            $("#AdvFilterDivId div[level-id='1'] ul li[name='Geography']").hide();
    }
    else {
        //need to uncomment on demo order coming right rev
        for (var levelId = 1; levelId <= $("#AdvFilterDivId .Lavel").length; levelId++) {
            $("#AdvFilterDivId div[level-id='" + levelId + "'] ul li").css("display", "flex !important");
        }
        for (var levelId = 1; levelId <= $("#retailer-measure .Lavel").length; levelId++) {
            $("#retailer-measure div[level-id='" + levelId + "'] ul li").css("display", "flex !important");
            $("#retailer-measure div[level-id='" + levelId + "'] ul li").removeClass("DNI");
        }
        //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList ul li").show();
        if (ChartTileName == "Demographics") {
            //$("#MeasureTypeHeaderContentTrip ul").eq(0).children("li").show();
            //$("#MeasureTypeHeaderContentShopper ul").eq(0).children("li").show();
        }
    }
    RemoveGroupAdvFilters(groupname, measurename);
}
function RemoveGroupAdvFilters(groupname, measurename) {
    if (groupname != "" || measurename != "") {
        var advfilterlength = SelectedDempgraphicList.length;
        if (SelectedDempgraphicList.length > 0) {
            for (var i = advfilterlength - 1; i >= 0; i--) {
                if (SelectedDempgraphicList[i].parentName == groupname
                    || SelectedDempgraphicList[i].parentName == measurename) {
                    $("#SecondaryAdvancedFilterContent .DemographicList[name='" + groupname + "']").children("ul").children(".lft-popup-ele").children("span[name='" + SelectedDempgraphicList[i].Name + "']").parent(".lft-popup-ele").removeClass("Selected");
                    $("#SecondaryAdvancedFilterContent .DemographicList[name='" + measurename + "']").children("ul").children(".lft-popup-ele").children("span[name='" + SelectedDempgraphicList[i].Name + "']").parent(".lft-popup-ele").removeClass("Selected");
                    SelectedDempgraphicList.splice(i, 1);
                }
            }
        }
        ShowSelectedFilters();
    }
}

function HideAdvFilterOnMeasureSelect() {
    //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList ul li").show();
    $("#AdvFilterDivId div[level-id='1'] ul li").show();

    if (Grouplist.length > 0) {
        var groupname = Grouplist[0].parentName;
        //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='" + groupname + "']").parent("li").hide();
        $("#AdvFilterDivId div[level-id='1'] ul li[name='" + groupname + "']").hide();
        if (Grouplist[0].isGeography == "true") {
            //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='Geography']").parent("li").hide();
            $("#AdvFilterDivId div[level-id='1'] ul li[name='Geography']").hide();
        }
    }
    else {
        $("#AdvFilterDivId div[level-id='1'] ul li").show();
    }

    if (Measurelist.length > 0) {
        var measurename = Measurelist[0].parentName;
        var measurelength = Measurelist[0].metriclist.length;
        if (measurelength > 0) {
            //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='" + measurename + "']").parent("li").hide();
            $("#AdvFilterDivId div[level-id='1'] ul li[name='" + measurename + "']").hide();

            if (Measurelist[0].isGeography == "true") {
                //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='Geography']").parent("li").hide();
                $("#AdvFilterDivId div[level-id='1'] ul li[name='Geography']").hide();
            }
        }
        else {
            //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='" + measurename + "']").parent("li").show();
            $("#AdvFilterDivId div[level-id='1'] ul li[name='" + measurename + "']").show();
        }
    }
    else {
        $("#AdvFilterDivId div[level-id='1'] ul li").show();
        //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList ul li").show();
    }
    //added by Nagaraju D
    //Date: 24-11-2017   

    if (ChartTileName.toLowerCase() == "beverage purchaser") {

        $("#adv-bevselectiontype-freq").show();
        $(".advancedfilter-seperator").show();
        if (sBevarageSelctionType.length == 0) {
            var obj = $("#RightPanelPartial #beverage-frequency ul li[name='Monthly Purchased']");
            var object = $(obj).find(".lft-popup-ele-label");
            $(obj).addClass("Selected");
            sBevarageSelctionType.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), UniqueId: $(obj).attr("uniqueid") });
        }
    }
    else {
        $("#adv-bevselectiontype-freq").hide();
        $(".advancedfilter-seperator").hide();
        sBevarageSelctionType = [];
    }
}

function HideMeasureFilterOnGroupSelect() {
    if (Grouplist.length > 0) {
        var groupname = Grouplist[0].parentName;
        //$('#MeasureScrollDivId').find('#MeasureTypeHeaderContentTrip').find('div[name="' + groupname + '"]').parents('li').eq(0).addClass('DNI');
        //$('#MeasureScrollDivId').find('#MeasureTypeHeaderContentShopper').find('div[name="' + groupname + '"]').parents('li').eq(0).addClass('DNI');
        $("#retailer-measure div[level-id='2'] ul").children("li[name='" + groupname + "']").addClass('DNI');

        if (Grouplist[0].isGeography == "true") {
            //$('#MeasureScrollDivId').find('#MeasureTypeHeaderContentTrip').find('div[name="Geography"]').parents('li').eq(0).addClass('DNI');
            //$('#MeasureScrollDivId').find('#MeasureTypeHeaderContentShopper').find('div[name="Geography"]').parents('li').eq(0).addClass('DNI');
            $("#retailer-measure div[level-id='2'] ul").children("li[name='Geography']").addClass('DNI');

        }
    }
    else {
        //$('#MeasureScrollDivId').find('#MeasureTypeHeaderContentTrip').find('li').removeClass('DNI');
        //$('#MeasureScrollDivId').find('#MeasureTypeHeaderContentShopper').find('li').removeClass('DNI');
        $("#retailer-measure div[level-id='2'] ul").children("li[name='Geography']").removeClass('DNI');
    }
}

function RemoveGroup(obj) {
    var ObjData = "";
    if (currentpage == "hdn-tbl-retailerdeepdive" || currentpage == "hdn-chart-retailerdeepdive" || currentpage == "hdn-analysis-withintrips") {
        if (ObjData.length <= 0)
            ObjData = $("#GroupTypeHeaderContent span[filtertype='Shopper Frequency'][uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#GroupTypeContent div[filtertype='Shopper Frequency']>span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    }
    else if (currentpage == "hdn-dashboard-demographic") {
        if (ObjData.length <= 0)
            ObjData = $("#AdvFilterDivId ul li span[uniqueid='" + $(obj).attr("UniqueId") + "']").parent();
    }
    else {
        ObjData = $("#GroupTypeContent span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#GroupTypeContentSub span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#GroupTypeGeoContentSub span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#GroupTypeHeaderContent span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent().parent();
    }

    if (ObjData.length <= 0)
        ObjData = $("#groupDivId ul li span[uniqueid='" + $(obj).attr("UniqueId") + "']").parent();
    removeGroupStatus = 2;
    SelecGroupMetricName(ObjData);
    removeGroupStatus = 1;
    if (Grouplist.length <= 0) {
        if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
            $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("cursor", "pointer");
            $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").css("background-color", "");
            $("#TotalMeasureShopperTripHeader ul div[name='Shopper Measures']").attr("onclick", "DisplayTotalMeasures(this);");
            SelectedTotalMeasure = [];
            SearchFilters("TotalMeasure", "Search-TotalMeasure-Type", "TotalMeasure-Type-Search-Content", AllTotalMeasures);
        }
        else if (currentpage == "hdn-chart-retailerdeepdive" || currentpage == "hdn-chart-beveragedeepdive") {
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("cursor", "pointer");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("background-color", "");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").attr("onclick", "DisplayMeasureTripShopperList(this);");
            Measurelist = [];
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
        }

        else if (currentpage == "hdn-analysis-withintrips") {
            $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").css("cursor", "pointer");
            $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").css("background-color", "");
            $("#ToShowShooperAndTrips ul div[name='Shopper Measures']").attr("onclick", "AdvancedAnalyticsClickShopper(this);");
            //SelectedFrequencyList = [];
            SelectedAdvancedAnalyticsList = [];
            SearchFilters("AdvancedAnalytics", "Search-Left-Advanced-AnalyticsFilters", "Advanced-AnalyticsFilter-Left-Search-Content", AllAdvancedAnalytics);
        }
    }
    if (currentpage == "hdn-dashboard-pathtopurchase") {
        //update custombase filters
        custombase_AddFilters = [];
        $("#custombase-groupDivId ul li").removeClass("Selected");
        for (var i = 0; i < Grouplist.length; i++) {
            var cus_Obj = $("#custombase-groupDivId div[level-id='" + Grouplist[i].levelId + "'] ul li[parentname='" + Grouplist[i].parentName + "'][name='" + Grouplist[i].Name + "']");
            SelecCustomBaseGroupMetricName(cus_Obj);
        }
    }
    ShowSelectedFilters();
}

function RemoveCustomBaseGroup(obj) {
    var cus_Obj = $("#custombase-groupDivId div[level-id='" + $(obj).attr("levelId") + "'] ul li[parentname='" + $(obj).attr("parentName") + "'][uniqueid='" + $(obj).attr("uniqueid") + "']");
    SelecCustomBaseGroupMetricName(cus_Obj);
    ShowSelectedFilters();
}
//End
//Geography Filter
function LoadGeographyFilters() {
    var DataList = [];
    if (dGeo == null || dGeo == [] || dGeo == "") {
        //dGeo = SelecGeography();
        //added by Nagaraju for default geography
        //Date: 13-04-2017
        dGeo = DefaultGeolist;
    }
    var DataList = dGeo;
    html = "";
    var index = 0;
    //var ImageDetails = GetDemographyImagePosition();

    if (DataList != null) {
        for (var i = 0; i < DataList.length; i++) {
            var object = DataList[i];
            if (index == 0) {
                html += "<ul>";
                //ulclose = false;
            }
            var sImageClassName = "";//_.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.DemographyName == object.Name; })[0].imagePosition : "";
            if (object.Level == "1") {
                html += "<li style=\"display:table;\">";
                //html += "<div Name=\"" + object.Name + "\" class=\"\" onclick=\"DisplaySecondaryDemoFilter(this);\">" + object.Name + "</div>";
                html += "<div onclick=\"DisplaySecondaryGeoFilter(this);\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";

                //AllDemographics.push(object.Id + "|" + object.Name);
                html += "</li>";

                //if (index == 1) {
                //html += "</ul>";
                //ulclose = true;
                //}
                index++;
            }
        }

        //if (ulclose == false)
        html += "</ul>";

        $("#PrimaryGeographyFilterList").html("");
        $("#PrimaryGeographyFilterList").html(html);
    }
}
//added by Nagaraju for filter revamp
function LoadSecondaryGeographyFilters() {
    AddOrUpdatefilter(customRegions[0].Name, customRegions);
    var filter = customRegions;
    var filterid = "soap-geography-data";
    var onclick_event_name = "SelectGeographyData(this);";

    if (filter != undefined && filter != null) {
        //add levels
        $("#" + filterid).html("");
        for (var level = 0; level < filter[0].Levels.length; level++) {
            if (filter[0].Levels[level].LevelItems.length > 0)
                $("#" + filterid).append("<div level-id=\"" + filter[0].Levels[level].Id + "\" level=\"level" + filter[0].Levels[level].Id + "\" class=\"Lavel level" + filter[0].Levels[level].Id + "\" style=\"display:" + (level == 0 ? "inline-block" : "none") + ";\"><ul></ul></div>");
        }
        //add filter items
        for (var level = 0; level < filter[0].Levels.length; level++) {
            for (var i = 0; i < filter[0].Levels[level].LevelItems.length; i++) {
                var obj = filter[0].Levels[level].LevelItems[i];
                var IsActive = getRegionActiveTimePeriod(obj) && !disableGeographiesWithTimeperiod(obj);
                var ToolTip = getRegionTimePeriodToolTip(obj);

                if (obj.HasSubLevel && obj.IsSelectable)
                    html = "<li primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype\"><div class=\"FilterStringContainerdiv\" style=\"\"><span onclick=\"" + onclick_event_name + "\" style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\">" + obj.Name + "</span><div class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                else if (obj.HasSubLevel && IsActive)
                    html = "<li primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype show-level\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\">" + obj.Name + "</span><span style=\"float:left;\" class=\"lft-popup-ele-next Geotooltipimage\" title=\"" + ToolTip + "\"></span><div class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                else if (obj.HasSubLevel && !IsActive)
                    html = "<li primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype show-level in-active-item\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\">" + obj.Name + "</span><span style=\"float:left;\" class=\"lft-popup-ele-next Geotooltipimage\" title=\"" + ToolTip + "\"></span><div class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                else if (!IsActive)
                    html = "<li primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" class=\"lft-popup-ele in-active-item\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"true\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\">" + obj.Name + "</span></li>";
                else
                    html = "<li onclick=\"" + onclick_event_name + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"true\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\">" + obj.Name + "</span></li>";
                if (obj.ToolTip == '') {
                    html = html.replace('Geotooltipimage', '')
                }
                $("#" + filterid + " .level" + filter[0].Levels[level].Id + " ul").append(html);
            }
        }
    }
    $('.Geotooltipimage').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left - width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function LoadSecondaryGeographyFilters2() {
    AllGeographics = [];
    html = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var DataList = [];
    var index = 0;
    if (dGeo == null || dGeo == [] || dGeo == "") {
        dGeo = SelecGeography();
    }
    DataList = dGeo;

    if (DataList != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].SecondaryAdvancedFilterlist.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
                thirdLevelhtml += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"display:none;\"><ul>";
                for (var j = 0; j < DataList[i].SecondaryAdvancedFilterlist.length; j++) {
                    var object = DataList[i].SecondaryAdvancedFilterlist[j];
                    //html += "<li>";
                    if (DataList[i].Level == "1") {
                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(DataList, function (u) {
                            return u.Name.toUpperCase() == object.Name.toUpperCase();
                        });
                        if (k.length <= 0) {
                            if (object.active != "false") {

                                html += "<div onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                AllGeographics.push(object.UniqueId + "|" + object.Name);
                            }
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                        else {
                            if (object.active != "false")
                                html += "<div onclick=\"DisplayThirdLevelGeoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                    }
                    else {
                        var object = DataList[i].SecondaryAdvancedFilterlist[j];
                        if (DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                            thirdLevelhtml += "<div onclick=\"DisplayFourthLevelGeoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].SecondaryAdvancedFilterlist[j].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            //if (indexSubLevel <= 0) {
                            fourthLevelhtml += "<div class=\"DemographicList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                            //    indexSubLevel++;
                            //}
                            for (var l = 0; l < DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                                var object1 = DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                                if (object1.active != "false") {
                                    fourthLevelhtml += "<div Name=\"" + object1.Name + "\" onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" Level=\"FouthLevel\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    AllGeographics.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else
                                    fourthLevelhtml += "<div Name=\"" + object1.Name + "\" onclick=\"\" class=\"lft-popup-ele\" Level=\"FouthLevel\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                            }
                            fourthLevelhtml += "</ul></div>";
                        }
                        else {
                            thirdLevelhtml += "<div onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" style=\"\" Level=\"ThirdLevel\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].SecondaryAdvancedFilterlist[j].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            AllGeographics.push(object.UniqueId + "|" + object.Name);
                        }

                    }

                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }
    $("#SecondaryGeographyFilterContent").html("");
    $("#SecondaryGeographyFilterContent").html(html);
    $("#ThirdGeographyFilterList").html("");
    $("#ThirdGeographyFilterList").html(thirdLevelhtml);
    $("#FourthGeographyFilterList").html("");
    $("#FourthGeographyFilterList").html(fourthLevelhtml);
    SearchFilters("Geography", "Search-GeographyFilters", "GeographyFilter-Search-Content", AllGeographics);
    $('.Geotooltipimage').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left - width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
    $("#SecondaryGeographyFilterContent div[id='100'] ul div").eq(0).trigger("click");
}
function LoadDefaultSecondaryGeographyFilters() {
    AllGeographics = [];
    html = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var DataList = [];
    var index = 0;
    if (dGeo == null || dGeo == [] || dGeo == "") {
        //dGeo = SelecGeography();
        //added by Nagaraju for default geography
        //Date: 13-04-2017
        dGeo = DefaultGeolist;
    }
    DataList = dGeo;

    if (DataList != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].SecondaryAdvancedFilterlist.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
                thirdLevelhtml += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"display:none;\"><ul>";
                for (var j = 0; j < DataList[i].SecondaryAdvancedFilterlist.length; j++) {
                    var object = DataList[i].SecondaryAdvancedFilterlist[j];
                    //html += "<li>";
                    if (DataList[i].Level == "1") {
                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(DataList, function (u) {
                            return u.Name.toUpperCase() == object.Name.toUpperCase();
                        });
                        if (k.length <= 0) {
                            if (object.active != "false") {

                                html += "<div onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                                AllGeographics.push(object.UniqueId + "|" + object.Name);
                            }
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                        else {
                            if (object.active != "false")
                                html += "<div onclick=\"DisplayThirdLevelGeoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                    }
                    else {
                        var object = DataList[i].SecondaryAdvancedFilterlist[j];
                        if (DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist != null && DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length > 0) {
                            thirdLevelhtml += "<div onclick=\"DisplayFourthLevelGeoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span style=\"width:83%;\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].SecondaryAdvancedFilterlist[j].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span style=\"float:left;\" title=\"" + object.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            //if (indexSubLevel <= 0) {
                            fourthLevelhtml += "<div class=\"DemographicList\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" style=\"display:none;\"><ul>";
                            //    indexSubLevel++;
                            //}
                            for (var l = 0; l < DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist.length; l++) {
                                var object1 = DataList[i].SecondaryAdvancedFilterlist[j].SecondaryAdvancedFilterlist[l];
                                if (object1.active != "false") {
                                    fourthLevelhtml += "<div Name=\"" + object1.Name + "\" onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" Level=\"FouthLevel\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    AllGeographics.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else
                                    fourthLevelhtml += "<div Name=\"" + object1.Name + "\" onclick=\"\" class=\"lft-popup-ele\" Level=\"FouthLevel\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                            }
                            fourthLevelhtml += "</ul></div>";
                        }
                        else {
                            thirdLevelhtml += "<div onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" style=\"\" Level=\"ThirdLevel\"><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].SecondaryAdvancedFilterlist[j].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            AllGeographics.push(object.UniqueId + "|" + object.Name);
                        }

                    }

                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }
    $("#SecondaryGeographyFilterContent").html("");
    $("#SecondaryGeographyFilterContent").html(html);
    $("#ThirdGeographyFilterList").html("");
    $("#ThirdGeographyFilterList").html(thirdLevelhtml);
    $("#FourthGeographyFilterList").html("");
    $("#FourthGeographyFilterList").html(fourthLevelhtml);
    SearchFilters("Geography", "Search-GeographyFilters", "GeographyFilter-Search-Content", AllGeographics);
    $('.Geotooltipimage').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left - width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
    $("#SecondaryGeographyFilterContent div[id='100'] ul div").eq(0).trigger("click");
}
function DisplaySecondaryGeoFilter(obj) {

    var sPrimaryDemo = $(obj).parent().parent().parent()[0];
    $(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $(sPrimaryDemo).find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(obj).addClass("Selected");
    $("#SecondaryGeographyFilterContent .DemographicList").hide();
    $("#ThirdGeographyFilterList .DemographicList").hide();
    $("#ThirdGeographyFilterList").hide();
    $("#SecondaryGeographyFilterContent").show();
    $("#SecondaryGeographyFilterContent div[name='" + $(obj).attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").text($(obj).attr("name"));
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").css("width", "287px");
    $("#PrimaryGeographyFilterContent").hide();
    SetScroll($("#SecondaryGeographyFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplayThirdLevelGeoFilter(obj) {
    $("#SecondaryGeographyFilterContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    $("#FourthLevelGeographyFilterContent").hide();
    $("#GeographyHeadingLevel3").hide();

    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    //$(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $("#SecondaryGeographyFilterContent div[onclick='DisplayThirdLevelGeoFilter(this);']").removeClass("Selected");
    $("#SecondaryGeographyFilterContent div[onclick='DisplayThirdLevelGeoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    //$(obj).addClass("Selected");
    $("#ThirdGeographyFilterList .DemographicList").hide();
    $("#ThirdGeographyFilterList").show();
    $("#ThirdGeographyFilterList div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $("#ThirdLevelGeographyFilterContent").css("display", "inline-block");
    $(".AdvancedFiltersDemoHeading #GeographyHeadingLevel2").text($(obj).find(".lft-popup-ele-label").attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #GeographyHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #GeographyHeadingLevel2").css("width", "287px");
    SetScroll($("#ThirdLevelGeographyFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    ShowSelectedFilters();
}
function DisplayFourthLevelGeoFilter(obj) {
    $("#ThirdLevelGeographyFilterContent .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    $("#" + $(obj).parent().parent().parent().attr("id").toString() + " div[onclick='DisplayFourthLevelGeoFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
    $("#" + $(obj).parent().parent().parent().attr("id").toString() + " div[onclick='DisplayFourthLevelGeoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    $("#FourthGeographyFilterList .DemographicList").hide();
    $("#FourthLevelGeographyFilterContent").show();
    $("#FourthGeographyFilterList").show();
    $("#FourthGeographyFilterList div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #GeographyHeadingLevel3").text($(obj).find(".lft-popup-ele-label").attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #GeographyHeadingLevel3").show();
    $(".AdvancedFiltersDemoHeading #GeographyHeadingLevel3").css("width", "287px");
    SetScroll($("#FourthLevelGeographyFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function SelectGeographyData(obj) {
    //$("#SecondaryGeographyFilterContent .sidearrw_OnCLick").each(function (i, j) {
    //    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //});
    //var sPrimaryDemo = $(obj);
    //$(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    var object = $(obj).find(".lft-popup-ele-label");
    if ($(obj).parent().parent().attr("Level") == "level3") {
        //$("#ThirdLevelGeographyFilterContent #ThirdGeographyFilterList .DemographicList div[Level = 'ThirdLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
        $("#soap-geography-data .level1 *").removeClass("Selected").addClass("Not-Selected-Channel");
        $("#soap-geography-data .level2 *").removeClass("Selected").addClass("Not-Selected-Channel");
        $("#ThirdLevelGeographyFilterContent #ThirdGeographyFilterList .DemographicList div[Level = 'ThirdLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
    }
    else {
        //$("#FourthLevelGeographyFilterContent").hide();
        $("#soap-geography-data .level1 *").removeClass("Selected").addClass("Not-Selected-Channel");
        $("#soap-geography-data .level3").hide();
        $("#GeographyHeadingLevel3").hide();
        //$("#FourthLevelGeographyFilterContent .DemographicList div[Level = 'FouthLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
        $("#soap-geography-data .level2 *").removeClass("Selected").addClass("Not-Selected-Channel");

        $("#soap-geography-data .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#FourthLevelGeographyFilterContent .DemographicList div[Level = 'FouthLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#soap-geography-data .level4 *").removeClass("RemoveSelectionClass");
        //$("#FourthLevelGeographyFilterContent .DemographicList div[Level = 'FouthLevel']").removeClass("RemoveSelectionClass");
        //$("#ThirdLevelGeographyFilterContent #ThirdGeographyFilterList .DemographicList div[onclick='DisplayFourthLevelGeoFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
        $("#soap-geography-data .level3 *").removeClass("Selected");
        $("#ThirdLevelGeographyFilterContent #ThirdGeographyFilterList .DemographicList div[onclick='DisplayFourthLevelGeoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#ThirdLevelGeographyFilterContent .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(obj);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    }
    var sCurrentDemoId = "";
    for (var i = 0; i < Geographylist.length; i++) {
        if (Geographylist[i].UniqueId == $(object).attr("uniqueid")) {
            sCurrentDemoId = i;
        }
    }

    if ($(obj).hasClass("Selected") || sCurrentDemoId.toString() != "") {
        $(obj).removeClass("Selected");
        $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(obj).removeClass("RemoveSelectionClass");
        Geographylist.splice(sCurrentDemoId, 1);
    }
    else {
        if (currentpage == "hdn-analysis-acrosstrips") {
            Geographylist = [];
            //$("#SecondaryGeographyFilterContent div[onclick ='SelectGeographyData(this);']").removeClass("Selected");
            $("#soap-geography-data .level3 *").removeClass("Selected").addClass("Not-Selected-Channel");
            //$("#ThirdLevelGeographyFilterContent #ThirdGeographyFilterList .DemographicList div[Level = 'ThirdLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#ThirdLevelGeographyFilterContent #ThirdGeographyFilterList .DemographicList div[Level = 'ThirdLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#soap-geography-data .level4 *").removeClass("Selected").addClass("Not-Selected-Channel");
            //$("#FourthLevelGeographyFilterContent .DemographicList div[Level = 'FouthLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#FourthLevelGeographyFilterContent .DemographicList div[Level = 'FouthLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            if ($(object).attr("name") == "Total") {
                //$("#SecondaryGeographyFilterContent div[onclick ='DisplayThirdLevelGeoFilter(this);']").removeClass("Selected");
                $("#SecondaryGeographyFilterContent").find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                $("#soap-geography-data .level3").hide();
                //$("#ThirdLevelGeographyFilterContent").hide();
                //$("#FourthLevelGeographyFilterContent").hide();
                $("#soap-geography-data .level4").hide();
                //$("#GeographyHeadingLevel2").hide();
                //$("#GeographyHeadingLevel3").hide();
            }

        }
        if (currentpage == "hdn-analysis-acrosstrips" && $(object).attr("name") != "Total") {
            if (Grouplist.length > 0 && Grouplist[0].isGeography == "true")
                Grouplist = [];
            $("#groupDivId *").removeClass("Selected");
            if (SelectedDempgraphicList.length > 0) {
                SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                    return obj.isGeography !== 'true';
                });
            }
        }

        $(obj).addClass("Selected");
        $(obj).addClass("RemoveSelectionClass");
        Geographylist.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: (($(object).attr("uniqueid") == null || $(object).attr("uniqueid") == "null" || $(object).attr("uniqueid") == "") ? "" : $(object).attr("uniqueid")) });
    }

    ShowSelectedFilters();
}
function RemoveGeographyData(obj) {
    var ObjData = $("#soap-geography-data span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    //if (ObjData.length <= 0)
    //    ObjData = $("#FourthGeographyFilterList .DemographicList span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();

    SelectGeographyData(ObjData);
}
//End
function LoadMeasureShopperTripLevel() {
    html = "";
    var datalist = [{
        Id: "1",
        Name: "Trip Measures"
    }, {
        Id: "2",
        Name: "Shopper Measures"
    }];

    html += "<ul>";
    for (var i = 0; i < datalist.length; i++) {
        var object = datalist[i];
        if ((currentpage.indexOf("retailerdeepdive") > -1) || (currentpage.indexOf("beveragedeepdive") > -1))
            html += "<li title=\"" + (object.Name == "Shopper Measures" ? "This measure is valid for only Demographic Groups and Beverage Shopper Groups" : "") + "\" id=\"" + object.Id + "\" Name=\"" + object.Name + "\" class=\"gouptype\" onclick=\"DisplayMeasureTripShopperList(this);\" parentname=\"\">";
        else
            html += "<li id=\"" + object.Id + "\" Name=\"" + object.Name + "\" class=\"gouptype\" onclick=\"DisplayMeasureTripShopperList(this);\" parentname=\"\">";

        html += "<div class=\"FilterStringContainerdiv\">";
        html += "<span class=\"img-retailer\"></span>";
        html += "<div style=\"text-align:left;overflow:hidden;width:82%;float:left;\" id=\"" + object[0] + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\">" + object.Name + "</div>";
        html += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
        html += "</div>";
        html += "</li>";
    }
    html += "</ul>";
    $("#MeasureTypeShopperTripHeader").html("");
    $("#MeasureTypeShopperTripHeader").html(html);
    $("#MeasureTypeShopperTripHeader").hide();
    $(".Geotooltipimage, #MeasureTypeHeaderMainTrip .gouptype, #TotalMeasureHeaderMainTrip ul li, #advanced-analytics-Retailer-Trips .gouptype").hover(function () {
        // Hover over code      
        if (ModuleBlock == "TREND")
            return false;

        var title = $(this).attr('title');
        var GroupNamelist = [];
        var ShopperGrps = [];
        if ($(this).hasClass("gouptype") && ModuleBlock != "TREND") {
            var filtertype = $(this).attr("filtertype").toLocaleLowerCase();
            $("#PrimeGroupTypeHeaderContent ul li").each(function () {
                var groupname = $(this).attr("filtertype").toLocaleLowerCase();
                if (filtertype.indexOf("demographic") > -1) {
                    GroupNamelist.push($(this).attr("primefiltertype"));
                }
                else if (groupname.indexOf(filtertype) > -1 || groupname == "demographics" || groupname == "demographic"
                     || filtertype == "demographics" || filtertype == "demographic") {
                    GroupNamelist.push($(this).attr("primefiltertype"));
                }
            });
        }
        //if (ShopperGrps.length > 0)
        //    title = "This measure is valid for only " + GroupNamelist.join(", ") + " Groups. It will be available for " + ShopperGrps.join(", ") + " shortly.";
        //else
        title = "This measure is valid for only " + GroupNamelist.join(", ") + " Groups";

        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function LoadTotalMeasureShopperTripLevel() {
    html = "";
    var datalist = [{
        Id: "1",
        Name: "Trip Measures"
    }, {
        Id: "2",
        Name: "Shopper Measures"
    }];

    html += "<ul>";
    for (var i = 0; i < datalist.length; i++) {
        var object = datalist[i];


        if (object.Name == "Shopper Measures")
            html += "<div name=\"" + object.Name + "\" title=\"" + (object.Name == "Shopper Measures" ? "This measure is valid for only Demographic Groups and Beverage Shopper Groups" : "") + "\" id=\"ToTotalMeasureShowShopper\" class=\"lft-popup-ele FilterStringContainerdiv lft-popup-ele-label\" style=\"float: left; border-bottom: 1px solid grey; cursor: pointer; min-height: 32px; padding-left: 0px;\" onclick=\"DisplayTotalMeasures(this);\">" + object.Name + "<div class=\"ArrowContainerdiv\" style=\"left:55%;\"><span class=\"lft-popup-ele-next sidearrw\" style=\"float: right; cursor: pointer\"></span></div></div>";
        else
            html += "<div name=\"" + object.Name + "\" id=\"ToTotalMeasureShowTrip\" class=\"lft-popup-ele FilterStringContainerdiv lft-popup-ele-label\" style=\"float: left; border-bottom: 1px solid grey; cursor: pointer; min-height: 32px; padding-left: 0px;\" onclick=\"DisplayTotalMeasures(this);\">" + object.Name + "<div class=\"ArrowContainerdiv\" style=\"left: 64%;\"><span class=\"lft-popup-ele-next sidearrw\" style=\"float: right; cursor: pointer\"></span></div></div>";

        //html += "<div>";
        //html += "<span class=\"img-retailer\"></span>";
        //html += "<div style=\"text-align:left;overflow:hidden;width:82%;float:left;\" id=\"" + object[0] + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\">" + object.Name + "</div>";
        //html += "<span class=\"lft-popup-ele-next sidearrw\"></span>";
        //html += "</div>";
        //html += "</li>";
    }
    html += "</ul>";
    $("#TotalMeasureShopperTripHeader").html("");
    $("#TotalMeasureShopperTripHeader").html(html);
    $(".Geotooltipimage, #TotalMeasureShopperTripHeader div[name='Shopper Measures']").hover(function () {
        // Hover over code      
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function LoadOthersMeasureShopperTripLevel() {
    html = "";
    var datalist = [{
        Id: "1",
        Name: "Trip Measures"
    }, {
        Id: "2",
        Name: "Shopper Measures"
    }];

    html += "<ul>";
    for (var i = 0; i < datalist.length; i++) {
        var object = datalist[i];
        if (object.Name == "Shopper Measures") {
            if (currentpage == "hdn-analysis-withinshopper") {
                html += "<div name=\"" + object.Name + "\" id=\"ToShowShopper\" name=\"shopper\" class=\"lft-popup-ele lft-popup-ele-label FilterStringContainerdiv\" style=\"float: left; border-bottom: 1px solid grey; cursor: pointer; min-height: 32px; padding-left: 0px;\" onclick=\"AdvancedAnalyticsClickShopper(this);\">" + object.Name + "<div class=\"ArrowContainerdiv\" style=\"left:51%\"><span class=\"lft-popup-ele-next sidearrw\" style=\"float: right; cursor: pointer\"></span></div></div>";
            }
            else {
                html += "<div name=\"" + object.Name + "\" id=\"ToShowShopper\" title=\"This measure is valid for only Demographic Groups and Beverage Shopper Groups\" name=\"shopper\" class=\"lft-popup-ele lft-popup-ele-label FilterStringContainerdiv\" style=\"float: left; border-bottom: 1px solid grey; cursor: pointer; min-height: 32px; padding-left: 0px;\" onclick=\"AdvancedAnalyticsClickShopper(this);\">" + object.Name + "<div class=\"ArrowContainerdiv\" style=\"left:51%\"><span class=\"lft-popup-ele-next sidearrw\" style=\"float: right; cursor: pointer\"></span></div></div>";
            }
        }
        else
            html += "<div  name=\"" + object.Name + "\" id=\"ToShowTrip\" class=\"lft-popup-ele lft-popup-ele-label FilterStringContainerdiv\" style=\"float: left; border-bottom: 1px solid grey; cursor: pointer; min-height: 32px; padding-left: 0px;\" onclick=\"AdvancedAnalyticsClickTrip(this);\">" + object.Name + "<div class=\"ArrowContainerdiv\" style=\"left:60.8%\"><span class=\"lft-popup-ele-next sidearrw\" style=\"float: right; cursor: pointer\"></span></div></div>";

        //html += "<div>";
        //html += "<span class=\"img-retailer\"></span>";
        //html += "<div style=\"text-align:left;overflow:hidden;width:82%;float:left;\" id=\"" + object[0] + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\">" + object.Name + "</div>";
        //html += "<span class=\"lft-popup-ele-next sidearrw\"></span>";
        //html += "</div>";
        //html += "</li>";
    }
    html += "</ul>";
    $("#ToShowShooperAndTrips").html("");
    $("#ToShowShooperAndTrips").html(html);
    $(".Geotooltipimage, #ToShowShooperAndTrips div[name='Shopper Measures']").hover(function () {
        // Hover over code      
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });
}
function ShowChannelRetailerVariablesTrips() {
    //$("#advanced-analytics-Channel-Shopper").hide();
    //$("#advanced-analytics-Retailer-Shopper").hide();
    //$("#advanced-analytics-Channel-Trips").hide();
    $("#CorrespondenceMeasureDivId .level1 ul *").show();
    for (var i = 0; i < Comparisonlist.length; i++) {
        if (Comparisonlist[i].LevelDesc.toLocaleLowerCase().indexOf("channels") > -1) {
            $("#CorrespondenceMeasureDivId .level1 ul *").show();
            _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
                if (v.attributes.selid.value == "1") {
                    $(v).show();
                }
                else {
                    $(v).hide();
                }
            })
            //$("#advanced-analytics-Retailer-Trips").hide();
            //$("#advanced-analytics-Channel-Trips").show();
            return true;
            break;
        }
    }
    $("#CorrespondenceMeasureDivId .level1 ul *").show();
    _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
        if (v.attributes.selid.value == "2") {
            $(v).show();
        }
        else {
            $(v).hide();
        }
    })
    //$("#advanced-analytics-Retailer-Trips").show();
}
function ShowChannelRetailerVariables() {
    //$("#advanced-analytics-Channel-Trips").hide();
    //$("#advanced-analytics-Retailer-Trips").hide();
    //$("#advanced-analytics-Channel-Shopper").hide();
    $("#CorrespondenceMeasureDivId .level1 ul *").show();
    for (var i = 0; i < Comparisonlist.length; i++) {
        if (Comparisonlist[i].LevelDesc.toLocaleLowerCase().indexOf("channel") > -1) {
            $("#CorrespondenceMeasureDivId .level1 ul *").show();
            _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
                if (v.attributes.selid.value == "1") {
                    $(v).show();
                }
                else {
                    $(v).hide();
                }
            })
            //$("#advanced-analytics-Retailer-Shopper").hide();
            //$("#advanced-analytics-Channel-Shopper").show();
            return true;
            break;
        }
    }
    $("#CorrespondenceMeasureDivId .level1 ul *").show();
    _.forEach($("#CorrespondenceMeasureDivId .level1 ul>li"), function (v, i) {
        if (v.attributes.selid.value == "2") {
            $(v).show();
        }
        else {
            $(v).hide();
        }
    })
    //$("#advanced-analytics-Retailer-Shopper").show();
}
function AdvancedAnalyticsClickTrip(obj) {
    {
        $("#ToShowShooperAndTrips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(obj);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

        $("#ToShowShooperAndTrips .sidearrw_OnCLick_1").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick_1").removeClass("sidearrw_OnCLick_1").addClass("sidearrw");
        });

        $(sPrimaryDemo).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw_OnCLick_1");

        $("#ToShowShooperAndTrips *").removeClass("Selected");
        $("#ToShowShooperAndTrips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(obj).addClass("Selected");
        SelectedAdvancedAnalyticsList = [];
        $("#advanced-analytics-Retailer-Trips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Channel-Trips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Retailer-Shopper .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Channel-Shopper .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Retailer-Trips *").removeClass("Selected");
        $("#advanced-analytics-Retailer-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#advanced-analytics-Channel-Trips *").removeClass("Selected");
        $("#advanced-analytics-Channel-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#advanced-analytics-Retailer-Shopper *").removeClass("Selected");
        $("#advanced-analytics-Retailer-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#advanced-analytics-Channel-Shopper *").removeClass("Selected");
        $("#advanced-analytics-Channel-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");

        $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
        $("#advancedanalyticsHeadingLevel2").css("display", "none");
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").text($(obj).text().trim().toLowerCase());
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").css("width", "270px");
        sVisitsOrGuests = "1";
        TabType = "trips";
        HideOrShowFilters();
        if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() != "total visits") {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
            //$("#ToShowTrip").trigger("click");
        }
        else {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
            //$("#ToShowTrip").trigger("click");
        }

        ShowChannelRetailerVariablesTrips();
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        SetScroll($(".rgt-cntrl-advanced-analytics-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        //GetDefaultFrequency();
    }
}
function AdvancedAnalyticsClickShopper(obj) {
    if ($(obj).attr("id") == "ToShowShopper") {

        $("#ToShowShooperAndTrips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(obj);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

        $("#ToShowShooperAndTrips .sidearrw_OnCLick_1").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick_1").removeClass("sidearrw_OnCLick_1").addClass("sidearrw");
        });

        $(sPrimaryDemo).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw_OnCLick_1");


        $("#ToShowShooperAndTrips *").removeClass("Selected");
        $("#ToShowShooperAndTrips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(obj).addClass("Selected");
        SelectedAdvancedAnalyticsList = [];
        $("#advanced-analytics-Retailer-Trips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Channel-Trips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Retailer-Shopper .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Channel-Shopper .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $("#advanced-analytics-Retailer-Trips *").removeClass("Selected");
        $("#advanced-analytics-Retailer-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#advanced-analytics-Channel-Trips *").removeClass("Selected");
        $("#advanced-analytics-Channel-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#advanced-analytics-Retailer-Shopper *").removeClass("Selected");
        $("#advanced-analytics-Retailer-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#advanced-analytics-Channel-Shopper *").removeClass("Selected");
        $("#advanced-analytics-Channel-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");

        $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
        $("#advancedanalyticsHeadingLevel2").css("display", "none");
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").text($(obj).text().trim().toLowerCase());
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").css("width", "270px");
        sVisitsOrGuests = "2";
        TabType = "shopper";
        HideOrShowFilters();
        if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() != "monthly +") {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
            //$("#ToShowShopper").trigger("click");
        }
        else {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
            //$("#ToShowShopper").trigger("click");
        }
        ShowChannelRetailerVariables();
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        SetScroll($(".rgt-cntrl-advanced-analytics-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        //GetDefaultFrequency();
    }
}
function AdvancedAnalyticsShopperTripSelection(Type) {
    if (Type.toLowerCase() == "visits")
        Type = "trips";

    if (Type == "trips") {
        // SelectedAdvancedAnalyticsList = [];
        sVisitsOrGuests = "1";
        TabType = "trips";
        HideOrShowFilters();
        if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() != "total visits") {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
        }
        else {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
            //$("#Advanced-Analytics-Select-Variable").trigger("click");
        }
        ShowChannelRetailerVariablesTrips();
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        SetScroll($(".rgt-cntrl-advanced-analytics-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    else if (Type == "shopper") {
        //SelectedAdvancedAnalyticsList = [];

        $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
        //$("#advancedanalyticsHeadingLevel2").css("display", "none");
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").text($(obj).text().trim().toLowerCase());
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        //$(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").css("width", "270px");
        sVisitsOrGuests = "2";
        TabType = "shopper";
        SelectedAdvFilterList = [];
        HideOrShowFilters();
        if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() != "monthly +") {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
        }
        else {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
            //$("#Advanced-Analytics-Select-Variable").trigger("click");
        }
        ShowChannelRetailerVariablesTrips();
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        SetScroll($(".rgt-cntrl-advanced-analytics-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
}
function LoadMeasure(data) {
    var html1 = "";
    var html2 = "";
    var html3 = "";
    var html4 = "";
    var html5 = "";

    var index = 0;
    var datalist = {};
    var searchlist = {};
    AllMeasures = [];
    tripsGroups = [];
    var parentname = "";
    if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "1"))
        datalist = sFilterData.SelTypelist[0].GroupTypelist;
    else if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "2"))
        datalist = sFilterData.SelTypelist[1].GroupTypelist;
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "1"))
        datalist = sFilterData.SelTypelist[2].GroupTypelist;
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "2"))
        datalist = sFilterData.SelTypelist[3].GroupTypelist;

    if ((currentpage.indexOf("retailer") > 0))
        searchlist = (sFilterData.SelTypelist[0].GroupTypelist).concat(sFilterData.SelTypelist[1].GroupTypelist);
    else if ((currentpage.indexOf("beverage") > 0))
        searchlist = (sFilterData.SelTypelist[2].GroupTypelist).concat(sFilterData.SelTypelist[3].GroupTypelist);

    if (sVisitsOrGuests == 1)
        parentname = "Trip";
    else
        parentname = "Shopper";
    $("#MeasureTypeHeaderMainTrip").html("");
    $("#MeasureTypeHeaderMainShopper").html("");
    $("#MeasureTypeHeaderContentTrip").html("");
    $("#MeasureTypeHeaderContentShopper").html("");
    $("#MeasureTypeHeaderContentSubLevelTrip").html("");
    $("#MeasureTypeHeaderContentSubLevelShopper").html("");
    $("#MeasureTypeContentTrip").html("");
    $("#MeasureTypeContentShopper").html("");

    if ((currentpage.indexOf("retailer") > 0))
        datalist = sFilterData.SelTypelist[0].GroupTypelist;
    else if ((currentpage.indexOf("beverage") > 0))
        datalist = sFilterData.SelTypelist[2].GroupTypelist;

    if (datalist != null) {

        for (var i = 0; i < datalist.length; i++) {
            //html1 = "";
            if (i == 0)
                html1 += "<ul>";
            var object = datalist[i];
            html1 += "<li style=\"display:none;\" Id=\"" + object.GroupId + "\" Name=\"" + object.GroupName + "\"  class=\"gouptype\" onclick=\"DisplayMeasureList(this);\">";
            html1 += "<div class=\"FilterStringContainerdiv\">";
            html1 += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" Id=\"" + object.GroupId + "\"  type=\"Main-Stub\"  Name=\"" + object.GroupName + "\">" + object.GroupName + "</div>";
            html1 += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
            html1 += "</div>";
            html1 += "</li>";
            if (i == datalist.length - 1)
                html1 += "</ul>";
            //$("#MeasureTypeHeaderMain").append(html1);

            for (var j = 0; j < datalist[i].PrimaryAdvancedFilter.length; j++) {
                //html2 = "";
                var object1 = datalist[i].PrimaryAdvancedFilter[j];
                if (j == 0)
                    html2 += "<ul name=\"" + datalist[i].PrimaryAdvancedFilter[j].Name + "\">";

                html2 += "<li ParentDetails=\"" + object1.ParentDetails + "\" style=\"display:none;\" FilterTypeId=\"" + object1.FilterTypeId + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\" Name=\"" + object1.Name + "\" class=\"gouptype\" ChartTypePIT=\"" + object1.ChartTypePIT + "\" ChartTypeTrend=\"" + object1.ChartTypeTrend + "\" onclick=\"SelecMeasure(this);\">";
                html2 += "<div class=\"FilterStringContainerdiv\">";
                html2 += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + object1.FilterTypeId + "\" id=\"" + object1.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + object1.ChartTypePIT + "\" ChartTypeTrend=\"" + object1.ChartTypeTrend + "\" Name=\"" + object1.Name + "\">" + object1.Name + "</div>";
                html2 += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
                html2 += "</div>";
                html2 += "</li>";
                if (j == datalist[i].PrimaryAdvancedFilter.length - 1)
                    html2 += "</ul>";
                //$("#MeasureTypeHeaderContent").append(html2);

                for (var k = 0; k < datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist.length; k++) {

                    var object2 = datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k];
                    if (datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist.length > 0) {
                        //html3 = "";
                        if (k == 0)
                            html3 += "<ul Name=\"" + datalist[i].PrimaryAdvancedFilter[j].Name + "\" style=\"display:none;\">";
                        html3 += "<li ParentDetails=\"" + object2.ParentDetails + "\" id=\"" + object2.Id + "\" FilterTypeId=\"" + object1.FilterTypeId + "\" type=\"Sub-Level\" DBName=\"" + object2.DBName + "\" UniqueId=\"" + object2.UniqueId + "\" ChartTypePIT=\"" + object2.ChartTypePIT + "\" ChartTypeTrend=\"" + object2.ChartTypeTrend + "\" shopperdbname=\"" + object2.ShopperDBName + "\" tripsdbname=\"" + object2.TripsDBName + "\" Name=\"" + object2.Name + "\" class=\"gouptype\" onclick=\"SelecMeasure(this);\">";
                        html3 += "<div class=\"FilterStringContainerdiv\">";
                        html3 += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + object1.FilterTypeId + "\" id=\"" + object2.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + object2.ChartTypePIT + "\" ChartTypeTrend=\"" + object2.ChartTypeTrend + "\" Name=\"" + object2.Name + "\">" + object2.Name + "</div>";
                        html3 += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
                        html3 += "</div>";
                        html3 += "</li>";
                        if (k == datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist.length - 1)
                            html3 += "</ul>";

                        //$("#MeasureTypeHeaderContentSubLevel").append(html3);

                        for (var l = 0; l < datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist.length; l++) {
                            var object3 = datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist[l];

                            //html4 = "";
                            if (l == 0)
                                html4 += "<ul uniqueid=\"" + datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].UniqueId + "\" Name=\"" + datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].Name + "\" style=\"display:none;\">";
                            html4 += "<li style=\"white-space:pre-wrap;\" Type=\"Trip\" ParentDetails=\"" + object3.ParentDetails + "\" UId=\"" + object3.FilterTypeId + "|" + object3.Id + "|" + object3.parentId + "\" id=\"" + object3.Id + "|" + object3.Id + "\" type=\"Main-Stub\" DBName=\"" + object3.DBName + "\" UniqueId=\"" + object3.UniqueId + "\" shopperdbname=\"" + object3.ShopperDBName + "\" tripsdbname=\"" + object3.TripsDBName + "\" Name=\"" + object3.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + object3.Name + "</li>";
                            if (l == datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist.length - 1)
                                html4 += "</ul>";
                            if (!IsItemExist(object3.Name, AllMeasures)) {
                                {
                                    AllMeasures.push(object3.UniqueId + "|" + object3.SearchName);
                                    tripsGroups.push(object3.UniqueId + "|" + object3.SearchName);
                                }
                            }
                            //$("#MeasureTypeContent").append(html4);
                        }
                    }
                    else {
                        //html5 = "";
                        if (k == 0)
                            html4 += "<ul uniqueid=\"" + datalist[i].PrimaryAdvancedFilter[j].UniqueId + "\" Name=\"" + datalist[i].PrimaryAdvancedFilter[j].Name + "\" style=\"display:none;\">";
                        html4 += "<li style=\"white-space:pre-wrap;\" Type=\"Trip\" ParentDetails=\"" + object2.ParentDetails + "\" UId=\"" + object2.FilterTypeId + "|" + object2.Id + "|" + object2.parentId + "\" id=\"" + object2.Id + "|" + object2.Id + "\" type=\"Main-Stub\" DBName=\"" + object2.DBName + "\" UniqueId=\"" + object2.UniqueId + "\" shopperdbname=\"" + object2.ShopperDBName + "\" tripsdbname=\"" + object2.TripsDBName + "\" Name=\"" + object2.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + object2.Name + "</li>";
                        if (k == datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist.length - 1)
                            html4 += "</ul>";
                        if (!IsItemExist(object2.Name, AllMeasures)) {
                            AllMeasures.push(object2.UniqueId + "|" + object2.SearchName);
                            tripsGroups.push(object2.UniqueId + "|" + object2.SearchName);
                        }
                        //$("#MeasureTypeContent").append(html5);
                    }
                }
            }
        }

        // html1 = "</ul>";
        $("#MeasureTypeHeaderMainTrip").append(html1);
        $("#MeasureTypeHeaderContentTrip").append(html2);
        $("#MeasureTypeHeaderContentSubLevelTrip").append(html3);
        $("#MeasureTypeContentTrip").append(html4);
        SetScroll($("#MeasureTypeHeaderMainTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#MeasureTypeHeaderContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#MeasureTypeHeaderContentSubLevelTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#MeasureTypeContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if ($("#MeasureTypeHeaderContentSubLevelTrip").is(':visible') && $("#MeasureTypeContentTrip").is(':visible')) {
            //$(".MeasureScrollDiv").css("width", "110%");
            $(".MeasureType").css("width", "95%");
            //$(".Lavel").css("width", "20%");
        }
        else {
            $(".MeasureType").css("width", "auto");
            //$(".Lavel").css("width", "262px");
        }
        SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }

    html1 = "";
    html2 = "";
    html3 = "";
    html4 = "";
    html5 = "";
    if ((currentpage.indexOf("retailer") > 0))
        datalist = sFilterData.SelTypelist[1].GroupTypelist;
    else if ((currentpage.indexOf("beverage") > 0))
        datalist = sFilterData.SelTypelist[3].GroupTypelist;
    if (datalist != null) {
        for (var i = 0; i < datalist.length; i++) {
            //html1 = "";
            if (i == 0)
                html1 += "<ul>";
            var object = datalist[i];
            html1 += "<li style=\"display:none;\" Id=\"" + object.GroupId + "\" Name=\"" + object.GroupName + "\"  class=\"gouptype\" onclick=\"DisplayMeasureList(this);\">";
            html1 += "<div class=\"FilterStringContainerdiv\">";
            html1 += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" Id=\"" + object.GroupId + "\"  type=\"Main-Stub\"  Name=\"" + object.GroupName + "\">" + object.GroupName + "</div>";
            html1 += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
            html1 += "</div>";
            html1 += "</li>";
            if (i == datalist.length - 1)
                html1 += "</ul>";
            //$("#MeasureTypeHeaderMain").append(html1);

            for (var j = 0; j < datalist[i].PrimaryAdvancedFilter.length; j++) {
                //html2 = "";
                var object1 = datalist[i].PrimaryAdvancedFilter[j];
                if (j == 0)
                    html2 += "<ul name=\"" + datalist[i].PrimaryAdvancedFilter[j].Name + "\">";

                html2 += "<li ParentDetails=\"" + object1.ParentDetails + "\" style=\"display:none;\" FilterTypeId=\"" + object1.FilterTypeId + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\" Name=\"" + object1.Name + "\" class=\"gouptype\" ChartTypePIT=\"" + object1.ChartTypePIT + "\" ChartTypeTrend=\"" + object1.ChartTypeTrend + "\" onclick=\"SelecMeasure(this);\">";
                html2 += "<div class=\"FilterStringContainerdiv\">";
                html2 += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + object1.FilterTypeId + "\" id=\"" + object1.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + object1.ChartTypePIT + "\" ChartTypeTrend=\"" + object1.ChartTypeTrend + "\" Name=\"" + object1.Name + "\">" + object1.Name + "</div>";
                html2 += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
                html2 += "</div>";
                html2 += "</li>";
                if (j == datalist[i].PrimaryAdvancedFilter.length - 1)
                    html2 += "</ul>";
                //$("#MeasureTypeHeaderContent").append(html2);

                for (var k = 0; k < datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist.length; k++) {

                    var object2 = datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k];
                    if (datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist.length > 0) {
                        //html3 = "";
                        if (k == 0)
                            html3 += "<ul Name=\"" + datalist[i].PrimaryAdvancedFilter[j].Name + "\" style=\"display:none;\">";
                        html3 += "<li ParentDetails=\"" + object2.ParentDetails + "\" id=\"" + object2.Id + "\" FilterTypeId=\"" + object1.FilterTypeId + "\" type=\"Sub-Level\" DBName=\"" + object2.DBName + "\" UniqueId=\"" + object2.UniqueId + "\" ChartTypePIT=\"" + object2.ChartTypePIT + "\" ChartTypeTrend=\"" + object2.ChartTypeTrend + "\" shopperdbname=\"" + object2.ShopperDBName + "\" tripsdbname=\"" + object2.TripsDBName + "\" Name=\"" + object2.Name + "\" class=\"gouptype\" onclick=\"SelecMeasure(this);\">";
                        html3 += "<div class=\"FilterStringContainerdiv\">";
                        html3 += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + object1.FilterTypeId + "\" id=\"" + object2.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + object2.ChartTypePIT + "\" ChartTypeTrend=\"" + object2.ChartTypeTrend + "\" Name=\"" + object2.Name + "\">" + object2.Name + "</div>";
                        html3 += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
                        html3 += "</div>";
                        html3 += "</li>";
                        if (k == datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist.length - 1)
                            html3 += "</ul>";

                        //$("#MeasureTypeHeaderContentSubLevel").append(html3);

                        for (var l = 0; l < datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist.length; l++) {
                            var object3 = datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist[l];

                            //html4 = "";
                            if (l == 0)
                                html4 += "<ul uniqueid=\"" + datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].UniqueId + "\" Name=\"" + datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].Name + "\" style=\"display:none;\">";
                            html4 += "<li style=\"white-space:pre-wrap;\" Type=\"Shopper\" ParentDetails=\"" + object3.ParentDetails + "\" UId=\"" + object3.FilterTypeId + "|" + object3.Id + "|" + object3.parentId + "\" id=\"" + object3.Id + "|" + object3.Id + "\" type=\"Main-Stub\" DBName=\"" + object3.DBName + "\" UniqueId=\"" + object3.UniqueId + "\" shopperdbname=\"" + object3.ShopperDBName + "\" tripsdbname=\"" + object3.TripsDBName + "\" Name=\"" + object3.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + object3.Name + "</li>";
                            if (l == datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist[k].SecondaryAdvancedFilterlist.length - 1)
                                html4 += "</ul>";
                            if (!IsItemExist(object3.Name, AllMeasures)) {
                                AllMeasures.push(object3.UniqueId + "|" + object3.SearchName + "|" + "1");
                            }
                            //$("#MeasureTypeContent").append(html4);
                        }
                    }
                    else {
                        //html5 = "";
                        if (k == 0)
                            html4 += "<ul uniqueid=\"" + datalist[i].PrimaryAdvancedFilter[j].UniqueId + "\" Name=\"" + datalist[i].PrimaryAdvancedFilter[j].Name + "\" style=\"display:none;\">";
                        html4 += "<li style=\"white-space:pre-wrap;\" Type=\"Shopper\" ParentDetails=\"" + object2.ParentDetails + "\" UId=\"" + object2.FilterTypeId + "|" + object2.Id + "|" + object2.parentId + "\" id=\"" + object2.Id + "|" + object2.Id + "\" type=\"Main-Stub\" DBName=\"" + object2.DBName + "\" UniqueId=\"" + object2.UniqueId + "\" shopperdbname=\"" + object2.ShopperDBName + "\" tripsdbname=\"" + object2.TripsDBName + "\" Name=\"" + object2.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + object2.Name + "</li>";
                        if (k == datalist[i].PrimaryAdvancedFilter[j].SecondaryAdvancedFilterlist.length - 1)
                            html4 += "</ul>";
                        if (!IsItemExist(object2.Name, AllMeasures))
                            AllMeasures.push(object2.UniqueId + "|" + object2.SearchName);
                        //$("#MeasureTypeContent").append(html5);
                    }
                }
            }
        }
        // html1 = "</ul>";
        $("#MeasureTypeHeaderMainShopper").append(html1);
        $("#MeasureTypeHeaderContentShopper").append(html2);
        $("#MeasureTypeHeaderContentSubLevelShopper").append(html3);
        $("#MeasureTypeContentShopper").append(html4);
        SetScroll($("#MeasureTypeHeaderMainShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#MeasureTypeHeaderContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#MeasureTypeHeaderContentSubLevelShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#MeasureTypeContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if ($("#MeasureTypeHeaderContentSubLevelShopper").is(':visible') && $("#MeasureTypeContentShopper").is(':visible')) {
            //$(".MeasureScrollDiv").css("width", "110%");
            $(".MeasureType").css("width", "95%");
            //$(".Lavel").css("width", "20%");
        }
        else {
            $(".MeasureType").css("width", "auto");
            //$(".Lavel").css("width", "262px");
        }
        SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
}

function SelecMeasure(obj) {

    ChartTileName = $(obj).attr("filtertype");
    isPopupVisible = true;
    //MeasureTypeHeaderContent
    if (($("#MeasureTypeContentTrip ul[name='" + $(obj).attr("name") + "']").length <= 0 || $("#MeasureTypeContentShopper ul[name='" + $(obj).attr("name") + "']").length <= 0) && ($("#MeasureTypeHeaderContentSubLevelTrip ul[name='" + $(obj).attr("name") + "']").length > 0 || $("#MeasureTypeHeaderContentSubLevelShopper ul[name='" + $(obj).attr("name") + "']").length > 0)) {
        $("#MeasureTypeHeaderContentTrip .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(obj);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
        $("#MeasureTypeHeaderContentShopper .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(obj);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

        if (isSearch == "0") {
            //if (sVisitsOrGuests == 1)
            {

                $("#MeasureTypeHeaderContentSubLevelTrip").show();
                $("#MeasureTypeHeaderContentSubLevelTrip ul").hide();
                $("#MeasureTypeHeaderContentSubLevelTrip ul li").removeClass("Selected");
                $("#MeasureTypeHeaderContentSubLevelTrip ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
            }
            //else if (sVisitsOrGuests == 2) {
            //    $("#MeasureTypeHeaderContentSubLevelShopper").show();
            //    $("#MeasureTypeHeaderContentSubLevelShopper ul").hide();
            //    $("#MeasureTypeHeaderContentSubLevelShopper ul li").removeClass("Selected");
            //    $("#MeasureTypeHeaderContentSubLevelShopper ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
            //}

            $("#MeasureTypeHeaderContentTrip ul li").removeClass("Selected");
            $("#MeasureTypeHeaderContentTrip ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContentShopper ul li").removeClass("Selected");
            $("#MeasureTypeHeaderContentShopper ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
            //$(obj).addClass("Selected");
            //if (sVisitsOrGuests == 1) 
            {
                $("#MeasureTypeHeaderContentSubLevelTrip ul[name='" + $(obj).attr("name") + "']").show();
            }
            //else if (sVisitsOrGuests == 2) {
            //    $("#MeasureTypeHeaderContentSubLevelShopper ul[name='" + $(obj).attr("name") + "']").show();
            //}

            $("#MeasureTypeContentTrip").hide();
            $("#MeasureTypeContentShopper").hide();
            //$("#MeasuresHeadingLevel3").hide();
            //$(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel4").text($(obj).attr("name").toLowerCase());
            //$(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel4").show();
            //$(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel4").css("width", "287px");
            //$(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel4").css("word-break", "break-all");
        }
        if (($("#MeasureTypeHeaderContentSubLevelTrip").is(':visible') || $("#MeasureTypeHeaderContentSubLevelShopper").is(':visible')) && ($("#MeasureTypeContentTrip").is(':visible') || $("#MeasureTypeContentShopper").is(':visible'))) {
            //$(".MeasureScrollDiv").css("width", "110%");
            $(".MeasureType").css("width", "95%");
            //$(".Lavel").css("width", "20%");
        }
        else {
            $(".MeasureType").css("width", "auto");
            // $(".Lavel").css("width", "262px");
        }
        SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);


        //$(".MeasureType").css("width", "auto");
    }
    else {

        Measurelist = [];
        var chartlist = [];

        chartlist = $(obj).attr("charttypePIT") != undefined ? $(obj).attr("charttypePIT").split("|") : [];
        ChartType = chartlist[0];
        if ($(obj).attr("parentname") != "Demographics") {
            var sChange = "";
            if ($(obj).attr("type").toLocaleLowerCase() == "visits" || $(obj).attr("type").toLocaleLowerCase() == "trips") {
                TabType = "trips";
                if (sVisitsOrGuests == 1)
                    sChange = "false";
                else
                    sChange = "true";
                sVisitsOrGuests = 1;
                sBevarageSelctionType = [];
            }
            else if ($(obj).attr("type").toLocaleLowerCase() == "shopper") {
                TabType = "shopper";
                if (sVisitsOrGuests == 2)
                    sChange = "false";
                else
                    sChange = "true";
                sVisitsOrGuests = 2;
                if (currentpage.indexOf("chart") > -1) {
                    $("#adv-bevselectiontype-freq").hide();
                    sBevarageSelctionType = [];
                }
                else
                    $("#beverage-frequency ul li[uniqueid='1']").trigger("click");
            }
            HideOrShowFilters();
            //TabType = $(obj).attr("name").split(' ')[0].toLowerCase().replace("trip", "trips");

            if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1))
                $("#adv-bevselectiontype-freq").show();
            else {
                $("#adv-bevselectiontype-freq").hide();
                sBevarageSelctionType = [];
            }
            GetDefaultFrequency();
            $(".MeasureType").show();
            if (!($(".adv-fltr-toggle-container").is(":visible")))
                $(".toggle-seperator").hide();
            else
                $(".toggle-seperator").show();
            if ($("#adv-fltr-Chnl").is(":visible") || $("#adv-bevselectiontype-freq").is(":visible"))
                $(".advancedfilter-seperator").show();
            else
                $(".advancedfilter-seperator").hide();
            SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        }

        if (ModuleBlock == "TREND")
            chartlist = $(obj).attr("charttypetrend") != undefined ? $(obj).attr("charttypetrend").split("|") : [];
        else
            chartlist = $(obj).attr("charttypePIT") != undefined ? $(obj).attr("charttypePIT").split("|") : [];

        ChartType = chartlist[0];

        $("#chart-list").html("");
        for (var i = 0; i < chartlist.length; i++) {

            var sImageClassName = ChartImagePosition(chartlist[i].toString());
            //$("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\"><div id=\"TextForChart\">" + chartlist[i] + "</div></div>").appendTo("#chart-list");
            if (chartlist[i].toString() != "Stacked Area")
                $("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\" style=\"background-image: url('../Images/sprite.svg?11');background-position:" + sImageClassName + "\"></div>").appendTo("#chart-list");
        }
        $("#LinkForCharts").show();
        if (isSearch == "0") {
            $("#MeasureTypeHeaderContentSubLevelTrip ul li").removeClass("Selected");
            $("#MeasureTypeHeaderContentSubLevelTrip ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContentSubLevelShopper ul li").removeClass("Selected");
            $("#MeasureTypeHeaderContentSubLevelShopper ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
            if ($(obj).attr("type") != "Sub-Level") {
                $("#MeasureTypeHeaderContentTrip ul li").removeClass("Selected");
                $("#MeasureTypeHeaderContentTrip ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
                $("#MeasureTypeHeaderContentShopper ul li").removeClass("Selected");
                $("#MeasureTypeHeaderContentShopper ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
                //$("#MeasureTypeHeaderContentSubLevelTrip").hide();
                //$("#MeasureTypeHeaderContentSubLevelShopper").hide();
                //$("#MeasuresHeadingLevel4").hide();
                $("#MeasureTypeHeaderContentTrip .sidearrw_OnCLick").each(function (i, j) {
                    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                });
                var sPrimaryDemo = $(obj);
                $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
                $("#MeasureTypeHeaderContentShopper .sidearrw_OnCLick").each(function (i, j) {
                    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                });
                var sPrimaryDemo = $(obj);
                $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

            }

            $("#retailer-measure .level3 *").removeClass("Selected");
            $("#retailer-measure .level4 *").removeClass("Selected");
            $("#retailer-measure .level3 *").find(".ArrowContainerdiv").css("background-color", "#58554D");


            $("#MeasureTypeContentTrip ul").hide();
            $("#MeasureTypeContentTrip").show();
            $("#MeasureTypeContentTrip ul[uniqueid='" + $(obj).attr("uniqueid") + "']").show();
            var count = 0;
            if ($(obj).parent().parent().attr('level-id') == '2') {
                if ($("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).text().trim() == "Top 10") {
                    if (sTrendType == "2" && ModuleBlock == "TREND") {
                        $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(1).addClass("Selected");
                    }
                    else {
                        $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).addClass("Selected");
                    }
                }
                else {
                    if (sTrendType == "2" && ModuleBlock == "TREND") {
                        $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).addClass("Selected");

                    }
                    else {
                        if ($("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").length <= 10) {
                            $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function (i, j) {
                                if ($(j).attr("parentname") == $(obj).attr("name") && $(j).attr("parentid") == $(obj).attr("id")) {
                                    if (count < 10) {
                                        $(j).addClass("Selected");
                                        count++;
                                    }
                                }
                            });
                        }
                        else {
                            $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function (i, j) {
                                if ($(j).attr("parentname") == $(obj).attr("name") && $(j).attr("parentid") == $(obj).attr("id")) {
                                    if (count < 10) {
                                        $(j).addClass("Selected");
                                        count++;
                                    }
                                }
                            });
                        }
                    }
                }
            }
            else if ($(obj).parent().parent().attr('level-id') == '3') {
                if ($("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).text().trim() == "Top 10") {
                    if (sTrendType == "2" && ModuleBlock == "TREND") {
                        $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(1).addClass("Selected");
                    }
                    else {
                        $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).addClass("Selected");
                    }
                }
                else {
                    if (sTrendType == "2" && ModuleBlock == "TREND") {
                        $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).addClass("Selected");

                    }
                    else {
                        if ($("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").length <= 10) {
                            $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function (i, j) {
                                if ($(j).attr("parentname") == $(obj).attr("name") && $(j).attr("parentid") == $(obj).attr("id")) {
                                    if (count < 10) {
                                        $(j).addClass("Selected");
                                        count++;
                                    }
                                }
                            });
                        }
                        else {
                            $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function (i, j) {
                                if ($(j).attr("parentname") == $(obj).attr("name") && $(j).attr("parentid") == $(obj).attr("id")) {
                                    if (count < 10) {
                                        $(j).addClass("Selected");
                                        count++;
                                    }
                                }
                            });
                        }
                    }
                }
            }

            $("#MeasureTypeHeaderContentSubLevelTrip .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            var sPrimaryDemo = $(obj);
            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
            $("#MeasureTypeHeaderContentSubLevelShopper .sidearrw_OnCLick").each(function (i, j) {
                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
            });
            var sPrimaryDemo = $(obj);
            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
        }
        if (($("#MeasureTypeHeaderContentSubLevelTrip").is(':visible') || $("#MeasureTypeHeaderContentSubLevelShopper").is(':visible')) && ($("#MeasureTypeContentTrip").is(':visible') || $("#MeasureTypeContentShopper").is(':visible'))) {
            //$(".MeasureScrollDiv").css("width", "110%");
            //$(".MeasureType").css("width", "95%");
            $(".MeasureType").css("width", "auto");
            // $(".Lavel").css("width", "20%");
        }
        else {
            $(".MeasureType").css("width", "auto");
            // $(".Lavel").css("width", "262px");
        }
        if ($(".MeasureType").width() > window.innerWidth)
            $(".MeasureType").css("width", "95%");
        else
            $(".MeasureType").css("width", "auto");

        SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        $(".shoppertrip-Toggle").hide();
        if ($(obj).attr("parentname") == "Demographics") {
            if (Grouptype == "demographics" || currentpage.indexOf("deepdive") == -1 || ModuleBlock == "TREND")
                $(".shoppertrip-Toggle").show();

            var sChange = "";
            if ($("#guest-visit-toggle").is(":checked") == false) {
                TabType = "trips";
                if (sVisitsOrGuests == 1)
                    sChange = "false";
                else
                    sChange = "true";
                sVisitsOrGuests = 1;
                sBevarageSelctionType = [];
            }
            else if ($("#guest-visit-toggle").is(":checked") == true) {
                TabType = "shopper";
                if (sVisitsOrGuests == 2)
                    sChange = "false";
                else
                    sChange = "true";
                sVisitsOrGuests = 2;
                if (currentpage.indexOf("chart") > -1) {
                    $("#adv-bevselectiontype-freq").hide();
                    sBevarageSelctionType = [];
                }
                else
                    $("#beverage-frequency ul li[uniqueid='1']").trigger("click");
            }
            if (Grouptype != "demographics") {
                if (Grouptype == "visits") {
                    TabType = "trips";
                    sVisitsOrGuests = 1;
                }
                else if (Grouptype == "shoppers") {
                    TabType = "shopper";
                    sVisitsOrGuests = 2;
                }
            }
            HideOrShowFilters();
            //TabType = $(obj).attr("name").split(' ')[0].toLowerCase().replace("trip", "trips");

            if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1))
                $("#adv-bevselectiontype-freq").show();
            else {
                $("#adv-bevselectiontype-freq").hide();
                sBevarageSelctionType = [];
            }
            GetDefaultFrequency();
            $(".MeasureType").show();
            $(".rgt-cntrl-frequency").hide();
            if (!($(".adv-fltr-toggle-container").is(":visible")))
                $(".toggle-seperator").hide();
            else
                $(".toggle-seperator").show();
            if ($("#adv-fltr-Chnl").is(":visible") || $("#adv-bevselectiontype-freq").is(":visible"))
                $(".advancedfilter-seperator").show();
            else
                $(".advancedfilter-seperator").hide();
            SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        }
        else
            $(".shoppertrip-Toggle").hide();
        Measurelist.push({ uniqueid: $(obj).attr("parentid"), parentName: $(obj).attr("name"), filtertypeid: $(obj).attr("filtertypeid"), MetricId: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), DBName: $(obj).attr("name"), charttype: $(obj).attr("charttype"), metriclist: [] });

        var _metriclist = [];
        if ($(obj).parent().parent().attr('level-id') == '2') {
            if (sTrendType == "2" && ModuleBlock == "TREND") {
                $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function (i, j) {
                    if ($(this).attr("parentname") == $(obj).attr("name")) {
                        if ($(this).text().trim() != "Top 10") {
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname") });
                            if (_metriclist.length == 1)
                                return false;
                        }
                    }
                });
            }
            else {
                $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function () {
                    if ($(this).attr("parentname") == $(obj).attr("name")) {
                        if ($(this).text().trim() == "Top 10") {
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname") });
                            return false;
                        }

                        else {
                            if (_metriclist.length < 10) {
                                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname") });

                            }
                        }
                    }
                });
            }
        }
        else if ($(obj).parent().parent().attr('level-id') == '3') {
            if (sTrendType == "2" && ModuleBlock == "TREND") {
                $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function (i, j) {
                    if ($(this).attr("parentname") == $(obj).attr("name")) {
                        if ($(this).text().trim() != "Top 10") {
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname") });
                            if (_metriclist.length == 1)
                                return false;
                        }
                    }
                });
            }
            else {
                $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function () {
                    if ($(this).attr("parentname") == $(obj).attr("name")) {
                        if ($(this).text().trim() == "Top 10") {
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname") });
                            return false;
                        }

                        else {
                            if (_metriclist.length < 10) {
                                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname") });

                            }
                        }
                    }
                });
            }
        }

        if (isSearch == "0") {
            //$(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel3").text($(obj).attr("name"));
            //$(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel3").show();
            //$(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel3").css("width", "287px");
        }
        Measurelist[0].metriclist = _metriclist;
        ShowSelectedFilters();
        var leftPos = $('#MeasureContainerDivId').scrollLeft() + 200;
        $('#MeasureContainerDivId').scrollLeft(leftPos);
        //if (sVisitsOrGuests == 1)
        {
            SetScroll($("#MeasureTypeHeaderContentSubLevelTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        }
        //else if (sVisitsOrGuests == 2) {
        //    SetScroll($("#MeasureTypeHeaderContentSubLevelShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        //}

        //$("#MeasureContainerDivId").animate({ scrollLeft: leftPos + 200 }, 800);
    }

    HideAdvFilterOnMeasureSelect();
    HideAdvFilterOnGroupSelect();
    DisplayHeightDynamicCalculation("Measure");
    SetScroll($("#MeasureTypeHeaderContentSubLevelShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function SelecMeasureMetricName(obj) {
    isPopupVisible = true;
    if (currentpage === 'hdn-analysis-establishmentdeepdive') {
        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            if (obj.innerText.toLowerCase() === SelectedDempgraphicList[i].parentName.toLowerCase()) {
                $("#AdvFilterDivId [parentname = '" + SelectedDempgraphicList[i].parentName + "']").removeClass("Selected");
                SelectedDempgraphicList.splice(i, 1);
                i--;
            }
        }
    }
    var sCount = 0;
    if (Measurelist.length > 0 && Measurelist[0].parentName.toLowerCase() != $(obj).attr("parentname").toLowerCase()) {
        $("#retailer-measure .level3 *").removeClass("Selected");
        $("#retailer-measure .level1 *").removeClass("Selected");
        Measurelist = [];
        Measurelist.push({ uniqueid: $(obj).attr("parentid"), parentName: $(obj).attr("parentname"), filtertypeid: $(obj).attr("filtertypeid"), MetricId: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), DBName: $(obj).attr("name"), charttype: $(obj).attr("charttype"), metriclist: [] });
        _.each(Measurelist, function (i) {
            i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), Type: $(obj).attr("Type"), parentname: $(obj).attr("parentname") });
        });
        $(obj).addClass("Selected");

        var chartlist = [];

        chartlist = $(obj).attr("charttypePIT") != undefined ? $(obj).attr("charttypePIT").split("|") : [];
        ChartType = chartlist[0];

        if (ModuleBlock == "TREND")
            chartlist = $(obj).attr("charttypetrend") != undefined ? $(obj).attr("charttypetrend").split("|") : [];
        else
            chartlist = $(obj).attr("charttypePIT") != undefined ? $(obj).attr("charttypePIT").split("|") : [];

        ChartType = chartlist[0];

        $("#chart-list").html("");
        for (var i = 0; i < chartlist.length; i++) {

            var sImageClassName = ChartImagePosition(chartlist[i].toString());
            //$("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\"><div id=\"TextForChart\">" + chartlist[i] + "</div></div>").appendTo("#chart-list");
            if (chartlist[i].toString() != "Stacked Area")
                $("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\" style=\"background-image: url('../Images/sprite.svg?11');background-position:" + sImageClassName + "\"></div>").appendTo("#chart-list");
        }
        $("#LinkForCharts").show();
    }
    else if (Measurelist.length == 0) {
        Measurelist.push({ uniqueid: $(obj).attr("parentid"), parentName: $(obj).attr("parentname"), filtertypeid: $(obj).attr("filtertypeid"), MetricId: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), DBName: $(obj).attr("name"), charttype: $(obj).attr("charttype"), metriclist: [] });
        _.each(Measurelist, function (i) {
            i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), Type: $(obj).attr("Type"), parentname: $(obj).attr("parentname") });
        });
        $(obj).addClass("Selected");

        var chartlist = [];

        chartlist = $(obj).attr("charttypePIT") != undefined ? $(obj).attr("charttypePIT").split("|") : [];
        ChartType = chartlist[0];

        if (ModuleBlock == "TREND")
            chartlist = $(obj).attr("charttypetrend") != undefined ? $(obj).attr("charttypetrend").split("|") : [];
        else
            chartlist = $(obj).attr("charttypePIT") != undefined ? $(obj).attr("charttypePIT").split("|") : [];

        ChartType = chartlist[0];

        $("#chart-list").html("");
        for (var i = 0; i < chartlist.length; i++) {

            var sImageClassName = ChartImagePosition(chartlist[i].toString());
            //$("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\"><div id=\"TextForChart\">" + chartlist[i] + "</div></div>").appendTo("#chart-list");
            if (chartlist[i].toString() != "Stacked Area")
                $("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\" style=\"background-image: url('../Images/sprite.svg?11');background-position:" + sImageClassName + "\"></div>").appendTo("#chart-list");
        }
        $("#LinkForCharts").show();
    }
    else {
        _.each(Measurelist, function (i) {
            sCount = 0;
            _.each(i.metriclist, function (y, k) {

                if (y != undefined && ($(obj).attr("uniqueid") == y.Id)) {

                    i.metriclist.splice(k, 1);
                    $(obj).removeClass("Selected");
                    $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
                    sCount++;
                }
            });
            if (sCount == 0) {
                if (sTrendType == "2" && ModuleBlock == "TREND") {
                    if ($(obj).attr("name").trim() != "Top 10") {
                        i.metriclist = [];
                        $(obj).parent().find("li").removeClass("Selected");
                        $(obj).parent().find("li").find(".ArrowContainerdiv").css("background-color", "#58554D");
                        i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name").trim(), Type: $(obj).attr("Type").trim(), SelType: $(obj).attr("seltypeid"), parentname: $(obj).attr("parentname") });
                        $(obj).addClass("Selected");
                    }
                }
                else {
                    if (i.metriclist.length > 0 && i.metriclist[0].Name.trim() == "Top 10") {
                        $(obj).parent().find('li[uniqueid="' + i.metriclist[0].Id + '"]').removeClass("Selected");
                        i.metriclist = [];
                        //$(obj).parent().find("li").eq(0).removeClass("Selected");
                        $(obj).parent().find("li").eq(0).find(".ArrowContainerdiv").css("background-color", "#58554D");
                    }
                    else if ($(obj).attr("name").trim() == "Top 10") {
                        $(obj).parent().find('li[uniqueid="' + i.metriclist[0].Id + '"]').removeClass("Selected");
                        i.metriclist = [];
                        $(obj).parent().find("li").removeClass("Selected");
                        $(obj).parent().find("li").find(".ArrowContainerdiv").css("background-color", "#58554D");
                    }
                    if (i.metriclist.length < 10) {
                        i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name").trim(), Type: $(obj).attr("Type").trim(), SelType: $(obj).attr("seltypeid"), parentname: $(obj).attr("parentname") });
                        $(obj).addClass("Selected");
                    }
                    else { showMessage("YOU CAN MAKE UPTO 10 SELECTIONS") }
                }
            }
        });
    }
    //added by Nagaraju
    //Date: 27-06-2017
    $(".shoppertrip-Toggle").hide();
    if ($(obj).attr("type").toLocaleLowerCase() != "demographics") {
        if ($(obj).attr("type").toLocaleLowerCase() == "visits" || $(obj).attr("type").toLocaleLowerCase() == "trips") {
            TabType = "trips";
            if (sVisitsOrGuests == 1)
                sChange = "false";
            else
                sChange = "true";
            sVisitsOrGuests = 1;
        }
        else if ($(obj).attr("type").toLocaleLowerCase() == "shopper") {
            TabType = "shopper";
            if (sVisitsOrGuests == 2)
                sChange = "false";
            else
                sChange = "true";
            sVisitsOrGuests = 2;
        }
        HideOrShowFilters();
        if (currentpage != "hdn-analysis-establishmentdeepdive") {
            GetDefaultFrequency();
        }
    }
    else if ($(obj).attr("type").toLocaleLowerCase() == "demographics") {
        if (Grouptype == "demographics" || currentpage.indexOf("deepdive") == -1 || ModuleBlock == "TREND")
            $(".shoppertrip-Toggle").show();

        if ($("#guest-visit-toggle").is(":checked") == false) {
            TabType = "trips";
            sVisitsOrGuests = 1;
        }
        else if ($("#guest-visit-toggle").is(":checked") == true) {
            TabType = "shopper";
            sVisitsOrGuests = 2;
        }
        if (Grouptype != "demographics") {
            if (Grouptype == "visits") {
                TabType = "trips";
                sVisitsOrGuests = 1;
            }
            else if (Grouptype == "shoppers") {
                TabType = "shopper";
                sVisitsOrGuests = 2;
            }
        }
        HideOrShowFilters();
        if (currentpage != "hdn-analysis-establishmentdeepdive") {
            GetDefaultFrequency();
        }
    }

    ShowSelectedFilters();

    HideAdvFilterOnMeasureSelect();
    DisplayHeightDynamicCalculation("Measure");
}
function RemoveMeasure(obj) {
    //if (sVisitsOrGuests == 1)
    {
        var ObjData = $("#retailer-measure ul span[uniqueid='" + $(obj).attr("id") + "']");// $("#MeasureTypeContentTrip ul li[uniqueid='" + $(obj).attr("id") + "']");
    }
    //else if (sVisitsOrGuests == 2) {
    //    var ObjData = $("#MeasureTypeContentShopper ul li[uniqueid='" + $(obj).attr("id") + "']");
    //}
    if (ObjData <= 0)
        ObjData = $("#retailer-measure ul span[uniqueid='" + $(obj).attr("id") + "']");
    var ObjDataParentUniqueId = ObjData.length > 0 ? ObjData.parent().attr("uniqueid") : "";
    var ObjDataParentName = ObjData.length > 0 ? ObjData.parent().attr("parentname") : "";
    var _metriclist = [];
    //if (sVisitsOrGuests == 1)
    {
        $("#retailer-measure ul li").each(function () {
            if ($(this).attr("parentname") == ObjDataParentName)
                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname") });
        });
    }
    //else if (sVisitsOrGuests == 2) {
    //    $("#MeasureTypeContentShopper ul[uniqueid='" + ObjDataParentUniqueId + "'] li").each(function () {
    //        _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html(), Type: $(this).attr("Type"), SelType: $(this).attr("seltypeid") });
    //    });
    //}

    //var objDataParent = "";
    ////if (sVisitsOrGuests == 1) 
    //{
    //    objDataParent = $("#MeasureTypeHeaderContentTrip ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    //}
    ////else if (sVisitsOrGuests == 2) {
    ////     objDataParent = $("#MeasureTypeHeaderContentShopper ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    ////}

    //if (objDataParent.length <= 0) {
    //    //if (sVisitsOrGuests == 1)
    //    {
    //        objDataParent = $("#MeasureTypeHeaderContentSubLevelTrip ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    //    }
    //    //else if (sVisitsOrGuests == 2) {
    //    //    objDataParent = $("#MeasureTypeHeaderContentSubLevelShopper ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    //    //}

    //}
    if (ObjData.length > 0) {
        if (($("#retailer-measure").is(':visible') == true) || ($("#retailer-measure").is(':visible') == true))
            var sVisible = 1;
        else
            var sVisible = 0;
        var sStatus = "0";
        _.filter(_metriclist, function (i) { if (i.Id.trim() == ObjData.attr("uniqueid").trim()) sStatus = "1"; })
        if (Measurelist.length > 0 && (Measurelist[0].uniqueid.trim() == ObjData.attr("parentid").trim() || sStatus == "1") && Measurelist[0].parentName.trim() == ObjData.attr("parentname").trim()) {
            SelecMeasureMetricName(ObjData.parent());
        }
    }
    ShowSelectedFilters();
}

function DisplayMeasureList(obj) {
    $("#MeasureTypeHeaderContentTrip .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#MeasureTypeHeaderContentShopper .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    $("#MeasureTypeHeaderMainTrip .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    $("#MeasureTypeHeaderMainShopper .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    ChartTileName = $(obj).attr("name");
    $(obj).parent().find(".Selected").removeClass("Selected");
    $(obj).parent().find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    //if (sVisitsOrGuests == 1)
    {
        $("#MeasureTypeHeaderContentTrip").show();
        $("#MeasureTypeHeaderContentTrip ul li").css("display", "none");
    }
    //else if (sVisitsOrGuests == 2) {
    //    $("#MeasureTypeHeaderContentShopper").show();
    //    $("#MeasureTypeHeaderContentShopper ul li").css("display", "none");
    //}

    $("#MeasureTypeContentTrip").hide();
    $("#MeasureTypeContentShopper").hide();
    $("#MeasureTypeHeaderContentSubLevelTrip").hide();
    $("#MeasureTypeHeaderContentSubLevelShopper").hide();
    $("#MeasuresHeadingLevel4").hide();
    $("#MeasuresHeadingLevel3").hide();
    //if (sVisitsOrGuests == 1)
    //{
    //23-11-17
    $("#MeasureTypeHeaderContentTrip ul *").removeClass('DNI');
    //End 23-11-17
    $("#MeasureTypeHeaderContentTrip ul li[FilterTypeId='" + $(obj).attr("Id") + "'][parentname='" + $(obj).attr("name") + "']").show();
    //}
    //else if (sVisitsOrGuests == 2) {
    //    $("#MeasureTypeHeaderContentShopper ul li[FilterTypeId='" + $(obj).attr("Id") + "'][parentname='" + $(obj).attr("name") + "']").show();
    //    $("#MeasureTypeHeaderContentShopper ul li[FilterTypeId='" + $(obj).attr("Id") + "'][parentname='" + $(obj).attr("name") + "']").show();
    //}

    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel2").text($(obj).attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel2").css("width", "287px");
    //if (sVisitsOrGuests == 1)
    {
        SetScroll($("#MeasureTypeHeaderContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        SetScroll($("#MeasureTypeContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    //else if (sVisitsOrGuests == 2) {
    //    SetScroll($("#MeasureTypeHeaderContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //    SetScroll($("#MeasureTypeContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //}


    if (($("#MeasureTypeHeaderContentSubLevelTrip").is(':visible') || $("#MeasureTypeHeaderContentSubLevelShopper").is(':visible')) && ($("#MeasureTypeContentTrip").is(':visible') || $("#MeasureTypeContentShopper").is(':visible'))) {
        //$(".MeasureScrollDiv").css("width", "110%");
        //$(".MeasureType").css("width", "95%");
        $(".MeasureType").css("width", "auto");
        // $(".Lavel").css("width", "20%");
    }
    else {
        $(".MeasureType").css("width", "auto");
        // $(".Lavel").css("width", "262px");
    }
    if ($(".MeasureType").width() > window.innerWidth)
        $(".MeasureType").css("width", "95%");
    else
        $(".MeasureType").css("width", "auto");
    DisplayHeightDynamicCalculation("Measure");
    HideAdvFilterOnGroupSelect();
    SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplayMeasureTripShopperList(obj) {
    TabType = $(obj).attr("name").split(' ')[0].toLowerCase().replace("trip", "trips");
    HideOrShowFilters();

    var sChange = "";
    if ($(obj).attr("id") == "1") {
        if (sVisitsOrGuests == 1)
            sChange = "false";
        else
            sChange = "true";
        sVisitsOrGuests = 1;
        sBevarageSelctionType = [];
        $(".MeasureType .trip").show();
        $(".MeasureType .Shopper").hide();
    }
    else {
        if (sVisitsOrGuests == 2)
            sChange = "false";
        else
            sChange = "true";
        sVisitsOrGuests = 2;
        if (currentpage.indexOf("chart") > -1) {
            $("#adv-bevselectiontype-freq").hide();
            sBevarageSelctionType = [];
        }
        else
            $("#beverage-frequency ul li[uniqueid='1']").trigger("click");
        $(".MeasureType .trip").hide();
        $(".MeasureType .Shopper").show();
    }
    if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1))
        $("#adv-bevselectiontype-freq").show();
    else {
        $("#adv-bevselectiontype-freq").hide();
        sBevarageSelctionType = [];
    }
    GetDefaultFrequency();
    $("#MeasureType").trigger("click");
    $("#MeasureTypeShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj).children();
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    $(obj).parent().find(".Selected").removeClass("Selected");
    $(obj).parent().find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");
    if (sVisitsOrGuests == 1) {
        $("#MeasureTypeHeaderMainTrip").show();
        //$("#MeasureTypeHeaderMainTrip ul li").css("display", "none");
    }
    else if (sVisitsOrGuests == 2) {
        //$("#MeasureTypeHeaderMainShopper").show();
        //$("#MeasureTypeHeaderMainShopper ul li").css("display", "none");
    }
    $("#MeasureTypeContentTrip").hide();
    $("#MeasureTypeContentShopper").hide();
    $("#MeasureTypeHeaderContentSubLevelTrip").hide();
    $("#MeasureTypeHeaderContentSubLevelShopper").hide();
    $("#MeasuresHeadingLevel4").hide();
    $("#MeasuresHeadingLevel2").hide();
    $("#MeasuresHeadingLevel3").hide();
    $("#MeasureTypeContentTrip").hide();
    $("#MeasureTypeContentShopper").hide();
    $("#MeasureTypeHeaderContentTrip").hide();
    $("#MeasureTypeHeaderContentShopper").hide();

    //if (sChange == "true") {
    //    //LoadMeasureTypeMain(sFilterData);
    //    //LoadMeasureTypeHeaderName(sFilterData);
    //    //LoadMeasureTypeNames(sFilterData);
    //    LoadMeasure(sFilterData);
    //}

    if (sVisitsOrGuests == 1) {
        $("#MeasureTypeHeaderMainTrip").show();
        $("#MeasureTypeHeaderMainTrip ul li").show();
    }
    else if (sVisitsOrGuests == 2) {
        //$("#MeasureTypeHeaderMainShopper").show();
        //$("#MeasureTypeHeaderMainShopper ul li").show();
    }
    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel1").text($(obj).attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel1").show();
    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel1").css("width", "287px");

    if (sVisitsOrGuests == 1) {
        SetScroll($("#MeasureTypeHeaderMainTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    else if (sVisitsOrGuests == 2) {
        SetScroll($("#MeasureTypeHeaderMainShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }

    if (($("#MeasureTypeHeaderContentSubLevelTrip").is(':visible') || $("#MeasureTypeHeaderContentSubLevelShopper").is(':visible')) && ($("#MeasureTypeContentTrip").is(':visible') || $("#MeasureTypeContentShopper").is(':visible'))) {
        //$(".MeasureScrollDiv").css("width", "110%");
        //$(".MeasureType").css("width", "95%");
        $(".MeasureType").css("width", "auto");
        // $(".Lavel").css("width", "20%");
    }
    else {
        $(".MeasureType").css("width", "auto");
        // $(".Lavel").css("width", "262px");
    }
    if ($(".MeasureType").width() > window.innerWidth)
        $(".MeasureType").css("width", "95%");
    else
        $(".MeasureType").css("width", "auto");
    DisplayHeightDynamicCalculation("Measure");
    if (!($(".adv-fltr-toggle-container").is(":visible")))
        $(".toggle-seperator").hide();
    else
        $(".toggle-seperator").show();
    if ($("#adv-fltr-Chnl").is(":visible") || $("#adv-bevselectiontype-freq").is(":visible"))
        $(".advancedfilter-seperator").show();
    else
        $(".advancedfilter-seperator").hide();
    SetScroll($("#MeasureContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}

function LoadNonBeverageFilter(data) {
    html = "";
    var sData = data.NonBeverageList;
    var index = 0;
    if (data != null) {
        for (var i = 0; i < sData.length; i++) {
            var object = sData[i];
            if (index == 0) {
                html += "<ul>";
            }
            html += "<li Name=\"" + object.MetricItem + "\" style=\"display:table;min-height:32px\">";
            html += "<div id=\"" + object.MetrticItemId + "\" uniqueid=\"" + object.UniqueId + "\" onclick=\"SelectBevComparison(this);\" Name=\"" + object.MetricItem + "\" class=\"\" style=\"\"><span uniqueid=\"" + object.UniqueId + "\" class=\"lft-popup-ele-label\" id=\"" + object.MetrticItemId + "\" FilterTypeId=\"" + object.FilterTypeId + "\" uniqueid=\"" + object.UniqueId + "\"  Name=\"" + object.MetricItem + "\" FullName=" + object.MetricItem + " data-isselectable=\"true\">" + object.MetricItem + "</span></div>";
            AllNonBeverages.push(object.UniqueId + "|" + object.MetricItem);
            html += "</li>";
            index++;
        }
        html += "</ul>";

        $("#BGMNonBeverageDiv").html("");
        $("#BGMNonBeverageDiv").append(html);
        SetScroll($("#BGMNonBeverageDiv"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
}
function Displaybevnonbevdivs(obj) {
    $("#BGMBeverage_NonBevarageDiv .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");


    if ($(obj).attr("id") == "1") {
        $("#BeverageOrCategoryContent").show();
        $("#BGMNonBeverageDiv").hide();
        $(".Beverage").hide();
        $("#beverageHeadingLevel2").hide();
        $(".Beverages").css("width", "auto");
        SearchFilters("Beverage", "Search-Beverages", "Beverage-Search-Content", AllBeverages);
    }
    else {
        $("#BGMNonBeverageDiv").show();
        $("#BeverageOrCategoryContent").hide();
        $(".Beverage").hide();
        $("#beverageHeadingLevel2").hide();
        $(".Beverages").css("width", "auto");
        SearchFilters("NonBeverage", "Search-Beverages", "Beverage-Search-Content", AllNonBeverages);
    }
    $(".AdvancedFiltersDemoHeading #beverageHeadingLevel1").text($(obj).attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #beverageHeadingLevel1").show();
}
//Activete current page
function ActivateCurrentPage() {
    if (currentpage != undefined && currentpage != null && currentpage != "") {
        $("#LeftPanel").show();
        $("#FilterHeader").show();
    }
    if (currentpage == "hdn-crossretailer-sarreport") {
        $("#Stat-Test").parent().hide();
    }
    else {
        $("#Stat-Test").parent().show();
    }
    //Export ppt icon
    $(".ExportToPPT").hide();
    $(".ExportToPDF").hide();
    if ((currentpage.indexOf("hdn-chart") > -1 || currentpage.indexOf("hdn-report") > -1
        || currentpage.indexOf("hdn-analysis") > -1 || currentpage == "hdn-dashboard-pathtopurchase") && (currentpage != "hdn-analysis-crossretailerfrequencies" && currentpage != "hdn-analysis-crossretailerimageries" && currentpage != "hdn-analysis-acrosstrips" && !(currentpage.indexOf("hdn-report") > -1))) {
        $(".ExportToPPT").show();
    }

    //Export excel icon
    $(".ExportToExcel").hide();
    if (currentpage != "hdn-analysis-acrosstrips" && !(currentpage.indexOf("hdn-report") > -1)) {
        $(".ExportToExcel").show();
    }

    SetStatTesting(currentpage);
    if (currentpage == "hdn-dashboard-demographic") {
        $("#MenuHeader #Dashboard span").trigger("click");
        $("#dashboard-demo").css("color", "red");
        $("#Page-Lavel").html("");
        $(".ExportToExcel").hide();
        $(".ExportToPDF").show();
        $(".ExportToPPT").show();
    }
    else if (currentpage == "hdn-dashboard-brandhealth") {
        $("#MenuHeader #Dashboard span").trigger("click");
        $("#dashboard-brand").css("color", "red");
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-dashboard-visits") {
        $("#MenuHeader #Dashboard span").trigger("click");
        $("#dashboard-visit").css("color", "red");
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-dashboard-pathtopurchase") {
        $("#LeftPanel").show();
        $("#MenuHeader #Dashboard span").trigger("click");
        $("#dashboard-pathtopurchase").css("color", "red");
        $("#Page-Lavel").html("");
        $(".ExportToExcel").hide();
        $(".ExportToPDF").show();
    }
    else if (currentpage == "hdn-tbl-beveragedeepdive") {
        $("#MenuHeader #Tables span").trigger("click");
        $("#SubMenuHeader #tbl-beverage").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-beverage").addClass("Selected-Menu");
        $("#SubMenuHeader #tbl-beverage-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-tbl-comparebeverages") {
        $("#MenuHeader #Tables span").trigger("click");
        $("#SubMenuHeader #tbl-beverage").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-beverage").addClass("Selected-Menu");
        $("#SubMenuHeader #tbl-beverage-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-tbl-compareretailers") {
        $("#MenuHeader #Tables span").trigger("click");
        $("#SubMenuHeader #tbl-retailers").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-retailers").addClass("Selected-Menu");
        $("#SubMenuHeader #tbl-retailers-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-e-commerce-tbl-comparesites") {
        $("#MenuHeader #Tables span").trigger("click");
        $("#SubMenuHeader #e-commerce-tbl-sites").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #e-commerce-tbl-sites-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-tbl-retailerdeepdive") {
        $("#MenuHeader #Tables span").trigger("click");
        $("#SubMenuHeader #tbl-retailers").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-retailers").addClass("Selected-Menu");
        $("#SubMenuHeader #tbl-retailers-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
        $("#MenuHeader #Tables span").trigger("click");
        $("#SubMenuHeader #tbl-retailers").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #tbl-retailers-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-tbl-BeverageDeepDive") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #chart-beverage").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-beverage").addClass("Selected-Menu");
        $("#SubMenuHeader #chart-beverage-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-chart-comparebeverages") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #chart-beverage").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-beverage").addClass("Selected-Menu");
        $("#SubMenuHeader #chart-beverage-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    }
    else if (currentpage == "hdn-chart-compareretailers") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #chart-retailers").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-retailers").addClass("Selected-Menu");
        $("#SubMenuHeader #chart-retailers-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    }
    else if (currentpage == "hdn-e-commerce-chart-compareresites") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #chart-retailers").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-retailers").addClass("Selected-Menu");
        $("#SubMenuHeader #chart-retailers-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-chart-retailerdeepdive") {
        TabType = "trips";
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #chart-retailers").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-retailers").addClass("Selected-Menu");
        $("#SubMenuHeader #chart-retailers-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    }
    else if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #chart-retailers").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-retailers").addClass("Selected-Menu");
        $("#SubMenuHeader #chart-retailers-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-chart-beveragedeepdive") {
        $("#GroupType").show();
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #chart-beverage").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-beverage").addClass("Selected-Menu");
        $("#SubMenuHeader #chart-beverage-arrow").css("border-top", "9px solid #ea1f2a");
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    }
    else if (currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-beverage-monthly-pluspurchasers-deepdive a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-report-beveragespurchasedetailsdeepdive") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-beverages-purchase-details-deepdive a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-report-comparebeveragesmonthlypluspurchasers") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-compare-beverages-monthly-pluspurchasers a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-report-comparebeveragespurchasedetails") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-compare-beverages-purchase-details a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-report-compareretailerspathtopurchase") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-compare-retailers-pathtopurchase a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").hide();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-report-compareretailersshoppers") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-compare-retailers-shoppers a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-retailers-pathtopurchase-deepdive a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").hide();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-report-retailersshopperdeepdive") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-retailers-shopper-deepdive a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-crossRetailer-TotalRespondentsTrips a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-crossretailer-sarreport") {
        $("#MenuHeader #Reports span").trigger("click");
        $("#SubMenuHeader #reports-crossRetailer-SarReport a").css("color", "#ea1f2a");
        $(".reports-menu").hide();
        $(".ExportToExcel").hide();
    }
    else if (currentpage == "hdn-analysis-acrossshopper") {
        $("#MenuHeader #Analysis span").trigger("click");
        $(".SubMenu #others-bgm a").css("color", "#ea1f2a");
        $(".timeperiod").show();
        $("#Left-Frequency").show();

    }
    else if (currentpage == "hdn-analysis-withinshopper") {
        $("#MenuHeader #Analysis span").trigger("click");
        $(".SubMenu #others-compare-retailers a").css("color", "#ea1f2a");
        $("#Advanced-Analytics-Select-Variable").show();
        $("#Retailers").show();
    }
    else if (currentpage == "hdn-analysis-crossretailerimageries") {
        $("#MenuHeader #Analysis span").trigger("click");
        $(".SubMenu #others-cross-retailer-imageries a").css("color", "#ea1f2a");
        $("#Left-Frequency").show();
    }
    else if (currentpage == "hdn-analysis-withintrips") {
        $("#MenuHeader #Analysis span").trigger("click");
        $(".SubMenu #others-retailer-deep-dive a").css("color", "#ea1f2a");
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
        $("#Retailers").show();
        $("#GroupType").show();
        $("#Advanced-Analytics-Select-Variable").show();
    }
    else if (currentpage == "hdn-analysis-acrosstrips") {
        $("#MenuHeader #Analysis span").trigger("click");
        $(".SubMenu #others-soap a").css("color", "#ea1f2a");
    }
    else if (currentpage == "hdn-analysis-crossretailerfrequencies") {
        $("#MenuHeader #Analysis span").trigger("click");
        $(".SubMenu #others-cross-retailer-frequencies a").css("color", "#ea1f2a");
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    }
    else if (currentpage == "hdn-analysis-establishmentdeepdive") {
        $("#MenuHeader #Analysis span").trigger("click");
        $(".SubMenu #others-establishment-deepdeive a").css("color", "#ea1f2a");
    }
    if (currentpage.indexOf("hdn-report") > -1 || currentpage.indexOf("hdn-analysis") > -1) {
        $(".SubMenu").hide();
    }
    $(".Item .Active").parent("li").css("border-right", "3px solid rgb(234, 31, 42)");
}
function HideOrShowFilters() {
    if (currentpage.indexOf("hdn-tbl") > -1 || currentpage.indexOf("hdn-chart") > -1 || currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
        if (currentpage.indexOf("chart") > -1) {
            if (TabType == "trips") {
                //$("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                //$("#frequency_containerId ul li[name='TOTAL VISITS']").show();
                if (currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive")
                    addfilter("frequency_containerId", getFilter("Cross-Retailer Shopper (Trips)"), "SelectFrequency(this);");
                else
                    addfilter("frequency_containerId", getFilter("Trips Frequency"), "SelectFrequency(this);");
                addRightPanelfilter("channel-content", getFilter("Beverage Where Purchased"), "SelectChannel(this);", "rgt-cntrl-chnl");
                addfilter("frequency_containerId", getFilter("Shopper Frequency"), "SelectFrequency(this);");


                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
                selectedChannels = [];
                $("#RightPanelPartial #channel-content ul li[name='TOTAL']").trigger("click");
                $(".shopperDiv").hide();
                $(".tripsDiv").show();
                if (currentpage.indexOf("beverage") > -1)
                    $(".shopperDiv").hide();
                $(".freq-seperator").show();
                $(".adv-fltr-details").show();
            }

            else if (TabType == "shopper" || $("#guest-visit-toggle").is(":checked") == true || (sVisitsOrGuests == "2")) {

                //$("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").show();
                //$("#frequency_containerId ul li[name='TOTAL VISITS']").hide();
                if (currentpage.indexOf("beverage") > 0) {
                    addfilter("frequency_containerId", getFilter("Beverage Shopper Frequency"), "SelectFrequency(this);");
                    SelectedFrequencyList = [];
                    $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
                }
                else {
                    SelectedFrequencyList = [];
                    if (currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive") {
                        addfilter("frequency_containerId", getFilter("Cross-Retailer Shopper (Shoppers)"), "SelectFrequency(this);");
                        $("#RightPanelPartial #frequency_containerId ul li[parentname='MONTHLY +' ][name='SELECTION']").trigger("click");
                    }
                    else {
                        addfilter("frequency_containerId", getFilter("Shopper Frequency"), "SelectFrequency(this);");
                        $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
                    }
                }
                $(".tripsDiv").hide();
                $(".freq-seperator").hide();
                $(".shopperDiv").show();
                if (currentpage.indexOf("retailer") > -1) {
                    //$("#adv-bevselectiontype-freq").show();
                    $("#adv-bevselectiontype-freq").hide();
                    sBevarageSelctionType = [];
                }
                else {
                    $("#adv-bevselectiontype-freq").hide();
                    sBevarageSelctionType = [];
                }
                $(".adv-fltr-details").show();
            }
            else {
                //trips filtesr
                $(".adv-fltr-suboptions-list-container").show();
                //beverage frequency            
                //shopper frequency
                //$("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                //$("#frequency_containerId ul li[name='TOTAL VISITS']").show();
                if (currentpage.indexOf("beverage") > 0) {
                    addfilter("frequency_containerId", getFilter("Beverage Shopper Frequency"), "SelectFrequency(this);");
                    $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
                }
                else {
                    if (currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive")
                        addfilter("frequency_containerId", getFilter("Cross-Retailer Shopper (Trips)"), "SelectFrequency(this);");
                    else
                        addfilter("frequency_containerId", getFilter("Trips Frequency"), "SelectFrequency(this);");
                    $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
                }

                $(".tripsDiv").hide();
                $(".freq-seperator").hide();
                $(".shopperDiv").hide();
                $(".adv-fltr-details").hide();
            }
        }

        else {
            if (TabType == "trips") {
                //$("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                //$("#frequency_containerId ul li[name='TOTAL VISITS']").show();
                //addfilter("frequency_containerId", getFilter("Trips Frequency"), "SelectFrequency(this);");
                if (currentpage == "hdn-tbl-compareretailers" || currentpage == "hdn-tbl-retailerdeepdive") {
                    addfilter("frequency_containerId", getFilter("Cross-Retailer Shopper (Trips)"), "SelectFrequency(this);");

                }
                else
                    addfilter("frequency_containerId", getFilter("Trips Frequency"), "SelectFrequency(this);");

                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").removeClass("Selected");
                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");

                $(".shopperDiv").hide();
                $(".tripsDiv").show();
                if (currentpage.indexOf("beverage") > -1)
                    $(".shopperDiv").hide();

                $(".freq-seperator").show();
                $(".adv-fltr-details").show();
            }
            else if (TabType == "shopper") {
                //$("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").show();
                //$("#frequency_containerId ul li[name='TOTAL VISITS']").hide();
                if (currentpage.indexOf("beverage") > 0) {
                    addfilter("frequency_containerId", getFilter("Beverage Shopper Frequency"), "SelectFrequency(this);");
                    $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
                }
                else {
                    if (currentpage == "hdn-tbl-compareretailers" || currentpage == "hdn-tbl-retailerdeepdive") {
                        addfilter("frequency_containerId", getFilter("Cross-Retailer Shopper (Shoppers)"), "SelectFrequency(this);");
                        $("#RightPanelPartial #frequency_containerId ul li[parentname='MONTHLY +' ][name='SELECTION']").trigger("click");
                    }
                    else {
                        addfilter("frequency_containerId", getFilter("Shopper Frequency"), "SelectFrequency(this);");
                        $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
                    }

                }
                $(".tripsDiv").hide();
                $(".freq-seperator").hide();
                $(".shopperDiv").show();
                if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1))
                    $("#adv-bevselectiontype-freq").show();
                else {
                    $("#adv-bevselectiontype-freq").hide();
                    sBevarageSelctionType = [];
                }
                $(".adv-fltr-details").show();
            }
            else {
                //trips filtesr
                $(".adv-fltr-suboptions-list-container").show();
                //beverage frequency            
                //shopper frequency
                //$("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                //$("#frequency_containerId ul li[name='TOTAL VISITS']").show();
                if (currentpage.indexOf("beverage") > 0) {
                    addfilter("frequency_containerId", getFilter("Beverage Shopper Frequency"), "SelectFrequency(this);");
                    $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
                }
                else
                    addfilter("frequency_containerId", getFilter("Shopper Frequency"), "SelectFrequency(this);");

                $(".tripsDiv").hide();
                $(".freq-seperator").hide();
                $(".shopperDiv").hide();
                $(".adv-fltr-details").hide();
            }
        }
    }
    AllFrequency = [];
    AllSubFrequency = [];
    $("#frequency_containerId ul li").each(function () {
        if ($(this).css('display') != "none") {
            if ($(this).find(".sidearrw").length <= 0)
                AllFrequency.push($(this).children("div").children("span").attr("uniqueid") + "|" + $(this).children("div").children("span").html());
        }
    });
    $("#frequency_containerId-SubLevel1 ul li").each(function () {
        if ($(this).css('display') != "none") {
            if ($(this).find(".sidearrw").length <= 0 && $.inArray($(this).children("div").children("span").attr("uniqueid") + "|" + $(this).children("div").children("span").html(), AllSubFrequency) == -1
                && $.inArray($(this).children("div").children("span").attr("uniqueid") + "|" + $(this).children("div").children("span").html(), AllFrequency) == -1)
                AllSubFrequency.push($(this).children("div").children("span").attr("uniqueid") + "|" + $(this).children("div").children("span").html());
        }
    });
    HideTotalVisits();
    if (sVisitsOrGuests == "2" || TabType == "shopper")
        SearchFilters_RightPanel("Frequency", "Search-FrequencyFilters", "FreqFilter-Search-Content", AllFrequency.concat(AllSubFrequency));
    else
        SearchFilters_RightPanel("Frequency", "Search-FrequencyFilters", "FreqFilter-Search-Content", AllFrequency);
    ShowSelectedFilters();
}
function SetDefaultValues() {
    if (currentpage.indexOf("beverage") > -1) {
        $("#Left-Frequency").hide();
        SelectedFrequencyList = [];
    }
    if (currentpage == "hdn-chart-comparebeverages" || currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive" || currentpage == "hdn-chart-beveragedeepdive" || currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    }
    $("#NoteSample").show();
    if (currentpage == "hdn-tbl-comparebeverages") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#Beverages").show();
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#RightPanelPartial #channel-content ul li[name='TOTAL']").trigger("click");
    }
    else if (currentpage == "hdn-dashboard-pathtopurchase") {
        TabType = "trips";
        $(".dashboard-save").show();
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#dashboard-pathtopurchase-size-skew").show();
        $("#stattest_benchmark").attr("filter-name", "Retailers");
        $("#stattest_benchmark").attr("search-params", "Custombase-Retailers|Search-Custombase-Retailers|Custombase-Retailer-Search-Content");
        $("#AdvancedFilters").hide();
        $("#Left-Frequency").hide();
        $("#Retailers").show();
        $("#GroupType").show();
        $(".GroupType #grouptypeHeadingLevel1").html("Advance Filters");
        $("#Retailers").attr("class", "FilterMenu DashboardRetailers classMouseHover");
        $("#GroupType").attr("class", "FilterMenu DashboardDemographics classMouseHover");
        //$(".Left-Frequency ul li[name='TOTAL VISITS']").hide();
        //$(".Left-Frequency ul li[name='TOTAL VISITS']").removeClass("Selected");
        //$(".Left-Frequency ul li[name='TOTAL VISITS']").trigger("click");
        $(".GroupType ul li[name='TOTAL VISITS']").trigger("click");
        //$("#Left-Frequency").show();
        $("#NoteSample").hide();
        $("#P2P-popup-note").show();
        $("#P2PSampleSize").show();

        custombase_defaulttime();

        var _retailer = $(".Retailers span[uniqueid='" + $("#default-retailer").val() + "']");
        if (_retailer.length == 0)
            _retailer = $(".Retailers li[uniqueid='" + $("#default-retailer").val() + "']");

        SelectComparison(_retailer);

        var _custom_base = $(".Custombase-Retailers span[uniqueid='" + $("#default-custom-base").val() + "']");
        if (_custom_base.length == 0)
            _custom_base = $(".Custombase-Retailers li[uniqueid='" + $("#default-custom-base").val() + "']");
        SelectPathToPurchaseCustomBase(_custom_base);
        CustomBaseFlag = 0;
        var _sigtype = $("#default-sigtype").val();
        $("#div_stattest span[sigtype-id='" + _sigtype + "']").parent(".stattest").trigger("click");
        $(".Custombase-Retailers").hide();

        //trigger filters
        if ($("#default-shopper-segment_uniqueid").val() != '') {
            var _filters_uniqueId = $("#default-shopper-segment_uniqueid").val().split('|');
            var _filters = $("#default-shopper-segment").val().split('|');
            for (var i = 0; i < _filters_uniqueId.length; i++) {
                $(".GroupType span[name='" + _filters[i] + "'][uniqueid='" + _filters_uniqueId[i] + "']").parent("li").trigger("click");
            }
        }

        //trigger custom filters
        custombase_AddFilters = [];
        custombase_Frequency = [];
        $("#custombase-groupDivId ul li[name='GEOGRAPHY']").find(".ArrowContainerdiv").trigger("click");
        $("#custombase-groupDivId ul li").removeClass("Selected");
        if ($("#default-custom-base-dual-filters_UniqueId").val() != '') {
            var _filters_uniqueId = $("#default-custom-base-dual-filters_UniqueId").val().split('|');
            var _filters = $("#default-custom-base-dual-filters").val().split('|');
            for (var i = 0; i < _filters_uniqueId.length; i++) {
                $(".Custombase-GroupType span[name='" + _filters[i] + "'][uniqueid='" + _filters_uniqueId[i] + "']").parent("li").trigger("click");
            }
        }

        //custom base default frequency       
        if ($("#default-custom-base-dual-ShopperFrequency_UniqueId").val() != '') {
            var _filters_uniqueId = $("#default-custom-base-dual-ShopperFrequency_UniqueId").val().split('|');
            var _filters = $("#default-custom-base-dual-shopperfrequency").val().split('|');
            for (var i = 0; i < _filters_uniqueId.length; i++) {
                $(".Custombase-GroupType span[name='" + _filters[i] + "'][uniqueid='" + _filters_uniqueId[i] + "']").parent("li").trigger("click");
            }
        }
        //trigger frequency
        LoadDashboardGroupTypeHeaderName(sFilterData);
        var _frequency = $("#default-shopper-frequency").val()
        $(".GroupType li[primefiltertype='Frequency'][uniqueid='" + _frequency + "']").trigger("click");
        P2P_Sort = $("#default-sort").val();
        CustomBaseFlag = 0;
        if ($("#default-sort").val() == 0) {
            DemogFlag = 0;
            $("#pathtopurchase-size-skew-toggle").trigger("click");
        }
        else {
            prepareContentArea();
        }

    }
    else if (currentpage == "hdn-dashboard-demographic") {
        TabType = "trips";
        $(".dashboard-save").show();
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#dashboard-pathtopurchase-size-skew").show();
        $("#dashboard-pathtopurchase-trips-shopper").show();
        $("#stattest_benchmark").attr("filter-name", "Retailers");
        $("#AdvancedFilters").attr("filter-name", "Shopper Groups");
        $("#Retailers").attr("class", "FilterMenu DashboardRetailers classMouseHover");
        $("#stattest_benchmark").attr("search-params", "Custombase-Retailers|Search-Custombase-Retailers|Custombase-Retailer-Search-Content");

        $(".AdvancedFilters #DemoHeadingLevel1").html("Advance Filters");
        $("#AdvancedFilters").attr("class", "FilterMenu DashboardDemographics classMouseHover");

        //$("#AdvancedFilters").show();
        ////$("#AdvFilterDivId li[filtertype=FREQUENCY]").show();
        //$("#Left-Frequency").hide();

        $("#AdvancedFilters").hide();
        $("#Left-Frequency").hide();
        $("#Retailers").show();

        $("#Left-Frequency").hide();
        $("#Retailers").show();
        $("#GroupType").show();

        $(".GroupType #grouptypeHeadingLevel1").html("Advance Filters");
        $("#GroupType").attr("class", "FilterMenu DashboardDemographics classMouseHover");

        //$(".Left-Frequency ul li[name='TOTAL VISITS']").hide();
        //$(".Left-Frequency ul li[name='TOTAL VISITS']").removeClass("Selected");
        //$(".Left-Frequency ul li[name='TOTAL VISITS']").trigger("click");
        $(".GroupType ul li[name='TOTAL VISITS']").trigger("click");
        //$("#Left-Frequency").show();
        $("#NoteSample").hide();
        $("#P2P-popup-note").show();
        $("#P2PSampleSize").show();

        custombase_defaulttime();

        var _retailer = $(".Retailers span[uniqueid='" + $("#default-retailer").val() + "']");
        if (_retailer.length == 0)
            _retailer = $(".Retailers li[uniqueid='" + $("#default-retailer").val() + "']");

        SelectComparison(_retailer);

        var _custom_base = $(".Custombase-Retailers span[uniqueid='" + $("#default-custom-base").val() + "']");
        if (_custom_base.length == 0)
            _custom_base = $(".Custombase-Retailers li[uniqueid='" + $("#default-custom-base").val() + "']");
        SelectPathToPurchaseCustomBase(_custom_base);
        CustomBaseFlag = 0;
        var _sigtype = $("#default-sigtype").val();
        $("#div_stattest span[sigtype-id='" + _sigtype + "']").parent(".stattest").trigger("click");
        $(".Custombase-Retailers").hide();

        //LoadVisitsFiltersLeftPanel(sFilterData);
        //LoadSecondaryVisitsFiltersLeftPanel(sFilterData);
        //AddFrequencyinAdvFilters();
        //SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllAdvancedFilterLeft));
        //trigger filters
        if ($("#default-shopper-segment").val() != '') {
            var _filters = $("#default-shopper-segment").val().split('|');
            for (var i = 0; i < _filters.length; i++) {
                $("#groupDivId span[uniqueid='" + _filters[i] + "']").parent("div").trigger("click");
            }
        }

        //trigger frequency
        LoadDashboardGroupTypeHeaderName(sFilterData);

        //trigger custom filters
        custombase_AddFilters = [];
        custombase_Frequency = [];
        $("#custombase-groupDivId ul li[name='GEOGRAPHY']").find(".ArrowContainerdiv").trigger("click");
        $("#custombase-groupDivId ul li").removeClass("Selected");

        var _frequency = $("#default-shopper-frequency").val();
        //$("#AdvFilterDivId .Frequency span[uniqueid='" + _frequency + "']").parent("div").trigger("click");
        $("#groupDivId span[parentname=FREQUENCY][uniqueid='" + _frequency + "']").parent("li").trigger("click");

        P2P_Sort = $("#default-sort").val();

        if ($("#default-sort").val() == 0) {
            DemogFlag = 1;
            $("#pathtopurchase-size-skew-toggle").trigger("click");
        }
        if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name != "Total Visits") {
            if ($("#default-tabtype").val() == "2") {
                SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllSubFrequencyDemo.splice(4)));
                DemogFlag = 1;
                $("#pathtopurchase-size-skew-toggleTrip").trigger("click");
            }
            else {
                SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics.concat(AllAdvancedFilterLeft));
                //LoadAdvancedFilterFromString(sFilterData);
                if (TabType == "Shopper") {
                    DemogFlag = 1;
                }
                else {
                    DemogFlag = 0;
                }

                if (DemogFlag == 0) {
                    SelectedFrequencyList = [];
                    SetTripsDefaultFrequency();
                }
                else {
                    LoadDashboardGroupTypeHeaderName(sFilterData);
                    var _frequency = $("#default-shopper-frequency").val();
                    //$("#AdvFilterDivId .Frequency span[uniqueid='" + _frequency + "']").parent("div").trigger("click");
                    $("#groupDivId span[parentname=FREQUENCY][uniqueid='" + _frequency + "']").parent("li").trigger("click");
                }
                ShowSelectedFilters();
                prepareContentArea();
                DemogFlag = 0;
            }
        }
        else {
            prepareContentArea();
        }
    }
    else if (currentpage == "hdn-tbl-compareretailers" || currentpage == "hdn-e-commerce-tbl-comparesites") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
    }
    else if (currentpage == "hdn-tbl-retailerdeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
        $("#PIT-TREND").show();
        ModuleBlock = "PIT";
        UpdateDeepDive();
    }
    else if (currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
        $("#MenuHeader #Tables span").trigger("click");
        $("#SubMenuHeader #e-commerce-tbl-sites").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #e-commerce-tbl-sites-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-tbl-beveragedeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");      
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").hide();
        $("#Beverages").show();
        $("#GroupType").attr("filter-name", "Beverage Groups");
        ModuleBlock = "PIT";
        UpdateDeepDive();
        $("#RightPanelPartial #channel-content ul li[name='TOTAL']").trigger("click");
    }
    else if (currentpage == "hdn-chart-beveragedeepdive") {
        ModuleBlock = "PIT";
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#MeasureType").attr("filter-name", "Beverage Measure");
        $("#GroupType").attr("filter-name", "Beverage Groups");
        //$("#RightPanelPartial #channel-content ul li[name='TOTAL']").trigger("click");
    }
    else if (currentpage == "hdn-chart-comparebeverages") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#Left-Channel-Visited").hide();
        $("#Beverages").show();
        $("#MeasureType").attr("filter-name", "Beverage Measure");
        //$("#RightPanelPartial #channel-content ul li[name='TOTAL']").trigger("click");
    }
    else if (currentpage == "hdn-chart-compareretailers") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
    }
    else if (currentpage == "hdn-e-commerce-chart-comparesites") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #e-commerce-chart-sites").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #e-commerce-chart-sites-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
    }
    else if (currentpage == "hdn-chart-retailerdeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        TabType = "trips";
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#RightPanelPartial #frequency_containerId ul li[name='Monthly +' i]").trigger("click");
        $("#PIT-TREND").show();
        ModuleBlock = "PIT";
        UpdateDeepDive();

    }
    else if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #e-commerce-chart-sites").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #e-commerce-chart-sites-arrow").css("border-top", "9px solid #ea1f2a");
        $("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
        $("#PIT-TREND").show();
        ModuleBlock = "PIT";
        UpdateDeepDive();
    }
    else if (currentpage == "hdn-analysis-withinshopper") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
        $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
        $("#frequency_containerId ul li[name='MAIN STORE/FAVORITE STORE']").show();
        $("#frequency_containerId ul li[name='TOTAL VISITS']").hide();

        $("#adv-fltr-freq").show();
        $(".adv-fltr-details").show();
    }
    else if (currentpage == "hdn-analysis-withintrips") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#GroupType").attr("filter-name", "Shopper Groups");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
        $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");

    }
    else if (currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#Beverages").attr("filter-name", "Beverages");
        $("#GroupType").attr("filter-name", "Reports Beverage Groups");
        $("#AdvancedFilters").attr("filter-name", "Demographic");

        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#frequency_containerId ul li[name='MONTHLY +']").trigger("click");
        $("#PIT-TREND").show();
        ModuleBlock = "PIT";
        UpdateDeepDive();
    }
    else if (currentpage == "hdn-report-beveragespurchasedetailsdeepdive") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);;
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#Beverages").attr("filter-name", "Beverages");
        $("#GroupType").attr("filter-name", "Beverage Groups");
        $("#Left-Channel-Visited").attr("filter-name", "Beverage Where Purchased");
        $("#AdvancedFilters").attr("filter-name", "Reports PathToPurchase Filters");

        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $(".Left-Channel-Visited ul li[name='TOTAL']").removeClass("Selected");

        $(".Left-Channel-Visited ul li[name='TOTAL']").trigger("click");
        $("#PIT-TREND").show();
        $("#Left-Channel-Visited").show();
        ModuleBlock = "PIT";
        UpdateDeepDive();
    }
    else if (currentpage == "hdn-report-comparebeveragesmonthlypluspurchasers") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);;
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#Beverages").attr("filter-name", "Beverages");
        $("#AdvancedFilters").attr("filter-name", "Demographic");

        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#frequency_containerId ul li[name='MONTHLY +']").trigger("click");;
    }
    else if (currentpage == "hdn-report-comparebeveragespurchasedetails") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);;
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#Beverages").attr("filter-name", "Beverages");
        $("#AdvancedFilters").attr("filter-name", "Reports PathToPurchase Filters");
        $("#Left-Channel-Visited").attr("filter-name", "Beverage Where Purchased");

        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $(".Left-Channel-Visited ul li[name='TOTAL']").removeClass("Selected");
        $(".Left-Channel-Visited ul li[name='TOTAL']").trigger("click");
        $("#Left-Channel-Visited").show();
    }
    else if (currentpage == "hdn-report-compareretailerspathtopurchase") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click"); 
        $("#AdvancedFilters").attr("filter-name", "Reports PathToPurchase Filters");

        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#AdvFilterDivId ul li[name='TOTAL VISITS']").trigger("click");
        $("#Left-Frequency").hide();
    }
    else if (currentpage == "hdn-report-compareretailersshoppers") {
        TabType = "shopper";
        $("#Retailers").attr("filter-name", "Priority Retailers");
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);
        $("#Left-Frequency").attr("filter-name", "Reports Retailer Frequency");
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");  
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $(".Left-Frequency ul li[parentname='MONTHLY +'][name='SELECTION']").removeClass("Selected");
        $(".Left-Frequency ul li[parentname='MONTHLY +'][name='SELECTION']").trigger("click");
        $("#Left-Frequency").show();
    }
    else if (currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);;
        // $("#TimeBlock ul li[name='TOTAL TIME']").trigger("click"); 
        $("#GroupType").attr("filter-name", "Shopper Groups");
        $("#AdvancedFilters").attr("filter-name", "Reports PathToPurchase Filters");

        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").show();
        $("#Left-Frequency").hide();
        ModuleBlock = "PIT";
        UpdateDeepDive();
        //$("#AdvFilterDivId ul li[name='TOTAL VISITS']").trigger("click");
        //$(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
        //$(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
    }
    else if (currentpage == "hdn-report-retailersshopperdeepdive") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);;
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");    
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#Retailers").attr("filter-name", "Priority Retailers");
        $("#PIT-TREND").show();
        $("#Left-Frequency").show();
        $("#GroupType").attr("filter-name", "Reports Retailer Groups");
        $("#Left-Frequency").attr("filter-name", "Reports Retailer Frequency");

        ModuleBlock = "PIT";
        UpdateDeepDive();
        TabType = "shopper";
        //$(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
        //$(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
    }
    else if (currentpage == "hdn-crossretailer-sarreport") {
        SetScroll($(".ShowChartArea2"), "#393939", 0, 0, 0, 0, 8);
        $("#AdvancedFilters").attr("filter-name", "Reports PathToPurchase Filters");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#Retailers").attr("filter-name", "Priority Retailers");
        $("#GroupType").attr("filter-name", "Reports Retailer Groups");
    }
    else if (currentpage == "hdn-analysis-crossretailerimageries") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        TabType = "shopper";
        $("#Left-Frequency").attr("filter-name", "Correspondance Retailer Frequency");
        $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
        $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
    }
    else if (currentpage == "hdn-analysis-acrosstrips") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");        
        $("#Retailers").show();
        $("#GroupType").show();
        $("#Left-Frequency").show();
        $("#GeographyFilters").show();
        $("#GroupType").attr("filter-name", "Reports Beverage Groups");
        $("#Left-Frequency").attr("filter-name", "Reports Retailer Frequency");

        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#soap-geography-data div[level-id='1'] ul li[uniqueid='9000']").trigger("click");
        TabType = "shopper";
        $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
        $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
        $("#PrimeGroupTypeHeaderContent ul li[primefiltertype='Beverage Purchaser']").remove();
    }
    else if (currentpage == "hdn-analysis-acrossshopper") {
        $("#Retailers").show();
        $("#Beverages").show();

        $("#Beverages").attr("filter-name", "BGM Beverag And NonBeverage Items");

        LoadTimePeriod(filters);
        $("#TimeBlock ul li[name='TOTAL TIME']").hide();
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        TabType = "shopper";
        $("#left-panel-frequency ul li[name='Monthly +']").trigger("click");

        SelectBevComparison($("#BevDivId ul li[name='Total Non-Alcoholic RTD Beverages']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='Total SSD']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='Packaged Water']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='Sports Drink']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='Energy Drink/Shot']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='100% Juice']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='SSD Regular']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='SSD Diet']").eq(0));
        SelectBevComparison($("#BevDivId ul li[name='RTD Tea']").eq(0));
    }
    else if (currentpage == "hdn-analysis-crossretailerfrequencies") { //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#Retailers").hide();
        $("#AdvancedFilters").show();
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel1").text("Retailers");
        $("#GroupType").show();
        $("#TotalMeasure").show();
        $(".LowerRightContent").show();
        $("#GroupType").attr("filter-name", "Shopper Groups");
        $("#TotalMeasure").attr("filter-name", "Total Measure");
    }
    else if (currentpage == "hdn-analysis-establishmentdeepdive") {
        $("#Retailers").show();
        $("#MeasureType").show();
        $("#TimeBlock ul li[name='TOTAL TIME']").hide();
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $(".ExportToExcel").hide();
        $(".AdvancedFilters #DemoHeadingLevel1").html("Advanced Filters");
    }
    if (currentpage == "hdn-chart-comparebeverages" || currentpage == "hdn-chart-beveragedeepdive") {
        $("#RightPanelPartial #frequency_containerId ul li[name='']").trigger("click");
        SelectedFrequencyList = [];
        ShowSelectedFilters();
    }
    HideOrShowFilters();
    FilterSelectionLimitText();
    $("#adv-bevselectiontype-freq").hide();
}
//Show Selected Filters
function ShowSelectedFilters() {
    var ele = "";
    $("#SelectedFilters #scrollableselection").css("width", "97%");
    //$("#SelectedFilters").getNiceScroll().remove();
    html = "<div>";
    var htmlText = "";

    //For Time Period
    if (currentpage.toLowerCase() == "hdn-analysis-acrossshopper") {//if (currentpage.toLowerCase() == "hdn-chart-beveragedeepdive") {
        html += "<div>" + sCurrent_PreviousTime + "</div>";
        htmlText += sCurrent_PreviousTime;
    }
    else {
        if ($(".timeType").val() != "") {
            html += "<div>" + $(".timeType").val() + "</div>";
            htmlText += $(".timeType").val();
        }
    }

    //For Channel Retailers
    if (Comparisonlist.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < Comparisonlist.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + Comparisonlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveComparison(this);\" Id=\"" + Comparisonlist[i].Id + "\" name=\"" + Comparisonlist[i].Name + "\" dbname=\"" + Comparisonlist[i].DBName + "\" UniqueId=\"" + Comparisonlist[i].UniqueId + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + Comparisonlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveComparison(this);\" Id=\"" + Comparisonlist[i].Id + "\" name=\"" + Comparisonlist[i].Name + "\" dbname=\"" + Comparisonlist[i].DBName + "\" UniqueId=\"" + Comparisonlist[i].UniqueId + "\"></a></div>";
            htmlText += Comparisonlist[i].Name;
        }
    }
    //For Groups
    if (Grouplist.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < Grouplist.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + Grouplist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveGroup(this);\" UniqueId=\"" + Grouplist[i].UniqueId + "\" Id=\"" + Grouplist[i].Id + "\" name=\"" + Grouplist[i].Name + "\" Fullname=\"" + Grouplist[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + Grouplist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveGroup(this);\" UniqueId=\"" + Grouplist[i].UniqueId + "\" name=\"" + Grouplist[i].Name + "\" Fullname=\"" + Grouplist[i].FullName + "\"></a></div>";
            htmlText += Grouplist[i].Name;
        }
    }
    //For Channel Retailers
    if (ComparisonBevlist.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < ComparisonBevlist.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + ComparisonBevlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveBevComparison(this);\" Id=\"" + ComparisonBevlist[i].Id + "\" name=\"" + ComparisonBevlist[i].Name + "\" dbname=\"" + ComparisonBevlist[i].DBName + "\" UniqueId=\"" + ComparisonBevlist[i].UniqueId + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + ComparisonBevlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveBevComparison(this);\" Id=\"" + ComparisonBevlist[i].Id + "\" name=\"" + ComparisonBevlist[i].Name + "\" dbname=\"" + ComparisonBevlist[i].DBName + "\" UniqueId=\"" + ComparisonBevlist[i].UniqueId + "\"></a></div>";
            htmlText += ComparisonBevlist[i].Name;
        }

    }
    if (Geographylist.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < Geographylist.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + Geographylist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveGeographyData(this);\" Id=\"" + Geographylist[i].Id + "\" Uniqueid=\"" + Geographylist[i].UniqueId + "\" name=\"" + Geographylist[i].Name + "\" dbname=\"" + Geographylist[i].DBName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + Geographylist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveGeographyData(this);\" Id=\"" + Geographylist[i].Id + "\" Uniqueid=\"" + Geographylist[i].UniqueId + "\" name=\"" + Geographylist[i].Name + "\" dbname=\"" + Geographylist[i].DBName + "\"></a></div>";
            htmlText += Geographylist[i].Name;
        }
    }

    //For Frequency
    if (SelectedFrequencyList.length > 0) {
        if (SelectedFrequencyList[0].Name != undefined) {
            if (SelectedFrequencyList[0].Name.toLocaleLowerCase() != "total visits") {
                var sText = "";
                if (currentpage.indexOf("beverage") > 0)
                    sText = "MONTHLY PURCHASE";
                else
                    sText = "FREQUENCY";

                if (html != "<div>")
                    html += "<div class=\"seperater\">|</div>";

                for (var i = 0; i < SelectedFrequencyList.length; i++) {
                    if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-report-compareretailerspathtopurchase" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
                        html += "<div class=\"selected-filter\">" + SelectedFrequencyList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveFrequency(this);\" UniqueId=\"" + SelectedFrequencyList[i].UniqueId + "\" Id=\"" + SelectedFrequencyList[i].Id + "\" name=\"" + SelectedFrequencyList[i].Name + "\" Fullname=\"" + SelectedFrequencyList[i].FullName + "\"></a></div>";
                    }
                    else if (i > 0) {
                        html += "<div class=\"selected-filter\">, " + SelectedFrequencyList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveFrequency(this);\" UniqueId=\"" + SelectedFrequencyList[i].UniqueId + "\" Id=\"" + SelectedFrequencyList[i].Id + "\" name=\"" + SelectedFrequencyList[i].Name + "\" Fullname=\"" + SelectedFrequencyList[i].FullName + "\"></a></div>";
                    }
                    else if ((currentpage.indexOf("tbl") > -1 || currentpage.indexOf("chart") > -1 || currentpage == "hdn-dashboard-demographic") && TabType == "trips") {
                        html += "<div class=\"selected-filter\">" + SelectedFrequencyList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveFrequency(this);\" UniqueId=\"" + SelectedFrequencyList[i].UniqueId + "\" Id=\"" + SelectedFrequencyList[i].Id + "\" name=\"" + SelectedFrequencyList[i].Name + "\" Fullname=\"" + SelectedFrequencyList[i].FullName + "\"></a></div>";
                    }
                    else
                        html += "<div class=\"selected-filter\">" + SelectedFrequencyList[i].Name + "</div>";

                    htmlText += SelectedFrequencyList[i].Name;
                }
            }
        }
    }

    //For Advanced Filtes
    if (SelectedAdvFilterList.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < SelectedAdvFilterList.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + SelectedAdvFilterList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveAdvfilters(this);\" UniqueId=\"" + SelectedAdvFilterList[i].UniqueId + "\" Id=\"" + SelectedAdvFilterList[i].Id + "\" name=\"" + SelectedAdvFilterList[i].Name + "\" Fullname=\"" + SelectedAdvFilterList[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + SelectedAdvFilterList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveAdvfilters(this);\" UniqueId=\"" + SelectedAdvFilterList[i].UniqueId + "\" Id=\"" + SelectedAdvFilterList[i].Id + "\" name=\"" + SelectedAdvFilterList[i].Name + "\" Fullname=\"" + SelectedAdvFilterList[i].FullName + "\"></a></div>";
            htmlText += SelectedAdvFilterList[i].Name;
        }
    }

    //For Competitor Retailers
    if (CompetitorRetailer.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < CompetitorRetailer.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + " Cross Retailer Shopper: " + CompetitorRetailer[i].Name + ", " + CompetitorFrequency[i].Name + " <a class=\"remove-item\" onclick=\"RemoveCompetitor(this);\" Id=\"" + CompetitorRetailer[i].Id + "\" name=\"" + CompetitorRetailer[i].Name + "\" dbname=\"" + CompetitorRetailer[i].DBName + "\" UniqueId=\"" + CompetitorRetailer[i].UniqueId + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + " Cross Retailer Shopper: " + CompetitorRetailer[i].Name + ", " + CompetitorFrequency[i].Name + " <a class=\"remove-item\" onclick=\"RemoveCompetitor(this);\" Id=\"" + CompetitorRetailer[i].Id + "\" name=\"" + CompetitorRetailer[i].Name + "\" dbname=\"" + CompetitorRetailer[i].DBName + "\" UniqueId=\"" + CompetitorRetailer[i].UniqueId + "\"></a></div>";
            htmlText += " Cross Retailer Shopper: " + CompetitorRetailer[i].Name + ", " + CompetitorFrequency[i].Name;
        }
    }

    //For Custom Base
    if ((currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic")) {
        if (CustomBase.length > 0) {
            if (html != "<div>")
                html += "<div class=\"seperater\">|</div>";

            html += "<div class=\"selected-filter\">Custom Base: " + CustomBase[0].Name + "</div>";
        }
    }

    //For Custombase add filters
    if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
        $("#custom-base-add-filters").html("<ul></ul>");
        if (custombase_AddFilters.length > 0 && Sigtype_Id == "1") {
            if (html != "<div>")
                html += "<div class=\"seperater\">|</div>";

            for (var i = 0; i < custombase_AddFilters.length; i++) {
                if (i > 0)
                    html += "<div class=\"selected-filter\">, " + custombase_AddFilters[i].Name + " <a class=\"remove-item\" onclick=\"RemoveCustomBaseGroup(this);\" UniqueId=\"" + custombase_AddFilters[i].UniqueId + "\" Id=\"" + custombase_AddFilters[i].Id + "\" name=\"" + custombase_AddFilters[i].Name + "\" levelId=\"" + custombase_AddFilters[i].levelId + "\" parentName=\"" + custombase_AddFilters[i].parentName + "\" Fullname=\"" + custombase_AddFilters[i].FullName + "\"></a></div>";
                else
                    html += "<div class=\"selected-filter\">" + custombase_AddFilters[i].Name + " <a class=\"remove-item\" onclick=\"RemoveCustomBaseGroup(this);\" UniqueId=\"" + custombase_AddFilters[i].UniqueId + "\" name=\"" + custombase_AddFilters[i].Name + "\" levelId=\"" + custombase_AddFilters[i].levelId + "\" parentName=\"" + custombase_AddFilters[i].parentName + "\" Fullname=\"" + custombase_AddFilters[i].FullName + "\"></a></div>";
                htmlText += custombase_AddFilters[i].Name;
            }
            //update in custombase popup

            for (var i = 0; i < custombase_AddFilters.length; i++) {
                ele = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\" style=\"width:10%;display:block;\"></div><div style=\"width:86%;border:0;cursor:auto;\" name=\"" + custombase_AddFilters[i].Name + "\" uniqueid=\"" + custombase_AddFilters[i].UniqueId + "\" class=\"stat-cust-estabmt\">" + custombase_AddFilters[i].Name + "</div></div>";
                $("#custom-base-add-filters ul").append(ele);
            }
        }
        if (custombase_Frequency.length > 0 && Sigtype_Id == "1") {
            if (html != "<div>")
                html += "<div class=\"seperater\">|</div>";

            for (var i = 0; i < custombase_Frequency.length; i++) {
                if (TabType == "trips")
                    html += "<div class=\"selected-filter\"> " + custombase_Frequency[i].Name + " <a class=\"remove-item\" onclick=\"RemoveCustomBaseFrequency(this);\" UniqueId=\"" + custombase_Frequency[i].UniqueId + "\" Id=\"" + custombase_Frequency[i].Id + "\" name=\"" + custombase_Frequency[i].Name + "\" levelId=\"" + custombase_Frequency[i].levelId + "\" parentName=\"" + custombase_Frequency[i].parentName + "\" Fullname=\"" + custombase_Frequency[i].FullName + "\"></a></div>";
                else
                    html += "<div class=\"selected-filter\">" + custombase_Frequency[i].Name + " <a class=\"\" onclick=\"RemoveCustomBaseFrequency(this);\" UniqueId=\"" + custombase_Frequency[i].UniqueId + "\" name=\"" + custombase_Frequency[i].Name + "\" levelId=\"" + custombase_Frequency[i].levelId + "\" parentName=\"" + custombase_Frequency[i].parentName + "\" Fullname=\"" + custombase_Frequency[i].FullName + "\"></a></div>";
                htmlText += custombase_Frequency[i].Name;
            }
            for (var i = 0; i < custombase_Frequency.length; i++) {
                ele = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\" style=\"width:10%;display:block;\"></div><div style=\"width:86%;border:0;cursor:auto;\" name=\"" + custombase_Frequency[i].Name + "\" uniqueid=\"" + custombase_Frequency[i].UniqueId + "\" class=\"stat-cust-estabmt\">" + custombase_Frequency[i].Name + "</div></div>";
                $("#custom-base-add-filters ul").append(ele);
            }
        }
        if (CompetitorCustomBaseRetailer.length > 0) {
            for (var i = 0; i < CompetitorCustomBaseRetailer.length; i++) {
                ele = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\" style=\"width:10%;display:block;\"></div><div style=\"width:86%;border:0;cursor:auto;\" name=\"" + CompetitorCustomBaseRetailer[i].Name + "\" uniqueid=\"" + CompetitorCustomBaseRetailer[i].UniqueId + "\" class=\"stat-cust-estabmt\">" + CompetitorCustomBaseRetailer[i].Name + ", " + CompetitorCustomBaseFrequency[i].Name + "</div></div>";
                $("#custom-base-add-filters ul").append(ele);
            }
        }
    }

    //For CustomBase Competitor Retailers
    if (CompetitorCustomBaseRetailer.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < CompetitorCustomBaseRetailer.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + " Cross Retailer Shopper: " + CompetitorCustomBaseRetailer[i].Name + ", " + CompetitorCustomBaseFrequency[i].Name + " <a class=\"remove-item\" onclick=\"RemoveCustomBaseCompetitor(this);\" Id=\"" + CompetitorCustomBaseRetailer[i].Id + "\" name=\"" + CompetitorCustomBaseRetailer[i].Name + "\" dbname=\"" + CompetitorCustomBaseRetailer[i].DBName + "\" UniqueId=\"" + CompetitorCustomBaseRetailer[i].UniqueId + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + " Cross Retailer Shopper: " + CompetitorCustomBaseRetailer[i].Name + ", " + CompetitorCustomBaseFrequency[i].Name + " <a class=\"remove-item\" onclick=\"RemoveCustomBaseCompetitor(this);\" Id=\"" + CompetitorCustomBaseRetailer[i].Id + "\" name=\"" + CompetitorCustomBaseRetailer[i].Name + "\" dbname=\"" + CompetitorCustomBaseRetailer[i].DBName + "\" UniqueId=\"" + CompetitorCustomBaseRetailer[i].UniqueId + "\"></a></div>";
            htmlText += " Cross Retailer Shopper: " + CompetitorCustomBaseRetailer[i].Name + ", " + CompetitorCustomBaseFrequency[i].Name;
        }
    }

    //For Channels
    if (selectedChannels.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < selectedChannels.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + selectedChannels[i].Name + " <a class=\"remove-item\" onclick=\"RemoveChannel(this);\" uniqueid=\"" + selectedChannels[i].UniqueId + "\" Id=\"" + selectedChannels[i].Id + "\" name=\"" + selectedChannels[i].Name + "\" Fullname=\"" + selectedChannels[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + selectedChannels[i].Name + " <a class=\"remove-item\" onclick=\"RemoveChannel(this);\"  uniqueid=\"" + selectedChannels[i].UniqueId + "\" Id=\"" + selectedChannels[i].Id + "\" name=\"" + selectedChannels[i].Name + "\" Fullname=\"" + selectedChannels[i].FullName + "\"></a></div>";
            htmlText += selectedChannels[i].Name;
        }
    }
    //Advanced Analyses
    if (SelectedAdvancedAnalyticsList.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < SelectedAdvancedAnalyticsList.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + SelectedAdvancedAnalyticsList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveAdvanceAnalytics(this,'" + SelectedAdvancedAnalyticsList[i].contentid + "');\" uniqueid=\"" + SelectedAdvancedAnalyticsList[i].UniqueId + "\" Id=\"" + SelectedAdvancedAnalyticsList[i].Id + "\" name=\"" + SelectedAdvancedAnalyticsList[i].Name + "\" parentname=\"" + SelectedAdvancedAnalyticsList[i].parentname + "\" Fullname=\"" + SelectedAdvancedAnalyticsList[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + SelectedAdvancedAnalyticsList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveAdvanceAnalytics(this,'" + SelectedAdvancedAnalyticsList[i].contentid + "');\"  uniqueid=\"" + SelectedAdvancedAnalyticsList[i].UniqueId + "\" Id=\"" + SelectedAdvancedAnalyticsList[i].Id + "\" name=\"" + SelectedAdvancedAnalyticsList[i].Name + "\" parentname=\"" + SelectedAdvancedAnalyticsList[i].parentname + "\" Fullname=\"" + SelectedAdvancedAnalyticsList[i].FullName + "\"></a></div>";
            htmlText += SelectedAdvancedAnalyticsList[i].Name;
        }
    }
    //For Measure
    if (Measurelist.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        //html += "<div>" + Measurelist[0].parentName;      
        for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + Measurelist[0].metriclist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveMeasure(this);\" uniqueid=\"" + Measurelist[0].metriclist[i].Id + "\" Id=\"" + Measurelist[0].metriclist[i].Id + "\" name=\"" + Measurelist[0].metriclist[i].Name + "\" parentname=\"" + Measurelist[0].metriclist[i].parentname + "\" Fullname=\"" + Measurelist[0].metriclist[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + Measurelist[0].metriclist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveMeasure(this);\"  uniqueid=\"" + Measurelist[0].metriclist[i].Id + "\" Id=\"" + Measurelist[0].metriclist[i].Id + "\" name=\"" + Measurelist[0].metriclist[i].Name + "\" parentname=\"" + Measurelist[0].metriclist[i].parentname + "\" Fullname=\"" + Measurelist[0].metriclist[i].FullName + "\"></a></div>";
            htmlText += Measurelist[0].metriclist[i].Name;
        }
    }
    //total Measures
    if (SelectedTotalMeasure.length > 0) {
        var sText = "";
        if (currentpage.indexOf("beverage") > 0)
            sText = "MONTHLY PURCHASE";
        else
            sText = "FREQUENCY";

        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < SelectedTotalMeasure.length; i++) {
            if (i > 0)

                html += "<div class=\"selected-filter\">, " + SelectedTotalMeasure[i].Name + " <a class=\"remove-item\" onclick=\"RemovetotalMeasure(this);\" UniqueId=\"" + SelectedTotalMeasure[i].UniqueId + "\" Id=\"" + SelectedTotalMeasure[i].Id + "\" name=\"" + SelectedTotalMeasure[i].Name + "\" Fullname=\"" + SelectedTotalMeasure[i].FullName + "\"></a></div>";

            else
                html += "<div class=\"selected-filter\"> " + SelectedTotalMeasure[i].Name + " <a class=\"remove-item\" onclick=\"RemovetotalMeasure(this);\" UniqueId=\"" + SelectedTotalMeasure[i].UniqueId + "\" Id=\"" + SelectedTotalMeasure[i].Id + "\" name=\"" + SelectedTotalMeasure[i].Name + "\" Fullname=\"" + SelectedTotalMeasure[i].FullName + "\"></a></div>";

            htmlText += SelectedTotalMeasure[i].Name;
        }
    }

    if (currentpage == "hdn-crossretailer-sarreport") {
        //For BenchMark
        if (sarRetailerBenchmarkList.length != 0) {
            if (html != "<div>")
                html += "<div class=\"seperater\">|&nbsp; Main Retailer : </div>"
            for (var i = 0; i < sarRetailerBenchmarkList.length; i++) {
                if (i > 0)

                    html += "<div class=\"selected-filter\">, " + sarRetailerBenchmarkList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveSarRetailer(this);\" UniqueId=\"" + sarRetailerBenchmarkList[i].UniqueId + "\" Id=\"" + sarRetailerBenchmarkList[i].Id + "\" name=\"" + sarRetailerBenchmarkList[i].Name + "\" parent-of-parent=\"" + sarRetailerBenchmarkList[i].ParentOfParent + "\"></a></div>";

                else
                    html += "<div class=\"selected-filter\"> " + sarRetailerBenchmarkList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveSarRetailer(this);\" UniqueId=\"" + sarRetailerBenchmarkList[i].UniqueId + "\" Id=\"" + sarRetailerBenchmarkList[i].Id + "\" name=\"" + sarRetailerBenchmarkList[i].Name + "\" parent-of-parent=\"" + sarRetailerBenchmarkList[i].ParentOfParent + "\"></a></div>";

                htmlText += sarRetailerBenchmarkList[i].Name;
            }
        }
        //For Custom Base
        if (sarRetailerCustomList.length != 0) {
            if (html != "<div>")
                html += "<div class=\"seperater\">|&nbsp; Comparision Retailer : </div>"
            for (var i = 0; i < sarRetailerCustomList.length; i++) {
                if (i > 0)

                    html += "<div class=\"selected-filter\">, " + sarRetailerCustomList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveSarRetailer(this);\" UniqueId=\"" + sarRetailerCustomList[i].UniqueId + "\" Id=\"" + sarRetailerCustomList[i].Id + "\" name=\"" + sarRetailerCustomList[i].Name + "\" parent-of-parent=\"" + sarRetailerCustomList[i].ParentOfParent + "\"></a></div>";

                else
                    html += "<div class=\"selected-filter\"> " + sarRetailerCustomList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveSarRetailer(this);\" UniqueId=\"" + sarRetailerCustomList[i].UniqueId + "\" Id=\"" + sarRetailerCustomList[i].Id + "\" name=\"" + sarRetailerCustomList[i].Name + "\" parent-of-parent=\"" + sarRetailerCustomList[i].ParentOfParent + "\"></a></div>";

                htmlText += sarRetailerCustomList[i].Name;
            }
        }
        //For Competitors
        if (sarCompetitorList.length != 0) {
            if (html != "<div>")
                html += "<div class=\"seperater\">|&nbsp; Competitors : </div>"
            for (var i = 0; i < sarCompetitorList.length; i++) {
                if (i > 0)

                    html += "<div class=\"selected-filter\">, " + sarCompetitorList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveSarCompetitor(this);\" UniqueId=\"" + sarCompetitorList[i].UniqueId + "\" Id=\"" + sarCompetitorList[i].Id + "\" name=\"" + sarCompetitorList[i].Name + "\" parent-of-parent=\"" + sarCompetitorList[i].ParentOfParent + "\"></a></div>";

                else
                    html += "<div class=\"selected-filter\"> " + sarCompetitorList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveSarCompetitor(this);\" UniqueId=\"" + sarCompetitorList[i].UniqueId + "\" Id=\"" + sarCompetitorList[i].Id + "\" name=\"" + sarCompetitorList[i].Name + "\" parent-of-parent=\"" + sarCompetitorList[i].ParentOfParent + "\"></a></div>";

                htmlText += sarCompetitorList[i].Name;
            }
        }


    }

    //For Demographic
    if (SelectedDempgraphicList.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + SelectedDempgraphicList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveDemographic(this);\" Id=\"" + SelectedDempgraphicList[i].Id + "\" UniqueId=\"" + SelectedDempgraphicList[i].UniqueId + "\" name=\"" + SelectedDempgraphicList[i].Name + "\" Fullname=\"" + SelectedDempgraphicList[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + SelectedDempgraphicList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveDemographic(this);\" Id=\"" + SelectedDempgraphicList[i].Id + "\" UniqueId=\"" + SelectedDempgraphicList[i].UniqueId + "\" name=\"" + SelectedDempgraphicList[i].Name + "\" Fullname=\"" + SelectedDempgraphicList[i].FullName + "\"></a></div>";
            htmlText += SelectedDempgraphicList[i].Name;
        }
    }

    //else {
    //    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel2").hide();
    //    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel3").hide();
    //}
    if (sBevarageSelctionType.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < sBevarageSelctionType.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + sBevarageSelctionType[i].Name + " </div>";
            else
                html += "<div class=\"selected-filter\">" + sBevarageSelctionType[i].Name + " </div>";
            htmlText += sBevarageSelctionType[i].Name;
        }
    }
    html += "</div>";
    //html += "</div>";
    $("#textLength").text("");
    $("#textLength").text(htmlText);
    //var sCount = getWidthOfText($("#textLength").text(), "sans - serif", "11");
    //if (sCount >= 450) {
    //    $("#SelectedFilters #scrollableselection").css("width", 890 + (sCount - 470) * (890 / 550) + 50 + "px");
    //    //SetScroll($("#SelectedFilters"), "1px #db535a", 0, 0, 0, 0, 8);
    //}
    $("#SelectedFilters #scrollableselection").html(html);
}
//End
function getWidthOfText(txt, fontname, fontsize) {
    // Create a dummy canvas (render invisible with css)
    var c = document.createElement('canvas');
    // Get the context of the dummy canvas
    var ctx = c.getContext('2d');
    // Set the context.font to the font that you are using
    ctx.font = fontsize + 'px' + fontname;
    // Measure the string 
    // !!! <CRUCIAL>  !!!
    var length = ctx.measureText(txt).width;
    // !!! </CRUCIAL> !!!
    // Return width
    return length;
}
//Search Functionality in filters
function SearchFilters(SearchFor, SearchBox, AppendTo, data) {
    //data = data.getUnique();
    ////$("#Search-Retailers").autocomplete({
    //$("#" + SearchBox).autocomplete({
    //    delay: 0,
    //    minLength: 2,
    //    appendTo: "#" + AppendTo,//"#Retailer-Search-Content",
    //    autoFocus: false,
    //    position: {
    //        my: "left top",
    //        at: "left bottom",
    //        collision: "none"
    //    },
    //    open: function () {
    //        //$(".Search-Filter .ui-widget-content").css("max-width", "196px");
    //        $(".Search-Filter .ui-widget-content").css("max-width", "300px");
    //        $(".Search-Filter .ui-widget-content").css("width", "231px");
    //        //$(".Search-Filter .ui-widget-content").css("max-width", "203px");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "black");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("cursor", "pointer");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("background-color", "transparent");
    //        //$(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("border","solid 0.1px blue");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("padding", "4px");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("margin-left", "4px");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("margin-right", "4px");
    //        //$(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "#7F7F7F");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "black");
    //        $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("text-transform", "uppercase");
    //    },
    //    close: function (event, ui) {

    //        if ($.isNumeric($("#" + SearchBox).val()))
    //            $(".txt-search").val("");

    //    },
    //    focus: function (event, ui) {
    //        this.value = ui.item.label;
    //        // or $('#autocomplete-input').val(ui.item.label);

    //        // Prevent the default focus behavior.
    //        event.preventDefault();
    //        // or return false;
    //    },
    //    source: function (request, response) {
    //        var sArr = [];
    //        var sArray = _.map(data).map(function (x) {
    //            sArr.push({
    //                label: x.split("|")[1].toUpperCase(),
    //                value: x.split("|")[0],
    //            });
    //            return {
    //                label: x.split("|")[1].toUpperCase(),
    //                value: x.split("|")[0],
    //            };
    //        });
    //        //response(sArray);
    //        if (request.term != "")
    //            response($.ui.autocomplete.filter(sArray, request.term));
    //        else
    //            $(".Search-Filter .ui-widget-content").css("display", "none");
    //        return;
    //    },
    //    select: function (e, ui) {
    //        //if (SearchFor == "Retailer") {
    //        //    $("#Search-Retailers").val(ui.item.label);
    //        //    var CompObj = $(".Retailer .Comparison[uniqueid='" + ui.item.value + "' i]");
    //        //    if (CompObj.length <= 0)
    //        //        CompObj = $("#ChannelOrCategoryContent ul li span[uniqueid='" + ui.item.value + "' i]");
    //        //    SelectComparison(CompObj);
    //        //}
    //        //else if (SearchFor == "Beverage") {
    //        //    $("#Search-Beverages").val(ui.item.label);
    //        //    var CompObj = $(".Beverage .Comparison[uniqueid='" + ui.item.value + "' i]");
    //        //    if (CompObj.length <= 0)
    //        //        CompObj = $("#BeverageOrCategoryContent ul li span[uniqueid='" + ui.item.value + "' i]");
    //        //    SelectBevComparison(CompObj);
    //        //}
    //        //else if (SearchFor == "NonBeverage") {
    //        //    $("#Search-Beverages").val(ui.item.label);
    //        //    var CompObj = $("#BGMNonBeverageDiv span[uniqueid='" + ui.item.value + "' i]").parent();
    //        //    SelectBevComparison(CompObj);
    //        //}
    //        //else if (SearchFor == "DemographicFilters") {
    //        //    var ObjData = $("#SecondaryAdvancedFilterContent .DemographicList span[uniqueId='" + ui.item.value + "' i]").parent();
    //        //if (ObjData.length <= 0)
    //        //    ObjData = $("#ThirdLevelAdvancedFilterContent .DemographicList span[uniqueId='" + ui.item.value + "' i]").parent();
    //        //    SelectDemographic(ObjData);
    //        //}
    //        //else if (SearchFor == "Measure") {
    //        //    isSearch = "1";
    //        //    var ObjData = $("#MeasureTypeContent ul li[uniqueid='" + ui.item.value + "' i]");
    //        //    var ObjDataParentUniqueId = ObjData.length > 0 ? ObjData.parent().attr("uniqueid") : "";
    //        //    var ObjDataParentName = ObjData.length > 0 ? ObjData.parent().attr("name") : "";
    //        //    var _metriclist = [];
    //        //    $("#MeasureTypeContent ul[uniqueid='" + ObjDataParentUniqueId + "'] li").each(function () {
    //        //        _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html() });
    //        //    });
    //        //    var objDataParent = $("#MeasureTypeHeaderContent ul li[uniqueid='" + ObjDataParentUniqueId + "' i][name='" + ObjDataParentName + "' i]");
    //        //    if (objDataParent.length <= 0)
    //        //        objDataParent = $("#MeasureTypeHeaderContentSubLevel ul li[uniqueid='" + ObjDataParentUniqueId + "' i][name='" + ObjDataParentName + "' i]");
    //        //    if (ObjData.length > 0 && objDataParent.length > 0) {
    //        //        if ($("#MeasureTypeContent").is(':visible') == true)
    //        //            var sVisible = 1;
    //        //        else
    //        //            var sVisible = 0;
    //        //        var sStatus = "0";
    //        //        _.filter(_metriclist, function (i) { if (i.Id.trim() == ObjData.attr("uniqueid").trim()) sStatus = "1"; })
    //        //        if (Measurelist.length > 0 && (Measurelist[0].uniqueid.trim() == objDataParent.attr("uniqueid").trim() || sStatus == "1") && Measurelist[0].parentName.trim() == objDataParent.attr("name").trim()) {
    //        //            SelecMeasureMetricName(ObjData);
    //        //        }
    //        //        else {
    //        //            SelecMeasure(objDataParent);
    //        //            Measurelist[0].metriclist = [];
    //        //            SelecMeasureMetricName(ObjData);
    //        //        }



    //        //        if (sVisible == 0) {
    //        //            $("#MeasureTypeContent").hide();
    //        //            $("#MeasuresHeadingLevel3").hide();
    //        //        }
    //        //        //$("#MeasureTypeHeaderContent").show();
    //        //        //$("#MeasureTypeHeaderContent ul li").show();
    //        //        SetScroll($("#MeasureTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //        SetScroll($("#MeasureTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //    }
    //        //    ObjData = $("#MeasureTypeContent ul li[name='" + ui.item.label + "' i][ id='" + ui.item.value + "' i]");
    //        //    //if (ObjData.length > 0)
    //        //    //    SelecMeasureMetricName(ObjData);
    //        //    $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel3").hide();
    //        //    $("#MeasureTypeContent").hide();
    //        //    $("#MeasureTypeHeaderContent *").removeClass("Selected");
    //        //    $("#MeasureTypeHeaderContentSubLevel *").removeClass("Selected");

    //        //    var chartlist = [];

    //        //    chartlist = objDataParent.attr("charttypePIT") != undefined ? objDataParent.attr("charttypePIT").split("|") : [];
    //        //    ChartType = chartlist[0];


    //        //    if (ModuleBlock == "TREND")
    //        //        chartlist = objDataParent.attr("charttypetrend") != undefined ? objDataParent.attr("charttypetrend").split("|") : [];
    //        //    else
    //        //        chartlist = objDataParent.attr("charttypePIT") != undefined ? objDataParent.attr("charttypePIT").split("|") : [];

    //        //    ChartType = chartlist[0];

    //        //    $("#chart-list").html("");
    //        //    for (var i = 0; i < chartlist.length; i++) {

    //        //        var sImageClassName = ChartImagePosition(chartlist[i].toString());
    //        //        //$("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\"><div id=\"TextForChart\">" + chartlist[i] + "</div></div>").appendTo("#chart-list");
    //        //        $("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\" style=\"background-image: url('../Images/sprite.svg');background-position:" + sImageClassName + "\"></div>").appendTo("#chart-list");
    //        //    }
    //        //    isSearch = "0";
    //        //    ShowSelectedFilters();
    //        //}
    //        //else if (SearchFor == "Type") {
    //        //    var isGeo = "0";
    //        //var ObjData = $("#GroupTypeContent ul span[uniqueid='" + ui.item.value + "' i]");
    //        //if (ObjData.length <= 0) {
    //        //    ObjData = $("#GroupTypeContentSub ul span[uniqueid='" + ui.item.value + "' i]");
    //        //    if (ObjData.length > 0)
    //        //        isGeo = "1";
    //        //}
    //        //if (ObjData.length > 0) {
    //        //    if(isGeo == "0"){
    //        //        var Obj = $("#GroupTypeHeaderContent ul li div span[id='" + ObjData.attr("parentid").trim() + "' i][name='" + ObjData.attr("parentname").trim() + "']");
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentid").trim())
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        else {
    //        //            SelecGroup(Obj.parent().parent());
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        }
    //        //        $("#GroupTypeContent").show();
    //        //        $("#GroupTypeHeaderContent ul li").show();
    //        //        SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //        SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //    }
    //        //    else
    //        //    {
    //        //        $("#GroupTypeContent div ul").hide();
    //        //        $("#GroupTypeContent").show();
    //        //        $("#GroupTypeContent div[name='Geography']").show();
    //        //        $("#GroupTypeContent div[name='Geography'] ul").css("display", "block");
    //        //        $("#GroupTypeContent div[name='Geography'] ul div").show();
    //        //        var Obj = $("#GroupTypeContent div[name='Geography'] span[name='" + ObjData.attr("parentname").trim() + "']").parent();
    //        //        //Obj.trigger("click");
    //        //        $(Obj).addClass("Selected");
    //        //        $("#GroupTypeContentSub .DemographicList").hide();
    //        //        $("#GroupTypeContentSub").show();
    //        //        $("#GroupTypeContentSub div[name='" + $(Obj).find(".lft-popup-ele-label").attr("name") + "' i]").show();
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").text($(Obj).find(".lft-popup-ele-label").attr("name").toUpperCase());
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").show();
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").css("width", "287px");
    //        //        SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").show()
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").show()
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentid").trim())
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        else {
    //        //            Grouplist = [];
    //        //            $("#GroupTypeHeaderContent ul li");
    //        //            $("#GroupTypeContent div").removeClass("Selected");
    //        //            $(Obj).addClass("Selected");
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        }
    //        //        $("#GroupTypeContent").show();
    //        //        $("#GroupTypeContentSub").show();
    //        //        $("#GroupTypeHeaderContent ul li").show();
    //        //        SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //        SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //        SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //    }
    //        //    }

    //        //}
    //        //else if (SearchFor == "Geography") {
    //        //    var ObjData = $("#ThirdGeographyFilterList .DemographicList span[uniqueId='" + ui.item.value + "' i]").parent();
    //        //    SelectGeographyData(ObjData);
    //        //}
    //        //if (SearchFor == "Frequency" || SearchFor == "Monthly") {
    //        //    if (SearchFor == "Frequency")
    //        //        var ObjData = $("#frequency_containerId span[name='" + ui.item.label + "' i]").parent();
    //        //    else
    //        //        var ObjData = $("#frequency_containerId * [name='" + ui.item.label + "' i][id='" + ui.item.value + "' i][onClick='SelectFrequency(this);']");
    //        //    if (ObjData.length <= 0) {
    //        //        ObjData = $("#frequency_containerId-SubLevel1 * [name='" + ui.item.label + "' i]").parent();
    //        //        if (ObjData.length > 0) {
    //        //            var Obj = $("#frequency_containerId span[name='" + $("#frequency_containerId-SubLevel1 * [name='" + ui.item.label + "' i]").attr("parent") + "' i]").parent()
    //        //            DisplaySecondaryFrequency(Obj);
    //        //        }
    //        //    }
    //        //    if (ObjData.length <= 0)
    //        //        ObjData = $("#frequency_containerId-SubLevel2 * [name='" + ui.item.label + "' i]").parent();
    //        //    //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")

    //        //    SelectFrequency(ObjData);
    //        //    //else
    //        //    //    DisplaySecondaryFrequency(ObjData);
    //        //}
    //        //$(".txt-search").val("");
    //    }

    //});

    //$("#" + SearchBox).on("autocompleteselect", function (e, ui) {
    //    if (SearchFor == "Retailer") {
    //        $("#Search-Retailers").val(ui.item.label);
    //        var CompObj = $(".Retailer .Comparison[uniqueid='" + ui.item.value + "']");
    //        if (CompObj.length <= 0)
    //            CompObj = $("#ChannelOrCategoryContent ul li span[uniqueid='" + ui.item.value + "']");
    //        if (ui.item.label == "TOTAL") {
    //            CompObj = $("#ChannelOrCategoryContent ul li span[uniqueid='" + ui.item.value + "']");
    //        }
    //        SelectComparison(CompObj);
    //    }
    //    if (SearchFor == "Custombase-Retailers") {
    //        var CompObj = $(".Custombase-Retailers .Comparison[uniqueid='" + ui.item.value + "']").parent("li").parent("ul").parent(".pathtopurchase-custombase");
    //        if (CompObj.length <= 0)
    //            CompObj = $(".Custombase-Retailers ul li[uniqueid='" + ui.item.value + "']");
    //        SelectPathToPurchaseCustomBase(CompObj);
    //    }
    //    else if (SearchFor == "Beverage") {
    //        $("#Search-Beverages").val(ui.item.label);
    //        //var CompObj = $(".Beverage .Comparison[uniqueid='" + ui.item.value + "']");
    //        //if (CompObj.length <= 0)
    //        //    CompObj = $("#BeverageOrCategoryContent ul li span[uniqueid='" + ui.item.value + "']");
    //        var CompObj = $(".Beverages .Comparison[id='" + ui.item.value + "']");

    //        if (CompObj.length <= 0)
    //            CompObj = $("#BeverageOrCategoryContent ul li span[id='" + ui.item.value + "']");
    //        if (CompObj.length <= 0)
    //            CompObj = $("#BGMNonBeverageDiv ul li span[id='" + ui.item.value + "']").parent();

    //        SelectBevComparison(CompObj);
    //    }
    //    else if (SearchFor == "NonBeverage") {
    //        $("#Search-Beverages").val(ui.item.label);
    //        var CompObj = $("#BGMNonBeverageDiv span[uniqueid='" + ui.item.value + "']").parent();
    //        SelectBevComparison(CompObj);
    //    }
    //    else if (SearchFor == "DemographicFilters") {
    //        var ObjData = $("#SecondaryAdvancedFilterContent .DemographicList span[uniqueId='" + ui.item.value + "']").parent();
    //        if (ObjData.length <= 0)
    //            ObjData = $("#ThirdLevelAdvancedFilterContent .DemographicList span[uniqueId='" + ui.item.value + "']").parent();
    //        if (ObjData.length <= 0)
    //            ObjData = $("#FourthLevelAdvancedFilterContent .DemographicList span[uniqueId='" + ui.item.value + "']").parent();

    //        if ((ObjData.length <= 0) && ((currentpage.indexOf("hdn-report") > -1) && currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive" && currentpage != "hdn-report-comparebeveragesmonthlypluspurchasers" && currentpage != "hdn-report-beveragemonthlypluspurchasersdeepdive")) {
    //            var ObjData = $("#SecondaryAdvancedFilterContentAdv .DemographicList span[uniqueId='" + ui.item.value + "']").parent();
    //            if (ObjData.length <= 0)
    //                ObjData = $("#ThirdLevelAdvancedFilterContentAdv .DemographicList span[uniqueId='" + ui.item.value + "']").parent();
    //            if (ObjData.length <= 0)
    //                ObjData = $("#FourthLevelAdvancedFilterContentAdv .DemographicList span[uniqueId='" + ui.item.value + "']").parent();
    //        }

    //        SelectDemographic(ObjData);
    //    }
    //    else if (SearchFor == "Measure") {
    //        isSearch = "1";
    //        var ObjData = $("#MeasureTypeContentTrip ul li[uniqueid='" + ui.item.value + "']");
    //        if (ObjData.length > 0) {
    //            if (ObjData.attr("mainparentname") == "Demographics") {
    //                $(".shoppertrip-Toggle").show();

    //                var sChange = "";
    //                if ($("#guest-visit-toggle").is(":checked") == false) {
    //                    TabType = "trips";
    //                    if (sVisitsOrGuests == 1)
    //                        sChange = "false";
    //                    else
    //                        sChange = "true";
    //                    sVisitsOrGuests = 1;
    //                    sBevarageSelctionType = [];

    //                    if (Measurelist.length > 0 && Measurelist[0].metriclist.length > 0) {
    //                        //if (Measurelist[0].metriclist[0].SelType == "1" || Measurelist[0].metriclist[0].SelType == "3") {
    //                        //    Measurelist = [];
    //                        //    //$(".MeasureType .trip").hide();
    //                        //    //$(".MeasureType .Shopper").hide();
    //                        //    $("#MeasuresHeadingLevel1").hide();
    //                        //    $("#MeasuresHeadingLevel2").hide();
    //                        //    $("#MeasuresHeadingLevel3").hide();
    //                        //    $("#MeasuresHeadingLevel4").hide();
    //                        //    $("#MeasureTypeShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
    //                        //        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //                        //    });
    //                        //    $("#MeasureTypeShopperTripHeader *").removeClass("Selected");
    //                        //    $("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //                        //    sBevarageSelctionType = [];
    //                        //}
    //                        $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
    //                    }
    //                }
    //                else if ($("#guest-visit-toggle").is(":checked") == true) {
    //                    TabType = "shopper";
    //                    if (sVisitsOrGuests == 2)
    //                        sChange = "false";
    //                    else
    //                        sChange = "true";
    //                    sVisitsOrGuests = 2;
    //                    if (currentpage.indexOf("chart") > -1) {
    //                        $("#adv-bevselectiontype-freq").hide();
    //                        sBevarageSelctionType = [];
    //                    }
    //                    else
    //                        $(".beverageItems ul div[uniqueid='1']").trigger("click");

    //                    if (ObjData.length > 0) {
    //                        if (Measurelist.length > 0 && Measurelist[0].metriclist.length > 0) {
    //                            //if (Measurelist[0].metriclist[0].SelType == "2" || Measurelist[0].metriclist[0].SelType == "4") {
    //                            //    Measurelist = [];
    //                            //    //$(".MeasureType .trip").hide();
    //                            //    //$(".MeasureType .Shopper").hide();
    //                            //    $("#MeasuresHeadingLevel1").hide();
    //                            //    $("#MeasuresHeadingLevel2").hide();
    //                            //    $("#MeasuresHeadingLevel3").hide();
    //                            //    $("#MeasuresHeadingLevel4").hide();
    //                            //    $("#MeasureTypeShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
    //                            //        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //                            //    });
    //                            //    $("#MeasureTypeShopperTripHeader *").removeClass("Selected");
    //                            //    $("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //                            //    $(".beverageItems ul div[uniqueid='1']").trigger("click");
    //                            //}

    //                        }

    //                        if (sBevarageSelctionType.length <= 0)
    //                            $(".beverageItems ul div[uniqueid='1']").trigger("click");
    //                        $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
    //                    }
    //                }
    //            }
    //            else {
    //                if (ObjData.attr("seltypeid") == "2" || ObjData.attr("seltypeid") == "4") {
    //                    //Shopper
    //                    //ObjData = $("#MeasureTypeContentShopper ul li[uniqueid='" + ui.item.value + "']");
    //                    if (ObjData.length > 0) {
    //                        if (Measurelist.length > 0 && Measurelist[0].metriclist.length > 0) {
    //                            if (Measurelist[0].metriclist[0].SelType == "1" || Measurelist[0].metriclist[0].SelType == "3") {
    //                                Measurelist = [];
    //                                //$(".MeasureType .trip").hide();
    //                                //$(".MeasureType .Shopper").hide();
    //                                $("#MeasuresHeadingLevel1").hide();
    //                                $("#MeasuresHeadingLevel2").hide();
    //                                $("#MeasuresHeadingLevel3").hide();
    //                                $("#MeasuresHeadingLevel4").hide();
    //                                $("#MeasureTypeShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
    //                                    $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //                                });
    //                                $("#MeasureTypeShopperTripHeader *").removeClass("Selected");
    //                                $("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //                                $(".beverageItems ul div[uniqueid='1']").trigger("click");
    //                            }

    //                        }
    //                        sVisitsOrGuests = 2;
    //                        TabType = "shopper";
    //                        if (sBevarageSelctionType.length <= 0)
    //                            $(".beverageItems ul div[uniqueid='1']").trigger("click");
    //                        $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
    //                    }
    //                }
    //                else {
    //                    //Trip
    //                    if (Measurelist.length > 0 && Measurelist[0].metriclist.length > 0) {
    //                        if (Measurelist[0].metriclist[0].SelType == "2" || Measurelist[0].metriclist[0].SelType == "4") {
    //                            Measurelist = [];
    //                            //$(".MeasureType .trip").hide();
    //                            //$(".MeasureType .Shopper").hide();
    //                            $("#MeasuresHeadingLevel1").hide();
    //                            $("#MeasuresHeadingLevel2").hide();
    //                            $("#MeasuresHeadingLevel3").hide();
    //                            $("#MeasuresHeadingLevel4").hide();
    //                            $("#MeasureTypeShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
    //                                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //                            });
    //                            $("#MeasureTypeShopperTripHeader *").removeClass("Selected");
    //                            $("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //                            sBevarageSelctionType = [];
    //                        }

    //                    }
    //                    sVisitsOrGuests = 1;
    //                    TabType = "trips";
    //                    $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");

    //                }
    //            }
    //        }
    //        GetDefaultFrequency();
    //        HideOrShowFilters();
    //        if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1))
    //            $("#adv-bevselectiontype-freq").show();
    //        else {
    //            $("#adv-bevselectiontype-freq").hide();
    //            sBevarageSelctionType = [];
    //        }
    //        var ObjDataParentUniqueId = ObjData.length > 0 ? ObjData.parent().attr("uniqueid") : "";
    //        var ObjDataParentName = ObjData.length > 0 ? ObjData.parent().attr("name") : "";
    //        var _metriclist = [];
    //        //if (sVisitsOrGuests == 1)
    //        {
    //            $("#MeasureTypeContentTrip ul[uniqueid='" + ObjDataParentUniqueId + "'] li").each(function () {
    //                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html(), Type: $(this).attr("Type"), SelType: $(this).attr("seltypeid") });
    //            });
    //        }
    //        //else if (sVisitsOrGuests == 2) {
    //        //    $("#MeasureTypeContentShopper ul[uniqueid='" + ObjDataParentUniqueId + "'] li").each(function () {
    //        //        _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html(), Type: $(this).attr("Type"), SelType: $(this).attr("seltypeid") });
    //        //    });
    //        //}

    //        //if (sVisitsOrGuests == 1)
    //        {
    //            var objDataParent = $("#MeasureTypeHeaderContentTrip ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    //        }
    //        //else if (sVisitsOrGuests == 2) {
    //        //    var objDataParent = $("#MeasureTypeHeaderContentShopper ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    //        //}

    //        if (objDataParent.length <= 0) {
    //            //if (sVisitsOrGuests == 1)
    //            {
    //                objDataParent = $("#MeasureTypeHeaderContentSubLevelTrip ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    //            }
    //            //else if (sVisitsOrGuests == 2) {
    //            //    objDataParent = $("#MeasureTypeHeaderContentSubLevelShopper ul li[uniqueid='" + ObjDataParentUniqueId + "'][name='" + ObjDataParentName + "']");
    //            //}

    //        }
    //        if (ObjData.length > 0 && objDataParent.length > 0) {
    //            if (($("#MeasureTypeContentTrip").is(':visible') == true) || ($("#MeasureTypeContentShopper").is(':visible') == true))
    //                var sVisible = 1;
    //            else
    //                var sVisible = 0;
    //            var sStatus = "0";
    //            _.filter(_metriclist, function (i) { if (i.Id.trim() == ObjData.attr("uniqueid").trim()) sStatus = "1"; })
    //            if (Measurelist.length > 0 && (Measurelist[0].uniqueid.trim() == objDataParent.attr("uniqueid").trim() || sStatus == "1") && Measurelist[0].parentName.trim() == objDataParent.attr("name").trim()) {
    //                SelecMeasureMetricName(ObjData);
    //            }
    //            else {
    //                SelecMeasure(objDataParent);
    //                Measurelist[0].metriclist = [];
    //                SelecMeasureMetricName(ObjData);
    //            }



    //            if (sVisible == 0) {
    //                $("#MeasureTypeContentTrip").hide();
    //                $("#MeasureTypeContentShopper").hide();
    //                $("#MeasuresHeadingLevel3").hide();
    //            }
    //            //$("#MeasureTypeHeaderContent").show();
    //            //$("#MeasureTypeHeaderContent ul li").show();
    //            //if (sVisitsOrGuests == 1)
    //            {
    //                SetScroll($("#MeasureTypeHeaderContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //                SetScroll($("#MeasureTypeContentTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //            }
    //            //else if (sVisitsOrGuests == 2) {
    //            //    SetScroll($("#MeasureTypeHeaderContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //            //    SetScroll($("#MeasureTypeContentShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //            //}


    //        }
    //        //if (sVisitsOrGuests == 1)
    //        {
    //            ObjData = $("#MeasureTypeContentTrip ul li[name='" + ui.item.label + "'][ id='" + ui.item.value + "']");
    //        }
    //        //else if (sVisitsOrGuests == 2) {
    //        //    ObjData = $("#MeasureTypeContentShopper ul li[name='" + ui.item.label + "'][ id='" + ui.item.value + "']");
    //        //}

    //        //if (ObjData.length > 0)
    //        //    SelecMeasureMetricName(ObjData);
    //        $(".AdvancedFiltersDemoHeading #MeasuresHeadingLevel3").hide();
    //        $("#MeasureTypeContentTrip").hide();
    //        $("#MeasureTypeContentShopper").hide();
    //        $("#MeasureTypeHeaderContentTrip *").removeClass("Selected");
    //        $("#MeasureTypeHeaderContentTrip *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#MeasureTypeHeaderContentShopper *").removeClass("Selected");
    //        $("#MeasureTypeHeaderContentShopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#MeasureTypeHeaderContentSubLevelTrip *").removeClass("Selected");
    //        $("#MeasureTypeHeaderContentSubLevelTrip *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#MeasureTypeHeaderContentSubLevelShopper *").removeClass("Selected");
    //        $("#MeasureTypeHeaderContentSubLevelShopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");

    //        var chartlist = [];

    //        chartlist = objDataParent.attr("charttypePIT") != undefined ? objDataParent.attr("charttypePIT").split("|") : [];
    //        ChartType = chartlist[0];


    //        if (ModuleBlock == "TREND")
    //            chartlist = objDataParent.attr("charttypetrend") != undefined ? objDataParent.attr("charttypetrend").split("|") : [];
    //        else
    //            chartlist = objDataParent.attr("charttypePIT") != undefined ? objDataParent.attr("charttypePIT").split("|") : [];

    //        ChartType = chartlist[0];

    //        $("#chart-list").html("");
    //        for (var i = 0; i < chartlist.length; i++) {

    //            var sImageClassName = ChartImagePosition(chartlist[i].toString());
    //            //$("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\"><div id=\"TextForChart\">" + chartlist[i] + "</div></div>").appendTo("#chart-list");
    //            $("<div class=\"chart-type\" chart-name=\"" + chartlist[i] + "\" style=\"background-image: url('../Images/sprite.svg?11');background-position:" + sImageClassName + "\"></div>").appendTo("#chart-list");
    //        }
    //        $("#LinkForCharts").show();

    //        isSearch = "0";

    //        if (TabType.toString() == "") {
    //            TabType = "trips";
    //            $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
    //        }
    //        //$(".MeasureType").show();
    //        $(".measure_img").css("background-position", "-556px -157px");
    //        $("#MeasureType").addClass("ActiveFilter");
    //        $("#MeasureType").trigger("click");
    //        ShowSelectedFilters();
    //    }
    //    else if (SearchFor == "Type") {
    //        var isGeo = "0";

    //        var ObjData = $("#GroupTypeContent ul span[uniqueid='" + ui.item.value + "']");
    //        if (ObjData.length <= 0)
    //            ObjData = $("#GroupTypeContent ul span[uniqueid='" + ui.item.value + "']");
    //        if (ObjData.length <= 0) {
    //            ObjData = $("#GroupTypeContentSub ul span[uniqueid='" + ui.item.value + "']");
    //            if (ObjData.length > 0) {
    //                if ($(ObjData).attr("isgeography") == "true")
    //                    isGeo = "1";
    //            }
    //        }
    //        if (ObjData.length <= 0) {
    //            ObjData = $("#GroupTypeGeoContentSub ul span[uniqueid='" + ui.item.value + "']");
    //            if (ObjData.length > 0)
    //                isGeo = "1";
    //        }

    //        if (isGeo != "1")
    //            var Obj = $("#GroupTypeHeaderContent ul li div span[id='" + ObjData.attr("parentid").trim() + "'][name='" + ObjData.attr("parentname").trim() + "']");
    //        else if (isGeo == "1")
    //            var Obj = $("#GroupTypeContent div[name='Geography'] span[name='" + ObjData.attr("parentname").trim() + "']").parent();

    //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentid").trim())
    //            SelecGroupMetricName(ObjData.parent());
    //        else {
    //            SelecGroup(Obj.parent().parent());
    //            SelecGroupMetricName(ObjData.parent());
    //        }

    //        $("#GroupTypeContent").hide();
    //        $("#GroupTypeContentSub").hide();
    //        $("#GroupTypeGeoContentSub").hide();
    //        $("#grouptypeHeadingLevel2").hide();
    //        $("#grouptypeHeadingLevel3").hide();
    //        $("#grouptypeHeadingLevel4").hide();
    //        $("#GroupTypeHeaderContent .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $("#GroupTypeHeaderContent *").removeClass("Selected");
    //        $("#GroupTypeHeaderContent *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        //if (ObjData.length <= 0) {
    //        //    ObjData = $("#GroupTypeContentSub ul span[uniqueid='" + ui.item.value + "' i]");
    //        //    //if (ObjData.length > 0)
    //        //    //    isGeo = "1";
    //        //}

    //        //if (ObjData.length > 0) {
    //        //    if (isGeo == "0") {
    //        //        var Obj = $("#GroupTypeHeaderContent ul li div span[id='" + ObjData.attr("parentid").trim() + "' i][name='" + ObjData.attr("parentname").trim() + "']");
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentid").trim())
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        else {
    //        //            SelecGroup(Obj.parent().parent());
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        }
    //        //        $("#GroupTypeContent").show();
    //        //        $("#GroupTypeHeaderContent ul li").show();
    //        //        SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //        SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //    }
    //        //    else {
    //        //        $("#GroupTypeContent div ul").hide();
    //        //        $("#GroupTypeContent").show();
    //        //        $("#GroupTypeContent div[name='Geography']").show();
    //        //        $("#GroupTypeContent div[name='Geography'] ul").css("display", "block");
    //        //        $("#GroupTypeContent div[name='Geography'] ul div").show();
    //        //        var Obj = $("#GroupTypeContent div[name='Geography'] span[name='" + ObjData.attr("parentname").trim() + "']").parent();
    //        //        //Obj.trigger("click");
    //        //        $(Obj).addClass("Selected");
    //        //        $("#GroupTypeContentSub .DemographicList").hide();
    //        //        $("#GroupTypeContentSub").show();
    //        //        $("#GroupTypeContentSub div[name='" + $(Obj).find(".lft-popup-ele-label").attr("name") + "' i]").show();
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").text($(Obj).find(".lft-popup-ele-label").attr("name").toUpperCase());
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").show();
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").css("width", "287px");
    //        //        SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);

    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").show()
    //        //        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").show()
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentid").trim())
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        else {
    //        //            Grouplist = [];
    //        //            $("#GroupTypeHeaderContent ul li");
    //        //            $("#GroupTypeContent div").removeClass("Selected");
    //        //            $(Obj).addClass("Selected");
    //        //            SelecGroupMetricName(ObjData.parent());
    //        //        }
    //        //        $("#GroupTypeContent").show();
    //        //        $("#GroupTypeContentSub").show();
    //        //        $("#GroupTypeHeaderContent ul li").show();
    //        //        SetScroll($("#GroupTypeContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //        SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //        SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    //        //    }
    //        //}

    //    }
    //    else if (SearchFor == "Geography") {
    //        var ObjData = $("#ThirdGeographyFilterList .DemographicList span[uniqueId='" + ui.item.value + "']").parent();
    //        if (ObjData.length <= 0)
    //            var ObjData = $("#FourthGeographyFilterList .DemographicList span[uniqueId='" + ui.item.value + "']").parent();
    //        SelectGeographyData(ObjData);
    //    }
    //    if (SearchFor == "Frequency" || SearchFor == "Monthly") {
    //        if (SearchFor == "Frequency")
    //            var ObjData = $("#frequency_containerId span[name='" + ui.item.label + "']").parent();
    //        else
    //            var ObjData = $("#frequency_containerId * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectFrequency(this);']");
    //        if (ObjData.length <= 0) {
    //            ObjData = $("#frequency_containerId-SubLevel1 * [name='" + ui.item.label + "']").parent();
    //            if (ObjData.length > 0) {
    //                var Obj = $("#frequency_containerId span[name='" + $("#frequency_containerId-SubLevel1 * [name='" + ui.item.label + "']").attr("parent") + "']").parent()
    //                DisplaySecondaryFrequency(Obj);
    //            }
    //        }
    //        if (ObjData.length <= 0)
    //            ObjData = $("#frequency_containerId-SubLevel2 * [name='" + ui.item.label + "']").parent();
    //        //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")

    //        SelectFrequency(ObjData);
    //        //else
    //        //    DisplaySecondaryFrequency(ObjData);
    //    }
    //    if (SearchFor == "AdvancedAnalytics") {
    //        //$("#advanced-analytics-Channel-Shopper").hide();
    //        //$("#advanced-analytics-Retailer-Trips").hide();
    //        //$("#advanced-analytics-Channel-Trips").hide();
    //        //$("#advanced-analytics-Retailer-Shopper").hide();
    //        //$("#advancedanalyticsHeadingLevel1").hide();
    //        $("#advancedanalyticsHeadingLevel1").show();
    //        //$("#advancedanalyticsHeadingLevel2").hide();
    //        $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
    //        $("#advanced-analytics-Retailer-Trips .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $("#advanced-analytics-Channel-Trips .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $("#advanced-analytics-Retailer-Shopper .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $("#advanced-analytics-Channel-Shopper .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $("#ToShowShooperAndTrips .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $("#advanced-analytics-Retailer-Trips *").removeClass("Selected");
    //        $("#advanced-analytics-Retailer-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#advanced-analytics-Channel-Trips *").removeClass("Selected");
    //        $("#advanced-analytics-Channel-Trips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#advanced-analytics-Retailer-Shopper *").removeClass("Selected");
    //        $("#advanced-analytics-Retailer-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#advanced-analytics-Channel-Shopper *").removeClass("Selected");
    //        $("#advanced-analytics-Channel-Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#ToShowShooperAndTrips *").removeClass("Selected");
    //        $("#ToShowShooperAndTrips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#Search-Left-Advanced-AnalyticsFilters").val(ui.item.label);
    //        var CompObj = $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 span[uniquefilterid='" + ui.item.value + "']");
    //        AdvancedAnalyticsShopperTripSelection(CompObj.parent().attr("type").toLowerCase());
    //        if (SelectedAdvancedAnalyticsList.length > 0) {
    //            if (SelectedAdvancedAnalyticsList[0].Type != CompObj.attr("Type")) {
    //                SelectedAdvancedAnalyticsList = [];
    //                sVisitsOrGuests = CompObj.attr("Type");
    //                if (CompObj.attr("Type") == "1")
    //                    TabType = "trips";
    //                else
    //                    TabType = "shopper";
    //                CompObj.parent().eq(0).click();
    //            }
    //            else {
    //                if (SelectedAdvancedAnalyticsList[0].FullName.trim() != CompObj.attr("parent").trim()) {
    //                    SelectedAdvancedAnalyticsList = [];
    //                    CompObj.parent().eq(0).click();
    //                }
    //                else
    //                    CompObj.parent().eq(0).click();
    //            }
    //        }
    //        else {
    //            sVisitsOrGuests = CompObj.attr("Type");
    //            if (CompObj.attr("Type") == "1")
    //                TabType = "trips";
    //            else
    //                TabType = "shopper";
    //            CompObj.parent().eq(0).click();
    //        }

    //        if (TabType == "trips") {
    //            if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() != "total visits") {
    //                SelectedFrequencyList = [];
    //                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
    //                $("#Advanced-Analytics-Select-Variable").trigger("click");
    //            }
    //            else {
    //                SelectedFrequencyList = [];
    //                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
    //                $("#Advanced-Analytics-Select-Variable").trigger("click");
    //            }
    //        }
    //        else if (TabType == "shopper") {
    //            if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name != "Monthly +") {
    //                SelectedFrequencyList = [];
    //                $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
    //                $("#Advanced-Analytics-Select-Variable").trigger("click");
    //            }
    //            else {
    //                SelectedFrequencyList = [];
    //                $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
    //                $("#Advanced-Analytics-Select-Variable").trigger("click");
    //            }
    //        }

    //    }
    //    else if (SearchFor == "TotalMeasure") {
    //        $(".TotalMeasure .Shopper").hide();
    //        $("#TotalMeasureHeaderContentTrip").hide();
    //        $("#TotalMeasureHeadingLevel3").hide();
    //        $(".TotalMeasure .trip").show();
    //        $("#TotalMeasureHeadingLevel2").show();
    //        $("#TotalMeasureHeadingLevel3").hide();
    //        $("#TotalMeasureShopperTripHeader *").removeClass("Selected");
    //        $("#TotalMeasureShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $("#TotalMeasureShopperTripHeader .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $(".TotalMeasure .Shopper *").removeClass("Selected");
    //        $(".TotalMeasure .Shopper *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $(".TotalMeasure .Shopper .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        $(".TotalMeasure .trip *").removeClass("Selected");
    //        $(".TotalMeasure .trip *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //        $(".TotalMeasure .trip .sidearrw_OnCLick").each(function (i, j) {
    //            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    //        });
    //        var CompObj = $(".TotalMeasure span[uniqueid='" + ui.item.value + "']").eq(0);
    //        TotalTripShopperSelection(CompObj.attr("type").toLowerCase().replace("trip", "trips"));
    //        if (SelectedTotalMeasure.length <= 0) {
    //            //sVisitsOrGuests = CompObj.attr("Type");
    //            if (CompObj.attr("Type").toLowerCase().replace("trip", "trips") == "trips")
    //                TabType = "trips";
    //            else
    //                TabType = "shopper";
    //            CompObj.parent().eq(0).click();
    //        }
    //        else {
    //            if (SelectedTotalMeasure[0].type.toLowerCase().replace("trip", "trips") == CompObj.attr("Type").toLowerCase().replace("trip", "trips")) {
    //                if (SelectedTotalMeasure[0].parent == CompObj.attr("parent"))
    //                    CompObj.parent().eq(0).click();
    //                else {
    //                    SelectedTotalMeasure = [];
    //                    CompObj.parent().eq(0).click();
    //                }
    //            }
    //            else {
    //                SelectedTotalMeasure = [];
    //                if (CompObj.attr("Type").toLowerCase().replace("trip", "trips") == "trips")
    //                    TabType = "trips";
    //                else
    //                    TabType = "shopper";
    //                CompObj.parent().eq(0).click();

    //            }
    //        }

    //        if (CompObj.attr("type").toLowerCase().replace("trip", "trips") == "trips") {
    //            sVisitsOrGuests = 1;
    //            sBevarageSelctionType = [];
    //            TabType = "trips";
    //            $("#Left-Frequency").hide();
    //            SelectedFrequencyList = [];
    //        }
    //        else {
    //            sVisitsOrGuests = 2;
    //            sBevarageSelctionType = [];
    //            TabType = "shopper";
    //            $("#left-panel-frequency ul div[name='STORE IN TRADE AREA']").trigger("click");
    //            $("#Left-Frequency").show();
    //        }
    //        HideOrShowFilters();
    //    }
    //    ShowSelectedFilters();
    //    HideOrShowFilters();
    //    e.stopImmediatePropagation();
    //});
}
//End

//demo adv filter Images Position
function GetDemographyImagePosition() {
    var DemographyImageDetails = [
        { DemographyName: "Gender", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "7px -297px;" },
        { DemographyName: "Age", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-47px -297px;" },
        { DemographyName: "Detailed Age", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-97px -297px;" },
        { DemographyName: "Age/Gender", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-203px -297px;" },
        { DemographyName: "Age/Ethnicity", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-309px -297px;" },
       // { DemographyName: "Ethnicity", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-309px -297px;" },//Ethnicity
        { DemographyName: "Gender/Ethnicity", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-367px -297px;" },
        { DemographyName: "Race Ethinicity", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-253px -297px;" },
        { DemographyName: "HH Total", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-422px -297px;" },
        { DemographyName: "HH Children", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-529px -297px;" },
        { DemographyName: "HH Adults", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-477px -297px;" },
        { DemographyName: "HCM", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-580px -297px;" },//HH Size
        { DemographyName: "Parental Identification", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-631px -297px;" },
        { DemographyName: "Employment Status", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-785px -297px;" },
        { DemographyName: "Employment Status Detailed", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-840px -297px;" },
        { DemographyName: "HH Income", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-732px -297px;" },
        { DemographyName: "Education", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-894px -297px;" },
        { DemographyName: "Socioeconomic", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-894px -297px;" },
        { DemographyName: "Education", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-894px -297px;" },
        { DemographyName: "Geography", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1055px  -297px;" },
        { DemographyName: "Hispanic_Acculturation", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1055px  -297px;" },//Attitudinal Segment//Attitudinal Statements - Top 2 Box//% HH Shopping Personally Responsible For
        { DemographyName: "Marital Status", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-680px -297px;" },
        /*Demographic Filter*/
    ];
    return DemographyImageDetails;
}
function GetBeverageImagePosition() {
    var BeverageImageDetails = [
        { BeverageName: "Regular (Non-Diet) Carbonated Soft Drink", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "7px -297px;" },
        { BeverageName: "RTD Coffee", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-72px -147px;" },
        { BeverageName: "RTD Tea", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-112px -147px;" },
        { BeverageName: "Protein Drinks", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-943px -147px;" },
        { BeverageName: "Packaged 100% Orange Juice", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1156px -148px;" },
        { BeverageName: "Sparkling Water (Unflavored)", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1614px -149px;" },
        { BeverageName: "Flavored Sparkling Water", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1688px -149px;" },
        { BeverageName: "Category Nets", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "10px -470px;" },
        { BeverageName: "Detailed Categories", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-36px -470px;" },
        { BeverageName: "SSD Regular", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "8px -149px;" },
        { BeverageName: "SSD Diet", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-30px -149px;" },
        { BeverageName: "RTD Smoothies in a Bottle", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1026px -149px;" },
        { BeverageName: "Packaged 100% Fruit Juice (NON-OJ)", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1240px -147px;" },
        { BeverageName: "Packaged 100% Grape Juice", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-647px -678px;" },
        { BeverageName: "Packaged 100% Apple Juice", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-694px -678px;" },
        { BeverageName: "Packaged 100% Grapefruit Juice", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-740px -678px;" },
        { BeverageName: "Packaged 100% Cranberry Juice", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-792px -678px;" },
        { BeverageName: "Packaged 100% Fruit Juice Blends", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-840px -678px;" },
        { BeverageName: "Vegetable Juice/ Vegetable + Juice Blend", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-92px -471px;" },
        { BeverageName: "Other Flavor 100% Juice", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-885px -678px;" },
        { BeverageName: "Sports Drinks", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1751px -360px;" },
        { BeverageName: "Energy Drink/ Shot", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1791px -360px;" },
        { BeverageName: "Liquid Flavor Enhancer", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-619px -570px;" },
        { BeverageName: "Enhanced Milk", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-90px -678px;" },
        { BeverageName: "Non-Sparkling Water - Nets", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-568px -678px;" },
        { BeverageName: "Sparkling Water - Nets", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-525px -681px;" },
        { BeverageName: "Bottled Water - Nets", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-606px -678px;" },
        { BeverageName: "SSD Regular/Diet", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-485px -681px;" },
        { BeverageName: "RTD Juice Drink", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1493px -147px;" },
        { BeverageName: "Single Serving Bottled Water (Non-Sparkling Water)", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-135px -470px;" },
        { BeverageName: "Flavored Non-Sparkling Bottled Water", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-174px -470px;" },
         { BeverageName: "Juice/Juice Drinks/Vege/Smoothies - Nets", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-195px -677px;" },
         { BeverageName: "Other Trademark/Brand Groups", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-141px -678px;" },
         { BeverageName: "Hard Seltzers and Other Flavored Alcoholic Beverages", imageName: "../../Images/hard_seltzers_icons.svg?id=2", imagePosition: "0px 0px;" },
        //Retailers

        { BeverageName: "Total", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-932px -678px;" },
        { BeverageName: "Supermarket/Grocery", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1414px -360px;" },
        { BeverageName: "Convenience", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1456px -360px;" },
        { BeverageName: "Drug", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1646px -360px;" },
        { BeverageName: "Dollar", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1603px -360px;" },
        { BeverageName: "Club", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1545px -360px;" },//-320px -48px;
        { BeverageName: "Mass Merc.", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1498px -360px;" },
        { BeverageName: "Supercenter", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-1692px -360px;" },
        { BeverageName: "Corporate Nets", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "4px -678px;" },


        /*Bev Retailer Filter*/
    ];
    return BeverageImageDetails;
}
function ChartImagePosition(chartName) {
    var imagePosition = "";
    switch (chartName) {
        case "Stacked Column": { imagePosition = "-921px -405px"; break; }
        case "Stacked Bar": { imagePosition = "-1013px -405px"; break; }
        case "Table": { imagePosition = "-237px -405px"; break; }
        case "Clustered Column": { imagePosition = "-43px -407px"; break; }
        case "Clustered Bar": { imagePosition = "-142px -405px"; break; }
        case "Bar with Change": { imagePosition = "-835px -402px"; break; }
        case "Pyramid": { imagePosition = "-441px -406px"; break; }
        case "Pyramid with Change": { imagePosition = "-1316px -406px"; break; }
        case "Line": { imagePosition = "-335px -405px"; break; }
        case "Stacked Area": { imagePosition = "-331px -407px"; break; }
    }
    return imagePosition;
}
function ChartImageNew_Position(chartName) {
    var imagePosition = "";
    switch (chartName) {
        case "Stacked Column": { imagePosition = "-879px -405px"; break; }
        case "Stacked Bar": { imagePosition = "-969px -405px"; break; }
        case "Table": { imagePosition = "-189px -405px"; break; }
        case "Clustered Column": { imagePosition = "6px -407px"; break; }
        case "Clustered Bar": { imagePosition = "-91px -407px"; break; }
        case "Bar with Change": { imagePosition = "-789px -405px"; break; }
        case "Pyramid": { imagePosition = "-387px -406px"; break; }
        case "Pyramid with Change": { imagePosition = "-1260px -406px"; break; }
        case "Line": { imagePosition = "-287px -405px"; break; }
        case "Stacked Area": { imagePosition = "-275px -407px"; break; }
    }
    return imagePosition;
}
function GetMouseHoverPopUpDetails() {
    var MousehoverDetails = [
         { Name: "Dashboard", cls: "classDashboard", size: "big", Text: "Navigate to a visual dashboard that provides an interactive overview of key metrics" },
        { Name: "Reports", cls: "classReport", size: "big", Text: "Navigate to pre-defined reports" },
        { Name: "Tables", cls: "classTable", size: "big", Text: "Navigate to data tables of key metrics" },
        { Name: "Charts", cls: "classChart", size: "big", Text: "Navigate to charts of key metrics" },
        { Name: "ADD’L CAPABILITIES", cls: "classAnalysis", size: "big", Text: "Navigate to additional scorecards and analysis frameworks" },
        { Name: "Retailer", cls: "Retailers-Table", size: "big", Text: "Navigate to data tables for key metrics by retailer/channel" },
        { Name: "Beverage", cls: "Beverage-Table", size: "big", Text: "Navigate to data tables for key metrics by beverage" },
        { Name: "Ecommerce Sites", cls: "ecom-Table", size: "big", Text: "Navigate to data tables for key metrics for ecommerce" },
        { Name: "Compare Beverages", cls: "CompareBeverage-Table", size: "big", Text: "Create data tables that compare across brands/categories" },
        { Name: "Beverage Deep Dive", cls: "BeverageDeepDive-Table", size: "big", Text: "Create data tables comparing demographics, key metrics or time periods within a single brand/category" },
        { Name: "Compare Retailers", cls: "CompareRetailer-Table", size: "big", Text: "Create data tables that compare across retailers/channels" },
        { Name: "Retailer Deep Dive", cls: "RetailerDeepDive-Table", size: "big", Text: "Create data tables comparing demographics, key metrics or time periods within a single retailers/channels" },
        { Name: "Compare ecommerce Sites", cls: "Compareecom-Table", size: "big", Text: "Create data tables that compare across ecommerce sites" },
        { Name: "Ecommerce Site Deep Dive", cls: "ecomdeepdive-Table", size: "big", Text: "Create data tables comparing demographics, key metrics or time periods within a single ecommerce site" },
        { Name: "Retailer", cls: "Retailers-Chart", size: "big", Text: "Navigate to charts for key metrics by retailer/channel" },
        { Name: "Beverage", cls: "Beverage-Chart", size: "big", Text: "Navigate to charts for key metrics by beverage" },
        { Name: "Ecommerce Sites", cls: "ecom-Chart", size: "big", Text: "Navigate to charts for key metrics for ecommerce" },
        { Name: "Compare Retailers", cls: "CompareRetailer-Chart", size: "big", Text: "Create charts that compare across retailers/channels" },
        { Name: "Retailer Deep Dive", cls: "RetailerDeepDive-Chart", size: "big", Text: "Create charts comparing demographics, key metrics or time periods within a single retailer/channel" },
        { Name: "Compare Beverages", cls: "CompareBeverage-Chart", size: "big", Text: "Create charts that compare across brands/categories" },
        { Name: "Beverage Deep Dive", cls: "BeverageDeepDive-Chart", size: "big", Text: "Create charts comparing demographics, key metrics or time periods within a single brand/category" },
        { Name: "Compare ecommerce Sites", cls: "CompareSites-Chart", size: "big", Text: "Create charts that compare across ecommerce sites" },
        { Name: "Ecommerce Site Deep Dive", cls: "sitesDeepDive-Chart", size: "big", Text: "Create charts comparing demographics, key metrics or time periods within a single ecommerce site" },
        { Name: "Retailer Overview Reports", cls: "RetailerOverview-Reports", size: "medium", Text: "Generate pre-defined retailer/channel overview PowerPoint reports" },
        { Name: "Beverage/Category Overview Reports", cls: "BeverageOverview-Reports", size: "medium", Text: "Generate pre-defined beverage/category overview PowerPoint reports" },
        { Name: "Total Respondents/Total Trips Report", cls: "TotalOverview-Reports", size: "medium", Text: "Create data tables comparing all retailers for a single key metric" },
        { Name: "Briefing Book – Situation Assessment", cls: "SarReport", size: "medium", Text: "Create data tables comparing all retailers for a single key metric" },
        { Name: "PIT, Trend Toggle", cls: "PitTrendToggle", size: "medium", Text: "Select to view as single point in time or trend " },
        { Name: "Toggle Button", cls: "PitTrendToggleDash", size: "medium", Text: "Select to view numbers based on highest percentage or largest skew " },

          { Name: "Toggle Button", cls: "PitTrendToggleDashTrip", size: "medium", Text: "Select to view Trips or Shopper based demographics" },

        { Name: "Time Period", cls: "TimePeriod", size: "medium", Text: "Select the time interval and end period " },
        { Name: "Retailers/Channels:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons", cls: "Retailers", size: "medium", Text: "Select the retailers/channels to compare:Select to determine the demographics or key metrics to be compared:Generate pre-defined retailer/channel overview PowerPoint reports:Create a Correspondence Map comparing retailers/channels across key metrics:Create a Correspondence Map of key metrics of a single retailer/channel:Select the retailers/channels to compare:Generate a table that shows a retailer/channel's shopper frequencies to all other priority retailers and channels:Genetrate a table that shows a retailer/channel shopper's perceptions of other priority retailers" },
        { Name: "Beverages - Compare Module:Beverages - Deep Dive Module:Product Selection", cls: "Beverages", size: "medium", Text: "Select the brands/categories to compare:Select a single brand/category for deep dive:Default products have been select,  click to select different products" },
       { Name: "Metric Comparisons", cls: "Group", size: "medium", Text: "Select the demographics or key metrics to compare" },
        { Name: "Advanced Filter", cls: "Demographics", size: "medium", Text: "Select additional demographic filters" },
         { Name: "Advance Filters", cls: "DashboardDemographics", size: "medium", Text: "Select additional demographic or key metric filters" },

          { Name: "Retailer/Channel", cls: "DashboardRetailers", size: "medium", Text: "Select Single Retailer/Channel" },

        { Name: "Overall Chart Type Selection Area", cls: "charttype", size: "medium", Text: "Select to display data in different chart types" },

        { Name: "Overall Chart Type Selection Area", cls: "charttype", size: "medium", Text: "Select to display data in different chart types" },
        { Name: "Correspondence Mapping", cls: "Correnspondence-Others", size: "medium", Text: "Navigate to a module that provides a Correspondence Map of key metrics" },
        { Name: "CBP Frameworks", cls: "CBP-Others", size: "medium", Text: "Generate typical channel planning frameworks" },
        //{ Name: "Select Variables", cls: "CorrespondanceMeasureVariable-Others", size: "medium", Text: "Select to determine the demographics or key metrics variables" },
        { Name: "Measures", cls: "CorrespondanceMeasureVariable-Others", size: "medium", Text: "SELECT MEASURES TO VIEW" },
        { Name: "Perceptual Map Icon", cls: "CorrespondancechartImage-Others", size: "medium", Text: "Select to view a Perceptual Map" },
        { Name: "Coordinate Table Icon", cls: "CorrespondanceCordinate-Others", size: "medium", Text: "Select to view the coordinates of the Correspondence Map" },
        { Name: "Tabular Output Icon", cls: "CorrespondanceTable-Others", size: "medium", Text: "Select to view the data in the Correspondence Map" },
        { Name: "Retailer Cross Shopping Analysis", cls: "RetailerCross-Others", size: "medium", Text: "Generate tables comparing shopper's cross-shopping frequency and perceptions" },
        { Name: "Shopper Metrics", cls: "bgm-Others", size: "medium", Text: "Generate a table of key business growth model metrics for a ratiler/channel for product categories" },
        { Name: "SOAP (Shopper On A Page)", cls: "soap-Others", size: "medium", Text: "Generate a table comparing key metrics by shopper type of a single retailer/channel" },
        { Name: "Annualized Projections", cls: "Annualised-Others", size: "medium", Text: "Annualized projection of trips along with decomposition of growth/decline by key metrics" },
        { Name: "Trip decomposition", cls: "EstablishmentDeepDive-Others", size: "medium", Text: "Annualized projection of trips along with decomposition of growth/decline by key metrics" },
        { Name: "Compare Retailers", cls: "Compareretailer-Others", size: "medium", Text: "Create a Correspondence Map comparing retailers/channels across key metrics" },
        { Name: "Retailers Deep Dive", cls: "retailerdeepdive-Others", size: "medium", Text: "Create a Correspondence Map of key metrics of a single retailer/channel" },
        { Name: "Cross Retailer Frequencies", cls: "frequencies-Others", size: "medium", Text: "Generate a table that shows a retailer/channel's shopper frequencies to all other priority retailers and channels" },
        { Name: "Cross Retailer Imageries", cls: "imageries-Others", size: "medium", Text: "Genetrate a table that shows a retailer/channel shopper's perceptions of other priority retailers" },
         { Name: "Frequency Selection", cls: "LeftFrequncy-Others", size: "medium", Text: "Default selection has been made, click to change shopping/purchase frequency" },
         { Name: "PPT export button", cls: "ppt-Export", size: "smalllight", Text: "Export to PowerPoint" },
         { Name: "PDF export button", cls: "pdf-Export", size: "smalllight", Text: "Export to PDF" },
         { Name: "Excel export button", cls: "excel-Export", size: "smalllight", Text: "Export to Excel" },
         { Name: "Home button", cls: "home-Export", size: "smalllight", Text: "Navigate to other Explorer Tools" },
         { Name: "Contact Support", cls: "help-Export", size: "smalllight", Text: "Contact support" },
         { Name: "Logout button", cls: "logout-Export", size: "smalllight", Text: "Logout of Explorer" },
         { Name: "Stat Setting", cls: "stat-Export", size: "smalllight", Text: "Click to change the stat testing level across the tool" },
         { Name: "Expand Button", cls: "breadcrumb-open", size: "smalldark", Text: "Click to see all selections made" },
         { Name: "Stat Testing", cls: "StattestingText", size: "smalldark", Text: "Select different stat testing options" },
         { Name: "Previous Year", cls: "Stat-prevyear", size: "smalldark", Text: "Stat test all data to same time period one year ago" },
         { Name: "Custom Base", cls: "Stat-custombase", size: "smalldark", Text: "Stat test data against any of the selections made" },
         { Name: "Previous Period", cls: "Stat-prevperiod", size: "smalldark", Text: "Stat test all data to the time period immediately proceeding" },
         { Name: "total time", cls: "Stat-totaltime", size: "smalldark", Text: "Stat test all data to the total time period of the selected range" },
         { Name: "base", cls: "Stat-base", size: "smalldark", Text: "Stat test all data to the first time period of the selected range" },
         { Name: "Shopper/Trips Toggle", cls: "shoppertrip-Toggle", size: "smalldark", Text: "Select to view shopper or trip based demographics" },
         { Name: "Zoom Out Button", cls: "zoomout-correspondence", size: "smalldark", Text: "Zoom out" },
         { Name: "Zoom In Button", cls: "zoomin-correspondence", size: "smalldark", Text: "Zoom in" },
         { Name: "Add to export icon", cls: "chart-Export", size: "smalldark", Text: "Click to add the current chart to your export list. Export list will allow you to export multuple charts at a time instead of one by one" },
         { Name: "Show Export List", cls: "viewchart-Export", size: "smalldark", Text: "Click to view your current export list" },
         { Name: "Clear All", cls: "clearchart-Export", size: "smalldark", Text: "Click to clear your current export list" },
         { Name: "Item Drop Down", cls: "BGM-Dropdown", size: "smalldark", Text: "Click to select different products" },

         { Name: "Where Purchased", cls: "WherePurchased-Reports", size: "medium", Text: "SELECT THE CHANNELS/RETAILERS WHERE BEVERAGE HAS BEEN PURCHASED" },
         { Name: "Shopping Sites", cls: "Sites-Ecom", size: "medium", Text: "SELECT THE E-COMMERCE SITES TO COMPARE" },
         { Name: "Measures", cls: "Measure-TotalRespondants", size: "medium", Text: "SELECT MEASURES TO VIEW" },
         { Name: "Measures", cls: "Measure-Charts", size: "medium", Text: "SELECT MEASURES TO VIEW" },
         { Name: "Geography", cls: "Geography-SOAP", size: "medium", Text: "SELECT GEOGRAPHY FILTERS" },

         //{ Name: "Shopper Comparison", cls: "CompareRetailers-RetailerOverview-Reports", size: "medium", Text: "Compare shopper metrics (such as frequency, demographics and attitudes) between channels and retailers. For example: - compare shoppers of Kroger, Publix and Total Grocery" },
         //{ Name: "Shopper Deep Dive", cls: "ShopperDeepDive-RetailerOverview-Reports", size: "medium", Text: "Compare shopper metrics (such as frequency, demographics and attitudes) between shopper segments in the Total market or within a specific channel or retailer. Change to trend and compare shopper metrics for Total US or a specific channel or retailer over time.For example:  - compare Kroger shoppers of different age groups - compare grocery shoppers across bottler regions - compare how Kroger's shoppers have changed over time" },
         //{ Name: "Path to Purchase Comparison", cls: "RetailersPathToPurchase-RetailerOverview-Reports", size: "medium", Text: "Compare paths to purchase (pre, in and post shop) between channels and retailers. For example: - compare the path to purchase for Kroger, Publix and Total Grocery" },
         //{ Name: "Path to Purchase Deep Dive", cls: "RetailerPurchaseDeepDive-RetailerOverview-Reports", size: "medium", Text: "Compare paths to purchase (pre, in and post shop) between different shopper segments in the Total market or within a specific channel or retailer. Change to trend and compare the path to purchase for Total US or a specific channel or retailer over time.For example:  - compare Kroger path to purchase for different age groups - compare the grocery path to purchase across bottler regions - compare how Kroger's path to purchase has changed over time" },
         //{ Name: "Monthly+ Purchaser Comparison", cls: "BeveragesMonthlyPlusPurchasers-RetailerOverview-Reports", size: "medium", Text: "Compare shopper metrics (such as demographics,  attitudes and general shopping behaviors) between monthly+ shoppers of different beverage categories and brands. For example: - compare monthly+ shoppers of Coke, Pepsi and Total SSD" },
         //{ Name: "Monthly+ Purchaser Deep Dive", cls: "BeveragesMonthlyPlusPurchasersDeepDive-RetailerOverview-Reports", size: "medium", Text: "Compare shopper metrics (such as demographics,  attitudes and general shopping behaviors) between shopper segments of a specific beverage category or brand. Change to trend and compare those metrics over time for a beverage category or brand. For example:  - compare Coke monthly+ shoppers of different age groups - compare SSD monthly+ shoppers by bottler region - compare how Coke monthly+ shoppers have changed over time" },
         //{ Name: "Beverage Purchase Comparison", cls: "BeveragesPurchaseDetails-RetailerOverview-Reports", size: "medium", Text: "Compare shopping dynamics (such as location in store, destination item and basket detail) between beverage category or brand purchases anywhere or within a specific channel or retailer. For example: - compare Coke vs Pepsi purchases - compare Coke vs Pepsi purchases in Kroger" },
         //{ Name: "Beverage Purchase Deep Dive", cls: "BeveragesPurchaseDetailsDeepDive-RetailerOverview-Reports", size: "medium", Text: "Compare shopping dynamics (such as location in store, destination item and basket detail) when different shopper segments purchase a specific beverage category or brand anywhere or within a specific channel or retailer. Change to trend and compare those metrics over time for a beverage category or brand purchased anywhere or within a specific channel or retailer. For example:  - compare Coke purchase dynamics by different age groups - compare SSD purchase dynamics in grocery by bottler region - compare how Coke purchase dynamics have changed over time in Kroger" },
         //{ Name: "Measures", cls: "BGM-Dropdown", size: "medium", Text: "SELECT MEASURES TO VIEW" },
    ];
    return MousehoverDetails;
}
function MouseHoverPopupshowHide(obj) {
    $("#MouseHoverBigDiv").hide();
    $("#MouseHoverSmallDiv").hide();
    $("#MouseHoverSmallerDiv").hide();
    $("#MouseHoverExtraSmallDiv").hide();
    if (obj.size == "big") {
        $("#MouseHoverBigDiv").show();
        var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingText").html();
        $("#MouseHoverBigDiv .ContainerClass .HeadingText").html("");
        $("#MouseHoverBigDiv .ContainerClass .HeadingText").html(obj.Name);
        $("#MouseHoverBigDiv .ContainerClass .HeadingText").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
        $("#MouseHoverBigDiv .ContainerClass .TextContainer").text(obj.Text);
    }
    else if (obj.size == "medium") {
        if (obj.cls == "Beverages") {
            if ((currentpage.indexOf("compare") > -1)) {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[0]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[0]);
            }
            else if (currentpage == "hdn-analysis-acrossshopper") {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[2]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[2]);
            }
            else {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[1]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[1]);
            }
        }
        else if (obj.cls == "Retailers") {
            //if (currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
            //    $("#MouseHoverSmallDiv").show();
            //    var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
            //    $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
            //    $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[0]);
            //    $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
            //    $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[0]);
            //}
            //else if (currentpage == "hdn-report-compareretailersshoppers" || currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-compareretailerspathtopurchase" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
            if (currentpage == "hdn-report-compareretailersshoppers" || currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-compareretailerspathtopurchase" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[2]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[2]);
            }
            else if (currentpage == "hdn-analysis-withinshopper") {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[3]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[3]);
            }
            else if (currentpage == "hdn-analysis-withintrips") {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[4]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[4]);
            }
            else if (currentpage == "hdn-chart-retailerdeepdive" || currentpage == "hdn-tbl-retailerdeepdive" || currentpage == "hdn-tbl-compareretailers" || currentpage == "hdn-chart-compareretailers") {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[5]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[5]);
            }
            else if (currentpage == "hdn-analysis-crossretailerfrequencies") {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[6]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[6]);
            }
            else if (currentpage == "hdn-analysis-crossretailerimageries") {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[7]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[7]);
            }
            else {
                $("#MouseHoverSmallDiv").show();
                var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name.split(':')[1]);
                $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
                $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text.split(':')[1]);
            }
        }
        else {
            $("#MouseHoverSmallDiv").show();
            var sString = $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html()
            $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html("");
            $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").html(obj.Name);
            $("#MouseHoverSmallDiv .ContainerClass .HeadingTextsmall").append("<div class=\"mouseOverImageTitleBottom\" style=\"margin-top:24px;\"></div>");
            $("#MouseHoverSmallDiv .ContainerClass .TextContainersmall").text(obj.Text);
        }
    }
    else if (obj.size == "smalldark") {
        $("#MouseHoverSmallerDiv").show();
        var sString = $("#MouseHoverSmallerDiv .mousehoversmallpopuptext").html()
        $("#MouseHoverSmallerDiv .mousehoversmallpopuptext").text("");
        $("#MouseHoverSmallerDiv .mousehoversmallpopuptext").text(obj.Text);

        if ($(obj).eq(0).attr("cls") == "chart-Export") {
            $("#MouseHoverSmallerDiv").css("min-height", "95px");
            $("#MouseHoverSmallerDiv .centerdivdark").height("93%");
        }
        else {
            $("#MouseHoverSmallerDiv").css("min-height", "58px");
            $("#MouseHoverSmallerDiv .centerdivdark").height("84%");
        }


        $(".dynpos").hover(function (e) {
            var pos = $(this).position();
            var width = $(this).outerWidth();
            var height = $(this).outerHeight();
            var widthdiv = $("#MouseHoverExtraSmallDiv").outerWidth();
            //show the menu directly over the placeholder
            $("#MouseHoverExtraSmallDiv").css({
                position: "absolute",
                top: (pos.top + height) + "px",
                left: (pos.left - widthdiv + 22) + "px",
            }).show();

        }, function () {
            // Hover out code
            $('#MouseHoverExtraSmallDiv').hide();
        });
        $(".dynpos1").hover(function (e) {
            var pos = $(this).offset();
            var width = $(this).outerWidth();
            var height = $(this).outerHeight();
            var widthdiv = $("#MouseHoverSmallerDiv").outerWidth();

            var pageWidth = $(window).width();
            var pageHeight = $(window).width();
            var elementWidth = $("#MouseHoverSmallerDiv").outerWidth();
            var elementLeft = $(this).offset().left;
            var elementHeight = $("#MouseHoverSmallerDiv").outerHeight();
            var elementTop = $(this).offset().top;
            if ($(this).hasClass("up")) {
                $("#MouseHoverSmallerDiv").css({
                    position: "absolute",
                    top: (pos.top - height - 50) + "px",
                    left: (pos.left + 5) + "px",
                }).show();
            } else {
                if ($(this).hasClass("breadcrumb-open")) {
                    $("#MouseHoverSmallerDiv").css({
                        position: "absolute",
                        top: (pos.top + height + 20) + "px",
                        left: (pos.left - widthdiv - 10) + "px",
                    }).show();
                }
                else if ($(this).attr("id") == "btnClearAll") {
                    $("#MouseHoverSmallerDiv").css({
                        position: "absolute",
                        top: (pos.top + height - 6) + "px",
                        left: (pos.left - widthdiv + $(this).width() + 15) + "px",
                    }).show();
                }
                else {
                    $("#MouseHoverSmallerDiv").css({
                        position: "absolute",
                        top: (pos.top + height - 6) + "px",
                        left: (pos.left) + "px",
                    }).show();
                }
            }
            //show the menu directly over the placeholder


        }, function () {
            // Hover out code
            $('#MouseHoverSmallerDiv').hide();
        });

        //if ($(obj).attr("cls") == "breadcrumb-open") {
        //    if (leftpositionval == "0") {
        //        leftpositionval = "1";
        //        $(".logout-Export").mouseover();
        //        $(".breadcrumb-open").mouseover();
        //    }
        //}
    }
    else if (obj.size == "smalllight") {
        $("#MouseHoverExtraSmallDiv").show();
        var sString = $("#MouseHoverExtraSmallDiv .mousehoversmallpopuptext").html()
        $("#MouseHoverExtraSmallDiv .mousehoversmallpopuptext").text("");
        $("#MouseHoverExtraSmallDiv .mousehoversmallpopuptext").text(obj.Text);
    }
}
//End

//Common Time Period Function
function SelectTimePeriodNew(Name, Maximum, obj) {
    if (currentpage == "hdn-analysis-acrosshopper") {
        month = ['Aug 2013', 'Sep 2013', 'Oct 2013', 'Nov 2013', 'Dec 2013', 'Jan 2014', 'Feb 2014', 'Mar 2014', 'Apr 2014', 'May 2014', 'Jun 2014', 'Jul 2014', 'Aug 2014', 'Sep 2014', 'Oct 2014', 'Nov 2014', 'Dec 2014', 'Jan 2015', 'Feb 2015', 'Mar 2015', 'Apr 2015', 'May 2015', 'Jun 2015', 'Jul 2015', 'Aug 2015', 'Sep 2015', 'Oct 2015', 'Nov 2015', 'Dec 2015', 'Jan 2016', 'Feb 2016', 'Mar 2016', 'Apr 2016', 'May 2016', 'Jun 2016', 'Jul 2016', 'Aug 2016', 'Sep 2016'];
        quarter = ['Q4 2014', 'Q1 2015', 'Q2 2015', 'Q3 2015', 'Q4 2015', 'Q1 2016', 'Q2 2016', 'Q3 2016'];
        mmt = ['Oct 2014 3MMT', 'Nov 2014 3MMT', 'Dec 2014 3MMT', 'Jan 2015 3MMT', 'Feb 2015 3MMT', 'Mar 2015 3MMT', 'Apr 2015 3MMT', 'May 2015 3MMT', 'Jun 2015 3MMT', 'Jul 2015 3MMT', 'Aug 2015 3MMT', 'Sep 2015 3MMT', 'Oct 2015 3MMT', 'Nov 2015 3MMT', 'Dec 2015 3MMT', 'Jan 2016 3MMT', 'Feb 2016 3MMT', 'Mar 2016 3MMT', 'Apr 2016 3MMT', 'May 2016 3MMT', 'Jun 2016 3MMT', , 'Jul 2016 3MMT', 'Aug 2016 3MMT', 'Sep 2016 3MMT'];
        year = ['2015'];
        MMT_12 = ['Jul 2015 12MMT', 'Aug 2015 12MMT', 'Sep 2015 12MMT', 'Oct 2015 12MMT', 'Nov 2015 12MMT', 'Dec 2015 12MMT', 'Jan 2016 12MMT', 'Feb 2016 12MMT', 'Mar 2016 12MMT', 'Apr 2016 12MMT', 'May 2016 12MMT', 'Jun 2016 12MMT', 'Jul 2016 12MMT', 'Aug 2016 12MMT', 'Sep 2016 12MMT'];
        YTD = ['YTD Jan 2015', 'YTD Feb 2015', 'YTD Mar 2015', 'YTD Apr 2015', 'YTD May 2015', 'YTD Jun 2015', 'YTD Jul 2015', 'YTD Aug 2015', 'YTD Sep 2015', 'YTD Oct 2015', 'YTD Nov 2015', 'YTD Dec 2015', 'YTD Jan 2016', 'YTD Feb 2016', 'YTD Mar 2016', 'YTD Apr 2016', 'YTD May 2016', 'YTD Jun 2016', 'YTD Jul 2016', 'YTD Aug 2016', 'YTD Sep 2016'];
        MMT_6 = ['Jan 2015 6MMT', 'Feb 2015 6MMT', 'Mar 2015 6MMT', 'Apr 2015 6MMT', 'May 2015 6MMT', 'Jun 2015 6MMT', 'Jul 2015 6MMT', 'Aug 2015 6MMT', 'Sep 2015 6MMT', 'Oct 2015 6MMT', 'Nov 2015 6MMT', 'Dec 2015 6MMT', 'Jan 2016 6MMT', 'Feb 2016 6MMT', 'Mar 2016 6MMT', 'Apr 2016 6MMT', 'May 2016 6MMT', 'Jun 2016 6MMT', 'Jul 2016 6MMT', 'Aug 2016 6MMT', 'Sep 2016 6MMT']
    }
    else {
        month = ['Aug 2013', 'Sep 2013', 'Oct 2013', 'Nov 2013', 'Dec 2013', 'Jan 2014', 'Feb 2014', 'Mar 2014', 'Apr 2014', 'May 2014', 'Jun 2014', 'Jul 2014', 'Aug 2014', 'Sep 2014', 'Oct 2014', 'Nov 2014', 'Dec 2014', 'Jan 2015', 'Feb 2015', 'Mar 2015', 'Apr 2015', 'May 2015', 'Jun 2015', 'Jul 2015', 'Aug 2015', 'Sep 2015', 'Oct 2015', 'Nov 2015', 'Dec 2015', 'Jan 2016', 'Feb 2016', 'Mar 2016', 'Apr 2016', 'May 2016', 'Jun 2016', 'Jul 2016', 'Aug 2016', 'Sep 2016'];
        quarter = ['Q4 2013', 'Q1 2014', 'Q2 2014', 'Q3 2014', 'Q4 2014', 'Q1 2015', 'Q2 2015', 'Q3 2015', 'Q4 2015', 'Q1 2016', 'Q2 2016', 'Q3 2016'];
        mmt = ['Oct 2013 3MMT', 'Nov 2013 3MMT', 'Dec 2013 3MMT', 'Jan 2014 3MMT', 'Feb 2014 3MMT', 'Mar 2014 3MMT', 'Apr 2014 3MMT', 'May 2014 3MMT', 'Jun 2014 3MMT', 'Jul 2014 3MMT', 'Aug 2014 3MMT', 'Sep 2014 3MMT', 'Oct 2014 3MMT', 'Nov 2014 3MMT', 'Dec 2014 3MMT', 'Jan 2015 3MMT', 'Feb 2015 3MMT', 'Mar 2015 3MMT', 'Apr 2015 3MMT', 'May 2015 3MMT', 'Jun 2015 3MMT', 'Jul 2015 3MMT', 'Aug 2015 3MMT', 'Sep 2015 3MMT', 'Oct 2015 3MMT', 'Nov 2015 3MMT', 'Dec 2015 3MMT', 'Jan 2016 3MMT', 'Feb 2016 3MMT', 'Mar 2016 3MMT', 'Apr 2016 3MMT', 'May 2016 3MMT', 'Jun 2016 3MMT', 'Jul 2016 3MMT', 'Aug 2016 3MMT', 'Sep 2016 3MMT'];
        year = ['2014', '2015'];
        MMT_12 = ['Jul 2014 12MMT', 'Aug 2014 12MMT', 'Sep 2014 12MMT', 'Oct 2014 12MMT', 'Nov 2014 12MMT', 'Dec 2014 12MMT', 'Jan 2015 12MMT', 'Feb 2015 12MMT', 'Mar 2015 12MMT', 'Apr 2015 12MMT', 'May 2015 12MMT', 'Jun 2015 12MMT', 'Jul 2015 12MMT', 'Aug 2015 12MMT', 'Sep 2015 12MMT', 'Oct 2015 12MMT', 'Nov 2015 12MMT', 'Dec 2015 12MMT', 'Jan 2016 12MMT', 'Feb 2016 12MMT', 'Mar 2016 12MMT', 'Apr 2016 12MMT', 'May 2016 12MMT', 'Jun 2016 12MMT', 'Jul 2016 12MMT', 'Aug 2016 12MMT', 'Sep 2016 12MMT'];
        YTD = ['YTD Jan 2014', 'YTD Feb 2014', 'YTD Mar 2014', 'YTD Apr 2014', 'YTD May 2014', 'YTD Jun 2014', 'YTD Jul 2014', 'YTD Aug 2014', 'YTD Sep 2014', 'YTD Oct 2014', 'YTD Nov 2014', 'YTD Dec 2014', 'YTD Jan 2015', 'YTD Feb 2015', 'YTD Mar 2015', 'YTD Apr 2015', 'YTD May 2015', 'YTD Jun 2015', 'YTD Jul 2015', 'YTD Aug 2015', 'YTD Sep 2015', 'YTD Oct 2015', 'YTD Nov 2015', 'YTD Dec 2015', 'YTD Jan 2016', 'YTD Feb 2016', 'YTD Mar 2016', 'YTD Apr 2016', 'YTD May 2016', 'YTD Jun 2016', 'YTD Jul 2016', 'YTD Aug 2016', 'YTD Sep 2016'];
        MMT_6 = ['Jan 2014 6MMT', 'Feb 2014 6MMT', 'Mar 2014 6MMT', 'Apr 2014 6MMT', 'May 2014 6MMT', 'Jun 2014 6MMT', 'Jul 2014 6MMT', 'Aug 2014 6MMT', 'Sep 2014 6MMT', 'Oct 2014 6MMT', 'Nov 2014 6MMT', 'Dec 2014 6MMT', 'Jan 2015 6MMT', 'Feb 2015 6MMT', 'Mar 2015 6MMT', 'Apr 2015 6MMT', 'May 2015 6MMT', 'Jun 2015 6MMT', 'Jul 2015 6MMT', 'Aug 2015 6MMT', 'Sep 2015 6MMT', 'Oct 2015 6MMT', 'Nov 2015 6MMT', 'Dec 2015 6MMT', 'Jan 2016 6MMT', 'Feb 2016 6MMT', 'Mar 2016 6MMT', 'Apr 2016 6MMT', 'May 2016 6MMT', 'Jun 2016 6MMT', 'Jul 2016 6MMT', 'Aug 2016 6MMT', 'Sep 2016 6MMT'];
    }


    if (TimeExtension == "Total") {
        TimePeriod = "total|total";
        var timeperiodarray = [];
        timeperiodarray = $(".timeType").val().split(" ");
        PreviousTimePeriod = "";
        for (var i = 0; i < timeperiodarray.length; i++) {
            if ($.isNumeric(timeperiodarray[i])) {
                timeperiodarray[i] = timeperiodarray[i] - 1;
            }
        }
        PreviousTimePeriod = timeperiodarray.join(" ");
    }
    else {
        sCurrent_PreviousTime = "";
        var timeperiodarray = [];
        timeperiodarray = $(".timeType").val().split(" ");
        PreviousTimePeriod = "";
        for (var i = 0; i < timeperiodarray.length; i++) {
            if ($.isNumeric(timeperiodarray[i])) {
                timeperiodarray[i] = timeperiodarray[i] - 1;
            }
        }
        PreviousTimePeriod = timeperiodarray.join(" ");
        if (TimePeriod.split('|')[0].toString().toUpperCase() == "3MMT" || TimePeriod.split('|')[0].toString().toUpperCase() == "6MMT" || TimePeriod.split('|')[0].toString().toUpperCase() == "12MMT") {
            if (TimePeriod.split('|').length > 1)
                sCurrent_PreviousTime = TimePeriod.split('|')[1].toString().toUpperCase() + " " + TimePeriod.split('|')[0].toString().toUpperCase() + " V/S " + PreviousTimePeriod.toString().toUpperCase();
        }
        else {
            if (TimePeriod.split('|').length > 1)
                sCurrent_PreviousTime = TimePeriod.split('|')[1].toString().toUpperCase() + " V/S " + PreviousTimePeriod.toString().toUpperCase();
        }
    }

}
Array.prototype.getUnique = function () {
    var u = {}, a = [];
    for (var i = 0, l = this.length; i < l; ++i) {
        if (u.hasOwnProperty(this[i])) {
            continue;
        }
        a.push(this[i]);
        u[this[i]] = 1;
    }
    return a;
}

//function GoHome()
//{
//    window.app.db.delete(1, function () {
//        window.location.href = $("#URLHome").val();
//    });
//    //sessionStorage.clear();
//    //window.location.href = $("#URLHome").val();
//}
//function SignOut() {
//    window.app.db.delete(1, function () {
//        window.location.href = $("#URLSignOut").val();
//    });
//    //sessionStorage.clear();
//    //window.location.href = $("#URLSignOut").val();
//}

function TempHideFn() {
    $(".StatArea").hide();
}
//----------Validation Block---------------->
//----> Compare Retailer -------->
function Validate_CompareRetailers() {
    if (Comparisonlist.length == 0 || Comparisonlist.length < 2) {
        showMessage("Please select minimum 2 Retailers");
        return false;
    }
    return true;
}
//----> Compare Beverages -------->
function Validate_CompareBeverages() {
    if (ComparisonBevlist.length == 0 || ComparisonBevlist.length < 2) {
        showMessage("Please select minimum 2 Beverages");
        return false;
    }
    return true;
}
//----> Group-------->
function Validate_Group() {
    if (Grouplist.length == 0 || Grouplist.length < 2) {
        showMessage("Please select minimum 2 Groups");
        return false;
    }
    return true;
}
function Validate_SOAPMinGroup() {
    if (Grouplist.length == 0) {
        showMessage("Please select Group");
        return false;
    }
    return true;
}

function Validate_CompareRetailers_Charts() {
    if (Comparisonlist.length == 0 || Comparisonlist.length < 1) {
        showMessage("Please select Retailer");
        return false;
    }
    return true;
}

function Validate_CompareRetailers_AdvanceAnalysis() {
    if (Comparisonlist.length == 0 || Comparisonlist.length < 3) {
        showMessage("Please select  minimum 3 Retailers");
        return false;
    }
    return true;
}

function Validate_CompareBeverages_Charts() {
    if (ComparisonBevlist.length == 0 || ComparisonBevlist.length < 1) {
        showMessage("Please select Beverage");
        return false;
    }
    return true;
}

function Validate_Group_AdvanceAnalysis() {
    if (Grouplist.length == 0 || Grouplist.length < 3) {
        showMessage("Please select  minimum 3 Groups");
        return false;
    }
    return true;
}

function Validate_Group_Charts() {
    if (Grouplist.length == 0 || Grouplist.length < 2) {
        showMessage("Please select  minimum 2 Groups");
        return false;
    }
    return true;
}


function Validate_Measures_AdvanceAnlysis() {
    if (SelectedAdvancedAnalyticsList.length == 0 || SelectedAdvancedAnalyticsList.length < 3) {
        showMessage("Please select  minimum 3 Measures");
        return false;
    }
    return true;
}

function Validate_Measures_Charts() {
    if (Measurelist.length == 0) {
        showMessage("Please select Measure");
        return false;
    }
    else if (Measurelist[0].metriclist.length == 0) {
        showMessage("Please select Measure");
        return false;
    }
    //else if (Measurelist[0].metriclist.length >10) {
    //    showMessage("Please select less than 10 Measure");
    //    return false;
    //}
    return true;
}
function Validate_DeepDive_Retailer_ShopperSegment() {
    if (Comparisonlist.length == 0) {
        showMessage("Please select Retailer");
        return false;
    }
    return true;
}
function Validate_DeepDive_Beverage_ShopperSegment() {
    if (ComparisonBevlist.length == 0) {
        showMessage("Please select Beverage");
        return false;
    }
    return true;
}
function Validate_Common_Groups() {
    if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        if (Grouplist.length == 13 && currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
            showMessage("YOU CAN MAKE UPTO 13 SELECTIONS");
            return false;
        }
    }
    else if (Grouplist.length == 11 && currentpage != "hdn-analysis-withintrips" && currentpage != "hdn-dashboard-pathtopurchase") {
        showMessage("YOU CAN MAKE UPTO 11 SELECTIONS");
        return false;
    }
    return true;
}
function Validate_Trend() {
    if (TimePeriod_ShortNames.length > 25) {
        showMessage("YOU CAN SELECT UPTO 25 TIME PERIODS");
        return false;
    }
    return true;
}
function DisplayHeightDynamicCalculation(filterName) {
    switch (filterName) {
        case "beverage": {
            var sHeight = 100 - ((($(".Beverages .Search-Filter").height()) / ($(".Beverages").height() + 9) * 100) + (($(".Beverages .AdvancedFiltersDemoHeading").height() + 1) / ($(".Beverages").height() + 9) * 100)) - 3;
            //$("#BevDivId").css("height", sHeight + "%");
        }
        case "retailer": {
            var sHeight = 100 - ((($(".Retailers .Search-Filter").height()) / ($(".Retailers").height() + 9) * 100) + (($(".Retailers .AdvancedFiltersDemoHeading").height() + 1) / ($(".Retailers").height() + 9) * 100)) - 4;
            //$("#RetailerDivId").css("height", sHeight + "%");
        }
        case "group": {
            var sHeight = 100 - ((($(".GroupType .Search-Filter").height()) / ($(".GroupType").height() + 9) * 100) + (($(".GroupType .AdvancedFiltersDemoHeading").height() + 1) / ($(".GroupType").height() + 9) * 100)) - 4;
            $("#groupDivId").css("height", sHeight + "%");
        }
        case "Measure": {
            if ($(".MeasureType .AdvancedFiltersDemoHeading").height() > 30)
                var sHeight = 100 - ((($(".MeasureType .Search-Filter").height()) / ($(".MeasureType").height() + 9) * 100) + (($(".MeasureType .AdvancedFiltersDemoHeading").height() + 1) / ($(".MeasureType").height() + 9) * 100)) - 2;
            else
                var sHeight = 100 - ((($(".MeasureType .Search-Filter").height()) / ($(".MeasureType").height() + 9) * 100) + (($(".MeasureType .AdvancedFiltersDemoHeading").height() + 1) / ($(".MeasureType").height() + 9) * 100)) - 3;

            $(".FilterContent").css("height", sHeight + "%");
        }
        case "AdvancedFilter": {
            var sHeight = 100 - ((($(".AdvancedFilters .Search-Filter").height()) / ($(".AdvancedFilters").height() + 9) * 100) + (($(".AdvancedFilters .AdvancedFiltersDemoHeading").height() + 1) / ($(".AdvancedFilters").height() + 9) * 100)) - 4;
            $("#AdvFilterDivId").css("height", sHeight + "%");
        }
        case "VisistsFilter": {
            if (($(".rgt-cntrl-SubFilter-Conatianer .AdvancedFiltersDemoHeading").height() + $(".rgt-cntrl-SubFilter-Conatianer .Search-Filter").height()) < 45)
                var sHeight = 100 - ((($(".rgt-cntrl-SubFilter-Conatianer .Search-Filter").height()) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100) + (($(".rgt-cntrl-SubFilter-Conatianer .AdvancedFiltersDemoHeading").height() + 1) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100)) - 10;
            else
                var sHeight = 100 - ((($(".rgt-cntrl-SubFilter-Conatianer .Search-Filter").height()) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100) + (($(".rgt-cntrl-SubFilter-Conatianer .AdvancedFiltersDemoHeading").height() + 1) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100)) - 8;
            $("#VisistsFilterDivId").css("height", sHeight + "%");
        }
        case "Frequency": {
            var sHeight = 100 - ((($(".rgt-cntrl-frequency .Search-Filter").height()) / ($(".rgt-cntrl-frequency").height() + 9) * 100) + (($(".rgt-cntrl-frequency .AdvancedFiltersDemoHeading").height() + 1) / ($(".rgt-cntrl-frequency").height() + 9) * 100)) - 14;
            $("#frequency_containerId").css("height", sHeight + "%");
        }
    }
}
//Low Sample Size Popup
function LowSampleProceed() {
    if ($(".LowSample-popup").is(":visible"))
        $(".TranslucentDiv").show();
    if (currentpage == "hdn-analysis-withintrips") {
        _.each(CorrespondaceMapsLowSampleSizeVariables.split('|'), function (obj) {
            var ObjData = $("#groupDivId span[uniqueid='" + obj.toString() + "']").parent();
            SelecGroupMetricName(ObjData);
        });
        if (Grouplist.length < 3)
            showMessage("Please select atleast Three Groups");
        else
            prepareContentArea();
    }
    else if (currentpage == "hdn-analysis-withinshopper") {
        _.each(CorrespondaceMapsLowSampleSizeVariables.split('|'), function (obj) {
            var searchObj = $("#RetailerDivId li[uniqueid='" + obj + "']");
            SelectComparison(searchObj);
        });
        if (Comparisonlist.length < 3)
            showMessage("Please select atleast Three Retailers");
        else
            prepareContentArea();
    }
    $(".LowSample-popup").hide();
    $("#Translucent").hide();
}
//End
function SetAnalysisItemActiveColor() {
    $("#ShowChart").css("background-position", "-544px -407px");
    $("#RTable").css("background-position", "-643px -407px");
    $("#DataTable ").css("background-position", "-740px -407px");
    switch (seltype) {
        case "Scatter Chart":
            {
                $(".zoomBtn").show();
                $("#ShowChart").css("background-position", "-492px -407px");
                break;
            }
        case "dimension Table":
            {
                $(".zoomBtn").hide();
                $("#RTable").css("background-position", "-592px -407px");
                break;
            }
        case "Contingency Table":
            {
                $(".zoomBtn").hide();
                $("#DataTable ").css("background-position", "-692px -407px");
                break;
            }
    }
}
function SetStyles() {
    ShowFrequencyHeader();
    $(".table-title").prev(".rowitem").children("ul").children("li").css("border", "0");
    var totalHeight = $("#Table-Content").height();
    var rightHeaderHeight = $(".rightheader").outerHeight() + 1;
    if (currentpage == "hdn-tbl-comparebeverages") {
        $(".leftbody").css("height", "calc(100% - " + (rightHeaderHeight - 1) + "px)");
        $(".rightbody").css("height", "calc(100% - " + (rightHeaderHeight - 1) + "px)");
    }

    else {
        $(".leftbody").css("height", "calc(100% - " + (rightHeaderHeight + 8) + "px)");
        $(".rightbody").css("height", "calc(100% - " + (rightHeaderHeight + 8) + "px)");
    }

    if (rightHeaderHeight != $(".leftheader").height())
        $(".leftheader").css("height", rightHeaderHeight - 0.5);
    var rowHeight = $(".rightheader .rowitem").height();
    if (rowHeight != $(".leftheader .rowitem").eq(0).height())
        $(".leftheader .rowitem").eq(0).css("height", rowHeight - 0.5);

    $(".leftheader").css("height", rightHeaderHeight - 1);
    $(".rightheader").css("height", rightHeaderHeight - 1);

    $(".leftbody .rowitem ul").each(function (i) {
        $(".rightbody .rowitem ul").eq(i).height($(this).height());
    })
    SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
}

function ShowFrequencyHeader() {
    $(".tbl-data-freqtxt").html("");
    if (currentpage.indexOf("beverage") > -1) {
        if (TabType == "shopper")
            $(".tbl-data-freqtxt").html("Monthly Purchasing Amount");
    }
    else {
        $(".tbl-data-freqtxt").html("Shopping Frequency");
    }
    //$("#guestFrqncy").show();
}

//function GetIShopFilters() {   
//    window.app.db.get(1, function (saveData) {      
//        if (saveData === null || saveData === undefined) {

//            var module = currentpage.split("-");
//            postBackData = "{modulename:'" + module[0] + "-" + module[1] + "'}";
//            jQuery.ajax({
//                type: "POST",
//                url: $("#URLCommon").val(),
//                async: true,
//                data: postBackData,
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    var saveData = {
//                        id: 1,
//                        value: JSON.stringify(data)
//                    };
//                    window.app.db.save(saveData, function () { });
//                },
//                error: function (xhr, status, error) {
//                }
//            });
//        }
//    });

//}
//Limits Text Updating Conditions
function FilterSelectionLimitText() {
    $(".RetailerSelectiontext").hide();
    $(".TimePeriodSelectionText").hide();
    $(".BeverageSelectionText").hide();
    $(".GroupSelectionText").hide();
    $(".MeasureSelectionText").hide();
    $(".RetailerSelectiontext").hide();
    $(".RetailerSelectiontext").hide();
    $(".SiteSelectionText").hide();

    if (currentpage == "hdn-tbl-compareretailers") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select from 2 to 11 Retailers");
    }
    else if (currentpage == "hdn-tbl-retailerdeepdive") {
        if (ModuleBlock == "PIT") {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select only one Retailer");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 11 Groups");
        }
        else {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select only one Retailer");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 25 Time Period");
        }
    }
    else if (currentpage == "hdn-tbl-comparebeverages") {
        $(".BeverageSelectionText").show();
        $(".BeverageSelectionText").text("");
        $(".BeverageSelectionText").text("You can select from 2 to 11 Beverages");
    }
    else if (currentpage == "hdn-tbl-beveragedeepdive") {
        if (ModuleBlock == "PIT") {
            $(".BeverageSelectionText").show();
            $(".BeverageSelectionText").text("");
            $(".BeverageSelectionText").text("You can select only one Beverage");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 11 Groups");
        }
        else {
            $(".BeverageSelectionText").show();
            $(".BeverageSelectionText").text("");
            $(".BeverageSelectionText").text("You can select only one Beverage");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 25 Time Period");
        }
    }
    else if (currentpage == "hdn-e-commerce-tbl-comparesites") {
        $(".SiteSelectionText").show();
        $(".SiteSelectionText").text("");
        $(".SiteSelectionText").text("You can select from 2 to 11 Sites");
    }
    else if (currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
        if (ModuleBlock == "PIT") {
            $(".SiteSelectionText").show();
            $(".SiteSelectionText").text("");
            $(".SiteSelectionText").text("You can select only one Site");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 11 Groups");
        }
        else {
            $(".SiteSelectionText").show();
            $(".SiteSelectionText").text("");
            $(".SiteSelectionText").text("You can select only one Site");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 25 Time Period");
        }
    }
    else if (currentpage == "hdn-chart-compareretailers") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select from 2 to 11 Retailers");

        $(".MeasureSelectionText").show();
        $(".MeasureSelectionText").text("");
        $(".MeasureSelectionText").text("You can select upto 10 Measures");
    }
    else if (currentpage == "hdn-chart-retailerdeepdive") {
        if (ModuleBlock == "PIT") {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select only one Retailer");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 11 Groups");

            $(".MeasureSelectionText").show();
            $(".MeasureSelectionText").text("");
            $(".MeasureSelectionText").text("You can select upto 10 Measures");
        }
        else {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select Either 1 or up to 11 Retailers");

            $(".MeasureSelectionText").show();
            $(".MeasureSelectionText").text("");
            $(".MeasureSelectionText").text("You can select Either 1 or up to 11 Measures");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 25 Time Period");
        }
    }
    else if (currentpage == "hdn-chart-comparebeverages") {
        $(".BeverageSelectionText").show();
        $(".BeverageSelectionText").text("");
        $(".BeverageSelectionText").text("You can select from 2 to 11 Beverages");

        $(".MeasureSelectionText").show();
        $(".MeasureSelectionText").text("");
        $(".MeasureSelectionText").text("You can select upto 10 Measures");
    }
    else if (currentpage == "hdn-chart-beveragedeepdive") {
        //if (ModuleBlock == "PIT") { }
        //else { }
        $(".BeverageSelectionText").show();
        $(".BeverageSelectionText").text("");
        $(".BeverageSelectionText").text("You can select only one Beverage");

        $(".GroupSelectionText").show();
        $(".GroupSelectionText").text("");
        $(".GroupSelectionText").text("You can select from 2 to 11 Groups");

        $(".MeasureSelectionText").show();
        $(".MeasureSelectionText").text("");
        $(".MeasureSelectionText").text("You can select upto 10 Measures");
    }
    else if (currentpage == "hdn-e-commerce-chart-comparesites") {
        $(".SiteSelectionText").show();
        $(".SiteSelectionText").text("");
        $(".SiteSelectionText").text("You can select from 2 to 11 Sites");

        $(".MeasureSelectionText").show();
        $(".MeasureSelectionText").text("");
        $(".MeasureSelectionText").text("You can select upto 10 Measures");
    }
    else if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
        if (ModuleBlock == "PIT") {
            $(".SiteSelectionText").show();
            $(".SiteSelectionText").text("");
            $(".SiteSelectionText").text("You can select only one Site");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 11 Groups");

            $(".MeasureSelectionText").show();
            $(".MeasureSelectionText").text("");
            $(".MeasureSelectionText").text("You can select upto 10 Measures");
        }
        else {
            $(".SiteSelectionText").show();
            $(".SiteSelectionText").text("");
            $(".SiteSelectionText").text("You can select only one Site");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 25 Time Period");

            $(".MeasureSelectionText").show();
            $(".MeasureSelectionText").text("");
            $(".MeasureSelectionText").text("You can select upto 10 Measures");
        }
    }
    else if (currentpage == "hdn-analysis-acrossshopper") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select only one Retailer");

        $(".BeverageSelectionText").show();
        $(".BeverageSelectionText").text("");
        $(".BeverageSelectionText").text("You can select up to 9 Products");
    }
    else if (currentpage == "hdn-analysis-acrosstrips") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select only one Retailer");

        $(".GroupSelectionText").show();
        $(".GroupSelectionText").text("");
        $(".GroupSelectionText").text("You can select up to 4 Groups");
    }
    else if (currentpage == "hdn-analysis-withinshopper") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select minimum 3 Retailers");
    }
    else if (currentpage == "hdn-analysis-withintrips") {
        $(".GroupSelectionText").show();
        $(".GroupSelectionText").text("");
        $(".GroupSelectionText").text("You can select minimum 3 Groups");

        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select only one Retailer");
    }
    else if (currentpage == "hdn-analysis-crossretailerfrequencies") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select only one Retailer");
    }
    else if (currentpage == "hdn-analysis-crossretailerimageries") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select up to 13 Retailers");
    }
    else if (currentpage == "hdn-report-compareretailersshoppers") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".Retailers .RetailerSelectiontext").text("You can select from 2 to 5 Retailers");
        $(".CompetitorFrequency-Retailers .RetailerSelectiontext").text("YOU CAN SELECT ONLY ONE RETAILER");
    }
    else if (currentpage == "hdn-report-retailersshopperdeepdive") {
        if (ModuleBlock == "PIT") {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select only one Retailer");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 5 Groups");
        }
        else {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select only one Retailer");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 6 Time Period");
        }
    }
    else if (currentpage == "hdn-report-compareretailerspathtopurchase") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".Retailers .RetailerSelectiontext").text("You can select from 2 to 5 Retailers");
        $(".CompetitorFrequency-Retailers .RetailerSelectiontext").text("YOU CAN SELECT ONLY ONE RETAILER");
    }
    else if (currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
        if (ModuleBlock == "PIT") {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select only one Retailer");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 5 Groups");
        }
        else {
            $(".RetailerSelectiontext").show();
            $(".RetailerSelectiontext").text("");
            $(".RetailerSelectiontext").text("You can select only one Retailer");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 6 Time Period");
        }
    }
    else if (currentpage == "hdn-report-comparebeveragesmonthlypluspurchasers") {
        $(".BeverageSelectionText").show();
        $(".BeverageSelectionText").text("");
        $(".BeverageSelectionText").text("You can select from 2 to 5 Beverages");
    }
    else if (currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive") {
        if (ModuleBlock == "PIT") {
            $(".BeverageSelectionText").show();
            $(".BeverageSelectionText").text("");
            $(".BeverageSelectionText").text("You can select only one Beverage");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 5 Groups");
        }
        else {
            $(".BeverageSelectionText").show();
            $(".BeverageSelectionText").text("");
            $(".BeverageSelectionText").text("You can select only one Beverage");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 6 Time Period");
        }
    }
    else if (currentpage == "hdn-report-comparebeveragespurchasedetails") {
        $(".BeverageSelectionText").show();
        $(".BeverageSelectionText").text("");
        $(".BeverageSelectionText").text("You can select from 2 to 5 Beverages");
    }
    else if (currentpage == "hdn-report-beveragespurchasedetailsdeepdive") {
        if (ModuleBlock == "PIT") {
            $(".BeverageSelectionText").show();
            $(".BeverageSelectionText").text("");
            $(".BeverageSelectionText").text("You can select only one Beverage");

            $(".GroupSelectionText").show();
            $(".GroupSelectionText").text("");
            $(".GroupSelectionText").text("You can select from 2 to 5 Groups");
        }
        else {
            $(".BeverageSelectionText").show();
            $(".BeverageSelectionText").text("");
            $(".BeverageSelectionText").text("You can select only one Beverage");

            $(".TimePeriodSelectionText").show();
            $(".TimePeriodSelectionText").text("");
            $(".TimePeriodSelectionText").text("You can select from 2 to 6 Time Period");
        }
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        $(".GroupSelectionText").show();
    }
    else if (currentpage == "hdn-dashboard-pathtopurchase") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select only one Retailer");
    }
    else if (currentpage == "hdn-dashboard-demographic") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select only one Retailer");
    }
    else if (currentpage == "hdn-analysis-establishmentdeepdive") {
        $(".RetailerSelectiontext").show();
        $(".RetailerSelectiontext").text("");
        $(".RetailerSelectiontext").text("You can select only one Retailer");
    }
}
//GetIShopFilters();
function clearOutScr() {
    $("#chart-title").hide();
    $("#ToShowChart").hide();
    $(".showChartMain").hide();
    $("#spChartLegend").hide();
    $("#LinkForCharts").hide();
    $(".ChartDivArea").hide();
}

function RemoveFrequency(obj) {
    if (compFreqViews.includes(currentpage)) {
        $(" * [filtertype='FREQUENCY'][uniqueid='" + $(obj).attr("id") + "']").removeClass("Selected");
        SelectedFrequencyList = [];
        ShowSelectedFilters();
        SetTripsDefaultFrequency();
        return;
    }
    if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
        var ObjData = $("#groupDivId * [filtertype='FREQUENCY'][uniqueid='" + $(obj).attr("Uniqueid") + "']");
        if (ObjData.length <= 0)
            ObjData = $("#groupDivId * [filtertype='FREQUENCY'][uniqueid='" + $(obj).attr("Uniqueid") + "']");
    }
    else if (currentpage == "hdn-report-compareretailerspathtopurchase" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
        //var ObjData = $("#SecondaryAdvancedFilterContentAdv .Frequency *[uniqueid='" + $(obj).attr("Uniqueid") + "']").parent();
        var ObjData = $("#AdvFilterDivId * [filtertype='FREQUENCY'][uniqueid='" + $(obj).attr("Uniqueid") + "']");

        //if (ObjData.length <= 0)
        //ObjData = $("#SecondaryAdvancedFilterContentAdv .Frequency *[uniqueid='" + $(obj).attr("Uniqueid") + "']").parent();
    }
    else {
        var ObjData = $("#frequency_containerId * [uniqueid='" + $(obj).attr("Uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#frequency_containerId-SubLevel1 * [uniqueid='" + $(obj).attr("Uniqueid") + "']").parent();
        if (ObjData.length <= 0)
            ObjData = $("#frequency_containerId-SubLevel2 * [uniqueid='" + $(obj).attr("Uniqueid") + "']").parent();
    }
    SelectFrequency(ObjData);
}
function SelectCustomBaseFrequency(obj, Flag) {
    custombase_Frequency = [];
    if ($(obj).hasClass("Selected") && Flag != true) {
        $(obj).removeClass("Selected");
    }
    else {
        $("#custombase-groupDivId ul li[filtertype='FREQUENCY']").removeClass("Selected");
        $(obj).addClass("Selected");
        if ($($(obj)[0]).attr("name").toLowerCase() == "selection") {
            custombase_Frequency.push({ Id: $(obj).attr("id"), Name: $(obj).attr("parentname"), FullName: $(obj).attr("Fullname"), UniqueId: $(obj).attr("parentid") });
            if (CompetitorCustomBaseFrequency.length > 0) {
                if (custombase_Frequency[0].Name != CompetitorCustomBaseFrequency[0].Name) {
                    //CompetitorFrequency = [];
                    //CompetitorRetailer = [];
                    CompetitorCustomBaseFrequency = [];
                    CompetitorCustomBaseRetailer = [];
                }
            }
        }
        else if ($($(obj)[0]).attr("name").toLowerCase() == "online") {
            custombase_Frequency.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), FullName: $(obj).attr("Fullname"), UniqueId: $(obj).attr("parentid") });
            CompetitorCustomBaseFrequency = [];
            CompetitorCustomBaseRetailer = [];
        }
        else
            custombase_Frequency.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), FullName: $(obj).attr("Fullname"), UniqueId: $(obj).attr("uniqueid") });
        //custombase_Frequency.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), FullName: $(obj).attr("Fullname"), UniqueId: $(obj).attr("uniqueid") });

        $(obj).addClass("Selected");
    }
    ShowSelectedFilters();
}
function RemoveCustomBaseFrequency(obj) {
    custombase_Frequency = [];
    $("#custombase-groupDivId ul li[filtertype='FREQUENCY']").removeClass("Selected");
    ShowSelectedFilters();
    //var fre = $("#custombase-groupDivId ul li[parentname='FREQUENCY'][name='" + $(obj).attr("name") + "'][uniqueid='" + $(obj).attr("uniqueid") + "']");
    //if (fre.length == 0)
    //    fre = $("#custombase-groupDivId ul li[name='" + $(obj).attr("name") + "'][uniqueid='" + $(obj).attr("uniqueid") + "']");
    //SelectCustomBaseFrequency(fre);
}
function SelectFrequency(obj) {
    if ($($(obj)[0]).attr("name").toLowerCase() == "cross-retailer shopper") {
        var compObj = $(obj).parent().parent().parent().find("ul li[name='" + $(obj).attr("parentname") + "'][id='" + $(obj).attr("parentid") + "']");
        OpenOrCloseCompetitorFrequencyRetailerPopup(compObj);
        return;
    }
    if ($(obj).parent().parent().parent().attr("id") == "custombase-groupDivId") {
        SelectCustomBaseFrequency(obj);
        return;
    }
    if (currentpage == "hdn-dashboard-pathtopurchase")
        CustomBaseFlag = 1;

    //$("#frequency_containerId ul li div").removeClass("Selected");    
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    //SelectedFrequencyList = [];
    for (var i = 0; i < SelectedFrequencyList.length; i++) {
        if (SelectedFrequencyList[i].UniqueId == $(object).attr("UniqueId") && $(object).attr("name") != "Online") {
            $(obj).addClass("Selected");
            sCurrentDemoId = i;
        }
    }

    if (sCurrentDemoId.toString() != "") {
        if (TabType.toLowerCase() != "shopper") {
            $(obj).removeClass("Selected");
            SelectedFrequencyList.splice(sCurrentDemoId, 1);
        }
    }
    else {
        SelectedFrequencyList = [];
        if (currentpage == "hdn-dashboard-pathtopurchase") {
            $("#groupDivId").find(".Selected").removeClass("Selected");
        }
        else if (currentpage == "hdn-dashboard-demographic") {
            _.each($("#AdvFilterDivId ul li[parentname=FREQUENCY]"), function (item) {
                item.attributes.class.value = "lft-popup-ele";
            });
        }
        else if (currentpage == "hdn-report-compareretailerspathtopurchase" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
            $("#SecondaryAdvancedFilterContentAdv .Frequency").find(".Selected").removeClass("Selected");
        }
        else {
            var ObjData = $("#left-panel-frequency *").find(".Selected").removeClass("Selected");
            $(".rgt-cntrl-frequency ul li").removeClass("Selected");
            //if (ObjData.length <= 0)
            //    ObjData = $("#frequency_containerId-SubLevel1 *").find(".Selected").removeClass("Selected");
            //if (ObjData.length <= 0)
            //    ObjData = $("#frequency_containerId-SubLevel2 *").find(".Selected").removeClass("Selected");
        }
        $(obj).parent().parent().find(".Selected").removeClass("Selected");
        $(obj).addClass("Selected");
        if ($($(obj)[0]).attr("name").toLowerCase() == "selection") {
            SelectedFrequencyList.push({ Id: $(object).attr("id"), Name: $(object).attr("parentname"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("parentid") });
            if (CompetitorFrequency.length > 0) {
                if (SelectedFrequencyList[0].Name != CompetitorFrequency[0].Name) {
                    CompetitorFrequency = [];
                    CompetitorRetailer = [];
                    //CompetitorCustomBaseFrequency = [];
                    //CompetitorCustomBaseRetailer = [];
                }
            }

        }
        else if ($($(obj)[0]).attr("name").toLowerCase() == "online") {
            SelectedFrequencyList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("parentid") });
            CompetitorFrequency = [];
            CompetitorRetailer = [];
            CompetitorCustomBaseFrequency = [];
            CompetitorCustomBaseRetailer = [];
        }
        else
            SelectedFrequencyList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("uniqueid") });

    }
    custombase_Frequency = [];
    $("#custombase-groupDivId ul li[parentname='FREQUENCY']").removeClass("Selected");
    for (var i = 0; i < SelectedFrequencyList.length; i++) {
        var fre = [];
        if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic")
            fre = $("#custombase-groupDivId ul li[parentname='" + SelectedFrequencyList[i].Name + "'][parentid='" + SelectedFrequencyList[i].UniqueId + "'][name='SELECTION']");
        if (currentpage == "hdn-dashboard-demographic" && fre.length == 0)
            fre = $("#custombase-groupDivId ul li[name='" + SelectedFrequencyList[i].Name + "'][parentid='" + SelectedFrequencyList[i].UniqueId + "']")
        if (fre.length == 0)
            fre = $("#custombase-groupDivId ul li[parentname='FREQUENCY'][name='" + SelectedFrequencyList[i].Name + "'][uniqueid='" + SelectedFrequencyList[i].UniqueId + "']");
        if (fre.length > 0) {
            SelectCustomBaseFrequency(fre, $(fre).hasClass("Selected"));
            $(fre).addClass("Selected");
        }
    }
    ShowSelectedFilters();
    SetTripsDefaultFrequency();
}
function PrepareSearch(SearchFor, SearchBox, AppendTo, searchArray) {
    $("#" + SearchBox).autocomplete({
        delay: 0,
        minLength: 2,
        appendTo: "#" + AppendTo,//"#Retailer-Search-Content",
        autoFocus: false,
        position: {
            my: "left top",
            at: "left bottom",
            collision: "none"
        },
        open: function () {
            //$(".Search-Filter .ui-widget-content").css("max-width", "196px");
            $(".Search-Filter .ui-widget-content").css("max-width", "300px");
            $(".Search-Filter .ui-widget-content").css("width", "231px");
            //$(".Search-Filter .ui-widget-content").css("max-width", "203px");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "black");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("cursor", "pointer");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("background-color", "transparent");
            //$(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("border","solid 0.1px blue");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("padding", "4px");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("margin-left", "4px");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("margin-right", "4px");
            //$(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "#7F7F7F");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "black");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("text-transform", "uppercase");
        },
        close: function (event, ui) {
            //if ($.isNumeric($("#" + SearchBox).val()))
            //$(".txt-search").val("");
        },
        focus: function (event, ui) {
            this.value = ui.item.label;
            $.each($('.ui-menu-item-wrapper'), function (index, value) {
                $('#' + value.id).attr('title', value.innerHTML);
            });
            event.preventDefault();
        },
        source: searchArray,
        select: function (e, ui) {
            var obj = ui.item.value;
            if (obj.IsGeography) {
                var geoobj = obj.GeoObj;
                addGeographyFilter(geoobj);
            }

            var searchObj;
            if (SearchFor == "Retailer") {
                searchObj = $("#RetailerDivId li[uniqueid='" + obj.UniqueId + "']");

                SelectComparison(searchObj);
            }
            if (SearchFor == "Competitor-Retailer") {
                searchObj = $("#CompetitorFrequency-RetailerDivId li[uniqueid='" + obj.UniqueId + "']");

                SelectCompetitor(searchObj);
            }
            else if (SearchFor == "DemographicFilters") {
                searchObj = $("#AdvFilterDivId li[parentname='" + obj.ParentName + "'][uniqueId='" + obj.UniqueId + "']");
                if (obj.ParentName == "FREQUENCY") {
                    SelectFrequency(searchObj);
                    return;
                }
                if ((searchObj.length <= 0) && ((currentpage.indexOf("hdn-report") > -1) && currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive" && currentpage != "hdn-report-comparebeveragesmonthlypluspurchasers" && currentpage != "hdn-report-beveragemonthlypluspurchasersdeepdive")) {
                    searchObj = $("#Advmaindiv span[uniqueId='" + obj.UniqueId + "']").parent();
                }

                if (currentpage == "hdn-dashboard-demographic")
                    SelecGroupMetricName(searchObj);
                else
                    SelectDemographic(searchObj);
            }
            else if (SearchFor == "Frequency" || SearchFor == "Monthly") {
                if (SearchFor == "Frequency")
                    searchObj = $("#frequency_containerId span[uniqueId='" + obj.UniqueId + "']").parent();
                else
                    searchObj = $("#frequency_containerId span[uniqueId='" + obj.UniqueId + "'][onClick='SelectFrequency(this);']");
                if (searchObj.length <= 0) {
                    searchObj = $("#frequency_containerId-SubLevel1 span[uniqueId='" + obj.UniqueId + "']").parent();
                }
                if (searchObj.length <= 0)
                    searchObj = $("#frequency_containerId-SubLevel2 span[uniqueId='" + obj.UniqueId + "']").parent();

                SelectFrequency(searchObj);
            }
            else if (SearchFor == "AllBevFrequency") {
                if (SearchFor == "AllBevFrequency")
                    searchObj = $(".rgt-cntrl-Selection-Conatiner span[id='" + obj.UniqueId + "']").parent();
                if (searchObj.length <= 0) {
                    searchObj = $(".rgt-cntrl-Selection-Conatiner span[id='" + obj.UniqueId + "']").parent();
                }
                SelectBeverageSelectionType(searchObj);
            }
            else if (SearchFor == "AdvancedFilters") {
                searchObj = $("#VisistsFilterDivId span[uniqueId='" + obj.UniqueId + "']").parent();
                SelectAdvfilters(searchObj);
            }
            else if (SearchFor == "Type") {
                searchObj = $("#groupDivId ul li[parentname='" + obj.ParentName + "'][uniqueid='" + obj.UniqueId + "']");
                if (obj.ParentName == "FREQUENCY") {
                    SelectFrequency(searchObj);
                }
                else {
                    SelecGroupMetricName(searchObj);
                }
            }
            else if (SearchFor == "Custom-Base-Filter-Type") {
                searchObj = $("#custombase-groupDivId ul li[parentname='" + obj.ParentName + "'][uniqueid='" + obj.UniqueId + "']");
                if (obj.ParentName == "FREQUENCY") {
                    SelectFrequency(searchObj);
                }
                else {
                    SelecCustomBaseGroupMetricName(searchObj);
                }
            }
            else if (SearchFor == "Geography") {
                searchObj = $("#soap-geography-data li[uniqueid='" + obj.UniqueId + "']");
                SelectGeographyData(searchObj);
            }
            else if (SearchFor == "Beverage") {
                searchObj = $("#BevDivId li[uniqueid='" + obj.UniqueId + "']");

                SelectBevComparison(searchObj);
            }
            else if (SearchFor == "Channel") {
                searchObj = $("#channel-content li[uniqueid='" + obj.UniqueId + "']");
                if (searchObj.length == 0)
                    searchObj = $("#beverage-where-purchased li[uniqueid='" + obj.UniqueId + "']");

                SelectChannel(searchObj);
            }
            else if (SearchFor == "TotalMeasure") {
                searchObj = $("#total-measure-trip li[uniqueid='" + obj.UniqueId + "']");

                SelectTotalMeasure(searchObj);
            }
            else if (SearchFor == "Measure") {
                searchObj = $("#retailer-measure li[parentname='" + obj.ParentName + "'][uniqueid='" + obj.UniqueId + "']");

                SelecMeasureMetricName(searchObj);
            }
            else if (SearchFor == "Left-Panel-Frequency") {
                searchObj = $("#left-panel-frequency li[uniqueid='" + obj.UniqueId + "']");
                if (currentpage == "hdn-report-compareretailersshoppers" || currentpage == "hdn-report-retailersshopperdeepdive") {
                    searchObj = $("#left-panel-frequency li[parentname='" + obj.Name + "'][name='SELECTION']");
                }
                SelectFrequency(searchObj);
            }
            else if (SearchFor == "AdvancedAnalytics") {
                searchObj = $("#CorrespondenceMeasureDivId li[uniqueid='" + obj.UniqueId + "']");

                SelectAdvanceAnalyticsTrips(searchObj);
            }
            else if (SearchFor == "Custombase-Retailers") {
                var CompObj = $("#Custombase-RetailerDivId span[uniqueid='" + obj.UniqueId + "']");
                SelectPathToPurchaseCustomBase(CompObj);
            }
            else if (SearchFor == "Sar-Retailer") {
                let compObj = $("#SarRetailerDivId .Lavel ul li[UniqueId='" + obj.UniqueId + "'][parent-of-parent='" + obj.ParentOfParent + "']");
                if (compObj.length <= 0) {
                    compObj = $("#SarRetailerDivId .Lavel ul li[UniqueId='" + obj.UniqueId + "']");
                }
                SelectSarRetailer(compObj)
            }
            else if (SearchFor == "Competitor") {
                let compObj = $("#SarCompetitorDivId .Lavel ul li[UniqueId='" + obj.UniqueId + "'][parent-of-parent='" + obj.ParentOfParent + "']");
                if (compObj.length <= 0) {
                    compObj = $("#SarCompetitorDivId .Lavel ul li[UniqueId='" + obj.UniqueId + "']");
                }
                SelectSarCompetitor(compObj);
            }
            else if (SearchFor == "Sar-Frequency") {
                let compObj = $("#SarFrequencyDivId .Lavel ul li[UniqueId='" + $(obj).attr("UniqueId") + "']");
                SelectSarFrequency(compObj);
            }
            e.stopImmediatePropagation();
            this.value = "";
            return false;
        }
    });

}

function getTimeperiodEx() {
    switch (TimeExtension) {
        case 'total time':
            return TimeExtension;
        case 'year':
        case 'ytd':
        case 'quarter':
            return $(".timeType").val();
        case '12mmt':
        case '6mmt':
        case '3mmt':
            return TimePeriod.split('|')[1] + " " + TimeExtension;
        default:
            return $(".timeType").val();

    }
}

//Competitor functions
function SelectCompetitor(obj) {
    var ptpcus = new Object();
    ptpcus = obj;
    if ($(ptpcus).attr("isselectable") != undefined && $(ptpcus).attr("isselectable") == "False")
        return;

    clearSelectedCompetitors();

    if ($(obj).parent("div").hasClass("FilterStringContainerdiv"))
        $(obj).parent(".FilterStringContainerdiv").parent("li").addClass("Selected");
    else
        $(obj).addClass("Selected");
    //CustomBasePrev = CustomBasePrev.length == 0 ? CustomBase : CustomBasePrev;
    if (!isCompetitorCustomBase) {
        if (SelectedFrequencyList[0].Name != CompetitorFrequency[0].Name) {
            var freqObj = $("li[filtertype='FREQUENCY'][name='SELECTION'][parentname='" + CompetitorFrequency[0].Name + "']");
            if (freqObj.length > 0) {
                if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
                    SelecGroupMetricName(freqObj[0]);
                }
                else {
                    SelectFrequency(freqObj);
                }
            }
        }
        CompetitorRetailer = [];
        CompetitorRetailer.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
        CompetitorCustomBaseRetailer = [];
        if ((currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") && CompetitorCustomBaseRetailer.length == 0) {
            CompetitorCustomBaseFrequency = CompetitorFrequency;
            if (custombase_Frequency[0].Name != CompetitorCustomBaseFrequency[0].Name) {
                var freqObj = $("li[filtertype='FREQUENCY'][name='SELECTION'][parentname='" + CompetitorCustomBaseFrequency[0].Name + "']");
                if (freqObj.length > 0)
                    SelectCustomBaseFrequency(freqObj);
            }
            CompetitorCustomBaseRetailer.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
        }
    }
    else {
        if (custombase_Frequency.length == 0 || custombase_Frequency[0].Name != CompetitorCustomBaseFrequency[0].Name) {
            var freqObj = $(".Custombase-GroupType li[filtertype='FREQUENCY'][name='SELECTION'][parentname='" + CompetitorCustomBaseFrequency[0].Name + "']");
            if (freqObj.length > 0)
                SelectCustomBaseFrequency(freqObj);
        }
        CompetitorCustomBaseRetailer = [];
        CompetitorCustomBaseRetailer.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
    }

    //$("#custom-base-ratailer").html($(ptpcus).attr("name"));
    ShowSelectedFilters();
}

function RemoveCompetitor(obj) {
    clearSelectedCompetitors();
    CompetitorRetailer = [];
    ShowSelectedFilters();
}

function RemoveCustomBaseCompetitor(obj) {
    clearSelectedCompetitors()
    CompetitorCustomBaseRetailer = [];
    ShowSelectedFilters();
}

function clearSelectedCompetitors() {
    $(".CompetitorFrequency-Retailers li").find(".Selected").removeClass("Selected");
    $(".CompetitorFrequency-Retailers #ChannelOrCategoryContent *").find(".Selected").removeClass("Selected");
    $(".CompetitorFrequency-Retailers .Retailer *").find(".Selected").removeClass("Selected");
    $(".CompetitorFrequency-Retailers .Retailer .Comparison").find(".Selected").removeClass("Selected");
    $("#CompetitorFrequency-RetailerDivId *").find(".Selected").removeClass("Selected");
}

function competitor_submit() {
    $("#Translucent").hide();
    $(".CompetitorFrequency-Retailers").hide();
    ShowSelectedFilters();

    if (isCompetitorCustomBase) {
        $(".Custombase-GroupType").hide();
        $(".Custombase-Retailers").hide();
        $("#Translucent").show();
        $("#custombase-stattesting-popup").show();
    }

    //$("#submitButton").trigger('click');
    //prepareContentArea();

    isCompetitorCustomBase = false;
}

function competitor_cancel() {
    setTimeout(function () {
        $("#Translucent").hide();
        $(".CompetitorFrequency-Retailers").hide();

        if (currentpage.indexOf("dashboard") > -1) {
            if (isCompetitorCustomBase) {
                $("#Custom-Base-Add-Filter-Popup").trigger('click');
                $(".Custombase-GroupType .level2").show();
                isCompetitorCustomBase = false;
            }
            else {
                $("#GroupType").trigger('click');
                $(".GroupType .level2").show();
            }
        }
        else {
            $("#AdvancedFilters").trigger('click');
            $(".AdvancedFilters .level2").show();
        }

    }, 1)
}


/*Start Color picker Sabat*/
function update(picker) {
    $(".redVal>input").val(Math.round(picker.rgb[0]));
    $(".greenVal>input").val(Math.round(picker.rgb[1]));
    $(".blueVal>input").val(Math.round(picker.rgb[2]));
    //Clear the Input val
    $(".jscolor").val('');
}
var performUpFunc = function (tempclass) {
    console.log("a");
    var curInputEle = $("." + tempclass + ">input");
    var tempVal = $(curInputEle).val();
    if (tempVal == undefined || tempVal == null || isNaN(tempVal)) {
        $(curInputEle).val(0);
    } else {
        if (tempVal < 255) {
            $(curInputEle).val(++tempVal);
        }
        if (tempVal > 255) {
            $(curInputEle).val(255);
        }
        if (tempVal < 0) {
            $(curInputEle).val(0);
        }
    }
    //Update the Color
    recalibrateColorfronInput($('.redVal>input'), $('.greenVal>input'), $('.blueVal>input'));
}
var performDownFunc = function (tempclass) {
    var curInputEle = $("." + tempclass + ">input");
    var tempVal = $(curInputEle).val();
    if (tempVal == undefined || tempVal == null || isNaN(tempVal)) {
        $(curInputEle).val(0);
    } else {
        if (tempVal > 0) {
            $(curInputEle).val(--tempVal);
        }
        if (tempVal < 0) {
            $(curInputEle).val(0);
        }
        if (tempVal > 255) {
            $(curInputEle).val(255);
        }
    }
    //Update the Color
    recalibrateColorfronInput($('.redVal>input'), $('.greenVal>input'), $('.blueVal>input'));
}
$(document).on('click', '.updownDiv .up', function (e) {
    var tempclass = $(this).attr('data-val');
    performUpFunc(tempclass);
});
$(document).on('click', '.updownDiv .down', function (e) {
    var tempclass = $(this).attr('data-val');
    performDownFunc(tempclass);

});
//Continuos Click
$(document).on('mousedown', '.updownDiv .up', function () {
    var tempclass = $(this).attr('data-val');
    intForContinuous = setInterval(function (tempClass) {
        performUpFunc(tempclass);
    }, 100);
});
$(document).on('mouseup', '.updownDiv .up', function () {
    clearInterval(intForContinuous);
});
$(document).on('mousedown', '.updownDiv .down', function () {
    var tempclass = $(this).attr('data-val');
    intForContinuous = setInterval(function (tempClass) {
        performDownFunc(tempclass);
    }, 100);
});
$(document).on('mouseup', '.updownDiv .down', function () {
    clearInterval(intForContinuous);
});
var recalibrateColorfronInput = function (r, g, b) {
    if (!(r == undefined || g == undefined || b == undefined)) {
        var red = colorValCheck($(r).val());
        var green = colorValCheck($(g).val());
        var blue = colorValCheck($(b).val());
        //Set the colorCodeDisplay input BG color
        $(".jscolor").css('background-color', 'rgb(' + red + ',' + green + ',' + blue + ')');
    }
}
var colorValCheck = function (num) {
    if (num == null || num == undefined || isNaN(num) || num > 255 || num < 0) {
        return -1;
    }
    else {
        return +num;
    }
}
var setValueBackInRangeOnKeyChange = function (ele) {
    var num = $(ele).val();
    if (num == null || num == undefined || isNaN(num)) {
        $(ele).val(0);
    } else {
        if (num > 255) { $(ele).val(255); }
    }
    $(ele).val(+($(ele).val()));
}
/*Start Keyup event to Chane the color code*/
$(document).on('keydown', '.valDisplay>input', function (event) {
    var keys = ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')'];
    if (ctrlUp && event.which == 65) { } else {
        if ((event.which != 8) && (event.key != "Tab") && (event.which != "Control") && (keys.indexOf(event.key) != -1 || event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    }
    if (event.key == "Control") { ctrlUp = true; } else { ctrlUp = false; }
});
$(document).on('keyup', '.valDisplay>input', function () {
    setValueBackInRangeOnKeyChange($(this));
    //Update the Color
    recalibrateColorfronInput($('.redVal>input'), $('.greenVal>input'), $('.blueVal>input'));
});
$(document).on('click', '#colorpalletmaincontainer .color_ok', function () {
    //Apply validation
    //CallvalidateColorCode();
    validateColorCode()
});
$(document).on('click', '#colorpalletmaincontainer .color_cancel', function () {
    $(".custom-estcolordiv").removeClass("active");
    $(".colorpalletwithTranslucentBG").hide();
});
$(document).on('click', '.custom-estcolordiv', function () {
    $(".custom-estcolordiv").removeClass("active");
    $(this).addClass('active');
    //Update the color in pallet
    var rgbarr = $(this).css('background-color').replace(/rgb| |\(|\)/g, '').split(',');
    $('.redVal>input').val(rgbarr[0]);
    $('.greenVal>input').val(rgbarr[1]);
    $('.blueVal>input').val(rgbarr[2]);
    recalibrateColorfronInput($('.redVal>input'), $('.greenVal>input'), $('.blueVal>input'));
    //Show color pallet
    $(".colorpalletwithTranslucentBG").show();
});
function rgbToHex(r) {
    return "#" + ((1 << 24) + (+r[0] << 16) + (+r[1] << 8) + +r[2]).toString(16).slice(1);
}
var assignfillcolorinpopup = function (dtval) {
    var tempInd = 0;
    $(".custom-estcolordiv").each(function (i, d) {
        var tempColorcode = "";
        //var tempColorcode = $('.master-lft-ctrl[data-val="' + dtval + '"] .lft-popup-ele-label[data-id=' + $(d).attr("id") + ']').attr("colorcode");//Get from establishments
        if (tempColorcode != undefined && tempColorcode != null && tempColorcode != "null" && tempColorcode != "") {
            //Assign bg-color
            $(d).css('background-color', tempColorcode);
            $(d).attr('colorcode', tempColorcode);
            $(d).attr('originalcolor', tempColorcode);
        } else {
            //Assign color from defaultcolors
            $(d).css('background-color', defaultchartColors[tempInd]);
            $(d).attr('colorcode', defaultchartColors[tempInd]);
            $(d).attr('originalcolor', defaultchartColors[tempInd]);
            tempInd++;
        }

    });
}

var validateColorCode = function () {
    $(".colorpalletwithTranslucentBG").hide();

    //get current selected colorcode in Hex form
    var tempCode = (rgbToHex($(".jscolor").css('background-color').replace(/rgb| |\(|\)/g, '').split(','))).toLocaleLowerCase();

    //Return if hex code is same as previous - check 1
    if (tempCode == $(".custom-estcolordiv.active").attr('colorcode') || tempCode.toLocaleUpperCase() == $(".custom-estcolordiv.active").attr('colorcode')) {
        $(".custom-estcolordiv").removeClass('active');
        return;
    }

    //Check if the color is one of the default colors - check 2
    if (defaultchartColors.indexOf(tempCode) != -1 || defaultchartColors.indexOf(tempCode.toLocaleUpperCase()) != -1) {
        showMessage("You can not select among default colors.");
        $(".custom-estcolordiv").removeClass('active');
        return;
    }

    //Check if it already selected in current list - check 3
    var isThere = false;
    $(".custom-estcolordiv:not(.active)").each(function (i, d) {
        if ($(d).attr('colorcode') == tempCode || $(d).attr('colorcode') == tempCode.toLocaleUpperCase()) {
            isThere = true;
            return false;
        };
    });
    if (isThere) {
        showMessage("Selected color is already assigned.");
        $(".custom-estcolordiv").removeClass('active');
        return;
    }

    //Check if the same exist in Establishment or not - check 4
    //------------------ Color code validation from db side -------------------//
    //let changedColorCodeList = [];
    //let selectionId = $(".custom-estcolordiv.active").attr('id');
    //if (dataval == "Measures") {
    //    let colorCodeObj = {
    //        ColourCode: tempCode,
    //        Establishmentid: '',
    //        MeasureId: selectionId,
    //        GroupsId: '',
    //        IsTrend: IsPIT_TREND
    //    };
    //    changedColorCodeList.push(colorCodeObj);
    //}
    //else if (dataval == "Establishment") {
    //    let colorCodeObj = {
    //        ColourCode: tempCode,
    //        Establishmentid: selectionId,
    //        MeasureId: '',
    //        GroupsId: '',
    //        IsTrend: IsPIT_TREND
    //    };
    //    changedColorCodeList.push(colorCodeObj);
    //}
    //else if (dataval == "Metric Comparisons") {
    //    let colorCodeObj = {
    //        ColourCode: tempCode,
    //        Establishmentid: '',
    //        MeasureId: '',
    //        GroupsId: selectionId,
    //        IsTrend: IsPIT_TREND
    //    };
    //    changedColorCodeList.push(colorCodeObj);
    //}
    //else if (dataval == "Beverage") {
    //    let colorCodeObj = {
    //        ColourCode: tempCode,
    //        Establishmentid: selectionId,
    //        MeasureId: '',
    //        GroupsId: '',
    //        IsTrend: IsPIT_TREND
    //    };
    //    changedColorCodeList.push(colorCodeObj);
    //}

    //let IsBeverageModule = false;
    //if (controllername == "chartestablishmentcompare" || controllername == "chartestablishmentdeepdive")
    //    IsBeverageModule = false;
    //else if (controllername == "chartbeveragecompare" || controllername == "chartbeveragedeepdive")
    //    IsBeverageModule = true;

    //$.ajax({
    //    url: appRouteUrl + "Chart/ValidatePalletteColor",
    //    data: JSON.stringify({ ChangedColorCodeList: changedColorCodeList, IsBeverageModule: IsBeverageModule }),
    //    method: "POST",
    //    async: false,
    //    contentType: "application/json",
    //    success: function (response) {
    //        if (response != "SUCCESS") {
    //            showMessage("Selected color is already assigned.");
    //            $(".custom-estcolordiv").removeClass('active');
    //            return;
    //        }
    //    },
    //    error: ajaxError
    //});
    ////-------------------------------------------------------------------------//
    //var est_with_color_Code = $('.master-lft-ctrl[data-val="' + dataval + '"] .lft-popup-ele-label[colorcode="' + tempCode + '"]');
    //est_with_color_Code = (est_with_color_Code.length == 0 ? $('.master-lft-ctrl[data-val="' + dataval + '"] .lft-popup-ele-label[colorcode="' + tempCode.toLocaleUpperCase() + '"]') : est_with_color_Code);
    //if (est_with_color_Code.length != 0) {
    //    //Check if that establishments color is updated in popup or not
    //    //Get id of est
    //    var t_id_ele = $(".custom-estcolordiv[id='" + $(est_with_color_Code).attr('data-id') + "']");
    //    t_id_ele = (t_id_ele.length == 0 ? $(".custom-estcolordiv[id='" + $(est_with_color_Code).attr('data-id') + "']") : t_id_ele);
    //    if (t_id_ele.length == 0) {
    //        showMessage("Selected color is already assigned.");
    //        $(".custom-estcolordiv").removeClass('active');
    //        return;
    //    } else {
    //        //Compare tempCode with t_id_ele colorcode
    //        if ($(t_id_ele).attr('colorcode') == tempCode || $(t_id_ele).attr('colorcode') == tempCode.toLocaleUpperCase()) {
    //            showMessage("Selected color is already assigned.");
    //            $(".custom-estcolordiv").removeClass('active');
    //            return;
    //        }
    //    }
    //}

    //Code for unassigned colorcode
    //Change the active box color bg, also set color-id
    $(".custom-estcolordiv.active").css('background-color', tempCode);
    $(".custom-estcolordiv.active").attr('colorcode', tempCode);
    $(".custom-estcolordiv").removeClass('active');
}

var getChangesColorToStore = function () {
    var changColorList = [];
    //Get color code from each elements compare with their old values
    $(".custom-estcolordiv").each(function (i, d) {
        if ($(d).attr('colorcode').toLocaleLowerCase() != $(d).attr('originalcolor').toLocaleLowerCase()) {
            //Add to list
            changColorList.push({
                "id": $(d).attr('id'),
                "name": $(d).attr('id'),
                "colorcode": $(d).attr('colorcode')
            });
        }
    });
    return changColorList;
}
/*End Color picker Sabat*/
