using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class EsModelPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_EsModel";
        private const string DESCRIPTION = "EsModel插件工厂";

        public EsModelPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
