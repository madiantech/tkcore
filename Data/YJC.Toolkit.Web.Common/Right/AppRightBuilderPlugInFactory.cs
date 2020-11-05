using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class AppRightBuilderPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_AppRightBuilder";
        private const string DESCRIPTION = "AppRightBuilder插件工厂";

        public AppRightBuilderPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}