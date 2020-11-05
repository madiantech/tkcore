using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-05-03", Description = "提供对单表增删改，Detail含有子表的数据源")]
    internal class SingleDbDetailListSourceConfig : BaseTotalConfig, IMultipleResolverConfig
    {
        #region IMultipleResolverConfig 成员

        IConfigCreator<TableResolver> IMultipleResolverConfig.MainResolver
        {
            get
            {
                return Resolver;
            }
        }

        IEnumerable<ChildTableInfoConfig> IMultipleResolverConfig.ChildResolvers
        {
            get
            {
                return ChildTables;
            }
        }

        #endregion IMultipleResolverConfig 成员

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> Resolver { get; protected set; }

        [SimpleAttribute]
        public bool FillDetailData { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "ChildTable")]
        public List<ChildTableInfoConfig> ChildTables { get; private set; }

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
                    return new MultipleDbDetailSource(this) { FillDetailData = FillDetailData };

                case PageStyle.List:
                    return new DbListSource(this);

                case PageStyle.Custom:
                    if (input.Style.Operation == DbListSource.TAB_STYLE_OPERATION)
                        return new DbListSource(this);
                    if (MetaDataUtil.StartsWith(input.Style, "DetailList"))
                    {
                        int index = input.QueryString["Index"].Value<int>(0);
                        if (ChildTables != null && index < ChildTables.Count)
                        {
                            var child = ChildTables[index];
                            return new DbDetailListSource(this, Resolver, child);
                        }
                    }
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