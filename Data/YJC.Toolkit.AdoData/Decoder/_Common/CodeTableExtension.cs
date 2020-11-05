using System.Data;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public static class CodeTableExtension
    {
        public static RegNameList<ListTabSheet> CreateTabSheets(this CodeTable codeTable,
            TkDbContext context, IFieldInfo field)
        {
            DataSet ds = new DataSet() { Locale = ObjectUtil.SysCulture };
            using (ds)
            {
                codeTable.Fill(ds, context);
                if (ds.Tables.Count == 0)
                    return null;
                DataTable table = ds.Tables[0];
                if (table.Rows.Count == 0)
                    return null;

                RegNameList<ListTabSheet> result = new RegNameList<ListTabSheet>();
                foreach (DataRow row in table.Rows)
                {
                    object value = row[DecoderConst.CODE_NICK_NAME];
                    IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context, field,
                        value);
                    ListTabSheet tab = new ListTabSheet(value.ToString(),
                        row[DecoderConst.NAME_NICK_NAME].ToString(), builder);
                    result.Add(tab);
                }

                return result;
            }
        }

        public static void Decode(this TableResolver resolver, IPageStyle style)
        {
            MetaDataTableResolver metaResolver = resolver as MetaDataTableResolver;
            if (metaResolver != null)
                metaResolver.Decode(style);
        }

        public static void FillCodeTable(this TableResolver resolver, IPageStyle style)
        {
            MetaDataTableResolver metaResolver = resolver as MetaDataTableResolver;
            if (metaResolver != null)
                metaResolver.FillCodeTable(style);
        }
    }
}
