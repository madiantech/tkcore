using Microsoft.Extensions.Options;
using System;
using System.Dynamic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class EngineHandler : IEngineHandler
    {
        public EngineHandler(TkRazorOptions options, IRazorTemplateCompiler compiler,
            ITemplateFactoryProvider factoryProvider)
        {
            TkDebug.AssertArgumentNull(options, nameof(options), null);
            TkDebug.AssertArgumentNull(compiler, nameof(compiler), null);
            TkDebug.AssertArgumentNull(factoryProvider, nameof(factoryProvider), null);

            Options = options;
            Compiler = compiler;
            FactoryProvider = factoryProvider;
        }

        public EngineHandler(IOptions<TkRazorOptions> options, IRazorTemplateCompiler compiler,
            ITemplateFactoryProvider factoryProvider)
            : this(options.Value, compiler, factoryProvider)
        {
        }

        public TkRazorOptions Options { get; }

        public IRazorTemplateCompiler Compiler { get; }

        public ITemplateFactoryProvider FactoryProvider { get; }

        public IRazorEngine RazorEngine { get; set; }

        private void SetModelContext<T>(ITemplatePage templatePage, TextWriter textWriter,
            T model, object initData, ExpandoObject viewBag)
        {
            var pageContext = new PageContext(viewBag, initData)
            {
                ExecutingPageKey = templatePage.Key,
                Writer = textWriter
            };

            if (model != null)
            {
                pageContext.ModelTypeInfo = new ModelTypeInfo(model.GetType());

                object pageModel = pageContext.ModelTypeInfo.CreateTemplateModel(model);
                templatePage.SetModel(pageModel);

                pageContext.Model = pageModel;
            }

            templatePage.PageContext = pageContext;
        }

        public async Task<ITemplatePage> CompileTemplateAsync(string key)
        {
            TemplateCacheItem item = CacheManager.GetItem("TemplatePage", key,
                Compiler, FactoryProvider).Convert<TemplateCacheItem>();
            Func<ITemplatePage> templateFactory = item.TemplatePageFactory;

            ITemplatePage templatePage = templateFactory();
            templatePage.DisableEncoding = Options.DisableEncoding;
            return templatePage;
            //if (IsCachingEnabled)
            //{
            //    var cacheLookupResult = Cache.RetrieveTemplate(key);
            //    if (cacheLookupResult.Success)
            //    {
            //        return cacheLookupResult.Template.TemplatePageFactory();
            //    }
            //}

            //CompiledTemplateDescriptor templateDescriptor = await Compiler.CompileAsync(key);
            //Func<ITemplatePage> templateFactory = FactoryProvider.CreateFactory(templateDescriptor);

            //if (IsCachingEnabled)
            //{
            //    Cache.CacheTemplate(
            //        key,
            //        templateFactory,
            //        templateDescriptor.ExpirationToken);
            //}

            //return templateFactory();
        }

        public async Task<string> RenderTemplateAsync<T>(ITemplatePage templatePage, T model,
            object initData, ExpandoObject viewBag)
        {
            using (var writer = new StringWriter())
            {
                await RenderTemplateAsync(templatePage, model, writer, initData, viewBag);

                return writer.ToString();
            }
        }

        public async Task RenderTemplateAsync<T>(ITemplatePage templatePage, T model,
            TextWriter textWriter, object initData, ExpandoObject viewBag)
        {
            TkDebug.AssertArgumentNull(textWriter, nameof(textWriter), this);

            SetModelContext(templatePage, textWriter, model, initData, viewBag);

            using (var scope = new MemoryPoolViewBufferScope())
            {
                var renderer = new TemplateRenderer(this, HtmlEncoder.Default, scope);
                await renderer.RenderAsync(templatePage).ConfigureAwait(false);
            }
        }

        public async Task RenderIncludedTemplateAsync<T>(ITemplatePage templatePage, T model,
            TextWriter textWriter, object initData, ExpandoObject viewBag, TemplateRenderer templateRenderer)
        {
            TkDebug.AssertArgumentNull(textWriter, nameof(textWriter), this);

            SetModelContext(templatePage, textWriter, model, initData, viewBag);

            //templateRenderer.RazorPage = templatePage;
            await templateRenderer.RenderAsync(templatePage).ConfigureAwait(false);
        }

        public async Task<string> CompileRenderAsync<T>(string key, T model, object initData, ExpandoObject viewBag)
        {
            ITemplatePage template = await CompileTemplateAsync(key).ConfigureAwait(false);
            template.RazorEngine = RazorEngine;

            return await RenderTemplateAsync(template, model, initData, viewBag).ConfigureAwait(false);
        }

        public async Task<string> RenderTemplateAsync<T>(ITemplatePage templatePage, T model, IPageContext context)
        {
            TkDebug.AssertArgumentNull(templatePage, nameof(templatePage), this);
            TkDebug.AssertArgumentNull(context, nameof(context), this);

            templatePage.SetModel(model);
            context.Convert<PageContext>().Model = model;
            using (var writer = new StringWriter())
            using (var scope = new MemoryPoolViewBufferScope())
            {
                var oldWriter = context.Writer;
                try
                {
                    context.Writer = writer;
                    var renderer = new TemplateRenderer(this, HtmlEncoder.Default, scope);
                    await renderer.RenderAsync(templatePage).ConfigureAwait(false);
                    return writer.ToString();
                }
                finally
                {
                    context.Writer = oldWriter;
                }
            }
        }

        public Task<string> CompileRenderStringAsync<T>(string key, string content,
            T model, object initData, ExpandoObject viewBag)
        {
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), this);
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), this);

            Options.DynamicTemplates[key] = content;
            return CompileRenderAsync(key, model, initData, viewBag);
        }
    }
}