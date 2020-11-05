using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class SimpleIdCreator
    {
        private const char DEFAULT_PADDINGCHAR = '0';

        public SimpleIdCreator()
        {
            PaddingChar = DEFAULT_PADDINGCHAR;
        }

        [SimpleAttribute]
        public int Length { get; set; }

        [SimpleAttribute(DefaultValue = DEFAULT_PADDINGCHAR)]
        public char PaddingChar { get; set; }

        [SimpleAttribute]
        public PaddingPosition PaddingPosition { get; set; }

        public string CreateId(TkDbContext context, string tableName)
        {
            string id = context.GetUniId(tableName);
            if (Length > 0 && id.Length < Length)
            {
                switch (PaddingPosition)
                {
                    case PaddingPosition.Left:
                        id = id.PadLeft(Length, PaddingChar);
                        break;
                    case PaddingPosition.Right:
                        id = id.PadRight(Length, PaddingChar);
                        break;
                }
            }
            return id;
        }
    }
}
