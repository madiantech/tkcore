using System.Collections.Generic;

namespace YJC.Toolkit.Data
{
    internal interface IMultipleMixDbSourceConfig
    {
        IEnumerable<OneToOneChildTableInfoConfig> OneToOneTables { get; }

        IEnumerable<ChildTableInfoConfig> OneToManyTables { get; }
    }
}