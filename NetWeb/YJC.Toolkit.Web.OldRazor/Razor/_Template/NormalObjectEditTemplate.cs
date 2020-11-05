using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\EditObject\template.cshtml", Author = "YJC",
        PageDataType = typeof(NormalEditData), CreateDate = "2014-07-29", 
        Description = "基于Bootstrap的普通Edit页面，数据采用Object对象")]
    public class NormalObjectEditTemplate : BaseToolkitObjectTemplate
    {
        public NormalObjectEditTemplate()
        {
            BaseType = typeof(NormalObjectEditTemplate);
        }

        public string RenderHidden(ObjectContainer receiver, Tk5FieldInfoEx field, bool needId = true)
        {
            string html = RenderFieldItem(receiver, field);
            if (string.IsNullOrEmpty(html))
            {
                IFieldValueProvider provider = new ObjectContainerFieldValueProvider(receiver, null);
                return field.Hidden(provider, needId);
            }
            return html;
        }

        public string RenderHidden(IFieldValueProvider provider, Tk5FieldInfoEx field, bool needId = true)
        {
            string html = RenderFieldItem(provider, field);
            if (string.IsNullOrEmpty(html))
            {
                return field.Hidden(provider, needId);
            }
            return html;
        }

        protected static IFieldValueProvider CreateProvider(ObjectContainer container, CodeTableContainer codeTables)
        {
            return new ObjectContainerFieldValueProvider(container, codeTables);
        }
    }
}
