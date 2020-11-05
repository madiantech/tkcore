using System;
using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.MetaData
{
    internal class InternalTableSchemeEx : ITableSchemeEx
    {
        private readonly IEnumerable<IFieldInfoEx> fFields;
        private readonly ITableScheme fScheme;

        public InternalTableSchemeEx(ITableScheme scheme, Func<IFieldInfo, IFieldInfoEx> converter)
        {
            fScheme = scheme;
            fFields = (from field in scheme.Fields
                       select CreateFieldInfoEx(field, converter)).ToArray();
            NameField = MetaDataUtil.GetNameField(fFields);
        }

        #region ITableSchemeEx 成员

        public string TableName
        {
            get
            {
                return fScheme.TableName;
            }
        }

        public string TableDesc
        {
            get
            {
                return TableName;
            }
        }

        public IEnumerable<IFieldInfoEx> Fields
        {
            get
            {
                return fFields;
            }
        }

        public IEnumerable<IFieldInfoEx> AllFields
        {
            get
            {
                return fFields;
            }
        }

        public IFieldInfoEx NameField { get; }

        #endregion ITableSchemeEx 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fScheme[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        private static IFieldInfoEx CreateFieldInfoEx(IFieldInfo field,
            Func<IFieldInfo, IFieldInfoEx> converter)
        {
            if (converter != null)
            {
                IFieldInfoEx result = converter(field);
                if (result != null)
                    return result;
            }
            return new InternalFieldInfoEx(field);
        }
    }
}