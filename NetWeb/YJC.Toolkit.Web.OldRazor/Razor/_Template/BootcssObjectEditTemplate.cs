using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootMobile\EditObject\template.cshtml", Author = "YJC", CreateDate = "2015-02-11",
        PageDataType = typeof(BootcssEditData), Description = "基于Bootstrap的手机Edit页面，数据采用Object对象")]
    public class BootcssObjectEditTemplate : BaseToolkitObjectTemplate
    {
        public BootcssObjectEditTemplate()
        {
            BaseType = typeof(BootcssObjectEditTemplate);
        }

        public string RenderHidden(ObjectContainer receiver, Tk5FieldInfoEx field, bool needId)
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
