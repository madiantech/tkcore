using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class RazorEnginePlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_RazorEngine";
        private const string DESCRIPTION = "RazorEngine插件工厂";

        public RazorEnginePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

        public static IRazorEngine CreateRazorEngine(string engineName)
        {
            if (string.IsNullOrEmpty(engineName))
                return RazorUtil.ToolkitEngine;

            TkDebug.ThrowIfNoGlobalVariable();
            RazorEnginePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager
                .GetCodeFactory(REG_NAME).Convert<RazorEnginePlugInFactory>();
            IRazorEngine engine = factory.CreateInstance<IRazorEngine>(engineName);

            return engine;
        }
    }
}