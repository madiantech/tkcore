namespace YJC.Toolkit.Data
{
    public class OneToOneChildTableInfo : ChildTableInfo
    {
        internal OneToOneChildTableInfo(IDbDataSource source,
            OneToOneChildTableInfoConfig config)
            : base(source, config)
        {
            NoRecordHandler = config.NoRecordHandler;
        }

        public NoRecordHandler NoRecordHandler { get; private set; }
    }
}