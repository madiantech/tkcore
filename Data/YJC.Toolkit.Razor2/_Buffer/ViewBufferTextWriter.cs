using Microsoft.AspNetCore.Html;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class ViewBufferTextWriter : TextWriter
    {
        private readonly TextWriter fInner;
        private readonly HtmlEncoder fHtmlEncoder;

        public ViewBufferTextWriter(ViewBuffer buffer, Encoding encoding)
        {
            TkDebug.AssertArgumentNull(buffer, nameof(buffer), null);
            TkDebug.AssertArgumentNull(encoding, nameof(encoding), null);

            Buffer = buffer;
            Encoding = encoding;
        }

        public ViewBufferTextWriter(ViewBuffer buffer, Encoding encoding, HtmlEncoder htmlEncoder, TextWriter inner)
        {
            TkDebug.AssertArgumentNull(buffer, nameof(buffer), null);
            TkDebug.AssertArgumentNull(encoding, nameof(encoding), null);
            TkDebug.AssertArgumentNull(htmlEncoder, nameof(htmlEncoder), null);
            TkDebug.AssertArgumentNull(inner, nameof(inner), null);

            Buffer = buffer;
            Encoding = encoding;
            fHtmlEncoder = htmlEncoder;
            fInner = inner;
        }

        public override Encoding Encoding { get; }

        public bool IsBuffering { get; private set; } = true;

        public ViewBuffer Buffer { get; }

        public override void Write(char value)
        {
            if (IsBuffering)
                Buffer.AppendHtml(value.ToString());
            else
                fInner.Write(value);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            TkDebug.AssertArgumentNull(buffer, nameof(buffer), this);
            TkDebug.AssertArgument(index >= 0 && index < buffer.Length, nameof(index),
                $"{nameof(index)}必须在0和{buffer.Length}之间，当前值为{index}", this);
            TkDebug.AssertArgument(count >= 0 && count < buffer.Length, nameof(count),
                $"{nameof(count)}必须在0和{buffer.Length}之间，当前值为{count}", this);

            if (IsBuffering)
                Buffer.AppendHtml(new string(buffer, index, count));
            else
                fInner.Write(buffer, index, count);
        }

        public override void Write(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (IsBuffering)
                Buffer.AppendHtml(value);
            else
                fInner.Write(value);
        }

        public override void Write(object value)
        {
            if (value == null)
                return;

            IHtmlContentContainer container;
            IHtmlContent content;
            if ((container = value as IHtmlContentContainer) != null)
                Write(container);
            else if ((content = value as IHtmlContent) != null)
                Write(content);
            else
                Write(value.ToString());
        }

        public void Write(IHtmlContent value)
        {
            if (value == null)
                return;

            if (IsBuffering)
                Buffer.AppendHtml(value);
            else
                value.WriteTo(fInner, fHtmlEncoder);
        }

        public void Write(IHtmlContentContainer value)
        {
            if (value == null)
                return;

            if (IsBuffering)
                value.MoveTo(Buffer);
            else
                value.WriteTo(fInner, fHtmlEncoder);
        }

        public override void WriteLine(object value)
        {
            if (value == null)
                return;

            IHtmlContentContainer container;
            IHtmlContent content;
            if ((container = value as IHtmlContentContainer) != null)
            {
                Write(container);
                Write(NewLine);
            }
            else if ((content = value as IHtmlContent) != null)
            {
                Write(content);
                Write(NewLine);
            }
            else
            {
                Write(value.ToString());
                Write(NewLine);
            }
        }

        public override Task WriteAsync(char value)
        {
            if (IsBuffering)
            {
                Buffer.AppendHtml(value.ToString());
                return Task.CompletedTask;
            }
            else
                return fInner.WriteAsync(value);
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            TkDebug.AssertArgumentNull(buffer, nameof(buffer), this);
            TkDebug.AssertArgument(index >= 0 && index < buffer.Length, nameof(index),
                $"{nameof(index)}必须在0和{buffer.Length}之间，当前值为{index}", this);
            TkDebug.AssertArgument(count >= 0 && count <= buffer.Length - index, nameof(count),
                $"{nameof(count)}必须在0和{buffer.Length - index}之间，当前值为{count}", this);

            if (IsBuffering)
            {
                Buffer.AppendHtml(new string(buffer, index, count));
                return Task.CompletedTask;
            }
            else
                return fInner.WriteAsync(buffer, index, count);
        }

        public override Task WriteAsync(string value)
        {
            if (IsBuffering)
            {
                Buffer.AppendHtml(value);
                return Task.CompletedTask;
            }
            else
            {
                return fInner.WriteAsync(value);
            }
        }

        public override void WriteLine()
        {
            if (IsBuffering)
            {
                Buffer.AppendHtml(NewLine);
            }
            else
            {
                fInner.WriteLine();
            }
        }

        public override void WriteLine(string value)
        {
            if (IsBuffering)
            {
                Buffer.AppendHtml(value);
                Buffer.AppendHtml(NewLine);
            }
            else
            {
                fInner.WriteLine(value);
            }
        }

        public override Task WriteLineAsync(char value)
        {
            if (IsBuffering)
            {
                Buffer.AppendHtml(value.ToString());
                Buffer.AppendHtml(NewLine);
                return Task.CompletedTask;
            }
            else
            {
                return fInner.WriteLineAsync(value);
            }
        }

        public override Task WriteLineAsync(char[] value, int start, int offset)
        {
            TkDebug.AssertArgumentNull(value, nameof(value), this);

            if (IsBuffering)
            {
                Buffer.AppendHtml(new string(value, start, offset));
                Buffer.AppendHtml(NewLine);
                return Task.CompletedTask;
            }
            else
            {
                return fInner.WriteLineAsync(value, start, offset);
            }
        }

        public override Task WriteLineAsync(string value)
        {
            if (IsBuffering)
            {
                Buffer.AppendHtml(value);
                Buffer.AppendHtml(NewLine);
                return Task.CompletedTask;
            }
            else
            {
                return fInner.WriteLineAsync(value);
            }
        }

        public override Task WriteLineAsync()
        {
            if (IsBuffering)
            {
                Buffer.AppendHtml(NewLine);
                return Task.CompletedTask;
            }
            else
            {
                return fInner.WriteLineAsync();
            }
        }

        public override void Flush()
        {
            if (fInner == null || fInner is ViewBufferTextWriter)
                return;

            if (IsBuffering)
            {
                IsBuffering = false;
                Buffer.WriteTo(fInner, fHtmlEncoder);
                Buffer.Clear();
            }

            fInner.Flush();
        }

        public override async Task FlushAsync()
        {
            if (fInner == null || fInner is ViewBufferTextWriter)
                return;

            if (IsBuffering)
            {
                IsBuffering = false;
                await Buffer.WriteToAsync(fInner, fHtmlEncoder);
                Buffer.Clear();
            }

            await fInner.FlushAsync();
        }
    }
}