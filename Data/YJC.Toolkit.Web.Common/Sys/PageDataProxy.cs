using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using System.Collections.Specialized;

namespace YJC.Toolkit.Sys
{
    public sealed class PageDataProxy : IPageData
    {
        private readonly IPageData fSource;
        private readonly IPageStyle fStyle;

        public PageDataProxy(IPageData source, IPageStyle style)
        {
            TkDebug.AssertArgumentNull(source, "source", null);
            TkDebug.AssertArgumentNull(style, "style", null);

            fSource = source;
            fStyle = style;
        }

        #region IPageData 成员

        public string Title
        {
            get
            {
                return fSource.Title;
            }
        }

        public Uri PageUrl
        {
            get
            {
                return fSource.PageUrl;
            }
        }

        public string PageExtension
        {
            get
            {
                return fSource.PageExtension;
            }
        }

        public bool IsMobileDevice
        {
            get
            {
                return fSource.IsMobileDevice;
            }
        }

        #endregion IPageData 成员

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
                return fSource.PostObject;
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