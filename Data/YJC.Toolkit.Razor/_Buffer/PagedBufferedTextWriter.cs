using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class PagedBufferedTextWriter : TextWriter
    {
        private readonly TextWriter fInner;
        private readonly PagedCharBuffer fCharBuffer;

        public PagedBufferedTextWriter(ArrayPool<char> pool, TextWriter inner)
        {
            fCharBuffer = new PagedCharBuffer(new ArrayPoolBufferSource(pool));
            fInner = inner;
        }

        public override Encoding Encoding => fInner.Encoding;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                fCharBuffer.Dispose();

            base.Dispose(disposing);
        }

        public override void Flush()
        {
            // Don't do anything. We'll call FlushAsync.
        }

        public override async Task FlushAsync()
        {
            var length = fCharBuffer.Length;
            if (length == 0)
            {
                return;
            }

            var pages = fCharBuffer.Pages;
            for (var i = 0; i < pages.Count; i++)
            {
                var page = pages[i];
                var pageLength = Math.Min(length, page.Length);
                if (pageLength != 0)
                {
                    await fInner.WriteAsync(page, 0, pageLength);
                }

                length -= pageLength;
            }

            Debug.Assert(length == 0);
            fCharBuffer.Clear();
        }

        public override void Write(char value)
        {
            fCharBuffer.Append(value);
        }

        public override void Write(char[] buffer)
        {
            if (buffer == null)
                return;

            fCharBuffer.Append(buffer, 0, buffer.Length);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            TkDebug.AssertArgumentNull(buffer, nameof(buffer), this);

            fCharBuffer.Append(buffer, index, count);
        }

        public override void Write(string value)
        {
            if (value == null)
                return;

            fCharBuffer.Append(value);
        }

        public override async Task WriteAsync(char value)
        {
            await FlushAsync();
            await fInner.WriteAsync(value);
        }

        public override async Task WriteAsync(char[] buffer, int index, int count)
        {
            await FlushAsync();
            await fInner.WriteAsync(buffer, index, count);
        }

        public override async Task WriteAsync(string value)
        {
            await FlushAsync();
            await fInner.WriteAsync(value);
        }
    }
}