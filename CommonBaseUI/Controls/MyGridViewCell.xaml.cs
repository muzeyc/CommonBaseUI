using CommonBaseUI.CommUtil;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyGridViewCell.xaml 的交互逻辑
    /// </summary>
    public partial class MyGridViewCell : UserControl, IInputControl
    {
        public MyGridViewCell()
        {
            InitializeComponent();
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
                return this.IsEnabled;
            }
            set
            {
                this.IsEnabled = value;
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
                return txtInput.Width;
            }
            set
            {
                txtInput.Width = value;
            }
        }

        public void _SetErr()
        {
            this.txtInput.Background = CommonUtil.ToBrush("#FA8072");
            this.txtInput.Foreground = CommonUtil.ToBrush("#FFFFFF");
        }

        public void _CleanErr()
        {
            this.txtInput.Background = CommonUtil.ToBrush("#FFFFFF");
            this.txtInput.Foreground = CommonUtil.ToBrush("#000000");
        }
    }
}
