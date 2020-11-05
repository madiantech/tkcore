using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-09-01", Description = "提供对单表Tree模型的数据源")]
    class SingleDbTreeSourceConfig : BaseResolverConfig, IEditDbConfig, ISingleResolverConfig,
        IDetailDbConfig, ITreeCreator, IReadObjectCallBack
    {
        #region IDetailResolverConfig 成员

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> DetailOperators { get; private set; }

        #endregion

        #region IEditResolverConfig 成员

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        #endregion

        #region ITreeCreator 成员

        public ITree CreateTree(IDbDataSource source)
        {
            ResolverTreeConfig config = new ResolverTreeConfig
            {
                Resolver = Resolver,
                DataRight = DataRight
            };
            return config.CreateObject(source);
        }

        #endregion

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (DetailOperators == null)
                DetailOperators = new SimpleTreeDetailOperatorsConfig();
        }

        #endregion

        public override ISource CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            if (input.IsPost)
            {
                switch (input.Style.Style)
                {
                    case PageStyle.Insert:
                    case PageStyle.Update:
                        return new SingleDbEditSource(this);
                }
            }
            else
            {
                switch (input.Style.Style)
                {
                    case PageStyle.Custom:
                        return new TreeOperationSource(this);
                    case PageStyle.Insert:
                        return new SingleDbInsertSource(this);
                    case PageStyle.Update:
                        return new SingleDbDetailSource(this);
                    case PageStyle.Delete:
                        return new SingleDbDeleteSource(this);
                    case PageStyle.Detail:
                        return new SingleDbDetailSource(this);
                    case PageStyle.List:
                        return new TreeSource(this);
                }
            }

            return null;
        }
    }
}
