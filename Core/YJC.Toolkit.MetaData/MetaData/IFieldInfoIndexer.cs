namespace YJC.Toolkit.MetaData
{
    public interface IFieldInfoIndexer
    {
        IFieldInfo this[string nickName] { get; }
    }
}
