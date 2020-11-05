using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class TableRelationConfig : IReadObjectCallBack, IRegName
    {
        #region IReadObjectCallBack 成员

        void IReadObjectCallBack.OnReadObject()
        {
            TkDebug.AssertNotNull(MasterFields, "没有配置MasterFields内容", this);
            TkDebug.AssertNotNull(DetailFields, "没有配置DetailFields内容", this);

            TkDebug.Assert(MasterFields.Length == DetailFields.Length, string.Format(
                ObjectUtil.SysCulture, "MasterFields和DetailFields中含有的字段个数不匹配，"
                + "现在MasterField中有{0}个字段，DetailField有{1}个字段",
                MasterFields.Length, DetailFields.Length), this);
        }

        #endregion

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return Name;
            }
        }

        #endregion

        [SimpleAttribute]
        public string Name { get; private set; }

        [SimpleAttribute(DefaultValue = RelationType.MasterRelation)]
        public RelationType Type { get; private set; }

        [SimpleAttribute]
        public int Top { get; private set; }

        [SimpleAttribute]
        public string OrderBy { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string[] MasterFields { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string[] DetailFields { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public ManyToManyRelation ManyToMany { get; private set; }

        public override string ToString()
        {
            try
            {
                return string.Format(ObjectUtil.SysCulture, "{0}和{1}的Relation",
                    string.Join(",", MasterFields), string.Join(",", DetailFields));
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}