using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class CompositePageMaker : IPageMaker, ISupportMetaData, ICallerInfo
    {
        private class PageMakerInfo : IOrder
        {
            /// <summary>
            /// Initializes a new instance of the PageMakerInfo class.
            /// </summary>
            public PageMakerInfo(Func<ISource, IInputData, OutputData, bool> function, IPageMaker pageMaker, int order)
            {
                Function = function;
                PageMaker = pageMaker;
                Order = order;
            }

            public Func<ISource, IInputData, OutputData, bool> Function { get; private set; }

            public IPageMaker PageMaker { get; private set; }

            public int Order { get; private set; }
        }

        private readonly List<PageMakerInfo> fList = new List<PageMakerInfo>();
        private ICallerInfo fCurrent;
        private readonly ICallerInfo fParent;

        public CompositePageMaker()
        {
        }

        public CompositePageMaker(IPageData pageData)
        {
            if (pageData != null)
            {
                string retUrl = ObjectUtil.GetDefaultValue(pageData.QueryString["RetUrl"], string.Empty);
                Uri ret = string.IsNullOrEmpty(retUrl) ? null : new Uri(retUrl, UriKind.RelativeOrAbsolute);
                fParent = new InternalCallerInfo(pageData, WebGlobalVariable.SessionGbl, ret, pageData.PageUrl);
            }
        }

        #region IPageMaker 成员

        IContent IPageMaker.WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            return WritePage(source, pageData, outputData);
        }

        #endregion IPageMaker 成员

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            foreach (var item in fList)
            {
                ISupportMetaData support = item.PageMaker as ISupportMetaData;
                if (support != null)
                {
                    if (support.CanUseMetaData(style))
                        support.SetMetaData(style, metaData);
                }
            }
        }

        #endregion ISupportMetaData 成员

        #region ICallerInfo 成员

        public void AddInfo(DataSet dataSet)
        {
            var callInfo = CallerInfo;
            if (callInfo != null)
                callInfo.AddInfo(dataSet);
        }

        public void AddInfo(StringBuilder builder)
        {
            var callInfo = CallerInfo;
            if (callInfo != null)
                callInfo.AddInfo(builder);
        }

        public void AddInfo(XElement element)
        {
            var callInfo = CallerInfo;
            if (callInfo != null)
                callInfo.AddInfo(element);
        }

        public void AddInfo(dynamic data)
        {
            var callInfo = CallerInfo;
            if (callInfo != null)
                callInfo.AddInfo(data);
        }

        #endregion ICallerInfo 成员

        private ICallerInfo CallerInfo
        {
            get
            {
                if (fCurrent != null)
                    return fCurrent;
                return fParent;
            }
        }

        protected void SetCallInfo(IPageData pageData)
        {
            TkDebug.AssertArgumentNull(pageData, "pageData", this);

            fList.Sort(OrderComparer.Comparer);
            foreach (PageMakerInfo item in fList)
            {
                try
                {
                    if (item.Function(null, pageData, null))
                    {
                        fCurrent = item.PageMaker as ICallerInfo;
                        break;
                    }
                }
                catch
                {
                }
            }
        }

        protected virtual void PrepareWritePage(IPageMaker pageMaker, ISource source,
            IPageData pageData, OutputData outputData)
        {
        }

        protected virtual IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            fList.Sort(OrderComparer.Comparer);
            foreach (PageMakerInfo item in fList)
            {
                if (item.Function(source, pageData, outputData))
                {
                    PrepareWritePage(item.PageMaker, source, pageData, outputData);
                    return item.PageMaker.WritePage(source, pageData, outputData);
                }
            }
            TkDebug.ThrowToolkitException("当前的状态下，没有一个注册的PageMaker满足其条件，请检查条件", this);
            return null;
        }

        public void Add(int order, Func<ISource, IInputData, OutputData, bool> function, IPageMaker pageMaker)
        {
            TkDebug.AssertArgumentNull(function, "function", this);
            TkDebug.AssertArgumentNull(pageMaker, "pageMaker", this);

            PageMakerInfo info = new PageMakerInfo(function, pageMaker, order);
            fList.Add(info);
        }

        public void Add(Func<ISource, IInputData, OutputData, bool> function, IPageMaker pageMaker)
        {
            int order = (fList.Count + 1) * 10;
            Add(order, function, pageMaker);
        }

        internal void InternalSetCallInfo(IPageData pageData)
        {
            SetCallInfo(pageData);
        }
    }
}