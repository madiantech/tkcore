using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-10", Description = "配置一段可能带有宏文本作为数据源")]
    internal class MarcoSourceConfig : MarcoConfigItem, IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new StaticSource(this);
        }

        #endregion
    }
}
