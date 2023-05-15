﻿/// <reference path="../Layout/Layout.js" />
/// <reference path="../Layout/RightPanelFilter.js" />
/// <reference path="../jquery-1.8.2.min.js" />


//Written by Nagaraju D
$(document).ready(function () {
    ClearChartReports();
});
function prepareContentArea() {   
    $(".trendChartMain").css("height", "100%");

    if (!Validate_CompareBeverages() || !Validate_Measures_Charts()) {
        return false;
    }
    $("#RightPanelPartial").show();
    if (TabType.toString() == "") {
        TabType = "trips";
        $("#RightPanelPartial .rgt-cntrl-frequency-Conatiner ul div[name='TOTAL VISITS']").trigger("click");
    }
    sRemovedLegendPosition = [];
    SetDefaultCustomBase();
    ChartTileName = "demographic profiling";
    if (ChartType.toLowerCase() == "table") {
        var _profilerparams = new Object();
        //_profilerparams.TabName = tabname;
        _profilerparams.Tab_Id_mapping = "true";
        // _profilerparams.Tab_Id_mapping = "false";
        //if (SelectedFrequencyList.length > 0) {
        //    _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
        //    _profilerparams.ShopperFrequencyShortName = SelectedFrequencyList[0].Name;
        //    _profilerparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
        //}

        if (TabType.toLocaleLowerCase() == "trips") {
            Channels_DBNames = [];
            Channels_UniqueId = [];
            if (selectedChannels.length > 0) {
                for (var i = 0; i < selectedChannels.length; i++) {
                    Channels_DBNames.push(selectedChannels[i].Name);
                    Channels_UniqueId.push(selectedChannels[i].UniqueId);
                }
                _profilerparams.ShopperFrequency = Channels_DBNames.join("|");
                if (_profilerparams.ShopperFrequency != '' && _profilerparams.ShopperFrequency.toUpperCase() == "TOTAL")
                    _profilerparams.ShopperFrequency_UniqueId = "";
                else
                    _profilerparams.ShopperFrequency_UniqueId = Channels_UniqueId.join("|");
            }
        }
        else {
            if (SelectedFrequencyList.length > 0) {
                _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
                _profilerparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
            }
        }
        _profilerparams.ViewType = "BEVERAGES";
        _profilerparams.ActiveTab = GetCurrentSPName();//"usp_ProfilerAcrossRetailerShopper";//
        _profilerparams.SelectedStatTest = Selected_StatTest;
        _profilerparams.Sigtype_UniqueId = Sigtype_Id;
        _profilerparams.TimePeriod = TimePeriod;
        _profilerparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
        _profilerparams.ShortTimePeriod = $(".timeType").val();
        _profilerparams.Comparison_DBNames = [];
        _profilerparams.Comparison_ShortNames = [];
        _profilerparams.Comparison_UniqueIds = [];
        var sCompList = [];
        //For binding table
        var sCompXValues = [];
        //custom base
        if (CustomBase.length > 0) {
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;
            else
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;

            _profilerparams.CustomBase_ShortName = CustomBase[0].Name;
            _profilerparams.CustomBase_UniqueId = CustomBase[0].UniqueId;
        }
        //----------->

        for (var i = 0; i < ComparisonBevlist.length; i++) {
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.Comparison_DBNames.push(ComparisonBevlist[i].Name);
            else
                _profilerparams.Comparison_DBNames.push(ComparisonBevlist[i].Name);

            _profilerparams.Comparison_ShortNames.push(ComparisonBevlist[i].Name);
            _profilerparams.Comparison_UniqueIds.push(ComparisonBevlist[i].UniqueId);
            if (i != 0) {
                if (TabType.toLocaleLowerCase() == "trips") {
                    sCompList.push(ComparisonBevlist[i].Name);
                    sCompXValues.push(ComparisonBevlist[i].Name.split('|')[1]);
                }
                else {
                    sCompList.push(ComparisonBevlist[i].Name);
                    sCompXValues.push(ComparisonBevlist[i].Name.split('|')[1]);
                }
            }
        }

        _profilerparams.Benchmark = _profilerparams.Comparison_DBNames[0];
        _profilerparams.BCFullNames = _profilerparams.Comparison_ShortNames;
        _profilerparams.Comparisonlist = sCompList.join("|");
        Advanced_Filters_DBNames = [];
        Advanced_Filters_ShortNames = [];
        Advanced_Filters = [];
        Advanced_Filters_UniqueId = [];
        //Guest advanced filters
        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].Name);
            Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].parentName + "|" + SelectedDempgraphicList[i].Name);
            Advanced_Filters.push(SelectedDempgraphicList[i].parentName + "|" + SelectedDempgraphicList[i].Name);
            Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
        }
        //Visits advanced filters
        for (var i = 0; i < SelectedAdvFilterList.length; i++) {
            Advanced_Filters_DBNames.push(SelectedAdvFilterList[i].Name);
            Advanced_Filters_ShortNames.push(SelectedAdvFilterList[i].parentName + "|" + SelectedAdvFilterList[i].Name);
            Advanced_Filters.push(SelectedAdvFilterList[i].parentName + "|" + SelectedAdvFilterList[i].Name);
            Advanced_Filters_UniqueId.push(SelectedAdvFilterList[i].UniqueId);
        }
        _profilerparams.ShopperSegment = Advanced_Filters_DBNames.join("|");
        _profilerparams.FilterShortnames = Advanced_Filters_ShortNames.join("|");
        _profilerparams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");
        //_profilerparams.ShopperFrequency = "";
        _profilerparams.Filters = "";//Advanced_Filters.join("|");
        //if(sVisitsOrGuests == "2")
        //    _profilerparams.ModuleBlock = "TREND";
        //else
        //    _profilerparams.ModuleBlock = "PIT";

        _profilerparams.ModuleBlock = "AcrossShopper";
        _profilerparams.Metric = Measurelist[0].Name;
        _profilerparams.Metric_UniqueId = Measurelist[0].MetricId;
        var sMetricNames = "";
        var sMetric = [];

        for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
            sMetricNames += Measurelist[0].metriclist[i].Name + "|";
            sMetric.push(Measurelist[0].metriclist[i].Id);
        }
        sMetricNames = sMetricNames.substring(0, sMetricNames.length - 1);
        _profilerparams.selectedMetrics = sMetric;
        _profilerparams.ChartXValues = _profilerparams.Comparison_ShortNames;
        _profilerparams.ChartXValues_UniqueId = _profilerparams.Comparison_UniqueIds;
        _profilerparams.ChartType = ChartType;

        _profilerparams.SelectedMetrics = sMetricNames;
        _profilerparams.selectedMetrics = sMetric;
        _profilerparams.MetricShortName = Measurelist[0].parentName.toUpperCase();
        _profilerparams.SelectedMetricsIds = sMetric.join('|');
        postBackData = "{_profilerparams:" + JSON.stringify(_profilerparams) + "}";
        jQuery.ajax({
            type: "POST",
            url: $("#URLCharts").val() + "/GenerateTable",
            async: true,
            data: postBackData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (!isAuthenticated(data))
                    return false;

                //RetailnChartReports();
                // ChartModuleData = data;
                $("#ToShowChart").show();
                //$('#spChartLegend').show();
                $("#ToShowChart").css("height", "65%");
                $("#chart-title").text("");
                if (ChartType == "Table" || ChartType == "") {
                    $("#chart-title").hide();
                    $(".ChartDivArea").css("padding-top", "30px");
                }
                else {
                    $("#chart-title").show();
                    $(".ChartDivArea").css("padding-top", "0px");
                }
                $(".showChartMain").hide();
                $(".showChartMain").getNiceScroll().resize().hide();
                var sImageClassName = "";
                $.each($("#chart-list div"), function (i, v) {

                    if (ChartType.toString() == v.attributes[1].value)
                        sImageClassName = ChartImageNew_Position(v.attributes[1].value);
                    else
                        sImageClassName = ChartImagePosition(v.attributes[1].value);
                    $("#chart-list div[chart-name='" + v.attributes[1].value + "']").css('background-position', sImageClassName);
                });
                if (ChartType.toLocaleLowerCase() == "table") {
                    $("#chart-list div[chart-name='" + ChartType.toLocaleLowerCase() + "']").css('background-position', '-178px -405px');
                    identifier = 0;
                    $(".ChartDivArea").html("");

                    //$(".ChartDivArea").css("height", "auto");
                    $(".ChartDivArea").css("width", "100%");

                    $("#spChartLegend").empty()
                    $(".trendChartMain").empty();
                    $(".trendChartMain").hide();
                    //$(".ChartDivArea").html(data);
                    $(".ChartDivArea").show();
                    $("#UpdateProgress").hide();
                    $(".TranslucentDiv").hide();
                    $(".tableheader").css("width", "100%");
                    //CheckExportList();
                    isPanelChange = "false";
                    //$(".tableheader tr").each(function () {
                    //    $(this).children("td").each(function (i) {
                    //        if (i == 0) {
                    //            $(this).css("padding-left", "10px");
                    //        }
                    //    });
                    //});
                    // GoToChartFunctionForLineChartTwo(data, identifier);
                    //GetTable();
                }
                $(".ChartDivArea").css("width", "100%");
                $(".ChartDivArea table").css("width", "100%");
                //$(".ChartDivArea").getNiceScroll().resize().hide();
                //SetScroll($(".ChartDivArea"), "#393939", -11, 0, 0, 0, 8);
                PrepareTable(data);
            },
            error: function (xhr, status, error) {
                //NoDataAvailable();
                GoToErrorPage();
            }
        });
    }
    else {
        var _profilerparams = new Object();
        //_profilerparams.TabName = tabname;
        //if (SelectedFrequencyList.length > 0) {
        //    _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
        //    _profilerparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
        //}
        if (TabType.toLocaleLowerCase() == "trips") {
            Channels_DBNames = [];
            Channels_UniqueId = [];
            if (selectedChannels.length > 0) {
                for (var i = 0; i < selectedChannels.length; i++) {
                    Channels_DBNames.push(selectedChannels[i].Name);
                    Channels_UniqueId.push(selectedChannels[i].UniqueId);
                }
                _profilerparams.ShopperFrequency = Channels_DBNames.join("|");
                if (_profilerparams.ShopperFrequency != '' && _profilerparams.ShopperFrequency.toUpperCase() == "TOTAL")
                    _profilerparams.ShopperFrequency_UniqueId = "";
                else
                    _profilerparams.ShopperFrequency_UniqueId = Channels_UniqueId.join("|");
            }
        }
        else {
            if (SelectedFrequencyList.length > 0) {
                _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
                _profilerparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
            }
        }
        _profilerparams.Tab_Id_mapping = "true";
        //_profilerparams.Tab_Id_mapping = "false";
        _profilerparams.TabName = tabname;
        _profilerparams.TabIndexId = Measurelist[0].filtertypeid;
        //_profilerparams.Tab_Id_mapping = Tab_Id_mapping;

        //custom base
        if (CustomBase.length > 0) {
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;
            else
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;

            _profilerparams.CustomBase_ShortName = CustomBase[0].Name;
            _profilerparams.CustomBase_UniqueId = CustomBase[0].UniqueId;
        }
        //----------->
        _profilerparams.ActiveTab = GetCurrentSPName(); //usp_ProfilerAcrossRetailerShopper
        _profilerparams.SelectedStatTest = Selected_StatTest;
        _profilerparams.Sigtype_UniqueId = Sigtype_Id;
        _profilerparams.TimePeriod = TimePeriod;
        _profilerparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
        _profilerparams.ShortTimePeriod = $(".timeType").val();
        _profilerparams.Comparison_DBNames = [];
        _profilerparams.Comparison_ShortNames = [];
        _profilerparams.Comparison_UniqueIds = [];


        for (var i = 0; i < ComparisonBevlist.length; i++) {

            if (TabType.toLocaleLowerCase() == "trips" || $("#guest-visit-toggle").is(":checked") == false)
                _profilerparams.Comparison_DBNames.push(ComparisonBevlist[i].Name);
            else
                _profilerparams.Comparison_DBNames.push(ComparisonBevlist[i].Name);

            _profilerparams.Comparison_ShortNames.push(ComparisonBevlist[i].Name);
            _profilerparams.Comparison_UniqueIds.push(ComparisonBevlist[i].UniqueId);
        }
        Advanced_Filters_DBNames = [];
        Advanced_Filters_ShortNames = [];
        Advanced_Filters_UniqueId = [];
        //Guest advanced filters
        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].Name);
            Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].parentName + "|" + SelectedDempgraphicList[i].Name);
            Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
        }
        //Visits advanced filters
        for (var i = 0; i < SelectedAdvFilterList.length; i++) {
            Advanced_Filters_DBNames.push(SelectedAdvFilterList[i].Name);
            Advanced_Filters_ShortNames.push(SelectedAdvFilterList[i].parentName + "|" + SelectedAdvFilterList[i].Name);
            Advanced_Filters_UniqueId.push(SelectedAdvFilterList[i].UniqueId);
        }
        _profilerparams.Filters = Advanced_Filters_ShortNames.join("|");
        _profilerparams.ShopperSegment = Advanced_Filters_DBNames.join("|");
        _profilerparams.FilterShortnames = Advanced_Filters_ShortNames.join("|");
        _profilerparams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");
        _profilerparams.Metric = Measurelist[0].Name;
        _profilerparams.Metric_UniqueId = Measurelist[0].MetricId;
        _profilerparams.Filtertypeid_UniqueId = Measurelist[0].filtertypeid.toString();
        //_profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
        var sMetricNames = "";
        var sMetric = [];
        //for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
        //    sMetricNames += Measurelist[0].metriclist[i].Name + "|";
        //    sMetric.push(Measurelist[0].metriclist[i].Name);

        //}

        //if (Measurelist[0].metriclist.length == 0) {
        //    showMessage("Please select Measure");
        //    return false;
        //}
        for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
            sMetricNames += Measurelist[0].metriclist[i].Id + "|";
            sMetric.push(Measurelist[0].metriclist[i].Name);

        }
        sMetricNames = sMetricNames.substring(0, sMetricNames.length - 1)
        _profilerparams.ChartXValues = _profilerparams.Comparison_ShortNames;
        _profilerparams.ChartXValues_UniqueId = _profilerparams.Comparison_UniqueIds;
        _profilerparams.ChartType = ChartType;

        _profilerparams.SelectedMetrics = sMetric.join("|");//"Male|Female";
        _profilerparams.SelectedMetricsIds = sMetricNames;
        postBackData = "{_profilerparams:" + JSON.stringify(_profilerparams) + "}";
        jQuery.ajax({
            type: "POST",
            url: $("#URLCharts").val() + "/GetChartData",
            async: true,
            data: postBackData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (!isAuthenticated(data))
                    return false;

                $("#ToShowChart").show();
                //$('#spChartLegend').show();
                $("#ToShowChart").css("height", "65%");
                if (data == null || data == "")
                    NoDataAvailable();
                $("#chart-title").show();
                if (ChartType == "Table" || ChartType == "") {
                    $("#chart-title").hide();
                    $(".ChartDivArea").css("padding-top", "30px");
                }
                else {
                    $("#chart-title").show();
                    $(".ChartDivArea").css("padding-top", "0px");
                }
                RetailnChartReports();
                ChartModuleData = data;
                $("#chart-list").css("background-image", " url(../../Images/Coke Dine_Sprite_7.svg)");
                $("#chart-list").css("background-position", " 548px -457px");
                $(".ChartDivArea").hide();
                $("#ToShowChart").show();
                $(".showChartMain").show();
                $(".ChartDivArea").getNiceScroll().resize().hide();
                if (data.ValueData.length == 0) {
                    $("#idtrendChartMain1").hide();
                    $("#idtrendChartMain2").hide();
                    $("#idtrendChartMain3").hide();
                    $("#idtrendChartMain4").hide();
                    $("#idtrendChartMain11").hide();
                    $("#idtrendChartMain00").show();
                    $("#chart-title").html(Measurelist[0].parentName.toUpperCase());
                    if (ChartType == "Table" || ChartType == "") {
                        $("#chart-title").hide();
                        $(".ChartDivArea").css("padding-top", "30px");
                    }
                    else {
                        $("#chart-title").show();
                        $(".ChartDivArea").css("padding-top", "0px");
                    }
                    $("#idtrendChartMain00").html("NO DATA AVAILABLE").css("color", "black").css("font-size", "13px");
                    $("#spChartLegend").hide();
                    return;
                }
                $("#spChartLegend").show();
                var sImageClassName = "";
                $.each($("#chart-list div"), function (i, v) {
                    if (ChartType.toString() == v.attributes[1].value)
                        sImageClassName = ChartImageNew_Position(v.attributes[1].value);
                    else
                        sImageClassName = ChartImagePosition(v.attributes[1].value);
                    $("#chart-list div[chart-name='" + v.attributes[1].value + "']").css('background-position', sImageClassName);
                });
                $(".ChartDivArea").html("");
                if (ChartType.toLocaleLowerCase() == "clustered column") {
                    identifier = 1;
                    GoToChartFunctionForColumnChartOne(data, identifier);
                }
                else if (ChartType.toLocaleLowerCase() == "clustered bar") {                   
                    identifier = 1;                    
                    GoToChartFunctionForColumnChartOne_NewForStackedBar(data, identifier);
                }
                else if (ChartType.toLocaleLowerCase() == "bar with change") {                   
                    identifier = 1;
                    GoToChartFunctionForColumnChartOne_NewForBar_with_change(data, identifier);
                }
                else if (ChartType.toLocaleLowerCase() == "stacked column") {                   
                    identifier = 0;
                    columnChart_Stacked(data, identifier);
                }
                else if (ChartType.toLocaleLowerCase() == "stacked bar") {                    
                    identifier = 0;
                    var MoreLessFact = 4;
                    if ($(".adv-fltr-showhide-txt").text() == "SHOW MORE") {
                        MoreLessFact = 6;
                    }
                    BarChart_Stacked(data, identifier, MoreLessFact);
                }
                else if (ChartType.toLocaleLowerCase() == "line") {                   
                    identifier = 0;                    
                    GoToChartFunctionForLineChartOne(data, identifier);

                }
                else if (ChartType.toLocaleLowerCase() == "pyramid") {                   
                    identifier = 0;
                    Plot_Pyramid_Chart(data, identifier);

                }
                else if (ChartType.toLocaleLowerCase() == "pyramid with change") {                    
                    identifier = 0;                   
                    Plot_Pyramid_Chart_With_Change(data, identifier);

                }
                $("#chart-title").html(Measurelist[0].parentName.toUpperCase());
                if (ChartType == "Table" || ChartType == "") {
                    $("#chart-title").hide();
                    $(".ChartDivArea").css("padding-top", "30px");
                }
                else {
                    $("#chart-title").show();
                    $(".ChartDivArea").css("padding-top", "0px");
                }
                $(".showChartMain").getNiceScroll().resize().hide();
                if (ChartType.toLocaleLowerCase() == "pyramid with change" || ChartType.toLocaleLowerCase() == "pyramid") {
                    SetScroll($(".showChartMain"), "#393939", 0, 0, 0, 0, 8);
                }
                $(".trendChartMain").css("height", "100%");
            },
            error: function (xhr, status, error) {
                //NoDataAvailable();
                  GoToErrorPage();
            }
        });
    }
}

