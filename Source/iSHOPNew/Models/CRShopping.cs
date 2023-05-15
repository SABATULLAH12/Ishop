using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using iSHOPNew.DAL;

namespace iSHOPNew.Models
{
    public class CRShopping
    {
        public Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        public List<string> mergeCell = new List<string>();
        public int WeeklyPlusSampleSize = 0;
        public int MonthlyPlusSampleSize = 0;
        public int PastThreeMonthsSampleSize = 0;
        bool IsNA = false;
        bool IsApplicable = true;
        public string GetRetailers(string _Retailer, string _timePeriod, string _shortTimePeriod, string isChange, string _width, int _height, string _filter, string TimePeriod_UniqueId, string Comparison_UniqueIds, string ShopperSegment_UniqueId)
   {
            string htmltext = string.Empty;
            string exportdata = string.Empty;
            mergeCell = new List<string>();
            sharedStrings = new Dictionary<string, int>();
            int rownumber = 4;
            var retailer = _Retailer.Split('|').ToList();
            string Retailer = retailer[retailer.Count - 1];
            string width = _width;
            int height = _height;
            string td_width = "23%";//(_width - 420) / 3;
            try
            {
                DAL.DataAccess dal = new DataAccess();
                //object[] paramvalues = new object[] { _Retailer.Replace("~", "`"), _timePeriod, _filter };
                object[] paramvalues = new object[] { Comparison_UniqueIds, TimePeriod_UniqueId, ShopperSegment_UniqueId };
                DataSet ds = null;
                if (isChange == "false" && HttpContext.Current.Session["CRShopping"] != null)
                {
                    ds = HttpContext.Current.Session["CRShopping"] as DataSet;
                }
                else
                {
                    //ds = dal.GetData(paramvalues, "sp_FactBookCrossTabReport");
                    ds = dal.GetData_WithIdMapping(paramvalues, "Usp_CrossRetailerShopping");
                }
                HttpContext.Current.Session["CRShopping"] = ds;
                DataTable tbl = null;
                if (ds != null && ds.Tables.Count > 0)
                {
                    //write header Nagaraju Data: 04-06-2013.
                    exportdata = GetSheetHeadandColumns();
                    exportdata += "<sheetData>";
                    //write filter in excel
                    exportdata += CreateAndCloseRow("create", "1");

                    string retmetricname = ReplaceToApostropheExcel("Retailer/Channel: " + ExcelShortChannel(Retailer.Replace("Channels|", "").Replace("Retailers|", "").Replace("`", "'")));

                    string excelmetricname = ExcelShortChannel(Retailer.Replace("Channels|", "").Replace("Retailers|", "").Replace("`", "'"));


                    retmetricname = cf.cleanExcelXML(retmetricname);
                    CreateSharedStringDate(retmetricname);

                    exportdata += CreateSheetData("B1", "9", GetSharedStringValue(retmetricname), "true");
                    string xmlstring = string.Empty;
                    if (!string.IsNullOrEmpty(_timePeriod))
                    {
                        if (_timePeriod.IndexOf("3MMT") > -1)
                        {
                            xmlstring = "Time Period : " + _timePeriod.Split('|')[1] + " 3MMT";
                        }
                        else if (_timePeriod.IndexOf("total") > -1)
                        {
                            xmlstring = "Time Period : " + _shortTimePeriod;
                        }
                        else
                        {

                            xmlstring = "Time Period : " + _timePeriod.Split('|')[1];
                        }
                    }
                    CreateSharedStringDate(xmlstring);
                    exportdata += CreateSheetData("C1", "9", GetSharedStringValue(xmlstring), "true");
                    exportdata += CreateAndCloseRow("close", "1");

                    //add filters                   
                    exportdata += CreateAndCloseRow("create", "2");
                    xmlstring = "* Filters";
                    CreateSharedStringDate(xmlstring);
                    exportdata += CreateSheetData("B2", "9", GetSharedStringValue(xmlstring), "true");

                    string CustomFilter = cf.GetExcelSortedFilters(_filter);

                    //string[] ss = _filter.Split(new String[] { "|", "|" },
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

                    CreateSharedStringDate(xmlstring);
                    exportdata += CreateSheetData("C2", "9", GetSharedStringValue(xmlstring), "true");
                    exportdata += CreateAndCloseRow("close", "2");
                    //

                    //add sample size note
                    xmlstring = cf.cleanExcelXML("NOTE : GREY FONT = LOW SAMPLE (30-99), BLANK = SAMPLE < 30; NA = NOT APPLICABLE");
                    CreateSharedStringDate(xmlstring);
                    exportdata += CreateAndCloseRow("create", "3");
                    exportdata += CreateSheetData("C3", "9", GetSharedStringValue(xmlstring), "true");
                    exportdata += CreateAndCloseRow("close", "3");
                    //
                    CreateSharedStringDate("Weekly+");
                    CreateSharedStringDate("Monthly+");
                    CreateSharedStringDate("Quarterly+"); // changes from "at least once in the past 3 month" to "Quarterly+"


                    exportdata += CreateAndCloseHeaderRow("create", rownumber.ToString());
                    //Nishanth                               

                    CreateSharedStringDate("Weekly+ \r\n Shoppers Of " + excelmetricname);
                    exportdata += CreateSheetData("C" + rownumber + "", "7", GetSharedStringValue("Weekly+ \r\n Shoppers Of " + excelmetricname), "true");

                    CreateSharedStringDate("Monthly+ \r\n Shoppers Of " + excelmetricname);
                    exportdata += CreateSheetData("D" + rownumber + "", "7", GetSharedStringValue("Monthly+ \r\n Shoppers Of " + excelmetricname), "true");

                    CreateSharedStringDate("Quarterly+ \r\n Shoppers Of " + excelmetricname);
                    exportdata += CreateSheetData("E" + rownumber + "", "7", GetSharedStringValue("Quarterly+ \r\n Shoppers Of " + excelmetricname), "true");
                    exportdata += CreateAndCloseRow("close", rownumber.ToString());
                    //close header

                    rownumber += 1;
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        tbl = ds.Tables[i].Copy();
                        if (tbl != null && tbl.Rows.Count > 0)
                        {
                            if (i == 0)
                            {
                                //write Sample Size
                                htmltext = "<div class=\"topHeaderCRCS\" style=\"width:100%;height: auto;\"><table style=\"width:" + width + ";\"><tr><td style=\"width:28.2%;\" class=\"litopHeaderCRCS\" style=\"width: 32%;\" id=\"ShopperOf\">Shopper Of " + ExcelShortChannel(Retailer) + "<a class=\"table - top - title - bottom - line\" style=\"background-color: #000000;border-radius: 1px 10px 0 0;bottom: 526px;display: block;height: 4px;left: 50px;width: 25px;position: absolute;\"></a></td><td style=\"width:" + td_width + ";\" class=\"litopHeaderCRCS\">Weekly +" +
                                            "</td><td style=\"width:" + td_width + ";\" class=\"litopHeaderCRCS\">Monthly +</td><td style=\"width:" + td_width + ";margin-right: 0;\" class=\"litopHeaderCRCS\">Quarterly +</td></tr>";
                                foreach (DataRow row in tbl.Rows)
                                {
                                    htmltext += "<tr><td style=\"width:28.2%;\" class=\"SampleSize\">Sample Size</td>";
                                    htmltext += "<td style=\"width:23%;\" class=\"SampleSize\">" + CommaSeparatedValues(Convert.ToString(row["Weekly+"])) + "" + CheckLowSampleSize(Convert.ToString(row["Weekly+"]), "Weekly+") + "</td>";
                                    htmltext += "<td style=\"width:23%;\" class=\"SampleSize\">" + CommaSeparatedValues(Convert.ToString(row["Monthly+"])) + "" + CheckLowSampleSize(Convert.ToString(row["Monthly+"]), "Monthly+") + "</td>";
                                    htmltext += "<td style=\"width:23%;margin-right: 0;\" class=\"SampleSize\">" + CommaSeparatedValues(Convert.ToString(row["Quarterly+"])) + "" + CheckLowSampleSize(Convert.ToString(row["Quarterly+"]), "Quarterly+") + "</td>";

                                    //write sample size for export
                                    exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                    CreateSharedStringDate("Sample Size");
                                    exportdata += CreateSheetData("A" + rownumber + "", "3", GetSharedStringValue("Sample Size"), "true");

                                    //Nishanth
                                    exportdata += CreateSheetData("B" + rownumber + "", "2", "", "");

                                    if (WeeklyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "2", CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(row["Weekly+"]), out IsApplicable), "false");
                                    }
                                    //atul new 
                                    else if (WeeklyPlusSampleSize == 2)
                                    {
                                        string metricname = Convert.ToString(row["Weekly+"]) + " (Use Directionally)";
                                        metricname = cf.cleanExcelXML(metricname);
                                        CreateSharedStringDate(metricname);
                                        exportdata += CreateSheetData("C" + rownumber + "", "2", GetSharedStringValue(metricname), "true");
                                    }
                                    else
                                    {
                                        string metricname = (IsNA ? "0" : Convert.ToString(row["Weekly+"])) + " (Low Sample Size)";
                                        metricname = cf.cleanExcelXML(metricname);
                                        CreateSharedStringDate(metricname);
                                        exportdata += CreateSheetData("C" + rownumber + "", "2", GetSharedStringValue(metricname), "true");
                                    }
                                    if (MonthlyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "2", CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(row["Monthly+"]), out IsApplicable), "false");
                                    }
                                    //atul new
                                    else if (MonthlyPlusSampleSize == 2)
                                    {
                                        string metricname = Convert.ToString(row["Monthly+"]) + " (Use Directionally)";
                                        metricname = cf.cleanExcelXML(metricname);
                                        CreateSharedStringDate(metricname);
                                        exportdata += CreateSheetData("D" + rownumber + "", "2", GetSharedStringValue(metricname), "true");
                                    }
                                    else
                                    {
                                        string metricname = (IsNA ? "0" : Convert.ToString(row["Monthly+"])) + " (Low Sample Size)";                                       
                                        metricname = cf.cleanExcelXML(metricname);
                                        CreateSharedStringDate(metricname);
                                        exportdata += CreateSheetData("D" + rownumber + "", "2", GetSharedStringValue(metricname), "true");
                                    }
                                    if (PastThreeMonthsSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "2", CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(row["Quarterly+"]), out IsApplicable), "false");
                                    }
                                    //atul new
                                    else if (PastThreeMonthsSampleSize == 2)
                                    {
                                        string metricname = Convert.ToString(row["Quarterly+"]) + " (Use Directionally)";
                                        metricname = cf.cleanExcelXML(metricname);
                                        CreateSharedStringDate(metricname);
                                        exportdata += CreateSheetData("E" + rownumber + "", "2", GetSharedStringValue(metricname), "true");
                                    }
                                    else
                                    {
                                        string metricname = (IsNA ? "0" : Convert.ToString(row["Quarterly+"])) + " (Low Sample Size)";                                           
                                        metricname = cf.cleanExcelXML(metricname);
                                        CreateSharedStringDate(metricname);
                                        exportdata += CreateSheetData("E" + rownumber + "", "2", GetSharedStringValue(metricname), "true");
                                    }

