using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class EsTemplatePlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_EsTemplate";
        private const string DESCRIPTION = "EsTemplate插件工厂";

        public EsTemplatePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
