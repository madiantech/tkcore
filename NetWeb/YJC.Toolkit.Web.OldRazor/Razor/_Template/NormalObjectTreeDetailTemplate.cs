namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\TreeObject\detailtemplate.cshtml", Author = "YJC", 
        PageDataType = typeof(NormalDetailData),CreateDate = "2014-11-25",
        Description = "基于Bootstrap的普通Tree Detail页面，数据采用Object对象")]
    public class NormalObjectTreeDetailTemplate : BaseToolkitTemplate
    {
        public NormalObjectTreeDetailTemplate()
        {
            BaseType = typeof(NormalObjectTreeDetailTemplate);
        }
    }
}
