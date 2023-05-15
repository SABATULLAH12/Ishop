
$(document).ready(function () {
    $("#ExportToExcel").click(function (e) {
        ExportVal = "EXCEL DOWNLOAD";
        prepareContentArea(ExportVal);
    });
});
/// <reference path="../Layout/Layout.js" />
function prepareContentArea(excelStatus) {
    if (!Validate_CompareRetailers_Charts()) {
        return false;
    }
    var localTime = new Date();
    var year = localTime.getFullYear();
    var month = localTime.getMonth() + 1;
    var date = localTime.getDate();
    var hours = localTime.getHours();
    var minutes = localTime.getMinutes();
    var seconds = localTime.getSeconds();

    var param = new Object();
    param.timePeriod = TimePeriod;
    param.TimePeriod_UniqueId = TimePeriod_UniqueId;
    param.shortTimePeriod = $(".timeType").val();
    param.isChange = "true";
    param.width = "100%";
    param.height = 473;
    Comparison_DBNames = [];
    Comparison_ShortNames = [];
    var sCompList = [];
    var Comparison_UniqueIds = [];
    var Benchmark_UniqueIds = [];
    for (var i = 0; i < Comparisonlist.length; i++) {


        if (TabType.toLocaleLowerCase() == "trips")
            Comparison_DBNames.push(Comparisonlist[i].Name);
        else
            Comparison_DBNames.push(Comparisonlist[i].Name);
        
        Comparison_ShortNames.push(Comparisonlist[i].Name);
        if (i == 0) {
            Benchmark_UniqueIds.push(Comparisonlist[i].UniqueId);
            Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);
        }
      if (i != 0) {
          if (TabType.toLocaleLowerCase() == "trips")
              sCompList.push(Comparisonlist[i].Name);
          else
              sCompList.push(Comparisonlist[i].Name);

          Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);
      }
    }
    
    param.BenchMark = Comparison_DBNames[0];
    param.Compare = sCompList.join(",");
    param.Benchmark_UniqueIds = Benchmark_UniqueIds[0].toString();
    param.Comparison_UniqueIds = Comparison_UniqueIds.join("|");
    param.Comparison_ShortNames = Comparison_ShortNames.join("|");

    if (CustomBase.length > 0) {
        param.CustomBase_ShortName = CustomBase[0].Name;
        param.CustomBase_UniqueId = CustomBase[0].UniqueId;
    }
    
    //for (var i = 0; i < Comparisonlist.length; i++) {
    //    Comparison_DBNames.push(Comparisonlist[i].DBName);
    //    Comparison_ShortNames.push(Comparisonlist[i].Name);
    //}
    //param.Retailer = Comparison_DBNames.toString();

    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    var Advanced_Filters_UniqueId = [];
    //Guest advanced filters
    for (var i = 0; i < SelectedDempgraphicList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].parentName + "|" + SelectedDempgraphicList[i].Name);
        Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
    }
    //Visits advanced filters
    for (var i = 0; i < SelectedAdvFilterList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedAdvFilterList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedAdvFilterList[i].parentName + "|" + SelectedAdvFilterList[i].Name);
        Advanced_Filters_UniqueId.push(SelectedAdvFilterList[i].UniqueId);
    }

    param.filter = Advanced_Filters_ShortNames.join("|").toString();
    param.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");

    if (SelectedFrequencyList.length > 0) {
        param.ShopperFrequency = SelectedFrequencyList[0].Name.toString();
        param.ShopperFrequencyShort = SelectedFrequencyList[0].Name.toString();
        param.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }
    //if (Selected_StatTest.toLowerCase() != "custom base")
        param.Selected_StatTest = Selected_StatTest;
    //else
    //    param.Selected_StatTest = "benchmark"
    param.Sigtype_UniqueId = Sigtype_Id;
    var postBackData = "{param:" + JSON.stringify(param) + "}"
    var sResult = [];
    jQuery.ajax({
        type: "POST",
        url: $("#URLCrossRetailerImageries").val(),
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!isAuthenticated(data))
                return false;

            if (excelStatus == "EXCEL DOWNLOAD") {
                window.location.href = $("#URLAnalysis").val() + "/" + "ExportExcel_Imageries?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                ExportVal = "";
            }
            LoadCRPData(data);
           
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });


}

