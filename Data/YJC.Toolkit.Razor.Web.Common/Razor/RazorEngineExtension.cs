using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public static class RazorEngineExtension
    {
        public static RazorEngineBuilder UseToolkitProject(this RazorEngineBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, nameof(builder), null);

            builder.UseProject(new ToolkitRazorProject());
            return builder;
        }
    }
}