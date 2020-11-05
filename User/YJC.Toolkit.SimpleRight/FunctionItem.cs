using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    internal class FunctionItem
    {
        private Dictionary<string, SubFuncClass> fSubFunction;
        private HashSet<SubFunctionKey> fSubKeys;

        public FunctionItem(DataRow row)
        {
            Id = row["FN_ID"].Value<int>();
            Key = row["FN_SHORT_NAME"].ToString();
            IsLeaf = row["FN_IS_LEAF"].ToString() == "1";
            Parser = Tk55Parser.Parse(row["FN_URL"].ToString());
        }

        public string Key { get; private set; }

        public bool IsLeaf { get; private set; }

        public int Id { get; private set; }

        public Tk55Parser Parser { get; private set; }

        public void AddSubFunctions(IEnumerable<DataRow> rows)
        {
            fSubKeys = new HashSet<SubFunctionKey>();
            fSubFunction = new Dictionary<string, SubFuncClass>();
            if (Parser != null)
                fSubKeys.Add(new SubFunctionKey(Parser.Style, Parser.Source));
            foreach (DataRow row in rows)
            {
                SubFuncClass subFunc = new SubFuncClass();
                subFunc.ReadFromDataRow(row);
                if (subFunc.NameId == "_Empty")
                    continue;
                var key = subFunc.CreateParser(this);
                if (key != null)
                    fSubKeys.Add(key);
                fSubFunction.Add(subFunc.NameId, subFunc);
            }
        }

        public bool IsSubFunction(SubFunctionKey subKey)
        {
            if (!IsLeaf)
                return false;
            if (fSubKeys == null)
                return false;
            return fSubKeys.Contains(subKey);
        }

        public IEnumerable<string> SubFunctions
        {
            get
            {
                if (fSubKeys == null)
                    return null;

                return fSubFunction.Keys;
            }
        }

        public IEnumerable<OperatorConfig> GetOperators(OperatorPage page)
        {
            if (fSubFunction == null)
                return null;

            var result = from item in fSubFunction.Values
                         where item != null && item.IsFitFor(page)
                         orderby item.OperOrder
                         select item.ConvertToOperatorConfig();
            return result.ToArray();
        }
    }
}