using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ModuleTemplateAttribute : BasePlugInAttribute
    {
        public ModuleTemplateAttribute()
        {
            Suffix = "ModuleTemplate";
        }

        public override string FactoryName
        {
            get
            {
                return ModuleTemplatePlugInFactory.REG_NAME;
            }
        }
    }
}
