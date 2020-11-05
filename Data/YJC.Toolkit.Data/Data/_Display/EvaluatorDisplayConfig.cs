using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-12-01", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "通过Evaluator计算得到显示的结果")]
    [ObjectContext]
    internal class EvaluatorDisplayConfig : IDisplay, IConfigCreator<IDisplay>
    {
        #region IDisplay 成员

        public string DisplayValue(object value, Tk5FieldInfoEx field, IFieldValueProvider rowValue)
        {
            if (string.IsNullOrEmpty(Content))
                return string.Empty;

            return EvaluatorUtil.Execute<string>(Content,
                ("value", value), ("row", rowValue));
        }

        #endregion IDisplay 成员

        #region IConfigCreator<IDisplay> 成员

        public IDisplay CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IDisplay> 成员

        [TextContent(Required = true)]
        public string Content { get; private set; }
    }
}