using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    class InternalJsonObjectPageMaker : IPageMaker
    {
        private readonly WriteSettings fSettings;
        private readonly string fModelName;

        public InternalJsonObjectPageMaker(string modelName, WriteSettings settings)
        {
            fModelName = modelName;
            fSettings = settings ?? ObjectUtil.WriteSettings;
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData, SourceOutputType.ToolkitObject);
            string json = outputData.Data.WriteJson(fModelName, fSettings);
            return new SimpleContent(ContentTypeConst.JSON, json);
        }

        #endregion
    }
}
