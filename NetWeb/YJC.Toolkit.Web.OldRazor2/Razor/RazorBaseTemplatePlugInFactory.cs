using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class RazorBaseTemplatePlugInFactory : BaseTypeFactory
    {
        public const string REG_NAME = "_tk_RazorBaseTemplate";
        private const string DESCRIPTION = "RazorBaseTemplate插件工厂";

        public RazorBaseTemplatePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}