using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class VoiceMessage : BaseMessage
    {
        public VoiceMessage(string mediaId, int duration)
            : this(new VoiceConfig(mediaId, duration))
        {
        }

        public VoiceMessage(VoiceConfig voice)
            : base(MessageType.Voice)
        {
            TkDebug.AssertArgumentNull(voice, "voice", null);

            Voice = voice;
        }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public VoiceConfig Voice { get; set; }
    }
}