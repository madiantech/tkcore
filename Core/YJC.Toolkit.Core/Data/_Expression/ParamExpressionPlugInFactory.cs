using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ParamExpressionPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_ParamExpression";
        internal const string DESCRIPTION = "带参数的宏的插件工厂";

        public ParamExpressionPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
            //Add(typeof(UniIdParamExpression));
            //Add(typeof(DataTableParamExpression));
        }
    }
}
