using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "Combo",
        Description = "生成CheckBoxList控件的HTML")]
    [InstancePlugIn]
    internal class CheckBoxListControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new CheckBoxListControlHtml();

        private CheckBoxListControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.CheckBoxList(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}