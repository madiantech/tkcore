namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Custom\template.cshtml", Author = "YJC", CreateDate = "2014-07-08",
        Description = "基于Bootstrap的普通Custom页面")]
    public class NormalCustomTemplate : BaseToolkitTemplate
    {
        public NormalCustomTemplate()
        {
            BaseType = typeof(NormalCustomTemplate);
        }
    }
}
