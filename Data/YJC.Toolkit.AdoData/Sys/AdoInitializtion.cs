using System;
using System.Globalization;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.Properties;

namespace YJC.Toolkit.Sys
{
    internal class AdoInitializtion : IInitialization
    {
        #region IInitialization 成员

        public void AppStarting(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
            TkAdoData.Culture = appsetting.Culture ?? CultureInfo.CurrentCulture;

            BaseXmlPlugInFactory factory = globalVariable.FactoryManager.GetCodeFactory(
                CodeTablePlugInFactory.REG_NAME).Convert<BaseXmlPlugInFactory>();

            if (factory != null)
            {
                factory.FailGetRegItem += AddStandardCodeTable;

                XmlPlugInAttribute codeTableAttr = new XmlPlugInAttribute(CodeTablePlugInFactory.PATH,
                    typeof(CodeTableXml))
                { SearchPattern = "*CodeTable.xml" };
                factory.AddXmlPlugInAttribute(codeTableAttr);

                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    StandardCodeTableConfig.BASE_CLASS, typeof(InternalStandardDbCodeTable)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5CodeTableConfig.BASE_CLASS, typeof(InternalTk5DbCodeTable)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    SqlCodeTableConfig.BASE_CLASS, typeof(InternalSqlCodeTable)));
            }

            factory = globalVariable.FactoryManager.GetCodeFactory(
                EasySearchPlugInFactory.REG_NAME).Convert<BaseXmlPlugInFactory>();

            if (factory != null)
            {
                XmlPlugInAttribute easySearchAttr = new XmlPlugInAttribute(CodeTablePlugInFactory.PATH,
                    typeof(EasySearchXml))
                { SearchPattern = "*EasySearch.xml" };
                factory.AddXmlPlugInAttribute(easySearchAttr);

                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5EasySearchConfig.BASE_CLASS, typeof(InternalTk5DbEasySearch)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5CodeTableEasySearchConfig.BASE_CLASS, typeof(InternalTk5DbCodeTableEasySearch)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5Level0CodeTableEasySearchConfig.BASE_CLASS,
                    typeof(InternalTk5DbLevel0CodeTableEasySearch)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5LevelCodeTableEasySearchConfig.BASE_CLASS,
                    typeof(InternalTk5DbLevelCodeTableEasySearch)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    Tk5TreeEasySearchConfig.BASE_CLASS,
                    typeof(InternalTk5DbTreeEasySearch)));
                factory.AddBaseClassAttribute(new XmlBaseClassAttribute(
                    SqlEasySearchConfig.BASE_CLASS,
                    typeof(InternalTk5DbSqlEasySearch)));
            }
        }

        public void AppStarted(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
        }

        public void AppEnd(object application)
        {
        }

        #endregion IInitialization 成员

        private void AddStandardCodeTable(object sender, RegItemEventArgs e)
        {
            if (e.RegName.StartsWith("CD_", StringComparison.Ordinal))
            {
                StandardDbCodeTable codeTable = new StandardDbCodeTable(e.RegName);
                CodeTablePlugInFactory factory = sender.Convert<CodeTablePlugInFactory>();
                InstanceRegItem regItem = factory.AddInstance(e.RegName, codeTable.Attribute,
                    codeTable, new DayChangeDependency());
                e.RegItem = regItem;
            }
        }
    }
}