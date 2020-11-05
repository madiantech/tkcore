using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "~",
        Description = "生成区间Time控件的HTML")]
    [InstancePlugIn]
    internal class TimeRangeControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new TimeRangeControlHtml();

        private TimeRangeControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            string startHtml = field.Time(provider, true);
            string endHtml = HtmlCommonExtension.Time(field, provider, true);
            return HtmlCommonUtil.GetRangeCtrl(startHtml, endHtml);
        }

        #endregion IControlHtml 成员
    }
}