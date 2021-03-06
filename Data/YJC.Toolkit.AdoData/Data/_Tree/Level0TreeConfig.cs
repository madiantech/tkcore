﻿using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [TreeConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-20",
        Description = "通过编码规律定义树形结构，不足的编码补0")]
    class Level0TreeConfig : BaseLevelTreeConfig
    {
        protected override ITree CreateTree(ITableScheme scheme, LevelTreeDefinition treeDef,
            IDbDataSource dataSource)
        {
            return new Level0Tree(scheme, treeDef, dataSource);
        }
    }
}
