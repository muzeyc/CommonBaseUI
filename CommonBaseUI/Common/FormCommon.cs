using CommonBaseUI.CommonView;
using CommonBaseUI.CommUtil;
using CommonBaseUI.Controls;
using CommonBaseUI.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Common
{
    public class FormCommon
    {
        /// <summary>
        /// 检索类型的提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public static T GetSearch<T>(string url, Dictionary<string, string> paramDic = null)
        {
            try
            {
                var json = HttpUtil.Get(StaticClass._URL_HEAD + url);
                var res = JsonUtil.DeSerializer<T>(json);

                if (res == null)
                {
                    ShowErr("检索失败！");
                    return System.Activator.CreateInstance<T>();
                }

                var baseRes = res as ResponseModelBase;
                if (!ResponseModelBase.SUCCESSED.Equals(baseRes.result))
                {
                    var msg = new StringBuilder();
                    msg.AppendLine("检索失败！");
                    if (!baseRes.errMessage.IsNullOrEmpty())
                    {
                        msg.AppendLine("原因：" + baseRes.errMessage);
                    }

                    ShowErr(msg.ToString());
                }
                return res;
            }
            catch (Exception e)
            {
                ShowErr("服务器端错误！" + e.Message);
            }

            return System.Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 检索类型的提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public static T PostSearch<T>(string url, object reqModel)
        {
            try
            {
                var json = HttpUtil.Post(StaticClass._URL_HEAD + url, reqModel.Serializer());
                var res = JsonUtil.DeSerializer<T>(json);

                if (res == null)
                {
                    ShowErr("检索失败！");
                    return System.Activator.CreateInstance<T>();
                }

                var baseRes = res as ResponseModelBase;
                if (!ResponseModelBase.SUCCESSED.Equals(baseRes.result))
                {
                    var msg = new StringBuilder();
                    msg.AppendLine("检索失败！");
                    if (!baseRes.errMessage.IsNullOrEmpty())
                    {
                        msg.AppendLine("原因：" + baseRes.errMessage);
                    }
                    ShowErr(msg.ToString());
                }

                return res;
            }
            catch
            {
                ShowErr("服务器端错误！");
            }

            return System.Activator.CreateInstance<T>();
        }

        private static string GetPostJson(string url, object reqModel)
        {
            try
            {
                var json = HttpUtil.Post(url, reqModel.Serializer());

                return json;
            }
            catch
            {
                ShowErr("服务器端错误！");
            }

            return "";
        }

        /// <summary>
        /// 保存类型的提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <param name="subForm"></param>
        /// <returns></returns>
        public static T PostSave<T>(string url, object reqModel)
        {
            try
            {
                var json = HttpUtil.Post(StaticClass._URL_HEAD + url, reqModel.Serializer());
                var res = JsonUtil.DeSerializer<T>(json);

                if (res == null)
                {
                    ShowErr("保存失败！");
                    return System.Activator.CreateInstance<T>();
                }

                var baseRes = res as ResponseModelBase;
                if (ResponseModelBase.SUCCESSED.Equals(baseRes.result))
                {
                    ShowMessage("保存成功！");
                }
                else
                {
                    var msg = new StringBuilder();
                    msg.AppendLine("保存失败！");
                    if (!baseRes.errMessage.IsNullOrEmpty())
                    {
                        msg.AppendLine("原因：" + baseRes.errMessage);
                    }

                    ShowErr(msg.ToString());
                }
                return res;
            }
            catch
            {
                ShowErr("服务器端错误！");
            }

            return System.Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title"></param>
        /// <param name="view"></param>
        /// <param name="onDialogCloseCallBack"></param>
        public static void ShowForm(string title, UserControl view, object item = null, MyWindow.AfterCloseDelegate afterClose = null)
        {
            try
            {
                var window = new MyWindow(item, afterClose, true);
                window.lblTitle.Content = title;
                window.pnlBody.Width = view.Width;
                window.pnlBody.Height = view.Height + 30;
                window.pnlBody.Children.Add(view);
                window.pnlHead.Width = view.Width;

                window.Show();
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title"></param>
        /// <param name="view"></param>
        /// <param name="onDialogCloseCallBack"></param>
        public static void ShowDialogForm(string title, UserControl view, object item = null, MyWindow.AfterCloseDelegate afterClose = null)
        {
            try
            {
                var window = new MyWindow(item, afterClose, false);
                window.lblTitle.Content = title;
                window.pnlBody.Width = view.Width;
                window.pnlBody.Height = view.Height;
                window.pnlBody.Children.Add(view);
                window.pnlHead.Width = view.Width;

                window.Show();
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="view"></param>
        public static void CloseForm(UserControl view)
        {
            var window = GetWindow(view);
            window._Close();
            Application.Current.MainWindow.Activate();
        }

        /// <summary>
        /// 关闭画面
        /// </summary>
        public static void CloseForm(WaitingForm window)
        {
            if (window != null)
            {
                window.Close();
                Application.Current.MainWindow.Activate();
            }
        }

        /// <summary>
        /// 弹出信息对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="subForm"></param>
        public static void ShowDialog(string message, MyWindow.AfterCloseDelegate afterClose)
        {
            var model = new DialogModel();
            model._Message = message;
            var form = new DialogForm(MessageBoxButton.YesNo, MessageBoxImage.Question, model);
            //form.Width = 400;
            form.Height = 160;
            ShowDialogForm("提示信息", form, model, afterClose);
        }
        
        /// <summary>
        /// 弹出信息对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="subForm"></param>
        public static void ShowMessage(string message, MyWindow.AfterCloseDelegate afterClose = null)
        {
            var model = new DialogModel();
            model._Message = message;
            var form = new DialogForm(MessageBoxButton.OK, MessageBoxImage.Information, model);
            //form.Width = 400;
            form.Height = 160;
            ShowDialogForm("提示信息", form, model, afterClose);
        }

        /// <summary>
        /// 弹出信息对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="subForm"></param>
        public static void ShowErr(string message, MyWindow.AfterCloseDelegate afterClose = null)
        {
            var model = new DialogModel();
            model._Message = message;
            var form = new DialogForm(MessageBoxButton.OK, MessageBoxImage.Error, model);
            //form.Width = 400;
            form.Height = 160;
            ShowDialogForm("错误信息", form, model, afterClose);
        }

        /// <summary>
        /// 获取最外层父级元素
        /// </summary>
        /// <param name="subForm"></param>
        /// <returns></returns>
        private static MyWindow GetWindow(FrameworkElement subForm)
        {
            if (subForm is MyWindow)
            {
                return subForm as MyWindow;
            }

            var parent = subForm.Parent;
            if (parent is MyWindow)
            {
                return parent as MyWindow;
            }
            else
            {
                if (parent is FrameworkElement)
                {
                    return GetWindow(parent as FrameworkElement);
                }
            }

            return null;
        }

        /// <summary>
        /// 获取子画面最外层父级元素
        /// </summary>
        /// <param name="subForm"></param>
        /// <returns></returns>
        private static SubFormPanel GetSubFormPanel(FrameworkElement subForm)
        {
            var parent = subForm.Parent;
            if (parent is SubFormPanel)
            {
                return parent as SubFormPanel;
            }
            else
            {
                if (parent is FrameworkElement)
                {
                    return GetSubFormPanel(parent as FrameworkElement);
                }
            }

            return null;
        }
    }
}
