using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.WeCorp;

namespace TestIM
{
    internal class TestWeCorpCreator : BaseXmlWeCorpPlatformSettingCreator
    {
        private WeCorpSettings fSettings;

        public TestWeCorpCreator()
            : base(@"d:\Temp\WeCorp.xml")
        {
            fSettings = new WeCorpSettings("ww0027a635e7aedc16", "1fde9Uk1h-4RJRR8Ln-MRK-_UPMCb3RTNcn_L0CI0_U", "11");
            WeCorpAppConfig app = new WeCorpAppConfig("TestApi", 1000002,
                "测试API", "LQUTKHHiYtXgWeHQRhEr9OfmggBct0QDrxqxxLMMWaM", "hello", "123");
            fSettings.Add(app);
        }

        public override WeCorpSettings Create(string tenantId)
        {
            return fSettings;
        }

        public override WeCorpSettings CreateWithCorpId(string corpId)
        {
            return fSettings;
        }
    }
}