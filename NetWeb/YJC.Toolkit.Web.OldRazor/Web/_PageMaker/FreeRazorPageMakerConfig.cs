using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "使用Razor引擎，但是没有Razor模板，以纯粹的Razor文件生成Html输出",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-05-31")]
    internal class FreeRazorPageMakerConfig : IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            string fileName = FileUtil.GetRealFileName(RazorFile, Position);
            FreeRazorPageMaker freePageMaker;
            if (string.IsNullOrEmpty(RegType))
                freePageMaker = new FreeRazorPageMaker(fileName);
            else
            {
                Type type = ObjectUtil.GetRegType(RegType);
                freePageMaker = new FreeRazorPageMaker(fileName, type);
            }
            freePageMaker.Assemblies = Assemblies;
            freePageMaker.RazorData = RazorData;
            return freePageMaker;
        }

        #endregion

        [SimpleAttribute]
        public string RazorFile { get; private set; }

        [SimpleAttribute(DefaultValue = FilePathPosition.Xml)]
        public FilePathPosition Position { get; private set; }

        [SimpleAttribute]
        public string RegType { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(RazorDataConfigFactory.REG_NAME)]
        public IConfigCreator<object> RazorData { get; set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Assembly")]
        public List<string> Assemblies { get; protected set; }
    }
}
