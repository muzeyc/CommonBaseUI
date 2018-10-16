using CommonBaseUI.CommonView;
using CommonBaseUI.CommUtil;
using CommonBaseUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyGridViewEx.xaml 的交互逻辑
    /// </summary>
    public partial class MyGridViewEx : UserControl
    {
        public const int _DefaultSize = 1000;
        private List<MyGridViewColumn> _Columns;
        public List<MyGridViewRow> _Rows = new List<MyGridViewRow>();
        private List<MyGridFuncButtonModel> _Buttons;
        public List<object> _DataSource;

        public MyGridViewEx()
        {
            InitializeComponent();
            SetPageButton();
        }

        #region 属性
        private int currentPage = 0;
        /// <summary>
        /// 当前页
        /// </summary>
        public int _CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                if (totalCount == 0)
                {
                    lblPage.Content = "0/0";
                }
                else
                {
                    int pageCount = GetPageCount();
                    lblPage.Content = string.Format("{0}/{1}", currentPage + 1, pageCount);
                }

                SetPageButton();
            }
        }

        private int size = _DefaultSize;
        /// <summary>
        /// 分页显示行数
        /// </summary>
        public int _Size
        {
            get
            {
                return size > 0 ? size : _DefaultSize;
            }
            set
            {
                size = value > 0 ? value : _DefaultSize;
            }
        }

        private int totalCount = 0;
        /// <summary>
        /// 数据总行数
        /// </summary>
        public int _TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                if (totalCount == 0)
                {
                    lblPage.Content = "0/0";
                }
                else
                {
                    int pageCount = GetPageCount();
                    lblPage.Content = string.Format("{0}/{1}", currentPage + 1, pageCount);
                }
            }
        }

        private bool isEnabled = true;
        /// <summary>
        /// 控件是否可用
        /// </summary>
        public bool _IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                SetButtonEnabled();
            }
        }

        private Dictionary<int, object> selectedItems;
        public List<object> _SelectedItems
        {
            get
            {
                var list = new List<object>();
                if (selectedItems != null)
                {
                    foreach (var item in selectedItems.Values)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }

        public double _MaxHeight
        {
            get
            {
                return pnlBodyScr.MaxHeight;
            }
            set
            {
                pnlBodyScr.MaxHeight = value;
            }
        }

        private SelectMode selectMode = SelectMode.MutiSelect;
        /// <summary>
        /// 选择模式
        /// </summary>
        public SelectMode _SelectMode
        {
            get
            {
                return selectMode;
            }
            set
            {
                selectMode = value;
            }
        }

        #endregion

        /// <summary>
        /// 创建GridView
        /// </summary>
        /// <param name="columns"></param>
        public void _CreateGrid(List<MyGridViewColumn> columns, List<MyGridFuncButtonModel> buttons = null)
        {
            // 设置列
            if (columns != null)
            {
                double totalWidth = 0;
                _Columns = columns;
                // 标记列
                var markHeak = new TextBlock();
                markHeak.Width = 20;
                pnlHead.Children.Add(markHeak);
                totalWidth += markHeak.Width;
                // 序号列
                var indexHead = new TextBlock();
                indexHead.Width = 30;
                pnlHead.Children.Add(indexHead);
                totalWidth += indexHead.Width;
                // 数据列
                foreach (var col in columns)
                {
                    var head = new MyLabel();
                    head.Width = col.Width;
                    head.TextWrapping = TextWrapping.Wrap;
                    head.Foreground = CommonUtil.ToBrush("#FFFFFF");
                    head.Text = col.ColumnName;
                    head.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    pnlHead.Children.Add(head);
                    totalWidth += head.Width;
                }
                pnlBody.Width = totalWidth;
            }

            // 设置按钮
            if (buttons != null)
            {
                _Buttons = buttons;
                for (int i = 0; i < _Buttons.Count; i++)
                {
                    var button = new MyButton();
                    button.Name = "btnFunc" + i;
                    button._Text = _Buttons[i].Text;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.Visibility = Visibility.Visible;
                    button._Height = 30;
                    button._IsEnabled = false;
                    button.Margin = new Thickness(1, 0, 0, 0);
                    //button.FontSize = 15;
                    button._Click += _Buttons[i].RoutedEventHandler;
                    pnlfunctionBar.Children.Add(button);
                    _Buttons[i].Button = button;
                    // 设置按钮是否亮起来
                    SetButtonEnabled(_Buttons[i]);
                }
            }
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dataSource"></param>
        public void _SetData<T>(List<T> dataSource, int totalCount, bool reset = false)
        {
            if (dataSource != null)
            {
                // 打开等待画面
                var form = new WaitingForm();
                form.Show();

                var thread = new Thread(new ParameterizedThreadStart(SetData<T>));
                var items = new object[4];
                items[0] = dataSource;
                items[1] = totalCount;
                items[2] = reset;
                items[3] = form;
                thread.Start(items);
            }
        }

        private void SetData<T>(object items)
        {
            var temp = items as object[];
            var dataSource = (List<T>)temp[0];
            int totalCount = temp[1].ToInt();
            var reset = (bool)temp[2];
            var waitingForm = temp[3] as WaitingForm;

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                _TotalCount = totalCount;
                _DataSource = new List<object>();
                _Rows = new List<MyGridViewRow>();
                selectedItems = new Dictionary<int, object>();
                pnlBody.Children.Clear();
                foreach (var item in dataSource)
                {
                    _DataSource.Add(item);
                }

                if (_Columns != null && _Columns.Count > 0)
                {
                    var dic = new Dictionary<int, MyGridViewColumn>();
                    for (int i = 0; i < _Columns.Count; i++)
                    {
                        dic[i] = _Columns[i];
                    }

                    Type t = typeof(T);

                    int rowCount = dataSource.Count < _Size ? dataSource.Count : _Size;
                    for (int i = 0; i < rowCount; i++)
                    {
                        insertAt(dataSource[i], i, dic, t);
                    }
                }

                SetButtonEnabled();
                if (reset)
                {
                    _CurrentPage = 0;
                }

            }));
            Thread.Sleep(500);

            CloseCallBack(waitingForm);
        }

        private void CloseCallBack(WaitingForm form)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                form.Close();
            }));
        }

        private void SetDataAt<T>(T obj, MyGridViewRow row)
        {
            row._SetConditon(obj);
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="index">小于0的时候插在最末行</param>
        public void _InsertAt<T>(T obj, int index)
        {
            var dic = new Dictionary<int, MyGridViewColumn>();
            for (int i = 0; i < _Columns.Count; i++)
            {
                dic[i] = _Columns[i];
            }
            Type t = typeof(T);

            insertAt(obj, index, dic, t);
            _DataSource = _DataSource ?? new List<object>();

            int j = index >= 0 ? index : _DataSource.Count;
            _DataSource.Insert(j, obj);
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="index">小于0的时候插在最末行</param>
        public void insertAt<T>(T obj, int index, Dictionary<int, MyGridViewColumn> dic, Type t)
        {
            if (_Rows == null)
            {
                _Rows = new List<MyGridViewRow>();
            }
            int i = index >= 0 ? index : _Rows.Count;

            var row = new MyGridViewRow(i);
            row._Index = i;
            row.MouseLeftButtonUp += Row_MouseLeftButtonUp;

            // 标记列
            var markCol = new Label();
            markCol.Width = 20;
            markCol.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            markCol.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            row.Children.Add(markCol);

            // 序号列
            var indexCol = new TextBlock();
            indexCol.Width = 30;
            indexCol.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            indexCol.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            indexCol.Text = (i + 1).ToString();
            row.Children.Add(indexCol);

            // 数据列
            for (int j = 0; j < dic.Keys.Count; j++)
            {
                string piName = dic[j].DisplayMemberBinding;

                switch (dic[j].ColumnType)
                {
                    case MyGridColumnType.LabelColumn:
                        {
                            // 文本列
                            var col = new MyGridViewCell();
                            col._InputWidth = dic[j].Width - 5;
                            col.MaxWidth = dic[j].Width - 5;
                            col._Binding = piName;
                            col.Margin = new Thickness(2, 2, 3, 2);
                            col.PreviewMouseLeftButtonDown += cell_MouseClick;
                            var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                            arge._RowIndex = row._Index;
                            arge._ColIndex = j;
                            arge._ColName = piName;
                            arge._Control = col;
                            arge._Item = obj;
                            RaiseEvent(arge);
                            row.Children.Add(col);
                            break;
                        }
                    case MyGridColumnType.ButtonColumn:
                        {
                            string val = t.GetProperty(piName).GetValue(obj, null).ToStr();
                            if (!val.IsNullOrEmpty() && !dic[j].ReadOnly)
                            {
                                // 按钮列
                                var col = new MyGridViewCellButton();
                                col._Width = dic[j].Width - 5;
                                col._Binding = val;
                                col._Text = val;
                                col.Margin = new Thickness(2, 2, 3, 2);
                                col.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                col.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                col._Click += CellButton_Click;
                                col._RowIndex = row._Index;
                                col._ColIndex = j;
                                col._ColName = piName;
                                col._IsEnabled = !dic[j].ReadOnly;
                                var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                                arge._RowIndex = row._Index;
                                arge._ColIndex = j;
                                arge._ColName = piName;
                                arge._Control = col;
                                arge._Item = obj;
                                RaiseEvent(arge);
                                row.Children.Add(col);
                            }
                            else
                            {
                                // 文本列
                                var col = new MyGridViewCell();
                                col._InputWidth = dic[j].Width - 5;
                                col.MaxWidth = dic[j].Width - 5;
                                col._Binding = piName;
                                col.Margin = new Thickness(2, 2, 3, 2);
                                col.PreviewMouseLeftButtonDown += cell_MouseClick;
                                var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                                arge._RowIndex = row._Index;
                                arge._ColIndex = j;
                                arge._ColName = piName;
                                arge._Control = col;
                                arge._Item = obj;
                                RaiseEvent(arge);
                                row.Children.Add(col);
                            }
                            break;
                        }
                    case MyGridColumnType.CheckBoxColumn:
                        {
                            var control = dic[j].InputControl;
                            // 单选框列
                            var col = new MyGridViewCellCheckBox();
                            col.Width = dic[j].Width - 10;
                            col.Margin = new Thickness(2, 2, 8, 2);
                            col.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            col.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            col.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                            col._RowIndex = row._Index;
                            col._ColIndex = j;
                            col._ColName = piName;
                            if (control != null)
                            {
                                var checkBox = control as MyGridViewCellCheckBox;
                                col.CheckedText = checkBox.CheckedText;
                                col.Text = checkBox.Text;
                                col._CheckValue = checkBox._CheckValue;
                                col._UnCheckValue = checkBox._UnCheckValue;
                            }

                            col._Binding = piName;
                            col.Checked += CellCheckBox_Checked;
                            col.Unchecked += CellCheckBox_Checked;
                            col._IsEnabled = !dic[j].ReadOnly;

                            var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                            arge._RowIndex = row._Index;
                            arge._ColIndex = j;
                            arge._ColName = piName;
                            arge._Control = col;
                            arge._Item = obj;
                            RaiseEvent(arge);
                            row.Children.Add(col);
                            break;
                        }
                    case MyGridColumnType.TextColumn:
                        {
                            if (!dic[j].ReadOnly)
                            {
                                // 文本框列
                                var col = new MyGridViewCellTextBox();
                                col._Binding = piName;
                                col._Caption = "";
                                col.Margin = new Thickness(2, 2, 5, 2);
                                col.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                col.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                col._RowIndex = row._Index;
                                col._ColIndex = j;
                                col._ColName = piName;
                                col.LostFocus += CellInput_LostFocus;
                                col._IsEnabled = !dic[j].ReadOnly;
                                col._InputWidth = dic[j].Width - 10;
                                var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                                arge._RowIndex = row._Index;
                                arge._ColIndex = j;
                                arge._ColName = piName;
                                arge._Control = col;
                                arge._Item = obj;
                                RaiseEvent(arge);
                                row.Children.Add(col);
                            }
                            else
                            {
                                // 文本列
                                var col = new MyGridViewCell();
                                col._InputWidth = dic[j].Width - 5;
                                col.MaxWidth = dic[j].Width - 5;
                                col._Binding = piName;
                                col.Margin = new Thickness(2, 2, 3, 2);
                                col.PreviewMouseLeftButtonDown += cell_MouseClick;
                                var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                                arge._RowIndex = row._Index;
                                arge._ColIndex = j;
                                arge._ColName = piName;
                                arge._Control = col;
                                arge._Item = obj;
                                RaiseEvent(arge);
                                row.Children.Add(col);
                            }
                            break;
                        }
                    case MyGridColumnType.CombBoxColumn:
                        {
                            var control = dic[j].InputControl;
                            // 下拉框列
                            var col = new MyGridViewCellCombBox();
                            col.Margin = new Thickness(2, 2, 5, 2);
                            col.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            col.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            col._RowIndex = row._Index;
                            col._ColIndex = j;
                            col._ColName = piName;
                            col._Caption = "";
                            if (control != null)
                            {
                                var combBox = control as MyGridViewCellCombBox;
                                col._SetListFromDataDic(combBox._GetList<DataDicModel>());
                            }

                            col._Binding = piName;
                            col._IsEnabled = !dic[j].ReadOnly;
                            col._InputWidth = dic[j].Width - 10;
                            var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                            arge._RowIndex = row._Index;
                            arge._ColIndex = j;
                            arge._ColName = piName;
                            arge._Control = col;
                            arge._Item = obj;
                            RaiseEvent(arge);

                            col._SelectChange += cmb_SelectionChanged;

                            row.Children.Add(col);
                            break;
                        }
                    case MyGridColumnType.NumberColumn:
                        {
                            if (!dic[j].ReadOnly)
                            {
                                // 文本框列
                                var col = new MyGridViewCellNumberBox();
                                col._Binding = piName;
                                col._Caption = "";
                                col.Margin = new Thickness(2, 2, 5, 2);
                                col.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                col.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                col._RowIndex = row._Index;
                                col._ColIndex = j;
                                col._ColName = piName;
                                col.LostFocus += CellInput_LostFocus;
                                col._IsEnabled = !dic[j].ReadOnly;
                                col._InputWidth = dic[j].Width - 10;
                                var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                                arge._RowIndex = row._Index;
                                arge._ColIndex = j;
                                arge._ColName = piName;
                                arge._Control = col;
                                arge._Item = obj;
                                RaiseEvent(arge);
                                row.Children.Add(col);
                            }
                            else
                            {
                                // 文本列
                                var col = new MyGridViewCell();
                                col._InputWidth = dic[j].Width - 5;
                                col.MaxWidth = dic[j].Width - 5;
                                col._Binding = piName;
                                col.Margin = new Thickness(2, 2, 3, 2);
                                col.PreviewMouseLeftButtonDown += cell_MouseClick;
                                var arge = new CellFormatEventArge(CellFormatRoutedEvent, this);
                                arge._RowIndex = row._Index;
                                arge._ColIndex = j;
                                arge._ColName = piName;
                                arge._Control = col;
                                arge._Item = obj;
                                RaiseEvent(arge);
                                row.Children.Add(col);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            pnlBody.Children.Add(row);
            _Rows.Insert(i, row);
            row._SetConditon(obj);
            if (index == 0)
            {
                pnlBody.MinWidth = pnlBodyScr.ActualWidth - 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ContextMenu GetRightButtonItem()
        {
            var contextMenu = new ContextMenu();
            var menu = new MenuItem();
            menu.Header = "复制(Copy)";
            menu.Click += btnCopy_Click;
            contextMenu.Items.Add(menu);

            return contextMenu;
        }

        /// <summary>
        /// 设置按钮是否亮起来
        /// </summary>
        private void SetButtonEnabled()
        {
            // 选择行，这个事件会被激活
            if (this._Buttons != null)
            {
                foreach (var button in this._Buttons)
                {
                    // 设置按钮是否亮起来
                    SetButtonEnabled(button);
                }
            }
        }

        /// <summary>
        /// 设置按钮是否亮起来
        /// </summary>
        /// <param name="button"></param>
        private void SetButtonEnabled(MyGridFuncButtonModel button)
        {
            if (_IsEnabled)
            {
                if (button.EnableSingleSelect == ButtonEnable.SingleSelect)
                {
                    button.Button._IsEnabled = _SelectedItems != null && _SelectedItems.Count == 1;
                }
                else if (button.EnableSingleSelect == ButtonEnable.MutiSelect)
                {
                    button.Button._IsEnabled = _SelectedItems != null && _SelectedItems.Count >= 1;
                }
                else if (button.EnableSingleSelect == ButtonEnable.HasData)
                {
                    button.Button._IsEnabled = _DataSource != null && _DataSource.Count >= 1;
                }
                else
                {
                    button.Button._IsEnabled = true;
                }
            }
            else
            {
                button.Button._IsEnabled = button.EnableSingleSelect == ButtonEnable.Always;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void _Refresh<T>()
        {
            var list = _GetData<T>();
            _SetData(list, _TotalCount);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void _RefreshAt<T>(int index, T item)
        {
            SetDataAt(item, _Rows[index]);
        }

        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MyGridViewRow _GetRow(int index)
        {
            if (_Rows != null && _Rows.Count > index)
            {
                return _Rows[index];
            }
            return null;
        }

        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="index"></param>
        public void _Remove(int index)
        {
            var selectList = _GetSelectRows();
            selectedItems.Remove(index);

            _Rows.RemoveAt(index);
            _DataSource.RemoveAt(index);

            pnlBody.Children.RemoveAt(index);

            for (int i = index; i < pnlBody.Children.Count; i++)
            {
                var row = pnlBody.Children[i] as MyGridViewRow;
                row._Index = i;
                var lab = row.Children[1] as TextBlock;
                lab.Text = (i + 1).ToString();
            }
            SetButtonEnabled();
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void _Clear()
        {
            pnlBody.Children.Clear();
            _DataSource = null;
            selectedItems = null;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void _ExportExcel()
        {

        }

        #region 私有方法



        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int GetPageCount()
        {
            if (_TotalCount == 0)
            {
                return 0;
            }

            int size = _Size == 0 ? _DefaultSize : _Size;
            int mo = _TotalCount % size;
            double temp = _TotalCount / size;
            int maxPage = Math.Floor(temp.ToDouble()).ToInt();
            if (mo > 0)
            {
                maxPage++;
            }

            return maxPage;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> _GetData<T>()
        {
            var list = new List<T>();
            if (_DataSource == null)
            {
                return list;
            }

            foreach (var row in _Rows)
            {
                var obj = (T)_DataSource[row._Index];
                row._GetConditon<T>(obj);
                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T _GetDataAt<T>(int index)
        {
            var t = typeof(T);
            var list = new List<T>();
            if (_DataSource == null || _DataSource.Count <= index)
            {
                return System.Activator.CreateInstance<T>();
            }

            var obj = (T)_DataSource[index];
            _Rows[index]._GetConditon<T>(obj);

            return obj;
        }

        /// <summary>
        /// 获取选择行的对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> _GetSelectItems<T>()
        {
            var list = new List<T>();
            if (_SelectedItems != null && _SelectedItems.Count > 0)
            {
                var temp = _Rows.Where(n => n._Selected);
                foreach (var row in temp)
                {
                    var obj = (T)_DataSource[row._Index];
                    row._GetConditon<T>(obj);
                    list.Add(obj);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取选择行的对象列表
        /// </summary>
        /// <returns></returns>
        public List<MyGridViewRow> _GetSelectRows()
        {
            var list = new List<MyGridViewRow>();
            if (_SelectedItems != null && _SelectedItems.Count > 0)
            {
                var temp = _Rows.Where(n => n._Selected);
                foreach (var row in temp)
                {
                    list.Add(row);
                }
            }

            return list;
        }

        #endregion

        #region 事件

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            var col = sender as MyLabel;
            Clipboard.SetDataObject(col._Value);
        }

        private void cell_MouseClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Row_MouseLeftButtonUp(sender, null);
        }

        #endregion

        #region 自定义事件

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent PageChangeButtonClickRoutedEvent = EventManager.RegisterRoutedEvent(
            "_PageChange", RoutingStrategy.Bubble, typeof(EventHandler<PageButtonClickEventArge>), typeof(MyGridViewEx));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _PageChange
        {
            add { base.AddHandler(PageChangeButtonClickRoutedEvent, value); }
            remove { base.RemoveHandler(PageChangeButtonClickRoutedEvent, value); }
        }

        /// <summary>
        /// 向后翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrePage_Click(object sender, RoutedEventArgs e)
        {
            if (_CurrentPage <= 0)
            {
                return;
            }
            _CurrentPage--;

            var arge = new PageButtonClickEventArge(PageChangeButtonClickRoutedEvent, this);
            arge._CurrentPage = _CurrentPage;
            arge._Size = _Size;
            arge._Offset = _CurrentPage * _Size;
            RaiseEvent(arge);
        }

        /// <summary>
        /// 向前翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            int maxPage = GetPageCount();
            if (_CurrentPage + 1 == maxPage)
            {
                btnNextPage._IsEnabled = false;
                btnPrePage._IsEnabled = true;
                return;
            }

            _CurrentPage++;
            var arge = new PageButtonClickEventArge(PageChangeButtonClickRoutedEvent, this);
            arge._CurrentPage = _CurrentPage;
            arge._Size = _Size;
            arge._Offset = _CurrentPage * _Size;
            RaiseEvent(arge);
        }

        /// <summary>
        /// 设置分页按钮是否可用
        /// </summary>
        private void SetPageButton()
        {
            if (this._Rows.Count <= 0)
            {
                btnPrePage._IsEnabled = false;
                btnNextPage._IsEnabled = false;
                return;
            }

            btnPrePage._IsEnabled = _CurrentPage > 0;

            int maxPage = GetPageCount();
            btnNextPage._IsEnabled = _CurrentPage + 1 < maxPage;
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent RowClickRoutedEvent = EventManager.RegisterRoutedEvent(
            "_RowClick", RoutingStrategy.Bubble, typeof(EventHandler<RowClickEventArge>), typeof(MyGridViewEx));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _RowClick
        {
            add { base.AddHandler(RowClickRoutedEvent, value); }
            remove { base.RemoveHandler(RowClickRoutedEvent, value); }
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Row_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MyGridViewRow row = null;
            if (sender is MyGridViewRow)
            {
                row = sender as MyGridViewRow;
            }
            else
            {
                row = (sender as FrameworkElement).Parent as MyGridViewRow;
            }

            foreach (var r in _Rows)
            {
                if (r._Index == row._Index)
                {
                    r._Selected = !row._Selected;
                }
                else
                {
                    if (selectMode == SelectMode.SingleSelect)
                    {
                        // 单选
                        r._Selected = false;
                    }
                }
            }

            selectedItems = new Dictionary<int, object>();
            for (int i = 0; i < _Rows.Count; i++)
            {
                if (_Rows[i]._Selected)
                {
                    selectedItems.Add(_Rows[i]._Index, _DataSource[i]);
                }
            }

            var arge = new RowClickEventArge(RowClickRoutedEvent, this);
            arge._RowIndex = row._Index;
            arge._Item = _DataSource[row._Index];
            RaiseEvent(arge);

            SetButtonEnabled();
        }

        /// <summary>
        /// 选中某一行
        /// </summary>
        /// <param name="index"></param>
        public void _SelectRow(int index)
        {
            if (_Rows != null && _Rows.Count > index)
            {
                _Rows[index]._Selected = true;

                selectedItems = new Dictionary<int, object>();
                for (int i = 0; i < _Rows.Count; i++)
                {
                    if (_Rows[i]._Selected)
                    {
                        selectedItems.Add(_Rows[i]._Index, _DataSource[i]);
                    }
                }

                SetButtonEnabled();
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CellButtonClickRoutedEvent = EventManager.RegisterRoutedEvent(
            "_CellButtonClick", RoutingStrategy.Bubble, typeof(EventHandler<CellButtonClickEventArge>), typeof(MyGridViewEx));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _CellButtonClick
        {
            add { base.AddHandler(CellButtonClickRoutedEvent, value); }
            remove { base.RemoveHandler(CellButtonClickRoutedEvent, value); }
        }

        /// <summary>
        /// 单元格内的按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as MyGridViewCellButton;
            var arge = new CellButtonClickEventArge(CellButtonClickRoutedEvent, this);
            arge._RowIndex = button._RowIndex;
            arge._Item = _DataSource[button._RowIndex];
            arge._ColIndex = button._ColIndex;
            arge._ColName = button._ColName;
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CellCheckBoxCheckRoutedEvent = EventManager.RegisterRoutedEvent(
            "_CellCheckBoxCheck", RoutingStrategy.Bubble, typeof(EventHandler<CellCheckBoxCheckEventArge>), typeof(MyGridViewEx));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _CellCheckBoxCheck
        {
            add { base.AddHandler(CellCheckBoxCheckRoutedEvent, value); }
            remove { base.RemoveHandler(CellCheckBoxCheckRoutedEvent, value); }
        }

        /// <summary>
        /// 单选按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as MyGridViewCellCheckBox;

            var arge = new CellCheckBoxCheckEventArge(CellCheckBoxCheckRoutedEvent, this);
            arge._RowIndex = checkBox._RowIndex;
            arge._ColIndex = checkBox._ColIndex;
            arge._ColName = checkBox._ColName;
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CellLostFocusRoutedEvent = EventManager.RegisterRoutedEvent(
            "_CellLostFocus", RoutingStrategy.Bubble, typeof(EventHandler<CellLostFocusEventArge>), typeof(MyGridViewEx));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _CellLostFocus
        {
            add { base.AddHandler(CellLostFocusRoutedEvent, value); }
            remove { base.RemoveHandler(CellLostFocusRoutedEvent, value); }
        }

        /// <summary>
        /// 单选按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is MyGridViewCellTextBox)
            {
                var textBox = sender as MyGridViewCellTextBox;
                var arge = new CellLostFocusEventArge(CellLostFocusRoutedEvent, this);
                arge._RowIndex = textBox._RowIndex;
                arge._ColIndex = textBox._ColIndex;
                arge._ColName = textBox._ColName;
                arge._TextBox = textBox;
                RaiseEvent(arge);
            }
            else if (sender is MyGridViewCellNumberBox)
            {
                var numberBox = sender as MyGridViewCellNumberBox;
                var arge = new CellLostFocusEventArge(CellLostFocusRoutedEvent, this);
                arge._RowIndex = numberBox._RowIndex;
                arge._ColIndex = numberBox._ColIndex;
                arge._ColName = numberBox._ColName;
                arge._NumberBox = numberBox;
                RaiseEvent(arge);
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CellCombSelectedRoutedEvent = EventManager.RegisterRoutedEvent(
            "_CellCombSelected", RoutingStrategy.Bubble, typeof(EventHandler<CellCombSelectedEventArge>), typeof(MyGridViewEx));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _CellCombSelected
        {
            add { base.AddHandler(CellCombSelectedRoutedEvent, value); }
            remove { base.RemoveHandler(CellCombSelectedRoutedEvent, value); }
        }

        private void cmb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var ex = e as CombBoxSelectionChangedEventArge;
            var combBox = sender as MyGridViewCellCombBox;
            var arge = new CellCombSelectedEventArge(CellCombSelectedRoutedEvent, this);
            arge._RowIndex = combBox._RowIndex;
            arge._ColIndex = combBox._ColIndex;
            arge._ColName = combBox._ColName;
            arge._Item = ex._item;
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent CellFormatRoutedEvent = EventManager.RegisterRoutedEvent(
            "_CellFormat", RoutingStrategy.Bubble, typeof(EventHandler<CellFormatEventArge>), typeof(MyGridViewEx));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _CellFormat
        {
            add { base.AddHandler(CellFormatRoutedEvent, value); }
            remove { base.RemoveHandler(CellFormatRoutedEvent, value); }
        }

        #endregion

        private void scr_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            pnlHeadScr.ScrollToHorizontalOffset(e.HorizontalOffset);
        }
    }

    /// <summary>
    /// 列的类型
    /// </summary>
    public enum MyGridColumnType
    {
        /// <summary>
        /// 文字列
        /// </summary>
        LabelColumn = 1,
        /// <summary>
        /// 文本输入列
        /// </summary>
        TextColumn = 2,
        /// <summary>
        /// 复选列
        /// </summary>
        CheckBoxColumn = 3,
        /// <summary>
        /// 下拉列
        /// </summary>
        CombBoxColumn = 4,
        /// <summary>
        /// 按钮列
        /// </summary>
        ButtonColumn = 5,
        /// <summary>
        /// 数字列
        /// </summary>
        NumberColumn = 6,
    }

    /// <summary>
    /// DataGrid功能按钮类
    /// </summary>
    public class MyGridFuncButtonModel
    {
        public MyGridFuncButtonModel()
        {
            // 没有空的构造函数，会有些问题，所以请保留
        }
        public MyGridFuncButtonModel(string text,
            RoutedEventHandler routedEventHandler,
            ButtonEnable enableSingleSelect = ButtonEnable.MutiSelect)
        {
            this.Text = text;
            this.RoutedEventHandler = routedEventHandler;
            this.EnableSingleSelect = enableSingleSelect;
        }
        /// <summary>
        /// 按钮显示的文字
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 按钮事件
        /// </summary>
        public RoutedEventHandler RoutedEventHandler { get; set; }
        /// <summary>
        /// true：只在单选时激活
        /// </summary>
        public ButtonEnable EnableSingleSelect { get; set; }
        /// <summary>
        /// 按钮
        /// </summary>
        public MyButton Button { get; set; }
    }

    public enum ButtonEnable
    {
        /// <summary>
        /// 单选可用
        /// </summary>
        SingleSelect = 1,
        /// <summary>
        /// 单选或多选可以
        /// </summary>
        MutiSelect = 2,
        /// <summary>
        /// 不受DataGrid本身的IsEnabled控制，一直可用
        /// </summary>
        Always = 3,
        /// <summary>
        /// 有数据时
        /// </summary>
        HasData = 4
    }


    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class PageButtonClickEventArge : RoutedEventArgs
    {
        public PageButtonClickEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        public int _CurrentPage { get; set; }
        public int _Size { get; set; }
        public int _Offset { get; set; }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class RowClickEventArge : RoutedEventArgs
    {
        public RowClickEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 该行的数据源
        /// </summary>
        public object _Item { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int _RowIndex { get; set; }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class CellButtonClickEventArge : RowClickEventArge
    {
        public CellButtonClickEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 列号
        /// </summary>
        public object _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public object _ColName { get; set; }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class CellCheckBoxCheckEventArge : RowClickEventArge
    {
        public CellCheckBoxCheckEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 列号
        /// </summary>
        public object _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public object _ColName { get; set; }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class CellLostFocusEventArge : RowClickEventArge
    {
        public CellLostFocusEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 列号
        /// </summary>
        public object _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public object _ColName { get; set; }
        public MyGridViewCellTextBox _TextBox { get; set; }
        public MyGridViewCellNumberBox _NumberBox { get; set; }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class CellCombSelectedEventArge : RowClickEventArge
    {
        public CellCombSelectedEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 列号
        /// </summary>
        public object _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public object _ColName { get; set; }
        /// <summary>
        /// 选择后的值
        /// </summary>
        public object _Item { get; set; }
    }

    /// <summary>
    /// 定义事件的参数类
    /// </summary>
    public class CellFormatEventArge : RowClickEventArge
    {
        public CellFormatEventArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 列号
        /// </summary>
        public object _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public object _ColName { get; set; }
        /// <summary>
        /// 当前控件
        /// </summary>
        public UIElement _Control { get; set; }
    }

    /// <summary>
    /// 选择模式
    /// </summary>
    public enum SelectMode
    {
        /// <summary>
        /// 单选
        /// </summary>
        SingleSelect = 1,
        /// <summary>
        /// 多选
        /// </summary>
        MutiSelect = 2,
    }

    public class MyGridViewCellButton : MyButton
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int _RowIndex { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        public int _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string _ColName { get; set; }
    }

    public class MyGridViewCellCheckBox : MyCheckBox
    {
        public MyGridViewCellCheckBox()
        {

        }

        public MyGridViewCellCheckBox(string checkedText, string unCheckedText, object checkValue, object unCheckedValue)
        {
            this.CheckedText = checkedText;
            this.Text = unCheckedText;
            this._CheckValue = checkValue;
            this._UnCheckValue = unCheckedValue;
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int _RowIndex { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        public int _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string _ColName { get; set; }
    }

    public class MyGridViewCellTextBox : MyTextBox
    {
        public MyGridViewCellTextBox()
        {

        }

        /// <summary>
        /// 行号
        /// </summary>
        public int _RowIndex { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        public int _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string _ColName { get; set; }
    }

    public class MyGridViewCellNumberBox : MyNumberBox
    {
        public MyGridViewCellNumberBox()
        {

        }

        /// <summary>
        /// 行号
        /// </summary>
        public int _RowIndex { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        public int _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string _ColName { get; set; }
    }

    public class MyGridViewCellCombBox : MyCombBox
    {
        public MyGridViewCellCombBox()
        {

        }

        public MyGridViewCellCombBox(List<DataDicModel> list)
        {
            this._SetListFromDataDic(list);
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int _RowIndex { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        public int _ColIndex { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string _ColName { get; set; }
    }
}
