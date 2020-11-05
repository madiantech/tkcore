using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IListDbConfig : IEditDbConfig
    {
        int PageSize { get; }

        string OrderBy { get; }

        bool SortQuery { get; }

        string FillTableName { get; }

        MarcoConfigItem FilterSql { get; }

        TabSheetsConfig TabSheets { get; }

        IConfigCreator<IOperatorsConfig> Operators { get; }
    }
}