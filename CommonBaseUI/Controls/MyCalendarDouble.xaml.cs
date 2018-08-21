using CommonBaseUI.Common;
using System.Windows;
using System.Windows.Controls;
using CommonBaseUI.CommUtil;
using System;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyCalendarDouble.xaml 的交互逻辑
    /// </summary>
    public partial class MyCalendarDouble : UserControl
    {
        private string DateFrom = "";
        private string DateTo = "";
        private MyDatePickerRange.DateRangeModel DateRange;
        public MyCalendarDouble()
        {
            this.Height = 367;
            this.Width = 650;
            InitializeComponent();
        }

        public void _Init(MyDatePickerRange.DateRangeModel date)
        {
            calendar1._Init(date.DateFrom, false);
            calendar2._Init(date.DateTo, false);

            this.DateRange = date;
            this.DateFrom = string.Format("{0}-{1}-{2}", date.DateFrom.Year, date.DateFrom.Month.ToString().PadLeft(2, '0'), date.DateFrom.Day.ToString().PadLeft(2, '0'));
            this.DateFrom = this.DateFrom.ToDateTime().Equals(DateTime.MinValue) ? "" : this.DateFrom;
            this.DateTo = string.Format("{0}-{1}-{2}", date.DateTo.Year, date.DateTo.Month.ToString().PadLeft(2, '0'), date.DateTo.Day.ToString().PadLeft(2, '0'));
            this.DateTo = this.DateTo.ToDateTime().Equals(DateTime.MinValue) ? "" : this.DateTo;
            SetDateDisp();
        }

        /// <summary>
        /// 选中开始日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calendar1_CalendarDateSelect(object sender, CalendarDateSelectEventArge e)
        {
            this.DateFrom = string.Format("{0}-{1}-{2}", e._Year, e._Month.ToString().PadLeft(2, '0'), e._Day.ToString().PadLeft(2, '0'));
            SetDateDisp();
        }

        /// <summary>
        /// 选中结束日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calendar2_CalendarDateSelect(object sender, CalendarDateSelectEventArge e)
        {
            this.DateTo = string.Format("{0}-{1}-{2}", e._Year, e._Month.ToString().PadLeft(2, '0'), e._Day.ToString().PadLeft(2, '0'));
            SetDateDisp();
        }

        /// <summary>
        /// 设置日期显示
        /// </summary>
        private void SetDateDisp()
        {
            lblDateRange.Content = DateFrom + "--" + DateTo;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var dateFrom = this.DateFrom.ToDateTime();
            if (!dateFrom.Equals(DateTime.MinValue))
            {
                DateRange.DateFrom.Year = dateFrom.Year;
                DateRange.DateFrom.Month = dateFrom.Month;
                DateRange.DateFrom.Day = dateFrom.Day;
            }

            var dateTo = this.DateTo.ToDateTime();
            DateRange.DateTo.Year = dateTo.Year;
            DateRange.DateTo.Month = dateTo.Month;
            DateRange.DateTo.Day = dateTo.Day;

            FormCommon.CloseForm(this);
        }
    }
}
