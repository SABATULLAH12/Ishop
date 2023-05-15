/// <reference path="../Layout/Layout.js" />
/// <reference path="../Layout/ECommerce.js" />

var SelectedChart = "";

var sCurrentChartData = [];
var sCurrentIdentifier = 0;
var sCurrentMoreLessFact = 0;
var sFilterChange = 0;
var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
$(document).ready(function () {
    $("#btnAddToExport").click(function () {
        if ($(this).attr("chart-type") == "active")
        {
            AddToExport();
        }
    });
    $("#btnViewSelections").click(function () {
        if ($(this).attr("chart-type") == "active") {
            ShowExportList();
        }
    });
    $("#btnClearAll").click(function () {
        if ($(this).attr("chart-type") == "active") {
            ClearExportList();
        }
    });

    $(document).on("click", ".exporttoppt-logo", function () {
        if (ChartType.toLocaleLowerCase() == "table")
        {
            showMessage("Please select chart");
            return false;
        }
        if (!CheckExportChartList()) {
            showMessage("No reports to export");
            return false;
        }
        Export_To_PPT();

    });
    $(document).on("click", ".chart-type", function () {
        if (($(".adv-fltr-showhide-txt").text().toLowerCase().trim() == "show less") && ((currentpage.indexOf("tbl") > -1) || (currentpage.indexOf("chart") > -1)))
            $(".adv-fltr-showhide-txt").trigger("click");

        $("#ToShowChart").show();
        $("#guestFrqncy").hide();
        ChartType = $(this).attr("chart-name");
        if (ChartType == "Table" || ChartType == "") {
            ClearChartReports();
            $("#chart-title").hide();
            $(".ChartDivArea").css("padding-top", "30px");
        }
        else {
            $("#chart-title").show();
            $(".ChartDivArea").css("padding-top", "0px");
        }
        sRemovedLegendPosition = [];
        SelectedChart = $(this).attr("chart-name");
        if (ChartType.toLowerCase() == "table") {
            prepareContentArea();
        }
        else {
            prepareContentArea();
            data = ChartModuleData;
            $("#chart-list").css("background-image", " url(../../Images/Coke Dine_Sprite_7.svg)");
            $("#chart-list").css("background-position", " 548px -457px");
            $(".ChartDivArea").hide();
            $("#ToShowChart").show();
            $(".showChartMain").show();
            $(".ChartDivArea").getNiceScroll().resize().hide();
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
                BarChart_Stacked(data, identifier,MoreLessFact);
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
            $(".showChartMain").getNiceScroll().resize().hide();
            SetScroll($(".showChartMain"), "#393939", 0, 0, 0, 0, 8);
        }
    });
    $(document).on("click", ".spLegend", function () {
        var i = $(this).attr("position");
        sRemovedLegendPosition = [];
        $('#spChartLegend div:visible:graylegend').each(function (i, j) {
            sRemovedLegendPosition.push($(j).parent().attr("position"))
        });
        if ($(this).find(".spIcon").css("background-color") != "gray" && $(this).find(".spIcon").css("background-color") != "rgb(128, 128, 128)")
            sRemovedLegendPosition.push($(this).attr("position"));
        else
        {
            var index = sRemovedLegendPosition.indexOf($(this).attr("position").toString());
            if (index > -1) {
                sRemovedLegendPosition.splice(index, 1);
            }
        }
        var xAxisCount = sRemovedLegendPosition;
        sRemovedLegendPosition = sRemovedLegendPosition.sort(sortNumber);
        var sdata = jQuery.extend(true, {}, sCurrentChartData); ;
        switch (SelectedChart) {
            case "Stacked Column": {
                
                var cdata = [];
                if (sCurrentIdentifier == 1)
                {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--) 
                         valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //BrandList
                    valuesArr = sdata.BrandList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                         valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.BrandList = valuesArr;
                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;
                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                         valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                else {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr =[];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                         valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //MetricList
                    valuesArr = sdata.MetricList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                         valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.MetricList = valuesArr;

                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                            valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;

                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                         valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                         valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                
                columnChartStacked(sdata, '#idtrendChartMain4', sCurrentIdentifier);
                break;
                //plotLegends(sCurrentChartData, sCurrentIdentifier, colorForLegends);
            };
            case "Stacked Bar": {
                var cdata = [];
                if (sCurrentIdentifier == 1) {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //BrandList
                    valuesArr = sdata.BrandList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.BrandList = valuesArr;
                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;
                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                else {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //MetricList
                    valuesArr = sdata.MetricList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.MetricList = valuesArr;

                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;

                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }

                BarChartStacked(sdata, '#idtrendChartMain4', sCurrentIdentifier, sCurrentMoreLessFact);
                break;
            };
            case "Clustered Column": {
                var cdata = [];
                if (sCurrentIdentifier == 1) {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) {
                            k.splice(removeValFromIndex[i], 1);
                        });
                    sdata.ChangeVsPy = valuesArr;
                    //BrandList
                    valuesArr = sdata.BrandList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.BrandList = valuesArr;
                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.BrandList.length - 1; i++)
                        for (var j = 0; j <= sdata.BrandList.length - 1; j++)
                            if (sCurrentChartData.BrandList[i].toString() == sdata.BrandList[j].toString())
                                valuesArr.push(sdata.BrandList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;
                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ValueData = valuesArr;
                }
                else {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //MetricList
                    valuesArr = sdata.MetricList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.MetricList = valuesArr;

                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;

                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                plotBarChartForColumnChartOne(sdata, "Percent", 8, "Percentage", true, '#idtrendChartMain4', sCurrentIdentifier);
                break;
            };
            case "Clustered Bar": {
                var cdata = [];
                if (sCurrentIdentifier == 1) {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ChangeVsPy = valuesArr;
                    //BrandList
                    valuesArr = sdata.BrandList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.BrandList = valuesArr;
                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.BrandList.length - 1; i++)
                        for (var j = 0; j <= sdata.BrandList.length - 1; j++)
                            if (sCurrentChartData.BrandList[i].toString() == sdata.BrandList[j].toString())
                                valuesArr.push(sdata.BrandList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;
                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ValueData = valuesArr;
                }
                else {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //MetricList
                    valuesArr = sdata.MetricList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.MetricList = valuesArr;

                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;

                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                plotBarChartForColumnChartOne_NewForStackedBar(sdata, "Percent", 8, "Percentage", true, '#idtrendChartMain4', sCurrentIdentifier);
                break;
            };
            case "Bar with Change": {
                var cdata = [];
                if (sCurrentIdentifier == 1) {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ChangeVsPy = valuesArr;
                    //BrandList
                    valuesArr = sdata.BrandList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.BrandList = valuesArr;
                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.BrandList.length - 1; i++)
                        for (var j = 0; j <= sdata.BrandList.length - 1; j++)
                            if (sCurrentChartData.BrandList[i].toString() == sdata.BrandList[j].toString())
                                valuesArr.push(sdata.BrandList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;
                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ValueData = valuesArr;
                }
                else {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //MetricList
                    valuesArr = sdata.MetricList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.MetricList = valuesArr;

                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;

                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                plotBarChartForColumnChartOne_NewFor_Bar_With_Change_StackedBar(sdata, "Percent", 8, "Percentage", true, '#idtrendChartMain4', sCurrentIdentifier);
                break;
            };
            case "Line": {
                var cdata = [];
                if (sCurrentIdentifier == 1) {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ChangeVsPy = valuesArr;
                    //BrandList
                    valuesArr = sdata.BrandList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.BrandList = valuesArr;
                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.BrandList.length - 1; i++)
                        for (var j = 0; j <= sdata.BrandList.length - 1; j++)
                            if (sCurrentChartData.BrandList[i].toString() == sdata.BrandList[j].toString())
                                valuesArr.push(sdata.BrandList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;
                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ValueData = valuesArr;
                }
                else {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //MetricList
                    valuesArr = sdata.MetricList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.MetricList = valuesArr;

                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;

                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                plotLineChartOne(sdata, '#idtrendChartMain00', sCurrentIdentifier);
                break;
            };
        }
        
        if (SelectedChart == "Clustered Column") {
            if ($('#spChartLegend .spLegend').length == xAxisCount.length) $('g[class="x axis"]').css("display", "none");
            else $('g[class="x axis"]').css("display", "block");
            xAxisCount =[];
        }
        else if (SelectedChart == "Clustered Bar" || SelectedChart == "Bar with Change") {
            if ($('#spChartLegend .spLegend').length == xAxisCount.length) $('g[class="y axis"]').css("display", "none");
            else $('g[class="y axis"]').css("display", "block");
            xAxisCount = [];
        }
        var i = $(this).attr("position");
        if ($(this).find(".spIcon").css("background-color") != "gray" && $(this).find(".spIcon").css("background-color") != "rgb(128, 128, 128)")
            $(this).find(".spIcon").css("background-color", "gray");
        else
            $(this).find(".spIcon").css("background-color", colorForLegends[i]);
        //sRemovedLegendPosition = [];
    });

    $(document).on("click", ".spLegendTrend", function () {
        var i = $(this).attr("position");
        sRemovedLegendPosition = [];
        $('#spChartLegend div:visible:graytext').each(function (i, j) {
            sRemovedLegendPosition.push($(j).parent().attr("position"))
        });
        if ($(this).find(".spTextTrend").css("color") != "gray" && $(this).find(".spTextTrend").css("color") != "rgb(128, 128, 128)")
            sRemovedLegendPosition.push($(this).attr("position"));
        else
        {
            var index = sRemovedLegendPosition.indexOf($(this).attr("position").toString());
            if (index > -1) {
                sRemovedLegendPosition.splice(index, 1);
            }
        }
        var xAxisCount = sRemovedLegendPosition;
        sRemovedLegendPosition = sRemovedLegendPosition.sort(sortNumber);
        var sdata = jQuery.extend(true, {}, sCurrentChartData); ;
        switch (SelectedChart) {
            case "Line": {
                var cdata = [];
                if (sCurrentIdentifier == 1) {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ChangeVsPy = valuesArr;
                    //BrandList
                    valuesArr = sdata.BrandList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.BrandList = valuesArr;
                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.BrandList.length - 1; i++)
                        for (var j = 0; j <= sdata.BrandList.length - 1; j++)
                            if (sCurrentChartData.BrandList[i].toString() == sdata.BrandList[j].toString())
                                valuesArr.push(sdata.BrandList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;
                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.forEach(function (k, j) { k.splice(removeValFromIndex[i], 1); });
                    sdata.ValueData = valuesArr;
                }
                else {
                    var removeValFromIndex = sRemovedLegendPosition;
                    //ChangeVsPy
                    var valuesArr = [];
                    valuesArr = sdata.ChangeVsPy;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ChangeVsPy = valuesArr;
                    //MetricList
                    valuesArr = sdata.MetricList;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.MetricList = valuesArr;

                    //ArrayIndexList
                    valuesArr = [];
                    for (var i = 0; i <= sCurrentChartData.MetricList.length - 1; i++)
                        for (var j = 0; j <= sdata.MetricList.length - 1; j++)
                            if (sCurrentChartData.MetricList[i].toString() == sdata.MetricList[j].toString())
                                valuesArr.push(sdata.MetricList[j].toString() + ";" + i);
                    sdata.ArrayIndexList = valuesArr;

                    //SignificanceData
                    valuesArr = sdata.SignificanceData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.SignificanceData = valuesArr;
                    //ValueData
                    valuesArr = sdata.ValueData;
                    for (var i = removeValFromIndex.length - 1; i >= 0; i--)
                        valuesArr.splice(removeValFromIndex[i], 1);
                    sdata.ValueData = valuesArr;
                }
                plotLineChartOne(sdata, '#idtrendChartMain00', sCurrentIdentifier);
                break;
            };
        }
        
        var i = $(this).attr("position");
        if ($(this).find(".spTextTrend").css("color") != "gray" && $(this).find(".spTextTrend").css("color") != "rgb(128, 128, 128)")
            $(this).find(".spTextTrend").css("color", "gray");
        else
            $(this).find(".spTextTrend").css("color", "black");
        //sRemovedLegendPosition = [];
    });

    jQuery.expr[':'].graylegend = function (elem) {
        return (jQuery(elem).css('background-color') === 'rgb(128, 128, 128)' || jQuery(elem).css('background-color') === 'gray');
    };

    jQuery.expr[':'].graytext = function (elem) {
        return (jQuery(elem).css('color') === 'rgb(128, 128, 128)' || jQuery(elem).css('color') === 'gray');
    };

    $("#ExportToExcel").click(function () {
        if (!CheckExportChartList()) {
            showMessage("No reports to export");
            return false;
        }
        if (SelectedChart == "" || SelectedChart != "Table") {
            showMessage("Please select table");
            exportchartlist = "false";
            $("#UpdateProgress").hide();
            $(".TranslucentDiv").hide();
            return false;
        }
        var _profilerparams = new Object();
        //_profilerparams.TabName = tabname;
        //custom base
        if (CustomBase.length > 0) {
            if (TabType.toLocaleLowerCase() == "trips")
                _profilerparams.CustomBase_DBName = CustomBase[0].TripsDBName;
            else
                _profilerparams.CustomBase_DBName = CustomBase[0].ShopperDBName;

            _profilerparams.CustomBase_ShortName = CustomBase[0].Name;
            _profilerparams.CustomBase_UniqueId = CustomBase[0].UniqueId;
        }
        //----------->
        if (SelectedFrequencyList.length > 0) {
            _profilerparams.ShopperFrequency = SelectedFrequencyList[0].Name.replace("+", " +");
            _profilerparams.ShopperFrequencyShortName = SelectedFrequencyList[0].Name.replace("+", " +");
        }
        if (currentpage.indexOf("deepdive") > -1) {
            if ($("#pit-toggle").is(":checked")) {
                _profilerparams.ViewType = "TIME PERIODS";
            }
            else {
                _profilerparams.ViewType = "GROUPS";
            }
        }
        else {
            _profilerparams.ViewType = "RETAILERS";
        }
        _profilerparams.ActiveTab = GetCurrentSPName();
        _profilerparams.SelectedStatTest = Selected_StatTest;
        _profilerparams.TimePeriod = TimePeriod;
        _profilerparams.ShortTimePeriod = $(".timeType").val();
        _profilerparams.Comparison_DBNames = [];
        _profilerparams.Comparison_ShortNames = [];
        var sCompList = [];
        for (var i = 0; i < Comparisonlist.length; i++) {
            _profilerparams.Comparison_DBNames.push(Comparisonlist[i].DBName);
            _profilerparams.Comparison_ShortNames.push(Comparisonlist[i].Name);
            if (i != 0)
                sCompList.push(Comparisonlist[i].DBName);
        }
        _profilerparams.Benchmark = _profilerparams.Comparison_DBNames[0];
        _profilerparams.BCFullNames = _profilerparams.Comparison_ShortNames;
        _profilerparams.Comparisonlist = sCompList.join("|");
        Advanced_Filters_DBNames = [];
        Advanced_Filters_ShortNames = [];
        Advanced_Filters = [];
        //Guest advanced filters
        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].DBName);
            Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].parentName + ":-" + SelectedDempgraphicList[i].Name);
            Advanced_Filters.push(SelectedDempgraphicList[i].parentName + "|" + SelectedDempgraphicList[i].Name);
        }
        //Visits advanced filters
        for (var i = 0; i < SelectedAdvFilterList.length; i++) {
            Advanced_Filters_DBNames.push(SelectedAdvFilterList[i].DBName);
            Advanced_Filters_ShortNames.push(SelectedAdvFilterList[i].parentName + ":-" + SelectedAdvFilterList[i].Name);
            Advanced_Filters.push(SelectedAdvFilterList[i].parentName + "|" + SelectedAdvFilterList[i].Name);
        }
        _profilerparams.ShopperSegment = Advanced_Filters_DBNames.join("|");
        _profilerparams.FilterShortnames = Advanced_Filters_ShortNames.join("|");
        _profilerparams.Filters = Advanced_Filters.join("|");
        _profilerparams.ModuleBlock = "AcrossShopper";
        _profilerparams.Metric = Measurelist[0].DBName;
        var sMetricNames = "";
        var sMetric = [];
        for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
            sMetricNames += Measurelist[0].metriclist[i].Name + "|";
            sMetric.push(Measurelist[0].metriclist[i].Name);
        }
        sMetricNames = sMetricNames.substring(0, sMetricNames.length - 1);
        _profilerparams.selectedMetrics = sMetric;
        _profilerparams.ChartXValues = _profilerparams.Comparison_ShortNames;
        _profilerparams.ChartType = ChartType;
        _profilerparams.SelectedMetrics = sMetricNames;
        _profilerparams.MetricShortName = Measurelist[0].parentName.toUpperCase();
        //_profilerparams.selectedMetrics = sMetricNames;
        postBackData = "{profilerchartparams:" + JSON.stringify(_profilerparams) + "}";
        jQuery.ajax({
            type: "POST",
            url: $("#URLCharts").val() + "/DownloadTable",
            async: true,
            data: postBackData,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (!isAuthenticated(data))
                    return false;

                var localTime = new Date();
                var year = localTime.getFullYear();
                var month = localTime.getMonth() + 1;
                var date = localTime.getDate();
                var hours = localTime.getHours();
                var minutes = localTime.getMinutes();
                var seconds = localTime.getSeconds();
                // window.location.href = "Download.aspx?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                var TimeDetails = new Object();
                TimeDetails.year = year;
                TimeDetails.month = month;
                TimeDetails.date = date;
                TimeDetails.hours = hours;
                TimeDetails.minutes = minutes;
                TimeDetails.seconds = seconds;
                postBackData = "{TimeDetails:" + JSON.stringify(TimeDetails) + "}";

                window.location.href = $("#URLCharts").val() + "/DownloadChartsExcel?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;

                $("#UpdateProgress").hide();
                $(".TranslucentDiv").hide();
            },
            error: function (xhr, status, error) {
                GoToErrorPage();
            }
        });
    });
    if (ChartType == "Table" || ChartType == "") {
        $("#chart-title").hide();
        $(".ChartDivArea").css("padding-top", "30px");
        $(".ChartDivArea").css("height", "114%");
    }
    else {
        $("#chart-title").show();
        $(".ChartDivArea").css("padding-top", "0px");
        $(".ChartDivArea").css("height", "");
    }

    var zoom = document.documentElement.clientWidth / window.innerWidth;
    $(window).resize(function () {
        if (ChartType.toLocaleLowerCase() == "stacked column") {
            data = ChartModuleData;
            identifier = 0;
            columnChart_Stacked(data, identifier);
        }
    });
});

function CheckExportChartList() {
    var exportReport = false;
    var postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/CheckExportChartList",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            exportReport = content;
        },
        error: function (error) {
            GoToErrorPage();
        }
    });
    return exportReport;
}

/*------------------------------------------------ Get current sp name -------------------------------------------------*/

function GetCurrentSPName() {
    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }  
    switch (currentpage) {
        case "hdn-chart-compareretailers": {
            if (sVisitsOrGuests == Number("2")) {
                //guest or Shopper
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "general":
                    case "store imagery":
                    case "beverage details":
                        {
                            //return "sp_FactBookRespDemoAcrossShopperMainPro";
                            return "usp_ProfilerAcrossRetailerShopper";
                            break;
                        }                  
                }
            }
            else {
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "pre-shop":
                    case "in-store behaviour":
                    case "post shop & trip summary":
                        {
                            // return "sp_FactBookTripDemoAcrossTripMainPro"; 
                            return "usp_ProfilerAcrossRetailerTrip";
                            break;
                        }                  
                }
            }
            break;
        }
        case "hdn-chart-retailerdeepdive": {
            if (ModuleBlock == "PIT") {
                if (sVisitsOrGuests == Number("2")) {
                    //guest
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "general":
                        case "store imagery":
                        case "beverage details":
                            {
                                //return "sp_FactBookRespDemoWithinShopperMainPro";
                                return "usp_ProfilerWithinRetailerShopper";
                                break;
                            }                       
                    }
                }
                else {
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "pre-shop":
                        case "in-store behaviour":
                        case "post shop & trip summary":
                            {
                                //return "sp_FactBookTripDemoWithinMainPro";
                                return "usp_profilerWithinRetailerTrip";
                                break;
                            }                      
                    }
                }
            }
            else {
                if (sVisitsOrGuests == Number("2")) {
                    //guest
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "general":
                        case "store imagery":
                        case "beverage details":
                            {
                                //return "sp_FactBookRespDemoOverTimePeriodMainPro";
                                //return "usp_profilerTrendRetailerShopper";
                                return "usp_profilerTrendRetailerShopper_TRENDCHANGE";
                                break;
                            }                     
                    }
                }
                else {
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "pre-shop":
                        case "in-store behaviour":
                        case "post shop & trip summary":
                            {
                                //return "sp_FactBookTripDemoOverTimePeriodMainPro";
                                //return "usp_profilerTrendRetailerTrip";
                                return "usp_profilerTrendRetailerTrip_TRENDCHANGE";
                                break;
                            }                     
                    }
                }

            }
            break;
        }
        case "hdn-chart-beveragedeepdive": {

            if (sVisitsOrGuests == Number("2")) {
                //guest
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "general":
                    case "store imagery":
                        {
                            return "usp_ProfilerWithinBeverageShopper";                                                    
                      //return "sp_FactBookRespDemoBeverageWithinMainPro";
                            break;
                        }                
                        //case "beverage details":
                        //    {
                        //        break;
                        //    }
                }
            }
            else {
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "pre-shop":
                    case "in-store behaviour":
                    case "post shop & trip summary":
                    case "beverage details":
                        {
                            return "usp_ProfilerWithinBeveragetrip";
                            //return "sp_FactBookTripDemoBeverageWithinMainPro";
                            break;
                        }                  
                }
            }

            break;
        }
        case "hdn-chart-comparebeverages": {

            if (sVisitsOrGuests == Number("2")) {
                //guest
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "general":
                    case "store imagery":
                        {
                            //return "sp_FactBookRespDemoAcrossBeverageShopperMainPro";
                            return "usp_profilerAcrossBeverageShopper";
                            break;
                        }               
                        //case "beverage details":
                        //    {
                        //        break;
                        //    }
                }
            }
            else {
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "pre-shop":
                    case "in-store behaviour":
                    case "post shop & trip summary":
                    case "beverage details":
                        {
                            //return "sp_FactBookTripDemoAcrossBeverageTripMainPro";
                            return "usp_profilerAcrossBeveragetrip";
                            break;
                        }                  
                }
            }

            break;
        }
        case "hdn-e-commerce-chart-comparesites": {

            if (sVisitsOrGuests == Number("2")) {
                //guest
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "general":
                    case "store imagery":
                        {
                            //return "sp_FactBookRespDemoAcrossBeverageShopperMainPro";
                            return "usp_profilerEcomAcrossSiteShopper";
                            break;
                        }                  
                        //case "beverage details":
                        //    {
                        //        break;
                        //    }
                }
            }
            else {
                switch (ChartTileName.toLowerCase()) {
                    case "demographic profiling":
                    case "pre-shop":
                    case "in-store behaviour":
                    case "post shop & trip summary":
                    case "beverage details":
                        {
                            //return "sp_FactBookTripDemoAcrossBeverageTripMainPro";
                            return "usp_profilerEcomAcrossSiteTrip";
                            break;
                        }                 
                }
            }

            break;
        }
        case "hdn-e-commerce-chart-sitedeepdive": {
            if (ModuleBlock == "PIT") {
                if (sVisitsOrGuests == Number("2")) {
                    //guest
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "general":
                        case "store imagery":
                        case "beverage details":
                            {
                                //return "sp_FactBookRespDemoWithinShopperMainPro";
                                return "usp_profilerEcomWithinSiteShopper";
                                break;
                            }                      
                    }
                }
                else {
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "pre-shop":
                        case "in-store behaviour":
                        case "post shop & trip summary":
                            {
                                //return "sp_FactBookTripDemoWithinMainPro";
                                return "usp_profilerEcomWithinSiteTrip";
                                break;
                            }                      
                    }
                }
            }
            else {
                if (sVisitsOrGuests == Number("2")) {
                    //guest
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "general":
                        case "store imagery":
                        case "beverage details":
                            {
                                //return "sp_FactBookRespDemoOverTimePeriodMainPro";
                                //return "usp_profilerTrendRetailerShopper";
                                return "usp_profilerEcomTrendSiteShopper";
                                break;
                            }                      
                    }
                }
                else {
                    switch (ChartTileName.toLowerCase()) {
                        case "demographic profiling":
                        case "pre-shop":
                        case "in-store behaviour":
                        case "post shop & trip summary":
                            {
                                //return "sp_FactBookTripDemoOverTimePeriodMainPro";
                                //return "usp_profilerTrendRetailerTrip";
                                return "usp_ProfilerEcomTrendSiteTrip";
                                break;
                            }                      
                    }
                }

            }
            break;
        }
    }
}

function AddToExport() {
    var postBackData = "{ChartType:'" + ChartType +"'}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/AddChartToExport",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            if (content == "true") {
                $("#btnViewSelections").attr('chart-type', 'active');
                $("#btnClearAll").attr('chart-type', 'active');
                $("#btnViewSelections").css("background", "");
                $("#btnClearAll").css("background", "");
                setTimeout(function () {
                    $('.TranslucentDiv').css("z-index", "6001");
                    $('.TranslucentDiv').show()
                }, 50);
                showMessage("Successfully added");                
            }
            else {
                showMessage("No chart to add");
            }
        },
        error: function (error) {
            GoToErrorPage();
        }
    });
}

/*------------------------------------------------ Check export list----------------------------------------------------*/

function CheckExportList() { 
    var postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/ShowExportChartList",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async:false,
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            if (content != "") {            
                $("#exportchartlist").html("");
                $("#exportchartlist").html(content);
                $("#btnViewSelections").attr('chart-type', 'active');
                $("#btnClearAll").attr('chart-type', 'active');

                $("#btnViewSelections").css("background", "");
                $("#btnClearAll").css("background", "");
                SetScroll($("#exportchartlist"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
            }
            else {               
                $("#btnViewSelections").attr('chart-type', 'inactive');
                $("#btnClearAll").attr('chart-type', 'inactive');
                $("#btnViewSelections").css("background", "lightgray");
                $("#btnClearAll").css("background", "lightgray");
            }
        },
        error: function (error) {
            GoToErrorPage();
        }
    });   
}

