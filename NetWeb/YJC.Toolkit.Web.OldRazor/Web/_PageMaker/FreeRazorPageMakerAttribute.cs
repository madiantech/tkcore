using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FreeRazorPageMakerAttribute : BasePageMakerAttribute
    {
        public FreeRazorPageMakerAttribute(string razorFile)
            : this(razorFile, FilePathPosition.Xml)
        {
        }

        public FreeRazorPageMakerAttribute(string razorFile, FilePathPosition position)
        {
            RazorFile = razorFile;
            Position = position;
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            string fileName = FileUtil.GetRealFileName(RazorFile, Position);
            return new FreeRazorPageMaker(fileName);
        }

        public string RazorFile { get; private set; }

        public FilePathPosition Position { get; private set; }
    }
}
