using System.Data;
using YJC.Toolkit.Data;
namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\MultiEdit\template.cshtml", Author = "YJC",
        PageDataType = typeof(NormalMultiEditData), CreateDate = "2014-09-08",
        Description = "基于Bootstrap的普通Edit页面，Edit页面可以同时编辑多张表的数据")]
    public class NormalMultiEditTemplate : NormalEditTemplate
    {
        public NormalMultiEditTemplate()
        {
            BaseType = typeof(NormalMultiEditTemplate);
        }

        protected virtual string RenderRow(string tableName, DataRow row)
        {
            SingleTableEditData tableData = GetTableData(tableName);
            if (tableData == null)
                return null;
            RazorOutputData output = tableData.RowDisplay;
            if (output == null)
                return null;
            return output.Execute(this, new RowModelData(row));
        }


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
