using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Log;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RecordLogConfigItem
    {
        [SimpleAttribute]
        public string TableName { get; private set; }

        [DynamicElement(RecordDataPickerConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit, Required = true)]
        public IConfigCreator<IRecordDataPicker> DataPicker { get; private set; }

        [DynamicElement(LogConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit, Required = true)]
        public IConfigCreator<ILog> LogData { get; private set; }
    }
}