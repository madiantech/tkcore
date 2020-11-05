using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class FieldOperateRightItem : IRegName
    {
        protected FieldOperateRightItem()
        {
        }

        public FieldOperateRightItem(string value, bool containsNull, IEnumerable<string> rights)
        {
            Value = value;
            ContainsNull = containsNull;
            Rights = rights;
        }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Value;
            }
        }

        #endregion

        [SimpleAttribute]
        public string Value { get; private set; }

        [SimpleAttribute]
        public bool ContainsNull { get; private set; }

        [TextContent(ObjectType = typeof(string[]))]
        public IEnumerable<string> Rights { get; private set; }
    }
}
