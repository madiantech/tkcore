using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ErrorInfo
    {
        internal ErrorInfo()
        {
        }

        internal ErrorInfo(string source, string errorPage, string url)
        {
            Source = source;
            Page = errorPage;
            Url = url;
            DateTime = DateTime.Now;
            if (BaseGlobalVariable.Current != null)
            {
                try
                {
                    IUserInfo info = BaseGlobalVariable.Current.UserInfo;
                    if (info != null)
                    {
                        if (info.UserId != null)
                            UserId = info.UserId.ToString();
                        if (info.MainOrgId != null)
                            OrgId = info.MainOrgId.ToString();
                    }
                }
                catch
                {
                }
            }
        }

        [SimpleElement]
        public string Source { get; private set; }

        [SimpleElement]
        public string Page { get; private set; }

        [SimpleElement]
        public string Url { get; private set; }

        [SimpleElement]
        public DateTime DateTime { get; private set; }

        [SimpleElement]
        public string UserId { get; private set; }

        [SimpleElement]
        public string OrgId { get; private set; }
    }
}
