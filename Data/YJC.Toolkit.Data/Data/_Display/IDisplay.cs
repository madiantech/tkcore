using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IDisplay
    {
        string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue);
    }
}
