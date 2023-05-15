var activetab = "divtotaltable";
var ShortNames = [];
var ExportVal = "";
var ExportSheetList = new Array();
var ExportSheetNames = new Array();


$(document).ready(function () {
    $("#ExportToExcel").click(function (e) {
        ExportVal = "EXCEL DOWNLOAD";
        ExportToExcel = "true";
        prepareContentArea();
    });
});

function prepareContentArea() {
    $("#divtotaltable").show();
    var localTime = new Date();
    var year = localTime.getFullYear();
    var month = localTime.getMonth() + 1;
    var date = localTime.getDate();
    var hours = localTime.getHours();
    var minutes = localTime.getMinutes();
    var seconds = localTime.getSeconds();

    SetDefaultCustomBase();
    if (Grouplist.length < 2) {
        showMessage("Please select minimum 2 groups.");
        return false;
    }
    else if (SelectedTotalMeasure.length == 0) {
        showMessage("Please select Measure.");
        return false;
    }
    
    var reportparams = new Object();
    reportparams.tabid = "'" + activetab + "'";//"";
    reportparams._BenchMark = "";
    reportparams.Comparison_DBNames = [];
    reportparams.Comparison_ShortNames = [];
    //reportparams.Comparison_UniqueIds = [];
    var BenchmarkUniqueId = [];
    var compUniqueId = [];
    reportparams.Comparisonlist = [];
    for (var i = 0; i < Grouplist.length; i++) {
        reportparams.Comparison_DBNames.push(Grouplist[i].DBName);
        reportparams.Comparison_ShortNames.push(Grouplist[i].Name);
        //reportparams.Comparison_UniqueIds.push(Grouplist[i].UniqueId);
        if (i == 0) {
            reportparams._BenchMark = Grouplist[i].DBName;
            BenchmarkUniqueId.push(Grouplist[i].UniqueId);
        }
        else {
            reportparams.Comparisonlist.push(Grouplist[i].DBName);
            
        }
        compUniqueId.push(Grouplist[i].UniqueId);
    }
    reportparams.Comparison_UniqueIds = compUniqueId.join("|");
    reportparams.BenchmarkUniqueId = BenchmarkUniqueId[0].toString();

    reportparams.timePeriod = TimePeriod;
    reportparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
    reportparams._shortTimePeriod = $(".timeType").val();

    if (SelectedFrequencyList.length > 0) {
        reportparams._ShopperFrequency = SelectedFrequencyList[0].Name;
        reportparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }

    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_UniqueId = [];
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
    reportparams._filter = Advanced_Filters_DBNames.join("|");
    reportparams.filterShortname = Advanced_Filters_ShortNames.join("|");
    reportparams.ShopperSegment_UniqueId = Advanced_Filters_UniqueId.join("|");
    reportparams.ShortNames = [];
    reportparams.ShortNames = reportparams.Comparison_ShortNames;
    ShortNames = reportparams.Comparison_ShortNames;
    reportparams.Selected_StatTest = Selected_StatTest;
    reportparams.Sigtype_UniqueId = Sigtype_Id;
    var TotalMeasureList = [];
    var TotalMeasureNames = [];
    for (var i = 0; i < SelectedTotalMeasure.length; i++) {
        if (i == 0)
            TotalMeasureNames.push(SelectedTotalMeasure[i].parent)

        TotalMeasureNames.push(SelectedTotalMeasure[i].Name)
        TotalMeasureList.push(SelectedTotalMeasure[i].UniqueId)
    }

    if (CustomBase.length > 0) {
        if (TabType.toLocaleLowerCase() == "trips")
            reportparams.CustomBase_DBName = CustomBase[0].TripsDBName;
        else
            reportparams.CustomBase_DBName = CustomBase[0].ShopperDBName;

        reportparams.CustomBase_ShortName = CustomBase[0].Name;
        reportparams.CustomBase_UniqueId = CustomBase[0].UniqueId;
    }

    reportparams._measure = TotalMeasureNames.join("|").trim().toString();//"Retailer Perceptions|Has low everyday prices";
    reportparams.MeasureUniqueIds = TotalMeasureList.join("|").trim().toString();
    if (sVisitsOrGuests == "1")
        reportparams.Module = "trips";
    else
        reportparams.Module = "shopper";
    reportparams.ExportToExcel = (ExportVal == "EXCEL DOWNLOAD" ? "true" : "false");
    reportparams.ExportSheetList = [];
    reportparams.ExportSheetNames = [];
    if (ExportToExcel == "true") {
        GetExportToExcelSheetList();
    }
    else {
        ExportSheetList = new Array();
        ExportSheetList[0] = "'" + activetab + "'";
    }
    reportparams.ExportSheetList = ExportSheetList.toString();
    reportparams.ExportSheetNames = ExportSheetNames.toString();
    
   // reportparams.Selected_StatTest = "";
    //string tabid, string _BenchMark, string[] Comparisonlist, string timePeriod, string _shortTimePeriod, string _ShopperFrequency, string _measure, string _filter, string filterShortname, string[] ShortNames, string ExportToExcel, string[] ExportSheetList, string[] ExportSheetNames, string Selected_StatTest
    postBackData = "{reportparams:" + JSON.stringify(reportparams) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLReports").val() + "/GetTotalData",
        async: true,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!isAuthenticated(data))
                return false;

            //BuildTable(data);
            //SetTotaldataSize();
            //isChange = "false";

            if (reportparams.ExportToExcel == "true") {
                PrepareTable(data);
                window.location.href = $("#URLReports").val() + "/" + "ExportExcel_Total?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                ExportVal = "";
            }
            else {
                PrepareTable(data);
                ExportVal = "";
            }
            $("#divtotaltable .leftheader .totalheader td").height($("#divtotaltable .rightheader .totalheader tr td").eq(0).height());        

            reportparams.ExportToExcel = "false";          
            $("#UpdateProgress").hide();
            $(".TranslucentDiv").hide();
        },
        error: function (xhr, status, error) {
            $("#UpdateProgress").hide();
            $(".TranslucentDiv").hide();
            //showMessage(error);
            //RedirectToErrorPage();
            GoToErrorPage();
        }
    });
}

