using CommonBaseUI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CommonBaseUI.CommUtil
{
    public class HttpUtil
    {
        /// <summary>
        /// Get方式获取json数据
        /// </summary>
        /// <param name="html">URL地址</param>
        /// <returns></returns>
        public static string GetEx(string html,Dictionary<string,string> paramDic = null)//传入网址
        {
            if (paramDic != null)
            {
                var isFirst = true;
                foreach (var kv in paramDic) 
                {
                    if(isFirst)
                    {
                        html += "?";
                        isFirst = false;
                    }
                    else
                    {
                        html += "&";
                    }

                    html += kv.Key + "=" + kv.Value;
                }
            }
            string pageHtml = "";
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData(html); //从指定网站下载数据
            MemoryStream ms = new MemoryStream(pageData);
            using (StreamReader sr = new StreamReader(ms, Encoding.GetEncoding("UTF-8")))
            {
                pageHtml = sr.ReadLine();
            }
            return pageHtml;
        }

        /// <summary>
        /// Get传输数据
        /// </summary>
        /// <param name="Url">URL地址</param>
        /// <param name="jsonParas">Json数据</param>
        /// <returns></returns>
        public static string Get(string Url, Dictionary<string, string> paramDic = null)
        {
            string retString = "";
            try
            {
                if (paramDic != null)
                {
                    var isFirst = true;
                    foreach (var kv in paramDic)
                    {
                        if (isFirst)
                        {
                            Url += "?";
                            isFirst = false;
                        }
                        else
                        {
                            Url += "&";
                        }

                        Url += kv.Key + "=" + kv.Value;
                    }
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.KeepAlive = false;
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.Timeout = 60000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception e) 
            {
                var resModel = new ResponseModelBase();
                resModel.result = ResponseModelBase.FAILED;
                resModel.errMessage = e.Message;
                retString = resModel.Serializer();
            }

            return retString;
        }

        /// <summary>
        /// Post传输数据
        /// </summary>
        /// <param name="Url">URL地址</param>
        /// <param name="jsonParas">Json数据</param>
        /// <returns></returns>
        public static string Post(string Url, string jsonParas)
        {
            string strURL = Url;
            
            if (jsonParas.Equals(""))
            {
                jsonParas = "null";
            }

            //创建一个HTTP请求  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.Timeout = 60000;

            //设置参数，并进行URL编码 
            string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas);   

            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception e)
            {
                writer = null;
                Console.Write("连接服务器失败!");
                var resModel = new ResponseModelBase();
                resModel.result = ResponseModelBase.FAILED;
                resModel.errMessage = "连接服务器失败!" + e.Message;
                return resModel.Serializer();
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流

            //String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
                var resModel = new ResponseModelBase();
                resModel.result = ResponseModelBase.FAILED;
                resModel.errMessage = ex.Message;
                return resModel.Serializer();
            }

            Stream s = response.GetResponseStream();

            //Stream postData = System.Net.WebReq.InputStream;
            StreamReader sRead = new StreamReader(s);
            string postContent = sRead.ReadToEnd();
            sRead.Close();

            return postContent;//返回Json数据
        }
    }
}
