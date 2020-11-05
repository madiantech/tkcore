using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public sealed class StdCodeTableResolver : MetaDataTableResolver
    {
        private readonly SimpleIdCreator fCreator;

        public StdCodeTableResolver(string tableName, IDbDataSource source)
            : base(new StdCodeTableScheme(tableName, false, true, false, null), source)
        {
            fCreator = new SimpleIdCreator();
            AutoCreatekey = true;
            //FakeDelete = new FakeDeleteInfo("Del", "1");
        }

        internal StdCodeTableResolver(string tableName, SimpleIdCreator creator, IDbDataSource source)
            : base(new StdCodeTableScheme(tableName, false, true, false, null), source)
        {
            //FakeDelete = new FakeDeleteInfo("Del", "1");
            fCreator = creator;
            if (fCreator == null)
            {
                AutoCreatekey = false;
                fCreator = new SimpleIdCreator();
            }
            else
                AutoCreatekey = true;
        }

        protected override void OnUpdatedRow(UpdatingEventArgs e)
        {
            base.OnUpdatedRow(e);

            if (e.Status == UpdateKind.Insert)
            {
                if (AutoCreatekey)
                    e.Row[DecoderConst.CODE_NICK_NAME] = fCreator.CreateId(Context, TableName);
                if (string.IsNullOrEmpty(e.Row["Py"].ToString()))
                    e.Row["Py"] = PinYinUtil.GetPyHeader(
                        e.Row[DecoderConst.NAME_NICK_NAME].ToString(), string.Empty);
            }
        }

        public bool AutoCreatekey { get; set; }

        public char PaddingChar
        {
            get
            {
                return fCreator.PaddingChar;
            }
            set
            {
                fCreator.PaddingChar = value;
            }
        }

        public int Length
        {
            get
            {
                return fCreator.Length;
            }
            set
            {
                fCreator.Length = value;
            }
        }

        public PaddingPosition PaddingPosition
        {
            get
            {
                return fCreator.PaddingPosition;
            }
            set
            {
                fCreator.PaddingPosition = value;
            }
        }
    }
}