function CreateTableWidthandHeight(data) {
    var tblwidth = 0;
    var tblheight = $("#rightPanel").height() - 80;
    if (ShortNames.length <= 4) {
        tblwidth = $("#rightPanel").width() - 560;
    }
    else if (ShortNames.length > 4) {
        tblwidth = (159 * ShortNames.length);
    }
    table = "<table id=\"tblheader\" class=\"FixedTables\" style=\"border-collapse: collapse;table-layout: fixed; width:920px;height:400px;overflow:auto;\">";
    table += data;
    table += "</table>";
    return table;
}
function BuildTable(data) {
    if (data.Retailer == "Login") {
        var SSOUrl = "false";
        if (SSOUrl == "true") {
            window.location.href = "../../../Views/Home.aspx?signout=true";
        }
        else {
            window.location.href = "../../../Login.aspx?signout=true";
        }
    }
    else {
        data.Retailer = CreateTableWidthandHeight(data.Retailer);
        var maintbwidth = $("#rightPanel").width() - 17;
        if (RightPanelWidth < 500) {
            maintbwidth = 600;
        }

        if (data.LeftHeader != "" && data.LeftBody != "") {
            PlotTable(data.LeftHeader, data.LeftBody, data.RightHeader, data.RightBody, maintbwidth, FixedColumnWidth, FixedTableHeight);
        }
        AddStyles();
        var height = $(".ShowResultBlock").height() - $(".RightFilterContent1").height();
        $(".tableDiv").css("height", height);
        $(".tablecontent").css("height", height);

        var mainheight = $(".tablecontent").height() - $(".fixedHead").height();
        $("#CrossTab3 .fixedTable").css("height", rightpaneltotalmaxheight);
        $("#" + activetab + ".ShoppingFrequencytitle").css("width", ($(".ShoppingFrequencyheader").width()) + "px");
        $("#" + activetab + ".benchmarktitle").css("width", ($(".benchmarkheader").width()) + "px");

        //Nagaraju 27-03-2014
        if (ExportToExcel == "true") {
            var localTime = new Date();
            var year = localTime.getFullYear();
            var month = localTime.getMonth() + 1;
            var date = localTime.getDate();
            var hours = localTime.getHours();
            var minutes = localTime.getMinutes();
            var seconds = localTime.getSeconds();
            window.location.href = "DownloadTotal.aspx?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
        }
        ExportToExcel = "false";
        //End
    }

}

