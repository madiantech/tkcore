using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    class CodeTableContainer
    {
        private readonly CodeTable fCodeTable;
        private MultipleCodeTable fMultiCodeTable;

        public CodeTableContainer(CodeTable codeTable)
        {
            fCodeTable = codeTable;
        }

        public CodeTable CodeTable
        {
            get
            {
                return fCodeTable;
            }
        }

        public CodeTable GetCodeTable(ControlType ctrlType)
        {
            if (ctrlType == ControlType.CheckBoxList)
            {
                if (fMultiCodeTable == null)
                    fMultiCodeTable = new MultipleCodeTable(fCodeTable);
                return fMultiCodeTable;
            }
            else
                return fCodeTable;
        }
    }
}
