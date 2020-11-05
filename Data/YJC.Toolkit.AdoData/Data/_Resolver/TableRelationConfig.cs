using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class TableRelationConfig : IReadObjectCallBack, IRegName
    {
        #region IReadObjectCallBack ��Ա

        void IReadObjectCallBack.OnReadObject()
        {
            TkDebug.AssertNotNull(MasterFields, "û������MasterFields����", this);
            TkDebug.AssertNotNull(DetailFields, "û������DetailFields����", this);

            TkDebug.Assert(MasterFields.Length == DetailFields.Length, string.Format(
                ObjectUtil.SysCulture, "MasterFields��DetailFields�к��е��ֶθ�����ƥ�䣬"
                + "����MasterField����{0}���ֶΣ�DetailField��{1}���ֶ�",
                MasterFields.Length, DetailFields.Length), this);
        }

        #endregion

        #region IRegName ��Ա

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
                return string.Format(ObjectUtil.SysCulture, "{0}��{1}��Relation",
                    string.Join(",", MasterFields), string.Join(",", DetailFields));
            }
            catch
            {
                return base.ToString();
            }
        }
    }
}