using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HttpHandlerAttribute : BasePlugInAttribute
    {
        public HttpHandlerAttribute()
        {
            Suffix = "HttpHandler";
        }

        public override string FactoryName
        {
            get
            {
                return HttpHandlerPlugInFactory.REG_NAME;
            }
        }
    }
}
