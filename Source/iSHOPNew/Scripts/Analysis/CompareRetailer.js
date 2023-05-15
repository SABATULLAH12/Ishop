var seltype = "Scatter Chart"
var ZoomValue = 1;
var ScatterChartData;
var ExportVal = "";
//var _tablewidth = 1000;
//var _tableheight = 500;
//var _fixedtablewidth = 159;
//var _fixedcolmnwidth = 159;
//var _firstcolwidth = 159;
//var _RightPanelWidth = 1000;
//var _FixedTableHeight = 300;
//var _FixedColumnWidth = 500;
$(document).ready(function(){
    $(".SelectionType").click(function () {
        $("#guestFrqncy").hide();
        seltype = $(this).attr("view-type");
        SetAnalysisItemActiveColor();
        prepareContentArea(false);
    });
    $("#ExportToExcel").click(function (e) {
        ExportVal = "EXCEL DOWNLOAD";
        prepareContentArea(ExportVal);
    });
    $("#ExportToPPT").click(function (e) {
        ExportVal = "PPT DOWNLOAD";
        prepareContentArea(ExportVal);
    });
    if ($(".LowSample-popup").is(":visible"))
        $(".TranslucentDiv").show();
});
function prepareContentArea(excelStatus) {
    var localTime = new Date();
    var year = localTime.getFullYear();
    var month = localTime.getMonth() + 1;
    var date = localTime.getDate();
    var hours = localTime.getHours();
    var minutes = localTime.getMinutes();
    var seconds = localTime.getSeconds();

    var _profilerparams = new Object();
    _profilerparams.ChartType = seltype;
    _profilerparams.ChartHeight = "393";
    _profilerparams.ChartWidth = "973";
    _profilerparams.ActiveTab = GetCurrentSPName();

    _profilerparams.FrequencyTitle = "Frequency";
    if (!Validate_CompareRetailers_AdvanceAnalysis() || !Validate_Measures_AdvanceAnlysis()) {
        return false;
    }
    $("#RightPanelPartial").show();
    if (SelectedFrequencyList.length > 0) {
        _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name;
        _profilerparams.ShopperFrequencyShortName = SelectedFrequencyList[0].Name;
        _profilerparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }
    _profilerparams.ViewType = "RETAILERS";
    _profilerparams.Comparison_DBNames = [];
    _profilerparams.Comparison_ShortNames = [];
    _profilerparams.Comparison_Ids = [];
    var sCompList = [];

    var Comparison_UniqueIds = [];
    for (var i = 0; i < Comparisonlist.length; i++) {
        if (TabType.toLocaleLowerCase() == "trips"){
            _profilerparams.Comparison_DBNames.push(Comparisonlist[i].LevelDesc + "|" + Comparisonlist[i].Name);
            //sCompList.push(Comparisonlist[i].Name);
        }
        else{
            _profilerparams.Comparison_DBNames.push(Comparisonlist[i].LevelDesc + "|" + Comparisonlist[i].Name);
            //sCompList.push(Comparisonlist[i].Name);
        }
        sCompList.push(Comparisonlist[i].Name);
        _profilerparams.Comparison_ShortNames.push(Comparisonlist[i].Name);
        _profilerparams.Comparison_Ids.push(Comparisonlist[i].UniqueId);
            
        Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);
    }
    _profilerparams.Comparison_UniqueIds = Comparison_UniqueIds.join("|");

    _profilerparams.Benchmark = _profilerparams.Comparison_DBNames[0];
    _profilerparams.BCFullNames = _profilerparams.Comparison_ShortNames;
    _profilerparams.Comparisonlist = sCompList.join("|");
    _profilerparams.TimePeriod = TimePeriod;
    _profilerparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
    _profilerparams.ShortTimePeriod = $(".timeType").val();
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
    _profilerparams.Filters = Advanced_Filters_DBNames.join("|");
    _profilerparams.ShopperSegment = "";
    _profilerparams.FilterShortnames = Advanced_Filters_ShortNames.join("|");
    _profilerparams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");
    _profilerparams.ModuleBlock = "AcrossShopper";
    _profilerparams.View = "AcrossShopper";
    _profilerparams.Metric = SelectedAdvancedAnalyticsList[0].parentname;//Measurelist[0].DBName;
    //_profilerparams.Metric_UniqueId = Measurelist[0].MetricId;
    var sMetricNames = "";
    var sMetric = [];
    var sMetricDBName = "";
    var sMericUniqueId = [];
    for (var i = 0; i < SelectedAdvancedAnalyticsList.length; i++) {
        sMetricDBName += SelectedAdvancedAnalyticsList[0].parentname + "|" + SelectedAdvancedAnalyticsList[i].Name + "|";
        sMetricNames += SelectedAdvancedAnalyticsList[i].Name + "|";
        sMetric.push(SelectedAdvancedAnalyticsList[i].Name);
        sMericUniqueId.push(SelectedAdvancedAnalyticsList[i].UniqueId);
    }
    sMetricNames = sMetricNames.substring(0, sMetricNames.length - 1);
    sMetricDBName = sMetricDBName.substring(0, sMetricDBName.length - 1);
    
    _profilerparams.selectedMetrics = sMetric;
    _profilerparams.MetricUniqueId = sMericUniqueId.join('|').toString();
    _profilerparams.ChartXValues = _profilerparams.Comparison_ShortNames;
    _profilerparams.ChartXValues_UniqueId = _profilerparams.Comparison_UniqueIds;
    _profilerparams.ChartType = seltype;//ChartType;
    _profilerparams.ShortNames = "";
    _profilerparams.SelectedMetrics = sMetricDBName;
    _profilerparams.ComparisonItems = sCompList;
    _profilerparams.StoreidItems = _profilerparams.Comparison_Ids;
    _profilerparams.ComparisonShortNameItems = _profilerparams.Comparison_ShortNames;
    _profilerparams.MetricShortName = SelectedAdvancedAnalyticsList[0].parentname;
    _profilerparams.GroupUniqueId = "";
    postBackData = "{AdvancedAnalyticsParams:" + JSON.stringify(_profilerparams) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLAnalysis").val() + "/StoreChartInput",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            $("#Table-Content").show();
            $(".ChartAreaNew").show();
            var text = "";
            $("#Table-Content").html("");
            if (content.Rlist != null && content.Rlist != "" && content.Rlist.length > 0) {
                ScatterChartData = prepareDataForScatter(content.Rlist, content.Objective_Count);
                if (seltype == "Scatter Chart") {
                    scatterPlotChart(ScatterChartData, 1);
                    SetScroll($("#Table-Content"), "#393939", 0, 0, 0, 0, 8);
                }

                else if (seltype == "dimension Table") {
                    $("#Table-Content").html("");
                    $("#Table-Content").html(content.R_Table);
                    SetScroll($(".tablebody"), "#393939", 0, -8, 0, -8, 8);
                    $("#Table-Content").getNiceScroll().remove()
                    $('.compareRetailerheader').click(function () {

                        if ($(this).children("td").find(".treeview").hasClass("minusIcon")) {
                            $(this).children("td").find(".treeview").removeClass("minusIcon");
                            $(this).children("td").find(".treeview").removeClass("plusIcon");
                            $(this).children("td").find(".treeview").addClass("plusIcon");
                        }
                        else {
                            $(this).children("td").find(".treeview").removeClass("minusIcon");
                            $(this).children("td").find(".treeview").removeClass("plusIcon");
                            $(this).children("td").find(".treeview").addClass("minusIcon");
                        }

                        if ($(this).nextUntil('tr.compareRetailerheader').length > 0)
                            $(this).nextUntil('tr.compareRetailerheader').slideToggle(100, function () {
                                SetScroll($(".tablebody"), "#393939", 0, -8, 0, -8, 8);
                            });
                        else
                            $(this).parent().nextUntil('tr.compareRetailerheader').slideToggle(100, function () {
                                SetScroll($(".tablebody"), "#393939", 0, -8, 0, -8, 8);
                            });
                        SetScroll($(".tablebody"), "#393939", 0, 0, 0, 0, 8);
                    });
                }
                else
                    BuildTable(content.ishoparams);

                //$("#ShowSampleSize").html("");
                //$("#ShowSampleSize").html(content.Get_SampleSize);
            }
            if (content != null) {
                //SetBGMTableWidthHeight();

                //$("#UpdateProgress").hide();
                //$(".TranslucentDiv").hide();
                if (excelStatus == "PPT DOWNLOAD") {
                    window.location.href = $("#URLAnalysis").val() + "/" + "Export_To_PPT_ForAdvancedAnalysis?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                    ExportVal = "";
                }
                else if (excelStatus == "EXCEL DOWNLOAD") {
                    //if (Check_Download_Data()) {
                        window.location.href = $("#URLAnalysis").val() + "/" + "Export_To_Excel_ForAdvancedAnalysis?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                        ExportVal = "";
                    //}
                }
                ExportVal = "";
            }
            if (content.LowSampleSize != null && content.LowSampleSize != "") {
                CorrespondaceMapsLowSampleSizeVariables = content.StoreidItems;
                $("#Loader").hide();
                $(".LowSample-popup").show();
                $(".LowSample-heading").text("LOW SAMPLE SIZE");
                $(".LowSample-submt").show();
                $(".LowSample-cancel").css("margin-left", "35px");
                $(".LowSample-submt").text("Proceed");
                $(".LowSample-content").text("");
                 text = "";
                text += "<p style=\"color:white;margin-left:4%;margin-top:0%;\">Note:</br>Data not available for selected element(s):<br>";
                _.each(content.LowSampleSize.split('^'), function (i) {
                    text += i + "<br>";
                });
                text += "</p>";
                $(".LowSample-content").append(text);
                $(".TranslucentDiv").show();
            }
            else if (content.LowVariables != null && content.LowVariables != "")
            {
                $("#Loader").hide();
                $(".LowSample-popup").show();
                $(".LowSample-heading").text("NO DATA AVAILABLE");
                $(".LowSample-submt").hide();
                $(".LowSample-content").css("margin-left", "0%");
                $(".LowSample-cancel").css("margin-left", "40%");
                $(".LowSample-content").text("");
                text = "";
                text += "<p style=\"color:white;margin-left:4%;margin-top:0%;\">Note:</br>" + content.LowVariables + "<br>";               
                text += "</p>";
                $(".LowSample-content").append(text);
                $(".TranslucentDiv").show();
            }
            else
                CorrespondaceMapsLowSampleSizeVariables = "";

        },
        error: function (error) {
            $("#UpdateProgress").hide();
            $(".TranslucentDiv").hide();
            //showMessage(error.responseText);       
            GoToErrorPage();
        }
    });
    //End 01-07-2014
    if ($(".LowSample-popup").is(":visible"))
        $(".TranslucentDiv").show();
}
function Check_Download_Data() {
    var isDownload = true;
    var postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLAnalysis").val() + "/" + "Get_Check_Download_Data",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            if (content == false) {
                isDownload = false;
            }
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
    return isDownload;
}
function GetDataNotAvailableNote() {

}

