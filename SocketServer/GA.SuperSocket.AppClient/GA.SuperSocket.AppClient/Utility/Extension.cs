using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace GA.SuperSocket.AppClient.Utility
{
    /// <summary>
    /// 常用扩展类
    /// </summary>
    public static class Extension
    {
        public static string ToString2(this object val)
        {
            if (val == null)
                return string.Empty;
            return val.ToString();
        }

        public static DateTime? ToDateTime(this string val)
        {
            if (string.IsNullOrEmpty(val)) return null;
            DateTime dt;
            if (DateTime.TryParse(val, out dt))
            {
                return dt;
            }
            return null;
        }

        public static bool ToBoolean(this string val)
        {
            if (string.IsNullOrEmpty(val)) return false;
            return val.ToLower() == Boolean.TrueString.ToLower();
        }

        public static bool IsNullOrEmpty(this string val)
        {
            return string.IsNullOrEmpty(val);
        }

        public static bool IsNotEmpty(this string val)
        {
            return !string.IsNullOrEmpty(val);
        }

        public static int ToInt(this string val)
        {
            int intValue;
            if (int.TryParse(val, out intValue))
            {
                return intValue;
            }
            return 0;
        }

        public static long ToLong(this string val)
        {
            long intValue;
            if (long.TryParse(val, out intValue))
            {
                return intValue;
            }
            return 0;
        }

        public static decimal ToDecimal(this string val)
        {
            decimal intValue;
            if (decimal.TryParse(val, out intValue))
            {
                return intValue;
            }
            return 0;
        }

        public static double ToDouble(this string val)
        {
            double result;
            if (double.TryParse(val, out result))
            {
                return result;
            }
            return 0;
        }

        public static float ToFloat(this string val)
        {
            float result;
            if (float.TryParse(val, out result))
            {
                return result;
            }
            return 0;
        }

        /// <summary>
        /// 通用简单实体类型互转
        /// </summary>
        public static List<ResultType> ConvertToEntityList<ResultType>(this object list) where ResultType : new()
        {
            List<ResultType> ResultList = new List<ResultType>();
            if (list == null) return ResultList;
            Type fromObj = list.GetType();
            if (fromObj.Equals(typeof(DataTable)))
            {
                var dt = list as DataTable;
                ResultList = dt.Rows.Cast<DataRow>().Where(m => !(m.RowState == DataRowState.Deleted || m.RowState == DataRowState.Detached)).Select(m => m.ConvertToEntityByDataRow<ResultType>()).ToList();
            }
            else if (list is IEnumerable)
            {
                ResultList = ((IList)list).Cast<object>().Select(m => m.ConvertToEntity<ResultType>()).ToList();
            }
            return ResultList;
        }

        /// <summary>
        /// 通用简单实体类型互转
        /// </summary>
        public static ResultType ConvertToEntity<ResultType>(this object fromEntity) where ResultType : new()
        {
            ResultType t = new ResultType();
            Type fromObj = fromEntity.GetType();
            if (fromObj.Equals(typeof(DataRow)))
            {
                //DataRow类型
                DataRow dr = fromEntity as DataRow;
                t = dr.ConvertToEntityByDataRow<ResultType>();
            }
            else
            {
                Type type = typeof(ResultType);
                PropertyInfo[] properties = type.GetProperties();
                PropertyInfo[] fromProperties = fromObj.GetProperties();
                foreach (PropertyInfo pro in properties)
                {
                    foreach (var fromPro in fromProperties)
                    {
                        if (fromPro.Name.Equals(pro.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            object value = fromPro.GetValue(fromEntity, null);
                            if (value != null && value != DBNull.Value)
                            {
                                if (fromPro.PropertyType.Name != pro.PropertyType.Name)
                                {
                                    if (pro.PropertyType.IsEnum)
                                    {
                                        pro.SetValue(t, Enum.Parse(pro.PropertyType, value.ToString()), null);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            value = Convert.ChangeType
                                            (
                                                value,
                                                (Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType)
                                            );
                                            pro.SetValue(t, value, null);
                                        }
                                        catch { }
                                    }
                                }
                                else
                                {
                                    pro.SetValue(t, value, null);
                                }
                            }
                            else
                            {
                                pro.SetValue(t, null, null);
                            }
                            break;
                        }
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// DataRow转换为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ConvertToEntityByDataRow<T>(this DataRow dr) where T : new()
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            T t = new T();
            if (dr == null) return t;
            var columns = dr.Table.Columns.Cast<DataColumn>();
            foreach (PropertyInfo pi in properties)
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    if (pi.Name.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        object value = dr[column];
                        if (value != null && value != DBNull.Value)
                        {
                            if (value.GetType().Name != pi.PropertyType.Name)
                            {
                                if (pi.PropertyType.IsEnum)
                                {
                                    pi.SetValue(t, Enum.Parse(pi.PropertyType, value.ToString()), null);
                                }
                                else
                                {
                                    try
                                    {
                                        value = Convert.ChangeType
                                        (
                                            value,
                                            (Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType)
                                        );
                                        pi.SetValue(t, value, null);
                                    }
                                    catch { }
                                }
                            }
                            else
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                        else
                        {
                            pi.SetValue(t, null, null);
                        }
                        break;
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 转换为DataTable，如果是集合没有数据行时候会抛异常。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(this object list)
        {
            if (list == null) return null;
            DataTable dt = new DataTable();
            if (list is IEnumerable)
            {
                var li = (IList)list;
                //li[0]代表的是一个对象，list没有行时，会抛异常。
                PropertyInfo[] properties = li[0].GetType().GetProperties();
                dt.Columns.AddRange(properties.Where(m => !m.PropertyType.IsClass || !m.PropertyType.IsInterface).Select(m =>
                    new DataColumn(m.Name, Nullable.GetUnderlyingType(m.PropertyType) ?? m.PropertyType)).ToArray());
                foreach (var item in li)
                {
                    DataRow dr = dt.NewRow();
                    foreach (PropertyInfo pp in properties.Where(m => m.PropertyType.GetProperty("Item") == null)) //过滤含有索引器的属性
                    {
                        object value = pp.GetValue(item, null);
                        dr[pp.Name] = value == null ? DBNull.Value : value;
                    }
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                PropertyInfo[] properties = list.GetType().GetProperties();
                properties = properties.Where(m => m.PropertyType.GetProperty("Item") == null).ToArray();//过滤含有索引器的属性
                dt.Columns.AddRange(properties.Select(m =>
                    new DataColumn(m.Name, Nullable.GetUnderlyingType(m.PropertyType) ?? m.PropertyType)).ToArray());
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pp in properties)
                {
                    object value = pp.GetValue(list, null);
                    dr[pp.Name] = value == null ? DBNull.Value : value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 实体类公共属性值复制
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        public static void CopyTo(this object entity, object target)
        {
            if (target == null) return;
            if (entity.GetType() != target.GetType())
                return;
            PropertyInfo[] properties = target.GetType().GetProperties();
            foreach (PropertyInfo pro in properties)
            {
                if (pro.PropertyType.GetProperty("Item") != null)
                    continue;
                object value = pro.GetValue(entity, null);
                if (value != null)
                {
                    if (value is ICloneable)
                    {
                        pro.SetValue(target, (value as ICloneable).Clone(), null);
                    }
                    else
                    {
                        pro.SetValue(target, value.Copy(), null);
                    }
                }
                else
                {
                    pro.SetValue(target, null, null);
                }
            }
        }

        public static object Copy(this object obj)
        {
            if (obj == null) return null;
            Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            if (targetType.IsValueType == true)
            {
                targetDeepCopyObj = obj;
            }
            else
            {
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.GetType().GetProperty("Item") != null)
                        continue;
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, fieldValue.Copy());
                        }
                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                myProperty.SetValue(targetDeepCopyObj, propertyValue.Copy(), null);
                            }
                        }
                    }
                }
            }
            return targetDeepCopyObj;
        }
    }
}