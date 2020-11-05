namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Query\conditiontemplate.cshtml", Author = "YJC",
        PageDataType = typeof(NormalQueryData), CreateDate = "2015-10-26", 
        Description = "基于Bootstrap，显示查询条件的页面")]
    public class NormalQueryConditionTemplate : BaseToolkitTemplate
    {
        public NormalQueryConditionTemplate()
        {
            BaseType = typeof(NormalQueryConditionTemplate);
        }
    }
}
