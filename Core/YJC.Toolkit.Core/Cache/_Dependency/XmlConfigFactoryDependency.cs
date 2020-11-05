using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal sealed class XmlConfigFactoryDependency : ICacheDependency
    {
        private readonly BaseXmlConfigFactory fFactory;
        private readonly int fCount;

        /// <summary>
        /// Initializes a new instance of the XmlConfigFactoryDependency class.
        /// </summary>
        /// <param name="factory"></param>
        public XmlConfigFactoryDependency(BaseXmlConfigFactory factory)
        {
            TkDebug.AssertArgumentNull(factory, "factory", null);

            fFactory = factory;
            fCount = fFactory.Count;
        }

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                return fFactory.Count != fCount;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "对{0}的Xml配置工厂的依赖", fFactory.Description);
        }
    }
}
