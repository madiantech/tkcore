using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [ObjectContext]
    [TableSchemeConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-08-20",
        Author = "YJC", Description = "从标记TypeSchemeAttribute的类型中提取获得相应的单表Scheme")]
    [TableSchemeExConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2017-11-01",
        Author = "YJC", Description = "从标记TypeSchemeAttribute的类型中提取获得相应的单表TableSchemeEx")]
    internal class TypeTableSchemeConfig : IConfigCreator<ITableScheme>, IConfigCreator<ITableSchemeEx>
    {
        #region IConfigCreator<ITableScheme> 成员

        public ITableScheme CreateObject(params object[] args)
        {
            return MetaDataUtil.CreateTypeTableScheme(TypeRegName);
        }

        #endregion IConfigCreator<ITableScheme> 成员

        [SimpleAttribute(Required = true)]
        public string TypeRegName { get; private set; }

        ITableSchemeEx IConfigCreator<ITableSchemeEx>.CreateObject(params object[] args)
        {
            return MetaDataUtil.CreateTypeTableScheme(TypeRegName);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "类型注册名为{0}的类型TableScheme", TypeRegName);
        }
    }
}