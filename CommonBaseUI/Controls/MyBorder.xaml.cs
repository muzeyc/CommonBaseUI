using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyBorder.xaml 的交互逻辑
    /// </summary>
    public partial class MyBorder : UserControl
    {
        public MyBorder()
        {
            InitializeComponent();
        }

        public UIElementCollection _Children
        {
            get
            {
                return pnlBody.Children;
            }
        }

        public Orientation _Orientation
        {
            get
            {
                return pnlBody.Orientation;
            }
            set
            {
                pnlBody.Orientation = value;
            }
        }

        public HorizontalAlignment _HorizontalAlignment
        {
            get
            {
                return pnlBody.HorizontalAlignment;
            }
            set
            {
                pnlBody.HorizontalAlignment = value;
            }
        }

        public VerticalAlignment _VerticalAlignment
        {
            get
            {
                return pnlBody.VerticalAlignment;
            }
            set
            {
                pnlBody.VerticalAlignment = value;
            }
        }

        public void _SetBackground(string color)
        {
            pnlBorder.Background = CommUtil.CommonUtil.ToBrush(color);
        }
        public void _ClearBackground()
        {
             pnlBorder.Background = CommUtil.CommonUtil.ToBrush("#FFFFFF");
        }
    }
}
