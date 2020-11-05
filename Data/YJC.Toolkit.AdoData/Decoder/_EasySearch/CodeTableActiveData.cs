using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Decoder
{
    class CodeTableActiveData : IActiveData
    {
        public static IActiveData Instance = new CodeTableActiveData();

        private CodeTableActiveData()
        {
        }

        #region IActiveData 成员

        public IParamBuilder CreateParamBuilder(TkDbContext context, IFieldInfoIndexer indexer)
        {
            return SqlParamBuilder.CreateSql("CODE_DEL IS NULL OR CODE_DEL <> 1");
        }

        #endregion
    }
}
