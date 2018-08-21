using CommonBaseUI.Common;
using CommonBaseUI.CommUtil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonBaseUI.Controls
{
    public class ScanManager
    {
        private KeyboardHook Hook;
        private DateTime previewTime = DateTime.MinValue;
        private StringBuilder ScanStr;
        private Dictionary<string, string> KeyDic;
        private Dictionary<string, string> CombKeyDic;
        private string FormName;

        public delegate void AfterScanDelegate(string scanResult);
        public event AfterScanDelegate _AfterScan;

        /// <summary>
        /// 状态
        /// 0：Start，1：Stop
        /// </summary>
        public int _Status { get; set; }

        public ScanManager(UserControl form)
        {
            FormName = form.GetType().FullName;
            InitKeyDic();
            Hook = new KeyboardHook();
            // 键按下
            Hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);
        }
        private void InitKeyDic()
        {
            this.KeyDic = new Dictionary<string, string>();
            this.KeyDic.Add(Key.OemMinus.ToString(), "-");
            this.KeyDic.Add(Key.OemPeriod.ToString(), ".");
            this.KeyDic.Add(Key.OemPlus.ToString(), "+");
            this.KeyDic.Add("LShiftKey", "");
            this.KeyDic.Add("Back", "");
            this.KeyDic.Add("Capital", "");
            this.KeyDic.Add(Key.D0.ToString(), "0");
            this.KeyDic.Add(Key.D1.ToString(), "1");
            this.KeyDic.Add(Key.D2.ToString(), "2");
            this.KeyDic.Add(Key.D3.ToString(), "3");
            this.KeyDic.Add(Key.D4.ToString(), "4");
            this.KeyDic.Add(Key.D5.ToString(), "5");
            this.KeyDic.Add(Key.D6.ToString(), "6");
            this.KeyDic.Add(Key.D7.ToString(), "7");
            this.KeyDic.Add(Key.D8.ToString(), "8");
            this.KeyDic.Add(Key.D9.ToString(), "9");

            this.CombKeyDic = new Dictionary<string, string>();
            this.CombKeyDic.Add("LShiftKeyD9", "(");
            this.CombKeyDic.Add("LShiftKeyD0", ")");
            this.CombKeyDic.Add("LShiftKeyD3", "#");
            this.CombKeyDic.Add("LShiftKeyD2", "@");
        }
        public void _Start()
        {
            Hook.Start();
            _Status = 1;
        }

        public void _Stop()
        {
            Hook.Stop();
            _Status = 0;
        }

        /// <summary>
        /// 键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Console.WriteLine(e.KeyData.ToString());
            try
            {
                var now = DateTime.Now;
                if (DateTime.MinValue.Equals(previewTime))
                {
                    this.ScanStr = new StringBuilder();
                    previewTime = now;
                }

                var milliseconds = (DateTime.Now - previewTime).Milliseconds;
                previewTime = now;
                // 50毫秒内视为扫码
                if (milliseconds < 50)
                {
                    if (!Key.Tab.ToString().Equals(e.KeyData.ToString())
                        && !Key.Return.ToString().Equals(e.KeyData.ToString()))
                    {
                        this.ScanStr.Append(e.KeyCode.ToString());
                    }
                    else
                    {
                        string scanResult = GetScanStr();
                        Console.WriteLine("ScanResult:" + scanResult);
                        if (!scanResult.IsNullOrEmpty())
                        {
                            if (_AfterScan != null)
                            {
                                _AfterScan(scanResult);
                            }
                            this.ScanStr = new StringBuilder();
                            previewTime = DateTime.MinValue;
                        }
                    }
                }
                else
                {
                    previewTime = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                //FormCommon.ShowErr("扫码失败！");
            }
        }

        /// <summary>
        /// 加工扫码字符
        /// </summary>
        /// <returns></returns>
        private string GetScanStr()
        {
            string scan = this.ScanStr.ToString();
            foreach (var item in this.CombKeyDic)
            {
                scan = scan.Replace(item.Key, item.Value);
            }

            foreach (var item in this.KeyDic)
            {
                scan = scan.Replace(item.Key, item.Value);
            }

            return scan;
        }

        
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class ScanFinishEventArge : RoutedEventArgs
    {
        public ScanFinishEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 列号
        /// </summary>
        public object _ScanResult { get; set; }
    }
}
