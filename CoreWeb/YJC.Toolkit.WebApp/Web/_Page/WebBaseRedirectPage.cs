using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class WebBaseRedirectPage : WebBasePage
    {
        protected WebBaseRedirectPage(HttpContext context, RequestDelegate next, PageSourceInfo info)
            : base(context, next, info)
        {
        }

        protected abstract IRedirector Redirector { get; }

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

                IRedirector redirector = Redirector;
                InternalWebUtil.RedirectPage(metaData, source, this, outputData, redirector);

                return Next(Context);
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
            IRedirector redirector = Redirector;
            return redirector == null ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "Redirector为{0}的RedirectPage", redirector);
        }
    }
}