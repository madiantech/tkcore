using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin
{
    public static class WeConst
    {
        public static readonly QName ROOT = QName.Get("xml");

        internal readonly static WriteSettings WRITE_SETTINGS = new WriteSettings
        {
            OmitHead = true,
            Encoding = new UTF8Encoding(false)
        };

        public const string USER_MODE = "WeUser";

        public const string XML_MODE = "WeXml";

        internal const string JS_MODE = "JS_SDK";

        public const int MAX_IMAGE_SIZE = 1 * 1024 * 1024;

        public const int MAX_VOICE_SIZE = 2 * 1024 * 1024;

        public const int MAX_VIDEO_SIZE = 10 * 1024 * 1024;

        public const int MAX_THUMB_SIZE = 64 * 1024;

        public const string AUTH_URL =
            "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect";
    }
}