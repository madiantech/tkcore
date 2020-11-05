using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class NotifyActionPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_NotifyAction";
        private const string DESCRIPTION = "NotifyAction插件工厂";

        public NotifyActionPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
