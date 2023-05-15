var Advanced_Filters_Names = [];

/// <reference path="../Layout/Layout.js" />
var ExportVal = "";
$(document).ready(function () {
    $("#ExportToExcel").click(function (e) {
        ExportVal = "EXCEL DOWNLOAD";
        prepareContentArea();
    });
    $("#ExportToPPT").click(function (e) {
        ExportVal = "PPT DOWNLOAD";
        prepareContentArea();
    });
});

function prepareContentArea() {
    $("#CrossTab4").show();
    if (!Validate_CompareRetailers_Charts()) {
        return false;
    }
    if (!Validate_CompareBeverages_Charts()) {
        return false;
    }
    
    GetISHOPBGMdata(ExportVal);
}

function GetBeverageDBName()
{
    var bevselectedname = "";
    for(var i=0;i<ComparisonBevlist.length;i++)
    {
        if (ComparisonBevlist[i].Name.toLocaleLowerCase() == $("#ddlbeverageitems option:selected").text().toLocaleLowerCase())
        {
            //if (TabType.toLocaleLowerCase() == "trips") 
            {
                if (ComparisonBevlist[i].Name != "" && ComparisonBevlist[i].Name != undefined) {
                    bevselectedname = $("#ddlbeverageitems option:selected").text();
                    break;
                    return;
                }
                else
                {
                    bevselectedname = $("#ddlbeverageitems option:selected").text();
                        break;
                        return;
                }
            }
            //else {
            //    if (ComparisonBevlist[i].Name != "") {
            //        bevselectedname = ComparisonBevlist[i].Name;//ComparisonBevlist[i].Name
            //        break;
            //        return;
            //    }
            //    else {
            //            bevselectedname = ComparisonBevlist[i].Name;
            //            break;
            //            return;                   
            //    }
            //}
        }
        else {
            if (ExportVal == "EXCEL DOWNLOAD") {
                bevselectedname = ComparisonBevlist[i].Name;
                break;
                return;
            }
        }
        
    }
    return bevselectedname;
}

function GetISHOPBGMdata(excelStatus) {
    var localTime = new Date();
    var year = localTime.getFullYear();
    var month = localTime.getMonth() + 1;
    var date = localTime.getDate();
    var hours = localTime.getHours();
    var minutes = localTime.getMinutes();
    var seconds = localTime.getSeconds();

   
    var param = new Object();
    Comparison_DBNames = [];
    Comparison_ShortNames = [];
    var Comparison_UniqueIds = [];
    var sCompList = [];
    for (var i = 0; i < Comparisonlist.length; i++) {
        if (TabType.toLocaleLowerCase() == "trips")
            Comparison_DBNames.push(Comparisonlist[i].LevelDesc + "|" + Comparisonlist[i].Name);
        else
            Comparison_DBNames.push(Comparisonlist[i].LevelDesc + "|" + Comparisonlist[i].Name);

        Comparison_ShortNames.push(Comparisonlist[i].Name);
        Comparison_UniqueIds.push(Comparisonlist[i].UniqueId);
        if (i != 0) {
            if (TabType.toLocaleLowerCase() == "trips")
                sCompList.push(Comparisonlist[i].Name);
            else
                sCompList.push(Comparisonlist[i].Name);
        }
    }
    param.Comparison_UniqueIds = Comparison_UniqueIds;
    param.BenchMark = Comparison_DBNames[0];
    param.BenchmarkShortName = Comparison_ShortNames[0];;
    param.timePeriod = TimePeriod;
    param.previoustimePeriod = PreviousTimePeriod;//$(".timeType").val();
    param.TimePeriod_UniqueId = TimePeriod_UniqueId.toString();

    if (SelectedFrequencyList.length > 0) {
        param.ShopperFrequency = SelectedFrequencyList[0].Name;
        param.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId;
    }
    param.selectionBevorNonBev = "";
    Advanced_Filters_DBNames = [];
    Advanced_Filters_ShortNames = [];
    Advanced_Filters_Names = [];
    Advanced_Filters_UniqueId = [];
    //Guest advanced filters
    for (var i = 0; i < SelectedDempgraphicList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].parentName + "|" + SelectedDempgraphicList[i].Name);
        Advanced_Filters_Names.push(SelectedDempgraphicList[i].Name);
        Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
    }
    //Visits advanced filters
    for (var i = 0; i < SelectedAdvFilterList.length; i++) {
        Advanced_Filters_DBNames.push(SelectedAdvFilterList[i].DBName);
        Advanced_Filters_ShortNames.push(SelectedAdvFilterList[i].parentName + "|" + SelectedAdvFilterList[i].Name);
        Advanced_Filters_Names.push(SelectedAdvFilterList[i].Name);
        Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
    }

    param.Filter_UniqueId = Advanced_Filters_UniqueId.join("|").toString();
    param.filter = Advanced_Filters_DBNames.join("|").toString();
    param.FilterShortNames = Advanced_Filters_Names.join(",").toString();
    param.timeType = $(".timeType").length > 1 ? $(".timeType")[1].value : $(".timeType").val();//$(".timeType").val();
    Comparison_DBNames = [];
    Comparison_ShortNames = [];
    Comparison_UniqueId = [];
    for (var i = 0; i < ComparisonBevlist.length; i++) {
        if (TabType.toLocaleLowerCase() == "trips") {
            Comparison_DBNames.push(ComparisonBevlist[i].LevelDesc + "||" + ComparisonBevlist[i].Name);
        }
        else {
            Comparison_DBNames.push(ComparisonBevlist[i].LevelDesc + "||" + ComparisonBevlist[i].Name);
        }
        Comparison_ShortNames.push(ComparisonBevlist[i].Name);
        Comparison_UniqueId.push(ComparisonBevlist[i].UniqueId);
    }    
    param.selectionBevorNonBev = Comparison_DBNames.join('|');
    param.BevorNonBevShortName = Comparison_ShortNames.join("|");
    param.Beverage_UniqueId = Comparison_UniqueId.join("|");
    var sDropDownValue = "";

    param.SelectedBevorNonBevShortName = GetBeverageDBName();

    param.BeverageNonBeveragelist = [];

    $("#ddlbeverageitems option").each(function () {
        param.BeverageNonBeveragelist.push($(this).text());
    });

    var postBackData = "{param:" + JSON.stringify(param) + "}"
    jQuery.ajax({
        type: "POST",
        url: $("#URLBGM").val(),
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!isAuthenticated(data))
                return false;

            PlotBGMTable(data);
            if (data != null && data.IsNoDataAvailable == false) {
                //SetBGMTableWidthHeight();
               
                //$("#UpdateProgress").hide();
                //$(".TranslucentDiv").hide();
                if (excelStatus == "EXCEL DOWNLOAD") {
                    window.location.href = $("#URLAnalysis").val() + "/" + "ExportExcel_BGM?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                    ExportVal = "";
                }
                else if (excelStatus == "PPT DOWNLOAD") {
                    window.location.href = $("#URLAnalysis").val() + "/" + "Export_To_PPT?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
                    ExportVal = "";
                }
                ExportVal = "";
            }

        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
}

// From Old iShop

var BevarageItem = "";
var BevarageshortName = "";

var NonBeverageItem = "";
var NonBeverageShortName = "";

var BeverageStoreId = "";
var NonBeverageStoreId = ""
var BGMBenchmark = "";

var BGMComparison = new Array();
var BGMstoreid = new Array();
var BGMdatabaseName = new Array();
var benchStoreId = "";
var AdvnedFltrTxtForExlBGM = "";
var BGMSelectedValue = "";

var tag = "";
var heightleftandRightBdy = "";
var widthleftPanel = "";

var BevarageItems = [];

var BenchmarkShortName = "";

var BGMLeftTablewidth = 385;

