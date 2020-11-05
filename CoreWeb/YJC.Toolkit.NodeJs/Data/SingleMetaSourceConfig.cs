using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig]
    internal class SingleMetaSourceConfig : IConfigCreator<ISource>
    {
        public ISource CreateObject(params object[] args)
        {
            return new SingleMetaSource(this);
        }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit, Required = true)]
        public IConfigCreator<TableResolver> Resolver { get; protected set; }

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }
    }
}