using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorTemplateTypeFactory : BaseTypeFactory
    {
        public const string REG_NAME = "_tk_RazorTemplate";
        private const string DESCRIPTION = "注册Razor模板类型的类型工厂";

        public RazorTemplateTypeFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
