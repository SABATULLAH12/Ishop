using iSHOPNew.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace iSHOPNew.Models
{
    public class Table : TableBase
    {              
        public override iSHOPParams BindTabs(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string timePeriod, string _ShopperSegment, string filterShortname, string _ShopperFrequency, string[] ShortNames, string StatPositive, string StatNegative, bool ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams)
        {
            View_Type = "COMPARE";
            iSHOPParams ishopParams = new iSHOPParams();
            table_Params = tableParams;
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
            frequency = _ShopperFrequency;
            PopulateShortNames();
            Shortnames();

            if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = tableParams.CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedStatTest = Selected_StatTest;
            }

            Retailerlist = _Comparisonlist;
            var query1 = from r in _Comparisonlist select string.Join("||", r.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            _Comparisonlist = query1.ToArray();
            param = new iSHOPParams();
            _BenchMark = string.Join("||", _BenchMark.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            param.BenchMark = _BenchMark;
            //param.BenchMark = string.Join("||", _BenchMark.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            string[] complistaArray = new string[0];
            //complist = new List<string>();
            var query = from r in ShortNames select r;
            complistaArray = query.ToArray();
            complist = query.ToList();
            ul_row_width = Math.Round(Convert.ToDouble(ulwidth), 0);
            ul_cell_width = Math.Round(Convert.ToDouble(ulliwidth), 0);

            //param.ShopperSegment = _ShopperSegment;
            TimePeriod = TimePeriodShortName;
            param.ShopperFrequency = _ShopperFrequency;
            param.CustomFilters = filterShortname;


            BenchmarkorComparisionList = _Comparisonlist.ToList();
            //BenchmarkorComparisionList.Insert(0, _BenchMark);


            isBevTotalTrips = Array.IndexOf(_Comparisonlist, checkBevTotalTrips);
            if (isBevTotalTrips > -1)
            {
                isBevTotalTrips = 1;
            }
            else
            {
                isBevTotalTrips = 0;
            }

            DataAccess dal = new DataAccess();
            object[] paramvalues = null;
            DataSet ds = null;
            DataSet ds_2 = null;
            DataTable tbl_Common_Sample_Size = null;
            common_SampleSize = false;
            if (tableParams.Tab_Id_mapping)
            {
                string benchmark_UID = string.Empty;
                string comp_UID = string.Empty;
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    benchmark_UID = tableParams.CustomBase_UniqueId;
                    List<string> comp_list = (from r in tableParams.Comparison_UniqueIds where r != benchmark_UID select r).ToList();
                    comp_UID = string.Join("|", comp_list);
                }
                else
                {
                     benchmark_UID = tableParams.Comparison_UniqueIds[0];
                    tableParams.Comparison_UniqueIds.RemoveAt(0);
                    comp_UID = string.Join("|", tableParams.Comparison_UniqueIds);
                }
                TabIndexId = int.Parse(tableParams.TabIndexId);
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, benchmark_UID.ToMyString(), comp_UID.ToMyString(), tableParams.TimePeriod_UniqueId.ToMyString(), tableParams.ShopperSegment_UniqueId.ToMyString(), tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString(), tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };
                }
                else
                {
                    paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, benchmark_UID.ToMyString(), comp_UID.ToMyString(), tableParams.TimePeriod_UniqueId.ToMyString(), tableParams.ShopperSegment_UniqueId.ToMyString(), tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString() };
                }
                ds_2 = dal.GetData_WithIdMapping(paramvalues, tabid);
                if(ds_2 != null && ds_2.Tables.Count > 0)
                {
                    ds = new DataSet();
                    var query2 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    var query4 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    if(query4 != null && query4.Count > 0)
                        common_SampleSize = false;
                   else if (query2 != null && query2.Count > 0)
                        common_SampleSize = true;

                    if (query2 != null && query2.Count > 0)
                    {
                        tbl_Common_Sample_Size = query2.CopyToDataTable();                      
                    }
                    sampleSize = new Dictionary<string, string>();
                    if (common_SampleSize)
                    {
                        var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                                where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                                && Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                                select row).Distinct().ToList();
                        if (query_samplesize != null && query_samplesize.Count > 0)
                        {
                            DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                            foreach (object column in trips_samplesize.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                        else
                        {
                            foreach (object column in tbl_Common_Sample_Size.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                    }
                    List<string> metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                               where !Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                               select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach(string metric in metriclist)
                    {
                       var query3 = (from row in ds_2.Tables[0].AsEnumerable() 
                                     where Convert.ToString(row["Metric"]).Equals(metric,StringComparison.OrdinalIgnoreCase)
                                    select row).Distinct().ToList();
                        if(query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
            }
            else
            {
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    _BenchMark = tableParams.CustomBase_DBName;
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();                   
                }
                else
                {
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();                   
                }
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { _BenchMark, String.Join("|", _Comparisonlist).Trim(), timePeriod, _ShopperSegment, _ShopperFrequency, Selected_StatTest, tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };
                }
                else
                {
                    paramvalues = new object[] { _BenchMark, String.Join("|", _Comparisonlist).Trim(), timePeriod, _ShopperSegment, _ShopperFrequency, Selected_StatTest };
                }
                ds = dal.GetData(paramvalues, tabid);
            }

            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            isBeverageDetail = false;
            if (tabid.Equals("sp_FactBookTripBevDetailsAcrossBeverageTripMain", StringComparison.OrdinalIgnoreCase))
                isBeverageDetail = true;
            //

            if (IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency/imagery") > -1 || IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency") > -1)
            {
                IsStoreImagery = "StoreImagery";
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    _BenchMark = tableParams.CustomBase_DBName;
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                else
                {
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { Convert.ToString(_BenchMark).ToMyString(), String.Join("|", _Comparisonlist).Trim(), timePeriod, _ShopperSegment, _ShopperFrequency, Selected_StatTest, tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };
                }
                else
                {
                    paramvalues = new object[] { Convert.ToString(_BenchMark).ToMyString(), String.Join("|", _Comparisonlist).Trim(), timePeriod, _ShopperSegment, _ShopperFrequency, Selected_StatTest };
                }
               
                //DataSet LoyaltyDs = dal.GetData(paramvalues, "sp_FactBookRespStoreChannelMapping");
                LoyaltyRetailerList = new Hashtable();
                //foreach (DataRow row in LoyaltyDs.Tables[0].Rows)
                //{
                //    if (!Convert.ToString(row["Flag"]).Equals(GlobalVariables.NA) && !LoyaltyRetailerList.ContainsKey(Convert.ToString(row["Flag"])))
                //        LoyaltyRetailerList.Add(Convert.ToString(row["Flag"]), Convert.ToString(row["DisplayMetricName"]));
                //}
            }
            //else if (IsStoreImagery == "divbeveragetripBeverageDetail" && (BenchMark.Trim() == "Total Trips" || isBevTotalTrips == 1))
            //{
            //    DataSet LoyaltyDs = dal.GetData(paramvalues, "sp_FactbookBeverageDetailsTOTALTRIPcondition");
            //    CheckBeverageTripNAhTbl = new Hashtable();
            //    foreach (DataRow row in LoyaltyDs.Tables[0].Rows)
            //    {
            //        if (!Convert.ToString(row["Flag"]).Equals(GlobalVariables.NA) && !LoyaltyRetailerList.ContainsKey(Convert.ToString(row["Flag"])))
            //            CheckBeverageTripNAhTbl.Add(Convert.ToString(row["Flag"]), Convert.ToString(row["DisplayMetricName"]));
            //    }
            //}



            int excelcolumnindex = 1;
            int rownumber = 6;

            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);
            //Nagaraju 27-03-2014
            if (ExportToExcel)
            {
                if (HttpContext.Current.Session["sharedstrings"] != null)
                {
                    sharedStrings = HttpContext.Current.Session["sharedstrings"] as Dictionary<string, int>;
                }
            }
            //End
            tbltext = new StringBuilder();
            string Significance = string.Empty;
            xmlstring = string.Empty;
            colmaxwidth = 0;
            StringBuilder xmltext = new StringBuilder();
            mergeCell = new List<string>();

            try
            {
                xmltext.Append("<sheetData>");
                //write top header
                xmltext.Append(WriteFilters());
                xmltext.Append(AddSampleSizeNote());
                xmltext.Append(GetTableHeader(complistaArray.Count(), tableParams.ViewType));

                if (complistaArray.Count() > 1)
                {
                    mergeCell.Add("<mergeCell ref = \"B5:" + ColumnIndexToName(complistaArray.Count()) + "5\"/>");
                }
                //if (!CheckSharedStringValue("BENCHMARK"))
                //{
                //    AddToSharedString("BENCHMARK");
                //}

                //if (!CheckSharedStringValue("COMPARISON AREAS"))
                //{
                //    AddToSharedString("COMPARISON AREAS");
                //}

                //write second header
                excelcolumnindex = 0;
                xmltext.Append(" <row" +
               " r = \"" + rownumber + "\" " +
                "spans = \"1:11\" " +
                "x14ac:dyDescent = \"0.25\">" +
               " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"5\"/>");
                excelcolumnindex += 1;

                tbltext.Append("<thead>");
                tbltext.Append(CreateFirstTableHeader());
                tbltext.Append("<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><span>" + frequency + "</span></li>");

                leftheader += "<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><a class=\"table-top-title-bottom-line\"></a><span style=\"\">" + (tableParams.IsWherePurchased ? string.Empty : frequency) + "</span></li></ul></div>";
                rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                for (int i = 0; i < complistaArray.Count(); i++)
                {
                    colNames = complistaArray[i] +  AddTradeAreaNoteforChannel(complistaArray[i]);
                    if (i == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(complistaArray[i] + "header");
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }

                    tbltext.Append("<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>");
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                    xmlstring = cf.cleanExcelXML(colNames);

                    if (!CheckSharedStringValue(xmlstring))
                    {
                        AddToSharedString(xmlstring);
                    }

                    xmltext.Append(" <c" +
                      " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                      " s = \"4\" " +
                      " t = \"s\">" +
                       "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                   "</c>");
                    excelcolumnindex += 1;
                }
                tbltext.Append("</ul></div>");
                rightheader += "</ul></div>";
                //add check sample size
                tbltext.Append("<div class=\"rowitem\"><ul>");
                xmltext.Append("</row>");

                List<iSHOPParams> iSHOPParamlist = null;
                SampleSize checksampleSize = new SampleSize();
                bool isOnlineTab = tableParams.TabName.ToLower() == "online metrics";
                string smapleSizeText = (isOnlineTab ? "Sample Size: Retailer’s " + tableParams.ShopperFrequency + " shoppers who ALSO shop for online for grocery at any retailer every month" : "Sample Size");
                iSHOPParamlist = checksampleSize.CheckAccrossRetailerSampleSize(checksamplesizesp, _BenchMark, Retailerlist, timePeriod, _ShopperSegment, _ShopperFrequency, ShortNames, tableParams.Tab_Id_mapping,tbl_Common_Sample_Size,tableParams);
                rownumber += 1;
                excelcolumnindex = 0;
                xmlstring = cf.cleanExcelXML(smapleSizeText);

                if (!CheckSharedStringValue(xmlstring))
                {
                    AddToSharedString(xmlstring);
                }

                xmltext.Append("<row " +
                                     "r = \"" + rownumber + "\" " +
                                     "spans = \"1:11\" " +
                                     "ht = \"15\" " +
                                     "thickBot = \"1\" " +
                                     "x14ac:dyDescent = \"0.3\">" +
                                     " <c" +
                                     " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                     " s = \"3\" " +
                                     " t = \"s\">" +
                                    "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                    "</c>");
                if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                {
                    tbltext.Append("<li style=\"\"><span>" + smapleSizeText + "</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>" + smapleSizeText + "</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    foreach (iSHOPParams para in iSHOPParamlist)
                    {
                        excelcolumnindex += 1;                                          
                        if (iSHOPParamlist.IndexOf(para) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\">" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>";

                        }
                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>");
                     
                        string lowsamplesize = CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize));     
                        if (!string.IsNullOrEmpty(lowsamplesize))
                        {
                             lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(para.SampleSize))) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize));
                            if (!CheckSharedStringValue(lowsamplesize))
                            {
                                AddToSharedString(lowsamplesize);
                            }

                            xmltext.Append(" <c" +
                                                                   " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\" " +
                                                                   " t = \"s\">" +
                                                                   "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                   "</c>");
                          
                        }
                        else
                        {
                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                     " s = \"2\">" +
                                                                "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(para.SampleSize).Replace(",", ""), out IsApplicable) + "</v>" +
                                                                      "</c> ");
                        }
                    }
                    rightheader += "</ul></div>";
                }
                else
                {
                    tbltext.Append("<li style=\"\"><span>" + smapleSizeText + "</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>" + smapleSizeText + "</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    for (int j = 0; j < complistaArray.Count(); j++)
                    {
                        excelcolumnindex += 1;
                        colNames = Get_ShortNames(complistaArray[j].Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                        if (j == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            leftheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(complistaArray[j]) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>";
                        }

                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>");
                        xmlstring = string.Empty;

                        if (!CheckSharedStringValue(xmlstring))
                        {
                            AddToSharedString(xmlstring);
                        }

                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                 " s = \"2\">" +
                                                            "<v></v>" +
                                                                  "</c> ");
                    }
                    rightheader += "</ul></div>";
                }
                tbltext.Append("</ul></div>");
                //tbltext.Append("";
                //tbltext.Append("<tbody>";
                xmltext.Append("</row>");

                //leftbody = "<table><body>";
                //righttbody = "<div class=\"rowitem\"><ul>";

                benchmark_comp_class = string.Empty;
                //end header

                //------->
                table_count = 0;
                rows_count = 0;
                if (ds != null && ds.Tables.Count > 0)
                {
                    table_count = ds.Tables.Count;
                    int colms = ds.Tables[0].Columns.Count;
                    //rownumber = 7;                   
                    for (int tbl = 0; tbl < table_count; tbl++)
                    {
                        if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                        {
                            isItemHasSpace = false;                           
                            excelcolumnindex = 0;
                            if (!common_SampleSize)
                            {
                                sampleSize = new Dictionary<string, string>();
                            }                           
                            rownumber += 1;
                            LoyaltyPyramid = false;
                            CheckBeverageTripNA = false;
                            LoyaltyPyramidmetric = Convert.ToString(ds.Tables[tbl].Rows[0][0]);
                            //if (LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(Base:CouldShop)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supermarket)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(convenience)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(drug store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(dollar store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(club)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(mass merchandise)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supercenter)")
                            //{
                            //    LoyaltyPyramid = true;
                            //}

                            //switch (LoyaltyPyramidmetric)
                            //{
                            //    //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                            //    case "RetailerLoyaltyPyramid(supermarket)":
                            //    case "RetailerLoyaltyPyramid(convenience)":
                            //    case "RetailerLoyaltyPyramid(drug store)":
                            //    case "RetailerLoyaltyPyramid(dollar store)":
                            //    case "RetailerLoyaltyPyramid(club)":
                            //    case "RetailerLoyaltyPyramid(mass merchandise)":
                            //    case "RetailerLoyaltyPyramid(supercenter)":
                            //        {
                            //            LoyaltyPyramid = true;
                            //            break;
                            //        }
                            //}

                            //switch (LoyaltyPyramidmetric)
                            //{
                            //    //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                            //    case "RetailerLoyaltyPyramid(supermarket)":
                            //    case "RetailerLoyaltyPyramid(convenience)":
                            //    case "RetailerLoyaltyPyramid(drug store)":
                            //    case "RetailerLoyaltyPyramid(dollar store)":
                            //    case "RetailerLoyaltyPyramid(club)":
                            //    case "RetailerLoyaltyPyramid(mass merchandise)":
                            //    case "RetailerLoyaltyPyramid(supercenter)":
                            //        {
                            //            LoyaltyPyramidForRetailers = true;
                            //            break;
                            //        }
                            //    default:
                            //        LoyaltyPyramidForRetailers = false;
                            //        break;
                            //}
                            switch (LoyaltyPyramidmetric)
                            {
                                case "Product Temperature":
                                case "Chilled - Location":
                                case "Room Temperature Location":
                                case "Intended Consumer":
                                    {
                                        CheckBeverageTripNA = true;
                                        break;
                                    }
                                default:
                                    CheckBeverageTripNA = false;
                                    break;
                            }

                            //else if (CheckRetailerorChannel.IndexOf("retailers") > -1 && StoreImageryCheck)
                            //{
                            //}
                            Table_Header_TotalUS_StyleId = 15;
                            Table_Header_BackgroundColor = "#D9E1EE";
                            Table_Header_BorderTopColor = "skyblue";
                            Table_Header_BottomTitleColor = "#72aaff";
                            if (IsStoreFrequencyTotalUS(LoyaltyPyramidmetric))
                            {
                                Table_Header_TotalUS_StyleId = 38;
                                Table_Header_BackgroundColor = "#E1E1E2; border-bottom: 1px solid darkgray;";
                                Table_Header_BorderTopColor = "#E1E1E2";
                                Table_Header_BottomTitleColor = "darkgrey";
                            }

                            tbltext.Append("<div class=\"rowitem\"><ul><li style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>");
                            leftbody += "<div class=\"rowitem table-title\"><ul><li style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><a class=\"table-title-bottom-line\" style=\"background-color:" + Table_Header_BottomTitleColor + "\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>";
                            righttbody += "<div class=\"rowitem table-title\"><ul style=\"\">";
                            for (int i = 0; i < complistaArray.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span></span></li>";
                                }
                                else
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span  style=\"\"></span></li>";
                                }
                                tbltext.Append("<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span></span></li>");

                            }
                            leftbody += "</ul></div>";
                            righttbody += "</ul></div>";

                            tbltext.Append("</ul></div>");

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                            xmlstring = cf.cleanExcelXML(Check_Beverage_Liquid_Flavor_Enhancer_NA_Table(tablename));

                            if (!CheckSharedStringValue(xmlstring))
                            {
                                AddToSharedString(xmlstring);
                            }


                            //write table name
                            List<string> tblcolumns = new List<string>();
                            //foreach (object col in ds.Tables[tbl].Columns)
                            //{
                            //    string coln = Convert.ToString(col);
                            //    tblcolumns.Add(coln.Trim().ToLower().ToString());
                            //}

                            tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                            List<string> comp = (from r in complistaArray select r.ToLower()).ToList();
                            tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();

                            xmltext.Append("<row " +
                      "r = \"" + rownumber + "\" " +
                   "spans = \"1:11\" " +
                  " ht = \"15\" " +
                   "thickBot = \"1\" " +
                  " x14ac:dyDescent = \"0.3\">" +
                   "<c " +
                       "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                       "s = \"" + (LoyaltyPyramid ? 16 : Table_Header_TotalUS_StyleId) + "\" " +
                       "t = \"s\">" +
                       "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                   "</c>");

                            //for (int i = 0; i < complistaArray.Count; i++)
                            //{
                            //    excelcolumnindex += 1;
                            //    xmltext.Append("<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : 15) + "\"/>");
                            //}

                            xmltext.Append(String.Join("", complistaArray.Select(x => "<c r = \" " + ColumnIndexToName(excelcolumnindex++) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : Table_Header_TotalUS_StyleId) + "\"/>")));

                            xmltext.Append("</row>");
                            rows_count = ds.Tables[tbl].Rows.Count;
                            for (int rows = 0; rows < rows_count; rows++)
                            {
                                excelcolumnindex = 0;
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                MetricItem = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext.Append("<div class=\"rowitem\"><ul>");
                                    leftbody += "<div class=\"rowitem\"><ul>";
                                    righttbody += "<div class=\"rowitem\"><ul style=\"\">";
                                    //if (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average Number of Items" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average time in store" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Amount Spent")
                                    //{
                                    //    average = "Average";
                                    //}
                                    //else
                                    //{
                                    //    average = "";
                                    //}

                                    average = "";
                                    switch (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]))
                                    {
                                        case "Approximate Average Number of Items":
                                        case "AVERAGE ONLINE BASKET SIZE":
                                        case "Approximate Average time in store":
                                        case "Approximate Amount Spent":
                                        case "AVERAGE ONLINE ORDER SIZE":
                                            {
                                                average = "Average";
                                                break;
                                            }
                                    }


                                    //write sample size
                                    if (String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Number of Trips", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "SampleSize", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Sample Size", true) == 0
                                         || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("SampleSize", StringComparison.OrdinalIgnoreCase) >= 0 || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Sample Size", StringComparison.OrdinalIgnoreCase) >= 0
                                        || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Number of Trips", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sampleSize = new Dictionary<string, string>();
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left; color: #878787;font-weight: bold;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>");

                                        leftbody += "<li style=\"overflow: hidden;text-align: left; color: #878787;font-weight: bold;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<"));

                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem);
                                        if (!CheckSharedStringValue(xmlstring))
                                        {
                                            AddToSharedString(xmlstring);
                                        }

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"3\" " +
                                           "t = \"s\">" +
                                           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                       "</c> ");

                                        //plot sample size
                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                                            //Date: 21-03-2016
                                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[rows][0]), tblcolumns[i]);
                                            //
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].ToLower()))
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]),out samplecellstyle,isBeverageDetail,isLiquidFlavorEnhancer,LoyaltyPyramid) + "</span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span  style=\"\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]),out samplecellstyle,isBeverageDetail,isLiquidFlavorEnhancer,LoyaltyPyramid) + "</span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]),out samplecellstyle,isBeverageDetail,isLiquidFlavorEnhancer,LoyaltyPyramid) + "</span></li>");
                                                    sampleSize.Add(tblcolumns[i], Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                    if (samplecellstyle == 20)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])));
                                                        if (!CheckSharedStringValue(lowsamplesize))
                                                        {
                                                            AddToSharedString(lowsamplesize);
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else if (samplecellstyle == 30)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])));
                                                        if (!CheckSharedStringValue(lowsamplesize))
                                                        {
                                                            AddToSharedString(lowsamplesize);
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else
                                                    {

                                                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\">" +
                                                              "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                    "</c> ");
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span></span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span  style=\"\"></span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                    " s = \"2\">" +
                                                      "<v></v>" +
                                                         "</c> ");
                                                }
                                            }
                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                        xmltext.Append("</row>");
                                        //End Sample Size
                                    }

                                    else
                                    {
                                        isitembold = false;
                                        if (IsItemBold(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[rows]["Metric"])))
                                            leftbody += "<li class=\"itembold\" style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";
                                        else
                                        leftbody += "<li style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";

                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>");

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<"));
                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        if (isitembold)
                                        {                                           
                                            isItemHasSpace = true;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                      
                                            xmlstring = cf.cleanExcelXML(metricitem);

                                        if (!CheckSharedStringValue(xmlstring))
                                        {
                                            AddToSharedString(xmlstring);
                                        }
                                        string metricstyleid = "5";
                                        if (isitembold)
                                            metricstyleid = "36";
                                        else if (!isitembold && isItemHasSpace)
                                            metricstyleid = "37";

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"" + (average == "Average" ? "16" : metricstyleid) + "\" " +
                                           "t = \"s\">" +
                                           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                       "</c> ");
                                        //cellfontstyle = 8;

                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                                            //Date: 21-03-2016
                                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[rows][0]), tblcolumns[i]);
                                            //
                                            BenchmarkOrComparison = complistaArray[i];
                                            RetailerNetCheck = false;
                                            //if (BenchmarkorComparisionList[i].IndexOf("Retailers") > -1 && (IsStoreImagery.IndexOf("StoreImagery") > -1) && LoyaltyPyramidForRetailers)
                                            //{
                                            //    StoreImageryCheck = true;
                                            //    CheckRetailerorChannel = false;
                                            //}
                                            //else if ((CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            //{
                                            //    RetailerNetCheck = true;
                                            //    CheckRetailerorChannel = true;
                                            //}
                                            //else if ((BenchmarkorComparisionList[i].IndexOf("Channel") > -1 || BenchmarkorComparisionList[i].IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            //{
                                            //    CheckRetailerorChannel = true;
                                            //    StoreImageryCheck = false;
                                            //}
                                            //else
                                            //{
                                            //    CheckRetailerorChannel = false;
                                            //    StoreImageryCheck = false;
                                            //}
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].ToLower()))
                                                {
                                                    if (CheckSampleSize(tblcolumns[i]))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                            xmlstring = CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                                            if (!CheckSharedStringValue(xmlstring))
                                                            {
                                                                AddToSharedString(xmlstring);
                                                            }

                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " t=\"s\"  s = \"17\">" +
                                                              "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
    " s = \"" + cellfontstyle.ToString() + "\" t=\"s\">" +
    "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
    "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstyle.ToString() + "\">" +
                                                                "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                "</c> ");
                                                            }
                                                        }

                                                    }
                                                    else if (CommonFunctions.CheckMediumSampleSize(tblcolumns[i], sampleSize))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            xmlstring = CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                                            if (!CheckSharedStringValue(xmlstring))
                                                            {
                                                                AddToSharedString(xmlstring);
                                                            }
                                                            
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            " t=\"s\" s = \"17\">" +
                                                              "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }

                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
    " s = \"" + cellfontstyle.ToString() + "\"  t= \"s\">" +
    "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
    "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                                "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                "</c> ");
                                                            }
                                                        }
                                                    }

                                                    else
                                                    {

                                                        string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                        //if (i == 0)
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + (Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) ? "color: blue;" : "color: #black;") + " \"><span>" + na + "</span></li>";
                                                        //}
                                                        //else
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + " color: black;\"><span style=\"\">" + na + "</span></li>";
                                                        //}
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + GetCellColor("", "", "") + " \"><span>" + na + "</span></li>";
                                                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + " color: black;\"><span></span></li>");
                                                        if (!string.IsNullOrEmpty(na))
                                                        {
                                                            GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                            if (!CheckSharedStringValue(na))
                                                            {
                                                                AddToSharedString(na);
                                                            }
                                                            if (cellfontstyle == 30)
                                                            {
                                                                xmltext.Append("<c " +
                                            "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                            "s = \"" + cellfontstyle + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + GetSharedStringKey(na) + "</v>" +
                                        "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c " +
                                             "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                             "s = \"" + (average == "Average" ? "16" : "4") + "\" " +
                                             "t = \"s\">" +
                                             "<v>" + GetSharedStringKey(na) + "</v>" +
                                         "</c> ");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"" + (average == "Average" ? "17" : "8") + "\">" +
                                                                    "<v></v>" +
                                                                    "</c> ");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span style=\"\"></span></li>";
                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstyle + "\">" +
                                                                         "<v></v>" +
                                                                            "</c> ");
                                                }
                                            }

                                        }
                                        xmltext.Append("</row>");
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                    }
                                }
                            }

                        }

                    }
                }
                else
                {
                    tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:center\"><span>No data available</span></li></ul></div>");
                    int nodatarow = 9;

                    string metricitem = "No data available";
                    xmlstring = cf.cleanExcelXML(metricitem);

                    if (!CheckSharedStringValue(xmlstring))
                    {
                        AddToSharedString(xmlstring);
                    }
                    xmltext.Append("<row " +
                                    "r = \"" + nodatarow.ToString() + "\" " +
                                    "spans = \"1:11\" " +
                                    "ht = \"15\" " +
                                    "thickBot = \"1\" " +
                                    "x14ac:dyDescent = \"0.3\">" +
                                    "<c " +
                                        "r = \"C" + nodatarow.ToString() + "\" " +
                                        "t = \"s\">" +
                                        "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                    "</c></row> ");
                }

                //tbltext.Append("</tbody>";
                //leftbody += "</tbody></table>";
                //righttbody += "</tbody>";
                if (isBeverageDetail)
                {
                    if (!CheckSharedStringValue(Get_Beverage_Liquid_Flavor_Enhancer_Note()))
                    {
                        AddToSharedString(Get_Beverage_Liquid_Flavor_Enhancer_Note());
                    }
                    xmltext.Append("<row " +
                                        "r = \"" + (rownumber + 2) + "\" " +
                                        "spans = \"1:11\" " +
                                        "ht = \"15\" " +
                                        "thickBot = \"1\" " +
                                        "x14ac:dyDescent = \"0.3\">" +
                                        "<c " +
                                            "r = \"A" + (rownumber + 2).ToString() + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + GetSharedStringKey(Get_Beverage_Liquid_Flavor_Enhancer_Note()) + "</v>" +
                                        "</c></row> ");
                }
                xmltext.Append("</sheetData>");

                if (mergeCell.Count > 0)
                {
                    string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                    foreach (string mergrrow in mergeCell)
                    {
                        mergetext += mergrrow;

                    }
                    mergetext += "</mergeCells>";
                    xmltext.Append(mergetext);
                }

                xmltext.Append(GetPageMargins());
                string _xmltext = xmltext.ToString();
                xmltext = new StringBuilder();
                xmltext.Append(GetSheetHeadandColumns() + _xmltext.ToString());
                //Nagaraju 27-03-2014
                xmlstring = xmltext.ToString();
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                //exportfiles = new Dictionary<string, string>();
                //exportfiles.Add("tab1", xmltext);
                //HttpContext.Current.Session["exportfiles"] = exportfiles;
                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }

        public override iSHOPParams BindTabsWithin(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string timePeriod, string _ShopperSegment, string _SingleSelection, string _ShopperFrequency, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, bool ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams)
        {
            View_Type = "TREND";
            table_Params = tableParams;
            iSHOPParams ishopParams = new iSHOPParams();
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
            frequency = _ShopperFrequency;
            PopulateShortNames();
            Shortnames();
            BenchMark = ShortNames[0];

            if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = tableParams.CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedStatTest = Selected_StatTest;
            }

            Retailerlist = _Comparisonlist;
            param = new iSHOPParams();
            param.BenchMark = _BenchMark;
            //complist = new List<string>();
            string[] complistaArray = new string[0];
            var query = from r in ShortNames select r;
             //;
            //complist = query.ToList();
            complistaArray = query.ToArray();
            complist = query.ToList();
            ul_row_width = Math.Round(Convert.ToDouble(ulwidth), 0);
            ul_cell_width = Math.Round(Convert.ToDouble(ulliwidth), 0);
            CheckString = _ShopperSegment;
            TimePeriod = TimePeriodShortName;
            param.ShopperSegment = _SingleSelection;
            param.ShopperFrequency = _ShopperFrequency;
            param.CustomFilters = filterShortname;

            BenchmarkorComparisionList = _Comparisonlist.ToList();
            BenchmarkorComparisionList.Insert(0, _BenchMark);
            _ShopperSegment = string.Join("||", _ShopperSegment.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            DataAccess dal = new DataAccess();
            object[] paramvalues = null;
            DataSet ds = null;
            DataSet ds_2 = null;
            DataTable tbl_Common_Sample_Size = null;
            common_SampleSize = false;
            if (tableParams.Tab_Id_mapping)
            {
                string benchmark_UID = string.Empty;
                string comp_UID = string.Empty;
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    benchmark_UID = tableParams.CustomBase_UniqueId;
                    List<string> comp_list = (from r in tableParams.Comparison_UniqueIds where r != benchmark_UID select r).ToList();
                    comp_UID = string.Join("|", comp_list);
                }
                else
                {
                    benchmark_UID = tableParams.Comparison_UniqueIds[0];
                    tableParams.Comparison_UniqueIds.RemoveAt(0);
                    comp_UID = string.Join("|", tableParams.Comparison_UniqueIds);
                }
                TabIndexId = int.Parse(tableParams.TabIndexId);
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, tableParams.ShopperSegment_UniqueId.ToMyString(), benchmark_UID.ToMyString(), comp_UID.ToMyString(), tableParams.TimePeriod_UniqueId.ToMyString(), tableParams.Filter_UniqueId, tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString(), tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };
                }
                else
                {
                    paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, tableParams.ShopperSegment_UniqueId.ToMyString(), benchmark_UID.ToMyString(), comp_UID.ToMyString(), tableParams.TimePeriod_UniqueId.ToMyString(), tableParams.Filter_UniqueId, tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString() };
                }
                ds_2 = dal.GetData_WithIdMapping(paramvalues, tabid);
                if (ds_2 != null && ds_2.Tables.Count > 0)
                {
                    ds = new DataSet();
                    var query2 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    var query4 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    if (query4 != null && query4.Count > 0)
                        common_SampleSize = false;
                    else if (query2 != null && query2.Count > 0)
                        common_SampleSize = true;

                    if (query2 != null && query2.Count > 0)
                    {
                        tbl_Common_Sample_Size = query2.CopyToDataTable();
                    }
                    sampleSize = new Dictionary<string, string>();
                    if (common_SampleSize)
                    {
                        var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                                where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                                && Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                                select row).Distinct().ToList();
                        if (query_samplesize != null && query_samplesize.Count > 0)
                        {
                            DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                            foreach (object column in trips_samplesize.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                        else
                        {
                            foreach (object column in tbl_Common_Sample_Size.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                    }
                    List<string> metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                               where !Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                               select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
            }
            else
            {
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    _BenchMark = tableParams.CustomBase_DBName;
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                else
                {
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { _BenchMark.Replace("~", "`"), String.Join("|", _Comparisonlist).Replace("~", "`"), timePeriod, _ShopperSegment, _ShopperFrequency, _filter, Selected_StatTest, tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };                  
                }
                else
                {
                    paramvalues = new object[] { _BenchMark.Replace("~", "`"), String.Join("|", _Comparisonlist).Replace("~", "`"), timePeriod, _ShopperSegment, _ShopperFrequency, _filter, Selected_StatTest };
                }
                ds = dal.GetData(paramvalues, tabid);
            }
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            isBeverageDetail = false;
            if (tabid.Equals("sp_FactBookTripBevDetailsBeverageWithinMain", StringComparison.OrdinalIgnoreCase))
                isBeverageDetail = true;
            //
            if (IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency/imagery") > -1 || IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency") > -1)
            {
                IsStoreImagery = "StoreImagery";
                if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                {
                    _BenchMark = tableParams.CustomBase_DBName;
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                else
                {
                    _Comparisonlist = (from r in _Comparisonlist where r != _BenchMark select r).ToArray();
                }
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { Convert.ToString(_BenchMark).ToMyString().Replace("~", "`"), String.Join("|", _Comparisonlist).Replace("~", "`"), timePeriod, _ShopperSegment, _ShopperFrequency, _filter, Selected_StatTest, tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };                }
                else
                {
                    paramvalues = new object[] { Convert.ToString(_BenchMark).ToMyString().Replace("~", "`"), String.Join("|", _Comparisonlist).Replace("~", "`"), timePeriod, _ShopperSegment, _ShopperFrequency, _filter, Selected_StatTest };
                }
                //DataSet LoyaltyDs = dal.GetData(paramvalues, "sp_FactBookRespStoreWithinChannelMapping");
                LoyaltyRetailerList = new Hashtable();
                //foreach (DataRow row in LoyaltyDs.Tables[0].Rows)
                //{
                //    if (!Convert.ToString(row["Flag"]).Equals(GlobalVariables.NA) && !LoyaltyRetailerList.ContainsKey(Convert.ToString(row["Flag"])))
                //        LoyaltyRetailerList.Add(Get_ShortNames(Convert.ToString(row["Flag"])), Convert.ToString(row["DisplayMetricName"]));
                //}
            }
            int excelcolumnindex = 1;
            int rownumber = 6;

            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);
            //Nagaraju 27-03-2014
            if (ExportToExcel)
            {
                if (HttpContext.Current.Session["sharedstrings"] != null)
                {
                    sharedStrings = HttpContext.Current.Session["sharedstrings"] as Dictionary<string, int>;
                }
            }
            //End
            tbltext = new StringBuilder();
            string Significance = string.Empty;
            xmlstring = string.Empty;
            colmaxwidth = 0;
            //string xmltext = string.Empty;
            StringBuilder xmltext = new StringBuilder();
            mergeCell = new List<string>();

            try
            {
                xmltext.Append("<sheetData>");
                //write top header
                xmltext.Append(WriteFilters());
                xmltext.Append(AddSampleSizeNote());
                xmltext.Append(GetTableHeader(complistaArray.Count(),tableParams.ViewType));

                if (complistaArray.Count() > 1)
                {
                    mergeCell.Add("<mergeCell ref = \"B5:" + ColumnIndexToName(complistaArray.Count()) + "5\"/>");
                }
                //if (!CheckSharedStringValue("BENCHMARK"))
                //{
                //    AddToSharedString("BENCHMARK");
                //}

                //if (!CheckSharedStringValue("COMPARISON AREAS"))
                //{
                //    AddToSharedString("COMPARISON AREAS");
                //}

                //write second header
                excelcolumnindex = 0;
                xmltext.Append(" <row" +
               " r = \"" + rownumber + "\" " +
                "spans = \"1:11\" " +
                "x14ac:dyDescent = \"0.25\">" +
               " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"5\"/>");
                excelcolumnindex += 1;

                tbltext.Append("<thead>");
                tbltext.Append(CreateFirstTableHeader());
                tbltext.Append("<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><span>" + frequency + "</span></li>");

                leftheader += "<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><a class=\"table-top-title-bottom-line\"></a><span style=\"\">" + (tableParams.IsWherePurchased ? string.Empty : frequency) + "</span></li></ul></div>";
                rightheader += "<div class=\"rowitem\"><ul style=\"\">";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                for (int i = 0; i < complistaArray.Count(); i++)
                {
                    colNames = Get_ShortNames(complistaArray[i]) + AddTradeAreaNoteforChannel(Get_ShortNames(complistaArray[i]));
                    if (i == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(complistaArray[i] + "header");
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }

                    tbltext.Append("<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>");
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                    xmlstring = cf.cleanExcelXML(colNames);

                    if (!CheckSharedStringValue(xmlstring))
                    {
                        AddToSharedString(xmlstring);
                    }

                    xmltext.Append(" <c" +
                      " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                      " s = \"4\" " +
                      " t = \"s\">" +
                       "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                   "</c>");
                    excelcolumnindex += 1;
                }
                tbltext.Append("</ul></div>");
                rightheader += "</ul></div>";
                //add check sample size
                tbltext.Append("<div class=\"rowitem\"><ul>");
                xmltext.Append("</row>");

                List<iSHOPParams> iSHOPParamlist = null;
                SampleSize checksampleSize = new SampleSize(); bool isOnlineTab = tableParams.TabName.ToLower() == "online metrics";
                string smapleSizeText = (isOnlineTab ? "Sample Size: Retailer’s " + tableParams.ShopperFrequency + " shoppers who ALSO shop for online for grocery at any retailer every month" : "Sample Size");
                iSHOPParamlist = checksampleSize.CheckWithinRetailerSampleSize(checksamplesizesp, _BenchMark, _Comparisonlist, timePeriod, _ShopperSegment, _ShopperFrequency, _filter, ShortNames, tableParams.Tab_Id_mapping,tbl_Common_Sample_Size, tableParams);
                rownumber += 1;
                excelcolumnindex = 0;
                xmlstring = cf.cleanExcelXML(smapleSizeText);

                if (!CheckSharedStringValue(xmlstring))
                {
                    AddToSharedString(xmlstring);
                }

                xmltext.Append("<row " +
                                     "r = \"" + rownumber + "\" " +
                                     "spans = \"1:11\" " +
                                     "ht = \"15\" " +
                                     "thickBot = \"1\" " +
                                     "x14ac:dyDescent = \"0.3\">" +
                                     " <c" +
                                     " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                     " s = \"3\" " +
                                     " t = \"s\">" +
                                    "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                    "</c>");
                if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                {
                    tbltext.Append("<li style=\"\"><span>" + smapleSizeText + "</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>" + smapleSizeText + "</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    foreach (iSHOPParams para in iSHOPParamlist)
                    {
                        excelcolumnindex += 1;
                        if (iSHOPParamlist.IndexOf(para) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\">" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>";

                        }
                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>");

                        string lowsamplesize = CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize));
                        if (!string.IsNullOrEmpty(lowsamplesize))
                        {
                            lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(para.SampleSize))) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize));
                            if (!CheckSharedStringValue(lowsamplesize))
                            {
                                AddToSharedString(lowsamplesize);
                            }

                            xmltext.Append(" <c" +
                                                                   " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\" " +
                                                                   " t = \"s\">" +
                                                                   "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                   "</c>");

                        }
                        else
                        {
                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                     " s = \"2\">" +
                                                                "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(para.SampleSize).Replace(",", ""), out IsApplicable) + "</v>" +
                                                                      "</c> ");
                        }
                    }
                    rightheader += "</ul></div>";
                }
                else
                {
                    tbltext.Append("<li style=\"\"><span>" + smapleSizeText + "</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>" + smapleSizeText + "</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    for (int j = 0; j < complistaArray.Count(); j++)
                    {
                        excelcolumnindex += 1;
                        colNames = Get_ShortNames(complistaArray[j].Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                        if (j == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(complistaArray[j]) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>";
                        }

                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>");
                        xmlstring = string.Empty;

                        if (!CheckSharedStringValue(xmlstring))
                        {
                            AddToSharedString(xmlstring);
                        }

                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                 " s = \"2\">" +
                                                            "<v></v>" +
                                                                  "</c> ");
                    }
                    rightheader += "</ul></div>";
                }
                tbltext.Append("</ul></div>");
                tbltext.Append("");
                tbltext.Append("<tbody>");
                xmltext.Append("</row>");

                leftbody = "";
                //righttbody += "<div class=\"rowitem\"><ul style=\"\">";

                benchmark_comp_class = string.Empty;
                //end header
                table_count = 0;
                rows_count = 0;
                //------->
                if (ds != null && ds.Tables.Count > 0)
                {
                    table_count = ds.Tables.Count;
                    int colms = ds.Tables[0].Columns.Count;
                    //rownumber = 7;
                    for (int tbl = 0; tbl < table_count; tbl++)
                    {
                        if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                        {
                            isItemHasSpace = false;
                            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
                            //Date: 21-03-2016
                            Check_Beverage_Liquid_Flavor_Enhancer(Convert.ToString(ds.Tables[tbl].Rows[0][0]), _ShopperSegment);
                            //
                            excelcolumnindex = 0;
                            if (!common_SampleSize)
                            {
                                sampleSize = new Dictionary<string, string>();
                            }
                            rownumber += 1;
                            LoyaltyPyramid = false;
                            CheckBeverageTripNA = false;
                            LoyaltyPyramidmetric = Convert.ToString(ds.Tables[tbl].Rows[0][0]);
                            //if (LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(Base:CouldShop)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supermarket)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(convenience)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(drug store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(dollar store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(club)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(mass merchandise)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supercenter)")
                            //{
                            //    LoyaltyPyramid = true;
                            //}

                            //switch (LoyaltyPyramidmetric)
                            //{
                            //    //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                            //    case "RetailerLoyaltyPyramid(supermarket)":
                            //    case "RetailerLoyaltyPyramid(convenience)":
                            //    case "RetailerLoyaltyPyramid(drug store)":
                            //    case "RetailerLoyaltyPyramid(dollar store)":
                            //    case "RetailerLoyaltyPyramid(club)":
                            //    case "RetailerLoyaltyPyramid(mass merchandise)":
                            //    case "RetailerLoyaltyPyramid(supercenter)":
                            //        {
                            //            LoyaltyPyramid = true;
                            //            break;
                            //        }
                            //}

                            //switch (LoyaltyPyramidmetric)
                            //{
                            //    //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                            //    case "RetailerLoyaltyPyramid(supermarket)":
                            //    case "RetailerLoyaltyPyramid(convenience)":
                            //    case "RetailerLoyaltyPyramid(drug store)":
                            //    case "RetailerLoyaltyPyramid(dollar store)":
                            //    case "RetailerLoyaltyPyramid(club)":
                            //    case "RetailerLoyaltyPyramid(mass merchandise)":
                            //    case "RetailerLoyaltyPyramid(supercenter)":
                            //        {
                            //            LoyaltyPyramidForRetailers = true;
                            //            break;
                            //        }
                            //    default:
                            //        LoyaltyPyramidForRetailers = false;
                            //        break;
                            //}

                            switch (LoyaltyPyramidmetric)
                            {
                                case "Product Temperature":
                                case "Chilled - Location":
                                case "Room Temperature Location":
                                case "Intended Consumer":
                                    {
                                        CheckBeverageTripNA = true;
                                        break;
                                    }
                                default:
                                    CheckBeverageTripNA = false;
                                    break;
                            }
                            Table_Header_TotalUS_StyleId = 15;
                            Table_Header_BackgroundColor = "#D9E1EE";                          
                            Table_Header_BorderTopColor = "skyblue";
                            Table_Header_BottomTitleColor = "#72aaff";
                            if (IsStoreFrequencyTotalUS(LoyaltyPyramidmetric))
                            {
                                Table_Header_TotalUS_StyleId = 38;
                                Table_Header_BackgroundColor = "#E1E1E2; border-bottom: 1px solid darkgray;";
                                Table_Header_BorderTopColor = "#E1E1E2";
                                Table_Header_BottomTitleColor = "darkgrey";
                            }

                            tbltext.Append("<div class=\"rowitem table-title\"><ul><li style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><a class=\"table-title-bottom-line\" style=\"background-color:" + Table_Header_BottomTitleColor + "\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>");
                            leftbody += "<div class=\"rowitem table-title\"><ul><li style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><a class=\"table-title-bottom-line\" style=\"background-color:" + Table_Header_BottomTitleColor + "\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>";
                            righttbody += "<div class=\"rowitem table-title\"><ul style=\"\">";
                            for (int i = 0; i < complistaArray.Count(); i++)
                            {
                                if (i == 0)
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span></span></li>";
                                }
                                else
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span  style=\"\"></span></li>";
                                }
                                tbltext.Append("<li class=\"" + CleanClass(complistaArray[i] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span></span></li>");

                            }
                            leftbody += "</ul></div>";
                            righttbody += "</ul></div>";

                            tbltext.Append("</ul></div>");

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                            xmlstring = cf.cleanExcelXML(Check_Beverage_Liquid_Flavor_Enhancer_NA_Table(tablename));

                            if (!CheckSharedStringValue(xmlstring))
                            {
                                AddToSharedString(xmlstring);
                            }


                            //write table name
                            List<string> tblcolumns = new List<string>();
                            //foreach (object col in ds.Tables[tbl].Columns)
                            //{
                            //    string coln = Convert.ToString(col);
                            //    tblcolumns.Add(coln.Trim().ToLower().ToString());
                            //}

                            tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                            List<string> comp = (from r in complistaArray select r.ToLower()).ToList();
                            tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();

                            xmltext.Append("<row " +
                       "r = \"" + rownumber + "\" " +
                    "spans = \"1:11\" " +
                   " ht = \"15\" " +
                    "thickBot = \"1\" " +
                   " x14ac:dyDescent = \"0.3\">" +
                    "<c " +
                        "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                        "s = \"" + (LoyaltyPyramid ? 16 : Table_Header_TotalUS_StyleId) + "\" " +
                        "t = \"s\">" +
                        "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                    "</c>");

                            //for (int i = 0; i < complistaArray.Count; i++)
                            //{
                            //    excelcolumnindex += 1;
                            //    xmltext.Append("<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : 15) + "\"/>");
                            //}

                            xmltext.Append(String.Join("", complistaArray.Select(x => "<c r = \" " + ColumnIndexToName(excelcolumnindex++) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : Table_Header_TotalUS_StyleId) + "\"/>")));

                            xmltext.Append("</row>");
                            rows_count = ds.Tables[tbl].Rows.Count;
                            for (int rows = 0; rows < rows_count; rows++)
                            {
                                excelcolumnindex = 0;
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                MetricItem = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext.Append("<div class=\"rowitem\"><ul>");
                                    leftbody += "<div class=\"rowitem\"><ul>";
                                    righttbody += "<div class=\"rowitem\"><ul style=\"\">";
                                    //if (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average Number of Items" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average time in store" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Amount Spent")
                                    //{
                                    //    average = "Average";
                                    //}
                                    //else
                                    //{
                                    //    average = "";
                                    //}

                                    average = "";
                                    switch (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]))
                                    {
                                        case "Approximate Average Number of Items":
                                        case "AVERAGE ONLINE BASKET SIZE":
                                        case "Approximate Average time in store":
                                        case "Approximate Amount Spent":
                                        case "AVERAGE ONLINE ORDER SIZE":
                                            {
                                                average = "Average";
                                                break;
                                            }
                                    }

                                    //write sample size
                                    if (String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Number of Trips", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "SampleSize", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Sample Size", true) == 0
                                         || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("SampleSize", StringComparison.OrdinalIgnoreCase) >= 0 || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Sample Size", StringComparison.OrdinalIgnoreCase) >= 0
                                        || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Number of Trips", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sampleSize = new Dictionary<string, string>();
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>");

                                        leftbody += "<li style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<"));

                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem);
                                        if (!CheckSharedStringValue(xmlstring))
                                        {
                                            AddToSharedString(xmlstring);
                                        }

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"3\" " +
                                           "t = \"s\">" +
                                           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                       "</c> ");

                                        //plot sample size
                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].ToLower()))
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]),out samplecellstyle,isBeverageDetail,isLiquidFlavorEnhancer,LoyaltyPyramid) + "</span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span  style=\"\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]),out samplecellstyle,isBeverageDetail,isLiquidFlavorEnhancer,LoyaltyPyramid) + "</span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]),out samplecellstyle,isBeverageDetail,isLiquidFlavorEnhancer,LoyaltyPyramid) + "</span></li>");
                                                    sampleSize.Add(tblcolumns[i], Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                    if (samplecellstyle == 20)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])));
                                                        if (!CheckSharedStringValue(lowsamplesize))
                                                        {
                                                            AddToSharedString(lowsamplesize);
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else if (samplecellstyle == 30)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])));
                                                        if (!CheckSharedStringValue(lowsamplesize))
                                                        {
                                                            AddToSharedString(lowsamplesize);
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else
                                                    {

                                                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\">" +
                                                              "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                    "</c> ");
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span></span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span  style=\"\"></span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                    " s = \"2\">" +
                                                      "<v></v>" +
                                                         "</c> ");
                                                }
                                            }
                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                        xmltext.Append("</row>");
                                        //End Sample Size
                                    }

                                    else
                                    {
                                        isitembold = false;
                                        if (IsItemBold(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[rows]["Metric"])))
                                            leftbody += "<li class=\"itembold\" style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";
                                        else
                                            leftbody += "<li style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";

                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>");

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<"));
                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        if (isitembold)
                                        {
                                            isItemHasSpace = true;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem);
                                        if (!CheckSharedStringValue(xmlstring))
                                        {
                                            AddToSharedString(xmlstring);
                                        }
                                        string metricstyleid = "5";
                                        if (isitembold)                                       
                                            metricstyleid = "36";                                 
                                        else if (!isitembold && isItemHasSpace)
                                            metricstyleid = "37";

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"" + (average == "Average" ? "16" : metricstyleid) + "\" " +
                                           "t = \"s\">" +
                                           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                       "</c> ");
                                        //cellfontstyle = 8;

                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            BenchmarkOrComparison = tblcolumns[i];
                                            RetailerNetCheck = false;
                                            //if (CheckString.IndexOf("Retailers") > -1 && (IsStoreImagery.IndexOf("StoreImagery") > -1) && LoyaltyPyramidForRetailers)
                                            //{
                                            //    StoreImageryCheck = true;
                                            //    CheckRetailerorChannel = false;
                                            //}
                                            //else if ((CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            //{
                                            //    RetailerNetCheck = true;
                                            //    CheckRetailerorChannel = true;
                                            //}
                                            //else if ((CheckString.IndexOf("Channels") > -1 || CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            //{
                                            //    CheckRetailerorChannel = true;
                                            //    StoreImageryCheck = false;
                                            //}
                                            //else
                                            //{
                                            //    CheckRetailerorChannel = false;
                                            //    StoreImageryCheck = false;
                                            //}

                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(tblcolumns[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(tblcolumns[i]))
                                            {
                                                if (tblcolumns.Contains(tblcolumns[i].ToLower()))
                                                {
                                                    if (CheckSampleSize(tblcolumns[i]))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                            xmlstring = CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                                            if (!CheckSharedStringValue(xmlstring))
                                                            {
                                                                AddToSharedString(xmlstring);
                                                            }

                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " t=\"s\"  s = \"17\">" +
                                                              "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"" + cellfontstyle.ToString() + "\" t=\"s\">" +
                                                                 "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                    "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }

                                                            //                                                        
                                                        }

                                                    }
                                                    else if (CommonFunctions.CheckMediumSampleSize(tblcolumns[i],sampleSize))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");
                                                            xmlstring = CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                                            if (!CheckSharedStringValue(xmlstring))
                                                            {
                                                                AddToSharedString(xmlstring);
                                                            }
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            " t=\"s\" s = \"17\">" +
                                                              "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (isBeverageDetail && isLiquidFlavorEnhancer)
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                                else
                                                                {
                                                                    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>";
                                                                }
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (tblcolumns[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]))) + "</span></li>");

                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
    " s = \"" + cellfontstyle.ToString() + "\"  t= \"s\">" +
    "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
    "</c> ");
                                                            }
                                                            else
                                                            {

                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }
                                                        }

                                                    }

                                                    else
                                                    {
                                                        string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                        //if (i == 0)
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + (Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) ? "color: blue;" : "color: black;") + "\"><span>" + na + "</span></li>";
                                                        //}
                                                        //else
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + " color: black;\"><span style=\"\">" + na + "</span></li>";
                                                        //}
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + GetCellColor("", "", "") + " \"><span>" + na + "</span></li>";
                                                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + " color: black;\"><span></span></li>");
                                                        if (!string.IsNullOrEmpty(na))
                                                        {
                                                            GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(tblcolumns[i])]));
                                                            if (!CheckSharedStringValue(na))
                                                            {
                                                                AddToSharedString(na);
                                                            }
                                                            if (cellfontstyle == 30)
                                                            {
                                                                xmltext.Append("<c " +
                                            "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                            "s = \"" + cellfontstyle + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + GetSharedStringKey(na) + "</v>" +
                                        "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c " +
                                             "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                             "s = \"" + (average == "Average" ? "16" : "4") + "\" " +
                                             "t = \"s\">" +
                                             "<v>" + GetSharedStringKey(na) + "</v>" +
                                         "</c> ");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"" + (average == "Average" ? "17" : "8") + "\">" +
                                                                    "<v></v>" +
                                                                    "</c> ");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span style=\"\"></span></li>";
                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstyle + "\">" +
                                                                         "<v></v>" +
                                                                            "</c> ");
                                                }
                                            }

                                        }
                                        xmltext.Append("</row>");
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                    }
                                }
                            }

                        }

                    }
                }
                else
                {
                    tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:center\"><span>No data available</span></li></ul></div>");
                    int nodatarow = 9;

                    string metricitem = "No data available";
                    xmlstring = cf.cleanExcelXML(metricitem);

                    if (!CheckSharedStringValue(xmlstring))
                    {
                        AddToSharedString(xmlstring);
                    }
                    xmltext.Append("<row " +
                                    "r = \"" + nodatarow.ToString() + "\" " +
                                    "spans = \"1:11\" " +
                                    "ht = \"15\" " +
                                    "thickBot = \"1\" " +
                                    "x14ac:dyDescent = \"0.3\">" +
                                    "<c " +
                                        "r = \"C" + nodatarow.ToString() + "\" " +
                                        "t = \"s\">" +
                                        "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                    "</c></row> ");
                }

                //tbltext.Append("</tbody>";
                //leftbody += "</tbody></table>";
                //righttbody += "</tbody>";
                if (isBeverageDetail)
                {
                    if (!CheckSharedStringValue(Get_Beverage_Liquid_Flavor_Enhancer_Note()))
                    {
                        AddToSharedString(Get_Beverage_Liquid_Flavor_Enhancer_Note());
                    }
                    xmltext.Append("<row " +
                                        "r = \"" + (rownumber + 2) + "\" " +
                                        "spans = \"1:11\" " +
                                        "ht = \"15\" " +
                                        "thickBot = \"1\" " +
                                        "x14ac:dyDescent = \"0.3\">" +
                                        "<c " +
                                             "r = \"A" + (rownumber + 2).ToString() + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + GetSharedStringKey(Get_Beverage_Liquid_Flavor_Enhancer_Note()) + "</v>" +
                                        "</c></row> ");
                }
                xmltext.Append("</sheetData>");

                if (mergeCell.Count > 0)
                {
                    string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                    foreach (string mergrrow in mergeCell)
                    {
                        mergetext += mergrrow;

                    }
                    mergetext += "</mergeCells>";
                    xmltext.Append(mergetext);
                }

                xmltext.Append(GetPageMargins());
                string _xmltext = xmltext.ToString();
                xmltext = new StringBuilder();
                xmltext.Append(GetSheetHeadandColumns() + _xmltext.ToString());
                //Nagaraju 27-03-2014
                xmlstring = xmltext.ToString();
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                //exportfiles = new Dictionary<string, string>();
                //exportfiles.Add("tab1", xmltext);
                //HttpContext.Current.Session["exportfiles"] = exportfiles;
                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }

        public override iSHOPParams BindTabsTimePeriod(out StringBuilder tbltext, out string xmlstring, string checksamplesizesp, string tabid, string _BenchMark, string[] _Comparisonlist, string _ShopperSegment, string _SingleSelection, string _ShopperFrequency, string _filter, string filterShortname, string[] ShortNames, string StatPositive, string StatNegative, bool ExportToExcel, string TimePeriodShortName, string ulwidth, string ulliwidth, string Selected_StatTest, string IsStoreImagery, TableParams tableParams)
        {
            View_Type = "TREND";
            table_Params = tableParams;
            iSHOPParams ishopParams = new iSHOPParams();
            sharedStrings = new Dictionary<string, int>();
            SelectedStatTest = Selected_StatTest;
           
            frequency = _ShopperFrequency;
            PopulateShortNames();
            Shortnames();
            BenchMark = _commonfunctions.Get_TableMappingNames(ShortNames[0].Trim());
            Retailerlist = _Comparisonlist;
            param = new iSHOPParams();
            param.BenchMark = _BenchMark;
            //TimePeriod = TimePeriodShortName;
            TimePeriod = tableParams.Comparison_ShortNames[0] + " To " + tableParams.Comparison_ShortNames[tableParams.Comparison_ShortNames.Count - 1];
            if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                BenchMark = tableParams.CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
            }
            else
            {
                BenchMark = ShortNames[0];
                SelectedStatTest = Selected_StatTest;
            }
            string[] complistaArray = new string[0];
            //complist = new List<string>();
            var query = from r in ShortNames select r;
            complistaArray = query.ToArray();
            complist = query.ToList();
            ul_row_width = Math.Round(Convert.ToDouble(ulwidth), 0);
            ul_cell_width = Math.Round(Convert.ToDouble(ulliwidth), 0);
            CheckString = _ShopperSegment;
            param.ShopperSegment = _SingleSelection;
            param.ShopperFrequency = _ShopperFrequency;
            param.CustomFilters = filterShortname;           
            _ShopperSegment = string.Join("||", _ShopperSegment.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
            DataAccess dal = new DataAccess();
            object[] paramvalues = null;
            DataSet ds = null;
            DataSet ds_2 = null;
            DataTable tbl_Common_Sample_Size = null;
            common_SampleSize = false;
            if (tableParams.Tab_Id_mapping)
            {
                TabIndexId = int.Parse(tableParams.TabIndexId);
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, tableParams.ShopperSegment_UniqueId.ToMyString(), tableParams.TimePeriodFrom_UniqueId.ToMyString(), tableParams.TimePeriodTo_UniqueId.ToMyString(), tableParams.Filter_UniqueId.ToMyString(), tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString(), tableParams.CustomBase_UniqueId.ToMyString(), tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };
                }
                else
                {
                    paramvalues = new object[] { tableParams.TabIndexId, tableParams.Beverage_UniqueId, tableParams.ShopperSegment_UniqueId.ToMyString(), tableParams.TimePeriodFrom_UniqueId.ToMyString(), tableParams.TimePeriodTo_UniqueId.ToMyString(), tableParams.Filter_UniqueId.ToMyString(), tableParams.ShopperFrequency_UniqueId.ToMyString(), tableParams.Sigtype_UniqueId.ToMyString(), tableParams.CustomBase_UniqueId.ToMyString() };
                }
                ds_2 = dal.GetData_WithIdMapping(paramvalues, tabid);
                if (ds_2 != null && ds_2.Tables.Count > 0)
                {
                    ds = new DataSet();
                    var query2 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    var query4 = (from row in ds_2.Tables[0].AsEnumerable()
                                  where Convert.ToString(row["MetricItem"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                  select row).Distinct().ToList();

                    if (query4 != null && query4.Count > 0)
                        common_SampleSize = false;
                    else if (query2 != null && query2.Count > 0)
                        common_SampleSize = true;

                    if (query2 != null && query2.Count > 0)
                    {
                        tbl_Common_Sample_Size = query2.CopyToDataTable();
                    }
                    sampleSize = new Dictionary<string, string>();
                    if (common_SampleSize)
                    {
                        var query_samplesize = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                      && Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query_samplesize != null && query_samplesize.Count > 0)
                        {
                            DataTable trips_samplesize = query_samplesize.CopyToDataTable();
                            foreach (object column in trips_samplesize.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(trips_samplesize.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                        else
                        {
                            foreach (object column in tbl_Common_Sample_Size.Columns)
                            {
                                if (!Convert.ToString(Convert.ToString(column)).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                    && !Convert.ToString(Convert.ToString(column)).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    sampleSize.Add(Convert.ToString(column).ToLower(), Convert.ToString(tbl_Common_Sample_Size.Rows[0][Convert.ToString(column)]));
                                }
                            }
                        }
                    }
                    List<string> metriclist = (from row in ds_2.Tables[0].AsEnumerable()
                                               where !Convert.ToString(row["Metric"]).Equals("Sample Size", StringComparison.OrdinalIgnoreCase)
                                               select Convert.ToString(row["Metric"])).Distinct().ToList();
                    foreach (string metric in metriclist)
                    {
                        var query3 = (from row in ds_2.Tables[0].AsEnumerable()
                                      where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                      select row).Distinct().ToList();
                        if (query3 != null)
                        {
                            ds.Tables.Add(query3.CopyToDataTable());
                        }
                    }
                }
            }
            else
            {
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { _BenchMark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), String.Join("|", _Comparisonlist).Replace("~", "`").Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), _ShopperSegment.Replace("~", "`"), _ShopperFrequency, _filter, Selected_StatTest, tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };
                }
                else
                {
                    paramvalues = new object[] { _BenchMark.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), String.Join("|", _Comparisonlist).Replace("~", "`").Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), _ShopperSegment.Replace("~", "`"), _ShopperFrequency, _filter, Selected_StatTest };
                }
                ds = dal.GetData(paramvalues, tabid);
            }
            if (IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency/imagery") > -1 || IsStoreImagery.Replace(" ", "").ToLower().IndexOf("storefrequency") > -1)
            {
                if (tabid.ToLower().IndexOf("retailer") > -1)
                {
                    paramvalues = new object[] { Convert.ToString(_BenchMark).ToMyString().Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), String.Join("|", _Comparisonlist).Replace("~", "`").Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), _ShopperSegment.Replace("~", "`"), _ShopperFrequency, _filter, Selected_StatTest, tableParams.CompetitorFrequency_UniqueId.ToMyString(), tableParams.CompetitorRetailer_UniqueId.ToMyString() };
                }
                else
                {
                    paramvalues = new object[] { Convert.ToString(_BenchMark).ToMyString().Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), String.Join("|", _Comparisonlist).Replace("~", "`").Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), _ShopperSegment.Replace("~", "`"), _ShopperFrequency, _filter, Selected_StatTest };
                }
                //DataSet LoyaltyDs = dal.GetData(paramvalues, "sp_FactBookRespStoreOverTimeChannelMapping");
                LoyaltyRetailerList = new Hashtable();
                //foreach (DataRow row in LoyaltyDs.Tables[0].Rows)
                //{
                //    if (!Convert.ToString(row["Flag"]).Equals(GlobalVariables.NA) && !LoyaltyRetailerList.ContainsKey(Convert.ToString(row["Flag"])))
                //        LoyaltyRetailerList.Add(Get_ShortNames(Convert.ToString(row["Flag"])), Convert.ToString(row["DisplayMetricName"]));
                //}
            }
            //if (_BenchMark.Equals("total|total", StringComparison.OrdinalIgnoreCase) && ds != null && ds.Tables.Count > 0)
            //{
            //    complistaArray = new List<string>();
            //    for (int i = 3; i < ds.Tables[0].Columns.Count; i++)
            //    {
            //        if (!complistaArray.Contains(Convert.ToString(ds.Tables[0].Columns[i])))
            //        {
            //            complistaArray.Add(Convert.ToString(ds.Tables[0].Columns[i]).Replace("~", "'"));
            //        }
            //    }
            //}
            complistaArray = new string[0];
            //for (int i = 3; i < ds.Tables[0].Columns.Count; i++)
            //{
            //    if (!complistaArray.Contains(Convert.ToString(ds.Tables[0].Columns[i])))
            //    {
            //        complistaArray.Add(Convert.ToString(ds.Tables[0].Columns[i]).Replace("~", "'"));
            //    }
            //}
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                complistaArray = ds.Tables[0].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName.ToLower())).Distinct().ToArray();
                List<string> comp = (from r in ShortNames select r.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 12MMT", "").Replace(" 6MMT", "").ToLower()).ToList();
                complistaArray = complistaArray.OrderBy(x => comp.IndexOf(x)).ToArray();
            }
            int excelcolumnindex = 1;
            int rownumber = 6;

            accuratestatvalueposi = Convert.ToDouble(StatPositive);
            accuratestatvaluenega = Convert.ToDouble(StatNegative);
            //Nagaraju 27-03-2014
            if (ExportToExcel)
            {
                if (HttpContext.Current.Session["sharedstrings"] != null)
                {
                    sharedStrings = HttpContext.Current.Session["sharedstrings"] as Dictionary<string, int>;
                }
            }
            //End
            tbltext = new StringBuilder();
            string Significance = string.Empty;
            xmlstring = string.Empty;
            colmaxwidth = 0;
            //string xmltext = string.Empty;
            StringBuilder xmltext = new StringBuilder();
            mergeCell = new List<string>();

            try
            {
                xmltext.Append("<sheetData>");
                //write top header
                xmltext.Append(WriteFilters());
                xmltext.Append(AddSampleSizeNote());
                xmltext.Append(GetTableHeader(complistaArray.Count(), tableParams.ViewType));

                if (complistaArray.Count() > 1)
                {
                    mergeCell.Add("<mergeCell ref = \"B5:" + ColumnIndexToName(complistaArray.Count()) + "5\"/>");
                }
                if (!CheckSharedStringValue("BENCHMARK"))
                {
                    AddToSharedString("BENCHMARK");
                }

                if (!CheckSharedStringValue("COMPARISON AREAS"))
                {
                    AddToSharedString("COMPARISON AREAS");
                }

                //write second header
                excelcolumnindex = 0;
                xmltext.Append(" <row" +
               " r = \"" + rownumber + "\" " +
                "spans = \"1:11\" " +
                "x14ac:dyDescent = \"0.25\">" +
               " <c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"5\"/>");
                excelcolumnindex += 1;

                tbltext.Append("<thead>");
                tbltext.Append(CreateFirstTableHeaderOvertime());
                tbltext.Append("<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><span>" + frequency + "</span></li>");

                leftheader += "<div class=\"rowitem\"><ul><li class=\"ShoppingFrequencyheader\" style=\"overflow: hidden;text-align: center;\"><a class=\"table-top-title-bottom-line\"></a><span style=\"\">" + (tableParams.IsWherePurchased ? string.Empty : frequency) + "</span></li></ul></div>";
                rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                //create header
                string colNames;

                //write comparison
                string benchmark_comp_class = string.Empty;
                for (int i = 0; i < complistaArray.Count(); i++)
                {
                    colNames = Get_ShortNames(complistaArray[i]) + AddTradeAreaNoteforChannel(Get_ShortNames(complistaArray[i]));
                    if (i == 0)
                    {
                        benchmark_comp_class = "benchmarkheader";
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }
                    else
                    {
                        benchmark_comp_class = CleanClass(complistaArray[i] + "header");
                        rightheader += "<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\"" + colNames + "\">" + colNames + "</span></li>";
                    }

                    tbltext.Append("<li class=\"" + CleanClass(benchmark_comp_class) + "\" style=\"overflow: hidden;text-align: center;\"><span title=\"" + colNames + "\">" + colNames + "</span></li>");
                    colNames = colNames.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                    xmlstring = cf.cleanExcelXML(colNames).ToUpper();

                    if (!CheckSharedStringValue(xmlstring))
                    {
                        AddToSharedString(xmlstring);
                    }

                    xmltext.Append(" <c" +
                      " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                      " s = \"4\" " +
                      " t = \"s\">" +
                       "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                   "</c>");
                    excelcolumnindex += 1;
                }
                tbltext.Append("</ul></div>");
                rightheader += "</ul></div>";
                //add check sample size
                tbltext.Append("<div class=\"rowitem\"><ul>");
                xmltext.Append("</row>");

                List<iSHOPParams> iSHOPParamlist = null;
                SampleSize checksampleSize = new SampleSize(); bool isOnlineTab = tableParams.TabName.ToLower() == "online metrics";
                string smapleSizeText = (isOnlineTab ? "Sample Size: Retailer’s " + tableParams.ShopperFrequency + " shoppers who ALSO shop for online for grocery at any retailer every month" : "Sample Size");
                iSHOPParamlist = checksampleSize.CheckTimePeriodSampleSize(checksamplesizesp, _BenchMark, Retailerlist, _ShopperSegment, _ShopperFrequency, _filter, ShortNames, tableParams.Tab_Id_mapping, tbl_Common_Sample_Size);
                rownumber += 1;
                excelcolumnindex = 0;
                xmlstring = cf.cleanExcelXML(smapleSizeText).ToUpper();

                if (!CheckSharedStringValue(xmlstring))
                {
                    AddToSharedString(xmlstring);
                }

                xmltext.Append("<row " +
                                     "r = \"" + rownumber + "\" " +
                                     "spans = \"1:11\" " +
                                     "ht = \"15\" " +
                                     "thickBot = \"1\" " +
                                     "x14ac:dyDescent = \"0.3\">" +
                                     " <c" +
                                     " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                     " s = \"3\" " +
                                     " t = \"s\">" +
                                    "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                    "</c>");
                if (iSHOPParamlist != null && iSHOPParamlist.Count > 0)
                {
                    tbltext.Append("<li style=\"\"><span>" + smapleSizeText + "</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>" + smapleSizeText + "</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    foreach (iSHOPParams para in iSHOPParamlist)
                    {
                        excelcolumnindex += 1;
                        if (iSHOPParamlist.IndexOf(para) == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(para.Retailer) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\">" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>";

                        }
                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span>" + CommonFunctions.CheckdecimalValue(para.SampleSize) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize)) + "</span></li>");

                        string lowsamplesize = CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize));
                        if (!string.IsNullOrEmpty(lowsamplesize))
                        {
                            lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(para.SampleSize))) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(para.SampleSize));
                            if (!CheckSharedStringValue(lowsamplesize))
                            {
                                AddToSharedString(lowsamplesize);
                            }

                            xmltext.Append(" <c" +
                                                                   " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\" " +
                                                                   " t = \"s\">" +
                                                                   "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                   "</c>");

                        }
                        else
                        {
                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                     " s = \"2\">" +
                                                                "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(para.SampleSize).Replace(",", ""), out IsApplicable) + "</v>" +
                                                                      "</c> ");
                        }
                    }
                    rightheader += "</ul></div>";
                }
                else
                {
                    tbltext.Append("<li style=\"\"><span>" + smapleSizeText + "</span></li>");
                    leftheader += "<div class=\"rowitem\"><ul><li style=\"\"><span>" + smapleSizeText + "</span></li></ul></div>";
                    rightheader += "<div class=\"rowitem\"><ul style=\"\" >";
                    for (int j = 0; j < complistaArray.Count(); j++)
                    {
                        excelcolumnindex += 1;
                        colNames = Get_ShortNames(complistaArray[j].Replace("~", "'").Replace("Channels|", "").Replace("Retailers|", "").Replace("Brand|", ""));
                        if (j == 0)
                        {
                            benchmark_comp_class = "benchmarkheader";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>";
                        }
                        else
                        {
                            benchmark_comp_class = CleanClass(complistaArray[j]) + "header";
                            rightheader += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span style=\"\" title=\" " + colNames + " \"></span></li>";
                        }

                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;\"><span title=\" " + colNames + " \"></span></li>");
                        xmlstring = string.Empty;

                        if (!CheckSharedStringValue(xmlstring))
                        {
                            AddToSharedString(xmlstring);
                        }

                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                 " s = \"2\">" +
                                                            "<v></v>" +
                                                                  "</c> ");
                    }
                    rightheader += "</ul></div>";
                }
                tbltext.Append("</ul></div>");
                tbltext.Append("");
                tbltext.Append("<tbody>");
                xmltext.Append("</row>");

                leftbody = "";
                //righttbody += "<div class=\"rowitem\"><ul style=\"\">";

                benchmark_comp_class = string.Empty;
                //end header

                //------->
                if (ds != null && ds.Tables.Count > 0)
                {
                    int colms = ds.Tables[0].Columns.Count;
                    //rownumber = 7;
                    for (int tbl = 0; tbl < ds.Tables.Count; tbl++)
                    {
                        if (ds.Tables[tbl] != null && ds.Tables[tbl].Rows.Count > 0)
                        {
                            isItemHasSpace = false;
                            excelcolumnindex = 0;
                            if (!common_SampleSize)
                            {
                                sampleSize = new Dictionary<string, string>();
                            }
                            rownumber += 1;
                            LoyaltyPyramid = false;
                            LoyaltyPyramidmetric = Convert.ToString(ds.Tables[tbl].Rows[0][0]);
                            //if (LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(Base:CouldShop)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supermarket)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(convenience)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(drug store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(dollar store)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(club)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(mass merchandise)"
                            //    || LoyaltyPyramidmetric == "RetailerLoyaltyPyramid(supercenter)")
                            //{
                            //    LoyaltyPyramid = true;
                            //}

                            //switch (LoyaltyPyramidmetric)
                            //{
                            //    //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                            //    case "RetailerLoyaltyPyramid(supermarket)":
                            //    case "RetailerLoyaltyPyramid(convenience)":
                            //    case "RetailerLoyaltyPyramid(drug store)":
                            //    case "RetailerLoyaltyPyramid(dollar store)":
                            //    case "RetailerLoyaltyPyramid(club)":
                            //    case "RetailerLoyaltyPyramid(mass merchandise)":
                            //    case "RetailerLoyaltyPyramid(supercenter)":
                            //        {
                            //            LoyaltyPyramid = true;
                            //            break;
                            //        }
                            //}

                            //switch (LoyaltyPyramidmetric)
                            //{
                            //    //case "RetailerLoyaltyPyramid(Base:CouldShop)":
                            //    case "RetailerLoyaltyPyramid(supermarket)":
                            //    case "RetailerLoyaltyPyramid(convenience)":
                            //    case "RetailerLoyaltyPyramid(drug store)":
                            //    case "RetailerLoyaltyPyramid(dollar store)":
                            //    case "RetailerLoyaltyPyramid(club)":
                            //    case "RetailerLoyaltyPyramid(mass merchandise)":
                            //    case "RetailerLoyaltyPyramid(supercenter)":
                            //        {
                            //            LoyaltyPyramidForRetailers = true;
                            //            break;
                            //        }
                            //    default:
                            //        LoyaltyPyramidForRetailers = false;
                            //        break;
                            //}
                            Table_Header_TotalUS_StyleId = 15;
                            Table_Header_BackgroundColor = "#D9E1EE";
                            Table_Header_BorderTopColor = "skyblue";
                            Table_Header_BottomTitleColor = "#72aaff";
                            if (IsStoreFrequencyTotalUS(LoyaltyPyramidmetric))
                            {
                                Table_Header_TotalUS_StyleId = 38;
                                Table_Header_BackgroundColor = "#E1E1E2; border-bottom: 1px solid darkgray;";
                                Table_Header_BorderTopColor = "#E1E1E2";
                                Table_Header_BottomTitleColor = "darkgrey";
                            }

                            tbltext.Append("<div class=\"rowitem table-title\"><ul><li style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>");
                            leftbody += "<div class=\"rowitem table-title\"><ul><li style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><a class=\"table-title-bottom-line\" style=\"background-color:" + Table_Header_BottomTitleColor + "\"></a><div class=\"treeview minusIcon\"></div><span> " + textInfo.ToTitleCase(Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString())) + " </span></li>";
                            righttbody += "<div class=\"rowitem table-title\"><ul style=\"\">";
                            for (int j = 0; j < complistaArray.Count(); j++)
                            {
                                if (j == 0)
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[j] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span></span></li>";
                                }
                                else
                                {
                                    righttbody += "<li class=\"" + CleanClass(complistaArray[j] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span  style=\"\"></span></li>";
                                }
                                tbltext.Append("<li class=\"" + CleanClass(complistaArray[j] + "cell") + "\" style=\"border-top:1px solid " + Table_Header_BorderTopColor + ";text-align:left;" + (LoyaltyPyramid ? "background-color: transparent" : "background-color: " + Table_Header_BackgroundColor + "") + ";color:#000000;\"><span></span></li>");

                            }
                            leftbody += "</ul></div>";
                            righttbody += "</ul></div>";

                            tbltext.Append("</ul></div>");

                            string tablename = Get_ShortNames(ds.Tables[tbl].Rows[0][0].ToString());
                            tablename = tablename.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                            xmlstring = cf.cleanExcelXML(tablename).ToUpper();

                            if (!CheckSharedStringValue(xmlstring))
                            {
                                AddToSharedString(xmlstring);
                            }


                            //write table name
                            List<string> tblcolumns = new List<string>();
                            //foreach (object col in ds.Tables[tbl].Columns)
                            //{
                            //    string coln = Convert.ToString(col);
                            //    tblcolumns.Add(coln.Trim().ToLower().ToString());
                            //}

                            tblcolumns = ds.Tables[tbl].Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();

                            xmltext.Append("<row " +
                       "r = \"" + rownumber + "\" " +
                    "spans = \"1:11\" " +
                   " ht = \"15\" " +
                    "thickBot = \"1\" " +
                   " x14ac:dyDescent = \"0.3\">" +
                    "<c " +
                        "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                        "s = \"" + (LoyaltyPyramid ? 16 : Table_Header_TotalUS_StyleId) + "\" " +
                        "t = \"s\">" +
                        "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                    "</c>");

                            //for (int i = 0; i < complistaArray.Count; i++)
                            //{
                            //    excelcolumnindex += 1;
                            //    xmltext.Append("<c r = \" " + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : 15) + "\"/>");
                            //}

                            xmltext.Append(String.Join("", complistaArray.Select(x => "<c r = \" " + ColumnIndexToName(excelcolumnindex++) + "" + rownumber + "\" s = \"" + (LoyaltyPyramid ? 16 : Table_Header_TotalUS_StyleId) + "\"/>")));

                            xmltext.Append("</row>");
                            for (int rows = 0; rows < ds.Tables[tbl].Rows.Count; rows++)
                            {
                                excelcolumnindex = 0;
                                DataRow dRow = ds.Tables[tbl].Rows[rows];
                                Significance = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                MetricItem = ds.Tables[tbl].Rows[rows]["MetricItem"].ToString();
                                if (!Significance.Trim().ToLower().Contains("significance"))
                                {
                                    rownumber += 1;
                                    //cellfontstyle = 2;
                                    tbltext.Append("<div class=\"rowitem\"><ul>");
                                    leftbody += "<div class=\"rowitem\"><ul>";
                                    righttbody += "<div class=\"rowitem\"><ul style=\"\">";
                                    //if (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average Number of Items" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Average time in store" || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]) == "Approximate Amount Spent")
                                    //{
                                    //    average = "Average";
                                    //}
                                    //else
                                    //{
                                    //    average = "";
                                    //}

                                    average = "";
                                    switch (Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]))
                                    {
                                        case "Approximate Average Number of Items":
                                        case "AVERAGE ONLINE BASKET SIZE":
                                        case "Approximate Average time in store":
                                        case "Approximate Amount Spent":
                                        case "AVERAGE ONLINE ORDER SIZE":
                                            {
                                                average = "Average";
                                                break;
                                            }
                                    }

                                    //write sample size
                                    //if (ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().ToLower() == "number of trips" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().ToLower() == "samplesize" || ds.Tables[tbl].Rows[rows]["MetricItem"].ToString().ToLower()=="sample size")
                                    if (String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Number of Trips", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "SampleSize", true) == 0 || String.Compare(ds.Tables[tbl].Rows[rows]["MetricItem"].ToString(), "Sample Size", true) == 0
                                         || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("SampleSize", StringComparison.OrdinalIgnoreCase) >= 0 || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Sample Size", StringComparison.OrdinalIgnoreCase) >= 0
                                        || Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).IndexOf("Number of Trips", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sampleSize = new Dictionary<string, string>();
                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left; color: #878787;font-weight: bold;\">" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>");

                                        leftbody += "<li style=\"overflow: hidden;text-align: left; color: #878787;font-weight: bold;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";

                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<"));

                                        if (metricitem.Length > colmaxwidth)
                                            colmaxwidth = metricitem.Length;

                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");
                                        xmlstring = cf.cleanExcelXML(metricitem).ToUpper();
                                        if (!CheckSharedStringValue(xmlstring))
                                        {
                                            AddToSharedString(xmlstring);
                                        }

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"3\" " +
                                           "t = \"s\">" +
                                           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                       "</c> ");

                                        //plot sample size
                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                                benchmark_comp_class = "benchmarkcell";
                                            else
                                                benchmark_comp_class = CleanClass(complistaArray[i] + "cell");

                                            if (!string.IsNullOrEmpty(complistaArray[i]))
                                            {
                                                if (tblcolumns.Contains(_commonfunctions.Get_TableMappingNames(complistaArray[i]).Trim().ToLower()))
                                                {
                                                    if (i == 0)
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out samplecellstyle,isBeverageDetail,isLiquidFlavorEnhancer,LoyaltyPyramid) + "</span></li>";
                                                    else
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span  style=\"\">" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out samplecellstyle, isBeverageDetail, isLiquidFlavorEnhancer, LoyaltyPyramid) + "</span></li>";

                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;font-weight: bold;\"><span>" + CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out samplecellstyle, isBeverageDetail, isLiquidFlavorEnhancer, LoyaltyPyramid) + "</span></li>");
                                                    sampleSize.Add(complistaArray[i], Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                    if (samplecellstyle == 20)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))).ToUpper();
                                                        if (!CheckSharedStringValue(lowsamplesize))
                                                        {
                                                            AddToSharedString(lowsamplesize);
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else if (samplecellstyle == 30)
                                                    {
                                                        string lowsamplesize = Convert.ToString(CommonFunctions.CheckdecimalValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])));
                                                        if (!CheckSharedStringValue(lowsamplesize))
                                                        {
                                                            AddToSharedString(lowsamplesize);
                                                        }
                                                        xmltext.Append(" <c" +
                                                                    " r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"2\" " +
                                                                    " t = \"s\">" +
                                                                    "<v>" + GetSharedStringKey(lowsamplesize) + "</v>" +
                                                                    "</c>");
                                                    }
                                                    else
                                                    {

                                                        xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"2\">" +
                                                              "<v>" + CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                    "</c> ");
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span></span></li>";

                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span  style=\"\"></span></li>";

                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent; color: #878787;font-weight: bold;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                    " s = \"2\">" +
                                                      "<v></v>" +
                                                         "</c> ");
                                                }
                                            }
                                        }
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                        xmltext.Append("</row>");
                                        //End Sample Size
                                    }

                                    else
                                    {
                                        isitembold = false;
                                        if (IsItemBold(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[rows]["Metric"])))
                                            leftbody += "<li class=\"itembold\" style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";
                                        else
                                            leftbody += "<li style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>";

                                        tbltext.Append("<li style=\"overflow: hidden;text-align: left;background-color: transparent;\"><span>" + Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<")) + "</span></li>");
                                        if (isitembold)
                                        {
                                            isItemHasSpace = true;
                                        }
                                        string metricitem = Get_ShortNames(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]).Replace("&lt;", "<"));
                                        if (metricitem.Length > colmaxwidth)
                                        {
                                            colmaxwidth = metricitem.Length;
                                        }
                                        metricitem = metricitem.Replace("<span style=\"font-size:15px;\">", "").Replace("</span>", "");                                       
                                     
                                            xmlstring = cf.cleanExcelXML(metricitem).ToUpper();

                                        if (!CheckSharedStringValue(xmlstring))
                                        {
                                            AddToSharedString(xmlstring);
                                        }
                                        string metricstyleid = "5";
                                        if (isitembold)
                                            metricstyleid = "36";
                                        else if (!isitembold && isItemHasSpace)
                                            metricstyleid = "37";

                                        xmltext.Append("<row " +
                                       "r = \"" + rownumber + "\" " +
                                       "spans = \"1:11\" " +
                                       "ht = \"15\" " +
                                       "thickBot = \"1\" " +
                                       "x14ac:dyDescent = \"0.3\">" +
                                       "<c " +
                                           "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                           "s = \"" + (average == "Average" ? "16" : metricstyleid) + "\" " +
                                           "t = \"s\">" +
                                           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                       "</c> ");
                                        //cellfontstyle = 8;

                                        for (int i = 0; i < complistaArray.Count(); i++)
                                        {
                                            BenchmarkOrComparison = complistaArray[i];
                                            RetailerNetCheck = false;
                                            //if (CheckString.IndexOf("Retailers") > -1 && (IsStoreImagery.IndexOf("StoreImagery") > -1 && LoyaltyPyramidForRetailers))
                                            //{
                                            //    StoreImageryCheck = true;
                                            //    CheckRetailerorChannel = false;
                                            //}
                                            //else if ((CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            //{
                                            //    RetailerNetCheck = true;
                                            //    CheckRetailerorChannel = true;
                                            //}
                                            //else if ((CheckString.IndexOf("Channels") > -1 || CheckString.IndexOf("RetailerNet") > -1) && (IsStoreImagery.IndexOf("StoreImagery") > -1))
                                            //{
                                            //    CheckRetailerorChannel = true;
                                            //    StoreImageryCheck = false;
                                            //}
                                            //else
                                            //{
                                            //    CheckRetailerorChannel = false;
                                            //    StoreImageryCheck = false;
                                            //}
                                            excelcolumnindex += 1;
                                            if (i == 0)
                                            {
                                                benchmark_comp_class = "benchmarkcell";
                                            }
                                            else
                                            {
                                                benchmark_comp_class = CleanClass(complistaArray[i] + "cell");
                                            }

                                            if (!string.IsNullOrEmpty(complistaArray[i]))
                                            {
                                                if (tblcolumns.Contains(_commonfunctions.Get_TableMappingNames(complistaArray[i]).Trim().ToLower()))
                                                {
                                                    if (CheckSampleSize(complistaArray[i]))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");

                                                            xmlstring = CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                                            if (!CheckSharedStringValue(xmlstring))
                                                            {
                                                                AddToSharedString(xmlstring);
                                                            }
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " t=\"s\"  s = \"17\">" +
                                                              "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span  style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;background-color: transparent;" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");
                                                            string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31 || na == GlobalVariables.NA)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                   " s = \"" + cellfontstyle.ToString() + "\" t=\"s\">" +
                                                                 "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                    "</c> ");
                                                            }
                                                            else
                                                            {

                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                    " s = \"" + cellfontstyle.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }

                                                            //                                                        if (cellfontstyle == 4)
                                                            //                                                        {
                                                            //                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            //" s = \"" + cellfontstyle.ToString() + "\" t=\"s\">" +
                                                            //"<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                            //"</c> ");
                                                            //                                                        }
                                                            //                                                        else
                                                            //                                                        {
                                                            //                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            //                                                            " s = \"" + cellfontstyle.ToString() + "\">" +
                                                            //                                                            "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                            //                                                            "</c> ");
                                                            //                                                        }
                                                        }

                                                    }
                                                    else if (CommonFunctions.CheckMediumSampleSize(complistaArray[i], sampleSize))
                                                    {
                                                        if (String.Compare(average, "Average", true) == 0)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");
                                                            xmlstring = CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]));

                                                            if (!CheckSharedStringValue(xmlstring))
                                                            {
                                                                AddToSharedString(xmlstring);
                                                            }
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                            " t=\"s\" s = \"17\">" +
                                                              "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                                                 "</c> ");
                                                        }
                                                        else
                                                        {
                                                            if (i == 0)
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            else
                                                            {
                                                                righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span style=\"\">" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>";
                                                            }
                                                            tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + "" + (complistaArray[i] == _BenchMark ? string.Empty : GetCellColorGrey(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + " \"><span>" + (average == "Average" ? CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]), tablename, Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"])) : CheckBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]))) + "</span></li>");
                                                            string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                            if (cellfontstyle == 4 || cellfontstyle == 30 || cellfontstyle == 31 || na == GlobalVariables.NA)
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
    " s = \"" + cellfontstyle.ToString() + "\"  t= \"s\">" +
    "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
    "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstylegrey.ToString() + "\">" +
                                                                  "<v>" + CheckXMLBlankValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])) + "</v>" +
                                                                     "</c> ");
                                                            }
                                                        }

                                                    }

                                                    else
                                                    {
                                                        string na = CheckNAValues(Convert.ToString(ds.Tables[tbl].Rows[rows][_commonfunctions.Get_TableMappingNames(complistaArray[i])])).ToUpper();
                                                        //if (i == 0)
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + (Convert.ToString(SelectedStatTest).Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase) ? "color: blue;" : "color: black;") + "\"><span>" + na + "</span></li>";
                                                        //}
                                                        //else
                                                        //{
                                                        //    righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + " color:black;\"><span style=\"\">" + na + "</span></li>";
                                                        //}
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + GetCellColor("", "", "") + " \"><span>" + na + "</span></li>";
                                                        tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center;" + (average == "Average" ? "background-color:transparent;" : "background-color: transparent;") + " color: black;\"><span></span></li>");
                                                        if (!string.IsNullOrEmpty(na))
                                                        {
                                                            GetCellColor(Convert.ToString(ds.Tables[tbl].Rows[rows]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)]["MetricItem"]), Convert.ToString(ds.Tables[tbl].Rows[GetRowNumber(ds.Tables[tbl], rows)][_commonfunctions.Get_TableMappingNames(complistaArray[i])]));
                                                            if (!CheckSharedStringValue(na))
                                                            {
                                                                AddToSharedString(na);
                                                            }
                                                            if (cellfontstyle == 30)
                                                            {
                                                                xmltext.Append("<c " +
                                            "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                            "s = \"" + cellfontstyle + "\" " +
                                            "t = \"s\">" +
                                            "<v>" + GetSharedStringKey(na) + "</v>" +
                                        "</c> ");
                                                            }
                                                            else
                                                            {
                                                                xmltext.Append("<c " +
                                             "r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                             "s = \"" + (average == "Average" ? "16" : "4") + "\" " +
                                             "t = \"s\">" +
                                             "<v>" + GetSharedStringKey(na) + "</v>" +
                                         "</c> ");

                                                            }
                                                        }
                                                        else
                                                        {
                                                            xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                        " s = \"" + (average == "Average" ? "17" : "8") + "\">" +
                                                                    "<v></v>" +
                                                                    "</c> ");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>";
                                                    }
                                                    else
                                                    {
                                                        righttbody += "<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span style=\"\"></span></li>";
                                                    }
                                                    tbltext.Append("<li class=\"" + benchmark_comp_class + "\" style=\"overflow: hidden;text-align: center; color: #878787;\"><span></span></li>");
                                                    xmltext.Append("<c r = \"" + ColumnIndexToName(excelcolumnindex) + "" + rownumber + "\" " +
                                                                " s = \"" + cellfontstyle + "\">" +
                                                                         "<v></v>" +
                                                                            "</c> ");
                                                }
                                            }

                                        }
                                        xmltext.Append("</row>");
                                        tbltext.Append("</ul></div>");
                                        leftbody += "</ul></div>";
                                        righttbody += "</ul></div>";
                                    }
                                }
                            }

                        }

                    }
                }
                else
                {
                    tbltext.Append("<div class=\"rowitem\"><ul><li style=\"text-align:center\"><span>No data available</span></li></ul></div>");
                    int nodatarow = 9;

                    string metricitem = "No data available";
                    xmlstring = cf.cleanExcelXML(metricitem).ToUpper();

                    if (!CheckSharedStringValue(xmlstring))
                    {
                        AddToSharedString(xmlstring);
                    }
                    xmltext.Append("<row " +
                                    "r = \"" + nodatarow.ToString() + "\" " +
                                    "spans = \"1:11\" " +
                                    "ht = \"15\" " +
                                    "thickBot = \"1\" " +
                                    "x14ac:dyDescent = \"0.3\">" +
                                    "<c " +
                                        "r = \"C" + nodatarow.ToString() + "\" " +
                                        "t = \"s\">" +
                                        "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
                                    "</c></row> ");
                }

                //tbltext.Append("</tbody>";
                //leftbody += "</tbody></table>";
                //righttbody += "</tbody>";

                xmltext.Append("</sheetData>");

                if (mergeCell.Count > 0)
                {
                    string mergetext = "<mergeCells count = \" " + mergeCell.Count + "\">";
                    foreach (string mergrrow in mergeCell)
                    {
                        mergetext += mergrrow;

                    }
                    mergetext += "</mergeCells>";
                    xmltext.Append(mergetext);
                }

                xmltext.Append(GetPageMargins());
                string _xmltext = xmltext.ToString();
                xmltext = new StringBuilder();
                xmltext.Append(GetSheetHeadandColumns() + _xmltext.ToString());
                //Nagaraju 27-03-2014
                xmlstring = xmltext.ToString();
                HttpContext.Current.Session["sharedstrings"] = sharedStrings;
                //exportfiles = new Dictionary<string, string>();
                //exportfiles.Add("tab1", xmltext);
                //HttpContext.Current.Session["exportfiles"] = exportfiles;
                ishopParams = new iSHOPParams();
                ishopParams.LeftHeader = leftheader;
                ishopParams.LeftBody = leftbody;
                ishopParams.RightHeader = rightheader;
                ishopParams.RightBody = righttbody;
                ishopParams.Retailer = tbltext.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return ishopParams;
        }

        public override bool CheckTotal_DeliveryMethodUseItem(string item)
        {
            switch (item.ToLower())
            {
                case "convenience":
                case "supermarket/grocery":
                case "drug":
                case "dollar":
                case "club":
                case "mass merc.":
                case "supercenter":
                case "kroger corporate":
                case "ausa corporate":
                case "safeway corporate":
                case "target corporate":
                case "walmart corporate":
                case "walmart inc.":
                    {
                        IsApplicable = false;
                        return true;
                    }
            }
            return false;
        }

        public override bool CheckSampleSize(string samplesizekey)
        {
            IsApplicable = true;
            try
            {
                if (sampleSize.ContainsKey(samplesizekey))
                {
                    string val = sampleSize[samplesizekey];
                    if (string.IsNullOrEmpty(val))
                        return false;
                    double szvalue = Convert.ToDouble(sampleSize[samplesizekey]);
                    if (View_Type.Equals("COMPARE", StringComparison.OrdinalIgnoreCase) && MetricItem.Equals("Awareness", StringComparison.OrdinalIgnoreCase) && TabIndexId == 2)                    
                        CheckTotal_DeliveryMethodUseItem(samplesizekey);
                    else if ((View_Type.Equals("PIT", StringComparison.OrdinalIgnoreCase) && MetricItem.Equals("Awareness", StringComparison.OrdinalIgnoreCase) && TabIndexId == 2)
                        || (View_Type.Equals("TREND", StringComparison.OrdinalIgnoreCase) && MetricItem.Equals("Awareness", StringComparison.OrdinalIgnoreCase) && TabIndexId == 2))                    
                    {
                        CheckTotal_DeliveryMethodUseItem(param.ShopperSegment);
                    }

                    if (!IsApplicable)
                        return false;
                    
                    if (szvalue >= 100)
                    {
                        return true;
                    }
                    else
                    {
                        //cellfontstyle = 10;
                        if (szvalue == GlobalVariables.NANumber)
                        {
                            IsApplicable = false;
                        }
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

        public override string CheckNAValues(string value)
        {
            string NA = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (!IsApplicable)
            {
                return NA = GlobalVariables.NA;
            }
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                return NA = GlobalVariables.NA;
            }
            if (LoyaltyPyramid && !Convert.ToString(GetLoyaltyPyramidName(BenchmarkOrComparison)).Equals(LoyaltyPyramidmetric, StringComparison.OrdinalIgnoreCase))
            {
                return NA = GlobalVariables.NA;
            }
            if (CheckSampleSize(BenchmarkOrComparison) == false && CommonFunctions.CheckMediumSampleSize(BenchmarkOrComparison, sampleSize) == false && CheckRetailerorChannel == false)
            {
                return NA;
            }
            if (string.IsNullOrEmpty(value) && LoyaltyPyramid) //&& LoyaltyPyramid)
            {
                NA = GlobalVariables.NA;

            }
            else if ((StoreImageryCheck || CheckRetailerorChannel || LoyaltyPyramidForRetailers) || (CheckBeverageTripNA && (BenchmarkOrComparison.Trim() == "Total Trips" || CheckString == checkBevTotalTrips)))
            {
                NA = GlobalVariables.NA;
            }
            return NA;
        }

        public override string CheckBlankValues(string rowvalue, string tablename, string metricItem)
        {
            string value = string.Empty;

            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                value = GlobalVariables.NA;
            }
            else if (Convert.ToString(metricItem).Equals("Approximate Average Number of Items",StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }
            else if (Convert.ToString(metricItem).Equals("AVERAGE ONLINE BASKET SIZE", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }
            else if (Convert.ToString(metricItem).Equals("Approximate Average time in store", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minute";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minutes";
                }
            }
            else if (Convert.ToString(metricItem).Equals("Approximate Amount Spent", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }
            else if (Convert.ToString(metricItem).Equals("AVERAGE ONLINE ORDER SIZE", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }

            return value;
        }

        public override string CheckXMLBlankValues(string rowvalue, string tablename, string metricItem)
        {
            string value = string.Empty;
            string valuereturn = string.Empty;
            //added by Nagaraju for Beverage Detail Liquid Flavor Enhancer
            //Date: 21-03-2016
            if (isBeverageDetail && isLiquidFlavorEnhancer)
            {
                value = GlobalVariables.NA;
            }
            else if (Convert.ToString(metricItem).Equals("Approximate Average Number of Items", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }
            else if (Convert.ToString(metricItem).Equals("AVERAGE ONLINE BASKET SIZE", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " item";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " items";
                }
            }
            else if (Convert.ToString(metricItem).Equals("Approximate Average time in store", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else if (Convert.ToDouble(rowvalue) < 1.5)
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minute";
                }
                else
                {
                    value = Convert.ToString(Math.Round(Convert.ToDouble(rowvalue))) + " minutes";
                }
            }

            else if (Convert.ToString(metricItem).Equals("Approximate Amount Spent", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }
            else if (Convert.ToString(metricItem).Equals("AVERAGE ONLINE ORDER SIZE", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    value = "$" + Convert.ToString(Math.Round(Convert.ToDouble(rowvalue)));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(rowvalue))
                {
                    value = "";
                }
                //else if (Convert.ToDouble(rowvalue) == 0.0)
                //{
                //    value = "";
                //}
                else
                {
                    double val = Convert.ToDouble(rowvalue) / 100;
                    value = val.ToString();
                }
            }

            valuereturn = cf.cleanExcelXML(value);
            if (!CheckSharedStringValue(valuereturn))
            {
                AddToSharedString(valuereturn);
            }
            //else
            //{
            //    valuereturn = Convert.ToString(sharedStrings[valuereturn]);
            //}
            return valuereturn;
        }

        public override string WriteFilters()
        {
            string value = "";
            switch (accuratestatvalueposi.ToString())
            {
                case "1.2816":
                    value = "80";
                    break;
                case "1.6449":
                    value = "90";
                    break;

                case "1.96":
                    value = "95";
                    break;

                case "2.5758":
                    value = "99";
                    break;
            }
            string xmlstring = "* SELECTION";
            StringBuilder xmltext = new StringBuilder();
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<row " +
           "r = \"1\" " +
           "spans = \"1:11\" " +
           "ht = \"15\" " +
           "thickBot = \"1\" " +
           "x14ac:dyDescent = \"0.3\">" +
           "<c " +
               "r = \"B1\" " +
               "s = \"9\" " +
               "t = \"s\">" +
               "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
           "</c> ");

            xmlstring = "* FILTERS";
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<c " +
               "r = \"C1\" " +
               "s = \"9\" " +
               "t = \"s\">" +
               "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
           "</c> ");


            xmlstring = "IS-TITLE STAT TEST:";
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<c " +
               "r = \"D1\" " +
               "s = \"10\" " +
               "t = \"s\">" +
               "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
           "</c> ");

            xmltext.Append("</row>");

            //Time Period
            //if (!string.IsNullOrEmpty(TimePeriod))
            //{
            //    if (TimePeriod.IndexOf("3MMT") > -1)
            //    {
            //        xmlstring = "Time Period : " + TimePeriod.Split('|')[1] + " 3MMT";
            //    }
            //    else if (TimePeriod.IndexOf("total") > -1)
            //    {
            //        xmlstring = "Time Period : AUG 2013 TO JUN 2014";
            //    }
            //    else
            //    {

            //        xmlstring = "Time Period : " + TimePeriod.Split('|')[1];
            //    }
            //}
            //else
            //{
            //    xmlstring = "Time Period :";
            //}
            xmlstring = "IS-TITLE TIME PERIOD: " + TimePeriod;
            xmlstring = cf.cleanExcelXML(xmlstring);
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<row " +
   "r = \"2\" " +
   "spans = \"1:11\" " +
   "ht = \"15\" " +
   "thickBot = \"1\" " +
   "x14ac:dyDescent = \"0.3\">" +
   "<c " +
       "r = \"B2\" " +
       "s = \"11\" " +
       "t = \"s\">" +
       "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
   "</c>");

            //if (BenchMark.IndexOf("Category") > -1 || BenchMark.IndexOf("Brand") > -1 || CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1 || CheckString.ToLower().IndexOf("totaltrip") > -1)
            if (table_Params.IsBeverages)
            {
                if (table_Params.IsWherePurchased)
                {
                    string text = string.Empty;
                    string[] cr = frequency.Split(new String[] { "|", "|" },
                                   StringSplitOptions.RemoveEmptyEntries);
                    if (cr != null && cr.Count() > 0)
                    {
                        for (int i = 0; i < cr.Length; i++)
                        {
                            text += Get_ShortNames(cr[i]) + ", ";
                        }
                    }
                    xmlstring = "IS-TITLE WHERE PURCHASED: " + frequency.Replace("|", ", ");
                }
                else
                {
                    //if (frequency == "Total")
                    //{
                    //    xmlstring = "Channel/Retailer : " + frequency;
                    //}
                    //else
                    //{
                    //    xmlstring = "Monthly Purchasing Amount : " + frequency;
                    //}
                    xmlstring = "IS-TITLE MONTHLY PURCHASE: " + frequency;
                }
            }
            else
            {
                if (Convert.ToString(frequency).Equals("Total", StringComparison.OrdinalIgnoreCase))
                {
                    xmlstring = "IS-TITLE CHANNEL/RETAILER: " + frequency;
                }
                else
                {
                    if (frequency.IndexOf("channels") > -1 || frequency.IndexOf("retailers") > -1)
                    {
                        string[] cr = frequency.Split(new String[] { "|", "|" },
                                       StringSplitOptions.RemoveEmptyEntries);
                        string text = string.Empty;
                        for (int i = 1; i < cr.Length; i += 2)
                        {
                            text += Get_ShortNames(cr[i]) + ", ";
                        }
                        xmlstring = "IS-TITLE CHANNEL/RETAILER: " + text;
                    }
                    else
                    {
                        //xmlstring = "Shopping Frequency: " + frequency;
                        xmlstring = "IS-TITLE SHOPPING FREQUENCY: " + Get_ShortNamesFrequency(frequency).Replace("Past 3 Month", "Quarterly +");
                    }
                }

            }
            xmlstring = cf.cleanExcelXML(xmlstring);
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<c " +
           "r = \"C2\" " +
           "s = \"11\" " +
           "t = \"s\">" +
           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
       "</c> ");

            xmlstring = ">" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<c " +
               "r = \"D2\" " +
               "s = \"12\" " +
               "t = \"s\">" +
               "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
           "</c> ");

            xmltext.Append("</row>");

            //Single Selection
            xmlstring = string.Empty;
            if (param.ShopperSegment != "")
            {
                if (CheckString.IndexOf("Channel") > -1 || CheckString.IndexOf("Retailer") > -1 || CheckString.ToLower().IndexOf("totalshopper") > -1)
                {
                    if (AddTradeAreaNoteforChannel(param.ShopperSegment) != string.Empty)
                    {
                        xmlstring = "IS-TITLE CHANNEL/RETAILER: " + param.ShopperSegment + AddTradeAreaNoteforChannel(param.ShopperSegment);
                    }
                    else
                    {
                        xmlstring = "IS-TITLE CHANNEL/RETAILER: " + param.ShopperSegment;
                    }
                }
                else if (CheckString.IndexOf("Category") > -1 || CheckString.IndexOf("Brand") > -1 || CheckString.ToLower().IndexOf("totaltrip") > -1)
                {
                    xmlstring = "IS-TITLE CATEGORY/BRAND: " + param.ShopperSegment;
                }
            }
            else
            {
                xmlstring = "";
            }
            // xmlstring = "Single Selection";
            xmlstring = cf.cleanExcelXML(xmlstring);
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<row " +
     "r = \"3\" " +
     "spans = \"1:11\" " +
     "ht = \"15\" " +
     "thickBot = \"1\" " +
     "x14ac:dyDescent = \"0.3\">" +
     "<c " +
         "r = \"B3\" " +
         "s = \"13\" " +
         "t = \"s\">" +
         "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
     "</c>");


            string CustomFilter = cf.GetExcelSortedFilters(param.CustomFilters);

            //string[] ss = param.CustomFilters.Split(new String[] { "|", "|" },
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
                xmlstring = "IS-TITLE " + CustomFilter;
            else
                xmlstring = " : ";

            xmlstring = cf.cleanExcelXML(xmlstring);
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }
            xmltext.Append("<c " +
           "r = \"C3\" " +
           "s = \"13\" " +
           "t = \"s\">" +
           "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
       "</c> ");

            xmlstring = "<" + value + "%";
            //xmlstring = cf.cleanExcelXML(xmlstring);
            if (!CheckSharedStringValue(xmlstring))
            {
                AddToSharedString(xmlstring);
            }

            xmltext.Append("<c " +
               "r = \"D3\" " +
               "s = \"14\" " +
               "t = \"s\">" +
               "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
           "</c> ");

            xmltext.Append("</row>");
            //Single Selection

            //       xmlstring = "Single Selection";
            //       xmlstring = cf.cleanExcelXML(xmlstring);
            //       if (!CheckSharedStringValue(xmlstring))
            //       {
            //           AddToSharedString(xmlstring);
            //       }

            //       xmltext.Append("<row " +
            //"r = \"3\" " +
            //"spans = \"1:11\" " +
            //"ht = \"15\" " +
            //"thickBot = \"1\" " +
            //"x14ac:dyDescent = \"0.3\">" +
            //"<c " +
            //    "r = \"B4\" " +
            //    "s = \"13\" " +
            //    "t = \"s\">" +
            //    "<v>" + GetSharedStringKey(xmlstring) + "</v>" +
            //"</c></row> ";

            return xmltext.ToString();
        }
        #region bold items
        private bool IsItemBold(string item,string parentName)
        {
            isitembold = false;
            if (parentName!= "Item Categories Purchased" && (TabIndexId == 2 || TabIndexId == 3 || TabIndexId == 4))
            {
                switch (item.Trim().ToUpper())
                {
                    case "FOOD ITEMS":
                    case "BEVERAGE ITEMS":
                    case "BEVERAGE INGREDIENTS":
                    case "HEALTH CARE AND PERSONAL CARE ITEMS":
                    case "OTHER ITEMS THAT ARE REGULARLY REPLENISHED":
                    case "NON-CONSUMABLE ITEMS":
                    case "OTHER GROCERY ITEMS":
                        {
                            isitembold = true;
                            break;
                        }
                }
            }
            else if (parentName != "Item Categories Purchased" && (TabIndexId == 4))
            {
                switch (item.Trim().ToUpper())
                {
                    case "HEALTH CARE AND PERSONAL CARE ITEMS":
                    case "OTHER ITEMS THAT ARE REGULARLY REPLENISHED":
                    case "NON-CONSUMABLE ITEMS":
                        {
                            isitembold = true;
                            break;
                        }
                }
            }
            return isitembold;
        }
        #endregion 
        private bool IsStoreFrequencyTotalUS(string metricName)
        {
            if (!string.IsNullOrEmpty(metricName) && Convert.ToString(metricName).Equals("Store Frequency (BASE-TOTAL US POPULATION)", StringComparison.OrdinalIgnoreCase))
                return true;
            else
                return false;
        }
    }
}