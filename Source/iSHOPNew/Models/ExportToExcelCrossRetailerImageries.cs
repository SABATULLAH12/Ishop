using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using iSHOPNew.DAL;
using ICSharpCode.SharpZipLib.Zip;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace iSHOPNew.Models
{
    public class ExportToExcelCrossRetailerImageries
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        public void ExportToExcel(string query, string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {

            if (query.Equals("Export To Excel", StringComparison.OrdinalIgnoreCase))
            {
                CopyFilesToDestination();
                if (HttpContext.Current.Session["exportcrossTAB"] != null && HttpContext.Current.Session["sharedstrings"] != null)
                {


                    writeSharedstring();
                    writeSheet();

                    string tempDir = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Editable Export Files");
                    string fileName = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Export Files/ExcelTemplate.xlsx");
                    ZipDirectory(tempDir, fileName);

                    FileStream fs1 = null;
                    fs1 = System.IO.File.Open(fileName, System.IO.FileMode.Open);

                    byte[] btFile = new byte[fs1.Length];
                    fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                    fs1.Close();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=iShopCrossRetailerPerception-TAB_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds)) + ".xlsx");
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AddHeader("Content-Length", new FileInfo(fileName).Length.ToString());
                    HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
                    HttpContext.Current.Response.BinaryWrite(btFile);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
                else
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                    {
                        HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                    }
                }
            }
        }

        public static void CopyFilesToDestination()
        {
            string source = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Export Files/Excel Template");
            string destination = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Editable Export Files");
            DirectoryCopy(source, destination, true);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static string ReplaceToApostrophe(string retailer)
        {
            if (!string.IsNullOrEmpty(retailer) && (retailer.Contains("`") || retailer.Contains("&")))
            {
                retailer = retailer.Replace("&", "&amp;");
                return retailer = retailer.Replace("`", "'");
            }
            else
            {
                return retailer;
            }

        }

        public void writeSharedstring()
        {
            if (HttpContext.Current.Session["sharedstrings"] != null)
            {
                string sharedstring = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Export Files/Excel Template/xl/sharedStrings.xml");
                string tempsharedstring = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Editable Export Files/xl/sharedStrings.xml");
                System.IO.File.Copy(sharedstring, tempsharedstring, true);
                XmlDocument xmlChart = new XmlDocument();
                xmlChart.Load(tempsharedstring);
                XmlNodeList snodelist = xmlChart.GetElementsByTagName("sst");

                Dictionary<string, int> sharedStr = HttpContext.Current.Session["sharedstrings"] as Dictionary<string, int>;
                sharedStr = sharedStr.ToDictionary(pair => Convert.ToString(pair.Key.ToUpper()), pair => pair.Value); 
                string xmltest = "xmlns = \"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
                               "count = \" " + sharedStr.Count + "\" " +
                               "uniqueCount = \" " + sharedStr.Count + "\">";

                var iCount = 0;
                var sSVal = "";
                List<string> sSharedStrings = new List<string>();
                foreach (string sString in sharedStr.Keys)
                {
                    sSVal = "";

                    if (sString.ToLower() == "sample size" || sString.ToLower() == "samplesize" || sString.ToLower() == "store associations")
                        iCount = 1;

                    if (iCount == 0)
                        sSVal = sString.ToUpper();
                    else
                        sSVal = CommonFunctions.ToTitleCase(sString);

                    sSharedStrings.Add(sSVal);

                }

                int count = 0;
                foreach (string sString in sSharedStrings)
                {
                    //xmltest += "<si><t>" + sString + "</t></si>";
                    count++;
                    if (count == 4 && sString != "")
                    {
                        if (sString == "")
                        {
                            xmltest += "<si><t>" + sString.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></si>";
                        }
                        else
                        {
                            string[] ss = sString.Split(new String[] { ":", ", " },
                                             StringSplitOptions.RemoveEmptyEntries);

                            xmltest += "<si><r><rPr><b /> <u /><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t>" + ss[0].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ": </t></r>";
                            //Temp variables Do not use else where
                            int countss = ss.Length;
                            int i = 2;
                            int j = 1;
                            while (countss > 0)
                            {
                                while (j < ss.Length - 2)
                                {
                                    xmltest += "<r><rPr><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t xml:space = \"preserve\">" + ss[j].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ", </t></r>";
                                    j += 2;
                                    break;
                                }
                                while (i < ss.Length)
                                {
                                    xmltest += "<r><rPr><b /> <u /><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t>" + ss[i].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ": </t></r>";
                                    i += 2;
                                    break;
                                }

                                countss -= 2;
                            }
                            xmltest += "<r><rPr><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t xml:space = \"preserve\">" + ss[j].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></r>";
                            xmltest += "</si>";
                        }
                    }
                    else
                    {
                        xmltest += "<si><t>" + sString.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></si>";
                    }
                }

                foreach (XmlNode node in snodelist)
                {
                    node.InnerXml = "";
                    node.InnerXml = xmltest;
                }
                xmlChart.Save(tempsharedstring);
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");
                }
                else
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                }
            }
        }

        public void writeSheet()
        {
            string sheetdata = string.Empty;
            if (HttpContext.Current.Session["exportcrossTAB"] != null)
            {
                Dictionary<string, string> exportfileslist = HttpContext.Current.Session["exportfiles"] as Dictionary<string, string>;
                string sheet = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Export Files/Excel Template/xl/worksheets/sheet1.xml");

                string tempsheet = HttpContext.Current.Server.MapPath("~/CrossRetailerPerception Excel Editable Export Files/xl/worksheets/sheet1.xml");
                System.IO.File.Copy(sheet, tempsheet, true);
                XmlDocument xmlChart = new XmlDocument();
                xmlChart.Load(tempsheet);
                XmlNodeList snodelist = xmlChart.GetElementsByTagName("worksheet");
                sheetdata = Convert.ToString(HttpContext.Current.Session["exportcrossTAB"]);
                foreach (XmlNode node in snodelist)
                {
                    node.InnerXml = "";
                    node.InnerXml = sheetdata;
                }
                xmlChart.Save(tempsheet);

            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SSOUrl"].ToString() == "true")
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Views/Home.aspx?signout=true");
                }
                else
                {
                    HttpContext.Current.Response.Redirect(CommonFunctions.ReWriteHost(System.Configuration.ConfigurationManager.AppSettings["KIMainlink"]) + "Login.aspx?signout=true");
                }
            }
        }



        public string FormateDateAndTime(string month)
        {
            if (month.Length == 1)
            {
                return "0" + month;
            }
            else
                return month;

        }

        public static void ZipDirectory(string sourceDirectory, string zipFileName)
        {
            (new FastZip()).CreateZip(zipFileName, sourceDirectory, true, null);
            //ICSharpCode.SharpZipLib.GZip.GZipInputStream ff;
        }
    }
}