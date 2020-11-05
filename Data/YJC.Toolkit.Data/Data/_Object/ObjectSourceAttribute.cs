using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ObjectSourceAttribute : BasePlugInAttribute
    {
        public ObjectSourceAttribute()
        {
            Suffix = "ObjectSource";
        }

        public override string FactoryName
        {
            get
            {
                return ObjectSourcePlugInFactory.REG_NAME;
            }
        }
    }
}
