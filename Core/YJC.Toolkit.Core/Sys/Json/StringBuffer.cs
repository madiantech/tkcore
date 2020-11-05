using System;

namespace YJC.Toolkit.Sys.Json
{
    internal class StringBuffer
    {
        private char[] fBuffer;
        private readonly static char[] fEmptyBuffer = new char[0];

        public StringBuffer()
        {
            fBuffer = fEmptyBuffer;
        }

        public StringBuffer(int initalSize)
        {
            fBuffer = new char[initalSize];
        }

        public int Position { get; set; }

        public void Append(char value)
        {
            // test if the buffer array is large enough to take the value
            if (Position + 1 > fBuffer.Length)
                EnsureSize(1);

            // set value and increment poisition
            fBuffer[Position++] = value;
        }

        public void Clear()
        {
            fBuffer = fEmptyBuffer;
            Position = 0;
        }

        private void EnsureSize(int appendLength)
        {
            char[] newBuffer = new char[(Position + appendLength) << 1]; // * 2

            Array.Copy(fBuffer, newBuffer, Position);

            fBuffer = newBuffer;
        }

        public override string ToString()
        {
            return ToString(0, Position);
        }

        public string ToString(int start, int length)
        {
            return new string(fBuffer, start, length);
        }
    }
}