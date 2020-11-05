using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2017-04-02", SearchControl = "~",
        Description = "生成Password控件的HTML")]
    [InstancePlugIn]
    internal class PasswordControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new PasswordControlHtml();

        private PasswordControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.Input(provider, needId);
        }

        #endregion IControlHtml 成员
    }
}