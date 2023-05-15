using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;
using System.Net;
using iSHOPNew.DAL;
using System.Collections.Generic;
using System.Collections;
using iSHOP.BLL;

namespace iSHOPNew.Models
{
    public partial class AdvancedAnalyticsContingencyTable
    {
        string UserExportFileName = string.Empty;
        string inputfile = string.Empty;
        string outputfile = string.Empty;
        FileStream file = null;
        StreamWriter writer = null;
        AdvancedAnalyticsParams advancedAnalyticsParams = null;
        List<string> selectedMetrics = new List<string>();
        List<string> ChartXValues = new List<string>();
        List<string> BCFullNames = new List<string>();
        Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] != null)
            {
                advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
                PopulateShortNames();
                Create_InputFile();
                GetChartTable();
            }
        }
        private static DataSet CreateTables(DataSet ds)
        {
            DataSet das = new DataSet();
            DataTable tbl = new DataTable();
            List<string> Benchmark_Comparisonlist = null;
            if (ds != null && ds.Tables.Count >= 3)
            {
                #region Create Columns
                tbl.Columns.Add("Metric", typeof(string));
                tbl.Columns.Add("MetricItem", typeof(string));
                #region Add Benchmark and Comparison Columns
                var query = from r in ds.Tables[2].AsEnumerable()
                            select r.Field<string>("Objective");
                Benchmark_Comparisonlist = query.Distinct().ToList();
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    tbl.Columns.Add(bc, typeof(string));
                }
                #endregion

                #endregion

                #region Create Rows
                #region Add Sample Size
                List<object> Metricrowitems = new List<object>();
                List<object> Significancerowitems = new List<object>();
                Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                Metricrowitems.Add("SampleSize");
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    var query2 = from r in ds.Tables[0].AsEnumerable()
                                 where r.Field<string>("Objective") == bc
                                 select r;
                    List<DataRow> Rows = query2.ToList();
                    if (Rows != null && Rows.Count > 0)
                    {
                        foreach (DataRow row in Rows)
                        {
                            Metricrowitems.Add(row["SampleSize"]);
                        }
                    }
                    else
                    {                     
                        if (bc.Equals("AVERAGE", StringComparison.OrdinalIgnoreCase))
                        {
                            string samplesize = (from r in ds.Tables[2].AsEnumerable()
                                                 where r.Field<string>("Objective") == bc
                                                 select Convert.ToString(r.Field<object>("SampleSize"))).FirstOrDefault();
                            Metricrowitems.Add(samplesize.ToMyString());
                        }
                        else
                        {
                            Metricrowitems.Add(string.Empty);
                        }
                    }
                }
                tbl.Rows.Add(Metricrowitems.ToArray());
                #endregion

                #region No Of Responses
                if (ds != null && ds.Tables.Count >= 4)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Metricrowitems.Add("Number of Responses");
                    foreach (string bc in Benchmark_Comparisonlist)
                    {
                        var query2 = from r in ds.Tables[3].AsEnumerable()
                                     where r.Field<string>("Metric") == bc
                                     select r;
                        List<DataRow> Rows = query2.ToList();
                        if (Rows != null && Rows.Count > 0)
                        {
                            foreach (DataRow row in Rows)
                            {
                                Metricrowitems.Add(row["SampleSize"]);
                            }
                        }
                        else
                        {
                            Metricrowitems.Add(string.Empty);
                        }
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                }
                #endregion

                #region Add Metric Items
                var query4 = from r in ds.Tables[2].AsEnumerable()
                             select r.Field<string>("MetricItem");
                List<string> MetricItems = query4.Distinct().ToList();
                foreach (string metric in MetricItems)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Metricrowitems.Add(metric);

                    Significancerowitems = new List<object>();
                    Significancerowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Significancerowitems.Add(metric + "Significance");
                    var query3 = from r in ds.Tables[2].AsEnumerable()
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
            return das;
        }
        
        public DAL.iSHOPParams Generate_Contingency_Table()
        {
            DAL.iSHOPParams param = new DAL.iSHOPParams();
            AdvancedAnalyticsParams advancedAnalyticsParams = null;
            ContingencyTable contingencyTable = new ContingencyTable();
            try
            {
                if (HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] != null)
                {
                    advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
                    string tbltxt = string.Empty;
                    string xmlstring = string.Empty;
                    DataSet ds = CreateTables(advancedAnalyticsParams.ChartDataSet);
                    var query = from r in advancedAnalyticsParams.ChartDataSet.Tables[2].AsEnumerable()
                                select r.Field<string>("Objective");
                    List<string> shortnames = query.Distinct().ToList();
                    param = contingencyTable.BindTabs(out tbltxt, out xmlstring, ds, advancedAnalyticsParams.TimePeriod, advancedAnalyticsParams.ShopperSegment,
                     advancedAnalyticsParams.FilterShortNames, advancedAnalyticsParams.ShopperFrequencyShortName, shortnames, Convert.ToString(advancedAnalyticsParams.StatPositive)
                     , Convert.ToString(advancedAnalyticsParams.StatNegative), "false", advancedAnalyticsParams.ShortTimePeriod, advancedAnalyticsParams.ViewType);
                }
            }
            catch (Exception ex)
            {

            }
            return param;
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
            HeaderTabs.Add("Diet carbonated soft drinks", "Diet SSD");
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

        private void Create_InputFile()
        {
            DataSet ds = advancedAnalyticsParams.ChartDataSet;
            List<string> Objectivelist = new List<string>();
            List<string> Metriclist = new List<string>();
            List<string> Rowlist = new List<string>();
            string RowValues = string.Empty;
            try
            {
                if (ds != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("name");
                    DataRow drVal, dr;
                    var query = from r in ds.Tables[1].AsEnumerable()
                                select r.Field<string>("Objective");
                    Objectivelist = query.Distinct().ToList();
                    var query2 = from r in ds.Tables[1].AsEnumerable()
                                 select r.Field<string>("MetricItem");
                    List<string> AllMetrics = query2.Distinct().ToList();
                    int i = 0, j = 0, row_index = 0;
                    foreach (string Object in Objectivelist)
                    {
                        j = 0;
                        dt.Columns.Add(Object);
                        foreach (string metric in AllMetrics)
                        {  
                                //add row heading
                                dr = dt.Rows.Add();
                                dr["name"] = metric;
                                drVal = dt.Rows[j];
                                var query5 = from r in ds.Tables[1].AsEnumerable()
                                             where r.Field<string>("MetricItem") == metric && r.Field<string>("Objective") == Object
                                             select r.Field<double>("Volume");
                                drVal[i + 1] = query5;
                                                     
                            j++;
                        }
                        i++;
                    }

                    List<RData> rOutput = new List<RData>();
                    DataTable res = Stats.CorrespondenceAnalysis(dt);
                    foreach (DataRow item in res.Rows)
                    {
                        RData temp = new RData();
                        temp.name = Convert.ToString(item[0]);
                        temp.x = Convert.ToString(item[1]);
                        temp.y = Convert.ToString(item[2]);
                        rOutput.Add(temp);
                    }
                    //info.rData = rOutput;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        private void CloseProcess()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                String ProcessUserSID = theprocess.StartInfo.EnvironmentVariables["USERNAME"];
                String CurrentUser = Environment.UserName;
                if (theprocess.ProcessName.ToLower().ToString() == "cmd" && ProcessUserSID == CurrentUser)
                {
                    theprocess.Kill();
                }
            }
        }
        public static string GetIP4Address()
        {
            string IP4Address = "";
            IP4Address = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (IP4Address == "" || IP4Address == null)
                IP4Address = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(IP4Address) && IP4Address.Trim().ToLower() == "::1")
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }
            }
            return IP4Address;
        }
        private DataTable GetChartTable()
        {
            DataTable dataTable = new DataTable();
           UserParams userparam = HttpContext.Current.Session[SessionVariables.USERID] as UserParams;
            if (userparam == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");
                }
                else
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                }
            }
            string pathOnly = CommonFunctions.Rserve_Serverpath + userparam.UserName.Replace(".", "");
            string outputfilepath = outputfile;
            //string outputfilepath = "plotchartoutput.csv";
            try
            {
                dataTable = new DataTable();
                dataTable.Columns.Add("NoName", typeof(string));
                dataTable.Columns.Add("V1", typeof(double));
                dataTable.Columns.Add("V2", typeof(double));
                string fileName = CommonFunctions.Rserve_Serverpath + userparam.UserName.Replace(".", "") + "/" + outputfile;
                List<string> lines = File.ReadAllLines(fileName).ToList();
                for (int line = 1; line < lines.Count; line++)
                {
                    string[] rowvalues = lines[line].Split(',');
                    List<string> drow = new List<string>();
                    for (int colname = 0; colname < dataTable.Columns.Count; colname++)
                    {
                        if (colname <= rowvalues.Count())
                        {
                            drow.Add(rowvalues[colname].Replace("\"", "").Replace("|", ","));
                        }
                        else
                        {
                            drow.Add(string.Empty);
                        }
                    }
                    dataTable.Rows.Add(drow.ToArray());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            //delete input and output files
            if (File.Exists(CommonFunctions.Rserve_Serverpath + userparam.UserName.Replace(".", "") + "/" + outputfile))
            {
                File.Delete(CommonFunctions.Rserve_Serverpath + userparam.UserName.Replace(".", "") + "/" + outputfile);
            }
            if (File.Exists(CommonFunctions.Rserve_Serverpath + userparam.UserName.Replace(".", "") + "/" + inputfile))
            {
                File.Delete(CommonFunctions.Rserve_Serverpath + userparam.UserName.Replace(".", "") + "/" + inputfile);
            }
            HttpContext.Current.Session["correspondenceData"] = dataTable;
            return dataTable;
        }

        public static DataSet FilterDataSet(DataSet inputds)
        {
            DataSet ds = new DataSet();
            CommonFunctions cf = new CommonFunctions();
            if (inputds != null && inputds.Tables != null)
            {
                ds = inputds.Copy();
                foreach (DataTable tbl in ds.Tables)
                {
                    if (tbl.Columns.Contains("MetricItem"))
                    {
                        foreach (DataRow row in tbl.Rows)
                        {
                            row["MetricItem"] = cf.Get_ShortNames(Convert.ToString(row["MetricItem"]));
                        }
                    }
                }

            }
            return ds;
        }
        [WebMethod]
        public static CorrespondenceParams StoreChartInputSelection(string ChartType, string ActiveTabSP, string BenchMark, string Comparisonlist, string TimePeriod, string ShortTimePeriod,
             string Filters, string ShortNames, string FrequencyTitle, string ShopperFrequency, string ShopperFrequencyShortName, string Metric, string MetricShortName, string ModuleBlock,
            string FilterShortNames, string ShopperSegment, int ChartHeight, int ChartWidth, string View, string SelectedMetrics,
            string[] ComparisonItems, string[] StoreidItems, string[] ComparisonShortNameItems)
        {
            AdvancedAnalyticsParams advancedAnalyticsParams = new AdvancedAnalyticsParams();
            CorrespondenceParams correspondenceParams = new CorrespondenceParams();
            CommonFunctions cf = new CommonFunctions();
            List<string> Objectivelist = new List<string>();
            List<string> LowVolumelist = new List<string>();
            List<string> Metriclist = new List<string>();
            List<string> LowSampleSizelist = new List<string>();
            List<string> LowSampleSizeStoreIDlist = new List<string>();

            correspondenceParams.LowSampleSizeShortNames = string.Empty;
            correspondenceParams.LowVariables = string.Empty;
            try
            {
                advancedAnalyticsParams.LowVolume = string.Empty;
                advancedAnalyticsParams.Benchmark = BenchMark.Replace("~", "`");
                advancedAnalyticsParams.Comparisonlist = Comparisonlist.Replace("~", "`");
                advancedAnalyticsParams.TimePeriod = TimePeriod;
                advancedAnalyticsParams.ShortTimePeriod = ShortTimePeriod;
                advancedAnalyticsParams.Filters = Filters;
                if (string.IsNullOrEmpty(FrequencyTitle) || FrequencyTitle == "null")
                {
                    advancedAnalyticsParams.FrequencyTitle = null;
                }
                else
                {
                    advancedAnalyticsParams.FrequencyTitle = FrequencyTitle;
                }
                advancedAnalyticsParams.ShopperFrequency = ShopperFrequency;
                advancedAnalyticsParams.ShopperFrequencyShortName = ShopperFrequencyShortName;
                advancedAnalyticsParams.Metric = Metric.Replace("<", "&lt;");
                advancedAnalyticsParams.MetricShortName = MetricShortName;
                advancedAnalyticsParams.ChartType = ChartType;
                advancedAnalyticsParams.ActiveTab = ActiveTabSP;
                advancedAnalyticsParams.FilterShortNames = FilterShortNames;
                advancedAnalyticsParams.ShopperSegment = ShopperSegment.Replace("~", "`");
                advancedAnalyticsParams.ModuleBlock = ModuleBlock;
                advancedAnalyticsParams.SelectedMetrics = SelectedMetrics;
                advancedAnalyticsParams.ChartHeight = ChartHeight;
                advancedAnalyticsParams.ChartWidth = ChartWidth;
                advancedAnalyticsParams.View = View;

                advancedAnalyticsParams.StatPositive = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
                advancedAnalyticsParams.StatNegative = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
                advancedAnalyticsParams.StatTesting = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);

                //check 0 volume              

                object[] paramvalues = null;

                if (Convert.ToString(advancedAnalyticsParams.ModuleBlock).Contains("Within"))
                {
                    paramvalues = new object[] { advancedAnalyticsParams.TimePeriod, advancedAnalyticsParams.Comparisonlist, advancedAnalyticsParams.ShopperFrequency, advancedAnalyticsParams.ShopperSegment.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), advancedAnalyticsParams.Filters, advancedAnalyticsParams.SelectedMetrics };
                    advancedAnalyticsParams.ActiveTab = "USP_IShopCorrespondenceWithinRetailer";
                }
                else
                {
                    paramvalues = new object[] { advancedAnalyticsParams.TimePeriod, advancedAnalyticsParams.Comparisonlist, advancedAnalyticsParams.ShopperFrequency, advancedAnalyticsParams.Filters, advancedAnalyticsParams.SelectedMetrics };
                    advancedAnalyticsParams.ActiveTab = "sp_IShopCorrespondenceAcrossRetailerMain";
                }
                DataAccess dal = new DataAccess();
                DataSet ds = dal.GetData(paramvalues, advancedAnalyticsParams.ActiveTab);
                ds = FilterDataSet(ds);
                advancedAnalyticsParams.ChartDataSet = ds.Copy();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var query = from r in ds.Tables[0].AsEnumerable()
                                select r.Field<string>("Objective");
                    Objectivelist = query.Distinct().ToList();
                    //check sample size
                    foreach (string objective in Objectivelist)
                    {
                        var query2 = from r in ds.Tables[0].AsEnumerable()
                                     where r.Field<string>("Objective") == objective
                                     select r.Field<object>("SampleSize");
                        List<object> volumn = query2.Where(x => Convert.ToDouble(x) < 30).ToList();
                        if (volumn != null && volumn.Count > 0)
                        {
                            for (int i = 0; i < ComparisonShortNameItems.Count(); i++)
                            {
                                if (ComparisonShortNameItems[i].Equals(objective, StringComparison.OrdinalIgnoreCase))
                                {
                                    LowVolumelist.Add(cf.Get_ShortNames(objective));
                                    LowSampleSizelist.Add(ComparisonItems[i]);
                                    LowSampleSizeStoreIDlist.Add(StoreidItems[i]);
                                }
                            }
                        }
                    }
                    if (LowVolumelist != null && LowVolumelist.Count > 0)
                    {
                        correspondenceParams.LowSampleSize = String.Join("~", LowSampleSizelist);
                        correspondenceParams.StoreidItems = String.Join("|", LowSampleSizeStoreIDlist);
                        correspondenceParams.LowSampleSizeShortNames = String.Join("|", LowVolumelist);
                        advancedAnalyticsParams.LowVolume = "Sample size too low for selected Retailer(s): " + String.Join(", ", LowVolumelist) + " and cannot proceed for correspondence analysis.";
                    }
                    //check 0 variables

                    if (string.IsNullOrEmpty(advancedAnalyticsParams.LowVolume))
                    {
                        var query3 = from r in ds.Tables[1].AsEnumerable()
                                     select r.Field<string>("MetricItem");
                        Metriclist = query3.Distinct().ToList();
                        foreach (string metric in Metriclist)
                        {
                            var query4 = from r in ds.Tables[1].AsEnumerable()
                                         where r.Field<string>("MetricItem") == metric
                                         select r.Field<double>("Volume");
                            List<double> volumn = query4.Where(x => x == 0).ToList();
                            if (query4 != null && volumn != null && query4.ToList().Count == volumn.Count)
                            {
                                LowVolumelist.Add(metric);
                            }
                        }
                        if ((Metriclist.Count - LowVolumelist.Count) < 3)
                        {
                            advancedAnalyticsParams.LowVolume = "Data not available for selected element: " + String.Join(", ", LowVolumelist) + ". Please make other selections.";
                            correspondenceParams.LowVariables = "Data not available for selected element: " + String.Join(", ", LowVolumelist) + ". Please make other selections.";
                        }
                    }
                    advancedAnalyticsParams.LowVolumelist = LowVolumelist;
                }
                //
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] = advancedAnalyticsParams;
            return correspondenceParams;
        }
        [WebMethod]
        public static bool Check_Download_Data()
        {
            bool isDownloadData = true;
            if (HttpContext.Current.Session["correspondenceData"] == null || HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] == null)
            {
                isDownloadData = false;
            }
            return isDownloadData;
        }

        [WebMethod]
        public static string GetSampleSize()
        {
            string sampleSize = string.Empty;
            List<string> samplesizelist = new List<string>();
            CommonFunctions _cf = new CommonFunctions();
            AdvancedAnalyticsParams advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
            if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                  && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                {
                    samplesizelist.Add(cf.cleanExcelXML(_cf.Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + CommonFunctions.CheckdecimalValue(Convert.ToString(row["SampleSize"])) + ")");
                }
            }
            if (samplesizelist.Count > 0)
            {
                sampleSize = String.Join(", ", samplesizelist);
            }
            return sampleSize;
        }

        [WebMethod]
        public static string GetDataNotAvailableNote()
        {
            string Variablenote = string.Empty;
            AdvancedAnalyticsParams advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
            if (advancedAnalyticsParams != null)
            {
                Variablenote = "<div style=\"clear:both;\"><span style=\"font-weight:bold;\">Sample Size: </span>" + GetSampleSize() + "</div>";
                if (advancedAnalyticsParams.LowVolumelist != null && advancedAnalyticsParams.LowVolumelist.Count > 0)
                {
                    Variablenote += "<span style=\"font-weight:bold;\">Note:</span> Data not available for selected element(s): " + String.Join(", ", advancedAnalyticsParams.LowVolumelist);
                }
            }
            return Variablenote;
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
    }
}