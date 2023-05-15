using AQ.Common.GenerateReport;
using iSHOP.BLL;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Xml;

namespace iSHOPNew.Models
{
    public class Download_ShopperReportForOverTime : BaseReport
    {           
        //Build slides
        public override List<FileDetails> BuildSlides()
        {
            StatTesting = Convert.ToDouble(Session["PercentStat"]);
            accuratestatvalueposi = Convert.ToDouble(Session["StatSessionPosi"]);
            accuratestatvaluenega = Convert.ToDouble(Session["StatSessionNega"]);
            reportparams = HttpContext.Current.Session["GenerateReportParams"] as ReportGeneratorParams;
            List<FileDetails> filelist = null;
            FileDetails _fileDetails = null;
            DataSet ds = null;        
            try
            { 
            List<String> preferences = null;
            if (reportparams.ModuleBlock.Equals("TimeShopper", StringComparison.OrdinalIgnoreCase))
            {
                preferences = new List<String> { "SUMMARY", "FREQUENTSHOPPER", "CROSSRETAILERSHOPPER",
                                                          "SHOPPERPERCEPTION", "BEVERAGEINTERACTION", "APPENDIX" };
            }
            else
            {
                preferences = new List<String> { "SUMMARY", "VISITORPROFILE", "TRIPTYPE", "PRESHOP",
                                                          "INSTORE", "TRIPSUMMARY", "APPENDIX" };
            }

            IEnumerable<String> orderedData = reportparams.SelectedReports.OrderBy(
               item => preferences.IndexOf(item));
            reportparams.SelectedReports = orderedData.ToList();

            ReportNumber = 1;
            Dictionary<string, int> serialno = preferences.ToDictionary(x => x, x => ReportNumber++);
            //lststatcolour = new List<CalculationColor>(){                       
            //             new CalculationColor(){Compare =CalculateEnum.EQUAL,CompareValue = 2000, ColorCode="#808080"},
            //              new CalculationColor(){Compare = CalculateEnum.GREATERTHAN,CompareValue = accuratestatvalueposi, ColorCode="#00B050"},
            //            new CalculationColor(){Compare = CalculateEnum.LESSERTHAN,CompareValue = accuratestatvaluenega, ColorCode="#FF0000"},
            //            new CalculationColor(){Compare = CalculateEnum.GREATEREQUALANDLESSEREQUAL,CompareValue = accuratestatvalueposi, ColorCode="#000000"}
            //             };
            lststatcolour = new List<CalculationColor>(){
                 new CalculationColor(){Compare =CalculateEnum.EQUAL,CompareValue = 1000, ColorCode="#0000ff"},
                  new CalculationColor(){Compare =CalculateEnum.EQUAL,CompareValue = 2000, ColorCode="#808080"},
                new CalculationColor(){Compare =CalculateEnum.GREATERTHAN,CompareValue = accuratestatvalueposi, ColorCode="#00B050"},
                new CalculationColor(){Compare =CalculateEnum.LESSERTHAN,CompareValue = accuratestatvaluenega, ColorCode="#FF0000"},
                new CalculationColor(){Compare =CalculateEnum.GREATEREQUALANDLESSEREQUAL,CompareValue = accuratestatvalueposi, ColorCode="#000000"},
                new CalculationColor(){Compare =CalculateEnum.DEFAULT,CompareValue = accuratestatvalueposi, ColorCode="#262626"}
            };

            if (reportparams != null && reportparams.ChartDataSet != null && reportparams.ChartDataSet.Count > 0)
            {
                filelist = new List<FileDetails>();
                foreach (string key in reportparams.SelectedReports)
                {
                    ReportNumber = serialno[key];
                    ds = null;
                    if (reportparams.ChartDataSet.ContainsKey(key.ToString()))
                    {
                        ds = reportparams.ChartDataSet[key.ToString()];
                    }
                    if (key.Equals("SUMMARY", StringComparison.OrdinalIgnoreCase))
                    {
                        _fileDetails = Build_SUMMARY_Slides(ds);
                        if (_fileDetails != null)
                        {
                            _fileDetails.PlaceUnderFolderPath = "Individual reports";
                            //filelist.Add(_fileDetails);
                        }
                    }
                    else if (ds != null && ds.Tables.Count > 0)
                    {
                        switch (key.ToUpper())
                        {
                            case "VISITORPROFILE":
                                {
                                    _fileDetails = Build_VISITORPROFILE_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "TRIPTYPE":
                                {
                                    _fileDetails = Build_TRIPTYPE_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "PRESHOP":
                                {
                                    _fileDetails = Build_PRESHOP_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "INSTORE":
                                {
                                    _fileDetails = Build_INSTORE_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "TRIPSUMMARY":
                                {
                                    _fileDetails = Build_TRIPSUMMARY_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "FREQUENTSHOPPER":
                                {
                                    _fileDetails = Build_FREQUENTSHOPPER_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "CROSSRETAILERSHOPPER":
                                {
                                    _fileDetails = Build_CROSSRETAILERSHOPPER_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "SHOPPERPERCEPTION":
                                {
                                    _fileDetails = Build_SHOPPERPERCEPTION_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "BEVERAGEINTERACTION":
                                {
                                    _fileDetails = Build_BEVERAGEINTERACTION_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                            case "APPENDIX":
                                {
                                    _fileDetails = Build_APPENDIX_Slides(ds);
                                    if (_fileDetails != null)
                                    {
                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                        //filelist.Add(_fileDetails);
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            filelist.Add(_fileDetails);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return filelist;
        }
        private FileDetails Build_SUMMARY_Slides(DataSet dataSet)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            //Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\1_140922_overtime overview_2 3MMT_summary_V0.1");
            if (reportparams.ModuleBlock.Equals("TimeShopper", StringComparison.OrdinalIgnoreCase))
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Trend - Across Shoppers\Trend - Across Shoppers");
            else
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Trend - Compare Path Purchase\Trend - Compare Path Purchase");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Retailer Summary");
            destination_FilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            string[] ChannelOrRetailerlist = reportparams.ShopperSegment.Split('|');
            string ChannelOrRetailer = ChannelOrRetailerlist[ChannelOrRetailerlist.Length - 1];

            slide.ReplaceText.Add(" Retailer/Channel: Kroger", " Retailer/Channel: " + Get_ShortNames(ChannelOrRetailer));
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + Get_ShortNames(ChannelOrRetailer));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slidelist.Add(slide);

            //Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent ")

            //Slide 2
            slide = new SlideDetails();
            //start- added by bramhanath for New Slide Changes
            slide.ReplaceText.Add("95%", StatTesting + "%");
            slide.ReplaceText.Add("This report compares RETAILER Monthly+ Shoppers who shopped between BENCHMARK and COMPARISON1 across the key metrics in the iSHOP survey", "This report compares " + Get_ShortNames(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "")) + (CommonFunctions.Channellist.Contains(Convert.ToString(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty) + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Shoppers who shopped between " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers who shopped between ") + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " and " + reportparams.Comparisonlist + " across the key metrics in the iSHOP survey.");
            slide.ReplaceText.Add("Monthly+ ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " ");
            slide.ReplaceText.Add("BENCHMARK ", Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            slide.ReplaceText.Add("COMPARISON1 ", Convert.ToString(reportparams.Comparisonlist).ToUpper() + " ");
            slide.ReplaceText.Add("BENCHMARK and COMPARISON1 ", Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " and " + Convert.ToString(reportparams.Comparisonlist).ToUpper());
            slide.ReplaceText.Add("RETAILER ", Get_ShortNames(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "")) + (CommonFunctions.Channellist.Contains(Convert.ToString(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty) + " ");
            slide.ReplaceText.Add("RETAILER Monthly+ Frequent ", Get_ShortNames(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "")) + (CommonFunctions.Channellist.Contains(Convert.ToString(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty) + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent "));
            slide.ReplaceText.Add("RETAILER Monthly+ Frequent Shopper’s", Get_ShortNames(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "")) + (CommonFunctions.Channellist.Contains(Convert.ToString(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty) + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s"));
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as a benchmark", "All statistical testing is compared against " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " as a benchmark");
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as a benchmark ", "All statistical testing is compared against " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " as a benchmark ");
            slide.ReplaceText.Add("Trips taken to RETAILER ", "Trips taken to " + Get_ShortNames(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Retailers|", "").Replace("Channels|", "")) + (CommonFunctions.Channellist.Contains(Convert.ToString(ChannelOrRetailer.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty) + " ");
            //end
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            files.ReplaceImages = AddRetailerImages(ChannelOrRetailer);
            fileName = ReportNumber + ".Retailer Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private Dictionary<string, string> AddRetailerImages(string ShopperSegment)
        {
            Dictionary<string, string> ReplaceValues = new Dictionary<string, string>();
            //if (!string.IsNullOrEmpty(ShopperSegment))
            //{
            //    ReplaceValues.Add("image5.png", GetRetailerImagePath(Get_ShortNames(ShopperSegment.Replace("Retailers|", "").Replace("Channels|", ""))));
            //}           
            return ReplaceValues;
        }
        public Dictionary<string, string> GetSourceDetail(string stroption)
        {
            Dictionary<string, string> DictSourceDetails = new Dictionary<string, string>();
            string strFilter = "";
            string strResult = "";
            string stroriginal = "Source: iSHOP: <<time period>>; Channel/Retailer: <<Retailer>>; Base: <<Frequency>>; Filters: <<Advanced Filers>>";
            string[] filt;

            filt = reportparams.Filters.Split('|');

            for (int i = 0; i < filt.Count(); i++)
            {
                if (i == 0)
                {
                    strFilter = Convert.ToString(filt[i]);
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        strFilter += "; " + Convert.ToString(filt[i]);
                    }
                    else if (i % 2 != 0)
                    {
                        strFilter += ": " + Convert.ToString(filt[i]);
                    }
                }
            }

            if (ReportNumber == 9)
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Split('|')[1])) ? " Channel" : string.Empty) + AppendStars((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Split('|')[1])) ? " Channel" : string.Empty) + AppendStars((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }
            else if (ReportNumber == 11)
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Split('|')[1])) ? " Channel" : string.Empty) + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Split('|')[1])) ? " Channel" : string.Empty) + AppendStars((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }
            else
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Split('|')[1])) ? " Channel" : string.Empty) + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Split('|')[1])) ? " Channel" : string.Empty) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }

            DictSourceDetails.Add(stroriginal, strResult);
            //DictSourceDetails.Add("* Significantly differed with <#benchmark> ", "* Significantly differed with " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            //DictSourceDetails.Add("* Significantly Different from <#benchmark> ", "* Significantly Different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " is significantly higher  ");
            DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add(">95%", ">" + Convert.ToString(StatTesting) + "%");
            DictSourceDetails.Add("<95%", "<" + Convert.ToString(StatTesting) + "%");
            return DictSourceDetails;
        }  
        public override DataSet FilterData(DataSet dtbl, [Optional] string strGapOption)
        {
            string[] strBenchMark = reportparams.Benchmark.Split('|');
            DataSet ds = new DataSet();
            List<string> metrics = new List<string>();
            if (dtbl != null && dtbl.Tables.Count > 0)
            {
                ds = dtbl.Copy();
                foreach (DataTable tb in ds.Tables)
                {
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        if (!metrics.Contains(Get_ShortNames(Convert.ToString(tb.Rows[0]["Metric"]).Trim())))
                        {
                            metrics.Add(Get_ShortNames(Convert.ToString(tb.Rows[0]["Metric"]).Trim()));
                        }
                        foreach (DataRow row in tb.Rows)
                        {
                            if (strBenchMark.Length > 1)
                            {
                                if (Convert.ToString(row["Objective"]).Equals(strBenchMark[1].Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), StringComparison.OrdinalIgnoreCase))
                                {
                                    row["Significance"] = 1000;
                                }
                                else
                                {
                                    row["Significance"] = CommonFunctions.SetReportMediumSamplesizeColorNumber(Convert.ToString(row["NoOfRespondents"]), Convert.ToString(row["Significance"]), accuratestatvalueposi, accuratestatvaluenega);
                                }
                            }
                            if (System.DBNull.Value != row["Volume"] && Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(row["Volume"])) ? "0" : row["Volume"])) != 0)
                            {
                                row["Volume"] = Convert.ToDouble(String.IsNullOrEmpty(Convert.ToString(row["Volume"])) ? "0" : row["Volume"]) / 100;
                            }
                            row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"])).Trim().ToUpper();
                            row["Metric"] = cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Metric"])).Trim()).Replace("&amp;lt;", "<").Replace("&lt;", "<").Replace("&amp;", "&");
                            row["MetricItem"] = cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["MetricItem"])).Trim()).Replace("&amp;lt;", "<").Replace("&lt;", "<").Replace("&amp;", "&").Replace("&#8217;", "`");
                        }
                    }
                }
            }           
            return ds;
        }
        public DataTable FilterDataTable(DataTable tb, [Optional] string strGapOption)
        {
            string[] strBenchMark = reportparams.Benchmark.Split('|');
            foreach (DataRow row in tb.Rows)
            {
                if (strBenchMark.Length > 1)
                {
                    if (Convert.ToString(row["Objective"]).Equals(strBenchMark[1].Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 6MMT", "").Replace(" 12MMT", ""), StringComparison.OrdinalIgnoreCase))
                    {
                        row["Significance"] = 1000;
                    }
                    else
                    {
                        row["Significance"] = CommonFunctions.SetReportMediumSamplesizeColorNumber(Convert.ToString(row["NoOfRespondents"]), Convert.ToString(row["Significance"]), accuratestatvalueposi, accuratestatvaluenega);
                    }
                }
                if (System.DBNull.Value != row["Volume"] && Convert.ToDouble((String.IsNullOrEmpty(Convert.ToString(row["Volume"])) ? "0" : row["Volume"])) != 0)
                {
                    row["Volume"] = Convert.ToDouble(String.IsNullOrEmpty(Convert.ToString(row["Volume"])) ? "0" : row["Volume"]) / 100;
                }
                row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"])).Trim().ToUpper();
                row["Metric"] = cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["Metric"])).Trim()).Replace("&amp;lt;", "<").Replace("&lt;", "<").Replace("&amp;", "&");
                row["MetricItem"] = cf.cleanPPTXML(Get_ShortNames(Convert.ToString(row["MetricItem"])).Trim()).Replace("&amp;lt;", "<").Replace("&lt;", "<").Replace("&amp;", "&").Replace("&#8217;", "`");
            }
            return tb;
        }
        //Get Sample Size
        private string Get_SampleSize(DataTable tbl)
        {
            string samplesize = string.Empty;
            List<string> samplesizelist = new List<string>();
            if (tbl != null && tbl.Rows.Count > 0)
            {
                List<string> objects= (from myRow in tbl.AsEnumerable()
                                      select Convert.ToString(myRow.Field<Object>("Objective"))).Distinct().ToList();
                               foreach(string obj in objects)

                {
                    var _samplesize = (from myRow in tbl.AsEnumerable()
                                         where Convert.ToString(myRow.Field<Object>("Objective")).Equals(obj,StringComparison.OrdinalIgnoreCase)
                                         orderby myRow.Field<Object>("NoOfRespondents") descending
                                         select Convert.ToString( myRow.Field<Object>("NoOfRespondents"))).Take(1).FirstOrDefault();
                    samplesizelist.Add(cf.cleanPPTXML(Convert.ToString(obj).ToUpper()) + " (" + FormateSampleSize(_samplesize) + ")");
                }

                //var query = from myRow in tbl.AsEnumerable()
                //            select (cf.cleanPPTXML(Convert.ToString(myRow.Field<Object>("Objective"))).ToUpper() + " (" + FormateSampleSize(Convert.ToString(myRow.Field<Object>("SampleSize"))) + ")");
                //List<string> Rowlist = query.Distinct().ToList();
                if (samplesizelist != null && samplesizelist.Count > 0)
                {
                    samplesize = String.Join(", ", samplesizelist);
                }
            }
            return samplesize;
        }          
        private FileDetails Build_APPENDIX_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            List<object> appendixcolumnlist = new List<object>();
            columnlist = new List<object>();
            if (reportparams.ModuleBlock == "TimeShopper")
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\11_140922overtime overview_2 3MMT_Appendix_V0.1");
            }
            else
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\11_140922overtime overview_2 3MMT_Appendix_V0.1 - Trips");
            }
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Appendix");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            if (reportparams.ModuleBlock == "TimeShopper")
            {
                //Slide 1
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
                slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
                slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
                 slide.ReplaceText.Add("Weekly+ Shoppers", reportparams.ShopperFrequencyShortName + " Shoppers");
                slidelist.Add(slide);

                ////Slide 2
                //slide = new SlideDetails();
                //slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Trips");


                //tbl = Get_Chart_Table(ds, "ReasonForStoreChoice");
                //var query = from r in tbl.AsEnumerable()
                //            select r.Field<object>("Objective");
                //appendixcolumnlist = query.Distinct().ToList();
                //tbl = CreateAppendixTable(tbl);
                //GetTableHeight_FontSize(tbl);
                //columnwidth = new List<string>();
                //for (int i = 0; i < appendixcolumnlist.Count; i++)
                //{
                //    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
                //}
                //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
                //UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 1", rowheight.ToString(), columnwidth, "Reasons for Store Choice");
                //slidelist.Add(slide);

                //Slide 3
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                slide.ReplaceText = GetSourceDetail("Shoppers");
                tbl = Get_Chart_Table(ds, "StoreAttribute",33,1);
                var query2 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query2.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                GetTableHeight_FontSize(tbl);
                columnwidth = new List<string>();
                for (int i = 0; i < appendixcolumnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
                }
                xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
                UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 2", rowheight.ToString(), columnwidth, "Store Imagery");
                slidelist.Add(slide);
                //

                //Slide 4
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                slide.ReplaceText = GetSourceDetail("Shoppers");
                tbl = Get_Chart_Table(ds, "GoodPlaceToShop",34,1);
                var query3 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query3.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                GetTableHeight_FontSize(tbl);
                columnwidth = new List<string>();
                for (int i = 0; i < appendixcolumnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
                }
                xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
                UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 1", rowheight.ToString(), columnwidth, "Good Place To Shop For");
                slidelist.Add(slide);
                //
            }
            else
            {
                //Slide 1
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
                slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
                slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
                slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: All Trips");
                slidelist.Add(slide);

                //Slide 2
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                slide.ReplaceText = GetSourceDetail("Trips");


                tbl = Get_Chart_Table(ds, "ReasonForStoreChoice",37,1);
                var query = from r in tbl.AsEnumerable()
                            select r.Field<object>("Objective");
                appendixcolumnlist = query.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                GetTableHeight_FontSize(tbl);
                columnwidth = new List<string>();
                for (int i = 0; i < appendixcolumnlist.Count; i++)
                {
                    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
                }
                xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
                UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 2", rowheight.ToString(), columnwidth, "Reason for Store Choice");
                slidelist.Add(slide);
            }

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Appendix";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_BEVERAGEINTERACTION_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\10_140922overtime overview_2 3MMT_Beverage Intraction_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Beverage Interaction");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
             slide.ReplaceText.Add("Weekly+ Shoppers", reportparams.ShopperFrequencyShortName + " Shoppers");
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");


            metriclist = new List<string>();
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Beverage Purchase Monthly", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            ////Slide 3
            //slide = new SlideDetails();
            //slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            ////chart Age
            //chart = new ChartDetails();
            //chart.Type = ChartType.LINE;
            //chart.ShowDataLegends = true;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //tbl = Get_Chart_Table(ds, "Beverage Consumed Monthly");
            //chart.Data = tbl;
            //chart.XAxisColumnName = "MetricItem";
            //chart.YAxisColumnName = "Volume";
            //chart.MetricColumnName = "Objective";
            //chart.ColorColumnName = "Significance";
            //lststatcolour = new List<CalculationColor>(){
            //            new CalculationColor(){Compare = CalculateEnum.GREATERTHAN,CompareValue = accuratestatvalueposi, ColorCode="#00B050"},
            //            new CalculationColor(){Compare = CalculateEnum.LESSERTHAN,CompareValue = accuratestatvaluenega, ColorCode="#FF0000"},
            //            new CalculationColor(){Compare = CalculateEnum.GREATEREQUALANDLESSEREQUAL,CompareValue = accuratestatvalueposi, ColorCode="#000000"}
            //             };
            //chart.TextColor = lststatcolour;
            //slide.Charts.Add(chart);
            ////            
            //slide.ReplaceText.Add("Top 10 categories consumed among  Weekly+ shoppers within Retailer", "Top 10 categories consumed among  " + reportparams.ShopperFrequencyShortName + " shoppers within Retailer");
            //slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            //slidelist.Add(slide);
            //

            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl = FilterDataTable(Get_Chart_Table(ds, "BeveragepurchasedMonthly",21,1));//"Beverage purchased Monthly");
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
         
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //          
            slide.ReplaceText.Add("Top 10 categories purchased among  Weekly+ shoppers within Retailer", "Top 10 categories purchased among  " + reportparams.ShopperFrequencyShortName + " shoppers within Retailer");
            slide.ReplaceText.Add("Top 10 Categories Purchased Among Weekly+ Shoppers", "Top 10 Categories Purchased Among " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            List<string> top10metrics = new List<string>();
            //var query = from r in tbl.AsEnumerable()
            //            select r.Field<string>("MetricItem");
            //top10metrics = query.Distinct().ToList();
            List<String> top2 = new List<String>() { "SSD Regular", "SSD Diet" };       
            var query3 = from r in tbl.AsEnumerable()
                         select Convert.ToString(r.Field<object>("MetricItem"));
            //List<string> top10 = top2;
            //top10.AddRange(query3.Distinct().ToList());
            top10metrics = query3.Distinct().ToList();
            int tbl_slide_no = slide.SlideNumber + 1;

            List<string> metrictitels = new List<string>() { "Top Regular Carbonated Soft Drinks Brands Purchased Monthly",
                                                             "Top Diet Carbonated Soft Drinks Brands Purchased Monthly",
                                                             "Top Non-Sparkling Water Brands Purchased Monthly",
                                                             "Top RTD Juice Brands Purchased Monthly",
                                                             "Top 100% Juice Brands Purchased Monthly",
                                                             "Top 100% Orange Juice Brands Purchased Monthly",
                                                             "Top Tea Brands Purchased Monthly",
                                                             "Top Coffee Brands Purchased Monthly",
                                                             "Top Sports Drinks Brands Purchased Monthly",
                                                             "Top Energy Drinks Brands Purchased Monthly"};
            int slideno = 4;
            int chartno = 2;
            for (int i = 0; i < 10; i++)
            {
                //Slide 8
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                slide.ReplaceText = GetSourceDetail("Shoppers");
                tbl = new DataTable();
                if (top10metrics.Count > i)
                {
                    tbl =  FilterDataTable(Get_Chart_Table(ds, top10metrics[i], tbl_slide_no,1));
                    //chart Age
                    if (tbl != null && tbl.Rows.Count > 0)
                    {
                        chart = new ChartDetails();
                        chart.Type = ChartType.LINE;
                        chart.ShowDataLegends = true;
                        chart.DataLabelFormatCode = "0.0%";
                        chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
                        chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");

                        chart.Data = tbl;
                        chart.XAxisColumnName = "MetricItem";
                        chart.YAxisColumnName = "Volume";
                        chart.MetricColumnName = "Objective";
                        chart.ColorColumnName = "Significance";
                       
                        chart.TextColor = lststatcolour;
                        slide.Charts.Add(chart);
                    }

                    //               
                    //if (top10metrics[i].Equals("Unflavored Bottled Water (Non-Sparkling)", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    slide.ReplaceText.Add(metrictitels[i], "         Top " + top10metrics[i] + " Brands Purchased Monthly");
                    //}
                    //else
                    //{
                    slide.ReplaceText.Add(metrictitels[i], "Top " + top10metrics[i] + " Brands Purchased Monthly");
                }
                slide.ReplaceText.Add("Top Brands Purchased Among Weekly+ Shoppers", "Top Brands Purchased Among  " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
                slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
                slidelist.Add(slide);
                slideno += 1;
                chartno += 1;
                //
            }

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Beverage Interaction";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_SHOPPERPERCEPTION_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\9_140922overtime overview_2 3MMT_Shopper Perceive_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
             slide.ReplaceText.Add("Weekly+ Shoppers", reportparams.ShopperFrequencyShortName + " Shoppers");
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");


            metriclist = new List<string> { "StoreAttribute0", "StoreAttribute1", "StoreAttribute2", "GoodPlaceToShop Top 10", "GoodPlaceToShop0", "GoodPlaceToShop1", "GoodPlaceToShop2" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "StoreAttributesFactors",13,1));//"Store Attributes Factors");
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
            
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //           
            slide.ReplaceText.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "StoreAttribute Top 10", 14, 1)); //"Good Place To Shop Factors");
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //  
            slide.ReplaceText.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Product imagery of Weekly+ shoppers", "Product imagery of " + reportparams.ShopperFrequencyShortName + " shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 5
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "GoodPlaceToShopFactors", 15, 1));//"Good Place To Shop Factors"); //"Store Attributes");
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            slide.ReplaceText.Add("Difference in Store imagery between 3MMT DEC 2013 and 3MMT OCT 2013 for Weekly+ Shoppers ", "Difference in Store imagery between 3MMT DEC 2013 and 3MMT OCT 2013 for " + reportparams.ShopperFrequencyShortName + " Shoppers ");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //   

            //Slide 6
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //tbl =  FilterDataTable(Get_Chart_Table(ds, "GoodPlaceToShopFactors");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "GoodPlaceToShop Top 10", 16, 1));            
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
         
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //        
            slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //   

            //Slide 7
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "MainFavoriteStore", 17, 1)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among  " + reportparams.ShopperFrequencyShortName + " shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //          
            //Slide 8
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "RetailerLoyaltyPyramid", 18, 1)); //Retailer Loyalty Pyramid - Total Grocery Across Channel
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //   
            slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among  " + reportparams.ShopperFrequencyShortName + " shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //          
            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_CROSSRETAILERSHOPPER_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\8_140922overtime overview_2 3MMT_Frequently Go_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Cross Retailer");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slide.ReplaceText.Add("Base: All Shoppers", "Base: " + reportparams.ShopperFrequencyShortName + " Shopper");
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");


            metriclist = new List<string>() { "Shopper Frequency1" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Cross Retailer Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Shopper Frequency2", 10, 2)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("<filter> Shoppers", "" + reportparams.ShopperFrequencyShortName + " Shoppers");
            slide.ReplaceText.Add("Weekly+ Shoppers ", reportparams.ShopperFrequencyShortName + " Shoppers ");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slide.ReplaceText.Add("% of <<Freq>>Shoppers by Top 10 Retailers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " by Top 10 Retailers");
            slide.ReplaceText.Add("Cross Shopping Behavior of Weekly+ Shoppers", "Cross Retailer Shopping Behavior of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            slidelist.Add(slide);
            //
            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Cross Retailer";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }   
        private FileDetails Build_FREQUENTSHOPPER_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\7_140922overtime overview_2 3MMT_Frequent Shopper_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Frequent Shopper");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Weekly+ Shoppers", reportparams.ShopperFrequencyShortName + " Shoppers");          

            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");


            metriclist = new List<string> { "ReasonForStoreChoice0", "DestinationItemDetails" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "FactAgeGroups", 5, 2)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //  
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Gender", 5, 1)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //      
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Ethnicity", 5, 3)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //   
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHIncomeGroups", 5, 4)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //     
            slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "MaritalStatus", 6, 1)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //  
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHTotal", 6, 2)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //      
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHAdults", 6, 3)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //   
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHChildren", 6, 4)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //     
            slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //
            //Slide 5
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Shoppers");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Attitudinal Segment", 7, 1)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ?  Convert.ToString(tbl.Rows[0]["Metric"]) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
        
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //           
            slide.ReplaceText.Add("Attitudinal Segment of <filter> Shoppers", "Attitudinal Segment of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //          

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Frequent Shopper";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_TRIPSUMMARY_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\6_140922overtime overview_2 3MMT_Trip Summary_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Summary");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");


            metriclist = new List<string> { "ReasonForStoreChoice0", "DestinationItemDetails" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "TimeSpent",31, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //  
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "TripExpenditure", 31, 2)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //      
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "ItemsPurchased", 31, 3)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //      
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));

            slidelist.Add(slide);
            //

            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "PaymentMode", 32, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //
            //Slide 5
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "RedeemedCouponTypes", 33, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 6
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "DestinationStoreTrip", 34, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 7
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "TripSatisfaction", 35, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
            
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //          

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_INSTORE_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\5_140922overtime overview_2 3MMT_In store_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".In Store");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");


            metriclist = new List<string>() { "InStoreDestinationDetails Top 10", "Top 10 Impulse Items" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Smartphone/TabletInfluencedPurchases",20, 1));  //"Smartphone/Tablet Influenced Purchases?");
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "ItemsPurchasedSummary",21, 1));  //"Items Purchased Summary");
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
            
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //          

            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //
            //Slide 5
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Items Purchased Detail Top 10", 22, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 6
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Impulse Item Top 10", 23, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 7
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "BeveragepurchasedMonthly", 24, 1)); //"Beverage purchased Monthly");
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //
            //List<string> top10metrics = new List<string>();
            //var query = from r in tbl.AsEnumerable()
            //            select r.Field<string>("MetricItem");
            //top10metrics = query.Distinct().ToList();
            //List<string> metrictitels = new List<string>() { "Top 10 Regular Carbonated Soft Drinks Brands Purchased",
            //                                                 "Top 10 Diet Carbonated Soft Drinks Brands Purchased",
            //                                                 "Top 10 Non-Sparkling Water Brands Purchased",
            //                                                 "Top 10 RTD Juice Brands Purchased","Top 10 100% Juice Brands Purchased",
            //                                                 "Top 10 100% Orange Juice Brands Purchased","Top 10 Tea Brands Purchased",
            //                                                 "Top 10 Coffee Brands Purchased","Top 10 Sports Drinks Brands Purchased",
            //                                                 "Top 10 Energy Drinks Brands Purchased"};

            //Slide 8
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "CSD Regular/CSD Diet", 25, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
            
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "CSD Regular/CSD Diet", 26, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";

            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            DataSet tempdst = new DataSet();
            List<object> appendixcolumnlist = new List<object>();
            List<string> tablemetriclist = new List<string>();
            DataTable dsttop10 = Get_Chart_Table(ds, "BeveragepurchasedMonthly",27, 1);  //"Beverage purchased Monthly");
            List<string> top5 = null;
            if (dsttop10 != null && dsttop10.Rows.Count > 0)
            {
                var query3 = from r in dsttop10.AsEnumerable()                           
                             select Convert.ToString(r.Field<object>("Metric"));
                top5 = query3.Distinct().ToList();
            }
            if (top5 != null && top5.Count > 0)
            {
                for (int i = 0; i <= 3; i++)
                {
                    tablemetriclist.Add(Convert.ToString(top5[i]));
                }
            }
            foreach (string metric in tablemetriclist)
            {
                tbl = new DataTable();
                //tbl = Get_Chart_Table(ds, metric);
                tbl = base.GetMetricData(metric, dsttop10);
                DataTable atbl = CreateAppendixTablePreSHOP(tbl);
                atbl.TableName = metric;
                tempdst.Tables.Add(atbl.Copy());
            }
            if (dsttop10 != null && dsttop10.Rows.Count > 0)
            {
                var query2 = from r in dsttop10.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query2.Distinct().ToList();
            }
            columnwidth = new List<string>();
            for (int i = 0; i < appendixcolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            rowheight = "127923";
            fontsize = 1000;
            UpdateAppendixMultipleTables(xmlpath, tempdst, appendixcolumnlist, "Table 11", rowheight.ToString(), columnwidth, "");

            slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            tempdst = new DataSet();
            appendixcolumnlist = new List<object>();
            tablemetriclist = new List<string>();
            dsttop10 = Get_Chart_Table(ds, "BeveragepurchasedMonthly", 28, 1); //"Beverage purchased Monthly");
            top5 = null;
            if (dsttop10 != null && dsttop10.Rows.Count > 0)
            {
                var query3 = from r in dsttop10.AsEnumerable()
                             select Convert.ToString(r.Field<object>("Metric"));
                top5 = query3.Distinct().ToList();
            }
            if (top5 != null && top5.Count > 0)
            {
                for (int i = 0; i <= 3; i++)
                {
                    tablemetriclist.Add(Convert.ToString(top5[i]));
                }
            }
            foreach (string metric in tablemetriclist)
            {
                tbl = new DataTable();
                //tbl = Get_Chart_Table(ds, metric);
                tbl = base.GetMetricData(metric, dsttop10);
                DataTable atbl = CreateAppendixTablePreSHOP(tbl);
                atbl.TableName = metric;
                tempdst.Tables.Add(atbl.Copy());
            }                       
            if (dsttop10 != null && dsttop10.Rows.Count > 0)
            {
                var query4 = from r in dsttop10.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query4.Distinct().ToList();
            }
            columnwidth = new List<string>();
            for (int i = 0; i < appendixcolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            rowheight = "87923";
            fontsize = 1000;
            UpdateAppendixMultipleTables(xmlpath, tempdst, appendixcolumnlist, "Table 11", rowheight.ToString(), columnwidth, "");

            slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //int slideno = 8;
            //int chartno = 6;
            //for (int i = 0; i < top10metrics.Count;i++)
            //{
            //    //Slide 8
            //    slide = new SlideDetails();
            //    slide.SlideNumber = slideno;
            //    slide.ReplaceText = GetSourceDetail("Trips");
            //    //chart Age
            //    chart = new ChartDetails();
            //    chart.Type = ChartType.LINE;
            //    chart.ShowDataLegends = true;
            //    chart.DataLabelFormatCode = "0.0%";
            //    chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + chartno + ".xml");
            //    chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //    tbl = Get_Chart_Table(ds, top10metrics[i]);
            //    chart.Data = tbl;
            //    chart.XAxisColumnName = "MetricItem";
            //    chart.YAxisColumnName = "Volume";
            //    chart.MetricColumnName = "Objective";
            //    chart.ColorColumnName = "Significance";
            //    lststatcolour = new List<CalculationColor>(){
            //            new CalculationColor(){Compare = CalculateEnum.GREATERTHAN,CompareValue = accuratestatvalueposi, ColorCode="#00B050"},
            //            new CalculationColor(){Compare = CalculateEnum.LESSERTHAN,CompareValue = accuratestatvaluenega, ColorCode="#FF0000"},
            //            new CalculationColor(){Compare = CalculateEnum.GREATEREQUALANDLESSEREQUAL,CompareValue = accuratestatvalueposi, ColorCode="#000000"}
            //             };
            //    chart.TextColor = lststatcolour;
            //    slide.Charts.Add(chart);
            //    //           
            //    slide.ReplaceText.Add(metrictitels[i], "Top 10 " + top10metrics[i] + " Brands Purchased");
            //    slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));

            //    slidelist.Add(slide);
            //    slideno += 1;
            //    chartno += 1;
            //    //
            //}

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".In Store";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_PRESHOP_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\4_140922overtime overview_2 3MMT_Before Visit_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");


            metriclist = new List<string> { "ReasonForStoreChoice0" }; //"Destination Item Detail" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "PreTripOrigin",12, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "DayofWeek",13, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Gender
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "WeekdayNet", 13, 2)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Ethnicity
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "DayParts", 13, 3)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
         
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //

            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //
            //Slide 5
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "VisitPlans", 14, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
            
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 6
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "VisitMotiviations", 15, 1)); //"Visit Motiviations");
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //Slide 7
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "ReasonForStoreChoice Top 10", 16, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            ////Slide 9
            //slide = new SlideDetails();
            ////slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText = GetSourceDetail("Trips");

            //tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10", 17, 1);
            //var query2 = from r in tbl.AsEnumerable()
            //             select Convert.ToString(r.Field<object>("Objective")).ToUpper();
            //List<string> appendixcolumnlist = query2.Distinct().ToList();
            //tbl = CreateAppendixTable(tbl);
            //GetTableHeight_FontSize(tbl);
            //columnwidth = new List<string>();
            //for (int i = 0; i < appendixcolumnlist.Count; i++)
            //{
            //    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            //}
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTrendAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "DestinationItemDetails Top 10",17, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
          
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_TRIPTYPE_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\3_140922overtime overview_2 3MMT_Type of Trip_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Type");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");


            metriclist = new List<string>();
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "TripMission",9, 1)); 
            chart.Data = tbl;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
         
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //            
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));
            slidelist.Add(slide);
            //
            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Type";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        private FileDetails Build_VISITORPROFILE_Slides(DataSet ds)
        {
            CalculationColor color = new CalculationColor();
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            string tempdestfilepath, destpath;
            string[] destinationFilePath;
            DataTable tbl = null;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\OverTimeReportGeneratorPPTFiles\2_140922_overtime overview_2 3MMT_Visitor Profile_V0.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Visitor Profile");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];
            //Slide 1
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText.Add(" Channel/Retailer: Family Dollar", " Channel/Retailer: " + cf.cleanPPTXML(Get_ShortNames(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) + (CommonFunctions.Channellist.Contains(Convert.ToString(reportparams.ShopperSegment.Replace("RetailerNet|", "").Replace("Channels|", "").Replace("Retailers|", ""))) ? " Channel" : string.Empty));
            slide.ReplaceText.Add("Time Period: Oct 2013 3MMT to Nov 2013 3MMT", "Time Period: " + reportparams.ShortTimePeriod);
            slide.ReplaceText.Add("Filters: None", "Filters: " + reportparams.FilterShortNames);
            slidelist.Add(slide);
            //Slide 2
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");


            metriclist = new List<string>();
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number).ToString() + ".xml");
            columnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < columnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / columnlist.Count));
            }
            UpdateSummaryTable(xmlpath, tbl, columnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slidelist.Add(slide);
            //Slide 3
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "FactAgeGroups", 5, 2)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Gender
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Gender",5, 1)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Ethnicity
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "Ethnicity", 5, 3)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
         
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Ethnicity
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHIncomeGroups", 5, 4)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));

            slidelist.Add(slide);
            //


            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips");
            //chart Age
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "MaritalStatus", 6, 1)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Gender
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHTotal", 6, 2)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Ethnicity
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHAdults", 6, 3)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            //chart Ethnicity
            chart = new ChartDetails();
            chart.Type = ChartType.LINE;
            chart.ShowDataLegends = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            tbl =  FilterDataTable(Get_Chart_Table(ds, "HHChildren", 6, 4)); 
            chart.Data = tbl;
            chart.Title = tbl.Rows.Count > 0 ? Get_ShortNames(Convert.ToString(tbl.Rows[0]["Metric"]).Trim()) : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "Objective";
            chart.ColorColumnName = "Significance";
           
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //
            slide.ReplaceText.Add("OCT 2013 3MMT (300), NOV 2013 3MMT (450)", Get_SampleSize(tbl));

            slidelist.Add(slide);
            //

            //add slides to file
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Visitor Profile";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
     
    }
}