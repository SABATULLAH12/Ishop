using iSHOPNew.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Xml;

namespace iSHOPNew.Models
{
    public class Total
    {
        bool IsApplicable = true;
        public string BenchMark = string.Empty;
        public string Compare1 = string.Empty;
        public string Compare2 = string.Empty;
        public string Compare3 = string.Empty;
        public string Compare4 = string.Empty;
        public string Compare5 = string.Empty;
        public string ShopperSegment = string.Empty;
        public string ShopperFrequency = string.Empty;
        public string CheckString = string.Empty;
        public string TimePeriod = string.Empty;
        public Dictionary<string, string> HeaderTabs = new Dictionary<string, string>();
        public Dictionary<string, string> FilterTabs = new Dictionary<string, string>();
        public Dictionary<string, string> TableMappingList = new Dictionary<string, string>();
        public Dictionary<string, string> sampleSize = new Dictionary<string, string>();
        public Dictionary<string, string> exportfiles = new Dictionary<string, string>();
        private string currentTab = string.Empty;
        public string average = string.Empty;

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
        int cellfontstyle = 5;
        //Nishanth
        int cellfontstylegrey = 13;
        int samplecellstyle = 4;
        string activetab = string.Empty;
        DataSet demo = new DataSet();
        DataSet general = new DataSet();
        DataSet postshop = new DataSet();
        DataSet bdetail = new DataSet();
        public string userRole { get; set; }
        public string frequency = "";
        CommonFunctions _commonfunctions = new CommonFunctions();
        string shortTimePeriod = string.Empty;
        string leftheader = string.Empty;
        string leftbody = string.Empty;
        string rightheader = string.Empty;
        string righttbody = string.Empty;

        string SelectedBenchmark = string.Empty;
        string BenchmarkOrComparison;
        string SelectedStatTest = string.Empty;

        public Total()
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
            HeaderTabs.Add("ItemsPurchased", "Items Purchased");
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

            HeaderTabs.Add("VisitPlans", "Pre Trip Planning");
            HeaderTabs.Add("VisitPreparation", "Preparation Types");
            HeaderTabs.Add("TechnologyUsed", "Use of Technology to Prepare");
            HeaderTabs.Add("ComputerBased", "Computer-Based Preparation Activities");
            HeaderTabs.Add("SmartPhoneBased", "Smartphone-Based Preparation Activities");
            HeaderTabs.Add("DestinationItemSummary", "Destination Item Summary");


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

            HeaderTabs.Add("RetailerLoyaltyPyramid", "Retailer Loyalty Pyramid - Total Grocery Across Channel(<span style=\"font-size:15px;\">Applicable only for Retailers</span>)");
            HeaderTabs.Add("TopBox", "Loyalty and Satisfaction Detail(<span style=\"font-size:15px;\">Applicable only for Retailers</span>)");
            HeaderTabs.Add("MainFavoriteStore", "Main/Favorite Store for Grocery Spending(<span style=\"font-size:15px;\">Applicable only for Retailers</span>)");

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

            HeaderTabs.Add("InStoreDestinationSummary", "Items Purchased Summary");
            HeaderTabs.Add("InStoreDestinationDetails", "Items Purchased Detail");

            HeaderTabs.Add("TripMission", "Trip Mission");

            HeaderTabs.Add("TimeSpent", "Time Spent in Store");
            HeaderTabs.Add("TripExpenditure", "Trip Expenditure");
            HeaderTabs.Add("CheckOutType", "Checkout Method");

            HeaderTabs.Add("RetailerLoyaltyPyramid(Base:CouldShop)", "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)");

            HeaderTabs.Add("RedeemedCoupon", "Coupon Redemption");
            HeaderTabs.Add("RedeemedCouponTypes", "Type of Coupons Redeemed");
            HeaderTabs.Add("DestinationStoreTrip", "Destination Following Store Trip");
            HeaderTabs.Add("TripSatisfaction", "Trip Satisfaction");

