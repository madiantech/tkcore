using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YJC.Toolkit.Razor
{
    public interface ITemplatePage
    {
        PageContext PageContext { get; set; }

        IHtmlContent BodyContent { get; set; }

        bool DisableEncoding { get; set; }

        string Key { get; set; }

        bool IsLayoutBeingRendered { get; set; }

        IRazorEngine RazorEngine { get; set; }

        string Layout { get; set; }

        void SetModel(object model);

        Task RunAsync();

        Func<string, object, Task> IncludeFunc { get; set; }

        void EnsureRenderedBodyOrSections();

        HtmlString RenderPart(string key, object model = null);
    }
}