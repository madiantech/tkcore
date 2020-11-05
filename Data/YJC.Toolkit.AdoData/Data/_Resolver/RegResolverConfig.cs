using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-22", Description = "根据配置的注册名实例化TableResolver")]
    [ObjectContext]
    internal class RegResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            return PlugInFactoryManager.CreateInstance<TableResolver>(
                ResolverPlugInFactory.REG_NAME, Content, source);
        }

        #endregion

        [TextContent(Required = true)]
        public string Content { get; protected set; }
    }
}