$(document).ready(function () {
    //$("#CrossTab4").width($("#rightPanel").width());
    BGMBenchmark = "";
    BGMComparison = new Array();
    BGMstoreid = new Array();
    BGMdatabaseName = new Array();

    var browser_width = $(window).width();
    widthleftPanel = browser_width - $("#leftPanel").width();
    heightleftandRightBdy = 57 + $(".leftheader").outerHeight() + $(".rightPanelTop").outerHeight() + $(".LowerRightContent").outerHeight(); //$("#tableRightHeader").outerHeight() + $(".rightPanelTop").outerHeight() + $(".LowerRightContent").outerHeight();

    $("#BevNonBev").hide();
    $("#BevNonBev input[type*='radio']").click(function () {
        $("#DropDowniSHOPBGMBeverageItem").hide();
        $("#DropDowniSHOPBGMNonBeverageItem").hide();
        if ($(this).parent("td").children("label").html() == "Beverage Item") {
            $("#DropDowniSHOPBGMBeverageItem").show();
        }
        else if ($(this).parent("td").children("label").html() == "Non Beverage Item") {
            $("#DropDowniSHOPBGMNonBeverageItem").show();
        }
    });
    //$("#ContentPlaceHolder1_rdoBeverageItem").click(function () {
    //    BevarageItem = "";
    //    $("#DropDowniSHOPBGMBeverageItem .liImgContainer").css("background-color", "#9B9B9B");
    //    $("#DropDowniSHOPBGMBeverageItem .liTextName").css("background-color", "#9B9B9B");
    //    $("#ShowiSHOPBGMBeverageRetailers").hide();
    //    BeverageStoreId = "";
    //    if (BeverageStoreId.indexOf("_") > -1) {
    //        $("#DropDowniSHOPBGMBeverageItem #Bench" + BeverageStoreId).css("background-color", "transparent");
    //    }
    //    $("#lblBeverages").show();
    //    $("#lblBeverages").html("Beverages : ");
    //    $("#beveragesValue").html(BevarageItem.replace("~","`"));
    //});

    // Changes for Left Panel Done by Bramhanath(25-09-2015)
    $("#rdoBeverageItem").click(function () {
        //
        $("#DropDowniSHOPBGMNonBeverageItem").hide();
        $("#DropDowniSHOPBGMBeverageItem").show();
        //$(".SecondLevel").css("display", "none");
        //
        BevarageItem = "";
        $("#DropDowniSHOPBGMBeverageItem .liImgContainer").css("background-color", "#9B9B9B");
        $("#DropDowniSHOPBGMBeverageItem .liTextName").css("background-color", "#9B9B9B");
        $("#ShowiSHOPBGMBeverageRetailers").hide();
        $("#ShowiSHOPBGMBeverageRetailersLevel1").html("").hide();
        $("#ShowiSHOPBGMBeverageRetailersLevel2").html("").hide();
        $("#ShowiSHOPBGMBeverageRetailersLevel3").html("").hide();
        $("#ShowiSHOPBGMBeverageRetailersLevel4").html("").hide();
        $("#ShowiSHOPBGMNonBeverageRetailersLevel1").html("").hide();
        $("#ShowiSHOPBGMNonBeverageRetailersLevel2").html("").hide();
        $("#ShowiSHOPBGMNonBeverageRetailersLevel3").html("").hide();
        $("#ShowiSHOPBGMNonBeverageRetailersLevel4").html("").hide();

        //BeverageStoreId = "";
        if (BeverageStoreId.indexOf("_") > -1) {
            $("#DropDowniSHOPBGMBeverageItem #Bench" + BeverageStoreId).css("background-color", "transparent");
        }
        $("#lblBeverages").show();
        $("#lblBeverages").html("Item : ");
        $("#beveragesValue").html(BevarageItem.replace("~", "`"));
    });
    //$("#ContentPlaceHolder1_rdoNonBeverageItem").click(function () {

    //    BevarageItem = "";
    //    $("#DropDowniSHOPBGMNonBeverageItem .contentboxFilter").css("background-color", "#9B9B9B");
    //    if (NonBeverageStoreId.indexOf("_") > -1) {
    //        $("#DropDowniSHOPBGMNonBeverageItem #filter" + NonBeverageStoreId).css("background-color", "transparent");
    //    }
    //    //$("#DropDowniSHOPBGMNonBeverageItem .text").css("background-color", "#9B9B9B");
    //    $("#lblBeverages").show();
    //    $("#lblBeverages").html("Non Beverages : ");
    //    $("#beveragesValue").html(BevarageItem.replace("~", "`"));
    //});

    $("#rdoNonBeverageItem").click(function () {

        //
        $("#DropDowniSHOPBGMNonBeverageItem").show();
        $("#DropDowniSHOPBGMBeverageItem").hide();
        //$(".SecondLevel").css("display", "block");

        //
        BevarageItem = "";
        $("#DropDowniSHOPBGMNonBeverageItem .contentboxFilter").css("background-color", "#9B9B9B");
        if (NonBeverageStoreId.indexOf("_") > -1) {
            $("#DropDowniSHOPBGMNonBeverageItem #filter" + NonBeverageStoreId).css("background-color", "transparent");
        }
        //$("#DropDowniSHOPBGMNonBeverageItem .text").css("background-color", "#9B9B9B");
        $("#lblBeverages").show();
        $("#lblBeverages").html("Item : ");
        $("#beveragesValue").html(BevarageItem.replace("~", "`"));
    });

    $("#DiviSHOPBGMBeverage").click(function () {

        cid = "true";
        $("#DropDowniSHOPBGMComparison").hide();
        $("#BevNonBev").toggle();
        if ($("#BevNonBev").css("display") == "block") {
            if ($("#ContentPlaceHolder1_rdoBeverageItem").is(":checked")) {
                $("#DropDowniSHOPBGMNonBeverageItem").hide();
                $("#DropDowniSHOPBGMBeverageItem").show();
            }
            else if ($("#ContentPlaceHolder1_rdoNonBeverageItem").is(":checked")) {
                $("#DropDowniSHOPBGMBeverageItem").hide();
                $("#DropDowniSHOPBGMNonBeverageItem").show();
                $("#DropDowniSHOPBGMNonBeverageItem .contentboxFilter").css("background-color", "#9B9B9B");
            }
        }
        else {
            $("#DropDowniSHOPBGMBeverageItem").hide();
            $("#DropDowniSHOPBGMNonBeverageItem").hide();
        }
    });

    $("#BevNonBev").click(function () {
        cid = "true";
    });

    $("#DropDowniSHOPBGMNonBeverageItem").click(function (e) {
        cid = "true";
    });
    $("#DropDowniSHOPBGMBeverageItem").click(function (e) {
        cid = "true";
        //$("#ShowiSHOPBGMBeverageRetailers").hide();
    });
    //$("#DiviSHOPBGMBenchmark").click(function (e) {

    //    $(".Benchretailercontentbox").css("background-color", "transparent");
    //    $(".liImgContainer").css("background-color", "#9B9B9B");
    //    $(".liTextName").css("background-color", "#9B9B9B");
    //    $("#Slide1 li").css("background-color", "transparent");

    //    $("#DropDownBenchSHOPBGMBenchmark div.liImgContainer").css("background-color", "#9B9B9B");
    //    $("#DropDownBenchSHOPBGMBenchmark div.liTextName").css("background-color", "#9B9B9B");

    //    if (benchStoreId.indexOf("_") > -1) {
    //        $("#Bench" + benchStoreId).css("background-color", "#FFCC00");
    //        $("#ShowiSHOPBGMBenchMarkRetailers").hide();
    //    }
    //    else {
    //        $("#Bench" + benchStoreId).css("background-color", "#FFCC00");
    //        $("#retailerBench" + benchStoreId).css("background-color", "#FFCC00");
    //        $("#ShowiSHOPBGMBenchMarkRetailers").hide();
    //    }

    //}
    //);
    $("#DiviSHOPBGMComparison").click(function (e) {
        storeid = BGMSelectedValue;
        //if (storeid.indexOf("_") > -1) {
        //if ($.inArray(storeid, BGMstoreid) > -1) {
        //    $("#" + storeid).css("background-color", "transparent");
        //    $("#Comp" + storeid).css("background-color", "#9B9B9B");
        //    $("#retailerComp" + storeid).css("background-color", "#9B9B9B");
        //}
        //else {
        //    $("#" + storeid).css("background-color", "#FFCC00");
        //    $("#Comp" + storeid).css("background-color", "#FFCC00");
        //    $("#retailerComp" + storeid).css("background-color", "#FFCC00");
        //}

        if ($.inArray(storeid, BGMstoreid) > -1) {
            $("#Comp" + storeid).css("background-color", "#FFCC00");
            $("#retailerComp" + storeid).css("background-color", "#FFCC00");
            for (i = 0; i < BGMstoreid.length; i++) {
                if (BGMstoreid[i].indexOf("_") == -1) {

                    $("#Comp" + BGMstoreid[i]).css("background-color", "#FFCC00");
                    $("#retailerComp" + BGMstoreid[i]).css("background-color", "#FFCC00");
                }

            }
        }
        else {
            $("#Comp" + storeid).css("background-color", "transparent");
            $("#retailerComp" + storeid).css("background-color", "transparent");
            for (i = 0; i < BGMstoreid.length; i++) {
                if (BGMstoreid[i].indexOf("_") == -1) {
                    $("#Comp" + BGMstoreid[i]).css("background-color", "#FFCC00");
                    $("#retailerComp" + BGMstoreid[i]).css("background-color", "#FFCC00");
                }

            }
        }
        //$("#ShowiSHOPBGMComparisonRetailers").hide();

    });

    // New Left Panel Chnages
    $("#rdoBeverageItem").addClass("addColors");
    $("#DropDowniSHOPBGMBeverageItem").toggle();
    $(".PurchaseItem").click(function () {
        $(".PurchaseItem").removeClass('addColors');
        $(this).addClass('addColors');
    });
    //
});

function BevPrev1() {
    $("#Bevnext1").css("background-color", "#686868");
    $("#Bevprev1").css("background-color", "#C3C3C3");
    if (slcount5 > 0) {
        if (slcount5 > 1) {
            $("#Bevprev1").css("background-color", "#686868");
        }
        slcount5--;
        position5 += 114;
        $("#BevSlide1").css("left", position5 + "px");
    }

}

function BevNext1() {
    $("#Bevprev1").css("background-color", "#686868");
    $("#Bevnext1").css("background-color", "#C3C3C3");
    var lng = $("#BevSlide1 li").length;
    if (slcount5 < lng - 3) {
        if (slcount5 < lng - 4) {
            $("#Bevnext1").css("background-color", "#686868");
        }
        slcount5++;
        position5 -= 114;
        $("#BevSlide1").css("left", position5 + "px");
    }
}

//second slider
function BevPrev2() {
    $("#Bevnext2").css("background-color", "#686868");
    $("#Bevprev2").css("background-color", "#C3C3C3");
    if (slcount6 > 0) {
        if (slcount6 > 1) {
            $("#Bevprev2").css("background-color", "#686868");
        }
        slcount6--;
        position6 += 114;
        $("#BevSlide2").css("left", position6 + "px");
    }

}

function BevNext2() {

    $("#Bevprev2").css("background-color", "#686868");
    $("#Bevnext2").css("background-color", "#C3C3C3");
    var lng = $("#BevSlide2 li").length;
    if (slcount6 < lng - 3) {
        if (slcount6 < lng - 4) {
            $("#Bevnext2").css("background-color", "#686868");
        }
        slcount6++;
        position6 -= 114;
        $("#BevSlide2").css("left", position6 + "px");
    }
}
//Added by Nagaraju for BGM PPT
var isPPTDownload = false;
function Export_To_PPT() {
    GetISHOPBGMdata("PPT DOWNLOAD");
}

