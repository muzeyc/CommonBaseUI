using CommonBaseUI.Common;
using CommonBaseUI.CommonView;
using CommonBaseUI.CommUtil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyAttachment.xaml 的交互逻辑
    /// </summary>
    public partial class MyAttachment : UserControl
    {
        /// <summary>
        /// 创建一个线程
        /// </summary>
        private Thread ThdCopyFile;
        /// <summary>
        /// 源文件路径
        /// </summary>
        private string SourceFile;
        private Dictionary<string, FileInfo> DeletedFiles;
        /// <summary>
        /// 指定该按钮所在画面的全称，如：CommonBaseUI.Controls.MyTab
        /// </summary>
        public string _ViewFullName { get; set; }
        /// <summary>
        /// 默认目录(以斜杠'\'结尾)
        /// </summary>
        public string _DefaultDirectory { get; set; }
        /// <summary>
        /// 附件信息列表
        /// </summary>
        public List<FileInfo> _AttachmentList { get; set; }

        private bool isEnabled = true;
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool _IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                btnAdd.Visibility = isEnabled ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                foreach (var item in body.Children)
                {
                    if (item is AttachmentItem)
                    {
                        var col = item as AttachmentItem;
                        col.ContextMenu.Visibility = isEnabled ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    }
                }
            }
        }

        public MyAttachment()
        {
            this._AttachmentList = new List<FileInfo>();
            this.DeletedFiles = new Dictionary<string, FileInfo>();
            InitializeComponent();
        }

        /// <summary>
        /// 刷新附件列表
        /// </summary>
        /// <param name="list"></param>
        public void _SetAttachmentList(List<FileInfo> list)
        {
            _AttachmentList = list == null ? new List<FileInfo>() : list;
            for (int i = body.Children.Count - 2; i >= 0; i--)
            {
                body.Children.RemoveAt(i);
            }
            for (int i = 0; i < list.Count; i++)
            {
                CreateSmallImg(list[i], i);
            }
        }

        /// <summary>
        /// 获取所有附件的列表字符串，复数个附件用","分隔
        /// </summary>
        /// <returns></returns>
        public string _GetAttachmentListStr()
        {
            var list = new List<string>();
            foreach (var file in this._AttachmentList){
                list.Add(file.FullName);
            }
            return string.Join(",", list);
        }

        private void btnAdd_Click(object sender, MouseButtonEventArgs e)
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

                //this._Text = this._Content;
                this.IsEnabled = true;

                var arge = new MyUploadEventArge(BeforeAttarchUploadRoutedEvent, this);
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show(ex.Message);
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
                        lblPercent.Content = (currentVal * 100 / max).ToString() + "%";
                    }));
                }
                int leftSize = (int)FormerOpenStream.Length - copied; //获取剩余文件的大小
                FileSize = FormerOpenStream.Read(buffer, 0, leftSize); //读取剩余的字节
                FormerOpenStream.Flush();
                ToFileOpenStream.Write(buffer, 0, leftSize); //写入剩余的部分
                ToFileOpenStream.Flush();
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
                lblPercent.Content = "100%";
                var fileInfo = new FileInfo(ToFile);
                _AttachmentList.Add(fileInfo);
                MyTimer.SetTimeout(1000, () =>
                {
                    lblPercent.Content = "";
                    CreateSmallImg(fileInfo, _AttachmentList.Count - 1);
                });

                this.IsEnabled = true;

                var arge = new MyUploadEventArge(AftervUploadRoutedEvent, this);
                arge._SourceFileInfo = new FileInfo(sourceFile);
                arge._TargetFileInfo = new FileInfo(ToFile);
                RaiseEvent(arge);
            }));
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        private void CreateSmallImg(FileInfo fileInfo, int index)
        {
            var border = new AttachmentItem();
            border._FileInfo = fileInfo;
            border._index = index;

            border.Height = 60;
            border.Width = 60;
            border.Margin = new Thickness(0, 0, 5, 5);
            border.BorderBrush = CommonUtil.ToBrush("#4a4a4a");
            border.BorderThickness = new Thickness(0.5);
            border.Background = CommonUtil.ToBrush("#FFFFFF");
            border.ContextMenu = new System.Windows.Controls.ContextMenu();

            // 创建右键菜单
            var deleteMenu = new AttachmentMenuItem();
            deleteMenu.Header = "删除";
            deleteMenu._FileInfo = fileInfo;
            deleteMenu._index = index;
            deleteMenu.Click += btnDel_Click;
            border.ContextMenu.Items.Add(deleteMenu);

            var saveMenu = new AttachmentMenuItem();
            saveMenu.Header = "另存为";
            saveMenu._FileInfo = fileInfo;
            saveMenu._index = index;
            saveMenu.Click += btSave_Click;
            border.ContextMenu.Items.Add(saveMenu);

            border.MouseMove += btnAdd_MouseMove;
            border.MouseLeave += btnAdd_MouseLeave;

            // 创建缩略图
            var img = new Image();
            if (".jpg".Equals(fileInfo.Extension.ToLower())
                 || ".gif".Equals(fileInfo.Extension.ToLower())
                 || ".png".Equals(fileInfo.Extension.ToLower())
                 || ".jpeg".Equals(fileInfo.Extension.ToLower()))
            {
                if (File.Exists(fileInfo.FullName))
                {
                    var uri = new Uri(fileInfo.FullName);
                    img.Source = new BitmapImage(uri);
                    border.MouseLeftButtonUp += btnShowImage_Click;
                }
                else
                {
                    img.Height = 36;
                    img.Width = 36;
                    img.Source = ImageUtil.GetImageFromResource(typeof(ResourceImage).FullName, "image");
                }
            }
            else
            {
                img.Height = 36;
                img.Width = 36;
                img.Source = ImageUtil.GetImageFromResource(typeof(ResourceImage).FullName, "file");
                border.MouseLeftButtonUp += btnDownloadFile_Click;
            }
            border.Child = img;

            body.Children.Insert(body.Children.Count - 1, border);
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as AttachmentMenuItem;
            DeletedFiles.Add(item._FileInfo.FullName, item._FileInfo);
            _AttachmentList.RemoveAt(item._index);
            _SetAttachmentList(_AttachmentList);
        }

        /// <summary>
        /// 另存附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as AttachmentMenuItem;

            var op = new System.Windows.Forms.FolderBrowserDialog();
            var result = op.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                File.Copy(item._FileInfo.FullName, op.SelectedPath + "\\" + item._FileInfo.Name);
            }
        }

        /// <summary>
        /// 弹出图片查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowImage_Click(object sender, RoutedEventArgs e)
        {
            var attachment = sender as AttachmentItem;
            var form = new ImageForm(attachment._FileInfo.FullName);
            FormCommon.ShowForm("", form);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            var attachment = sender as AttachmentItem;
            if (!File.Exists(attachment._FileInfo.FullName))
            {
                FormCommon.ShowErr("文件：" + attachment._FileInfo.FullName + "不存在！");
                return;
            }

            var op = new System.Windows.Forms.FolderBrowserDialog();
            var result = op.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                File.Copy(attachment._FileInfo.FullName, op.SelectedPath + "\\" + attachment._FileInfo.Name);
            }
        }

        private void btnAdd_MouseMove(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            border.Background = CommonUtil.ToBrush("#E0EEEE");
        }

        private void btnAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            border.Background = CommonUtil.ToBrush("#FFFFFF");
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent BeforeAttarchUploadRoutedEvent = EventManager.RegisterRoutedEvent(
            "_BeforeAttarchUpload", RoutingStrategy.Bubble, typeof(EventHandler<MyUploadEventArge>), typeof(MyUploadButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _BeforeAttarchUpload
        {
            add { base.AddHandler(BeforeAttarchUploadRoutedEvent, value); }
            remove { base.RemoveHandler(BeforeAttarchUploadRoutedEvent, value); }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent AftervUploadRoutedEvent = EventManager.RegisterRoutedEvent(
            "_AfterAttarchUpload", RoutingStrategy.Bubble, typeof(EventHandler<MyUploadEventArge>), typeof(MyUploadButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _AfterAttarchUpload
        {
            add { base.AddHandler(AftervUploadRoutedEvent, value); }
            remove { base.RemoveHandler(AftervUploadRoutedEvent, value); }
        }

        private class AttachmentMenuItem : MenuItem
        {
            public int _index { get; set; }
            public FileInfo _FileInfo { get; set; }
        }

        private class AttachmentItem : Border
        {
            public int _index { get; set; }
            public FileInfo _FileInfo { get; set; }
        }
    }
}
