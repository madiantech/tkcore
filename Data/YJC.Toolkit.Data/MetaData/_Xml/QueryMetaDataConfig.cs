using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [MetaDataConfig(Author = "YJC", CreateDate = "2015-10-23", NamespaceType = NamespaceType.Toolkit, 
        Description = "根据条件输出查询结果的MetaData，即在Get时，输出Condition的MetaData，"
        + "Post时输出Result的MetaData。无论当前页面的Style是什么，结果都是List的页面的MetaData")]
    [ObjectContext]
    internal class QueryMetaDataConfig : IConfigCreator<IMetaData>
    {
        #region IConfigCreator<IMetaData> 成员

        public IMetaData CreateObject(params object[] args)
        {
            IInputData input = ObjectUtil.ConfirmQueryObject<IInputData>(this, args);

            InputDataProxy inputProxy = new InputDataProxy(input, (PageStyleClass)PageStyle.List);
            if (input.IsPost)
                return CreateListMetaData(inputProxy, Result);
            else
                return CreateListMetaData(inputProxy, Condition);
        }

        #endregion

        [DynamicElement(SingleMetaDataConfigFactory.REG_NAME, Required = true)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<ISingleMetaData> Condition { get; private set; }

        [DynamicElement(SingleMetaDataConfigFactory.REG_NAME, Required = true)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<ISingleMetaData> Result { get; private set; }

        private static IMetaData CreateListMetaData(InputDataProxy inputProxy, 
            IConfigCreator<ISingleMetaData> singleMetaData)
        {
            var metaData = singleMetaData.CreateObject();
            var scheme = metaData.CreateSourceScheme(inputProxy);
            return new Tk5ListMetaData(scheme, inputProxy, metaData);
        }
    }
}
