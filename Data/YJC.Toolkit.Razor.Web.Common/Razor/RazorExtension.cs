using System.Dynamic;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public static class RazorExtension
    {
        public static async Task<string> CompileRenderWithLayoutAsync<T>(this IRazorEngine engine, string key, string layout,
            T model, object initData, ExpandoObject viewBag = null)
        {
            TkDebug.AssertArgumentNull(engine, nameof(engine), null);

            var handler = engine.Handler;
            ITemplatePage page;
            if (string.IsNullOrEmpty(key))
            {
                page = await handler.CompileTemplateAsync(layout);
            }
            else
            {
                page = await handler.CompileTemplateAsync(key);
                page.Layout = layout;
            }
            page.RazorEngine = engine;

            return await handler.RenderTemplateAsync(page, model, initData, viewBag);
        }
    }
}