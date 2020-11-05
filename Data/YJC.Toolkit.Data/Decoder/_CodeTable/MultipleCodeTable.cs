using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public sealed class MultipleCodeTable : CodeTable
    {
        private readonly CodeTable fCodeTable;
        public MultipleCodeTable(CodeTable codeTable)
        {
            TkDebug.AssertArgumentNull(codeTable, "codeTable", null);
            fCodeTable = codeTable;
        }

        public override IDecoderItem Decode(string code, params object[] args)
        {
            MultipleDecodeItem item = new MultipleDecodeItem(code, fCodeTable, args);
            return item;
        }

        public override void Fill(params object[] args)
        {
            fCodeTable.Fill(args);
        }
    }
}
