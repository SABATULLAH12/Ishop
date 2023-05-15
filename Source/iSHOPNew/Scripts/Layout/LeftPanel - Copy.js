/// <reference path="Common.js" />
/// <reference path="RightPanelFilter.js" />
/// <reference path="../jquery.nicescroll.js" />
/// <reference path="Layout.js" />
//written by Nagaraju D for left panel filters
//Date: 04-01-2017
var viewFilters = [
{ Name: 'hdn-tbl-compareretailers', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-tbl-retailerdeepdive', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-chart-compareretailers', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'retailer-measure', Name: 'Retailer Measure', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.MeasureType' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-chart-retailerdeepdive', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'retailer-measure', Name: 'Retailer Measure', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.MeasureType' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }, { Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-tbl-comparebeverages', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'channel-content', Name: 'Beverage Where Purchased', onclick: 'SelectChannel(this);', PopupName: 'rgt-cntrl-chnl' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }] },

{ Name: 'hdn-tbl-beveragedeepdive', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'groupDivId', Name: 'Beverage Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'channel-content', Name: 'Beverage Where Purchased', onclick: 'SelectChannel(this);', PopupName: 'rgt-cntrl-chnl' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }] },

{ Name: 'hdn-chart-comparebeverages', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'retailer-measure', Name: 'Beverage Measure', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.MeasureType' }, { Id: 'channel-content', Name: 'Beverage Where Purchased', onclick: 'SelectChannel(this);', PopupName: 'rgt-cntrl-chnl' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }] },

{ Name: 'hdn-chart-beveragedeepdive', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'retailer-measure', Name: 'Beverage Measure', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.MeasureType' }, { Id: 'channel-content', Name: 'Beverage Where Purchased', onclick: 'SelectChannel(this);', PopupName: 'rgt-cntrl-chnl' }, { Id: 'groupDivId', Name: 'Beverage Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }] },

