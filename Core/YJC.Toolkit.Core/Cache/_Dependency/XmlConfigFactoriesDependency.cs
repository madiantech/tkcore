using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal sealed class XmlConfigFactoriesDependency : ICacheDependency
    {
        private readonly ICollection<BaseXmlConfigFactory> fConfigFactories;
        private readonly int[] fCounts;

        /// <summary>
        /// Initializes a new instance of the XmlConfigFactoriesDependency class.
        /// </summary>
        /// <param name="configFactories"></param>
        public XmlConfigFactoriesDependency(ICollection<BaseXmlConfigFactory> configFactories)
        {
            TkDebug.AssertArgumentNull(configFactories, "configFactories", null);
            TkDebug.AssertArgument(configFactories.Count > 1, "configFactories",
                "List列表中的数量应至少多于1个，否则可以考虑使用XmlConfigFactoryDependency", null);

            fConfigFactories = configFactories;
            fCounts = (from factory in configFactories
                       select factory.Count).ToArray();
        }

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                int index = 0;
                foreach (var factory in fConfigFactories)
                {
                    if (fCounts[index++] != factory.Count)
                        return true;
                }

                return false;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "对{0}等{1}个Xml配置工厂的依赖",
                fConfigFactories.First().Description, fConfigFactories.Count);
        }
    }
}
