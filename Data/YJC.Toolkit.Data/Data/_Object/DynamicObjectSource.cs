using System;
using System.Dynamic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class DynamicObjectSource : ISource
    {
        protected DynamicObjectSource()
        {
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
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
            {
                ExpandoObject bag = new ExpandoObject();
                AddObject(input, bag);
                if (UseCallerInfo)
                    input.CallerInfo.AddInfo(bag);

                return OutputData.CreateObject(bag);
            }
        }

        #endregion ISource 成员

        public bool UseCallerInfo { get; set; }

        protected virtual OutputData DoPost(IInputData input)
        {
            throw new NotSupportedException();
        }

        protected abstract void AddObject(IInputData input, dynamic bag);
    }
}