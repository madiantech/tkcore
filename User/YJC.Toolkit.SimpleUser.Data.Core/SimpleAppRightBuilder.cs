using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [InstancePlugIn, AlwaysCache]
    [AppRightBuilder(Author = "YJC", CreateDate = "2019-10-11", Description = "基于传统UR_USERS的简单权限")]
    internal class SimpleAppRightBuilder : IAppRightBuilder
    {
        public static readonly IAppRightBuilder Instance = new SimpleAppRightBuilder();

        private SimpleAppRightBuilder()
        {
        }

        public IFunctionRight CreateFunctionRight()
        {
            return new UserFunctionRight();
        }

        public ILogOnRight CreateLogOnRight()
        {
            return new UserLogOnRight();
        }

        public IMenuScriptBuilder CreateScriptBuilder()
        {
            return new BootstrapMenuBuilder();
        }
    }
}