using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "~",
        Description = "生成区间EasySearch控件的HTML")]
    [InstancePlugIn]
    internal class EasySearchRangeControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new EasySearchRangeControlHtml();

        private EasySearchRangeControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            string startHtml = field.EasySearch(provider, true);
            string endHtml = HtmlCommonExtension.EasySearchEnd(field, provider);
            return HtmlCommonUtil.GetRangeCtrl(startHtml, endHtml);
        }

        #endregion IControlHtml 成员
    }
}