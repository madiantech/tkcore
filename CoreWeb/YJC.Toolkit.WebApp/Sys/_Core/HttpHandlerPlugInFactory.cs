using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public class HttpHandlerPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_HttpHandler";
        private const string DESCRIPTION = "HttpHandler插件工厂";

        public HttpHandlerPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}