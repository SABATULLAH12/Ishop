/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../Slider/jquery-ui-slider-pips.js" />
/// <reference path="../AngularJS/angular.min.js" />

//23-11-17
var AllDemographicsSF = [];
//End 23-11-17
var html = "";
var width = 1024;
var height = 520;
var postBackData = "";
var isFirstTimePageLoad = true;
var Sites = [];
var AllSites = [];
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
var isPopupVisible = false;
var Stat_PositiveValue = "";
var Stat_NegativeValue = "";

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
var AllGeographics = [];
var AllFrequency = [];
var TripsFrequency = [];
var AllLeftPanelFrequency = [];
var AllAdvancedFilters = [];
var AllGeography = [];
var SelectedDempgraphicList = [];
var SelectedDempgraphicGeoList = [];
var SelectedFrequencyList = [];
var SelectedTripsFrequencyList = [];
var SelectedTotalMeasure = [];
var SelectedAdvFilterList = [];
var allChannels = [];
var selectedChannels = [];
var AllMonthly = [];
var Channels_DBNames = [];
var Channels_UniqueId = [];
var Metric_UniqueId = "";
var SelectedAdvancedAnalyticsList = [];
var AllTotalMeasures = [];
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

var sFilterData = {};
var sGeographyData = [];
//var currentpage = "";
var dGeo = [];

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
var sClosePopupStatus = 0;

