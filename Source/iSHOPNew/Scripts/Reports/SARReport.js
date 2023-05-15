//let selectedCustomBaseOrBenchMark = ""
//let sarRetailerCustomBaseOrBenchMarkClicked = false
//let sarRetailerBenchmarkList = []
//let sarRetailerCustomList = []
//let sarCompetitorList = []
//let sarFrequencyList = {}
//let isVisitSar = true
let isRetailerMSS = false
let isCompetitorMSS = false
let competitorMSSArray = []
let isTripFilterSelected = false
let validateLeftPanelSar = () => {
    let validationFlag = true;
    if (sarRetailerBenchmarkList.length == 0) {
        validationFlag = false;
        showMessage("Please select 1 Main Retailer / Channel and 2 Comparision Retailer / Channel")
    }
    else if (sarRetailerCustomList.length != 2) {
        validationFlag = false;
        showMessage("Please select 2 Comparision Retailer / Channel")
    }
    else if (sarCompetitorList.length < 5) {
        validationFlag = false;
        showMessage("Please select minimum 5 Competitors")
    }
    return validationFlag;
}
let getTimeperiodType = () => {
    let id = ""
    let type = ""
    if (id.includes("MMT")) {
        if (id != "") {
            type = id.split(" ")[2]
        }
    }
    else if (id.includes("YTD")) {
        if (id != "") {
            type = id.split(" ")[0]
        }
    }
    else if (id.split(" ").length == 2 && id.includes("Q")) {
        if (id != "") {
            type = "QUARTER"
        }
    }
    else if (id!="") {
        if (id == "TOTAL TIME") {
            type = id
        }
        else {
            type = "YEAR"
        }
    }
    return type
}

let prepareSarReport = () => {
    let output = {}
    if ($(".timeType").val() != "") {
        //output.TimeperiodID = $(".timeType").val();
        output.TimeperiodID = TimePeriod_UniqueId;
        output.TimeperiodType = $(".timeType").val()
    }
    if (sarRetailerBenchmarkList.length != 0) {
        output.BenchMark = {
            //ID: sarRetailerBenchmarkList[0].Id,
            ID: sarRetailerBenchmarkList[0].ParentId,
            Name: sarRetailerBenchmarkList[0].Name,
            ParentName: sarRetailerBenchmarkList[0].parentName,
            UniqueFilterId: sarRetailerBenchmarkList[0].UniqueId,
            selectionType: sarRetailerBenchmarkList[0].ParentOfParent
        }
    }
    if (sarRetailerCustomList.length != 0) {
        output.CustomBase = []
        $.each(sarRetailerCustomList, (i, v) => {
            output.CustomBase.push({
                //ID: v.Id,
                ID: v.ParentId,
                Name: v.Name,
                ParentName: v.parentName,
                UniqueFilterId: v.UniqueId,
                selectionType: v.ParentOfParent
            })
        })
    }
    if (sarCompetitorList.length != 0) {
        output.Competitors = []
        $.each(sarCompetitorList, (i, v) => {
            output.Competitors.push({
                ID: v.Id,
                Name: v.Name,
                ParentName: v.parentName,
                UniqueFilterId: v.UniqueId,
                selectionType: v.ParentOfParent
            })
        })
    }
    if (sarFrequencyList != {}) {
        output.Frequency = []
        $.each(sarFrequencyList, (i, v) => {
            output.Frequency.push({
                ID: v.Id,
                Name: v.Name,
                ParentName: v.parentName,
                UniqueFilterId: v.UniqueId,
                selectionType: i,
                FrequencyId: v.FrequencyId
            })
        })
    }
    let isTripFilter = false;
    isTripFilterSelected = false;
    if (SelectedDempgraphicList.length != 0) {
        output.Filters = []
        $.each(SelectedDempgraphicList, (i, v) => {
            if (v.isTripFilter == true || v.isTripFilter=="true") {
                isTripFilter = true;
            }
            output.Filters.push({
                ID: v.Id,
                Name: v.Name,
                ParentName: v.parentName,
                UniqueFilterId: v.UniqueId
            })
        })
    }
    output.IsTripsOrShopper = isVisitSar;
    output.IsTripFilter = isTripFilter;
    isTripFilterSelected = isTripFilter;
    return output;
}

