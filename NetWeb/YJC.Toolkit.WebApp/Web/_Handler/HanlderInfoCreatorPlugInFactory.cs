using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class HanlderInfoCreatorPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_HanlderInfoCreator";
        private const string DESCRIPTION = "根据Web页面扩展名获取相关基本信息(兼容.tkx，.c文件的补丁)的插件工厂";

        public HanlderInfoCreatorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
