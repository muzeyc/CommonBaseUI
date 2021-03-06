﻿using CommonUtils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyNumberBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyNumberBox : UserControl, IInputControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private object BeforeInputValue = null;
        public MyNumberBox()
        {
            InitializeComponent();
        }
        private object val = null;
        public object _Value
        {
            get
            {
                if (txtInput.Text.IsNullOrEmpty())
                {
                    return null;
                }
                if (dataType == DataType.Decimal)
                {
                    return txtInput.Text.ToDec();
                }
                else
                {
                    return txtInput.Text.ToInt();
                }
            }
            set
            {
                val = value;
                txtInput.Text = value.ToStr();
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("_Value"));//对_Value进行监听  
                }
            }
        }

        public bool _IsEnabled
        {
            get
            {
                return !txtInput.IsReadOnly;
            }
            set
            {
                txtInput.IsReadOnly = !value;
                txtInput.Background = value ? CommonUtils.CommonUtil.ToBrush("#fff") : CommonUtils.CommonUtil.ToBrush("#EBEBEB");
                btnClear.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                btnClear.Width = value ? 20 : 0;
                if (!_IsEnabled)
                {
                    txtInput.Width = inputWidth;
                }
                else
                {
                    txtInput.Width = inputWidth - 18;
                }
                txtInput.Margin = value ? new Thickness(2, 2, 20, 2) : new Thickness(2);
            }
        }


        public string _Caption
        {
            get
            {
                return lblCaption.Content.ToStr();
            }
            set
            {
                lblCaption.Content = value;
                if (lblCaption.Content.ToStr().IsNullOrEmpty())
                {
                    splBorder.Children.RemoveAt(0);

                    var border = splBorder.Children[0] as Border;
                    border.CornerRadius = new System.Windows.CornerRadius(3);
                }
            }
        }

        private bool mustInput = false;
        public bool _MustInput
        {
            get
            {
                return mustInput;
            }
            set
            {
                mustInput = value;
                //if (mustInput)
                //{
                //    var color = ColorTranslator.FromHtml("#EE7600");
                //    SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));

                //    var colorW = ColorTranslator.FromHtml("#FFFFFF");
                //    SolidColorBrush myBrushW = new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorW.R, colorW.G, colorW.B));

                //    this.lblCaption.Background = (System.Windows.Media.Brush)myBrush;
                //    this.lblCaption.Foreground = (System.Windows.Media.Brush)myBrushW;
                //    var border = lblCaption.Parent as Border;
                //    border.BorderBrush = (System.Windows.Media.Brush)myBrush;
                //    border.Background = (System.Windows.Media.Brush)myBrush;
                //}
            }
        }

        /// <summary>
        /// 绑定的对象中的字段
        /// </summary>
        public string _Binding { get; set; }

        /// <summary>
        /// 设置输入框背景色
        /// </summary>
        public void _SetErr()
        {
            this.txtInput.Background = CommonUtils.CommonUtil.ToBrush("#FA8072");
            this.txtInput.Foreground = CommonUtils.CommonUtil.ToBrush("#FFFFFF");
        }


        public double _CaptionWidth
        {
            get
            {
                return lblCaption.Width;
            }
            set
            {
                lblCaption.Width = value;
            }
        }

        private double inputWidth = 120;
        public double _InputWidth
        {
            get
            {
                return txtInput.Width;
            }
            set
            {
                double width = value > 20 ? value : inputWidth;
                inputWidth = width;
                if (_IsEnabled)
                {
                    txtInput.Width = width - 20;
                }
                else
                {
                    txtInput.Width = width;
                }
            }
        }

        private DataType dataType = DataType.Decimal;
        public DataType _DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }

        /// <summary>
        /// 清除输入框背景色
        /// </summary>
        public void _CleanErr()
        {
            this.txtInput.Background = CommonUtils.CommonUtil.ToBrush("#FFFFFF");
            this.txtInput.Foreground = CommonUtils.CommonUtil.ToBrush("#000000");
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void _Clear()
        {
            _Value = string.Empty;
        }

        /// <summary>
        /// 清除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _Clear();
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("_Value"));//对_Value进行监听  
            }

            var arge = new ValueChangeEventArge(NumberBoxValueChangeRoutedEvent, this);
            arge._ChangeBeforeValue = BeforeInputValue;
            arge._ChangeAfterValue = _Value;
            RaiseEvent(arge);
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            BeforeInputValue = _Value;
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                 (e.Key >= Key.D0 && e.Key <= Key.D9) ||
                 e.Key == Key.Back ||
                 e.Key == Key.Left || e.Key == Key.Right ||
                 CheckInput(e.Key))
            {
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private bool CheckInput(Key key)
        {
            switch (key)
            {
                case Key.OemMinus:
                    return _Value.ToStr().IsNullOrEmpty();
                case Key.OemPeriod:
                    return (dataType == DataType.Decimal
                        && !_Value.ToStr().IsNullOrEmpty()
                        && this.txtInput.Text.IndexOf('.') < 0); 
            }

            return false;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public enum DataType
        {
            Int = 1,
            Decimal = 2,
            Text = 3,
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent NumberBoxValueChangeRoutedEvent = EventManager.RegisterRoutedEvent(
            "_ NumberBoxValueChange", RoutingStrategy.Bubble, typeof(EventHandler<ValueChangeEventArge>), typeof(MyNumberBox));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _ValueChange
        {
            add { base.AddHandler(NumberBoxValueChangeRoutedEvent, value); }
            remove { base.RemoveHandler(NumberBoxValueChangeRoutedEvent, value); }
        }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class ValueChangeEventArge : RoutedEventArgs
    {
        public ValueChangeEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 变化前的值
        /// </summary>
        public object _ChangeBeforeValue { get; set; }
        /// <summary>
        /// 变化后的值
        /// </summary>
        public object _ChangeAfterValue { get; set; }
    }
}
