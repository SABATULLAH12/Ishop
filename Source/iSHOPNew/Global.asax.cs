using iSHOPNew.CommonFilters;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace iSHOPNew
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
                ErrorLog.LogError(ex.Message, ex.StackTrace);
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            string sessionId = Session.SessionID;
        }
    }
}