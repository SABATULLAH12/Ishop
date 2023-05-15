/// <reference path="../Layout/Layout.js" />
$(document).ready(function (e) {
    $("#ExportToExcel").click(function (e) {
        ExportVal = "EXCEL DOWNLOAD";
        prepareContentArea(ExportVal);
    });
});
function prepareContentArea(excelStatus) {
    $("#GetCRCSData").css("height","94%");
    var localTime = new Date();
    var year = localTime.getFullYear();
    var month = localTime.getMonth() + 1;
    var date = localTime.getDate();
    var hours = localTime.getHours();
    var minutes = localTime.getMinutes();
    var seconds = localTime.getSeconds();

    if (!Validate_CompareRetailers_Charts()) {
        return false;
    }
    var param = new Object();
    param.timePeriod = TimePeriod;
    param.TimePeriod_UniqueId = TimePeriod_UniqueId;
    param.shortTimePeriod = $(".timeType").val();
    param.isChange = "true";
    param.width = "99.5%";
    param.height = 473;

    var Comparison_UniqueIds = [];
    Comparison_DBNames = [];
    Comparison_ShortNames = [];
    for (var i = 0; i < Comparisonlist.length; i++) {

        if (TabType.toLocaleLowerCase() == "trips")
            Comparison_DBNames.push(Comparisonlist[i].LevelDesc + "|" + Comparisonlist[i].Name);           
        else
            Comparison_DBNames.push(Comparisonlist[i].LevelDesc + "|" + Comparisonlist[i].Name);
          
        Comparison_ShortNames.push(Comparisonlist[i].Name);
        Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);
    }
    param.Retailer = Comparison_DBNames.toString();
    param.Comparison_UniqueIds = Comparison_UniqueIds[0].toString();
    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    var Advanced_Filters_UniqueId = [];
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
     
    param.filter = Advanced_Filters_ShortNames.join('|').toString();
    param.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");

    var postBackData = "{param:" + JSON.stringify(param) + "}"
    var sResult = [];
    jQuery.ajax({
        type: "POST",
        url: $("#URLComparisonRetailerFrequencies").val(),       
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!isAuthenticated(data))
                return false;

            if (excelStatus == "EXCEL DOWNLOAD") {
                window.location.href = $("#URLAnalysis").val() + "/" + "ExportExcel_CossRetailerImageries?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                ExportVal = "";
            }

            LoadCRCSData(data);
            _.each($(".dataCRCS tr td"), function (i, j) { $(i).css("min-height", $(i).parent().innerHeight()) });
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
    
    
}

function LoadCRCSData(data) {
    $("#GetCRCSData").html("");
    $("#GetCRCSData").html(data);
    $(".dataCRCS").css("max-height", $(".ShowResultBlock").height() - $(".topHeaderCRCS").height());
    SetScroll($(".dataCRCS"), "#393939", 0, 0, 0, 0, 8);
    $('.crossRetailerheader').click(function () {
        if ($(this).children("td").eq(0).find(".treeview").hasClass("minusIcon")) {
            $(this).children("td").eq(0).find(".treeview").removeClass("minusIcon");
            $(this).children("td").eq(0).find(".treeview").removeClass("plusIcon");
            $(this).children("td").eq(0).find(".treeview").addClass("plusIcon");
        }
        else {
            $(this).children("td").eq(0).find(".treeview").removeClass("minusIcon");
            $(this).children("td").eq(0).find(".treeview").removeClass("plusIcon");
            $(this).children("td").eq(0).find(".treeview").addClass("minusIcon");
        }

        if ($(this).nextUntil('tr.crossRetailerheader').length > 0)
            $(this).nextUntil('tr.crossRetailerheader').slideToggle(100, function () { SetScroll($(".dataCRCS"), "#393939", 0, -8, 0, -8, 8); });
        else
            $(this).parent().nextUntil('tr.crossRetailerheader').slideToggle(100, function () { SetScroll($(".dataCRCS"), "#393939", 0, -8, 0, -8, 8); });
        SetScroll($(".dataCRCS"), "#393939", 0, 0, 0, 0, 8);
    });
    
}




