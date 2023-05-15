var low_samplesize_list = [];
var IsProceedGenerateReport = false;
var report_data = null;
var samplesize_check = 0;
var proceedGenerateReport = false;
var reportPopupFlag = 0;

function CustomBasePopup() {
    $(".stat-popup").show();
    $(".TranslucentDiv").show();
    $(".stat-popup").css({ "z-index": "9900" });
    $(".stat-popup .stat-heading").text("SELECT STAT TESTING BASE");
    $(".stat-popup .stat-sub-heading").text("(ANY ONE)");
    $(".stat-popup .stat-submt").attr("onclick", "CloseReportStatPopup()");
    $(".stat-popup .stat-submt").text("download");
    $(".stat-popup .stat-submt").css({ "margin-left": "160px" });
    prepareStatTestContent();
    $(".stat-popup .stat-cust-estabmt").css({"width": "89%"});
    $(".stat-popup .stat-cust-dot").css("width", "8%");
    $(".stat-cust-dot").show();
}

function prepareStatTestContent() {
    $(".stat-content").html("");
    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }

    var statList = [];
    statList = Comparisonlist;
    var html = '',leftDiv='',rightDiv='', header = (currentpage.indexOf("retailer") > -1 ? "Retailers" : "Beverages"),trendText='';
    var width = 50;
    
    if (currentpage.indexOf("deepdive") > -1) {
        if (ModuleBlock == "TREND") {
            header = "Custom Base Time Period";
            trendText += "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div sigtype-id='2' sigtype='TOTAL TIME' class='stat-cust-estabmt' onclick='updateStatTestValues(this)'>TOTAL TIME</div></div>";
            if (SelectedDempgraphicList.filter(function (d) { return d.isGeography == "true" }).length > 0) {
                statList = selectQuaterEndingTimePeriod(JSON.parse(JSON.stringify(TrendCustomBaselist)));
            }
            else {
                statList = TrendCustomBaselist;
            }
        }
        else {
            statList = Grouplist;
            header = "Metric Comparison";
        }
            
    }
    if (TimeExtension == 'total time')
        width = 100;

    leftDiv = "<div class='leftStatDiv' style='width:" + width + "%;float:left'>";
    rightDiv = "<div class='rightStatDiv' style='width:" + width + "%;float:right'>";

    leftDiv += "<div class=\"stat-custdiv\"><div sigtype-id='4' class='stat-inside-header'>" + header + "</div></div>";

    if (TimeExtension != 'total time') {
        rightDiv += "<div class=\"stat-custdiv\"><div sigtype-id='4' class='stat-inside-header'>TIME PERIOD</div></div>";
    }

    for (var i = 0; i < statList.length; i++) {
        leftDiv += "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div sigtype-id='1' sigtype='CUSTOM BASE' name=\"" + statList[i].Name + "\" uniqueid=\"" + statList[i].UniqueId + "\" shopperdbname=\"" + statList[i].Name + "\" tripsdbname=\"" + statList[i].Name + "\" class=\"stat-cust-estabmt\" onclick='updateStatTestValues(this)'>" + statList[i].Name + "</div></div>";
        if (i == 0 && TimeExtension != 'total time') {
            rightDiv += trendText + "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div sigtype-id='4' sigtype='PREVIOUS YEAR' class='stat-cust-estabmt' onclick='updateStatTestValues(this)'>PREVIOUS YEAR</div></div>";
        }
        else if (i == 1 && TimeExtension != 'total time') {
            rightDiv += "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div sigtype-id='3' sigtype='PREVIOUS PERIOD' class='stat-cust-estabmt' onclick='updateStatTestValues(this)'>PREVIOUS PERIOD</div></div>";
        }       
    }
    leftDiv += "</div>";
    rightDiv += "</div>";
    $(".stat-content").append(leftDiv + rightDiv);

    $(".stat-popup .leftStatDiv .stat-cust-estabmt").first().trigger("click");
}

function updateStatTestValues(e) {
    CustomBase = [];
    Sigtype_Id = $(e).attr('sigtype-id');
    Selected_StatTest = $(e).attr('sigtype');
    if (Sigtype_Id == 1) {
        CustomBase.push({ "Name": $(e).attr('name'), "UniqueId": $(e).attr('uniqueid') });
    }
}


function CloseReportStatPopup() {
    if ($(".stat-popup .stat-cust-active").length > 0) {
        reportPopupFlag = 1;
        prepareContentArea();
    }
}

function ShowLowSampleSize(data) {
    samplesize_check = 1;
    report_data = data;
    low_samplesize_list = data.LowSampleSizeItems;
    proceedGenerateReport = false;
    if (data.LowSampleSizeItems != null && data.LowSampleSizeItems.length > 0) {    
        updateSampleSize("Sample size is less than " + MinSampleSize + " for the following", data.LowSampleSizeItems);
    }
    else {
        proceedGenerateReport = true;
        updateSampleSize("Sample size is between " + (MinSampleSize) + " and " + (MaxSampleSize - 1) + " Use Directionally", data.UseDirectionallyItems);      
    }

    Set_zIndex();     
}

