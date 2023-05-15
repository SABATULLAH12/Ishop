/// <reference path="../jquery-1.8.2.min.js" />
var selectedMetric = ''
var lowSamplePopup = 0;
var chartData = [];
function makeChartData(data) {
    //console.log("data", data)
    selectedMetric = data[1].MetricType;
    //console.log("sel",selectedMetric)
    //data = [{
    //    "name": "12MMT Jun 2018 Visits",
    //    "value":1000
    //},
    //{
    //    "name": "Morning(6AM to 11AM)",
    //    "value": 100
    //},
    //{
    //    "name": "Mid-Day(11AM to 2PM)",
    //    "value": 200
    //},
    //{
    //    "name": "Day(2PM to 5PM)",
    //    "value": -90
    //},
    //{
    //    "name": "Evening(5PM to 10PM)",
    //    "value": 500
    //},
    //{
    //    "name": "Night(10PM to 6AM)",
    //    "value": -110
    //},
    //{
    //    "name": "12MMT Jun 2019 Visits",
    //    "value": 1600
    //}]
    var plottingData = [];
    var cumulative = data[0].Volume;

    var dataPoint = { "Metric": '', "start": 0, "end": 0, "class": 'positive' }
    dataPoint.Metric = data[0].Metric;
    dataPoint.start = 0;
    dataPoint.Volume = data[0].Volume;
    dataPoint.Volume = dataPoint.Volume == -10000 ? 0 : dataPoint.Volume;
    dataPoint.DisplayValue = data[0].DisplayValue;
    dataPoint.Share = "% of Trips";
    dataPoint.ChangePercentage = "% Change in Trips";
    dataPoint.end = dataPoint.Volume;
    dataPoint.class = 'start'
    dataPoint.isTotal = 1;//start bar
    dataPoint.class = (dataPoint.Volume.toFixed(1) == 0) ? 'zero' : dataPoint.class
    plottingData.push(dataPoint);

    for (var i = 1; i < data.length - 1; i++) {
        var dataPoint = { "Metric": '', "start": 0, "end": 0, "class": 'positive' }
        dataPoint.Metric = data[i].Metric;
        dataPoint.start = cumulative;
        dataPoint.Volume = data[i].Volume;
        dataPoint.DisplayValue = data[i].DisplayValue;
        dataPoint.Share = data[i].Share;
        dataPoint.ChangePercentage = data[i].ChangePercentage;
        dataPoint.Volume = dataPoint.Volume == -10000 ? 0 : dataPoint.Volume;
        cumulative += dataPoint.Volume;
        dataPoint.end = cumulative;
        dataPoint.isTotal = 0; //Middle bars
        dataPoint.class = (dataPoint.Volume >= 0) ? 'positive' : 'negative'
        dataPoint.class = (dataPoint.Volume.toFixed(1) == 0) ? 'zero' : dataPoint.class
        plottingData.push(dataPoint);
    }

    var dataPoint = { "Metric": '', "start": 0, "end": 0, "class": 'positive' }
    dataPoint.Metric = data[data.length - 1].Metric;
    dataPoint.start = 0;
    dataPoint.Volume = data[i].Volume;
    dataPoint.DisplayValue = data[data.length - 1].DisplayValue;
    dataPoint.Share = data[data.length - 1].Share;
    dataPoint.ChangePercentage = data[data.length - 1].ChangePercentage;
    dataPoint.Volume = dataPoint.Volume == -10000 ? 0 : dataPoint.Volume;
    dataPoint.end = dataPoint.Volume;
    dataPoint.isTotal = 2; //end bar
    dataPoint.class = 'end'
    dataPoint.class = (dataPoint.Volume.toFixed(1) == 0) ? 'zero' : dataPoint.class
    plottingData.push(dataPoint);


    //console.log("plott", plottingData)
    $("#RightPanel").css("background", "none");
    plotWaterfallChart(plottingData);
}

