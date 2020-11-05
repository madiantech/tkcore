using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public abstract class BaseTemplatePage : ITemplatePage
    {
        private struct AttributeInfo
        {
            public AttributeInfo(string name, string prefix, int prefixOffset, string suffix,
                int suffixOffset, int attributeValuesCount)
            {
                Name = name;
                Prefix = prefix;
                PrefixOffset = prefixOffset;
                Suffix = suffix;
                SuffixOffset = suffixOffset;
                AttributeValuesCount = attributeValuesCount;

                Suppressed = false;
            }

            public int AttributeValuesCount { get; }

            public string Name { get; }

            public string Prefix { get; }

            public int PrefixOffset { get; }

            public string Suffix { get; }

            public int SuffixOffset { get; }

            public bool Suppressed { get; set; }
        }

        private struct TagHelperAttributeInfo
        {
            public TagHelperAttributeInfo(TagHelperExecutionContext tagHelperExecutionContext, string name,
                int attributeValuesCount, HtmlAttributeValueStyle attributeValueStyle)
            {
                ExecutionContext = tagHelperExecutionContext;
                Name = name;
                AttributeValuesCount = attributeValuesCount;
                AttributeValueStyle = attributeValueStyle;

                Suppressed = false;
            }

            public string Name { get; }

            public TagHelperExecutionContext ExecutionContext { get; }

            public int AttributeValuesCount { get; }

            public HtmlAttributeValueStyle AttributeValueStyle { get; }

            public bool Suppressed { get; set; }
        }

        private struct TagHelperScopeInfo
        {
            public TagHelperScopeInfo(ViewBuffer buffer, HtmlEncoder encoder, TextWriter writer)
            {
                Buffer = buffer;
                HtmlEncoder = encoder;
                Writer = writer;
            }

            public ViewBuffer Buffer { get; }

            public HtmlEncoder HtmlEncoder { get; }

            public TextWriter Writer { get; }
        }

        private readonly Stack<TextWriter> fTextWriterStack = new Stack<TextWriter>();
        private StringWriter fValueBuffer;

        private ITagHelperFactory fTagHelperFactory;
        private IViewBufferScope fBufferScope;

        private TextWriter fPageWriter;
        private AttributeInfo fAttributeInfo;
        private TagHelperAttributeInfo fTagHelperAttributeInfo;

        protected BaseTemplatePage()
        {
            //SectionWriters = new Dictionary<string, Func<Task>>(StringComparer.OrdinalIgnoreCase);
        }

        private Stack<TagHelperScopeInfo> TagHelperScopes { get; } = new Stack<TagHelperScopeInfo>();

        private ITagHelperFactory TagHelperFactory
        {
            get
            {
                if (fTagHelperFactory == null)
                {
                    //var services = ViewContext.HttpContext.RequestServices;
                    //_tagHelperFactory = services.GetRequiredService<ITagHelperFactory>();
                    fTagHelperFactory = new DefaultTagHelperFactory(new DefaultTagHelperActivator()); //TODO: replace cache with cached instance
                }

                return fTagHelperFactory;
            }
        }

        private IViewBufferScope BufferScope
        {
            get
            {
                if (fBufferScope == null)
                {
                    //TODO: replace with services maybe
                    //var services = ViewContext.HttpContext.RequestServices;
                    //_bufferScope = services.GetRequiredService<IViewBufferScope>();
                    fBufferScope = new MemoryPoolViewBufferScope(ArrayPool<ViewBufferValue>.Shared, ArrayPool<char>.Shared);
                }

                return fBufferScope;
            }
        }

        public virtual PageContext PageContext { get; set; }

        public IHtmlContent BodyContent { get; set; }

        public bool DisableEncoding { get; set; }

        public string Key { get; set; }

        public bool IsLayoutBeingRendered { get; set; }

        public string Layout { get; set; }

        public IRazorEngine RazorEngine { get; set; }

        //public IDictionary<string, Func<Task>> PreviousSectionWriters { get; set; }

        //public IDictionary<string, Func<Task>> SectionWriters { get; }

        public Func<string, object, Task> IncludeFunc { get; set; }

        public virtual dynamic ViewBag
        {
            get
            {
                if (PageContext == null)
                {
                    throw new InvalidOperationException();
                }

                return PageContext.ViewBag;
            }
        }

        public HtmlEncoder HtmlEncoder { get; set; } = HtmlEncoder.Default;

        public virtual TextWriter Output
        {
            get
            {
                if (PageContext == null)
                {
                    throw new InvalidOperationException();
                }

                return PageContext.Writer;
            }
        }

        public abstract void EnsureRenderedBodyOrSections();

        public abstract Task ExecuteAsync();

        public abstract void SetModel(object model);

        public abstract void BeginContext(int position, int length, bool isLiteral);

        public abstract void EndContext();

        public virtual Task RunAsync()
        {
            return ExecuteAsync();
        }

        public virtual async Task<HtmlString> FlushAsync()
        {
            // If there are active scopes, then we should throw. Cannot flush content that has the
            // potential to change.
            if (TagHelperScopes.Count > 0)
            {
                throw new InvalidOperationException();
            }

            // Calls to Flush are allowed if the page does not specify a Layout or if it is executing
            // a section in the Layout.
            if (!IsLayoutBeingRendered && !string.IsNullOrEmpty(Layout))
            {
                throw new InvalidOperationException();
            }

            await Output.FlushAsync();
            return HtmlString.Empty;
        }

        public IRawString Raw(string rawString)
        {
            return new RawString(rawString);
        }

        public TTagHelper CreateTagHelper<TTagHelper>() where TTagHelper : ITagHelper
        {
            return TagHelperFactory.CreateTagHelper<TTagHelper>(PageContext);
        }

        public void StartTagHelperWritingScope(HtmlEncoder encoder)
        {
            var buffer = new ViewBuffer(BufferScope, Key, ViewBuffer.TagHelperPageSize);
            TagHelperScopes.Push(new TagHelperScopeInfo(buffer, HtmlEncoder, PageContext.Writer));

            // If passed an HtmlEncoder, override the property.
            if (encoder != null)
            {
                HtmlEncoder = encoder;
            }

            // We need to replace the ViewContext's Writer to ensure that all content (including
            // content written from HTML helpers) is redirected.
            PageContext.Writer = new ViewBufferTextWriter(buffer, PageContext.Writer.Encoding);
        }

        public TagHelperContent EndTagHelperWritingScope()
        {
            if (TagHelperScopes.Count == 0)
            {
                throw new InvalidOperationException("There is no active scope to write");
            }

            var scopeInfo = TagHelperScopes.Pop();

            // Get the content written during the current scope.
            var tagHelperContent = new DefaultTagHelperContent();
            tagHelperContent.AppendHtml(scopeInfo.Buffer);

            // Restore previous scope.
            HtmlEncoder = scopeInfo.HtmlEncoder;
            PageContext.Writer = scopeInfo.Writer;

            return tagHelperContent;
        }

        public void BeginWriteTagHelperAttribute()
        {
            if (fPageWriter != null)
            {
                throw new InvalidOperationException("Nesting of attribute writing scope is not supported");
            }

            fPageWriter = PageContext.Writer;

            if (fValueBuffer == null)
            {
                fValueBuffer = new StringWriter();
            }

            // We need to replace the ViewContext's Writer to ensure that all content (including
            // content written from HTML helpers) is redirected.
            PageContext.Writer = fValueBuffer;
        }

        public string EndWriteTagHelperAttribute()
        {
            if (fPageWriter == null)
            {
                throw new InvalidOperationException("There is no active writing scope to end");
            }

            var content = fValueBuffer.ToString();
            fValueBuffer.GetStringBuilder().Clear();

            // Restore previous writer.
            PageContext.Writer = fPageWriter;
            fPageWriter = null;

            return content;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void DefineSection(string name, Func<object, Task> section)
            => DefineSection(name, () => section(null /* writer */));

        public virtual void DefineSection(string name, Func<Task> section)
        {
            //TkDebug.AssertArgumentNull(name, nameof(name), this);
            //TkDebug.AssertArgumentNull(section, nameof(section), this);

            //if (SectionWriters.ContainsKey(name))
            //{
            //    throw new InvalidOperationException();
            //}
            //SectionWriters[name] = section;
            PageContext.DefineSection(name, section);
        }

        public virtual void Write(object value)
        {
            if (value == null || value == HtmlString.Empty)
            {
                return;
            }

            var writer = Output;
            var encoder = HtmlEncoder;

            switch (value)
            {
                case IRawString raw:
                    raw.WriteTo(writer);
                    break;

                case IHtmlContent html:
                    var bufferedWriter = writer as ViewBufferTextWriter;
                    if (bufferedWriter == null || !bufferedWriter.IsBuffering)
                    {
                        html.WriteTo(writer, encoder);
                    }
                    else
                    {
                        if (value is IHtmlContentContainer htmlContentContainer)
                        {
                            // This is likely another ViewBuffer.
                            htmlContentContainer.MoveTo(bufferedWriter.Buffer);
                        }
                        else
                        {
                            // Perf: This is the common case for IHtmlContent, ViewBufferTextWriter
                            //       is inefficient for writing character by character.
                            bufferedWriter.Buffer.AppendHtml(html);
                        }
                    }
                    break;

                default:
                    Write(value.ToString());
                    break;
            }
        }

        public virtual void Write(string value)
        {
            var writer = Output;
            var encoder = HtmlEncoder;
            if (!string.IsNullOrEmpty(value))
            {
                // Perf: Encode right away instead of writing it character-by-character.
                // character-by-character isn't efficient when using a writer backed by a ViewBuffer.
                var encoded = DisableEncoding ? value : encoder.Encode(value);
                writer.Write(encoded);
            }
        }

        public virtual void WriteLiteral(object value)
        {
            if (value == null)
            {
                return;
            }

            WriteLiteral(value.ToString());
        }

        public virtual void WriteLiteral(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Output.Write(value);
            }
        }

        protected internal virtual void PushWriter(TextWriter writer)
        {
            TkDebug.AssertArgumentNull(writer, nameof(writer), this);

            fTextWriterStack.Push(PageContext.Writer);
            PageContext.Writer = writer;
        }

        protected internal virtual TextWriter PopWriter()
        {
            PageContext.Writer = fTextWriterStack.Pop();
            return PageContext.Writer;
        }

        public virtual void BeginWriteAttribute(string name, string prefix, int prefixOffset,
            string suffix, int suffixOffset, int attributeValuesCount)
        {
            TkDebug.AssertArgumentNull(prefix, nameof(prefix), this);
            TkDebug.AssertArgumentNull(suffix, nameof(suffix), this);

            fAttributeInfo = new AttributeInfo(name, prefix, prefixOffset, suffix, suffixOffset, attributeValuesCount);

            // Single valued attributes might be omitted in entirety if it the attribute value
            // strictly evaluates to null or false. Consequently defer the prefix generation until we
            // encounter the attribute value.
            if (attributeValuesCount != 1)
            {
                WritePositionTaggedLiteral(prefix, prefixOffset);
            }
        }

        public void WriteAttributeValue(string prefix, int prefixOffset, object value,
            int valueOffset, int valueLength, bool isLiteral)
        {
            if (fAttributeInfo.AttributeValuesCount == 1)
            {
                if (IsBoolFalseOrNullValue(prefix, value))
                {
                    // Value is either null or the bool 'false' with no prefix; don't render the attribute.
                    fAttributeInfo.Suppressed = true;
                    return;
                }

                // We are not omitting the attribute. Write the prefix.
                WritePositionTaggedLiteral(fAttributeInfo.Prefix, fAttributeInfo.PrefixOffset);

                if (IsBoolTrueWithEmptyPrefixValue(prefix, value))
                {
                    // The value is just the bool 'true', write the attribute name instead of the
                    // string 'True'.
                    value = fAttributeInfo.Name;
                }
            }

            // This block handles two cases.
            // 1. Single value with prefix.
            // 2. Multiple values with or without prefix.
            if (value != null)
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    WritePositionTaggedLiteral(prefix, prefixOffset);
                }

                BeginContext(valueOffset, valueLength, isLiteral);

                WriteUnprefixedAttributeValue(value, isLiteral);

                EndContext();
            }
        }

        public virtual void EndWriteAttribute()
        {
            if (!fAttributeInfo.Suppressed)
            {
                WritePositionTaggedLiteral(fAttributeInfo.Suffix, fAttributeInfo.SuffixOffset);
            }
        }

        public void BeginAddHtmlAttributeValues(TagHelperExecutionContext executionContext,
            string attributeName, int attributeValuesCount, HtmlAttributeValueStyle attributeValueStyle)
        {
            fTagHelperAttributeInfo = new TagHelperAttributeInfo(executionContext, attributeName,
                attributeValuesCount, attributeValueStyle);
        }

        public void AddHtmlAttributeValue(string prefix, int prefixOffset, object value,
            int valueOffset, int valueLength, bool isLiteral)
        {
            //Debug.Assert(_tagHelperAttributeInfo.ExecutionContext != null);
            if (fTagHelperAttributeInfo.AttributeValuesCount == 1)
            {
                if (IsBoolFalseOrNullValue(prefix, value))
                {
                    // The first value was 'null' or 'false' indicating that we shouldn't render the
                    // attribute. The attribute is treated as a TagHelper attribute so it's only
                    // available in TagHelperContext.AllAttributes for TagHelper authors to see (if
                    // they want to see why the attribute was removed from TagHelperOutput.Attributes).
                    fTagHelperAttributeInfo.ExecutionContext.AddTagHelperAttribute(fTagHelperAttributeInfo.Name,
                        value?.ToString() ?? string.Empty, fTagHelperAttributeInfo.AttributeValueStyle);
                    fTagHelperAttributeInfo.Suppressed = true;
                    return;
                }
                else if (IsBoolTrueWithEmptyPrefixValue(prefix, value))
                {
                    fTagHelperAttributeInfo.ExecutionContext.AddHtmlAttribute(fTagHelperAttributeInfo.Name,
                        fTagHelperAttributeInfo.Name, fTagHelperAttributeInfo.AttributeValueStyle);
                    fTagHelperAttributeInfo.Suppressed = true;
                    return;
                }
            }

            if (value != null)
            {
                // Perf: We'll use this buffer for all of the attribute values and then clear it to
                // reduce allocations.
                if (fValueBuffer == null)
                {
                    fValueBuffer = new StringWriter();
                }

                PushWriter(fValueBuffer);
                if (!string.IsNullOrEmpty(prefix))
                {
                    WriteLiteral(prefix);
                }

                WriteUnprefixedAttributeValue(value, isLiteral);
                PopWriter();
            }
        }

        public void EndAddHtmlAttributeValues(TagHelperExecutionContext executionContext)
        {
            if (!fTagHelperAttributeInfo.Suppressed)
            {
                // Perf: _valueBuffer might be null if nothing was written. If it is set, clear it so
                // it is reset for the next value.
                var content = fValueBuffer == null ? HtmlString.Empty : new HtmlString(fValueBuffer.ToString());
                fValueBuffer?.GetStringBuilder().Clear();

                executionContext.AddHtmlAttribute(fTagHelperAttributeInfo.Name, content, fTagHelperAttributeInfo.AttributeValueStyle);
            }
        }

        private void WriteUnprefixedAttributeValue(object value, bool isLiteral)
        {
            var stringValue = value as string;

            // The extra branching here is to ensure that we call the Write*To(string) overload where possible.
            if (isLiteral && stringValue != null)
            {
                WriteLiteral(stringValue);
            }
            else if (isLiteral)
            {
                WriteLiteral(value);
            }
            else if (stringValue != null)
            {
                Write(stringValue);
            }
            else
            {
                Write(value);
            }
        }

        private void WritePositionTaggedLiteral(string value, int position)
        {
            BeginContext(position, value.Length, isLiteral: true);
            WriteLiteral(value);
            EndContext();
        }

        private bool IsBoolFalseOrNullValue(string prefix, object value)
        {
            return string.IsNullOrEmpty(prefix) &&
                (value == null || (value is bool && !(bool)value));
        }

        private bool IsBoolTrueWithEmptyPrefixValue(string prefix, object value)
        {
            // If the value is just the bool 'true', use the attribute name as the value.
            return string.IsNullOrEmpty(prefix) &&
                (value is bool && (bool)value);
        }

        protected virtual string PrepareKey(string key)
        {
            return key;
        }

        public HtmlString RenderPart(string key, object model = null)
        {
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), this);

            key = PrepareKey(key);
            var result = Task.Run(async () => await InternalRenderPart(key, model)).GetAwaiter().GetResult();
            return result;
        }

        private async Task<HtmlString> InternalRenderPart(string key, object model)
        {
            ITemplatePage templatePage = await RazorEngine.CompileTemplateAsync(key);
            templatePage.RazorEngine = RazorEngine;
            templatePage.PageContext = PageContext;
            var objModel = PageContext.ModelTypeInfo.CreateTemplateModel(model ?? PageContext.Model);

            var result = await RazorEngine.Handler.RenderTemplateAsync(templatePage, objModel, PageContext);
            return new HtmlString(result);
        }
    }
}