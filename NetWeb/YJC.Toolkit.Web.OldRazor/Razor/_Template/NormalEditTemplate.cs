using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Edit\template.cshtml", Author = "YJC", CreateDate = "2014-07-02",
        PageDataType = typeof(NormalEditData), Description = "基于Bootstrap的普通Edit页面")]
    public class NormalEditTemplate : BaseToolkitTemplate
    {
        public NormalEditTemplate()
        {
            BaseType = typeof(NormalEditTemplate);
        }

        public string RenderHidden(DataRow row, Tk5FieldInfoEx field, bool needId = true)
        {
            string html = RenderFieldItem(row, field);
            if (string.IsNullOrEmpty(html))
            {
                IFieldValueProvider provider = new DataRowFieldValueProvider(row, row.Table.DataSet);
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

        protected static IFieldValueProvider CreateProvider(DataRow row, DataSet dataSet)
        {
            return new DataRowFieldValueProvider(row, dataSet);
        }
    }
}
