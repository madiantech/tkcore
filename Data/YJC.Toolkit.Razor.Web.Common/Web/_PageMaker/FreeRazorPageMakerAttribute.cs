using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FreeRazorPageMakerAttribute : BasePageMakerAttribute
    {
        public FreeRazorPageMakerAttribute(string fileName)
        {
            FileName = fileName;
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new FreeRazorPageMaker(FileName)
            {
                Layout = Layout,
                EngineName = EngineName,
                UseTemplate = UseTemplate
            };
        }

        public string FileName { get; private set; }

        public bool UseTemplate { get; set; }

        public string Layout { get; set; }

        public string EngineName { get; set; }
    }
}