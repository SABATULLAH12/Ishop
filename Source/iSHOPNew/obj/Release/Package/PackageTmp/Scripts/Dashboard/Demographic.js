/// <reference path="../Layout/Common.js" />
/// <reference path="../Layout/Layout.js" />

var AllPieChartData = [];
var MetricValue = [];
var MetricData = [];
var metricTypeData = [];
var sdata = [];
var time_pie_chart_flag = 0, timePieData = 0;
var active_metric_name = "";
var retailer_samplesize = 0;
var SaveFlag = 0;
var Selected_Filters = [];
var dynamicChanges = [{ "name": "WEEKDAY OR WEEKEND", "value": "none" },
                      { "name": "TIME OF DAY", "value": "none" },
                      { "name": "LOCATION PRIOR TO VISIT", "value": "none" },
                      { "name": "PLANNING", "value": "none" },
                      { "name": "Preparation Types", "value": "none" },
                      { "name": "WHO WITH", "value": "none" },
                      { "name": "CONSIDERATION", "value": "none" },
                      { "name": "REASON FOR STORE CHOICE", "value": "none" },
                      { "name": "DESTINATION ITEM", "value": "none" },
                      { "name": "TRIP MISSION", "value": "none" },
                      { "name": "ORDER SUMMARY", "value": "none" },
                      { "name": "NUMBER OF ITEMS", "value": "none" },
                      { "name": "DOLLARS SPENT", "value": "none" },
                      { "name": "PURCHASED NARTD", "value": "none" },
                      { "name": "IMMEDIATE CONSUMPTION", "value": "none" },
                      { "name": "TIME SPENT", "value": "none" },
                      { "name": "OVERALL SATISFACTION", "value": "none" },
                      { "name": "SATISFACTION DRIVERS", "value": "none" },
                      { "name": "LOCATION AFTER VISIT", "value": "none" },
];
var channel_color_code = [{ "name": "Grocery", "color": "#fbcf51" },
                      { "name": "Conv.", "color": "#01a7d9" },
                      { "name": "Super Centers", "color": "#ff5212" },
                      { "name": "Mass Merch", "color": "#a8a8a5" },
                      { "name": "Drug", "color": "#bb2c2b" },
                      { "name": "Dollar", "color": "#658781" },
                      { "name": "Club", "color": "#72984B" }];
$(document).ready(function () {
    $("#GroupType .FilteriCon div").removeClass("grouptype_img").addClass("demograhicFitr_img").css("background-position", "-300px -159px");
    prepareContentAreaAfterLoad();

    $("#GroupType").on("click", function () {
        hideShowVisitShopperFilters()
    })
    hideShowVisitShopperFilters();
    $(".custombase-groupDivId div[level-id='2'] ul li[name='GEOGRAPHY']").trigger('click')
});

function hideShowVisitShopperFilters() {
    if (currentpage == "hdn-dashboard-demographic") {
        if (sVisitsOrGuests == 1) {
            $("#groupDivId ul li[filtertype='Shopper']").css("background-color", "rgb(128, 128, 128)");
            $("#groupDivId ul li[filtertype='Visits']").css("background-color", "");
            $("#groupDivId ul li[filtertype='Shopper']").hide();
            $("#groupDivId ul li[filtertype='Visits']").show();

            $("#custombase-groupDivId ul li[filtertype='Shopper']").css("background-color", "rgb(128, 128, 128)");
            $("#custombase-groupDivId ul li[filtertype='Visits']").css("background-color", "");
            $("#custombase-groupDivId ul li[filtertype='Shopper']").hide();
            $("#custombase-groupDivId ul li[filtertype='Visits']").show();

        }
        else {
            $("#groupDivId ul li[filtertype='Visits']").css("background-color", "rgb(128, 128, 128)");
            $("#groupDivId ul li[filtertype='Shopper']").css("background-color", "");
            $("#groupDivId ul li[filtertype='Visits']").hide();
            $("#groupDivId ul li[filtertype='Shopper']").show();

            $("#custombase-groupDivId ul li[filtertype='Visits']").css("background-color", "rgb(128, 128, 128)");
            $("#custombase-groupDivId ul li[filtertype='Shopper']").css("background-color", "");
            $("#custombase-groupDivId ul li[filtertype='Visits']").hide();
            $("#custombase-groupDivId ul li[filtertype='Shopper']").show();
        }
    }
}

