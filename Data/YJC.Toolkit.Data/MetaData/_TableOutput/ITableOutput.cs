namespace YJC.Toolkit.MetaData
{
    public interface ITableOutput
    {
        bool IsSingle { get; }

        string CreateEditHtml(INormalTableData tableData, object model, object pageData);

        string CreateDetailHeadHtml(INormalTableData tableData, object pageData);

        string CreateDetailBodyHtml(INormalTableData tableData, object model,
            object pageData, int index);

        string CreateDetailListHtml(IListMetaData tableData, object model, object pageData);
    }
}