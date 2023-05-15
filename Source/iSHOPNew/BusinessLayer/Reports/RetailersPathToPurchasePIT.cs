using Aspose.Slides;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSHOPNew.Reports.BusinessLayer
{
    public class RetailersPathToPurchasePIT : BaseReport
    {
        public RetailersPathToPurchasePIT()
        { }
        public override void PrepareReport(ReportGeneratorParams param)
        {
            try
            {
                base.benchmark = param.CustomBase_ShortName;
                base.LoadSlides(HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Report_Within_Retailers_Path_To_Purchase.pptx"));

                string footerText = string.Empty;
                footerText += "Source: CCNA iSHOP Tracker- Time Period : " + param.ShortTimePeriod + " ; Base - " + param.ShopperSegment.Split('|')[1] + (param.ShopperFrequency.Equals("Total Visits", StringComparison.OrdinalIgnoreCase) || (string.IsNullOrEmpty(param.ShopperFrequency)) ? "" : "(" + param.ShopperFrequency + " )") + "; % Trips";
                footerText += "\nFilters: " + (string.IsNullOrEmpty(param.FilterShortNames) ? "None" : param.FilterShortNames);

                GetReadASTextForPIT(param, "Report_Retailer_ReadAsText.json");

                foreach (ISlide slide in slds)
                {

                    #region Footer Selection
                    IAutoShape shape = (IAutoShape)slide.Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault();
                    if (shape != null)
                        shape.TextFrame.Text = footerText;
                    #endregion

                    base.ReplaceBenchmarkAndStatText(slide, param);
                    base.ReplaceMainHeaderText(slide, "main_h", "_retailer", param.ShopperSegment.Split('|')[1]);

                    SlideNumber += 1;
                    switch (SlideNumber)
                    {
                        case 1:
                            {
                                string textBox = string.Empty;
                                textBox += string.Join(", ", param.Comparison_ShortNames) + "\n";
                                textBox += "Base - " + param.ShopperSegment.Split('|')[1] + (param.ShopperFrequency.Equals("Total Visits", StringComparison.OrdinalIgnoreCase) || (string.IsNullOrEmpty(param.ShopperFrequency)) ? "" : "(" + param.ShopperFrequency + " )");
                                textBox += " ,Filters - " + (string.IsNullOrEmpty(param.FilterShortNames) ? "None" : param.FilterShortNames) + "\n";
                                textBox += param.ShortTimePeriod;
                                var temp = (IAutoShape)slide.Shapes.Where(x => x.Name == "Filter_Timeperiod").FirstOrDefault();
                                temp.TextFrame.Text = textBox;
                                break;
                            }
                        case 4:
                            {
                                ds = param.ChartDataSet["DEMOGRAPHICS"];
                                chart_table = base.Get_Chart_Table(ds, 4, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Gender_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 4, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Age_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);

                                chart_table = base.Get_Chart_Table(ds, 4, 3);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Race-Ethnicity_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", false);

                                chart_table = base.Get_Chart_Table(ds, 4, 4);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Density_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header4", false);
                                break;


                            }
                        case 5:
                            {
                                ds = param.ChartDataSet["DEMOGRAPHICS"];
                                chart_table = base.Get_Chart_Table(ds, 5, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Socioeconomic_Level_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 5, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "HH_Income_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);

                                chart_table = base.Get_Chart_Table(ds, 5, 3);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "HH_Size_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", false);
                                break;
                            }
                        case 6:
                            {
                                ds = param.ChartDataSet["DEMOGRAPHICS"];
                                chart_table = base.Get_Chart_Table(ds, 6, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Marital_Status_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 6, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Parental_Identification_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);

                                chart_table = base.Get_Chart_Table(ds, 6, 3);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Attitudinal_Segmentation_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", false);
                                break;
                            }
                        case 8:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 8, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Daypart_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 8, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Day_of_the_Week_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 9:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 9, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Location_Prior_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 10:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 10, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Pre-Visit_Origin_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 11:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 11, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "TripMission_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 12:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 12, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "TripMission_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 13:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 13, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Reason_For_Store_Choice_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 13, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Reason_For_Store_Choice_Top2Box_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", true);
                                break;
                            }
                        case 14:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 14, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Claimed_Visit_Motivation_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 15:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 15, 1);
                                base.ReplaceRectangleText(slide, chart_table, "Another_Store_Considered");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 15, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Type_Of_Other_Stores_Considered_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", true);
                                break;
                            }
                        case 16:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 16, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Trip_Planing_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 17:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 17, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Device_Used_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 17, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Trip_Preparation_Type_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 18:
                            {
                                ds = param.ChartDataSet["PRE-SHOP"];
                                chart_table = base.Get_Chart_Table(ds, 18, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Most_Destination_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 18, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "All_Destinatiton_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", true);
                                break;
                            }
                        case 20:
                            {
                                ds = param.ChartDataSet["IN-STORE"];
                                chart_table = base.Get_Chart_Table(ds, 20, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Item_Purchased_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 21:
                            {
                                ds = param.ChartDataSet["IN-STORE"];
                                chart_table = base.Get_Chart_Table(ds, 21, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Departments_Purchased_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 22:
                            {
                                ds = param.ChartDataSet["IN-STORE"];
                                chart_table = base.Get_Chart_Table(ds, 22, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Food_Categories_Purchased_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 23:
                            {
                                ds = param.ChartDataSet["IN-STORE"];
                                chart_table = base.Get_Chart_Table(ds, 23, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Bev_Ingredient_Categories_Purchased_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 24:
                            {
                                ds = param.ChartDataSet["IN-STORE"];
                                chart_table = base.Get_Chart_Table(ds, 24, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Bev_Categories_Purchased_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 25:
                            {
                                ds = param.ChartDataSet["IN-STORE"];
                                chart_table = base.Get_Chart_Table(ds, 25, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Brand_Categories_Purchased_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 26:
                            {
                                ds = param.ChartDataSet["IN-STORE"];
                                chart_table = base.Get_Chart_Table(ds, 26, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Device_Used_InStore_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 28:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 28, 1);
                                base.ReplaceRectangleText(slide, chart_table, "Average_Time_Spent");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 28, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Time_Spent_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 29:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 29, 1);
                                base.ReplaceRectangleText(slide, chart_table, "Average_Expenditure");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 29, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Expenditure_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 30:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 30, 1);
                                base.ReplaceRectangleText(slide, chart_table, "Average_NumOf_Items");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 30, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Numbeof_Items_Purchased_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 31:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 31, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Use_Self_CheckOut_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 31, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Payment_Method_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 32:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 32, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "UseOf_Coupons_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 32, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "TypeOf_Coupon_Used_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 33:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 33, 1);
                                base.ReplaceDoughtNutChart(slide, chart_table, "Overall_Satisfaction");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 33, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Detailed_Satisfaction_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                break;
                            }
                        case 34:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 34, 1);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "Location_After_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                break;
                            }
                        case 35:
                            {
                                ds = param.ChartDataSet["POSTSHOP"];
                                chart_table = base.Get_Chart_Table(ds, 35, 1);
                                base.ReplaceRectangleText(slide, chart_table, "Immediate_Consumption");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);

                                base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");

                                chart_table = base.Get_Chart_Table(ds, 35, 2);
                                base.ReplaceClusteredColumnChart(slide, chart_table, "ImmediateConsumption_Top10_Chart");
                                base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", true);
                                break;
                            }
                    }
                }
                base.SaveFile();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
    }
}