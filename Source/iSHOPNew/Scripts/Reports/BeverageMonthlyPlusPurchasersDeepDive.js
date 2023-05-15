/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../Layout/Layout.js" />

//Date: 04-11-2016
//Written by Nagaraju D
function prepareContentArea() {
    if (!Validate_Report_SingleCompareBeverages()) {
        return false;
    }
    var reportparams = new Object();
    reportparams.Tab_Id_mapping = true;
    reportparams.TabName = tabname;
    if (SelectedFrequencyList.length > 0)
        reportparams.ShopperFrequency = SelectedFrequencyList[0].Name;

    reportparams.StatTest = "";
    reportparams.SelectedReports = [];
    reportparams.Comparison_DBNames = [];
    reportparams.Comparison_ShortNames = [];
    reportparams.Comparison_UniqueIds = [];

    if (ModuleBlock == "PIT") {
        if (!Validate_Report_Group()) {
            return false;
        }
        reportparams.ModuleBlock = "WithinBeverageShopper";
        //reportparams.SPName = "usp_BeveragesMonthlyplusReportWithin";
        reportparams.SPName = "usp_ReportBeverageMonthlyDeepDivePITShoppers";
        if (IsProceedGenerateReport)
            reportparams.SampleSizeCheck = 1;
        else
            reportparams.SampleSizeCheck = 0;

        reportparams.TimePeriod = TimePeriod;
        reportparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
        reportparams.TimePeriodShortName = $(".timeType").val();
        reportparams.ShortTimePeriod = $(".timeType").val();
        for (var i = 0; i < Grouplist.length; i++) {
            reportparams.Comparison_DBNames.push(Grouplist[i].DBName);
            reportparams.Comparison_ShortNames.push(Grouplist[i].Name);
            reportparams.Comparison_UniqueIds.push(Grouplist[i].UniqueId);
        }
    }
    else {
        if (!Validate_Report_Trend()) {
            return false;
        }
        reportparams.ModuleBlock = "TimeBeverageShopper";
        //reportparams.SPName = "usp_BeveragesMonthlyplusReportTrend";
        reportparams.SPName = "usp_ReportBeverageMonthlyDeepDiveTrendShoppers";
        if (IsProceedGenerateReport)
            reportparams.SampleSizeCheck = 1;
        else
            reportparams.SampleSizeCheck = 0;

        reportparams.Comparison_DBNames.push(TimePeriod_From);
        reportparams.Comparison_DBNames.push(TimePeriod_To);
        reportparams.Comparison_ShortNames = TimePeriod_ShortNames;

        reportparams.ShortTimePeriod = $(".timeType").val();
        reportparams.TimePeriodFrom_UniqueId = TimePeriodFrom_UniqueId;
        reportparams.TimePeriodTo_UniqueId = TimePeriodTo_UniqueId;
    }
   
    if (ComparisonBevlist.length > 0) {
        if (TabType.toLocaleLowerCase() == "trips")
            reportparams.ShopperSegment = ComparisonBevlist[0].TripsDBName;
        else
            reportparams.ShopperSegment = ComparisonBevlist[0].ShopperDBName;

        reportparams.ShopperSegmentShortName = ComparisonBevlist[0].Name;
        reportparams.ShopperSegment_UniqueId = ComparisonBevlist[0].UniqueId;
    }
    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_UniqueId = [];
    //Guest advanced filters
    for (var i = 0; i < SelectedDempgraphicList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].FullName);
        Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
    }
    //Visits advanced filters
    for (var i = 0; i < SelectedAdvFilterList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedAdvFilterList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedAdvFilterList[i].FullName);
        Advanced_Filters_UniqueId.push(SelectedAdvFilterList[i].UniqueId);
    }
    reportparams.Filters = Advanced_Filters_DBNames.join("|");
    reportparams.FilterShortname = Advanced_Filters_ShortNames.join(", ");
    reportparams.FilterShortNames = Advanced_Filters_ShortNames.join(", ");
    reportparams.Filter_UniqueId = Advanced_Filters_UniqueId.join("|");

    postBackData = "{reportparams:" + JSON.stringify(reportparams) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLReports").val() + "/BuildSlides",
        async: true,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            IsProceedGenerateReport = false;
            if (!isAuthenticated(data))
                return false;

            if (data != null || data != "") {
               if ((data.LowSampleSizeItems != null && data.LowSampleSizeItems.length > 0) || (data.UseDirectionallyItems != null && data.UseDirectionallyItems.length)) {
                   // $("#UpdateProgress").hide();
                    ShowLowSampleSize(data);
                }
                else {
                    DownloadReport();
                }
            }
        },
        error: function (xhr, status, error) {
            GoToErrorPage();
        }
    });
}
function DownloadReport() {
    var localTime = new Date();
    var year = localTime.getFullYear();
    var month = localTime.getMonth() + 1;
    var date = localTime.getDate();
    var hours = localTime.getHours();
    var minutes = localTime.getMinutes();
    var seconds = localTime.getSeconds();
    if (ModuleBlock == "PIT") 
        window.location.href = $("#URLReports").val() + "/Download?View=WithinBeverageShopper&year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
    else
        window.location.href = $("#URLReports").val() + "/Download?View=TimeBeverageShopper&year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
}

