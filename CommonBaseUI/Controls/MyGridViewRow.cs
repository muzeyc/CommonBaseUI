using CommonBaseUI.CommUtil;
using System.Collections.Generic;
using System.Windows.Controls;
namespace CommonBaseUI.Controls
{
    public class MyGridViewRow : ConditionPanel
    {
        private string rowActColor;
        public MyGridViewRow(int index)
        {
            this._Index = index;
            this.MinHeight = 30;
            this.Orientation = Orientation.Horizontal;
            this.Margin = new System.Windows.Thickness(0, 0.2, 0, 0.2);
        }

        private int index = 0;
        public int _Index 
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                this.rowActColor = index % 2 == 0 ? "#F9F9F9" : "#FFFFFF";
                this.Background = CommonUtil.ToBrush(rowColor.IsNullOrEmpty() ? rowActColor : rowColor);
            } 
        }

        private bool selected = false;
        public bool _Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                if (selected)
                {
                    this.Background = CommonUtil.ToBrush("#9BCD9B");
                    foreach (var child in this.Children)
                    {
                        if (child is Label)
                        {
                            ((Label)child).Content = "▶";
                            break;
                        }
                    }
                }
                else
                {
                    // 偶数行
                    if (index % 2 == 0)
                    {
                        this.Background = CommonUtil.ToBrush(rowColor.IsNullOrEmpty() ? "#f9f9f9" : rowColor);
                    }
                    else
                    {
                        this.Background = CommonUtil.ToBrush(rowColor.IsNullOrEmpty() ? "#ffffff" : rowColor);
                    }
                    foreach (var child in this.Children)
                    {
                        if (child is TextBlock)
                        {
                            ((TextBlock)child).Foreground = CommonUtil.ToBrush("#000000");
                        }
                        else if (child is Label)
                        {
                            ((Label)child).Content = "";
                        }
                    }
                }
            }
        }

        private string rowColor = "";
        public string _RowColor
        {
            get { return rowColor; }
            set
            {
                rowColor = value;
                if (!rowColor.IsNullOrEmpty())
                {
                    this.Background = CommonUtil.ToBrush(rowColor);
                    //foreach (var child in this.Children)
                    //{
                    //    if (child is TextBlock)
                    //    {
                    //        if ("#FA8072".Equals(rowColor) || "#63B8FF".Equals(rowColor))
                    //        {
                    //            ((TextBlock)child).Foreground = CommonUtil.ToBrush("#FFFFFF");
                    //        }
                    //        else
                    //        {
                    //            ((TextBlock)child).Foreground = CommonUtil.ToBrush("#000000");
                    //        }
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// 还原当前行背景颜色
        /// </summary>
        public void _CleanRowColor()
        {
            _RowColor = rowActColor;
        }

        public bool readOnly = true;
        public bool _ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
                foreach (var col in this.Children)
                {
                    if (col is MyGridViewCellButton)
                    {
                        var c = col as MyGridViewCellButton;
                        c._IsEnabled = !readOnly;
                    }
                    else if (col is MyGridViewCellCheckBox)
                    {
                        var c = col as MyGridViewCellCheckBox;
                        c._IsEnabled = !readOnly;
                    }
                }
            }
        }
    }
}
