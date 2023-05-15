using iSHOP.BLL;
using iSHOPNew.Controllers;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iSHOPNew.Models
{
    public class ErrorLog 
    {
        public static void LogError(string error_message, string stack_trace)
        {
            error_message = Convert.ToString(error_message).Replace("'", "");
            stack_trace = Convert.ToString(stack_trace).Replace("'", "");
            if (HttpContext.Current.Session[SessionVariables.USERID] == null)
            {
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                    {
                        HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["SSOLogoutPageUrl"].ToString()));
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                    }
                }
              }

            UserParams userparam = HttpContext.Current.Session[SessionVariables.USERID] as UserParams;
            if (userparam != null)
            {
                string username = userparam.UserName;
                string name = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

                string comments = "";
                comments += HttpContext.Current.Request.Browser.Platform;
                comments += "Type=" + HttpContext.Current.Request.Browser.Type + ", ";
                comments += "Name=" + HttpContext.Current.Request.Browser.Browser + ", ";
                comments += "Version=" + HttpContext.Current.Request.Browser.Version + ", ";
                comments += "Major Version=" + HttpContext.Current.Request.Browser.MajorVersion + ", ";
                comments += "Minor Version=" + HttpContext.Current.Request.Browser.MinorVersion + ", ";
                comments += "Platform=" + HttpContext.Current.Request.Browser.Platform + ", ";
                comments += "Is Beta=" + HttpContext.Current.Request.Browser.Beta + ", ";
                comments += "Is Crawler=" + HttpContext.Current.Request.Browser.Crawler + ", ";
                comments += "Is AOL=" + HttpContext.Current.Request.Browser.AOL + ", ";
                comments += "Is Win16=" + HttpContext.Current.Request.Browser.Win16 + ", ";
                comments += "Is Win32=" + HttpContext.Current.Request.Browser.Win32 + ", ";
                comments += "Supports Frames=" + HttpContext.Current.Request.Browser.Frames + ", ";
                comments += "Supports Tables=" + HttpContext.Current.Request.Browser.Tables + ", ";
                comments += "Supports Cookies=" + HttpContext.Current.Request.Browser.Cookies + ", ";
                comments += "Supports VB Script=" + HttpContext.Current.Request.Browser.VBScript + ", ";
                comments += "Supports JavaScript=" + HttpContext.Current.Request.Browser.JavaScript + ", ";
                comments += "Supports Java Applets=" + HttpContext.Current.Request.Browser.JavaApplets + ", ";
                comments += "CDF=" + HttpContext.Current.Request.Browser.CDF;

                string strHostName = System.Net.Dns.GetHostName();
                string strAddress = "";
                try
                {
                    strAddress = GetIP4Address();

                    strHostName = (System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName);
                }
                catch
                {

                }

                Random rnd = new Random();
                string strSessionId = username + rnd.Next(0, 100000).ToString();
                strSessionId += "." + (rnd.NextDouble() * 1000).ToString("0.00000");
                string qry = "insert into tblUserErrorLog(error_message, stack_trace, session_id, username, login_time, active, machine_ip, hostname, comments, browser) values ('" +
                             error_message + "', '" +
                             stack_trace + "', " +
                            cf.getQ(strSessionId) + ", " +
                            cf.getQ(username) + ", GETDATE(), 1, " + cf.getQ(strAddress) + ", " + cf.getQ(strHostName) + ", " +
                            cf.getQ(comments) + ", " + cf.getQ(name) + ")";

                //string Connectionstring = ConfigurationManager.ConnectionStrings["Sql_Connectionstring_IdMapping"].ToString();
                string Connectionstring = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Sql_Connectionstring_IdMapping"]].ConnectionString;
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(Connectionstring);
                System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(qry, conn);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                sqlCmd.ExecuteNonQuery();
                conn.Close();
                sqlCmd.Dispose();
                conn.Dispose();
                //HttpContext.Current.Response.Flush();     
                var httpRequestBase = new HttpRequestWrapper(HttpContext.Current.Request);               
                if (!HttpContext.Current.Response.IsRequestBeingRedirected && !httpRequestBase.IsAjaxRequest())
                    HttpContext.Current.Response.Redirect("~/Error/Error");
                //ErrorController error = new ErrorController();
                //error.Error();
            }
        }

        public static string GetIP4Address()
        {
            string IP4Address = "";
            IP4Address = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (IP4Address == "" || IP4Address == null)
                IP4Address = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(IP4Address) && IP4Address.Trim().ToLower() == "::1")
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }
            }
            return IP4Address;
        }
    }
}