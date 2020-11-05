using System.Collections.Generic;
using System.IO;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class Tk5DataXml : IDisplayObject, ITableSchemeEx, ITableScheme, IActiveData,
        ICacheDependencyCreator, IFileDependency
    {
        private readonly InternalTk5DataXml fDataXml;

        internal Tk5DataXml(InternalTk5DataXml dataXml)
            : this(dataXml, dataXml.TableName)
        {
        }

        internal Tk5DataXml(InternalTk5DataXml dataXml, string tableName)
        {
            TkDebug.AssertArgumentNull(dataXml, "dataXml", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            fDataXml = dataXml;
            TableName = tableName;
            TableDesc = dataXml.TableDesc;
            Files = dataXml.Files;
        }

        #region IDisplayObject 成员

        public bool SupportDisplay
        {
            get
            {
                return fDataXml.SupportDisplay;
            }
        }

        public IFieldInfo Id
        {
            get
            {
                return fDataXml.Id;
            }
        }

        public IFieldInfo Name
        {
            get
            {
                return fDataXml.Name;
            }
        }

        #endregion IDisplayObject 成员

        #region ITableSchemeEx 成员

        public string TableName { get; private set; }

        public string TableDesc { get; internal set; }

        public IFieldInfoEx NameField { get => fDataXml.NameField; }

        public IEnumerable<IFieldInfoEx> Fields
        {
            get
            {
                return fDataXml.Fields;
            }
        }

        public IEnumerable<IFieldInfoEx> AllFields
        {
            get
            {
                return fDataXml.AllFields;
            }
        }

        #endregion ITableSchemeEx 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fDataXml[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region ITableScheme 成员

        IEnumerable<IFieldInfo> ITableScheme.Fields
        {
            get
            {
                ITableScheme scheme = fDataXml;
                return scheme.Fields;
            }
        }

        IEnumerable<IFieldInfo> ITableScheme.AllFields
        {
            get
            {
                ITableScheme scheme = fDataXml;
                return scheme.AllFields;
            }
        }

        #endregion ITableScheme 成员

        #region IActiveData 成员

        public IParamBuilder CreateParamBuilder(TkDbContext context, IFieldInfoIndexer indexer)
        {
            return fDataXml.CreateParamBuilder(context, indexer);
        }

        #endregion IActiveData 成员

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return fDataXml.CreateCacheDependency();
        }

        #endregion ICacheDependencyCreator 成员

        public FakeDeleteInfo FakeDeleteInfo
        {
            get
            {
                return fDataXml.Table.FakeDeleteInfo;
            }
        }

        public DbTreeDefinition TreeDefinition
        {
            get
            {
                return fDataXml.Table.Tree;
            }
        }

        public IEnumerable<string> Files { get; }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "表名{0},{1}", TableName, fDataXml);
        }

        private static InternalTk5DataXml GetInternalDataXml(string fileName)
        {
            string path = Path.Combine(BaseAppSetting.Current.XmlPath, "Data", fileName);
            InternalTk5DataXml dataXml = (InternalTk5DataXml)CacheManager.GetItem("Tk5DataXml", path);
            return dataXml;
        }

        private static InternalTk5DataXml GetInternalDataXml(Stream stream)
        {
            InternalTk5DataXml dataXml = new InternalTk5DataXml();
            dataXml.ReadFromStream("Xml", null, stream, ObjectUtil.ReadSettings, QName.Toolkit);
            return dataXml;
        }

        public static Tk5DataXml Create(string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            InternalTk5DataXml dataXml = GetInternalDataXml(fileName);
            return new Tk5DataXml(dataXml);
        }

        public static Tk5DataXml Create(string fileName, string tableName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            InternalTk5DataXml dataXml = GetInternalDataXml(fileName);
            return new Tk5DataXml(dataXml, tableName);
        }

        public static Tk5DataXml Create(Stream stream)
        {
            TkDebug.AssertArgumentNull(stream, "stream", null);

            InternalTk5DataXml dataXml = GetInternalDataXml(stream);
            return new Tk5DataXml(dataXml);
        }

        public static Tk5DataXml Create(Stream stream, string tableName)
        {
            TkDebug.AssertArgumentNull(stream, "stream", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            InternalTk5DataXml dataXml = GetInternalDataXml(stream);
            return new Tk5DataXml(dataXml, tableName);
        }
    }
}