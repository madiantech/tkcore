using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EvaluateAdditionAttribute : BasePlugInAttribute
    {
        public EvaluateAdditionAttribute()
        {
            Suffix = "EvaluateAddition";
        }

        public override string FactoryName
        {
            get
            {
                return EvaluateAdditionPlugInFactory.REG_NAME;
            }
        }
    }
}