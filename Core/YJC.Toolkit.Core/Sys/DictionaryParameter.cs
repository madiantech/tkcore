using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    internal sealed class DictionaryParameter : IParameter
    {
        private readonly Dictionary<string, string> fDictionary;

        /// <summary>
        /// Initializes a new instance of the DictionaryCreateParameter class.
        /// </summary>
        /// <param name="dictionary"></param>
        public DictionaryParameter(Dictionary<string, string> dictionary)
        {
            TkDebug.AssertArgumentNull(dictionary, "dictionary", null);

            fDictionary = dictionary;
        }

        #region IParameter 成员

        public string this[string name]
        {
            get
            {
                string result;
                fDictionary.TryGetValue(name, out result);
                return result ?? string.Empty;
            }
        }

        #endregion

        public override string ToString()
        {
            return "Count=" + fDictionary.Count;
        }
    }
}
