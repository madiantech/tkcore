using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    internal class AppMessageParam : BaseResult
    {
        public AppMessageParam(int agentId, string userIdList, string deptIdList,
            bool toAllUser, BaseMessage msg)
        {
            AgentId = agentId;
            UserIdList = ParseListString(userIdList);
            DeptIdList = ParseListString(deptIdList);
            ToAllUser = toAllUser;
            Msg = msg;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, UseSourceType = true)]
        public int AgentId { get; set; }

        [SimpleElement(LocalName = "userid_list")]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> UserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> DeptIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, UseSourceType = true)]
        public bool ToAllUser { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public BaseMessage Msg { get; set; }

        public static List<string> ParseListString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var converter = new CommaStringListTypeConverter();
            return converter.ConvertFromString(value,
                ObjectUtil.ReadSettings).Convert<List<string>>();
        }
    }
}