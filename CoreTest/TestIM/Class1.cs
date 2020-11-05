using System;
using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.WeCorp.Service;
using Xunit;
using YJC.Toolkit.WeCorp;
using static Xunit.Assert;
using YJC.Toolkit.WeCorp.Model.Company;

namespace TestIM
{
    public class Class1 : IClassFixture<TestsFixture>
    {
        public Class1(TestsFixture data)
        {
        }

        [Fact]
        public void TestDepartment()
        {
            var service = WeCorpUtil.GetWeCorpService<IDepartmentService>("TestApi");
            List<Department> list = service.GetDepartmentList();
            Equal(10, list.Count);
        }
    }
}