function makeParamsForSelections() {
    // parameters to pass to the sp
    var params = new Object();
    params.TimePeriod = TimePeriod;
    params.TimePeriod_UniqueId = TimePeriod_UniqueId;
    params.TimePeriodShortName = $(".timeType").val();

    //For the retailer selected
    if (Comparisonlist.length > 0) {
        params.Comparison_ShortNames = Comparisonlist[0].Name;
        params.Comparison_UniqueIds = Comparisonlist[0].UniqueId;
    }

    //For the filters
    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_UniqueId = [];
    //Guest advanced filters
    for (var i = 0; i < SelectedDempgraphicList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].parentName + "|" + SelectedDempgraphicList[i].Name);
        Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
    }
    params.Filter = Advanced_Filters_DBNames.join("|");
    params.FilterShortname = Advanced_Filters_ShortNames.join("|");
    params.Filter_UniqueId = Advanced_Filters_UniqueId.join("|");

    //Measures
    var sMetricNames = "";
    var sMetric = [];

    for (var i = 0; i < Measurelist[0].metriclist.length; i++) {
        sMetricNames += Measurelist[0].metriclist[i].Name + "|";
        sMetric.push(Measurelist[0].metriclist[i].Id);
    }
    sMetricNames = sMetricNames.substring(0, sMetricNames.length - 1);
    params.selectedMetrics = sMetric;
    params.SelectedMetricsNames = sMetricNames;
    params.selectedMetrics = sMetric.join('|');
    params.MetricShortName = Measurelist[0].parentName.toUpperCase();
    params.SelectedMetricsIds = sMetric.join('|');

    return params;
}

function prepareContentArea() {
    KillChart();
    if (!Validate_CompareRetailers_Charts() || !Validate_Measures_Charts()) {
        return false;
    }

    // parameters to pass to the sp
    var params = makeParamsForSelections();

    postBackData = '{"EstablishmentDeepDiveParams":' + JSON.stringify(params) + '}';
    jQuery.ajax({
        type: "POST",
        url: $("#URLESTABLISHMENTDEEPDIVE").val(),
        data: postBackData,
        contentType: "application/json",
        success: function (data) {
            ShowLoader();
            if (!isAuthenticated(data))
                return false;
            chartData = data;
            checkForLowSampleSize(data);
            //makeChartData(data);
        },
        error: function (xhr, status, error) {
            //showMessage(xhr.responseText)
            console.log("err", error);
        }
    })
}

function checkForLowSampleSize(data) {
    $('.stat-submt.save-proceed-btn.proceedClick').show();
    if (data[0].SampleSize < 30 || data[data.length - 1].SampleSize < 30) {
        $('.stat-submt.save-proceed-btn.proceedClick').hide();
        $('#lowsample-popup .stat-heading.heading_text').text('Sample size is less than 30 for following')
        showPopUp()
    }
    else if (data[0].SampleSize < 100 || data[data.length - 1].SampleSize < 100) {
        $('#lowsample-popup .stat-heading.heading_text').text('Sample size is between 30 and 99 for following')
        showPopUp()
    }
    else {
        makeChartData(data);
    }
}

function showPopUp() {
    lowSamplePopup = 1;
    $('#lowsample-popup .list-of-nulls').html("");
    $('#lowsample-popup .list-of-nulls').html("<div class=\"stat-custdiv\" style=\"pointer-events:none\"><div class=\"stat-cust-dot\"></div><div class=\"stat-cust-estabmt popup1\">" + Comparisonlist[0].Name + "</div></div>")
    $("#lowsample-popup").css("display", "block");
}

function CloseLowSamplePopup() {
    lowSamplePopup = 0;
    $("#lowsample-popup").css("display", "none");
    $(".TranslucentDiv").hide();
    $("#UpdateProgress").hide();
}

function ProceedWithChart() {
    CloseLowSamplePopup();
    //console.log("chart", chartData);
    makeChartData(chartData);
}

function KillChart() {
    $("#chart-title").text("");
    d3.selectAll('svg').remove();
    d3.selectAll('.d3_tooltip').remove();
}

