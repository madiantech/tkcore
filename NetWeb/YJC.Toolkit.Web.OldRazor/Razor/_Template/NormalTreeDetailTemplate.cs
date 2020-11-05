namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Tree\detailtemplate.cshtml", Author = "YJC", CreateDate = "2014-08-26",
        PageDataType = typeof(NormalDetailData), Description = "基于Bootstrap的普通Tree Detail页面")]
    public class NormalTreeDetailTemplate : BaseToolkitTemplate
    {
        public NormalTreeDetailTemplate()
        {
            BaseType = typeof(NormalTreeDetailTemplate);
        }
    }
}
