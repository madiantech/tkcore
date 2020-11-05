using System;
using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin;

namespace YJC.Toolkit.WeChat.Model.User
{
    public class WeUser : BaseResult
    {
        internal WeUser()
        {
        }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 10)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        [NameModel(WeConst.USER_MODE)]
        public bool Subscribe { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 20)]
        [NameModel(WeConst.USER_MODE)]
        public string OpenId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 30)]
        [NameModel(WeConst.USER_MODE)]
        public string NickName { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 40)]
        [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
        [NameModel(WeConst.USER_MODE)]
        public SexType Sex { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 50)]
        [NameModel(WeConst.USER_MODE)]
        public string Language { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 60)]
        [NameModel(WeConst.USER_MODE)]
        public string City { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 70)]
        [NameModel(WeConst.USER_MODE)]
        public string Province { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 80)]
        [NameModel(WeConst.USER_MODE)]
        public string Country { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 90)]
        [NameModel(WeConst.USER_MODE)]
        public string HeadImgUrl { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 100)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        [NameModel(WeConst.USER_MODE)]
        public DateTime SubscribeTime { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 110)]
        [NameModel(WeConst.USER_MODE)]
        public string UnionId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 120)]
        [NameModel(WeConst.USER_MODE)]
        public string Remark { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 130)]
        public int GroupId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true, Order = 140)]
        public List<int> TagidList { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 150)]
        [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
        public SubscribeScene SubscribeScene { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 160)]
        public int QRScene { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 170)]
        public string QRSceneStr { get; private set; }

        public override string ToString()
        {
            return NickName ?? base.ToString();
        }
    }
}