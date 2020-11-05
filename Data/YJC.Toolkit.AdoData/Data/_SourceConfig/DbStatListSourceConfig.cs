using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2019-04-29", Description = "提供统计功能的单表数据源")]
    internal class DbStatListSourceConfig : DbListSourceConfig, IStatListDbConfig
    {
        #region IStatListDbConfig 成员

        [ObjectElement(NamespaceType.Toolkit)]
        public StatConfigItem Stat { get; private set; }

        #endregion IStatListDbConfig 成员

        public override void OnReadObject()
        {
            if (Operators == null)
                Operators = new EmptyOperatorsConfig();
        }

        public override ISource CreateObject(params object[] args)
        {
            return new DbStatListSource(this);
        }
    }
}