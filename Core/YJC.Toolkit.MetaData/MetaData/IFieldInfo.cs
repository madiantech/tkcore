namespace YJC.Toolkit.MetaData
{
    public interface IFieldInfo
    {
        string FieldName { get; }

        string DisplayName { get; }

        string NickName { get; }

        TkDataType DataType { get; }

        bool IsKey { get; }

        bool IsAutoInc { get; }
    }
}
