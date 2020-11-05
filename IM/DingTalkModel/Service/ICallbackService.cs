using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface ICallbackService
    {
        //注册业务事件回调接口
        [ApiMethod("/call_back/register_call_back", Method = HttpMethod.Post)]
        BaseResult RegisterCallback(
            [ApiParameter(Location = ParamLocation.Content)]CallbackDetail param);

        //查询事件回调接口
        [ApiMethod("/call_back/get_call_back")]
        CallbackDetail GetCallback();

        //更新事件回调接口
        [ApiMethod("/call_back/update_call_back", Method = HttpMethod.Post)]
        BaseResult UpdateCallback(
            [ApiParameter(Location = ParamLocation.Content)]CallbackDetail param);

        //删除事件回调接口
        [ApiMethod("/call_back/delete_call_back")]
        BaseResult DeleteCallback();

        //获取回调失败的结果
        [ApiMethod("/call_back/get_call_back_failed_result")]
        PageResult<FailedInfo> GetCallbackFailedResult();
    }
}