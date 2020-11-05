using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Query\resulttemplate.cshtml", Author = "YJC",
        PageDataType = typeof(NormalQueryData), CreateDate = "2015-10-26",
        Description = "基于Bootstrap，显示查询结果的页面")]
    public class NormalQueryResultTemplate : BaseToolkitTemplate
    {
        public NormalQueryResultTemplate()
        {
            BaseType = typeof(NormalQueryResultTemplate);
        }

        protected static IFieldValueProvider CreateProvider(DataRow row, DataSet dataSet)
        {
            return new DataRowFieldValueProvider(row, dataSet);
        }
    }
}
