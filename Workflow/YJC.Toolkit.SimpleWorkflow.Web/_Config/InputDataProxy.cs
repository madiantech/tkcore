using System.Collections.Specialized;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class InputDataProxy : IInputData
    {
        private readonly IInputData fSource;
        private readonly PageStyleClass fStyle;
        private readonly object fPostObject;

        public InputDataProxy(IInputData source, PageStyleClass style)
            : this(source, style, null)
        {
        }

        public InputDataProxy(IInputData source, PageStyleClass style, object postObject)
        {
            TkDebug.AssertArgumentNull(source, "source", null);
            TkDebug.AssertArgumentNull(style, "style", null);

            fStyle = style;
            fSource = source;
            fPostObject = postObject ?? source.PostObject;
        }

        #region IInputData 成员

        public IPageStyle Style
        {
            get
            {
                return fStyle;
            }
        }

        public bool IsPost
        {
            get
            {
                return fSource.IsPost;
            }
        }

        public IQueryString QueryString
        {
            get
            {
                return fSource.QueryString;
            }
        }

        public object PostObject
        {
            get
            {
                return fPostObject;
            }
        }

        public ICallerInfo CallerInfo
        {
            get
            {
                return fSource.CallerInfo;
            }
        }

        public string QueryStringText
        {
            get
            {
                return fSource.QueryStringText;
            }
        }

        public PageSourceInfo SourceInfo
        {
            get
            {
                return fSource.SourceInfo;
            }
        }

        #endregion IInputData 成员
    }
}