using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Excel
{
    public class NormalColumnReader : BaseColumnReader
    {
        public NormalColumnReader(Tk5FieldInfoEx fieldInfo, int cellIndex)
            : base(fieldInfo, cellIndex)
        {
        }

        protected override string ConvertValue(TkDbContext context, string value)
        {
            return value;
        }
    }
}
