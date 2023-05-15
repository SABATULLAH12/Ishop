using Aspose.Slides;
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
    public class Download_ShopperReportFor_2Retailers : BaseReport
    {
        public override List<FileDetails> BuildSlides()
        {
          
            StatTesting = Convert.ToDouble(Session["PercentStat"]);
            accuratestatvalueposi = Convert.ToDouble(Session["StatSessionPosi"]);
            accuratestatvaluenega = Convert.ToDouble(Session["StatSessionNega"]);
            reportparams = HttpContext.Current.Session["GenerateReportParams"] as ReportGeneratorParams;
            List<FileDetails> filelist = null;
            FileDetails _fileDetails = null;
            List<string> lstHeaderText = new List<string>();

            string tempcomp;
            string[] complist;
            tempcomp = reportparams.Comparisonlist;
            complist = tempcomp.Split('|');
            try
            {

                if (complist.Length > 7)
                {
                    chkComparisonFolderNumber = "5";
                }
                else if (complist.Length > 5 && complist.Length < 7)
                {
                    chkComparisonFolderNumber = "4";
                }
                else if (complist.Length > 3 && complist.Length < 5)
                {
                    chkComparisonFolderNumber = "3";
                }
                List<String> preferences = null;
                if (reportparams.ModuleBlock.Equals("AcrossShopper", StringComparison.OrdinalIgnoreCase))
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
                    DataSet ds = null;
                    List<string> objectivelist = new List<string>();
                    foreach (string key in reportparams.SelectedReports)
                    {
                        ReportNumber = serialno[key];
                        if (Convert.ToString(key).Equals("Summary", StringComparison.OrdinalIgnoreCase))
                        {
                            if (reportparams.Comparisonlist.Split('|').Count() == 2)
                            {
                                _fileDetails = Build_1_Comparison_Summary_Slides();
                            }
                            else
                            {
                                _fileDetails = Build_2_Comparison_Summary_Slides();
                            }

                            if (_fileDetails != null)
                            {
                                _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                //filelist.Add(_fileDetails);
                            }
                        }
                        else
                        {
                            ds = null;
                            if (reportparams.ChartDataSet.ContainsKey(key.ToString()))
                            {
                                ds = FormatAppendixTable(reportparams.ChartDataSet[key.ToString()]);
                            }

                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                if (complist.Count() == 4 || complist.Count() == 6 || complist.Count() == 8)
                                {
                                    if (reportparams.SelectedReports.Contains("Pre", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("In Store", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Summary", StringComparer.CurrentCultureIgnoreCase)
                                        && reportparams.SelectedReports.Contains("Shopper", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Retailer", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Perception", StringComparer.CurrentCultureIgnoreCase)
                                        && reportparams.SelectedReports.Contains("Intration", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Appendix", StringComparer.CurrentCultureIgnoreCase))
                                    {
                                        //_fileDetails = Build_1_Comparison_ISHOP_Slides(ds); 
                                        if (_fileDetails != null)
                                        {
                                            _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                            //filelist.Add(_fileDetails);
                                        }
                                    }
                                    else
                                    {
                                        //foreach (string strkey in reportparams.SelectedReports)
                                        //{
                                        switch (key)
                                        {
                                            case "VISITORPROFILE":
                                                {
                                                    _fileDetails = Build_2_Comparison_Visitor_Profile_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "TRIPTYPE":
                                                {
                                                    _fileDetails = Build_2_Comparison_Trip_Type_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "PRESHOP":
                                                {
                                                    if (chkComparisonFolderNumber == "3")
                                                    {
                                                        _fileDetails = Build_2_Comparison_PreShop_Slides(ds, chkComparisonFolderNumber);
                                                    }
                                                    else if (chkComparisonFolderNumber == "4")
                                                    {
                                                        _fileDetails = Build_3_Comparison_PreShop_Slides(ds, chkComparisonFolderNumber);
                                                    }
                                                    else if (chkComparisonFolderNumber == "5")
                                                    {
                                                        _fileDetails = Build_4_Comparison_PreShop_Slides(ds, chkComparisonFolderNumber);
                                                    }
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "INSTORE":
                                                {
                                                    _fileDetails = Build_2_Comparison_In_Store_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "TRIPSUMMARY":
                                                {
                                                    _fileDetails = Build_2_Comparison_Trip_Summary_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "FREQUENTSHOPPER":
                                                {
                                                    _fileDetails = Build_2_Comparison_Shopper_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "CROSSRETAILERSHOPPER":
                                                {
                                                    _fileDetails = Build_2_Comparison_Retailer_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "SHOPPERPERCEPTION":
                                                {
                                                    if (chkComparisonFolderNumber == "3")
                                                    {
                                                        _fileDetails = Build_2_Comparison_Perception_Slides(ds, chkComparisonFolderNumber);
                                                    }
                                                    else if (chkComparisonFolderNumber == "4")
                                                    {
                                                        _fileDetails = Build_3_Comparison_Perception_Slides(ds, chkComparisonFolderNumber);
                                                    }
                                                    else if (chkComparisonFolderNumber == "5")
                                                    {
                                                        _fileDetails = Build_4_Comparison_Perception_Slides(ds, chkComparisonFolderNumber);
                                                    }
                                                    //_fileDetails = Build_2_Comparison_Perception_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "BEVERAGEINTERACTION":
                                                {
                                                    _fileDetails = Build_2_Comparison_Beverage_Interaction_Slides(ds, chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "APPENDIX":
                                                {
                                                    _fileDetails = Build_2_Comparison_Appendix_Slides(FormatAppendixTable(ds), chkComparisonFolderNumber);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                        }
                                        //}
                                    }
                                }
                                else if (complist.Count() == 2) //This if code executes when Selector is 1 and comparator is 1. (i.e. 1 to 1)
                                {
                                    if (reportparams.SelectedReports.Contains("Pre", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("In Store", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Summary", StringComparer.CurrentCultureIgnoreCase)
                                        && reportparams.SelectedReports.Contains("Shopper", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Retailer", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Perception", StringComparer.CurrentCultureIgnoreCase)
                                        && reportparams.SelectedReports.Contains("Intration", StringComparer.CurrentCultureIgnoreCase) && reportparams.SelectedReports.Contains("Appendix", StringComparer.CurrentCultureIgnoreCase))
                                    {
                                        //_fileDetails = Build_2_Comparison_ISHOP_Slides();
                                        if (_fileDetails != null)
                                        {
                                            //filelist.Add(_fileDetails);
                                        }
                                    }
                                    else
                                    {
                                        //foreach (string strkey in reportparams.SelectedReports)
                                        //{
                                        switch (key)
                                        {
                                            case "VISITORPROFILE":
                                                {
                                                    _fileDetails = Build_1_Comparison_Visitor_Profile_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "TRIPTYPE":
                                                {
                                                    _fileDetails = Build_1_Comparison_Trip_Type_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "PRESHOP":
                                                {
                                                    _fileDetails = Build_1_Comparison_PreShop_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "INSTORE":
                                                {
                                                    _fileDetails = Build_1_Comparison_In_Store_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "TRIPSUMMARY":
                                                {
                                                    _fileDetails = Build_1_Comparison_Trip_Summary_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "FREQUENTSHOPPER":
                                                {
                                                    _fileDetails = Build_1_Comparison_Shopper_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "CROSSRETAILERSHOPPER":
                                                {
                                                    _fileDetails = Build_1_Comparison_Retailer_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "SHOPPERPERCEPTION":
                                                {
                                                    _fileDetails = Build_1_Comparison_Perception_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "BEVERAGEINTERACTION":
                                                {
                                                    _fileDetails = Build_1_Comparison_Beverage_Interaction_Slides(ds);
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                            case "APPENDIX":
                                                {
                                                    _fileDetails = Build_1_Comparison_Appendix_Slides(FormatAppendixTable(ds));
                                                    if (_fileDetails != null)
                                                    {
                                                        _fileDetails.PlaceUnderFolderPath = "Individual reports";
                                                        //filelist.Add(_fileDetails);
                                                    }
                                                    break;
                                                }
                                        }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
                filelist.Add(_fileDetails);
            }
            catch(Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return filelist;
        }
        #region 1 Comparison Slides
        #region 1 Comparison Summary Slides
        private FileDetails Build_1_Comparison_Summary_Slides()
        {
            string tempdestfilepath, Benchlist1, complist1, compchannel0, BenchChannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            List<string> strRetailersList = new List<string>();
            strRetailersList.Add(Benchlist[1]);
            strRetailersList.Add(complist[1]);

            string[] destinationFilePath;
            if (reportparams.ModuleBlock.Equals("AcrossShopper", StringComparison.OrdinalIgnoreCase))
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Across Shoppers\iShop RG New - 2 Retailer_Report_V5.1");
            else
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Compare Path Purchase\iShop RG New - 2 Retailer_Report_V5.1");

            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Retailer Summary");
            destination_FilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];


            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Dollar Tree Vs Family Dollar", Benchlist1 + " Vs " + complist1);
            DataSet dst = new DataSet();
            slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Benchmark: Dollar Tree", "Benchmark: " + Benchlist1 + BenchChannel0);
            slide.ReplaceText.Add("Comparison: Family Dollar", "Comparison: " + complist1 + compchannel0);

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Overall Summary", "Overall Summary ");
            //start- added by bramhanath for New Slide Changes
            slide.ReplaceText.Add("This report compares BENCHMARK Monthly+ Shoppers to the Monthly+ shoppers of the ", "This report compares " + Benchlist1 + BenchChannel0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Shoppers to the " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers to the ") + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "shoppers of the " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of the "));
            slide.ReplaceText.Add("Monthly+ ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName).Replace("NONE", "")) + " ");

            slide.ReplaceText.Add("Monthly+ Frequent ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent "));
            slide.ReplaceText.Add("COMPARISON1", complist1 + compchannel0);
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as a benchmark (shown in Gray)", "All statistical testing is compared against " + Benchlist1 + BenchChannel0 + " as a benchmark (shown in Gray)");
            slide.ReplaceText.Add("Monthly+ Frequent Shopper’s for to ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s for " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s for "));
            slide.ReplaceText.Add("Monthly+ Frequent Shopper’s for ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s for " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s for "));
            slide.ReplaceText.Add("COMPARISON1 ", complist1 + compchannel0 + " ");
            slide.ReplaceText.Add("BENCHMARK and COMPARISON1 ", Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " ");
            slide.ReplaceText.Add("BENCHMARK and COMPARISON1", Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0);
            slide.ReplaceText.Add("BENCHMARK ", Benchlist1 + BenchChannel0 + " ");
            slide.ReplaceText.Add("BENCHMARK and ", Benchlist1 + BenchChannel0 + " and ");
            slide.ReplaceText.Add("COMPARISON1(Blue) ", complist1 + compchannel0 + "(Blue) ");
            slide.ReplaceText.Add("95% ", StatTesting + "% ");
            //end
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            files.ReplaceImages = AddRetailerImages(strRetailersList);
            fileName = ReportNumber + ".Retailer Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Visitor Profile Slides
        private FileDetails Build_1_Comparison_Visitor_Profile_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\2_iShop RG New - 2 Retailer _Visitor Profile_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Visitor Profile");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1 );
            //slide.ReplaceText.Add("Comparisons: Dollar General ", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Gender");
            lstMetricNames.Add("FactAgeGroups");
            lstMetricNames.Add("Ethnicity");
            lstMetricNames.Add("MaritalStatus");
            lstMetricNames.Add("HHIncomeGroups");
            lstMetricNames.Add("HHAdults");
            lstMetricNames.Add("HHChildren");
            lstMetricNames.Add("HHTotal");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("474124");
            lstSize.Add("16");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Demographic Segments");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));

            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips",be);
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Gender", "1",5,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "FactAgeGroups", "1", 5, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Ethnicity", "1", 5, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHIncomeGroups", "1", 5, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MaritalStatus", "1", 6, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHTotal", "1", 6, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHAdults", "1", 6, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHChildren", "1", 6, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Visitor Profile";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Trip Type Slides
        private FileDetails Build_1_Comparison_Trip_Type_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\3_iShop RG New - 2 Retailer _Trip type_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Type");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General ", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("TripMission");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("544124");
            lstSize.Add("20");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Trip Mission Summary");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripMission", "1",9,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Type";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison PreShop Slides
        private FileDetails Build_1_Comparison_PreShop_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\4_iShop RG New - 2 Retailer _Pre Shop_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataTable tbl = new DataTable();
            List<Color> colorlist = new List<Color>();
            List<object> columnlist = new List<object>();
            string strFilter = "";
            List<object> appendixcolumnlist = new List<object>();
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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General ", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();
            slide.ReplaceText.Add("Summary : Pre-Shop of trip takers", "Summary : Pre-Shop of trip takers ");

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("PreTripOrigin");
            lstMetricNames.Add("DayofWeek");
            lstMetricNames.Add("WeekdayNet");
            lstMetricNames.Add("DayParts");
            lstMetricNames.Add("VisitPreparation");
            lstMetricNames.Add("VisitMotiviations");
            lstMetricNames.Add("ReasonForStoreChoice Top 10");
            lstMetricNames.Add("InStoreDestinationDetails Top 10");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("795922");
            lstSize.Add("543954");
            lstSize.Add("10");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key Pre-Shop Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);

            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "ReasonForStoreChoice0", "ReasonForStoreChoice1", "Destination Item Detail" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "PreTripOrigin", "1",12,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayofWeek", "1", 13, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayParts", "1", 13, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "WeekdayNet", "1", 13, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPreparation", "1", 14, 2));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 5;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1",14,1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 6;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitMotiviations", "1",15,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);

            tbl = Get_Chart_Table(ds, "ReasonForStoreChoice Top 10",16,1);
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 39", rowheight.ToString(), columnwidth, "Reasons for Store Choice");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1",17,1), "GapAnalysis");

            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1",17,1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, geods);
            slide.ReplaceText.Add("Dollar ", complist1 + compchannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar1", Benchlist1);
            slide.ReplaceText.Add("Family2", complist1);
            slide.ReplaceText.Add("Comparision1 Leads", complist1 + compchannel0 + " Leads");
            slide.ReplaceText.Add("Benchmark1 Leads", Benchlist1 + BenchChannel0 + " Leads");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            ////Slide 9
            //slide = new SlideDetails();
            ////slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);

            //tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10",18,1);
            //var query2 = from r in tbl.AsEnumerable()
            //             select r.Field<object>("Objective");
            //appendixcolumnlist = query2.Distinct().ToList();
            //tbl = CreateAppendixTable(tbl);
            //GetTableHeight_FontSize(tbl);
            //columnwidth = new List<string>();
            //for (int i = 0; i < appendixcolumnlist.Count; i++)
            //{
            //    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            //}
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1",18,1,true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Absolute Difference with Family Dollar: Destination Items", "Absolute Difference with " + Benchlist1 + ": Destination Items");
            slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison In Store Slides
        private FileDetails Build_1_Comparison_In_Store_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\5_iShop RG New - 2 Retailer_In Store_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".In Store");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();
            Dictionary<string, object> tablecolumnlist = new Dictionary<string, object>();
            List<string> columnlist = new List<string>();
            DataTable tbl = new DataTable();
            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("What they do in the store?", "What they do in the store? ");
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General ", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("ItemsPurchasedSummary");
            lstMetricNames.Add("MostImportantDestinationItems Top 10");
            lstMetricNames.Add("ImpulseItem Top 10");
            lstMetricNames.Add("BeveragepurchasedMonthly");

            dst = GetSlideTables(ds, "BeveragepurchasedMonthly", "1");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    lstMetricNames.Add(Convert.ToString(dst.Tables[0].Rows[i]["MetricItem"]));
                }

            }

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("353901");
            lstSize.Add("285802");
            lstSize.Add("10");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key In-Store Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));

            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);

            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "InStoreDestinationDetails Top 10", "Top 10 Impulse Items" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 18;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Smartphone/TabletInfluencedPurchases", "1",21,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            //chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 18;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "ItemsPurchasedSummary", "1", 22, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Trip Net", "Items Purchased on Trip (Net)");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide5.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "InStoreDestinationDetails", "1", 23, 1,true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Absolute Difference with Family Dollar: Items Purchased", "Absolute Difference with " + Benchlist1 + ": Items Purchased");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            ////Slide 7 
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide7.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //tablecolumnlist = new Dictionary<string, object>();
            //columnlist = new List<string>() { " ", " " };
            //tablecolumnlist.Add("Reason for store choice", Benchlist[1]);
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide7.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "ImpulseItem", "1", 24, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Absolute Difference with Family Dollar: Impulse Items", "Absolute Difference with " + Benchlist1 + ": Impulse Items");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            DataSet tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "BeveragepurchasedMonthly", "1", 25, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "CSD Regular/CSD Diet", "1", 26, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "CSD Regular/CSD Diet", "1", 27, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            tempdst = new DataSet();
            List<object> appendixcolumnlist = new List<object>();
            List<string> tablemetriclist = new List<string>();
            DataSet dsttop10 = GetSlideTables(ds, "BeveragepurchasedMonthly", "1",28,1);
            List<string> top5 = null;
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query3 = from r in dsttop10.Tables[0].AsEnumerable()                           
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
                //tbl = Get_Chart_Table(ds, metric,29,1);
                tbl = base.GetMetricData(metric, dsttop10.Tables[0],true);
                DataTable atbl = CreateAppendixTablePreSHOP(tbl);
                atbl.TableName = metric;
                tempdst.Tables.Add(atbl.Copy());
            }
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query = from r in dsttop10.Tables[0].AsEnumerable()
                            select r.Field<object>("Objective");
                appendixcolumnlist = query.Distinct().ToList();
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

            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            tempdst = new DataSet();
            appendixcolumnlist = new List<object>();
            tablemetriclist = new List<string>();
            dsttop10 = GetSlideTables(ds, "BeveragepurchasedMonthly", "1", 29, 1);
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query3 = from r in dsttop10.Tables[0].AsEnumerable()
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
                //tbl = Get_Chart_Table(ds, metric,30,1);
                tbl = base.GetMetricData(metric, dsttop10.Tables[0],true);
                DataTable atbl = CreateAppendixTablePreSHOP(tbl);
                atbl.TableName = metric;
                tempdst.Tables.Add(atbl.Copy());
            }
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query2 = from r in dsttop10.Tables[0].AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query2.Distinct().ToList();
            }

            columnwidth = new List<string>();
            for (int i = 0; i < appendixcolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            rowheight = "87923";
            fontsize = 1000;
            UpdateAppendixMultipleTables(xmlpath, tempdst, appendixcolumnlist, "Table 13", rowheight.ToString(), columnwidth, "");

            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Below is for the remaining slides (11 to 10). Data for these slides gets change based on the selection and top 10 Metric items.
            //int j = 10;
            //if (dst != null && dst.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        tempdst = new DataSet();
            //        tempdst = FilterData(GetSlideTables(ds, Convert.ToString(dst.Tables[0].Rows[i]["MetricItem"]), "1"));

            //        slide = new SlideDetails();
            //        chart = new ChartDetails();
            //        chart.Type = ChartType.BAR;
            //        chart.ShowDataLegends = false;
            //        chart.DataLabelFormatCode = "0.0%";
            //        chart.ChartXmlPath = chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //        chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //        chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(tempdst));
            //        chart.Title = Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Trim();
            //        chart.XAxisColumnName = "Objective";
            //        chart.YAxisColumnName = "Volume";
            //        chart.MetricColumnName = "MetricItem";
            //        chart.ColorColumnName = "Significance";
            //        chart.TextColor = lststatcolour;
            //        slide.Charts.Add(chart);
            //        slide.ReplaceText = GetSourceDetail("Trips");
            //        slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(tempdst.Tables[0], Benchlist1) + ")");
            //        slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(tempdst.Tables[0], complist1) + ")");
            //        slide.SlideNumber = i + 10;

            //        if (i == 0)
            //        {
            //            slide.ReplaceText.Add("Top 10 Regular Carbonated Soft ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Drinks Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 1)
            //        {
            //            slide.ReplaceText.Add("Diet ", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">"));
            //            slide.ReplaceText.Add("Carbonated Soft ", " ");
            //            slide.ReplaceText.Add("Drinks Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 2)
            //        {
            //            slide.ReplaceText.Add("Top 10 Non-Sparkling ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Water Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 3)
            //        {
            //            slide.ReplaceText.Add("Top 10 RTD ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Juice Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 4)
            //        {
            //            slide.ReplaceText.Add("Top 10 100% ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Juice Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 5)
            //        {
            //            slide.ReplaceText.Add("Top 10 100% Orange ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">"));
            //            slide.ReplaceText.Add("Juice Brands Purchased", " Brands Purchased");
            //        }

            //        if (i == 6)
            //        {
            //            slide.ReplaceText.Add("Tea Brands Purchased", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased");
            //        }

            //        if (i == 7)
            //        {
            //            slide.ReplaceText.Add("Coffee Brands Purchased", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased");
            //        }

            //        if (i == 8)
            //        {
            //            slide.ReplaceText.Add("Top 10 Sports ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">"));
            //            slide.ReplaceText.Add("Drinks Brands Purchased", " Brands Purchased");
            //        }

            //        if (i == 9)
            //        {
            //            slide.ReplaceText.Add("Energy Drinks Brands Purchased", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased");
            //        }

            //        slidelist.Add(slide);
            //    }
            //}

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".In Store";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Trip Summary Slides
        private FileDetails Build_1_Comparison_Trip_Summary_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\6_iShop RG New - 2 Retailer _Trip Summary_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Summary");
            //destination_FilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            Dictionary<string, string> DicOvalData = new Dictionary<string, string>();
            DataTable tbl = new DataTable();
            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General ", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("TimeSpent");
            lstMetricNames.Add("TripExpenditure");
            lstMetricNames.Add("ItemsPurchased");
            lstMetricNames.Add("PaymentMode");
            lstMetricNames.Add("RedeemedCouponTypes");
            lstMetricNames.Add("DestinationStoreTrip");
            lstMetricNames.Add("TripSatisfaction");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("584124");
            lstSize.Add("14");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key Post Shop Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);

            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //

            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TimeSpent", "1",32,1));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            dst= FilterData(GetSlideTables(ds, "Average Time Spent (In HH:MM)", "1", 32, 4));
            timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
            hh = timeSpan.Hours;
            mm = timeSpan.Minutes;
            DicOvalData.Add("Oval 2", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

            timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
            hh = timeSpan.Hours;
            mm = timeSpan.Minutes;
            DicOvalData.Add("Oval 35", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripExpenditure", "1", 32, 2));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            dst = FilterData(GetSlideTables(ds, "Average Trip Expenditure (In $)", "1", 32, 5));
            DicOvalData.Add("Oval 43", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
            DicOvalData.Add("Oval 44", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "ItemsPurchased", "1", 32, 3));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            dst = FilterData(GetSlideTables(ds, "Average Items Purchased (In Items)", "1", 32, 6));
            DicOvalData.Add("Oval 46", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
            DicOvalData.Add("Oval 47", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));

            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateOvalData(xmlpath, DicOvalData);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();

            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "PaymentMode", "1", 33, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "RedeemedCouponTypes", "1", 33, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationStoreTrip", "1", 34, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripSatisfaction", "1", 35, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);


            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Frequent Shopper Slides (Frequest Shopper)
        private FileDetails Build_1_Comparison_Shopper_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\7_iShop RG New - 2 Retailer_Frequent shopper_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Frequent Shopper");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General ", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Gender");
            lstMetricNames.Add("FactAgeGroups");
            lstMetricNames.Add("Ethnicity");
            lstMetricNames.Add("MaritalStatus");
            lstMetricNames.Add("HHIncomeGroups");
            lstMetricNames.Add("HHAdults");
            lstMetricNames.Add("HHChildren");
            lstMetricNames.Add("HHTotal");
            lstMetricNames.Add("Attitudinal Segment");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("633257");
            lstSize.Add("505726");
            lstSize.Add("14");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Demographic Segments");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);

            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();

            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Gender", "1", 5, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "FactAgeGroups", "1", 5, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Ethnicity", "1", 5, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHIncomeGroups", "1", 5, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MaritalStatus", "1", 6, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHTotal", "1", 6, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHAdults", "1", 6, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHChildren", "1", 6, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Attitudinal Segment", "1", 7, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Attitudinal Segment of <filter> Shoppers", "Attitudinal Segment of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Frequent Shopper";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Cross Retailer Slides(8_iShop RG New - 2 Retailer_Cross Retailer Shoppers_V5.1)
        private FileDetails Build_1_Comparison_Retailer_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\8_iShop RG New - 2 Retailer_Cross Retailer Shoppers_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Cross Retailer");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            DataSet dstemp = ds.Copy();
            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();
            DataTable tbl = new DataTable();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: All Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Shopper Frequency2");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("621740");
            lstSize.Add("445060");
            lstSize.Add("20");

            lstHeaderText = new List<string>();
            lstHeaderText.Add(Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "Shopper Frequency1" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Cross Retailer Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Shoppers");

            tbl = new DataTable();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dstTemp = FilterData(GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2));
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dstTemp);
            chart.Data = CleanXMLBeforeBind(dstTemp.Tables[0]);
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            //chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            dst = GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "Retailer");

            dstemp = GetSlideTables(ds, "Shopper Frequency1", "1", 10, 1);

            slide.ReplaceText.Add("14%", Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["Volume"]), 1)) + "%");
            slide.ReplaceText.Add("10%", Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[1]["Volume"]), 1)) + "%");

            slide.ReplaceText.Add("Monthly + Shoppers of Benchmark", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(Benchlist1) + BenchChannel0);
            slide.ReplaceText.Add("Monthly + Shoppers of Comparison1", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist1) + compchannel0);
            dstemp = GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2);
            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), Benchlist[1]);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                slide.ReplaceText.Add("Family Dollar : 48%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers of " + Benchlist1 + ": " + (tbl.Rows.Count > 0 ? (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%" : string.Empty));
            }
            slide.ReplaceText.Add("Cross Shopping Behavior of Weekly+ Shoppers", "Cross Retailer Shopping Behavior of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            slide.ReplaceText.Add("(Weekly + Shopper of Retailer/Channel) / (Total Shoppers)", "(" + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shopper of Retailer/Channel) / (Total Shoppers)");
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.IsBarHexColorForSeriesPoints = false;
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Objective";
            //chart.YAxisColumnName = "Volume";
            //chart.MetricColumnName = "MetricItem";
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            //chart.BarHexColors = colorlist;
            //slide.Charts.Add(chart);

            UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "Retailer");

            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), complist[1]);
            slide.ReplaceText.Add("Dollar General : 52%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers of " + complist1 + ": " + (tbl.Rows.Count > 0 ? (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%" : string.Empty));
            slide.ReplaceText.Add("<filter> Shoppers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by top 10 retailers");
            slide.ReplaceText.Add("Text1", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            slide.ReplaceText.Add("Text2", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            //slide.ReplaceText.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dstTemp);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");

            slide.ReplaceText.Add("% of Monthly + Shoppers by Top 10 Retailers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by Top 10 Retailers");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Cross Retailer";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Shopper Perception Slides
        private FileDetails Build_1_Comparison_Perception_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\9_iShop RG New - 2 Retailer_Shopper Perceptions_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General ", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dstTemp);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("StoreAttributesFactors");
            lstMetricNames.Add("GoodPlaceToShopFactors");
            lstMetricNames.Add("StoreAttribute Top 10");
            lstMetricNames.Add("GoodPlaceToShop Top 10");
            lstMetricNames.Add("MainFavoriteStore");
            lstMetricNames.Add("RetailerLoyaltyPyramid");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("804269");
            lstSize.Add("626683");
            lstSize.Add("14");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key imagery measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dstTemp);
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "StoreAttributesFactors", "1", 13, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Store Associations of Monthly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide5.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            tbl = Get_Chart_Table(ds, "StoreAttribute Top 10", 14, 1);
            List<object> appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Store Imagery");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "StoreAttribute0", "1", 15, 1));
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText.Add("Family1", Benchlist1);
            slide.ReplaceText.Add("Dollar2", complist1);
            slide.ReplaceText.Add("Comparision1 Leads", complist1 + compchannel0 + " Leads");
            slide.ReplaceText.Add("Benchmark1 Leads", Benchlist1 + BenchChannel0 + " Leads");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "GoodPlaceToShopFactors", "1", 16, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide7.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);
            tbl = Get_Chart_Table(ds, "GoodPlaceToShop Top 10", 17, 1);
            appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Good Place to Shop for");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1", 18, 1));
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText.Add("Family1", Benchlist1);
            slide.ReplaceText.Add("Dollar2", complist1);
            slide.ReplaceText.Add("Comparision1 Leads", complist1 + compchannel0 + " Leads");
            slide.ReplaceText.Add("Benchmark1 Leads", Benchlist1 + BenchChannel0 + " Leads");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MainFavoriteStore", "1", 19, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\PyramidChart_Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "RetailerLoyaltyPyramid", "1", 20, 1));
            DataView dv = dst.Tables[0].Copy().DefaultView;
            //dv.Sort = "Volume desc";
            DataTable sortedDT = dv.ToTable();
            chart.Data = sortedDT;
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Beverage Intration Slides
        private FileDetails Build_1_Comparison_Beverage_Interaction_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;

            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\10_iShop RG New - 2 Retailer_Beverage Interaction_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Beverage Interaction");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General", "Comparisons: " + complist1);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("BeverageConsumedMonthly");
            lstMetricNames.Add("BeveragepurchasedMonthly");

            dst = GetSlideTables(ds, "BeveragepurchasedMonthly", "1");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    lstMetricNames.Add(Convert.ToString(dst.Tables[0].Rows[i]["MetricItem"]));
                }

            }

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("403901");
            lstSize.Add("350901");
            lstSize.Add("10");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Beverages Consumed Monthly");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));

            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "Beverage Consumed Monthly" };
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Beverage Purchase Monthly", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            ////Slide 3
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "BeverageConsumedMonthly", "1"));
            //chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Objective";
            //chart.YAxisColumnName = "Volume";
            //chart.MetricColumnName = "MetricItem";
            //chart.ColorColumnName = "Significance";
            //chart.TextColor = lststatcolour;
            //slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            //slide.ReplaceText.Add("Top 10 categories consumed among  Weekly+ shoppers within Retailer", "Top 10 categories consumed among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "BeveragepurchasedMonthly", "1", 23, 1,true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
            slide.ReplaceText.Add("Top 10 categories purchased among  Weekly+ shoppers within Retailer", "Top 10 categories purchased among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");

            slide.ReplaceText.Add("Top 10 Categories Purchased Among Weekly+ Shoppers", "Top 10 Categories Purchased Among " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");

            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Below is for the remaining slides (11 to 10). Data for these slides gets change based on the selection and top 10 Metric items.
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
            int j = 2;
            DataSet tempdst = new DataSet();
            //List<String> top2 = new List<String>() { "SSD Regular", "SSD Diet" };
            var query3 = from r in dst.Tables[0].AsEnumerable()
                             //where !Convert.ToString(r.Field<object>("MetricItem")).Equals("SSD Regular", StringComparison.OrdinalIgnoreCase)
                             //&& !Convert.ToString(r.Field<object>("MetricItem")).Equals("SSD Diet", StringComparison.OrdinalIgnoreCase)
                         select Convert.ToString(r.Field<object>("MetricItem"));
            List<string> top10 = query3.Distinct().ToList();
            //top10.AddRange(query3.Distinct().ToList());
            int tbl_slide_no = 24;
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    tempdst = new DataSet();
                    tempdst = FilterData(GetSlideTables(ds, Convert.ToString(top10[i]), "1", tbl_slide_no, 1, true));
                    tbl_slide_no++;
                    slide = new SlideDetails();
                    if (tempdst != null && tempdst.Tables.Count > 0)
                    {
                        chart = new ChartDetails();
                        chart.Type = ChartType.BAR;
                        chart.ShowDataLegends = false;
                        chart.DataLabelFormatCode = "0.0%";
                        chart.ChartXmlPath = chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
                        chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
                        chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(tempdst));
                        chart.Title = Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Trim();
                        chart.XAxisColumnName = "Objective";
                        chart.YAxisColumnName = "Volume";
                        chart.MetricColumnName = "MetricItem";
                        chart.ColorColumnName = "Significance";
                        chart.TextColor = lststatcolour;
                        slide.Charts.Add(chart);
                        //slide.ReplaceText = GetSourceDetail("Shoppers");
                        slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, tempdst);
                        slide.ReplaceText.Add("Top 10 Brands among  Weekly+ shoppers within Retailer", "Top 10 Brands among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
                        //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(tempdst.Tables[0], Benchlist1) + ")");
                        //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(tempdst.Tables[0], complist1) + ")");


                        slide.ReplaceText.Add(metrictitels[i], "Top " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased Monthly");
                        slide.ReplaceText.Add("Top Brands Purchased Among Weekly+ Shoppers", "Top Brands Purchased Among  " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
                    }
                    slide.SlideNumber = GetSlideNumber();
                    slidelist.Add(slide);
                }
            }

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Beverage Interaction";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Appendix Slides
        private FileDetails Build_1_Comparison_Appendix_Slides(DataSet ds)
        {
            string tempdestfilepath, Benchlist1, complist1, BenchChannel0, compchannel0;
            string[] complist, filt, Benchlist;

            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            if (Convert.ToString(complist[0]) == "Channels")
            {
                compchannel0 = " Channel";
            }
            else
            {
                compchannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();

            string[] destinationFilePath;
            if (reportparams.ModuleBlock == "AcrossShopper")
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\11_iShop RG New - 2 Retailer_Appendix_V5.1");
            }
            else
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\2-Comparisons\11_iShop RG New - 2 Retailer_Appendix_V5.1 - Trips");
            }
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Appendix");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            Dictionary<string, object> tablecolumnlist = new Dictionary<string, object>();
            List<string> columnlist = new List<string>();
            List<object> appendixcolumnlist = new List<object>();
            DataTable tbl = new DataTable();
            string strFilter = "";

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

            if (reportparams.ModuleBlock == "AcrossShopper")
            {
                //Slide 1
                slide = new SlideDetails();
                //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
                //slide.ReplaceText.Add("Comparisons: Dollar General", "Comparisons: " + complist1);
                //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
                //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
                slide.ReplaceText = GetSourceDetail("", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
                slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                ////slide 2
                //slide = new SlideDetails();
                //slide.SlideNumber = GetSlideNumber();
                ////slide.ReplaceText = GetSourceDetail("Trips");
                //slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);

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
                // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
                //UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 1", rowheight.ToString(), columnwidth, "Reasons for Store Choice");
                //slidelist.Add(slide);
                //slide 3
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Shoppers");
                slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
                tbl = Get_Chart_Table(ds, "StoreAttribute", 35, 1);
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
                //slide 4
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Shoppers");
                slide.ReplaceText = GetSourceDetail("Shoppers", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
                tbl = Get_Chart_Table(ds, "GoodPlaceToShop", 36, 1);
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
            }
            else
            {
                //Slide 1
                slide = new SlideDetails();
                slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1 + BenchChannel0);
                slide.ReplaceText.Add("Comparisons: Dollar General", "Comparisons: " + complist1 + compchannel0);
                slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
                slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: All Trips");

                slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                //slide 2
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Trips");
                slide.ReplaceText = GetSourceDetail("Trips", Benchlist1, complist1, BenchChannel0, compchannel0, dst);
                tbl = Get_Chart_Table(ds, "ReasonForStoreChoice",37,1);
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
                UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 1", rowheight.ToString(), columnwidth, "Reason for Store Choice");
                slidelist.Add(slide);
            }

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Appendix";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #endregion
        #region 2 Comparison Slides

        #region 2 Comparison Summary Slides
        private FileDetails Build_2_Comparison_Summary_Slides()
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string[] complist, filt, Benchlist;
            string chkComparisonFolderNumber = string.Empty;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1]));
            complist1 = Get_ShortNames(Convert.ToString(complist[1]));
            complist3 = Get_ShortNames(Convert.ToString(complist[3]));
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            List<string> strRetailersList = new List<string>();
            strRetailersList.Add(Benchlist[1]);
            strRetailersList.Add(complist[1]);
            strRetailersList.Add(complist[3]);
            if (complist.Length > 7)
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
                chkComparisonFolderNumber = "5";
                strRetailersList.Add(complist[7]);
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
                chkComparisonFolderNumber = "4";
                strRetailersList.Add(complist[5]);
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
                chkComparisonFolderNumber = "3";
            }


            string[] destinationFilePath;
            //Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\1_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Summary_V5.1");
            if (reportparams.ModuleBlock.Equals("AcrossShopper", StringComparison.OrdinalIgnoreCase))
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Across Shoppers\iShop RG New - " + chkComparisonFolderNumber + " Retailer_Report_V5.1");
            else
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Compare Path Purchase\iShop RG New - " + chkComparisonFolderNumber + " Retailer_Report_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Retailer Summary");
            destination_FilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            DataSet ds = new DataSet();

            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Dollar Tree ; 99 Cents Only Store Vs Family Dollar", Benchlist1 + "; " + complist1 + " Vs " + complist3);

            //slide.ReplaceText.Add("Benchmark: Dollar Tree", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5!="")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Family Dollar, Kroger & Publix", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: 99 Cents Only Store & Family Dollar", "Comparisons: " + complist1 + " & " + complist3);
            //}
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Overall Summary", "Overall Summary ");

            //start-added by bramhanath for New Slide Changes
            slide.ReplaceText.Add("This report compares BENCHMARK Monthly+ Shoppers to the Monthly+ shoppers of the ", "This report compares " + Benchlist1 + BenchChannel0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Shoppers to the " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers to the ") + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "shoppers of the " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of the "));
            slide.ReplaceText.Add("This report compares BENCHMARK ", "This report compares " + Benchlist1 + BenchChannel0 + " ");


            slide.ReplaceText.Add("Monthly+ ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName).Replace("NONE", "")) + " ");
            slide.ReplaceText.Add("Monthly+ Frequent Shopper’s for to ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s for " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s for "));
            slide.ReplaceText.Add("Monthly+ Frequent Shopper’s for ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s for " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s for "));
            slide.ReplaceText.Add("BENCHMARK as a benchmark (shown in Gray)", Benchlist1 + BenchChannel0 + " as a benchmark (shown in Gray)");
            slide.ReplaceText.Add("BENCHMARK and not against each other.", Benchlist1 + BenchChannel0 + " and not against each other.");
            slide.ReplaceText.Add("BENCHMARK", Benchlist1 + BenchChannel0);
            slide.ReplaceText.Add("COMPARISON1 (Blue) ", complist1 + compchannel0 + " (Blue) ");
            slide.ReplaceText.Add("COMPARISON1 (Blue) , ", complist1 + compchannel0 + "(Blue), ");
            slide.ReplaceText.Add("COMPARISON3 (Red)", complist3 + compchannel2 + " (Red)");
            slide.ReplaceText.Add("COMPARISON5 (Purple)", complist5 + compchannel4 + " (Purple)");
            slide.ReplaceText.Add("COMPARISON7 (Olive Green)", complist7 + compchannel6 + " (Olive Green)");

            slide.ReplaceText.Add("95%", StatTesting + "%");

            //3-comparision
            //slide.ReplaceText.Add("are only compared against BENCHMARK and ", "are only compared against " + Benchlist1 + BenchChannel0 + " and ");
            slide.ReplaceText.Add("BENCHMARK as ", Benchlist1 + BenchChannel0 + " as ");
            slide.ReplaceText.Add("COMPARISON1 and COMPARISON3 ", complist1 + compchannel0 + " and " + complist3 + compchannel2 + " ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 and COMPARISON3 ", Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + " and " + complist3 + compchannel2 + " ");
            //3
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 and ", Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + " and ");
            slide.ReplaceText.Add("COMPARISON3", complist3 + compchannel2);

            slide.ReplaceText.Add("and COMPARISON3 (Red) ", "and " + complist3 + compchannel2 + "(Red) ");
            slide.ReplaceText.Add("are only compared against BENCHMARK and ", "are only compared against " + Benchlist1 + BenchChannel0 + " and ");
            //4-comparision
            slide.ReplaceText.Add("COMPARISON3 ", complist3 + compchannel2 + " ");
            slide.ReplaceText.Add(" COMPARISON5 ", complist5 + compchannel4 + " ");
            slide.ReplaceText.Add("COMPARISON5 ", complist5 + compchannel4 + " ");

            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as a benchmark (shown in Gray)", "All statistical testing is compared against " + Benchlist1 + BenchChannel0 + " as a benchmark (shown in Gray)");
            slide.ReplaceText.Add("COMPARISON1 (Blue) , COMPARISON3 (Red) ", complist1 + compchannel0 + " (Blue), " + complist3 + compchannel2 + " (Red) ");
            slide.ReplaceText.Add("COMPARISON5 (Purple) are only compared against BENCHMARK and ", complist5 + compchannel4 + " (Purple)" + " are only compared against " + Benchlist1 + BenchChannel0 + " and ");
            slide.ReplaceText.Add("Monthly+ Frequent Shopper’s for to BENCHMARK , COMPARISON1 , ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s for " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s for ") + Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + ", ");
            slide.ReplaceText.Add("Trips taken to BENCHMARK , COMPARISON1 , COMPARISON3 and ", "Trips taken to " + Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + " and ");
            slide.ReplaceText.Add("COMPARISON1 , COMPARISON3 ", complist1 + compchannel0 + ", " + complist3 + compchannel2 + " ");
            slide.ReplaceText.Add("COMPARISON1 , COMPARISON3 and COMPARISON5 ", complist1 + compchannel0 + ", " + complist3 + compchannel2 + " and " + complist5 + compchannel4 + " ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 , COMPARISON3 and COMPARISON5", Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + " and " + complist5 + compchannel4 + " ");
            //4

            //5-comparision
            slide.ReplaceText.Add("This report compares BENCHMARK Monthly+ Shoppers to the Monthly+ shoppers of the COMPARISON1 , COMPARISON3, COMPARISON5 and COMPARISON7 across the key metrics in the iSHOP survey.", "This report compares " + Benchlist1 + BenchChannel0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Shoppers to the " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers to the ") + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "shoppers of the " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of the ") + complist1 + compchannel0 + ", " + complist3 + compchannel2 + ", " + complist5 + compchannel4 + ", " + complist7 + compchannel6 + " ");
            slide.ReplaceText.Add("COMPARISON1 , COMPARISON3, COMPARISON5 and COMPARISON7 ", complist1 + compchannel0 + ", " + complist3 + compchannel2 + ", " + complist5 + compchannel4 + " and " + complist7 + compchannel6 + " ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 , COMPARISON3, COMPARISON5 and COMPARISON7 ", Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + ", " + complist5 + compchannel4 + " and " + complist7 + compchannel6 + " ");
            slide.ReplaceText.Add("Trips taken to BENCHMARK , COMPARISON1 , COMPARISON3, COMPARISON5 and COMPARISON7 ", "Trips taken to " + Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + ", " + complist5 + compchannel4 + " and " + complist7 + compchannel6 + " ");
            slide.ReplaceText.Add("Monthly+ Frequent Shopper’s for to BENCHMARK , COMPARISON1 , COMPARISON3, COMPARISON5 and COMPARISON7 ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s for " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s for ") + Benchlist1 + BenchChannel0 + ", " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + ", " + complist5 + compchannel4 + " and " + complist7 + compchannel6 + " ");
            slide.ReplaceText.Add("COMPARISON1 (Blue) , COMPARISON3 (Red) , COMPARISON5 (Purple) and COMPARISON7 (Olive Green) are only compared against BENCHMARK and ", complist1 + compchannel0 + "(Blue), " + complist3 + compchannel2 + " (Red), " + complist5 + compchannel4 + " (Purple)" + " and " + complist7 + compchannel6 + " (Olive Green)" + " are only compared against " + Benchlist1 + BenchChannel0 + " and ");
            //5

            //end
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            files.ReplaceImages = AddRetailerImages(strRetailersList);
            fileName = ReportNumber + ".Retailer Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Visitor Profile Slides
        private FileDetails Build_2_Comparison_Visitor_Profile_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');

            channelornot = CheckingChannelorNot(complist, channelornot);

            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));
            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\2_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Visitor Profile_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Visitor Profile");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5!="")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree & 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            ////slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Gender");
            lstMetricNames.Add("FactAgeGroups");
            lstMetricNames.Add("Ethnicity");
            lstMetricNames.Add("MaritalStatus");
            lstMetricNames.Add("HHIncomeGroups");
            lstMetricNames.Add("HHAdults");
            lstMetricNames.Add("HHChildren");
            lstMetricNames.Add("HHTotal");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("474124");
            lstSize.Add("16");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Demographic Segments");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + compchannel0 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + compchannel2 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + compchannel4 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + compchannel6 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            }
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            //chart.SizeOfText = ;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Gender", "1",5,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";

            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;

            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 6;
            }
            else
            {
                chart.SizeOfText = 8;
            }
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "FactAgeGroups", "1", 5, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";

            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;

            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 8;
            }
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Ethnicity", "1", 5, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";

            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;

            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 6;
            }
            else
            {
                chart.SizeOfText = 8;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHIncomeGroups", "1", 5, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            // chart.SizeOfText = 7;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MaritalStatus", "1", 6, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 8;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHTotal", "1", 6, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 8;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHAdults", "1", 6, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 8;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHChildren", "1", 6, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");    
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Visitor Profile";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Trip Type Slides
        private FileDetails Build_2_Comparison_Trip_Type_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');

            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\3_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Trip Type_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Type");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree & 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("TripMission");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("544124");
            lstSize.Add("20");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Trip Mission");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripMission", "1",9,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");    
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);
            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Type";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison PreShop Slides
        private FileDetails Build_2_Comparison_PreShop_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\4_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Pre Shop_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataTable tbl = new DataTable();
            List<Color> colorlist = new List<Color>();
            List<object> columnlist = new List<object>();
            string strFilter = "";
            List<object> appendixcolumnlist = new List<object>();
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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree & 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();
            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("PreTripOrigin");
            lstMetricNames.Add("DayofWeek");
            lstMetricNames.Add("WeekdayNet");
            lstMetricNames.Add("DayParts");
            lstMetricNames.Add("VisitPreparation");
            lstMetricNames.Add("VisitMotiviations");
            lstMetricNames.Add("ReasonForStoreChoice Top 10");
            lstMetricNames.Add("InStoreDestinationDetails Top 10");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("795922");
            lstSize.Add("543954");
            lstSize.Add("10");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key Pre-Shop Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "ReasonForStoreChoice0", "ReasonForStoreChoice1", "Destination Item Detail" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "PreTripOrigin", "1",12,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayofWeek", "1", 13, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayParts", "1", 13, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "WeekdayNet", "1", 13, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPreparation", "1", 14, 2));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.SizeOfText = 7;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.SizeOfText = 7;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.SizeOfText = 7;

            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitMotiviations", "1", 15, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath

            tbl = Get_Chart_Table(ds, "ReasonForStoreChoice Top 10",16,1);
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 39", rowheight.ToString(), columnwidth, "Reasons for Store Choice");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1"), "GapAnalysis");

            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1",17,1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist1);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            //slide.ReplaceText.Add("Dollar General (267048)", Benchlist1 + BenchChannel0 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (156748)", complist1 + compchannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice1", "1"), "GapAnalysis");
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[1])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist3);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            //slide.ReplaceText.Add("Dollar General (267049)", Benchlist1 + BenchChannel0 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (156749)", complist3 + compchannel2 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            ////Slide 10
            //slide = new SlideDetails();
            ////slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath

            //tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10",19,1);
            //var query2 = from r in tbl.AsEnumerable()
            //             select r.Field<object>("Objective");
            //appendixcolumnlist = query2.Distinct().ToList();
            //tbl = CreateAppendixTable(tbl);
            //GetTableHeight_FontSize(tbl);
            //columnwidth = new List<string>();
            //for (int i = 0; i < appendixcolumnlist.Count; i++)
            //{
            //    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            //}
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1",19,1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with Family Dollar: Destination Items", "Absolute Difference with " + Benchlist1 + ": Destination Items");
            //slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison In Store Slides
        private FileDetails Build_2_Comparison_In_Store_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\5_iShop RG New - " + chkComparisonFolderNumber + " Retailer_In Store_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".In Store");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();
            DataTable tbl = new DataTable();
            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("What they do in the store?", "What they do in the store? ");
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("ItemsPurchasedSummary");
            lstMetricNames.Add("MostImportantDestinationItems Top 10");
            lstMetricNames.Add("ImpulseItem Top 10");
            lstMetricNames.Add("BeveragepurchasedMonthly");

            dst = GetSlideTables(ds, "BeveragepurchasedMonthly", "1");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    lstMetricNames.Add(Convert.ToString(dst.Tables[0].Rows[i]["MetricItem"]));
                }

            }

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("353901");
            lstSize.Add("285802");
            lstSize.Add("10");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key In-Store Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));

            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);

            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "InStoreDestinationDetails Top 10", "Top 10 Impulse Items" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", "", "", "", "", "", ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 18;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "Smartphone/TabletInfluencedPurchases", "1", slide.SlideNumber,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            //chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
           
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 18;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "ItemsPurchasedSummary", "1", slide.SlideNumber, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Trip Net", "Items Purchased on Trip (Net)");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
           
            slidelist.Add(slide);

            //slide 5
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide5.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "InStoreDestinationDetails", "1", slide.SlideNumber, 1,true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with Family Dollar: Items Purchased", "Absolute Difference with " + Benchlist1 + ": Items Purchased");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
           
            slidelist.Add(slide);

            ////Slide 7 
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide7.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //tablecolumnlist = new Dictionary<string, object>();
            //columnlist = new List<string>() { " ", " " };
            //tablecolumnlist.Add("Reason for store choice", Benchlist[1]);
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide7.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "ImpulseItem", "1", slide.SlideNumber, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with Family Dollar: Impulse Items", "Absolute Difference with " + Benchlist1 + ": Impulse Items");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
           
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            DataSet tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "BeveragepurchasedMonthly", "1", slide.SlideNumber, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
           
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "CSD Regular/CSD Diet", "1", slide.SlideNumber, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);

            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "CSD Regular/CSD Diet", "1", slide.SlideNumber, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
           
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            tempdst = new DataSet();
            List<object> appendixcolumnlist = new List<object>();
            List<string> tablemetriclist = new List<string>();
            DataSet dsttop10 = GetSlideTables(ds, "BeveragepurchasedMonthly", "1", slide_Number + 1, 1);
            List<string> top5 = null;
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query3 = from r in dsttop10.Tables[0].AsEnumerable()                            
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
                tbl = base.GetMetricData(metric, dsttop10.Tables[0],true);
                DataTable atbl = CreateAppendixTablePreSHOP(tbl);
                atbl.TableName = metric;
                tempdst.Tables.Add(atbl.Copy());
            }
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query = from r in dsttop10.Tables[0].AsEnumerable()
                            select r.Field<object>("Objective");
                appendixcolumnlist = query.Distinct().ToList();
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

            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            tempdst = new DataSet();
            appendixcolumnlist = new List<object>();
            tablemetriclist = new List<string>();
            dsttop10 = GetSlideTables(ds, "BeveragepurchasedMonthly", "1", slide_Number + 1, 1);
            top5 = null;
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query3 = from r in dsttop10.Tables[0].AsEnumerable()
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
                tbl = base.GetMetricData(metric, dsttop10.Tables[0],true);
                DataTable atbl = CreateAppendixTablePreSHOP(tbl);               
                atbl.TableName = metric;
                tempdst.Tables.Add(atbl.Copy());
            }
            if (dsttop10 != null && dsttop10.Tables.Count > 0 && dsttop10.Tables[0].Rows.Count > 0)
            {
                var query2 = from r in dsttop10.Tables[0].AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query2.Distinct().ToList();
            }

            columnwidth = new List<string>();
            for (int i = 0; i < appendixcolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            rowheight = "87923";
            fontsize = 1000;
            UpdateAppendixMultipleTables(xmlpath, tempdst, appendixcolumnlist, "Table 13", rowheight.ToString(), columnwidth, "");

            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Below is for the remaining slides (11 to 10). Data for these slides gets change based on the selection and top 10 Metric items.
            //int j = 10;
            //if (dst != null && dst.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        tempdst = new DataSet();
            //        tempdst = FilterData(GetSlideTables(ds, Convert.ToString(dst.Tables[0].Rows[i]["MetricItem"]), "1"));

            //        slide = new SlideDetails();
            //        chart = new ChartDetails();
            //        chart.Type = ChartType.BAR;
            //        chart.ShowDataLegends = false;
            //        chart.DataLabelFormatCode = "0.0%";
            //        chart.ChartXmlPath = chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //        chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //        chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(tempdst));
            //        chart.Title = Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Trim();
            //        chart.XAxisColumnName = "Objective";
            //        chart.YAxisColumnName = "Volume";
            //        chart.MetricColumnName = "MetricItem";
            //        chart.ColorColumnName = "Significance";
            //        chart.TextColor = lststatcolour;
            //        slide.Charts.Add(chart);
            //        slide.ReplaceText = GetSourceDetail("Trips");
            //        slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(tempdst.Tables[0], Benchlist1) + ")");
            //        slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(tempdst.Tables[0], complist1) + ")");
            //        slide.SlideNumber = i + 10;

            //        if (i == 0)
            //        {
            //            slide.ReplaceText.Add("Top 10 Regular Carbonated Soft ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Drinks Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 1)
            //        {
            //            slide.ReplaceText.Add("Diet ", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">"));
            //            slide.ReplaceText.Add("Carbonated Soft ", " ");
            //            slide.ReplaceText.Add("Drinks Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 2)
            //        {
            //            slide.ReplaceText.Add("Top 10 Non-Sparkling ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Water Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 3)
            //        {
            //            slide.ReplaceText.Add("Top 10 RTD ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Juice Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 4)
            //        {
            //            slide.ReplaceText.Add("Top 10 100% ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " ");
            //            slide.ReplaceText.Add("Juice Brands Purchased", "Brands Purchased");
            //        }

            //        if (i == 5)
            //        {
            //            slide.ReplaceText.Add("Top 10 100% Orange ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">"));
            //            slide.ReplaceText.Add("Juice Brands Purchased", " Brands Purchased");
            //        }

            //        if (i == 6)
            //        {
            //            slide.ReplaceText.Add("Tea Brands Purchased", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased");
            //        }

            //        if (i == 7)
            //        {
            //            slide.ReplaceText.Add("Coffee Brands Purchased", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased");
            //        }

            //        if (i == 8)
            //        {
            //            slide.ReplaceText.Add("Top 10 Sports ", "Top 10 " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">"));
            //            slide.ReplaceText.Add("Drinks Brands Purchased", " Brands Purchased");
            //        }

            //        if (i == 9)
            //        {
            //            slide.ReplaceText.Add("Energy Drinks Brands Purchased", Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased");
            //        }

            //        slidelist.Add(slide);
            //    }
            //}

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".In Store";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Trip Summary Slides
        private FileDetails Build_2_Comparison_Trip_Summary_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\6_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Trip summary_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Summary");
            //destination_FilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            Dictionary<string, string> DicOvalData = new Dictionary<string, string>();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree &amp; 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();
            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("TimeSpent");
            lstMetricNames.Add("TripExpenditure");
            lstMetricNames.Add("ItemsPurchased");
            lstMetricNames.Add("PaymentMode");
            lstMetricNames.Add("RedeemedCouponTypes");
            lstMetricNames.Add("DestinationStoreTrip");
            lstMetricNames.Add("TripSatisfaction");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("584124");
            lstSize.Add("14");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key Post Shop Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TimeSpent", "1",(slide_Number + 1),1));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            //--->trip expenditure
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripExpenditure", "1", (slide_Number + 1), 2));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            //------->item purchased
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "ItemsPurchased", "1", (slide_Number + 1), 3));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            dst = FilterData(GetSlideTables(ds, "Average Time Spent (In HH:MM)", "1", (slide_Number + 1), 4));
            if (chkComparisonFolderNumber == "3")
            {
                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 2", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 35", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 36", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));
            }
            else if (chkComparisonFolderNumber == "4")
            {
                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 2", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 35", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 29", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[3]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 39", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));
            }
            else if (chkComparisonFolderNumber == "5")
            {
                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 2", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 35", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 36", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[3]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 42", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));

                timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[4]["Volume"])) * 100, 0)));
                hh = timeSpan.Hours;
                mm = timeSpan.Minutes;
                DicOvalData.Add("Oval 49", Convert.ToString(hh) + ":" + Convert.ToString(timeSpan.ToString().Split(':')[1]));
            }

            //--------->
            dst = FilterData(GetSlideTables(ds, "Average Trip Expenditure (In $)", "1", (slide_Number + 1), 5));
            if (chkComparisonFolderNumber == "3")
            {
                DicOvalData.Add("Oval 43", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 44", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 45", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
            }
            else if (chkComparisonFolderNumber == "4")
            {
                DicOvalData.Add("Oval 43", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 44", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 40", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 41", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[3]["Volume"])) * 100, 0)));
            }
            else if (chkComparisonFolderNumber == "5")
            {
                DicOvalData.Add("Oval 65", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 66", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 67", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 68", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[3]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 69", "$" + Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[4]["Volume"])) * 100, 0)));
            }
            //----------->
            dst = FilterData(GetSlideTables(ds, "Average Items Purchased (In Items)", "1", (slide_Number + 1), 6));
            if (chkComparisonFolderNumber == "3")
            {
                DicOvalData.Add("Oval 46", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 47", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 48", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
            }
            else if (chkComparisonFolderNumber == "4")
            {
                DicOvalData.Add("Oval 46", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 47", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 42", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 45", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[3]["Volume"])) * 100, 0)));
            }
            else if (chkComparisonFolderNumber == "5")
            {
                DicOvalData.Add("Oval 70", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[0]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 71", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[1]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 72", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[2]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 73", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[3]["Volume"])) * 100, 0)));
                DicOvalData.Add("Oval 74", Convert.ToString(Math.Round(Convert.ToDecimal((FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[1].Rows[4]["Volume"])) * 100, 0)));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateOvalData(xmlpath, DicOvalData);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 8;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "PaymentMode", "1", (slide_Number + 1), 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "RedeemedCouponTypes", "1", (slide_Number + 1), 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationStoreTrip", "1", (slide_Number + 1), 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripSatisfaction", "1", (slide_Number + 1), 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);


            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Frequent Shopper Slides (Frequest Shopper)
        private FileDetails Build_2_Comparison_Shopper_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\7_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Frequent Shopper_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Frequent Shopper");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree &amp; 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();
            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Gender");
            lstMetricNames.Add("FactAgeGroups");
            lstMetricNames.Add("Ethnicity");
            lstMetricNames.Add("MaritalStatus");
            lstMetricNames.Add("HHIncomeGroups");
            lstMetricNames.Add("HHAdults");
            lstMetricNames.Add("HHChildren");
            lstMetricNames.Add("HHTotal");
            lstMetricNames.Add("Attitudinal Segment");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("633257");
            lstSize.Add("505726");
            lstSize.Add("14");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Demographic Segments");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Gender", "1", 5, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "FactAgeGroups", "1", 5, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Ethnicity", "1", 5, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHIncomeGroups", "1", 5, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MaritalStatus", "1", 6, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHTotal", "1", 6, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHAdults", "1", 6, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHChildren", "1", 6, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            //}
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 8;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Attitudinal Segment", "1", 7, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Attitudinal Segment of <filter> Shoppers", "Attitudinal Segment of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Frequent Shopper";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Cross Retailer Slides(8_iShop RG New - 2 Retailer_Cross Retailer Shoppers_V5.1)
        private FileDetails Build_2_Comparison_Retailer_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\8_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Cross Retailer Shoppers_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Cross Retailer");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            DataSet dstemp = ds.Copy();
            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();
            DataTable tbl = new DataTable();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree &amp; 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: All Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Shopper Frequency2");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("621740");
            lstSize.Add("445060");
            lstSize.Add("20");

            lstHeaderText = new List<string>();
            lstHeaderText.Add(Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "Shopper Frequency1" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //

            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Cross Retailer Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();

            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";

            //if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            //{
            //    chart.SizeOfText = 7;
            //}
            //else
            //{
            //    chart.SizeOfText = 9;
            //}
            chart.IsBarHexColorForSeriesPoints = false;
            chart.SizeOfText = 6;
            //chart.SizeOfText = 8;

            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dstTemp = FilterData(GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2));
            chart.Data = CleanXMLBeforeBind(dstTemp.Tables[0]);
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            //chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dstTemp, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("14%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[0]["Volume"]), 1)) + "%");
            slide.ReplaceText.Add("10%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[1]["Volume"]), 1)) + "%");
            slide.ReplaceText.Add("7%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[2]["Volume"]), 1)) + "%");

            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                slide.ReplaceText.Add("3%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[3]["Volume"]), 1)) + "%");
            }
            if (complist7 != null && complist7 != "")
            {
                slide.ReplaceText.Add("1%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[4]["Volume"]), 1)) + "%");
            }
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("3%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[3]["Volume"]), 0)) + "%");
            //    slide.ReplaceText.Add("Monthly + Shoppers of Comparison3", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + complist5);
            //}


            //slide.ReplaceText.Add("Monthly + Shoppers of Benchmark", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(Benchlist1));//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[0]["Objective"]))));
            //slide.ReplaceText.Add("Monthly + Shoppers of Comparison1", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist1));//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[1]["Objective"]))));
            //slide.ReplaceText.Add("Monthly + Shoppers of Whole Foods", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + complist3);

            //slide.ReplaceText.Add("Cross Shopping Behavior of Weekly+ Shoppers", "Cross Shopping Behavior of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            //slide.ReplaceText.Add("(Weekly + Shopper of Retailer/Channel) / (Total Shoppers)", "(" + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shopper of Retailer/Channel) / (Total Shoppers)"); 



            dst = GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            if (chkComparisonFolderNumber != "5")
            {
                UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 2, "Retailer");
            }

            dstemp = GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2);
            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), Benchlist[1]);
            slide.ReplaceText.Add("Family Dollar : 48%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + Benchlist1 + ": " + (tbl.Rows.Count > 0 ? (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%" : string.Empty));

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.IsBarHexColorForSeriesPoints = false;
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(dstemp.Tables[0]);
            //chart.XAxisColumnName = "Objective";
            //chart.YAxisColumnName = "Volume";
            //chart.MetricColumnName = "MetricItem";
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            //chart.BarHexColors = colorlist;
            //slide.Charts.Add(chart);
            if (chkComparisonFolderNumber != "5")
            {
                UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 2, "Retailer");

            }
            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), complist[1]);
            slide.ReplaceText.Add("Dollar General : 52%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + complist1 + ": " + (tbl.Rows.Count > 0 ? (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%" : string.Empty));

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.IsBarHexColorForSeriesPoints = false;
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Objective";
            //chart.YAxisColumnName = "Volume";
            //chart.MetricColumnName = "MetricItem";
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            //chart.BarHexColors = colorlist;
            //slide.Charts.Add(chart);

            if (chkComparisonFolderNumber != "5")
            {
                UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 25", 2, "Retailer");
            }

            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), complist[3]);
            //slide.ReplaceText.Add("99 Cents Only Store: 38%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + complist3 + ": " + (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 0)) + "%");
            //slide.ReplaceText.Add("<filter> Shoppers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by top 10 retailers");
            //slide.ReplaceText.Add("Text1", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            //slide.ReplaceText.Add("Text2", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            //slide.ReplaceText.Add("Text3", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            //slide.ReplaceText.Add("Weekly+ Shoppers ", reportparams.ShopperFrequencyShortName.ToString() + " Shoppers ");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + GetSampleSize(dstTemp.Tables[0], complist5) + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //}

            //slide.ReplaceText.Add("% of Monthly + Shoppers by Top 10 Retailers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by Top 10 Retailers");
            //slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5,complist7, dstTemp);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Cross Retailer";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Shopper Perception Slides
        private FileDetails Build_2_Comparison_Perception_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\9_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Shopper Perceptions_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("StoreAttributesFactors");
            lstMetricNames.Add("GoodPlaceToShopFactors");
            lstMetricNames.Add("StoreAttribute Top 10");
            lstMetricNames.Add("GoodPlaceToShop Top 10");
            lstMetricNames.Add("MainFavoriteStore");
            lstMetricNames.Add("RetailerLoyaltyPyramid");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("804269");
            lstSize.Add("626683");
            lstSize.Add("14");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key imagery measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "StoreAttributesFactors", "1", 13, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide5.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";            
            tbl = Get_Chart_Table(ds, "StoreAttribute Top 10", 14, 1);
            List<object> appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Store Imagery");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute0", "1"));

            geods = FilterData(GetSlideTables(ds, "StoreAttribute0", "1", 15, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1);
            slide.ReplaceText.Add("Dollar2", complist1);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute1", "1"));

            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[1])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1);
            slide.ReplaceText.Add("Dollar2", complist3);
            //slide.ReplaceText.Add("99 Cents Only Store", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "GoodPlaceToShopFactors", "1", 17, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            //slide.ReplaceText.Add("roduct imagery of Weekly+ shoppers", "roduct imagery of " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //chart.ColorColumnName = "Significance";
            //chart.TextColor = lststatcolour;
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide8.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);
            tbl = Get_Chart_Table(ds, "GoodPlaceToShop Top 10", 18, 1);
            appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Good Place to Shop for");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1"));
            geods = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1", 19, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1);
            slide.ReplaceText.Add("Dollar2", complist1);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop1", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[1])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1);
            slide.ReplaceText.Add("Dollar2", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MainFavoriteStore", "1", 21, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "RetailerLoyaltyPyramid", "1", 22, 1));
            DataView dv = dst.Tables[0].Copy().DefaultView;
            //dv.Sort = "Volume desc";
            DataTable sortedDT = dv.ToTable();
            chart.Data = sortedDT;
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Beverage Interaction Slides
        private FileDetails Build_2_Comparison_Beverage_Interaction_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\10_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Beverage Interaction_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Beverage Interaction");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree &amp; 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("BeverageConsumedMonthly");
            lstMetricNames.Add("BeveragepurchasedMonthly");

            dst = GetSlideTables(ds, "BeveragepurchasedMonthly", "1");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    lstMetricNames.Add(Convert.ToString(dst.Tables[0].Rows[i]["MetricItem"]));
                }

            }

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("403901");
            lstSize.Add("350901");
            lstSize.Add("10");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Beverages Consumed Monthly");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "Beverage Consumed Monthly" };
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Beverage Purchase Monthly", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //Slide 3
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "BeverageConsumedMonthly", "1"));
            //chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Objective";
            //chart.YAxisColumnName = "Volume";
            //chart.MetricColumnName = "MetricItem";
            //chart.ColorColumnName = "Significance";
            //chart.TextColor = lststatcolour;
            //slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Top 10 categories consumed among  Weekly+ shoppers within Retailer", "Top 10 categories consumed among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            slide.SlideNumber = GetSlideNumber();
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 8;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "BeveragepurchasedMonthly", "1", slide.SlideNumber, 1,true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Top 10 categories purchased among  Weekly+ shoppers within Retailer", "Top 10 categories purchased among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            //slide.ReplaceText.Add("Top 10 Categories Purchased Among Weekly+ Shoppers", "Top 10 Categories Purchased Among " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);

            slidelist.Add(slide);

            //Below is for the remaining slides (11 to 10). Data for these slides gets change based on the selection and top 10 Metric items.
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
            int j = 2;
            DataSet tempdst = new DataSet();
            List<String> top2 = new List<String>() { "SSD Regular", "SSD Diet" };
            var query3 = from r in dst.Tables[0].AsEnumerable()
                         select Convert.ToString(r.Field<object>("MetricItem"));
            //List<string> top10 = top2;
            //top10.AddRange(query3.Distinct().ToList());
            List<string> top10 = query3.Distinct().ToList();
            int tbl_slide_no = slide.SlideNumber + 1;
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    tempdst = new DataSet();
                    tempdst = FilterData(GetSlideTables(ds, Convert.ToString(top10[i]), "1", tbl_slide_no, 1, true));
                    tbl_slide_no++;
                    slide = new SlideDetails();
                    if (tempdst != null && tempdst.Tables.Count > 0)
                    {
                        chart = new ChartDetails();
                        chart.Type = ChartType.BAR;
                        chart.ShowDataLegends = false;
                        chart.DataLabelFormatCode = "0.0%";
                        if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
                        {
                            chart.SizeOfText = 8;
                        }
                        else
                        {
                            chart.SizeOfText = 10;
                        }
                        chart.ChartXmlPath = chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
                        chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
                        chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(tempdst));
                        chart.Title = Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Trim();
                        chart.XAxisColumnName = "Objective";
                        chart.YAxisColumnName = "Volume";
                        chart.MetricColumnName = "MetricItem";
                        chart.ColorColumnName = "Significance";
                        chart.TextColor = lststatcolour;
                        slide.Charts.Add(chart);
                        //slide.ReplaceText = GetSourceDetail("Shoppers");
                        //slide.ReplaceText.Add("Top 10 Brands among  Weekly+ shoppers within Retailer", "Top 10 Brands among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
                        //if (complist5 != null && complist5 != "")// checking 4-comparison 
                        //{
                        //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                        //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + GetSampleSize(tempdst.Tables[0], Benchlist1) + ")");
                        //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(tempdst.Tables[0], complist1) + ")");
                        //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + GetSampleSize(tempdst.Tables[0], complist3) + ")");
                        //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + GetSampleSize(tempdst.Tables[0], complist5) + ")");
                        //}
                        //else
                        //{
                        //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(tempdst.Tables[0], Benchlist1) + ")");
                        //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(tempdst.Tables[0], complist1) + ")");
                        //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + GetSampleSize(tempdst.Tables[0], complist3) + ")");
                        //}
                        slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
                        slide.ReplaceText.Add(metrictitels[i], "Top " + Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Replace("&amp;lt;", "<").Replace("&amp;gt;", ">") + " Brands Purchased Monthly");
                        //slide.ReplaceText.Add("Top Brands Purchased Among Weekly+ Shoppers", "Top Brands Purchased Among  " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");

                    }
                    slide.SlideNumber = GetSlideNumber();
                    slidelist.Add(slide);
                }
            }

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Beverage Interaction";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Appendix Slides
        private FileDetails Build_2_Comparison_Appendix_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            DataTable tbl = new DataTable();
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            if (reportparams.ModuleBlock == "AcrossShopper")
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\11_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Appendix_V5.1");
            }
            else
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\11_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Appendix_V5.1 - Trips");
            }

            //tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Appendix");
            //destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            Dictionary<string, object> tablecolumnlist = new Dictionary<string, object>();
            List<string> columnlist = new List<string>();
            List<object> appendixcolumnlist = new List<object>();
            string strFilter = "";

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

            if (reportparams.ModuleBlock == "AcrossShopper")
            {
                //Slide 1
                slide = new SlideDetails();
                //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
                //if (complist5 != null && complist5 != "")// checking 4-comparison 
                //{
                //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree &amp; 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
                //}
                //else
                //{
                //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
                //}
                //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
                //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
                slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
                slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                //slide 2
                //slide = new SlideDetails();
                //slide.SlideNumber = GetSlideNumber();
                ////slide.ReplaceText = GetSourceDetail("Trips");
                //slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);

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
                // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
                //UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 1", rowheight.ToString(), columnwidth, "Reasons for Store Choice");
                //slidelist.Add(slide);
                //slide 3
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Shoppers");
                slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);

                tbl = Get_Chart_Table(ds, "StoreAttribute", slide.SlideNumber, 1);
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
                //slide 4
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Shoppers");
                slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
                tbl = Get_Chart_Table(ds, "GoodPlaceToShop", slide.SlideNumber, 1);
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
            }
            else
            {
                //Slide 1
                slide = new SlideDetails();
                slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1 + BenchChannel0);
                if (complist5 != null && complist5 != "")// checking 4-comparison 
                {
                    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree &amp; 7-Eleven ", "Comparisons: " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + " & " + complist5 + compchannel4);
                }
                else
                {
                    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + compchannel0 + "; " + complist3 + compchannel2);
                }
                slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));

                //slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
                slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: All Trips");
                slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                //slide 2
                slide = new SlideDetails();
                slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Trips");
                slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
                tbl = Get_Chart_Table(ds, "ReasonForStoreChoice", slide.SlideNumber, 1);
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
                UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 1", rowheight.ToString(), columnwidth, "Reason for Store Choice");
                slidelist.Add(slide);
            }

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Appendix";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #endregion
        #region 3 Comparison Slides only for Preshop and Shopper Perception
        #region 3 Comparison PreShop Slides
        private FileDetails Build_3_Comparison_PreShop_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\4_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Pre Shop_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataTable tbl = new DataTable();
            List<Color> colorlist = new List<Color>();
            List<object> columnlist = new List<object>();
            string strFilter = "";
            List<object> appendixcolumnlist = new List<object>();
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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree & 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);


            //slide 2
            slide = new SlideDetails();
            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("PreTripOrigin");
            lstMetricNames.Add("DayofWeek");
            lstMetricNames.Add("WeekdayNet");
            lstMetricNames.Add("DayParts");
            lstMetricNames.Add("VisitPreparation");
            lstMetricNames.Add("VisitMotiviations");
            lstMetricNames.Add("ReasonForStoreChoice Top 10");
            lstMetricNames.Add("InStoreDestinationDetails Top 10");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("795922");
            lstSize.Add("543954");
            lstSize.Add("10");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key Pre-Shop Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + compchannel0 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + compchannel2 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + compchannel4 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + compchannel6 + " differs from " + Benchlist1.Replace("&", "&amp;") + BenchChannel0);
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "ReasonForStoreChoice0", "ReasonForStoreChoice1", "ReasonForStoreChoice2", "Destination Item Detail" };
            tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "PreTripOrigin", "1",12,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayofWeek", "1", 13, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayParts", "1", 13, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "WeekdayNet", "1", 13, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPreparation", "1", 14, 2));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPreparation", "1", 14, 2));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 7;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 7;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 7;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 7;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitMotiviations", "1", 15, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath

            tbl = Get_Chart_Table(ds, "ReasonForStoreChoice Top 10", 16, 1);
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 39", rowheight.ToString(), columnwidth, "Reasons for Store Choice");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1", 17, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);
            //chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(dst.Tables[0]));
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist1);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Dollar ", complist1 + compchannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");

            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);
            //chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(dst.Tables[0]));
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[1])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist3);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.ReplaceText.Add("Dollar ", complist3 + compchannel2 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice2", "1"), "GapAnalysis");
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[2], Benchlist1);
            //chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(dst.Tables[0]));
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[2])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist3);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.ReplaceText.Add("Dollar ", complist5 + compchannel4 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            ////Slide 11
            //slide = new SlideDetails();
            ////slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath

            //tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10",20,1);
            //var query2 = from r in tbl.AsEnumerable()
            //             select r.Field<object>("Objective");
            //appendixcolumnlist = query2.Distinct().ToList();
            //tbl = CreateAppendixTable(tbl);
            //GetTableHeight_FontSize(tbl);
            //columnwidth = new List<string>();
            //for (int i = 0; i < appendixcolumnlist.Count; i++)
            //{
            //    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            //}
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1",20,1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with Family Dollar: Destination Items", "Absolute Difference with " + Benchlist1 + ": Destination Items");
            //slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion
        #region 3 Comparison Shopper Perception Slides
        private FileDetails Build_3_Comparison_Perception_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\9_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Shopper Perceptions_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;

            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("StoreAttributesFactors");
            lstMetricNames.Add("GoodPlaceToShopFactors");
            lstMetricNames.Add("StoreAttribute Top 10");
            lstMetricNames.Add("GoodPlaceToShop Top 10");
            lstMetricNames.Add("MainFavoriteStore");
            lstMetricNames.Add("RetailerLoyaltyPyramid");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("804269");
            lstSize.Add("626683");
            lstSize.Add("14");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key imagery measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "StoreAttributesFactors", "1", 13, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide5.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";            
            tbl = Get_Chart_Table(ds, "StoreAttribute Top 10", 14, 1);
            List<object> appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Store Imagery");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute0", "1"));
            geods = FilterData(GetSlideTables(ds, "StoreAttribute0", "1", 15, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1 + BenchChannel0 + AppendStars(Benchlist1));
            slide.ReplaceText.Add("Dollar2", complist1 + compchannel0 + AppendStars(complist1));
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute1", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1 + BenchChannel0 + AppendStars(Benchlist1));
            slide.ReplaceText.Add("Dollar General (26702)", complist3 + compchannel2 + AppendStars(complist3) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute2", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[2], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family", Benchlist1);
            slide.ReplaceText.Add("Dollar General (26703)", complist5 + compchannel4 + AppendStars(complist5) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "GoodPlaceToShopFactors", "1", 18, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            //slide.ReplaceText.Add("roduct imagery of Weekly+ shoppers", "roduct imagery of " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //chart.ColorColumnName = "Significance";
            //chart.TextColor = lststatcolour;
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide8.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);
            tbl = Get_Chart_Table(ds, "GoodPlaceToShop Top 10", 19, 1);
            appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Good Place to Shop for");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1"));
            geods = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1", 20, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1 + BenchChannel0);
            slide.ReplaceText.Add("Dollar2", complist1 + compchannel0);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop1", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1 + BenchChannel0);
            slide.ReplaceText.Add("Dollar General (267011)", complist3 + compchannel2 + AppendStars(complist3) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop2", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[2], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family1", Benchlist1 + BenchChannel0);
            slide.ReplaceText.Add("Dollar General (267012)", complist5 + compchannel4 + AppendStars(complist5) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 13
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MainFavoriteStore", "1", 23, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 14
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "RetailerLoyaltyPyramid", "1", 24, 1));
            DataView dv = dst.Tables[0].Copy().DefaultView;
            //dv.Sort = "Volume desc";
            DataTable sortedDT = dv.ToTable();
            chart.Data = sortedDT;
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #endregion
        #region 4 Comparison Slides only for Preshop and Shopper Perception
        #region 4 Comparison PreShop Slides
        private FileDetails Build_4_Comparison_PreShop_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string  tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\4_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Pre Shop_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataTable tbl = new DataTable();
            List<Color> colorlist = new List<Color>();
            List<object> columnlist = new List<object>();
            string strFilter = "";
            List<object> appendixcolumnlist = new List<object>();
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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, Dollar Tree & 7-Eleven ", "Comparisons: " + complist1 + ", " + complist3 + " & " + complist5);
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //}
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();
            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("PreTripOrigin");
            lstMetricNames.Add("DayofWeek");
            lstMetricNames.Add("WeekdayNet");
            lstMetricNames.Add("DayParts");
            lstMetricNames.Add("VisitPreparation");
            lstMetricNames.Add("VisitMotiviations");
            lstMetricNames.Add("ReasonForStoreChoice Top 10");
            lstMetricNames.Add("InStoreDestinationDetails Top 10");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("795922");
            lstSize.Add("543954");
            lstSize.Add("10");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key Pre-Shop Measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")// checking 4-comparison 
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "ReasonForStoreChoice0", "ReasonForStoreChoice1", "ReasonForStoreChoice2", "ReasonForStoreChoice3", "Destination Item Detail" };
            tbl = Get_Summary_Table(ds, metriclist);
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "PreTripOrigin", "1", 12, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //if (complist5 != null && complist5 != "")// checking 4-comparison 
            //{
            //    //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Kroger (2705)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Publix (2012)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("Whole Foods (385)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //    slide.ReplaceText.Add("Walmart (2000)", complist5 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            //}
            //else
            //{
            //    slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //    slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //    slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //}
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            if (chkComparisonFolderNumber == "5" || chkComparisonFolderNumber == "4")
            {
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayofWeek", "1", 13, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DayParts", "1", 13, 3));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "WeekdayNet", "1", 13, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPreparation", "1", 14, 2));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPreparation", "1", 14, 2));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist7));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);



            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1", 14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist7));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#4f6228"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitMotiviations", "1", 15, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            tbl = Get_Chart_Table(ds, "ReasonForStoreChoice Top 10", 16, 1);
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 39", rowheight.ToString(), columnwidth, "Reasons for Store Choice");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1"), "GapAnalysis");

            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1", 17, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist1);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice1", "1"), "GapAnalysis");

            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[1])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist3);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice2", "1"), "GapAnalysis");
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[2], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[2])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist3);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml"); ;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice2", "1"), "GapAnalysis");
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[3], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[3])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar1", Benchlist1);
            //slide.ReplaceText.Add("Family2", complist3);
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            ////Slide 11
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.IsBarHexColorForSeriesPoints = true;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            ////dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice3", "1"), "GapAnalysis");
            //dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[3], Benchlist1);
            //chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[3])));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //chart.ColorColumnName = "Significance";
            //chart.TextColor = lststatcolour;
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#4f6228"));
            //chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            //slide.Charts.Add(chart);
            ////slide.ReplaceText = GetSourceDetail("Trips");
            ////slide.ReplaceText.Add("Dollar General (2670)", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            ////slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            ////slide.ReplaceText.Add("Dollar1", Benchlist1);
            ////slide.ReplaceText.Add("Family2", complist3);
            //slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            ////Slide 12
            //slide = new SlideDetails();
            ////slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath

            //tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10",21,1);
            //var query2 = from r in tbl.AsEnumerable()
            //             select r.Field<object>("Objective");
            //appendixcolumnlist = query2.Distinct().ToList();
            //tbl = CreateAppendixTable(tbl);
            //GetTableHeight_FontSize(tbl);
            //columnwidth = new List<string>();
            //for (int i = 0; i < appendixcolumnlist.Count; i++)
            //{
            //    columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
            //}
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);

            //slide 13
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1",21,1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with Family Dollar: Destination Items", "Absolute Difference with " + Benchlist1 + ": Destination Items");
            //slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);//added by bramhanath
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion
        #region 4 Comparison Shopper Perception Slides
        private FileDetails Build_4_Comparison_Perception_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string  tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7;
            string compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0;
            string[] complist, filt, Benchlist;
            string[] channelornot = new string[8] { "", "", "", "", "", "", "", "" };
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');

            if (Convert.ToString(Benchlist[0]) == "Channels")
            {
                BenchChannel0 = " Channel";
            }
            else
            {
                BenchChannel0 = "";
            }

            channelornot = CheckingChannelorNot(complist, channelornot);
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            compchannel0 = (Convert.ToString(channelornot[0]));
            compchannel2 = (Convert.ToString(channelornot[2]));

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = (Convert.ToString(channelornot[6]));
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
                compchannel6 = "";
                compchannel4 = (Convert.ToString(channelornot[4]));
            }
            else
            {
                complist7 = "";
                complist5 = "";
                compchannel6 = "";
                compchannel4 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\" + chkComparisonFolderNumber + "-Comparisons\\9_iShop RG New - " + chkComparisonFolderNumber + " Retailer_Shopper Perceptions_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
            sPowerPointTemplatePath = destination_FilePath[0];
            destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();

            string strFilter = "";

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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + "; " + complist3);
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("StoreAttributesFactors");
            lstMetricNames.Add("GoodPlaceToShopFactors");
            lstMetricNames.Add("StoreAttribute Top 10");
            lstMetricNames.Add("GoodPlaceToShop Top 10");
            lstMetricNames.Add("MainFavoriteStore");
            lstMetricNames.Add("RetailerLoyaltyPyramid");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("804269");
            lstSize.Add("626683");
            lstSize.Add("14");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key imagery measures");
            lstHeaderText.Add(complist1.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            lstHeaderText.Add(complist3.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            if (complist5 != null && complist5 != "")
            {
                lstHeaderText.Add(complist5.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            if (complist7 != null && complist7 != "")
            {
                lstHeaderText.Add(complist7.Replace("&", "&amp;") + " differs from " + Benchlist1.Replace("&", "&amp;"));
            }
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>();
            DataTable tbl = Get_Summary_Table(ds, metriclist);
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            objectivecolumnlist = GetColumnlist(tbl);
            GetTableHeight_FontSize(tbl);
            columnwidth = new List<string>();
            for (int i = 0; i < objectivecolumnlist.Count; i++)
            {
                columnwidth.Add(Convert.ToString(table_width / objectivecolumnlist.Count));
            }
            //


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 4", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

            slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "StoreAttributesFactors", "1",13,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide5.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";            
            tbl = Get_Chart_Table(ds, "StoreAttribute Top 10", 14, 1);
            List<object> appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Store Imagery");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, ds, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute0", "1"));

            geods = FilterData(GetSlideTables(ds, "StoreAttribute0", "1", 15, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist1);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute1", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[1])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist3);
            //slide.ReplaceText.Add("99 Cents Only Store", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute2", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[2], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[2])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist5);
            //slide.ReplaceText.Add("99 Cents Only Store", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute3", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[3], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[3])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#4f6228"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + BenchChannel0 + " and " + complist7 + compchannel6 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + BenchChannel0 + " and " + complist7 + compchannel6 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist7);
            //slide.ReplaceText.Add("99 Cents Only Store", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "GoodPlaceToShopFactors", "1", 19, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            //slide.ReplaceText.Add("roduct imagery of Weekly+ shoppers", "roduct imagery of " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //chart.ColorColumnName = "Significance";
            //chart.TextColor = lststatcolour;
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            //xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide8.xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = GetSlideNumber();
            //slidelist.Add(slide);
            tbl = Get_Chart_Table(ds, "GoodPlaceToShop Top 10", 20, 1);
            appendixcolumnlist = new List<object>();
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
            xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Good Place to Shop for");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1"));
            geods = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1", 21, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives = CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[0], Benchlist1);

            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[0])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist1 + compchannel0 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist1);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop1", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[1], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist3)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#953735"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist3 + compchannel2 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist3);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 13
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop2", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[2], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#403152"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist5 + compchannel4 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist5);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 14
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop3", "1"));
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[3], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist7)));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#4f6228"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, geods, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + BenchChannel0 + " and " + complist7 + compchannel6 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 99 Cents Only Store and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + BenchChannel0 + " and " + complist7 + compchannel6 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("Family1", Benchlist1);
            //slide.ReplaceText.Add("Dollar2", complist7);
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 15
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MainFavoriteStore", "1", 25, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 16
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");;
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "RetailerLoyaltyPyramid", "1", 26, 1));
            DataView dv = dst.Tables[0].Copy().DefaultView;
            //dv.Sort = "Volume desc";
            DataTable sortedDT = dv.ToTable();
            chart.Data = sortedDT;
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("99 Cents Only Store (1567)", complist3 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist1, complist3, complist5, complist7, dst, compchannel0, compchannel2, compchannel4, compchannel6, BenchChannel0);
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFiles/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion
        #endregion        
        public override DataSet FilterData(DataSet dtbl, [Optional] string strGapOption)
        {
            DataSet ds = new DataSet();
            if (dtbl != null && dtbl.Tables.Count > 0 && dtbl.Tables[0].Rows.Count > 0)
            {
                DataTable tb = dtbl.Tables[0].Copy();
                foreach (DataRow row in tb.Rows)
                {
                    string[] strBenchMark = reportparams.Benchmark.Split('|');
                    if (strBenchMark.Length > 1)
                    {
                        if (Convert.ToString(row["Objective"]).Equals(reportparams.BenchmarkShortName, StringComparison.OrdinalIgnoreCase))
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
                    row["Objective"] = Get_ShortNames(Convert.ToString(row["Objective"])).Trim();
                    row["Metric"] = Get_ShortNames(Convert.ToString(row["Metric"])).Trim();

                    if (strGapOption == "GapAnalysis")
                    {
                        row["MetricItem"] = Get_ShortNames(Convert.ToString(row["MetricItem"])).Trim();
                    }
                }
                ds.Tables.Add(tb);
            }
            return ds;
        }
        public Dictionary<string, string> GetSourceDetail(string stroption, string Benchlist1, string complist1, string BenchChannel0, string compChannel0, DataSet dst)
        {
            Dictionary<string, string> DictSourceDetails = new Dictionary<string, string>();
            string strFilter = "";
            string strResult = "";
            //string stroriginal = "Source: iSHOP <<time period>>; Base: <<Frequency>>; Filters: <<Advanced Filers>>";
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

            if (stroption == "Trips")
            {
                strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            }
            else
            {
                strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            }

            DictSourceDetails.Add(stroriginal, strResult);
            DictSourceDetails.Add("Source: iSHOP <<time period>>; Base: <<Frequency>>; Filters: <<Advanced Filers>>", strResult);
            //DictSourceDetails.Add("* Significantly differed with <#benchmark> ", "* Significantly differed with " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            //DictSourceDetails.Add("* Significantly Different from <#benchmark> ", "* Significantly Different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            if (ReportNumber == 9)
            {
                DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " is significantly higher  ");
                DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " ");
            }
            else if (ReportNumber == 11)
            {
                if (stroption == "Shoppers")
                {
                    DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " is significantly higher  ");
                    DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " ");
                }
                else
                {
                    DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " is significantly higher  ");
                    DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " ");
                }
            }
            else
            {
                DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " is significantly higher  ");
                DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " ");
            }
            DictSourceDetails.Add(">95%", ">" + Convert.ToString(StatTesting) + "%");
            DictSourceDetails.Add("<95%", "<" + Convert.ToString(StatTesting) + "%");
            //start - added by bramhanath
            DictSourceDetails.Add("What they do in the store?", "What they do in the store? ");
            DictSourceDetails.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1 + BenchChannel0);
            DictSourceDetails.Add("Comparisons: Dollar General ", "Comparisons: " + complist1 + compChannel0);
            DictSourceDetails.Add("Comparisons: Dollar General", "Comparisons: " + complist1 + compChannel0);
            DictSourceDetails.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            
            DictSourceDetails.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            DictSourceDetails.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Base: All Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");

            if (dst != null && dst.Tables.Count > 0)
            {
                if (ReportNumber == 9)
                {
                    DictSourceDetails.Add("Family", Benchlist1 + BenchChannel0 + AppendStars(Benchlist1) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("Dollar General (2670)", complist1 + compChannel0 + AppendStars(complist1) + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
                }
                else
                {
                    DictSourceDetails.Add("Family", Benchlist1 + BenchChannel0 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("Dollar General (2670)", complist1 + compChannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
                    DictSourceDetails.Add("Dollar", complist1 + compChannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
                }
            }
            //end
            return DictSourceDetails;
        }
        //New Source function added by bramhanath only 3,4,5 comparisions
        public Dictionary<string, string> GetSourceDetailNew(string stroption, string Benchlist1, string complist1, string complist3, string complist5, string complist7, DataSet dst, string compchannel0, string compchannel2, string compchannel4, string compchannel6, string BenchChannel0)
        {
            Dictionary<string, string> DictSourceDetails = new Dictionary<string, string>();
            string strFilter = "";
            string strResult = "";
            //string stroriginal = "Source: iSHOP <<time period>>; Base: <<Frequency>>; Filters: <<Advanced Filers>>";
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

            if (stroption == "Trips")
            {
                strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            }
            else
            {
                strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
            }

            DictSourceDetails.Add(stroriginal, strResult);
            DictSourceDetails.Add("Source: iSHOP <<time period>>; Base: <<Frequency>>; Filters: <<Advanced Filers>>", strResult);
            //DictSourceDetails.Add("* Significantly differed with <#benchmark> ", "* Significantly differed with " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            //DictSourceDetails.Add("* Significantly Different from <#benchmark> ", "* Significantly Different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            if (ReportNumber == 9)
            {
                DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " is significantly higher  ");
                DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " ");
            }
            else if (ReportNumber == 11)
            {
                if (stroption == "Shoppers")
                {
                    DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " is significantly higher  ");
                    DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + AppendStars(Benchlist1) + " ");
                }
                else
                {
                    DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " is significantly higher  ");
                    DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " ");
                }
            }
            else
            {
                DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " is significantly higher  ");
                DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + BenchChannel0 + " ");
            }
            DictSourceDetails.Add(">95%", ">" + Convert.ToString(StatTesting) + "%");
            DictSourceDetails.Add("<95%", "<" + Convert.ToString(StatTesting) + "%");
            //start - changes done by bramhanath
            DictSourceDetails.Add("Benchmark: Family Dollar", "Benchmark: " + Benchlist1 + BenchChannel0);
            DictSourceDetails.Add("Comparisons: Dollar General, 99 Cents Only Store, ALDI & Dollar Tree", "Comparisons: " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + ", " + complist5 + compchannel4 + " & " + complist7 + compchannel6);
            DictSourceDetails.Add("Comparisons: Dollar General, Dollar Tree & 7-Eleven", "Comparisons: " + complist1 + compchannel0 + ", " + complist3 + compchannel2 + " & " + complist5 + compchannel4);
            DictSourceDetails.Add("Comparisons: Dollar General, 99 Cents Only Store", "Comparisons: " + complist1 + compchannel0 + "; " + complist3 + compchannel2);

            DictSourceDetails.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            DictSourceDetails.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            DictSourceDetails.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");

            DictSourceDetails.Add("Benchmark1 Leads", Benchlist1 + BenchChannel0 + " Leads");
            DictSourceDetails.Add("Comparision1 Leads", complist1 + compchannel0 + " Leads");
            DictSourceDetails.Add("Comparision2 Leads", complist3 + compchannel2 + " Leads");
            DictSourceDetails.Add("Comparision3 Leads", complist5 + compchannel4 + " Leads");
            DictSourceDetails.Add("Comparision4 Leads", complist7 + compchannel6 + " Leads");
            if (dst != null && dst.Tables.Count > 0)
            {
                if (ReportNumber == 9)
                {
                    DictSourceDetails.Add("Kroger (2705)", Benchlist1 + BenchChannel0 + AppendStars(Benchlist1) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("Publix (2012)", complist1 + compchannel0 + AppendStars(complist1) + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
                    DictSourceDetails.Add("Whole Foods (385)", complist3 + compchannel2 + AppendStars(complist3) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
                    DictSourceDetails.Add("Walmart (2000)", complist5 + compchannel4 + AppendStars(complist5) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
                    DictSourceDetails.Add("Target (3000)", complist7 + compchannel6 + AppendStars(complist7) + " (" + GetSampleSize(dst.Tables[0], complist7) + ")");

                    DictSourceDetails.Add("Family", Benchlist1 + BenchChannel0 + AppendStars(Benchlist1) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("Dollar General (2670)", complist1 + compchannel0 + AppendStars(complist1) + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
                    DictSourceDetails.Add("99 Cents Only Store (1567)", complist3 + compchannel2 + AppendStars(complist3) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
                }
                else
                {
                    DictSourceDetails.Add("Kroger (2705)", Benchlist1 + BenchChannel0 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("Publix (2012)", complist1 + compchannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
                    DictSourceDetails.Add("Whole Foods (385)", complist3 + compchannel2 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
                    DictSourceDetails.Add("Walmart (2000)", complist5 + compchannel4 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
                    DictSourceDetails.Add("Target (3000)", complist7 + compchannel6 + " (" + GetSampleSize(dst.Tables[0], complist7) + ")");

                    DictSourceDetails.Add("Family", Benchlist1 + BenchChannel0 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("Dollar General (2670)", complist1 + compchannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");
                    DictSourceDetails.Add("99 Cents Only Store (1567)", complist3 + compchannel2 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
                    DictSourceDetails.Add("99 Cents Only Store (1568)", complist5 + compchannel4 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");

                    DictSourceDetails.Add("Dollar General (267048)", Benchlist1 + BenchChannel0 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("99 Cents Only Store (156748)", complist1 + compchannel0 + " (" + GetSampleSize(dst.Tables[0], complist1) + ")");

                    DictSourceDetails.Add("Dollar General (267049)", Benchlist1 + BenchChannel0 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                    DictSourceDetails.Add("99 Cents Only Store (156749)", complist3 + compchannel2 + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");                  
                }

                //DictSourceDetails.Add("14%", Convert.ToString(Math.Round(Convert.ToDouble(dst.Tables[0].Rows[0]["Volume"]), 0)) + "%");
                //DictSourceDetails.Add("10%", Convert.ToString(Math.Round(Convert.ToDouble(dst.Tables[0].Rows[1]["Volume"]), 0)) + "%");
                //DictSourceDetails.Add("7%", Convert.ToString(Math.Round(Convert.ToDouble(dst.Tables[0].Rows[2]["Volume"]), 0)) + "%");
                //DictSourceDetails.Add("3%", Convert.ToString(Math.Round(Convert.ToDouble(dst.Tables[0].Rows[3]["Volume"]), 0)) + "%");
            }

            DictSourceDetails.Add("Dollar1", Benchlist1 + BenchChannel0);
            DictSourceDetails.Add("Family2", complist3 + compchannel2);

            DictSourceDetails.Add("Absolute Difference with Family Dollar: Destination Items", "Absolute Difference with " + Benchlist1 + BenchChannel0 + ": Destination Items");
            DictSourceDetails.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + BenchChannel0 + " ");

            DictSourceDetails.Add("What they do in the store?", "What they do in the store? ");

            DictSourceDetails.Add("Trip Net", "Items Purchased on Trip (Net)");

            DictSourceDetails.Add("Absolute Difference with Family Dollar: Items Purchased", "Absolute Difference with " + Benchlist1 + BenchChannel0 + ": Items Purchased");
            DictSourceDetails.Add("Absolute Difference with Family Dollar: Impulse Items", "Absolute Difference with " + Benchlist1 + BenchChannel0 + ": Impulse Items");

            DictSourceDetails.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Attitudinal Segment of <filter> Shoppers", "Attitudinal Segment of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");


            DictSourceDetails.Add("Monthly + Shoppers of Benchmark", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(Benchlist1) + BenchChannel0);//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[0]["Objective"]))));
            DictSourceDetails.Add("Monthly + Shoppers of Comparison1", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist1) + compchannel0);//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[1]["Objective"]))));
            DictSourceDetails.Add("Monthly + Shoppers of Whole Foods", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + complist3 + compchannel2);
            DictSourceDetails.Add("Monthly + Shoppers of Comparison3", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + complist3 + compchannel2);
            DictSourceDetails.Add("Monthly + Shoppers of Comparison5", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + complist5 + compchannel4);
            DictSourceDetails.Add("Monthly + Shoppers of Comparison7", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + complist7 + compchannel6);

            DictSourceDetails.Add("Cross Shopping Behavior of Weekly+ Shoppers", "Cross Retailer Shopping Behavior of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            DictSourceDetails.Add("(Weekly + Shopper of Retailer/Channel) / (Total Shoppers)", "(" + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shopper of Retailer/Channel) / (Total Shoppers)");

            DictSourceDetails.Add("<filter> Shoppers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by top 10 retailers");
            DictSourceDetails.Add("Text1", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            DictSourceDetails.Add("Text2", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            DictSourceDetails.Add("Text3", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            DictSourceDetails.Add("Weekly+ Shoppers", reportparams.ShopperFrequencyShortName.ToString() + " Shoppers ");
            DictSourceDetails.Add("% of Monthly + Shoppers by Top 10 Retailers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by Top 10 Retailers");

            DictSourceDetails.Add("Top 10 categories purchased among  Weekly+ shoppers within Retailer", "Top 10 categories purchased among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            DictSourceDetails.Add("Top 10 Categories Purchased Among Weekly+ Shoppers", "Top 10 Categories Purchased Among " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");

            DictSourceDetails.Add("Top 10 Brands among  Weekly+ shoppers within Retailer", "Top 10 Brands among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            DictSourceDetails.Add("Top Brands Purchased Among Weekly+ Shoppers", "Top Brands Purchased Among  " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            DictSourceDetails.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");

            //DictSourceDetails.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist1 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //DictSourceDetails.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + Benchlist1 + " and " + complist1 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //DictSourceDetails.Add("Family1", Benchlist1);
            //DictSourceDetails.Add("Dollar2", complist1);

            //DictSourceDetails.Add("99 Cents Only Store", complist3 + compchannel2);

            DictSourceDetails.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            DictSourceDetails.Add("roduct imagery of Weekly+ shoppers", "roduct imagery of " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");

            //DictSourceDetails.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist1 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //DictSourceDetails.Add("Difference in product imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in product imagery between " + Benchlist1 + " and " + complist1 + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");

            DictSourceDetails.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            DictSourceDetails.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");

            return DictSourceDetails;
            //end
        }                
    }
}