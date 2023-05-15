/// <reference path="ECommerce.js" />
/// <reference path="LeftPanel.js" />
var sCurrentAdvancedFilters = [];
var sNonSelecteble = [];
var AllBevFrequency = [];
$(document).ready(function () {
    //LoadRightFilter();
    //if (sFilterData == null || sFilterData == "" || sFilterData.length <= 0 || sFilterData == {} || sFilterData.Measure == undefined)
    //    LoadFilterData();
    //else
    //{
    //    LoadRightPanel(sFilterData);
    //    if (currentpage.indexOf("beverage") > 0) {
    //        LoadMonthlyFilters(sFilterData);
    //    }
    //    else {
    //        LoadFrequencyFilter(sFilterData);
    //    }

    //    LoadChannelFilters(sFilterData);
    //}#pit-tabs-block > div.adv-fltr.guests > div > div > span

    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }
    //if (currentpage == "hdn-chart-comparebeverages" || currentpage == "hdn-chart-compareretailers" || currentpage == "hdn-chart-retailerdeepdive" || currentpage == "hdn-chart-beveragedeepdive" || currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
    //    $(".adv-fltr-selection .adv-fltr-toggle-container").css("display", "none");
    //}

    $(document).on("click", ".rgt-cntrl-SubFilter-Conatianer", function (e) {
        e.stopImmediatePropagation();
    });
    $(document).on("click", ".adv-fltr-showhide", function (e) {
        ClosePopups();
        if ($(".adv-fltr-showhide-txt").text() == "SHOW LESS") {
            $("#Table-Content").css("height", "calc(100% - 134px)");
            $(".adv-fltr-showhide-txt").text("SHOW MORE");
            $(".adv-fltr-selection").hide();
            $(".adv-fltr-headers").css("height", "30.0313px");
            //$(".adv-fltr-showhide").css("margin-top", "-23px");
            $(".adv-fltr-details").css("height", "30px");

            //$(".adv-fltr-top").css("height", "30%");
            //$(".advance-filters").css("height", "20%");            
            $(".table-bottomlayer").css("height", "71%");
            $(".adv-fltr-showhide-img").css("background-position", "-193px -212px");
            //$(".adv-fltr-showhide-sectn").css("height", "16.7px");
            $(".adv-fltr-applyfiltr").css("visibility", "hidden");
            $(".showChartMain").css("height", "96%");
            $(".ChartDivArea").css("height", "436px");
            $("#spChartLegend").css(" margin-left", "0").css("padding-top", "17px").css("margin-top", "32px").css("margin-bottom", "7px");
            if (currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
                $(".ChartAreaNew").css("height", "77%");
                $("#Table-Content").css("height", "98%");
            }
        }
        else {
            $(".adv-fltr-showhide").css("margin-top", "0");
            $(".adv-fltr-showhide-txt").text("SHOW LESS");
            //$(".adv-fltr-top").css("height", "20%");
            //$(".advance-filters").css("height", "30%");          

            $(".adv-fltr-details").css("height", "75px");
            $(".table-bottomlayer").css("height", "55%");
            $(".adv-fltr-selection").show();
            $(".adv-fltr-headers").css("height", "29.1563px");
            $(".adv-fltr-showhide-img").css("background-position", "-231px -212px");
            //$(".adv-fltr-showhide-sectn").css("height", "18%");
            $(".adv-fltr-selection").show();

            //if (currentpage == "hdn-e-commerce-tbl-comparesites" || currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") {
            //    if ($(".adv-fltr-suboptions-list-container").is(":visible") && $(".adv-fltr-suboptions-list-container ul li:visible").length > 3) {
            //        $(".adv-filters-wraper").css("left", "1%");
            //    }
            //    else {
            //        $(".adv-filters-wraper").css("left", "27%");
            //    }
            //}
            //else if (sVisitsOrGuests == 2) {
            //    $(".adv-filters-wraper").css("left", "28%");
            //}
            //else {
            //    $(".adv-filters-wraper").css("left", "1%");
            //}

            $(".adv-fltr-applyfiltr").css("visibility", "visible");
            $("#Table-Content").css("height", "calc(100% - 197px)");
            $(".showChartMain").css("height", "74%");
            $(".ChartDivArea").css("height", "320px");
            $("#spChartLegend").css("margin", "0 auto").css("margin-left", "0").css("margin-top", "-56px");
            if (currentpage == "hdn-analysis-withinshopper" || currentpage == "hdn-analysis-withintrips") {
                $(".ChartAreaNew").css("height", "57%");
                $("#Table-Content").css("height", "101%");
            }
            if (!($(".adv-fltr-toggle-container").is(":visible")))
                $(".toggle-seperator").hide();
            else
                $(".toggle-seperator").show();
            if ($("#adv-fltr-Chnl").is(":visible") || $("#adv-bevselectiontype-freq").is(":visible"))
                $(".advancedfilter-seperator").show();
            else
                $(".advancedfilter-seperator").hide();
        }
        if (currentpage.indexOf("chart") > -1) {
            if (ChartType.toLowerCase() == "table") {
                prepareContentArea();
            }
            else {
                data = ChartModuleData;
                $("#chart-list").css("background-image", " url(../../Images/Coke Dine_Sprite_7.svg)");
                $("#chart-list").css("background-position", " 548px -457px");
                $(".ChartDivArea").hide();
                $(".showChartMain").show();
                $(".ChartDivArea").getNiceScroll().resize().hide();
                var sImageClassName = "";
                $.each($("#chart-list div"), function (i, v) {
                    if (ChartType.toString() == v.attributes[1].value)
                        sImageClassName = ChartImageNew_Position(v.attributes[1].value);
                    else
                        sImageClassName = ChartImagePosition(v.attributes[1].value);
                    $("#chart-list div[chart-name='" + v.attributes[1].value + "']").css('background-position', sImageClassName);
                });
                $(".ChartDivArea").html("");
                if (ChartType.toLocaleLowerCase() == "clustered column") {
                    //$("#chart-list div[chart-name='" + ChartType + "']").css('background-position', sImageClassName);
                    identifier = 1;
                    //GoToChartFunctionForGroupChartTwo(data, identifier);
                    GoToChartFunctionForColumnChartOne(data, identifier);
                }

                else if (ChartType.toLocaleLowerCase() == "clustered bar") {
                    //$("#chart-list div[chart-name='" + ChartType.toLocaleLowerCase() + "']").css('background-position', '-78px -408px');
                    identifier = 1;
                    //GoToChartFunctionForColumnChartOne_NewForClusteredBar(data, identifier);
                    GoToChartFunctionForColumnChartOne_NewForStackedBar(data, identifier);
                }

                else if (ChartType.toLocaleLowerCase() == "bar with change") {
                    //$("#chart-list div[chart-name='" + ChartType.toLocaleLowerCase() + "']").css('background-position', '-776px -408px');
                    identifier = 1;
                    GoToChartFunctionForColumnChartOne_NewForBar_with_change(data, identifier);
                }

                else if (ChartType.toLocaleLowerCase() == "stacked column") {

                    //$("#chart-list div[chart-name='" + ChartType + "']").css('background-position', '-868px -405px');
                    identifier = 0;
                    columnChart_Stacked(data, identifier);
                }
                else if (ChartType.toLocaleLowerCase() == "stacked bar") {
                    //$("#chart-list div[chart-name='" + ChartType.toLocaleLowerCase() + "']").css('background-position', '-956px -405px');
                    identifier = 0;
                    var MoreLessFact = 4;
                    if ($(".adv-fltr-showhide-txt").text() == "SHOW MORE") {
                        MoreLessFact = 6;
                    }
                    BarChart_Stacked(data, identifier, MoreLessFact);
                }
                else if (ChartType.toLocaleLowerCase() == "line") {
                    //$("#chart-list div[chart-name='" + ChartType.toLocaleLowerCase() + "']").css('background-position', '-275px -407px');
                    identifier = 0;
                    //GoToChartFunctionForLineChartTwo(data, identifier);
                    GoToChartFunctionForLineChartOne(data, identifier);

                }
                else if (ChartType.toLocaleLowerCase() == "pyramid") {
                    if ($(".adv-fltr-showhide-txt").text() == "SHOW MORE")
                        $(".showChartMain").css("height", "96%");
                    else
                        $(".showChartMain").css("height", "85%");
                    //$("#chart-list div[chart-name='" + ChartType.toLocaleLowerCase() + "']").css('background-position', '-375px -408px');
                    identifier = 0;
                    //GoToChartFunctionForLineChartTwo(data, identifier);
                    Plot_Pyramid_Chart(data, identifier);

                }
                else if (ChartType.toLocaleLowerCase() == "pyramid with change") {
                    if ($(".adv-fltr-showhide-txt").text() == "SHOW MORE")
                        $(".showChartMain").css("height", "96%");
                    else
                        $(".showChartMain").css("height", "85%");
                    //$("#chart-list div[chart-name='" + ChartType.toLocaleLowerCase() + "']").css('background-position', '-375px -408px');
                    identifier = 0;
                    //GoToChartFunctionForLineChartTwo(data, identifier);
                    Plot_Pyramid_Chart_With_Change(data, identifier);

                }
                if (Measurelist.length > 0)
                    $("#chart-title").html(Measurelist[0].parentName.toUpperCase());

                $(".showChartMain").getNiceScroll().resize().hide();
                SetScroll($(".showChartMain"), "#393939", 0, 0, 0, 0, 8);
            }
        }
        if ($(".adv-fltr-sub-selection > div:visible").length >= 2 && $(".adv-fltr-freq-container").is(":visible") && $("#adv-bevselectiontype-freq").is(":visible")) {
            $(".freq-seperator").css("display", "none");
            $(".advanced-seperator").css("background-position", "2px 0px");
            $(".advanced-seperator").show();
        }
        SetScroll($("#Table-Content .rightbody"), "#393939", 0, -8, 0, -8, 8);
        e.stopImmediatePropagation();
    });
    $(document).on("click", ".adv-fltr-details", function (e) {
        if ((!$('.adv-fltr-suboptions-list-container').is(e.target) && !$('.adv-fltr-suboptions-list-container').is(e.target.parentNode) && !$('.adv-fltr-suboptions-list-container').is(e.target.parentNode.parentNode))
                && (!$('.adv-fltr-toggle').is(e.target) && !$('.adv-fltr-toggle').is(e.target.parentNode) && !$('.adv-fltr-toggle').is(e.target.parentNode.parentNode))) {
            $(".adv-fltr-showhide").trigger("click");
        }
    });

    $(document).on("click", ".toggle-slider", function (e) {
        adv_toggletype = TabType;
        $(".DemoLevel2").hide();
        $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").hide();
        $(".rgt-cntrl-frequency-Conatiner-SubLevel1").hide();
        $(".rgt-cntrl-frequency-Conatiner-SubLevel2").hide();
        $(".toggle-seperator").css("display", "block");
        $(".freq-seperator").css("display", "block");
        $(".advancedfilter-seperator").css("display", "none");
        if ($('#guest-visit-toggle').hasClass('active')) {
            $('#guest-visit-toggle').removeClass('active');
            $(".adv-fltr-guest").css("color", "#000");
            $(".adv-fltr-visit").css("color", "#f00");
            //if (currentpage == "hdn-e-commerce-tbl-comparesites" || currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") {
            //    if ($(".adv-fltr-suboptions-list-container").is(":visible") && $(".adv-fltr-suboptions-list-container ul li:visible").length > 3) {
            //        $(".adv-filters-wraper").css("left", "1%");
            //    }
            //    else {
            //        $(".adv-filters-wraper").css("left", "27%");
            //    }
            //}
            //else {
            //    $(".adv-filters-wraper").css("left", "1%");
            //}

            sVisitsOrGuests = "1";
            //$("#adv-fltr-freq").css("display", "none");
            if (currentpage.indexOf("beverage") > 0) {
                $("#adv-fltr-Chnl").css("display", "block");
                $(".toggle-seperator").css("display", "none");
                $(".freq-seperator").css("display", "none");
            }
        } else {
            $('#guest-visit-toggle').addClass('active');
            $(".adv-fltr-visit").css("color", "#000");
            $(".adv-fltr-guest").css("color", "#f00");
            //$(".adv-filters-wraper").css("left", "28%")

            sVisitsOrGuests = "2";
            $("#adv-fltr-freq").css("display", "block");
            if (currentpage.indexOf("beverage") > 0)
                $("#adv-fltr-Chnl").css("display", "none");
        }
        //LoadGroupTypeHeaderName(sFilterData);
        //LoadGroupTypeNames(sFilterData);
        //LoadMeasureTypeMain(sFilterData);
        //LoadMeasureTypeHeaderName(sFilterData);
        //LoadMeasureTypeNames(sFilterData);
        if (currentpage.indexOf("hdn-chart") > -1) {
            //LoadMeasure(sFilterData);
        }
        e.stopImmediatePropagation();
    });
    $(document).on("click", "#adv-fltr-freq", function (e) {
        if (isPopupVisible)
            return false;

        ClosePopups();
        $(".rgt-cntrl-frequency").css("display", "block");
        $(".rgt-cntrl-frequency").show();
        //$(".rgt-cntrl-frequency").css("width", "240px");
        //$(".rgt-cntrl-frequency-Conatiner-SubLevel1").hide();
        //$(".rgt-cntrl-frequency-Conatiner-SubLevel2").hide();
        var offset = $(".adv-fltr-freq-container").offset();
        var height = $(".adv-fltr-freq-container").height();
        var width = $(".adv-fltr-freq-container").innerWidth();
        var top = offset.top + height;
        var offset1 = $(".adv-filters-wraper").offset();
        var height_wraper = $(".adv-filters-wraper").height();
        //var right = offset.left + width + 2 + "px";

        $('.rgt-cntrl-frequency').css({
            'position': 'absolute',
            'left': offset.left - offset1.left,
            'top': (height_wraper + 1),//($(this).height() + 1),//$(this).offset().top +
        });
        SetScroll($(".rgt-cntrl-frequency-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        $(".rgt-cntrl-SubFilter-Conatianer").hide();
        $(".adv-fltr-selection-container").removeClass("TileActive");
        $(".adv-fltr-suboptions-list-container *").removeClass("TileActive");
        $(".MonPurFreq").addClass("TileActive");
        //if (currentpage.indexOf("beverage") > 0)
        //    SearchFilters_RightPanel("Monthly", "Search-FrequencyFilters", "FreqFilter-Search-Content", AllMonthly);
        //else {
        //    SearchFilters_RightPanel("Frequency", "Search-FrequencyFilters", "FreqFilter-Search-Content", AllFrequency);
        //}
        AllFrequency = [];
        AllFrequency = getSearchItemsFromElement("#shopper_frequency_containerId");
        PrepareSearch("Frequency", "Search-FrequencyFilters", "FreqFilter-Search-Content", AllFrequency);

        e.stopImmediatePropagation();
    });
    $(document).on("click", "#adv-fltr-trips-freq", function (e) {
        if (isPopupVisible)
            return false;

        ClosePopups();
        $(".rgt-cntrl-trips-frequency").css("display", "block");
        $(".rgt-cntrl-trips-frequency").show();
        //$(".rgt-cntrl-frequency").css("width", "240px");
        //$(".rgt-cntrl-frequency-Conatiner-SubLevel1").hide();
        //$(".rgt-cntrl-frequency-Conatiner-SubLevel2").hide();
        var offset = $("#adv-fltr-trips-freq").offset();
        var height = $("#adv-fltr-trips-freq").height();
        var width = $("#adv-fltr-trips-freq").innerWidth();
        var top = offset.top + height;
        //var right = offset.left + width + 2 + "px";
        var offset1 = $(".adv-filters-wraper").offset();
        var height_wraper = $(".adv-filters-wraper").height();

        $('.rgt-cntrl-trips-frequency').css({
            'position': 'absolute',
            'left': offset.left - offset1.left,
            'top': (height_wraper + 1),//($(this).height() + 1),//$(this).offset().top + 
        });
        SetScroll($(".rgt-cntrl-trips-frequency-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        $(".rgt-cntrl-SubFilter-Conatianer").hide();
        $(".adv-fltr-selection-container").removeClass("TileActive");
        $(".adv-fltr-suboptions-list-container *").removeClass("TileActive");
        $(".MonPurFreq").addClass("TileActive");
        //if (currentpage.indexOf("beverage") > 0)
        //    SearchFilters_RightPanel("Monthly", "Search-trips-FrequencyFilters", "FreqFilter-trips-Search-Content", AllMonthly);
        //else {
        //    SearchFilters_RightPanel("Trips-Frequency", "Search-trips-FrequencyFilters", "FreqFilter-trips-Search-Content", AllFrequency);
        //}
        AllFrequency = [];
        AllFrequency = getSearchItemsFromElement("#shopper_frequency_containerId");
        PrepareSearch("Trips-Frequency", "Search-trips-FrequencyFilters", "FreqFilter-trips-Search-Content", AllFrequency);
        e.stopImmediatePropagation();
    });
    $(document).on("click", "#adv-fltr-ordertype", function (e) {
        if (isPopupVisible)
            return false;

        ClosePopups();
        $(".rgt-cntrl-ordertype").css("display", "block");
        $(".rgt-cntrl-ordertype").show();  
        var offset = $(".adv-fltr-ordertype-container").offset();
        var height = $(".adv-fltr-ordertype-container").height();
        var width = $(".adv-fltr-ordertype-container").innerWidth();
        var top = offset.top + height;
        var offset1 = $(".adv-filters-wraper").offset();
        var height_wraper = $(".adv-filters-wraper").height();

        //var right = offset.left + width + 2 + "px";

        $('.rgt-cntrl-ordertype').css({
            'position': 'absolute',
            'left': offset.left - offset1.left,
            'top': (height_wraper + 1),//($(this).height() + 1),// $(this).offset().top +
        });
        SetScroll($(".rgt-cntrl-ordertype-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        $(".rgt-cntrl-SubFilter-Conatianer").hide();
        $(".adv-fltr-selection-container").removeClass("TileActive");
        $(".adv-fltr-suboptions-list-container *").removeClass("TileActive");
        $(".adv-fltr-ordertype-container").addClass("TileActive");
        $(".MonPurordertype").addClass("TileActive");
        //SearchFilters_RightPanel("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);

        TripsFrequency = [];
        TripsFrequency = getSearchItemsFromElement("#visit_frequency_containerId");
        PrepareSearch("ordertype", "Search-ordertypeFilters", "ordertypeFilter-Search-Content", TripsFrequency);
        e.stopImmediatePropagation();
    });
    $(document).on("click", "#adv-fltr-Chnl", function (e) {
        if (isPopupVisible)
            return false;

        ClosePopups();
        //$(".rgt-cntrl-chnl").css("width", "240px");
        var offset = $(".adv-fltr-Chnl-container").offset();
        var height = $(".adv-fltr-Chnl-container").height();
        var width = $(".adv-fltr-Chnl-container").width();
        var top = offset.top + height + "px";
        var right = offset.left + width + 2 + "px";
        var height_wraper = $(".adv-filters-wraper").height();

        $('.rgt-cntrl-chnl').css({
            'position': 'absolute',
            'left': offset.left - ($(".rgt-cntrl-chnl").width() - $(this).width()),
            'top':  (height_wraper + 1),//$(this).offset().top + ($(this).height() - 6),
        });
        $(".rgt-cntrl-chnl").css("display", "block");
        $(".rgt-cntrl-chnl").show();
        $(".adv-fltr-Chnl-container").addClass("TileActive");
        PrepareSearch("Channel", "Search-Channel", "channel-Search-Content", allChannels);
        $("#channel-Search-Content ul").css("text-align", "left");
        $("#channel-Search-Content ul").css("height", "auto");
        e.stopImmediatePropagation();
    });
    $(document).on("click", "#adv-bevselectiontype-freq", function (e) {
        ClosePopups();

        $(".rgt-cntrl-Selection").css("display", "block");
        $(".rgt-cntrl-Selection").show();
        var offset = $(".adv-fltr-selection-container").offset();
        var height = $(".adv-fltr-selection-container").height();
        var width = $(".adv-fltr-selection-container").width();
        var top = offset.top + height + "px";
        var right = offset.left + width + 2 + "px";
        var offset1 = $(".adv-filters-wraper").offset();
        var height_wraper = $(".adv-filters-wraper").height();

        var dd_width = $(".rgt-cntrl-Selection.beverageItems").outerWidth();
        if (currentpage.indexOf("chart") > -1)
            var tableContent_Width = $('#ToShowChart').outerWidth();
        else
            var tableContent_Width = $('#Table-Content').outerWidth();

        $(".adv-fltr-freq-container").removeClass("TileActive");
        $(".adv-fltr-suboptions-list-container *").removeClass("TileActive");
        $(".beverageChannel").addClass("TileActive");
        if ($("#hdn-page").length > 0 && $("#hdn-page").attr("name").toLocaleLowerCase().indexOf("beverage") > -1) {
            $('.rgt-cntrl-Selection').css({
                'position': 'absolute',
                //'left': offset.left - ($(".rgt-cntrl-Selection").width() - $(this).width()),
                'left': offset.left- offset1.left,
                'top':  (height_wraper + 1),//($(this).height() + 1),//$(this).offset().top + 
            });
        }
        else if (tableContent_Width != null && tableContent_Width < (offset.left + dd_width)) {
            $('.rgt-cntrl-Selection').css({
                'position': 'absolute',
                'left': offset.left - offset1.left + width - dd_width,//left - offset1.left - ((left - offset1.left + dd_width) - tableContent_Width)
                'top': (height_wraper + 1),//($(obj).height() - 3),//$(obj).offset().top
            });
        }
        else {
            $('.rgt-cntrl-Selection').css({
                'position': 'absolute',
                'left': offset.left - offset1.left,
                'top':  (height_wraper + 1),//($(this).height() + 1),//$(this).offset().top + 
            });
        }
        SetScroll($(".rgt-cntrl-Selection-Conatiner"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        //SearchFilters_RightPanel("AllBevFrequency", "Search-BEVERAGEFrequencyFilters", "BEVERAGEFreqFilter-Search-Content", AllBevFrequency);
        AllBevFrequency = [];
        AllBevFrequency = getSearchItemsFromElement("#beverage-frequency");
        PrepareSearch("AllBevFrequency", "Search-BEVERAGEFrequencyFilters", "BEVERAGEFreqFilter-Search-Content", AllBevFrequency);

        //$(".rgt-cntrl-Selection-Conatiner").addClass("TileActive");
        e.stopImmediatePropagation();
    });
    $(document).on("click", ".adv-fltr-suboptions-list-container .MainSelection", function (e) {
        ClosePopups();
        sCurrentAdvancedFilters = [];
        DisplayVisistSecondaryDemoFilter($(this));
        var searchItems = getAdditionalFiltersSearchItems($(this), '#VisistsFilterDivId');
        PrepareSearch("AdvancedFilters", "Search-VisitsAdvancedFilters", "VisitsAdvancedFilter-Search-Content", searchItems);
        $("#VisitsAdvancedFilter-Search-Content ul").css("text-align", "left");
        $("#VisitsAdvancedFilter-Search-Content ul").css("height", "auto");
        e.stopImmediatePropagation();
    });
});

function LoadFilterData() { 
    //jQuery.ajax({
    //    type: "POST",
    //    url: $("#URLCommon").val(),
    //    async: true,
    //    data: "",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        sFilterData = data;         
    //        LoadRightPanel(data);
    //        LoadFrequencyFilter(data);           
    //        LoadChannelFilters(data);
            
    //    },
    //    error: function (xhr, status, error) {
    //        showMessage(error);
    //    }
    //});
}
//Load Right Panel
function LoadRightFilter() {  
    if ($("#hdn-page").length > 0) {
      //  $("#FilterHeader").show();
        $("#RightPanelPartial").empty();
        $("#RightPanelPartial").append("<div class=\"adv-fltr-details\" style=\"height: 30px;\"></div>");
        $(".adv-fltr-details").append("<div class=\"adv-fltr-headers\"><div class=\"adv-fltr-redband\"></div><div class=\"adv-fltr-title\">ADDITIONAL FILTER OPTIONS</div><div class=\"adv-fltr-showhide-sectn\" style=\"height: 30%;\"></div></div>");
        $(".adv-fltr-details").append("<div class=\"adv-fltr-selection\" style=\"display: none;\"></div>");
        //In rifgt pannel show more and apply filter buttons
        //$(".adv-fltr-details").append("<div class=\"adv-fltr-showhide-sectn\" style=\"height: 30%;\"></div>");
        $(".adv-fltr-showhide-sectn").append("<div class=\"adv-emptydiv\"></div>");
        $(".adv-fltr-showhide-sectn").append("<div class=\"adv-fltr-applyfiltr\" style=\"visibility: hidden;\"></div>");
        $(".adv-fltr-applyfiltr").append("<div class=\"adv-fltr-applyfiltr-txt\">APPLY FILTER</div><div class=\"adv-fltr-applyfiltr-img\"></div>");
        $(".adv-fltr-showhide-sectn").append("<div class=\"adv-fltr-showhide\"></div>");
        $(".adv-fltr-showhide").append("<div class=\"adv-fltr-showhide-txt\">SHOW MORE</div>");
        $(".adv-fltr-showhide").append("<div class=\"adv-fltr-showhide-img\" style=\"background-position: -193px -212px;\"></div>");
        //In rifgt pannel show more and apply filter buttons End
    }
}
function LoadRightPanel(data) {
    $(".adv-fltr-selection").append("<div class=\"adv-filters-wraper\"></div>");
    $(".adv-filters-wraper").append("<div class=\"adv-fltr-toggle-container shoppertrip-Toggle classMouseHover dynpos1\" style=\"width:248px;\"> <div class=\"adv-fltr-toggle\"><div TabType=\"trips\" class=\"adv-fltr-visit\" style=\"width:124px;\">ONLINE PURCHASE</div> <label class=\"switch\"> <input type=\"checkbox\" id=\"guest-visit-toggle\"><div class=\"toggle-slider round\"></div> </label><div TabType=\"shopper\" class=\"adv-fltr-guest\">SHOPPERS</div></div></div>");
    $(".adv-filters-wraper").append("<div class=\"filter-separator toggle-seperator\"></div>");
    $(".adv-filters-wraper").append("<div class=\"adv-fltr-sub-selection\"></div>");

    $(".adv-fltr-sub-selection").append("<div style=\"\" id=\"adv-fltr-freq\" class=\"adv-fltr-freq-container shopperDiv\"></div>");
    $(".adv-fltr-sub-selection").append("<div class=\"filter-separator freq-seperator\"></div>");
    $("#adv-fltr-freq").append("<div class=\"lft-cntrl adv-fltr-freq Frequency MonPurFreq\"></div>");
    $("#adv-fltr-freq").append("<div class=\"rgt-cntrl-frequency\"></div>");

    //added by Nagaraju for trips add frequency
    //Date: 09-08-2017
    $(".adv-fltr-sub-selection").append("<div style=\"\" id=\"adv-fltr-trips-freq\" class=\"adv-fltr-freq-container tripsDiv\"></div>");
    $(".adv-fltr-sub-selection").append("<div class=\"filter-separator freq-seperator\"></div>");
    $("#adv-fltr-trips-freq").append("<div class=\"lft-cntrl adv-fltr-freq Frequency MonPurFreq\"></div>");
    $("#adv-fltr-trips-freq").append("<div class=\"rgt-cntrl-trips-frequency\"></div>");
    //

    //Trips frequency
    $(".adv-fltr-sub-selection").append("<div style=\"\" id=\"adv-fltr-ordertype\" class=\"adv-fltr-ordertype-container tripsDiv\"></div>");
    $(".adv-fltr-sub-selection").append("<div class=\"filter-separator advanced-seperator\"></div>");
    $("#adv-fltr-ordertype").append("<div class=\"lft-cntrl adv-fltr-ordertype ordertype MonPurordertype\"></div>");
    $("#adv-fltr-ordertype").append("<div class=\"rgt-cntrl-ordertype\"></div>");

    var htmlCommon = "";
    htmlCommon += "<div id=\"FreqFilter-Search-Content\" class=\"Search-Filter\" style=\"margin-top: 2%;width:269px;\"><div class=\"Search\"><input type=\"text\" id=\"Search-FrequencyFilters\" class=\"txt-search ui-autocomplete-input\" name=\"Channel-Retailer-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"><div class=\"img-search\"></div></div></div>";
    if (currentpage.indexOf("beverage") > 0) {
        //htmlCommon += "<div class=\"VisistsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\">MONTHLY PURCHASE</div>";
        htmlCommon += "<div class=\"VisistsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\"><div id=\"freqFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:100%;float:left;\">Monthly Purchase</div><div id=\"freqFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Monthly Purchase</div><div id=\"freqFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Monthly Purchase</div></div>";
    }
    else {
        //htmlCommon += "<div class=\"VisistsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\">FREQUENCY</div>";
        htmlCommon += "<div class=\"VisistsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\"><div id=\"freqFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:100%;float:left;\">Online Order Frequency</div><div id=\"freqFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Frequency</div><div id=\"freqFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Online Order Frequency</div></div>";
    }
    
    
    $(".rgt-cntrl-frequency").append(htmlCommon);
    $(".rgt-cntrl-frequency").append("<div id=\"shopper_frequency_containerId\" class=\"frequency_container\"></div>");
    $(".frequency_container").append("<div class=\"rgt-cntrl-frequency-Conatiner DemoLevel Lavel Lavel1\" style=\"height:98%;float:left;left: -1%;\"></div><div class=\"rgt-cntrl-frequency-Conatiner-SubLevel1 DemoLevel2 Lavel Lavel2\" style=\"display:none;float:left;height:98%; left: 285px;\"></div><div class=\"rgt-cntrl-frequency-Conatiner-SubLevel2 DemoLevel2 Lavel Lavel2\" style=\"display:none;float:left;height:98%; left: 570px;\"></div>");
    
    //trips fre
    htmlCommon = "<div id=\"FreqFilter-trips-Search-Content\" class=\"Search-Filter\" style=\"margin-top: 2%;width:269px;\"><div class=\"Search\"><input type=\"text\" id=\"Search-trips-FrequencyFilters\" class=\"txt-search ui-autocomplete-input\" name=\"Channel-Retailer-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"><div class=\"img-search\"></div></div></div>";
    htmlCommon += "<div class=\"VisistsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\"><div id=\"freqFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:100%;float:left;\">Online Order Frequency</div><div id=\"freqFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Frequency</div><div id=\"freqFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Online Order Frequency</div></div>";

    $(".rgt-cntrl-trips-frequency").append(htmlCommon);
    $(".rgt-cntrl-trips-frequency").append("<div id=\"frequency_containerId_trips\" class=\"frequency_container_trips\"></div>");
    $(".frequency_container_trips").append("<div class=\"rgt-cntrl-trips-frequency-Conatiner DemoLevel Lavel Lavel1\" style=\"height:98%;float:left;left: -1%;\"></div><div class=\"rgt-cntrl-frequency-Conatiner-SubLevel1 DemoLevel2 Lavel Lavel2\" style=\"display:none;float:left;height:98%; left: 285px;\"></div><div class=\"rgt-cntrl-trips-frequency-Conatiner-SubLevel2 DemoLevel2 Lavel Lavel2\" style=\"display:none;float:left;height:98%; left: 570px;\"></div>");
    //

    htmlCommon = "<div id=\"ordertypeFilter-Search-Content\" class=\"Search-Filter\" style=\"margin-top: 2%;width:269px;\"><div class=\"Search\"><input type=\"text\" id=\"Search-ordertypeFilters\" class=\"txt-search ui-autocomplete-input\" name=\"Channel-Retailer-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"><div class=\"img-search\"></div></div></div>";

    htmlCommon += "<div class=\"VisistsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\"><div id=\"freqFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:100%;float:left;\">Online Order Type</div><div id=\"freqFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Online Order Type</div><div id=\"freqFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:243px;float:left;display:none;\">Online Order Type</div></div>";
    
    $(".rgt-cntrl-ordertype").append(htmlCommon);
    $(".rgt-cntrl-ordertype").append("<div id=\"visit_frequency_containerId\" class=\"frequency_ordertype\"></div>");
    $(".frequency_ordertype").append("<div class=\"rgt-cntrl-ordertype-Conatiner DemoLevel Lavel Lavel1\" style=\"height:98%;float:left;left: -1%;\"></div><div class=\"rgt-cntrl-ordertype-Conatiner-SubLevel1 DemoLevel2 Lavel Lavel2\" style=\"display:none;float:left;height:98%; left: 285px;\"></div><div class=\"rgt-cntrl-ordertype-Conatiner-SubLevel2 DemoLevel2 Lavel Lavel2\" style=\"display:none;float:left;height:98%; left: 570px;\"></div>");



    if (currentpage.indexOf("beverage") > 0)
        $(".adv-fltr-freq").append("<div class=\"adv-fltr-text\">MONTHLY PURCHASE</div>");
    else
        $(".adv-fltr-freq").append("<div class=\"adv-fltr-text\">ONLINE ORDER FREQUENCY</div>");

    $(".adv-fltr-ordertype").append("<div class=\"adv-fltr-text\">ONLINE ORDER TYPE</div>");



    $(".adv-fltr-sub-selection").append("<div class=\"adv-fltr-suboptions-list-container visit-adv-filters tripsDiv\"></div>");

    //$(".adv-fltr-sub-selection").append("<div  class=\"adv-fltr-estblishment-container\"><div class=\"adv-fltr-estblishment\"></div></div>");
    //if (currentpage.indexOf("beverage") > 0 && sVisitsOrGuests == "1")
    //    $(".adv-fltr-sub-selection").append("<div id=\"adv-fltr-Chnl\" class=\"adv-fltr-Chnl-container tripsDiv\"></div>");

    $(".adv-fltr-sub-selection").append("<div  id=\"adv-bevselectiontype-freq\" class=\"adv-fltr-selection-container shopperDiv\"></div>");
    $("#adv-bevselectiontype-freq").append("<div class=\"lft-cntrl adv-fltr-freq bevselectiontype beverageChannel\"></div>");
    $("#adv-bevselectiontype-freq").append("<div class=\"rgt-cntrl-Selection beverageItems\"></div>");

    //Bevarage Selction
    $(".bevselectiontype").append("<div class=\"adv-fltr-text\">BEVERAGE PURCHASER LEVEL</div>");
    $(".rgt-cntrl-Selection").css("height", "277px");
    $(".rgt-cntrl-Selection").append("<div id=\"BEVERAGEFreqFilter-Search-Content\" class=\"Search-Filter\" style=\"margin-top: 2%;width:269px;\"><div class=\"Search\"><input type=\"text\" id=\"Search-BEVERAGEFrequencyFilters\" class=\"txt-search ui-autocomplete-input\" name=\"Channel-Retailer-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"><div class=\"img-search\"></div></div></div>");
    $(".rgt-cntrl-Selection").append("<div class=\"GuestsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\" style=\"margin-bottom:0px;\"><div id=\"BEVERAGEfreqFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;\">BEVERAGE PURCHASER LEVEL</div></div>");
    //$(".rgt-cntrl-Selection").append("<div class=\"rgt-cntrl-Selection-Conatiner DemoLevel Lavel Lavel1\" style=\"height:72%;float:left;margin-top: 3%;\"></div>");
    $(".rgt-cntrl-Selection").append("<div id=\"beverage-frequency\" class=\"rgt-cntrl-Selection-Conatiner\" style=\"height:72%;float:left;margin-top: 3%;\"></div>");
    //End   
    LoadVisitAdvancedFilters(data);

    jQuery('.classMouseHover').mouseenter(function () {
        jQuery(this).find('.play').show();
        var objs = GetMouseHoverPopUpDetails();
        var sClassName = $(this).attr('class').split(' ')[1];
        var sPopupDetails = _.filter(objs, function (i) {
            return i.cls == sClassName
        }).length > 0 ? _.filter(objs, function (i) { return i.cls == sClassName; })[0] : "";
        if (sPopupDetails != undefined && sPopupDetails != "" && sPopupDetails != null)
            MouseHoverPopupshowHide(sPopupDetails);
        //$("#MouseHoverBigDiv").show();
    });
    jQuery('.classMouseHover').mouseleave(function () {
        jQuery(this).find('.play').hide();
        var objs = GetMouseHoverPopUpDetails();

        $("#MouseHoverBigDiv").hide();
        $("#MouseHoverSmallDiv").hide();
        $("#MouseHoverSmallerDiv").hide();
        $("#MouseHoverExtraSmallDiv").hide();
    });
    $(".dynpos").hover(function () {
        var pos = $(this).position();
        var width = $(this).outerWidth();
        var height = $(this).outerHeight();
        var widthdiv = $("#MouseHoverExtraSmallDiv").outerWidth();
        //show the menu directly over the placeholder
        $("#MouseHoverExtraSmallDiv").css({
            position: "absolute",
            top: (pos.top + height) + "px",
            left: (pos.left - widthdiv + 22) + "px",
        }).show();

    }, function () {
        // Hover out code
        $('#MouseHoverExtraSmallDiv').hide();
    });
    $(".dynpos1").hover(function (e) {
        var pos = $(this).offset();
        var width = $(this).outerWidth();
        var height = $(this).outerHeight();
        var widthdiv = $("#MouseHoverSmallerDiv").outerWidth();

        var pageWidth = $(window).width();
        var pageHeight = $(window).width();
        var elementWidth = $("#MouseHoverSmallerDiv").outerWidth();
        var elementLeft = $(this).offset().left;
        var elementHeight = $("#MouseHoverSmallerDiv").outerHeight();
        var elementTop = $(this).offset().top;

        if ($(this).hasClass("up")) {
            $("#MouseHoverSmallerDiv").css({
                position: "absolute",
                top: (pos.top - height - 50) + "px",
                left: (pos.left + 5) + "px",
            }).show();
        }
        else {
            if ($(this).attr("id") == "btnClearAll") {
                $("#MouseHoverSmallerDiv").css({
                    position: "absolute",
                    top: (pos.top + height - 6) + "px",
                    left: (pos.left - widthdiv + $(this).width() + 15) + "px",
                }).show();
            }
           else{
            $("#MouseHoverSmallerDiv").css({
                position: "absolute",
                top: (pos.top + height - 6) + "px",
                left: (pos.left) + "px",
            }).show();
           }
        }
        //show the menu directly over the placeholder


    }, function () {
        // Hover out code
        $('#MouseHoverSmallerDiv').hide();
    });
}
function LoadFrequencyFilter(data) {
    html = "";
    var filter = null;
    if (currentpage.indexOf("beverage") > 0) {
        filter = data.MonthlyPurchaselist;
        //$(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text("MONTHLY PURCHASE");
    }
    else {
        //var sData = data.EcommFrequencylist;
        //$(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text("FREQUENCY");
       
    }
    filter = getFilter("E-Com Shopper Frequency");
    addfilter("frequency_containerId_trips", filter, "SelectTripsFrequency(this);");

    filter = getFilter("E-Com Shopper Frequency");
    addfilter("shopper_frequency_containerId", filter, "SelectFrequency(this);");
    
    filter = getFilter("E-Com Trips Frequency");
    addfilter("visit_frequency_containerId", filter, "SelectFrequency(this);");
    //var index = 0;
    //if (data != null) {
    //    for (var i = 0; i < sData.length; i++) {
    //        var object = sData[i];            
    //            if (index == 0) {
    //                html += "<ul>";
    //            }
    //            html += "<li Name=\"" + object.Name + "\" style=\"display:table;min-height:22px\">";              
               

    //            html += "<div onclick=\"SelectFrequency(this);\" tabtype=\"shopper\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"display: table-cell;width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.Name + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";


    //                AllFrequency.push(object.Id + "|" + object.Name);
                              
    //            html += "</li>";             
    //            index++;            
    //    }

    //    html += "</ul>";     

    //    $(".rgt-cntrl-frequency-Conatiner").html("");
    //    $(".rgt-cntrl-frequency-Conatiner").append(html);       
    //}
    
    //LoadSecondaryFrequencyFilters(data);
}
function LoadEcomTripsFrequencyFilter(data) {
    html = "";
    if (currentpage.indexOf("beverage") > 0) {
        var sData = data.MonthlyPurchaselist;
        //$(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text("MONTHLY PURCHASE");
    }
    else {
        var sData = data.EcommFrequencylist;
        //$(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text("FREQUENCY");
    }

    var index = 0;
    if (data != null) {
        for (var i = 0; i < sData.length; i++) {
            var object = sData[i];
            if (index == 0) {
                html += "<ul>";
            }
            html += "<li Name=\"" + object.Name + "\" style=\"display:table;min-height:22px\">";


            html += "<div onclick=\"SelectTripsFrequency(this);\" tabtype=\"shopper\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"display: table-cell;width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.Name + "\" data-isselectable=\"true\">" + object.Name + "</span></div>";


            AllFrequency.push(object.Id + "|" + object.Name);

            html += "</li>";
            index++;
        }

        html += "</ul>";      

        $(".rgt-cntrl-trips-frequency-Conatiner").html("");
        $(".rgt-cntrl-trips-frequency-Conatiner").append(html);
    }

    //LoadSecondaryFrequencyFilters(data);
}
function LoadTripsFrequencyFilter(data) {
    html = "";
    var sData = null;
    var filter = getFilter("E-Com Trips Frequency");
    addfilter("visit_frequency_containerId", filter, "SelectFrequency(this);");
    if (currentpage.indexOf("beverage") > 0) {
         sData = data.MonthlyPurchaselist;
        //$(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text("MONTHLY PURCHASE");
    }
    else {
        //var sData = data.EcommTripsFrequencylist;
        if (tabname.toUpperCase() == "ONLINE ORDERS") {
            //sData = _.filter(sFilterData.EcommTripsFrequencylist, function (o) {
            //    return (o.Name != 'TOTAL' && o.Name != 'AUTO-REPLENISHMENT')
            //});

            sData = _.filter(filter[0].Levels[0].LevelItems, function (o) {
                return (o.Name != 'TOTAL' && o.Name != 'AUTO-REPLENISHMENT')
            });
        }
        else if (tabname.toUpperCase() == "AUTO REPLENISHED DELIVERIES" || tabname.toUpperCase() == "AUTO REPLENISHMENT DELIVERIES")
            sData = _.filter(filter[0].Levels[0].LevelItems, function (o) {
                return (o.Name != 'TOTAL' && o.Name != 'ONLINE ORDER')
            });
        else
            sData = filter[0].Levels[0].LevelItems;

        //$(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text("FREQUENCY");
    }
    TripsFrequency = [];
    var index = 0;
    if (data != null) {
        for (var i = 0; i < sData.length; i++) {
            var object = sData[i];
            if (index == 0) {
                html += "<ul>";
            }
            html += "<li Name=\"" + object.Name + "\" style=\"display:table;min-height:22px\">";

            html += "<div onclick=\"SelectFrequency(this);\" tabtype=\"trips\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"display: table-cell;width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.Name + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";

            TripsFrequency.push(object.Id + "|" + object.Name);

            html += "</li>";
            index++;
        }

        html += "</ul>";

        $(".rgt-cntrl-ordertype-Conatiner").html("");
        $(".rgt-cntrl-ordertype-Conatiner").append(html);
    }

    //LoadSecondaryFrequencyFilters(data);
}

function LoadShopperAndTripList() {

}

function LoadAdvancedAnalyticsFilter(data) {
    $("#advanced-analytics-Channel-Trips").css("display","none");
    $("#advanced-analytics-Retailer-Trips").css("display", "none");
    $("#advanced-analytics-Channel-Shopper").css("display", "none");
    $("#advanced-analytics-Retailer-Shopper").css("display", "none");
    $("#advancedanalyticsHeadingLevel1").css("display", "none");
    $("#advancedanalyticsHeadingLevel2").css("display", "none");
    var advlist = ["#advanced-analytics-Channel-Shopper", "#advanced-analytics-Retailer-Shopper"];

    for (var k = 0; k < advlist.length; k++)
    {
        html = "";
        var sData = [];
        var index = 0;
        if(k == 0)
            sData = data.AdvanceAnalytics.ChannelVariables
        else
            sData = data.AdvanceAnalytics.RetailerVariables

        if (sData != null) {
            for (var i = 0; i < sData.length; i++) {
                var object = sData[i];
                if (index == 0) {
                    html += "<ul>";
                }
                html += "<li Name=\"" + object.MetricName + "\" style=\"display:table;min-height:22px;border-bottom: 0px;\">";
                html += "<div id=\"" + object.MetricId + "\" onclick=\"DisplaySecondaryAdvancedAnalytics(this,'" + advlist[k] + "');\" Name=\"" + object.MetricName + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.MetricId + "\" Name=\"" + object.MetricName.toString() + "\" FullName=" + object.MetricName + " data-isselectable=\"true\">" + object.MetricName + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                html += "</li>";
                index++;
            }

            html += "</ul>";


            $(advlist[k]).html("");
            $(advlist[k]).html(html);
            LoadSecondaryChannelAdvancedAnalyticsFilter(sData, advlist[k]);
        }
    }
   
}

function LoadAdvancedAnalyticsFilterTrips(data) {
    $("#advanced-analytics-Channel-Shopper").css("display", "none");
    $("#advanced-analytics-Retailer-Shopper").css("display", "none");
    $("#advanced-analytics-Channel-Trips").css("display", "none");
    $("#advanced-analytics-Retailer-Trips").css("display", "none");
    $("#advancedanalyticsHeadingLevel1").css("display", "none");
    $("#advancedanalyticsHeadingLevel2").css("display", "none");    
    var advlist = ["#advanced-analytics-Channel-Trips", "#advanced-analytics-Retailer-Trips"];

    for (var k = 0; k < advlist.length; k++) {
        html = "";
        var sData = [];
        var index = 0;
        if (k == 0)
            sData = data.TripsAdvanceAnalytics.ChannelVariables
        else
            sData = data.TripsAdvanceAnalytics.RetailerVariables

        if (sData != null) {
            for (var i = 0; i < sData.length; i++) {
                var object = sData[i];
                if (index == 0) {
                    html += "<ul>";
                }
                html += "<li Name=\"" + object.MetricName + "\" style=\"display:table;min-height:22px;border-bottom: 0px;\">";
                html += "<div id=\"" + object.MetricId + "\" onclick=\"DisplaySecondaryAdvancedAnalyticsTrips(this,'" + advlist[k] + "');\" Name=\"" + object.MetricName + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.MetricId + "\" Name=\"" + object.MetricName.toString() + "\" FullName=" + object.MetricName + " data-isselectable=\"true\">" + object.MetricName + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                html += "</li>";
                index++;
            }

            html += "</ul>";


            $(advlist[k]).html("");
            $(advlist[k]).html(html);
            LoadSecondaryChannelAdvancedAnalyticsFilterTrips(sData, advlist[k]);
        }
    }

}
//added by Nagaraju for frequency
function CheckRespectiveViewFrequency(frequency)
{
    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }

    if (currentpage == "hdn-report-compareretailersshoppers"
        || currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-analysis-acrossshopper") {
        switch(frequency.toLowerCase())
        {
            case "weekly +":
            case "monthly +":
                {
                    return true;
                    break;
                }
            default:
                {
                    return false;
                    break;
                }

        }
    }
    return true;
}

function LoadSecondaryFrequencyFilters(data) {
    html = "";
    var thirdLevelhtml = "";
   
    if (data != null) {
        for (var i = 0; i < data.EcommFrequencylist.length; i++) {
            if (data.EcommFrequencylist[i].Frequencylist.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + data.EcommFrequencylist[i].Id + "\" Name=\"" + data.EcommFrequencylist[i].Name + "\" FullName=\"" + data.EcommFrequencylist[i].FullName + "\" style=\"overflow-y:auto;display:none;position:relative;\"><ul>";
                for (var j = 0; j < data.EcommFrequencylist[i].Frequencylist.length; j++) {
                    var object = data.EcommFrequencylist[i].Frequencylist[j];
                    html += "<li Name=\"" + object.Name + "\" style=\"display:table;min-height:22px;\">";
                            // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                    html += "<div id=\"" + object.Id + "\" onclick=\"SelectFrequency(this);\" class=\"lft-popup-ele\" style=\"width:100%;height:auto;\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" parent=\"" + data.EcommFrequencylist[i].Name + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";
                    html += "</li>";
                    AllFrequency.push(object.Id + "|" + object.Name);
                }
                html += "</ul></div>";
            }
        }
    }

    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").html("");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").html(html);
    
}
function LoadSecondaryChannelAdvancedAnalyticsFilter(data, contentid) {
    html = "";
    if (data != null) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].MetricItemList.length > 0) {
                html += "<div class=\"DemographicList\" contentid=\"" + contentid + "\"id=\"" + data[i].MetricId + "\" Name=\"" + data[i].MetricName + "\" style=\"overflow-y:auto;display:none;position:relative;\"><ul>";
                for (var j = 0; j < data[i].MetricItemList.length; j++) {
                    var object = data[i].MetricItemList[j];
                    html += "<li Name=\"" + object.MetricItemName + "\" style=\"display:table;min-height:22px;border-bottom: 0px;\">";
                    // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                    html += "<div id=\"" + object.MetricItemId + "\" onclick=\"SelectAdvanceAnalytics(this,'" + contentid + "');\" class=\"lft-popup-ele\" style=\"width:100%;height:auto;\"></span><span class=\"lft-popup-ele-label\" Name=\"" + object.MetricItemName + "\" id=" + object.MetricItemId + "\" SelId=" + object.SelId + "\" SelType=" + object.SelType + " UniqueFilterId=\"" + object.UniqueFilterId + "\" Name=\"" + object.MetricItemName + "\" parent=\"" + data[i].MetricName + "\" data-isselectable=\"true\">" + object.MetricItemName + "</span></div>";
                    html += "</li>";
                    AllFrequency.push(object.MetricItemId + "|" + object.MetricItemName);
                }
                html += "</ul></div>";
            }
        }
    }

    //$(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").html("");
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").append(html);

}
function LoadSecondaryChannelAdvancedAnalyticsFilterTrips(data, contentid) {
    html = "";
    if (data != null) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].MetricItemList.length > 0) {
                html += "<div class=\"DemographicList\" contentid=\"" + contentid + "\"id=\"" + data[i].MetricId + "\" Name=\"" + data[i].MetricName + "\" style=\"overflow-y:auto;display:none;position:relative;\"><ul>";
                for (var j = 0; j < data[i].MetricItemList.length; j++) {
                    var object = data[i].MetricItemList[j];
                    html += "<li Name=\"" + object.MetricItemName + "\" style=\"display:table;min-height:22px;border-bottom: 0px;\">";
                    // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                    html += "<div id=\"" + object.MetricItemId + "\" onclick=\"SelectAdvanceAnalyticsTrips(this,'" + contentid + "');\" class=\"lft-popup-ele\" style=\"width:100%;height:auto;\"></span><span class=\"lft-popup-ele-label\" Name=\"" + object.MetricItemName + "\" id=" + object.MetricItemId + "\" SelId=" + object.SelId + "\" SelType=" + object.SelType + " UniqueFilterId=\"" + object.UniqueFilterId + "\" Name=\"" + object.MetricItemName + "\" parent=\"" + data[i].MetricName + "\" data-isselectable=\"true\">" + object.MetricItemName + "</span></div>";
                    html += "</li>";
                    AllFrequency.push(object.MetricItemId + "|" + object.MetricItemName);
                }
                html += "</ul></div>";
            }
        }
    }

    //$(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").html("");
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").append(html);

}