function ShowExportList() {
    var ischartavailable = false;
    var postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/ShowExportChartList",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",     
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            if (content != "") {
                ischartavailable = true;             
                $("#exportchartlist").html("");
                $("#exportchartlist").html(content);
                    setTimeout(function () {
                        $('.TranslucentDiv').show()
                        }, 100);               
                $(".exportchartlistpopup").show();
            }
            else {               
                $("#btnViewSelections").attr('chart-type', 'inactive');
                $("#btnClearAll").attr('chart-type', 'inactive');
                $("#btnViewSelections").css("background", "lightgray");
                $("#btnClearAll").css("background", "lightgray");
                $('.TranslucentDiv').hide()
                $(".exportchartlistpopup").hide();
                showMessage("No charts to show");
            }
        },
        error: function (error) {
            GoToErrorPage();
        }
    });   
    return ischartavailable;
}

function ClearExportList() {
    var postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/ClearAllChartsFromExportList",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            var table = "<table style=\"color: #000000; margin-top: 40px;\" align=\"center\" valign=\"center\"><tr><td style=\"width: 100%;text-align: center; display: flex; justify-content: center; border: none;\">No charts to show</td></tr></table>";
            $("#exportchartlist").html("");
            $("#exportchartlist").html(table);
            $("#btnViewSelections").attr('chart-type', 'inactive');
            $("#btnClearAll").attr('chart-type', 'inactive');
            $("#btnViewSelections").css("background", "lightgray");
            $("#btnClearAll").css("background", "lightgray");
        },
        error: function (error) {
            GoToErrorPage();
        }
    });
}

function DeleteChartFromExportList(_chartid) {
    var postBackData = "{ChartID:'" + _chartid + "'}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/DeleteChartFromExportList",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            var table = "<table style=\"color: #000000; margin-top: 40px;\" align=\"center\" valign=\"center\"><tr><td style=\"width: 100%;text-align: center; display: flex; justify-content: center; border: none;\">No charts to show</td></tr></table>";
            if (content != "") {
                table = content;
            }
            if (content == "") {
                $("#btnViewSelections").attr('chart-type', 'inactive');
                $("#btnClearAll").attr('chart-type', 'inactive');
                $("#btnViewSelections").css("background", "lightgray");
                $("#btnClearAll").css("background", "lightgray");
            }

            $("#exportchartlist").html(table);
            showMessage("Successfully deleted");
            setTimeout(function () {
                $(".TranslucentDiv").show();
            }, 10);
        },
        error: function (error) {
            GoToErrorPage();
        }
    });
}

function CloseExportSelectionPopup() {
    $(".exportchartlistpopup").hide();
    $(".TranslucentDiv").hide();
}

/*------------------------------------------------ Export to ppt ------------------------------------------------------*/
function CheckChartReports() {
    var exportchartlist = false;
    var postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/CheckChartReports",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            exportchartlist = content;
        },
        error: function (error) {
            GoToErrorPage();
        }
    });
    return exportchartlist;
}

function Export_To_PPT() {
    debugger;
    var exportchartlist = true;
    //ExportToPPT

    var postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLCharts").val() + "/CheckExportChartList",
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            if (exportchartlist == false) {
                showMessage("No charts to export");
                return;
            }
            var localTime = new Date();
            var year = localTime.getFullYear();
            var month = localTime.getMonth() + 1;
            var date = localTime.getDate();
            var hours = localTime.getHours();
            var minutes = localTime.getMinutes();
            var seconds = localTime.getSeconds();
            window.location.href = $("#URLCharts").val() + "/" + "ExportToPPT?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
        },
        error: function (error) {
            GoToErrorPage();
        }
    });

}

var defs = '<defs><linearGradient id="sampleGradient" x1="0%" y1="0%" x2="0%" y2="100%"><stop offset="0%" stop-color="rgba(0, 0, 0, 0.3)" /><stop offset="100%" stop-color="transparent" /></linearGradient><linearGradient id="sampleYGradient" x1="0%" y1="0%" x2="100%" y2="0%"><stop offset="0%" stop-color="rgba(0, 0, 0, 0.3)" /><stop offset="100%" stop-color="transparent" /></linearGradient><linearGradient id="sampleGradientNew" x1="0%" y1="0%" x2="0%" y2="100%"><stop offset="0%" stop-color="transparent" /><stop offset="100%" stop-color="rgba(0, 0, 0, 0.3)" /></linearGradient><linearGradient id="shadowGradient" x1="0%" y1="0%" x2="0%" y2="100%"><stop stop-color="rgbdebua(148, 148, 148, 1)" offset="0%" /><stop stop-color="rgba(171, 171, 171, 1)" offset="9.41%" /><stop stop-color="rgba(201, 201, 201, 1)" offset="24.76%" /><stop stop-color="rgba(225, 225, 225, 1)" offset="40.92%" /><stop stop-color="rgba(242, 242, 242, 1)" offset="58.03%" /><stop stop-color="rgba(252, 252, 252, 0.4)" offset="76.72%" /><stop stop-color="rgba(252, 252, 252, 0.3)" offset="100%" /></linearGradient></defs>';


/*------------------------------------------------ clustered column ----------------------------------------------------*/

function GoToChartFunctionForColumnChartOne(data, identifier) {
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain4').css('display', 'block');
    //$('#idtrendChartMain4').css('height', '107%');
    SelectedChart = "Clustered Column";
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    plotBarChartForColumnChartOne(data, "Percent", 8, "Percentage", true, '#idtrendChartMain4', identifier);
    plotLegends(data, identifier, colorForLegends);
}

function plotBarChartForColumnChartOne(data, metricType, noOfTicks, yLabel, barShadow, id, identifier) {
    if (data.SampleSize != undefined && data.SampleSize != "" && data.SampleSize.length > 0)
        data.SampleSize = CheckNegativeSampleSize(data.SampleSize);

    _.forEach(data.BrandList, function (i, j) { data.BrandList[j] = i.trim() });
    _.forEach(data.MetricList, function (i, j) { data.MetricList[j] = i.trim() });

    if (Selected_StatTest.toLocaleLowerCase().trim() == "custom base" || Sigtype_Id == "1") {
        //Sorting Data
        var sCompList = [];
        if (currentpage.indexOf("deepdive") > -1) {
            if (Grouplist.length > 0)
                $.grep(Grouplist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        else {
            if (Comparisonlist.length > 0)
                $.grep(Comparisonlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (ComparisonBevlist.length > 0)
                $.grep(ComparisonBevlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (Sites.length > 0)
                $.grep(Sites, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        var sIndex = [];
        _.each(sCompList, function (i, j) {
            _.each(data.BrandList, function (k, l) { if (i.toLocaleLowerCase().trim() == k.toLocaleLowerCase().trim()) sIndex.push(l); })
        }
        )

        data.BrandList = getSorted(data.BrandList, sIndex);
        var temp = [];
        _.each(data.ChangeVsPy, function (i, j) {
            temp.push(getSorted(i, sIndex));
        });
        data.ChangeVsPy = temp;
        data.NumberOfResponses = getSorted(data.NumberOfResponses, sIndex);
        data.SampleSize = getSorted(data.SampleSize, sIndex);
        temp = [];
        _.each(data.SignificanceData, function (i, j) {
            temp.push(getSorted(i, sIndex));
        });
        data.SignificanceData = temp;
        temp = [];
        _.each(data.ValueData, function (i, j) {
            temp.push(getSorted(i, sIndex));
        });
        data.ValueData = temp;

        //End Sorting Data
    }
    d3.select(id).select("svg").remove();
    $(id).empty();
    var color = d3.scale.ordinal()
                .range(["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"]);
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    var colorForTopRect = d3.scale.ordinal()
                .range(["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"]);
    var colorForTopRectVal = ["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"];
    //var count = 6;
    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';
    //resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");

    //use this height and width for entire chart and adjust here with svg traslation
    //height = parseFloat($(id).height() - 120);
    //width = parseFloat($(id).width() - 100);
    //To Set width and height of the svg
    var each_grp_len = (data.ValueData.length - 3) / 2;
    var NoOfElements = data.length * each_grp_len;

    $(id).css("overflow", "hidden");

    //var NoOfElements = data.BrandList.length * data.ValueData.length;
    var Check_BrandList = data.BrandList.length;
    //var factor = 60, fctr4mrgn = 40;
    //var margin = { top: 80, right: 20, bottom: 100, left: fctr4mrgn };
    //if (NoOfElements < 10) { factor = 100; fctr4mrgn = NoOfElements * 30; }
    //if (NoOfElements < 5) { factor = 150; fctr4mrgn = NoOfElements * 70; }

    //if (Check_BrandList == 1) {
    //    factor = 190; fctr4mrgn = NoOfElements * 30 + 30;
    //    //$('.showChartMain').css("overflow-x", "hidden");
    //    //$('.showChartMain').css("overflow-y", "hidden");
    //}
    //if (Check_BrandList == 2) {
    //    factor = 100; fctr4mrgn = NoOfElements * 20 + 30;
    //    //$('.showChartMain').css("overflow-x", "hidden");
    //    //$('.showChartMain').css("overflow-y", "hidden");
    //}
    //if (NoOfElements > 12) {
    //    //$('.showChartMain').css("overflow-x", "auto"); $('.showChartMain').css("overflow-y", "hidden")
    //}
    //var dynamic_width_caliberation = factor * NoOfElements,


    var margin = { top: 0, right: 10, bottom: 60, left: 10 },
    width = $(id).width() - margin.left - margin.right, //NoOfElements*70
    height = $(id).height() - margin.top - margin.bottom;
    var x0;

    //width = dynamic_width_caliberation - margin.left - margin.right,
    // height = 360 - margin.top - margin.bottom;
    //height = parseFloat($(id).height() - 50);

    //var x0 = d3.scale.ordinal()
    //.rangeRoundBands([0, width], .15);

    //var x1 = d3.scale.ordinal();

    //var y = d3.scale.linear()
    //    .range([height, 0]);
   

    //Append the div for tooltip
    var divTooltip = d3.select(".showChartMain").append("div")
        .attr("class", "d3_tooltip")
        .style("opacity", 0);

    //var svg = d3.select(id).append("svg")
    //    .attr("width", width + margin.left + margin.right)
    //    .attr("height", "99%")
    //    .append("g")
    //    //.attr("transform", "translate(40,-20)")
    //    .attr("transform", "translate(0,-20)")
    //    //.attr("width", width + margin.left + margin.right)
    //    .attr("height", "95%");


    x0 = d3.scale.ordinal().rangeRoundBands([0, width], .05);
    var x1 = d3.scale.ordinal();

    var y = d3.scale.linear()
        .range([height * 0.95, 0]);

    var xAxis = d3.svg.axis()
        .scale(x0)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .tickFormat(d3.format(".2s"));
    ///////////////////////////////////////////////////////////
    var viewBoxWidth = $("#idtrendChartMain4").width();
    var viewBoxHeight = $("#idtrendChartMain4").height();

    var svg = d3.select(id)
        .classed("svg-container", true) //container class to make it responsive
        .append("svg")
        .attr("preserveAspectRatio", "xMidYMid meet")
   //.attr("viewBox", "0 0 1280 370")
        //.attr("viewBox", "0 0 20% 20%")
    .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
   //class to make it responsive
   .classed("svg-content-responsive", true)
        //.attr("width", width + margin.left + margin.right)
        //.attr("height", height + margin.top + margin.bottom)
        .attr("width", "100%")
        .attr("height", "100%")
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    svg.html(defs);


    var xvalues = [];
    var dataArray = [];
    var cumulative = 0;
    var start = '';
    var end = '';
    var xStart = [, ];
    var countt = 1;

    //For Measure :-EDUCATION and Socio Economic
    if (identifier == 1) {
        $.each(data.MetricList, function (i, v) {
            xvalues.push(v);
        });
        for (var i = 0; i < xvalues.length; i++) {
            var obj = {};
            obj.name = xvalues[i];
            $.each(data.ValueData[i], function (j, v) {
                var tempName = data.BrandList[j] + " (" + addCommas(data.SampleSize[j]) + ")";//"val" + (j + 1);
                if (CheckIfStoreFrequencyMeasure(data.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                    obj[tempName] = "0.0";
                else
                    obj[tempName] = v;
                obj["Significance"+j] = data.SignificanceData[i][j];
                obj["customBase" + j] = data.BrandList[j];
                obj["samplesize" + j] = data.NumberOfResponses[j];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = [];
                    obj.index = data.ArrayIndexList;
                }
            });           
            dataArray.push(obj);
        }
    }

    //For other Measures
    else {
        $.each(data.BrandList, function (i, v) {
            xvalues.push(v);
        });
        for (var i = 0; i < xvalues.length; i++) {
            var obj = {};
            obj.name = xvalues[i];
            for (var j = 0; j < data.ValueData.length; j++) {
                var tempName = data.MetricList[j]; //"val" + (j + 1);
                if (CheckIfStoreFrequencyMeasure(data.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                    obj[tempName] = "0.0";
                else
                    obj[tempName] = data.ValueData[j][i];
                //obj[tempName] = data.ValueData[j][i];
                obj["samplesize"] = data.NumberOfResponses[j];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = [];
                    obj.index = data.ArrayIndexList;
                }
            }
            dataArray.push(obj);
        }
    }
    var yearNames = d3.keys(dataArray[0]).filter(function (key) {
        return key != "name" && !(key.indexOf("Significance") > -1) && !(key.indexOf("customBase") > -1) && !(key.indexOf("index") > -1) && !(key.indexOf("samplesize") > -1);
    });
    dataArray.forEach(function (d) {
        d.yearValues = yearNames.map(function (name, i) { return { name: name, value: +d[name], Significance: d["Significance" + i], index: d["index"], customBase: d["customBase" + i], samplesize: d["samplesize" + i], legendName: d.name }; });
    });

    if (dataArray.length > 0) {
        x0.domain(dataArray.map(function (d) { return d.name; }));
        x1.domain(yearNames).rangeRoundBands([0, x0.rangeBand()]);
        y.domain([0, d3.max(dataArray, function (d) {
            var val = ((18.8 * d3.max(dataArray, function (d) { return d3.max(d.yearValues, function (d) { return d.value; }); })) / 100) + d3.max(dataArray, function (d) { return d3.max(d.yearValues, function (d) { return d.value; }); });
            return val;
        })]);
    }

    //var xAxis = d3.svg.axis()
    //   .scale(x0)
    //   .tickSize(0)
    //   .tickPadding(15)
    //   .innerTickSize(0)
    //   .orient("bottom");

    //var yAxis = d3.svg.axis()
    //    .scale(y)
    //    .orient("left")
    //    .innerTickSize(-width);

    //if (data.BrandList.length == 1 && data.MetricList.length > 1) {
    //    svg.append("g")
    //    .attr("class", "x axis")
    //    .attr("transform", "translate(-17," + height + ")")
    //    //    .attr("transform", "translate(0," + height + ")")
    //    .call(xAxis)
    //    .selectAll(".x.axis path")
    //    .attr("display", "none");

    //    svg.selectAll(".axis path,.axis line")
    //    .style("fill", "none")
    //    .style("stroke", "#303030")
    //    .style("shape-rendering", "crispEdges");

    //    d3.selectAll(".tick text")
    //        .style("fill", "#303030")
    //        .style("font", "contentFontFamily")
    //        .style("font-size", "10px")
    //        .style("font-weight", "bold")
    //        .style("text-anchor", "middle")
    //        .attr("transform", "translate(5,0)")
    //       // .call(wrap, x0.rangeBand());
    //    .call(wrap, x0.rangeBand() + 10);
    //}
    //else if (data.BrandList.length == 1 && data.MetricList.length == 1) {
    //    svg.append("g")
    //    .attr("class", "x axis")
    //    .attr("transform", "translate(15," + height + ")")
    //    //      .attr("transform", "translate(0," + height + ")")
    //    .call(xAxis)
    //    .selectAll(".x.axis path")
    //    .attr("display", "none");

    //    svg.selectAll(".axis path,.axis line")
    //    .style("fill", "none")
    //    .style("stroke", "#303030")
    //    .style("shape-rendering", "crispEdges");

    //    d3.selectAll(".tick text")
    //        .style("fill", "#303030")
    //        .style("font", "contentFontFamily")
    //        .style("font-size", "10px")
    //        .style("font-weight", "bold")
    //        .style("text-anchor", "middle")
    //        .attr("transform", "translate(5,0)")
    //    //.call(wrap, x0.rangeBand());
    //    .call(wrap, x0.rangeBand() + 10);
    //}
    //else 
    {
        //svg.append("g")
        //    .attr("class", "x axis")
        //    .attr("transform", "translate(0," + height + ")")
        //    //  .attr("transform", "translate(0," + height + ")")
        //    .call(xAxis)
        //    .selectAll(".x.axis path")
        //    .attr("display", "none");

        //svg.selectAll(".axis path,.axis line")
        //.style("fill", "none")
        //.style("stroke", "#303030")
        //.style("shape-rendering", "crispEdges");

        //d3.selectAll(".tick text")
        //    .style("fill", "#303030")
        //    .style("font", "contentFontFamily")
        //    .style("font-size", "10px")
        //    .style("font-weight", "bold")
        //    .style("text-anchor", "middle")
        //    .attr("transform", "translate(5,0)")
        //// .call(wrap, x0.rangeBand());
        //.call(wrap, x0.rangeBand() + 10);
        svg.append("g")
       .attr("class", "x axis")
       .attr("transform", "translate(0," + (height - 4) + ")")
       .call(xAxis)
            .selectAll(".x.axis path")
        .attr("display", "none")
       .selectAll("text")
       .attr("class", "xtext")
       .style("text-anchor", "end")
       .attr("dx", "0em")
       .attr("dy", ".15em")
       .style("font-size", "10px")
       .style("text-anchor", "middle")
        .call(wrap, x0.rangeBand() + 10);
        
        //svg.selectAll(".x.axis .tick text")
        //    .style("font-family", "Arial")
        //    .style("font-size", "11px")
        //    .call(verticalWrap, 100);

        svg.append("g")
            .attr("class", "y axis")
            .style("fill", "transparent")
            .style("stroke", "black")
            .style("stroke-width", "0")
            .call(yAxis)
    }


    

    //var gap = 10;
    //var wdt = width - (gap * data.BrandList.length * data.ValueData.length) - margin.left - margin.right;  //42;
    //var wdt = 42;

    var gap = 10;
    var wdt = x1.rangeBand();

    //var xShift = wdt / 3.9;
    //if (Check_BrandList == 1 || Check_BrandList == 2) {
    //    xShift = factor / 4;
    //}
    //else {
    //    xShift = wdt / 3.9;
    //}
    
    var state = svg.append("g")
        .selectAll(".name")
        .data(dataArray)
        .enter().append("g")
        .attr("class", function (d, i) { return "name " + d.name; })
        .attr("transform", function (d) { return "translate(" + x0(d.name) + ",0)"; });

    height = height - 10;
    cummul_width = 0;

    //state.selectAll("rect")
    //   .data(function (d) { return d.yearValues; })
    //   .enter().append("rect")
    //   .attr("class", "rectOfEachNameForInner")
    //   .attr("width", wdt - gap)
    //   .attr("x", function (d) { return x1(d.name) + xShift; })
    //   .attr("y", function (d) { return y(d.value); })
    //   .attr("height", function (d) { return (height - y(d.value))<0 ? 0 : (height - y(d.value)); })
    //   .style("stroke", function (d) { return color(d.name); })
    //   .style("stroke-width", "1")
    //   .style("fill", "none");
    
    state.selectAll("rectBig")
        .data(function (d) { return d.yearValues; })
        .enter().append("g").append("rect")
        .attr("class", "rectOfEachName")
        //.attr("width", wdt - 6 - gap)
        .attr("width", (wdt - gap) <= 0 ? 0.5 : (wdt - gap))
        .attr("x", function (d) { return x1(d.name); })
        .attr("y", function (d) { return y(d.value) - 7; })
        .attr("height", function (d, i) {
            //Append the small lines in each rect
            var context = d3.select(this.parentNode);
            ///*SemiRectangle*/
            //context.append("rect")
            //    .attr("class", "dualInverse")
            //    .attr("fill", "url(#sampleGradientNew)")
            //    .attr("width", (wdt - gap) / 2 - 3)
            //    .attr("x", x1(d.name) + xShift + gap - 6)
            //    .attr("y", y(d.value) + 4)
            //    .attr("height", (height - y(d.value) - 3) < 0 ? 0 : (height - y(d.value) - 3))
            //    .on("mousemove", function () {
            //        divTooltip.style("left", d3.event.pageX - 50 + "px");
            //        divTooltip.style("top", d3.event.pageY - 90 + "px");
            //        divTooltip.style("display", "inline-block");
            //        divTooltip.style("opacity", 1);
            //        var x = d3.event.pageX, y = d3.event.pageY
            //        var elements = document.querySelectorAll(':hover');
            //        l = elements.length
            //        l = l - 1
            //        elementData = elements[l].__data__
            //        divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //    })
            //    .on("mouseout", function (d) {
            //        divTooltip.style("display", "none");
            //        divTooltip.style("opacity", 0);
            //    });
            ///*SemiRectangle*/
            ///*Lines*/
            //var hgh = Math.floor((height - y(d.value) - 2) / 6);
            //for (var i = 0; i < hgh; i++) {
            //    context.append("rect")
            //    .attr("class", "LineRect")
            //    .attr("width", wdt - 10 - gap)
            //    .attr("x", x1(d.name) + xShift + 5)
            //    .attr("y", (y(d.value) + 8 + (i * 6)))
            //    .attr("height", 1)
            //    .style("fill", "black")
            //    .on("mousemove", function () {
            //        divTooltip.style("left", d3.event.pageX - 50 + "px");
            //        divTooltip.style("top", d3.event.pageY - 90 + "px");
            //        divTooltip.style("display", "inline-block");
            //        divTooltip.style("opacity", 1);
            //        var x = d3.event.pageX, y = d3.event.pageY
            //        var elements = document.querySelectorAll(':hover');
            //        l = elements.length
            //        l = l - 1
            //        elementData = elements[l].__data__
            //        divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //    })
            //    .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //        });
            //}
            //var radius = (wdt - gap) / 2,
            //    startAngle = (-90 * (Math.PI / 180)),
            //    endAngle = (-270 * (Math.PI / 180));

            context.append("rect")
            .attr("class","rectAtMiddle")
                //.attr("x1", x1(d.name) + xShift + (wdt - gap) / 2)
                //.attr("y2", -10)               
                //.attr("x2", x1(d.name) + xShift + (wdt - gap) / 2)
                //.attr("y2", (y(d.value)))
                //.style("stroke-dasharray", ("3, 3"))
                //.style("stroke-opacity", 1)
                //.style("stroke", "black")
                        .attr("x", x1(d.name) + (wdt - gap) / 2)
                        .attr("y", y(d.value) - 7)
                        .attr("height", (d.value ==0 ? 0 : (height - y(d.value)) < 0 ? 0 : (height - y(d.value) + 7)))
                        .style("opacity", 0.4)
                .style("fill", function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForTopRectVal[d.index[i].split(';')[1]];
                    }
                    else
                        return colorForTopRect(d.name);
                })
                .on("mousemove", function () {
                    divTooltip.style("left", d3.event.pageX - 50 + "px");
                    divTooltip.style("top", d3.event.pageY - 90 + "px");
                    divTooltip.style("display", "inline-block");
                    divTooltip.style("opacity", 1);
                    var x = d3.event.pageX, y = d3.event.pageY
                    var elements = document.querySelectorAll(':hover');
                    l = elements.length
                    l = l - 1
                    elementData = elements[l].__data__
                    if (elementData == undefined)
                        divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                    else
                        divTooltip.html((d.name) + "<br>" + elementData.legendName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                })
                .on("mouseout", function (d) {
                    divTooltip.style("display", "none");
                    divTooltip.style("opacity", 0);
                });

            
           
            context.append("text")
                .attr("class", "TextForRect")
                //.attr("x", cx)
                //.attr("y", -(radius))
                .attr("x", x1(d.name) + (wdt - gap) / 2 + 4)//12 is the size of text
                .attr("y", d.value == 0 ? y(d.value) - 5 + 7 : y(d.value) - 5 - 7)
                .text(function (i) {
                    if (CheckIfStoreFrequencyMeasure(i.name.split('(')[0].trim()) && Measurelist[0].metriclist.length > 0)
                        return 'NA';
                    else
                        return (d.value.toFixed(1)) + '%';
                })
                .style("font-size", "12px")
                .style("font-weight", "bold")
                .style("text-anchor", "middle")
                .style("fill", Get_Significance_Color(d.Significance, d.customBase, d.samplesize))
            .on("mousemove", function () {
                divTooltip.style("left", d3.event.pageX - 50 + "px");
                divTooltip.style("top", d3.event.pageY - 90 + "px");
                divTooltip.style("display", "inline-block");
                divTooltip.style("opacity", 1);
                var x = d3.event.pageX, y = d3.event.pageY
                var elements = document.querySelectorAll(':hover');
                l = elements.length
                l = l - 1
                elementData = elements[l].__data__
                if (elementData == undefined)
                    divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                else
                    divTooltip.html((d.name) + "<br>" + elementData.legendName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
            })
            .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });
            /*Text above circle*/
            /*Rect at top of each bar*/
            context.append("rect")
                .attr("width", wdt + 6 - gap)
                        .attr("x", x1(d.name) - 3)
                        .attr("y", height + 5)
                //.attr("height", d.value <= 0 ? 0 : 3)
                .attr("height", 3)
                .style("fill",
                function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForTopRectVal[d.index[i].split(';')[1]];
                    }
                    else
                        return colorForTopRect(d.name);
                })
                .on("mousemove", function () {
                                divTooltip.style("left", d3.event.pageX - 50 + "px");
                                divTooltip.style("top", d3.event.pageY - 90 + "px");
                                divTooltip.style("display", "inline-block");
                                divTooltip.style("opacity", 1);
                                var x = d3.event.pageX, y = d3.event.pageY
                                var elements = document.querySelectorAll(':hover');
                                l = elements.length
                                l = l - 1
                                elementData = elements[l].__data__
                                if (elementData == undefined)
                                    divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                                else
                                    divTooltip.html((d.name) + "<br>" + elementData.legendName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                            })
                .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                });
            /*Rect at top of each bar*/
            ///*Rectangular Points*/
            //context.append("rect")
            //    .attr("width", 5)
            //    .attr("x", x1(d.name) + xShift + wdt - 3 - gap)
            //    .attr("y", height - 3)
            //    .attr("height", 5)
            //    .style("fill", color(d.name))
            //    .on("mousemove", function () {
            //                    divTooltip.style("left", d3.event.pageX - 50 + "px");
            //                    divTooltip.style("top", d3.event.pageY - 90 + "px");
            //                    divTooltip.style("display", "inline-block");
            //                    divTooltip.style("opacity", 1);
            //                    var x = d3.event.pageX, y = d3.event.pageY
            //                    var elements = document.querySelectorAll(':hover');
            //                    l = elements.length
            //                    l = l - 1
            //                    elementData = elements[l].__data__
            //                    divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //                })
            //    .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //    });

            //context.append("rect")
            //    .attr("width", 5)
            //    .attr("x", x1(d.name) + xShift - 2)
            //    .attr("y", height - 3)
            //    .attr("height", 5)
            //    .style("fill", color(d.name))
            //    .on("mousemove", function () {
            //            divTooltip.style("left", d3.event.pageX - 50 + "px");
            //            divTooltip.style("top", d3.event.pageY - 90 + "px");
            //            divTooltip.style("display", "inline-block");
            //            divTooltip.style("opacity", 1);
            //            var x = d3.event.pageX, y = d3.event.pageY
            //            var elements = document.querySelectorAll(':hover');
            //            l = elements.length
            //            l = l - 1
            //            elementData = elements[l].__data__
            //            divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //        })
            //    .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //        });
            ///*Rectangular Points*/
            /*Rectangular Plot at bottom*/
            context.append("rect")
                .attr("width", wdt - gap)
                .attr("x", x1(d.name))
                .attr("y", y(d.value) - 10)
                .attr("height", (d.value == 0 ? 0 : (height - y(d.value) + 10) == 0 ? 0 : 3))
                .style("fill", function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForLegends[d.index[i].split(';')[1]];
                    }
                    else
                        return color(d.name);
                })
                .on("mousemove", function () {
                        divTooltip.style("left", d3.event.pageX - 50 + "px");
                        divTooltip.style("top", d3.event.pageY - 90 + "px");
                        divTooltip.style("display", "inline-block");
                        divTooltip.style("opacity", 1);
                        var x = d3.event.pageX, y = d3.event.pageY
                        var elements = document.querySelectorAll(':hover');
                        l = elements.length
                        l = l - 1
                        elementData = elements[l].__data__
                        if (elementData == undefined)
                            divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                        else
                            divTooltip.html((d.name) + "<br>" + elementData.legendName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                    })
                .on("mouseout", function (d) {
                divTooltip.style("display", "none");
                divTooltip.style("opacity", 0);
            });
            /*Rectangular Plot at bottom*/
            if (d.value == 0)
                return 0;
            else;
            return (height - y(d.value)) < 0 ? 0 : (height - y(d.value) + 9);
        })
        .style("fill", function (d,i) {
        if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
            return colorForLegends[d.index[i].split(';')[1]];
        }
            else
            return color(d.name);
        })
                .on("mousemove", function (d,i) {
                    var sample = $(this).parent().attr('class');
                    if (sample != undefined)
                        sampleText = sample.substr(sample.indexOf(' ') + 1, sample.length)
                    else
                        sampleText = "";
                        divTooltip.style("left", d3.event.pageX - 50 + "px");
                        divTooltip.style("top", d3.event.pageY - 90 + "px");
                        divTooltip.style("display", "inline-block");
                        divTooltip.style("opacity", 1);
                        var x = d3.event.pageX, y = d3.event.pageY
                        var elements = document.querySelectorAll(':hover');
                        l = elements.length
                        l = l - 1
                        elementData = elements[l].__data__
                        if (elementData == undefined)
                            divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                        else
                            divTooltip.html((d.name) + "<br>" + elementData.legendName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize)  + ";\">" + elementData.value.toFixed(1) + "%" + "</div>");//(d.name) + "<br>" +
                    })
                .on("mouseout", function () {
                                divTooltip.style("display", "none");
                                divTooltip.style("opacity", 0);
                });
    
    var element = document.getElementsByClassName(data.MetricList[0].split(' ')[0]);
    var width = element[0].getBoundingClientRect().width;
    svg.selectAll(".x.axis .tick text")
           .style("font-family", "Arial")
           .style("font-size", "11px")
           .call(verticalWrap, width - 20);

    var xaxiselement = document.getElementsByClassName("x axis");
    var xaxisheight = xaxiselement[0].getBoundingClientRect().height;
    if (xaxisheight > 64) {
        $("#idtrendChartMain4").css("height", "115%");
        var viewBoxWidth1 = $("#idtrendChartMain4").width();
        var viewBoxHeight1 = $("#idtrendChartMain4").height();
        var svgelement = document.getElementsByClassName("svg-content-responsive");
        svgelement[0].setAttribute("viewBox", "0 0 " + viewBoxWidth1 + " " + viewBoxHeight1 + "");
    }
    else {
        $("#idtrendChartMain4").css("height", "100%")
    }

    sRemovedLegendPosition = [];
}