function plotWaterfallChart(plottingData) {
    //console.log("data", plottingData)

    $("#chart-title").text("Growth Decomposition by " + selectedMetric);

    var width = 0, height = 0;
    var margin = { top: 16, right: 0, bottom: 200, left: 120 },
    padding = 0.3;

    width = $("#chart-area").width() - margin.left - margin.right;
    height = $("#chart-area").height() - margin.top - margin.bottom;

    var labels = ["Increase", "Decrease", "Total"];
    var colors = ["green", "red", "orange"];

    var color = d3.scale.ordinal()
                .range(colors);

    //clearing previous chart and tooltips
    d3.selectAll('svg').remove();
    d3.selectAll('.d3_tooltip').remove();

    d3.select("#chart-area").append("svg").attr({
        "class": "chart svg-content-responsive"
    })

    var x = d3.scale.ordinal()
              .domain(plottingData.map(function (d) { return d.Metric}))
              .rangeBands([0, width], padding);

    var y = d3.scale.linear()
              .domain([0, d3.max(plottingData, function (d) { return d.end; })])
              .range([height, 0])              
              .nice();


    var xAxis = d3.svg.axis()
                  .scale(x)
                  .orient("bottom");

    var yAxis = d3.svg.axis()
                  .scale(y)
                  .orient("left")
                  .ticks(10)

    var yGridLine = d3.svg.axis()
                      .scale(y)
                      .tickSize(-width, 0, 0)
                      .tickFormat("")
                      .orient("left");


    var chart = d3.select(".chart")
                   .attr("width", width + margin.left + margin.right)
                   .attr("height", height + margin.top + margin.bottom)

   

    chart.append("text")
         .attr("transform", "translate(" + margin.left + ",20)")
         .attr("x", (width / 2) - margin.left/2 )
         .attr("y", 0 - (margin.top / 2))
         .attr("text-anchor", "middle")
         .style("font-size", "16px")
         .style("color", "")
         .text("Drivers of Total Trip Growth/Decline");



    var itemWidth = 100;
    var itemHeight = 18;

    var leg = chart.append("g")
                   .attr("transform", "translate(435,25)")
                   .attr("x", (width / 2) )
                   .attr("y", 0 - (margin.top / 2))



    var legend = leg.selectAll(".legend")
                    .data(labels)
                    .enter()
                    .append("g")
                    .attr("transform", function (d, i) { return "translate(" + i % 3 * itemWidth + "," + Math.floor(i / 3) * itemHeight + ")"; })
                    .attr("class", "legend");

    var rects = legend.append('rect')
                      .attr("width", 10)
                      .attr("height", 10)
                      .attr("fill", function (d, i) { return color(i); });

    var text = legend.append('text')
                     .attr("x", 15)
                     .attr("y", 10)
                     .text(function (d) { return d; });


    //console.log("data", plottingData);

    //console.log("range",x.rangeBand())
    var g = chart.append("g")
                 .attr("transform", "translate(" + margin.left + ",65)");


    var tooltip = d3.select("body").append("div").attr("class", "toolTip");

    g.append("g")
     .classed("gridLine", true)
     .attr("transform", "translate(0,0)")
     .call(yGridLine);

    g.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .attr("fill", "black")
        .call(xAxis)
        .selectAll(".tick text")
        .call(wrap, 90)
        .call(function (t) {
            var max_line = 0
            t.each(function (d, idx) {
                var self = d3.select(this);
                var s = d3.select(this).selectAll("tspan")[0];
                if (s.length > max_line) {
                    max_line = s.length;
                }
            });

            t.each(function(d,idx){ // for each one
                var self = d3.select(this);
                var s = d3.select(this).selectAll("tspan")[0]
                adjust_line = max_line - s.length 
                //self.text(''); // clear it out
                self.append("tspan") // insert two tspans
                   .attr("x", 0)
                   .attr("dy", adjust_line + 3 + "em")
                   .text(idx == 0 ? plottingData[idx].ChangePercentage : (idx == plottingData.length - 1 ? "" : (numberWithCommas(plottingData[idx].ChangePercentage, 1) + "%")));
                self.append("tspan")
                   .attr("x", 0)
                   .attr("dy", "3em")
                   .text(idx == 0 ? plottingData[idx].Share : (idx == plottingData.length - 1 ? "" : (numberWithCommas(plottingData[idx].Share, 1) + "%")));
            })
        })

    g.append("g")
        .attr("class", "y axis")
        .attr("fill", "black")
        .call(yAxis)
        
    g.append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 0 - margin.left)
        .attr("x", 0 - (height / 2) + margin.top)
        .attr("dy", "1em")
        .style("text-anchor", "middle")
        .text("Projected Annualized Trip Count")
        .style("font-size", "15px")
        .style("fill", "black");

    d3.selectAll("path")
            .attr("fill", "none")
            .attr("stroke", "rgba(155, 155, 155, 1)");

    var bar = g.selectAll(".bar")
               .data(plottingData)
               .enter().append("g")
               .attr("class", function (d) { return "bar " + d.class })
               .attr("transform", function (d) { return "translate(" + x(d.Metric) + ",0)"; })
               .on("mousemove", function (d) {
                    tooltip
                      .style("left", d3.event.pageX - 50 + "px")
                      .style("top", d3.event.pageY - 70 + "px")
                      .style("display", "inline-block")
                      .html((d.Metric) + "<br>" + (d.isTotal == 0 ? numberWithCommas(d.DisplayValue, 1) + "%" : numberWithCommas(d.Volume, 0)));
                 })
    		   .on("mouseout", function (d) { tooltip.style("display", "none"); });
               

    bar.append("rect")
       .attr("y", function (d) { return y(Math.max(d.start, d.end)); })
       .attr("height", function (d) { return Math.abs(y(d.start) - y(d.end)); })
       .attr("width", x.rangeBand());

    bar.append("text")
       .attr("class", "val-txt")
       .attr("x", x.rangeBand() / 2)
       .attr("y", function (d) { return ((d.class == 'positive' || d.class == 'start' || d.class == 'end'|| d.class == 'zero') ? y(d.end) - 15 : y(d.end) + 20) })
       .attr("dy", function (d) { return ((d.class == 'negative') ? '-' : '') + ".75em" })
       .attr("text-anchor", "middle")
       .text(function (d) {
           if (d.isTotal == 1) {
               return "";
           }
           else if (d.isTotal == 2) {
               return numberWithCommas(d.DisplayValue, 1) + "%";
           }
           else {
               return numberWithCommas(d.DisplayValue, 1) + "%";
           }
       });

    bar.append("line")
       .attr("class", function (d) { return "line " + d.class })
       .attr("x1", 0)
       .attr("y1", function (d) { return y(d.end) })
       .attr("x2", x.rangeBand())
       .attr("y2", function (d) { return y(d.end) })

    bar.filter(function (d) { return d.class != "end"}).append("line")
       .attr("class", "connector")
       .attr("x1", x.rangeBand())
       .attr("y1", function (d) { return y(d.end) })
       .attr("x2", x.rangeBand() / (1 - padding))
       .attr("y2", function (d) { return y(d.end) })


}
function type(d) {
    d.value = +d.value;
    return d;
}

