using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class WebFilePageMaker : IPageMaker
    {
        public static readonly IPageMaker PageMaker = new WebFilePageMaker();

        #region IPageMaker 成员

        IContent IPageMaker.WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            PageMakerUtil.AssertType(this, outputData, SourceOutputType.FileContent);
            FileContent content = outputData.Data.Convert<FileContent>();
            bool useCache = pageData.QueryString["Cache"].Value<bool>();
            ContentDisposition disposition;
            if (Disposition.HasValue)
                disposition = Disposition.Value;
            else
                disposition = pageData.QueryString["Disposition"].Value<ContentDisposition>(
                    ContentDisposition.Attachment);
            return new WebFileContent(content, useCache) { Disposition = disposition };
        }

        #endregion

        public ContentDisposition? Disposition { get; set; }
    }
}
