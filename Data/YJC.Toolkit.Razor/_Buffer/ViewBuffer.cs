using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [DebuggerDisplay("{DebuggerToString()}")]
    internal class ViewBuffer : IHtmlContentBuilder
    {
        private class EncodingWrapper : IHtmlContent
        {
            private readonly string fUnencoded;

            public EncodingWrapper(string unencoded)
            {
                fUnencoded = unencoded;
            }

            public void WriteTo(TextWriter writer, HtmlEncoder encoder)
            {
                encoder.Encode(writer, fUnencoded);
            }
        }

        private const int DEFAULT_BUFFER_SIZE = 2048;

        public static readonly int PartialViewPageSize = 32;
        public static readonly int TagHelperPageSize = 32;
        public static readonly int ViewComponentPageSize = 32;
        public static readonly int ViewPageSize = ReadDefaultBufferSize();

        private readonly IViewBufferScope fBufferScope;
        private readonly string fName;
        private readonly int fPageSize;
        private ViewBufferPage fCurrentPage;         // Limits allocation if the ViewBuffer has only one page (frequent case).
        private List<ViewBufferPage> fMultiplePages; // Allocated only if necessary

        public ViewBuffer(IViewBufferScope bufferScope, string name, int pageSize)
        {
            TkDebug.AssertArgumentNull(bufferScope, nameof(bufferScope), null);
            TkDebug.AssertArgument(pageSize > 0, nameof(pageSize), $"{nameof(pageSize)}必须大于0", null);

            fBufferScope = bufferScope;
            fName = name;
            fPageSize = pageSize;
        }

        public int Count
        {
            get
            {
                if (fMultiplePages != null)
                    return fMultiplePages.Count;
                if (fCurrentPage != null)
                    return 1;
                return 0;
            }
        }

        public ViewBufferPage this[int index]
        {
            get
            {
                if (fMultiplePages != null)
                    return fMultiplePages[index];
                if (index == 0 && fCurrentPage != null)
                    return fCurrentPage;

                throw new IndexOutOfRangeException();
            }
        }

        private void AddPage(ViewBufferPage page)
        {
            if (fMultiplePages != null)
                fMultiplePages.Add(page);
            else if (fCurrentPage != null)
            {
                fMultiplePages = new List<ViewBufferPage>(2);
                fMultiplePages.Add(fCurrentPage);
                fMultiplePages.Add(page);
            }

            fCurrentPage = page;
        }

        private ViewBufferPage GetCurrentPage()
        {
            if (fCurrentPage == null || fCurrentPage.IsFull)
            {
                AddPage(new ViewBufferPage(fBufferScope.GetPage(fPageSize)));
            }
            return fCurrentPage;
        }

        private string DebuggerToString() => fName;

        private void AppendValue(ViewBufferValue value)
        {
            var page = GetCurrentPage();
            page.Append(value);
        }

        private void MoveTo(ViewBuffer destination)
        {
            for (var i = 0; i < Count; i++)
            {
                var page = this[i];

                var destinationPage = destination.Count == 0 ? null : destination[destination.Count - 1];

                // If the source page is less or equal to than half full, let's copy it's content to the destination
                // page if possible.
                var isLessThanHalfFull = 2 * page.Count <= page.Capacity;
                if (isLessThanHalfFull && destinationPage != null &&
                    (destinationPage.Capacity - destinationPage.Count >= page.Count))
                {
                    // We have room, let's copy the items.
                    Array.Copy(page.Buffer, 0, destinationPage.Buffer, destinationPage.Count, page.Count);

                    destinationPage.Count += page.Count;

                    // Now we can return the source page, and it can be reused in the scope of this request.
                    Array.Clear(page.Buffer, 0, page.Count);
                    fBufferScope.ReturnSegment(page.Buffer);
                }
                else
                {
                    // Otherwise, let's just add the source page to the other buffer.
                    destination.AddPage(page);
                }
            }

            Clear();
        }

        public IHtmlContentBuilder Append(string unencoded)
        {
            if (unencoded == null)
                return this;

            AppendValue(new ViewBufferValue(new EncodingWrapper(unencoded)));
            return this;
        }

        public IHtmlContentBuilder AppendHtml(IHtmlContent content)
        {
            if (content == null)
                return this;

            AppendValue(new ViewBufferValue(content));
            return this;
        }

        public IHtmlContentBuilder AppendHtml(string encoded)
        {
            if (encoded == null)
                return this;

            AppendValue(new ViewBufferValue(encoded));
            return this;
        }

        public IHtmlContentBuilder Clear()
        {
            fMultiplePages = null;
            fCurrentPage = null;
            return this;
        }

        public void CopyTo(IHtmlContentBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, nameof(builder), this);

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];

                    IHtmlContentContainer valueAsContainer;
                    if (value.Value is string valueAsString)
                        builder.AppendHtml(valueAsString);
                    else if ((valueAsContainer = value.Value as IHtmlContentContainer) != null)
                        valueAsContainer.CopyTo(builder);
                    else
                        builder.AppendHtml((IHtmlContent)value.Value);
                }
            }
        }

        public void MoveTo(IHtmlContentBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, nameof(builder), this);

            // Perf: We have an efficient implementation when the destination is another view buffer,
            // we can just insert our pages as-is.
            if (builder is ViewBuffer other)
            {
                MoveTo(other);
                return;
            }

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];

                    string valueAsString;
                    IHtmlContentContainer valueAsContainer;
                    if ((valueAsString = value.Value as string) != null)
                    {
                        builder.AppendHtml(valueAsString);
                    }
                    else if ((valueAsContainer = value.Value as IHtmlContentContainer) != null)
                    {
                        valueAsContainer.MoveTo(builder);
                    }
                    else
                    {
                        builder.AppendHtml((IHtmlContent)value.Value);
                    }
                }
            }

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                Array.Clear(page.Buffer, 0, page.Count);
                fBufferScope.ReturnSegment(page.Buffer);
            }

            Clear();
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            TkDebug.AssertArgumentNull(writer, nameof(writer), this);
            TkDebug.AssertArgumentNull(encoder, nameof(encoder), this);

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];

                    if (value.Value is string valueAsString)
                    {
                        writer.Write(valueAsString);
                        continue;
                    }

                    if (value.Value is IHtmlContent valueAsHtmlContent)
                    {
                        valueAsHtmlContent.WriteTo(writer, encoder);
                        continue;
                    }
                }
            }
        }

        public async Task WriteToAsync(TextWriter writer, HtmlEncoder encoder)
        {
            TkDebug.AssertArgumentNull(writer, nameof(writer), this);
            TkDebug.AssertArgumentNull(encoder, nameof(encoder), this);

            for (var i = 0; i < Count; i++)
            {
                var page = this[i];
                for (var j = 0; j < page.Count; j++)
                {
                    var value = page.Buffer[j];
                    if (value == null)
                        continue;
                    if (value.Value is string valueAsString)
                    {
                        await writer.WriteAsync(valueAsString);
                        continue;
                    }

                    if (value.Value is ViewBuffer valueAsViewBuffer)
                    {
                        await valueAsViewBuffer.WriteToAsync(writer, encoder);
                        continue;
                    }

                    if (value.Value is IHtmlContent valueAsHtmlContent)
                    {
                        valueAsHtmlContent.WriteTo(writer, encoder);
                        await writer.FlushAsync();
                        continue;
                    }
                }
            }
        }

        private static int ReadDefaultBufferSize()
        {
            TkDebug.ThrowIfNoGlobalVariable();

            int buffer = BaseGlobalVariable.Current.DefaultValue.GetSimpleDefaultValue(
                "DefaultRazorBufferSize").Value<int>(DEFAULT_BUFFER_SIZE);
            return buffer;
        }
    }
}