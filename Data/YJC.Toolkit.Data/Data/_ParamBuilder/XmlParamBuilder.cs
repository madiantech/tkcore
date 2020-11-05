using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class XmlParamBuilder : IParamBuilder
    {
        //private readonly static XName ROOT = XName.Get("ParamBuilder");

        internal XmlParamBuilder()
        {
        }

        public XmlParamBuilder(IParamBuilder builder)
        {
            TkDebug.AssertArgumentNull(builder, "builder", null);

            Sql = builder.Sql;
            Parameters = builder.Parameters;
        }

        #region IParamBuilder 成员

        [SimpleAttribute]
        public string Sql { get; private set; }

        [ObjectElement]
        public DbParameterList Parameters { get; private set; }

        #endregion

        //public static string ToString(IParamBuilder builder)
        //{
        //    XmlParamBuilder paramBuilder = new XmlParamBuilder(builder);

        //    StringBuilder result = new StringBuilder();
        //    //XmlWriter writer = XmlWriter.Create(result);
        //    //using (writer)
        //    //{
        //    //    XmlUtil.WriteXml(writer, paramBuilder, ROOT);
        //    //}
        //    return result.ToString();
        //}

        //public static IParamBuilder FromString(string xml)
        //{
        //    return null;
        //    //XmlReader reader = XmlReader.Create(new StringReader(xml), XmlUtil.ReaderSetting);
        //    //using (reader)
        //    //{
        //    //    XmlParamBuilder paramBuilder = new XmlParamBuilder();
        //    //    XmlUtil.ReadXml(reader, ROOT, paramBuilder);
        //    //    return paramBuilder;
        //    //}
        //}
    }
}
