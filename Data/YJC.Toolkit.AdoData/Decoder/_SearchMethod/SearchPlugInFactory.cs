using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class SearchPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_SearchMethod";
        internal const string DESCRIPTION = "EasySearch对数据进行查询的方法的插件工厂";

        public SearchPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

        public static ISearch CreateSearch(string regName, bool throwIfNoRegName)
        {
            if (!throwIfNoRegName && string.IsNullOrEmpty(regName))
                return null;

            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            try
            {
                TkDebug.ThrowIfNoGlobalVariable();
                return PlugInFactoryManager.CreateInstance<ISearch>(REG_NAME, regName);
            }
            catch (Exception ex)
            {
                if (throwIfNoRegName)
                    throw ex;
                else
                    return null;
            }
        }
    }
}