function PrepareTable(data) {
    ShowFrequencyHeader();
    var maintbwidth = 900;
    var FixedColumnWidth = 300;
    var FixedTableHeight = 400;
    if (data.LeftHeader != "" && data.LeftBody != "" && data.RightHeader != "" && data.RightBody != "") {
        PlotTable(data.LeftHeader, data.LeftBody, data.RightHeader, data.RightBody, maintbwidth, FixedColumnWidth, FixedTableHeight);
    }
    else {
        NoDataAvailable();
    }
}

function PlotTable(leftheader, leftbody, rightheader, rightbody, _width, _fixedColumnWidth, _fixedTableheight) {
    //left table header
    var table = "<div class=\"leftheader\" style=\"\">";
    table += leftheader;
    table += "</div>";
    //end left table header

    //right table header
    table += "<div class=\"rightheader\" style=\"overflow:hidden;\">";
    table += rightheader;
    table += "</div>";
    //end right table header

    //left table body
    table += "<div class=\"leftbody\" style=\"overflow:hidden;\">";
    table += leftbody;
    table += "</div>";
    //  

    //right table body  
    table += "<div onscroll=\"reposVertical(this);\" class=\"rightbody\" style=\"overflow:auto;\">";
    table += rightbody;
    table += "</div>";
    //end left table body   

    $(".ChartDivArea").html("");
    $(".ChartDivArea").html(table);
    SetStyles();
    SetScroll($(".ChartDivArea .rightbody"), "#393939", 0, -8, 0, -8, 8);

    if (SelectedFrequencyList.length > 0) {
        $(".ShoppingFrequencyheader span").text("");
        $(".ShoppingFrequencyheader span").text(SelectedFrequencyList[0].Name);
    }
}
function SetStyles() {
    $(".table-title").prev(".rowitem").children("ul").children("li").css("border", "0");
    //var rightHeaderHeight = $(".rightheader").height()
    //$(".leftheader").css("height", rightHeaderHeight);
    //var rowHeight = $(".rightheader .rowitem").height();
    //$(".rightheader .rowitem").eq(0).find("li").css("height", rowHeight);
    //$(".leftheader .rowitem").eq(0).css("height", rowHeight);
    //$(".leftheader .rowitem").eq(0).find("li").css("height", rowHeight);
    //var totalHeight = $(".ChartDivArea").height();
    //$(".leftbody").css("height", totalHeight - rightHeaderHeight - 5);
    //$(".rightbody").css("height", totalHeight - rightHeaderHeight - 5);

    var rightHeaderHeight = $(".rightheader").height();
    var totalHeight = $(".ChartDivArea").height();
    $(".leftbody").css("height", "calc(100% - " + (rightHeaderHeight - 5) + "px)");
    $(".rightbody").css("height", "calc(100% - " + (rightHeaderHeight - 5) + "px)");

    if (rightHeaderHeight != $(".leftheader").height())
        $(".leftheader").css("height", rightHeaderHeight - 0.5);
    var rowHeight = $(".rightheader .rowitem").height();
    if (rowHeight != $(".rightheader .rowitem").eq(0).find("li").height())
        $(".rightheader .rowitem").eq(0).find("li").css("height", rowHeight - 0.5);
    if (rowHeight != $(".leftheader .rowitem").eq(0).height())
        $(".leftheader .rowitem").eq(0).css("height", rowHeight - 0.5);
    if (rowHeight != $(".leftheader .rowitem").eq(0).find("li").height())
        $(".leftheader .rowitem").eq(0).find("li").css("height", rowHeight - 0.5);

    $(".leftbody ul").each(function (i) {
        $(".rightbody ul").eq(i).height($(this).height());
    });
}
function reposVertical(e) {
    $(".rightheader").scrollTop(e.scrollTop);
    $(".leftbody").scrollTop(e.scrollTop);
    $(".rightbody").scrollTop(e.scrollTop);

    $(".rightheader").scrollLeft(e.scrollLeft);
    $(".rightbody").scrollLeft(e.scrollLeft);
}

