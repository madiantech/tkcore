using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface ISingleResolverConfig : IBaseDbConfig
    {
        IConfigCreator<TableResolver> Resolver { get; }
    }
}
