using Microsoft.AspNetCore.Html;
using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class RazorOutputData
    {
        protected RazorOutputData()
        {
        }

        public RazorOutputData(RazorContentType contentType, string content)
        {
            TkDebug.AssertArgumentNull(content, "content", null);

            ContentType = contentType;
            Content = content;
        }

        [SimpleAttribute]
        public RazorContentType ContentType { get; protected set; }

        [TextContent]
        public string Content { get; protected set; }

        public HtmlString Execute<T>(TemplatePage template, T model)
        {
            switch (ContentType)
            {
                case RazorContentType.Section:
                    if (template.IsSectionDefined(Content))
                        return template.RenderSection(Content);
                    break;

                case RazorContentType.RazorFile:
                    return template.RenderPart(Content, model);

                case RazorContentType.Text:
                    return new HtmlString(Content);
            }
            return HtmlString.Empty;
        }
    }
}