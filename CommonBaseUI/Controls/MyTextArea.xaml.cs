using CommonBaseUI.CommUtil;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyTextArea.xaml 的交互逻辑
    /// </summary>
    public partial class MyTextArea : UserControl, IInputControl
    {
        public MyTextArea()
        {
            InitializeComponent();
            brdInput.Width = _InputWidth + 4;
        }

        private object val = null;
        public object _Value
        {
            get
            {
                return txtInput.Text.IsNullOrEmpty() ? null : txtInput.Text;
            }
            set
            {
                val = value;
                txtInput.Text = value.ToStr();
            }
        }

        public bool _IsEnabled
        {
            get
            {
                return !txtInput.IsReadOnly;
            }
            set
            {
                txtInput.Background = value ? CommonUtil.ToBrush("#fff") : CommonUtil.ToBrush("#EBEBEB");
                txtInput.IsReadOnly = !value;
            }
        }

        public string _Caption
        {
            get
            {
                return lblCaption.Content.ToStr();
            }
            set
            {
                lblCaption.Content = value;
                if (lblCaption.Content.ToStr().IsNullOrEmpty())
                {
                    splBorder.Children.RemoveAt(0);
                }
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
                //if (mustInput)
                //{
                //    var color = ColorTranslator.FromHtml("#FF0000");
                //    SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));

                //    this.lblCaption.Foreground = (System.Windows.Media.Brush)myBrush;
                //}
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
            this.txtInput.Background = CommonUtil.ToBrush("#FA8072");
            this.txtInput.Foreground = CommonUtil.ToBrush("#FFFFFF");
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
                brdInput.Width = value + 4;
            }
        }

        public double _InputHeight
        {
            get
            {
                return txtInput.Height;
            }
            set
            {
                txtInput.Height = value;
                brdInput.Height = value + 4;
            }
        }

        /// <summary>
        /// 清除输入框背景色
        /// </summary>
        public void _CleanErr()
        {
            this.txtInput.Background = CommonUtil.ToBrush("#FFFFFF");
            this.txtInput.Foreground = CommonUtil.ToBrush("#000000");
        }
    }
}