function DisplaySecondaryFrequency(obj) {
    $(".rgt-cntrl-frequency").css("width", "auto");
    var offset = $(obj).offset();
    var height = $(obj).height();
    var width = $(obj).width();
    var top = offset.top + height + "px";
    var right = offset.left + width + 2 + "px";
    if ($(obj).attr("name").length > 18) {
        $('.rgt-cntrl-frequency').css({
            'position': 'absolute',
            'left': offset.left - width - 10 + "px",
            'top': offset.top - 17,
        });
    }
    else {
        $('.rgt-cntrl-frequency').css({
            
            'left': offset.left - width - 1 + "px",
            'top': offset.top - 17,
        });
    }
    //$("#rgt-cntrl-frequency-Conatiner").find(".Selected").removeClass("Selected");
    $(obj).addClass("Selected");
    $(".rgt-cntrl-frequency-Conatiner .DemographicList").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1 .DemographicList").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2").hide();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel3").hide();
    //$(".rgt-cntrl-frequency-Conatiner-SubLevel2").css("display", "inline-block");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").show();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1 div[Id='" + $(obj).attr("Id") + "']").css("display", "inline-block");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1 div[Id='" + $(obj).attr("Id") + "']").show();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").text($(obj).attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").css("width", "245px");
    SetScroll($(".rgt-cntrl-frequency-Conatiner-SubLevel1"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplaySecondaryAdvancedAnalytics(obj, contentid) {
    $("#advanced-analytics-Retailer-Shopper ul li .lft-popup-ele").removeClass("Selected");
    $("#advanced-analytics-Channel-Shopper ul li .lft-popup-ele").removeClass("Selected");
    $(".rgt-cntrl-advanced-analytics").css("width", "auto");
   
    //$("#rgt-cntrl-frequency-Conatiner").find(".Selected").removeClass("Selected");
    //$(obj).addClass("Selected");
    var _metriclist = [];
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 div[id='" + $(obj).attr("Id") + "'][contentid='" + contentid + "'] ul li div").each(function (index, object) {
        $(object).addClass("Selected");
        var objec = $(object).find(".lft-popup-ele-label");
        _metriclist.push({ Id: $(objec).attr("id"), Name: $(objec).attr("name"), UniqueFilterId: $(objec).attr("UniqueFilterId"), FullName: $(objec).attr("parent") });
    });
    SelectedAdvancedAnalyticsList = _metriclist;
    $(contentid +" .DemographicList").hide();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 .DemographicList").hide();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel2").hide();
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel3").hide();
    //$(".rgt-cntrl-frequency-Conatiner-SubLevel2").css("display", "inline-block");
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").show();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 div[id='" + $(obj).attr("Id") + "'][contentid='" + contentid + "']").css("display", "inline-block");
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 div[id='" + $(obj).attr("Id") + "'][contentid='" + contentid + "']").show();
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel2").text($(obj).attr("name").toUpperCase());
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel2").css("width", "245px");
    SetScroll($(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplaySecondaryAdvancedAnalyticsTrips(obj, contentid) {
    $("#advanced-analytics-Retailer-Trips ul li .lft-popup-ele").removeClass("Selected");
    $("#advanced-analytics-Channel-Trips ul li .lft-popup-ele").removeClass("Selected");
    $(".rgt-cntrl-advanced-analytics").css("width", "auto");

    //$("#rgt-cntrl-frequency-Conatiner").find(".Selected").removeClass("Selected");
    //$(obj).addClass("Selected");
    var _metriclist = [];
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 div[id='" + $(obj).attr("Id") + "'][contentid='" + contentid + "'] ul li div").each(function (index, object) {
        $(object).addClass("Selected");
        var objec = $(object).find(".lft-popup-ele-label");
        _metriclist.push({ Id: $(objec).attr("id"), Name: $(objec).attr("name"), UniqueFilterId: $(objec).attr("UniqueFilterId"), FullName: $(objec).attr("parent") });
    });
    SelectedAdvancedAnalyticsList = _metriclist;
    $(contentid + " .DemographicList").hide();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 .DemographicList").hide();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").hide();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel2").hide();
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel3").hide();
    //$(".rgt-cntrl-frequency-Conatiner-SubLevel2").css("display", "inline-block");
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1").show();
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 div[id='" + $(obj).attr("Id") + "'][contentid='" + contentid + "']").css("display", "inline-block");
    $(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1 div[id='" + $(obj).attr("Id") + "'][contentid='" + contentid + "']").show();
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel2").text($(obj).attr("name").toUpperCase());
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #advancedanalyticsHeadingLevel2").css("width", "245px");
    SetScroll($(".rgt-cntrl-advanced-analytics-Conatier-SubLevel1"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function SelectFrequency(obj) {   
    SelectedFrequencyList = [];
    $("#visit_frequency_containerId ul li,#shopper_frequency_containerId ul li").removeClass("Selected");
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    for (var i = 0; i < SelectedFrequencyList.length; i++) {
        if (SelectedFrequencyList[i].Id == $(object).attr("id")) {
            sCurrentDemoId = i;
        }
    }

    if ($(obj).hasClass("Selected")) {
        $(obj).removeClass("Selected");
        SelectedFrequencyList.splice(sCurrentDemoId, 1);
    }
    else {
        SelectedFrequencyList = [];
        $(".rgt-cntrl-frequency-Conatiner").find(".Selected").removeClass("Selected");
        $(".rgt-cntrl-frequency-Conatiner-SubLevel1").find(".Selected").removeClass("Selected");
        $(obj).parent().parent().find(".Selected").removeClass("Selected");
        $(obj).addClass("Selected");
        SelectedFrequencyList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("uniqueid"), TabType: $(obj).attr("tabtype") });
    }

    ShowOrHideVisitFilters();
    ShowSelectedFilters();

    //if (currentpage == "hdn-e-commerce-tbl-comparesites" || currentpage == "hdn-e-commerce-tbl-sitedeepdive" || currentpage == "hdn-e-commerce-chart-comparesites" || currentpage == "hdn-e-commerce-chart-sitedeepdive") {
    //    if ($(".adv-fltr-suboptions-list-container").is(":visible") && $(".adv-fltr-suboptions-list-container ul li:visible").length > 3) {
    //        $(".adv-filters-wraper").css("left", "1%");
    //    }
    //    else {
    //        $(".adv-filters-wraper").css("left", "27%");
    //    }
    //}
    //else if (sVisitsOrGuests == 2) {
    //    $(".adv-filters-wraper").css("left", "28%");
    //}
    //else {
    //    $(".adv-filters-wraper").css("left", "1%");
    //}
}
//added by Nagaraju for trips fre
//Date: 09-06-2017
function SelectTripsFrequency(obj) {
    SelectedTripsFrequencyList = [];
    $(".rgt-cntrl-frequency-Conatiner ul li div").removeClass("Selected");
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    for (var i = 0; i < SelectedTripsFrequencyList.length; i++) {
        if (SelectedTripsFrequencyList[i].Id == $(object).attr("id")) {
            sCurrentDemoId = i;
        }
    }

    if ($(obj).hasClass("Selected")) {
        $(obj).removeClass("Selected");
        SelectedTripsFrequencyList.splice(sCurrentDemoId, 1);
    }
    else {
        SelectedTripsFrequencyList = [];
        $(".rgt-cntrl-frequency-Conatiner").find(".Selected").removeClass("Selected");
        $(".rgt-cntrl-frequency-Conatiner-SubLevel1").find(".Selected").removeClass("Selected");
        $(obj).parent().parent().find(".Selected").removeClass("Selected");
        $(obj).addClass("Selected");
        SelectedTripsFrequencyList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), UniqueId: $(object).attr("uniqueid"), TabType: $(obj).attr("tabtype") });
    }
    ShowOrHideVisitFilters();
    ShowSelectedFilters();
}

