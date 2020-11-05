using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class DbContextConfigConverter : BaseTypeConverter<DbContextConfig>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            ISupportDbContext support = BaseAppSetting.Current as ISupportDbContext;
            if (support == null)
                return null;

            if (string.IsNullOrEmpty(text))
                return support.Default;
            return support.GetContextConfig(text);
        }
    }
}
