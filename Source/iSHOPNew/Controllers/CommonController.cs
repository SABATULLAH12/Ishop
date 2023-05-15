using iSHOPNew.CommonFilters;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace iSHOPNew.Controllers
{  
    public class CommonController : Controller
    {
        iSHOPParams param = new iSHOPParams();
        Table table = new Table();
        CommonFunctions common = new CommonFunctions();
        #region GetFilters
        [HttpPost]
        [OutputCache(Duration = int.MaxValue)]
        public JsonResult GetFilters()
        {
            LoadFilters loadFilters = new LoadFilters();
            Filters filters = null;
            try
            {
                filters = loadFilters.GetFilters();
            }
            catch (Exception ex)
            {
                filters = null;
                HttpContext.Response.Cache.SetNoStore();
                HttpContext.Response.Cache.SetNoServerCaching();
            }
            var jsonResult = Json(filters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;           
        }
        #endregion
        #region GetEcommerceFilters
        [HttpPost]
        [OutputCache(Duration = int.MaxValue)]
        public JsonResult GetECommerceFilters()
        {
            LoadECommerceFilters loadFilters = new LoadECommerceFilters();
            Filters filters = null;
            try
            {
                filters = loadFilters.GetFilters();
            }
            catch (Exception ex)
            {
                filters = null;
                HttpContext.Response.Cache.SetNoStore();
                HttpContext.Response.Cache.SetNoServerCaching();
            }
            var jsonResult = Json(filters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;          
        }
        #endregion
        [HttpPost]
        public JsonResult CustomRegionsFunction(GeoCustomDetails GeoCustomDetails)
        {
            //List<PrimaryAdvancedFilter> data = new List<PrimaryAdvancedFilter>();
            List<Models.Filter> data = null;
            try
            {
                //data = common.GetWithinGeographyBenchmarkComparison(GeoCustomDetails.TagName, GeoCustomDetails.TimePeriod, GeoCustomDetails.TimePeriodType, GeoCustomDetails.CheckModule);
                LoadLeftPanelFilters leftpanelfilter = new LoadLeftPanelFilters();
                data = leftpanelfilter.LoadGeography(GeoCustomDetails.TagName, GeoCustomDetails.TimePeriod, GeoCustomDetails.TimePeriodType, GeoCustomDetails.CheckModule, "Default Geography");
            }
            catch (Exception ex) { }
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }
        #region Set Stat test
        public JsonResult SetStatTestValue(string PosiValue, string NegaValue, string Percent)
        {           
                Session["StatSessionPosi"] = PosiValue;
                Session["StatSessionNega"] = NegaValue;
                Session["PercentStat"] = Percent;
                var jsonResult = Json(true, JsonRequestBehavior.AllowGet);
                return jsonResult;          
        }
        public JsonResult GetStatTestValue()
        {           
            //#region Clear default view session values
            //    ClearDefaultViewSessionValues();
            //    #endregion
                StatTest statTest = new StatTest();
                if (string.IsNullOrEmpty(Convert.ToString(Session["PercentStat"])))
                {
                    Session["StatSessionPosi"] = 1.96;
                    Session["StatSessionNega"] = -1.96;
                    Session["PercentStat"] = 95;
                }

                statTest.PosiValue = Convert.ToString(Session["StatSessionPosi"]);
                statTest.NegaValue = Convert.ToString(Session["StatSessionNega"]);
                statTest.Percent = Convert.ToString(Session["PercentStat"]);

                var jsonResult = Json(statTest, JsonRequestBehavior.AllowGet);
                return jsonResult;           
        }
        #endregion
        #region Clear default view session values
        private void ClearDefaultViewSessionValues()
        {
            Session.Remove("ProfilerChartExportList");
            Session.Remove("ProfilerChartData");
        }
        #endregion
        #region Log Selection
        public JsonResult LogSelection(string Module)
        {
                bool logstatus = LogEntries.LogSelection(Module);
                var jsonResult = Json(logstatus, JsonRequestBehavior.AllowGet);
                return jsonResult;           
        }
        #endregion
    }
}
