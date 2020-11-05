using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public sealed class RazorField : RazorOutputData, IRegName
    {
        internal RazorField()
        {
        }

        public RazorField(string nickName, RazorContentType contentType, string content)
            : base(contentType, content)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);

            NickName = nickName;
        }

        [SimpleAttribute]
        internal string NickName { get; private set; }

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion IRegName 成员
    }
}