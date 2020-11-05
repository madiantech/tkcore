using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal static class RazorTest
    {
        private static readonly Dictionary<string, Assembly> fAssemblyDictionary = TestAssembly();

        public static Dictionary<string, Assembly> TestAssembly()
        {
            AppDomain domain = AppDomain.CurrentDomain;
            var assembies = domain.GetAssemblies();
            Dictionary<string, Assembly> dict = new Dictionary<string, Assembly>();
            foreach (var assembly in assembies)
            {
                string name = assembly.GetName().Name;
                dict.Add(name, assembly);
            }

            return dict;
        }

        //private static MetadataReference[] GetMetadataReference(params string[] names)
        //{
        //    MetadataReference[] result = new MetadataReference[names.Length];
        //    for (int i = 0; i < names.Length; i++)
        //    {
        //        string name = names[i];
        //        Assembly ass = fAssemblyDictionary[name];
        //        result[i] = MetadataReference.CreateFromFile(ass.Location);
        //    }

        //    return result;
        //}

        public static void TestUpdateData()
        {
            IInputData input = new TestInputData((PageStyleClass)PageStyle.Update,
                new TestQueryString("Id", "1"), "hello");
            const string resolverString = "<tk:DataXmlResolver DataXml='UserManager/Part.xml'/>";
            IConfigCreator<TableResolver> creator = resolverString.ReadXmlFromFactory<IConfigCreator<TableResolver>>(
                ResolverCreatorConfigFactory.REG_NAME);
            TestDetailConfig config = new TestDetailConfig(creator);
            SingleDbDetailSource source = new SingleDbDetailSource(config);
            var result = source.DoAction(input);
            string xml = result.Data.Convert<DataSet>().GetXml();

            const string metaString = "<tk:SingleXmlMetaData DataXml=\"UserManager/Part.xml\" />";
            IConfigCreator<IMetaData> metaDataCreator = metaString.ReadXmlFromFactory<IConfigCreator<IMetaData>>(
                MetaDataConfigFactory.REG_NAME);

            var metaData = metaDataCreator.CreateObject(input);
            string metaXml = metaData.WriteXml();

            dynamic viewBag = new ExpandoObject();
            viewBag.MetaData = metaData;
            //var engine = new RazorLightEngineBuilder()
            //    .UseFileSystemProject(@"E:\VS2018\Toolkit5.5\Test\TestConsoleApp", ".cshtml")
            //    .UseMemoryCachingProvider()
            //    .Build();

            //string html = await engine.CompileRenderAsync<DataSet>("test", result.Data.Convert<DataSet>(), viewBag);
            //Console.WriteLine(html);
        }
    }
}