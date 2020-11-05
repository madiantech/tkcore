using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TabSheetsConfig
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "TabSheet")]
        internal List<TabSheetConfig> TabSheets { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        internal CodeTabSheetConfig CodeTabSheet { get; private set; }

        public RegNameList<ListTabSheet> CreateTabSheet(IDbDataSource dataSource,
            IFieldInfoIndexer indexer)
        {
            if (TabSheets != null)
            {
                RegNameList<ListTabSheet> result = new RegNameList<ListTabSheet>();
                foreach (var item in TabSheets)
                {
                    string sql = item.Condition == null ? null :
                        Expression.Execute(item.Condition, dataSource);
                    IParamBuilder builder = string.IsNullOrEmpty(sql) ? null :
                        SqlParamBuilder.CreateSql(sql);
                    result.Add(new ListTabSheet(item.Id,
                        item.Caption.ToString(ObjectUtil.SysCulture), builder));
                }
                return result;
            }
            if (CodeTabSheet != null)
            {
                RegNameList<ListTabSheet> result = new RegNameList<ListTabSheet>();
                if (CodeTabSheet.NeedAllTab)
                    result.Add(new ListTabSheet("_All", "全部", null));
                CodeTable table = PlugInFactoryManager.CreateInstance<CodeTable>(
                    CodeTablePlugInFactory.REG_NAME, CodeTabSheet.CodeRegName);
                YJC.Toolkit.Decoder.CodeTableContainer data = new YJC.Toolkit.Decoder.CodeTableContainer();
                table.Fill(data, dataSource.Context);

                var tableData = data[CodeTabSheet.CodeRegName];
                IFieldInfo info = indexer[CodeTabSheet.NickName];
                TkDebug.AssertNotNull(info, "", this);
                foreach (var item in tableData)
                {
                    IParamBuilder builder = SqlParamBuilder.CreateEqualSql(dataSource.Context, info, item.Value);
                    var tabSheet = new ListTabSheet(item.Value, item.Name, builder);
                    result.Add(tabSheet);
                }
                return result;
            }

            return null;
        }
    }
}