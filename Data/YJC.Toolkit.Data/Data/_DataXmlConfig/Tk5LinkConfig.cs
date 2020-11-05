using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public sealed class Tk5LinkConfig
    {
        internal Tk5LinkConfig()
        {
        }

        public Tk5LinkConfig(string content)
        {
            Content = content;
            RefType = LinkRefType.Href;
        }

        [SimpleAttribute]
        public string Base { get; private set; }

        [SimpleAttribute]
        public string Target { get; private set; }

        [TextContent]
        public string Content { get; private set; }

        [SimpleAttribute]
        public LinkRefType RefType { get; internal set; }

        internal void ProcessType()
        {
            if (Content != null)
            {
                LinkRefType refType;
                if (string.Compare(Content, "email", StringComparison.OrdinalIgnoreCase) == 0)
                    refType = LinkRefType.Email;
                else if (string.Compare(Content, "http", StringComparison.OrdinalIgnoreCase) == 0)
                    refType = LinkRefType.Http;
                else
                    refType = LinkRefType.Href;
                RefType = refType;
            }
        }
    }
}