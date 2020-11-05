using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class WebAgentUtil
    {
        private static readonly string[] mobileAgents = new string[] { "iphone",
            "android", "phone", "mobile", "wap", "netfront", "java",
            "opera mobi", "opera mini", "ucweb", "windows ce", "symbian",
            "series", "webos", "sony", "blackberry", "dopod", "nokia",
            "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc",
            "motorola", "Googlebot-Mobile" };

        private static readonly string[] padAgents = new string[] { "pad" };

        public static DeviceType JudgeClient(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                userAgent = userAgent.ToLower();
                foreach (var mobile in mobileAgents)
                {
                    if (userAgent.IndexOf(mobile) >= 0)
                    {
                        foreach (string pad in padAgents)
                        {// 手机端基础上，有pad关键字,设定为pad
                            if (userAgent.IndexOf(pad) >= 0)
                                return DeviceType.Pad;
                        }
                        return DeviceType.Mobile;
                    }
                }
            }
            return DeviceType.PC;
        }

        public static bool IsMobile(string agent)
        {
            return JudgeClient(agent) == DeviceType.Mobile;
        }
    }
}