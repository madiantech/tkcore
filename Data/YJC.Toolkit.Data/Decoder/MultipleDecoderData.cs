using System.Collections;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public sealed class MultipleDecoderData : IEnumerable<CodeItem>
    {
        internal MultipleDecoderData()
        {
            Data = new List<CodeItem>();
        }

        #region IEnumerable<CodeItem> 成员

        public IEnumerator<CodeItem> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        [ObjectElement(IsMultiple = true, UseConstructor = true)]
        private List<CodeItem> Data { get; set; }


        public void AddItem(IDecoderItem item)
        {
            if (item == null)
                return;

            CodeItem codeItem = item as CodeItem;
            if (codeItem == null)
                codeItem = new CodeItem(item);
            Data.Add(codeItem);
        }

        public string ToJson()
        {
            string json = this.WriteJson(WriteSettings.Default);
            return json;
        }

        public static MultipleDecoderData ReadFromString(string json)
        {
            MultipleDecoderData result = new MultipleDecoderData();
            if (!string.IsNullOrEmpty(json))
                result.ReadJson(json, ReadSettings.Default);

            return result;
        }
    }
}
