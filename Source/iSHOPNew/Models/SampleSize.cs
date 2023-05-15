using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace iSHOPNew.Models
{
    public class SampleSize

    {
        List<string> Retailerlist = null;
        CommonFunctions _commonfunctions = new CommonFunctions();
        //Check  Across Retailers Sample Size
        public List<iSHOPParams> CheckAccrossRetailerSampleSize(string retailertypeid, string _BenchMark, string[] Comparisonlist, string timePeriod, string _ShopperSegment, string _ShopperFrequency, string[] ShortNames, bool Tab_Id_mapping, DataTable tbl_SampleSize, TableParams tableParams)
        {
            List<iSHOPParams> iSHOPParamlist = null;
            iSHOPParams param = null;
            bool isSampleSizeNull = false;
            DataAccess dal = new DataAccess();
            try
            {

                var query1 = from r in Comparisonlist select string.Join("||", r.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
                Comparisonlist = query1.ToArray();
                object[] paramvalues = null;
                DataSet ds = null;
                if (Tab_Id_mapping)
                {
                    if (tbl_SampleSize != null && tbl_SampleSize.Rows.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        List<string> tblcolumns = tbl_SampleSize.Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                        List<string> comp = (from r in ShortNames select r.ToLower()).ToList();
                        tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();
                        foreach (object column in tblcolumns)
                        {
                            if (!Convert.ToString(column).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(column).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                param = new iSHOPParams();
                                param.Retailer = Convert.ToString(column);

                                if (!string.IsNullOrEmpty(Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)])) && Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)]) == GlobalVariables.NANumber)
                                {
                                    param.SampleSize = Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)]);
                                }
                               else if (!string.IsNullOrEmpty(Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)])) && Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)]) > 0)
                                {
                                    param.SampleSize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)])));                                   
                                }
                                else
                                    param.SampleSize = "0";

                                isSampleSizeNull = false;
                                iSHOPParamlist.Add(param);
                            }
                        }
                    }
                }
                else
                {
                    if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                    {
                        _BenchMark = tableParams.CustomBase_DBName;
                        Comparisonlist = (from r in tableParams.Comparison_DBNames where r != _BenchMark select r).ToArray();                      
                    }
                    else
                    {
                        Comparisonlist = (from r in Comparisonlist where r != _BenchMark select r).ToArray();                       
                    }
                    paramvalues = new object[] { _BenchMark, String.Join("|", Comparisonlist), timePeriod, _ShopperSegment, _ShopperFrequency };
                    ds = dal.GetData(paramvalues, retailertypeid);
                    Retailerlist = new List<string>();
                    Retailerlist = ShortNames.ToList();

                    if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        List<string> tblcolumns = (from r in ds.Tables[0].AsEnumerable() select Convert.ToString(r.Field<object>("Objective")).ToLower()).ToList();
                        List<string> comp = (from r in ShortNames select r.ToLower()).ToList();
                        tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();

                        foreach (string retailer in tblcolumns)
                        {
                            isSampleSizeNull = true;
                            param = new iSHOPParams();

                            string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                                 where Convert.ToString(r.Field<object>("Objective")).Equals(retailer, StringComparison.OrdinalIgnoreCase)
                                                 select Convert.ToString(r.Field<object>("SampleSize"))).FirstOrDefault();

                            if (!string.IsNullOrEmpty(samplesize) && Convert.ToDouble(samplesize) > 0)
                            {
                                param.SampleSize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(samplesize)));
                            }
                            else
                            {
                                param.SampleSize = "0";
                            }
                            iSHOPParamlist.Add(param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return iSHOPParamlist;
        }

        //Check  Within Retailers Sample Size
        public List<iSHOPParams> CheckWithinRetailerSampleSize(string retailertypeid, string _BenchMark, string[] Comparisonlist, string timePeriod, string _ShopperSegment, string _ShopperFrequency, string filter, string[] ShortNames, bool Tab_Id_mapping, DataTable tbl_SampleSize, TableParams tableParams)
        {
            List<iSHOPParams> iSHOPParamlist = null;
            iSHOPParams param = null;
            bool isSampleSizeNull = false;
            DataAccess dal = new DataAccess();
            try
            {
                _ShopperSegment = string.Join("||", _ShopperSegment.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
                object[] paramvalues = null;
                DataSet ds = null;
                if (Tab_Id_mapping)
                {
                    if (tbl_SampleSize != null && tbl_SampleSize.Rows.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        List<string> tblcolumns = tbl_SampleSize.Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                        List<string> comp = (from r in ShortNames select r.ToLower()).ToList();
                        tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();
                        foreach (object column in tblcolumns)
                        {
                            if (!Convert.ToString(column).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(column).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                param = new iSHOPParams();
                                param.Retailer = Convert.ToString(column);
                                if (!string.IsNullOrEmpty(Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)])) && Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)]) == GlobalVariables.NANumber)
                                {
                                    param.SampleSize = Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)])) && Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)]) > 0)
                                    param.SampleSize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)])));
                                else
                                    param.SampleSize = "0";

                                isSampleSizeNull = false;
                                iSHOPParamlist.Add(param);
                            }
                        }
                    }
                }
                else
                {
                    if (tableParams.StatTest.Equals("Custom Base", StringComparison.OrdinalIgnoreCase))
                    {
                        _BenchMark = tableParams.CustomBase_DBName;
                        Comparisonlist = (from r in tableParams.Comparison_DBNames where r != _BenchMark select r).ToArray();
                    }
                    else
                    {
                        Comparisonlist = (from r in Comparisonlist where r != _BenchMark select r).ToArray();
                    }
                    paramvalues = new object[] { _BenchMark.Replace("~", "`"), String.Join("|", Comparisonlist).Replace("~", "`"), timePeriod, _ShopperSegment, _ShopperFrequency, filter };
                     ds = dal.GetData(paramvalues, retailertypeid);

                    Retailerlist = new List<string>();
                    Retailerlist = ShortNames.ToList();

                    if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        List<string> tblcolumns = (from r in ds.Tables[0].AsEnumerable() select Convert.ToString(r.Field<object>("Objective")).ToLower()).ToList();
                        List<string> comp = (from r in ShortNames select r.ToLower()).ToList();
                        tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();

                        foreach (string retailer in tblcolumns)
                        {
                            isSampleSizeNull = true;
                            param = new iSHOPParams();

                            string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                                 where Convert.ToString(r.Field<object>("Objective")).Equals(retailer, StringComparison.OrdinalIgnoreCase)
                                                 select Convert.ToString(r.Field<object>("SampleSize"))).FirstOrDefault();
                            if (!string.IsNullOrEmpty(samplesize) && Convert.ToDouble(samplesize) == GlobalVariables.NANumber)
                            {
                                param.SampleSize = samplesize;
                            }
                            else if (!string.IsNullOrEmpty(samplesize) && Convert.ToDouble(samplesize) > 0)
                            {
                                param.SampleSize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(samplesize)));
                            }
                            else
                            {
                                param.SampleSize = "0";
                            }
                            iSHOPParamlist.Add(param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return iSHOPParamlist;
        }

        public List<iSHOPParams> CheckTimePeriodSampleSize(string retailertypeid, string _BenchMark, string[] Comparisonlist, string _ShopperSegment, string _ShopperFrequency, string filter, string[] ShortNames, bool Tab_Id_mapping, DataTable tbl_SampleSize)
        {
            List<iSHOPParams> iSHOPParamlist = null;
            iSHOPParams param = null;
            bool isSampleSizeNull = false;
            DataAccess dal = new DataAccess();
            try
            {
                _ShopperSegment = string.Join("||", _ShopperSegment.Split(new string[] { "||" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray());
                object[] paramvalues = null;
                DataSet ds = null;
                if (Tab_Id_mapping)
                {
                    if (tbl_SampleSize != null && tbl_SampleSize.Rows.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        List<string> tblcolumns = tbl_SampleSize.Columns.Cast<DataColumn>().Where(x => x.ColumnName != "Metric" && x.ColumnName != "Sortid" && x.ColumnName != "MetricItem").Select(x => Convert.ToString(x.ColumnName).ToLower()).Distinct().ToList();
                        List<string> comp = (from r in ShortNames select r.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 12MMT", "").Replace(" 6MMT", "").ToLower()).ToList();
                        tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();                       
                        foreach (object column in tblcolumns)
                        {
                            if (!Convert.ToString(column).Equals("Metric", StringComparison.OrdinalIgnoreCase)
                                && !Convert.ToString(column).Equals("MetricItem", StringComparison.OrdinalIgnoreCase))
                            {
                                param = new iSHOPParams();
                                param.Retailer = Convert.ToString(column);
                                if (!string.IsNullOrEmpty(Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)])) && Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)]) == GlobalVariables.NANumber)
                                {
                                    param.SampleSize = Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)]);
                                }
                                else if (!string.IsNullOrEmpty(Convert.ToString(tbl_SampleSize.Rows[0][Convert.ToString(column)])) && Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)]) > 0)
                                    param.SampleSize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(tbl_SampleSize.Rows[0][Convert.ToString(column)])));
                                else
                                    param.SampleSize = "0";

                                isSampleSizeNull = false;
                                iSHOPParamlist.Add(param);
                            }
                        }
                    }
                }
                else
                {
                    paramvalues = new object[] { _BenchMark, String.Join("|", Comparisonlist), _ShopperSegment.Replace("~", "`"), _ShopperFrequency, filter.Replace("~", "`") };
                    ds = dal.GetData(paramvalues, retailertypeid);

                    Retailerlist = new List<string>();
                    Retailerlist = ShortNames.ToList();                                    
                    if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        iSHOPParamlist = new List<iSHOPParams>();
                        List<string> tblcolumns = (from r in ds.Tables[0].AsEnumerable() select Convert.ToString(r.Field<object>("Objective")).ToLower()).ToList();
                        List<string> comp = (from r in ShortNames select r.Replace(" 48MMT", "").Replace(" 36MMT", "").Replace(" 30MMT", "").Replace(" 24MMT", "").Replace(" 18MMT", "").Replace(" 3MMT", "").Replace(" 12MMT", "").Replace(" 6MMT", "").ToLower()).ToList();
                        tblcolumns = tblcolumns.OrderBy(x => comp.IndexOf(x.Trim())).ToList();

                        foreach (string retailer in tblcolumns)
                        {
                            isSampleSizeNull = true;
                            param = new iSHOPParams();

                            string samplesize = (from r in ds.Tables[0].AsEnumerable()
                                                 where Convert.ToString(r.Field<object>("Objective")).Equals(retailer, StringComparison.OrdinalIgnoreCase)
                                                 select Convert.ToString(r.Field<object>("SampleSize"))).FirstOrDefault();
                            if (!string.IsNullOrEmpty(samplesize) && Convert.ToDouble(samplesize) == GlobalVariables.NANumber)
                            {
                                param.SampleSize = samplesize;
                            }
                           else if (!string.IsNullOrEmpty(samplesize) && Convert.ToDouble(samplesize) > 0)
                            {
                                param.SampleSize = Convert.ToString(String.Format("{0:#,###}", Convert.ToDouble(samplesize)));
                            }
                            else
                            {
                                param.SampleSize = "0";
                            }
                            iSHOPParamlist.Add(param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
            return iSHOPParamlist;
        }

    }
}