function updateSampleSize(title, samplesizelist)
{
    if (report_data.IsAllLowSampleSizes)
        $("#report-popup .proceedClick").hide();      
    else
        $("#report-popup .proceedClick").show();

    $("#report-popup .heading_text").html(title);
    $("#report-popup .list-of-nulls").html("");
    if (samplesizelist != null && samplesizelist.length > 0) {
        for (var i = 0; i < samplesizelist.length; i++) {
            $("#report-popup .list-of-nulls").append("<div class=\"stat-custdiv\" style=\"pointer-events:none\"><div class=\"stat-cust-dot\"></div><div class=\"stat-cust-estabmt popup1\">" + samplesizelist[i].Name + "</div></div>");
        }
    }
    $("#report-popup").css("display","block");
}

function CloseReportPopup()
{
    reportPopupFlag = 0;
    $("#report-popup").css("display", "none");
    $(".TranslucentDiv").hide();
    $("#UpdateProgress").hide();
}
function ProceedGenerateReport() {
    samplesize_check++;
    if (samplesize_check == 2 && !proceedGenerateReport && report_data.UseDirectionallyItems != null && report_data.UseDirectionallyItems.length > 0) {
        updateSampleSize("Sample size is between " + (MinSampleSize) + " and " + (MaxSampleSize - 1) + " Use Directionally", report_data.UseDirectionallyItems);
        return;
    }
    $("#report-popup").css("display", "none");
    removeLowsamplesizeItems();
    IsProceedGenerateReport=true;
    $(".exporttoexcelpopup").hide();  
        prepareContentArea();   
}
//remove low sample size items from the list
function removeLowsamplesizeItems() {
    var searchObj;
    var obj = null;
    if (low_samplesize_list != null && low_samplesize_list.length > 0) {
        for (var i = 0; i < low_samplesize_list.length; i++) {
            if (currentpage == "hdn-report-compareretailersshoppers" || currentpage == "hdn-report-compareretailerspathtopurchase")
            {
                obj = GetLowSampleSizeItem(low_samplesize_list[i].Name, Comparisonlist);
                if (obj != null) {
                    searchObj = $("#RetailerDivId li[uniqueid='" + obj.UniqueId + "']");
                    SelectComparison(searchObj);
                }
            }
            else if ((currentpage == "hdn-report-retailersshopperdeepdive" && ModuleBlock == "PIT")
                || (currentpage == "hdn-report-retailerspathtopurchasedeepdive" && ModuleBlock == "PIT")
                || (currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive" && ModuleBlock == "PIT")
                || (currentpage == "hdn-report-beveragespurchasedetailsdeepdive" && ModuleBlock == "PIT")) {
                obj = GetLowSampleSizeItem(low_samplesize_list[i].Name, Grouplist);
                if (obj != null) {
                    searchObj = $("#groupDivId ul li[parentname='" + obj.parentName + "'][uniqueid='" + obj.UniqueId + "']");
                    SelecGroupMetricName(searchObj);
                }
            }
            else if (currentpage == "hdn-report-comparebeveragesmonthlypluspurchasers" || currentpage == "hdn-report-comparebeveragespurchasedetails") {
                obj = GetLowSampleSizeItem(low_samplesize_list[i].Name, ComparisonBevlist);
                if (obj != null) {
                    searchObj = $("#BevDivId li[uniqueid='" + obj.UniqueId + "']");
                    SelectBevComparison(searchObj);
                }
            }
        }
    }
    ShowSelectedFilters();
}
function GetLowSampleSizeItem(name, Array) {
    for (var i = 0; i < Array.length; i++) {
        if (Array[i].Name.toLowerCase() == name.toLowerCase()) {
            if (Sigtype_Id==1 && CustomBase[0].Name.toLowerCase() == name.toLowerCase())
                reportPopupFlag = 0;          
            return Array[i];
            break;
        }      
    }
    return null;
}
function Validate_Report_CompareRetailers() {
    if (Comparisonlist.length < 2) {
        showMessage("Please select minimum 2 Retailers");
        return false;
    }
    return true;
}
//----> Compare Beverages -------->
function Validate_Report_CompareBeverages() {
    if (ComparisonBevlist.length < 2) {
        showMessage("Please select minimum 2 Beverages");
        return false;
    }
    return true;
}

function Validate_Report_SingleCompareBeverages() {
    if (ComparisonBevlist.length < 1) {
        showMessage("Please select minimum 1 Beverages");
        return false;
    }
    return true;
}
//----> Group-------->
function Validate_Report_Group() {
    if (Grouplist.length < 2) {
        showMessage("Please select minimum 2 Groups");
        return false;
    }
    return true;
}
function Validate_Report_Trend() {
    if (TimePeriod_ShortNames.length > 6) {
        showMessage("YOU CAN SELECT UPTO 6 TIME PERIODS");
        return false;
    }
    return true;
}