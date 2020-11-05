using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [MergerConfig(RegName = "AllFinish", CreateDate = "2018-04-25", Author = "YJC",
        Description = "全部完成的Merger")]
    internal sealed class ChildrenFinishMergerConfig : IConfigCreator<IMerger>
    {
        #region IConfigCreator<IMerger> 成员

        public IMerger CreateObject(params object[] args)
        {
            return new ChildrenFinishMerger();
        }

        #endregion IConfigCreator<IMerger> 成员
    }
}