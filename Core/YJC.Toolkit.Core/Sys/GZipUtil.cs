using System.IO;
using System.IO.Compression;

namespace YJC.Toolkit.Sys
{
    public static class GZipUtil
    {
        private const int BUFFER_SIZE = 10 * 1024;

        public static void ReadFromStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            int length;
            while ((length = input.Read(buffer, 0, BUFFER_SIZE)) > 0)
            {
                output.Write(buffer, 0, length);
                output.Flush();
            }
        }
        public static MemoryStream GZipDecompress(Stream stream)
        {
            TkDebug.AssertArgumentNull(stream, "stream", null);

            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                MemoryStream result = new MemoryStream();
                ReadFromStream(gzip, result);
                if (result.CanSeek)
                    result.Seek(0, SeekOrigin.Begin);
                return result;
            }
        }

        public static MemoryStream GZipCompress(byte[] data)
        {
            TkDebug.AssertArgumentNull(data, "data", null);

            MemoryStream result = new MemoryStream();
            using (GZipStream gzip = new GZipStream(result, CompressionMode.Compress, true))
            {
                gzip.Write(data, 0, data.Length);
            }
            return result;
        }

        public static MemoryStream GZipCompress(Stream stream)
        {
            TkDebug.AssertArgumentNull(stream, "stream", null);

            if (stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);
            MemoryStream result = new MemoryStream();
            using (stream)
            using (GZipStream gzip = new GZipStream(result, CompressionMode.Compress, true))
            {
                ReadFromStream(stream, gzip);
                return result;
            }
        }
    }
}
