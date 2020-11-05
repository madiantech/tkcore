using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-29", Description = "单表的列表数据源")]
    internal class DbListSourceConfig : BaseResolverConfig, IListDbConfig, IReadObjectCallBack
    {
        #region IReadObjectCallBack 成员

        public virtual void OnReadObject()
        {
            if (Operators == null)
                Operators = new SimpleListOperatorsConfig() { Operators = UpdateKind.All };
        }

        #endregion IReadObjectCallBack 成员

        public override ISource CreateObject(params object[] args)
        {
            return new DbListSource(this);
        }

        [SimpleAttribute]
        public int PageSize { get; protected set; }

        [SimpleAttribute]
        public string OrderBy { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool SortQuery { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseMetaData { get; protected set; }

        [SimpleAttribute]
        public string FillTableName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public TabSheetsConfig TabSheets { get; protected set; }

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> Operators { get; protected set; }
    }
}