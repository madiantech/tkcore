using Microsoft.AspNetCore.Html;
using System.Diagnostics;
using System.IO;
using System.Text.Encodings.Web;

namespace YJC.Toolkit.Razor
{
    [DebuggerDisplay("{DebuggerToString()}")]
    internal class ViewBufferValue
    {
        public ViewBufferValue(string value)
        {
            Value = value;
        }

        public ViewBufferValue(IHtmlContent content)
        {
            Value = content;
        }

        public object Value { get; }

        private string DebuggerToString()
        {
            using (var writer = new StringWriter())
            {
                var valueAsString = Value as string;
                if (valueAsString != null)
                {
                    writer.Write(valueAsString);
                    return writer.ToString();
                }

                var valueAsContent = Value as IHtmlContent;
                if (valueAsContent != null)
                {
                    valueAsContent.WriteTo(writer, HtmlEncoder.Default);
                    return writer.ToString();
                }

                return "(null)";
            }
        }
    }
}