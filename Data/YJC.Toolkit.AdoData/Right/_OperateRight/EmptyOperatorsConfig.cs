using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-31", Description = "空操作符配置")]
    internal class EmptyOperatorsConfig : IConfigCreator<IOperatorsConfig>
    {
        #region IConfigCreator<IOperatorsConfig> 成员

        public IOperatorsConfig CreateObject(params object[] args)
        {
            return null;
        }

        #endregion
    }
}
