using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class UploadData
    {
        public UploadData(string fileName, byte[] fileData)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);
            TkDebug.AssertArgumentNull(fileData, "fileData", null);

            FileName = fileName;
            FileData = fileData;
        }

        public UploadData(string fileName, Stream stream)
            : this(fileName, ReadByteData(stream))
        {
        }

        public UploadData(string fileName)
            : this(fileName, File.ReadAllBytes(fileName))
        {
        }

        public string FileName { get; private set; }
        
        public byte[] FileData { get; private set; }

        private static byte[] ReadByteData(Stream stream)
        {
            TkDebug.AssertArgumentNull(stream, "stream", null);

            MemoryStream output = new MemoryStream();
            using (output)
            {
                FileUtil.CopyStream(stream, output);
                return output.ToArray();
            }
        }
    }
}
