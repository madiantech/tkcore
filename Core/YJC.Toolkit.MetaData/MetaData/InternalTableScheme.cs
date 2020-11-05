using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.MetaData
{
    internal sealed class InternalTableScheme : ITableScheme
    {
        private readonly ITableSchemeEx fTableScheme;
        private readonly IEnumerable<IFieldInfoEx> fDataFields;

        /// <summary>
        /// Initializes a new instance of the InternalTableScheme class.
        /// </summary>
        /// <param name="tableScheme"></param>
        public InternalTableScheme(ITableSchemeEx tableScheme)
        {
            fTableScheme = tableScheme;
            fDataFields = (from item in tableScheme.Fields
                           where item.Kind == FieldKind.Data
                           select item).ToArray();
        }

        #region ITableScheme 成员

        public string TableName
        {
            get
            {
                return fTableScheme.TableName;
            }
        }

        public IEnumerable<IFieldInfo> Fields
        {
            get
            {
                return fDataFields;
            }
        }

        public IEnumerable<IFieldInfo> AllFields
        {
            get
            {
                return fTableScheme.Fields;
            }
        }

        public IFieldInfo this[string name]
        {
            get
            {
                return fTableScheme[name];
            }
        }

        #endregion ITableScheme 成员
    }
}