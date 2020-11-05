using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SimpleXsltPageMakerAttribute : BasePageMakerAttribute
    {
        /// <summary>
        /// Initializes a new instance of the XsltPageMakerAttribute class.
        /// </summary>
        public SimpleXsltPageMakerAttribute(string xsltFile, FilePathPosition position)
        {
            XsltFile = xsltFile;
            Position = position;
        }

        public string XsltFile { get; private set; }

        public FilePathPosition Position { get; private set; }

        public bool UseXsltArgs { get; set; }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            string fileName = FileUtil.GetRealFileName(XsltFile, Position);
            return new SimpleXsltPageMaker(fileName, UseXsltArgs);
        }
    }
}
