using Aspose.Slides;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace iSHOPNew.BusinessLayer.Exports
{
    public class EstablishmentDeepDiveExport : BaseExport
    {
        public override string PrepareSlides(DataSet ds, EstablishmentDeepDiveParams param)
        {
            base.LoadSlides(HttpContext.Current.Server.MapPath(@"~\Templates\EstablishmentDeepDive.pptx"));

            foreach (ISlide slide in slds)
            {
                SlideNumber += 1;
                switch (SlideNumber)
                {
                    case 1:
                        {
                            string textBox = string.Empty;
                            textBox += string.Join(", ", param.Comparison_ShortNames);
                            textBox += ", Time period - " + param.TimePeriodShortName + "\n";
                            textBox += "Measures - " + param.SelectedMetricsNames;
                            textBox += ", Filters - " + (string.IsNullOrEmpty(param.FilterShortname) ? "None" : param.FilterShortname) + "\n";
                            var temp = (IAutoShape)slide.Shapes.Where(x => x.Name == "Filter_Timeperiod").FirstOrDefault();
                            temp.TextFrame.Text = textBox;
                            break;
                        }
                    case 2:
                        {
                            string textBox = string.Empty;
                            string chartHeader = string.Empty;
                            chartHeader += "Growth Decomposition by " + param.MetricShortName;
                            textBox += "Source:ISHOP-,Time period:" + param.TimePeriodShortName;
                            textBox += ", Base:" + string.Join(", ", param.Comparison_ShortNames);
                            textBox += ", Measures - " + param.SelectedMetricsNames + "\n";
                            textBox += "Filters - " + (string.IsNullOrEmpty(param.FilterShortname) ? "None" : param.FilterShortname) + "\n";
                            var temp = (IAutoShape)slide.Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault();
                            temp.TextFrame.Text = textBox;
                            temp = (IAutoShape)slide.Shapes.Where(x => x.Name == "ChartHeader").FirstOrDefault();
                            temp.TextFrame.Text = chartHeader;
                            chart_table = ds.Tables[0];
                            base.ReplaceWaterFallChart(slide, chart_table);
                            break;
                        }
                        
                }
            }
           return base.SaveFile();
        }
    }
}