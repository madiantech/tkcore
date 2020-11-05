namespace YJC.Toolkit.MetaData
{
    public interface IFieldInfoEx : IFieldInfo
    {
        int Length { get; }

        bool IsEmpty { get; }

        int Precision { get; }

        FieldKind Kind { get; }

        string Expression { get; }

        IFieldLayout Layout { get; }

        IFieldControl Control { get; }

        IFieldDecoder Decoder { get; }

        IFieldUpload Upload { get; }

        bool IsShowInList(IPageStyle style, bool isInTable);
    }
}