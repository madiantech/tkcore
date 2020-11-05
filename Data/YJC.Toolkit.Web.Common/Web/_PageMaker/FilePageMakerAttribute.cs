using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FilePageMakerAttribute : BasePageMakerAttribute
    {
        public FilePageMakerAttribute(FilePathPosition position, string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            Position = position;
            FileName = fileName;
            ContentType = ContentTypeConst.HTML;
            Encoding = FileEncoding = "utf-8";
        }

        public string ContentType { get; set; }

        public string FileName { get; private set; }

        public string Encoding { get; set; }

        public string FileEncoding { get; set; }

        public FilePathPosition Position { get; private set; }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new FilePageMaker(this);
        }
    }
}