function prepareContentAreaAfterLoad() {
    //To Load svg charts
    $(".male-female-chart").html('<object class="donutChartGender" data="../Images/Demog/P2P Dashboard elements_Gender_PiChart_Bg.svg?v=23" type="image/svg+xml"></object>');
    $(".age-middle-chart").html('<object class="age-pie-chart" data="../Images/Demog/P2P Dashboard elements_Age_PiChart_Bg.svg?v=23" type="image/svg+xml"> </object>');
    $(".occ-container[pos='1'] .occ-bar-chart").html('<object class="bar-chart" data="../Images/Demog/P2P Dashboard elements_Occupation BarChart Bg.svg?v=23" type="image/svg+xml"></object>');
    $(".occ-container[pos='2'] .occ-bar-chart").html('<object class="bar-chart" data="../Images/Demog/P2P Dashboard elements_Occupation BarChart Bg.svg?v=23" type="image/svg+xml"></object>');
    $(".occ-container[pos='3'] .occ-bar-chart").html('<object class="bar-chart" data="../Images/Demog/P2P Dashboard elements_Occupation BarChart Bg.svg?v=23" type="image/svg+xml"></object>');
    $(".occ-container[pos='4'] .occ-bar-chart").html('<object class="bar-chart" data="../Images/Demog/P2P Dashboard elements_Occupation BarChart Bg.svg?v=23" type="image/svg+xml"></object>');
    $(".occ-container[pos='5'] .occ-bar-chart").html('<object class="bar-chart" data="../Images/Demog/P2P Dashboard elements_Occupation BarChart Bg.svg?v=23" type="image/svg+xml"></object>');
    $(".occ-container[pos='6'] .occ-bar-chart").html('<object class="bar-chart" data="../Images/Demog/P2P Dashboard elements_Occupation BarChart Bg.svg?v=23" type="image/svg+xml"></object>');
    $(".dhs-container[pos='2'] .dhs-image-container").html('<object class="dhs-image one" data="../Images/Demog/P2P Dashboard elements_1Person HHS BarChart Bg_white.svg?v=23#svgView(preserveAspectRatio(xMidYMax meet))" type="image/svg+xml"> </object>');
    $(".dhs-container[pos='3'] .dhs-image-container").html('<object class="dhs-image one" data="../Images/Demog/P2P Dashboard elements_2Person HHS BarChart Bg_white.svg?v=23#svgView(preserveAspectRatio(xMidYMax meet))" type="image/svg+xml"> </object>');
    $(".dhs-container[pos='4'] .dhs-image-container").html('<object class="dhs-image one" data="../Images/Demog/P2P Dashboard elements_3Person HHS BarChart Bg_white.svg?v=23#svgView(preserveAspectRatio(xMidYMax meet))" type="image/svg+xml"> </object>');
    $(".ms-container[pos='1'] .dhs-image-container").html('<object class="dhs-image one heightforMartl" data="../Images/Demog/P2P Dashboard elements_1Person HHS BarChart Bg_white.svg?v=23#svgView(preserveAspectRatio(xMidYMax meet))" type="image/svg+xml"> </object>');
    $(".ms-container[pos='2'] .dhs-image-container").html('<object class="dhs-image one heightforMartl" data="../Images/Demog/P2P Dashboard elements_2Person HHS BarChart Bg_white.svg?v=23#svgView(preserveAspectRatio(xMidYMax meet))" type="image/svg+xml"> </object>');
    $(".ps-container .dhs-image-container").html('<object class="dhs-image one heightforMartl" data="../Images/Demog/P2P Dashboard elements_3Person HHS BarChart Bg_white.svg?v=23#svgView(preserveAspectRatio(xMidYMax meet))" type="image/svg+xml"> </object>');
    $(".da-container[pos='1'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='2'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='3'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='4'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='5'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='6'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='7'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='8'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='9'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".da-container[pos='10'] .da-chart").html('<object class="da-bar-chart" data="../Images/Demog/P2P Dashboard elements_Diner Attitudes BarChart Bg.svg" type="image/svg+xml"> </object>');
    $(".dashboard-content").show();
}

function prepareContentArea() {
    $("#custombase-stattesting-popup").hide();
    if (!Validate_CompareRetailers_Charts()) {
        return false;
    }
    if ($(".Custombase-Retailers").is(":visible") == true) {
        $("#Translucent").hide();
        $(".Custombase-Retailers").hide();
        CustomBaseFlag == 0;
    }
    else if (CustomBaseFlag == 1) {
        //SetDefaultStatTest($("#stattest_benchmark .stattest"));
        $("#stattest_benchmark .stattest").trigger("click");
        return;
    }
    else {
        $("#Translucent").hide();
        $(".Custombase-Retailers").hide();
    }
    //$(".save-reportPopup popup_css").hide();
    //$('.establishmt_img').css("background-image", 'url("../Images/P2PDashboardEsthmtImages/' + replace_file_special_characters(Comparisonlist[0].Name) + '.svg?26")');//replace("/","")
    var demogDashboardData = new Object();
    if (SelectedFrequencyList.length > 0) {
        demogDashboardData.ShopperFrequency = SelectedFrequencyList[0].Name;
        demogDashboardData.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }


    demogDashboardData.StatTest = Selected_StatTest;
    demogDashboardData.Sigtype_UniqueId = Sigtype_Id;
    demogDashboardData.TimePeriod = TimePeriod;
    demogDashboardData.TimePeriod_UniqueId = TimePeriod_UniqueId;
    demogDashboardData.TimePeriodShortName = $(".timeType").val();

    if (CustomBase.length > 0) {
        demogDashboardData.CustomBase_ShortName = CustomBase[0].Name;
        demogDashboardData.CustomBase_UniqueId = CustomBase[0].UniqueId;
    }

    if (Comparisonlist.length > 0) {
        demogDashboardData.Comparison_ShortNames = Comparisonlist[0].Name;
        demogDashboardData.Comparison_UniqueIds = Comparisonlist[0].UniqueId;
    }

    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_UniqueId = [];
    for (var i = 0; i < Grouplist.length; i++) {
        Advanced_Filters_DBNames.push(Grouplist[i].DBName);
        Advanced_Filters_ShortNames.push(Grouplist[i].Name);
        Advanced_Filters_UniqueId.push(Grouplist[i].UniqueId);
    }
    demogDashboardData.ShopperSegment = Advanced_Filters_DBNames.join("|");
    demogDashboardData.FilterShortname = Advanced_Filters_ShortNames.join("|");
    demogDashboardData.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");

    //add custom base dual filters   
    CustomBaseAdvancedFilters = [];
    CustomBaseAdvancedFilters_UniqueId = [];
    for (var i = 0; i < custombase_AddFilters.length; i++) {
        CustomBaseAdvancedFilters.push(custombase_AddFilters[i].Name);
        CustomBaseAdvancedFilters_UniqueId.push(custombase_AddFilters[i].UniqueId);
    }
    demogDashboardData.CustomBaseAdvancedFilters = CustomBaseAdvancedFilters.join("|");
    demogDashboardData.CustomBaseAdvancedFilters_UniqueId = CustomBaseAdvancedFilters_UniqueId.join("|");

    //add custom base frequency dual filters   
    CustomBaseFrequencyFilters = [];
    CustomBaseFrequency_UniqueId = [];
    for (var i = 0; i < custombase_Frequency.length; i++) {
        CustomBaseFrequencyFilters.push(custombase_Frequency[i].Name);
        CustomBaseFrequency_UniqueId.push(custombase_Frequency[i].UniqueId);
    }
    demogDashboardData.CustomBaseShopperFrequency = CustomBaseFrequencyFilters.join("|");
    demogDashboardData.CustomBaseShopperFrequency_UniqueId = CustomBaseFrequency_UniqueId.join("|");

    demogDashboardData.Sort = P2P_Sort;
    var custompopUpText = "";
    if (Selected_StatTest.toUpperCase() == "CUSTOM BASE") {
        custompopUpText = CustomBase[0].Name;
    }
    else {
        custompopUpText = Selected_StatTest;
    }
    if (TabType.toLowerCase() == "trips") {
        demogDashboardData.TabType = "1";
    }
    else {
        demogDashboardData.TabType = "2";
    }

    //competitor Params
    if (CompetitorRetailer.length > 0) {
        demogDashboardData.CompetitorRetailer_Name = CompetitorRetailer[0].Name;
        demogDashboardData.CompetitorRetailer_UniqueId = CompetitorRetailer[0].UniqueId;

        demogDashboardData.CompetitorFrequency_Name = CompetitorFrequency[0].Name;
        demogDashboardData.CompetitorFrequency_UniqueId = CompetitorFrequency[0].UniqueId;
    }
    if (CompetitorCustomBaseRetailer.length > 0) {
        demogDashboardData.CustomBaseCompetitorRetailer_Name = CompetitorCustomBaseRetailer[0].Name;
        demogDashboardData.CustomBaseCompetitorRetailer_UniqueId = CompetitorCustomBaseRetailer[0].UniqueId;

        demogDashboardData.CustomBaseCompetitorFrequency_Name = CompetitorCustomBaseFrequency[0].Name;
        demogDashboardData.CustomBaseCompetitorFrequency_UniqueId = CompetitorCustomBaseFrequency[0].UniqueId;
    }
    demogDashboardData.IsOnlineSelected = false;
    demogDashboardData.IsOnlineSelectedAsBase = false;

    if (SelectedFrequencyList.length>0 && SelectedFrequencyList[0].Name != undefined && SelectedFrequencyList[0].Name.toLowerCase() == 'online') {
        demogDashboardData.IsOnlineSelected = true;
    }
    if (custombase_Frequency.length > 0 && custombase_Frequency[0].Name != undefined && custombase_Frequency[0].Name.toLowerCase() == 'online') {
        demogDashboardData.IsOnlineSelectedAsBase = true;
    }
    postBackData = "{demogDashboardData:" + JSON.stringify(demogDashboardData) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLDemographicData").val(),
        data: postBackData,
        contentType: "application/json",
        success: function (data) {
            $(".dashboard-content").show();
            ShowLoader();
            if (!isAuthenticated(data))
                return false;
            if (data != null) {
                //add sample size
                $(".P2PSampleSize").html("Sample Size: " + data.SampleSize);
                retailer_samplesize = parseFloat(data.SampleSize.replace(",", ""));
                var stattest_samplesize = parseFloat(data.StatTestSampleSize.replace(",", ""));
                $(".ind-item-retailer, .ind-item-stattest").hide();
                $(".ind-item-retailer").html("RETAILER: " + Comparisonlist[0].Name);
                $(".ind-item-stattest").html("STAT TESTING BASE: " + custompopUpText);
                if (retailer_samplesize < 30 || stattest_samplesize < 30) {
                    $(".heading_text").html("Sample Size for the following selection is less than 30, Kindly change your selection.");
                    if (retailer_samplesize < 30 && stattest_samplesize < 30) {
                        $(".ind-item-retailer, .ind-item-stattest").show();
                    }
                    else if (retailer_samplesize < 30) {
                        $(".ind-item-retailer").show();
                    }
                    else if (stattest_samplesize < 30) {
                        $(".ind-item-stattest").show();
                    }
                    $(".dashboard-content").css("visibility", "hidden");
                    $("#dashboard-popup .proceedClick").hide();
                    $("#dashboard-popup .closeSavePopup").show();
                    $("#dashboard-popup").show();
                    return false;
                }
                else if (retailer_samplesize < 100 || stattest_samplesize < 100) {
                    $(".heading_text").html("Sample Size is between 30 and 99. Use directionally.");
                    $(".dashboard-content").css("visibility", "hidden");
                    $("list-of-low-SS").hide();
                    $(".ind-item-retailer, .ind-item-stattest").show();
                    $("#dashboard-popup .proceedClick").show();
                    $("#dashboard-popup .closeSavePopup").hide();
                    $("#dashboard-popup").show();
                }
                else {
                    $("#dashboard-popup").hide();
                    $(".dashboard-content").css("visibility", "visible");
                }
                MetricData = data.pathToPurchaseMetricEntitylist;
                update_metric_data(data.pathToPurchaseMetricEntitylist);
                sdata = data;
                dynamicChanges = _.forEach(dynamicChanges, function (ele) { ele.value = "none" });
                //$(".dashboard-content").show();
                $('.est-logo-container .est-logo').css("background-image", 'url("../Images/P2PDashboardEsthmtImages/' + replace_file_special_characters(Comparisonlist[0].Name) + '.svg?26")');
            }
            HideLoader();
            if (SaveFlag == 1) {
                setTimeout(function () {
                    $('#Translucent').css("z-index", "6001");
                    $('.TranslucentDiv').show()
                }, 50);
                showMessage("Saved Successfully");
            }
            SaveFlag = 0;
        },
        error: function (xhr, status, error) {
            //showMessage(xhr.responseText)
            GoToErrorPage();
        }
    });
}

