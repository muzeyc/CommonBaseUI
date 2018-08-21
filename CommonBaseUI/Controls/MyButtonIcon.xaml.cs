using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyButtonIcon.xaml 的交互逻辑
    /// </summary>
    public partial class MyButtonIcon : UserControl
    {
        public MyButtonIcon()
        {
            InitializeComponent();
        }
        public double _Width
        {
            get
            {
                return btn.Width;
            }
            set
            {
                btn.Width = value;
            }
        }

        public string _Icon
        {
            get
            {
                return btn.FIcon;
            }
            set
            {
                btn.FIcon = System.Web.HttpUtility.HtmlDecode(value);
            }
        }

        public string _Text
        {
            get
            {
                return btn.Content.ToString();
            }
            set
            {
                btn.Content = value;
            }
        }

        public Brush _Background
        {
            get
            {
                return btn.Background;
            }
            set
            {
                btn.Background = value;
                pnlBorder.Background = value;
                pnlBorder.BorderBrush = value;
            }
        }

        public Brush _Foreground
        {
            get
            {
                return btn.Foreground;
            }
            set
            {
                btn.Foreground = value;
            }
        }

        public Brush _MouseOverBackground
        {
            get
            {
                return btn.MouseOverBackground;
            }
            set
            {
                btn.MouseOverBackground = value;
            }
        }

        public Brush _MouseOverForeground
        {
            get
            {
                return btn.MouseOverForeground;
            }
            set
            {
                btn.MouseOverForeground = value;
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var arge = new RoutedEventArgs(MyButtonIconClickEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent MyButtonIconClickEvent = EventManager.RegisterRoutedEvent(
            "_IconBtnClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyButton));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _Click
        {
            add { base.AddHandler(MyButtonIconClickEvent, value); }
            remove { base.RemoveHandler(MyButtonIconClickEvent, value); }
        }

        private void btn_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var border = btn.Parent as Border;
            border.Background = btn.MouseOverBackground;
            border.BorderBrush = btn.MouseOverBackground;
        }

        private void btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var border = btn.Parent as Border;
            border.Background = btn.Background;
            border.BorderBrush = btn.Background;
        }
    }
}
