using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IEditModel : IModel
    {
        string PageStyle { get; }

        string RetUrl { get; }

        IFieldValueProvider CreateMainObjectProvider(INormalTableData metaData);

        IFieldValueProvider CreateMainObjectProvider(string tableName);
    }
}