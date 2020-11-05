using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        Description = "监控Xml配置工厂数量的缓存依赖", CreateDate = "2013-09-29")]
    class XmlConfigFactoryConfig : IConfigCreator<ICacheDependency>
    {
        [TkTypeConverter(typeof(XmlConfigFactoryTypeConverter))]
        [SimpleAttribute]
        public BaseXmlConfigFactory Factory { get; private set; }

        #region IConfigCreator<ICacheDependency> 成员

        public ICacheDependency CreateObject(params object[] args)
        {
            if (Factory != null)
                return new XmlConfigFactoryDependency(Factory);
            else
                return NoDependency.Dependency;
        }

        #endregion
    }
}