function update_metric_data(data) {
    if (data != null && data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var metrictype = data[i].MetricType.toLowerCase();
            switch (metrictype) {
                case "gender":
                    {
                        fillGenderData(data[i]);
                        break;
                    }
                case "age":
                    {
                        fillAgeData(data[i]);
                        break;
                    }
                case "ethnicity":
                    {
                        fillEthnicity(data[i]);
                        break;
                    }
                case "density":
                    {
                        filldensity(data[i]);
                        break;
                    }
                case "hh size - total":
                    {
                        fillSHS(data[i]);
                        break;
                    }
                case "marital status":
                    {
                        fillMaritalStatusData(data[i]);
                        break;
                    }
                case "parental identification":
                    {
                        fillParentalStatusData(data[i]);
                        break;
                    }
                case "hh income":
                    {
                        fillHouseholdIncomeData(data[i]);
                        break;
                    }
                case "socio economic":
                    {
                        fillSocioEconomicData(data[i]);
                        break;
                    }
                case "attitudinal statements - top 2 box":
                    {
                        fillShopperSegmentData(data[i]);
                        break;
                    }
                case "average monthy channel visit":
                    {
                        fillAvgMonthlyChannelData(data[i]);
                        break;
                    }

            }
        }
    }
}

function fillGenderData(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 2 || data == []) { return; }
    $(".gender-section").removeClass("active-widget");
    for (i = 0; i < 2; i++) {
        //Fill age Metric texts
        $(".gender-text .metric-text:eq(" + i + ") .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".gender-text .metric-values:eq(" + i + ") .mValDemog").text(returnFormattedValues(data.MetricData[i].Volume) + "% |");
        //Fill age change
        $(".gender-text .metric-values:eq(" + i + ") .cValDemog").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".gender-text .metric-values:eq(" + i + ") .cValDemog").removeClass("plus minus");
        $(".gender-text .metric-values:eq(" + i + ") .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".gender-text .metric-values:eq(" + i + ") .cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".gender-section:eq(" + i + ")").addClass("active-widget"); }
    }
    plotGenderChart(data);
}

