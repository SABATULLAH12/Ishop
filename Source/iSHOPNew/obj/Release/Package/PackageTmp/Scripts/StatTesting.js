/// <reference path="Layout/Layout.js" />
/// <reference path="Layout/RightPanelFilter.js" />

//Stat test block 05/10/2015
var CustomBasePrev = [];
$(document).ready(function () {
    $(".LowerRightContent").hide();
    $("#custom-base-cancel").click(function (e) {
        $("#Translucent").hide();
        $(".Custombase-GroupType").hide();
        Remove_zIndex();
        $(".StatArea").hide();
        $(".stat-popup").hide();
        $(".exporttoexcelpopup").hide();
        $(".exportchartlistpopup").hide();
        $("#Translucent").hide();
        if (TimeExtension.toLocaleLowerCase() != "total time") {
            if (Selected_StatTest.trim().toLocaleLowerCase() != "custom base")
                SetDefaultStatTest($(stattest_obj));
            CustomBase = CustomBasePrev.length != 0 ? CustomBasePrev : CustomBase;
        }       
        if (Selected_StatTest.trim().toLocaleLowerCase() == "custom base") {
            CustomBase = CustomBasePrev.length != 0 ? CustomBasePrev : CustomBase;
        }
        else {
            CustomBase = CustomBase;
        }
        var _custom_base = $("#Custombase-RetailerDivId span[uniqueid='" + CustomBase[0].UniqueId + "']");
        //if (_custom_base.length == 0)
        //    _custom_base = $(".Custombase-Retailers li[uniqueid='" + CustomBase[0].Id + "']");

        SelectPathToPurchaseCustomBase(_custom_base);
        ShowSelectedFilters();
        if (currentpage == "hdn-dashboard-pathtopurchase") {
            if (!$("#custom-base-customise").is(':visible')) {
                $("#custom-base-customise").trigger("click");
            }
        }
        $(".Custombase-Retailers").hide();
        e.stopImmediatePropagation();
    });
    $("#custom-addfilter-cancel").click(function (e) {
        $("#Translucent").hide();
        isCompetitorCustomBase = false;
        $(".Custombase-GroupType").hide();
        Remove_zIndex();
        $(".StatArea").hide();
        $(".stat-popup").hide();
        $(".exporttoexcelpopup").hide();
        $(".exportchartlistpopup").hide();
        $("#Translucent").hide();
        //update cancelled filters    
        custombase_AddFilters = [];
        $("#custombase-groupDivId ul li").removeClass("Selected");
        for (var i = 0; i < custombase_Canceled_AddFilters.length; i++) {
            var cus_Obj = $("#custombase-groupDivId div[level-id='" + custombase_Canceled_AddFilters[i].levelId + "'] ul li[parentname='" + custombase_Canceled_AddFilters[i].parentName + "'][uniqueid='" + custombase_Canceled_AddFilters[i].UniqueId + "']");
            SelecCustomBaseGroupMetricName(cus_Obj);
        }
        ShowSelectedFilters();

        $("#custom-base-customise").click();
        e.stopImmediatePropagation();
    })
    $("#Custom-Base-Retailer-Popup").click(function (e) {
        $("#custom-base-on-submit, #custom-base-customise").hide();
        $("#custom-base-customise-ok").show();
        $(".Custombase-GroupType").hide();
        $("#custombase-stattesting-popup").hide();
        $("#Translucent").show();
        $(".Custombase-Retailers").show();       
    });
    $("#custom-base-onsubmit-btn").click(function (e) {
        $("#custombase-stattesting-popup").hide();
        $("#Translucent").hide();
        CustomBaseFlag = 0;
        prepareContentArea();
    });
    $("#Custom-Base-Add-Filter-Popup").click(function (e) {
        custombase_Canceled_AddFilters = [];
      
        isCompetitorCustomBase = true;

        for (var i = 0; i < custombase_AddFilters.length; i++) {
            custombase_Canceled_AddFilters.push({
                Id: custombase_AddFilters[i].Id,
                Name: custombase_AddFilters[i].Name,
                FullName: custombase_AddFilters[i].FullName,
                parentId: custombase_AddFilters[i].parentId,
                parentName: custombase_AddFilters[i].parentName,
                UniqueId: custombase_AddFilters[i].UniqueId,
                isGeography: custombase_AddFilters[i].isGeography,
                filtertype: custombase_AddFilters[i].filtertype,
                levelId: custombase_AddFilters[i].levelId
            });
        }

        $(".Custombase-Retailers").hide();
        $("#custombase-stattesting-popup").hide();
        $("#Translucent").show();
        updateSearch($(this));
        resetLevels($("#custombase-groupDivId"));
        $(".Custombase-GroupType").show();
        if (currentpage == "hdn-dashboard-demographic") {
            hideShowVisitShopperFilters();
        }
        e.stopImmediatePropagation();
    });
    $("#custom-base-customise, #custom-base-customise-ok").click(function (e) {
        $(".Custombase-GroupType").hide();
        $(".Custombase-Retailers").hide();
        $("#Translucent").show();
        $("#custombase-stattesting-popup").show();
        e.stopImmediatePropagation();
    });
    $("#custom-base-ok").click(function (e) {
        isCompetitorCustomBase = false;
        $(".Custombase-GroupType").hide();
        $(".Custombase-Retailers").hide();
        $("#Translucent").show();
        $("#custombase-stattesting-popup").show();
        e.stopImmediatePropagation();
    });
   
    $(document).on("click", ".stat-clsebtn, .stat-cancel,.custom-cancel", function (e) {
        $("#Translucent").hide();
        Remove_zIndex();
        $(".StatArea").hide();
        $(".stat-popup").hide();
        $(".exporttoexcelpopup").hide();
        $(".exportchartlistpopup").hide();
        $("#Translucent").hide();
        if (TimeExtension.toLocaleLowerCase() != "total time") {
            if (Selected_StatTest.trim().toLocaleLowerCase() != "custom base")
                SetDefaultStatTest($(stattest_obj));
        }
    });
    $(document).on("click", ".stat-cust-estabmt:not(#custombase-stattesting-popup .stat-cust-estabmt)", function (e) {
        $(".stat-cust-estabmt").removeClass("stat-cust-active");
        $(this).addClass("stat-cust-active");
        obj_CustomBase = $(this);
    });
    $(document).on("click", ".stattest", function () {
        ClosePopups();
        if ($(this).hasClass("stattestdeactive"))
            return false;
        if ((document.getElementById('adv-fltr visitsId') != null) || (document.getElementById('adv-fltr visitsTrendId') != null)) {
            $(".advance-filters").show();
            var navbarwidth = document.getElementById('adv-fltr visitsId').offsetWidth;
            if (currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-tbl-retailerdeepdive") {
                if (ModuleBlock.toUpperCase() == "TREND")
                    navbarwidth = document.getElementById('adv-fltr visitsTrendId').offsetWidth;
                else
                    navbarwidth = document.getElementById('adv-fltr visitsId').offsetWidth;
            }

            if (currentpage == "hdn-tbl-beveragedeepdive" || currentpage == "hdn-tbl-comparebeverages")
                var sWidth = (navbarwidth * 19.19) / 100;
            else
                var sWidth = (navbarwidth * 24.6) / 100;
            $(".width-5").css("width", sWidth);
        }
        if (TimeExtension.toLocaleLowerCase() != "total time") {
            $(".stattest:not(.stattestdeactive)").css("background-color", "transparent");
            $(".stattest:not(.stattestdeactive)").css("color", "black");
        }
        //$(this).css("background-color", "#505050");
        $(this).css("background-color", "#EB1F2A");
        $(this).css("color", "white");       
        if ($(this).children("span").html().toLowerCase() == "base")
        {
            Selected_StatTest = "";
        }
        else if ($(this).children("span").html().toLowerCase() == "custom base") {
            if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {
                if (currentpage == "hdn-dashboard-pathtopurchase" || currentpage == "hdn-dashboard-demographic") {                
                    resetLevels($("#Custombase-RetailerDivId"));
                    $("#custom-base-on-submit, #custom-base-customise").show();
                    $("#custom-base-customise-ok").hide();
                }
                $("#Translucent").show();
                $(".Custombase-Retailers").show();
                CustomBasePrev = [];
                if (CustomBase.length > 0) {
                    CustomBasePrev.push({
                        Id: CustomBase[0].Id,
                        Name: CustomBase[0].Name,
                        DBName: CustomBase[0].DBName,
                        ShopperDBName: CustomBase[0].ShopperDBName,
                        TripsDBName: CustomBase[0].TripsDBName,
                        UniqueId: CustomBase[0].UniqueId
                    });
                }
                updateSearch($(this).parent());
                SetScroll($("#Custombase-RetailerDivId #ChannelOrCategoryContent"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
                ShowSelectedFilters();
            }
            else {
                prepareCustomBase();
                $(".stat-popup").show();
                $("#Translucent").show();
                SetDefaultCustomBase();
                ActiveteSelectedCustomBase();
            }
            return false;
        }
        else
        {
            custombase_AddFilters = [];

            //New code
            CustomBase = []
            $(".Custombase-Retailers li").find(".Selected").removeClass("Selected");
            $(".Custombase-Retailers #ChannelOrCategoryContent *").find(".Selected").removeClass("Selected");
            $(".Custombase-Retailers .Retailer *").find(".Selected").removeClass("Selected");
            $(".Custombase-Retailers .Retailer .Comparison").find(".Selected").removeClass("Selected");
            $("#Custombase-RetailerDivId *").find(".Selected").removeClass("Selected");

            custombase_Frequency = []
            $(".Custombase-GroupType").find(".Selected").removeClass("Selected");

            CompetitorCustomBaseFrequency = [];
            CompetitorCustomBaseRetailer = [];
            $(".CompetitorFrequency-Retailers").find(".Selected").removeClass("Selected");

            stattest_obj = $(this);
            Selected_StatTest = $(this).children("span").html();
            Sigtype_Id = $(this).children("span").attr("sigtype-Id");
            CustomBaseFlag = 0;
        }
        if (currentpage == "hdn-analysis-withintrips" || currentpage.indexOf("tbl") > -1)
            $(".advance-filters").css("display", "block");
        ShowSelectedFilters();
        prepareContentArea();
    });
});
//added by Nagaraju for Total Time
//03-05-2017
function SetCustomBaseforTotalTime(obj)
{
    if ($(obj).attr("name").toLowerCase() == "total time") {
        $(".stattest:not(.stattestdeactive)").css("background-color", "gray").css("color", "black").css("cursor", "auto").css("pointer-events", "none");

        $("#stattest_benchmark .stattest").css("background-color", "#EB1F2A").css("color", "white").css("cursor", "pointer").css("pointer-events", "auto");
        var objcus = $("#stattest_benchmark .stattest");
        stattest_obj = $(objcus);
        Selected_StatTest = $(objcus).children("span").html();
        Sigtype_Id = $(objcus).children("span").attr("sigtype-Id");
    }
    else if ($(obj).attr("name").toLowerCase() != "total time") {
        $(".stattest").each(function () {
            if ($(this).css("pointer-events") == "none") {
                $(this).css("background-color", "transparent").css("color", "black").css("cursor", "pointer").css("pointer-events", "auto");
            }
        });

        //$(".stattest:not(.stattestdeactive)").css("background-color", "transparent").css("color", "black").css("cursor", "pointer").css("pointer-events", "auto");
        //$("#stattest_benchmark .stattest").css("background-color", "#EB1F2A").css("color", "white").css("cursor", "pointer").css("pointer-events", "auto");
        //var objcus = $("#stattest_benchmark .stattest");
        //stattest_obj = $(objcus);
        //Selected_StatTest = $(objcus).children("span").html();
        //Sigtype_Id = $(objcus).children("span").attr("sigtype-Id");
    }   
}
function SetDefaultCustomBase()
{
    prepareCustomBase();
    if (CustomBase.length == 0 && $(".stat-popup .stat-custdiv").length > 0) {
        SetCustomBase($(".stat-popup .stat-custdiv .stat-cust-estabmt").eq(0));
    }
    if (CustomBase.length > 0 && $(".stat-popup .stat-custdiv").length > 0) {
        var isCustombaseexist = false;
        $(".stat-popup .stat-custdiv .stat-cust-estabmt").each(function () {
            if ($(this).html().toLocaleLowerCase().trim() == CustomBase[0].Name.toLocaleLowerCase().trim() && $(this).attr("uniqueid") == CustomBase[0].UniqueId) {
                isCustombaseexist = true;
                return true;
            }
        });
        if(!isCustombaseexist)
        {
            SetCustomBase($(".stat-popup .stat-custdiv .stat-cust-estabmt").eq(0));
        }
    }
}
function ActiveteSelectedCustomBase()
{    
    if(CustomBase.length > 0)
    {
        $(".stat-popup .stat-custdiv .stat-cust-estabmt").each(function () {
            if($(this).html().toLocaleLowerCase().trim() == CustomBase[0].Name.toLocaleLowerCase().trim())
            {
                $(this).trigger("click");
                return true;              
            }
        });
    }
}
function selectQuaterEndingTimePeriod(TrendCustomBaselist)
{
    var Custombaselist = TrendCustomBaselist;
    var geoID = SelectedDempgraphicList.filter(function (d) { return d.isGeography == "true" })[0].Id;
    var level = 0, i = 0; check = -1;
    for (level = 0; level < customRegions[0].Levels.length; level++) {
        for (i = 0; i < customRegions[0].Levels[level].LevelItems.length; i++) {
            if (customRegions[0].Levels[level].LevelItems[i].Id == geoID) {
                check = 1;
                break;
            }
        }
        if (check == 1)
            break;
    }
    var obj = customRegions[0].Levels[level].LevelItems[i];
    var timePeriodlist = obj.GeoTimePeriods.filter(function (d) { return d.split('|')[0].toLowerCase() == TimeExtension.toLowerCase() });

    for (var i = 0, j = 0; i < Custombaselist.length; i++) {
        var index = TimeExtension.toLowerCase().indexOf("ytd") > -1 ? 1 : 0;
        var flag = timePeriodlist.filter(function (d) { return d.split('|')[1].toLowerCase() == Custombaselist[j].Name.split(' ')[index].toLowerCase() || TimeExtension.toLowerCase()=="year" }).length;
        if (flag == 0) {
            Custombaselist.splice(j, 1);
            i--;
        }
        else
            j++;
    }
    return Custombaselist;
}
function prepareCustomBase()
{
    $(".stat-content").html("");
    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }
    if (currentpage == "hdn-tbl-comparebeverages" || currentpage == "hdn-chart-comparebeverages") {
        for (var i = 0; i < ComparisonBevlist.length; i++) {
            html = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div name=\"" + ComparisonBevlist[i].Name + "\" uniqueid=\"" + ComparisonBevlist[i].UniqueId + "\" shopperdbname=\"" + ComparisonBevlist[i].Name + "\" tripsdbname=\"" + ComparisonBevlist[i].Name + "\" class=\"stat-cust-estabmt\">" + ComparisonBevlist[i].Name + "</div></div>";
            $(".stat-content").append(html);
        }
    }   
    else if (currentpage.indexOf("deepdive") > -1) {
        var Custombaselist = [];
        if (ModuleBlock == "TREND")
        {
            if (SelectedDempgraphicList.filter(function (d) { return d.isGeography == "true" }).length > 0) {
                Custombaselist = selectQuaterEndingTimePeriod(JSON.parse(JSON.stringify(TrendCustomBaselist)));
            }
            else {
                Custombaselist = TrendCustomBaselist;
            }
           
        }
           
        else
            Custombaselist = Grouplist;

        for (var i = 0; i < Custombaselist.length; i++) {
            html = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div name=\"" + Custombaselist[i].Name + "\" uniqueid=\"" + Custombaselist[i].UniqueId + "\" dbname=\"" + Custombaselist[i].DBName + "\" class=\"stat-cust-estabmt\">" + Custombaselist[i].Name + "</div></div>";
            $(".stat-content").append(html);
        }
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        for (var i = 0; i < Grouplist.length; i++) {
            html = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div name=\"" + Grouplist[i].Name + "\" uniqueid=\"" + Grouplist[i].UniqueId + "\" dbname=\"" + Grouplist[i].DBName + "\" class=\"stat-cust-estabmt\">" + Grouplist[i].Name + "</div></div>";
            $(".stat-content").append(html);
        }
    }
    else if (currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-tbl-comparesites") {
        for (var i = 0; i < Sites.length; i++) {
            html = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div name=\"" + Sites[i].Name + "\" uniqueid=\"" + Sites[i].UniqueId + "\" dbname=\"" + Sites[i].DBName + "\" class=\"stat-cust-estabmt\">" + Sites[i].Name + "</div></div>";
            $(".stat-content").append(html);
        }
    }
    
    else {
        for (var i = 0; i < Comparisonlist.length; i++) {
            html = "<div class=\"stat-custdiv\"><div class=\"stat-cust-dot\"></div><div name=\"" + Comparisonlist[i].Name + "\" uniqueid=\"" + Comparisonlist[i].UniqueId + "\" shopperdbname=\"" + Comparisonlist[i].Name + "\" tripsdbname=\"" + Comparisonlist[i].Name + "\" class=\"stat-cust-estabmt\">" + Comparisonlist[i].Name + "</div></div>";
            $(".stat-content").append(html);
        }
    }   
}
function SetCustomBase(obj)
{
    CustomBase = [];
    if (currentpage == "hdn-tbl-comparebeverages" || currentpage == "hdn-chart-comparebeverages") {
        for (var i = 0; i < ComparisonBevlist.length; i++) {
            if (ComparisonBevlist[i].Name == $(obj).attr("name")) {
                CustomBase.push(ComparisonBevlist[i]);
                break;
                return true;
            }
        }
    }
    else if (currentpage.indexOf("deepdive") > -1) {
        var Custombaselist = [];
        if (ModuleBlock == "TREND")
        {
            Custombaselist = TrendCustomBaselist;
        }
        else
            Custombaselist = Grouplist;

        for (var i = 0; i < Custombaselist.length; i++) {
            if (Custombaselist[i].Name == $(obj).attr("name")) {
                CustomBase.push(Custombaselist[i]);
                break;
                return true;
            }
        }
    }
    else if (currentpage == "hdn-crossretailer-totalrespondentstripsreport") {
        for (var i = 0; i < Grouplist.length; i++) {
            if (Grouplist[i].Name == $(obj).attr("name")) {
                CustomBase.push(Grouplist[i]);
                break;
                return true;
            }
        }
    }
    else if (currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-tbl-comparesites") {
        for (var i = 0; i < Sites.length; i++) {
            if (Sites[i].Name == $(obj).attr("name")) {
                CustomBase.push(Sites[i]);
                break;
                return true;
            }
        }
    }
    else {
        for (var i = 0; i < Comparisonlist.length; i++) {
            if (Comparisonlist[i].Name == $(obj).attr("name")) {
                CustomBase.push(Comparisonlist[i]);
                break;
                return true;
            }
        }
    }
}
function custombaseSubmit()
{
    stattest_obj = obj_CustomBase;
    Selected_StatTest = "CUSTOM BASE";
    Sigtype_Id = "1";
    SetCustomBase(obj_CustomBase);
    $(".stat-popup").hide();
    $("#Translucent").hide();
    if (currentpage == "hdn-analysis-withintrips" || currentpage.indexOf("tbl") > -1)
        $(".advance-filters").css("display", "block");
    isCustomBasePopUpVisible = true;
    prepareContentArea();
}
function SetDefaultStatTest(obj)
{
    $(".stattest").css("background-color", "transparent");
    $(".stattest").css("color", "black");
    $(obj).css("background-color", "#505050");
    $(obj).css("background-color", "#EB1F2A");
    $(obj).css("color", "white");
    stattest_obj = $(obj);
    Selected_StatTest = $(obj).children("span").html();
    Sigtype_Id = $(obj).children("span").attr("sigtype-Id");
    if ($(obj).children("span").html().toLowerCase() == "base") {
        Selected_StatTest = "";
    }   
}
function SetStatTesting(selectedview) {
    $(".LowerRightContent").hide();
    $(".stattestselection").hide();
    $(".stattestpercentage").show();
    switch (selectedview) {       
        case "hdn-dashboard-pathtopurchase":
        case "hdn-dashboard-demographic":
            {
                $("#stattest_previousyear").show();
                $("#stattest_previousperiod").show();
                $("#stattest_benchmark").show();
                $(".LowerRightContent").css("display", "block");
                SetDefaultStatTest($("#stattest_benchmark .stattest"));
                break;
            }
        case "hdn-tbl-compareretailers":
        case "hdn-chart-compareretailers":
        case "hdn-tbl-comparebeverages":
        case "hdn-chart-beveragedeepdive":
        case "hdn-chart-comparebeverages":
        case "hdn-e-commerce-tbl-comparesites":
        case "hdn-e-commerce-chart-comparesites":
            {
                if ($("#divReportGeneratorStatHide").css("background-color") == "rgb(72, 99, 112)") {
                    $("#stattest_benchmark").show();
                    $("#stattest_previousyear").hide();
                    $("#stattest_previousperiod").hide();
                    $(".LowerRightContent").css("display", "block");
                    $("#hideStatTestVsText").show();
                }
                else {
                    $("#stattest_benchmark").show();
                    $("#stattest_previousyear").show();
                    $("#stattest_previousperiod").show();
                    $(".LowerRightContent").css("display", "block");
                    SetDefaultStatTest($("#stattest_previousyear .stattest"));
                    $("#hideStatTestVsText").show();
                }
                break;
            }
        case "hdn-tbl-beveragedeepdive":
        case "hdn-chart-retailerdeepdive":
        case "hdn-tbl-retailerdeepdive":
        case "hdn-e-commerce-chart-sitedeepdive":
        case "hdn-e-commerce-tbl-sitedeepdive":
            {
                if ($("#divReportGeneratorStatHide").css("background-color") == "rgb(72, 99, 112)") {
                    $("#stattest_benchmark").show();
                    $("#stattest_previousyear").hide();
                    $("#stattest_previousperiod").hide();
                    $(".LowerRightContent").css("display", "block");
                    $("#hideStatTestVsText").show();
                }
                if (ModuleBlock == "TREND") {
                    $("#stattest_previousyear").show();
                    $("#stattest_previousperiod").show();
                    $("#stattest_totaltime").show();
                    $("#stattest_base").hide();
                    $("#stattest_benchmark").show();
                    $(".LowerRightContent").css("display", "block");
                    SetDefaultStatTest($("#stattest_totaltime .stattest"));
                    $("#hideStatTestVsText").show();
                }
                else {
                    $("#stattest_benchmark").show();
                    $("#stattest_previousyear").show();
                    $("#stattest_previousperiod").show();
                    $(".LowerRightContent").css("display", "block");
                    SetDefaultStatTest($("#stattest_previousyear .stattest"));
                    $("#hideStatTestVsText").show();

                }
                break;
            }
        case "hdn-analysis-crossretailerimageries":
            {
                $("#stattest_benchmark").show();
                $("#stattest_previousyear").show();
                $("#stattest_previousperiod").show();
                $(".LowerRightContent").css("display", "block");
                SetDefaultStatTest($("#stattest_previousyear .stattest"));
                $("#hideStatTestVsText").show();
                break;
            }
        case "Total Respondents/Trips Report":
            {
                $("#stattest_benchmark").show();
                $("#stattest_previousyear").show();
                $("#stattest_previousperiod").show();
                $(".LowerRightContent").css("display", "block");
                SetDefaultStatTest($("#stattest_previousyear .stattest"));
                $("#hideStatTestVsText").show();
                break;
            }
        case "hdn-analysis-acrossshopper":
            {
                SetDefaultStatTest($("#stattest_previousyear .stattest"));
                $("#stattest_benchmark").hide();
                $("#stattest_previousyear").show();
                $("#stattest_previousperiod").hide();
                $(".LowerRightContent").css("display", "block");
                $("#hideStatTestVsText").show();
                $(".stattestpercentage").show();
                SetDefaultStatTest($("#stattest_previousyear .stattest"));
                break;
            }
        case "hdn-crossretailer-totalrespondentstripsreport":
            {
                $("#stattest_benchmark").show();
                $("#stattest_previousyear").show();
                $("#stattest_previousperiod").show();
                $(".LowerRightContent").css("display", "block");
                SetDefaultStatTest($("#stattest_previousyear .stattest"));
                $("#hideStatTestVsText").show();
                break;
            }

        default: {
            $(".LowerRightContent").hide();
            break;
        }
    }

}
function Get_Significance_Color(significance,benchmark,samplesize)
{  
    var sigcolor = "black";
    var sigvalue=0;
    if (significance != null && significance != '' && significance != undefined)
        sigvalue = parseFloat(significance);
    if (Selected_StatTest.toLocaleLowerCase() == "custom base") {
        if (CustomBase[0].Name.toLocaleLowerCase() == benchmark.toLocaleLowerCase())
            sigcolor = "blue";
        else if (sigvalue > Stat_PositiveValue)
            sigcolor = "#20B250";
        else if (sigvalue < Stat_NegativeValue)
            sigcolor = "red";
    } else {
        if (sigvalue > Stat_PositiveValue)
            sigcolor = "#20B250";
        else if (sigvalue < Stat_NegativeValue)
            sigcolor = "red";
        else if (parseFloat(samplesize) >= 30 && parseFloat(samplesize) < 100)
            sigcolor = "gray";
    }
    return sigcolor;
}
function custom_submit()
{
    CustomBasePrev = [];
    Selected_StatTest = "CUSTOM BASE";
    Sigtype_Id = "1";
    $("#Translucent").hide();
    $(".Custombase-Retailers").hide();
    ShowSelectedFilters();
    CustomBaseFlag = 0;
    $(".Custombase-GroupType").hide();
    prepareContentArea();
}
function PreparePathToPurchaseCustombaseFilters(data) {
    $("#Custombase-RetailerDivId").html("");
    $("#Custombase-RetailerDivId").html(data.Channel.RetailersFilterlist.SearchObj.HTML_String.toString());
    $("#Custombase-RetailerDivId .Comparison").removeAttr("onclick");
    $("#Custombase-RetailerDivId .ArrowContainerdiv span").attr("onclick", "DisplayPathToPurchaseCustomBase(this);");
    $("#Custombase-RetailerDivId .Retailer .Comparison").attr("onclick", "SelectPathToPurchaseCustomBase(this);");
    $("#Custombase-RetailerDivId #ChannelOrCategoryContent .Comparison").attr("onclick", "SelectPathToPurchaseCustomBase(this);");

    //html = "<ul>";
    //$("#Custombase-RetailerDivId #ChannelOrCategoryContent ul li").each(function (i) {
    //    var curobj = $(this).children(".FilterStringContainerdiv");
    //    html += "<li>";
    //    html += "<div class=\"custombase\"><div class=\"pathtopurchase-custombase\" onclick=\"SelectPathToPurchaseCustomBase(this);\">";
    //    html += "<ul>";
    //    html += "<li>";
    //    html += $(curobj).children("span").eq(0)[0].outerHTML;
    //    html += "</li>";
    //    html += "<li>";
    //    html += $(curobj).children("span").eq(1)[0].outerHTML;
    //    html += "</li>";
    //    html += "</ul>";
    //    html += "</div>";
    //    html += $(curobj).children(".ArrowContainerdiv")[0].outerHTML;
    //    html += "</div>";
    //    html += "</li>";
    //});
    //html += "</ul>";
    //$("#Custombase-RetailerDivId #ChannelOrCategoryContent").html("");
    //$("#Custombase-RetailerDivId #ChannelOrCategoryContent").html(html);
}
function SelectPathToPurchaseCustomBase(obj) {
    var ptpcus = new Object();
    ptpcus = obj;
    if ($(ptpcus).attr("isselectable") != undefined && $(ptpcus).attr("isselectable") == "False")
        return;

    $(".Custombase-Retailers li").find(".Selected").removeClass("Selected");
    $(".Custombase-Retailers #ChannelOrCategoryContent *").find(".Selected").removeClass("Selected");
    $(".Custombase-Retailers .Retailer *").find(".Selected").removeClass("Selected");
    $(".Custombase-Retailers .Retailer .Comparison").find(".Selected").removeClass("Selected");
    $("#Custombase-RetailerDivId *").find(".Selected").removeClass("Selected");
    if ($(obj).parent("div").hasClass("FilterStringContainerdiv"))
        $(obj).parent(".FilterStringContainerdiv").parent("li").addClass("Selected");
    else
        $(obj).addClass("Selected");
    //CustomBasePrev = CustomBasePrev.length == 0 ? CustomBase : CustomBasePrev;
    CustomBase = [];
    CustomBase.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
    $("#custom-base-ratailer").html($(ptpcus).attr("name"));
    ShowSelectedFilters();
}
function DisplayPathToPurchaseCustomBase(obj) {
    $(".Custombase-Retailers .ArrowContainerdiv span").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    var sPrimaryDemo = $(obj).parent();
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw");
    $(obj).addClass("sidearrw_OnCLick");

    $(".Retailer").hide();
    $(".Retailer div").hide();
    var lavels = parseInt($(obj).attr("lavels"));
    for (var i = 0; i < lavels ; i++) {
        $(".Custombase-Retailers .Custombase-RetailerDiv .Lavel" + i).show();
        $(".Custombase-Retailers .Custombase-RetailerDiv .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".Custombase-Retailers .Custombase-RetailerDiv .Lavel" + i + " .priorityclass").hide();
        $(".Custombase-Retailers .Custombase-RetailerDiv .Lavel" + i + " div[name='" + $(obj).attr("name") + "']").show();
        $(".Custombase-Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").text($(obj).attr("name").toLowerCase());
        $(".Custombase-Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").show();

        if (i == 1)
            $(".Custombase-Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").css("width", "574px");
        else
            $(".Custombase-Retailers .AdvancedFiltersDemoHeading #retailerHeadingLevel2").css("width", "287px");

        SetScroll($(".Custombase-Retailers .Custombase-RetailerDiv .Lavel" + i), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    }

    DisplayHeightDynamicCalculation("retailer");
    if ($(obj).attr("name") == "Total") {
        sRetailer = "1";
        $(".Custombase-Retailers .Custombase-RetailerDiv .Lavel0").hide();
        $("#retailerHeadingLevel2").css("width", "287px");
    }
    else {
        $(".Custombase-Retailers .Custombase-RetailerDiv .Lavel0").show();
        sRetailer = "2";
    }
}
//