//============================
//Benchmark & Comparison block
function ShowRespectiveBlockiSHOPBGM(Block, Module) {
    Block = "iSHOPBGM";
    Module = "BGM";
    var height = $(window).height();
    $("#MainTableContent").css("height", height - 112);
    BevarageItems = [];
    $("#bgmnote").hide();
    $("#ddlbeverageitems").html("");
    rightpanelmaxheight = rightpanelheight - 195;
    SetStatTesting(Module);
    clearAllBGM();
    $("#PPT").show();
    $(".liImgContainer").css("background-color", "#9B9B9B");
    $(".liTextName").css("background-color", "#9B9B9B");

    $(".Benchretailercontentbox").css("background-color", "transparent");
    $("#DropDowniSHOPBGMComparison .liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMComparison .liTextName").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMBeverageItem #Slide1 li").css("background-color", "transparent");
    $("#DropDownBenchSHOPBGMBenchmark .liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDownBenchSHOPBGMBenchmark .liTextName").css("background-color", "#9B9B9B");
    $(".Compretailercontentbox").css("background-color", "transparent");
    $(".contentboxFilter").css("background-color", "transparent");
    isChange = "true";
    $("#ShowTotalSelection").hide();
    //$(".BigImgContentSelector").css("background-color", "#374B57").css("border", "1px solid black").css("border-bottom", "none");
    $(".BigImgContentSelector").css("background-color", "#2A3A42").css("border", "1px solid black").css("border-bottom", "none");
    $("#Shopper").css("background-position", "-337px -544px");
    $("#Trips").css("background-position", "-533px -544px");

    $("#Shopping").css("background-position", "-409px -287px");
    $("#Perception").css("background-position", "-513px -280px");
    $("#BGM").css("background-position", "-719px -284px");
    $("#Total").css("background-position", "-617px -283px");
    $(".LowerRightContent").show();
    $("#CrossTab3").hide();
    $(".RightFilterContent").html("");
    //$("#Total").css("background-position", "-608px -284px");
    GetView(Block);
    $(".ShowResultBlock").hide();
    $(".ImgContentSelector").css("background-color", "#2A3A42").css("border", "1px solid black").css("border-bottom", "none");
    $("#ShowRespectiveBlock").children("div").hide();
    $("#ModuleSection div").live('click', function () {
        $(this).css("background-color", "#486370").css("border", "1px solid black");
    });



    $("#GetIshopBGMData").html("");
    $("#moduleType").html(Module);
    $("#" + Block).show();
    $(".rightPanelTop").show();
    $("#AnalysisType").html("");
    //$(".timeType").html("");
    TimePeriod = "total|total";
    //$(".timeType").html("AUG 2013 TO " + month[month.length - 1]); 
    $(".timeType").html("Oct 2013 3MMT TO " + month[month.length - 1]);
    //$("#ContentPlaceHolder1_RadioButton1").attr("disabled", true);
    $("#RadioButton1").attr("disabled", true);
    SelectTimePeriod('12MMT', 8, $("#RadioButton25"));
    //$("#ContentPlaceHolder1_RadioButton22").attr('checked', true);
    //$("#ContentPlaceHolder1_RadioButton28").attr('checked', true);
    $("#RadioButton28").removeClass("removeColorsForFrequency");
    $("#RadioButton28").addClass("addColorsForFrequency");
    $("#lblBeverages").css("display", "block");
    $("#beveragesValue").html("")
    $("#SubmitButton").show();
    $("#CrossTab4").show();


    frequencyName = "Weekly +";
    storeCompArrName.length = 0;
    benchmark = "";
    comparisonlist.length = 0;
    viewBlock = ""; //Added by Mehatab on 23rd Dec 2014

    //New LeftPanel Changes added by Bramhanath(24-09-2015)

    $(".FilterArea").hide();
    $(".arrow-left").hide();
    $("#DropDownComp").hide();
    $("#DropDownBench").hide();
    $("#DropDownType").hide();
    $("#DropDownSingleSelection").hide();
    $("#ChannelRetailerSelection").hide();
    $(".MeasureArea").hide();
    $("#AlertPopup").hide();
    $(".arrow-left").hide();
    $(".SliderContent").hide();
    $("#TranslucentDivRight").hide();
    $(".Selection").removeClass("colorForClick");
    $(".Benchmark_LabelBGM").html("");
    $("#bgmretailer").html("Retailer");
    $(".Comparison_LabelBGM").html("");
    //
    //added by Nagaraju for Quartly + default selection

    SelectFrequencyPerception('Quarterly +', 'Quarterly +', $("#frequencyContentBGM #Div1"));
    //

    $("#FrequencyBlockBGM").show();

    $("#NAnote").show();

    if (benchStoreId.indexOf("_") > -1) {
        $("#Bench" + benchStoreId).css("background-color", "#FFFFFF");
    }
    CreateBGMBlankTable();
    jQuery.ajax({
        type: "POST",
        url: "iSHOPBGM.aspx/LoadBenchmarkComparison",
        async: false,
        data: "{MainTagName:'AcrossShopper'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            //if (content.d != "")
            if (!isAuthenticated(content))
                return false;

            DisplayiSHOPBGMSelectedBenchComp(content.d);
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
    //BeverageItem
    jQuery.ajax({
        type: "POST",
        url: "iSHOPBGM.aspx/LoadBenchmarkComparison",
        async: false,
        data: "{MainTagName:'BeverageAcrossShopper'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            //if (content.d != "")
            DisplayiSHOPBGMBeverageSelectedBenchComp(content.d);
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
    //NonBeverageItem
    jQuery.ajax({
        type: "POST",
        url: "iSHOPBGM.aspx/NonBeverageItem",
        async: false,
        data: "{TagName:'ItemPurchased'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            //if (content.d != "")
            DisplayiSHOPBGMNonBeverageItem(content.d);
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
    //$.ajax({
    //    type: 'get',
    //    url: "ISHOPBGMData.aspx",
    //    //data: "View=" + view + "&Retailer=" + $("#RetailerName").html() + "&ViewType=" + viewtype + "&CurrentTimePeriod=" + $("#CurrentTimePeriod").html(),
    //    success: function (data) {
    //        $("#CrossTab4").html("");
    //        $("#CrossTab4").html(data);
    //    },
    //    cache: false
    //});
}
function DisplayiSHOPBGMNonBeverageItem(data) {
    //iSHOP BGM 
    $("#DropDowniSHOPBGMNonBeverageItem").html("");
    $("#DropDowniSHOPBGMNonBeverageItem").html(data.filterlist);
}
function DisplayiSHOPBGMSelectedBenchComp(data) {
    //iSHOP BGM 
    $("#DropDownBenchSHOPBGMBenchmark").html("");
    $("#DropDowniSHOPBGMComparison").html("");
    $("#DropDownBenchSHOPBGMBenchmark").html(data.benchmark);
    $("#DropDowniSHOPBGMComparison").html(data.comparison);
}
function DisplayiSHOPBGMBeverageSelectedBenchComp(data) {
    //iSHOP BGM 
    $("#DropDowniSHOPBGMBeverageItem").html("");
    $("#DropDowniSHOPBGMBeverageItem").html(data.benchmark);
}

function DisplayiSHOPBGMBenchmarkRetailer(tagname, storeid) {
    isChange = "true";
    $("#DropDownBenchSHOPBGMBenchmark .liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDownBenchSHOPBGMBenchmark .liTextName").css("background-color", "#9B9B9B");
    $("#DropDownBenchSHOPBGMBenchmark #Slide1 li").css("background-color", "transparent");

    $("#DropDownBenchSHOPBGMBenchmark #retailerBench" + storeid).css("background-color", "transparent");
    $("#DropDownBenchSHOPBGMBenchmark #Bench" + storeid).css("background-color", "transparent");
    $("#DropDownBenchSHOPBGMBenchmark #B" + storeid).css("background-color", "#FFFFFF");
    var postBackData = "{TagName:'" + tagname + "'}";

    jQuery.ajax({
        type: "POST",
        url: "iSHOPBGM.aspx/LoadRetailer",
        async: false,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            //if (content.d != "")
            loadiSHOPBGMBenchRetailer(content.d);
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
}

//Loading Retailer Of selected channel in Benchmark
function loadiSHOPBGMBenchRetailer(data) {
    //$("#ShowiSHOPBGMComparisonRetailers").html("").hide();
    //$("#ShowiSHOPBGMBenchMarkRetailers").html("").hide();
    //$("#ShowiSHOPBGMBenchMarkRetailers").html(data.benchmarklist).show();

    //Added by Bramhanath for Corporate Nets(22-09-2015)
    $("#ShowiSHOPBGMBenchMarkRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMBenchMarkRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMBenchMarkRetailersLevel3").html("").hide();
    $("#ShowiSHOPBGMComparisonRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMComparisonRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMComparisonRetailersLevel3").html("").hide();

    for (var i = 0; i < data.length; i++) {
        var levelcontent = data[i];
        $("#ShowiSHOPBGMBenchMarkRetailersLevel" + (i + 1)).html(levelcontent.benchmarklist).show();
    }
    //
    if (benchStoreId.indexOf("_") > -1) {

        $("#Bench" + benchStoreId).css("background-color", "#FFCC00");
    }
    else {

        $("#Bench" + benchStoreId).css("background-color", "#FFCC00");
        $("#retailerBench" + benchStoreId).css("background-color", "#FFCC00");
    }
}

function DisplayiSHOPBGMComparisonRetailer(tagname, storeid) {
    isChange = "true";
    $("#DropDowniSHOPBGMComparison #Slide2 li").css("background-color", "transparent");
    $("#DropDowniSHOPBGMComparison .liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMComparison .liTextName").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMComparison #" + storeid).css("background-color", "#FFFFFF");

    if (benchStoreId.indexOf("_") > -1) {

        $("#DropDowniSHOPBGMComparison #Bench" + benchStoreId).css("background-color", "#FFCC00");
    }
    else {
        $("#DropDowniSHOPBGMComparison #Bench" + benchStoreId).css("background-color", "#FFCC00");
        $("#DropDowniSHOPBGMComparison #retailerBench" + benchStoreId).css("background-color", "#FFCC00");
    }

    //Coloring of channels
    if ($.inArray(storeid, BGMstoreid) > -1) {
        $("#DropDowniSHOPBGMComparison #Comp" + storeid).css("background-color", "#FFCC00");
        $("#DropDowniSHOPBGMComparison #retailerComp" + storeid).css("background-color", "#FFCC00");
        for (i = 0; i < BGMstoreid.length; i++) {
            if (BGMstoreid[i].indexOf("_") == -1) {
                $("#DropDowniSHOPBGMComparison #Comp" + BGMstoreid[i]).css("background-color", "#FFCC00");
                $("#DropDowniSHOPBGMComparison #retailerComp" + BGMstoreid[i]).css("background-color", "#FFCC00");
            }
        }
    }
    else {
        $("#DropDowniSHOPBGMComparison #Comp" + storeid).css("background-color", "transparent");
        $("#DropDowniSHOPBGMComparison #retailerComp" + storeid).css("background-color", "transparent");
        for (i = 0; i < BGMstoreid.length; i++) {
            if (BGMstoreid[i].indexOf("_") == -1) {
                $("#DropDowniSHOPBGMComparison #Comp" + BGMstoreid[i]).css("background-color", "#FFCC00");
                $("#DropDowniSHOPBGMComparison #retailerComp" + BGMstoreid[i]).css("background-color", "#FFCC00");
            }
        }
    }
    var postBackData = "{TagName:'" + tagname + "'}";
    jQuery.ajax({
        type: "POST",
        url: "iSHOPBGM.aspx/LoadRetailer",
        async: false,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            //if (content.d != "")
            loadiSHOPBGMCompRetailer(content.d);
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
}
//Load retailer of selected channel in comparison
function loadiSHOPBGMCompRetailer(data) {
    //$("#ShowiSHOPBGMBenchMarkRetailers").html("").hide();
    //$("#ShowiSHOPBGMComparisonRetailers").html(data.comparisonlist).show();

    //Added by Bramhanath for Corporate Nets(22-09-2015)
    $("#ShowiSHOPBGMBenchMarkRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMBenchMarkRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMBenchMarkRetailersLevel3").html("").hide();
    $("#ShowiSHOPBGMComparisonRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMComparisonRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMComparisonRetailersLevel3").html("").hide();

    for (var i = 0; i < data.length; i++) {
        var levelcontent = data[i];
        $("#ShowiSHOPBGMComparisonRetailersLevel" + (i + 1)).html(levelcontent.comparisonlist).show();
    }
    for (i = 0; i < BGMstoreid.length; i++) {
        if (BGMstoreid[i].indexOf("_") == -1) {
            $("#" + BGMstoreid[i]).css("background-color", "#FFCC00");
        }
        else {
            $("#" + BGMstoreid[i]).css("background-color", "#FFCC00");
        }
    }
}

//=======================================================

//Beverage block
function DisplayiSHOPBGMBeverageRetailer(tagname, storeid) {
    isChange = "true";
    $("#DropDowniSHOPBGMBeverageItem .liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMBeverageItem .liTextName").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMBeverageItem .liImgContainer2").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMBeverageItem .liTextName2").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMBeverageItem #Slide1 li").css("background-color", "transparent");

    $("#DropDowniSHOPBGMBeverageItem #retailerBench" + storeid).css("background-color", "transparent");
    $("#DropDowniSHOPBGMBeverageItem #Bench" + storeid).css("background-color", "transparent");
    $("#DropDowniSHOPBGMBeverageItem #B" + storeid).css("background-color", "#FFFFFF");
    var postBackData = "{TagName:'" + tagname + "',MainTagName:'" + "BeverageAcrossShopper" + "'}";

    jQuery.ajax({
        type: "POST",
        url: "iSHOPBGM.aspx/LoadBeverage",
        async: false,
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            //if (content.d != "")
            loadiSHOPBGMBeverageRetailer(content.d);
        },
        error: function (error) {
            //showMessage(error);
            GoToErrorPage();
        }
    });
}

//Loading Retailer Of selected channel in Benchmark
function loadiSHOPBGMBeverageRetailer(data) {
    //$("#ShowiSHOPBGMBeverageRetailers").html("").hide();  
    //$("#ShowiSHOPBGMBeverageRetailers").html(data.benchmarklist).show();

    //Added by Bramhanath for Corporate Nets(22-09-2015)
    $("#ShowiSHOPBGMBeverageRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel3").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel4").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel3").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel4").html("").hide();

    for (var i = 0; i < data.length; i++) {
        var levelcontent = data[i];
        $("#ShowiSHOPBGMBeverageRetailersLevel" + (i + 1)).html(levelcontent.benchmarklist).show();
    }

    for (var i = 0; i < BevarageItems.length; i++) {
        if (BevarageItems[i].id.indexOf("_") > -1) {
            $("#Bench" + BevarageItems[i].id).css("background-color", "#FFCC00");
        }
        else {
            $("#DropDowniSHOPBGMBeverageItem #Bench" + BevarageItems[i].id).css("background-color", "#FFCC00");
            $("#DropDowniSHOPBGMBeverageItem #retailerBench" + BevarageItems[i].id).css("background-color", "#FFCC00");
        }
    }
}


//Load retailer of selected channel in comparison
function loadiSHOPBGMNonBeverageRetailer(data) {
    //$("#ShowiSHOPBGMNonBeverageRetailers").html("").hide();
    //$("#ShowiSHOPBGMNonBeverageRetailers").html(data.comparisonlist).show();

    //Added by Bramhanath for Corporate Nets(22-09-2015)
    $("#ShowiSHOPBGMBeverageRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel3").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel3").html("").hide();

    for (var i = 0; i < data.length; i++) {
        var levelcontent = data[i];
        $("#ShowiSHOPBGMNonBeverageRetailersLevel" + (i + 1)).html(levelcontent.comparisonlist).show();
    }
    for (i = 0; i < storeCompArr.length; i++) {
        if (storeCompArr[i].indexOf('_') > -1) {
            $("#" + storeCompArr[i]).css("background-color", "#FFCC00");
        }
    }
}

function SelectBeverageItem(retailer, storeid, databaseName) {

    //$("#DropDowniSHOPBGMBeverageItem .Benchretailercontentbox").css("background-color", "transparent");
    //if (ModuleBlock.indexOf('Within') == -1) {
    //    $("#DropDowniSHOPBGMBeverageItem .liImgContainer").css("background-color", "#9B9B9B");
    //    $("#DropDowniSHOPBGMBeverageItem .liTextName").css("background-color", "#9B9B9B");
    //}
    //$("#DropDowniSHOPBGMBeverageItem #Slide1 li").css("background-color", "transparent");

    //if (storeid.indexOf("_") > -1) {
    //    $("#DropDowniSHOPBGMBeverageItem #Bench" + storeid).css("background-color", "#FFCC00");
    //}
    //else {
    //    $("#DropDowniSHOPBGMBeverageItem #Bench" + storeid).css("background-color", "#FFCC00");
    //    $("#DropDowniSHOPBGMBeverageItem #retailerBench" + storeid).css("background-color", "#FFCC00");

    //    $("#ShowiSHOPBGMBeverageRetailers").hide();
    //    $("#ShowiSHOPBGMBeverageRetailersLevel1").html("").hide();
    //    $("#ShowiSHOPBGMBeverageRetailersLevel2").html("").hide();
    //    $("#ShowiSHOPBGMBeverageRetailersLevel3").html("").hide();
    //    $("#ShowiSHOPBGMBeverageRetailersLevel4").html("").hide();
    //    $("#ShowiSHOPBGMNonBeverageRetailersLevel1").html("").hide();
    //    $("#ShowiSHOPBGMNonBeverageRetailersLevel2").html("").hide();
    //    $("#ShowiSHOPBGMNonBeverageRetailersLevel3").html("").hide();
    //    $("#ShowiSHOPBGMNonBeverageRetailersLevel4").html("").hide();

    //}
    if (BevarageItems.length == 9 && BeverageItemContains(retailer, storeid, databaseName, BevarageItems) == false) {
        showMessage("YOU CAN MAKE UPTO 9 SELECTIONS");
        return false;
    }
    else if (DuplicateBeverageItem(retailer, storeid, databaseName, BevarageItems)) {
        showMessage("Already Selected.");
        return false;
    }


    BeverageStoreId = storeid;
    SelectedBeverageItem(retailer, storeid, databaseName, "Purchase");
}
function SelectedBeverageItem(ShortName, id, BevItem, ItemType) {
    if (BeverageItemContains(ShortName, id, BevItem, BevarageItems)) {
        var currentitem = GetCurrentBeverageItem(ShortName, id, BevItem, BevarageItems);
        BevarageItems.splice(currentitem, 1);
        if (id.indexOf("_") > -1) {
            $("#DropDowniSHOPBGMBeverageItem #Bench" + id).css("background-color", "transparent");
        }
        else {
            $("#DropDowniSHOPBGMBeverageItem #Bench" + id).css("background-color", "#9B9B9B");
            $("#DropDowniSHOPBGMBeverageItem #retailerBench" + id).css("background-color", "#9B9B9B");

            $("#ShowiSHOPBGMBeverageRetailers").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel1").html("").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel2").html("").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel3").html("").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel4").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel1").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel2").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel3").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel4").html("").hide();
        }
    }
    else if (BeverageItemContains(ShortName, id, BevItem, BevarageItems) == false) {
        BevarageItems.push({ ShortName: ShortName, id: id, BevItem: BevItem, ItemType: ItemType });
        if (id.indexOf("_") > -1) {
            $("#DropDowniSHOPBGMBeverageItem #Bench" + id).css("background-color", "#FFCC00");
        }
        else {
            $("#DropDowniSHOPBGMBeverageItem #Bench" + id).css("background-color", "#FFCC00");
            $("#DropDowniSHOPBGMBeverageItem #retailerBench" + id).css("background-color", "#FFCC00");

            $("#ShowiSHOPBGMBeverageRetailers").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel1").html("").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel2").html("").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel3").html("").hide();
            $("#ShowiSHOPBGMBeverageRetailersLevel4").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel1").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel2").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel3").html("").hide();
            $("#ShowiSHOPBGMNonBeverageRetailersLevel4").html("").hide();
        }
    }
    AddToBevarageDropDown()
}
function GetCurrentBeverageItem(ShortName, id, BevItem, Bevaragelist) {
    for (var i = 0; i < Bevaragelist.length; i++) {
        if (Bevaragelist[i].ShortName == ShortName
            && Bevaragelist[i].id == id
            && Bevaragelist[i].BevItem == BevItem) {
            return i;
            break;
        }
    }
}
function BeverageItemContains(ShortName, id, BevItem, Bevaragelist) {
    for (var i = 0; i < Bevaragelist.length; i++) {
        if (Bevaragelist[i].ShortName == ShortName
            && Bevaragelist[i].id == id
            && Bevaragelist[i].BevItem == BevItem) {
            return true;
            break;
        }
    }
    return false;
}
function DuplicateBeverageItem(ShortName, id, BevItem, Bevaragelist) {
    for (var i = 0; i < Bevaragelist.length; i++) {
        if (Bevaragelist[i].BevItem == BevItem && Bevaragelist[i].id != id) {
            return true;
            break;
        }
    }
    return false;
}
//function SelectedBeverageItem2(ShortName, id, BevItem) {
//    BevarageItem = "";
//    BevarageItem = BevItem;
//    BevarageshortName = ShortName;
//    $("#beveragesValue").html(BevarageshortName.replace("~", "`"));
//}

//Select NonBeverageItem 30--06-2014 
function SelectedNonBeverageItem(ShortName, id, BevItem, ItemType) {
    if (BeverageItemContains(ShortName, id, BevItem, BevarageItems)) {
        var currentitem = GetCurrentBeverageItem(ShortName, id, BevItem, BevarageItems);
        BevarageItems.splice(currentitem, 1);
        $("#DropDowniSHOPBGMNonBeverageItem #filter" + id).css("background-color", "transparent");
    }
    else if (BeverageItemContains(ShortName, id, BevItem, BevarageItems) == false) {
        BevarageItems.push({ ShortName: ShortName, id: id, BevItem: BevItem, ItemType: ItemType });
        $("#DropDowniSHOPBGMNonBeverageItem #filter" + id).css("background-color", "#FFCC00");
    }
    AddToBevarageDropDown();
}
function SelectNonBeverageItem(Value, DatabaseName, storeid) {
    //BevarageItem = "";
    //BevarageItem = Value;
    //BevarageshortName = Value;
    //$("#DropDowniSHOPBGMNonBeverageItem .Benchretailercontentbox").css("background-color", "transparent");
    //if (ModuleBlock.indexOf('Within') == -1) {
    //    $("#DropDowniSHOPBGMNonBeverageItem .contentboxFilter").css("background-color", "#9B9B9B");
    //}
    //$("#DropDowniSHOPBGMNonBeverageItem #Slide1 li").css("background-color", "transparent");

    //if (storeid.indexOf("_") > -1) {
    //    $("#DropDowniSHOPBGMNonBeverageItem #filter" + storeid).css("background-color", "#FFCC00");
    //}
    //else {
    //    $("#DropDowniSHOPBGMNonBeverageItem #filter" + storeid).css("background-color", "#FFCC00");
    //    $("#DropDowniSHOPBGMNonBeverageItem .text").css("background-color", "#FFCC00");
    //}
    //NonBeverageStoreId = storeid;

    //$("#beveragesValue").html(Value.replace("ItemPurchased|", "").replace("~", "`"));
    if (BevarageItems.length == 9 && BeverageItemContains(Value, storeid, DatabaseName, BevarageItems) == false) {
        showMessage("YOU CAN MAKE UPTO 9 SELECTIONS");
        return false;
    }
    BeverageStoreId = storeid;
    SelectedNonBeverageItem(Value, storeid, DatabaseName, "NonPurchase");
}

/*Getting ISHOPBGM Data */
function DeleteBeverageItem(retailer, storeid, databaseName, ItemType) {
    if (ItemType == "Purchase")
        SelectBeverageItem(retailer, storeid, databaseName);
    else if (ItemType == "NonPurchase")
        SelectNonBeverageItem(retailer, databaseName, storeid);
}

function SetBGMTableWidthHeight() {
    //set left table width
    var divwidth = ($("#rightPanel").outerWidth() - 3) - BGMLeftTablewidth;

    $("#CrossTab4 #bgmrighttablecontent").css("width", divwidth);
    $("#CrossTab4 #bgmrighttablecontent #bgmrighttablebodywrapercontent").css("width", divwidth);
    $("#CrossTab4 #bgmrighttablecontent #bgmrighttableheaderwrapercontent").css("width", divwidth);

    $("#CrossTab4 #bgmsamplesizetablecontent").css("width", "100%");
    $("#CrossTab4 #bgmsamplesizetablecontent table").css("width", "100%");

    $("#CrossTab4 #bgmlefttablecontent").height($("#CrossTab4").height() - ($("#bgmsamplesizetablecontent").height() + 25));
    $("#CrossTab4 #bgmrighttablecontent").height($("#CrossTab4").height() - ($("#bgmsamplesizetablecontent").height() + 25));

    $("#CrossTab4 #bgmlefttablecontent table").width(BGMLeftTablewidth);
    $("#CrossTab4 #bgmlefttablecontent #leftbgmtablebody").css("height", $("#CrossTab4").height() - (40 + $("#bgmsamplesizetablecontent").height()));
    $("#CrossTab4 #bgmrighttablecontent #rightbgmtablebody").css("height", $("#CrossTab4").height() - (40 + $("#bgmsamplesizetablecontent").height()));


    $("#CrossTab4 #bgmrighttablecontent #bgmrighttablebodywrapercontent").height($("#CrossTab4").height() - ($("#bgmsamplesizetablecontent").height() + 60));


    $("#CrossTab4 #bgmlefttablecontent #bgmlefttablebodywrapercontent").height($("#CrossTab4").height() - ($("#bgmsamplesizetablecontent").height() + 60));

    $("#CrossTab4 #bgmrighttablecontent table").css("width", "100%");
    $("#CrossTab4 #bgmsource").css("width", $("#CrossTab4").outerWidth() - 7);

    //set right table column width
    $("#CrossTab4 #bgmrighttablecontent #rightbgmtabletheader tr").eq(0).each(function () {
        $(this).children("td").each(function (i) {
            var td_width = $(this).innerWidth();
            $("#CrossTab4 #bgmrighttablecontent #rightbgmtablebody tr").each(function () {
                $(this).children("td").eq(i).width(td_width);
            });
        });
    });

    //set right table row height
    $("#CrossTab4 #bgmlefttablecontent #leftbgmtablebody tr").each(function (i) {
        var td_height = $(this).innerHeight();
        $(this).height(td_height);
        $("#CrossTab4 #bgmrighttablecontent #rightbgmtablebody tr").eq(i).height(td_height);

    });

    $("#rightPanel").css("overflow", "hidden");
    $("#CrossTab4 #bgmrighttablecontent").css("width", divwidth);
    $("#CrossTab4 #bgmrighttablecontent #bgmrighttablebodywrapercontent").css("width", divwidth);
    $("#CrossTab4 #bgmrighttablecontent #bgmrighttableheaderwrapercontent").css("width", divwidth);

    if ($("#CrossTab4 #bgmrighttablecontent #bgmrighttablebodywrapercontent").hasVerticalScrollBar()) {
        $("#CrossTab4 #bgmrighttablecontent #bgmrighttableheaderwrapercontent").width(divwidth - 16);
    }
    else {
        $("#CrossTab4 #bgmrighttablecontent #bgmrighttablebodywrapercontent").width(divwidth);
        $("#CrossTab4 #bgmrighttablecontent #bgmrighttableheaderwrapercontent").width(divwidth);
    }
}
(function ($) {
    $.fn.hasHorizontalScrollBar = function () {
        if (this[0] != undefined) {
            if (this[0].clientWidth < this[0].scrollWidth) {
                return true
            } else {
                return false
            }
        }
        else {
            return false
        }
    }
})(jQuery);
//Plot BGM Table
function PlotBGMTable(data) {
    if (data != null && data.IsNoDataAvailable == false) {
        //left content
        var tablecontent = "<div id=\"bgmcontent\" style=\"float:left;height: 100%;width:100%;background-color: #ededee;\">";
        tablecontent += "<div id=\"bgmsamplesizetablecontent\" style=\"float:left;width:100%;clear:both;height:20%;background-color: #ededee;\">";
        tablecontent += data.SampleSizeHeaderTable;
        tablecontent += data.SampleSizeBodyTable;
        tablecontent += "</div>";


        tablecontent += "<div id=\"bgmlefttablecontent\" style=\"float:left;width:45%;height: 68%;\">";
        tablecontent += data.LeftHeader;
        tablecontent += "<div id=\"bgmlefttablebodywrapercontent\" style=\"float:left;width:100%;height:88%;overflow:hidden;\">";
        tablecontent += data.LeftBody;
        tablecontent += "</div>";
        tablecontent += "</div>";
        
        //right content
        tablecontent += "<div id=\"bgmrighttablecontent\" style=\"float:left;width: 55%;height:68%;overflow:hidden;margin-top: 0.13%;\">";
        tablecontent += "<div id=\"bgmrighttableheaderwrapercontent\" style=\"float:left;width: 100%;overflow:hidden;height: 7.7%;margin-top: 1.75%;background-color: #ededee;\">";
        tablecontent += data.RightHeader;
        tablecontent += "</div>";

        tablecontent += "<div id=\"bgmrighttablebodywrapercontent\" onscroll=\"BGMreposVertical(this);\" style=\"float:left;width: 100%;height:88%;overflow:auto;\">";
        tablecontent += data.RightBody;
        tablecontent += "</div>";
        tablecontent += "</div>";

        tablecontent += "<div id=\"bgmsource\" style=\"float:left;width:99.5%;clear:both;height:20px;display: inline-flex;align-items: center;text-transform: uppercase;font-size: 10px;\">";
        tablecontent += "<ul style=\"width:100%;\">";
        tablecontent += "<li>Source: Coca-Cola iSHOP Shopper Metrics, </li>";
        tablecontent += "<li class=\"timeTypefooter\">" + $(".timeType").val() + "</li>";
        tablecontent += "<li> vs. YAGO, </li>";
        tablecontent += "<li class=\"Frequency_labelBGM\">" + SelectedFrequencyList[0].Name + "</li>";
        tablecontent += "<li>, </li>";
        tablecontent += "<li class=\"BGMRightFilterContent\">" + (Advanced_Filters_Names.join(",").toString() == "" ? "All Demographics" : Advanced_Filters_Names.join(",").toString()) + "</li>";
        tablecontent += "<li>, </li>";
        tablecontent += "<li>" + (Comparisonlist.length > 0 ? Comparisonlist[0].Name.toString() : "") + " </li>";
        tablecontent += "<li>Visits</li>";
        tablecontent += "</ul>";
        tablecontent += "</div>";

        //tablecontent += "<div id=\"bgmnote\" style=\"float:left;width:99.5%;clear:both;height:20px;font-size: 10px;text-align: left;font-family: Calibri;color: black;display: inline-flex;align-items: center;\">";
        //tablecontent += "<span style=\"margin-left:0.2%;\">";
        //tablecontent += "*MONTHLY+ PURCHASER IS ONLY AVAILABLE FOR THE FOLLOWING CATEGORIES: CSD, RTD COFFEE, RTD TEA, PROTEIN DRINKS, RTD SMOOTHIES, JUICE/JUICE DRINKS, PACKAGED WATER, SPOTS DRINKS, ENERGY SHOTS/DRINKS AND LIQUID FLAVOR ENHANCERS.";
        //tablecontent += "</span>";
        //tablecontent += "</div>";
        tablecontent += "</div>";
        $("#CrossTab4").html(tablecontent);
    }
}
function BGMreposVertical(e) {
    $("#bgmrighttableheaderwrapercontent").scrollTop(e.scrollTop);
    $("#bgmlefttablebodywrapercontent").scrollTop(e.scrollTop);
    $("#bgmrighttablebodywrapercontent").scrollTop(e.scrollTop);

    $("#bgmrighttableheaderwrapercontent").scrollLeft(e.scrollLeft);
    $("#bgmrighttablebodywrapercontent").scrollLeft(e.scrollLeft);
}
function GetPurchaseItemsDatabaseItems() {
    var bevdatabaselist = [];
    if (BevarageItems.length > 0) {
        for (var i = 0; i < BevarageItems.length; i++) {
            bevdatabaselist.push(BevarageItems[i].BevItem);
        }
    }
    return bevdatabaselist.join("|");
}

function ApplyBorder() {
    $("#CrossTab4 .lefttablebody tr").eq(0).css("border-top", "2px solid gray");
    $("#CrossTab4 .lefttablebody tr").eq(1).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .lefttablebody tr").eq(8).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .lefttablebody tr").eq(13).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .lefttablebody tr").eq(19).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .lefttablebody tr").eq(21).css("border-top", "2px solid gray");

    $("#CrossTab4 .righttablebody tr").eq(0).css("border-top", "2px solid gray");
    $("#CrossTab4 .righttablebody tr").eq(1).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .righttablebody tr").eq(8).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .righttablebody tr").eq(13).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .righttablebody tr").eq(19).css("border-bottom", "2px solid gray");
    $("#CrossTab4 .righttablebody tr").eq(21).css("border-top", "2px solid gray");
    $("#CrossTab4 .righttablebody tr").eq(27).css("border-bottom", "2px solid gray");

    $("#CrossTab4 .righttablebody tr").each(function () {
        $(this.children[4]).css("border-right", "2px solid gray");
        $(this.children[9]).css("border-right", "2px solid gray");
        $(this.children[14]).css("border-right", "2px solid gray");
        $(this.children[19]).css("border-right", "2px solid gray");
    });

    //Apply Color to Second Column

    //$("#CrossTab4 .lefttablebody tr td").eq(1).css("background-color", "rgb(197,217,241)");
    //$("#CrossTab4 .lefttablebody tr td").eq(2).css("background-color", "rgb(242,220,219)");

    //$("#CrossTab4 .lefttablebody tr td").eq(4).css("background-color", "rgb(216,228,188)");
    //$("#CrossTab4 .lefttablebody tr td").eq(5).css("background-color", "rgb(146,205,220)");
    //$("#CrossTab4 .lefttablebody tr td").eq(6).css("background-color", "rgb(242,220,219)");
    //$("#CrossTab4 .lefttablebody tr td").eq(7).css("background-color", "rgb(216,228,188)");
    //$("#CrossTab4 .lefttablebody tr td").eq(8).css("background-color", "rgb(242,220,219)");
    //$("#CrossTab4 .lefttablebody tr td").eq(9).css("background-color", "rgb(216,228,188)");
    //$("#CrossTab4 .lefttablebody tr td").eq(10).css("background-color", "rgb(146,205,220)");

    //$("#CrossTab4 .lefttablebody tr td").eq(12).css("background-color", "rgb(204,192,218)");
    //$("#CrossTab4 .lefttablebody tr td").eq(13).css("background-color", "rgb(252,213,180)");
    //$("#CrossTab4 .lefttablebody tr td").eq(14).css("background-color", "rgb(191,191,191)");
    //$("#CrossTab4 .lefttablebody tr td").eq(15).css("background-color", "rgb(191,191,191)");
    //$("#CrossTab4 .lefttablebody tr td").eq(16).css("background-color", "rgb(191,191,191)");

    //$("#CrossTab4 .lefttablebody tr td").eq(18).css("background-color", "rgb(242,220,219)");
    //$("#CrossTab4 .lefttablebody tr td").eq(19).css("background-color", "rgb(204,192,218)");
    //$("#CrossTab4 .lefttablebody tr td").eq(20).css("background-color", "rgb(216,228,188)");
    //$("#CrossTab4 .lefttablebody tr td").eq(21).css("background-color", "rgb(252,213,180)");
    //$("#CrossTab4 .lefttablebody tr td").eq(22).css("background-color", "rgb(146,205,220)");
    //$("#CrossTab4 .lefttablebody tr td").eq(23).css("background-color", "rgb(191,191,191)");
}
function AdjustWidthHeight() {
    //if ($(".showleftpanel").css("display") == "block") {
    //    $("#tdNAnote").css("margin-left", "900px");
    //}
    //else
    //    $("#tdNAnote").css("margin-left", "480px")
    $("#CrossTab4 .righttablecontent .righttableheader").css("table-layout", "fixed");
    $("#CrossTab4 .righttablecontent .righttablebody").css("table-layout", "fixed");

    $("#CrossTab4 .lefttablebody tr").each(function (lr) {
        var height = $(this).outerHeight();
        $(".righttablebody tr").eq(lr).height(height);
    });

    $("#CrossTab4 .righttablecontent .righttableheader tr").eq(2).children("td").each(function (rrhc) {
        var width = 150;
        $(this).css("font-size", "13px");
        $(".righttablecontent .righttablebody tr").each(function () {
            $(this).width($(".righttablecontent .righttableheader tr").eq(2).width());
            $(this).children("td").eq(rrhc).width(width);
        });
        $(this).width(width);
        $("#CrossTab4 .righttablecontent .righttableheader tr").eq(0).children("td").eq(rrhc).width(width);
    });
    //set table width
    var tablewidth = 0;
    var isspantrue = false;

    $(".righttablecontent .righttableheader tr").eq(1).children("td").each(function (rrhc) {
        var colspan = $(this).attr("colspan");
        if (colspan != undefined) {
            tablewidth += 800;
            isspantrue = true;
        }
        else {
            tablewidth += 150;
        }
    });

    //set height
    $("#CrossTab4 .righttablecontent .righttableheader tr").each(function (i) {
        var trheight = $(this).outerHeight();
        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).height(trheight);
        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).css("min-height", trheight);
        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).css("max-height", trheight);

        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).children("td").each(function () {
            $(this).height(trheight);
            $(this).css("min-height", trheight);
            $(this).css("max-height", trheight);
        });
        $("#CrossTab4 .righttablecontent .righttableheader tr").eq(i).children("td").each(function () {
            $(this).height(trheight);
            $(this).css("min-height", trheight);
            $(this).css("max-height", trheight);
        });
        $(this).height(trheight);
        $(this).css("min-height", trheight);
        $(this).css("max-height", trheight);
    });

    if (isspantrue) {
        $(".righttablecontent .righttableheader").width(tablewidth);
        $(".righttablecontent .righttablebody").width(tablewidth);

        $(".righttablecontent .righttableheader tr").eq(2).children("td").each(function (rrhc) {
            $(this).css("background-color", "#808080");
        });
    }


    var divwidth = $("#rightPanel").width() - (390);

    $("#CrossTab4 .leftbody").height(rightpanelmaxheight - 18);
    $("#CrossTab4 .righttbody").height(rightpanelmaxheight);

    if ($("#CrossTab4 .righttbody").hasVerticalScrollBar()) {
        $("#CrossTab4 .rightheader").width(divwidth - 18);
        $("#CrossTab4 .righttbody").width(divwidth);
    }
    else {
        $("#CrossTab4 .rightheader").width(divwidth);
        $("#CrossTab4 .righttbody").width(divwidth);
    }

    $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(($("#CrossTab4 .lefttablecontent .lefttableheader tr").length - 1)).children("td").eq(0).css("border-bottom", "1px solid #e41e2a");

}
//Scroll
function ScrollVerticalBGM(e) {
    $("#rightTableHeaderBGM").scrollTop(e.scrollTop);
    $("#leftTableBodyBGM").scrollTop(e.scrollTop);
    $("#rightTableBodyBGM").scrollTop(e.scrollTop);
    $("#rightTableHeaderBGM").scrollLeft(e.scrollLeft);
    $("#rightTableBodyBGM").scrollLeft(e.scrollLeft);
}

