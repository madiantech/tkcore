using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model.Message;

namespace YJC.Toolkit.WeCorp.Service
{
    public interface IChatService
    {
        //获取企业微信服务器的ip段
        [ApiMethod("/getcallbackip", ResultKey = "ip_list")]
        List<string> GetCallbackIp();

        //创建群聊会话
        [ApiMethod("/appchat/create", Method = HttpMethod.Post, ResultKey = "chatid")]
        string CreateAppChat([ApiParameter(Location = ParamLocation.Content)]ChatInfo info);

        //修改群聊会话
        [ApiMethod("/appchat/update", Method = HttpMethod.Post)]
        BaseResult UpdateAppChat([ApiParameter(Location = ParamLocation.Content)]ChatDetail detail);

        //获取群聊会话
        [ApiMethod("/appchat/get", ResultKey = "chat_info")]
        ChatInfo GetAppChat([ApiParameter(NamingRule = NamingRule.Lower)]string chatId);
    }
}