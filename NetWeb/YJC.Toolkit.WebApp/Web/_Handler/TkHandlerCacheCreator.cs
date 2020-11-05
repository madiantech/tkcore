using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2013-10-11",
        Description = "Toolkit Handler的缓存对象创建器")]
    [InstancePlugIn, AlwaysCache]
    internal sealed class TkHandlerCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new TkHandlerCacheCreator();

        private TkHandlerCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            TkxXml xml = new TkxXml();
            xml.ReadXmlFromFile(key);
            string typeName = xml.PageHandler;
            ToolkitHttpHandler handler = PlugInFactoryManager.CreateInstance<ToolkitHttpHandler>(
                RegClassPlugInFactory.REG_NAME, typeName);
            //Type type = Type.GetType(typeName);
            //if (type == null)
            //{
            //    TkDebug.ThrowIfNoGlobalVariable();
            //    type = GlobalVariable.Current.BinAssembly.FindType(typeName);
            //    TkDebug.AssertNotNull(type, string.Format(ObjectUtil.SysCulture,
            //        "在Bin目录下，没有找到包含类型{0}的Assembly，请确认", typeName), this);
            //}
            //ToolkitHttpHandler handler = ObjectUtil.CreateObject(type).Convert<ToolkitHttpHandler>();
            handler.SetTkFileName(key);
            return handler;
        }

        public override string TransformCacheKey(string key)
        {
            return key.ToLower(ObjectUtil.SysCulture);
        }
    }
}
