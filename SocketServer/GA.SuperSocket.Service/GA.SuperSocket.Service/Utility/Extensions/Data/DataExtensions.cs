using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Globalegrow.Toolkit
{
    public static class DataExtensions
    {
        public static void AddRange(this DataColumnCollection dataColumns, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                dataColumns.Add(columnName);
            }
        }

        public static void AddRange(this DataColumnCollection dataColumns, params DataColumn[] columns)
        {
            foreach (DataColumn column in columns)
            {
                dataColumns.Add(column);
            }
        }

        public static void AddRange<T>(this DataColumnCollection dataColumns, IEnumerable<string> enumerable)
        {
            foreach (string item in enumerable)
            {
                dataColumns.Add(item);
            }
        }

        public static bool TryAdd(this DataColumnCollection dataColumns, string columnName)
        {
            if (!dataColumns.Contains(columnName))
            {
                dataColumns.Add(columnName);
                return true;
            }
            return false;
        }

        public static bool SaveToCsv(this DataSet dataSet, string filePath)
        {
            return dataSet.SaveToCsv(filePath, true);
        }

        public static bool SaveToCsv(this DataSet dataSet, string filePath, bool outputColumnNames)
        {
            bool ok = false;
            StringBuilder sb = new StringBuilder(2000);
            if (dataSet.Tables.Count > 0)
            {
                foreach (DataTable table in dataSet.Tables)
                {
                    #region Column Names

                    if (outputColumnNames)
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            sb.Append(column.ColumnName);
                            sb.Append(',');
                        }
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                    }

                    #endregion Column Names

                    #region Rows (Data)

                    foreach (DataRow row in table.Rows)
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            sb.Append(row[column].ToString());
                            sb.Append(',');
                        }
                        //Remove Last ','
                        sb.Remove(sb.Length - 1, 1);
                        sb.Append(Environment.NewLine);
                    }

                    #endregion Rows (Data)
                }

                ok = sb.ToString().ToFile(filePath);
            }

            return ok;
        }
    }
}