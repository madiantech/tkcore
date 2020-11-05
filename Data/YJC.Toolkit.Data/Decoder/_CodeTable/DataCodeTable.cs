using System.Collections;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public abstract class DataCodeTable : CodeTable, IEnumerable<CodeItem>, ICacheDependencyCreator
    {
        private readonly RegNameList<CodeItem> fData;

        protected DataCodeTable()
        {
            fData = new RegNameList<CodeItem>();
        }

        #region IEnumerable<CodeItem> 成员

        public IEnumerator<CodeItem> GetEnumerator()
        {
            return fData.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return AlwaysDependency.Dependency;
        }

        #endregion

        protected void Add(CodeItem item)
        {
            TkDebug.AssertArgumentNull(item, "item", this);

            fData.Add(item);
        }

        public override IDecoderItem Decode(string code, params object[] args)
        {
            return fData[code];
        }

        public override void Fill(params object[] args)
        {
            DataSet ds = ObjectUtil.QueryObject<DataSet>(args);
            if (ds != null)
            {
                string regName = RegName;
                if (!ds.Tables.Contains(regName))
                {
                    DataTable table = fData.CreateTable(regName);
                    ds.Tables.Add(table);
                }
                return;
            }

            CodeTableContainer container = ObjectUtil.QueryObject<CodeTableContainer>(args);
            if (container != null)
                container.Add(RegName, fData);
        }
    }
}
