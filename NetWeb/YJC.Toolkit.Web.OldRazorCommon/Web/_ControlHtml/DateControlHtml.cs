using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02",
        RangeControl = "DateRange", Description = "生成Date控件的HTML")]
    [InstancePlugIn]
    internal class DateControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new DateControlHtml();

        private DateControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.Date(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}