/*------------------------------------------------ clustered bar ------------------------------------------------------*/
function GoToChartFunctionForColumnChartOne_NewForStackedBar(data, identifier) {  
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain4').css('display', 'block');
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    plotBarChartForColumnChartOne_NewForStackedBar(data, "Percent", 8, "Percentage", true, '#idtrendChartMain4', identifier);
    plotLegends(data, identifier, colorForLegends);
}

function plotBarChartForColumnChartOne_NewForStackedBar(data, metricType, noOfTicks, yLabel, barShadow, id, identifier) {
    if (data.SampleSize != undefined && data.SampleSize != "" && data.SampleSize.length > 0)
        data.SampleSize = CheckNegativeSampleSize(data.SampleSize);

    _.forEach(data.BrandList, function (i, j) { data.BrandList[j] = i.trim() });
    _.forEach(data.MetricList, function (i, j) { data.MetricList[j] = i.trim() });

    if (Selected_StatTest.toLocaleLowerCase().trim() == "custom base" || Sigtype_Id == "1") {
        //Sorting Data
        var sCompList = [];
        if (currentpage.indexOf("deepdive") > -1) {
            if (Grouplist.length > 0)
                $.grep(Grouplist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        else {
            if (Comparisonlist.length > 0)
                $.grep(Comparisonlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (ComparisonBevlist.length > 0)
                $.grep(ComparisonBevlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (Sites.length > 0)
                $.grep(Sites, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        var sIndex = [];
        _.each(sCompList, function (i, j) {
            _.each(data.BrandList, function (k, l) { if (i.toLocaleLowerCase().trim() == k.toLocaleLowerCase().trim()) sIndex.push(l); })
        }
        )

        data.BrandList = getSorted(data.BrandList, sIndex);
        var temp = [];
        _.each(data.ChangeVsPy, function (i, j) {
            temp.push(getSorted(i, sIndex));
        });
        data.ChangeVsPy = temp;
        data.NumberOfResponses = getSorted(data.NumberOfResponses, sIndex);
        data.SampleSize = getSorted(data.SampleSize, sIndex);
        temp = [];
        _.each(data.SignificanceData, function (i, j) {
            temp.push(getSorted(i, sIndex));
        });
        data.SignificanceData = temp;
        temp = [];
        _.each(data.ValueData, function (i, j) {
            temp.push(getSorted(i, sIndex));
        });
        data.ValueData = temp;
    }
    d3.select(id).select("svg").remove();
   $(id).empty();
    var color = d3.scale.ordinal()
                .range(["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"]);
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    var colorForTopRect = d3.scale.ordinal()
                .range(["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"]);
    var colorForTopRectVal = ["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"];
    //var count = 6;
    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';

    //resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");

    //use this height and width for entire chart and adjust here with svg traslation
    //height = parseFloat($(id).height() - 100);
    //width = parseFloat($(id).width() - 100);
    //Toset width and height of svg

    //var NoOfElements = data.BrandList.length * data.ValueData.length;
    var Check_BrandList = data.BrandList.length;
    //var factor = 60, fctr4mrgn = 40;

    //if (NoOfElements < 10) { factor = 100; fctr4mrgn = NoOfElements * 30; }
    //if (NoOfElements < 5) { factor = 150; fctr4mrgn = NoOfElements * 70; }


    //if (NoOfElements > 2) {
    //    //$('.showChartMain').css("overflow-x", "hidden"); $('.showChartMain').css("overflow-y", "auto")
    //}
    //var dynamic_width_caliberation = factor * NoOfElements;
    //var margin = { left: 60, right: 20, bottom: 20, top: fctr4mrgn },
    //height = dynamic_width_caliberation - margin.top - margin.bottom,
    //width = parseFloat($(id).width() - 100);


    //var y0 = d3.scale.ordinal()
    //.rangeRoundBands([0,height], .2);

    //var y1 = d3.scale.ordinal();

    //var x = d3.scale.linear()
    //    .range([0, width]);
    //var xAxis = d3.svg.axis()
    //    .scale(x)
    //    .orient("bottom");

    //var yAxis = d3.svg.axis()
    //    .scale(y0)
    //    .orient("left")
    //    .tickPadding(20)
    //    .innerTickSize(0);
  
    //Append the div for tooltip
    var divTooltip = d3.select(".showChartMain").append("div")
        .attr("class", "d3_tooltip")
        .style("opacity", 0);

    //var svg = d3.select(id).append("svg")
    //    .attr("width", "99%")
    //    .attr("height", height + margin.top + margin.bottom)
    //    .append("g")
    //    .attr("transform", "translate(250,55)");

    var each_grp_len = (data.ValueData.length - 3) / 2;
    var NoOfElements = data.BrandList.length * data.ValueData.length;
    //$("#chart-visualisation").css("overflow-y", "auto");
    //$("#chart-visualisation").css("overflow-x", "hidden");
    if (NoOfElements <= 6) {
        NoOfElements = $(id).height() / 70;
        $(id).css("overflow-x", "hidden");
        $(id).css("overflow-y", "hidden");
    }
    var margin = { top: 5, right: 0, bottom: 5, left: 0 },
    //width = $(id).width() - 100 - margin.left - margin.right,
    width = $(id).width() - margin.left - margin.right,
    height = $(id).height() - margin.top - margin.bottom; //NoOfElements * 70

    var y0 = d3.scale.ordinal()
        .rangeRoundBands([height, 0], 0.15);

    var y1 = d3.scale.ordinal();

    var x = d3.scale.linear()
        .range([0, width - 50]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    //var yAxis = d3.svg.axis()
    //    .scale(y0)  //shak
    //    .orient("left")
    //    .tickFormat(d3.format(".2s"));

    var yAxis = d3.svg.axis()
        .scale(y0)
        .orient("left")
       .tickPadding(20)
        .innerTickSize(0);
    ///////////////////////////////////////////////////////////
    var viewBoxWidth = $("#idtrendChartMain4").width();
    var viewBoxHeight = $("#idtrendChartMain4").height();

    var svg = d3.select(id)
        .classed("svg-container", true) //container class to make it responsive
        .append("svg")
         .attr("preserveAspectRatio", "xMidYMid meet")
    .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
        //.attr("width", width + margin.left + margin.right)
        //.attr("height", height + margin.top + margin.bottom)
          .attr("width", "100%")
        .attr("height", "97%")
      .append("g")
        .attr("id","maingcontainer")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
    
    svg.html(defs);


    var xvalues = [];
    var dataArray = [];
    var cumulative = 0;
    var start = '';
    var end = '';
    var xStart = [, ];
    var countt = 1;

    //For Measure :-EDUCATION and Socio Economic
    if (identifier == 1) {
        $.each(data.MetricList, function (i, v) {
            xvalues.push(v);
        });
        for (var i = 0; i < xvalues.length; i++) {
            var obj = {};
            obj.name = xvalues[i];
            $.each(data.ValueData[i], function (j, v) {
                var tempName = data.BrandList[j] + " (" + addCommas(data.SampleSize[j]) + ")";//"val" + (j + 1);
                if (CheckIfStoreFrequencyMeasure(data.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                    obj[tempName] = "0";
                else
                    obj[tempName] = v;
                //obj[tempName] = v;
                obj["Significance" + j] = data.SignificanceData[i][j];
                obj["customBase" + j] = data.BrandList[j];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = [];
                    obj.index = data.ArrayIndexList;
                }
            });
            dataArray.push(obj);
        }
    }

    //For other Measures
    else {
        $.each(data.BrandList, function (i, v) {
            xvalues.push(v);
        });
        for (var i = 0; i < xvalues.length; i++) {
            var obj = {};
            obj.name = xvalues[i];
            for (var j = 0; j < data.ValueData.length; j++) {
                var tempName = data.MetricList[j];//"val" + (j + 1);
                if (CheckIfStoreFrequencyMeasure(data.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                    obj[tempName] = "0";
                else
                    obj[tempName] = data.ValueData[j][i];
                //obj[tempName] = data.ValueData[j][i];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = [];
                    obj.index = data.ArrayIndexList;
                }
            }
            dataArray.push(obj);
        }v
    }
    dataArray = dataArray.reverse();
    var yearNames = d3.keys(dataArray[0]).filter(function (key) { return key != "name" && !(key.indexOf("Significance") > -1) && !(key.indexOf("customBase") > -1) && !(key.indexOf("index") > -1); });

    //dataArray.forEach(function (d) {
    //    d.yearValues = yearNames.map(function (name) { return { name: name, value: +d[name] }; });
    //});
    dataArray.forEach(function (d) {
        d.yearValues = yearNames.map(function (name, i) { return { name: name, value: +d[name], Significance: d["Significance" + i],index: d["index"], customBase: d["customBase" + i] }; });
    });

    if (dataArray.length > 0) {
        y0.domain(dataArray.map(function (d) { return d.name; }));
        y1.domain(yearNames).rangeRoundBands([0, y0.rangeBand()]);
        x.domain([0, d3.max(dataArray, function (d) {
            var val = ((28.20 * d3.max(dataArray, function (d) { return d3.max(d.yearValues, function (d) { return d.value; }); })) / 100) + d3.max(dataArray, function (d) { return d3.max(d.yearValues, function (d) { return d.value; }); });
            return val;
        })]);
    }

    //var gap = 16;
    //var wdt = 48;
    //var yShift = wdt / 3.9;

    var gap = 6;
    var wdt = y1.rangeBand(), xShift = 10;

    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + (height - 10) + ")")
        .call(xAxis)
        .selectAll(".x.axis path")
        .attr("display", "none");
    d3.selectAll(".x.axis .tick")
        .style("display", "none");
    d3.selectAll(".x.axis .tick text")
        .style("display", "none");
    
    svg.selectAll(".x.axis .tick text")
        .style("font-family", "Arial")
        .style("font-size", "11px")
        .call(verticalWrap, 80);
    
    var yaxis = svg.append("g")
    .attr("id", "yaxis")
       .attr("class", "y axis")
        .style("fill", "black")
        .style("stroke", "black")
        .style("stroke-width", "0")
        .attr("transform", "translate(15,-10)")
       .call(yAxis);

    svg.selectAll(".y.axis path")
       .attr("display", "none");

    svg.selectAll(".y.axis .tick text")
        .style("font-family", "Arial")
        .style("font-size", "11px")
        .call(verticalWrap, 200);

    var el = document.getElementById("yaxis"); // or other selector like querySelector()
    var rect = el.getBoundingClientRect(); // get the bounding rectangle
    if (rect.width < 200) {
        var myElement = document.getElementById("maingcontainer");
        myElement.setAttribute('transform', 'translate(205,0)');
    }
    else {
        var myElement = document.getElementById("maingcontainer");
        myElement.setAttribute('transform', 'translate(' + rect.width + ',0)');
    }

    var state = svg.selectAll(".name")
        .data(dataArray)
        .enter().append("g")
        .attr("class", function (d, i) { return "name " + d.name; })
        .attr("transform", function (d) { return "translate(0," + y0(d.name) + ")"; });

    //state.selectAll("rect")
    //    .data(function (d) { return d.yearValues; })
    //    .enter().append("rect")
    //    .attr("height", wdt - gap)
    //    .attr("y", function (d) { return y1(d.name) + yShift; })
    //    .attr("x", function (d) { return 7; })
    //    .attr("width", function (d) { return (x(d.value)) < 0 ? 0 : (x(d.value)); })
    //    .style("stroke", function (d) { return color(d.name); })
    //    .style("stroke-width", "1")
    //    .style("fill", "none");


    state.selectAll("rectBig")
        .data(function (d) { return d.yearValues; })
        .enter().append("rect")
        .attr("class", "rectOfEachName")
        .attr("height", (wdt - gap) < 0 ? 0 : (wdt - gap))
        .attr("y", function (d) { return y1(d.name); })
        .attr("x", function (d) { return xShift; })
        .attr("width", function (d,i) {
            //Append the small lines in each rect
            var context = d3.select(this.parentNode);
            ///*SemiRectangle*/
            //context.append("rect")
            //.attr("class", "dualInverse")
            //.attr("fill", "url(#sampleYGradient)")
            //            .attr("height", (wdt - gap - 10) / 2)
            //            .attr("y", y1(d.name) + yShift + 6.5)
            //            .attr("x", 7)
            //            .attr("width", (x(d.value) - 9) < 0 ? 0 : (x(d.value) - 9))
            //            //.style("opacity",0.5)
            //  .on("mousemove", function () {
            //      divTooltip.style("left", d3.event.pageX - 50 + "px");
            //      divTooltip.style("top", d3.event.pageY - 90 + "px");
            //      divTooltip.style("display", "inline-block");
            //      divTooltip.style("opacity", 1);
            //      var x = d3.event.pageX, y = d3.event.pageY
            //      var elements = document.querySelectorAll(':hover');
            //      l = elements.length
            //      l = l - 1
            //      elementData = elements[l].__data__
            //      divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //  })

            //        .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //        });
            ////For Lines inside bar            
            //var hgh = Math.floor((x(d.value) - 2) / 6);

            //for (var i = 0; i < hgh; i++) {

            //    context.append("rect")
            //    .attr("height", wdt - 12 - gap)
            //    .attr("y", y1(d.name) + yShift + 6)
            //    .attr("x", (i * 6) + 2)
            //    .attr("width", 1)
            //    .style("fill", "black")
            //      .on("mousemove", function () {
            //          divTooltip.style("left", d3.event.pageX - 50 + "px");
            //          divTooltip.style("top", d3.event.pageY - 90 + "px");
            //          divTooltip.style("display", "inline-block");
            //          divTooltip.style("opacity", 1);
            //          var x = d3.event.pageX, y = d3.event.pageY
            //          var elements = document.querySelectorAll(':hover');
            //          l = elements.length
            //          l = l - 1
            //          elementData = elements[l].__data__
            //          divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //      })

            //        .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //        });
            //}
            //// radius of upper circle
            //var radius = (wdt - gap) / 2,
            //       startAngle = (0 * (Math.PI / 180)),
            //       endAngle = (-180 * (Math.PI / 180));
            ///*Dashed Line*/
            context.append("rect")
                .attr("class", "rectAtMiddle")
               .attr("height", 1)
                        .attr("y", y1(d.name) + (wdt - gap) / 2)
                        .attr("x", xShift)
                        .attr("width", (x(d.value) - 10) < 0 ? 0 : x(d.value) - 10)
                .style("opacity", 0.7)
                .style("fill", function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForTopRectVal[d.index[i].split(';')[1]];
                    }
                    else
                        return colorForTopRect(d.name);
                })
                //.attr("y1", y1(d.name) + yShift + (wdt - gap) / 2)
                //.attr("x1", (x(d.value)))
                //.attr("y2", y1(d.name) + yShift + (wdt - gap) / 2)
                //.attr("x2", width / 1.3)
                //.style("stroke-dasharray", ("3, 3"))
                //.style("stroke-opacity", 0.5)
                //.style("stroke", "black")
                 .on("mousemove", function () {
                     divTooltip.style("left", d3.event.pageX - 50 + "px");
                     divTooltip.style("top", d3.event.pageY - 90 + "px");
                     divTooltip.style("display", "inline-block");
                     divTooltip.style("opacity", 1);
                     var x = d3.event.pageX, y = d3.event.pageY
                     var elements = document.querySelectorAll(':hover');
                     l = elements.length
                     l = l - 1
                     elementData = elements[l].__data__
                     if (elementData == undefined)
                         divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize)  + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                     else
                         divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize)  + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                 })

                    .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });

            //var cy = y1(d.name) + yShift + (wdt - gap) / 2;
            //var cx = (x(d.value));
            //var arc = d3.svg.arc()
            //    .innerRadius(radius)
            //    .outerRadius(radius)
            //    .startAngle(startAngle)
            //    .endAngle(endAngle)

            //context.append("path")
            //    .attr("d", arc)
            //    .attr("transform", "translate(" + (width / 1.3 + radius) + "," + cy + ")")
            //    .style("fill", "none")
            //    .style("stroke-dasharray", ("3, 3"))
            //    .style("stroke-opacity", 0.5)
            //    .style("stroke", "black");
            // text at each bar
            context.append("text")
               //.attr("y", cy + 2)
               //.attr("x", width / 1.3 + 2 * radius)
               .attr("y", y1(d.name) + (wdt) / 2)
                .attr("x", x(d.value) + xShift + 15)
               //.text(d.value + '%')
                .text(function (i) {
                    if (CheckIfStoreFrequencyMeasure(d.name.split('(')[0].trim()) && Measurelist[0].metriclist.length > 0)
                        return 'NA';
                    else
                        return (d.value.toFixed(1)) + '%';
                })
               .style("text-anchor", "middle")
               .style("font-weight", "bold")
                .style("font-size", "12px")
                .style("text-anchor", "start")
               .style("fill", Get_Significance_Color(d.Significance, d.customBase, d.samplesize))//color(d.name)
         .on("mousemove", function () {
             divTooltip.style("left", d3.event.pageX - 50 + "px");
             divTooltip.style("top", d3.event.pageY - 90 + "px");
             divTooltip.style("display", "inline-block");
             divTooltip.style("opacity", 1);
             var x = d3.event.pageX, y = d3.event.pageY
             var elements = document.querySelectorAll(':hover');
             l = elements.length
             l = l - 1
             elementData = elements[l].__data__
             if (elementData == undefined)
                 divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
             else
                 divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
         })

                    .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });
            //// small rects besides  bar
            //context.append("rect")
            //            .attr("width", 5)
            //            .attr("y", y1(d.name) + yShift + wdt - 3 - gap)
            //            .attr("x", 7)
            //            .attr("height", 5)
            //            .style("fill", color(d.name))
            //             .on("mousemove", function () {
            //                 divTooltip.style("left", d3.event.pageX - 50 + "px");
            //                 divTooltip.style("top", d3.event.pageY - 90 + "px");
            //                 divTooltip.style("display", "inline-block");
            //                 divTooltip.style("opacity", 1);
            //                 var x = d3.event.pageX, y = d3.event.pageY
            //                 var elements = document.querySelectorAll(':hover');
            //                 l = elements.length
            //                 l = l - 1
            //                 elementData = elements[l].__data__
            //                 divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //             })

            //        .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //        });
            //context.append("rect")
            //            .attr("width", 5)
            //            .attr("y", y1(d.name) + yShift - 2)
            //            .attr("x", 7)
            //            .attr("height", 5)
            //            .style("fill", color(d.name))
            //             .on("mousemove", function () {
            //                 divTooltip.style("left", d3.event.pageX - 50 + "px");
            //                 divTooltip.style("top", d3.event.pageY - 90 + "px");
            //                 divTooltip.style("display", "inline-block");
            //                 divTooltip.style("opacity", 1);
            //                 var x = d3.event.pageX, y = d3.event.pageY
            //                 var elements = document.querySelectorAll(':hover');
            //                 l = elements.length
            //                 l = l - 1
            //                 elementData = elements[l].__data__
            //                 divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%");
            //             })

            //        .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //        });
            ////Bottom rect
            //Rect at the end of the width 
            context.append("rect")
                .attr("height", wdt + 4 - gap)
                        .attr("y", y1(d.name) - 2)
                        .attr("x", 5)
                        .attr("width", 2)
                .style("fill", function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForTopRectVal[d.index[i].split(';')[1]];
                    }
                    else
                        return colorForTopRect(d.name);
                })
                 .on("mousemove", function () {
                                      divTooltip.style("left", d3.event.pageX - 50 + "px");
                                      divTooltip.style("top", d3.event.pageY - 90 + "px");
                                      divTooltip.style("display", "inline-block");
                                      divTooltip.style("opacity", 1);
                                      var x = d3.event.pageX, y = d3.event.pageY
                                      var elements = document.querySelectorAll(':hover');
                                      l = elements.length
                                      l = l - 1
                                      elementData = elements[l].__data__
                                      if (elementData == undefined)
                                          divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                                      else
                                          divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                                  })

                             .on("mouseout", function (d) {
                                 divTooltip.style("display", "none");
                                 divTooltip.style("opacity", 0);
                             });
            //Rect at the end of the width 
            context.append("rect")
                .attr("height", (wdt - gap) < 0 ? 0 : (wdt - gap))
                        .attr("y", y1(d.name))
                        .attr("x", x(d.value) + xShift - 10)
                        .attr("width", (x(d.value) == 0 || x(d.value) == "NA") ? 0 : 3)
                .style("fill", function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForLegends[d.index[i].split(';')[1]];
                    }
                    else
                        return color(d.name);
                })
                 .on("mousemove", function () {
                     divTooltip.style("left", d3.event.pageX - 50 + "px");
                     divTooltip.style("top", d3.event.pageY - 90 + "px");
                     divTooltip.style("display", "inline-block");
                     divTooltip.style("opacity", 1);
                     var x = d3.event.pageX, y = d3.event.pageY
                     var elements = document.querySelectorAll(':hover');
                     l = elements.length
                     l = l - 1
                     elementData = elements[l].__data__
                     if (elementData == undefined)
                         divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                     else
                         divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
                 })

                    .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });

            return (x(d.value) - 9) < 0 ? 0 : (x(d.value) - 9);
        })
        .style("fill", function (d,i) {
            if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                return colorForLegends[d.index[i].split(';')[1]];
            }
            else
            return color(d.name);
        })
        .on("mousemove", function (d) {

             var sample = $(this).parent().attr('class');
             sampleText = sample.substr(sample.indexOf(' ') + 1, sample.length)

             divTooltip.style("left", d3.event.pageX - 50 + "px");
             divTooltip.style("top", d3.event.pageY - 90 + "px");
             divTooltip.style("display", "inline-block");
             divTooltip.style("opacity", 1);
             var x = d3.event.pageX, y = d3.event.pageY
             var elements = document.querySelectorAll(':hover');
             l = elements.length
             l = l - 1
             elementData = elements[l].__data__
             if (elementData == undefined)
                 divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
             else
                 divTooltip.html((d.name) + "<br>" + sampleText + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + "</div>");
         })
        .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });
}

