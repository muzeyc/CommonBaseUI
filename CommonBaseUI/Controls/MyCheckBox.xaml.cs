using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyCheckBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyCheckBox : UserControl, IInputControl
    {
        public MyCheckBox()
        {
            InitializeComponent();
        }

        private object checkValue;
        public object _CheckValue
        {
            get
            {
                return checkValue;
            }
            set
            {
                checkValue = value;
            }
        }
        private object uncheckValue;
        public object _UnCheckValue
        {
            get
            {
                return uncheckValue;
            }
            set
            {
                uncheckValue = value;
            }
        }

        public object _Value
        {
            get
            {
                if (chkInput.IsChecked.Value)
                {
                    return checkValue != null ? checkValue : true;
                }
                else
                {
                    return uncheckValue != null ? uncheckValue : false;
                }
            }
            set
            {
                object cv = checkValue != null ? checkValue : true;
                object val = value ?? false;
                chkInput.IsChecked = val.Equals(cv);
            }
        }
        public string _Caption { get; set; }
        public bool _MustInput { get; set; }
        /// <summary>
        /// 绑定的对象中的字段
        /// </summary>
        public string _Binding { get; set; }
        public double _CaptionWidth { get; set; }
        public double _InputWidth { get; set; }

        public string Text
        {
            get { return chkInput.Text; }
            set { chkInput.Text = value; }
        }

        public string CheckedText
        {
            get { return chkInput.CheckedText; }
            set { chkInput.CheckedText = value; }
        }

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
                return this.IsEnabled;
            }
            set
            {
                this.IsEnabled = value;
            }
        }

        private void MyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (chkInput.IsChecked.Value)
            {
                this._Value = checkValue != null ? checkValue : true;
                var arge = new RoutedEventArgs(MyCheckBoxCheckedEvent, this);
                RaiseEvent(arge);
            }
            else
            {
                this._Value = uncheckValue != null ? uncheckValue : false;
                var arge = new RoutedEventArgs(MyCheckBoxUnCheckedEvent, this);
                RaiseEvent(arge);
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent MyCheckBoxCheckedEvent = EventManager.RegisterRoutedEvent(
            "Checked", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyCheckBox));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler Checked
        {
            add { base.AddHandler(MyCheckBoxCheckedEvent, value); }
            remove { base.RemoveHandler(MyCheckBoxCheckedEvent, value); }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent MyCheckBoxUnCheckedEvent = EventManager.RegisterRoutedEvent(
            "Unchecked", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyCheckBox));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler Unchecked
        {
            add { base.AddHandler(MyCheckBoxUnCheckedEvent, value); }
            remove { base.RemoveHandler(MyCheckBoxUnCheckedEvent, value); }
        }
    }
}
