using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class BaseChat : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]//群名称
        public string Name { get; protected set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//群主userID
        public string Owner { get; protected set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//群禁言
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool ChatBannedType { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//新成员是否可查看聊天记录
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool ShowHistoryType { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//群是否可搜索
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Searchable { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//是否有入群验证
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool ValidationType { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//@all权限
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool MentionAllAuthority { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//管理类型
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool ManagementType { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}