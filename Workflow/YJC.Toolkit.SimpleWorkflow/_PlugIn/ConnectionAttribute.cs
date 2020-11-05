using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ConnectionAttribute : BasePlugInAttribute
    {
        public ConnectionAttribute()
        {
            Suffix = "Connection";
        }

        public override string FactoryName
        {
            get
            {
                return ConnectionPlugInFactory.REG_NAME;
            }
        }
    }
}