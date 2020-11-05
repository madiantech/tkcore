namespace YJC.Toolkit.Data
{
    public interface IEditDbConfig : IBaseDbConfig
    {
        bool UseMetaData { get; }
    }
}