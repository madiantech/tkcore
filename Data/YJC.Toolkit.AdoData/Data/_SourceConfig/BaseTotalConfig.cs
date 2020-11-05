using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal abstract class BaseTotalConfig : BaseDbConfig, IListDbConfig,
        IDetailDbConfig, IEditDbConfig, IReadObjectCallBack
    {
        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Operators == null)
                Operators = new SimpleListOperatorsConfig() { Operators = UpdateKind.All };
            if (DetailOperators == null)
                DetailOperators = new SimpleDetailOperatorsConfig();
        }

        #endregion IReadObjectCallBack 成员

        [SimpleAttribute]
        public int PageSize { get; protected set; }

        [SimpleAttribute]
        public PageStyle DisablePage { get; protected set; }

        [SimpleAttribute]
        public string OrderBy { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool SortQuery { get; protected set; }

        [SimpleAttribute]
        public string FillTableName { get; protected set; }

        [SimpleAttribute]
        public bool UseMetaData { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public TabSheetsConfig TabSheets { get; protected set; }

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> Operators { get; protected set; }

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> DetailOperators { get; protected set; }

        protected abstract ISource CreatePostSource(PageStyle style, IInputData input);

        protected abstract ISource CreateGetSource(PageStyle style, IInputData input);

        public override ISource CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            if ((input.Style.Style & DisablePage) == input.Style.Style)
                throw new ErrorOperationException("该页面被禁止，无法访问", this);

            if (input.IsPost)
                return CreatePostSource(input.Style.Style, input);
            else
                return CreateGetSource(input.Style.Style, input);
        }
    }
}