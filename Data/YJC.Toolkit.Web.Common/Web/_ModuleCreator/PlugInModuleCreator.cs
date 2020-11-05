using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleCreator(Author = "YJC", CreateDate = "2015-09-28",
        Description = "根据Module的插件注册名称来创建Module")]
    [AlwaysCache, InstancePlugIn]
    internal class PlugInModuleCreator : IModuleCreator
    {
        public static readonly IModuleCreator Instance = new PlugInModuleCreator();

        private PlugInModuleCreator()
        {
        }

        #region IModuleCreator 成员

        public IModule Create(string source)
        {
            TkDebug.AssertArgumentNullOrEmpty(source, "source", this);

            IModule module = PlugInFactoryManager.CreateInstance<IModule>(
                ModulePlugInFactory.REG_NAME, source);
            return module;
        }

        #endregion
    }
}
