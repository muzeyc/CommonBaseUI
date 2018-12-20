using CommonUtils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyGridViewCellGantt.xaml 的交互逻辑
    /// </summary>
    public partial class MyGridViewCellGantt : UserControl
    {
        private List<string> colorList;
        public MyGridViewCellGantt()
        {
            InitializeComponent();
        }
        public MyGridViewCellGantt(GanttUnit unit, DateTime timeFrom, DateTime timeTo)
        {
            InitializeComponent();
            Init(unit, timeFrom, timeTo);
        }

        private void Init(GanttUnit unit, DateTime timeFrom, DateTime timeTo)
        {
            _Unit = unit;
            _TimeFrom = timeFrom;
            _TimeTo = timeTo;
            this.colorList = new List<string>
			{
				"#FFF8DC",
				"#AFEEEE",
				"#FFFFF0",
				"#F0FFF0",
				"#FFFACD",
				"#F5FFFA",
				"#FFF5EE",
				"#F0FFFF",
				"#FFF0F5",
				"#F0F8FF",
				"#E6E6FA",
				"#FFE4E1",
				"#00CED1",
				"#B0C4DE",
				"#ADD8E6",
				"#B0E0E6",
				"#00FFFF",
				"#48D1CC",
				"#40E0D0",
				"#20B2AA"
			};
            this.SetCellBorder();
        }

        private void SetCellBorder()
        {
            this.pnlBorder.Children.Clear();
            int count = _GetDispUnitCount();
            double width = 0;
            for (int i = 0; i < count; i++)
            {
                Border border = new Border();
                border.Width = _PiceWidth * _Pice;
                border.BorderBrush = CommonUtil.ToBrush("#ccc");
                border.BorderThickness = new Thickness(0.5, 0, 0.5, 0);
                border.Margin = new Thickness(-0.5, 0.0, 0.0, 0.0);
                this.pnlBorder.Children.Add(border);
                width += (border.Width + 0.5);
            }
            pnlBody.Width = width;
        }

        /// <summary>
        /// 获取标题列表
        /// </summary>
        /// <returns></returns>
        public List<string> _GetTitleList()
        {
            int count = _GetDispUnitCount();
            var list = new List<string>();
            switch (this._Unit)
            {
                case GanttUnit.Year:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, 1, 1);
                        for (int i = 0; i < count; i++)
                        {
                            var date = timeFrom.AddYears(i);
                            list.Add(string.Format("{0}年", date.Year));
                        }
                    }
                    break;
                case GanttUnit.Quarter:
                    {
                        DateTime timeFrom;
                        if (_TimeFrom.Month <= 3)
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 1, 1);
                        }
                        else if (_TimeFrom.Month <= 6)
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 4, 1);
                        }
                        else if (_TimeFrom.Month <= 9)
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 7, 1);
                        }
                        else
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 10, 1);
                        }

                        for (int i = 0; i < count; i++)
                        {
                            var date = timeFrom.AddMonths(i * 3);
                            if (date.Month <= 3)
                            {
                                list.Add(string.Format("{0}年{1}季度", date.Year, 1));
                            }
                            else if (date.Month <= 6)
                            {
                                list.Add(string.Format("{0}年{1}季度", date.Year, 2));
                            }
                            else if (date.Month <= 9)
                            {
                                list.Add(string.Format("{0}年{1}季度", date.Year, 3));
                            }
                            else
                            {
                                list.Add(string.Format("{0}年{1}季度", date.Year, 4));
                            }
                        }
                    }
                    break;
                case GanttUnit.Month:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, _TimeFrom.Month, 1);
                        for (int i = 0; i < count; i++)
                        {
                            timeFrom = timeFrom.AddMonths(i);
                            list.Add(string.Format("{0}年{1}月", timeFrom.Year, timeFrom.Month));
                        }
                    }
                    break;
                case GanttUnit.Week:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, _TimeFrom.Month, _TimeFrom.Day);

                        for (int i = 0; i < count; i++)
                        {
                            int week = DateUtil.GetWeekOfYear(timeFrom);
                            list.Add(string.Format("{0}年第{1}周", timeFrom.Year, week));
                            timeFrom = timeFrom.AddDays(7);
                        }
                    }
                    break;
                case GanttUnit.Day:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, _TimeFrom.Month, _TimeFrom.Day);
                        for (int i = 0; i < count; i++)
                        {
                            var date = timeFrom.AddDays(i);
                            list.Add(date.ToString("yyyy-MM-dd"));
                        }
                    }
                    break;
                case GanttUnit.Hour:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, _TimeFrom.Month, _TimeFrom.Day, 0, 0, 0);
                        for (int i = 0; i < count; i++)
                        {
                            var time = timeFrom.AddHours(i);
                            list.Add(time.ToString("yyyy-MM-dd"));
                        }
                    }

                    break;
                default:
                    break;
            }
            return list;
        }

        /// <summary>
        /// 获取单元格数量
        /// </summary>
        public int _GetDispUnitCount()
        {
            int count = 0;
            switch (this._Unit)
            {
                case GanttUnit.Year:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, 1, 1);
                        var timeTo = new DateTime(_TimeTo.Year, 1, 1);
                        while (timeFrom.AddYears(count) <= timeTo)
                        {
                            count++;
                        }
                    }
                    break;
                case GanttUnit.Quarter:
                    {
                        DateTime timeFrom;
                        DateTime timeTo;
                        if (_TimeFrom.Month <= 3)
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 1, 1);
                        }
                        else if (_TimeFrom.Month <= 6)
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 4, 1);
                        }
                        else if (_TimeFrom.Month <= 9)
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 7, 1);
                        }
                        else
                        {
                            timeFrom = new DateTime(_TimeFrom.Year, 10, 1);
                        }

                        if (_TimeTo.Month <= 3)
                        {
                            timeTo = new DateTime(_TimeTo.Year, 1, 1);
                        }
                        else if (_TimeTo.Month <= 6)
                        {
                            timeTo = new DateTime(_TimeTo.Year, 4, 1);
                        }
                        else if (_TimeTo.Month <= 9)
                        {
                            timeTo = new DateTime(_TimeTo.Year, 7, 1);
                        }
                        else
                        {
                            timeTo = new DateTime(_TimeTo.Year, 10, 1);
                        }
                        int i = 0;
                        while (timeFrom.AddMonths(i) <= timeTo)
                        {
                            i += 3;
                            count++;
                        }
                    }
                    break;
                case GanttUnit.Month:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, _TimeFrom.Month, 1);
                        var timeTo = new DateTime(_TimeTo.Year, _TimeTo.Month, 1);
                        while (timeFrom.AddMonths(count) <= timeTo)
                        {
                            count++;
                        }
                    }
                    break;
                case GanttUnit.Week:
                    {
                        var temp1 = DateUtil.GetDaysOfThisWeek(_TimeFrom);
                        var temp2 = DateUtil.GetDaysOfThisWeek(_TimeTo);
                        var timeFrom = temp1[0].ToDateTime();
                        var timeTo = temp2[6].ToDateTime();
                        int i = 0;
                        while (timeFrom.AddDays(i) <= timeTo)
                        {
                            i += 7;
                            count++;
                        }
                    }
                    break;
                case GanttUnit.Day:
                    {
                        var timeFrom = new DateTime(_TimeFrom.Year, _TimeFrom.Month, _TimeFrom.Day);
                        var timeTo = new DateTime(_TimeTo.Year, _TimeTo.Month, _TimeTo.Day);
                        count = Math.Ceiling((timeTo - timeFrom).TotalDays).ToInt() + 1;
                    }
                    break;
                case GanttUnit.Hour:
                    count = Math.Ceiling((_TimeTo - _TimeFrom).TotalHours).ToInt();
                    break;
                default:
                    count = 0;
                    break;
            }
            return count;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="list"></param>
        public void _SetData(List<GanttObjModel> list)
        {
            if (!DateTime.MinValue.Equals(this._TimeFrom))
            {
                foreach (GanttObjModel current in list)
                {
                    var grid = new Grid();
                    grid.Height = 25.0;
                    grid.Background = CommonUtil.ToBrush(this._ObjColor);
                    grid.HorizontalAlignment = HorizontalAlignment.Left;
                    double offsetWidth = this.GetOffsetWidth(this._TimeFrom, current._TimeFrom);
                    grid.Margin = new Thickness(offsetWidth, 0.0, 0.0, 0.0);
                    grid.Width = this.GetOffsetWidth(current._TimeFrom, current._TimeTo, true);
                    int num = 0;
                    if (current._SubObjList != null)
                    {
                        foreach (GanttObjModel current2 in current._SubObjList)
                        {
                            Label textBlock = new Label();
                            textBlock.Height = 18.0;
                            textBlock.Background = CommonUtil.ToBrush(this.colorList[num % this.colorList.Count]);
                            textBlock.Foreground = CommonUtil.ToBrush("#3B3B3B");
                            textBlock.Content = current2._Content;
                            textBlock.FontSize = 9;
                            textBlock.Padding = new Thickness(0);
                            textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                            textBlock.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                            textBlock.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            double offsetWidth2 = this.GetOffsetWidth(current._TimeFrom, current2._TimeFrom);
                            textBlock.Margin = new Thickness(offsetWidth2, 0.0, 0.0, 0.0);
                            textBlock.Width = this.GetOffsetWidth(current2._TimeFrom, current2._TimeTo, true);
                            grid.Children.Add(textBlock);
                            num++;
                        }
                    }
                    this.pnlGantt.Children.Add(grid);
                }
            }
        }

        /// <summary>
        /// 获取进程的偏移量
        /// </summary>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="isWidth">是否是算宽度</param>
        /// <returns></returns>
        private double GetOffsetWidth(DateTime timeFrom, DateTime timeTo, bool isWidth = false)
        {
            TimeSpan timeSpan = timeTo - timeFrom;
            int num;
            switch (this._Unit)
            {
                case GanttUnit.Year:
                    num = Math.Floor(timeSpan.TotalDays / 30.0).ToInt();
                    break;
                case GanttUnit.Quarter:
                    num = Math.Floor(timeSpan.TotalDays / 10.0).ToInt();
                    break;
                case GanttUnit.Month:
                    num = Math.Floor(timeSpan.TotalDays).ToInt();
                    break;
                case GanttUnit.Week:
                    num = Math.Floor(timeSpan.TotalDays).ToInt();
                    break;
                case GanttUnit.Day:
                    num = Math.Floor(timeSpan.TotalHours).ToInt() + (isWidth ? 24 : 0);
                    break;
                case GanttUnit.Hour:
                    num = Math.Floor(timeSpan.TotalMinutes / 10.0).ToInt();
                    break;
                default:
                    num = 0;
                    break;
            }
            var offset = Math.Round((double)num * this._PiceWidth, 1);
            // 修正因为0.5像素边框产生的偏移误差
            int offsetUnit = num / _Pice;
            offset -= (offsetUnit * 0.5);
            return offset;
        }

        public void _Clear()
        {
            this.pnlGantt.Children.Clear();
        }

        #region 属性

        public List<GanttObjModel> _GanttList { get; set; }

        private DateTime timeFrom;
        public DateTime _TimeFrom
        {
            get
            {
                switch (this._Unit)
                {
                    case GanttUnit.Year:
                        return new DateTime(timeFrom.Year, 1, 1);
                    case GanttUnit.Quarter:
                        if (timeFrom.Month <= 3)
                        {
                            return new DateTime(timeFrom.Year, 1, 1);
                        }
                        else if (timeFrom.Month > 3 && timeFrom.Month <= 6)
                        {
                            return new DateTime(timeFrom.Year, 4, 1);
                        }
                        else if (timeFrom.Month > 6 && timeFrom.Month <= 9)
                        {
                            return new DateTime(timeFrom.Year, 7, 1);
                        }
                        else
                        {
                            return new DateTime(timeFrom.Year, 10, 1);
                        }
                    case GanttUnit.Month:
                        return new DateTime(timeFrom.Year, timeFrom.Month, 1);
                    case GanttUnit.Week:
                        var temp = DateUtil.GetDaysOfThisWeek(timeFrom);
                        return temp[0].ToDateTime();
                    case GanttUnit.Day:
                    case GanttUnit.Hour:
                        return new DateTime(timeFrom.Year, timeFrom.Month, timeFrom.Day, 0, 0, 0);
                    default:
                        return timeFrom;
                }
            }
            set
            {
                timeFrom = value;
            }
        }

        private DateTime timeTo;
        public DateTime _TimeTo
        {
            get
            {
                switch (this._Unit)
                {
                    case GanttUnit.Year:
                        return new DateTime(timeTo.Year, 12, 31);
                    case GanttUnit.Quarter:
                        if (timeTo.Month <= 3)
                        {
                            return new DateTime(timeTo.Year, 3, 31);
                        }
                        else if (timeTo.Month > 3 && timeTo.Month <= 6)
                        {
                            return new DateTime(timeTo.Year, 6, 30);
                        }
                        else if (timeTo.Month > 6 && timeTo.Month <= 9)
                        {
                            return new DateTime(timeTo.Year, 9, 30);
                        }
                        else
                        {
                            return new DateTime(timeTo.Year, 12, 31);
                        }
                    case GanttUnit.Month:
                        return DateUtil.GetLastDateForMonth(timeTo.Year, timeTo.Month);
                    case GanttUnit.Week:
                        var temp = DateUtil.GetDaysOfThisWeek(timeTo);
                        return temp[6].ToDateTime();
                    case GanttUnit.Day:
                    case GanttUnit.Hour:
                        return new DateTime(timeTo.Year, timeTo.Month, timeTo.Day, 23, 59, 59);
                    default:
                        return timeTo;
                }
            }
            set
            {
                timeTo = value;
            }
        }

        private string objColor = "#DCDCDC";
        /// <summary>
        /// 主进程颜色
        /// </summary>
        public string _ObjColor
        {
            get
            {
                return this.objColor;
            }
            set
            {
                this.objColor = value;
            }
        }
        /// <summary>
        /// 单元格单位
        /// </summary>
        public GanttUnit _Unit { get; set; }

        private int pice = 10;
        /// <summary>
        /// 切片数量
        /// </summary>
        public int _Pice
        {
            get
            {
                int result;
                if (this.pice == 10)
                {
                    switch (this._Unit)
                    {
                        case GanttUnit.Year:
                            result = 12;
                            break;
                        case GanttUnit.Quarter:
                            result = 9;
                            break;
                        case GanttUnit.Month:
                            result = 30;
                            break;
                        case GanttUnit.Week:
                            result = 7;
                            break;
                        case GanttUnit.Day:
                            result = 24;
                            break;
                        case GanttUnit.Hour:
                            result = 6;
                            break;
                        default:
                            result = this.pice;
                            break;
                    }
                }
                else
                {
                    result = this.pice;
                }
                return result;
            }
            set
            {
                this.pice = value;
            }
        }

        private double piceWidth = 10.0;
        /// <summary>
        /// 切片宽度
        /// </summary>
        public double _PiceWidth
        {
            get
            {
                double result;
                if (this.piceWidth == 10.0)
                {
                    switch (this._Unit)
                    {
                        case GanttUnit.Year:
                            result = 8.3;
                            break;
                        case GanttUnit.Quarter:
                            result = 11.1;
                            break;
                        case GanttUnit.Month:
                            result = 3.3;
                            break;
                        case GanttUnit.Week:
                            result = 14.3;
                            break;
                        case GanttUnit.Day:
                            result = 4.2;
                            break;
                        case GanttUnit.Hour:
                            result = 16.7;
                            break;
                        default:
                            result = this.piceWidth;
                            break;
                    }
                }
                else
                {
                    result = this.piceWidth;
                }
                return result;
            }
            set
            {
                this.piceWidth = value;
            }
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int _RowIndex { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        public int _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string _ColName { get; set; }
        public double _Width
        {
            get { return pnlBody.Width; }
        }

        #endregion
    }

    public class GanttObjModel
    {
        public DateTime _TimeFrom { get; set; }

        public DateTime _TimeTo { get; set; }

        public string _Content { get; set; }

        public List<GanttObjModel> _SubObjList { get; set; }
    }

    public enum GanttUnit
    {
        Year = 1,
        Quarter = 2,
        Month = 3,
        Week = 4,
        Day = 5,
        Hour = 6
    }
}
