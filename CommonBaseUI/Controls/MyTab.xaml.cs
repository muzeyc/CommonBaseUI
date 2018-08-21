using CommonBaseUI.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    /// <summary>
    /// MyTab.xaml 的交互逻辑
    /// </summary>
    public partial class MyTab : UserControl
    {
        private Assembly ControllerUIAssembly;
        public Dictionary<string, MyTabButton> _Items;

        public MyTab()
        {
            this._Items = new Dictionary<string, MyTabButton>();
            InitializeComponent();
        }

        /// <summary>
        /// 添加选项卡
        /// </summary>
        /// <param name="title"></param>
        /// <param name="item"></param>
        public void _AddItem(MyTabButton item)
        {
            if (!this._Items.ContainsKey(item._ContentFormName))
            {
                item._Index = this._Items.Count;
                item._TabClick += tab_Click;
                item._TabClosed += tab_Close;
                item._SelectionChange += tab_SelectionChange;
                this._Items.Add(item._ContentFormName, item);
                pnlItems.Children.Add(item);
            }
            else
            {
                _SelectTab(ControllerUIAssembly, item._ContentFormName);
            }
        }

        /// <summary>
        /// 选中选项
        /// </summary>
        /// <param name="formName"></param>
        public void _SelectTab(Assembly assembly, string formName)
        {
            ControllerUIAssembly = ControllerUIAssembly == null ? assembly : ControllerUIAssembly;
            bool hasIndex = false;
            MyTabButton btn = null;
            foreach (var item in this._Items)
            {
                if (formName.Equals(item.Value._ContentFormName) && (!item.Value._IsSelected || pnlBody.Children.Count == 0))
                {
                    btn = item.Value;
                    hasIndex = true;
                    break;
                }
            }

            if (!hasIndex)
            {
                return;
            }

            foreach (var item in this._Items)
            {
                if (!item.Value._ContentFormName.Equals(formName))
                {
                    item.Value._IsSelected = false;
                }
            }

            btn._IsSelected = true;
            pnlBody.Children.Clear();
            if (btn._ContentForm == null)
            {
                var form = assembly.CreateInstance(btn._ContentFormName, true);
                if (form == null)
                {
                    // 转换失败/画面不存在
                    return;
                }
                var contentForm = form as ContentControl;
                contentForm.Width = SystemParameters.WorkArea.Width - 150;
                //contentForm.Height = SystemParameters.WorkArea.Width - 70;
                btn._ContentForm = contentForm;
            }

            pnlBody.Children.Add(btn._ContentForm);
        }

        /// <summary>
        /// 关闭Tab
        /// </summary>
        /// <param name="formName"></param>
        public void _RemoveTab(string formName)
        {
            if (this._Items.ContainsKey(formName))
            {
                this._Items.Remove(formName);
                pnlBody.Children.Clear();
                ThreadManager._RemoveThread(formName);
            }

            foreach (var item in this._Items)
            {
                _SelectTab(ControllerUIAssembly, item.Value._ContentFormName);
                break;
            }

            for (int i = 0; i < pnlItems.Children.Count; i++)
            {
                var button = pnlItems.Children[i] as MyTabButton;
                if (button._ContentFormName.Equals(formName))
                {
                    pnlItems.Children.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent(
            "_ItemClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyTab));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _ItemClick
        {
            add { base.AddHandler(ItemClickEvent, value); }
            remove { base.RemoveHandler(ItemClickEvent, value); }
        }

        /// <summary>
        /// 选中Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tab_Click(object sender, RoutedEventArgs e)
        {
            var tabButton = sender as MyTabButton;
            _SelectTab(ControllerUIAssembly, tabButton._ContentFormName);

            var arge = new RoutedEventArgs(ItemClickEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 关闭Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tab_Close(object sender, RoutedEventArgs e)
        {
            var tabButton = sender as MyTabButton;
            _RemoveTab(tabButton._ContentFormName);

            var arge = new RoutedEventArgs(ItemClickEvent, this);
            RaiseEvent(arge);

            var argeChange = new MyTabSelectionChangeArge(TabSelectionChangeEvent, this);
            argeChange._FormName = tabButton._ContentFormName;
            argeChange._IsActive = false;
            RaiseEvent(argeChange);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent TabSelectionChangeEvent = EventManager.RegisterRoutedEvent(
            "_MyTabSelectionChange", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MyTab));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _SelectionChange
        {
            add { base.AddHandler(TabSelectionChangeEvent, value); }
            remove { base.RemoveHandler(TabSelectionChangeEvent, value); }
        }

        /// <summary>
        /// 列表选中状态变化时的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tab_SelectionChange(object sender, RoutedEventArgs e)
        {
            var tabButton = sender as MyTabButton;

            var arge = new MyTabSelectionChangeArge(TabSelectionChangeEvent, this);
            arge._Form = tabButton._ContentForm;
            arge._FormName = tabButton._ContentFormName;
            arge._IsActive = tabButton._IsSelected;
            RaiseEvent(arge);
        }
    }

    public class MyTabSelectionChangeArge : RoutedEventArgs
    {
        public MyTabSelectionChangeArge(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {

        }
        /// <summary>
        /// 该标签页要显示的画面
        /// </summary>
        public ContentControl _Form { get; set; }
         /// <summary>
        /// 该标签页要显示的画面的名称，包括命名空间
        /// </summary>
        public string _FormName { get; set; }
        /// <summary>
        /// 选中状态,true:当前tab激活，false:当前tab未激活
        /// </summary>
        public bool _IsActive { get; set; } 
    }
}
