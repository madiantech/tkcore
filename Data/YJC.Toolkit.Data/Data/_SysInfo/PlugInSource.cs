using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class PlugInSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            TkDebug.ThrowIfNoGlobalVariable();

            if (MetaDataUtil.Equals(input.Style, (PageStyleClass)"Code"))
            {
                string factoryName = input.QueryString["Name"];
                var factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(factoryName);
                return OutputData.CreateToolkitObject(new PlugInFactoryData(factory));
            }
            else if (MetaDataUtil.Equals(input.Style, (PageStyleClass)"Xml"))
            {
                string factoryName = input.QueryString["Name"];
                var factory = BaseGlobalVariable.Current.FactoryManager.GetConfigFactory(factoryName);
                return OutputData.CreateToolkitObject(new ConfigFactoryData(factory));
            }
            else
                return OutputData.CreateToolkitObject(new PlugInManagerData());
        }

        #endregion
    }
}
