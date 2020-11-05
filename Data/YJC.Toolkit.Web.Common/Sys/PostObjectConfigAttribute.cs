using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PostObjectConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return PostObjectConfigFactory.REG_NAME;
            }
        }
    }
}
