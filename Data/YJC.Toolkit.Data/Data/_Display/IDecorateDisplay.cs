using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IDecorateDisplay
    {
        string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue, string linkedValue);
    }
}
