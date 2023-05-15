using Aspose.Slides;
using iSHOP.BLL;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using System.Web.Security;

namespace iSHOPNew.Models
{
    public class CommonFunctions
    {
        public Dictionary<string, string> TableMappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, string> TableMappingListtotal = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public static string Rserve_Serverpath = HttpContext.Current.Server.MapPath("~/iSHOPExplorer/AdvancedAnalyticsUserExportFiles/");

        public Dictionary<string, string> Beverage_Shoppers_MappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, string> Beverage_Trips_MappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public Dictionary<string, string> Retailer_Shoppers_MappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, string> Retailer_Trips_MappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public Dictionary<string, string> Reports_MappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public Dictionary<string, string> Reports_AppendixMappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        public static List<string> Channellist = new List<string>()
        {
            "A supermarket or grocery store",
            "A convenience store or gas station food mart (excluding gas)",
            "A drug store","A dollar store such as Family Dollar or Dollar General","A warehouse club such as Sam`s Club or Costco",
            "A Mass Merchandise store or super center such as walmart, target, walmart supercenter, or supertarget",
            "A mass merchandise store without a full-line grocery section such as Walmart or Target",
            "A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget",
            "Total"
        };
        //added by Nagaraju for Gapanalysis
        #region Get comparison Gapanalysis data
        public static DataSet GetComparisonGapanalysisData(DataSet ds, string comparison, string Benchlist)
        {
            DataTable _dt = null;
            List<string> objli = new List<string>() { Benchlist, comparison };
            DataSet dsgeo = new DataSet();
            if (ds != null && ds.Tables.Count > 0)
            {
                var query = (from row in ds.Tables[0].AsEnumerable()
                             where Convert.ToString(row["Objective"]).Equals(comparison, StringComparison.OrdinalIgnoreCase)
                             || Convert.ToString(row["Objective"]).Equals(Benchlist, StringComparison.OrdinalIgnoreCase)
                             orderby row["Volume"] descending
                             select row).ToList();
                if (query != null && query.Count > 0)
                {
                    dsgeo.Tables.Add(query.CopyToDataTable());
                    _dt = dsgeo.Tables[0].Clone();
                    foreach (string obj in objli)
                    {
                        foreach (DataRow row in dsgeo.Tables[0].Rows)
                        {
                            if (Convert.ToString(row["Objective"]).Equals(obj, StringComparison.OrdinalIgnoreCase))
                                _dt.ImportRow(row);
                        }
                    }
                    dsgeo = new DataSet();
                    dsgeo.Tables.Add(_dt);
                }
            }
            return dsgeo;
        }
        public static List<string> GetGapanalysisComparisons(DataSet ds, string Benchlist, ReportGeneratorParams reportparams)
        {
            List<string> Comparisons = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                var query = (from row in ds.Tables[0].AsEnumerable()
                             where !Convert.ToString(row["Objective"]).Equals(Benchlist, StringComparison.OrdinalIgnoreCase)
                             select Convert.ToString(row["Objective"])).Distinct().ToList();
                if (query != null && query.Count > 0)
                {
                    Comparisons = query.ToList();
                    Comparisons = Comparisons.OrderBy(x => reportparams.ComparisonShortNamelist.IndexOf(x)).ToList();
                }
            }
            return Comparisons;
        }
        #endregion

        #region set reports medium sample size colour number
        public static double SetReportMediumSamplesizeColorNumber(string samplesize, string signi, double accuratestatvalueposi, double accuratestatvaluenega)
        {
            double _sigvalue = 0;
            double _sampleSize = 0;
            if (!string.IsNullOrEmpty(signi) && !string.IsNullOrEmpty(samplesize))
            {
                _sigvalue = Convert.ToDouble(signi);
                _sampleSize = Convert.ToDouble(samplesize);
                if (_sigvalue > accuratestatvalueposi)
                    return _sigvalue;
                else if (_sigvalue < accuratestatvaluenega)
                    return _sigvalue;
                if (_sampleSize >= GlobalVariables.LowSample && _sampleSize < 100)
                {
                    _sigvalue = 2000;
                }
            }
            return _sigvalue;
        }
        #endregion

