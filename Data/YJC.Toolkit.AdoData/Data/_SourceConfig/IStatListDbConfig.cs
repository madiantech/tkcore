namespace YJC.Toolkit.Data
{
    public interface IStatListDbConfig : IListDbConfig
    {
        StatConfigItem Stat { get; }
    }
}