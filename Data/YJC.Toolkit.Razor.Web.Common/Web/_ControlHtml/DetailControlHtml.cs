using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ControlHtml(Author = "YJC", CreateDate = "2018-06-27",
        Description = "生成Detail控件的HTML")]
    [InstancePlugIn]
    internal class DetailControlHtml : IControlHtml
    {
        public static readonly IControlHtml Instance = new DetailControlHtml();

        private DetailControlHtml()
        {
        }

        #region IControlHtml 成员

        public string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId)
        {
            TkDebug.AssertArgumentNull(field, "field", this);

            return field.Detail(provider, true, needId);
        }

        #endregion IControlHtml 成员
    }
}