using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ObjectSerializerAttribute : BasePlugInAttribute
    {
        public ObjectSerializerAttribute()
        {
            Suffix = "ObjectSerializer";
        }

        public override string FactoryName
        {
            get
            {
                return SerializerPlugInFactory.REG_NAME;
            }
        }
    }
}
