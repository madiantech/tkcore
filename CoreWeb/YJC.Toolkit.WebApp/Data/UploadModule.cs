using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [Module(Author = "YJC", CreateDate = "2019-11-18", Description = "上传附件")]
    internal class UploadModule : BaseCustomModule
    {
        public UploadModule() : base("上传附件")
        {
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new JsonObjectPageMaker();
        }

        public override ISource CreateSource(IPageData pageData)
        {
            return new UploadSource();
        }

        public override IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            return null;
        }

        public override bool IsSupportLogOn(IPageData pageData)
        {
            return false;
        }
    }
}