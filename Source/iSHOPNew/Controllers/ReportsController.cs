using AQ.Common.GenerateReport;
using iSHOP.BLL;
using iSHOPNew.BusinessLayer.Reports;
using iSHOPNew.CommonFilters;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ReportsBLL = iSHOPNew.Reports.BusinessLayer;

namespace iSHOPNew.Controllers
{
      [AuthenticateUser]
    public class ReportsController : Controller
    {
        public ActionResult CompareRetailersShoppers()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult RetailersShopperDeepDive()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult CompareRetailersPathToPurchase()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult RetailersPathToPurchaseDeepDive()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult CompareBeveragesMonthlyPlusPurchasers()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult BeverageMonthlyPlusPurchasersDeepDive()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult CompareBeveragesPurchaseDetails()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult BeveragesPurchaseDetailsDeepDive()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }

        public ActionResult TotalRespondentsReports()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult SARReports()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public JsonResult BuildSlides(ReportGeneratorParams reportparams)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

            SampleSizeParams sampleSizeParams = new SampleSizeParams();
            try
            {
                //log selection
                Session.Remove("GenerateReportParams");
                LogEntries.LogSelection("REPORTS");

                reportparams.Benchmark = reportparams.Comparison_DBNames[0].ToMyString().Replace("~", "`");
                reportparams.BenchmarkShortName = reportparams.Comparison_ShortNames[0].ToMyString();
                //reportparams.Comparison_ShortNames.RemoveAt(0);
                reportparams.ComparisonShortNamelist = reportparams.Comparison_ShortNames;
                //reportparams.Comparison_DBNames.RemoveAt(0);

                reportparams.Comparisonlist = String.Join("|", reportparams.Comparison_DBNames).ToMyString().Replace("~", "`");
                reportparams.TimePeriod = reportparams.TimePeriod.ToMyString();
                reportparams.ShortTimePeriod = reportparams.ShortTimePeriod.ToMyString();
                reportparams.Filters = reportparams.Filters.ToMyString();

                reportparams.FrequencyTitle = reportparams.FrequencyTitle.ToMyString();

                reportparams.ShopperFrequency = reportparams.ShopperFrequency.ToMyString();
                reportparams.ShopperFrequencyShortName = reportparams.ShopperFrequencyShortName.ToMyString();
                reportparams.SPName = reportparams.SPName.ToMyString();
                reportparams.SelectedReports = reportparams.SelectedReports;

                if (Convert.ToString(reportparams.ModuleBlock).Equals("AcrossTrips", StringComparison.OrdinalIgnoreCase)
                    || Convert.ToString(reportparams.ModuleBlock).Equals("WithinTrips", StringComparison.OrdinalIgnoreCase)
                    || Convert.ToString(reportparams.ModuleBlock).Equals("TimeTrips", StringComparison.OrdinalIgnoreCase))
                {
                    string filters = Convert.ToString(reportparams.ShopperFrequencyShortName);
                    if (!string.IsNullOrEmpty(reportparams.FilterShortNames))
                        filters += ", " + Convert.ToString(reportparams.FilterShortNames);

                    reportparams.FilterShortNames = reportparams.FilterShortNames = string.IsNullOrEmpty(filters) ? "None" : reportparams.FilterShortNames;
                }
                else
                {
                    reportparams.FilterShortNames = reportparams.FilterShortNames = string.IsNullOrEmpty(reportparams.FilterShortNames) ? "None" : reportparams.FilterShortNames;
                }
                
                reportparams.ShopperSegment = reportparams.ShopperSegment.ToMyString().Replace("~", "`");
                reportparams.ShopperSegmentShortName = reportparams.ShopperSegmentShortName.ToMyString().Replace("~", "`");
                reportparams.ModuleBlock = reportparams.ModuleBlock.ToMyString();
                reportparams.View = reportparams.View.ToMyString();
                Report_Generator reportgenerator = new Report_Generator();

                List<string> ShopperLowSampleSizelist = null;
                List<string> TripsLowSampleSizelist = null;
                List<string> Selected_Reports = null;
                bool GenerateReport = true;
                bool IsUseDirectionally = false;
                bool IsAllLowSampleSizes = false;
                sampleSizeParams.GenerateReport = true;
                List<LowSampleSizeItems> LowSampleSizeItems = new List<DAL.LowSampleSizeItems>();
                List<UseDirectionallyItems> useDirectionallyItems = new List<UseDirectionallyItems>();
                reportparams.ChartDataSet = reportgenerator.BuildSlides(reportparams, out ShopperLowSampleSizelist, out TripsLowSampleSizelist, out Selected_Reports, reportparams.StatTest.ToMyString(), out GenerateReport, out IsUseDirectionally,out LowSampleSizeItems, out useDirectionallyItems, out IsAllLowSampleSizes);
                sampleSizeParams.GenerateReport = GenerateReport;
                reportparams.SelectedReports = Selected_Reports;
                Session["GenerateReportParams"] = reportparams;
                sampleSizeParams.Shopperlist = ShopperLowSampleSizelist;
                sampleSizeParams.Tripslist = TripsLowSampleSizelist;
                sampleSizeParams.IsUseDirectionally = IsUseDirectionally;
                sampleSizeParams.LowSampleSizeItems = LowSampleSizeItems;
                sampleSizeParams.UseDirectionallyItems = useDirectionallyItems;
                sampleSizeParams.IsAllLowSampleSizes = IsAllLowSampleSizes;
                //prepare ppt
                //added by Nagaraju Date: 26-05-2017
                if (GenerateReport)
                PreparePPT(reportparams);
            }
            catch (Exception ex)
            {

            }
            var jsonResult = Json(sampleSizeParams, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;           
        }

