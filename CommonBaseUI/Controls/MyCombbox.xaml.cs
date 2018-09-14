using CommonBaseUI.CommUtil;
using CommonBaseUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyCombbox.xaml 的交互逻辑
    /// </summary>
    public partial class MyCombBox : UserControl, IInputControl, INotifyPropertyChanged
    {
        private List<DataDicModel> List;
        bool HasBlankItem = true;
        public MyCombBox()
        {
            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public object _Value
        {
            get
            {
                return cmbInput.SelectedValue;
            }
            set
            {
                bool containVal = false;
                if (cmbInput.ItemsSource != null)
                {
                    foreach (var obj in cmbInput.ItemsSource)
                    {
                        object val = obj.GetType().GetProperty(cmbInput.SelectedValuePath).GetValue(obj, null);
                        if (val.ToStr().Equals(value.ToStr()))
                        {
                            containVal = true;
                            break;
                        }
                    }
                }
                if (PropertyChanged != null && !cmbInput.SelectedValue.ToStr().Equals(value.ToStr()))
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("_Value"));//对_Value进行监听  
                }
                cmbInput.SelectedValue = containVal ? value : null;
            }
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
                btnClear.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                btnClear.Width = value ? 20 : 0;
                if (!_IsEnabled)
                {
                    cmbInput.Width = inputWidth;
                }
                else
                {
                    cmbInput.Width = inputWidth - 18;
                }
                cmbInput.Margin = value ? new Thickness(2, 2, 20, 2) : new Thickness(2);
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
                //else
                //{
                //    var color = ColorTranslator.FromHtml("#EBEBEB");
                //    SolidColorBrush myBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));

                //    var colorW = ColorTranslator.FromHtml("#000000");
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
            this.cmbInput.Background = CommonUtil.ToBrush("#FA8072");
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
                return cmbInput.Width;
            }
            set
            {
                double width = value > 20 ? value : inputWidth;
                inputWidth = width;
                if (_IsEnabled)
                {
                    cmbInput.Width = width - 20;
                }
                else
                {
                    cmbInput.Width = width;
                }
            }
        }

        public string _Text
        {
            get
            {
                if (cmbInput.ItemsSource != null)
                {
                    foreach (var obj in cmbInput.ItemsSource)
                    {
                        object val = obj.GetType().GetProperty(cmbInput.SelectedValuePath).GetValue(obj, null);
                        if (val.ToStr().Equals(_Value))
                        {
                            object displayName = obj.GetType().GetProperty(cmbInput.DisplayMemberPath).GetValue(obj, null);
                            return displayName.ToStr();
                        }
                    }
                }

                return string.Empty;
            }
        }

        public bool _IsEditable
        {
            get
            {
                return cmbInput.IsEditable;
            }
            set
            {
                cmbInput.IsEditable = value;
            }
        }

        /// <summary>
        /// 清除输入框背景色
        /// </summary>
        public void _CleanErr()
        {
            this.cmbInput.Background = CommonUtil.ToBrush("#FFFFFF");
        }

        /// <summary>
        /// 填充下拉列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SelectedValuePath"></param>
        /// <param name="DisplayMemberPath"></param>
        /// <param name="list"></param>
        /// <param name="hasBlank">是否有空选项</param>
        public void _SetList<T>(string SelectedValuePath, string DisplayMemberPath, List<T> list, bool hasBlank = true)
        {
            this.HasBlankItem = hasBlank;
            _Value = null;
            if (hasBlank)
            {
                T blank = System.Activator.CreateInstance<T>();
                blank.GetType().GetProperty(SelectedValuePath).SetValue(blank, "", null);
                list.Insert(0, blank);
            }

            cmbInput.SelectedValuePath = SelectedValuePath;
            cmbInput.DisplayMemberPath = DisplayMemberPath;
            cmbInput.ItemsSource = list;
        }

        public List<T> _GetList<T>()
        {
            var list = new List<T>();
            foreach (var item in cmbInput.Items)
            {
                T model = System.Activator.CreateInstance<T>();
                var properties = model.GetType().GetProperties();
                foreach (var p in properties)
                {
                    model.GetType().GetProperty(p.Name).SetValue(model, p.GetValue(item, null), null);
                }
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 填充下拉列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void _SetListFromDataDic(List<DataDicModel> list, bool hasBlank = true)
        {
            this.HasBlankItem = hasBlank;
            _Value = null;
            if (list != null)
            {
                if (hasBlank && list.Count > 0 && !list[0].name.IsNullOrEmpty() && !list[0].val.IsNullOrEmpty())
                {
                    var modelB = new DataDicModel();
                    modelB.val = "";
                    modelB.name = "";
                    list.Insert(0, modelB);
                }

                cmbInput.SelectedValuePath = "val";
                cmbInput.DisplayMemberPath = "name";
                cmbInput.ItemsSource = list;
                this.List = _GetList<DataDicModel>();
            }
        }

        /// <summary>
        /// 填充下拉列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void _SetListFromDataDic(Dictionary<string, string> dic, bool hasBlank = true)
        {
            this.HasBlankItem = hasBlank;
            _Value = null;

            var list = new List<DataDicModel>();
            foreach (var item in dic)
            {
                list.Add(new DataDicModel { val = item.Key, name = item.Value });
            }
            _SetListFromDataDic(list, hasBlank);
        }

        /// <summary>
        /// 填充下拉列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void SetListFromDataDic(List<DataDicModel> list, bool hasBlank = true)
        {
            _Value = null;
            if (list != null)
            {
                if (hasBlank && list.Count > 0 && !list[0].name.IsNullOrEmpty() && !list[0].val.IsNullOrEmpty())
                {
                    var modelB = new DataDicModel();
                    modelB.val = "";
                    modelB.name = "";
                    list.Insert(0, modelB);
                }

                cmbInput.SelectedValuePath = "val";
                cmbInput.DisplayMemberPath = "name";
                cmbInput.ItemsSource = list;
            }
        }

        /// <summary>
        /// 填充下拉列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void _SetListFromDataTable(DataTable dt, string valueCol, string dispCol = null, bool hasBlank = true)
        {
            this.HasBlankItem = hasBlank;
            _Value = null;
            if (dt != null)
            {
                var list = new List<DataDicModel>();
                if (hasBlank && !list[0].name.IsNullOrEmpty() && !list[0].val.IsNullOrEmpty())
                {
                    var modelB = new DataDicModel();
                    modelB.val = "";
                    modelB.name = "";
                    list.Insert(0, modelB);
                }

                dispCol = dispCol.IsNullOrEmpty() ? valueCol : dispCol;
                foreach (DataRow row in dt.Rows)
                {
                    var model = new DataDicModel();
                    model.val = row[valueCol].ToStr();
                    model.name = row[dispCol].ToStr();
                    list.Add(model);
                }

                cmbInput.SelectedValuePath = "val";
                cmbInput.DisplayMemberPath = "name";
                cmbInput.ItemsSource = list;
                this.List = _GetList<DataDicModel>();
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent SelectChangeEvent = EventManager.RegisterRoutedEvent(
            "_SelectChange", RoutingStrategy.Bubble, typeof(EventHandler<CombBoxSelectionChangedEventArge>), typeof(MyCombBox));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _SelectChange
        {
            add { base.AddHandler(SelectChangeEvent, value); }
            remove { base.RemoveHandler(SelectChangeEvent, value); }
        }

        private void cmbInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("_Value"));//对_Value进行监听  
            }

            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var arge = new CombBoxSelectionChangedEventArge(SelectChangeEvent, this);
                arge._item = e.AddedItems[0];
                RaiseEvent(arge);
            }
        }

        private void txb_StaffID_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            string inputVal = textBox.Text.ToStr().Trim();
            if (inputVal.IsNullOrEmpty())
            {
                _SetListFromDataDic(this.List, this.HasBlankItem);
            }

            var newList = new List<DataDicModel>();

            foreach (var model in this.List)
            {
                if (model.name.IndexOf(inputVal) >= 0)
                {
                    newList.Add(new DataDicModel { val = model.val, name = model.name });
                }
            }

            SetListFromDataDic(newList, this.HasBlankItem);
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void _Clear()
        {
            _Value = string.Empty;
            if (cmbInput.IsEditable)
            {
                cmbInput.Text = string.Empty;
            }
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
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class CombBoxSelectionChangedEventArge : RoutedEventArgs
    {
        public CombBoxSelectionChangedEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 选择后的值
        /// </summary>
        public object _item { get; set; }
    }
}
