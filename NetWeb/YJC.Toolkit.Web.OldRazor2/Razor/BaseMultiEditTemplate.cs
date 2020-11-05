using YJC.Toolkit.Data;

namespace YJC.Toolkit.Razor
{
    [RazorBaseTemplate(RegName = "MultiEdit", Author = "YJC", CreateDate = "2017-04-20",
        Description = "多表编辑的Razor模板")]
    public class BaseMultiEditTemplate : BaseToolkit2Template
    {
        protected virtual string RenderRow(string tableName, IFieldValueProvider provider)
        {
            SingleTableEditData tableData = GetTableData(tableName);
            if (tableData == null)
                return null;
            RazorOutputData output = tableData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new ProviderModelData(provider));
        }

        protected SingleTableEditData GetTableData(string tableName)
        {
            NormalMultiEditData pageData = ViewBag.PageData as NormalMultiEditData;
            if (pageData == null)
                return null;

            SingleTableEditData tableData = pageData[tableName];
            return tableData;
        }
    }
}