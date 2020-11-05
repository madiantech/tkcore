using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\ListObject\template.cshtml", Author = "YJC",
       PageDataType = typeof(NormalListData), CreateDate = "2014-11-12",
       Description = "基于Bootstrap的普通Edit页面，数据采用Object对象")]
    public class NormalObjectListTemplate : BaseToolkitObjectTemplate
    {
        public NormalObjectListTemplate()
        {
            BaseType = typeof(NormalObjectListTemplate);
        }

        protected virtual string RenderRow(ObjectContainer item)
        {
            NormalListData pageData = ViewBag.PageData as NormalListData;
            if (pageData == null)
                return null;

            RazorOutputData output = pageData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new ObjectModelData(item));
        }

        protected virtual string RenderRow(IFieldValueProvider provider)
        {
            NormalListData pageData = ViewBag.PageData as NormalListData;
            if (pageData == null)
                return null;

            RazorOutputData output = pageData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new ProviderModelData(provider));
        }

        protected static IFieldValueProvider CreateProvider(ObjectContainer container, CodeTableContainer codeTables)
        {
            return new ObjectContainerFieldValueProvider(container, codeTables);
        }
    }
}
