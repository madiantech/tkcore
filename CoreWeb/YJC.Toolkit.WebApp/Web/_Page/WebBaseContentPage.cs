using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class WebBaseContentPage : WebBasePage
    {
        protected WebBaseContentPage(HttpContext context, RequestDelegate next, PageSourceInfo info)
            : base(context, next, info)
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

        private Task DoAction()
        {
            var metaData = MetaData;
            ISource source = Source;
            TkDebug.AssertNotNull(source, $"当前{Style}下，Source为Null，请检查配置是否正确", this);
            using (source as IDisposable)
            {
                InternalWebUtil.PrepareSource(source, this);
                CheckFunctionRight();
                PrepareRecordLog(source);

                OutputData outputData = InternalWebUtil.CreateOutputData(metaData, source, Style, this);

                Log(source, outputData);
                IPageMaker pageMaker = PageMaker;
                return InternalWebUtil.WritePage(metaData, source, pageMaker, this, outputData);
            }
        }

        protected virtual void PrepareRecordLog(ISource source)
        {
        }

        protected sealed override Task DoGet()
        {
            return DoAction();
        }

        protected sealed override Task DoPost()
        {
            return DoAction();
        }

        public override string ToString()
        {
            IPageMaker pageMaker = PageMaker;
            return pageMaker == null ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "PageMaker为{0}的ContentPage", pageMaker);
        }
    }
}