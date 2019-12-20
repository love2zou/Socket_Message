using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace GA.SuperSocket.AppClient.Utility
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class,new()
        {
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            Type t = typeof(TResult);
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            List<TResult> oblist = new List<TResult>();
            foreach (DataRow row in dt.Rows)
            {
                TResult ob = new TResult();
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                oblist.Add(ob);
            }
            return oblist;
        }

        /// <summary>
        /// 转换为一个DataTable
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult : class
        {
            List<PropertyInfo> pList = new List<PropertyInfo>();
            Type type = typeof(TResult);
            DataTable dt = new DataTable();
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in value)
            {
                DataRow row = dt.NewRow();
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}