//function PlotTable(leftheader, leftbody, rightheader, rightbody, _width, _fixedColumnWidth, _fixedTableheight) {
//    //var leftcolumns = opts.fixedColumns;
//    htmlstring = "<div class=\"tablecontent\" style=\"\">";
//    htmlstring += "<div class=\"fixedContainer\">";
//    htmlstring += "<div class=\"fixedHead\" style=\"width:" + _width + "px;overflow:hidden;\">";
//    htmlstring += leftheader;
//    htmlstring += "</div>";
//    htmlstring += "</div>";
//    htmlstring += "<div class=\"fixedContainer\">";
//    htmlstring += "<div class=\"fixedTable\" onscroll=\"reposVertical(this);\" style=\"width:" + (_width) + "px;height:395px;overflow:auto;\">";
//    htmlstring += leftbody;
//    htmlstring += "</div>";
//    htmlstring += "</div>";
//    htmlstring += "</div>";
//    $("#" + activetab)[0].innerHTML = htmlstring;

//    adjustscroll();
//}
function GetExportToExcelSheetList() {
    ExportSheetList = new Array();
    ExportSheetNames = new Array();
    var indx = 0;
    if (sVisitsOrGuests == 2) {
        ExportSheetList[indx] = "divshoppertotaltable";
        ExportSheetNames[indx] = "TOTAL RESPONDENTS";
        indx += 1;
    }
    else if (sVisitsOrGuests == 1) {
        ExportSheetList[indx] = "divtripstotaltable";
        ExportSheetNames[indx] = "TOTAL TRIPS";
        indx += 1;
    }
}
function AddStyles() {
    $("#" + activetab + " .fixedContainer .fixedHead table tr").eq(1).children("td").each(function () {
        $(this).css("background-color", "#808080");
        $(this).css("height", "32px");
    });
    $("#" + activetab + " .fixedContainer .fixedHead table tr").eq(1).children("td").each(function () {
        $(this).css("height", "32px");
    });
    $("#" + activetab + " .fixedContainer .fixedHead table tr").eq(2).children("td").each(function () {
        $(this).css("background-color", "#FFFFFF");
        $(this).css("color", "#000000");
    });
    $("#" + activetab + " .fixedContainer .fixedHead table tr").eq(2).children("td").each(function () {
        $(this).css("background-color", "#FFFFFF");
        $(this).css("color", "#000000");
    });
    $("#" + activetab + " .fixedContainer .fixedHead table tr").eq(0).children("td").each(function () {
        //$(this).css("border-right", "0");
    });

    $("#" + activetab + " .fixedContainer .fixedHead table tr").eq(0).children("td").each(function () {
        $(this).css("height", "24px");
        $(this).css("font-weight", "bold");
        $(this).css("font-size", "15px");
    });
    $("#" + activetab + " .fixedContainer .fixedHead table tr").eq(0).children("td").each(function () {
        $(this).css("height", "24px");
    });
}

function adjustscroll() {
    ////adjust scroll 
    $("#" + activetab + " .FixedTables .fixedColumn .fixedTable").height(rightpaneltotalmaxheight);
    var _width = $("#rightPanel").width() - 17;
    var _height = 378;
    var tablewidth = ($("#" + activetab + " .fixedContainer .fixedHead table tr").eq(0).children("td").length * 200) + 100;
    if (tablewidth < _width) {
        $("#" + activetab + " .fixedContainer .fixedHead table").width(_width)
    }
    else {
        $("#" + activetab + " .fixedContainer .fixedHead table").width(tablewidth)
    }
    if (tablewidth < _width) {
        $("#" + activetab + " .fixedContainer .fixedTable table").eq(0).width(_width)
    }
    else {
        $("#" + activetab + " .fixedContainer .fixedTable table").eq(0).width(tablewidth)
    }
    if ($("#" + activetab + " .fixedContainer .fixedTable table").eq(0).height() > _height) {
        $("#" + activetab + " .fixedContainer .fixedTable").width(_width + 17)
    }
}


