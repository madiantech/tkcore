using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class TableSchemeExtension
    {
        internal static string GetCacheKey(this ITableScheme scheme)
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);

            return string.Format(ObjectUtil.SysCulture, "{0}+{1}",
                scheme, scheme.TableName);
        }

        public static string GetSelectFields(this ITableScheme scheme, TkDbContext context)
        {
            TkDebug.AssertArgumentNull(scheme, "scheme", null);

            TableSchemeData data = TableSchemeData.Create(context, scheme);
            return data.SelectFields;
        }

        public static string GetSelectFields(this ITableScheme scheme)
        {
            return GetSelectFields(scheme, null);
        }
    }
}
