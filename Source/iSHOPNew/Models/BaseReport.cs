using AQ.Common.GenerateReport;
using iSHOP.BLL;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Xml;

namespace iSHOPNew.Models
{
    public abstract class BaseReport : System.Web.UI.Page
    {
        public int slide_Number = 0;
        public int chart_Number = 0;
        public string destpath = string.Empty;
        public string[] destination_FilePath = null;
        public FileDetails files = new FileDetails();
        public List<SlideDetails> slidelist = new List<SlideDetails>();
        public SlideDetails slide = new SlideDetails();
        public ChartDetails chart = new ChartDetails();
        public int slideNo = 0;
        public string UserExportFileName = string.Empty;
        public string Source = string.Empty;
        public string fileName = string.Empty;
        public string sPowerPointTemplatePath = string.Empty;
        public ReportGeneratorParams reportparams = null;
        public  Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public  Random rnd = new Random();
        public object SignificanceValue, PositiveValue, NegativeValue;
        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public string volume = string.Empty;
        public string significance = string.Empty;
        public string SampleSize = String.Empty;
        public List<string> lstHeaderText = new List<string>();
        public CommonFunctions _commonfunctions = new CommonFunctions();
        public List<CalculationColor> lststatcolour = null;
        public int ReportNumber = 0;
        public List<object> objectivecolumnlist = new List<object>();
        public List<string> objectives = new List<string>();
        public DataSet geods = null;
        public List<string> columnwidth = null;
        public string rowheight = string.Empty;
        public double fontsize = 800;
        public List<string> metriclist = null;
        public double StatTesting;
        public TimeSpan timeSpan = new TimeSpan();
        public int hh = 0;
        public int mm = 0;
        public int table_width = 9700000;
        public int top5_table_width = 8950000;
        public List<string> ChannelNets = new List<string>();
        public string chkComparisonFolderNumber = string.Empty;
        public List<object> columnlist = null;
        public string xmlpath = string.Empty;
        public abstract List<FileDetails> BuildSlides();
        public abstract DataSet FilterData(DataSet dtbl, [Optional] string strGapOption);
        public BaseReport()
        {
            StatTesting = Convert.ToDouble(Session["PercentStat"]);
            accuratestatvalueposi = Convert.ToDouble(Session["StatSessionPosi"]);
            accuratestatvaluenega = Convert.ToDouble(Session["StatSessionNega"]);
            PopulateShortNames();
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
            HeaderTabs.Add("EmployeeStatus1", "Employment Status");
            HeaderTabs.Add("EmployeeStatus2", "Employment Status");
            HeaderTabs.Add("Education ", "Education ");
            HeaderTabs.Add("primaryHHShopper", "PRIMARY HH SHOPPER ");

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

            HeaderTabs.Add("Top Box Satisfaction", "TopBoxSatisfaction");
            HeaderTabs.Add("Top Box Willingness to Recommend", "TopBoxWillingnesstoRecommend");
            HeaderTabs.Add("Main Store", "MainStore");
            HeaderTabs.Add("Main Store OverAll", "MainStoreOverAll");
            HeaderTabs.Add("Favorite Store", "FavoriteStore");
            HeaderTabs.Add("Favorite Store OverAll", "FavoriteStoreOverAll");
            HeaderTabs.Add("Top Box Likeability", "TopBoxLikeability");



            HeaderTabs.Add("TopBoxSatisfaction SampleSize", "Sample Size - Top Box Satisfaction");
            HeaderTabs.Add("TopBoxSatisfaction", "Top Box Satisfaction");
            HeaderTabs.Add("TopBoxLikeability SampleSize", "Sample Size - Top Box Likeability");
            HeaderTabs.Add("TopBoxLikeability", "Top Box Likeability");
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
            HeaderTabs.Add("Diet Carbonated Soft Drinks", "Diet SSD");
            HeaderTabs.Add("Regular (non-diet) carbonated soft drinks", "REG SSD");
            HeaderTabs.Add("Bottled water", "Water");
            HeaderTabs.Add("Juice or juice drinks", "Juice");
            HeaderTabs.Add("Iced tea in bottles, cans, or cartons", "Iced Tea");
            HeaderTabs.Add("Coffee in bottles or cans", "Coffee");
            HeaderTabs.Add("Sports drinks", "Sports Drinks");
            HeaderTabs.Add("Energy drinks", "Energy Drinks");
            HeaderTabs.Add("P3M+ CHANNEL SHOPPING FREQUENCY", "Quarterly+ Channel Shopping Frequency");
            HeaderTabs.Add("P3M+ PRIORITY STORE SHOPPING FREQUENCY", "Quarterly+ Priority Store Shopping Frequency");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(REG SSD)", "Do not purchase(REG SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(DIET SSD)", "Do not purchase(DIET SSD)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(WATER)", "Do not purchase(WATER)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(JUICE)", "Do not purchase(JUICE)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(SPORTS DRINKS)", "Do not purchase(SPORTS DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(ENERGY DRINKS)", "Do not purchase(ENERGY DRINKS)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD TEA)", "Do not purchase(RTD TEA)");
            HeaderTabs.Add("I don~t purchase any of these brands at least once a month(RTD COFFEE)", "Do not purchase(RTD COFFEE)");

            HeaderTabs.Add("AcrossShopper", "Total Shopper");
            HeaderTabs.Add("AcrossTrips", "Total Trips");
            HeaderTabs.Add("AcrossBeverageShopper", "Total Beverage Shopper");
            HeaderTabs.Add("AcrossBeverageTrips", "Total Beverage Trips");

            HeaderTabs.Add("Shopper Frequency2", "Shopper Frequency");

            HeaderTabs.Add("BeverageConsumedMonthly", "Beverage Consumed Monthly");
            HeaderTabs.Add("BeveragepurchasedMonthly", "Beverage purchased Monthly");
            HeaderTabs.Add("FAVOURITESPORTDRINKS", "FAVORITE SPORT DRINKS");
            HeaderTabs.Add("FAVOURITEENERGYDRINKS", "FAVORITE ENERGY DRINKS");
            HeaderTabs.Add("FAVOURITEREGSSD", "FAVORITE REG SSD");
            HeaderTabs.Add("FAVOURITEDIETSSD", "FAVORITE DIET SSD");
            HeaderTabs.Add("FAVOURITEUNFLAVOREDBOTTLEDWATER(NON-SPARKLING)", "FAVORITE UNFLAVORED BOTTLED WATER (NON-SPARKLING)");
            HeaderTabs.Add("FAVOURITEFLAVOREDSPARKLINGWATER", "FAVORITE FLAVORED SPARKLING WATER");
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
            HeaderTabs.Add("FAVOURITE RTD TEA", "FAVORITE RTD TEA");
            HeaderTabs.Add("FAVOURITE RTD COFFEE", "FAVORITE RTD COFFEE");
            HeaderTabs.Add("FAVOURITEUNFLAVOREDSPARKLINGWATER", "FAVORITE UNFLAVORED SPARKLING WATER");
            HeaderTabs.Add("FAVOURITEFLAVOREDNON-SPARKLINGWATER", "FAVORITE FLAVORED NON-SPARKLING WATER");
        }
        public string AppendStars(string ChannelorNet)
        {
            string stars = string.Empty;
            if (ChannelNets.Contains("Channels|" + ChannelorNet, StringComparer.OrdinalIgnoreCase))
            {
                stars = "**";
            }
            else if (ChannelNets.Contains("RetailerNet|" + ChannelorNet, StringComparer.OrdinalIgnoreCase))
            {
                stars = "**";
            }
            return stars;
        }
        public bool IsChannel(string Channel)
        {
            bool is_Channel = false;
            if (ChannelNets.Contains("Channels|" + Channel, StringComparer.OrdinalIgnoreCase))
            {
                is_Channel = true;
            }

            return is_Channel;
        }
        #region Format Appendix table
        public DataSet FormatAppendixTable(DataSet dtbl)
        {
            string[] strBenchMark = reportparams.Benchmark.Split('|');
            if (dtbl != null && dtbl.Tables.Count > 0 && dtbl.Tables[0].Rows.Count > 0)
            {
                foreach (DataTable tb in dtbl.Tables)
                {
                    foreach (DataRow row in tb.Rows)
                    {
                        if (strBenchMark.Length > 1)
                        {
                            if (Convert.ToString(row["Objective"]).Equals(reportparams.BenchmarkShortName, StringComparison.OrdinalIgnoreCase))
                            {
                                row["Significance"] = 1000;
                            }
                            else
                            {
                                row["Significance"] = CommonFunctions.SetReportMediumSamplesizeColorNumber(Convert.ToString(row["NoOfRespondents"]), Convert.ToString(row["Significance"]), accuratestatvalueposi, accuratestatvaluenega);
                            }
                        }
                    }
                }
            }
            return dtbl;
        }
        #endregion    
        public string Get_ShortNames(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (_commonfunctions.HeaderTabs.ContainsKey(spVal))
                    slRetVal = _commonfunctions.HeaderTabs[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        public string FormateDateAndTime(string month)
        {
            if (month.Length == 1)
            {
                return "0" + month;
            }
            else
                return month;
        }
        public string CopyFilesToDestination(string _source, string filename)
        {
            UserParams userparam = Session[SessionVariables.USERID] as UserParams;
            if (userparam == null)
                Response.Redirect("~/Home/SignOut");

            UserExportFileName = "~/Downloads/ReportGenerator/" + userparam.UserName + "/" + filename + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UserExportFileName));
            }

            string destination = HttpContext.Current.Server.MapPath(UserExportFileName);
            DirectoryCopy(_source, destination, true);
            return (HttpContext.Current.Server.MapPath(UserExportFileName) + "|" + UserExportFileName);
        }
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {

            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public DataTable Get_Chart_Table(DataSet ds, string metric, int slideNumber = 0, int tableNumber = 0)
        {
            DataTable tbl = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (slideNumber == 0 || tableNumber == 0)
                {
                    foreach (DataTable tb in ds.Tables)
                    {
                        if (tb.Rows.Count > 0 && Convert.ToString(tb.Rows[0]["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase))
                        {
                            tbl = tb;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (DataTable tb in ds.Tables)
                    {
                        if (tb.Rows.Count > 0 && Convert.ToInt32(tb.Rows[0]["SlideNumber"]) == slideNumber && Convert.ToInt32(tb.Rows[0]["TableNumber"]) == tableNumber)
                        {
                            tbl = tb;
                            break;
                        }
                    }
                }
            }
            return tbl;
        }
        public DataSet GetSlideTables(DataSet ds, string metricname, string type, int slideNumber = 0, int tableNumber = 0, bool IsTop10 = false)
        {
            DataSet _ds = new DataSet();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (slideNumber == 0 || tableNumber == 0)
                {
                    foreach (DataTable tbl in ds.Tables)
                    {
                        if (tbl.Rows.Count > 0 && Convert.ToString(tbl.Rows[0]["Metric"]).Equals(metricname, StringComparison.OrdinalIgnoreCase))
                        {
                            _ds.Tables.Add(tbl.Copy());
                            break;
                        }
                    }
                }
                else
                {
                    foreach (DataTable tbl in ds.Tables)
                    {
                        if (tbl.Rows.Count > 0 && Convert.ToInt32(tbl.Rows[0]["SlideNumber"]) == slideNumber && Convert.ToInt32(tbl.Rows[0]["TableNumber"]) == tableNumber)
                        {
                            _ds.Tables.Add(tbl.Copy());
                            break;
                        }
                    }
                }
            }
            if (metricname.Equals("RetailerLoyaltyPyramid", StringComparison.OrdinalIgnoreCase))
            {
                if (_ds != null && _ds.Tables.Count > 0)
                {
                    List<string> objectives = (from row in _ds.Tables[0].AsEnumerable() select Convert.ToString(row["Objective"])).Distinct().ToList();
                    List<DataRow> query = new List<DataRow>();
                    foreach (string objective in objectives)
                    {
                        query.AddRange((from row in _ds.Tables[0].AsEnumerable()
                                        where Convert.ToString(row["Objective"]).Equals(objective)
                                        orderby row["Volume"] ascending
                                        select row).ToList());
                    }
                    _ds = new DataSet();
                    _ds.Tables.Add(query.CopyToDataTable());
                }
            }
            else if (IsTop10)
            {
                if (_ds != null && _ds.Tables.Count > 0)
                {
                    List<string> objectives = (from row in _ds.Tables[0].AsEnumerable() select Convert.ToString(row["Objective"])).Distinct().ToList();
                    List<DataRow> query = new List<DataRow>();
                    foreach (string objective in objectives)
                    {
                        query.AddRange((from row in _ds.Tables[0].AsEnumerable()
                                        where Convert.ToString(row["Objective"]).Equals(objective)
                                        orderby row["Volume"] descending
                                        select row).ToList());
                    }
                    _ds = new DataSet();
                    _ds.Tables.Add(query.CopyToDataTable());
                }
            }
            return _ds;
        }
        //Below function is to filter table for "Approximate" Row Data. 
        public DataSet FilterDataForTrip(DataTable tbl)
        {
            DataSet dstResult = new DataSet();
            DataTable tblParent = new DataTable();
            DataTable tblChild = new DataTable();
            tblParent = tbl.Clone();
            tblChild = tbl.Clone();
            tblParent.TableName = "Tb1";
            tblChild.TableName = "Tb2";

            if (tbl != null && tbl.Rows.Count > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    if (Convert.ToString(row["MetricItem"]).Trim() == "Approximate")
                    {
                        tblChild.ImportRow(row);
                    }
                    else
                    {
                        tblParent.ImportRow(row);
                    }
                }
            }
            dstResult.Tables.Add(tblParent);
            if (tblChild.Rows.Count > 0)
            {
                dstResult.Tables.Add(tblChild);
            }
            else
            {
                tblChild = tblParent.Copy();
                tblChild.TableName = "Tb2";
                dstResult.Tables.Add(tblChild);
            }
            return dstResult;
        }
        public DataSet CleanXML(DataSet ds)
        {           
            return ds;
        }
        public DataTable CleanXMLBeforeBind(DataTable dt)
        {
            DataTable tbl = dt.Copy();
            //var results2 = from myRow in tbl.AsEnumerable()
            //               where (myRow.Field<string>("MetricItem").Contains("&amp;") || myRow.Field<string>("MetricItem").Contains("&amp;lt;") || myRow.Field<string>("MetricItem").Contains("&amp;gt;"))
            //               select myRow.Field<object>("MetricItem");
            //List<object> Rowlist = results2.ToList();

            //if (Rowlist.Count != 0)
            //{
            //    for (int i = 0; i < Rowlist.Count; i++)
            //    {
            //        foreach (DataRow row in tbl.Rows)
            //        {
            //            if (Convert.ToString(row["MetricItem"]) == Convert.ToString(Rowlist[i]))
            //            {
            //                row["MetricItem"] = Convert.ToString(row["MetricItem"]).Replace("&amp;lt;", "<").Replace("&amp;", "&").Replace("&amp;lt;", ">");
            //                break;
            //            }
            //        }
            //    }
            //}
            return tbl;
        }
        public DataTable GetSlideIndividualTable(DataTable dt, string objname)
        {
            DataTable _dt = dt.Clone();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToString(dr["Objective"]).Equals(objname, StringComparison.OrdinalIgnoreCase))
                    {
                        _dt.ImportRow(dr);
                    }
                }
            }
            return _dt;
        }
        public DataTable ValidateSingleDatatable(DataSet ds)
        {
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        public DataTable ReverseRowsInDataTable(DataTable inputTable)
        {
            DataTable outputTable = inputTable.Clone();

            for (int i = inputTable.Rows.Count - 1; i >= 0; i--)
            {
                outputTable.ImportRow(inputTable.Rows[i]);
            }
            return outputTable;
        }
        public void UpdateLabelPercentageAndIndexValue(string xmlpath, DataTable dt, Dictionary<string, int> replacelist, string segment)
        {
            XmlDocument xmlChart = new XmlDocument();
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = null;
            modulegraphicFrames = xmlChart.GetElementsByTagName("a:r");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList atnodes = graphicnode.SelectNodes("a:t", nsmgr);
                foreach (XmlNode atnode in atnodes)
                {
                    if (replacelist.ContainsKey(atnode.InnerText.Trim()))
                    {
                        XmlNodeList srgbClrnodes = graphicnode.SelectNodes("a:rPr/a:solidFill/a:srgbClr", nsmgr);
                        foreach (XmlNode srgbclrnode in srgbClrnodes)
                        {
                            XmlAttributeCollection srgbattr = srgbclrnode.Attributes;
                            if (srgbattr != null && srgbattr["val"] != null)
                            {
                                srgbattr["val"].Value = GetIndexlabelColor(Convert.ToString(dt.Rows[replacelist[atnode.InnerText.Trim()]][segment + " Index"]));
                            }
                        }
                        atnode.InnerText = GetSegmentAndTotalValue(Convert.ToString(dt.Rows[replacelist[atnode.InnerText.Trim()]][segment])) + "% (" + GetSegmentAndTotalValue(Convert.ToString(dt.Rows[replacelist[atnode.InnerText.Trim()]][segment + " Index"])) + ")";
                    }
                }
            }
            xmlChart.Save(xmlpath);
        }
        public string GetIndexlabelColor(string indexvalue)
        {
            string _xmltext = string.Empty;
            if (!string.IsNullOrEmpty(indexvalue))
            {
                double comparisonvalue = Convert.ToDouble(indexvalue);
                double index = Math.Round(comparisonvalue * 100, 0);
                if (index >= 120)
                {
                    _xmltext = "19B30D";
                }
                else if (index <= 80)
                {
                    _xmltext = "FF0000";
                }
                else if (index < 120 && index > 80)
                {
                    _xmltext = "000000";
                }
            }
            else
            {

            }
            return _xmltext;
        }
        public string GetSegmentAndTotalValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Convert.ToString(Math.Round(Convert.ToDouble(value) * 100, 0)) + "%";
            }
            else
            {
                value = "0%";
            }
            return value;
        }
        public void UpdateOvalData(string xmlpath, Dictionary<string, string> strOval)
        {
            XmlDocument xmlChart = new XmlDocument();
            string xmltext = string.Empty;
            xmlChart.Load(xmlpath);
            XmlNodeList XmlNodelst = xmlChart.GetElementsByTagName("p:sp");
            foreach (XmlNode xmlNode in XmlNodelst)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = xmlNode.SelectNodes("p:nvSpPr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                    foreach (string key in strOval.Keys)
                    {
                        if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                        {
                            cNvPrnodes = xmlNode.SelectNodes("p:txBody/a:p/a:r/a:t", nsmgr);
                            cNvPrnodes[0].InnerText = strOval[key];
                        }
                    }
                }
            }
            xmlChart.Save(xmlpath);
        }
        public string GetIndexTablelabelColor(string value, string significancevalue, string blackcelltext, string greencelltext, string redcelltext)
        {
            string _xmltext = string.Empty;
            if (!string.IsNullOrEmpty(significancevalue))
            {
                double comparisonvalue = Convert.ToDouble(significancevalue);
                double index = Math.Round(comparisonvalue * 100, 0);
                if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                {
                    _xmltext = greencelltext.Replace("CellValue", Convert.ToString(value) + "%");
                }
                else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                {
                    _xmltext = redcelltext.Replace("CellValue", Convert.ToString(value) + "%");
                }
                else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                {
                    _xmltext = blackcelltext.Replace("CellValue", Convert.ToString(value) + "%");
                }
            }
            else
            {
                _xmltext = blackcelltext.Replace("CellValue", Convert.ToString(value) + "%");
            }
            return _xmltext;
        }
        public DataTable Get_Summary_Table(DataSet ds, List<string> _metriclist)
        {
            DataTable tbl = new DataTable();
            List<object> objectives = null;
            List<string> rows = null;
            //List<string> hidelist = new List<string>() { "StoreAttribute0", "StoreAttribute1", "StoreAttribute2", "StoreAttribute3", "GoodPlaceToShop0", "GoodPlaceToShop1", "GoodPlaceToShop2", "GoodPlaceToShop3", "Reasons for Store Choice - Top 2 Box" };
            List<string> hidelist = new List<string>() { "StoreAttribute0", "GoodPlaceToShop0" };
            _metriclist.AddRange(hidelist);
            if (ds != null && ds.Tables != null)
            {
                tbl.Columns.Add("Metric");
                var query = from r in ds.Tables[0].AsEnumerable()
                            where !Convert.ToString(r.Field<object>("Objective")).Equals(Convert.ToString(ds.Tables[0].Rows[0]["Objective"]), StringComparison.OrdinalIgnoreCase)
                            select r.Field<object>("Objective");
                objectives = query.Distinct().ToList();
                if (objectives != null && objectives.Count > 0)
                {
                    foreach (object objective in objectives)
                    {
                        tbl.Columns.Add(objective + "GreenColor");
                        tbl.Columns.Add(objective + "RedColor");
                    }

                    //add rows
                    foreach (DataTable tb in ds.Tables)
                    {
                        if (_metriclist != null && !_metriclist.Contains(Get_ShortNames(Convert.ToString(tb.Rows[0]["Metric"]))))
                        {
                            if (!metriclist.Contains(Get_ShortNames(Convert.ToString(tb.Rows[0]["Metric"])), StringComparer.OrdinalIgnoreCase))
                            {
                                rows = new List<string>();
                                rows.Add(Get_ShortNames(Convert.ToString(tb.Rows[0]["Metric"])));
                                foreach (object objective in objectives)
                                {
                                    string sgColor = string.Empty;
                                    rows.AddRange(Get_Summary_Metrics(tb, Convert.ToString(objective), out sgColor));
                                }
                                tbl.Rows.Add(rows.ToArray());
                                metriclist.Add(Get_ShortNames(Convert.ToString(tb.Rows[0]["Metric"])));
                            }
                        }
                    }
                }
            }

            return tbl;
        }
        public List<object> GetColumnlist(DataTable tbl)
        {
            List<object> _columnlist = new List<object>();
            if (tbl != null && tbl.Rows.Count > 0)
            {
                _columnlist = new List<object>();
                foreach (object column in tbl.Columns)
                {
                    if (!Convert.ToString(column).Equals("Metric", StringComparison.OrdinalIgnoreCase) && !_columnlist.Contains(Convert.ToString(column).Replace("GreenColor", "").Replace("RedColor", "")))
                    {
                        _columnlist.Add(Convert.ToString(column).Replace("GreenColor", "").Replace("RedColor", ""));
                    }
                }
            }
            return _columnlist;
        }
        public List<string> Get_Summary_Metrics(DataTable tbl, string channelOrRetailer, out string sigcolor)
        {
            List<string> MetricList = new List<string>();
            List<string> MetricValues = new List<string>();
            List<DataRow> rows = new List<DataRow>();
            sigcolor = string.Empty;
            bool top10 = false;
            int greenitems = 0;
            int reditems = 0;
            List<object> objectives = null;
            bool is45comp = false;
            var query0 = from r in tbl.AsEnumerable()
                         where !Convert.ToString(r.Field<object>("Objective")).Equals(Convert.ToString(tbl.Rows[0]["Objective"]), StringComparison.OrdinalIgnoreCase)
                         select r.Field<object>("Objective");
            objectives = query0.Distinct().ToList();
            if (objectives != null && objectives.Count > 0)
            {
                if (objectives.Count <= 2)
                {
                    is45comp = false;
                }
                else
                {
                    is45comp = true;

                }

            }
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //List<string> redandgreenlist = new List<string>() {  "Reasons For Store Choice", "Destination Items",
                //    "Preparation Types","Items Purchased Detail","Impulse Item",
                //    "Beverage purchased Monthly","CSD Regular/CSD Diet","Energy Drinks/Shots","Sport Drinks",
                //    "RTD Tea","Bottled Water Still Unflavored","Juice(Nets)","RTD Coffee",
                //    "100% Orange Juice","Bottled Water Still Flavored","Good Place To Shop Factors","Store Attributes",
                //    "StoreAttribute0","Good Places To Shop for","GoodPlaceToShop0","Beverage purchased Monthly","Regular Carbonated Soft Drinks",
                //    "Diet Carbonated Soft Drinks","Unflavored Bottled Water (Non-Sparkling)","100% Orange Juice","100% Juice (not Orange)","Sports Drinks",
                //    "Tea in a Bottle, Can, Carton or Fountain","Ready-to-Drink Juice Drinks/Ade (&amp;lt;100% Juice)","Energy Drinks or Energy Shots",
                //    "Coffee in a Bottle, Can, or Carton","Good Place To Shop","Planning Types","Primary Motivation for Store Selection","StoreAttribute2","GoodPlaceToShop2","StoreAttribute3","GoodPlaceToShop3","Beverage Categories Purchased"};
                List<string> redandgreenlist = new List<string>();
                if (redandgreenlist.Contains(Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"])), StringComparer.OrdinalIgnoreCase))
                {
                    top10 = true;
                }
                if (top10)
                {

                    var query = from r in tbl.AsEnumerable()
                                where Convert.ToString(r.Field<object>("Objective")) == channelOrRetailer
                                orderby r.Field<object>("volume") descending
                                select r;
                    rows = query.ToList();
                    //green rows
                    var query1 = from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")) == channelOrRetailer
                                 && Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(r.Field<object>("Significance"))) ? "0" : r.Field<object>("Significance"))) > accuratestatvalueposi
                                 && Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(r.Field<object>("Significance"))) ? "0" : r.Field<object>("Significance"))) != 2000
                                 orderby r.Field<object>("volume") descending
                                 select r;
                    List<DataRow> greenrows = query1.ToList();


                    //red rows
                    var query2 = from r in tbl.AsEnumerable()
                                 where Convert.ToString(r.Field<object>("Objective")) == channelOrRetailer
                                 && Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(r.Field<object>("Significance"))) ? "0" : r.Field<object>("Significance"))) < accuratestatvalueposi
                                 orderby r.Field<object>("volume") descending
                                 select r;
                    List<DataRow> redrows = query2.ToList();

                    if (greenrows != null && greenrows.Count > 0 && greenrows.Count >= 2
                        && redrows != null && redrows.Count > 0 && redrows.Count >= 2)
                    {
                        if (is45comp)
                        {
                            greenitems = 1;
                            reditems = 1;
                        }
                        else
                        {
                            greenitems = 2;
                            reditems = 2;
                        }
                    }
                    else if (greenrows != null && greenrows.Count == 0
                       && redrows != null && redrows.Count > 0)
                    {
                        if (is45comp)
                        {
                            greenitems = 0;
                            reditems = 2;
                        }
                        else
                        {
                            greenitems = 0;
                            reditems = 4;
                        }
                    }
                    else if (redrows != null && redrows.Count == 0
                       && greenrows != null && greenrows.Count > 0)
                    {
                        if (is45comp)
                        {
                            greenitems = 2;
                            reditems = 0;
                        }
                        else
                        {
                            greenitems = 4;
                            reditems = 0;
                        }
                    }
                    else if (greenrows != null && greenrows.Count > 0 && greenrows.Count < 2
                       && redrows != null && redrows.Count > 0)
                    {
                        if (is45comp)
                        {
                            greenitems = 1;
                            reditems = 1;
                        }
                        else
                        {
                            greenitems = 1;
                            reditems = 3;
                        }
                    }
                    else if (redrows != null && redrows.Count > 0 && redrows.Count < 2
                      && greenrows != null && greenrows.Count > 0)
                    {
                        if (is45comp)
                        {
                            greenitems = 1;
                            reditems = 1;
                        }
                        else
                        {
                            greenitems = 3;
                            reditems = 1;
                        }
                    }
                }
                else
                {
                    var query = from r in tbl.AsEnumerable()
                                where Convert.ToString(r.Field<object>("Objective")) == channelOrRetailer
                                orderby r.Field<object>("Significance") descending
                                select r;
                    rows = query.ToList();
                }
                if (rows != null && rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        if (Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(row["Significance"])) ? "0" : row["Significance"])) > accuratestatvalueposi
                            && Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(row["Significance"])) ? "0" : row["Significance"])) != 2000)
                        {
                            MetricValues.Add(Convert.ToString(row["MetricItem"]));
                            sigcolor = "Green";
                            if (top10 && greenitems == MetricValues.Count)
                            {
                                break;
                            }
                        }
                    }
                    MetricList.Add(String.Join(", ", MetricValues));
                    MetricValues = new List<string>();

                    if (top10)
                    {
                        var query = from r in tbl.AsEnumerable()
                                    where Convert.ToString(r.Field<object>("Objective")) == channelOrRetailer
                                    && Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(r.Field<object>("Significance"))) ? "0" : r.Field<object>("Significance"))) < accuratestatvalueposi
                                    orderby r.Field<object>("volume") descending
                                    select r;
                        rows = query.ToList();
                    }
                    else
                    {
                        var query2 = from r in tbl.AsEnumerable()
                                     where Convert.ToString(r.Field<object>("Objective")) == channelOrRetailer
                                     orderby r.Field<object>("Significance") ascending
                                     select r;
                        rows = query2.ToList();
                    }
                    foreach (DataRow row in rows)
                    {
                        if (Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(row["Significance"])) ? "0" : row["Significance"])) < accuratestatvaluenega)
                        {
                            MetricValues.Add(Convert.ToString(row["MetricItem"]));
                            sigcolor = "Red";
                            if (top10 && reditems == MetricValues.Count)
                            {
                                break;
                            }
                        }
                    }
                    MetricList.Add(String.Join(", ", MetricValues));
                }
            }
            return MetricList;
        }
        public void GetSegmentValue(DataTable dtTemp, string strObjective, string strmetricitem, out string volume, out string significance, out string SampleSize)
        {
            Decimal dblResult = 0;
            volume = string.Empty;
            significance = string.Empty;
            SampleSize = string.Empty;
            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                dblResult = 0;
                foreach (DataRow dr in dtTemp.Rows)
                {
                    if (Convert.ToString(dr["MetricItem"]).Equals(strmetricitem, StringComparison.OrdinalIgnoreCase) && (Convert.ToString(dr["Objective"]).Equals(strObjective, StringComparison.OrdinalIgnoreCase)))
                    {
                        dblResult = Math.Round(Convert.ToDecimal((String.IsNullOrEmpty(Convert.ToString(dr["Volume"])) ? "0" : dr["Volume"])), 0);
                        volume = Convert.ToString(Math.Round(Convert.ToDecimal((String.IsNullOrEmpty(Convert.ToString(dr["Volume"])) ? "0" : dr["Volume"])), 0));
                        significance = Convert.ToString((String.IsNullOrEmpty(Convert.ToString(dr["Significance"])) ? "0" : dr["Significance"]));
                        SampleSize = Convert.ToString((String.IsNullOrEmpty(Convert.ToString(dr["SampleSize"])) ? "0" : dr["SampleSize"]));
                        break;
                    }
                }
            }
        }
        public void GetTableHeight_FontSize(DataTable tbl)
        {
            if (tbl != null && tbl.Rows.Count >= 8)
            {
                rowheight = (3415461 / tbl.Rows.Count).ToString();
                if (objectivecolumnlist != null && objectivecolumnlist.Count <= 4 && tbl.Rows.Count <= 10)
                {
                    fontsize = 1400;
                }
                else
                {
                    if (objectivecolumnlist.Count == 0 && (tbl.Rows.Count) / 2 <= 30)
                    {
                        fontsize = 1000;
                    }
                    else
                    {
                        fontsize = 800;
                    }
                }
            }
            else
            {
                rowheight = "487923";
                fontsize = 1400;
            }
        }
        public void UpdateAppendixSlide(string xmlpath, DataSet ds, Dictionary<string, object> tablecolumnlist, string xmltblattrname, string segment, string strColumnListKey)
        {
            XmlDocument xmlChart = new XmlDocument();
            DataTable tbl = new DataTable();
            bool isupdate = false;
            string xmltext = string.Empty;
            string xmlcolumntext1 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2column1.xml"), "tableheader");
            string xmlcolumntext2 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2column2.xml"), "tableheader");

            string xmlMetriccelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2MetricColumn.xml"), "tableheader");
            string xmlBlackcelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2BlackCell.xml"), "tableheader");
            string xmlRedcelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2RedCell.xml"), "tableheader");
            string xmlGreencelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2GreenCell.xml"), "tableheader");

            //update Gender
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Trim().Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = "<a:tblPr><a:tableStyleId>{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>" +
                                "<a:gridCol w = \"5289557\"/>" +
                                "<a:gridCol w = \"1851021\"/>" +
                                "<a:gridCol w = \"1851021\"/>" +
                            "</a:tblGrid>";

                    foreach (XmlNode tblnode in tblnodes)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            List<string> clist = Checkkeyvalue(tablecolumnlist, strColumnListKey) as List<string>;
                            xmltext += "<a:tr h = \"" + ((4486240 / (dt.Rows.Count / 2)) + 250000) + "\">";

                            xmltext += xmlcolumntext2.Replace("Column Name", Convert.ToString(clist[0]));
                            xmltext += xmlcolumntext1.Replace("Column Name", clist[1]);
                            xmltext += xmlcolumntext1.Replace("Column Name", clist[2]);

                            xmltext += "</a:tr>";
                            for (int row = 0; row < (dt.Rows.Count / 2); row++)
                            {
                                xmltext += "<a:tr h = \"" + (4486240 / (dt.Rows.Count / 2)) + "\">";
                                if (row == 0)
                                {
                                    xmltext += GetMetricXML(xmlMetriccelltext, "Sample Size");
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(dt.Rows[row]["SampleSize"])) ? "0" : dt.Rows[row]["SampleSize"])), 0)));
                                    GetSegmentValue(dt, segment, Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize);
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(SampleSize) ? "0" : SampleSize)), 0)));
                                    xmltext += "</a:tr>";

                                    xmltext += "<a:tr h = \"" + (4486240 / (dt.Rows.Count / 2)) + "\">";
                                    xmltext += GetMetricXML(xmlMetriccelltext, Get_ShortNames(Convert.ToString(dt.Rows[row]["MetricItem"])).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;"));
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(dt.Rows[row]["Volume"])) ? "0" : dt.Rows[row]["Volume"])), 1)) + "%");
                                    GetSegmentValue(dt, segment, Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize);
                                    xmltext += GetIndexTablelabelColor(volume, significance, xmlBlackcelltext, xmlGreencelltext, xmlRedcelltext);
                                }
                                else
                                {
                                    xmltext += GetMetricXML(xmlMetriccelltext, Get_ShortNames(Convert.ToString(dt.Rows[row]["MetricItem"])).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;"));
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(dt.Rows[row]["Volume"])) ? "0" : dt.Rows[row]["Volume"])), 1)) + "%");
                                    GetSegmentValue(dt, segment, Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize);
                                    xmltext += GetIndexTablelabelColor(volume, significance, xmlBlackcelltext, xmlGreencelltext, xmlRedcelltext);
                                }
                                xmltext += "</a:tr>";
                            }
                        }
                        tblnode.InnerXml = xmltext.Replace("-<", "<");
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        public void UpdateAppendixSlideFor2Comp(string xmlpath, DataSet ds, Dictionary<string, object> tablecolumnlist, string xmltblattrname, string[] complist, string strColumnListKey)
        {
            XmlDocument xmlChart = new XmlDocument();
            DataTable tbl = new DataTable();
            bool isupdate = false;
            string xmltext = string.Empty;

            string xmlcolumntext1 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2column1.xml"), "tableheader");
            string xmlcolumntext2 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2column2.xml"), "tableheader");
            string xmlMetriccelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2MetricColumn.xml"), "tableheader");
            string xmlBlackcelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2BlackCell.xml"), "tableheader");
            string xmlRedcelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2RedCell.xml"), "tableheader");
            string xmlGreencelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/2SlideXMLFiles/Slide2GreenCell.xml"), "tableheader");

            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Trim().Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = "<a:tblPr><a:tableStyleId>{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>" +
                                "<a:gridCol w = \"4691552\"/>" +
                                "<a:gridCol w = \"1433349\"/>" +
                                "<a:gridCol w = \"1433349\"/>" +
                                "<a:gridCol w = \"1433349\"/>" +
                            "</a:tblGrid>";

                    foreach (XmlNode tblnode in tblnodes)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            List<string> clist = Checkkeyvalue(tablecolumnlist, strColumnListKey) as List<string>;
                            xmltext += "<a:tr h = \"" + ((4286240 / (dt.Rows.Count / 3)) + 250000) + "\">";

                            xmltext += xmlcolumntext2.Replace("Column Name", Convert.ToString(clist[0]));
                            xmltext += xmlcolumntext1.Replace("Column Name", clist[1]);
                            xmltext += xmlcolumntext1.Replace("Column Name", clist[2]);
                            xmltext += xmlcolumntext1.Replace("Column Name", clist[3]);

                            xmltext += "</a:tr>";

                            for (int row = 0; row < (dt.Rows.Count / 3); row++)
                            {
                                xmltext += "<a:tr h = \"" + (4286240 / (dt.Rows.Count / 3)) + "\">";
                                if (row == 0)
                                {
                                    xmltext += GetMetricXML(xmlMetriccelltext, "Sample Size");
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(dt.Rows[row]["SampleSize"])) ? "0" : dt.Rows[row]["SampleSize"])), 0)));
                                    GetSegmentValue(dt, Convert.ToString(complist[1]), Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize);
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(SampleSize) ? "0" : SampleSize)), 0)));
                                    GetSegmentValue(dt, Convert.ToString(complist[3]), Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize);
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(SampleSize) ? "0" : SampleSize)), 0)));
                                    xmltext += "</a:tr>";

                                    xmltext += "<a:tr h = \"" + (4286240 / (dt.Rows.Count / 3)) + "\">";
                                    xmltext += GetMetricXML(xmlMetriccelltext, Get_ShortNames(Convert.ToString(dt.Rows[row]["MetricItem"])).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;"));
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(dt.Rows[row]["Volume"]))) ? "0" : dt.Rows[row]["Volume"]), 1)) + "%");
                                    GetSegmentValue(dt, Convert.ToString(complist[1]), Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize); // dt, complist[1], Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance);
                                    xmltext += GetIndexTablelabelColor(volume, significance, xmlBlackcelltext, xmlGreencelltext, xmlRedcelltext);
                                    GetSegmentValue(dt, Convert.ToString(complist[3]), Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize); // dt, complist[1], Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance);
                                    xmltext += GetIndexTablelabelColor(volume, significance, xmlBlackcelltext, xmlGreencelltext, xmlRedcelltext);
                                }
                                else
                                {
                                    xmltext += GetMetricXML(xmlMetriccelltext, Get_ShortNames(Convert.ToString(dt.Rows[row]["MetricItem"])).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;"));
                                    xmltext += xmlBlackcelltext.Replace("CellValue", Convert.ToString(Math.Round(Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(dt.Rows[row]["Volume"]))) ? "0" : dt.Rows[row]["Volume"]), 1)) + "%");
                                    GetSegmentValue(dt, Convert.ToString(complist[1]), Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize); // dt, complist[1], Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance);
                                    xmltext += GetIndexTablelabelColor(volume, significance, xmlBlackcelltext, xmlGreencelltext, xmlRedcelltext);
                                    GetSegmentValue(dt, Convert.ToString(complist[3]), Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance, out SampleSize); // dt, complist[1], Convert.ToString(dt.Rows[row]["MetricItem"]), out volume, out significance);
                                    xmltext += GetIndexTablelabelColor(volume, significance, xmlBlackcelltext, xmlGreencelltext, xmlRedcelltext);
                                }
                                xmltext += "</a:tr>";
                            }
                        }
                        tblnode.InnerXml = xmltext.Replace("-<", "<");
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        public void UpdateTableSlide(string xmlpath, DataTable ds, string xmltblattrname, int Comparison, string strSizeOpt)
        {
            XmlDocument xmlChart = new XmlDocument();
            DataTable tbl = new DataTable();
            bool isupdate = false;
            string xmltext = string.Empty;
            string RowSize = string.Empty;
            RowSize = (strSizeOpt == "Retailer") ? "351400" : "373400";
            string xmlMetriccelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/7SlideXMLFiles/Slide7MetricCell.xml"), "tableheader");
            string xmlBlackcelltext = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/7SlideXMLFiles/Slide7BlackCell.xml"), "tableheader");


            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Trim().Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    if (Comparison == 1)
                    {
                        xmltext = "<a:tblPr/>" +
                            "<a:tblGrid>" +
                                "<a:gridCol w = \"2814321\"/>" +
                                "<a:gridCol w = \"873632\"/>" +
                            "</a:tblGrid>";
                    }
                    else
                    {
                        xmltext = "<a:tblPr/>" +
                           "<a:tblGrid>" +
                               "<a:gridCol w = \"2101403\"/>" +
                               "<a:gridCol w = \"652325\"/>" +
                           "</a:tblGrid>";
                    }

                    foreach (XmlNode tblnode in tblnodes)
                    {
                        foreach (DataRow row in ds.Rows)
                        {
                            xmltext += "<a:tr h = \"" + RowSize + "\">";
                            xmltext += GetMetricXML7(xmlMetriccelltext, Get_ShortNames(Convert.ToString(row["MetricItem"])).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("<", "&lt;").Replace(">", "&gt;"));
                            xmltext += xmlBlackcelltext.Replace("CellValue", Math.Round(Convert.ToDecimal((string.IsNullOrEmpty(Convert.ToString(row["Volume"])) ? "0" : row["Volume"])), 1) + "%");
                            xmltext += "</a:tr>";
                        }
                        tblnode.InnerXml = xmltext.Replace("-<", "<");
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        public string Getxmlcolumntext(string xmlpath, string xmlelementname)
        {
            XmlDocument xmlChart = new XmlDocument();
            string xmltext = string.Empty;
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName(xmlelementname);

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                xmltext = graphicnode.InnerXml;
            }
            return xmltext;
        }
        public List<Color> GetColorListForGapAnalysis(DataTable tbl, String Benchmark, List<Color> lstcolor)
        {
            List<Color> ColorList = new List<Color>();
            if (tbl != null && tbl.Rows.Count > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    if (!(Convert.ToString(row["Objective"]) == Benchmark))
                    {
                        if (Convert.ToDouble(row["Volume"]) > 0)
                        {
                            ColorList.Add(lstcolor[0]);
                        }
                        else
                        {
                            ColorList.Add(lstcolor[1]);
                        }
                    }

                }
            }
            ColorList.Reverse();
            return ColorList;
        }
        public object Checkkeyvalue(Dictionary<string, object> tablecolumnlist, string keyvalue)
        {
            object klist = new object();
            if (tablecolumnlist.ContainsKey(keyvalue))
            {
                klist = tablecolumnlist[keyvalue];
            }
            return klist;
        }
        public string GetMetricXML(string xmltext, string value)
        {
            string xmlstring = string.Empty;
            xmlstring = xmltext.Replace("Number of Trips", value);
            return xmlstring;
        }
        public string GetMetricXML7(string xmltext, string value)
        {
            string xmlstring = string.Empty;
            xmlstring = xmltext.Replace("Store", value);
            return xmlstring;
        }
        public string Get14SlideIndexlabelColor(string indexvalue, string blackcelltext, string greencelltext, string redcelltext)
        {
            string _xmltext = string.Empty;
            if (!string.IsNullOrEmpty(indexvalue))
            {
                double comparisonvalue = Convert.ToDouble(indexvalue);
                double index = (comparisonvalue * 100);
                if (index > 120)
                {
                    _xmltext = greencelltext.Replace("cellvalue", GetSegmentAndTotalValue(Convert.ToString(indexvalue)));
                }
                else if (index < 80)
                {
                    _xmltext = redcelltext.Replace("cellvalue", GetSegmentAndTotalValue(Convert.ToString(indexvalue)));
                }
                else if (index <= 120 && index >= 80)
                {
                    _xmltext = blackcelltext.Replace("cellvalue", GetSegmentAndTotalValue(Convert.ToString(indexvalue)));
                }
            }
            else
            {
                _xmltext = blackcelltext.Replace("cellvalue", GetSegmentAndTotalValue(Convert.ToString(indexvalue)));
            }
            return _xmltext;
        }
        public Dictionary<string, string> AddRetailerImages(List<string> ShopperSegment)
        {
            List<string> XvalluesTextBoxesValues = new List<string>() { "image5.png", "image6.png", "image7.png" };
            Dictionary<string, string> ReplaceValues = new Dictionary<string, string>();

            //for (int i = 0; i < ShopperSegment.Count; i++)
            //{
            //    ReplaceValues.Add(XvalluesTextBoxesValues[i], GetRetailerImagePath(Get_ShortNames(ShopperSegment[i].Replace("Retailers|", ""))));
            //}
            return ReplaceValues;
        }
        public string GetRetailerImagePath(string retailer)
        {
            string imagepath = string.Empty;

            if (retailer == "Total")
            {
                retailer = "Total Shopper";
            }

            imagepath = HttpContext.Current.Server.MapPath("../Pics/PyramidChartImages/" + retailer.Replace(" (Any Priority Store in Trade Area)", "").Replace("|", "").Replace("/", "").ToUpper() + ".png").ToString();
            if (!File.Exists(imagepath))
            {
                retailer = "NoImage";
            }
            imagepath = Request.ApplicationPath + "/Templates/ReportGenerator/Pics/PyramidChartImages/" + retailer.Replace(" (Any Priority Store in Trade Area)", "").Replace("|", "").Replace("/", "") + ".png";
            return imagepath;
        }       
        public string Getxmltext(string labelcolor, string value)
        {
            string xmltext = "<a:r> " +
              "<a:rPr " +
               "lang = \"en-US\" " +
               "sz = \"" + fontsize + "\" " +
               "b = \"0\" " +
               "i = \"0\" " +
               "u = \"none\" " +
               "strike = \"noStrike\" " +
               "dirty = \"0\"> " +
                "<a:solidFill> " +
                  "<a:srgbClr val = \" " + labelcolor + "\"/> " +
                "</a:solidFill> " +
                "<a:effectLst/> " +
                "<a:latin  " +
                 "typeface = \"Calibri\" " +
                 "panose = \"020F0502020204030204\" " +
                 "pitchFamily = \"34\" " +
                 "charset = \"0\"/> " +
              "</a:rPr> " +
              "<a:t> " + value + "</a:t> " +
            "</a:r>";
            return xmltext;
        }
        public void UpdateSummaryTable(string xmlpath, DataTable tbl, List<object> tablecolumnlist, string xmltblattrname, string rowheight, List<string> columnwidth, string MetricName, double _fontsize, string Benchmark)
        {
            XmlDocument xmlChart = new XmlDocument();
            bool isupdate = false;
            string xmltext = string.Empty;
            string xmlMetric = string.Empty;
            string xmlBlack = string.Empty;
            string xmlRed = string.Empty;
            string xmlGreen = string.Empty;
            string xmlColumn = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Summary_Slide/Column.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"1400\"");

            if (chkComparisonFolderNumber == "5")
            {
                xmlMetric = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Summary_Slide/MetricCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"1100\"");
                fontsize = 900;
            }
            else
            {
                xmlMetric = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Summary_Slide/MetricCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"1200\"");
                fontsize = 1000;
            }
            xmlBlack = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Summary_Slide/BlackCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"" + fontsize.ToString() + "\"");
            xmlRed = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Summary_Slide/RedCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"" + fontsize.ToString() + "\"");
            xmlGreen = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Summary_Slide/GreenCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"" + fontsize.ToString() + "\"");
            //update Gender

            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = " <a:tblPr firstRow = \"1\" bandRow = \"1\"><a:tableStyleId>{21E4AEA4-8DFA-4A89-87EB-49C32662AFE0}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>";
                    xmltext += "<a:gridCol w = \"2098649\"/>";
                    for (int width = 0; width < columnwidth.Count; width++)
                    {
                        xmltext += "<a:gridCol w = \"" + columnwidth[width] + "\"/>";
                    }
                    xmltext += "</a:tblGrid>";

                    xmltext += "<a:tr h = \"427923\">";
                    xmltext += xmlColumn.Replace("columnname", MetricName);
                    foreach (object column in tablecolumnlist)
                    {
                        if (ReportNumber == 9)
                        {
                            xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Benchmark) + (CommonFunctions.Channellist.Contains(Convert.ToString(Benchmark)) ? " Channel" : string.Empty) + AppendStars(Convert.ToString(Benchmark)) + " differs from " + Get_ShortNames(Convert.ToString(column).Replace("GreenColor", "").Replace("RedColor", "")) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty) + AppendStars(Convert.ToString(column))));
                        }
                        else
                        {
                            xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Benchmark) + (CommonFunctions.Channellist.Contains(Convert.ToString(Benchmark)) ? " Channel" : string.Empty) + " differs from " + Get_ShortNames(Convert.ToString(column).Replace("GreenColor", "").Replace("RedColor", "")) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty)));
                        }
                    }
                    xmltext += "</a:tr>";
                    foreach (XmlNode tblnode in tblnodes)
                    {
                        for (int row = 0; row < tbl.Rows.Count; row++)
                        {
                            string Metric = cf.cleanPPTXML(Convert.ToString(tbl.Rows[row]["Metric"]));
                            xmltext += "<a:tr h = \"" + rowheight + "\">";
                            xmltext += xmlMetric.Replace("cellvalue", Metric);
                            foreach (object comparisonpoint in tablecolumnlist)
                            {
                                string xmltrtext = string.Empty;

                                if (!string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint) + "RedColor"])))
                                {
                                    xmltrtext += Getxmltext("00B050", cf.cleanPPTXML(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint) + "RedColor"])));
                                }
                                if (!string.IsNullOrEmpty(xmltrtext) && !string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint) + "RedColor"]))
                                        && !string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint) + "GreenColor"])))
                                {
                                    xmltrtext += Getxmltext("000000", ", ");
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint) + "GreenColor"])))
                                {
                                    xmltrtext += Getxmltext("FF0000", cf.cleanPPTXML(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint) + "GreenColor"])));
                                }

                                if (string.IsNullOrEmpty(xmltrtext))
                                {
                                    xmltrtext += Getxmltext("000000", string.Empty);
                                }
                                xmltext += xmlGreen.Replace("<a:r></a:r>", xmltrtext);
                            }
                            xmltext += "</a:tr>";
                        }
                        tblnode.InnerXml = xmltext;
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
            objectivecolumnlist = new List<object>();
            fontsize = 800;
        }
        //
        public void UpdateSummarySlide(string xmlpath, DataTable tblRes, string xmltblattrname, List<string> lstHeaderText, List<string> lstSize)
        {
            XmlDocument xmlChart = new XmlDocument();
            DataTable tbl = new DataTable();
            bool isupdate = false;
            string xmltext = string.Empty;
            String xmlHeaderValue1 = String.Empty;
            string xmlMetricCell = String.Empty;
            string xmlGreenCell = String.Empty;
            string xmlRedCell = String.Empty;
            string xmlBlankCell = String.Empty;
            int intSubValue = 0;

            xmlBlankCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlBlankCell.xml"), "tableheader");
            if (lstSize[2] == "16")
            {
                intSubValue = 90040;
                xmlHeaderValue1 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlHeaderValueII.xml"), "tableheader");
                xmlMetricCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlMetricCellII.xml"), "tableheader");
                xmlGreenCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlGreenCellII.xml"), "tableheader");
                xmlRedCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlRedCellII.xml"), "tableheader");
            }
            else if (lstSize[2] == "10")
            {
                intSubValue = 50000;
                xmlHeaderValue1 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlHeaderValue.xml"), "tableheader");
                xmlMetricCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlMetricCell.xml"), "tableheader");
                xmlGreenCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlGreenCell.xml"), "tableheader");
                xmlRedCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlRedCell.xml"), "tableheader");
            }
            else if (lstSize[2] == "14")
            {
                intSubValue = 95516;
                xmlHeaderValue1 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlHeaderValueIII.xml"), "tableheader");
                xmlMetricCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlMetricCellIII.xml"), "tableheader");
                xmlGreenCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlGreenCellIII.xml"), "tableheader");
                xmlRedCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/SummarySlides/xmlRedCellIII.xml"), "tableheader");
            }
            else
            {
                intSubValue = 4176364;
                xmlHeaderValue1 = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/SummarySlides/xmlHeaderValueII.xml"), "tableheader");
                xmlMetricCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/SummarySlides/xmlMetricCellII.xml"), "tableheader");
                xmlGreenCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/SummarySlides/xmlGreenCellII.xml"), "tableheader");
                xmlRedCell = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/SummarySlides/xmlRedCellII.xml"), "tableheader");
            }

            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Trim().Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = "<a:tblPr firstRow = \"1\" bandRow = \"1\"><a:tableStyleId>{21E4AEA4-8DFA-4A89-87EB-49C32662AFE0}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>" +
                                "<a:gridCol w = \"4102962\"/>" +
                                "<a:gridCol w = \"4888638\"/>" +
                            "</a:tblGrid>";

                    foreach (XmlNode tblnode in tblnodes)
                    {
                        xmltext += "<a:tr h = \"" + (((4717104 / tblRes.Rows.Count) + 2000) - intSubValue) + "\">";
                        xmltext += xmlHeaderValue1.Replace("HeaderValue1", lstHeaderText[0]);
                        xmltext += xmlHeaderValue1.Replace("HeaderValue1", lstHeaderText[1]);
                        xmltext += "</a:tr>";
                        for (int i = 0; i < tblRes.Rows.Count; i++)
                        {
                            xmltext += "<a:tr h = \"" + ((4717104 / tblRes.Rows.Count) - intSubValue) + "\">";
                            xmltext += xmlMetricCell.Replace("CellValue1", Get_ShortNames(Convert.ToString(tblRes.Rows[i]["Metric"])).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;"));
                            if (Convert.ToString(tblRes.Rows[i]["Colour"]).Equals("Green", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(tblRes.Rows[i]["Value"])))
                                    xmltext += xmlGreenCell.Replace("CellValue2", (String.IsNullOrEmpty(Convert.ToString(tblRes.Rows[i]["Value"])) ? " " : Convert.ToString(tblRes.Rows[i]["Value"]).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;")));
                            }
                            else if (Convert.ToString(tblRes.Rows[i]["Colour"]).Equals("Red", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(tblRes.Rows[i]["Value"])))
                                    xmltext += xmlRedCell.Replace("CellValue2", (String.IsNullOrEmpty(Convert.ToString(tblRes.Rows[i]["Value"])) ? " " : Convert.ToString(tblRes.Rows[i]["Value"]).Replace("& ", "&amp; ").Replace("&amp;lt;", "&lt;").Replace("<", "&lt;").Replace(">", "&gt;")));
                            }
                            else
                            {
                                xmltext += xmlBlankCell;
                            }

                            xmltext += "</a:tr>";
                        }
                        tblnode.InnerXml = xmltext.Replace("-<", "<");
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        public DataTable GetSummaryTablesData(DataSet ds, List<string> MetricItems, string[] strcomplist)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Metric");
            dt.Columns.Add("Value");
            dt.Columns.Add("Colour");
            Dictionary<string, double> CellValue = new Dictionary<string, double>();
            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = 0; i < MetricItems.Count; i++)
                {
                    foreach (DataTable tbl in ds.Tables)
                    {
                        //if (Convert.ToString(tbl.Rows[0]["Metric"]).Equals(MetricItems[i], StringComparison.OrdinalIgnoreCase))
                        //{
                        string sigcolor = string.Empty;
                        CellValue = GetSummaryCellData(GetSlideIndividualTable(tbl, strcomplist[1]), strcomplist[1], out sigcolor);
                        dt.Rows.Add(MetricItems[i].Replace("&amp;lt;", "&lt;").Replace("&amp;gt;", "&gt;"), String.Join(", ", CellValue.Keys), sigcolor);
                        break;
                        //}
                    }
                }
            }
            return dt;
        }
        public DataTable GetSummaryTablesDataFor2(DataSet ds, List<string> MetricItems, string[] strcomplist)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Metric");
            dt.Columns.Add("Value");
            dt.Columns.Add("Color1");
            dt.Columns.Add("Value2");
            dt.Columns.Add("Color2");
            Dictionary<string, double> CellValue1 = new Dictionary<string, double>();
            Dictionary<string, double> CellValue2 = new Dictionary<string, double>();
            int k = 0;
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataTable tbl in ds.Tables)
                {
                    if (tbl.Rows.Count > 0)
                    {
                        for (int i = 0; i < MetricItems.Count; i++)
                        {
                            if (Convert.ToString(tbl.Rows[0]["Metric"]).Equals(MetricItems[i], StringComparison.OrdinalIgnoreCase))
                            {
                                string sigcolor1 = string.Empty;
                                string sigcolor2 = string.Empty;
                                CellValue1 = GetSummaryCellData(GetSlideIndividualTable(tbl, strcomplist[1]), strcomplist[1], out sigcolor1);
                                CellValue2 = GetSummaryCellData(GetSlideIndividualTable(tbl, strcomplist[3]), strcomplist[3], out sigcolor2);
                                dt.Rows.Add(MetricItems[i].Replace("&amp;lt;", "&lt;").Replace("&amp;gt;", "&gt;"), String.Join(", ", CellValue1.Keys), sigcolor1, String.Join(", ", CellValue2.Keys), sigcolor2);
                                k++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        dt.Rows.Add(MetricItems[k], " ", " ");
                        if ((MetricItems.Count() - 1) > k)
                        {
                            k++;
                        }
                    }
                }
            }
            return dt;
        }
        public Dictionary<string, Double> GetSummaryCellData(DataTable tbl, string strcomp, out string sigcolor)
        {
            Double PosVal, Negval;
            double significance;

            sigcolor = string.Empty;
            Dictionary<string, Double> MetricValues = new Dictionary<string, double>();
            try
            {
                Double.TryParse(Convert.ToString(HttpContext.Current.Session["StatSessionPosi"]), out PosVal);
                Double.TryParse(Convert.ToString(HttpContext.Current.Session["StatSessionNega"]), out Negval);

                var query = from r in tbl.AsEnumerable()
                            orderby r.Field<object>("Significance") descending
                            select r;
                List<DataRow> rows = query.ToList();
                foreach (DataRow row in rows)
                {
                    if (Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(row["Significance"])) ? "0" : row["Significance"])) > PosVal)
                    {
                        double.TryParse(Convert.ToString(row["Significance"]), out significance);
                        if (!MetricValues.ContainsKey(Convert.ToString(row["MetricItem"])))
                        {
                            MetricValues.Add(Convert.ToString(row["MetricItem"]), significance);
                        }
                        sigcolor = "Green";
                    }

                    if (MetricValues.Count > 1)
                    {
                        break;
                    }
                }

                if (MetricValues.Count == 0)
                {
                    var query1 = from r in tbl.AsEnumerable()
                                 orderby r.Field<object>("Significance") ascending
                                 select r;
                    List<DataRow> rows1 = query1.ToList();
                    foreach (DataRow row in rows1)
                    {
                        if (Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(row["Significance"])) ? "0" : row["Significance"])) < Negval)
                        {
                            double.TryParse(Convert.ToString(row["Significance"]), out significance);
                            if (!MetricValues.ContainsKey(Convert.ToString(row["MetricItem"])))
                            {
                                MetricValues.Add(Convert.ToString(row["MetricItem"]), significance);
                            }
                            sigcolor = "Red";
                        }

                        if (MetricValues.Count > 1)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MetricValues;
        }
        public String GetSampleSize(DataTable tbl, string slMetricList)
        {
            //Dictionary<string, double> samplesize = new Dictionary<string, double>();
            string dblresult = "0";
            if (tbl != null && tbl.Rows.Count > 0)
            {
                var results2 = (from myRow in tbl.AsEnumerable()
                                where myRow.Field<string>("Objective") == slMetricList
                                orderby myRow.Field<Object>("NoOfRespondents") descending
                                select myRow.Field<Object>("NoOfRespondents")).Take(1);
                List<object> Rowlist = results2.ToList();

                if (Rowlist.Count != 0)
                    dblresult = (String.IsNullOrEmpty(Convert.ToString(Rowlist[0])) ? "0" : Convert.ToString(Rowlist[0]).FormateSampleSizeNumber()); //Convert.ToDouble(Rowlist[0].ToString)) ;

            }
            return dblresult;
        }
        public void UpdateAppendixMultipleTables(string xmlpath, DataSet ds, List<object> tablecolumnlist, string xmltblattrname, string rowheight, List<string> columnwidth, string tablename)
        {
            XmlDocument xmlChart = new XmlDocument();
            try
            {
                bool isupdate = false;
                string xmltext = string.Empty;
                string xmlColumn = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/Column.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"1200\"");
                string xmlMetric = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/MetricCell.xml"), "tableheader");
                string xmlBlack = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/BlackCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"" + fontsize + "\"");
                string xmlRed = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/RedCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"" + fontsize + "\"");
                string xmlGreen = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/GreenCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"" + fontsize + "\"");
                string xmlGray = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/GrayCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"" + fontsize + "\"");
                string xmlBlue = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/BlueCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");   //"sz=\"" + fontsize + "\"");

                //update Gender
                if (ds != null && ds.Tables.Count > 0)
                {
                    xmlChart.Load(xmlpath);
                    XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

                    foreach (XmlNode graphicnode in modulegraphicFrames)
                    {
                        isupdate = false; ;
                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                        nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                        XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                        foreach (XmlNode cNvPr in cNvPrnodes)
                        {
                            XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                            if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                            {
                                isupdate = true;
                            }
                        }
                        nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                        nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                        XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                        if (isupdate)
                        {
                            xmltext = " <a:tblPr><a:tableStyleId>{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}</a:tableStyleId></a:tblPr>" +
                                    "<a:tblGrid>";
                            xmltext += "<a:gridCol w = \"2844000\"/>";
                            for (int width = 0; width < columnwidth.Count; width++)
                            {
                                xmltext += "<a:gridCol w = \"" + columnwidth[width] + "\"/>";
                            }
                            xmltext += "</a:tblGrid>";

                            foreach (XmlNode tblnode in tblnodes)
                            {
                                foreach (DataTable tbl in ds.Tables)
                                {
                                    if (tbl != null && tbl.Rows.Count > 0)
                                    {
                                        xmltext += "<a:tr h = \"109803\">";
                                        xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML("Top 5 " + Convert.ToString(tbl.Rows[0]["Metric"])));
                                        foreach (object column in tablecolumnlist)
                                        {
                                            xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (IsChannel(Convert.ToString(column)) ? " Channel" : string.Empty));
                                        }
                                        xmltext += "</a:tr>";
                                        for (int row = 0; row < tbl.Rows.Count; row++)
                                        {
                                            string MetricItem = cf.cleanPPTXML(tbl.Rows[row]["MetricItem"].ToString());
                                            if (!MetricItem.Trim().ToLower().Contains("significance"))
                                            {
                                                if (!MetricItem.Trim().ToLower().Contains("sample size"))
                                                {
                                                    xmltext += "<a:tr h = \"" + rowheight + "\">";
                                                    xmltext += xmlMetric.Replace("cellvalue", Convert.ToString(MetricItem));
                                                    foreach (object comparisonpoint in tablecolumnlist)
                                                    {
                                                        string samplesize = string.Empty;
                                                        samplesize = (from r in tbl.AsEnumerable()
                                                                      where Convert.ToString(r["MetricItem"]).Equals("Sample Size")
                                                                      select Convert.ToString(r[comparisonpoint.ToString()])).FirstOrDefault();

                                                        string sigvalue = GetCellColor(Convert.ToString(tbl.Rows[row]["MetricItem"]), Convert.ToString(tbl.Rows[row + 1]["MetricItem"]), Convert.ToString(Convert.ToDouble(tbl.Rows[row + 1][Convert.ToString(comparisonpoint)])), samplesize);
                                                        if (sigvalue.Trim().ToLower() == "red")
                                                        {
                                                            xmltext += xmlRed.Replace("cellvalue", (string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) ? string.Empty : Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]).ToString("0.0") + "%"));
                                                        }
                                                        else if (sigvalue.Trim().ToLower() == "blue")
                                                        {
                                                            xmltext += xmlBlue.Replace("cellvalue", (string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) ? string.Empty : Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]).ToString("0.0") + "%"));
                                                        }
                                                        else if (sigvalue.Trim().ToLower() == "green")
                                                        {
                                                            xmltext += xmlGreen.Replace("cellvalue", (string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) ? string.Empty : Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]).ToString("0.0") + "%"));
                                                        }
                                                        else if (sigvalue.Trim().ToLower() == "gray")
                                                        {
                                                            xmltext += xmlGray.Replace("cellvalue", (string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) ? string.Empty : Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]).ToString("0.0") + "%"));
                                                        }
                                                        else if (sigvalue.Trim().ToLower() == "black")
                                                        {
                                                            xmltext += xmlBlack.Replace("cellvalue", (string.IsNullOrEmpty(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) ? string.Empty : Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]).ToString("0.0") + "%"));
                                                        }
                                                    }

                                                    xmltext += "</a:tr>";
                                                }
                                            }
                                        }
                                    }
                                }
                                tblnode.InnerXml = xmltext;
                                xmlChart.Save(xmlpath);
                                break;
                            }
                        }

                        if (isupdate)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void UpdateAppendixTable(string xmlpath, DataTable tbl, List<object> tablecolumnlist, string xmltblattrname, string rowheight, List<string> columnwidth, string tablename)
        {
            XmlDocument xmlChart = new XmlDocument();
            bool isupdate = false;
            string xmltext = string.Empty;
            string xmlColumn = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/Column.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"1200\"");
            string xmlMetric = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/MetricCell.xml"), "tableheader");
            string xmlBlack = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/BlackCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\""); //"sz=\"" + fontsize + "\"");
            string xmlRed = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/RedCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");    //"sz=\"" + fontsize + "\"");
            string xmlGreen = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/GreenCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");   //"sz=\"" + fontsize + "\"");
            string xmlGray = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/GrayCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");   //"sz=\"" + fontsize + "\"");
            string xmlBlue = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/BlueCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");   //"sz=\"" + fontsize + "\"");

            List<object> naList = new List<object>();//added by bramhanath for showing n/a instead of 0
            //update Gender

            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = " <a:tblPr><a:tableStyleId>{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>";
                    xmltext += "<a:gridCol w = \"2844000\"/>";
                    for (int width = 0; width < columnwidth.Count; width++)
                    {
                        xmltext += "<a:gridCol w = \"" + columnwidth[width] + "\"/>";
                    }
                    xmltext += "</a:tblGrid>";

                    xmltext += "<a:tr h = \"309803\">";
                    xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Convert.ToString(tablename)));
                    foreach (object column in tablecolumnlist)
                    {
                        if (ReportNumber == 9)
                        {
                            xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty) + AppendStars(Convert.ToString(column)));
                        }
                        else if (ReportNumber == 11)
                        {
                            if (tablename == "Store Imagery" || tablename == "Good Place To Shop For")
                            {
                                xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty) + AppendStars(Convert.ToString(column)));
                            }
                            else
                            {
                                xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty));
                            }
                        }
                        else
                        {
                            xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty));
                        }
                    }
                    xmltext += "</a:tr>";
                    foreach (XmlNode tblnode in tblnodes)
                    {
                        for (int row = 0; row < tbl.Rows.Count; row++)
                        {
                            naList = new List<object>();
                            string MetricItem = cf.cleanPPTXML(tbl.Rows[row]["MetricItem"].ToString());
                            if (!MetricItem.Trim().ToLower().Contains("significance"))
                            {
                                xmltext += "<a:tr h = \"" + rowheight + "\">";
                                xmltext += xmlMetric.Replace("cellvalue", Convert.ToString(MetricItem));
                                if (MetricItem.Trim().ToLower().Contains("sample size"))
                                {
                                    foreach (object comparisonpoint in tablecolumnlist)
                                    {

                                        if (Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "0" || Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "")
                                        {
                                            //xmltext += "n/a";
                                            //naList.Add(comparisonpoint);
                                            xmltext += xmlBlack.Replace("cellvalue", "n/a");
                                        }
                                        else
                                        {
                                            xmltext += xmlBlack.Replace("cellvalue", FormateSampleSize(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])));
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (object comparisonpoint in tablecolumnlist)
                                    {
                                        //if (naList.Contains(comparisonpoint))
                                        //{
                                        //    xmltext += xmlBlack.Replace("cellvalue", "n/a");
                                        //}
                                        if (Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "0" || Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "")
                                        {
                                            xmltext += xmlBlack.Replace("cellvalue", "n/a");
                                        }
                                        else
                                        {
                                            string samplesize = string.Empty;
                                            samplesize = (from r in tbl.AsEnumerable()
                                                          where Convert.ToString(r["MetricItem"]).Equals("Sample Size")
                                                          select Convert.ToString(r[comparisonpoint.ToString()])).FirstOrDefault();

                                            string sigvalue = GetCellColor(Convert.ToString(tbl.Rows[row]["MetricItem"]), Convert.ToString(tbl.Rows[row + 1]["MetricItem"]), Convert.ToString(tbl.Rows[row + 1][Convert.ToString(comparisonpoint)]), samplesize);
                                            if (sigvalue.Trim().ToLower() == "red")
                                            {
                                                xmltext += xmlRed.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "blue")
                                            {
                                                xmltext += xmlBlue.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "green")
                                            {
                                                xmltext += xmlGreen.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "gray")
                                            {
                                                xmltext += xmlGray.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "black")
                                            {
                                                xmltext += xmlBlack.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else
                                            {
                                                xmltext += xmlBlack.Replace("cellvalue", "0%");
                                            }
                                        }
                                    }
                                }
                                xmltext += "</a:tr>";
                            }
                        }
                        tblnode.InnerXml = xmltext;
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        public void UpdateTrendAppendixTable(string xmlpath, DataTable tbl, List<string> tablecolumnlist, string xmltblattrname, string rowheight, List<string> columnwidth, string tablename)
        {
            XmlDocument xmlChart = new XmlDocument();
            bool isupdate = false;
            string xmltext = string.Empty;
            string xmlColumn = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/Column.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"1200\"");
            string xmlMetric = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/MetricCell.xml"), "tableheader");
            string xmlBlack = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/BlackCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\""); //"sz=\"" + fontsize + "\"");
            string xmlRed = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/RedCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");    //"sz=\"" + fontsize + "\"");
            string xmlGreen = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/GreenCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");   //"sz=\"" + fontsize + "\"");
            string xmlGray = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/GrayCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");   //"sz=\"" + fontsize + "\"");
            string xmlBlue = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/OverTimeReportGeneratorPPTFiles/Appendix/BlueCell.xml"), "tableheader").Replace("sz=\"800\"", "sz=\"800\"");   //"sz=\"" + fontsize + "\"");

            List<object> naList = new List<object>();//added by bramhanath for showing n/a instead of 0
            //update Gender

            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = " <a:tblPr><a:tableStyleId>{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>";
                    xmltext += "<a:gridCol w = \"2844000\"/>";
                    for (int width = 0; width < columnwidth.Count; width++)
                    {
                        xmltext += "<a:gridCol w = \"" + columnwidth[width] + "\"/>";
                    }
                    xmltext += "</a:tblGrid>";

                    xmltext += "<a:tr h = \"309803\">";
                    xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Convert.ToString(tablename)));
                    foreach (object column in tablecolumnlist)
                    {
                        if (ReportNumber == 9)
                        {
                            xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty) + AppendStars(Convert.ToString(column)));
                        }
                        else if (ReportNumber == 11)
                        {
                            if (tablename == "Store Imagery" || tablename == "Good Place To Shop For")
                            {
                                xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty) + AppendStars(Convert.ToString(column)));
                            }
                            else
                            {
                                xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty));
                            }
                        }
                        else
                        {
                            xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(column))) + (CommonFunctions.Channellist.Contains(Convert.ToString(column)) ? " Channel" : string.Empty));
                        }
                    }
                    xmltext += "</a:tr>";
                    foreach (XmlNode tblnode in tblnodes)
                    {
                        for (int row = 0; row < tbl.Rows.Count; row++)
                        {
                            naList = new List<object>();
                            string MetricItem = cf.cleanPPTXML(tbl.Rows[row]["MetricItem"].ToString());
                            if (!MetricItem.Trim().ToLower().Contains("significance"))
                            {
                                xmltext += "<a:tr h = \"" + rowheight + "\">";
                                xmltext += xmlMetric.Replace("cellvalue", Convert.ToString(MetricItem));
                                if (MetricItem.Trim().ToLower().Contains("sample size"))
                                {
                                    foreach (object comparisonpoint in tablecolumnlist)
                                    {

                                        if (Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "0" || Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "")
                                        {
                                            //xmltext += "n/a";
                                            //naList.Add(comparisonpoint);
                                            xmltext += xmlBlack.Replace("cellvalue", "n/a");
                                        }
                                        else
                                        {
                                            xmltext += xmlBlack.Replace("cellvalue", FormateSampleSize(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])));
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (object comparisonpoint in tablecolumnlist)
                                    {
                                        //if (naList.Contains(comparisonpoint))
                                        //{
                                        //    xmltext += xmlBlack.Replace("cellvalue", "n/a");
                                        //}
                                        if (Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "0" || Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)]) == "")
                                        {
                                            xmltext += xmlBlack.Replace("cellvalue", "n/a");
                                        }
                                        else
                                        {
                                            string samplesize = string.Empty;
                                            samplesize = (from r in tbl.AsEnumerable()
                                                          where Convert.ToString(r["MetricItem"]).Equals("Sample Size")
                                                          select Convert.ToString(r[comparisonpoint.ToString()])).FirstOrDefault();

                                            string sigvalue = GetCellColor(Convert.ToString(tbl.Rows[row]["MetricItem"]), Convert.ToString(tbl.Rows[row + 1]["MetricItem"]), Convert.ToString(tbl.Rows[row + 1][Convert.ToString(comparisonpoint)]), samplesize);
                                            if (sigvalue.Trim().ToLower() == "red")
                                            {
                                                xmltext += xmlRed.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "blue")
                                            {
                                                xmltext += xmlBlue.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "green")
                                            {
                                                xmltext += xmlGreen.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "gray")
                                            {
                                                xmltext += xmlGray.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else if (sigvalue.Trim().ToLower() == "black")
                                            {
                                                xmltext += xmlBlack.Replace("cellvalue", Convert.ToString(Math.Round(Convert.ToDouble(tbl.Rows[row][Convert.ToString(comparisonpoint)]), 1)) + "%");
                                            }
                                            else
                                            {
                                                xmltext += xmlBlack.Replace("cellvalue", "0%");
                                            }
                                        }
                                    }
                                }
                                xmltext += "</a:tr>";
                            }
                        }
                        tblnode.InnerXml = xmltext;
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        public DataTable CreateAppendixTablePreSHOP(DataTable dt)
        {
            DataSet das = new DataSet();
            DataTable tbl = new DataTable();
            List<string> Benchmark_Comparisonlist = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                #region Create Columns
                tbl.Columns.Add("Metric", typeof(string));
                tbl.Columns.Add("MetricItem", typeof(string));
                #region Add Benchmark and Comparison Columns
                var query = from r in dt.AsEnumerable()
                            select r.Field<string>("Objective");
                Benchmark_Comparisonlist = query.Distinct().ToList();
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    tbl.Columns.Add(Get_ShortNames(bc), typeof(double));
                }
                #endregion

                #endregion

                #region Create Rows
                #region Add Sample Size
                List<object> Metricrowitems = new List<object>();
                List<object> Significancerowitems = new List<object>();
                Metricrowitems.Add(dt.Rows[0]["Metric"]);
                Metricrowitems.Add("Sample Size");
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    var query2 = from r in dt.AsEnumerable()
                                 where Get_ShortNames(r.Field<string>("Objective")) == bc
                                 select r.Field<object>("SampleSize");
                    List<object> SampleSizelist = query2.Distinct().Take(1).ToList();
                    foreach (object samplesize in SampleSizelist)
                    {
                        Metricrowitems.Add(samplesize);
                    }
                }
                tbl.Rows.Add(Metricrowitems.ToArray());
                #endregion

                #region Add Metric Items
                var query4 = from r in dt.AsEnumerable()
                             select r.Field<string>("MetricItem");
                List<string> MetricItems = query4.Distinct().ToList();
                foreach (string metric in MetricItems)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(dt.Rows[0]["Metric"]);
                    Metricrowitems.Add(metric);

                    Significancerowitems = new List<object>();
                    Significancerowitems.Add(dt.Rows[0]["Metric"]);
                    Significancerowitems.Add(metric + "Significance");
                    var query3 = from r in dt.AsEnumerable()
                                 where r.Field<string>("MetricItem") == metric
                                 select r;
                    List<DataRow> Rows = query3.ToList();
                    foreach (DataRow row in Rows)
                    {
                        Metricrowitems.Add(row["Volume"]);
                        Significancerowitems.Add(row["Significance"]);
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                    tbl.Rows.Add(Significancerowitems.ToArray());
                }

                #endregion
                #endregion
                das.Tables.Add(tbl);
            }
            return tbl;
        }
        public DataTable CreateAppendixTable(DataTable dt)
        {
            DataSet das = new DataSet();
            DataTable tbl = new DataTable();
            List<string> Benchmark_Comparisonlist = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                #region Create Columns
                tbl.Columns.Add("Metric", typeof(string));
                tbl.Columns.Add("MetricItem", typeof(string));
                #region Add Benchmark and Comparison Columns
                var query = from r in dt.AsEnumerable()
                            select r.Field<string>("Objective");
                Benchmark_Comparisonlist = query.Distinct().ToList();
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    tbl.Columns.Add(bc, typeof(double));
                }
                #endregion

                #endregion

                #region Create Rows
                #region Add Sample Size
                List<object> Metricrowitems = new List<object>();
                List<object> Significancerowitems = new List<object>();
                Metricrowitems.Add(dt.Rows[0]["Metric"]);
                Metricrowitems.Add("Sample Size");
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    var query2 = from r in dt.AsEnumerable()
                                 where r.Field<string>("Objective") == bc
                                 select r.Field<object>("SampleSize");
                    List<object> SampleSizelist = query2.Distinct().Take(1).ToList();
                    foreach (object samplesize in SampleSizelist)
                    {
                        Metricrowitems.Add(samplesize);
                    }
                }
                tbl.Rows.Add(Metricrowitems.ToArray());
                #endregion

                #region Add Metric Items
                var query4 = from r in dt.AsEnumerable()
                             select r.Field<string>("MetricItem");
                List<string> MetricItems = query4.Distinct().ToList();
                foreach (string metric in MetricItems)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(dt.Rows[0]["Metric"]);
                    Metricrowitems.Add(metric);

                    Significancerowitems = new List<object>();
                    Significancerowitems.Add(dt.Rows[0]["Metric"]);
                    Significancerowitems.Add(metric + "Significance");
                    var query3 = from r in dt.AsEnumerable()
                                 where r.Field<string>("MetricItem") == metric
                                 select r;
                    List<DataRow> Rows = query3.ToList();
                    foreach (DataRow row in Rows)
                    {
                        Metricrowitems.Add(row["Volume"]);
                        Significancerowitems.Add(row["Significance"]);
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                    tbl.Rows.Add(Significancerowitems.ToArray());
                }

                #endregion
                #endregion
                das.Tables.Add(tbl);
            }
            return tbl;
        }
        public string FormateSampleSize(string value)
        {
            string samplesize = string.Empty;
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                samplesize = "0";
            }
            else
            {
                samplesize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(value)));
            }
            return samplesize;
        }
        public string GetCellColor(string currentrow, string significancerow, string significancevalue, string samplesize)
        {
            string color = string.Empty;
            double _sampleSize = 0;
            if (!string.IsNullOrEmpty(samplesize))
            {
                _sampleSize = Convert.ToDouble(samplesize);
            }
            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (Convert.ToDouble(significancevalue) == 2000)
                    {
                        color = "Gray";
                    }
                    else if (Convert.ToDouble(significancevalue) == 1000)
                    {
                        color = "Blue";
                    }
                    else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    {
                        color = "Green";
                    }
                    else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    {
                        color = "Red";
                    }
                    else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                    {
                        color = "Black";
                    }

                }
            }
            return color;
        }
        public string[] CheckingChannelorNot(string[] complist, string[] channelornot)
        {
            try
            {
                for (int i = 0; i < complist.Length; i++)
                {
                    if (complist[i] == "Channels")
                    {

                        channelornot[i] = " Channel";
                        i = i + 1;
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return channelornot;
        }
        public int GetSlideNumber()
        {
            slide_Number += 1;
            return slide_Number;
        }
        public int GetChartNumber()
        {
            chart_Number += 1;
            return chart_Number;
        }
        public DataTable GetMetricData(string metric,DataTable tbl, bool IsTop10=false)
        {
            DataTable metric_tbl = null;
            if(tbl != null && tbl.Rows.Count > 0)
            {
                var query = (from row in tbl.AsEnumerable()
                             where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                             select row).ToList();
                if (query != null && query.Count() > 0)
                    metric_tbl = query.CopyToDataTable();
            }
            if (IsTop10)
            {
                if (metric_tbl != null)
                {
                    List<string> objectives = (from row in metric_tbl.AsEnumerable() select Convert.ToString(row["Objective"])).Distinct().ToList();
                    List<DataRow> query = new List<DataRow>();
                    foreach (string objective in objectives)
                    {
                        query.AddRange((from row in metric_tbl.AsEnumerable()
                                        where Convert.ToString(row["Objective"]).Equals(objective)
                                        orderby row["Volume"] descending
                                        select row).ToList());
                    }
                    metric_tbl = query.CopyToDataTable();
                }
            }
            return metric_tbl;
        }
    }
}