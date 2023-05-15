using iSHOPNew.CommonFilters;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSHOPNew.Controllers
{
    //[AuthenticateUser]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        public ActionResult Demographic()
        {
            DataAccess dal = new DataAccess();
            object[] paramvalues = null;
            iSHOP.BLL.UserParams userpara = Session[SessionVariables.USERID] as iSHOP.BLL.UserParams;
            paramvalues = new object[] { userpara.UserID, userpara.UserName, "Demographic" };
            DataSet ds = dal.GetData_WithIdMapping(paramvalues, "usp_LoadUserSelections");
            PathToPurchaseParams demogDashboardData = new PathToPurchaseParams();

            if (ds != null)
            {
                string[] sellist = Convert.ToString(ds.Tables[0].Rows[0][0]).Split(',');
                demogDashboardData.CustomBase_UniqueId = sellist[0].Split(':')[1];
                demogDashboardData.Comparison_UniqueIds = sellist[1].Split(':')[1];
                demogDashboardData.TimePeriod_UniqueId = sellist[2].Split(':')[1];
                demogDashboardData.ShopperSegment_UniqueId = sellist[3].Split(':')[1];
                demogDashboardData.ShopperFrequency_UniqueId = sellist[4].Split(':')[1];
                demogDashboardData.Sigtype_UniqueId = sellist[5].Split(':')[1];
                demogDashboardData.Sort = sellist[6].Split(':')[1];
                demogDashboardData.TabType = sellist[7].Split(':')[1];
            }
            ViewData["user-selection"] = demogDashboardData;
            return View();
        }
        public ActionResult Brandhealth()
        {
            return View();
        }
        public ActionResult Visits()
        {
            return View();
        }
        public ActionResult PathToPurchase()
        {
            DataAccess dal = new DataAccess();
            object[] paramvalues = null;
            iSHOP.BLL.UserParams userpara = Session[SessionVariables.USERID] as iSHOP.BLL.UserParams;
            paramvalues = new object[] { userpara.UserID, userpara.UserName, "P2P Dashboard" };
            DataSet ds = dal.GetData_WithIdMapping(paramvalues, "usp_LoadUserSelections");
            PathToPurchaseParams pathToPurchaseParams = new PathToPurchaseParams();

            if (ds != null)
            {
                string[] sellist = Convert.ToString(ds.Tables[0].Rows[0][0]).Split(',');
                pathToPurchaseParams.CustomBase_UniqueId = sellist[0].Split(':')[1];
                pathToPurchaseParams.Comparison_UniqueIds = sellist[1].Split(':')[1];
                pathToPurchaseParams.TimePeriod_UniqueId = sellist[2].Split(':')[1];
                pathToPurchaseParams.ShopperSegment_UniqueId = sellist[3].Split(':')[1];
                pathToPurchaseParams.ShopperFrequency_UniqueId = sellist[4].Split(':')[1];
                pathToPurchaseParams.Sigtype_UniqueId = sellist[5].Split(':')[1];
                pathToPurchaseParams.Sort = sellist[6].Split(':')[1];
                if (sellist.Count() >= 8)
                    pathToPurchaseParams.CustomBaseAdvancedFilters = sellist[7].Split(':')[1];

                if (sellist.Count() >= 9)
                    pathToPurchaseParams.CustomBaseAdvancedFilters_UniqueId = sellist[8].Split(':')[1];

                if (sellist.Count() >= 10)
                    pathToPurchaseParams.ShopperSegment = sellist[9].Split(':')[1];

                if (sellist.Count() >= 11)
                    pathToPurchaseParams.CustomBaseShopperFrequency = sellist[10].Split(':')[1];

                if (sellist.Count() >= 12)
                    pathToPurchaseParams.CustomBaseShopperFrequency_UniqueId = sellist[11].Split(':')[1];
            }
            ViewData["user-selection"] = pathToPurchaseParams;
            return View();
        }
        [HttpPost]
        public JsonResult GetPathToPurchaseData(PathToPurchaseParams pathToPurchaseParams)
        {
            PathToPurchaseMetrics pathToPurchaseMetrics = null;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                //log selection
                LogEntries.LogSelection("PATH TO PURCHASE DASHBOARD");
                Demographic demographic = new Demographic();
                PathToPurchase pathToPurchase = new PathToPurchase();
                pathToPurchaseMetrics = pathToPurchase.GetData(pathToPurchaseParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var jsonResult = Json(pathToPurchaseMetrics, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetDemographicData(PathToPurchaseParams demogDashboardData)
        {
            PathToPurchaseMetrics demographicMetrics = null;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                //log selection
                LogEntries.LogSelection("DEMOGRAPHICS DASHBOARD");
                Demographic demographic = new Demographic();
                demographicMetrics = demographic.GetData(demogDashboardData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var jsonRessult = Json(demographicMetrics, JsonRequestBehavior.AllowGet);
            jsonRessult.MaxJsonLength = Int32.MaxValue;
            return jsonRessult;
        }
        [HttpPost]
        public JsonResult SaveUserSelection(PathToPurchaseParams pathToPurchaseParams)
        {
            bool status = false;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                DataAccess dal = new DataAccess();
                object[] paramvalues = null;
                string selection = "Custom-Base:" + pathToPurchaseParams.CustomBase_UniqueId +
                 ",Retailer:" + pathToPurchaseParams.Comparison_UniqueIds +
                 ",Time-Period:" + pathToPurchaseParams.TimePeriod_UniqueId +
                ",Shopper-Segment_uniqueid:" + pathToPurchaseParams.ShopperSegment_UniqueId +
                ",Shopper-Frequency:" + pathToPurchaseParams.ShopperFrequency_UniqueId +
                ",Sigtype:" + pathToPurchaseParams.Sigtype_UniqueId +
                ",Sort:" + pathToPurchaseParams.Sort +
                ",Custom-Base-Dual-Filters:" + pathToPurchaseParams.CustomBaseAdvancedFilters +
                ",Custom-Base-Dual-Filters_UniqueId:" + pathToPurchaseParams.CustomBaseAdvancedFilters_UniqueId +
                ",Shopper-Segment:" + pathToPurchaseParams.ShopperSegment +
                ",Custom-Base-Shopper-Frequency:" + pathToPurchaseParams.CustomBaseShopperFrequency +
                ",Custom-Base-Shopper-Frequency_UniqueId:" + pathToPurchaseParams.CustomBaseShopperFrequency_UniqueId;

                iSHOP.BLL.UserParams userpara = Session[SessionVariables.USERID] as iSHOP.BLL.UserParams;
                paramvalues = new object[] { userpara.UserID, userpara.UserName, selection, "P2P Dashboard" };
                dal.SaveData(paramvalues, "usp_SaveUserSelections");
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                throw ex;
            }
            var jsonResult = Json(status, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult SaveUserSelectionDemo(PathToPurchaseParams demogDashboardData)
        {
            bool status = false;
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                DataAccess dal = new DataAccess();
                object[] paramvalues = null;
                string selection = "Custom-Base:" + demogDashboardData.CustomBase_UniqueId +
                 ",Retailer:" + demogDashboardData.Comparison_UniqueIds +
                 ",Time-Period:" + demogDashboardData.TimePeriod_UniqueId +
                ",Shopper-Segment:" + demogDashboardData.ShopperSegment_UniqueId +
                ",Shopper-Frequency:" + demogDashboardData.ShopperFrequency_UniqueId +
                ",Sigtype:" + demogDashboardData.Sigtype_UniqueId +
                ",Sort:" + demogDashboardData.Sort +
                ",TabType:" + demogDashboardData.TabType;

                iSHOP.BLL.UserParams userpara = Session[SessionVariables.USERID] as iSHOP.BLL.UserParams;
                paramvalues = new object[] { userpara.UserID, userpara.UserName, selection, "Demographic" };
                dal.SaveData(paramvalues, "usp_SaveUserSelections");
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                throw ex;
            }
            var jsonResult = Json(status, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }
        //To download ppt
        public FileResult DownloadFullDashboardPDF(string path)
        {
            LogEntries.LogSelection("PATH TO PURCHASE DASHBOARD");
            return File(Server.MapPath(path), "application/vnd.openxmlformats-officedocument.presentationml.presentation", FileNamingConventn("P2P Dashboard") + ".pdf");
        }
        public FileResult DownloadFullDashboardPPT(string path)
        {
            LogEntries.LogSelection("PATH TO PURCHASE DASHBOARD");
            return File(Server.MapPath(path), "application/vnd.openxmlformats-officedocument.presentationml.presentation", FileNamingConventn("P2P Dashboard") + ".pptx");
        }

        public FileResult DownloadFullDemogDashboardPPT(string path)
        {
            LogEntries.LogSelection("DEMOGRAPHICS DASHBOARD");
            return File(Server.MapPath(path), "application/vnd.openxmlformats-officedocument.presentationml.presentation", FileNamingConventn("Demographics Dashboard") + ".pptx");
        }

        //To download ppt
        public FileResult DownloadFullDemogDashboardPDF(string path)
        {
            LogEntries.LogSelection("DEMOGRAPHICS DASHBOARD");
            return File(Server.MapPath(path), "application/vnd.openxmlformats-officedocument.presentationml.presentation", FileNamingConventn("Demographics Dashboard") + ".pdf");
        }
        [HttpPost]
        public string PopupExportDashboard(P2PPopupDashboardData pathToPurchas)
        {
            PathToPurchase pathToPurchase = new PathToPurchase();
            try
            {
                string filepath = Server.MapPath("~/Templates/P2PDashboard_Popup");
                string destFile = Server.MapPath("~/Temp/ExportedDashboardPPT" + Session.SessionID + ".pdf");
                pathToPurchase.PopupExportDashboard(filepath, destFile, pathToPurchas, this.HttpContext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "~/Temp/ExportedDashboardPPT" + Session.SessionID + ".pdf";
        }
        public static string FileNamingConventn(string filename)
        {
            string fileNamingConventn = "";
            fileNamingConventn = "Ishop " + filename + "_" + System.DateTime.Now.DayOfWeek.ToString().Substring(0, 3) + " " + System.DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + System.DateTime.Today.Day + " " + System.DateTime.Today.Year + "" + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
            return fileNamingConventn;
        }

        //To export full dashboard ppt
        [HttpPost]
        public string ExportToFullDashboardPPT(P2PDashboardData p2PDashboardData)
        {
            PathToPurchase pathToPurchase = new PathToPurchase();
            string filepath = Server.MapPath("~/Templates/P2PDashboard.pptx");
            string destFile = Server.MapPath("~/Temp/ExportedDashboardPPT" + Session.SessionID + ".pdf");
            pathToPurchase.ExportToFullDashboardPPT(filepath, destFile, p2PDashboardData, this.HttpContext);
            return "~/Temp/ExportedDashboardPPT" + Session.SessionID + ".pdf";
        }

        [HttpPost]
        public string ExportToDemogDashboardPPT(P2PDashboardData demogDashboardData)
        {
            Demographic demographic = new Demographic();
            string filepath = Server.MapPath("~/Templates/DemogDashboard.pptx");
            string destFile = Server.MapPath("~/Temp/ExportedDemogDashboardPPT" + Session.SessionID + ".pdf");
            demographic.ExportToDemogDashboardPPT(filepath, destFile, demogDashboardData, this.HttpContext);
            return "~/Temp/ExportedDemogDashboardPPT" + Session.SessionID + ".pdf";
        }

    }
}
