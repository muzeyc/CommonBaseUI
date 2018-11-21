using CommonUtils;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyButton.xaml 的交互逻辑
    /// </summary>
    public partial class MyButton : UserControl, IInputControl
    {
        public MyButton()
        {
            InitializeComponent();
        }

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

        private string backGroud = "#ccc";
        public string _BackGroud
        {
            get
            {
                return backGroud;
            }
            set
            {
                var color = ColorTranslator.FromHtml(value);
                SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
            }
        }

        private string fontColor = "#3b3b3b";
        public string _FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                var color = ColorTranslator.FromHtml(value);
                SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));

                button1.Foreground = myBrush;
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
                    button1.Foreground = myBrushG;
                }
                else
                {
                    var color = ColorTranslator.FromHtml("#EBEBEB");
                    SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
                    var colorG = ColorTranslator.FromHtml("#3b3b3b");
                    SolidColorBrush myBrushG = new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorG.R, colorG.G, colorG.B));

                    button1.Background = myBrush;
                    button1.Foreground = myBrushG;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var arge = new RoutedEventArgs(ClickEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "_Click", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _Click
        {
            add { base.AddHandler(ClickEvent, value); }
            remove { base.RemoveHandler(ClickEvent, value); }
        }

        public object _Value
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
    }
}
