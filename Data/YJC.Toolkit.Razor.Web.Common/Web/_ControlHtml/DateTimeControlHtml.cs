using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02",
        RangeControl = "DateTimeRange", Description = "生成DateTime控件的HTML")]
    [InstancePlugIn]
    internal class DateTimeControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new DateTimeControlHtml();

        private DateTimeControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.DateTime(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}