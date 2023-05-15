﻿/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../Layout/Layout.js" />

//Date: 04-11-2016
//Written by Nagaraju D
function prepareContentArea() {

    var reportparams = new Object();
    reportparams.Tab_Id_mapping = true;
    reportparams.ReportFlag = 4;
    reportparams.TabName = tabname;
    if (SelectedFrequencyList.length > 0) {
        reportparams.ShopperFrequency = SelectedFrequencyList[0].Name;
        reportparams.ShopperFrequencyShortName = SelectedFrequencyList[0].Name;
        reportparams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }

    reportparams.SelectedReports = ["SUMMARY", "VISITORPROFILE", "TRIPTYPE", "PRESHOP", "INSTORE", "TRIPSUMMARY", "APPENDIX"];
    reportparams.Comparison_DBNames = [];
    reportparams.Comparison_ShortNames = [];
    reportparams.Comparison_UniqueIds = [];
    if (!Validate_DeepDive_Retailer_ShopperSegment()) {
        return false;
    }
    if (ModuleBlock == "PIT") {
        if (!Validate_Report_Group())
        {
            return false;
        }
        reportparams.ModuleBlock = "WithinTrips";
        reportparams.SPName = "usp_ReportWithinRetailerTrip";
        if (IsProceedGenerateReport)
            reportparams.SampleSizeCheck = 1;
        else
            reportparams.SampleSizeCheck = 0;
        reportparams.TimePeriod = TimePeriod;
        reportparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
        reportparams.TimePeriodShortName = $(".timeType").val();
        reportparams.ShortTimePeriod = $(".timeType").val();
        for (var i = 0; i < Grouplist.length; i++) {
            reportparams.Comparison_DBNames.push(Grouplist[i].parentName + "|" + Grouplist[i].Name);
            reportparams.Comparison_ShortNames.push(Grouplist[i].Name);
            reportparams.Comparison_UniqueIds.push(Grouplist[i].UniqueId);
        }
    }
    else {
        if (!Validate_Report_Trend()) {
            return false;
        }
        reportparams.ModuleBlock = "TimeTrips";
        //reportparams.SPName = "usp_RetailerOverviewReportsForTrend";
        reportparams.SPName = "usp_ReportTrendRetailerTrip";
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
    if (Comparisonlist.length > 0) {
        reportparams.ShopperSegment = Comparisonlist[0].LevelDesc + "|" + Comparisonlist[0].Name;
        reportparams.ShopperSegment_UniqueId = Comparisonlist[0].UniqueId;
    }

    reportparams.Filters = Advanced_Filters_DBNames.join("|");
    reportparams.FilterShortname = Advanced_Filters_ShortNames.join(", ");
    reportparams.FilterShortNames = Advanced_Filters_ShortNames.join(", ");
    reportparams.Filter_UniqueId = Advanced_Filters_UniqueId.join("|");

    if ($(".stat-popup").is(":visible") == false && reportPopupFlag == 0) {
        CustomBasePopup();
        return;
    }
    else {
        $(".stat-popup").hide();
        $(".TranslucentDiv").hide();
    }

    //Stat Test
    reportparams.StatTest = Selected_StatTest;
    reportparams.Sigtype_UniqueId = Sigtype_Id;
    if (CustomBase.length > 0) {
        reportparams.CustomBase_ShortName = CustomBase[0].Name;
        reportparams.CustomBase_UniqueId = CustomBase[0].UniqueId;
    }

    //competitor Params
    if (CompetitorRetailer.length > 0) {
        reportparams.CompetitorRetailer_Name = CompetitorRetailer[0].Name;
        reportparams.CompetitorRetailer_UniqueId = CompetitorRetailer[0].UniqueId;

        reportparams.CompetitorFrequency_Name = CompetitorFrequency[0].Name;
        reportparams.CompetitorFrequency_UniqueId = CompetitorFrequency[0].UniqueId;
    }

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
               reportPopupFlag = 0;
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
    window.location.href = $("#URLReports").val() + "/Download?View=" + ModuleBlock + "&year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
}