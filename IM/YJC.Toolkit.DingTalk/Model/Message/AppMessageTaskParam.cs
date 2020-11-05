using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    internal class AppMessageTaskParam
    {
        public AppMessageTaskParam()
        {
        }

        public AppMessageTaskParam(int agentId, int taskId)
        {
            AgentId = agentId;
            TaskId = taskId;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int TaskId { get; set; }
    }
}