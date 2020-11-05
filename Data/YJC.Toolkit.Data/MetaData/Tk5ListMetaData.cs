using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public sealed class Tk5ListMetaData : IMetaData, IListMetaData, IFileDependency
    {
        private readonly Tk5TableScheme fTableScheme;

        public Tk5ListMetaData(ITableSchemeEx dataXml, IInputData input, ISingleMetaData config,
            ITableOutput tableOutput = null)
        {
            fTableScheme = config.CreateTableScheme(dataXml, input);
            SetFileDependency(fTableScheme);
            Table = new Tk5ListTableData(fTableScheme, tableOutput);
        }

        public Tk5ListMetaData(Tk5ListMetaData metaData, IEnumerable<Tk5FieldInfoEx> fields)
        {
            fTableScheme = new Tk5TableScheme(metaData.fTableScheme, fields);
            SetFileDependency(fTableScheme);
            Table = new Tk5ListTableData(fTableScheme, null);
        }

        #region IMetaData 成员

        public object ToToolkitObject()
        {
            return this;
        }

        public ITableSchemeEx GetTableScheme(string tableName)
        {
            if (tableName == fTableScheme.TableName)
                return fTableScheme;
            else
                return null;
        }

        [SimpleAttribute]
        public string Title { get; set; }

        #endregion IMetaData 成员

        #region IListMetaData 成员

        public IListTableData TableData
        {
            get
            {
                return Table;
            }
        }

        #endregion IListMetaData 成员

        [ObjectElement(NamespaceType.Toolkit)]
        public Tk5ListTableData Table { get; private set; }

        public IEnumerable<string> Files { get; private set; }

        private void SetFileDependency(ITableSchemeEx scheme)
        {
            Files = FileUtil.GetFileDependecy(scheme);
        }
    }
}