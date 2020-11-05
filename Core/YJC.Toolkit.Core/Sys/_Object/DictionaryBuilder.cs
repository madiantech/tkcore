using System.Collections;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    internal class DictionaryBuilder
    {
        private readonly Dictionary<string, object> fData;
        private readonly DictionaryOutput fOutput;

        public DictionaryBuilder(DictionaryOutput output)
        {
            fOutput = output ?? DictionaryOutput.Default;
            fData = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Data
        {
            get
            {
                return fData;
            }
        }

        public DictionaryOutput Output
        {
            get
            {
                return fOutput;
            }
        }

        public void Add(string name, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            if (!(fOutput.IgnoreEmpty && string.IsNullOrEmpty(value)))
                fData[name] = value;
        }

        public void Add(string name, IList value)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            if (!(fOutput.IgnoreEmpty && value == null))
                fData[name] = value;
        }

        public void Add(string name, DictionaryBuilder builder)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);
            TkDebug.AssertArgumentNull(builder, "builder", this);

            if (!(fOutput.IgnoreEmpty && builder.Data.Count == 0))
                fData[name] = builder.Data;
        }

        public DictionaryBuilder CreateBuilder()
        {
            return new DictionaryBuilder(fOutput);
        }
    }
}
