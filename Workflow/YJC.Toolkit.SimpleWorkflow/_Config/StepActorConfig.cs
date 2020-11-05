using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class StepActorConfig
    {
        private static readonly string[] ACTOR_CONTENT = new string[] { "创建者", "个人",
            "指定组织和部门",  "指定组织和角色", "指定组织和岗位", "相对步骤", "指定角色，组织待定",
            "指定岗位，组织待定", "自定义"};

        public StepActorConfig()
        {
            ActorType = ActorType.Creator;
            RelationType = RelationType.SamePerson;
            Calculation = CalculationType.Expression;
        }

        [SimpleAttribute(DefaultValue = ActorType.Creator)]
        public ActorType ActorType { get; internal set; }

        [SimpleAttribute]
        public string RelativeStepName { get; internal set; }

        [SimpleAttribute(DefaultValue = RelativeUserType.Receiver)]
        public RelativeUserType RelativeUserType { get; internal set; }

        [SimpleAttribute(DefaultValue = RelationType.SamePerson)]
        public RelationType RelationType { get; internal set; }

        [SimpleAttribute(DefaultValue = CalculationType.Expression)]
        public CalculationType Calculation { get; internal set; }

        [SimpleAttribute]
        public string Expression { get; internal set; }

        [SimpleAttribute]
        public string ActorInfo1 { get; internal set; }

        [SimpleAttribute]
        public string ActorInfo2 { get; internal set; }

        public override string ToString()
        {
            return ACTOR_CONTENT[(int)ActorType];
        }
    }
}