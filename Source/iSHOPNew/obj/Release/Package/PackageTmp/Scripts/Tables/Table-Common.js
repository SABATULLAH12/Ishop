/// <reference path="../Layout/Layout.js" />
var benchmark = "";
var benchmarkshortName = "";
var comparison = "";
var ShopperFrequency = "";
var table = "";
var comparisonlist = new Array();
var ShortNames = new Array();
var comparisonshortnames = new Array();
var compindex = 0;
var activetab = "";
var tablewidth = 1000;
var tableheight = 500;
var fixedtablewidth = 159;
var fixedcolmnwidth = 159;
var firstcolwidth = 159;
var SelectedRetailer = "";
var SelectedTimePeriod = "";
var tablist = new Array();
var ChannelOrRetailer = "";
//Nagaraju 27-03-2014
var ExportToExcel = false;
var ExportSheetList = new Array();
var ExportSheetNames = new Array();
var spnames = [];
var RightPanelWidth = 1000;
var FixedTableHeight = 320;
var FixedColumnWidth = 500;
var TimePeriodShortName = "";


$(document).ready(function () {
    $(document).on("click", ".adv-fltr-label", function () {
        //if ($(".adv-fltr-showhide-txt").text().toLowerCase().trim() == "show less")
        //    $(".adv-fltr-showhide-txt").trigger("click");

        $(".adv-fltr-label").removeClass("adv-fltr-label-demo");
        $(".adv-fltr-label").removeClass("adv-fltr-label-visits");
        $(".adv-fltr-label").removeClass("adv-fltr-label-guests");
        $(".filter-separator").css("background-position", "1px 0px");
        $(".advanced-seperator").css("background-position", "1px 0px");
        
        if (TabType != "") {
            if ($(this).parent().parent().hasClass("demo")) {
                if (TabType == "trips" && $("#guest-visit-toggle").is(":checked") == true) {
                    SelectedAdvFilterList = [];
                    if (currentpage.indexOf("beverage") > -1) {
                        selectedChannels = [];
                        SelectedFrequencyList = [];                        
                        $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
                    }
                    else {
                        selectedChannels = [];
                        SelectedFrequencyList = [];                        
                        $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
                    }
                }
                else if ($("#guest-visit-toggle").is(":checked") == false && TabType == "shopper") {
                    if (currentpage.indexOf("beverage") > -1) {
                        selectedChannels = [];
                        SelectedFrequencyList = [];                        
                        $("#RightPanelPartial .rgt-cntrl-chnl ul li[name='TOTAL']").trigger("click");
                    }
                    else {
                        selectedChannels = [];
                        SelectedFrequencyList = [];                        
                        $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
                    }
                }
            }
            else if (TabType == "shopper" && (($(this).parent().hasClass("visits")) || ($(this).parent().parent().hasClass("visits")))) {
                if (currentpage.indexOf("beverage") > -1) {
                    SelectedFrequencyList = [];
                    selectedChannels = [];                    
                    $("#RightPanelPartial .rgt-cntrl-chnl ul li[name='TOTAL']").trigger("click");
                }
                else {
                    selectedChannels = [];
                    SelectedFrequencyList = [];                    
                    $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
                }
            }
            else if (TabType == "trips" && $(this).parent().parent().hasClass("guests")) {
                SelectedAdvFilterList = [];
                if (currentpage.indexOf("beverage") > -1) {
                    selectedChannels = [];
                    SelectedFrequencyList = [];                    
                    $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
                }
                else {
                    selectedChannels = [];
                    SelectedFrequencyList = [];                    
                    $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
                }
            }
        }


        if ($(this).parent().parent().hasClass("demo")) {
            $(this).addClass("adv-fltr-label-demo");
        }
        if (($(this).parent().hasClass("visits")) || ($(this).parent().parent().hasClass("visits"))) {
            $(this).addClass("adv-fltr-label-visits");
            TabType = "trips";
            $(".adv-fltr-toggle-container").hide();
            $(".toggle-seperator").hide();
        }
        if ($(this).parent().parent().hasClass("guests")) {
            $(this).addClass("adv-fltr-label-guests");
            TabType = "shopper";
            $(".adv-fltr-toggle-container").hide();
            $(".toggle-seperator").hide();
            
        }
     
        $("#adv-bevselectiontype-freq").hide();
        $(".advancedfilter-seperator").hide();
        if ($(this).attr("data-val").trim().toLowerCase() == "demographic profiling") {
            $(".adv-fltr-toggle-container").show();
            $(".toggle-seperator").show();
            if ($("#guest-visit-toggle").is(":checked")) {
                main_spname = $(this).children("span").attr("guest-spname");
                checksamplesize_spname = $(".demo .adv-fltr-label").children("span").attr("guest-samplesize-spname");
                TabType = "shopper";               
                Tab_Id_mapping = $(this).children("span").attr("guest-id-mapping");
            }
            else {
                main_spname = $(this).children("span").attr("visit-spname");
                checksamplesize_spname = $(".demo .adv-fltr-label").children("span").attr("visit-samplesize-spname");
                TabType = "trips";
                Tab_Id_mapping = $(this).children("span").attr("visit-id-mapping");
            }
        }
        else {
            main_spname = $(this).children("span").attr("spname");
            checksamplesize_spname = $(this).children("span").attr("samplesize-spname");
            Tab_Id_mapping = $(this).children("span").attr("id-mapping");               
        }
        if ($("#hdn-page").length > 0) {
            currentpage = $("#hdn-page").attr("name").toLowerCase();
        }       

        Tab_Index_Id = $(this).children("span").attr("tabindex");
        tabname = $(this).children("span").html();
        HideOrShowFilters();
        $("#adv-bevselectiontype-freq").hide();
        if ($(this).attr("data-val").toLocaleLowerCase() == "establishment imagery")
        $(".freq-seperator").hide();
        $(".advancedfilter-seperator").hide();
        if ($(this).attr("data-val").toLocaleLowerCase() == "beverage details") {
            $("#adv-bevselectiontype-freq").show();
            $(".advancedfilter-seperator").show();
            if (sBevarageSelctionType.length == 0) {
                $("#RightPanelPartial .beverageItems ul li[name='Monthly Purchased']").trigger("click");
            }
        }
        else
            sBevarageSelctionType = [];

        ShowSelectedFilters();
        //if (Comparisonlist.length > 0 || ComparisonBevlist.length > 0 || Grouplist.length > 0)
        //$(".adv-fltr-showhide").trigger("click");
        prepareContentArea();

        if (!($(".adv-fltr-toggle-container").is(":visible")))
            $(".toggle-seperator").hide();
        else
            $(".toggle-seperator").show();
        if ($("#adv-fltr-Chnl").is(":visible") || $("#adv-bevselectiontype-freq").is(":visible"))
            $(".advancedfilter-seperator").show();
        else
            $(".advancedfilter-seperator").hide();
        if ($(".adv-fltr-sub-selection > div:visible").length > 1) {
            if ($(".adv-fltr-freq-container").is(":visible")) {
                $(".freq-seperator").css("display", "block");
            }
            else {
                $(".freq-seperator").css("display", "none");
            }
        }
        if ($(".adv-fltr-sub-selection > div:visible").length >= 2 && $(".adv-fltr-freq-container").is(":visible") && $("#adv-bevselectiontype-freq").is(":visible")) {
            $(".filter-separator").css("background-position", "2px 0px");
            $(".advancedfilter-seperator").hide();
        }
    });
    if ($("#hdn-page").length > 0 && $("#hdn-page").attr("name").toLocaleLowerCase() == "hdn-tbl-retailerdeepdive") {
        //$("#pit-tabs-block .demo .adv-fltr-label").trigger("click");
        ActivateTableDefaultTab($("#pit-tabs-block .demo .adv-fltr-label"));
    }
    else {
        //$(".demo .adv-fltr-label").trigger("click");
        ActivateTableDefaultTab($(".demo .adv-fltr-label"));
    }
    $("#ExportToExcel").click(function () {
        if ($("#Table-Content .leftbody").length == 0) {
            showMessage("No data available for export");
            return false;
        }
        Set_zIndex();
        $("#Translucent").show();
        ShowExportToExcelList();       
    });
});
function ActivateTableDefaultTab(obj) {
    $(".adv-fltr-label").removeClass("adv-fltr-label-demo");
    $(".adv-fltr-label").removeClass("adv-fltr-label-visits");
    $(".adv-fltr-label").removeClass("adv-fltr-label-guests");

    if (TabType != "") {
        if ($(obj).parent().parent().hasClass("demo")) {
            if (TabType == "trips" && $("#guest-visit-toggle").is(":checked") == true) {
                SelectedAdvFilterList = [];
                if (currentpage.indexOf("beverage") > -1) {
                    selectedChannels = [];
                    SelectedFrequencyList = [];
                    
                    $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
                }
                else {
                    selectedChannels = [];
                    SelectedFrequencyList = [];
                    
                    $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
                }
            }
            else if ($("#guest-visit-toggle").is(":checked") == false && TabType == "shopper") {
                if (currentpage.indexOf("beverage") > -1) {
                    selectedChannels = [];
                    SelectedFrequencyList = [];
                    
                    $("#RightPanelPartial .rgt-cntrl-chnl ul li[name='TOTAL']").trigger("click");
                }
                else {
                    selectedChannels = [];
                    SelectedFrequencyList = [];
                    
                    $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
                }
            }
        }
        else if (TabType == "shopper" && (($(obj).parent().hasClass("visits")) || ($(obj).parent().parent().hasClass("visits")))) {
            if (currentpage.indexOf("beverage") > -1) {
                SelectedFrequencyList = [];
                selectedChannels = [];
                
                $("#RightPanelPartial .rgt-cntrl-chnl ul li[name='TOTAL']").trigger("click");
            }
            else {
                selectedChannels = [];
                SelectedFrequencyList = [];
                
                $("#RightPanelPartial #frequency_containerId ul li[name='TOTAL VISITS']").trigger("click");
            }
        }
        else if (TabType == "trips" && $(obj).parent().parent().hasClass("guests")) {
            SelectedAdvFilterList = [];
            if (currentpage.indexOf("beverage") > -1) {
                selectedChannels = [];
                SelectedFrequencyList = [];
                
                $("#RightPanelPartial #frequency_containerId ul li[name='ALL MONTHLY +']").trigger("click");
            }
            else {
                selectedChannels = [];
                SelectedFrequencyList = [];
                
                $("#RightPanelPartial #frequency_containerId ul li[name='MONTHLY +']").trigger("click");
            }
        }
    }


    if ($(obj).parent().parent().hasClass("demo")) {
        $(obj).addClass("adv-fltr-label-demo");
    }
    if (($(obj).parent().hasClass("visits")) || ($(obj).parent().parent().hasClass("visits"))) {
        $(obj).addClass("adv-fltr-label-visits");
        TabType = "trips";
        $(".adv-fltr-toggle-container").hide();
        $(".toggle-seperator").hide();
    }
    if ($(obj).parent().parent().hasClass("guests")) {
        $(obj).addClass("adv-fltr-label-guests");
        TabType = "shopper";
        $(".adv-fltr-toggle-container").hide(); 
        $(".toggle-seperator").hide();
    }

    $("#adv-bevselectiontype-freq").hide();
    $(".advancedfilter-seperator").hide();
    if ($(obj).attr("data-val").trim().toLowerCase() == "demographic profiling") {
        $(".adv-fltr-toggle-container").show();
        $(".toggle-seperator").show(); 
        if ($("#guest-visit-toggle").is(":checked")) {
            main_spname = $(obj).children("span").attr("guest-spname");
            checksamplesize_spname = $(".demo .adv-fltr-label").children("span").attr("guest-samplesize-spname");
            TabType = "shopper";
            Tab_Id_mapping = $(obj).children("span").attr("guest-id-mapping");
        }
        else {
            main_spname = $(obj).children("span").attr("visit-spname");
            checksamplesize_spname = $(".demo .adv-fltr-label").children("span").attr("visit-samplesize-spname");
            TabType = "trips";
            Tab_Id_mapping = $(obj).children("span").attr("visit-id-mapping");
        }
    }
    else {
        main_spname = $(obj).children("span").attr("spname");
        checksamplesize_spname = $(obj).children("span").attr("samplesize-spname");
        Tab_Id_mapping = $(obj).children("span").attr("id-mapping");
    }
    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }

    Tab_Index_Id = $(obj).children("span").attr("tabindex");
    tabname = $(obj).children("span").html();
    HideOrShowFilters();
    $("#adv-bevselectiontype-freq").hide();
    $(".advancedfilter-seperator").hide();
    if ($(obj).attr("data-val").toLocaleLowerCase() == "beverage details") {
        $("#adv-bevselectiontype-freq").show();
        $(".advancedfilter-seperator").show();
        if (sBevarageSelctionType.length == 0) {
            $("#RightPanelPartial .beverageItems ul li[name='Monthly Purchased']").trigger("click");
        }
    }
    else
        sBevarageSelctionType = [];

    ShowSelectedFilters();
}
function NoDataAvailable() {
    var table = "<div style=\"width:99%;height:55%;text-align:center;border:1px solid grey;margin:auto;background-color: #fafafa;\"><span class=\"no-data\">No data available</span></div>";
    $("#Table-Content").html("");
    $("#Table-Content").html(table);
}
//Export To Excel-------Start----------->
function CleanClass(_sheetname) {
    _sheetname = _sheetname.replace(/[,./@#$%;&*~()+?]/g, "");
    return _sheetname;
}
function ShowExportToExcelList()
{
    var _tabtype = "";
    html = "<ul>"      
    $("#excel-title").html("Export To Excel");
    var demo_contentid = ".demo .adv-fltr-label";
    var guests_contentid = ".guests .adv-fltr-label";
    var visits_contentid = ".visits .adv-fltr-label";

    if ($("#hdn-page").attr("name").toLocaleLowerCase() == "hdn-tbl-retailerdeepdive") {
        if ($("#pit-toggle").is(":checked"))
        {
            demo_contentid = "#trend-tabs-block .demo .adv-fltr-label";
            guests_contentid = "#trend-tabs-block .guests .adv-fltr-label";
            visits_contentid = "#trend-tabs-block .visits .adv-fltr-label";           
        }
        else
        {
            demo_contentid = "#pit-tabs-block .demo .adv-fltr-label";
            guests_contentid = "#pit-tabs-block .guests .adv-fltr-label";
            visits_contentid = "#pit-tabs-block .visits .adv-fltr-label";
        }
    }
  
    $(demo_contentid + ", " + guests_contentid + ", " + visits_contentid).each(function () {
            if ($(this).is(':visible')) {
                if (($(this).parent().hasClass("visits")) || ($(this).parent().parent().hasClass("visits"))) {
                    _tabtype = "trips";
                }
                if ($(this).parent().parent().hasClass("guests")) {
                    _tabtype = "shopper";
                }

                if ($(this).children("span").attr("guest-spname") != "" && $(this).children("span").attr("guest-spname") != undefined) {
                    html += "<li><input class=\"exporttoexcel\" type=\"checkbox\" tabtype=\"trips\" tabindex=\"1\" id-mapping=\"" + $(this).children("span").attr("visit-id-mapping") + "\" spname=\"" + $(this).children("span").attr("visit-spname") + "\" samplesize-spname=\"" + $(this).children("span").attr("visit-samplesize-spname") + "\" onclick=\"CheckSelectOrUnSelectSheets();\"><span  spname=\"" + $(this).children("span").attr("visit-spname") + "\" samplesize-spname=\"" + $(this).children("span").attr("visit-samplesize-spname") + "\">TRIPS - " + $(this).children("span").html().toUpperCase() + "</span></li>";
                    html += "<li><input class=\"exporttoexcel\" type=\"checkbox\" tabtype=\"shopper\" tabindex=\"1\"  id-mapping=\"" + $(this).children("span").attr("guest-id-mapping") + "\" spname=\"" + $(this).children("span").attr("guest-spname") + "\" samplesize-spname=\"" + $(this).children("span").attr("guest-samplesize-spname") + "\" onclick=\"CheckSelectOrUnSelectSheets();\"><span  spname=\"" + $(this).children("span").attr("guest-spname") + "\" samplesize-spname=\"" + $(this).children("span").attr("guest-samplesize-spname") + "\">SHOPPERS - " + $(this).children("span").html().toUpperCase() + "</span></li>";
                }
                else {
                    html += "<li><input class=\"exporttoexcel\" type=\"checkbox\" tabtype=\"" + _tabtype + "\" tabindex=\"" + $(this).children("span").attr("tabindex") + "\" id-mapping=\"" + $(this).children("span").attr("id-mapping") + "\" spname=\"" + $(this).children("span").attr("spname") + "\" samplesize-spname=\"" + $(this).children("span").attr("samplesize-spname") + "\" onclick=\"CheckSelectOrUnSelectSheets();\"><span  spname=\"" + $(this).children("span").attr("spname") + "\" samplesize-spname=\"" + $(this).children("span").attr("samplesize-spname") + "\">" + $(this).children("span").html().toUpperCase() + "</span></li>";

                }
            }
        });
   
    $(".ExcelSelection").html("");
    $(".ExcelSelection").html(html);
    $(".exporttoexcelpopup").show();
    if ($("#chk_SelectAll").is(":checked") == true)
        $("#chk_SelectAll").trigger("click");
}

function GenerateExportToExcel() {
    Remove_zIndex();
    var selecteditems = false;
    if ($(".exporttoexcelpopup .ExcelSelection input[type*='checkbox']:checked").length == 0)
   {
       showMessage("Please select atleast one item");
       return;
    }
    //if ($(".exporttoexcelpopup .ExcelSelection input[type*='checkbox']:checked").length == $(".exporttoexcelpopup .ExcelSelection input[type*='checkbox']").length) {
    //    var r = confirm("All are Selected.This Option will take time");
    //    if (r == true) {
    //        //txt = "You pressed OK!";
    //    } else {
    //        return;
    //    }
    //}
   
    $(".exporttoexcelpopup").hide();
    $(".TranslucentDiv").hide();
    ExportToExcel = true;
    GetExportToExcelSheetList();   
}
function GetExportToExcelSheetList() {
    ExportSheetList = [];  
    $(".exporttoexcel").each(function () {
        if ($(this).is(':checked')) {           
            ExportSheetList.push({ tabname: CleanClass($(this).parent("li").children("span").html().substr(0, 20)), Tab_Index_Id: $(this).attr("tabindex"), Tab_Id_mapping: $(this).attr("id-mapping"), TabType: $(this).attr("tabtype"), main_spname: $(this).parent().find("span").attr("spname"), checksamplesize_spname: $(this).parent().find("span").attr("samplesize-spname") });
        }
    });
    prepareContentArea();
}
//Nagaraju 27-03-2014
function DownloadExcel() {
    //var localTime = new Date();
    //var year = localTime.getFullYear();
    //var month = localTime.getMonth() + 1;
    //var date = localTime.getDate();
    //var hours = localTime.getHours();
    //var minutes = localTime.getMinutes();
    //var seconds = localTime.getSeconds();
    //window.location.href = $("#URLTables").val() + "/DownloadExcel?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;

    postBackData = "{}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLTables").val() + "/DownloadExcel",
        async: true,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var localTime = new Date();
            var year = localTime.getFullYear();
            var month = localTime.getMonth() + 1;
            var date = localTime.getDate();
            var hours = localTime.getHours();
            var minutes = localTime.getMinutes();
            var seconds = localTime.getSeconds();
            window.location.href = $("#URLTables").val() + "/DownloadExcelFile?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
        },
        error: function (xhr, status, error) {
            GoToErrorPage();
        }
    });
}
function CloseExportToExcel() {
    Remove_zIndex();
    $(".exporttoexcelpopup").hide();
    $(".TranslucentDiv").hide();
}
function SelectOrUnSelectAllSheets(obj) {
    if ($(obj).is(':checked')) {
        $(".exporttoexcel").each(function () {
            $(this).prop('checked', true);
            $(".InfoForTimeSpent").show();
        });
    }
    else {
        $(".exporttoexcel").each(function () {
            $(this).prop('checked', false);
            $(".InfoForTimeSpent").hide();
        });
    }
}
function CheckSelectOrUnSelectSheets() {
    var checkedlist = 0;
    $(".exporttoexcel").each(function () {
        if ($(this).is(':checked')) {
            checkedlist += 1;
        }
    });
    if ($(".exporttoexcel").length == checkedlist) {
        $(".exporttoexcelpopup #chk_SelectAll").prop('checked', true);
    }
    else {
        $(".exporttoexcelpopup #chk_SelectAll").prop('checked', false);
    }
}