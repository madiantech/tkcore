using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    class BaseXmlMetaDataConfig : BaseSingleMetaDataConfig
    {
        [SimpleAttribute(Required = true)]
        public string DataXml { get; protected set; }

        public override ITableSchemeEx CreateSourceScheme(IInputData input)
        {
            Tk5DataXml dataXml;
            if (string.IsNullOrEmpty(TableName))
                dataXml = Tk5DataXml.Create(DataXml);
            else
                dataXml = Tk5DataXml.Create(DataXml, TableName);

            return dataXml;
        }

        public override Tk5TableScheme CreateTableScheme(ITableSchemeEx scheme, IInputData input)
        {
            return new Tk5TableScheme(scheme, input, this, SchemeUtil.CreateDataXmlField);
        }
    }
}
