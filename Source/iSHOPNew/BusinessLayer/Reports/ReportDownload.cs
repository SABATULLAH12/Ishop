using iSHOP.BLL;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace iSHOPNew.BusinessLayer.Reports
{
    public class ReportDownload
    {
        #region download report
        public void Download(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            try
            {
                string filename = "iSHOP_ReportGenerator_" + hdnyear + "" + Convert.ToString(hdnmonth).FormateDateTime() + "" + Convert.ToString(hdndate).FormateDateTime() + "_" + Convert.ToString(hdnhours).FormateDateTime() + "" + Convert.ToString(hdnminutes).FormateDateTime() + Convert.ToString(hdnseconds).FormateDateTime() + GlobalVariables.GetRandomNumber;
                FileStream fs1 = null;
                fs1 = System.IO.File.Open(HttpContext.Current.Session[SessionVariables.BeveragePPT] as string, System.IO.FileMode.Open);

                byte[] btFile = new byte[fs1.Length];
                fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                fs1.Close();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + filename + ".pptx");

                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                HttpContext.Current.Response.AddHeader("Content-Length", btFile.Length.ToString());
                HttpContext.Current.Response.BinaryWrite(btFile);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
            }
            catch (Exception ex)
            {
                UserParams userparam = HttpContext.Current.Session[SessionVariables.USERID] as UserParams;
                if (userparam == null)
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx");

                if (ex.HResult != -2146233040)
                    ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
        #endregion
    }
}