using System;
using System.Collections.Generic;
using System.Text;
using UnitTest;
using YJC.Toolkit.Sys;

namespace TestToolApp
{
    public static class XmlTestUtil
    {
        public static void Hello()
        {
            string xml = "<tk:Toolkit xmlns:tk='http://www.qdocuments.net' Hello='2' Modifiers='Shift Alt'><World>Hello</World></tk:Toolkit>";
            TestObject obj = new TestObject();
            obj.ReadXml(xml, ReadSettings.Default, QName.Toolkit);

            Console.WriteLine(obj.Hello);
        }
    }
}