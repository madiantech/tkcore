using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.User;

namespace YJC.Toolkit.Weixin.Data
{
    [ObjectSource(Author = "YJC", CreateDate = "2014-11-19",
        Description = "编辑微信公众号用户组")]
    [InstancePlugIn, AlwaysCache]
    public class WeGroupEditObjectSource : IInsertObjectSource
    {
        public static object Instance = new WeGroupEditObjectSource();

        #region IObjectInsertSource 成员

        public object CreateNew(IInputData input)
        {
            return new WeGroup() { Name = "Hello" };
        }

        public OutputData Insert(IInputData input, object instance)
        {
            return OutputData.Create("Hello");
        }

        #endregion IObjectInsertSource 成员
    }
}