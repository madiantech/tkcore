using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootMobile\Edit\template.cshtml", Author = "YJC", CreateDate = "2014-05-26",
        PageDataType = typeof(BootcssEditData), Description = "基于Bootstrap的手机Edit页面")]
    public class BootcssEditTemplate : BaseToolkitTemplate
    {
        public BootcssEditTemplate()
        {
            BaseType = typeof(BootcssEditTemplate);
        }

        protected static IFieldValueProvider CreateProvider(DataRow row, DataSet dataSet)
        {
            return new DataRowFieldValueProvider(row, dataSet);
        }
    }
}
