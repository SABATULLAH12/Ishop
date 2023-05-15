using iSHOPNew.com.aqinsights.coke;
using iSHOPNew.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSHOPNew.Models
{
    public class LogEntries
    {
        public static bool LogSelection(string Module)
        {
            bool status = false;
            iSHOP.BLL.UserParams userparams = HttpContext.Current.Session[SessionVariables.USERID] as iSHOP.BLL.UserParams;
            try
            {
                if (userparams != null)
                {
                    com.aqinsights.coke.User_Management usermanagement = new com.aqinsights.coke.User_Management();
                    com.aqinsights.coke.UserParams uparams = new com.aqinsights.coke.UserParams();
                    iSHOP.BLL.UserParams currentuser = HttpContext.Current.Session[SessionVariables.USERID] as iSHOP.BLL.UserParams;
                    if (currentuser != null)
                    {
                        uparams.UserID = currentuser.UserID;
                        uparams.UserName = currentuser.UserName;
                        uparams.Tool = "iSHOP";
                        uparams.Module = Module;
                        usermanagement.LogUserData(uparams);
                        status = true;
                    }                  
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
    }
}