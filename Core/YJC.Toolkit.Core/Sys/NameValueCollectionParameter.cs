using System.Collections.Specialized;

namespace YJC.Toolkit.Sys
{
    internal sealed class NameValueCollectionParameter : IParameter
    {
        private readonly NameValueCollection fCollection;

        /// <summary>
        /// Initializes a new instance of the NameValueCollectionCreateParameter class.
        /// </summary>
        /// <param name="collection"></param>
        public NameValueCollectionParameter(NameValueCollection collection)
        {
            TkDebug.AssertArgumentNull(collection, "collection", null);

            fCollection = collection;
        }

        #region IParameter 成员

        public string this[string name]
        {
            get
            {
                return fCollection[name] ?? string.Empty;
            }
        }

        #endregion
    }
}
