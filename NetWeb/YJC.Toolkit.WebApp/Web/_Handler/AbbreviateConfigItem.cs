using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class AbbreviateConfigItem : IRegName, IReadObjectCallBack
    {
        [SimpleAttribute]
        public string Head { get; private set; }

        [SimpleAttribute]
        public string Path { get; private set; }

        [SimpleAttribute(DefaultValue = "Id")]
        public string IdName { get; private set; }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Head.ToLower(ObjectUtil.SysCulture);
            }
        }

        #endregion

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Path.IndexOf('?') == -1)
                Path += "?";
            else if (!Path.EndsWith("&", StringComparison.Ordinal))
                Path += "&";
        }

        #endregion
    }
}