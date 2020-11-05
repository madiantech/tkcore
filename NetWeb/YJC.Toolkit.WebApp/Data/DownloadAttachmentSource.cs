using System.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [Source(Author = "YJC", CreateDate = "2013-12-06",
        Description = "下载存在于SYS_ATTACHMENT中的附件")]
    [WebFilePageMaker]
    [WebPage(SupportLogOn = false)]
    internal class DownloadAttachmentSource : BaseDbSource
    {
        public override OutputData DoAction(IInputData input)
        {
            string context = input.QueryString["Context"];
            if (!string.IsNullOrEmpty(context))
                Context = DbContextUtil.CreateDbContext(context);
            AttachmentResolver resolver = new AttachmentResolver(this);
            using (resolver)
            {
                DataRow row = resolver.Query(input.QueryString);
                bool noFileName = input.QueryString["NoFileName"].Value<bool>();
                FileContent content;

                if (noFileName)
                    content = new FileContent(row["ContentType"].ToString(), (byte[])row["Content"]);
                else
                    content = new FileContent(row["ContentType"].ToString(),
                        row["OriginalName"].ToString(), (byte[])row["Content"]);
                return OutputData.Create(content);
            }
        }
    }
}
