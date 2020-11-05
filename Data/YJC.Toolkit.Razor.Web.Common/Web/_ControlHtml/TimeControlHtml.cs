using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02",
        RangeControl = "TimeRange", Description = "生成Time控件的HTML")]
    [InstancePlugIn]
    internal class TimeControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new TimeControlHtml();

        private TimeControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.Time(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}