using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace PortProxy
{
    public static class BaseExtensions
    {


        public static Object? GetPropValue(this Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        public static Int32 ToInt32(this Object obj)
        {
            if (obj == null) return 0;
            else
            {
                int val = 0;
                if (int.TryParse(obj.ToString(), out val))
                {
                    return val;
                }
                else { return 0; }
            }
            //else return Convert.ToInt32(obj);
        }

        public static Int64 ToInt64(this Object obj)
        {
            if (obj == null) return 0;
            else
            {
                long val = 0;
                if (long.TryParse(obj.ToString(), out val))
                {
                    return val;
                }
                else { return 0; }
            }
            //else return Convert.ToInt64(obj);
        }

        public static Double ToDouble(this Object obj)
        {
            if (obj == null) return 0;
            else if (obj == DBNull.Value) return 0;
            else if (obj.ToString() == "") return 0;
            else
            {
                double val = 0;
                if (double.TryParse(obj.ToString(), out val))
                {
                    return val;
                }
                else { return 0; }
            }
        }

        public static Double ToDouble(this Object obj, int decimalcharacters)
        {
            if (obj == null) return 0;
            else if (obj == DBNull.Value) return 0;
            else if (obj.ToString() == "") return 0;
            else
            {
                double val = 0;
                if (double.TryParse(obj.ToString(), out val))
                {
                    return Math.Round(val, decimalcharacters);
                }
                else { return 0; }
            }
        }



        public static Boolean ToBoolean(this Object obj)
        {
            try
            {
                if (obj == null) return false;
                if (obj?.ToString() == "1" || obj?.ToString() == "0")
                { obj = int.Parse(obj.ToString()); }

                return Convert.ToBoolean(obj);
            }
            catch { return false; }
        }

        public static DateTime ToDateTime(this Object obj)
        {
            try
            {
                if (obj == null) return new DateTime();
                return Convert.ToDateTime(obj);
            }
            catch { return new DateTime(); }
        }


        public static string GetScalarByDataSet(this DataSet dS, int table = 0, int row = 0, int item = 0)
        {

            if (dS == null) { return ""; }
            if (dS.Tables.Count < table) { return ""; }
            if (dS.Tables[table].Columns.Count < item + 1) { return ""; }
            if (dS.Tables[table].Rows.Count < row + 1) { return ""; }
            return dS.Tables[table]?.Rows[row][item].ToString() ?? String.Empty;
        }

        public static DataRow? SingleDataRow(this DataSet dS, int table = 0, int row = 0)
        {
            if (dS.Tables.Count >= table)
            {
                if (dS.Tables[table].Rows.Count >= row)
                {
                    return dS.Tables[table].Rows[row];
                }
            }
            return null;
        }




        public static string JoinAllObjectAsString(this Hashtable ht, string Seperator)
        {
            string filePaths = "";
            if (string.IsNullOrEmpty(Seperator)) Seperator = ",";
            for (var i = 0; i < ht.Keys.Count; i++)
                filePaths += ht[i]?.ToString() + Seperator;
            return filePaths;
        }

        /// <summary>
        /// Attach Query Parameters to a URL
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="hT"></param>
        /// <returns></returns>
        public static Uri AttachParameters(this Uri uri, Hashtable hT)
        {
            var stringBuilder = new StringBuilder();
            string str = "?";
            foreach (string key in hT.Keys)
            {
                stringBuilder.Append(str + key + "=" + hT[key]);
                str = "&";
            }
            return new Uri(uri + stringBuilder.ToString());
        }
    }
}
