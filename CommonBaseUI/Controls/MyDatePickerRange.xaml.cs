using CommonBaseUI.Common;
using CommonUtils;
using System;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyDatePickerRange.xaml 的交互逻辑
    /// </summary>
    public partial class MyDatePickerRange : UserControl, IInputControl
    {
        public MyDatePickerRange()
        {
            InitializeComponent();
            this.Loaded += MyDatePicker_Loaded;
        }

        public object _Value
        {
            get
            {
                if (datePicker1.Text.Trim().IsNullOrEmpty())
                {
                    return null;
                }
                if (_Mode == DateMode.Date)
                {
                    if (DateTime.MinValue.Equals(datePicker1.Text.ToDateTime()))
                    {
                        return null;
                    }
                    return datePicker1.Text;
                }
                else
                {
                    if (DateTime.MinValue.Equals((datePicker1.Text + "-01").ToDateTime()))
                    {
                        return null;
                    }
                    return datePicker1.Text;
                }
            }
            set
            {
                if (value.ToStr().IsNullOrEmpty())
                {
                    datePicker1.Text = string.Empty;
                    return;
                }
                if (DateTime.MinValue.Equals(value.ToDateTime()))
                {
                    datePicker1.Text = string.Empty;
                    return;
                }
                if (_Mode == DateMode.Date)
                {
                    datePicker1.Text = value.ToDateTime().ToString("yyyy-MM-dd");
                }
                else
                {
                    if (value is DateTime)
                    {
                        datePicker1.Text = value.ToDateTime().ToString("yyyy-MM");
                    }
                    else if (value is string)
                    {
                        if (value.ToStr().Length == 10)
                        {
                            datePicker1.Text = value.ToDateTime().ToString("yyyy-MM");
                        }
                        else if (value.ToStr().Length == 7)
                        {
                            datePicker1.Text = (value.ToStr() + "-01").ToDateTime().ToString("yyyy-MM");
                        }
                    }
                }
            }
        }

        public object _Value2
        {
            get
            {
                if (datePicker2.Text.Trim().IsNullOrEmpty())
                {
                    return null;
                }
                if (_Mode == DateMode.Date)
                {
                    if (DateTime.MinValue.Equals(datePicker2.Text.ToDateTime()))
                    {
                        return null;
                    }
                    return datePicker2.Text;
                }
                else
                {
                    if (DateTime.MinValue.Equals((datePicker2.Text + "-01").ToDateTime()))
                    {
                        return null;
                    }
                    return datePicker2.Text;
                }
            }
            set
            {
                if (value.ToStr().IsNullOrEmpty())
                {
                    datePicker2.Text = string.Empty;
                    return;
                }
                if (DateTime.MinValue.Equals(value.ToDateTime()))
                {
                    datePicker2.Text = string.Empty;
                    return;
                }
                if (_Mode == DateMode.Date)
                {
                    datePicker2.Text = value.ToDateTime().ToString("yyyy-MM-dd");
                }
                else
                {
                    if (value is DateTime)
                    {
                        datePicker2.Text = value.ToDateTime().ToString("yyyy-MM");
                    }
                    else if (value is string)
                    {
                        if (value.ToStr().Length == 10)
                        {
                            datePicker2.Text = value.ToDateTime().ToString("yyyy-MM");
                        }
                        else if (value.ToStr().Length == 7)
                        {
                            datePicker2.Text = (value.ToStr() + "-01").ToDateTime().ToString("yyyy-MM");
                        }
                    }
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
        public string _Binding2 { get; set; }

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
                return datePicker1.Width;
            }
            set
            {
                datePicker1.Width = value;
                datePicker2.Width = value;
            }
        }

        /// <summary>
        /// 是否初始化为当天日期
        /// </summary>
        public bool _InitDefautValue { get; set; }

        private DateMode mode = DateMode.Date;
        public DateMode _Mode
        {

            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

        /// <summary>
        /// 设置输入框背景色
        /// </summary>
        public void _SetErr()
        {
            this.datePicker1.Background = CommonUtil.ToBrush("#FA8072");
            this.datePicker1.Foreground = CommonUtil.ToBrush("#FFFFFF");
            this.datePicker2.Background = CommonUtil.ToBrush("#FA8072");
            this.datePicker2.Foreground = CommonUtil.ToBrush("#FFFFFF");
        }

        /// <summary>
        /// 清除输入框背景色
        /// </summary>
        public void _CleanErr()
        {
            this.datePicker1.Background = CommonUtil.ToBrush("#FFFFFF");
            this.datePicker1.Foreground = CommonUtil.ToBrush("#000000");
            this.datePicker2.Background = CommonUtil.ToBrush("#FFFFFF");
            this.datePicker2.Foreground = CommonUtil.ToBrush("#000000");
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
                btnClear1.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                btnClear1.Width = value ? 20 : 0;
                btnClear2.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                btnClear2.Width = value ? 20 : 0;
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void _Clear()
        {
            _Value = string.Empty;
            _Value2 = string.Empty;
        }

        private void MyDatePicker_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_InitDefautValue)
            {
                _Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                _Value2 = DateTime.Now;
            }
        }

        /// <summary>
        /// 清除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearFrom_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _Value = string.Empty;
        }

        /// <summary>
        /// 清除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearTo_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _Value2 = string.Empty;
        }

        private void btnCalendar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var modelFrom = new MyDatePicker.DateModel();
            var dateFrom = _Value.ToDateTime();
            dateFrom = dateFrom.Equals(DateTime.MinValue) ? DateTime.Now : dateFrom;
            modelFrom.Year = dateFrom.Year;
            modelFrom.Month = dateFrom.Month;
            modelFrom.Day = dateFrom.Day;

            var modelTo = new MyDatePicker.DateModel();
            var dateTo = _Value2.ToDateTime();
            dateTo = dateTo.Equals(DateTime.MinValue) ? DateTime.Now : dateTo;
            modelTo.Year = dateTo.Year;
            modelTo.Month = dateTo.Month;
            modelTo.Day = dateTo.Day;

            var model = new DateRangeModel();
            model.DateFrom = modelFrom;
            model.DateTo = modelTo;

            var form = new MyCalendarDouble();
            form._Mode = _Mode;
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
                var model = item as DateRangeModel;
                _Value = new DateTime(model.DateFrom.Year, model.DateFrom.Month, model.DateFrom.Day);
                _Value2 = new DateTime(model.DateTo.Year, model.DateTo.Month, model.DateTo.Day);
            }
        }

        public class DateRangeModel
        {
            public MyDatePicker.DateModel DateFrom { get; set; }
            public MyDatePicker.DateModel DateTo { get; set; }
        }
    }
}
