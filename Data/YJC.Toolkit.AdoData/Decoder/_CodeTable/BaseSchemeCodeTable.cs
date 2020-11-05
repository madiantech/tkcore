using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class BaseSchemeCodeTable : BaseDbCodeTable
    {
        private readonly ITableScheme fScheme;
        private readonly IDisplayObject fDisplay;

        protected BaseSchemeCodeTable(ITableScheme scheme)
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);
            TkDebug.AssertArgument(scheme is IDisplayObject, "scheme",
                "scheme需要支持IDisplayObject接口", scheme);

            fScheme = scheme;
            fDisplay = scheme.Convert<IDisplayObject>();
            TkDebug.Assert(fDisplay.SupportDisplay,
                "scheme的SupportDisplay必须为true，当前是false", scheme);
        }

        public MarcoConfigItem FilterSql { get; set; }

        public string OrderBy { get; set; }

        public ITableScheme Scheme
        {
            get
            {
                return fScheme;
            }
        }

        protected override DataTable FillDbData(TkDbContext context, DataSet dataSet)
        {
            using (SqlSelector selector = new SqlSelector(context, dataSet))
            {
                TableSchemeData data = new TableSchemeData(context, Scheme);
                ParamBuilderContainer container = new ParamBuilderContainer();
                if (FilterSql != null)
                {
                    string filter = Expression.Execute(FilterSql, selector);
                    container.Add(filter);
                }
                container.Add(CreateActiveFilter(context));

                string tableName = Scheme.TableName;
                string sql = string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1}",
                    data.SelectFields, context.EscapeName(tableName));
                string regName = RegName;
                selector.Select(regName, sql, container, OrderBy);
                DataTable dataTable = dataSet.Tables[regName];

                DataColumn idColumn = dataTable.Columns[fDisplay.Id.NickName];
                idColumn.ColumnName = DecoderConst.CODE_NICK_NAME;
                dataTable.Columns[fDisplay.Name.NickName].ColumnName = DecoderConst.NAME_NICK_NAME;

                if (NameExpression != "{Name}")
                {
                    foreach (DataRow row in dataTable.Rows)
                        row[DecoderConst.NAME_NICK_NAME] = InternalExpression.Execute(row);
                }
                return dataTable;
            }
        }

        protected virtual IParamBuilder CreateActiveFilter(TkDbContext context)
        {
            return null;
        }
    }
}
