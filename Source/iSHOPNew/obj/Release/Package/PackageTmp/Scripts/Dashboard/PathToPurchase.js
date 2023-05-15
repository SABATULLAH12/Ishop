/// <reference path="../Layout/Common.js" />
/// <reference path="../Layout/Layout.js" />

var AllPieChartData = [];
var MetricValue = [];
var MetricData = [];
var metricTypeDataSecondary = [];
var metricTypeData = [];
var sdata = [];
var time_pie_chart_flag = 0, timePieData = 0;
var active_metric_name = "";
var retailer_samplesize = 0;
var SaveFlag = 0;
var Selected_Filters = [];
var dynamicChanges = [{ "name": "WEEKDAY OR WEEKEND", "value": "none" },
                      { "name": "TIME OF DAY", "value": "none" },
                      { "name": "LOCATION PRIOR TO TRIP", "value": "none" },
                      { "name": "PLANNING", "value": "none" },
                      { "name": "Preparation Types", "value": "none" },
                      { "name": "WHO WITH", "value": "none" },
                      { "name": "CONSIDERATION", "value": "none" },
                      { "name": "REASON FOR STORE CHOICE", "value": "none" },
                      { "name": "DESTINATION ITEM", "value": "none" },
                      { "name": "DESTINATION ITEM-Nets", "value": "none" },
                      { "name": "TRIP MISSION", "value": "none" },
                      { "name": "ORDER SUMMARY", "value": "none" },
                      { "name": "NUMBER OF ITEMS", "value": "none" },
                      { "name": "DOLLARS SPENT", "value": "none" },
                      { "name": "PURCHASED NARTD", "value": "none" },
                      { "name": "IMMEDIATE CONSUMPTION", "value": "none" },
                      { "name": "TIME SPENT", "value": "none" },
                      { "name": "OVERALL SATISFACTION", "value": "none" },
                      { "name": "SATISFACTION DRIVERS", "value": "none" },
                      { "name": "LOCATION AFTER TRIP", "value": "none" },
];

