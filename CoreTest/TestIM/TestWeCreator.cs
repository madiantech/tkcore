using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.WeCorp;

namespace TestIM
{
    internal class TestWeCreator : BaseXmlPlatformSettingCreator<WeCorpSettings>
    {
        private readonly static WeCorpSettings fSettings;
        public static readonly TestWeCreator Creator = new TestWeCreator();

        static TestWeCreator()
        {
            fSettings = new WeCorpSettings("ww0027a635e7aedc16", "1fde9Uk1h-4RJRR8Ln-MRK-_UPMCb3RTNcn_L0CI0_U", null);
            var app = new WeCorpAppConfig("TestApi", 1000002, "测试API",
                "LQUTKHHiYtXgWeHQRhEr9OfmggBct0QDrxqxxLMMWaM", "test", "test");
            fSettings.Add(app);
            //fSettings.SetCallbackParam("DataNew", "mua44q8dbrnqsrsmg2iuej8j45hq4b9nvgy13azqjim");
        }

        public TestWeCreator()
            : base(@"d:\wecorp.xml")
        {
        }

        public override WeCorpSettings Create(string tenantId)
        {
            return fSettings;
        }
    }
}