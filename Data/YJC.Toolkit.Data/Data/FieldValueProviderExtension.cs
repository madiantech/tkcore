using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class FieldValueProviderExtension
    {
        public static string GetValue(this IFieldValueProvider provider, string nickName)
        {
            try
            {
                return provider[nickName].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T GetValue<T>(this IFieldValueProvider provider, string nickName)
        {
            try
            {
                return provider[nickName].Value<T>();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
