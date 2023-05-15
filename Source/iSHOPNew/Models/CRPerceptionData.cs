using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Aspose.Slides.Charts;
using iSHOPNew.DAL;


namespace iSHOPNew.Models
{
    
    public class CRPerceptionData
    {
        public Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        public List<string> mergeCell = new List<string>();
        CommonTable objCommonTable = new CommonTable();
        int cellfontstyle = 5;
        //Nishanth
        int cellfontstylegrey = 8;
        int samplecellstyle = 2;
        int sampleflag = 0;
        public string StatPositive = string.Empty;
        public string StatNegative = string.Empty;
        bool IsApplicable = true;

        public string BenchMark = string.Empty;
        string BenchmarkOrComparison;
        string SelectedStatTest = string.Empty;
        iSHOPParams paramSub = new iSHOPParams();
        public iSHOPParams GetPerceptionsData(string _BenchMark, string _Compare, string _timePeriod, string _shortTimePeriod, string _ShopperFrequencyShort, string _ShopperFrequency, string isChange, string _width, int _height, string _filter, string Selected_StatTest, string TimePeriod_UniqueId, string Benchmark_UniqueIds, string Comparison_UniqueIds, string ShopperFrequency_UniqueId, string ShopperSegment_UniqueId, string Sigtype_UniqueId,string CustomBase_UniqueId,string CustomBase_ShortName, string shortNameList)
        {
            string exportdata = string.Empty;
            SelectedStatTest = Selected_StatTest;
            List<string> benchmarkparams = _BenchMark.Split('|').ToList();
            if (benchmarkparams != null && benchmarkparams.Count > 0)
            {
                BenchMark = benchmarkparams[benchmarkparams.Count - 1];
            }
            //if (Selected_StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            //{
              
            //}
           
            mergeCell = new List<string>();
            sharedStrings = new Dictionary<string, int>();
            int rownumber = 1;
            int colindex = 2;
            string htmltext = String.Empty;
            string width = _width;
            int height = _height;
            System.Data.DataTable tbl = null;
            List<string> columnnames = new List<string>();
            List<string> shortNames = new List<string>();

            string Benchmark = _BenchMark.Replace("`","'");
            string cmp = _Compare.Replace("`", "'");
            string Frequency = _ShopperFrequency;
            cmp = cmp.Replace("`", "'").Replace("RetailerNet|", "");
            string[] cmplist = cmp.Split(',');
            //HttpContext.Current.Session["StatSessionPosi"] = "1.96";
            //HttpContext.Current.Session["StatSessionNega"] = "-1.96";
            //List<bool> samplesizelist = new List<bool>();
            //atul new
            List<int> samplesizelist = new List<int>();
            //bool Samplevalue;
            int Samplevalue;
            List<string> parameters = new List<string>();
            string sStatTest = "";
            sStatTest = Selected_StatTest;
            if (Selected_StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
            {
                Benchmark_UniqueIds = CustomBase_UniqueId;
                List<string> sComp = Comparison_UniqueIds.Split('|').ToList();
                List<string> comp_list = (from r in sComp where r != Benchmark_UniqueIds.ToString() select r).ToList();
                Comparison_UniqueIds = string.Join("|", comp_list);
                BenchMark = CustomBase_ShortName;
                SelectedStatTest = "Benchmark";
                Selected_StatTest = "Benchmark";
            }
            else
            {
                Benchmark_UniqueIds = Comparison_UniqueIds.Split('|')[0];
                List<string> sComp = Comparison_UniqueIds.Split('|').ToList();
                sComp.RemoveAt(0);
                Comparison_UniqueIds = string.Join("|", sComp);
            }

            try
            {
                if (HttpContext.Current.Session["StatSessionPosi"] == null || HttpContext.Current.Session["StatSessionNega"] == null)
                {
                    return paramSub;
                }
                StatPositive = Convert.ToString(HttpContext.Current.Session["StatSessionPosi"]);
                StatNegative = Convert.ToString(HttpContext.Current.Session["StatSessionNega"]);
                DataAccess dal = new DataAccess();
                //object[] paramvalues = new object[] { _BenchMark.Replace("~", "`"), _Compare.Replace(",", "|").Replace("~", "`"), _timePeriod, _ShopperFrequency, _filter, Selected_StatTest };
                object[] paramvalues = new object[] {  Benchmark_UniqueIds, Comparison_UniqueIds, TimePeriod_UniqueId, ShopperSegment_UniqueId, ShopperFrequency_UniqueId, Sigtype_UniqueId };
                //DataSet ds = dal.GetData(paramvalues, "sp_FactBookStoreAssoCrossTabReport"); 
                DataSet ds = dal.GetData_WithIdMapping(paramvalues, "usp_CrossRetailerPerceptions");

                //List<System.Data.DataTable> result = ds.Tables[0].AsEnumerable()
                // .GroupBy(row => row.Field<string>("Metric"))
                // .Select(g => g.CopyToDataTable())
                // .ToList();

                //DataSet data = new DataSet();
                //foreach (System.Data.DataTable dt in result)
                //{
                //    data.Tables.Add(dt);
                //}
                ////add ShopperFrequency
                //ds = new DataSet();
                //ds = data;
                shortNames = new List<string>();
                foreach (object col in ds.Tables[0].Columns)
                {
                    string coln = Convert.ToString(col);
                    if(coln!= "Metric" && coln != "MetricItem")
                    shortNames.Add(coln.ToLower().ToString());
                }
                shortNames = new List<string>();
                shortNames = shortNameList.ToLower().Split('|').ToList();
                List<string> sComparisonString = (_BenchMark + "," + _Compare).ToString().Split(',').ToList();
                paramSub = objCommonTable.BindTabs(_BenchMark, sComparisonString.ToArray(), _timePeriod, _filter, _filter, _ShopperFrequency, shortNames.ToArray(), StatPositive, StatNegative, _shortTimePeriod, "381", "381", sStatTest, CustomBase_ShortName, ds,"2");

                rownumber = 4;
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<string> metrics = (from row in ds.Tables[0].AsEnumerable()
                                            select Convert.ToString(row["Metric"])).Distinct().ToList();
                    DataSet dstemp = new DataSet();
                    foreach (string metric in metrics)
                    {
                        var datarows = (from row in ds.Tables[0].AsEnumerable()
                                        where Convert.ToString(row["Metric"]).Equals(metric, StringComparison.OrdinalIgnoreCase)
                                        select row).Distinct().ToList();
                        dstemp.Tables.Add(datarows.CopyToDataTable());
                    }
                    ds = new DataSet();
                    ds = dstemp;

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        tbl = ds.Tables[i].Copy();
                        //samplesizelist = new List<bool>();
                        samplesizelist = new List<int>();
                        if (i == 0)
                        {
                            if (tbl != null && tbl.Rows.Count > 0)
                            {
                                int rows = tbl.Columns.Count - 2;
                                //write table header
                                htmltext = "<div id=\"PercepHeader\" style=\"overflow:hidden;width:" + width + ";\">";
                                htmltext += "<table style=\"width:100%;\">";

                                //htmltext += "<tr><td class=\"SFBenchComp\">Shopping Frequency<a class=\"table - top - title - bottom - line\" style=\"background-color: #000000;border-radius: 1px 10px 0 0;bottom: 528px;display: block;height: 4px;left: 57px;width: 25px;position: absolute;\"></a></td>";
                                //htmltext += "<td class=\"Benchmark\">BenchMark</td>";
                                //htmltext += "<td colspan=\"" + (rows - 2) + "\" class=\"CompHeader\"> Comparison Areas</td></tr>";
                                htmltext += "<tr><td id=\"SFLabel\" style=\"width:;background-color: #d5d6d6; border-bottom: 1px solid #686868;text-align:center;color:black;\">" + _ShopperFrequencyShort + "<a class=\"table - top - title - bottom - line\" style=\"background-color: #000000;border-radius: 1px 10px 0 0;bottom: 505px;display: block;height: 4px;left: 57px;width: 25px;position: absolute;\"></a><div class=\"treeview minusIcon\"></div></td>";
                                htmltext += "<td class=\"BenchComp\" style=\"width:;height:22px;\">" + Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'") + "</td>";
                                //Write table header in Excel                          
                                exportdata = GetSheetHeadandColumns();
                                exportdata += "<sheetData>";

                                //write benchmark filter
                                exportdata += CreateAndCloseRow("create", "1");
                                string benchmarkfilter = "BENCHMARK: " + Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToUpper() + "(" + GetExcelFrequency(Frequency) + ")";
                                CreateSharedStringDate(benchmarkfilter);
                                exportdata += CreateSheetData("B1", "10", GetSharedStringValue(benchmarkfilter), "true");
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
                                exportdata += CreateSheetData("C1", "10", GetSharedStringValue(xmlstring), "true");
                                exportdata += CreateAndCloseRow("close", "1");
                                //add filters                   
                                exportdata += CreateAndCloseRow("create", "2");
                                xmlstring = "* Filters";
                                CreateSharedStringDate(xmlstring);
                                exportdata += CreateSheetData("B2", "10", GetSharedStringValue(xmlstring), "true");

                                string CustomFilter = cf.GetExcelSortedFilters(_filter); 

                                //string[] ss = _filter.Split(new String[] { "|", "|" },
                                //                        StringSplitOptions.RemoveEmptyEntries);

                                //for (int j = 0; j < ss.Length; j += 2)
                                //{
                                //    ss[j] = ss[j] + ": ";
                                //}

                                //for (int j = 1; j < ss.Length; j += 2)
                                //{
                                //    ss[j] = ss[j] + ", ";
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
                                exportdata += CreateSheetData("C2", "10", GetSharedStringValue(xmlstring), "true");
                                exportdata += CreateAndCloseRow("close", "2");
                                //
                                //add sample size note
                                xmlstring = cf.cleanExcelXML("NOTE : GREY FONT = LOW SAMPLE (30-99), BLANK = SAMPLE < 30; NA = NOT APPLICABLE");
                                CreateSharedStringDate(xmlstring);
                                exportdata += CreateAndCloseRow("create", "3");
                                exportdata += CreateSheetData("C3", "10", GetSharedStringValue(xmlstring), "true");
                                exportdata += CreateAndCloseRow("close", "3");
                                //

                                exportdata += CreateAndCloseRow("create", rownumber.ToString());

                                string filter = "AMONG " + GetExcelFrequency(Frequency) + " SHOPPERS OF " + Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'") + ":";
                                CreateSharedStringDate(filter);
                                exportdata += CreateSheetData("A4", "1", GetSharedStringValue(filter), "true");
                                //write excel bench mark header.
                                CreateSharedStringDate(Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'"));
                                exportdata += CreateSheetData("B4", "1", GetSharedStringValue(Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")), "true");

                                colindex = 2;
                                foreach (string item in cmplist)
                                {
                                    htmltext += "<td class=\"BenchComp\">" + item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'") + "</td>";

                                    CreateSharedStringDate(item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'"));
                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), "1", GetSharedStringValue(item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")), "true");
                                    colindex += 1;
                                }
                                htmltext += "</tr></table></div>";
                                exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                rownumber += 1;
                                columnnames = new List<string>();
                                foreach (object col in tbl.Columns)
                                {
                                    string coln = Convert.ToString(col);
                                    columnnames.Add(coln.ToLower().ToString());
                                }

                                //write Store Associations

                                htmltext += "<div class=\"DataPerception\" style=\"overflow:auto;width:" + width + ";height:" + height + "px;display: flex;\" onscroll=\"reposHorizontal(this);\" color: black;\" > ";
                                htmltext += "<table style=\"width:100%\">";

                                htmltext += "<td colspan=\"" + rows + "\" class=\"TableName\">" + Convert.ToString(tbl.Rows[1]["Metric"]) + "</td></tr>";

                                //write Excel Store Associations
                                exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                CreateSharedStringDate(Convert.ToString(tbl.Rows[1]["Metric"]));
                                exportdata += CreateSheetData("A" + rownumber.ToString(), "8", GetSharedStringValue(Convert.ToString(tbl.Rows[1]["Metric"])), "true");

                                //Nishanth
                                colindex = 1;
                                foreach (string item in cmplist)
                                {
                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), "8", "", "");
                                    colindex += 1;
                                }
                                exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), "8", "", "");

                                exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                mergeCell.Add("<mergeCell ref = \"A" + rownumber + ":" + ColumnIndexToName(colindex - 1) + rownumber + "\"/>");
                                rownumber += 1;

                                for (int irow = 0; irow < tbl.Rows.Count; irow++)
                                {
                                    samplecellstyle = 2;
                                    if (Convert.ToString(tbl.Rows[irow]["MetricItem"]) == "SampleSize")
                                    {
                                            htmltext += "<tr>";
                                        htmltext += "<td class=\"CRPerceptionMetricBlock\">Sample Size</td>";

                                        exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                        CreateSharedStringDate("Sample Size");
                                        exportdata += CreateSheetData("A" + rownumber.ToString(), "3", GetSharedStringValue("Sample Size"), "true");


                                        if (!string.IsNullOrEmpty(Benchmark) && columnnames.Contains(Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                        {
                                            htmltext += "<td class=\"SampleBenchComp\">" + CommaSeparatedValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "" + CheckLowSampleSize(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out Samplevalue) + "</td>";
                                            samplesizelist.Add(Samplevalue);
                                            //write benchmark excel sample size
                                            if (sampleflag == 1)
                                            {
                                                string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                CreateSharedStringDate(lowsamplesize);
                                                exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                            }
                                            //atul new
                                            else if (sampleflag == 2)
                                            {
                                                string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                CreateSharedStringDate(lowsamplesize);
                                                exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                            }
                                            else
                                            {
                                                exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable), "false");
                                            }
                                        }

                                        colindex = 2;
                                        foreach (string item in cmplist)
                                        {
                                            if (!string.IsNullOrEmpty(item) && columnnames.Contains(item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                            {
                                                htmltext += "<td class=\"SampleBenchComp\">" + CommaSeparatedValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "" + CheckLowSampleSize(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out Samplevalue) + "</td>";
                                                samplesizelist.Add(Samplevalue);
                                                //write comparison excel sample size
                                                if (sampleflag == 1)
                                                {
                                                    string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                    CreateSharedStringDate(lowsamplesize);
                                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                                }
                                                //atul new
                                                else if (sampleflag == 2)
                                                {
                                                    string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                    CreateSharedStringDate(lowsamplesize);
                                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                                }
                                                else
                                                {
                                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable), "false");
                                                }

                                                colindex += 1;
                                            }
                                        }
                                        htmltext += "</tr>";
                                        exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                        rownumber += 1;
                                    }

                                    else
                                    {
                                        cellfontstyle = 5;
                                        string Significance = Convert.ToString(tbl.Rows[irow]["MetricItem"]);
                                        if (!Significance.Trim().ToLower().Contains("significance"))
                                        {
                                            htmltext += "<tr>";
                                            htmltext += "<td class=\"MetricItem\" style=\"width:;\">" + Convert.ToString(tbl.Rows[irow]["MetricItem"]) + "</td>";


                                            string metricname = Convert.ToString(tbl.Rows[irow]["MetricItem"]);
                                            metricname = cf.cleanExcelXML(metricname);
                                            CreateSharedStringDate(metricname);
                                            exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                            exportdata += CreateSheetData("A" + rownumber + "", "4", GetSharedStringValue(metricname), "true");

                                            int samplesizeinde = 0;
                                            if (!string.IsNullOrEmpty(Benchmark) && columnnames.Contains(Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                            {
                                                if (benchmarkparams != null && benchmarkparams.Count > 0)
                                                {
                                                    BenchmarkOrComparison = (benchmarkparams[benchmarkparams.Count - 1]).Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "");
                                                }
                                                if (benchmarkparams != null && benchmarkparams.Count > 0)
                                                {
                                                    BenchmarkOrComparison = (benchmarkparams[benchmarkparams.Count - 1]).Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "");
                                                }
                                                if (samplesizelist[samplesizeinde] == 1)
                                                {
                                                    htmltext += "<td class=\"DataStyle\" style=\"width:;" + GetCellColor(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                    exportdata += CreateSheetData("B" + rownumber + "", cellfontstyle.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                }
                                                //atul new
                                                else if (samplesizelist[samplesizeinde] == 2)
                                                {
                                                    htmltext += "<td class=\"GreyDataStyle\" style=\"width:;" + GetCellColorGrey(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                    exportdata += CreateSheetData("B" + rownumber + "", cellfontstylegrey.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                }
                                                else
                                                {
                                                    htmltext += "<td class=\"DataStyle\" style=\"width:;\"></td>";
                                                    if (!IsApplicable)
                                                    {
                                                        CreateSharedStringDate(GlobalVariables.NA);
                                                        exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), GetSharedStringValue(GlobalVariables.NA), "true");
                                                    }
                                                    else
                                                    {
                                                        exportdata += CreateSheetData("B" + rownumber + "", cellfontstyle.ToString(), "", "false");
                                                    }
                                                }
                                            }
                                            samplesizeinde += 1;
                                            colindex = 2;
                                            foreach (string item in cmplist)
                                            {
                                                BenchmarkOrComparison = item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "");
                                                if (!string.IsNullOrEmpty(item) && columnnames.Contains(item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                                {
                                                    if (samplesizelist[samplesizeinde] == 1)
                                                    {
                                                        htmltext += "<td class=\"DataStyle\" style=\"" + GetCellColor(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                        exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), cellfontstyle.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                    }
                                                    //atul new
                                                    else if (samplesizelist[samplesizeinde] == 2)
                                                    {
                                                        htmltext += "<td class=\"GreyDataStyle\" style=\"" + GetCellColorGrey(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                        exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), cellfontstylegrey.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                    }
                                                    else
                                                    {
                                                        htmltext += "<td class=\"DataStyle\"></td>";
                                                        if (!IsApplicable)
                                                        {
                                                            CreateSharedStringDate(GlobalVariables.NA);
                                                            exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), GetSharedStringValue(GlobalVariables.NA), "true");
                                                        }
                                                        else
                                                        {
                                                            exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), cellfontstyle.ToString(), "", "false");
                                                        }                                                       
                                                       
                                                    }
                                                    samplesizeinde += 1;
                                                    colindex += 1;
                                                }
                                            }
                                            htmltext += "</tr>";
                                            exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                            rownumber += 1;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            //Good Place to Shop
                            if (tbl != null && tbl.Rows.Count > 0)
                            {
                                columnnames = new List<string>();
                                foreach (object col in tbl.Columns)
                                {
                                    string coln = Convert.ToString(col);
                                    columnnames.Add(coln.ToLower().ToString());
                                }

                                //Good Place to Shop
                                int rows = tbl.Columns.Count - 2;
                                htmltext += "<tr style=\"background-color: black; color: white;\">";
                                CommonFunctions objcomFunc = new CommonFunctions();

                                htmltext += "<td colspan=\"" + rows + "\" class=\"TableName\">" + Convert.ToString(tbl.Rows[1]["Metric"]) + "</td></tr>";

                                //write Excel Store Associations
                                exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                CreateSharedStringDate(Convert.ToString(tbl.Rows[1]["Metric"]));//"Good Place to Shop For..");
                                exportdata += CreateSheetData("A" + rownumber.ToString(), "8", GetSharedStringValue(Convert.ToString(tbl.Rows[1]["Metric"])), "true");//"Good Place to Shop For.."

                                //Nishanth
                                colindex = 1;
                                foreach (string item in cmplist)
                                {
                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), "8", "", "");
                                    colindex += 1;
                                }
                                exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), "8", "", "");

                                exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                mergeCell.Add("<mergeCell ref = \"A" + rownumber + ":" + ColumnIndexToName(colindex - 1) + rownumber + "\"/>");
                                rownumber += 1;

                                for (int irow = 0; irow < tbl.Rows.Count; irow++)
                                {
                                    samplecellstyle = 2;
                                    if (Convert.ToString(tbl.Rows[irow]["MetricItem"]) == "SampleSize")
                                    {
                                        htmltext += "<tr>";
                                        htmltext += "<td class=\"CRPerceptionMetricBlock\">Sample Size</td>";

                                        exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                        CreateSharedStringDate("Sample Size");
                                        exportdata += CreateSheetData("A" + rownumber.ToString(), "3", GetSharedStringValue("Sample Size"), "true");

                                        if (!string.IsNullOrEmpty(Benchmark) && columnnames.Contains(Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                        {

                                            htmltext += "<td class=\"SampleBenchComp\">" + CommaSeparatedValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "" + CheckLowSampleSize(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out Samplevalue) + "</td>";
                                            samplesizelist.Add(Samplevalue);
                                            //write benchmark excel sample size
                                            if (sampleflag == 1)
                                            {
                                                string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                CreateSharedStringDate(lowsamplesize);
                                                exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                            }
                                            //atul new
                                            else if (sampleflag == 2)
                                            {
                                                string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                CreateSharedStringDate(lowsamplesize);
                                                exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                            }
                                            else
                                            {
                                                exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable), "false");
                                            }
                                        }
                                        colindex = 2;
                                        foreach (string item in cmplist)
                                        {
                                            if (!string.IsNullOrEmpty(item) && columnnames.Contains(item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                            {
                                                htmltext += "<td class=\"SampleBenchComp\">" + CommaSeparatedValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "" + CheckLowSampleSize(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out Samplevalue) + "</td>";
                                                samplesizelist.Add(Samplevalue);
                                                //write comparison excel sample size
                                                if (sampleflag == 1)
                                                {
                                                    string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                    CreateSharedStringDate(lowsamplesize);
                                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                                }
                                                //atul new
                                                else if (sampleflag == 2)
                                                {
                                                    string lowsamplesize = CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable) + " " + CommonFunctions.CheckXMLLowSampleSize(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]));
                                                    CreateSharedStringDate(lowsamplesize);
                                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), GetSharedStringValue(lowsamplesize), "true");
                                                }
                                                else
                                                {
                                                    exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), CommonFunctions.CheckXMLCommaSeparatedValue(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")]), out IsApplicable), "false");
                                                }
                                                colindex += 1;
                                            }
                                        }
                                        htmltext += "</tr>";
                                        exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                        rownumber += 1;
                                    }

                                    else
                                    {
                                        cellfontstyle = 5;
                                        string Significance = Convert.ToString(tbl.Rows[irow]["MetricItem"]);
                                        if (!Significance.Trim().ToLower().Contains("significance"))
                                        {
                                            htmltext += "<tr>";
                                            htmltext += "<td class=\"MetricItem\">" + Convert.ToString(tbl.Rows[irow]["MetricItem"]) + "</td>";


                                            string metricname = Convert.ToString(tbl.Rows[irow]["MetricItem"]);
                                            metricname = cf.cleanExcelXML(metricname);
                                            CreateSharedStringDate(metricname);
                                            exportdata += CreateAndCloseRow("create", rownumber.ToString());
                                            exportdata += CreateSheetData("A" + rownumber + "", "4", GetSharedStringValue(metricname), "true");

                                            int samplesizeinde = 0;
                                            if (!string.IsNullOrEmpty(Benchmark) && columnnames.Contains(Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                            {
                                                if (benchmarkparams != null && benchmarkparams.Count > 0)
                                                {
                                                    BenchmarkOrComparison = (benchmarkparams[benchmarkparams.Count - 1]).Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "");
                                                }
                                                if (samplesizelist[samplesizeinde] == 1)
                                                {
                                                    htmltext += "<td class=\"DataStyle\" style=\"" + GetCellColor(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                    exportdata += CreateSheetData("B" + rownumber + "", cellfontstyle.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                }
                                                //atul new
                                                else if (samplesizelist[samplesizeinde] == 2)
                                                {
                                                    htmltext += "<td class=\"GreyDataStyle\" style=\"" + GetCellColorGrey(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                    exportdata += CreateSheetData("B" + rownumber + "", cellfontstylegrey.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][Benchmark.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                }
                                                else
                                                {
                                                    htmltext += "<td class=\"DataStyle\" style=\"width:;\"></td>";                                                  
                                                    if (!IsApplicable)
                                                    {
                                                        CreateSharedStringDate(GlobalVariables.NA);
                                                        exportdata += CreateSheetData("B" + rownumber + "", samplecellstyle.ToString(), GetSharedStringValue(GlobalVariables.NA), "true");
                                                    }
                                                    else
                                                    {
                                                        exportdata += CreateSheetData("B" + rownumber + "", cellfontstyle.ToString(), "", "false");
                                                    }
                                                }

                                            }
                                            samplesizeinde += 1;
                                            colindex = 2;
                                            foreach (string item in cmplist)
                                            {
                                                BenchmarkOrComparison = item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "");
                                                if (!string.IsNullOrEmpty(item) && columnnames.Contains(item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'").ToLower()))
                                                {
                                                    if (samplesizelist[samplesizeinde] == 1)
                                                    {
                                                        htmltext += "<td class=\"DataStyle\" style=\"" + GetCellColor(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                        exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), cellfontstyle.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                    }
                                                    //atul new
                                                    else if (samplesizelist[samplesizeinde] == 2)
                                                    {
                                                        htmltext += "<td class=\"GreyDataStyle\" style=\"" + GetCellColorGrey(Convert.ToString(tbl.Rows[irow]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)]["MetricItem"]), Convert.ToString(tbl.Rows[GetRowNumber(tbl, irow)][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "")])) + "\">" + DecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])) + "</td>";
                                                        exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), cellfontstylegrey.ToString(), MathExportDecimalValues(Convert.ToString(tbl.Rows[irow][item.Replace("Channels|", "").Replace("Retailers|", "").Replace("RetailerNet|", "").Replace("`", "'")])), "false");
                                                    }
                                                    else
                                                    {
                                                        htmltext += "<td class=\"DataStyle\"></td>";
                                                        if (!IsApplicable)
                                                        {
                                                            CreateSharedStringDate(GlobalVariables.NA);
                                                            exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), samplecellstyle.ToString(), GetSharedStringValue(GlobalVariables.NA), "true");
                                                        }
                                                        else
                                                        {
                                                            exportdata += CreateSheetData(ColumnIndexToName(colindex) + rownumber.ToString(), cellfontstyle.ToString(), "", "false");
                                                        }  
                                                    }
                                                    samplesizeinde += 1;
                                                    colindex += 1;
                                                }
                                            }
                                            htmltext += "</tr>";
                                            exportdata += CreateAndCloseRow("close", rownumber.ToString());
                                            rownumber += 1;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    htmltext += "</table></div>";
                }

