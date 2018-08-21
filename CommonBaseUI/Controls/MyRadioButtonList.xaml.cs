using CommonBaseUI.CommUtil;
using CommonBaseUI.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyRadioButtonList.xaml 的交互逻辑
    /// </summary>
    public partial class MyRadioButtonList : UserControl, IInputControl
    {
        private List<MyRadioButton> List;
        public MyRadioButtonList()
        {
            InitializeComponent();

            List = new List<MyRadioButton>();
        }

        private object val = null;
        public object _Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                foreach (var radio in this.List)
                {
                    radio._IsChecked = radio._CheckedValue.ToStr().Equals(val.ToStr());
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
                btnClear.Visibility = value ? Visibility.Visible : Visibility.Hidden;
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

        public bool _MustInput { get; set; }

        /// <summary>
        /// 绑定的对象中的字段
        /// </summary>
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

        /// <summary>
        /// 默认值
        /// </summary>
        public object _DefaultValue { get; set; }

        /// <summary>
        /// 填充列表
        /// </summary>
        /// <param name="list"></param>
        public void _SetListFromDataDic(List<DataDicModel> list)
        {
            pnlBody.Children.Clear();
            this.List = new List<MyRadioButton>();
            if (list != null)
            {
                foreach (var model in list)
                {
                    var radio = new MyRadioButton();
                    radio.Margin = new Thickness(2, 0, 2, 0);
                    radio._CheckedValue = model.val;
                    radio._Text = model.name;
                    radio._Selected += radio_SelectionChange;
                    this.List.Add(radio);
                    pnlBody.Children.Add(radio);
                }

                _Clear();
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void _Clear()
        {
            MyRadioButton radio = null;
            if (!_DefaultValue.ToStr().IsNullOrEmpty())
            {
                foreach (var rdo in List)
                {
                    if (rdo._CheckedValue.ToStr().Equals(_DefaultValue))
                    {
                        radio = rdo;
                    }
                }
            }
            else
            {
                if (List.Count > 0)
                {
                    radio = List[0];
                }
            }

            if (radio == null)
            {
                return;
            }

            if (!_Value.ToStr().Equals(radio._CheckedValue.ToStr()))
            {
                _Value = radio._CheckedValue;

                var arge = new RoutedEventArgs(MyRadioListSelectionChangeRoutedEvent, this);
                RaiseEvent(arge);
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


        public double _InputWidth { get; set; }

        public void _SetErr()
        {
            return;
        }

        public void _CleanErr()
        {
            return;
        }

        private void radio_SelectionChange(object sender, RoutedEventArgs e)
        {
            var radio = sender as MyRadioButton;
            if (_Value.ToStr().Equals(radio._CheckedValue.ToStr()))
            {
                return;
            }

            foreach (var rdo in this.List)
            {
                rdo._IsChecked = false;
            }

            _Value = radio._CheckedValue;

            var arge = new RoutedEventArgs(MyRadioListSelectionChangeRoutedEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent MyRadioListSelectionChangeRoutedEvent = EventManager.RegisterRoutedEvent(
            "_SelectionChange", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyRadioButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _SelectionChange
        {
            add { base.AddHandler(MyRadioListSelectionChangeRoutedEvent, value); }
            remove { base.RemoveHandler(MyRadioListSelectionChangeRoutedEvent, value); }
        }
    }
}