function prepareContentArea() {
    //ShowLoader();
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
    $('.establishmt_img').css("background-image", 'url("../Images/P2PDashboardEsthmtImages/' + replace_file_special_characters(Comparisonlist[0].Name) + '.svg?39")');//replace("/","")

    var pathToPurchaseParams = new Object();
    if (SelectedFrequencyList.length > 0) {
        pathToPurchaseParams.ShopperFrequency = SelectedFrequencyList[0].Name;
        pathToPurchaseParams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }

    pathToPurchaseParams.StatTest = Selected_StatTest;
    pathToPurchaseParams.Sigtype_UniqueId = Sigtype_Id;
    pathToPurchaseParams.TimePeriod = TimePeriod;
    pathToPurchaseParams.TimePeriod_UniqueId = TimePeriod_UniqueId;
    pathToPurchaseParams.TimePeriodShortName = $(".timeType").val();

    if (CustomBase.length > 0) {
        pathToPurchaseParams.CustomBase_ShortName = CustomBase[0].Name;
        pathToPurchaseParams.CustomBase_UniqueId = CustomBase[0].UniqueId;
    }

    if (Comparisonlist.length > 0) {
        pathToPurchaseParams.Comparison_ShortNames = Comparisonlist[0].Name;
        pathToPurchaseParams.Comparison_UniqueIds = Comparisonlist[0].UniqueId;
    }

    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_UniqueId = [];
    for (var i = 0; i < Grouplist.length; i++) {
        Advanced_Filters_DBNames.push(Grouplist[i].DBName);
        Advanced_Filters_ShortNames.push(Grouplist[i].Name);
        Advanced_Filters_UniqueId.push(Grouplist[i].UniqueId);
    }
    pathToPurchaseParams.ShopperSegment = Advanced_Filters_ShortNames.join("|");
    pathToPurchaseParams.FilterShortname = Advanced_Filters_ShortNames.join("|");
    pathToPurchaseParams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");

    //add custom base dual filters   
    CustomBaseAdvancedFilters = [];
    CustomBaseAdvancedFilters_UniqueId = [];
    for (var i = 0; i < custombase_AddFilters.length; i++) {
        CustomBaseAdvancedFilters.push(custombase_AddFilters[i].Name);
        CustomBaseAdvancedFilters_UniqueId.push(custombase_AddFilters[i].UniqueId);
    }
    pathToPurchaseParams.CustomBaseAdvancedFilters = CustomBaseAdvancedFilters.join("|");
    pathToPurchaseParams.CustomBaseAdvancedFilters_UniqueId = CustomBaseAdvancedFilters_UniqueId.join("|");

    //add custom base frequency dual filters   
    CustomBaseFrequencyFilters = [];
    CustomBaseFrequency_UniqueId = [];
    for (var i = 0; i < custombase_Frequency.length; i++) {
        CustomBaseFrequencyFilters.push(custombase_Frequency[i].Name);
        CustomBaseFrequency_UniqueId.push(custombase_Frequency[i].UniqueId);
    }
    pathToPurchaseParams.CustomBaseShopperFrequency = CustomBaseFrequencyFilters.join("|");
    pathToPurchaseParams.CustomBaseShopperFrequency_UniqueId = CustomBaseFrequency_UniqueId.join("|");

    pathToPurchaseParams.Sort = P2P_Sort;
    var custompopUpText = "";
    if (Selected_StatTest.toUpperCase() == "CUSTOM BASE") {
        custompopUpText = CustomBase[0].Name;
    }
    else {
        custompopUpText = Selected_StatTest;
    }

    //competitor Params
    if (CompetitorRetailer.length > 0) {
        pathToPurchaseParams.CompetitorRetailer_Name = CompetitorRetailer[0].Name;
        pathToPurchaseParams.CompetitorRetailer_UniqueId = CompetitorRetailer[0].UniqueId;

        pathToPurchaseParams.CompetitorFrequency_Name = CompetitorFrequency[0].Name;
        pathToPurchaseParams.CompetitorFrequency_UniqueId = CompetitorFrequency[0].UniqueId;
    }
    if (CompetitorCustomBaseRetailer.length > 0) {
        pathToPurchaseParams.CustomBaseCompetitorRetailer_Name = CompetitorCustomBaseRetailer[0].Name;
        pathToPurchaseParams.CustomBaseCompetitorRetailer_UniqueId = CompetitorCustomBaseRetailer[0].UniqueId;

        pathToPurchaseParams.CustomBaseCompetitorFrequency_Name = CompetitorCustomBaseFrequency[0].Name;
        pathToPurchaseParams.CustomBaseCompetitorFrequency_UniqueId = CompetitorCustomBaseFrequency[0].UniqueId;
    }

    postBackData = "{pathToPurchaseParams:" + JSON.stringify(pathToPurchaseParams) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLPathToPurchaseData").val(),
        data: postBackData,
        contentType: "application/json",
        success: function (data) {

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
                    HideLoader();
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
                //$(".dashboard-content").css("visibility","visible");
            }
            HideLoader();
            if (SaveFlag == 1) {
                setTimeout(function () {
                    $('.TranslucentDiv').css("z-index", "6001");
                    $('.TranslucentDiv').show()
                }, 50);
                showMessage("Saved Successfully");
            }
            SaveFlag = 0;
        },
        error: function (xhr, status, error) {
            showMessage(xhr.responseText)
        }
    });
}
function createPieChart(parent_id, classForElement, per, width_fact, color1, color2) {
    var cssClass = parent_id + " .main." + "_id" + classForElement;
    var paddFact = 0;
    if (parent_id == ".PURCHASED_NARTD_CHRT")
        $(parent_id).append('<div class="main _id' + classForElement + '"><div class="submain pieClass"><p class="purchased_nartd"></p><svg class="PieChart ' + classForElement + '"><g></g></svg></div></div>');
    else if (parent_id == ".IMMEDIATE_CONSUMPTION_CHRT")
        $(parent_id).append('<div class="main _id' + classForElement + '"><div class="submain pieClass"><p class="immediate_consumption"></p><svg class="PieChart ' + classForElement + '"><g></g></svg></div></div>');
    else
        $(parent_id).append('<div class="main _id' + classForElement + '"><div class="submain pieClass"><svg class="PieChart ' + classForElement + '"><g></g></svg></div></div>');
    //Set height for PieChart
    //Set height for PieChart
    $(cssClass).css("height", width_fact + "px");
    $(cssClass).css("width", width_fact + "px");
    var height = $(cssClass).height();
    var width = $(cssClass).width();
    var arc = d3.svg.arc()
      .innerRadius(0)
      .outerRadius(height / 2 - paddFact)
      .startAngle(0)
      .endAngle((2 * Math.PI));
    var svg = d3.select(cssClass).select("g");
    svg.attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");
    svg.append("path")
    .attr("class", "arcMain")
    .attr("d", arc).style("fill", color2);
    arc = d3.svg.arc()
      .innerRadius(0)
      .outerRadius(height / 2 - paddFact)
      .startAngle(0)
      .endAngle((2 * Math.PI) * (per / 100));
    svg.append("path")
    .attr("class", "arcMain")
    .attr("d", arc).style("fill", color1);
}
var callAllBalloonPieCharts = function (i, v) {
    var pieChrtroadCls = '';
    //if (isThresholdLmt)
    //pieChrtroadCls = ".pie_roads_3_" + i;
    //else
    pieChrtroadCls = ".pie_roads_4_" + i;
    //createBalloonPieChart(".pie_charts" + i + " " + pieChrtroadCls, "ballon1", v, $(".pie_charts" + i).height(), "#F15F2E", "#DFDFDF");
    createBalloonPieChart(pieChrtroadCls, "ballon1", v, $(pieChrtroadCls).height(), "#F15F2E", "#DFDFDF");

}
var createBalloonPieChart = function (parent_id, classForElement, per, width_fact, color1, color2) {
    var cssClass = parent_id + " .main." + "_id" + classForElement;
    $(parent_id).append('<div class="main _id' + classForElement + '"><div class="submain"><p class="p_submain"></p><svg class="ballonPieChart ' + classForElement + '"><g></g></svg></div></div>');
    //need to change the css for height and width
    $(cssClass).css("height", width_fact + "px");
    $(cssClass).css("width", (width_fact / 1.181102362204724) + "px");
    drawBalloonPieChart(cssClass, per, color1, color2);
}
var drawBalloonPieChart = function (id, per, color1, color2) {
    var padding = 0;
    var height = $(id).height();
    var width = $(id).width();
    var arc = d3.svg.arc()
      .innerRadius(0)
      .outerRadius(height / 2 - padding)
      .startAngle(0)
      .endAngle(2 * Math.PI);
    var svg = d3.select(id).select("g");
    svg.attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");
    svg.append("path")
    .attr("class", "arcMain")
    .attr("d", arc).style("fill", color2);
    arc = d3.svg.arc()
      .innerRadius(0)
      .outerRadius(height / 2 - padding)
      .startAngle(0)
      .endAngle((2 * Math.PI) * (per / 100));
    svg.append("path")
    .attr("class", "arcMain")
    .attr("d", arc).style("fill", color1);
}
var createTimePieChart = function (parent_id, classForElement, per, color1, color2) {
    var cssClass = parent_id + " .time_main." + "_id" + classForElement;
    var x_fact = 0.7, x_trans_Fact = 1.41, y_trans_Fact = 1.6;
    $(parent_id).append('<div class="time_main _id' + classForElement + '"><div class="submain"><p class="time_submain"></p><svg class="timePieChart ' + classForElement + '"><g></g></svg></div></div>');
    //Change the height and width of time_main based on image ratio
    //$(cssClass).css("height",($(parent_id).height())*(150/237));
    $(cssClass).css("height", "100%");
    $(cssClass).css("width", $(parent_id).width());
    var height = $(cssClass).height() * x_fact;
    var width = $(cssClass).width();
    var arc = d3.svg.arc()
      .innerRadius(0)
      .outerRadius(height / 2 * 1.17)
      .startAngle(0)
      .endAngle((2 * Math.PI));
    var svg = d3.select(cssClass).select("g");
    svg.attr("transform", "translate(" + (width / 2 * x_trans_Fact) + "," + (height / 2 * y_trans_Fact) + ")");
    svg.append("path")
    .attr("class", "arcMain")
    .attr("d", arc).style("fill", color2);
    arc = d3.svg.arc()
      .innerRadius(0)
      .outerRadius(height / 2 * 1.17)
      .startAngle(0)
      .endAngle((2 * Math.PI) * (per / 100));
    svg.append("path")
    .attr("class", "arcMain")
    .attr("d", arc).style("fill", color1);
}
function SetVolume(Volume) {
    var _volume = 0;
    if (Volume != null && Volume != "") {
        _volume = parseFloat(Volume);
    }
    return parseFloat(_volume).toFixed(0) + "%";
}
function SetMetricWiseVolume(Volume, metrictype) {
    var _volume = 0;
    if (Volume != null && Volume != "") {
        _volume = parseFloat(Volume);
    }
    if (metrictype.toLowerCase() == "#dollar_spent")
        return "$" + parseFloat(_volume).toFixed(0);
    else if (metrictype.toLowerCase() == "#numberofitems") {
        return parseFloat(_volume).toFixed(1);
    }
    else
        return parseFloat(_volume).toFixed(0) + "%";
}
function SetChangeVolume(ChangeVolume) {
    var _significance = 0;
    if (ChangeVolume != null && ChangeVolume != "") {
        _significance = parseFloat(ChangeVolume).toFixed(1);
        //if (ChangeVolume == "-0.0" || ChangeVolume == "+0.0") {
        //    return ChangeVolume;
        //}
        //else
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
    var sigcolor = "#58595B;";
    var _significance = 0;
    if (Significance != null && Significance != "") {
        _significance = parseFloat(Significance);
        if (_significance > Stat_PositiveValue) {
            sigcolor = "#20B250";
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
function update_metric_data(data) {
    if (data != null && data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var metrictype = data[i].MetricType.toLowerCase();
            switch (metrictype) {
                case "reason for store choice":
                    {
                        update_reason_for_store_choise(data[i]);
                        break;
                    }
                case "location after trip":
                    {
                        update_location_after_visit(data[i]);
                        break;
                    }
                case "overall satisfaction":
                    {
                        update_overall_satisfaction(data[i]);
                        break;
                    }
                case "trip mission":
                    {
                        update_trip_mission(data[i]);
                        break;
                    }
                case "planning":
                    {
                        update_planning(data[i]);
                        break;
                    }
                case "consideration":
                    {
                        update_considaration(data[i]);
                        break;
                    }
                case "location prior to trip":
                    {
                        update_location_prior_to_visit(data[i]);
                        break;
                    }
                case "weekday or weekend":
                    {
                        update_weekday_or_weekend(data[i]);
                        break;
                    }
                case "time of day":
                    {
                        update_time_of_day(data[i]);
                        break;
                    }
                case "preparation types":
                    {
                        update_preparation_types(data[i]);
                        break;
                    }
                case "destination item":
                    {
                        update_destination_item(data[i]);
                        break;
                    }
                case "order summary":
                    {
                        update_order_summary(data[i]);
                        break;
                    }
                case "satisfaction drivers":
                    {
                        update_satisfaction_drivers(data[i]);
                        break;
                    }
                case "immediate consumption":
                    {
                        update_beverage_consumption(data[i]);
                        break;
                    }
                case "who with":
                    {
                        update_who_with(data[i]);
                        break;
                    }
                case "purchased nartd":
                    {
                        update_purchased_summary(data[i]);
                        break;
                    }
                case "time spent":
                    {
                        update_time_spent(data[i]);
                        break;
                    }
                case "dollars spent":
                    {
                        update_dollars_spent(data[i]);
                        break;
                    }
                case "number of items":
                    {
                        update_number_of_items(data[i]);
                        break;
                    }
            }
        }
    }
}
function update_metricType_data(metric_row, metricType) {
    if (metric_row != null && metric_row.MetricData.length > 0) {
        $(metricType + " .metric-item").each(function (i) {
            var metric_obj = metric_row.MetricData[i];

            var Retailer = metric_obj.Retailer;
            var MetricType = metric_obj.MetricType;
            var Metric = metric_obj.Metric;
            var Volume = metric_obj.Volume;
            var Significance = metric_obj.Significance;
            var ChangeVolume = metric_obj.ChangeVolume;
            var Flag = metric_obj.Flag;
            var Selected_Popup_Metric_Item = metric_obj.Selected_Popup_Metric_Item;


            if (metricType == "#Time_Spent") {
                if (Metric.toLocaleLowerCase().indexOf("or") > -1) {
                    $(this).children("ul").children("li").find(".all_sub_text").html(Metric);
                }
                else {
                    var min = Metric.toLocaleLowerCase().split("min")[0].trim();
                    var addminclass = "<div data-val='" + Metric + "' class='timespent_val'>" + min + "</div><div class='timespent_min'>Min</div>";
                    $(this).children("ul").children("li").find(".all_sub_text").html(addminclass);
                }
            }
            else if (metricType == "#Purchased_NARTD") {
                $(this).children("ul").children("li").find(".all_sub_text").html();
            }
            else {
                $(this).children("ul").children("li").find(".all_sub_text").html(Metric);
            }
            if ($(this).children("ul").children("li").children(".all_meric_per").hasClass("vertical-align")) {
                $(this).children("ul").children("li").children(".all_meric_per").children(".vertical-align").html(SetMetricWiseVolume(Volume, metricType));
            }
            else {
                $(this).children("ul").children("li").find(".all_meric_per").html(SetMetricWiseVolume(Volume, metricType));
            }

            $(this).children("ul").children("li").find(".sig_text").html(SetChangeVolume(ChangeVolume)).css("color", SetSignificanceColor(Significance));
        });
    }
}

function update_time_spent(_metric_type_data) {
    // update_metricType_data(_metric_type_data,"#Time_Spent");
    var average = new Object();
    average.MetricData = [];
    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (_metric_type_data.MetricData[i].Flag == 1) {
            average.MetricData.push(_metric_type_data.MetricData[i]);
            break;
        }
    }
    update_metricType_data(average, "#Time_Spent");
}
function update_dollars_spent(_metric_type_data) {
    var average = new Object();
    average.MetricData = [];
    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (_metric_type_data.MetricData[i].Flag == 2) {
            average.MetricData.push(_metric_type_data.MetricData[i]);
            break;
        }
    }
    update_metricType_data(average, "#Dollar_Spent");
}
function update_number_of_items(_metric_type_data) {
    var average = new Object();
    average.MetricData = [];

    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (_metric_type_data.MetricData[i].Flag == 2) {
            average.MetricData.push(_metric_type_data.MetricData[i]);
            break;
        }
    }
    update_metricType_data(average, "#NumberOfItems");
}
function update_beverage_consumption(_metric_type_data) {
    var average = new Object();
    average.MetricData = [];
    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (_metric_type_data.MetricData[i].Flag == 2) {
            average.MetricData.push(_metric_type_data.MetricData[i]);
            break;
        }
    }
    update_metricType_data(average, "#Immediate_Consumption");
    //update_metricType_data(_metric_type_data,"#Immediate_Consumption");
    $(".IMMEDIATE_CONSUMPTION_CHRT").html("");
    createPieChart(".IMMEDIATE_CONSUMPTION_CHRT", "ballon1", average.MetricData[0].Volume == undefined ? 0 : average.MetricData[0].Volume, $(".IMMEDIATE_CONSUMPTION_CHRT").height(), "#f15f2e", "#dfdfdf");
}
function update_order_summary(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#TopItems");
    //add image
    $("#TopItems .img1").css("background-image", 'url("../Images/ishop-P2P-Icons/Top Items/' + replace_file_special_characters(_metric_type_data.MetricData[0].Metric) + '.svg?39")');
    $("#TopItems .img2").css("background-image", 'url("../Images/ishop-P2P-Icons/Top Items/' + replace_file_special_characters(_metric_type_data.MetricData[1].Metric) + '.svg?39")');
    $("#TopItems .img3").css("background-image", 'url("../Images/ishop-P2P-Icons/Top Items/' + replace_file_special_characters(_metric_type_data.MetricData[2].Metric) + '.svg?39")');
}
function update_weekday_or_weekend(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#weekday-or-weekend");
}
function update_time_of_day(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#time-of-day");
}
function update_satisfaction_drivers(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Satisfaction_Drivers");
    //update images
    if (_metric_type_data != null && _metric_type_data.MetricData.length > 0) {
        $("#Satisfaction_Drivers .metric-item").each(function (i) {
            var metric_obj = _metric_type_data.MetricData[i];
            var MetricType = metric_obj.MetricType;
            var Metric = metric_obj.Metric;
            $(this).children("ul").children("li").find(".img_satisfaction_drivers").css("background-image", 'url("../Images/ishop-P2P-Icons/' + _metric_type_data.MetricData[i].MetricType + '/' + replace_file_special_characters(_metric_type_data.MetricData[i].Metric) + '.svg?39")');
        });
    }
}
function update_location_prior_to_visit(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Location_Prior");
    //add image
    $(".LocationPriorToTrip").css("background-image", 'url("../Images/ishop-P2P-Icons/' + _metric_type_data.MetricData[0].MetricType + '/' + replace_file_special_characters(_metric_type_data.MetricData[0].Metric) + '.svg?39")');
}
function update_planning(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Planning_Type");
    //add image
    var sParentName = "Planning";
    var val = 0;
    var sValue = _metric_type_data.MetricData[0].Volume;
    var metrictype = _metric_type_data.MetricData[0].Metric;
    var url = "";
    if (metrictype.toLowerCase() == "spur of the moment") {
        url = "url('../Images/ishop-P2P-Icons/" + sParentName + "/P2P_Planning_Pie_Chart_Bg.svg?39')";
        $("." + sParentName).eq(0).hide();
        time_pie_chart_flag = 1; timePieData = sValue;
        $(".Planning_Type .p_pieChart").html('');
        $(".Planning_Type").show(); $(".Planning_Type .p_img").hide();
        createTimePieChart(".Planning_Type .p_pieChart", "Pie4", sValue, "#8dc63f", "transparent");
    }
    else {
        $(".Planning_Type .p_pieChart").html('');
        $(".Planning_Type").show(); $(".Planning_Type .p_img").hide();
        url = "url('../Images/ishop-P2P-Icons/" + sParentName + "/" + metrictype + ".svg?39')";
        $("." + sParentName).eq(0).show();
    }

    $("." + sParentName).eq(0).css("background-image", url);
}
function update_considaration(_metric_type_data) {
    var average = new Object();
    average.MetricData = [];
    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (_metric_type_data.MetricData[i].Flag == 2) {
            average.MetricData.push(_metric_type_data.MetricData[i]);
            break;
        }
    }
    update_metricType_data(average, "#Consideration");
    //update_metricType_data(_metric_type_data,"#Consideration");
    $(".TRAVELLED-OUT-OF-THE-WAY").html("");
    createPieChart(".TRAVELLED-OUT-OF-THE-WAY", "ballon1", average.MetricData[0].Volume == undefined ? 0 : average.MetricData[0].Volume, $(".TRAVELLED-OUT-OF-THE-WAY").height(), "#f15f2e", "#dfdfdf");
}
function update_who_with(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Who_With");
    //add image
    $(".WhoWith").css("background-image", 'url("../Images/ishop-P2P-Icons/' + _metric_type_data.MetricData[0].MetricType + '/' + replace_file_special_characters(_metric_type_data.MetricData[0].Metric) + '.svg?39")');
}
function update_reason_for_store_choise(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Reason_For_Store_Choice");
}
function update_trip_mission(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#TripMission");
    if (_metric_type_data.MetricData.length >= 4) {
        //add images       
        $("#TripMission .metric-item").each(function (i) {
            $(this).children("ul").children("li").find(".img").css("background-image", 'url("../Images/ishop-P2P-Icons/Trip Mission/' + replace_file_special_characters(_metric_type_data.MetricData[i].Metric) + '.svg?39")');
        });

        $(".pie_charts").html("");
        callAllBalloonPieCharts(0, _metric_type_data.MetricData[0].Volume);
        callAllBalloonPieCharts(1, _metric_type_data.MetricData[1].Volume);
        callAllBalloonPieCharts(2, _metric_type_data.MetricData[2].Volume);
        callAllBalloonPieCharts(3, _metric_type_data.MetricData[3].Volume);
    }
}
function update_preparation_types(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#PreparationTypes");
}
function update_destination_item(_metric_type_data) {
    //update_metricType_data(_metric_type_data, "#DestinationItems");
    var average = new Object();
    average.MetricData = [];
    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (_metric_type_data.MetricData[i].Flag == 1) {
            average.MetricData.push(_metric_type_data.MetricData[i]);
        }
    }
    update_metricType_data(average, "#DestinationItems");

    //add image
    var j = 1;
    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (j == 4) {
            break;
        }
        else if (_metric_type_data.MetricData[i].Flag == 1) {
            $("#DestinationItems .img" + j).css("background-image", 'url("../Images/ishop-P2P-Icons/Destination Item/' + replace_file_special_characters(_metric_type_data.MetricData[i].Metric) + '.svg?39")');
            j++;
        }
        else {
            continue;
        }
        //$("#DestinationItems .img2").css("background-image", 'url("../Images/ishop-P2P-Icons/Destination Item/' + replace_file_special_characters(_metric_type_data.MetricData[1].Metric) + '.svg?39")');
        //$("#DestinationItems .img3").css("background-image", 'url("../Images/ishop-P2P-Icons/Destination Item/' + replace_file_special_characters(_metric_type_data.MetricData[2].Metric) + '.svg?39")');
    }
    _metric_type_data = GetMetricTypeData("Destination Item-Nets");
    for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
        if (j == 4) {
            break;
        }
        else if (_metric_type_data.MetricData[i].Flag == 1) {
            $("#DestinationItems .img" + j).css("background-image", 'url("../Images/ishop-P2P-Icons/Destination Item/' + replace_file_special_characters(_metric_type_data.MetricData[i].Metric) + '.svg?39")');
            j++;
        }
        else {
            continue;
        }
    }

}
function update_location_after_visit(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Location_After_Visit");
    //add image
    $(".LocationAfterTrip").css("background-image", 'url("../Images/ishop-P2P-Icons/' + _metric_type_data.MetricData[0].MetricType + '/' + replace_file_special_characters(_metric_type_data.MetricData[0].Metric) + '.svg?39")');
}
function update_overall_satisfaction(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Overall_Satisfaction");
    $(".OVERALL_SATISFACTION_CHRT").html("");
    createPieChart(".OVERALL_SATISFACTION_CHRT", "ballon1", _metric_type_data.MetricData[0].Volume == undefined ? 0 : _metric_type_data.MetricData[0].Volume, $(".OVERALL_SATISFACTION_CHRT").height(), "#f15f2e", "#dfdfdf");
}
function update_purchased_summary(_metric_type_data) {
    update_metricType_data(_metric_type_data, "#Purchased_NARTD");
    $(".PURCHASED_NARTD_CHRT").html("");
    createPieChart(".PURCHASED_NARTD_CHRT", "ballon1", _metric_type_data.MetricData[0].Volume == undefined ? 0 : _metric_type_data.MetricData[0].Volume, $(".PURCHASED_NARTD_CHRT").height(), "#f15f2e", "#dfdfdf");
}
function GetMetricTypeData(metrictype) {
    var metricL = GetActualMetricName(metrictype);
    var _metric_data = [];
    if (MetricData != null && MetricData.length > 0) {
        for (var i = 0; i < MetricData.length; i++) {
            if (MetricData[i].MetricType.toLowerCase() == metricL.toLowerCase()) {
                _metric_data = MetricData[i];
                break;
            }
        }
    }
    return _metric_data;
}

