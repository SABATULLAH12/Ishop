using Aspose.Slides;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using iSHOPNew.DAL;

namespace iSHOPNew.Models
{
    public class SOAPBase
    {       
        public ProfilerChartParams profilerparams;
        public string UserExportFileName = string.Empty;
        public string Source = string.Empty;
        public string filename = string.Empty;
        public string sPowerPointTemplatePath = string.Empty;
        public static Dictionary<string, string> HeaderTabs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public static Random rnd = new Random();
        public object SignificanceValue, PositiveValue, NegativeValue;
        public string volume = string.Empty;
        public string significance = string.Empty;
        public List<string> lstHeaderText = new List<string>();
        string xmlpath = string.Empty;
        string rowheight = string.Empty;
        CommonFunctions _commonfunctions = new CommonFunctions();
        List<string> ChannelNets = new List<string>();
        public Presentation pres = new Presentation(); // creates a blank presentation with one blank slide.  must be done first
        public ISlideCollection slds = null;
        public ISlide sld = null;
        public DataSet ds = new DataSet();
        public List<string> objectivelist = null;
        public List<string> headerlist = null;

        public List<double> objectivelistTripSummary = null;
        public List<string> metriclist = null;

        public double accuratestatvalueposi;
        public double accuratestatvaluenega;
        public double StatTesting;

        public float chart_x_position = 0;
        public float chart_y_position = 0;
        public float chart_width = 0;
        public float chart_height = 0;
        public CommonFunctions commonfunctions = new CommonFunctions();
        public ReportGeneratorParams reportparams = null;

        public string Benchlist1 = string.Empty;
        public string benchMarkActualValue = string.Empty;
        public string[] complist, filt, Benchlist;
        public string complist1 = string.Empty;
        public double shopperBenchValue;
        public double tripsBenchValue;
        public string texboxvalue;
        public List<object> sampleSizelist;
        public List<string> beveragelist;
        public string samplesizeNames;
        public string complistNames;
        public string ChannelRetailersVisited = string.Empty;
        public string ShopperSegment = string.Empty;
        public string ComparisonPointsBanner = string.Empty;
        public int RowNumber = 0;
        public int ColumnNumber = 0;
        public List<string> SubHeaderlist = null;
        public float chart_legend_width
        {
            get
            {
                return 110;
            }
        }
        public float chart_legend_height
        {
            get
            {
                return 45;
            }
        }
        public int SlideNumber = 0;

        public Color backgroundcolor = new Color();
        public Color fontcolor = new Color();
        public string textboxvalue = string.Empty;        
        private float lagend_fontsize
        {
            get
            {
                return 12;
            }

        }
        public double chart_Min_Axis_Value = 0.0;
        public double chart_Max_Axis_Value = 0.0;

        public string chart_Top_MetriItem = "";
        public double chart_Top_MetriItemVolume = 0.0;

        Dictionary<string, string> bench_Comp_ShortNames = new Dictionary<string, string>();

