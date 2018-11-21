using CommonBaseUI.Common;
using CommonUtils;
using System;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyDatePicker.xaml 的交互逻辑
    /// </summary>
    public partial class MyDatePicker : UserControl, IInputControl
    {
        public MyDatePicker()
        {
            InitializeComponent();
            _Value = DateTime.Now;
        }

        public object _Value
        {
            get
            {
                if (txtInput.Text.Trim().IsNullOrEmpty())
                {
                    return null;
                }
                if (DateTime.MinValue.Equals(txtInput.Text.ToDateTime()))
                {
                    return null;
                }
                return txtInput.Text.ToDateTime().ToString("yyyy-MM-dd");
            }
            set
            {
                if (value.ToStr().IsNullOrEmpty())
                {
                    txtInput.Text = string.Empty;
                    return;
                }
                if (DateTime.MinValue.Equals(value.ToDateTime()))
                {
                    txtInput.Text = string.Empty;
                    return;
                }
                txtInput.Text = value.ToDateTime().ToString("yyyy-MM-dd");
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

        private void btnCalendar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var model = new DateModel();
            var date = _Value.ToDateTime();
            date = date.Equals(DateTime.MinValue) ? DateTime.Now : date;
            model.Year = date.Year;
            model.Month = date.Month;
            model.Day = date.Day;
            var form = new MyCalendar();
            form._Init(model);

            FormCommon.ShowForm("", form, model, AfterCloseCallBack);
        }

        /// <summary>
        /// 关闭日历画面的回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isCloseOnly"></param>
        private void AfterCloseCallBack(object item, bool isCloseOnly)
        {
            if (!isCloseOnly)
            {
                var model = item as DateModel;
                _Value = new DateTime(model.Year, model.Month, model.Day);
            }
        }

        public class DateModel
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
        }
    }
}