function fillAgeData(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 6 || data == []) { return; }
    $(".age-ind-section").removeClass("active-widget");
    for (i = 0; i < 6; i++) {
        //Fill age Metric texts
        $(".age-ind-section[pos=" + (i + 1) + "] .age-text .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".age-ind-section[pos=" + (i + 1) + "] .age-values .mValDemog").text(returnFormattedValues(data.MetricData[i].Volume) + "% |");
        //Fill age change
        $(".age-ind-section[pos=" + (i + 1) + "] .age-values .cValDemog").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".age-ind-section[pos=" + (i + 1) + "] .age-values .cValDemog").removeClass("plus minus");
        $(".age-ind-section[pos=" + (i + 1) + "] .age-values .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".age-ind-section[pos=" + (i + 1) + "] .age-values .cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".age-ind-section[pos=" + (i + 1) + "]").addClass("active-widget"); }
    }
    //Plot the Age Chart
    plotAgeChart(data);
}

function fillEthnicity(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 5 || data == []) { return; }
    $(".eth-sub-container").removeClass("active-widget");
    for (i = 0; i < 5; i++) {
        //Fill age Metric texts
        $(".eth-1:eq(" + i + ") .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".eth-2:eq(" + i + ").mValDemog .vertical-align").text(returnFormattedValues(data.MetricData[i].Volume) + "%");
        //Fill age change
        $(".eth-3:eq(" + i + ").cValDemog .vertical-align").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".eth-3:eq(" + i + ").cValDemog").removeClass("plus minus");
        $(".eth-3:eq(" + i + ").cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".eth-3:eq(" + i + ").cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".eth-sub-container:eq(" + i + ")").addClass("active-widget"); }
    }
}

function filldensity(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 4 || data == []) { return; }
    $(".den-container").removeClass("active-widget");
    for (i = 0; i < 4; i++) {
        //Fill age Metric texts
        $(".den-text:eq(" + i + ") .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".den-values:eq(" + i + ") .mValDemog .vertical-align").text(data.MetricData[i].Volume < 0 ? "NA" : returnFormattedValues(data.MetricData[i].Volume) + "%");
        //Fill age change
        $(".den-values:eq(" + i + ") .cValDemog .vertical-align").text(data.MetricData[i].Volume < 0 ? "NA" : SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".den-values:eq(" + i + ") .cValDemog .vertical-align").removeClass("plus minus");
        $(".den-values:eq(" + i + ") .cValDemog .vertical-align").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".den-values:eq(" + i + ") .cValDemog .vertical-align").addClass(data.MetricData[i].Volume < 0 ? "black" : SetSignificanceColor(data.MetricData[i].Significance));
        //Fill line chart
        if ($(".den-container:eq(" + i + ") .bar-chart")[0].getSVGDocument() != null && $(".den-container:eq(" + i + ") .bar-chart")[0].getSVGDocument().querySelector(".line-fill") != null) {
            fillLineChart($(".den-container:eq(" + i + ") .bar-chart")[0].getSVGDocument().querySelector(".line-fill"), data.MetricData[i].Volume);
        }
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".den-container:eq(" + i + ")").addClass("active-widget"); }
    }
}

