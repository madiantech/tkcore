
namespace YJC.Toolkit.Razor
{
    [RazorTemplate(@"BootCss\MultiDetail\template.cshtml", Author = "YJC",
        PageDataType = typeof(NormalDetailData), CreateDate = "2014-09-14",
        Description = "基于Bootstrap的普通Detail页面，Detail页面显示多张表的详细信息")]
    public class NormalMultiDetailTemplate : NormalDetailTemplate
    {
        public NormalMultiDetailTemplate()
        {
            BaseType = typeof(NormalMultiDetailTemplate);
        }
    }
}
