using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using iSHOPNew.DAL;

namespace iSHOPNew.Models
{

    
       public class ContingencyTable
        {
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
            int cellfontstyle = 34;
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
            bool IsApplicable = true;
            string leftheader = string.Empty;
            string leftbody = string.Empty;
            string rightheader = string.Empty;
            string righttbody = string.Empty;
            public ContingencyTable()
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
                leftheader = "<table><thead>";
                rightheader = "<thead>";
                string table = string.Empty;
                table += "<tr>";
                table += "<td style=\"min-height: 25px;width:386px;\" class=\"ShoppingFrequencytitle\"></td>";
                table += "<td class=\"benchmarktitle\" style=\"\">BENCHMARK</td>";
                table += "<td class=\"comparisonheader\" style=\"\">COMPARISON POINTS</td>";

                //leftheader += "<tr>";
                //leftheader += "<td style=\"height: 42px;width:386px;\" class=\"ShoppingFrequencytitle\">SHOPPING FREQUENCY <a class=\"table-top-title-bottom-line\" style=\"background-color: #000000;border-radius: 1px 10px 0 0; bottom: 46px;display: block;height: 4px;left: 2px;width: 25px;position: absolute;\"></a></td>";
                //leftheader += "<td style=\"height: 42px;\" class=\"Benchmarktitle\">BENCHMARK</td>";
                //leftheader += "</tr>";

                if (complist != null && complist.Count > 0)
                   // rightheader += "<tr><td style=\"height: 42px;\" class=\"" + CleanClass(Convert.ToString(complist[1])) + "header\" style=\"\">COMPARISON POINTS</td>";

                if (complist != null && complist.Count > 0)
                {
                    //for (int i = 2; i < complist.Count; i++)
                    //{
                    //    table += "<td class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"></td>";
                    //    rightheader += "<td style=\"height: 42px;\" class=\"" + CleanClass(Convert.ToString(complist[i])) + "header\"></td>";
                    //}
                }
                rightheader += "</tr>";
                table += "</tr>";
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

