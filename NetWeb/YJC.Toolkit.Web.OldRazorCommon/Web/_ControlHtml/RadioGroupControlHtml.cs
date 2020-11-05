using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "Combo",
        RangeControl = "ComboRange", Description = "生成RadioGroup控件的HTML")]
    [InstancePlugIn]
    internal class RadioGroupControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new RadioGroupControlHtml();

        private RadioGroupControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.RadioGroup(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}