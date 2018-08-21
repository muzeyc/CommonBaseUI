using CommonBaseUI.Common;
using CommonBaseUI.CommUtil;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyCalendar.xaml 的交互逻辑
    /// </summary>
    public partial class MyCalendar : UserControl
    {
        private Dictionary<string, int> DicDayOfWeek;
        private Dictionary<int, string> DicMonth;
        private int Year;
        private int Month;
        private MyDatePicker.DateModel Date;
        private int Mode = 1;
        private int Year_Year;
        private int Year_Month;
        private bool SelectDateClose = true;

        public MyCalendar()
        {
            this.Width = 300;
            this.Height = 320;
            InitializeComponent();

            DicDayOfWeek = new Dictionary<string, int>();
            DicDayOfWeek.Add("Monday", 1);
            DicDayOfWeek.Add("Tuesday", 2);
            DicDayOfWeek.Add("Wednesday", 3);
            DicDayOfWeek.Add("Thursday", 4);
            DicDayOfWeek.Add("Friday", 5);
            DicDayOfWeek.Add("Saturday", 6);
            DicDayOfWeek.Add("Sunday", 7);

            DicMonth = new Dictionary<int, string>();
            DicMonth.Add(1, "Jan");
            DicMonth.Add(2, "Feb");
            DicMonth.Add(3, "Mar");
            DicMonth.Add(4, "Apr");
            DicMonth.Add(5, "May");
            DicMonth.Add(6, "Jun");
            DicMonth.Add(7, "Jul");
            DicMonth.Add(8, "Aug");
            DicMonth.Add(9, "Sept");
            DicMonth.Add(10, "Ort");
            DicMonth.Add(11, "Nov");
            DicMonth.Add(12, "Dec");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="date"></param>
        public void _Init(MyDatePicker.DateModel date, bool selectDateClose = true)
        {
            lblDate.Content = string.Format("{0}-{1}", date.Year, date.Month.ToString().PadLeft(2, '0'));
            this.Year = date.Year;
            this.Month = date.Month;
            this.Date = date;
            this.Year_Year = date.Year;
            this.Year_Month = date.Year;
            this.SelectDateClose = selectDateClose;
            SetCalendar(date.Year, date.Month);
            SetMonthCalendar(date.Year);
            SetYearCalendar(date.Year);
        }

        /// <summary>
        /// 设置画面
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void SetCalendar(int year, int month)
        {
            pnlDate.Children.Clear();
            var list = GetDayOfMonth(year, month);

            foreach (var item in list)
            {
                var row = new StackPanel();
                row.Orientation = Orientation.Horizontal;
                row.Margin = new System.Windows.Thickness(10, 0, 10, 0);

                for (int i = 1; i <= 7; i++)
                {
                    var subItem = item[i];
                    var cell = new CalendarCell();
                    cell.Width = 40;
                    cell.Height = 40;
                    cell.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    cell.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                    cell.Content = subItem.Day;
                    cell.Year = subItem.Year;
                    cell.Month = subItem.Month;
                    cell.Day = subItem.Day;
                    cell.Background = CommonUtil.ToBrush("#FFFFFF");
                    if (year == subItem.Year && month == subItem.Month)
                    {
                        cell.Foreground = CommonUtil.ToBrush("#3b3b3b");
                    }
                    else
                    {
                        cell.Foreground = CommonUtil.ToBrush("#CCC");
                    }
                    cell.MouseMove += Cell_MouseMove;
                    cell.MouseLeave += Cell_MouseLeave;
                    cell.MouseLeftButtonUp += btnDay_Click;
                    row.Children.Add(cell);
                }

                pnlDate.Children.Add(row);
            }
            SelectDate();
        }

        /// <summary>
        /// 获取日历列表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="mounth"></param>
        private List<Dictionary<int, DateTime>> GetDayOfMonth(int year, int month)
        {
            int days = DaysInMonth(year, month);

            var list = new List<Dictionary<int, DateTime>>();
            int currentPoint = 0;
            for (int day = 1; day <= days; day++)
            {
                var date = new DateTime(year, month, day);
                int weekDay = DicDayOfWeek[date.DayOfWeek.ToString()];

                if (weekDay == 7)
                {
                    currentPoint = day;
                    var dic = new Dictionary<int, DateTime>();

                    for (int i = 0; i < 7; i++)
                    {
                        if (currentPoint - i > 0)
                        {
                            date = new DateTime(year, month, currentPoint - i);
                        }
                        else
                        {
                            // 计算前个月的日期
                            date = new DateTime(year, month, 1);
                            date = date.AddDays(currentPoint - i - 1);
                        }
                        weekDay = DicDayOfWeek[date.DayOfWeek.ToString()];
                        dic[weekDay] = date;
                    }
                    list.Add(dic);
                }
            }

            if (currentPoint < days)
            {
                var dic = new Dictionary<int, DateTime>();
                for (int i = 1; i <= 7; i++)
                {
                    if (currentPoint + i <= days)
                    {
                        var date = new DateTime(year, month, currentPoint + i);
                        var weekDay = DicDayOfWeek[date.DayOfWeek.ToString()];
                        dic[weekDay] = date;
                    }
                    else
                    {
                        var date = new DateTime(year, month, days);
                        date = date.AddDays(currentPoint + i - days);
                        var weekDay = DicDayOfWeek[date.DayOfWeek.ToString()];
                        dic[weekDay] = date;
                    }
                }
                list.Add(dic);
            }

            return list;
        }

        /// <summary>
        /// 设置月份日历
        /// </summary>
        /// <param name="year"></param>
        private void SetMonthCalendar(int year)
        {
            pnlMonth.Children.Clear();
            for (int i = 0; i < 4; i++)
            {
                var row = new StackPanel();
                row.Orientation = Orientation.Horizontal;
                row.Margin = new System.Windows.Thickness(15, 0, 15, 0);

                for (int j = 1; j <= 3; j++)
                {
                    int month = i * 3 +j;
                    var cell = new CalendarCell();
                    cell.Width = 90;
                    cell.Height = 90;
                    cell.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    cell.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                    cell.Content = string.Format("{0}月\n\r{1}", month, DicMonth[month]);
                    cell.Year = year;
                    cell.Month = month;
                    if (this.Date.Year == year && this.Date.Month == month)
                    {
                        cell.Background = CommonUtil.ToBrush("#FF4169E1");
                        cell.Foreground = CommonUtil.ToBrush("#FFFFFF");
                    }
                    else
                    {
                        cell.Background = CommonUtil.ToBrush("#FFFFFF");
                        cell.Foreground = CommonUtil.ToBrush("#3b3b3b");
                    }
                    cell.MouseMove += Cell_MouseMove;
                    cell.MouseLeave += Cell_MouseLeave;
                    cell.MouseLeftButtonUp += btnMonth_Click;
                    row.Children.Add(cell);
                }

                pnlMonth.Children.Add(row);
            }
        }

        /// <summary>
        /// 设置年日历
        /// </summary>
        /// <param name="year"></param>
        private void SetYearCalendar(int year)
        {
            pnlYear.Children.Clear();
            pnlYear.Children.Add(GetYearRow(year - 4, year - 2));
            pnlYear.Children.Add(GetYearRow(year - 1, year + 1));
            pnlYear.Children.Add(GetYearRow(year + 2, year + 4));
        }

        private StackPanel GetYearRow(int from, int to)
        {
            var row = new StackPanel();
            row.Orientation = Orientation.Horizontal;
            row.Margin = new System.Windows.Thickness(15, 0, 15, 0);

            for (int year = from; year <= to; year++)
            {
                var cell = new CalendarCell();
                cell.Width = 90;
                cell.Height = 90;
                cell.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                cell.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                cell.Content = year;
                cell.Year = year;
                if (this.Date.Year == year)
                {
                    cell.Background = CommonUtil.ToBrush("#FF4169E1");
                    cell.Foreground = CommonUtil.ToBrush("#FFFFFF");
                }
                else
                {
                    cell.Background = CommonUtil.ToBrush("#FFFFFF");
                    cell.Foreground = CommonUtil.ToBrush("#3b3b3b");
                }
                cell.MouseMove += Cell_MouseMove;
                cell.MouseLeave += Cell_MouseLeave;
                cell.MouseLeftButtonUp += btnYear_Click;
                row.Children.Add(cell);
            }

            return row;
        }

        /// <summary>
        /// 判断该单元格是否被选中
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool IsCellSelected(object sender)
        {
            var lbl = sender as Label;
            return "#FF4169E1".Equals(lbl.Background.ToString());
        }

        /// <summary>
        /// 判断该单元格是否被选中
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private bool IsCurrentYearMonth(object sender)
        {
            var lbl = sender as CalendarCell;
            return lbl.Year == this.Year && lbl.Month == this.Month;
        }

        /// <summary>
        /// 选中日期
        /// </summary>
        private void SelectDate()
        {
            foreach (var row in pnlDate.Children)
            {
                var pnl = row as StackPanel;
                foreach (var cell in pnl.Children)
                {
                    var lbl = cell as CalendarCell;

                    if (this.Date.Day == lbl.Day && this.Date.Year == lbl.Year && this.Date.Month == lbl.Month)
                    {
                        lbl.Background = CommonUtil.ToBrush("#FF4169E1");
                        lbl.Foreground = CommonUtil.ToBrush("#FFFFFF");
                    }
                }
            }
        }

        #region 算法

        /// <summary>
        /// 根据年月获得当月天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int DaysInMonth(int year, int month)
        {
            int days = 0;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: days = 31; 
                break;
                case 4:
                case 6:
                case 9:
                case 11: days = 30; break;
                case 2:
                    if ((year % 100 != 0 && year % 4 == 0) || (year % 400 == 0)) days = 29;
                    else days = 28;
                    break;
                default: days = 0; break;
            }
            return days;
        }

        /// <summary>
        /// 日期日历计算年月
        /// </summary>
        /// <param name="addMonth"></param>
        private void SetYearMonthForDay(int addMonth)
        {
            var date = new DateTime(this.Year, this.Month, 1);
            date = date.AddMonths(addMonth);
            this.Year = date.Year;
            this.Month = date.Month;
            lblDate.Content = string.Format("{0}-{1}", this.Year, this.Month.ToString().PadLeft(2, '0'));
            SetCalendar(this.Year, this.Month);
        }

        /// <summary>
        /// 月日历计算年月
        /// </summary>
        /// <param name="addMonth"></param>
        private void SetYearMonthForMonth(int addYear)
        {
            this.Year_Month += addYear;
            lblDate.Content = this.Year_Month;
            SetMonthCalendar(this.Year_Month);
        }

        /// <summary>
        /// 年日历计算年月
        /// </summary>
        /// <param name="addMonth"></param>
        private void SetYearMonthForYear(int addYear)
        {
            this.Year_Year += addYear;
            lblDate.Content = string.Format("{0}-{1}", this.Year_Year - 4, this.Year_Year + 4);
            SetYearCalendar(this.Year_Year);
        }

        #endregion

        private void Cell_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!IsCellSelected(sender))
            {
                var lbl = sender as Label;

                lbl.Background = CommonUtil.ToBrush("#F08080");
                lbl.Foreground = CommonUtil.ToBrush("#FFFFFF");
            }
        }

        private void Cell_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!IsCellSelected(sender))
            {
                var lbl = sender as Label;

                lbl.Background = CommonUtil.ToBrush("#FFFFFF");

                if (IsCurrentYearMonth(sender))
                {
                    lbl.Foreground = CommonUtil.ToBrush("#3B3B3B");
                }
                else
                {
                    lbl.Foreground = CommonUtil.ToBrush("#CCC");
                }
            }
        }

        private void btnPre_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Mode == 1)
            {
                SetYearMonthForDay(-1);
            }
            else if (this.Mode == 2)
            {
                SetYearMonthForMonth(-1);
            }
            else if (this.Mode == 3)
            {
                SetYearMonthForYear(-9);
            }
        }

        private void btnNext_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Mode == 1)
            {
            SetYearMonthForDay(1);
            }
            else if (this.Mode == 2)
            {
                SetYearMonthForMonth(1);
            }
            else if (this.Mode == 3)
            {
                SetYearMonthForYear(9);
            }
        }

        /// <summary>
        /// 选中日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDay_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cell = sender as CalendarCell;
            this.Date.Year = cell.Year;
            this.Date.Month = cell.Month;
            this.Date.Day = cell.Day;
            foreach (var row in pnlDate.Children)
            {
                var pnl = row as StackPanel;
                foreach (var cel in pnl.Children)
                {
                    var lbl = cel as CalendarCell;
                    lbl.Background = CommonUtil.ToBrush("#FFFFFF");
                    if (lbl.Year == this.Year && lbl.Month == this.Month)
                    {
                        lbl.Foreground = CommonUtil.ToBrush("#3b3b3b");
                    }
                    else
                    {
                        lbl.Foreground = CommonUtil.ToBrush("#CCC");
                    }
                }
            }

            var col = sender as Label;

            col.Background = CommonUtil.ToBrush("#FF4169E1");
            col.Foreground = CommonUtil.ToBrush("#FFFFFF");

            var arge = new CalendarDateSelectEventArge(CalendarDateSelectRoutedEvent, this);
            arge._Year = this.Date.Year;
            arge._Month = this.Date.Month;
            arge._Day = this.Date.Day;
            RaiseEvent(arge);

            if (this.SelectDateClose)
            {
                FormCommon.CloseForm(this);
            }
        }

        /// <summary>
        /// 选中月份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonth_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cell = sender as CalendarCell;
            this.Year = cell.Year;
            this.Month = cell.Month;
            this.Year_Month = cell.Year;
            this.Year_Year = cell.Year;
            foreach (var row in pnlMonth.Children)
            {
                var pnl = row as StackPanel;
                foreach (var cel in pnl.Children)
                {
                    var lbl = cel as Label;
                    lbl.Background = CommonUtil.ToBrush("#FFFFFF");
                    lbl.Foreground = CommonUtil.ToBrush("#3b3b3b");
                }
            }

            var col = sender as Label;

            col.Background = CommonUtil.ToBrush("#FF4169E1");
            col.Foreground = CommonUtil.ToBrush("#FFFFFF");

            var arge = new CalendarDateSelectEventArge(CalendarMonthSelectRoutedEvent, this);
            arge._Year = this.Date.Year;
            arge._Month = this.Date.Month;
            arge._Day = 0;
            RaiseEvent(arge);

            this.Mode = 1;
            SetByMode();
        }

        /// <summary>
        /// 选中年
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYear_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cell = sender as CalendarCell;
            this.Year = cell.Year;
            this.Year_Month = cell.Year;
            this.Year_Year = cell.Year;
            foreach (var row in pnlYear.Children)
            {
                var pnl = row as StackPanel;
                foreach (var cel in pnl.Children)
                {
                    var lbl = cel as Label;
                    lbl.Background = CommonUtil.ToBrush("#FFFFFF");
                    lbl.Foreground = CommonUtil.ToBrush("#3b3b3b");
                }
            }

            var col = sender as Label;

            col.Background = CommonUtil.ToBrush("#FF4169E1");
            col.Foreground = CommonUtil.ToBrush("#FFFFFF");

            this.Mode = 2;
            SetByMode();
        }

        private class CalendarCell : Label
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
        }

        /// <summary>
        /// 切换模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModeChange_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Mode++;
            if (this.Mode > 3)
            {
                this.Mode = 1;
            }

            SetByMode();
        }

        /// <summary>
        /// 通过模式显示
        /// </summary>
        private void SetByMode()
        {
            if (this.Mode == 1)
            {
                lblDate.Content = string.Format("{0}-{1}", this.Year, this.Month.ToString().PadLeft(2, '0'));
                if (pnlMonth.Height > 0)
                {
                    ShowOrHideChild(pnlMonth, 290, 0);
                }
                else if (pnlYear.Height > 0)
                {
                    ShowOrHideChild(pnlYear, 290, 0);
                }
                SetCalendar(this.Year, this.Month);
                ShowOrHideChild(pnlDay, 0, 290);
            }
            else if (this.Mode == 2)
            {
                lblDate.Content = this.Year;
                if (pnlDay.Height > 0)
                {
                    ShowOrHideChild(pnlDay, 290, 0);
                }
                else if (pnlYear.Height > 0)
                {
                    ShowOrHideChild(pnlYear, 290, 0);
                }
                SetMonthCalendar(this.Year);
                ShowOrHideChild(pnlMonth, 0, 290);
            }
            else if (this.Mode == 3)
            {
                lblDate.Content = string.Format("{0}-{1}", this.Year - 4, this.Year + 4);
                SetYearCalendar(this.Year);
                ShowOrHideChild(pnlYear, 0, 290);
                ShowOrHideChild(pnlMonth, 290, 0);
            }
        }

        /// <summary>
        /// 展开/折叠
        /// </summary>
        /// <param name="subMenuList"></param>
        private void ShowOrHideChild(StackPanel subMenuList, double from, double to)
        {
            //实例化一个DoubleAninmation对象
            DoubleAnimation myani = new DoubleAnimation();
            //开始值
            myani.From = from;
            //结束值
            myani.To = to;
            //所用时间
            myani.Duration = TimeSpan.FromMilliseconds(300);
            //设置应用的对象
            Storyboard.SetTarget(myani, subMenuList);
            //设置应用的依赖项属性
            Storyboard.SetTargetProperty(myani, new PropertyPath("Height"));
            // 实例化一个故事板
            Storyboard s = new Storyboard();
            //s.Completed += new EventHandler(ShowChildConmplete);
            //将先前动画添加进来
            s.Children.Add(myani);
            //启动故事版
            s.Begin();
        }

        #region 自定义事件
        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CalendarDateSelectRoutedEvent = EventManager.RegisterRoutedEvent(
            "_CalendarDateSelect", RoutingStrategy.Bubble, typeof(EventHandler<CalendarDateSelectEventArge>), typeof(MyCalendar));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _CalendarDateSelect
        {
            add { base.AddHandler(CalendarDateSelectRoutedEvent, value); }
            remove { base.RemoveHandler(CalendarDateSelectRoutedEvent, value); }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CalendarMonthSelectRoutedEvent = EventManager.RegisterRoutedEvent(
            "_CalendarMonthSelect", RoutingStrategy.Bubble, typeof(EventHandler<CalendarDateSelectEventArge>), typeof(MyCalendar));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _CalendarMonthSelect
        {
            add { base.AddHandler(CalendarMonthSelectRoutedEvent, value); }
            remove { base.RemoveHandler(CalendarMonthSelectRoutedEvent, value); }
        }

        #endregion
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class CalendarDateSelectEventArge : RoutedEventArgs
    {
        public CalendarDateSelectEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        public int _Year { get; set; }
        public int _Month { get; set; }
        public int _Day { get; set; }
    }
}
