using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GA.SuperSocket.AppClient.Utility
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether this System.Object is contained in the specified IEnumerable
        /// </summary>
        /// <param name="o">The System.Object</param>
        /// <param name="enumerable">The IEnumerable to check</param>
        /// <returns>true if enumerable contains this System.Object, otherwise false.</returns>
        public static bool In(this object o, IEnumerable enumerable)
        {
            foreach (object item in enumerable)
            {
                if (item.Equals(o))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether this T is contained in the specified 'IEnumerable of T'
        /// </summary>
        /// <typeparam name="T">This System.Object's type</typeparam>
        /// <param name="t">This item</param>
        /// <param name="enumerable">The 'IEnumerable of T' to check</param>
        /// <returns>true if enumerable contains this item, otherwise false.</returns>
        public static bool In<T>(this T t, IEnumerable<T> enumerable)
        {
            foreach (T item in enumerable)
            {
                if (item.Equals(t))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether this System.Object is contained in the specified values
        /// </summary>
        /// <param name="o">The System.Object</param>
        /// <param name="items">The values to compare</param>
        /// <returns>true if values contains this System.Object, otherwise false.</returns>
        public static bool In(this object o, params object[] items)
        {
            foreach (object item in items)
            {
                if (item.Equals(o))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether this T is contained in the specified values
        /// </summary>
        /// <typeparam name="T">This System.Object's type</typeparam>
        /// <param name="t">This item</param>
        /// <param name="items">The values to compare</param>
        /// <returns>true if values contains this item, otherwise false.</returns>
        public static bool In<T>(this T t, params T[] items)
        {
            foreach (T item in items)
            {
                if (item.Equals(t))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether this collections contains any of the specified values
        /// </summary>
        /// <typeparam name="T">The type of the values to compare</typeparam>
        /// <param name="t">This collection</param>
        /// <param name="items">The values to compare</param>
        /// <returns>true if the collection contains any of the specified values, otherwise false</returns>
        public static bool ContainsAnyOf<T>(this T t, params T[] items) where T : ICollection<T>
        {
            foreach (T item in items)
            {
                if (t.Contains(item))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// <para>Serializes the specified System.Object and writes the XML document</para>
        /// <para>to the specified file.</para>
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <param name="fileName">The file to which you want to write.</param>
        /// <returns>true if successful, otherwise false.</returns>
        public static bool XmlSerialize<T>(this T item, string fileName)
        {
            object o = new object();
            FileStream f = null;
            bool isSaved = false;
            try
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                lock (o)
                {
                    using (XmlWriter writer = XmlWriter.Create(fileName, settings))
                    {
                        xmlSerializer.Serialize(writer, item, xmlns);
                        writer.Close();
                    }
                    isSaved = true;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("ERROR: " + x.Message);
                return false;
            }
            finally
            {
                if (f != null)
                { f.Close(); }
            }

            return isSaved;
        }

        public static string XmlSerialize<T>(this T item)
        {
            object o = new object();
            try
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                lock (o)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    using (StringWriter stringWriter = new StringWriter(stringBuilder))
                    {
                        using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                        {
                            xmlSerializer.Serialize(xmlWriter, item, xmlns);
                            return stringBuilder.ToString();
                        }
                    }
                }
            }
            catch (Exception x)
            {
                return string.Empty;
            }
        }
    }
}