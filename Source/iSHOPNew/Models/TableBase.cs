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
    public abstract class TableBase
    {
        public string Table_Header_BackgroundColor = "#D9E1EE";
        public string Table_Header_BorderTopColor = "skyblue";
        public string Table_Header_BottomTitleColor = "#72aaff";
        public int Table_Header_TotalUS_StyleId = 38;

        public bool isItemHasSpace = false;
        public TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        public TableParams table_Params = new TableParams();
        public string MetricItem = string.Empty;
        public bool itemboldcontent = false;
        public string View_Type = string.Empty;
        public int TabIndexId = 0;
        public bool isitembold = false;
        public bool IsApplicable = true;
        public bool common_SampleSize = false;
        public int table_count = 0;
        public int rows_count = 0;
        public string BenchMark = string.Empty;
        public string ShopperSegment = string.Empty;
        public string ShopperFrequency = string.Empty;
        public string CheckString = string.Empty;
        public string TimePeriod = string.Empty;
        public Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, string> FilterTabs = new Dictionary<string, string>();
        public Dictionary<string, string> TableMappingList = new Dictionary<string, string>();
        public Dictionary<string, string> sampleSize = new Dictionary<string, string>();
        public Dictionary<string, string> exportfiles = new Dictionary<string, string>();
        public string currentTab = string.Empty;
        public string average = string.Empty;

        public Hashtable LoyaltyRetailerList = new Hashtable();
        public iSHOPParams param = new iSHOPParams();
        public iSHOPParams getvalue = new iSHOPParams();
        public string[] Retailerlist = null;
        public List<string> complist = new List<string>();
        public Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        public Dictionary<string, string> selectedsheets = new Dictionary<string, string>();
        public List<string> mergeCell = new List<string>();
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public int colmaxwidth = 0;
        public int cellfontstyle = 8;
        //Nishanth
        public int cellfontstylegrey = 19;
        public int samplecellstyle = 4;
        public string activetab = string.Empty;
        public DataSet demo = new DataSet();
        public DataSet general = new DataSet();
        public DataSet postshop = new DataSet();
        public DataSet bdetail = new DataSet();
        public string userRole { get; set; }
        public string frequency = "";
        public CommonFunctions _commonfunctions = new CommonFunctions();

        public string leftheader = string.Empty;
        public string leftbody = string.Empty;
        public string rightheader = string.Empty;
        public string righttbody = string.Empty;
        public double ul_row_width = 0;
        public double ul_cell_width = 0;
        public bool LoyaltyPyramid = false;
        public bool RetailerNetCheck = false;

        public string LoyaltyPyramidmetric = string.Empty;

        public string BenchmarkOrComparison;
        public string SelectedStatTest = string.Empty;

        public bool LoyaltyPyramidForRetailers = false;
        public bool StoreImageryCheck = false;
        public bool CheckRetailerorChannel = false;
        public string NA_Text = string.Empty;
        public List<string> BenchmarkorComparisionList;

        //Added by Bramhanath for BeverageTripsDetails Tab NA(09-12-2015)
        public bool CheckBeverageTripNA = false;
        public Hashtable CheckBeverageTripNAhTbl = new Hashtable();
        public string checkBevTotalTrips = "totaltrip||totaltrip";
        public int isBevTotalTrips = 0;
        //

        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        public bool isBeverageDetail = false;
        public bool isLiquidFlavorEnhancer = false;
        //
         public abstract iSHOPParams BindTabs(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string timePeriod, string _ShopperSegment, string filterShortname, string _ShopperFrequency, string[] ShortNames, string StatPositive, string StatNegative, bool ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams);
         public abstract iSHOPParams BindTabsWithin(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string timePeriod, string _ShopperSegment, string _SingleSelection, string _ShopperFrequency, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, bool ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams);
         public abstract iSHOPParams BindTabsTimePeriod(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string _ShopperSegment, string _SingleSelection, string _ShopperFrequency, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, bool ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams);
         public abstract bool CheckTotal_DeliveryMethodUseItem(string item);
         public abstract bool CheckSampleSize(string samplesizekey);
         public abstract string CheckNAValues(string value);
         public abstract string CheckBlankValues(string rowvalue, string tablename, string metricItem);
         public abstract string CheckXMLBlankValues(string rowvalue, string tablename, string metricItem);
         public abstract string WriteFilters();

        public TableBase()
        {
            leftheader = string.Empty;
            leftbody = string.Empty;
            rightheader = string.Empty;
            righttbody = string.Empty;
        }

        //Nagaraju D 25-03-2014
        //clean class
        public string CleanClass(string _class)
        {
            _class = Regex.Replace(_class, @"[/\s,.`/@#$%;&*~()+?/]", "");
            return _class;
        }

        public void PopulateShortNames()
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

        //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
        //Date: 21-03-2016
        public bool Check_Beverage_Liquid_Flavor_Enhancer(string tableName, string columnName)
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

        public string Check_Beverage_Liquid_Flavor_Enhancer_NA_Table(string tableName)
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

        public string Get_Beverage_Liquid_Flavor_Enhancer_Note()
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

        public bool Is_Beverage_Detail_NA_Column(string columnName)
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

        #region Add sample size note
        public string AddSampleSizeNote()
        {
            string samplesizenote = cf.cleanExcelXML("NOTE : GREY FONT = LOW SAMPLE (30-99), BLANK = SAMPLE < 30; NA = NOT APPLICABLE");
            StringBuilder xmltext = new StringBuilder();
            if (!CheckSharedStringValue(samplesizenote))
            {
                AddToSharedString(samplesizenote);
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
                    "<v>" + GetSharedStringKey(samplesizenote) + "</v>" +
                 "</c>");

            xmltext.Append("</row> ");
            return xmltext.ToString();
        }
        #endregion

        public string GetTableHeader(int comparisons, string viewtype)
        {
            StringBuilder xmltext = new StringBuilder();
            if (!CheckSharedStringValue(viewtype))
            {
                AddToSharedString(viewtype);
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
                    "<v>" + GetSharedStringKey(viewtype) + "</v>" +
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

        public string Get_ShortNames(String spVal)
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

        public string AddTradeAreaNoteforChannel(string ChannelRetailer)
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

        public void AddToSharedString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!sharedStrings.ContainsKey(value.ToUpper()))
                {
                    sharedStrings.Add(value.ToUpper(), sharedStrings.Count());
                }
            }
        }

        public int GetSharedStringKey(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (sharedStrings.ContainsKey(value.ToUpper()))
                {
                    return sharedStrings[value.ToUpper()];
                }
            }
            return 0;
        }

        public bool CheckSharedStringValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (sharedStrings.ContainsKey(value.ToUpper()))
                    return true;
                else
                    return false;
            }
            return false;
        }

        public string Get_ShortNamesFrequency(String spVal)
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

        //Nagaraju D 07-04-2014
        //Create first header
        public string CreateFirstTableHeader()
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

        public string CreateFirstTableHeaderOvertime()
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

        public void Shortnames()
        {
            FilterTabs.Clear();
            FilterTabs.Add("MainStore", "Main Store (in channel)");
            FilterTabs.Add("MainStoreOverAll", "Main Store (across channel)");
            FilterTabs.Add("FavoriteStore", "Favorite Store (in channel)");
            FilterTabs.Add("FavoriteStoreOverAll", "Favorite Store (across channel)");
        }

        public string GetSheetHeadandColumns()
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

        public string GetPageMargins()
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

        public string GetCellColor(string currentrow, string significancerow, string significancevalue)
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
                    color = "color:#878787";
                    cellfontstyle = 8;
                }
            }
            return color;
        }

        public string GetCellColorGrey(string currentrow, string significancerow, string significancevalue)
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
                    color = "color:gray";
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
                            color = "color:gray";
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
                            color = "color:gray";
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
                            color = "color:gray";
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
                        color = "color:gray";
                        cellfontstylegrey = 19;
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
                        color = "color:gray";
                        cellfontstyle = 4;
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

        public string GetLoyaltyPyramidName(string value)
        {
            string Name = string.Empty;
            if (LoyaltyRetailerList.ContainsKey(value))
            {
                Name = Convert.ToString(LoyaltyRetailerList[value]);
            }
            return Name;
        }

        public int GetRowNumber(DataTable tbl, int currentrow)
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

        public string CheckBlankValues(string rowvalue)
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
                if (rowvalue == "-9999")
                {
                    value = GlobalVariables.NA;
                    NA_Text = GlobalVariables.NA;
                }
                else if (string.IsNullOrEmpty(rowvalue))
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

        public string CheckXMLBlankValues(string rowvalue)
        {
            NA_Text = string.Empty;
            string value = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!CheckSharedStringValue(value))
                {
                    AddToSharedString(value);
                }
                return GetSharedStringKey(value).ToString();
            }
            else if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!CheckSharedStringValue(value))
                {
                    AddToSharedString(value);
                }
                return GetSharedStringKey(value).ToString();
            }
            else if ((StoreImageryCheck && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))) ||
                 (CheckRetailerorChannel && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))) || (LoyaltyPyramidForRetailers && (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase)))
                 || (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
                || CheckRetailerorChannel)
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!CheckSharedStringValue(value))
                {
                    AddToSharedString(value);
                }
                return GetSharedStringKey(value).ToString();
            }
            else if (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips))
            {
                value = GlobalVariables.NA;
                NA_Text = GlobalVariables.NA;
                if (!CheckSharedStringValue(value))
                {
                    AddToSharedString(value);
                }
                return GetSharedStringKey(value).ToString();
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
            return value.ToUpper();
        }       

    }
}