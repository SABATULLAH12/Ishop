using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iSHOPNew.Models
{
    public class cf
    {
        public static bool SiteMinderAuthentication = false; // make it TRUE when deployed in darden !!! IMPORTANT !!!
        public static bool AuthenticationRequired = true; // make it TRUE at the time of deployment !!! IMPORTANT !!!
        public static bool RedirectToMaintenanceSite = false; // make it FALSE at the time of deployment !!! IMPORTANT !!!

        private static Hashtable hstBrandColors = new Hashtable();
        private static Random rnd = new Random();
        public static string ssLowWarning = "Brands having sample sizes less than 30 are not displayed.";
        public const string SESSION_KEY_USERNAME = "USERNAME";
        public const string GSBRANDNAME_DONOTPURCHASE = "i don't purchase any of these brands at least once a month";
        public const string GSBRANDNAME_STORE = "store brand/private label";

        public static string cleanExcelXML(string data)
        {
            // ref: http://en.wikipedia.org/wiki/List_of_XML_and_HTML_character_entity_references
            string result = data.Replace("–", "-");
            int[] reservedChars = { 38, 34, 39, 60, 62, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 234, 235, 236, 237,
                                  238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 338, 339, 352, 353, 376, 402, 710, 732, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978, 982, 8194, 8195, 8201,
                                  8211, 8212, 8216, 8217, 8218, 8220, 8221, 8222, 8224, 8225, 8226, 8230, 8240, 8242, 8243, 8249, 8250, 8254, 8260, 8364, 8465, 8472, 8476, 8482, 8501, 8592, 8593, 8594, 8595, 8596, 8629, 8656, 8657, 8658, 8659, 8660, 8704, 8706, 8707, 8709, 8711, 8712, 8713, 8715, 8719, 8721, 8722, 8727, 8730, 8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756, 8764, 8773, 8776, 8800, 8801, 8804,
                                  8805, 8834, 8835, 8836, 8838, 8839, 8853, 8855, 8869, 8901, 8968, 8969, 8970, 8971, 9674, 9824, 9827, 9829, 9830 };
            // following characters are not converted due to conversion issues.
            //unicode: 160, 173, 8204, 8205, 8206, 8207, 9001, 9002
            //name: nbsb, shy, zwnj, zwj, lrm, rlm, lang, rang

            foreach (int i in reservedChars)
            {
                char ch = Convert.ToChar(i);
                string newch = "";
                if (result.IndexOf(ch) > -1)
                {
                    switch (i)
                    {
                        case 34:
                            {
                                newch = "&quot;";
                                break;
                            }
                        case 38:
                            {
                                //newch = "&amp;";
                                newch = "&";
                                break;
                            }
                        case 39:
                            {
                                //newch = "&apos;";
                                newch = "'";
                                break;
                            }
                        case 60:
                            {
                                //newch = "&lt;";
                                newch = "<";
                                break;
                            }
                        case 62:
                            {
                                //newch = "&gt;";
                                newch = ">";
                                break;
                            }
                        case 8217:
                            {
                                //newch = "&gt;";
                                newch = "’";
                                break;
                            }
                        default:
                            {
                                newch = "&#" + Convert.ToInt32(ch).ToString() + ";";
                                break;
                            }
                    }
                    result = result.Replace(ch.ToString(), newch);
                }
            }
            return result;
        }
        public static string cleanPPTXML(string data)
        {
            // ref: http://en.wikipedia.org/wiki/List_of_XML_and_HTML_character_entity_references
            string result = data.Replace("–", "-");
            int[] reservedChars = { 38, 34, 39, 60, 62, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 234, 235, 236, 237,
                                  238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 338, 339, 352, 353, 376, 402, 710, 732, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978, 982, 8194, 8195, 8201,
                                  8211, 8212, 8216, 8217, 8218, 8220, 8221, 8222, 8224, 8225, 8226, 8230, 8240, 8242, 8243, 8249, 8250, 8254, 8260, 8364, 8465, 8472, 8476, 8482, 8501, 8592, 8593, 8594, 8595, 8596, 8629, 8656, 8657, 8658, 8659, 8660, 8704, 8706, 8707, 8709, 8711, 8712, 8713, 8715, 8719, 8721, 8722, 8727, 8730, 8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756, 8764, 8773, 8776, 8800, 8801, 8804,
                                  8805, 8834, 8835, 8836, 8838, 8839, 8853, 8855, 8869, 8901, 8968, 8969, 8970, 8971, 9674, 9824, 9827, 9829, 9830 };
            // following characters are not converted due to conversion issues.
            //unicode: 160, 173, 8204, 8205, 8206, 8207, 9001, 9002
            //name: nbsb, shy, zwnj, zwj, lrm, rlm, lang, rang

            foreach (int i in reservedChars)
            {
                char ch = Convert.ToChar(i);
                string newch = "";
                if (result.IndexOf(ch) > -1)
                {
                    switch (i)
                    {
                        case 34:
                            {
                                newch = "&quot;";
                                break;
                            }
                        case 38:
                            {
                                newch = "&amp;";
                                //newch = "&";
                                break;
                            }
                        case 39:
                            {
                                newch = "&apos;";
                                //newch = "'";
                                break;
                            }
                        case 60:
                            {
                                newch = "&lt;";
                                //newch = "<";
                                break;
                            }
                        case 62:
                            {
                                newch = "&gt;";
                                //newch = ">";
                                break;
                            }
                        case 8217:
                            {
                                //newch = "&gt;";
                                newch = "’";
                                break;
                            }
                        default:
                            {
                                newch = "&#" + Convert.ToInt32(ch).ToString() + ";";
                                break;
                            }
                    }
                    result = result.Replace(ch.ToString(), newch);
                }
            }
            return result;
        }
        public static string getQ(string sourceStr)
        {
            return "'" + sourceStr.Replace("'", "''") + "'";
        }
        public static String GetSortedFilters(String filters)
        {
            if(!string.IsNullOrEmpty(filters))
            {
                var sFilterList = filters.Split('|');
                var sFilter = string.Empty;
                for (var i = 0; i < sFilterList.Length; i++)
                {
                    if (i % 2 == 0)
                        sFilter += sFilterList[i] + ":-";
                    else
                    {
                        sFilter += sFilterList[i] + (sFilterList.Length - 1 == i ? string.Empty : ", ");
                    }
                }
                filters = sFilter;
            }
            return filters;
        }
        public static String GetExcelSortedFilters(String filters)
        {
            if (!string.IsNullOrEmpty(filters))
            {
                string CustomFilter = string.Empty;
                string[] ss = filters.Split(new String[] { "|", "|" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ss.Length; i += 2)
                {
                    ss[i] = ss[i] + ": ";
                }

                for (int i = 1; i < ss.Length; i += 2)
                {
                    ss[i] = ss[i] + ", ";
                }
                foreach (string xmlfilter in ss)
                {
                    CustomFilter += xmlfilter;
                }
                filters = CustomFilter;
            }
            return filters;
        }
    }
}