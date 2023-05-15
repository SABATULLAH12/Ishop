/// <reference path="LeftPanel.js" />
//added by Nagaraju 
//Date: 17-04-2017
var storegeId = 3344456;//2311233435;
var adv_toggletype = "trips";
var pit_trend_toggletype = "pit";
var currentpage = "";
var TrendCustomBaselist = [];
var MaxSampleSize = 100;
var MinSampleSize = 30;
var filters = null;
var currentviewfilters = null;
var isonreadystatechange = true;
var customRegions = [];
var geo_fiter_styles = [];
//added by Nagaraju
//Date: 13-07-2017
document.onreadystatechange = function (e) {
    if (!e.bubbles && isonreadystatechange) {
        //ShowLoader();
    }
};
$(window).on('load', function (e) {
    if (!e.bubbles && isonreadystatechange) {
        //HideLoader();
    }
});
$(document).ready(function () {
    $(".alert-message-close").click(function (e) {
        $(".show-message-container").hide();
        HideLoader();
        e.stopImmediatePropagation();
    });
    $("#Translucent").click(function (e) {
        e.stopImmediatePropagation();
    });
});
function showMessage(message) {
    if (message != undefined && message != '' && typeof message === 'string') {
        $("#Translucent").css("z-index", "900000");
        $(".TranslucentDiv").show();
        $(".alert-message").html(message);
        $(".show-message-container").show();
    }
}
function IsViewFiltersExist() {
    if (localStorage.getItem("view-filters") === null)
        return null;
    else {
        filters = JSON.parse(localStorage.getItem("view-filters"));
        currentviewfilters = getCurrentViewFilters();
        if (currentviewfilters != false && currentviewfilters != undefined && currentviewfilters != null) {
            for (var j = 0; j < currentviewfilters.Filters.length; j++) {
                if (getFilter(currentviewfilters.Filters[j].Name) == null)
                    return null;
            }
        }
    }
    return true;
}
function updatefilters(data) {
    if (localStorage.getItem("view-filters") === null)
        localStorage.setItem("view-filters", JSON.stringify(data));
    else {
        filters = JSON.parse(localStorage.getItem("view-filters"));
        for (var i = 0; i < data.filters.length; i++) {
            AddOrUpdatefilter(data.filters[i][0].Name, data.filters[i]);
        }
        localStorage.clear();
        localStorage.setItem("view-filters", JSON.stringify(filters));
    }
}
function LoadFilters() {
    ShowLoader();
    //if (IsViewFiltersExist() == null) {
    var postBackData = "{viewName:'" + currentpage + "', TimePeriodType:'" + TimeExtension + "'}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLLeftPanelFilters").val(),
        async: true,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            filters = data;
            //updatefilters(data);
            if (filters != null)
                PrepareFilters(data);
            else
                LoadViewFilters();
        },
        error: function (xhr, status, error) {           
            LoadViewFilters();
        }
    });
    //}
    //else {       
    //    filters = JSON.parse(localStorage.getItem("view-filters"));
    //    PrepareFilters(filters);       
    //}
}
//added by Nagaraju D for to load view filters if server side output cache returns null
//Date: 12/07/2018
function LoadViewFilters() {
    ShowLoader();
    //if (IsViewFiltersExist() == null) {
    var postBackData = "{viewName:'" + currentpage + "', TimePeriodType:'" + TimeExtension + "'}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLViewFilters").val(),
        async: true,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            filters = data;
            //updatefilters(data);
            PrepareFilters(data);
        },
        error: function (xhr, status, error) {
        }
    });
    //}
    //else {       
    //    filters = JSON.parse(localStorage.getItem("view-filters"));
    //    PrepareFilters(filters);       
    //}
}
function GetUserDetails() {
    jQuery.ajax({
        type: "POST",
        url: $("#URLHomeFrom").val(),
        async: false,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            localStorage["Use_r_Id"] = content.UserIdAnalytics;
        },
        error: function (error) {
            alert(error);
        }
    });
}
function GoToKIHomePage(page, SSOUrl, SSOLogOut) {
    jQuery.ajax({
        type: "POST",
        url: $("#URLHomeFrom").val(),
        async: false,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (content == null || content == '') {
                if (SSOUrl == "true") {
                    window.location.href = page + "Views/Home.aspx?signout=true";
                }
                else {
                    window.location.href = page + "Login.aspx?signout=true";
                }
                return false;
            }

            $("#KIHome").remove();
            var form = $(document.createElement('form'));
            $(form).attr("id", "KIHome");
            $(form).attr("action", page + "Views/Home.aspx");
            $(form).attr("method", "POST");
            //$(form).attr("target", "target");

            var input_UserName = $("<input>")
               .attr("type", "hidden")
               .attr("name", content.UserName_Str)
               .val(content.UserName);
            $(form).append($(input_UserName));

            var input_B3 = $("<input>")
           .attr("type", "hidden")
           //.attr("name", "mQtWjUtawhO8K L9D9aPeg==")
           .attr("name", content.B3_Str)
           .val(content.B3);
            $(form).append($(input_B3));

            var input_BGM = $("<input>")
            .attr("type", "hidden")
            //.attr("name", "3pLGUhuMy2YfifTBqZdgtg==")
            .attr("name", content.BGM_Str)
            .val(content.BGM);
            $(form).append($(input_BGM));

            var input_CBL = $("<input>")
             .attr("type", "hidden")
             //.attr("name", "og1O5t72VJmcTagSmraTzw==")
             .attr("name", content.CBL_Str)
             .val(content.CBL);
            $(form).append($(input_CBL));

            var input_Groups = $("<input>")
            .attr("type", "hidden")
            //.attr("name", "L9bl70 z8Z66JbssVmkYTw==")
            .attr("name", content.Groups_Str)
            .val(content.Groups);
            $(form).append($(input_Groups));

            var input_Name = $("<input>")
            .attr("type", "hidden")
            //.attr("name", "5PhYxCnUO4LsMOQJUHA8Rw==")
            .attr("name", content.Name_Str)
            .val(content.Name);
            $(form).append($(input_Name));

            var input_Role = $("<input>")
           .attr("type", "hidden")
           //.attr("name", "Wcnup6sin10Io3eDAYsIIg==")
           .attr("name", content.Role_Str)
           .val(content.Role);
            $(form).append($(input_Role));

            var input_UserID = $("<input>")
           .attr("type", "hidden")
           //.attr("name", " CmaFa4aGgddF0vyK2Ke2g==")
           .attr("name", content.UserID_Str)
           .val(content.UserID);
            $(form).append($(input_UserID));

            var input_iSHOP = $("<input>")
           .attr("type", "hidden")
           //.attr("name", "79omsApz674 jfVC7vSFjw==")
           .attr("name", content.iSHOP_Str)
           .val(content.iSHOP);
            $(form).append($(input_iSHOP));

            //var input_Password = $("<input>")
            //.attr("type", "hidden")
            //.attr("name", content.Password_Str)
            //.val(content.Password);
            //$(form).append($(input_Password));


            var input_Bev360Drinkers = $("<input>")
           .attr("type", "hidden")
           .attr("name", content.Bev360Drinkers_Str)
           .val(content.Bev360Drinkers);
            $(form).append($(input_Bev360Drinkers));

            var input_Bev360Drinks = $("<input>")
            .attr("type", "hidden")
            .attr("name", content.Bev360Drinks_Str)
            .val(content.Bev360Drinks);
            $(form).append($(input_Bev360Drinks));

            var input_CBLV2 = $("<input>")
            .attr("type", "hidden")
            .attr("name", content.CBLV2_Str)
            .val(content.CBLV2);
            $(form).append($(input_CBLV2));

            var input_CREST = $("<input>")
            .attr("type", "hidden")
            .attr("name", content.CREST_Str)
            .val(content.CREST);
            $(form).append($(input_CREST));

            var input_DINE = $("<input>")
            .attr("type", "hidden")
            .attr("name", content.DINE_Str)
            .val(content.DINE);
            $(form).append($(input_DINE));

            var input_EmailId = $("<input>")
           .attr("type", "hidden")
           .attr("name", content.EmailId_Str)
           .val(content.EmailId);
            $(form).append($(input_EmailId));

            var input_Login_Flag = $("<input>")
           .attr("type", "hidden")
           .attr("name", content.Login_Flag_Str)
           .val(content.Login_Flag);
            $(form).append($(input_Login_Flag));

            form.appendTo(document.body);
            $(form).submit();

            $("#KIHome").remove();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function GoHome(page, userform, SSOUrl, SSOLogOut) {
    window.app.db.delete(storegeId, function () {
        if (userform == 'false') {
            window.location.href = $("#URLHome").val();
        }
        else {
            GoToKIHomePage(ReWriteHost(page), SSOUrl, SSOLogOut);
        }
    });
    //sessionStorage.clear();
    //window.location.href = $("#URLHome").val();
}
function SignOut() {
    window.app.db.delete(storegeId, function () {
        window.location.href = $("#URLSignOut").val();
    });
    //sessionStorage.clear();
    //window.location.href = $("#URLSignOut").val();
}
function ReWriteHost(_url) {
    return _url.toLowerCase().replace("{host}", window.location.hostname);
}
//build dynamic table
//added by Nagaraju 
//Date: 20-04-2017
function BuildDynamicTable() {
    if ((currentpage == "hdn-analysis-withinshopper" && seltype == "Scatter Chart") || (currentpage == "hdn-analysis-withintrips" && seltype == "Scatter Chart"))
        return;

    var comparelist = [];

    if (currentpage.indexOf("deepdive") > -1) {
        if (ModuleBlock == "TREND")
            comparelist = TimePeriod_ShortNames;
        else
            comparelist = Grouplist;
    }
    else {
        if (currentpage.indexOf("beverage") > -1)
            comparelist = ComparisonBevlist;
        else
            if (currentpage.indexOf("sites") > -1)
                comparelist = Sites;
            else
                comparelist = Comparisonlist;
    }
    var fre = "";
    if (SelectedFrequencyList.length > 0)
        fre = SelectedFrequencyList[0].Name;

    html = "";
    if (comparelist.length > 0) {
        html = "<div class=\"leftheader\" style=\"height: 58px;\">"
        html += "<div class=\"rowitem\" style=\"\">";
        html += "<ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\">";
        html += "<a class=\"table-top-title-bottom-line\"></a><span style=\"\">" + fre + "</span></li></ul></div>";
        html += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
        html += "</div>";

        html += "<div class=\"rightheader\" style=\"overflow:hidden;\">";
        var row1 = "";
        var row2 = "";
        var row3 = "";

        row1 += "<div class=\"rowitem\">";
        row1 += "<ul style=\"\">";

        row2 += "<div class=\"rowitem\">";
        row2 += "<ul style=\"\">";
        for (var i = 0; i < comparelist.length; i++) {
            row1 += "<li class=\"benchmarkheader\" style=\"overflow: hidden;text-align: center;\">";
            if (ModuleBlock == "TREND")
                row1 += "<span title=\"" + comparelist[i].replace("48MMT", "").replace("36MMT", "").replace("30MMT", "").replace("24MMT", "").replace("18MMT", "").replace("3MMT", "").replace("6MMT", "").replace("12MMT", "") + "\">" + comparelist[i].replace("48MMT", "").replace("36MMT", "").replace("30MMT", "").replace("24MMT", "").replace("18MMT", "").replace("3MMT", "").replace("6MMT", "").replace("12MMT", "") + "</span></li>";
            else
                row1 += "<span title=\"" + comparelist[i].Name + "\">" + comparelist[i].Name + "</span></li>";

            row2 += "<li class=\"benchmarkheader\" style=\"overflow: hidden;text-align: center;\">";
            row2 += "<span></span></li>";
        }
        row1 += "</ul></div>";
        row2 += "</ul></div>";


        html += row1;
        html += row2;
        html += "</div>";
    }
    $("#Table-Content").html(html);
    SetScroll($("#Table-Content .rightheader"), "#393939", 0, -8, 0, -8, 8);
    SetStyles();
}
function SelectStatValue() {
    $(".StatArea").hide();
    $("#GreenValue").html(StatPercent + "%");
    $("#RedValue").html(StatPercent + "%");

    Stat_PositiveValue = PositiveValue;
    Stat_NegativeValue = NegativeValue;

    postBackData = "{PosiValue:'" + PositiveValue + "', NegaValue:'" + NegativeValue + "', Percent:'" + StatPercent + "'}";

    jQuery.ajax({
        type: "POST",
        url: $("#URLCommonStatTest").val(),
        async: true,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {

        },
        error: function (error) {
            showMessage(error.responseText);
        }
    });
}
function GetStatTestValue() {
    $(".StatTestValue").removeClass("selected-stattest");
    jQuery.ajax({
        type: "POST",
        url: $("#URLCommonGetStatTest").val(),
        async: true,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            $("#GreenValue").html(content.Percent + "%");
            $("#RedValue").html(content.Percent + "%");
            $("#stat" + content.Percent).addClass("selected-stattest");

            Stat_PositiveValue = content.PosiValue;
            Stat_NegativeValue = content.NegaValue;
            GetStatValue(content.PosiValue, content.NegaValue, content.Percent);
        },
        error: function (error) {
            showMessage(error.responseText);
        }
    });
}
function GetStatValue(posivalue, negavalue, percent) {
    StatPercent = percent;
    PositiveValue = posivalue;
    NegativeValue = negavalue;

    //$(".StatTestValue").live('click', function () {
    //    $(".StatTestValue").each(function () {
    //        $(this).css("background-color", "transparent");
    //    });
    //    $(this).css("background-color", "#FFCC00");
    //});


}
//added by Nagaraju for preventing background click
//date: 27-04-2017
function Set_zIndex() {
    $("#Translucent").css("z-index", "9000");
}
function Remove_zIndex() {
    $("#Translucent").css("z-index", "1001");
}
$(document).ready(function () {
    GetUserDetails();
    $(document).on("click", ".adv-fltr-visit, .adv-fltr-guest", function () {
        adv_toggletype = $(this).attr("TabType");
        if (adv_toggletype != TabType) {
            $(".toggle-slider").trigger("click");
        }
        UpdateVisitGuestFilters();
        prepareContentArea();
    });
    $(document).on("click", ".lft-ctrl-toggle-text", function () {
        if (pit_trend_toggletype.toLocaleLowerCase() != $(this).attr("name").toLocaleLowerCase()) {
            if (currentpage.indexOf('dashboard') > -1) {
                if ($(this).attr("id") == "lft-fltr-pitDashTrip" || $(this).attr("id") == "lft-fltr-trendDashShopper") {
                    $("#pathtopurchase-size-skew-toggleTrip").trigger("click");
                }
                else if ($(this).attr("id") == "lft-fltr-trendDash" || $(this).attr("id") == "lft-fltr-pitDash") {
                    $("#pathtopurchase-size-skew-toggle").trigger("click");
                }
            }
            else {
                $("#pit-toggle").trigger("click");
            }
        }
    });
});

function LogSelection() {
    var modulename = "";
    if (currentpage.indexOf("hdn-chart") > -1) {
        modulename = "CHARTS";
    }
    else if (currentpage.indexOf("hdn-tbl") > -1) {
        modulename = "TABLES";
    }
    else if (currentpage.indexOf("hdn-report") > -1) {
        modulename = "REPORTS";
    }
    else if (currentpage.indexOf("hdn-analysis") > -1) {
        modulename = "ADD’L CAPABILITIES";
    }

    if (modulename != "") {
        postBackData = "{Module:'" + modulename + "'}";
        jQuery.ajax({
            type: "POST",
            url: $("#URLCommon_LogSelection").val(),
            async: true,
            data: postBackData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (content) {
                if (!content)
                    SignOut();
            },
            error: function (error) {
                showMessage(error.responseText);
            }
        });
    }
}
function isAuthenticated(data) {
    var userauthenticated = true;
    if (data.result != undefined && data.result.toLocaleLowerCase() == "redirect") {
        userauthenticated = false;
        SignOut();
    }
    return userauthenticated;
}
function GoToDashboard(page) {
    if (page == "demographic") {
        window.location.href = $("#URLDemographic").val();
    }
    else if (page == "brandhealth") {
        window.location.href = $("#URLBrandhealth").val();
    }
    else if (page == "visits") {
        window.location.href = $("#URLVisits").val();
    }
    else if (page == "pathtopurchase") {
        window.location.href = $("#URLPathToPurchase").val();
    }
}
function GoToErrorPage() {
    window.location.href = $("#URLError").val();
}
//-Abhay
function filterChannelBasedOnView() {
    switch (currentpage) {
        case "hdn-analysis-crossretailerimageries":
            $('#RetailerDivId').find('div.level1 ul').find('li').find('span[name="Corporate Nets"]').parents('li').eq(0).hide();
            $('#RetailerDivId').find('div.level1 ul').find('li').find('span[name="Total"]').parents('li').eq(0).hide();

            break;
        case "hdn-analysis-acrossshopper":
            $('#RetailerDivId').find('div.level1 ul').find('li').find('span[name="Total"]').parents('li').eq(0).hide();
            break;
    }
}

