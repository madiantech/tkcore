using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EsTemplateAttribute : BasePlugInAttribute
    {
        public EsTemplateAttribute()
        {
            Suffix = "EsTemplate";
        }

        public override string FactoryName
        {
            get
            {
                return EsTemplatePlugInFactory.REG_NAME;
            }
        }
    }
}