function PrepareTable(data) {
    var maintbwidth = 900;
    var FixedColumnWidth = 300;
    var FixedTableHeight = 400;
    if (data.LeftHeader != "" && data.LeftBody != "" && data.htmlstring != "" && data.Retailer != "") {
        PlotTable(data.LeftHeader, data.LeftBody, data.RightHeader, data.RightBody, maintbwidth, FixedColumnWidth, FixedTableHeight);
    }
    else {
        NoDataAvailable();
    }
}
function PlotTable(leftheader, leftbody, rightheader, rightbody, _width, _fixedColumnWidth, _fixedTableheight) {
    //left table header
    var table = "<div class=\"leftheader\" style=\"margin-right:26px;\">";
    table += leftheader;
    table += "</div>";
    //end left table header

    //right table header
    table += "<div class=\"rightheader\" style=\"overflow:hidden;width: 67.4%;\">";
    table += rightheader;
    table += "</div>"; 
    //end right table header

    //left table body
    table += "<div id=\"total-left-body\" class=\"leftbody\" style=\"overflow:hidden;margin-right: 25px;height:88.5%;\">";
    table += leftbody;
    table += "</div>";
    //  

    //right table body  
    table += "<div id=\"total-right-body\" onscroll=\"reposVertical(this);\" class=\"rightbody\" style=\"overflow:auto;width:67.4%;height:88.5%;\">";
    table += rightbody;
    table += "</div>";
    //end left table body   

    $("#divtotaltable").html("");
    $("#divtotaltable").html(table);
    var header_height = $("#divtotaltable .rightheader .totalheader tr td").eq(0).height();
    $("#divtotaltable #total-left-body").css("height", "calc(100% - " + (header_height + 43) + "px)");
    $("#divtotaltable #total-right-body").css("height", "calc(100% - " + (header_height + 43) + "px)");
    SetScroll($("#divtotaltable .rightbody"), "#393939", 0, -8, 0, -8, 8);

        //_.each($(".leftbody .crossRetailertotalheader"), function (i, j) {
        //    _.each($(".rightbody .crossRetailertotalheader td"), function (k, l) {
        //        var id = $(i).attr("id");
        //        var minheight = $(i).css("height")
        //        $(k).css("height",minheight);
        //    });
        //});
    //SetStyles();
    SetTableRowHeight(); 
    $('.crossRetailertotalheader').click(function () {
        var id = $(this).attr("id");
        if ($(".leftbody table tr[id='" + id + "']").children("td").eq(0).find(".treeview").hasClass("minusIcon")) {
           $(".leftbody table tr[id='" + id + "']").children("td").eq(0).find(".treeview").removeClass("minusIcon");
           $(".leftbody table tr[id='" + id + "']").children("td").eq(0).find(".treeview").removeClass("plusIcon");
           $(".leftbody table tr[id='" + id + "']").children("td").eq(0).find(".treeview").addClass("plusIcon");
        }
        else {
           $(".leftbody table tr[id='" + id + "']").children("td").eq(0).find(".treeview").removeClass("minusIcon");
           $(".leftbody table tr[id='" + id + "']").children("td").eq(0).find(".treeview").removeClass("plusIcon");
           $(".leftbody table tr[id='" + id + "']").children("td").eq(0).find(".treeview").addClass("minusIcon");
        }
        if ($(this).nextUntil('tr.crossRetailertotalheader').length > 0) {
            $(".leftbody .crossRetailertotalheader").eq(id).nextUntil('tr.crossRetailertotalheader').slideToggle(0, function () { SetScroll($("#divtotaltable .rightbody"), "#393939", 0, -8, 0, -8, 8); });

            $(".rightbody .crossRetailertotalheader").eq(id).nextUntil('tr.crossRetailertotalheader').slideToggle(0, function () { SetScroll($("#divtotaltable .rightbody"), "#393939", 0, -8, 0, -8, 8); });
        }
        else {
            $(".leftbody .crossRetailertotalheader").eq(id).parent().nextUntil('tr.crossRetailertotalheader').slideToggle(0, function () { SetScroll($("#divtotaltable .rightbody"), "#393939", 0, -8, 0, -8, 8); });
            $(".rightbody .crossRetailertotalheader").eq(id).parent().nextUntil('tr.crossRetailertotalheader').slideToggle(0, function () { SetScroll($("#divtotaltable .rightbody"), "#393939", 0, -8, 0, -8, 8); });
        }
        SetScroll($("#divtotaltable .rightbody"), "#393939", 0, -8, 0, -8, 8);
    });  
}
function SetTableRowHeight()
{
    $(".leftbody table tr").each(function (i) {
        var height = $(this).height();
        //$(this).css("height", height + "px");
        $(".leftbody table tr").eq(i).children("td").height(height);
        //$(".rightbody table tr").eq(i).height(height);
        $(".rightbody table tr").eq(i).children("td").height(height);
    });

    $(".leftbody .crossRetailertotalheader").each(function (i) {
        var height = $(this).height();
        $(this).css("height", height + "px");      
        $(".rightbody .crossRetailertotalheader").eq(i).css("height", height + "px");
        $(".rightbody .crossRetailertotalheader").eq(i).children("td").css("height", height + "px");
    });
}
function SetStyles() {
    $(".table-title").prev(".rowitem").children("ul").children("li").css("border", "0");
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