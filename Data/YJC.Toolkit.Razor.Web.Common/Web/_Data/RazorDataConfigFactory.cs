using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [FactoryDefaultValue(RazorDataConst.SECTION_NAME, REG_NAME, Author = "YJC", CreateDate = "2019-11-12",
        Description = "在Default.xml设置Section为PageData的DefaultValue区域，可以定义该工厂的缺省值")]
    public sealed class RazorDataConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_RazorData";
        private const string DESCRIPTION = "Razor生成界面的微数据的配置插件工厂";

        public RazorDataConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}