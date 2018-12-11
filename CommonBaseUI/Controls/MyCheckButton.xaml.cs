using CommonUtils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyCheckButton.xaml 的交互逻辑
    /// </summary>
    public partial class MyCheckButton : UserControl, IInputControl
    {
        public MyCheckButton()
        {
            InitializeComponent();
        }

        public object _Value
        {
            get
            {
                if (this.button1.IsChecked.Value)
                {
                    return _CheckValue != null ? _CheckValue : true;
                }
                else
                {
                    return _UnCheckValue != null ? _UnCheckValue : false;
                }
            }
            set
            {
                object cv = _CheckValue != null ? _CheckValue : true;
                object val = value ?? false;
                this.button1.IsChecked = val.Equals(cv);
            }
        }

        public string _Caption { get; set; }
        public bool _MustInput { get; set; }
        public string _Binding { get; set; }
        public double _CaptionWidth { get; set; }
        public double _InputWidth { get; set; }

        public void _SetErr()
        {
            return;
        }

        public void _CleanErr()
        {
            return;
        }

        public bool _IsEnabled
        {
            get
            {
                return button1.IsEnabled;
            }
            set
            {
                button1.IsEnabled = value;
            }
        }

        #region 自定义属性

        public double _Width
        {
            get
            {
                return button1.Width;
            }
            set
            {
                button1.Width = value;
            }
        }

        public double _Height
        {
            get
            {
                return button1.Height;
            }
            set
            {
                button1.Height = value;
            }
        }

        public string _Text
        {
            get
            {
                return button1.Content.ToStr();
            }
            set
            {
                button1.Content = value;
            }
        }
        
        public object _CheckValue { get; set; }
        public object _UnCheckValue { get; set; }

        public string _GroupName {
            get { return button1.GroupName; }
            set { button1.GroupName = value; }
        }
        public bool? _IsChecked
        {
            get { return button1.IsChecked; }
            set { button1.IsChecked = value; }
        }

        #endregion

        #region 自定义事件

        private void MyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.button1.IsChecked.Value)
            {
                this._Value = _CheckValue != null ? _CheckValue : true;
                var arge = new RoutedEventArgs(CheckedEvent, this);
                RaiseEvent(arge);
            }
            else
            {
                this._Value = _UnCheckValue != null ? _UnCheckValue : false;
                var arge = new RoutedEventArgs(UncheckedEvent, this);
                RaiseEvent(arge);
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent(
            "_Checked", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyCheckButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _Checked
        {
            add { base.AddHandler(CheckedEvent, value); }
            remove { base.RemoveHandler(CheckedEvent, value); }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent(
            "_Unchecked", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyCheckButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _Unchecked
        {
            add { base.AddHandler(UncheckedEvent, value); }
            remove { base.RemoveHandler(UncheckedEvent, value); }
        }

        #endregion
    }
}
