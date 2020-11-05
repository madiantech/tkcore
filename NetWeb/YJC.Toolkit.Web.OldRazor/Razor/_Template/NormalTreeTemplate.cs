namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Tree\template.cshtml", Author = "YJC", CreateDate = "2014-08-25",
        PageDataType = typeof(NormalTreeData), Description = "基于Bootstrap的普通Tree页面")]
    public class NormalTreeTemplate : BaseToolkitTemplate
    {
        public NormalTreeTemplate()
        {
            BaseType = typeof(NormalTreeTemplate);
        }
    }
}