        private void PreparePPT(ReportGeneratorParams reportparams)
        {
            string UserExportFileName = string.Empty;
            List<FileDetails> filelist = null;
            GenerateReport gr = new GenerateReport();
            try
            {
                Download_ShopperReportFor_2Retailers CompareRetailers = new Download_ShopperReportFor_2Retailers();
                Download_ShopperReportForWithin Within = new Download_ShopperReportForWithin();
                Download_ShopperReportForOverTime Trend = new Download_ShopperReportForOverTime();

                BevaragesShopperReportGenerator bevaragesShopperReportGenerator = new BevaragesShopperReportGenerator();
                BevaragesTripsReportGenerator bevaragesTripsReportGenerator = new BevaragesTripsReportGenerator();

                BevaragesWithinShopperReportGenerator bevaragesWithinShopperReportGenerator = new BevaragesWithinShopperReportGenerator();
                BevaragesWithinTripsReportGenerator bevaragesWithinTripsReportGenerator = new BevaragesWithinTripsReportGenerator();

                BevaragesTrendShopperReportGenerator bevaragesTrendShopperReportGenerator = new BevaragesTrendShopperReportGenerator();
                BevaragesTrendTripsReportGenerator bevaragesTrendTripsReportGenerator = new BevaragesTrendTripsReportGenerator();

                UserParams userparam = Session[SessionVariables.USERID] as UserParams;
                if (userparam == null)
                    Response.Redirect("~/Home/SignOut");

                if (reportparams.ModuleBlock.Equals("AcrossTrips", StringComparison.OrdinalIgnoreCase))
                {
                    //revamp reports Date:16-05-2018               
                    ReportsBLL.BaseReport baseReport = new ReportsBLL.CompareRetailersPathToPurchase();
                    //end
                    baseReport.PrepareReport(reportparams);
                }
                else if (reportparams.ModuleBlock.Equals("AcrossShopper", StringComparison.OrdinalIgnoreCase))
                {
                    //revamp reports Date:11-05-2018               
                    ReportsBLL.BaseReport baseReport = new ReportsBLL.CompareRetailersShoppers();
                    //end
                    baseReport.PrepareReport(reportparams);
                }
                else if (reportparams.ModuleBlock.Equals("WithinTrips", StringComparison.OrdinalIgnoreCase))
                {
                    ReportsBLL.BaseReport baseReport = new ReportsBLL.RetailersPathToPurchasePIT();
                    baseReport.PrepareReport(reportparams);
                }
                else if (reportparams.ModuleBlock.Equals("WithinShopper", StringComparison.OrdinalIgnoreCase))
                {
                    ReportsBLL.BaseReport baseReport = new ReportsBLL.RetailersShopperPIT();
                    baseReport.PrepareReport(reportparams);
                }
                else if (reportparams.ModuleBlock.Equals("TimeTrips", StringComparison.OrdinalIgnoreCase))
                {
                    ReportsBLL.BaseReport baseReport = new ReportsBLL.RetailersPathToPurchaseTREND();
                    baseReport.PrepareReport(reportparams);
                }
                else if (reportparams.ModuleBlock.Equals("TimeShopper", StringComparison.OrdinalIgnoreCase))
                {
                    ReportsBLL.BaseReport baseReport = new ReportsBLL.RetailersShopperTREND();
                    baseReport.PrepareReport(reportparams);
                }
                else if (reportparams.ModuleBlock.Equals("AcrossBeverageShopper", StringComparison.OrdinalIgnoreCase))
                    bevaragesShopperReportGenerator.PrepareSlides();
                else if (reportparams.ModuleBlock.Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase))
                    bevaragesTripsReportGenerator.PrepareSlides();
                else if (reportparams.ModuleBlock.Equals("WithinBeverageShopper", StringComparison.OrdinalIgnoreCase))
                    bevaragesWithinShopperReportGenerator.PrepareSlides();
                else if (reportparams.ModuleBlock.Equals("WithinBeverageTrips", StringComparison.OrdinalIgnoreCase))
                    bevaragesWithinTripsReportGenerator.PrepareSlides();
                else if (reportparams.ModuleBlock.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase))
                    bevaragesTrendShopperReportGenerator.PrepareSlides();
                else if (reportparams.ModuleBlock.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
                    bevaragesTrendTripsReportGenerator.PrepareSlides();               
            }
            catch (Exception ex)
            { 
            
            }
        }
        public void DeleteDirectory(string fileName)
        {
            if (Directory.Exists(Server.MapPath(fileName)))
                Directory.Delete(Server.MapPath(fileName),true);
        }
        public void CreateDirectory(string fileName)
        {
            if (!Directory.Exists(Server.MapPath(fileName)))
                Directory.CreateDirectory(Server.MapPath(fileName));
        }
        public void Download(string View)
        {
            BevaragesShopperReportGenerator bevaragesShopperReportGenerator = new BevaragesShopperReportGenerator();
            BevaragesTripsReportGenerator bevaragesTripsReportGenerator = new BevaragesTripsReportGenerator();

            BevaragesWithinShopperReportGenerator bevaragesWithinShopperReportGenerator = new BevaragesWithinShopperReportGenerator();
            BevaragesWithinTripsReportGenerator bevaragesWithinTripsReportGenerator = new BevaragesWithinTripsReportGenerator();

            BevaragesTrendShopperReportGenerator bevaragesTrendShopperReportGenerator = new BevaragesTrendShopperReportGenerator();
            BevaragesTrendTripsReportGenerator bevaragesTrendTripsReportGenerator = new BevaragesTrendTripsReportGenerator();

            //for revamp reports
            ReportDownload reportDownload = new BusinessLayer.Reports.ReportDownload();
            //end        

            if (View.Equals("AcrossBeverageShopper", StringComparison.OrdinalIgnoreCase))
                bevaragesShopperReportGenerator.GenerateBeverageReport(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            else if (View.Equals("AcrossBeverageTrips", StringComparison.OrdinalIgnoreCase))
                bevaragesTripsReportGenerator.GenerateBeverageReport(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            else if (View.Equals("WithinBeverageShopper", StringComparison.OrdinalIgnoreCase))
                bevaragesWithinShopperReportGenerator.GenerateBeverageReport(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            else if (View.Equals("WithinBeverageTrips", StringComparison.OrdinalIgnoreCase))
                bevaragesWithinTripsReportGenerator.GenerateBeverageReport(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            else if (View.Equals("TimeBeverageShopper", StringComparison.OrdinalIgnoreCase))
                bevaragesTrendShopperReportGenerator.GenerateBeverageReport(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            else if (View.Equals("TimeBeverageTrips", StringComparison.OrdinalIgnoreCase))
                bevaragesTrendTripsReportGenerator.GenerateBeverageReport(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            else if (View.Equals("AcrossShopper", StringComparison.OrdinalIgnoreCase) || View.Equals("AcrossTrips", StringComparison.OrdinalIgnoreCase) || View.Equals("PIT", StringComparison.OrdinalIgnoreCase) || View.Equals("TREND", StringComparison.OrdinalIgnoreCase))
                reportDownload.Download(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            else
                CommonFunctions.MergeAndDownload_AcrossShoppersPPT(Convert.ToString(Session[SessionVariables.ReportPath]), Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
        }

        public JsonResult GetTotalData(reportparams reportparams)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

            string tbltext = string.Empty;
            string xmlstring = string.Empty;
            string currenthtmlstring = string.Empty;
            iSHOPNew.DAL.iSHOPParams param = new iSHOPNew.DAL.iSHOPParams();
            iSHOPNew.DAL.iSHOPParams exportparam = new iSHOPNew.DAL.iSHOPParams();

            string StatPositive = Session["StatSessionPosi"].ToString();
            string StatNegative = Session["StatSessionNega"].ToString();           

            Total factSheetManager = new Total();
            Dictionary<string, string> exportfiles = new Dictionary<string, string>();
            Dictionary<string, int> sharedStrings = new Dictionary<string, int>();

            {
                try
                {
                    //Nagaraju 27-03-2014              
                    //lock (_lockObj)
                    //{
                    if (reportparams.ExportToExcel == "true")
                    {
                        exportfiles = new Dictionary<string, string>();
                        sharedStrings = new Dictionary<string, int>();
                        for (int i = 0; i < reportparams.ExportSheetList.Count(); i++)
                        {
                            string[] sCompList = (reportparams.Comparisonlist == null || reportparams.Comparisonlist.Count > 0) ? null : reportparams.Comparisonlist.ToArray();
                            param = factSheetManager.BindTabs(out tbltext, out xmlstring, "", reportparams._BenchMark.ToMyString(), sCompList, reportparams.timePeriod.ToMyString(), reportparams._shortTimePeriod.ToMyString(), reportparams._ShopperFrequency.ToMyString(), reportparams._measure.ToMyString(), reportparams._filter.ToMyString(), reportparams.filterShortname.ToMyString(), reportparams.ShortNames.ToArray(), StatPositive.ToMyString(), StatNegative.ToMyString(), "false", reportparams.Selected_StatTest.ToMyString(), reportparams.TimePeriod_UniqueId.ToMyString(), reportparams.BenchmarkUniqueId.ToMyString(), reportparams.Comparison_UniqueIds.ToMyString(), reportparams.ShopperFrequency_UniqueId.ToMyString(), reportparams.ShopperSegment_UniqueId.ToMyString(), reportparams.MeasureUniqueIds.ToMyString(), reportparams.Sigtype_UniqueId.ToMyString(), reportparams.Module.ToMyString(), reportparams.CustomBase_DBName, reportparams.CustomBase_ShortName, reportparams.CustomBase_UniqueId);
                            exportfiles.Add("tab" + (i + 1), xmlstring);
                            if (reportparams.tabid == reportparams.ExportSheetList[i])
                            {
                                currenthtmlstring = tbltext;
                                exportparam = param;

                            }
                        }
                        List<string> sheetlist = reportparams.ExportSheetNames.ToList();
                        System.Web.HttpContext.Current.Session["exportfiles"] = exportfiles;
                        System.Web.HttpContext.Current.Session["ExportSheetNames"] = sheetlist;
                        tbltext = currenthtmlstring;
                    }
                    //End
                    else
                    {
                        string[] sCompList = (reportparams.Comparisonlist == null || reportparams.Comparisonlist.Count > 0) ? null:reportparams.Comparisonlist.ToArray();
                        param = factSheetManager.BindTabs(out tbltext, out xmlstring, "", reportparams._BenchMark.ToMyString(), sCompList, reportparams.timePeriod.ToMyString(), reportparams._shortTimePeriod.ToMyString(), reportparams._ShopperFrequency.ToMyString(), reportparams._measure.ToMyString(), reportparams._filter.ToMyString(), reportparams.filterShortname.ToMyString(), reportparams.ShortNames.ToArray(), StatPositive.ToMyString(), StatNegative.ToMyString(), "false", reportparams.Selected_StatTest.ToMyString(), reportparams.TimePeriod_UniqueId.ToMyString(), reportparams.BenchmarkUniqueId.ToMyString(), reportparams.Comparison_UniqueIds.ToMyString(), reportparams.ShopperFrequency_UniqueId.ToMyString(), reportparams.ShopperSegment_UniqueId.ToMyString(), reportparams.MeasureUniqueIds.ToMyString(), reportparams.Sigtype_UniqueId.ToMyString(), reportparams.Module.ToMyString(), reportparams.CustomBase_DBName, reportparams.CustomBase_ShortName, reportparams.CustomBase_UniqueId);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    //CL.ErrorLog.LogError(ex.Message, ex.StackTrace);
                }
            }

            if (reportparams.ExportToExcel == "true")
            {
                //param = exportparam;
            }

            //return tbltext;            
            var jsonResult = Json(param, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;            
        }
        public void ExportExcel_Total()
        {
            ExcelDownloadTotal ete = new ExcelDownloadTotal();
            List<string> SheetLiat = System.Web.HttpContext.Current.Session["ExportSheetNames"] as List<string>;
            ete.ExportToExcel("Export To Excel",SheetLiat, Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));

        }

        [HttpPost]
        public JsonResult GetCompetitorList(int RetailerId)
        {
            LoadLeftPanelFilters lf = new LoadLeftPanelFilters();
            List<Competitors> _compList = null;
            try
            {
                _compList = lf.GetSarCompetitors(RetailerId);
            }
            catch(Exception ex)
            {
                _compList = null;
                HttpContext.Response.Cache.SetNoStore();
                HttpContext.Response.Cache.SetNoServerCaching();
            }
            var jsonResult = Json(_compList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult validateSampleSize(FilterPanelInfo leftPanelData)
        {
            Report_Generator report = new Report_Generator();
            List<LowSampleSizeRetailerList> _retailerList = new List<LowSampleSizeRetailerList>();
            try
            {
                _retailerList = report.GetRetailerSampleSize(leftPanelData,this.HttpContext);
            }
            catch (Exception ex)
            {
                _retailerList = null;
                HttpContext.Response.Cache.SetNoStore();
                HttpContext.Response.Cache.SetNoServerCaching();
            }
            var jsonResult = Json(_retailerList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public async Task<ActionResult> downloadSarReport(FilterPanelInfo leftPanelData, List<string> spList)
        {
            string filepath = string.Empty;

            filepath = Server.MapPath("~/Templates/Reports/sarTemplate.pptx");

            SARReport _report = new SARReport();
            List<string> spNames = new List<string>() { "abc" };
            try
            {
                await _report.PrepareSARReport(filepath, spList, this.HttpContext, leftPanelData);
            }
            catch (Exception ex)
            {
                ErrorLogSar.LogError(ex.Message + "   when calling downloadSarReport", ex.StackTrace, this.HttpContext);
            }
            LogEntries.LogSelection("REPORTS - Briefing Book");//Added By Bramhanath for User Tracking(16-05-2017)
            ErrorLogSar.LogError("Sar Report download Complete---url:" + "~/Temp/PPT/ExportedReportPPT" + this.HttpContext.Session.SessionID + ".pptx", "", this.HttpContext);
            //return "~/Temp/PPT/ExportedReportPPT" + this.HttpContext.Session.SessionID + ".pptx";
            return Json("~/Temp/PPT/ExportedReportPPT" + this.HttpContext.Session.SessionID + ".pptx",JsonRequestBehavior.AllowGet);
        }
        public FileResult DownloadSARReportFile(string path)
        {
            //LogEntries.LogSelection("REPORTS");//Added By Bramhanath for User Tracking(16-05-2017)
            return File(Server.MapPath(path), "application/vnd.openxmlformats-officedocument.presentationml.presentation", SARReport.FileNamingConventn("Briefing Book") + ".pptx");
        }
    }
}
