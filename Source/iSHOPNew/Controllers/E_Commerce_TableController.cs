﻿using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Services;
using System.Data;
using iSHOPNew.CommonFilters;

namespace iSHOPNew.Controllers
{
    [AuthenticateUser]
    public class E_Commerce_TableController : Controller
    {
        Models.TableBase table = new Models.ECommerceTable();
        iSHOPParams param = new iSHOPParams();
        iSHOPParams exportparam = new iSHOPParams();
        //
        // GE
        public ActionResult Index()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }

        public ActionResult CompareSites()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }
        public ActionResult SiteDeepDive()
        {
            CommonFunctions.AuthenticateUser();
            return View();
        }

        public JsonResult GetTable(List<TableParams> tableParams)
        {
           if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                //log selection
                LogEntries.LogSelection("TABLES");

                Dictionary<string, string> exportfiles = new Dictionary<string, string>();
                Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
                List<string> sheetlist = new List<string>();
                try
                {
                    foreach (TableParams tblparam in tableParams)
                    {
                        StringBuilder tbltext = new StringBuilder();
                        string xmlstring = string.Empty;
                        string checksamplesizesp = tblparam.CheckSampleSize_SPName.ToMyString();
                        string _BenchMark = tblparam.Comparison_DBNames[0].ToMyString();
                        //tblparam.Comparison_DBNames.RemoveAt(0);
                        string[] _Comparisonlist = tblparam.Comparison_DBNames.ToArray();
                        string timePeriod = tblparam.TimePeriod.ToMyString();
                        string _ShopperSegment = tblparam.ShopperSegment.ToMyString();
                        string filterShortname = tblparam.FilterShortname.ToMyString();
                        string _ShopperFrequency = tblparam.ShopperFrequency.ToMyString();
                        string[] ShortNames = tblparam.Comparison_ShortNames.ToArray();
                        string StatPositive = Convert.ToString(Session["StatSessionPosi"]);
                        string StatNegative = Convert.ToString(Session["StatSessionNega"]);
                        bool ExportToExcel = tblparam.IsExportToExcel;
                        string TimePeriodShortName = tblparam.TimePeriodShortName;
                        string ulwidth = "381";
                        string ulliwidth = "381";
                        string Selected_StatTest = tblparam.StatTest;
                        param = table.BindTabs(out tbltext, out xmlstring, checksamplesizesp, tblparam.Main_SPName, _BenchMark, _Comparisonlist, timePeriod, _ShopperSegment, filterShortname, _ShopperFrequency, ShortNames, StatPositive, StatNegative, ExportToExcel, TimePeriodShortName, ulwidth, ulliwidth, Selected_StatTest, tblparam.TabName, tblparam);
                        if (tblparam.IsExportToExcel)
                        {
                            exportfiles.Add("tab" + (tableParams.IndexOf(tblparam) + 1), xmlstring);
                            sheetlist.Add(tblparam.TabName);
                            System.Web.HttpContext.Current.Session["exportfiles"] = exportfiles;
                            System.Web.HttpContext.Current.Session["ExportSheetNames"] = sheetlist;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                var jsonResult = Json(param, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;           
        }
        public JsonResult GetTrendTable(List<TableParams> tableParams)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                //log selection
                LogEntries.LogSelection("TABLES");
                Dictionary<string, string> exportfiles = new Dictionary<string, string>();
                Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
                List<string> sheetlist = new List<string>();
                try
                {
                    foreach (TableParams tblparam in tableParams)
                    {
                        StringBuilder tbltext = new StringBuilder();
                        string xmlstring = string.Empty;
                        string checksamplesizesp = tblparam.CheckSampleSize_SPName.ToMyString();
                        string _BenchMark = tblparam.Comparison_DBNames[0].ToMyString();
                        tblparam.Comparison_DBNames.RemoveAt(0);
                        string[] _Comparisonlist = tblparam.Comparison_DBNames.ToArray();
                        string timePeriod = tblparam.TimePeriod.ToMyString();
                        string _ShopperSegment = tblparam.ShopperSegment.ToMyString();
                        string _SingleSelection = tblparam.SingleSelection.ToMyString();
                        string _filter = tblparam.Filter.ToMyString();
                        string filterShortname = tblparam.FilterShortname.ToMyString();
                        string _ShopperFrequency = tblparam.ShopperFrequency.ToMyString();
                        string[] ShortNames = tblparam.Comparison_ShortNames.ToArray();
                        string StatPositive = "1.96";
                        string StatNegative = "-1.96";
                        bool ExportToExcel = tblparam.IsExportToExcel;
                        string TimePeriodShortName = tblparam.TimePeriodShortName.ToMyString();
                        string ulwidth = "381";
                        string ulliwidth = "381";
                        string Selected_StatTest = tblparam.StatTest;
                        param = table.BindTabsTimePeriod(out tbltext, out xmlstring, checksamplesizesp, tblparam.Main_SPName, _BenchMark, _Comparisonlist, _ShopperSegment, _SingleSelection, _ShopperFrequency, _filter, filterShortname, ShortNames, StatPositive, StatNegative, tblparam.IsExportToExcel, TimePeriodShortName, ulwidth, ulliwidth, Selected_StatTest, tblparam.TabName, tblparam);
                        if (tblparam.IsExportToExcel)
                        {
                            exportfiles.Add("tab" + (tableParams.IndexOf(tblparam) + 1), xmlstring);
                            sheetlist.Add(tblparam.TabName);
                            System.Web.HttpContext.Current.Session["exportfiles"] = exportfiles;
                            System.Web.HttpContext.Current.Session["ExportSheetNames"] = sheetlist;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                var jsonResult = Json(param, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;           
        }
        public JsonResult GetWithinTable(List<TableParams> tableParams)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                //log selection
                LogEntries.LogSelection("TABLES");
                Dictionary<string, string> exportfiles = new Dictionary<string, string>();
                Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
                List<string> sheetlist = new List<string>();
                try
                {
                    foreach (TableParams tblparam in tableParams)
                    {
                        StringBuilder tbltext = new StringBuilder();
                        string xmlstring = string.Empty;
                        string checksamplesizesp = tblparam.CheckSampleSize_SPName.ToMyString();
                        string _BenchMark = tblparam.Comparison_DBNames[0].ToMyString();
                        //tblparam.Comparison_DBNames.RemoveAt(0);
                        string[] _Comparisonlist = tblparam.Comparison_DBNames.ToArray();
                        string timePeriod = tblparam.TimePeriod.ToMyString();
                        string _ShopperSegment = tblparam.ShopperSegment.ToMyString();
                        string _SingleSelection = tblparam.SingleSelection.ToMyString();
                        string _filter = tblparam.Filter.ToMyString();
                        string filterShortname = tblparam.FilterShortname.ToMyString();
                        string _ShopperFrequency = tblparam.ShopperFrequency.ToMyString();
                        string[] ShortNames = tblparam.Comparison_ShortNames.ToArray();
                        string StatPositive = "1.96";
                        string StatNegative = "-1.96";
                        bool ExportToExcel = tblparam.IsExportToExcel;
                        string TimePeriodShortName = tblparam.TimePeriodShortName.ToMyString();
                        string ulwidth = "381";
                        string ulliwidth = "381";
                        string Selected_StatTest = tblparam.StatTest.ToMyString();
                        param = table.BindTabsWithin(out tbltext, out xmlstring, checksamplesizesp, tblparam.Main_SPName, _BenchMark, _Comparisonlist, timePeriod, _ShopperSegment, _SingleSelection, _ShopperFrequency, _filter, filterShortname, ShortNames, StatPositive, StatNegative, tblparam.IsExportToExcel, TimePeriodShortName, ulwidth, ulliwidth, Selected_StatTest, tblparam.TabName, tblparam);
                        if (tblparam.IsExportToExcel)
                        {
                            exportfiles.Add("tab" + (tableParams.IndexOf(tblparam) + 1), xmlstring);
                            sheetlist.Add(tblparam.TabName);
                            System.Web.HttpContext.Current.Session["exportfiles"] = exportfiles;
                            System.Web.HttpContext.Current.Session["ExportSheetNames"] = sheetlist;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                var jsonResult = Json(param, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;           
        }
        #region Excel Export GetData
        public JsonResult GetAccrossRetailerData(TableParams tableParams)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "Redirect", url = Url.Action("SignOut", "Home") });

                try
                {
                    StringBuilder tbltext = new StringBuilder();
                    string xmlstring = string.Empty;
                    StringBuilder currenthtmlstring = new StringBuilder();
                    string checksamplesizesp = tableParams.CheckSampleSize_SPName.ToMyString();
                    string _BenchMark = tableParams.Comparison_DBNames[0].ToMyString();
                    tableParams.Comparison_DBNames.RemoveAt(0);
                    List<string> _Comparisonlist = tableParams.Comparison_DBNames.ToList();
                    if (_Comparisonlist.Count <= 0)
                        _Comparisonlist.Add(_BenchMark);
                    string timePeriod = tableParams.TimePeriod.ToMyString();
                    string _ShopperSegment = tableParams.ShopperSegment.ToMyString();
                    string filter = tableParams.Filter.ToMyString();
                    string filterShortname = tableParams.FilterShortname.ToMyString();
                    string _ShopperFrequency = tableParams.ShopperFrequency.ToMyString();
                    string[] ShortNames = tableParams.Comparison_ShortNames.ToArray();
                    string StatPositive = "1.96";
                    string StatNegative = "-1.96";
                    string ExportToExcel = "true";//tableParams.IsExportToExcel;
                    string TimePeriodShortName = tableParams.TimePeriodShortName;
                    string ulwidth = "381";
                    string ulliwidth = "381";
                    string Selected_StatTest = tableParams.StatTest;

                    List<string> ExportSheetList = tableParams.ExportSheetList.ToList();
                    List<string> ExportSheetNames = tableParams.ExportSheetNames.ToList();
                    List<string> Mainspnames = tableParams.Mainspnames.ToList();
                    List<string> SampleSpnames = tableParams.SampleSpnames.ToList();

                    //AccrossWithinRetailer factSheetManager = new AccrossWithinRetailer();
                    Dictionary<string, string> exportfiles = new Dictionary<string, string>();
                    Dictionary<string, int> sharedStrings = new Dictionary<string, int>();

                    //Within
                    var sSingleSelection = tableParams.SingleSelection;

                    if (ExportToExcel == "true")
                    {
                        exportfiles = new Dictionary<string, string>();
                        sharedStrings = new Dictionary<string, int>();
                        for (int i = 0; i < ExportSheetList.Count(); i++)
                        {
                            if (Mainspnames[i].IndexOf("Within") > 0)
                                param = table.BindTabsWithin(out tbltext, out xmlstring, SampleSpnames[i], Mainspnames[i], _BenchMark, _Comparisonlist.ToArray(), timePeriod, _ShopperSegment, sSingleSelection, _ShopperFrequency, filter, filterShortname, ShortNames, StatPositive, StatNegative, tableParams.IsExportToExcel, TimePeriodShortName, ulwidth, ulliwidth, Selected_StatTest, ExportSheetList[i], tableParams);
                            else
                                param = table.BindTabs(out tbltext, out xmlstring, SampleSpnames[i], Mainspnames[i], _BenchMark, _Comparisonlist.ToArray(), timePeriod, _ShopperSegment, filterShortname, _ShopperFrequency, ShortNames, StatPositive, StatNegative, true, TimePeriodShortName, ulwidth, ulliwidth, Selected_StatTest, ExportSheetList[i], tableParams);
                            exportfiles.Add("tab" + (i + 1), xmlstring);

                            currenthtmlstring = tbltext;
                            exportparam = param;

                        }
                        List<string> sheetlist = ExportSheetNames.ToList();
                        System.Web.HttpContext.Current.Session["exportfiles"] = exportfiles;
                        System.Web.HttpContext.Current.Session["ExportSheetNames"] = sheetlist;
                        tbltext = currenthtmlstring;
                    }

                    //param = table.BindTabs(out tbltext, out xmlstring, GetSPName(retailertypeid), GetSPName(ExportSheetList[i]), _BenchMark, Comparisonlist, timePeriod, _ShopperSegment, filterShortname, _ShopperFrequency, ShortNames, StatPositive, StatNegative, "true", TimePeriodShortName, ulwidth, ulliwidth, Selected_StatTest, ExportSheetList[i]);
                }
                catch (Exception ex)
                {

                }
                var jsonResult = Json(param, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;           
        }
        #endregion
        public JsonResult DownloadExcel()
        {
            TableExportToExcel download = new Models.TableExportToExcel();
            download.DownloadReport();
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }
        public void DownloadExcelFile()
        {
            string hdnyear = Convert.ToString(Request.QueryString["year"]);
            string hdnmonth = Convert.ToString(Request.QueryString["month"]);
            string hdndate = Convert.ToString(Request.QueryString["date"]);
            string hdnhours = Convert.ToString(Request.QueryString["hours"]);
            string hdnminutes = Convert.ToString(Request.QueryString["minutes"]);
            string hdnseconds = Convert.ToString(Request.QueryString["seconds"]);

            byte[] btFile = Session["FileStreamByte"] as byte[];
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=iShop_Explorer_" + hdnyear + "" + CommonFunctions.FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + CommonFunctions.FormateDateAndTime(Convert.ToString(hdndate)) + "_" + CommonFunctions.FormateDateAndTime(Convert.ToString(hdnhours)) + "" + CommonFunctions.FormateDateAndTime(Convert.ToString(hdnminutes)) + CommonFunctions.FormateDateAndTime(Convert.ToString(hdnseconds)) + ".xlsx");
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Length", btFile.Length.ToString());
            System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
            System.Web.HttpContext.Current.Response.BinaryWrite(btFile);
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
        }
    }
}

