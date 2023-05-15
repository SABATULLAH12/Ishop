using System;
using System.Web;
using System.Web.Optimization;

namespace iSHOPNew
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            #region add tool bundles
            #region layout bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/layout").Include(
                       "~/Scripts/Slider/jquery-ui-slider-pips.css",
                       "~/Scripts/Slider/jquery-ui-1.10.1.custom.css",
                       "~/Content/Layout/Layout.css"));
            #endregion
            #region script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/top-layout").Include(
                       "~/Scripts/jquery-1.11.0.library.js",
                       "~/Scripts/jquery-ui.library.js",
                       "~/Scripts/index.db.js",
                       "~/Scripts/Layout/RightPanelFilter.js",
                       "~/Scripts/StatTesting.js",
                       "~/Scripts/Layout/Common.js",
                       "~/Scripts/Layout/LeftPanel.js",
                       "~/Scripts/Layout/Layout.js"                      
                       ));

            bundles.Add(new ScriptBundle("~/bundle/scripts/bottom-layout").Include(                     
                      "~/Scripts/Slider/jquery-ui-1.10.3.custom.library.js",
                      "~/Scripts/Slider/jquery.cookie.js",
                      "~/Scripts/Slider/waypoints.library.js",
                      "~/Scripts/Slider/plugins.js",
                      "~/Scripts/Slider/prettify.js",
                      "~/Scripts/Slider/jquery-ui-slider-pips.js",
                      "~/Scripts/Slider/init.js",
                      "~/Scripts/jquery.nicescroll.js",
                      "~/Scripts/lodash.library.js",
                      "~/Scripts/d3.v3.library.js"
                      ));
            #endregion
            #endregion

            #region E-Com layout bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/E-com-layout").Include(
                       "~/Scripts/Slider/jquery-ui-slider-pips.css",
                       "~/Scripts/Slider/jquery-ui-1.10.1.custom.css",
                       "~/Content/Layout/Layout.css"));
            #endregion
            #region script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/E-com-top-layout").Include(
                       "~/Scripts/jquery-1.11.0.library.js",
                       "~/Scripts/jquery-ui.library.js",
                       "~/Scripts/index.db.js",
                        "~/Scripts/StatTesting.js",
                       "~/Scripts/Layout/Common.js",
                       "~/Scripts/Layout/LeftPanel.js",
                       "~/Scripts/Layout/ECommerce.js",
                       "~/Scripts/Layout/ECommerceRightPanel.js"                       
                       ));

            bundles.Add(new ScriptBundle("~/bundle/scripts/E-com-bottom-layout").Include(
                      "~/Scripts/Slider/jquery-ui-1.10.3.custom.library.js",
                      "~/Scripts/Slider/jquery.cookie.js",
                      "~/Scripts/Slider/waypoints.library.js",
                      "~/Scripts/Slider/plugins.js",
                      "~/Scripts/Slider/prettify.js",
                      "~/Scripts/Slider/jquery-ui-slider-pips.js",
                      "~/Scripts/Slider/init.js",
                      "~/Scripts/jquery.nicescroll.js",
                      "~/Scripts/lodash.library.js",
                      "~/Scripts/d3.v3.library.js"
                      ));
            #endregion
            #endregion

            #region table bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/table").Include(
                    "~/Content/Tables/Tables.css"));
            #endregion

            #region compare retailer script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/table-compare_retailer").Include(
                  "~/Scripts/Tables/CompareRetailers.js",
                  "~/Scripts/Tables/Table-Common.js"));
            #endregion

            #region retailer deep dive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/table-retailer_deep_dive").Include(
                  "~/Scripts/Tables/RetailerDeepDive.js",
                  "~/Scripts/Tables/Table-Common.js"));
            #endregion

            #region compare beverages script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/table-compare_beverages").Include(
                  "~/Scripts/Tables/CompareBeverages.js",
                  "~/Scripts/Tables/Table-Common.js"));
            #endregion

            #region beverage deep dive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/table-beverage_deepdive").Include(
                  "~/Scripts/Tables/BeverageDeepDive.js",
                  "~/Scripts/Tables/Table-Common.js"));
            #endregion
            #endregion

            #region chart bundles
            #region style bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/chart").Include(
                    "~/Content/Charts/Charts.css"));
            #endregion

            #region beverage deep dive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/chart-beverage_deepdive").Include(
                  "~/Scripts/Charts/Chart-Common.js",
                  "~/Scripts/Charts/BeverageDeepDive.js"));
            #endregion

            #region compare beverages script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/chart-compare_beverages").Include(
                  "~/Scripts/Charts/Chart-Common.js",
                  "~/Scripts/Charts/CompareBeverages.js"));
            #endregion

            #region compare retailers script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/chart-compare_retailers").Include(
                  "~/Scripts/Charts/Chart-Common.js",
                  "~/Scripts/Charts/CompareRetailers.js"));
            #endregion

            #region retailer deep dive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/chart-retailer_deep_dive").Include(
                  "~/Scripts/Charts/Chart-Common.js",
                  "~/Scripts/Charts/RetailerDeepDive.js"));
            #endregion
            #endregion
            #endregion

            #region reports bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/reports").Include(
                   "~/Content/Reports/Reports.css"));
            #endregion

            #region Beverage Monthly Plus Purchasers DeepDive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Beverage_MonthlyPlus_Purchasers_Deep_Dive").Include(
                  "~/Scripts/Reports/BeverageMonthlyPlusPurchasersDeepDive.js",
                  "~/Scripts/Reports/Reports-Common.js"));
            #endregion

            #region Beverages Purchase Details Deep Dive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Beverages_Purchase_Details_Deep_Dive").Include(
                  "~/Scripts/Reports/BeveragesPurchaseDetailsDeepDive.js",
                  "~/Scripts/Reports/Reports-Common.js"));
            #endregion

            #region Compare Beverages Monthly Plus Purchasers script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Compare_Beverages_MonthlyPlus_Purchasers").Include(
                  "~/Scripts/Reports/CompareBeveragesMonthlyPlusPurchasers.js",
                  "~/Scripts/Reports/Reports-Common.js"));
            #endregion

            #region Compare Beverages Purchase Details script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Compare_Beverages_Purchase_Details").Include(
                  "~/Scripts/Reports/CompareBeveragesPurchaseDetails.js",
                  "~/Scripts/Reports/Reports-Common.js"));
            #endregion

            #region Compare Retailers Path To Purchase script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Compare_Retailers_Path_To_Purchase").Include(
                  "~/Scripts/Reports/CompareRetailersPathToPurchase.js",
                  "~/Scripts/Reports/Reports-Common.js"));

            #region Compare Retailers Shoppers script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Compare_Retailers_Shoppers").Include(
                  "~/Scripts/Reports/CompareRetailersShoppers.js",
                  "~/Scripts/Reports/Reports-Common.js"));
            #endregion

            #region Retailers Path To Purchase DeepDive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Retailers_Path_To_Purchase_Deep_Dive").Include(
                  "~/Scripts/Reports/RetailersPathToPurchaseDeepDive.js",
                  "~/Scripts/Reports/Reports-Common.js"));
            #endregion

            #region Retailers Shopper Deep Dive script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Retailers_Shopper_Deep_Dive").Include(
                  "~/Scripts/Reports/RetailersShopperDeepDive.js",
                  "~/Scripts/Reports/Reports-Common.js"));
            #endregion
            #endregion

            #region Total Respondents Reports
            bundles.Add(new StyleBundle("~/bundle/styles/Total_Respondents_Reports").Include(
                "~/Content/Reports/TotalRespondentsReport.css"));

            bundles.Add(new ScriptBundle("~/bundle/scripts/reports-Total_Respondents_Reports").Include(
                 "~/Scripts/Reports/TotalRespondentsReport.js",
                 "~/Scripts/Reports/Reports-Common.js"));
            #endregion

            #region Sar BriefingBook Reports
            bundles.Add(new StyleBundle("~/bundle/styles/ReportsSarCSS").Include(
                "~/Content/Reports/ReportsSar.css"
                ));

            bundles.Add(new ScriptBundle("~/bundle/scripts/ReportsSarJS").Include(
                 "~/Scripts/jscolor.js",
                 "~/Scripts/Reports/SARReport.js"));
            #endregion

            #endregion

            #region add'l capabilities bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/addlcapabilities").Include(
                              "~/Content/Analysis/Analysis.css"));
            #endregion

            #region Across Shopper
            bundles.Add(new ScriptBundle("~/bundle/scripts/addlcapabilities-across_shopper").Include(
                  "~/Scripts/Analysis/BGM.js",
                  "~/Scripts/Analysis/Analysis-Common.js"));
            #endregion

            #region Across Trips
            bundles.Add(new ScriptBundle("~/bundle/scripts/addlcapabilities-across_trips").Include(
                  "~/Scripts/Analysis/SOAPReport.js",
                  "~/Scripts/Analysis/Analysis-Common.js"));
            #endregion

            #region Cross Retailer Frequencies
            bundles.Add(new ScriptBundle("~/bundle/scripts/addlcapabilities-Cross_Retailer_Frequencies").Include(
                  "~/Scripts/Analysis/Analysis.js",
                  "~/Scripts/Analysis/Analysis-Common.js"));
            #endregion

            #region Cross Retailer Imageries
            bundles.Add(new ScriptBundle("~/bundle/scripts/addlcapabilities-Cross_Retailer_Imageries").Include(
                  "~/Scripts/Analysis/CRImageries.js",
                  "~/Scripts/Analysis/Analysis-Common.js"));
            #endregion

            #region Within Shopper
            bundles.Add(new ScriptBundle("~/bundle/scripts/addlcapabilities-Within_Shopper").Include(
                  "~/Scripts/Analysis/CompareRetailer.js",
                  "~/Scripts/Analysis/Analysis-Common.js"));
            #endregion

            #region Within Trips
            bundles.Add(new ScriptBundle("~/bundle/scripts/addlcapabilities-Within_Trips").Include(
                  "~/Scripts/Analysis/RetailerDeepDive.js",
                  "~/Scripts/Analysis/Analysis-Common.js"));
            #endregion
            #endregion

            #region dashboard bundles
            #region Demographic
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/dashboard-Demographic").Include(
                              "~/Content/Dashboard/Demographic.css"));
            #endregion

            #region script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/dashboard-Demographic").Include(
                             "~/Scripts/Dashboard/Demographic.js"));
            #endregion
            #endregion

            #region PathToPurchase
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/dashboard-PathToPurchase").Include(
                              "~/Content/Dashboard/PathToPurchase.css"));
            #endregion

            #region script bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/dashboard-PathToPurchase").Include(
                             "~/Scripts/Dashboard/PathToPurchase.js"));
            #endregion
            #endregion
            #endregion

            #region E-com chart bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/Ecom-chart").Include(
                   "~/Content/Charts/Charts.css"));
            #endregion

            #region Compare Sites scrips bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/Ecom-chart-Compare_Sites").Include(
                  "~/Scripts/Charts/Chart-Common.js",
                  "~/Scripts/E_Commerce_Chart/CompareSites.js"));
            #endregion

            #region Site Deep Dive scrips bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/Ecom-chart-Site_Deep_Dive").Include(
                  "~/Scripts/Charts/Chart-Common.js",
                  "~/Scripts/E_Commerce_Chart/SiteDeepDive.js"));
            #endregion
            #endregion

            #region E-com table bundles
            #region style bundles
            bundles.Add(new StyleBundle("~/bundle/styles/Ecom-table").Include(
                   "~/Content/Tables/Tables.css"));
            #endregion

            #region Compare Sites scrips bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/Ecom-table-Compare_Sites").Include(
                  "~/Scripts/E_Commerce_Table/CompareSites.js",
                  "~/Scripts/E_Commerce_Table/Table-Common.js"));
            #endregion

            #region Site Deep Dive scrips bundles
            bundles.Add(new ScriptBundle("~/bundle/scripts/Ecom-table-Site_Deep_Dive").Include(
                  "~/Scripts/E_Commerce_Table/SiteDeepDive.js",
                  "~/Scripts/E_Commerce_Table/Table-Common.js"));
            #endregion
            #endregion

            #region EstablishmentDeepDive
            bundles.Add(new ScriptBundle("~/bundle/scripts/addlcapabilities-EstablishmentDeepDive").Include(
                  "~/Scripts/Analysis/EstablishmentDeepDive.js"));
            #endregion
            #endregion
            BundleTable.EnableOptimizations = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableOptimizations"]);            
        }
    }
}