            public iSHOPParams BindTabs(out string tbltext, out string xmlstring, DataSet inputds, string timePeriod, string _ShopperSegment, string filterShortname, string _ShopperFrequency, List<string> ShortNames, string StatPositive, string StatNegative, string ExportToExcel, string TimePeriodShortName,string ViewType)
            {
                iSHOPParams ishopParams = new iSHOPParams();
                sharedStrings = new Dictionary<string, int>();
                frequency = _ShopperFrequency;
                PopulateShortNames();
                Shortnames();
                string _BenchMark = string.Empty;
                BenchMark = _BenchMark;
                param = new iSHOPParams();
                param.BenchMark = _BenchMark;
                complist = new List<string>();
                var query = from r in ShortNames select r;
                complist = query.ToList();

                param.ShopperSegment = _ShopperSegment;
                TimePeriod = TimePeriodShortName;
                param.ShopperFrequency = _ShopperFrequency;
                param.CustomFilters = filterShortname;
                DataAccess dal = new DataAccess();

                DataSet ds = inputds;

                int excelcolumnindex = 1;
                int rownumber = 6;

                accuratestatvalueposi = Convert.ToDouble(StatPositive);
                accuratestatvaluenega = Convert.ToDouble(StatNegative);
                //Nagaraju 27-03-2014

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
                    xmltext += AddSampleSizeNote();
                    xmltext += GetTableHeader(complist.Count,Convert.ToString(ViewType));

                    if (complist.Count > 1)
                    {
                        mergeCell.Add("<mergeCell ref = \"B5:" + ColumnIndexToName(complist.Count) + "5\"/>");
                    }
                    if (!sharedStrings.ContainsKey("BENCHMARK"))
                    {
                        sharedStrings.Add("BENCHMARK", sharedStrings.Count());
                    }

                    if (!sharedStrings.ContainsKey("COMPARISON POINTS"))
                    {
                        sharedStrings.Add("COMPARISON POINTS", sharedStrings.Count());
                    }

                    //write second header
                    excelcolumnindex = 0;
                    xmltext += " <row" +
                   " r = \"" + rownumber + "\" " +
                    "spans = \"1:11\" " +
                    "x14ac:dyDescent = \"0.25\">" +
                   " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"5\"/>";
                    excelcolumnindex += 1;

                    tbltext = "<thead>";
                    tbltext += CreateFirstTableHeader();
                    tbltext += "<tr style=\"display:flex;\"><td class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;min-height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;;display: flex;\"></td>";
                    leftheader += "<tr style=\"display:flex;\"><td class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;;display: flex;\">" + Get_ShortNamesFrequency(frequency) + "<a class=\"table-top-title-bottom-line\" style=\"background-color: #000000;border-radius: 1px 10px 0 0;bottom: 0px;display: block;height: 4px;left: 0px;position: absolute;width: 25px;\"></a></td>";
                    rightheader += "<tr style=\"display:flex;\">";
                    //create header
                    string colNames;

                    //write comparison
                    string benchmark_comp_class = string.Empty;
                    var kCount = 1;
                    foreach (string Comparison in complist)
                    {
                        colNames = Get_ShortNames(Comparison) + AddTradeAreaNoteforChannel(Get_ShortNames(Comparison));
                        if (complist.IndexOf(Comparison) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            leftheader += "<td class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;margin-right:0;display: flex;\"><span title=\"" + colNames + "\">" + colNames + "</span></td>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(Comparison + "header");
                            if (kCount >= (complist.Count))
                                rightheader += "<td class=\"" + CleanClass(benchmark_comp_class) + " ColumnWidthHeader\" style=\"overflow: hidden;text-align: center;min-height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;margin-right: 0px;display: flex;\"><span title=\"" + colNames + "\">" + colNames + "</span></td>";
                            else
                            rightheader += "<td class=\"" + CleanClass(benchmark_comp_class) + " ColumnWidthHeader\" style=\"overflow: hidden;text-align: center;min-height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;;display: flex;\"><span title=\"" + colNames + "\">" + colNames + "</span></td>";

                        }

                        tbltext += "<td class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></td>";
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
                        excelcolumnindex += 1;
                        kCount++;
                    }
                    tbltext += "</tr>";
                    leftheader += "</tr>";
                    rightheader += "</tr>";
                    tbltext += "<tr>";
                    xmltext += "</row>";

                    //add check sample size
                    //List<iSHOPParams> iSHOPParamlist = new List<iSHOPParams> ();
                    //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToString(ds.Tables[0].Rows[0]["MetricItem"]).Equals("SampleSize",StringComparison.OrdinalIgnoreCase))
                    //{
                    //    iSHOPParams iparam = null;
                    //    foreach (string Comparison in complist)
                    //    {
                    //        iparam = new iSHOPParams();
                    //        sampleSize.Add(Comparison, Convert.ToString(ds.Tables[0].Rows[0][Comparison]));
                    //        iparam.SampleSize = CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[0].Rows[0][Comparison]));
                    //        iSHOPParamlist.Add(iparam);
                    //    }                   
                    //}             

                    //SampleSize checksampleSize = new SampleSize();
                    ////iSHOPParamlist = checksampleSize.CheckAccrossRetailerSampleSize(checksamplesizesp, BenchMark, Retailerlist, timePeriod, _ShopperSegment, _ShopperFrequency, ShortNames);
                    //rownumber += 1;
                    //excelcolumnindex = 0;
                    //xmlstring = cf.cleanExcelXML("Sample Size");

                    //if (!sharedStrings.ContainsKey(xmlstring))
                    //{
                    //    sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    //}

                    //xmltext += "<row " +
                    //                     "r = \"" + rownumber + "\" " +
                    //                     "spans = \"1:11\" " +
                    //                     "ht = \"15\" " +
                    //                     "thickBot = \"1\" " +
                    //                     "x14ac:dyDescent = \"0.3\">" +
                    //                     " <c" +
                    //                     " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                    //                     " s = \"32\" " +
                    //                     " t = \"s\">" +
                    //                    "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                    //                    "</c>";
                    //if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                    //{
                    //    tbltext += "<td style=\"background-color: #ededee;color:#000000;border-color: #D3D3D3;border-left-style: solid;\"><span>Sample Size</span></td>";
                    //    leftheader += "<tr><td style=\"background-color: #ededee;color:#000000;border-color: #D3D3D3;border-left-style: solid;\"><span>Sample Size</span></td>";
                    //    leftheader += "</tr>";
                    //    rightheader += "<tr>";
                    //    foreach (iSHOPParams para in iSHOPParamlist)
                    //    {
                    //        excelcolumnindex += 1;
                    //        if (iSHOPParamlist.IndexOf(para) == 0)
                    //        {
                    //            benchmark_comp_class = "benchmarkheader";
                    //            rightheader += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></td>";
                    //        }
                    //        else
                    //        {
                    //            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                    //            rightheader += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></td>";

                    //        }
                    //        tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + para.SampleSize + "</span></td>";

                    //        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                    //                                                 " s = \"33\">" +
                    //                                            "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(para.SampleSize).Replace(",", "")) + "</v>" +
                    //                                                  "</c> ";
                    //    }
                    //    leftheader += "</thead></table>";
                    //    rightheader += "</tr></thead>";
                    //}
                    //else
                    //{
                    //    tbltext += "<td style=\"background-color: #ededee;color:#000000;border-color: #D3D3D3;border-left-style: solid;\"><span>Sample Size</span></td>";
                    //    leftheader += "<tr><td style=\"background-color: #ededee;color:#000000;border-color: #D3D3D3;border-left-style: solid;\"><span>Sample Size</span></td>";
                    //    leftheader += "</tr>";
                    //    rightheader += "<tr>";
                    //    foreach (string Comparison in complist)
                    //    {
                    //        excelcolumnindex += 1;
                    //        colNames = Get_ShortNames(Comparison.Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                    //        if (complist.IndexOf(Comparison) == 0)
                    //        {
                    //            benchmark_comp_class = "benchmarkheader";
                    //            rightheader += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></td>";
                    //        }
                    //        else
                    //        {
                    //            benchmark_comp_class = CleanClass(Comparison) + "header";
                    //            rightheader += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></td>";
                    //        }

                    //        tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></td>";
                    //        xmlstring = string.Empty;

                    //       if (!sharedStrings.ContainsKey(xmlstring.ToUpper()))
                    //        {
                    //            sharedStrings.Add(xmlstring.ToUpper(), sharedStrings.Count());
                    //        }

                    //        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                    //                                                 " s = \"33\">" +
                    //                                            "<v></v>" +
                    //                                                  "</c> ";
                    //    }
                    //    leftheader += "</thead></table>";
                    //    rightheader += "</tr></thead>";
                    //}
                    //tbltext += "</tr>";
                    //tbltext += "</thead>";
                    //tbltext += "<tbody>";
                    //xmltext += "</row>";

                    leftheader += "</thead></table>";
                    rightheader += "</thead>";

                    leftbody = "<table><body>";
                    righttbody = "<body>";

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
                                sampleSize = new Dictionary<string, string>();
                                rownumber += 1;
                                tbltext += "<tr><td style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"> " + Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString()) + " </td>";
                                leftbody += "<tr style=\"display:flex;cursor:pointer\" class=\"correspondancecompretailerheading\"><td style=\"text-align:left;position:relative;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;min-height: 25px;display flex;align-items:center;vertical-align:middle;;display: flex;\"> " + Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString()) + "<a class=\"table-top-title-bottom-line\" style=\"background-color: #72aaff;border-radius: 1px 10px 0 0;bottom: 0px;display: block;height: 4px;left: 0px;position: absolute;width: 25px;\"></a><div class=\"treeview minusIcon\"></div></td>";
                                righttbody += "<tr style=\"display:flex;cursor:pointer\" class=\"correspondancecompretailerheading\">";
                            var index = 0;
                            //min-height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;;display: flex;
                            foreach (string Comparison in complist)
                                {
                                index++;
                                    if (complist.IndexOf(Comparison) == 0)
                                    {
                                        leftbody += "<td class=\"" + CleanClass(Comparison + "cell") + "\" style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;min-height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;margin-right:0;display: flex;\"></td>";
                                    }
                                    else
                                    {
                                    var margin = "";
                                        margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                        righttbody += "<td class=\"" + CleanClass(Comparison + "cell") + " ColumnWidth\" style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;min-height: 25px;display flex;align-items:center;justify-content:center;vertical-align:middle;display: flex;" + margin + "\"></td>";
                                    }
                                    tbltext += "<td class=\"" + CleanClass(Comparison + "cell") + "\" style=\"text-align:left;" + (ds.Tables[tbl].Rows[0][0].ToString() == "RetailerLoyaltyPyramid(Base:CouldShop)" ? "background-color: #FCFA7A" : "background-color: #D9E1EE") + ";color:#000000;\"></td>";

                                }
                            index = 0;
                                leftbody += "</tr>";
                                righttbody += "</tr style=\"display:flex;\">";

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
                           "s = \"" + (xmlstring == "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)" ? 16 : 16) + "\" " +
                           "t = \"s\">" +
                           "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                       "</c>";

