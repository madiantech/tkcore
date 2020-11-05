using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [WebPagePlugIn(Author = "YJC", CreateDate = "2014-06-26",
       Description = "kindeditor控件上传文件的处理页面")]
    internal class HtmlUploadPage : WebBaseContentPage
    {
        private readonly ISource fSource;

        public HtmlUploadPage()
        {
            fSource = new HtmlUploadSource();
            SupportLogOn = false;
        }

        protected override IPageMaker PageMaker
        {
            get
            {
                return new JsonObjectPageMaker();
            }
        }

        protected override IPostObjectCreator PostObjectCreator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override ISource Source
        {
            get
            {
                return fSource;
            }
        }

        protected override void PreparePostData()
        {
        }
    }
}
