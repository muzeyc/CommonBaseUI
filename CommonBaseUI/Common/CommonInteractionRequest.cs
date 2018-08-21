
using System.Windows;
namespace CommonBaseUI.Common
{
    public class CommonInteractionRequest
    {
        public event RoutedEventHandler Requested;

        public void Request()
        {
            if (Requested != null)
            {
                var eventArgs = new CommonFormCloseEventArgs();
                eventArgs._IsFuncClose = true;
                Requested(this, eventArgs);
            }
        }
    }

    public class CommonFormCloseEventArgs : RoutedEventArgs
    {
        public CommonFormCloseEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }

        public CommonFormCloseEventArgs()
            : base()
        {

        }
        public bool _IsFuncClose { get; set; }
    }
}
