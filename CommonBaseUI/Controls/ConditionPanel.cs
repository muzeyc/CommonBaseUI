using CommonBaseUI.Common;
using CommonUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CommonBaseUI.Controls
{
    public class ConditionPanel : StackPanel
    {
        /// <summary>
        /// 收集条件 
        /// 遍历该容器下所有继承IInputControl接口的控件的_Value属性，并反序列化到对应的类的对象字段中
        /// </summary>
        /// <returns></returns>
        public T _GetConditon<T>()
        {
            var dic = new Dictionary<string, object>();
            GetCondition(this, dic);

            if (dic.Count > 0)
            {
                string json = dic.DictionaryToJson();
                return JsonUtil.DeSerializer<T>(json);
            }

            return System.Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dic"></param>
        private void GetCondition(UIElement element, Dictionary<string, object> dic)
        {
            if (element is Panel)
            {
                var panel = element as Panel;
                foreach (UIElement control in panel.Children)
                {
                    GetCondition(control, dic);
                }
            }
            else if (element is IInputControl)
            {
                var contr = element as IInputControl;
                if (!contr._Binding.IsNullOrEmpty())
                {
                    dic[contr._Binding] = contr._Value;
                }
            }

            // 对日期范围控件特殊处理
            if (element is MyDatePickerRange)
            {
                var contr = element as MyDatePickerRange;
                if (!contr._Binding2.IsNullOrEmpty())
                {
                    dic[contr._Binding2] = contr._Value2;
                }
            }
        }

        /// <summary>
        /// 收集条件
        /// 遍历该容器下所有继承IInputControl接口的控件的_Value属性，并反序列化到对应的类的对象字段中
        /// </summary>
        /// <returns></returns>
        public void _GetConditon<T>(T obj)
        {
            var dic = new Dictionary<string, object>();
            GetCondition(this, dic);

            if (dic.Count > 0)
            {
                Type t = obj.GetType();
                foreach (var pi in t.GetProperties())
                {
                    if (dic.ContainsKey(pi.Name))
                    {
                        var typeName = t.GetProperty(pi.Name).PropertyType.Name;
                        if ("Nullable`1".Equals(typeName))
                        {
                            typeName = t.GetProperty(pi.Name).PropertyType.GenericTypeArguments[0].Name;
                        }
                        switch (typeName)
                        {
                            case "String":
                                t.GetProperty(pi.Name).SetValue(obj, dic[pi.Name].ToStr());
                                break;
                            case "Int32":
                                t.GetProperty(pi.Name).SetValue(obj, dic[pi.Name].ToInt());
                                break;
                            case "Double":
                                t.GetProperty(pi.Name).SetValue(obj, dic[pi.Name].ToDouble());
                                break;
                            case "Decimal":
                                t.GetProperty(pi.Name).SetValue(obj, dic[pi.Name].ToDec());
                                break;
                            case "Int64":
                                t.GetProperty(pi.Name).SetValue(obj, dic[pi.Name].ToLong(), null);
                                break;
                            case "DateTime":
                                t.GetProperty(pi.Name).SetValue(obj, dic[pi.Name].ToDateTime(), null);
                                break;
                            default:
                                t.GetProperty(pi.Name).SetValue(obj, dic[pi.Name]);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置条件
        /// </summary>
        /// <returns></returns>
        public void _SetConditon<T>(T obj)
        {
            if (obj != null)
            {
                var dic = new Dictionary<string, string>();
                var properties = obj.GetType().GetProperties();
                foreach (var property  in properties)
                {
                    dic[property.Name] = property.Name;
                }

                SetCondition(this, dic, obj);
            }
        }

        private void SetCondition(UIElement element,  Dictionary<string, string> dic, object obj)
        {
            if (element is Panel)
            {
                var panel = element as Panel;
                foreach (UIElement child in panel.Children)
                {
                    SetCondition(child, dic, obj);
                }
            }
            else if (element is IInputControl)
            {
                var contr = element as IInputControl;
                if (!contr._Binding.IsNullOrEmpty())
                {
                    if (dic.ContainsKey(contr._Binding))
                    {
                        if (!contr._Binding.IsNullOrEmpty())
                        {
                            var value = obj.GetType().GetProperty(contr._Binding).GetValue(obj, null);
                            //contr._Value = value.ToStr().IsNullOrEmpty() ? contr._Value : value;
                            contr._Value = value;
                        }
                    }
                }
            }

            // 对日期范围控件特殊处理
            if (element is MyDatePickerRange)
            {
                var contr = element as MyDatePickerRange;
                if (dic.ContainsKey(contr._Binding2) && !contr._Binding2.IsNullOrEmpty())
                {
                    var value = obj.GetType().GetProperty(contr._Binding2).GetValue(obj, null);
                    //contr._Value2 = value.ToStr().IsNullOrEmpty() ? contr._Value2 : value;
                    contr._Value2 = value;
                }
            }
        }

        /// <summary>
        /// 验证条件
        /// 遍历该容器下所有继承IInputControl接口的控件的_Value属性，当被遍历的控件的_MustInput属性为true，
        /// 并且没有输入时会返回这些控件的_Caption，用逗号分隔
        /// </summary>
        /// <returns></returns>
        public string _CheckCondition()
        {
            var listMustInput = new List<IInputControl>();
            GetMustInputCondition(this, listMustInput);


            var list = new List<string>();
            var msg = new StringBuilder();
            foreach (var contr in listMustInput)
            {
                // 对日期范围控件特殊处理
                if (contr is MyDatePickerRange)
                {
                    var datePickerRange = contr as MyDatePickerRange;
                    string from = datePickerRange._Value.ToStr();
                    string to = datePickerRange._Value2.ToStr();
                    if (!from.IsNullOrEmpty() && !from.IsNullOrEmpty())
                    {
                        if (from.CompareTo(to) > 0)
                        {
                            list.Add(contr._Caption);
                            contr._SetErr();
                            msg.AppendLine(contr._Caption + "的大小关系不正确！");
                        }
                    }

                    if (from.IsNullOrEmpty() && to.IsNullOrEmpty())
                    {
                        list.Add(contr._Caption);
                        contr._SetErr();
                    }
                    else
                    {
                        contr._CleanErr();
                    }
                }
                else
                {
                    if (contr._Value.ToStr().IsNullOrEmpty())
                    {
                        list.Add(contr._Caption);
                        contr._SetErr();
                    }
                    else
                    {
                        contr._CleanErr();
                    }
                }
            }
            string errControls = string.Join(",", list);
            if (!errControls.IsNullOrEmpty())
            {
                msg.AppendLine(errControls + "必须输入！");
            }

            if (!msg.ToString().IsNullOrEmpty())
            {
                FormCommon.ShowErr(msg.ToString());
            }
            return errControls;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="list"></param>
        private void GetMustInputCondition(UIElement element, List<IInputControl> list)
        {
            if (element is Panel)
            {
                var panel = element as Panel;
                foreach (UIElement control in panel.Children)
                {
                    GetMustInputCondition(control, list);
                }
            }
            else if (element is IInputControl)
            {
                var contr = element as IInputControl;
                if (contr._MustInput)
                {
                    list.Add(contr);
                }
            }
        } 
    }
}
