using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RelationConfigItem : IReadObjectCallBack
    {
        [SimpleAttribute(DefaultValue = YJC.Toolkit.Data.RelationType.MasterRelation)]
        public YJC.Toolkit.Data.RelationType Type { get; private set; }

        [SimpleAttribute(Required = true)]
        public string MasterTableName { get; private set; }

        [SimpleAttribute(Required = true)]
        public string DetailTableName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string[] MasterFields { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string[] DetailFields { get; private set; }

        public void OnReadObject()
        {
            TkDebug.AssertNotNull(MasterFields, "没有配置MasterFields内容", this);
            TkDebug.AssertNotNull(DetailFields, "没有配置DetailFields内容", this);

            TkDebug.Assert(MasterFields.Length == DetailFields.Length, string.Format(
                ObjectUtil.SysCulture, "MasterFields和DetailFields中含有的字段个数不匹配，"
                + "现在MasterField中有{0}个字段，DetailField有{1}个字段",
                MasterFields.Length, DetailFields.Length), this);
        }

        public TableRelation CreateRelation()
        {
            return new TableRelation(MasterFields, DetailFields, Type, null, null);
        }

        public override string ToString()
        {
            return $"{MasterTableName}和{DetailTableName}的关系";
        }
    }
}