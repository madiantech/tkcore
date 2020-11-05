using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [XmlBaseClass(CodeDataConfigItem.CODE_DATA_BASE_CLASS, typeof(CodeDataCodeTable))]
    [XmlPlugIn(PATH, typeof(CodeDataXml), SearchPattern = "*CodeData.xml")]
    public class CodeTablePlugInFactory : BaseXmlPlugInFactory
    {
        public const string REG_NAME = "_tk_CodeTable";
        public const string PATH = "Decoder";
        private const string DESCRIPTION = "代码表的插件工厂";

        public CodeTablePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

        protected override bool Add(string regName, BasePlugInAttribute attribute, Type type)
        {
            if (type.IsEnum)
            {
                EnumCodeTableAttribute enumAttr = attribute as EnumCodeTableAttribute;
                if (enumAttr != null)
                {
                    lock (this)
                    {
                        if (!Contains(regName))
                        {
                            EnumCodeRegItem regItem = new EnumCodeRegItem(regName, attribute, type);
                            InternalAddPlugIn(regName, regItem);
                            return true;
                        }
                    }
                }
                return false;
            }
            else
                return base.Add(regName, attribute, type);
        }

        public void RemoveStdCodeTable(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);
            TkDebug.AssertArgumentNull(regName.StartsWith("CD_", StringComparison.CurrentCulture),
                string.Format(ObjectUtil.SysCulture, "当前注册名为{0}，不是以CD_开始的，无法删除", regName), this);

            RemoveInstancePlugIn(regName);
        }
    }
}
