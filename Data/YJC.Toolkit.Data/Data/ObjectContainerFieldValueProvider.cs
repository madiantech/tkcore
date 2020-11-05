using System.Collections.Generic;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ObjectContainerFieldValueProvider : IFieldValueProvider, IOperatorFieldProvider
    {
        public ObjectContainerFieldValueProvider(ObjectContainer container, CodeTableContainer codeTables)
        {
            Container = container;
            CodeTables = codeTables;
        }

        #region IFieldValueProvider 成员

        public object this[string nickName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

                if (Container == null)
                    return null;

                FieldValueProviderName name = nickName.Value<FieldValueProviderName>();
                if (name.IsDecoder)
                    return Container.Decoder.GetNameString(name.SourceName);
                else
                    return MemberValue(nickName, Container.MainObject);
            }
        }

        public IEnumerable<IDecoderItem> GetCodeTable(string regName)
        {
            if (CodeTables == null)
                return null;

            return CodeTables[regName];
        }

        public bool IsEmpty
        {
            get
            {
                return Container == null || Container.MainObject == null;
            }
        }

        #endregion IFieldValueProvider 成员

        #region IOperatorFieldProvider 成员

        public ObjectOperatorCollection Operators
        {
            get
            {
                return Container.Operators;
            }
        }

        #endregion IOperatorFieldProvider 成员

        public ObjectContainer Container { get; private set; }

        public CodeTableContainer CodeTables { get; private set; }

        private static object MemberValue(string fieldName, object receiver)
        {
            object value = null;
            try
            {
                value = receiver.MemberValue(fieldName);
            }
            catch
            {
            }
            return value;
        }
    }
}