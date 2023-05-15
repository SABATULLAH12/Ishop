using iSHOPNew.BusinessLayer.Exports;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace iSHOPNew.Models
{
    public class EstablishmentDeepDiveData
    {
        DataAccess dal = null;
        DataSet ds = null;
        public List<EstablishmentDeepDiveMetrics> GetEstablishmentDeepDiveData(EstablishmentDeepDiveParams establishmentDeepDive)
        {
            List<EstablishmentDeepDiveMetrics> establishmentDeepDiveMetrics = null;
            dal = new DataAccess();
            object[] paramvalues = null;
            try
            {
                paramvalues = new object[] { establishmentDeepDive.Comparison_UniqueIds,establishmentDeepDive.TimePeriod_UniqueId,
                                            establishmentDeepDive.Filter_UniqueId,establishmentDeepDive.SelectedMetricsIds};

                ds = dal.GetData_WithIdMapping(paramvalues, "usp_EstablishmentDeepDive");
                if(ds != null && ds.Tables.Count > 0)
                {
                    establishmentDeepDiveMetrics = new List<EstablishmentDeepDiveMetrics>();
                    establishmentDeepDiveMetrics = (from r in ds.Tables[0].AsEnumerable()
                                                    select new EstablishmentDeepDiveMetrics()
                                                    {
                                                        Retailer = Convert.ToString(r["Retailer"]),
                                                        MetricType = Convert.ToString(r["MetricType"]),
                                                        Metric = Convert.ToString(r["Metric"]),
                                                        Volume = string.IsNullOrEmpty(Convert.ToString(r["Volume"])) ? 0 : Convert.ToDouble(r["Volume"]),
                                                        DisplayValue = string.IsNullOrEmpty(Convert.ToString(r["DisplayValue"])) ? 0 : Convert.ToDouble(r["DisplayValue"]),
                                                        Share = string.IsNullOrEmpty(Convert.ToString(r["DisplayValue"])) ? 0 : Convert.ToDouble(r["Share"]),
                                                        ChangePercentage = string.IsNullOrEmpty(Convert.ToString(r["DisplayValue"])) ? 0 : Convert.ToDouble(r["ChangePercentage"]),
                                                        SampleSize = string.IsNullOrEmpty(Convert.ToString(r["SampleSize"])) ? 0 : Convert.ToDouble(r["SampleSize"])
                                                    }).ToList();
                                        }

                              }
            catch (Exception ex)
            {
                establishmentDeepDiveMetrics = null;
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
            return establishmentDeepDiveMetrics;
        }

        public string prepareSlidesForPPTDownload(EstablishmentDeepDiveParams establishmentDeepDive)
        {
            dal = new DataAccess();
            object[] paramvalues = null;
            try
            {
                paramvalues = new object[] { establishmentDeepDive.Comparison_UniqueIds,establishmentDeepDive.TimePeriod_UniqueId,
                                            establishmentDeepDive.Filter_UniqueId,establishmentDeepDive.SelectedMetricsIds};
                ds = dal.GetData_WithIdMapping(paramvalues, "usp_EstablishmentDeepDive");
                EstablishmentDeepDiveExport establishmentDeepDiveExport = new EstablishmentDeepDiveExport();
                return establishmentDeepDiveExport.PrepareSlides(ds,establishmentDeepDive);

            }
            catch (Exception ex)
            {
                ErrorLog.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}