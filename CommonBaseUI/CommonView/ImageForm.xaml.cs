using CommonBaseUI.Common;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CommonBaseUI.CommonView
{
    /// <summary>
    /// ImageForm.xaml 的交互逻辑
    /// </summary>
    public partial class ImageForm : UserControl
    {
        public ImageForm()
        {
            InitializeComponent();
        }
        public ImageForm(string path)
        {
            this.Height = 700;
            this.Width = 800;
            InitializeComponent();
            path = "\\" + path.Substring(1);

            if (File.Exists(path))
            {
                var uri = new Uri(path);
                //strImagePath 就绝对路径
                try
                {
                    img.Source = new BitmapImage(uri);
                    FormCommon.ShowForm("", this);
                }
                catch (Exception e)
                {
                    if (e is NotSupportedException)
                    {
                        FormCommon.ShowErr("图片格式错误！");
                    }
                    else
                    {
                        FormCommon.ShowErr(e.Message);
                    }
                }
            }
            else
            {
                FormCommon.ShowErr("图片文件不存在！");
            }
        }
    }
}
