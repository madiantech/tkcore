using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public class BoolColumnReader : BaseColumnReader
    {
        private readonly string fCheckValue;
        private readonly string fUncheckValue;

        public BoolColumnReader(Tk5FieldInfoEx fieldInfo, int cellIndex)
            : base(fieldInfo, cellIndex)
        {
            fCheckValue = fieldInfo.Extension == null ? "1" : fieldInfo.Extension.CheckValue;
            fUncheckValue = fieldInfo.Extension == null ? "0" : fieldInfo.Extension.UnCheckValue;
        }

        protected override string ConvertValue(TkDbContext context, string value)
        {
            if (value == "√")
                return fCheckValue;
            else if (value == "X" || string.IsNullOrEmpty(value))
                return fUncheckValue;

            throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                "{0}不是合法的，选择√或者X", value), this);
        }
    }
}
