using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\List\template.cshtml", Author = "YJC", CreateDate = "2014-06-25",
        PageDataType = typeof(NormalListData), Description = "基于Bootstrap的普通List页面")]
    public class NormalListTemplate : BaseToolkitTemplate
    {
        public NormalListTemplate()
        {
            BaseType = typeof(NormalListTemplate);
        }

        protected virtual string RenderRow(DataRow row)
        {
            NormalListData pageData = ViewBag.PageData as NormalListData;
            if (pageData == null)
                return null;

            RazorOutputData output = pageData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new RowModelData(row));
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

        protected virtual string RenderRowOperator(IFieldValueProvider provider)
        {
            NormalListData pageData = ViewBag.PageData as NormalListData;
            if (pageData == null)
                return null;

            RazorOutputData output = pageData.RowOperator;
            if (output == null)
                return null;
            return output.Execute(this, new ProviderModelData(provider));
        }

        protected virtual string RenderQueryItem(DataRow row, IFieldInfoEx field)
        {
            return null;
        }

        protected virtual string RenderQueryItem(IFieldValueProvider provider, IFieldInfoEx field)
        {
            return null;
        }

        protected static IFieldValueProvider CreateProvider(DataRow row, DataSet dataSet)
        {
            return new DataRowFieldValueProvider(row, dataSet);
        }
    }
}