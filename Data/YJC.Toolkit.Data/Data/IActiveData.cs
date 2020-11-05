using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IActiveData
    {
        IParamBuilder CreateParamBuilder(TkDbContext context, IFieldInfoIndexer indexer);
    }
}
