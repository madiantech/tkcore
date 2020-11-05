using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2016-08-05",
        Description = "创建接口分析数据的缓存")]
    internal class InterfaceStructureCacheCreator : BaseCacheItemCreator
    {
        public override object Create(string key, params object[] args)
        {
            Type intfType = ObjectUtil.QueryObject<Type>(args);
            TkDebug.AssertNotNull(intfType, "参数中需要有Interface的类型", this);
            TkDebug.Assert(intfType.IsInterface, string.Format(ObjectUtil.SysCulture,
                "{0}不是接口类型，请确认", intfType), this);

            InterfaceStructure result = new InterfaceStructure(intfType);
            return result;
        }
    }
}