let callSampleSizeForSar = () => {
    if (!validateLeftPanelSar()) {
        return;
    }
    
    let selection = prepareSarReport();
    let leftPanelData = "{leftPanelData:'" + JSON.stringify(selection) + "'}";
    isRetailerMSS = false
    isCompetitorMSS = false
    competitorMSSArray = []
    isColPaletCalledDirectly = false
    jQuery.ajax({
        type: "POST",
        url: $("#URLvalidateSampleSize").val(),
        async: true,
        data: JSON.stringify(selection),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data == undefined || data == null || data.length == 0) {
                if (isTripFilterSelected) {
                            showPopupOfSarTrip("trip")
                        }
                        else if(isNonPrioritySelected){
                            showPopupOfSarTrip("non-priority")
                        }
                       else{
                            showColorPallatePpup();
                            //isColPaletCalledDirectly = true;
                      }
                      isColPaletCalledDirectly = true;
                
            }
            else {
                data.forEach(function (i, v) {
                    if (i.IsUseDirectional == "MSS" && (i.SelectionType == "BenchMark" || i.SelectionType == "Custom")) {
                        isRetailerMSS = true;
                    }
                    else if (i.IsUseDirectional == "MSS" && (i.SelectionType == "Competitors")) {
                        competitorMSSArray.push(i)
                    }
                });
                let mssData = data.filter((value, index) =>value.IsUseDirectional == "MSS")
                data = data.filter((value, index) =>value.IsUseDirectional == "LSS");
                let competitorTripLowSample = data.filter((value, index) =>value.SelectionType == "Competitors" && value.IsUseDirectional!="MSS");
                if ((sarCompetitorList.length - competitorMSSArray.length - competitorTripLowSample.length) < 5)
                {
                    isCompetitorMSS = true
                }
                if (data.length == 0) {
                    if (isRetailerMSS == true && !(isTripFilterSelected || isNonPrioritySelected)) {
                        showPopupOfSarMSS();
                    }
                    else if (isRetailerMSS != true && (isTripFilterSelected || isNonPrioritySelected)) {
                        if (isTripFilterSelected) {
                            showPopupOfSarTrip("trip")
                        }
                        else {
                            showPopupOfSarTrip("non-priority")
                        }
                        isColPaletCalledDirectly = true;
                    }
                    else if (isRetailerMSS == true && (isTripFilterSelected || isNonPrioritySelected)) {
                        showPopupOfSarLSS(mssData,"MSS")
                    }
                    else {

                        showColorPallatePpup();
                        isColPaletCalledDirectly = true;
                    }

                }
                else {
                    showPopupOfSarLSS(data,"LSS");
                }
            }
            
           
        },
        error: function (xhr, status, error) {
            return false;
        }
    });
}

