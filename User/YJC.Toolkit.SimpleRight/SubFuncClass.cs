using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    public class SubFuncClass
    {
        [SimpleElement(Order = 10)]
        public int Id { get; private set; }

        [SimpleElement(Order = 20)]
        public int FnId { get; private set; }

        [SimpleElement(Order = 30)]
        public string NameId { get; private set; }

        [SimpleElement(Order = 40)]
        public string Name { get; private set; }

        [SimpleElement(Order = 50)]
        public OperatorPosition Position { get; private set; }

        [SimpleElement(Order = 60)]
        public string Icon { get; private set; }

        [SimpleElement(Order = 70)]
        public short UseKey { get; private set; }

        [SimpleElement(Order = 80)]
        public string Content { get; private set; }

        [SimpleElement(Order = 90)]
        public short UseMarco { get; private set; }

        [SimpleElement(Order = 100)]
        public string ConfirmData { get; private set; }

        [SimpleElement(Order = 110)]
        public string DialogTitle { get; private set; }

        [SimpleElement(Order = 120)]
        public string Info { get; private set; }

        [SimpleElement(Order = 125)]
        public int OperOrder { get; private set; }

        [SimpleElement(Order = 130)]
        public OperatorPage Page { get; private set; }

        internal Tk55Parser Parser { get; private set; }

        internal SubFunctionKey CreateParser(FunctionItem parent)
        {
            Parser = Tk55Parser.Create(parent, this);
            if (Parser != null)
                return new SubFunctionKey(Parser.Style, Parser.Source);
            return null;
        }

        private static MarcoConfigItem CreateMarco(bool useMarco, string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return new MarcoConfigItem(useMarco, true, value);
        }

        public OperatorConfig ConvertToOperatorConfig()
        {
            OperatorConfig config = new OperatorConfig(NameId, Name, Position, Info, ConfirmData,
                Icon, CreateMarco(UseMarco == 1, Content))
            {
                DialogTitle = CreateMarco(false, DialogTitle),
                UseKey = UseKey == 1
            };
            return config;
        }

        public bool IsFitFor(OperatorPage page)
        {
            return (page & Page) == page;
        }
    }
}