function BuildTable(data) {
    ShowFrequencyHeader();
    if (data.LeftHeader != "" && data.LeftBody != "" && data.RightHeader != "" && data.RightBody != "") {
        PlotTable(data.LeftHeader, data.LeftBody, data.RightHeader, data.RightBody, 937, 159, 300);
    }
    $('.correspondancecompretailerheading').click(function () {
        if ($(".treeview").hasClass("minusIcon")) {
            $(".treeview").removeClass("minusIcon");
            $(".treeview").removeClass("plusIcon");
            $(".treeview").addClass("plusIcon");
        }
        else {
            $(".treeview").removeClass("minusIcon");
            $(".treeview").removeClass("plusIcon");
            $(".treeview").addClass("minusIcon");
        }
        if ($(this).nextUntil('tr.correspondancecompretailerheading').length > 0) {
            $(".fixedContainer .correspondancecompretailerheading").eq(0).nextUntil('tr.correspondancecompretailerheading').slideToggle(0, function () { SetScroll($("#contentscroll"), "#393939", 0, -8, 0, -8, 8); });
            $("#divfrozen .correspondancecompretailerheading").eq(0).nextUntil('tr.correspondancecompretailerheading').slideToggle(0, function () { SetScroll($("#contentscroll"), "#393939", 0, -8, 0, -8, 8); });
        }
        else {
            $(".fixedContainer .correspondancecompretailerheading").eq(0).parent().nextUntil('tr.correspondancecompretailerheading').slideToggle(0, function () { SetScroll($("#contentscroll"), "#393939", 0, -8, 0, -8, 8); });
            $("#divfrozen .correspondancecompretailerheading").eq(0).parent().nextUntil('tr.correspondancecompretailerheading').slideToggle(0, function () { SetScroll($("#contentscroll"), "#393939", 0, -8, 0, -8, 8); });
}
            SetScroll($("#contentscroll"), "#393939", 0, -8, 0, -8, 8);
    });
    //$(".leftbody ul").each(function (i) {
    //    $(".rightbody ul").eq(i).height($(this).height());
    //});
}

