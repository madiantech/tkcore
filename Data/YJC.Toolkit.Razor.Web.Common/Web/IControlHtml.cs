using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Web
{
    public interface IControlHtml
    {
        string GetHtml(Tk5FieldInfoEx field, IFieldValueProvider provider, bool needId);
    }
}