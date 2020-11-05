using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class ConnectionConfig
    {
        [SimpleAttribute]
        public string Name { get; internal set; }

        [SimpleAttribute]
        public string DisplayName { get; internal set; }

        [SimpleAttribute(DefaultValue = CalculationType.Expression)]
        public CalculationType ExpressionType { get; internal set; }

        [SimpleElement(UseCData = true)]
        public string Expression { get; internal set; }

        [SimpleAttribute]
        public string NextStepName { get; internal set; }
    }
}