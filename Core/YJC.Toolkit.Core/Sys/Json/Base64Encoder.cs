using System;
using System.IO;

namespace YJC.Toolkit.Sys.Json
{
    internal class Base64Encoder
    {
        private const int BASE64_LINE_SIZE = 76;
        private const int LINE_SIZE_IN_BYTES = 57;

        private readonly char[] fCharsLine = new char[BASE64_LINE_SIZE];
        private readonly TextWriter fWriter;

        private byte[] fLeftOverBytes;
        private int fLeftOverBytesCount;

        public Base64Encoder(TextWriter writer)
        {
            TkDebug.AssertArgumentNull(writer, "writer", null);
            fWriter = writer;
        }

        private void WriteChars(char[] chars, int index, int count)
        {
            fWriter.Write(chars, index, count);
        }

        public void Encode(byte[] buffer, int index, int count)
        {
            TkDebug.AssertArgumentNull(buffer, "buffer", this);
            TkDebug.AssertArgument(index >= 0, "index", "参数index必须非负数", this);
            TkDebug.AssertArgument(count >= 0 && count <= (buffer.Length - index), "count",
                string.Format(ObjectUtil.SysCulture, "参数count必须在0和{0}之间", buffer.Length - index), this);

            //if (index < 0)
            //    throw new ArgumentOutOfRangeException("index");

            //if (count < 0)
            //    throw new ArgumentOutOfRangeException("count");

            //if (count > (buffer.Length - index))
            //    throw new ArgumentOutOfRangeException("count");

            if (fLeftOverBytesCount > 0)
            {
                int leftOverBytesCount = fLeftOverBytesCount;
                while (leftOverBytesCount < 3 && count > 0)
                {
                    fLeftOverBytes[leftOverBytesCount++] = buffer[index++];
                    count--;
                }
                if (count == 0 && leftOverBytesCount < 3)
                {
                    fLeftOverBytesCount = leftOverBytesCount;
                    return;
                }
                int num2 = Convert.ToBase64CharArray(fLeftOverBytes, 0, 3, fCharsLine, 0);
                WriteChars(fCharsLine, 0, num2);
            }
            fLeftOverBytesCount = count % 3;
            if (fLeftOverBytesCount > 0)
            {
                count -= fLeftOverBytesCount;
                if (fLeftOverBytes == null)
                {
                    fLeftOverBytes = new byte[3];
                }
                for (int i = 0; i < fLeftOverBytesCount; i++)
                {
                    fLeftOverBytes[i] = buffer[(index + count) + i];
                }
            }
            int num4 = index + count;
            int length = LINE_SIZE_IN_BYTES;
            while (index < num4)
            {
                if ((index + length) > num4)
                {
                    length = num4 - index;
                }
                int num6 = Convert.ToBase64CharArray(buffer, index, length, fCharsLine, 0);
                WriteChars(fCharsLine, 0, num6);
                index += length;
            }
        }

        public void Flush()
        {
            if (fLeftOverBytesCount > 0)
            {
                int count = Convert.ToBase64CharArray(fLeftOverBytes, 0, fLeftOverBytesCount, fCharsLine, 0);
                WriteChars(fCharsLine, 0, count);
                fLeftOverBytesCount = 0;
            }
        }
    }
}