namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootMobile\Custom\template.cshtml", Author = "YJC", CreateDate = "2014-05-29",
        Description = "基于Bootstrap的手机Custom页面")]
    public class BootcssCustomTemplate : BaseToolkitTemplate
    {
        public BootcssCustomTemplate()
        {
            BaseType = typeof(BootcssCustomTemplate);
        }
    }
}