function GetActualMetricName(metrictype) {
    if (metrictype.toLowerCase() == "destination item") {
        return "Destination Item";
    }
    else if (metrictype.toLowerCase() == "day of the week") {
        return "weekday or weekend";
    }
    else if (metrictype.toLowerCase() == "stores considered") {
        return "consideration";
    }
    else if (metrictype.toLowerCase() == "ic items in basket - (consumed within 1 hour)") {
        return "immediate consumption";
    }
    else if (metrictype.toLowerCase() == "items purchased") {
        return "order summary";
    }
    else {
        return metrictype;
    }

}
function updatepopupdata(obj) {
    metricTypeData = GetMetricTypeData($(obj).attr("data-val"));
    if ($(obj).attr("data-val-secondary")) {
        metricTypeDataSecondary = GetMetricTypeData($(obj).attr("data-val-secondary"));
    }
    else metricTypeDataSecondary = [];
    var popup_type = $(obj).attr("popup-type").toLowerCase();
    if (popup_type == "chart")
        update_popup_metric_dynamic_data(metricTypeData, obj);
    else
        update_popup_metric_static_data(metricTypeData, obj);


    SetScroll($(".bar-content-header"), "rgb(241, 95, 46)", 0, 0, 0, 0, 4);
}
//******* update popup metric dynamic data *********
function update_popup_metric_dynamic_data(_metric_type_data, obj) {
    var htmlstring = "<div class=\"bar-content-header\" tabindex=\"0\" style=\"\">";
    var alt_color = "backgn-clr";
    if (_metric_type_data != null && _metric_type_data.MetricData.length > 0) {
        for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
            if (_metric_type_data.MetricData[i].Flag != 2) {
                htmlstring += GetDynamicpopupMetricRow(_metric_type_data.MetricData[i], obj, alt_color);
                if (alt_color == "backgn-clr")
                    alt_color = "";
                else
                    alt_color = "backgn-clr";
            }
        }
    }
    htmlstring += "</div>";
    htmlstring += "</div>";

    $(".popup1 .popup-body").html(htmlstring);
}
//************ end ********