        #region merge ppt and download
        public static void MergeAndDownloadPPT(string sourcedir, string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));

            string pptfilename = string.Empty;
            DirectoryInfo dir = new DirectoryInfo(sourcedir);
            List<FileInfo> files = dir.GetFiles().OrderBy(x => int.Parse(x.Name.Split('.')[0])).ToList();
            Presentation presmerge = new Presentation();
            Presentation pres = new Presentation(files[0].FullName); // 
            ISlideCollection slds = pres.Slides;
            string reportname = string.Empty;
            try
            {
                for (int i = 1; i < files.Count(); i++)
                {
                    pptfilename = files[i].FullName;
                    reportname = files[i].FullName;
                    try
                    {
                        presmerge = new Presentation(pptfilename);
                        foreach (Slide slide in presmerge.Slides)
                        {
                            slds.AddClone(slide);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                string filename = "iSHOP_ReportGenerator_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds));

                pres.Save(HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);

                FileStream fs1 = null;
                fs1 = System.IO.File.Open(HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx"), System.IO.FileMode.Open);

                byte[] btFile = new byte[fs1.Length];
                fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                fs1.Close();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + filename + ".pptx");

                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                HttpContext.Current.Response.AddHeader("Content-Length", btFile.Length.ToString());
                HttpContext.Current.Response.BinaryWrite(btFile);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            catch (Exception ex)
            {
                string repname = reportname;
            }
        }

        //across shoppers
        public static void MergeAndDownload_AcrossShoppersPPT(string sourcedir, string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            //Aspose.Slides.License license = new Aspose.Slides.License();
            //Pass only the name of the license file embedded in the assembly
            //license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));

            string pptfilename = string.Empty;
            DirectoryInfo dir = new DirectoryInfo(sourcedir);
            List<FileInfo> files = dir.GetFiles().OrderBy(x => int.Parse(x.Name.Split('.')[0])).ToList();
            //Presentation presmerge = new Presentation();
            //Presentation pres = new Presentation(files[0].FullName); // 
            //ISlideCollection slds = pres.Slides;
            string reportname = string.Empty;
            try
            {
                //for (int i = 1; i < files.Count(); i++)
                //{
                //    pptfilename = files[i].FullName;
                //    reportname = files[i].FullName;
                //    try
                //    {
                //        presmerge = new Presentation(pptfilename);
                //        foreach (Slide slide in presmerge.Slides)
                //        {
                //            slds.AddClone(slide);
                //        }
                //    }
                //    catch (Exception ex)
                //    { 

                //    }
                //}
                //
                string filename = "iSHOP_ReportGenerator_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds));

                //pres.Save(HttpContext.Current.Server.MapPath("~/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);

                FileStream fs1 = null;
                fs1 = System.IO.File.Open(files[0].FullName, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs1.Length];
                fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                fs1.Close();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + filename + ".pptx");

                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                HttpContext.Current.Response.AddHeader("Content-Length", btFile.Length.ToString());
                HttpContext.Current.Response.BinaryWrite(btFile);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            catch (Exception ex)
            {
                string repname = reportname;
            }
        }
        #endregion
        public static List<Dictionary<string, double>> LoadChartSampleSizeSizeNew(DataSet ds)
        {
            List<Dictionary<string, double>> SampleSizeArray = new List<Dictionary<string, double>>();
            Dictionary<string, double> samplesizelist = null;

            var distinctRetailers = (from r in ds.Tables[0].AsEnumerable()
                                     select r["Retailer"]).Distinct().ToList();

            foreach (var Ret in distinctRetailers)
            {
                var numberofres = (from row in ds.Tables[0].AsEnumerable()
                                   where Convert.ToString(row.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                   && Convert.ToString(row.Field<object>("Retailer")).Equals(Ret.ToString(), StringComparison.OrdinalIgnoreCase)
                                   select row).ToDictionary(x => Convert.ToString(x["Objective"]), x => string.IsNullOrEmpty(Convert.ToString(x["Volume"])) ? 0 : Convert.ToDouble(x["Volume"]));

                if (numberofres != null && numberofres.Count() > 0)
                {
                    samplesizelist = numberofres;
                }
                else
                {
                    var numberofshoppers = (from row in ds.Tables[0].AsEnumerable()
                                            where !Convert.ToString(row.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                            && Convert.ToString(row.Field<object>("Retailer")).Equals(Ret.ToString(), StringComparison.OrdinalIgnoreCase)
                                            select row).ToDictionary(x => Convert.ToString(x["Objective"]), x => string.IsNullOrEmpty(Convert.ToString(x["Volume"])) ? 0 : Convert.ToDouble(x["Volume"]));
                    if (numberofshoppers != null && numberofshoppers.Count() > 0)
                    {
                        samplesizelist = numberofshoppers;
                    }
                }
                SampleSizeArray.Add(samplesizelist);
            }
            //return samplesizelist;
            return SampleSizeArray;
        }

        public static Dictionary<string, double> LoadChartSampleSizeSize(DataSet ds)
        {
            Dictionary<string, double> samplesizelist = null;
            var numberofres = (from row in ds.Tables[0].AsEnumerable()
                               where Convert.ToString(row.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                               select row).ToDictionary(x => Convert.ToString(x["Objective"]), x => string.IsNullOrEmpty(Convert.ToString(x["Volume"])) ? 0 : Convert.ToDouble(x["Volume"]));

            if (numberofres != null && numberofres.Count() > 0)
            {
                samplesizelist = numberofres;
            }
            else
            {
                var numberofshoppers = (from row in ds.Tables[0].AsEnumerable()
                                        where !Convert.ToString(row.Field<object>("MetricItem")).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                        select row).ToDictionary(x => Convert.ToString(x["Objective"]), x => string.IsNullOrEmpty(Convert.ToString(x["Volume"])) ? 0 : Convert.ToDouble(x["Volume"]));
                if (numberofshoppers != null && numberofshoppers.Count() > 0)
                {
                    samplesizelist = numberofshoppers;
                }
            }
            return samplesizelist;
        }
        public static string FormateDateAndTime(string month)
        {
            if (month.Length == 1)
            {
                return "0" + month;
            }
            else
                return month;
        }
        public static string AddTradeAreaNoteforChannel(string ChannelRetailer, string ShopperFrequency)
        {
            string TradeNode = string.Empty;
            if (Convert.ToString(ShopperFrequency).ToMyString().Equals("Store In Trade Area", StringComparison.OrdinalIgnoreCase))
            {
                if (ChannelRetailer.ToLower() == "convenience" || ChannelRetailer.ToLower() == "dollar" || ChannelRetailer.ToLower() == "supermarketGrocery" ||
                     ChannelRetailer.ToLower() == "massmerc" || ChannelRetailer.ToLower() == "drug" || ChannelRetailer.ToLower() == "club" ||
                     ChannelRetailer.ToLower() == "supercenter" || ChannelRetailer.ToLower() == "total shopper" || ChannelRetailer.ToLower() == "total trips")
                {
                    TradeNode = " (Any Priority Store in Trade Area)";
                }
            }
            return TradeNode;
        }
        //Nets
        public List<string> NetsHeaderTabs = new List<string>();
        public CommonFunctions()
        {
            CreateTableMappingNames();
            CreateTableMappingNamesTotal();
            PopulateShortNames();
            CreateNetsHeaderTabs();
            AddMappingKeyValues();
            Create_Report_MappingNames();
            Create_AppendixReport_MappingNames();
            //Beverage_Shoppers_MappingList = GetMappingKeyValues("BeverageAcrossShopper");
            //Beverage_Trips_MappingList = GetMappingKeyValues("BeverageAcrossTrips");

            //Retailer_Shoppers_MappingList = GetMappingKeyValues("AcrossShopper");
            //Retailer_Trips_MappingList = GetMappingKeyValues("AcrossTrips");
        }

        #region reports mapping names
        private void Create_AppendixReport_MappingNames()
        {
            Reports_AppendixMappingList.Clear();
            Reports_AppendixMappingList.Add("Reasons for Store choice", "ReasonForStoreChoice");
            Reports_AppendixMappingList.Add("Store Attributes", "StoreAttribute");
            Reports_AppendixMappingList.Add("Good Places To Shop for", "GoodPlaceToShop");
        }
        private void Create_Report_MappingNames()
        {
            Reports_MappingList.Clear();
            //Visitor Profile
            Reports_MappingList.Add("Age", "FactAgeGroups");
            Reports_MappingList.Add("Race/Ethnicity", "Ethnicity");
            Reports_MappingList.Add("HH Income", "HHIncomeGroups");
            Reports_MappingList.Add("Marital Status", "MaritalStatus");
            Reports_MappingList.Add("HH Size-Total", "HHTotal");
            Reports_MappingList.Add("HH Size-Adults in HH", "HHAdults");
            Reports_MappingList.Add("HH Size-Children in HH", "HHChildren");

            //Trip Type
            Reports_MappingList.Add("Trip Mission", "TripMission");

            //Pre Shop
            Reports_MappingList.Add("Pre-Trip origin", "PreTripOrigin");
            Reports_MappingList.Add("Day of week", "DayofWeek");
            Reports_MappingList.Add("Weekday Net", "WeekdayNet");
            Reports_MappingList.Add("Daypart of Trip", "DayParts");
            Reports_MappingList.Add("Pre Trip Planning", "VisitPlans");
            Reports_MappingList.Add("Planning Types", "VisitPreparation");
            Reports_MappingList.Add("Primary Motivation for Store Selection", "VisitMotiviations");
            Reports_MappingList.Add("Reasons for Store choice", "ReasonForStoreChoice Top 10");
            Reports_MappingList.Add("Destination Items Top 25", "DestinationItemDetails Top 10");
            Reports_MappingList.Add("Destination Items Top 10", "DestinationItemDetails");
            Reports_MappingList.Add("Gap Analysis:Reasons for Store choice", "ReasonForStoreChoice0");

            //In Store
            Reports_MappingList.Add("Smartphone/Tablet Usage in Store", "Smartphone/TabletInfluencedPurchases");
            Reports_MappingList.Add("Trips with a Food or Beverage Purchase", "ItemsPurchasedSummary");
            Reports_MappingList.Add("Items Purchased Detail", "InStoreDestinationDetails");
            Reports_MappingList.Add("Impulse Item", "ImpulseItem");
            Reports_MappingList.Add("Beverage Categories Purchased", "BeveragepurchasedMonthly");

            //TRIP SUMMARY
            Reports_MappingList.Add("# of Items Purchased", "ItemsPurchased");
            Reports_MappingList.Add("Time Spent in Store", "TimeSpent");
            Reports_MappingList.Add("Trip Expenditure", "TripExpenditure");
            Reports_MappingList.Add("Method of Payment and Store Cards", "PaymentMode");
            Reports_MappingList.Add("Type of Coupons Redeemed", "RedeemedCouponTypes");
            Reports_MappingList.Add("Destination Following Store Trip", "DestinationStoreTrip");
            Reports_MappingList.Add("Trip Satisfaction", "TripSatisfaction");

            //Frequent shopper
            Reports_MappingList.Add("Attitudinal Segment", "Attitudinal Segment");

            //CrossRetailersShopper
            Reports_MappingList.Add("Shopper Frequency1", "Shopper Frequency1");
            Reports_MappingList.Add("Shopper Frequency2", "Shopper Frequency2");

            //Shopper Perception
            Reports_MappingList.Add("Store Associations - High Level Groupings", "StoreAttributesFactors");
            Reports_MappingList.Add("Good Place To Shop for - High Level Groupings", "GoodPlaceToShopFactors");
            Reports_MappingList.Add("Store Attributes", "StoreAttribute Top 10");
            Reports_MappingList.Add("Good Places To Shop for", "GoodPlaceToShop Top 10");
            Reports_MappingList.Add("Retailer Loyalty Pyramid(Among Trade Area Shoppers)", "RetailerLoyaltyPyramid");
            Reports_MappingList.Add("Main/Favorite Store (Applicable only for Retailers/Retailer nets)", "MainFavoriteStore");

            Reports_MappingList.Add("Gap Analysis:Store Attributes", "StoreAttribute0");
            Reports_MappingList.Add("Gap Analysis:Good Places To Shop for", "GoodPlaceToShop0");

            //Beverage Interaction           
        }
        public string Get_AppendixReport_MappingName(string metric)
        {
            string metricname = metric;
            if (!string.IsNullOrEmpty(metric) && Reports_AppendixMappingList.ContainsKey(metric))
                metricname = Reports_AppendixMappingList[metric];

            return metricname;
        }
        public string Get_Report_MappingName(string metric)
        {
            string metricname = metric;
            if (!string.IsNullOrEmpty(metric) && Reports_MappingList.ContainsKey(metric))
                metricname = Reports_MappingList[metric];

            return metricname;
        }
        public DataSet Formate_Report_Metrics(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataTable tbl in ds.Tables)
                {
                    if (tbl != null && tbl.Rows.Count > 0)
                    {
                        foreach (DataRow row in tbl.Rows)
                        {
                            row["Objective"] = Convert.ToString(row["Objective"]).Trim();
                            if (Convert.ToString(row["ReportType"]).Equals("APPENDIX", StringComparison.OrdinalIgnoreCase))
                            {
                                row["Metric"] = Get_AppendixReport_MappingName(Convert.ToString(row["Metric"]));
                            }
                            else
                            {
                                row["Metric"] = Get_Report_MappingName(Convert.ToString(row["Metric"]));
                            }

                            if (string.IsNullOrEmpty(Convert.ToString(row["Volume"])))
                                row["Volume"] = 0;
                        }
                    }
                }
            }
            return ds;
        }
        #endregion

        #region authenticate user
        public static bool AuthenticateUser()
        {
            bool isAuthenticated = false;
            iSHOP.BLL.UserParams userpara = null;
            FormCollection form = HttpContext.Current.Session["Form"] as FormCollection;

            UserManager usermanager = new UserManager();
            if (System.Configuration.ConfigurationManager.AppSettings["newQueryString"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (HttpContext.Current.Request.QueryString[" CmaFa4aGgddF0vyK2Ke2g=="] != null && HttpContext.Current.Request.QueryString["5PhYxCnUO4LsMOQJUHA8Rw=="] != null
                  && HttpContext.Current.Request.QueryString["9LmgmAfJY DEr9wxJ OTZzDfZIBqTBxOm0OP4PikI0o="] != null && HttpContext.Current.Request.QueryString["Wcnup6sin10Io3eDAYsIIg=="] != null)
                {
                    userpara = new iSHOP.BLL.UserParams()
                    {
                        UserID = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString[" CmaFa4aGgddF0vyK2Ke2g=="])),
                        Name = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["5PhYxCnUO4LsMOQJUHA8Rw=="])),
                        UserName = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["9LmgmAfJY DEr9wxJ OTZzDfZIBqTBxOm0OP4PikI0o="])),
                        Role = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["Wcnup6sin10Io3eDAYsIIg=="])),
                        B3 = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["mQtWjUtawhO8K L9D9aPeg=="]))),
                        CBL = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["og1O5t72VJmcTagSmraTzw=="]))),
                        iSHOP = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["79omsApz674 jfVC7vSFjw=="]))),
                        BGM = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["3pLGUhuMy2YfifTBqZdgtg=="]))),
                        Groups = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["L9bl70 z8Z66JbssVmkYTw=="]))
                    };
                }
                else
                {
                    if (form != null && form.Count != 0)
                    {
                        userpara = new iSHOP.BLL.UserParams()
                        {
                            B3 = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["mQtWjUtawhO8K+L9D9aPeg=="]))),
                            BGM = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["3pLGUhuMy2YfifTBqZdgtg=="]))),
                            Bev360Drinks = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["acCQL/tGgywKn6nYVwccpuymQJw6Akbfb9fukTIBHwo="]))),
                            Bev360Drinkers = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["acCQL/tGgywKn6nYVwccppbeL0zwlM0Xai5hK72tlCw="]))),
                            CBL = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["og1O5t72VJmcTagSmraTzw=="]))),
                            CBLV2 = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["/UyziSdF+klY6LYqky6UTg=="]))),
                            CREST = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["e/KxbK1pXy9wg1IOerZu1w=="]))),
                            DINE = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["lUtCRCcRYf7n0sOWnoxWJQ=="]))),
                            EmailId = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["JWFvSOya6/sSK3oPY+qVug=="])),
                            Groups = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["L9bl70+z8Z66JbssVmkYTw=="])),
                            Login_Flag = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["B4BEOFhH5DF8B+ckC3cV7jjEIC2pxDLzNyB8Of2P5Jc="])),
                            Name = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["5PhYxCnUO4LsMOQJUHA8Rw=="])),
                            Role = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["Wcnup6sin10Io3eDAYsIIg=="])),
                            UserID = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["+CmaFa4aGgddF0vyK2Ke2g=="])),
                            UserName = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["9LmgmAfJY+DEr9wxJ+OTZzDfZIBqTBxOm0OP4PikI0o="])),
                            //Password = AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["8MW0Q1QpA8T+89X1efluUhuBaEHH8AL0aKXJu2f0Lyg="])),
                            iSHOP = Convert.ToBoolean(AQ.Security.Cryptography.EncryptionHelper.Decryptdata(Convert.ToString(form["79omsApz674+jfVC7vSFjw=="]))),
                        };
                    }
                }
            }
            else
            {
                if (HttpContext.Current.Request.QueryString["VXNlcklE"] != null && HttpContext.Current.Request.QueryString["TmFtZQ=="] != null
                 && HttpContext.Current.Request.QueryString["VXNlck5hbWU="] != null && HttpContext.Current.Request.QueryString["Um9sZQ=="] != null)
                {
                    userpara = new iSHOP.BLL.UserParams()
                    {
                        UserID = usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["VXNlcklE"])),
                        Name = usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["TmFtZQ=="])),
                        UserName = usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["VXNlck5hbWU="])),
                        Role = usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["Um9sZQ=="])),
                        B3 = Convert.ToBoolean(usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["QjM="]))),
                        CBL = Convert.ToBoolean(usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["Q0JM"]))),
                        iSHOP = Convert.ToBoolean(usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["aVNIT1A="]))),
                        BGM = Convert.ToBoolean(usermanager.Decryptdata(Convert.ToString(HttpContext.Current.Request.QueryString["QkdN"])))
                    };
                }
            }
            if (userpara != null)
            {
                HttpContext.Current.Session[SessionVariables.USERID] = userpara;
                isAuthenticated = true;
                FormsAuthentication.SetAuthCookie(userpara.Name, true);
            }
            else if (HttpContext.Current.Session[SessionVariables.USERID] != null)
            {
                isAuthenticated = true;
            }
            else if (HttpContext.Current.Session[SessionVariables.USERID] == null)
            {
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                 {
                    FormsAuthentication.SignOut();
                    if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                    {
                        HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["SSOLogoutPageUrl"].ToString()));
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                    }
                 }
            }
            return isAuthenticated;
        }
        #endregion
        #region rewrite host name
        public static string ReWriteHost(string _url)
        {
            return _url.ToLower().Replace("{host}", HttpContext.Current.Request.Url.Host);
        }
        #endregion
        private Dictionary<string, string> GetMappingKeyValues(string tagname)
        {
            Dictionary<string, string> MappingList = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            XmlDocument xmlChart = new XmlDocument();
            string xmlCR = HttpContext.Current.Server.MapPath("~/Templates/Retailers.xml");
            xmlChart.Load(xmlCR);
            XmlNodeList datanode = xmlChart.GetElementsByTagName(tagname);
            XmlNodeList datalist = datanode[0].SelectNodes("data");
            foreach (XmlNode data in datalist)
            {
                XmlAttributeCollection dataAttr = data.Attributes;
                if (dataAttr["label"] != null)
                {
                    if (dataAttr["label"] != null && dataAttr["shortname"] != null)
                    {
                        if (!MappingList.ContainsKey(dataAttr["label"].Value))
                            MappingList.Add(dataAttr["label"].Value, dataAttr["shortname"].Value);
                    }
                    //else if (dataAttr["label"] != null && dataAttr["shortname"] != null)
                    //{
                    //    if (!MappingList.ContainsKey(dataAttr["shortname"].Value))
                    //        MappingList.Add(dataAttr["shortname"].Value, dataAttr["label"].Value);
                    //}
                    //else if (dataAttr["label"] != null)
                    //{
                    //    if (!MappingList.ContainsKey(dataAttr["label"].Value))
                    //        MappingList.Add(dataAttr["label"].Value, string.Empty);
                    //}
                }
                //XmlNodeList subdatalist = data.SelectNodes("data");
                //foreach (XmlNode subdata in subdatalist)
                //{
                //    XmlAttributeCollection subdataAttr = subdata.Attributes;
                //    if (subdataAttr["label"] != null)
                //    {
                //        if (subdataAttr["label"] != null && subdataAttr["realName"] != null)
                //        {
                //            if (!MappingList.ContainsKey(subdataAttr["label"].Value))
                //                MappingList.Add(subdataAttr["label"].Value, subdataAttr["realName"].Value);
                //        }
                //        else if (subdataAttr["label"] != null && subdataAttr["database"] != null)
                //        {
                //            if (!MappingList.ContainsKey(subdataAttr["label"].Value))
                //                MappingList.Add(subdataAttr["label"].Value, subdataAttr["database"].Value);
                //        }
                //        else if (subdataAttr["label"] != null)
                //        {
                //            if (!MappingList.ContainsKey(subdataAttr["label"].Value))
                //                MappingList.Add(subdataAttr["label"].Value, subdataAttr["label"].Value);
                //        }
                //    }
                //}
            }
            List<string> keys = (from key in MappingList.Keys select key).ToList();
            List<string> values = (from value in MappingList.Values select value).ToList();
            string shortnames = string.Join("\n", keys);
            string longnames = string.Join("\n", values);
            return MappingList;
        }

        private void AddMappingKeyValues()
        {
            XmlDocument xmlChart = new XmlDocument();
            string xmlCR = HttpContext.Current.Server.MapPath("~/Templates/Retailers.xml");
            xmlChart.Load(xmlCR);
            XmlNodeList datalist = xmlChart.GetElementsByTagName("data");
            foreach (XmlNode data in datalist)
            {
                XmlAttributeCollection dataAttr = data.Attributes;
                if (dataAttr["label"] != null)
                {
                    if (dataAttr["label"] != null && dataAttr["shortname"] != null)
                    {
                        if (!TableMappingList.ContainsKey(dataAttr["shortname"].Value))
                            TableMappingList.Add(dataAttr["shortname"].Value, dataAttr["label"].Value);
                    }
                    else if (dataAttr["label"] != null)
                    {
                        if (!TableMappingList.ContainsKey(dataAttr["label"].Value))
                            TableMappingList.Add(dataAttr["label"].Value, dataAttr["label"].Value);
                    }

                }
            }
        }
        //public string FormateSampleSize(string value)
        //{
        //    string samplesize = string.Empty;
        //    if (string.IsNullOrEmpty(value) || value == "0")
        //    {
        //        samplesize = "0";
        //    }
        //    else
        //    {
        //        samplesize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(value)));
        //    }
        //    return samplesize;
        //}
        public string Get_Shopper_Trips_TableMappingNames(String spVal, string tagname)
        {
            if (tagname.Equals("Retailers", StringComparison.OrdinalIgnoreCase)
                || tagname.Equals("Channels", StringComparison.OrdinalIgnoreCase))
            {
                tagname = "acrossshopper";
            }
            string slRetVal = "";
            try
            {
                switch (tagname.Trim().ToLower())
                {
                    case "beverageacrossshopper":
                        {
                            if (Beverage_Shoppers_MappingList.ContainsKey(spVal))
                                slRetVal = Beverage_Shoppers_MappingList[spVal];
                            else
                                slRetVal = spVal;
                            break;
                        }
                    case "beverageacrosstrips":
                        {
                            if (Beverage_Trips_MappingList.ContainsKey(spVal))
                                slRetVal = Beverage_Trips_MappingList[spVal];
                            else
                                slRetVal = spVal;
                            break;
                        }
                    case "acrossshopper":
                        {
                            if (Retailer_Shoppers_MappingList.ContainsKey(spVal))
                                slRetVal = Retailer_Shoppers_MappingList[spVal];
                            else
                                slRetVal = spVal;
                            break;
                        }
                    case "acrosstrips":
                        {
                            if (Retailer_Trips_MappingList.ContainsKey(spVal))
                                slRetVal = Retailer_Trips_MappingList[spVal];
                            else
                                slRetVal = spVal;
                            break;
                        }
                }

            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        public string Get_TableMappingNames(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (TableMappingList.ContainsKey(spVal))
                    slRetVal = TableMappingList[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }

        public string Get_TableMappingNamesTotal(String spVal)
        {
            string slRetVal = "";
            try
            {
                if (TableMappingListtotal.ContainsKey(spVal))
                    slRetVal = TableMappingListtotal[spVal];
                else
                    slRetVal = spVal;
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }

        public string Get_NetsExtension(String extension, String spVal)
        {
            string slRetVal = extension;
            try
            {
                //if (NetsHeaderTabs.Contains(spVal, StringComparer.InvariantCultureIgnoreCase))
                //{
                //    if (Convert.ToString(extension).Equals("Retailers|", StringComparison.OrdinalIgnoreCase))
                //    {
                //        slRetVal = "RetailerNet|";
                //    }
                //    else if (Convert.ToString(extension).Equals("Category||", StringComparison.OrdinalIgnoreCase))
                //    {
                //        slRetVal = "CategoryNet||";
                //    }
                //    else if (Convert.ToString(extension).Equals("Brands|", StringComparison.OrdinalIgnoreCase) || Convert.ToString(extension).Equals("Brand|", StringComparison.OrdinalIgnoreCase))
                //    {
                //        slRetVal = "BrandNet||";
                //    }                   
                //}
            }
            catch
            {
                slRetVal = "";
            }

            return slRetVal;
        }
        //written by Nagaraju on 29-12-2014
        //Nets
        public void CreateNetsHeaderTabs()
        {
            NetsHeaderTabs = new List<string>();
            //Retailer Nets
            NetsHeaderTabs.Add("Kroger Corporate");
            NetsHeaderTabs.Add("AUSA Corporate");
            NetsHeaderTabs.Add("Safeway Corporate");
            NetsHeaderTabs.Add("Target Corporate");
            NetsHeaderTabs.Add("Walmart Banner");
            NetsHeaderTabs.Add("Walmart Inc.");
            //Brand Nets
            NetsHeaderTabs.Add("Coke Regular Brand Group");
            NetsHeaderTabs.Add("Diet Coke Brand Broup");
            NetsHeaderTabs.Add("Coke Zero Brand Group");
            NetsHeaderTabs.Add("Coke Diet/Zero Brand Group");
            NetsHeaderTabs.Add("Coke TM Brand Group");
            NetsHeaderTabs.Add("TCCC Regular Non-Colas");
            NetsHeaderTabs.Add("TCCC Diet Non-Colas");
            NetsHeaderTabs.Add("TCCC Sparkling Non Colas");
            NetsHeaderTabs.Add("Sprite ");
            NetsHeaderTabs.Add("Pepsi Regular Brand Group");
            NetsHeaderTabs.Add("Diet Pepsi Brand Group");
            NetsHeaderTabs.Add("Diet/Low Calorie Pepsi Group");
            NetsHeaderTabs.Add("Pepsi TM Brand Group");
            NetsHeaderTabs.Add("Dr. Pepper Regular Brand Group");
            NetsHeaderTabs.Add("Dr. Pepper Diet Brand Group");
            NetsHeaderTabs.Add("Dr. Pepper TM");
            NetsHeaderTabs.Add("Mountain Dew Brand Group");
            NetsHeaderTabs.Add("7-Up Brand Group");
            NetsHeaderTabs.Add("A&W Root Beer Brand Group");
            NetsHeaderTabs.Add("Sunkist Brand Group");
            NetsHeaderTabs.Add("Canada Dry Ginger Ale Brand Group");
            NetsHeaderTabs.Add("Regular Colas Flavor Group");
            NetsHeaderTabs.Add("Diet Colas Flavor Group");
            NetsHeaderTabs.Add("Total Colas Flavor Group");
            NetsHeaderTabs.Add("Regular Root Beer Flavor Group");
            NetsHeaderTabs.Add("Diet Root Beer Flavor Group");
            NetsHeaderTabs.Add("Total Root Beer Flavor Group");
            NetsHeaderTabs.Add("Regular Citrus Flavor Group");
            NetsHeaderTabs.Add("Total Citrus Flavor Group");
            NetsHeaderTabs.Add("Regular Lemon Lime Flavor Group");
            NetsHeaderTabs.Add("Diet Lemon Lime Flavor Group");
            NetsHeaderTabs.Add("Total Lemon Lime Flavor Group");
            NetsHeaderTabs.Add("Regular Flavors Group");
            NetsHeaderTabs.Add("Diet Flavors Group");
            NetsHeaderTabs.Add("Total Flavors Group");
            NetsHeaderTabs.Add("Regular Ginger Ale Flavor Group");
            NetsHeaderTabs.Add("Total Ginger Ale Flavor Group");
            NetsHeaderTabs.Add("SSD Store Brand Net");
            NetsHeaderTabs.Add("SSD Other Brand Net");
            NetsHeaderTabs.Add("Seagrams TM");
            NetsHeaderTabs.Add("Canada Dry TM");
            NetsHeaderTabs.Add("Schweppes TM");
            NetsHeaderTabs.Add("Starbucks Brand Group");
            NetsHeaderTabs.Add("TCCC RTD Coffee Net");
            NetsHeaderTabs.Add("Lipton Brand Group");
            NetsHeaderTabs.Add("TCCC RTD Tea Net");
            NetsHeaderTabs.Add("Minute Maid 100% OJ Brand Group");
            NetsHeaderTabs.Add("Minute Maid Juice TM");
            NetsHeaderTabs.Add("Minute Maid Pure Squeezed");
            NetsHeaderTabs.Add("Simply Juice TM");
            NetsHeaderTabs.Add("Simply Trademark Variety Juice & Drinks");
            NetsHeaderTabs.Add("Odwalla Juice Brand Group");
            NetsHeaderTabs.Add("Odwalla Juice/Smoothies TM");
            NetsHeaderTabs.Add("Naked Juice Brand Group");
            NetsHeaderTabs.Add("Naked Juice/Smoothies TM");
            NetsHeaderTabs.Add("Bolthouse Farms Juice/Smoothies TM");
            NetsHeaderTabs.Add("Dole Juice Brand Group");
            NetsHeaderTabs.Add("Dole Juice/Smoothies TM");
            NetsHeaderTabs.Add("Tropicana 100% OJ Brand Group");
            NetsHeaderTabs.Add("Tropicana 100% Juice Non-OJ Brand Group");
            NetsHeaderTabs.Add("Trop 50 TM");
            NetsHeaderTabs.Add("Tropicana Juice TM");
            NetsHeaderTabs.Add("Florida's Natural Juice TM");
            NetsHeaderTabs.Add("Total Citurs World 100% OJ");
            NetsHeaderTabs.Add("Apple & Eve Juice TM");
            NetsHeaderTabs.Add("Mott's Juice TM");
            NetsHeaderTabs.Add("Ocean Spray Juice TM");
            NetsHeaderTabs.Add("Old Orchard TM");
            NetsHeaderTabs.Add("Northland TM");
            NetsHeaderTabs.Add("Hansen's TM");
            NetsHeaderTabs.Add("Treetop TM");
            NetsHeaderTabs.Add("Langer's TM");
            NetsHeaderTabs.Add("V8 Juice TM");
            NetsHeaderTabs.Add("Capri Sun Juice TM");
            NetsHeaderTabs.Add("POM TM");
            NetsHeaderTabs.Add("Total NHB");
            NetsHeaderTabs.Add("100% OJ without NHB");
            NetsHeaderTabs.Add("100% Fruit Juice Blends without NHB");
            NetsHeaderTabs.Add("Other Flavor 100% Juice without NHB");
            NetsHeaderTabs.Add("TCCC Juice Net");
            NetsHeaderTabs.Add("TCCC Juice/Smoothies Net");
            NetsHeaderTabs.Add("PBNA Juice Net");
            NetsHeaderTabs.Add("PBNA Juice/Smoothies Net");
            NetsHeaderTabs.Add("Kool Aid TM");
            NetsHeaderTabs.Add("Hawaiian Punch TM");
            NetsHeaderTabs.Add("Wylers Flavor Enhancers Brand Group");
            NetsHeaderTabs.Add("Turkey Hill TM");
            NetsHeaderTabs.Add("Arizona TM");
            NetsHeaderTabs.Add("Snapple TM");
            NetsHeaderTabs.Add("Fuze TM");
            NetsHeaderTabs.Add("Honest TM");
            NetsHeaderTabs.Add("Sobe TM");
            NetsHeaderTabs.Add("Welch's TM");
            NetsHeaderTabs.Add("Total Juicy Juice TM");
            NetsHeaderTabs.Add("Total Store Brands");
            NetsHeaderTabs.Add("Nestle Plain Water Net");
            NetsHeaderTabs.Add("Nestle Plain Sparkling Water Net");
            NetsHeaderTabs.Add("Nestle Flavored Sparkling Water Net");
            NetsHeaderTabs.Add("Nestle Water TM");
            NetsHeaderTabs.Add("Arrowhead Water TM");
            NetsHeaderTabs.Add("Arrowhead Sparkling Water Brand Group");
            NetsHeaderTabs.Add("Polland Spring Water TM");
            NetsHeaderTabs.Add("Polland Spring Sparkling Water Brand Group");
            NetsHeaderTabs.Add("Perrier Spring Water TM");
            NetsHeaderTabs.Add("San Pellegrino Water TM");
            NetsHeaderTabs.Add("Nestle Pure Life Water TM");
            NetsHeaderTabs.Add("Aquafina Water TM");
            NetsHeaderTabs.Add("Crystal GeyserWater Sparkling Water TM");
            NetsHeaderTabs.Add("Sobe Flavored Water Brand Group");
            NetsHeaderTabs.Add("vitaminwater Brand Group");
            NetsHeaderTabs.Add("Glaceau TM");
            NetsHeaderTabs.Add("TCCC Plain Water Net");
            NetsHeaderTabs.Add("TCCC Water Net");
            NetsHeaderTabs.Add("PBNA Water Net");
            NetsHeaderTabs.Add("Dasani TM");
            NetsHeaderTabs.Add("Deer Park Water Net");
            NetsHeaderTabs.Add("Adirondack Water Net");
            NetsHeaderTabs.Add("Seagrams Sparkling Water Brand Group");
            NetsHeaderTabs.Add("Canada Dry Sparkling Water Brand Group");
            NetsHeaderTabs.Add("Schweppes Sparkling Water Brand Group");
            NetsHeaderTabs.Add("Gatorade TM");
            NetsHeaderTabs.Add("Powerade TM");
            NetsHeaderTabs.Add("Red Bull TM");
            NetsHeaderTabs.Add("Monster TM");
            NetsHeaderTabs.Add("Rockstar TM");
            NetsHeaderTabs.Add("Full Throttle TM");
            NetsHeaderTabs.Add("TCCC Stills Net");
            NetsHeaderTabs.Add("TCCC Sparkling Net");
            NetsHeaderTabs.Add("TCCC Net");
            NetsHeaderTabs.Add("PBNA Sparkling Net");
            NetsHeaderTabs.Add("PBNA Stills Net");
            NetsHeaderTabs.Add("PBNA Net");
            NetsHeaderTabs.Add("TCCC Low/No Calorie Net");
            NetsHeaderTabs.Add("TCCC Low/No Calorie Stills Net");
            NetsHeaderTabs.Add("PBNA Low/No Calorie Net");
            NetsHeaderTabs.Add("PBNA Low/No Calorie Stills Net");
            NetsHeaderTabs.Add("PBNA Calorie Net");
            NetsHeaderTabs.Add("TCCC Calorie Net");
            //Category Nets
            NetsHeaderTabs.Add("Carbonated Soft Drinks");
            NetsHeaderTabs.Add("Coffee");
            NetsHeaderTabs.Add("Fresh Brewed Coffee");
            NetsHeaderTabs.Add("Hot Chocolate/Cocoa");
            NetsHeaderTabs.Add("Coffee or Cocoa");
            NetsHeaderTabs.Add("Tea");
            NetsHeaderTabs.Add("Fresh Brewed Tea");
            NetsHeaderTabs.Add("Dairy/ Dairy Alternatives");
            NetsHeaderTabs.Add("Protein Drinks, Yogurt Drinks, Meal Replacements, and Smoothies");
            NetsHeaderTabs.Add("Smoothies");
            NetsHeaderTabs.Add("Protein Drinks/Meal Replacements");
            NetsHeaderTabs.Add("Juice");
            NetsHeaderTabs.Add("100% Juice");
            NetsHeaderTabs.Add("RTD Juice");
            NetsHeaderTabs.Add("RTD 100%Juice");
            NetsHeaderTabs.Add("Powdered Soft Drinks");
            NetsHeaderTabs.Add("Flavor Additives");
            NetsHeaderTabs.Add("Unflavored  Packaged Water");
            NetsHeaderTabs.Add("SS Unflavored  Packaged Water");
            NetsHeaderTabs.Add("Flavored Packaged Water");
            NetsHeaderTabs.Add("SS Sparkling Water");
            NetsHeaderTabs.Add("SS Non-Sparkling Packaged Water");
            NetsHeaderTabs.Add("Total Packaged Water");
            NetsHeaderTabs.Add("Total SS Packaged Water");
            NetsHeaderTabs.Add("Tap Water");
            NetsHeaderTabs.Add("Flavored Water/ Sports Drinks");
            NetsHeaderTabs.Add("Total Non-Alcoholic RTD Beverages");
            NetsHeaderTabs.Add("Total Non-Alcoholic Non-RTD Beverages");
            NetsHeaderTabs.Add("Alcohol");
        }
        public void CreateTableMappingNames()
        {
            TableMappingList = new Dictionary<string, string>();
            TableMappingList.Clear();
            TableMappingList.Add("Convenience", "A convenience store or gas station food mart (excluding gas)");
            TableMappingList.Add("Dollar", "A dollar store such as Family Dollar or Dollar General");
            TableMappingList.Add("SupermarketGrocery", "A supermarket or grocery store");
            TableMappingList.Add("MassMerc", "A mass merchandise store without a full-line grocery section such as Walmart or Target");
            TableMappingList.Add("Drug", "A drug store");
            TableMappingList.Add("Club", "A warehouse club such as Sam`s Club or Costco");
            TableMappingList.Add("Supercenter", "A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget");
            TableMappingList.Add("Oct 2013 3MMT", "Oct 2013");
            TableMappingList.Add("Nov 2013 3MMT", "Nov 2013");
            TableMappingList.Add("Dec 2013 3MMT", "Dec 2013");
            TableMappingList.Add("Jan 2014 3MMT", "Jan 2014");
            TableMappingList.Add("Feb 2014 3MMT", "Feb 2014");
            TableMappingList.Add("Mar 2014 3MMT", "Mar 2014");
            TableMappingList.Add("Apr 2014 3MMT", "Apr 2014");
            TableMappingList.Add("May 2014 3MMT", "May 2014");
            TableMappingList.Add("Jun 2014 3MMT", "Jun 2014");
            TableMappingList.Add("Jul 2014 3MMT", "Jul 2014");
            TableMappingList.Add("Aug 2014 3MMT", "Aug 2014");
            TableMappingList.Add("Sep 2014 3MMT", "Sep 2014");

            TableMappingList.Add("Jan 2015 3MMT", "Jan 2015");
            TableMappingList.Add("Feb 2015 3MMT", "Feb 2015");
            TableMappingList.Add("Mar 2015 3MMT", "Mar 2015");

            TableMappingList.Add("Jul 2016 3MMT", "Jul 2016");
            TableMappingList.Add("Aug 2016 3MMT", "Aug 2016");
            TableMappingList.Add("Sep 2016 3MMT", "Sep 2016");

            TableMappingList.Add("White Collar", "A white-collar occupation");
            TableMappingList.Add("Blue Collar", "A blue-collar occupation");
            //TableMappingList.Add("Other Employed", "Other");
            TableMappingList.Add("Total Trips", "Total");
            TableMappingList.Add("Total Shopper", "Total");
            TableMappingList.Add("Total Beverage Trips", "Total");
            TableMappingList.Add("Total Beverage Shopper", "Total");

            //added by Nagaraju on 30-12-2014
            TableMappingList.Add("Total Trips ", "TotalTrip");
            TableMappingList.Add("Total Shopper ", "TotalShopper");

            //added by Bramhanath for Beverage on 16-10-2015
            TableMappingList.Add("Reg SSD", "Regular Carbonated Soft Drinks");//TableMappingList.Add("CSD Reg", "Regular Carbonated Soft Drinks");
            TableMappingList.Add("Diet SSD", "Diet Carbonated Soft Drinks");//TableMappingList.Add("CSD Diet", "Diet Carbonated Soft Drinks");
            //TableMappingList.Add("RTD Coffee", "RTD Coffee");
            //TableMappingList.Add("RTD Tea", "RTD Tea");
            TableMappingList.Add("RTD Coffee", "Coffee in a Bottle, Can, or Carton");
            TableMappingList.Add("RTD Tea", "Tea in a Bottle, Can, Carton or Fountain");

            TableMappingList.Add("RTD Smoothies", "RTD Smoothies");
            TableMappingList.Add(" RTD Smoothies", "Ready-to-Drink Smoothies in a Bottle");

            TableMappingList.Add("RTD Juice Drinks/Ades", "Ready-to-Drink Juice Drinks/Ade (&lt;100% Juice)");
            TableMappingList.Add("100% Non Orange Juice", "100% Non Orange Juice");

            //added by Nagaraju 2016-04-29
            TableMappingList.Add(" 100% Non Orange Juice", "100% juice (not orange)");

            TableMappingList.Add("Packaged Water", "Total Packaged Water");
            TableMappingList.Add("Sports  Drinks", "Sport Drinks");
            TableMappingList.Add("Unflavored Packaged Water", "Unflavored Packaged Water");
            TableMappingList.Add("Sparkling Packaged Water", "SS Sparkling Water");
            TableMappingList.Add("Non-Sparkling Packaged Water", "SS Non-Sparkling Packaged Water");
            TableMappingList.Add("Unflavored Sparkling Packaged Water", "Unflavored Sparkling Water");
            TableMappingList.Add("Unflavored Non Sparkling Packaged Water", "Unflavored Bottled Water (Non-Sparkling)");
            TableMappingList.Add("Flavored Sparkling Packaged Water", "Flavored Sparkling Water");

            TableMappingList.Add("Flavored Non-Sparkling Packaged Water", "Flavored Non-Sparkling Water");

            TableMappingList.Add("Mountain Dew", "Mountain Dew (Any brand or flavor)");
            TableMappingList.Add("Mountain  Dew", "Mtn Dew");
            TableMappingList.Add("Sierra Mist/Sierra Mist Natural", "Sierra Mist/Sierra Mist Natural (Any flavor)");
            TableMappingList.Add("Crush", "Crush (Any flavor)");
            TableMappingList.Add("Fanta", "Fanta (Any flavor)");
            TableMappingList.Add("Faygo", "Faygo (Any flavor)");
            TableMappingList.Add("Squirt", "Squirt (Any flavor)");
            TableMappingList.Add("Sunkist", "Sunkist (Any flavor)");

            TableMappingList.Add("Diet Mountain Dew", "Diet Mountain Dew (Any brand or flavor)");
            TableMappingList.Add("Diet Sunkist", "Diet Sunkist (Any flavor)");
            TableMappingList.Add("Fresca", "Fresca (Any flavor)");
            //TableMappingList.Add("Store Brand", "Store Brand(RTD COFFEE)");
            //TableMappingList.Add("Store Brand", "Store Brand(RTD TEA)");
            //TableMappingList.Add("Store Brand", "Store Brand(100% Juice (not Orange))");
            TableMappingList.Add("Honest/Honest Kids", "Honest/Honest Kids (Juices and Ades)");
            TableMappingList.Add("Kool-Aid", "Kool-Aid (Any variety)");
            TableMappingList.Add("Canada Dry", "Canada Dry (Any Sparkling Water; Seltzer, Tonic or Club Soda)");
            TableMappingList.Add("Canada  Dry", "Canada Dry (Any Flavored Sparkling Water; Seltzer, Tonic or Club Soda)");
            TableMappingList.Add("Schweppes", "Schweppes (Any Sparkling Water; Seltzer, Tonic or Club Soda)");
            TableMappingList.Add("Seagram?s", "Seagram?s (Any Sparkling Water; Seltzer, Tonic or Club Soda)");
            TableMappingList.Add("Polar", "Polar (Any Sparkling Water, Seltzer, Tonic or Club Soda)");
            TableMappingList.Add("Vintage", "Vintage (Any Sparkling Water, Seltzer, Tonic or Club Soda)");
            TableMappingList.Add("Adirondack", "Adirondack (Any Sparkling Water; Seltzer, Tonic or Club Soda)");

            TableMappingList.Add(" Schweppes", "Schweppes (Any Flavored Sparkling Water; Seltzer, Tonic or Club Soda)");
            TableMappingList.Add(" Seagram?s", "Seagram?s (Any Flavored Sparkling Water; Seltzer, Tonic or Club Soda)");
            TableMappingList.Add(" Polar", "Polar (Any flavored Sparkling Water, Seltzer, Tonic or Club Soda)");
            TableMappingList.Add(" Vintage", "Vintage (Any flavored Sparkling Water, Seltzer, Tonic or Club Soda)");
            TableMappingList.Add(" Adirondack", "Adirondack (Any flavored Sparkling Water; Seltzer, Tonic or Club Soda)");

            TableMappingList.Add("White Rock", "White Rock (Any Sparkling Water, Seltzer, Tonic or Club Soda)");

            TableMappingList.Add("Monster", "Monster (Any brand or flavor)");
            TableMappingList.Add("Rockstar", "Rockstar (Any brand or flavor)");
            TableMappingList.Add("Reg  SSD", "CSD Regular");
            TableMappingList.Add("Diet  SSD", "CSD Diet");
            TableMappingList.Add("RTD Juice  Drinks/Ades", "Juice Drinks/Ade");

            TableMappingList.Add("Unflavored Sparkling Packaged  Water", "Bottled Water Sparkling Unflavored");
            TableMappingList.Add("Unflavored Non-Sparkling Packaged  Water", "Bottled Water Still Unflavored");
            TableMappingList.Add("Flavored Sparkling Packaged  Water", "Bottled Water Sparkling Flavored");
            TableMappingList.Add("Flavored Non-Sparkling Packaged  Water", "Bottled Water Still Flavored");

            //Trips
            //TableMappingList.Add(" RTD Smoothies", "RTD Smoothies");
            TableMappingList.Add("RTD  Coffee", "RTD Coffee");
            TableMappingList.Add("RTD  Tea", "RTD Tea");
            TableMappingList.Add(" RTD Coffee", "RTD Coffee");
            TableMappingList.Add(" RTD Tea", "RTD Tea");
            TableMappingList.Add(" CSD Reg", "CSD Regular");
            TableMappingList.Add(" CSD Diet", "CSD Diet");
            TableMappingList.Add("Diet Mountain Dew (CSD Diet)", "Diet Mtn Dew (CSD Diet)");
            TableMappingList.Add("White Rock (Bottled Water Sparkling Unflavored)", "White Rock (Any Sparkling Water, Seltzer, Tonic or Club Soda) (Bottled Water Sparkling Unflavored)");
            TableMappingList.Add("Polar (Bottled Water Sparkling Flavored)", "polar (any flavored sparkling water, seltzer, tonic or club soda) (bottled water sparkling flavored)");

            TableMappingList.Add("Vintage (Bottled Water Sparkling Flavored)", "vintage (any flavored sparkling water, seltzer, tonic or club soda) (bottled water sparkling flavored)");

            TableMappingList.Add("Adirondack (Bottled Water Sparkling Flavored)", "adirondack (any flavored sparkling water; seltzer, tonic or club soda) (bottled water sparkling flavored)");


            TableMappingList.Add("Polar (Bottled Water Sparkling Unflavored)", "polar (any sparkling water, seltzer, tonic or club soda) (bottled water sparkling unflavored)");


            TableMappingList.Add("Vintage (Bottled Water Sparkling Unflavored)", "vintage (any sparkling water, seltzer, tonic or club soda) (bottled water sparkling unflavored)");

            TableMappingList.Add("Adirondack (Bottled Water Sparkling Unflavored)", "adirondack (any sparkling water; seltzer, tonic or club soda) (bottled water sparkling unflavored)");

            //

            //

            //TableMappingList.Add("Regular Carbonated Soft Drinks", "CSD Reg");

            //added by Nagaraju for Trips on 24-05-2016
            TableMappingList.Add("Other mass merchandise store without full-line grocery section", "Other mass merchandise store without full-line grocery section (specify)");
        }

        public void CreateTableMappingNamesTotal()
        {
            TableMappingListtotal = new Dictionary<string, string>();
            TableMappingListtotal.Clear();
            TableMappingListtotal.Add("Convenience", "A convenience store or gas station food mart (excluding gas)");
            TableMappingListtotal.Add("Dollar", "A dollar store such as Family Dollar or Dollar General");
            TableMappingListtotal.Add("SupermarketGrocery", "A supermarket or grocery store");
            TableMappingListtotal.Add("MassMerc", "A mass merchandise store without a full-line grocery section such as Walmart or Target");
            TableMappingListtotal.Add("Drug", "A drug store");
            TableMappingListtotal.Add("Club", "A warehouse club such as Sam`s Club or Costco");
            TableMappingListtotal.Add("Supercenter", "A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget");
            TableMappingListtotal.Add("Oct 2013 3MMT", "3MMT Oct 2013");
            TableMappingListtotal.Add("Nov 2013 3MMT", "3MMT Nov 2013");
            TableMappingListtotal.Add("Dec 2013 3MMT", "3MMT Dec 2013");
            TableMappingListtotal.Add("Jan 2014 3MMT", "3MMT Jan 2014");
            TableMappingListtotal.Add("Feb 2014 3MMT", "3MMT Feb 2014");
            TableMappingListtotal.Add("Mar 2014 3MMT", "3MMT Mar 2014");
            TableMappingListtotal.Add("Apr 2014 3MMT", "3MMT Apr 2014");
            TableMappingListtotal.Add("May 2014 3MMT", "3MMT May 2014");
            TableMappingListtotal.Add("Jun 2014 3MMT", "3MMT Jun 2014");
            TableMappingListtotal.Add("Jul 2014 3MMT", "3MMT Jul 2014");
            TableMappingListtotal.Add("Aug 2014 3MMT", "3MMT Aug 2014");
            TableMappingListtotal.Add("Sep 2014 3MMT", "3MMT Sep 2014");

            TableMappingListtotal.Add("Oct 2014 3MMT", "3MMT Oct 2014");
            TableMappingListtotal.Add("Nov 2014 3MMT", "3MMT Nov 2014");
            TableMappingListtotal.Add("Dec 2014 3MMT", "3MMT Dec 2014");

            TableMappingListtotal.Add("Jan 2015 3MMT", "3MMT Jan 2015");
            TableMappingListtotal.Add("Feb 2015 3MMT", "3MMT Feb 2015");
            TableMappingListtotal.Add("Mar 2015 3MMT", "3MMT Mar 2015");
            TableMappingListtotal.Add("Apr 2015 3MMT", "3MMT Apr 2015");
            TableMappingListtotal.Add("May 2015 3MMT", "3MMT May 2015");
            TableMappingListtotal.Add("Jun 2015 3MMT", "3MMT Jun 2015");
            TableMappingListtotal.Add("Jul 2015 3MMT", "3MMT Jul 2015");
            TableMappingListtotal.Add("Aug 2015 3MMT", "3MMT Aug 2015");
            TableMappingListtotal.Add("Sep 2015 3MMT", "3MMT Sep 2015");

            TableMappingListtotal.Add("Oct 2015 3MMT", "3MMT Oct 2015");
            TableMappingListtotal.Add("Nov 2015 3MMT", "3MMT Nov 2015");
            TableMappingListtotal.Add("Dec 2015 3MMT", "3MMT Dec 2015");

            TableMappingListtotal.Add("Jan 2016 3MMT", "3MMT Jan 2016");
            TableMappingListtotal.Add("Feb 2016 3MMT", "3MMT Feb 2016");
            TableMappingListtotal.Add("Mar 2016 3MMT", "3MMT Mar 2016");

            TableMappingListtotal.Add("Apr 2016 3MMT", "3MMT Apr 2016");
            TableMappingListtotal.Add("May 2016 3MMT", "3MMT May 2016");
            TableMappingListtotal.Add("Jun 2016 3MMT", "3MMT Jun 2016");

            TableMappingListtotal.Add("Jul 2016 3MMT", "3MMT Jul 2016");
            TableMappingListtotal.Add("Aug 2016 3MMT", "3MMT Aug 2016");
            TableMappingListtotal.Add("Sep 2016 3MMT", "3MMT Sep 2016");

            TableMappingListtotal.Add("Q3 2013", "Quarter Q3 2013");
            TableMappingListtotal.Add("Q4 2013", "Quarter Q4 2013");
            TableMappingListtotal.Add("Q1 2014", "Quarter Q1 2014");
            TableMappingListtotal.Add("Q2 2014", "Quarter Q2 2014");
            TableMappingListtotal.Add("Q3 2014", "Quarter Q3 2014");
            TableMappingListtotal.Add("Q4 2014", "Quarter Q4 2014");

            TableMappingListtotal.Add("Q1 2015", "Quarter Q1 2015");
            TableMappingListtotal.Add("Q2 2015", "Quarter Q2 2015");
            TableMappingListtotal.Add("Q3 2015", "Quarter Q3 2015");

            TableMappingListtotal.Add("Q4 2015", "Quarter Q4 2015");

            TableMappingListtotal.Add("Q1 2016", "Quarter Q1 2016");

            TableMappingListtotal.Add("Q2 2016", "Quarter Q2 2016");

            TableMappingListtotal.Add("Q3 2016", "Quarter Q3 2016");

            TableMappingListtotal.Add("Total Time", "Total");
            TableMappingListtotal.Add("White Collar", "A white-collar occupation");
            TableMappingListtotal.Add("Blue Collar", "A blue-collar occupation");
            //TableMappingListtotal.Add("Other Employed", "Other");


            TableMappingListtotal.Add("Jul 2014 12MMT", "12MMT Jul 2014");
            TableMappingListtotal.Add("Aug 2014 12MMT", "12MMT Aug 2014");
            TableMappingListtotal.Add("Sep 2014 12MMT", "12MMT Sep 2014");
            TableMappingListtotal.Add("Oct 2014 12MMT", "12MMT Oct 2014");
            TableMappingListtotal.Add("Nov 2014 12MMT", "12MMT Nov 2014");
            TableMappingListtotal.Add("Dec 2014 12MMT", "12MMT Dec 2014");
            TableMappingListtotal.Add("Jan 2015 12MMT", "12MMT Jan 2015");
            TableMappingListtotal.Add("Feb 2015 12MMT", "12MMT Feb 2015");
            TableMappingListtotal.Add("Mar 2015 12MMT", "12MMT Mar 2015");
            TableMappingListtotal.Add("Apr 2015 12MMT", "12MMT Apr 2015");
            TableMappingListtotal.Add("May 2015 12MMT", "12MMT May 2015");
            TableMappingListtotal.Add("Jun 2015 12MMT", "12MMT Jun 2015");
            TableMappingListtotal.Add("Jul 2015 12MMT", "12MMT Jul 2015");
            TableMappingListtotal.Add("Aug 2015 12MMT", "12MMT Aug 2015");
            TableMappingListtotal.Add("Sep 2015 12MMT", "12MMT Sep 2015");

            TableMappingListtotal.Add("Oct 2015 12MMT", "12MMT Oct 2015");
            TableMappingListtotal.Add("Nov 2015 12MMT", "12MMT Nov 2015");
            TableMappingListtotal.Add("Dec 2015 12MMT", "12MMT Dec 2015");

            TableMappingListtotal.Add("Jan 2016 12MMT", "12MMT Jan 2016");
            TableMappingListtotal.Add("Feb 2016 12MMT", "12MMT Feb 2016");
            TableMappingListtotal.Add("Mar 2016 12MMT", "12MMT Mar 2016");

            TableMappingListtotal.Add("Apr 2016 12MMT", "12MMT Apr 2016");
            TableMappingListtotal.Add("May 2016 12MMT", "12MMT May 2016");
            TableMappingListtotal.Add("Jun 2016 12MMT", "12MMT Jun 2016");

            TableMappingListtotal.Add("Jul 2016 12MMT", "12MMT Jul 2016");
            TableMappingListtotal.Add("Aug 2016 12MMT", "12MMT Aug 2016");
            TableMappingListtotal.Add("Sep 2016 12MMT", "12MMT Sep 2016");

            TableMappingListtotal.Add("YTD Jan 2014", "YTD YTD Jan 2014");
            TableMappingListtotal.Add("YTD Feb 2014", "YTD YTD Feb 2014");
            TableMappingListtotal.Add("YTD Mar 2014", "YTD YTD Mar 2014");
            TableMappingListtotal.Add("YTD Apr 2014", "YTD YTD Apr 2014");
            TableMappingListtotal.Add("YTD May 2014", "YTD YTD May 2014");
            TableMappingListtotal.Add("YTD Jun 2014", "YTD YTD Jun 2014");
            TableMappingListtotal.Add("YTD Jul 2014", "YTD YTD Jul 2014");
            TableMappingListtotal.Add("YTD Aug 2014", "YTD YTD Aug 2014");
            TableMappingListtotal.Add("YTD Sep 2014", "YTD YTD Sep 2014");
            TableMappingListtotal.Add("YTD Oct 2014", "YTD YTD Oct 2014");
            TableMappingListtotal.Add("YTD Nov 2014", "YTD YTD Nov 2014");
            TableMappingListtotal.Add("YTD Dec 2014", "YTD YTD Dec 2014");
            TableMappingListtotal.Add("YTD Jan 2015", "YTD YTD Jan 2015");
            TableMappingListtotal.Add("YTD Feb 2015", "YTD YTD Feb 2015");
            TableMappingListtotal.Add("YTD Mar 2015", "YTD YTD Mar 2015");
            TableMappingListtotal.Add("YTD Apr 2015", "YTD YTD Apr 2015");
            TableMappingListtotal.Add("YTD May 2015", "YTD YTD May 2015");
            TableMappingListtotal.Add("YTD Jun 2015", "YTD YTD Jun 2015");
            TableMappingListtotal.Add("YTD Jul 2015", "YTD YTD Jul 2015");
            TableMappingListtotal.Add("YTD Aug 2015", "YTD YTD Aug 2015");
            TableMappingListtotal.Add("YTD Sep 2015", "YTD YTD Sep 2015");

            TableMappingListtotal.Add("YTD Oct 2015", "YTD YTD Oct 2015");
            TableMappingListtotal.Add("YTD Nov 2015", "YTD YTD Nov 2015");
            TableMappingListtotal.Add("YTD Dec 2015", "YTD YTD Dec 2015");

            TableMappingListtotal.Add("YTD Jan 2016", "YTD YTD Jan 2016");
            TableMappingListtotal.Add("YTD Feb 2016", "YTD YTD Feb 2016");
            TableMappingListtotal.Add("YTD Mar 2016", "YTD YTD Mar 2016");

            TableMappingListtotal.Add("YTD Apr 2016", "YTD YTD Apr 2016");
            TableMappingListtotal.Add("YTD May 2016", "YTD YTD May 2016");
            TableMappingListtotal.Add("YTD Jun 2016", "YTD YTD Jun 2016");

            TableMappingListtotal.Add("YTD Jul 2016", "YTD YTD Jul 2016");
            TableMappingListtotal.Add("YTD Aug 2016", "YTD YTD Aug 2016");
            TableMappingListtotal.Add("YTD Sep 2016", "YTD YTD Sep 2016");

            //6MMT added by Bramhanath(01-11-2015)

            TableMappingListtotal.Add("Jan 2014 6MMT", "6MMT Jan 2014");
            TableMappingListtotal.Add("Feb 2014 6MMT", "6MMT Feb 2014");
            TableMappingListtotal.Add("Mar 2014 6MMT", "6MMT Mar 2014");
            TableMappingListtotal.Add("Apr 2014 6MMT", "6MMT Apr 2014");
            TableMappingListtotal.Add("May 2014 6MMT", "6MMT May 2014");
            TableMappingListtotal.Add("Jun 2014 6MMT", "6MMT Jun 2014");
            TableMappingListtotal.Add("Jul 2014 6MMT", "6MMT Jul 2014");
            TableMappingListtotal.Add("Aug 2014 6MMT", "6MMT Aug 2014");
            TableMappingListtotal.Add("Sep 2014 6MMT", "6MMT Sep 2014");

            TableMappingListtotal.Add("Oct 2014 6MMT", "6MMT Oct 2014");
            TableMappingListtotal.Add("Nov 2014 6MMT", "6MMT Nov 2014");
            TableMappingListtotal.Add("Dec 2014 6MMT", "6MMT Dec 2014");

            TableMappingListtotal.Add("Jan 2015 6MMT", "6MMT Jan 2015");
            TableMappingListtotal.Add("Feb 2015 6MMT", "6MMT Feb 2015");
            TableMappingListtotal.Add("Mar 2015 6MMT", "6MMT Mar 2015");
            TableMappingListtotal.Add("Apr 2015 6MMT", "6MMT Apr 2015");
            TableMappingListtotal.Add("May 2015 6MMT", "6MMT May 2015");
            TableMappingListtotal.Add("Jun 2015 6MMT", "6MMT Jun 2015");
            TableMappingListtotal.Add("Jul 2015 6MMT", "6MMT Jul 2015");
            TableMappingListtotal.Add("Aug 2015 6MMT", "6MMT Aug 2015");
            TableMappingListtotal.Add("Sep 2015 6MMT", "6MMT Sep 2015");

            TableMappingListtotal.Add("Oct 2015 6MMT", "6MMT Oct 2015");
            TableMappingListtotal.Add("Nov 2015 6MMT", "6MMT Nov 2015");
            TableMappingListtotal.Add("Dec 2015 6MMT", "6MMT Dec 2015");

            TableMappingListtotal.Add("Jan 2016 6MMT", "6MMT Jan 2016");
            TableMappingListtotal.Add("Feb 2016 6MMT", "6MMT Feb 2016");
            TableMappingListtotal.Add("Mar 2016 6MMT", "6MMT Mar 2016");

            TableMappingListtotal.Add("Apr 2016 6MMT", "6MMT Apr 2016");
            TableMappingListtotal.Add("May 2016 6MMT", "6MMT May 2016");
            TableMappingListtotal.Add("Jun 2016 6MMT", "6MMT Jun 2016");

            TableMappingListtotal.Add("Jul 2016 6MMT", "6MMT Jul 2016");
            TableMappingListtotal.Add("Aug 2016 6MMT", "6MMT Aug 2016");
            TableMappingListtotal.Add("Sep 2016 6MMT", "6MMT Sep 2016");

            //

        }
        public string GetDBMappingName(string value)
        {
            string DBMappingName = HeaderTabs.FirstOrDefault(x => x.Value.Trim().ToLower() == value.Trim().ToLower()).Key;
            if (!string.IsNullOrEmpty(DBMappingName))
                value = Convert.ToString(DBMappingName);

            return value;
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
            HeaderTabs.Add("VisitPreparation", "Planning Types");//changes reg bugid-2742
            HeaderTabs.Add("TechnologyUsed", "Use of Technology to Prepare");
            HeaderTabs.Add("ComputerBased", "Computer-Based Preparation Activities");
            HeaderTabs.Add("SmartPhoneBased", "Smartphone-Based Preparation Activities");
            HeaderTabs.Add("DestinationItemSummary", "Destination Item Summary");
            HeaderTabs.Add("DestinationItemDetails", "Destination Item Detail");

            HeaderTabs.Add("A supermarket or grocery store", "Grocery");
            HeaderTabs.Add("A convenience store or gas station food mart (excluding gas)", "Convenience");
            HeaderTabs.Add("A drug store", "Drug");
            HeaderTabs.Add("A dollar store such as Family Dollar or Dollar General", "Dollar");
            HeaderTabs.Add("A warehouse club such as Sam`s Club or Costco", "Club");
            HeaderTabs.Add("A Mass Merchandise store or super center such as walmart, target, walmart supercenter, or supertarget", "Mass Merc. with Supers");
            HeaderTabs.Add("A mass merchandise store without a full-line grocery section such as Walmart or Target", "MassMerc");
            HeaderTabs.Add("A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget", "Supercenter");

            HeaderTabs.Add("Shopper Attitude", "Top 2 Box Attitudinal Statements");
            HeaderTabs.Add("Attitudinal Segment", "Attitudinal Segment");

            HeaderTabs.Add("RetailerLoyaltyPyramid", "Retailer Loyalty Pyramid(Among Trade Area Shoppers)");//Retailer Loyalty Pyramid - Total Grocery Across Channel
            HeaderTabs.Add("TopBox", "Loyalty and Satisfaction Detail(Applicable only for Retailers)");
            HeaderTabs.Add("MainFavoriteStore", "Main/Favorite Store (Applicable only for Retailers/Retailer nets)");

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
            HeaderTabs.Add("Smartphone/TabletInfluencedPurchases", "Smartphone/Tablet Usage in Store");

            HeaderTabs.Add("SmartPhoneUsage", "Ways Technology Used to Influence");
            HeaderTabs.Add("BeverageBrandsPurchased", "Beverage Brands Purchased: SSD");

            HeaderTabs.Add("ItemsPurchasedSummary", "Trips with a Food or Beverage Purchase");
            HeaderTabs.Add("InStoreDestinationDetails", "Items Purchased Detail");
            HeaderTabs.Add("ImpulseItem", "Impulse Item");
            HeaderTabs.Add("TripMission", "Trip Mission");

            HeaderTabs.Add("WayTabletInfluenced", "Way Tablet Influenced");
            HeaderTabs.Add("WaySmartphoneInfluenced", "Way Smartphone Influenced");

            HeaderTabs.Add("TimeSpent", "Time Spent in Store");
            HeaderTabs.Add("TripExpenditure", "Trip Expenditure");
            HeaderTabs.Add("CheckOutType", "Checkout Method");
            HeaderTabs.Add("ConsideredStoreVisits", "Considered Store Visits");
            HeaderTabs.Add("VisitMotiviations", "Primary Motivation for Store Selection");
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

            //Variables
            HeaderTabs.Add("Has low everyday prices", "Low everyday prices");
            HeaderTabs.Add("Has good sales", "Good sales");
            HeaderTabs.Add("Has a good loyalty program (such as shopper card, store credit card, fuel rewards)", "Good loyalty program");
            HeaderTabs.Add("Has a good coupon policy (accepts competitors` coupons, allows double coupons, etc.)", "Good coupon policy");
            HeaderTabs.Add("Has an easy, quick checkout experience", "Easy/Quick checkout");
            HeaderTabs.Add("Has helpful, courteous, knowledgeable employees", "Helpful employees");
            HeaderTabs.Add("Has good customer service policies (returns, exchanges, satisfaction guarantees, etc.)", "Good customer service policies");
            HeaderTabs.Add("Has a friendly, comfortable atmosphere", "Friendly atmosphere");
            HeaderTabs.Add("Has product demonstrations or offers product samples", "Product demonstrations available");
            HeaderTabs.Add("Has a clean shopping area", "Clean shopping area");
            HeaderTabs.Add("Is a fun place to shop", "Fun place to shop");
            HeaderTabs.Add("Has the items I frequently buy in a convenient location in the store", "Items are placed in a convenient location");
            HeaderTabs.Add("Has clean restrooms", "Clean restrooms");
            HeaderTabs.Add("Is well-organized and easy to shop", "Well-organized and easy to shop");
            HeaderTabs.Add("Is conveniently located", "Conveniently located");
            HeaderTabs.Add("Is easy to get in and out of quickly", "Easy in get in /out quickly");
            HeaderTabs.Add("Has a familiar layout", "Familiar layout");
            HeaderTabs.Add("Has good, safe parking facilities", "Safe parking facilities");
            HeaderTabs.Add("Has convenient hours of operation", "Convenient hours of operation");
            HeaderTabs.Add("Offers one-stop shopping for the items I need", "One-stop shopping for needy items");
            HeaderTabs.Add("Has good fuel prices", "Good fuel prices");
            HeaderTabs.Add("Provides a good overall value", "Good overall value");
            HeaderTabs.Add("Has a good selection", "Good selection");
            HeaderTabs.Add("Offers unique and appealing products", "Unique and appealing products");
            HeaderTabs.Add("Carries high-quality products", "High-quality products");
            HeaderTabs.Add("Store brand/private label products", "Store brand");
            HeaderTabs.Add("Other services (bank, video rental, event tickets, etc.)", "Other services");
            HeaderTabs.Add("Ready-to-eat or heat foods such as rotisserie chicken, prepared entrees, or side dishes", "Ready-to-eat or heat foods");
            HeaderTabs.Add("Non-grocery products such as clothing, housewares, toys, greeting cards, etc.", "Non-grocery products");
            HeaderTabs.Add("Cosmetics and personal care such as razors, shampoo, etc.", "Cosmetics and personal care");
            HeaderTabs.Add("I`m responsible for the majority of grocery shopping for my household", "Responsible for grocery shopping");
            HeaderTabs.Add("I`m responsible for most of the meal preparation in my household", "Responsible for meal preparation");
            HeaderTabs.Add("I often use my computer to help me plan and prepare for my grocery shopping trip", "Use computer to plan grocery shopping trip");
            HeaderTabs.Add("I usually check the grocery store website for specials", "Check grocery store website for specials");
            HeaderTabs.Add("I use social media to report or influence my grocery shopping experiences (such as blog, tweet, etc.)", "Use social media to influence my grocery shopping experiences");
            HeaderTabs.Add("I like to buy grocery items at a store where I can hang out with friends", "like to buy grocery items where I can hang out with friends");
            HeaderTabs.Add("I often look for grocery coupons and deals on the internet before shopping in stores", "Look for grocery coupons and deals on the interet");
            HeaderTabs.Add("I often compare prices between stores", "Compare prices between stores");
            HeaderTabs.Add("I choose what I purchase based on price, coupons, and discounts", "Purchase based on final price");
            HeaderTabs.Add("I have a fixed budget for grocery shopping", "Have fixed budget for grocery shopping");
            HeaderTabs.Add("I wait until payday to stock up on grocery items", "Wait until payday to stock up on grocery items");
            HeaderTabs.Add("I frequently go to more than one store to get the best deal or price", "Go to more stores to get best price");
            HeaderTabs.Add("I frequently use SNAP or WIC to help pay for items", "Use SNAP or WIC to help payment");
            HeaderTabs.Add("I prefer to buy organic or natural foods", "Buy organic/natural foods");
            HeaderTabs.Add("I intend to purchase healthy products but end up purchasing products that are not so healthy for me and my family", "Purchase prroducts that are not so healthy");
            HeaderTabs.Add("I prefer to buy high-quality grocery products", "Buy high-quality grocery products");
            HeaderTabs.Add("I prefer to go to specialty shops such as the butcher, green grocer/farmer`s market, bakery, etc. for fresh foods", "Go to specialty shops");
            HeaderTabs.Add("I am willing to pay more for products from companies who give back to the community", "Pay more for products from social resposible companies");
            HeaderTabs.Add("When I go to the store for groceries, I just want to get in and out fast", "Want to get in and out fast");
            HeaderTabs.Add("I`m willing to pay more for grocery items that save me time", "Pay more for grocery items that save time");
            HeaderTabs.Add("I often purchase products that are ready made and ready to eat", "Purchase ready to eat");
            HeaderTabs.Add("I have started to purchase some grocery items through auto-replenishment programs", "Purchase through auto-replenishment programs");
            HeaderTabs.Add("I look for stores to provide ideas for meals and snack solutions", "Look for stores to provide ideas for meals");
            HeaderTabs.Add("I prefer stores that have a wide product selection", "Prefer stores with wide product selection");
            HeaderTabs.Add("I like to browse for new and interesting grocery items when I shop", "Look for new grocery items");
            HeaderTabs.Add("I wanted to get in and out as quickly as possible", "Get in and out as quickly as possible");
            HeaderTabs.Add("I wanted to have a fun, enjoyable shopping experience", "Have an enjoyable shopping experience");
            HeaderTabs.Add("I wanted to get the best overall value possible", "Get the best overall value possible");
            HeaderTabs.Add("I wanted to purchase items for a special occasion, holiday or party", "Purchase items for a special occasion");
            HeaderTabs.Add("I wanted to use the pharmacy", "Use the pharmacy");
            HeaderTabs.Add("I wanted to take advantage of a sale or promotion I was aware of", "Take advantage of a sale I was aware of");
            HeaderTabs.Add("I wanted to buy items that aren`t readily available elsewhere", "Buy items that aren’t readily available elsewhere");
            HeaderTabs.Add("I wanted to purchase items for my business", "purchase items for my business");
            HeaderTabs.Add("I wanted to save as much money as possible", "Save as much money as possible");
            HeaderTabs.Add("I wanted to look for bargains or deals", "Look for bargains or deals");
            HeaderTabs.Add("I wanted to pick up a prepared meal to eat when I got home", "Pick up a prepared meal to eat when I got home");
            HeaderTabs.Add("I wanted to get a meal/snack to eat right away", "Get a meal/snack to eat right away");
            HeaderTabs.Add("I wanted to get a beverage to drink right away", "Get a beverage to drink right away");
            HeaderTabs.Add("I wanted to take some time to browse around for interesting items", "Take some time to browse around for interesting items");
            HeaderTabs.Add("I wanted to go to the closest store that had what I needed", "Go to the closest store that had what I needed");
            HeaderTabs.Add("I wanted to purchase fuel for my car", "Purchase fuel for my car");
            HeaderTabs.Add("I wanted a place where I could easily find the items I needed", "I could easily find the items I needed");
            HeaderTabs.Add("I wanted to take advantage of the store`s loyalty program", "Take advantage of the store's loyalty program");
            HeaderTabs.Add("I wanted to be able to purchase all the items I needed in one trip", "To purchase all the items I needed in one trip");
            HeaderTabs.Add("I wanted a place with a clean restroom", "Place with a clean restroom");
            HeaderTabs.Add("I wanted to take advantage of a good coupon policy", "Take advantage of a good coupon policy");
            HeaderTabs.Add("I wanted high-quality products", "Wanted high-quality products");
            HeaderTabs.Add("I wanted to purchase items to prepare my next meal", "Purchase items to prepare my next meal");
            HeaderTabs.Add("It has the highest quality items", "Has highest quality items");
            HeaderTabs.Add("It was the most convenient; saved me the most time", "Convenient /saved the most time");
            HeaderTabs.Add("It has the best prices", "Has best prices");
            HeaderTabs.Add("It has specific item(s) hard to find in other places", "Has specific item(s) hard to find in other places");
            HeaderTabs.Add("It has the pharmacy or other services I needed", "Has the pharmacy / other services");
            HeaderTabs.Add("Some other motivation", "Other motivation");

            //Added below By Mehatab
            HeaderTabs.Add("BeverageConsumedMonthly", "Beverage Consumed Monthly");
            HeaderTabs.Add("BeveragepurchasedMonthly", "Beverage Categories Purchased");
            HeaderTabs.Add("ImpulseItem Top 10", "Top 10 Impulse Items");
            HeaderTabs.Add("WeekdayNet", "Weekday Net");
            HeaderTabs.Add("ReasonForStoreChoice Top 10", "Reasons For Store Choice");
            HeaderTabs.Add("DestinationItemDetails Top 10", "Destination Items");

            HeaderTabs.Add("StoreAttributesFactors", "Store Associations - High Level Groupings");
            HeaderTabs.Add("GoodPlaceToShopFactors", "Good Place To Shop for - High Level Groupings");//"Good Place To Shop Factors");
            HeaderTabs.Add("StoreAttribute Top 10", "Store Attributes");
            HeaderTabs.Add("GoodPlaceToShop Top 10", "Good Places To Shop for");
            HeaderTabs.Add("Shopper Frequency2", "Shopper Frequency");
            //HeaderTabs.Add("Primary Motivation for Store Selection", "Primary Motivation for Store Selection");
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
            HeaderTabs.Add("FAVOURITEPROTEINDRINKS", "FAVOURITE PROTEIN DRINKS");

            //added by Bramhanath for Beverage on 16-10-2015
            HeaderTabs.Add("Regular Carbonated Soft Drinks", "Reg SSD");//HeaderTabs.Add("Regular Carbonated Soft Drinks","CSD Reg");
            //HeaderTabs.Add("Diet Carbonated Soft Drinks", "CSD Diet");
            HeaderTabs.Add("Coffee in a Bottle, Can, or Carton", "RTD Coffee");
            HeaderTabs.Add("Tea in a Bottle, Can, Carton or Fountain", "RTD Tea");

            HeaderTabs.Add("Ready-to-Drink Smoothies in a Bottle", "RTD Smoothies");
            HeaderTabs.Add("Ready-to-Drink Juice Drinks/Ade (&lt;100% Juice)", "RTD Juice Drinks/Ades");
            HeaderTabs.Add("CSD Regular", "Reg SSD");
            HeaderTabs.Add("CSD Diet", "Diet SSD");
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

        //public string CommonFunctions.CheckLowSampleSize(string samplesize)
        //{
        //    string sz = string.Empty;
        //    if (!string.IsNullOrEmpty(samplesize))
        //    {
        //        //atul new
        //        if (Convert.ToDouble(samplesize) < GlobalVariables.MinSampleSize)
        //        {
        //            sz = GlobalVariables.LowSampleSize;

        //        }
        //        else if (Convert.ToDouble(samplesize) >= GlobalVariables.MinSampleSize && Convert.ToDouble(samplesize) < GlobalVariables.MaxSampleSize)
        //        {
        //            sz = GlobalVariables.UseDirectionally;

        //        }
        //        else
        //            sz = "";

        //    }

        //    return sz;
        //}

        public int GetCompIndex(int colIndex)
        {
            int indexno = 0;
            if (colIndex >= 5 && colIndex < 10)
            {
                indexno = 0;
            }
            else if (colIndex >= 10 && colIndex < 15)
            {
                indexno = 1;
            }
            else if (colIndex >= 15 && colIndex < 20)
            {
                indexno = 2;
            }
            else if (colIndex >= 20 && colIndex < 25)
                indexno = 3;

            return indexno;
            //    return CleanClass(Convert.ToString(objcomFunc.Get_ShortNames(col1[0]))); ;
        }

        //added by Nagaraju for Rounding values
        public static string GetRoundingValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = Convert.ToString(Convert.ToDouble(Math.Round(Convert.ToDouble(value), 1)).ToString("0.0"));
            return value;
        }
        //get default Geography list
        //Added by Nagaraju 
        //Date: 16-05-2016
        public Dictionary<int, string> GetDefaultGeographylist()
        {
            Dictionary<int, string> geographylist = new Dictionary<int, string>();
            DataSet dsCR = null;
            dsCR = HttpContext.Current.Session["CustomRegions"] as DataSet;
            List<int> regionidlist = (from r in dsCR.Tables[4].AsEnumerable()
                                      where string.IsNullOrEmpty(Convert.ToString(r.Field<object>("ParentId")))
                                      select Convert.ToInt32(r.Field<object>("RegionId"))).Distinct().ToList();
            foreach (int parentid in regionidlist)
            {
                var query = (from r in dsCR.Tables[4].AsEnumerable()
                             where Convert.ToInt32(r.Field<object>("RegionId")) == parentid
                             select Convert.ToString(r.Field<object>("Regions"))).FirstOrDefault();
                if (!string.IsNullOrEmpty(query) && !geographylist.ContainsKey(parentid))
                {
                    geographylist.Add(parentid, query);
                }
            }
            return geographylist;
        }

        //check time period availability
        //Added by Nagaraju 
        //Date: 16-05-2016
        public bool RegionTimePeriodType(string region, string TimePeriod, string TimePeriodType, int parentid, string CheckModule)
        {
            bool isavailable = false;
            DataSet dsCR = null;
            dsCR = HttpContext.Current.Session["CustomRegions"] as DataSet;
            List<DataRow> RegionRows = null;
            if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
            {
                RegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                              join r2 in dsCR.Tables[7].AsEnumerable()
                              on Convert.ToString(r.Field<object>("Regions")) equals Convert.ToString(r2.Field<object>("Regions"))
                              where Convert.ToInt32(r.Field<object>("ParentID")) == parentid
                              && Convert.ToString(r2.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                              && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(region), StringComparison.OrdinalIgnoreCase)
                              select r).Distinct().ToList();
            }
            else
            {
                RegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                              join r2 in dsCR.Tables[5].AsEnumerable()
                              on Convert.ToString(r.Field<object>("Regions")) equals Convert.ToString(r2.Field<object>("Regions"))
                              where Convert.ToInt32(r.Field<object>("ParentID")) == parentid
                              && Convert.ToString(r2.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                              && Convert.ToString(r2.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                              && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(region), StringComparison.OrdinalIgnoreCase)
                              select r).Distinct().ToList();
            }

            if (RegionRows != null && RegionRows.Count > 0)
                isavailable = true;

            return isavailable;
        }


        //common function to get Geography filters
        //Added by Nagaraju 
        //Date: 16-05-2016
        public string GetGeographyFilters(string TagName, string Compare, string TimePeriod, string TimePeriodType, string CheckModule)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsCR = null;
            Dictionary<int, string> regionsListDefault = new Dictionary<int, string>();
            string title = string.Empty;
            Table factSheetManager = new Table();
            DataRow query2 = null;
            DataTable tbl = null;
            string NotAvailableRegionClass = string.Empty;
            string disablestyle = string.Empty;

            try
            {
                if (HttpContext.Current.Session["CustomRegions"] != null)
                {
                    dsCR = HttpContext.Current.Session["CustomRegions"] as DataSet;

                    if (TimePeriodType.Equals("Total Time", StringComparison.OrdinalIgnoreCase) || TimePeriodType.Equals("Year", StringComparison.OrdinalIgnoreCase))
                        TimePeriod = TimePeriodType;

                    regionsListDefault = GetDefaultGeographylist();
                    string clasBackGrndColor = "colorTimePeriod", contentboxFilterColor = "contentboxFilterDummy";
                    if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                    {
                        title = "Available for Year, YTD, Quarter, 12MMT, 6MMT, 3MMT Time Periods";
                        sb.Append("<li title=\"" + title + "\" class=\"containerFilter\">" + "<table><td style=\"width:10px;\"></td><td class=\"contentboxFilter regions\" id=\"filter9999_111\" onclick=\"SelectGeographyFilter('Total US|Total US','Total US','9999_111',this,'Children','9999','Total US','Yes'); \"><span class=\"text\">Total US</span></td><td></td></table></li>");
                    }
                    else
                    {
                        title = "Available for Total Time, Year, YTD, Quarter, 12MMT, 6MMT, 3MMT Time Periods";
                        sb.Append("<li title=\"" + title + "\" class=\"containerFilter\">" + "<table><td style=\"width:10px;\"></td><td class=\"contentboxFilter regions\" id=\"filter9999_111\" onclick=\"SelectGeographyFilter('Total US|Total US','Total US','9999_111',this,'Children','9999','Total US','Yes'); \"><span class=\"text\">Total US</span></td><td></td></table></li>");
                    }

                    List<DataRow> TimePeriodTypeRows = null;
                    if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                    {
                        TimePeriodTypeRows = (from r in dsCR.Tables[7].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              select r).ToList();
                    }
                    else
                    {
                        TimePeriodTypeRows = (from r in dsCR.Tables[5].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              select r).ToList();
                    }
                    if (TimePeriodTypeRows != null && TimePeriodTypeRows.Count > 0)
                        tbl = TimePeriodTypeRows.CopyToDataTable();

                    foreach (int parentid in regionsListDefault.Keys)
                    {
                        disablestyle = string.Empty;
                        clasBackGrndColor = "";
                        contentboxFilterColor = "";
                        title = string.Empty;

                        if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                        {
                            query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                      where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                      && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();
                        }
                        else
                        {
                            query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                      where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                      && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                      && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();
                        }

                        if (query2 != null)
                            title = Convert.ToString(query2["ToolTip"]);
                        else
                        {
                            if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                            {

                                query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }
                            else
                            {
                                query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }
                            if (query2 != null)
                                title = Convert.ToString(query2["ToolTip"]);
                        }

                        if (RegionTimePeriodType(regionsListDefault[parentid], TimePeriod, TimePeriodType, parentid, CheckModule))
                        {
                            sb.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"ShowFilterSub('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "','" + regionsListDefault[parentid].Trim() + "',this)\"></td>" +
                          "<td name=\"" + TagName + "|" + regionsListDefault[parentid] + "\" value=\"" + regionsListDefault[parentid] + "\" class=\"contentboxFilter regions " + contentboxFilterColor + "\" id=\"filter" + regionsListDefault[parentid] + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"></td></tr></table>");
                        }
                        else
                        {
                            clasBackGrndColor = "colorTimePeriod";
                            contentboxFilterColor = "contentboxFilterDummy";
                            disablestyle = "pointer-events: auto; cursor:auto;";

                            sb.Append("<li style=\"" + disablestyle + "\" class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\"></td>" +
                              "<td name=\"" + TagName + "|" + regionsListDefault[parentid] + "\" value=\"" + regionsListDefault[parentid] + "\" class=\"contentboxFilter regions " + contentboxFilterColor + "\" id=\"filter" + regionsListDefault[parentid] + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"></td></tr></table>");
                        }

                        List<DataRow> RegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                    where Convert.ToInt32(r.Field<object>("ParentID")) == parentid
                                                    && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                    select r).Distinct().ToList();

                        sb.Append("<ul id=\"id" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" style=\"display:none;padding-left: 20px;\">");

                        foreach (DataRow row in RegionRows)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(row["RespColumnName"])))
                            {

                                title = string.Empty;
                                if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                {
                                    query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }
                                else
                                {
                                    query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }

                                if (query2 != null)
                                    title = Convert.ToString(query2["ToolTip"]);
                                else
                                {
                                    if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                    {
                                        query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                                  where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                                  select r).FirstOrDefault();
                                    }
                                    else
                                    {
                                        query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                                  where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                                  select r).FirstOrDefault();
                                    }
                                    if (query2 != null)
                                        title = Convert.ToString(query2["ToolTip"]);
                                }

                                sb.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"ShowFilterSub('" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "','" + Convert.ToString(row["Regions"]).Trim() + "',this)\"></td>" +
                                "<td name=\"" + TagName + "|" + Convert.ToString(row["Regions"]) + "\" value=\"" + Convert.ToString(row["Regions"]) + "\" class=\"contentboxFilter regions " + contentboxFilterColor + "\" id=\"filter" + Convert.ToString(row["Regions"]) + "\"><span style=\"position:relative;left:-3px;\" class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td><td class=\"\"></td></tr></table>");
                                sb.Append("<ul id=\"id" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" style=\"display:none;padding-left: 20px;\">");

                                List<DataRow> SubLevelRegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                                    where Convert.ToInt32(r.Field<object>("ParentID")) == Convert.ToInt32(row["RegionId"])
                                                            && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                                    select r).Distinct().ToList();

                                foreach (DataRow subrow in SubLevelRegionRows)
                                {
                                    if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                    {
                                        query2 = (from r in tbl.AsEnumerable()
                                                  where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                                  && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                                  select r).FirstOrDefault();
                                    }
                                    else
                                    {
                                        query2 = (from r in tbl.AsEnumerable()
                                                  where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                                  && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                                  && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                                  select r).FirstOrDefault();
                                    }

                                    NotAvailableRegionClass = string.Empty;
                                    disablestyle = string.Empty;
                                    clasBackGrndColor = string.Empty;
                                    contentboxFilterColor = "";
                                    title = string.Empty;
                                    if (query2 != null)
                                    {
                                        title = Convert.ToString(query2["ToolTip"]);

                                        if (regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (Convert.ToString(subrow["Regions"]).Trim().Equals("Albertson`s/Safeway Corporate NET Trade Area", StringComparison.OrdinalIgnoreCase)
                                                || Convert.ToString(subrow["Regions"]).Trim().Equals("HEB Trade Area", StringComparison.OrdinalIgnoreCase))
                                                sb.Append("<li>" + "<div title=\"" + title + "\" class=\"contentboxFilter regions " + NotAvailableRegionClass + "\" id=\"filter" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\"SelectGeographyFilter('" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "`").Trim() + "','" + Convert.ToString(subrow["Regions"]).Replace("'", "`").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "',this,'Children','" + subrow["RegionId"].ToString() + "','" + Convert.ToString(subrow["Regions"]).Replace("'", "`").Trim() + "','Yes'); \"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></div></li>");
                                            else
                                                sb.Append("<li>" + "<div class=\"contentboxFilter " + NotAvailableRegionClass + "\" id=\"filter" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\"SelectGeographyFilter('" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "`").Trim() + "','" + Convert.ToString(subrow["Regions"]).Replace("'", "`").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "',this,'Children','" + subrow["RegionId"].ToString() + "','" + Convert.ToString(row["Regions"]).Replace("'", "`").Trim() + "','NO'); \"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></div></li>");
                                        }
                                        else
                                        {
                                            sb.Append("<li>" + "<div class=\"contentboxFilter " + NotAvailableRegionClass + "\" id=\"filter" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\"SelectGeographyFilter('" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "`").Trim() + "','" + Convert.ToString(subrow["Regions"]).Replace("'", "`").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "',this,'Children','" + subrow["RegionId"].ToString() + "','" + regionsListDefault[parentid].Trim() + "','No'); \"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></div></li>");
                                        }
                                    }
                                    else
                                    {
                                        clasBackGrndColor = "colorTimePeriod";
                                        contentboxFilterColor = "contentboxFilterDummy";
                                        disablestyle = "pointer-events: auto; cursor:auto;";
                                        title = "";
                                        NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;

                                        query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                                  where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                                  select r).FirstOrDefault();

                                        if (query2 != null)
                                        {
                                            title = Convert.ToString(query2["ToolTip"]);
                                        }
                                        sb.Append("<li>" + "<div style=\"" + disablestyle + "\" class=\"contentboxFilter " + NotAvailableRegionClass + "\" id=\"filter" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></div></li>");
                                    }

                                }
                                sb.Append("</ul>");
                                sb.Append("</li>");
                            }
                            else
                            {
                                if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                {
                                    query2 = (from r in tbl.AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }
                                else
                                {
                                    query2 = (from r in tbl.AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }

                                NotAvailableRegionClass = string.Empty;
                                disablestyle = string.Empty;
                                clasBackGrndColor = string.Empty;
                                contentboxFilterColor = "";
                                title = string.Empty;
                                if (query2 != null)
                                {
                                    title = Convert.ToString(query2["ToolTip"]);

                                    if (regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase))
                                        sb.Append("<li>" + "<div class=\"contentboxFilter " + NotAvailableRegionClass + "\" id=\"filter" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" onclick=\"SelectGeographyFilter('" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "`").Trim() + "','" + Convert.ToString(row["Regions"]).Replace("'", "`").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "',this,'Children','" + row["RegionId"].ToString() + "','" + regionsListDefault[parentid].Trim() + "','Yes'); \"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></div></li>");
                                    else
                                        sb.Append("<li>" + "<div class=\"contentboxFilter " + NotAvailableRegionClass + "\" id=\"filter" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" onclick=\"SelectGeographyFilter('" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "`").Trim() + "','" + Convert.ToString(row["Regions"]).Replace("'", "`").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "',this,'Children','" + row["RegionId"].ToString() + "','" + regionsListDefault[parentid].Trim() + "','No'); \"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></div></li>");
                                }
                                else
                                {
                                    clasBackGrndColor = "colorTimePeriod";
                                    contentboxFilterColor = "contentboxFilterDummy";
                                    disablestyle = "pointer-events: auto; cursor:auto;";
                                    NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;

                                    query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                    if (query2 != null)
                                    {
                                        title = Convert.ToString(query2["ToolTip"]);
                                    }
                                    title = Convert.ToString(query2["ToolTip"]);
                                    sb.Append("<li>" + "<div style=\"" + disablestyle + "\" class=\"contentboxFilter " + NotAvailableRegionClass + "\" id=\"filter" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></div></li>");

                                }
                            }
                        }
                        sb.Append("</ul>");
                        sb.Append("</li>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sb.ToString();
        }

        #region Within Geography
        public List<PrimaryAdvancedFilter> GetWithinGeographyBenchmarkComparison(string TagName, string TimePeriod, string TimePeriodType, string CheckModule)
        {
            List<PrimaryAdvancedFilter> sData = new List<PrimaryAdvancedFilter>();
            List<PrimaryAdvancedFilter> sublevelData = new List<PrimaryAdvancedFilter>();
            PrimaryAdvancedFilter sGeoData = new PrimaryAdvancedFilter();
            sGeoData.Name = "Geography";
            sGeoData.FullName = "Geography";
            sGeoData.DBName = "Geography";
            sGeoData.Id = 100;
            sGeoData.Level = "1";

            BenchCompSelect param = new BenchCompSelect();
            StringBuilder sb_benchmark = new StringBuilder();
            StringBuilder sb_comparison = new StringBuilder();
            DataSet dsCR = null;
            Dictionary<int, string> regionsListDefault = new Dictionary<int, string>();
            string title = string.Empty;
            Table factSheetManager = new Table();
            string NotAvailableRegionClass = string.Empty;
            DataRow query2 = null;
            DataTable tbl = null;
            string disablestyle = string.Empty;
            DataAccess da = new DataAccess();
            if (HttpContext.Current.Session["CustomRegions"] == null)
            {
                DataSet dsC = da.GetData("CustomRegions");
                HttpContext.Current.Session["CustomRegions"] = dsC;
            }
            if (HttpContext.Current.Session["CustomRegions"] != null)
            {
                dsCR = HttpContext.Current.Session["CustomRegions"] as DataSet;

                if (TimePeriodType.Equals("Total Time", StringComparison.OrdinalIgnoreCase) || TimePeriodType.Equals("Year", StringComparison.OrdinalIgnoreCase))
                    TimePeriod = TimePeriodType;

                regionsListDefault = GetDefaultGeographylist();
                string clasBackGrndColor = "colorTimePeriod", contentboxFilterColor = "contentboxFilterDummy";
                SecondaryAdvancedFilter sGeoDataSecondary = new SecondaryAdvancedFilter();
                SecondaryAdvancedFilter sSubGeoDataSecondary = new SecondaryAdvancedFilter();
                List<SecondaryAdvancedFilter> sGeoDataSecondaryList = new List<SecondaryAdvancedFilter>();
                List<SecondaryAdvancedFilter> SecondaryList = new List<SecondaryAdvancedFilter>();
                List<SecondaryAdvancedFilter> SubSecondaryList = new List<SecondaryAdvancedFilter>();
                sGeoDataSecondary.Id = "Bench" + "13_01_2016";
                sGeoDataSecondary.Name = "Total";
                sGeoDataSecondary.DBName = "Census Divisions|Total";
                sGeoDataSecondaryList.Add(sGeoDataSecondary);
                //sGeoData.SecondaryAdvancedFilterlist.Add(sGeoDataSecondary);
                title = "Available for Total Time, Year, YTD, Quarter, 12MMT, 6MMT, 3MMT Time Periods";
                sb_benchmark.Append("<li><table title=\"" + title + "\"><tr><td></td>" +
                      "<td style=\"width:180px;\" id=\"Bench" + "13_01_2016" + "\" class=\"geographyBox Benchretailercontentbox regions\" onclick=\" SelectBenchmark('" + "Total" + "','" + "13_01_2016" + "','" + "Census Divisions|Total" + "');\"><span class=\"text\">" + "Total" + "</span></td></tr></table></li>");

                sb_comparison.Append("<li><table title=\"" + title + "\"><tr><td></td>" +
               "<td style=\"width:180px;\" class=\"geographyBox Compretailercontentbox regions\" id=\"" + "13_01_2016" + "\" onclick=\" SelectComparison('" + "Total" + "','" + "13_01_2016" + "','" + "Census Divisions|Total" + "');\"><span class=\"text\">" + "Total" + "</span></td></tr></table></li>");

                List<DataRow> TimePeriodTypeRows = null;
                if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                {
                    TimePeriodTypeRows = (from r in dsCR.Tables[7].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          select r).ToList();
                }
                else
                {
                    TimePeriodTypeRows = (from r in dsCR.Tables[5].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          select r).ToList();
                }
                if (TimePeriodTypeRows != null && TimePeriodTypeRows.Count > 0)
                    tbl = TimePeriodTypeRows.CopyToDataTable();

                foreach (int parentid in regionsListDefault.Keys)
                {
                    PrimaryAdvancedFilter sSubData = new PrimaryAdvancedFilter();
                    clasBackGrndColor = "";
                    contentboxFilterColor = "";
                    disablestyle = "";

                    title = string.Empty;
                    if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                    {
                        query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                  where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                  && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                  select r).FirstOrDefault();
                    }
                    else
                    {
                        query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                  where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                  && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                  && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                  select r).FirstOrDefault();
                    }

                    if (query2 != null)
                        title = Convert.ToString(query2["ToolTip"]);
                    else
                    {
                        if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                        {

                            query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                      where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();
                        }
                        else
                        {
                            query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                      where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();
                        }
                        if (query2 != null)
                            title = Convert.ToString(query2["ToolTip"]);
                    }

                    if (RegionTimePeriodType(regionsListDefault[parentid], TimePeriod, TimePeriodType, parentid, CheckModule))
                    {
                        //SecondaryAdvancedFilter sGeoDataSecondaryList = new SecondaryAdvancedFilter();
                        sGeoDataSecondary = new SecondaryAdvancedFilter();
                        sGeoDataSecondary.Id = "Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]);
                        sGeoDataSecondary.Name = regionsListDefault[parentid];
                        sGeoDataSecondary.DBName = "Census Divisions|" + regionsListDefault[parentid];
                        sGeoDataSecondary.active = "true";
                        sGeoDataSecondary.isGeography = "true";
                        sGeoDataSecondary.ToolTip = title;
                        sGeoDataSecondaryList.Add(sGeoDataSecondary);

                        sSubData.Name = regionsListDefault[parentid];
                        sSubData.FullName = regionsListDefault[parentid];
                        sSubData.DBName = regionsListDefault[parentid];
                        sSubData.Id = parentid;
                        sSubData.Level = "2";
                        sSubData.ToolTip = title;
                        //sGeoData.SecondaryAdvancedFilterlist = sGeoDataSecondary = sGeoDataSecondaryList;

                        sb_benchmark.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevel('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "',this)\"></td>" +
                 "<td id=\"Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                        sb_comparison.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table  title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevelComp('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "',this)\"></td>" +
                       "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");
                    }
                    else
                    {
                        clasBackGrndColor = "colorTimePeriod";
                        contentboxFilterColor = "contentboxFilterDummy";
                        disablestyle = "pointer-events: auto; cursor:auto;";

                        sGeoDataSecondary = new SecondaryAdvancedFilter();
                        sGeoDataSecondary.Id = "Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]);
                        sGeoDataSecondary.Name = regionsListDefault[parentid];
                        sGeoDataSecondary.DBName = "Census Divisions|" + regionsListDefault[parentid];
                        sGeoDataSecondary.active = "false";
                        sGeoDataSecondary.isGeography = "true";
                        sGeoDataSecondary.ToolTip = title;
                        sGeoDataSecondaryList.Add(sGeoDataSecondary);

                        sb_benchmark.Append("<li style=\"" + disablestyle + "\" class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\"></td>" +
                "<td id=\"Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                        sb_comparison.Append("<li style=\"" + disablestyle + "\" class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\"></td>" +
                       "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");
                    }

                    List<DataRow> RegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                where Convert.ToInt32(r.Field<object>("ParentID")) == parentid
                                                && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                select r).Distinct().ToList();

                    sb_benchmark.Append("<ul id=\"ul" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");
                    sb_comparison.Append("<ul id=\"ulComp" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");

                    foreach (DataRow row in RegionRows)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(row["RespColumnName"])))
                        {
                            sb_benchmark.Append("</tr></table>");
                            sb_comparison.Append("</tr></table>");

                            title = string.Empty;
                            if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                            {
                                query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }
                            else
                            {
                                query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }

                            if (query2 != null)
                                title = Convert.ToString(query2["ToolTip"]);
                            else
                            {
                                if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                {
                                    query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }
                                else
                                {
                                    query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }
                                if (query2 != null)
                                    title = Convert.ToString(query2["ToolTip"]);
                            }

                            sb_benchmark.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevel('" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "',this)\"></td>" +
                          "<td id=\"Bench" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span style=\"position:relative;left:-7px;\" class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                            sb_comparison.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevelComp('" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "',this)\"></td>" +
                           "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\"><span style=\"position:relative;left:-7px;\" class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                            sb_benchmark.Append("<ul id=\"ul" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");
                            sb_comparison.Append("<ul id=\"ulComp" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");

                            List<DataRow> SubLevelRegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                                where Convert.ToInt32(r.Field<object>("ParentID")) == Convert.ToInt32(row["RegionId"])
                                                        && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                                select r).Distinct().ToList();

                            SubSecondaryList = new List<SecondaryAdvancedFilter>();
                            foreach (DataRow subrow in SubLevelRegionRows)
                            {
                                if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                {
                                    query2 = (from r in tbl.AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }
                                else
                                {
                                    query2 = (from r in tbl.AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }

                                NotAvailableRegionClass = string.Empty;
                                disablestyle = string.Empty;
                                clasBackGrndColor = string.Empty;
                                contentboxFilterColor = "";
                                title = string.Empty;
                                sSubGeoDataSecondary = new SecondaryAdvancedFilter();
                                if (query2 != null)
                                {
                                    title = Convert.ToString(query2["ToolTip"]);


                                    if ((regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase)
                                        && Convert.ToString(subrow["Regions"]).Trim().Equals("Albertson`s/Safeway Corporate NET Trade Area", StringComparison.OrdinalIgnoreCase))
                                                || (regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase) && Convert.ToString(subrow["Regions"]).Trim().Equals("HEB Trade Area", StringComparison.OrdinalIgnoreCase)))
                                    {

                                        sb_benchmark.Append("<tr><td title=\"" + title + "\" id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox regions" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmark('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                        sb_comparison.Append("<tr><td title=\"" + title + "\" class=\"geographyBox Compretailercontentbox regions" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\" SelectComparison('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                    }
                                    else
                                    {
                                        sb_benchmark.Append("<tr><td id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmark('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                        sb_comparison.Append("<tr><td class=\"geographyBox Compretailercontentbox" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\" SelectComparison('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                    }

                                    sSubGeoDataSecondary.Id = "Bench" + subrow["RegionId"].ToString();
                                    sSubGeoDataSecondary.Name = Convert.ToString(subrow["Regions"]);
                                    sSubGeoDataSecondary.DBName = Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim();
                                    sSubGeoDataSecondary.active = "true";
                                    sSubGeoDataSecondary.ToolTip = title;
                                    sGeoDataSecondary.isGeography = "true";
                                }
                                else
                                {
                                    clasBackGrndColor = "colorTimePeriod";
                                    contentboxFilterColor = "contentboxFilterDummy";
                                    title = "";
                                    NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;
                                    disablestyle = "pointer-events: auto; cursor:auto;font-size:12px; height:auto;";

                                    query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                    if (query2 != null)
                                    {
                                        title = Convert.ToString(query2["ToolTip"]);
                                    }
                                    sb_benchmark.Append("<tr><td style=\"" + disablestyle + "\" id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"" + NotAvailableRegionClass + "\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                    sb_comparison.Append("<tr><td style=\"" + disablestyle + "\" class=\"" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                    sSubGeoDataSecondary.Id = "Bench" + subrow["RegionId"].ToString();
                                    sSubGeoDataSecondary.Name = Convert.ToString(subrow["Regions"]);
                                    sSubGeoDataSecondary.DBName = Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim();
                                    sSubGeoDataSecondary.active = "false";
                                    sSubGeoDataSecondary.ToolTip = title;
                                    sGeoDataSecondary.isGeography = "true";
                                }
                                SubSecondaryList.Add(sSubGeoDataSecondary);
                            }
                            sb_benchmark.Append("</table></li></ul></li>");
                            sb_comparison.Append("</table></li></ul></li>");

                            sb_benchmark.Append("<table>");
                            sb_comparison.Append("<table>");

                            sGeoDataSecondary = new SecondaryAdvancedFilter();
                            sGeoDataSecondary.Id = "Bench" + row["RegionId"].ToString();
                            sGeoDataSecondary.Name = Convert.ToString(row["Regions"]);
                            sGeoDataSecondary.DBName = Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim();
                            sGeoDataSecondary.active = "true";
                            sGeoDataSecondary.ToolTip = title;
                            sGeoDataSecondary.isGeography = "true";
                            sGeoDataSecondary.SecondaryAdvancedFilterlist = SubSecondaryList;
                            SecondaryList.Add(sGeoDataSecondary);
                        }
                        else
                        {
                            if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                            {
                                query2 = (from r in tbl.AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }
                            else
                            {
                                query2 = (from r in tbl.AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }

                            NotAvailableRegionClass = string.Empty;
                            disablestyle = string.Empty;
                            clasBackGrndColor = string.Empty;
                            contentboxFilterColor = "";
                            title = string.Empty;
                            if (query2 != null)
                            {
                                title = Convert.ToString(query2["ToolTip"]);

                                sGeoDataSecondary = new SecondaryAdvancedFilter();
                                sGeoDataSecondary.Id = "Bench" + row["RegionId"].ToString();
                                sGeoDataSecondary.Name = Convert.ToString(row["Regions"]);
                                sGeoDataSecondary.DBName = Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim();
                                sGeoDataSecondary.active = "true";
                                sGeoDataSecondary.ToolTip = title;
                                sGeoDataSecondary.isGeography = "true";
                                //sGeoDataSecondary.SecondaryAdvancedFilterlist = SubSecondaryList;
                                SecondaryList.Add(sGeoDataSecondary);

                                sb_benchmark.Append("<tr><td id=\"Bench" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmark('" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "','" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");

                                sb_comparison.Append("<tr><td class=\"geographyBox Compretailercontentbox" + NotAvailableRegionClass + "\" id=\"" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" onclick=\" SelectComparison('" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "','" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");
                            }
                            else
                            {
                                clasBackGrndColor = "colorTimePeriod";
                                contentboxFilterColor = "contentboxFilterDummy";
                                title = "";
                                NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;
                                disablestyle = "pointer-events: auto; cursor:auto;font-size:11px; height:auto;font-family: Calibri;";

                                query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();

                                if (query2 != null)
                                {
                                    title = Convert.ToString(query2["ToolTip"]);
                                }
                                sb_benchmark.Append("<tr><td  style=\"" + disablestyle + "\" id=\"Bench" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" class=\"" + NotAvailableRegionClass + "\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");

                                sb_comparison.Append("<tr><td  style=\"" + disablestyle + "\" class=\"" + NotAvailableRegionClass + "\" id=\"" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");
                            }

                        }
                    }
                    if (SecondaryList.Count > 0)
                        sSubData.SecondaryAdvancedFilterlist = SecondaryList;
                    SecondaryList = new List<SecondaryAdvancedFilter>();
                    if (sSubData.Name != null)
                        sData.Add(sSubData);
                    sb_benchmark.Append("</table></li></ul></li>");
                    sb_comparison.Append("</table></li></ul></li>");
                }
                sGeoData.SecondaryAdvancedFilterlist = sGeoDataSecondaryList;
                sData.Add(sGeoData);
            }

            sb_benchmark.Append("</ul>");
            sb_comparison.Append("</ul>");

            param.benchmarklist = sb_benchmark.ToString();
            param.comparisonlist = sb_comparison.ToString();
            return sData;
        }

        public BenchCompSelect GetWithinGeography(string TagName, string TimePeriod, string TimePeriodType, string CheckModule)
        {
            BenchCompSelect param = new BenchCompSelect();
            StringBuilder sb_benchmark = new StringBuilder();
            StringBuilder sb_comparison = new StringBuilder();
            DataSet dsCR = null;
            Dictionary<int, string> regionsListDefault = new Dictionary<int, string>();
            string title = string.Empty;
            Table factSheetManager = new Table();
            string NotAvailableRegionClass = string.Empty;
            DataRow query2 = null;
            DataTable tbl = null;
            string disablestyle = string.Empty;
            if (HttpContext.Current.Session["CustomRegions"] != null)
            {
                dsCR = HttpContext.Current.Session["CustomRegions"] as DataSet;


                regionsListDefault = GetDefaultGeographylist();
                string clasBackGrndColor = "colorTimePeriod", contentboxFilterColor = "contentboxFilterDummy";

                title = "Available for Total Time, Year, YTD, Quarter, 12MMT, 6MMT, 3MMT Time Periods";
                sb_benchmark.Append("<li><table title=\"" + title + "\"><tr><td></td>" +
                      "<td style=\"width:180px;\" id=\"Bench" + "13_01_2016" + "\" class=\"geographyBox Benchretailercontentbox regions\" onclick=\"showLevel('" + "Total" + "','" + "13_01_2016" + "','" + "Census Divisions|Total" + "');\"><span class=\"text\">" + "Total" + "</span></td></tr></table></li>");

                sb_comparison.Append("<li><table title=\"" + title + "\"><tr><td></td>" +
               "<td style=\"width:180px;\" class=\"geographyBox Compretailercontentbox regions\" id=\"" + "13_01_2016" + "\" onclick=\"SelectComparison('" + "Total" + "','" + "13_01_2016" + "','" + "Census Divisions|Total" + "');\"><span class=\"text\">" + "Total" + "</span></td></tr></table></li>");

                List<DataRow> TimePeriodTypeRows = null;

                TimePeriodTypeRows = (from r in dsCR.Tables[5].AsEnumerable()
                                      select r).ToList();
                if (TimePeriodTypeRows != null && TimePeriodTypeRows.Count > 0)
                    tbl = TimePeriodTypeRows.CopyToDataTable();

                foreach (int parentid in regionsListDefault.Keys)
                {
                    clasBackGrndColor = "";
                    contentboxFilterColor = "";
                    disablestyle = "";

                    title = string.Empty;
                    //ria : changed
                    query2 = (from r in dsCR.Tables[6].AsEnumerable()
                              where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                              select r).FirstOrDefault();
                    //change end
                    if (query2 != null)
                        title = Convert.ToString(query2["ToolTip"]);
                    else
                    {

                        query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                  where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                  select r).FirstOrDefault();
                        if (query2 != null)
                            title = Convert.ToString(query2["ToolTip"]);
                    }

                    if (RegionTimePeriodType(regionsListDefault[parentid], TimePeriod, TimePeriodType, parentid, CheckModule))
                    {
                        sb_benchmark.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevel('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "',this)\"></td>" +
                 "<td id=\"Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                        sb_comparison.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table  title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevelComp('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "',this)\"></td>" +
                       "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");
                    }
                    else
                    {
                        clasBackGrndColor = "colorTimePeriod";
                        contentboxFilterColor = "contentboxFilterDummy";
                        disablestyle = "pointer-events: auto; cursor:auto;";

                        sb_benchmark.Append("<li style=\"" + disablestyle + "\" class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse onclick=\"showLevel('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "',this)\"\"></td>" +
                "<td id=\"Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                        sb_comparison.Append("<li style=\"" + disablestyle + "\" class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\"></td>" +
                       "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");
                    }

                    List<DataRow> RegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                where Convert.ToInt32(r.Field<object>("ParentID")) == parentid
                                                && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                select r).Distinct().ToList();

                    sb_benchmark.Append("<ul id=\"ul" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");
                    sb_comparison.Append("<ul id=\"ulComp" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");

                    foreach (DataRow row in RegionRows)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(row["RespColumnName"])))
                        {
                            sb_benchmark.Append("</tr></table>");
                            sb_comparison.Append("</tr></table>");

                            title = string.Empty;

                            query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                      where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();

                            if (query2 != null)
                                title = Convert.ToString(query2["ToolTip"]);
                            else
                            {

                                query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                                if (query2 != null)
                                    title = Convert.ToString(query2["ToolTip"]);
                            }

                            sb_benchmark.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevel('" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "',this)\"></td>" +
                          "<td id=\"Bench" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span style=\"position:relative;left:-7px;\" class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                            sb_comparison.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevelComp('" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "',this)\"></td>" +
                           "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\"><span style=\"position:relative;left:-7px;\" class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                            sb_benchmark.Append("<ul id=\"ul" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");
                            sb_comparison.Append("<ul id=\"ulComp" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");

                            List<DataRow> SubLevelRegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                                where Convert.ToInt32(r.Field<object>("ParentID")) == Convert.ToInt32(row["RegionId"])
                                                        && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                                select r).Distinct().ToList();

                            foreach (DataRow subrow in SubLevelRegionRows)
                            {

                                query2 = (from r in tbl.AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();

                                NotAvailableRegionClass = string.Empty;
                                disablestyle = string.Empty;
                                clasBackGrndColor = string.Empty;
                                contentboxFilterColor = "";
                                title = string.Empty;
                                if (query2 != null)
                                {
                                    title = Convert.ToString(query2["ToolTip"]);


                                    if ((regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase)
                                        && Convert.ToString(subrow["Regions"]).Trim().Equals("Albertson`s/Safeway Corporate NET Trade Area", StringComparison.OrdinalIgnoreCase))
                                                || (regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase) && Convert.ToString(subrow["Regions"]).Trim().Equals("HEB Trade Area", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        sb_benchmark.Append("<tr><td title=\"" + title + "\" id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox regions" + NotAvailableRegionClass + "\" onclick=\"SelectBenchmark('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                        sb_comparison.Append("<tr><td title=\"" + title + "\" class=\"geographyBox Compretailercontentbox regions" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\" SelectComparison('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                    }
                                    else
                                    {
                                        sb_benchmark.Append("<tr><td id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmark('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                        sb_comparison.Append("<tr><td class=\"geographyBox Compretailercontentbox" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\" SelectComparison('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                    }
                                }
                                else
                                {
                                    clasBackGrndColor = "colorTimePeriod";
                                    contentboxFilterColor = "contentboxFilterDummy";
                                    title = "";
                                    NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;
                                    disablestyle = "pointer-events: auto; cursor:auto;font-size:12px; height:auto;";

                                    query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                    if (query2 != null)
                                    {
                                        title = Convert.ToString(query2["ToolTip"]);
                                    }
                                    sb_benchmark.Append("<tr><td style=\"" + disablestyle + "\" id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"" + NotAvailableRegionClass + "\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                    sb_comparison.Append("<tr><td style=\"" + disablestyle + "\" class=\"" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                }

                            }
                            sb_benchmark.Append("</table></li></ul></li>");
                            sb_comparison.Append("</table></li></ul></li>");

                            sb_benchmark.Append("<table>");
                            sb_comparison.Append("<table>");

                        }
                        else
                        {

                            query2 = (from r in tbl.AsEnumerable()
                                      where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();
                            NotAvailableRegionClass = string.Empty;
                            disablestyle = string.Empty;
                            clasBackGrndColor = string.Empty;
                            contentboxFilterColor = "";
                            title = string.Empty;
                            if (query2 != null)
                            {
                                title = Convert.ToString(query2["ToolTip"]);

                                sb_benchmark.Append("<tr><td id=\"Bench" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmark('" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "','" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");

                                sb_comparison.Append("<tr><td class=\"geographyBox Compretailercontentbox" + NotAvailableRegionClass + "\" id=\"" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" onclick=\" SelectComparison('" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "','" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");
                            }
                            else
                            {
                                clasBackGrndColor = "colorTimePeriod";
                                contentboxFilterColor = "contentboxFilterDummy";
                                title = "";
                                NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;
                                disablestyle = "pointer-events: auto; cursor:auto;font-size:11px; height:auto;font-family: Calibri;";

                                query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();

                                if (query2 != null)
                                {
                                    title = Convert.ToString(query2["ToolTip"]);
                                }
                                sb_benchmark.Append("<tr><td  style=\"" + disablestyle + "\" id=\"Bench" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" class=\"" + NotAvailableRegionClass + "\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");

                                sb_comparison.Append("<tr><td  style=\"" + disablestyle + "\" class=\"" + NotAvailableRegionClass + "\" id=\"" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");
                            }

                        }
                    }
                    sb_benchmark.Append("</table></li></ul></li>");
                    sb_comparison.Append("</table></li></ul></li>");
                }
            }

            sb_benchmark.Append("</ul>");
            sb_comparison.Append("</ul>");

            param.benchmarklist = sb_benchmark.ToString();
            param.comparisonlist = sb_comparison.ToString();
            return param;
        }

        public BenchCompSelect GetWithinGeographyTotalBenchmarkComparison(string TagName, string TimePeriod, string TimePeriodType, string CheckModule)
        {
            BenchCompSelect param = new BenchCompSelect();
            StringBuilder sb_benchmark = new StringBuilder();
            StringBuilder sb_comparison = new StringBuilder();
            DataSet dsCR = null;
            Dictionary<int, string> regionsListDefault = new Dictionary<int, string>();
            string title = string.Empty;
            Table factSheetManager = new Table();
            string NotAvailableRegionClass = string.Empty;
            DataRow query2 = null;
            DataTable tbl = null;
            string disablestyle = string.Empty;
            if (HttpContext.Current.Session["CustomRegions"] != null)
            {
                dsCR = HttpContext.Current.Session["CustomRegions"] as DataSet;

                if (TimePeriodType.Equals("Total Time", StringComparison.OrdinalIgnoreCase) || TimePeriodType.Equals("Year", StringComparison.OrdinalIgnoreCase))
                    TimePeriod = TimePeriodType;

                regionsListDefault = GetDefaultGeographylist();
                string clasBackGrndColor = "colorTimePeriod", contentboxFilterColor = "contentboxFilterDummy";

                title = "Available for Total Time, Year, YTD, Quarter, 12MMT, 6MMT, 3MMT Time Periods";
                sb_benchmark.Append("<li><table title=\"" + title + "\"><tr><td></td>" +
                      "<td  style=\"width:180px;\" id=\"Bench" + "13_01_2016" + "\" class=\"geographyBox Benchretailercontentbox regions\" onclick=\" SelectBenchmarkTotal('" + "Total" + "','" + "13_01_2016" + "','" + "Census Divisions|Total" + "');\"><span class=\"text\">" + "Total" + "</span></td></tr></table></li>");

                sb_comparison.Append("<li><table title=\"" + title + "\"><tr><td></td>" +
               "<td style=\"width:180px;\" class=\"geographyBox Compretailercontentbox regions\" id=\"" + "13_01_2016" + "\" onclick=\" SelectComparisonTotal('" + "Total" + "','" + "13_01_2016" + "','" + "Census Divisions|Total" + "');\"><span class=\"text\">" + "Total" + "</span></td></tr></table></li>");

                List<DataRow> TimePeriodTypeRows = null;
                if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                {
                    TimePeriodTypeRows = (from r in dsCR.Tables[7].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          select r).ToList();
                }
                else
                {
                    TimePeriodTypeRows = (from r in dsCR.Tables[5].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          select r).ToList();
                }
                if (TimePeriodTypeRows != null && TimePeriodTypeRows.Count > 0)
                    tbl = TimePeriodTypeRows.CopyToDataTable();

                foreach (int parentid in regionsListDefault.Keys)
                {
                    clasBackGrndColor = "";
                    contentboxFilterColor = "";
                    disablestyle = "";

                    title = string.Empty;
                    if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                    {
                        query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                  where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                  && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                  select r).FirstOrDefault();
                    }
                    else
                    {
                        query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                  where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                  && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                  && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                  select r).FirstOrDefault();
                    }

                    if (query2 != null)
                        title = Convert.ToString(query2["ToolTip"]);
                    else
                    {

                        if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                        {
                            query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                      where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();
                        }
                        else
                        {
                            query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                      where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                      select r).FirstOrDefault();
                        }
                        if (query2 != null)
                            title = Convert.ToString(query2["ToolTip"]);
                    }

                    if (RegionTimePeriodType(regionsListDefault[parentid], TimePeriod, TimePeriodType, parentid, CheckModule))
                    {
                        sb_benchmark.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevel('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "',this)\"></td>" +
                 "<td id=\"Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                        sb_comparison.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table  title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevelComp('" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "',this)\"></td>" +
                       "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");
                    }
                    else
                    {
                        clasBackGrndColor = "colorTimePeriod";
                        contentboxFilterColor = "contentboxFilterDummy";
                        disablestyle = "pointer-events: auto; cursor:auto;";

                        sb_benchmark.Append("<li style=\"" + disablestyle + "\" class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\"></td>" +
                "<td id=\"Bench" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                        sb_comparison.Append("<li style=\"" + disablestyle + "\" class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\"></td>" +
                       "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\"><span class=\"text\">" + regionsListDefault[parentid] + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");
                    }

                    List<DataRow> RegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                where Convert.ToInt32(r.Field<object>("ParentID")) == parentid
                                                && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                select r).Distinct().ToList();

                    sb_benchmark.Append("<ul id=\"ul" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");
                    sb_comparison.Append("<ul id=\"ulComp" + factSheetManager.CleanClass(regionsListDefault[parentid]) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");

                    foreach (DataRow row in RegionRows)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(row["RespColumnName"])))
                        {
                            sb_benchmark.Append("</tr></table>");
                            sb_comparison.Append("</tr></table>");

                            title = string.Empty;
                            if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                            {
                                query2 = (from r in dsCR.Tables[8].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }
                            else
                            {
                                query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }

                            if (query2 != null)
                                title = Convert.ToString(query2["ToolTip"]);
                            else
                            {
                                query2 = (from r in dsCR.Tables[6].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(Convert.ToString(row["Regions"])), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();

                                if (query2 != null)
                                    title = Convert.ToString(query2["ToolTip"]);
                            }

                            sb_benchmark.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevel('" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "',this)\"></td>" +
                          "<td id=\"Bench" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" ><span style=\"position:relative;left:-7px;\" class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                            sb_comparison.Append("<li class=\"containerFilter " + clasBackGrndColor + " " + contentboxFilterColor + "\"><table title=\"" + title + "\"><tr><td class=\"TreeviewImage treeview-collapse\" onclick=\"showLevelComp('" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "',this)\"></td>" +
                           "<td class=\"geographyBoxHeading regions " + contentboxFilterColor + "\" id=\"" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\"><span style=\"position:relative;left:-7px;\" class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td><td class=\"\"  title=\"" + title + "\"></td></tr></table>");

                            sb_benchmark.Append("<ul id=\"ul" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");
                            sb_comparison.Append("<ul id=\"ulComp" + factSheetManager.CleanClass(Convert.ToString(row["Regions"])) + "\" style=\"display:none;padding-left: 20px;\"><li class=\"containerFilter\"><table>");

                            List<DataRow> SubLevelRegionRows = (from r in dsCR.Tables[4].AsEnumerable()
                                                                where Convert.ToInt32(r.Field<object>("ParentID")) == Convert.ToInt32(row["RegionId"])
                                                        && !Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(regionsListDefault[parentid]), StringComparison.OrdinalIgnoreCase)
                                                                select r).Distinct().ToList();

                            foreach (DataRow subrow in SubLevelRegionRows)
                            {
                                if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                                {
                                    query2 = (from r in tbl.AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }
                                else
                                {
                                    query2 = (from r in tbl.AsEnumerable()
                                              where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                              && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                }

                                NotAvailableRegionClass = string.Empty;
                                disablestyle = string.Empty;
                                clasBackGrndColor = string.Empty;
                                contentboxFilterColor = "";
                                title = string.Empty;
                                if (query2 != null)
                                {
                                    title = Convert.ToString(query2["ToolTip"]);
                                    if ((regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase)
                                        && Convert.ToString(subrow["Regions"]).Trim().Equals("Albertson`s/Safeway Corporate NET Trade Area", StringComparison.OrdinalIgnoreCase))
                                                || (regionsListDefault[parentid].Equals("Trade Areas", StringComparison.OrdinalIgnoreCase) && Convert.ToString(subrow["Regions"]).Trim().Equals("HEB Trade Area", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        sb_benchmark.Append("<tr><td title=\"" + title + "\" id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox regions" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmarkTotal('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                        sb_comparison.Append("<tr><td title=\"" + title + "\" class=\"geographyBox Compretailercontentbox regions" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\" SelectComparisonTotal('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                    }
                                    else
                                    {
                                        sb_benchmark.Append("<tr><td id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmarkTotal('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                        sb_comparison.Append("<tr><td class=\"geographyBox Compretailercontentbox" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" onclick=\" SelectComparisonTotal('" + Convert.ToString(subrow["Regions"]).Replace("'", "~").Trim() + "','" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "','" + Convert.ToString(subrow["RespColumnName"]).Trim() + "|" + Convert.ToString(subrow["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                    }
                                }
                                else
                                {
                                    clasBackGrndColor = "colorTimePeriod";
                                    contentboxFilterColor = "contentboxFilterDummy";
                                    title = "";
                                    NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;
                                    disablestyle = "pointer-events: auto; cursor:auto;font-size:12px; height:auto;";

                                    query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                              where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(subrow["Regions"]), StringComparison.OrdinalIgnoreCase)
                                              select r).FirstOrDefault();
                                    if (query2 != null)
                                    {
                                        title = Convert.ToString(query2["ToolTip"]);
                                    }
                                    sb_benchmark.Append("<tr><td style=\"" + disablestyle + "\" id=\"Bench" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\" class=\"" + NotAvailableRegionClass + "\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");

                                    sb_comparison.Append("<tr><td style=\"" + disablestyle + "\" class=\"" + NotAvailableRegionClass + "\" id=\"" + subrow["RegionId"].ToString() + "_" + subrow["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(subrow["Regions"]) + "</span></td></tr>");
                                }

                            }
                            sb_benchmark.Append("</table></li></ul></li>");
                            sb_comparison.Append("</table></li></ul></li>");

                            sb_benchmark.Append("<table>");
                            sb_comparison.Append("<table>");

                        }
                        else
                        {
                            if (CheckModule.Equals("Time", StringComparison.OrdinalIgnoreCase))
                            {
                                query2 = (from r in tbl.AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }
                            else
                            {
                                query2 = (from r in tbl.AsEnumerable()
                                          where Convert.ToString(r.Field<object>("TimePeriodType")).Equals(TimePeriodType, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("TimePeriod")).Equals(TimePeriod, StringComparison.OrdinalIgnoreCase)
                                          && Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();
                            }

                            NotAvailableRegionClass = string.Empty;
                            disablestyle = string.Empty;
                            clasBackGrndColor = string.Empty;
                            contentboxFilterColor = "";
                            title = string.Empty;
                            if (query2 != null)
                            {
                                title = Convert.ToString(query2["ToolTip"]);

                                sb_benchmark.Append("<tr><td id=\"Bench" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" class=\"geographyBox Benchretailercontentbox" + NotAvailableRegionClass + "\" onclick=\" SelectBenchmarkTotal('" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "','" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "');\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");

                                sb_comparison.Append("<tr><td class=\"geographyBox Compretailercontentbox" + NotAvailableRegionClass + "\" id=\"" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" onclick=\" SelectComparisonTotal('" + Convert.ToString(row["Regions"]).Replace("'", "~").Trim() + "','" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "','" + Convert.ToString(row["RespColumnName"]).Trim() + "|" + Convert.ToString(row["Regions"]).Replace("'", "~") + "');\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");
                            }
                            else
                            {
                                clasBackGrndColor = "colorTimePeriod";
                                contentboxFilterColor = "contentboxFilterDummy";
                                title = "";
                                NotAvailableRegionClass = " " + clasBackGrndColor + " " + contentboxFilterColor;
                                disablestyle = "pointer-events: auto; cursor:auto;font-size:11px; height:auto;font-family: Calibri;";

                                query2 = (from r in dsCR.Tables[5].AsEnumerable()
                                          where Convert.ToString(r.Field<object>("Regions")).Equals(Convert.ToString(row["Regions"]), StringComparison.OrdinalIgnoreCase)
                                          select r).FirstOrDefault();

                                if (query2 != null)
                                {
                                    title = Convert.ToString(query2["ToolTip"]);
                                }
                                sb_benchmark.Append("<tr><td  style=\"" + disablestyle + "\" id=\"Bench" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\" class=\"" + NotAvailableRegionClass + "\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");

                                sb_comparison.Append("<tr><td  style=\"" + disablestyle + "\" class=\"" + NotAvailableRegionClass + "\" id=\"" + row["RegionId"].ToString() + "_" + row["ParentID"].ToString() + "\"><span class=\"text\">" + Convert.ToString(row["Regions"]) + "</span></td></tr>");
                            }

                        }
                    }
                    sb_benchmark.Append("</table></li></ul></li>");
                    sb_comparison.Append("</table></li></ul></li>");
                }
            }

            sb_benchmark.Append("</ul>");
            sb_comparison.Append("</ul>");

            param.benchmarklist = sb_benchmark.ToString();
            param.comparisonlist = sb_comparison.ToString();
            return param;
        }
        #endregion

        #region Sample Size functions
        public static String CheckdecimalValue(String value)
        {
            string decimaval = "0";
            if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) != 0 && Convert.ToDouble(value) == GlobalVariables.NANumber)
                return string.Empty;
            else if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) != 0 && Convert.ToDouble(value) != GlobalVariables.NANumber)
                decimaval = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(value)));
            return decimaval;
        }
        public static String CheckdecimalValue(String value, bool isBeverageDetail = false, bool isLiquidFlavorEnhancer = false)
        {
            string decimaval = "0";
            if (isBeverageDetail && isLiquidFlavorEnhancer)
                decimaval = string.Empty;
            else if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) != 0 && Convert.ToDouble(value) != GlobalVariables.NANumber)
                decimaval = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(value)));
            return decimaval;
        }
        public static String CheckXMLCommaSeparatedValue(String value, out bool IsApplicable)
        {
            string decimaval = "0";
            IsApplicable = true;
            if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) == GlobalVariables.NANumber)
            {
                IsApplicable = false;
            }
            else if (!string.IsNullOrEmpty(value))
            {
                decimaval = value;
            }
            return decimaval;
        }
        public static string CheckLowSampleSize(string samplesize, out int samplecellstyle, bool isBeverageDetail = false, bool isLiquidFlavorEnhancer = false, bool LoyaltyPyramid = false)
        {
            string sz = string.Empty;
            samplecellstyle = 4;
            if ((isBeverageDetail && isLiquidFlavorEnhancer) || (!string.IsNullOrEmpty(samplesize) && Convert.ToDouble(samplesize) == GlobalVariables.NANumber))
            {
                sz = GlobalVariables.NA;
                samplecellstyle = 20;
            }
            else if (!string.IsNullOrEmpty(samplesize))
            {
                if (Convert.ToDouble(samplesize) < GlobalVariables.MinSampleSize)
                {
                    sz = GlobalVariables.LowSampleSize;
                    samplecellstyle = 20;
                }
                else if (Convert.ToDouble(samplesize) >= GlobalVariables.MinSampleSize && Convert.ToDouble(samplesize) < GlobalVariables.MaxSampleSize)
                {
                    sz = GlobalVariables.UseDirectionally;
                    samplecellstyle = 30;
                }
                else
                {
                    samplecellstyle = 4;
                }
            }
            else if (string.IsNullOrEmpty(samplesize) && LoyaltyPyramid)
            {
                sz = GlobalVariables.NA;
                samplecellstyle = 20;
            }
            else if (string.IsNullOrEmpty(samplesize))
            {
                sz = GlobalVariables.LowSampleSize;
                samplecellstyle = 20;
            }
            return sz;
        }
        public static String CheckXMLLowSampleSize(string samplesize, bool isBeverageDetail = false, bool isLiquidFlavorEnhancer = false, bool LoyaltyPyramid = false)
        {
            string sz = string.Empty;
            if ((isBeverageDetail && isLiquidFlavorEnhancer) || (!string.IsNullOrEmpty(samplesize) && Convert.ToDouble(samplesize) == GlobalVariables.NANumber))
            {
                sz = GlobalVariables.NA;
            }
            else if (!string.IsNullOrEmpty(samplesize))
            {
                if (Convert.ToDouble(samplesize) < GlobalVariables.MinSampleSize)
                {
                    sz = GlobalVariables.LowSampleSize;
                }
                else if (Convert.ToDouble(samplesize) >= GlobalVariables.MinSampleSize && Convert.ToDouble(samplesize) < GlobalVariables.MaxSampleSize)
                {
                    sz = GlobalVariables.UseDirectionally;
                }
            }
            else if (string.IsNullOrEmpty(samplesize) && LoyaltyPyramid)
            {
                sz = GlobalVariables.NA;
            }
            else if (string.IsNullOrEmpty(samplesize))
            {
                sz = GlobalVariables.LowSampleSize;
            }
            return sz;
        }
        public static bool CheckMediumSampleSize(string samplesizekey, Dictionary<string, string> sampleSize)
        {
            try
            {
                if (sampleSize.ContainsKey(samplesizekey))
                {
                    string val = sampleSize[samplesizekey];
                    if (string.IsNullOrEmpty(val))
                        return false;
                    double szvalue = Convert.ToDouble(sampleSize[samplesizekey]);
                    if (szvalue >= GlobalVariables.MinSampleSize && szvalue < GlobalVariables.MaxSampleSize)
                    {
                        return true;
                    }
                    else
                    {
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
        #endregion
        #region Canvert to title case
        public static string ToTitleCase(String str)
        {
            if (!string.IsNullOrEmpty(str))
                return Regex.Replace(str, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
            else
                return string.Empty;
        }
        #endregion

        #region SignOut
        public void SignOut()
        {
            ClearSession();
            if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
            {
                HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");
            }
            else
            {
                HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
            }

        }
        #endregion
        #region Clear User Session
        private void ClearSession()
        {
            HttpContext.Current.Session.Remove(SessionVariables.USERID);
            HttpContext.Current.Session.Remove(SessionVariables.FilterData);
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
        }
        #endregion

    }
}