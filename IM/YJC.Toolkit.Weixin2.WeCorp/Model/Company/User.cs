using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //成员管理（创建成员）
    public class User : SimpleUser
    {
        protected User()
        {
        }

        public User(string userId, string userName, IEnumerable<int> department)
        {
            TkDebug.AssertArgumentNullOrEmpty(userId, "userId", null);
            TkDebug.AssertArgumentNullOrEmpty(userName, "userName", null);

            Id = userId;
            Name = userName;
            List<int> depart = department as List<int>;
            if (depart != null)
                Department = depart;
            else
                Department = new List<int>(department);
        }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 30)]//成员别名
        public string Alias { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 40)]//手机号码
        // [NameModel(WeCorpConst.USER_MODE)]
        public string Mobile { get; set; }

        //[SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true, Order = 50)]//成员所属部门Id列表
        //internal List<int> Department { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true, Order = 60, UseSourceType = true)]
        //部门内的排序值
        public List<int> Order { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 70)]//职务信息
        // [NameModel(WeCorpConst.USER_MODE)]
        public string Position { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 80)]
        [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]//性别
        public Gender Gender { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 90)]//邮箱
        // [NameModel(WeCorpConst.USER_MODE)]
        public string Email { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true, Order = 110)]
        public List<int> IsLeaderInDept { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 130, UseSourceType = true)]//启用禁用成员
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Enable { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 120)]//成员头像
        public string AvatarMediaid { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 122)]
        public string Avatar { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]//座机
        public string Telephone { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 180)]//地址
        public string Address { get; set; }

        [TagElement(LocalName = "extattr", Order = 130)]
        [ObjectElement(IsMultiple = true, LocalName = "attrs", UseConstructor = true, Order = 140)]//自定义字段
        public List<ExtAttribute> ExtAttrs { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 150, UseSourceType = true)]
        public bool ToInvite { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 153, UseSourceType = true)]
        public UserStatus Status { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 154)]
        public string QrCode { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, Order = 160)]//成员对外属性
        public ExternalProfile ExternalProfile { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 170)]//对外职务
        public string ExternalPosition { get; set; }
    }
}