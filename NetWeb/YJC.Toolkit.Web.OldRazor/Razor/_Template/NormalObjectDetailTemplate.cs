using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\DetailObject\template.cshtml", Author = "YJC",
        PageDataType = typeof(NormalDetailData), CreateDate = "2014-11-28",
        Description = "基于Bootstrap的普通Edit页面，数据采用Object对象")]
    public class NormalObjectDetailTemplate : BaseToolkitObjectTemplate
    {
        public NormalObjectDetailTemplate()
        {
            BaseType = typeof(NormalObjectDetailTemplate);
        }

        protected static IFieldValueProvider CreateProvider(ObjectContainer container, CodeTableContainer codeTables)
        {
            return new ObjectContainerFieldValueProvider(container, codeTables);
        }
    }
}
