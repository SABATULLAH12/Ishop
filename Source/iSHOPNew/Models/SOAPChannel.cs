﻿using Aspose.Slides;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using iSHOPNew.DAL;

namespace iSHOPNew.Models
{
    public class SOAPChannel : SOAPBase
    {
        public void GenerateReport(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            try
            {
                reportparams = HttpContext.Current.Session["SOAPData"] as ReportGeneratorParams;
                ds = reportparams.SOAPData;
                objectivelist = (from row in ds.Tables[1].AsEnumerable() select Convert.ToString(row.Field<object>("GroupItem")).ToUpper()).Distinct().ToList();

                InitializeAsposePresentationFile(HttpContext.Current.Server.MapPath(@"~\SOAP PPT Templates\Channels\SOAP Template_Channel slides" + (objectivelist.Count) + ".pptx"));
                headerlist = new List<string>() { "KEY STATS", "MOTIVATORS", "PROFILE", "OTHER" };
                UpdateTable(slds[0], "Table 15");
                headerlist = new List<string>() { "TOP BEVERAGE CATEGORIES", "TOP NON-BEVERAGE CATEGORIES", "IC MAIN REASON", "AVERAGE BASKET SIZE" };
                UpdateTable(slds[1], "Table 56");

                filename = "iSHOP_ReportGenerator_" + hdnyear + "" + Convert.ToString(hdnmonth).FormateDateTime() + "" + Convert.ToString(hdndate).FormateDateTime() + "_" + Convert.ToString(hdnhours).FormateDateTime() + "" + Convert.ToString(hdnminutes).FormateDateTime() + Convert.ToString(hdnseconds).FormateDateTime();
                pres.Save(HttpContext.Current.Server.MapPath("~/ProfilerPPTFiles/Downloads/" + filename + ".pptx"), Aspose.Slides.Export.SaveFormat.Pptx);

                FileStream fs1 = null;
                fs1 = System.IO.File.Open(HttpContext.Current.Server.MapPath("~/ProfilerPPTFiles/Downloads/" + filename + ".pptx"), System.IO.FileMode.Open);

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
                ErrorLog.LogError(ex.Message, ex.StackTrace);
            }
        }
        private void UpdateTable(ISlide slide, string TableName)
        {
            ITable datatbl = null;
            DataTable tble = null;
            foreach (IShape shp in slide.Shapes)
            {
                if (shp is ITable)
                {
                    if (shp.Name == TableName)
                    {
                        datatbl = (ITable)shp;
                        //update Header
                        RowNumber = 1;
                        ColumnNumber = 2;
                        datatbl[1, RowNumber].TextFrame.Text = Convert.ToString(reportparams.GeographyShortName + " - " + reportparams.ShopperSegmentShortName).ToUpper();
                        foreach (string _group in objectivelist)
                        {
                            datatbl[ColumnNumber, RowNumber].TextFrame.Text = Convert.ToString(_group).ToUpper();
                            ColumnNumber++;
                        }
                        RowNumber = 2;
                        foreach (string header in headerlist)
                        {
                            ColumnNumber = 0;
                            var query = (from row in ds.Tables[1].AsEnumerable()
                                         where Convert.ToString(row.Field<object>("Header")).Equals(header, StringComparison.OrdinalIgnoreCase)
                                         select row).ToList();
                            tble = query.CopyToDataTable();
                            List<string> SubHeaderlist = (from row in tble.AsEnumerable()
                                                          where Convert.ToString(row.Field<object>("Header")).Equals(header, StringComparison.OrdinalIgnoreCase)
                                                          select Convert.ToString(row.Field<object>("SubHeader")).ToUpper()).Distinct().ToList();
                            foreach (string subheader in SubHeaderlist)
                            {
                                ColumnNumber = 1;
                                if (header.Equals("OTHER", StringComparison.OrdinalIgnoreCase))
                                    datatbl[ColumnNumber, RowNumber].TextFrame.Text = string.Empty;
                                else
                                    datatbl[ColumnNumber, RowNumber].TextFrame.Text = Convert.ToString(subheader).ToUpper();
                                foreach (string _group in objectivelist)
                                {
                                    ColumnNumber++;
                                    if (header.Equals("OTHER", StringComparison.OrdinalIgnoreCase))
                                        datatbl[ColumnNumber, RowNumber].TextFrame.Text = string.Empty;
                                    else
                                    {
                                        DataRow rowdata = (from row in tble.AsEnumerable()
                                                           where Convert.ToString(row.Field<object>("Header")).Equals(header, StringComparison.OrdinalIgnoreCase)
                                                           && Convert.ToString(row.Field<object>("GroupItem")).Equals(_group, StringComparison.OrdinalIgnoreCase)
                                                            && Convert.ToString(row.Field<object>("SubHeader")).Equals(Convert.ToString(subheader), StringComparison.OrdinalIgnoreCase)
                                                           select row).FirstOrDefault();
                                        List<string> additionalinfo = new List<string>();
                                        var query2 = (from row in tble.AsEnumerable()
                                                      where Convert.ToString(row.Field<object>("Header")).Equals(header, StringComparison.OrdinalIgnoreCase)
                                                      && Convert.ToString(row.Field<object>("GroupItem")).Equals(_group, StringComparison.OrdinalIgnoreCase)
                                                       && Convert.ToString(row.Field<object>("SubHeader")).Equals(Convert.ToString(subheader), StringComparison.OrdinalIgnoreCase)
                                                       && !string.IsNullOrEmpty(Convert.ToString(row.Field<object>("additionalinfo")))
                                                      select Convert.ToString(row.Field<object>("additionalinfo"))).ToList();
                                        if (query2 != null && query2.Count > 0)
                                        {
                                            if (Convert.ToString(rowdata["Header"]) == "PROFILE")
                                            {
                                                additionalinfo = (from row in tble.AsEnumerable()
                                                                  where Convert.ToString(row.Field<object>("Header")).Equals(header, StringComparison.OrdinalIgnoreCase)
                                                                  && Convert.ToString(row.Field<object>("GroupItem")).Equals(_group, StringComparison.OrdinalIgnoreCase)
                                                                   && Convert.ToString(row.Field<object>("SubHeader")).Equals(Convert.ToString(subheader), StringComparison.OrdinalIgnoreCase)
                                                                    && !string.IsNullOrEmpty(Convert.ToString(row.Field<object>("additionalinfo")))
                                                                  select Convert.ToString(row.Field<object>("additionalinfo")).Replace("African American", "AA").Replace("Hispanic", "Hisp").Replace("5 or More", "5+").Replace("FactAgeGroups", "Age Groups").Replace("HHTotal", "HH Size").Replace("shopper segment", "Shopper Segment").ToUpper() + " |" + Math.Round(Convert.ToDouble((row.Field<object>("Volume"))), 0).ToString() + " |" + (row.Field<object>("Significance"))).ToList();
                                            }
                                            else
                                            {
                                                additionalinfo = (from row in tble.AsEnumerable()
                                                                  where Convert.ToString(row.Field<object>("Header")).Equals(header, StringComparison.OrdinalIgnoreCase)
                                                                  && Convert.ToString(row.Field<object>("GroupItem")).Equals(_group, StringComparison.OrdinalIgnoreCase)
                                                                   && Convert.ToString(row.Field<object>("SubHeader")).Equals(Convert.ToString(subheader), StringComparison.OrdinalIgnoreCase)
                                                                    && !string.IsNullOrEmpty(Convert.ToString(row.Field<object>("additionalinfo")))
                                                                  select Convert.ToString(row.Field<object>("additionalinfo")).Replace("African American", "AA").Replace("Hispanic", "Hisp").Replace("5 or More", "5+").Replace("FactAgeGroups", "Age Groups").Replace("HHTotal", "HH Size").Replace("shopper segment", "Shopper Segment").ToUpper() + " |" + Math.Round(Convert.ToDouble((row.Field<object>("Volume"))), 0).ToString() + " |" + (row.Field<object>("Significance"))).ToList();
                                            }
                                        }

                                        string SampleSize = base.CheckLowSampleSize(Convert.ToString(rowdata["SampleSize"]), String.Join(", ", additionalinfo).ToUpper());
                                        if (!string.IsNullOrEmpty(Convert.ToString(rowdata["SampleSize"])) && Convert.ToDouble(rowdata["SampleSize"]) > 30 && additionalinfo.Count == 1 && additionalinfo[0].IndexOf("No Skew") > -1)
                                        {
                                            datatbl[ColumnNumber, RowNumber].TextFrame.Text = "NO SKEW";
                                            datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                                            datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                        }
                                        else if (!string.IsNullOrEmpty(SampleSize))
                                        {
                                            if (SampleSize.Equals(GlobalVariables.LowSampleSize, StringComparison.OrdinalIgnoreCase))
                                            {
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Text = SampleSize.ToUpper();
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                            }
                                            else
                                                base.RectangleTextInDiffFormat(datatbl[ColumnNumber, RowNumber].TextFrame, false, additionalinfo);
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(Convert.ToString(rowdata["additionalinfo"])) && !string.IsNullOrEmpty(Convert.ToString(rowdata["Volume"])))
                                            {
                                                //datatbl[ColumnNumber, RowNumber].TextFrame.Text = String.Join(", ", additionalinfo) + " (" + Math.Round(Convert.ToDouble((rowdata["Volume"])), 0).ToString() + "%" + ")";
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Text = "";
                                                IParagraph para0 = datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0];
                                                IPortion port01 = new Portion();
                                                IPortion port02 = new Portion();
                                                IPortion port03 = new Portion();

                                                para0.Portions.Add(port01);
                                                para0.Portions.Add(port02);
                                                para0.Portions.Add(port03);

                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].Text = String.Join(", ", additionalinfo).ToUpper() + " (";
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.FillType = FillType.Solid;
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontHeight = 10;
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FontBold = NullableBool.False;

                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[1].Text = Math.Round(Convert.ToDouble((rowdata["Volume"])), 0).ToString() + "%";
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[1].PortionFormat.FillFormat.FillType = FillType.Solid;
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[1].PortionFormat.FillFormat.SolidFillColor.Color = base.GetSignificanceColor(Convert.ToString(rowdata["Significance"]));
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[1].PortionFormat.FontHeight = 10;
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[1].PortionFormat.FontBold = NullableBool.False;

                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[2].Text = ")";
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[2].PortionFormat.FillFormat.FillType = FillType.Solid;
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[2].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0,0,0);
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[2].PortionFormat.FontHeight = 10;
                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[2].PortionFormat.FontBold = NullableBool.False;
                                                //datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = base.GetSignificanceColor(Convert.ToString(rowdata["Significance"]));

                                            }
                                            else
                                            {
                                                if (subheader.Equals("Dollar Spent", StringComparison.OrdinalIgnoreCase))
                                                    datatbl[ColumnNumber, RowNumber].TextFrame.Text = "$" + Math.Round(Convert.ToDouble((rowdata["Volume"])), 1).ToString().ToUpper();
                                                else if (subheader.Equals("No of Items", StringComparison.OrdinalIgnoreCase))
                                                    datatbl[ColumnNumber, RowNumber].TextFrame.Text = Math.Round(Convert.ToDouble((rowdata["Volume"])), 1).ToString().ToUpper();
                                                else
                                                    datatbl[ColumnNumber, RowNumber].TextFrame.Text = Math.Round(Convert.ToDouble((rowdata["Volume"])), 0).ToString().ToUpper() + "%";

                                                datatbl[ColumnNumber, RowNumber].TextFrame.Paragraphs[0].Portions[0].PortionFormat.FillFormat.SolidFillColor.Color = base.GetSignificanceColor(Convert.ToString(rowdata["Significance"]));

                                            }
                                        }
                                    }
                                }
                                RowNumber++;
                            }
                            RowNumber++;
                        }
                    }
                }
            }
            UpdateInputSelection(slide);
        }
        public void UpdateInputSelection(ISlide slide)
        {
            //1st slide
            ReplaceText(slide, "Title 1", "Shopper On A Page:" + reportparams.GeographyShortName + " - " + reportparams.ShopperSegmentShortName);
            //ReplaceText(slide, "TextBox 49", "Source: Coca-Cola iSHOP Tracking Study " + reportparams.ShortTimePeriod + ", " + reportparams.ShopperFrequencyShortName + " " + reportparams.ShopperSegmentShortName + " Shoppers, " + reportparams.ShopperSegmentShortName + " Visits, Filters: " + Convert.ToString(reportparams.FilterShortNames).Replace("|", ", "));
            ReplaceText(slide, "TextBox 53", "[1] Stat Test vs Share of Shopper of " + reportparams.ShopperFrequencyShortName + " " + reportparams.GeographyShortName + "-" + reportparams.ShopperSegmentShortName + ", [2] Stat Test vs Total Trips of " + reportparams.GeographyShortName + "-" + reportparams.ShopperSegmentShortName + ", [3] Stat Test vs Total Shopper of " + reportparams.ShopperFrequencyShortName + " " + reportparams.GeographyShortName + "-" + reportparams.ShopperSegmentShortName + "");

            //2nd slide          
            //ReplaceText(slide, "TextBox 13", "Source: Coca-Cola iSHOP Tracking Study " + reportparams.ShortTimePeriod + ", " + reportparams.ShopperFrequencyShortName + " " + reportparams.ShopperSegmentShortName + " Shoppers, " + reportparams.ShopperSegmentShortName + " Visits, Filters: " + Convert.ToString(reportparams.FilterShortNames).Replace("|", ", "));
            ReplaceText(slide, "TextBox 16", "Stat Test vs Total Trips of " + reportparams.GeographyShortName + " - " + reportparams.ShopperSegmentShortName);

            texboxvalue = "Source: CCNA iSHOP Tracker, Time Period: " + Convert.ToString(reportparams.ShortTimePeriod) + "; Base: " + reportparams.ShopperFrequencyShortName + " " + reportparams.ShopperSegmentShortName + " Shoppers, " + reportparams.ShopperSegmentShortName + " Visits\n" +
            "Filters: " + (String.IsNullOrEmpty(reportparams.FilterShortNames) ? "NONE" : Convert.ToString(reportparams.FilterShortNames).Replace("|", ", "));
         
            ((IAutoShape)pres.LayoutSlides[1].Shapes.Where(x => x.Name == "TPandFilters").FirstOrDefault()).TextFrame.Text = texboxvalue;          
        }        
    }
}