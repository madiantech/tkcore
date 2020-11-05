using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [ReplyMessageConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-03-16", Description = "注册插件返回的消息")]
    internal class RegMessageConfig : RegFactoryConfig<IRule>
    {
        public RegMessageConfig()
            : base(ReplyMessagePlugInFactory.REG_NAME)
        {
        }
    }
}
