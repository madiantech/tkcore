using System.Buffers;

namespace YJC.Toolkit.Razor
{
    internal class ArrayPoolBufferSource : ICharBufferSource
    {
        private readonly ArrayPool<char> fPool;

        public ArrayPoolBufferSource(ArrayPool<char> pool)
        {
            fPool = pool;
        }

        public char[] Rent(int bufferSize) => fPool.Rent(bufferSize);

        public void Return(char[] buffer) => fPool.Return(buffer);
    }
}