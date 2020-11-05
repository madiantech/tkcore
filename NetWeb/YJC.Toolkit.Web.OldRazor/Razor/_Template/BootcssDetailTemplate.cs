using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootMobile\Detail\template.cshtml", Author = "YJC", CreateDate = "2014-05-26",
        PageDataType = typeof(BootcssDetailData), Description = "基于Bootstrap的手机Detail页面")]
    public class BootcssDetailTemplate : BaseToolkitTemplate
    {
        public BootcssDetailTemplate()
        {
            BaseType = typeof(BootcssDetailTemplate);
        }

        protected static IFieldValueProvider CreateProvider(DataRow row, DataSet dataSet)
        {
            return new DataRowFieldValueProvider(row, dataSet);
        }
    }
}