function wrap(text, width) {
    text.each(function () {
        var text = d3.select(this),
            words = text.text().split(/\s+/).reverse(),
            word,
            line = [],
            lineNumber = 0,
            lineHeight = 1.0, // ems
            y = text.attr("y"),
            //dy = lineHeight * 1.2
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


$(document).ready(function () {
    $('#SliderContent').css('margin-top', '1%');
    $('.FilterPopup.TimePeriod').height(146);
    $("#ExportToPPT").click(function () {
        if (!Validate_CompareRetailers_Charts() || !Validate_Measures_Charts()) {
            return false;
        }
        var params = makeParamsForSelections();
        //console.log("params", params);
        postBackData = '{"establishmentDeepDive":' + JSON.stringify(params) + '}';
        //console.log("postback", postBackData);
        //make ajax call for PPT Download
        jQuery.ajax({
            type: "POST",
            url: $("#URLESTABLISHMENTDEEPDIVEPPT").val(),
            data: postBackData,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //console.log("success", data);
                downloadPPT(data);
            },
            error: function (xhr, status, error) {
                console.log("error", error)
            }
        })

    })
})

function downloadPPT(response) {
    //console.log("res", response);
    if (response != "error")
        window.location.href = $("#URLESTABLISHMENTDEEPDIVEPPTDOWNLOAD").val() + "/?path=" + response;
}


function numberWithCommas(x,roundDigits) {
    return x.toFixed(roundDigits).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

