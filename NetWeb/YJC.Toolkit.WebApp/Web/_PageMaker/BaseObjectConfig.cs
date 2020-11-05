using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal abstract class BaseObjectConfig : IConfigCreator<IPageMaker>, IObjectFormat
    {
        protected BaseObjectConfig()
        {
        }

        #region IConfigCreator<IPageMaker> 成员

        public abstract IPageMaker CreateObject(params object[] args);

        #endregion

        [SimpleAttribute(DefaultValue = ConfigType.SystemConfiged)]
        public ConfigType GZip { get; protected set; }

        [SimpleAttribute(DefaultValue = ConfigType.SystemConfiged)]
        public ConfigType Encrypt { get; protected set; }

        [SimpleAttribute]
        public string ModelName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public WriteSettings WriteSettings { get; protected set; }
    }
}
