using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyTabButton.xaml 的交互逻辑
    /// </summary>
    public partial class MyTabButton : UserControl
    {
        public MyTabButton()
        {
            InitializeComponent();
            var backColor = ColorTranslator.FromHtml("#ccc");
            SolidColorBrush brushBackground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(backColor.R, backColor.G, backColor.B));
            button1.Background = brushBackground;

            var fontColor = ColorTranslator.FromHtml("#3b3b3b");
            SolidColorBrush bruchForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(fontColor.R, fontColor.G, fontColor.B));
            lblContent.Foreground = bruchForeground;
        }

        public int _Index { get; set; }

        public string _Text
        {
            get
            {
                return lblContent.Text;
            }
            set
            {
                lblContent.Text = value;
            }
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
                if (!value)
                {
                    var color = ColorTranslator.FromHtml("#ccc");
                    SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
                    var colorG = ColorTranslator.FromHtml("#999999");
                    SolidColorBrush myBrushG = new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorG.R, colorG.G, colorG.B));

                    button1.Background = myBrush;
                    lblContent.Foreground = myBrushG;
                }
                else
                {
                    var color = ColorTranslator.FromHtml("#EBEBEB");
                    SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
                    var colorG = ColorTranslator.FromHtml("#3b3b3b");
                    SolidColorBrush myBrushG = new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorG.R, colorG.G, colorG.B));

                    button1.Background = myBrush;
                    lblContent.Foreground = myBrushG;
                }
            }
        }

        /// <summary>
        /// 该标签页要显示的画面的名称，包括命名空间
        /// </summary>
        public string _ContentFormName { get; set; }
        /// <summary>
        /// Tab显示的画面
        /// </summary>
        public ContentControl _ContentForm { get; set; }

        private bool isSelected = false;
        /// <summary>
        /// 是否被选中
        /// </summary>
        /// 
        public bool _IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                var backColorStr = isSelected ? "#EBEBEB" : "#ccc";
                var backColor = ColorTranslator.FromHtml(backColorStr);
                SolidColorBrush brushBackground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(backColor.R, backColor.G, backColor.B));
                button1.Background = brushBackground;

                var arge = new RoutedEventArgs(TabBtnSelectionChangeEvent, this);
                RaiseEvent(arge);
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent TabClickEvent = EventManager.RegisterRoutedEvent(
            "_TabClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyTabButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _TabClick
        {
            add { base.AddHandler(TabClickEvent, value); }
            remove { base.RemoveHandler(TabClickEvent, value); }
        }

        private void button1_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var arge = new RoutedEventArgs(TabClickEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent TabClosedEvent = EventManager.RegisterRoutedEvent(
            "_TabClosed", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyTabButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _TabClosed
        {
            add { base.AddHandler(TabClosedEvent, value); }
            remove { base.RemoveHandler(TabClosedEvent, value); }
        }

        private void btnClose_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var arge = new RoutedEventArgs(TabClosedEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent TabBtnSelectionChangeEvent = EventManager.RegisterRoutedEvent(
            "_MyTabBtnSelectionChange", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyTab));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _SelectionChange
        {
            add { base.AddHandler(TabBtnSelectionChangeEvent, value); }
            remove { base.RemoveHandler(TabBtnSelectionChangeEvent, value); }
        }
    }
}
