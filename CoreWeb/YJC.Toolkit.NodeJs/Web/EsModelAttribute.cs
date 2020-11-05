using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EsModelAttribute : BasePlugInAttribute
    {
        public EsModelAttribute()
        {
            Suffix = "EsModel";
        }

        public override string FactoryName
        {
            get
            {
                return EsModelPlugInFactory.REG_NAME;
            }
        }
    }
}
