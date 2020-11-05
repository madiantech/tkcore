using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.AliyunOSS
{
    [Source(Author = "YJC", CreateDate = "2019-01-13", Description = "")]
    [OutputRedirector]
    [WebPage(SupportLogOn = false)]
    internal class AliyunOSSUrlSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            string fileString = input.QueryString[AliyunOSSConst.CINFIG_QUERY_STRING];
            FileConfig config = FileConfig.ReadFromBase64(fileString);

            string result = config.GenerateUri().AbsoluteUri;
            return OutputData.Create(result);
        }

        #endregion ISource 成员
    }
}