function SelectAdvanceAnalytics(obj, contentid) {
    $(".Lavel#advanced-analytics-Channel-Shopper ul li div").removeClass("Selected");
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    for (var i = 0; i < SelectedAdvancedAnalyticsList.length; i++) {
        if (SelectedAdvancedAnalyticsList[i].Id == $(object).attr("id")) {
            sCurrentDemoId = i;
        }
    }

    if ($(obj).hasClass("Selected")) {
        $(obj).removeClass("Selected");
        SelectedAdvancedAnalyticsList.splice(sCurrentDemoId, 1);
    }
    else {
        //SelectedAdvancedAnalyticsList = [];
        //$(".Lavel#advanced-analytics-Channel").find(".Selected").removeClass("Selected");
        //$(".Lavel#advanced-analytics-Channel-SubLevel1").find(".Selected").removeClass("Selected");
        //$(".Lavel#advanced-analytics-Retailer").find(".Selected").removeClass("Selected");
        //$(".Lavel#advanced-analytics-Retailer-SubLevel1").find(".Selected").removeClass("Selected");
        //$(obj).parent().parent().find(".Selected").removeClass("Selected");
        $(obj).addClass("Selected");
        SelectedAdvancedAnalyticsList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), UniqueFilterId: $(object).attr("UniqueFilterId"), FullName: $(object).attr("parent") });
    }
    ShowSelectedFilters();
}
function SelectAdvanceAnalyticsTrips(obj, contentid) {
    $(".Lavel#advanced-analytics-Channel-Trips ul li div").removeClass("Selected");
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    for (var i = 0; i < SelectedAdvancedAnalyticsList.length; i++) {
        if (SelectedAdvancedAnalyticsList[i].Id == $(object).attr("id")) {
            sCurrentDemoId = i;
        }
    }

    if ($(obj).hasClass("Selected")) {
        $(obj).removeClass("Selected");
        SelectedAdvancedAnalyticsList.splice(sCurrentDemoId, 1);
    }
    else {
        //SelectedAdvancedAnalyticsList = [];
        //$(".Lavel#advanced-analytics-Channel").find(".Selected").removeClass("Selected");
        //$(".Lavel#advanced-analytics-Channel-SubLevel1").find(".Selected").removeClass("Selected");
        //$(".Lavel#advanced-analytics-Retailer").find(".Selected").removeClass("Selected");
        //$(".Lavel#advanced-analytics-Retailer-SubLevel1").find(".Selected").removeClass("Selected");
        //$(obj).parent().parent().find(".Selected").removeClass("Selected");
        $(obj).addClass("Selected");
        SelectedAdvancedAnalyticsList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), UniqueFilterId: $(object).attr("UniqueFilterId"), FullName: $(object).attr("parent") });
    }
    ShowSelectedFilters();
}

