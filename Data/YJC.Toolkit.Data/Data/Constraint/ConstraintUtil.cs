using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public static class ConstraintUtil
    {
        public static IFieldInfo GetFieldInfo(object[] args)
        {
            TkDebug.AssertArgumentNull(args, "args", null);
            //TkDebug.Assert(args.Length == 1, string.Format(ObjectUtil.SysCulture,
            //    "args中的参数只能有一个，现在是{0}个", args.Length), null);
            IFieldInfo result = ObjectUtil.QueryObject<IFieldInfo>(args);

            TkDebug.AssertNotNull(result, "创建参数中缺乏IFieldInfo类型", null);
            return result;
        }

        public static IFieldInfo GetFieldInfo(string nickName, object[] args)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);
            TkDebug.AssertArgumentNull(args, "args", null);

            IFieldInfoIndexer indexer = ObjectUtil.QueryObject<IFieldInfoIndexer>(
                    args);
            TkDebug.AssertNotNull(indexer, "创建参数中缺乏IFieldInfoIndexer类型", null);
            IFieldInfo result = indexer[nickName];
            TkDebug.AssertNotNull(result,
                    string.Format(ObjectUtil.SysCulture, "缺少名称为{0}的字段", nickName), indexer);
            return result;
        }
    }
}