using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CommonBaseUI.CommUtil
{
    public class ModelUtil
    {
        /// <summary>
        /// ListModel转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DataTable ListModelToDataTable<T>(List<T> dataList, string tableName, List<string> columns = null)
        {
            DataTable dt = new DataTable(tableName);
            List<string> colNameList = new List<string>();
            if (dataList.Count > 0)
            {
                //追加列
                Type t = dataList[0].GetType();//获得该类的Type
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    if (columns != null)
                    {
                        if (columns.Contains(pi.Name))
                        {
                            dt.Columns.Add(pi.Name);
                            colNameList.Add(pi.Name);
                        }
                    }
                    else
                    {
                        dt.Columns.Add(pi.Name);
                        colNameList.Add(pi.Name);
                    }
                }

                //追加行
                foreach (T dataT in dataList)
                {
                    DataRow dr = dt.NewRow();
                    foreach (string colName in colNameList)
                    {
                        dr[colName] = t.GetProperty(colName).GetValue(dataT, null);
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T obj = System.Activator.CreateInstance<T>();
                Type t = obj.GetType();

                foreach (var pi in t.GetProperties())
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        var typeName = t.GetProperty(pi.Name).PropertyType.Name;
                        if ("Nullable`1".Equals(typeName))
                        {
                            typeName = t.GetProperty(pi.Name).PropertyType.GenericTypeArguments[0].Name;
                        }
                        switch (typeName)
                        {
                            case "String":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToStr(), null);
                                break;
                            case "Int32":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToInt(), null);
                                break;
                            case "Double":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDouble(), null);
                                break;
                            case "Decimal":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDec(), null);
                                break;
                            case "Int64":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToLong(), null);
                                break;
                            case "DateTime":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDateTime(), null);
                                break;
                            default:
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name], null);
                                break;
                        }
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromObj"></param>
        /// <returns></returns>
        public static T Copy<T>(object obj)
        {
            //获得该类的Type
            Type t = obj.GetType();

            var dic = new Dictionary<string, object>();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                dic[pi.Name] = t.GetProperty(pi.Name).GetValue(obj, null);
            }

            T newObj = System.Activator.CreateInstance<T>();
            Type rt = newObj.GetType();
            foreach (PropertyInfo pi in rt.GetProperties())
            {
                if (dic.ContainsKey(pi.Name))
                {
                    rt.GetProperty(pi.Name).SetValue(newObj, dic[pi.Name], null);
                }
            }

            return newObj;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="fromObj"></typeparam>
        /// <param name="toObj"></param>
        /// <returns></returns>
        public static void Copy(object fromObj, object toObj)
        {
            //获得该类的Type
            Type t = fromObj.GetType();

            var dic = new Dictionary<string, object>();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                dic[pi.Name] = t.GetProperty(pi.Name).GetValue(fromObj, null);
            }

            Type rt = toObj.GetType();
            foreach (PropertyInfo pi in rt.GetProperties())
            {
                if (dic.ContainsKey(pi.Name))
                {
                    rt.GetProperty(pi.Name).SetValue(toObj, dic[pi.Name], null);
                }
            }
        }
    }
}
