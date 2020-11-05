using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class TemplateRenderer
    {
        private readonly HtmlEncoder fHtmlEncoder;
        private readonly IEngineHandler fEngineHandler;
        private readonly IViewBufferScope fBufferScope;

        internal TemplateRenderer(IEngineHandler engineHandler,
            HtmlEncoder htmlEncoder, IViewBufferScope bufferScope)
        {
            TkDebug.AssertArgumentNull(engineHandler, nameof(engineHandler), null);
            TkDebug.AssertArgumentNull(bufferScope, nameof(bufferScope), null);
            TkDebug.AssertArgumentNull(htmlEncoder, nameof(htmlEncoder), null);

            fEngineHandler = engineHandler;
            fBufferScope = bufferScope;
            fHtmlEncoder = htmlEncoder;
        }

        private async Task<ViewBufferTextWriter> RenderPageAsync(ITemplatePage page,
            PageContext context, bool invokeViewStarts)
        {
            if (!(context.Writer is ViewBufferTextWriter writer))
            {
                TkDebug.AssertNotNull(fBufferScope, $"nameof(_bufferScope) is null", this);

                // If we get here, this is likely the top-level page (not a partial) - this means
                // that context.Writer is wrapping the output stream. We need to buffer, so create a buffered writer.
                var buffer = new ViewBuffer(fBufferScope, page.Key, ViewBuffer.ViewPageSize);
                writer = new ViewBufferTextWriter(buffer, context.Writer.Encoding, fHtmlEncoder, context.Writer);
            }
            else
            {
                // This means we're writing something like a partial, where the output needs to be buffered.
                // Create a new buffer, but without the ability to flush.
                var buffer = new ViewBuffer(fBufferScope, page.Key, ViewBuffer.ViewPageSize);
                writer = new ViewBufferTextWriter(buffer, context.Writer.Encoding);
            }

            // The writer for the body is passed through the PageContext, allowing things like HtmlHelpers
            // and ViewComponents to reference it.
            var oldWriter = context.Writer;
            var oldFilePath = context.ExecutingPageKey;

            context.Writer = writer;
            context.ExecutingPageKey = page.Key;

            try
            {
                //Apply engine-global callbacks
                ExecutePageCallbacks(page, fEngineHandler.Options.PreRenderCallbacks.ToList());

                if (invokeViewStarts)
                {
                    // Execute view starts using the same context + writer as the page to render.
                    await RenderViewStartsAsync(context).ConfigureAwait(false);
                }

                await RenderPageCoreAsync(page, context).ConfigureAwait(false);
                return writer;
            }
            finally
            {
                context.Writer = oldWriter;
                context.ExecutingPageKey = oldFilePath;
            }
        }

        private async Task RenderPageCoreAsync(ITemplatePage page, PageContext context)
        {
            page.PageContext = context;
            page.IncludeFunc = async (key, model) =>
            {
                ITemplatePage template = await fEngineHandler.CompileTemplateAsync(key);

                await fEngineHandler.RenderIncludedTemplateAsync(template, model, context.Writer,
                    context.InitData, context.ViewBag, this);
            };

            //_pageActivator.Activate(page, context);

            await page.RunAsync().ConfigureAwait(false);
        }

        private Task RenderViewStartsAsync(PageContext context)
        {
            return Task.FromResult(0);
        }

        private async Task RenderLayoutAsync(ITemplatePage page, PageContext context, ViewBufferTextWriter bodyWriter)
        {
            //string layout = RazorPage.Layout;
            //if (!string.IsNullOrEmpty(layout))
            //{
            //    ITemplatePage layoutPage = await fEngineHandler.CompileTemplateAsync(layout).ConfigureAwait(false);
            //    layoutPage.RazorEngine = fEngineHandler.RazorEngine;
            //    layoutPage.PageContext = context;
            //    layoutPage.SetModel(context.Model);
            //    layoutPage.BodyContent = bodyWriter.Buffer;

            //    bodyWriter = await RenderPageAsync(layoutPage, context, invokeViewStarts: false).ConfigureAwait(false);
            //}
            //A layout page can specify another layout page. We'll need to continue
            // looking for layout pages until they're no longer specified.
            var previousPage = page;
            var renderedLayouts = new List<ITemplatePage>();

            // This loop will execute Layout pages from the inside to the outside. With each
            // iteration, bodyWriter is replaced with the aggregate of all the "body" content
            // (including the layout page we just rendered).
            while (!string.IsNullOrEmpty(previousPage.Layout))
            {
                if (!bodyWriter.IsBuffering)
                {
                    // Once a call to RazorPage.FlushAsync is made, we can no longer render Layout pages - content has
                    // already been written to the client and the layout content would be appended rather than surround
                    // the body content. Throwing this exception wouldn't return a 500 (since content has already been
                    // written), but a diagnostic component should be able to capture it.

                    throw new InvalidOperationException("Layout can not be rendered");
                }

                ITemplatePage layoutPage = await fEngineHandler.CompileTemplateAsync(previousPage.Layout).ConfigureAwait(false);
                layoutPage.RazorEngine = fEngineHandler.RazorEngine;
                layoutPage.PageContext = context;
                layoutPage.SetModel(context.Model);

                if (renderedLayouts.Count > 0 &&
                    renderedLayouts.Any(l => string.Equals(l.Key, layoutPage.Key, StringComparison.Ordinal)))
                {
                    // If the layout has been previously rendered as part of this view, we're potentially in a layout
                    // rendering cycle.
                    throw new InvalidOperationException($"Layout {layoutPage.Key} has circular reference");
                }

                // Notify the previous page that any writes that are performed on it are part of sections being written
                // in the layout.
                previousPage.IsLayoutBeingRendered = true;
                //layoutPage.PreviousSectionWriters = previousPage.SectionWriters;
                layoutPage.BodyContent = bodyWriter.Buffer;
                bodyWriter = await RenderPageAsync(layoutPage, context, invokeViewStarts: false).ConfigureAwait(false);

                renderedLayouts.Add(layoutPage);
                previousPage = layoutPage;
            }

            // Now we've reached and rendered the outer-most layout page. Nothing left to execute.
            // Ensure all defined sections were rendered or RenderBody was invoked for page without defined sections.
            foreach (var layoutPage in renderedLayouts)
            {
                layoutPage.EnsureRenderedBodyOrSections();
            }

            if (bodyWriter.IsBuffering)
            {
                // If IsBuffering - then we've got a bunch of content in the view buffer. How to best deal with it
                // really depends on whether or not we're writing directly to the output or if we're writing to
                // another buffer.
                if (!(context.Writer is ViewBufferTextWriter viewBufferTextWriter) || !viewBufferTextWriter.IsBuffering)
                {
                    // This means we're writing to a 'real' writer, probably to the actual output stream.
                    // We're using PagedBufferedTextWriter here to 'smooth' synchronous writes of IHtmlContent values.
                    using (var writer = fBufferScope.CreateWriter(context.Writer))
                    {
                        await bodyWriter.Buffer.WriteToAsync(writer, fHtmlEncoder).ConfigureAwait(false);
                    }
                }
                else
                {
                    // This means we're writing to another buffer. Use MoveTo to combine them.
                    bodyWriter.Buffer.MoveTo(viewBufferTextWriter.Buffer);
                }
            }
        }

        private void ExecutePageCallbacks(ITemplatePage page, IList<Action<ITemplatePage>> callbacks)
        {
            if (callbacks?.Count > 0)
            {
                foreach (var callback in callbacks)
                {
                    try
                    {
                        callback(page);
                    }
                    catch (Exception)
                    {
                        //Ignore
                    }
                }
            }
        }

        public virtual async Task RenderAsync(ITemplatePage page)
        {
            TkDebug.AssertArgumentNull(page, nameof(page), this);

            var context = page.PageContext;

            var bodyWriter = await RenderPageAsync(page, context, invokeViewStarts: false).ConfigureAwait(false);
            await RenderLayoutAsync(page, context, bodyWriter).ConfigureAwait(false);
        }
    }
}