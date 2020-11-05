using System;
using System.Collections.Generic;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [Serializable]
    public sealed class Tk5MultipleMetaData : IMetaData, INormalMetaData, IFileDependency
    {
        private readonly RegNameList<Tk5TableScheme> fSchemes;

        private Tk5MultipleMetaData()
        {
            fSchemes = new RegNameList<Tk5TableScheme>();
            Tables = new List<Tk5NormalTableData>();
        }

        internal Tk5MultipleMetaData(IInputData input, IEnumerable<ISingleMetaData> masters,
            IEnumerable<DetailSingleMetaDataConfig> details)
            : this()
        {
            foreach (var item in masters)
            {
                Tk5TableScheme scheme = CreateTableScheme(input, item);
                Tk5NormalTableData table = new Tk5NormalTableData(scheme, item, SearchControlMethod.Id, input.Style);
                Tables.Add(table);
            }
            if (details != null)
                foreach (var item in details)
                    CreateDetailSchemaData(input, item);

            Files = FileUtil.GetFileDependecyFromEnumerable(fSchemes);
        }

        internal Tk5MultipleMetaData(IInputData input, DetailSingleMetaDataConfig detail)
            : this()
        {
            CreateDetailSchemaData(input, detail);
            Files = FileUtil.GetFileDependecyFromEnumerable(fSchemes);
        }

        #region IMetaData 成员

        public string Title { get; set; }

        public object ToToolkitObject()
        {
            return this;
        }

        public ITableSchemeEx GetTableScheme(string tableName)
        {
            return fSchemes[tableName];
        }

        #endregion IMetaData 成员

        #region INormalMetaData 成员

        public bool Single
        {
            get
            {
                return false;
            }
        }

        public INormalTableData TableData
        {
            get
            {
                if (Tables.Count > 0)
                    return Tables[0];
                TkDebug.ThrowToolkitException("多表配置中，连一个TableData也没有", this);
                return null;
            }
        }

        public IEnumerable<INormalTableData> TableDatas
        {
            get
            {
                return Tables;
            }
        }

        #endregion INormalMetaData 成员

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Table")]
        public List<Tk5NormalTableData> Tables { get; private set; }

        public IEnumerable<string> Files { get; }

        private void CreateDetailSchemaData(IInputData input, DetailSingleMetaDataConfig item)
        {
            Tk5TableScheme scheme = CreateTableScheme(input, item.CreateSingleMetaData());
            Tk5NormalTableData table = new Tk5NormalTableData(scheme, item, input.Style);
            Tables.Add(table);
        }

        private Tk5TableScheme CreateTableScheme(IInputData input, ISingleMetaData item)
        {
            ITableSchemeEx sourceScheme = item.CreateSourceScheme(input);
            Tk5TableScheme scheme = item.CreateTableScheme(sourceScheme, input);
            fSchemes.Add(scheme);
            return scheme;
        }
    }
}