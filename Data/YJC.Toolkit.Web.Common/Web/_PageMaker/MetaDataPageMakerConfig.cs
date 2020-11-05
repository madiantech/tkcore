using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "如果配置了MetaData，将以Xml或Json的方式显示MetaData的内容",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-12-04")]
    class MetaDataPageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new MetaDataPageMaker { DataType = DataType };
        }

        #endregion

        [SimpleAttribute(DefaultValue = ContentDataType.Xml)]
        public ContentDataType DataType { get; private set; }
    }
}