function fillSHS(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 4 || data == []) { return; }
    $(".dhs-container").removeClass("active-widget");
    //For Avg HH
    $(".dhs-container:eq(0) .tval .vertical-align").text(data.MetricData[0].Metric);
    $(".dhs-container:eq(0) .mValDemog .vertical-align").text(returnFormattedValues(data.MetricData[0].Volume, 1));
    $(".dhs-container:eq(0) .cValDemog .vertical-align").text(SetChangeVolume(data.MetricData[0].ChangeVolume));
    $(".dhs-container:eq(0) .cValDemog .vertical-align").removeClass("plus minus");
    $(".dhs-container:eq(0) .cValDemog .vertical-align").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
    $(".dhs-container:eq(0) .cValDemog .vertical-align").addClass(SetSignificanceColor(data.MetricData[0].Significance));
    for (i = 1; i < 4; i++) {
        //Fill age Metric texts
        $(".dhs-container:eq(" + i + ") .dhs-text .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".dhs-container:eq(" + i + ") .dhs-mVal").text(returnFormattedValues(data.MetricData[i].Volume) + "% |");
        //Fill age change
        $(".dhs-container:eq(" + i + ") .dhs-cVal").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".dhs-container:eq(" + i + ") .dhs-cVal").removeClass("plus minus");
        $(".dhs-container:eq(" + i + ") .dhs-cVal").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".dhs-container:eq(" + i + ") .dhs-cVal").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".dhs-container:eq(" + i + ")").addClass("active-widget"); }
        fillSHSsvg($(".dhs-container:eq(" + i + ") .dhs-image"), data.MetricData[i]);
    }
}

function fillMaritalStatusData(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 2 || data == []) { return; }
    $(".ms-container").removeClass("active-widget");
    for (i = 0; i < 2; i++) {
        //Fill age Metric texts
        $(".ms-container:eq(" + i + ") .textDemog .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".ms-container:eq(" + i + ") .mValDemog").text(returnFormattedValues(data.MetricData[i].Volume) + "% |");
        //Fill age change
        $(".ms-container:eq(" + i + ") .cValDemog").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".ms-container:eq(" + i + ") .cValDemog").removeClass("plus minus");
        $(".ms-container:eq(" + i + ") .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".ms-container:eq(" + i + ") .cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".ms-container:eq(" + i + ")").addClass("active-widget"); }
        fillMSsvg($(".ms-container:eq(" + i + ") .dhs-image"), data.MetricData[i], true);
    }
}

function fillParentalStatusData(data) {
    if (data == null || data.MetricData.length != 1 || data == []) { return; }
    //Fill age Metric texts
    $(".ps-container .textDemog .vertical-align").text(data.MetricData[0].Metric);
    //Fill age metricValues
    $(".ps-container .mValDemog").text(returnFormattedValues(data.MetricData[0].Volume) + "% |");
    //Fill age change
    $(".ps-container .cValDemog").text(SetChangeVolume(data.MetricData[0].ChangeVolume));
    $(".ps-container .cValDemog").removeClass("plus minus");
    $(".ps-container .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
    $(".ps-container .cValDemog").addClass(SetSignificanceColor(data.MetricData[0].Significance));
    fillMSsvg($(".ps-container .dhs-image"), data.MetricData[0], false);
}

var fillHouseholdIncomeData = function (data) {
    let i = 0;
    if (data == null || data.MetricData.length != 3 || data == []) { return; }
    $(".hi-container").removeClass("active-widget");
    //For Avg
    $(".hi-container:eq(0) .hi-tVal .vertical-align").text(data.MetricData[i].Metric);
    $(".hi-container:eq(0) .mValDemog .vertical-align").text("$" + addCommas(returnFormattedValues(data.MetricData[i].Volume)) + "K");
    $(".hi-container:eq(0) .cValDemog .vertical-align").text(SetChangeVolume(data.MetricData[i].ChangeVolume) + "K");
    $(".hi-container:eq(0) .cValDemog").removeClass("plus minus");
    $(".hi-container:eq(0) .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
    $(".hi-container:eq(0) .cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
    for (i = 1; i < 3; i++) {
        //Fill age Metric texts
        $(".hi-container:eq(" + i + ") .hi-text .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".hi-container:eq(" + i + ") .mValDemog").text(returnFormattedValues(data.MetricData[i].Volume) + "% |");
        //Fill age change
        $(".hi-container:eq(" + i + ") .cValDemog").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".hi-container:eq(" + i + ") .cValDemog").removeClass("plus minus");
        $(".hi-container:eq(" + i + ") .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".hi-container:eq(" + i + ") .cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".hi-container:eq(" + i + ")").addClass("active-widget"); }
    }
}

var fillSocioEconomicData = function (data) {
    let i = 0;
    if (data == null || data.MetricData.length != 4 || data == []) { return; }
    $(".se-container").removeClass("active-widget");
    for (i = 0; i < 4; i++) {
        //Fill age Metric texts
        $(".se-container:eq(" + i + ") .se-text .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".se-container:eq(" + i + ") .mValDemog").text(returnFormattedValues(data.MetricData[i].Volume) + "% |");
        //Fill age change
        $(".se-container:eq(" + i + ") .cValDemog").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".se-container:eq(" + i + ") .cValDemog").removeClass("plus minus");
        $(".se-container:eq(" + i + ") .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".se-container:eq(" + i + ") .cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".se-container:eq(" + i + ")").addClass("active-widget"); }
    }
}

function fillShopperSegmentData(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 10 || data == []) { return; }
    $(".ss-container").removeClass("active-widget");
    for (i = 0; i < 10; i++) {
        //Fill age Metric texts
        $(".ss-container:eq(" + i + ") .ss-text .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues
        $(".ss-container:eq(" + i + ") .mValDemog .vertical-align").text(returnFormattedValues(data.MetricData[i].Volume) + "%");
        //Fill age change
        $(".ss-container:eq(" + i + ") .cValDemog .vertical-align").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".ss-container:eq(" + i + ") .cValDemog .vertical-align").removeClass("plus minus");
        $(".ss-container:eq(" + i + ") .cValDemog .vertical-align").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".ss-container:eq(" + i + ") .cValDemog .vertical-align").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Fill line chart
        if ($(".ss-container:eq(" + i + ") .ss-bar-chart")[0].getSVGDocument() != null && $(".ss-container:eq(" + i + ") .ss-bar-chart")[0].getSVGDocument().querySelector(".line-fill") != null) {
            fillLineChart($(".ss-container:eq(" + i + ") .ss-bar-chart")[0].getSVGDocument().querySelector(".line-fill"), data.MetricData[i].Volume);
        }
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".ss-container:eq(" + i + ")").addClass("active-widget"); }
    }
}

