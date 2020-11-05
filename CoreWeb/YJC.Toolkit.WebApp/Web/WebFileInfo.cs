using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class WebFileInfo : IFileInfo
    {
        private readonly FileContent fContent;

        public WebFileInfo(FileContent content)
        {
            fContent = content;
            LastModified = DateTimeOffset.Now;
            if (content.IsWriteFileName)
                Name = content.FileName;
            else
                Name = string.Empty;
        }

        public bool Exists => true;

        public long Length => fContent.GetData().Length;

        public string PhysicalPath => null;

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public bool IsDirectory => false;

        public Stream CreateReadStream()
        {
            return new MemoryStream(fContent.GetData());
        }
    }
}