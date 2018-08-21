using CommonBaseUI.Common;
using CommonBaseUI.CommUtil;
using System.Windows;

namespace CommonBaseUI.CommonView
{
    /// <summary>
    /// MyLayer.xaml 的交互逻辑
    /// </summary>
    public partial class MyWindow : Window
    {
        private object Item = null;
        public delegate void AfterCloseDelegate(object item, bool isCloseOnly);
        private AfterCloseDelegate _AfterClose;

        public MyWindow()
        {
            InitializeComponent();
        }

        public MyWindow(object item = null, AfterCloseDelegate afterClose = null, bool showTop = true)
        {
            InitializeComponent();

            this.Background = CommonUtil.From16JinZhi("#4F000000");
            if (!showTop)
            {
                pnlHead.Visibility = System.Windows.Visibility.Collapsed;
            }
            // 这句话是防止弹出窗口挡住桌面的工具栏
            //FullScreenManager.RepairWpfWindowFullScreenBehavior(this);
            try
            {
                //HwndSource winformWindow = (HwndSource.FromDependencyObject(Application.Current.MainWindow) as HwndSource);
                //if (winformWindow != null) new WindowInteropHelper(this) { Owner = winformWindow.Handle };

                this.Owner = Application.Current.MainWindow;
            }
            catch
            {

            }
            this.Item = item;
            _AfterClose = afterClose;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (_AfterClose != null)
            {
                _AfterClose(Item, true);
            }
            this.Close();
            Application.Current.MainWindow.Activate();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="afterClose"></param>
        public void _Close(RoutedEventHandler afterClose = null)
        {
            if (_AfterClose != null)
            {
                _AfterClose(Item, false);
            }
            this.Close();
            Application.Current.MainWindow.Activate();
        }
    }

    public class AfterCloseEventArgs : RoutedEventArgs
    {
        public bool _CloseOnly { get; set; }
        public object _Item { get; set; }
    }
}
