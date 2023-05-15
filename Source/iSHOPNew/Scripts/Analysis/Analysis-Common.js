function GetCurrentSPName() {
    if ($("#hdn-page").length > 0) {
        currentpage = $("#hdn-page").attr("name").toLowerCase();
    }
    switch (currentpage) {
        case "hdn-analysis-withinshopper": {
            if (sVisitsOrGuests == "2") {
                return "usp_AdvancedAcrossRetailerShopper";
                break;
            }
            else {
                return "usp_AdvancedAcrossRetailerTrip";
                break;
            }
            break;
        }
        case "hdn-analysis-withintrips": {
            if (sVisitsOrGuests == "2") {
                return "usp_AdvancedWithinRetailerShopper";
                break;
            }
            else {
                return "usp_AdvancedWithinRetailertrip";
                break;
            }
            break;
        }
    }
}
