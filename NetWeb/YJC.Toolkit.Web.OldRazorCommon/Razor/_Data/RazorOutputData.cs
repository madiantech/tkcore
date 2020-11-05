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

        public string Execute(RazorFileTemplate template, object model)
        {
            switch (ContentType)
            {
                case RazorContentType.Section:
                    if (template.IsSectionDefined(Content))
                        return template.RenderSection(Content, model);
                    break;
                case RazorContentType.RazorFile:
                    return template.RenderLocalPartial(Content, model);
                case RazorContentType.Text:
                    return Content;
            }
            return string.Empty;
        }
    }
}
