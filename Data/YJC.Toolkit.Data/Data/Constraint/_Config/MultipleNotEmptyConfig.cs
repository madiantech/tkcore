using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    [ConstraintConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-03",
       Author = "YJC", Description = "几个字段不能同时为空校验")]
    [ObjectContext]
    internal class MultipleNotEmptyConfig : IConfigCreator<BaseConstraint>
    {
        #region IConfigCreator<BaseConstraint> 成员

        BaseConstraint IConfigCreator<BaseConstraint>.CreateObject(params object[] args)
        {
            IFieldInfo[] otherFields;
            if (OtherFields == null)
                otherFields = null;
            else
            {
                IFieldInfoIndexer indexer = ObjectUtil.QueryObject<IFieldInfoIndexer>();
                otherFields = (from item in OtherFields
                               let field = indexer[item]
                               where field != null
                               select field).ToArray();
            }
            return new MultipleNotEmptyConstraint(ConstraintUtil.GetFieldInfo(args), otherFields);
        }

        #endregion

        [SimpleAttribute]
        public string[] OtherFields { get; private set; }
    }
}
