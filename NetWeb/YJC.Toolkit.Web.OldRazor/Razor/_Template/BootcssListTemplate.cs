using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootMobile\List\template.cshtml", Author = "YJC", CreateDate = "2014-05-21",
        PageDataType = typeof(BootcssListData), Description = "基于Bootstrap的手机List页面")]
    public class BootcssListTemplate : BaseToolkitTemplate
    {
        public BootcssListTemplate()
        {
            BaseType = typeof(BootcssListTemplate);
        }

        protected virtual string RenderRow(DataRow row)
        {
            BootcssListData pageData = ViewBag.PageData as BootcssListData;
            if (pageData == null)
                return null;

            RazorOutputData output = pageData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new RowModelData(row));
        }

        protected virtual string RenderRow(IFieldValueProvider provider)
        {
            BootcssListData pageData = ViewBag.PageData as BootcssListData;
            if (pageData == null)
                return null;

            RazorOutputData output = pageData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new ProviderModelData(provider));
        }

        protected virtual string RenderQueryItem(DataRow row, IFieldInfoEx field)
        {
            return null;
        }

        protected static IFieldValueProvider CreateProvider(DataRow row, DataSet dataSet)
        {
            return new DataRowFieldValueProvider(row, dataSet);
        }
    }
}
