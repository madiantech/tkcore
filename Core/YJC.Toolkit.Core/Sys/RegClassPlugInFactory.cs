namespace YJC.Toolkit.Sys
{
    public sealed class RegClassPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_RegClass";
        private const string DESCRIPTION = "收集注册类的插件工厂";

        public RegClassPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
