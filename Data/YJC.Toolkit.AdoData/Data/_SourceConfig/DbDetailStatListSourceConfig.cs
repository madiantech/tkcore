using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2019-05-06", Description = "在Detail页面下，用统计列表的方式显示子表的数据源")]
    internal class DbDetailStatListSourceConfig : DbDetailListSourceConfig
    {
        public override ISource CreateObject(params object[] args)
        {
            TkDebug.AssertNotNull(ChildTables, "没有配置ChildTable子节点", this);

            if (ChildTables.Count == 1)
                return new DbDetailStatListSource(this, Resolver, ChildTables[0]);

            IInputData input = ObjectUtil.QueryObject<IInputData>(args);
            TkDebug.AssertNotNull(input, "参数args缺少IInputData类型", this);
            string name = input.QueryString["ChildName"];
            var childTable = (from item in ChildTables
                              where item.RegName == name
                              select item).FirstOrDefault();
            TkDebug.AssertNotNull(childTable, string.Format(ObjectUtil.SysCulture,
                "QueryString中的ChildName是{0},没有找到对应的Child配置", name), this);
            return new DbDetailStatListSource(this, Resolver, childTable);
        }
    }
}