function fillAvgMonthlyChannelData(data) {
    let i = 0;
    if (data == null || data.MetricData.length != 7 || data == []) { return; }
    $(".amc-values").removeClass("active-widget");
    for (i = 0; i < 7; i++) {
        //Fill age Metric texts
        var channelColor = "";
        _.forEach(channel_color_code, function (ele) {
            if (ele.name.toLowerCase() == data.MetricData[i].Metric.toLowerCase()) {
                channelColor = ele.color;
            }
        })
        $(".amc-block:eq(" + i + ") .amc-text").css("background-color", channelColor);
        $(".amc-block:eq(" + i + ") .amc-text .vertical-align").text(data.MetricData[i].Metric);
        //Fill age metricValues 
        $(".amc-block:eq(" + i + ") .mValDemog .vertical-align").text(parseFloat(data.MetricData[i].Volume).toFixed(1));
        //Fill age change
        $(".amc-block:eq(" + i + ") .cValDemog .vertical-align").text(SetChangeVolume(data.MetricData[i].ChangeVolume));
        $(".amc-block:eq(" + i + ") .cValDemog").removeClass("plus minus red gray");
        $(".amc-block:eq(" + i + ") .cValDemog").removeClass("red").removeClass("black").removeClass("green").removeClass("gray");
        $(".amc-block:eq(" + i + ") .cValDemog").addClass(SetSignificanceColor(data.MetricData[i].Significance));
        //Set active-widget
        if (data.MetricData[i].Flag == 1) { $(".amc-block:eq(" + i + ") .amc-values").addClass("active-widget"); }
    }
}

