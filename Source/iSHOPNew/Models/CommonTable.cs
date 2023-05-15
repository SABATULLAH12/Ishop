using iSHOPNew.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace iSHOPNew.Models
{
    public class CommonTable
    {
        string View_Type = string.Empty;
        bool IsApplicable = true;
        int TabIndexId = 0;
        bool common_SampleSize = false;
        int table_count = 0;
        int rows_count = 0;
        public string BenchMark = string.Empty;
        public string ShopperSegment = string.Empty;
        public string ShopperFrequency = string.Empty;
        public string CheckString = string.Empty;
        public string TimePeriod = string.Empty;
        public Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, string> FilterTabs = new Dictionary<string, string>();
        public Dictionary<string, string> TableMappingList = new Dictionary<string, string>();
        public Dictionary<string, string> sampleSize = new Dictionary<string, string>();
        public Dictionary<string, string> DisplaysampleSize = new Dictionary<string, string>();
        public Dictionary<string, string> exportfiles = new Dictionary<string, string>();
        private string currentTab = string.Empty;
        public string average = string.Empty;

        Hashtable LoyaltyRetailerList = new Hashtable();

        public iSHOPParams param = new iSHOPParams();
        public iSHOPParams getvalue = new iSHOPParams();
        public string[] Retailerlist = null;
        public List<string> complist = new List<string>();
        public Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        public Dictionary<string, string> selectedsheets = new Dictionary<string, string>();
        public List<string> mergeCell = new List<string>();
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        private int colmaxwidth = 0;
        int cellfontstyle = 8;
        //Nishanth
        int cellfontstylegrey = 19;
        int samplecellstyle = 4;
        string activetab = string.Empty;
        DataSet demo = new DataSet();
        DataSet general = new DataSet();
        DataSet postshop = new DataSet();
        DataSet bdetail = new DataSet();
        public string userRole { get; set; }
        public string frequency = "";
        CommonFunctions _commonfunctions = new CommonFunctions();

        string leftheader = string.Empty;
        string leftbody = string.Empty;
        string rightheader = string.Empty;
        string righttbody = string.Empty;
        double ul_row_width = 0;
        double ul_cell_width = 0;
        bool LoyaltyPyramid = false;
        bool RetailerNetCheck = false;

        string LoyaltyPyramidmetric = string.Empty;

        string BenchmarkOrComparison;
        string SelectedStatTest = string.Empty;

        bool LoyaltyPyramidForRetailers = false;
        bool StoreImageryCheck = false;
        bool CheckRetailerorChannel = false;
        string NA_Text = string.Empty;
        List<string> BenchmarkorComparisionList;

        //Added by Bramhanath for BeverageTripsDetails Tab NA(09-12-2015)

        bool CheckBeverageTripNA = false;
        Hashtable CheckBeverageTripNAhTbl = new Hashtable();
        string checkBevTotalTrips = "totaltrip||totaltrip";
        int isBevTotalTrips = 0;
        //
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        bool isBeverageDetail = false;
        bool isLiquidFlavorEnhancer = false;
        //

        public CommonTable()
        {
            leftheader = string.Empty;
            leftbody = string.Empty;
            rightheader = string.Empty;
            righttbody = string.Empty;
        }
        private void PopulateShortNames()
        {
            HeaderTabs.Clear();

            HeaderTabs.Add("Store brand/private label (diet)", "Store brand/private label (DIET)");
            HeaderTabs.Add("Store brand/private label (regular)", "Store brand/private label (REGULAR)");
            HeaderTabs.Add("smartwater", "SmartWater");
            HeaderTabs.Add("vitaminwater", "Vitamin Water");
            HeaderTabs.Add("Gender ", "Gender");
            HeaderTabs.Add("FactAgeGroups", "Age");
            HeaderTabs.Add("FactAgeGender", "Age-Gender");
            HeaderTabs.Add("ItemsPurchased", "# of Items Purchased");
            HeaderTabs.Add("Ethnicity", "Race/Ethnicity");
            HeaderTabs.Add("HHTotal", "HH Size - Total");
            HeaderTabs.Add("HHAdults", "HH Size - Adults in HH");
            HeaderTabs.Add("HHChildren", "HH Size - Children in HH");
            HeaderTabs.Add("MaritalStatus", "Marital Status");
            HeaderTabs.Add("HHIncomeGroups", "HH Income");
            HeaderTabs.Add("EmployeeStatus2", "Employment Status");
            HeaderTabs.Add("Education ", "Education ");

            HeaderTabs.Add("DayParts", "Daypart of Trip");
            HeaderTabs.Add("DayofWeek", "Day of Week");
            HeaderTabs.Add("PreTripOrigin", "Pre-Trip Origin");
            HeaderTabs.Add("OtherStoreConsidered", "Consideration of Another Store");
            HeaderTabs.Add("ReasonForStoreChoice", "Reasons for Store Choice - Top 2 Box");
            HeaderTabs.Add("VisitPlans", "Pre Trip Planning");
            HeaderTabs.Add("VisitPreparation", "Preparation Types");
            HeaderTabs.Add("TechnologyUsed", "Use of Technology to Prepare");
            HeaderTabs.Add("ComputerBased", "Computer-Based Preparation Activities");
            HeaderTabs.Add("SmartPhoneBased", "Smartphone-Based Preparation Activities");
            HeaderTabs.Add("DestinationItemSummary", "Destination Item Summary");
            HeaderTabs.Add("DestinationItemDetails", "Destination Item Detail");

            HeaderTabs.Add("A supermarket or grocery store", "Supermarket / Grocery");
            HeaderTabs.Add("A convenience store or gas station food mart (excluding gas)", "Convenience");
            HeaderTabs.Add("A drug store", "Drug");
            HeaderTabs.Add("A dollar store such as Family Dollar or Dollar General", "Dollar");
            HeaderTabs.Add("A warehouse club such as Sam`s Club or Costco", "Club");
            HeaderTabs.Add("A Mass Merchandise store or super center such as walmart, target, walmart supercenter, or supertarget", "Mass Merc. with Supers");
            HeaderTabs.Add("A mass merchandise store without a full-line grocery section such as Walmart or Target", "Mass Merc.");
            HeaderTabs.Add("A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget", "Supercenter");

            HeaderTabs.Add("Shopper Attitude", "Top 2 Box Attitudinal Statements");
            HeaderTabs.Add("Attitudinal Segment", "Attitudinal Segment");

            HeaderTabs.Add("RetailerLoyaltyPyramid", "Retailer Loyalty Pyramid - Total Grocery Across Channel(Applicable only for Retailers)");
            HeaderTabs.Add("TopBox", "Loyalty and Satisfaction Detail(Applicable only for Retailers)");
            HeaderTabs.Add("MainFavoriteStore", "Main/Favorite Store for Grocery Spending(Applicable only for Retailers)");

            HeaderTabs.Add("shoppingpercent", "% HH Shopping Personally Responsible For");
            HeaderTabs.Add("SmartphoneTabletOwnership", "Smartphone/Tablet Ownership");
            HeaderTabs.Add("CrossDeviceOwnership", "Cross-Device Ownership");
            HeaderTabs.Add("SampleSize", "Sample Size");

            HeaderTabs.Add("TopBoxSatisfaction SampleSize", "Sample Size - Top Box Satisfaction");
            HeaderTabs.Add("TopBoxSatisfaction", "Top Box Satisfaction");
            HeaderTabs.Add("TopBoxLikeability SampleSize", "Sample Size - Top Box Likeability");
            HeaderTabs.Add("TopBoxWillingnesstoRecommend", "Top Box Willingness to Recommend");

            HeaderTabs.Add("TopBoxWillingnesstoRecommend SampleSize", "Sample Size - Top Box Willingness to Recommend");
            HeaderTabs.Add("TopBoxEarnedLoyalty", "Top Box Earned Loyalty");
            HeaderTabs.Add("TopBoxEarnedLoyalty SampleSize", "Sample Size - Top Box Earned Loyalty");
            HeaderTabs.Add("MainStore SampleSize", "Sample Size - Main Store");
            HeaderTabs.Add("MainStore", "Main Store");

            HeaderTabs.Add("MainStoreOverAll", "Main Store OverAll");
            HeaderTabs.Add("MainStoreOverAll SampleSize", "Sample Size - Main Store OverAll");
            HeaderTabs.Add("FavoriteStore", "Favorite Store");
            HeaderTabs.Add("FavoriteStore SampleSize", "Sample Size - Favorite Store");

            HeaderTabs.Add("FavoriteStoreOverAll", "Favorite Store OverAll");
            HeaderTabs.Add("FavoriteStoreOverAll SampleSize", "Sample Size - Favorite Store OverAll");

            HeaderTabs.Add("StoreAttribute", "Store Attributes");
            HeaderTabs.Add("GoodPlaceToShop", "Good Place To Shop");
            HeaderTabs.Add("InstorePurchaseInfluence", "In-Store Influencers");
            HeaderTabs.Add("Smartphone/TabletInfluencedPurchases", "Smartphone/Tablet Influenced Purchases?");

            HeaderTabs.Add("SmartPhoneUsage", "Ways Technology Used to Influence");
            HeaderTabs.Add("BeverageBrandsPurchased", "Beverage Brands Purchased: SSD");

            HeaderTabs.Add("ItemsPurchasedSummary", "Items Purchased Summary");
            HeaderTabs.Add("InStoreDestinationDetails", "Items Purchased Detail");
            HeaderTabs.Add("ImpulseItem", "Impulse Item");
            HeaderTabs.Add("TripMission", "Trip Mission");

            HeaderTabs.Add("WayTabletInfluenced", "Way Tablet Influenced");
            HeaderTabs.Add("WaySmartphoneInfluenced", "Way Smartphone Influenced");

            HeaderTabs.Add("TimeSpent", "Time Spent in Store");
            HeaderTabs.Add("TripExpenditure", "Trip Expenditure");
            HeaderTabs.Add("CheckOutType", "Checkout Method");
            HeaderTabs.Add("ConsideredStoreVisits", "Considered Store Visits");
            HeaderTabs.Add("VisitMotiviations", "Visit Motiviations");
            HeaderTabs.Add("tabletBased", "Tablet Based");
            HeaderTabs.Add("MostImportantDestinationItems", "Most Important Destination Items");
            HeaderTabs.Add("PaymentMode", "Method of Payment and Store Cards");
            HeaderTabs.Add("RetailerLoyaltyPyramid(Base:CouldShop)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)");

            HeaderTabs.Add("RetailerLoyaltyPyramid(supermarket)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)(Favorite & Commited by Within Channel - Supermarket)");
            HeaderTabs.Add("RetailerLoyaltyPyramid(convenience)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)(Favorite & Commited by Within Channel - Convenience)");
            HeaderTabs.Add("RetailerLoyaltyPyramid(drug store)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)(Favorite & Commited by Within Channel - Drug Store)");
            HeaderTabs.Add("RetailerLoyaltyPyramid(dollar store)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)(Favorite & Commited by Within Channel - Dollar Store)");
            HeaderTabs.Add("RetailerLoyaltyPyramid(club)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)(Favorite & Commited by Within Channel - Club)");
            HeaderTabs.Add("RetailerLoyaltyPyramid(mass merchandise)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)(Favorite & Commited by Within Channel - Mass Merchandise)");
            HeaderTabs.Add("RetailerLoyaltyPyramid(supercenter)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)(Favorite & Commited by Within Channel - Supercenter)");

            HeaderTabs.Add("RedeemedCoupon", "Coupon Redemption");
            HeaderTabs.Add("RedeemedCouponTypes", "Type of Coupons Redeemed");
            HeaderTabs.Add("DestinationStoreTrip", "Destination Following Store Trip");
            HeaderTabs.Add("TripSatisfaction", "Trip Satisfaction");
            HeaderTabs.Add("TripAttributeSatisfaction", "Trip Attribute Satisfaction - Top 2 Box");
            HeaderTabs.Add("Diet carbonated soft drinks", "Diet SSD");
            HeaderTabs.Add("Regular (non-diet) carbonated soft drinks", "REG SSD");
            HeaderTabs.Add("Bottled water", "Water");
            HeaderTabs.Add("Juice or juice drinks", "Juice");
            HeaderTabs.Add("Iced tea in bottles, cans, or cartons", "Iced Tea");
            HeaderTabs.Add("Coffee in bottles or cans", "Coffee");
            HeaderTabs.Add("Sports drinks", "Sports Drinks");
            HeaderTabs.Add("Energy drinks", "Energy Drinks");
            HeaderTabs.Add("P3M+ Channel Shopping Frequency", "Quarterly+ Channel Shopping Frequency");
            HeaderTabs.Add("P3M+ Priority Store Shopping Frequency", "Quarterly+ Priority Store Shopping Frequency");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(REG SSD)", "Do not purchase(REG SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(DIET SSD)", "Do not purchase(DIET SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(WATER)", "Do not purchase(WATER)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(JUICE)", "Do not purchase(JUICE)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(SPORTS DRINKS)", "Do not purchase(SPORTS DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(ENERGY DRINKS)", "Do not purchase(ENERGY DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD TEA)", "Do not purchase(RTD TEA)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD COFFEE)", "Do not purchase(RTD COFFEE)");

            //Below is added By Mehatab on 22nd Dec 2014.
            HeaderTabs.Add("Coffee", "Coffee (Hot, Iced, RTD Bottle/Can/Carton)");
            HeaderTabs.Add("Tea", "Tea (Hot, Iced, RTD Bottle/Can/Carton)");

            HeaderTabs.Add("StoreAttributesFactors", "Store Attributes Factors");
            HeaderTabs.Add("GoodPlaceToShopFactors", "Good Place To Shop Factors");

            HeaderTabs.Add("PRIMARYHHSHOPPER", "PRIMARY HH SHOPPER");
            HeaderTabs.Add("BeverageConsumedMonthly", "Beverage Consumed Monthly");
            HeaderTabs.Add("BeveragepurchasedMonthly", "Beverage purchased Monthly");
            HeaderTabs.Add("FAVOURITESPORTDRINKS", "FAVORITE SPORT DRINKS");
            HeaderTabs.Add("FAVOURITEENERGYDRINKS", "FAVORITE ENERGY DRINKS");
            HeaderTabs.Add("FAVOURITEREGSSD", "FAVORITE REG SSD");
            HeaderTabs.Add("FAVOURITEDIETSSD", "FAVORITE DIET SSD");
            HeaderTabs.Add("FAVOURITEUNFLAVOREDBOTTLEDWATER(NON-SPARKLING)", "FAVORITE UNFLAVORED BOTTLED WATER (NON-SPARKLING)");
            HeaderTabs.Add("FAVOURITEFLAVOREDSPARKLINGWATER", "FAVORITE FLAVORED SPARKLING WATER");
            HeaderTabs.Add("FAVOURITE RTD TEA", "FAVORITE RTD TEA");

            HeaderTabs.Add("FAVOURITEUNFLAVOREDSPARKLINGWATER", "FAVORITE UNFLAVORED SPARKLING WATER");
            HeaderTabs.Add("FAVOURITEFLAVOREDNON-SPARKLINGWATER", "FAVORITE FLAVORED NON-SPARKLING WATER");

            HeaderTabs.Add("FAVOURITE100%ORANGEJUICE", "FAVORITE 100% ORANGE JUICE");
            HeaderTabs.Add("FAVOURITE100%JUICE(NOT ORANGE)", "FAVORITE 100% JUICE (NOT ORANGE)");
            HeaderTabs.Add("FAVOURITEREADY-TO-DRINKJUICEDRINKS/ADE(<100% JUICE)", "FAVORITE READY-TO-DRINK JUICE DRINKS/ADE(<100% JUICE)");
            HeaderTabs.Add("FAVOURITEREADY-TO-DRINKSMOOTHIES", "FAVORITE READY-TO-DRINK SMOOTHIES");
            HeaderTabs.Add("FAVOURITELIQUIDFLAVORENHANCERS", "FAVORITE LIQUID FLAVOR ENHANCERS");
            HeaderTabs.Add("FAVOURITERTDTEA", "FAVORITE RTD TEA");
            HeaderTabs.Add("FAVOURITERTDCOFFEE", "FAVORITE RTD COFFEE");
            HeaderTabs.Add("SEAGRAM?S", "SEAGRAM'S");
            HeaderTabs.Add("FAVOURITE BEVERAGE BRAND", "FAVORITE BEVERAGE BRAND");
            HeaderTabs.Add("FAVOURITE SPORT DRINKS", "FAVORITE SPORT DRINKS");
            HeaderTabs.Add("FAVOURITE ENERGY DRINKS", "FAVORITE ENERGY DRINKS");
            HeaderTabs.Add("FAVOURITE REG SSD", "FAVORITE REG SSD");
            HeaderTabs.Add("FAVOURITE DIET SSD", "FAVORITE DIET SSD");
            HeaderTabs.Add("FAVOURITE UNFLAVORED BOTTLED WATER (NON-SPARKLING)", "FAVORITE UNFLAVORED BOTTLED WATER(NON-SPARKLING)");

            HeaderTabs.Add("FAVOURITE UNFLAVORED SPARKLING WATER", "FAVORITE UNFLAVORED SPARKLING WATER");
            HeaderTabs.Add("FAVOURITE FLAVORED NON-SPARKLING WATER", "FAVORITE FLAVORED NON-SPARKLING WATER");
            HeaderTabs.Add("FAVOURITE FLAVORED SPARKLING WATER", "FAVORITE FLAVORED SPARKLING WATER");
            HeaderTabs.Add("FAVOURITE 100% ORANGE JUICE", "FAVORITE 100% ORANGE JUICE");
            HeaderTabs.Add("FAVOURITE 100% JUICE (NOT ORANGE)", "FAVORITE 100% JUICE(NOT ORANGE)");
            HeaderTabs.Add("FAVOURITE READY-TO-DRINK JUICE DRINKS/ADE(<100%JUICE)", "FAVORITE READY-TO-DRINK JUICE DRINKS/ADE(<100%JUICE)");
            HeaderTabs.Add("FAVOURITE READY-TO-DRINK SMOOTHIES", "FAVORITE READY-TO-DRINK SMOOTHIES");
            HeaderTabs.Add("FAVOURITE LIQUID FLAVORED ENHANCERS", "FAVORITE LIQUID FLAVORED ENHANCERS");
            HeaderTabs.Add("FAVOURITE RTD COFFEE", "FAVORITE RTD COFFEE");
            HeaderTabs.Add("A blue-collar occupation", "Blue Collar");
            HeaderTabs.Add("A white-collar occupation", "White Collar");
            //HeaderTabs.Add("Other","Other Employed");
            HeaderTabs.Add("FAVOURITEPROTEINDRINKS", "FAVOURITE PROTEIN DRINKS");
        }

        //Nagaraju D 25-03-2014
        //clean class
        public string CleanClass(string _class)
        {
            _class = Regex.Replace(_class, @"[/\s,.`/@#$%;&*~()+?/]", "");
            return _class;
        }

        //Nagaraju D 07-04-2014
        //Create first header
        private string CreateFirstTableHeader()
        {
            leftheader = "";
            rightheader = "";
            string table = string.Empty;
            //table += "<div class=\"rowitem\"><ul>";
            //table += "<li style=\"height: 32px;\" class=\"ShoppingFrequencytitle\"><span>Shopping Frequency</span></li>";
            //table += "<li class=\"benchmarktitle\" style=\"\"><span>BENCHMARK</span></li>";
            //table += "<li class=\"comparisonheader\" style=\"\"><span>COMPARISON AREAS</span></li>";

            //leftheader += "<div class=\"rowitem\"><ul><li style=\"height: 25px;\" class=\"ShoppingFrequencytitle\"><span>Shopping Frequency</span></li>";
            //leftheader += "<li class=\"benchmarktitle\" style=\"height:25px;\"><span>BENCHMARK</span></li></ul></div>";
            if (complist != null && complist.Count > 1)
                //rightheader += "<div class=\"rowitem\"><ul style=\"\" ><li class=\"" + CleanClass(Convert.ToString(complist[1])) + "header\" style=\"\"><span style=\"\">COMPARISON AREAS</span></li>";

                if (complist != null && complist.Count > 2)
                {
                    for (int i = 2; i < complist.Count; i++)
                    {
                        //table += "<li class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"><span></span></li>";
                        //rightheader += "<li style=\"\" class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"><span style=\"\"></span></li>";
                    }
                }
            //rightheader += "</ul></div>";
            //table += "</ul></div>";
            return table;
        }

        private string CreateFirstTableHeaderOvertime()
        {
            leftheader = "";
            rightheader = "";
            string table = string.Empty;
            //table += "<div class=\"rowitem\"><ul>";
            //table += "<li style=\"height: 32px;\" class=\"ShoppingFrequencytitle\"><span></span></li>";
            //table += "<li class=\"benchmarktitle\" style=\"\"><span></span></li>";
            //table += "<li class=\"comparisonheader\" style=\"\"><span></span></li>";

            //leftheader += "<div class=\"rowitem\"><ul><li style=\"height: 32px;height:25px;\" class=\"ShoppingFrequencytitle\"><span></span></li>";
            //leftheader += "<li class=\"benchmarktitle\" style=\"height:25px;\"><span></span></li></ul></div>";
            //if (complist != null && complist.Count > 1)
            //    rightheader += "<div class=\"rowitem\"><ul style=\"\" ><li class=\"" + CleanClass(Convert.ToString(complist[1])) + "header\" style=\"\"><span style=\"\"></span></li>";

            //if (complist != null && complist.Count > 2)
            //{
            //    for (int i = 2; i < complist.Count; i++)
            //    {
            //        table += "<li class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"><span></span></li>";
            //        rightheader += "<li style=\"\" class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"><span style=\"\"></span></li>";
            //    }
            //}
            //rightheader += "</ul></div>";
            //table += "</ul></div>";
            return table;
        }
        private string AddTradeAreaNoteforChannel(string ChannelRetailer)
        {
            string TradeNode = string.Empty;
            if (frequency == "Store In Trade Area")
            {
                if (ChannelRetailer.ToLower() == "convenience" || ChannelRetailer.ToLower() == "dollar" || ChannelRetailer.ToLower() == "supermarketgrocery" ||
                    ChannelRetailer.ToLower() == "massmerc" || ChannelRetailer.ToLower() == "drug" || ChannelRetailer.ToLower() == "club" ||
                    ChannelRetailer.ToLower() == "supercenter" || ChannelRetailer.ToLower() == "total shopper" || ChannelRetailer.ToLower() == "total trips")
                {
                    TradeNode = " (Any Priority Store in Trade Area)";
                }
            }
            return TradeNode;
        }

        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        private bool Check_Beverage_Liquid_Flavor_Enhancer(string tableName, string columnName)
        {
            isLiquidFlavorEnhancer = false;
            if (isBeverageDetail)
            {
                if (Is_Beverage_Detail_NA_Column(columnName))
                {
                    switch (tableName)
                    {
                        case "Product Temperature":
                        case "Chilled - Location":
                        case "Room Temperature Location":
                        case "Intended Consumer":
                            {
                                isLiquidFlavorEnhancer = true;
                                break;
                            }
                    }

                }
            }
            return isLiquidFlavorEnhancer;
        }
        //
        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        private string Check_Beverage_Liquid_Flavor_Enhancer_NA_Table(string tableName)
        {
            if (isBeverageDetail)
            {
                switch (tableName)
                {
                    case "Product Temperature":
                    case "Chilled - Location":
                    case "Room Temperature Location":
                    case "Intended Consumer":
                        {
                            tableName = tableName + "*";
                            break;
                        }
                }
            }
            return tableName;
        }
        //
        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        private string Get_Beverage_Liquid_Flavor_Enhancer_Note()
        {
            string Note = string.Empty;
            if (isBeverageDetail)
            {
                Note = "*Beverage temperature, location in store and intended consumer are only asked of the following categories: SSD, RTD Coffee, RTD Tea," +
                       "Enhanced Milk, Protein Drinks, RTD Smoothies, Juice/Juice Drinks, Packaged Water, Spots Drinks and Energy Shots/Drinks";
            }
            return Note;
        }
        //
        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        private bool Is_Beverage_Detail_NA_Column(string columnName)
        {
            bool isBeverageDetail_NA_Column = false;
            if (columnName.IndexOf("|") > -1)
            {
                List<string> ShopperSegment = columnName.Split('|').ToList();
                if (ShopperSegment != null && ShopperSegment.Count > 0)
                {
                    columnName = ShopperSegment[ShopperSegment.Count - 1];
                }
            }
            if (Convert.ToString(columnName).IndexOf("Liquid Flavor Enhancer") > -1)
            {
                isBeverageDetail_NA_Column = true;
            }
            else
            {
                switch (columnName.Trim().ToLower())
                {
                    case "total trips":
                    case "total beverage trips":
                    case "total non-alcoholic non-rtd beverages":
                    case "alcohol":
                    case "coffee or cocoa":
                    case "tea":
                    case "dairy/ dairy alternatives":
                    case "protein drinks, yogurt drinks, meal replacements, and smoothies":
                    case "bulk water":
                    case "liquid flavor enhancers":
                    case "frozen slushies":
                    case "coffee":
                    case "hot chocolate/cocoa":
                    case "fresh brewed coffee":
                    case "fresh brewed tea":
                    case "plain or flavored milk":
                    case "dairy alternatives":
                    case "smoothies":
                    case "meal replacements":
                    case "drinkable yogurt":
                    case "fresh brewed hot coffee":
                    case "fresh brewed iced coffee":
                    case "fresh brewed hot tea":
                    case "fresh brewed iced tea":
                    case "non enhanced plain or flavored milk":
                    case "freshly prepared smoothies":
                    case "beer":
                    case "wine":
                    case "liquor":
                        {
                            isBeverageDetail_NA_Column = true;
                            break;
                        }
                }
            }
            return isBeverageDetail_NA_Column;
        }
        //
        public iSHOPParams BindTabs(string _BenchMark, string[] _Comparisonlist, string timePeriod, string _ShopperSegment, string filterShortname, string _ShopperFrequency, string[] ShortNames, string StatPositive, string StatNegative, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string CustomBase_ShortName, DataSet ds_2, string SampleSizetype)
        {
            iSHOPParams ishopParams = new iSHOPParams();
            View_Type = "COMPARE";
            TabIndexId = 2;
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
            frequency = _ShopperFrequency;
            //PopulateShortNames();
            //Shortnames();

            Retailerlist = _Comparisonlist;
            var query1 = from r in _Comparisonlist select string.Join("||", r.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            _Comparisonlist = query1.ToArray();
            param = new iSHOPParams();
            _BenchMark = string.Join("||", _BenchMark.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            param.BenchMark = _BenchMark;
            //param.BenchMark = string.Join("||", _BenchMark.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            string[] complistaArray = new string[0];
            //complist = new List<string>();
            var query = from r in ShortNames select r;
            complistaArray = query.ToArray();
            complist = query.ToList();
            ul_row_width = Math.Round(Convert.ToDouble(ulwidth), 0);
            ul_cell_width = Math.Round(Convert.ToDouble(ulliwidth), 0);

            //param.ShopperSegment = _ShopperSegment;
            TimePeriod = TimePeriodShortName;
            param.ShopperFrequency = _ShopperFrequency;
            param.CustomFilters = filterShortname;

            if (Selected_StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedStatTest = Selected_StatTest;
            }


            BenchmarkorComparisionList = _Comparisonlist.ToList();
            //BenchmarkorComparisionList.Insert(0, _BenchMark);

            DataAccess dal = new DataAccess();
            DataSet ds = null;
            DataTable tbl_Common_Sample_Size = null;
            common_SampleSize = false;

            if (ds_2 != null && ds_2.Tables.Count > 0)
            {
                ds = new DataSet();
                var query2 = (from row in ds_2.Tables[0].AsEnumerable()
                              where (Convert.ToString(row["Metric"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase))

                              select row).Distinct().ToList();

                var query4 = (from row in ds_2.Tables[0].AsEnumerable()
                              where (Convert.ToString(row["MetricItem"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase))
                              select row).Distinct().ToList();

                if (query4 != null && query4.Count > 0)
                    common_SampleSize = false;
                else if (query2 != null && query2.Count > 0)
                    common_SampleSize = true;

                if (query2 != null && query2.Count > 0)
                {
                    tbl_Common_Sample_Size = query2.CopyToDataTable();
                }
                if (tbl_Common_Sample_Size == null && query4 != null && query4.Count > 0 && SampleSizetype == "1")
                {
                    tbl_Common_Sample_Size = query4.CopyToDataTable();
                }


                //Display SampleSize
                if (common_SampleSize)
                {
                    var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                            where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                            && Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                            select row).Distinct().ToList();
                    if (query_samplesize != null && query_samplesize.Count > 0)
                    {
                        DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                        foreach (object column in trips_samplesize.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                    else
                    {
                        foreach (object column in tbl_Common_Sample_Size.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                }
                else
                {
                    if (tbl_Common_Sample_Size != null)
                    {
                        common_SampleSize = true;
                        var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                                where Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                                ||
                                                Convert.ToString(row["MetricItem"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                                                select row).Distinct().ToList();
                        if (query_samplesize != null && query_samplesize.Count > 0)
                        {
                            DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                            foreach (object column in trips_samplesize.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                        else
                        {
                            foreach (object column in tbl_Common_Sample_Size.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                    }
                }
                //Display Samplesize End



                if (common_SampleSize)
                {
                    var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                            where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                            && Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                            select row).Distinct().ToList();
                    if (query_samplesize != null && query_samplesize.Count > 0)
                    {
                        DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                        foreach (object column in trips_samplesize.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                    else
                    {
                        foreach (object column in tbl_Common_Sample_Size.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                }
                else
                {
                    if (tbl_Common_Sample_Size != null)
                    {
                        common_SampleSize = true;
                        var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                                where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                                select row).Distinct().ToList();
                        if (query_samplesize != null && query_samplesize.Count > 0)
                        {
                            DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                            foreach (object column in trips_samplesize.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                        else
                        {
                            foreach (object column in tbl_Common_Sample_Size.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                    }
                }

                List<string> metriclist = new List<string>();
                if (SampleSizetype == "1")
                {
                    metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                  where !Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                  select Convert.ToString(row["Metric"])).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      where !Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(row["MetricItem"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
                else if (SampleSizetype == "2")
                {
                    metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                  where !Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                  select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
            }

            int rownumber = 6;

            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);

            StringBuilder tbltext = new StringBuilder();
            string Significance = string.Empty;
            colmaxwidth = 0;

            try
            {
                tbltext.Append("<thead>");
                tbltext.Append(CreateFirstTableHeader());
                tbltext.Append("<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;height:29px;\"><span>" + frequency + "</span></li>");

                leftheader += "<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;height:29px;\"><a class=\"table-top-title-bottom-line\"></a><span style=\"\">Among " + frequency + " Shoppers for " + complistaArray[0] + "</span></li></ul></div>";
                rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                for (int i = 0; i < complistaArray.Count(); i++)
                {
                    colNames = complistaArray[i] + AddTradeAreaNoteforChannel(complistaArray[i]);
                    if (i == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(complistaArray[i] + "header");
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }

                    tbltext.Append("<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>");
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                }
                tbltext.Append("</ul></div>");
                rightheader += "</ul></div>";
                //add check sample size
                tbltext.Append("<div class=\"rowitem\"><ul>");

                List<iSHOPParams> iSHOPParamlist = null;
                iSHOPParams iparams = null;
                List<string> comp_list = (from r in complistaArray select r.Replace("&lt;", "<").ToLower()).ToList();
                DisplaysampleSize = DisplaysampleSize.OrderBy(x => comp_list.IndexOf(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                if (sampleSize != null && sampleSize.Count > 0)
                    if (DisplaysampleSize != null && DisplaysampleSize.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        //foreach (string key in sampleSize.Keys)
                            foreach (string key in DisplaysampleSize.Keys)
                            {
                                iparams = new iSHOPParams();
                                iparams.Retailer = key;
                                iparams.SampleSize = CommonFunctions.CheckdecimalValue(sampleSize[key]);
                                iparams.SampleSize = CommonFunctions.CheckdecimalValue(DisplaysampleSize[key]);
                                iSHOPParamlist.Add(iparams);
                            }
                    }

                rownumber += 1;
                if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                {
                    tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    foreach (iSHOPParams para in iSHOPParamlist)
                    {
                        double sample_size = 0;
                        if (!string.IsNullOrEmpty(para.SampleSize) && Convert.ToDouble(para.SampleSize) > 0)
                            sample_size = Convert.ToDouble(para.SampleSize);

                        if (iSHOPParamlist.IndexOf(para) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(sample_size.ToString()) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(sample_size)) + "</span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\">" + CommonFunctions.CheckdecimalValue(sample_size.ToString()) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(sample_size)) + "</span></li>";

                        }
                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(sample_size.ToString()) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(sample_size)) + "</span></li>");

                    }
                    rightheader += "</ul></div>";
                }
                else
                {
                    //if (common_SampleSize == true)
                    {
                        tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                        leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                        rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                        for (int j = 0; j < complistaArray.Count(); j++)
                        {
                            colNames = Get_ShortNames(complistaArray[j].Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                            if (j == 0)
                            {
                                benchmark_comp_class = "benchmarkheader";
                                leftheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>";
                            }
                            else
                            {
                                benchmark_comp_class = CleanClass(complistaArray[j]) + "header";
                                rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>";
                            }

                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>");

                        }
                        rightheader += "</ul></div>";
                    }
                }
                tbltext.Append("</ul></div>");

                benchmark_comp_class = string.Empty;
                //end header

                //------->
                table_count = 0;
                rows_count = 0;
                if (ds != null && ds.Tables.Count > 0)
                {
                    table_count = ds.Tables.Count;
                    int colms = ds.Tables[0].Columns.Count;
                    for (int tbl = 0; tbl < table_count; tbl++)
                    {
                        if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                        {

                            if (!common_SampleSize)
                            {
                                sampleSize = new Dictionary<string, string>();
                            }
                            rownumber += 1;
                            LoyaltyPyramid = false;
                            CheckBeverageTripNA = false;
                            LoyaltyPyramidmetric = Convert.ToString(ds.Tables[tbl].Rows[0][0]);

                            tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>");
                            leftbody += "<div class=\"rowitem table-title\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><a class=\"table-title-bottom-line\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>";
                            righttbody += "<div class=\"rowitem table-title\"><ul style=\"\">";
                            for (int i = 0; i < complistaArray.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>";
                                }
                                else
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span  style=\"\"></span></li>";
                                }
                                tbltext.Append("<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>");

                            }
                            leftbody += "</ul></div>";
                            righttbody += "</ul></div>";

                            tbltext.Append("</ul></div>");

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                            //write table name
                            List<string> tblcolumns = new List<string>();

                            tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                            List<string> comp = (from r in complistaArray select r.Replace("&lt;","<").ToLower()).ToList();
                            tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();
                            List<string> result = tblcolumns.OrderBy(str =>
                            {
                                int index = comp.IndexOf(str.ToLower());
                                return index == -1 ? int.MaxValue : index;
                            }).ToList();
                            rows_count = ds.Tables[tbl].Rows.Count;
                            for (int rows = 0; rows < rows_count; rows++)
                            {
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext.Append("<div class=\"rowitem\"><ul>");
                                    leftbody += "<div class=\"rowitem\"><ul>";
                                    righttbody += "<div class=\"rowitem\"><ul style=\"\">";

                                    average = "";
                                    switch (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]))
                                    {
                                        case "Approximate Average Number of Items":
                                        case "AVERAGE ONLINE BASKET SIZE":
                                        case "Approximate Average time in store":
                                        case "Approximate Amount Spent":
                                        case "AVERAGE ONLINE ORDER SIZE":
                                            {
                                                average = "Average";
                                                break;
                                            }
                                    }


                                    //write sample size
                                    if (String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Number of Trips", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "SampleSize", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Sample Size", true) == 0
                                         || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("SampleSize", StringComparison.OrdinalIgnoreCase) >= 0 || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Sample Size", StringComparison.OrdinalIgnoreCase) >= 0
                                        || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Number of Trips", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sampleSize = new Dictionary<string, string>();
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left; color: black;font-weight: normal;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        leftbody += "<li style=\"overflow: hidden;text-align: left; color: black;font-weight: normal;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                                        //plot sample size
                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                                            //Date: 21-03-2016
                                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[rows][0]), tblcolumns[i]);
                                            //
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].Trim().ToLower()))
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span  style=\"\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>");
                                                    sampleSize.Add(tblcolumns[i], Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));

                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color:black;font-weight: normal;\"><span  style=\"\"></span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>");

                                                }
                                            }

                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                        //End Sample Size
                                    }

                                    else
                                    {
                                        leftbody += "<li style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));
                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                                        //cellfontstyle = 8;

                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                                            //Date: 21-03-2016
                                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[rows][0]), tblcolumns[i]);
                                            //
                                            BenchmarkOrComparison = complistaArray[i];
                                            RetailerNetCheck = false;

                                            CheckRetailerorChannel = false;
                                            StoreImageryCheck = false;

                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].Trim().ToLower()))
                                                {
                                                    if (CheckSampleSize(tblcolumns[i]))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty:  GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }

                                                    }
                                                    else if (CommonFunctions.CheckMediumSampleSize(tblcolumns[i], sampleSize))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }

                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }
                                                    }

                                                    else
                                                    {

                                                        string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                        //if (i == 0)
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + (Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) ? "color: blue;" : "color: black;") + " \"><span>" + na + "</span></li>";
                                                        //}
                                                        //else
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color: black;\"><span style=\"\">" + na + "</span></li>";
                                                        //}
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + GetCellColor("", "", "") + " \"><span>" + na + "</span></li>";
                                                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color: black;\"><span></span></li>");

                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span style=\"\"></span></li>";
                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>");

                                                }

                                            }

                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                    }
                                }

                            }
                        }

                        else
                        {
                            tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:center\"><span>No data available</span></li></ul></div>");

                        }
                    }
                }
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;

                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext.ToString();
            }

            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }

        public iSHOPParams BindTabsTrends(string _BenchMark, string[] _Comparisonlist, string timePeriod, string _ShopperSegment, string filterShortname, string _ShopperFrequency, string[] ShortNames, string StatPositive, string StatNegative, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string CustomBase_ShortName, DataSet ds_2, string SampleSizetype)
        {
            iSHOPParams ishopParams = new iSHOPParams();
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
            frequency = _ShopperFrequency;
            //PopulateShortNames();
            //Shortnames();

            Retailerlist = _Comparisonlist;
            var query1 = from r in _Comparisonlist select string.Join("||", r.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            _Comparisonlist = query1.ToArray();
            param = new iSHOPParams();
            _BenchMark = string.Join("||", _BenchMark.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            param.BenchMark = _BenchMark;
            //param.BenchMark = string.Join("||", _BenchMark.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            string[] complistaArray = new string[0];
            //complist = new List<string>();
            var query = from r in ShortNames select r;
            complistaArray = query.ToArray();
            complist = query.ToList();
            ul_row_width = Math.Round(Convert.ToDouble(ulwidth), 0);
            ul_cell_width = Math.Round(Convert.ToDouble(ulliwidth), 0);

            //param.ShopperSegment = _ShopperSegment;
            TimePeriod = TimePeriodShortName;
            param.ShopperFrequency = _ShopperFrequency;
            param.CustomFilters = filterShortname;

            if (Selected_StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedStatTest = Selected_StatTest;
            }


            BenchmarkorComparisionList = _Comparisonlist.ToList();
            //BenchmarkorComparisionList.Insert(0, _BenchMark);

            DataAccess dal = new DataAccess();
            DataSet ds = null;
            DataTable tbl_Common_Sample_Size = null;
            common_SampleSize = false;

            if (ds_2 != null && ds_2.Tables.Count > 0)
            {
                ds = new DataSet();
                var query2 = (from row in ds_2.Tables[0].AsEnumerable()
                              where (Convert.ToString(row["Metric"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase))

                              select row).Distinct().ToList();

                var query4 = (from row in ds_2.Tables[0].AsEnumerable()
                              where (Convert.ToString(row["MetricItem"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase) || Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase))
                              select row).Distinct().ToList();

                if (query4 != null && query4.Count > 0)
                    common_SampleSize = false;
                else if (query2 != null && query2.Count > 0)
                    common_SampleSize = true;

                if (query2 != null && query2.Count > 0)
                {
                    tbl_Common_Sample_Size = query2.CopyToDataTable();
                }
                if (tbl_Common_Sample_Size == null && query4 != null && query4.Count > 0 && SampleSizetype == "1")
                {
                    tbl_Common_Sample_Size = query4.CopyToDataTable();
                }


                //Display SampleSize
                if (common_SampleSize)
                {
                    var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                            where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                            && Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                            select row).Distinct().ToList();
                    if (query_samplesize != null && query_samplesize.Count > 0)
                    {
                        DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                        foreach (object column in trips_samplesize.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                    else
                    {
                        foreach (object column in tbl_Common_Sample_Size.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                }
                else
                {
                    if (tbl_Common_Sample_Size != null)
                    {
                        common_SampleSize = true;
                        var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                                where Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                                ||
                                                Convert.ToString(row["MetricItem"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                                                select row).Distinct().ToList();
                        if (query_samplesize != null && query_samplesize.Count > 0)
                        {
                            DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                            foreach (object column in trips_samplesize.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                        else
                        {
                            foreach (object column in tbl_Common_Sample_Size.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    DisplaysampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                    }
                }
                //Display Samplesize End



                if (common_SampleSize)
                {
                    var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                            where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                            && Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                            select row).Distinct().ToList();
                    if (query_samplesize != null && query_samplesize.Count > 0)
                    {
                        DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                        foreach (object column in trips_samplesize.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                    else
                    {
                        foreach (object column in tbl_Common_Sample_Size.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                }
                else
                {
                    if (tbl_Common_Sample_Size != null)
                    {
                        common_SampleSize = true;
                        var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                                where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                                select row).Distinct().ToList();
                        if (query_samplesize != null && query_samplesize.Count > 0)
                        {
                            DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                            foreach (object column in trips_samplesize.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                        else
                        {
                            foreach (object column in tbl_Common_Sample_Size.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                    }
                }

                List<string> metriclist = new List<string>();
                if (SampleSizetype == "1")
                {
                    metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                  where !Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                  select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      where !Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                      && !Convert.ToString(row["MetricItem"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
                else if (SampleSizetype == "2")
                {
                    metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                  where !Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                  select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
            }

            int rownumber = 6;

            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);

            StringBuilder tbltext = new StringBuilder();
            string Significance = string.Empty;
            colmaxwidth = 0;

            try
            {
                tbltext.Append("<thead>");
                tbltext.Append(CreateFirstTableHeader());
                tbltext.Append("<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;height:29px;\"><span>" + frequency + "</span></li>");

                leftheader += "<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;height:29px;\"><a class=\"table-top-title-bottom-line\"></a><span style=\"\">" + frequency + "</span></li></ul></div>";
                rightheader += "<div class=\"rowitem\" style=\"margin-top: 3px;\"><ul style=\"height:100%\" >";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                for (int i = 0; i < complistaArray.Count(); i++)
                {
                    colNames = complistaArray[i] + AddTradeAreaNoteforChannel(complistaArray[i]);
                    if (i == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(complistaArray[i] + "header");
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }

                    tbltext.Append("<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>");
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                }
                tbltext.Append("</ul></div>");
                rightheader += "</ul></div>";
                //add check sample size
                tbltext.Append("<div class=\"rowitem\"><ul>");

                List<iSHOPParams> iSHOPParamlist = null;
                iSHOPParams iparams = null;
                List<string> comp_list = (from r in complistaArray select r.Replace("&lt;", "<").ToLower()).ToList();
                DisplaysampleSize = DisplaysampleSize.OrderBy(x => comp_list.IndexOf(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                if (sampleSize != null && sampleSize.Count > 0)
                    if (DisplaysampleSize != null && DisplaysampleSize.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        //foreach (string key in sampleSize.Keys)
                        foreach (string key in DisplaysampleSize.Keys)
                        {
                            iparams = new iSHOPParams();
                            iparams.Retailer = key;
                            iparams.SampleSize = CommonFunctions.CheckdecimalValue(sampleSize[key]);
                            iparams.SampleSize = CommonFunctions.CheckdecimalValue(DisplaysampleSize[key]);
                            iSHOPParamlist.Add(iparams);
                        }
                    }

                rownumber += 1;
                if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                {
                    tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                    //leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    foreach (iSHOPParams para in iSHOPParamlist)
                    {
                        if (iSHOPParamlist.IndexOf(para) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\">" + para.SampleSize + "</span></li>";

                        }
                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></li>");

                    }
                    rightheader += "</ul></div>";
                }
                else
                {
                    //if (common_SampleSize == true)
                    {
                        tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                        //leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                        rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                        for (int j = 0; j < complistaArray.Count(); j++)
                        {
                            colNames = Get_ShortNames(complistaArray[j].Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                            if (j == 0)
                            {
                                benchmark_comp_class = "benchmarkheader";
                                leftheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>";
                            }
                            else
                            {
                                benchmark_comp_class = CleanClass(complistaArray[j]) + "header";
                                rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>";
                            }

                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>");

                        }
                        rightheader += "</ul></div>";
                    }
                }
                tbltext.Append("</ul></div>");

                benchmark_comp_class = string.Empty;
                //end header

                //------->
                table_count = 0;
                rows_count = 0;
                if (ds != null && ds.Tables.Count > 0)
                {
                    table_count = ds.Tables.Count;
                    int colms = ds.Tables[0].Columns.Count;
                    for (int tbl = 0; tbl < table_count; tbl++)
                    {
                        if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                        {

                            if (!common_SampleSize)
                            {
                                sampleSize = new Dictionary<string, string>();
                            }
                            rownumber += 1;
                            LoyaltyPyramid = false;
                            CheckBeverageTripNA = false;
                            LoyaltyPyramidmetric = Convert.ToString(ds.Tables[tbl].Rows[0][0]);

                            tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>");
                            leftbody += "<div class=\"rowitem table-title\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><a class=\"table-title-bottom-line\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>";
                            righttbody += "<div class=\"rowitem table-title\"><ul style=\"\">";
                            for (int i = 0; i < complistaArray.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>";
                                }
                                else
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span  style=\"\"></span></li>";
                                }
                                tbltext.Append("<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>");

                            }
                            leftbody += "</ul></div>";
                            righttbody += "</ul></div>";

                            tbltext.Append("</ul></div>");

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                            //write table name
                            List<string> tblcolumns = new List<string>();

                            tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem" && x.ColumnName != "SortOrder").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                            List<string> comp = (from r in complistaArray select r.ToLower()).ToList();
                            tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();

                            rows_count = ds.Tables[tbl].Rows.Count;
                            for (int rows = 0; rows < rows_count; rows++)
                            {
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext.Append("<div class=\"rowitem\"><ul>");
                                    leftbody += "<div class=\"rowitem\"><ul>";
                                    righttbody += "<div class=\"rowitem\"><ul style=\"\">";

                                    average = "";
                                    switch (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]))
                                    {
                                        case "Approximate Average Number of Items":
                                        case "AVERAGE ONLINE BASKET SIZE":
                                        case "Approximate Average time in store":
                                        case "Approximate Amount Spent":
                                        case "AVERAGE ONLINE ORDER SIZE":
                                            {
                                                average = "Average";
                                                break;
                                            }
                                    }


                                    //write sample size
                                    if (String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Number of Trips", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "SampleSize", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Sample Size", true) == 0
                                         || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("SampleSize", StringComparison.OrdinalIgnoreCase) >= 0 || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Sample Size", StringComparison.OrdinalIgnoreCase) >= 0
                                        || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Number of Trips", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sampleSize = new Dictionary<string, string>();
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left; color: black;font-weight: normal;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        leftbody += "<li style=\"overflow: hidden;text-align: left; color: black;font-weight: normal;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                                        //plot sample size
                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                                            //Date: 21-03-2016
                                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[rows][0]), tblcolumns[i]);
                                            //
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].Trim().ToLower()))
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span  style=\"\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>");
                                                    sampleSize.Add(tblcolumns[i], Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));

                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span  style=\"\"></span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>");

                                                }
                                            }

                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                        //End Sample Size
                                    }

                                    else
                                    {
                                        leftbody += "<li style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));
                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");

                                        //cellfontstyle = 8;

                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                                            //Date: 21-03-2016
                                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[rows][0]), tblcolumns[i]);
                                            //
                                            BenchmarkOrComparison = complistaArray[i];
                                            RetailerNetCheck = false;

                                            CheckRetailerorChannel = false;
                                            StoreImageryCheck = false;

                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].Trim().ToLower()))
                                                {
                                                    if (CheckSampleSize(tblcolumns[i]))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }

                                                    }
                                                    else if (CommonFunctions.CheckMediumSampleSize(tblcolumns[i], sampleSize))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }

                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                        }
                                                    }

                                                    else
                                                    {

                                                        string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                        if (i == 0)
                                                        {
                                                            righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + (Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) ? "color: blue;" : "color: black;") + " \"><span>" + na + "</span></li>";
                                                        }
                                                        else
                                                        {
                                                            righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color: black;\"><span style=\"\">" + na + "</span></li>";
                                                        }
                                                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color: black;\"><span></span></li>");

                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span></span></li>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span style=\"\"></span></li>";
                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span></span></li>");

                                                }

                                            }

                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                    }
                                }

                            }
                        }

                        else
                        {
                            tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:center\"><span>No data available</span></li></ul></div>");

                        }
                    }
                }
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;

                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext.ToString();
            }

            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }

        public iSHOPParams BindTabsWithin(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string timePeriod, string _ShopperSegment, string _SingleSelection, string _ShopperFrequency, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, string ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams)
        {
            iSHOPParams ishopParams = new iSHOPParams();
            View_Type = "PIT";
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
            frequency = _ShopperFrequency;
            PopulateShortNames();
            Shortnames();
            BenchMark = ShortNames[0];

            if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = tableParams.CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedStatTest = Selected_StatTest;
            }

            Retailerlist = _Comparisonlist;
            param = new iSHOPParams();
            param.BenchMark = _BenchMark;
            //complist = new List<string>();
            string[] complistaArray = new string[0];
            var query = from r in ShortNames select r;
            //complist = query.ToList();
            complistaArray = query.ToArray();
            complist = query.ToList();
            ul_row_width = Math.Round(Convert.ToDouble(ulwidth), 0);
            ul_cell_width = Math.Round(Convert.ToDouble(ulliwidth), 0);
            CheckString = _ShopperSegment;
            TimePeriod = TimePeriodShortName;
            param.ShopperSegment = _SingleSelection;
            param.ShopperFrequency = _ShopperFrequency;
            param.CustomFilters = filterShortname;

            BenchmarkorComparisionList = _Comparisonlist.ToList();
            BenchmarkorComparisionList.Insert(0, _BenchMark);
            _ShopperSegment = string.Join("||", _ShopperSegment.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            DataAccess dal = new DataAccess();
            object[] paramvalues = null;
            DataSet ds = null;
            DataSet ds_2 = null;
            DataTable tbl_Common_Sample_Size = null;
            common_SampleSize = false;
            if (tableParams.Tab_Id_mapping)
            {
                string benchmark_UID = string.Empty;
                string comp_UID = string.Empty;
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    benchmark_UID = tableParams.CustomBase_UniqueId;
                    List<string> comp_list = (from r in tableParams.Comparison_UniqueIds where r != benchmark_UID select r).ToList();
                    comp_UID = string.Join("|", comp_list);
                }
                else
                {
                    benchmark_UID = tableParams.Comparison_UniqueIds[0];
                    tableParams.Comparison_UniqueIds.RemoveAt(0);
                    comp_UID = string.Join("|", tableParams.Comparison_UniqueIds);
                }

                paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, tableParams.ShopperSegment_UniqueId.ToMyString(), benchmark_UID.ToMyString(), comp_UID.ToMyString(), tableParams.TimePeriod_UniqueId.ToMyString(), tableParams.Filter_UniqueId, tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString() };
                ds_2 = dal.GetData_WithIdMapping(paramvalues, tabid);
                if (ds_2 != null && ds_2.Tables.Count > 0)
                {
                    ds = new DataSet();
                    var query2 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    var query4 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    if (query4 != null && query4.Count > 0)
                        common_SampleSize = false;
                    else if (query2 != null && query2.Count > 0)
                        common_SampleSize = true;

                    if (query2 != null && query2.Count > 0)
                    {
                        tbl_Common_Sample_Size = query2.CopyToDataTable();
                    }
                    if (common_SampleSize)
                    {
                        foreach (object column in tbl_Common_Sample_Size.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                    List<string> metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                               where !Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                               select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
            }
            else
            {
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    _BenchMark = tableParams.CustomBase_DBName;
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                else
                {
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                paramvalues = new object[] { _BenchMark.Replace("~", "`"), String.Join("|", _Comparisonlist).Replace("~", "`"), timePeriod, _ShopperSegment, _ShopperFrequency, _filter, Selected_StatTest };
                ds = dal.GetData(paramvalues, tabid);
            }
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            isBeverageDetail = false;
            if (tabid.Equals("sp_FactBookTripBevDetailsBeverageWithinMain", StringComparison.OrdinalIgnoreCase))
                isBeverageDetail = true;
            //
            if (IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency/imagery") > -1 || IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency") > -1)
            {
                IsStoreImagery = "StoreImagery";
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    _BenchMark = tableParams.CustomBase_DBName;
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                else
                {
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                paramvalues = new object[] { _BenchMark.Replace("~", "`"), String.Join("|", _Comparisonlist).Replace("~", "`"), timePeriod, _ShopperSegment, _ShopperFrequency, _filter, Selected_StatTest };
                DataSet LoyaltyDs = dal.GetData(paramvalues, "sp_FactBookRespStoreWithinChannelMapping");
                LoyaltyRetailerList = new Hashtable();
                foreach (DataRow row in LoyaltyDs.Tables[0].Rows)
                {
                    if (!Convert.ToString(row["Flag"]).Equals(GlobalVariables.NA) && !LoyaltyRetailerList.ContainsKey(Convert.ToString(row["Flag"])))
                        LoyaltyRetailerList.Add(Get_ShortNames(Convert.ToString(row["Flag"])), Convert.ToString(row["DisplayMetricName"]));
                }
            }
            int excelcolumnindex = 1;
            int rownumber = 6;

            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);
            //Nagaraju 27-03-2014
            if (ExportToExcel == "true")
            {
                if (HttpContext.Current.Session["sharedstrings"] != null)
                {
                    sharedStrings = HttpContext.Current.Session["sharedstrings"] as Dictionary<string, int>;
                }
            }
            //End
            tbltext = new StringBuilder();
            string Significance = string.Empty;
            xmlstring = string.Empty;
            colmaxwidth = 0;
            //string xmltext = string.Empty;
            StringBuilder xmltext = new StringBuilder();
            mergeCell = new List<string>();

            try
            {
                xmltext.Append("<sheetData>");
                //write top header
                xmltext.Append(WriteFilters());
                xmltext.Append(AddSampleSizeNote());
                xmltext.Append(GetTableHeader(complistaArray.Count(),"RETAILERS"));

                if (complistaArray.Count() > 1)
                {
                    mergeCell.Add("<mergeCell ref = \"C5:" + ColumnIndexToName(complistaArray.Count()) + "5\"/>");
                }
                //if (!sharedStrings.ContainsKey("BENCHMARK"))
                //{
                //    sharedStrings.Add("BENCHMARK", sharedStrings.Count());
                //}

                //if (!sharedStrings.ContainsKey("COMPARISON AREAS"))
                //{
                //    sharedStrings.Add("COMPARISON AREAS", sharedStrings.Count());
                //}

                //write second header
                excelcolumnindex = 0;
                xmltext.Append(" <row" +
               " r = \"" + rownumber + "\" " +
                "spans = \"1:11\" " +
                "x14ac:dyDescent = \"0.25\">" +
               " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"5\"/>");
                excelcolumnindex += 1;

                tbltext.Append("<thead>");
                tbltext.Append(CreateFirstTableHeader());
                tbltext.Append("<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><span>" + frequency + "</span></li>");

                leftheader += "<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><a class=\"table-top-title-bottom-line\"></a><span style=\"\">" + frequency + "</span></li></ul></div>";
                rightheader += "<div class=\"rowitem\"><ul style=\"\">";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                for (int i = 0; i < complistaArray.Count(); i++)
                {
                    colNames = Get_ShortNames(complistaArray[i]) + AddTradeAreaNoteforChannel(Get_ShortNames(complistaArray[i]));
                    if (i == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + (colNames) + "</span></li>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(complistaArray[i] + "header");
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\"" + colNames + "\">" + (colNames) + "</span></li>";
                    }

                    tbltext.Append("<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + (colNames) + "</span></li>");
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                    xmlstring = cf.cleanExcelXML(colNames);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }

                    xmltext.Append(" <c" +
                      " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                      " s = \"4\" " +
                      " t = \"s\">" +
                       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                   "</c>");
                    excelcolumnindex += 1;
                }
                tbltext.Append("</ul></div>");
                rightheader += "</ul></div>";
                //add check sample size
                tbltext.Append("<div class=\"rowitem\"><ul>");
                xmltext.Append("</row>");

                List<iSHOPParams> iSHOPParamlist = null;
                SampleSize checksampleSize = new SampleSize();
                iSHOPParamlist = checksampleSize.CheckWithinRetailerSampleSize(checksamplesizesp, _BenchMark, _Comparisonlist, timePeriod, _ShopperSegment, _ShopperFrequency, _filter, ShortNames, tableParams.Tab_Id_mapping, tbl_Common_Sample_Size, tableParams);
                rownumber += 1;
                excelcolumnindex = 0;
                xmlstring = cf.cleanExcelXML("Sample Size");

               if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                {
                    sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                }

                xmltext.Append("<row " +
                                     "r = \"" + rownumber + "\" " +
                                     "spans = \"1:11\" " +
                                     "ht = \"15\" " +
                                     "thickBot = \"1\" " +
                                     "x14ac:dyDescent = \"0.3\">" +
                                     " <c" +
                                     " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                     " s = \"3\" " +
                                     " t = \"s\">" +
                                    "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                    "</c>");
                if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                {
                    tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    foreach (iSHOPParams para in iSHOPParamlist)
                    {
                        excelcolumnindex += 1;
                        if (iSHOPParamlist.IndexOf(para) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\">" + para.SampleSize + "</span></li>";

                        }
                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></li>");

                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                 " s = \"2\">" +
                                                            "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(para.SampleSize).Replace(",", ""), out IsApplicable) + "</v>" +
                                                                  "</c> ");
                    }
                    rightheader += "</ul></div>";
                }
                else
                {
                    tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    for (int j = 0; j < complistaArray.Count(); j++)
                    {
                        excelcolumnindex += 1;
                        colNames = Get_ShortNames(complistaArray[j].Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                        if (j == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(complistaArray[j]) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>";
                        }

                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>");
                        xmlstring = string.Empty;

                       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                        {
                            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                        }

                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                 " s = \"2\">" +
                                                            "<v></v>" +
                                                                  "</c> ");
                    }
                    rightheader += "</ul></div>";
                }
                tbltext.Append("</ul></div>");
                tbltext.Append("");
                tbltext.Append("<tbody>");
                xmltext.Append("</row>");

                leftbody = "";
                //righttbody += "<div class=\"rowitem\"><ul style=\"\">";

                benchmark_comp_class = string.Empty;
                //end header
                table_count = 0;
                rows_count = 0;
                //------->
                if (ds != null && ds.Tables.Count > 0)
                {
                    table_count = ds.Tables.Count;
                    int colms = ds.Tables[0].Columns.Count;
                    //rownumber = 7;
                    for (int tbl = 0; tbl < table_count; tbl++)
                    {
                        if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                        {
                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                            //Date: 21-03-2016
                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[0][0]), _ShopperSegment);
                            //
                            excelcolumnindex = 0;
                            if (!common_SampleSize)
                            {
                                sampleSize = new Dictionary<string, string>();
                            }
                            rownumber += 1;
                            LoyaltyPyramid = false;
                            CheckBeverageTripNA = false;
                            LoyaltyPyramidmetric = Convert.ToString(ds.Tables[tbl].Rows[0][0]);
                            //if (LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(Base:CouldShop)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supermarket)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(convenience)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(drug store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(dollar store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(club)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(mass merchandise)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supercenter)")
                            //{
                            //    LoyaltyPyramid = true;
                            //}

                            switch (LoyaltyPyramidmetric)
                            {
                                //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                                case "RetailerLoyaltyPyramid(supermarket)":
                                case "RetailerLoyaltyPyramid(convenience)":
                                case "RetailerLoyaltyPyramid(drug store)":
                                case "RetailerLoyaltyPyramid(dollar store)":
                                case "RetailerLoyaltyPyramid(club)":
                                case "RetailerLoyaltyPyramid(mass merchandise)":
                                case "RetailerLoyaltyPyramid(supercenter)":
                                    {
                                        LoyaltyPyramid = true;
                                        break;
                                    }
                            }

                            switch (LoyaltyPyramidmetric)
                            {
                                //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                                case "RetailerLoyaltyPyramid(supermarket)":
                                case "RetailerLoyaltyPyramid(convenience)":
                                case "RetailerLoyaltyPyramid(drug store)":
                                case "RetailerLoyaltyPyramid(dollar store)":
                                case "RetailerLoyaltyPyramid(club)":
                                case "RetailerLoyaltyPyramid(mass merchandise)":
                                case "RetailerLoyaltyPyramid(supercenter)":
                                    {
                                        LoyaltyPyramidForRetailers = true;
                                        break;
                                    }
                                default:
                                    LoyaltyPyramidForRetailers = false;
                                    break;
                            }

                            switch (LoyaltyPyramidmetric)
                            {
                                case "Product Temperature":
                                case "Chilled - Location":
                                case "Room Temperature Location":
                                case "Intended Consumer":
                                    {
                                        CheckBeverageTripNA = true;
                                        break;
                                    }
                                default:
                                    CheckBeverageTripNA = false;
                                    break;
                            }


                            tbltext.Append("<div class=\"rowitem table-title\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><a class=\"table-title-bottom-line\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>");
                            leftbody += "<div class=\"rowitem table-title\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><a class=\"table-title-bottom-line\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>";
                            righttbody += "<div class=\"rowitem table-title\"><ul style=\"\">";
                            for (int i = 0; i < complistaArray.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>";
                                }
                                else
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span  style=\"\"></span></li>";
                                }
                                tbltext.Append("<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>");

                            }
                            leftbody += "</ul></div>";
                            righttbody += "</ul></div>";

                            tbltext.Append("</ul></div>");

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                            xmlstring = cf.cleanExcelXML(Check_Beverage_Liquid_Flavor_Enhancer_NA_Table(tablename));

                           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                            {
                                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                            }


                            //write table name
                            List<string> tblcolumns = new List<string>();
                            //foreach (object col in ds.Tables[tbl].Columns)
                            //{
                            //    string coln = Convert.ToString(col);
                            //    tblcolumns.Add(coln.Trim().ToLower().ToString());
                            //}

                            tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                            List<string> comp = (from r in complistaArray select r.ToLower()).ToList();
                            tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();

                            xmltext.Append("<row " +
                       "r = \"" + rownumber + "\" " +
                    "spans = \"1:11\" " +
                   " ht = \"15\" " +
                    "thickBot = \"1\" " +
                   " x14ac:dyDescent = \"0.3\">" +
                    "<c " +
                        "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                        "s = \"" + (LoyaltyPyramid ? 16 : 15) + "\" " +
                        "t = \"s\">" +
                        "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                    "</c>");

                            //for (int i = 0; i < complistaArray.Count; i++)
                            //{
                            //    excelcolumnindex += 1;
                            //    xmltext.Append("<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : 15) + "\"/>");
                            //}

                            xmltext.Append(String.Join("", complistaArray.Select(x => "<c r = \" " + ColumnIndexToName(excelcolumnindex++) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : 15) + "\"/>")));

                            xmltext.Append("</row>");
                            rows_count = ds.Tables[tbl].Rows.Count;
                            for (int rows = 0; rows < rows_count; rows++)
                            {
                                excelcolumnindex = 0;
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext.Append("<div class=\"rowitem\"><ul>");
                                    leftbody += "<div class=\"rowitem\"><ul>";
                                    righttbody += "<div class=\"rowitem\"><ul style=\"\">";
                                    //if (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average Number of Items" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average time in store" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Amount Spent")
                                    //{
                                    //    average = "Average";
                                    //}
                                    //else
                                    //{
                                    //    average = "";
                                    //}

                                    average = "";
                                    switch (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]))
                                    {
                                        case "Approximate Average Number of Items":
                                        case "AVERAGE ONLINE BASKET SIZE":
                                        case "Approximate Average time in store":
                                        case "Approximate Amount Spent":
                                        case "AVERAGE ONLINE ORDER SIZE":
                                            {
                                                average = "Average";
                                                break;
                                            }
                                    }

                                    //write sample size
                                    if (String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Number of Trips", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "SampleSize", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Sample Size", true) == 0
                                         || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("SampleSize", StringComparison.OrdinalIgnoreCase) >= 0 || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Sample Size", StringComparison.OrdinalIgnoreCase) >= 0
                                        || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Number of Trips", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sampleSize = new Dictionary<string, string>();
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        leftbody += "<li style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem);
                                       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                                        {
                                            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                                        }

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"3\" " +
                                           "t = \"s\">" +
                                           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                       "</c> ");

                                        //plot sample size
                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].Trim().ToLower()))
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span  style=\"\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out samplecellstyle) + "</span></li>");
                                                    sampleSize.Add(tblcolumns[i], Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                    if (samplecellstyle == 20)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])));
                                                        if (!sharedStrings.ContainsKey(lowsamplesize))
                                                        {
                                                            sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else if (samplecellstyle == 30)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])));
                                                        if (!sharedStrings.ContainsKey(lowsamplesize))
                                                        {
                                                            sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else
                                                    {

                                                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\">" +
                                                              "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                    "</c> ");
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span  style=\"\"></span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                    " s = \"2\">" +
                                                      "<v></v>" +
                                                         "</c> ");
                                                }
                                            }
                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                        xmltext.Append("</row>");
                                        //End Sample Size
                                    }

                                    else
                                    {
                                        leftbody += "<li style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));
                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem);

                                       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                                        {
                                            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                                        }

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"" + (average == "Average" ? "16" : "5") + "\" " +
                                           "t = \"s\">" +
                                           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                       "</c> ");
                                        //cellfontstyle = 8;

                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            BenchmarkOrComparison = tblcolumns[i];
                                            RetailerNetCheck = false;
                                            if (CheckString.IndexOf("Retailers") > -1 && (IsStoreImagery.IndexOf("StoreImagery") > -1) && LoyaltyPyramidForRetailers)
                                            {
                                                StoreImageryCheck = true;
                                                CheckRetailerorChannel = false;
                                            }
                                            else if ((CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            {
                                                RetailerNetCheck = true;
                                                CheckRetailerorChannel = true;
                                            }
                                            else if ((CheckString.IndexOf("Channels") > -1 || CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            {
                                                CheckRetailerorChannel = true;
                                                StoreImageryCheck = false;
                                            }
                                            else
                                            {
                                                CheckRetailerorChannel = false;
                                                StoreImageryCheck = false;
                                            }

                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].Trim().ToLower()))
                                                {
                                                    if (CheckSampleSize(tblcolumns[i]))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " t=\"s\"  s = \"17\">" +
                                                              "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename)] + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"" + cellfontstyle.ToString() + "\" t=\"s\">" +
                                                                 "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                    "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }

                                                            //                                                        
                                                        }

                                                    }
                                                    else if (CommonFunctions.CheckMediumSampleSize(tblcolumns[i], sampleSize))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            " t=\"s\" s = \"17\">" +
                                                              "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename)] + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
    " s = \"" + cellfontstyle.ToString() + "\"  t= \"s\">" +
    "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
    "</c> ");
                                                            }
                                                            else
                                                            {

                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }
                                                        }

                                                    }

                                                    else
                                                    {
                                                        string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                        if (i == 0)
                                                        {
                                                            righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + (Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) ? "color: blue;" : "color: black;") + "\"><span>" + na + "</span></li>";
                                                        }
                                                        else
                                                        {
                                                            righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color: black;\"><span style=\"\">" + na + "</span></li>";
                                                        }
                                                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color: black;\"><span></span></li>");
                                                        if (!string.IsNullOrEmpty(na))
                                                        {
                                                            GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                            if (!sharedStrings.ContainsKey(na))
                                                            {
                                                                sharedStrings.Add(na, sharedStrings.Count());
                                                            }
                                                            if (cellfontstyle == 30)
                                                            {
                                                                xmltext.Append("<c " +
                                            "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                            "s = \"" + cellfontstyle + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + sharedStrings[na] + "</v>" +
                                        "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c " +
                                             "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                             "s = \"" + (average == "Average" ? "16" : "4") + "\" " +
                                             "t = \"s\">" +
                                             "<v>" + sharedStrings[na] + "</v>" +
                                         "</c> ");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"" + (average == "Average" ? "17" : "8") + "\">" +
                                                                    "<v></v>" +
                                                                    "</c> ");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span></span></li>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span style=\"\"></span></li>";
                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstyle + "\">" +
                                                                         "<v></v>" +
                                                                            "</c> ");
                                                }
                                            }

                                        }
                                        xmltext.Append("</row>");
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                    }
                                }
                            }

                        }

                    }
                }
                else
                {
                    tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:center\"><span>No data available</span></li></ul></div>");
                    int nodatarow = 9;

                    string metricitem = "No data available";
                    xmlstring = cf.cleanExcelXML(metricitem);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }
                    xmltext.Append("<row " +
                                    "r = \"" + nodatarow.ToString() + "\" " +
                                    "spans = \"1:11\" " +
                                    "ht = \"15\" " +
                                    "thickBot = \"1\" " +
                                    "x14ac:dyDescent = \"0.3\">" +
                                    "<c " +
                                        "r = \"C" + nodatarow.ToString() + "\" " +
                                        "t = \"s\">" +
                                        "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                    "</c></row> ");
                }

                //tbltext.Append("</tbody>";
                //leftbody += "</tbody></table>";
                //righttbody += "</tbody>";
                if (isBeverageDetail)
                {
                    if (!sharedStrings.ContainsKey(Get_Beverage_Liquid_Flavor_Enhancer_Note()))
                    {
                        sharedStrings.Add(Get_Beverage_Liquid_Flavor_Enhancer_Note(), sharedStrings.Count());
                    }
                    xmltext.Append("<row " +
                                        "r = \"" + (rownumber + 2) + "\" " +
                                        "spans = \"1:11\" " +
                                        "ht = \"15\" " +
                                        "thickBot = \"1\" " +
                                        "x14ac:dyDescent = \"0.3\">" +
                                        "<c " +
                                             "r = \"A" + (rownumber + 2).ToString() + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + sharedStrings[Get_Beverage_Liquid_Flavor_Enhancer_Note()] + "</v>" +
                                        "</c></row> ");
                }
                xmltext.Append("</sheetData>");

                if (mergeCell.Count > 0)
                {
                    string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                    foreach (string mergrrow in mergeCell)
                    {
                        mergetext += mergrrow;

                    }
                    mergetext += "</mergeCells>";
                    xmltext.Append(mergetext);
                }

                xmltext.Append(GetPageMargins());
                string _xmltext = xmltext.ToString();
                xmltext = new StringBuilder();
                xmltext.Append(GetSheetHeadandColumns() + _xmltext.ToString());
                //Nagaraju 27-03-2014
                xmlstring = xmltext.ToString();
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                //exportfiles = new Dictionary<string, string>();
                //exportfiles.Add("tab1", xmltext);
                //HttpContext.Current.Session["exportfiles"] = exportfiles;
                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }

        public iSHOPParams BindTabsTimePeriod(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string _ShopperSegment, string _SingleSelection, string _ShopperFrequency, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, string ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams)
        {
            iSHOPParams ishopParams = new iSHOPParams();
            View_Type = "TREND";
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
            frequency = _ShopperFrequency;
            PopulateShortNames();
            Shortnames();
            BenchMark = _commonfunctions.Get_TableMappingNames(ShortNames[0].Trim());
            Retailerlist = _Comparisonlist;
            param = new iSHOPParams();
            param.BenchMark = _BenchMark;
            TimePeriod = TimePeriodShortName;
            string[] complistaArray = new string[0];
            //complist = new List<string>();
            var query = from r in ShortNames select r;
            complistaArray = query.ToArray();
            complist = query.ToList();
            ul_row_width = Math.Round(Convert.ToDouble(ulwidth), 0);
            ul_cell_width = Math.Round(Convert.ToDouble(ulliwidth), 0);
            CheckString = _ShopperSegment;
            param.ShopperSegment = _SingleSelection;
            param.ShopperFrequency = _ShopperFrequency;
            param.CustomFilters = filterShortname;
            _ShopperSegment = string.Join("||", _ShopperSegment.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            DataAccess dal = new DataAccess();
            object[] paramvalues = null;
            DataSet ds = null;
            DataSet ds_2 = null;
            DataTable tbl_Common_Sample_Size = null;
            common_SampleSize = false;
            if (tableParams.Tab_Id_mapping)
            {
                paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, tableParams.ShopperSegment_UniqueId.ToMyString(), tableParams.TimePeriodFrom_UniqueId.ToMyString(), tableParams.TimePeriodTo_UniqueId.ToMyString(), tableParams.Filter_UniqueId.ToMyString(), tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString() };
                ds_2 = dal.GetData_WithIdMapping(paramvalues, tabid);
                if (ds_2 != null && ds_2.Tables.Count > 0)
                {
                    ds = new DataSet();
                    var query2 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    var query4 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    if (query4 != null && query4.Count > 0)
                        common_SampleSize = false;
                    else if (query2 != null && query2.Count > 0)
                        common_SampleSize = true;

                    if (query2 != null && query2.Count > 0)
                    {
                        tbl_Common_Sample_Size = query2.CopyToDataTable();
                    }
                    if (common_SampleSize)
                    {
                        foreach (object column in tbl_Common_Sample_Size.Columns)
                        {
                            if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                            }
                        }
                    }
                    List<string> metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                               where !Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                               select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
            }
            else
            {
                paramvalues = new object[] { _BenchMark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), String.Join("|", _Comparisonlist).Replace("~", "`").Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), _ShopperSegment.Replace("~", "`"), _ShopperFrequency, _filter, Selected_StatTest };
                ds = dal.GetData(paramvalues, tabid);
            }
            if (IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency/imagery") > -1 || IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency") > -1)
            {
                paramvalues = new object[] { _BenchMark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), String.Join("|", _Comparisonlist).Replace("~", "`").Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), _ShopperSegment.Replace("~", "`"), _ShopperFrequency, _filter, Selected_StatTest };
                DataSet LoyaltyDs = dal.GetData(paramvalues, "sp_FactBookRespStoreOverTimeChannelMapping");
                LoyaltyRetailerList = new Hashtable();
                foreach (DataRow row in LoyaltyDs.Tables[0].Rows)
                {
                    if (!Convert.ToString(row["Flag"]).Equals(GlobalVariables.NA) && !LoyaltyRetailerList.ContainsKey(Convert.ToString(row["Flag"])))
                        LoyaltyRetailerList.Add(Get_ShortNames(Convert.ToString(row["Flag"])), Convert.ToString(row["DisplayMetricName"]));
                }
            }
            //if (_BenchMark.Equals("total|total", StringComparison.OrdinalIgnoreCase) && ds != null && ds.Tables.Count > 0)
            //{
            //    complistaArray = new List<string>();
            //    for (int i = 3; i < ds.Tables[0].Columns.Count; i++)
            //    {
            //        if (!complistaArray.Contains(Convert.ToString(ds.Tables[0].Columns[i])))
            //        {
            //            complistaArray.Add(Convert.ToString(ds.Tables[0].Columns[i]).Replace("~", "'"));
            //        }
            //    }
            //}
            complistaArray = new string[0];
            //for (int i = 3; i < ds.Tables[0].Columns.Count; i++)
            //{
            //    if (!complistaArray.Contains(Convert.ToString(ds.Tables[0].Columns[i])))
            //    {
            //        complistaArray.Add(Convert.ToString(ds.Tables[0].Columns[i]).Replace("~", "'"));
            //    }
            //}
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                complistaArray = ds.Tables[0].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName.ToLower())).Distinct().ToArray();
                List<string> comp = (from r in ShortNames select r.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 12MMT", "").Replace(" 6MMT", "").ToLower()).ToList();
                complistaArray = complistaArray.OrderBy(x => comp.IndexOf(x)).ToArray();
            }
            int excelcolumnindex = 1;
            int rownumber = 6;

            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);
            //Nagaraju 27-03-2014
            if (ExportToExcel == "true")
            {
                if (HttpContext.Current.Session["sharedstrings"] != null)
                {
                    sharedStrings = HttpContext.Current.Session["sharedstrings"] as Dictionary<string, int>;
                }
            }
            //End
            tbltext = new StringBuilder();
            string Significance = string.Empty;
            xmlstring = string.Empty;
            colmaxwidth = 0;
            //string xmltext = string.Empty;
            StringBuilder xmltext = new StringBuilder();
            mergeCell = new List<string>();

            try
            {
                xmltext.Append("<sheetData>");
                //write top header
                xmltext.Append(WriteFilters());
                xmltext.Append(AddSampleSizeNote());
                xmltext.Append(GetTableHeader(complistaArray.Count(),"RETAILERS"));

                if (complistaArray.Count() > 1)
                {
                    mergeCell.Add("<mergeCell ref = \"C5:" + ColumnIndexToName(complistaArray.Count()) + "5\"/>");
                }
                if (!sharedStrings.ContainsKey("BENCHMARK"))
                {
                    sharedStrings.Add("BENCHMARK", sharedStrings.Count());
                }

                if (!sharedStrings.ContainsKey("COMPARISON AREAS"))
                {
                    sharedStrings.Add("COMPARISON AREAS", sharedStrings.Count());
                }

                //write second header
                excelcolumnindex = 0;
                xmltext.Append(" <row" +
               " r = \"" + rownumber + "\" " +
                "spans = \"1:11\" " +
                "x14ac:dyDescent = \"0.25\">" +
               " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"5\"/>");
                excelcolumnindex += 1;

                tbltext.Append("<thead>");
                tbltext.Append(CreateFirstTableHeaderOvertime());
                tbltext.Append("<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><span>" + frequency + "</span></li>");

                leftheader += "<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><a class=\"table-top-title-bottom-line\"></a><span style=\"\">" + frequency + "</span></li></ul></div>";
                rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                for (int i = 0; i < complistaArray.Count(); i++)
                {
                    colNames = Get_ShortNames(complistaArray[i]) + AddTradeAreaNoteforChannel(Get_ShortNames(complistaArray[i]));
                    if (i == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + (colNames) + "</span></li>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(complistaArray[i] + "header");
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\"" + colNames + "\">" + (colNames) + "</span></li>";
                    }

                    tbltext.Append("<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + (colNames) + "</span></li>");
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                    xmlstring = cf.cleanExcelXML(colNames);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }

                    xmltext.Append(" <c" +
                      " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                      " s = \"4\" " +
                      " t = \"s\">" +
                       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                   "</c>");
                    excelcolumnindex += 1;
                }
                tbltext.Append("</ul></div>");
                rightheader += "</ul></div>";
                //add check sample size
                tbltext.Append("<div class=\"rowitem\"><ul>");
                xmltext.Append("</row>");

                List<iSHOPParams> iSHOPParamlist = null;
                SampleSize checksampleSize = new SampleSize();
                iSHOPParamlist = checksampleSize.CheckTimePeriodSampleSize(checksamplesizesp, _BenchMark, Retailerlist, _ShopperSegment, _ShopperFrequency, _filter, ShortNames, tableParams.Tab_Id_mapping, tbl_Common_Sample_Size);
                rownumber += 1;
                excelcolumnindex = 0;
                xmlstring = cf.cleanExcelXML("Sample Size");

               if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                {
                    sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                }

                xmltext.Append("<row " +
                                     "r = \"" + rownumber + "\" " +
                                     "spans = \"1:11\" " +
                                     "ht = \"15\" " +
                                     "thickBot = \"1\" " +
                                     "x14ac:dyDescent = \"0.3\">" +
                                     " <c" +
                                     " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                     " s = \"3\" " +
                                     " t = \"s\">" +
                                    "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                    "</c>");
                if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                {
                    tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    foreach (iSHOPParams para in iSHOPParamlist)
                    {
                        excelcolumnindex += 1;
                        if (iSHOPParamlist.IndexOf(para) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\">" + para.SampleSize + "</span></li>";

                        }
                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></li>");

                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                 " s = \"2\">" +
                                                            "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(para.SampleSize).Replace(",", ""), out IsApplicable) + "</v>" +
                                                                  "</c> ");
                    }
                    rightheader += "</ul></div>";
                }
                else
                {
                    tbltext.Append("<li style=\"\"><span>Sample Size</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>Sample Size</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    for (int j = 0; j < complistaArray.Count(); j++)
                    {
                        excelcolumnindex += 1;
                        colNames = Get_ShortNames(complistaArray[j].Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                        if (j == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(complistaArray[j]) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>";
                        }

                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>");
                        xmlstring = string.Empty;

                       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                        {
                            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                        }

                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                 " s = \"2\">" +
                                                            "<v></v>" +
                                                                  "</c> ");
                    }
                    rightheader += "</ul></div>";
                }
                tbltext.Append("</ul></div>");
                tbltext.Append("");
                tbltext.Append("<tbody>");
                xmltext.Append("</row>");

                leftbody = "";
                //righttbody += "<div class=\"rowitem\"><ul style=\"\">";

                benchmark_comp_class = string.Empty;
                //end header

                //------->
                if (ds != null && ds.Tables.Count > 0)
                {
                    int colms = ds.Tables[0].Columns.Count;
                    //rownumber = 7;
                    for (int tbl = 0; tbl < ds.Tables.Count; tbl++)
                    {
                        if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                        {
                            excelcolumnindex = 0;
                            if (!common_SampleSize)
                            {
                                sampleSize = new Dictionary<string, string>();
                            }
                            rownumber += 1;
                            LoyaltyPyramid = false;
                            LoyaltyPyramidmetric = Convert.ToString(ds.Tables[tbl].Rows[0][0]);
                            //if (LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(Base:CouldShop)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supermarket)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(convenience)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(drug store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(dollar store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(club)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(mass merchandise)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supercenter)")
                            //{
                            //    LoyaltyPyramid = true;
                            //}

                            switch (LoyaltyPyramidmetric)
                            {
                                //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                                case "RetailerLoyaltyPyramid(supermarket)":
                                case "RetailerLoyaltyPyramid(convenience)":
                                case "RetailerLoyaltyPyramid(drug store)":
                                case "RetailerLoyaltyPyramid(dollar store)":
                                case "RetailerLoyaltyPyramid(club)":
                                case "RetailerLoyaltyPyramid(mass merchandise)":
                                case "RetailerLoyaltyPyramid(supercenter)":
                                    {
                                        LoyaltyPyramid = true;
                                        break;
                                    }
                            }

                            switch (LoyaltyPyramidmetric)
                            {
                                //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                                case "RetailerLoyaltyPyramid(supermarket)":
                                case "RetailerLoyaltyPyramid(convenience)":
                                case "RetailerLoyaltyPyramid(drug store)":
                                case "RetailerLoyaltyPyramid(dollar store)":
                                case "RetailerLoyaltyPyramid(club)":
                                case "RetailerLoyaltyPyramid(mass merchandise)":
                                case "RetailerLoyaltyPyramid(supercenter)":
                                    {
                                        LoyaltyPyramidForRetailers = true;
                                        break;
                                    }
                                default:
                                    LoyaltyPyramidForRetailers = false;
                                    break;
                            }

                            tbltext.Append("<div class=\"rowitem table-title\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>");
                            leftbody += "<div class=\"rowitem table-title\"><ul><li style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><a class=\"table-title-bottom-line\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>";
                            righttbody += "<div class=\"rowitem table-title\"><ul style=\"\">";
                            for (int j = 0; j < complistaArray.Count(); j++)
                            {
                                if (j == 0)
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[j] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>";
                                }
                                else
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[j] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span  style=\"\"></span></li>";
                                }
                                tbltext.Append("<li class=\"" + CleanClass(complistaArray[j] + "cell") + "\" style=\"text-align:left;" + (LoyaltyPyramid ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"><span></span></li>");

                            }
                            leftbody += "</ul></div>";
                            righttbody += "</ul></div>";

                            tbltext.Append("</ul></div>");

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                            xmlstring = cf.cleanExcelXML(tablename);

                           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                            {
                                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                            }


                            //write table name
                            List<string> tblcolumns = new List<string>();
                            //foreach (object col in ds.Tables[tbl].Columns)
                            //{
                            //    string coln = Convert.ToString(col);
                            //    tblcolumns.Add(coln.Trim().ToLower().ToString());
                            //}

                            tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();

                            xmltext.Append("<row " +
                       "r = \"" + rownumber + "\" " +
                    "spans = \"1:11\" " +
                   " ht = \"15\" " +
                    "thickBot = \"1\" " +
                   " x14ac:dyDescent = \"0.3\">" +
                    "<c " +
                        "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                        "s = \"" + (LoyaltyPyramid ? 16 : 15) + "\" " +
                        "t = \"s\">" +
                        "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                    "</c>");

                            //for (int i = 0; i < complistaArray.Count; i++)
                            //{
                            //    excelcolumnindex += 1;
                            //    xmltext.Append("<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : 15) + "\"/>");
                            //}

                            xmltext.Append(String.Join("", complistaArray.Select(x => "<c r = \" " + ColumnIndexToName(excelcolumnindex++) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : 15) + "\"/>")));

                            xmltext.Append("</row>");
                            for (int rows = 0; rows < ds.Tables[tbl].Rows.Count; rows++)
                            {
                                excelcolumnindex = 0;
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext.Append("<div class=\"rowitem\"><ul>");
                                    leftbody += "<div class=\"rowitem\"><ul>";
                                    righttbody += "<div class=\"rowitem\"><ul style=\"\">";
                                    //if (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average Number of Items" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average time in store" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Amount Spent")
                                    //{
                                    //    average = "Average";
                                    //}
                                    //else
                                    //{
                                    //    average = "";
                                    //}

                                    average = "";
                                    switch (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]))
                                    {
                                        case "Approximate Average Number of Items":
                                        case "AVERAGE ONLINE BASKET SIZE":
                                        case "Approximate Average time in store":
                                        case "Approximate Amount Spent":
                                        case "AVERAGE ONLINE ORDER SIZE":
                                            {
                                                average = "Average";
                                                break;
                                            }
                                    }

                                    //write sample size
                                    //if (ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().ToLower() == "number of trips" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().ToLower() == "samplesize" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().ToLower()=="sample size")
                                    if (String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Number of Trips", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "SampleSize", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Sample Size", true) == 0
                                         || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("SampleSize", StringComparison.OrdinalIgnoreCase) >= 0 || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Sample Size", StringComparison.OrdinalIgnoreCase) >= 0
                                        || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Number of Trips", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sampleSize = new Dictionary<string, string>();
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left; color: black;font-weight: normal;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        leftbody += "<li style=\"overflow: hidden;text-align: left; color: black;font-weight: normal;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                        if (metricitem.Length > colmaxwidth)
                                            colmaxwidth = metricitem.Length;

                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem);
                                       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                                        {
                                            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                                        }

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"3\" " +
                                           "t = \"s\">" +
                                           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                       "</c> ");

                                        //plot sample size
                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                                benchmark_comp_class = "benchmarkcell";
                                            else
                                                benchmark_comp_class = CleanClass(complistaArray[i] + "cell");

                                            if (!string.IsNullOrEmpty(complistaArray[i]))
                                            {
                                                if (tblcolumns.Contains(_commonfunctions.Get_TableMappingNames(complistaArray[i]).Trim().ToLower()))
                                                {
                                                    if (i == 0)
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out samplecellstyle) + "</span></li>";
                                                    else
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span  style=\"\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out samplecellstyle) + "</span></li>";

                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;font-weight: normal;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out samplecellstyle) + "</span></li>");
                                                    sampleSize.Add(complistaArray[i], Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                    if (samplecellstyle == 20)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])));
                                                        if (!sharedStrings.ContainsKey(lowsamplesize))
                                                        {
                                                            sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else if (samplecellstyle == 30)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])));
                                                        if (!sharedStrings.ContainsKey(lowsamplesize))
                                                        {
                                                            sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else
                                                    {

                                                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\">" +
                                                              "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                    "</c> ");
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span  style=\"\"></span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: ; color: black;font-weight: normal;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                    " s = \"2\">" +
                                                      "<v></v>" +
                                                         "</c> ");
                                                }
                                            }
                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                        xmltext.Append("</row>");
                                        //End Sample Size
                                    }

                                    else
                                    {
                                        leftbody += "<li style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>";
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</span></li>");

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));
                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem);

                                       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                                        {
                                            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                                        }

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"" + (average == "Average" ? "16" : "5") + "\" " +
                                           "t = \"s\">" +
                                           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                       "</c> ");
                                        //cellfontstyle = 8;

                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            BenchmarkOrComparison = complistaArray[i];
                                            RetailerNetCheck = false;
                                            if (CheckString.IndexOf("Retailers") > -1 && (IsStoreImagery.IndexOf("StoreImagery") > -1 && LoyaltyPyramidForRetailers))
                                            {
                                                StoreImageryCheck = true;
                                                CheckRetailerorChannel = false;
                                            }
                                            else if ((CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            {
                                                RetailerNetCheck = true;
                                                CheckRetailerorChannel = true;
                                            }
                                            else if ((CheckString.IndexOf("Channels") > -1 || CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            {
                                                CheckRetailerorChannel = true;
                                                StoreImageryCheck = false;
                                            }
                                            else
                                            {
                                                CheckRetailerorChannel = false;
                                                StoreImageryCheck = false;
                                            }
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(complistaArray[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(complistaArray[i]))
                                            {
                                                if (tblcolumns.Contains(_commonfunctions.Get_TableMappingNames(complistaArray[i]).Trim().ToLower()))
                                                {
                                                    if (CheckSampleSize(complistaArray[i]))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " t=\"s\"  s = \"17\">" +
                                                              "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename)] + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");
                                                            string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31 || na == GlobalVariables.NA)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"" + cellfontstyle.ToString() + "\" t=\"s\">" +
                                                                 "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                    "</c> ");
                                                            }
                                                            else
                                                            {

                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }

                                                            //                                                        if (cellfontstyle == 4)
                                                            //                                                        {
                                                            //                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            //" s = \"" + cellfontstyle.ToString() + "\" t=\"s\">" +
                                                            //"<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                            //"</c> ");
                                                            //                                                        }
                                                            //                                                        else
                                                            //                                                        {
                                                            //                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            //                                                            " s = \"" + cellfontstyle.ToString() + "\">" +
                                                            //                                                            "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                            //                                                            "</c> ");
                                                            //                                                        }
                                                        }

                                                    }
                                                    else if (CommonFunctions.CheckMediumSampleSize(complistaArray[i],sampleSize))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            " t=\"s\" s = \"17\">" +
                                                              "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename)] + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: ;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");
                                                            string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31 || na == GlobalVariables.NA)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
    " s = \"" + cellfontstyle.ToString() + "\"  t= \"s\">" +
    "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
    "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }
                                                        }

                                                    }

                                                    else
                                                    {
                                                        string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                        if (i == 0)
                                                        {
                                                            righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + (Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) ? "color: blue;" : "color: black;") + "\"><span>" + na + "</span></li>";
                                                        }
                                                        else
                                                        {
                                                            righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color:black;\"><span style=\"\">" + na + "</span></li>";
                                                        }
                                                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + " color: black;\"><span></span></li>");
                                                        if (!string.IsNullOrEmpty(na))
                                                        {
                                                            GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                            if (!sharedStrings.ContainsKey(na))
                                                            {
                                                                sharedStrings.Add(na, sharedStrings.Count());
                                                            }
                                                            if (cellfontstyle == 30)
                                                            {
                                                                xmltext.Append("<c " +
                                            "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                            "s = \"" + cellfontstyle + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + sharedStrings[na] + "</v>" +
                                        "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c " +
                                             "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                             "s = \"" + (average == "Average" ? "16" : "4") + "\" " +
                                             "t = \"s\">" +
                                             "<v>" + sharedStrings[na] + "</v>" +
                                         "</c> ");

                                                            }
                                                        }
                                                        else
                                                        {
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"" + (average == "Average" ? "17" : "8") + "\">" +
                                                                    "<v></v>" +
                                                                    "</c> ");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span></span></li>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span style=\"\"></span></li>";
                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: black;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstyle + "\">" +
                                                                         "<v></v>" +
                                                                            "</c> ");
                                                }
                                            }

                                        }
                                        xmltext.Append("</row>");
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                    }
                                }
                            }

                        }

                    }
                }
                else
                {
                    tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:center\"><span>No data available</span></li></ul></div>");
                    int nodatarow = 9;

                    string metricitem = "No data available";
                    xmlstring = cf.cleanExcelXML(metricitem);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }
                    xmltext.Append("<row " +
                                    "r = \"" + nodatarow.ToString() + "\" " +
                                    "spans = \"1:11\" " +
                                    "ht = \"15\" " +
                                    "thickBot = \"1\" " +
                                    "x14ac:dyDescent = \"0.3\">" +
                                    "<c " +
                                        "r = \"C" + nodatarow.ToString() + "\" " +
                                        "t = \"s\">" +
                                        "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                    "</c></row> ");
                }

                //tbltext.Append("</tbody>";
                //leftbody += "</tbody></table>";
                //righttbody += "</tbody>";

                xmltext.Append("</sheetData>");

                if (mergeCell.Count > 0)
                {
                    string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                    foreach (string mergrrow in mergeCell)
                    {
                        mergetext += mergrrow;

                    }
                    mergetext += "</mergeCells>";
                    xmltext.Append(mergetext);
                }

                xmltext.Append(GetPageMargins());
                string _xmltext = xmltext.ToString();
                xmltext = new StringBuilder();
                xmltext.Append(GetSheetHeadandColumns() + _xmltext.ToString());
                //Nagaraju 27-03-2014
                xmlstring = xmltext.ToString();
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                //exportfiles = new Dictionary<string, string>();
                //exportfiles.Add("tab1", xmltext);
                //HttpContext.Current.Session["exportfiles"] = exportfiles;
                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }
        public string WriteFilters()
        {
            string value = "";
            switch (accuratestatvalueposi.ToString())
            {
                case "1.2816":
                    value = "80";
                    break;
                case "1.6449":
                    value = "90";
                    break;

                case "1.96":
                    value = "95";
                    break;

                case "2.5758":
                    value = "99";
                    break;
            }
            string xmlstring = "* Selection";
            StringBuilder xmltext = new StringBuilder();
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<row " +
           "r = \"1\" " +
           "spans = \"1:11\" " +
           "ht = \"15\" " +
           "thickBot = \"1\" " +
           "x14ac:dyDescent = \"0.3\">" +
           "<c " +
               "r = \"B1\" " +
               "s = \"9\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ");

            xmlstring = "* Filters";
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<c " +
               "r = \"C1\" " +
               "s = \"9\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ");


            xmlstring = "Stat Test:";
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<c " +
               "r = \"D1\" " +
               "s = \"10\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ");

            xmltext.Append("</row>");

            //Time Period
            //if (!string.IsNullOrEmpty(TimePeriod))
            //{
            //    if (TimePeriod.IndexOf("3MMT") > -1)
            //    {
            //        xmlstring = "Time Period : " + TimePeriod.Split('|')[1] + " 3MMT";
            //    }
            //    else if (TimePeriod.IndexOf("total") > -1)
            //    {
            //        xmlstring = "Time Period : AUG 2013 TO JUN 2014";
            //    }
            //    else
            //    {

            //        xmlstring = "Time Period : " + TimePeriod.Split('|')[1];
            //    }
            //}
            //else
            //{
            //    xmlstring = "Time Period :";
            //}
            xmlstring = "Time Period : " + TimePeriod;
            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<row " +
   "r = \"2\" " +
   "spans = \"1:11\" " +
   "ht = \"15\" " +
   "thickBot = \"1\" " +
   "x14ac:dyDescent = \"0.3\">" +
   "<c " +
       "r = \"B2\" " +
       "s = \"11\" " +
       "t = \"s\">" +
       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
   "</c>");

            if (BenchMark.IndexOf("Category") > -1 || BenchMark.IndexOf("Brand") > -1 || CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1 || CheckString.ToLower().IndexOf("totaltrip") > -1)
            {
                if (frequency.IndexOf("channels") > -1 || frequency.IndexOf("retailers") > -1)
                {
                    string[] cr = frequency.Split(new String[] { "|", "|" },
                                   StringSplitOptions.RemoveEmptyEntries);
                    string text = string.Empty;
                    for (int i = 1; i < cr.Length; i += 2)
                    {
                        text += Get_ShortNames(cr[i]) + ", ";
                    }
                    xmlstring = "Channel/Retailer : " + text;
                }
                else
                {
                    if (frequency == "Total")
                    {
                        xmlstring = "Channel/Retailer : " + frequency;
                    }
                    else
                    {
                        xmlstring = "Monthly Purchasing Amount : " + frequency;
                    }
                }
            }
            else
            {
                if (frequency == "Total")
                {
                    xmlstring = "Channel/Retailer : " + frequency;
                }
                else
                {
                    if (frequency.IndexOf("channels") > -1 || frequency.IndexOf("retailers") > -1)
                    {
                        string[] cr = frequency.Split(new String[] { "|", "|" },
                                       StringSplitOptions.RemoveEmptyEntries);
                        string text = string.Empty;
                        for (int i = 1; i < cr.Length; i += 2)
                        {
                            text += Get_ShortNames(cr[i]) + ", ";
                        }
                        xmlstring = "Channel/Retailer : " + text;
                    }
                    else
                    {
                        //xmlstring = "Shopping Frequency: " + frequency;
                        xmlstring = "Shopping Frequency: " + Get_ShortNamesFrequency(frequency).Replace("Past 3 Month", "Quarterly +");
                    }
                }

            }
            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<c " +
           "r = \"C2\" " +
           "s = \"11\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ");

            xmlstring = ">" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<c " +
               "r = \"D2\" " +
               "s = \"12\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ");

            xmltext.Append("</row>");

            //Single Selection
            if (param.ShopperSegment != "")
            {
                if (CheckString.IndexOf("Channel") > -1 || CheckString.IndexOf("Retailer") > -1 || CheckString.ToLower().IndexOf("totalshopper") > -1)
                {
                    if (AddTradeAreaNoteforChannel(param.ShopperSegment) != string.Empty)
                    {
                        xmlstring = "Channel/Retailer : " + param.ShopperSegment + AddTradeAreaNoteforChannel(param.ShopperSegment);
                    }
                    else
                    {
                        xmlstring = "Channel/Retailer : " + param.ShopperSegment;
                    }
                }
                else if (CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1 || CheckString.ToLower().IndexOf("totaltrip") > -1)
                {
                    xmlstring = "Category/Brand : " + param.ShopperSegment;
                }
            }
            else
            {
                xmlstring = "";
            }
            // xmlstring = "Single Selection";
            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<row " +
     "r = \"3\" " +
     "spans = \"1:11\" " +
     "ht = \"15\" " +
     "thickBot = \"1\" " +
     "x14ac:dyDescent = \"0.3\">" +
     "<c " +
         "r = \"B3\" " +
         "s = \"13\" " +
         "t = \"s\">" +
         "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
     "</c>");


            string CustomFilter = cf.GetExcelSortedFilters(param.CustomFilters);

            //string[] ss = param.CustomFilters.Split(new String[] { "|", "|" },
            //                        StringSplitOptions.RemoveEmptyEntries);

            //for (int i = 0; i < ss.Length; i += 2)
            //{
            //    ss[i] = ss[i] + ": ";
            //}

            //for (int i = 1; i < ss.Length; i += 2)
            //{
            //    ss[i] = ss[i] + ", ";
            //}
            //foreach (string xmlfilter in ss)
            //{
            //    CustomFilter += xmlfilter;
            //}
            if (CustomFilter != "")
                xmlstring = CustomFilter;
            else
                xmlstring = " : ";

            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }
            xmltext.Append("<c " +
           "r = \"C3\" " +
           "s = \"13\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ");

            xmlstring = "<" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext.Append("<c " +
               "r = \"D3\" " +
               "s = \"14\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ");

            xmltext.Append("</row>");
            //Single Selection

            //       xmlstring = "Single Selection";
            //       xmlstring = cf.cleanExcelXML(xmlstring);
            //      if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            //       {
            //           sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            //       }

            //       xmltext.Append("<row " +
            //"r = \"3\" " +
            //"spans = \"1:11\" " +
            //"ht = \"15\" " +
            //"thickBot = \"1\" " +
            //"x14ac:dyDescent = \"0.3\">" +
            //"<c " +
            //    "r = \"B4\" " +
            //    "s = \"13\" " +
            //    "t = \"s\">" +
            //    "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
            //"</c></row> ";

            return xmltext.ToString();

        }
        #region Add sample size note
        private string AddSampleSizeNote()
        {
            string samplesizenote = cf.cleanExcelXML("NOTE : GREY FONT = LOW SAMPLE (30-99), BLANK = SAMPLE < 30; NA = NOT APPLICABLE");
            StringBuilder xmltext = new StringBuilder();
            if (!sharedStrings.ContainsKey(samplesizenote))
            {
                sharedStrings.Add(samplesizenote, sharedStrings.Count());
            }
            xmltext.Append("<row" +
                " r = \"4\" " +
                 "spans = \"1:11\" " +
                 "ht = \"16.5\" " +
                 "thickTop = \"1\" " +
                 "thickBot = \"1\" " +
                 "x14ac:dyDescent = \"0.3\">" +
                 "<c " +
                     "r = \"C4\" " +
                     "s = \"13\" " +
                     "t = \"s\">" +
                    "<v>" + sharedStrings[samplesizenote] + "</v>" +
                 "</c>");

            xmltext.Append("</row> ");
            return xmltext.ToString();
        }
        #endregion
        private string GetTableHeader(int comparisons, string viewtype)
        {
            StringBuilder xmltext = new StringBuilder();
            if (!sharedStrings.ContainsKey(viewtype))
            {
                sharedStrings.Add(viewtype, sharedStrings.Count());
            }
            xmltext.Append("<row" +
                " r = \"5\" " +
                 "spans = \"1:11\" " +
                 "ht = \"16.5\" " +
                 "thickTop = \"1\" " +
                 "thickBot = \"1\" " +
                 "x14ac:dyDescent = \"0.3\">" +
                 "<c " +
                     "r = \"B5\" " +
                     "s = \"18\" " +
                     "t = \"s\">" +
                    "<v>" + sharedStrings[viewtype] + "</v>" +
                 "</c>" +
                 "<c " +
                     "r = \"C5\" " +
                     "s = \"18\" " +
                     "t = \"s\">" +
                 "</c>");

            xmltext.Append("</row> ");
            return xmltext.ToString();
        }

        public string ColumnIndexToName(int columnIndex)
        {
            char second = (char)(((int)'A') + columnIndex % 26);

            columnIndex /= 26;

            if (columnIndex == 0)
                return second.ToString();
            else
                return ((char)(((int)'A') - 1 + columnIndex)).ToString() + second.ToString();
        }
        private string Get_ShortNames(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (HeaderTabs.ContainsKey(spVal))
                    slRetVal = HeaderTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        private bool CheckTotal_DeliveryMethodUseItem(string item)
        {
            switch (item.ToLower())
            {
                case "total shopper":
                case "total online shopper":
                case "total online grocery shopper":
                    {
                        IsApplicable = false;
                        return true;
                    }
            }
            return false;
        }
        private bool CheckSampleSize(string samplesizekey)
        {
            IsApplicable = true;
            try
            {
                if (sampleSize.ContainsKey(samplesizekey))
                {
                    string val = sampleSize[samplesizekey];
                    if (string.IsNullOrEmpty(val))
                        return false;
                    double szvalue = Convert.ToDouble(sampleSize[samplesizekey]);
                    //added by Nagaraju for DELIVERY METHOD USE
                    //date: 24-04-2017

                    if (View_Type.Equals("COMPARE", StringComparison.OrdinalIgnoreCase) && LoyaltyPyramidmetric.Equals("DELIVERY METHOD USE", StringComparison.OrdinalIgnoreCase)
                        && TabIndexId == 2)
                    {
                        CheckTotal_DeliveryMethodUseItem(samplesizekey);
                    }
                    else if ((View_Type.Equals("PIT", StringComparison.OrdinalIgnoreCase) && LoyaltyPyramidmetric.Equals("DELIVERY METHOD USE", StringComparison.OrdinalIgnoreCase)
                       && TabIndexId == 2)
                        || (View_Type.Equals("TREND", StringComparison.OrdinalIgnoreCase) && LoyaltyPyramidmetric.Equals("DELIVERY METHOD USE", StringComparison.OrdinalIgnoreCase)
                     && TabIndexId == 2))
                    {
                        CheckTotal_DeliveryMethodUseItem(param.ShopperSegment);
                    }

                    if (!IsApplicable)
                        return false;

                    if (szvalue >= GlobalVariables.MaxSampleSize)
                    {
                        return true;
                    }
                    else
                    {
                        //cellfontstyle = 10;
                        if (szvalue == GlobalVariables.NANumber)
                        {
                            IsApplicable = false;
                        }
                        return false;
                    }
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }   

        private string CheckNAValues(string value)
        {
            string NA = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (!IsApplicable)
            {
                return NA = GlobalVariables.NA;
            }
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                return NA = GlobalVariables.NA;
            }
            if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
            {
                return NA = GlobalVariables.NA;
            }
            if (CheckSampleSize(BenchmarkOrComparison) == false && CommonFunctions.CheckMediumSampleSize(BenchmarkOrComparison, sampleSize) == false && CheckRetailerorChannel == false)
            {
                return NA;
            }
            if (string.IsNullOrEmpty(value) && LoyaltyPyramid) //&& LoyaltyPyramid)
            {
                NA = GlobalVariables.NA;

            }
            else if ((StoreImageryCheck || CheckRetailerorChannel || LoyaltyPyramidForRetailers) || (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips)))
            {
                NA = GlobalVariables.NA;
            }
            return NA;
        }             
      
        private int GetRowNumber(DataTable tbl, int currentrow)
        {
            int rownum = 0;
            if (tbl.Rows.Count > currentrow + 1)
            {
                rownum = currentrow + 1;
            }
            else
            {
                rownum = currentrow;
            }

            return rownum;

        }
        private string GetLoyaltyPyramidName(string value)
        {
            string Name = string.Empty;
            if (LoyaltyRetailerList.ContainsKey(value))
            {
                Name = Convert.ToString(LoyaltyRetailerList[value]);
            }
            return Name;
        }
        private string CheckXMLBlankValues(string rowvalue)
        {
            NA_Text = string.Empty;
            string value = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!sharedStrings.ContainsKey(value))
                {
                    sharedStrings.Add(value, sharedStrings.Count());
                }
                return sharedStrings[value].ToString();
            }
            else if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!sharedStrings.ContainsKey(value))
                {
                    sharedStrings.Add(value, sharedStrings.Count());
                }
                return sharedStrings[value].ToString();
            }
            else if ((StoreImageryCheck && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))) ||
                 (CheckRetailerorChannel && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))) || (LoyaltyPyramidForRetailers && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase)))
                 || (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
                || CheckRetailerorChannel)
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!sharedStrings.ContainsKey(value))
                {
                    sharedStrings.Add(value, sharedStrings.Count());
                }
                return sharedStrings[value].ToString();
            }
            else if (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips))
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!sharedStrings.ContainsKey(value))
                {
                    sharedStrings.Add(value, sharedStrings.Count());
                }
                return sharedStrings[value].ToString();
            }
            else if (string.IsNullOrEmpty(rowvalue))
            {
                value = "";
            }
            else
            {
                double val = Convert.ToDouble(rowvalue) / 100;
                value = val.ToString();
            }
            return value;
        }

        private string CheckBlankValues(string rowvalue)
        {
            NA_Text = string.Empty;
            string value = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
            }
            else if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
            }
            else if ((StoreImageryCheck && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))) ||
                (CheckRetailerorChannel && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))) || (LoyaltyPyramidForRetailers && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase)))
                || (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
                || CheckRetailerorChannel)
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
            }
            else if (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips))
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
            }
            else
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }

                else
                {
                    value = CommonFunctions.GetRoundingValue(rowvalue) + "%";
                }
            }
            return value;
        }

        private string CheckBlankValues(string rowvalue, string tablename)
        {
            string value = string.Empty;

            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                value = GlobalVariables.NA;
            }
            else if (tablename == "# of Items Purchased")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }
            else if (tablename == "Time Spent in Store")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minute";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minutes";
                }
            }
            else if (tablename == "Trip Expenditure")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }

            return value;
        }

        private string GetCellColor(string currentrow, string significancerow, string significancevalue)
        {
            string color = string.Empty;
            cellfontstylegrey = 19;
            cellfontstyle = 8;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            color = "color:black";
            if (String.Compare(average, "Average", true) == 0)
            {
                color = "color:black";
                cellfontstyle = 4;
            }
            else if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 30;
                }
                else
                {
                    color = "color:black";
                    cellfontstyle = 4;
                }
            }
            else if (significancevalue != "" || LoyaltyPyramidForRetailers || Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) || CheckBeverageTripNA || RetailerNetCheck)
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
                    {
                        if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                        {
                            color = "color:blue";
                            cellfontstyle = 30;
                        }
                        else
                        {
                            color = "color:black";
                            cellfontstyle = 4;
                        }
                    }
                    else if ((LoyaltyPyramidForRetailers && LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase)) ||
                        (CheckRetailerorChannel && LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
                        || CheckRetailerorChannel)
                    {
                        if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                        {
                            color = "color:blue";
                            cellfontstyle = 30;
                        }
                        else
                        {
                            color = "color:black";
                            cellfontstyle = 4;
                        }
                    }
                    else if (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips))
                    {
                        if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                        {
                            color = "color:blue";
                            cellfontstyle = 30;
                        }
                        else
                        {
                            color = "color:black";
                            cellfontstyle = 4;
                        }

                    }
                    else if (IsApplicable && BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstyle = 28;
                    }
                    else if (!IsApplicable && BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstyle = 30;
                    }
                    else if (!string.IsNullOrEmpty(significancevalue) && (Convert.ToDouble(significancevalue) > accuratestatvalueposi))
                    {
                        color = "color:#20B250";

                        cellfontstyle = 7;
                    }
                    else if (!string.IsNullOrEmpty(significancevalue) && (Convert.ToDouble(significancevalue) < accuratestatvaluenega))
                    {
                        color = "color:red";
                        cellfontstyle = 6;
                    }
                    else if (!string.IsNullOrEmpty(significancevalue) && (Convert.ToDouble(significancevalue) <= accuratestatvalueposi) && (Convert.ToDouble(significancevalue) >= accuratestatvaluenega))
                    {
                        color = "color:black";
                        cellfontstyle = 8;
                    }
                    else if (RetailerNetCheck)
                    {
                        color = "color:black";
                        cellfontstyle = 4;
                    }

                }
                else if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 30;
                }
            }
            else if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
            {
                color = "color:blue";
                cellfontstyle = 30;
            }
            else
            {
                if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 30;
                }
                else
                {
                    color = "color:black";
                    cellfontstyle = 8;
                }
            }
            return color;
        }

        private string GetCellColorGrey(string currentrow, string significancerow, string significancevalue)
        {
            string color = string.Empty;
            cellfontstylegrey = 19;
            cellfontstyle = 8;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 30;
                }
                else
                {
                    color = "color:black";
                    cellfontstyle = 4;
                }
            }
            else if (significancevalue != "" || LoyaltyPyramidForRetailers || Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) || RetailerNetCheck)
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
                    {
                        if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                        {
                            color = "color:blue";
                            cellfontstyle = 31;
                        }
                        else
                        {
                            color = "color:black";
                            cellfontstyle = 4;
                        }
                    }
                    else if ((LoyaltyPyramidForRetailers && LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase)) ||
                      (CheckRetailerorChannel && LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
                        || CheckRetailerorChannel)
                    {
                        if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                        {
                            color = "color:blue";
                            cellfontstyle = 31;
                        }
                        else
                        {
                            color = "color:black";
                            cellfontstyle = 4;
                        }
                    }
                    else if (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips))
                    {
                        if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                        {
                            color = "color:blue";
                            cellfontstyle = 30;
                        }
                        else
                        {
                            color = "color:black";
                            cellfontstyle = 4;
                        }

                    }
                    else if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstylegrey = 29;
                    }
                    else if (!string.IsNullOrEmpty(significancevalue) && (Convert.ToDouble(significancevalue) <= accuratestatvalueposi) && (Convert.ToDouble(significancevalue) >= accuratestatvaluenega))
                    {
                        //color = "color:black";
                        //cellfontstylegrey = 19;
                        color = "color:#878787";
                        cellfontstyle = 19;
                    }
                    else if (!string.IsNullOrEmpty(significancevalue) && (Convert.ToDouble(significancevalue) < accuratestatvaluenega))
                    {
                        color = "color:red";
                        cellfontstylegrey = 23;
                    }
                    else if (!string.IsNullOrEmpty(significancevalue) && (Convert.ToDouble(significancevalue) > accuratestatvalueposi))
                    {
                        color = "color:#20B250";
                        cellfontstylegrey = 22;
                    }
                    else if (RetailerNetCheck)
                    {
                        color = "color:black";
                        cellfontstyle = 4;
                    }
                    else {
                        color = "color:#878787";
                        cellfontstyle = 19;
                    }
                }
            }
            else
            {
                if (BenchmarkOrComparison.Equals(BenchMark.Replace("~", "`"), StringComparison.OrdinalIgnoreCase) && Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 31;
                }
                else
                {
                    color = "color:#878787";
                    cellfontstyle = 19;
                }
            }
            return color;
        }

        private string GetPageMargins()
        {
            string pagem = "<pageMargins " +
                "left = \"0.7\" " +
                "right = \"0.7\" " +
                "top = \"0.75\" " +
                "bottom = \"0.75\" " +
                "header = \"0.3\" " +
                "footer = \"0.3\"/>" +
                 "<pageSetup " +
                "paperSize = \"9\" " +
                "orientation = \"portrait\" " +
                "r:id = \"rId1\"/>" +
                "<drawing r:id = \"rId2\"/>";

            return pagem;

        }

        private string CheckXMLBlankValues(string rowvalue, string tablename)
        {
            string value = string.Empty;
            string valuereturn = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                value = GlobalVariables.NA;
            }
            else if (tablename == "# of Items Purchased")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }

            else if (tablename == "Time Spent in Store")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minute";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minutes";
                }
            }

            else if (tablename == "Trip Expenditure")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    double val = Convert.ToDouble(rowvalue) / 100;
                    value = val.ToString();
                }
            }

            valuereturn = cf.cleanExcelXML(value);
            if (!sharedStrings.ContainsKey(valuereturn))
            {
                sharedStrings.Add(valuereturn, sharedStrings.Count());
            }
            //else
            //{
            //    valuereturn = Convert.ToString(sharedStrings[valuereturn]);
            //}
            return valuereturn;
        }

        private string GetSheetHeadandColumns()
        {
            if (colmaxwidth <= 40)
            {
                colmaxwidth = 40;
            }

            string sheetstr = "xmlns = \"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
        "xmlns:r = \"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" " +
        "xmlns:mc = \"http://schemas.openxmlformats.org/markup-compatibility/2006\" " +
        "mc:Ignorable = \"x14ac\" " +
        "xmlns:x14ac = \"http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac\"> " +
        "<dimension ref = \"A1\"/> " +
        "<sheetViews> " +
            "<sheetView showGridLines = \"0\" tabSelected = \"1\" workbookViewId = \"0\"> " +
                "<selection activeCell = \"A1\" sqref = \"A1\"/> " +
           "</sheetView> " +
        "</sheetViews> " +
        "<sheetFormatPr defaultRowHeight = \"15\" x14ac:dyDescent = \"0.25\"/> " +
        "<cols> ";

            for (int i = 0; i < (complist.Count + 1); i++)
            {
                if (i == 0)
                {
                    sheetstr += "<col " +
                   "min = \"" + (i + 1) + "\" " +
                   "max = \"" + (i + 1) + "\" " +
                   "width = \"" + (colmaxwidth + 10).ToString() + "\" " +
                   "customWidth = \"1\"/> ";
                }
                else
                {
                    sheetstr += "<col " +
                 "min = \"" + (i + 1) + "\" " +
                "max = \"" + (i + 1) + "\" " +
                 "width = \"30\" " +
                 "customWidth = \"1\"/> ";
                }
            }
            sheetstr += "</cols>";
            return sheetstr;
        }

        public void Shortnames()
        {
            FilterTabs.Clear();
            FilterTabs.Add("MainStore", "Main Store (in channel)");
            FilterTabs.Add("MainStoreOverAll", "Main Store (across channel)");
            FilterTabs.Add("FavoriteStore", "Favorite Store (in channel)");
            FilterTabs.Add("FavoriteStoreOverAll", "Favorite Store (across channel)");
        }

        private string Get_ShortNamesFrequency(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (FilterTabs.ContainsKey(spVal))
                    slRetVal = FilterTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }


    }
}
