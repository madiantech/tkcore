using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class InputConditionConfig
    {
        [SimpleAttribute(Required = true)]
        public ConditionUseType SearchType { get; private set; }

        [SimpleAttribute]
        public bool IsPost { get; private set; }

        [SimpleAttribute(LocalName = "Style")]
        [TkTypeConverter(typeof(HashSetPageStyleTypeConverter))]
        public HashSet<PageStyleClass> Styles { get; private set; }

        [SimpleAttribute]
        public SourceOutputType OutputType { get; private set; }

        [SimpleAttribute]
        public string StartsWith { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true,
            IsMultiple = true, LocalName = "QueryString")]
        public List<CodeItem> QueryStrings { get; private set; }

        private bool Contains(ConditionUseType condition)
        {
            return (SearchType & condition) == condition;
        }

        public bool UseCondition(IInputData input)
        {
            TkDebug.AssertArgumentNull(input, "input", this);

            if (Contains(ConditionUseType.True))
                return true;
            bool result = true;
            if (Contains(ConditionUseType.Post))
                result &= input.IsPost == IsPost;
            if (Contains(ConditionUseType.Style))
                result &= Styles != null && Styles.Contains(PageStyleClass.FromStyle(input.Style));
            if (Contains(ConditionUseType.StyleStartsWith))
                result &= MetaDataUtil.StartsWith(input.Style, StartsWith);

            if (Contains(ConditionUseType.QueryString) && QueryStrings != null)
            {
                foreach (var item in QueryStrings)
                    result &= input.QueryString[item.Name] == item.Value;
            }

            return result;
        }

        public bool UseCondition(ISource source, IInputData pageData, OutputData outputData)
        {
            bool result = UseCondition(pageData);
            if (Contains(ConditionUseType.OutputType))
                result &= outputData.OutputType == OutputType;

            return result;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (Contains(ConditionUseType.True))
                builder.Append("[True]");
            else
            {
                if (Contains(ConditionUseType.Style))
                    builder.AppendFormat("[Style:{0}]", ObjectUtil.ToString(Styles, ObjectUtil.WriteSettings));
                if (Contains(ConditionUseType.Post))
                    builder.AppendFormat("[Style:{0}]", IsPost);
                if (Contains(ConditionUseType.StyleStartsWith))
                    builder.AppendFormat("[StartsWith:{0}]", StartsWith);
                if (Contains(ConditionUseType.QueryString) && QueryStrings != null)
                {
                    var query = (from item in QueryStrings
                                 select item.ToString());
                    builder.AppendFormat("[Query:{0}]", string.Join(",", query));
                }
            }
            return builder.ToString();
        }
    }
}
