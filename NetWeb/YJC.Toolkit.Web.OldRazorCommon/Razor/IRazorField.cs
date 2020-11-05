using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Razor
{
    public interface IRazorField
    {
        RazorField GetDisplayField(IFieldInfo field);

        RazorField GetDisplayField(string tableName, IFieldInfo field);
    }
}
