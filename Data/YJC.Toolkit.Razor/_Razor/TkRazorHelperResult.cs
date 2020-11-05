using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class TkRazorHelperResult : IHtmlContent
    {
        private readonly Func<TextWriter, Task> fWriteAction;

        public TkRazorHelperResult(Func<TextWriter, Task> asyncAction)
        {
            TkDebug.AssertArgumentNull(asyncAction, nameof(asyncAction), null);

            fWriteAction = asyncAction;
        }

        public virtual void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            TkDebug.AssertArgumentNull(writer, nameof(writer), this);
            TkDebug.AssertArgumentNull(encoder, nameof(encoder), this);

            fWriteAction(writer).GetAwaiter().GetResult();
        }
    }
}