namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\DetailList\template.cshtml", Author = "YJC",
        PageDataType = typeof(NormalListData), CreateDate = "2014-09-16", 
        Description = "基于Bootstrap的普通DetailList页面，针对Detail页面下，从表使用List展示多条记录")]
    public class NormalDetailListTemplate : NormalListTemplate
    {
        public NormalDetailListTemplate()
        {
            BaseType = typeof(NormalDetailListTemplate);
        }
    }
}
