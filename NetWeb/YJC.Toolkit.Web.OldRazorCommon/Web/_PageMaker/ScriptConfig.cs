using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [Serializable]
    public class ScriptConfig : IReadObjectCallBack
    {
        [SimpleAttribute]
        public ScriptType? Type { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseAppPath { get; private set; }

        [TextContent]
        public string Content { get; private set; }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Type == null)
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    if (Content.EndsWith("css", StringComparison.OrdinalIgnoreCase))
                        Type = ScriptType.Css;
                    else
                        Type = ScriptType.JavaScript;
                }
            }
        }

        #endregion IReadObjectCallBack 成员

        public string CreateContent()
        {
            return UseAppPath ? Content.AppVirutalPath() : Content;
        }
    }
}