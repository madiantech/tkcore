using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "RealCheckBox",
        Description = "生成CheckBox(Switcher方式)控件的HTML")]
    [InstancePlugIn]
    internal class CheckBoxControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new CheckBoxControlHtml();

        private CheckBoxControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.Switcher(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}