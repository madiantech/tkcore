using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-12-02", Description = "提供对单表的Detail数据源")]
    class SingleDbDetailSourceConfig : BaseResolverConfig, IDetailDbConfig,
        ISingleResolverConfig, IReadObjectCallBack
    {
        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (DetailOperators == null)
                DetailOperators = new SimpleDetailOperatorsConfig();
        }

        #endregion

        public override ISource CreateObject(params object[] args)
        {
            return new SingleDbDetailSource(this);
        }

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> DetailOperators { get; private set; }
    }
}
