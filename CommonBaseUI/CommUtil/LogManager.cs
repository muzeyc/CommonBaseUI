using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CommonBaseUI.CommUtil
{

    public class LogManager
    {
        public static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    //if (System.Web.HttpContext.Current == null)
                    // Windows Forms 应用
                    //logPath = AppDomain.CurrentDomain.BaseDirectory;
                    //logPath = @"c:\";
                    /*else
                    // Web 应用*/
                    logPath = AppDomain.CurrentDomain.BaseDirectory + @"log\";

                    if (!Directory.Exists(logPath))
                        Directory.CreateDirectory(logPath);
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFielPrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix
        {
            get { return logFielPrefix; }
            set { logFielPrefix = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string msg)
        {
            try
            {
                string ip;
                /*if (System.Web.HttpContext.Current != null)
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"].ToString();
                else*/
                ip = "local";

                System.IO.StreamWriter sw = System.IO.File.AppendText(
                    LogPath + LogFielPrefix + logFile + "_" +
                    DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + "from:" + ip + " " + msg);
                sw.Close();
            }
            catch
            { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteGlobalLog(string msg)
        {
            try
            {

                if (!Directory.Exists(LogPath))
                    Directory.CreateDirectory(LogPath);
                string logFile = "log";
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                    LogPath + "Global_" + logFile + "_" +
                    DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + "from:" + msg);
                sw.Close();
            }
            catch
            { }
        }

        public static void WriteLog(string msg)
        {
            try
            {
                string ip;
                /*if (System.Web.HttpContext.Current != null)
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"].ToString();
                else*/
                if (!Directory.Exists(LogPath))
                    Directory.CreateDirectory(LogPath);
                ip = "local";
                string logFile = "log";
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                    LogPath + LogFielPrefix + logFile + "_" +
                    DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + "from:" + ip + " " + msg);
                sw.Close();
            }
            catch
            { }
        }

        public static void WriteLog(Exception ex)
        {
            WriteLog("------SysDataException------");
            WriteLog(ex.Message);
            WriteLog(ex.Source);
            WriteLog(ex.StackTrace);
            WriteLog("------SysDataException------");
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(SysLogFile logFile, string msg)
        {
            WriteLog(logFile.ToString(), msg);
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum SysLogFile
    {
        Trace,
        Warning,
        Error,
        SQL
    }
}
