$(document).ready(function () {
});

function prepareContentArea() {
    if (!Validate_CompareRetailers_Charts() || !Validate_SOAPMinGroup()) {
        return false;
    }
    GenerateSOAP();
}
function GenerateSOAP() {
    $("#SOAPLoader").show();
    $(".TranslucentDiv").show();
    $("#UpdateProgress").show();
    var param = new Object();
    if (SelectedFrequencyList.length > 0) {
        param.ShopperFrequency = SelectedFrequencyList[0].Name.toString();
        param.ShoppingFrequencyShortname = SelectedFrequencyList[0].Name.toString();
        param.ShopperFrequency_UniqueId = SelectedFrequencyList[0].UniqueId.toString();
    }
    
    param.TimePeriod = TimePeriod.toString();
    param.ShortTimePeriod = $(".timeType").val().toString();
    param.TimePeriod_UniqueId = TimePeriod_UniqueId.toString();
    if (Comparisonlist.length > 0) {
        param.ShopperSegment = Comparisonlist[0].LevelDesc + "|" + Comparisonlist[0].Name;
        param.ShopperSegmentShortName = Comparisonlist[0].Name.toString();
        param.Comparison_UniqueIds = Comparisonlist[0].UniqueId.toString();
    }
   
        Advanced_Filters_DBNames = [];
        Advanced_Filters_ShortNames = [];
    var Advanced_Filters_UniqueId = [];
        //Guest advanced filters
        for (var i = 0; i < SelectedDempgraphicList.length; i++) {
            Advanced_Filters_DBNames.push(SelectedDempgraphicList[i].Name);
            Advanced_Filters_ShortNames.push(SelectedDempgraphicList[i].Name);
            Advanced_Filters_UniqueId.push(SelectedDempgraphicList[i].UniqueId);
        }
        param.Filters = Advanced_Filters_DBNames.join("|").toString();
        param.FilterShortNames = Advanced_Filters_ShortNames.join("|").toString();
        param.Filter_UniqueId= Advanced_Filters_UniqueId.join("|").toString();

        if (Geographylist.length <= 0)
            $("#SecondaryGeographyFilterContent div[name='Geography'] ul div").eq(0).trigger("click");
        param.Geography = Geographylist[0].Name.toString();
        param.GeographyShortName = Geographylist[0].Name.toString();
        param.Geography_UniqueId = Geographylist[0].UniqueId.toString();

        var Group_DBNames = [];
        var Group_ShortNames = [];
        var Group_UniqueId = [];
        for (var i = 0; i < Grouplist.length; i++) {
            Group_DBNames.push(Grouplist[i].parentName + " |" + Grouplist[i].Name);
            Group_ShortNames.push(Grouplist[i].Name);
            Group_UniqueId.push(Grouplist[i].UniqueId);
        }
        param.ShopperGroup = Group_DBNames.join("|").toString();
        param.ShopperGroup = param.ShopperGroup.replace(/[" "]/g, "");
        param.Group_UniqueId = Group_UniqueId.join("|").toString();
        postBackData = "{param:" + JSON.stringify(param) + "}";
    jQuery.ajax({
        type: "POST",
        url: $("#URLSOAP").val(),
        data: postBackData,
        contentType: "application/json; charset=utf-8",
        success: function (content) {
            if (!isAuthenticated(content))
                return false;

            var localTime = new Date();
            var year = localTime.getFullYear();
            var month = localTime.getMonth() + 1;
            var date = localTime.getDate();
            var hours = localTime.getHours();
            var minutes = localTime.getMinutes();
            var seconds = localTime.getSeconds();
            // window.location.href = "Download.aspx?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
            var TimeDetails = new Object();
            TimeDetails.year = year;
            TimeDetails.month = month;
            TimeDetails.date = date;
            TimeDetails.hours = hours;
            TimeDetails.minutes = minutes;
            TimeDetails.seconds = seconds;
            postBackData = "{TimeDetails:" + JSON.stringify(TimeDetails) + "}";
            window.location.href = $("#URLAnalysis").val() + "/SOAPppt?year=" + year + "&month=" + month + "&date=" + date + "&hours=" + hours + "&minutes=" + minutes + "&seconds=" + seconds;
            $("#SOAPLoader").hide();
            $("#UpdateProgress").hide();
            $(".TranslucentDiv").hide();
        },
        error: function (error) {
            //showMessage("Hurray");
            $("#UpdateProgress").hide();
            $(".TranslucentDiv").hide();
            GoToErrorPage();
        }
    });
}