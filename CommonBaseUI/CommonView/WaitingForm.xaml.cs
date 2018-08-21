using CommonBaseUI.Common;
using CommonBaseUI.CommUtil;
using System.Windows;

namespace CommonBaseUI.CommonView
{
    /// <summary>
    /// WaitingForm.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingForm : Window
    {
        public WaitingForm()
        {
            InitializeComponent();
            this.Background = CommonUtil.From16JinZhi("#4F000000");
            // 这句话是防止弹出窗口挡住桌面的工具栏
            FullScreenManager.RepairWpfWindowFullScreenBehavior(this);
            try
            {
                this.Owner = Application.Current.MainWindow;
            }
            catch
            {

            }
        }
    }
}
