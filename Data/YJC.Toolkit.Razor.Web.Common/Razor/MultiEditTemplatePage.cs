using Microsoft.AspNetCore.Html;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public abstract class MultiEditTemplatePage<TModel> : ToolkitTemplatePage<TModel>
    {
        protected MultiEditTemplatePage()
        {
        }

        protected virtual HtmlString RenderRow(string tableName, IFieldValueProvider provider)
        {
            SingleTableEditData tableData = GetTableData(tableName);
            if (tableData == null)
                return HtmlString.Empty;
            RazorOutputData output = tableData.RowDisplay;
            if (output == null)
                return HtmlString.Empty;
            return output.Execute(this, new ProviderModelData(provider));
        }

        protected SingleTableEditData GetTableData(string tableName)
        {
            if (ViewBag.PageData is NormalMultiEditData pageData)
            {
                SingleTableEditData tableData = pageData[tableName];
                return tableData;
            }

            return null;
        }

        protected SingleTableDetailData GetDetailTableData(string tableName)
        {
            if (ViewBag.PageData is ITableDataIndexer indexer)
            {
                SingleTableDetailData tableData = indexer[tableName];
                return tableData;
            }

            return null;
        }

        protected virtual HtmlString RenderTable(string tableName, object model)
        {
            SingleTableDetailData tableData = GetDetailTableData(tableName);
            RazorOutputData output = tableData?.TableDisplay;

            return RenderRazorOutputData(output, model);
        }
    }
}