using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using iSHOPNew.CommonFilters;
using iSHOPNew.BusinessLayer.Exports;
using System.Data;

namespace iSHOPNew.Controllers
{
      //[AuthenticateUser]
    public class AnalysisController : Controller
    {
        Models.Analysis Analysis = new Models.Analysis();
        public ActionResult AcrossShopper()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult AcrossTrips()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult WithinShopper()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult WithinTrips()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult EstablishmentDeepDive()
        {
            return View();
        }
        public ActionResult CrossRetailerFrequencies()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult CrossRetailerImageries()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public JsonResult GetRetailersData(CrossFrequenciesParams param)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

            //log selection
            LogEntries.LogSelection("ADD’L CAPABILITIES");

            string htmltext = string.Empty;
            CRShopping perception = new CRShopping();
            htmltext = perception.GetRetailers(param.Retailer.ToMyString(), param.timePeriod, param.shortTimePeriod, param.isChange.ToMyString(), param.width.ToString(), Convert.ToInt32(param.height), param.filter.ToMyString(), param.TimePeriod_UniqueId.ToMyString(), param.Comparison_UniqueIds.ToMyString(), param.ShopperSegment_UniqueId.ToMyString());
            return Json(htmltext, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRetailersPerceptionsData(CRImageries param)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });
                //log selection
                LogEntries.LogSelection("ADD’L CAPABILITIES");
                string htmltext = string.Empty;
                CRPerceptionData perception = new CRPerceptionData();
                iSHOPParams paramSub = new iSHOPParams();
                paramSub = perception.GetPerceptionsData(param.BenchMark, param.Compare.ToMyString(), param.timePeriod, param.shortTimePeriod, param.ShopperFrequencyShort, param.ShopperFrequency, param.isChange, (Convert.ToString(param.width).Replace("'", "")), Convert.ToInt16(Convert.ToString(param.height).Replace("'", "")), param.filter.ToMyString(), param.Selected_StatTest.ToMyString(), param.TimePeriod_UniqueId.ToMyString(), param.Benchmark_UniqueIds.ToMyString(), param.Comparison_UniqueIds.ToMyString(), param.ShopperFrequency_UniqueId.ToMyString(), param.ShopperSegment_UniqueId.ToMyString(), param.Sigtype_UniqueId.ToMyString(), param.CustomBase_UniqueId.ToMyString(), param.CustomBase_ShortName.ToMyString(), param.Comparison_ShortNames.ToMyString());
                return Json(paramSub, JsonRequestBehavior.AllowGet);   

        }
        public JsonResult GetISHOPBGMData(BGMParamsNew param)
        {
           if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });
            //log selection
            LogEntries.LogSelection("ADD’L CAPABILITIES");
            iSHOPParams ishopParams = new iSHOPParams();
            BGM objBGM = new BGM();
            ishopParams = objBGM.PlotData(param.BenchMark, param.timePeriod, param.previoustimePeriod, param.ShopperFrequency.ToMyString(), param.selectionBevorNonBev, param.filter.ToMyString(), param.timeType, param.BevorNonBevShortName.ToMyString(), param.SelectedBevorNonBevShortName, param.BenchmarkShortName, param.FilterShortNames.ToMyString(),param.TimePeriod_UniqueId,param.Comparison_UniqueIds[0].ToString(),param.Beverage_UniqueId,param.Filter_UniqueId,param.ShopperFrequency_UniqueId, param.BeverageNonBeveragelist);
            var jsonResult = Json(ishopParams, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
            
        }
        public void ExportExcel_BGM()
        {
            ExportToExcelBGM ete = new ExportToExcelBGM();
            ete.ExportToExcel("Export To Excel", Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
           
        }
        public void ExportExcel_Imageries()
        {
            ExportToExcelCrossRetailerImageries ete = new ExportToExcelCrossRetailerImageries();
            ete.ExportToExcel("Export To Excel", Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));

        }
        public void ExportExcel_CossRetailerImageries()
        {
            ExportToExcelCrossRetailerFrequencies ete = new ExportToExcelCrossRetailerFrequencies();
            ete.ExportToExcel("Export To Excel", Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));

        }
        public void Export_To_PPT()
        {
            if (Session["BGMData"] != null)
            {
                BGMPPTDownload bGMPPTDownload = new BGMPPTDownload();
                bGMPPTDownload.GeneratePPT(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            }
        }
        public void GetISHOPSOAPData(SOAPParams param)
        {
            //log selection
            LogEntries.LogSelection("ADD’L CAPABILITIES");
            Analysis.GetSOAPData(param.TimePeriod,param.ShortTimePeriod,param.ShopperSegment,param.ShopperSegmentShortName,param.Geography,param.GeographyShortName,param.ShopperGroup,param.ShopperFrequency,param.ShoppingFrequencyShortname,param.Filters,param.FilterShortNames, param.TimePeriod_UniqueId.ToMyString(), param.Comparison_UniqueIds.ToMyString(), param.Geography_UniqueId.ToMyString(), param.Group_UniqueId.ToMyString(), param.Filter_UniqueId.ToMyString(), param.ShopperFrequency_UniqueId.ToMyString());
        }
        public void SOAPppt(TimeDetails TimeDetails)
        {
            Models.SOAPBase sOAPBase = new Models.SOAPBase();
            sOAPBase.GeneratePPTReport(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
        }
        public JsonResult StoreChartInput(AdvancedAnalyticsParams AdvancedAnalyticsParams)
        {

           if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                CorrespondenceParams correspondenceParams = null;
                Models.AdvancedAnalyticsChartArea sOAPBase = new Models.AdvancedAnalyticsChartArea();
                correspondenceParams = sOAPBase.StoreChartInputSelection(AdvancedAnalyticsParams.ChartType, AdvancedAnalyticsParams.ActiveTab, AdvancedAnalyticsParams.Benchmark, AdvancedAnalyticsParams.Comparisonlist, AdvancedAnalyticsParams.TimePeriod, AdvancedAnalyticsParams.ShortTimePeriod, AdvancedAnalyticsParams.Filters.ToMyString(), AdvancedAnalyticsParams.ShortNames.ToMyString(), AdvancedAnalyticsParams.FrequencyTitle, AdvancedAnalyticsParams.ShopperFrequency, AdvancedAnalyticsParams.ShopperFrequencyShortName, AdvancedAnalyticsParams.Metric, AdvancedAnalyticsParams.MetricShortName, AdvancedAnalyticsParams.ModuleBlock, AdvancedAnalyticsParams.FilterShortNames.ToMyString(), AdvancedAnalyticsParams.ShopperSegment.ToMyString(), AdvancedAnalyticsParams.ChartHeight, AdvancedAnalyticsParams.ChartWidth, AdvancedAnalyticsParams.View, AdvancedAnalyticsParams.SelectedMetrics, AdvancedAnalyticsParams.ComparisonItems.ToArray(), AdvancedAnalyticsParams.StoreidItems.ToArray(), AdvancedAnalyticsParams.ComparisonShortNameItems.ToArray(), AdvancedAnalyticsParams.Comparison_UniqueIds.ToMyString(), AdvancedAnalyticsParams.TimePeriod_UniqueId.ToMyString(), AdvancedAnalyticsParams.ShopperSegment_UniqueId.ToMyString(), AdvancedAnalyticsParams.ShopperFrequency_UniqueId.ToMyString(), AdvancedAnalyticsParams.MetricUniqueId.ToMyString(), AdvancedAnalyticsParams.GroupUniqueId.ToMyString(), AdvancedAnalyticsParams.ViewType.ToMyString());
                return Json(correspondenceParams, JsonRequestBehavior.AllowGet);           
        }
        public void Get_Check_Download_Data()
        {
            if (Session["correspondenceData"] == null || Session["AdvancedAnalyticsInputSelection"] == null)
            {
                AdvancedAnalyticsChartArea advancedAnalyticsChartArea = new AdvancedAnalyticsChartArea();
                advancedAnalyticsChartArea.Check_Download_Data();
            }
        }
        public void Export_To_Excel_ForAdvancedAnalysis()
        {
            if (Session["correspondenceData"] != null || Session["AdvancedAnalyticsInputSelection"] != null)
            {
                DownloadCorrespondenceExcel bGMPPTDownload = new DownloadCorrespondenceExcel();
                bGMPPTDownload.ExportToExcel(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            }
        }
        public void Export_To_PPT_ForAdvancedAnalysis()
        {
            if (Session["correspondenceData"] != null || Session["AdvancedAnalyticsInputSelection"] != null)
            {
                DownloadCorrespondencePPT aPPTDownload = new DownloadCorrespondencePPT();
                aPPTDownload.ExportToPPT(Convert.ToString(Request.QueryString["year"]), Convert.ToString(Request.QueryString["month"]), Convert.ToString(Request.QueryString["date"]), Convert.ToString(Request.QueryString["hours"]), Convert.ToString(Request.QueryString["minutes"]), Convert.ToString(Request.QueryString["seconds"]));
            }
        }
        [HttpPost]
        public string ExportToFullEstablishmentDeepDivePPT(EstablishmentDeepDiveParams establishmentDeepDive)
        {
            EstablishmentDeepDiveParams establishmentDeepDiveParams = new EstablishmentDeepDiveParams();
            EstablishmentDeepDiveData establishmentDeepDiveData = new EstablishmentDeepDiveData();
            return establishmentDeepDiveData.prepareSlidesForPPTDownload(establishmentDeepDive);     
        }
        public FileResult DownloadFullEstablishmentDeepDivePPT(string path)
        {
            LogEntries.LogSelection("ESTABLISHMENT DEEP DIVE");
            return File(Server.MapPath(path), "application/vnd.openxmlformats-officedocument.presentationml.presentation", FileNamingConventn("EstablishmentDeepDive") + ".pptx");
        }

        public static string FileNamingConventn(string filename)
        {
            string fileNamingConventn = "";
            fileNamingConventn = "Ishop " + filename + "_" + System.DateTime.Now.DayOfWeek.ToString().Substring(0, 3) + " " + System.DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + System.DateTime.Today.Day + " " + System.DateTime.Today.Year + "" + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
            return fileNamingConventn;
        }
        [HttpPost]
        public JsonResult GetEstablishmentDeepDiveData(EstablishmentDeepDiveParams establishmentDeepDiveParams)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

            List<EstablishmentDeepDiveMetrics> establishmentDeepDiveMetrics = null;
            //log selection
            LogEntries.LogSelection("ADD’L CAPABILITIES");
            EstablishmentDeepDiveData establishmentDeepDiveData = new EstablishmentDeepDiveData();
            establishmentDeepDiveMetrics = establishmentDeepDiveData.GetEstablishmentDeepDiveData(establishmentDeepDiveParams);
            return Json(establishmentDeepDiveMetrics, JsonRequestBehavior.AllowGet);

        }
    }
}