//******* update popup metric dynamic data *********
function update_popup_metric_static_data(_metric_type_data, obj) {
    var cell_count = 0;
    var MetricLen = _.filter(_metric_type_data.MetricData, function (o) {
        return o.Flag != '2';
    }).length;
    var htmlstring = "<div class=\"bar-content-header tableShadowImage\" tabindex=\"0\" style=\"\">";
    var header = $(".popup1 .metric-type-name").html();
    if (header == "IC ITEMS IN BASKET - (CONSUMED WITHIN 1 HOUR)")
        header = "IC ITEMS";
    htmlstring = (($(obj).attr("data-val-secondary")) ? " <div class='popup-hdr-stat-textdiv-second'> <div class='popup-hdr-metric-text' >" + header + " - Detail</div></div>" : "") + htmlstring;
    
    if (_metric_type_data != null && _metric_type_data.MetricData.length > 0) {
        for (var i = 0; i < _metric_type_data.MetricData.length; i++) {
            if (_metric_type_data.MetricData[i].Flag != 2) {
                if (cell_count < Math.ceil(MetricLen / 2)) {
                    if (cell_count == 0)
                        htmlstring += "<div class=\"bar-content-divleft\"><div class=\"bar-content-main-divleft\">";

                    cell_count++;
                    htmlstring += GetStaticpopupMetricRow(_metric_type_data.MetricData[i], obj);
                }
                else if (cell_count >= Math.ceil(MetricLen / 2)) {
                    if (cell_count == Math.ceil(MetricLen / 2))
                        htmlstring += "</div></div><div class=\"bar-content-divright\"><div class=\"bar-content-main-divleft\">";

                    cell_count++;
                    htmlstring += GetStaticpopupMetricRow(_metric_type_data.MetricData[i], obj);
                }
                //cell_count=0;

            }
        }
    }
    if ($(obj).attr("data-val-secondary")){
        $(".popup1 .popup-hdr-stat-textdiv .popup-hdr-metric-text").show();
        $(".popup1 .bar-content-divleft").attr("style", "height:auto");
        cell_count = 0;
        MetricLen = _.filter(metricTypeDataSecondary.MetricData, function (o) {
            return o.Flag != '2';
        }).length;
        htmlstring += "</div></div></div> <div class='popup-hdr-stat-textdiv-second'> <div class='popup-hdr-metric-text' >" + header + " - Nets</div></div> <div class=\"bar-content-header tableShadowImage\" tabindex=\"0\" style=\"\">";
        if (metricTypeDataSecondary != null && metricTypeDataSecondary.MetricData.length > 0) {
            for (var i = 0; i < metricTypeDataSecondary.MetricData.length; i++) {
                if (metricTypeDataSecondary.MetricData[i].Flag != 2) {
                    if (cell_count < Math.ceil(MetricLen / 2)) {
                        if (cell_count == 0)
                            htmlstring += "<div class=\"bar-content-divleft\"><div class=\"bar-content-main-divleft\">";

                        cell_count++;
                        htmlstring += GetStaticpopupMetricRow(metricTypeDataSecondary.MetricData[i], obj);
                    }
                    else if (cell_count >= Math.ceil(MetricLen / 2)) {
                        if (cell_count == Math.ceil(MetricLen / 2))
                            htmlstring += "</div></div><div class=\"bar-content-divright\"><div class=\"bar-content-main-divleft\">";

                        cell_count++;
                        htmlstring += GetStaticpopupMetricRow(metricTypeDataSecondary.MetricData[i], obj);
                    }
                    //cell_count=0;

                }
            }
        }

    }
    else {
        $(".popup1 .popup-hdr-stat-textdiv .popup-hdr-metric-text").hide();      
    }
    htmlstring += "</div>";
    htmlstring += "</div></div></div>";

    $(".popup1 .popup-body").html(htmlstring);

    if ($(obj).attr("data-val-secondary")) {
        if (header == "IC ITEMS") {
            $(".popup1 .popup-body .bar-content-header").first().css("height", "62%");
        }
        else {
            $(".popup1 .popup-body .bar-content-header").first().css("height", "54%");
        }     
        $(".popup1 .popup-body .bar-content-header").last().css({ "max-height": "25%", "height": "auto" });

    }
    
    
}
//************ end ********

