using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace PortProxy
{
    public class ScalarFunctions
    {
        /// <summary>
        /// Gets First Regex Match from string set.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetFirstRegexMatch(string pattern, string text)
        {
            if (isMatchRegex(text, pattern))
                return Regex.Matches(text, pattern, RegexOptions.IgnoreCase)[0].Value ?? "";
            else return "";
        }

        /// <summary>
        /// Check if a string character matches a regex pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool isMatchRegex(string text, string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return true;
            return Regex.IsMatch(text, pattern);
        }

        private static readonly Random random = new Random();
        /// <summary>
        /// Get a Random String Character w.r.t Length Provided
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GetRandomString(int Length = 10)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", Length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        /// <summary>
        /// Sees if Google is pingable or not 
        /// </summary>
        public static bool IsInternetConnected
        {
            get
            {
                string host = "google.com";
                bool result = false;
                Ping p = new Ping();
                try
                {
                    PingReply reply = p.Send(host, 5000);
                    if (reply.Status == IPStatus.Success)
                        return true;
                }
                catch { return false; }
                return result;
            }
        }

        /// <summary>
        /// Becomes Stubborn while deleting a file
        /// </summary>
        /// <param name="fullPath"></param>
        public static void DeleteFile(string fullPath)
        {
            try
            {
                File.Delete(fullPath);
            }
            catch { DeleteFile(fullPath); }
        }

        /// <summary>
        /// //Create a Folder in C Users Local Temp :-)<br/>
        /// Creates a Cache Folder in Current Directory<br/>
        /// It has \ at end of the path 
        /// </summary>
        public static string EnsureTempFolder
        {
            get
            {
                //if (!Directory.Exists(Path.GetTempPath() + "FinancialSuite"))
                //    Directory.CreateDirectory(Path.GetTempPath() + "FinancialSuite");
                //   return Path.GetTempFileName() + "FinancialSuite\\";
                //DirectorySecurity securityRules = new DirectorySecurity();
                //securityRules.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.Modify, AccessControlType.Allow));
                if (!Directory.Exists(Environment.CurrentDirectory + "\\" + "Cache\\"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + "Cache\\");
                //Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + "Cache\\", securityRules);
                return Environment.CurrentDirectory + "\\" + "Cache\\";
            }
        }




        public static void SetTimeout(Action AfterAction, int timeout)
        => new Thread(new ThreadStart(delegate { Thread.Sleep(timeout); AfterAction(); }));




        private static String[] units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static String[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };



        public static String ConvertAmountInWords(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return ConvertValue(amount_int) + " Only.";
                }
                else
                {
                    return ConvertValue(amount_int) + " Point " + ConvertValue(amount_dec) + " Only.";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception
            }
            return "";
        }
        public static String ConvertValue(Int64 i)
        {
            if (i < 20) { return units[i]; }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + ConvertValue(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred" + ((i % 100 > 0) ? " And " + ConvertValue(i % 100) : "");
            }
            if (i < 100000)
            {
                return ConvertValue(i / 1000) + " Thousand " + ((i % 1000 > 0) ? " " + ConvertValue(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return ConvertValue(i / 100000) + " Lakh " + ((i % 100000 > 0) ? " " + ConvertValue(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return ConvertValue(i / 10000000) + " Crore " + ((i % 10000000 > 0) ? " " + ConvertValue(i % 10000000) : "");
            }
            return ConvertValue(i / 1000000000) + " Arab "
            + ((i % 1000000000 > 0) ? " " + ConvertValue(i % 1000000000) : "");
        }

    }
}
