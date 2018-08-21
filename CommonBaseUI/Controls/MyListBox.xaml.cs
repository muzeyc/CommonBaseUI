using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyListBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyListBox : UserControl
    {
        public MyListBox()
        {
            InitializeComponent();
        }

        public void _InsertAt(string content, int index)
        {
            var lbl = new TextBlock();
            lbl.TextWrapping = TextWrapping.Wrap;
            lbl.Height = 20;
            lbl.Margin = new Thickness(5, 0, 5, 0);
            lbl.Text = content;

            index = index > pnlBody.Children.Count ? pnlBody.Children.Count : index;

            if (index < 0)
            {
                pnlBody.Children.Add(lbl);
            }
            else
            {
                pnlBody.Children.Insert(index, lbl);
            }
        }

        /// <summary>
        /// 将滚动条固定在底部
        /// </summary>
        public void _FitScrollAtBottom()
        {
            double d = this.pnlScroll.ActualHeight + this.pnlScroll.ViewportHeight + this.pnlScroll.ExtentHeight;
            this.pnlScroll.ScrollToVerticalOffset(d); 
        }

        public void _RemoveRange(int index, int count)
        {
            pnlBody.Children.RemoveRange(index, count);
        }

        public double _MaxHeight
        {
            get
            {
                return pnlScroll.MaxHeight;
            }
            set
            {
                pnlScroll.MaxHeight = value;
            }
        }

        public double _MaxWidth
        {
            get
            {
                return pnlScroll.MaxWidth;
            }
            set
            {
                pnlScroll.MaxWidth = value;
            }
        }

        public int _RowCount
        {
            get
            {
                return pnlBody.Children.Count;
            }
        }
    }
}
