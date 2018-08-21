using CommonBaseUI.CommUtil;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyPasswordBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyPasswordBox : UserControl, IInputControl
    {
        public MyPasswordBox()
        {
            InitializeComponent();
        }

        private object val;
        public object _Value
        {
            get { return txtInput.Password; }
            set
            {
                val = value;
            }
        }
        public string _Password
        {
            get
            {
                return txtInput.Password;
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
                btnClear.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                btnClear.Width = value ? 20 : 0;
                if (!_IsEnabled)
                {
                    txtInput.Width = txtInput.Width + 18;
                }
                txtInput.Margin = value ? new Thickness(2, 2, 20, 2) : new Thickness(2);
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
                //    var color = ColorTranslator.FromHtml("#EE7600");
                //    SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));

                //    var colorW = ColorTranslator.FromHtml("#FFFFFF");
                //    SolidColorBrush myBrushW = new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorW.R, colorW.G, colorW.B));

                //    this.lblCaption.Background = (System.Windows.Media.Brush)myBrush;
                //    this.lblCaption.Foreground = (System.Windows.Media.Brush)myBrushW;
                //    var border = lblCaption.Parent as Border;
                //    border.BorderBrush = (System.Windows.Media.Brush)myBrush;
                //    border.Background = (System.Windows.Media.Brush)myBrush;
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
                if (_IsEnabled)
                {
                    txtInput.Width = value - 20;
                }
                else
                {
                    txtInput.Width = value;
                }
            }
        }

        private CommonBaseUI.Controls.MyNumberBox.DataType dataType = CommonBaseUI.Controls.MyNumberBox.DataType.Text;
        public CommonBaseUI.Controls.MyNumberBox.DataType _DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
                if (dataType != CommonBaseUI.Controls.MyNumberBox.DataType.Text)
                {
                    InputMethod.SetIsInputMethodEnabled(txtInput, false);
                    txtInput.PreviewKeyDown -= txtInput_KeyDown;
                    txtInput.PreviewKeyDown += txtInput_KeyDown;
                }
                else
                {
                    InputMethod.SetIsInputMethodEnabled(txtInput, true);
                    txtInput.PreviewKeyDown -= txtInput_KeyDown;
                }
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

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                 (e.Key >= Key.D0 && e.Key <= Key.D9) ||
                 e.Key == Key.Back ||
                 e.Key == Key.Left || e.Key == Key.Right ||
                (dataType == CommonBaseUI.Controls.MyNumberBox.DataType.Decimal && e.Key == Key.OemPeriod))
            {
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
