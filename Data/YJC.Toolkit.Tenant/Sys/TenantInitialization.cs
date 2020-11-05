using YJC.Toolkit.Decoder;

namespace YJC.Toolkit.Sys
{
    internal class TenantInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppEnd(object application)
        {
        }

        public void AppStarted(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
        }

        public void AppStarting(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
            var factory = globalVariable.FactoryManager.GetCodeFactory(
                EasySearchPlugInFactory.REG_NAME).Convert<BaseXmlPlugInFactory>();
            if (factory != null)
            {
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5TenantEasySearchConfig.BASE_CLASS, typeof(InternalTenantDbEasySearch)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5TenantTreeEasySearchConfig.TREE_BASE_CLASS, typeof(InternalTenantDbTreeEasySearch)));
            }

            factory = globalVariable.FactoryManager.GetCodeFactory(
                CodeTablePlugInFactory.REG_NAME).Convert<BaseXmlPlugInFactory>();
            if (factory != null)
            {
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    TenantStandardCodeTableConfig.TENANT_BASE_CLASS, typeof(InternalTenantStandardDbCodeTable)));
            }
        }

        #endregion IInitialization 成员
    }
}