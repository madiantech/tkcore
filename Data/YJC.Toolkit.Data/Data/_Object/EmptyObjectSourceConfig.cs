using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-08-25", Description = "空对象数据源")]
    internal class EmptyObjectSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new EmptyObjectSource { UseCallerInfo = UseCallerInfo };
        }

        #endregion

        [SimpleAttribute]
        public bool UseCallerInfo { get; private set; }
    }
}