/*------------------------------------------------ bar with change ---------------------------------------------------*/

function GoToChartFunctionForColumnChartOne_NewForBar_with_change(data, identifier) {
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain4').css('display', 'block');

    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];

    plotBarChartForColumnChartOne_NewFor_Bar_With_Change_StackedBar(data, "Percent", 8, "Percentage", true, '#idtrendChartMain4', identifier);
    plotLegends(data, identifier, colorForLegends);
}

function plotBarChartForColumnChartOne_NewFor_Bar_With_Change_StackedBar(data, metricType, noOfTicks, yLabel, barShadow, id, identifier) {

    _.forEach(data.BrandList, function (i, j) { data.BrandList[j] = i.trim() });
    _.forEach(data.MetricList, function (i, j) { data.MetricList[j] = i.trim() });

    if (data.SampleSize != undefined && data.SampleSize != "" && data.SampleSize.length > 0)
        if (Selected_StatTest.toLocaleLowerCase().trim() == "custom base" || Sigtype_Id == "1") {
            //Sorting Data
            var sCompList = [];
            if (currentpage.indexOf("deepdive") > -1) {
                if (Grouplist.length > 0)
                    $.grep(Grouplist, function (i, j) {
                        sCompList.push(i.Name.toLocaleLowerCase().trim());
                    });
            }
            else {
                if (Comparisonlist.length > 0)
                    $.grep(Comparisonlist, function (i, j) {
                        sCompList.push(i.Name.toLocaleLowerCase().trim());
                    });
                else if (ComparisonBevlist.length > 0)
                    $.grep(ComparisonBevlist, function (i, j) {
                        sCompList.push(i.Name.toLocaleLowerCase().trim());
                    });
                else if (Sites.length > 0)
                    $.grep(Sites, function (i, j) {
                        sCompList.push(i.Name.toLocaleLowerCase().trim());
                    });
            }
            var sIndex = [];
            _.each(sCompList, function (i, j) {
                _.each(data.BrandList, function (k, l) { if (i.toLocaleLowerCase().trim() == k.toLocaleLowerCase().trim()) sIndex.push(l); })
            }
            )

            data.BrandList = getSorted(data.BrandList, sIndex);
            var temp = [];
            _.each(data.ChangeVsPy, function (i, j) {
                temp.push(getSorted(i, sIndex));
            });
            data.ChangeVsPy = temp;
            data.NumberOfResponses = getSorted(data.NumberOfResponses, sIndex);
            data.SampleSize = getSorted(data.SampleSize, sIndex);
            temp = [];
            _.each(data.SignificanceData, function (i, j) {
                temp.push(getSorted(i, sIndex));
            });
            data.SignificanceData = temp;
            temp = [];
            _.each(data.ValueData, function (i, j) {
                temp.push(getSorted(i, sIndex));
            });
            data.ValueData = temp;
        }
        data.SampleSize = CheckNegativeSampleSize(data.SampleSize);
    d3.select(id).select("svg").remove();
    $(id).empty();

    var color = d3.scale.ordinal()
                .range(["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"]);
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    var colorForTopRect = d3.scale.ordinal()
              .range(["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"]);
    var colorForTopRectVal = ["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"];
    //var count = 6;
    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';

    //resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");

    //use this height and width for entire chart and adjust here with svg traslation
    //height = parseFloat($(id).height() - 100);
    //width = parseFloat($(id).width() - 100);
    //Toset width and height of svg

    var NoOfElements = data.BrandList.length * data.ValueData.length;
    var Check_BrandList = data.BrandList.length;
    //var factor = 60, fctr4mrgn = 40;

    //if (NoOfElements < 10) { factor = 100; fctr4mrgn = NoOfElements * 30; }
    //if (NoOfElements < 5) { factor = 150; fctr4mrgn = NoOfElements * 70; }


    //if (NoOfElements > 2) {
    //    //$('.showChartMain').css("overflow-x", "hidden"); $('.showChartMain').css("overflow-y", "auto")
    //}
    //var dynamic_width_caliberation = factor * NoOfElements;
    //var margin = { left: 60, right: 20, bottom: 20, top: fctr4mrgn },
    //height = dynamic_width_caliberation - margin.top - margin.bottom,
    //width = parseFloat($(id).width() - 100);


    //var y0 = d3.scale.ordinal()
    //.rangeRoundBands([0, height], .2);

    //var y1 = d3.scale.ordinal();

    //var x = d3.scale.linear()
    //    .range([0, width]);

    //var xAxis = d3.svg.axis()
    //    .scale(x)
    //    .orient("bottom");

    //var yAxis = d3.svg.axis()
    //    .scale(y0)
    //    .orient("left")
    //    .tickPadding(20)
    //    .innerTickSize(0);

    var each_grp_len = (data.ValueData.length - 3) / 2;
    if (NoOfElements <= 6) {
        NoOfElements = $("#chart-visualisation").height() / 70;
        $(id).css("overflow-x", "hidden");
        $(id).css("overflow-y", "hidden");
    }
    var margin = { top: 5, right: 50, bottom: 5, left: 205 },
    width = $(id).width() - 100 - margin.left - margin.right,
    height = $(id).height() - margin.top - margin.bottom; //NoOfElements * 70

    var y0 = d3.scale.ordinal()
        .rangeRoundBands([height, 0], 0.15);

    var y1 = d3.scale.ordinal();

    var x = d3.scale.linear()
        .range([0, width]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y0)
        .orient("left")
        .tickPadding(20)
        .innerTickSize(0);
    ///////////////////////////////////////////////////////////
    var viewBoxWidth = $("#idtrendChartMain4").width();
    var viewBoxHeight = $("#idtrendChartMain4").height();

    var svg = d3.select(id)
        .classed("svg-container", true) //container class to make it responsive
        .append("svg")
          .attr("preserveAspectRatio", "xMidYMid meet")
    .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
   //class to make it responsive
   .classed("svg-content-responsive", true)
        //.attr("width", width + margin.left + margin.right)
        //.attr("height", height + margin.top + margin.bottom)
          .attr("width", "100%")
        .attr("height", "97%")
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    //Append the div for tooltip
    var divTooltip = d3.select(".showChartMain").append("div")
        .attr("class", "d3_tooltip")
        .style("opacity", 0);

    //var svg = d3.select(id).append("svg")
    //    .attr("width", "99%")
    //    .attr("height", height + margin.top + margin.bottom)
    //    .append("g")
    //    .attr("transform", "translate(250,55)");

    svg.html(defs);

    var xvalues = [];
    var dataArray = [];
    var cumulative = 0;
    var start = '';
    var end = '';
    var xStart = [, ];
    var countt = 1;
    //For Measure :-EDUCATION and Socio Economic
    if (identifier == 1) {
        $.each(data.MetricList, function (i, v) {
            xvalues.push(v);
        });
        for (var i = 0; i < xvalues.length; i++) {
            var obj = {};
            obj.name = xvalues[i];
            $.each(data.ValueData[i], function (j, v) {
                var tempName = data.BrandList[j] + " (" + addCommas(data.SampleSize[j]) + ")";//"val" + (j + 1);
                if (CheckIfStoreFrequencyMeasure(data.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                    obj[tempName] = "0";
                else
                    obj[tempName] = v;
                //obj[tempName] = v;
                obj["Significance" + j] = data.SignificanceData[i][j];
                obj["customBase" + j] = data.BrandList[j];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = [];
                    obj.index = data.ArrayIndexList;
                }
            });
            $.each(data.ChangeVsPy[i], function (j, v) {
                var tempName = data.BrandList[j] + " (" + addCommas(data.SampleSize[j]) + ")";//"val" + (j + 1);
                obj[tempName + "Changepy"] = v;
            });
            dataArray.push(obj);
        }
    }

    //For other Measures

    else {
        $.each(data.BrandList, function (i, v) {
            xvalues.push(v);
        });
        for (var i = 0; i < xvalues.length; i++) {
            var obj = {};
            obj.name = xvalues[i];
            for (var j = 0; j < data.ValueData.length; j++) {
                var tempName = data.MetricList[j];//"val" + (j + 1);
                if (CheckIfStoreFrequencyMeasure(data.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                    obj[tempName] = "0";
                else
                    obj[tempName] = data.ValueData[j][i];
                //obj[tempName] = data.ValueData[j][i];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = [];
                    obj.index = data.ArrayIndexList;
                }
            }
            $.each(data.ChangeVsPy[i], function (j, v) {
                var tempName = data.MetricList[j];//"val" + (j + 1);
                obj[tempName + "Changepy"] = v;
            });
            dataArray.push(obj);
        }
    }
    dataArray = dataArray.reverse();
    var yearNames = d3.keys(dataArray[0]).filter(function (key) {
        return key != "name" && !(key.indexOf("Significance") > -1) && !(key.indexOf("customBase") > -1) && !(key.indexOf("index") > -1) && !(key.indexOf("Changepy") > -1);
    });

    //dataArray.forEach(function (d) {
    //    d.yearValues = yearNames.map(function (name) { return { name: name, value: +d[name] }; });
    //});
    dataArray.forEach(function (d) {
        d.yearValues = yearNames.map(function (name, i) {
            return { name: name, value: +d[name],changepy:+d[name+"Changepy"], Significance: d["Significance" + i], index: d["index"], customBase: d["customBase" + i] };
        });
    });

    if (dataArray.length > 0) {
        y0.domain(dataArray.map(function (d) { return d.name; }));
        y1.domain(yearNames).rangeRoundBands([0, y0.rangeBand()]);
        x.domain([0, d3.max(dataArray, function (d) {
            var val = ((33.9 * d3.max(dataArray, function (d) { return d3.max(d.yearValues, function (d) { return d.value; }); })) / 100) + d3.max(dataArray, function (d) { return d3.max(d.yearValues, function (d) { return d.value; }); });
            return val;
        })]);
    }

    //var gap = 16;
    //var wdt = 48;
    //var yShift = wdt / 3.9;
    var gap = 6;
    var wdt = y1.rangeBand(), xShift = 10;

    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + (height + 10) + ")")
        .call(xAxis)
        .selectAll("text").style("fill", "transparent");

    svg.append("g")
        .attr("class", "y axis")
        .style("fill", "black")
        .style("stroke", "black")
        .style("stroke-width", "0")
        .attr("transform", "translate(15, -10)")
        .call(yAxis).selectAll("text").text(function (d) {
            return d;
        });

    //svg.append("g")
    //    .attr("class", "x axis")
    //    .attr("transform", "translate(0," + height + ")")
    //    .call(xAxis)
    //    .selectAll(".x.axis path")
    //    .attr("display", "none");

    d3.selectAll(".x.axis path")
      .style("display", "none");

    d3.selectAll(".x.axis .tick")
       .style("display", "none");

    d3.selectAll(".x.axis .tick text")
      .style("display", "none");

    //svg.append("g")
    //  .attr("class", "y axis")
    //  .call(yAxis);

    svg.selectAll(".y.axis path")
      .attr("display", "none");

    svg.selectAll(".y.axis .tick text")
     .style("font-family", "Arial")
     .style("font-size", "11px")
     .call(verticalWrap, 200);

    var state = svg.selectAll(".name")
        .data(dataArray)
        .enter().append("g")
        .attr("class", function (d, i) { return "name " + d.name; })
        .attr("transform", function (d) { return "translate(0," + y0(d.name) + ")"; });

    //state.selectAll("rect")
    //    .data(function (d) { return d.yearValues; })
    //    .enter().append("rect")
    //    .attr("height", wdt - gap)
    //    .attr("y", function (d) { return y1(d.name) + yShift; })
    //    .attr("x", function (d) { return 7; })
    //    .attr("width", function (d) { return (x(d.value)) < 0 ? 0 : (x(d.value)); })
    //    .style("stroke", function (d) { return color(d.name); })
    //    .style("stroke-width", "1")
    //    .style("fill", "none");


    state.selectAll("rectBig")
        .data(function (d) { return d.yearValues; })
        .enter().append("rect")
        .attr("class", "rectOfEachName")
        .attr("height", (wdt - gap) < 0 ? 0 : (wdt - gap))
        .attr("y", function (d) { return y1(d.name); })
        .attr("x", function (d) { return xShift; })
        .attr("width", function (d,i) {
            //Append the small lines in each rect
            var context = d3.select(this.parentNode);
            ///*SemiRectangle*/
            //context.append("rect")
            //.attr("class", "dualInverse")
            //.attr("fill", "url(#sampleYGradient)")
            //.attr("height", (wdt - gap - 10) / 2)
            //.attr("y", y1(d.name) + yShift + 6.5)
            //.attr("x", 7)
            //.attr("width", (x(d.value) - 9) < 0 ? 0 : (x(d.value) - 9))
            //.on("mousemove", function () {
            //    divTooltip.style("left", d3.event.pageX - 50 + "px");
            //    divTooltip.style("top", d3.event.pageY - 90 + "px");
            //    divTooltip.style("display", "inline-block");
            //    divTooltip.style("opacity", 1);
            //    var x = d3.event.pageX, y = d3.event.pageY
            //    var elements = document.querySelectorAll(':hover');
            //    l = elements.length
            //    l = l - 1
            //    elementData = elements[l].__data__
            //    divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%" + " (" + d.value + ")");
            //})
            //.on("mouseout", function (d) {
            //        divTooltip.style("display", "none");
            //        divTooltip.style("opacity", 0);
            //    });
            //For Lines inside bar            
            //var hgh = Math.floor((x(d.value) - 2) / 6);

            //for (var i = 0; i < hgh; i++) {

            //    context.append("rect")
            //    .attr("height", wdt - 12 - gap)
            //    .attr("y", y1(d.name) + yShift + 6)
            //    .attr("x", (i * 6) + 2)
            //    .attr("width", 1)
            //    .style("fill", "black")
            //      .on("mousemove", function () {
            //          divTooltip.style("left", d3.event.pageX - 50 + "px");
            //          divTooltip.style("top", d3.event.pageY - 90 + "px");
            //          divTooltip.style("display", "inline-block");
            //          divTooltip.style("opacity", 1);
            //          var x = d3.event.pageX, y = d3.event.pageY
            //          var elements = document.querySelectorAll(':hover');
            //          l = elements.length
            //          l = l - 1
            //          elementData = elements[l].__data__
            //          divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%" + " (" + d.value + ")");
            //      })

            //        .on("mouseout", function (d) {
            //            divTooltip.style("display", "none");
            //            divTooltip.style("opacity", 0);
            //        });
            //}
            //// radius of upper circle
            //var radius = (wdt - gap) / 2,
            //startAngle = (0 * (Math.PI / 180)),
            //endAngle = (-180 * (Math.PI / 180));
            /*Dashed Line*/
            context.append("rect")
             .attr("class", "rectAtMiddle")
            //.attr("y1", y1(d.name) + yShift + (wdt - gap) / 2)
            //.attr("x1", (x(d.value)))
            //.attr("y2", y1(d.name) + yShift + (wdt - gap) / 2)
            //.attr("x2", width / 1.3)
            //.style("stroke-dasharray", ("3, 3"))
            //.style("stroke-opacity", 0.5)
            //.style("stroke", "black")
           .attr("height", 1)
                        .attr("y", y1(d.name) + (wdt - gap) / 2)
                        .attr("x", xShift)
                        .attr("width", (x(d.value) -10) < 0 ? 0 : x(d.value) -10)
               .style("opacity", 0.7)
               .style("fill", function () {
                   if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                       return colorForTopRectVal[d.index[i].split(';')[1]];
                   }
                   else
                       return colorForTopRect(d.name);
               })
            .on("mousemove", function () {
                divTooltip.style("left", d3.event.pageX - 50 + "px");
                divTooltip.style("top", d3.event.pageY - 90 + "px");
                divTooltip.style("display", "inline-block");
                divTooltip.style("opacity", 1);
                var x = d3.event.pageX, y = d3.event.pageY
                var elements = document.querySelectorAll(':hover');
                l = elements.length
                l = l - 1
                elementData = elements[l].__data__
                var changeVal = d.changepy.toString() == "NaN" ? "NA" : d.changepy.toFixed(1);
                if (elementData == undefined)
                    divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
                else
                    divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
            })
            .on("mouseout", function (d) {
                    divTooltip.style("display", "none");
                    divTooltip.style("opacity", 0);
                });

            //var cy = y1(d.name) + yShift + (wdt - gap) / 2;
            //var cx = (x(d.value));
            //var arc = d3.svg.arc()
            //    .innerRadius(radius)
            //    .outerRadius(radius)
            //    .startAngle(startAngle)
            //    .endAngle(endAngle)

            //context.append("path")
            //    .attr("d", arc)
            //    .attr("transform", "translate(" + (width / 1.3 + radius) + "," + cy + ")")
            //    .style("fill", "none")
            //    .style("stroke-dasharray", ("3, 3"))
            //    .style("stroke-opacity", 0.5)
            //    .style("stroke", "black");
            // text at each bar
            var changeVal = d.changepy.toString() == "NaN" ? "NA" : d.changepy.toFixed(1);
            context.append("text")
               //.attr("y", cy + 2)
               //.attr("x", width / 1.3 + 3 * radius)
               .attr("y", y1(d.name) + (wdt) / 2)
                .attr("x", x(d.value) + xShift + 15 + 15)
               //.text(d.value + "% (" + changeVal + ")")
                .text(function (i) {
                    if (CheckIfStoreFrequencyMeasure(d.name.split('(')[0].trim()) && Measurelist[0].metriclist.length > 0)
                        return 'NA (NA)';
                    else
                        return d.value.toFixed(1) + "% (" + changeVal + ")";
                })
               .style("font-size", "12px")
                .style("font-weight", "bold")
                .style("text-anchor", "start")
               .style("text-anchor", "middle")
               .style("fill", Get_Significance_Color(d.Significance, d.customBase, d.samplesize))
               .on("mousemove", function () {
             divTooltip.style("left", d3.event.pageX - 50 + "px");
             divTooltip.style("top", d3.event.pageY - 90 + "px");
             divTooltip.style("display", "inline-block");
             divTooltip.style("opacity", 1);
             var x = d3.event.pageX, y = d3.event.pageY
             var elements = document.querySelectorAll(':hover');
             l = elements.length
             l = l - 1
             elementData = elements[l].__data__
             var changeVal = d.changepy.toString() == "NaN" ? "NA" : d.changepy.toFixed(1);
             if (elementData == undefined)
                 divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
             else
                 divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
         })
               .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });
            //// small rects besides  bar
            //context.append("rect")
            //    .attr("width", 5)
            //    .attr("y", y1(d.name) + yShift + wdt - 3 - gap)
            //    .attr("x", 7)
            //    .attr("height", 5)
            //    .style("fill", color(d.name))
            //    .on("mousemove", function () {
            //        divTooltip.style("left", d3.event.pageX - 50 + "px");
            //        divTooltip.style("top", d3.event.pageY - 90 + "px");
            //        divTooltip.style("display", "inline-block");
            //        divTooltip.style("opacity", 1);
            //        var x = d3.event.pageX, y = d3.event.pageY
            //        var elements = document.querySelectorAll(':hover');
            //        l = elements.length
            //        l = l - 1
            //        elementData = elements[l].__data__
            //        divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%" + " (" + d.value + ")");
            //    })
            //    .on("mouseout", function (d) {
            //    divTooltip.style("display", "none");
            //    divTooltip.style("opacity", 0);
            //});
            //context.append("rect")
            //    .attr("width", 5)
            //    .attr("y", y1(d.name) + yShift - 2)
            //    .attr("x", 7)
            //    .attr("height", 5)
            //    .style("fill", color(d.name))
            //    .on("mousemove", function () {
            //        divTooltip.style("left", d3.event.pageX - 50 + "px");
            //        divTooltip.style("top", d3.event.pageY - 90 + "px");
            //        divTooltip.style("display", "inline-block");
            //        divTooltip.style("opacity", 1);
            //        var x = d3.event.pageX, y = d3.event.pageY
            //        var elements = document.querySelectorAll(':hover');
            //        l = elements.length
            //        l = l - 1
            //        elementData = elements[l].__data__
            //        divTooltip.html((d.name) + "<br>" + elementData.name + "<br>" + d.value + "%" + " (" + d.value + ")");
            //    })
            //    .on("mouseout", function (d) {
            //    divTooltip.style("display", "none");
            //    divTooltip.style("opacity", 0);
            //});
            //Bottom rect
            //Rect at the end of the width 
            context.append("rect")
                .attr("height", wdt + 4 - gap)
                        .attr("y", y1(d.name) - 2)
                        .attr("x", 5)
                        .attr("width", 2)
                .style("fill", function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForTopRectVal[d.index[i].split(';')[1]];
                    }
                    else
                        return colorForTopRect(d.name);
                })
                 .on("mousemove", function () {
                     divTooltip.style("left", d3.event.pageX - 50 + "px");
                     divTooltip.style("top", d3.event.pageY - 90 + "px");
                     divTooltip.style("display", "inline-block");
                     divTooltip.style("opacity", 1);
                     var x = d3.event.pageX, y = d3.event.pageY
                     var elements = document.querySelectorAll(':hover');
                     l = elements.length
                     l = l - 1
                     elementData = elements[l].__data__
                     var changeVal = d.changepy.toString() == "NaN" ? "NA" : d.changepy;
                     if (elementData == undefined)
                         divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + changeVal + "%" + "</div>");
                     else
                         divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + changeVal + "%" + "</div>");
                 })

                             .on("mouseout", function (d) {
                                 divTooltip.style("display", "none");
                                 divTooltip.style("opacity", 0);
                             });
            //Rect at the end of the width 

            context.append("rect")
                .attr("height", wdt - gap)
                        .attr("y", y1(d.name))
                        .attr("x", x(d.value) + xShift - 10)
                        .attr("width", (x(d.value) == 0 || x(d.value) == "NA") ? 0 : 3)
                .style("fill", function () {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                        return colorForLegends[d.index[i].split(';')[1]];
                    }
                    else
                        return color(d.name);
                })
                .on("mousemove", function () {
                    divTooltip.style("left", d3.event.pageX - 50 + "px");
                    divTooltip.style("top", d3.event.pageY - 90 + "px");
                    divTooltip.style("display", "inline-block");
                    divTooltip.style("opacity", 1);
                    var x = d3.event.pageX, y = d3.event.pageY
                    var elements = document.querySelectorAll(':hover');
                    l = elements.length
                    l = l - 1
                    elementData = elements[l].__data__
                    var changeVal = d.changepy.toString() == "NaN" ? "NA" : d.changepy.toFixed(1);
                    if (elementData == undefined)
                        divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
                    else
                        divTooltip.html((d.name) + "<br>" + elementData.name + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
                })

                    .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });

            return (x(d.value) - 9) < 0 ? 0 : (x(d.value) - 9);
        })
        .style("fill", function (d,i) {
            if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                return colorForLegends[d.index[i].split(';')[1]];
            }
            else
            return color(d.name);
        })
        .on("mousemove", function (d) {

             var sample = $(this).parent().attr('class');
             sampleText = sample.substr(sample.indexOf(' ') + 1, sample.length)

             divTooltip.style("left", d3.event.pageX - 50 + "px");
             divTooltip.style("top", d3.event.pageY - 90 + "px");
             divTooltip.style("display", "inline-block");
             divTooltip.style("opacity", 1);
             var x = d3.event.pageX, y = d3.event.pageY
             var elements = document.querySelectorAll(':hover');
             l = elements.length
             l = l - 1
             elementData = elements[l].__data__
             var changeVal = d.changepy.toString() == "NaN" ? "NA" : d.changepy.toFixed(1);
             if (elementData == undefined)
                 divTooltip.html((d.name) + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
             else
                 divTooltip.html((d.name) + "<br>" + sampleText + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.value.toFixed(1) + "%" + " (" + changeVal + ")" + "</div>");
         })
        .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });

}

/*---------------------------------------------- Stacked Column Chart -------------------------------------------------*/

function columnChart_Stacked(data, identifier) {
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain4').css('display', 'block');
    SelectedChart = "Stacked Column";
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];

    columnChartStacked(data, '#idtrendChartMain4', identifier);
    plotLegends(data, identifier, colorForLegends);
}