function PlotTable(leftheader, leftbody, rightheader, rightbody, _width, _fixedColumnWidth, _fixedTableheight) {
    //var leftcolumns = opts.fixedColumns;
    htmlstring = "<table class=\"FixedTables\" style=\"width:auto;height: 100%;display:flex;\">";
    htmlstring += "<tr style=\"display: flex;width: 100%;height: 100%;\">";

    //create left table header          
    htmlstring += "<td valign=\"top\" class=\"fixedColumn\" style=\"width:47%;margin-right: 10px;\">";
    htmlstring += "<div class=\"fixedHead\">";
    htmlstring += leftheader;
    htmlstring += "</div>";

    ////create left table body
    htmlstring += "<div id=\"divfrozen\" class=\"fixedTable\" style=\"max-height:403px;overflow:hidden;\">";
    htmlstring += leftbody;
    htmlstring += "</div>";
    htmlstring += "</td>";

    ////create right table header          
    htmlstring += "<td valign=\"top\" class=\"fixedContainer\" style=\"width:50.5%;\">";
    htmlstring += "<div id=\"headscroll\"  class=\"fixedHead\" style=\"width:98.8%;\">";
    htmlstring += "<table style=\"width:100%;\">";
    htmlstring += rightheader;
    htmlstring += "</table>";
    htmlstring += "</div>";

    //////create right table body
    htmlstring += "<div id=\"contentscroll\" class=\"fixedTable\" onscroll=\"reposVertical(this);\" style=\"min-height:50px;width:99%;max-height: 403px;height:92%;\">";
    htmlstring += "<table>";
    htmlstring += rightbody;
    htmlstring += "</table>";
    htmlstring += "</div>";
    htmlstring += "</td>";
    htmlstring += "</tr>";
    htmlstring += "</table>";
    $("#Table-Content").getNiceScroll().remove();
    $("#Table-Content").html("");
    $("#Table-Content").html(htmlstring);
    //SetScroll($("#contentscroll"), "#393939", 0, -8, 0, -8, 8);
    var swidth = $(".fixedContainer").width();
    var sNumber = (swidth/150);
    if (sNumber > Comparisonlist.length) {
        var SetWidth = (swidth / (Comparisonlist.length));
        var SetWidth1 = (SetWidth * (202 / 215));
        //var SetWidth2 = (SetWidth * (244 / 215));
        //$(".ColumnWidthHeader").css("min-width", SetWidth1);
        //if (Comparisonlist.length == 4)
        //    //$(".ColumnWidth").css("min-width", "150px");
        //else
        //    $(".ColumnWidth").css("min-width", SetWidth1);
    }
    else {
        //var SetWidth = 150;
        //$(".ColumnWidth").css("min-width", SetWidth);
    }
    SetStyles();
    SetScroll($("#contentscroll"), "#393939", 0, -8, 0, -8, 8);
}
function SetStyles() {
    //$(".table-title").prev(".rowitem").children("ul").children("li").css("border", "0");
    //if ($(".FixedTables td").length > 0) {
    //    var sMainWidth = $(".FixedTables td")[0].getBoundingClientRect().width;
    //    var leftWidth = (52.83 * sMainWidth) / 100;
    //    var rightWidth = (45.8 * sMainWidth) / 100;
    //    $(".leftColumnHeaderWidth").css("width", leftWidth);
    //    $(".leftColumnWidth").css("width", rightWidth);
    //}

    var rightHeaderHeight = $("#headscroll").height();
    //$(".fixedHead").eq(0).css("height", rightHeaderHeight);
    $(".fixedHead td").css("height", rightHeaderHeight-3);
    //var rowHeight = $(".fixedContainer .rowitem").height();
    //$(".fixedContainer .rowitem").eq(0).find("li").css("height", rowHeight);
    //$(".fixedColumn .rowitem").eq(0).css("height", rowHeight);
    //$(".fixedColumn .rowitem").eq(0).find("li").css("height", rowHeight);
    var totalHeight = $(".FixedTables").height();
    $("#divfrozen").css("height", totalHeight - rightHeaderHeight - 5);
    $("#contentscroll").css("height", totalHeight - rightHeaderHeight);
    //$("#divfrozen ul").each(function (i) {
    //    $("#contentscroll ul").eq(i).height($(this).height());
    //});

    $(".fixedContainer .fixedHead table tr").each(function (i) {
        var height = $(this).height();
        $(".fixedContainer .fixedHead table tr").eq(i).children("td").height(height);
        //$("#contentscroll table tr").eq(i).height(height);
        $(".fixedColumn .fixedHead table tr").eq(i).children("td").height(height);
    });

    $("#divfrozen table tr").each(function (i) {
        var height = $(this).height();       
        $("#divfrozen table tr").eq(i).children("td").height(height);
        //$("#contentscroll table tr").eq(i).height(height);
        $("#contentscroll table tr").eq(i).children("td").height(height);
    });
}
//Scroll
function reposVertical(e) {
    //SetScroll($(".fixedColumn .fixedTable"), "#393939", 0, 0, 0, 0, 8);
    $(".fixedContainer .fixedHead").scrollLeft(e.scrollLeft);
    $(".fixedColumn .fixedTable").scrollTop(e.scrollTop);
    $(".fixedContainer .fixedTable").scrollTop(e.scrollTop);

    $(".fixedContainer .fixedHead").scrollLeft(e.scrollLeft);
    $(".fixedContainer .fixedTable").scrollLeft(e.scrollLeft);
}
function GetNoDataVariables() {
    var postBackData = "{}";

    jQuery.ajax({
        type: "POST",
        url: "AdvancedAnalyticsChartArea.aspx/GetDataNotAvailableNote",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            $(".notecontent").html(content.d);
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
}
function ChaneImage() {
    $("#TimePeriodBlock .BarIcon").css("background-position", "-910px -834px");
    $("#DivComparison .BarIcon").css("background-position", "-1036px -834px");
    $("#DivBenchmark .BarIcon").css("background-position", "-971px -834px");
    $("#FrequencyBlock .BarIcon").css("background-position", "-1160px -834px");
    $("#AdvancedFilter .BarIcon").css("background-position", "-1095px -834px");
}
function GetBenchmarkTimePeriod() {
    isChange = "true";
    if (TimeExtension == "" || TimeExtension == "Total") {
        return "total|total";
    }
    else {
        return TimeExtension + "|" + $(".timeType").html();
    }

}

function GetComparisonTimePeriod() {
    isChange = "true";
    if (TimeExtension == "" || TimeExtension == "Total") {
        var shortnamelist = new Array();
        var timeindx = 0;
        shortnamelist[timeindx] = "total";
        return shortnamelist;
    }
    else {
        var shortnamelist = new Array();
        var timeindx = 0;
        shortnamelist[timeindx] = "" + $(".totime").html() + "";
        return shortnamelist;
    }
}

var scatterPlotChart = function (data, val) {
    $("#Table-Content").html("");
    $(".Top_Header").css("display", "inline-block");
    //data = [{ "yVal": -0.2, "xVal": 0.10, "species": "setosa", "radialWidth": 5 },
    //            { "yVal": 0.30, "xVal": 0.10, "species": "setosa", "radialWidth": 6 },
    //            { "yVal": 0.25, "xVal": 0.25, "species": "versicolor", "radialWidth": 5 },
    //            { "yVal": 0.5, "xVal": 0.3, "species": "versicolor", "radialWidth": 3 },
    //            { "yVal": 0.7, "xVal": 0.2, "species": "setosa", "radialWidth": 4 },
    //            { "yVal": 0.15, "xVal": -0.2, "species": "setosa", "radialWidth": 12 },
    //            { "yVal": 0.5, "xVal": 0.16, "species": "PKR", "radialWidth": 5 },
    //            { "yVal": 0.5, "xVal": 0.4, "species": "MIG", "radialWidth": 2 }
    //];
    data.forEach(function (d) {
        d.yVal = +d.yVal;
        d.xVal = +d.xVal;
        d.radialWidth = +d.radialWidth;
        d.species = d.species;
        d.colorDot = d.colorDot;

    });
    var max_rad = d3.max(data, function (d) { return d.radialWidth });
    //var margin = { top: 30, right: 100, bottom: 60, left: 100 },
    var margin = { top: 20, right: 100, bottom: 20, left: 100 },
    width = $("#Table-Content").width() - margin.left - margin.right,
    height = $("#Table-Content").height() - margin.top - margin.bottom;
    if (val != 1) {
        width *= Math.pow(1.1, val);
        height *= Math.pow(1.1, val);
    }
    var x = d3.scale.linear()
        .range([0, width]);

    var y = d3.scale.linear()
        .range([height, 0]);

    var color = d3.scale.category10();

    var xAxis = d3.svg.axis().orient("bottom")
        .scale(x).outerTickSize(0).ticks(0);

    var yAxis = d3.svg.axis().orient("left")
        .scale(y).outerTickSize(0).ticks(0);
    var svg = d3.select("#Table-Content").append("svg")
    .attr("width", width + margin.left + margin.right)
    .attr("height", height + margin.top + margin.bottom - 6)
  .append("g")
    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    x.domain(d3.extent(data, function (d) { return d.xVal; })).nice();
    y.domain(d3.extent(data, function (d) { return d.yVal; })).nice();
    /////////////////////////////////////////////
    var X_max = 0, X_min = 0, Y_max = 0, Y_min = 0;
    X_max = d3.max(x.domain());
    X_min = d3.min(x.domain());
    Y_max = d3.max(y.domain());
    Y_min = d3.min(y.domain());
    /*checking for origin trnlst*/
    var xTranFact = 0, yTranFact = height;
    if ((X_max > 0 && X_min < 0)) { xTranFact = (-X_min / (X_max - X_min)) * width; }
    if ((Y_max > 0 && Y_min < 0)) { yTranFact = (Y_max / (Y_max - Y_min)) * height; }
    /*checking for origin trnlst*/
    /////////////////////////////////////////////
    var tool_tip = d3.select(".ChartAreaNew").append("div")
    .attr("class", "d3_tooltip")
    .style("opacity", 0);

    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + yTranFact + ")")
        .style("fill", "transparent")
        //.style("stroke", "#BBBDBF")
        .style("stroke", "transparent")
        .style("stroke-width", "1")
        .call(xAxis)
      .append("text")
        .attr("class", "label")
        .attr("x", width)
        .attr("y", -6)
        .style("text-anchor", "end")
        .text("Sepal Width (cm)");

    svg.append("g")
        .attr("class", "y axis")
        .attr("transform", "translate(" + xTranFact + ",0)")
        .style("fill", "transparent")
        //.style("stroke", "#BBBDBF")
        .style("stroke", "transparent")
        .style("stroke-width", "1")
        .call(yAxis)
      .append("text")
        .attr("class", "label")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .text("Sepal Length (cm)")

    svg.selectAll(".dot")
        .data(data)
      .enter().append("circle")
        .attr("class", "dot")
        .attr("r", function (d) { return d.radialWidth + 1; })
        .attr("cx", function (d) { return x(d.xVal); })
        .attr("cy", function (d) {
            /*Text*/
            var context = d3.select(this.parentNode);
            context.append("text")
                .attr("class", "dot_text")
            .attr("x", x(d.xVal))
            .attr("y", y(d.yVal) - d.radialWidth)
            .text(d.species)
            .style("fill", "#000").style("text-anchor", "middle").style("text-transform", "uppercase");
            /*Text*/
            return y(d.yVal);
        })
        .style("fill", function (d) { return d.colorDot; })
        .style("stroke", function (d) { if (d.colorDot == "#f00") { return "#DB7783"; } return "#93B3CE"; })
        .style("stroke-width", function (d) { return 4; })
        .on("mouseover", function (d) {
            tool_tip.transition()
            .duration(200)
            .style("opacity", 1);
            tool_tip.html("1." + d.nearEle[0].Text + "<br/>2." + d.nearEle[1].Text + "<br/>3." + d.nearEle[2].Text)
            .style("left", (d3.event.pageX) + "px")
            .style("top", (d3.event.pageY - 28) + "px");
        })
            .on("mouseout", function (d) {
                tool_tip.transition()
                    .duration(500)
                    .style("opacity", 0);
            });
    svg.append("rect")
        .attr("x", xTranFact - 15)
        .attr("y", yTranFact - 1)
        .attr("width", 30)
        .attr("height", 2)
        .style("fill", "#F39D1F")
    svg.append("rect")
        .attr("x", xTranFact - 1)
        .attr("y", yTranFact - 15)
        .attr("width", 2)
        .attr("height", 30)
        .style("fill", "#F39D1F")
    //var legend = svg.selectAll(".legend")
    //    .data(color.domain())
    //  .enter().append("g")
    //    .attr("class", "legend")
    //    .attr("transform", function (d, i) { return "translate(0," + i * 20 + ")"; });

    //legend.append("rect")
    //    .attr("x", width - 18)
    //    .attr("width", 18)
    //    .attr("height", 18)
    //    .style("fill", color);

    //legend.append("text")
    //    .attr("x", width - 24)
    //    .attr("y", 9)
    //    .attr("dy", ".35em")
    //    .style("text-anchor", "end")
    //    .text(function (d) { return d; });
}

var prepareDataForScatter = function (rData, NoOfEst) {
    var temp_data = [];
    var hd = rData.map(function (d) {
        var g = { species: d.name.replace(/\"/g, ""), xVal: +d.x, yVal: +d.y, radialWidth: 5, colorDot: "#f00", nearEle: [] };
        temp_data.push(g);
    });
    temp_data.forEach(function (d, i) {
        if (i >= temp_data.length - NoOfEst) {
            d.colorDot = "#00f";
        }
    });
    /**/
    var j = 0;
    temp_data.forEach(function (d, i) {
        var Temp_n3 = [];
        if (d.colorDot == "#00f") {
            for (j = 0; j < temp_data.length - NoOfEst; j++) {
                var obj_d = { Text: temp_data[j].species, dis: 0 };
                obj_d.dis = Math.sqrt((d.xVal - temp_data[j].xVal) * (d.xVal - temp_data[j].xVal) + (d.yVal - temp_data[j].yVal) * (d.yVal - temp_data[j].yVal));
                Temp_n3.push(obj_d);
            }
        }
        if (d.colorDot == "#f00") {
            for (j = temp_data.length - NoOfEst; j < temp_data.length; j++) {
                var obj_d = { Text: temp_data[j].species, dis: 0 };
                obj_d.dis = Math.sqrt((d.xVal - temp_data[j].xVal) * (d.xVal - temp_data[j].xVal) + (d.yVal - temp_data[j].yVal) * (d.yVal - temp_data[j].yVal));
                Temp_n3.push(obj_d);
            }
        }
        //sort top 3
        Temp_n3.sort(function compare(a, b) {
            if (a.dis < b.dis)
                return -1;
            if (a.dis > b.dis)
                return 1;
            return 0;
        });
        //Store top 3;
        d.nearEle.push(Temp_n3[0], Temp_n3[1], Temp_n3[2]);
    });
    /**/
    return temp_data;

}

$(document).on("click", ".zoomBtn", function () {
    if ($(this).hasClass("zoom-out")) {
        if (ZoomValue > 1) {
            ZoomValue -= 1;
        }
    }
    else {
        if ($(this).hasClass("zoom-in")) {
            if (ZoomValue < 10) {
                ZoomValue += 1;
            }
        }
    }
    scatterPlotChart(ScatterChartData, ZoomValue);
});