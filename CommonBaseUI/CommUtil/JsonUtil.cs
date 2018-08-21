//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace CommonBaseUI.CommUtil
{
    public static class JsonUtil
    {
        /// <summary>
        /// 将json数据反序列化为Dictionary
        /// </summary>
        /// <param name="jsonData">json数据</param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将DataTable转换成JSON格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(this DataTable dt)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList lstResult = new ArrayList();

            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dic.Add(col.ColumnName, row[col.ColumnName]);
                }

                lstResult.Add(dic);
            }

            return jss.Serialize(lstResult);
        }

        /// <summary>
        /// 将Dictionary转换成JSON格式
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string DictionaryToJson(this Dictionary<string, object> dic)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(dic);
        }

        /// <summary>
        /// 将Dictionary转换成JSON格式s
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string DictionaryToJson(this Dictionary<string, string> dic)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(dic);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="replaceChars">转义字符</param>
        /// <returns></returns>
        public static string Serializer(this object obj)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            string json = javaScriptSerializer.Serialize(obj);
            Dictionary<string, string> replaceChars = new Dictionary<string, string>();
            replaceChars.Add("%", "⊙");
            replaceChars.Add("=", "≡");
            replaceChars.Add("+", "▲");
            foreach (var item in replaceChars)
            {
                json = json.Replace(item.Key, item.Value);
            }

            return json;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T DeSerializer<T>(string json)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            try
            {
                json = Regex.Replace(json, @"/Date\((\d+)\)/", match =>
                {
                    DateTime datetime = new DateTime(1970, 1, 1);
                    datetime = datetime.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    datetime = datetime.ToLocalTime();
                    return datetime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
                return javaScriptSerializer.Deserialize<T>(json);
            }
            catch (Exception exp)
            {
                string ex = exp.Message;
                return default(T);
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <param name="json">是否日期转换,如果转换失败请尝试改成false或true</param>
        /// <returns></returns>
        public static T DeSerializer<T>(string json, bool flag)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            try
            {
                if (flag)
                {
                    json = Regex.Replace(json, @"/Date\((\d+)\)/", match =>
                    {
                        DateTime datetime = new DateTime(1970, 1, 1);
                        datetime = datetime.AddMilliseconds(long.Parse(match.Groups[1].Value));
                        datetime = datetime.ToLocalTime();
                        return datetime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    });
                }
                return javaScriptSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTableTwo(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
       
    }
}