function columnChartStacked(list, id, identifier) {
    if (list.SampleSize != undefined && list.SampleSize != "" && list.SampleSize.length > 0)
        list.SampleSize = CheckNegativeSampleSize(list.SampleSize);
    d3.select(id).select("svg").remove();
    $(id).empty();
    _.forEach(list.BrandList, function (i, j) { list.BrandList[j] = i.trim() });
    _.forEach(list.MetricList, function (i, j) { list.MetricList[j] = i.trim() });

    var color = d3.scale.ordinal()
                .range(["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"]);
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];

    //var count = 6;
    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';

    //resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");

    width = parseFloat($(id).width() - 100);
   
    var NoOfEle = list.BrandList.length, leftFix = 0;
    var margin_left_val = 0;
    //if (NoOfEle < 11) {
    //    NoOfEle = (3 + NoOfEle);
    //}
    //if (NoOfEle < 15) {
    //    margin_left_val = +margin_left_val + 30 * (15 - NoOfEle);
    //}
    //if (NoOfEle > 16) {
    //    //$('.showChartMain').css("overflow", "auto");
    //}
    if (NoOfEle < 4)
        NoOfEle = (2 + NoOfEle);
    else if (NoOfEle < 10) {
        NoOfEle = (2 + NoOfEle);
    }

    if (NoOfEle < 15) {
        //NoOfEle = $(id).width() / 80;
        $(id).css("overflow", "hidden");
    } else {
        $(id).css("overflow-x", "auto");
        $(id).css("overflow-y", "hidden");
    }

    //var margin = { top: 0, right: 30, bottom: 10, left: 50 },
    //width = 137 * NoOfEle - margin.left - margin.right,
    //height = 300 - margin.top - margin.bottom;
    //width = parseFloat($(id).width() - 100);
    //height = parseFloat($(id).height() - 80);
    //width = 80 * parseFloat($(id).width() - 100);

    var margin = { top: 20, right: 10, bottom: 80, left: 10 },
            width = $(id).width() - margin.left - margin.right, //80 * NoOfEle
            height = $(id).height() - margin.top - margin.bottom;

    var xvalues = [];
    var dataArray = [];

    $.each(list.BrandList, function (i, v) {
        xvalues.push(v);
    });

    var Total = [];
    for (var i = 0; i < list.BrandList.length; i++) {
        var mat = [];
        Total[i] = 0;
        for (var j = 0; j < list.ValueData.length; j++) {
            Total[i] = Total[i] + list.ValueData[j][i];
        }

    }

    for (var i = 0; i < list.ValueData.length; i++) {
        var mat = [];
        for (var j = 0; j < list.BrandList.length; j++) {
            var obj = {};
            var tempName = "y";
            obj.MetricName = list.MetricList[i];
            obj.samplesize = list.NumberOfResponses[j];
            obj.x = xvalues[j] + " (" + addCommas(list.SampleSize[j]) + ")";
            obj.original_val = list.ValueData[i][j];
            obj.customBase = xvalues[j];
            obj.Significance = list.SignificanceData[i][j];
            obj["y"] = list.ValueData[i][j] * 100 / Total[j];
            if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length >0)
                obj.index = list.ArrayIndexList[i];
            mat.push(obj);

        }
        dataArray.push(mat);
    }
    
    if (Selected_StatTest.toLocaleLowerCase().trim() == "custom base" || Sigtype_Id == "1") {
        var sCompList = [];
        if (currentpage.indexOf("deepdive") > -1) {
            if (Grouplist.length > 0)
                $.grep(Grouplist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        else {
            if (Comparisonlist.length > 0)
                $.grep(Comparisonlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (ComparisonBevlist.length > 0)
                $.grep(ComparisonBevlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (Sites.length > 0)
                $.grep(Sites, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }

        _.each(dataArray, function (i, j) {
            i.sort(function (a, b) {
                return sCompList.indexOf(a.customBase.toLocaleLowerCase().trim()) < sCompList.indexOf(b.customBase.toLocaleLowerCase().trim()) ? -1 : 1;
            });
        });
    }
    //var x = d3.scale.ordinal()
    //        .rangeRoundBands([0, width], .35);

    //var y = d3.scale.linear()
    //        .rangeRound([height, 0]);


    var x = d3.scale.ordinal()
            .rangeRoundBands([0, width], .15);
    var y = d3.scale.linear()
            .rangeRound([height, 0]);

    var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom");

    var viewBoxWidth = $("#idtrendChartMain4").width();
    var viewBoxHeight = $("#idtrendChartMain4").height();

   // var svg = d3.select(id)
   //     .classed("svg-container", true) //container class to make it responsive
   // .append("svg")
   //      //responsive SVG needs these 2 attributes and no width and height attr
   // .attr("preserveAspectRatio", "xMidYMid meet")
   // .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
   ////class to make it responsive
   //.classed("svg-content-responsive", true)
   //         //.attr("width", width + margin.left + margin.right)
   //         //.attr("height", height + margin.top + margin.bottom)
   //       .attr("width", "100%")
   //     .attr("height", "97%")
   //         .append("g")
    //         .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    var svg = d3.select(id).append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
            .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");



    var color = d3.scale.category20();

    //var xAxis = d3.svg.axis()
    //        .scale(x)
    //        .orient("bottom");

    //Append the div for tooltip
    var divTooltip = d3.select(".showChartMain").append("div")
             .attr("class", "d3_tooltip")
             .style("opacity", 0);

    //var svg = d3.select(id).append("svg")
    //        .attr("width", width + 50)
    //        .attr("height", "99%")
    //        .append("g")
    //        .attr("transform", "translate(40,40)");

    svg.html(defs);

    var dataStackLayout = d3.layout.stack()(dataArray);

    if (dataStackLayout.length > 0)
    x.domain(dataStackLayout[0].map(function (d) {
        return d.x;
    }));

    if (dataStackLayout.length > 0)
    y.domain([0,
        d3.max(dataStackLayout[dataStackLayout.length - 1],
                function (d) { return d.y0 + d.y; })
    ])
      .nice();
    //var linegap = 0, colorInd = 0, wdt = 30, individual_Width = 24, x_State = x.rangeBand() / 2 - individual_Width / 2, posFix = x.rangeBand() / 2 - wdt / 2, cummul_width = 0;
    //var label_h = 15, label_w = 24;

    var linegap = 0, colorInd = 0, intnl_margin = 4, wdt = x.rangeBand() - 5, posFix = x.rangeBand() / 2 - wdt / 2, cummul_width = 0;
    var label_h = 22, label_w = 40;
    var widt = x.rangeBand() - 5
    var posFix1 = x.rangeBand() / 2 - widt / 2
    if (list.BrandList.length < 4) {
        //wdt = 252;
    }

    svg.append("g")
           .attr("class", "x axis")
           .attr("transform", "translate(0," + height + ")")
           .call(xAxis);
    d3.selectAll(".x.axis path").style("fill", "none").style("stroke", "#000").style("stroke-width", 1).style("opacity", 0);
    svg.selectAll("g .tick").append("rect").attr("class", function (d, i) { return "xaxis" + i; }).attr("x", function (d) {
        //return "-23px";
        return -((wdt - 2 * linegap) < 0 ? 0 : (wdt - 2 * linegap) / 2);
    }).attr("y", "3px").attr("height", "3px").attr("width", (wdt - 2 * linegap) < 0 ? 0 : (wdt - 2 * linegap)).style("stroke", "rgb(181, 60, 52)").attr("stroke-width", "0").style("fill", "rgb(103, 103, 103)");

    var xvalues = [];
    $.each($(".tick"), function (i, v) {
        xvalues.push(d3.select(v).node().getBoundingClientRect().left);
    });

    var layer = svg.selectAll(".stack")
            .data(dataStackLayout)
            .enter().append("g")
            .attr("class", "stack")
            .style("fill", function (d, i) {
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    //if (sRemovedLegendPosition.indexOf(i) !== -1)

                    //if (i < d.length)
                        return colorForLegends[d[0].index.split(';')[1]];
                }
                else
                    return colorForLegends[i];
            });
    sRemovedLegendPosition = [];
    layer.selectAll("rect")
            .data(function (d) {
                return d;
            })
            .enter().append("rect")
            .attr("x", function (d,i) {
                //return x(d.x) + x_State + linegap -3;
                //return x(d.x) + linegap + posFix1;
                //if(list.BrandList.length < 4)
                //    return xvalues[i] + 21 - ((wdt - 2 * linegap) < 0 ? 0 : (wdt - 2 * linegap) / 2) + 26;
                //else
                    return x(d.x) + linegap + posFix;
            })
            .attr("y", function (d) {
                return y(d.y + d.y0) + linegap;
            })
            .attr("height", function (d, i) {

                /*Text above circle*/
                var context = d3.select(this.parentNode)

                //lines
                var hgh = Math.floor((y(d.y0) - y(d.y + d.y0)) / 6);

                colorInd = colorInd + 1;
                context.append("rect")
                            .attr("width", "0.3")
                            .attr("x",
                            function(d){
                            //    if(list.BrandList.length < 4)
                            //        return xvalues[i] + 21 + 22;
                            //else
                                    return x(d[i].x) + posFix + wdt / 2 + 1;
                            }
                            )
                            .attr("y", function () {
                                if (isNaN(y(d.y + d.y0)))
                                    return 0;
                                else
                                    return y(d.y + d.y0);
                            })
                            .attr("height", (y(d.y0) - y(d.y + d.y0) - 2 * linegap))
                            .style("stroke", "rgb(101, 65, 128)")
                            .style("stroke-width", "0")
                            .style("fill", "rgb(101, 65, 128)")
                            .on("mouseover", function () {
                                divTooltip.style("left", d3.event.pageX - 50 + "px");
                                divTooltip.style("top", d3.event.pageY - 90 + "px");
                                divTooltip.style("display", "inline-block");
                                divTooltip.style("opacity", 1);
                                var x = d3.event.pageX, y = d3.event.pageY
                                var elements = document.querySelectorAll(':hover');
                                l = elements.length
                                l = l - 1
                                elementData = elements[l].__data__
                                divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.original_val.toFixed(1) + "%" + "</div>");
                            }).on("mouseout", function () {
                                divTooltip.style("display", "none");
                                divTooltip.style("opacity", 0);
                            });
                /*Data Label*/
                context.append("rect")
                .attr("class", d.nullIdentifier)
                .attr("width", label_w)
                .attr("x", function(d){
                    //if(list.BrandList.length < 4)
                    //    return xvalues[i] + 21+13-7;
                    //else
                        return x(d[i].x) + posFix + wdt / 2 - label_w / 2;
                }
                )
                .attr("y",function () {
                    if (isNaN(y(d.y + d.y0) + (y(d.y0) - y(d.y + d.y0)) / 2 - label_h / 2))
                        return 0;
                    else
                        return y(d.y + d.y0) + (y(d.y0) - y(d.y + d.y0)) / 2 - label_h / 2;
                })
                .attr("height", label_h)
                .style("fill", "rgba(255,255,255,0.7)")
                .on("mousemove", function () {
                    divTooltip.style("left", d3.event.pageX - 50 + "px");
                    divTooltip.style("top", d3.event.pageY - 90 + "px");
                    divTooltip.style("display", "inline-block");
                    divTooltip.style("opacity", 1);
                    var x = d3.event.pageX, y = d3.event.pageY
                    var elements = document.querySelectorAll(':hover');
                    l = elements.length
                    l = l - 1
                    elementData = elements[l].__data__
                    divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.original_val.toFixed(1) + "%" + "</div>");
                })
                .on("mouseout", function (d) {
                    divTooltip.style("display", "none");
                    divTooltip.style("opacity", 0);
                });
               
                context.append("text")
                .attr("class", d.nullIdentifier)
                .attr("x",
                function(d){
                    //if(list.BrandList.length < 4)
                    //    return xvalues[i] + 21 + 13 +13;
                    //else
                        return x(d[i].x) + posFix + wdt / 2 - label_w / 2 + label_h - 2;
                }
                )
                .attr("y",function () {
                    if (isNaN(y(y(d.y + d.y0) + (y(d.y0) - y(d.y + d.y0)) / 2 - label_h / 2 + label_h - 6)))
                        return 16;
                    else
                        return y(d.y + d.y0) + (y(d.y0) - y(d.y + d.y0)) / 2 - label_h / 2 + label_h - 6;
                })
                .text(d.original_val.toFixed(1) + "%")
                //.style("fill", "black")
                .style("fill", Get_Significance_Color(d.Significance, d.customBase, d.samplesize))
                .style("text-anchor", "middle")
                .style("font-size", "12px")
                .style("font-weight", "bold")
                 .on("mousemove", function () {
                     divTooltip.style("left", d3.event.pageX - 50 + "px");
                     divTooltip.style("top", d3.event.pageY - 90 + "px");
                     divTooltip.style("display", "inline-block");
                     divTooltip.style("opacity", 1);
                     var x = d3.event.pageX, y = d3.event.pageY
                     var elements = document.querySelectorAll(':hover');
                     l = elements.length
                     l = l - 1
                     elementData = elements[l].__data__
                     divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.original_val.toFixed(1) + "%" + "</div>");
                 })
                 .on("mouseout", function (d) {
                     divTooltip.style("display", "none");
                     divTooltip.style("opacity", 0);
                 });
               
                if (isNaN((y(d.y0) - y(d.y + d.y0) - 2 * linegap)))
                    return 0;
                else
                    return (y(d.y0) - y(d.y + d.y0) - 2 * linegap) < 0 ? 0 : (y(d.y0) - y(d.y + d.y0) - 2 * linegap);
            })
            //.attr("width", individual_Width - 2 * linegap + 6)
        .attr("width", (wdt - 2 * linegap) < 0 ? 0 : (wdt - 2 * linegap))
            .on("mousemove", function () {
             divTooltip.style("left", d3.event.pageX - 50 + "px");
             divTooltip.style("top", d3.event.pageY - 90 + "px");
             divTooltip.style("display", "inline-block");
             divTooltip.style("opacity", 1);
             var x = d3.event.pageX, y = d3.event.pageY
             var elements = document.querySelectorAll(':hover');
             l = elements.length
             l = l - 1
             elementData = elements[l].__data__
             divTooltip.html((elementData.MetricName) + "<br>" + elementData.x + "<br><div style=\"color:" + Get_Significance_Color(elementData.Significance, elementData.customBase, elementData.samplesize) + ";\">" + elementData.original_val.toFixed(1) + "%" + "</div>");
         })
            .on("mouseout", function (d) {
                         divTooltip.style("display", "none");
                         divTooltip.style("opacity", 0);
                     });

    d3.selectAll(".tick text")
        .call(verticalWrap, 100);
   

    //svg.selectAll(".x.axis .tick text")
    //     .style("font-family", "Arial")
    //     .style("font-size", "11px")
    //     .call(wrap, x.rangeBand()+50);

}

/*---------------------------------------------- Stacked Bar Chart ----------------------------------------------------*/

function BarChart_Stacked(data, identifier, MoreLessFact) {   
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain4').css('display', 'block');
    SelectedChart = "Stacked Bar";
    sCurrentMoreLessFact = MoreLessFact;
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];

    BarChartStacked(data, '#idtrendChartMain4', identifier,MoreLessFact);
    plotLegends(data, identifier, colorForLegends);
}

function BarChartStacked(list, id, identifier, MoreLessFact) {
    if (list.SampleSize != undefined && list.SampleSize != "" && list.SampleSize.length > 0)
        list.SampleSize = CheckNegativeSampleSize(list.SampleSize);
    d3.select(id).select("svg").remove();
    $(id).empty();

    _.forEach(list.BrandList, function (i, j) { list.BrandList[j] = i.trim() });
    _.forEach(list.MetricList, function (i, j) { list.MetricList[j] = i.trim() });
    var color = d3.scale.ordinal()
                .range(["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"]);
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];

    //var count = 6;
    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';
    var mainParentheight = $('.showChartMain').height();
    var resheight = '';

    //resheight = (count <= 15) ? mainParentheight : (mainParentheight + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('height', resheight + "px");

    //use this height and width for entire chart and adjust here with svg traslation
    
    height = parseFloat($(id).height() - 100);
    width = parseFloat($(id).width() - 250);

    var NoOfEle = list.BrandList.length, leftFix = 0;
    //var margin_left_val = 0;
    //if (NoOfEle < MoreLessFact) {
    //    NoOfEle = $(".showChartMain").height() / 80;
    //    //$(".showChartMain").css("overflow","hidden");
    //} //else {
        //$(".showChartMain").css("overflow-x", "hidden");
        //$(".showChartMain").css("overflow-y", "auto");
   // }
    //if (NoOfEle < 15) {
    //    margin_left_val = +margin_left_val + 30 * (15 - NoOfEle);
    //}
    //$('.showChartMain').css("overflow", "auto");
    //var ele = $('.showChartMain');
    //ele.css('margin-left', margin_left_val);
    //var margin = { top: 20, right: 30, bottom: 50, left: 80 },
    //height = 80 * NoOfEle - margin.top - margin.bottom,
    //height = 300 - margin.top - margin.bottom;

    //width = parseFloat($(id).width() - 400);
    //width = 80 * parseFloat($(id).width() - 100);

    //var NoOfEle = data.length, leftFix = 0;
    var margin_top_val = 0;
    if (NoOfEle > 5 && NoOfEle < 11) {
        NoOfEle = NoOfEle + 3;
    }
    if (NoOfEle < 6) {
        $(id).css("overflow-y", "hidden");
        NoOfEle = $(id).height() / 70;
    } else {
        //$("#chart-visualisation").css("overflow-y", "auto");
    }

    if (NoOfEle < 4)
        NoOfEle = (2 + NoOfEle);
    

    var margin = { top: 5, right: 100, bottom: 5, left: 190 },
            width = $(id).width() - margin.left - margin.right,
            height = $(id).height() - margin.top - margin.bottom; //70 * NoOfEle

    var y = d3.scale.ordinal()
            .rangeRoundBands([0, height], 0.1);
    var x = d3.scale.linear()
            .rangeRound([0, width]);

    var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom");
    var yAxis = d3.svg.axis()
            .scale(y)
            .orient("left");

    var xvalues = [];
    var dataArray = [];

    $.each(list.BrandList, function (i, v) {
        xvalues.push(v);
    });

    var Total = [];
    for (var i = 0; i < list.BrandList.length; i++) {
        var mat = [];
        Total[i] = 0;
        for (var j = 0; j < list.ValueData.length; j++) {
            Total[i] = Total[i] + list.ValueData[j][i];
        }

    }

    for (var i = 0; i < list.ValueData.length; i++) {
        var mat = [];
        for (var j = 0; j < list.BrandList.length; j++) {
            var obj = {};
            var tempName = "y";
            obj.MetricName = list.MetricList[i];
            obj.samplesize = list.NumberOfResponses[j];
            obj.x = xvalues[j] + " (" + addCommas(list.SampleSize[j]) + ")";
            obj.original_val = list.ValueData[i][j];
            obj.customBase = xvalues[j];
            obj.Significance = list.SignificanceData[i][j];
            obj["y"] = list.ValueData[i][j] * 100 / Total[j];
            if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0)
                obj.index = list.ArrayIndexList[i];
            mat.push(obj);

        }
        dataArray.push(mat);
    }
    if (Selected_StatTest.toLocaleLowerCase().trim() == "custom base" || Sigtype_Id == "1") {
        var sCompList = [];
        if (currentpage.indexOf("deepdive") > -1) {
            if (Grouplist.length > 0)
                $.grep(Grouplist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        else {
            if (Comparisonlist.length > 0)
                $.grep(Comparisonlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (ComparisonBevlist.length > 0)
                $.grep(ComparisonBevlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (Sites.length > 0)
                $.grep(Sites, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        _.each(dataArray, function (i, j) {
            i.sort(function (a, b) {
                return sCompList.indexOf(a.customBase.toLocaleLowerCase().trim()) < sCompList.indexOf(b.customBase.toLocaleLowerCase().trim()) ? -1 : 1;
            });
        });
    }
    //var y = d3.scale.ordinal()
    //        .rangeRoundBands([0,height], .35);

    //var x = d3.scale.linear()
    //        .rangeRound([0, width]);

    var color = d3.scale.category20();

    //var xAxis = d3.svg.axis()
    //        .scale(x)
    //        .orient("bottom");
    //var yAxis = d3.svg.axis()
    //        .scale(y)
    //        .orient("left")
    //        .tickPadding(20);

    //Append the div for tooltip
    var divTooltip = d3.select(".showChartMain").append("div")
            .attr("class", "d3_tooltip")
            .style("opacity", 0);

    //var svg = d3.select(id).append("svg")
    //        .attr("width", "99%")
    //        .attr("height", height + 60)
    //        .append("g")
    //        .attr("transform", "translate(210,55)");
    var viewBoxWidth = $("#idtrendChartMain4").width();
    var viewBoxHeight = $("#idtrendChartMain4").height();

    var svg = d3.select(id)
        .classed("svg-container", true) //container class to make it responsive
        .append("svg")
          .attr("preserveAspectRatio", "xMidYMid meet")
    .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
   //class to make it responsive
   .classed("svg-content-responsive", true)
            //.attr("width", width + margin.left + margin.right)
            //.attr("height", height + margin.top + margin.bottom)
            .attr("width", "100%")
        .attr("height", "97%")
            .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    svg.html(defs);

    var dataStackLayout = d3.layout.stack()(dataArray);
    if (dataStackLayout.length >0)
    y.domain(dataStackLayout[0].map(function (d) {
        return d.x;
    }));

    if (dataStackLayout.length > 0)
    x.domain([0,
        d3.max(dataStackLayout[dataStackLayout.length - 1],
                function (d) { return d.y0 + d.y; })
    ]);
   
    //var linegap = 0, colorInd = 0, individual_Width = 30, x_State = y.rangeBand() / 2 - individual_Width / 2, label_h = 15, label_w = 24, wdt = 30, posFix = y.rangeBand() / 2 - wdt / 2;

    var linegap = 0, colorInd = 0, intnl_margin = 4, wdt = y.rangeBand() - 10, posFix = y.rangeBand() / 2 - wdt / 2, cummul_width = 0;
    var label_h = 22, label_w = 40;

    var widt = y.rangeBand() - 10;
    var posFix1 = y.rangeBand() / 2 - wdt / 2;
    if (list.BrandList.length < 3) {
        //wdt = 141;
        wdt = y.rangeBand() - 52;
        var posFix = y.rangeBand() / 2 - wdt / 2;
    }
    
    var layer = svg.selectAll(".stack")
            .data(dataStackLayout)
            .enter().append("g")
            .attr("class", "stack")
            .style("fill", function (d, i) {
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    
                    return colorForLegends[d[0].index.split(';')[1]];
                }
                else
                return colorForLegends[i];
            });

    layer.selectAll("rect")
            .data(function (d) {
                return d;
            })
            .enter().append("rect")
            .attr("x", function (d) {
                return x(d.y0) + linegap;
            })
            .attr("y", function (d) {
                //return y(d.x) + x_State + linegap;
                if (list.BrandList.length == 1)
                    return y(d.x) + linegap + posFix + 68;
                //else if (list.BrandList.length == 2)
                //    return y(d.x) + linegap + posFix + 30;
                else
                    return y(d.x) + linegap + posFix;
            })
            .attr("width", function (d, i) {

                /*Text above circle*/
                var context = d3.select(this.parentNode)

                //Append the outerRect
                //context.append("rect")
                //       .attr("x", x(d.y0))
                //       .attr("y", y(d.x) + x_State)
                //       .attr("width", (x(d.y + d.y0) - x(d.y0)) < 0 ? 0 : (x(d.y + d.y0) - x(d.y0)))
                //       .attr("height", individual_Width)
                //       .style("fill", "none")
                //       .style("stroke", colorForLegends[Math.floor(colorInd / dataArray[0].length)])
                //       .style("stroke-width", "1px")
                // .on("mousemove", function () {
                //     divTooltip.style("left", d3.event.pageX - 50 + "px");
                //     divTooltip.style("top", d3.event.pageY - 90 + "px");
                //     divTooltip.style("display", "inline-block");
                //     divTooltip.style("opacity", 1);
                //     var x = d3.event.pageX, y = d3.event.pageY
                //     var elements = document.querySelectorAll(':hover');
                //     l = elements.length
                //     l = l - 1
                //     elementData = elements[l].__data__
                //     divTooltip.html((d.MetricName) + "<br>" + d.x + "<br>" + d.original_val + "%");
                // })

                //     .on("mouseout", function (d) {
                //         divTooltip.style("display", "none");
                //         divTooltip.style("opacity", 0);
                //     });

             //   context.append("rect")
             //   .attr("class", "dualInverse")
             //   .attr("fill", "url(#sampleYGradient)")
             // .attr("x", x(d.y0) + linegap)
             // .attr("y", y(d.x) + x_State + linegap)
             // .attr("width", (x(d.y + d.y0) - x(d.y0) - 2 * linegap) < 0 ? 0 : (x(d.y + d.y0) - x(d.y0) - 2 * linegap))
             // .attr("height", individual_Width / 2 - linegap)
             //// .style("opacity",0.5)
             //  .on("mousemove", function () {
             //      divTooltip.style("left", d3.event.pageX - 50 + "px");
             //      divTooltip.style("top", d3.event.pageY - 90 + "px");
             //      divTooltip.style("display", "inline-block");
             //      divTooltip.style("opacity", 1);
             //      var x = d3.event.pageX, y = d3.event.pageY
             //      var elements = document.querySelectorAll(':hover');
             //      l = elements.length
             //      l = l - 1
             //      elementData = elements[l].__data__
             //      divTooltip.html((d.MetricName) + "<br>" + d.x + "<br>" + d.original_val + "%");
             //  })

             //        .on("mouseout", function (d) {
             //            divTooltip.style("display", "none");
             //            divTooltip.style("opacity", 0);
             //        });


                //lines
                var hgh = Math.floor((x(d.y + d.y0) - x(d.y0)) / 6);

                //for (var i = 0; i < hgh; i++) {

                //    context.append("rect")
                //    .attr("class", "Line_Over_Rect")
                //    .attr("width", 1)
                //    .attr("y", y(d.x) + x_State + linegap + individual_Width / 24 + 2)
                //    .attr("x", x(d.y0) + (i * 6) + linegap + 1)
                //    .attr("height", individual_Width - 2 * linegap - 2 * individual_Width / 24 - 4)
                //    .style("fill", "black")
                //    .style("opacity", 0.5)
                //     .on("mousemove", function () {
                //         divTooltip.style("left", d3.event.pageX - 50 + "px");
                //         divTooltip.style("top", d3.event.pageY - 90 + "px");
                //         divTooltip.style("display", "inline-block");
                //         divTooltip.style("opacity", 1);
                //         var x = d3.event.pageX, y = d3.event.pageY
                //         var elements = document.querySelectorAll(':hover');
                //         l = elements.length
                //         l = l - 1
                //         elementData = elements[l].__data__
                //         divTooltip.html((d.MetricName) + "<br>" + d.x + "<br>" + d.original_val + "%");
                //     })

                //     .on("mouseout", function (d) {
                //         divTooltip.style("display", "none");
                //         divTooltip.style("opacity", 0);
                //     });
                //}
                colorInd = colorInd + 1;
                context.append("rect")
                            .attr("height", "0.2")
                            .attr("class", d.lowSampleSize)
                            .attr("y",function(d){
                                if (list.BrandList.length == 1)
                                    return y(d[i].x) + posFix + wdt / 2 + 68;
                                else
                                    return y(d[i].x) + posFix + wdt / 2;
                            })
                            .attr("x", x(d.y0))
                            .attr("width", (x(d.y + d.y0) - x(d.y0)) < 0 ? 0 : (x(d.y + d.y0) - x(d.y0)))
                            .style("stroke", "rgb(101, 65, 128)")
                            .style("stroke-width", "0")
                            .style("fill", "rgb(101, 65, 128)")
                            .on("mousemove", function () {
                                divTooltip.style("left", d3.event.pageX - 50 + "px");
                                divTooltip.style("top", d3.event.pageY - 90 + "px");
                                divTooltip.style("display", "inline-block");
                                divTooltip.style("opacity", 1);
                                var x = d3.event.pageX, y = d3.event.pageY
                                var elements = document.querySelectorAll(':hover');
                                l = elements.length
                                l = l - 1
                                elementData = elements[l].__data__
                                divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.original_val.toFixed(1) + "%" + "</div>");
                            })
                    .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });
                /*Data Label*/
                context.append("rect")
                .attr("class", d.nullIdentifier)
                .attr("width", label_w)
                .attr("x", x(d.y0) + (x(d.y + d.y0) - x(d.y0)) / 2 - label_w / 2 -1)
                .attr("y", function (d) {
                    if (list.BrandList.length == 1)
                        return y(d[i].x) + posFix + wdt / 2 - 10 + 68;
                    else
                        return y(d[i].x) + posFix + wdt / 2 - 8;
                }
                )
                .attr("height", label_h - 3)
                .style("fill", "rgba(255,255,255,0.7)")
                 .on("mousemove", function () {
                     divTooltip.style("left", d3.event.pageX - 50 + "px");
                     divTooltip.style("top", d3.event.pageY - 90 + "px");
                     divTooltip.style("display", "inline-block");
                     divTooltip.style("opacity", 1);
                     var x = d3.event.pageX, y = d3.event.pageY
                     var elements = document.querySelectorAll(':hover');
                     l = elements.length
                     l = l - 1
                     elementData = elements[l].__data__
                     divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.original_val.toFixed(1) + "%" + "</div>");
                 })
                 .on("mouseout", function (d) {
                     divTooltip.style("display", "none");
                     divTooltip.style("opacity", 0);
                 });

                context.append("text")
                .attr("class", d.nullIdentifier)
                .attr("x", x(d.y0) + (x(d.y + d.y0) - x(d.y0)) / 2 - label_w / 2 + label_h - 3)
                .attr("y",function(d){
                    if (list.BrandList.length == 1)
                        return y(d[i].x) + posFix + wdt / 2 + 68;
                    else
                        return y(d[i].x) + posFix + wdt / 2 - 10 + 16;
                })
                .text(d.original_val.toFixed(1) + "%")
                .style("fill", Get_Significance_Color(d.Significance, d.customBase, d.samplesize))//d.labelClass
                .style("text-anchor", "middle")
                .style("font-size", "12px")
                .style("font-weight", "bold")
                 .on("mousemove", function () {
                     divTooltip.style("left", d3.event.pageX - 50 + "px");
                     divTooltip.style("top", d3.event.pageY - 90 + "px");
                     divTooltip.style("display", "inline-block");
                     divTooltip.style("opacity", 1);
                     var x = d3.event.pageX, y = d3.event.pageY
                     var elements = document.querySelectorAll(':hover');
                     l = elements.length
                     l = l - 1
                     elementData = elements[l].__data__
                     divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.original_val.toFixed(1) + "%" + "</div>");
                 })
                    .on("mouseout", function (d) {
                        divTooltip.style("display", "none");
                        divTooltip.style("opacity", 0);
                    });
                /*Data Label*/
                return (x(d.y + d.y0) - x(d.y0) - 2 * linegap) < 0 ? 0 : (x(d.y + d.y0) - x(d.y0) - 2 * linegap);
            })
            //.attr("height", individual_Width - 2 * linegap)
        .attr("height", (wdt - 2 * linegap) < 0 ? 0 : (wdt - 2 * linegap))
            .on("mousemove", function () {
                 divTooltip.style("left", d3.event.pageX - 50 + "px");
                 divTooltip.style("top", d3.event.pageY - 90 + "px");
                 divTooltip.style("display", "inline-block");
                 divTooltip.style("opacity", 1);
                 var x = d3.event.pageX, y = d3.event.pageY
                 var elements = document.querySelectorAll(':hover');
                 l = elements.length
                 l = l - 1
                 elementData = elements[l].__data__
                 divTooltip.html((elementData.MetricName) + "<br>" + elementData.x + "<br><div style=\"color:" + Get_Significance_Color(elementData.Significance, elementData.customBase, elementData.samplesize) + ";\">" + elementData.original_val.toFixed(1) + "%" + "</div>");
             })
            .on("mouseout", function (d) {
                         divTooltip.style("display", "none");
                         divTooltip.style("opacity", 0);
                     });


    svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis);

    svg.selectAll(".x.axis path")
           .attr("display", "none");

    d3.selectAll(".x.axis .tick")
           .style("display", "none");

    d3.selectAll(".x.axis .tick text")
           .style("display", "none");

    svg.append("g")
            .attr("class", "y axis")
            .call(yAxis);

    d3.selectAll(".y.axis path,.y.axis line")
           .style("fill", "none")
           .style("stroke", "transparent")
           .style("shape-rendering", "crispEdges");

    svg.selectAll("g .tick").append("rect").attr("class", function (d) {
        return data.BrandList[d];
    }).attr("x", function (d) {
        return "-6px";
    }).attr("y", function (d) {
        return -((wdt - 2 * linegap) < 0 ? 0 : (wdt - 2 * linegap) / 2)
    }).attr("height", (wdt - 2 * linegap) < 0 ? 0 : (wdt - 2 * linegap)).attr("width", "3px").style("stroke", "rgb(181, 60, 52)").attr("stroke-width", "0").style("fill", "rgb(103, 103, 103)");

    svg.selectAll(".y.axis .tick text")
           .style("font-family", "Arial")
           .style("font-size", "11px")
           .call(verticalWrap, 130);
}