let showPopupOfSarLSS = (response,type) =>{
    let showProceedButton = true;
    let lssCompetitorCount = 0;
    let retMainHeaderAdded = false, compMainHeaderAdded = false;
    let appendPrevsPrd = "";
    let previousYear = null;
    let previousPeriod = null;
    $.each(sarRetailerCustomList, (i, v) => {
        if (v.Name == "Previous Period") {
            previousPeriod = v;
        }
        else if (v.Name == "Previous Year") {
            previousYear = v;
        }
    })
    $.each(response, (i, v) => {
        if ((v.SelectionType == "BenchMark" || v.SelectionType == "Custom") ) {
            showProceedButton = false
        }
        else {
            lssCompetitorCount++
        }
    })
    if (((isTripFilterSelected || isNonPrioritySelected) && isRetailerMSS)) {
        showProceedButton = false
    }
    if (showProceedButton)
        $("#sar-report-popup .proceedClick").show();
    else
        $("#sar-report-popup .proceedClick").hide();

    if ((isTripFilterSelected || isNonPrioritySelected))
    {
        $("#sar-report-popup .heading_text").html("Trip Sample size is less than 30 for the following");
        $("#sar-report-popup .list-of-nulls").html("");
    }
    else {
        $("#sar-report-popup .heading_text").html("Sample size is less than 30 for the following");
        $("#sar-report-popup .list-of-nulls").html("");
    }
   
    if (response != null && response.length > 0) {
        $.each(response, (i, v) => {
            if (v.IsUseDirectional == type) {
                if ((v.SelectionType == "BenchMark" || v.SelectionType == "Custom") && !retMainHeaderAdded) {
                    $("#sar-report-popup .list-of-nulls").append("<div class='retchannelmainheader'>Main Retailer/Channel</div>");
                    retMainHeaderAdded = true;
                }
                else if (v.SelectionType == "Competitors" && !compMainHeaderAdded) {
                    $("#sar-report-popup .list-of-nulls").append("<div class='retchannelmainheader'>Competitors</div>");
                    compMainHeaderAdded = true;
                }
                if (v.TimePeriodType != "Current Period")
                    appendPrevsPrd = " - " + v.TimePeriodType;
                else
                    appendPrevsPrd = "";
                if (v.TimePeriodType == "Current Period") {
                    $("#sar-report-popup .list-of-nulls").append("<div class=\"stat-custdiv\" style=\"pointer-events:none\"><div class=\"stat-cust-dot\" style=\"display:inline-block;\"></div><div class=\"stat-cust-estabmt popup1\" parent-of-parent=\"" + v.SelectionType + "\" id=\"" + v.RetailerId + "\" uniqueid=\"" + v.UniqueFilterId + "\" name=\"" + v.RetailerName + "\">" + v.RetailerName + "</div></div>");
                }
                else if (v.TimePeriodType == "Previous Period" && (previousPeriod!=null))
                {
                    $("#sar-report-popup .list-of-nulls").append("<div class=\"stat-custdiv\" style=\"pointer-events:none\"><div class=\"stat-cust-dot\" style=\"display:inline-block;\"></div><div class=\"stat-cust-estabmt popup1\" parent-of-parent=\"" + previousPeriod.ParentOfParent + "\" id=\"" + previousPeriod.Id + "\" uniqueid=\"" + previousPeriod.UniqueId + "\" name=\"" + previousPeriod.Name + "\">" + v.RetailerName + appendPrevsPrd + "</div></div>");
                }
                else if (v.TimePeriodType == "Previous Year" && (previousYear != null)) {
                    $("#sar-report-popup .list-of-nulls").append("<div class=\"stat-custdiv\" style=\"pointer-events:none\"><div class=\"stat-cust-dot\" style=\"display:inline-block;\"></div><div class=\"stat-cust-estabmt popup1\" parent-of-parent=\"" + previousYear.ParentOfParent + "\" id=\"" + previousYear.Id + "\" uniqueid=\"" + previousYear.UniqueId + "\" name=\"" + previousYear.Name + "\">" + v.RetailerName + appendPrevsPrd + "</div></div>");
                }
                //$("#sar-report-popup .list-of-nulls").append("<div class=\"stat-custdiv\" style=\"pointer-events:none\"><div class=\"stat-cust-dot\" style=\"display:inline-block;\"></div><div class=\"stat-cust-estabmt popup1\" parent-of-parent=\"" + v.SelectionType + "\" id=\"" + v.RetailerId + "\" uniqueid=\"\" name=\"" + v.RetailerName + "\">" + v.RetailerName+appendPrevsPrd + "</div></div>");
            }
        })
    }
    $("#sar-report-popup").css("display", "block");
}

let showPopupOfSarTrip = (tripOrPriority) => {
    $("#sar-shopper-report-popup .stat-closediv #downlaodtext").css("display", "none");
    $("#sar-shopper-report-popup .stat-closediv #downlaodtext2").css("display", "none");
    $("#sar-shopper-report-popup .stat-closediv #downlaodtext3").css("display", "none");
    if (tripOrPriority == "trip") {
        $("#sar-shopper-report-popup .stat-closediv #downlaodtext2").css("display", "block");
    }
    else {
        $("#sar-shopper-report-popup .stat-closediv #downlaodtext3").css("display", "block");
    }
    $("#Translucent").css("z-index", "9000");
    $(".TranslucentDiv").show();
    $("#sar-shopper-report-popup .proceedClick").show();
    $("#sar-shopper-report-popup").css("display", "block");
}

let showPopupOfSarMSS = () => {
    //$("#sar-download-report-popup .proceedClick").show();
    //$("#sar-download-report-popup").css("display", "block");

    $("#sar-shopper-report-popup .stat-closediv #downlaodtext").css("display", "block");
    $("#sar-shopper-report-popup .stat-closediv #downlaodtext2").css("display", "none");
    $("#sar-shopper-report-popup .stat-closediv #downlaodtext3").css("display", "none");
    
    $("#Translucent").css("z-index", "9000");
    $(".TranslucentDiv").show();
    $("#sar-shopper-report-popup .proceedClick").show();
    $("#sar-shopper-report-popup").css("display", "block");

}

let showPopupOfSarCompMSS = () => {
    $("#Translucent").css("z-index", "9000");
    $(".TranslucentDiv").show();
    $("#sar-download-report-popup .proceedClick").show();
    $("#sar-download-report-popup").css("display", "block");
}


let CloseSarReportPopup = () => {
    $("#sar-report-popup").css("display", "none");
    $(".TranslucentDiv").hide();
    $("#Translucent").css("z-index", "1001");
}

let CloseSarDownReportPopup = () => {
    $("#sar-download-report-popup").css("display", "none");
    $(".TranslucentDiv").hide();
    $("#Translucent").css("z-index", "1001");
}

