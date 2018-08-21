using CommonBaseUI.Common;
using CommonBaseUI.CommUtil;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyUploadButton.xaml 的交互逻辑
    /// </summary>
    public partial class MyUploadButton : UserControl
    {
        /// <summary>
        /// 创建一个线程
        /// </summary>
        private Thread ThdCopyFile;
        /// <summary>
        /// 源文件路径
        /// </summary>
        private string SourceFile;
        /// <summary>
        /// 指定该按钮所在画面的全称，如：CommonBaseUI.Controls.MyTab
        /// </summary>
        public string _ViewFullName { get; set; }

        private string _Text {
            get
            {
                return btnUpload.Content.ToStr();
            }
            set
            {
                btnUpload.Content = value;
            }
        }

        private string content;
        public string _Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                btnUpload.Content = value;
            }
        }

        /// <summary>
        /// 默认目录(以斜杠'\'结尾)
        /// </summary>
        public string _DefaultDirectory { get; set; }

        public MyUploadButton()
        {
            InitializeComponent();
        }

        private void btnUpload_MouseMove(object sender, MouseEventArgs e)
        {
            var border = btnUpload.Parent as Border;
            border.Background = CommonUtil.ToBrush("#1C86EE");
            border.BorderBrush = border.Background;
        }

        private void btnUpload_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = btnUpload.Parent as Border;
            border.Background = CommonUtil.ToBrush("#EBEBEB");
            border.BorderBrush = border.Background;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (_ViewFullName.IsNullOrEmpty())
            {
                FormCommon.ShowErr("必须将属性“_ViewFullName”设置为当前画面的全称！");
                return;
            } 
            
            if (this._DefaultDirectory.IsNullOrEmpty())
            {
                FormCommon.ShowErr("必须设置属性“_DefaultDirectory”！");
                return;
            }

            var op = new System.Windows.Forms.OpenFileDialog();
            op.RestoreDirectory = true;
            var result = op.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                SourceFile = op.FileName;

                this._Text = this._Content;
                this.IsEnabled = true;

                var arge = new MyUploadEventArge(BeforeUploadRoutedEvent, this);
                arge._SourceFileInfo = new FileInfo(op.FileName);
                RaiseEvent(arge);


                ThdCopyFile = new Thread(new ThreadStart(RunCopyFile));
                // 将子线程管理起来，便宜在画面关闭后将子线程停止
                ThreadManager._AddThread(_ViewFullName, ThdCopyFile);
                // 开始下载
                this.IsEnabled = false;
                ThdCopyFile.Start();
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFile">源文件路径</param>
        /// <param name="ToFile">目的文件路径</param>
        /// <param name="TranSize">传输大小</param>
        /// <param name="progressBar1">ProgressBar控件</param>
        private void CopyFile(string sourceFile, string ToFile, int TranSize)
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
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    FormCommon.ShowErr(ex.Message);
                    this.IsEnabled = true;
                }));
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
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    FormCommon.ShowErr(ex.Message);
                    this.IsEnabled = true;
                }));
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
                        this._Text = (currentVal * 100 / max).ToString() + "%";
                    }));
                }
                int leftSize = (int)FormerOpenStream.Length - copied; //获取剩余文件的大小
                FileSize = FormerOpenStream.Read(buffer, 0, leftSize); //读取剩余的字节
                FormerOpenStream.Flush();
                ToFileOpenStream.Write(buffer, 0, leftSize); //写入剩余的部分
                ToFileOpenStream.Flush();
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    this._Text = "100%";
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
                this._Text = this._Content;
                this.IsEnabled = true;

                var arge = new MyUploadEventArge(AfterUploadRoutedEvent, this);
                arge._SourceFileInfo = new FileInfo(sourceFile);
                arge._TargetFileInfo = new FileInfo(ToFile);
                RaiseEvent(arge);
            }));
        }

        /// <summary>
        /// 设置线程，运行copy文件，它与代理CopyFile_Delegate应具有相同的参数和返回类型
        /// </summary>
        private void RunCopyFile()
        {
            var file = new FileInfo(SourceFile);
            CopyFile(SourceFile, _DefaultDirectory + file.Name, 1024); //复制文件
            Thread.Sleep(0); //避免假死 
            ThdCopyFile.Abort();  //关闭线程
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent BeforeUploadRoutedEvent = EventManager.RegisterRoutedEvent(
            "_BeforeUpload", RoutingStrategy.Bubble, typeof(EventHandler<MyUploadEventArge>), typeof(MyUploadButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _BeforeUpload
        {
            add { base.AddHandler(BeforeUploadRoutedEvent, value); }
            remove { base.RemoveHandler(BeforeUploadRoutedEvent, value); }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent AfterUploadRoutedEvent = EventManager.RegisterRoutedEvent(
            "_AfterUpload", RoutingStrategy.Bubble, typeof(EventHandler<MyUploadEventArge>), typeof(MyUploadButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _AfterUpload
        {
            add { base.AddHandler(AfterUploadRoutedEvent, value); }
            remove { base.RemoveHandler(AfterUploadRoutedEvent, value); }
        }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class MyUploadEventArge : RoutedEventArgs
    {
        public MyUploadEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 源文件
        /// </summary>
        public FileInfo _SourceFileInfo { get; set; }
        /// <summary>
        /// 目标文件
        /// </summary>
        public FileInfo _TargetFileInfo { get; set; }
    }
}
