using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Sys
{
    public static class TenantUtil
    {
        public const string TENANT_NAME = "TenantId";
        public readonly static string TENANT_FIELD_NAME = "CODE_TENANT_ID";
        public readonly static FieldItem TENANT_FIELD = new FieldItem(TENANT_FIELD_NAME, TkDataType.Int);

        internal static IFieldInfo GetTenantId(IFieldInfoIndexer indexer, string nickName)
        {
            if (string.IsNullOrEmpty(nickName))
                nickName = TENANT_NAME;

            IFieldInfo result = indexer[TENANT_NAME];
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "当前表没有昵称为{0}的字段，请确认", nickName), indexer);
            return result;
        }

        internal static Tk5DataXml GetTk5DataXml(IConfigCreator<ITableSchemeEx> schemeCreator)
        {
            ITableSchemeEx schemeEx = schemeCreator.CreateObject();
            Tk5DataXml dataXml = schemeEx as Tk5DataXml;
            TkDebug.AssertNotNull(dataXml, string.Format(ObjectUtil.SysCulture,
                "模型需要Tk5DataXml，当前的Scheme是{0}，不适配", schemeEx.GetType()), schemeCreator);
            return dataXml;
        }

        public static IParamBuilder GetTenantParamBuilder(TkDbContext context, IFieldInfo field)
        {
            return SqlParamBuilder.CreateEqualSql(context, field,
                BaseGlobalVariable.Current.UserInfo.GetTenantValue(true));
        }
    }
}