function replace_file_special_characters(filename) {
    if (filename != null && filename != '' && filename != undefined)
        filename = filename.replace(/[&/\\#,+()$~%.':*?<>{}]/g, '-');

    return filename;
}

function plotGenderChart(data) {
    if ($(".donutChartGender")[0].getSVGDocument() == null || $(".donutChartGender")[0].getSVGDocument().querySelector("#drawArcsHere") == null) { return; }
    var startangle = 0, midAngle = (2 * Math.PI) * data.MetricData[1].Volume / 100, endAngle = 2 * Math.PI;
    //Clear chart
    $($(".donutChartGender")[0].getSVGDocument().querySelector("#drawArcsHere")).html('');
    var genderChart = $(".donutChartGender")[0].getSVGDocument().querySelector("#drawArcsHere");
    var arc1 = d3.svg.arc()
    .innerRadius(19.2)
    .outerRadius(35.2)
    .startAngle(startangle)
    .endAngle(midAngle);
    d3.select(genderChart).append("path")
        .attr("class", "femaleArc")
        .attr("d", arc1).style('fill', "#01A7D9").style("stroke", "#000000").style("stroke-width", "0.3px");
    var arc2 = d3.svg.arc()
    .innerRadius(19.2)
    .outerRadius(35.2)
    .startAngle(midAngle)
    .endAngle(endAngle);
    d3.select(genderChart).append("path")
        .attr("class", "maleArc")
        .attr("d", arc2).style('fill', "#BB2C2B").style("stroke", "#000000").style("stroke-width", "0.3px");
}

function plotAgeChart(data) {
    if (data.MetricData.length != 6) { return; }
    if ($(".age-pie-chart")[0].getSVGDocument() == null || $(".age-pie-chart")[0].getSVGDocument().querySelector(".age-chart") == null) { return; }
    var i = 0, startangle = 2 * Math.PI, prevAngle = 2 * Math.PI, endAngle = 0;
    //Clear chart
    $($(".age-pie-chart")[0].getSVGDocument().querySelector(".age-chart")).html('');
    var genderChart = $(".age-pie-chart")[0].getSVGDocument().querySelector(".age-chart");
    var colors = ["#FBCF51", "#01A7D9", "#FF5212", "#A8A8A5", "#BB2C2B", "#658781"];
    for (i = 0; i < data.MetricData.length; i++) {
        startangle = prevAngle;
        endAngle = prevAngle - ((2 * Math.PI) * data.MetricData[i].Volume / 100);
        var arc = d3.svg.arc()
            .innerRadius(34.4)
            .outerRadius(55.26)
            .startAngle(startangle)
            .endAngle(endAngle);
        d3.select(genderChart).append("path")
            .attr("class", "Arc" + i)
            .attr("d", arc).style('fill', colors[i]).style("stroke", "#000000").style("stroke-width", "0.5px");
        prevAngle = endAngle;
    }
}

function SetChangeVolume(ChangeVolume) {
    var _significance = 0;
    if (ChangeVolume != null && ChangeVolume != "") {
        _significance = parseFloat(ChangeVolume).toFixed(1);
        if (_significance == 0) {
            return parseFloat(_significance).toFixed(1);
        }
        else if (_significance > 0) {
            return "+" + parseFloat(_significance).toFixed(1);
        }
        else if (_significance < 0) {
            return parseFloat(_significance).toFixed(1);
        }
    }
    return parseFloat(_significance).toFixed(1);
}

function SetSignificanceColor(Significance) {
    var sigcolor = "black";
    var _significance = 0;
    if (Significance != null && Significance != "") {
        _significance = parseFloat(Significance);
        if (_significance > Stat_PositiveValue) {
            sigcolor = "green";
        }
        else if (_significance < Stat_NegativeValue) {
            sigcolor = "red";
        }
        else if (retailer_samplesize >= MinSampleSize && retailer_samplesize < MaxSampleSize) {
            sigcolor = "gray";
        }
    }
    return sigcolor;
}

function returnFormattedValues(val, fixedPlaces) {
    fixedPlaces = typeof fixedPlaces !== 'undefined' ? fixedPlaces : 0;
    if (val == null || val == undefined || isNaN(val)) { return 0; }
    return val.toFixed(fixedPlaces);
}

var fillLineChart = function (ele, val) {
    if (ele == undefined || ele == null) { return; }
    if ($(".den-container .bar-chart:eq(0)")[0].getSVGDocument() == null || $(".den-container .bar-chart:eq(0)")[0].getSVGDocument().querySelector(".line-fill") == null) { return; }
    var max_w = d3.select(ele).attr('max-width');
    d3.select(ele).attr('x2', max_w * (val / 100));
}

var fillSHSsvg = function (ele, data) {
    if ($(ele)[0].getSVGDocument() == null || $(ele)[0].getSVGDocument().querySelector(".fillPerson") == null) { return; }
    var DHSchart = $(ele)[0].getSVGDocument().querySelector(".fillPerson");
    var act_wdgt = $(ele)[0].getSVGDocument().querySelector(".cls-1");
    var max_height = d3.select(DHSchart).attr('max-height');
    var calc_h = max_height * data.Volume / 100;
    var calc_y = max_height - calc_h;
    d3.select(DHSchart).attr('y', calc_y).attr('height', calc_h);
    //Active-widget
    var fill_color = data.Flag == 1 ? "#dddddd" : "#fafafa";
    d3.select(act_wdgt).style("fill", fill_color);
}

var fillMSsvg = function (ele, data, flag) {
    if ($(ele)[0].getSVGDocument() == null || $(ele)[0].getSVGDocument().querySelector(".fillPerson") == null) { return; }
    var MSchart = $(ele)[0].getSVGDocument().querySelector(".fillPerson");
    var act_wdgt = $(ele)[0].getSVGDocument().querySelector(".cls-1");
    var max_height = d3.select(MSchart).attr('max-height');
    var calc_h = max_height * data.Volume / 100;
    var calc_y = max_height - calc_h;
    d3.select(MSchart).attr('y', calc_y).attr('height', calc_h);
    if (flag) {
        //Active-widget
        var fill_color = data.Flag == 1 ? "#dddddd" : "#fafafa";
        d3.select(act_wdgt).style("fill", fill_color);
    }
}

function addCommas(str) {
    var parts = (str + "").split("."),
        main = parts[0],
        len = main.length,
        output = "",
        first = main.charAt(0),
        i;

    if (first === '-') {
        main = main.slice(1);
        len = main.length;
    } else {
        first = "";
    }
    i = len - 1;
    while (i >= 0) {
        output = main.charAt(i) + output;
        if ((len - i) % 3 === 0 && i > 0) {
            output = "," + output;
        }
        --i;
    }
    // put sign back
    output = first + output;
    // put decimal part back
    if (parts.length > 1) {
        output += "." + parts[1];
    }
    return output;
}

//save dashboard selection
function SaveDashboardSelection() {
    
    if (!Validate_CompareRetailers_Charts()) {
        return false;
    }
    var demogDashboardData = new Object();
    if (SelectedFrequencyList.length > 0) {
        demogDashboardData.ShopperFrequency = SelectedFrequencyList[0].Name;
        demogDashboardData.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }

    demogDashboardData.StatTest = Selected_StatTest;
    demogDashboardData.Sigtype_UniqueId = Sigtype_Id;
    demogDashboardData.TimePeriod = TimePeriod;
    demogDashboardData.TimePeriod_UniqueId = TimePeriod_UniqueId;
    demogDashboardData.TimePeriodShortName = $(".timeType").val();

    if (CustomBase.length > 0) {
        demogDashboardData.CustomBase_ShortName = CustomBase[0].Name;
        demogDashboardData.CustomBase_UniqueId = CustomBase[0].UniqueId;
    }

    if (Comparisonlist.length > 0) {
        demogDashboardData.Comparison_ShortNames = Comparisonlist[0].Name;
        demogDashboardData.Comparison_UniqueIds = Comparisonlist[0].UniqueId;
    }

    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_UniqueId = [];
    for (var i = 0; i < SelectedDempgraphicList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].Name);
        Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
    }
    demogDashboardData.ShopperSegment = Advanced_Filters_DBNames.join("|");
    demogDashboardData.FilterShortname = Advanced_Filters_ShortNames.join("|");
    demogDashboardData.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");

    demogDashboardData.Sort = P2P_Sort;
    if (TabType.toLowerCase() == "trips") {
        demogDashboardData.TabType = "1";
    }
    else {
        demogDashboardData.TabType = "2";
    }
    postBackData = "{demogDashboardData:" + JSON.stringify(demogDashboardData) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLDashboardSaveUserSelectionDemo").val(),// + "/SaveUserSelection",
        data: postBackData,
        contentType: "application/json",
        success: function (data) {
            if (!isAuthenticated(data))
                return false;
            SaveFlag = 1;
            prepareContentArea();
        },
        error: function (xhr, status, error) {
            //showMessage(xhr.responseText)
            GoToErrorPage();
        }
    });
}

/**********PPT Download For Dashboard ************/

