namespace YJC.Toolkit.Data
{
    interface ITreeCreator
    {
        string Context { get; }

        ITree CreateTree(IDbDataSource source);
    }
}
