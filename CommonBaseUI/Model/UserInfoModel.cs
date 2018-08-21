using System;

namespace CommonBaseUI.Model
{
    public class UserInfoModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string Sex { get; set; }
        public string PersonId { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string CreateUserName { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateUserName { get; set; }
        public string OrgCode { get; set; }
        /// <summary>
        /// 所属车间
        /// </summary>
        public string workshop { get; set; }
        /// <summary>
        /// 所属班组编号
        /// </summary>
        public string team { get; set; }
        /// <summary>
        /// 所属班组名称
        /// </summary>
        public string teamName { get; set; }
    }
}