{ Name: 'hdn-report-compareretailersshoppers', Filters: [{ Id: 'RetailerDivId', Name: 'Priority Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'left-panel-frequency', Name: 'Reports Retailer Frequency-Cross-Retailer Shopper (Shoppers)', onclick: 'SelectFrequency(this);', PopupName: '.FilterPopup.Left-Frequency' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-report-retailersshopperdeepdive', Filters: [{ Id: 'RetailerDivId', Name: 'Priority Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Reports Retailer Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'left-panel-frequency', Name: 'Reports Retailer Frequency-Cross-Retailer Shopper (Shoppers)', onclick: 'SelectFrequency(this);', PopupName: '.FilterPopup.Left-Frequency' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-report-compareretailerspathtopurchase', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'AdvFilterDivId', Name: 'Reports PathToPurchase Filters', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-report-retailerspathtopurchasedeepdive', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'AdvFilterDivId', Name: 'Reports PathToPurchase Filters', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-report-comparebeveragesmonthlypluspurchasers', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-report-beveragemonthlypluspurchasersdeepdive', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'groupDivId', Name: 'Reports Beverage Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-report-comparebeveragespurchasedetails', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'beverage-where-purchased', Name: 'Beverage Where Purchased', onclick: 'SelectChannel(this);', PopupName: '.FilterPopup.Left-Channel-Visited' }, { Id: 'AdvFilterDivId', Name: 'Reports PathToPurchase Filters', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-report-beveragespurchasedetailsdeepdive', Filters: [{ Id: 'BevDivId', Name: 'Beverages', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'groupDivId', Name: 'Beverage Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'beverage-where-purchased', Name: 'Beverage Where Purchased', onclick: 'SelectChannel(this);', PopupName: '.FilterPopup.Left-Channel-Visited' }, { Id: 'AdvFilterDivId', Name: 'Reports PathToPurchase Filters', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-crossretailer-totalrespondentstripsreport', Filters: [{ Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'total-measure-trip', Name: 'Total Measure', onclick: 'SelectTotalMeasure(this);', PopupName: '.FilterPopup.TotalMeasure' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'left-panel-frequency', Name: 'Correspondance Retailer Frequency', onclick: 'SelectFrequency(this);', PopupName: '.FilterPopup.Left-Frequency' }] },

{ Name: 'hdn-analysis-acrossshopper', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'BevDivId', Name: 'BGM Beverag And NonBeverage Items', onclick: 'SelectBevComparison(this);', PopupName: '.FilterPopup.Beverages' }, { Id: 'left-panel-frequency', Name: 'BGM Frequency', onclick: 'SelectFrequency(this);', PopupName: '.FilterPopup.Left-Frequency' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-tbl-retailerdeepdive', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-analysis-acrosstrips', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Reports Beverage Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'left-panel-frequency', Name: 'Reports Retailer Frequency', onclick: 'SelectFrequency(this);', PopupName: '.FilterPopup.Left-Frequency' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-analysis-withinshopper', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'CorrespondenceMeasureDivId', Name: 'CorrespondenceMeasure', onclick: 'SelectAdvanceAnalyticsTrips(this);', PopupName: '.FilterPopup.Advanced-Analytics-Select-Variable' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }] },

{ Name: 'hdn-analysis-withintrips', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'CorrespondenceMeasureDivId', Name: 'CorrespondenceMeasure', onclick: 'SelectAdvanceAnalyticsTrips(this);', PopupName: '.FilterPopup.Advanced-Analytics-Select-Variable' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'VisistsFilterDivId', Name: 'Visits', onclick: 'SelectAdvfilters(this);', PopupName: 'rgt-cntrl-SubFilter-Conatianer' }] },

{ Name: 'hdn-analysis-crossretailerfrequencies', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-analysis-crossretailerimageries', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'left-panel-frequency', Name: 'Correspondance Retailer Frequency', onclick: 'SelectFrequency(this);', PopupName: '.FilterPopup.Left-Frequency' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

{ Name: 'hdn-dashboard-pathtopurchase', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'custombase-groupDivId', Name: 'Shopper Groups', onclick: 'SelecCustomBaseGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'Custombase-RetailerDivId', Name: 'Retailers', onclick: 'SelectPathToPurchaseCustomBase(this);', PopupName: 'Custombase-Retailers' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-dashboard-demographic', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'groupDivId', Name: 'Shopper Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'custombase-groupDivId', Name: 'Shopper Groups', onclick: 'SelecCustomBaseGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'Custombase-RetailerDivId', Name: 'Retailers', onclick: 'SelectPathToPurchaseCustomBase(this);', PopupName: 'Custombase-Retailers' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

{ Name: 'hdn-e-commerce-tbl-comparesites', Filters: [{ Id: 'e-com-sites', Name: 'E-Com Sites', onclick: 'SelectSite(this);', PopupName: '.FilterPopup.SiteFilters' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters', searchParams: 'DemographicFilters|Search-AdvancedFilters|AdvancedFilter-Search-Content' }, { Id: 'VisistsFilterDivId', Name: 'E-Com Visits', onclick: 'SelectAdvfilters(this);', PopupName: '.rgt-cntrl-SubFilter-Conatianer' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }] },

{ Name: 'hdn-e-commerce-tbl-sitedeepdive', Filters: [{ Id: 'e-com-sites', Name: 'E-Com Sites', onclick: 'SelectSite(this);', PopupName: '.FilterPopup.SiteFilters' }, { Id: 'groupDivId', Name: 'E-Com Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters', searchParams: 'DemographicFilters|Search-AdvancedFilters|AdvancedFilter-Search-Content' }, { Id: 'VisistsFilterDivId', Name: 'E-Com Visits', onclick: 'SelectAdvfilters(this);', PopupName: '.rgt-cntrl-SubFilter-Conatianer' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }] },

{ Name: 'hdn-e-commerce-chart-comparesites', Filters: [{ Id: 'e-com-sites', Name: 'E-Com Sites', onclick: 'SelectSite(this);', PopupName: '.FilterPopup.SiteFilters' }, { Id: 'retailer-measure', Name: 'E-Com Measure', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.MeasureType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters', searchParams: 'DemographicFilters|Search-AdvancedFilters|AdvancedFilter-Search-Content' }, { Id: 'VisistsFilterDivId', Name: 'E-Com Visits', onclick: 'SelectAdvfilters(this);', PopupName: '.rgt-cntrl-SubFilter-Conatianer' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }] },

{ Name: 'hdn-e-commerce-chart-sitedeepdive', Filters: [{ Id: 'e-com-sites', Name: 'E-Com Sites', onclick: 'SelectSite(this);', PopupName: '.FilterPopup.SiteFilters' }, { Id: 'groupDivId', Name: 'E-Com Groups', onclick: 'SelecGroupMetricName(this);', PopupName: '.FilterPopup.GroupType' }, { Id: 'retailer-measure', Name: 'E-Com Measure', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.MeasureType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters', searchParams: 'DemographicFilters|Search-AdvancedFilters|AdvancedFilter-Search-Content' }, { Id: 'VisistsFilterDivId', Name: 'E-Com Visits', onclick: 'SelectAdvfilters(this);', PopupName: '.rgt-cntrl-SubFilter-Conatianer' }, { Id: 'beverage-frequency', Name: 'Beverage Frequency', onclick: 'SelectBeverageSelectionType(this);', PopupName: 'rgt-cntrl-Selection beverageItems' }] },

{ Name: 'hdn-analysis-establishmentdeepdive', Filters: [{ Id: 'RetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Retailers' }, { Id: 'retailer-measure', Name: 'Retailer Measure', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.MeasureType' }, { Id: 'AdvFilterDivId', Name: 'Demographic', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }, { Id: 'CompetitorFrequency-RetailerDivId', Name: 'Retailers', onclick: 'SelectCompetitor(this);', PopupName: 'CompetitorFrequency-Retailers' }] },

//{ Name: 'hdn-crossretailer-sarreport', Filters: [{ Id: 'SarRetailerDivId', Name: 'Retailers', onclick: 'SelectComparison(this);', PopupName: '.FilterPopup.Sar-Retailers' }, { Id: 'SarCompetitorDivId', Name: 'Competitors', onclick: 'SelecMeasureMetricName(this);', PopupName: '.FilterPopup.Sar-Competitors' }, { Id: 'SarFrequencyDivId', Name: 'SarFrequency', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.Sar-Frequency' }, { Id: 'AdvFilterDivId', Name: 'Reports PathToPurchase Filters', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },
{ Name: 'hdn-crossretailer-sarreport', Filters: [{ Id: 'SarRetailerDivId', Name: 'Retailers', onclick: 'SelectSarRetailer(this);', PopupName: '.FilterPopup.Sar-Retailers' }, { Id: 'SarCompetitorDivId', Name: 'Competitors', onclick: 'SelectSarCompetitor(this);', PopupName: '.FilterPopup.Sar-Competitors' }, { Id: 'SarFrequencyDivId', Name: 'SarFrequency', onclick: 'SelectSarFrequency(this);', PopupName: '.FilterPopup.Sar-Frequency' }, { Id: 'AdvFilterDivId', Name: 'Reports PathToPurchase Filters', onclick: 'SelectDemographic(this);', PopupName: '.FilterPopup.AdvancedFilters' }] },

];
function prepareViewFilters() {  
    currentviewfilters = getCurrentViewFilters();
    if (currentviewfilters != false && currentviewfilters != undefined && currentviewfilters != null) {        
        for (var j = 0; j < currentviewfilters.Filters.length; j++) {
            if (currentviewfilters.Filters[j].Id == "VisistsFilterDivId" || currentviewfilters.Filters[j].Id == "channel-content")
                addRightPanelfilter(currentviewfilters.Filters[j].Id, getFilter(currentviewfilters.Filters[j].Name), currentviewfilters.Filters[j].onclick, currentviewfilters.Filters[j].PopupName);
            else
                addfilter(currentviewfilters.Filters[j].Id, getFilter(currentviewfilters.Filters[j].Name), currentviewfilters.Filters[j].onclick);
        }
    }
    $("#visit_frequency_containerId ul li, #frequency_containerId_trips ul li, #frequency_containerId_trips ul li").attr("tabtype", "trips");
    $("#shopper_frequency_containerId ul li").attr("tabtype", "shopper");  
}
function addRightPanelfilter(filterid, filter, onclick_event_name, popupName) {
    if (filter != undefined && filter != null) {
        //add levels
        var start_level = 0;
        if (onclick_event_name.toLowerCase() == "selectadvfilters(this);")
            start_level = 1;

        $("#" + filterid).html("");
        for (var level = start_level; level < filter[0].Levels.length; level++) {
            if (filter[0].Levels[level].LevelItems.length > 0)
                $("#" + filterid).append("<div level-id=\"" + filter[0].Levels[level].Id + "\" level=\"level" + filter[0].Levels[level].Id + "\" class=\"Lavel level" + filter[0].Levels[level].Id + "\" style=\"display:" + (level == 0 ? "inline-block" : "none") + ";\"><ul></ul></div>");
        }
        //add filter items
        for (var level = start_level; level < filter[0].Levels.length; level++) {
            for (var i = 0; i < filter[0].Levels[level].LevelItems.length; i++) {
                var obj = filter[0].Levels[level].LevelItems[i];
                var imghtml = getFilterImage(filterid, obj);
                if (obj.DispFilterType == "Visits")
                    obj.DispFilterType = "trips";
                if (obj.HasSubLevel && obj.IsSelectable)
                    html = "<li is-geography=\"" + obj.IsGeography + "\"  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype\"><div class=\"FilterStringContainerdiv\" style=\"\">" + imghtml + "<span onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span><div event-name=\"" + onclick_event_name + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" filter-id=\"" + filterid + "\" popup-name=\"" + popupName + "\"  class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                else if (obj.HasSubLevel)
                    html = "<li is-geography=\"" + obj.IsGeography + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\"SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype show-level\"><div class=\"FilterStringContainerdiv\" style=\"\">" + imghtml + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span><div event-name=\"" + onclick_event_name + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" filter-id=\"" + filterid + "\" popup-name=\"" + popupName + "\" class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                else {
                    if (onclick_event_name == "SelectFrequency(this);" && obj.Name.toLowerCase() == "total visits") {
                        html = "<li is-geography=\"" + obj.IsGeography + "\"  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:none;\" onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"false\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                    }
                    else
                        html = "<li is-geography=\"" + obj.IsGeography + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" class=\"lft-popup-ele\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"false\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                }
                $("#" + filterid + " .level" + filter[0].Levels[level].Id + " ul").append(html);
            }
        }
    }
}
function addfilter(filterid, filter, onclick_event_name)
{

    if (filter != undefined && filter != null) {
        //add levels
        $("#" + filterid).html("");
        if (currentpage == "hdn-crossretailer-sarreport")
        {
            for (var level = 0; level < filter[0].Levels.length; level++) {
                if (filter[0].Levels[level].LevelItems.length > 0)
                    $("#" + filterid).append("<div level-id=\"" + filter[0].Levels[level].Id + "\" level=\"level" + filter[0].Levels[level].Id + "\" class=\"Lavel level" + filter[0].Levels[level].Id + "\" style=\"display:" + (level == 0 ? "inline-block" : "none") + ";\"><ul></ul></div>");
            }
            //add filter items
            for (var level = 0; level < filter[0].Levels.length; level++) {
                for (var i = 0; i < filter[0].Levels[level].LevelItems.length; i++) {
                    var obj = filter[0].Levels[level].LevelItems[i];
                    var imghtml = getFilterImage(filterid, obj);
                    var ToolTip = "";
                    if (obj.DispFilterType == "Visits")
                        obj.DispFilterType = "trips";
                    if (obj.Name.trim().toLowerCase() == "visitguesttoggle") {
                        html = "<li donothover='true' is-geography=\"" + obj.IsGeography + "\" parent-of-parent=\"" + obj.ParentOfParent + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\"priorityid=\"" + obj.IsPriority + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\"title=\"" + ToolTip + "\" class=\"lft-popup-ele GeotooltipI" + (obj.IsHeader ? " priorityclass" : "") + "\" style=\"cursor:default !important;\"><div class=\"sar-adv-fltr-toggle\"><div tabtype=\"visit\" class=\"sar-adv-fltr-visit\" style=\"color: rgb(255, 0, 0);\">VISITS</div> <label class=\"sar-switch\"> <input type=\"checkbox\" id=\"sar-guest-visit-toggle\" class=\"\"><div class=\"sar-toggle-slider round\"></div> </label><div tabtype=\"guest\" class=\"sar-adv-fltr-guest\" style=\"color: rgb(255, 255, 255)\">GUESTS</div></div></li>";
                    }
                    else if (obj.HasSubLevel && obj.IsSelectable)
                        html = "<li is-geography=\"" + obj.IsGeography + "\" parent-of-parent=\"" + obj.ParentOfParent + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\"title=\"" + ToolTip + "\" class=\"gouptype GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + imghtml + "<span onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" priorityid=\"" + obj.IsPriority + "\" style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span><div event-name=\"" + onclick_event_name + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" filter-id=\"" + filterid + "\"  class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                    else if (obj.HasSubLevel)
                        html = "<li is-geography=\"" + obj.IsGeography + "\" parent-of-parent=\"" + obj.ParentOfParent + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\"SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\"title=\"" + ToolTip + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + imghtml + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span><div event-name=\"" + onclick_event_name + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" filter-id=\"" + filterid + "\"  class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                    else {
                        if (onclick_event_name == "SelectFrequency(this);" && obj.Name.toLowerCase() == "total visits") {
                            html = "<li is-geography=\"" + obj.IsGeography + "\" parent-of-parent=\"" + obj.ParentOfParent + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:none;\" onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" priorityid=\"" + obj.IsPriority + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\"title=\"" + ToolTip + "\" class=\"lft-popup-ele GeotooltipI\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"false\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                        }
                            //else if (obj.Name.toLowerCase() == "total" && obj.id == "4000") {
                            //    html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" onclick=\"" + (obj.IsHeader ? "" : (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name)) + "\" type=\"" + (obj.FilterType == "Visits" ? "trips" : "shopper") + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\"title=\"" + ToolTip + "\" class=\"lft-popup-ele GeotooltipI" + (obj.IsHeader ? " priorityclass" : "") + "\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"true\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                            //}
                        else
                            html = "<li is-geography=\"" + obj.IsGeography + "\" parent-of-parent=\"" + obj.ParentOfParent + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" onclick=\"" + (obj.IsHeader ? "" : (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name)) + "\" priorityid=\"" + obj.IsPriority + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\"title=\"" + ToolTip + "\" class=\"lft-popup-ele GeotooltipI" + (obj.IsHeader ? " priorityclass" : "") + "\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"false\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                    }
                    $("#" + filterid + " .level" + filter[0].Levels[level].Id + " ul").append(html);
                }
            }
        }
        else {
            for (var level = 0; level < filter[0].Levels.length; level++) {
                if (filter[0].Levels[level].LevelItems.length > 0)
                    $("#" + filterid).append("<div level-id=\"" + filter[0].Levels[level].Id + "\" level=\"level" + filter[0].Levels[level].Id + "\" class=\"Lavel level" + filter[0].Levels[level].Id + "\" style=\"display:" + (level == 0 ? "inline-block" : "none") + ";\"><ul></ul></div>");
            }
            //add filter items
            for (var level = 0; level < filter[0].Levels.length; level++) {
                for (var i = 0; i < filter[0].Levels[level].LevelItems.length; i++) {
                    var obj = filter[0].Levels[level].LevelItems[i];
                    var imghtml = getFilterImage(filterid, obj);
                    var ToolTip = "";
                    if (obj.DispFilterType == "Visits")
                        obj.DispFilterType = "trips";
                    if (getEcomchannel(obj))
                        html = "<li is-geography=\"" + obj.IsGeography + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" type=\"" + obj.DispFilterType + "\" priorityid=\"" + obj.IsPriority + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\"title=\"" + ToolTip + "\" class=\"gouptype GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + imghtml + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></div></li>";
                    else if (obj.HasSubLevel && obj.IsSelectable)
                        html = "<li is-geography=\"" + obj.IsGeography + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\"title=\"" + ToolTip + "\" class=\"gouptype GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + imghtml + "<span onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" priorityid=\"" + obj.IsPriority + "\" style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span><div event-name=\"" + onclick_event_name + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" filter-id=\"" + filterid + "\"  class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                    else if (obj.HasSubLevel)
                        html = "<li is-geography=\"" + obj.IsGeography + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\"SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\"title=\"" + ToolTip + "\" class=\"gouptype show-level GeotooltipI\"><div class=\"FilterStringContainerdiv\" style=\"\">" + imghtml + "<span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\" uniqueid=\"" + obj.UniqueId + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span><div event-name=\"" + onclick_event_name + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" filter-id=\"" + filterid + "\"  class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw\"></span></div></div></li>";
                    else {
                        if (onclick_event_name == "SelectFrequency(this);" && obj.Name.toLowerCase() == "total visits") {
                            html = "<li is-geography=\"" + obj.IsGeography + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:none;\" onclick=\"" + (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name) + "\" priorityid=\"" + obj.IsPriority + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\"title=\"" + ToolTip + "\" class=\"lft-popup-ele GeotooltipI\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"false\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                        }
                            //else if (obj.Name.toLowerCase() == "total" && obj.id == "4000") {
                            //    html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" onclick=\"" + (obj.IsHeader ? "" : (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name)) + "\" type=\"" + (obj.FilterType == "Visits" ? "trips" : "shopper") + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\"title=\"" + ToolTip + "\" class=\"lft-popup-ele GeotooltipI" + (obj.IsHeader ? " priorityclass" : "") + "\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"true\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                            //}
                        else
                            html = "<li is-geography=\"" + obj.IsGeography + "\" metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" onclick=\"" + (obj.IsHeader ? "" : (obj.IsFrequency ? "SelectFrequency(this);" : onclick_event_name)) + "\" priorityid=\"" + obj.IsPriority + "\" type=\"" + obj.DispFilterType + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\" LevelDesc=\"" + obj.LevelDesc + "\" SelID=\"" + obj.SelID + "\" FilterTypeId=\"" + obj.FilterTypeId + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\"title=\"" + ToolTip + "\" class=\"lft-popup-ele GeotooltipI" + (obj.IsHeader ? " priorityclass" : "") + "\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"false\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\" ChartTypeTrend=\"" + obj.ChartTypeTrend + "\" ChartTypePIT=\"" + obj.ChartTypePIT + "\">" + obj.Name + "</span></li>";
                    }
                    $("#" + filterid + " .level" + filter[0].Levels[level].Id + " ul").append(html);
                }
            }
        }
        
    }
    filterChannelBasedOnView();
    //for measure tooltip
    $("#retailer-measure div[level-id='1'] .gouptype,#CorrespondenceMeasureDivId div[level-id='1'] .gouptype,#total-measure-trip div[level-id='1'] .gouptype").hover(function () {
        // Hover over code      
        if (($(this).parent().parent().parent().attr('id') == "retailer-measure" && (ModuleBlock == "TREND" || $("#groupDivId div[level-id='1'] .gouptype").length == 0)) || ($(this).parent().parent().parent().attr('id') == "CorrespondenceMeasureDivId" && $("#groupDivId div[level-id='1'] .gouptype").length == 0)) {
            return false;
        }
        else{
        var title = $(this).attr('title');
        var GroupNamelist = [];
        var ShopperGrps = [];
        if ($(this).hasClass("gouptype") && ModuleBlock != "TREND") {
            var filtertype = $(this).attr("filtertype").toLocaleLowerCase();
            $("#groupDivId div[level-id='1'] .gouptype").each(function () {
                var groupname = $(this).attr("filtertype").toLocaleLowerCase();
                if (filtertype.indexOf("demographic") > -1) {
                    GroupNamelist.push($(this).attr("primefiltertype"));
                }
                else if (groupname.indexOf(filtertype) > -1 || groupname == "demographics" || groupname == "demographic"
                     || filtertype == "demographics" || filtertype == "demographic") {
                    GroupNamelist.push($(this).attr("primefiltertype"));
                }
            });
        }
        title = "This measure is valid for only " + GroupNamelist.join(", ") + " Groups";

        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px",
            }).show();
        }
    }
    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.GeoToolTip')
            .css({ top: mousey, left: mousex })
    });

    //end measure tooltip
    $(".FilterPopup, .rgt-cntrl-SubFilter-Conatianer").scroll(function () {
        SetFilterLayerScroll();
    });
    $("#visit_frequency_containerId ul li, #frequency_containerId_trips ul li, #frequency_containerId_trips ul li").attr("tabtype", "trips");
    $("#shopper_frequency_containerId ul li").attr("tabtype", "shopper");
}
//ger filter
function getFilter(name)
{
    if (filters != null && filters != undefined && filters.filters != null && filters.filters != undefined && name != undefined && name != '') {
        for (var i = 0; i < filters.filters.length; i++) {
            if (filters.filters[i][0].Name.toLowerCase() == name.toLowerCase())
                return filters.filters[i];
        }
    }
    return null;
}
function AddOrUpdatefilter(name, filter) {
    var isfilterexist = false;
    for (var i = 0; i < filters.filters.length; i++) {
        if (filters.filters[i][0].Name.toLowerCase() == name.toLowerCase()) {
            filters.filters[i] = filter;
            isfilterexist = true;
        }
    }
    if (!isfilterexist)
        filters.filters.push(filter)
}

//get view filters
function getCurrentViewFilters() {
    if ($("#hdn-page").length == 0)
        return false;

    for (var i = 0; i < viewFilters.length; i++) {
        if (viewFilters[i].Name.toLowerCase() == $("#hdn-page").attr("name").toLowerCase())
            return viewFilters[i];
    }
    return false;
}

//prepare filters
function PrepareFilters(data)
{
    if (currentpage == "hdn-crossretailer-sarreport")
    {
        $("#Sar-Retailers").show();
        $("#Competitors").show();
        $("#Sar-Frequency").show();
    }
    else if (currentpage.indexOf("retailer") > -1)
        $("#Retailers").show();
    else if (currentpage.indexOf("beverage") > -1)
        $("#Beverages").show();

    customRegions = getFilter("Default Geography");
    LoadTimePeriod(data);   
    LoadRightFilter();
    LoadRightPanel(data);
    LoadFrequencyFilter(data);
    LoadChannelFilters(data);
    prepareViewFilters();
    SetDefaultValues();
    $(document).on("mouseover", ".Lavel ul li[donothover!='true']", function () {
        if (!($(this).hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray") && !($(this).find("div").hasClass("Selected"))) {
            $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
        }
    });
    $(document).on("mouseleave", ".Lavel ul li[donothover!='true']", function () {
        $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
        if (!$(this).hasClass("Selected") && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray") && !($(this).find("div").hasClass("Selected"))) {
            $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D");
        }

    });

    $(document).on("mouseover", ".lft-popup-ele[donothover!='true']", function () {
        if (!($(this).hasClass("Selected")) && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) {
            $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#EB1F2A");
        }
    });
    $(document).on("mouseleave", ".lft-popup-ele[donothover!='true']", function () {
        if (!$(this).hasClass("Selected") && !($(this).find("div").hasClass("Selected")) && !($(this).css("background-color") == "rgb(128, 128, 128)" || $(this).css("background-color") == "gray")) {
            $(this).find(".ArrowContainerdiv").eq(0).css("background-color", "#58554D")
        };
    });
}
//document ready events
$(document).ready(function () {   
    $(document).on("click", ".ArrowContainerdiv, .show-level", function (e) {
        var obj = $(this);
        if ($(this).hasClass("show-level"))
            obj = $(this).children(".FilterStringContainerdiv").children(".ArrowContainerdiv");

        if ($(obj).parent().parent("li").hasClass("in-active-item") || $(obj).parent().parent("li").css("background-color") == "rgb(128, 128, 128)") {
            return false;           
        }

        var headerText = $(obj).parent().parent("li").children().children("span.lft-popup-ele-label").text();
        var parentDiv = $(obj).parent().parent().parent().parent().parent().children(".Lavel").eq(0).offsetParent();
        //var parentDiv = $(obj).parent().parent().parent().parent().parent().children(".Lavel").eq(0).parent();
        var parentDivID = $(obj).parent().parent().parent().parent().parent().children(".Lavel").eq(0).parent().attr('id');
        $(obj).parent().parent().parent().parent().children("ul").children("li[donothover!='true']").children("div").children("div").children("span").removeClass("sidearrw_OnCLick").addClass("sidearrw");
        $(obj).children("span").addClass("sidearrw_OnCLick");
      
        //add Geography
        if ($(obj).attr("name") != undefined && $(obj).attr("name") != '' &&
            $(obj).attr("name").toLowerCase() == "geography") {
            addGeographyFilter(obj);
        }
        //if ($(obj).attr("name") != undefined && $(obj).attr("name") != '' &&
        //     $(obj).attr("name").toLowerCase() == "cross-retailer shopper") {
        //    addCompetitorFreequencyFilter(obj);
        //}
        //add existing items if not exist for selected level
        var name = $(obj).parent("div").parent("li").attr("name");
        var id = $(obj).parent("div").parent("li").attr("id");
        if ($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").next(".Lavel").find("ul li[parentid='" + id + "']").length == 0)
        {
            var lilist = $(obj).parent().parent().parent().parent().parent().find(".Lavel ul li[parentid='" + id + "']");
            if (lilist.length > 0)
            {
                $(lilist).each(function () {
                    $(this).appendTo($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").next(".Lavel").children("ul"));
                });
            }           
        }
        //if ($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").next(".Lavel").find("ul li[parentname='" + name + "'][parentid='" + id + "']").length == 0)
        //{
        //    var lilist = $(obj).parent().parent().parent().parent().parent().find(".Lavel ul li[parentname='" + name + "'][parentid='" + id + "']");
        //    if (lilist.length > 0)
        //    {
        //        $(lilist).each(function () {
        //            $(this).appendTo($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").next(".Lavel").children("ul"));
        //        });
        //    }           
        //}
        if (currentpage == "hdn-crossretailer-sarreport" && $(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id") == 1) {
            $(".SarSearch").css("display", "inline-block");
            $(".SarSelectionText").css("display", "inline-flex");
        }
        if (parentDivID == "SarRetailerDivId" && $(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id") == 1)
        {
            //selectedCustomBaseOrBenchMark = $(obj).parent("div").parent("li").attr("parentname").trim().toLowerCase()
            selectedCustomBaseOrBenchMark = $(obj).parent("div").parent("li").attr("parent-of-parent").trim().toLowerCase()
            sarRetailerCustomBaseOrBenchMarkClicked = true
            updateSearch($("#Sar-Retailers"))
        }
        //--end
        var isSelectable = false;
        var activelevel = 0;
        $(obj).parent().parent().parent().parent().parent().children(".Lavel").each(function (i) {
            if ($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id") != $(this).attr("level-id")
               && $(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").length > 0 && $(obj).parent("div").parent("li").attr("has-metric-sort-id") == "true") {

                $(this).children("ul").children("li").hide();
                $(this).css("display", "inline-block");

                 var headerID = Number($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id"));//$(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").parent().parent().attr("level-id");

                //To hide previoud header divs
                parentDiv.find(".HeaderLevel").children(".lft-popup-col-selected-text").each(function (i) {
                    if (Number($(this).eq(0).attr('class').split(' ')[1].charAt(11)) > headerID){
                        $(this).hide();
                        /*To romove white arrow*/
                        $(".FilterPopup .level" + Number($(this).eq(0).attr('class').split(' ')[1].charAt(11)) + " li").find('.sidearrw_OnCLick').removeClass('sidearrw_OnCLick').addClass('sidearrw');
                        /*Ends*/
                    }
                });
                //Ends To hide previoud header divs

                $(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "'][metric-sort-id='" + $(obj).parent("div").parent("li").attr("metric-sort-id") + "']").show();

                if ($(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "'][metric-sort-id='" + $(obj).parent("div").parent("li").attr("metric-sort-id") + "']").find(".lft-popup-ele-label").attr("data-isselectable") == "true") {
                    isSelectable = true;                
                }
                activelevel++;
                var headerWidth = activelevel * 287;

                if ($(obj).parent("div").parent("li").attr("show-all") == 'false' && activelevel > 1){
                    $(this).hide();
                }
                else {
                    parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).css("width", headerWidth + "px");
                }

                //Adding the heading to the levelHeader                
                parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).text(headerText);
                parentDiv.find(".HeaderLevel").children(".lft-popup-col-selected-text").css("white-space", "normal");
                parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).show();

                //End Adding the heading to the levelHeader
            }
            else if ((parentDivID == "SarRetailerDivId" || parentDivID == "SarCompetitorDivId") && $(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id") != $(this).attr("level-id")
                && $(this).children("ul").children("li[parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").length > 0) {

                $(this).children("ul").children("li").hide();
                $(this).css("display", "inline-block");

                var headerID = Number($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id"));//$(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").parent().parent().attr("level-id");

                if (parentDivID == "SarRetailerDivId" || parentDivID == "SarCompetitorDivId") {
                    headerID = headerID - 1;
                }

                //To hide previoud header divs
                parentDiv.find(".HeaderLevel").children(".lft-popup-col-selected-text").each(function (i) {
                    if ($(this).eq(0).hasClass("sar-first-lft-popup-col-text"))
                    {
                        return
                    }
                    if (Number($(this).eq(0).attr('class').split(' ')[1].charAt(11)) > headerID){
                        $(this).hide();
                        /*To romove white arrow*/
                        let currentLevel = Number($(this).eq(0).attr('class').split(' ')[1].charAt(11))
                        if (parentDivID == "SarRetailerDivId" || parentDivID == "SarCompetitorDivId")
                        {
                            currentLevel = currentLevel + 1;
                        }
                        $(".FilterPopup .level" + (currentLevel) + " li").find('.sidearrw_OnCLick').removeClass('sidearrw_OnCLick').addClass('sidearrw');
                        /*Ends*/
                    }
                });
                //Ends To hide previoud header divs
                if (parentDivID == "SarRetailerDivId" || parentDivID == "SarCompetitorDivId") {
                    if ($(obj).parent("div").parent("li").attr("parent-of-parent") == "null" || $(obj).parent("div").parent("li").attr("parent-of-parent") == null || $(obj).parent("div").parent("li").attr("parent-of-parent") == undefined || $(obj).parent("div").parent("li").attr("parent-of-parent") == "")
                    {
                        $(this).children("ul").children("li[parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").show();

                        if ($(this).children("ul").children("li[parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").find(".lft-popup-ele-label").attr("data-isselectable") == "true") {
                            isSelectable = true;
                        }
                    }
                    else {
                        $(this).children("ul").children("li[parentid='" + $(obj).parent("div").parent("li").attr("id") + "'][parent-of-parent='" + $(obj).parent("div").parent("li").attr("parent-of-parent") + "']").show();
                        $(this).children("ul").children("li.priorityclass[parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").show()

                        if ($(this).children("ul").children("li[parentid='" + $(obj).parent("div").parent("li").attr("id") + "'][parent-of-parent='" + $(obj).parent("div").parent("li").attr("parent-of-parent") + "']").find(".lft-popup-ele-label").attr("data-isselectable") == "true") {
                            isSelectable = true;
                        }
                    }
                   
                }
                else {
                    $(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").show();
               
                if ($(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").find(".lft-popup-ele-label").attr("data-isselectable") == "true")
                {
                    isSelectable = true;                  
                }
                }

                
                activelevel++;
                var headerWidth = activelevel * 287;

                if ($(obj).parent("div").parent("li").attr("show-all") == 'false' && activelevel > 1) {
                    $(this).hide();
                    parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).css("width", "287px");
                }
                else {
                    parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).css("width", headerWidth + "px");
                }

                if (headerID!=0) {
                    parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).text(headerText);
                }
                //Adding the heading to the levelHeader                
                //parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).text(headerText);
                parentDiv.find(".HeaderLevel").children(".lft-popup-col-selected-text").css("white-space", "normal");
                parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).show();
                //End Adding the heading to the levelHeader
                if (parentDiv.attr('class') != undefined && parentDiv.attr('class').indexOf("cntrl") > -1) {

                    if(headerID > 0){
                        parentDiv.css("width","586px");
                        parentDiv.find(".HeaderLevel").css("width", "586px");
                    }
                    else {
                        parentDiv.css("width","287px");
                        parentDiv.find(".HeaderLevel").css("width","287px");
                    }
                }
               
            }
            else if (!(parentDivID == "SarRetailerDivId" || parentDivID == "SarCompetitorDivId") && $(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id") != $(this).attr("level-id")
           && $(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").length > 0) {

                $(this).children("ul").children("li").hide();
                $(this).css("display", "inline-block");

                var headerID = Number($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id"));//$(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").parent().parent().attr("level-id");

                //To hide previoud header divs
                parentDiv.find(".HeaderLevel").children(".lft-popup-col-selected-text").each(function (i) {
                    if (Number($(this).eq(0).attr('class').split(' ')[1].charAt(11)) > headerID) {
                        $(this).hide();
                        /*To romove white arrow*/
                        $(".FilterPopup .level" + Number($(this).eq(0).attr('class').split(' ')[1].charAt(11)) + " li").find('.sidearrw_OnCLick').removeClass('sidearrw_OnCLick').addClass('sidearrw');
                        /*Ends*/
                    }
                });
                //Ends To hide previoud header divs

                $(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").show();

                if ($(this).children("ul").children("li[parentname='" + $(obj).parent("div").parent("li").attr("name") + "'][parentid='" + $(obj).parent("div").parent("li").attr("id") + "']").find(".lft-popup-ele-label").attr("data-isselectable") == "true") {
                    isSelectable = true;
                }
                activelevel++;
                var headerWidth = activelevel * 287;

                if ($(obj).parent("div").parent("li").attr("show-all") == 'false' && activelevel > 1) {
                    $(this).hide();
                    parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).css("width", "287px");
                }
                else {
                    parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).css("width", headerWidth + "px");
                }

                //Adding the heading to the levelHeader                
                parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).text(headerText);
                parentDiv.find(".HeaderLevel").children(".lft-popup-col-selected-text").css("white-space", "normal");
                parentDiv.find(".HeaderLevel").children(".Headerlevel" + String(headerID + 1)).show();
                //End Adding the heading to the levelHeader
                if (parentDiv.attr('class') != undefined && parentDiv.attr('class').indexOf("cntrl") > -1) {

                    if (headerID > 0) {
                        parentDiv.css("width", "586px");
                        parentDiv.find(".HeaderLevel").css("width", "586px");
                    }
                    else {
                        parentDiv.css("width", "287px");
                        parentDiv.find(".HeaderLevel").css("width", "287px");
                    }
                }

            }
            else
            {
                if (parseInt($(this).attr("level-id")) > parseInt($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id")))
                    $(this).hide();               
            }
          
        });        
        HideGroupsFromAdvfilters();
        if ($(obj).attr("filter-id") != undefined && $(obj).attr("filter-id") != '' && $(obj).attr("filter-id").toLowerCase() == "channel-content")
        {
            $(".rgt-cntrl-chnl .AdvancedFiltersDemoHeading").css("width", "auto");
            $(".rgt-cntrl-chnl .AdvancedFiltersDemoHeading").css("width", $(".rgt-cntrl-chnl").get(0).scrollWidth + "px");            
        }
        if ($(obj).attr("popup-name") != undefined && $(obj).attr("popup-name") != null && $(obj).attr("popup-name") != '')
            SetAddFilterPanelWidth($(obj).attr("popup-name"));

        if (currentpage.indexOf("chart") > -1 && $(obj).parent("div").parent("li").parent("ul").parent(".Lavel").parent().attr("id") == "retailer-measure" && isSelectable) {
            SelecMeasure($(obj).parent().parent());
        }
        if ($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id") == 1 && $(obj).parent("div").parent("li").parent("ul").parent(".Lavel").parent().attr("id") == "CorrespondenceMeasureDivId") {
            DisplaySecondaryAdvancedAnalytics($(obj).parent().parent());
        }
        if ($(obj).parent("div").parent("li").parent("ul").parent(".Lavel").parent().attr("id") == "groupDivId" && isSelectable) {
            SelecGroup($(obj).parent().parent());
        }

        if (currentpage == "hdn-crossretailer-totalrespondentstripsreport" && $(obj).attr("event-name") == "SelectTotalMeasure(this);")
        {
            DisplaySecondaryTotalFilter($(obj).parent().parent());
        }
        if (currentpage == "hdn-dashboard-demographic" && $(obj).attr("name") == "ADDITIONAL FILTERS" && (obj).parent("div").parent("li").parent("ul").parent(".Lavel").attr("level-id") == 1 && $(obj).parent("div").parent("li").parent("ul").parent(".Lavel").parent().attr("id") == "AdvFilterDivId") {
            $("#AdvFilterDivId li[filtertype=FREQUENCY]").show();
            if(TabType.toLowerCase() == "shopper"){
                $("#AdvFilterDivId .level2 ul").find('li[parentname="ADDITIONAL FILTERS"]').hide();
                $("#AdvFilterDivId li[filtertype=FREQUENCY]").show();
            }
            else if (TabType.toLowerCase() == "trips") {
                $("#AdvFilterDivId .level2 ul").find('li[parentname="ADDITIONAL FILTERS"]').show();
        }
        }
        if (parentDivID == "AdvFilterDivId") {
            $("#AdvFilterDivId *").find('li[name="Total"][uniqueid="4000"]').hide();
        }
        $("#AdvFilterDivId ul li[name='TOTAL VISITS'], .Custombase-GroupType ul li[name='TOTAL VISITS'], .GroupType ul li[name='TOTAL VISITS']").hide();

        updateFilterWidth(obj);
        //set header width       
        SetFilterLayerScroll();
    });
});
function HideGroupsFromAdvfilters() {
    var groupname = "";
    var measurename = "";
    $("#AdvFilterDivId div[level-id='1'] ul li").show();
    if (Grouplist.length > 0 || Measurelist.length > 0) {
        if (Grouplist.length > 0)
            groupname = Grouplist[0].parentName;
        if (Measurelist.length > 0)
            measurename = Measurelist[0].parentName;

        for (var levelId = 1; levelId <= $("#AdvFilterDivId .Lavel").length; levelId++) {
            $("#AdvFilterDivId div[level-id='" + levelId + "'] ul li[name='" + groupname + "']").hide();
            $("#AdvFilterDivId div[level-id='" + levelId + "'] ul li[name='" + measurename + "']").hide();
        }
        for (var levelId = 1; levelId <= $("#retailer-measure .Lavel").length; levelId++) {           
            $("#retailer-measure div[level-id='" + levelId + "'] ul li[name='" + groupname + "']").hide();
        }
        //for measure
       
        //$("#MeasureTypeHeaderContentShopper ul").children("li[name='" + groupname + "']").hide();

        if (Grouplist.length > 0 && Grouplist[0].isGeography == "true")
            //$("#AdvFilterDivId #PrimaryAdvancedFilterContent #PrimaryDemoFilterList div[name='Geography']").parent("li").hide();
            $("#AdvFilterDivId div[level-id='1'] ul li[name='Geography']").hide();
    }
    RemoveGroupAdvFilters(groupname, measurename);
}
function updateFilterWidth(obj)
{    
    $(obj).parent().parent().parent().parent().parent().css("width", "auto");
    $(obj).parent().parent().parent().parent().parent().prev().css("width", "auto");   

    var header_container = $(obj).parent().parent().parent().parent().parent().prev();
    if(header_container.length > 0 && $(header_container).hasClass("AdvancedFiltersDemoHeading"))
    {
        var scroll_width = 0;
        if ($(obj).parent().parent().parent().parent().parent().attr("id") == "channel-content") {
            scroll_width = $(obj).parent().parent().parent().parent().parent().get(0).scrollWidth;
        }
        else {
            scroll_width = $(obj).parent().parent().parent().parent().parent().innerWidth();
        }
        $(header_container).css("width", scroll_width + "px");
        $(header_container).next("div").css("width", scroll_width + "px");
    }
    SetScroll($("#channel-content"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
}
function getGeoDisplayStyle(name)
{
    var geoObjstyles = new Object();
    for (var i = 0; i < geo_fiter_styles.length; i++) {
        if(geo_fiter_styles[i].Name.toLowerCase() == name.toLowerCase())
        {
            geoObjstyles = geo_fiter_styles[i];
            break;
        }
    }
    return geoObjstyles;
}
function addGeographyFilter(geoobj)
{
    var filter = customRegions;
    var filterid = $(geoobj).attr("filter-id");
    var onclick_event_name = $(geoobj).attr("event-name");
    var level_id = parseInt($(geoobj).parent().parent().parent().parent(".Lavel").attr("level-id"), 0) + 1;
    geo_fiter_styles = [];
    $("#" + filterid + " ul li[is-geography='true']").each(function (i) {
        var selected = $(this).hasClass("Selected") ? "Selected" : "";
        var arrow_selected = $(this).find(".lft-popup-ele-next").hasClass("sidearrw_OnCLick") ? "sidearrw_OnCLick" : "";
        geo_fiter_styles.push({ Name: $(this).attr("name"), Display: $(this).css("display"), Selected: selected, ArrowSelected:arrow_selected });
    });
    $("#" + filterid + " ul li[is-geography='true']").remove();

    //update levels
    for (var level = 0; level < filter[0].Levels.length; level++) {
        filter[0].Levels[level].Id = level_id;
        level_id++;
    }

    //add levels
    for (var level = 0; level < filter[0].Levels.length; level++) {
        if (filter[0].Levels[level].LevelItems.length > 0) {
            if ($("#" + filterid + " div[level-id='" + (filter[0].Levels[level].Id) + "']").length == 0)
                $("#" + filterid).append("<div level-id=\"" + (filter[0].Levels[level].Id) + "\" level=\"level" + (filter[0].Levels[level].Id) + "\" class=\"Lavel level" + (filter[0].Levels[level].Id) + "\" style=\"display: none;\"><ul></ul></div>");
        }
    }
    //add filter items
    for (var level = 0; level < filter[0].Levels.length; level++) {
        for (var i = 0; i < filter[0].Levels[level].LevelItems.length; i++) {
            var obj = filter[0].Levels[level].LevelItems[i];
            if (level == 0)
            {
                obj.ParentName = "GEOGRAPHY";
                obj.ParentId = 100;
            }
            var IsActive = getRegionActiveTimePeriod(obj) && !disableGeographiesWithTimeperiod(obj);
            var ToolTip = getRegionTimePeriodToolTip(obj);
            var geoObjstyles = getGeoDisplayStyle(obj.Name);
            if (obj.HasSubLevel && obj.IsSelectable)
                html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:" + geoObjstyles.Display + ";\" is-geography=\"" + obj.IsGeography + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype " + geoObjstyles.Selected + "\"><div class=\"FilterStringContainerdiv\" style=\"\"><span onclick=\"" + onclick_event_name + "\" style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\">" + obj.Name + "</span><div class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw " + geoObjstyles.ArrowSelected + "\"></span></div></div></li>";
            else if (obj.HasSubLevel && IsActive)
                html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:" + geoObjstyles.Display + ";\" is-geography=\"" + obj.IsGeography + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype show-level " + geoObjstyles.Selected + "\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\">" + obj.Name + "</span><span style=\"float:left;\" class=\"lft-popup-ele-next Geotooltipimage\" title=\"" + ToolTip + "\"></span><div class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw " + geoObjstyles.ArrowSelected + "\"></span></div></div></li>";
            else if (obj.HasSubLevel && !IsActive)
                html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:" + geoObjstyles.Display + ";\" is-geography=\"" + obj.IsGeography + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype show-level in-active-item " + geoObjstyles.Selected + "\"><div class=\"FilterStringContainerdiv\" style=\"\"><span style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\">" + obj.Name + "</span><span style=\"float:left;\" class=\"lft-popup-ele-next Geotooltipimage\" title=\"" + ToolTip + "\"></span><div class=\"ArrowContainerdiv\" style=\"background-color: rgb(88, 85, 77);\"><span class=\"lft-popup-ele-next sidearrw " + geoObjstyles.ArrowSelected + "\"></span></div></div></li>";
            else if (!IsActive)
                html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:" + geoObjstyles.Display + ";\" is-geography=\"" + obj.IsGeography + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" class=\"lft-popup-ele in-active-item " + geoObjstyles.Selected + "\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"true\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\">" + obj.Name + "</span><span style=\"float:left;\" class=\"lft-popup-ele-next Geotooltipimage\" title=\"" + ToolTip + "\"></span></li>";
            else if (ToolTip != null && ToolTip !="" )
                html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:" + geoObjstyles.Display + ";\" is-geography=\"" + obj.IsGeography + "\" onclick=\"" + onclick_event_name + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" class=\"lft-popup-ele " + geoObjstyles.Selected + "\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"true\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\">" + obj.Name + "</span><span style=\"float:left;\" class=\"lft-popup-ele-next Geotooltipimage\" title=\"" + ToolTip + "\"></span></li>";
            else
                html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:" + geoObjstyles.Display + ";\" is-geography=\"" + obj.IsGeography + "\" onclick=\"" + onclick_event_name + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" id=\"" + obj.Id + "\"  uniqueid=\"" + obj.UniqueId + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" class=\"lft-popup-ele " + geoObjstyles.Selected + "\" style=\"\"><span class=\"lft-popup-ele-label\" isgeography=\"true\" uniqueid=\"" + obj.UniqueId + "\" id=\"" + obj.Id + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" data-isselectable=\"" + obj.IsSelectable + "\">" + obj.Name + "</span></li>";

            if (obj.ToolTip == '') {
                html = html.replace('Geotooltipimage','')
            }
            $("#" + filterid + " .level" + (filter[0].Levels[level].Id) + " ul").append(html);          
        }      
    }
    $('.Geotooltipimage').hover(function () {
        // Hover over code
        var title = $(this).attr('title');
        if (title != undefined && title != "" && title != null) {
            $(this).data('tipText', title).removeAttr('title');
            $('<p class="GeoToolTip"></p>')
            .text(title)
            .appendTo('body')
            .fadeIn('slow');

            var pos = $(this).position();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();
            //show the menu directly over the placeholder          
            $(".GeoToolTip").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left - width) + "px",
            }).show();
        }

    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.GeoToolTip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 10; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        if ((mousex + 100) > $("#RightPanel").width()) {
            $(".GeoToolTip").css('max-width','240px');
            $('.GeoToolTip').css({ top: mousey, left: mousex - $(".GeoToolTip").width() - 10});
        }
        else {
            $('.GeoToolTip').css({ top: mousey, left: mousex })
        }       
    });
}
function getRegionTimePeriodToolTip(obj) {
    if (ModuleBlock.toUpperCase() == "TREND")
        return obj.TrendToolTip;
    else
        return obj.ToolTip;
}
function getRegionActiveTimePeriod(obj)
{
    //if (obj.Name.toLowerCase() == 'total')
    //    return true;
    //if (ModuleBlock.toUpperCase() != "TREND") {
    //    if (TimeExtension.toLowerCase() == "year" || TimeExtension.toLowerCase() == "total time")
    //        return true;
    //}
    var IsActive = false;
    var selected_timeperiod = TimePeriodName.split(' ')[0];
    if (TimePeriodName.toLowerCase().indexOf("ytd") > -1)
        selected_timeperiod = TimePeriodName.split(' ')[1];

    if (ModuleBlock.toUpperCase() != "TREND") {
        if (TimeExtension.toLowerCase() == "year" || TimeExtension.toLowerCase() == "total time")
            selected_timeperiod = TimeExtension;
    }

    var geoTimePeriodType = "";
    var geoTimePeriod = "";
    var geoTimePeriodlist = [];
    var timePeriodlist = obj.GeoTimePeriods;       
    if (ModuleBlock.toUpperCase() == "TREND") {
        for (var i = 0; i < obj.GeoTimePeriods.length; i++) {
            geoTimePeriodlist = obj.GeoTimePeriods[i].split('|');
            geoTimePeriodType = geoTimePeriodlist[0];
            geoTimePeriod = geoTimePeriodlist[1];
            var index = TimeExtension.toLowerCase().indexOf("ytd") > -1 ? 1 : 0;
            if (TimeExtension.toLowerCase() == geoTimePeriodType.toLowerCase() && (TimePeriod_To.split(' ')[index].toLowerCase() == geoTimePeriod.toLowerCase()) || TimeExtension.toLowerCase() == "year") {
                IsActive = true;
                break;
            }
        }
    }
    else 
    {
        for (var i = 0; i < obj.GeoTimePeriods.length; i++) {
            geoTimePeriodlist = obj.GeoTimePeriods[i].split('|');
            geoTimePeriodType = geoTimePeriodlist[0];
            geoTimePeriod = geoTimePeriodlist[1];
            if (TimeExtension.toLowerCase() == geoTimePeriodType.toLowerCase() && selected_timeperiod.toLowerCase() == geoTimePeriod.toLowerCase()) {
                IsActive = true;
                break;
            }
        }
    }
    return IsActive;
}
function resetLevels(obj)
{
    $(".AdvancedFiltersDemoHeading").css("width", "auto");
    $(".AdvancedFiltersDemoHeading").next("div").css("width", "auto");
    var IsFirstLevel = 0;
    $("." + $(obj).attr("id") + " .Lavel .ArrowContainerdiv span").removeClass("sidearrw_OnCLick").addClass("sidearrw");
    $("." + $(obj).attr("id") + " .Lavel").each(function (i) {
        if ($(this).attr("level") != undefined && $(this).attr("level") != '')
            IsFirstLevel++;

        if (IsFirstLevel == 1) {
            $(this).css("display", "inline-block");          
        }
        else
            $(this).hide();
    });
    $(".lft-popup-col-selected-text").hide();
    $(".Headerlevel1").show();
    if (currentpage == "hdn-crossretailer-sarreport") {
        $(".SarSearch").hide();
        $(".SarSelectionText").hide();
        $(".sar-first-lft-popup-col-text").show();
        $(".sar-first-lft-popup-col-text").parent().find(".Headerlevel1").hide();
    }
}
//update search
//written by Nagaraju 
//Date: 19-12-2017
function updateSearch(obj) {
    if (sarRetailerCustomBaseOrBenchMarkClicked == false) {
        resetLevels(obj);
    }
    sarRetailerCustomBaseOrBenchMarkClicked = false;
    var searchItems = [];
    var filter = null;
    if ($(obj).attr("filter-name") != undefined && $(obj).attr("filter-name") != null && $(obj).attr("filter-name") != null) {
        if ($(obj).attr("filter-name").toLowerCase() == 'default geography')
            filter = customRegions;
        else
            filter = getFilter($(obj).attr("filter-name"));

        if (filter != null && filter != undefined) {
            if ($(obj).attr("filter-name").toLowerCase() != 'default geography')
                searchItems = getSearchItems(filter, obj);

            var search_params = $(obj).attr("search-params").split('|');
            if ($(obj).attr("update-geography") != undefined && $(obj).attr("update-geography") != '' && $(obj).attr("update-geography").toLowerCase() == 'true') {              
                var geosearchItems = getGeographySearchItems(customRegions,obj);
                searchItems = searchItems.concat(geosearchItems);
            }
            
            validationSearch($(obj).attr('id'),search_params[0], search_params[1], search_params[2], searchItems);
            //PrepareSearch(search_params[0], search_params[1], search_params[2], searchItems);
        }
    }
    else if ($(obj).attr("filtertype") == "FREQUENCY")
    {
        filter = getFilter("RETAILERS");
        if (filter != null && filter != undefined) {
            searchItems = getSearchItems(filter, obj);
            var search_params = ["Competitor-Retailer", "Search-CompetitorFrequency-Retailers", "CompetitorFrequency-Retailer-Search-Content"];
            if ($(obj).attr("update-geography") != undefined && $(obj).attr("update-geography") != '' && $(obj).attr("update-geography").toLowerCase() == 'true') {
                var geosearchItems = getGeographySearchItems(customRegions, obj);
                searchItems = searchItems.concat(geosearchItems);
            }

            validationSearch($(obj).attr('id'), search_params[0], search_params[1], search_params[2], searchItems);
        }

    }
}
//get geography search items
function getGeographySearchItems(filterList, geoobj) {
    var searchArray = [];
    if (filterList != undefined && filterList != null) {
        for (var level = 0; level < filterList[0].Levels.length; level++) {
            for (var i = 0; i < filterList[0].Levels[level].LevelItems.length; i++) {
                var obj = filterList[0].Levels[level].LevelItems[i];
                if (obj.IsSelectable && (getRegionActiveTimePeriod(obj) && !disableGeographiesWithTimeperiod(obj)))
                    searchArray.push({ label: obj.searchName, value: { LevelId: filterList[0].Levels[level].Id, Name: obj.Name, ParentName: obj.ParentName, UniqueId: obj.UniqueId, Id: obj.Id, IsGeography: true, GeoObj: $("." + $(geoobj).attr("id") + " .Lavel ul li div[name='GEOGRAPHY']") } });
            }
        }
    }
    return searchArray;
}
//get inactive measures
function getInactiveMeasures(filter_obj) {
    var inActiveMeasures = [];
    $("." + $(filter_obj).attr("id") + " div[level-id='1'] ul li").each(function () {
        if($(this).css("background-color") == "rgb(128, 128, 128)")
        {
            inActiveMeasures.push($(this).attr("name"));
        }
    });
    return inActiveMeasures;
}
//get search items
function getSearchItems(filterList, filter_obj) {
    var inActiveMeasures = [];
    inActiveMeasures = getInactiveMeasures(filter_obj);
    let sarRetailerSelected = false;
    if ($(filter_obj).attr('id') == "Sar-Retailers") {
        sarRetailerSelected = true;
    }
    var searchArray = [];
    if (filterList != undefined && filterList != null) {
        for (var level = 0; level < filterList[0].Levels.length; level++) {
            for (var i = 0; i < filterList[0].Levels[level].LevelItems.length; i++) {
                var obj = filterList[0].Levels[level].LevelItems[i];
                var Type = (currentpage == "hdn-dashboard-demographic") ? obj.PrimeFilterType : obj.MeasureType;
                if (obj.IsSelectable && obj.IsShowInSearch && (!sarRetailerSelected || sarRetailerSelected && selectedCustomBaseOrBenchMark != "" && ((obj.ParentName.trim().toLowerCase()==selectedCustomBaseOrBenchMark) || (obj.ParentOfParent.trim().toLowerCase()==selectedCustomBaseOrBenchMark))) && $.inArray(Type, inActiveMeasures) == -1)
                    searchArray.push({ label: obj.searchName, value: { LevelId: filterList[0].Levels[level].Id, Name: obj.Name, ParentName: obj.ParentName, UniqueId: obj.UniqueId, Id: obj.Id, IsGeography: false, ParentOfParent: obj.ParentOfParent } });
            }
        }
    }
    return searchArray;
}
function getSearchItemsFromElement(ele_Id) {
    var searchArray = [];
    $(ele_Id + " ul li").each(function (i) {
        if ($(this).find(".lft-popup-ele-label").attr("data-isselectable") == "true") {
            {
                var obj = $(this).find(".lft-popup-ele-label");
                if ($(obj).attr("onclick") != "SelectFrequency(this);" && $(obj).attr("name").toLowerCase() != "total visits") {
                    searchArray.push({ label: $(obj).attr("searchname"), value: { Name: $(obj).attr("name"), ParentName: $(obj).attr("parentname"), UniqueId: $(obj).attr("uniqueid"), Id: $(obj).attr("id"), IsGeography: false } });
                }
            }
        }
    });
    return searchArray;
}
function getAdditionalFiltersSearchItems(obj, ele_Id) {
    var searchArray = [];
    $(ele_Id + " ul li[parentid='" + $(obj).attr("id") + "']").each(function () {
        var childobj = $(this).children("span");
        if ($(childobj).attr("data-isselectable") == 'true')
            searchArray.push({ label: $(childobj).attr("searchname"), value: { Name: $(childobj).attr("name"), ParentName: $(childobj).attr("parentname"), UniqueId: $(childobj).attr("uniqueid"), Id: $(childobj).attr("parentid") } });
    });
    return searchArray;
}
//added by Nagaraju for filter layer scroll
//Date: 11-04-2017
function SetFilterLayerScroll() {
    $(".AdvancedFilters").getNiceScroll().remove();
    $(".GroupContentDiv").getNiceScroll().remove();
    SetScroll("#AdvFilterDivId", left_scroll_bgcolor, 0, -5, 0, 0, 8);
    SetScroll("#groupDivId", left_scroll_bgcolor, 0, -5, 0, 0, 8);
    $(".Lavel").each(function () {
        SetScroll($(this), left_scroll_bgcolor, 0, -5, 0, 0, 8);
    }); 
}
function SetScroll(Obj, cursor_color, top, right, left, bottom, cursorwidth) {
    $(Obj).niceScroll({
        cursorcolor: cursor_color,
        cursorborder: cursor_color,
        cursorwidth: cursorwidth + "px",
        autohidemode: false,
        zindex: "10000",
        railpadding: {
            top: top,
            right: right,
            left: left,
            bottom: bottom
        },
    });
    if ($(Obj).attr("id") == "MeasureContainerDivId") {
        $(Obj).niceScroll().scrollstart(function (info) {
            $("#MeasureTypeHeaderContentSubLevelTrip").getNiceScroll().hide();
            $("#MeasureTypeHeaderContentSubLevelShopper").getNiceScroll().hide();
        });
        $(Obj).niceScroll().scrollend(function (info) {
            $("#MeasureTypeHeaderContentSubLevelTrip").getNiceScroll().show();
            $("#MeasureTypeHeaderContentSubLevelShopper").getNiceScroll().show();
            SetScroll($("#MeasureTypeHeaderContentSubLevelTrip"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
            SetScroll($("#MeasureTypeHeaderContentSubLevelShopper"), left_scroll_bgcolor, 0, 0, 0, 0, 8);
        });
    }
    $(Obj).getNiceScroll().resize();  
}
function SetNiceScroll(Obj, cursor_color, top, right, left, bottom, cursorwidth, overflowx, overflowy) {
    $(Obj).niceScroll({
        cursorcolor: cursor_color,
        cursorborder: cursor_color,
        cursorwidth: cursorwidth + "px",
        autohidemode: false,
        overflowx: overflowx,
        overflowy: overflowy,
        railpadding: {
            top: top,
            right: right,
            left: left,
            bottom: bottom
        },
    });
    $(Obj).getNiceScroll().remove();
}
function getFilterImage(filterId, obj) {   
    if (!obj.IsShowImage)
        return "";
    
    var image_position = "";
    switch (obj.Name.toLowerCase().trim()) {
        case "total": image_position = "-932px -678px;";
            break;
        case "supermarket/grocery": image_position = "-1414px -360px;";
            break;
        case "convenience": image_position = "-1456px -360px;";
            break;
        case "drug": image_position = "-1646px -360px;";
            break;
        case "dollar": image_position = "-1603px -360px;";
            break;
        case "club": image_position = "-1545px -360px;";
            break;
        case "mass merc.": image_position = "-1498px -360px;";
            break;
        case "supercenter": image_position = "-1692px -360px;";
            break;
        case "corporate nets": image_position = "4px -678px;";
            break;
        case "channel nets": image_position = "-1490px -672px";
            break;
        case "regular (non-diet) carbonated soft drink": { image_position = "7px -297px;"; break; }
        case "rtd coffee": { image_position = "-76px -147px;"; break; }
        case "rtd tea": { image_position = "-115px -147px;"; break; }
        case "protein drinks": { image_position = "-943px -147px;"; break; }
        case "packaged 100% orange juice": { image_position = "-1156px -148px;"; break; }
        case "sparkling water (unflavored)": { image_position = "-1614px -149px;"; break; }
        case "flavored sparkling water": { image_position = "-1688px -149px;"; break; }
        case "category nets": { image_position = "10px -470px;"; break; }
        case "detailed categories": { image_position = "-36px -470px;"; break; }
        case "ssd regular": { image_position = "8px -149px;"; break; }
        case "ssd diet": { image_position = "-30px -149px;"; break; }
        case "rtd smoothies in a bottle": { image_position = "-1026px -149px;"; break; }
        case "packaged 100% fruit juice (non-oj)": { image_position = "-1240px -147px;"; break; }
        case "packaged 100% grape juice": { image_position = "-647px -678px;"; break; }
        case "packaged 100% apple juice": { image_position = "-694px -678px;"; break; }
        case "packaged 100% grapefruit juice": { image_position = "-740px -678px;"; break; }
        case "packaged 100% cranberry juice": { image_position = "-792px -678px;"; break; }
        case "packaged 100% fruit juice blends": { image_position = "-840px -678px;"; break; }
        case "vegetable juice/ vegetable + juice blend": { image_position = "-92px -471px;"; break; }
        case "other flavor 100% juice": { image_position = "-885px -678px;"; break; }
        case "sports drinks": { image_position = "-224px -468px;"; break; }
        case "energy drink/ shot": { image_position = "-275px -467px;"; break; }
        case "liquid flavor enhancer": { image_position = "-619px -570px;"; break; }
        case "enhanced milk": { image_position = "-90px -678px;"; break; }
        case "non-sparkling water - nets": { image_position = "-568px -678px;"; break; }
        case "sparkling water - nets": { image_position = "-484px -680px;"; break; }
        case "bottled water - nets": { image_position = "-606px -678px;"; break; }
        case "ssd regular/diet": { image_position = "-485px -681px;"; break; }
        case "rtd juice drink": { image_position = "-1493px -147px;"; break; }
        case "single serving bottled water (non-sparkling water)": { image_position = "-135px -470px;"; break; }
        case "flavored non-sparkling bottled water": { image_position = "-174px -470px;"; break; }
        case "juice/juice drinks/vege/smoothies - nets": { image_position = "-195px -677px;"; break; }
        case "other trademark/brand groups": { image_position = "-141px -678px;"; break; }
        case "total shopper": { image_position = "-932px -675px;"; break; }
        case "total online shopper": { image_position = "-1080px -672px;"; break; }
        case "total online grocery shopper": { image_position = "-1133px -672px;"; break; }
        case "sites": { image_position = "-1182px -676px;"; break; }
        case "hard seltzers and other flavored alcoholic beverages": { image_position = "0px 0px;"; break;}
    }
    var imghtml = "";
    if (image_position)
    {
        if (obj.Name.toLowerCase().trim() == "hard seltzers and other flavored alcoholic beverages") {
            imghtml = "<span class=\"img-retailer\" style=\"width:35px;height:31px;background-image: url('../Images/hard_seltzers_icons.svg?id=2');background-repeat: no-repeat;background-position:" + image_position + "\"></span>";
        }
        else {
            imghtml = "<span class=\"img-retailer\" style=\"width:35px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + image_position + "\"></span>";
        }
    }
        //imghtml = "<span class=\"img-retailer\" style=\"width:35px;height:31px;background-image: url('../Images/sprite_filter_icons.svg?id=2');background-position:" + image_position + "\"></span>";

    return imghtml;
}

function getEcomchannel(obj) {
    switch (obj.Name.toLowerCase()) {
        case "total shopper":
        case "total online shopper":
        case "total online grocery shopper":
            return true;
    }
    return false;
}

function validationSearch(objId,search_params0, search_params1, search_params2, searchItems) {
    if (objId == "AdvancedFilters") {
        //23-11-17
        _.each(_.filter(searchItems), function (item) {
            if (item.value.Name.toLowerCase() == "total")
                searchItems.splice($.inArray(item, searchItems), 1);
        });

        AllDemographicsSF = $.extend(true, [], searchItems);
        if (Grouplist.length > 0) {
            for (var j = 0; j < Grouplist.length; j++) {
                _.each(_.filter(AllDemographicsSF), function (item) {
                    if (item.value.ParentName == Grouplist[j].parentName)
                        AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                });
            }
        }
        if (Measurelist.length > 0) {
            for (var j = 0; j < Measurelist[0].metriclist.length; j++) {
                _.each(_.filter(AllDemographicsSF), function (item) {
                    if (item.value.ParentName == Measurelist[0].parentName)
                        AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                });
            }
        }

        if (Grouplist.length > 0 || Measurelist.length > 0) {
            PrepareSearch(search_params0, search_params1, search_params2, AllDemographicsSF);
        }
        else {
            PrepareSearch(search_params0, search_params1, search_params2, searchItems);
        }

      

    }
    else if (objId == "MeasureType" && (currentpage.indexOf("chart") > -1)) {
        //23-11-17
        var AllMeasuresSF = [];
        AllMeasuresSF = $.extend(true, [], searchItems);
        for (var j = 0; j < Grouplist.length; j++) {
            _.each(_.filter(AllMeasuresSF), function (item) {
                if (item.value.ParentName == Grouplist[j].parentName)
                    AllMeasuresSF.splice($.inArray(item, AllMeasuresSF), 1);
            });
        }
        //End 23-11-17
        $("#MeasureTypeHeaderMainTrip").show();
        $("#MeasureTypeHeaderMainTrip ul li").show();
        //23-11-17
        PrepareSearch(search_params0, search_params1, search_params2, Grouplist.length == 0 ? searchItems : AllMeasuresSF);
        //End 23-11-17
    }
    else if (currentpage == "hdn-report-compareretailersshoppers" || currentpage == "hdn-report-retailersshopperdeepdive" || currentpage == "hdn-report-comparebeveragesmonthlypluspurchasers" || currentpage == "hdn-report-beveragemonthlypluspurchasersdeepdive") {
        //23-11-17
          AllDemographicsSF = $.extend(true, [], searchItems);
        if (Grouplist.length > 0) {
            for (var j = 0; j < Grouplist.length; j++) {
                _.each(_.filter(AllDemographicsSF), function (item) {
                    if (item.value.ParentName == Grouplist[j].parentName)
                        AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                });
            }
        }
        if (Measurelist.length > 0) {
            for (var j = 0; j < Measurelist[0].metriclist.length; j++) {
                _.each(_.filter(AllDemographicsSF), function (item) {
                    if (item.value.ParentName == Measurelist[0].parentName)
                        AllDemographicsSF.splice($.inArray(item, AllDemographicsSF), 1);
                });
            }
        }
        //End 23-11-17
        $("#PrimaryAdvancedFilterContent").css("display", "inline-block");
        $("#DemoHeadingLevel1").show();
        $("#ToShowDemoAndAdvFilters").hide();
        $("#DemoHeadingLevel0").hide();
        //23-11-17
        if (Grouplist.length > 0 || Measurelist.length > 0) {
            PrepareSearch(search_params0, search_params1, search_params2, AllDemographicsSF);
        }
        else {
            PrepareSearch(search_params0, search_params1, search_params2, searchItems);
        }
        //End 23-11-17
    }
    else {
        PrepareSearch(search_params0, search_params1, search_params2, searchItems);
    }
}


function addCompetitorFreequencyFilter(Obj) {
    var filter = getFilter("Cross-Retailer Shopper");;
    var filterid = $(Obj).attr("filter-id");
    var onclick_event_name = "OpenOrCloseCompetitorFrequencyRetailerPopup(this)";//$(Obj).attr("event-name");
    var level_id = parseInt($(Obj).parent().parent().parent().parent(".Lavel").attr("level-id"), 0) + 1;

    //update levels
    for (var level = 0; level < filter[0].Levels.length; level++) {
        filter[0].Levels[level].Id = level_id;
        level_id++;
    }

    $("#" + filterid + " ul li[parentname='Cross-Retailer Shopper']").remove();

    //add levels
    for (var level = 0; level < filter[0].Levels.length; level++) {
        if (filter[0].Levels[level].LevelItems.length > 0) {
            if ($("#" + filterid + " div[level-id='" + (filter[0].Levels[level].Id) + "']").length == 0)
                $("#" + filterid).append("<div level-id=\"" + (filter[0].Levels[level].Id) + "\" level=\"level" + (filter[0].Levels[level].Id) + "\" class=\"Lavel level" + (filter[0].Levels[level].Id) + "\" style=\"display: none;\"><ul></ul></div>");
        }
    }
    //add filter items
    for (var level = 0; level < filter[0].Levels.length; level++) {
        for (var i = 0; i < filter[0].Levels[level].LevelItems.length; i++) {
            var obj = filter[0].Levels[level].LevelItems[i];
            if (level == 0) {
                obj.ParentName = "Cross-Retailer Shopper";
                obj.ParentId = 101;
                obj.IsSelectable = false;
            }
            html = "<li  metric-sort-id=\"" + obj.MetricSortId + "\" has-metric-sort-id=\"" + obj.HasMetricSortId + "\" show-all=\"" + obj.ShowAll + "\" style=\"display:;\" is-geography=\"" + obj.IsGeography + "\" primefiltertype=\"" + obj.PrimeFilterType + "\" filtertype=\"" + obj.FilterType + "\" id=\"" + obj.Id + "\" data-isselectable=\"" + obj.IsSelectable + "\" parentid=\"" + obj.ParentId + "\" parentname=\"" + obj.ParentName + "\" name=\"" + obj.Name + "\" searchname=\"" + obj.searchName + "\" uniqueid=\"" + obj.UniqueId + "\" class=\"gouptype \"><div class=\"FilterStringContainerdiv\" style=\"\"><span onclick=\"" + onclick_event_name + "\" style=\"width:90%;margin-left:1%\" class=\"lft-popup-ele-label\" filetrtypeid=\"" + obj.FilterTypeId + "\" id=\"" + obj.Id + "\" type=\"Main-Stub\" name=\"" + obj.Name + "\">" + obj.Name + "</span><div class=\"ArrowContainerdiv\" style=\"display:none;background-color: rgb(88, 85, 77);\"><span onclick=\"" + onclick_event_name + "\" class=\"lft-popup-ele-next sidearrw \"></span></div></div></li>";

            $("#" + filterid + " .level" + (filter[0].Levels[level].Id) + " ul").append(html);
        }
    }
}
function OpenOrCloseCompetitorFrequencyRetailerPopup(obj) {
    ClosePopups();
    clearSelectedCompetitors();
    var ptpcus = $(obj).closest("li");
    //isCompetitorCustomBase = (CustomBaseFlag == 1)
    if (!isCompetitorCustomBase) {
        if (CompetitorFrequency.length > 0 && CompetitorFrequency[0].Id != $(ptpcus).attr("id")) {
            CompetitorFrequency = [];
            RemoveCompetitor();
            CompetitorFrequency.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
            ShowSelectedFilters();
        }
        if (CompetitorFrequency.length == 0) {
            CompetitorFrequency.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
        }
        if (CompetitorRetailer.length > 0) {
            $(".CompetitorFrequency-Retailers li[uniqueid='" + CompetitorRetailer[0].UniqueId + "']").addClass("Selected");
        }
    }
    else {
        if (CompetitorCustomBaseFrequency.length > 0 && CompetitorCustomBaseFrequency[0].Id != $(ptpcus).attr("id")) {
            CompetitorCustomBaseFrequency = [];
            RemoveCustomBaseCompetitor();
            CompetitorCustomBaseFrequency.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
            ShowSelectedFilters();
        }
        if (CompetitorCustomBaseFrequency.length == 0) {
            CompetitorCustomBaseFrequency.push({ Id: $(ptpcus).attr("id"), Name: $(ptpcus).attr("name"), DBName: $(ptpcus).attr("dbname"), ShopperDBName: $(ptpcus).attr("shopperdbname"), TripsDBName: $(ptpcus).attr("tripsdbname"), UniqueId: $(ptpcus).attr("uniqueid") });
        }
        if (CompetitorCustomBaseRetailer.length > 0) {
            $(".CompetitorFrequency-Retailers li[uniqueid='" + CompetitorCustomBaseRetailer[0].UniqueId + "']").addClass("Selected");
        }
    }
    $("#Translucent").show();

    $("#CompetitorFrequency-RetailerDivId .Lavel").hide();
    $("#CompetitorFrequency-RetailerDivId div[level-id='1']").show();

    $('.CompetitorFrequency-Retailers #retailerHeadingLevel2').hide();
    $('.CompetitorFrequency-Retailers #retailerHeadingLevel3').hide();
    $('.CompetitorFrequency-Retailers #CompetitorFrequency-RetailerDivId').css('width', 'auto')
    $('.CompetitorFrequency-Retailers .sidearrw_OnCLick').removeClass('sidearrw_OnCLick')
    $('.CompetitorFrequency-Retailers').show();
    updateSearch(obj)
}

function disableGeographiesWithTimeperiod(obj) {
    var year = 2021;
    var month = 0; // From Jan
    var geoList = ['FCL Ops OU', 'CCNA Regions', 'Census Divisions', 'Census Regions']
    var geoList1 = ['Circle K Las vegas', 'HEB Northwest Div', 'Walgreens Trade Areas',
                    '7-Eleven Baltimore/Washington DC', '7-Eleven Boston/Connecticut', '7-Eleven Chicago', '7-Eleven Colorado Springs/Pueblo',
                    '7-Eleven Dallas-Ft Worth', '7-Eleven Denver', '7-Eleven Detroit', '7-Eleven LA/San Diego', '7-Eleven Miami Dade',
                    '7-Eleven New York City', '7-Eleven Orlando/Tampa', '7-Eleven Philadelphia', '7-Eleven Phoenix', '7-Eleven San Francisco',
                    '7-Eleven Seattle/Portland']
    var tFlag = 0;

    if (!geoList.includes(obj.Name) && !geoList.includes(obj.ParentName) && !geoList1.includes(obj.Name) && !geoList1.includes(obj.ParentName)) {
        return false
    }
    if (geoList1.includes(obj.Name) || geoList1.includes(obj.ParentName)) {
        tFlag = 1;
    }
    if (tFlag == 1) {
        var tExtension = (ModuleBlock == "TREND") ? (TimeExtension.toString()) : (TimeExtension.toString());
        if (tExtension == '3mmt' || tExtension == 'quarter') { year = year; month = month + 2; }
        else if (tExtension == '6mmt') { year = year; month = month + 5; }
        else if (tExtension == '12mmt') { year = year; month = month + 11; }
        else if (tExtension == '18mmt') { year = year + 1; month = month + 5; }
        else if (tExtension == '24mmt') { year = year + 1; month = month + 11; }
        else if (tExtension == '30mmt') { year = year + 2; month = month + 5; }
        else if (tExtension == '36mmt') { year = year + 2; month = month + 11; }
        else if (tExtension == '48mmt') { year = year + 3; month = month + 11; }
    }
    var flag = false;
    if (ModuleBlock == "TREND") {
        for (var i = 0; i < TrendCustomBaselist.length; i++) {
            var date = TrendCustomBaselist[i].Name.toString();
            date = date.replace("Q1", "Mar").replace("Q2", "Jun").replace("Q3", "Sep").replace("Q4", "Dec")
            date = date.length == 4 ? "Dec " + date : date;
            date = new Date(date.toString())
            if (date.getFullYear() > year) {
                flag = true;
                break;
            }
            else if (date.getFullYear() == year && date.getMonth() >= month)//0-11 --> 6 means from july 
            {
                flag = true;
                break;
            }

        }
    }
    else {
        if (TimePeriodId == 1)
            flag = true;
        else {
            var date = TimePeriod.split('|')[1].toString();
            date = date.replace("Q1", "Mar").replace("Q2", "Jun").replace("Q3", "Sep").replace("Q4", "Dec")
            date = date.length == 4 ? "Dec " + date : date;
            date = new Date(date.toString())
            if (date.getFullYear() > year)
                flag = true;
            else if (date.getFullYear() == year && date.getMonth() >= month)//0-11 --> 6 means from july 
                flag = true;
        }
    }
    if (TimePeriodId == 1) {
        return true;
    }
    
    if (tFlag) {
        return !flag;
    }
    else {
        return flag;
    }
}