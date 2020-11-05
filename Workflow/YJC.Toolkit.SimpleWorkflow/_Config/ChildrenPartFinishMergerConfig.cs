using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [MergerConfig(RegName = "PartFinish", CreateDate = "2018-04-25", Author = "YJC",
        Description = "部分完成的Merger")]
    internal sealed class ChildrenPartFinishMergerConfig : IConfigCreator<IMerger>
    {
        [SimpleAttribute(Required = true)]
        public PartType Type { get; internal set; }

        [SimpleAttribute(Required = true)]
        public double Number { get; internal set; }

        #region IConfigCreator<IMerger> 成员

        public IMerger CreateObject(params object[] args)
        {
            return new ChildrenPartFinishMerger(Type, Number);
        }

        #endregion IConfigCreator<IMerger> 成员
    }
}