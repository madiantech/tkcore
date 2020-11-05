using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SysFunction
{
    [MetaDataConfig(CreateDate = "2014-10-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "通过SYS_CUSTOM_TABLE表的定义获取的单表MetaData")]
    [SingleMetaDataConfig(CreateDate = "2014-10-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "通过SYS_CUSTOM_TABLE表的定义获取的单表MetaData")]
    internal class DbSingleCustomTableConfig : ISingleMetaData, IConfigCreator<IMetaData>,
        IConfigCreator<ISingleMetaData>
    {
        public DbSingleCustomTableConfig()
        {
            CommitDetail = false;
            ColumnCount = 3;
            JsonDataType = JsonObjectType.Array;
        }

        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            using (TableSource source = new TableSource(Context))
            {
                ITableSchemeEx scheme = source.CreateTableScheme(TableName);
                if (scheme == null)
                    return null;

                IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);
                switch (input.Style.Style)
                {
                    case PageStyle.Insert:
                    case PageStyle.Update:
                    case PageStyle.Detail:
                        return new Tk5SingleNormalMetaData(scheme, input, this);

                    case PageStyle.List:
                        return new Tk5ListMetaData(scheme, input, this);
                }
            }

            return null;
        }

        #endregion IConfigCreator<IMetaData> 成员

        #region ISingleMetaData 成员

        [SimpleAttribute(DefaultValue = 3)]
        public int ColumnCount { get; private set; }

        public bool CommitDetail { get; private set; }

        public JsonObjectType JsonDataType { get; private set; }

        public Tk5TableScheme CreateTableScheme(ITableSchemeEx scheme, IInputData input)
        {
            return new Tk5TableScheme(scheme, input, this, CreateDataXmlField);
        }

        public ITableSchemeEx CreateSourceScheme(IInputData input)
        {
            using (TableSource source = new TableSource(Context))
            {
                ITableSchemeEx scheme = source.CreateTableScheme(TableName);
                return scheme;
            }
        }

        #endregion ISingleMetaData 成员

        #region IConfigCreator<ISingleMetaData> 成员

        ISingleMetaData IConfigCreator<ISingleMetaData>.CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<ISingleMetaData> 成员

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleAttribute]
        public string TableName { get; private set; }

        public static Tk5FieldInfoEx CreateDataXmlField(ITableSchemeEx scheme,
            IFieldInfoEx field, IInputData input, ISingleMetaData config)
        {
            Tk5FieldInfoEx fieldInfo = new Tk5FieldInfoEx(field, input.Style);
            CustomField custField = field.Convert<CustomField>();
            if (custField.Query)
            {
                if (fieldInfo.ListDetail == null)
                    fieldInfo.ListDetail = new Tk5ListDetailConfig();

                fieldInfo.ListDetail.Search = FieldSearchMethod.True;
            }
            CustomTable dataXml = scheme.Convert<CustomTable>();
            if (input.Style.Style == PageStyle.List)
                if (dataXml.SupportDisplay && field.FieldName == dataXml.Name.FieldName)
                {
                    if (fieldInfo.ListDetail == null)
                        fieldInfo.ListDetail = new Tk5ListDetailConfig();
                    if (fieldInfo.ListDetail.Link == null)
                    {
                        string content = string.Format(ObjectUtil.SysCulture,
                            "~/c/xml/detail/{0}?{1}=*{1}*",
                            input.SourceInfo.Source, dataXml.Id.NickName);
                        fieldInfo.ListDetail.Link = new Tk5LinkConfig(content);
                    }
                }
            return fieldInfo;
        }
    }
}