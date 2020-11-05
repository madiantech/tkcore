using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorConfigXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public RazorConfigItem Razor { get; private set; }

        public void SetConfiguration(RazorConfiguration config)
        {
            if (Razor == null)
                return;
            config.RaiseOnCompileError = Razor.RaiseOnCompileError;
            config.RaiseOnRunError = Razor.RaiseOnRunError;
            config.SaveCompileAssembly = Razor.SaveCompileAssembly;
            config.SaveCompileCode = Razor.SaveCompileCode;
            if (Razor.SavePath != null)
                config.SavePath = FileUtil.GetRealFileName(Razor.SavePath.RelativePath,
                    Razor.SavePath.Position);
            if (Razor.Service == RazorServiceType.Isolated)
                RazorUtil.SetTemplateService(new IsolatedTemplateService());
        }
    }
}