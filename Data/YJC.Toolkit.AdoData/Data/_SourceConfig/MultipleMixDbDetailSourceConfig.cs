using System.Collections.Generic;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2018-07-30", Description = "提供对一对一，一对多表混合使用的数据源")]
    internal class MultipleMixDbDetailSourceConfig : BaseDbConfig, IMultipleResolverConfig,
        IEditDbConfig, IDetailDbConfig, IMultipleMixDbSourceConfig, IReadObjectCallBack
    {
        #region IMultipleResolverConfig 成员

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> MainResolver { get; private set; }

        public IEnumerable<ChildTableInfoConfig> ChildResolvers
        {
            get
            {
                return OneToManyTables;
            }
        }

        #endregion IMultipleResolverConfig 成员

        #region IEditDbConfig 成员

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        #endregion IEditDbConfig 成员

        #region IDetailDbConfig 成员

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> DetailOperators { get; private set; }

        #endregion IDetailDbConfig 成员

        #region IMultipleMixDbSourceConfig 成员

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "OtoOTable",
            CollectionType = typeof(List<OneToOneChildTableInfoConfig>))]
        public IEnumerable<OneToOneChildTableInfoConfig> OneToOneTables { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "OtoMTable",
            CollectionType = typeof(List<ChildTableInfoConfig>))]
        public IEnumerable<ChildTableInfoConfig> OneToManyTables { get; private set; }

        #endregion IMultipleMixDbSourceConfig 成员

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (DetailOperators == null)
                DetailOperators = new SimpleDetailOperatorsConfig();
        }

        #endregion IReadObjectCallBack 成员

        [SimpleAttribute]
        public bool FillDetailData { get; private set; }

        public override ISource CreateObject(params object[] args)
        {
            MultipleMixDbDetailSource source = new MultipleMixDbDetailSource(this)
            {
                FillDetailData = FillDetailData
            };
            return source;
        }
    }
}