function ClearAdvancedFilter() {
    AdvancedFilterText = "";
    AdvancedFilterTextShortname = "";
    storeHeadFilter.length = 0;
    storeFilterId.length = 0;
    storeFilterName.length = 0;
    storeHeadFilterEmp = "";
    storeFilterIdEmp = "";
    storeFilterNameEmp = "";
    $('.contentboxFilter').attr('style', '');
    $(".RightFilterContent").html("");

    //added by Nagaraju to clear Custom Region filters
    $(".contentboxFilter").removeClass("filterBackColor");
    $(".contentboxFilter div").removeClass("filterBackColor");
    //
}

//added by Nagaraju
function CleanClass(_class) {
    _class = _class.replace(/[\s,./@#$%;&*~()+?]/g, "");
    return _class;
}
function reposVertical(e) {
    $("#CrossTab4 .rightheader").scrollTop(e.scrollTop);
    $("#CrossTab4 .leftbody").scrollTop(e.scrollTop);
    $("#CrossTab4 .rightbody").scrollTop(e.scrollTop);

    $("#CrossTab4 .rightheader").scrollLeft(e.scrollLeft);
    $("#CrossTab4 .rightbody").scrollLeft(e.scrollLeft);
}
//Create Blank Table
function CreateBGMBlankTable() {

    var height = $("#rightPanel").height() - 200;
    var width = $("#rightPanel").width() - 387;

    ////left table content
    //table = "<div class=\"lefttablecontent\" style=\"float:left;width:387px;\">";
    ////left table header
    //table += "<div class=\"leftheader\" style=\"clear:both;width:387px;\">";
    //table += "<table class=\"lefttableheader\" style=\"width:100%;\">";
    ////first row
    //table += "<tr>"
    //table += "<td style=\"background-color: #E41E2A;border-right:0;border-bottom:1px solid #E41E2A;height:20px;\" class=\"BenchComp\"></td>";
    //table += "</tr>"

    ////second row
    //table += "<tr>"
    //table += "<td style=\"background-color: #E41E2A;color:white;text-align: center;height: 40px;border-left: 1px solid #ffffff;\" class=\"ShoppingFrequencyheader\"></td>";
    //table += "</tr>"
    //table += "</table>";
    //table += "</div>";

    ////left table body
    //table += "<div class=\"leftbody\" style=\"clear:both;width:387px;height:" + height + "px;overflow:hidden;\">";
    //table += "<table class=\"lefttablebody\" style=\"width:100%;\">";
    ////first row
    //for (var i = 0; i < 15; i++) {
    //    table += "<tr>"
    //    table += "<td style=\"height:20px;border:1px solid lightgrey\"></td>";
    //    table += "</tr>"
    //}
    //table += "</table>";
    //table += "</div>";
    //table += "</div>";

    ////right table content
    //table += "<div class=\"righttablecontent\" style=\"float:left;width:" + (width - 25) + "px;\">";
    //table += "<div class=\"rightheader\" style=\"clear:both;width:" + width + "px;overflow: hidden;\">";

    //table += "<table class=\"righttableheader\" style=\"width:100%;table-layout:fixed;\">";
    ////first row
    //table += "<tr>"
    //table += "<td style=\"background-color: #E41E2A;border: 1px solid white; color: white; font-size: 16px; text-align: center; width: 200px;border-top:0;height: 20px;\" class=\"Benchmarktitle\">Benchmark</td>";
    //table += "<td style=\"background-color: #E41E2A;border: 0; color: white; font-size: 16px; text-align: center; width: 200px;height: 20px;\" class=\"comparisonheader\">Comaprison Areas</td>";
    //table += "</tr>"

    ////second row
    //table += "<tr>"
    //table += "<td style=\"background-color: #808080;height:40px;\" class=\"benchmarkheader\"></td>";
    //table += "<td style=\"background-color: #808080;border-left: 1px solid lightgrey;height:40px;\" class=\"comparisonheader\"></td>";
    //table += "</tr>"
    //table += "</table>";
    //table += "</div>";

    ////right table body
    //table += "<div onscroll=\"reposVertical(this);\" class=\"righttbody\" style=\"clear:both;width:" + width + "px;height:" + height + "px;overflow:auto;\">";
    //table += "<table class=\"righttablebody\" style=\"width:100%;table-layout:fixed;\">";
    ////first row
    //for (var i = 0; i < 15; i++) {
    //    table += "<tr>"
    //    table += "<td class=\"benchmarkcell\" style=\"height:20px;border:1px solid lightgrey;width:200px;\"></td>";
    //    table += "<td class=\"comparisoncell\" style=\"height:20px;border:1px solid lightgrey;width:200px;\"></td>";
    //    table += "</tr>"
    //}
    //table += "</table>";
    //table += "</div>";
    //table += "</div>";



    $("#CrossTab4").html($("#bgmtabledefaultdata").html());
}

(function ($) {
    $.fn.hasVerticalScrollBar = function () {
        if (this[0] != undefined) {
            if (this[0].clientHeight < this[0].scrollHeight) {
                return true
            } else {
                return false
            }
        }
        else {
            return false
        }
    }
})(jQuery);

function SelectBGMBenchmark(comparison, storeid, databaseName, tag) {

    benchStoreId = storeid;
    if (tag == "" || tag == null) {
        BGMBenchmark = databaseName;
    }
    else
        BGMBenchmark = databaseName + "|" + tag;

    if ($.inArray(storeid, BGMstoreid) > -1) {
        showMessage("Unable to select, this item already selected");
        return;
    }

    //if ((contains(comparison, BGMComparison)) || (databaseName == BGMComparison)) {
    //    showMessage("Unable to select, this item already selected");
    //    return;
    //}
    $(".Benchretailercontentbox").css("background-color", "transparent");
    $(".liImgContainer").css("background-color", "#9B9B9B");
    $(".liTextName").css("background-color", "#9B9B9B");
    $("#Slide1 li").css("background-color", "transparent");

    $("#DropDownBenchSHOPBGMBenchmark div.liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDownBenchSHOPBGMBenchmark div.liTextName").css("background-color", "#9B9B9B");

    if (storeid.indexOf("_") > -1) {
        $("#Bench" + storeid).css("background-color", "#FFCC00");
    }
    else {
        $("#Bench" + storeid).css("background-color", "#FFCC00");
        $("#retailerBench" + storeid).css("background-color", "#FFCC00");
        $("#ShowiSHOPBGMBenchMarkRetailers").hide();
    }
    $("#CrossTab4  .righttableheader tr").eq(1).children("td .benchmarkheader").html(comparison.replace("~", "`"));
    $(".Benchmark_LabelBGM").html(comparison.replace("~", "'"));
    $("#bgmretailer").html(comparison.replace("~", "'"));
    BenchmarkShortName = comparison;
}

function SelectBGMComparison(comparison, storeid, databaseName, tag) {

    if (tag == "" || tag == null) {
        databaseName = databaseName;
    }
    else
        databaseName = databaseName + "|" + tag;
    BGMSelectedValue = storeid;
    var colspan = $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(0).attr("colspan");
    var td = "";
    if ($.inArray(storeid, BGMComparison) == -1 && $.inArray(databaseName, BGMComparison) > -1) {
        showMessage("Unable to select, this item already selected");
        return;
    }
    if ((contains(BGMComparison, BGMBenchmark)) || (databaseName == BGMBenchmark)) {
        showMessage("Unable to select, this item already selected");
        return;
    }



    //$("#DropDowniSHOPBGMComparison .liImgContainer").css("background-color", "#9B9B9B");
    //$("#DropDowniSHOPBGMComparison .liTextName").css("background-color", "#9B9B9B");

    //if (Module == "iSHOPBGM") {


    var flg = false;
    $.each(BGMComparison, function (i, v) {
        if (v == comparison) {
            flg = true;
            return false;
        }
    });
    var channelretailerlimit = true;
    if (flg == false)
        channelretailerlimit = ValidateChannelsRetailersMaxLimit(databaseName);

    var htmltext = "";

    if (channelretailerlimit) {
        if (storeid.indexOf("_") > -1) {
            if ($.inArray(storeid, BGMstoreid) > -1) {
                $("#" + storeid).css("background-color", "transparent");
                $("#Comp" + storeid).css("background-color", "#9B9B9B");
                $("#retailerComp" + storeid).css("background-color", "#9B9B9B");
            }
            else {
                $("#" + storeid).css("background-color", "#FFCC00");
                $("#Comp" + storeid).css("background-color", "#FFCC00");
                $("#retailerComp" + storeid).css("background-color", "#FFCC00");
            }
        }
        else {
            if ($.inArray(storeid, BGMstoreid) > -1) {
                $("#Comp" + storeid).css("background-color", "#9B9B9B");
                $("#retailerComp" + storeid).css("background-color", "#9B9B9B");
            }
            else {

                $("#Comp" + storeid).css("background-color", "#FFCC00");
                $("#retailerComp" + storeid).css("background-color", "#FFCC00");
                //$("#ShowiSHOPBGMComparisonRetailers").hide();
            }
        }
        if ($.inArray(comparison, BGMComparison) == -1 && $.inArray(storeid, BGMstoreid == -1) == -1 && $.inArray(databaseName, BGMdatabaseName) == -1) {
            if (BGMComparison.length == 0 && BGMstoreid.length == 0 && BGMdatabaseName.length == 0) {

                $("#CrossTab4 .righttableheader tr").eq(1).children("td .comparisonheader").html(comparison.replace("~", "`"));

                $(".comparisonheader").attr('class', CleanClass(comparison) + "header");
                $(".comparisoncell").attr('class', CleanClass(comparison) + "cell");

            }
            else {
                td = "<td style=\"width:200px;border:1px solid white;\" class=\"" + CleanClass(comparison) + "header" + "\"></td>";
                $("#CrossTab4 .righttableheader tr").append(td);
                $("#CrossTab4 .righttableheader tr").eq(1).children("td ." + CleanClass(comparison) + "header").html(comparison);

                td = "<td style=\"width:200px;border:1px solid lightgrey;\" class=\"" + CleanClass(comparison) + "cell" + "\"></td>";
                $("#CrossTab4 .righttablebody tr").append(td);

                ////Added by Bramhanath for New Left Panel Comaprision Selections(22-09-2015)
                //for (i = 0; i < BGMComparison.length; i++) {
                //    htmltext += "<div class=\"" + "Comparison_LabelBGM " + CleanClass(BGMComparison[i].replace("'", "~")) + "cell\"" + ">" + BGMComparison[i] + "<a onclick=\"DeleteComparisonBGM('" + CleanClass(BGMComparison[i].replace("'", "~")) + "','" + BGMstoreid[i] + "')\" class=\"deleteComp\">X</a></div>";
                //}
                //$(".Comparison_LabelBGM").html(htmltext);
                ////
            }
            BGMComparison.push(comparison);
            BGMstoreid.push(storeid);
            BGMdatabaseName.push(databaseName);

            //Added by Bramhanath for New Left Panel Comaprision Selections(22-09-2015)
            for (i = 0; i < BGMComparison.length; i++) {
                htmltext += "<div class=\"" + "Comparison_LabelBGM " + CleanClass(BGMComparison[i].replace("'", "~")) + "cell\"" + ">" + BGMComparison[i].replace("~", "'") + "<a onclick=\"DeleteComparisonBGM('" + BGMComparison[i].replace("'", "~") + "','" + BGMstoreid[i] + "','" + BGMdatabaseName[i] + "')\" class=\"deleteComp\">X</a></div>";
            }
            $(".Comparison_LabelBGM").html(htmltext);
            //
            //BGMCompPriorityorNonProp.push(databaseName + "|" + tag);


        }
        else if ($.inArray(comparison, BGMComparison) > -1 && $.inArray(storeid, BGMstoreid) > -1 && $.inArray(databaseName, BGMdatabaseName) > -1) {
            $("#" + storeid).css("background-color", "transparent");
            $("#CrossTab4 .righttableheader tr ." + CleanClass(comparison) + "header").remove();

            $("#CrossTab4 .righttablebody tr ." + CleanClass(comparison) + "cell").remove();

            //Added by Bramhanath for New Left Panel Comaprision Selections(22-09-2015)
            $(".Comparison_LabelBGM ." + CleanClass(comparison) + "cell").remove();


            BGMComparison.splice($.inArray(comparison, BGMComparison), 1);
            BGMstoreid.splice($.inArray(storeid, BGMstoreid), 1);
            BGMdatabaseName.splice($.inArray(databaseName, BGMdatabaseName), 1);
            //BGMCompPriorityorNonProp.splice($.inArray(databaseName, BGMCompPriorityorNonProp), 1);

            if (BGMComparison.length == 0 && BGMstoreid.length == 0 && BGMdatabaseName.length == 0) {
                td = "<td class=\"comparisonheader\" style=\"width:200px;border: 1px solid white;\"></td>";
                $("#CrossTab4 .righttableheader tr").append(td);
                if (colspan != undefined) {
                    $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(5).attr("class", "comparisonheader");
                    $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(5).attr("class", "comparisonheader");
                    $("#CrossTab4 .righttableheader tr").eq(2).children("td").eq(($("#CrossTab4 .righttableheader tr").eq(2).children("td").length - 1)).attr("class", "comparisonheader");
                }
                else {
                    $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(1).attr("class", "comparisonheader");
                    $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(1).attr("class", "comparisonheader");
                    $("#CrossTab4 .righttableheader tr").eq(2).children("td").eq(($("#CrossTab4 .righttableheader tr").eq(2).children("td").length - 1)).attr("class", "comparisonheader");
                }
                td = "<td style=\"width:200px;border:1px solid lightgrey;\" class=\"" + CleanClass(comparison) + "cell" + "\"></td>";
                $("#CrossTab4 .righttablebody tr").append(td);
                if (colspan != undefined) {
                    $("#CrossTab4 .righttablebody tr td:nth-child(6)").attr("class", "comparisoncell");
                }
                else {
                    $("#CrossTab4 .righttablebody tr td:nth-child(2)").attr("class", "comparisoncell");
                }
            }
        }

        $("#CrossTab4 .righttableheader tr").eq(0).children("td").each(function (i) {
            if (i != 0)
                $(this).css("border", "none");
        });
        if (colspan != undefined) {
            $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(5).css("border-left", "1px solid white");
            $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(5).html("Comaprison Areas");
        }
        else {
            $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(1).css("border-left", "1px solid white");
            $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(1).html("Comaprison Areas");
        }
        SetTableHeaderDefaultHeight();
        AdjustWidthHeight();

    }
}

//Added by Bramhanath for New Left Panel Comaprision Deletions Selections(22-09-2015)
function DeleteComparisonBGM(comparison, storeid, databaseName) {
    var colspan = $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(0).attr("colspan");
    var td = "";
    if (storeid.indexOf("_") > -1) {
        if ($.inArray(storeid, BGMstoreid) > -1) {
            $("#" + storeid).css("background-color", "transparent");
            $("#Comp" + storeid).css("background-color", "#9B9B9B");
            $("#retailerComp" + storeid).css("background-color", "#9B9B9B");
        }
        else {
            $("#" + storeid).css("background-color", "#FFCC00");
            $("#Comp" + storeid).css("background-color", "#FFCC00");
            $("#retailerComp" + storeid).css("background-color", "#FFCC00");
        }
    }
    else {
        if ($.inArray(storeid, BGMstoreid) > -1) {
            $("#Comp" + storeid).css("background-color", "#9B9B9B");
            $("#retailerComp" + storeid).css("background-color", "#9B9B9B");
        }
        else {

            $("#Comp" + storeid).css("background-color", "#FFCC00");
            $("#retailerComp" + storeid).css("background-color", "#FFCC00");
            //$("#ShowiSHOPBGMComparisonRetailers").hide();
        }
    }

    $("#" + storeid).css("background-color", "transparent");
    $("#CrossTab4 .righttableheader tr ." + CleanClass(comparison) + "header").remove();

    $("#CrossTab4 .righttablebody tr ." + CleanClass(comparison) + "cell").remove();

    //Added by Bramhanath for New Left Panel Comaprision Selections(22-09-2015)
    $(".Comparison_LabelBGM ." + CleanClass(comparison) + "cell").remove();


    BGMComparison.splice($.inArray(comparison, BGMComparison), 1);
    BGMstoreid.splice($.inArray(storeid, BGMstoreid), 1);
    BGMdatabaseName.splice($.inArray(databaseName, BGMdatabaseName), 1);



    //td = "<td class=\"comparisonheader\" style=\"width:200px;border: 1px solid white;\"></td>";
    //$("#CrossTab4 .righttableheader tr").append(td);
    //if (colspan != undefined) {
    //    $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(5).attr("class", "comparisonheader");
    //    $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(5).attr("class", "comparisonheader");
    //    $("#CrossTab4 .righttableheader tr").eq(2).children("td").eq(($("#CrossTab4 .righttableheader tr").eq(2).children("td").length - 1)).attr("class", "comparisonheader");
    //}
    //else {
    //    $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(1).attr("class", "comparisonheader");
    //    $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(1).attr("class", "comparisonheader");
    //    $("#CrossTab4 .righttableheader tr").eq(2).children("td").eq(($("#CrossTab4 .righttableheader tr").eq(2).children("td").length - 1)).attr("class", "comparisonheader");
    //}
    //td = "<td style=\"width:200px;border:1px solid lightgrey;\" class=\"" + CleanClass(comparison) + "cell" + "\"></td>";
    // $("#CrossTab4 .righttablebody tr").append(td);
    //if (colspan != undefined) {
    //    $("#CrossTab4 .righttablebody tr td:nth-child(6)").attr("class", "comparisoncell");
    //}
    //else {
    //    $("#CrossTab4 .righttablebody tr td:nth-child(2)").attr("class", "comparisoncell");
    //}
    if (BGMComparison.length == 0 && BGMstoreid.length == 0 && BGMdatabaseName.length == 0) {
        td = "<td class=\"comparisonheader\" style=\"width:200px;border: 1px solid white;\"></td>";
        $("#CrossTab4 .righttableheader tr").append(td);
        if (colspan != undefined) {
            $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(5).attr("class", "comparisonheader");
            $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(5).attr("class", "comparisonheader");
            $("#CrossTab4 .righttableheader tr").eq(2).children("td").eq(($("#CrossTab4 .righttableheader tr").eq(2).children("td").length - 1)).attr("class", "comparisonheader");
        }
        else {
            $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(1).attr("class", "comparisonheader");
            $("#CrossTab4 .righttableheader tr").eq(1).children("td").eq(1).attr("class", "comparisonheader");
            $("#CrossTab4 .righttableheader tr").eq(2).children("td").eq(($("#CrossTab4 .righttableheader tr").eq(2).children("td").length - 1)).attr("class", "comparisonheader");
        }
        td = "<td style=\"width:200px;border:1px solid lightgrey;\" class=\"" + CleanClass(comparison) + "cell" + "\"></td>";
        $("#CrossTab4 .righttablebody tr").append(td);
        if (colspan != undefined) {
            $("#CrossTab4 .righttablebody tr td:nth-child(6)").attr("class", "comparisoncell");
        }
        else {
            $("#CrossTab4 .righttablebody tr td:nth-child(2)").attr("class", "comparisoncell");
        }
    }


    $("#CrossTab4 .righttableheader tr").eq(0).children("td").each(function (i) {
        if (i != 0)
            $(this).css("border", "none");
    });
    if (colspan != undefined) {
        $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(5).css("border-left", "1px solid white");
        $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(5).html("Comaprison Areas");
    }
    else {
        $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(1).css("border-left", "1px solid white");
        $("#CrossTab4 .righttableheader tr").eq(0).children("td").eq(1).html("Comaprison Areas");
    }
    SetTableHeaderDefaultHeight();
    AdjustWidthHeight()
}
function SetTableHeaderDefaultHeight() {
    //set height
    $("#CrossTab4 .righttablecontent .righttableheader tr").each(function (i) {
        var trheight = $(this).outerHeight();
        if (i == 0) {
            trheight = 20;
        }
        else {
            trheight = 40;
        }
        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).height(trheight);
        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).css("min-height", trheight);
        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).css("max-height", trheight);

        $("#CrossTab4 .lefttablecontent .lefttableheader tr").eq(i).children("td").each(function () {
            $(this).height(trheight);
            $(this).css("min-height", trheight);
            $(this).css("max-height", trheight);
        });
        $("#CrossTab4 .righttablecontent .righttableheader tr").eq(i).children("td").each(function () {
            $(this).height(trheight);
            $(this).css("min-height", trheight);
            $(this).css("max-height", trheight);
        });
        $(this).height(trheight);
        $(this).css("min-height", trheight);
        $(this).css("max-height", trheight);
    });
}

