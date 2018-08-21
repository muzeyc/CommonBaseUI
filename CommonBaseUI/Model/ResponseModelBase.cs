namespace CommonBaseUI.Model
{
    public class ResponseModelBase
    {
        public ResponseModelBase()
        {
            this.result = SUCCESSED;
        }

        public const string SUCCESSED = "ok";
        public const string FAILED = "err";
        public const string NOT_LOGIN = "no_login";
        public const string NOT_LOGIN_PAD = "no_login_pad";
        public const string UPLOAD_SUCCESSED = "upload_ok";
        public const string NO_DATA = "no_data";

        public string result { get; set; }
        public string errMessage { get; set; }
        /// <summary>
        /// 扩展字段，可以放任何需要的内容
        /// </summary>
        public string exInfo { get; set; }
        /// <summary>
        /// 用于放列表的总行数
        /// </summary>
        public int totalCount { get; set; }
    }
}