using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class MemoryPoolViewBufferScope : IViewBufferScope, IDisposable
    {
        public static readonly int MinimumSize = 16;
        private readonly ArrayPool<ViewBufferValue> fViewBufferPool;
        private readonly ArrayPool<char> fCharPool;
        private List<ViewBufferValue[]> fAvailable;
        private List<ViewBufferValue[]> fLeased;
        private bool fDisposed;

        public MemoryPoolViewBufferScope(ArrayPool<ViewBufferValue> viewBufferPool, ArrayPool<char> charPool)
        {
            fViewBufferPool = viewBufferPool;
            fCharPool = charPool;
        }

        public MemoryPoolViewBufferScope()
        {
            fViewBufferPool = ArrayPool<ViewBufferValue>.Shared;
            fCharPool = ArrayPool<char>.Shared;
        }

        public PagedBufferedTextWriter CreateWriter(TextWriter writer)
        {
            TkDebug.AssertArgumentNull(writer, nameof(writer), this);

            return new PagedBufferedTextWriter(fCharPool, writer);
        }

        public void Dispose()
        {
            if (!fDisposed)
            {
                fDisposed = true;

                if (fLeased == null)
                {
                    return;
                }

                for (var i = 0; i < fLeased.Count; i++)
                {
                    fViewBufferPool.Return(fLeased[i], clearArray: true);
                }

                fLeased.Clear();
            }
        }

        public ViewBufferValue[] GetPage(int pageSize)
        {
            TkDebug.AssertArgument(pageSize > 0, nameof(pageSize),
                $"{nameof(pageSize)}必须大于0，当前值是{pageSize}", this);

            if (fDisposed)
            {
                throw new ObjectDisposedException(typeof(MemoryPoolViewBufferScope).FullName);
            }

            if (fLeased == null)
                fLeased = new List<ViewBufferValue[]>(1);

            ViewBufferValue[] segment = null;

            // Reuse pages that have been returned before going back to the memory pool.
            if (fAvailable != null && fAvailable.Count > 0)
            {
                segment = fAvailable[fAvailable.Count - 1];
                fAvailable.RemoveAt(fAvailable.Count - 1);
                return segment;
            }

            try
            {
                segment = fViewBufferPool.Rent(Math.Max(pageSize, MinimumSize));
                fLeased.Add(segment);
            }
            catch when (segment != null)
            {
                fViewBufferPool.Return(segment);
                throw;
            }

            return segment;
        }

        public void ReturnSegment(ViewBufferValue[] segment)
        {
            TkDebug.AssertArgumentNull(segment, nameof(segment), this);

            Array.Clear(segment, 0, segment.Length);

            if (fAvailable == null)
                fAvailable = new List<ViewBufferValue[]>();

            fAvailable.Add(segment);
        }
    }
}