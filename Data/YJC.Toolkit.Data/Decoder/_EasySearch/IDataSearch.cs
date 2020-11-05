
namespace YJC.Toolkit.Decoder
{
    public interface IDataSearch
    {
        bool Search(EasySearch easySearch, SearchField searchType,
            string dataValue, string searchValue);
    }
}
