using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public class DefaultValueTypeFactory : BaseTypeFactory
    {
        public const string REG_NAME = "_tk_DefaultValue";
        private const string DESCRIPTION = "DefaultValue插件工厂";

        public DefaultValueTypeFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}