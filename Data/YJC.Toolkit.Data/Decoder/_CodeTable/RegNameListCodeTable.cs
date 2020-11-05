using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class RegNameListCodeTable<T> : CodeTable, IEnumerable<IDecoderItem>
        where T : class, IRegName, IDecoderItem
    {
        private readonly RegNameList<T> fList;

        public RegNameListCodeTable(RegNameList<T> list)
        {
            TkDebug.AssertArgumentNull(list, "list", null);

            fList = list;
        }

        #region IEnumerable<IDecoderItem> 成员

        public IEnumerator<IDecoderItem> GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override IDecoderItem Decode(string code, params object[] args)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            return fList[code];
        }

        public override void Fill(params object[] args)
        {
            DataSet ds = ObjectUtil.QueryObject<DataSet>(args);
            if (ds != null)
            {
                string regName = RegName;
                if (!ds.Tables.Contains(regName))
                {
                    var fields = from item in fList
                                 select new CodeItem(item);
                    DataTable table = fields.CreateTable(regName);
                    ds.Tables.Add(table);
                }
                return;
            }

            CodeTableContainer container = ObjectUtil.QueryObject<CodeTableContainer>(args);
            if (container != null)
                container.Add(RegName, fList);
        }
    }
}
