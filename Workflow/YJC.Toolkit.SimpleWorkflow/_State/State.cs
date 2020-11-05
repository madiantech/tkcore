using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal abstract class State
    {
        protected State()
        {
        }

        public abstract StepState StepState { get; }

        public abstract bool Execute(Workflow workflow);

        protected void ThrowInvalidState()
        {
            throw new InvalidStateException(this);
        }

        protected T GetStep<T>(Step step) where T : Step
        {
            TkDebug.AssertArgumentNull(step, "step", this);

            T result = step as T;
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "当前的Step类型必须{0}，而现在却是{1}", typeof(T), step.GetType()), this);
            return result;
        }
    }
}