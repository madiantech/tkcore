using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    internal class InternalTk5DataXml : ToolkitConfig, ICacheDependencyCreator, IDisplayObject,
        ITableSchemeEx, ITableScheme, IActiveData, IFileDependency
    {
        private string fFileName;

        internal InternalTk5DataXml()
        {
            ExpectedVersion = "5.0";
        }

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            if (string.IsNullOrEmpty(FullPath))
                return NoDependency.Dependency;
            else
                return new FileInfoDependency(FullPath);
        }

        #endregion ICacheDependencyCreator 成员

        #region IDisplayObject 成员

        public bool SupportDisplay { get; private set; }

        public IFieldInfo Id { get; private set; }

        public IFieldInfo Name { get; private set; }

        #endregion IDisplayObject 成员

        #region ITableSchemeEx 成员

        public string TableName
        {
            get
            {
                return Table.TableName;
            }
        }

        public string TableDesc
        {
            get
            {
                return Table.TableDesc == null ? TableName : Table.TableDesc.ToString();
            }
        }

        public IFieldInfoEx NameField { get => Name as IFieldInfoEx; }

        public IEnumerable<IFieldInfoEx> Fields
        {
            get
            {
                return Table.FieldList;
            }
        }

        public IEnumerable<IFieldInfoEx> AllFields
        {
            get
            {
                return Table.FieldList;
            }
        }

        #endregion ITableSchemeEx 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return Table.FieldList == null ? null : Table.FieldList[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region ITableScheme 成员

        IEnumerable<IFieldInfo> ITableScheme.Fields
        {
            get
            {
                return Table.DataList;
            }
        }

        IEnumerable<IFieldInfo> ITableScheme.AllFields
        {
            get
            {
                return Table.DataList;
            }
        }

        #endregion ITableScheme 成员

        #region IActiveData 成员

        public IParamBuilder CreateParamBuilder(TkDbContext context, IFieldInfoIndexer indexer)
        {
            return Table.CreateParamBuilder(context, indexer);
        }

        #endregion IActiveData 成员

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        internal TableConfigItem Table { get; private set; }

        public IEnumerable<string> Files { get; private set; }

        protected override void OnSetFullPath(string path)
        {
            fFileName = Path.GetFileName(path);
            Files = EnumUtil.Convert(path);
        }

        protected override void OnReadObject()
        {
            base.OnReadObject();

            SupportDisplay = false;
            var keyFields = (from item in Table.FieldList
                             where item.IsKey
                             select item).ToArray();
            if (keyFields.Length == 1)
            {
                Id = keyFields[0];
                if (string.IsNullOrEmpty(Table.NameField))
                {
                    Name = (from item in Table.FieldList
                            where item.NickName.EndsWith("Name", StringComparison.Ordinal)
                            select item).FirstOrDefault();
                    if (Name != null)
                        SupportDisplay = true;
                }
                else
                {
                    Name = Table.FieldList[Table.NameField];
                    if (Name != null)
                        SupportDisplay = true;
                }
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(fFileName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "文件名为{0}的Tk5DataXml", fFileName);
        }
    }
}