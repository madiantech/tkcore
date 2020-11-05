using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ConstraintConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return ConstraintConfigFactory.REG_NAME;
            }
        }
    }
}