            HeaderTabs.Add("Diet carbonated soft drinks", "Diet SSD");
            HeaderTabs.Add("Regular (non-diet) carbonated soft drinks", "REG SSD");
            HeaderTabs.Add("Bottled water", "Water");
            HeaderTabs.Add("Juice or juice drinks", "Juice");
            HeaderTabs.Add("Iced tea in bottles, cans, or cartons", "Iced Tea");
            HeaderTabs.Add("Coffee in bottles or cans", "Coffee");
            HeaderTabs.Add("Sports drinks", "Sports Drinks");
            HeaderTabs.Add("Energy drinks", "Energy Drinks");

            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(REG SSD)", "Do not purchase(REG SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(DIET SSD)", "Do not purchase(DIET SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(WATER)", "Do not purchase(WATER)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(JUICE)", "Do not purchase(JUICE)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(SPORTS DRINKS)", "Do not purchase(SPORTS DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(ENERGY DRINKS)", "Do not purchase(ENERGY DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD TEA)", "Do not purchase(RTD TEA)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD COFFEE)", "Do not purchase(RTD COFFEE)");
            HeaderTabs.Add("past 3 month", "Quarterly +");

            HeaderTabs.Add("VisitMotiviations", "Visit Motiviations");
            HeaderTabs.Add("ReasonForStoreChoice", "Reason For Store Choice");
            HeaderTabs.Add("DestinationItemDetails", "Destination Item Details");
            HeaderTabs.Add("MostImportantDestinationItems", "Most Important Destination Items");
            HeaderTabs.Add("ImpulseItem", "Impulse Item");
            HeaderTabs.Add("PaymentMode", "Payment Mode");
            HeaderTabs.Add("TripAttributeSatisfaction", "Trip Attribute Satisfaction");


        }

        //Nagaraju D 25-03-2014
        //clean class
        public string CleanClass(string _class)
        {
            _class = Regex.Replace(_class, @"[/\s,`/@#$%;&*~()+/]", "");
            return _class;
        }

        //Nagaraju D 07-04-2014
        //Create first header
        private string CreateFirstTableHeader()
        {
            leftheader = "<table class=\"totalheader\"></thead>";
            rightheader = "<table class=\"totalheader\" style=\"width:100%;\"></thead>";
            string table = string.Empty;
            table += "<tr>";
            table += "<td style=\"height: 32px;width:300px;\" class=\"ShoppingFrequencytitle\">Shopping Frequency</td>";
            table += "<td class=\"benchmarktitle\" style=\"width:200px;\">BENCHMARK</td>";
            table += "<td class=\"comparisonheader\" style=\"\">COMPARISON AREAS</td>";

            //leftheader += "<tr><td style=\"height: 32px;width:300px;\" class=\"ShoppingFrequencytitle\">Shopping Frequency</td>";
            //rightheader += "<td class=\"benchmarktitle\" style=\"width:200px;\">BENCHMARK</td>";
            if (complist != null && complist.Count > 1)
                //rightheader += "<td class=\"" + CleanClass(Convert.ToString(complist[1])) + "header\" style=\"\">COMPARISON AREAS</td>";

            if (complist != null && complist.Count > 2)
            {
                for (int i = 2; i < complist.Count; i++)
                {
                    table += "<td class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"></td>";
                    //rightheader += "<td class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"></td>";
                }
            }
            //leftheader += "</tr>";
            //rightheader += "</tr>";
            table += "</tr>";
            return table;
        }

        //public iSHOPParams BindTabs(out string tbltext, out string xmlstring, string tabid, string _BenchMark, string[] _Comparisonlist, string timePeriod, string _shortTimePeriod, string _ShopperFrequency, string _measure, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, string ExportToExcel, string Selected_StatTest)
        public iSHOPParams BindTabs(out string tbltext, out string xmlstring, string tabid, string _BenchMark, string[] _Comparisonlist, string timePeriod, string _shortTimePeriod, string _ShopperFrequency, string _measure, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, string ExportToExcel, string Selected_StatTest,string TimePeriod_UniqueId, string Benchmark_UniqueIds, string Comparison_UniqueIds, string ShopperFrequency_UniqueId, string ShopperSegment_UniqueId, string MeasureUniqueIds,string Sigtype_UniqueId,string Module,string CustomBase_DBName,string CustomBase_ShortName,string CustomBase_UniqueId)
        {
            iSHOPParams ishopParams = new iSHOPParams();
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
            SelectedBenchmark = ShortNames[0];
            PopulateShortNames();
            Shortnames();
            BenchMark = _BenchMark;
            if (Selected_StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = CustomBase_ShortName;
                SelectedBenchmark = CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedBenchmark = ShortNames[0];
                SelectedStatTest = Selected_StatTest;
            }
            List<string> sCompIds = new List<string>();
            sCompIds = Comparison_UniqueIds.Split('|').ToList();
            if (Selected_StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                Benchmark_UniqueIds = CustomBase_UniqueId;
                List<string> comp_list = (from r in sCompIds where r != Benchmark_UniqueIds select r).ToList();
                Comparison_UniqueIds = string.Join("|", comp_list);
            }
            else
            {
                Benchmark_UniqueIds = sCompIds[0];
                sCompIds.RemoveAt(0);
                Comparison_UniqueIds = string.Join("|", sCompIds);
            } 

            Retailerlist = _Comparisonlist;
            param = new iSHOPParams();
            param.BenchMark = _BenchMark;
            shortTimePeriod = _shortTimePeriod;
            complist = new List<string>();
            var query = from r in ShortNames select r;
            complist = query.ToList();
            frequency = _ShopperFrequency;
            object[] paramvalues = null;
            //param.ShopperSegment = _ShopperSegment;
            TimePeriod = timePeriod;
            param.CustomFilters = filterShortname;
            DataAccess dal = new DataAccess();
            //exec [usp_crossRetailerTotalShopper] '2','1|3','7|36','','2','11','1'

            //object[] paramvalues = new object[] { _BenchMark, String.Join("|", _Comparisonlist), timePeriod, _measure, _filter, _ShopperFrequency,Selected_StatTest };
            //paramvalues = new object[] { "2", "1|3", "7|36", "", "2", "11", "1" };
            if (Module == "trips")
                tabid = "usp_ReportCrossRetailerTotalTrip";
            else if (Module == "shopper")
                tabid = "usp_ReportCrossRetailerTotalShopper";

            paramvalues = new object[] { TimePeriod_UniqueId.ToMyString(), Benchmark_UniqueIds.ToMyString(), Comparison_UniqueIds.ToMyString(), MeasureUniqueIds.ToMyString(), ShopperSegment_UniqueId.ToMyString(), ShopperFrequency_UniqueId.ToMyString(), Sigtype_UniqueId.ToMyString() };
            //DataSet ds = dal.GetData(paramvalues, tabid);
            DataSet ds = dal.GetData_WithIdMapping(paramvalues, tabid.ToMyString());

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
            tbltext = string.Empty;
            string Significance = string.Empty;
            xmlstring = string.Empty;
            colmaxwidth = 0;
            string xmltext = string.Empty;
            mergeCell = new List<string>();

            try
            {
                xmltext += "<sheetData>";
                //write top header
                xmltext += WriteFilters();
                xmltext += GetTableHeader(complist.Count);
                mergeCell.Add("<mergeCell ref = \"B5:C5\"/>");
                mergeCell.Add("<mergeCell ref = \"D5:" + ColumnIndexToName(complist.Count * 2) + "5\"/>");
                //if (complist.Count > 1)
                //{
                //    mergeCell.Add("<mergeCell ref = \"C5:" + ColumnIndexToName(complist.Count) + "5\"/>");
                //}
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
                xmltext += " <row" +
               " r = \"" + rownumber + "\" " +
                "spans = \"1:11\" " +
                "x14ac:dyDescent = \"0.25\">" +
               " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"4\"/>";
                excelcolumnindex += 1;

                tbltext = "<thead>";
                tbltext += CreateFirstTableHeader();
                tbltext += "<tr><td class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;padding: 1px;text-align: center;width:300px;\">" + Get_ShortNamesFrequency(frequency) + "</td>";

                var sWidth = "298px";
                if (complist.Count == 1)
                    sWidth = "100%";
                else if (complist.Count == 2)
                    sWidth = "48.2%";
                else
                    sWidth = "298px";

                leftheader += "<tr><td class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;padding: 1px;text-align: center;width:394.8px;min-height: 21px;\">" + Get_ShortNamesFrequency(frequency) + "</td>";
                rightheader += "<tr>";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                foreach (string Comparison in complist)
                {
                    colNames = Get_ShortNames(Comparison);
                    if (complist.IndexOf(Comparison) == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<td style=\"font-weight: bold;min-width:" + sWidth + ";\" class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;padding: 1px;text-align: center;\"><span title=\" " + colNames + " \">" +  colNames + "</span></td>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(Comparison + "header");
                        rightheader += "<td  class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;padding: 1px;text-align: center;min-width:" + sWidth + ";\"><span title=\" " + colNames + " \">" + (colNames) + "</span></td>";
                    }

                    tbltext += "<td class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;padding: 1px;text-align: center;\"><span title=\" " + colNames + " \">" + (colNames) + "</span></td>";
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                    xmlstring = cf.cleanExcelXML(colNames);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }

                    xmltext += " <c" +
                      " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                      " s = \"30\" " +
                      " t = \"s\">" +
                       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                   "</c>";
                    mergeCell.Add("<mergeCell ref = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + ":" + ColumnIndexToName(excelcolumnindex + 1) + "" + rownumber + "\"/>");
                    excelcolumnindex += 2;
                }
                tbltext += "</tr>";
                leftheader += "</tr>";
                rightheader += "</tr><tr>";
                xmltext += "</row>";

                rownumber += 1;
                excelcolumnindex = 0;
                xmltext += " <row" +
                " r = \"" + rownumber + "\" " +
                 "spans = \"1:11\" " +
                 "x14ac:dyDescent = \"0.25\">" +
                " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"4\"/>";
                excelcolumnindex += 1;

                for (int i = 0; i < complist.Count; i++)
                {
                    rightheader += "<td style=\"background-color:transparent;font-weight: normal;min-width:" + sWidth + ";\"><ul style=\"width:100%;\"><li style=\"background-color:transparent;\" class=\"FixedHeaderColumn\">%</li>";
                    xmlstring = cf.cleanExcelXML("%");
                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }
                    xmltext += " <c" +
                     " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                     " s = \"23\" " +
                     " t = \"s\">" +
                      "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                  "</c>";
                    excelcolumnindex += 1;


                    rightheader += "<li style=\"background-color:transparent;\" class=\"FixedHeaderColumn\">Sample Size</li></ul></td>";
                    xmlstring = cf.cleanExcelXML("Sample Size");
                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }
                    xmltext += " <c" +
                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                    " s = \"23\" " +
                    " t = \"s\">" +
                     "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                 "</c>";
                    excelcolumnindex += 1;
                }

