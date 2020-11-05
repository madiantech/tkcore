using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig]
    internal class ListOperatorSourceConfig : IConfigCreator<ISource>
    {
        public ISource CreateObject(params object[] args)
        {
            return new ListOperatorSource
            {
                Operators = Operators?.CreateObject()
            };
        }

        [DynamicElement(OperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IOperatorsConfig> Operators { get; protected set; }
    }
}