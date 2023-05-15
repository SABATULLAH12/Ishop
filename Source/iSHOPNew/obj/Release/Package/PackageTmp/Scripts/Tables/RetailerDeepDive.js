/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../Layout/Layout.js" />

//Date: 04-11-2016
//Written by Nagaraju D
function prepareContentArea() {
    SetDefaultCustomBase();
    var tableParamslist = [];

    var tableParams = new Object();
    if (ExportToExcel) {
        //for export
        for (var j = 0; j < ExportSheetList.length; j++) {          
            tableParams = new Object();
            tableParams.IsExportToExcel = true;
            tableParams.TabName = ExportSheetList[j].tabname;
            tableParams.TabIndexId = ExportSheetList[j].Tab_Index_Id;
            tableParams.Tab_Id_mapping = ExportSheetList[j].Tab_Id_mapping;
            if (sBevarageSelctionType.length > 0)
                tableParams.Beverage_UniqueId = sBevarageSelctionType[0].UniqueId;
            else if (sBevarageSelctionType.length == 0) {              
                tableParams.Beverage_UniqueId = 1;
            }

            tableParams.Main_SPName = ExportSheetList[j].main_spname;
            tableParams.CheckSampleSize_SPName = ExportSheetList[j].checksamplesize_spname;
            tableParams.StatTest = Selected_StatTest;
            tableParams.Sigtype_UniqueId = Sigtype_Id;

            if (TabType == ExportSheetList[j].TabType) {
                if (SelectedFrequencyList.length > 0) {
                    tableParams.ShopperFrequency = SelectedFrequencyList[0].Name;
                    tableParams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
                }
            }
            else {
                if (ExportSheetList[j].TabType == "trips") {
                    tableParams.ShopperFrequency = "Total Visits";
                    tableParams.ShopperFrequency_UniqueId = "10";
                }
                else if (ExportSheetList[j].TabType == "shopper") {
                    tableParams.ShopperFrequency = "Monthly +";
                    tableParams.ShopperFrequency_UniqueId = "2";
                }
            }
            if (Comparisonlist.length > 0) {
                if (TabType.toLocaleLowerCase() == "trips")
                    tableParams.ShopperSegment = Comparisonlist[0].TripsDBName;
                else
                    tableParams.ShopperSegment = Comparisonlist[0].ShopperDBName;

                tableParams.SingleSelection = Comparisonlist[0].Name;
                tableParams.ShopperSegment_UniqueId = Comparisonlist[0].UniqueId;
            }
            tableParams.Comparison_DBNames = [];
            tableParams.Comparison_ShortNames = [];
            tableParams.Comparison_UniqueIds = [];
            var action_method = "";
            if (CustomBase.length > 0) {
                tableParams.CustomBase_DBName = CustomBase[0].DBName;
                tableParams.CustomBase_ShortName = CustomBase[0].Name;
                tableParams.CustomBase_UniqueId = CustomBase[0].UniqueId;
            }
            if (ModuleBlock == "PIT") {
                if (!Validate_CompareRetailers_Charts() || !Validate_Group()) {
                    return false;
                }
                $("#RightPanelPartial").show();
                action_method = "GetWithinTable";
                tableParams.ViewType = "GROUPS";
                tableParams.TimePeriod = TimePeriod;
                tableParams.TimePeriodShortName = $(".timeType").val();
                tableParams.TimePeriod_UniqueId = TimePeriod_UniqueId;              

                for (var i = 0; i < Grouplist.length; i++) {
                    tableParams.Comparison_DBNames.push(Grouplist[i].DBName);
                    tableParams.Comparison_ShortNames.push(Grouplist[i].Name);
                    tableParams.Comparison_UniqueIds.push(Grouplist[i].UniqueId);
                }
            }
            else {
                if (!Validate_CompareRetailers_Charts() || !Validate_Trend()) {
                    return false;
                }
                $("#RightPanelPartial").show();
                action_method = "GetTrendTable";
                tableParams.ViewType = "TIME PERIODS";
                tableParams.Comparison_DBNames.push(TimePeriod_From);
                tableParams.Comparison_DBNames.push(TimePeriod_To);
                tableParams.Comparison_ShortNames = TimePeriod_ShortNames;

                tableParams.TimePeriodFrom_UniqueId = TimePeriodFrom_UniqueId;
                tableParams.TimePeriodTo_UniqueId = TimePeriodTo_UniqueId;
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
            if (ExportSheetList[j].TabType.toLocaleLowerCase() == "trips") {
                for (var i = 0; i < SelectedAdvFilterList.length; i++) {
                    Advanced_Filters_DBNames.push(SelectedAdvFilterList[i].DBName);
                    Advanced_Filters_ShortNames.push(SelectedAdvFilterList[i].parentName + "|" + SelectedAdvFilterList[i].Name);
                    Advanced_Filters_UniqueId.push(SelectedAdvFilterList[i].UniqueId);
                }
            }
            tableParams.Filter = Advanced_Filters_DBNames.join("|");
            tableParams.FilterShortname = Advanced_Filters_ShortNames.join("|");
            tableParams.Filter_UniqueId = Advanced_Filters_UniqueId.join("|");

            //competitor Params
            if (CompetitorRetailer.length > 0) {
                tableParams.CompetitorRetailer_Name = CompetitorRetailer[0].Name;
                tableParams.CompetitorRetailer_UniqueId = CompetitorRetailer[0].UniqueId;

                tableParams.CompetitorFrequency_Name = CompetitorFrequency[0].Name;
                tableParams.CompetitorFrequency_UniqueId = CompetitorFrequency[0].UniqueId;
            }

            tableParamslist.push(tableParams);
        }
    }
    else
    {
        tableParams.ViewType = "RETAILERS";
        tableParams.IsExportToExcel = false;
        tableParams.TabName = tabname;
        tableParams.TabIndexId = Tab_Index_Id;
        tableParams.Tab_Id_mapping = Tab_Id_mapping;
        tableParams.Main_SPName = main_spname;
        tableParams.CheckSampleSize_SPName = checksamplesize_spname;
        tableParams.StatTest = Selected_StatTest;
        tableParams.Sigtype_UniqueId = Sigtype_Id;
        if (sBevarageSelctionType.length > 0)
            tableParams.Beverage_UniqueId = sBevarageSelctionType[0].UniqueId;
        else if (sBevarageSelctionType.length == 0) {           
            tableParams.Beverage_UniqueId = 1;
        }

        if (SelectedFrequencyList.length > 0) {

            tableParams.ShopperFrequency = SelectedFrequencyList[0].Name;

            tableParams.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
        }
        if (Comparisonlist.length > 0) {
            if (TabType.toLocaleLowerCase() == "trips")
                tableParams.ShopperSegment = Comparisonlist[0].TripsDBName;
            else
                tableParams.ShopperSegment = Comparisonlist[0].ShopperDBName;

            tableParams.SingleSelection = Comparisonlist[0].Name;
            tableParams.ShopperSegment_UniqueId = Comparisonlist[0].UniqueId;
        }
        tableParams.Comparison_DBNames = [];
        tableParams.Comparison_ShortNames = [];
        tableParams.Comparison_UniqueIds = [];
        var action_method = "";
        if (CustomBase.length > 0) {
            tableParams.CustomBase_DBName = CustomBase[0].DBName;
            tableParams.CustomBase_ShortName = CustomBase[0].Name;
            tableParams.CustomBase_UniqueId = CustomBase[0].UniqueId;
        }
        if (ModuleBlock == "PIT") {
            if (!Validate_CompareRetailers_Charts() || !Validate_Group()) {
                return false;
            }
            $("#RightPanelPartial").show();
            action_method = "GetWithinTable";
            tableParams.TimePeriod = TimePeriod;
            tableParams.TimePeriodShortName = $(".timeType").val();
            tableParams.TimePeriod_UniqueId = TimePeriod_UniqueId;

            for (var i = 0; i < Grouplist.length; i++) {
                tableParams.Comparison_DBNames.push(Grouplist[i].DBName);
                tableParams.Comparison_ShortNames.push(Grouplist[i].Name);
                tableParams.Comparison_UniqueIds.push(Grouplist[i].UniqueId);
            }
        }
        else {
            if (!Validate_CompareRetailers_Charts() || !Validate_Trend()) {
                return false;
            }
            $("#RightPanelPartial").show();
            action_method = "GetTrendTable";
            tableParams.Comparison_DBNames.push(TimePeriod_From);
            tableParams.Comparison_DBNames.push(TimePeriod_To);
            tableParams.Comparison_ShortNames = TimePeriod_ShortNames;

            tableParams.TimePeriodFrom_UniqueId = TimePeriodFrom_UniqueId;
            tableParams.TimePeriodTo_UniqueId = TimePeriodTo_UniqueId;
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
        tableParams.Filter = Advanced_Filters_DBNames.join("|");
        tableParams.FilterShortname = Advanced_Filters_ShortNames.join("|");
        tableParams.Filter_UniqueId = Advanced_Filters_UniqueId.join("|");
        //competitor Params
        if (CompetitorRetailer.length > 0) {
            tableParams.CompetitorRetailer_Name = CompetitorRetailer[0].Name;
            tableParams.CompetitorRetailer_UniqueId = CompetitorRetailer[0].UniqueId;

            tableParams.CompetitorFrequency_Name = CompetitorFrequency[0].Name;
            tableParams.CompetitorFrequency_UniqueId = CompetitorFrequency[0].UniqueId;
        }

        tableParamslist.push(tableParams);
    }
    postBackData = "{tableParams:" + JSON.stringify(tableParamslist) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLTables").val() + "/" + action_method,
        async: true,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!isAuthenticated(data))
                return false;

            if (ExportToExcel)
                DownloadExcel();
            else
                PrepareTable(data);

            ExportToExcel = false;
        },
        error: function (xhr, status, error) {
            GoToErrorPage();
        }
    });
}
function PrepareTable(data) {
    if ($(".adv-fltr-showhide-txt").text().toUpperCase() == "SHOW LESS")
        $("#Table-Content").css("height", "calc(100% - 192px)");
    else
        $("#Table-Content").css("height", "calc(100% - 133px)");
    
    var maintbwidth = 900;
    var FixedColumnWidth = 300;
    var FixedTableHeight = 400;
    if (data.LeftHeader != "" && data.LeftBody != "" && data.RightHeader != "" && data.RightBody != "") {
        PlotTable(data.LeftHeader, data.LeftBody, data.RightHeader, data.RightBody, maintbwidth, FixedColumnWidth, FixedTableHeight);
    }
    else
    {
        NoDataAvailable();
    }
}
function PlotTable(leftheader, leftbody, rightheader, rightbody, _width, _fixedColumnWidth, _fixedTableheight) {
    //left table header
    var table = "<div class=\"leftheader\" style=\"\">";
    table += leftheader;
    table += "</div>";
    //end left table header

    //right table header
    table += "<div class=\"rightheader\" style=\"overflow:hidden;\">";
    table += rightheader;
    table += "</div>";
    //end right table header

    //left table body
    table += "<div class=\"leftbody\" style=\"overflow:hidden;\">";
    table += leftbody;
    table += "</div>";
    //  

    //right table body  
    table += "<div onscroll=\"reposVertical(this);\" class=\"rightbody\" style=\"overflow:auto;\">";
    table += rightbody;
    table += "</div>";
    //end left table body   

    $("#Table-Content").html("");
    $("#Table-Content").html(table);
    SetStyles();
}
function reposVertical(e) {
    $(".rightheader").scrollTop(e.scrollTop);
    $(".leftbody").scrollTop(e.scrollTop);
    $(".rightbody").scrollTop(e.scrollTop);

    $(".rightheader").scrollLeft(e.scrollLeft);
    $(".rightbody").scrollLeft(e.scrollLeft);
}