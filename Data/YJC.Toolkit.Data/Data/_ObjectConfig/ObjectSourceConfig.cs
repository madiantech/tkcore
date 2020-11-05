using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class ObjectSourceConfig
    {
        [SimpleElement(NamespaceType.Toolkit)]
        public string RegNameObjectSource { get; private set; }

        public object CreateObjectSource()
        {
            object source = PlugInFactoryManager.CreateInstance<object>(
                ObjectSourcePlugInFactory.REG_NAME, RegNameObjectSource);

            return source;
        }
    }
}
