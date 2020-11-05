namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\TreeObject\template.cshtml", Author = "YJC",
        PageDataType = typeof(NormalTreeData), CreateDate = "2014-11-25",
        Description = "基于Bootstrap的普通Tree页面，数据采用Object对象")]
    public class NormalObjectTreeTemplate : BaseToolkitTemplate
    {
        public NormalObjectTreeTemplate()
        {
            BaseType = typeof(NormalObjectTreeTemplate);
        }
    }
}
