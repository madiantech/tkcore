using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using YJC.Toolkit.DingTalk;
using YJC.Toolkit.DingTalk.Model.Company;
using YJC.Toolkit.DingTalk.Service;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp;
using static Xunit.Assert;

namespace TestIM
{
    public class TestsFixture : IDisposable
    {
        public TestsFixture()
        {
            TestApp.Initialize(@"");
            DingTalkConfiguration.SetCreator(TestDingTalkCreator.Creator);
            WeCorpConfiguration.SetCreator(new TestWeCorpCreator());
        }

        public void Dispose()
        {
            // Do "global" teardown here; Only called once.
        }
    }

    public class UnitTest1 : IClassFixture<TestsFixture>
    {
        public UnitTest1(TestsFixture data)
        {
        }

        [Fact]
        public void TestConnectionService()
        {
            IConnectionService service = DingTalkUtil.GetDingTalkService<IConnectionService>("TestApp");
            AccessToken token = service.GetAccessToken("ding74v6mfwnmieu2quo",
                "kDSnSI3j9Gfjx0q3JA5w4cdL15ZtZt4qZMzco7ebLNtOvjAiWalfX5qZBWhY0Wc-");
        }

        [Fact]
        public void TestDepartment()
        {
            var service = DingTalkUtil.GetDingTalkService<IDepartmentService>("TestApp");
            List<SimpleDepartment> list = service.GetDepartmentList();
            Equal(10, list.Count);
        }
    }
}