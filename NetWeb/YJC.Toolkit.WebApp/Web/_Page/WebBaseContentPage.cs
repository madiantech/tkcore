using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class WebBaseContentPage : WebBasePage
    {
        protected WebBaseContentPage()
        {
        }

        protected abstract IPageMaker PageMaker { get; }

        public override ICallerInfo CallerInfo
        {
            get
            {
                ICallerInfo info = PageMaker as ICallerInfo;
                if (info != null)
                    return info;
                else
                    return base.CallerInfo;
            }
        }

        private void DoAction()
        {
            var metaData = MetaData;
            ISource source = Source;
            using (source as IDisposable)
            {
                InternalWebUtil.PrepareSource(source, this);
                PrepareRecordLog(source);

                OutputData outputData = InternalWebUtil.CreateOutputData(metaData, source, Style, this);

                Log(source, outputData);
                IPageMaker pageMaker = PageMaker;
                InternalWebUtil.WritePage(metaData, source, pageMaker, this, outputData);
            }
        }

        protected virtual void PrepareRecordLog(ISource source)
        {
        }

        protected sealed override void DoGet()
        {
            if (!WriteCachePage())
                DoAction();
        }

        protected sealed override void DoPost()
        {
            DoAction();
        }

        protected virtual bool WriteCachePage()
        {
            return false;
        }

        public override string ToString()
        {
            IPageMaker pageMaker = PageMaker;
            return pageMaker == null ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "PageMaker为{0}的ContentPage", pageMaker);
        }
    }
}