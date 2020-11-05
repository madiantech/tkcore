using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace RazorTest
{
    public static class QuickTest
    {
        public static void TestUpdateData()
        {
            IInputData input = new TestInputData((PageStyleClass)PageStyle.Update,
                new TestQueryString("Id", "1"), "hello");
            const string resolverString = "<tk:TableNameResolver TableName='UR_PART' AutoUpdateKey='true' />";
            IConfigCreator<TableResolver> creator = resolverString.ReadXmlFromFactory<IConfigCreator<TableResolver>>(
                ResolverCreatorConfigFactory.REG_NAME);
            TestDetailConfig config = new TestDetailConfig(creator);
            SingleDbDetailSource source = new SingleDbDetailSource(config);
            var result = source.DoAction(input);
            string xml = result.Data.Convert<DataSet>().GetXml();

            //const string metaString = "<tk:SingleXmlMetaData DataXml=\"UserManager/Part.xml\" />";
            //IConfigCreator<IMetaData> metaDataCreator = metaString.ReadXmlFromFactory<IConfigCreator<IMetaData>>(
            //    MetaDataConfigFactory.REG_NAME);
        }
    }
}