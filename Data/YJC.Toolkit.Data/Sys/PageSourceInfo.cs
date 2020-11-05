using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Sys
{
    public sealed class PageSourceInfo
    {
        //public PageSourceInfo(string source)
        //    : this(null, null, source, true)
        //{
        //}

        public PageSourceInfo(string parser, string moduleCreator, IPageStyle style, string source, bool isContent)
        {
            TkDebug.AssertArgumentNullOrEmpty(parser, nameof(parser), null);
            TkDebug.AssertArgumentNullOrEmpty(source, nameof(source), null);

            Parser = parser;
            if (string.IsNullOrEmpty(moduleCreator))
                ModuleCreator = "xml";
            else
                ModuleCreator = moduleCreator.ToLower(ObjectUtil.SysCulture);
            if (style == null)
                Style = (PageStyleClass)string.Empty;
            else
                Style = style;
            Source = source;
            IsContent = isContent;
        }

        public string Parser { get; private set; }

        public string ModuleCreator { get; private set; }

        public IPageStyle Style { get; internal set; }

        public string Source { get; private set; }

        //public string CcSource
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Source))
        //            return string.Empty;
        //        return Source.Replace('/', '_');
        //    }
        //}

        public bool IsContent { get; private set; }
    }
}