//Reset filters
function ResetFilters() {
    SelectedDempgraphicList = [];
    SelectedAdvFilterList = [];
    Comparisonlist = [];
    SelectedFrequencyList = [];
    SelectedTripsFrequencyList = [];
    ComparisonBevlist = [];
    Sites = [];
    Measurelist = [];
    Grouplist = [];
    HideAdvFilterOnGroupSelect();
    $("#RightPanelPartial").hide();
    $(".adv-fltr-details").hide();
    if (currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive" || currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-e-commerce-tbl-comparesites") {
        $(".SiteFilters *").removeClass("Selected");
        $(".SiteFilters *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(".SiteFilters div[onclick='DisplaySecondarySiteFilter(this);']").removeClass("Selected");
        $(".SiteFilters div[onclick='DisplaySecondarySiteFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#MeasureTypeShopperTripHeader *").removeClass("Selected");
        $("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#MeasureTypeHeaderMain *").removeClass("Selected");
        $("#MeasureTypeHeaderMain *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#MeasureTypeHeaderContent *").removeClass("Selected");
        $("#MeasureTypeHeaderContent *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#MeasureTypeHeaderContentSubLevel *").removeClass("Selected");
        $("#MeasureTypeHeaderContentSubLevel *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(".rgt-cntrl-SubFilter-Conatianer *").removeClass("Selected");
        $(".rgt-cntrl-trips-frequency *").removeClass("Selected");
        $(".rgt-cntrl-ordertype *").removeClass("Selected");       
    }
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
    //if ($(".adv-fltr-showhide-txt").text() == "SHOW LESS") {
    //    $(".adv-fltr-showhide").trigger("click");
    //}
    clearOutScr();
    if (currentpage == "hdn-tbl-compareretailers") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    else if (currentpage == "hdn-tbl-retailerdeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
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
    else if (currentpage == "hdn-chart-retailerdeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        //$("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    else if (currentpage == "hdn-e-commerce-chart-comparesites") {
        $("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    else if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
        $("#TimeBlock ul li[name='TOTAL TIME']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
    }
    SetStatTesting(currentpage);
}
function UpdateDeepDive() {
    $(".stattest").each(function () {
        if ($(this).css("pointer-events") == "none") {
            $(this).css("background-color", "transparent").css("color", "black").css("cursor", "pointer").css("pointer-events", "auto");
        }
    });
    TimeExtension = "total time";
    TimePeriod = "total time";
    clearOutScr();
    ResetFilters();
    if (ModuleBlock == "TREND") {
        TimeExtension = "3MMT";
        TimePeriod = "3MMT";
        Grouplist = [];
        $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
        if (currentpage.indexOf("hdn-e-commerce-tbl") > -1) {
            TabType = "trips";
        }
        if (TabType != "")
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
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
        else if (currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
            $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
            $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
        }
    }
    else {
        LoadTimePeriod(filters);
        if (sVisitsOrGuests == "2") {
            //$("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").removeClass("Selected");
            //$("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").trigger("click");

            $("#RightPanelPartial #shopper_frequency_containerId ul li[name='MONTHLY']").removeClass("Selected");
            $("#RightPanelPartial #shopper_frequency_containerId ul li[name='MONTHLY']").trigger("click");
        }
        else if (currentpage.indexOf("e-commerce-chart") > -1 && TabType != "") {
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
        }
        else if (currentpage.indexOf("hdn-e-commerce") > -1) {
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
        }
        else if (currentpage.indexOf("hdn-report") > -1 && currentpage.indexOf("purchase") == -1) {
            $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
            $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
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
        else if (currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-retailerspathtopurchasedeepdive") {
            $(".Left-Frequency ul li[name='MONTHLY +']").removeClass("Selected");
            $(".Left-Frequency ul li[name='MONTHLY +']").trigger("click");
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
            LoadTripsFrequencyFilter(sFilterData);
            PrepareSearch("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
            HideOrShowFilters();

            $("#adv-fltr-freq").css("display", "none");
            $("#adv-fltr-Chnl").css("display", "block");
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
            $(".MeasureType .trip").show();
            $(".MeasureType .Shopper").hide();
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
                $(".beverageItems ul div[uniqueid='1']").trigger("click");

            $(".MeasureType .trip").show();
            $(".MeasureType .Shopper").hide();
        }
        if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1))
            $("#adv-bevselectiontype-freq").show();
        else {
            $("#adv-bevselectiontype-freq").hide();
            sBevarageSelctionType = [];
        }
        GetDefaultFrequency();
        $("#MeasureTypeHeaderMainTrip").show();
        $("#MeasureTypeHeaderMainTrip ul li").show();
    }
    else {

    SelectedTripsFrequencyList = [];

    if ($("#guest-visit-toggle").is(":checked")) {
        SelectedAdvFilterList = [];
        $("#RightPanelPartial .lft-popup-ele").removeClass("Selected");
        $("#RightPanelPartial .lft-popup-ele").find(".ArrowContainerdiv").css("background-color", "#58554D");
        if (currentpage.indexOf("beverage") > -1) {
            selectedChannels = [];
            $("#RightPanelPartial #frequency_containerId_trips ul li[name='ALL MONTHLY +']").trigger("click");
        }
        else {
            //$("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").removeClass("Selected");
            //$("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").trigger("click");

            $("#RightPanelPartial #shopper_frequency_containerId ul li[name='MONTHLY']").removeClass("Selected");
            $("#RightPanelPartial #shopper_frequency_containerId ul li[name='MONTHLY']").trigger("click");
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
            $("#RightPanelPartial .rgt-cntrl-SubFilter ul li[name='TOTAL']").trigger("click");
        }
        else {
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
            $("#RightPanelPartial .rgt-cntrl-ordertype ul li[name='TOTAL'").trigger("click");
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
    HideOrShowFilters();
    $("#adv-bevselectiontype-freq").hide();
    $(".freq-seperator").hide();
    ShowSelectedFilters();
}
}
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

        if ($(".BevScrollDiv").width() > window.innerWidth)
            $(".Beverages").css("width", "95%");
        else
            $(".Beverages").css("width", "auto");
        SetScroll($("#BevContainerDivId"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        if (typeof SetStyles === 'function') {
            SetStyles();
        }
        //var leftPos = $('#BevContainerDivId').scrollLeft() + 200;
        //$('#BevContainerDivId').scrollLeft(leftPos);
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
        e.stopImmediatePropagation();
    });
    $(document).on("click", ".adv-fltr-applyfiltr", function () {
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
    });
    $(".stattest-sign").mouseleave(function () {
        $(".ShowStatDetails").hide();
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

    $("#pit-toggle").click(function (e) {
        //var timer = setTimeout(function () {
        //    document.getElementById('Translucent').style.display = "block";
        //}, 100);
        GetStatTestValue();
        TabType = "";
        if ($(this).is(":checked")) {
            ModuleBlock = "TREND";
            $("#lft-fltr-trend").addClass("Active");
            $("#lft-fltr-pit").removeClass("Active");
        }
        else {
            ModuleBlock = "PIT";
            $("#lft-fltr-pit").addClass("Active");
            $("#lft-fltr-trend").removeClass("Active");
        }
        pit_trend_toggletype = ModuleBlock;

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
        if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("cursor", "pointer");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("background-color", "");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").attr("onclick", "DisplayMeasureTripShopperList(this);");
            Measurelist = [];
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
        }
        //clearTimeout(timer);
        FilterSelectionLimitText();
        $("#retailer-measure div[level-id='2'] ul li").removeClass('DNI');
        $("#AdvFilterDivId div[level-id='1'] ul li").show();
        e.stopImmediatePropagation();
    });
    $(document).on("click", "#guest-visit-toggle", function () {
        UpdateVisitGuestFilters();
        HideOrShowFilters();
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
    });
    $("#SubMenuHeader .SubMenu .SubItem").mouseleave(function (e) {
        $("#SubMenuHeader .SubMenu .SubItem .Item").hide();
        $("#SubMenuHeader .SubMenu .SubItem span").removeClass("Active");
        $("#SubMenuHeader .SubMenu .SubItem span").addClass("InActive");
        $("#SubMenuHeader .SubMenu .SubItem .Menu").removeClass("Menu-Active");
        $("#SubMenuHeader .SubMenu .SubItem .Menu .MenuTitle div").removeClass("downarrw_active");
        $("#SubMenuHeader .SubMenu .SubItem .Menu .MenuTitle div").addClass("downarrw");       
    });
    $("#ToShowShopper").click(function (index, obj) {
        $("#ToShowShooperAndTrips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(this);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
        $("#ToShowShooperAndTrips *").removeClass("Selected");
        $("#ToShowShooperAndTrips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(this).addClass("Selected");
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
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").text($(this).text().trim().toUpperCase());
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").css("width", "270px");
        sVisitsOrGuests = "2";
        TabType = "shopper";
        HideOrShowFilters();
        if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name != "Monthly +") {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId_trips ul li[name='MONTHLY +'").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
            $("#ToShowShopper").trigger("click");
        }
        ShowChannelRetailerVariables();
        SetScroll($(".rgt-cntrl-advanced-analytics-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        //GetDefaultFrequency();
    });

    $("#ToShowTrip").click(function (index, obj) {
        $("#ToShowShooperAndTrips .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(this);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
        $("#ToShowShooperAndTrips *").removeClass("Selected");
        $("#ToShowShooperAndTrips *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(this).addClass("Selected");
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
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").text($(this).text().trim().toUpperCase());
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").show();
        $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel1").css("width", "270px");
        sVisitsOrGuests = "1";
        TabType = "trips";
        HideOrShowFilters();
        if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() != "total visits") {
            SelectedFrequencyList = [];
            $("#RightPanelPartial #frequency_containerId_trips ul li[name='TOTAL VISITS']").trigger("click");
            $("#Advanced-Analytics-Select-Variable").trigger("click");
            $("#ToShowTrip").trigger("click");
        }

        ShowChannelRetailerVariablesTrips();
        SetScroll($(".rgt-cntrl-advanced-analytics-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        //GetDefaultFrequency();
    });

    $(document).on("click", "#LeftPanel .FilterMenu", function (e) {
        ClosePopups();
        if ($(this).attr("id") == "MeasureType" && (currentpage.indexOf("chart") > -1)) {
            $(".popup-menu").hide();
            $(".Sub-Lavel").hide();
            //23-11-17
            var AllMeasuresSF =[];
            AllMeasuresSF = $.extend(true, [], AllMeasures);
            for (var j = 0; j < Grouplist.length; j++) {
                _.each(_.filter(AllMeasuresSF), function(item) {
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

            $("#MeasureTypeShopperTripHeader *").removeClass("Selected");
            $("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderMain *").removeClass("Selected");
            $("#MeasureTypeHeaderMain *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContent *").removeClass("Selected");
            $("#MeasureTypeHeaderContent *").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#MeasureTypeHeaderContentSubLevel *").removeClass("Selected");
            $("#MeasureTypeHeaderContentSubLevel *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }


        UpdatePopupScroll();
        //ShowChannelRetailerVariables();
        if ($(".FilterPopup." + $(this).attr("id")).css('display') == 'none') {
            //$(".FilterPopup").css("left", $(this).width());
            //$(".FilterPopup").css("left", (55 / window.innerWidth) * 100 + "%");
            ClosePopups();
            $(".rgt-cntrl-SubFilter-Conatianer").hide();
            $(".rgt-cntrl-frequency").hide();
            $("#LeftPanel .FilterPopup").removeClass("ActiveFilter");
            $(this).addClass("ActiveFilter");
            $("." + $(this).attr("id")).show();
            updateSearch($(this));
            if ($(this).attr("id") == "AdvancedFilters") {
                //23-11-17
                AllDemographicsSF = $.extend(true, [], AllDemographics);
                if (Grouplist.length > 0) {
                    for (var j = 0; j < Grouplist.length; j++) {
                        _.each(_.filter(AllDemographicsSF), function(item) {
                            if (item.split("|")[2] == Grouplist[j].parentName)
                                AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                        });
                    }
                }
                if (Measurelist.length > 0) {
                    for (var j = 0; j < Measurelist[0].metriclist.length; j++) {
                        _.each(_.filter(AllDemographicsSF), function(item) {
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
            }
            if ($(this).find(".demograhicFitr_img")) { $(this).find(".demograhicFitr_img").css("background-position", "-241px -159px") }
            if ($(this).find(".establishment_img")) {
                $(this).find(".establishment_img").css("background-position", "-466px -147px");
            }
            if ($(this).find(".retailer_img")) { $(this).find(".retailer_img").css("background-position", "-663px -158px") }
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

        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").hide();
        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").hide();
        $("#GroupTypeHeaderContent").find(".Selected").removeClass("Selected");
        $("#GroupTypeHeaderContent").find(".ArrowContainerdiv").css("background-color", "#58554D");

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
        FilterSelectionLimitText();
        //SetFilterLayerScroll();
        if ($(this).attr("id") == "GroupType" && isFirstTimePageLoad) {
            setTimeout(function () {
                $(".GroupType").show();
            }, 10);
            isFirstTimePageLoad = false;
        }
        if ($(this).attr("id") == "MeasureType" && (currentpage.indexOf("chart") > -1)) {
            $("#MeasureTypeHeaderMainTrip").show();
            $("#MeasureTypeHeaderMainTrip ul li").show();
        }
        //updateSearch($(this));
        e.stopImmediatePropagation();
    });
    function ShowChannelRetailerVariables() {
        $("#advanced-analytics-Channel-Trips").hide();
        $("#advanced-analytics-Retailer-Trips").hide();
        $("#advanced-analytics-Channel-Shopper").hide();
        for (var i = 0; i < Comparisonlist.length; i++) {
            if (Comparisonlist[i].Name.toLocaleLowerCase().indexOf("channels|") > -1) {
                $("#advanced-analytics-Retailer-Shopper").hide();
                $("#advanced-analytics-Channel-Shopper").show();
                return true;
                break;
            }
        }
        $("#advanced-analytics-Retailer-Shopper").show();
    }

    function ShowChannelRetailerVariablesTrips() {
        $("#advanced-analytics-Channel-Shopper").hide();
        $("#advanced-analytics-Retailer-Shopper").hide();
        $("#advanced-analytics-Channel-Trips").hide();
        for (var i = 0; i < Comparisonlist.length; i++) {
            if (Comparisonlist[i].Name.toLocaleLowerCase().indexOf("channels|") > -1) {
                $("#advanced-analytics-Retailer-Trips").hide();
                $("#advanced-analytics-Channel-Trips").show();
                return true;
                break;
            }
        }
        $("#advanced-analytics-Retailer-Trips").show();
    }
    $(document).click(function (e) {
        isPopupVisible = false;
        var popup = $(".FilterPopup");
        var popup_menu = $(".popup-menu");
        var popup_block = $("#PopupBlock");
        if (popup_menu.is(e.target)) { return; }
        if ((!$('.FilterPopup').is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0)
            && (!$('.popup-menu').is(e.target) && !popup_menu.is(e.target) && popup_menu.has(e.target).length == 0)) {
            if (sClosePopupStatus == 0)
                ClosePopups();
        }

        if (!$(".adv-fltr-freq-container *").is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) {
            $(".rgt-cntrl-frequency").hide();
            $(".adv-fltr-freq-container").removeClass("TileActive");
            $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").hide();
            $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel3").hide();
            $(".rgt-cntrl-frequency-Conatiner-SubLevel1").hide();
            $(".rgt-cntrl-frequency-Conatiner-SubLevel2").hide();
        }
        if (!$(".adv-fltr-ordertype-container *").is(e.target) && !popup.is(e.target) && popup.has(e.target).length == 0) {
            $(".rgt-cntrl-ordertype").hide();
            $(".adv-fltr-ordertype-container").removeClass("TileActive");
            $(".AdvancedFiltersDemoHeading #ordertypeFilterHeadingLevel2").hide();
            $(".AdvancedFiltersDemoHeading #ordertypeFilterHeadingLevel3").hide();
            $(".rgt-cntrl-ordertype-Conatiner-SubLevel1").hide();
            $(".rgt-cntrl-ordertype-Conatiner-SubLevel2").hide();
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
        e.stopImmediatePropagation();
    });

    $("#submitButton").click(function Submit() {
        //LogSelection();
        if (($(".adv-fltr-showhide-txt").text().toLowerCase().trim() == "show less") && ((currentpage.indexOf("tbl") > -1) || (currentpage.indexOf("chart") > -1)))
            $(".adv-fltr-showhide-txt").trigger("click");

        $(".adv-fltr-details").show();
        //$("#adv-fltr-freq").show();
        //$("#RightPanelPartial").show();
        if (currentpage == "hdn-analysis-withintrips" || currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive" || currentpage.indexOf("tbl") > -1 || currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
            if (currentpage == "hdn-e-commerce-tbl-comparesites") {
                if (!Validate_Sites_Charts()) {
                    return false;
                }
            }
            else if (currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
                if (!Validate_Group_Site()) {
                    return false;
                }
            }
            
            $(".advance-filters").css("display", "block");
            $(".adv-fltr-details").css("margin-top", "0%");
        }
        if (currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
            $(".rgt-cntrl-frequency-Conatiner ul li[name='MAIN STORE/FAVORITE STORE']").show();
            $(".rgt-cntrl-frequency-Conatiner ul li[name='TOTAL VISITS']").hide();

            $("#adv-fltr-freq").show();
            $(".adv-fltr-details").show();
            HideOrShowFilters();
        }
        if ((currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") && (sVisitsOrGuests == "1" || TabType == "trips" || TabType == "online"))
            $(".adv-fltr-suboptions-list-container").show();
        else if ((currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") && (sVisitsOrGuests == "2" || TabType == "shopper"))
            $(".adv-fltr-suboptions-list-container").hide();
        sRemovedLegendPosition = [];
        if ((document.getElementById('adv-fltr visitsId') != null) || (document.getElementById('adv-fltr visitsTrendId') != null)) {
            var navbarwidth = document.getElementById('adv-fltr visitsId').offsetWidth;
            if (currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-tbl-retailerdeepdive") {
                if (ModuleBlock.toUpperCase() == "TREND")
                    navbarwidth = document.getElementById('adv-fltr visitsTrendId').offsetWidth;
                else
                    navbarwidth = document.getElementById('adv-fltr visitsId').offsetWidth;
            }
            if (currentpage.indexOf('commerce')) {
                var sWidth1 = (navbarwidth * 32.6) / 100;
                $(".width-3").css("width", sWidth1);
            }
            if (currentpage == "hdn-tbl-beveragedeepdive" || currentpage == "hdn-tbl-comparebeverages")
                var sWidth = (navbarwidth * 19.19) / 100;
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
    LoadFilters();
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
    if (currentpage == "hdn-e-commerce-chart-sitedeepdive" || currentpage == "hdn-e-commerce-chart-comparesites")
        $("#LeftPanel #MeasureType").show();
    if (currentpage == "hdn-e-commerce-chart-sitedeepdive")
        ModuleBlock = "PIT";
    else
        ModuleBlock = "";
    if (currentpage.indexOf("commerce") > 0) {
        $("#LeftPanel #SiteFilters").show();
        $("#LeftPanel #Retailers").hide();
        if (currentpage.indexOf("deep") > 0 && currentpage.indexOf("chart") > 0)
            $("#LeftPanel #GroupType").show();
    }

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
        if ($(this).hasClass("Selected") || $(this).find("div").hasClass("Selected")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
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
    $(".dynpos").hover(function () {
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
    $(".dynpos1").hover(function () {
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
            if ($(this).attr("id") == "btnClearAll") {
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
var numberOfAjaxRequests = 0;
$(document).ajaxSend(function () {
    numberOfAjaxRequests++;
    ShowLoader();
});
$(document).ajaxComplete(function () {
    numberOfAjaxRequests--;
    if (numberOfAjaxRequests == 0) {
        HideLoader();
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
}
function ClosePopups() {
    if (sClosePopupStatus == 0) {
        if (!$(".MeasureType").is(':visible')) {
            $(".popup-menu").hide();
            $(".Sub-Lavel").hide();
        }
        $(".adv-fltr-Chnl").removeClass("TileActive");
        $(".rgt-cntrl-frequency-Conatiner .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        $(".rgt-cntrl-frequency-Conatiner-SubLevel1 .sidearrw_OnCLick").each(function (i, j) {
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
        $(".rgt-cntrl-frequency-Conatiner .sidearrw_OnCLick").each(function (i, j) {
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
        $(".MonPurordertype").removeClass("TileActive");
        $(".adv-fltr-freq-container").removeClass("TileActive");
        $(".rgt-cntrl-frequency").hide();
        $(".rgt-cntrl-ordertype").hide();
        $(".rgt-cntrl-trips-frequency").hide();
        $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").hide();

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
        //$(".popup-menu").hide();
        //$(".Sub-Lavel").hide();
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
        $(".sites_img").css("background-position", "-2808px -159px");

        $(".comparission_img").css("background-position", "-1277px -149px");
        $(".timeperiod_img").css("background-position", "-1291px -159px");
        $(".grouptype_img").css("background-position", "-421px -159px");
        $(".measure_img").css("background-position", "-611px -158px");
        $(".Freq_img").css("background-position", "-55px -160px");
        $("#SecondaryAdvancedFilterContent div[name=Other]").find(".Selected").removeClass("Selected");
        $("#SecondaryAdvancedFilterContent div[name=Other]").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel2").hide();
        $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").hide();
        $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").hide();
        $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").hide();
        $(".AdvancedFiltersDemoHeading #beverageHeadingLevel0").hide();
        $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").hide();
        $(".AdvancedFiltersDemoHeading #beverageHeadingLevel3").hide();

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
        //$("#ThirdGeographyFilterList div[onclick='DisplayFourthLevelGeoFilter(this);' i]").find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        //$("#ThirdGeographyFilterList div[onclick='DisplayFourthLevelGeoFilter(this);' i]").removeClass("Selected");
        $("#ThirdGeographyFilterList div .RemoveSelectionClass").find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        $("#ThirdGeographyFilterList div .RemoveSelectionClass").removeClass("Selected");
        $("#ThirdGeographyFilterList div .RemoveSelectionClass").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#rgt-cntrl-chnl-SubFilter1 div[onclick='DisplayChannelDemoFilter(this);' i]").removeClass("Selected");
        //$("#rgt-cntrl-SubFilter1 div[onclick='DisplayVisistThirdLevelDemoFilter(this);' i]").removeClass("Selected");
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
        $("#grouptypeHeadingLevel4").hide();
        $("#DemoHeadingLevel4").hide();
        $("#advanced-analytics-Retailer-Trips").hide();
        $("#advanced-analytics-Retailer-Shopper").hide();
        $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
        $("#advancedanalyticsHeadingLevel1").hide();
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
        $("#PrimeGroupTypeHeaderContent ul li").removeClass("Selected");
        $("#PrimeGroupTypeHeaderContent ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").removeClass("sidearrw_OnCLick");
        $("#PrimeGroupTypeHeaderContent .lft-popup-ele-next").addClass("sidearrw");
        $("#TotalMeasureHeadingLevel3").hide();
        $("#GroupTypeHeaderContent").hide();
        //$(".LowSample-popup").hide();
        $(".SiteFilters div[onclick='DisplaySecondarySiteFilter(this);']").removeClass("Selected");
        $(".SiteFilters div[onclick='DisplaySecondarySiteFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#MeasureTypeShopperTripHeader *").removeClass("Selected");
        //$("#MeasureTypeShopperTripHeader *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#MeasureTypeHeaderMain *").removeClass("Selected");
        //$("#MeasureTypeHeaderMain *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#MeasureTypeHeaderContent *").removeClass("Selected");
        //$("#MeasureTypeHeaderContent *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#MeasureTypeHeaderContentSubLevel *").removeClass("Selected");
        //$("#MeasureTypeHeaderContentSubLevel *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    }
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
                //data.EcommTimePeriodList.slice(0, n).remove(); 
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
    label = [];
    $("#TimeBlock ul li").removeClass("Selected");
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
            TimePeriod_UniqueId = TimePeriodId + "|" + (CurrentTimePeriodlist[parseInt(ui.value)].Id);
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
//added by Nagaraju for cashing 
//Date: 13-04-2017
//function LoadFilters() {
//    ShowLoader();
//    window.app.db.get(1, function (saveData) {
//        if (saveData !== null && saveData !== undefined) {
//            var data = JSON.parse(saveData.value);
//            PrepareFilters(data);
//            HideLoader();
//        }
//        else {
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
//}
//function LoadFilters() {
//    if (sessionStorage.getItem('E-filters') === null) {
//        jQuery.ajax({
//            type: "POST",
//            url: $("#URLCommonEcommerce").val(),
//            async: true,
//            data: "",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//                PrepareFilters(data);
//            },
//            error: function (xhr, status, error) {
//                showMessage(xhr.responseText);
//            }
//        });
//    }
//    else {
//        var data = JSON.parse(sessionStorage.getItem('E-filters'));
//        PrepareFilters(data);
//    }
//}
function PrepareFilters2(data) {
    sFilterData = data;
    sEcommerceData = data;
    DefaultGeolist = SelecDefaultGeography(data);
    LoadTimePeriod(data);
    //LoadAdvancedFilters(data);
    //LoadSecondaryAdvancedFilters(data);
    LoadAdvancedFilterFromString(data);
    LoadGroupsPrimeFilters(data);  
    //added by Nagaraju for Groups Date: 11-04-2017   
    $("#GroupTypeGeoContentSub").before(data.Ecomm_GroupsFilterlist.SearchObj.HTML_String);
    AllTypes = data.Ecomm_GroupsFilterlist.SearchObj.SearchItems;
    //add Geography
    $("#GroupTypeHeaderContent ul").append("<li style=\"\" primefiltertype=\"Demographics\" filtertype=\"Demographics\" name=\"Geography\" dbname=\"Geography\" uniqueid=\"null\" shopperdbname=\"null\" tripsdbname=\"null\" class=\"gouptype\" onclick=\"SelecGroup(this);\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%;\" class=\"lft-popup-ele-label\" filetrtypeid=\"1\" id=\"100\" type=\"Main-Stub\" name=\"Geography\">Geography</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>");
    $("#GroupTypeContent").append("<div class=\"DemographicList\" id=\"100\" name=\"Geography\" fullname=\"Geography\" style=\"overflow-y: auto;\"></div>");

    UpdateDefaultGeography(data);
    SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);
    //----->
    //-------->

    //LoadGroupTypeHeaderName(data);
    //LoadGroupTypeNames(data);
    //LoadMeasureTypeMain(data);
    //LoadMeasureTypeHeaderName(data);
    //LoadMeasureTypeNames(data);
    //LoadMeasureShopperTripLevel();
    //LoadMeasure(data);

    //LoadMeasureShopperTripLevel();
    //LoadMeasure(data);
    //added by Nagaraju for Measure Date: 11-04-2017
    $("#MeasureTypeHeaderMainTrip").append(data.TripEcommerceMeasures[0].html1);
    $("#MeasureTypeHeaderContentTrip").append(data.TripEcommerceMeasures[0].html2);
    $("#MeasureTypeHeaderContentSubLevelTrip").append(data.TripEcommerceMeasures[0].html3);
    $("#MeasureTypeContentTrip").append(data.TripEcommerceMeasures[0].html4);

    //$("#MeasureTypeHeaderMainShopper").append(data.shopperEcommerceMeasures[0].html1);
    //$("#MeasureTypeHeaderContentShopper").append(data.shopperEcommerceMeasures[0].html2);
    //$("#MeasureTypeHeaderContentSubLevelShopper").append(data.shopperEcommerceMeasures[0].html3);
    //$("#MeasureTypeContentShopper").append(data.shopperEcommerceMeasures[0].html4);

    $("#MeasureTypeHeaderMainTrip").show();
    $("#MeasureTypeHeaderMainTrip ul li").show();

    AllMeasures = data.TripEcommerceMeasures[0].SearchObj.SearchItems;
    ShopperMeasureSearchItems = data.TripEcommerceMeasures[0].SearchObj.ShopperSearchItems;
    TripsMeasureSearchItems = data.TripEcommerceMeasures[0].SearchObj.TripsSearchItems;

    //AllMeasures = AllMeasures.concat(data.shopperEcommerceMeasures[0].SearchObj.SearchItems);
    SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);

    //LoadSiteFilters(data);
    //added by Nagaraju for Site html string
    //Date: 13-04-2017 
    $(".SiteFilters").append(data.SiteHTMLFilters.SearchObj.HTML_String);
    AllSites = data.SiteHTMLFilters.SearchObj.SearchItems;
    LoadRightFilterE();
    LoadRightPanelEcommerce(data);
    LoadFrequencyFilter(data);
    LoadEcomTripsFrequencyFilter(data);
    LoadTripsFrequencyFilter(data);
    LoadBeverageSlectionTypeFilter(data);

    if (currentpage == "hdn-report-compareretailersshoppers" || currentpage == "hdn-report-retailersshopperdeepdive")
        SearchFilters("Retailer", "Search-Retailers", "Retailer-Search-Content", AllPriorityRetailers);
    else
        SearchFilters("Retailer", "Search-Retailers", "Retailer-Search-Content", AllRetailers);
    SearchFilters("Beverage", "Search-Beverages", "Beverage-Search-Content", AllBeverages);
    SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
    SearchFilters("DemographicFilters", "Search-AdvancedFilters", "AdvancedFilter-Search-Content", AllDemographics);
    SearchFilters("Type", "Search-Group-Type", "Group-Type-Search-Content", AllTypes);

    PrepareSearch("Frequency", "Search-FrequencyFilters", "FreqFilter-Search-Content", AllFrequency);
    PrepareSearch("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);

    SearchFilters("Sites", "Search-Site-AdvancedFilters", "AdvancedFilter-Site-Search-Content", AllSites);

    SetDefaultValues();
    SetFilterLayerScroll();

    if (currentpage.indexOf("deepdive") > -1)
        LoadMeasureShopperTripLevel();

    $(".Lavel ul li").mouseover(function () { if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected"))) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A"); });
    $(".Lavel ul li").mouseleave(function () {
        $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected"))) {
            $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
        }
    });
    $(".lft-popup-ele").mouseover(function () {
        if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
    });
    $(".lft-popup-ele").mouseleave(function () {
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
    });
}
function LoadLeftPanelFrequencyFilter(data) {
    html = "";
    AllLeftPanelFrequency = [];
    var frequencyfilterlist = [];
    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }
    if (currentpage == "hdn-analysis-acrossshopper")
        frequencyfilterlist = data.BGMFrequencylist;
    else
        frequencyfilterlist = data.ReportFrequencylist;

    if (data != null) {
        html += "<ul>";
        for (var i = 0; i < frequencyfilterlist.length; i++) {
            var object = frequencyfilterlist[i];
            html += "<li Name=\"" + object.Name + "\" style=\"display:table;\">";
            html += "<div onclick=\"SelectFrequency(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.Name + "\" data-isselectable=\"true\">" + object.Name + "</span></div>";
            html += "</li>";
            AllLeftPanelFrequency.push(object.Id + "|" + object.Name);
        }
        html += "</ul>";
        $("#left-panel-frequency").html("");
        $("#left-panel-frequency").append(html);
    }
    PrepareSearch("Left-Panel-Frequency", "Search-Left-Panel-FrequencyFilters", "FreqFilter-Left-Search-Content", AllLeftPanelFrequency);
}
function LoadChannelOrCategory(data) {
    html = "";
    var index = 0;
    var ulclose = false;
    if (data != null) {
        //add lavels
        for (var i = 0; i < data.Channel.Lavels.length; i++) {
            $("<div class=\"Retailer Lavel Lavel" + data.Channel.Lavels[i] + " Sub-Lavel\" style=\"display: none;\"></div>").appendTo(".Retailers");
        }
        html += "<ul>";
        for (var i = 0; i < data.Channel.ChannelOrCategorylist.length; i++) {
            var object = data.Channel.ChannelOrCategorylist[i];
            html += "<li>";
            html += "<div class=\"FilterStringContainerdiv\">";
            html += "<span class=\"img-retailer\"></span>";
            html += "<span id=\"" + AllRetailers.length + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\" isselectable=\"" + object.IsSelectable + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"Comparison\" onclick=\"SelectComparison(this);\">" + object.Name + "</span>";
            html += "<div class=\"ArrowContainerdiv\"><span Name=\"" + object.Name + "\" lavels=\"" + object.ChannelOrCategoryLavel.length + "\" class=\"sidearrw\" onclick=\"DisplayComparisonRetailer(this);\"></span></div>";
            if (!IsItemExist(object.Name, AllRetailers))
                AllRetailers.push(AllRetailers.length + "|" + object.Name);

            html += "</div>";
            html += "</li>";

        }
        html += "</ul>";

        $("#ChannelOrCategoryContent").html("");
        $("#ChannelOrCategoryContent").html(html);
    }
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
                                    html = "<div class=\"RetailerOrBrand\" id=\"" + AllRetailers.length + "\" Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" DBName=\"" + data.Channel.ChannelOrCategorylist[i].DBName + "\" style=\"display:none;\"><ul>";
                                    index++;
                                }
                                html += "<li class=\"Comparison\" parentLevelId=\"" + data.Channel.ChannelOrCategorylist[i].Id + "\" id=\"" + AllRetailers.length + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectComparison(this);\" PriorityId=\"" + object.PriorityId + "\">" + object.Name + "</li>";
                            }
                            else {
                                if (sindex == 0) {
                                    shtml = "<div class=\"RetailerOrBrand\" id=\"" + AllRetailers.length + "\" Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" DBName=\"" + data.Channel.ChannelOrCategorylist[i].DBName + "\" style=\"display:none;\"><ul>";
                                    sindex++;
                                }
                                shtml += "<li class=\"Comparison\" parentLevelId=\"" + data.Channel.ChannelOrCategorylist[i].Id + "\" id=\"" + AllRetailers.length + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectComparison(this);\" PriorityId=\"" + object.PriorityId + "\">" + object.Name + "</li>";
                            }
                        }
                        else {
                            if (ssindex == 0)
                                html = "<div class=\"RetailerOrBrand\" id=\"" + AllRetailers.length + "\" Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" DBName=\"" + data.Channel.ChannelOrCategorylist[i].DBName + "\" style=\"display:none;\"><ul>";
                            html += "<li class=\"Comparison\" parentLevelId=\"" + data.Channel.ChannelOrCategorylist[i].Id + "\" id=\"" + AllRetailers.length + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectComparison(this);\">" + object.Name + "</li>";
                            ssindex++;
                        }
                        if (!IsItemExist(object.Name, AllRetailers))
                            AllRetailers.push(AllRetailers.length + "|" + object.Name);
                    }

                    if (sindex > 0) {
                        html += "</ul></div>";
                        shtml += "</ul></div>";
                        $(".Retailers .Lavel" + lavel + "").append("<div Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\" class=\"priorityclass\" style=\"display:inline-block;\">--------------------PRIORITY--------------------</div>");
                        $(".Retailers .Lavel" + lavel + "").append(html);
                        if (currentpage != "hdn-report-compareretailersshoppers" && currentpage != "hdn-report-retailersshopperdeepdive") {
                            $(".Retailers .Lavel" + lavel + "").append("<div Name=\"" + data.Channel.ChannelOrCategorylist[i].Name + "\"  class=\"priorityclass\" style=\"display:inline-block;\">-----------------NON-PRIORITY-----------------</div>");
                            $(".Retailers .Lavel" + lavel + "").append(shtml);
                        }
                    }
                    else {
                        html += "</ul></div>";
                        $(".Retailers .Lavel" + lavel + "").append(html);
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
            $("<div class=\"Beverage Lavel Lavel" + data.Category.Lavels[i] + " Sub-Lavel\" style=\"display: none;\"></div>").appendTo(".Beverages");
        }
        html += "<ul>";
        for (var i = 0; i < data.Category.CategoryOrBeveragelist.length; i++) {
            var object = data.Category.CategoryOrBeveragelist[i];
            var sImageClassName = _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; })[0].imagePosition : "";
            html += "<li>";
            html += "<div class=\"FilterStringContainerdiv\">";
            html += "<span style=\"background-position:" + sImageClassName + "\" class=\"img-retailer\"></span>";


            html += "<span id=\"" + AllBeverages.length + "\" type=\"Main-Stub\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"Comparison\" onclick=\"SelectBevComparison(this);\">" + object.Name + "</span>";

            html += "<div class=\"ArrowContainerdiv\"><span Name=\"" + object.Name + "\" lavels=\"" + object.CategoryOrBeverageLavel.length + "\" class=\"sidearrw\" onclick=\"DisplayBevComparisonRetailer(this);\"></span></div>";

            if (!IsItemExist(object.Name, AllBeverages))
                AllBeverages.push(AllBeverages.length + "|" + object.Name);

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
                    html = "<div class=\"RetailerOrBrand\" id=\"" + AllBeverages.length + "\" Name=\"" + data.Category.CategoryOrBeveragelist[i].Name + "\" DBName=\"" + data.Category.CategoryOrBeveragelist[i].DBName + "\" style=\"display:none;\"><ul>";
                    for (var j = 0; j < data.Category.CategoryOrBeveragelist[i].CategoryOrBeverageLavel[lavel].LavelRetailerlist.length; j++) {
                        var object = data.Category.CategoryOrBeveragelist[i].CategoryOrBeverageLavel[lavel].LavelRetailerlist[j];


                        html += "<li class=\"Comparison\" id=\"" + AllBeverages.length + "\" Name=\"" + object.Name + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" onclick=\"SelectBevComparison(this);\">" + object.Name + "</li>";

                        if (!IsItemExist(object.Name, AllBeverages))
                            AllBeverages.push(AllBeverages.length + "|" + object.Name);
                    }
                    html += "</ul></div>";
                    $(".Beverages .Lavel" + lavel + "").append(html);
                }
            }
        }
    }
}
function ShowGroup(obj) {
    $("#PrimeGroupTypeHeaderContent ul li").removeClass("Selected");
    $("#PrimeGroupTypeHeaderContent ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    //$(obj).addClass("Selected");

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

    $("#grouptypeHeadingLevel2").html($(obj).attr("PrimeFilterType").toLowerCase());
    $("#grouptypeHeadingLevel2").show();
    UpdatePopupScroll();
}
function LoadGroupTypeHeaderName(data) {
    //LoadGroupsPrimeFilters(data);
    html = "";
    var index = 0;
    var datalist = {};
    if (currentpage.indexOf("beverage") > -1) {
        if (currentpage != "hdn-analysis-acrosstrips") {
            dGeo = SelecGeography();
            datalist = (data.TripGroupTypelist).concat(dGeo);//data.EcommShopperGroupTypeList;
        }
        else if (currentpage == "hdn-analysis-acrosstrips") {
            if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                dGeo = SelecGeography();
                datalist = (data.TripGroupTypelist).concat(dGeo);//data.EcommShopperGroupTypeList;
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
            datalist = (data.EcommShopperGroupTypeList).concat(dGeo);//data.EcommShopperGroupTypeList;
        }
        else if (currentpage == "hdn-analysis-acrosstrips") {
            if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                dGeo = SelecGeography();
                datalist = (data.EcommShopperGroupTypeList).concat(dGeo);//data.EcommShopperGroupTypeList;
            }
            else
                datalist = (data.EcommShopperGroupTypeList).concat(dGeo);//data.EcommShopperGroupTypeList;
        }
        else
            datalist = data.EcommShopperGroupTypeList;
        //data.EcommShopperGroupTypeList//[data.EcommShopperGroupTypeList.length - 1] = d[0];
        //data.EcommShopperGroupTypeList = d.concat(data.EcommShopperGroupTypeList);
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
                html += "<li style=\"display:none;\" PrimeFilterType=\"" + (object.Name == "Geography" ? "Demographic" : object.PrimeFilterType) + "\" FilterType=\"" + (object.Name == "Geography" ? "Demographic" : object.FilterType) + "\" Name=\"" + object.Name.replace("'", "") + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" class=\"gouptype\" onclick=\"SelecGroup(this);\">";
                html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
                html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" Name=\"" + object.Name.replace("'", "") + "\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";

                html += "</div>";
                html += "</li>";
            }
        }
        html += "</ul>";
        $("#GroupTypeHeaderContent").html("");
        $("#GroupTypeHeaderContent").html(html);
        SetScroll($("#GroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);


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
                                    thirdLevelhtml += "<div MetricId=\"" + object.MetricId + "\" style=\"display: none;\" id=\"" + object.Id + "\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + objthirdlavel.Name + "\" MericName=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + objthirdlavel.isGeography + "\" FullName=\"" + objthirdlavel.FullName + "\" DBName=\"" + objthirdlavel.DBName + "\" UniqueId=\"" + objthirdlavel.UniqueId + "\" shopperdbname=\"" + objthirdlavel.ShopperDBName + "\" tripsdbname=\"" + objthirdlavel.TripsDBName + "\"  data-id=\"" + objthirdlavel.Id + "\" id=" + objthirdlavel.Id + "-" + objthirdlavel.MetricId + "-" + objthirdlavel.ParentId + " Name=\"" + objthirdlavel.Name + "\" parent=\"" + objthirdlavel.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + objthirdlavel.Name + "</span></div>";
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
                            if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                            else
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                            if (!IsItemExist(object.Name, AllTypes))
                                AllTypes.push(object1.UniqueId + "|" + object1.Name);
                        }
                        else {
                            if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
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
                                    thirdLevelhtml += "<div MetricId=\"" + object.MetricId + "\" style=\"display: none;\" id=\"" + object.Id + "\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + objthirdlavel.Name + "\" MericName=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + objthirdlavel.isGeography + "\" FullName=\"" + objthirdlavel.FullName + "\" DBName=\"" + objthirdlavel.DBName + "\" UniqueId=\"" + objthirdlavel.UniqueId + "\" shopperdbname=\"" + objthirdlavel.ShopperDBName + "\" tripsdbname=\"" + objthirdlavel.TripsDBName + "\"  data-id=\"" + objthirdlavel.Id + "\" id=" + objthirdlavel.Id + "-" + objthirdlavel.MetricId + "-" + objthirdlavel.ParentId + " Name=\"" + objthirdlavel.Name + "\" parent=\"" + objthirdlavel.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + objthirdlavel.Name + "</span></div>";
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
                            if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                            else
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                            if (!IsItemExist(object.Name, AllTypes))
                                AllTypes.push(object1.UniqueId + "|" + object1.Name);
                        }
                        else {
                            if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
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
                                    if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    AllDemographicsSub.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else {
                                    if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
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
            datalist = (data.TripGroupTypelist).concat(dGeo);//data.EcommShopperGroupTypeList;
        }
        else if (currentpage == "hdn-analysis-acrosstrips") {
            if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                dGeo = SelecGeography();
                datalist = (data.TripGroupTypelist).concat(dGeo);//data.EcommShopperGroupTypeList;
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
            datalist = (data.EcommShopperGroupTypeList).concat(dGeo);//data.EcommShopperGroupTypeList;
        }
        else if (currentpage == "hdn-analysis-acrosstrips") {
            if (Geographylist.length > 0 && Geographylist[0].Name == "Total") {
                dGeo = SelecGeography();
                datalist = (data.EcommShopperGroupTypeList).concat(dGeo);//data.EcommShopperGroupTypeList;
            }
            else
                datalist = (data.EcommShopperGroupTypeList).concat(dGeo);//data.EcommShopperGroupTypeList;
        }
        else
            datalist = data.EcommShopperGroupTypeList;
        //data.EcommShopperGroupTypeList//[data.EcommShopperGroupTypeList.length - 1] = d[0];
        //data.EcommShopperGroupTypeList = d.concat(data.EcommShopperGroupTypeList);
        //End
    }
    if (datalist != null) {

        for (var i = 0; i < datalist.length; i++) {
            html += "<div class=\"DemographicList\" id=\"" + datalist[i].Id + "\" Name=\"" + datalist[i].Name.replace("'", "") + "\" FullName=\"" + datalist[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
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
                                        thirdLevelhtml += "<div MetricId=\"" + object.MetricId + "\" style=\"display: none;\" id=\"" + object.Id + "\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + objthirdlavel.Name + "\" MericName=\"" + object.Name.replace("'", "") + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + objthirdlavel.isGeography + "\" FullName=\"" + objthirdlavel.FullName + "\" DBName=\"" + objthirdlavel.DBName + "\" UniqueId=\"" + objthirdlavel.UniqueId + "\" shopperdbname=\"" + objthirdlavel.ShopperDBName + "\" tripsdbname=\"" + objthirdlavel.TripsDBName + "\"  data-id=\"" + objthirdlavel.Id + "\" id=" + objthirdlavel.Id + "-" + objthirdlavel.MetricId + "-" + objthirdlavel.ParentId + " Name=\"" + objthirdlavel.Name + "\" parent=\"" + objthirdlavel.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + objthirdlavel.Name + "</span></div>";
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

    $(".Geotooltipimage, #MeasureTypeShopperTripHeader li[name='Shopper']").hover(function () {
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
    if (sResult.length > 0)
        sResult = _.forEach(sResult, function (i) {
            _.forEach(i.SecondaryAdvancedFilterlist, function (j) {
                if (j.SecondaryAdvancedFilterlist == null || j.SecondaryAdvancedFilterlist.length <= 0) {
                    _.forEach(sFilterData.GeographyList, function (k) {
                        if (j.Name == "Total")
                            j.UniqueId = "4000";
                        else if (j.Name == k.MetricItem)
                            j.UniqueId = k.UniqueId;
                        j.isGeography = "true";
                    });
                }
                else {
                    _.forEach(j.SecondaryAdvancedFilterlist, function (l) {
                        _.forEach(sFilterData.GeographyList, function (k) {
                            if (l.Name == "Total")
                                l.UniqueId = "4000";
                            else if (l.Name == k.MetricItem)
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
    $(obj).addClass("Selected");
    $("#GroupTypeContentSub .DemographicList").hide();
    $("#GroupTypeContentSub").show();
    $("#GroupTypeContentSub div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").text($(obj).find(".lft-popup-ele-label").attr("name").toUpperCase() == "PRODUCTS CAME FROM " ? "PRODUCT'S CAME FROM " : $(obj).find(".lft-popup-ele-label").attr("name"));
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel3").css("width", "287px");
    SetScroll($("#GroupTypeContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    UpdatePopupScroll();
    ShowSelectedFilters();
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
    $(obj).addClass("Selected");
    $("#GroupTypeGeoContentSub .DemographicList").hide();
    $("#GroupTypeGeoContentSub").show();
    $("#GroupTypeGeoContentSub div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").text($(obj).find(".lft-popup-ele-label").attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").show();
    $(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel4").css("width", "287px");
    SetScroll($("#GroupTypeGeoContentSub"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    UpdatePopupScroll();
    ShowSelectedFilters();
}
function UpdatePopupScroll() {
    $(".GroupType").css("width", "auto").css("overflow", "hidden");
    if (($(".GroupType").width() + 10) > $("#RightPanel").width()) {
        $(".GroupType").css("width", $("#RightPanel").width()).css("overflow-y", "hidden").css("overflow-x", "auto");
        SetScroll($(".GroupType"), left_scroll_bgcolor, 0, 0, 0, 5, 8);
    }
    else {
        SetScroll($(".GroupType"), left_scroll_bgcolor, 0, 0, 0, 0, 0);
    }

}
function LoadTimePeriod(data) {
    html = "";
    AllTimePeriods = [];
    var indx = 0;
    if (data != null) {
        html += "<ul>";
        for (var i = 0; i < data.EcommTimePeriodList.length; i++) {
            if (data.EcommTimePeriodList[i].Name.toUpperCase() == "3MMT" || data.EcommTimePeriodList[i].Name.toUpperCase() == "12MMT" || data.EcommTimePeriodList[i].Name.toUpperCase() == "YTD" || data.EcommTimePeriodList[i].Name.toUpperCase() == "6MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "48MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "36MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "30MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "24MMT" || data.TimePeriodlist[i].Name.toUpperCase() == "18MMT")
                html += "<li name=\"" + data.EcommTimePeriodList[i].Name + "\" id=\"" + data.EcommTimePeriodList[i].Id + "\" onclick=\"SelectTimePeriod(this);\" style=\"text-transform:uppercase;\">" + data.EcommTimePeriodList[i].Name.toLowerCase() + "</li>";
            else
            html += "<li name=\"" + data.EcommTimePeriodList[i].Name + "\" id=\"" + data.EcommTimePeriodList[i].Id + "\" onclick=\"SelectTimePeriod(this);\">" + data.EcommTimePeriodList[i].Name.toLowerCase() + "</li>";
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
            AllTimePeriods.push({ Id: data.EcommTimePeriodList[i].Id, Name: data.EcommTimePeriodList[i].Name, TimePeriods: data.EcommTimePeriodList[i].TimePeriodlist, Sliderlist: data.EcommTimePeriodList[i].Sliderlist });
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
        if (Array[i].Id == $(obj).attr("id") && Array[i].Name == $(obj).attr("name")) {
            return i;
            break;
        }
        else if (Array[i].Id == $(obj).find(".lft-popup-ele-label").attr("id") && Array[i].Name == $(obj).attr("name")) {
            return i;
            break;
        }
    }
    return -1;
}
function SelectComparison(obj) {
    if ($(obj).attr("isselectable") != undefined && $(obj).attr("isselectable") != "" && JSON.parse($(obj).attr("isselectable")) == false)
        return false;
    CompCurrentId = GetArrayId(obj, Comparisonlist);
    if ((ModuleBlock == "TREND" || ModuleBlock == "PIT" || currentpage == "hdn-analysis-crossretailerfrequencies" || currentpage == "hdn-analysis-acrossshopper")) {
        Comparisonlist = [];
        $(".Retailers .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
        $(".Retailers .Lavel li").find(".ArrowContainerdiv").css("background-color", "#58554D");
    }

    if (CompCurrentId == -1) {
        Comparisonlist.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), DBName: $(obj).attr("dbname"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid") });
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Not-Selected-Channel").addClass("Selected");
        }
        else {
            $(obj).addClass("Selected");
        }
    }
    else {
        Comparisonlist.splice(CompCurrentId, 1);
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Selected").addClass("Not-Selected-Channel");
            $(obj).parent("div").parent("li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
        else {
            $(obj).removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
    }

    if ($(obj).attr("type") == "Main-Stub") {
        $(".Retailer").hide();
        $("#retailerHeadingLevel2").hide();
        $("#retailerHeadingLevel3").hide();
    }
    ShowSelectedFilters();
}
function RemoveComparison(obj) {
    var CompObj = $(".Retailer .Comparison[id='" + $(obj).attr("Id") + "']");
    if (CompObj.length <= 0)
        CompObj = $("#ChannelOrCategoryContent ul li span[id='" + $(obj).attr("Id") + "']");
    SelectComparison(CompObj);
}
function DisplayComparisonRetailer(obj) {
    $(".Retailer").hide();
    $(".Retailer div").hide();
    var lavels = parseInt($(obj).attr("lavels"));
    for (var i = 0; i < lavels ; i++) {
        $(".Retailers .Lavel" + i).show();
        $(".Retailers .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".Retailers .Lavel" + i + " .priorityclass").hide();
        $(".Retailers .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel2").text($(obj).attr("name").toUpperCase());
        $(".AdvancedFiltersDemoHeading #retailerHeadingLevel2").show();

        if (i == 1)
            $(".AdvancedFiltersDemoHeading #retailerHeadingLevel2").css("width", "505px");
        else
            $(".AdvancedFiltersDemoHeading #retailerHeadingLevel2").css("width", "252px");
        SetScroll($(".Retailers .Lavel" + i), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
}
function SelectBevComparison(obj) {
    if ($(obj).attr("isselectable") != undefined && $(obj).attr("isselectable") != "" && JSON.parse($(obj).attr("isselectable")) == false)
        return false;

    CompCurrentId = GetArrayId($(obj), ComparisonBevlist);
    if (ModuleBlock == "TREND" || ModuleBlock == "PIT" || currentpage == "hdn-analysis-crossretailerfrequencies" || currentpage == "hdn-chart-beveragedeepdive") {
        ComparisonBevlist = [];
        $(".Beverages .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
        $(".Beverages .Lavel li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        //$("#BGMNonBeverageDiv .Lavel li").removeClass("Selected").addClass("Not-Selected-Channel");
    }

    if (CompCurrentId == -1) {
        ComparisonBevlist.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), DBName: $(obj).attr("dbname"), ShopperDBName: $(obj).attr("shopperdbname"), TripsDBName: $(obj).attr("tripsdbname"), UniqueId: $(obj).attr("uniqueid") });
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Not-Selected-Channel").addClass("Selected");
        }
        else {
            $(obj).addClass("Selected");
        }
    }
    else {
        ComparisonBevlist.splice(CompCurrentId, 1);
        if ($(obj).attr("type") == "Main-Stub") {
            $(obj).parent("div").parent("li").removeClass("Selected").addClass("Not-Selected-Channel");
            $(obj).parent("div").parent("li").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
        else {
            $(obj).removeClass("Selected");
            $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
    }

    if ($(obj).attr("type") == "Main-Stub") {
        $(".Beverage").hide();
        $("#beverageHeadingLevel2").hide();
        $("#beverageHeadingLevel3").hide();
    }
    ShowSelectedFilters();
    if (currentpage == "hdn-analysis-acrossshopper") {
        AddToBevarageDropDown();
    }
}
function RemoveBevComparison(obj) {
    var CompObj = $(".Beverages .Comparison[dbname='" + $(obj).attr("dbname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']");
    if (CompObj.length <= 0)
        CompObj = $("#BeverageOrCategoryContent ul li span[dbname='" + $(obj).attr("dbname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']");
    if (CompObj.length <= 0)
        CompObj = $("#BGMNonBeverageDiv ul li span[name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']");

    SelectBevComparison(CompObj);
}
function DisplayBevComparisonRetailer(obj) {
    $(".Beverage").hide();
    $(".Beverage div").hide();
    if ($(obj).attr("name") == "Category Nets" && currentpage == "hdn-analysis-acrossshopper")
        $(".Beverages").css("width", "116%");
    else
        $(".Beverages").css("width", "auto");
    var lavels = parseInt($(obj).attr("lavels"));
    for (var i = 0; i < lavels ; i++) {
        $(".Beverages .Lavel" + i).show();
        $(".Beverages .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").text($(obj).attr("name").toUpperCase());
        $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").show();
        if (i == 3)
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "1049px");
        else if (i == 2)
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "786px");
        else if (i == 1)
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "505px");
        else
            $(".AdvancedFiltersDemoHeading #beverageHeadingLevel2").css("width", "252px");
        SetScroll($(".Beverages .Lavel" + i), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
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
                                    if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else {
                                        if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
                                            fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                        else
                                            fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    }
                                    AllDemographics.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else
                                    fourthLevelhtml += "<div onclick=\"\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
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
    $(obj).addClass("Selected");
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
    if ($(obj).hasClass("FrequencyItem")) {
        SelectFrequency(obj);
    }
    else if ($(obj).find("span").attr("isgeography") == "true") {
        var object = $(obj).find(".lft-popup-ele-label");
        if ($(obj).parent().parent().attr("level") == "level4") {
            $("#AdvFilterDivId .level3 *").removeClass("Selected");
            //$("#AdvFilterDivId .level3 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
        else {
            $("#AdvFilterDivId .level4").hide();
            //$("#FourthLevelAdvancedFilterContent").hide();
            $("#DemoHeadingLevel4").hide();
            $("#AdvFilterDivId .level4 *").removeClass("Selected").addClass("Not-Selected-Channel");
            //$("#FourthLevelAdvancedFilterContent .DemographicList div[Level = 'FouthLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#FourthLevelAdvancedFilterContent .DemographicList div[Level = 'FouthLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[onclick='DisplayFourthLevelGeoFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
            $("#AdvFilterDivId .level3 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
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
                            $("#AdvFilterDivId .level3 *").removeClass("Selected");
                            //$("#ThirdLevelAdvancedFilterContent *").removeClass("Selected");
                            $("#AdvFilterDivId .level3 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            $("#AdvFilterDivId .level4 *").removeClass("Selected");
                            $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            $("#AdvFilterDivId .level3").hide();
                            $("#AdvFilterDivId .level4").hide();
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
                                $("#AdvFilterDivId .level3 *").removeClass("Selected");
                                $("#AdvFilterDivId .level3 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                            else {
                                $("#AdvFilterDivId .level2 li[is-geography='true'][onclick='SelectDemographic(this);']").removeClass("Selected");
                                $("#AdvFilterDivId .level2 li[is-geography='true'][onclick='SelectDemographic(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                                $("#AdvFilterDivId .level3 li[onclick='SelectDemographic(this);']").removeClass("Selected");
                                $("#AdvFilterDivId .level3 liv[onclick='SelectDemographic(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                        }


                        $(obj).addClass("Selected");
                        SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
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

                            SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                                return obj.isGeography !== 'true';
                            });
                            $("#AdvFilterDivId .level4 *").removeClass("Selected");
                            $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");

                        }
                        if ($(obj).find("span").attr("parentname").trim() == "Trade Areas") {
                            SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                                return obj.isGeography !== 'true';
                            });
                            $("#AdvFilterDivId .level3 li[parentname='Store Trade Areas'] *").removeClass("Selected");
                            $("#AdvFilterDivId .level3 li[parentname='Store Trade Areas'] *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            $("#AdvFilterDivId .level3 .sidearrw_OnCLick").each(function (i, j) {
                                $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
                            });
                            var sPrimaryDemo = $(obj);
                            $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
                            $("#ThirdDemoFilterList div[onclick='DisplayFourthLevelDemoFilter(this);']").removeClass("Selected");
                            $("#ThirdDemoFilterList div[onclick='DisplayFourthLevelDemoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
                        }
                        if ($(obj).find("span").attr("parentname").trim() == "Albertson's/Safeway Trade Areas" || $(obj).find("span").attr("parentname").trim() == "HEB Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Kroger Trade Areas" || $(obj).find("span").attr("parentname").trim() == "Publix Trade Areas") {
                            if ($(obj).find("span").text().trim() == "Albertson's/Safeway Corporate Net Trade Area" || $(obj).find("span").text().trim() == "HEB Trade Area" || $(obj).find("span").text().trim() == "Kroger Trade Area" || $(obj).find("span").text().trim() == "Publix Trade Area") {
                                SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                                    return obj.isGeography !== 'true';
                                });
                                $("#AdvFilterDivId .level4 *").removeClass("Selected");
                                $("#AdvFilterDivId .level4 *").find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                            else {
                                SelectedDempgraphicList = SelectedDempgraphicList.filter(function (obj) {
                                    return (obj.UniqueId !== '292' && obj.UniqueId !== '310');
                                });
                                $("#AdvFilterDivId .level4 span[uniqueid='292']").parent().removeClass("Selected");
                                $("#AdvFilterDivId .level4 span[uniqueid='292']").parent().find(".ArrowContainerdiv").css("background-color", "#58554D");
                                $("#AdvFilterDivId .level4 span[uniqueid='310']").parent().removeClass("Selected");
                                $("#AdvFilterDivId .level4 span[uniqueid='310']").parent().find(".ArrowContainerdiv").css("background-color", "#58554D");
                            }
                        }

                        $(obj).addClass("Selected");
                        SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
                    }
                }
                else {
                    $(obj).addClass("Selected");
                    SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
                }
            }
            else {
                $(obj).addClass("Selected");
                SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
            }
        }
    }
    else {
        var object = $(obj).find(".lft-popup-ele-label");
        if ($(obj).parent().parent().attr("level") == "level4") {
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[Level = 'ThirdLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[Level = 'ThirdLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        }
        else {
            $("#FourthLevelAdvancedFilterContent").hide();
            $("#DemoHeadingLevel4").hide();
            $("#FourthLevelAdvancedFilterContent .DemographicList div[Level = 'FouthLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
            $("#FourthLevelAdvancedFilterContent .DemographicList div[Level = 'FouthLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[onclick='DisplayFourthLevelGeoFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
            $("#ThirdLevelAdvancedFilterContent .DemographicList div[onclick='DisplayFourthLevelGeoFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
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

            $(obj).addClass("Selected");
            SelectedDempgraphicList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isGeography") });
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
    $("#GroupTypeHeaderContent ul li").removeClass("Selected");
    $("#GroupTypeHeaderContent ul li").find(".ArrowContainerdiv").css("background-color", "#58554D");
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

    //$("#GroupTypeContent div[name='" + $(obj).attr("name") + "'][parentname='" + $(obj).attr("primefiltertype") + "']").show();
    //$("#GroupTypeContent div[name='" + $(obj).attr("name") + "'][parentname='" + $(obj).attr("primefiltertype") + "'] ul").show();
    
    $("#GroupTypeContent div[name='" + $(obj).attr("name") + "'][parentname='" + $(obj).attr("primefiltertype") + "']").show();
    $("#GroupTypeContent div[name='" + $(obj).attr("name") + "'][parentname='" + $(obj).attr("primefiltertype") + "'] ul").show();

    //$("#GroupTypeContent div[name='" + $(obj).attr("name") + "']").show();
    //$("#GroupTypeContent div[name='" + $(obj).attr("name") + "'] ul").show();
    //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").text($(obj).attr("name").toUpperCase());
    //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").show();
    //$(".AdvancedFiltersDemoHeading #grouptypeHeadingLevel2").css("width", "287px");
    HideAdvFilterOnGroupSelect();
    HideMeasureFilterOnGroupSelect();
    ShowSelectedFilters();
    DisplayHeightDynamicCalculation("group");
    //$("#grouptypeHeadingLevel4").hide();
    //$("#grouptypeHeadingLevel3").html($(obj).attr("name").toUpperCase() == "PRODUCTS CAME FROM " ? "PRODUCT'S CAME FROM " : $(obj).attr("name"));
    //$("#grouptypeHeadingLevel3").show();
    UpdatePopupScroll();
}
function SelecGroupMetricName(obj) {
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
        measureOnclick = "DisplaySecondaryAdvancedAnalytics(this);";
    }
    if (removeGroupStatus != 2 && currentpage != "hdn-e-commerce-tbl-sitedeepdive") {
        if (Groupfiltertype.toLocaleLowerCase() == "visits" || Groupfiltertype.toLocaleLowerCase() == "online ordering process"
            || Groupfiltertype.toLocaleLowerCase() == "auto-replenished deliveries" || Groupfiltertype.toLocaleLowerCase() == "items purchased") {
            Grouptype = "visits";
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
            //SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", tripsGroups);
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", TripsMeasureSearchItems);
        }
        else if (Groupfiltertype.toLocaleLowerCase().indexOf("shopper") > -1) {
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

            //SelectedFrequencyList = [];
            Measurelist = [];
            //SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", ShopperMeasureSearchItems);
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
            //SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
        }
    }

    var object = $(obj).find(".lft-popup-ele-label");
    if ($(obj).attr("Level") == "FouthLevel") {
        // $("#GroupTypeContentSub .DemographicList div[Level = 'ThirdLevel' i]").removeClass("Selected").addClass("Not-Selected-Channel");
    }
    else {
        $("#GroupTypeGeoContentSub").hide();
        //$("#grouptypeHeadingLevel4").hide();
        $("#GroupTypeGeoContentSub .DemographicList div[Level = 'FouthLevel']").removeClass("Selected").addClass("Not-Selected-Channel");
        $("#GroupTypeGeoContentSub .DemographicList div[Level = 'FouthLevel']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#GroupTypeContentSub .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").removeClass("Selected"); //$("#ThirdGeographyFilterList ")
        $("#GroupTypeContentSub .DemographicList div[onclick='DisplayForthLevelGroupFilter(this);']").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#GroupTypeContentSub .sidearrw_OnCLick").each(function (i, j) {
            $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        });
        var sPrimaryDemo = $(obj);
        $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");
    }

    if (Grouplist.length > 0 && $(obj).attr("parentname").toLowerCase() != Grouplist[0].parentName.toLowerCase()) {
        $("#groupDivId *").removeClass("Selected");
        Grouplist = [];
        $(obj).addClass("Selected");       
        Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography") });
    }
    else{
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
            //Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: $(object).attr("Fullname").trim(), parentId: $(object).attr("ParentLevelId").trim(), parentName: $(object).attr("ParentLevelName").trim(), DBName: $(object).attr("DBName"), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography") });
             Grouplist.push({ Id: $(object).attr("id"), Name: $(object).attr("name").trim(), FullName: ($(object).attr("Fullname") != undefined ? $(object).attr("Fullname").trim() : ""), parentId: $(object).attr("parentid").trim(), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid"), isGeography: $(object).attr("isgeography") });
        }
    }
    }
    //LoadAdvancedFilters(sFilterData);
    //LoadSecondaryAdvancedFilters(sFilterData);
    HideAdvFilterOnGroupSelect();
    HideMeasureFilterOnGroupSelect();
    UpdatePopupScroll();
    ShowSelectedFilters();
    BuildDynamicTable();
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

        $("#AdvFilterDivId div[level-id='1'] ul li[name='" + groupname + "']").hide();
        $("#AdvFilterDivId div[level-id='1'] ul li[name='" + measurename + "']").hide();
        //for measure
        $("#retailer-measure div[level-id='2'] ul").children("li[name='" + groupname + "']").hide();
        //$("#MeasureTypeHeaderContentShopper ul").children("li[name='" + groupname + "']").hide();

        if (Grouplist.length > 0 && Grouplist[0].isGeography == "true")
            //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='Geography']").parent("li").hide();
            $("#AdvFilterDivId div[level-id='1'] ul li[name='Geography']").hide();
    }
    else {
        //need to uncomment on demo order coming right rev
        $("#AdvFilterDivId div[level-id='1'] ul li").show();
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

function RemoveGroup(obj) {
    var ObjData = $("#GroupTypeContent span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $("#GroupTypeContentSub span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $("#GroupTypeGeoContentSub span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $("#groupDivId ul li span[uniqueid='" + $(obj).attr("UniqueId") + "']").parent();
    SelecGroupMetricName(ObjData);
    removeGroupStatus = 1;
    if (Grouplist.length <= 0) {
        if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("cursor", "pointer");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").css("background-color", "");
            $("#MeasureTypeShopperTripHeader ul li[name='Shopper Measures']").attr("onclick", "DisplayMeasureTripShopperList(this);");
            SelectedFrequencyList = [];
            Measurelist = [];
            SearchFilters("Measure", "Search-Measure-Type", "Measure-Type-Search-Content", AllMeasures);
        }
    }
    ShowSelectedFilters();
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
            sBevarageSelctionType.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), Params: $(object).attr("Params"), UniqueId: $(obj).attr("uniqueid") });
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

//End
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
//Geography Filter
function LoadGeographyFilters() {
    var DataList = [];
    if (dGeo == null || dGeo == [] || dGeo == "")
        dGeo = SelecGeography();
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
function LoadSecondaryGeographyFilters() {
    html = "";
    var thirdLevelhtml = "";
    var DataList = [];
    if (dGeo == null || dGeo == [] || dGeo == "")
        dGeo = SelecGeography();
    DataList = dGeo;

    if (DataList != null) {
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
                            if (object.active != "false")

                                html += "<div onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label-img " + object.Name + "\" id=\"" + object.Id + "\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";

                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label-img " + object.Name + "\" id=\"" + object.Id + "\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";
                        }
                        else {
                            if (object.active != "false")
                                html += "<div onclick=\"DisplayThirdLevelGeoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label-img " + object.Name + "\" id=\"" + object.Id + "\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            else
                                html += "<div onclick=\"\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"background-color:gray;\"><span class=\"lft-popup-ele-label-img " + object.Name + "\" id=\"" + object.Id + "\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                        }
                    }
                    else

                        thirdLevelhtml += "<div onclick=\"SelectGeographyData(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label-img " + object.Name + "\" id=\"" + object.Id + "\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + DataList[i].SecondaryAdvancedFilterlist[i].Id.toString().trim() + " \" ParentLevelName=\" " + DataList[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span></div>";

                    AllDemographics.push(object.Id + "|" + object.Name);
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
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel2").css("width", "252px");
    $("#PrimaryGeographyFilterContent").hide();
    SetScroll($("#SecondaryGeographyFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplayThirdLevelGeoFilter(obj) {
    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    $(sPrimaryDemo).find(".Selected").removeClass("Selected");
    $(sPrimaryDemo).find(".ArrowContainerdiv").css("background-color", "#58554D");
    $(obj).addClass("Selected");
    $("#ThirdGeographyFilterList .DemographicList").hide();
    $("#ThirdGeographyFilterList").show();
    $("#ThirdGeographyFilterList div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $("#ThirdLevelGeographyFilterContent").css("display", "inline-block");
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").text($(obj).find(".lft-popup-ele-label").attr("name"));
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").show();
    $(".AdvancedFiltersDemoHeading #DemoHeadingLevel3").css("width", "252px");
    SetScroll($("#ThirdLevelAdvancedFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function SelectGeographyData(obj) {
    var object = $(obj).find(".lft-popup-ele-label");
    if (currentpage == "hdn-analysis-acrosstrips") {
        Geographylist = [];
        $("#ThirdGeographyFilterList *").removeClass("Selected").addClass("Not-Selected-Channel");
        $("#ThirdGeographyFilterList *").find(".ArrowContainerdiv").css("background-color", "#58554D");
    }
    var sCurrentDemoId = "";
    for (var i = 0; i < Geographylist.length; i++) {
        if (Geographylist[i].Id == $(object).attr("id")) {
            sCurrentDemoId = i;
        }
    }

    if ($(obj).hasClass("Selected")) {
        $(obj).removeClass("Selected");
        $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        Geographylist.splice(sCurrentDemoId, 1);
    }
    else {
        $(obj).addClass("Selected");
        Geographylist.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), parentId: $(object).attr("ParentLevelId").trim(), parentName: $(object).attr("ParentLevelName").trim(), DBName: $(object).attr("DBName") });
    }
    ShowSelectedFilters();
}
function RemoveGeographyData(obj) {
    var ObjData = $("#SecondaryAdvancedFilterContent .DemographicList [Fullname='" + $(obj).attr("Fullname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']").parent();
    SelectGeographyData(ObjData);
}
//End
function LoadMeasureShopperTripLevel() {
    html = "";
    var datalist = [{
        Id: "1",
        Name: "Online Purchase Measures"
    }, {
        Id: "2",
        Name: "Shopper Measures"
    }];

    html += "<ul>";
    for (var i = 0; i < datalist.length; i++) {
        var object = datalist[i];
        if ((currentpage.indexOf("retailerdeepdive") > -1) || (currentpage.indexOf("beveragedeepdive") > -1) || (currentpage.indexOf("sitedeepdive") > -1))
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
    $(".Geotooltipimage, #MeasureTypeHeaderMainTrip .gouptype").hover(function () {
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
                if (groupname == "online ordering process" || groupname == "auto-replenished deliveries" || groupname == "items purchased (online orders)")
                    groupname = "visits";

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


    //searchlist = (sFilterData.TripEcommerceMeasures[0].GroupTypelist).concat(sFilterData.shopperEcommerceMeasures[0].GroupTypelist);
    searchlist = (sFilterData.TripEcommerceMeasures[0].GroupTypelist);


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

    datalist = sFilterData.TripEcommerceMeasures[0].GroupTypelist;

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

                html2 += "<li ParentDetails=\"" + object1.ParentDetails + "\" style=\"display:none;\" FilterTypeId=\"" + object1.FilterTypeId + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\" Name=\"" + object1.Name.replace("'", "") + "\" class=\"gouptype\" ChartTypePIT=\"" + object1.ChartTypePIT + "\" ChartTypeTrend=\"" + object1.ChartTypeTrend + "\" onclick=\"SelecMeasure(this);\">";
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
                        html4 += "<li style=\"white-space:pre-wrap;\" Type=\"Trip\" ParentDetails=\"" + object2.ParentDetails + "\" UId=\"" + object2.FilterTypeId + "|" + object2.Id + "|" + object2.parentId + "\" id=\"" + object2.Id + "|" + object2.Id + "\" type=\"Main-Stub\" DBName=\"" + object2.DBName + "\" UniqueId=\"" + object2.UniqueId + "\" shopperdbname=\"" + object2.ShopperDBName + "\" tripsdbname=\"" + object2.TripsDBName + "\" Name=\"" + object2.Name.replace("'", "") + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + object2.Name + "</li>";
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

    datalist = sFilterData.shopperEcommerceMeasures[0].GroupTypelist;

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
function LoadMeasureTypeMain(data) {
    html = "";
    var index = 0;
    var datalist = {};
    var parentname = "";
    if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "1"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "1";
        });
    else if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "2"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "2";
        });
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "1"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "3";
        });
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "2"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "4";
        });

    if (sVisitsOrGuests == 1)
        parentname = "Trip";
    else
        parentname = "Shopper";
    datalist = _.uniq(_.map(_.flatten(datalist), function (e) {
        return e.FilterTypeId + "|" + e.FilterType;
    }));

    var ulclose = false;
    if (data != null) {

        html += "<ul>";
        for (var i = 0; i < datalist.length; i++) {
            var object = datalist[i].split("|");

            html += "<li id=\"" + object[0] + "\" Name=\"" + object[1] + "\" class=\"gouptype\" onclick=\"DisplayMeasureList(this);\" parentname=\"" + parentname + "\">";
            html += "<div class=\"FilterStringContainerdiv\">";
            //html += "<span class=\"img-retailer\"></span>";
            html += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" id=\"" + object[0] + "\" type=\"Main-Stub\" Name=\"" + object[1] + "\">" + object[1] + "</div>";
            html += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
            html += "</div>";
            html += "</li>";
        }
        html += "</ul>";
        $("#MeasureTypeHeaderMain").html("");
        $("#MeasureTypeHeaderMain").html(html);
    }
}
function LoadMeasureTypeHeaderName(data) {
    html = "";
    var shtml = "";
    var index = 0;
    var datalist = {};
    if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "1"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "1";
        });
    else if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "2"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "2";
        });
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "1"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "3";
        });
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "2"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "4";
        });

    datalist = _.uniqWith(datalist, _.isEqual);

    var ulclose = false;
    if (data != null) {

        html += "<ul>";
        shtml += "<ul>";
        for (var i = 0; i < datalist.length; i++) {

            var object = datalist[i];
            if (object.Level == "1") {
                html += "<li style=\"display:none;\" FilterTypeId=\"" + object.FilterTypeId + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" Name=\"" + object.Name + "\" class=\"gouptype\" ChartTypePIT=\"" + object.ChartTypePIT + "\" ChartTypeTrend=\"" + object.ChartTypeTrend + "\" onclick=\"SelecMeasure(this);\">";
                html += "<div class=\"FilterStringContainerdiv\">";
                //html += "<span class=\"img-retailer\"></span>";
                html += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + object.ChartTypePIT + "\" ChartTypeTrend=\"" + object.ChartTypeTrend + "\" Name=\"" + object.Name + "\">" + object.Name + "</div>";
                html += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
                html += "</div>";
                html += "</li>";
                AllMeasures.push(object.Id + "|" + object.Name);
            }

        }
        html += "</ul>";
        shtml += "</ul>";

        $("#MeasureTypeHeaderContent").html("");
        $("#MeasureTypeHeaderContent").html(html);
    }
}
function LoadMeasureTypeNames(data) {
    html = "";
    var shtml = "";
    var index = 0;
    var ulclose = false;
    var datalist = {};
    if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "1"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "1";
        });
    else if ((currentpage.indexOf("retailer") > 0) && (sVisitsOrGuests == "2"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "2";
        });
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "1"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "3";
        });
    else if ((currentpage.indexOf("beverage") > 0) && (sVisitsOrGuests == "2"))
        datalist = _.filter(data.Measure, function (u) {
            return u.selectType == "4";
        });

    datalist = _.uniqWith(datalist, _.isEqual);
    var index = 0;
    if (data != null) {

        for (var i = 0; i < datalist.length; i++) {
            index = 0;

            for (var j = 0; j < datalist[i].SecondaryAdvancedFilterlist.length; j++) {
                var object = datalist[i].SecondaryAdvancedFilterlist[j];

                var k = _.filter(datalist, function (u) {
                    return u.Name.toUpperCase() == object.Name.toUpperCase();
                });
                if (k.length <= 0) {
                    if (index == 0)
                        html += "<ul uniqueid=\"" + datalist[i].UniqueId + "\" Name=\"" + datalist[i].Name + "\" style=\"display:none;\">";
                    html += "<li UId=\"" + datalist[i].FilterTypeId + "|" + datalist[i].Id + "|" + datalist[i].parentId + "\" id=\"" + datalist[i].Id + "|" + object.Id + "\" type=\"Main-Stub\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" Name=\"" + object.Name + "\" class=\"gouptype\" onclick=\"SelecMeasureMetricName(this);\">" + object.Name + "</li>";
                }
                else {
                    if (index == 0)
                        shtml += "<ul Name=\"" + datalist[i].Name + "\" style=\"display:none;\">";
                    shtml += "<li id=\"" + datalist[i].Id + "|" + object.Id + "\" type=\"Main-Stub\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" Name=\"" + object.Name + "\" class=\"gouptype\" onclick=\"SelecMeasure(this);\">";
                    shtml += "<div class=\"FilterStringContainerdiv\">";
                    shtml += "<div style=\"text-align:left;overflow:hidden;width: 92%;float: left;\" FilterTypeId=\"" + object.FilterTypeId + "\" id=\"" + object.Id + "\" type=\"Main-Stub\" ChartTypePIT=\"" + object.ChartTypePIT + "\" ChartTypeTrend=\"" + object.ChartTypeTrend + "\" Name=\"" + object.Name + "\">" + object.Name + "</div>";
                    shtml += "<div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
                    shtml += "</div>";
                    shtml += "</li>";
                }
                //AllMeasures.push(object.Id + "|" + object.Name);
                index++;
            }
            html += "</ul>";
            shtml += "</ul>";
        }

        $("#MeasureTypeHeaderContentSubLevel").html("");
        $("#MeasureTypeHeaderContentSubLevel").html(shtml);
        SetScroll($("#MeasureTypeHeaderContentSubLevel"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        $("#MeasureTypeContent").html("");
        $("#MeasureTypeContent").html(html);
    }
}
function SelecMeasure(obj) {
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
            $(".rgt-cntrl-frequency").hide();
            $(".rgt-cntrl-ordertype").hide();
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
                            //$("#MeasureTypeContentTrip ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(1).addClass("Selected");
                            $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(1).addClass("Selected");
                        }
                        else {
                            //$("#MeasureTypeContentTrip ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(0).addClass("Selected");
                            $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).addClass("Selected");
                        }
                    }
                    else {
                        if (sTrendType == "2" && ModuleBlock == "TREND") {
                            //$("#MeasureTypeContentTrip ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(0).addClass("Selected");
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
                            //$("#MeasureTypeContentTrip ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(1).addClass("Selected");
                            $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(1).addClass("Selected");
                        }
                        else {
                            //$("#MeasureTypeContentTrip ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(0).addClass("Selected");
                            $("#retailer-measure .level4 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").eq(0).addClass("Selected");
                        }
                    }
                    else {
                        if (sTrendType == "2" && ModuleBlock == "TREND") {
                            //$("#MeasureTypeContentTrip ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(0).addClass("Selected");
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
            
            //else if (sVisitsOrGuests == 2) {
            //    $("#MeasureTypeContentShopper ul").hide();
            //    $("#MeasureTypeContentShopper").show();
            //    $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "']").show();
            //    if ($("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(0).text().trim() == "Top 10") {
            //        if (sTrendType == "2" && ModuleBlock == "TREND")
            //            $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(1).addClass("Selected");
            //        else
            //            $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(0).addClass("Selected");
            //    }
            //    else {
            //        if (sTrendType == "2" && ModuleBlock == "TREND") {
            //            $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").eq(0).addClass("Selected");
            //        }
            //        else {
            //        if ($("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").length <= 10)
            //        $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").addClass("Selected");
            //    else {
            //            $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").each(function (i, j) {
            //                if (i < 10)
            //                    $(j).addClass("Selected");
            //            });
            //    }
            //}
            //    }
            //}
            

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
            $(".rgt-cntrl-ordertype").hide();
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
        Measurelist.push({ uniqueid: $(obj).attr("parentid"), parentName: $(obj).attr("name"), filtertypeid: $(obj).attr("filtertypeid"), MetricId: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), DBName: $(obj).attr("name"), charttype: $(obj).attr("charttype"), FilterType: $(obj).attr("filtertype"), metriclist: [] });
   
        var _metriclist = [];
        if ($(obj).parent().parent().attr('level-id') == '2') {
            if (sTrendType == "2" && ModuleBlock == "TREND") {
                $("#retailer-measure .level3 ul li[parentname='" + $(obj).attr("name") + "'][parentid='" + $(obj).attr('id') + "']").each(function (i, j) {
                    if ($(this).attr("parentname") == $(obj).attr("name")) {
                        if ($(this).text().trim() != "Top 10") {
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname"), FilterType: $(this).attr("filtertype") });
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
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname"), FilterType: $(this).attr("filtertype") });
                            return false;
                        }

                        else {
                            if (_metriclist.length < 10) {
                                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname"), FilterType: $(this).attr("filtertype") });

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
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname"), FilterType: $(this).attr("filtertype") });
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
                            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname"), FilterType: $(this).attr("filtertype") });
                            return false;
                        }

                        else {
                            if (_metriclist.length < 10) {
                                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname"), FilterType: $(this).attr("filtertype") });

                            }
                        }
                    }
                });
            }
        }
        
        
        //else if (sVisitsOrGuests == 2) {
        //    if (sTrendType == "2" && ModuleBlock == "TREND") {
        //        $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").each(function (i, j) {
        //            if ($(this).html().trim() != "Top 10") {
        //                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html().trim(), Type: $(this).attr("Type") });
        //                if (_metriclist.length == 1)
        //                    return false;
        //            }
        //        });
        //    }
        //    else {
        //    $("#MeasureTypeContentShopper ul[uniqueid='" + $(obj).attr("uniqueid") + "'] li").each(function () {
        //        if ($(this).html().trim() == "Top 10") {
        //            _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html().trim(), Type: $(this).attr("Type") });
        //            return false;
        //        }
        //        else {
        //            if (_metriclist.length < 10)
        //                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html().trim(), Type: $(this).attr("Type") });
        //        }
        //    });
        //}
        //}
        
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
    DisplayHeightDynamicCalculation("Measure");
   //19-01-2018 
    if ((currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") && (Measurelist.length > 0) && (Measurelist[0].metriclist.length > 0) && (TabType.toUpperCase() == "TRIP" || TabType.toUpperCase() == "TRIPS")) {
        sClosePopupStatus = 1;
        if (((TabType.toUpperCase() == "TRIP" || TabType.toUpperCase() == "TRIPS") && Measurelist[0].filtertypeid == String(2))) {
            tabname = Measurelist[0].metriclist[0].FilterType.toUpperCase();
            LoadTripsFrequencyFilter(sFilterData);
            TabType = "trips";
            $("#RightPanelPartial .rgt-cntrl-ordertype ul li[name='ONLINE ORDER']").removeClass("Selected");
            $("#RightPanelPartial .rgt-cntrl-ordertype ul li[name='ONLINE ORDER']").trigger("click");
            PrepareSearch("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
        }
        else if ((TabType.toUpperCase() == "TRIP" || TabType.toUpperCase() == "TRIPS") && Measurelist[0].filtertypeid == String(3)) {
            tabname = Measurelist[0].metriclist[0].FilterType.toUpperCase();
            LoadTripsFrequencyFilter(sFilterData);
            TabType = "trips";
            $("#RightPanelPartial .rgt-cntrl-ordertype ul li[name='AUTO-REPLENISHMENT']").removeClass("Selected");
            $("#RightPanelPartial .rgt-cntrl-ordertype ul li[name='AUTO-REPLENISHMENT']").trigger("click");
            PrepareSearch("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
        }
        else {
            tabname = Measurelist[0].metriclist[0].FilterType.toUpperCase();
            LoadTripsFrequencyFilter(sFilterData);
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
            $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
            PrepareSearch("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
        }
        sClosePopupStatus = 0;
    }
    //End 19-01-2018
    //sClosePopupStatus = 0;
    HideAdvFilterOnGroupSelect();
    $("#MeasureTypeHeaderContentSubLevelShopper").getNiceScroll().remove();
    $("#MeasureTypeContentShopper").getNiceScroll().remove();
    $(".rgt-cntrl-frequency").hide();
    $(".rgt-cntrl-ordertype").hide();
}
function SelecMeasureMetricName(obj) {
    isPopupVisible = true;
    var sCount = 0;
    if (Measurelist.length > 0 && Measurelist[0].parentName.toLowerCase() != $(obj).attr("parentname").toLowerCase()) {
        $("#retailer-measure .level3 *").removeClass("Selected");
        Measurelist = [];
        Measurelist.push({ uniqueid: $(obj).attr("parentid"), parentName: $(obj).attr("parentname"), filtertypeid: $(obj).attr("filtertypeid"), MetricId: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), DBName: $(obj).attr("name"), charttype: $(obj).attr("charttype"), FilterType: $(obj).attr("filtertype"), metriclist: [] });
        _.each(Measurelist, function (i) {
            i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), Type: $(obj).attr("Type"), parentname: $(obj).attr("parentname"), FilterType: $(this).attr("filtertype") });
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
        Measurelist.push({ uniqueid: $(obj).attr("parentid"), parentName: $(obj).attr("parentname"), filtertypeid: $(obj).attr("filtertypeid"), MetricId: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), DBName: $(obj).attr("name"), charttype: $(obj).attr("charttype"), FilterType: $(obj).attr("filtertype"), metriclist: [] });
        _.each(Measurelist, function (i) {
            i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name"), Type: $(obj).attr("Type"), parentname: $(obj).attr("parentname"), FilterType: $(this).attr("filtertype") });
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
                        i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name").trim(), Type: $(obj).attr("Type").trim(), SelType: $(obj).attr("seltypeid"), parentname: $(obj).attr("parentname"), FilterType: $(this).attr("filtertype") });
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
                        i.metriclist.push({ Id: $(obj).attr("uniqueid"), Name: $(obj).attr("name").trim(), Type: $(obj).attr("Type").trim(), SelType: $(obj).attr("seltypeid"), parentname: $(obj).attr("parentname"), FilterType: $(this).attr("filtertype") });
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
        GetDefaultFrequency();
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
        GetDefaultFrequency();
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
                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).attr("name"), Type: $(this).attr("Type"), parentname: $(this).attr("parentname"), FilterType: $(this).attr("filtertype") });
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
    {
        //23-11-17
        $("#MeasureTypeHeaderContentTrip ul *").removeClass('DNI');
        //End 23-11-17
        $("#MeasureTypeHeaderContentTrip ul li[FilterTypeId='" + $(obj).attr("Id") + "'][parentname='" + $(obj).attr("name") + "']").show();
    }
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
    //TabType = $(obj).attr("name").toLowerCase().replace("trip","trips");
    tabname = "";
    LoadTripsFrequencyFilter(sFilterData);
    PrepareSearch("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);

    HideOrShowFilters();
    var sChange = "";
    if ($(obj).attr("id") == "1") {
        $('#guest-visit-toggle').removeClass('active');
        $(".adv-fltr-guest").css("color", "#000");
        $(".adv-fltr-visit").css("color", "#f00");
        sVisitsOrGuests = "1";
        $("#adv-fltr-freq").css("display", "none");
        $("#adv-fltr-Chnl").css("display", "block");
        $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
        $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");

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
        $('#guest-visit-toggle').addClass('active');
        $(".adv-fltr-visit").css("color", "#000");
        $(".adv-fltr-guest").css("color", "#f00");
        sVisitsOrGuests = "2";
        $("#adv-fltr-freq").css("display", "block");
        $("#adv-fltr-Chnl").css("display", "none");
        //$("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").removeClass("Selected");
        //$("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").trigger("click");

        $("#RightPanelPartial #shopper_frequency_containerId ul li[name='MONTHLY']").removeClass("Selected");
        $("#RightPanelPartial #shopper_frequency_containerId ul li[name='MONTHLY']").trigger("click");


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
            $(".beverageItems ul div[uniqueid='1']").trigger("click");
        $(".MeasureType .trip").hide();
        $(".MeasureType .Shopper").show();
    }

    //LoadGroupTypeHeaderName(sFilterData);
    //LoadGroupTypeNames(sFilterData);

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
        $("#MeasureTypeHeaderMainTrip ul li").css("display", "none");
    }
    else if (sVisitsOrGuests == 2) {
        $("#MeasureTypeHeaderMainShopper").show();
        $("#MeasureTypeHeaderMainShopper ul li").css("display", "none");
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
        $("#MeasureTypeHeaderMainShopper").show();
        $("#MeasureTypeHeaderMainShopper ul li").show();
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
            html += "<li Name=\"" + object.MetricItem + "\" style=\"display:table;min-height:22px\">";
            html += "<div id=\"" + object.MetrticItemId + "\" onclick=\"SelectBevComparison(this);\" Name=\"" + object.MetricItem + "\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.MetrticItemId + "\" FilterTypeId=\"" + object.FilterTypeId + "\" uniqueid=\"" + object.UniqueId + "\"  Name=\"" + object.MetricItem + "\" FullName=" + object.MetricItem + " data-isselectable=\"true\">" + object.MetricItem + "</span></div>";

            html += "</li>";
            index++;
        }
        html += "</ul>";

        $("#BGMNonBeverageDiv").html("");
        $("#BGMNonBeverageDiv").append(html);
        SetScroll($("#BGMNonBeverageDiv"), "#393939", 0, 0, 0, 0, 8);
    }
}
function Displaybevnonbevdivs(obj) {
    if ($(obj).attr("id") == "1") {
        $("#BeverageOrCategoryContent").show();
        $("#BGMNonBeverageDiv").hide();
        $(".Beverage").hide();
        $("#beverageHeadingLevel2").hide();
        $(".Beverages").css("width", "auto");
    }
    else {
        $("#BGMNonBeverageDiv").show();
        $("#BeverageOrCategoryContent").hide();
        $(".Beverage").hide();
        $("#beverageHeadingLevel2").hide();
        $(".Beverages").css("width", "auto");
    }
    $(".AdvancedFiltersDemoHeading #beverageHeadingLevel1").text($(obj).attr("name").toUpperCase());
    $(".AdvancedFiltersDemoHeading #beverageHeadingLevel1").show();
}
//Activete current page
function ActivateCurrentPage() {

    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }
    if (currentpage != undefined && currentpage != null && currentpage != "") {
        $("#LeftPanel").show();
        $("#FilterHeader").show();
    }

    //Export ppt icon
    $(".ExportToPPT").hide();
    if ((currentpage.indexOf("hdn-chart") > -1 || (currentpage.indexOf("hdn-e-commerce-chart") > -1) || currentpage.indexOf("hdn-report") > -1
        || currentpage.indexOf("hdn-analysis") > -1) && (currentpage != "hdn-analysis-crossretailerfrequencies" && currentpage != "hdn-analysis-crossretailerimageries" && currentpage != "hdn-analysis-acrosstrips" && !(currentpage.indexOf("hdn-report") > -1))) {
        $(".ExportToPPT").show();
    }

    //Export excel icon
    $(".ExportToExcel").hide();
    if (currentpage != "hdn-analysis-acrosstrips" && !(currentpage.indexOf("hdn-report") > -1)) {
        $(".ExportToExcel").show();
    }

    //
    SetStatTesting(currentpage);
    if (currentpage == "hdn-dashboard-demographic") {
        $("#MenuHeader #Dashboard span").trigger("click");
        $("#dashboard-demo").css("color", "red");
        $("#Page-Lavel").html("");
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
        $("#SubMenuHeader #e-commerce-tbl-sites").css("color", "#ea1f2a");
        $("#SubMenuHeader #tbl-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #e-commerce-tbl-sites-arrow").css("border-top", "9px solid #ea1f2a");
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
    else if (currentpage == "hdn-e-commerce-chart-comparesites") {
        $("#MenuHeader #Charts span").trigger("click");
        $("#SubMenuHeader #e-commerce-chart-sites").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #e-commerce-chart-sites-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-chart-retailerdeepdive") {
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
        $("#SubMenuHeader #e-commerce-chart-sites").css("color", "#ea1f2a");
        $("#SubMenuHeader #chart-menu-e-commerce").addClass("Selected-Menu");
        $("#SubMenuHeader #e-commerce-chart-sites-arrow").css("border-top", "9px solid #ea1f2a");
        $("#Page-Lavel").html("");
        $("#PIT-TREND").show();
    }
    else if (currentpage == "hdn-chart-beveragedeepdive") {
        $("#GroupType").show();
        $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    }

    else if (currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive"
        || currentpage == "hdn-report-beveragespurchasedetailsdeepdive"
        || currentpage == "hdn-report-comparebeveragesmonthlypluspurchasers"
        || currentpage == "hdn-report-comparebeveragespurchasedetails"
        || currentpage == "hdn-report-compareretailerspathtopurchase"
        || currentpage == "hdn-report-compareretailersshoppers"
        || currentpage == "hdn-report-retailerspathtopurchasedeepdive"
        || currentpage == "hdn-report-retailersshopperdeepdive") {
        $("#MenuHeader #Reports span").trigger("click");
        $(".reports-menu").hide();
        $("#Frequency").show();
        $("#Page-Lavel").html("");
    }
    else if (currentpage == "hdn-analysis-acrossshopper") {
        $(".timeperiod").show();
        $("#Left-Frequency").show();
    }
    else if (currentpage == "hdn-analysis-withinshopper") {
        $("#Advanced-Analytics-Select-Variable").show();
    }
    else if (currentpage == "hdn-analysis-crossretailerimageries") {
        $("#Left-Frequency").show();
    }
    $(".Item .Active").parent("li").css("border-right", "3px solid rgb(234, 31, 42)");
}
function ShowOrHideVisitFilters() {
    //added by Nagaraju for Visit adv flters
    //date: 24-04-2017  
    if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() == "online order") {
        $(".advanced-seperator").show();
        $(".visit-adv-filters ul li").show();       
    }
    else {
        $(".visit-adv-filters").hide();
        if (sBevarageSelctionType.length == 0) {
            $(".advanced-seperator").hide();
        }
        $(".visit-adv-filters ul li").hide();
        $(".visit-adv-filters ul li[name='Item Purchased']").show();       
        if (SelectedAdvFilterList.length > 0) {
            var advfilterlength = SelectedAdvFilterList.length;
            for (var i = advfilterlength - 1; i >= 0; i--) {
                if (SelectedAdvFilterList[i].parentName.toLowerCase() != "item purchased") {
                    //$(".VisistsFilterDiv .DemographicList[name='" + SelectedAdvFilterList[i].parentName + "'] ul .lft-popup-ele").removeClass("Selected")
                    $(".VisistsFilterDiv .Lavel ul li[uniqueid='" + SelectedAdvFilterList[i].UniqueId + "']").removeClass("Selected")
                    SelectedAdvFilterList.splice(i, 1);
                }
            }
        }
    }
    $(".visit-adv-filters").show();
    if (SelectedFrequencyList.length > 0)
        {
        if ((SelectedFrequencyList[0].Name.toLocaleLowerCase() == "online order" && tabname.toLocaleLowerCase() == "online orders")
            || (SelectedFrequencyList[0].Name.toLocaleLowerCase() == "auto-replenishment" && tabname.toLocaleLowerCase() == "auto replenished deliveries")) {
            $("#adv-fltr-ordertype").hide();
        }
    }
    if (TabType == "shopper") {
        $(".adv-fltr-suboptions-list-container").hide();
        $(".adv-fltr-suboptions-list-container li[name='Item Purchased']").hide();
    }
    else {
        $(".adv-fltr-suboptions-list-container").show();
        $(".adv-fltr-suboptions-list-container li[name='Item Purchased']").show();
    }
}
function HideOrShowFilters() {
    if (currentpage.indexOf("hdn-e-commerce") > -1 || currentpage.indexOf("hdn-tbl") > -1 || currentpage.indexOf("hdn-chart") > -1 || currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
        if (currentpage.indexOf("chart") > -1) {
            if (TabType == "trips" || TabType == "online") {
                $(".rgt-cntrl-frequency-Conatiner ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='TOTAL VISITS']").show();

                $(".shopperDiv").hide();
                $(".tripsDiv").show();
                $(".freq-seperator").hide();
                $(".adv-fltr-details").show();
            }

            else if (TabType == "shopper" || $("#guest-visit-toggle").is(":checked") == true || (sVisitsOrGuests == "2")) {
                RemoveTripsFrequency();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='MAIN STORE/FAVORITE STORE']").show();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='TOTAL VISITS']").hide();

                $(".tripsDiv").hide();
                $(".freq-seperator").show();
                $(".shopperDiv").show();
                if (currentpage.indexOf("retailer") > -1) {
                    //$("#adv-bevselectiontype-freq").show();
                    $("#adv-bevselectiontype-freq").hide();
                    $(".freq-seperator").hide();
                    sBevarageSelctionType = [];
                }
                else {
                    $("#adv-bevselectiontype-freq").hide();
                    $(".freq-seperator").hide();
                    sBevarageSelctionType = [];
                }
                $(".adv-fltr-details").show();
            }
            else {
                //trips filtesr
                $(".adv-fltr-suboptions-list-container").show();
                //beverage frequency            
                //shopper frequency
                $(".rgt-cntrl-frequency-Conatiner ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='TOTAL VISITS']").show();

                $(".tripsDiv").hide();
                $(".freq-seperator").hide();
                $(".shopperDiv").hide();
                $(".adv-fltr-details").hide();
            }
        }

        else {
            if (TabType == "trips" || TabType == "online") {
                $(".rgt-cntrl-frequency-Conatiner ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='TOTAL VISITS']").show();

                $(".shopperDiv").hide();
                $(".tripsDiv").show();
                $(".freq-seperator").hide();
                $(".adv-fltr-details").show();
            }
            else if (TabType == "shopper") {
                RemoveTripsFrequency();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='MAIN STORE/FAVORITE STORE']").show();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='TOTAL VISITS']").hide();

                $(".tripsDiv").hide();
                $(".freq-seperator").show();
                $(".shopperDiv").show();
                if ((currentpage.indexOf("retailer") > -1) && !(currentpage.indexOf("chart") > -1)) {
                    $("#adv-bevselectiontype-freq").show();
                    $(".freq-seperator").show();
                }
                else {
                    $("#adv-bevselectiontype-freq").hide();
                    $(".freq-seperator").hide();
                    sBevarageSelctionType = [];
                }
                $(".adv-fltr-details").show();
            }
            else {
                //trips filtesr
                $(".adv-fltr-suboptions-list-container").show();
                //beverage frequency            
                //shopper frequency
                $(".rgt-cntrl-frequency-Conatiner ul li[name='MAIN STORE/FAVORITE STORE']").hide();
                $(".rgt-cntrl-frequency-Conatiner ul li[name='TOTAL VISITS']").show();

                $(".tripsDiv").hide();
                $(".freq-seperator").hide();
                $(".shopperDiv").hide();
                $(".adv-fltr-details").hide();
            }
        }
    }
    if (currentpage.indexOf("chart") > -1 && currentpage.indexOf("hdn-e-commerce") > -1)
        $(".freq-seperator").hide();
    ShowSelectedFilters();
    ShowOrHideVisitFilters();
}
function SetDefaultValues() {
    if (currentpage == "hdn-e-commerce-tbl-comparesites") {
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
    }
    else if (currentpage == "hdn-dashboard-pathtopurchase") {
        $("#GroupType").attr("class", "FilterMenu DashboardDemographics classMouseHover");
        $("#Retailers").attr("class", "FilterMenu DashboardRetailers classMouseHover");
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#dashboard-pathtopurchase-size-skew").show();
    }
    else if (currentpage == "hdn-e-commerce-tbl-sitedeepdive") {
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
        $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
        $("#GroupType").show();
        ModuleBlock = "PIT";
    }
    else if (currentpage == "hdn-e-commerce-chart-comparesites") {
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
        $(".adv-fltr-toggle-container").hide();
    }
    else if (currentpage == "hdn-e-commerce-chart-sitedeepdive") {
        $("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#TimeBlock ul li[name='12MMT']").trigger("click");
        //$("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
        $(".adv-fltr-toggle-container").hide();
    }
    FilterSelectionLimitText();
    HideOrShowFilters();
}
//Show Selected Filters
function ShowSelectedFilters() {
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
                html += "<div class=\"selected-filter\">, " + Comparisonlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveComparison(this);\" Id=\"" + Comparisonlist[i].Id + "\" name=\"" + Comparisonlist[i].Name + "\" dbname=\"" + Comparisonlist[i].DBName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + Comparisonlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveComparison(this);\" Id=\"" + Comparisonlist[i].Id + "\" name=\"" + Comparisonlist[i].Name + "\" dbname=\"" + Comparisonlist[i].DBName + "\"></a></div>";
            htmlText += Comparisonlist[i].Name;
        }
    }
    //For Site
    if (Sites.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < Sites.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + Sites[i].Name.trim() + "<a class=\"remove-item\" onclick=\"RemoveSite(this);\" Id=\"" + Sites[i].UniqueId + "\" name=\"" + Sites[i].Name.trim() + "\" dbname=\"" + Sites[i].DBName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + Sites[i].Name.trim() + "<a class=\"remove-item\" onclick=\"RemoveSite(this);\" Id=\"" + Sites[i].UniqueId + "\" name=\"" + Sites[i].Name.trim() + "\" dbname=\"" + Sites[i].DBName + "\"></a></div>";
            htmlText += Sites[i].Name.toString().trim();
        }
    }
    //For Channel Retailers
    if (ComparisonBevlist.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < ComparisonBevlist.length; i++) {
            if (i > 0)
                html += "<div class=\"selected-filter\">, " + ComparisonBevlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveBevComparison(this);\" Id=\"" + ComparisonBevlist[i].Id + "\" name=\"" + ComparisonBevlist[i].Name + "\" dbname=\"" + ComparisonBevlist[i].DBName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + ComparisonBevlist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveBevComparison(this);\" Id=\"" + ComparisonBevlist[i].Id + "\" name=\"" + ComparisonBevlist[i].Name + "\" dbname=\"" + ComparisonBevlist[i].DBName + "\"></a></div>";
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
        var sText = "";
        if (currentpage.indexOf("beverage") > 0)
            sText = "MONTHLY PURCHASE";
        else
            sText = "FREQUENCY";

        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < SelectedFrequencyList.length; i++) {
            if (i > 0)

                html += "<div class=\"selected-filter\">, " + SelectedFrequencyList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveFrequency(this);\" UniqueId=\"" + SelectedFrequencyList[i].UniqueId + "\" Id=\"" + SelectedFrequencyList[i].Id + "\" name=\"" + SelectedFrequencyList[i].Name + "\" Fullname=\"" + SelectedFrequencyList[i].FullName + "\"></a></div>";

            else
                html += "<div class=\"selected-filter\">" + SelectedFrequencyList[i].Name + "</div>";

            htmlText += SelectedFrequencyList[i].Name;
        }
    }

    //For trips Frequency
    if (SelectedTripsFrequencyList.length > 0) {
        if (html != "<div>")
            html += "<div class=\"seperater\">|</div>";

        for (var i = 0; i < SelectedTripsFrequencyList.length; i++) {
            html += "<div class=\"selected-filter\">" + SelectedTripsFrequencyList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveTripsFrequency();\" UniqueId=\"" + SelectedTripsFrequencyList[i].UniqueId + "\" Id=\"" + SelectedTripsFrequencyList[i].Id + "\" name=\"" + SelectedTripsFrequencyList[i].Name + "\" Fullname=\"" + SelectedTripsFrequencyList[i].FullName + "\"></a></div>";
                    
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

    //For Channels
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
                html += "<div class=\"selected-filter\">, " + SelectedAdvancedAnalyticsList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveAdvanceAnalytics(this,'" + SelectedAdvancedAnalyticsList[i].contentid + "');\" uniqueid=\"" + SelectedAdvancedAnalyticsList[i].UniqueFilterId + "\" Id=\"" + SelectedAdvancedAnalyticsList[i].Id + "\" name=\"" + SelectedAdvancedAnalyticsList[i].Name + "\" Fullname=\"" + SelectedAdvancedAnalyticsList[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + SelectedAdvancedAnalyticsList[i].Name + " <a class=\"remove-item\" onclick=\"RemoveAdvanceAnalytics(this,'" + SelectedAdvancedAnalyticsList[i].contentid + "');\"  uniqueid=\"" + SelectedAdvancedAnalyticsList[i].UniqueFilterId + "\" Id=\"" + SelectedAdvancedAnalyticsList[i].Id + "\" name=\"" + SelectedAdvancedAnalyticsList[i].Name + "\" Fullname=\"" + SelectedAdvancedAnalyticsList[i].FullName + "\"></a></div>";
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
                html += "<div class=\"selected-filter\">, " + Measurelist[0].metriclist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveMeasure(this);\" uniqueid=\"" + Measurelist[0].metriclist[i].Id + "\" Id=\"" + Measurelist[0].metriclist[i].Id + "\" name=\"" + Measurelist[0].metriclist[i].Name + "\" Fullname=\"" + Measurelist[0].metriclist[i].FullName + "\"></a></div>";
            else
                html += "<div class=\"selected-filter\">" + Measurelist[0].metriclist[i].Name + " <a class=\"remove-item\" onclick=\"RemoveMeasure(this);\"  uniqueid=\"" + Measurelist[0].metriclist[i].Id + "\" Id=\"" + Measurelist[0].metriclist[i].Id + "\" name=\"" + Measurelist[0].metriclist[i].Name + "\" Fullname=\"" + Measurelist[0].metriclist[i].FullName + "\"></a></div>";
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
    //                label: x.split("|")[1],
    //                value: x.split("|")[0],
    //            });
    //            return {
    //                label: x.split("|")[1],
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
    //        //        var Obj = $("#GroupTypeHeaderContent ul li div span[id='" + ObjData.attr("parentlevelid").trim() + "' i][name='" + ObjData.attr("parentlevelname").trim() + "']");
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentlevelid").trim())
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
    //        //        var Obj = $("#GroupTypeContent div[name='Geography'] span[name='" + ObjData.attr("parentlevelname").trim() + "']").parent();
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
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentlevelid").trim())
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
    //        //        var ObjData = $(".rgt-cntrl-frequency-Conatiner span[name='" + ui.item.label + "' i]").parent();
    //        //    else
    //        //        var ObjData = $(".rgt-cntrl-frequency-Conatiner * [name='" + ui.item.label + "' i][id='" + ui.item.value + "' i][onClick='SelectFrequency(this);']");
    //        //    if (ObjData.length <= 0) {
    //        //        ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "' i]").parent();
    //        //        if (ObjData.length > 0) {
    //        //            var Obj = $(".rgt-cntrl-frequency-Conatiner span[name='" + $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "' i]").attr("parent") + "' i]").parent()
    //        //            DisplaySecondaryFrequency(Obj);
    //        //        }
    //        //    }
    //        //    if (ObjData.length <= 0)
    //        //        ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel2 * [name='" + ui.item.label + "' i]").parent();
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
    //        SelectComparison(CompObj);
    //    }
    //    else if (SearchFor == "Sites") {
    //        $("#Search-Site-AdvancedFilters").val(ui.item.label);
    //        //var SiteObj = $("#PrimarySiteFilterContent .lft-popup-ele[uniqueid='" + ui.item.value + "']");
    //        //if (SiteObj.length <= 0)
    //        //    SiteObj = $("#SecondarSiteFilterContent .lft-popup-ele[uniqueid='" + ui.item.value + "']");
    //        //if (SiteObj.length <= 0)
    //        var SiteObj = $(".SiteFilters .lft-popup-ele[uniqueid='" + ui.item.value + "']");
    //        SelectSite(SiteObj);
    //    }
    //    else if (SearchFor == "Beverage") {
    //        $("#Search-Beverages").val(ui.item.label);
    //        var CompObj = $(".Beverage .Comparison[uniqueid='" + ui.item.value + "']");
    //        if (CompObj.length <= 0)
    //            CompObj = $("#BeverageOrCategoryContent ul li span[uniqueid='" + ui.item.value + "']");
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
    //                        $("#RightPanelPartial #frequency_containerId_trips ul li[name='TOTAL VISITS']").trigger("click");
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
    //                        $("#RightPanelPartial #frequency_containerId_trips ul li[name='MONTHLY +']").trigger("click");
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
    //                        $("#RightPanelPartial #frequency_containerId_trips ul li[name='MONTHLY +']").trigger("click");
    //                        $('#guest-visit-toggle').addClass('active');
    //                        $(".adv-fltr-visit").css("color", "#000");
    //                        $(".adv-fltr-guest").css("color", "#f00");
    //                        sVisitsOrGuests = "2";
    //                        $("#adv-fltr-freq").css("display", "block");
    //                        $("#adv-fltr-Chnl").css("display", "none");
    //                        $("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").removeClass("Selected");
    //                        $("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").trigger("click");
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
    //                    $("#RightPanelPartial #frequency_containerId_trips ul li[name='TOTAL VISITS']").trigger("click");

    //                    $('#guest-visit-toggle').removeClass('active');
    //                    $(".adv-fltr-guest").css("color", "#000");
    //                    $(".adv-fltr-visit").css("color", "#f00");
    //                    sVisitsOrGuests = "1";
    //                    $("#adv-fltr-freq").css("display", "none");
    //                    $("#adv-fltr-Chnl").css("display", "block");
    //                    $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
    //                    $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
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
    //                _metriclist.push({ Id: $(this).attr("uniqueid"), Name: $(this).html(), Type: $(this).attr("Type"), SelType: $(this).attr("seltypeid"), FilterType: $(this).attr("filtertype") });
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
                    
    //                if ((currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") && (Measurelist.length > 0) && (Measurelist[0].metriclist.length > 0) && (TabType.toUpperCase() == "TRIP" || TabType.toUpperCase() == "TRIPS")) {
    //                    sClosePopupStatus = 1;
    //                    if (((TabType.toUpperCase() == "TRIP" || TabType.toUpperCase() == "TRIPS") && Measurelist[0].filtertypeid == 2)) {
    //                        tabname = Measurelist[0].metriclist[0].FilterType.toUpperCase();
    //                        LoadTripsFrequencyFilter(sFilterData);
    //                        TabType = "trips";
    //                        $("#RightPanelPartial .rgt-cntrl-ordertype ul div[name='ONLINE ORDER']").removeClass("Selected");
    //                        $("#RightPanelPartial .rgt-cntrl-ordertype ul div[name='ONLINE ORDER']").trigger("click");
    //                        SearchFilters_RightPanel("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
    //                    }
    //                    else if ((TabType.toUpperCase() == "TRIP" || TabType.toUpperCase() == "TRIPS") && Measurelist[0].filtertypeid == 3) {
    //                        tabname = Measurelist[0].metriclist[0].FilterType.toUpperCase();
    //                        LoadTripsFrequencyFilter(sFilterData);
    //                        TabType = "trips";
    //                        $("#RightPanelPartial .rgt-cntrl-ordertype ul div[name='AUTO-REPLENISHMENT']").removeClass("Selected");
    //                        $("#RightPanelPartial .rgt-cntrl-ordertype ul div[name='AUTO-REPLENISHMENT']").trigger("click");
    //                        SearchFilters_RightPanel("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
    //                    }
    //                    else {
    //                        tabname = Measurelist[0].metriclist[0].FilterType.toUpperCase();
    //                        LoadTripsFrequencyFilter(sFilterData);
    //                        $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").removeClass("Selected");
    //                        $("#RightPanelPartial #visit_frequency_containerId ul li[name='TOTAL']").trigger("click");
    //                        SearchFilters_RightPanel("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
    //                    }
    //                    sClosePopupStatus = 0;

    //                    }
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
    //        isSearch = "0";
    //        $("#LinkForCharts").show();

    //        if (TabType.toString() == "") {
    //            TabType = "trips";
    //            $("#RightPanelPartial #frequency_containerId_trips ul li[name='TOTAL VISITS']").trigger("click");
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
    //            if (ObjData.length > 0)
    //                isGeo = "1";
    //        }
    //        if (ObjData.length <= 0) {
    //            ObjData = $("#GroupTypeGeoContentSub ul span[uniqueid='" + ui.item.value + "']");
    //            if (ObjData.length > 0)
    //                isGeo = "1";
    //        }

    //        if (isGeo != "1")
    //            var Obj = $("#GroupTypeHeaderContent ul li div span[id='" + ObjData.attr("parentlevelid").trim() + "'][name='" + ObjData.attr("parentlevelname").trim().replace("'", "") + "']");
    //        else if (isGeo == "1")
    //            var Obj = $("#GroupTypeContent div[name='Geography'] span[name='" + ObjData.attr("parentlevelname").trim().replace("'", "") + "']").parent();

    //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentlevelid").trim())
    //            SelecGroupMetricName(ObjData.parent());
    //        else {
    //            //SelecGroup(Obj.parent().parent());
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
    //        //    ObjData = $("#GroupTypeContentSub ul span[uniqueid='" + ui.item.value + "']");
    //        //    //if (ObjData.length > 0)
    //        //    //    isGeo = "1";
    //        //}

    //        //if (ObjData.length > 0) {
    //        //    if (isGeo == "0") {
    //        //        var Obj = $("#GroupTypeHeaderContent ul li div span[id='" + ObjData.attr("parentlevelid").trim() + "' i][name='" + ObjData.attr("parentlevelname").trim() + "']");
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentlevelid").trim())
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
    //        //        var Obj = $("#GroupTypeContent div[name='Geography'] span[name='" + ObjData.attr("parentlevelname").trim() + "']").parent();
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
    //        //        if (Grouplist.length > 0 && Grouplist[0].parentId == ObjData.attr("parentlevelid").trim())
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
    //            var ObjData = $(".rgt-cntrl-frequency-Conatiner span[name='" + ui.item.label + "']").parent();
    //        else
    //            var ObjData = $(".rgt-cntrl-frequency-Conatiner * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectFrequency(this);']");
    //        if (ObjData.length <= 0) {
    //            ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").parent();
    //            if (ObjData.length > 0) {
    //                var Obj = $(".rgt-cntrl-frequency-Conatiner span[name='" + $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").attr("parent") + "']").parent()
    //                DisplaySecondaryFrequency(Obj);
    //            }
    //        }
    //        if (ObjData.length <= 0)
    //            ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel2 * [name='" + ui.item.label + "']").parent();
    //        //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")

    //        SelectFrequency(ObjData);
    //        //else
    //        //    DisplaySecondaryFrequency(ObjData);
    //    }
    //    if (SearchFor == "AdvancedAnalytics") {
    //        $("#advanced-analytics-Channel-Shopper").hide();
    //        $("#advanced-analytics-Retailer-Trips").hide();
    //        $("#advanced-analytics-Channel-Trips").hide();
    //        $("#advanced-analytics-Retailer-Shopper").hide();
    //        $("#advancedanalyticsHeadingLevel1").hide();
    //        $("#advancedanalyticsHeadingLevel2").hide();
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

    //        if (TabType == "trips" || TabType == "online") {
    //            if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name.toLocaleLowerCase() != "total visits") {
    //                SelectedFrequencyList = [];
    //                $("#RightPanelPartial #frequency_containerId_trips ul li[name='TOTAL']").trigger("click");
    //                $("#Advanced-Analytics-Select-Variable").trigger("click");
    //            }
    //        }
    //        else if (TabType == "shopper") {
    //            if (SelectedFrequencyList.length > 0 && SelectedFrequencyList[0].Name != "Monthly +") {
    //                SelectedFrequencyList = [];
    //                $("#RightPanelPartial #frequency_containerId_trips ul li[name='MONTHLY +']").trigger("click");
    //                $("#Advanced-Analytics-Select-Variable").trigger("click");
    //            }
    //        }

    //    }
    //    HideOrShowFilters();
    //    e.stopImmediatePropagation();
    //});
}
//End

//demo adv filter Images Position
function GetDemographyImagePosition() {
    var DemographyImageDetails = [
        { DemographyName: "Gender", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "7px -297px;" },
        { DemographyName: "Age", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-47px -297px;" },
        { DemographyName: "Detailed Age", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-97px -297px;" },
        { DemographyName: "Age/Gender", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-203px -297px;" },
        { DemographyName: "Age/Ethnicity", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-309px -297px;" },
       // { DemographyName: "Ethnicity", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-309px -297px;" },//Ethnicity
        { DemographyName: "Gender/Ethnicity", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-367px -297px;" },
        { DemographyName: "Race Ethinicity", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-253px -297px;" },
        { DemographyName: "HH Total", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-422px -297px;" },
        { DemographyName: "HH Children", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-529px -297px;" },
        { DemographyName: "HH Adults", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-477px -297px;" },
        { DemographyName: "HCM", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-580px -297px;" },//HH Size
        { DemographyName: "Parental Identification", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-631px -297px;" },
        { DemographyName: "Employment Status", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-785px -297px;" },
        { DemographyName: "Employment Status Detailed", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-840px -297px;" },
        { DemographyName: "HH Income", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-732px -297px;" },
        { DemographyName: "Education", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-894px -297px;" },
        { DemographyName: "Socioeconomic", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-894px -297px;" },
        { DemographyName: "Education", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-894px -297px;" },
        { DemographyName: "Geography", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-1055px  -297px;" },
        { DemographyName: "Hispanic_Acculturation", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-1055px  -297px;" },//Attitudinal Segment//Attitudinal Statements - Top 2 Box//% HH Shopping Personally Responsible For
        { DemographyName: "Marital Status", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-680px -297px;" },
        /*Demographic Filter*/
    ];
    return DemographyImageDetails;
}
function GetBeverageImagePosition() {
    var BeverageImageDetails = [
        { BeverageName: "Regular (Non-Diet) Carbonated Soft Drink", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "7px -297px;" },
        { BeverageName: "Age", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-47px -297px;" },
        { BeverageName: "Detailed Age", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-97px -297px;" },
        { BeverageName: "Age/Gender", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-203px -297px;" },
        { BeverageName: "Age/Ethnicity", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-309px -297px;" },
        { BeverageName: "Gender/Ethnicity", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-367px -297px;" },
        { BeverageName: "Race Ethinicity", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-253px -297px;" },
        { BeverageName: "HH Total", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-422px -297px;" },
        { BeverageName: "HH Children", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-529px -297px;" },
        { BeverageName: "HH Adults", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-477px -297px;" },
        { BeverageName: "HCM", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-580px -297px;" },//HH Size
        { BeverageName: "Parental Identification", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-631px -297px;" },
        { BeverageName: "Employment Status", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-785px -297px;" },
        { BeverageName: "Employment Status Detailed", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-840px -297px;" },
        { BeverageName: "HH Income", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-732px -297px;" },
        { BeverageName: "Education", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-894px -297px;" },
        { BeverageName: "Socioeconomic", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-894px -297px;" },
        { BeverageName: "Education", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-894px -297px;" },
        { BeverageName: "Geography", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-1055px  -297px;" },
        { BeverageName: "Hispanic_Acculturation", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-1055px  -297px;" },//Attitudinal Segment//Attitudinal Statements - Top 2 Box//% HH Shopping Personally Responsible For
        { BeverageName: "Marital Status", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-680px -297px;" },
        /*Demographic Filter*/
    ];
    return BeverageImageDetails;
}
function GetSitesImagePosition() {
    var BeverageImageDetails = [
        //{ BeverageName: "Total Shopper", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "7px -297px;" },
        //{ BeverageName: "Total Online Shopper", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-72px -147px;" },
        //{ BeverageName: "Total Online Grocery Shopper", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-112px -147px;" },
        //{ BeverageName: "Sites", imageName: "../../Images/sprite_filter_icons.svg?id=2", imagePosition: "-112px -147px;" },
        /*Sites Retailer Filter*/
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
//    sessionStorage.clear();
//    window.location.href = $("#URLHome").val();
//}
//function SignOut() {
//    sessionStorage.clear();
//    window.location.href = $("#URLSignOut").val();
//}

//function GoToDashboard(page) {
//    if (page == "demographic") {
//        window.location.href = $("#URLDemographic").val();
//    }
//    else if (page == "brandhealth") {
//        window.location.href = $("#URLBrandhealth").val();
//    }
//    else if (page == "visits") {
//        window.location.href = $("#URLVisits").val();
//    }
//}
//added by Nagaraju for Groups prime filters Date: 04-03-2017
function LoadGroupsPrimeFilters(data) {
    html = "";
    html += "<ul>";
    var primegroups = [];
    if (currentpage.indexOf("beverage") > -1) {
        primegroups = data.TripsGroupsPrimeFilterlist;
    }
    else {
        primegroups = data.EcommShopperGroupsPrimeFilterList;
    }

    for (var i = 0; i < primegroups.length; i++) {
        var object = primegroups[i];
        html += "<li PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" onclick=\"ShowGroup(this);\">";
        html += "<div  class=\"FilterStringContainerdiv\" style=\"\">";
        html += "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\">" + object.PrimeFilterType + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
        html += "</div>";
        html += "</li>";
    }
    html += "</ul>";
    $("#PrimeGroupTypeHeaderContent").html("");
    $("#PrimeGroupTypeHeaderContent").html(html);
    SetScroll($("#PrimeGroupTypeHeaderContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}

function LoadSiteFilters(data) {
    html = "";
    var index = 0;
    var DataList = [];
    DataList = data.SitesList;
    var ImageDetails = GetSitesImagePosition();
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            var object = DataList[i];
            var sImageClassName = "";
            sImageClassName = _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.BeverageName == object.Name; })[0].imagePosition : "";
            if (index == 0)
                html += "<ul>";
            if (object.SiteList.length > 0) {
                html += "<li style=\"display:table;\">";
                html += "<div onclick=\"DisplaySecondarySiteFilter(this);\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" class=\"lft-popup-ele header FilterStringContainerdiv\" style=\"\">";

                if (sImageClassName == "")
                    html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>";
                else
                    html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>";

                html += "<span style=\"width:79%;\" class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div>";

                html += "</li>";
                index++;
            }
            else {
                html += "<li style=\"display:table;\">";
                html += "<div onclick=\"SelectSite(this);\" LevelId=\"" + object.LevelId + "\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" uniqueid=\"" + object.Id + "\" class=\"lft-popup-ele\" style=\"height:31px;\">";

                if (sImageClassName == "")
                    html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-position:" + sImageClassName + "\"></span>";
                else
                    html += "<span class=\"img-retailer\" style=\"width:32px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + sImageClassName + "\"></span>";

                html += "<span style=\"width:83%;height:31px;\" class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span></div>";

                AllSites.push(object.Id + "|" + object.Name);
                html += "</li>";
                index++;
            }
        }
        html += "</ul>";

        $("#PrimarySiteFilterList").html("");
        $("#PrimarySiteFilterList").html(html);
        SetScroll($("#PrimarySiteFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }
    LoadSecondarySiteFilters(data);
}
function LoadSecondarySiteFilters(data) {
    html = "";
    var thirdLevelhtml = "";
    var DataList = [];
    DataList = data.SitesList;
    if (data != null) {
        for (var i = 0; i < DataList.length; i++) {
            if (DataList[i].SiteList.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + DataList[i].Id + "\" Name=\"" + DataList[i].Name + "\" FullName=\"" + DataList[i].FullName + "\" style=\"overflow-y:auto;display:none;\"><ul>";
                for (var j = 0; j < DataList[i].SiteList.length; j++) {
                    var object = DataList[i].SiteList[j];

                    html += "<div onclick=\"SelectSite(this);\" LevelId=\"" + object.LevelId + "\" Name=\"" + object.Name + "\" id=\"" + object.Id + "\" uniqueid=\"" + object.Id + "\" class=\"lft-popup-ele\" style=\"height:31px;\"><span style=\"height:31px;\" class=\"lft-popup-ele-label\" FullName=\"" + object.MetricItemName + "\" DBName=\"" + object.MetricItemName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\" id=\"" + object.MetricItemId + "\"  Name=\"" + object.MetricItemName + "\" parent=\"" + object.ParentId + "\"  data-isselectable=\"true\">" + object.MetricItemName + "</span></div>";
                    AllSites.push(object.Id + "|" + object.Name);
                }
                html += "</ul></div>";
            }
        }
    }
    $("#SecondarSiteFilterContent").html("");
    $("#SecondarSiteFilterContent").html(html);
}
function DisplaySecondarySiteFilter(obj) {
    var sPrimaryDemo = $(obj).parent().parent().parent()[0];

    //$(sPrimaryDemo).find(".Selected").removeClass("Selected");
    //$(obj).addClass("Selected");
    $("#PrimarySiteFilterContent .lft-popup-ele-next").removeClass("sidearrw_OnCLick");
    $("#PrimarySiteFilterContent .lft-popup-ele-next").addClass("sidearrw");

    $(obj).find(".lft-popup-ele-next").removeClass("sidearrw");
    $(obj).find(".lft-popup-ele-next").addClass("sidearrw_OnCLick");

    $("#SecondarSiteFilterContent .DemographicList").hide();
    $("#ThirdLevelSiteFilterContent .DemographicList").hide();
    $("#ThirdLevelSiteFilterContent").hide();

    var maxlevel = parseInt($(obj).attr("lavels"));
    for (var i = 2; i <= maxlevel; i++) {
        $(".SiteFilters .Lavel" + i).show();
        $(".SiteFilters .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
    }

    //$("#SecondarSiteFilterContent").show();
    //$("#SecondarSiteFilterContent div[name='" + $(obj).attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #SiteHeadingLevel2").text($(obj).attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #SiteHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #SiteHeadingLevel2").css("width", "252px");
    $(".AdvancedFiltersDemoHeading #SiteHeadingLevel3").hide();
    SetScroll($("#SecondarSiteFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    SetScroll($("#Level3SiteFilterContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplayHeightDynamicCalculation(filterName) {
    switch (filterName) {
        case "beverage": {
            var sHeight = 100 - ((($(".Beverages .Search-Filter").height()) / ($(".Beverages").height() + 9) * 100) + (($(".Beverages .AdvancedFiltersDemoHeading").height() + 1) / ($(".Beverages").height() + 9) * 100)) - 3;
            $("#BevDivId").css("height", sHeight + "%");
        }
        case "retailer": {
            var sHeight = 100 - ((($(".Retailers .Search-Filter").height()) / ($(".Retailers").height() + 9) * 100) + (($(".Retailers .AdvancedFiltersDemoHeading").height() + 1) / ($(".Retailers").height() + 9) * 100)) - 4;
            $("#RetailerDivId").css("height", sHeight + "%");
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
                var sHeight = 100 - ((($(".rgt-cntrl-SubFilter-Conatianer .Search-Filter").height()) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100) + (($(".rgt-cntrl-SubFilter-Conatianer .AdvancedFiltersDemoHeading").height() + 1) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100)) - 14;
            $("#VisistsFilterDivId").css("height", sHeight + "%");
        }
        case "Frequency": {
            var sHeight = 100 - ((($(".rgt-cntrl-frequency .Search-Filter").height()) / ($(".rgt-cntrl-frequency").height() + 9) * 100) + (($(".rgt-cntrl-frequency .AdvancedFiltersDemoHeading").height() + 1) / ($(".rgt-cntrl-frequency").height() + 9) * 100)) - 14;
            $("#frequency_containerId").css("height", sHeight + "%");
        }
    }
}
function SelectSite(obj) {
    CompCurrentId = GetArrayId(obj, Sites);
    if (ModuleBlock == "TREND" || ModuleBlock == "PIT") {
        Sites = [];
        $("#PrimarySiteFilterList .lft-popup-ele").not("#PrimarySiteFilterList .header").removeClass("Selected");
        $("#PrimarySiteFilterList .lft-popup-ele").not("#PrimarySiteFilterList .header").find(".ArrowContainerdiv").css("background-color", "#58554D");
        $("#SecondarSiteFilterContent .lft-popup-ele").not("#PrimarySiteFilterList .header").removeClass("Selected");
        $("#SecondarSiteFilterContent .lft-popup-ele").not("#PrimarySiteFilterList .header").find(".ArrowContainerdiv").css("background-color", "#58554D");

        $(".SiteFilters .Lavel3 .lft-popup-ele").not("#PrimarySiteFilterList .header").removeClass("Selected");
        $(".SiteFilters .Lavel3 .lft-popup-ele").not("#PrimarySiteFilterList .header").find(".ArrowContainerdiv").css("background-color", "#58554D");

        if ($(obj).attr("levelid") == "1") {
            $("#SecondarSiteFilterContent").hide();
            $("#SiteHeadingLevel2").hide();
            $(".SiteFilters .Lavel3").hide();

            $("#PrimarySiteFilterList .lft-popup-ele").removeClass("Selected");
            $("#PrimarySiteFilterList .lft-popup-ele").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#SecondarSiteFilterContent .lft-popup-ele").removeClass("Selected");
            $("#SecondarSiteFilterContent .lft-popup-ele").find(".ArrowContainerdiv").css("background-color", "#58554D");

            $("#PrimarySiteFilterContent .lft-popup-ele-next").removeClass("sidearrw_OnCLick");
            $("#PrimarySiteFilterContent .lft-popup-ele-next").addClass("sidearrw");
        }
        $("#e-com-sites *").removeClass("Selected");
    }
    else {
        if (CompCurrentId == -1 && Sites.length == 11) {
            showMessage("YOU CAN MAKE UPTO 11 SELECTIONS");
            return false;
        }
        if ($(obj).attr("levelid") == "1") {
            $("#SecondarSiteFilterContent").hide();
            $("#SiteHeadingLevel2").hide();
            $(".SiteFilters .Lavel3").hide();

            $("#PrimarySiteFilterList .header").removeClass("Selected");
            $("#PrimarySiteFilterList .header").find(".ArrowContainerdiv").css("background-color", "#58554D");
            $("#PrimarySiteFilterContent .lft-popup-ele-next").removeClass("sidearrw_OnCLick");
            $("#PrimarySiteFilterContent .lft-popup-ele-next").addClass("sidearrw");
        }
    }


    if (CompCurrentId == -1) {
        $(obj).addClass("Selected");
        Sites.push({ Id: $(obj).attr("id"), Name: $(obj).attr("name"), UniqueId: $(obj).attr("uniqueid") });
    }
    else {
        $(obj).removeClass("Selected");
        $(obj).find(".ArrowContainerdiv").css("background-color", "#58554D");
        Sites.splice(CompCurrentId, 1);
    }
    ShowSelectedFilters();
    BuildDynamicTable();
}
function RemoveSite(obj) {
    //var SiteObj = $("#PrimarySiteFilterList .lft-popup-ele[uniqueid='" + $(obj).attr("Id") + "']");
    //if (SiteObj.length <= 0)
    //    SiteObj = $("#SecondarSiteFilterContent .lft-popup-ele[uniqueid='" + $(obj).attr("Id") + "']");
    var SiteObj = $(".SiteFilters .lft-popup-ele[uniqueid='" + $(obj).attr("Id") + "']");
    if (SiteObj.length <= 0) {
        SiteObj = $(".SiteFilters li[uniqueid='" + $(obj).attr("Id") + "']")
    }
    SelectSite(SiteObj);
}

function TempHideFn() {
    $(".StatArea").hide();
}
function Validate_Site() {
    if (Sites.length == 0 || Sites.length < 2) {
        showMessage("Please select minimum 2 Sites");
        return false;
    }
    return true;
}
function Validate_Group_Site() {
    if (Sites.length == 0) {
        showMessage("Please select Site");
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
function Validate_Common_Groups() {
    if (Grouplist.length == 11 && currentpage != "hdn-analysis-withintrips") {
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
function SetStyles() {
    $(".table-title").prev(".rowitem").children("ul").children("li").css("border", "0");
    $(".leftbody .rowitem ul").each(function (i) {
        $(".rightbody .rowitem ul").eq(i).height($(this).height());
    })
    SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
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

//----------Validation Block---------------->
//----> Compare Retailer -------->
function Validate_CompareRetailers() {
    if (Comparisonlist.length == 0) {
        showMessage("Please select Retailer");
        return false;
    }
    return true;
}

function Validate_CompareSites() {
    if (Sites.length == 0) {
        showMessage("Please select Site");
        return false;
    }
    return true;
}
//----> Compare Beverages -------->
function Validate_CompareBeverages() {
    if (ComparisonBevlist.length == 0) {
        showMessage("Please select Beverage");
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

function Validate_CompareRetailers_Charts() {
    if (Comparisonlist.length == 0 || Comparisonlist.length < 2) {
        showMessage("Please select atleast one Retailers");
        return false;
    }
    return true;
}
function Validate_Sites_Charts() {
    if (Sites.length == 0 || Sites.length < 2) {
        showMessage("Please select minimum 2 Sites");
        return false;
    }
    return true;
}

function Validate_CompareRetailers_AdvanceAnalysis() {
    if (Comparisonlist.length == 0 || Comparisonlist.length < 3) {
        showMessage("Please select minimum 3 Retailers");
        return false;
    }
    return true;
}

function Validate_CompareBeverages_Charts() {
    if (ComparisonBevlist.length == 0 || ComparisonBevlist.length < 2) {
        showMessage("Please select minimum 2 Beverages");
        return false;
    }
    return true;
}

function Validate_Group_AdvanceAnalysis() {
    if (Grouplist.length == 0 || Grouplist.length < 3) {
        showMessage("Please select minimum 3 Groups");
        return false;
    }
    return true;
}

function Validate_Group_Charts() {
    if (Grouplist.length == 0 || Grouplist.length < 2) {
        showMessage("Please select minimum 2 Groups");
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
    if (Grouplist.length == 11 && currentpage != "hdn-analysis-withintrips") {
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
            $("#BevDivId").css("height", sHeight + "%");
        }
        case "retailer": {
            var sHeight = 100 - ((($(".Retailers .Search-Filter").height()) / ($(".Retailers").height() + 9) * 100) + (($(".Retailers .AdvancedFiltersDemoHeading").height() + 1) / ($(".Retailers").height() + 9) * 100)) - 4;
            $("#RetailerDivId").css("height", sHeight + "%");
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
                var sHeight = 100 - ((($(".rgt-cntrl-SubFilter-Conatianer .Search-Filter").height()) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100) + (($(".rgt-cntrl-SubFilter-Conatianer .AdvancedFiltersDemoHeading").height() + 1) / ($(".rgt-cntrl-SubFilter-Conatianer").height() + 9) * 100)) - 14;
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
            var ObjData = $("#GroupTypeContent span[id='" + obj.toString() + "']").parent();
            if (ObjData.length <= 0)
                ObjData = $("#GroupTypeContentSub span[id='" + obj.toString() + "']").parent();
            if (ObjData.length <= 0)
                ObjData = $("#GroupTypeGeoContentSub span[id='" + obj.toString() + "']").parent();
            SelecGroupMetricName(ObjData);
        });
        if (Grouplist.length < 3)
            showMessage("Please select atleast Three Groups");
        else
            prepareContentArea();
    }
    else if (currentpage == "hdn-analysis-withinshopper") {
        _.each(CorrespondaceMapsLowSampleSizeVariables.split('|'), function (obj) {
            var CompObj = $(".Retailer .Comparison[id='" + obj.toString() + "']");
            if (CompObj.length <= 0)
                CompObj = $("#ChannelOrCategoryContent ul li span[id='" + obj.toString() + "']");
            SelectComparison(CompObj);
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
    var rightHeaderHeight = $(".rightheader").height();
    $(".leftbody").css("height", "calc(100% - " + (rightHeaderHeight + 3) + "px)");
    $(".rightbody").css("height", "calc(100% - " + (rightHeaderHeight + 3) + "px)");


    if (rightHeaderHeight != $(".leftheader").height())
        $(".leftheader").css("height", rightHeaderHeight - 0.5);
    var rowHeight = $(".rightheader .rowitem").height();
    if (rowHeight != $(".leftheader .rowitem").eq(0).height())
        $(".leftheader .rowitem").eq(0).css("height", rowHeight - 0.5);


    $(".leftbody .rowitem ul").each(function (i) {
        $(".rightbody .rowitem ul").eq(i).height($(this).height());
    })
    SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
}
function ShowFrequencyHeader() {
    $(".tbl-data-freqtxt").html("");
    if (TabType == "shopper")
        $(".tbl-data-freqtxt").html("ONLINE ORDER FREQUENCY");
    else
        $(".tbl-data-freqtxt").html("ONLINE ORDER TYPE");

    //$("#guestFrqncy").show();
}
function ClearChartReports() {
    $("#btnAddToExport").attr('chart-type', 'inactive');
    $("#btnViewSelections").attr('chart-type', 'inactive');
    $("#btnClearAll").attr('chart-type', 'inactive');

    $("#btnAddToExport").css("background", "lightgray");
    $("#btnViewSelections").css("background", "lightgray");
    $("#btnClearAll").css("background", "lightgray");
}

//function GetIShopFilters() {

//    window.app.db.get(1, function (saveData) {
//        if (saveData === null || saveData === undefined) {

//            var module = currentpage.split("-"); SelectSite(this);
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
                                    thirdLevelhtml += "<div MetricId=\"" + object.MetricId + "\" style=\"display: none;\" id=\"" + object.Id + "\" PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + objthirdlavel.Name + "\" MericName=\"" + object.Name + "\" Level=\"ThirdLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + objthirdlavel.isGeography + "\" FullName=\"" + objthirdlavel.FullName + "\" DBName=\"" + objthirdlavel.DBName + "\" UniqueId=\"" + objthirdlavel.UniqueId + "\" shopperdbname=\"" + objthirdlavel.ShopperDBName + "\" tripsdbname=\"" + objthirdlavel.TripsDBName + "\"  data-id=\"" + objthirdlavel.Id + "\" id=" + objthirdlavel.Id + "-" + objthirdlavel.MetricId + "-" + objthirdlavel.ParentId + " Name=\"" + objthirdlavel.Name + "\" parent=\"" + objthirdlavel.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + objthirdlavel.Name + "</span></div>";
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
                            html += "<div Uniqueid=\"" + object.Id + "\" Name=\"" + object.Name + "\" onclick=\"DisplayThirdLevelGroupFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object.isGeography + "\" FullName=\"" + object.FullName + "\" DBName=\"" + object.DBName + "\" UniqueId=\"" + object.UniqueId + "\" shopperdbname=\"" + object.ShopperDBName + "\" tripsdbname=\"" + object.TripsDBName + "\"  data-id=\"" + object.Id + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object.Name + "</span><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
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
                            if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                            else
                                fourthLevelhtml += "<div PrimeFilterType=\"" + object.PrimeFilterType + "\" FilterType=\"" + object.FilterType + "\" Name=\"" + object1.Name + "\" Level=\"FouthLevel\" onclick=\"SelecGroupMetricName(this);\" class=\"lft-popup-ele\" style=\"display:inline-flex;\"><span class=\"lft-popup-ele-label\" isGeography=\"" + object1.isGeography + "\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + datalist[i].Id.toString().trim() + " \" ParentLevelName=\" " + datalist[i].Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                            if (!IsItemExist(object.Name, AllTypes))
                                AllTypes.push(object1.UniqueId + "|" + object1.Name);
                        }
                        else {
                            if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
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
}
function SelecDefaultGeography(data) {
    var sResult = [];
    sResult = data.DefaultGeographyFiltersEcom;
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
}
function UpdateDefaultAdvancedFilterGeography() {
    html = "";
    var thirdLevelhtml = "";
    var fourthLevelhtml = "";
    var DataList = [];
    var AllDemographicsSub = [];
    //dGeo = SelecGeography();
    DataList = DefaultGeolist;
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
                                    if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span><span title=\"" + object1.ToolTip + "\" class=\"lft-popup-ele-next Geotooltipimage\"></div>";
                                    else
                                        fourthLevelhtml += "<div onclick=\"SelectDemographic(this);\" Level=\"FouthLevel\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" FullName=\"" + object1.FullName + "\" DBName=\"" + object1.DBName + "\" isGeography=\"true\" UniqueId=\"" + object1.UniqueId + "\" shopperdbname=\"" + object1.ShopperDBName + "\" tripsdbname=\"" + object1.TripsDBName + "\"  data-id=\"" + object1.Id + "\" id=" + object1.Id + "-" + object1.MetricId + "-" + object1.ParentId + " Name=\"" + object1.Name + "\" parent=\"" + object1.ParentId + "\" ParentLevelId=\" " + object.Id.toString().trim() + " \" ParentLevelName=\" " + object.Name.toString().trim() + " \" data-isselectable=\"true\">" + object1.Name + "</span></div>";
                                    AllDemographicsSub.push(object1.UniqueId + "|" + object1.Name);
                                }
                                else {
                                    if (object1.Name.replace("`", "") == "Albertsons/Safeway Corporate NET Trade Area" || object1.Name.replace("`", "") == "HEB Trade Area")
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
        { Name: "Time Period", cls: "TimePeriod", size: "medium", Text: "Select the time interval and end period " },
        { Name: "Retailers/Channels:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons:Select Comparisons", cls: "Retailers", size: "medium", Text: "Select the retailers/channels to compare:Select to determine the demographics or key metrics to be compared:Generate pre-defined retailer/channel overview PowerPoint reports:Create a Correspondence Map comparing retailers/channels across key metrics:Create a Correspondence Map of key metrics of a single retailer/channel:Select the retailers/channels to compare:Generate a table that shows a retailer/channel's shopper frequencies to all other priority retailers and channels:Genetrate a table that shows a retailer/channel shopper's perceptions of other priority retailers" },
        { Name: "Beverages - Compare Module:Beverages - Deep Dive Module:Product Selection", cls: "Beverages", size: "medium", Text: "Select the brands/categories to compare:Select a single brand/category for deep dive:Default products have been select,  click to select different products" },
{ Name: "Metric Comparisons", cls: "Group", size: "medium", Text: "Select the demographics or key metrics to compare" },
        { Name: "Advanced Filter", cls: "Demographics", size: "medium", Text: "Select additional demographic filters" },
          { Name: "Advanced Filter", cls: "DashboardDemographics", size: "medium", Text: "Select additional demographic or key metric filters" },
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
         { Name: "Shopper/Trips Toggle", cls: "shoppertrip-Toggle", size: "smalldark", Text: "Select to view shopper or online purchase based demographics" },
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
            $("#MouseHoverSmallerDiv").css("min-height","95px");
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
            }
            else {
                if ($(this).attr("id") == "btnClearAll") {
                    $("#MouseHoverSmallerDiv").css({
                        position: "absolute",
                        top: (pos.top + height - 6) + "px",
                        left: (pos.left - widthdiv + $(this).width() + 15) + "px",
                    }).show();
                }
                else if ($(this).hasClass("breadcrumb-open")) {
                    $("#MouseHoverSmallerDiv").css({
                        position: "absolute",
                        top: (pos.top + height + 20) + "px",
                        left: (pos.left - widthdiv - 10) + "px",
                    }).show();
                }
                else {
                    $("#MouseHoverSmallerDiv").css({
                        position: "absolute",
                        //top: (pos.top + height + 20) + "px",
                        //left: (pos.left - widthdiv + 22) + "px",
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
        $(".RetailerSelectiontext").text("You can select from 2 to 5 Retailers");
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
        $(".RetailerSelectiontext").text("You can select from 2 to 5 Retailers");
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
    }
}
//GetIShopFilters();
function clearOutScr() {
    $("#ToShowChart").hide();
    $("#chart-title").hide();
    $(".showChartMain").hide();
    $("#spChartLegend").hide();
    $(".ChartDivArea").hide();
    $("#LinkForCharts").hide();
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
            //E-Com search filters
            if (SearchFor == "Frequency" || SearchFor == "Monthly") {
                searchObj = $("#shopper_frequency_containerId li[uniqueId='" + obj.UniqueId + "']");
                SelectFrequency(searchObj);
            }
            else if (SearchFor == "AllBevFrequency") {
                searchObj = $("#beverage-frequency ul li[uniqueId='" + obj.UniqueId + "']");
                SelectBeverageSelectionType(searchObj);
            }
            else if (SearchFor == "Trips-Frequency") {
                searchObj = $("#frequency_containerId_trips li[uniqueId='" + obj.UniqueId + "']");
                SelectTripsFrequency(searchObj);
            }
            else if (SearchFor == "ordertype" || SearchFor == "Monthly") {
                if (SearchFor == "ordertype")
                    searchObj = $("#visit_frequency_containerId li[uniqueid='" + obj.UniqueId + "']");
                else
                    var searchObj = $(".rgt-cntrl-ordertype-Conatiner * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectFrequency(this);']");

                SelectFrequency(searchObj);
                if (SearchFor == "ordertype") {
                    var offset = $(".adv-fltr-ordertype-container").offset();
                    var height = $(".adv-fltr-ordertype-container").height();
                    var width = $(".adv-fltr-ordertype-container").innerWidth();
                    var top = offset.top + height;
                    var offset1 = $(".adv-filters-wraper").offset();
                    var height_wraper = $(".adv-filters-wraper").height();

                    //var right = offset.left + width + 2 + "px";

                    $('.rgt-cntrl-ordertype').css({
                        'position': 'absolute',
                        'left': offset.left - offset1.left,
                        'top': (height_wraper + 1),//($(this).height() + 1),// $(this).offset().top +
                    });
                    $(".rgt-cntrl-ordertype").css("display", "block");
                    $(".rgt-cntrl-ordertype").show();
                }
                //else
                //    DisplaySecondaryFrequency(ObjData);
            }
            else if (SearchFor == "AdvancedFilters") {
                searchObj = $("#VisistsFilterDivId ul li[uniqueid='" + obj.UniqueId + "']");
                SelectAdvfilters(searchObj);
            }
            else if (SearchFor == "Channel") {
                var ObjData = $("#rgt-cntrl-chnl-SubFilter1  div[name=\"" + ui.item.label + "\"]").parent();
                if (ObjData.length <= 0)
                    ObjData = $("#rgt-cntrl-chnl-SubFilter2 div[name=\"" + ui.item.label + "\"]").parent();
                if (ObjData.length <= 0) {
                    ObjData = $("#rgt-cntrl-chnl-SubFilter3 .DemographicList[name=\"" + ui.item.label + "\"]").parent();
                    if (ObjData.length <= 0)
                        ObjData = $("#rgt-cntrl-chnl-SubFilter3 .DemographicList span[name=\"" + ui.item.label + "\"]").parent();
                }
                SelectChannel(ObjData);
            }
            else if (SearchFor == "Left-Panel-Frequency") {
                $("#left-panel-frequency li div[name='" + ui.item.label + "']").trigger("click");
            }
                //End E-Com search filters
            else if (SearchFor == "Retailer") {
                searchObj = $("#RetailerDivId li[uniqueid='" + obj.UniqueId + "']");
                SelectComparison(searchObj);
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

                searchObj = [];
                if ($(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text() != "Other") {
                    searchObj = $("#rgt-cntrl-SubFilter1 span[uniqueId='" + obj.UniqueId + "']").parent();
                    if (searchObj.length <= 0)
                        searchObj = $("#rgt-cntrl-SubFilter1 span[uniqueId='" + obj.UniqueId + "']").parent();
                }

                if (searchObj.length <= 0)
                    searchObj = $("#rgt-cntrl-SubFilter2 span[uniqueId='" + obj.UniqueId + "']").parent();
                if (searchObj.length <= 0)
                    searchObj = $("#rgt-cntrl-SubFilter3 span[uniqueId='" + obj.UniqueId + "']").parent();
                if (searchObj.length <= 0)
                    searchObj = $("#VisistsFilterDivId span[uniqueId='" + obj.UniqueId + "']").parent();

                SelectAdvfilters(searchObj);
            }
            else if (SearchFor == "Type") {
                //searchObj = $("#groupDivId ul li span[uniqueid='" + obj.UniqueId + "']").parent();
                searchObj = $("#groupDivId ul li[parentname='" + obj.ParentName + "'][uniqueid='" + obj.UniqueId + "']");
                if (obj.ParentName == "FREQUENCY") {
                    SelectFrequency(searchObj);
                    return;
                }
                else {
                    SelecGroupMetricName(searchObj);
                }
                //SelecGroupMetricName(searchObj);
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

                SelectFrequency(searchObj);
            }
            else if (SearchFor == "Sites") {
                $("#e-com-sites").val(ui.item.label);
                var SiteObj = $("#e-com-sites li[uniqueid='" + obj.UniqueId + "']");
                SelectSite(SiteObj);
            }
            e.stopImmediatePropagation();
            this.value = "";
            return false;
        }
    });
}