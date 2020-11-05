using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public class CodeTableColumnReader : BaseColumnReader
    {
        private readonly Dictionary<string, IDecoderItem> fCodeTable;

        public CodeTableColumnReader(Tk5FieldInfoEx fieldInfo, TkDbContext context, int cellIndex)
            : base(fieldInfo, cellIndex)
        {
            CodeTableContainer ctContainer = new CodeTableContainer();
            string regName = fieldInfo.Decoder.RegName;
            CodeTable codeTable = PlugInFactoryManager.CreateInstance<CodeTable>(
                CodeTablePlugInFactory.REG_NAME, regName);
            codeTable.Fill(ctContainer, context);
            IEnumerable<IDecoderItem> data = ctContainer[regName];
            fCodeTable = new Dictionary<string, IDecoderItem>();
            foreach (var item in data)
                fCodeTable[item.Name] = item;
        }

        protected override string ConvertValue(TkDbContext context, string value)
        {
            if (fCodeTable.ContainsKey(value))
                return fCodeTable[value].Value;
            throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                "{0}不是一个有效的代码，请从模板的下拉框选择", value), this);
        }
    }
}
