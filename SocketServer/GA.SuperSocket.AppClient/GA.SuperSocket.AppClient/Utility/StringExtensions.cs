using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace GA.SuperSocket.AppClient.Utility
{
    public static class StringExtensions
    {
        #region Private Members

        private static readonly Regex RegexHtmlTag = new Regex("<.*?>", RegexOptions.Compiled);

        #endregion Private Members

        public static string Append(this string s, string value)
        {
            return string.Concat(s, value);
        }

        public static int CharacterCount(this string s, char c)
        {
            return (from x in s.ToCharArray()
                    where x == c
                    select x).Count();
        }

        public static string CutString(this string str, int length)
        {
            string result = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                int r = i % length;
                int last = (str.Length / length) * length;

                if (i != 0 && i <= last)
                {

                    if (r == 0)
                    {
                        result += str.Substring(i - length, length) + "\n";
                    }
                }
                else if (i > last)
                {
                    result += str.Substring(i - 1);
                    break;
                }

            }

            return result;
        }

        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            return s.IndexOf(value, comparisonType) != -1;
        }

        /// <summary>
        /// <para>Returns a value indicating whether all of the specified System.String objects</para>
        /// <para>occur within this string.</para>
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="values">The strings to seek</param>
        /// <returns>true if all values are contained in this string; otherwise, false.</returns>
        public static bool ContainsAllOf(this string s, params string[] values)
        {
            foreach (string value in values)
            {
                if (!s.Contains(value))
                { return false; }
            }
            return true;
        }

        public static bool ContainsAllOf(this string s, params char[] values)
        {
            foreach (char value in values)
            {
                if (!s.Contains(value.ToString()))
                { return false; }
            }
            return true;
        }

        /// <summary>
        /// <para>Returns a value indicating whether any of the specified System.String objects</para>
        /// <para>occur within this string.</para>
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="values">The strings to seek</param>
        /// <returns>true if any value is contained in this string; otherwise, false.</returns>
        public static bool ContainsAnyOf(this string s, params string[] values)
        {
            foreach (string value in values)
            {
                if (s.Contains(value))
                { return true; }
            }
            return false;
        }

        public static bool ContainsAnyOf(this string s, params char[] values)
        {
            foreach (char value in values)
            {
                if (s.Contains(value.ToString()))
                { return true; }
            }
            return false;
        }

        public static bool ContainsAnyOf(this string s, IEnumerable<string> values)
        {
            foreach (string value in values)
            {
                if (string.IsNullOrEmpty(value))
                { continue; }
                if (s.Contains(value))
                { return true; }
            }
            return false;
        }

        public static string HtmlDecode(this string s)
        {
            return System.Web.HttpUtility.HtmlDecode(s);
        }

        public static string HtmlEncode(this string s)
        {
            return System.Web.HttpUtility.HtmlEncode(s);
        }

        public static string HtmlStrip(this string s)
        {
            return RegexHtmlTag.Replace(s, string.Empty);
        }

        /// <summary>
        /// Gets specified number of characters from left of string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Left(this string s, int count)
        {
            return s.Substring(0, count);
        }

        public static string Prepend(this string s, string value)
        {
            return string.Concat(value, s);
        }

        /// <summary>
        /// <para>Removes all spaces and tabs surrounding the specified substring contained</para>
        /// <para>within this System.String</para>
        /// </summary>
        /// <param name="s">The System.String to check</param>
        /// <param name="subString">The substring to remove whitespace from</param>
        /// <returns>System.String without whitespace around specified substring</returns>
        public static string RemoveSurroundingWhitespace(this string s, string subString)
        {
            string newString = s;

            while (newString.Contains(string.Concat(' ', subString)))
            { newString = newString.Replace(string.Concat(' ', subString), subString); }

            while (newString.Contains(string.Concat(subString, ' ')))
            { newString = newString.Replace(string.Concat(subString, ' '), subString); }

            while (newString.Contains(string.Concat('\t', subString)))
            { newString = newString.Replace(string.Concat('\t', subString), subString); }

            while (newString.Contains(string.Concat(subString, '\t')))
            { newString = newString.Replace(string.Concat(subString, '\t'), subString); }

            return newString;
        }

        public static string RegexDecode(this string s)
        {
            return Regex.Unescape(s);
        }

        public static string RegexEncode(this string s)
        {
            return Regex.Escape(s);
        }

        /// <summary>
        /// <para>Takes a System.String and returns a new System.String of the original</para>
        /// <para>repeated [n] number of times</para>
        /// </summary>
        /// <param name="s">The String</param>
        /// <param name="count">The number of times to repeat the String</param>
        /// <returns>A new System.String of the original repeated [n] number of times</returns>
        public static string Repeat(this string s, byte count)
        {
            if (count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(s.Length * byte.MaxValue);
            for (int i = 0; i < count; i++)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets specified number of characters from right of string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Right(this string s, int count)
        {
            return s.Substring(s.Length - count, count);
        }

        /// <summary>
        /// <para>Determines whether the beginning of this string instance matches</para>
        /// <para>one of the specified strings.</para>
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="values">The strings to compare</param>
        /// <returns>true if any value matches the beginning of this string; otherwise, false.</returns>
        public static bool StartsWithAnyOf(this string s, params string[] values)
        {
            foreach (string value in values)
            {
                if (s.StartsWith(value))
                { return true; }
            }
            return false;
        }

        public static bool ToFile(this string s, string filePath)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(filePath);
                }

                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);

                sw.Write(s);
                sw.Flush();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (sw != null)
                { sw.Close(); }

                if (fs != null)
                { fs.Close(); }
            }
        }

        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        public static string ToTitleCase(this string s, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(s);
        }

        public static string UrlEncode(this string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        public static string UrlDecode(this string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        public static int WordCount(this string s)
        {
            return s.Split(' ').Count();
        }

        /// <summary>
        /// Deserializes the XML data contained by the specified System.String
        /// </summary>
        /// <typeparam name="T">The type of System.Object to be deserialized</typeparam>
        /// <param name="s">The System.String containing xml data</param>
        /// <returns>The System.Object being deserialized.</returns>
        public static T XmlDeserialize<T>(this string s)
        {
            object o = new object();
            StringReader stringReader = new StringReader(s);
            XmlTextReader reader = new XmlTextReader(stringReader);
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                lock (o)
                {
                    T obj = (T)xmlSerializer.Deserialize(reader);
                    reader.Close();
                    return obj;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("ERROR: " + x.Message);
            }
            finally
            {
                if (reader != null)
                { reader.Close(); }
            }

            return default(T);
        }
    }
}