function LoadCRPData(data) {
        //left table header
    var table = "<div class=\"header-content\" style=\"\"><div class=\"leftheader\" style=\"\">";
        table += data.LeftHeader;
        table += "</div>";
        //end left table header

        //right table header
        table += "<div class=\"rightheader\" style=\"overflow:hidden;\">";
        table += data.RightHeader;
        table += "</div></div>";
        //end right table header

        //left table body
        table += "<div class=\"body-content\" style=\"\"><div class=\"leftbody\" style=\"overflow:hidden;\">";
        table += data.LeftBody;
        table += "</div>";
        //  

        //right table body  
        table += "<div onscroll=\"reposVertical(this);\" class=\"rightbody\" style=\"overflow:auto;\">";
        table += data.RightBody;
        table += "</div></div>";
        //end left table body   

        $("#GetCRPData").html("");
        $("#GetCRPData").html(table);
        $(".leftheader .rowitem").eq(1).length > 0 ? $(".leftheader .rowitem").eq(1).remove() : "";
        $(".leftheader .benchmarkheader").eq(0).length > 0 ? $(".leftheader .benchmarkheader").eq(0).hide() : ""
        SetStyles();
        var header_height = $("#GetCRPData .header-content").height();
        $("#GetCRPData .body-content").css("height", "calc(100% - " + (header_height + 10) + "px)");
        SetScroll($("#GetCRPData .rightbody"), "#393939", 0, -8, 0, -8, 8);
}

function reposVertical(e) {
  
    //$(".leftheader").scrollTop(e.scrollTop);
    //$(".rightheader").scrollTop(e.scrollTop);
    $(".leftbody").scrollTop(e.scrollTop);
    $(".rightbody").scrollTop(e.scrollTop);

    $(".rightheader").scrollLeft(e.scrollLeft);
    $(".rightbody").scrollLeft(e.scrollLeft);
}

function SetStyles() {
    $(".table-title").prev(".rowitem").children("ul").children("li").css("border", "0");
    //var rightHeaderHeight = $(".rightheader").height()
    //$(".leftheader").css("height", rightHeaderHeight);
    //var rowHeight = $(".rightheader .rowitem").height();
    //$(".rightheader .rowitem").eq(0).find("li").css("height", rowHeight);
    //$(".leftheader .rowitem").eq(0).css("height", rowHeight);
    //$(".leftheader .rowitem").eq(0).find("li").css("height", rowHeight);
    //var totalHeight = $("#GetCRPData").height();
    //$(".leftbody").css("height", totalHeight - rightHeaderHeight - 5);
    //$(".rightbody").css("height", totalHeight - rightHeaderHeight - 5);

    $(".leftheader .rowitem").eq(0).find("li").css("height", "auto");

    var rightHeaderHeight = $(".rightheader").height();
    var totalHeight = $("#GetCRPData").height();
    $(".leftbody").css("height", "100%");
    $(".rightbody").css("height", "100%");

    if (rightHeaderHeight != $(".leftheader").height())
        $(".leftheader").css("height", rightHeaderHeight - 0.5);
    var rowHeight = $(".rightheader .rowitem").height();
    //if (rowHeight != $(".rightheader .rowitem").eq(0).find("li").height())
    //    //$(".rightheader .rowitem").eq(0).find("li").css("height", rowHeight - 0.5);
    //    if (rowHeight != $(".leftheader .rowitem").eq(0).height())
    //        //$(".leftheader .rowitem").eq(0).css("height", rowHeight - 0.5);
    //        if (rowHeight != $(".leftheader .rowitem").eq(0).find("li").height())
    //$(".leftheader .rowitem").eq(0).find("li").css("height", rowHeight - 0.5);

    //added by Nagaraju for table header Date: 22-05-2017
    if ($(".leftheader .rowitem").eq(0).height() > $(".rightheader .rowitem").eq(0).height())
        $(".rightheader .rowitem").eq(0).height($(".leftheader .rowitem").eq(0).height());
    else
        $(".leftheader .rowitem").eq(0).height($(".rightheader .rowitem").eq(0).height());

    if ($(".leftheader").height() > $(".rightheader").height())
        $(".rightheader").height($(".leftheader").height());
    else
        $(".leftheader").height($(".rightheader").height());


    $(".leftbody ul").each(function (i) {
        if ($(this).find("span").eq(0).text().toLocaleLowerCase() == "samplesize")
            $(this).height($(".rightbody ul").eq(i).height());
        else
            $(".rightbody ul").eq(i).height($(this).height());
    });
}