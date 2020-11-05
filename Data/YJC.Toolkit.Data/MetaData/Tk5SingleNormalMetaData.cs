using System;
using YJC.Toolkit.Sys;
using System.Collections.Generic;

namespace YJC.Toolkit.MetaData
{
    [Serializable]
    public sealed class Tk5SingleNormalMetaData : IMetaData, INormalMetaData, IFileDependency
    {
        private readonly Tk5TableScheme fTableScheme;

        public Tk5SingleNormalMetaData(ITableSchemeEx dataXml, IInputData input,
            ISingleMetaData config)
        {
            TkDebug.AssertArgumentNull(dataXml, "dataXml", null);
            TkDebug.AssertArgumentNull(input, "input", null);
            TkDebug.AssertArgumentNull(config, "config", null);

            ColumnCount = config.ColumnCount;
            fTableScheme = config.CreateTableScheme(dataXml, input);
            Files = FileUtil.GetFileDependecy(fTableScheme);
            Table = new Tk5NormalTableData(fTableScheme, config, SearchControlMethod.Id, input.Style);
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

        #region INormalMetaData 成员

        public bool Single
        {
            get
            {
                return true;
            }
        }

        public INormalTableData TableData
        {
            get
            {
                return Table;
            }
        }

        public IEnumerable<INormalTableData> TableDatas
        {
            get
            {
                return EnumUtil.Convert<INormalTableData>(Table);
            }
        }

        #endregion INormalMetaData 成员

        [SimpleAttribute]
        public int ColumnCount { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public Tk5NormalTableData Table { get; private set; }

        public IEnumerable<string> Files { get; }
    }
}