function GetDynamicpopupMetricRow(metric_row, obj, alt_bc_color) {
    var Retailer = metric_row.Retailer;
    var MetricType = metric_row.MetricType;
    var Metric = metric_row.Metric;
    var Volume = metric_row.Volume;
    var Significance = metric_row.Significance;
    var ChangeVolume = metric_row.ChangeVolume;
    var Flag = metric_row.Flag;
    var Selected_Popup_Metric_Item = metric_row.Selected_Popup_Metric_Item;

    var popup_metric_row = "";
    if ($(obj).attr("options") == "true") {
        if (metric_row.MetricType == "ORDER SUMMARY" || metric_row.MetricType == "DESTINATION ITEM") {
            popup_metric_row = "<div class=\"bar-content-main-divleft\">";
            if (Selected_Popup_Metric_Item) {
                //popup_metric_row += "<input style=\"float: left;margin-top: 2%;margin-left: 0%;\" type=\"checkbox\" checked name=\"checkbox\" id=\"checkbox_id\" value=\"" + Metric + "\" data-val=\"" + Metric + "\">" ;
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\">" +
                        "<div id=\"checkbox_id\" class=\"bar-cntnt-chkboximg bar-cntnt-chkboximg-active\" data-val=\"" + Metric + "\" value=\"" + Metric + "\"></div>" +
                      "</div>";
            }
            else {
                //popup_metric_row += "<input style=\"float: left;margin-top: 2%;margin-left: 0%;\" type=\"checkbox\" name=\"checkbox\" id=\"checkbox_id\" value=\"" + Metric + "\" data-val=\"" + Metric + "\">" ;
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\">" +
                          "<div id=\"checkbox_id\" class=\"bar-cntnt-chkboximg\" data-val=\"" + Metric + "\" value=\"" + Metric + "\"></div>" +
                        "</div>";
            }
        }
        else {
            popup_metric_row = "<div class=\"bar-content-main-div\">";
            if (Selected_Popup_Metric_Item) {
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\">" +
                        "<div id=\"radio-click\" class=\"bar-cntnt-radioimg barcntntradioactiveimg\" data-val=\"" + Metric + "\"></div>" +
                      "</div>";
            }
            else {
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\">" +
                       "<div id=\"radio-click\" class=\"bar-cntnt-radioimg\" data-val=\"" + Metric + "\"></div>" +
                     "</div>";
            }
        }
        popup_metric_row += "<div class=\"bar-cntnt-metric-brdr\"></div>";
    }
    else {
        popup_metric_row = "<div class=\"bar-content-main-div\">";
    }
    popup_metric_row += "<div class=\"bar-cntnt-metric-name\">" + Metric + "</div>";
    if (metric_row.MetricType != "ORDER SUMMARY" && metric_row.MetricType != "DESTINATION ITEM") {
        if ($(obj).attr("options") == "true")
            popup_metric_row += "<div class=\"bar-cntnt-metric-div " + alt_bc_color + "\">";
        else
            popup_metric_row += "<div class=\"bar-cntnt-metric-div " + alt_bc_color + "\" style=\"width:63%;\">";

        popup_metric_row += "<div class=\"bar-cntnt-innerdiv\">" +
           "<div class=\"bar-cntnt-outer\">" +
              "<div class=\"bar-cntnt-inner\">" +
                 "<div class=\"bar-cnt-inner-dashdiv\"> </div>" +
                 "<div class=\"bar-cnt-inner-metrc-percnt\" style=\"width:" + parseFloat(Volume).toFixed(0) + "%\"></div>" +
              "</div>" +
           "</div>" +
        "</div>" +
        "<div class=\"bar-cnt-left-metrc-val-brdr\"></div>";
    }
    else {
        popup_metric_row += "<div class=\"bar-cntnt-metric-div " + alt_bc_color + "\" style=\"background-color: transparent;border:0;\">";
    }
    popup_metric_row += "<div class=\"bar-cntnt-metric-val\" style=\"color:#58595B\">" + parseFloat(Volume).toFixed(0) + "%</div>" +
       "<div class=\"bar-cntnt-brder\" style=\"background-color:#58595B\"></div>" +
       "<div class=\"bar-cntnt-change-val\" style=\"color:" + SetSignificanceColor(Significance) + "\">" + SetChangeVolume(ChangeVolume) + "</div>" +
    "</div>" +
 "</div>";
    return popup_metric_row;
}
function GetStaticpopupMetricRow(metric_row, obj) {
    var Retailer = metric_row.Retailer;
    var MetricType = metric_row.MetricType;
    var Metric = metric_row.Metric;
    var Volume = metric_row.Volume;
    var Significance = metric_row.Significance;
    var ChangeVolume = metric_row.ChangeVolume;
    var Flag = metric_row.Flag;
    var Selected_Popup_Metric_Item = metric_row.Selected_Popup_Metric_Item;
    popup_metric_row = "<div class=\"bar-content-left\">";
    var getWidth = "";
    if ($(obj).attr("options") == "true") {
        if (metric_row.MetricType == "ORDER SUMMARY" || metric_row.MetricType == "ORDER SUMMARY-Nets" || metric_row.MetricType == "DESTINATION ITEM" || metric_row.MetricType == "DESTINATION ITEM-Nets") {
            //popup_metric_row = "<div class=\"bar-content-main-divleft\">";
            if (Selected_Popup_Metric_Item) {
                //popup_metric_row += "<input style=\"float: left;margin-top: 2%;margin-left: 0%;\" type=\"checkbox\" checked name=\"checkbox\" id=\"checkbox_id\" value=\"" + Metric + "\" data-val=\"" + Metric + "\">" ;
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\">" +
                        "<div id=\"checkbox_id\" class=\"bar-cntnt-chkboximg bar-cntnt-chkboximg-active\" data-val=\"" + Metric + "\" value=\"" + Metric + "\"></div>" +
                      "</div>";
            }
            else {
                //popup_metric_row += "<input style=\"float: left;margin-top: 2%;margin-left: 0%;\" type=\"checkbox\" name=\"checkbox\" id=\"checkbox_id\" value=\"" + Metric + "\" data-val=\"" + Metric + "\">" ;
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\">" +
                          "<div id=\"checkbox_id\" class=\"bar-cntnt-chkboximg\" data-val=\"" + Metric + "\" value=\"" + Metric + "\"></div>" +
                        "</div>";


            }
        }
        else if (metric_row.MetricType.toUpperCase() == "REASON FOR STORE CHOICE") {
            if (Selected_Popup_Metric_Item) {
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\" style=\"width: 11%\">" +
         "<div id=\"radio-click\" style=\"margin-left: 8px;\" class=\"bar-cntnt-radioimg barcntntradioactiveimg\" data-val=\"" + Metric + "\" value=\"" + Metric + "\"></div>" +
         "<div class=\"bar-cntnt-metric-brdr topfdbrdr\"></div>" +
      "</div>";
            }
            else {
                popup_metric_row += "<div class=\"bar-cntnt-radiodiv\" style=\"width: 11%\">" +
             "<div id=\"radio-click\" style=\"margin-left: 8px;\" class=\"bar-cntnt-radioimg marginforradiotop\" data-val=\"" + Metric + "\" value=\"" + Metric + "\"></div>" +
             "<div class=\"bar-cntnt-metric-brdr topfdbrdr\"></div>" +
          "</div>";
            }
        }
    }

    if (metric_row.MetricType == "ORDER SUMMARY" || metric_row.MetricType == "ORDER SUMMARY-Nets" || metric_row.MetricType == "DESTINATION ITEM" || metric_row.MetricType == "DESTINATION ITEM-Nets" || metric_row.MetricType.toUpperCase() == "REASON FOR STORE CHOICE") {
        getWidth = "62%";
    }
    else {
        getWidth = "70%";
    }

    popup_metric_row += "<div class=\"bar-cntnt-metric-name top-fditem-lnehght\" style=\"width:" + getWidth + "\">" + Metric + "</div>" +
    "<div class=\"bar-cntnt-metric-val\" style=\"color:#58595B\">" + parseFloat(Volume).toFixed(0) + "%</div>" +
    "<div class=\"bar-cntnt-brder\" style=\"background-color:#58595B\"> </div>" +
    "<div class=\"bar-cntnt-change-val topfditemchange\" style=\"color:" + SetSignificanceColor(Significance) + "\">" + SetChangeVolume(ChangeVolume) + "</div></div>";
    return popup_metric_row;
}
function update_popup_metric_item_Id(metricTypeData) {
    for (var i = 0; i < metricTypeData.MetricData.length; i++) {
        metricTypeData.MetricData[i].Selected_Popup_Metric_Item = false;
    }
    $(".popup1 .bar-cntnt-radioimg").each(function (i) {
        if ($(this).hasClass("barcntntradioactiveimg")) {
            metricTypeData.MetricData[i].Selected_Popup_Metric_Item = true;
            return false;
        }
    });
    $(".popup1 .bar-cntnt-chkboximg-active").each(function (i, j) {
        _.filter(metricTypeData.MetricData, function (k, l) {
            if ($(j).attr("data-val") == k.Metric.replace('&lt;', '<'))
                metricTypeData.MetricData[l].Selected_Popup_Metric_Item = true;
        });
    });
}
function dynamicUpdate(metricparent, metrictype, Value, Change, Sig) {
    this.metricparent = metricparent;
    this.metrictype = metrictype;
    this.Sig = Sig;
    this.Value = parseFloat(Value).toFixed(1);
    this.Change = Change;
    this.editedParentName = function (parentname) { return parentname.replace(/ /g, '') };
}
dynamicUpdate.prototype.changebackground = function () {
    //Update Image
    var sParentName = this.editedParentName(this.metricparent);
    var url = "url('../Images/ishop-P2P-Icons/" + this.metricparent + "/" + replace_file_special_characters(this.metrictype) + ".svg?39')";
    $("." + sParentName).eq(0).css("background-image", url);
};
dynamicUpdate.prototype.changebackgroundplanning = function (sValue) {
    var sParentName = this.editedParentName(this.metricparent);
    var val = 0;   

    var url = "";
    if (this.metrictype.toLowerCase() == "spur of the moment") {
        url = "url('../Images/ishop-P2P-Icons/" + this.metricparent + "/P2P_Planning_Pie_Chart_Bg.svg?39')";
        $("." + sParentName).eq(0).hide();
        time_pie_chart_flag = 1; timePieData = this.Value;
        $(".Planning_Type .p_pieChart").html('');
        $(".Planning_Type").show(); $(".Planning_Type .p_img").hide();
        createTimePieChart(".Planning_Type .p_pieChart", "Pie4", this.Value, "#8dc63f", "transparent");
    }
    else {
        $(".Planning_Type .p_pieChart").html('');
        $(".Planning_Type").show(); $(".Planning_Type .p_img").hide();
        url = "url('../Images/ishop-P2P-Icons/" + this.metricparent + "/" + this.metrictype + ".svg?39')";
        $("." + sParentName).eq(0).show();
    }

    $("." + sParentName).eq(0).css("background-image", url);
};
dynamicUpdate.prototype.changebackgroundmultiple = function (sValue) {
    var sParentName = this.editedParentName(this.metricparent);
    var metricname = this.metricparent;
    var selectedmetrics = [];
    if (sParentName == "TripMission") {
        $('div.popup1 .bar-cntnt-metric-name').map(function (i, j) {
            if (i <= 3) {
                var obj = {};
                obj.name = $(this).text();
                obj.Value = parseFloat($(this).parent().find(".bar-cntnt-metric-val").text().split('%')[0]).toFixed(1);
                obj.change = parseFloat($(this).parent().find(".bar-cntnt-change-val").text().split('%')[0]).toFixed(1);
                obj.sig = $(this).parent().find(".bar-cntnt-change-val").css("color");
                selectedmetrics.push(obj);
            }
        });
    }
    else {
        $('.popup1 .bar-cntnt-chkboximg-active').map(function () {
            var obj = {};
            obj.name = $(this).attr("data-val");
            obj.Value = parseFloat($(this).parent().parent().find(".bar-cntnt-metric-val").text().split('%')[0]).toFixed(1);
            obj.change = parseFloat($(this).parent().parent().find(".bar-cntnt-change-val").text().split('%')[0]).toFixed(1);
            obj.sig = $(this).parent().parent().find(".bar-cntnt-change-val").css("color");
            selectedmetrics.push(obj);
        });
    }
    _.each(selectedmetrics, function (i, j) {
        //Updating images
        var imgName = replace_file_special_characters(i.name);
        var url = "url('../Images/ishop-P2P-Icons/" + metricname + "/" + imgName + ".svg?39')";
        $("." + sParentName + " .img" + (j + 1)).eq(0).css("background-image", url);
        //Updating Values
        if (sParentName != "TripMission") {
            $("." + sParentName + "Value" + (j + 1)).text(parseFloat(i.Value).toFixed(0) + "%");
            $("." + sParentName + "Name" + (j + 1)).text(i.name);
            $("." + sParentName + "Change" + (j + 1)).text(SetChangeVolume(i.change));
            $("." + sParentName + "Change" + (j + 1)).css("color", i.sig);
        }
    });
};
dynamicUpdate.prototype.UpdateData = function () {
    //Update Data
    var sParentName = this.editedParentName(this.metricparent);
    $("." + sParentName + "Value").text(parseFloat(this.Value).toFixed(0) + "%");
    if (sParentName != "TimeSpent") {
        $("." + sParentName + "Name").text(this.metrictype);
    }
    else {
        if (this.metrictype.toLocaleLowerCase().indexOf("or") > -1) {
            $("." + sParentName + "Name").text(this.metrictype.toUpperCase());
        }
        else {
            var min = this.metrictype.toLocaleLowerCase().split("min")[0].trim();
            var addminclass = "<div data-val='" + this.metrictype + "' class='timespent_val'>" + min + "</div><div class='timespent_min'>Min</div>";
            $("#Time_Spent .metric-item").children("ul").children("li").find(".all_sub_text").html(addminclass);
        }

    }

    $("." + sParentName + "Change").text(SetChangeVolume(this.Change));
    $("." + sParentName + "Change").css("color", this.Sig);
    if (this.metricparent == "Overall Satisfaction") {
        $(".OVERALL_SATISFACTION_CHRT").html("");
        createPieChart(".OVERALL_SATISFACTION_CHRT", "ballon1", this.Value == undefined ? 0 : this.Value, $(".OVERALL_SATISFACTION_CHRT").height(), "#f15f2e", "#dfdfdf");
    }
}
$(document).ready(function () {
    //$(".p_immediate_consumption .popup-icon-wraper").hide();
    $("#custom-base-customise").show();
    $("#GroupType .FilteriCon div").removeClass("grouptype_img").addClass("demograhicFitr_img").css("background-position", "-300px -159px");
    $(document).on("click", ".popup-icons", function () {
        $(".popup1 .metric-type-name").attr("popup-type", $(this).attr("popup-type"))
        var headerText = "";
        if ($(this).attr("data-val").toLowerCase() == "destination item") {
            headerText = "Destination Item";
        }
        else if ($(this).attr("data-val").toLowerCase() == "weekday or weekend") {
            headerText = "DAY OF THE WEEK";
        }
        else if ($(this).attr("data-val").toLowerCase() == "consideration") {
            headerText = "STORES CONSIDERED";
        }
        else if ($(this).attr("data-val").toLowerCase() == "immediate consumption") {
            headerText = "IC ITEMS IN BASKET - (CONSUMED WITHIN 1 HOUR)";
        }
        else if ($(this).attr("data-val").toLowerCase() == "order summary") {
            headerText = "ITEMS PURCHASED";
        }
        else {
            headerText = $(this).attr("data-val");
        }
        $(".popup1 .metric-type-name").html(headerText);
        updatepopupdata($(this));
        if ($(this).attr("options") == 'false') {
            $(".submit_popup_div").hide();
            $(".popup-hdr-stat-textdiv").hide();
        }
        else {
            $(".submit_popup_div").show();
            $(".popup-hdr-stat-textdiv").show();
        }
        $("#Translucent").css("z-index", "10010");
        $("#Translucent").show();
        $(".popup1").css("display", "inline-flex");

        var barHeight = 0;
        if ($(".bar-content-main-div").length > 0)
            barHeight = ($(".bar-content-main-div").height() * $(".bar-content-main-div").length);
        else if ($(".bar-content-main-divleft").length > 0)
            barHeight = ($(".bar-content-left").height() * ($(".bar-content-left").length) / 2);

        if (barHeight > $(window).height()) {
            $(".popup1").height($(window).height() - 100);
            if ($(".submit_popup_div").is(":visible") == true) {
                $(".popup-body").height($(".popup1").height() - 107);
            }
            else {
                $(".popup-body").height($(".popup1").height() - 50);
            }
        }
        else {
            if ($(".submit_popup_div").is(":visible") == true) {
                $(".popup1").height(barHeight + 107);
                $(".popup-body").height($(".popup1").height() - 107);
            }
            else {
                $(".popup1").height(barHeight + 50);
                $(".popup-body").height($(".popup1").height() - 50);
            }
        }



        if ($('div.popup1 .bar-cntnt-chkboximg-active').length > 1) {
            $(".clear_all").css("display", "block");
        }
        else {
            $(".clear_all").css("display", "none");
        }
    });
    $(document).on("click", ".popup-imageclse", function () {
        if ($(".popup1 .popup-header-div .metric-type-name").text().trim().toLocaleLowerCase() == "items purchased" || $(".popup1 .popup-header-div .popup-hdrtxt").text().trim().toLocaleLowerCase() == "destination item") {
            if ($('div.popup1 .bar-cntnt-chkboximg-active').length < 3) {
                showMessage("Please select 3 items.");
                return;
            }
        }
        $(".popup1").hide();
        $("#Translucent").hide();
    });
    $(document).on("click", ".inner_submit", function () {
        update_popup_metric_item_Id(metricTypeData);
        if (metricTypeDataSecondary.MetricData) {
            update_popup_metric_item_Id(metricTypeDataSecondary);
        }
        if ($(".popup1 .popup-header-div .metric-type-name").text().trim().toLocaleLowerCase() == "items purchased" || $(".popup1 .popup-header-div .popup-hdrtxt").text().trim().toLocaleLowerCase() == "destination item" || $(".popup1 .popup-header-div .popup-hdrtxt").text().trim().toLocaleLowerCase() == "trip mission") {
            if ($('div.popup1 .bar-cntnt-chkboximg-active').length < 3) {
                showMessage("Please select 3 items.");
                return;
            }
            if ($('div.popup1 .bar-cntnt-chkboximg-active').length >= 3) {
                var s = $(".popup1 .popup-header-div .metric-type-name").text();
                s = GetActualMetricName(s);
                var parent = titleCase(s) == "Order Summary" ? "Top Items" : (titleCase(s) == "Destination Item" ? "Destination Item" : titleCase(s));
                var child = "";
                var imgchange = new dynamicUpdate(parent, child);
                imgchange.changebackgroundmultiple(parseInt(sValue));
            }
            if ($(".popup1 .popup-header-div .metric-type-name").text().toLocaleLowerCase() == "trip mission") {
                var s = $(".popup1 .popup-header-div .metric-type-name").text();
                s = GetActualMetricName(s);
                var parent = titleCase(s) == "Order Summary" ? "Top Items" : titleCase(s);
                var child = "";
                var imgchange = new dynamicUpdate(parent, child);
                imgchange.changebackgroundmultiple(parseInt(sValue));
            }
        }
        else {
            if ($(".popup1 .barcntntradioactiveimg").length > 0) {
                var s = $(".popup1 .popup-header-div").text().trim();
                s = GetActualMetricName(s);
                var parent = titleCase(s);
                var child = $(".popup1 .barcntntradioactiveimg").attr("data-val");
                //var child = $(".popup1 .barcntntradioactiveimg").attr("data-val").split(' ').map(([h, ...t]) => h.toUpperCase() + t.join('').toLowerCase()).join(' ');//.replace('/','').replace(' 18+','');
                var value = parseFloat($(".popup1 .barcntntradioactiveimg").parent().parent().find(".bar-cntnt-metric-val").text().split("%")[0]).toFixed(1);
                var change = parseFloat($(".popup1 .barcntntradioactiveimg").parent().parent().find(".bar-cntnt-change-val").text().split("%")[0]).toFixed(1);
                //var change = $(".popup1 .barcntntradioactiveimg").parent().parent().find(".bar-cntnt-change-val").text().split("%")[0];
                var sig = $($(".popup1 .barcntntradioactiveimg").parent().parent().find(".bar-cntnt-change-val")).css("color");
                var imgchange = new dynamicUpdate(parent, child, value, change, sig);
                if (parent != "Planning")
                    imgchange.changebackground();
                else {

                    var sValue = $(".popup1 .barcntntradioactiveimg").parent().parent().eq(0).find(".bar-cntnt-metric-val").eq(0).text().split('%')[0];
                    imgchange.changebackgroundplanning(parseFloat(sValue).toFixed(1));
                }
                imgchange.UpdateData();
            }
            //To Get dynamicChanges
            var data_val_active = parent;
            active_metric_name = $(".popup1 .barcntntradioactiveimg").attr("data-val");
            var DataArr = retrnMetricList(data_val_active, MetricData);
            $.each(DataArr[0], function (i, v) {
                if (active_metric_name.toLocaleLowerCase() == DataArr[0][i].Metric.toLocaleLowerCase()) {
                    //set the changes name in DynamicChange variable
                    $.each(dynamicChanges, function (j, dd) {
                        if (dd.name.toLocaleLowerCase() == data_val_active.toLocaleLowerCase()) {
                            dynamicChanges[j].value = DataArr[0][i].Metric; return false;
                        }
                    });
                    return false;
                }
            });
        }

        $(".popup1").hide();
        $("#Translucent").hide();
    });
    $(document).on("click", ".bar-cntnt-radioimg", function () {
        if (!$(this).hasClass("barcntntradioactiveimg")) {
            $(".popup1 .bar-cntnt-radioimg").removeClass("barcntntradioactiveimg");
            $(this).addClass("barcntntradioactiveimg");
        }
    });
    $(document).on("click", ".bar-cntnt-chkboximg", function () {
        if (!$(this).hasClass("bar-cntnt-chkboximg-active")) {
            if ($(".popup1 .bar-cntnt-chkboximg-active").length < 3) {
                $(this).addClass("bar-cntnt-chkboximg-active");
            }
            else if ($(".popup1 .bar-cntnt-chkboximg-active").length = 3) {
                showMessage("YOU CAN MAKE UPTO 3 SELECTIONS.");
                return;
            }
        }
        else {
            $(this).removeClass("bar-cntnt-chkboximg-active");
        }

    });
    $(window).resize(function () {
        if (MetricData != null && MetricData.length > 0) {
            for (var i = 0; i < MetricData.length; i++) {
                var metrictype = MetricData[i].MetricType.toLowerCase();
                switch (metrictype) {
                    case "overall satisfaction":
                        {
                            update_overall_satisfaction(MetricData[i]);
                            break;
                        }
                    case "trip mission":
                        {
                            update_trip_mission(MetricData[i]);
                            break;
                        }
                    case "consideration":
                        {
                            update_considaration(MetricData[i]);
                            break;
                        }
                    case "immediate consumption":
                        {
                            update_beverage_consumption(MetricData[i]);
                            break;
                        }
                    case "purchased nartd":
                        {
                            update_purchased_summary(MetricData[i]);
                            break;
                        }
                }
            }
        }
        if (time_pie_chart_flag == 1) {
            $(".Planning_Type .p_pieChart").html('');
            $(".Planning_Type").show(); $(".Planning_Type .p_img").hide();
            if ($("#Planning_Type .metric").html().toLowerCase() == "spur of the moment")
                createTimePieChart(".Planning_Type .p_pieChart", "Pie4", timePieData, "#8dc63f", "transparent");
        }
    });
    $(document).on("click", ".popup-ppt", function () {
        ShowLoaderDB();
        var leftpanelData = Comparisonlist[0].Name + " | " + getTimeperiodEx().toUpperCase();//(TimePeriod.toLowerCase().indexOf("total") > -1 ? $(".timeType").val() : (TimePeriod.split('|').join(' ').trim()));
        Selected_Filters = [];
        if (Grouplist.length > 0) {
            leftpanelData += ", ";
            $(Grouplist).each(function (i, d) {
                leftpanelData = leftpanelData.concat(d.Name.trim() + ", ");
                Selected_Filters.push(d.Name);
            });
            leftpanelData = leftpanelData.slice(0, -2);
        }
        var statTest = "* Stat tested at " + StatPercent + "% CL against : " + (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
        var headerText = $(".popup1 .metric-type-name").html() == "Destination Item" ? "Destination Item" : $(".popup1 .metric-type-name").html();
        var OutputData = [];
        var OutputDataSecondary = [];
        OutputData = GetMetricTypeData(headerText).MetricData;
        var MetricType = GetMetricTypeData(headerText).MetricType;
        var MetricTypeSecondary = "";
        if (headerText == "Destination Item") {
            OutputDataSecondary = GetMetricTypeData("Destination Item-Nets").MetricData;
            MetricTypeSecondary = "Destination Item - Nets"
            MetricType = "Destination Item - Detail";
        }
        if (headerText == "ITEMS PURCHASED") {
            OutputDataSecondary = GetMetricTypeData("ORDER SUMMARY-Nets").MetricData;
            MetricTypeSecondary = "ITEMS PURCHASED - Nets"
            MetricType = "ITEMS PURCHASED - Detail";
        }
        if (headerText == "IC ITEMS IN BASKET - (CONSUMED WITHIN 1 HOUR)") {
            OutputDataSecondary = GetMetricTypeData("IMMEDIATE CONSUMPTION-Nets").MetricData;
            MetricTypeSecondary = "IC ITEMS - Nets"
            MetricType = "IC ITEMS - Detail";
        }
        if ($(".dashboard-content").is(":visible") == false) {
            return;
        }

        var pathToPurchas = new Object();
        pathToPurchas.TimePeriod = getTimeperiodEx().toUpperCase();
        pathToPurchas.Base = Comparisonlist[0].Name;
        pathToPurchas.CustomBase = (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
        pathToPurchas.Filters = Selected_Filters.join(", ");

        pathToPurchas.OutputData = OutputData;//JSON.stringify(OutputData); 
        pathToPurchas.OutputDataSecondary = OutputDataSecondary;
        pathToPurchas.LeftpanelData = leftpanelData;
        var cusAddFilters = [];
        if (Selected_StatTest == "CUSTOM BASE") {
            if (custombase_AddFilters.length > 0) {
                for (var i = 0; i < custombase_AddFilters.length; i++) {
                    cusAddFilters.push(custombase_AddFilters[i].Name);
                }
                //statTest = statTest + " | " + cusAddFilters.join(", ");
            }
            statTest += ", Filters: " + (cusAddFilters.length == 0 ? "NONE" : cusAddFilters.join(", "));
            //}
            cusAddFilters = [];
            if (custombase_Frequency.length > 0) {
                for (var i = 0; i < custombase_Frequency.length; i++) {
                    cusAddFilters.push(custombase_Frequency[i].Name);
                }
                statTest = statTest + " | " + cusAddFilters.join(", ");
            }
        }
        pathToPurchas.statTest = statTest;
        pathToPurchas.pptOrPdf = "pptx";
        pathToPurchas.DemofilterName = $(".popup1 .metric-type-name").attr("popup-type");
        pathToPurchas.DemoTitle = MetricType;
        pathToPurchas.DemoTitleSecondary = MetricTypeSecondary;
        pathToPurchas.ss = Number(sdata.SampleSize.replace(",", ""));// sdata.SampleSize;
        if (SelectedFrequencyList.length > 0) {
            pathToPurchas.ShopperFrequency = SelectedFrequencyList[0].Name;
            pathToPurchas.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
        }
        var P2P_SortText = "";
        if (P2P_Sort == "1") {
            P2P_SortText = "Size"
        }
        else {
            P2P_SortText = "Skew"
        }
        pathToPurchas.Sort = P2P_SortText;
        postBackData = "{pathToPurchas:" + JSON.stringify(pathToPurchas) + "}";
        jQuery.ajax({
            type: "POST",
            url: $("#URLDashboardPopUpExp").val(),// + "/PopupExportDashboard",
            data: postBackData,
            contentType: "application/json",
            success: function (response) {
                if (response != "error")
                    window.location.href = $("#URLDashboardDownloadExpPPT").val() + "/?path=" + response;
                else {
                    showMessage("Some error occured !");
                }
                HideLoaderDB();
            },
            error: function (xhr, status, error) {
                HideLoaderDB();
                showMessage(xhr.responseText)
            }
        });
    });
    $(document).on("click", ".popup-hdrpdf", function () {
        ShowLoaderDB();
        var leftpanelData = Comparisonlist[0].Name + " | " + getTimeperiodEx().toUpperCase();// (TimePeriod.toLowerCase().indexOf("total") > -1 ? $(".timeType").val() : (TimePeriod.split('|').join(' ').trim()));
        Selected_Filters = [];
        if (Grouplist.length > 0) {
            leftpanelData += ", ";
            $(Grouplist).each(function (i, d) {
                leftpanelData = leftpanelData.concat(d.Name.trim() + ", ");
                Selected_Filters.push(d.Name);
            });
            leftpanelData = leftpanelData.slice(0, -2);
        }
        var statTest = "* Stat tested at " + StatPercent + "% CL against : " + (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
        var headerText = $(".popup1 .metric-type-name").html() == "Destination Item" ? "Destination Item" : $(".popup1 .metric-type-name").html();
        var OutputData = [];
        var OutputDataSecondary = [];
        OutputData = GetMetricTypeData(headerText).MetricData;
        var MetricType = GetMetricTypeData(headerText).MetricType;
        var MetricTypeSecondary = "";
        if (headerText == "Destination Item") {
            OutputDataSecondary = GetMetricTypeData("Destination Item-Nets").MetricData;
            MetricTypeSecondary = "Destination Item - Nets"
            MetricType = "Destination Item - Detail";
        }
        if (headerText == "ITEMS PURCHASED") {
            OutputDataSecondary = GetMetricTypeData("ORDER SUMMARY-Nets").MetricData;
            MetricTypeSecondary = "ITEMS PURCHASED - Nets"
            MetricType = "ITEMS PURCHASED - Detail";
        }
        if (headerText == "IC ITEMS IN BASKET - (CONSUMED WITHIN 1 HOUR)") {
            OutputDataSecondary = GetMetricTypeData("IMMEDIATE CONSUMPTION-Nets").MetricData;
            MetricTypeSecondary = "IC ITEMS - Nets"
            MetricType = "IC ITEMS - Detail";
        }
        if ($(".dashboard-content").is(":visible") == false) {
            return;
        }

        var pathToPurchas = new Object();
        pathToPurchas.TimePeriod = getTimeperiodEx().toUpperCase();
        pathToPurchas.Base = Comparisonlist[0].Name;
        pathToPurchas.CustomBase = (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
        pathToPurchas.Filters = Selected_Filters.join(", ");

        pathToPurchas.OutputData = OutputData;//JSON.stringify(OutputData); 
        pathToPurchas.OutputDataSecondary = OutputDataSecondary;
        pathToPurchas.LeftpanelData = leftpanelData;

        var cusAddFilters = [];
        if (Selected_StatTest == "CUSTOM BASE") {
            if (custombase_AddFilters.length > 0) {
                for (var i = 0; i < custombase_AddFilters.length; i++) {
                    cusAddFilters.push(custombase_AddFilters[i].Name);
                }
                //statTest = statTest + " | " + cusAddFilters.join(", ");
            }
            statTest += ", Filters: " + (cusAddFilters.length == 0 ? "NONE" : cusAddFilters.join(", "));

            cusAddFilters = [];
            if (custombase_Frequency.length > 0) {
                for (var i = 0; i < custombase_Frequency.length; i++) {
                    cusAddFilters.push(custombase_Frequency[i].Name);
                }
                statTest = statTest + " | " + cusAddFilters.join(", ");
            }
        }
        pathToPurchas.statTest = statTest;
        pathToPurchas.pptOrPdf = "pdf";
        pathToPurchas.DemofilterName = $(".popup1 .metric-type-name").attr("popup-type");
        pathToPurchas.DemoTitle = MetricType;
        pathToPurchas.DemoTitleSecondary = MetricTypeSecondary;
        pathToPurchas.ss = Number(sdata.SampleSize.replace(",", ""));// sdata.SampleSize;
        if (SelectedFrequencyList.length > 0) {
            pathToPurchas.ShopperFrequency = SelectedFrequencyList[0].Name;
            pathToPurchas.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
        }
        var P2P_SortText = "";
        if (P2P_Sort == "1") {
            P2P_SortText = "Size"
        }
        else {
            P2P_SortText = "Skew"
        }
        pathToPurchas.Sort = P2P_SortText;
        postBackData = "{pathToPurchas:" + JSON.stringify(pathToPurchas) + "}";
        jQuery.ajax({
            type: "POST",
            url: $("#URLDashboardPopUpExp").val(),// + "/PopupExportDashboard",
            data: postBackData,
            contentType: "application/json",
            success: function (response) {
                if (response != "error")
                    window.location.href = $("#URLDashboardDownloadExpPDF").val() + "/?path=" + response;
                else {
                    showMessage("Some error occured !");
                }
                HideLoaderDB();
                //$(".loader,.transparentBGHigh").hide();
            },
            error: function (xhr, status, error) {
                //$(".transparentBGHigh").hide(); 
                HideLoaderDB();
                showMessage(xhr.responseText)
            }
        });
    });
});
function titleCase(str) {
    return str
        .toLowerCase()
        .split(' ')
        .map(function (word) {
            return word[0].toUpperCase() + word.substr(1);
        })
        .join(' ');
}

/**********PPT Download For Dashboard ************/

$(document).on("click", ".exporttoppt-logo", function () {
    if ($(".dashboard-content").is(":visible") == false) {
        return;
    }
    ShowLoader();
    var leftpanelData = Comparisonlist[0].Name + "|";
    leftpanelData += getTimeperiodEx().toUpperCase();//$(".timeType").val();
    Selected_Filters = [];
    if (Grouplist.length > 0) {
        leftpanelData += ", ";
        for (var i = 0; i < Grouplist.length; i++) {
            leftpanelData += Grouplist[i].Name + ", ";
            Selected_Filters.push(Grouplist[i].Name);
        }
        leftpanelData = leftpanelData.slice(0, -2);
    }
   
    var statTest = "* Stat tested at " + StatPercent + "% CL against : " + (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name.toUpperCase());
    if (Selected_StatTest == "CUSTOM BASE") {
        var cusAddFilters = [];
        if (custombase_AddFilters.length > 0) {
            for (var i = 0; i < custombase_AddFilters.length; i++) {
                cusAddFilters.push(custombase_AddFilters[i].Name);
            }
            //statTest = statTest + " | " + cusAddFilters.join(", ");
        }
        statTest += ", Filters: " + (cusAddFilters.length == 0 ? "NONE" : cusAddFilters.join(", "));
        //}
        cusAddFilters = [];
        if (custombase_Frequency.length > 0) {
            for (var i = 0; i < custombase_Frequency.length; i++) {
                cusAddFilters.push(custombase_Frequency[i].Name);
            }
            statTest = statTest + " | " + cusAddFilters.join(", ");
        }
    }
    //MetricData, "NoOfRoads": , "changedData": dynamicChanges, "LeftpanelData": leftpanelData, "statTest": statTest, "pptOrPdf": "ppt", "ss":  Number(sdata.SampleSize.replace(",",""))
    var p2PDashboardData = new Object();
    p2PDashboardData.TimePeriod = getTimeperiodEx().toUpperCase();
    p2PDashboardData.Base = Comparisonlist[0].Name;
    p2PDashboardData.CustomBase = (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
    p2PDashboardData.Filters = Selected_Filters.join(", ");

    p2PDashboardData.OutputData = MetricData;
    p2PDashboardData.NoOfRoads = ($("#p2p-dsbrd-BG object").attr("data")).indexOf("BG2") == -1 ? "4" : "3";
    p2PDashboardData.changedData = dynamicChanges;
    p2PDashboardData.LeftpanelData = leftpanelData;
    p2PDashboardData.statTest = statTest;
    p2PDashboardData.pptOrPdf = "pptx";
    p2PDashboardData.ss = Number(sdata.SampleSize.replace(",", ""));
    if (SelectedFrequencyList.length > 0) {
        p2PDashboardData.ShopperFrequency = SelectedFrequencyList[0].Name;
        p2PDashboardData.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }
    var P2P_SortText = "";
    if (P2P_Sort == "1") {
        P2P_SortText = "Size"
    }
    else {
        P2P_SortText = "Skew"
    }
    p2PDashboardData.Sort = P2P_SortText;
    jQuery.ajax({
        type: "POST",
        url: $("#URLDashboardFullExp").val(),// + "/ExportToFullDashboardPPT",
        data: "{p2PDashboardData:" + JSON.stringify(p2PDashboardData) + "}",
        contentType: "application/json",
        success: function (response) {
            if (response != "error")
                window.location.href = $("#URLDashboardDownloadExpPPT").val() + "/?path=" + response;
            else {
                showMessage("Some error occured !");
            }
            HideLoader();
        },
        error: function (xhr, status, error) {
            HideLoader();
            showMessage(xhr.responseText)
        }
    });

});

/**********PDF Download FOr Dashboard ***********/

$(document).on("click", ".exporttopdf-logo", function () {
    if ($(".dashboard-content").is(":visible") == false) {
        return;
    }

    ShowLoader();
    var leftpanelData = Comparisonlist[0].Name + "|";
    leftpanelData += getTimeperiodEx().toUpperCase();//$(".timeType").val();
    Selected_Filters = [];
    if (Grouplist.length > 0) {
        leftpanelData += ", ";
        for (var i = 0; i < Grouplist.length; i++) {
            leftpanelData += Grouplist[i].Name + ", ";
            Selected_Filters.push(Grouplist[i].Name);
        }
        leftpanelData = leftpanelData.slice(0, -2);
    }

    var statTest = "* Stat tested at " + StatPercent + "% CL against : " + (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name.toUpperCase());
    if (Selected_StatTest == "CUSTOM BASE") {
        var cusAddFilters = [];
        if (custombase_AddFilters.length > 0) {
            for (var i = 0; i < custombase_AddFilters.length; i++) {
                cusAddFilters.push(custombase_AddFilters[i].Name);
            }
            //statTest = statTest + " | " + cusAddFilters.join(", ");
        }
        statTest += ", Filters: " + (cusAddFilters.length == 0 ? "NONE" : cusAddFilters.join(", "));

        cusAddFilters = [];
        if (custombase_Frequency.length > 0) {
            for (var i = 0; i < custombase_Frequency.length; i++) {
                cusAddFilters.push(custombase_Frequency[i].Name);
            }
            statTest = statTest + " | " + cusAddFilters.join(", ");
        }
    }
    var p2PDashboardData = new Object();
    p2PDashboardData.TimePeriod = getTimeperiodEx().toUpperCase();
    p2PDashboardData.Base = Comparisonlist[0].Name;
    p2PDashboardData.CustomBase = (Selected_StatTest != "CUSTOM BASE" ? Selected_StatTest.toUpperCase() : CustomBase[0].Name);
    p2PDashboardData.Filters = Selected_Filters.join(", ");

    p2PDashboardData.OutputData = MetricData;
    p2PDashboardData.NoOfRoads = ($("#p2p-dsbrd-BG object").attr("data")).indexOf("BG2") == -1 ? "4" : "3";
    p2PDashboardData.changedData = dynamicChanges;
    p2PDashboardData.LeftpanelData = leftpanelData;
    p2PDashboardData.statTest = statTest;
    p2PDashboardData.pptOrPdf = "pdf";
    p2PDashboardData.ss = Number(sdata.SampleSize.replace(",", ""));
    if (SelectedFrequencyList.length > 0) {
        p2PDashboardData.ShopperFrequency = SelectedFrequencyList[0].Name;
        p2PDashboardData.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }
    var P2P_SortText = "";
    if (P2P_Sort == "1") {
        P2P_SortText = "Size"
    }
    else {
        P2P_SortText = "Skew"
    }
    p2PDashboardData.Sort = P2P_SortText;
    jQuery.ajax({
        type: "POST",
        url: $("#URLDashboardFullExp").val(),//+ "/ExportToFullDashboardPPT",
        data: "{p2PDashboardData:" + JSON.stringify(p2PDashboardData) + "}",
        contentType: "application/json",
        success: function (response) {
            if (response != "error")
                window.location.href = $("#URLDashboardDownloadExpPDF").val() + "/?path=" + response;
            else {
                showMessage("Some error occured !");
            }
            HideLoader();
        },
        error: function (xhr, status, error) {
            HideLoader();
            showMessage(xhr.responseText)
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

//To return the MetricList
var retrnMetricList = function (demofiltername, MetricData) {
    var outputlist = [];

    $.each(MetricData, function (i, v) {
        if (v.MetricType.toLocaleLowerCase() == demofiltername.toLocaleLowerCase())
            outputlist.push(v.MetricData);
    });

    return outputlist;

}
//save dashboard selection
function SaveDashboardSelection() {
    if (!Validate_CompareRetailers_Charts()) {
        return false;
    }
    var pathToPurchaseParams = new Object();
    if (SelectedFrequencyList.length > 0) {
        pathToPurchaseParams.ShopperFrequency = SelectedFrequencyList[0].Name;
        pathToPurchaseParams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }

    pathToPurchaseParams.StatTest = Selected_StatTest;
    pathToPurchaseParams.Sigtype_UniqueId = Sigtype_Id;
    pathToPurchaseParams.TimePeriod = TimePeriod;
    pathToPurchaseParams.TimePeriod_UniqueId = TimePeriod_UniqueId;
    pathToPurchaseParams.TimePeriodShortName = $(".timeType").val();

    if (CustomBase.length > 0) {
        pathToPurchaseParams.CustomBase_ShortName = CustomBase[0].Name;
        pathToPurchaseParams.CustomBase_UniqueId = CustomBase[0].UniqueId;
    }

    if (Comparisonlist.length > 0) {
        pathToPurchaseParams.Comparison_ShortNames = Comparisonlist[0].Name;
        pathToPurchaseParams.Comparison_UniqueIds = Comparisonlist[0].UniqueId;
    }

    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_UniqueId = [];
    for (var i = 0; i < Grouplist.length; i++) {
        Advanced_Filters_DBNames.push(Grouplist[i].DBName);
        Advanced_Filters_ShortNames.push(Grouplist[i].Name);
        Advanced_Filters_UniqueId.push(Grouplist[i].UniqueId);
    }
    pathToPurchaseParams.ShopperSegment = Advanced_Filters_ShortNames.join("|");
    pathToPurchaseParams.FilterShortname = Advanced_Filters_ShortNames.join("|");
    pathToPurchaseParams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");

    //add custom base dual filters   
    CustomBaseAdvancedFilters = [];
    CustomBaseAdvancedFilters_UniqueId = [];
    for (var i = 0; i < custombase_AddFilters.length; i++) {
        CustomBaseAdvancedFilters.push(custombase_AddFilters[i].Name);
        CustomBaseAdvancedFilters_UniqueId.push(custombase_AddFilters[i].UniqueId);
    }
    pathToPurchaseParams.CustomBaseAdvancedFilters = CustomBaseAdvancedFilters.join("|");
    pathToPurchaseParams.CustomBaseAdvancedFilters_UniqueId = CustomBaseAdvancedFilters_UniqueId.join("|");

    //add custom base frequency dual filters   
    CustomBaseFrequencyFilters = [];
    CustomBaseFrequency_UniqueId = [];
    for (var i = 0; i < custombase_Frequency.length; i++) {
        CustomBaseFrequencyFilters.push(custombase_Frequency[i].Name);
        CustomBaseFrequency_UniqueId.push(custombase_Frequency[i].UniqueId);
    } 
    pathToPurchaseParams.CustomBaseShopperFrequency = CustomBaseFrequencyFilters.join("|");
    pathToPurchaseParams.CustomBaseShopperFrequency_UniqueId = CustomBaseFrequency_UniqueId.join("|");

    pathToPurchaseParams.Sort = P2P_Sort;
    postBackData = "{pathToPurchaseParams:" + JSON.stringify(pathToPurchaseParams) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLDashboardSaveUserSelection").val(),// + "/SaveUserSelection",
        data: postBackData,
        contentType: "application/json",
        success: function (data) {
            if (!isAuthenticated(data))
                return false;
            SaveFlag = 1;
            prepareContentArea();
        },
        error: function (xhr, status, error) {
            showMessage(xhr.responseText)
        }
    });
}
function replace_file_special_characters(filename) {
    if (filename != null && filename != '' && filename != undefined)
        filename = filename.replace(/[&/\\#,+()$~%.':*?<>{}]/g, '-');

    return filename;
}

//ClearAll functionality
function clearAll() {
    if ($('div.popup1 .bar-cntnt-chkboximg-active').length >= 1) {
        $('div.popup1 .bar-content-header *').removeClass("bar-cntnt-chkboximg-active");
    }
}
