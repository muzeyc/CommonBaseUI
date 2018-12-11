using CommonUtils;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyProgress.xaml 的交互逻辑
    /// </summary>
    public partial class MyProgress : UserControl, IInputControl
    {
        public MyProgress()
        {
            InitializeComponent();
        }

        private object _value = 0;
        public object _Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                decimal v = value.ToDec() * 100;
                v = v >= 0 ? v : 0;
                v = v > 100 ? 100 : v;
                lblPercentage.Text =  v + "%";
                if (v >= 100)
                {
                    borInput.Background = lblInput.Background;
                    lblInput.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (v >= 0 && v < 100)
                {
                    lblInput.Width = grdFull.Width * value.ToDouble();
                    borInput.Background = CommonUtil.ToBrush("#FFFFFF");
                    lblInput.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    borInput.Background = CommonUtil.ToBrush("#FFFFFF");
                    lblInput.Visibility = System.Windows.Visibility.Collapsed;
                }
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

                    var border = splBorder.Children[0] as Border;
                    border.CornerRadius = new System.Windows.CornerRadius(3);
                }
            }
        }

        public bool _MustInput { get; set; }

        public string _Binding { get; set; }

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
                return grdFull.Width;
            }
            set
            {
                grdFull.Width = value;
            }
        }

        public void _SetErr()
        {
            return;
        }

        public void _CleanErr()
        {
            return;
        }

        public bool _IsEnabled { get; set; }

        #region 自定义属性


        #endregion
    }
}
