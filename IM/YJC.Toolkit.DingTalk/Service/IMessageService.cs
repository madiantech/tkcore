using YJC.Toolkit.DingTalk.Model.Message;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IMessageService
    {
        //发送普通消息*cid   返回结果为string数组*
        [ApiMethod("/message/send_to_conversation", Method = HttpMethod.Post, ResultKey = "receiver",
            PostType = typeof(NormalMessageParam))]
        string SendMessage([ApiParameter(Location = ParamLocation.Partial)]string sender,
            [ApiParameter(Location = ParamLocation.Partial)]string cid,
            [ApiParameter(Location = ParamLocation.Partial)]BaseMessage msg);

        //发送群消息
        [ApiMethod("/chat/send", Method = HttpMethod.Post, ResultKey = "messageId",
            PostType = typeof(ChatMessageParam))]
        string SendChatMessage([ApiParameter(Location = ParamLocation.Partial)]string chatId,
            [ApiParameter(Location = ParamLocation.Partial)]BaseMessage msg);

        //发送工作通知消息*  微应用agent_id
        [ApiMethod("/topapi/message/corpconversation/asyncsend_v2", Method = HttpMethod.Post,
            PostType = typeof(AppMessageParam), ResultKey = "task_id")]
        long SendAppMessage([ApiParameter(Location = ParamLocation.Partial)]int agentId,
            [ApiParameter(Location = ParamLocation.Partial)]string userIdList,
            [ApiParameter(Location = ParamLocation.Partial)]string deptIdList,
            [ApiParameter(Location = ParamLocation.Partial)]bool toAllUser,
            [ApiParameter(Location = ParamLocation.Partial)]BaseMessage msg);

        //查询工作通知消息的发送进度
        [ApiMethod("/topapi/message/corpconversation/getsendprogress", Method = HttpMethod.Post,
            ResultKey = "progress", PostType = typeof(AppMessageTaskParam))]
        AppMessageProgress GetProgress([ApiParameter(Location = ParamLocation.Partial)]int agentId,
            [ApiParameter(Location = ParamLocation.Partial)]int taskId);

        //查询工作通知消息的发送结果
        [ApiMethod("/topapi/message/corpconversation/getsendresult", Method = HttpMethod.Post,
            ResultKey = "send_result", PostType = typeof(AppMessageTaskParam))]
        AppMessageResult GetSendResult([ApiParameter(Location = ParamLocation.Partial)]int agentId,
            [ApiParameter(Location = ParamLocation.Partial)]int taskId);
    }
}