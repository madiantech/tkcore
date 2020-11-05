using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Expression(Author = "YJC", CreateDate = "2019-10-15", Description = "返回系统配置的HomePage")]
    internal class HomePageExpression : IExpression
    {
        public string Execute()
        {
            return WebAppSetting.WebCurrent?.HomePath;
        }
    }
}