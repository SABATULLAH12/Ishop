using Aspose.Slides;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSHOPNew.Reports.BusinessLayer
{
    public class RetailersShopperTREND : BaseReport
    {
        public RetailersShopperTREND()
        { }
        public override void PrepareReport(ReportGeneratorParams param)
        {
            try
            {
                base.LoadSlides(HttpContext.Current.Server.MapPath(@"~\Templates\Reports\Report_Trend_Retailers_Shoppers.pptx"));
                base.isTrend = true;
                base.benchmark = param.CustomBase_ShortName;
                base.shopperFrequency = param.ShopperFrequency;
                ISlide slide = null;
                string footerText = string.Empty;
                footerText += "Source: CCNA iSHOP Tracker- Time Period : " + param.ShortTimePeriod + " ; Retailer: " + param.ShopperSegment.Split('|')[1] + (param.ShopperFrequency.Equals("Total Visits", StringComparison.OrdinalIgnoreCase) || (string.IsNullOrEmpty(param.ShopperFrequency)) ? " - Total Visits" : " - " + param.ShopperFrequency) + "; % Shoppers";
                footerText += "\nFilters: " + (string.IsNullOrEmpty(param.FilterShortNames) ? "None" : param.FilterShortNames);

                for (int i = 0; i < slds.Count; i++)
                {
                    slide = slds[i];
                    SlideNumber += 1;
                    base.ReplaceBenchmarkAndStatText(slide, param);
                    base.ReplaceMainHeaderText(slide, "main_h", "_frequency", base.ToPascalCase(shopperFrequency));
                    base.ReplaceMainHeaderText(slide, "main_h", "_retailer", param.ShopperSegment.Split('|')[1]);
                    base.ReplaceGroupHeaderText(slide, "Header1", "Header", "_frequency", base.ToPascalCase(shopperFrequency));
                    switch (SlideNumber)
                    {
                        case 1:
                            {
                                string textBox = string.Empty;
                                textBox = param.ShortTimePeriod + "\n";
                                textBox +=   "Base - " + param.ShopperSegment.Split('|')[1]+"(" + (param.ShopperFrequency.Equals("Total Visits", StringComparison.OrdinalIgnoreCase) || (string.IsNullOrEmpty(param.ShopperFrequency)) ? "Total Trips" : param.ShopperFrequency) + " )";
                                textBox += " ,Filters - " + (string.IsNullOrEmpty(param.FilterShortNames) ? "None" : param.FilterShortNames) + "\n";

                                var temp = (IAutoShape)slide.Shapes.Where(x => x.Name == "Filter_Timeperiod").FirstOrDefault();
                                temp.TextFrame.Text = textBox;
                                break;
                            }
                        case 4:
                            {
                                if (param.ChartDataSet.ContainsKey("FREQUENCYPROFILE"))
                                {
                                    ds = param.ChartDataSet["FREQUENCYPROFILE"];
                                    chart_table = base.Get_Chart_Table(ds, 4, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "TotalGB_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 5:
                            {
                                if (param.ChartDataSet.ContainsKey("FREQUENCYPROFILE"))
                                {
                                    ds = param.ChartDataSet["FREQUENCYPROFILE"];
                                    chart_table = base.Get_Chart_Table(ds, 5, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "MGB_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 7:
                            {
                                if (param.ChartDataSet.ContainsKey("DEMOGRAPHICS"))
                                {
                                    ds = param.ChartDataSet["DEMOGRAPHICS"];
                                    //Gender
                                    chart_table = base.Get_Chart_Table(ds, 7, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Gender_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    //Age
                                    chart_table = base.Get_Chart_Table(ds, 7, 2);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Age_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                    //Ethinity
                                    chart_table = base.Get_Chart_Table(ds, 7, 3);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Race-Ethnicity_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", false);
                                    //Density
                                    chart_table = base.Get_Chart_Table(ds, 7, 4);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Occupation_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header4", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 8:
                            {
                                if (param.ChartDataSet.ContainsKey("DEMOGRAPHICS"))
                                {
                                    ds = param.ChartDataSet["DEMOGRAPHICS"];
                                    //Socioeconomic 
                                    chart_table = base.Get_Chart_Table(ds, 8, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Socioeconomic_Level_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    //HH Income
                                    chart_table = base.Get_Chart_Table(ds, 8, 2);
                                    base.ReplaceTrendLineChart(slide, chart_table, "HH_Income_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                    //HH Size
                                    chart_table = base.Get_Chart_Table(ds, 8, 3);
                                    base.ReplaceTrendLineChart(slide, chart_table, "HH_Size_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 9:
                            {
                                if (param.ChartDataSet.ContainsKey("DEMOGRAPHICS"))
                                {
                                    ds = param.ChartDataSet["DEMOGRAPHICS"];
                                    //Marital Status
                                    chart_table = base.Get_Chart_Table(ds, 9, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Marital_Status_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    //Parental Identification
                                    chart_table = base.Get_Chart_Table(ds, 9, 2);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Parental_Identification_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                    //Attitudinal Segmentation
                                    chart_table = base.Get_Chart_Table(ds, 9, 3);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Attitudinal_Segmentation_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 10:
                            {
                                if (param.ChartDataSet.ContainsKey("DEMOGRAPHICS"))
                                {
                                    ds = param.ChartDataSet["DEMOGRAPHICS"];
                                    //OnlineShoppe
                                    chart_table = base.Get_Chart_Table(ds, 10, 1);
                                    base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");
                                    base.ReplaceTrendLineChart(slide, chart_table, "OnlineShopper_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    //Delivery
                                    chart_table = base.Get_Chart_Table(ds, 10, 2);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Delivery_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", false);
                                    //TechnologyUser
                                    chart_table = base.Get_Chart_Table(ds, 10, 3);
                                    base.ReplaceTrendLineChart(slide, chart_table, "TechnologyUser_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", false);
                                    //OnlineSpend
                                    chart_table = base.Get_Chart_Table(ds, 10, 4);
                                    base.ReplaceTrendLineChart(slide, chart_table, "OnlineSpend_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header4", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 12:
                            {
                                if (param.ChartDataSet.ContainsKey("CROSSRETAILERSHOPPINGBEHAVIOR-CHANNELS")
                                    && param.ChartDataSet.ContainsKey("CROSSRETAILERSHOPPINGBEHAVIOR-RETAILERS"))
                                {
                                    //Channels_Chart
                                    ds = param.ChartDataSet["CROSSRETAILERSHOPPINGBEHAVIOR-CHANNELS"];
                                    chart_table = base.Get_Chart_Table(ds, 12, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Channels_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    //Retailers_Chart
                                    ds = param.ChartDataSet["CROSSRETAILERSHOPPINGBEHAVIOR-RETAILERS"];
                                    chart_table = base.Get_Chart_Table(ds, 12, 2);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Retailers_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", true);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 13:
                            {
                                if (param.ChartDataSet.ContainsKey("CROSSRETAILERSHOPPINGBEHAVIOR-ONLINERETAILERS"))
                                {
                                    //OnlineRetailers
                                    ds = param.ChartDataSet["CROSSRETAILERSHOPPINGBEHAVIOR-ONLINERETAILERS"];
                                    chart_table = base.Get_Chart_Table(ds, 13, 1);
                                    base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends");
                                    base.ReplaceTrendLineChart(slide, chart_table, "OnlineRetailers_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 15:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 15, 1);
                                    
                                    base.UpdatePyramidSeriesData(slide, chart_table, "Retailer_Loyalty_Pyramid_Chart");
                                    base.SampleSizeFooterTableUpdate(slide, chart_table, "TableLegends",true);
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                }
                                break;
                            }
                        case 16:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 16, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Store_Associations_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 17:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 17, 1);
                                    
                                    base.Update_Table(slide, chart_table, "Store_Imagery_Table", i);
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 18:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 17, 1);

                                    System.Data.DataTable tbl = new System.Data.DataTable();
                                    tbl = chart_table.Clone();

                                    foreach (var row in chart_table.Select("Metric='Quality'"))
                                    {
                                        tbl.Rows.Add(row.ItemArray);
                                    }

                                    base.SampleSizeFooterTableUpdate(slide, tbl, "TableLegends");
                                    base.ReplaceHeaderAndDescriptionText(slide, tbl, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 19:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 19, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Good_Place_to_Shop_for_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 20:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 20, 1);
                                    
                                    base.Update_Table(slide, chart_table, "Good_Place_To_Shop_Table", i);
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 21:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 20, 1);

                                    System.Data.DataTable tbl = new System.Data.DataTable();
                                    tbl = chart_table.Clone();

                                    foreach (var row in chart_table.Select("Metric='NON FOOD ITEMS'"))
                                    {
                                        tbl.Rows.Add(row.ItemArray);
                                    }

                                    base.SampleSizeFooterTableUpdate(slide, tbl, "TableLegends");
                                    base.ReplaceHeaderAndDescriptionText(slide, tbl, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 22:
                            {
                                if (param.ChartDataSet.ContainsKey("BRANDHEALTH"))
                                {
                                    ds = param.ChartDataSet["BRANDHEALTH"];
                                    chart_table = base.Get_Chart_Table(ds, 22, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Main_Store_Favorite_Store_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", false);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 24:
                            {
                                if (param.ChartDataSet.ContainsKey("SHOPPERBEVERAGEPURCHASE"))
                                {
                                    //Beverage_Categories_Summary_Chart
                                    ds = param.ChartDataSet["SHOPPERBEVERAGEPURCHASE"];
                                    chart_table = base.Get_Chart_Table(ds, 24, 1);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Beverage_Categories_Summary_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);
                                    //OnlineNARTD_Chart
                                    chart_table = base.Get_Chart_Table(ds, 24, 2);
                                    base.ReplaceTrendLineChart(slide, chart_table, "OnlineNARTD_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", true);
                                    //Beverage_Categories_Chart
                                    chart_table = base.Get_Chart_Table(ds, 24, 3);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Beverage_Categories_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", true);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 25:
                            {
                                if (param.ChartDataSet.ContainsKey("SHOPPERBEVERAGEPURCHASE"))
                                {
                                    //Manufacturer_Beverage_Summary_Chart 
                                    ds = param.ChartDataSet["SHOPPERBEVERAGEPURCHASE"];
                                    chart_table = base.Get_Chart_Table(ds, 25, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Manufacturer_Beverage_Summary_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                        case 26:
                            {
                                if (param.ChartDataSet.ContainsKey("SHOPPERBEVERAGEPURCHASE"))
                                {
                                    //Total_SSD_Chart 
                                    ds = param.ChartDataSet["SHOPPERBEVERAGEPURCHASE"];
                                    chart_table = base.Get_Chart_Table(ds, 26, 1);
                                    
                                    base.ReplaceTrendLineChart(slide, chart_table, "Total_SSD_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header1", true);
                                    //Race-Ethnicity_Chart                               
                                    chart_table = base.Get_Chart_Table(ds, 26, 2);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Race-Ethnicity_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header2", true);
                                    //Occupation_Chart                                
                                    chart_table = base.Get_Chart_Table(ds, 26, 3);
                                    base.ReplaceTrendLineChart(slide, chart_table, "Occupation_Chart");
                                    base.ReplaceHeaderAndDescriptionText(slide, chart_table, "Header3", true);
                                    base.SampleSizeFooterTextForTrend(slide, chart_table, "TrendSampleSize");
                                }
                                break;
                            }
                    }
                    #region Footer Selection
                    IAutoShape shape = (IAutoShape)slide.Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault();
                    if (shape != null)
                        shape.TextFrame.Text = footerText;
                    #endregion
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