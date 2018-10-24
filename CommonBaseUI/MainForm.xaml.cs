using CommonBaseUI.CommUtil;
using CommonBaseUI.Controls;
using CommonBaseUI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Util.Controls;

namespace CommonBaseUI
{
    /// <summary>
    /// MainForm.xaml 的交互逻辑
    /// </summary>
    public partial class MainForm : UserControl
    {
        private const int rowHeight = 25;
        private Assembly ControllerUIAssembly;
        private MenuButton _SelectedParent { get; set; }
        private MenuButton _SelectedChild { get; set; }

        public MainForm()
        {
            InitializeComponent();
            img.Source = ImageUtil.GetImageFromResource(typeof(ResourceImage).FullName, "User_96px");
        }

        public MainForm(string menuJson, string urlHead)
        {
            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;
            InitializeComponent();
            StaticClass._URL_HEAD = urlHead;
            var list = JsonUtil.DeSerializer<List<MenuModel>>(menuJson);
            Init(list);

            img.Source = ImageUtil.GetImageFromResource(typeof(ResourceImage).FullName, "User_96px");
            //ControllerUIAssembly = Assembly.LoadFile("C://PFMDView/ControllerUI.dll");
            ControllerUIAssembly = Assembly.Load("ControllerUI");
        }

        /// <summary>
        /// 设置显示的用户名
        /// </summary>
        /// <param name="userName"></param>
        public void _SetUserName(string userName)
        {
            lblUserName.Content = userName;
        }

        /// <summary>
        /// 设置头像
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="userName"></param>
        public void _SetUserPhoto(string userCode, string userName)
        {
            img.Source = ImageUtil.GetImageFromResource(typeof(ResourceImage).FullName, "User_96px");
            //string uri = string.Format("{0}PersonImg/{1}_{2}.jpg", StaticClass._FILE_URL_HEAD, userCode, userName);
            //try
            //{
            //    string result = HttpUtil.Get(uri);
            //    if (result.IndexOf("404") >= 0)
            //    {
            //        img.Source = ImageUtil.GetImageFromResource(typeof(ResourceImage).FullName, "User_96px");
            //    }
            //    else
            //    {
            //        img.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(uri));
            //    }
            //}
            //catch
            //{
            //    img.Source = ImageUtil.GetImageFromResource(typeof(ResourceImage).FullName, "User_96px");
            //}
        }

