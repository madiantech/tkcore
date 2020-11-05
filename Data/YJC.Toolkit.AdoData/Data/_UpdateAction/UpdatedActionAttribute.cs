using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class UpdatedActionAttribute : BasePlugInAttribute
    {
        public UpdatedActionAttribute()
        {
            Suffix = "UpdatedAction";
        }

        public override string FactoryName
        {
            get
            {
                return UpdatedActionPlugInFactory.REG_NAME;
            }
        }
    }
}
