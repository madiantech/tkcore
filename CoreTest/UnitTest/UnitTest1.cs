using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;
using static Xunit.Assert;

namespace UnitTest
{
    public class UnitTest1 : IClassFixture<UnitTest1>
    {
        public UnitTest1()
        {
            TestApp.Initialize(@"E:\VS2014\Toolkit5\Test");
        }

        [Fact]
        public void Test1()
        {
            string xml = "<tk:Toolkit xmlns:tk='http://www.qdocuments.net' Hello='2' Modifiers='Shift Alt'><World>Hello</World></tk:Toolkit>";
            TestObject obj = new TestObject();
            obj.ReadXml(xml, ReadSettings.Default, QName.Toolkit);

            Equal(2, obj.Hello);
            //Assert.AreEqual(obj.Content, 3.14);
            Equal("Hello", obj.World);
            Equal(obj.Modifiers, ConsoleModifiers.Alt | ConsoleModifiers.Shift);
        }

        [Fact]
        public void TestPathString()
        {
            PathString path = PathString.FromUriComponent(new Uri("https://docs.microsoft.com/zh-cn/dotnet/api/"));
            path += (PathString)"/hello";
            path += "/world";
            string path2 = path.ToString();
            Equal("/zh-cn/dotnet/api/hello/world", path2);

            HostString host = HostString.FromUriComponent(new Uri("http://docs.microsoft.com/zh-cn/dotnet/api/"));
            string host2 = host.ToString();
            Equal("https://docs.microsoft.com/", host2);
        }

        [Fact]
        public void TestDayChange()
        {
            DayChangeCacheAttribute attribute = new DayChangeCacheAttribute();
            var de = attribute.CreateObject();
            bool changed = de.HasChanged;
            False(changed);
        }

        [Fact]
        public void TestString()
        {
            string s = "zd89xcqcy0g0pr7n2dlkhq==";
            byte[] data = Convert.FromBase64String(s);

            True(data.Length > 0);
            TkDebug.AssertArgumentNull(null, "hello", null);
        }

        [Fact]
        public void TestPath()
        {
            string path1 = "^Normal/List/template.cshtml";
            string path2 = "../Bin/js.cshtml";
            string newPath = FileUtil.JoinPath(path1, path2);
            Equal("^Normal/Bin/js.cshtml", newPath);

            path1 = "^Normal/List/template.cshtml";
            path2 = "Bin/js.cshtml";
            newPath = FileUtil.JoinPath(path1, path2);
            Equal("^Normal/List/Bin/js.cshtml", newPath);

            path1 = "^Normal/List/template.cshtml";
            path2 = "../../Bin/js.cshtml";
            newPath = FileUtil.JoinPath(path1, path2);
            Equal("Bin/js.cshtml", newPath);

            path1 = "^Normal/List/template.cshtml";
            path2 = "../../../../Bin/js.cshtml";
            newPath = FileUtil.JoinPath(path1, path2);
            Equal("../../Bin/js.cshtml", newPath);
        }

        [Fact]
        public void TestType()
        {
            //var t = typeof(SimpleAttributeAttribute);
            //Trace.WriteLine(t.AssemblyQualifiedName);
            //t = typeof(BaseDistributeCacheItemCreator<>);
            //Trace.WriteLine(t.AssemblyQualifiedName);
            Type t = Type.GetType("YJC.Toolkit.Sys.SimpleAttributeAttribute, YJC.Toolkit.Core, Version=5.6.0.0, Culture=neutral, PublicKeyToken=dcd89483ee33cb8f");
            Trace.WriteLine(t.AssemblyQualifiedName);
            t = Type.GetType("YJC.Toolkit.Cache.BaseDistributeCacheItemCreator`1, YJC.Toolkit.Core, Version=5.6.0.0, Culture=neutral, PublicKeyToken=dcd89483ee33cb8f");
            Trace.WriteLine(t.AssemblyQualifiedName);
            t = Type.GetType("System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            Trace.WriteLine(t.AssemblyQualifiedName);
            t = Type.GetType("System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            Trace.WriteLine(t.AssemblyQualifiedName);
        }
    }
}