                                for (int i = 0; i < complist.Count; i++)
                                {
                                    excelcolumnindex += 1;
                                    xmltext += "<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (xmlstring == "Retailer Loyalty Pyramid(Base:Could Shop)(Applicable Only For Retailers)" ? 16 : 16) + "\"/>";
                                }


                                xmltext += "</row>";
                                for (int rows = 0; rows < ds.Tables[tbl].Rows.Count; rows++)
                                {
                                    excelcolumnindex = 0;
                                    DataRow dRow = ds.Tables[tbl].Rows[rows];
                                    Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                    if (!Significance.Trim().ToLower().Contains("significance") && !Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Equals("Number Of Responses",StringComparison.OrdinalIgnoreCase))
                                    {
                                        rownumber += 1;
                                        //cellfontstyle = 2;
                                        tbltext += "<tr>";
                                        leftbody += "<tr style=\"display:flex\">";
                                        righttbody += "<tr style=\"display:flex;\">";
                                        if (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average Number of Items" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average time in store" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Amount Spent" ||
                                     Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "AVERAGE ONLINE ORDER SIZE" || 
                                     Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "AVERAGE ONLINE BASKET SIZE")
                                    {
                                            average = "Average";
                                        }
                                        else
                                        {
                                            average = "";
                                        }
                                        //write sample size
                                        if (ds.Tables[tbl].Rows[rows]["MetricItem"].ToString() == "Number of Trips" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString() == "SampleSize" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().Contains("SampleSize"))
                                        {
                                            //sampleSize = new Dictionary<string, string>();
                                            tbltext += "<td style=\"overflow: hidden;text-align: left;background-color:  #ededee; color: #878787;font-weight: bold;color: black;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</td>";

                                            leftbody += "<td style=\"overflow: hidden;text-align: left;background-color:  #ededee; color: #878787;font-weight: bold;color: black;min-height: 25px;  align-items: center; display: flex;;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</td>";

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

                                            xmltext += "<row " +
                                           "r = \"" + rownumber + "\" " +
                                           "spans = \"1:11\" " +
                                           "ht = \"15\" " +
                                           "thickBot = \"1\" " +
                                           "x14ac:dyDescent = \"0.3\">" +
                                           "<c " +
                                               "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                               "s = \"32\" " +
                                               "t = \"s\">" +
                                               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                           "</c> ";

                                        //plot sample size
                                        index = 0;
                                            foreach (string Comparison in complist)
                                            {
                                                excelcolumnindex += 1;
                                                index++;
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
                                                    if (tblcolumns.Contains(Comparison.Trim().ToLower()))
                                                    {
                                                        if (complist.IndexOf(Comparison) == 0)
                                                        {
                                                            leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color:  #ededee; color: #878787;font-weight: bold;min-height: 25px;  align-items: center; display: flex;justify-content: center;\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]),out samplecellstyle) + "</td>";

                                                        }
                                                        else
                                                        {
                                                        var margin = "";
                                                        margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                                        righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;background-color:  #ededee; color: #878787;font-weight: bold;color: black;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + "\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), out samplecellstyle) + "</td>";

                                                        }
                                                        tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color:  #ededee; color: #878787;font-weight: bold;\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]),out samplecellstyle) + "</td>";
                                                        if (Comparison.Equals("Average", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            sampleSize.Add(Comparison, "101");
                                                        }
                                                        else
                                                        {
                                                        string noOfRes = (from r in ds.Tables[tbl].AsEnumerable()
                                                                          where Convert.ToString(r["MetricItem"]).Equals("Number Of Responses", StringComparison.OrdinalIgnoreCase)
                                                                          select Convert.ToString(r[Comparison])).FirstOrDefault();
                                                        if(!string.IsNullOrEmpty(noOfRes))
                                                            sampleSize.Add(Comparison, noOfRes);
                                                        else
                                                        sampleSize.Add(Comparison, Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]));

                                                        }
                                                        string _lowsamplesize = Convert.ToString(CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])));
                                                        if (samplecellstyle == 46 || !string.IsNullOrEmpty(_lowsamplesize))
                                                        {
                                                            string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])));
                                                            if (!sharedStrings.ContainsKey(lowsamplesize))
                                                            {
                                                                sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                            }
                                                            xmltext += " <c" +
                                                                        " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"33\" " +
                                                                        " t = \"s\">" +
                                                                        "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                        "</c>";
                                                        }
                                                        else if (samplecellstyle == 30 || !string.IsNullOrEmpty(_lowsamplesize))
                                                        {
                                                            string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])));
                                                            if (!sharedStrings.ContainsKey(lowsamplesize))
                                                            {
                                                                sharedStrings.Add(lowsamplesize, sharedStrings.Count());
                                                            }
                                                            xmltext += " <c" +
                                                                        " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"33\" " +
                                                                        " t = \"s\">" +
                                                                        "<v>" + sharedStrings[lowsamplesize] + "</v>" +
                                                                        "</c>";
                                                        }
                                                        else
                                                        {

                                                            xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                       " s = \"33\">" +
                                                                  "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + "</v>" +
                                                                        "</c> ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (complist.IndexOf(Comparison) == 0)
                                                        {
                                                            leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;min-height: 25px;  align-items: center; display: flex;justify-content: center;\"></td>";

                                                        }
                                                        else
                                                        {
                                                        var margin = "";
                                                        margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                                        righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + "\"></td>";

                                                        }
                                                        tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"></td>";
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                        " s = \"33\">" +
                                                          "<v></v>" +
                                                             "</c> ";
                                                    }
                                                }
                                            }
                                        index = 0;
                                        tbltext += "</tr>";
                                            leftbody += "</tr>";
                                            righttbody += "</tr>";
                                            xmltext += "</row>";
                                            //End Sample Size
                                        }

                                        else
                                        {
                                            leftbody += "<td style=\"overflow: hidden;text-align: left;min-height: 25px;  align-items: center; display: flex;;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</td>";
                                            tbltext += "<td style=\"overflow: hidden;text-align: left;" + (average == "Average" ? "background-color:#FCFA7A" : string.Empty) + "\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) + "</td>";

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

                                            xmltext += "<row " +
                                           "r = \"" + rownumber + "\" " +
                                           "spans = \"1:11\" " +
                                           "ht = \"15\" " +
                                           "thickBot = \"1\" " +
                                           "x14ac:dyDescent = \"0.3\">" +
                                           "<c " +
                                               "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                               "s = \"" + (average == "Average" ? "43" : "31") + "\" " +
                                               "t = \"s\">" +
                                               "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
                                           "</c> ";
                                        //cellfontstyle = 17;
                                        index = 0;
                                            foreach (string Comparison in complist)
                                            {
                                            index++;
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
                                                    if (tblcolumns.Contains(Comparison.Trim().ToLower()))
                                                    {
                                                        if (CheckSampleSize(Comparison))
                                                        {
                                                            if (average == "Average")
                                                            {
                                                                if (complist.IndexOf(Comparison) == 0)
                                                                {
                                                                    leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;min-height: 25px;  align-items: center; display: flex;justify-content: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                else
                                                                {
                                                                var margin = "";
                                                                margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                                                righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + ";" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " t=\"s\"  s = \"44\">" +
                                                                  "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename)] + "</v>" +
                                                                     "</c> ";
                                                            }
                                                            else
                                                            {
                                                                if (complist.IndexOf(Comparison) == 0)
                                                                {
                                                                    leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;min-height: 25px;  align-items: center; display: flex;justify-content: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                else
                                                                {
                                                                var margin = "";
                                                                margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                                                righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + ";" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : string.Empty) + "" + (Comparison == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + "</v>" +
                                                                     "</c> ";
                                                            }

                                                        }
                                                        else if (CommonFunctions.CheckMediumSampleSize(Comparison, sampleSize))
                                                        {
                                                            if (average == "Average")
                                                            {
                                                                if (complist.IndexOf(Comparison) == 0)
                                                                {
                                                                    leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;min-height: 25px;  align-items: center; display: flex;justify-content: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                else
                                                                {
                                                                var margin = "";
                                                                margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                                                righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + ";" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " t=\"s\" s = \"44\">" +
                                                                  "<v>" + sharedStrings[CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename)] + "</v>" +
                                                                     "</c> ";
                                                            }
                                                            else
                                                            {
                                                                if (complist.IndexOf(Comparison) == 0)
                                                                {
                                                                    leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;min-height: 25px;  align-items: center; display: flex;justify-content: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                else
                                                                {
                                                                var margin = "";
                                                                margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                                                righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + ";" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                }
                                                                tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: transparent;") + "" + (Comparison == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][Comparison]))) + " \">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]), tablename) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison]))) + "</td>";
                                                                xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][Comparison])) + "</v>" +
                                                                     "</c> ";
                                                            }

                                                        }

                                                        else
                                                        {
                                                            if (complist.IndexOf(Comparison) == 0)
                                                            {
                                                                leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;min-height: 25px;  align-items: center; display: flex;justify-content: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: #ededee;") + " color: #878787;\"></td>";
                                                            }
                                                            else
                                                            {
                                                            var margin = "";
                                                            margin = (index != (complist.Count)) ? ";" : "margin-right: 0px;";
                                                            righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + ";" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: #ededee;") + " color: #878787;\"></td>";
                                                            }
                                                            tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:#FCFA7A;" : "background-color: #ededee;") + " color: #878787;\"></td>";
                                                            xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"" + (average == "Average" ? "44" : "31") + "\">" +
                                                                    "<v></v>" +
                                                                    "</c> ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (complist.IndexOf(Comparison) == 0)
                                                        {
                                                            leftbody += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color:  #ededee; color: #878787;min-height: 25px;  align-items: center; display: flex;justify-content: center;\"></td>";
                                                        }
                                                        else
                                                        {
                                                        var margin = "";
                                                        margin = (index != (complist.Count + 1)) ? ";" : "margin-right: 0px;";
                                                        righttbody += "<td class=\"" + benchmark_comp_class + " ColumnWidth\" style=\"overflow: hidden;text-align: center;background-color:  #ededee; color: #878787;min-height: 25px; align-items: center; justify-content: center;vertical-align: middle;display: flex;" + margin + "\"></td>";
                                                        }
                                                        tbltext += "<td class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color:  #ededee; color: #878787;\"></td>";
                                                        xmltext += "<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle + "\">" +
                                                                             "<v></v>" +
                                                                                "</c> ";
                                                    }
                                                }

                                            }
                                        index = 0;
                                            xmltext += "</row>";
                                            tbltext += "</tr>";
                                            leftbody += "</tr>";
                                            righttbody += "</tr>";
                                        }
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
                    righttbody += "</tbody>";

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
                    HttpContext.Current.Session["CorrespondenceMapsSharedstrings"] = sharedStrings;
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
                   "s = \"10\" " +
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
                   "s = \"10\" " +
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
                   "s = \"27\" " +
                   "t = \"s\">" +
                   "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
               "</c> ";

                xmltext += "</row>";

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

                xmltext += "<row " +
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
               "s = \"11\" " +
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
                   "s = \"28\" " +
                   "t = \"s\">" +
                   "<v>" + sharedStrings[xmlstring.ToUpper()] + "</v>" +
               "</c> ";

                xmltext += "</row>";

                //Single Selection
                xmlstring = "";
                if (param.ShopperSegment != "")
                {
                    if (param.ShopperSegment.IndexOf("Channel") > -1 || param.ShopperSegment.IndexOf("Retailer") > -1)
                    {
                        if (AddTradeAreaNoteforChannel(Get_ShortNames(param.ShopperSegment.Replace("Channels|", "").Replace("Retailers|", ""))) != string.Empty)
                        {
                            xmlstring = "Channel/Retailer : " + Get_ShortNames(param.ShopperSegment.Replace("Channels|", "").Replace("Retailers|", "")) + AddTradeAreaNoteforChannel(param.ShopperSegment.Replace("Channels|", "").Replace("Retailers|", ""));
                        }
                        else
                        {
                            xmlstring = "Channel/Retailer : " + Get_ShortNames(param.ShopperSegment.Replace("Channels|", "").Replace("Retailers|", ""));
                        }
                    }
                    else if (param.ShopperSegment.IndexOf("Category") > -1 || param.ShopperSegment.IndexOf("Brand") > -1)
                    {
                        xmlstring = "Category/Brand : " + Get_ShortNames(param.ShopperSegment.Replace("Channels|", "").Replace("Retailers|", ""));
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
             "s = \"12\" " +
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
               "s = \"12\" " +
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
                   "s = \"29\" " +
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
                         "s = \"38\" " +
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
                         "s = \"37\" " +
                         "t = \"s\">" +
                        "<v>" + sharedStrings[viewtype] + "</v>" +
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

            private bool CheckSampleSize(string samplesizekey)
            {
                try
                {
                    if (sampleSize.ContainsKey(samplesizekey))
                    {
                        string val = sampleSize[samplesizekey];
                        if (string.IsNullOrEmpty(val))
                            return false;
                        double szvalue = Convert.ToDouble(sampleSize[samplesizekey]);
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

            private string CheckBlankValues(string rowvalue)
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
                    value = CommonFunctions.GetRoundingValue(rowvalue) + "%";
                }
                return value;
            }

            private string CheckBlankValues(string rowvalue, string tablename)
            {
                string value = string.Empty;
                if (tablename == "# of Items Purchased")
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

                return value;
            }

            private string GetCellColor(string currentrow, string significancerow, string significancevalue)
            {
                string color = string.Empty;
                if (significancevalue != "")
                {
                    if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                    {
                        if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                        {
                            color = "color:#20B250";

                            cellfontstyle = 35;
                        }
                        else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                        {
                            color = "color:red";
                            cellfontstyle = 36;
                        }
                        else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                        {
                            color = "color:black";
                            cellfontstyle = 34;
                        }

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
                        if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                        {
                            color = "color:gray";
                            cellfontstylegrey = 39;
                        }
                        else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                        {
                            color = "color:red";
                            cellfontstylegrey = 41;
                        }
                        else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                        {
                            color = "color:#20B250";
                            cellfontstylegrey = 40;
                        }
                    }
                }
                return color;
            }

            private string GetPageMargins()
            {
                string pagem = "<pageMargins  left = \"0.7\"  right = \"0.7\"  top = \"0.75\"  bottom = \"0.75\"  header = \"0.3\"  footer = \"0.3\"/>";
                return pagem;
            }

            private string CheckXMLBlankValues(string rowvalue, string tablename)
            {
                string value = string.Empty;
                string valuereturn = string.Empty;

                if (tablename == "# of Items Purchased")
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
                       "width = \"" + colmaxwidth.ToString() + "\" " +
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