using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class FakeDeleteInfo : IActiveData
    {
        private IParamBuilder fSelectBuilder;

        private FakeDeleteInfo()
        {
        }

        public FakeDeleteInfo(string fieldName, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", null);
            TkDebug.AssertArgumentNull(value, "value", null);

            FieldName = fieldName;
            Value = value;
            AllowNull = true;
        }

        #region IActiveData 成员

        public IParamBuilder CreateParamBuilder(TkDbContext context, IFieldInfoIndexer indexer)
        {
            if (fSelectBuilder == null)
            {
                TkDebug.AssertArgumentNull(context, "context", this);
                TkDebug.AssertArgumentNull(indexer, "indexer", this);

                IFieldInfo fakeInfo = indexer[FieldName];
                TkDebug.AssertNotNull(fakeInfo, string.Format(ObjectUtil.SysCulture,
                    "{0}中没有配置字段{1}", indexer, FieldName), this);
                fSelectBuilder = ParamBuilder.InternalCreateSingleSql(context,
                    fakeInfo.FieldName, fakeInfo.DataType, "!=", fakeInfo.FieldName, Value);
                if (AllowNull)
                    fSelectBuilder = ParamBuilder.CreateParamBuilderWithOr(fSelectBuilder,
                        ParamBuilder.CreateSql(fakeInfo.FieldName + " IS NULL"));
            }
            return fSelectBuilder;
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public string FieldName { get; private set; }

        [SimpleAttribute(Required = true)]
        public string Value { get; private set; }

        [SimpleAttribute]
        public bool AllowNull { get; set; }
    }
}