/*----------------------------------------------- Pyramid Chart  ------------------------------------------------------*/

function Plot_Pyramid_Chart(data, identifier) {
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain4').css('display', 'block');
    $('#spChartLegend').html('');

    $('#spChartLegend').hide();
    $("#ToShowChart").css("height", "75%");
    //$("#idtrendChartMain4").css("height", "104%");

    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];

    Pyramid_Chart(data, '#idtrendChartMain4', identifier);
}

function Pyramid_Chart(list, id, identifier) {
    d3.select(id).select("svg").selectAll("*").remove();
    if (list.SampleSize != undefined && list.SampleSize != "" && list.SampleSize.length > 0)
        list.SampleSize = CheckNegativeSampleSize(list.SampleSize);
    d3.select(id).select("svg").remove();
    $(id).empty();
    
    var ht2 = $("#chart-title").height();
    $(".showChartMain").css("height", "calc(100% - " + ht2 + "px + 28px)");

    _.forEach(list.BrandList, function (i, j) { list.BrandList[j] = i.trim() });
    _.forEach(list.MetricList, function (i, j) { list.MetricList[j] = i.trim() });

    var color = d3.scale.ordinal()
                .range(["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"]);
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    var colorForTopRect = d3.scale.ordinal()
               .range(["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"]);
    //var count = 6;
    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';

    //resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");
    $(id).height("100%");
    //Megha
    $(".showChartMain").css("overflow", "auto");

    //use this height and width for entire chart and adjust here with svg traslation
    height = parseFloat($(id).height() - 100);
    width = parseFloat($(id).width() - 100);

    var NoOfEle = list.BrandList.length, leftFix = 0;
    //var NoOfEle = data.length, leftFix = 0;
    var margin_left_val = 0;

    var margin = { top: 0, right: 30, bottom: 10, left: 50 };
    if (NoOfEle < 4) {
        //width = 250 * NoOfEle;
        //width = "100%"
    }
    else {
        width = 350 * NoOfEle;
    }


    height = parseFloat($(id).height() - 130);
    var variableheight = (13.82 * $(id).height()) / 100;
    var xvalues = [];
    var dataArray = [];
    var yvalues = [];
    for (var i = list.MetricList.length - 1; i >= 0; i--) {
        yvalues.push(list.MetricList[i]);
    }

    $.each(list.BrandList, function (i, v) {
        xvalues.push(v);
    });

    var Total = [];
    for (var i = 0; i < list.BrandList.length; i++) {
        var mat = [];
        Total[i] = 0;
        for (var j = 0; j < list.ValueData.length; j++) {
            Total[i] = Total[i] + list.ValueData[j][i];
        }

    }

    for (var i = 0; i < list.ValueData.length; i++) {
        var mat = [];
        for (var j = 0; j < list.BrandList.length; j++) {
            var obj = {};
            var tempName = "y";
            obj.MetricName = list.MetricList[i];
            obj.samplesize = list.NumberOfResponses[j];
            obj.x = xvalues[j];
            if (CheckIfStoreFrequencyMeasure(list.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                obj.original_val = "0";
            else
                obj.original_val = list.ValueData[i][j];
            //obj.original_val = list.ValueData[i][j];
            obj.customBase = xvalues[j];
            obj.Significance = list.SignificanceData[i][j];
            obj["y"] = list.ValueData[i][j] * 100 / Total[j];
            mat.push(obj);

        }
        dataArray.push(mat);
    }
    if (Selected_StatTest.toLocaleLowerCase().trim() == "custom base" || Sigtype_Id == "1") {
        var sCompList = [];
        if (currentpage.indexOf("deepdive") > -1) {
            if (Grouplist.length > 0)
                $.grep(Grouplist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        else {
            if (Comparisonlist.length > 0)
                $.grep(Comparisonlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (ComparisonBevlist.length > 0)
                $.grep(ComparisonBevlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (Sites.length > 0)
                $.grep(Sites, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        _.each(dataArray, function (i, j) {
            i.sort(function (a, b) {
                return sCompList.indexOf(a.customBase.toLocaleLowerCase().trim()) < sCompList.indexOf(b.customBase.toLocaleLowerCase().trim()) ? -1 : 1;
            });
        });
    }
    var dataArrayNew = [];
    for (var i = dataArray.length - 1 ; i >= 0; i--) {
        dataArrayNew.push(dataArray[i]);
    }
    var x = d3.scale.ordinal()
            .rangeRoundBands([0, width], .35);

    var y = d3.scale.ordinal()
            .rangeRoundBands([0, height]);

    // var color = d3.scale.category20();

    var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom")
            .innerTickSize(0);

    var yAxis = d3.svg.axis()
            .scale(y)
            .orient("left")
            .innerTickSize(10);

    //Append the div for tooltip
    var divTooltip = d3.select(".showChartMain").append("div")
            .attr("class", "d3_tooltip")
            .style("opacity", 0);

    var viewBoxWidth = $(id).width();
    var viewBoxHeight = $(id).height();

    if (NoOfEle > 3) {
        var widthpyr = NoOfEle * 31.25;
        var viewBoxWidth = NoOfEle * 370;
    }

    if (NoOfEle >= 2) {
        var svg = d3.select(id)
            .classed("svg-container", true) //container class to make it responsive
           .append("svg")
            .attr("id", "mainsvg")
            //responsive SVG needs these 2 attributes and no width and height attr
            .attr("preserveAspectRatio", "xMinYMin meet")
             .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
            //class to make it responsive
            .classed("svg-content-responsive", true)
            .attr("width", function () {
                if (NoOfEle < 4) {
                    return "100%";
                }
                else
                {
                    var percentwidth = ((width / $(id).width()) * 100) + 10;
                    return percentwidth + "%";
                }
            })
             //.attr("width", "" + widthpyr + "%")
             .attr("height", "100%")
             .style("overflow", "scroll")
             .append("g")
             .attr("transform", "translate(10,70)");
    }
    


    svg.html(defs);

    //  for Static data:- data maping
    var temp, i, j, k;
    var total = [], xData = [], perData = [];

    var dataStackLayout = d3.layout.stack()(dataArrayNew);

    x.domain(dataStackLayout[0].map(function (d) {
        return d.x;
    }));

    y.domain([0,
        d3.max(dataStackLayout[dataStackLayout.length - 1],
                function (d) { return d.y0 + d.y; })
    ]);

    //To add the height and yTrans for charts
    var heightLatest;
    var yTranslate;
    var addFact;
    if (Measurelist.length > 0 && (Measurelist[0].parentname == "Relationship Pyramid" || Measurelist[0].parentname == "CORA Pyramid")) {
         heightLatest = 37.5;
         yTranslate = 60;
         addFact = 4;
         addFactl = 5;
         addFactll = 3;
    }
    else {
         heightLatest = 30;
         yTranslate = 50;
         addFact = 0;
         addFactl = 0;
         addFactll = 0;
    }

    var wdt = 50.5, individual_width = 0, colorInd = -1, y_Shift, index_Color, lineWidth, xShift = (width / list.BrandList.length) / 5 + 50;
    //xShift = x.rangeBand() / list.BrandList.length;
    if (NoOfEle == 1)
        xShift = (width / list.BrandList.length) / 5 + 50 + 120 - 120;
    else if (NoOfEle == 2)
        xShift = (width / list.BrandList.length) / 5 + 50 + 100;
    else
        xShift = (width / list.BrandList.length) / 5 + 100;
    lineWidth = (width / NoOfEle) / 5;

    if (NoOfEle == 1)
        wdt = 12.84;
    if (NoOfEle == 2)
        wdt = 40;//25.9;

    var layer7 = svg.append("g").selectAll(".stackLatest")
          .data(dataStackLayout)
          .enter().append("g")
          .attr("class", "stackLatest")
    var colorInd2 = -1;
    //Append the line 
    layer7.selectAll("line")
       .data(function (d) {
           return d;
       })
       .enter().append("line")
       .attr("id", function (d, i) { return "dashedline"+i;})
       .attr("class", "LineBehindEachRect")
       .attr("x1", function (d, i) {
           if (NoOfEle == 1)
               return x(d.x) - (width / 3) + xShift + 10;
           else if (NoOfEle == 2)
               return x(d.x) - ((width / 2) / 3) + xShift + 25;
           else
               return x(d.x) - 120 + xShift + 25;
       })
       .attr("y1", function (d, i) {
           ++colorInd;
           index_Color = Math.floor(colorInd / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 15 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .attr("x2", function (d) {
           if (NoOfEle == 1)
               return x(d.x) + (width / 3) + xShift - 10;
           else if (NoOfEle == 2)
               return x(d.x) + ((width / 2) / 3) + xShift - 25/2;
           else
               return x(d.x) + 120 + wdt + xShift - 25/2;
       })
       .attr("y2", function (d, i) {
           ++colorInd2;
           index_Color = Math.floor(colorInd2 / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 15 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .style("stroke", "black")
       .style("stroke-dasharray", "2,2")
       .style("stroke-width", "0.5px")
       .style("opacity", 1);

    colorInd = -1;
    colorInd2 = -1;
    var layer6 = svg.selectAll(".stackLatest1")
      .data(dataStackLayout)
      .enter().append("g")
      .attr("class", "stackLatest1")

    layer6.selectAll("line")
       .data(function (d) {
           return d;
       })
       .enter().append("line")
       .attr("class", "LineBehindEachRect2")
       .attr("x1", function (d) {
           if (NoOfEle == 1)
               return x(d.x) - (width / 3) + xShift + 10;
           else if (NoOfEle == 2)
               return x(d.x) - ((width / 2) / 3) + xShift + 25;
           else
               return x(d.x) - 120 + xShift + 25;//xShift;
       })
       .attr("y1", function (d, i) {
           ++colorInd;
           index_Color = Math.floor(colorInd / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 12 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .attr("x2", function (d) {
           if (NoOfEle == 1)
               return x(d.x) - (width / 3) + xShift + 10;
           else if (NoOfEle == 2)
               return x(d.x) - ((width / 2) / 3) + xShift + 25;
           else
               return x(d.x) - 120 + xShift + 25; //xShift;
       })
       .attr("y2", function (d, i) {
           ++colorInd2;
           index_Color = Math.floor(colorInd2 / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 19 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .style("stroke", "black")
       .style("stroke-width", "1px")
       .style("opacity", 1);


    colorInd = -1;
    colorInd2 = -1;
    var layer5 = svg.selectAll(".stackLatest2")
      .data(dataStackLayout)
      .enter().append("g")
      .attr("class", "stackLatest2")

    layer5.selectAll("line")
       .data(function (d) {
           return d;
       })
       .enter().append("line")
       .attr("class", "LineBehindEachRect3")
       .attr("x1", function (d) {
           if (NoOfEle == 1)
               return x(d.x) + (width / 3) + xShift - 10;
           else if (NoOfEle == 2)
               return x(d.x) + ((width / 2) / 3) + xShift - 25/2;
           else
               return x(d.x) + 120 + wdt + xShift - 25/2; //width -xShift;
       })
       .attr("y1", function (d, i) {
           ++colorInd;
           index_Color = Math.floor(colorInd / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 12 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .attr("x2", function (d) {
           if (NoOfEle == 1)
               return x(d.x) + (width / 3) + xShift - 10;
           else if (NoOfEle == 2)
               return x(d.x) + ((width / 2) / 3) + xShift - 25/2;
           else
               return x(d.x) + 120 + wdt + xShift - 25/2; //width - xShift;
       })
       .attr("y2", function (d, i) {
           ++colorInd2;
           index_Color = Math.floor(colorInd2 / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 19 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .style("stroke", "black")
       .style("stroke-width", "1px")
       .style("opacity", 1);

    var layer8 = svg.selectAll(".stackNew")
        .data(dataStackLayout)
        .enter().append("g")
        .attr("class", "stackNew")

    colorInd = -1;
    colorInd2 = -1;

    var element = document.getElementById("dashedline0"); // or other selector like querySelector()
    var line = element.getBBox();
    var dashedlinewidth = line.width;

    layer8.selectAll("rect")
        .data(function (d) {
            return d;
        })
        .enter().append("rect")
        .attr("class", "InnerRect")
        .attr("x", function (d,i) {
            if (NoOfEle == 1)
                return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 15;
            //if (NoOfEle == 2) {
                var el = document.getElementById("dashedline" + i);
                var rect = el.getBBox();
                return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) + 1;
                //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 15 - 6;
                //return el.getBBox().x + 2;
                //return x(d.x) + ((((dashedlinewidth - 3) / 100) * d.original_val)) / 2 + xShift + 7 + 1 - 15 - 6;
           // }
            //else
            //    return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1;
        })
        .attr("y", function (d, i) {
            ++colorInd;
            index_Color = Math.floor(colorInd / list.BrandList.length)
            y_Shift = index_Color == 0 ? 1 : 2;
            var context = d3.select(this.parentNode);
            /*SemiRectangle*/
            ////For Lines inside bar            
            //var hgh = Math.floor((100 * d.original_val / wdt - 2) / 6);
            //var yNew = 0;
            //for (var i = 0; i < hgh; i++) {
            context.append("rect")
            .attr("class", "LineRect")
            .attr("height", heightLatest)
            .attr("y", height - (variableheight * (index_Color + 1)))
            .attr("x",
            function(){
                if (NoOfEle == 1)
                    return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 -  17;
                else if (NoOfEle == 2) {
                    var el = document.getElementById("dashedline" + i);
                    var rect = el.getBBox();
                    return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 1;

                    //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 17 - 7;
                }
                else {
                    var el = document.getElementById("dashedline" + i);
                    var rect = el.getBBox();
                    return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 1;
                    //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift - 4 + 7 + 1
                    }
            })
            .attr("width", 4)
            .style("fill", colorForTopRect(i))
            .style("opacity", 1)
                .on("mousemove", function () {
                    divTooltip.style("left", d3.event.pageX - 50 + "px");
                    divTooltip.style("top", d3.event.pageY - 90 + "px");
                    divTooltip.style("display", "inline-block");
                    divTooltip.style("opacity", 1);
                    var x = d3.event.pageX, y = d3.event.pageY
                    var elements = document.querySelectorAll(':hover');
                    l = elements.length
                    l = l - 1
                    elementData = elements[l].__data__
                    divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + parseFloat(d.original_val).toFixed(1) + "%" + "</div>");
                })
                 .on("mouseout", function (d) {
                     divTooltip.style("display", "none");
                     divTooltip.style("opacity", 0);
                 });
            //}

            context.append("rect")
               .attr("class", "LineRect")
               .attr("height", heightLatest)
               .attr("y", height - (variableheight * (index_Color + 1)))
               .attr("x",
               function(){
                   if (NoOfEle == 1)
                       return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + (d.original_val * 100 / wdt) + xShift + 7 + 0.5 - 1 - 14;
                   else if (NoOfEle == 2) {
                       var el = document.getElementById("dashedline" + i);
                       var rect = el.getBBox();
                       return rect.x + (rect.width / 2) + ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 2;

                       //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + (d.original_val * 100 / wdt) + xShift + 7 + 0.5 - 1 - 14 - 6;
                   }
                   else {
                       var el = document.getElementById("dashedline" + i);
                       var rect = el.getBBox();
                       return rect.x + (rect.width / 2) + ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 2;
                       //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + (d.original_val * 100 / wdt) + xShift + 7 + 0.5 - 1
                   }
               })
               .attr("width", 4)
               .style("fill", colorForTopRect(i))
               .style("opacity", 1)
                .on("mousemove", function () {
                    divTooltip.style("left", d3.event.pageX - 50 + "px");
                    divTooltip.style("top", d3.event.pageY - 90 + "px");
                    divTooltip.style("display", "inline-block");
                    divTooltip.style("opacity", 1);
                    var x = d3.event.pageX, y = d3.event.pageY
                    var elements = document.querySelectorAll(':hover');
                    l = elements.length
                    l = l - 1
                    elementData = elements[l].__data__
                    divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + parseFloat(d.original_val).toFixed(1) + "%" + "</div>");
                })
                 .on("mouseout", function (d) {
                     divTooltip.style("display", "none");
                     divTooltip.style("opacity", 0);
                 });

            context.append("rect")
                 .attr("class", "LineRect")
                 .attr("height", 1)
                 .attr("y", height + 15 - (variableheight * (index_Color + 1)) + addFactll)
                 .attr("x",
                 function(){
                     if (NoOfEle == 1)
                         return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 17;
                     else if (NoOfEle == 2) {
                         var el = document.getElementById("dashedline" + i);
                         var rect = el.getBBox();
                         return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2);

                         //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 17 - 5;
                     }
                     else {
                         var el = document.getElementById("dashedline" + i);
                         var rect = el.getBBox();
                         return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2);
                         //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1;
                     }
                 })
                 .attr("width", function () {
                     //if (NoOfEle == 2) {
                         var element1 = document.getElementById("dashedline0"); // or other selector like querySelector()
                         var line1 = element1.getBBox();
                         var dashedlinewidth1 = line1.width;
                         return (((dashedlinewidth1 - 3) / 100) * d.original_val);
                     //}
                     //else
                     //    return (d[i].original_val * 100 / wdt)
                 })
                 .style("fill", colorForTopRect(i))//colorForTopRect(d.name))
                 .style("opacity", 1)
                  .on("mousemove", function () {
                      divTooltip.style("left", d3.event.pageX - 50 + "px");
                      divTooltip.style("top", d3.event.pageY - 90 + "px");
                      divTooltip.style("display", "inline-block");
                      divTooltip.style("opacity", 1);
                      var x = d3.event.pageX, y = d3.event.pageY
                      var elements = document.querySelectorAll(':hover');
                      l = elements.length
                      l = l - 1
                      elementData = elements[l].__data__
                      divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + parseFloat(d.original_val).toFixed(1) + "%" + "</div>");
                  })
                   .on("mouseout", function (d) {
                       divTooltip.style("display", "none");
                       divTooltip.style("opacity", 0);
                   });

            //}
            return (height - (variableheight * (index_Color + 1)));// y(d.y + d.y0);
        })
        .attr("height", function (d, i) { return 30; })
        .attr("width", function (d) {
            //if (NoOfEle == 2) {
                return (((dashedlinewidth - 3) / 100) * d.original_val);
            //}
            //else
            //return (100 * d.original_val / wdt);
            // return (((x(d.x) + 170 + xShift) - (x(d.x) - 120 + xShift) - 10) * d.original_val) / 100;
        })
        .style("fill", function (d, i) { return color(i); })
        .style("stroke-width", "1px")
        .on("mousemove", function (d) {
            divTooltip.style("left", d3.event.pageX - 50 + "px");
            divTooltip.style("top", d3.event.pageY - 100 + "px");
            divTooltip.style("display", "inline-block");
            divTooltip.style("opacity", 1);
            var x = d3.event.pageX, y = d3.event.pageY
            var elements = document.querySelectorAll(':hover');
            l = elements.length
            l = l - 1
            elementData = elements[l].__data__
            divTooltip.html((elementData.MetricName) + "<br>" + elementData.x + "<br><div style=\"color:" + Get_Significance_Color(elementData.Significance, elementData.customBase, elementData.samplesize) + ";\">" + elementData.original_val.toFixed(1) + "%" + "</div>");
        })
        .on("mouseout", function (d) {
            divTooltip.style("display", "none");
            divTooltip.style("opacity", 0);
        });


    //To append the value
    var layerForText = svg.selectAll(".stackNewForText")
        .data(dataStackLayout)
        .enter().append("g")
        .attr("class", "stackNewForText");



    colorInd = -1;
    colorInd2 = -1;
    layerForText.selectAll("text")
        .data(function (d) {
            return d;
        })
        .enter().append("text")
        .attr("class", "textForOriginalVal")
        .attr("x", 
            function(d){
                if (NoOfEle == 1)
                    return x(d.x) + (width / 3) + xShift + 25;
                else if (NoOfEle == 2)
                    return x(d.x) + ((width / 2) / 3) + xShift + 15;
                else
                    return x(d.x) + 170 + xShift + 20;

            })
        .attr("y", function (d, i) {
            ++colorInd;
            index_Color = Math.floor(colorInd / list.BrandList.length)
            y_Shift = index_Color == 0 ? 1 : 2;
            return (height + 11 - (variableheight * (index_Color + 1)) + 8 + +addFactl);// y(d.y + d.y0);
        })
        .style("text-anchor", "middle")
        .style("fill", function (d) { return Get_Significance_Color(d.Significance, d.customBase, d.samplesize); })
        .style("font-size", "12px")
        .style("font-weight", "bold")
        .text(function (d, i) {
            if (CheckIfStoreFrequencyMeasure(d.x.split('(')[0].trim()) && Measurelist[0].metriclist.length > 0)
                return 'NA';
            else
                return d.original_val.toFixed(1) + '%';
        })
        .on("mousemove", function (d) {
            divTooltip.style("left", d3.event.pageX - 50 + "px");
            divTooltip.style("top", d3.event.pageY - 100 + "px");
            divTooltip.style("display", "inline-block");
            divTooltip.style("opacity", 1);
            var x = d3.event.pageX, y = d3.event.pageY
            var elements = document.querySelectorAll(':hover');
            l = elements.length
            l = l - 1
            elementData = elements[l].__data__
            divTooltip.html((elementData.MetricName) + "<br>" + elementData.x + "<br><div style=\"color:" + Get_Significance_Color(elementData.Significance, elementData.customBase, elementData.samplesize) + ";\">" + parseFloat(elementData.original_val).toFixed(1) + "%" + "</div>");
        })
        .on("mouseout", function (d) {
            divTooltip.style("display", "none");
            divTooltip.style("opacity", 0);
        });

    var layer10 = svg.selectAll(".stackNewForLeft")
          .data(yvalues)
          .enter().append("g")
          .attr("class", "stackNewForLeft")
    .attr("transform", function () {
        if (NoOfEle < 4)
            return "translate(22,0)";
        else
            return "translate(22,0)";
    });

    colorInd = -1;

    layer10.selectAll("text")
           .data(yvalues)
           .enter().append("text")
           .attr("class", "textAtLeft")
             .attr("x", function (d) {
                 return 30+100;//xShift / 2 - 5 - 50;
             })
    .attr("y", function (d, i) {
        ++colorInd;
        index_Color = Math.floor(colorInd / list.BrandList.length)
        y_Shift = index_Color == 0 ? 1 : 2;
        return (height + 21 - (variableheight * (i + 1)) + addFactl);// y(d.y + d.y0);
    })
           .style("text-anchor", "end")
           .text(function (d, i) { return yvalues[i]; });
    if (NoOfEle == 1)
        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(121," + height + ")")
            .call(xAxis);
    else if (NoOfEle == 2)
        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(105," + height + ")")
            .call(xAxis);
    else
    svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(90," + height + ")")
            .call(xAxis);

    d3.selectAll(".x.axis path")
          .attr("display", "none");

    svg.selectAll(".x.axis .tick text")
          .style("font-family", "Arial")
          .style("font-size", "12px")
          .style("font-weight", "bold");

    var el = document.getElementById("mainsvg"); // or other selector like querySelector()
    var rect = el.getBoundingClientRect();
    el.setAttribute("viewBox", "0 0 " + rect.width + " " + rect.height + "");
    $(id).show();
    //$(id).css("overflow","scroll");
}

/*-------------------------------------- Pyramid Chart With Change Value ----------------------------------------------*/

function Plot_Pyramid_Chart_With_Change(data, identifier) {
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain4').css('display', 'block');

    $('#spChartLegend').hide();
    $("#ToShowChart").css("height", "75%");
    $(".trendChartMain").css("height", "116%");

    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    $('#spChartLegend').html('');
    Pyramid_Chart_With_Change(data, '#idtrendChartMain4', identifier);
}

function Pyramid_Chart_With_Change(list, id, identifier) {
   d3.select(id).select("svg").selectAll("*").remove();
    if (list.SampleSize != undefined && list.SampleSize != "" && list.SampleSize.length > 0)
        list.SampleSize = CheckNegativeSampleSize(list.SampleSize);
    d3.select(id).select("svg").remove();
    $(id).empty();
    
var ht2 = $("#chart-title").height();
    $(".showChartMain").css("height", "calc(100% - " + ht2 + "px + 28px)");

    _.forEach(list.BrandList, function (i, j) { list.BrandList[j] = i.trim() });
    _.forEach(list.MetricList, function (i, j) { list.MetricList[j] = i.trim() });

    var color = d3.scale.ordinal()
                .range(["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"]);
    var colorForLegends = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    var colorForTopRect = d3.scale.ordinal()
               .range(["#AE192C", "#2E6878", "#936C0D", "#037C37", "#4D1F77", "#515151", "#820A0B", "#0A517C", "#C5790F", "#818284", "#000000"]);
    //var count = 6;
    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';

    //resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");
    $(id).height("100%");
    //Megha
    $(".showChartMain").css("overflow", "auto");

    //use this height and width for entire chart and adjust here with svg traslation
    height = parseFloat($(id).height() - 100);
    width = parseFloat($(id).width() - 100);

    var NoOfEle = list.BrandList.length, leftFix = 0;
    //var NoOfEle = data.length, leftFix = 0;
    var margin_left_val = 0;

    var margin = { top: 0, right: 30, bottom: 10, left: 50 };
    if (NoOfEle < 4) {
        //width = 250 * NoOfEle;
    }
    else {
        width = 390 * NoOfEle;
    }


    height = parseFloat($(id).height() - 130);
    var variableheight = (13.82 * $(id).height()) / 100;
    var xvalues = [];
    var dataArray = [];
    var yvalues = [];
    for (var i = list.MetricList.length - 1; i >= 0; i--) {
        yvalues.push(list.MetricList[i]);
    }

    $.each(list.BrandList, function (i, v) {
        xvalues.push(v);
    });

    var Total = [];
    for (var i = 0; i < list.BrandList.length; i++) {
        var mat = [];
        Total[i] = 0;
        for (var j = 0; j < list.ValueData.length; j++) {
            Total[i] = Total[i] + list.ValueData[j][i];
        }

    }

    for (var i = 0; i < list.ValueData.length; i++) {
        var mat = [];
        for (var j = 0; j < list.BrandList.length; j++) {
            var obj = {};
            var tempName = "y";
            obj.MetricName = list.MetricList[i];
            obj.samplesize = list.NumberOfResponses[j];
            obj.x = xvalues[j];
            if (CheckIfStoreFrequencyMeasure(list.BrandList[j]) && Measurelist[0].metriclist.length > 0)
                obj.original_val = "0";
            else
                obj.original_val = list.ValueData[i][j];
            //obj.original_val = list.ValueData[i][j];
            obj.changepy = list.ChangeVsPy[i][j];
            obj.customBase = xvalues[j];
            obj.Significance = list.SignificanceData[i][j];
            obj["y"] = list.ValueData[i][j] * 100 / Total[j];
            mat.push(obj);

        }
        dataArray.push(mat);
    }
    if (Selected_StatTest.toLocaleLowerCase().trim() == "custom base" || Sigtype_Id == "1") {
        var sCompList = [];
        if (currentpage.indexOf("deepdive") > -1) {
            if (Grouplist.length > 0)
                $.grep(Grouplist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        else {
            if (Comparisonlist.length > 0)
                $.grep(Comparisonlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (ComparisonBevlist.length > 0)
                $.grep(ComparisonBevlist, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
            else if (Sites.length > 0)
                $.grep(Sites, function (i, j) {
                    sCompList.push(i.Name.toLocaleLowerCase().trim());
                });
        }
        _.each(dataArray, function (i, j) {
            i.sort(function (a, b) {
                return sCompList.indexOf(a.customBase.toLocaleLowerCase().trim()) < sCompList.indexOf(b.customBase.toLocaleLowerCase().trim()) ? -1 : 1;
            });
        });
    }
    var dataArrayNew = [];
    for (var i = dataArray.length - 1 ; i >= 0; i--) {
        dataArrayNew.push(dataArray[i]);
    }
    var x = d3.scale.ordinal()
            .rangeRoundBands([0, width], .35);

    var y = d3.scale.ordinal()
            .rangeRoundBands([0, height]);

    // var color = d3.scale.category20();

    var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom")
            .innerTickSize(0);

    var yAxis = d3.svg.axis()
            .scale(y)
            .orient("left")
            .innerTickSize(10);

    //Append the div for tooltip
    var divTooltip = d3.select(".showChartMain").append("div")
            .attr("class", "d3_tooltip")
            .style("opacity", 0);

    var widthpyr = 100;
    var viewBoxWidth = $(id).width();
    var viewBoxHeight = $(id).height();

    if (NoOfEle > 3) {
        widthpyr = NoOfEle * 34.5;
        viewBoxWidth = NoOfEle * 420;
    }
    if (NoOfEle >= 2) {
        var svg = d3.select(id)
             .classed("svg-container", true) //container class to make it responsive
            .append("svg")
	    .attr("id", "mainsvg")
             //responsive SVG needs these 2 attributes and no width and height attr
             .attr("preserveAspectRatio", "xMinYMin meet")
             //.attr("viewBox", "0 0 " + viewBoxWidth + " 400")
              .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
             //class to make it responsive
             .classed("svg-content-responsive", true)
              .attr("width", function () {
                if (NoOfEle < 4) {
                    return "100%";
                }
                else
                {
                    var percentwidth = ((width / $(id).width()) * 100) + 10;
                    return percentwidth + "%";
        }})
              .attr("height", "100%")
              .style("overflow", "scroll")
              .append("g")
              .attr("transform", "translate(10,70)");
    }
   

    svg.html(defs);

    //  for Static data:- data maping
    var temp, i, j, k;
    var total = [], xData = [], perData = [];

    var dataStackLayout = d3.layout.stack()(dataArrayNew);

    x.domain(dataStackLayout[0].map(function (d) {
        return d.x;
    }));

    y.domain([0,
        d3.max(dataStackLayout[dataStackLayout.length - 1],
                function (d) { return d.y0 + d.y; })
    ]);

    //To add the height and yTrans for charts
    var heightLatest;
    var yTranslate;
    var addFact;
    if (Measurelist.length > 0 && (Measurelist[0].parentname == "Relationship Pyramid" || Measurelist[0].parentname == "CORA Pyramid")) {
        heightLatest = 37.5;
        yTranslate = 60;
        addFact = 4;
        addFactl = 5;
        addFactll = 3;
    }
    else {
        heightLatest = 30;
        yTranslate = 50;
        addFact = 0;
        addFactl = 0;
        addFactll = 0;
    }


    var wdt = 50.5, individual_width = 0, colorInd = -1, y_Shift, index_Color, lineWidth, xShift = x.rangeBand() / 5; xShift = (width / list.BrandList.length) / 5 + 50,
    lineWidth = (width / NoOfEle) / 5;

    if (NoOfEle == 1)
        xShift = (width / list.BrandList.length) / 5 + 50 + 120 - 120;
    else if (NoOfEle == 2)
        xShift = (width / list.BrandList.length) / 5 + 50 + 100;
    else
        xShift = (width / list.BrandList.length) / 5 + 100;

    if (NoOfEle == 1)
        wdt = 12.84;
    if (NoOfEle == 2)
        wdt = 40;//25.9;

    var layer7 = svg.append("g").selectAll(".stackLatest")
          .data(dataStackLayout)
          .enter().append("g")
          .attr("class", "stackLatest")
    var colorInd2 = -1;
    //Append the line 
    layer7.selectAll("line")
       .data(function (d) {
           return d;
       })
       .enter().append("line")
       .attr("id", function (d, i) { return "dashedline"+i;})
       .attr("class", "LineBehindEachRect")
       .attr("x1", function (d) {
           if (NoOfEle == 1)
               return x(d.x) - (width / 3) + xShift + 10;
           else if (NoOfEle == 2)
               return x(d.x) - ((width / 2) / 3) + xShift + 25;
           else
               return x(d.x) - 120 + xShift + 25;

       })
       .attr("y1", function (d, i) {
           ++colorInd;
           index_Color = Math.floor(colorInd / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 15 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .attr("x2", function (d) {
           if (NoOfEle == 1)
               return x(d.x) + (width / 3) + xShift - 10;
           else if (NoOfEle == 2)
               return x(d.x) + ((width / 2) / 3) + xShift - 25/2;
           else
               return x(d.x) + 120 + wdt + xShift - 25/2;
       })
       .attr("y2", function (d, i) {
           ++colorInd2;
           index_Color = Math.floor(colorInd2 / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 15 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .style("stroke", "black")
       .style("stroke-dasharray", "2,2")
       .style("stroke-width", "0.5px")
       .style("opacity", 1);

    colorInd = -1;
    colorInd2 = -1;
    var layer6 = svg.selectAll(".stackLatest1")
      .data(dataStackLayout)
      .enter().append("g")
      .attr("class", "stackLatest1")


    layer6.selectAll("line")
       .data(function (d) {
           return d;
       })
       .enter().append("line")
       .attr("class", "LineBehindEachRect2")
       .attr("x1", function (d) {
           if (NoOfEle == 1)
               return x(d.x) - (width / 3) + xShift + 10;
           else if (NoOfEle == 2)
               return x(d.x) - ((width / 2) / 3) + xShift + 25;
           else
               return x(d.x) - 120 + xShift + 25;//xShift;
       })
       .attr("y1", function (d, i) {
           ++colorInd;
           index_Color = Math.floor(colorInd / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 12 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .attr("x2", function (d) {
           if (NoOfEle == 1)
               return x(d.x) - (width / 3) + xShift + 10;
           else if (NoOfEle == 2)
               return x(d.x) - ((width / 2) / 3) + xShift + 25;
           else
               return x(d.x) - 120 + xShift + 25; //xShift;
       })
       .attr("y2", function (d, i) {
           ++colorInd2;
           index_Color = Math.floor(colorInd2 / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 19 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .style("stroke", "black")
       .style("stroke-width", "1px")
       .style("opacity", 1);


    colorInd = -1;
    colorInd2 = -1;
    var layer5 = svg.selectAll(".stackLatest2")
      .data(dataStackLayout)
      .enter().append("g")
      .attr("class", "stackLatest2")

    layer5.selectAll("line")
       .data(function (d) {
           return d;
       })
       .enter().append("line")
       .attr("class", "LineBehindEachRect3")
       .attr("x1", function (d) {
           if (NoOfEle == 1)
               return x(d.x) + (width / 3) + xShift - 10;
           else if (NoOfEle == 2)
               return x(d.x) + ((width / 2) / 3) + xShift - 25/2;
           else
               return x(d.x) + 120 + wdt + xShift - 25/2; //width -xShift;
       })
       .attr("y1", function (d, i) {
           ++colorInd;
           index_Color = Math.floor(colorInd / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 12 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .attr("x2", function (d) {
           if (NoOfEle == 1)
               return x(d.x) + (width / 3) + xShift - 10;
           else if (NoOfEle == 2)
               return x(d.x) + ((width / 2) / 3) + xShift - 25/2;
           else
               return x(d.x) + 120 + wdt + xShift - 25/2; //width - xShift;
       })
       .attr("y2", function (d, i) {
           ++colorInd2;
           index_Color = Math.floor(colorInd2 / list.BrandList.length)
           y_Shift = index_Color == 0 ? 1 : 2;
           return (height + 19 - (variableheight * (index_Color + 1)) + addFact);// y(d.y + d.y0);
       })
       .style("stroke", "black")
       .style("stroke-width", "1px")
       .style("opacity", 1);



    var layer8 = svg.selectAll(".stackNew")
        .data(dataStackLayout)
        .enter().append("g")
        .attr("class", "stackNew")

    colorInd = -1;
    colorInd2 = -1;

    var element = document.getElementById("dashedline0"); // or other selector like querySelector()
    var line = element.getBBox();
    var dashedlinewidth = line.width;

    layer8.selectAll("rect")
        .data(function (d) {
            return d;
        })
        .enter().append("rect")
        .attr("class", "InnerRect")
        .attr("x", function (d,i) {
            if (NoOfEle == 1)
                return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 15;
            if (NoOfEle == 2) {
                var el = document.getElementById("dashedline" + i);
                var rect = el.getBBox();
                return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) + 1;
                //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 15 - 6;
                //return el.getBBox().x + 2;
                //return x(d.x) + ((((dashedlinewidth - 3) / 100) * d.original_val)) / 2 + xShift + 7 + 1 - 15 - 6;
            }
            else{
                //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1;
       var el = document.getElementById("dashedline" + i);
                var rect = el.getBBox();
                return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) + 1;
       }
        })
        .attr("y", function (d, i) {
            ++colorInd;
            index_Color = Math.floor(colorInd / list.BrandList.length)
            y_Shift = index_Color == 0 ? 1 : 2;
            var context = d3.select(this.parentNode);
            /*SemiRectangle*/
            ////For Lines inside bar            
            //var hgh = Math.floor((100 * d.original_val / wdt - 2) / 6);
            //var yNew = 0;
            //for (var i = 0; i < hgh; i++) {
            context.append("rect")
            .attr("class", "LineRect")
            .attr("height", heightLatest)
            .attr("y", height - (variableheight * (index_Color + 1)))
             .attr("x",
            function () {
                if (NoOfEle == 1)
                    return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 -  17;
                else if (NoOfEle == 2) {
                    var el = document.getElementById("dashedline" + i);
                    var rect = el.getBBox();
                    return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 1;

                    //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 17 - 7;
                }
                else{
		 var el = document.getElementById("dashedline" + i);
                    var rect = el.getBBox();
                    return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 1;

                   // return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift - 4 + 7 + 1
           }
	    })
            .attr("width", 4)
            .style("fill", colorForTopRect(i))// colorForTopRect(d.name))
            .style("opacity", 1)
                .on("mousemove", function () {
                    divTooltip.style("left", d3.event.pageX - 50 + "px");
                    divTooltip.style("top", d3.event.pageY - 90 + "px");
                    divTooltip.style("display", "inline-block");
                    divTooltip.style("opacity", 1);
                    var x = d3.event.pageX, y = d3.event.pageY
                    var elements = document.querySelectorAll(':hover');
                    l = elements.length
                    l = l - 1
                    elementData = elements[l].__data__
                    divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + parseFloat(d.original_val).toFixed(1) + "%" + "</div>");
                })
                 .on("mouseout", function (d) {
                     divTooltip.style("display", "none");
                     divTooltip.style("opacity", 0);
                 });
            //}

            context.append("rect")
               .attr("class", "LineRect")
               .attr("height", heightLatest)
               .attr("y", height - (variableheight * (index_Color + 1)))
               .attr("x",
               function () {
                   if (NoOfEle == 1)
                       return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + (d.original_val * 100 / wdt) + xShift + 7 + 0.5 - 1 - 14;
                   else if (NoOfEle == 2) {
                       var el = document.getElementById("dashedline" + i);
                       var rect = el.getBBox();
                       return rect.x + (rect.width / 2) + ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 2;

                       //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + (d.original_val * 100 / wdt) + xShift + 7 + 0.5 - 1 - 14 - 6;
                   }
                   else{
		    var el = document.getElementById("dashedline" + i);
                       var rect = el.getBBox();
                       return rect.x + (rect.width / 2) + ((((dashedlinewidth - 3) / 100) * d.original_val) / 2) - 2;
                      // return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + (d.original_val * 100 / wdt) + xShift + 7 + 0.5;
              }
	       })
               .attr("width", 4)
               .style("fill", colorForTopRect(i))//colorForTopRect(d.name))
               .style("opacity", 1)
                .on("mousemove", function () {
                    divTooltip.style("left", d3.event.pageX - 50 + "px");
                    divTooltip.style("top", d3.event.pageY - 90 + "px");
                    divTooltip.style("display", "inline-block");
                    divTooltip.style("opacity", 1);
                    var x = d3.event.pageX, y = d3.event.pageY
                    var elements = document.querySelectorAll(':hover');
                    l = elements.length
                    l = l - 1
                    elementData = elements[l].__data__
                    divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + parseFloat(d.original_val).toFixed(1) + "%" + "</div>");
                })
                 .on("mouseout", function (d) {
                     divTooltip.style("display", "none");
                     divTooltip.style("opacity", 0);
                 });

            context.append("rect")
                 .attr("class", "LineRect")
                 .attr("height", 1)
                 .attr("y", height + 15 - (variableheight * (index_Color + 1)) + addFactll)
                .attr("x",
                 function () {
                     if (NoOfEle == 1)
                         return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 0.5 - 1 - 15;
                    else if (NoOfEle == 2) {
                         var el = document.getElementById("dashedline" + i);
                         var rect = el.getBBox();
                         return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2);

                         //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 1 - 17 - 5;
                     }  
		     else{
		      var el = document.getElementById("dashedline" + i);
                         var rect = el.getBBox();
                         return rect.x + (rect.width / 2) - ((((dashedlinewidth - 3) / 100) * d.original_val) / 2);
                         //return x(d.x) + (wdt - (d.original_val * 100 / wdt)) / 2 + xShift + 7 + 0.5 - 1;
                }
		 })
                 .attr("width", function () {
                     //if (NoOfEle == 2) {
                         var element1 = document.getElementById("dashedline0"); // or other selector like querySelector()
                         var line1 = element1.getBBox();
                         var dashedlinewidth1 = line1.width;
                         return (((dashedlinewidth1 - 3) / 100) * d.original_val);
                     //}
                     //else
                         //return (d[i].original_val * 100 / wdt)
                 })
                 .style("fill", colorForTopRect(i))//colorForTopRect(d.name))
                 .style("opacity", 1)
                  .on("mousemove", function () {
                      divTooltip.style("left", d3.event.pageX - 50 + "px");
                      divTooltip.style("top", d3.event.pageY - 90 + "px");
                      divTooltip.style("display", "inline-block");
                      divTooltip.style("opacity", 1);
                      var x = d3.event.pageX, y = d3.event.pageY
                      var elements = document.querySelectorAll(':hover');
                      l = elements.length
                      l = l - 1
                      elementData = elements[l].__data__
                      divTooltip.html((d.MetricName) + "<br>" + d.x + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + parseFloat(d.original_val).toFixed(1) + "%" + "</div>");
                  })
                   .on("mouseout", function (d) {
                       divTooltip.style("display", "none");
                       divTooltip.style("opacity", 0);
                   });

            //}
            return (height - (variableheight * (index_Color + 1)));// y(d.y + d.y0);
        })
        .attr("height", function (d, i) { return 30; })
        .attr("width", function (d) {
            //if (NoOfEle == 2) {
                return (((dashedlinewidth - 3) / 100) * d.original_val);
            //}
            //else
            //return (100 * d.original_val / wdt);
        })
        .style("fill", function (d, i) { return color(i); })
        .style("stroke-width", "1px")
        .on("mousemove", function (d) {
            divTooltip.style("left", d3.event.pageX - 50 + "px");
            divTooltip.style("top", d3.event.pageY - 100 + "px");
            divTooltip.style("display", "inline-block");
            divTooltip.style("opacity", 1);
            var x = d3.event.pageX, y = d3.event.pageY
            var elements = document.querySelectorAll(':hover');
            l = elements.length
            l = l - 1
            elementData = elements[l].__data__
            divTooltip.html((elementData.MetricName) + "<br>" + elementData.x + "<br><div style=\"color:" + Get_Significance_Color(elementData.Significance, elementData.customBase, elementData.samplesize) + ";\">" + parseFloat(elementData.original_val).toFixed(1) + "%" + "</div>");
        })
        .on("mouseout", function (d) {
            divTooltip.style("display", "none");
            divTooltip.style("opacity", 0);
        });


    //To append the value
    var layerForText = svg.selectAll(".stackNewForText")
        .data(dataStackLayout)
        .enter().append("g")
        .attr("class", "stackNewForText");



    colorInd = -1;
    colorInd2 = -1;
    layerForText.selectAll("text")
        .data(function (d) {
            return d;
        })
        .enter().append("text")
        .attr("class", "textForOriginalVal")
        .attr("x",
            function (d) {
                if (NoOfEle == 1)
                    return x(d.x) + (width / 3) + xShift + 35;
                else if (NoOfEle == 2)
                    return x(d.x) + ((width / 2) / 3) + xShift + 25;
                else
                    return x(d.x) + 170 + xShift + 25;

            })
        .attr("y", function (d, i) {
            ++colorInd;
            index_Color = Math.floor(colorInd / list.BrandList.length)
            y_Shift = index_Color == 0 ? 1 : 2;
            return (height + 11 - (variableheight * (index_Color + 1)) + 8 + addFactl);// y(d.y + d.y0);
        })
        .style("text-anchor", "middle")
        .style("fill", function (d) { return Get_Significance_Color(d.Significance, d.customBase, d.samplesize); })
        .style("font-size", "12px")
        .style("font-weight", "bold")
        .text(function (d, i) {
            if (CheckIfStoreFrequencyMeasure(d.x.split('(')[0].trim()) && Measurelist[0].metriclist.length > 0)
                return 'NA |NA';
            else
                return d.original_val.toFixed(1) + '%|' + d.changepy;

        })
        .on("mousemove", function (d) {
            divTooltip.style("left", d3.event.pageX - 50 + "px");
            divTooltip.style("top", d3.event.pageY - 100 + "px");
            divTooltip.style("display", "inline-block");
            divTooltip.style("opacity", 1);
            var x = d3.event.pageX, y = d3.event.pageY
            var elements = document.querySelectorAll(':hover');
            l = elements.length
            l = l - 1
            elementData = elements[l].__data__
            divTooltip.html((elementData.MetricName) + "<br>" + elementData.x + "<br><div style=\"color:" + Get_Significance_Color(elementData.Significance, elementData.customBase, elementData.samplesize) + ";\">" + parseFloat(elementData.original_val).toFixed(1) + "%" + "</div>");
        })
        .on("mouseout", function (d) {
            divTooltip.style("display", "none");
            divTooltip.style("opacity", 0);
        });

    var layer10 = svg.selectAll(".stackNewForLeft")
          .data(yvalues)
          .enter().append("g")
          .attr("class", "stackNewForLeft")
    .attr("transform", function () {
        if (NoOfEle < 4)
            return "translate(22,0)";
        else
            return "translate(0,0)";
    });

    colorInd = -1;

    layer10.selectAll("text")
           .data(yvalues)
           .enter().append("text")
           .attr("class", "textAtLeft")
             .attr("x", function (d) {
                 return 30+100;//xShift / 2 - 5 - 50;
             })
    .attr("y", function (d, i) {
        ++colorInd;
        index_Color = Math.floor(colorInd / list.BrandList.length)
        y_Shift = index_Color == 0 ? 1 : 2;
        return (height + 21 - (variableheight * (i + 1)) + addFactl);// y(d.y + d.y0);
    })
           .style("text-anchor", "end")
           .text(function (d, i) { return yvalues[i]; });

    if (NoOfEle == 1)
        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(121," + height + ")")
            .call(xAxis);
    else if (NoOfEle == 2)
        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(105," + (height - 5) + ")")
            .call(xAxis);
    else
        svg.append("g")
                .attr("class", "x axis")
                .attr("transform", "translate(90," + (height - 5) + ")")
                .call(xAxis);

    d3.selectAll(".x.axis path")
          .attr("display", "none");

    svg.selectAll(".x.axis .tick text")
          .style("font-family", "Arial")
          .style("font-size", "12px")
          .style("font-weight", "bold");

    var el = document.getElementById("mainsvg"); // or other selector like querySelector()
    var rect = el.getBoundingClientRect();
    el.setAttribute("viewBox", "0 0 " + rect.width + " " + rect.height + "");
    $(id).show();
}

/*----------------------------------------------- Line Chart  One ------------------------------------------------------*/

function GoToChartFunctionForLineChartOne(data, identifier) {
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain00').css('display', 'block');
    SelectedChart = "Line";
    var colorArray = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    plotLineChartOne(data, '#idtrendChartMain00', identifier);
    //plotLegends(data, identifier, colorArray);
    plotLegends_Line(data, identifier, colorArray);
}

function plotLineChartOne(list, id, identifier) {
    if (list.SampleSize != undefined && list.SampleSize != "" && list.SampleSize.length > 0)
        list.SampleSize = CheckNegativeSampleSize(list.SampleSize);
    d3.select(id).select("svg").remove();
    $(id).empty();

    var xvalues = [];
    var colorArray = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    var data = [], mainData = [], NonZeroData = [];
    var count = 6;
    var max = 0;

    if (identifier == 1) {


        $.each(list.MetricList, function (i, v) {
            xvalues.push(v);
        });

        $.each(list.BrandList, function (i, v) {

            data = [];
            for (var j = 0; j < xvalues.length; j++) {
                var obj = {};
                obj.MetricName = list.BrandList[i];
                obj.sale = list.ValueData[j][i];
                obj.year = xvalues[j];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = list.ArrayIndexList[i];
                }
                data.push(obj);
                if (list.ValueData[j][i] >= max)
                    max = list.ValueData[j][i];
                
            }

            mainData.push(data);
        });

    }
    else {


        $.each(list.BrandList, function (i, v) {
            xvalues.push(v);
        });

        $.each(list.MetricList, function (i, v) {

            data = [];
            for (var j = 0; j < xvalues.length; j++) {

                var obj = {};
                obj.MetricName = list.MetricList[i];
                obj.sale = list.ValueData[i][j];
                if (sTrendType == "1")
                    obj.year = xvalues[j] + " (" + addCommas(list.SampleSize[j]) + ")";
                else
                    obj.year = xvalues[j] + " (NA)";
                obj.customBase = xvalues[j];
                obj.Significance = list.SignificanceData[i][j];
                obj.samplesize = list.NumberOfResponses[j];
                if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {
                    obj.index = list.ArrayIndexList[i];
                }
                data.push(obj);
                if (list.ValueData[i][j] >= max)
                    max = list.ValueData[i][j];
                
            }

            mainData.push(data);
        });
    }

    

    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';

    resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");

    //use this height and width for entire chart and adjust here with svg traslation
    height = parseFloat($(id).height() - 100);
    width = parseFloat($(id).width() - 70);


    var x = d3.scale.ordinal()
        .domain(data.map(function (d) { return d.year; }))
        .rangeRoundBands([0, width]);

    var y = d3.scale.linear()
        .domain([0, max + 20])//set the maximum of the value,responsible for setting y axis labels and measurement
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    //setting the tick values
    var formatPercent = d3.format(".00%");
    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .tickFormat(function (d) { return d + "%" });
        //.innerTickSize(-width)
        //.ticks(6)

    var viewBoxWidth = $("#idtrendChartMain00").width();
    var viewBoxHeight = $("#idtrendChartMain00").height();

    var chart = d3.select(id)
        .classed("svg-container", true) //container class to make it responsive
        .append("svg")
        .attr("preserveAspectRatio", "xMidYMid meet")
    .attr("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight + "")
   //class to make it responsive
   .classed("svg-content-responsive", true)
        .attr("width", "100%")
        .attr("height", "100%")
      .append("g")
     .attr("transform", "translate(40,30)");
       // .attr("transform", "translate(" + margin.left + "," + margin.top + ")");



    //var chart = d3.select(id).append("svg")
    //   .attr("width", "99%")
    //   .attr("height", "99%")
    //   .append("g")
    //   .attr("transform", "translate(40,30)");

    chart.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis)
    d3.selectAll(".x.axis path,.x.axis line ")
        .style("fill", "none")
        .style("stroke", "#000")
        .style("shape-rendering", "geometricPrecision")
        .style("font-size", "10px")
        .style("font-family", "Arial");

    //break the text at x-axis
    d3.selectAll(".tick text")
       .call(wrap, x.rangeBand());

    
    chart.append("g")
            .attr("class", "y axis")
           //.style("fill", "none")
        //.style("stroke", "#000")
        //.style("shape-rendering", "crispEdges")
            //.style("stroke-width", "1px")
            .call(yAxis);


    d3.selectAll(".y.axis path,.y.axis line")
        .style("fill", "none")
        .style("stroke", "#000")
        .style("shape-rendering", "geometricPrecision")
        .style("font-size", "10px")
        .style("font-family", "Arial");


    //one hidden div for tooltip
    var div = d3.select(".showChartMain").append("div")
        .attr("class", "d3_tooltip")
        .style("opacity", 0);

    var line = d3.svg.line()
   .x(function (d) {
       var tmpVal = x(d.year) + x.rangeBand() / 2;
       return tmpVal;
   })
   .y(function (d) { return y(d.sale); });

    //arc for mouse over
    var arc = d3.svg.arc()
          .innerRadius(14)
          .outerRadius(6)
          .startAngle(0)
          .endAngle(2 * Math.PI);

    //drawing line chart start
    for (var i = 0; i < mainData.length; i++) {
        chart.append("path")
          .attr("d", line(mainData[i]))
          .data(mainData[i])
          .attr('fill', 'none')
          .attr("class", "lineData" + i)
          .attr("stroke", function (d) {
              if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {

                  return colorArray[d.index.split(';')[1]];
              }
              else
                  return colorArray[i];
          })
          .style("stroke-dasharray", "3,3")
          .style("stroke-width", "2.5px");
        countID = 0;//resetting the count for id

        //Append Outer Circle
        chart.selectAll("dot")
               .data(mainData[i])
               .enter().append("circle")
               .attr("cx", function (d) { return x(d.year) + x.rangeBand() / 2; })
               .attr("cy", function (d) { return y(d.sale); })
               .attr("id", function (d) {
                   var tmpCount = ++countID;
                   return "circle" + i + "_" + tmpCount;
               })
               .attr("class", "lineData" + i)
               .attr("r", 8)
               .style("fill", "none")
               .attr("stroke", function (d) {
                   if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {

                       return colorArray[d.index.split(';')[1]];
                   }
                   else
                       return colorArray[i];
               })
               .attr("stroke-width", "1px");

        //Append Inner Circle
        chart.selectAll("dot")
                .data(mainData[i])
                .enter().append("circle")
                .attr("cx", function (d) { return x(d.year) + x.rangeBand() / 2; })
                .attr("cy", function (d) { return y(d.sale); })
                .attr("id", function (d) {
                    /*Data Label*/
                    var context = d3.select(this.parentNode);
                    context.append("text")
                        .attr("x", x(d.year) + x.rangeBand() / 2 - 7)
                        .attr("y", y(d.sale) - 10)
                        .text(d.sale.toFixed(1) + "%")
                        .style("fill", Get_Significance_Color(d.Significance, d.customBase, d.samplesize));
                    /*Data Label*/
                    var tmpCount = ++countID;
                    return "circle" + i + "_" + tmpCount;
                })
                .attr("class", "lineData" + i)
                .attr("r", 5)
                .attr("fill", function (d) {
                    if (sRemovedLegendPosition != [] && sRemovedLegendPosition != null && sRemovedLegendPosition != "null" && sRemovedLegendPosition.length > 0) {

                        return colorArray[d.index.split(';')[1]];
                    }
                    else
                        return colorArray[i];
                })
        //for tooltip we need above hidden div
        .on("mouseover", function (d) {

            div.transition()
                .duration(200)
                .style("opacity", .9);
            if (d.sale >= 50) {
                div.html(d.year + "<br>" + d.MetricName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.sale.toFixed(1) + "%" + "<span class='toolimg1'></span>" + "</div>")
                    .style("left", (d3.event.pageX - 60) + "px")
                    .style("top", (d3.event.pageY - 60) + "px");
            }

            if (d.sale < 50) {
                div.html(d.year + "<br>" + d.MetricName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.sale.toFixed(1) + "%" + "<span class='toolimg1'></span>" + "</div>")
                    .style("left", (d3.event.pageX - 60) + "px")
                    .style("top", (d3.event.pageY - 60) + "px");
            }

            //for arc
            var firstX = $($(this)[0]).attr('cx'),
            firstY = $($(this)[0]).attr('cy'),
            color = $($(this)[0]).attr('fill');
            chart.append("path")
           .attr("d", arc)
           .attr("class", "overArc")
           .style("fill", color)
           .style("opacity", .5)
           .attr("transform", "translate(" + firstX + "," + firstY + ")")
        })
        .on("mouseout", function (d, i) {
            div.transition()
                .duration(500)
                .style("opacity", 0);

            //hiding the arc
            $('.overArc').hide();

        });
    }

}

/*----------------------------------------------- Line Chart  Two ------------------------------------------------------*/

function GoToChartFunctionForLineChartTwo(data, identifier) {
    $('.trendChartMain').css('display', 'none');
    $('#idtrendChartMain11').css('display', 'block');

    var colorArray = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB", "#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];


    plotLineChartNewTwo(data, '#idtrendChartMain11', identifier);
    plotLegendsFor_LineChart(data, identifier, colorArray);
}

function plotLineChartNewTwo(list, id, identifier) {
    if (list.SampleSize != undefined && list.SampleSize != "" && list.SampleSize.length > 0)
        list.SampleSize = CheckNegativeSampleSize(list.SampleSize);
    d3.select(id).select("svg").remove();
    $(id).empty();

    var xvalues = [];
    var colorArray = ["#E41E2B", "#31859C", "#FFC000", "#00B050", "#7030A0", "#7F7F7F", "#C00000", "#0070C0", "#FF9900", "#D2D9DF", "#000000", "#838C87", "#83E5BB"];
    var data = [], mainData = [],NonZeroData = [];
    var count = 6;
    var max = 0;

    if (identifier == 1) {


        $.each(list.MetricList, function (i, v) {
            xvalues.push(v);
        });

        $.each(list.BrandList, function (i, v) {

            data = [];
            for (var j = 0; j < xvalues.length; j++) {
                var obj = {};
                obj.MetricName = list.BrandList[i];
                obj.sale = list.ValueData[j][i];
                obj.year = xvalues[j];
                data.push(obj);
                if (list.ValueData[j][i] >= max)
                    max = list.ValueData[j][i];
            }

            mainData.push(data);
        });

    }
    else {


        $.each(list.BrandList, function (i, v) {
            xvalues.push(v);
        });

        $.each(list.MetricList, function (i, v) {

            data = [];
            for (var j = 0; j < xvalues.length; j++) {

                var obj = {};
                obj.MetricName = list.MetricList[i];
                obj.sale = list.ValueData[i][j];
                obj.year = xvalues[j];
                data.push(obj);
                if (list.ValueData[i][j] >= max)
                    max = list.ValueData[i][j];
            }

            mainData.push(data);
        });
    }

  

    //finding width according to the value
    var mainParentWidth = $('.showChartMain').width();
    var resWidth = '';

    resWidth = (count <= 15) ? mainParentWidth : (mainParentWidth + (72.5 * (count - 15)));//manually find this 15 maximum bar we can accomodate without slider
    //1088/15=72.5 1088 is the width of mainParent(chartParent) without scroll
    $(id).css('width', resWidth + "px");

    //use this height and width for entire chart and adjust here with svg traslation
    height = parseFloat($(id).height() - 120);
    width = parseFloat($(id).width() - 100);


    var x = d3.scale.ordinal()
        .domain(data.map(function (d) { return d.year; }))
        .rangeRoundBands([0, width]);//adjust here to reduce 
    //bar width and space between then

    var y = d3.scale.linear()
        .domain([0, max])//set the maximum of the value,responsible for setting y axis labels and measurement
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    //setting the tick values
    var formatPercent = d3.format(".0%");
    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .tickFormat(formatPercent);

    var chart = d3.select(id).append("svg")
       .attr("width", "99%")
       .attr("height", "99%")
       .append("g")
       .attr("transform", "translate(40,30)");

    chart.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis);

     chart.append("g")
            .attr("class", "y axis")
            //.style("fill", "transparent")
            .style("stroke", "black")
            //.style("stroke-width", "0")
            .call(yAxis)

    d3.selectAll(".x.axis path,.x.axis line ")
        .style("fill", "none")
        .style("stroke", "#000")
        .style("shape-rendering", "crispEdges")
        .style("font-size", "12px")
        .style("font-family", "Arial");

    //break the text at x-axis
    d3.selectAll(".tick text")
      .call(wrap, x.rangeBand() + 5);

    //one hidden div for tooltip
    var div = d3.select(".showChartMain").append("div")
        .attr("class", "d3_tooltip")
        .style("opacity", 0);

    var line = d3.svg.line()
   .x(function (d) {
       var tmpVal = x(d.year) + x.rangeBand() / 2;
       return tmpVal;
   })
   .y(function (d) { return y(d.sale); });

    //arc for mouse over
    var arc = d3.svg.arc()
      .innerRadius(14)
      .outerRadius(6)
      .startAngle(0)
      .endAngle(2 * Math.PI);

    //drawing line chart start
    for (var i = 0; i < mainData.length; i++) {
        chart.append("path")
          .attr("d", line(mainData[i]))
          .data(mainData[i])
          .attr('fill', 'none')
          .attr("class", "lineData" + i)
          .attr("stroke", colorArray[i]);
        //.style("stroke-dasharray", "5,5");
        countID = 0;//resetting the count for id

        //Append  Circle At each point
        chart.selectAll("dot")
            .data(mainData[i])
            .enter().append("circle")
            .attr("cx", function (d) { return x(d.year) + x.rangeBand() / 2; })
            .attr("cy", function (d) { return y(d.sale); })
            .attr("id", function (d) {
                var tmpCount = ++countID;
                return "circle" + i + "_" + tmpCount;
            })
            .attr("class", "lineData" + i)
            .attr("r", 5)
            .attr("fill", colorArray[i])
        //for tooltip we need above hidden div
         .on("mouseover", function (d) {

            div.transition()
                .duration(200)
                .style("opacity", .9);
            if (d.sale >= 50) {
                div.html(d.year + "<br>" + d.MetricName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.sale.toFixed(1) + "%" + "<span class='toolimg1'></span>" + "</div>")
                    .style("left", (d3.event.pageX - 60) + "px")
                    .style("top", (d3.event.pageY - 60) + "px");
            }

            if (d.sale < 50) {
                div.html(d.year + "<br>" + d.MetricName + "<br><div style=\"color:" + Get_Significance_Color(d.Significance, d.customBase, d.samplesize) + ";\">" + d.sale.toFixed(1) + "%" + "<span class='toolimg1'></span>" + "</div>")
                    .style("left", (d3.event.pageX - 60) + "px")
                    .style("top", (d3.event.pageY - 60) + "px");
            }

            //for arc
            var firstX = $($(this)[0]).attr('cx'),
            firstY = $($(this)[0]).attr('cy'),
            color = $($(this)[0]).attr('fill');
            chart.append("path")
           .attr("d", arc)
           .attr("class", "overArc")
           .style("fill", color)
           .style("opacity", .5)
           .attr("transform", "translate(" + firstX + "," + firstY + ")")
        })
         .on("mouseout", function (d, i) {
            div.transition()
                .duration(500)
                .style("opacity", 0);

            //hiding the arc
            $('.overArc').hide();

        });
    }

}

/*------------------------------------------------- Commom Funtions ---------------------------------------------------*/

function wrap(text, width) {
    text.each(function () {
        var text = d3.select(this),
            words = text.text().trim().split(/\s+/).reverse(),
            word,
            line = [],
            lineNumber = 0,
            lineHeight = 1.1, // ems
            y = text.attr("y"),
            dy = parseFloat(text.attr("dy")),
            tspan = text.text(null).append("tspan").attr("x", 0).attr("y", y).attr("dy", dy + "em");
        while (word = words.pop()) {
            line.push(word);
            tspan.text(line.join(" "));
            if (tspan.node().getComputedTextLength() > width) {
                line.pop();
                tspan.text(line.join(" "));
                line = [word];
                tspan = text.append("tspan").attr("x", 0).attr("y", y).attr("dy", ++lineNumber * lineHeight + dy + "em").text(word);
            }
        }
    });
}

function verticalWrap(text, width) {
    text.each(function () {
        var text = d3.select(this),
                words = text.text().indexOf("/") > -1 ? cleanArray(text.text().split(/[ ]+|(\/)/g)).reverse() : text.text().split(/\s+/).reverse(),
                word,
                line = [],
                lineNumber = 0,
                lineHeight = 1.0, // ems
                y = text.attr("y"),
                x = text.attr("x"),
                dy = parseFloat(text.attr("dy")),
                tspan = text.text(null).append("tspan").attr("x", x).attr("y", y).attr("dy", dy + "em");
        while (word = words.pop()) {
            line.push(word);
            tspan.text(line.join(" "));
            if (tspan.node().getComputedTextLength() > width) {
                line.pop();
                tspan.text(line.join(" "));
                line = [word];
                tspan = text.append("tspan").attr("x", x).attr("y", y).attr("dy", ++lineNumber * lineHeight + dy + "em").text(word);
            }
        }
    });
}

function cleanArray(actual) {
    var newArray = new Array();
    for (var i = 0; i < actual.length; i++) {
        if (actual[i]) {
            newArray.push(actual[i]);
        }
    }
    return newArray;
}

function type(d) {
    d.value = +d.value;
    return d;
}

function plotLegends(data, identifier, _colorForLegends) {
    sCurrentChartData = data;
    sCurrentIdentifier = identifier;

    var colorForLegends = [];
    colorForLegends = _colorForLegends;
    var htmlString = '';
    if (identifier == 1) {
        var length = data.BrandList.length;
        for (var i = 0; i < length; i++) {
            htmlString += '<div id="spLegend' + (i + 1) + '" class="spLegend" position="'+ i +'">';
            htmlString += '<div id="spIcon' + (i + 1) + '" class="spIcon"></div>';
            htmlString += '<div id="spText"' + (i + 1) + '" class="spText">' + data.BrandList[i] + " (" + addCommas(data.SampleSize[i]) + ")" + '</div>';
            htmlString += '</div>';

        }
        
        $('#spChartLegend').html(htmlString);
        for (var i = 0; i < length; i++) {
            $("#spIcon" + (i + 1)).css("background-color", colorForLegends[i]);
        }

    }

    else {
        var length = data.MetricList.length;
        for (var i = 0; i < length; i++) {
            htmlString += '<div id="spLegend' + (i + 1) + '" class="spLegend" position="' + i + '">';
            htmlString += '<div id="spIcon' + (i + 1) + '" class="spIcon"></div>';
            htmlString += '<div id="spText"' + (i + 1) + '" class="spText">' + data.MetricList[i] + '</div>';
            htmlString += '</div>';

        }
        $('#spChartLegend').html(htmlString);
        for (var i = 0; i < length; i++) {
            $("#spIcon" + (i + 1)).css("background-color", colorForLegends[i]);
        }
    }

}

function plotLegends_Line(data, identifier, _colorForLegends) {
    sCurrentChartData = data;
    sCurrentIdentifier = identifier;

    var colorForLegends = [];
    colorForLegends = _colorForLegends;
    var htmlString = '';
    if (identifier == 1) {
        var length = data.BrandList.length;
        for (var i = 0; i < length; i++) {
            htmlString += '<div id="spLegend' + (i + 1) + '" class="spLegendTrend" position="' + i + '">';
            htmlString += '<div id="spIcon' + (i + 1) + '" class="spIcon"></div>';
            htmlString += '<div id="spText' + (i + 1) + '" class="spTextTrend">' + data.BrandList[i] + " (" + addCommas(data.SampleSize[i]) + ")" + '</div>';
            htmlString += '</div>';

        }

        $('#spChartLegend').html(htmlString);
        for (var i = 0; i < length; i++) {
            //$("#spIcon" + (i + 1)).css("background-color", colorForLegends[i]);
            $("#spIcon" + (i + 1)).append('<svg style="width:100%;height:100%;"><g zindex="1" style="cursor: pointer;width: 100%;height: 100%;"><path fill="none" d="M 0 11 L 16 11" stroke-dasharray="2.5,2.5" stroke="' + colorForLegends[i] + '" stroke-width="2.5"></path><path fill="' + colorForLegends[i] + '" d="M 8 5 C 15.992 5 15.992 17 8 17 C 0.008000000000000007 17 0.008000000000000007 5 8 5 Z" stroke="#FAFAFA" stroke-width="3"></path></g></svg>');
        }

    }

    else {
        var length = data.MetricList.length;
        for (var i = 0; i < length; i++) {
            htmlString += '<div id="spLegend' + (i + 1) + '" class="spLegendTrend" position="' + i + '">';
            htmlString += '<div id="spIcon' + (i + 1) + '" class="spIcon"></div>';
            //htmlString += '<div id="spIcon' + (i + 1) + '" class="spIcon"></div>';
            htmlString += '<div id="spText' + (i + 1) + '" class="spTextTrend">' + data.MetricList[i] + '</div>';
            htmlString += '</div>';

        }
        $('#spChartLegend').html(htmlString);
        for (var i = 0; i < length; i++) {
            // $("#spIcon" + (i + 1)).css("background-color", colorForLegends[i]);
            $("#spIcon" + (i + 1)).append('<svg style="width:100%;height:100%;"><g zindex="1" style="cursor: pointer;width: 100%;height: 100%;"><path fill="none" d="M 0 11 L 16 11" stroke-dasharray="2.5,2.5" stroke="' + colorForLegends[i] + '" stroke-width="2.5"></path><path fill="' + colorForLegends[i] + '" d="M 8 5 C 15.992 5 15.992 17 8 17 C 0.008000000000000007 17 0.008000000000000007 5 8 5 Z" stroke="#FAFAFA" stroke-width="3"></path></g></svg>');
        }
    }

}

function plotLegendsFor_LineChart(data, identifier, _colorForLegends) {
    var colorForLegends = [];
    colorForLegends = _colorForLegends;
    var htmlString = '';
    if (identifier == 1) {
        var length = data.BrandList.length;
        for (var i = 0; i < length; i++) {
            htmlString += '<div id="spLegend' + (i + 1) + '" class="spLegend">';
            htmlString += '<div id="spIcon' + (i + 1) + '" class="spIcon"></div>';
            htmlString += '<div id="spText' + (i + 1) + '" class="spTextTrend">' + data.BrandList[i] + '</div>';
            htmlString += '</div>';

        }
        $('#spChartLegend').html(htmlString);
        for (var i = 0; i < length; i++) {
            $("#spIcon" + (i + 1)).css("background-color", colorForLegends[i]);
        }

    }

    else {
        var length = data.MetricList.length;
        for (var i = 0; i < length; i++) {
            htmlString += '<div id="spLegend' + (i + 1) + '" class="spLegend">';
            htmlString += '<div id="spIcon' + (i + 1) + '" class="spIcon"></div>';
            htmlString += '<div id="spText' + (i + 1) + '" class="spTextTrend">' + data.MetricList[i] + '</div>';
            htmlString += '</div>';

        }
        $('#spChartLegend').html(htmlString);
        for (var i = 0; i < length; i++) {
            $("#spIcon" + (i + 1)).css("background-color", colorForLegends[i]);
        }
    }

}

function GetDefaultFrequency() {
    if (sVisitsOrGuests != "") {

        if (sVisitsOrGuests == Number("1")) {
            if (currentpage.indexOf("beverage") > -1) {
                SelectedFrequencyList = [];
                selectedChannels = [];
                $("#RightPanelPartial #channel-content ul li[name='TOTAL']").removeClass("Selected");
                $("#RightPanelPartial #channel-content ul li[name='TOTAL']").trigger("click");
            }
            else if (currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") {
                SelectedFrequencyList = [];
                selectedChannels = [];
                $("#RightPanelPartial .rgt-cntrl-ordertype ul li[name='TOTAL']").removeClass("Selected");
                $("#RightPanelPartial .rgt-cntrl-ordertype ul li[name='TOTAL']").trigger("click");
            }
            else {
                selectedChannels = [];
                SelectedFrequencyList = [];
                if (currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive")
                    addfilter("frequency_containerId", getFilter("Cross-Retailer Shopper (Trips)"), "SelectFrequency(this);");
                else
                    addfilter("frequency_containerId", getFilter("Trips Frequency"), "SelectFrequency(this);");

                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").removeClass("Selected");
                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
            }
        }
        else if (sVisitsOrGuests == Number("2")) {
            SelectedAdvFilterList = [];
            if (currentpage.indexOf("beverage") > -1) {
                selectedChannels = [];
                SelectedFrequencyList = [];
                $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").removeClass("Selected");
                $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
            }
            else if (currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") {
                SelectedFrequencyList = [];
                selectedChannels = [];
                addfilter("shopper_frequency_containerId", getFilter("E-Com Shopper Frequency"), "SelectFrequency(this);");
                $("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").removeClass("Selected");
                $("#RightPanelPartial #shopper_frequency_containerId ul li[name='P12M']").trigger("click");
            }
            else {
                selectedChannels = [];
                SelectedFrequencyList = [];
                $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").removeClass("Selected");
                $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
                if (currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive") {
                    addfilter("frequency_containerId", getFilter("Cross-Retailer Shopper (Shoppers)"), "SelectFrequency(this);");
                    $("#RightPanelPartial #frequency_containerId ul li[parentname='MONTHLY +' ][name='SELECTION']").trigger("click");
                }

                
            }
        }
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

function sortNumber(a, b) {
    return a - b;
}

function NoDataAvailable() {
    $(".showChartMain").hide();
    var table = "<div style=\"width:99%;height:20%;text-align:center;border:1px solid grey;margin:auto;\"><span style=\"color:black;\" class=\"no-data\">No data available</span></div>";
    $(".ChartDivArea").html("");
    $(".ChartDivArea").html(table);
}

function CheckIfStoreFrequencyMeasure(sRetailerName) {
    var sMeasureName = Measurelist[0].Name;
    if (((sMeasureName.trim() == "Store Imagery Summary")))
        if (sRetailerName.toUpperCase().trim() == "CONVENIENCE" || sRetailerName.toUpperCase().trim() == "DRUG" || sRetailerName.toUpperCase().trim() == "DOLLAR" || sRetailerName.toUpperCase().trim() == "CLUB" || sRetailerName.toUpperCase().trim() == "MASS MERC." || sRetailerName.toUpperCase().trim() == "SUPERMARKET/GROCERY" || sRetailerName.toUpperCase().trim() == "SUPERCENTER" || sRetailerName.toUpperCase().trim() == "TOTAL TRIPS" || sRetailerName.toUpperCase().trim() == "TOTAL SHOPPER")
            return true;
        else
        {
            for (var i = 0; i < ChartModuleData.BrandList.length; i++)
            {
                if (sRetailerName.toUpperCase().trim() == ChartModuleData.BrandList[i].toUpperCase().trim() && ChartModuleData.SampleSize[i] == "NA")
                {
                    return true;
                    break;
                }
            }
        }
}

function CheckNegativeSampleSize(SampleSizeList) {
    var sSamplArray = [];
    _.each(SampleSizeList, function (i, j) {
        (i == -10000 ? (sSamplArray.push("NA")) : sSamplArray.push(i))
    });
    return sSamplArray;
}

function getSorted(arr, sortArr) {
    var result = [];
    for (var i = 0; i < arr.length; i++) {
        result[i] = arr[sortArr[i]];
    }
    return result;
}