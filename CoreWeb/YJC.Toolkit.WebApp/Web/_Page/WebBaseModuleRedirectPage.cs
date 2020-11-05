using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Data;
using YJC.Toolkit.Log;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class WebBaseModuleRedirectPage : WebBaseRedirectPage
    {
        private ISource fSource;
        private IMetaData fMetaData;
        private IPostObjectCreator fPostCreator;
        private IRedirector fRedirector;

        protected WebBaseModuleRedirectPage(HttpContext context, RequestDelegate next, PageSourceInfo info)
            : base(context, next, info)
        {
        }

        protected abstract IModule Module { get; }

        public override ISource Source
        {
            get
            {
                if (fSource == null)
                    fSource = Module.CreateSource(this);
                return fSource;
            }
        }

        protected override IMetaData MetaData
        {
            get
            {
                if (fMetaData == null)
                    fMetaData = Module.CreateMetaData(this);
                return fMetaData;
            }
        }

        protected override IPostObjectCreator PostObjectCreator
        {
            get
            {
                if (fPostCreator == null)
                    fPostCreator = Module.CreatePostCreator(this);
                return fPostCreator;
            }
        }

        protected override IRedirector Redirector
        {
            get
            {
                if (fRedirector == null)
                    fRedirector = Module.CreateRedirector(this);
                return fRedirector;
            }
        }

        protected override void PrepareRecordLog(ISource source)
        {
            LogUtil.PrepareRecordLog(Module, this, source);
        }

        protected override void Log(ISource source, OutputData output)
        {
            LogUtil.Log(Module, this, source, output);
        }
    }
}