                if (!string.IsNullOrEmpty(exportdata))
                {
                    exportdata += "</sheetData>";
                    if (mergeCell != null && mergeCell.Count > 0)
                    {
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
            return paramSub;
        }

        public string CommaSeparatedValues(string value)
        {
            string decimaval = string.Empty;
            if (string.IsNullOrEmpty(value))
                decimaval = "";
            else if (value == "0")
            {
                decimaval = "0";
            }
            else
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
            //else if (Convert.ToDouble(rowvalue) == 0.0)
            //{
            //    value = "";
            //}
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
            //else if (Convert.ToDouble(rowvalue) == 0.0)
            //{
            //    value = "";
            //}
            else
            {
                double val = Convert.ToDouble(rowvalue) / 100;
                value = val.ToString();
            }
            return value;
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
            "<col " +
                "min = \"5\" " +
                "max = \"5\" " +
                "width = \"35.140625\" " +
                "customWidth = \"1\"/> " +
            "<col " +
                "min = \"6\" " +
                "max = \"6\" " +
                "width = \"35.140625\" " +
                "customWidth = \"1\"/> " +
            "<col " +
                "min = \"7\" " +
                "max = \"7\" " +
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

        public string CreateSheetData(string rownumber, string style, string value, string title)
        {
            string exportdata = string.Empty;
            if (!string.IsNullOrEmpty(rownumber) && title == "true")
            {
                exportdata = "<c " +
                       "r = \"" + rownumber + "\" " +
                        "s = \"" + style + "\" " +
                        "t = \"s\"> " +
                        "<v>" + value + "</v> " +
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

        public string ColumnIndexToName(int columnIndex)
        {
            char second = (char)(((int)'A') + columnIndex % 26);

            columnIndex /= 26;

            if (columnIndex == 0)
                return second.ToString();
            else
                return ((char)(((int)'A') - 1 + columnIndex)).ToString() + second.ToString();
        }

        public string GetExcelFrequency(string frequency)
        {
            string frequencyvalue = frequency;
            switch (frequency)
            {
                case "Weekly +":
                    {
                        frequencyvalue = "Weekly+";
                        break;
                    }
                case "Monthly +":
                    {
                        frequencyvalue = "Monthly+";
                        break;
                    }
                case "Past 3 Month":
                    {
                        frequencyvalue = "Quarterly+";
                        break;
                    }
            }
            return frequencyvalue;
        }

        public string CheckLowSampleSize(string samplesize, out int Samplevalue)
        {
            string sz = string.Empty;
            Samplevalue = 0;
            if (!string.IsNullOrEmpty(samplesize))
            {
                if (Convert.ToDouble(samplesize) < GlobalVariables.MinSampleSize)
                {
                    sz = "(<span style=\"font-size:14px;\">Low Sample Size</span>)";
                    samplecellstyle = 2;
                    Samplevalue = 3;
                    sampleflag = 1;
                }
                //atul new
                else if (Convert.ToDouble(samplesize) >= GlobalVariables.MinSampleSize && Convert.ToDouble(samplesize) < GlobalVariables.MaxSampleSize)
                {
                    sz = "(<span style=\"font-size:14px;\">Use Directionally</span>)";
                    samplecellstyle = 2;
                    Samplevalue = 2;
                    sampleflag = 2;
                }
                else
                {
                    samplecellstyle = 2;
                    Samplevalue = 1;
                    sampleflag = 3;
                }
            }

            return sz;
        }

        public string GetCellColor(string currentrow, string significancerow, string significancevalue)
        {
            string color = string.Empty;
            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstyle = 11;
                    }
                    else if (Convert.ToDouble(significancevalue) <= Convert.ToDouble(StatPositive) && Convert.ToDouble(significancevalue) >= Convert.ToDouble(StatNegative))
                    {
                        color = "color:black";
                        cellfontstyle = 5;
                    }
                    else if (Convert.ToDouble(significancevalue) < Convert.ToDouble(StatNegative))
                    {
                        color = "color:red";
                        cellfontstyle = 7;
                    }
                    else if (Convert.ToDouble(significancevalue) > Convert.ToDouble(StatPositive))
                    {
                        color = "color:#20B250";
                        cellfontstyle = 6;
                    }

                }
            }
            else
            {
                if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstyle = 11;
                }
                else
                {
                    color = "color:black";
                    cellfontstylegrey = 8;
                }
            }


            return color;
        }

