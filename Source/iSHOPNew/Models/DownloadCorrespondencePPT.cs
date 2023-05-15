using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using iSHOPNew.DAL;
using iSHOP.BLL;

namespace iSHOPNew.Models
{
    public partial class DownloadCorrespondencePPT 
    {
        string UserExportFileName = string.Empty;
        AdvancedAnalyticsParams advancedAnalyticsParams = null;
        Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        List<object> comparisonpoints = new List<object>();
        DataSet contingencyDS = new DataSet();
        double accuratestatvalueposi = 0;
        double accuratestatvaluenega = 0;
        double StatNegative = 0;
        int table_width = 8790000;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["correspondenceData"] != null)
            {
                advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
                accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
                accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
                StatNegative = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);
                PopulateShortNames();
                ExportToPPT(Convert.ToString(HttpContext.Current.Request.QueryString["year"]), Convert.ToString(HttpContext.Current.Request.QueryString["month"]), Convert.ToString(HttpContext.Current.Request.QueryString["date"]), Convert.ToString(HttpContext.Current.Request.QueryString["hours"]), Convert.ToString(HttpContext.Current.Request.QueryString["minutes"]), Convert.ToString(HttpContext.Current.Request.QueryString["seconds"]));
            }
        }
        public void ExportToPPT(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            try
            {
                advancedAnalyticsParams = HttpContext.Current.Session["AdvancedAnalyticsInputSelection"] as AdvancedAnalyticsParams;
                accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
                accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
                StatNegative = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);
                advancedAnalyticsParams.ChartDataSet = FilterData(advancedAnalyticsParams.ChartDataSet);
                contingencyDS = CreateTables(advancedAnalyticsParams.ChartDataSet);
                CopyFilesToDestination();
                PlotMetrics_X_Y_Data();
                Set_Axis_Min_Max_Values();
                Update_Selection();
                Plot_Excel_Data();
                UpdateContingencyTableData();
                string tempDir = HttpContext.Current.Server.MapPath(UserExportFileName + "");
                string fileName = HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/PPTTemplate.pptx");
                ZipDirectory(tempDir, fileName);

                if (Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
                {
                    Directory.Delete(HttpContext.Current.Server.MapPath(UserExportFileName), true);
                }

                FileStream fs1 = null;
                fs1 = System.IO.File.Open(fileName, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs1.Length];
                fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                fs1.Close();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=iShop_Explorer_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds)) + ".pptx");
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Length", new FileInfo(fileName).Length.ToString());
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
                HttpContext.Current.Response.BinaryWrite(btFile);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex.Message, ex.StackTrace);            
            }
        }

        private DataSet CreateTables(DataSet ds)
        {
            DataSet das = new DataSet();
            DataTable tbl = new DataTable();
            List<string> Benchmark_Comparisonlist = null;
            if (ds != null && ds.Tables.Count >= 3)
            {
                #region Create Columns
                tbl.Columns.Add("Metric", typeof(string));
                tbl.Columns.Add("MetricItem", typeof(string));
                #region Add Benchmark and Comparison Columns
                var query = from r in ds.Tables[2].AsEnumerable()
                            select r.Field<string>("Objective");
                Benchmark_Comparisonlist = query.Distinct().ToList();
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    tbl.Columns.Add(bc, typeof(string));
                    comparisonpoints.Add(bc);
                }
                #endregion

                #endregion

                #region Create Rows
                #region Add Sample Size
                List<object> Metricrowitems = new List<object>();
                List<object> Significancerowitems = new List<object>();
                Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                Metricrowitems.Add("SampleSize");
                foreach (string bc in Benchmark_Comparisonlist)
                {
                    var query2 = from r in ds.Tables[0].AsEnumerable()
                                 where r.Field<string>("Objective") == bc
                                 select r;
                    List<DataRow> Rows = query2.ToList();
                    if (Rows != null && Rows.Count > 0)
                    {
                        foreach (DataRow row in Rows)
                        {
                            Metricrowitems.Add(row["SampleSize"]);
                        }
                    }
                    else
                    {
                        Metricrowitems.Add(string.Empty);
                    }
                }
                tbl.Rows.Add(Metricrowitems.ToArray());
                #endregion
                #region No Of Responses
                if (ds != null && ds.Tables.Count >= 4)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Metricrowitems.Add("Number of Responses");
                    foreach (string bc in Benchmark_Comparisonlist)
                    {
                        var query2 = from r in ds.Tables[3].AsEnumerable()
                                     where r.Field<string>("Metric") == bc
                                     select r;
                        List<DataRow> Rows = query2.ToList();
                        if (Rows != null && Rows.Count > 0)
                        {
                            foreach (DataRow row in Rows)
                            {
                                Metricrowitems.Add(row["SampleSize"]);
                            }
                        }
                        else
                        {
                            Metricrowitems.Add(string.Empty);
                        }
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                }
                #endregion
                #region Add Metric Items
                var query4 = from r in ds.Tables[2].AsEnumerable()
                             select r.Field<string>("MetricItem");
                List<string> MetricItems = query4.Distinct().ToList();
                foreach (string metric in MetricItems)
                {
                    Metricrowitems = new List<object>();
                    Metricrowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Metricrowitems.Add(metric);

                    Significancerowitems = new List<object>();
                    Significancerowitems.Add(ds.Tables[2].Rows[0]["Metric"]);
                    Significancerowitems.Add(metric + "Significance");
                    var query3 = from r in ds.Tables[2].AsEnumerable()
                                 where r.Field<string>("MetricItem") == metric
                                 select r;
                    List<DataRow> Rows = query3.ToList();
                    foreach (DataRow row in Rows)
                    {
                        Metricrowitems.Add(row["Volume"]);
                        Significancerowitems.Add(row["Significance"]);
                    }
                    tbl.Rows.Add(Metricrowitems.ToArray());
                    tbl.Rows.Add(Significancerowitems.ToArray());
                }

                #endregion
                #endregion
                das.Tables.Add(tbl);
            }
            return das;
        }
        private List<object> GetColumns(int from, int to)
        {
            List<object> _columns = new List<object>();
            for (int i = from; i < to; i++)
            {
                _columns.Add(comparisonpoints[i]);
            }
            return _columns;
        }
        private DataSet FilterData(DataSet dtbl)
        {
            DataSet ds = new DataSet();
            List<string> metrics = new List<string>();
            if (dtbl != null && dtbl.Tables.Count > 0)
            {
                ds = dtbl.Copy();
                foreach (DataTable tb in ds.Tables)
                {
                    if (tb.Columns.Contains("Objective"))
                    {
                        if (tb != null && tb.Rows.Count > 0)
                        {
                            foreach (DataRow row in tb.Rows)
                            {
                                row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"]));
                            }
                        }
                    }
                }
            }
            return ds;
        }
        private void UpdateContingencyTableData()
        {
            string xmlpath = string.Empty;
            Dictionary<string, string> keyvalues = null;
            List<object> columnlist = null;
            List<string> columnwidth = null;
            int rowheight = 105515;
            List<string> samplesizelist = null;
            string sampleSize = string.Empty;
            if (comparisonpoints.Count <= 10)
            {
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide3.xml");
                columnlist = new List<object>();
                columnwidth = new List<string>();
                columnlist = comparisonpoints.ToList();

                //update selection
                samplesizelist = new List<string>();
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add(" Metric Name", " " + advancedAnalyticsParams.MetricShortName);
                keyvalues.Add(" 3MMT June 2014", " " + advancedAnalyticsParams.ShortTimePeriod);
                keyvalues.Add(" >95%; ", " >" + Convert.ToString(StatNegative) + "%;");
                keyvalues.Add(" <95%", " <" + Convert.ToString(StatNegative) + "%");
                //
                if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                               && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                    {
                        if (columnlist.Contains(Get_ShortNames(Convert.ToString(row["Objective"]))))
                        {
                            samplesizelist.Add(cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + FormateSampleSize(Convert.ToString(row["SampleSize"])) + ")");
                        }
                    }
                }
                if (samplesizelist.Count > 0)
                {
                    sampleSize = String.Join(", ", samplesizelist);
                }
                keyvalues.Add(" Supermarket(300) ; Dollar (450)", " " + sampleSize);
                Update_Input_Selection(xmlpath, keyvalues);
                for (int i = 0; i < columnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
                }

                UpdateContingencyTable(xmlpath, contingencyDS.Tables[0], columnlist, "Table 1", "", rowheight.ToString(), columnwidth);
                //update Dimension Table
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide5.xml");
                columnlist = new List<object>() { "DIMENSION 1", "DIMENSION 2" };
                columnwidth = new List<string>() { "3059503", "3059503" }; ;
                UpdateDimensionTable(xmlpath, columnlist, "Table 1", rowheight.ToString(), columnwidth, true);
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide6.xml");
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add("Dimension Table: Metric Name", "Dimension Table: " + advancedAnalyticsParams.MetricShortName);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateDimensionTable(xmlpath, columnlist, "Table 1", rowheight.ToString(), columnwidth, false);
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide4.xml");
                Update_Appendix_Slide(xmlpath);
            }
            else if (comparisonpoints.Count <= 20)
            {
                //slide 3
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide3.xml");
                columnlist = new List<object>();
                columnwidth = new List<string>();
                columnlist = GetColumns(0, 10);
                for (int i = 0; i < columnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
                }
                //update selection
                samplesizelist = new List<string>();
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add(" Metric Name", " " + advancedAnalyticsParams.MetricShortName);
                keyvalues.Add(" 3MMT June 2014", " " + advancedAnalyticsParams.ShortTimePeriod);
                keyvalues.Add(" >95%; ", " >" + Convert.ToString(StatNegative) + "%;");
                keyvalues.Add(" <95%", " <" + Convert.ToString(StatNegative) + "%");
                //
                if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                               && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                    {
                        if (columnlist.Contains(Get_ShortNames(Convert.ToString(row["Objective"]))))
                        {
                            samplesizelist.Add(cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + FormateSampleSize(Convert.ToString(row["SampleSize"])) + ")");
                        }
                    }
                }
                if (samplesizelist.Count > 0)
                {
                    sampleSize = String.Join(", ", samplesizelist);
                }
                keyvalues.Add(" Supermarket(300) ; Dollar (450)", " " + sampleSize);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateContingencyTable(xmlpath, contingencyDS.Tables[0], columnlist, "Table 1", "", rowheight.ToString(), columnwidth);

                //slide 4
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide4.xml");
                columnlist = new List<object>();
                columnwidth = new List<string>();
                columnlist = GetColumns(10, comparisonpoints.Count);
                for (int i = 0; i < columnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
                }
                //update selection
                samplesizelist = new List<string>();
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add(" Metric Name", " " + advancedAnalyticsParams.MetricShortName);
                keyvalues.Add(" 3MMT June 2014", " " + advancedAnalyticsParams.ShortTimePeriod);
                keyvalues.Add(" >95%; ", " >" + Convert.ToString(StatNegative) + "%;");
                keyvalues.Add(" <95%", " <" + Convert.ToString(StatNegative) + "%");
                //
                if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                               && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                    {
                        if (columnlist.Contains(Get_ShortNames(Convert.ToString(row["Objective"]))))
                        {
                            samplesizelist.Add(cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + FormateSampleSize(Convert.ToString(row["SampleSize"])) + ")");
                        }
                    }
                }
                if (samplesizelist.Count > 0)
                {
                    sampleSize = String.Join(", ", samplesizelist);
                }
                keyvalues.Add(" Supermarket(300) ; Dollar (450)", " " + sampleSize);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateContingencyTable(xmlpath, contingencyDS.Tables[0], columnlist, "Table 1", "", rowheight.ToString(), columnwidth);

                //update Dimension Table
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide6.xml");
                columnlist = new List<object>() { "DIMENSION 1", "DIMENSION 2" };
                columnwidth = new List<string>() { "3059503", "3059503" }; ;
                UpdateDimensionTable(xmlpath, columnlist, "Table 1", rowheight.ToString(), columnwidth, true);
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide7.xml");
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add("Dimension Table: Metric Name", "Dimension Table: " + advancedAnalyticsParams.MetricShortName);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateDimensionTable(xmlpath, columnlist, "Table 1", rowheight.ToString(), columnwidth, false);
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide5.xml");
                Update_Appendix_Slide(xmlpath);
            }
            else if (comparisonpoints.Count <= 30 || comparisonpoints.Count > 30)
            {
                //slide 3
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide3.xml");
                columnlist = new List<object>();
                columnwidth = new List<string>();

                columnlist = GetColumns(0, 10);
                for (int i = 0; i < columnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
                }
                //update selection
                samplesizelist = new List<string>();
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add(" Metric Name", " " + advancedAnalyticsParams.MetricShortName);
                keyvalues.Add(" 3MMT June 2014", " " + advancedAnalyticsParams.ShortTimePeriod);
                keyvalues.Add(" >95%; ", " >" + Convert.ToString(StatNegative) + "%;");
                keyvalues.Add(" <95%", " <" + Convert.ToString(StatNegative) + "%");
                //
                if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                               && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                    {
                        if (columnlist.Contains(Get_ShortNames(Convert.ToString(row["Objective"]))))
                        {
                            samplesizelist.Add(cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + FormateSampleSize(Convert.ToString(row["SampleSize"])) + ")");
                        }
                    }
                }
                if (samplesizelist.Count > 0)
                {
                    sampleSize = String.Join(", ", samplesizelist);
                }
                keyvalues.Add(" Supermarket(300) ; Dollar (450)", " " + sampleSize);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateContingencyTable(xmlpath, contingencyDS.Tables[0], columnlist, "Table 1", "", rowheight.ToString(), columnwidth);

                //slide 4
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide4.xml");
                columnlist = new List<object>();
                columnwidth = new List<string>();

                columnlist = GetColumns(10, 20);
                for (int i = 0; i < columnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
                }
                //update selection
                samplesizelist = new List<string>();
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add(" Metric Name", " " + advancedAnalyticsParams.MetricShortName);
                keyvalues.Add(" 3MMT June 2014", " " + advancedAnalyticsParams.ShortTimePeriod);
                keyvalues.Add(" >95%; ", " >" + Convert.ToString(StatNegative) + "%;");
                keyvalues.Add(" <95%", " <" + Convert.ToString(StatNegative) + "%");
                //
                if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                               && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                    {
                        if (columnlist.Contains(Get_ShortNames(Convert.ToString(row["Objective"]))))
                        {
                            samplesizelist.Add(cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + FormateSampleSize(Convert.ToString(row["SampleSize"])) + ")");
                        }
                    }
                }
                if (samplesizelist.Count > 0)
                {
                    sampleSize = String.Join(", ", samplesizelist);
                }
                keyvalues.Add(" Supermarket(300) ; Dollar (450)", " " + sampleSize);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateContingencyTable(xmlpath, contingencyDS.Tables[0], columnlist, "Table 1", "", rowheight.ToString(), columnwidth);

                //slide 4
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide5.xml");
                columnlist = new List<object>();
                columnwidth = new List<string>();

                columnlist = GetColumns(20, comparisonpoints.Count);
                for (int i = 0; i < columnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
                }
                //update selection
                samplesizelist = new List<string>();
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add(" Metric Name", " " + advancedAnalyticsParams.MetricShortName);
                keyvalues.Add(" 3MMT June 2014", " " + advancedAnalyticsParams.ShortTimePeriod);
                keyvalues.Add(" >95%; ", " >" + Convert.ToString(StatNegative) + "%;");
                keyvalues.Add(" <95%", " <" + Convert.ToString(StatNegative) + "%");
                //
                if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                               && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                    {
                        if (columnlist.Contains(Get_ShortNames(Convert.ToString(row["Objective"]))))
                        {
                            samplesizelist.Add(cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + FormateSampleSize(Convert.ToString(row["SampleSize"])) + ")");
                        }
                    }
                }
                if (samplesizelist.Count > 0)
                {
                    sampleSize = String.Join(", ", samplesizelist);
                }
                keyvalues.Add(" Supermarket(300) ; Dollar (450)", " " + sampleSize);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateContingencyTable(xmlpath, contingencyDS.Tables[0], columnlist, "Table 1", "", rowheight.ToString(), columnwidth);
                //update Dimension Table
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide7.xml");
                columnlist = new List<object>() { "DIMENSION 1", "DIMENSION 2" };
                columnwidth = new List<string>() { "3059503", "3059503" }; ;
                UpdateDimensionTable(xmlpath, columnlist, "Table 1", rowheight.ToString(), columnwidth, true);
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide8.xml");
                keyvalues = new Dictionary<string, string>();
                keyvalues.Add("Dimension Table: Metric Name", "Dimension Table: " + advancedAnalyticsParams.MetricShortName);
                Update_Input_Selection(xmlpath, keyvalues);
                UpdateDimensionTable(xmlpath, columnlist, "Table 1", rowheight.ToString(), columnwidth, false);
                xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide6.xml");
                Update_Appendix_Slide(xmlpath);
            }
        }
        private void Update_Appendix_Slide(string xmlpath)
        {
            XmlDocument xmlChart = new XmlDocument();
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("a:t");
            List<string> retailerslist = new List<string>();
            DataTable dt = HttpContext.Current.Session["correspondenceData"] as DataTable;
            string sampleSize = string.Empty;
            List<string> samplesizelist = new List<string>();
            string channelRetailer = string.Empty;
            //update Retailers
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (advancedAnalyticsParams.Comparisonlist.Contains(Convert.ToString(row["Name"])))
                    {
                        retailerslist.Add(Get_ShortNames(Convert.ToString(row["Name"])));
                    }
                }
            }
            foreach (XmlNode selnode in modulegraphicFrames)
            {
                if (selnode.InnerText.Equals("Points of Comparison: Family Dollar, Dollar Tree, Dollar, General", StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "POINTS OF COMPARISON: " + String.Join(", ", retailerslist);
                }
                //update Metric
                else if (selnode.InnerText.Equals("Metric: Shopper Attitude Segment", StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "METRIC: " + advancedAnalyticsParams.MetricShortName;
                }
                //update Time Period
                else if (selnode.InnerText.Equals("Time Period: 3MMT June 2014", StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "TIME PERIOD: " + advancedAnalyticsParams.ShortTimePeriod;
                }
                //update Frequency
                else if (selnode.InnerText.Equals("Frequency: Weekly +", StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "FREQUENCY: " + advancedAnalyticsParams.ShopperFrequencyShortName;
                }
                //update filters
                else if (selnode.InnerText.Equals("Filters: None", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(advancedAnalyticsParams.FilterShortNames))
                    {
                        selnode.InnerText = "FILTERS: " + cf.GetSortedFilters(advancedAnalyticsParams.FilterShortNames);
                    }
                    else
                    {
                        selnode.InnerText = "FILTERS: NONE";
                    }
                }
            }
            xmlChart.Save(xmlpath);

            //update slide 2
            xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide2.xml");
            xmlChart.Load(xmlpath);
            modulegraphicFrames = xmlChart.GetElementsByTagName("a:t");
            //update Channels/Retailers

            if (advancedAnalyticsParams.Comparisonlist.Contains("Channels") && advancedAnalyticsParams.Comparisonlist.Contains("Retailers"))
            {
                channelRetailer = "CORRESPONDENCE PLOT : RETAILERS/CHANNELS ";
            }
            else if (advancedAnalyticsParams.Comparisonlist.Contains("Channels"))
            {
                channelRetailer = "CORRESPONDENCE PLOT : CHANNELS ";
            }
            else if (advancedAnalyticsParams.Comparisonlist.Contains("Retailers"))
            {
                channelRetailer = "CORRESPONDENCE PLOT : RETAILERS ";
            }

            foreach (XmlNode selnode in modulegraphicFrames)
            {
                if (selnode.InnerText.Equals(" Correspondence Plot : Retailers "))
                {
                    selnode.InnerText = " " + channelRetailer;
                }
                //update Measure
                else if (selnode.InnerText.Equals(" Shoppers Attitude Segment "))
                {
                    selnode.InnerText = " " + advancedAnalyticsParams.MetricShortName;
                }
                //update Time Period
                else if (selnode.InnerText.Equals(" 3MMT June 2014"))
                {
                    selnode.InnerText = " " + advancedAnalyticsParams.ShortTimePeriod;
                }
            }

            xmlChart.Save(xmlpath);
        }
        private void Update_Input_Selection(string xmlpath, Dictionary<string, string> keyvalues)
        {
            XmlDocument xmlChart = new XmlDocument();
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("a:t");
            foreach (XmlNode selnode in modulegraphicFrames)
            {
                foreach (string key in keyvalues.Keys)
                {
                    if (selnode.InnerText.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        selnode.InnerText = keyvalues[key];
                    }
                }
            }
            xmlChart.Save(xmlpath);
        }
        private void UpdateDimensionTable(string xmlpath, List<object> tablecolumnlist, string xmltblattrname, string rowheight, List<string> columnwidth, bool iscomparisonpoints)
        {
            DataTable dt = HttpContext.Current.Session["correspondenceData"] as DataTable;
            XmlDocument xmlChart = new XmlDocument();
            bool isupdate = false;
            string xmltext = string.Empty;
            string xmlColumn = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/Column.xml"), "tableheader");
            string xmlMetric = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/MetricCell.xml"), "tableheader");
            string xmlBlack = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/BlackCell.xml"), "tableheader");
            string xmlRed = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/RedCell.xml"), "tableheader");
            string xmlGreen = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/GreenCell.xml"), "tableheader");

            //update Gender

            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = " <a:tblPr><a:tableStyleId>{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>";
                    xmltext += "<a:gridCol w = \"2567795\"/>";
                    for (int width = 0; width < columnwidth.Count; width++)
                    {
                        xmltext += "<a:gridCol w = \"" + columnwidth[width] + "\"/>";
                    }
                    xmltext += "</a:tblGrid>";

                    xmltext += "<a:tr h = \"259803\">";
                    xmltext += xmlColumn.Replace("columnname", "");
                    foreach (object column in tablecolumnlist)
                    {
                        xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Convert.ToString(column)));
                    }
                    xmltext += "</a:tr>";
                    foreach (XmlNode tblnode in tblnodes)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (iscomparisonpoints)
                            {
                                if (comparisonpoints.Contains(Get_ShortNames(Convert.ToString(row["Name"]))))
                                {
                                    xmltext += "<a:tr h = \"" + rowheight + "\">";
                                    xmltext += xmlMetric.Replace("cellvalue", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Name"]))));
                                    xmltext += xmlBlack.Replace("cellvalue", Get_ShortNames(Convert.ToString(Math.Round(Convert.ToDouble(row["Dim1"]), 1))));
                                    xmltext += xmlBlack.Replace("cellvalue", Get_ShortNames(Convert.ToString(Math.Round(Convert.ToDouble(row["Dim2"]), 1))));
                                    xmltext += "</a:tr>";
                                }
                            }
                            else
                            {
                                if (!comparisonpoints.Contains(Get_ShortNames(Convert.ToString(row["Name"]))))
                                {
                                    xmltext += "<a:tr h = \"" + rowheight + "\">";
                                    xmltext += xmlMetric.Replace("cellvalue", cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Name"]))));
                                    xmltext += xmlBlack.Replace("cellvalue", Get_ShortNames(Convert.ToString(Math.Round(Convert.ToDouble(row["Dim1"]), 1))));
                                    xmltext += xmlBlack.Replace("cellvalue", Get_ShortNames(Convert.ToString(Math.Round(Convert.ToDouble(row["Dim2"]), 1))));
                                    xmltext += "</a:tr>";
                                }
                            }
                        }
                        tblnode.InnerXml = xmltext;
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        private void UpdateContingencyTable(string xmlpath, DataTable tbl, List<object> tablecolumnlist, string xmltblattrname, string segment, string rowheight, List<string> columnwidth)
        {
            XmlDocument xmlChart = new XmlDocument();
            bool isupdate = false;
            string xmltext = string.Empty;
            string xmlColumn = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/Column.xml"), "tableheader");
            string xmlMetric = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/MetricCell.xml"), "tableheader");
            string xmlBlack = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/BlackCell.xml"), "tableheader");
            string xmlRed = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/RedCell.xml"), "tableheader");
            string xmlGreen = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/GreenCell.xml"), "tableheader");
            string xmlGray = Getxmlcolumntext(HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Contingency Table/GrayCell.xml"), "tableheader");

            //update Gender

            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("p:graphicFrame");

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                isupdate = false; ;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("p", "http://schemas.openxmlformats.org/presentationml/2006/main");
                XmlNodeList cNvPrnodes = graphicnode.SelectNodes("p:nvGraphicFramePr/p:cNvPr", nsmgr);
                foreach (XmlNode cNvPr in cNvPrnodes)
                {
                    XmlAttributeCollection cNvPrAttr = cNvPr.Attributes;
                    if (cNvPrAttr != null && cNvPrAttr["name"] != null && Convert.ToString(cNvPrAttr["name"].Value).Equals(xmltblattrname, StringComparison.OrdinalIgnoreCase))
                    {
                        isupdate = true;
                    }
                }
                nsmgr = new XmlNamespaceManager(xmlChart.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                XmlNodeList tblnodes = graphicnode.SelectNodes("a:graphic/a:graphicData/a:tbl", nsmgr);
                if (isupdate)
                {
                    xmltext = " <a:tblPr><a:tableStyleId>{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}</a:tableStyleId></a:tblPr>" +
                            "<a:tblGrid>";
                    xmltext += "<a:gridCol w = \"2844000\"/>";
                    for (int width = 0; width < columnwidth.Count; width++)
                    {
                        xmltext += "<a:gridCol w = \"" + columnwidth[width] + "\"/>";
                    }
                    xmltext += "</a:tblGrid>";

                    xmltext += "<a:tr h = \"259803\">";
                    xmltext += xmlColumn.Replace("columnname", "");
                    foreach (object column in tablecolumnlist)
                    {
                        xmltext += xmlColumn.Replace("columnname", cf.cleanPPTXML(Convert.ToString(column)));
                    }
                    xmltext += "</a:tr>";
                    foreach (XmlNode tblnode in tblnodes)
                    {
                        for (int row = 0; row < tbl.Rows.Count; row++)
                        {
                            string MetricItem = cf.cleanPPTXML(tbl.Rows[row]["MetricItem"].ToString());
                            if (!MetricItem.Trim().ToLower().Contains("significance") && !MetricItem.Trim().ToLower().Contains("samplesize")
                                && !MetricItem.Trim().ToLower().Contains("number of responses"))
                            {
                                xmltext += "<a:tr h = \"" + rowheight + "\">";
                                xmltext += xmlMetric.Replace("cellvalue", Convert.ToString(tbl.Rows[row]["MetricItem"]));
                                foreach (object comparisonpoint in tablecolumnlist)
                                {
                                    string samplesize = (from r in tbl.AsEnumerable()
                                                         where Convert.ToString(r["MetricItem"]).Equals("Number of Responses", StringComparison.OrdinalIgnoreCase)
                                                         select Convert.ToString(r[Convert.ToString(comparisonpoint)])).FirstOrDefault();
                                    if(string.IsNullOrEmpty(samplesize))
                                    {
                                        samplesize = (from r in tbl.AsEnumerable()
                                                      where Convert.ToString(r["MetricItem"]).Equals("SampleSize", StringComparison.OrdinalIgnoreCase)
                                                      select Convert.ToString(r[Convert.ToString(comparisonpoint)])).FirstOrDefault();
                                    }

                                    string sigvalue = GetCellColor(Convert.ToString(tbl.Rows[row]["MetricItem"]), Convert.ToString(tbl.Rows[row + 1]["MetricItem"]), Convert.ToString(tbl.Rows[row + 1][Convert.ToString(comparisonpoint)]), samplesize);
                                    if (sigvalue.Trim().ToLower() == "red")
                                    {
                                        xmltext += xmlRed.Replace("cellvalue", CommonFunctions.GetRoundingValue(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) + "%");
                                    }
                                    else if (sigvalue.Trim().ToLower() == "green")
                                    {
                                        xmltext += xmlGreen.Replace("cellvalue", CommonFunctions.GetRoundingValue(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) + "%");
                                    }
                                    else if (sigvalue.Trim().ToLower() == "gray")
                                    {
                                        xmltext += xmlGray.Replace("cellvalue", CommonFunctions.GetRoundingValue(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) + "%");
                                    }
                                    else if (sigvalue.Trim().ToLower() == "black")
                                    {
                                        xmltext += xmlBlack.Replace("cellvalue", CommonFunctions.GetRoundingValue(Convert.ToString(tbl.Rows[row][Convert.ToString(comparisonpoint)])) + "%");
                                    }
                                }

                                xmltext += "</a:tr>";
                            }
                        }
                        tblnode.InnerXml = xmltext;
                        xmlChart.Save(xmlpath);
                        break;
                    }
                }

                if (isupdate)
                {
                    break;
                }
            }
        }
        private string GetCellColor(string currentrow, string significancerow, string significancevalue, string samplesize)
        {
            string color = string.Empty;
            double _sampleSize = 0;
            if (!string.IsNullOrEmpty(samplesize))
                _sampleSize = Convert.ToDouble(samplesize);

            if (significancevalue != "")
            {
                if ((significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + "significance") || (significancerow.Trim().ToLower() == currentrow.Trim().ToLower() + " significance"))
                {
                    if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    {
                        color = "Green";
                    }
                    else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    {
                        color = "Red";
                    }
                   else if (_sampleSize >= GlobalVariables.LowSample && _sampleSize < 100)
                    {
                        color = "Gray";
                    }
                    else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                    {
                        color = "Black";
                    }

                }
            }
            return color;
        }
        public static string Getxmlcolumntext(string xmlpath, string xmlelementname)
        {
            XmlDocument xmlChart = new XmlDocument();
            string xmltext = string.Empty;
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName(xmlelementname);

            foreach (XmlNode graphicnode in modulegraphicFrames)
            {
                xmltext = graphicnode.InnerXml;
            }

            return xmltext;
        }

        public string GetSegmentAndTotalValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = Convert.ToString(Math.Round(Convert.ToDouble(value) * 100, 0));
            }
            else
            {
                value = "0";
            }
            return value;
        }

        private void Update_Selection()
        {
            XmlDocument xmlChart = new XmlDocument();
            string xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide1.xml");
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("a:t");
            List<string> retailerslist = new List<string>();
            DataTable dt = HttpContext.Current.Session["correspondenceData"] as DataTable;
            string sampleSize = string.Empty;
            List<string> samplesizelist = new List<string>();
            string channelRetailer = string.Empty;
            //update Retailers
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (advancedAnalyticsParams.Comparisonlist.Contains(Convert.ToString(row["Name"])))
                    {
                        retailerslist.Add(Get_ShortNames(Convert.ToString(row["Name"])));
                    }
                }
            }
            foreach (XmlNode selnode in modulegraphicFrames)
            {
                if (selnode.InnerText.Equals("Points of Comparison: Family Dollar, Dollar Tree, Dollar, General",StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "POINTS OF COMPARISON: " + String.Join(", ", retailerslist);
                }
                //update Metric
                else if (selnode.InnerText.Equals("Metric: Shopper Attitude Segment", StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "METRIC: " + advancedAnalyticsParams.MetricShortName;
                }
                //update Time Period
                else if (selnode.InnerText.Equals("Time Period: 3MMT June 2014", StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "TIME PERIOD: " + advancedAnalyticsParams.ShortTimePeriod;
                }
                //update Frequency
                else if (selnode.InnerText.Equals("Frequency: Weekly +", StringComparison.OrdinalIgnoreCase))
                {
                    selnode.InnerText = "FREQUENCY: " + advancedAnalyticsParams.ShopperFrequencyShortName;
                }
                //update filters
                else if (selnode.InnerText.Equals("Filters: None", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(advancedAnalyticsParams.FilterShortNames))
                    {
                        selnode.InnerText = "FILTERS: " + cf.GetSortedFilters(advancedAnalyticsParams.FilterShortNames);
                    }
                    else
                    {
                        selnode.InnerText = "FILTERS: NONE";
                    }
                }
            }
            xmlChart.Save(xmlpath);

            //update slide 2
            xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/slides/slide2.xml");
            xmlChart.Load(xmlpath);
            modulegraphicFrames = xmlChart.GetElementsByTagName("a:t");
            //update Channels/Retailers

            if (advancedAnalyticsParams.Comparisonlist.Contains("Channels") && advancedAnalyticsParams.Comparisonlist.Contains("Retailers"))
            {
                channelRetailer = "CORRESPONDENCE PLOT : RETAILERS/CHANNELS ";
            }
            else if (advancedAnalyticsParams.Comparisonlist.Contains("Channels"))
            {
                channelRetailer = "CORRESPONDENCE PLOT : CHANNELS ";
            }
            else if (advancedAnalyticsParams.Comparisonlist.Contains("Retailers"))
            {
                channelRetailer = "CORRESPONDENCE PLOT : RETAILERS ";
            }

            //update sample size

            if (advancedAnalyticsParams.ChartDataSet != null && advancedAnalyticsParams.ChartDataSet.Tables[0] != null
                && advancedAnalyticsParams.ChartDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in advancedAnalyticsParams.ChartDataSet.Tables[0].Rows)
                {
                    samplesizelist.Add(cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Objective"]))) + " (" + FormateSampleSize(Convert.ToString(row["SampleSize"])) + ")");
                }
            }
            if (samplesizelist.Count > 0)
            {
                sampleSize = String.Join(", ", samplesizelist);
            }

            foreach (XmlNode selnode in modulegraphicFrames)
            {
                if (selnode.InnerText.Equals(" Correspondence Plot : Retailers "))
                {
                    selnode.InnerText = " " + channelRetailer;
                }
                //update Measure
                else if (selnode.InnerText.Equals(" Shoppers Attitude Segment "))
                {
                    selnode.InnerText = " " + advancedAnalyticsParams.MetricShortName;
                }
                //update Time Period
                else if (selnode.InnerText.Equals(" 3MMT June 2014"))
                {
                    selnode.InnerText = " " + advancedAnalyticsParams.ShortTimePeriod;
                }
                else if (selnode.InnerText.Equals(" Supermarket(300) ; Dollar (450)"))
                {
                    selnode.InnerText = " " + sampleSize;
                }
            }

            xmlChart.Save(xmlpath);
        }
        private string FormateSampleSize(string value)
        {
            string samplesize = string.Empty;
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                samplesize = "0";
            }
            else
            {
                samplesize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(value)));
            }
            return samplesize;
        }
        private void PlotMetrics_X_Y_Data()
        {
            XmlDocument xmlChart = new XmlDocument();
            string xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/charts/chart1.xml");
            xmlChart.Load(xmlpath);
            XmlNodeList modulegraphicFrames = xmlChart.GetElementsByTagName("c:ser");
            XmlNode metricnode = modulegraphicFrames[0];
            string xmltext = string.Empty;
            DataTable dt = HttpContext.Current.Session["correspondenceData"] as DataTable;
            try
            {

                xmltext = "<c:idx val = \"0\"/>" +
         "<c:order val = \"0\"/>" +
         "<c:tx>" +
          "<c:strRef>" +
           "<c:f>Sheet1!$B$1</c:f>" +
           "<c:strCache>" +
            "<c:ptCount val = \"1\"/>" +
            "<c:pt idx = \"0\">" +
             "<c:v>Y-Values</c:v>" +
            "</c:pt>" +
           "</c:strCache>" +
          "</c:strRef>" +
         "</c:tx>" +
         "<c:spPr>" +
          "<a:ln w = \"28575\">" +
           "<a:noFill/>" +
          "</a:ln>" +
         "</c:spPr>"+
                "<c:marker>" +
                   "<c:spPr>" +
                     "<a:ln w=\"38100\">" +
                       "<a:solidFill>" +
                         "<a:schemeClr val=\"accent1\">" +
                           "<a:shade val=\"95000\"/>" +
                           "<a:satMod val=\"105000\"/>" +
                           "<a:alpha val=\"50000\"/>" +
                         "</a:schemeClr>" +
                       "</a:solidFill>" +
                     "</a:ln>" +
                   "</c:spPr>" +
                 "</c:marker>";
                // add markers
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    xmltext += "<c:dPt>" +
              "<c:idx val = \"" + i + "\"/>" +
              "<c:marker>" +
               "<c:symbol val = \"circle\"/>" +
              "<c:size val = \"7\"/>" +
               "<c:spPr>" +
                "<a:solidFill>";
                    if (advancedAnalyticsParams.Comparisonlist.Contains(Convert.ToString(dt.Rows[i]["Name"])))
                        xmltext += "<a:srgbClr val = \"0070C0\"/>";
                    else
                        xmltext += "<a:srgbClr val = \"FF0000\"/>";

                    xmltext += "</a:solidFill>" +
                        "<a:ln w=\"38100\">" +
                      "<a:solidFill>" +
                        "<a:schemeClr val=\"accent1\">" +
                          "<a:shade val=\"95000\"/>" +
                          "<a:satMod val=\"105000\"/>" +
                          "<a:alpha val=\"50000\"/>" +
                        "</a:schemeClr>" +
                      "</a:solidFill>" +
                    "</a:ln>" +
                "</c:spPr>" +
               "</c:marker>" +
               "<c:bubble3D val = \"0\"/>" +
              "</c:dPt>";
                }

                xmltext += "<c:dLbls>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    xmltext += "<c:dLbl>" +
               "<c:idx val = \"" + i + "\"/>" +
               "<c:layout/>" +
               "<c:tx>" +
                "<c:rich>" +
                 "<a:bodyPr/>" +
                 "<a:lstStyle/>" +
                 "<a:p>" +
                  "<a:r>" +
                   "<a:rPr lang = \"en-US\" sz = \"1100\" smtClean = \"0\"/>" +
                   "<a:t>" + cf.cleanPPTXML(Get_ShortNames(Convert.ToString(dt.Rows[i]["Name"]))) + "</a:t>" +
                  "</a:r>" +
                  "<a:endParaRPr lang = \"en-US\"/>" +
                 "</a:p>" +
                "</c:rich>" +
               "</c:tx>" +
               "<c:dLblPos val = \"b\"/>" +
               "<c:showLegendKey val = \"0\"/>" +
               "<c:showVal val = \"1\"/>" +
               "<c:showCatName val = \"0\"/>" +
               "<c:showSerName val = \"0\"/>" +
               "<c:showPercent val = \"0\"/>" +
               "<c:showBubbleSize val = \"0\"/>" +
              "</c:dLbl>";
                }
                xmltext += "<c:dLblPos val = \"l\"/>" +
                 "<c:showLegendKey val = \"0\"/>" +
                 "<c:showVal val = \"1\"/>" +
                 "<c:showCatName val = \"0\"/>" +
                 "<c:showSerName val = \"0\"/>" +
                 "<c:showPercent val = \"0\"/>" +
                 "<c:showBubbleSize val = \"0\"/>" +
                 "<c:showLeaderLines val = \"0\"/>";
                xmltext += "</c:dLbls>";

                //update X, Y Values
                //update X values
                xmltext += "<c:xVal><c:numRef>" +
           "<c:f>Sheet1!$A$2:$A$" + (dt.Rows.Count + 1) + "</c:f>" +
           "<c:numCache>" +
            "<c:formatCode>General</c:formatCode>" +
            "<c:ptCount val = \"" + dt.Rows.Count + "\"/>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    xmltext += "<c:pt idx = \"" + i + "\">" +
             "<c:v>" + Convert.ToString(Convert.ToDouble(Math.Round(Convert.ToDouble(dt.Rows[i]["Dim1"]), 2)).ToString("0.00")) + "</c:v>" +
            "</c:pt>";
                }
                xmltext += "</c:numCache>" +
          "</c:numRef></c:xVal>";


                //update Y values
                XmlNodeList Yvaluesnodelist = xmlChart.GetElementsByTagName("c:yVal");
                xmltext += " <c:yVal><c:numRef>" +
          "<c:f>Sheet1!$B$2:$B$" + (dt.Rows.Count + 1) + "</c:f>" +
          "<c:numCache>" +
           "<c:formatCode>General</c:formatCode>" +
           "<c:ptCount val = \"" + dt.Rows.Count + "\"/>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    xmltext += "<c:pt idx = \"" + i + "\">" +
             "<c:v>" + Convert.ToString(Convert.ToDouble(Math.Round(Convert.ToDouble(dt.Rows[i]["Dim2"]), 2)).ToString("0.00")) + "</c:v>" +
            "</c:pt>";
                }
                xmltext += "</c:numCache>" +
          "</c:numRef></c:yVal>";
                xmltext += "<c:smooth val = \"0\"/>";

                metricnode.InnerXml = xmltext;
                xmlChart.Save(xmlpath);
            }
            catch (Exception ex)
            { 
            
            }
        }
        private void Plot_Excel_Data()
        {
            XmlDocument xmlChart = new XmlDocument();
            string xmlpath = HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Microsoft_Excel_Worksheet1/xl/worksheets/sheet1.xml");
            xmlChart.Load(xmlpath);
            XmlNodeList sheetnode = xmlChart.GetElementsByTagName("sheetData");
            string xmltext = string.Empty;
            int rowNumber = 2;
            DataTable dt = HttpContext.Current.Session["correspondenceData"] as DataTable;

            xmltext += "<row " + "r = \"1\" spans = \"1:2\" x14ac:dyDescent = \"0.25\">";
            xmltext += "<c r = \"A1\" s = \"1\" t = \"s\"><v>0</v></c>";
            xmltext += "<c r = \"B1\" s = \"1\" t = \"s\"><v>1</v></c>";
            xmltext += "</row>";
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                xmltext += "<row " + "r = \"" + (rowNumber) + "\" spans = \"1:2\" x14ac:dyDescent = \"0.25\">";
                xmltext += "<c r = \"A" + rowNumber + "\" s = \"1\"><v>" + Convert.ToString(Convert.ToDouble(Math.Round(Convert.ToDouble(dt.Rows[row]["Dim1"]), 2)).ToString("0.00")) + "</v></c>";
                xmltext += "<c r = \"B" + rowNumber + "\" s = \"1\"><v>" + Convert.ToString(Convert.ToDouble(Math.Round(Convert.ToDouble(dt.Rows[row]["Dim2"]), 2)).ToString("0.00")) + "</v></c>";
                xmltext += "</row>";
                rowNumber += 1;
            }
            sheetnode[0].InnerXml = xmltext;
            xmlChart.Save(xmlpath);

            //update table

            xmlpath = HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Microsoft_Excel_Worksheet1/xl/tables/table1.xml");
            xmlChart.Load(xmlpath);
            xmlChart.DocumentElement.Attributes[4].Value = "A1:B" + (dt.Rows.Count + 1);
            xmlChart.Save(xmlpath);
            string tempDir = HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/Microsoft_Excel_Worksheet1");
            string fileName = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/embeddings/Microsoft_Excel_Worksheet1.xlsx");
            ZipDirectory(tempDir, fileName);
        }
        private void Set_Axis_Min_Max_Values()
        {
            XmlDocument xmlChart = new XmlDocument();
            DataTable dt = HttpContext.Current.Session["correspondenceData"] as DataTable;
            string xmlpath = HttpContext.Current.Server.MapPath(UserExportFileName + "/ppt/charts/chart1.xml");
            var query = from r in dt.AsEnumerable()
                        select r.Field<double>("Dim1");
            List<double> Xvalues = query.Distinct().ToList();

            var query2 = from r in dt.AsEnumerable()
                         select r.Field<double>("Dim2");
            List<double> Yvalues = query2.Distinct().ToList();
            xmlChart.Load(xmlpath);
            XmlNodeList maxnodes = xmlChart.GetElementsByTagName("c:max");
            string xmltext = string.Empty;
            XmlAttributeCollection maxAttr = maxnodes[0].Attributes;
            if (maxAttr != null && maxAttr["val"] != null)
            {
                maxAttr["val"].Value = (Math.Round(Xvalues.Max(), 2) + 0.1).ToString("0.0");
            }
            maxAttr = maxnodes[1].Attributes;
            if (maxAttr != null && maxAttr["val"] != null)
            {
                maxAttr["val"].Value = (Math.Round(Yvalues.Max(), 2) + 0.1).ToString("0.0");
            }
            xmlChart.Save(xmlpath);

            maxnodes = xmlChart.GetElementsByTagName("c:min");
            XmlAttributeCollection minAttr = maxnodes[0].Attributes;

            if (minAttr != null && minAttr["val"] != null)
            {
                minAttr["val"].Value = (Xvalues.Min() + -0.1).ToString("0.0");
            }
            minAttr = maxnodes[1].Attributes;
            if (minAttr != null && minAttr["val"] != null)
            {
                minAttr["val"].Value = (Yvalues.Min() + -0.1).ToString("0.0");
            }
            xmlChart.Save(xmlpath);
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
        private string FormateDateAndTime(string month)
        {
            if (month.Length == 1)
            {
                return "0" + month;
            }
            else
                return month;
        }
        private void CopyFilesToDestination()
        {
            string source = string.Empty;
            if (comparisonpoints.Count <= 10)
            {
                source = HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/PPT Template-1");
            }
            else if (comparisonpoints.Count <= 20)
            {
                source = HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/PPT Template-2");
            }
            else if (comparisonpoints.Count <= 30 || comparisonpoints.Count > 30)
            {
                source = HttpContext.Current.Server.MapPath("~/CorrespondenceExportPPTFiles/PPT Template-3");
            }
            UserParams userparam = HttpContext.Current.Session[SessionVariables.USERID] as UserParams;
            if (userparam == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");
                }
                else
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                }
            }
            UserExportFileName = "~/CorrespondenceUserExportFiles/" + userparam.UserName + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UserExportFileName));
            }

            string destination = HttpContext.Current.Server.MapPath(UserExportFileName);
            DirectoryCopy(source, destination, true);
        }
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public void ZipDirectory(string sourceDirectory, string zipFileName)
        {
            (new FastZip()).CreateZip(zipFileName, sourceDirectory, true, null);
            //ICSharpCode.SharpZipLib.GZip.GZipInputStream ff;
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
            HeaderTabs.Add("VisitPreparation", "Preparation Types");
            HeaderTabs.Add("TechnologyUsed", "Use of Technology to Prepare");
            HeaderTabs.Add("ComputerBased", "Computer-Based Preparation Activities");
            HeaderTabs.Add("SmartPhoneBased", "Smartphone-Based Preparation Activities");
            HeaderTabs.Add("DestinationItemSummary", "Destination Item Summary");
            HeaderTabs.Add("DestinationItemDetails", "Destination Item Detail");

            HeaderTabs.Add("A supermarket or grocery store", "Supermarket / Grocery");
            HeaderTabs.Add("A convenience store or gas station food mart (excluding gas)", "Convenience");
            HeaderTabs.Add("A drug store", "Drug");
            HeaderTabs.Add("A dollar store such as Family Dollar or Dollar General", "Dollar");
            HeaderTabs.Add("A warehouse club such as Sam`s Club or Costco", "Club");
            HeaderTabs.Add("A Mass Merchandise store or super center such as walmart, target, walmart supercenter, or supertarget", "Mass Merc. with Supers");
            HeaderTabs.Add("A mass merchandise store without a full-line grocery section such as Walmart or Target", "Mass Merc.");
            HeaderTabs.Add("A mass merchandise supercenter with a full-line grocery section such as Walmart Supercenter or SuperTarget", "Supercenter");

            HeaderTabs.Add("Shopper Attitude", "Top 2 Box Attitudinal Statements");
            HeaderTabs.Add("Attitudinal Segment", "Attitudinal Segment");

            HeaderTabs.Add("RetailerLoyaltyPyramid", "Retailer Loyalty Pyramid - Total Grocery Across Channel(Applicable only for Retailers)");
            HeaderTabs.Add("TopBox", "Loyalty and Satisfaction Detail(Applicable only for Retailers)");
            HeaderTabs.Add("MainFavoriteStore", "Main/Favorite Store for Grocery Spending(Applicable only for Retailers)");

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
            HeaderTabs.Add("Smartphone/TabletInfluencedPurchases", "Smartphone/Tablet Influenced Purchases?");

            HeaderTabs.Add("SmartPhoneUsage", "Ways Technology Used to Influence");
            HeaderTabs.Add("BeverageBrandsPurchased", "Beverage Brands Purchased: SSD");

            HeaderTabs.Add("ItemsPurchasedSummary", "Items Purchased Summary");
            HeaderTabs.Add("InStoreDestinationDetails", "Items Purchased Detail");
            HeaderTabs.Add("ImpulseItem", "Impulse Item");
            HeaderTabs.Add("TripMission", "Trip Mission");

            HeaderTabs.Add("WayTabletInfluenced", "Way Tablet Influenced");
            HeaderTabs.Add("WaySmartphoneInfluenced", "Way Smartphone Influenced");

            HeaderTabs.Add("TimeSpent", "Time Spent in Store");
            HeaderTabs.Add("TripExpenditure", "Trip Expenditure");
            HeaderTabs.Add("CheckOutType", "Checkout Method");
            HeaderTabs.Add("ConsideredStoreVisits", "Considered Store Visits");
            HeaderTabs.Add("VisitMotiviations", "Visit Motiviations");
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

            HeaderTabs.Add("BeverageConsumedMonthly", "Beverage Consumed Monthly");
            HeaderTabs.Add("BeveragepurchasedMonthly", "Beverage purchased Monthly");
            HeaderTabs.Add("FAVOURITESPORTDRINKS", "FAVORITE SPORT DRINKS");
            HeaderTabs.Add("FAVOURITEENERGYDRINKS", "FAVORITE ENERGY DRINKS");
            HeaderTabs.Add("FAVOURITEREGSSD", "FAVORITE REG SSD");
        }
        private string Get_ShortNames(String spVal)
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
    }
}