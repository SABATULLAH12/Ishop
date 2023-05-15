using iSHOP.BLL;
using iSHOPNew.CommonFilters;
using iSHOPNew.DAL;
using iSHOPNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace iSHOPNew.Controllers
{  
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Home(FormCollection form)
        {
            //ClearSession();
            if (form != null && form.Count != 0)
            {
                GetUserForm(form);
            }
            CommonFunctions.AuthenticateUser();
            SetDefaultSessionValues();
            //return RedirectToAction("PathToPurchase", "Dashboard");
            return View();
        }

        [HttpPost]
        [OutputCache(Duration = int.MaxValue)]
        public JsonResult GetFilters(string viewName, string TimePeriodType)
        {
            LoadLeftPanelFilters lf = new LoadLeftPanelFilters();
            LeftPanelFilters filters = null;
            try
            {
                filters = lf.LoadFilters(viewName, TimePeriodType);
            }
            catch (Exception ex)
            {
                filters = null;
                HttpContext.Response.Cache.SetNoStore();
                HttpContext.Response.Cache.SetNoServerCaching();
            }
            var jsonResult = Json(filters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }
        /// <summary>
        /// this method is for to load filters if output cache method results is null
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="TimePeriodType"></param>
        /// <returns></returns>
        public JsonResult GetViewFilters(string viewName, string TimePeriodType)
        {
            LoadLeftPanelFilters lf = new LoadLeftPanelFilters();
            LeftPanelFilters filters = null;
            try
            {
                filters = lf.LoadFilters(viewName, TimePeriodType);
            }
            catch (Exception ex)
            {
                filters = null;
                HttpContext.Response.Cache.SetNoStore();
                HttpContext.Response.Cache.SetNoServerCaching();
            }
            var jsonResult = Json(filters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }
        public string SanitizeURLString(string RawURLParameter)
        {
            string Results = RawURLParameter;
            Results = Results.Replace("%", "%25");
            Results = Results.Replace("<", "%3C");
            Results = Results.Replace(">", "%3E");
            Results = Results.Replace("#", "%23");
            Results = Results.Replace("{", "%7B");
            Results = Results.Replace("}", "%7D");
            Results = Results.Replace("|", "%7C");
            Results = Results.Replace(@"\", "%5C");
            Results = Results.Replace("^", "%5E");
            Results = Results.Replace("~", "%7E");
            Results = Results.Replace("[", "%5B");
            Results = Results.Replace("]", "%5D");
            Results = Results.Replace("`", "%60");
            Results = Results.Replace(";", "%3B");
            Results = Results.Replace("/", "%2F");
            Results = Results.Replace("?", "%3F");
            Results = Results.Replace(":", "%3A");
            Results = Results.Replace("@", "%40");
            Results = Results.Replace("=", "%3D");
            Results = Results.Replace("&", "%26");
            Results = Results.Replace("$", "%24");
            return Results;
        }

        public string GetUserQueryStringValues()
        {
            string querystring = string.Empty;         
            iSHOP.BLL.UserParams user = HttpContext.Session[SessionVariables.USERID] as iSHOP.BLL.UserParams;
            if (user != null)
            {
                querystring = "?" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("UserID")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.UserID)));
                querystring += "&" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("Name")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Name)));
                querystring += "&" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("UserName")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.UserName)));
                querystring += "&" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("Role")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Role)));
                querystring += "&" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("B3")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.B3)));
                querystring += "&" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("CBL")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.CBL)));
                querystring += "&" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("iSHOP")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.iSHOP)));
                querystring += "&" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata("BGM")) + "=" + SanitizeURLString(AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.BGM)));
            }           
            return querystring;
        }

        public void GetUserForm(FormCollection form)
        {
            Session["Form"] = form;
        }

        public JsonResult GetKIUserDetails()
        {
            UserManager usermanager = new UserManager();
            UserParams user = Session[SessionVariables.USERID] as UserParams;
            if (user == null)
                return Json("", JsonRequestBehavior.AllowGet);

            var userDetails = new
            {
                UserID = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.UserID)),
                UserIdAnalytics = Convert.ToString(user.Name),
                UserID_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("UserID")),
                Name = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Name)),
                Name_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("Name")),
                UserName = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.UserName)),
                UserName_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("UserName")),
                Role = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Role)),
                Role_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("Role")),
                B3 = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.B3)),
                B3_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("B3")),
                CBL = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.CBL)),
                CBL_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("CBL")),
                CBLV2 = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.CBLV2)),
                CBLV2_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("CBLV2")),
                iSHOP = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.iSHOP)),
                iSHOP_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("iSHOP")),
                Bev360Drinks = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Bev360Drinks)),
                Bev360Drinks_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("Bev360Drinks")),
                Bev360Drinkers = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Bev360Drinkers)),
                Bev360Drinkers_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("Bev360Drinkers")),
                CREST = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.CREST)),
                CREST_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("CREST")),
                DINE = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.DINE)),
                DINE_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("DINE")),
                BGM = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.BGM)),
                BGM_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("BGM")),
                Groups = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Groups)),
                Groups_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("Groups")),
                EmailId = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.EmailId)),
                EmailId_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("EmailId")),
                Login_Flag = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Login_Flag)),
                Login_Flag_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("Login_Flag"))
                //Password = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString(user.Password)),
                //Password_Str = AQ.Security.Cryptography.EncryptionHelper.Encryptdata(Convert.ToString("Password"))
            };

            //return userDetails;
            var jsonResult = Json(userDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        #region Clear User Session
        private void ClearSession()
        {
            //Session.Remove(SessionVariables.USERID);
            //HttpContext.Session.Remove(SessionVariables.FilterData);
            Session.Clear();
            //Session.Abandon();
            Session.RemoveAll();           
        }
        #endregion
        #region Clear User Session
        private void SetDefaultSessionValues()
        {           
            Session["StatSessionPosi"] = 1.96;
            Session["StatSessionNega"] = -1.96;
            Session["PercentStat"] = 95;
        }
        #endregion

        public static UserParams GetUserDetails()
        {
            UserParams user = System.Web.HttpContext.Current.Session["USERID"] as UserParams;
            return user;
        }

        #region Home
        public void KIHome()
        {
            HttpContext.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx" + GetUserQueryStringValues());
        }
        #endregion
        #region SignOut
        public void SignOut()
        {
            FormsAuthentication.SignOut();
            ClearSession();
            if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
            {
                HttpContext.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");

            }
            else
            {
                HttpContext.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
            }

        }
        #endregion
    }
}
