using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    //[ActiveTimeCache]
    internal sealed class TableSchemeData : ICacheDependencyCreator
    {
        private readonly ICacheDependency fDependency;

        internal TableSchemeData(TkDbContext context, ITableScheme scheme)
        {
            ICacheDependencyCreator creator = scheme as ICacheDependencyCreator;
            if (creator != null)
                fDependency = creator.CreateCacheDependency();
            if (fDependency == null)
                fDependency = (new ActiveTimeCacheAttribute()).CreateObject();
            KeyFieldInfos = new DictionaryList<IFieldInfo>();

            ProcessFields(context, scheme);
        }

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }

        #endregion ICacheDependencyCreator 成员

        public string SelectFields { get; private set; }

        public DictionaryList<IFieldInfo> KeyFieldInfos { get; private set; }

        public IFieldInfo[] KeyFieldArray { get; private set; }

        private static string GetField(TkDbContext context, IFieldInfo item)
        {
            if (item.FieldName == item.NickName)
            {
                if (context != null)
                    return context.EscapeName(item.NickName);
                else
                    return item.NickName;
            }
            else
            {
                string fieldName = item.FieldName;
                string nickName = item.NickName;
                if (context != null)
                {
                    fieldName = context.EscapeName(fieldName);
                    nickName = context.EscapeName(nickName);
                }
                return string.Format(ObjectUtil.SysCulture, "{0} {1}", fieldName, nickName);
            }
        }

        private void ProcessFields(TkDbContext context, ITableScheme scheme)
        {
            foreach (var item in scheme.Fields)
            {
                if (item.IsKey)
                    KeyFieldInfos.Add(item.NickName, item);
            }
            TkDebug.Assert(KeyFieldInfos.Count > 0, string.Format(ObjectUtil.SysCulture,
                "数据表{0}中没有找到主键的字段，请确认主键字段是否遗漏或者写错", scheme.TableName), this);
            KeyFieldArray = KeyFieldInfos.ToArray();
            var fields = from item in scheme.Fields
                         select GetField(context, item);
            SelectFields = string.Join(", ", fields);
        }

        public static TableSchemeData Create(TkDbContext context, ITableScheme scheme)
        {
            return CacheManager.GetItem("TableSchemeData", scheme.GetCacheKey(),
                scheme, context).Convert<TableSchemeData>();
        }
    }
}