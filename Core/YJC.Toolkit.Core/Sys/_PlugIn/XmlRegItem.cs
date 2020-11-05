using System;

namespace YJC.Toolkit.Sys
{
    internal class XmlRegItem : BaseRegItem
    {
        public XmlRegItem(string regName, IXmlPlugInItem xmlConfig,
            Type baseClassType, string fileName)
            : base(regName, new XmlDummyPlugInAttribute(regName, xmlConfig))
        {
            XmlConfig = xmlConfig;
            BaseClassType = baseClassType;
            FileName = fileName;
        }

        public IXmlPlugInItem XmlConfig { get; private set; }

        public Type BaseClassType { get; private set; }

        public string FileName { get; private set; }

        public override T CreateInstance<T>()
        {
            object result = ObjectUtil.CreateObject(BaseClassType, XmlConfig);
            AssertCreateType<T>(BaseClassType);
            return result as T;
        }

        public override T CreateInstance<T>(params object[] args)
        {
            object[] realArgs = new object[args.Length + 1];
            realArgs[0] = XmlConfig;
            Array.Copy(args, 0, realArgs, 1, args.Length);

            object result = ObjectUtil.CreateObject(BaseClassType, realArgs);
            AssertCreateType<T>(BaseClassType);
            return result as T;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(RegName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "注册名为{0}的Xml配置单元", RegName);
        }
    }
}
