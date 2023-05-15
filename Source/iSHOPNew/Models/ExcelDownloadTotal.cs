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
using iSHOP.BLL;

namespace iSHOPNew.Models
{
    public class ExcelDownloadTotal
    {
        private Dictionary<string, int> sharedStrings = new Dictionary<string, int>();
        private Dictionary<string, string> selectedsheets = new Dictionary<string, string>();
        string UserExportFileName = string.Empty;
        List<string> SheetLiat = HttpContext.Current.Session["ExportSheetNames"] as List<string>;
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


        public void ExportToExcel(string query, List<string> SheetList, string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            //SheetList = SheetLiat;
            if (query.Equals("Export To Excel", StringComparison.OrdinalIgnoreCase))
            {
                if (HttpContext.Current.Session["exportfiles"] != null && HttpContext.Current.Session["sharedstrings"] != null)
                {
                    CopyFilesToDestination();
                    CreateSheets(SheetList);

                    writeSharedstring();
                    writeSheet();

                    string tempDir = HttpContext.Current.Server.MapPath(UserExportFileName + "");
                    string fileName = HttpContext.Current.Server.MapPath("~/Excel Export Total Files/ExcelTemplate.xlsx");
                    ZipDirectory(tempDir, fileName);

                    if (Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
                    {
                        Directory.Delete(HttpContext.Current.Server.MapPath(UserExportFileName), true);
                    }

                    FileStream fs1 = null;
                    fs1 = System.IO.File.Open(fileName, System.IO.FileMode.Open);

                    byte[] btFile = new byte[fs1.Length];
                    fs1.Read(btFile, 0, Convert.ToInt32(fs1.Length));
                    fs1.Close();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=iShop_Explorer_" + hdnyear + "" + FormateDateAndTime(Convert.ToString(hdnmonth)) + "" + FormateDateAndTime(Convert.ToString(hdndate)) + "_" + FormateDateAndTime(Convert.ToString(hdnhours)) + "" + FormateDateAndTime(Convert.ToString(hdnminutes)) + FormateDateAndTime(Convert.ToString(hdnseconds)) + ".xlsx");
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AddHeader("Content-Length", new FileInfo(fileName).Length.ToString());
                    HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
                    HttpContext.Current.Response.BinaryWrite(btFile);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
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
        public void CopyFilesToDestination()
        {
            string source = HttpContext.Current.Server.MapPath("~/Excel Export Total Files/Excel Template");
            UserExportFileName = "~/UserExportFiles/User" + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(UserExportFileName)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UserExportFileName));
            }

            string destination = HttpContext.Current.Server.MapPath(UserExportFileName);
            DirectoryCopy(source, destination, true);
        }
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
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
        public void CreateSheets(List<string> SheetList)
        {
            FileStream filnew;
            string tempdir = HttpContext.Current.Server.MapPath(UserExportFileName + "/xl/worksheets/");
            string sheetsource = HttpContext.Current.Server.MapPath("~/Excel Export Total Files/Excel Template/xl/worksheets/sheet1.xml");
            //           
            string sheetdrawingsource = HttpContext.Current.Server.MapPath("~/Excel Export Total Files/Excel Template/xl/worksheets/_rels/sheet1.xml.rels");
            selectedsheets.Clear();
            int sheet = 1;
            foreach (string _sheet in SheetList)
            {
                filnew = new FileStream(tempdir + "sheet" + sheet.ToString() + ".xml", FileMode.Create);
                filnew.Close();
                System.IO.File.Copy(sheetsource, tempdir + "sheet" + sheet.ToString() + ".xml", true);
                selectedsheets.Add("tab" + sheet.ToString(), _sheet);
                //
                filnew = new FileStream(tempdir + "_rels/sheet" + sheet.ToString() + ".xml.rels", FileMode.Create);
                filnew.Close();
                System.IO.File.Copy(sheetdrawingsource, tempdir + "_rels/sheet" + sheet.ToString() + ".xml.rels", true);
                sheet += 1;
            }

            string tempdirfile = HttpContext.Current.Server.MapPath(UserExportFileName + "/");
            string sourcefile = HttpContext.Current.Server.MapPath("~/Excel Export Total Files/Excel Template/");
            XmlDocument xmlChart = new XmlDocument();
            XmlNodeList snodelist;

            //write app HeadingPairs
            System.IO.File.Copy(sourcefile + "docProps/app.xml", tempdirfile + "docProps/app.xml", true);
            xmlChart.Load(tempdirfile + "docProps/app.xml");

            snodelist = xmlChart.GetElementsByTagName("HeadingPairs");
            string xmltest = "<vt:vector size = \"2\" baseType = \"variant\">" +
                             "<vt:variant>" +
                             "<vt:lpstr>Worksheets</vt:lpstr>" +
                              "</vt:variant>" +
                              "<vt:variant>" +
                              "<vt:i4>" + selectedsheets.Count + "</vt:i4>" +
                              "</vt:variant>" +
                              "</vt:vector>";

            foreach (XmlNode node in snodelist)
            {
                node.InnerXml = "";
                node.InnerXml = xmltest;
            }
            xmlChart.Save(tempdirfile + "docProps/app.xml");

            //write app TitlesOfParts
            xmltest = "<vt:vector size = \"" + selectedsheets.Count + "\" baseType = \"lpstr\">";
            foreach (string sheetname in selectedsheets.Values)
            {
                xmltest += "<vt:lpstr>" + sheetname + "</vt:lpstr>";
            }
            xmltest += "</vt:vector>";
            snodelist = xmlChart.GetElementsByTagName("TitlesOfParts");
            foreach (XmlNode node in snodelist)
            {
                node.InnerXml = "";
                node.InnerXml = xmltest;
            }
            xmlChart.Save(tempdirfile + "docProps/app.xml");

            //write Relationships

            xmlChart.Load(tempdirfile + "xl/_rels/workbook.xml.rels");
            snodelist = xmlChart.GetElementsByTagName("Relationships");

            xmltest = "";
            int scount = selectedsheets.Count + 2;

            xmltest += "<Relationship" +
           " Id = \"rId" + scount.ToString() + "\"" +
           " Type = \"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\"" +
            " Target = \"styles.xml\"/>";
            scount -= 1;

            xmltest += "<Relationship" +
         " Id = \"rId" + scount.ToString() + "\"" +
         " Type = \"http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme\"" +
          " Target = \"theme/theme1.xml\"/>";

            for (int i = 0; i < selectedsheets.Count; i++)
            {
                int snum = selectedsheets.Count - i;
                xmltest += "<Relationship" +
               " Id = \"rId" + snum.ToString() + "\"" +
               " Type = \"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\"" +
               " Target = \"worksheets/sheet" + snum.ToString() + ".xml\"/>";
            }

            xmltest += "<Relationship" +
            " Id = \"rId" + (selectedsheets.Count + 3).ToString() + "\"" +
            " Type = \"http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings\"" +
            " Target = \"sharedStrings.xml\"/>";

            foreach (XmlNode node in snodelist)
            {
                node.InnerXml = "";
                node.InnerXml = xmltest;
            }
            xmlChart.Save(tempdirfile + "xl/_rels/workbook.xml.rels");

            //write workbook
            xmlChart.Load(tempdirfile + "xl/workbook.xml");
            snodelist = xmlChart.GetElementsByTagName("sheets");

            xmltest = "";
            int sheetno = 0;
            foreach (string sheetname in selectedsheets.Values)
            {
                sheetno += 1;
                xmltest += "<sheet" +
                " name = \"" + sheetname + "\"" +
                " sheetId = \"" + sheetno.ToString() + "\"" +
                " r:id = \"rId" + sheetno.ToString() + "\"/>";
            }
            foreach (XmlNode node in snodelist)
            {
                node.InnerXml = "";
                node.InnerXml = xmltest;
            }
            xmlChart.Save(tempdirfile + "xl/workbook.xml");
        }

        public void writeSharedstring()
        {
            if (HttpContext.Current.Session["sharedstrings"] != null)
            {
                string sharedstring = HttpContext.Current.Server.MapPath("~/Excel Export Total Files/Excel Template/xl/sharedStrings.xml");
                string tempsharedstring = HttpContext.Current.Server.MapPath(UserExportFileName + "/xl/sharedStrings.xml");
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
                    count++;
                    if ((count == 4 || count == 5 || count == 7) && sString != "")
                    {
                        string[] sf = sString.Split(':');
                        xmltest += "<si><r><rPr><b /><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t>" + sf[0].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + ": </t></r>";
                        xmltest += "<r><rPr><sz val=\"11\" /><color theme=\"1\" /><rFont val=\"Calibri\" /><family val=\"2\" /><scheme val=\"minor\" /></rPr><t xml:space = \"preserve\">" + sf[1].Replace("&amp;", "&").Replace("&apos;", "'").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("'", "&apos;") + "</t></r></si>";
                    }
                    else if (count == 8)
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

        }

        public void writeSheet()
        {
            string sheetdata = string.Empty;
            if (HttpContext.Current.Session["exportfiles"] != null)
            {
                Dictionary<string, string> exportfileslist = HttpContext.Current.Session["exportfiles"] as Dictionary<string, string>;
                string sheet = HttpContext.Current.Server.MapPath("~/Excel Export Total Files/Excel Template/xl/worksheets/sheet1.xml");

                int sheetid = 0;
                foreach (string tabid in selectedsheets.Keys)
                {
                    if (exportfileslist.Keys.Contains(tabid))
                    {
                        sheetid += 1;
                        string tempsheet = HttpContext.Current.Server.MapPath(UserExportFileName + "/xl/worksheets/sheet" + sheetid.ToString() + ".xml");
                        System.IO.File.Copy(sheet, tempsheet, true);
                        XmlDocument xmlChart = new XmlDocument();
                        sheetdata = exportfileslist[tabid];
                        xmlChart.Load(tempsheet);
                        XmlNodeList snodelist = xmlChart.GetElementsByTagName("worksheet");
                        foreach (XmlNode node in snodelist)
                        {
                            node.InnerXml = "";
                            node.InnerXml = sheetdata;
                        }
                        xmlChart.Save(tempsheet);

                        if (sheetid == 1)
                        {
                            snodelist = xmlChart.GetElementsByTagName("sheetViews");
                            string sview = "<sheetView showGridLines = \"0\" tabSelected = \"1\" zoomScale = \"80\" zoomScaleNormal = \"80\" workbookViewId = \"0\"> " +
                                       "<pane ySplit =\"7\" topLeftCell =\"A8\" activePane =\"bottomLeft\" state =\"frozen\"/>" +
                                      "<selection pane = \"bottomLeft\" activeCell = \"A1\" sqref = \"A1\"/> " +
                                      "</sheetView> ";
                            foreach (XmlNode fsnode in snodelist)
                            {

                                fsnode.InnerXml = "";
                                fsnode.InnerXml = sview;
                            }
                            xmlChart.Save(tempsheet);
                        }
                        else
                        {
                            snodelist = xmlChart.GetElementsByTagName("sheetViews");
                            string sview = "<sheetView showGridLines = \"0\" zoomScale = \"80\" zoomScaleNormal = \"80\" workbookViewId = \"0\"> " +
                                           "<pane ySplit =\"7\" topLeftCell =\"A8\" activePane =\"bottomLeft\" state =\"frozen\"/>" +
                                           "<selection pane = \"bottomLeft\" activeCell = \"A1\" sqref = \"A1\"/> " +
                                          "</sheetView> ";
                            foreach (XmlNode fsnode in snodelist)
                            {

                                fsnode.InnerXml = "";
                                fsnode.InnerXml = sview;
                            }
                            xmlChart.Save(tempsheet);
                        }

                    }
                }
            }
        }
        public void ZipDirectory(string sourceDirectory, string zipFileName)
        {
            (new FastZip()).CreateZip(zipFileName, sourceDirectory, true, null);
            //ICSharpCode.SharpZipLib.GZip.GZipInputStream ff;
        }
    }
}