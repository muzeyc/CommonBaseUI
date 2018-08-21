using CommonBaseUI.CommUtil;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyRadioButton.xaml 的交互逻辑
    /// </summary>
    public partial class MyRadioButton : UserControl
    {
        public MyRadioButton()
        {
            InitializeComponent();
            RadioButton r = new RadioButton();
        }

        public string _Text
        {
            get
            {
                return btnRadio.Content.ToString();
            }
            set
            {
                btnRadio.Content = value;
            }
        }
        public object _CheckedValue { get; set; }

        private bool isChecked = false;
        public bool _IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                if (isChecked)
                {
                    btnRadio.FIcon = System.Web.HttpUtility.HtmlDecode("&#xf058;");
                    var brushB = CommonUtil.ToBrush("#4A4A4A"); ;
                    btnRadio.Background = brushB;
                    btnRadio.MouseOverBackground = brushB;
                    btnRadio.PressedBackground = brushB;
                    pnlBorder.Background = brushB;

                    var brushF = CommonUtil.ToBrush("#FFFFFF");
                    btnRadio.Foreground = brushF;
                    btnRadio.MouseOverForeground = brushF;
                    btnRadio.PressedForeground = brushF;
                }
                else
                {
                    btnRadio.FIcon = System.Web.HttpUtility.HtmlDecode("&#xf10c;");

                    var brushB = CommonUtil.ToBrush("#FFFFFF"); ;
                    btnRadio.Background = brushB;
                    btnRadio.MouseOverBackground = brushB;
                    btnRadio.PressedBackground = brushB;
                    pnlBorder.Background = brushB;

                    var brushF = CommonUtil.ToBrush("#4A4A4A");
                    btnRadio.Foreground = brushF;
                    btnRadio.MouseOverForeground = brushF;
                    btnRadio.PressedForeground = brushF;
                }
            }
        }

        private void btnRadio_Click(object sender, RoutedEventArgs e)
        {
            _IsChecked = true;

            var arge = new RoutedEventArgs(MyRadioSelectedRoutedEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent MyRadioSelectedRoutedEvent = EventManager.RegisterRoutedEvent(
            "_Selected", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyRadioButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _Selected
        {
            add { base.AddHandler(MyRadioSelectedRoutedEvent, value); }
            remove { base.RemoveHandler(MyRadioSelectedRoutedEvent, value); }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent MyRadioUnSelectedRoutedEvent = EventManager.RegisterRoutedEvent(
            "_UnSelected", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyRadioButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _UnSelected
        {
            add { base.AddHandler(MyRadioUnSelectedRoutedEvent, value); }
            remove { base.RemoveHandler(MyRadioUnSelectedRoutedEvent, value); }
        }
    }
}
