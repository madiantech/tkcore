using YJC.Toolkit.Collections;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class StepConfigCollection : RegNameList<StepConfig>
    {
        protected override void OnAdded(StepConfig item, int index)
        {
            if (item.StepType == StepType.Begin)
                BeginStep = item;
        }

        protected override void OnCleared()
        {
            BeginStep = null;
        }

        protected override void OnRemoved(StepConfig item, int index)
        {
            if (item.StepType == StepType.Begin)
                BeginStep = null;
        }

        public StepConfig BeginStep { get; private set; }
    }
}