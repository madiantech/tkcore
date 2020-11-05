using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ChildTableInfo : IRegName, IDisposable
    {
        private readonly bool fCreateResolver;

        internal ChildTableInfo(IDbDataSource source, ChildTableInfoConfig config)
            : this(config.Resolver.CreateObject(source), new TableRelation(config.Relation))
        {
            Resolver.UpdateMode = config.UpdateMode;
            fCreateResolver = true;
        }

        public ChildTableInfo(TableResolver resolver, TableRelation relation)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(relation, "relation", null);

            Relation = relation;
            Resolver = resolver;
            fCreateResolver = false;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Resolver.TableName;
            }
        }

        #endregion

        public TableRelation Relation { get; private set; }

        public TableResolver Resolver { get; private set; }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (fCreateResolver)
                    Resolver.Dispose();
            }
        }

        public void FillDetailTables(TableResolver masterResolver)
        {
            TkDebug.AssertNotNull(masterResolver, "masterResolver", this);

            Relation.FillDetailTable(masterResolver, Resolver);
        }

        public void SetRelationFieldValue(TableResolver masterResolver)
        {
            TkDebug.AssertNotNull(masterResolver, "masterResolver", this);

            Relation.SetSimpleFieldValue(masterResolver, Resolver);
        }

        public IParamBuilder CreateDetailParamBuilder(TableResolver masterResolver, IInputData input)
        {
            return Relation.CreateDetailParamBuilder(masterResolver, Resolver, input);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "{0}的子表结构", Resolver);
        }
    }
}
