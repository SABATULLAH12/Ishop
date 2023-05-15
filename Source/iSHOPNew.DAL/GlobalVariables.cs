using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSHOPNew.DAL
{
    public static class GlobalVariables
    {
        public const string NA = "NA";

        public const string LowSampleSize = "(Low Sample Size)";
        public const string UseDirectionally = "(Use Directionally)";

        public const double MaxSampleSize = 100;
        public const double MinSampleSize = 30;

        public const double NANumber = -10000;  

        public static string GetRandomNumber
        {
            get
            {
                Random ra = new Random();
                return ra.Next().ToString();
            }
        }

        public static string GetRandNumber(string value)
        {
            return GetRandomNumber;
        }
        public static double LowSample
        {
            get
            {
                return 30;
            }
        }
        public static double MediumSample
        {
            get
            {
                return 100;
            }
        }
    }
}