using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    public class EasySearchColumnReader : BaseColumnReader
    {
        private readonly EasySearch fEasySearch;
 
        public EasySearchColumnReader(Tk5FieldInfoEx fieldInfo, int cellIndex)
            : base(fieldInfo, cellIndex)
        {
            fEasySearch = PlugInFactoryManager.CreateInstance<EasySearch>(
                EasySearchPlugInFactory.REG_NAME, fieldInfo.Decoder.RegName); 
        }

        protected override string ConvertValue(TkDbContext context, string value)
        {
            IDecoderItem[] items = fEasySearch.SearchByName(value, context);
            switch (items.Length)
            {
                case 0:
                    throw new ToolkitException(string.Format(ObjectUtil.SysCulture, 
                        "{0}不存在，请确认", value), this);
                case 1:
                    return items[0].Value;
                default:
                    throw new ToolkitException(string.Format(ObjectUtil.SysCulture,
                        "{0}存在{1}条记录，有二义性，请处理", value, items.Length), this);
            }
        }
    }
}
