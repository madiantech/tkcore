using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class WebBaseRedirectPage : WebBasePage
    {
        protected WebBaseRedirectPage()
        {
        }

        protected abstract IRedirector Redirector { get; }

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

                IRedirector redirector = Redirector;
                InternalWebUtil.RedirectPage(metaData, source, this, outputData, redirector);
            }
        }

        protected virtual void PrepareRecordLog(ISource source)
        {
        }

        protected sealed override void DoGet()
        {
            DoAction();
        }

        protected sealed override void DoPost()
        {
            DoAction();
        }

        public override string ToString()
        {
            IRedirector redirector = Redirector;
            return redirector == null ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "Redirector为{0}的RedirectPage", redirector);
        }
    }
}