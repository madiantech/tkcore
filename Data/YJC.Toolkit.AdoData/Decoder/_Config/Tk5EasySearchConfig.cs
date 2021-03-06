﻿using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [EasySearchConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-02-07",
        Author = "YJC", Description = "支持Tk5的DataXml的EasySearch配置")]
    [ObjectContext]
    internal class Tk5EasySearchConfig : BaseEasySearchConfig
    {
        public const string BASE_CLASS = "Tk5EasySearch";

        [SimpleAttribute(Required = true)]
        public string DataXml { get; protected set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }

        public override ITableSchemeEx CreateScheme()
        {
            return Tk5DataXml.Create(DataXml);
        }
    }
}