using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TimingJobAttribute : BasePlugInAttribute
    {
        public TimingJobAttribute()
        {
            Suffix = "Job";
        }

        public override string FactoryName
        {
            get
            {
                return TimingJobPlugInFactory.REG_NAME;
            }
        }
    }
}