using CommonBaseUI.Common;
using CommonBaseUI.Model;
using CommonUtils;
using System;
using System.Windows;
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
            this.Loaded += MyDatePicker_Loaded;
        }

        public object _Value
        {
            get
            {
                if (txtInput.Text.Trim().IsNullOrEmpty())
                {
                    return null;
                }
                if (_Mode == DateMode.Year)
                {
                    if (DateTime.MinValue.Equals((txtInput.Text + "-01-01").ToDateTime()))
                    {
                        return null;
                    }
                    return txtInput.Text;
                }
                else if (_Mode == DateMode.Month)
                {
                    if (DateTime.MinValue.Equals((txtInput.Text + "-01").ToDateTime()))
                    {
                        return null;
                    }
                    return txtInput.Text;
                }
                else
                {
                    if (DateTime.MinValue.Equals(txtInput.Text.ToDateTime()))
                    {
                        return null;
                    }
                    return txtInput.Text;
                }
            }
            set
            {
                var ar = new ValueChangedEventArge(DatePickerValueChangeEvent, this);
                ar._ValueBeforeChange = txtInput.Text;

                if (value.ToStr().IsNullOrEmpty() || DateTime.MinValue.Equals(value.ToDateTime()))
                {
                    txtInput.Text = string.Empty;
                    ar._Value = txtInput.Text;
                    if (!ar._ValueBeforeChange.Equals(ar._Value))
                    {
                        RaiseEvent(ar);
                    }
                    return;
                }
                if (_Mode == DateMode.Year)
                {
                    if (value is DateTime)
                    {
                        txtInput.Text = value.ToDateTime().ToString("yyyy");
                    }
                    else if (value is string)
                    {
                        if (value.ToStr().Length == 10)
                        {
                            txtInput.Text = value.ToDateTime().ToString("yyyy");
                        }
                        else if (value.ToStr().Length == 7)
                        {
                            txtInput.Text = (value.ToStr() + "-01").ToDateTime().ToString("yyyy");
                        }
                        else if (value.ToStr().Length == 4)
                        {
                            txtInput.Text = (value.ToStr() + "-01-01").ToDateTime().ToString("yyyy");
                        }
                    }
                }
                else if (_Mode == DateMode.Month)
                {
                    if (value is DateTime)
                    {
                        txtInput.Text = value.ToDateTime().ToString("yyyy-MM");
                    }
                    else if (value is string)
                    {
                        if (value.ToStr().Length == 10)
                        {
                            txtInput.Text = value.ToDateTime().ToString("yyyy-MM");
                        }
                        else if (value.ToStr().Length == 7)
                        {
                            txtInput.Text = (value.ToStr() + "-01").ToDateTime().ToString("yyyy-MM");
                        }
                    }
                }
                else
                {
                    txtInput.Text = value.ToDateTime().ToString("yyyy-MM-dd");
                }
                ar._Value = txtInput.Text;
                if (!ar._ValueBeforeChange.Equals(ar._Value))
                {
                    RaiseEvent(ar);
                }
            }
        }

        public int _Year
        {
            get
            {
                if (txtInput.Text.Trim().IsNullOrEmpty())
                {
                    return 0;
                }
                if (_Mode == DateMode.Date)
                {
                    var date = txtInput.Text.ToDateTime();
                    if (DateTime.MinValue.Equals(txtInput.Text.ToDateTime()))
                    {
                        return 0;
                    }
                    return date.Year;
                }
                else
                {
                    var date = (txtInput.Text + "-01").ToDateTime();
                    if (DateTime.MinValue.Equals(date))
                    {
                        return 0;
                    }
                    return date.Year;
                }
            }
        }

        public int _Month
        {
            get
            {
                if (txtInput.Text.Trim().IsNullOrEmpty())
                {
                    return 0;
                }
                if (_Mode == DateMode.Date)
                {
                    var date = txtInput.Text.ToDateTime();
                    if (DateTime.MinValue.Equals(txtInput.Text.ToDateTime()))
                    {
                        return 0;
                    }
                    return date.Month;
                }
                else
                {
                    var date = (txtInput.Text + "-01").ToDateTime();
                    if (DateTime.MinValue.Equals(date))
                    {
                        return 0;
                    }
                    return date.Month;
                }
            }
        }

        public int _Day
        {
            get
            {
                if (txtInput.Text.Trim().IsNullOrEmpty())
                {
                    return 0;
                }
                if (_Mode == DateMode.Date)
                {
                    var date = txtInput.Text.ToDateTime();
                    if (DateTime.MinValue.Equals(txtInput.Text.ToDateTime()))
                    {
                        return 0;
                    }
                    return date.Day;
                }
                else
                {
                    return 0;
                }
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
                txtInput.Width = value;
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
                if (value == DateMode.Year)
                {
                    _InputWidth = 40;
                }
                else if (value == DateMode.Month)
                {
                    _InputWidth = 60;
                }
                else
                {
                    _InputWidth = 80;
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

        private void MyDatePicker_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_InitDefautValue)
            {
                _Value = DateTime.Now;
            }
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
            form._Mode = this._Mode;
            form._Init(model);
            form._CalendarMonthSelect += MonthSelect;
            form._CalendarDateSelect += DaySelect;
            form._CalendarYearSelect += YearSelect;
            FormCommon.ShowForm("", form);
        }

        /// <summary>
        /// 选择月份的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthSelect(object sender, System.Windows.RoutedEventArgs e)
        {
            var args = e as CalendarDateSelectEventArge;
            if (_Mode == DateMode.Month)
            {
                var ar = new ValueChangedEventArge(DatePickerValueChangeEvent, this);
                ar._ValueBeforeChange = _Value;
                _Value = new DateTime(args._Year, args._Month, 1);
                ar._Value = _Value;

                if (ar._ValueBeforeChange == null && ar._Value != null)
                {
                    RaiseEvent(ar);
                    return;
                }
                if (!ar._ValueBeforeChange.Equals(ar._Value))
                {
                    RaiseEvent(ar);
                    return;
                }
            }
        }

        /// <summary>
        /// 选择日期的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaySelect(object sender, System.Windows.RoutedEventArgs e)
        {
            var args = e as CalendarDateSelectEventArge;
            if (_Mode == DateMode.Date)
            {
                var ar = new ValueChangedEventArge(DatePickerValueChangeEvent, this);
                ar._ValueBeforeChange = _Value;
                _Value = new DateTime(args._Year, args._Month, args._Day);
                ar._Value = _Value;

                if (ar._ValueBeforeChange == null && ar._Value != null)
                {
                    RaiseEvent(ar);
                    return;
                }
                if (!ar._ValueBeforeChange.Equals(ar._Value))
                {
                    RaiseEvent(ar);
                    return;
                }
            }
        }

        /// <summary>
        /// 选择日期的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearSelect(object sender, System.Windows.RoutedEventArgs e)
        {
            var args = e as CalendarDateSelectEventArge;
            if (_Mode == DateMode.Year)
            {
                var ar = new ValueChangedEventArge(DatePickerValueChangeEvent, this);
                ar._ValueBeforeChange = _Value;
                _Value = new DateTime(args._Year, 1, 1);
                ar._Value = _Value;

                if (ar._ValueBeforeChange == null && ar._Value != null)
                {
                    RaiseEvent(ar);
                    return;
                }
                if (!ar._ValueBeforeChange.Equals(ar._Value))
                {
                    RaiseEvent(ar);
                    return;
                }
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent DatePickerValueChangeEvent = EventManager.RegisterRoutedEvent(
            "_ValueChanged", RoutingStrategy.Bubble, typeof(EventHandler<ValueChangedEventArge>), typeof(MyDatePicker));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _ValueChanged
        {
            add { base.AddHandler(DatePickerValueChangeEvent, value); }
            remove { base.RemoveHandler(DatePickerValueChangeEvent, value); }
        }

        public class DateModel
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
        }
    }

    /// <summary>
    /// 选择模式
    /// </summary>
    public enum DateMode
    {
        Month = 1,
        Date = 2,
        Year = 3
    }
}
