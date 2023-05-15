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
    public class Download_ShopperReportForWithin : BaseReport
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
            if (reportparams.ModuleBlock.Equals("WithinShopper", StringComparison.OrdinalIgnoreCase))
            {
                preferences = new List<String> { "SUMMARY", "FREQUENTSHOPPER", "CROSSRETAILERSHOPPER",
                                                          "SHOPPERPERCEPTION", "BEVERAGEINTERACTION", "APPENDIX" };            
            }
            else
            {
                preferences = new List<String> { "SUMMARY", "VISITORPROFILE", "TRIPTYPE", "PRESHOP",
                                                          "INSTORE", "TRIPSUMMARY","APPENDIX" };
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

                        if (ds != null && ds.Tables.Count > 0)
                        {
                            if (complist.Count() == 4 || complist.Count() == 6 || complist.Count() == 8)
                            {
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
                                            //_fileDetails = Build_2_Comparison_PreShop_Slides(ds, chkComparisonFolderNumber);
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
                            }
                            else if (complist.Count() == 2) //This if code executes when Selector is 1 and comparator is 1. (i.e. 1 to 1)
                            {
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
        #region 1 Comparison Slides
        #region 1 Comparison Summary Slides
        private FileDetails Build_1_Comparison_Summary_Slides()
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            List<string> strRetailersList = new List<string>();
            strRetailersList.Add(shopseg[1]);

            string[] destinationFilePath;
            //Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 retailer Demographic_summary_V0.2");
            if (reportparams.ModuleBlock.Equals("WithinShopper", StringComparison.OrdinalIgnoreCase))
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Within - Across Shoppers\iShop RG New - 2 Retailer_Report_V5.1");
            else
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Within - Compare Path Purchase\iShop RG New - 2 Retailer_Report_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Retailer Summary");
            destination_FilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            Dictionary<string, int> replacekeyvalue = new Dictionary<string, int>();
            FileDetails _fileDetails = new FileDetails();

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Male ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " ");
            //slide.ReplaceText.Add("  Female", " " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparison Across Gender", "Comparison Across " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[0])));

            slide.ReplaceText.Add("Retailer: Family Dollar", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparison: Female", "Comparison: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 2
            slide = new SlideDetails();

            //Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" :  Convert.ToString(reportparams.ShopperFrequencyShortName)

            //start- added by bramhanath for New Slide Changes
            slide.ReplaceText.Add("95%", StatTesting + "%");
            slide.ReplaceText.Add("Monthly+ ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName).Replace("NONE", "")) + " ");
            slide.ReplaceText.Add("BENCHMARK ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("COMPARISON1 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("COMPARISON1 (", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (");
            slide.ReplaceText.Add("BENCHMARK and COMPARISON1 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("RETAILER ", shopseg1 + shopseg0 + " ");
            slide.ReplaceText.Add("BENCHMARK and ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and ");
            slide.ReplaceText.Add("This report compares RETAILER Monthly+ BENCHMARK Shoppers to the Monthly+ COMPARISON1 ", "This report compares " + shopseg1 + shopseg0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Shoppers to the " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("Trips taken to RETAILER by ", "Trips taken to " + shopseg1 + shopseg0 + " by ");
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as a ", "All statistical testing is compared against " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " as a ");
            slide.ReplaceText.Add("group(s) COMPARISON1 is only ", "group(s) " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " is only ");
            slide.ReplaceText.Add("RETAILER Monthly+ Frequent Shopper’s who are ", shopseg1 + shopseg0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + "Frequent Shopper’s who are ");
            //end
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            files.ReplaceImages = AddRetailerImages(strRetailersList);
            fileName = ReportNumber + ".Retailer Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Visitor Profile Slides
        private FileDetails Build_1_Comparison_Visitor_Profile_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();


            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_visitor_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Visitor Profile");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female ", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
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
            lstSize.Add("657087");
            lstSize.Add("540331");
            lstSize.Add("16");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Demographic Segments");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHIncomeGroups", "1", 5, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHChildren", "1", 6, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Visitor Profile";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Trip Type Slides
        private FileDetails Build_1_Comparison_Trip_Type_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_kind of trip_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Type");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female ", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripMission", "1", 9, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Type";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison PreShop Slides
        private FileDetails Build_1_Comparison_PreShop_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_before visit_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
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

            List<string> lstSize = new List<string>();
            lstSize.Add("795922");
            lstSize.Add("543954");
            lstSize.Add("10");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key Pre-Shop Measures");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 6", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

           slide.SlideNumber = GetSlideNumber();
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "PreTripOrigin", "1", 12, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "WeekdayNet", "1", 13, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1",14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitPlans", "1",14, 1));
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist1));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "MetricItem";
            chart.YAxisColumnName = "Volume";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#376092"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#595959"));
            chart.BarHexColors = colorlist;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "VisitMotiviations", "1",15, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);

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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1", 17, 1), "GapAnalysis");

            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1", 17, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                dst = geods.Copy();
                slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Female", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Comparision1 Leads", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " Leads");
                slide.ReplaceText.Add("Benchmark1 Leads", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Leads");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

           // //Slide 9
           // slide = new SlideDetails();
           // slide.ReplaceText = GetSourceDetail("Trips", shopseg0);

           // tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10", 18, 1);
           // var query2 = from r in tbl.AsEnumerable()
           //              select r.Field<object>("Objective");
           // appendixcolumnlist = query2.Distinct().ToList();
           // tbl = CreateAppendixTable(tbl);
           // GetTableHeight_FontSize(tbl);
           // columnwidth = new List<string>();
           // for (int i = 0; i < appendixcolumnlist.Count; i++)
           // {
           //     columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
           // }
           //  xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
           // UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
           //slide.SlideNumber = GetSlideNumber();
           // slidelist.Add(slide);

            //slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1", 18, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Absolute Difference with Male: Destination Items", "Absolute Difference with " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ": Destination Items");
            slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {              
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison In Store Slides
        private FileDetails Build_1_Comparison_In_Store_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_in the store_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".In Store");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            List<Color> colorlist = new List<Color>();
            Dictionary<string, object> tablecolumnlist = new Dictionary<string, object>();
            List<string> columnlist = new List<string>();

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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
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
            lstSize.Add("276380");
            lstSize.Add("10");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key In-Store Measures");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlide(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "InStoreDestinationDetails Top 10", "Top 10 Impulse Items" };
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 18;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Smartphone/TabletInfluencedPurchases", "1", 21, 1));
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Trip Net", "Items Purchased on Trip (Net)");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = 5;
            //slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "InStoreDestinationDetails", "1", 23, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Absolute Difference with Family Dollar: Items Purchased", "Absolute Difference with " + Benchlist1 + ": Items Purchased");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            ////Slide 7 
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //tablecolumnlist = new Dictionary<string, object>();
            //columnlist = new List<string>() { " ", " " };
            //tablecolumnlist.Add("Reason for store choice", Benchlist[1]);
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = 7;
            //slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slide.ReplaceText.Add("Absolute Difference with Family Dollar: Impulse Items", "Absolute Difference with " + Benchlist1 + ": Impulse Items");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            DataSet tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            tempdst = new DataSet();
            List<object> appendixcolumnlist = new List<object>();
            List<string> tablemetriclist = new List<string>();
            DataSet dsttop10 = GetSlideTables(ds, "BeveragepurchasedMonthly", "1", 28, 1);
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

            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
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
            UpdateAppendixMultipleTables(xmlpath, tempdst, appendixcolumnlist, "Table 11", rowheight.ToString(), columnwidth, "");

            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Family", Benchlist1 + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
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
            //        chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Trip Summary Slides
        private FileDetails Build_1_Comparison_Trip_Summary_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_trip summary_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Summary");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TimeSpent", "1", 32, 1));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            dst = FilterData(GetSlideTables(ds, "Average Time Spent (In HH:MM)", "1", 32, 4));
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "RedeemedCouponTypes", "1", 33, 2));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationStoreTrip", "1", 34, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripSatisfaction", "1", 35, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);


             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Frequent Shopper Slides (Frequest Shopper)
        private FileDetails Build_1_Comparison_Shopper_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_frequent shopper_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Frequent Shopper");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female ", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            slide.ReplaceText.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHIncomeGroups", "1", 5, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "HHChildren", "1", 6, 4));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "Attitudinal Segment", "1",7,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("Attitudinal Segment of <filter> Shoppers", "Attitudinal Segment of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Frequent Shopper";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Cross Retailer Slides
        private FileDetails Build_1_Comparison_Retailer_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_frequently go_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Cross Retailer");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            DataSet dstemp = ds.Copy();
            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            slide.ReplaceText.Add("Base: All Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Shopper Frequency2");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("544124");
            lstSize.Add("20");

            lstHeaderText = new List<string>();
            lstHeaderText.Add(Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            tbl = new DataTable();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dstTemp = FilterData(GetSlideTables(ds, "Shopper Frequency2", "1",10,2));
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

            slide.ReplaceText.Add("14%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[0]["Volume"]), 1)) + "%");
            slide.ReplaceText.Add("10%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[1]["Volume"]), 1)) + "%");

            slide.ReplaceText.Add("Monthly + Shoppers of Benchmark", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(Benchlist1));
            slide.ReplaceText.Add("Monthly + Shoppers of Comparison1", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist1));
            slide.ReplaceText.Add("Cross Shopping Behavior of Weekly+ Shoppers", "Cross Retailer Shopping Behavior of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");

            dst = GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2);
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "Retailer");

            dstemp = GetSlideTables(ds, "Shopper Frequency1", "1", 10, 1);
            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), Benchlist[1]);
            slide.ReplaceText.Add("Male : 48%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + shopseg1 + " & " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ": " + (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.IsBarHexColorForSeriesPoints = false;
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText.Add("Female : 52%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + shopseg1 + shopseg0 + " & " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ": " + (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%");
            slide.ReplaceText.Add("<filter> Shoppers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by top 10 retailers");
            slide.ReplaceText.Add("Text1", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            slide.ReplaceText.Add("Text2", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            slide.ReplaceText.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");

            slide.ReplaceText.Add("% of Monthly + Shoppers by Top 10 Benchmark", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by Top 10 Benchmark");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Cross Retailer";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Shopper Perception Slides
        private FileDetails Build_1_Comparison_Perception_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_shopper perceve_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            slide.ReplaceText.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "StoreAttributesFactors", "1",13,1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("Store Associations of Monthly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Sub Title", "Store associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            slide.ReplaceText.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            //slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = 4;
            //slidelist.Add(slide);
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
            //slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparision1 Leads", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " Leads");
            slide.ReplaceText.Add("Benchmark1 Leads", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Leads");
            slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "GoodPlaceToShopFactors", "1", 16, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
            {
                slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            }
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            //slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = 7;
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between Female and Male for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparision1 Leads", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " Leads");
            slide.ReplaceText.Add("Benchmark1 Leads", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Leads");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "MainFavoriteStore", "1", 19, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            //slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among Trade Area Shoppers");
            slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Beverage Intration Slides
        private FileDetails Build_1_Comparison_Beverage_Interaction_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_beverage intraction_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Beverage Interaction");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
            slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
            slide.ReplaceText.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
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
            tblRes = GetSummaryTablesData(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("403901");
            lstSize.Add("350901");
            lstSize.Add("10");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Beverages Consumed Monthly");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
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
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            slidelist.Add(slide);

            //Slide 3
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.SlideNumber = 3;
            //slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            slide.SlideNumber = GetSlideNumber();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "BeveragepurchasedMonthly", "1",23, 1,true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
            //slide.ReplaceText.Add("Top 10 categories purchased among  Weekly+ shoppers within Retailer", "Top 10 categories purchased among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            slide.ReplaceText.Add("Top 10 Categories Purchased Among Weekly+ Shoppers", "Top 10 Categories Purchased Among " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");          
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
                    tempdst = FilterData(GetSlideTables(ds, Convert.ToString(top10[i]), "1", tbl_slide_no, 1,true));
                    tbl_slide_no++;
                    slide = new SlideDetails();
                    if (tempdst != null && tempdst.Tables.Count > 0)
                    {
                        chart = new ChartDetails();
                        chart.Type = ChartType.BAR;
                        chart.ShowDataLegends = false;
                        chart.DataLabelFormatCode = "0.0%";
                        chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
                        chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
                        chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(tempdst));
                        chart.Title = Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Trim();
                        chart.XAxisColumnName = "Objective";
                        chart.YAxisColumnName = "Volume";
                        chart.MetricColumnName = "MetricItem";
                        chart.ColorColumnName = "Significance";
                        chart.TextColor = lststatcolour;
                        slide.Charts.Add(chart);
                        slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
                        slide.ReplaceText.Add("Top 10 Brands among  Weekly+ shoppers within Retailer", "Top 10 Brands among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
                        slide.ReplaceText.Add("Male", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(tempdst.Tables[0], Benchlist1) + ")");
                        slide.ReplaceText.Add("Female ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(tempdst.Tables[0], complist1) + ")");

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
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 1 Comparison Appendix Slides
        private FileDetails Build_1_Comparison_Appendix_Slides(DataSet ds)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();

            string[] destinationFilePath;
            if (reportparams.ModuleBlock == "WithinShopper")
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_appendix_V0.2");
            }
            else
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\2-Comparisons\2 Demographic_appendix_V0.2 - Trips");
            }
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Appendix");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
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

            if (reportparams.ModuleBlock == "WithinShopper")
            {
                //Slide 1
                slide = new SlideDetails();
                slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
                slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
                slide.ReplaceText.Add("Weekly+ Shoppers", Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
               slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                //slide 2
                //slide = new SlideDetails();
                //slide.SlideNumber = 2;
                //slide.ReplaceText = GetSourceDetail("Trips", shopseg0);

                //tbl = Get_Chart_Table(ds, "ReasonForStoreChoice");
                //var query = from r in tbl.AsEnumerable()
                //            select r.Field<object>("Objective");
                //appendixcolumnlist = query.Distinct().ToList();
                //tbl = CreateAppendixTable(tbl);
                //objectivecolumnlist = GetColumnlist(tbl);
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
                slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
                tbl = Get_Chart_Table(ds, "StoreAttribute", 35, 1);
                var query2 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query2.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                objectivecolumnlist = GetColumnlist(tbl);
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
                slide.ReplaceText = GetSourceDetail("Shoppers", shopseg0);
                tbl = Get_Chart_Table(ds, "GoodPlaceToShop", 36, 1);
                var query3 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query3.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                objectivecolumnlist = GetColumnlist(tbl);
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
                slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
                slide.ReplaceText.Add("Benchmark: Male", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Comparisons: Female", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
                slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: All Trips");
               slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                //slide 2
                slide = new SlideDetails();
               slide.SlideNumber = GetSlideNumber();
                slide.ReplaceText = GetSourceDetail("Trips", shopseg0);
                tbl = Get_Chart_Table(ds, "ReasonForStoreChoice", 37, 1);
                var query3 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query3.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                objectivecolumnlist = GetColumnlist(tbl);
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
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #endregion
        #region 2 Comparison Slides

        #region 2 Comparison Summary Slides
        private FileDetails Build_2_Comparison_Summary_Slides()
        {
            string destpath, tempdestfilepath, Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            string chkComparisonFolderNumber = string.Empty;

            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]).Trim();

            List<string> strRetailersList = new List<string>();
            strRetailersList.Add(shopseg[1]);
            if (complist.Length > 7)
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();

                chkComparisonFolderNumber = "5";
                strRetailersList.Add(complist[7]);
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();

                chkComparisonFolderNumber = "4";
            }
            else
            {
                complist7 = "";
                complist5 = "";

                chkComparisonFolderNumber = "3";
            }
            string[] destinationFilePath;
            //Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_summary_V0.2");
            if (reportparams.ModuleBlock.Equals("WithinShopper", StringComparison.OrdinalIgnoreCase))
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Within - Across Shoppers\iShop RG New - " + chkComparisonFolderNumber + " Retailer_Report_V5.1");
            else
                Source = HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Within - Compare Path Purchase\iShop RG New - " + chkComparisonFolderNumber + " Retailer_Report_V5.1");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Retailer Summary");
            destination_FilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataSet ds = new DataSet();
            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("16-18 Years ; 19-24 Years ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " ; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " ");
            //slide.ReplaceText.Add(" 25-34 Years", " " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparison Across Gender", "Comparison Across " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[0])));

            //slide.ReplaceText.Add("Channel/Retailer: Family Dollar", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + Benchlist1);
            //slide.ReplaceText.Add("Comparisons: 16-18 Years & 19-24 Years", "Comparisons: " + complist1 + " & " + complist3);
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, ds, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);



            //Slide 2
            slide = new SlideDetails();
            //start- added by bramhanath for New Slide Changes
            slide.ReplaceText.Add("95%", StatTesting + "%");
            slide.ReplaceText.Add("RETAILER ", shopseg1 + shopseg0 + " ");
            slide.ReplaceText.Add("Monthly+ ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " ");
            slide.ReplaceText.Add("Monthly+ Frequent ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent "));
            slide.ReplaceText.Add("BENCHMARK ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("This report compares RETAILER Monthly+ BENCHMARK Shoppers to the Monthly+ COMPARISON1 ", "This report compares " + shopseg1 + shopseg0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Shoppers to the " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("COMPARISON1 and COMPARISON3 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 and COMPARISON3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));

            slide.ReplaceText.Add("and COMPARISON3 ", "and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("Trips taken to RETAILER by ", "Trips taken to " + shopseg1 + shopseg0 + " by ");
            slide.ReplaceText.Add("RETAILER Monthly+ Frequent Shopper’s who are ", shopseg1 + shopseg0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s who are " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s who are "));
            slide.ReplaceText.Add("COMPARISON3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 and ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " and ");//+ (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " and " + (complist0.Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " and");
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as a benchmark", "All statistical testing is compared against " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " as a benchmark");
            slide.ReplaceText.Add("COMPARISON1(Blue) and COMPARISON3(Red) ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "(Blue) and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + "(Red) ");
            slide.ReplaceText.Add("are only compared against BENCHMARK and ", "are only compared against " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and ");

            //3-comp
            slide.ReplaceText.Add("Monthly+ Frequent Shopper’s who are ", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "Frequent Shopper’s who are " : Convert.ToString(reportparams.ShopperFrequencyShortName) + " Frequent Shopper’s who are "));
            //


            //4-comp
            slide.ReplaceText.Add("group(s) COMPARISON1 ", "group(s) " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 , COMPARISON3 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("COMPARISON1 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("This report compares RETAILER Monthly+ BENCHMARK Shoppers to the Monthly+ ", "This report compares " + shopseg1 + shopseg0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Shoppers to the " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " "));
            slide.ReplaceText.Add("COMPARISON1 , COMPARISON3 and COMPARISON5 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 , COMPARISON3 and ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " and ");
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as a ", "All statistical testing is compared against " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " as a ");
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as ", "All statistical testing is compared against " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " as ");
            slide.ReplaceText.Add("All statistical testing is compared against BENCHMARK as", "All statistical testing is compared against " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " as");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 , COMPARISON3 and COMPARISON5", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")));
            slide.ReplaceText.Add("COMPARISON5", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")));
            slide.ReplaceText.Add("(Blue), COMPARISON3 (Red", "(Blue), " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + "(Red");
            slide.ReplaceText.Add(") and COMPARISON5 ", ") and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("only compared against BENCHMARK and ", "only compared against " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and ");
            //

            //5-comp
            slide.ReplaceText.Add("BENCHMARK as a ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " as a ");
            slide.ReplaceText.Add("and COMPARISON7 ", "and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("COMPARISON3 (Red), COMPARISON5 (Purple) ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + "(Red), " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + "(Purple) ");
            slide.ReplaceText.Add("COMPARISON7", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")));
            slide.ReplaceText.Add("This report compares RETAILER Monthly+ BENCHMARK Shoppers to the Monthly+ COMPARISON1 , COMPARISON3 , COMPARISON5 and COMPARISON7 shoppers across the key metrics in the iSHOP survey.", "This report compares " + shopseg1 + shopseg0 + " " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Shoppers to the " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName) == "NONE" ? "" : Convert.ToString(reportparams.ShopperFrequencyShortName) + " ") + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " shoppers across the key metrics in the iSHOP survey.");
            slide.ReplaceText.Add("COMPARISON1 , COMPARISON3 , COMPARISON5 and COMPARISON7 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 , COMPARISON3, COMPARISON5 and ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " and ");
            slide.ReplaceText.Add("BENCHMARK , COMPARISON1 , COMPARISON3, COMPARISON5 and COMPARISON7", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + ", " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")));
            slide.ReplaceText.Add("COMPARISON3 (Red", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + "(Red");
            slide.ReplaceText.Add("COMPARISON5 (Purple) ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + "(Purple) ");
            slide.ReplaceText.Add("COMPARISON7 ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " ");
            //
            //end
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            files.ReplaceImages = AddRetailerImages(strRetailersList);
            fileName = ReportNumber + ".Retailer Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Visitor Profile Slides bb
        private FileDetails Build_2_Comparison_Visitor_Profile_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');

            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Retailer Demographic_visitors_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Visitor Profile");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            DataSet dstemp = ds.Copy();
            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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

            lstSize.Add("657087");
            lstSize.Add("540331");
            lstSize.Add("16");

            lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Demographic Segments");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Visitor Profile";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Trip Type Slides
        private FileDetails Build_2_Comparison_Trip_Type_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_kind of trip_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Type");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            DataSet dstemp = ds.Copy();
            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            Dictionary<string, int> replacekeyvalue = new Dictionary<string, int>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripMission", "1", 9, 1));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Type";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison PreShop Slides
        private FileDetails Build_2_Comparison_PreShop_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_before visit_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataTable tbl = new DataTable();
            List<Color> colorlist = new List<Color>();
            List<object> columnlist = new List<object>();
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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 6", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

           slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1"), "GapAnalysis");

            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1", 17, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice1", "1"), "GapAnalysis");
            if (objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + shopseg0 + " Age Group" : Benchlist1.Replace("&", "&amp;") + shopseg0));
            slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + shopseg0 + " Age Group" : Benchlist1.Replace("&", "&amp;") + shopseg0) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

           // //Slide 10
           // slide = new SlideDetails();
           // //slide.ReplaceText = GetSourceDetail("Trips");
           // slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

           // tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10", 19, 1);
           // var query2 = from r in tbl.AsEnumerable()
           //              select r.Field<object>("Objective");
           // appendixcolumnlist = query2.Distinct().ToList();
           // tbl = CreateAppendixTable(tbl);
           // GetTableHeight_FontSize(tbl);
           // columnwidth = new List<string>();
           // for (int i = 0; i < appendixcolumnlist.Count; i++)
           // {
           //     columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
           // }
           //  xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
           // UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
           //slide.SlideNumber = GetSlideNumber();
           // slidelist.Add(slide);

            //slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1", 19, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with 25-34 Years: Destination Items", "Absolute Difference with " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ": Destination Items");
            //slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison In Store Slides
        private FileDetails Build_2_Comparison_In_Store_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_in the store_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".In Store");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("353901");
            lstSize.Add("276380");
            lstSize.Add("10");

            List<string> lstHeaderText = new List<string>();
            lstHeaderText.Add("Comparing Key In-Store Measures");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
            //added by Nagaraju 05-02-2015
            metriclist = new List<string>() { "InStoreDestinationDetails Top 10", "Top 10 Impulse Items" };
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 18;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "Smartphone/TabletInfluencedPurchases", "1", slide.SlideNumber, 1));
            chart.Data = chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

         
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 18;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Trip Net", "Items Purchased on Trip (Net)");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

          
            slidelist.Add(slide);

            //slide 5
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "InStoreDestinationDetails Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = 5;
            //slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
            dst = FilterData(GetSlideTables(ds, "InStoreDestinationDetails", "1", slide.SlideNumber, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Absolute Difference with Family Dollar: Items Purchased", "Absolute Difference with " + Benchlist1 + ": Items Purchased");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
          
            slidelist.Add(slide);

            ////Slide 7 
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
            //chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "ImpulseItem Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 1, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFiles\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //tablecolumnlist = new Dictionary<string, object>();
            //columnlist = new List<string>() { " ", " " };
            //tablecolumnlist.Add("Reason for store choice", Benchlist[1]);
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[1])), "Table 24", 1, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Family", Benchlist1 + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Dollar General (2670)", complist1 + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.SlideNumber = 7;
            //slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Absolute Difference with Family Dollar: Impulse Items", "Absolute Difference with " + Benchlist1 + ": Impulse Items");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
        
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            DataSet tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            tempdst = new DataSet();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");


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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            tempdst = new DataSet();
            appendixcolumnlist = new List<object>();

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
            UpdateAppendixMultipleTables(xmlpath, tempdst, appendixcolumnlist, "Table 11", rowheight.ToString(), columnwidth, "");

            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

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
            //        chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Trip Summary Slides
        private FileDetails Build_2_Comparison_Trip_Summary_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_trip summary_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Trip Summary");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TimeSpent", "1", (slide_Number + 1), 1));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            //----------->
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "TripExpenditure", "1", (slide_Number + 1), 2));
            chart.Data = CleanXMLBeforeBind(FilterDataForTrip(ValidateSingleDatatable(dst)).Tables[0]);
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);

            //--------->
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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

            //----------->
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

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

            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);


             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Trip Summary";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Frequent Shopper Slides (Frequest Shopper)
        private FileDetails Build_2_Comparison_Shopper_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_frequent shopper_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Frequent Shopper");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
                chart.SizeOfText = 7;
            }
            else
            {
                chart.SizeOfText = 10;
            }
            chart.ShowDataLegends = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 4
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Attitudinal Segment of <filter> Shoppers", "Attitudinal Segment of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Frequent Shopper";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Cross Retailer Slides
        private FileDetails Build_2_Comparison_Retailer_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_frequently go_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Cross Retailer");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            DataSet dstemp = ds.Copy();
            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: All Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 2
            slide = new SlideDetails();

            List<string> lstMetricNames = new List<string>();
            lstMetricNames.Add("Shopper Frequency2");

            DataTable tblRes = new DataTable();
            tblRes = GetSummaryTablesDataFor2(ds, lstMetricNames, complist);

            List<string> lstSize = new List<string>();
            lstSize.Add("681338");
            lstSize.Add("544124");
            lstSize.Add("20");

            lstHeaderText = new List<string>();
            lstHeaderText.Add(Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));

             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
                chart.SizeOfText = 9;
            }
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dstTemp = FilterData(GetSlideTables(ds, "Shopper Frequency2", "1", 10,2));
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

            slide.ReplaceText.Add("14%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[0]["Volume"]), 1)) + "%");
            slide.ReplaceText.Add("10%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[1]["Volume"]), 1)) + "%");
            slide.ReplaceText.Add("7%", Convert.ToString(Math.Round(Convert.ToDouble(dstemp.Tables[0].Rows[2]["Volume"]), 1)) + "%");

            //slide.ReplaceText.Add("Monthly + Shoppers of Benchmark", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(Benchlist1));//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[0]["Objective"]))));
            //slide.ReplaceText.Add("Monthly + Shoppers of Comparison1", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist1));//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[1]["Objective"]))));
            //slide.ReplaceText.Add("Monthly + Shoppers of Whole Foods", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + complist3);
            //slide.ReplaceText.Add("Cross Shopping Behavior of Weekly+ Shoppers", "Cross Shopping Behavior of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers"); 

            dst = GetSlideTables(ds, "Shopper Frequency2", "1", 10, 2);
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 3", 2, "Retailer");

            dstemp = GetSlideTables(ds, "Shopper Frequency1", "1", 10, 1);
            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), Benchlist[1]);
            slide.ReplaceText.Add("25-34 Years : 48%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + shopseg1 + " & " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ": " + (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.IsBarHexColorForSeriesPoints = false;
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist1));
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
            slide.ReplaceText.Add("16-18 Years : 52%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + shopseg1 + " & " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ": " + (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.IsBarHexColorForSeriesPoints = false;
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.XAxisColumnName = "Objective";
            //chart.YAxisColumnName = "Volume";
            //chart.MetricColumnName = "MetricItem";
            //colorlist = new List<Color>();
            //colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            //chart.BarHexColors = colorlist;
            //slide.Charts.Add(chart);

            UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 25", 2, "Retailer");

            tbl = GetSlideIndividualTable(ValidateSingleDatatable(dstemp), complist[3]);
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dstTemp, shopseg0);
            slide.ReplaceText.Add("19-24 Years: 38%", Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers of " + shopseg1 + " & " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + ": " + (Math.Round(Convert.ToDecimal(tbl.Rows[0]["Volume"]), 1)) + "%");
            //slide.ReplaceText.Add("<filter> Shoppers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by top 10 retailers");
            //slide.ReplaceText.Add("Text1", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            //slide.ReplaceText.Add("Text2", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            //slide.ReplaceText.Add("Text3", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            //slide.ReplaceText.Add("Weekly+ Shoppers ", reportparams.ShopperFrequencyShortName.ToString() + " Shoppers ");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");

            //slide.ReplaceText.Add("% of Monthly + Shoppers by Top 10 Benchmark", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by Top 10 Benchmark");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Cross Retailer";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Shopper Perception Slides
        private FileDetails Build_2_Comparison_Perception_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }

            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\3-Comparisons\3 Demographic_perceive store_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1 + );
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Sub Title", "Store associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            slide.ReplaceText.Add("Store Associations of Monthly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = 4;
            //slidelist.Add(slide);
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
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute0", "1"));

            geods = FilterData(GetSlideTables(ds, "StoreAttribute0", "1", 15, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if(objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 16-18 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute1", "1"));
            if (objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = 8;
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1"));
            geods = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1", 19, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 16-18 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop1", "1"));
            if (objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in product imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Favorites/Main Store among  Weekly+ shoppers", "Favorites/Main Store among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among Trade Area Shoppers");
            slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Beverage Interaction Slides
        private FileDetails Build_2_Comparison_Beverage_Interaction_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_beverage intraction_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Beverage Interaction");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //Slide 3
            //slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            //slide.SlideNumber = 3;
            //slidelist.Add(slide);

            //Slide 4
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            slide.SlideNumber = GetSlideNumber();
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
            ////slide.ReplaceText.Add("Top 10 categories purchased among  Weekly+ shoppers within Retailer", "Top 10 categories purchased among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            //slide.ReplaceText.Add("Top 10 Categories Purchased Among Weekly+ Shoppers", "Top 10 Categories Purchased Among " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");

            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
          
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
            //List<String> top2 = new List<String>() { "SSD Regular" };
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
                    tempdst = FilterData(GetSlideTables(ds, Convert.ToString(top10[i]), "1", tbl_slide_no, 1,true));
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
                        chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
                        chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
                        chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(tempdst));
                        chart.Title = Convert.ToString(tempdst.Tables[0].Rows[0]["Metric"]).Trim();
                        chart.XAxisColumnName = "Objective";
                        chart.YAxisColumnName = "Volume";
                        chart.MetricColumnName = "MetricItem";
                        chart.ColorColumnName = "Significance";
                        chart.TextColor = lststatcolour;
                        slide.Charts.Add(chart);
                        //slide.ReplaceText = GetSourceDetail("Shoppers");
                        slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, tempdst, shopseg0);
                        //slide.ReplaceText.Add("Top 10 Brands among  Weekly+ shoppers within Retailer", "Top 10 Brands among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
                        slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(tempdst.Tables[0], Benchlist1) + ")");
                        slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(tempdst.Tables[0], complist1) + ")");
                        slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(tempdst.Tables[0], complist3) + ")");

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
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 2 Comparison Appendix Slides
        private FileDetails Build_2_Comparison_Appendix_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            if (reportparams.ModuleBlock == "WithinShopper")
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_appendix_V0.2");
            }
            else
            {
                Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_appendix_V0.2 - Trips");
            }
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Appendix");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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

            if (reportparams.ModuleBlock == "WithinShopper")
            {
                //Slide 1
                slide = new SlideDetails();
                //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
                //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
                //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
                //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
                //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
                slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
               slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                //slide 2
                //slide = new SlideDetails();
                //slide.SlideNumber = 2;
                ////slide.ReplaceText = GetSourceDetail("Trips");
                //slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

                //tbl = Get_Chart_Table(ds, "ReasonForStoreChoice");
                //var query = from r in tbl.AsEnumerable()
                //            select r.Field<object>("Objective");
                //appendixcolumnlist = query.Distinct().ToList();
                //tbl = CreateAppendixTable(tbl);
                //objectivecolumnlist = GetColumnlist(tbl);
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
                slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
                tbl = Get_Chart_Table(ds, "StoreAttribute", slide.SlideNumber, 1);
                var query2 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query2.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                objectivecolumnlist = GetColumnlist(tbl);
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
                slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
                tbl = Get_Chart_Table(ds, "GoodPlaceToShop", slide.SlideNumber, 1);
                var query3 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query3.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                objectivecolumnlist = GetColumnlist(tbl);
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
                slide.ReplaceText.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
                slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
                slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
                slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));
                slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: All Trips");
                slide.SlideNumber = GetSlideNumber();
                slidelist.Add(slide);

                //slide 2
                slide = new SlideDetails();
               slide.SlideNumber = GetSlideNumber();
                //slide.ReplaceText = GetSourceDetail("Trips");
                slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
                tbl = Get_Chart_Table(ds, "ReasonForStoreChoice", slide.SlideNumber, 1);
                var query3 = from r in tbl.AsEnumerable()
                             select r.Field<object>("Objective");
                appendixcolumnlist = query3.Distinct().ToList();
                tbl = CreateAppendixTable(tbl);
                objectivecolumnlist = GetColumnlist(tbl);
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
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #endregion
        #region 3 Comparison Slides only for Preshop and Shopper Perception

        #region 3 Comparison PreShop Slides
        private FileDetails Build_3_Comparison_PreShop_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_before visit_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataTable tbl = new DataTable();
            List<Color> colorlist = new List<Color>();
            List<object> columnlist = new List<object>();
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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 6", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

           slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();          

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist5));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
           
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1"), "GapAnalysis");

            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1", 17, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice1", "1"), "GapAnalysis");
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")));
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

           // //Slide 11
           // slide = new SlideDetails();
           // //slide.ReplaceText = GetSourceDetail("Trips");
           // slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

           // tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10", 20, 1);
           // var query2 = from r in tbl.AsEnumerable()
           //              select r.Field<object>("Objective");
           // appendixcolumnlist = query2.Distinct().ToList();
           // tbl = CreateAppendixTable(tbl);
           // GetTableHeight_FontSize(tbl);
           // columnwidth = new List<string>();
           // for (int i = 0; i < appendixcolumnlist.Count; i++)
           // {
           //     columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
           // }
           //  xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
           // UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
           //slide.SlideNumber = GetSlideNumber();
           // slidelist.Add(slide);

            //slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1", 20, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with 25-34 Years: Destination Items", "Absolute Difference with " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ": Destination Items");
            //slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 3 Comparison Shopper Perception Slides
        private FileDetails Build_3_Comparison_Perception_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_perceive store_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Sub Title", "Store associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            //slide.ReplaceText.Add("Store Associations of Monthly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = 4;
            //slidelist.Add(slide);
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute0", "1"));

            geods = FilterData(GetSlideTables(ds, "StoreAttribute0", "1", 15, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist1 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in Store imagery between 16-18 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute1", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in Store imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute2", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in Store imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            ////slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            ////slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            ////slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            ////slide.SlideNumber = 8;
            ////slidelist.Add(slide);
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
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1"));
            geods = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1", 20, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist1 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in product imagery between 16-18 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add(" Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop1", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in product imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop2", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in product imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 13
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 14
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            ////slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among Trade Area Shoppers");
            //slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #endregion
        #region 4 Comparison Slides only for Preshop and Shopper Perception
        #region 4 Comparison PreShop Slides
        private FileDetails Build_4_Comparison_PreShop_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }
            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);

            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_before visit_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Pre Shop");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
            SlideDetails slide = new SlideDetails();
            ChartDetails chart = new ChartDetails();
            FileDetails _fileDetails = new FileDetails();
            DataTable tbl = new DataTable();
            List<Color> colorlist = new List<Color>();
            List<object> columnlist = new List<object>();
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

            //Slide 1
            slide = new SlideDetails();
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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


            UpdateSummaryTable(xmlpath, tbl, objectivecolumnlist, "Table 6", rowheight, columnwidth, "Measures", fontsize, Convert.ToString(ds.Tables[0].Rows[0]["Objective"]));

           slide.SlideNumber = GetSlideNumber();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);

            //slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //slide 5
            slide = new SlideDetails();

            chart = new ChartDetails();
            chart.Type = ChartType.PIE;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 7;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            chart.SizeOfText = 7;
            chart.IsBarHexColorForSeriesPoints = false;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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

            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.SizeOfText = 8;
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist7));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
           
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            //slide.ReplaceText = GetSourceDetail("Trips");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1"), "GapAnalysis");

            geods = FilterData(GetSlideTables(ds, "ReasonForStoreChoice0", "1", 17, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice1", "1"), "GapAnalysis");
            if (objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice2", "1"), "GapAnalysis");
            if (objectives != null && objectives.Count > 0)
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
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")));
            slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            slide.ReplaceText.Add("Dollar ", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 11
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "ReasonForStoreChoice3", "1"), "GapAnalysis");
            if (objectives != null && objectives.Count > 0)
            dst = CommonFunctions.GetComparisonGapanalysisData(geods, objectives[3], Benchlist1);
            chart.Data = CleanXMLBeforeBind(ReverseRowsInDataTable(GetSlideIndividualTable(ValidateSingleDatatable(dst), objectives[3])));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Volume";
            chart.YAxisColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            colorlist = new List<Color>();
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));
            colorlist.Add(System.Drawing.ColorTranslator.FromHtml("#4f6228"));
            chart.BarHexColors = dst.Tables.Count > 0 ? GetColorListForGapAnalysis(dst.Tables[0], Benchlist1, colorlist) : new List<Color> { Color.Transparent };
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Family", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Dollar", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

           // //Slide 12
           // slide = new SlideDetails();
           // //slide.ReplaceText = GetSourceDetail("Trips");
           // slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);

           // tbl = Get_Chart_Table(ds, "DestinationItemDetails Top 10", 21, 1);
           // var query2 = from r in tbl.AsEnumerable()
           //              select r.Field<object>("Objective");
           // appendixcolumnlist = query2.Distinct().ToList();
           // tbl = CreateAppendixTable(tbl);
           // GetTableHeight_FontSize(tbl);
           // columnwidth = new List<string>();
           // for (int i = 0; i < appendixcolumnlist.Count; i++)
           // {
           //     columnwidth.Add(Convert.ToString(top5_table_width / appendixcolumnlist.Count));
           // }
           //  xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
           // UpdateAppendixTable(xmlpath, tbl, appendixcolumnlist, "Table 22", rowheight.ToString(), columnwidth, "Destination Items");
           //slide.SlideNumber = GetSlideNumber();
           // slidelist.Add(slide);

            //slide 13
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            dst = FilterData(GetSlideTables(ds, "DestinationItemDetails", "1", 21, 1, true));
            chart.Data = CleanXMLBeforeBind(ValidateSingleDatatable(dst));
            chart.Title = dst.Tables.Count > 0 ? Convert.ToString(dst.Tables[0].Rows[0]["Metric"]).Trim() : string.Empty;
            chart.XAxisColumnName = "Objective";
            chart.YAxisColumnName = "Volume";
            chart.MetricColumnName = "MetricItem";
            chart.ColorColumnName = "Significance";
            chart.TextColor = lststatcolour;
            slide.Charts.Add(chart);
            //slide.ReplaceText = GetSourceDetail("Trips");
            //slide.ReplaceText.Add("Absolute Difference with 25-34 Years: Destination Items", "Absolute Difference with " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ": Destination Items");
            //slide.ReplaceText.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Trips", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Pre Shop";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
            return files;
        }
        #endregion

        #region 4 Comparison Shopper Perception Slides
        private FileDetails Build_4_Comparison_Perception_Slides(DataSet ds, string chkComparisonFolderNumber)
        {
            string destpath, tempdestfilepath, Benchlist1, complist1, complist3, complist5, complist7, complist0, shopseg1, shopseg0;
            string[] complist, filt, Benchlist, shopseg;
            complist = reportparams.Comparisonlist.Split('|');
            filt = reportparams.Filters.Split('|');
            Benchlist = reportparams.Benchmark.Split('|');
            shopseg = reportparams.ShopperSegment.Split('|');
            if (Convert.ToString(shopseg[0]) == "Channels")
            {
                shopseg0 = " Channel";
            }
            else
            {
                shopseg0 = "";
            }

            Benchlist1 = Get_ShortNames(Convert.ToString(Benchlist[1])).Trim();
            complist1 = Get_ShortNames(Convert.ToString(complist[1])).Trim();
            complist3 = Get_ShortNames(Convert.ToString(complist[3])).Trim();
            shopseg1 = Get_ShortNames(Convert.ToString(shopseg[1])).Trim();
            complist0 = Convert.ToString(complist[0]);
            if (complist.Length > 7)// checking 5-comparison 
            {
                complist7 = Get_ShortNames(Convert.ToString(complist[7])).Trim();
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else if (complist.Length > 5 && complist.Length < 7)// checking 4-comparison 
            {
                complist7 = "";
                complist5 = Get_ShortNames(Convert.ToString(complist[5])).Trim();
            }
            else
            {
                complist7 = "";
                complist5 = "";
            }
            string[] destinationFilePath;
            Source = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\" + chkComparisonFolderNumber + "-Comparisons\\" + chkComparisonFolderNumber + " Demographic_perceive store_V0.2");
            tempdestfilepath = CopyFilesToDestination(Source, ReportNumber + ".Shopper Perception");
            destinationFilePath = tempdestfilepath.Split('|');
           sPowerPointTemplatePath = destination_FilePath[0];
             destpath = destination_FilePath[1];

            ds = CleanXML(ds);
            DataSet dst = new DataSet();
            DataSet dstTemp = new DataSet();
            string xmlpath = string.Empty;
            //List<SlideDetails> slidelist = new List<SlideDetails>();
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
            //slide.ReplaceText.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1);
            //slide.ReplaceText.Add("Benchmark: 25-34 Years", "Benchmark: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            //slide.ReplaceText.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(strFilter) ? "NONE" : strFilter));
            //slide.ReplaceText.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            slide.ReplaceText = GetSourceDetailNew("", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
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
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            lstHeaderText.Add((Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " differs from " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
             xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateSummarySlideFor2Comp(xmlpath, tblRes, "Table 4", lstHeaderText, lstSize);
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
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slidelist.Add(slide);
            //Slide 3
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Sub Title", "Store associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers");
            //slide.ReplaceText.Add("Store Associations of Monthly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Store Associations of Weekly+ Shoppers", "Store Associations of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 4
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "StoreAttribute Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = 4;
            //slidelist.Add(slide);
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
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 5
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute0", "1"));

            geods = FilterData(GetSlideTables(ds, "StoreAttribute0", "1", 15, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist1 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in Store imagery between 16-18 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 6
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute1", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in Store imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            slide.ReplaceText.Add("Difference in Store imagery between Dollar Tree and Family Dollar for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 7
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute2", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in Store imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 8
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "StoreAttribute3", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in Store imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in Store imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in Store Imagery between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in Store Imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 9
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("‘Good Place to Shop for’ of Weekly+ Shoppers", "‘Good Place to Shop for’ of " + reportparams.ShopperFrequencyShortName.ToString() + " Shoppers");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 10
            slide = new SlideDetails();
            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dstTemp = FilterData(GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric"));
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), complist3));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //dst = GetSlideTables(ds, "GoodPlaceToShop Top 10", "Top 10 Metric");
            // xmlpath = HttpContext.Current.Server.MapPath(destpath + "/ppt/slides/slide" + (slide_Number + 1).ToString() + ".xml");
            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), Benchlist[1])), "Table 18", 2, "NonRetailer");

            //chart = new ChartDetails();
            //chart.Type = ChartType.BAR;
            //chart.ShowDataLegends = false;
            //chart.DataLabelFormatCode = "0.0%";
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            //chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //chart.Data = CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dstTemp), Benchlist1));
            //chart.Title = Convert.ToString(dstTemp.Tables[0].Rows[0]["Metric"]).Trim();
            //chart.XAxisColumnName = "Volume";
            //chart.YAxisColumnName = "MetricItem";
            //slide.Charts.Add(chart);

            //UpdateTableSlide(xmlpath, CleanXMLBeforeBind(GetSlideIndividualTable(ValidateSingleDatatable(dst), complist[3])), "Table 16", 2, "NonRetailer");
            //slide.ReplaceText = GetSourceDetail("Shoppers");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], Benchlist1) + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist1) + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + GetSampleSize(dstTemp.Tables[0], complist3) + ")");
            //slide.SlideNumber = 8;
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
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1"));
            geods = FilterData(GetSlideTables(ds, "GoodPlaceToShop0", "1", 21, 1), "GapAnalysis");
            //get gapanalysis comparisons
            objectives =CommonFunctions.GetGapanalysisComparisons(geods, Benchlist1, reportparams);
            //
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist1 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in product imagery between 16-18 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("16-18 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add(" Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 12
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop1", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in product imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 13
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop2", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in product imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 14
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.IsBarHexColorForSeriesPoints = true;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
            //dst = FilterData(GetSlideTables(ds, "GoodPlaceToShop3", "1"));
            if (objectives != null && objectives.Count > 0)
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
            //slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + Benchlist1 + " and " + complist3 + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
            //slide.ReplaceText.Add("Difference in product imagery between 19-24 Years and 25-34 Years for Weekly+ Shoppers ", "Difference in product imagery between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " for " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "Shoppers ");
            //slide.ReplaceText.Add("25-34 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("19-24 Years", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, geods, shopseg0);
            slide.ReplaceText.Add("Difference in ‘Good Place to Shop for’ between <Benchmark> and <Comparison> Weekly+ Shoppers ", "Difference in ‘Good Place to Shop for’ between " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " and " + (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers ");
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 15
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BAR;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
            chart.ExcelTemplatePath = HttpContext.Current.Server.MapPath(@"~\Templates/ReportGenerator\ReportGeneratorPPTFilesForWithin\Microsoft_Excel_Worksheet1");
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
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
            slide.ReplaceText.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");

           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

            //Slide 16
            slide = new SlideDetails();
            chart = new ChartDetails();
            chart.Type = ChartType.BARPYRAMID;
            chart.ShowDataLegends = false;
            chart.DataLabelFormatCode = "0.0%";
            chart.ChartXmlPath = HttpContext.Current.Server.MapPath(destpath + "/ppt/charts/chart" + GetChartNumber().ToString() + ".xml");
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
            ////slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            //slide.ReplaceText.Add("Retailer Loyalty Pyramid Among Trade Area Shoppers", "Retailer Loyalty Pyramid Among Trade Area Shoppers");
            //slide.ReplaceText.Add("Retailer Pyramid among  Weekly+ shoppers", "Retailer Pyramid among " + reportparams.ShopperFrequencyShortName.ToString() + " shoppers");
            //slide.ReplaceText.Add("Male1", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male2", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
            //slide.ReplaceText.Add("Male3", (Convert.ToString(complist[0]).Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
            slide.ReplaceText = GetSourceDetailNew("Shoppers", Benchlist1, complist0, complist1, complist3, complist5, complist7, shopseg1, dst, shopseg0);
           slide.SlideNumber = GetSlideNumber();
            slidelist.Add(slide);

             //FileDetails files = new FileDetails();
            files.PowerPointTemplatePath = sPowerPointTemplatePath;
            files.Slides = slidelist;
            fileName = ReportNumber + ".Shopper Perception";
            files.FileName = fileName.Replace(" ", string.Empty);
            files.ExcelTemplatePath = HttpContext.Current.Server.MapPath("~/Templates/ReportGenerator/ReportGeneratorPPTFilesForWithin/Microsoft_Excel_Worksheet1");
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
                string[] strBenchMark = reportparams.Benchmark.Split('|');
                foreach (DataRow row in tb.Rows)
                {
                    if (strBenchMark.Length > 1)
                    {
                        if (Convert.ToString(row["Objective"]).Equals(strBenchMark[1], StringComparison.OrdinalIgnoreCase))
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
        public Dictionary<string, string> GetSourceDetail(string stroption, string shopseg0)
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
            if (ReportNumber == 9)
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + AppendStars(Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1])))) + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + AppendStars(Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1])))) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }
            else if (ReportNumber == 11)
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + AppendStars(Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1])))) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }
            else
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }

            DictSourceDetails.Add(stroriginal, strResult);
            DictSourceDetails.Add("Source: iSHOP <<time period>>; Base: <<Frequency>>; Filters: <<Advanced Filers>>", strResult);
            //DictSourceDetails.Add("* Significantly differed with <#benchmark> ", "* Significantly differed with " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            //DictSourceDetails.Add("* Significantly Different from <#benchmark> ", "* Significantly Different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " is significantly higher  ");
            DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add("* Significantly different from Dollar ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add(">95%", ">" + Convert.ToString(StatTesting) + "%");
            DictSourceDetails.Add("<95%", "<" + Convert.ToString(StatTesting) + "%");
            return DictSourceDetails;
        }
        //New Source function added by bramhanath only 3,4,5 comparisions
        public Dictionary<string, string> GetSourceDetailNew(string stroption, string Benchlist1, string complist0, string complist1, string complist3, string complist5, string complist7, string shopseg1, DataSet dst, string shopseg0)
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

            if (ReportNumber == 9)
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + AppendStars(shopseg1) + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + AppendStars(shopseg1) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }
            else if (ReportNumber == 11)
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + AppendStars(shopseg1) + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }
            else
            {
                if (stroption == "Trips")
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + "; Base: All Trips; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
                else
                {
                    strResult = "Source: iSHOP - Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Channel/Retailer: " + Get_ShortNames((Convert.ToString(reportparams.ShopperSegment.Split('|')[1]))) + shopseg0 + "; Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + "; Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames);
                }
            }

            DictSourceDetails.Add(stroriginal, strResult);
            DictSourceDetails.Add("Source: iSHOP <<time period>>; Base: <<Frequency>>; Filters: <<Advanced Filers>>", strResult);
            //DictSourceDetails.Add("* Significantly differed with <#benchmark> ", "* Significantly differed with " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            //DictSourceDetails.Add("* Significantly Different from <#benchmark> ", "* Significantly Different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add("* <#benchmark> is significantly higher  ", "* " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " is significantly higher  ");
            DictSourceDetails.Add("* Significantly different from <#benchmark> ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add("* Significantly different from Dollar ", "* Significantly different from " + Get_ShortNames(Convert.ToString(reportparams.Benchmark.Split('|')[1])).Trim() + " ");
            DictSourceDetails.Add(">95%", ">" + Convert.ToString(StatTesting) + "%");
            DictSourceDetails.Add("<95%", "<" + Convert.ToString(StatTesting) + "%");
            //start - changes done by bramhanath

            DictSourceDetails.Add("Channel/Retailer: Family Dollar", "Channel/Retailer: " + shopseg1 + shopseg0);
            DictSourceDetails.Add("Comparisons: 16-18 Years & 19-24 Years", "Comparisons: " + (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " & " + (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            DictSourceDetails.Add("Channel/Retailer: Dollar Tree", "Channel/Retailer: " + shopseg1 + shopseg0);
            DictSourceDetails.Add("Channel/Retailer: Dollar Tree ", "Channel/Retailer: " + shopseg1 + shopseg0);
            DictSourceDetails.Add("Benchmark: 25-34 Years", "Benchmark: " + (complist0.Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            DictSourceDetails.Add("Comparisons: 16-18 Years ; 19-24 Years", "Comparisons: " + (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + "; " + (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")));
            DictSourceDetails.Add("Comparisons: 16-18 Years, 19-24 Years & 25-34 Years", "Comparisons: " + (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " & " + (complist0.Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")));
            DictSourceDetails.Add("Comparisons: 16-18 Years, 19-24 Years, 25-34 Years & 35-49 Years", "Comparisons: " + (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + ", " + (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + ", " + (complist0.Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " & " + (complist0.Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")));
            DictSourceDetails.Add("Time Period: 3MMT June 2014", "Time Period: " + Convert.ToString(reportparams.ShortTimePeriod));
            DictSourceDetails.Add("Filters: None", "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : reportparams.FilterShortNames));

            DictSourceDetails.Add("Base: Weekly+ Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Base: All Shoppers", "Base: " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");

            if (dst != null && dst.Tables.Count > 0)
            {
                //DictSourceDetails.Add("Male1", (complist0.Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                //DictSourceDetails.Add("Male2", (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
                //DictSourceDetails.Add("Male3", (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");

                DictSourceDetails.Add("Kroger (2705)", (complist0.Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], Benchlist1) : "0.0") + ")");
                DictSourceDetails.Add("Publix (2012)", (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ?  GetSampleSize(dst.Tables[0], complist1) : "0.0") + ")");
                DictSourceDetails.Add("Whole Foods (385)", (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist3) : "0.0") + ")");
                DictSourceDetails.Add("Walmart (2000)", (complist0.Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " (" + (dst.Tables.Count > 0 ? GetSampleSize(dst.Tables[0], complist5) : "0.0") + ")");
                DictSourceDetails.Add("Target (3000)", (complist0.Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " (" + GetSampleSize(dst.Tables[0], complist7) + ")");
            }
            //DictSourceDetails.Add("16-18 Years", (complist0.Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")));
            //DictSourceDetails.Add("25-34 Years", (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")));
            DictSourceDetails.Add("Absolute Difference with 25-34 Years: Destination Items", "Absolute Difference with " + (complist0.Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + ": Destination Items");
            DictSourceDetails.Add("Top 10 Destination Items for <#benchmark> ", "Top 10 Destination Items for " + Benchlist1 + " ");

            DictSourceDetails.Add("Trip Net", "Items Purchased on Trip (Net)");
            DictSourceDetails.Add("Absolute Difference with Family Dollar: Items Purchased", "Absolute Difference with " + Benchlist1 + ": Items Purchased");
            DictSourceDetails.Add("Absolute Difference with Family Dollar: Impulse Items", "Absolute Difference with " + Benchlist1 + ": Impulse Items");

            DictSourceDetails.Add("Demographics of <filter> Shoppers", "Demographics of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Attitudinal Segment of <filter> Shoppers", "Attitudinal Segment of " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");

            DictSourceDetails.Add("Monthly + Shoppers of Benchmark", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(Benchlist1));//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[0]["Objective"]))));
            DictSourceDetails.Add("Monthly + Shoppers of Comparison1", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist1));//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[1]["Objective"]))));
            DictSourceDetails.Add("Monthly + Shoppers of Whole Foods", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist3));
            DictSourceDetails.Add("Monthly + Shoppers of Comparison3", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist3));//Get_ShortNames(Convert.ToString(dstemp.Tables[0].Rows[1]["Objective"]))));
            DictSourceDetails.Add("Monthly + Shoppers of Comparison5", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist5));
            DictSourceDetails.Add("Monthly + Shoppers of Comparison7", (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of " + cf.cleanPPTXML(complist7));

            DictSourceDetails.Add("Cross Shopping Behavior of Weekly+ Shoppers", "Cross Retailer Shopping Behavior of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");


            DictSourceDetails.Add("<filter> Shoppers", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by top 10 retailers");
            DictSourceDetails.Add("Text1", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            DictSourceDetails.Add("Text2", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            DictSourceDetails.Add("Text3", "Out of these " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers of");
            DictSourceDetails.Add("Weekly+ Shoppers", reportparams.ShopperFrequencyShortName.ToString() + " Shoppers ");

            DictSourceDetails.Add("% of Monthly + Shoppers by Top 10 Benchmark", "% of " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers by Top 10 Benchmark");
            DictSourceDetails.Add("Top 10 Categories Purchased Among Weekly+ Shoppers", "Top 10 Categories Purchased Among " + (Convert.ToString(reportparams.ShopperFrequencyShortName).Contains("Main Store") ? "Weekly +" : Convert.ToString(reportparams.ShopperFrequencyShortName)) + " Shoppers");
            DictSourceDetails.Add("Top 10 Brands among  Weekly+ shoppers within Retailer", "Top 10 Brands among  " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " shoppers within Retailer/Channel");
            //DictSourceDetails.Add("Main Store/Favorite Store Among Weekly+ Shoppers", "Main Store/Favorite Store Among " + Convert.ToString(reportparams.ShopperFrequencyShortName) + " Shoppers");
            DictSourceDetails.Add("Benchmark1 Leads", (complist0.Equals("FactAgeGroups") ? Benchlist1.Replace("&", "&amp;") + " Age Group" : Benchlist1.Replace("&", "&amp;")) + " Leads");
            DictSourceDetails.Add("Comparision1 Leads", (complist0.Equals("FactAgeGroups") ? complist1.Replace("&", "&amp;") + " Age Group" : complist1.Replace("&", "&amp;")) + " Leads");
            DictSourceDetails.Add("Comparision2 Leads", (complist0.Equals("FactAgeGroups") ? complist3.Replace("&", "&amp;") + " Age Group" : complist3.Replace("&", "&amp;")) + " Leads");
            DictSourceDetails.Add("Comparision3 Leads", (complist0.Equals("FactAgeGroups") ? complist5.Replace("&", "&amp;") + " Age Group" : complist5.Replace("&", "&amp;")) + " Leads");
            DictSourceDetails.Add("Comparision4 Leads", (complist0.Equals("FactAgeGroups") ? complist7.Replace("&", "&amp;") + " Age Group" : complist7.Replace("&", "&amp;")) + " Leads");
            //end
            return DictSourceDetails;
        }                      
    }
}