                leftheader += "</thead></table>";
                rightheader += "</tr></thead></table>";

                tbltext += "</thead>";
                tbltext += "<tbody>";
                xmltext += "</row>";

                leftbody = "<table class=\"totalbody leftbody\"><body>";
                righttbody = "<table class=\"totalbody rightbody\"><body>";

                benchmark_comp_class = string.Empty;
                //end header
                sWidth = "300px";
                if (complist.Count == 1)
                    sWidth = "100%";
                else if (complist.Count == 2)
                    sWidth = "48.5%";
                else
                    sWidth = "300px";
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
                            sampleSize = new Dictionary<string, string>();
                            rownumber += 1;
                            tbltext += "<tr><td style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;width:394.8px;\"> " + Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString()) + " (" + Get_ShortNames(_measure.Split('|')[0]) + ")" + " </td>";
                            leftbody += "<tr id=\"" + tbl + "\" class=\"crossRetailertotalheader\"><td style=\"text-align:left;padding-top: 2px;padding-bottom: 2px;height: auto;cursor:pointer;position:relative;border-top: 1px solid skyblue;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;width:394.8px;padding-left:40px;\"> " + Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString()) + " (" + Get_ShortNames(_measure.Split('|')[0]) + ")" + " <a class=\"table-title-bottom-line\"></a><div class=\"treeview minusIcon\"></div></td>";
                            righttbody += "<tr id=\"" + tbl + "\" class=\"crossRetailertotalheader\">";
                            foreach (string Comparison in complist)
                            {
                                if (complist.IndexOf(Comparison) == 0)
                                {
                                    righttbody += "<td class=\"" + CleanClass(Comparison + "cell") + "\" style=\"text-align:left;cursor:pointer;padding-top: 2px;padding-bottom: 2px;height: auto;border-top: 1px solid skyblue;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;height:25px;min-width:"+ sWidth +"; \"></td>";
                                }
                                else
                                {
                                    righttbody += "<td class=\"" + CleanClass(Comparison + "cell") + "\" style=\"text-align:left;cursor:pointer;padding-top: 2px;padding-bottom: 2px;height: auto;border-top: 1px solid skyblue;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;height:25px;min-width:" + sWidth + "; \"></td>";
                                }
                                tbltext += "<td class=\"" + CleanClass(Comparison + "cell") + "\" style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"></td>";

                            }
                            leftbody += "</tr>";
                            righttbody += "</tr>";

                            tbltext += "</tr>";

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                            xmlstring = cf.cleanExcelXML(tablename);

                           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                            {
                                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                            }


                            //write table name
                            List<string> tblcolumns = new List<string>();
                            foreach (object col in ds.Tables[tbl].Columns)
                            {
                                string coln = Convert.ToString(col);
                                tblcolumns.Add(coln.Trim().ToLower().ToString());
                            }



                            xmltext += "<row " +
                      "r = \"" + rownumber + "\" " +
                   "spans = \"1:11\" " +
                  " ht = \"15\" " +
                   "thickBot = \"1\" " +
                  " x14ac:dyDescent = \"0.3\">" +
                   "<c " +
                       "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                       "s = \"" + (xmlstring == "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)" ? 16 : 22) + "\" " +
                       "t = \"s\">" +
                       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                   "</c>";

                            for (int i = 0; i < complist.Count * 2; i++)
                            {
                                excelcolumnindex += 1;
                                xmltext += "<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (xmlstring == "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)" ? 16 : 22) + "\"/>";
                            }


                            xmltext += "</row>";
                            for (int rows = 0; rows < ds.Tables[tbl].Rows.Count; rows++)
                            {
                                excelcolumnindex = 0;
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows][1].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext += "<tr>";
                                    leftbody += "<tr>";
                                    righttbody += "<tr>";



                                    leftbody += "<td style=\"overflow: hidden;text-align: left;width:394.8px;padding-left:50px;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows][1])) + "</td>";
                                    tbltext += "<td style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows][1])) + "</td>";

                                    string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows][1]));
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

                                    xmltext += "<row " +
                                   "r = \"" + rownumber + "\" " +
                                   "spans = \"1:11\" " +
                                   "ht = \"15\" " +
                                   "thickBot = \"1\" " +
                                   "x14ac:dyDescent = \"0.3\">" +
                                   "<c " +
                                       "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                       "s = \"" + (average == "Average" ? "16" : "2") + "\" " +
                                       "t = \"s\">" +
                                       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                   "</c> ";
                                    //cellfontstyle = 8;

                                    foreach (string Comparison in complist)
                                    {
                                        BenchmarkOrComparison = Comparison;
                                        excelcolumnindex += 1;
                                        if (complist.IndexOf(Comparison) == 0)
                                        {
                                            benchmark_comp_class = "benchmarkcell";
                                        }
                                        else
                                        {
                                            benchmark_comp_class = CleanClass(Comparison + "cell");
                                        }

                                        if (!string.IsNullOrEmpty(Comparison))
                                        {
                                            if (tblcolumns.Contains(_commonfunctions.Get_TableMappingNamesTotal(Comparison).Trim().ToLower()))
                                            {
                                                if (CheckSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"])))
                                                {

                                                    if (complist.IndexOf(Comparison) == 0)
                                                    {
                                                        righttbody += "<td style=\"min-width:" + sWidth + "\"><ul style=\"width:100%;\"><li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;border:none;width:48%;float:left;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + "</li>";
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;border:none;border-left:1px solid #E6E6E6;width:48%;float:right;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CommonFunctions.CheckdecimalValue(ShowSampleSizeValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"])))) + "</li></ul></td>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<td style=\"min-width:" + sWidth + "\"><ul style=\"width:100%;\"><li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;border:none;width:48%;float:left;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + "</li>";
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;border:none;border-left:1px solid #E6E6E6;width:48%;float:right;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CommonFunctions.CheckdecimalValue(ShowSampleSizeValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"])))) + "</li></ul></td>";
                                                    }
                                                    tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + "</li>";
                                                    xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                        " s = \"" + cellfontstyle.ToString() + "\">" +
                                                      "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)])) + "</v>" +
                                                         "</c> ";
                                                    excelcolumnindex += 1;

                                                    xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                       " s = \"15\">" +
                                                        "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]), out IsApplicable) + "</v>" +

                                                     "</c> ";

                                                }
                                                else if (CheckMediumSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"])))
                                                {

                                                    if (complist.IndexOf(Comparison) == 0)
                                                    {
                                                        righttbody += "<td style=\"min-width:" + sWidth + "\"><ul style=\"width:100%;\"><li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:left;border:none;color: #878787;" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + "</li>";
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:right;border:none;border-left:1px solid #E6E6E6;color: #878787;\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : ShowSampleSizeValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]))) + "</li></ul></td>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<td style=\"min-width:" + sWidth + "\"><ul style=\"width:100%;\"><li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:left;border:none;color: #878787;" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + "</li>";
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:right;border:none;border-left:1px solid #E6E6E6;color: #878787;\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : ShowSampleSizeValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]))) + "</li></ul></td>";
                                                    }
                                                    tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:right;" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][2]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]))) + "</td>";
                                                    if (cellfontstylegrey == 30)
                                                    {
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                        " s = \"26\">" +
                                                          "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)])) + "</v>" +
                                                             "</c> ";
                                                        excelcolumnindex += 1;
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                         " s = \"14\">" +
                                                          "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]), out IsApplicable) + "</v>" +

                                                       "</c> ";
                                                    }
                                                    else
                                                    {
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                        " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                          "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)])) + "</v>" +
                                                             "</c> ";
                                                        excelcolumnindex += 1;
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                         " s = \"14\">" +
                                                          "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]), out IsApplicable) + "</v>" +

                                                       "</c> ";
                                                    }
                                                }

                                                else
                                                {
                                                    if (complist.IndexOf(Comparison) == 0)
                                                    {
                                                        righttbody += "<td style=\"min-width:" + sWidth + "\"><ul style=\"width:100%;\"><li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:left;border:none;" + (average == "Average" ? "background-color:#FCFA7A;" : "") + " color: #878787;\"></li>";
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:right;border:none;border-left:1px solid #E6E6E6;" + (average == "Average" ? "background-color:#FCFA7A;" : "") + " color: black;\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : ShowSampleSizeValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]))) + "</li></ul></td>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<td style=\"min-width:" + sWidth + "\"><ul style=\"width:100%;\"><li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:left;border:none;" + (average == "Average" ? "background-color:#FCFA7A;" : "") + " color: #878787;\"></li>";
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;width:48%;float:right;border:none;border-left:1px solid #E6E6E6;" + (average == "Average" ? "background-color:#FCFA7A;" : "") + " color: black;\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison)]), tablename) : ShowSampleSizeValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]))) + "</li></ul></td>";
                                                    }
                                                    tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "") + " color: #878787;\"></td>";

                                                    if (!sharedStrings.ContainsKey(GlobalVariables.NA))
                                                    {
                                                        sharedStrings.Add(GlobalVariables.NA, sharedStrings.Count());
                                                    }
                                                    xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"31\"" + 
                                                                " t=\"s\">" +
                                                            "<v>" + sharedStrings[GlobalVariables.NA] + "</v>" +
                                                            "</c> ";
                                                    excelcolumnindex += 1;
                                                    xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                          " s = \"15\">" +
                                                           "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNamesTotal(Comparison) + "SampleSize"]), out IsApplicable) + "</v>" +

                                                        "</c> ";
                                                }
                                            }
                                            else
                                            {
                                                if (complist.IndexOf(Comparison) == 0)
                                                {
                                                    righttbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;padding: 1px;text-align: center; color: #878787;min-width:"+ sWidth +"\">NA</td>";
                                                }
                                                else
                                                {
                                                    righttbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;padding: 1px;text-align: center; color: #878787;min-width:" + sWidth + "\">NA</td>";
                                                }
                                                tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;padding: 1px;text-align: center; color: #878787;\"></td>";
                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            " s = \"" + cellfontstyle + "\">" +
                                                                     "<v>NA</v>" +
                                                                        "</c> ";
                                            }
                                        }

                                    }
                                    xmltext += "</row>";
                                    tbltext += "</tr>";
                                    leftbody += "</tr>";
                                    righttbody += "</tr>";
                                }

                            }

                        }

                    }
                }
                else
                {
                    tbltext += "<tr><td style=\"text-align:center\">No data available</td></tr>";
                    int nodatarow = 9;

                    string metricitem = "No data available";
                    xmlstring = cf.cleanExcelXML(metricitem);

                   if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    {
                        sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    }
                    xmltext += "<row " +
                                    "r = \"" + nodatarow.ToString() + "\" " +
                                    "spans = \"1:11\" " +
                                    "ht = \"15\" " +
                                    "thickBot = \"1\" " +
                                    "x14ac:dyDescent = \"0.3\">" +
                                    "<c " +
                                        "r = \"C" + nodatarow.ToString() + "\" " +
                                        "t = \"s\">" +
                                        "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                    "</c></row> ";
                }

                tbltext += "</tbody>";
                leftbody += "</tbody></table>";


                xmltext += "</sheetData>";

                if (mergeCell.Count > 0)
                {
                    string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                    foreach (string mergrrow in mergeCell)
                    {
                        mergetext += mergrrow;

                    }
                    mergetext += "</mergeCells>";
                    xmltext += mergetext;
                }

                xmltext += GetPageMargins();
                xmltext = GetSheetHeadandColumns() + xmltext;
                //Nagaraju 27-03-2014
                xmlstring = xmltext;
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                //exportfiles = new Dictionary<string, string>();
                //exportfiles.Add("tab1", xmltext);
                //HttpContext.Current.Session["exportfiles"] = exportfiles;
                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }

        public void ReadSessionData()
        {

            param.BenchMark = "Channels|A convenience store or gas station food mart";
            param.Compare1 = "Retailers|Dollar General";
            param.Compare2 = string.Empty;
            param.Compare3 = string.Empty;
            param.Compare4 = string.Empty;
            param.Compare5 = string.Empty;
            param.ShopperSegment = string.Empty;
            param.ShopperFrequency = "Weekly +";
            param.CustomFilters = string.Empty;

            BenchMark = "Channels|A convenience store or gas station food mart";
            Compare1 = "Retailers|Dollar General";
            Compare2 = string.Empty;
            Compare3 = string.Empty;
            Compare4 = string.Empty;
            Compare5 = string.Empty;
            ShopperSegment = string.Empty;
            ShopperFrequency = "Weekly +";

            string comparison = param.BenchMark.Split('|')[1];
            complist = new List<string>();

            if (!string.IsNullOrEmpty(param.Compare1))
            {
                comparison = Get_ShortNames(param.Compare1.Split('|')[1]);
                complist.Add(comparison);
            }
            if (!string.IsNullOrEmpty(param.Compare2))
            {
                comparison = Get_ShortNames(param.Compare2.Split('|')[1]);
                complist.Add(comparison);
            }
            if (!string.IsNullOrEmpty(param.Compare3))
            {
                comparison = Get_ShortNames(param.Compare3.Split('|')[1]);
                complist.Add(comparison);
            }
            if (!string.IsNullOrEmpty(param.Compare4))
            {
                comparison = Get_ShortNames(param.Compare4.Split('|')[1]);
                complist.Add(comparison);
            }
            if (!string.IsNullOrEmpty(param.Compare5))
            {
                comparison = Get_ShortNames(param.Compare5.Split('|')[1]);
                complist.Add(comparison);
            }
        }
        public string WriteFilters()
        {
            string value = "";
            switch (accuratestatvalueposi.ToString())
            {
                case "1.2816": value = "80";
                    break;
                case "1.6449": value = "90";
                    break;

                case "1.96": value = "95";
                    break;

                case "2.5758": value = "99";
                    break;
            }
            string xmlstring = "* Selection";
            string xmltext = string.Empty;
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
           "r = \"1\" " +
           "spans = \"1:11\" " +
           "ht = \"15\" " +
           "thickBot = \"1\" " +
           "x14ac:dyDescent = \"0.3\">" +
           "<c " +
               "r = \"B1\" " +
               "s = \"6\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmlstring = "* Filters";
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"C1\" " +
               "s = \"6\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";


            xmlstring = "Stat Test:";
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"D1\" " +
               "s = \"7\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmltext += "</row>";

            //Time Period
            if (!string.IsNullOrEmpty(TimePeriod))
            {
                if (TimePeriod.IndexOf("3MMT") > -1)
                {
                    xmlstring = "Time Period : " + TimePeriod.Split('|')[1] + " 3MMT";
                }
                else if (TimePeriod.IndexOf("total") > -1)
                {
                    xmlstring = "Time Period : " + shortTimePeriod;
                }
                else
                {

                    xmlstring = "Time Period : " + TimePeriod.Split('|')[1];
                }
            }
            else
            {
                xmlstring = "Time Period :";
            }

            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<row " +
   "r = \"2\" " +
   "spans = \"1:11\" " +
   "ht = \"15\" " +
   "thickBot = \"1\" " +
   "x14ac:dyDescent = \"0.3\">" +
   "<c " +
       "r = \"B2\" " +
       "s = \"8\" " +
       "t = \"s\">" +
       "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
   "</c>";

            if (BenchMark.IndexOf("Category") > -1 || BenchMark.IndexOf("Brand") > -1 || CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1)
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
                    xmlstring = "Monthly Purchasing Amount : " + frequency;
                }
            }
            else
            {
                //xmlstring = "Shopping Frequency: " + frequency;
                xmlstring = "Shopping Frequency: " + Get_ShortNamesFrequency(frequency);

            }
            xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
           "r = \"C2\" " +
           "s = \"8\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ";

            xmlstring = ">" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"D2\" " +
               "s = \"9\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmltext += "</row>";

            //Single Selection
            if (param.ShopperSegment != "")
            {
                if (CheckString.IndexOf("Channel") > -1 || CheckString.IndexOf("Retailer") > -1)
                {
                    xmlstring = "Channel/Retailer : " + param.ShopperSegment;
                }
                else if (CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1)
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

            xmltext += "<row " +
     "r = \"3\" " +
     "spans = \"1:11\" " +
     "ht = \"15\" " +
     "thickBot = \"1\" " +
     "x14ac:dyDescent = \"0.3\">" +
     "<c " +
         "r = \"B3\" " +
         "s = \"10\" " +
         "t = \"s\">" +
         "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
     "</c>";


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
            xmltext += "<c " +
           "r = \"C3\" " +
           "s = \"10\" " +
           "t = \"s\">" +
           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
       "</c> ";

            xmlstring = "<" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
           if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            {
                sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            }

            xmltext += "<c " +
               "r = \"D3\" " +
               "s = \"11\" " +
               "t = \"s\">" +
               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
           "</c> ";

            xmltext += "</row>";
            //Single Selection

            //       xmlstring = "Single Selection";
            //       xmlstring = cf.cleanExcelXML(xmlstring);
            //      if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
            //       {
            //           sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
            //       }

            //       xmltext += "<row " +
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

            return xmltext;

        }
        private string GetTableHeader(int comparisons)
        {
            string xmltext = "<row" +
                   " r = \"5\" " +
                    "spans = \"1:11\" " +
                    "ht = \"16.5\" " +
                    "thickTop = \"1\" " +
                    "thickBot = \"1\" " +
                    "x14ac:dyDescent = \"0.3\">" +
                    "<c r = \"A5\" s = \"4\"/>" +
                    "<c " +
                        "r = \"B5\" " +
                        "s = \"16\" " +
                        "t = \"s\">" +
                        "<v>9</v>" +
                    "</c>" +
                    "<c " +
                        "r = \"D5\" " +
                        "s = \"24\" " +
                        "t = \"s\">" +
                        "<v>10</v>" +
                    "</c>";

            xmltext += "</row> ";
            return xmltext;
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
            //string slRetVal = "";
            //try
            //{
            //    if (HeaderTabs.ContainsKey(spVal))
            //        slRetVal = HeaderTabs[spVal];
            //    else
            //        slRetVal = spVal;
            //}
            //catch
            //{
            //    slRetVal = "";
            //}

            return _commonfunctions.Get_ShortNames(spVal);
        }

        private bool CheckSampleSize(string samplesizekey)
        {
            try
            {
                if (samplesizekey != "")
                {
                    double szvalue = Convert.ToDouble(samplesizekey);
                    if (szvalue >= 100)
                    {
                        return true;
                    }
                    else
                    {
                        //cellfontstyle = 10;
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
        //atul new
        private bool CheckMediumSampleSize(string samplesizekey)
        {
            try
            {
                if (samplesizekey != "")
                {

                    double szvalue = Convert.ToDouble(samplesizekey);
                    if (szvalue >= 30 && szvalue < 100)
                    {
                        return true;
                    }

                    else
                    {
                        //cellfontstyle = 10;
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

        private string CheckXMLBlank(string rowvalue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(rowvalue))
            {
                value = "";
            }
            return value;
        }

        private string CheckXMLBlankValues(string rowvalue)
        {
            string value = string.Empty;
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
            return value;
        }

        public string ShowSampleSizeValue(string rowvalue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(rowvalue))
            {
                value = "0";
            }
            else if (Convert.ToDouble(rowvalue) == 0.0)
            {
                value = "0";
            }
            else
            {
                value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
            }
            return value;
        }

        private string CheckBlankValues(string rowvalue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(rowvalue))
            {
                value = GlobalVariables.NA;
            }
            //else if (Convert.ToDouble(rowvalue) == 0.0)
            //{
            //    value = GlobalVariables.NA;
            //}
            else
            {
                //value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + "%";
                value = CommonFunctions.GetRoundingValue(rowvalue) + "%";
            }
            return value;
        }

        private string CheckBlankValues(string rowvalue, string tablename)
        {
            string value = string.Empty;
            if (tablename == "Items Purchased")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                else if (Convert.ToDouble(rowvalue) == 0.0)
                {
                    value = "";
                }
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
                else if (Convert.ToDouble(rowvalue) == 0.0)
                {
                    value = "";
                }
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
            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (BenchmarkOrComparison.Equals(SelectedBenchmark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstyle = 25;
                    }
                    else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    {
                        color = "color:#20B250";

                        cellfontstyle = 4;
                    }
                    else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    {
                        color = "color:red";
                        cellfontstyle = 3;
                    }
                    else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                    {
                        color = "color:black";
                        cellfontstyle = 5;
                    }

                }
                else {
                    if (BenchmarkOrComparison.Equals(SelectedBenchmark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstyle = 25;
                    }
                    else
                    {
                        color = "color:black";
                        cellfontstyle = 5;
                    }
                }
            }
             else
            {
                if (BenchmarkOrComparison.Equals(SelectedBenchmark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 25;
                }
                else
                {
                    color = "color:black";
                    cellfontstyle = 5;
                }
            }
            return color;
        }

        private string GetCellColorGrey(string currentrow, string significancerow, string significancevalue)
        {
            string color = string.Empty;
            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (BenchmarkOrComparison.Equals(SelectedBenchmark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstylegrey = 30;
                    }
                    else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                    {
                        color = "color:black";
                        cellfontstylegrey = 13;
                    }
                    else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    {
                        color = "color:red";
                        cellfontstylegrey = 21;
                    }
                    else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    {
                        color = "color:#20B250";
                        cellfontstylegrey = 20;
                    }
                }
            }
            else
            {
                if (BenchmarkOrComparison.Equals(SelectedBenchmark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstylegrey = 30;
                }
                else
                {
                    color = "color:black";
                    cellfontstylegrey = 13;
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

            if (tablename == "Items Purchased")
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                else if (Convert.ToDouble(rowvalue) == 0.0)
                {
                    value = "";
                }
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
                else if (Convert.ToDouble(rowvalue) == 0.0)
                {
                    value = "";
                }
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
                else if (Convert.ToDouble(rowvalue) == 0.0)
                {
                    value = "";
                }
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
            if (colmaxwidth == 0)
            {
                colmaxwidth = 30;
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
        "<cols> " +
            "<col " +
                "min = \"1\" " +
                "max = \"1\" " +
                "width = \"" + colmaxwidth.ToString() + "\" " +
                "customWidth = \"1\"/> " +
            "<col " +
                "min = \"2\" " +
               "max = \"2\" " +
                "width = \"35.28515625\" " +
                "customWidth = \"1\"/> " +
            "<col " +
                "min = \"3\" " +
                "max = \"3\" " +
                "width = \"36.85546875\" " +
                "customWidth = \"1\"/> " +
           "<col " +
                "min = \"4\" " +
                "max = \"4\" " +
                "width = \"19.85546875\" " +
                "customWidth = \"1\"/> " +
           "<col " +
                "min = \"5\" " +
                "max = \"5\" " +
                "width = \"21\" " +
                "customWidth = \"1\"/> " +
            "<col " +
                "min = \"6\" " +
                "max = \"6\" " +
                "width = \"22\" " +
                "customWidth = \"1\"/> " +
           "<col " +
                "min = \"7\" " +
                "max = \"7\" " +
                "width = \"23\" " +
                "customWidth = \"1\"/> " +
        "</cols>";
            return sheetstr;
        }

        public void Shortnames()
        {
            FilterTabs.Clear();
            FilterTabs.Add("MainStore", "Main Store (in channel)");
            FilterTabs.Add("MainStoreOverAll", "Main Store (across channel)");
            FilterTabs.Add("FavoriteStore", "Favorite Store (in channel)");
            FilterTabs.Add("FavoriteStoreOverAll", "Favorite Store (across channel)");
            FilterTabs.Add("Past 3 Month", "Quarterly +");           
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