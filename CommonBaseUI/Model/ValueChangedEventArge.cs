using System.Windows;

namespace CommonBaseUI.Model
{
    public class ValueChangedEventArge : RoutedEventArgs
    {
        public ValueChangedEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 变化后的值
        /// </summary>
        public object _Value { get; set; }
        /// <summary>
        /// 变化前的值
        /// </summary>
        public object _ValueBeforeChange { get; set; }
    }
}
