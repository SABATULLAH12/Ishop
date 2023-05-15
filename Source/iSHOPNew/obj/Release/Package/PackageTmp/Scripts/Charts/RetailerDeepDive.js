/// <reference path="../Layout/Layout.js" />
/// <reference path="../Layout/RightPanelFilter.js" />

$(document).ready(function () {
    ClearChartReports();
});
function prepareContentArea() {  
    $(".trendChartMain").css("height", "100%");
    SetDefaultCustomBase();
    if (TabType.toString() == "") {
        TabType = "trips";
        $("#RightPanelPartial .rgt-cntrl-frequency-Conatiner ul div[name='TOTAL VISITS']").trigger("click");
    }
    if (ChartType.toLowerCase() == "table") {
        //-Abhay
        if (ModuleBlock == "PIT") {
            if (!Validate_CompareRetailers_Charts() || !Validate_Group_Charts() || !Validate_Measures_Charts()) {
                return false;
            }
        }
        else {
            if (!Validate_CompareRetailers_Charts() || !Validate_Measures_Charts()) {
                return false;
            }
        }
        $("#RightPanelPartial").show();
        sRemovedLegendPosition = [];
        var _profilerparams = new Object();
        ChartTileName = "demographic profiling";
        //_profilerparams.TabName = tabname;
        if (SelectedFrequencyList.length > 0) {
            _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
            _profilerparams.ShopperFrequencyShortName = SelectedFrequencyList[0].Name;
            _profilerparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
        }
        _profilerparams.ActiveTab = GetCurrentSPName();
        _profilerparams.Sigtype_UniqueId = Sigtype_Id;
        _profilerparams.SelectedStatTest = Selected_StatTest;
        _profilerparams.TimePeriod = TimePeriod;
        _profilerparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
        _profilerparams.ShortTimePeriod = $(".timeType").val();
        _profilerparams.Benchmark = TimePeriodFrom_UniqueId;
        _profilerparams.Comparisonlist = TimePeriodTo_UniqueId;
        _profilerparams.ShortTimePeriod = $(".timeType").val();
        _profilerparams.Comparison_DBNames = [];
        _profilerparams.Comparison_ShortNames = [];
        _profilerparams.Comparison_UniqueIds = [];

        //_profilerparams.Tab_Id_mapping = "true";
        //custom base
        if (CustomBase.length > 0) {
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;
            else
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;

            _profilerparams.CustomBase_ShortName = CustomBase[0].Name;
            _profilerparams.CustomBase_UniqueId = CustomBase[0].UniqueId;
        }
        if (ModuleBlock == "PIT") {
            _profilerparams.ViewType = "GROUPS";

            //----------->

            if (!Validate_CompareRetailers_Charts() || !Validate_Group_Charts() || !Validate_Measures_Charts()) {
                return false;
            }
            $("#RightPanelPartial").show();
            _profilerparams.View = "Within";
            _profilerparams.TimePeriod = TimePeriod;
            _profilerparams.Tab_Id_mapping = "true";
            //_profilerparams.Tab_Id_mapping = "false";
            _profilerparams.TimePeriodShortName = $(".timeType").val();
            for (var i = 0; i < Grouplist.length; i++) {
                _profilerparams.Comparison_DBNames.push(Grouplist[i].parentName + " |" + Grouplist[i].Name);
                _profilerparams.Comparison_ShortNames.push(Grouplist[i].Name);
                _profilerparams.Comparison_UniqueIds.push(Grouplist[i].UniqueId);
                if (i == 0)
                    _profilerparams.Benchmark = Grouplist[i].parentName + " |" + Grouplist[i].Name;
            }
        }
        else {
            if (!Validate_CompareRetailers_Charts() || !Validate_Measures_Charts() || !Validate_Trend()) {
                return false;
            }
            $("#RightPanelPartial").show();
            _profilerparams.ViewType = "TIME PERIODS";
            _profilerparams.Tab_Id_mapping = "true";
            _profilerparams.View = "Time";
            //_profilerparams.Tab_Id_mapping = "false";
            _profilerparams.Comparison_DBNames = [TimePeriod_From, TimePeriod_To];
            _profilerparams.Comparison_ShortNames = TimePeriod_ShortNames;
            _profilerparams.Benchmark = TimePeriod_From;
            _profilerparams.Comparisonlist = TimePeriod_To;
            // _profilerparams.Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);

            _profilerparams.TimePeriodFrom_UniqueId = TimePeriodFrom_UniqueId;
            _profilerparams.TimePeriodTo_UniqueId = TimePeriodTo_UniqueId;
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.beverageSelectionType_UniqueId = "";
            else
                _profilerparams.beverageSelectionType_UniqueId = "";
            //_profilerparams.beverageSelectionType_UniqueId = sBevarageSelctionType[0].UniqueId.toString();

            if (Comparisonlist.length > 1) {
                _profilerparams.TrendType = "2";
                sTrendType = "2";
            }
            else {
                _profilerparams.TrendType = "1";
                sTrendType = "1";
            }
        }

        _profilerparams.BCFullNames = _profilerparams.Comparison_ShortNames.toString();


        Advanced_Filters_DBNames = [];
        Advanced_Filters_ShortNames = [];
        Advanced_Filters_UniqueId = [];
        Advanced_Filters = [];
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
        _profilerparams.Filters = Advanced_Filters_DBNames.join("|");
        _profilerparams.FilterShortnames = Advanced_Filters_ShortNames.join("|");
        _profilerparams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");



        if (Comparisonlist.length > 0) {

            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.ShopperSegment = Comparisonlist[0].LevelDesc.split("||")[0] + "||" + Comparisonlist[0].Name;
            else
                _profilerparams.ShopperSegment = Comparisonlist[0].LevelDesc.split("||")[0] + "||" + Comparisonlist[0].Name;

            var sComparisonNames = [];
            if (ModuleBlock != "PIT") {
                //_profilerparams.Comparison_UniqueIds.push(Comparisonlist[0].UniqueId);
                for (var i = 0; i < Comparisonlist.length; i++) {
                    _profilerparams.Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);
                    sComparisonNames.push(Comparisonlist[i].Name);
                }
                _profilerparams.ComparisonNames = sComparisonNames;
            }
            else
                _profilerparams.SingleSelection = Comparisonlist[0].UniqueId;

            if (_profilerparams.Comparison_UniqueIds == null || _profilerparams.Comparison_UniqueIds.length <= 0 || _profilerparams.Comparison_UniqueIds == [])
                _profilerparams.Comparison_UniqueIds.push(Comparisonlist[0].UniqueId);

        }



        _profilerparams.ModuleBlock = "TimeShopper";
        if (ModuleBlock == "PIT")
            _profilerparams.ModuleBlock = "PIT";
        else
            _profilerparams.ModuleBlock = "TREND";
        _profilerparams.Metric = Measurelist[0].Name;

        _profilerparams.ChartXValues = _profilerparams.Comparison_ShortNames;
        _profilerparams.ChartType = ChartType;
        _profilerparams.MetricShortName = Measurelist[0].parentName.toUpperCase();

        var sMetricNames = "";
        var sMetric = [];
        for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
            sMetricNames += Measurelist[0].metriclist[i].Name + "|";
            sMetric.push(Measurelist[0].metriclist[i].Id);
        }
        sMetricNames = sMetricNames.substring(0, sMetricNames.length - 1);
        _profilerparams.selectedMetrics = sMetric;
        _profilerparams.SelectedMetrics = sMetricNames;
        _profilerparams.MetricShortName = Measurelist[0].parentName.toUpperCase();
        _profilerparams.SelectedMetricsIds = sMetric.join('|');
        if (CompetitorRetailer.length > 0) {
            _profilerparams.CompetitorRetailer_Name = CompetitorRetailer[0].Name;
            _profilerparams.CompetitorRetailer_UniqueId = CompetitorRetailer[0].UniqueId;

            _profilerparams.CompetitorFrequency_Name = CompetitorFrequency[0].Name;
            _profilerparams.CompetitorFrequency_UniqueId = CompetitorFrequency[0].UniqueId;
        }
        //_profilerparams.selectedMetrics = sMetricNames;
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

                //ChartModuleData = data;
                //$("#btnAddToExport").prop('disabled', false);
                //$("#btnViewSelections").prop('disabled', true);
                //$("#btnClearAll").prop('disabled', true); 
                //$("#btnViewSelections").css("background-color", "#BEBEBE");
                //$("#btnViewSelections").css("background-color", "lightgray");
                //$("#btnClearAll").css("background-color", "#lightgray");
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
                    // $(".ChartDivArea").css("height", "auto");
                    $(".ChartDivArea").css("width", "100%");
                    $("#spChartLegend").empty()
                    $(".trendChartMain").empty();
                    $(".trendChartMain").hide();
                    //$(".ChartDivArea").html(data);
                    $(".ChartDivArea").show();
                    $("#UpdateProgress").hide();
                    $(".TranslucentDiv").hide();

                    //CheckExportList();
                    isPanelChange = "false";
                    $(".tableheader").css("width", "100%");
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
        if (!Validate_CompareRetailers_Charts() || !Validate_Measures_Charts()) {
            return false;
        }
        $("#RightPanelPartial").show();
        ChartTileName = "demographic profiling";
        var _profilerparams = new Object();
        _profilerparams.ModuleBlock = ModuleBlock;
        _profilerparams.ActiveTab = GetCurrentSPName();
        if (SelectedFrequencyList.length > 0) {
            _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
            _profilerparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
        }
        //_profilerparams.Tab_Id_mapping ="true";
        _profilerparams.TabIndexId = Measurelist[0].filtertypeid;
        //_profilerparams.Tab_Id_mapping = Tab_Id_mapping;

        _profilerparams.SelectedStatTest = Selected_StatTest;
        _profilerparams.Sigtype_UniqueId = Sigtype_Id;

        _profilerparams.TimePeriod = TimePeriod;
        _profilerparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
        _profilerparams.Benchmark = TimePeriodFrom_UniqueId;
        _profilerparams.Comparisonlist = TimePeriodTo_UniqueId;
        _profilerparams.ShortTimePeriod = $(".timeType").val();
        _profilerparams.Comparison_DBNames = [];
        _profilerparams.Comparison_ShortNames = [];
        _profilerparams.Comparison_UniqueIds = [];
        //custom base
        if (CustomBase.length > 0) {
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;
            else
                _profilerparams.CustomBase_DBName = CustomBase[0].Name;

            _profilerparams.CustomBase_ShortName = CustomBase[0].Name;
            _profilerparams.CustomBase_UniqueId = CustomBase[0].UniqueId;
        }
        if (ModuleBlock == "PIT") {
            if (!Validate_CompareRetailers_Charts() || !Validate_Group_Charts() || !Validate_Measures_Charts()) {
                return false;
            }
            $("#RightPanelPartial").show();
            _profilerparams.ViewType = "GROUPS";
            _profilerparams.TimePeriod = TimePeriod;
            _profilerparams.Tab_Id_mapping = "true";

            //_profilerparams.Tab_Id_mapping = "false";
            _profilerparams.TimePeriodShortName = $(".timeType").val();
            for (var i = 0; i < Grouplist.length; i++) {
                _profilerparams.Comparison_DBNames.push(Grouplist[i].parentName + " |" + Grouplist[i].Name);
                _profilerparams.Comparison_ShortNames.push(Grouplist[i].Name);
                _profilerparams.Comparison_UniqueIds.push(Grouplist[i].UniqueId);
            }
        }
        else {
            if (!Validate_CompareRetailers_Charts() || !Validate_Measures_Charts() || !Validate_Trend()) {
                return false;
            }
            $("#RightPanelPartial").show();
            _profilerparams.ViewType = "TIME PERIODS";
            _profilerparams.Tab_Id_mapping = "true";
            //_profilerparams.Tab_Id_mapping = "false";
            _profilerparams.Comparison_DBNames.push(TimePeriod_From);
            _profilerparams.Comparison_DBNames.push(TimePeriod_To);
            _profilerparams.Comparison_ShortNames = TimePeriod_ShortNames;

            _profilerparams.TimePeriodFrom_UniqueId = TimePeriodFrom_UniqueId;
            _profilerparams.TimePeriodTo_UniqueId = TimePeriodTo_UniqueId;
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.beverageSelectionType_UniqueId = "";
            else
                _profilerparams.beverageSelectionType_UniqueId = "";
            // _profilerparams.beverageSelectionType_UniqueId = sBevarageSelctionType[0].UniqueId.toString();

            if (Comparisonlist.length > 1) {
                _profilerparams.TrendType = "2";
                sTrendType = "2";
            }
            else {
                _profilerparams.TrendType = "1";
                sTrendType = "1";
            }
        }
        if (Comparisonlist.length > 0) {


            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.ShopperSegment = Comparisonlist[0].LevelDesc.split("||")[0] + "||" + Comparisonlist[0].Name;
            else
                _profilerparams.ShopperSegment = Comparisonlist[0].LevelDesc.split("||")[0] + "||" + Comparisonlist[0].Name;

            _profilerparams.SingleSelection = Comparisonlist[0].Name;
            if (ModuleBlock != "PIT") {
                //_profilerparams.Comparison_UniqueIds.push(Comparisonlist[0].UniqueId);
                for (var i = 0; i < Comparisonlist.length; i++)
                    _profilerparams.Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);
            }
            else
                _profilerparams.SingleSelection = Comparisonlist[0].UniqueId;
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
        _profilerparams.Filters = Advanced_Filters_DBNames.join("|");
        _profilerparams.FilterShortnames = Advanced_Filters_ShortNames.join("|");
        _profilerparams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");
        _profilerparams.Metric = Measurelist[0].Name;
        _profilerparams.Metric_UniqueId = Measurelist[0].MetricId;
        _profilerparams.Filtertypeid_UniqueId = Measurelist[0].filtertypeid.toString();
        var sMetricNames = "";
        var sMetric = [];

        for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
            sMetricNames += Measurelist[0].metriclist[i].Id + "|";
            sMetric.push(Measurelist[0].metriclist[i].Name);
        }
        sMetricNames = sMetricNames.substring(0, sMetricNames.length - 1)
        _profilerparams.SelectedMetrics = sMetric.join("|");//"Male|Female";
        _profilerparams.SelectedMetricsIds = sMetricNames;
        //_profilerparams.Tab_Id_mapping = "true";
        _profilerparams.ChartType = ChartType;
        if (CompetitorRetailer.length > 0) {
            _profilerparams.CompetitorRetailer_Name = CompetitorRetailer[0].Name;
            _profilerparams.CompetitorRetailer_UniqueId = CompetitorRetailer[0].UniqueId;

            _profilerparams.CompetitorFrequency_Name = CompetitorFrequency[0].Name;
            _profilerparams.CompetitorFrequency_UniqueId = CompetitorFrequency[0].UniqueId;
        }
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
                $("#idtrendChartMain00").html("");
                $("#idtrendChartMain00").hide();
                RetailnChartReports();
                ChartModuleData = data;
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
                $("#spChartLegend").css("margin", "32px auto 7px 0px");
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


