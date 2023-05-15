/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../Layout/Layout.js" />

//Date: 04-11-2016
//Written by Nagaraju D
function prepareContentArea() {
    if (!Validate_Report_CompareBeverages()) {
        return false;
    }
    var reportparams = new Object();
    reportparams.Tab_Id_mapping = true;
    reportparams.TabName = tabname;
    reportparams.ModuleBlock = "AcrossBeverageShopper";
    if (SelectedFrequencyList.length > 0)
        reportparams.ShopperFrequency = SelectedFrequencyList[0].Name;

    reportparams.StatTest = "";
    reportparams.TimePeriod = TimePeriod;
    reportparams.TimePeriodShortName = $(".timeType").val();
    reportparams.ShortTimePeriod = $(".timeType").val();
    reportparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
    //reportparams.SPName = "usp_BeveragesMonthlyplusReportAcross";
    reportparams.SPName = "usp_ReportCompareBeverageMonthlyShoppers";
    if (IsProceedGenerateReport)
        reportparams.SampleSizeCheck = 1;
    else
        reportparams.SampleSizeCheck = 0;

    reportparams.SelectedReports = ["AcrossBeverageShopper"];
    reportparams.Comparison_DBNames = [];
    reportparams.Comparison_ShortNames = [];
    reportparams.Comparison_UniqueIds = [];
    for (var i = 0; i < ComparisonBevlist.length; i++) {
        if (TabType.toLocaleLowerCase() == "trips")
            reportparams.Comparison_DBNames.push(ComparisonBevlist[i].Name);
        else
            reportparams.Comparison_DBNames.push(ComparisonBevlist[i].Name);

        reportparams.Comparison_ShortNames.push(ComparisonBevlist[i].Name);
        reportparams.Comparison_UniqueIds.push(ComparisonBevlist[i].UniqueId);
    }
    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
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

           if ((data.LowSampleSizeItems != null && data.LowSampleSizeItems.length > 0) || (data.UseDirectionallyItems != null && data.UseDirectionallyItems.length)) {
                $("#UpdateProgress").hide();
                ShowLowSampleSize(data);
            }
            else {
                DownloadReport();
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
    window.location.href = $("#URLReports").val() + "/Download?View=AcrossBeverageShopper&year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
}