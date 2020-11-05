using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ModelCreatorAttribute : BasePlugInAttribute
    {
        public ModelCreatorAttribute()
        {
            Suffix = "ModelCreator";
        }

        public override string FactoryName
        {
            get
            {
                return ModelCreatorPlugInFactory.REG_NAME;
            }
        }
    }
}