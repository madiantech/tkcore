using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Data;
using YJC.Toolkit.Log;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class WebBaseModuleContentPage : WebBaseContentPage
    {
        private ISource fSource;
        private IMetaData fMetaData;
        private IPageMaker fPageMaker;
        private IPostObjectCreator fPostCreator;

        protected WebBaseModuleContentPage(HttpContext context, RequestDelegate next, PageSourceInfo info)
            : base(context, next, info)
        {
        }

        protected abstract IModule Module { get; }

        public override string Title
        {
            get
            {
                return Module.Title;
            }
        }

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

        protected override IPageMaker PageMaker
        {
            get
            {
                if (fPageMaker == null)
                    fPageMaker = Module.CreatePageMaker(this);
                return fPageMaker;
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