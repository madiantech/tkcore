using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class BackConfig
    {
        [SimpleAttribute]
        public string BackStepName { get; internal set; }

        [SimpleAttribute]
        public string PlugRegName { get; internal set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(BackStepName) && !string.IsNullOrEmpty(PlugRegName))
                return string.Format(ObjectUtil.SysCulture, "{0}, {1}", BackStepName, PlugRegName);
            else if (!string.IsNullOrEmpty(BackStepName))
                return BackStepName;
            else if (!string.IsNullOrEmpty(PlugRegName))
                return PlugRegName;
            else
                return "无设置";
        }
    }
}