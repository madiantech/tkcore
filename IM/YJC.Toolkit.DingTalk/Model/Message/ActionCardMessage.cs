using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ActionCardMessage : BaseMessage
    {
        public ActionCardMessage(ActionCardConfig actionCard)
            : base(MessageType.ActionCard)
        {
            TkDebug.AssertArgumentNull(actionCard, "actionCard", null);

            ActionCard = actionCard;
        }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower)]
        public ActionCardConfig ActionCard { get; set; }
    }
}