        public MyTab _GetTab()
        {
            return tabBody;
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        private void Init(List<MenuModel> menuList)
        {
            int i = 1;
            foreach (var model in menuList)
            {
                var parentBtn = new MenuButton();
                parentBtn._MenuInfo = model;
                parentBtn._MenuInfo.id = i.ToString();
                parentBtn.FIcon = model.iconName.IsNullOrEmpty() ? parentBtn.FIcon : StringToUnicode(model.iconName);
                parentBtn.Width = pnlList.Width;
                parentBtn.Content = model.menuTitle;
                parentBtn.Background = CommonUtil.ToBrush("#4b4b4b");
                parentBtn.Foreground = CommonUtil.ToBrush("#FFFFFF");
                parentBtn.MouseOverBackground = CommonUtil.ToBrush("#4b4b4b");
                parentBtn.MouseOverForeground = CommonUtil.ToBrush("#FFFFFF");
                parentBtn.PressedBackground = CommonUtil.ToBrush("#000000");
                parentBtn.PressedForeground = CommonUtil.ToBrush("#FFFFFF");
                parentBtn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                parentBtn.FontSize = 11;
                parentBtn.Height = rowHeight;
                parentBtn.Click += ParentClick;
                pnlList.Children.Add(parentBtn);

                var border = new SubMenuList();
                border.Background = CommonUtil.ToBrush("#4a4a4a");
                border._ParentId = parentBtn._MenuInfo.id;
                border.Height = 0;
                int j = 1;
                foreach (var subItem in model.subList)
                {
                    var childBtn = new MenuButton();
                    childBtn._MenuInfo = subItem;
                    childBtn._MenuInfo.id = string.Format("{0}-{1}", i, j);
                    childBtn.FIcon = subItem.iconName.IsNullOrEmpty() ? parentBtn.FIcon : StringToUnicode(subItem.iconName);
                    childBtn.Width = pnlList.Width;
                    childBtn.Content = subItem.menuTitle;
                    childBtn.Margin = new Thickness(15, 0, 0, 0);
                    childBtn.Background = CommonUtil.ToBrush("#4a4a4a");
                    childBtn.Foreground = CommonUtil.ToBrush("#FFFFFF");
                    childBtn.MouseOverBackground = CommonUtil.ToBrush("#4b4b4b");
                    childBtn.MouseOverForeground = CommonUtil.ToBrush("#FFFFFF");
                    childBtn.PressedBackground = CommonUtil.ToBrush("#000000");
                    childBtn.PressedForeground = CommonUtil.ToBrush("#FFFFFF");
                    childBtn.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                    childBtn.FontSize = 11;
                    childBtn.Height = rowHeight;
                    childBtn.Click += ChildClick;
                    border.Children.Add(childBtn);
                    j++;
                }

                pnlList.Children.Add(border);

                i++;
            }

            if (pnlList.Children.Count > 0)
            {
                ParentClick(pnlList.Children[0], null);
            }
        }

        /// <summary>    
        /// 字符串转为UniCode码字符串    
        /// </summary>    
        /// <param name="s"></param>    
        /// <returns></returns>    
        private string StringToUnicode(string s)
        {
            return System.Web.HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// 一级菜单点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentClick(object sender, EventArgs e)
        {
            var button = sender as MenuButton;
            if (_SelectedParent != null && _SelectedParent._MenuInfo.id.Equals(button._MenuInfo.id))
            {
                foreach (var col in pnlList.Children)
                {
                    if (col is SubMenuList)
                    {
                        var subMenuList = col as SubMenuList;

                        if (subMenuList._ParentId.Equals(button._MenuInfo.id))
                        {
                            if (subMenuList._Opened)
                            {
                                ShowOrHideChild(subMenuList, subMenuList.Height, 0);
                            }
                            else
                            {
                                ShowOrHideChild(subMenuList, 0, subMenuList.Children.Count * rowHeight);
                            }
                        }
                    }
                }
                return;
            }

            _SelectedParent = button;

            foreach (var col in pnlList.Children)
            {
                if (col is SubMenuList)
                {
                    var subMenuList = col as SubMenuList;

                    if (subMenuList._ParentId.Equals(button._MenuInfo.id))
                    {
                        ShowOrHideChild(subMenuList, 0, subMenuList.Children.Count * rowHeight);
                    }
                    else
                    {
                        if (subMenuList.Height > 0)
                        {
                            ShowOrHideChild(subMenuList, subMenuList.Height, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 展开/折叠
        /// </summary>
        /// <param name="subMenuList"></param>
        private void ShowOrHideChild(SubMenuList subMenuList, double from, double to)
        {
            subMenuList._Opened = to > 0;
            //实例化一个DoubleAninmation对象
            DoubleAnimation myani = new DoubleAnimation();
            //开始值
            myani.From = from;
            //结束值
            myani.To = to;
            //所用时间
            myani.Duration = TimeSpan.FromMilliseconds(300);
            //设置应用的对象
            Storyboard.SetTarget(myani, subMenuList);
            //设置应用的依赖项属性
            Storyboard.SetTargetProperty(myani, new PropertyPath("Height"));
            // 实例化一个故事板
            Storyboard s = new Storyboard();
            //s.Completed += new EventHandler(ShowChildConmplete);
            //将先前动画添加进来
            s.Children.Add(myani);
            //启动故事版
            s.Begin();
        }

        /// <summary>
        /// 响应动画完成事件(虽然没有用到，但是很有用，所以保留)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowChildConmplete(object sender, EventArgs e)
        {
            //获取当前的ClockGroup对象
            var clockgroup = (ClockGroup)sender;
            // 获取当前的DoubleAnimation对象
            var mys = ((DoubleAnimation)clockgroup.Children[0].Timeline);
            //获取当前按钮对象
            var subMenuList = (SubMenuList)Storyboard.GetTarget(mys);
            //实例化一个DoubleAninmation对象
            var myani = new DoubleAnimation();
            //开始值
            myani.From = subMenuList.Height;
            //忽略结束值
            //所用时间
            myani.Duration = TimeSpan.FromSeconds(1);
            //设置应用的对象
            Storyboard.SetTarget(myani, subMenuList);
            //设置应用的依赖项属性
            Storyboard.SetTargetProperty(myani, new PropertyPath("Height"));
            // 实例化一个故事板
            Storyboard s = new Storyboard();
            //将先前动画添加进来
            s.Children.Add(myani);
            //启动故事版
            s.Begin();
        }

        /// <summary>
        /// 子菜单点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChildClick(object sender, EventArgs e)
        {
            var button = sender as MenuButton;
            if (_SelectedChild != null && _SelectedChild._MenuInfo.Equals(button._MenuInfo.id) && tabBody._Items.Count > 0)
            {
                return;
            }

            _SelectedChild = button;

            var tabButton = new MyTabButton();
            tabButton._ContentFormName = button._MenuInfo.url;
            tabButton._Text = button._MenuInfo.menuTitle;
            tabBody._AddItem(tabButton);
            tabBody._SelectTab(ControllerUIAssembly, button._MenuInfo.url);
        }

        private class MenuButton : FButton
        {
            public MenuModel _MenuInfo { get; set; }
        }

        private class SubMenuList : StackPanel
        {
            public string _ParentId { get; set; }
            public bool _Opened { get; set; }
        }

        private void img_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var arge = new RoutedEventArgs(UserClickRoutedEvent, this);
            RaiseEvent(arge);
        }

        /// <summary>
        /// 定义和注册事件
        /// </summary>
        public static readonly RoutedEvent UserClickRoutedEvent = EventManager.RegisterRoutedEvent(
            "_UserClick", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(MainForm));

        /// <summary>
        /// 定义传统事件包装
        /// </summary>
        public event RoutedEventHandler _UserClick
        {
            add { base.AddHandler(UserClickRoutedEvent, value); }
            remove { base.RemoveHandler(UserClickRoutedEvent, value); }
        }
    }


}
