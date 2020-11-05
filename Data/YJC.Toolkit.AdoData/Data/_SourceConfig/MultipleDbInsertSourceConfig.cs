using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-09-11", Description = "提供对多表的Insert在Get的时候的数据源")]
    internal class MultipleDbInsertSourceConfig : BaseDbConfig
    {
        public override ISource CreateObject(params object[] args)
        {
            MultipleDbInsertSource source = new MultipleDbInsertSource(this)
            {
                UseMetaData = UseMetaData
            };
            if (Resolvers != null)
            {
                int i = 0;
                foreach (var item in Resolvers)
                {
                    TableResolver resolver = item.CreateObject(source);
                    if (i++ == 0)
                        source.SetMainResolver(resolver);
                    else
                        source.AddDetailTableResolver(resolver, IsDetailEmptyRow);
                }
            }

            return source;
        }

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        [SimpleAttribute]
        public bool IsDetailEmptyRow { get; set; }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME, IsMultiple = true)]
        [TagElement(NamespaceType.Toolkit, LocalName = "Resolver", Required = true)]
        public List<IConfigCreator<TableResolver>> Resolvers { get; protected set; }
    }
}