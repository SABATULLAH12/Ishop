using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace iSHOPNew.Models
{
    public static class ExtensionMethods
    {       
        //Check null values
        public static string ToMyString(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                return str;
            else
                return string.Empty;
        }
        //Formate Date Time
        public static String FormateDateTime(this String datetime)
        {
            if (datetime == null)
                return datetime;
            if (datetime.Length == 1)
                return "0" + datetime;
            else
                return datetime;
        }
        //Formate Sample Size
        public static String FormateSampleSizeNumber(this String SampleSize)
        {
            string samplesize = SampleSize;
            if (string.IsNullOrEmpty(SampleSize))
                samplesize = "0";
            else if (SampleSize != "0")
                samplesize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(SampleSize)));
            return samplesize;
        }     
        public static ProfilerChartParams Clone(this ProfilerChartParams profilerparams)
        {
            ProfilerChartParams pparams = new ProfilerChartParams();

            pparams.ViewType = profilerparams.ViewType;
            pparams.selectedMetrics = profilerparams.selectedMetrics;
            pparams.BCFullNames = profilerparams.BCFullNames;
            pparams.ChartDataSet = profilerparams.ChartDataSet;
            pparams.SingleSelection = profilerparams.SingleSelection;
            pparams.ChartType = profilerparams.ChartType;
            pparams.Comparison_DBNames = profilerparams.Comparison_DBNames;
            pparams.Comparison_ShortNames = profilerparams.Comparison_ShortNames;
            pparams.Benchmark = profilerparams.Benchmark;
            pparams.BenchmarkShortName = profilerparams.BenchmarkShortName;
            pparams.Comparisonlist = profilerparams.Comparisonlist;
            pparams.ComparisonShortNames = profilerparams.ComparisonShortNames;
            pparams.TimePeriod = profilerparams.TimePeriod;
            pparams.ShortTimePeriod = profilerparams.ShortTimePeriod;
            pparams.ShopperSegment = profilerparams.ShopperSegment;
            pparams.FrequencyTitle = profilerparams.FrequencyTitle;
            pparams.ShopperFrequency = profilerparams.ShopperFrequency;
            pparams.ShopperFrequencyShortName = profilerparams.ShopperFrequencyShortName;
            pparams.Metric = profilerparams.Metric;
            pparams.MetricShortName = profilerparams.MetricShortName;
            pparams.ActiveTab = profilerparams.ActiveTab;
            pparams.FilterShortNames = profilerparams.FilterShortNames;
            pparams.Filters = profilerparams.Filters;
            pparams.ModuleBlock = profilerparams.ModuleBlock;
            pparams.SelectedMetrics = profilerparams.SelectedMetrics;
            pparams.Top_10 = profilerparams.Top_10;
            pparams.SelectedMetricsIds = profilerparams.SelectedMetricsIds;
            pparams.View = profilerparams.View;
            pparams.ChartHeight = profilerparams.ChartHeight;
            pparams.ChartWidth = profilerparams.ChartWidth;
            pparams.ChartXValues = profilerparams.ChartXValues;

            pparams.SelectedStatTest = profilerparams.SelectedStatTest;

            pparams.IsSelectionChange = profilerparams.IsSelectionChange;
            pparams.StatTest = profilerparams.StatTest;
            pparams.StatPositive = profilerparams.StatPositive;
            pparams.StatNegative = profilerparams.StatNegative;
            pparams.StatTesting = profilerparams.StatTesting;

            pparams.CustomBase_DBName = profilerparams.CustomBase_DBName;
            pparams.CustomBase_ShortName = profilerparams.CustomBase_ShortName;
            pparams.CustomBase_UniqueId = profilerparams.CustomBase_UniqueId;

            pparams.Comparison_UniqueIds = profilerparams.Comparison_UniqueIds;
            pparams.TimePeriod_UniqueId = profilerparams.TimePeriod_UniqueId;
            pparams.TimePeriodFrom_UniqueId = profilerparams.TimePeriodFrom_UniqueId;
            pparams.TimePeriodTo_UniqueId = profilerparams.TimePeriodTo_UniqueId;
            pparams.ShopperSegment_UniqueId = profilerparams.ShopperSegment_UniqueId;
            pparams.ShopperFrequency_UniqueId = profilerparams.ShopperFrequency_UniqueId;
            pparams.Sigtype_UniqueId = profilerparams.Sigtype_UniqueId;
            pparams.Filter_UniqueId = profilerparams.Filter_UniqueId;
            pparams.Beverage_UniqueId = profilerparams.Beverage_UniqueId;

            pparams.Metric_UniqueId = profilerparams.Metric_UniqueId;
            pparams.Filtertypeid_UniqueId = profilerparams.Filtertypeid_UniqueId;
            pparams.TabName = profilerparams.TabName;
            pparams.TabIndexId = profilerparams.TabIndexId;
            pparams.Tab_Id_mapping = profilerparams.Tab_Id_mapping;
            pparams.ChartXValues_UniqueId = profilerparams.ChartXValues_UniqueId;
            pparams.beverageSelectionType_UniqueId = profilerparams.beverageSelectionType_UniqueId;
            pparams.TrendType = profilerparams.TrendType;
            pparams.ComparisonNames = profilerparams.ComparisonNames;
            pparams.ViewType = profilerparams.ViewType;
            return pparams;
        }
    }
}