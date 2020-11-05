
namespace YJC.Toolkit.MetaData
{
    public interface ISupportMetaData
    {
        bool CanUseMetaData(IPageStyle style);

        void SetMetaData(IPageStyle style, IMetaData metaData);
    }
}
