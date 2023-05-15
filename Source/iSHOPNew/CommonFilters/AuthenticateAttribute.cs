using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace iSHOPNew.CommonFilters
{
    public class AuthenticateUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session[SessionVariables.USERID] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Controller.ViewData.ModelState.AddModelError("Session-Expired", "User Session Expired");
                else
                {
                    return;
                    //CommonFunctions.AuthenticateUser();
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}