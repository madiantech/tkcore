using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ManyToManyRelation
    {
        internal ManyToManyRelation()
        {
        }

        public ManyToManyRelation(string tableName, string masterField, string detailField)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNullOrEmpty(masterField, "masterField", null);
            TkDebug.AssertArgumentNullOrEmpty(detailField, "detailField", null);

            TableName = tableName;
            MasterField = masterField;
            DetailField = detailField;
        }

        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string MasterField { get; private set; }

        [SimpleAttribute]
        public string DetailField { get; private set; }

        public string GetSubSelectSql(object masterValue)
        {
            TkDebug.AssertArgumentNull(masterValue, "masterValue", this);

            return string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1} WHERE {2} = '{3}'",
                DetailField, TableName, MasterField, masterValue);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "多对多表({0})的结构", TableName);
        }
    }
}
