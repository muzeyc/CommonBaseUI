using CommonBaseUI.Common;
using CommonBaseUI.Controls;
using CommonBaseUI.Model;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.CommonView
{
    /// <summary>
    /// DialogForm.xaml 的交互逻辑
    /// </summary>
    public partial class DialogForm : UserControl
    {
        public DialogForm()
        {
            InitializeComponent();
        }
        public DialogModel _Model { get; set; }
        MessageBoxButton Button;
        public DialogForm(MessageBoxButton button, MessageBoxImage image, DialogModel model)
        {
            this.Button = button;

            InitializeComponent();
            _Model = model;
            lblMessage.Text = model._Message;

            if (MessageBoxImage.Error.Equals(image))
            {
                pnlBack.Background = CommonBaseUI.CommUtil.CommonUtil.ToBrush("#CD5555");
            }

            if (button == MessageBoxButton.OK)
            {
                var btn = new MyButton();
                btn._Text = "OK";
                btn._Height = 30;
                btn._Width = 70;
                btn._Click += OK_Click;
                btnList.Children.Add(btn);
            }

            else if (button == MessageBoxButton.YesNo)
            {
                var btn1 = new MyButton();
                btn1._Text = "Yes";
                btn1.Height = 30;
                btn1.Width = 70;
                btn1.Margin = new Thickness(10, 0, 10, 0);
                btn1._Click += OK_Click;
                btnList.Children.Add(btn1);

                var btn2 = new MyButton();
                btn2._Text = "No";
                btn2._Height = 30;
                btn2._Width = 70;
                btn2.Margin = new Thickness(10, 0, 10, 0);
                btn2._Click += Cancel_Click;
                btnList.Children.Add(btn2);
            }
        }

        public void _SetData<T>(T model)
        {
            var m = model as DialogModel;
            this.lblMessage.Text = m._Message;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Button == MessageBoxButton.OK)
            {
                _Model._Result = MessageBoxResult.OK;
            }
            else if (Button == MessageBoxButton.YesNo)
            {
                _Model._Result = MessageBoxResult.Yes;
            }
            FormCommon.CloseForm(this);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _Model._Result = MessageBoxResult.No;
            FormCommon.CloseForm(this);
        }
    }
}
