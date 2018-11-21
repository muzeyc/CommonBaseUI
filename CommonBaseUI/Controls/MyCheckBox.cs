using System.Windows;
using Util.Controls;
using CommonUtils;

namespace CommonBaseUI.Controls
{
    public class MyCheckBox : BulletCheckBox, IInputControl
    {
        public MyCheckBox()
        {
            this.Checked += MyCheckBox_Checked;
            this.Unchecked += MyCheckBox_Checked;
        }

        private object checkValue;
        public object _CheckValue
        {
            get
            {
                return checkValue;
            }
            set
            {
                checkValue = value;
            }
        }
        private object uncheckValue;
        public object _UnCheckValue
        {
            get
            {
                return uncheckValue;
            }
            set
            {
                uncheckValue = value;
            }
        }

        public object _Value
        {
            get
            {
                if (this.IsChecked.Value)
                {
                    return checkValue != null ? checkValue : true;
                }
                else
                {
                    return uncheckValue != null ? uncheckValue : false;
                }
            }
            set
            {
                object cv = checkValue != null ? checkValue : true;
                object val = value ?? false;
                this.IsChecked = val.Equals(cv);
            }
        }
        public string _Caption { get; set; }
        public bool _MustInput { get; set; }
        /// <summary>
        /// 绑定的对象中的字段
        /// </summary>
        public string _Binding { get; set; }
        public double _CaptionWidth { get; set; }
        public double _InputWidth { get; set; }

        public void _SetErr()
        {
            return;
        }

        public void _CleanErr()
        {
            return;
        }

        public bool _IsEnabled
        {
            get
            {
                return this.IsEnabled;
            }
            set
            {
                this.IsEnabled = value;
            }
        }

        private void MyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsChecked.Value)
            {
                this._Value = checkValue != null ? checkValue : true;
            }
            else
            {
                this._Value = uncheckValue != null ? uncheckValue : false;
            }
        }
    }
}
