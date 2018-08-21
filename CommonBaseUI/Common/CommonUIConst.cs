
namespace CommonBaseUI.Common
{
    public class CommonUIConst
    {
        /// <summary>
        /// 订单状态的枚举类型
        /// </summary> 
        public enum FormMode
        {
            /// <summary>
            /// 无状态
            /// </summary> 
            FORM_MODE_NONE = 0,
            /// <summary>
            /// 新建
            /// </summary> 
            FORM_MODE_NEW = 1,
            /// <summary>
            /// 编辑
            /// </summary> 
            FORM_MODE_EDIT = 2,
            /// <summary>
            /// 复制
            /// </summary>
            FORM_MODE_COPY = 3,
            /// <summary>
            /// 查看详情
            /// </summary>
            FORM_MODE_VIEW = 4,
        }
    }
}
