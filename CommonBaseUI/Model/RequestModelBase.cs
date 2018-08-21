
namespace CommonBaseUI.Model
{
    public class RequestModelBase
    {
        public RequestModelBase()
        {
            size = 10000;
        }
        /// <summary>
        /// 分页偏移量
        /// </summary>
        public int offset { get; set; }
        /// <summary>
        /// 分页显示行数
        /// </summary>
        public int size { get; set; }
    }
}
