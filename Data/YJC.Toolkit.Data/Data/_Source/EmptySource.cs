using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class EmptySource : ISource
    {
        public EmptySource()
            : this(false)
        {
        }

        public EmptySource(bool useCallerInfo)
        {
            UseCallerInfo = useCallerInfo;
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            if (UseCallerInfo)
            {
                DataSet ds = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
                input.CallerInfo.AddInfo(ds);
                return OutputData.Create(ds);
            }
            else
                return OutputData.Create(string.Empty);
        }

        #endregion

        public bool UseCallerInfo { get; set; }
    }
}