function RemoveFrequency(obj) {
    var ObjData = $(".rgt-cntrl-frequency-Conatiner * [Fullname='" + $(obj).attr("Fullname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [Fullname='" + $(obj).attr("Fullname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']").parent();
    if (ObjData.length <= 0)
        ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel2 * [Fullname='" + $(obj).attr("Fullname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']").parent();
    SelectFrequency(ObjData);
}
function RemoveTripsFrequency() {
    SelectedTripsFrequencyList = [];
    $("#frequency_containerId_trips li").removeClass("Selected");
    ShowSelectedFilters();
}
function LoadVisitAdvancedFilters(data) {   
    html = "";
    var index = 0;
    //var ulclose = false;
    var ImageDetails = GetRightPanelImagePosition(1);
    if (data != null) {
        var filter = getFilter("E-Com Visits");
        //for (var i = 0; i < data.EcommerceVisitAdvancedFilter.length; i++) {
        //    var object = data.EcommerceVisitAdvancedFilter[i];
        //    if (index == 0) {
        //        html += "<ul style=\"\">";
        //    }
        //    if (object.Level == "1") {
        //        html += "<li class=\"MainSelection\" name=\"" + object.Name + "\" style=\"display: inline-block;height: 100%;width: 90px;float: left;margin: 0px 3px;border-radius: 5px;\">";
        //        //html += "<div Name=\"" + object.Name + "\" class=\"\" onclick=\"DisplaySecondaryDemoFilter(this);\">" + object.Name + "</div>";
        //        //html += "<div onclick=\"DisplaySecondaryDemoFilter(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"display: table-cell;width: 216px;\"><span class=\"lft-popup-ele-label\" data-id=\"1\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><span class=\"lft-popup-ele-next sidearrw\"></span></div>";
        //        var sImageClassName = _.filter(ImageDetails, function (i) { return i.MetricName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.MetricName == object.Name; })[0].imagePosition : "";
        //        if (object.Name.length > 18)
        //            html += "<div style=\"width: 100%;margin-right:0%;border-radius:5px;\" class=\"adv-fltr-estblishment-container\"><div style=\"width:100%;height:98%;align-items: center;justify-content: center;box-sizing: border-box;display: flex;\"><div class=\"adv-fltr-text\" style=\"word-break: break-all;\">" + object.Name.toUpperCase() + "</div></div></div><div class=\"rgt-cntrl-" + object.Name + " DemoLevel\"></div>";
        //        else
        //            html += "<div style=\"width: 100%;border-radius:5px;\" class=\"adv-fltr-estblishment-container\"><div style=\"width:100%;height:98%;align-items: center;justify-content: center;box-sizing: border-box;display: flex;\"><div class=\"adv-fltr-text\">" + object.Name.toUpperCase() + "</div></div></div><div class=\"rgt-cntrl-" + object.Name + " DemoLevel\"></div>";

        //        //AllDemographics.push(object.Name);
        //        if (data.EcommerceVisitAdvancedFilter.length-1 != i) {
        //            //html += "</li>";
        //            //html += "<li style=\"width: 30px; margin: 0; display: flex; justify-content: center;\">";
        //            //html += "<div class=\"filter-separator\"></div>";
        //            //html += "</li>";
        //        }
        //        index++;
        //    }
        //}
        for (var i = 0; i < filter[0].Levels[0].LevelItems.length; i++) {
            var object = filter[0].Levels[0].LevelItems[i];
            if (index == 0) {
                html += "<ul style=\"\">";
            }
            if (filter[0].Levels[0].Id == "1") {
                html += "<li class=\"MainSelection\" id=\"" + object.Id + "\"  parentid=\"" + object.ParentId + "\" name=\"" + object.Name + "\" style=\"display:inline-block;height: 100%;width: 90px;float: left;margin: 0px 3px;border-radius: 5px;\">";
                //html += "<div Name=\"" + object.Name + "\" class=\"\" onclick=\"DisplaySecondaryDemoFilter(this);\">" + object.Name + "</div>";
                //html += "<div onclick=\"DisplaySecondaryDemoFilter(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"display: table-cell;width: 216px;\"><span class=\"lft-popup-ele-label\" data-id=\"1\" data-val=" + object.Name + " data-parentid=\"\" data-isselectable=\"true\">" + object.Name + "</span><span class=\"lft-popup-ele-next sidearrw\"></span></div>";adv-fltr-suboptions-list-container
                var sImageClassName = _.filter(ImageDetails, function (i) { return i.MetricName == object.Name; }).length > 0 ? _.filter(ImageDetails, function (i) { return i.MetricName == object.Name; })[0].imagePosition : "";
                if (object.Name.length > 18)
                    html += "<div style=\"width: 100%;margin-right:0%;border-radius:5px;\" class=\"adv-fltr-estblishment-container\"><div style=\"width:100%;height:98%;align-items: center;justify-content: center;box-sizing: border-box;display: flex;\"><div class=\"adv-fltr-text\">" + object.Name.toUpperCase() + "</div></div></div><div class=\"rgt-cntrl-" + object.Name + " DemoLevel\"></div>";
                else
                    html += "<div style=\"width: 100%;border-radius:5px;\" class=\"adv-fltr-estblishment-container\"><div style=\"width:100%;height:98%;align-items: center;justify-content: center;box-sizing: border-box;display: flex;\"><div class=\"adv-fltr-text\">" + object.Name.toUpperCase() + "</div></div></div><div class=\"rgt-cntrl-" + object.Name + " DemoLevel\"></div>";

                //AllDemographics.push(object.Name);
                if (filter[0].Levels[0].LevelItems.length - 1 != i) {
                    //html += "</li>";
                    //html += "<li style=\"width: 30px; margin: 0; display: flex; justify-content: center;\">";
                    //html += "<div class=\"filter-separator\"></div>";
                    //html += "</li>";
                }
                index++;
            }
        }
        //if (ulclose == false)
        html += "</ul>";

        $(".adv-fltr-suboptions-list-container").html("");
        $(".adv-fltr-suboptions-list-container").append(html);
        //$(".adv-fltr-suboptions-list-container").append("<div class=\"rgt-cntrl-SubFilter-Conatianer\"><div class=\"Search-Filter\"><div class=\"Search\"><ul><li><input type=\"text\" class=\"txt-search ui-autocomplete-input\" name=\"Channel-AdvancedFilters-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"></li><li><div class=\"img-search\"></div></li></ul></div><ul id=\"ui-id-2\" tabindex=\"0\" class=\"ui-menu ui-widget ui-widget-content ui-autocomplete ui-front\" style=\"display: none;\"></ul></div><div style=\"float:left;overflow-y:auto;height:100%;\"  id=\"rgt-cntrl-SubFilter1\" class=\"rgt-cntrl-SubFilter1 DemoLevel\"></div><div id=\"rgt-cntrl-SubFilter2\" class=\"rgt-cntrl-SubFilter2 DemoLevel\"></div></div>");
        $(".adv-fltr-suboptions-list-container").append("<div class=\"rgt-cntrl-SubFilter-Conatianer\"></div>");
        var htmlCommon = "";
        htmlCommon += "<div id=\"VisitsAdvancedFilter-Search-Content\" class=\"Search-Filter\" style=\"margin-top: 2%;width:269px;\"><div class=\"Search\"><input type=\"text\" id=\"Search-VisitsAdvancedFilters\" class=\"txt-search\" name=\"Channel-Retailer-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"><div class=\"img-search\"></div></div></div>";

        htmlCommon += "<div class=\"VisistsAdvancedFiltersDemoHeading AdvancedFiltersDemoHeading\"><div id=\"AdvFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;\">DEMOGRAPHIC FILTER</div><div id=\"AdvFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;display:none;\">DEMOGRAPHIC FILTER</div><div id=\"AdvFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;display:none;\">DEMOGRAPHIC FILTER</div></div>";
       
        
        $(".rgt-cntrl-SubFilter-Conatianer").append(htmlCommon);
        $(".rgt-cntrl-SubFilter-Conatianer").append("<div id=\"VisistsFilterDivId\" class=\"VisistsFilterDiv\"></div>");
        
        $("#VisistsFilterDivId").append("<div style=\"float:left;overflow-y:auto;height:100%;left: -1%;\"  id=\"rgt-cntrl-SubFilter1\" class=\"rgt-cntrl-SubFilter1 DemoLevel Lavel Lavel1\"></div><div id=\"rgt-cntrl-SubFilter2\" style=\"width:240px;height:98%;float:right;\"  class=\"rgt-cntrl-SubFilter2 DemoLevel2 Lavel2\"></div>");
        
        LoadSecondaryVisitsAdvancedFilters(data);

    }
}
function LoadSecondaryVisitsAdvancedFilters(data) {
   // html = "";
   //var thirdLevelhtml = "";
   // if (data != null) {
   //     for (var i = 0; i < data.EcommerceVisitAdvancedFilter.length; i++) {
   //         if (data.EcommerceVisitAdvancedFilter[i].SecondaryAdvancedFilterlist.length > 0) {
   //             html += "<div class=\"DemographicList\" id=\"" + data.EcommerceVisitAdvancedFilter[i].Id + "\" Name=\"" + data.EcommerceVisitAdvancedFilter[i].Name + "\" FullName=\"" + data.EcommerceVisitAdvancedFilter[i].FullName + "\" style=\"overflow-y:auto;height:auto;position:relative;\"><ul>";
   //             thirdLevelhtml += "<div parentName=\"" + data.EcommerceVisitAdvancedFilter[i].Name + "\" class=\"DemographicList\" id=\"" + data.EcommerceVisitAdvancedFilter[i].Id + "\" Name=\"" + data.EcommerceVisitAdvancedFilter[i].Name + "\" FullName=\"" + data.EcommerceVisitAdvancedFilter[i].FullName + "\" style=\"display:none;position:relative;\"><ul>";

   //             for (var j = 0; j < data.EcommerceVisitAdvancedFilter[i].SecondaryAdvancedFilterlist.length; j++) {
   //                 var object = data.EcommerceVisitAdvancedFilter[i].SecondaryAdvancedFilterlist[j];
   //                 if (data.EcommerceVisitAdvancedFilter[i].Level == "1") {
   //                     //if (data.EcommerceVisitAdvancedFilter[i].SecondaryAdvancedFilterlist.length <= 0)
   //                     var k = _.filter(sFilterData.EcommerceVisitAdvancedFilter, function (u) {
   //                         return ((u.FullName == object.Name) && (u.Id == object.MetricId));
   //                     });
   //                     if (k.length <= 0) {
   //                         // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
   //                         html += "<div onclick=\"SelectAdvfilters(this);\" class=\"lft-popup-ele\" style=\"min-height: 28px;\"></span><span uniqueid=\"" + object.UniqueId + "\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" dbname=\"" + object.DBName + "\" data-isselectable=\"true\" parentlevelname=\"" + data.EcommerceVisitAdvancedFilter[i].Name.toString().trim() + "\" parent=\"" + object.ParentId + "\" >" + object.Name + "</span></div>";
   //                         AllAdvancedFilters.push(object.Id + "|" + object.Name);
   //                     }
   //                     else
   //                         html += "<div onclick=\"DisplayVisistThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"></span><span uniqueid=\"" + object.UniqueId + "\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" dbname=\"" + object.DBName + "\" data-isselectable=\"false\" parentlevelname=\"" + data.EcommerceVisitAdvancedFilter[i].Name.toString().trim() + "\" parent=\"" + object.ParentId + "\" >" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
   //                 }
   //                 else {
   //                     thirdLevelhtml += "<div parentName=\"" + data.EcommerceVisitAdvancedFilter[i].Name + "\" onclick=\"SelectAdvfilters(this);\" class=\"lft-popup-ele\" style=\"\"></span><span uniqueid=\"" + object.UniqueId + "\" class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " dbname=\"" + object.DBName + "\" data-isselectable=\"true\" parentlevelname=\"" + data.EcommerceVisitAdvancedFilter[i].Name.toString().trim() + "\" Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" >" + object.Name + "</span></div>";
   //                     AllAdvancedFilters.push(object.Id + "|" + object.Name);
   //                 }
   //                     //AllDemographics.push(object.Name);
                    
   //             }
   //             html += "</ul></div>";
   //             thirdLevelhtml += "</ul></div>";
   //         }
   //     }
   // }
   // $("#rgt-cntrl-SubFilter1").html("");
   // $("#rgt-cntrl-SubFilter1").html(html);
   // $("#rgt-cntrl-SubFilter2").html("");
    // $("#rgt-cntrl-SubFilter2").html(thirdLevelhtml);   
    //added by Nagaraju D for filter revamp
    //Date: 12-14-2017
    $("#rgt-cntrl-SubFilter1").html("<ul></ul>");
    $("#rgt-cntrl-SubFilter2").html("<ul></ul>");
    if (data != null) {
        var filter = getFilter("E-Com Visits");
        for (var level = 0; level < filter[0].Levels.length; level++) {
            if (level != 0) {
                for (var i = 0; i < filter[0].Levels[level].LevelItems.length; i++) {
                    obj = filter[0].Levels[level].LevelItems[i];
                    if (obj.HasSubLevel)
                        $("#rgt-cntrl-SubFilter" + level + " ul").append("<li name=\"" + obj.Name + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" onclick=\"DisplayVisistThirdLevelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span uniqueid=\"" + obj.UniqueId + "\" class=\"lft-popup-ele-label\" fullname=\"" + obj.Name + "\" name=\"" + obj.Name + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentname=\"" + obj.ParentName + "\" parentid=\"" + obj.ParentId + "\">" + obj.Name.trim() + "</span><div class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></li>");
                    else
                        $("#rgt-cntrl-SubFilter" + level + " ul").append("<li name=\"" + obj.Name + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" onclick=\"SelectAdvfilters(this);\" class=\"lft-popup-ele\" style=\"display: inline-flex;\"><span style=\"width: 94%;\" uniqueid=\"" + obj.UniqueId + "\" class=\"lft-popup-ele-label\" fullname=\"" + obj.Name + "\" name=\"" + obj.Name + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentname=\"" + obj.ParentName + "\" parentid=\"" + obj.ParentId + "\">" + obj.Name.trim() + "</span></li>");
                }
            }
        }
    }
}
function DisplayVisistSecondaryDemoFilter(obj) {
    //var sPrimaryDemo = $(obj).parent().parent().parent()[0];
    //$(sPrimaryDemo).find(".Selected").removeClass("Selected");
    //$(obj).addClass("Selected");
    $(".rgt-cntrl-SubFilter-Conatianer .Lavel").hide();
    $(".rgt-cntrl-SubFilter-Conatianer").show();
    $(".rgt-cntrl-SubFilter-Conatianer").css("display", "inline-block");
    $(".rgt-cntrl-SubFilter-Conatianer").css("width", "auto");
    $(".adv-fltr-suboptions-list-container *").removeClass("TileActive");
    //LoadVisitAdvancedFilters(sFilterData);
    //var ImageDetails = GetRightPanelImagePosition(2);
    //var sImageClassName = _.filter(ImageDetails, function (i) { return i.MetricName == $(obj).attr("name"); }).length > 0 ? _.filter(ImageDetails, function (i) { return i.MetricName == $(obj).attr("name"); })[0].imagePosition : "";
    //$(".adv-fltr-suboptions-list-container li[name='" + $(obj).attr("name") + "' i] .adv-fltr-estblishment").css("background-position", sImageClassName.substring(0, sImageClassName.length -1));
    $(".adv-fltr-suboptions-list-container li[name='" + $(obj).attr("name") + "'] div").addClass("TileActive");
    //$(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text($(obj).attr("name"));  AdvFilterHeadingLevel1

    var height = $(obj).height();
    var width = $(obj).width();

    var left = $(obj).children("div").eq(0).offset().left;
    var offset1 = $(".adv-filters-wraper").offset()
    var height_wraper = $(".adv-filters-wraper").height();

    var dd_width = $('#VisistsFilterDivId').find('#rgt-cntrl-SubFilter1').outerWidth();
    if (currentpage.indexOf("chart") > -1)
        var tableContent_Width = $('#ToShowChart').outerWidth();
    else
        var tableContent_Width = $('#Table-Content').outerWidth();
    if (tableContent_Width != null && tableContent_Width < (left + dd_width)) {
        $('.rgt-cntrl-SubFilter-Conatianer').css({
            'position': 'absolute',
            'left': left - offset1.left + width - dd_width,//left - offset1.left - ((left - offset1.left + dd_width) - tableContent_Width)
            'top': (height_wraper + 1),//($(obj).height() - 3),//$(obj).offset().top
        });
    }
    else if ($(obj).attr("name") != undefined && $(obj).attr("name").length > 18) {
        $('.rgt-cntrl-SubFilter-Conatianer').css({
            'position': 'absolute',
            'left': left - offset1.left,
            'top': (height_wraper + 1),//($(obj).height() + 1),//$(obj).offset().top + 
        });
    }
    else {
        $('.rgt-cntrl-SubFilter-Conatianer').css({
            'position': 'absolute',
            'left': left - offset1.left,
            'top': (height_wraper + 1),//($(obj).height() + 1),//$(obj).offset().top + 
        });
    }
    $("#rgt-cntrl-SubFilter1 .DemographicList").hide();
    $("#rgt-cntrl-SubFilter1 .DemographicList").hide();
    $("#rgt-cntrl-SubFilter2").hide();
    $("#rgt-cntrl-SubFilter1").css("display", "inline-block");
    $("#rgt-cntrl-SubFilter1").show();
    $(".rgt-cntrl-SubFilter1 ul li").hide();
    $(".rgt-cntrl-SubFilter1 ul li[parentid='" + $(obj).attr("id") + "'][parentname='" + $(obj).attr("name") + "']").show();

    $("#VisistsFilterDivId .Lavel").hide();
    $("#VisistsFilterDivId ul li").hide();
    $("#VisistsFilterDivId ul li[parentid='" + $(obj).attr("id") + "'][parentname='" + $(obj).attr("name") + "']").show();
    $("#VisistsFilterDivId div[level-id='2']").show();
    SetScroll($("#rgt-cntrl-SubFilter1"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
    sNonSelecteble = [];

    $("#rgt-cntrl-SubFilter1 div[name='" + $(obj).attr("name") + "'] ul .lft-popup-ele-label").each(function () {
        var object = $(this)[0];
        if (object.attributes[1] != undefined && object.attributes[6].textContent == "true")
            //if (object.attributes[2] != undefined)
            sCurrentAdvancedFilters.push(object.attributes[0].textContent + "|" + object.attributes[4].textContent);
        else if (object.attributes[1] != undefined)
            sNonSelecteble.push(object.attributes[0].textContent + "|" + object.attributes[4].textContent);
    });
    _.each($("#rgt-cntrl-SubFilter2 .DemographicList"), function (object) {
        //if (object.attributes[2] != undefined)
        var sIndex = 0;
        _.each(sCurrentAdvancedFilters, function (i) {
            if (object.attributes[4].textContent != "")
                if (i.split("|")[1] == (object.attributes[4].textContent)) {
                    sIndex = 5;
                    return false;
                }
                else
                    sIndex = 0;
            else
                sIndex = 0;
        });
        if (sIndex <= 0) {
            _.each(sNonSelecteble, function (i) {
                if (object.attributes[4].textContent != "")
                    if (i.split("|")[1] == (object.attributes[3].textContent)) {
                        sIndex = 5;
                        return false;
                    }
                    else
                        sIndex = 0;
                else
                    sIndex = 0;
            });
        }
        if (sIndex > 0) {
            $(object.childNodes[0].children).each(function () {
                sCurrentAdvancedFilters.push($(this).children("span").eq(0)[0].attributes[0].textContent + "|" + $(this).children("span").eq(0)[0].attributes[2].textContent);
            });

        }
    });

    //_.each($("#rgt-cntrl-SubFilter2 ul div"), function (object) {
    //    //if (object.attributes[2] != undefined)
    //    var sIndex = -1;
    //    _.each(sCurrentAdvancedFilters, function (i) { object.children[1].attributes[1].textContent != "" ? (sIndex = i.split("|")[1] == (object.children[1].attributes[1].textContent) ? 1 : -1) : sIndex = -1; });
    //    _.each(sNonSelecteble, function (i) { object.children[1].attributes[1].textContent != "" ? (sIndex = i.split("|")[1] == (object.children[1].attributes[1].textContent) ? 1 : -1) : sIndex = -1; });

    //    if (sIndex >= 0) {
    //        sCurrentAdvancedFilters.push(object.children[1].attributes[2].textContent + "|" + object.children[1].attributes[1].textContent);
    //    }
    //});
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel1").hide();
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").hide();
    var s = ($(obj).attr("name") != undefined && $(obj).attr("name") != "") ? $(obj).attr("name") : "";
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel1").text(s);
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel1").show();
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel1").css("width", "286px");
    DisplayHeightDynamicCalculation("VisistsFilter");
    SetScroll($("#rgt-cntrl-SubFilter1"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplayVisistThirdLevelDemoFilter(obj) {
    $(".rgt-cntrl-SubFilter-Conatianer .sidearrw_OnCLick").each(function (i, j) {
        $(j).eq(0).parent().eq(0).find(".sidearrw_OnCLick").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    });
    var sPrimaryDemo = $(obj);
    $(sPrimaryDemo).find(".sidearrw").removeClass("sidearrw").addClass("sidearrw_OnCLick");

    var sPrimaryDemo = $(obj).parent();//$(obj).parent().parent().parent().find(".hasSubLevel");
    //$(sPrimaryDemo).parent().parent().find(".Selected").removeClass("Selected");
    $(sPrimaryDemo).find(".Selected").removeClass("Selected");
    //$(obj).addClass("Selected");
    $(".rgt-cntrl-SubFilter-Conatianer").show();
    $(".rgt-cntrl-SubFilter-Conatianer").css("display", "inline-block");

    $("#rgt-cntrl-SubFilter2 .DemographicList").hide();
    $("#rgt-cntrl-SubFilter2").css("display", "inline-block");
    $("#rgt-cntrl-SubFilter2").show();
    $("#rgt-cntrl-SubFilter2 div[name='" + $(obj).find(".lft-popup-ele-label").attr("name") + "']").show();
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").text($(obj).find(".lft-popup-ele-label").attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").css("width", "286px");
    if ($(".rgt-cntrl-SubFilter-Conatianer").position().left > 850) {
        $(".rgt-cntrl-SubFilter-Conatianer").css("width", "auto");
        $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel1").css("width", "286px");
        $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").css("width", "286px");
        $("#rgt-cntrl-SubFilter2").css("width", "220px");
    }
    else {
        $(".rgt-cntrl-SubFilter-Conatianer").css("width", "auto");
        $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel1").css("width", "286px");
        $(".AdvancedFiltersDemoHeading #AdvFilterHeadingLevel2").css("width", "286px");
        $("#rgt-cntrl-SubFilter2").css("width", "240px");
    }
    DisplayHeightDynamicCalculation("VisistsFilter");
    SetScroll($("#rgt-cntrl-SubFilter2"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function SelectAdvfilters(obj) {
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    for (var i = 0; i < SelectedAdvFilterList.length; i++) {
        if (SelectedAdvFilterList[i].UniqueId == $(object).attr("uniqueid")) {
            sCurrentDemoId = i;
        }
    }

    if (sCurrentDemoId.toString() != "") {
        $(obj).removeClass("Selected");
        SelectedAdvFilterList.splice(sCurrentDemoId, 1);
    }
    else {
        $(obj).addClass("Selected");
        SelectedAdvFilterList.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("name"), DBName: $(object).attr("name"), parentName: $(object).attr("parentname").trim(), UniqueId: $(object).attr("uniqueid").trim() });
    }
    ShowSelectedFilters();
}
function RemoveAdvfilters(obj) {
    //var ObjData = $(".DemographicList [Fullname='" + $(obj).attr("Fullname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']").parent();
    //SelectAdvfilters(ObjData);
    var ObjData = $(".rgt-cntrl-SubFilter-Conatianer ul li span[uniqueid='" + $(obj).attr("uniqueid") + "']").parent();
    SelectAdvfilters(ObjData);
}
//End

//Channel for Beverages
//Start 31-01-18
//function LoadChannelFilters(data) {
//    html = "";
//    var index = 0;
//    //var ulclose = false;
//    if (data != null) {
//        for (var i = 0; i < data.ChannelFilterlist.length; i++) {
//            var object = data.ChannelFilterlist[i];
//            if (index == 0) {
//                html += "<ul>";
//                //ulclose = false;
//            }
//            if (object.Level == "1") {
//                html += "<li style=\"display:table;min-height:22px\">";
//                //html += "<div Name=\"" + object.Name + "\" class=\"\" onclick=\"DisplaySecondaryDemoFilter(this);\">" + object.Name + "</div>";
//                html += "<div onclick=\"DisplayChannelDemoFilter(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";

//                AllDemographics.push(object.Id + "|" + object.Name);;
//                html += "</li>";

//                //if (index == 1) {
//                //html += "</ul>";
//                //ulclose = true;
//                //}
//                index++;
//            }
//        }

//        //if (ulclose == false)
//        html += "</ul>";

//        //$(".rgt-cntrl-chnl").html("");
//        $("#adv-fltr-Chnl").append("<div class=\"lft-cntrl adv-fltr-Chnl Frequency\"><div class=\"adv-fltr-estblishment\"></div><div class=\"adv-fltr-text\">CHANNEL</div></div><div class=\"rgt-cntrl-chnl DemoLevel\"></div>");
//        var htmlCommon = "";
//        htmlCommon += "<div id=\"channel-Search-Content\" class=\"Search-Filter\" style=\"margin-top: 2%;width:269px;\"><div class=\"Search\"><input type=\"text\" id=\"Search-Channel\" class=\"txt-search ui-autocomplete-input\" name=\"Channel-Retailer-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"><div class=\"img-search\"></div></div></div>";

//        //htmlCommon += "<div class=\"ChannelDemoHeading AdvancedFiltersDemoHeading\">CHANNEL</div>";
//        htmlCommon += "<div class=\"ChannelDemoHeading AdvancedFiltersDemoHeading\"><div id=\"channelFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:233px;float:left;display:none;\">CHANNEL</div><div id=\"channelFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:233px;float:left;display:none;\">CHANNEL</div><div id=\"channelFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:233px;float:left;\">CHANNEL</div></div>";
        
//        $(".rgt-cntrl-chnl").append(htmlCommon);
//        $(".rgt-cntrl-chnl").append("<div id=\"rgt-cntrl-chnl-SubFilter3\" class=\"rgt-cntrl-SubFilter2  DemoLevel2 Lavel Lavel2\"></div><div id=\"rgt-cntrl-chnl-SubFilter2\"  class=\"rgt-cntrl-SubFilter2 DemoLevel2 Lavel Lavel2\"></div><div style=\"float:left;overflow-y:auto;height:63%;\"  id=\"rgt-cntrl-chnl-SubFilter1\" class=\"rgt-cntrl-SubFilter DemoLevel Lavel Lavel1\"></div>");
//        LoadSecondaryChannelFilters(data);
//    }
//}
//End

function LoadChannelFilters(data) {
    html = "";
    var index = 0;
    //var ulclose = false;
    //if (data != null && data.ChannelFilterlist != null && data.ChannelFilterlist.length > 0) {
    //for (var i = 0; i < data.ChannelFilterlist.length; i++) {
    //    var object = data.ChannelFilterlist[i];
    //    if (index == 0) {
    //        html += "<ul>";
    //        //ulclose = false;
    //    }
    //    if (object.Level == "1") {
    //        html += "<li style=\"display:table;min-height:32px\">";
    //        //html += "<div Name=\"" + object.Name + "\" class=\"\" onclick=\"DisplaySecondaryDemoFilter(this);\">" + object.Name + "</div>";
    //        html += "<div uniqueid=\"" + object.UniqueId + "\" onclick=\"DisplayChannelDemoFilter(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " data-parentid=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";

    //        AllDemographics.push(object.Id + "|" + object.Name);;
    //        html += "</li>";

    //        //if (index == 1) {
    //        //html += "</ul>";
    //        //ulclose = true;
    //        //}
    //        index++;
    //    }
    //}

    ////if (ulclose == false)
    //html += "</ul>";

    //$(".rgt-cntrl-chnl").html("");
    $("#adv-fltr-Chnl").append("<div class=\"lft-cntrl adv-fltr-Chnl Frequency\"><div class=\"adv-fltr-text\">WHERE PURCHASED</div></div><div class=\"rgt-cntrl-chnl DemoLevel\" style=\"overflow-x:scroll;\"></div>");
    var htmlCommon = "";
    htmlCommon += "<div id=\"channel-Search-Content\" class=\"Search-Filter\" style=\"margin-top:2%;width:269px;\"><div class=\"Search\"><input type=\"text\" id=\"Search-Channel\" class=\"txt-search ui-autocomplete-input\" name=\"Channel-Retailer-Search-Content\" placeholder=\"Search..\" autocomplete=\"off\"><div class=\"img-search\"></div></div></div>";
    //htmlCommon += "<div class=\"ChannelDemoHeading AdvancedFiltersDemoHeading\">CHANNEL</div>";
    //htmlCommon += "<div class=\"ChannelDemoHeading AdvancedFiltersDemoHeading\"><div id=\"channelFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;display:none;\">CHANNEL</div><div id=\"channelFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;display:none;\">CHANNEL</div><div id=\"channelFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;\">CHANNEL</div></div>";
    htmlCommon += "<div class=\"ChannelDemoHeading AdvancedFiltersDemoHeading\"><div id=\"channelFilterHeadingLevel1\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;\">CHANNEL</div><div id=\"channelFilterHeadingLevel2\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;display:none;\">CHANNEL</div><div id=\"channelFilterHeadingLevel3\" class=\"lft-popup-col-selected-text\" style=\"width:286px;float:left;display:none;\">CHANNEL</div></div>";
    $(".rgt-cntrl-chnl").append(htmlCommon);
    //$(".rgt-cntrl-chnl").append("<div style=\"height:69%;display:none;\" id=\"rgt-cntrl-chnl-SubFilter3\" class=\"rgt-cntrl-SubFilter2  DemoLevel2 Lavel Lavel2\"></div><div style=\"height:69%;display:none;\" id=\"rgt-cntrl-chnl-SubFilter2\"  class=\"rgt-cntrl-SubFilter2 DemoLevel2 Lavel Lavel2\"></div><div style=\"float:left;overflow-y:auto;height:69%;width:257px;\"  id=\"rgt-cntrl-chnl-SubFilter1\" class=\"rgt-cntrl-SubFilter DemoLevel Lavel Lavel1\"></div>");
    $(".rgt-cntrl-chnl").append("<div id=\"channel-content\" style=\"width:auto;height: 68%;\"><div style=\"float:left;overflow-y:auto;height:69%;width:257px;\"  id=\"rgt-cntrl-chnl-SubFilter1\" class=\"rgt-cntrl-SubFilter DemoLevel Lavel Lavel1\"></div><div style=\"float:left;height:69%;display:none;\" id=\"rgt-cntrl-chnl-SubFilter2\"  class=\"rgt-cntrl-SubFilter2 DemoLevel2 Lavel Lavel2\"></div><div style=\"float:left;height:69%;display:none;\" id=\"rgt-cntrl-chnl-SubFilter3\" class=\"rgt-cntrl-SubFilter2  DemoLevel2 Lavel Lavel2\"></div></div>");
    //LoadSecondaryChannelFilters(data);
    //}
}

function LoadSecondaryChannelFilters(data) {
    html = "";
    var thirdLevelhtml = "";
    var index = 0;

    if (data != null) {
        for (var i = 0; i < data.ChannelFilterlist.length; i++) {
            var object = data.ChannelFilterlist[i];
            if (index == 0) {
                html += "<ul>";
                //ulclose = false;
            }
            if (object.Level == "1") {
                html += "<li style=\"display:table;\">";

                if (object.Name.toUpperCase() == "CHANNEL VISITED" || object.Name.toUpperCase() == "RETAILER VISITED")
                    html += "<div id=\"" + object.Id + "\" onclick=\"DisplayChannelDemoFilter(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + " name=" + object.Name + " Fullname=" + object.Name + " data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                else {
                    html += "<div id=\"" + object.Id + "\" onclick=\"SelectChannel(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=" + object.Name + "  name=" + object.Name + " Fullname=" + object.Name + " dbname=\"" + object.DBName + "\" data-parent=\"\" data-isselectable=\"true\">" + object.Name + "</span></div>";
                    allChannels.push(object.Id + "|" + object.Name);
                }
                //AllDemographics.push(object.Id + "|" + object.Name);;
                html += "</li>";

                //if (index == 1) {
                //html += "</ul>";
                //ulclose = true;
                //}
                index++;
            }
        }

        //if (ulclose == false)
        html += "</ul>";
    }
    $("#rgt-cntrl-chnl-SubFilter1").html("");
    $("#rgt-cntrl-chnl-SubFilter1").html(html);

    html = "";
    if (data != null) {
        for (var i = 0; i < data.ChannelFilterlist.length; i++) {
            if (data.ChannelFilterlist[i].SecondaryAdvancedFilterlist.length > 0) {
                var index=0;
                
                
                for (var j = 0; j < data.ChannelFilterlist[i].SecondaryAdvancedFilterlist.length; j++) {
                    var object = data.ChannelFilterlist[i].SecondaryAdvancedFilterlist[j];
                    if (data.ChannelFilterlist[i].Level == "1") {
                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(sFilterData.ChannelFilterlist, function (u) {
                            return (u.Name.toUpperCase() == object.Name.toUpperCase() && u.Id.toString() == data.ChannelFilterlist[i].Id.toString());
                        });
                        if (k.length <= 0 && object.Name != "") {
                            if (index == 0)
                                html += "<div class=\"DemographicList\" id=\"" + data.ChannelFilterlist[i].Id + "\" Name=\"" + data.ChannelFilterlist[i].Name + "\" FullName=\"" + data.ChannelFilterlist[i].FullName + "\" style=\"overflow-y:auto;display:none;position:relative;\"><ul>";
                            // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                            html += "<div id=\"" + object.Id + "\" onclick=\"SelectChannel(this);\" class=\"lft-popup-ele\" style=\"\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" dbname=\"" + object.DBName + "\" data-isselectable=\"true\">" + object.Name + "</span></div>";
                            allChannels.push(object.Id + "|" + object.Name);
                        }
                        else {
                            if (object.Name != "") {
                                if (index == 0)
                                    html += "<div class=\"DemographicList\" id=\"" + data.ChannelFilterlist[i].Id + "\" Name=\"" + data.ChannelFilterlist[i].Name + "\" FullName=\"" + data.ChannelFilterlist[i].FullName + "\" style=\"overflow-y:auto;display:none;position:relative;\"><ul>";

                                html += "<div id=\"" + object.Id + "\" onclick=\"DisplayThirdChannelDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" data-isselectable=\"true\">" + object.Name + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                            }
                            }
                        }
                    else if (data.ChannelFilterlist[i].Level != "1") {
                        if (index == 0)
                            thirdLevelhtml += "<div class=\"DemographicList\" id=\"" + data.ChannelFilterlist[i].Id + "\" Name=\"" + data.ChannelFilterlist[i].Name + "\" FullName=\"" + data.ChannelFilterlist[i].FullName + "\" style=\"display:none;position:relative;\"><ul>";

                        thirdLevelhtml += "<div id=\"" + object.Id + "\" onclick=\"SelectChannel(this);\" class=\"lft-popup-ele\" style=\"\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" dbname=\"" + object.DBName + "\" data-isselectable=\"true\">" + object.Name + "</span></div>";
                        allChannels.push(object.Id + "|" + object.Name);
                    }
                    index++;
                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }

    $("#rgt-cntrl-chnl-SubFilter2").html("");
    $("#rgt-cntrl-chnl-SubFilter2").html(html);

    $("#rgt-cntrl-chnl-SubFilter3").html("");
    $("#rgt-cntrl-chnl-SubFilter3").html(thirdLevelhtml);

}
function DisplayChannelDemoFilter(obj) {
    $(".rgt-cntrl-chnl").css("width", "auto");
    var offset = $(obj).offset();
    var height = $(obj).height();
    var width = $(obj).width();
    var top = offset.top + height + "px";
    var right = offset.left + width + 2 + "px";
    if ($(obj).attr("name").length > 18) {
        $('.rgt-cntrl-chnl').css({
            'position': 'absolute',
            'left': offset.left - width - 10 + "px",
            'top': offset.top - 17,
        });
    }
    else {
        $('.rgt-cntrl-chnl').css({
            'position': 'absolute',
            'left': offset.left - width - 1 + "px",
            'top': offset.top - 17,
        });
    }
    $("#rgt-cntrl-chnl-SubFilter1 .DemographicList").hide();
    $("#rgt-cntrl-chnl-SubFilter2 .DemographicList").hide();
    $("#rgt-cntrl-chnl-SubFilter2").hide();
    $("#rgt-cntrl-chnl-SubFilter3").hide();
    $("#rgt-cntrl-chnl-SubFilter2").css("display", "inline-block");
    $("#rgt-cntrl-chnl-SubFilter2").show();
    $("#rgt-cntrl-chnl-SubFilter2 div[Id='" + $(obj).attr("Id") + "']").css("display", "inline-block");
    $("#rgt-cntrl-chnl-SubFilter2 div[Id='" + $(obj).attr("Id") + "']").show();
    $(".AdvancedFiltersDemoHeading #channelFilterHeadingLevel3").hide();
    $(".AdvancedFiltersDemoHeading #channelFilterHeadingLevel2").text($(obj).attr("name").toUpperCase());
    $(".AdvancedFiltersDemoHeading #channelFilterHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #channelFilterHeadingLevel2").css("width", "233px");
    SetScroll($("#rgt-cntrl-chnl-SubFilter2"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplayThirdChannelDemoFilter(obj) {
    $(".rgt-cntrl-chnl").css("width", "auto");
    $("#rgt-cntrl-chnl-SubFilter1 .DemographicList").hide();
    $("#rgt-cntrl-chnl-SubFilter3 .DemographicList").hide();
    $("#rgt-cntrl-chnl-SubFilter3").hide();
    $("#rgt-cntrl-chnl-SubFilter3").css("display", "inline-block");
    $("#rgt-cntrl-chnl-SubFilter3").show();
    $("#rgt-cntrl-chnl-SubFilter3 div[Name='" + $(obj).find(".lft-popup-ele-label").attr("Name") + "']").css("display", "inline-block");
    $("#rgt-cntrl-chnl-SubFilter3 div[Name='" + $(obj).find(".lft-popup-ele-label").attr("Name") + "']").show();
    $(".AdvancedFiltersDemoHeading #channelFilterHeadingLevel3").text($(obj).find(".lft-popup-ele-label").attr("Name").toUpperCase());
    $(".AdvancedFiltersDemoHeading #channelFilterHeadingLevel3").show();
    $(".AdvancedFiltersDemoHeading #channelFilterHeadingLevel3").css("width", "233px");
    SetScroll($("#rgt-cntrl-chnl-SubFilter3"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function SelectChannel(obj) {
    var object = $(obj).find(".lft-popup-ele-label");
    var sCurrentDemoId = "";
    for (var i = 0; i < selectedChannels.length; i++) {
        if (selectedChannels[i].Id == $(object).attr("id")) {
            sCurrentDemoId = i;
        }
    }

    if ($(obj).hasClass("Selected")) {
        $(obj).removeClass("Selected");
        selectedChannels.splice(sCurrentDemoId, 1);
    }
    else {
        $(obj).addClass("Selected");
        selectedChannels.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), FullName: $(object).attr("Fullname"), DBName: $(object).attr("dbname") });
    }
    ShowSelectedFilters();
}
function RemoveChannel(obj) {
    var ObjData = $(".rgt-cntrl-chnl * [Fullname='" + $(obj).attr("Fullname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']").parent();
    SelectChannel(ObjData);
}
//End

//Channel for Monthly Purchase
function LoadMonthlyFilters(data) {
   LoadSecondaryMonthlyFilters(data);
}
function LoadSecondaryMonthlyFilters(data) {
    html = "";
    var thirdLevelhtml = "";
    var index = 0;
    if (data != null) {
        for (var i = 0; i < data.MonthlyPurchaselist.length; i++) {
            var object = data.MonthlyPurchaselist[i];
            if (index == 0) {
                html += "<ul>";
            }
            if (object.LevelId == "1") {
                html += "<li Name=\"" + object.Name + "\" style=\"display:table;\">";

                if (object.Name.toUpperCase() == "FAVOURITE BRAND")
                    html += "<div id=\"" + object.Id + "\" onclick=\"DisplayMonthlyFilter(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" data-val=\"" + object.Name + "\" name=\"" + object.Name + "\" Fullname=\"" + object.Name + "\" data-parent=\"\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                else {
                    html += "<div id=\"" + object.Id + "\" onclick=\"SelectFrequency(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"display: table-cell;width: 216px;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" UniqueId=\"" + object.UniqueId + "\" data-val=\"" + object.Name + "\"  name=\"" + object.Name + "\" Fullname=\"" + object.Name + "\" data-parent=\"\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";
                    AllMonthly.push(object.Id + "|" + object.Name);
                }
                html += "</li>";
                index++;
            }
        }
        html += "</ul>";
    }

        $(".rgt-cntrl-frequency-Conatiner").html("");
        $(".rgt-cntrl-frequency-Conatiner").html(html);

    html = "";
    if (data != null) {
        for (var i = 0; i < data.MonthlyPurchaselist.length; i++) {
            if (data.MonthlyPurchaselist[i].MonthlyPurchseList.length > 0) {
                html += "<div class=\"DemographicList\" id=\"" + data.MonthlyPurchaselist[i].Id + "\" Name=\"" + data.MonthlyPurchaselist[i].Name + "\" FullName=\"" + data.MonthlyPurchaselist[i].FullName + "\" style=\"overflow-y:auto;display:none;position:relative;\"><ul>";
                thirdLevelhtml += "<div class=\"DemographicList\" id=\"" + data.MonthlyPurchaselist[i].Id + "\" Name=\"" + data.MonthlyPurchaselist[i].Name + "\" FullName=\"" + data.MonthlyPurchaselist[i].FullName + "\" style=\"display:none;position:relative;\"><ul>";
                for (var j = 0; j < data.MonthlyPurchaselist[i].MonthlyPurchseList.length; j++) {
                    var object = data.MonthlyPurchaselist[i].MonthlyPurchseList[j];
                    if (data.MonthlyPurchaselist[i].LevelId == "1") {

                        //if (data.AdvancedFilterlist[i].Name != "Other")
                        var k = _.filter(sFilterData.MonthlyPurchaselist, function (u) {
                            return u.Name.toUpperCase() == object.Name.toUpperCase();
                        });
                        if (k.length <= 0) {
                            // html += "<li class=\"Demography\" id=\"" + object.Id + "-" + object.MetricId + "-" + object.ParentId + "\" Name=\"" + object.Name + "\" FullName=\"" + object.FullName + "\" onclick=\"SelectDemographic(this);\">" + object.Name + "</li>";
                            html += "<div id=\"" + object.Id + "\" onclick=\"SelectFrequency(this);\" class=\"lft-popup-ele\" style=\"\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " UniqueId=\"" + object.UniqueId + "\" Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";
                            AllMonthly.push(object.Id + "|" + object.Name);
                        }
                        else
                            html += "<div id=\"" + object.Id + "\" onclick=\"DisplayThirdMonthlyDemoFilter(this);\" class=\"lft-popup-ele FilterStringContainerdiv\" style=\"\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span><div class=\"ArrowContainerdiv\"><span style=\"float:right;\" class=\"lft-popup-ele-next sidearrw\"></span></div></div>";
                    }
                    else if (data.MonthlyPurchaselist[i].LevelId != "1") {
                        thirdLevelhtml += "<div id=\"" + object.Id + "\" onclick=\"SelectFrequency(this);\" class=\"lft-popup-ele\" style=\"\"></span><span class=\"lft-popup-ele-label\" FullName=\"" + object.FullName + "\" id=" + object.Id + "-" + object.MetricId + "-" + object.ParentId + " Name=\"" + object.Name + "\" parent=\"" + object.ParentId + "\" data-isselectable=\"true\">" + object.Name.toLowerCase() + "</span></div>";
                        AllMonthly.push(object.Id + "|" + object.Name);
                    }
                    
                }
                html += "</ul></div>";
                thirdLevelhtml += "</ul></div>";
            }
        }
    }

    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").html("");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").html(html);

    $(".rgt-cntrl-frequency-Conatiner-SubLevel2").html("");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2").html(thirdLevelhtml);

}
function DisplayMonthlyFilter(obj) {
    $(".rgt-cntrl-chnl").css("width", "480px");
    var offset = $(obj).offset();
    var height = $(obj).height();
    var width = $(obj).width();
    var top = offset.top + height + "px";
    var right = offset.left + width + 2 + "px";
    if ($(obj).attr("name").length > 18) {
        $('.rgt-cntrl-frequency').css({
            'position': 'absolute',
            'left': offset.left - width - 10 + "px",
            'top': offset.top - 17,
        });
    }
    else {
        $('.rgt-cntrl-frequency').css({
            'position': 'absolute',
            'left': offset.left - width - 1 + "px",
            'top': offset.top - 17,
        });
    }
    $(".rgt-cntrl-frequency-Conatiner .DemographicList").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1 .DemographicList").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2").hide();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel3").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").css("display", "inline-block");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1").show();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1 div[Id='" + $(obj).attr("Id") + "']").css("display", "inline-block");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel1 div[Id='" + $(obj).attr("Id") + "']").show();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").text($(obj).attr("name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").show();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel2").css("width", "245px");
    SetScroll($(".rgt-cntrl-frequency-Conatiner-SubLevel1"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function DisplayThirdMonthlyDemoFilter(obj) {
    $(".rgt-cntrl-frequency").css("width", "740px");
    $(".rgt-cntrl-frequency-Conatiner .DemographicList").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2 .DemographicList").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2").hide();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2").css("display", "inline-block");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2").show();
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2 div[Name='" + $(obj).find(".lft-popup-ele-label").attr("Name") + "']").css("display", "inline-block");
    $(".rgt-cntrl-frequency-Conatiner-SubLevel2 div[Name='" + $(obj).find(".lft-popup-ele-label").attr("Name") + "']").show();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel3").text($(obj).find(".lft-popup-ele-label").attr("Name").toLowerCase());
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel3").show();
    $(".AdvancedFiltersDemoHeading #freqFilterHeadingLevel3").css("width", "245px");
    SetScroll($(".rgt-cntrl-frequency-Conatiner-SubLevel2"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
//End

//Beverage Selection
function LoadBeverageSlectionTypeFilter(data) {
    html = "";
    var sData = data.BeverageSelection;
    var index = 0;
    if (data != null) {
        for (var i = 0; i < sData.length; i++) {
            var object = sData[i];
            if (index == 0)
                html += "<ul>";

            html += "<li style=\"display:table;min-height:32px\">";
            html += "<div id=\"" + object.Id + "\" uniqueid=\"" + object.Id + "\" onclick=\"SelectBeverageSelectionType(this);\" Name=\"" + object.Name + "\" class=\"lft-popup-ele\" style=\"display: table-cell;\"><span class=\"lft-popup-ele-label\" id=\"" + object.Id + "\" Name=\"" + object.Name.toString() + "\" FullName=" + object.Name + " data-isselectable=\"true\">" + object.Name + "</span></div>";
            AllBevFrequency.push(object.Id + "|" + object.Name);
            html += "</li>";
            index++;
        }
        html += "</ul>";
        $(".rgt-cntrl-Selection-Conatiner").html("");
        $(".rgt-cntrl-Selection-Conatiner").append(html);
    }
}
function SelectBeverageSelectionType(obj) {
    var object = $(obj).find(".lft-popup-ele-label");
    $("#beverage-frequency ul li").removeClass("Selected");
    var sCurrentDemoId = "";
    sBevarageSelctionType = [];
    for (var i = 0; i < sBevarageSelctionType.length; i++) {
        if (sBevarageSelctionType[i].Id == $(object).attr("id")) {
            sCurrentDemoId = i;
        }
    }

    if (sCurrentDemoId.toString() != "") {
        $(obj).removeClass("Selected");
        sBevarageSelctionType.splice(sCurrentDemoId, 1);
    }
    else {
        $(obj).addClass("Selected");
        sBevarageSelctionType.push({ Id: $(object).attr("id"), Name: $(object).attr("name"), Params: $(object).attr("Params"), UniqueId: $(obj).attr("uniqueid") });
    }
    ShowSelectedFilters();
}
function RemoveBeverageSelectionType(obj) {
    var ObjData = $(".rgt-cntrl-chnl * [Fullname='" + $(obj).attr("Fullname") + "'][name='" + $(obj).attr("Name") + "'][id='" + $(obj).attr("Id") + "']").parent();
    SelectChannel(ObjData);
}
//End

function SearchFilters_RightPanel(SearchFor, SearchBox, AppendTo, data) {    
    data = data.getUnique();
    //$("#Search-Retailers").autocomplete({
    $("#" + SearchBox).autocomplete({
        delay: 0,
        minLength: 2,
        appendTo: "#" + AppendTo,//"#Retailer-Search-Content",
        autoFocus: false,
        position: {
            my: "left top",
            at: "left bottom",
            collision: "none"
        },
        open: function () {
             $(".Search-Filter .ui-widget-content").css("max-width", "300px");
            $(".Search-Filter .ui-widget-content").css("width", "231px");
            //$(".Search-Filter .ui-widget-content").css("max-width", "203px");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color","black");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("cursor","pointer");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("background-color","transparent");
            //$(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("border","solid 0.1px blue");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("padding","4px");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("margin-left","4px");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("margin-right","4px");
            //$(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "#7F7F7F");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("color", "black");
            $(".Search-Filter .ui-menu-item .ui-menu-item-wrapper").css("text-transform", "uppercase");
        },
        close: function (event, ui) {
            $(".txt-search").val("");
        },
        source: function (request, response) {
            var sArr = [];
            var sArray = _.map(data).map(function (x) {
                sArr.push({
                    label: x.split("|")[1],
                    value: x.split("|")[0],
                });
                return {
                    label: x.split("|")[1],
                    value: x.split("|")[0],
                };
            });
            //response(sArray);
            response($.ui.autocomplete.filter(sArray, request.term));
            return;
        },
        focus: function (event, ui) {
            this.value = ui.item.label;
            // or $('#autocomplete-input').val(ui.item.label);

            // Prevent the default focus behavior.
            event.preventDefault();
            // or return false;
        },
        select: function (e, ui) {
            //if (SearchFor == "Frequency" || SearchFor == "Monthly") {
            //    if (SearchFor == "Frequency")
            //    var ObjData = $(".rgt-cntrl-frequency-Conatiner span[name='" + ui.item.label + "']").parent();
            //    else
            //        var ObjData = $(".rgt-cntrl-frequency-Conatiner * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectFrequency(this);']");
            //    if (ObjData.length <= 0) {
            //        ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").parent();
            //        if (ObjData.length > 0) {
            //            var Obj = $(".rgt-cntrl-frequency-Conatiner span[name='" + $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").attr("parent") + "']").parent()
            //            DisplaySecondaryFrequency(Obj);
            //        }
            //    }
            //    if (ObjData.length <= 0)
            //        ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel2 * [name='" + ui.item.label + "']").parent();
            //    //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")
                
            //        SelectFrequency(ObjData);
            //    //else
            //    //    DisplaySecondaryFrequency(ObjData);
            //}
            //else if (SearchFor == "ordertype" || SearchFor == "Monthly") {
            //    if (SearchFor == "ordertype")
            //        var ObjData = $(".rgt-cntrl-ordertype-Conatiner span[name='" + ui.item.label + "']").parent();
            //    else
            //        var ObjData = $(".rgt-cntrl-ordertype-Conatiner * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectFrequency(this);']");
            //    if (ObjData.length <= 0) {
            //        ObjData = $(".rgt-cntrl-ordertype-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").parent();
            //        if (ObjData.length > 0) {
            //            var Obj = $(".rgt-cntrl-ordertype-Conatiner span[name='" + $(".rgt-cntrl-ordertype-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").attr("parent") + "']").parent()
            //            DisplaySecondaryFrequency(Obj);
            //        }
            //    }
            //    if (ObjData.length <= 0)
            //        ObjData = $(".rgt-cntrl-ordertype-Conatiner-SubLevel2 * [name='" + ui.item.label + "']").parent();
            //    //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")

            //    SelectFrequency(ObjData);
            //    //else
            //    //    DisplaySecondaryFrequency(ObjData);
            //}
            //else if (SearchFor == "AdvancedFilters") {

            //    var ObjData = [];
            //    if ($(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text() != "Other") {
            //        ObjData = $("#rgt-cntrl-SubFilter1 .DemographicList[uniqueId='" + ui.item.value + "']").parent();
            //        if (ObjData.length <= 0)
            //            ObjData = $("#rgt-cntrl-SubFilter1 .DemographicList ul div span[uniqueId='" + ui.item.value + "']").parent();
            //    }

            //    if (ObjData.length <= 0)
            //        ObjData = $("#rgt-cntrl-SubFilter2 .DemographicList ul div span[uniqueId='" + ui.item.value + "']").parent();
            //    if (ObjData.length <= 0)
            //        ObjData = $("#rgt-cntrl-SubFilter3 .DemographicList[uniqueId='" + ui.item.value + "']").parent();
            //    //if (ObjData[0].getAttribute('onclick') == "SelectAdvfilters(this);")
            //    SelectAdvfilters(ObjData);
            //}
            //else if (SearchFor == "Channel") {
            //    var ObjData = $("#rgt-cntrl-chnl-SubFilter1  div[name=\"" + ui.item.label + "\"]").parent();
            //    if (ObjData.length <= 0)
            //        ObjData = $("#rgt-cntrl-chnl-SubFilter2 div[name=\"" + ui.item.label + "\"]").parent();
            //    if (ObjData.length <= 0) {
            //        ObjData = $("#rgt-cntrl-chnl-SubFilter3 .DemographicList[name=\"" + ui.item.label + "\"]").parent();
            //        if(ObjData.length <= 0)
            //            ObjData = $("#rgt-cntrl-chnl-SubFilter3 .DemographicList span[name=\"" + ui.item.label + "\"]").parent();
            //    }
            //    SelectChannel(ObjData);
            //}
            //else if (SearchFor == "Left-Panel-Frequency") {
            //   $("#left-panel-frequency li div[name='" + ui.item.label + "']").trigger("click");
            //}
        }

    });
    $("#" + SearchBox).on("autocompleteselect", function (e, ui) {
        if (SearchFor == "Frequency" || SearchFor == "Monthly") {
            if (SearchFor == "Frequency")
            var ObjData = $(".rgt-cntrl-frequency-Conatiner span[name='" + ui.item.label + "']").parent();
            else
                var ObjData = $(".rgt-cntrl-frequency-Conatiner * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectFrequency(this);']");
            if (ObjData.length <= 0) {
                ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").parent();
                if (ObjData.length > 0) {
                    var Obj = $(".rgt-cntrl-frequency-Conatiner span[name='" + $(".rgt-cntrl-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").attr("parent") + "']").parent()
                    DisplaySecondaryFrequency(Obj);
                }
            }
            if (ObjData.length <= 0)
                ObjData = $(".rgt-cntrl-frequency-Conatiner-SubLevel2 * [name='" + ui.item.label + "']").parent();
            //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")

                SelectFrequency(ObjData);
            //else
            //    DisplaySecondaryFrequency(ObjData);
        }
        else if (SearchFor == "AllBevFrequency") {
            if (SearchFor == "AllBevFrequency")
                var ObjData = $(".rgt-cntrl-Selection-Conatiner span[id='" + ui.item.value + "']").parent();
            if (ObjData.length <= 0) {
                ObjData = $(".rgt-cntrl-Selection-Conatiner span[id='" + ui.item.value + "']").parent();
            }
            SelectBeverageSelectionType(ObjData);
        }
        else if (SearchFor == "Trips-Frequency") {
            if (SearchFor == "Trips-Frequency")
                var ObjData = $(".rgt-cntrl-trips-frequency-Conatiner span[name='" + ui.item.label + "']").parent();
            else
                var ObjData = $(".rgt-cntrl-trips-frequency-Conatiner * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectTripsFrequency(this);']");
            if (ObjData.length <= 0) {
                ObjData = $(".rgt-cntrl-trips-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").parent();
                if (ObjData.length > 0) {
                    var Obj = $(".rgt-cntrl-trips-frequency-Conatiner span[name='" + $(".rgt-cntrl-trips-frequency-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").attr("parent") + "']").parent()
                    DisplaySecondaryFrequency(Obj);
                }
            }
            if (ObjData.length <= 0)
                ObjData = $(".rgt-cntrl-trips-frequency-Conatiner-SubLevel2 * [name='" + ui.item.label + "']").parent();
            //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")

            SelectTripsFrequency(ObjData);
            //else
            //    DisplaySecondaryFrequency(ObjData);
        }
        else if (SearchFor == "ordertype" || SearchFor == "Monthly") {
            if (SearchFor == "ordertype")
                var ObjData = $(".rgt-cntrl-ordertype-Conatiner span[name='" + ui.item.label + "']").parent();
            else
                var ObjData = $(".rgt-cntrl-ordertype-Conatiner * [name='" + ui.item.label + "'][id='" + ui.item.value + "'][onClick='SelectFrequency(this);']");
            if (ObjData.length <= 0) {
                ObjData = $(".rgt-cntrl-ordertype-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").parent();
                if (ObjData.length > 0) {
                    var Obj = $(".rgt-cntrl-ordertype-Conatiner span[name='" + $(".rgt-cntrl-ordertype-Conatiner-SubLevel1 * [name='" + ui.item.label + "']").attr("parent") + "']").parent()
                    DisplaySecondaryFrequency(Obj);
                }
            }
            if (ObjData.length <= 0)
                ObjData = $(".rgt-cntrl-ordertype-Conatiner-SubLevel2 * [name='" + ui.item.label + "']").parent();
            //if (ObjData[0].getAttribute('onclick') == "SelectFrequency(this);")

            SelectFrequency(ObjData);
            if (SearchFor == "ordertype") {
                var offset = $(".adv-fltr-ordertype-container").offset();
                var height = $(".adv-fltr-ordertype-container").height();
                var width = $(".adv-fltr-ordertype-container").innerWidth();
                var top = offset.top + height;
                var offset1 = $(".adv-filters-wraper").offset();
                var height_wraper = $(".adv-filters-wraper").height();

                //var right = offset.left + width + 2 + "px";

                $('.rgt-cntrl-ordertype').css({
                    'position': 'absolute',
                    'left': offset.left - offset1.left,
                    'top': (height_wraper + 1),//($(this).height() + 1),// $(this).offset().top +
                });
                $(".rgt-cntrl-ordertype").css("display", "block");
                $(".rgt-cntrl-ordertype").show();
            }
            //else
            //    DisplaySecondaryFrequency(ObjData);
        }
        else if (SearchFor == "AdvancedFilters") {

            var ObjData = [];
            if ($(".rgt-cntrl-SubFilter-Conatianer .VisistsAdvancedFiltersDemoHeading").text() != "Other") {
                ObjData = $("#rgt-cntrl-SubFilter1 .DemographicList[uniqueId='" + ui.item.value + "']").parent();
                if (ObjData.length <= 0)
                    ObjData = $("#rgt-cntrl-SubFilter1 .DemographicList ul div span[uniqueId='" + ui.item.value + "']").parent();
            }

            if (ObjData.length <= 0)
                ObjData = $("#rgt-cntrl-SubFilter2 .DemographicList ul div span[uniqueId='" + ui.item.value + "']").parent();
            if (ObjData.length <= 0)
                ObjData = $("#rgt-cntrl-SubFilter3 .DemographicList[uniqueId='" + ui.item.value + "']").parent();
            //if (ObjData[0].getAttribute('onclick') == "SelectAdvfilters(this);")
            SelectAdvfilters(ObjData);
        }
        else if (SearchFor == "Channel") {
            var ObjData = $("#rgt-cntrl-chnl-SubFilter1  div[name=\"" + ui.item.label + "\"]").parent();
            if (ObjData.length <= 0)
                ObjData = $("#rgt-cntrl-chnl-SubFilter2 div[name=\"" + ui.item.label + "\"]").parent();
            if (ObjData.length <= 0) {
                ObjData = $("#rgt-cntrl-chnl-SubFilter3 .DemographicList[name=\"" + ui.item.label + "\"]").parent();
                if(ObjData.length <= 0)
                    ObjData = $("#rgt-cntrl-chnl-SubFilter3 .DemographicList span[name=\"" + ui.item.label + "\"]").parent();
            }
            SelectChannel(ObjData);
        }
        else if (SearchFor == "Left-Panel-Frequency") {
           $("#left-panel-frequency li div[name='" + ui.item.label + "']").trigger("click");
        }
        e.stopImmediatePropagation();
        $(".txt-search").val("");
    });
}

function GetRightPanelImagePosition(selection) {
    if (selection == 1)
    var RightPanelImagePosition = [
        { MetricName: "Item Purchased", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-364px -271px;" },
        { MetricName: "Daypart", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "16px -278px;" },
        { MetricName: "Day of Week", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-115px -273px;" },
        { MetricName: "Trip Mission", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-231px -267px;" },
        { MetricName: "Any Influential", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-479px -271px;" },
        { MetricName: "Most Influential", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-585px -271px;" },
        { MetricName: "Immediate Consumption", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-798px -271px;" },

        { MetricName: "Beverage Purchase", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-364px -271px;" },
        { MetricName: "NARTD", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-364px -271px;" },
        { MetricName: "Beverage Trips", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-364px -271px;" },

        { MetricName: "Frequency", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-631px -297px;" },
        { MetricName: "Channel", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-785px -297px;" },
        { MetricName: "Beverage Selection", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-840px -297px;" },
    ];
    else
    var RightPanelImagePosition = [
        { MetricName: "Item Purchased", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-420px -271px;" },
        { MetricName: "Daypart", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-52px -278px;" },
        { MetricName: "Day of Week", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-170px -273px;" },
        { MetricName: "Trip Mission", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-301px -267px;" },
        { MetricName: "Any Influential", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-534px -271px;" },
        { MetricName: "Most Influential", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-640px -271px;" },
        { MetricName: "Immediate Consumption", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-856px -271px;" },

        { MetricName: "Beverage Purchase", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-420px -271px;" },
        { MetricName: "NARTD", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-420px -271px;" },
        { MetricName: "Beverage Trips", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-420px -271px;" },

        { MetricName: "Frequency", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-631px -297px;" },
        { MetricName: "Channel", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-785px -297px;" },
        { MetricName: "Beverage Selection", imageName: "../../Images/sprite_filter_icons.svg", imagePosition: "-840px -297px;" },
    ];
    return RightPanelImagePosition;
}