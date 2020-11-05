using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Decoder
{
    public static class DecoderConst
    {
        public const string CODE_FIELD_NAME = "CODE_VALUE";

        public const string NAME_FIELD_NAME = "CODE_NAME";

        public const string CODE_NICK_NAME = "Value";

        public const string NAME_NICK_NAME = "Name";

        public readonly static string PY_FIELD_NAME = "CODE_PY";

        public readonly static string SORT_FIELD_NAME = "CODE_SORT";

        public readonly static string DEL_FIELD_NAME = "CODE_DEL";

        public readonly static string REF_TABLE_NAME = "REF";

        public readonly static string PARENT_FIELD_NAME = "CODE_PARENT";

        public readonly static string LEAF_FIELD_NAME = "CODE_IS_LEAF";

        public readonly static string INFO_FIELD_NAME = "CODE_INFO";

        public readonly static string DEFAULT_ORDER = "CODE_SORT, CODE_VALUE";

        public readonly static FieldItem CODE_FIELD = new KeyFieldItem(CODE_FIELD_NAME);

        public readonly static FieldItem NAME_FIELD = new FieldItem(NAME_FIELD_NAME);

        public readonly static FieldItem PY_FIELD = new FieldItem(PY_FIELD_NAME);

        public readonly static FieldItem SORT_FIELD = new FieldItem(SORT_FIELD_NAME);

        public readonly static FieldItem ACTIVE_FIELD = new FieldItem(DEL_FIELD_NAME);

        public const string DECODER_TAG = "~~|";

        //public readonly static ActiveFieldItem ACTIVE_FIELD = CreateCodeActiveField();

        //public readonly static FieldItem PARENT_FIELD = new FieldItem(PARENT_FIELD_NAME);

        //public readonly static FieldItem IS_LEAF_FIELD = new FieldItem(LEAF_FIELD_NAME, XmlDataType.Short);

        //private static ActiveFieldItem CreateCodeActiveField()
        //{
        //    ActiveFieldItem field = new ActiveFieldItem(DEL_FIELD_NAME, XmlDataType.Short)
        //    {
        //        UseActive = false
        //    };
        //    return field;
        //}
    }
}
