using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-05-03",
        Description = "将List，Detail，Edit的单表操作的Source整合在一起，完成整个单表的增删改查工作的数据源")]
    internal class SingleDbSourceConfig : BaseTotalConfig, ISingleResolverConfig
    {
        #region ISingleResolverConfig 成员

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> Resolver { get; protected set; }

        #endregion ISingleResolverConfig 成员

        protected override ISource CreateGetSource(PageStyle style, IInputData input)
        {
            switch (style)
            {
                case PageStyle.Insert:
                    return new SingleDbInsertSource(this);

                case PageStyle.Update:
                    return new SingleDbDetailSource(this);

                case PageStyle.Delete:
                    return new SingleDbDeleteSource(this);

                case PageStyle.Detail:
                    return new SingleDbDetailSource(this);

                case PageStyle.List:
                    return new DbListSource(this);

                case PageStyle.Custom:
                    if (input.Style.Operation == DbListSource.TAB_STYLE_OPERATION)
                        return new DbListSource(this);
                    break;
            }
            return null;
        }

        protected override ISource CreatePostSource(PageStyle style, IInputData input)
        {
            switch (style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                    return new SingleDbEditSource(this);

                case PageStyle.List:
                    return new DbListSource(this);
            }
            return null;
        }
    }
}