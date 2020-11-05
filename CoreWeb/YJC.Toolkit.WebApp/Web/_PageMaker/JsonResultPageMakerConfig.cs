using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将DataSet输出成Json，同时附着输出结果",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-02-18")]
    internal class JsonResultPageMakerConfig : IConfigCreator<IPageMaker>, IObjectFormat, IReadObjectCallBack
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new JsonResultPageMaker(this);
        }

        #endregion

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Result == null)
                Result = ActionResultData.CreateSuccessResult("操作成功");
        }

        #endregion

        [SimpleAttribute(DefaultValue = ConfigType.SystemConfiged)]
        public ConfigType GZip { get; private set; }

        [SimpleAttribute(DefaultValue = ConfigType.SystemConfiged)]
        public ConfigType Encrypt { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public ActionResultData Result { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "TableMapping")]
        public List<TableMappingConfig> TableMappings { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "RemoveTable")]
        public List<string> RemoveTables { get; private set; }
    }
}
