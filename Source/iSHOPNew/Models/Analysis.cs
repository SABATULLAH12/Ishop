using iSHOPNew.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace iSHOPNew.Models
{
    public class Analysis
    {
        string leftheader = string.Empty;
        string leftbody = string.Empty;
        string rightheader = string.Empty;
        string righttbody = string.Empty;
        public Analysis()
        {
            leftheader = string.Empty;
            leftbody = string.Empty;
            rightheader = string.Empty;
            righttbody = string.Empty;
        }
        public void GetSOAPData(string TimePeriod, string ShortTimePeriod, string ShopperSegment, string ShopperSegmentShortName, string Geography, string GeographyShortName, string ShopperGroup, string ShopperFrequency, string ShoppingFrequencyShortname, string Filters, string FilterShortNames, string TimePeriod_UniqueId, string ComparisonList_UniqueIds, string Geography_UniqueId, string Group_UniqueId, string Filter_UniqueId, string ShopperFrequency_UniqueId)
        {
            ReportGeneratorParams reportparams = new ReportGeneratorParams();
            DataAccess dal = new DataAccess();
            if (ShopperFrequency_UniqueId == "")
                ShopperFrequency_UniqueId = "2";
            object[] paramvalues = null;
            reportparams.TimePeriod = TimePeriod;
            reportparams.ShortTimePeriod = ShortTimePeriod;
            reportparams.Filters = Filters;
            reportparams.ShopperSegment = ShopperSegment;
            reportparams.ShopperSegmentShortName = ShopperSegmentShortName;
            reportparams.Geography = Geography;
            reportparams.GeographyShortName = GeographyShortName;
            reportparams.ShopperFrequencyShortName = ShoppingFrequencyShortname;
            reportparams.FilterShortNames = FilterShortNames = string.IsNullOrEmpty(FilterShortNames) ? "NONE" : FilterShortNames;
            reportparams.TimePeriod_UniqueId = TimePeriod_UniqueId;
            reportparams.ComparisonList_UniqueIds = ComparisonList_UniqueIds;
            reportparams.Geography_UniqueId = Geography_UniqueId;
            reportparams.Group_UniqueId = Group_UniqueId;
            reportparams.Filter_UniqueId = Filter_UniqueId;
            reportparams.ShopperFrequency_UniqueId = ShopperFrequency_UniqueId;
            if (Convert.ToString(Geography).Equals("Census Divisions|Total"))
                Geography = "Total USA";
            
            //paramvalues = new object[] { TimePeriod, ShopperSegment, Geography, ShopperGroup, ShopperFrequency, Filters };
            paramvalues = new object[] { TimePeriod_UniqueId, ComparisonList_UniqueIds, Geography_UniqueId, Group_UniqueId, ShopperFrequency_UniqueId, Filter_UniqueId };
            //paramvalues = new object[] { TimePeriod, ShopperSegment, Geography, ShopperGroup, ShopperFrequency, Filters };
            //DataSet ds = dal.GetData(paramvalues, "USP_IshopSOAP");
            DataSet ds = dal.GetData_WithIdMapping(paramvalues, "USP_IshopSOAPReport");
            reportparams.SOAPData = ds;
            HttpContext.Current.Session["SOAPData"] = reportparams;
        }
        
    }
}