using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class CodeTableList
    {
        private readonly Dictionary<string, CodeTableContainer> fData;

        public CodeTableList()
        {
            fData = new Dictionary<string, CodeTableContainer>();
        }

        public void Add(string regName, CodeTable codeTable)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);
            TkDebug.AssertArgumentNull(codeTable, "codeTable", this);

            if (!fData.ContainsKey(regName))
                fData.Add(regName, new CodeTableContainer(codeTable));
        }

        public CodeTable GetCodeTable(string regName, ControlType ctrlType)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            CodeTableContainer result = ObjectUtil.TryGetValue(fData, regName);
            return result == null ? null : result.GetCodeTable(ctrlType);
        }

        public IEnumerable<CodeTable> CreateEnumerable()
        {
            var result = from item in fData.Values
                         select item.CodeTable;
            return result;
        }

        public IEnumerable<CodeTable> CreateCachedEnumerable()
        {
            var result = from item in fData.Values
                         where IsCached(item.CodeTable)
                         select item.CodeTable;
            return result;
        }

        internal CodeTable GetFilledCodeTable(string regName)
        {
            CodeTableContainer result = ObjectUtil.TryGetValue(fData, regName);
            return result == null ? null : result.CodeTable;
        }

        public bool ContainsKey(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            return fData.ContainsKey(regName);
        }

        public void Clear()
        {
            fData.Clear();
        }

        private static bool IsCached(CodeTable codeTable)
        {
            if (codeTable is ICacheDependencyCreator creator)
            {
                var cache = creator.CreateCacheDependency();
                return cache == AlwaysDependency.Dependency;
            }

            return false;
        }
    }
}