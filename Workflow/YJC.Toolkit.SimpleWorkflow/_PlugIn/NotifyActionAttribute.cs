using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class NotifyActionAttribute : BasePlugInAttribute
    {
        public NotifyActionAttribute()
        {
            Suffix = "NotifyAction";
        }

        public override string FactoryName
        {
            get
            {
                return NotifyActionPlugInFactory.REG_NAME;
            }
        }
    }
}
