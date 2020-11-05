using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    public sealed class ReplyMessagePlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_weixin_ReplyMessage";
        private const string DESCRIPTION = "微信回复消息的插件工厂";

        public ReplyMessagePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}