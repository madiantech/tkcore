using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public class ConfigTypeFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_Config";
        private const string DESCRIPTION = "注册配置的类型工厂";

        public ConfigTypeFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}