        //Nishanth
        public string GetCellColorGrey(string currentrow, string significancerow, string significancevalue)
        {
            string color = string.Empty;
            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                    {
                        color = "color:blue";
                        cellfontstylegrey = 11;
                    }
                    else if (Convert.ToDouble(significancevalue) <= Convert.ToDouble(StatPositive) && Convert.ToDouble(significancevalue) >= Convert.ToDouble(StatNegative))
                    {
                        color = "color:black";
                        cellfontstylegrey = 8;
                    }
                    else if (Convert.ToDouble(significancevalue) < Convert.ToDouble(StatNegative))
                    {
                        color = "color:red";
                        cellfontstylegrey = 7;
                    }
                    else if (Convert.ToDouble(significancevalue) > Convert.ToDouble(StatPositive))
                    {
                        color = "color:#20B250";
                        cellfontstylegrey = 6;
                    }

                }
            }
            else
            {
                if (BenchmarkOrComparison.Equals(BenchMark, StringComparison.OrdinalIgnoreCase) && SelectedStatTest.Equals("BENCHMARK", StringComparison.OrdinalIgnoreCase))
                {
                    color = "color:blue";
                    cellfontstylegrey = 11;
                }
                else
                {
                    color = "color:black";
                    cellfontstylegrey = 8;
                }
            }
            return color;
        }

        public int GetRowNumber(System.Data.DataTable tbl, int currentrow)
        {
            int rownum = 0;
            if (tbl.Rows.Count > currentrow + 1)
            {
                rownum = currentrow + 1;
            }
            else
            {
                rownum = currentrow;
            }

            return rownum;

        }       
    }
}