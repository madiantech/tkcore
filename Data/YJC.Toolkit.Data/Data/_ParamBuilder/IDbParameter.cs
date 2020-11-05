using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IDbParameter
    {
        string FieldName { get; }

        TkDataType DataType { get; }

        object FieldValue { get; }
    }
}
