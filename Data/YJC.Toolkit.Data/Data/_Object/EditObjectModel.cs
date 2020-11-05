using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class EditObjectModel : BaseObjectModel
    {
        public EditObjectModel()
        {
            CodeTables = new CodeTableContainer();
        }

        public CodeTableContainer CodeTables { get; private set; }

        private static IDecoder GetDecoder(IFieldDecoder decoder)
        {
            BasePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                CodeTablePlugInFactory.REG_NAME);
            CodeTable ct = factory.CreateInstance<CodeTable>(decoder.RegName);
            return ct;
        }

        public void FillCodeTable(IEnumerable<IFieldInfoEx> fields)
        {
            if (fields == null)
                return;

            var decodeFields = from item in fields
                               where item.Decoder != null && item.Decoder.Type == DecoderType.CodeTable
                               select item;

            foreach (var item in decodeFields)
            {
                IDecoder coder = GetDecoder(item.Decoder);
                if (coder != null)
                    coder.Fill(CodeTables);
            }
        }
    }
}