let CloseSarPopupMSS = () => {
    $("#sar-shopper-report-popup").css("display", "none");
    $(".TranslucentDiv").hide();
    $("#Translucent").css("z-index", "1001");
}

let ProceedSarRetailerMSS = () => {
    $("#sar-shopper-report-popup").hide();
    if (isCompetitorMSS) {
        showPopupOfSarCompMSS();
    }
    else {
        showColorPallatePpup()
    }
    
}

let ProceedSarGenerateReport = () => {
    $("#sar-report-popup").hide();
    $.each($("#sar-report-popup .list-of-nulls .stat-cust-estabmt"), (i, v) => {
        if ($($(v)[0]).attr("parent-of-parent") == "BenchMark" || $($(v)[0]).attr("parent-of-parent") == "Custom") {
            //RemoveSarRetailer($($(v)[0]));
            let objSelected = $("#SarRetailerDivId .Lavel[level!='level1'] ul li[id='" + $($(v)[0]).attr("id") + "'][parent-of-parent='" + $($(v)[0]).attr("parent-of-parent") + "']");
            SelectSarRetailer(objSelected);
            return false;
        }
        else if ($($(v)[0]).attr("parent-of-parent") == "Competitors") {
            //RemoveSarCompetitor($($(v)[0]));
            let objSelected = $("#SarCompetitorDivId .Lavel[level!='level1'] ul li[id='" + $($(v)[0]).attr("id") + "'][parent-of-parent='" + $($(v)[0]).attr("parent-of-parent") + "']");
            SelectSarCompetitor(objSelected);
        }
    })
    $("#sar-report-popup .list-of-nulls").html("");
    if (!validateLeftPanelSar()) {
        return;
    }
    //if ((sarCompetitorList.length - competitorMSSArray.length) < 5) {
    //    isCompetitorMSS = true
    //}

    //if (isRetailerMSS) {
    //    showPopupOfSarMSS()
    //}
    //else {
    //    showColorPallatePpup();
    //}
    if (isRetailerMSS) {
        showPopupOfSarMSS();
    }
    else if (isCompetitorMSS) {
        showPopupOfSarCompMSS();
    }
    else {
        showColorPallatePpup();
    }
}



let ProceedSarDownGenerateReport = () => {
    $("#sar-download-report-popup").hide();
    showColorPallatePpup();
}

let customcolrSubmit = function () {
    if (!validateLeftPanelSar()) {
        return;
    }
    $(".TranslucentDiv").hide();
    $("#Translucent").css("z-index", "1001");
    $(".custom-color-palte").hide();
    //let selection = prepareSarReport();
    //selection.ColorCode = $('.custom-estcolordiv').attr('colorcode');
    //let leftPanelData = "{leftPanelData:'" + JSON.stringify(selection) + "'}";
    //isRetailerMSS = false
    //isCompetitorMSS = false
    //competitorMSSArray = []    
    downloadSarReport();
    
}

let customcolrCancel = function () {
    $(".TranslucentDiv").hide();
    $("#Translucent").css("z-index", "1001");
    $(".custom-color-palte").hide();
    //$(".custom-color-palte,.transparentBG").hide();
}

let closeColorPopup = function () {
    $(".TranslucentDiv").hide();
    $("#Translucent").css("z-index", "1001");
    $(".custom-color-palte").hide();
    //$(".custom-color-palte,.transparentBG").hide();
}

let showColorPallatePpup = function () {
    

    //$(".loader,.transparentBG").hide();
    $('.customcolor-content').html("");
    //var selctdBenchMarkId = $('.filter-info-panel-lbl[parent-of-parent="BenchMark"]').attr("data-id");
    //var selctdBenchMarkText = $('.filter-info-panel-lbl[parent-of-parent="BenchMark"]').text().trim();
    var selctdBenchMarkId = sarRetailerBenchmarkList[0].Id;
    var selctdBenchMarkText = sarRetailerBenchmarkList[0].Name;
    var customcolorhtml = "";
    customcolorhtml += "<div class='customcolor-div'><div class='customcolor-est-labeldiv'>";
    customcolorhtml += "<div class='customcolor-est-label'>" + selctdBenchMarkText + "</div></div>";
    customcolorhtml += "<div class='customcolor-est-color'>";
    customcolorhtml += '<div class="custom-estcolordiv" id="' + selctdBenchMarkId + '"  text="' + selctdBenchMarkText + '" ></div>';
    customcolorhtml += "</div>";
    customcolorhtml += "</div>";
    $(".customcolor-content").append(customcolorhtml);
    assignfillcolorinpopup("Establishment");
    $(".custom-color-palte").show();
    $("#Translucent").css("z-index", "9000");
    $(".TranslucentDiv").show();
    //$(".transparentBG").show();
}

