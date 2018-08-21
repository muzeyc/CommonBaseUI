using System;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    public class SubFormPanel : StackPanel
    {
        /// <summary>
        /// 可以放任何从前一个画面传递到该画面的对象
        /// </summary>
        //public object _Item { get; set; }

        /// <summary>
        /// 显示子画面
        /// </summary>
        /// <param name="element"></param>
        public void _Show(UIElement element)
        {
            this.Children.Add(element);
            this.Visibility = Visibility.Visible;

            var arge = new RoutedEventArgs(AfterShowRoutedEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 移除子画面
        /// </summary>
        /// <param name="element"></param>
        public void _Remove(UIElement element, object item = null)
        {
            this.Children.Remove(element);
            this.Visibility = Visibility.Hidden;

            var arge = new SubFormPanelEventArge(AfterRemoveRoutedEvent, this);
            arge._Item = item;
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent AfterShowRoutedEvent = EventManager.RegisterRoutedEvent(
            "_AfterShow", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(SubFormPanel));

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent AfterRemoveRoutedEvent = EventManager.RegisterRoutedEvent(
            "_AfterRemove", RoutingStrategy.Bubble, typeof(EventHandler<SubFormPanelEventArge>), typeof(SubFormPanel));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _AfterShow
        {
            add { base.AddHandler(AfterShowRoutedEvent, value); }
            remove { base.RemoveHandler(AfterShowRoutedEvent, value); }
        }

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _AfterRemove
        {
            add { base.AddHandler(AfterRemoveRoutedEvent, value); }
            remove { base.RemoveHandler(AfterRemoveRoutedEvent, value); }
        }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class SubFormPanelEventArge : RoutedEventArgs
    {
        public SubFormPanelEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        public object _Item { get; set; }
    }
}