$(document).on("click", ".exporttoppt-logo", function () {
    if ($(".dashboard-content").is(":visible") == false) {
        return;
    }
    ShowLoader();
    var leftpanelData = Comparisonlist[0].Name + " | ";
    leftpanelData += getTimeperiodEx().toUpperCase();//$(".timeType").val();

    //if (SelectedDempgraphicList.length > 0) {
    //    leftpanelData += ", ";
    //    for (var i = 0; i < SelectedDempgraphicList.length; i++) {
    //        leftpanelData += SelectedDempgraphicList[i].Name + ", ";
    //    }
    //    leftpanelData = leftpanelData.slice(0, -2);
    //}
    Selected_Filters = [];
    if (Grouplist.length > 0) {
        leftpanelData += ", ";
        $(Grouplist).each(function (i, d) {
            leftpanelData = leftpanelData.concat(d.Name.trim() + ", ");
            Selected_Filters.push(d.Name);
        });
        leftpanelData = leftpanelData.slice(0, -2);
    }
    var statTest = Selected_StatTest;
    if (Selected_StatTest == "CUSTOM BASE") {
        statTest = CustomBase[0].Name;
    }

    //MetricData, "NoOfRoads": , "changedData": dynamicChanges, "LeftpanelData": leftpanelData, "statTest": statTest, "pptOrPdf": "ppt", "ss":  Number(sdata.SampleSize.replace(",",""))
    var demogDashboardData = new Object();
    demogDashboardData.TimePeriod = getTimeperiodEx().toUpperCase();
    demogDashboardData.Base = Comparisonlist[0].Name;
    demogDashboardData.CustomBase = (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
    demogDashboardData.Filters = Selected_Filters.join(", ");

    demogDashboardData.OutputData = MetricData;
    demogDashboardData.LeftpanelData = leftpanelData;
    demogDashboardData.statTest = statTest;
    demogDashboardData.pptOrPdf = "ppt";
    demogDashboardData.ss = Number(sdata.SampleSize.replace(",", ""));

    if (SelectedFrequencyList.length > 0) {
        demogDashboardData.ShopperFrequency = SelectedFrequencyList[0].Name;
        demogDashboardData.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }
    var P2P_SortText = "";
    if (P2P_Sort == "1") {
        P2P_SortText = "Size"
    }
    else {
        P2P_SortText = "Skew"
    }
    demogDashboardData.Sort = P2P_SortText;
    if (TabType.toLowerCase() == "trips") {
        demogDashboardData.TabType = "Trips";
    }
    else {
        demogDashboardData.TabType = "Shopper";
    }


    jQuery.ajax({
        type: "POST",
        url: $("#URLDemogDashboardFullExp").val(),// + "/ExportToFullDashboardPPT",
        data: "{demogDashboardData:" + JSON.stringify(demogDashboardData) + "}",
        contentType: "application/json",
        success: function (response) {
            if (response != "error")
                window.location.href = $("#URLDemogDashboardDownloadExpPPT").val() + "/?path=" + response;
            else {
                showMessage("Some error occured !");
            }
            HideLoader();
        },
        error: function (xhr, status, error) {
            //HideLoader();
            //showMessage(xhr.responseText)
            GoToErrorPage();
        }
    });

});

/**********PDF Download FOr Dashboard ***********/

$(document).on("click", ".exporttopdf-logo", function () {
    if ($(".dashboard-content").is(":visible") == false) {
        return;
    }

    ShowLoader();
    var leftpanelData = Comparisonlist[0].Name + " | ";
    leftpanelData += getTimeperiodEx().toUpperCase();//$(".timeType").val();

    //if (SelectedDempgraphicList.length > 0) {
    //    leftpanelData += ", ";
    //    for (var i = 0; i < SelectedDempgraphicList.length; i++) {
    //        leftpanelData += SelectedDempgraphicList[i].Name + ", ";
    //    }
    //    leftpanelData = leftpanelData.slice(0, -2);
    //}
    Selected_Filters = [];
    if (Grouplist.length > 0) {
        leftpanelData += ", ";
        $(Grouplist).each(function (i, d) {
            leftpanelData = leftpanelData.concat(d.Name.trim() + ", ");
            Selected_Filters.push(d.Name);
        });
        leftpanelData = leftpanelData.slice(0, -2);
    }
    var statTest = Selected_StatTest;
    if (Selected_StatTest == "CUSTOM BASE") {
        statTest = CustomBase[0].Name;
    }
    var demogDashboardData = new Object();
    demogDashboardData.TimePeriod = getTimeperiodEx().toUpperCase();
    demogDashboardData.Base = Comparisonlist[0].Name;
    demogDashboardData.CustomBase = (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
    demogDashboardData.Filters = Selected_Filters.join(", ");

    demogDashboardData.OutputData = MetricData;
    demogDashboardData.LeftpanelData = leftpanelData;
    demogDashboardData.statTest = statTest;
    demogDashboardData.pptOrPdf = "pdf";
    demogDashboardData.ss = Number(sdata.SampleSize.replace(",", ""));

    if (SelectedFrequencyList.length > 0) {
        demogDashboardData.ShopperFrequency = SelectedFrequencyList[0].Name;
        demogDashboardData.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }
    var P2P_SortText = "";
    if (P2P_Sort == "1") {
        P2P_SortText = "Size"
    }
    else {
        P2P_SortText = "Skew"
    }
    demogDashboardData.Sort = P2P_SortText;
    if (TabType.toLowerCase() == "trips") {
        demogDashboardData.TabType = "Trips";
    }
    else {
        demogDashboardData.TabType = "Shopper";
    }


    jQuery.ajax({
        type: "POST",
        url: $("#URLDemogDashboardFullExp").val(),//+ "/ExportToFullDashboardPPT",
        data: "{demogDashboardData:" + JSON.stringify(demogDashboardData) + "}",
        contentType: "application/json",
        success: function (response) {
            if (response != "error")
                window.location.href = $("#URLDemogDashboardDownloadExpPDF").val() + "/?path=" + response;
            else {
                showMessage("Some error occured !");
            }
            HideLoader();
        },
        error: function (xhr, status, error) {
            //HideLoader();
            //showMessage(xhr.responseText)
            GoToErrorPage();
        }
    });
});

function proceedClick() {
    $(".dashboard-content").css("visibility", "visible");
    $("#dashboard-popup").hide();
}
function closeSavePopup() {
    $(".dashboard-content").css("visibility", "hidden");
    $("#dashboard-popup").hide();
}
