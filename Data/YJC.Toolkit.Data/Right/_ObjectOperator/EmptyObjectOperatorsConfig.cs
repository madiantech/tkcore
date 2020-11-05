using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [ObjectOperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-31", Description = "空对象操作符配置")]
    internal class EmptyObjectOperatorsConfig : IConfigCreator<IObjectOperatorsConfig>
    {
        #region IConfigCreator<IObjectOperatorsConfig> 成员

        public IObjectOperatorsConfig CreateObject(params object[] args)
        {
            return null;
        }

        #endregion
    }
}
