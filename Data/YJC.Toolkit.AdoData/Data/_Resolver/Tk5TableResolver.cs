using System.Linq;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class Tk5TableResolver : MetaDataTableResolver
    {
        public Tk5TableResolver(string fileName, IDbDataSource source)
            : this(Tk5DataXml.Create(fileName), source)
        {
        }

        public Tk5TableResolver(string fileName, string tableName, IDbDataSource source)
            : this(Tk5DataXml.Create(fileName, tableName), source)
        {
            SetFakeDeleteInfo();
        }

        public Tk5TableResolver(Tk5DataXml dataXml, IDbDataSource source)
            : base(dataXml, source)
        {
            FakeDelete = dataXml.FakeDeleteInfo;
        }

        private void SetFakeDeleteInfo()
        {
            Tk5DataXml dataXml = CurrentSchemeEx.Convert<Tk5DataXml>();
            FakeDelete = dataXml.FakeDeleteInfo;
        }

        private static bool IsQueryField(ITk5FieldInfo tk5Field)
        {
            return tk5Field != null && tk5Field.ListDetail != null
                && tk5Field.ListDetail.Search != FieldSearchMethod.False
                && tk5Field.Decoder != null && tk5Field.Decoder.Type == DecoderType.CodeTable;
        }

        public override void FillCodeTable(IPageStyle style)
        {
            TkDebug.AssertArgumentNull(style, "style", this);

            if (style.Style == PageStyle.List)
            {
                var queryCodeTables = (from field in CurrentSchemeEx.Fields
                                       let tk5Field = field as ITk5FieldInfo
                                       where IsQueryField(tk5Field)
                                       select tk5Field.Decoder.RegName).Distinct().ToArray();
                if (queryCodeTables.Length == 0)
                    return;

                InitializeCodeTable(style);
                foreach (string regName in queryCodeTables)
                {
                    CodeTable code = CodeTables.GetFilledCodeTable(regName);
                    if (code != null)
                        code.Fill(HostDataSet, Context);
                }
            }
            else
                base.FillCodeTable(style);
        }


        protected override IUploadProcessor2 CreateUploadProcessor(IFieldInfoEx info, UpdateKind status)
        {
            Tk5UploadConfig upload = info.AssertUpload();

            return upload.CreateUploadProcessor2();
        }
    }
}
