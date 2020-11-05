using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class VoiceConfig
    {
        public VoiceConfig(string mediaId, int duration)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, "mediaId", null);
            TkDebug.AssertArgument(duration >= 0 && duration < 60, "duration",
                string.Format(ObjectUtil.SysCulture, "duration必须在0到60之间，当前值是{0}", duration), null);

            MediaId = mediaId;
            Duration = duration;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MediaId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Duration { get; set; }
    }
}