using System.Windows.Controls;
using CommonBaseUI.CommUtil;

namespace CommonBaseUI.Controls
{
    public class MyLabel : TextBlock, IInputControl
    {
        private object val = null;
        public object _Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                this.Text = val.ToStr();
            }
        }

        public string _Caption { get; set; }
        public bool _MustInput { get; set; }
        public string _Binding { get; set; }
        public double _CaptionWidth { get; set; }
        public double _InputWidth { get; set; }
        public bool _IsEnabled { get; set; }

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
