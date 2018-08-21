using CommonBaseUI.Model;
using System.Windows.Controls;
using Util.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyMenu.xaml 的交互逻辑
    /// </summary>
    public partial class MyMenu : UserControl
    {
        public MyMenu()
        {
            InitializeComponent();
        }
    }

    public class MyMenuButton : FButton
    {
        public MenuModel _MenuInfo { get; set; }
    }
}
