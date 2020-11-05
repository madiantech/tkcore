using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model.Company;

namespace YJC.Toolkit.WeCorp.Model.Application
{
    public class AllowUserInfo
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<User> User { get; set; }
    }
}