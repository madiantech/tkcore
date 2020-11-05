using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Detail\template.cshtml", Author = "YJC", CreateDate = "2014-07-03",
        PageDataType = typeof(NormalDetailData), Description = "基于Bootstrap的普通Detail页面")]
    public class NormalDetailTemplate : BaseToolkitTemplate
    {
        public NormalDetailTemplate()
        {
            BaseType = typeof(NormalDetailTemplate);
        }

        protected static IFieldValueProvider CreateProvider(DataRow row, DataSet dataSet)
        {
            return new DataRowFieldValueProvider(row, dataSet);
        }
    }
}