let downloadSarReport = () => {
    if (!validateLeftPanelSar()) {
        return;
    }
    let selection = prepareSarReport();
    selection.ColorCode = $('.custom-estcolordiv').attr('colorcode');
    selection.IsRetailerMSS = isRetailerMSS;
    selection.IsCompetitorMSS = isCompetitorMSS;
    selection.IsChannelSelected = isChannelSelected;
    selection.IsNonPrioritySelected = isNonPrioritySelected;
    selection.corporateOrChannelNetSelected = corporateOrChannelNetSelected;
    //spNames = ["USP_BrefingBook_Slide6_Shruti", "USP_BrefingBook_Slide7_Shruti", "USP_BrefingBook_Slide8_TopRetailers", "", "usp_BriefingBook_Slide10", "usp_BriefingBook_Slide11", "usp_BriefingBook_Slide12", "usp_BriefingBook_Slide13_Akshay", "", "usp_BriefingBook_Slide16", "usp_BriefingBook_Slide17_Akshay", "usp_BriefingBook_Slide18_Akshay", "usp_BriefingBook_Slide19", "usp_BriefingBook_Slide20", "", "usp_BriefingBook_ShopperFrequencyFunnel", "usp_BriefingBook_Shopper&TripShare", "usp_BriefingBook_Shopper&TripShare"]
    //spNames = ["USP_BriefingBook_FrequencySampleSize", "USP_BriefingBook_MarketShare", "USP_BriefingBook_TopRetailers", "", "usp_BriefingBook_Demographic", "usp_BriefingBook_ShopperSegment", "usp_BriefingBook_AttitudeSegment", "usp_BriefingBook_MonthlyShopper", "", "usp_BriefingBook_Path2Purchase", "usp_BriefingBook_ShopperPlan", "usp_BriefingBook_ShopperMotivates", "usp_BriefingBook_ShopperDifferentiates", "usp_BriefingBook_ShopperSatisfaction", "", "usp_BriefingBook_ShopperFrequencyFunnel", "usp_BriefingBook_Shopper&TripShare", "", "", "usp_BriefingBook_EstablishmentImageries", "usp_BriefingBook_EstablishmentStrengths", "usp_BriefingBook_GoodPlaceImageries", "usp_BriefingBook_GoodPlaceStrengths", "", "usp_BriefingBook_ImportantPerception", "", "", "USP_BriefingBook_MarketShare", "USP_BrefingBook_Slide34_Sabat", "USP_BriefingBook_MarketShare", "USP_BriefingBook_MarketShare", "", "", "USP_BrefingBook_Slide39_Sabat"]
    spNames = ["USP_BriefingBook_FrequencySampleSize", "USP_BriefingBook_MarketShare", "USP_BriefingBook_TopRetailers", "", "usp_BriefingBook_Demographic", "usp_BriefingBook_ShopperSegment", "usp_BriefingBook_AttitudeSegment", "usp_BriefingBook_MonthlyShopper", "", "usp_BriefingBook_Path2Purchase", "usp_BriefingBook_ShopperPlan", "usp_BriefingBook_ShopperMotivates", "usp_BriefingBook_ShopperDifferentiates", "usp_BriefingBook_ShopperSatisfaction", "", "usp_BriefingBook_ShopperFrequencyFunnel", "usp_BriefingBook_Shopper&TripShare", "", "", "usp_BriefingBook_EstablishmentImageries", "usp_BriefingBook_EstablishmentStrengths", "usp_BriefingBook_GoodPlaceImageries", "usp_BriefingBook_GoodPlaceStrengths", "", "usp_BriefingBook_ImportantPerception", "", "", "USP_BriefingBook_KeyBeverages", "USP_BriefingBook_Beverage_KeyCategory", "USP_BriefingBook_KeyBeverageManufacturers", "USP_BriefingBook_KeyBeveragePackageNets", "usp_BriefingBook_BeverageInstoreLocation", "", "", "usp_BriefingBook_ImagerySnapshot"]
    let leftPanelData = {leftPanelData: selection, spList: spNames };
    isRetailerMSS = false
    isCompetitorMSS = false
    competitorMSSArray = []
    jQuery.ajax({
        type: "POST",
        url: $("#URLdownloadSarReport").val(),
        async: true,
        data: JSON.stringify(leftPanelData),
        timeout:6000000,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            window.location.href = $("#URLdownloadSarReportFile").val() + "?path=" + data;


        },
        error: function (xhr, status, error) {
            return false;
        }
    });
}
