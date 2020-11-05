using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2018-07-30", Description = "提供对一对一，一对多表混合使用的增删改查的数据源")]
    internal class MultipleMixDbSourceConfig : BaseTotalConfig, IMultipleResolverConfig, IMultipleMixDbSourceConfig
    {
        private class PrivateConfig : IMultipleResolverConfig, IEditDbConfig
        {
            private readonly MultipleMixDbSourceConfig fConfig;

            public PrivateConfig(MultipleMixDbSourceConfig config)
            {
                fConfig = config;
            }

            #region IMultipleResolverConfig 成员

            public IConfigCreator<TableResolver> MainResolver
            {
                get
                {
                    return fConfig.Resolver;
                }
            }

            public IEnumerable<ChildTableInfoConfig> ChildResolvers
            {
                get
                {
                    return EnumUtil.Convert(fConfig.OneToOneTables, fConfig.OneToManyTables);
                }
            }

            #endregion IMultipleResolverConfig 成员

            #region IEditDbConfig 成员

            public bool UseMetaData
            {
                get
                {
                    return fConfig.UseMetaData;
                }
            }

            #endregion IEditDbConfig 成员

            #region IBaseDbConfig 成员

            public string Context
            {
                get
                {
                    return fConfig.Context;
                }
            }

            public bool SupportData
            {
                get
                {
                    return fConfig.SupportData;
                }
            }

            public IConfigCreator<IDataRight> DataRight
            {
                get
                {
                    return fConfig.DataRight;
                }
            }

            public FunctionRightConfig FunctionRight { get => fConfig.FunctionRight; }

            #endregion IBaseDbConfig 成员
        }

        #region IMultipleResolverConfig 成员

        public IConfigCreator<TableResolver> MainResolver
        {
            get
            {
                return Resolver;
            }
        }

        public IEnumerable<ChildTableInfoConfig> ChildResolvers
        {
            get
            {
                return OneToManyTables;
            }
        }

        #endregion IMultipleResolverConfig 成员

        #region IMultipleMixDbSourceConfig 成员

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "OtoOTable",
            CollectionType = typeof(List<OneToOneChildTableInfoConfig>))]
        public IEnumerable<OneToOneChildTableInfoConfig> OneToOneTables { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "OtoMTable",
            CollectionType = typeof(List<ChildTableInfoConfig>))]
        public IEnumerable<ChildTableInfoConfig> OneToManyTables { get; private set; }

        #endregion IMultipleMixDbSourceConfig 成员

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> Resolver { get; private set; }

        protected override ISource CreatePostSource(PageStyle style, IInputData input)
        {
            switch (style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                    PrivateConfig config = new PrivateConfig(this);
                    return new MultipleDbEditSource(config);

                case PageStyle.List:
                    return new DbListSource(this);
            }
            return null;
        }

        protected override ISource CreateGetSource(PageStyle style, IInputData input)
        {
            PrivateConfig config;
            switch (style)
            {
                case PageStyle.Custom:
                    if (input.Style.Operation == DbListSource.TAB_STYLE_OPERATION)
                        return new DbListSource(this);
                    if (MetaDataUtil.StartsWith(input.Style, "DetailList"))
                    {
                        int index = input.QueryString["Index"].Value<int>();
                        List<ChildTableInfoConfig> oneMany = OneToManyTables.Convert<List<ChildTableInfoConfig>>();
                        if (OneToManyTables != null && index < oneMany.Count)
                        {
                            var child = oneMany[index];
                            return new DbDetailListSource(this, Resolver, child);
                        }
                    }
                    break;

                case PageStyle.Insert:
                    config = new PrivateConfig(this);
                    MultipleDbInsertSource source = new MultipleDbInsertSource(config);
                    return source;

                case PageStyle.Update:
                    MultipleMixDbDetailSource updateSource = new MultipleMixDbDetailSource(this)
                    {
                        FillDetailData = true
                    };
                    return updateSource;

                case PageStyle.Delete:
                    config = new PrivateConfig(this);
                    return new MultipleDbDeleteSource(config);

                case PageStyle.Detail:
                    MultipleMixDbDetailSource detailSource = new MultipleMixDbDetailSource(this)
                    {
                        FillDetailData = false
                    };
                    return detailSource;

                case PageStyle.List:
                    return new DbListSource(this);
            }
            return null;
        }
    }
}