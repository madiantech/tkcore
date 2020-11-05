using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace YJC.Toolkit.Razor
{
    public interface IEngineHandler
    {
        IRazorTemplateCompiler Compiler { get; }

        ITemplateFactoryProvider FactoryProvider { get; }

        TkRazorOptions Options { get; }

        IRazorEngine RazorEngine { get; set; }

        Task<ITemplatePage> CompileTemplateAsync(string key);

        Task<string> CompileRenderAsync<T>(string key, T model, object initData, ExpandoObject viewBag);

        Task<string> RenderTemplateAsync<T>(ITemplatePage templatePage, T model, IPageContext context);

        Task<string> CompileRenderStringAsync<T>(string key, string content, T model, object initData, ExpandoObject viewBag);

        Task<string> RenderTemplateAsync<T>(ITemplatePage templatePage, T model, object initData, ExpandoObject viewBag);

        Task RenderTemplateAsync<T>(ITemplatePage templatePage, T model, TextWriter textWriter, object initData, ExpandoObject viewBag);

        Task RenderIncludedTemplateAsync<T>(ITemplatePage templatePage, T model, TextWriter textWriter, object initData, ExpandoObject viewBag, TemplateRenderer templateRenderer);
    }
}