using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-09-19", Description = "提供对多表的Detail数据源")]
    internal class MultipleDbDetailSourceConfig : BaseMultipleConfig, IEditDbConfig,
        IDetailDbConfig, IReadObjectCallBack
    {
        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (DetailOperators == null)
                DetailOperators = new SimpleDetailOperatorsConfig();
        }

        #endregion

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        [SimpleAttribute]
        public bool FillDetailData { get; private set; }

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> DetailOperators { get; private set; }

        public override ISource CreateObject(params object[] args)
        {
            return new MultipleDbDetailSource(this) { FillDetailData = FillDetailData };
        }
    }
}
