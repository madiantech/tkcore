using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [ObjectContext]
    [TableSchemeConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-08-20",
        Author = "YJC", Description = "根据Tk5的DataXml获取得到的单表Scheme")]
    [TableSchemeExConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2017-11-01",
        Author = "YJC", Description = "根据Tk5的DataXml获取得到的单表TableSchemeEx")]
    internal class Tk5DataXmlConfig : IConfigCreator<ITableScheme>, IConfigCreator<ITableSchemeEx>
    {
        #region IConfigCreator<ITableScheme> 成员

        public ITableScheme CreateObject(params object[] args)
        {
            return CreateTk5DataXml();
        }

        #endregion IConfigCreator<ITableScheme> 成员

        [SimpleAttribute(Required = true)]
        public string FileName { get; private set; }

        [SimpleAttribute]
        public string TableName { get; private set; }

        private Tk5DataXml CreateTk5DataXml()
        {
            if (string.IsNullOrEmpty(TableName))
                return Tk5DataXml.Create(FileName);
            else
                return Tk5DataXml.Create(FileName, TableName);
        }

        ITableSchemeEx IConfigCreator<ITableSchemeEx>.CreateObject(params object[] args)
        {
            return CreateTk5DataXml();
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "DataXml是{0}的TableScheme", FileName);
        }
    }
}