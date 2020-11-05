using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseWebDbSource : BaseDbSource
    {
        protected BaseWebDbSource()
        {
        }

        protected abstract OutputData DoGet(IInputData input);

        protected abstract OutputData DoPost(IInputData input);

        public sealed override OutputData DoAction(IInputData input)
        {
            if (input.IsPost)
            {
                try
                {
                    return DoPost(input);
                }
                catch (WebPostException ex)
                {
                    return OutputData.CreateToolkitObject(ex.CreateErrorResult());
                }
            }
            else
                return DoGet(input);
        }
    }
}
