using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "~",
        Description = "生成CheckBox控件的HTML")]
    [InstancePlugIn]
    internal class RealCheckBoxControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new RealCheckBoxControlHtml();

        private RealCheckBoxControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.CheckBox(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}