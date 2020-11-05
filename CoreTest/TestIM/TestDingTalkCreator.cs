using YJC.Toolkit.DingTalk;
using YJC.Toolkit.IM;
using System;

namespace TestIM
{
    ///*
    // * AgentId: 222084295
    // * AppKey: ding74v6mfwnmieu2quo
    // * AppSecret: kDSnSI3j9Gfjx0q3JA5w4cdL15ZtZt4qZMzco7ebLNtOvjAiWalfX5qZBWhY0Wc- *
    // * /
    internal class TestDingTalkCreator : BaseDingTalkPlatformSettingCreator
    {
        private readonly static DingTalkSettings fSettings;
        public static readonly TestDingTalkCreator Creator = new TestDingTalkCreator();

        static TestDingTalkCreator()
        {
            DingTalkAppConfig app = new DingTalkAppConfig("TestApp", 222084295, "测试钉钉API",
                "ding74v6mfwnmieu2quo", "kDSnSI3j9Gfjx0q3JA5w4cdL15ZtZt4qZMzco7ebLNtOvjAiWalfX5qZBWhY0Wc-");
            fSettings = new DingTalkSettings(app);
        }

        public TestDingTalkCreator()
            : base(@"d:\dingtalk.xml")
        {
        }

        public override DingTalkSettings Create(string tenantId)
        {
            return fSettings;
        }
    }
}