using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public static class IMConst
    {
        public static readonly Uri BASE_DINGTALK_URL = new Uri("https://oapi.dingtalk.com");
        public static readonly Uri BASE_WECORP_URL = new Uri("https://qyapi.weixin.qq.com/cgi-bin");
        public static readonly Uri BASE_WECHAT_URL = new Uri("https://api.weixin.qq.com/cgi-bin");

        public const string ACCESS_TOKEN_NAME = "access_token";

        public const string WECORP_MENU = "_MENU";
        public const string WECORP_USER_MANAGER = "_USER_MANAGER";
        public const string WECORP_CHAT = "_CHAT";
    }
}