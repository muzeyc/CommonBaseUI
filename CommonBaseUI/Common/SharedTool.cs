using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;  

namespace CommonBaseUI.Common
{
    public class SharedTool : IDisposable
    {
        // obtains user token         
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser         
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        extern static bool CloseHandle(IntPtr handle);

        [DllImport("Advapi32.DLL")]
        static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("Advapi32.DLL")]
        static extern bool RevertToSelf();
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_NEWCREDENTIALS = 9;//域控中的需要用:Interactive = 2         
        private bool disposed;

        public SharedTool(string username, string password, string ip)
        {
            // initialize tokens         
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);

            try
            {
                // get handle to token         
                bool bImpersonated = LogonUser(username, ip, password,
                    LOGON32_LOGON_NEWCREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                if (bImpersonated)
                {
                    if (!ImpersonateLoggedOnUser(pExistingTokenHandle))
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        throw new Exception("ImpersonateLoggedOnUser error;Code=" + nErrorCode);
                    }
                }
                else
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    throw new Exception("LogonUser error;Code=" + nErrorCode);
                }
            }
            finally
            {
                // close handle(s)         
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                RevertToSelf();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 打开服务器的共享文件
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="password"></param>
        /// <param name="userName"></param>
        public static void OpenFile(string sharePath, string userName, string password, string filePath)
        {
            bool status = false;
            status = connectState(sharePath, userName, password);
            if (status)
            {
                System.Diagnostics.Process.Start(filePath);
            }
            else
            {
                Console.Write("共享文件连接失败");
            }
        }

        /// <summary>
        /// 打开服务器的PLM图纸
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="password"></param>
        /// <param name="userName"></param>
        public static void OpenFileForPlmPDF(string filePath, string SHARE_PATH, string SERVER_USERNAME, string SERVER_PASSWORD)
        {
            bool status = false;
            status = connectState(SHARE_PATH, SERVER_USERNAME, SERVER_PASSWORD);
            bool openFlag = false;
            if (status)
            {
                int num =0;
                while (num < 10)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(filePath);
                        openFlag = true;
                        break;
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(800);
                    }

                    num++;
                }
            }

            if (!openFlag)
            {
                FormCommon.ShowErr("设计图打开失败");
            }
        }

        /// <summary>
        /// 连接远程共享文件夹
        /// </summary>
        /// <param name="path">远程共享文件夹的路径</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "net use " + path + " " + passWord + " /user:" + userName;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
    }    
}
