using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2015-03-25", Description = "标准代码表的单表MetaData")]
    [SingleMetaDataConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2015-03-25", Description = "标准代码表的单表MetaData")]
    [ObjectContext]
    internal class StdCodeTableMetaDataConfig : BaseSingleMetaDataConfig, IConfigCreator<IMetaData>,
        IConfigCreator<ISingleMetaData>
    {
        private string fQueryStringName;

        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            ITableSchemeEx scheme = CreateSourceScheme(input);
            switch (input.Style.Style)
            {
                case PageStyle.Insert:
                case PageStyle.Update:
                case PageStyle.Detail:
                    return new Tk5SingleNormalMetaData(scheme, input, this);
                case PageStyle.List:
                    return new Tk5ListMetaData(scheme, input, this);
            }
            return null;
        }

        #endregion

        #region IConfigCreator<ISingleMetaData> 成员

        ISingleMetaData IConfigCreator<ISingleMetaData>.CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute]
        public bool ShowCodeValue { get; private set; }

        [SimpleAttribute]
        public bool ShowSort { get; private set; }

        [SimpleAttribute]
        public bool ShowPy { get; private set; }

        [SimpleAttribute]
        public string PyCaption { get; private set; }

        [SimpleAttribute]
        public bool UseQueryString { get; private set; }

        public override ITableSchemeEx CreateSourceScheme(IInputData input)
        {
            string tableName = UseQueryString ? input.QueryString[fQueryStringName] : TableName;
            return new StdCodeTableScheme(tableName, ShowCodeValue, ShowSort, ShowPy, PyCaption);
        }

        public override Tk5TableScheme CreateTableScheme(ITableSchemeEx scheme, IInputData input)
        {
            return new Tk5TableScheme(scheme, input, this, CreateDataXmlField);
        }

        public static Tk5FieldInfoEx CreateDataXmlField(ITableSchemeEx scheme,
            IFieldInfoEx field, IInputData input, BaseSingleMetaDataConfig config)
        {
            Tk5FieldInfoEx fieldInfo = new Tk5FieldInfoEx(field, input.Style);
            return fieldInfo;
        }

        public override void OnReadObject()
        {
            ColumnCount = 1;
            if (UseQueryString)
            {
                fQueryStringName = TableName;
                TableName = string.Empty;
            }
        }
    }
}
