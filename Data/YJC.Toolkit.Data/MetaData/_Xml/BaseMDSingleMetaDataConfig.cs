using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public abstract class BaseMDSingleMetaDataConfig
    {
        protected BaseMDSingleMetaDataConfig()
        {
        }

        [DynamicElement(SingleMetaDataConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<ISingleMetaData> MetaData { get; protected set; }

        public ISingleMetaData CreateSingleMetaData()
        {
            return MetaData.CreateObject();
        }
    }
}