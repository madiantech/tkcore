using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [RazorEngine(Author = "YJC", CreateDate = "2019-03-27",
        Description = "以ListTemplatePage<TModel>为基类的Razor Engine")]
    [InstancePlugIn, AlwaysCache]
    internal class ListRazorEngine
    {
        public static readonly IRazorEngine Instance = RazorUtil.ListEngine;

        private ListRazorEngine()
        {
        }
    }
}