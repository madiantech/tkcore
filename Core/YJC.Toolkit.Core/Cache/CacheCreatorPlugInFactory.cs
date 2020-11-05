using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public class CacheCreatorPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_CacheCreator";
        private const string DESCRIPTION = "CacheCreator插件工厂";

        public CacheCreatorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}