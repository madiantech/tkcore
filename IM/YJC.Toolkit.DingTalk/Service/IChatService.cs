using YJC.Toolkit.DingTalk.Model.Message;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IChatService
    {
        //查询群消息已读人员列表
        [ApiMethod("/chat/getReadList")]
        ChatReadUserList GetReadList([ApiParameter]string messageId, [ApiParameter]long cursor = 0,
            [ApiParameter]int size = 100);

        //创建会话
        [ApiMethod("/chat/create", Method = HttpMethod.Post)]
        ChatIdInfo CreateChat([ApiParameter(Location = ParamLocation.Content)]ChatCreatedInfo createSession);

        //修改会话
        [ApiMethod("/chat/update", Method = HttpMethod.Post)]
        BaseResult UpdateChat([ApiParameter(Location = ParamLocation.Content)]ChatUpdatedInfo modifySession);

        //获取会话
        [ApiMethod("/chat/get", ResultKey = "chat_info")]
        ChatInfo GetChat([ApiParameter(NamingRule = NamingRule.Lower)]string chatId);
    }
}