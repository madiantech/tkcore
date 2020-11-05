using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class EasySearchPlugInFactory : BaseXmlPlugInFactory
    {
        public const string REG_NAME = "_tk_EasySearch";
        private const string DESCRIPTION = "EasySearch的插件工厂";

        public EasySearchPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