        public float table_width
        {
            get
            {
                return 675;
            }
        }
        int seriesnumber = 0;
        public IEnumerable<double> dblCols = null;
        public IEnumerable<double> dblRows = null;
        FontData fontfamily = new FontData("Arial (Body)");     
        public SOAPBase()
        {
            
        }
        public void GeneratePPTReport(string hdnyear, string hdnmonth, string hdndate, string hdnhours, string hdnminutes, string hdnseconds)
        {
            
                StatTesting = Convert.ToDouble(HttpContext.Current.Session["PercentStat"]);
                accuratestatvalueposi = Convert.ToDouble(HttpContext.Current.Session["StatSessionPosi"]);
                accuratestatvaluenega = Convert.ToDouble(HttpContext.Current.Session["StatSessionNega"]);
            
            reportparams = HttpContext.Current.Session["SOAPData"] as ReportGeneratorParams;
            ds = reportparams.SOAPData;
            if (ds != null)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["selection"]).Equals("Priority", StringComparison.OrdinalIgnoreCase))
                {
                    SOAPPriorityRetailer sOAPPriorityRetailer = new SOAPPriorityRetailer();
                    sOAPPriorityRetailer.GenerateReport( hdnyear,  hdnmonth,  hdndate,  hdnhours,  hdnminutes,  hdnseconds);
                }
                else if (Convert.ToString(ds.Tables[0].Rows[0]["selection"]).Equals("Channels", StringComparison.OrdinalIgnoreCase))
                {
                    SOAPChannel sOAPChannel = new SOAPChannel();
                    sOAPChannel.GenerateReport(hdnyear, hdnmonth, hdndate, hdnhours, hdnminutes, hdnseconds);
                }
                else
                {
                    SOAPNonPriorityRetailer sOAPNonPriorityRetailer = new SOAPNonPriorityRetailer();
                    sOAPNonPriorityRetailer.GenerateReport(hdnyear, hdnmonth, hdndate, hdnhours, hdnminutes, hdnseconds);
                }
            }
        }

        public void InitializeAsposePresentationFile(string _fileName)
        {
            pres = new Presentation(_fileName);
            Aspose.Slides.License license = new Aspose.Slides.License();
            license.SetLicense(HttpContext.Current.Server.MapPath("~/Aspose.Slides.lic"));
            slds = pres.Slides;
        }
        public ITable Create_Trips_Table(ISlide slide, IEnumerable<double> _dblCols, IEnumerable<double> _dblRows, float x, float y)
        {
            dblCols = _dblCols;
            dblRows = _dblRows;
            //Add table shape to slide
            ITable tbl = slide.Shapes.AddTable(x, y, dblCols.ToArray(), dblRows.ToArray());
            return tbl;
        }

        public void ReplaceText(ISlide slide, string labelName, string text)
        {
            foreach (IShape shp in slide.Shapes)
            {
                if (shp.Name == labelName)
                {
                    IAutoShape ashp = (IAutoShape)shp;
                    ITextFrame txtFrame = ashp.TextFrame;
                    int i = 0;
                    foreach (IParagraph pa in txtFrame.Paragraphs)
                    {
                        if (i > 0)
                            pa.Portions.Clear();
                        i++;
                    }
                    IParagraph para = txtFrame.Paragraphs[0];
                    IPortion portion = para.Portions[0];
                    portion.Text = text.ToUpper();
                }
            }
        }
        public System.Drawing.Color GetSignificanceColor(string significancevalue)
        {
            System.Drawing.Color color = System.Drawing.Color.Black;
            if (!string.IsNullOrEmpty(significancevalue))
            {
                if (Convert.ToDouble(significancevalue) == 1000)
                    color = System.Drawing.Color.Blue;
                else if (Convert.ToDouble(significancevalue) > accuratestatvalueposi)
                    color = System.Drawing.Color.Green;
                else if (Convert.ToDouble(significancevalue) < accuratestatvaluenega)
                    color = System.Drawing.Color.Red;
                else if (Convert.ToDouble(significancevalue) <= accuratestatvalueposi && Convert.ToDouble(significancevalue) >= accuratestatvaluenega)
                    color = System.Drawing.Color.Black;
            }
            return color;
        }

        public string CheckLowSampleSize(string samplesize, string additionalinfo)
        {
            string sz = string.Empty;
            if (!string.IsNullOrEmpty(samplesize))
            {
                if (Convert.ToDouble(samplesize) > 30 && !string.IsNullOrEmpty(additionalinfo))
                    sz = additionalinfo;
                else if (Convert.ToDouble(samplesize) < GlobalVariables.MinSampleSize)
                {
                    sz = GlobalVariables.LowSampleSize;
                }
            }
            else if (string.IsNullOrEmpty(samplesize))
            {
                sz = GlobalVariables.LowSampleSize;
            }
            else if (!string.IsNullOrEmpty(additionalinfo) && additionalinfo.Contains("No Skew"))
            {
                sz = additionalinfo;
            }

            return sz;
        }
        //added by Nagaraju Date:21-09-2016
        public void RectangleTextInDiffFormat(Aspose.Slides.ITextFrame tf, bool InNewLine, List<string> TextFontSizeRGBFontBoldInPipeSeperated)
        {
            tf.Text = "";
            IParagraph para0 = tf.Paragraphs[0];
            IPortion port01 = new Portion();
            List<string> headers = new List<string>();
            int portionindx = 0;
            string value = string.Empty;
            string Signi = string.Empty;
            List<string> vallist = null;
            List<string> itemlist = null;
            float fontsize = 10;
            //get distinct header names
            if (TextFontSizeRGBFontBoldInPipeSeperated != null && TextFontSizeRGBFontBoldInPipeSeperated.Count > 1)
            {
                foreach (string item in TextFontSizeRGBFontBoldInPipeSeperated)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (!headers.Contains(item.Split(':')[0]))
                            headers.Add(item.Split(':')[0]);
                    }
                }

                //update value             
                foreach (string header in headers)
                {                   
                        itemlist = (from row in TextFontSizeRGBFontBoldInPipeSeperated
                                    where row.IndexOf(":") > -1 ? Convert.ToString(row.Split(':')[0]).Equals(header, StringComparison.OrdinalIgnoreCase)
                                    : Convert.ToString(row).Equals(header, StringComparison.OrdinalIgnoreCase)
                                    select row.IndexOf(":") > -1 ? row.Split(':')[1] : row).ToList();
                   
                    for (int i = 0; i < itemlist.Count; i++)
                    {
                        vallist = itemlist[i].Split('|').ToList();
                        if (vallist != null && vallist.Count > 1)
                            value = vallist[1];

                        if (vallist != null && vallist.Count > 2)
                            Signi = vallist[2];

                        if (i == 0)
                        {
                            port01 = new Portion();
                            para0.Portions.Add(port01);
                            if (itemlist[i].IndexOf("No Skew") > -1)
                            {
                                if (itemlist.Count - 1 > i)
                                {
                                    if (header.IndexOf("|") > -1)
                                        tf.Paragraphs[0].Portions[portionindx].Text = itemlist[i].Split('|')[0] + ": No Skew, ";
                                    else
                                        tf.Paragraphs[0].Portions[portionindx].Text = header + ": No Skew, ";

                                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                }
                                else
                                {
                                    if (header.IndexOf("|") > -1)
                                    {
                                        if (headers[headers.Count - 1] != header)
                                            tf.Paragraphs[0].Portions[portionindx].Text = itemlist[i].Split('|')[0] + ": No Skew\n";
                                        else
                                            tf.Paragraphs[0].Portions[portionindx].Text = itemlist[i].Split('|')[0] + ": No Skew";

                                        tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                        tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                    }
                                    else
                                    {
                                        if (headers[headers.Count - 1] != header)
                                            tf.Paragraphs[0].Portions[portionindx].Text = header + ": No Skew\n";
                                        else
                                            tf.Paragraphs[0].Portions[portionindx].Text = header + ": No Skew";

                                        tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                        tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                    }
                                }
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                portionindx++;
                            }
                            else
                            {
                                if(header.IndexOf("|") > -1)
                                    tf.Paragraphs[0].Portions[portionindx].Text = itemlist[i].Split('|')[0] + " (";
                                else
                                tf.Paragraphs[0].Portions[portionindx].Text = header + ": " + itemlist[i].Split('|')[0] + " (";

                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                portionindx++;

                                port01 = new Portion();
                                para0.Portions.Add(port01);
                                tf.Paragraphs[0].Portions[portionindx].Text = value + "%";
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(Convert.ToString(Signi));
                                portionindx++;

                                port01 = new Portion();
                                para0.Portions.Add(port01);
                                if (itemlist.Count - 1 > i)
                                    tf.Paragraphs[0].Portions[portionindx].Text = "), ";
                                else
                                {
                                    if (headers[headers.Count - 1] != header)
                                    {
                                        tf.Paragraphs[0].Portions[portionindx].Text = ")\n";
                                    }
                                    else
                                    {
                                        tf.Paragraphs[0].Portions[portionindx].Text = ")";
                                    }
                                }
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                portionindx++;
                            }

                        }
                        else
                        {
                            port01 = new Portion();
                            para0.Portions.Add(port01);
                            if (itemlist[i].IndexOf("No Skew") > -1)
                            {
                                if (itemlist.Count - 1 > i)
                                {
                                    if (header.IndexOf("|") > -1)
                                    {
                                        tf.Paragraphs[0].Portions[portionindx].Text = "No Skew, ";
                                    }
                                }
                                else
                                {
                                    if (headers[headers.Count - 1] != header)
                                    {
                                        tf.Paragraphs[0].Portions[portionindx].Text = "No Skew\n";
                                    }
                                    else
                                    {
                                        tf.Paragraphs[0].Portions[portionindx].Text = "No Skew";
                                    }
                                }

                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                portionindx++;
                            }
                            else
                            {
                                tf.Paragraphs[0].Portions[portionindx].Text = itemlist[i].Split('|')[0] + " (";
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                portionindx++;

                                port01 = new Portion();
                                para0.Portions.Add(port01);
                                tf.Paragraphs[0].Portions[portionindx].Text = value + "%";
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(Convert.ToString(Signi));
                                portionindx++;

                                port01 = new Portion();
                                para0.Portions.Add(port01);
                                if (itemlist.Count - 1 > i)
                                    tf.Paragraphs[0].Portions[portionindx].Text = "), ";
                                else
                                {
                                    if (headers[headers.Count - 1] != header)
                                        tf.Paragraphs[0].Portions[portionindx].Text = ")\n";
                                    else
                                        tf.Paragraphs[0].Portions[portionindx].Text = ")";
                                }

                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                                tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                                portionindx++;
                            }

                        }
                    }
                }

            }
            else if (TextFontSizeRGBFontBoldInPipeSeperated.Count == 1)
            {
                vallist = TextFontSizeRGBFontBoldInPipeSeperated[0].Split('|').ToList();
                if (vallist != null && vallist.Count > 1)
                    value = vallist[1];

                if (vallist != null && vallist.Count > 2)
                    Signi = vallist[2];

                if (TextFontSizeRGBFontBoldInPipeSeperated[0].IndexOf("No Skew") > -1)
                {
                    tf.Paragraphs[0].Portions[portionindx].Text = "No Skew";

                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                    portionindx++;
                }
                else
                {
                    tf.Paragraphs[0].Portions[portionindx].Text = TextFontSizeRGBFontBoldInPipeSeperated[0].Split('|')[0] + " (";
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                    portionindx++;

                    port01 = new Portion();
                    para0.Portions.Add(port01);
                    tf.Paragraphs[0].Portions[portionindx].Text = value + "%";
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = GetSignificanceColor(Convert.ToString(Signi));
                    portionindx++;

                    port01 = new Portion();
                    para0.Portions.Add(port01);
                    tf.Paragraphs[0].Portions[portionindx].Text = ")";

                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FontHeight = fontsize;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.FillType = FillType.Solid;
                    tf.Paragraphs[0].Portions[portionindx].PortionFormat.FillFormat.SolidFillColor.Color = Color.FromArgb(0, 0, 0);
                    portionindx++;
                }

            }
        }
    }
}