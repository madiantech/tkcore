using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-07-12", Description = "对某表的某字段值进行开关切换的数据源")]
    internal class SwitchFieldSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new SwitchFieldSource(this);
        }

        #endregion

        [SimpleAttribute]
        public string Context { get; private set; }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        public IConfigCreator<TableResolver> Resolver { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public SwitchConfig Switch { get; private set; }
    }
}
