using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleCreator(Author = "YJC", CreateDate = "2015-09-28",
        Description = "根据Source的注册名以及其他附着在Source的Attribute来创建Module")]
    [ModuleCreator(RegName = "true", Author = "YJC", CreateDate = "2015-09-28",
        Description = "根据Source的注册名以及其他附着在Source的Attribute来创建Module")]
    [AlwaysCache, InstancePlugIn]
    internal class SourceModuleCreator : IModuleCreator
    {
        public static readonly IModuleCreator Instance = new SourceModuleCreator();

        private SourceModuleCreator()
        {
        }

        #region IModuleCreator 成员

        public IModule Create(string source)
        {
            TkDebug.AssertArgumentNullOrEmpty(source, "source", this);

            ISource dataSource = PlugInFactoryManager.CreateInstance<ISource>(
                        SourcePlugInFactory.REG_NAME, source);
            return new SourceModuleXml(dataSource);
        }

        #endregion
    }
}
