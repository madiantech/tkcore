using YJC.Toolkit.IM;
using YJC.Toolkit.WeCorp.Model.Message;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IMessageService
    {
        //接口定义
        [ApiMethod("/message/send", Method = HttpMethod.Post)]
        SendMessageResult SendMessage([ApiParameter(Location = ParamLocation.Content)]
            BaseCorpMessage msg);

        //发送群消息
        [ApiMethod("/appchat/send", Method = HttpMethod.Post)]
        BaseResult AppChatSendMessage([ApiParameter(Location = ParamLocation.Content)]
            BaseChatMessage msg);

        //互联企业消息推送 发送应用消息 接口定义
        [ApiMethod("/linkedcorp/message/send", Method = HttpMethod.Post)]
        SendMessageResult SendLinkedCorpMsg([ApiParameter(Location = ParamLocation.Content)]
            BaseCorpMessage msg);
    }
}