function ValidateChannelsRetailersMaxLimit(databaseName) {

    if (BGMComparison.length > 11 && !contains(BGMComparison, databaseName) && databaseName != "") {
        showMessage("YOU CAN MAKE UPTO 12 SELECTIONS");
        return false;
    }
    else { return true; }
}

function clearAllBGM() {
    $("#beveragesValue").html("");
    BevarageItem = "";
    BevarageItems = [];
    //TimePeriod = "total|total";
    //SelectTimePeriod('Total', 0);
    SelectTimePeriod('3MMT', 8, $("#RadioButton22"));
    BGMComparison.length = 0;
    BGMBenchmark = "";
    BGMComparison.length = 0;
    BGMstoreid.length = 0;
    BGMdatabaseName.length = 0;
    benchStoreId = "";
    $("#ContentPlaceHolder1_RadioButton22").attr('checked', true);
    $("#ContentPlaceHolder1_RadioButton28").attr('checked', true);
    $(".Benchretailercontentbox").css("background-color", "transparent");
    $("#DropDowniSHOPBGMComparison .liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMComparison .liTextName").css("background-color", "#9B9B9B");
    NonBeverageStoreId = "";
    BeverageStoreId = "";
    $("#DropDowniSHOPBGMBeverageItem .liImgContainer").css("background-color", "#9B9B9B");
    $("#DropDowniSHOPBGMBeverageItem .liTextName").css("background-color", "#9B9B9B");
    CreateBGMBlankTable();
    $(".liImgContainer").css("background-color", "#9B9B9B");
    $(".liTextName").css("background-color", "#9B9B9B");
    $(".Compretailercontentbox").css("background-color", "transparent");
    $(".contentboxFilter").css("background-color", "transparent");
    SelectFrequencyPerception("Weekly +", "Weekly +");
    $("#ContentPlaceHolder1_rdoBeverageItem").attr('checked', true);
    $("#lblBeverages").html("Item : ");
    $("#ShowiSHOPBGMComparisonRetailers").html("").hide();
    $("#ShowiSHOPBGMBenchMarkRetailers").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailers").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel3").html("").hide();
    $("#ShowiSHOPBGMBeverageRetailersLevel4").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel1").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel2").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel3").html("").hide();
    $("#ShowiSHOPBGMNonBeverageRetailersLevel4").html("").hide();

    ClearAdvancedFilter();
    //add default purchase items
    SelectBeverageItem('Total Non-Alcoholic RTD Beverages', '40_02', 'CategoryNet||Total Non-Alcoholic RTD Beverages');
    SelectBeverageItem('Carbonated Soft Drinks', '40_04', 'CategoryNet||Carbonated Soft Drinks');
    SelectBeverageItem('RTD Tea', '40_06', 'Category||Tea in a Bottle, Can, Carton or Fountain');
    SelectBeverageItem('100% Juice', '40_08', 'CategoryNet||100% Juice');
    SelectBeverageItem('Packaged Water', '40_10', 'CategoryNet||Total Packaged Water');
    SelectBeverageItem('Sports Drinks', '40_11', 'Category||Sports Drinks');
    SelectBeverageItem('Energy Drinks or Energy Shots', '40_12', 'Category||Energy Drinks or Energy Shots');
    SelectBeverageItem('Reg SSD', '40_15', 'Category||Regular Carbonated Soft Drinks');
    SelectBeverageItem('Diet SSD', '40_16', 'Category||Diet Carbonated Soft Drinks');
    $("#ddlbeverageitems").show();
}
