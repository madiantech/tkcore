namespace YJC.Toolkit.MetaData
{
    public interface IDisplayObject
    {
        bool SupportDisplay { get; }

        IFieldInfo Id { get; }

        IFieldInfo Name { get; }
    }
}
