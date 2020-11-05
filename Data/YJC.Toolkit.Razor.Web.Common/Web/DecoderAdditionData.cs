using System.Collections.Generic;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class DecoderAdditionData
    {
        public DecoderAdditionData()
        {
            AdditionData = new Dictionary<string, string>();
        }

        [Dictionary]
        public Dictionary<string, string> AdditionData { get; private set; }

        public void AddData(IDecoderItem row, IEnumerable<DecoderAdditionInfo> infos)
        {
            if (row == null || infos == null)
                return;

            foreach (var info in infos)
            {
                try
                {
                    string columnName = info.DecoderNickName;
                    string value = row[columnName].ToString();
                    AdditionData.Add(columnName, value);
                }
                catch
                {
                }
            }
        }

        public string ToJson()
        {
            return this.WriteJson();
        }
    }
}