using CommonBaseUI.CommonView;
using CommonBaseUI.CommUtil;
using CommonBaseUI.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace CommonBaseUI.Common
{
    public class RequestCommon
    {
        private delegate void CloseWaitingDelegate(WaitingForm form);
        private CloseWaitingDelegate _CloseWaiting;

        public delegate void AfterResponseDelegate(object res);
        private AfterResponseDelegate _AfterResponse;

        public delegate void BeforeSaveDelegate();
        private BeforeSaveDelegate _BeforeSave;

        private string Url = "";
        private object ReqModel = null;

        /// <summary>
        /// 请求返回值
        /// </summary>
        private object Res;

        public T GetRes<T>()
        {
            var res = (T)this.Res;
            return res;
        }

        #region GET检索

        /// <summary>
        /// 检索类型的提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public void GetSearch<T>(string url, AfterResponseDelegate afterResponseCallBack, Dictionary<string, string> paramDic = null)
        {
            var form = new WaitingForm();
            form.Show();

            var thread = new Thread(new ParameterizedThreadStart(PrivateGetSearch<T>));
            var items = new object[3];
            items[0] = url;
            items[1] = null;
            items[2] = form;
            thread.Start(items);

            _CloseWaiting = CloseCallBack;
            _AfterResponse = afterResponseCallBack;
        }

        private void PrivateGetSearch<T>(object items)
        {
            var temp = items as object[];

            string url = temp[0].ToStr();

            var json = GetJson(url);
            this.Res = JsonUtil.DeSerializer<T>(json);
            var form = temp[2] as WaitingForm;

            _CloseWaiting(form);
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (this.Res == null)
                {
                    FormCommon.ShowErr("检索失败！");
                }
                else
                {
                    var baseRes = this.Res as ResponseModelBase;
                    if (ResponseModelBase.SUCCESSED.Equals(baseRes.result))
                    {
                        if (_AfterResponse != null)
                        {
                            _AfterResponse(this.Res);
                        }
                    }
                    else
                    {
                        var msg = new StringBuilder();
                        msg.AppendLine("检索失败！");
                        if (!baseRes.errMessage.IsNullOrEmpty())
                        {
                            msg.AppendLine("原因：" + baseRes.errMessage);
                        }
                        FormCommon.ShowErr(msg.ToString());
                    }
                }
            }));
        }

        #endregion

        #region POST检索
        /// <summary>
        /// 检索类型的提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <returns></returns>
        public void PostSearch<T>(string url, object reqModel, AfterResponseDelegate afterResponseCallBack)
        {
            var form = new WaitingForm();
            form.Show();

            var thread = new Thread(new ParameterizedThreadStart(PrivatePostSearch<T>));
            var items = new object[3];
            items[0] = url;
            items[1] = reqModel;
            items[2] = form;
            thread.Start(items);

            _CloseWaiting = CloseCallBack;
            _AfterResponse = afterResponseCallBack;
        }

        private void PrivatePostSearch<T>(object items)
        {
            var temp = items as object[];

            string url = temp[0].ToStr();
            object reqModel = temp[1];

            var json = PostJson(url, reqModel);
            this.Res = JsonUtil.DeSerializer<T>(json);
            var form = temp[2] as WaitingForm;

            _CloseWaiting(form);
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (this.Res == null)
                {
                    FormCommon.ShowErr("检索失败！");
                }

                var baseRes = this.Res as ResponseModelBase;
                if (!ResponseModelBase.SUCCESSED.Equals(baseRes.result))
                {
                    var msg = new StringBuilder();
                    msg.AppendLine("检索失败！");
                    if (!baseRes.errMessage.IsNullOrEmpty())
                    {
                        msg.AppendLine("原因：" + baseRes.errMessage);
                    }
                    FormCommon.ShowErr(msg.ToString());
                }
                _AfterResponse(this.Res);
            }));
        }

        #endregion

        #region POST保存

        /// <summary>
        /// 检索类型的提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <param name="afterResponseCallBack">回调函数</param>
        /// <param name="message">替换消息，不为空的时候报该消息</param>
        /// <returns></returns>
        public void PostSave(string url, object reqModel, AfterResponseDelegate afterResponseCallBack = null, string message = null)
        {
            _AfterResponse = afterResponseCallBack;
            this.Url = url;
            this.ReqModel = reqModel;
            string msg = message.IsNullOrEmpty() ? "是否保存？" : message;
            FormCommon.ShowDialog(msg, AfterSaveCallBack);
        }

        /// <summary>
        /// 检索类型的提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <param name="beforeSaveCallBack">保存确认之后，请求后台之前触发的回调</param>
        /// <param name="afterResponseCallBack">回调函数</param>
        /// <param name="message">替换消息，不为空的时候报该消息</param>
        /// <returns></returns>
        public void PostSaveEx(string url, object reqModel, BeforeSaveDelegate beforeSaveCallBack = null, AfterResponseDelegate afterResponseCallBack = null, string message = null)
        {
            _BeforeSave = beforeSaveCallBack;
            _AfterResponse = afterResponseCallBack;
            this.Url = url;
            this.ReqModel = reqModel;
            string msg = message.IsNullOrEmpty() ? "是否保存？" : message;
            FormCommon.ShowDialog(msg, AfterSaveCallBack);
        }

        /// <summary>
        /// 检索类型的提交，不确认直接提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <param name="afterResponseCallBack">回调函数</param>
        /// <param name="message">替换消息，不为空的时候报该消息</param>
        /// <returns></returns>
        public void PostSaveNoComfirm(string url, object reqModel, AfterResponseDelegate afterResponseCallBack = null, string message = null)
        {
            _AfterResponse = afterResponseCallBack;
            this.Url = url;
            this.ReqModel = reqModel;
            AfterSaveCallBack(new DialogModel { _Result = MessageBoxResult.Yes }, false, false);
        }

        /// <summary>
        /// 保存确认的回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isCloseOnly"></param>
        private void AfterSaveCallBack(object item, bool isCloseOnly)
        {
            var model = item as DialogModel;
            if (model._Result == MessageBoxResult.Yes)
            {
                if (_BeforeSave != null)
                {
                    _BeforeSave();
                }
                var form = new WaitingForm();
                form.Show();

                var thread = new Thread(new ParameterizedThreadStart(PrivatePostSave));
                var items = new object[3];
                items[0] = this.Url;
                items[1] = this.ReqModel;
                items[2] = form;
                thread.Start(items);

                _CloseWaiting = CloseCallBack;
            }
        }

        /// <summary>
        /// 保存确认的回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isCloseOnly"></param>
        private void AfterSaveCallBack(object item, bool isCloseOnly, bool comfirm)
        {
            var model = item as DialogModel;
            if (model._Result == MessageBoxResult.Yes)
            {
                if (_BeforeSave != null)
                {
                    _BeforeSave();
                }
                var form = new WaitingForm();
                form.Show();

                var thread = new Thread(new ParameterizedThreadStart(PrivatePostSaveNoComfirm));
                var items = new object[3];
                items[0] = this.Url;
                items[1] = this.ReqModel;
                items[2] = form;
                thread.Start(items);

                _CloseWaiting = CloseCallBack;
            }
        }

        private void PrivatePostSave(object items)
        {
            var temp = items as object[];

            string url = temp[0].ToStr();
            object reqModel = temp[1];

            var json = PostJson(url, reqModel);
            var res = JsonUtil.DeSerializer<ResponseModelBase>(json);
            var form = temp[2] as WaitingForm;

            _CloseWaiting(form);
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (res == null)
                {
                    FormCommon.ShowErr("保存失败！");
                }
                else
                {
                    if (ResponseModelBase.SUCCESSED.Equals(res.result))
                    {
                        FormCommon.ShowMessage("保存成功！", (object item, bool isCloseOnly) =>
                        {
                            if (_AfterResponse != null)
                            {
                                _AfterResponse(res);
                            }
                        });
                    }
                    else
                    {
                        var msg = new StringBuilder();
                        msg.AppendLine("保存失败！");
                        if (!res.errMessage.IsNullOrEmpty())
                        {
                            msg.AppendLine("原因：" + res.errMessage);
                        }
                        FormCommon.ShowErr(msg.ToString());
                    }
                }
            }));
        }

        private void PrivatePostSaveNoComfirm(object items)
        {
            var temp = items as object[];

            string url = temp[0].ToStr();
            object reqModel = temp[1];

            var json = PostJson(url, reqModel);
            var res = JsonUtil.DeSerializer<ResponseModelBase>(json);
            var form = temp[2] as WaitingForm;

            _CloseWaiting(form);
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (res == null)
                {
                    Console.WriteLine("保存失败！");
                }
                else
                {
                    if (ResponseModelBase.SUCCESSED.Equals(res.result))
                    {
                        Console.WriteLine("保存成功！");
                        if (_AfterResponse != null)
                        {
                            _AfterResponse(res);
                        }
                    }
                    else
                    {
                        var msg = new StringBuilder();
                        msg.AppendLine("保存失败！");
                        if (!res.errMessage.IsNullOrEmpty())
                        {
                            msg.AppendLine("原因：" + res.errMessage);
                        }
                        Console.WriteLine(msg.ToString());
                    }
                }
            }));
        }

        #endregion

        #region POST删除

        /// <summary>
        /// 检索类型的提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="reqModel"></param>
        /// <param name="afterResponseCallBack">回调函数</param>
        /// <param name="message">替换消息，不为空的时候报该消息</param>
        /// <returns></returns>
        public void PostDelete(string url, object reqModel, AfterResponseDelegate afterResponseCallBack = null, string message = null)
        {
            _AfterResponse = afterResponseCallBack;
            this.Url = url;
            this.ReqModel = reqModel;
            string msg = message.IsNullOrEmpty() ? "是否删除？" : message;
            FormCommon.ShowDialog(msg, AfterDeleteCallBack);
        }

        /// <summary>
        /// 保存删除的回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isCloseOnly"></param>
        private void AfterDeleteCallBack(object item, bool isCloseOnly)
        {
            var model = item as DialogModel;
            if (model._Result == MessageBoxResult.Yes)
            {
                var form = new WaitingForm();
                form.Show();

                var thread = new Thread(new ParameterizedThreadStart(PrivatePostDelete));
                var items = new object[3];
                items[0] = this.Url;
                items[1] = this.ReqModel;
                items[2] = form;
                thread.Start(items);

                _CloseWaiting = CloseCallBack;
            }
        }

        private void PrivatePostDelete(object items)
        {
            var temp = items as object[];

            string url = temp[0].ToStr();
            object reqModel = temp[1];

            var json = PostJson(url, reqModel);
            var res = JsonUtil.DeSerializer<ResponseModelBase>(json);
            var form = temp[2] as WaitingForm;

            _CloseWaiting(form);
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (res == null)
                {
                    FormCommon.ShowErr("删除失败！");
                }
                else
                {
                    if (ResponseModelBase.SUCCESSED.Equals(res.result))
                    {
                        FormCommon.ShowMessage("删除成功！", (object item, bool isCloseOnly) =>
                        {
                            if (_AfterResponse != null)
                            {
                                _AfterResponse(res);
                            }
                        });
                    }
                    else
                    {
                        var msg = new StringBuilder();
                        msg.AppendLine("删除失败！");
                        if (!res.errMessage.IsNullOrEmpty())
                        {
                            msg.AppendLine("原因：" + res.errMessage);
                        }
                        FormCommon.ShowErr(msg.ToString());
                    }
                }

                if (_AfterResponse != null)
                {
                    _AfterResponse(res);
                }
            }));
        }

        #endregion

        private void CloseCallBack(WaitingForm form)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                form.Close();
                Application.Current.MainWindow.Activate();
            }));
        }

        private string GetJson(string url)
        {
            try
            {
                var json = HttpUtil.Get(StaticClass._URL_HEAD + url);
                return json;
            }
            catch
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    FormCommon.ShowErr("服务器端错误！");
                }));
            }

            return "";
        }

        private string PostJson(string url, object reqModel)
        {
            try
            {
                var json = HttpUtil.Post(StaticClass._URL_HEAD + url, reqModel.Serializer());
                return json;
            }
            catch
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    FormCommon.ShowErr("服务器端错误！");
                }));
            }

            return "";
        }
    }
}