                                    exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                    rownumber += 1;
                                }
                                htmltext += "</tr></table></div>";
                            }
                            else if (i == 1)
                            {
                                //write Weekly +
                                //htmltext += "<div class=\"dataCRCS\" style=\"max-height:" + height + "px;\">";
                                htmltext += "<div class=\"dataCRCS\" style=\"\">";
                                htmltext += "<table style=\"width:" + width + ";\"><tr class=\"crossRetailerheader\"><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:28.2%;background-color: #D9E1EE;padding-left:40px;position:relative;justify-content:flex-start;\" class=\"WeeklyMonthlyQuarterly\">Weekly+<a class=\"table - top - title - bottom - line\" style=\"background - color: #000000;border-radius: 1px 10px 0 0;bottom: 463px;background-color: #72aaff;display: block;height: 4px;left: 0%;width: 25px;position: absolute;top:83%;\"></a><div class=\"treeview minusIcon\"></div></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;margin-right:0;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td></tr>";

                                exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                exportdata += CreateSheetData("A" + rownumber + "", "7", GetSharedStringValue("Weekly+"), "true");
                                exportdata += CreateSheetData("B" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("C" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("D" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("E" + rownumber + "", "7", "", "");
                                exportdata += CreateAndCloseRow("close", rownumber.ToString());

                                mergeCell.Add("<mergeCell ref = \"A" + rownumber + ":D" + rownumber + "\"/>");

                                foreach (DataRow row in tbl.Rows)
                                {
                                    htmltext += "<tr>";
                                    if(Convert.ToString(row["Flag"]) == "1")
                                        htmltext += "<td style=\"text-align: left;width:20.5%;color:black;margin-right:0px;padding-left:50px;font-weight:bold;font-size:14px;justify-content:flex-start;\">" + ReplaceToApostrophe(Convert.ToString(row["store"])) + "</td>";
                                    else
                                    htmltext += "<td style=\"text-align: left;width:20.5%;color:black;margin-right:0px;padding-left:50px;justify-content:flex-start;\">" + ReplaceToApostrophe(Convert.ToString(row["store"])) + "</td>";

                                    //Nishanth
                                    htmltext += "<td  style=\"text-align: center; background-color:#E4EEEF;color:grey;width:7.7%;font-weight:bold; \">Weekly + </td>";

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (WeeklyPlusSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Weekly+"])) + "</td>";
                                    }
                                    //atul new check for all else if
                                    else if (WeeklyPlusSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Weekly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\"></td>";
                                    }


                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (MonthlyPlusSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Monthly+"])) + "</td>";
                                    }
                                    else if (MonthlyPlusSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Monthly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\"></td>";
                                    }

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";margin-right:0px;\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (PastThreeMonthsSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";margin-right:0px;\">" + DecimalValues(Convert.ToString(row["Quarterly+"])) + "</td>";
                                    }
                                    else if (PastThreeMonthsSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;width:" + td_width + ";margin-right:0px;\">" + DecimalValues(Convert.ToString(row["Quarterly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";margin-right:0px;\"></td>";
                                    }

                                    htmltext += "</tr>";

                                    //write export Weekly+
                                    rownumber += 1;

                                    string metricname = ReplaceToApostropheExcel(Convert.ToString(row["store"]));
                                    metricname = cf.cleanExcelXML(metricname);
                                    CreateSharedStringDate(metricname);
                                    exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                    exportdata += CreateSheetData("A" + rownumber + "", "4", GetSharedStringValue(metricname), "true");

                                    //Nishanth
                                    exportdata += CreateSheetData("B" + rownumber + "", "7", GetSharedStringValue("Weekly+"), "true");
                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (WeeklyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Weekly+"])), "false");
                                    }
                                    //atul new check for all else if
                                    else if (WeeklyPlusSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Weekly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "5", "", "false");
                                    }

                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (MonthlyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Monthly+"])), "false");
                                    }
                                    else if (MonthlyPlusSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Monthly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "5", "", "false");
                                    }


                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (PastThreeMonthsSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Quarterly+"])), "false");
                                    }
                                    else if (PastThreeMonthsSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Quarterly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "5", "", "false");
                                    }

                                    exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                }
                                rownumber += 1;
                            }
                            else if (i == 2)
                            {
                                //write Monthly +
                                htmltext += "<tr class=\"crossRetailerheader\"><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:28.2%;background-color: #D9E1EE;padding-left:40px;position:relative;justify-content: flex-start;\" class=\"WeeklyMonthlyQuarterly\">Monthly+<a class=\"table - top - title - bottom - line\" style=\"background - color: #000000;border-radius: 1px 10px 0 0;bottom: 463px;background-color: #72aaff;display: block;height: 4px;left:0;width: 25px;position: absolute;top:83%;\"></a><div class=\"treeview minusIcon\"></div></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;margin-right:0;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td></tr>";

                                exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                exportdata += CreateSheetData("A" + rownumber + "", "7", GetSharedStringValue("Monthly+"), "true");
                                exportdata += CreateSheetData("B" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("C" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("D" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("E" + rownumber + "", "7", "", "");
                                exportdata += CreateAndCloseRow("close", rownumber.ToString());

                                mergeCell.Add("<mergeCell ref = \"A" + rownumber + ":D" + rownumber + "\"/>");

                                foreach (DataRow row in tbl.Rows)
                                {
                                    htmltext += "<tr>";
                                    if (Convert.ToString(row["Flag"]) == "1") 
                                    htmltext += "<td style=\"text-align: left;width:20.5%;color:black;margin-right:0px;padding-left:50px;font-weight:bold;font-size:14px;justify-content:flex-start;\">" + ReplaceToApostrophe(Convert.ToString(row["store"])) + "</td>";
                                    else
                                        htmltext += "<td style=\"text-align: left;width:20.5%;color:black;margin-right:0px;padding-left:50px;justify-content:flex-start;\">" + ReplaceToApostrophe(Convert.ToString(row["store"])) + "</td>";

                                    //Nishanth
                                    htmltext += "<td  style=\"text-align: center; background-color:#E4EEEF;color:grey;width:7.7%;font-weight:bold; \">Monthly+ </td>";

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (WeeklyPlusSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Weekly+"])) + "</td>";
                                    }
                                    //atul new check for all else if
                                    else if (WeeklyPlusSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Weekly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\"></td>";
                                    }

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (MonthlyPlusSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Monthly+"])) + "</td>";
                                    }
                                    else if (MonthlyPlusSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Monthly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\"></td>";
                                    }

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";margin-right:0px;\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (PastThreeMonthsSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"margin-right:0px;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Quarterly+"])) + "</td>";
                                    }
                                    else if (PastThreeMonthsSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;margin-right:0px;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Quarterly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"margin-right:0px;width:" + td_width + ";\"></td>";
                                    }
                                    htmltext += "</tr>";

                                    //write export Monthly+
                                    rownumber += 1;

                                    string metricname = ReplaceToApostropheExcel(Convert.ToString(row["store"]));
                                    metricname = cf.cleanExcelXML(metricname);
                                    CreateSharedStringDate(metricname);
                                    exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                    exportdata += CreateSheetData("A" + rownumber + "", "4", GetSharedStringValue(metricname), "true");

                                    //Nishanth
                                    exportdata += CreateSheetData("B" + rownumber + "", "7", GetSharedStringValue("Monthly+"), "true");

                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (WeeklyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Weekly+"])), "false");
                                    }
                                    //atul new check for all else if
                                    else if (WeeklyPlusSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Weekly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "5", "", "false");
                                    }

                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (MonthlyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Monthly+"])), "false");
                                    }
                                    else if (MonthlyPlusSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Monthly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "5", "", "false");
                                    }

                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (PastThreeMonthsSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Quarterly+"])), "false");
                                    }
                                    else if (PastThreeMonthsSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Quarterly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "5", "", "false");
                                    }
                                    exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                }
                                rownumber += 1;
                            }
                            else if (i == 3)
                            {
                                //write Past 3 month
                                htmltext += "<tr class=\"crossRetailerheader\"><td colspan=\"5\" style=\"text-align:left; border-top: 1px solid skyblue;width:28.2%;background-color: #D9E1EE;width:28.2%;padding-left:40px;position:relative;justify-content: flex-start; \" class=\"WeeklyMonthlyQuarterly\">Quarterly+<a class=\"table - top - title - bottom - line\" style=\"background - color: #000000;border-radius: 1px 10px 0 0;bottom: 463px;background-color: #72aaff;display: block;height: 4px;left:0;width: 25px;position: absolute;top:83%;\"></a><div class=\"treeview minusIcon\"></div></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td><td colspan=\"5\" style=\"text-align:left;border-top: 1px solid skyblue;width:23%;margin-right:0;background-color: #D9E1EE;padding-left: 3%;position:relative;\" class=\"WeeklyMonthlyQuarterly\"></td></tr>";// same changes here

                                exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                exportdata += CreateSheetData("A" + rownumber + "", "7", GetSharedStringValue("Quarterly+"), "true");// changes from "at least once in the past 3 month" to "Quarterly+"
                                exportdata += CreateSheetData("B" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("C" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("D" + rownumber + "", "7", "", "");
                                exportdata += CreateSheetData("E" + rownumber + "", "7", "", "");
                                exportdata += CreateAndCloseRow("close", rownumber.ToString());

                                mergeCell.Add("<mergeCell ref = \"A" + rownumber + ":D" + rownumber + "\"/>");

                                foreach (DataRow row in tbl.Rows)
                                {
                                    htmltext += "<tr>";
                                    if (Convert.ToString(row["Flag"]) == "1")
                                        htmltext += "<td style=\"text-align: left;width:20.5%;color:black;margin-right:0px;padding-left:50px;font-weight:bold;font-size:14px;justify-content:flex-start;\">" + ReplaceToApostrophe(Convert.ToString(row["store"])) + "</td>";
                                    else
                                        htmltext += "<td style=\"text-align: left;width:20.5%;color:black;margin-right:0px;padding-left:50px;justify-content:flex-start;\">" + ReplaceToApostrophe(Convert.ToString(row["store"])) + "</td>";

                                    //Nishanth
                                    htmltext += "<td  style=\"text-align: center; background-color:#E4EEEF;color:grey;width:7.7%;font-weight:bold; \">Quarterly +  </td>";

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (WeeklyPlusSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Weekly+"])) + "</td>";
                                    }
                                    //atul new check for all else if
                                    else if (WeeklyPlusSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;\">" + DecimalValues(Convert.ToString(row["Weekly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\"></td>";
                                    }

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (MonthlyPlusSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Monthly+"])) + "</td>";
                                    }
                                    else if (MonthlyPlusSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Monthly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";\"></td>";
                                    }

                                    if (IsNA)
                                    {
                                        htmltext += "<td style=\"width:" + td_width + ";margin-right:0px;\">" + GlobalVariables.NA + "</td>";
                                    }
                                    else if (PastThreeMonthsSampleSize == 3)
                                    {
                                        htmltext += "<td style=\"margin-right:0px;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Quarterly+"])) + "</td>";
                                    }
                                    else if (PastThreeMonthsSampleSize == 2)
                                    {
                                        htmltext += "<td style=\"background-color: #E6E6E6;margin-right:0px;width:" + td_width + ";\">" + DecimalValues(Convert.ToString(row["Quarterly+"])) + "</td>";
                                    }
                                    else
                                    {
                                        htmltext += "<td style=\"margin-right:0px;width:" + td_width + ";\"></td>";
                                    }
                                    htmltext += "</tr>";

                                    //write export Past 3 Months
                                    rownumber += 1;

                                    string metricname = ReplaceToApostropheExcel(Convert.ToString(row["store"]));
                                    metricname = cf.cleanExcelXML(metricname);
                                    CreateSharedStringDate(metricname);
                                    exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                    exportdata += CreateSheetData("A" + rownumber + "", "4", GetSharedStringValue(metricname), "true");

                                    //Nishanth
                                    exportdata += CreateSheetData("B" + rownumber + "", "7", GetSharedStringValue("Quarterly+"), "true");

                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (WeeklyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Weekly+"])), "false");
                                    }
                                    //atul new check for all else if
                                    else if (WeeklyPlusSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Weekly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("C" + rownumber + "", "5", "", "false");
                                    }

                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (MonthlyPlusSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Monthly+"])), "false");
                                    }
                                    else if (MonthlyPlusSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Monthly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("D" + rownumber + "", "5", "", "false");
                                    }

                                    CreateSharedStringDate(GlobalVariables.NA);
                                    if (IsNA)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "7", GetSharedStringValue(GlobalVariables.NA), "true");
                                    }
                                    else if (PastThreeMonthsSampleSize == 3)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "5", MathExportDecimalValues(Convert.ToString(row["Quarterly+"])), "false");
                                    }
                                    else if (PastThreeMonthsSampleSize == 2)
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "6", MathExportDecimalValues(Convert.ToString(row["Quarterly+"])), "false");
                                    }
                                    else
                                    {
                                        exportdata += CreateSheetData("E" + rownumber + "", "5", "", "false");
                                    }
                                    exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                }
                                htmltext += "</tr></table>";
                                htmltext += "</div>";
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(exportdata))
                {
                    exportdata += "</sheetData>";
                    if (mergeCell != null && mergeCell.Count > 0)
                    {
                        //Nishanth
                        //string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                        //foreach (string mergrrow in mergeCell)
                        //{
                        //    mergetext += mergrrow;

                        //}
                        //mergetext += "</mergeCells>";
                        //exportdata += mergetext;
                    }
                    exportdata += GetPageMargins();
                }
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                HttpContext.Current.Session["exportcrossTAB"] = exportdata;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);

            }
            return htmltext;
        }

        public string CommaSeparatedValues(string value)
        {
            string decimaval = "0";
            if (!string.IsNullOrEmpty(value) && Convert.ToDouble(value) != GlobalVariables.NANumber)
            {
                decimaval = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(value)));
            }
            return decimaval;
        }  

        public string DecimalValues(string rowvalue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(rowvalue))
            {
                value = "";
            }
            else
            {
                //value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + "%";
                value = CommonFunctions.GetRoundingValue(rowvalue) + "%";
            }
            return value;
        }


        public string MathExportDecimalValues(string rowvalue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(rowvalue))
            {
                value = "";
            }
            else
            {
                double val = Convert.ToDouble(rowvalue) / 100;
                value = val.ToString();
            }
            return value;
        }
        public string ReplaceToApostrophe(string retailer)
        {
            string strvalue = retailer;
            if (!string.IsNullOrEmpty(retailer) && retailer.Contains("`"))
            {
                strvalue = retailer.Replace("`", "'");
            }
            return MakeboldToChannel(strvalue);

        }
        public string ReplaceToApostropheExcel(string retailer)
        {
            string strvalue = retailer;
            if (!string.IsNullOrEmpty(retailer) && retailer.Contains("`"))
            {
                strvalue = retailer.Replace("`", "'");
            }
            return ExcelShortChannel(strvalue);

        }
        public string ExcelShortChannel(string channel)
        {
            string channelvalue = channel;
            switch (channel)
            {
                case "A convenience store or gas station food mart (excluding gas)":
                    {
                        channelvalue = "Convenience";
                        break;
                    }
                case "A dollar store such as Family Dollar or Dollar General":
                    {
                        channelvalue = "Dollar";
                        break;
                    }
                case "A supermarket or grocery store":
                    {
                        channelvalue = "Supermarket / Grocery";
                        break;
                    }
                case "A mass merchandise store without a full-line grocery section such as Walmart or Target":
                    {
                        channelvalue = "Mass Merc.";
                        break;
                    }
                case "A drug store":
                    {
                        channelvalue = "Drug";
                        break;
                    }
                case "A warehouse club such as Sam's Club or Costco":
                    {
                        channelvalue = "Club";
                        break;
                    }
                case "A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget":
                    {
                        channelvalue = "Supercenter";
                        break;
                    }
            }
            return channelvalue;
        }
        public string MakeboldToChannel(string channel)
        {
            string channelvalue = channel;
            switch (channel)
            {
                case "A convenience store or gas station food mart (excluding gas)":
                    {
                        channelvalue = "<span style=\"font-weight:bold;font-size:17px;\">Convenience</span>";
                        break;
                    }
                case "A dollar store such as Family Dollar or Dollar General":
                    {
                        channelvalue = "<span style=\"font-weight:bold;font-size:17px;\">Dollar</span>";
                        break;
                    }
                case "A supermarket or grocery store":
                    {
                        channelvalue = "<span style=\"font-weight:bold;font-size:17px;\">Supermarket / Grocery</span>";
                        break;
                    }
                case "A mass merchandise store without a full-line grocery section such as Walmart or Target":
                    {
                        channelvalue = "<span style=\"font-weight:bold;font-size:17px;\">Mass Merc.</span>";
                        break;
                    }
                case "A drug store":
                    {
                        channelvalue = "<span style=\"font-weight:bold;font-size:17px;\">Drug</span>";
                        break;
                    }
                case "A warehouse club such as Sam's Club or Costco":
                    {
                        channelvalue = "<span style=\"font-weight:bold;font-size:17px;\">Club</span>";
                        break;
                    }
                case "A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget":
                    {
                        channelvalue = "<span style=\"font-weight:bold;font-size:17px;\">Supercenter</span>";
                        break;
                    }
            }
            return channelvalue;
        }










        public string GetSheetHeadandColumns()
        {
            string sheetstr = "xmlns = \"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
        "xmlns:r = \"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" " +
        "xmlns:mc = \"http://schemas.openxmlformats.org/markup-compatibility/2006\" " +
        "mc:Ignorable = \"x14ac\" " +
        "xmlns:x14ac = \"http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac\"> " +
        "<dimension ref = \"A1\"/> " +
        "<sheetViews> " +
           "<sheetView showGridLines = \"0\" tabSelected = \"1\" zoomScale = \"80\" zoomScaleNormal = \"80\" workbookViewId = \"0\"> " +
             "<pane ySplit =\"4\" topLeftCell =\"A5\" activePane =\"bottomLeft\" state =\"frozen\"/>" +
              "<selection pane = \"bottomLeft\" activeCell = \"A1\" sqref = \"A1\"/> " +
              "</sheetView> " +
        "</sheetViews> " +
        "<sheetFormatPr defaultRowHeight = \"15\" x14ac:dyDescent = \"0.25\"/> " +
        "<cols> " +
            "<col " +
                "min = \"1\" " +
                "max = \"1\" " +
                "width = \"35.140625\" " +
                "customWidth = \"1\"/> " +
            "<col " +
                "min = \"2\" " +
               "max = \"2\" " +
                "width = \"35.140625\" " +
                "customWidth = \"1\"/> " +
            "<col " +
                "min = \"3\" " +
                "max = \"3\" " +
                "width = \"35.140625\" " +
                "customWidth = \"1\"/> " +
           "<col " +
                "min = \"4\" " +
                "max = \"4\" " +
                "width = \"35.140625\" " +
                "customWidth = \"1\"/> " +
                 "customWidth = \"1\"/> " +
            "<col " +
                "min = \"5\" " +
                "max = \"5\" " +
                "width = \"35.140625\" " +
                "customWidth = \"1\"/> " +
        "</cols>";
            return sheetstr;
        }

        public string GetPageMargins()
        {
            string pagem = "<pageMargins " +
                "left = \"0.7\" " +
                "right = \"0.7\" " +
                "top = \"0.75\" " +
                "bottom = \"0.75\" " +
                "header = \"0.3\" " +
                "footer = \"0.3\"/>" +
                 "<pageSetup " +
                "paperSize = \"9\" " +
                "orientation = \"portrait\" " +
                "r:id = \"rId1\"/> " +
            "<drawing r:id = \"rId2\"/>";
            return pagem;

        }



        public string CreateAndCloseRow(string rowType, string rowNumber)
        {
            string exportdata = string.Empty;
            if (rowType.Equals("create", StringComparison.OrdinalIgnoreCase))
            {
                exportdata = "<row " +
                    "r = \"" + rowNumber + "\" " +
                    "spans = \"1:4\" " +
                    "ht = \"17.25\" " +
                    "x14ac:dyDescent = \"0.25\">";
            }
            else
            {
                exportdata = "</row>";
            }
            return exportdata;
        }

        public string CreateAndCloseHeaderRow(string rowType, string rowNumber)
        {
            string exportdata = string.Empty;
            if (rowType.Equals("create", StringComparison.OrdinalIgnoreCase))
            {
                exportdata = "<row " +
                    "r = \"" + rowNumber + "\" " +
                    "spans = \"1:4\" " +
                    "ht = \"30\" " +
                    "x14ac:dyDescent = \"0.25\">";
            }
            else
            {
                exportdata = "</row>";
            }
            return exportdata;
        }


        public string CreateSheetData(string rownumber, string style, string value, string title)
        {
            string exportdata = string.Empty;
            if (!string.IsNullOrEmpty(rownumber) && title == "true")
            {
                exportdata = "<c " +
                       "r = \"" + rownumber + "\" " +
                        "s = \"" + style + "\" " +
                        "t = \"s\"> " +
                        "<v \r\n>" + value + "</v> " +
                    "</c>";
            }
            else if (!string.IsNullOrEmpty(rownumber) && title == "false")
            {
                exportdata = "<c " +
                          "r = \"" + rownumber + "\" " +
                           "s = \"" + style + "\"> " +
                           "<v>" + value + "</v> " +
                       "</c>";
            }
            else if (!string.IsNullOrEmpty(rownumber) && string.IsNullOrEmpty(value) && title == "")
            {
                exportdata = "<c " +
                          "r = \"" + rownumber + "\" " +
                           "s = \"" + style + "\"> " +
                       "</c>";
            }
            return exportdata;
        }

        public void CreateSharedStringDate(string value)
        {
            if (!sharedStrings.ContainsKey(value))
            {
                sharedStrings.Add(value, sharedStrings.Count());
            }
        }

        public string GetSharedStringValue(string value)
        {
            string strvalue = "0";
            if (sharedStrings.ContainsKey(value))
            {
                strvalue = Convert.ToString(sharedStrings[value]);
            }
            return strvalue;
        }

        public string CheckLowSampleSize(string samplesize, string Samplevalue)
        {
            string sz = string.Empty;
            IsNA = false;
            if (!string.IsNullOrEmpty(samplesize))
            {
                if (Convert.ToDouble(samplesize) == GlobalVariables.NANumber)
                {
                    IsNA = true;
                }
                if (Convert.ToDouble(samplesize) < GlobalVariables.MinSampleSize)
                {
                    sz = " (<span style=\"font-size:14px;\">Low Sample Size</span>)";
                    if (Samplevalue == "Weekly+")
                    {
                        WeeklyPlusSampleSize = 1;
                    }
                    if (Samplevalue == "Monthly+")
                    {
                        MonthlyPlusSampleSize = 1;
                    }
                    if (Samplevalue == "Quarterly+")
                    {
                        PastThreeMonthsSampleSize = 1;
                    }
                }
                //atul new
                if (Convert.ToDouble(samplesize) >= GlobalVariables.MinSampleSize && Convert.ToDouble(samplesize) < GlobalVariables.MaxSampleSize)
                {
                    sz = " (<span style=\"font-size:14px;\">Use Directionally</span>)";
                    if (Samplevalue == "Weekly+")
                    {
                        WeeklyPlusSampleSize = 2;
                    }
                    if (Samplevalue == "Monthly+")
                    {
                        MonthlyPlusSampleSize = 2;
                    }
                    if (Samplevalue == "Quarterly+")
                    {
                        PastThreeMonthsSampleSize = 2;
                    }
                }
                if (Convert.ToDouble(samplesize) >= 100)
                {
                    //sz = "(<span></span>)";
                    if (Samplevalue == "Weekly+")
                    {
                        WeeklyPlusSampleSize = 3;
                    }
                    if (Samplevalue == "Monthly+")
                    {
                        MonthlyPlusSampleSize = 3;
                    }
                    if (Samplevalue == "Quarterly+")
                    {
                        PastThreeMonthsSampleSize = 3;
                    }
                }

            }

            return sz;
        }

    }
}