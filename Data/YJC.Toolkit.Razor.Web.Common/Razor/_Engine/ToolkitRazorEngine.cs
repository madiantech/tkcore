using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [RazorEngine(Author = "YJC", CreateDate = "2019-03-18",
        Description = "以ToolkitTemplatePage<TModel>为基类的Razor Engine")]
    [InstancePlugIn, AlwaysCache]
    internal class ToolkitRazorEngine
    {
        public static readonly IRazorEngine Instance = RazorUtil.ToolkitEngine;

        private ToolkitRazorEngine()
        {
        }
    }
}