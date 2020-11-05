using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-07-12", Description = "对某表的某字段值进行更新的数据源")]
    internal class ChangeStatusSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new ChangeStatusSource(this);
        }

        #endregion

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleAttribute]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public bool UpdateTrackField { get; set; }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        public IConfigCreator<TableResolver> Resolver { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem Status { get; private set; }
    }
}
