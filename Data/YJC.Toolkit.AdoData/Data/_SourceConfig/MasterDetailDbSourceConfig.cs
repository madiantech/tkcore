using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-09-13", Description = "提供对主从表的增删改查的数据源")]
    internal class MasterDetailDbSourceConfig : BaseTotalConfig, IMultipleResolverConfig
    {
        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> MasterResolver { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public ChildTableInfoConfig Detail { get; private set; }

        IEnumerable<ChildTableInfoConfig> IMultipleResolverConfig.ChildResolvers
        {
            get
            {
                return Detail == null ? null : EnumUtil.Convert(Detail);
            }
        }

        IConfigCreator<TableResolver> IMultipleResolverConfig.MainResolver
        {
            get
            {
                return MasterResolver;
            }
        }

        protected override ISource CreateGetSource(PageStyle style, IInputData input)
        {
            switch (style)
            {
                case PageStyle.Insert:
                    MultipleDbInsertSource source = new MultipleDbInsertSource(this);
                    //source.AddTableResolver(MasterResolver.CreateObject(source));
                    //source.AddTableResolver(Detail.Resolver.CreateObject(source));
                    return source;

                case PageStyle.Update:
                    return new MultipleDbDetailSource(this) { FillDetailData = true };

                case PageStyle.Delete:
                    return new MultipleDbDeleteSource(this);

                case PageStyle.Detail:
                    return new MultipleDbDetailSource(this) { FillDetailData = false };

                case PageStyle.List:
                    return new DbListSource(this);

                case PageStyle.Custom:
                    if (input.Style.Operation == "DetailList")
                        return new DbDetailListSource(this, MasterResolver, Detail);
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
                    return new MultipleDbEditSource(this);

                case PageStyle.List:
                    return new DbListSource(this);
            }
            return null;
        }
    }
}