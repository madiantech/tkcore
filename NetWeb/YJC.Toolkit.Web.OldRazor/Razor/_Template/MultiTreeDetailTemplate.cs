namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\Tree\multidetailtemplate.cshtml", Author = "YJC",
        PageDataType = typeof(NormalDetailData), CreateDate = "2015-07-29",
        Description = "基于Bootstrap的普通Tree Detail页面，Detail页面显示多张表的详细信息")]
    public class MultiTreeDetailTemplate : BaseToolkitTemplate
    {
        public MultiTreeDetailTemplate()
        {
            BaseType = typeof(NormalTreeDetailTemplate);
        }
    }
}
