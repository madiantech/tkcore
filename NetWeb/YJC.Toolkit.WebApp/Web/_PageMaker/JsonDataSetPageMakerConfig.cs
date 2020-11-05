using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将DataSet或者类DataSet的Xml输出为Json", 
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-16")]
    internal class JsonDataSetPageMakerConfig : IConfigCreator<IPageMaker>, IObjectFormat
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new JsonDataSetPageMaker(this);
        }

        #endregion

        [SimpleAttribute(DefaultValue = ConfigType.SystemConfiged)]
        public ConfigType GZip { get; private set; }

        [SimpleAttribute(DefaultValue = ConfigType.SystemConfiged)]
        public ConfigType Encrypt { get; private set; }
    }
}
