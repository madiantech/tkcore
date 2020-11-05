using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ExpressionPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_Expression";
        internal const string DESCRIPTION = "宏的插件工厂";

        public ExpressionPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
