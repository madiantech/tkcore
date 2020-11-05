using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class TreeEditPostPageMaker : PostPageMaker
    {
        public TreeEditPostPageMaker(PageStyle sourceStyle)
            : base(ContentDataType.Json, PageStyle.List, null)
        {
            UseRetUrlFirst = sourceStyle == PageStyle.Update;
        }

        protected override string GetDefaultUrl(ISource source, IPageData pageData, OutputData outputData)
        {
            KeyData key = outputData.Data.Convert<KeyData>();
            var pageInfo = pageData.SourceInfo;
            var url = $"~/c/xml/C/{pageInfo.Source}?InitValue={key.SingleValue}";
            return WebUtil.ResolveUrl(url);
        }
    }
}