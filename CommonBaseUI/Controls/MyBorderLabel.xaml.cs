using System.Windows.Controls;
using CommonUtils;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyBorderLabel.xaml 的交互逻辑
    /// </summary>
    public partial class MyBorderLabel : UserControl, IInputControl
    {
        public MyBorderLabel()
        {
            InitializeComponent();
            lblContent.TextWrapping = System.Windows.TextWrapping.Wrap;
        }

        private object val = null;
        public object _Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                lblContent.Text = val.ToStr();
            }
        }

        public string _Caption { get; set; }
        public bool _MustInput { get; set; }
        public string _Binding { get; set; }
        public double _CaptionWidth { get; set; }
        public double _InputWidth
        {
            get
            {
                return pnlBody.Width;
            }
            set
            {
                pnlBody.Width = value;
            }
        }

        public double _InputHeight
        {
            get
            {
                return pnlBody.Height;
            }
            set
            {
                pnlBody.Height = value;
            }
        }

        public bool _IsEnabled { get; set; }

        public void _SetErr()
        {
            return;
        }

        public void _CleanErr()
        {
            return;
        }

        private string background = "#FFFFFF";
        public string _Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                pnlBorder.Background = CommonUtils.CommonUtil.ToBrush(background);
            }
        }

        public System.Windows.HorizontalAlignment _HorizontalContentAlignment
        {
            get
            {
                return lblContent.HorizontalAlignment;
            }
            set
            {
                lblContent.HorizontalAlignment = value;
            }
        }

        public System.Windows.VerticalAlignment _VerticalContentAlignment
        {
            get
            {
                return lblContent.VerticalAlignment;
            }
            set
            {
                lblContent.VerticalAlignment = value;
            }
        }
    }
}
