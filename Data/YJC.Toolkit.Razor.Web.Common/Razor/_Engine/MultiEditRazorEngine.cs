using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [RazorEngine(Author = "YJC", CreateDate = "2019-03-27",
        Description = "以MultiEditTemplatePage<TModel>为基类的Razor Engine")]
    [InstancePlugIn, AlwaysCache]
    internal class MultiEditRazorEngine
    {
        public static readonly IRazorEngine Instance = RazorUtil.MultiEditEngine;

        private MultiEditRazorEngine()
        {
        }
    }
}