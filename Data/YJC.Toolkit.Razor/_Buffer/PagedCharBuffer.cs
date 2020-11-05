using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace YJC.Toolkit.Razor
{
    internal class PagedCharBuffer : IDisposable
    {
        public const int PageSize = 1024;
        private int fCharIndex;
        private char[] fCurrentPage;

        public PagedCharBuffer(ICharBufferSource bufferSource)
        {
            BufferSource = bufferSource;
        }

        public ICharBufferSource BufferSource { get; }

        public IList<char[]> Pages { get; } = new List<char[]>();

        public int Length
        {
            get
            {
                var length = fCharIndex;
                for (var i = 0; i < Pages.Count - 1; i++)
                {
                    length += Pages[i].Length;
                }

                return length;
            }
        }

        private char[] GetCurrentPage()
        {
            if (fCurrentPage == null || fCharIndex == fCurrentPage.Length)
            {
                fCurrentPage = NewPage();
                fCharIndex = 0;
            }

            return fCurrentPage;
        }

        private char[] NewPage()
        {
            char[] page = null;
            try
            {
                page = BufferSource.Rent(PageSize);
                Pages.Add(page);
            }
            catch when (page != null)
            {
                BufferSource.Return(page);
                throw;
            }

            return page;
        }

        public void Append(char value)
        {
            var page = GetCurrentPage();
            page[fCharIndex++] = value;
        }

        public void Append(string value)
        {
            if (value == null)
                return;

            var index = 0;
            var count = value.Length;

            while (count > 0)
            {
                var page = GetCurrentPage();
                var copyLength = Math.Min(count, page.Length - fCharIndex);

                Debug.Assert(copyLength > 0);

                value.CopyTo(index, page, fCharIndex, copyLength);

                fCharIndex += copyLength;
                index += copyLength;

                count -= copyLength;
            }
        }

        public void Append(char[] buffer, int index, int count)
        {
            while (count > 0)
            {
                var page = GetCurrentPage();
                var copyLength = Math.Min(count, page.Length - fCharIndex);
                Debug.Assert(copyLength > 0);

                Array.Copy(buffer, index, page, fCharIndex, copyLength);

                fCharIndex += copyLength;
                index += copyLength;
                count -= copyLength;
            }
        }

        public void Clear()
        {
            for (var i = Pages.Count - 1; i > 0; i--)
            {
                var page = Pages[i];

                try
                {
                    Pages.RemoveAt(i);
                }
                finally
                {
                    BufferSource.Return(page);
                }
            }

            fCharIndex = 0;
            fCurrentPage = Pages.Count > 0 ? Pages[0] : null;
        }

        public void Dispose()
        {
            for (var i = 0; i < Pages.Count; i++)
            {
                BufferSource.Return(Pages[i]);
            }

            Pages.Clear();
        }
    }
}