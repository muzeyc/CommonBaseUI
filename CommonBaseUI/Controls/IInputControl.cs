
namespace CommonBaseUI.Controls
{
    public interface IInputControl : IControl
    {
        /// <summary>
        /// 值
        /// </summary>
        object _Value { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        string _Caption { get; set; }
        /// <summary>
        /// 是否必须
        /// </summary>
        bool _MustInput { get; set; }
        /// <summary>
        /// 绑定的对象中的字段
        /// </summary>
        string _Binding { get; set; }
        /// <summary>
        /// 标题部分的宽度
        /// </summary>
        double _CaptionWidth { get; set; }
        /// <summary>
        /// 输入部分的宽度
        /// </summary>
        double _InputWidth { get; set; }
        void _SetErr();
        void _CleanErr();
        //void _Clear();
    }
}
