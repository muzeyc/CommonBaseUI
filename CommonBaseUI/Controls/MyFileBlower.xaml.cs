using CommonBaseUI.Common;
using CommonUtils;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyFileUpload.xaml 的交互逻辑
    /// </summary>
    public partial class MyFileBlower : UserControl, IInputControl
    {
        public FileInfo _FileInfo { get; set; }
        public MyFileBlower()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 文件的完整路径
        /// </summary>
        public object _Value
        {
            get
            {
                return txtInput.Text;
            }
            set
            {
                txtInput.Text = value.ToStr();
            }
        }

        public bool _IsEnabled
        {
            get
            {
                return txtInput.IsEnabled;
            }
            set
            {
                txtInput.IsEnabled = value;
            }
        }

        private string _Text
        {
            get
            {
                return lblCaption.Content.ToStr();
            }
            set
            {
                lblCaption.Content = value;
            }
        }


        private string content;
        public string _Caption
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                lblCaption.Content = value;
            }
        }

        private bool mustInput = false;
        public bool _MustInput
        {
            get
            {
                return mustInput;
            }
            set
            {
                mustInput = value;
            }
        }

        /// <summary>
        /// 绑定的对象中的字段
        /// </summary>
        public string _Binding { get; set; }

        /// <summary>
        /// 设置输入框背景色
        /// </summary>
        public void _SetErr()
        {
            this.txtInput.Background = CommonUtils.CommonUtil.ToBrush("#FA8072");
            this.txtInput.Foreground = CommonUtils.CommonUtil.ToBrush("#FFFFFF");
        }

        public double _CaptionWidth
        {
            get
            {
                return lblCaption.Width;
            }
            set
            {
                lblCaption.Width = value;
            }
        }

        public double _InputWidth
        {
            get
            {
                return txtInput.Width;
            }
            set
            {
                txtInput.Width = value;
            }
        }

        /// <summary>
        /// 清除输入框背景色
        /// </summary>
        public void _CleanErr()
        {
            this.txtInput.Background = CommonUtils.CommonUtil.ToBrush("#FFFFFF");
            this.txtInput.Foreground = CommonUtils.CommonUtil.ToBrush("#000000");
        }


        /// <summary>
        /// 清除数据
        /// </summary>
        public void _Clear()
        {
            _Value = string.Empty;
        }

        /// <summary>
        /// 清除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _Clear();
        }

        /// <summary>
        /// 打开文件浏览器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var op = new System.Windows.Forms.OpenFileDialog();
            //默认的打开路径
            if (!txtInput.Text.IsNullOrEmpty())
            {
                var file = new FileInfo(txtInput.Text);
                op.InitialDirectory = file.DirectoryName;
            }
            op.RestoreDirectory = true;
            //op.Filter = "所有文件(*.*)|*.* ";
            var result = op.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _Value = op.FileName;
                _FileInfo = new FileInfo(op.FileName);
                var arge = new MyDownloadEventArge(AfterFileSelectEvent, this);
                arge._TargetFileInfo = new FileInfo(op.FileName);
                RaiseEvent(arge);
            }
        }

        /// <summary>
        /// 指定该按钮所在画面的全称，如：CommonBaseUI.Controls.MyTab
        /// </summary>
        public string _ViewFullName { get; set; }

        /// <summary>
        /// 默认目录(以斜杠'\'结尾)
        /// </summary>
        public string _DefaultDirectory { get; set; }


        #region 上传文件
        /// <summary>
        /// 创建一个线程
        /// </summary>
        private Thread ThdCopyFile;
        public delegate void AfterUploadDelegate(bool result, string message);
        private AfterUploadDelegate _AfterUpload;

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFile">源文件路径</param>
        /// <param name="ToFile">目的文件路径</param>
        /// <param name="TranSize">传输大小</param>
        /// <param name="progressBar1">ProgressBar控件</param>
        private void UploadFile(string sourceFile, string ToFile, int TranSize)
        {
            if (sourceFile.IsNullOrEmpty())
            {
                FormCommon.ShowErr("未指定源文件！");
                return;
            }

            // 实例化源文件FileStream类
            FileStream FormerOpenStream;
            // 实例化目标文件FileStream类
            FileStream ToFileOpenStream;
            try
            {
                FormerOpenStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);//以只读方式打开源文件
            }
            catch (IOException ex)
            {
                if (_AfterUpload != null)
                {
                    _AfterUpload(false, ex.Message);
                }
                return;
            }

            try
            {
                FileStream fileToCreate = new FileStream(ToFile, FileMode.Create); //创建目的文件，如果已存在将被覆盖
                fileToCreate.Close();//关闭所有fileToCreate的资源
                fileToCreate.Dispose();//释放所有fileToCreate的资源
            }
            catch (IOException ex)
            {
                if (_AfterUpload != null)
                {
                    _AfterUpload(false, ex.Message);
                }
                return;
            }

            ToFileOpenStream = new FileStream(ToFile, FileMode.Append, FileAccess.Write);//以写方式打开目的文件

            //根据一次传输的大小，计算最大传输个数. Math.Ceiling 方法 (Double),返回大于或等于指定的双精度浮点数的最小整数值。
            int max = Convert.ToInt32(Math.Ceiling((Double)FormerOpenStream.Length / (Double)TranSize));
            int currentVal = 0;
            //progressBar1.Maximum = max;//设置进度条的最大值
            int FileSize; //每次要拷贝的文件的大小
            if (TranSize < FormerOpenStream.Length)  //如果分段拷贝，即每次拷贝内容小于文件总长度
            {
                byte[] buffer = new byte[TranSize]; //根据传输的大小，定义一个字节数组，用来存储传输的字节
                int copied = 0;//记录传输的大小
                int tem_n = 1;//设置进度栏中进度的增加个数
                while (copied <= ((int)FormerOpenStream.Length - TranSize))
                {
                    FileSize = FormerOpenStream.Read(buffer, 0, TranSize);//从0开始读到buffer字节数组中，每次最大读TranSize
                    FormerOpenStream.Flush();   //清空缓存
                    ToFileOpenStream.Write(buffer, 0, TranSize); //向目的文件写入字节
                    ToFileOpenStream.Flush();//清空缓存
                    ToFileOpenStream.Position = FormerOpenStream.Position; //是源文件的目的文件流的位置相同
                    copied += FileSize; //记录已经拷贝的大小
                    currentVal += tem_n; //增加进度栏的进度块

                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        this._Text = _Caption + (currentVal * 100 / max).ToString() + "%";
                    }));
                }
                int leftSize = (int)FormerOpenStream.Length - copied; //获取剩余文件的大小
                FileSize = FormerOpenStream.Read(buffer, 0, leftSize); //读取剩余的字节
                FormerOpenStream.Flush();
                ToFileOpenStream.Write(buffer, 0, leftSize); //写入剩余的部分
                ToFileOpenStream.Flush();
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    this._Text = _Caption + "100%";
                    Thread.Sleep(1000);
                }));
            }
            else //如果整体拷贝，即每次拷贝内容大于文件总长度
            {
                byte[] buffer = new byte[FormerOpenStream.Length];
                FormerOpenStream.Read(buffer, 0, (int)FormerOpenStream.Length);
                FormerOpenStream.Flush();
                ToFileOpenStream.Write(buffer, 0, (int)FormerOpenStream.Length);
                ToFileOpenStream.Flush();

            }
            FormerOpenStream.Close();
            ToFileOpenStream.Close();
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                this._Text = this._Caption;
                this.IsEnabled = true;
            }));
        }

        /// <summary>
        /// 设置线程，运行copy文件，它与代理CopyFile_Delegate应具有相同的参数和返回类型
        /// </summary>
        private void RunCopyFile()
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (_FileInfo == null)
                {
                    _SetErr();
                    FormCommon.ShowErr("请选择要上传的文件！");
                    return;
                }
                if (!File.Exists(_FileInfo.FullName))
                {
                    _SetErr();
                    FormCommon.ShowErr("源文件不存在！");
                    return;
                }
            }));

            var path = _DefaultDirectory;
            try
            {
                UploadFile(_FileInfo.FullName, path + _FileInfo.Name, 1024); //复制文件
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    if (_AfterUpload != null)
                    {
                        _AfterUpload(false, ex.Message);
                    }
                }));
            }
            Thread.Sleep(0); //避免假死 

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (_AfterUpload != null)
                {
                    _AfterUpload(true, null);
                }
            }));
            ThdCopyFile.Abort();  //关闭线程
        }

        public void _StartUpload(AfterUploadDelegate afterUploadCallBack = null)
        {
            _AfterUpload = afterUploadCallBack;

            ThdCopyFile = new Thread(new ThreadStart(RunCopyFile));
            // 将子线程管理起来，以便在画面关闭后将子线程停止
            ThreadManager._AddThread(_ViewFullName, ThdCopyFile);
            // 开始下载
            this.IsEnabled = false;
            ThdCopyFile.Start();
        }

        #endregion

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent AfterFileSelectEvent = EventManager.RegisterRoutedEvent(
            "_AfterFileSelect", RoutingStrategy.Bubble, typeof(EventHandler<MyDownloadEventArge>), typeof(MyFileBlower));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _AfterFileSelect
        {
            add { base.AddHandler(AfterFileSelectEvent, value); }
            remove { base.RemoveHandler(AfterFileSelectEvent, value); }
        }
    }
}
