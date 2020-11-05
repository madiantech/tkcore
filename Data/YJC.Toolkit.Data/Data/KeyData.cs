using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class KeyData : IEnumerable<KeyValuePair<string, string>>
    {
        public static readonly KeyData Empty = new KeyData(ToolkitConst.TOOLKIT, string.Empty);
        public KeyData(IEntity entity)
        {
            TkDebug.AssertArgumentNull(entity, "entity", null);

            Initialize("Id", entity.Id);
        }

        public KeyData(string nickName, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);
            TkDebug.AssertArgumentNull(value, "value", null);

            Initialize(nickName, value);
        }

        public KeyData(string[] nickNames, string[] values)
        {
            TkDebug.AssertArgumentNull(nickNames, "nickNames", null);
            TkDebug.AssertArgumentNull(values, "values", null);
            TkDebug.AssertArgument(nickNames.Length == values.Length, "nickNames", string.Format(
                ObjectUtil.SysCulture, "nickNames和values的长度不一致，nickNames长度为{0}，values的长度为{1}",
                nickNames.Length, values.Length), this);
            TkDebug.AssertArgument(nickNames.Length > 1, "nickNames", string.Format(
                ObjectUtil.SysCulture, "nickNames的长度必须大于1，当前为{0}", nickNames.Length), this);

            Data = new Dictionary<string, string>();
            int len = nickNames.Length;
            for (int i = 0; i < len; ++i)
                Data.Add(nickNames[i], values[i]);
            IsSingleValue = false;
        }

        public KeyData(IEnumerable<KeyValuePair<string, string>> keyPairs)
        {
            TkDebug.AssertArgumentNull(keyPairs, "keyPairs", this);

            Data = new Dictionary<string, string>();
            foreach (var item in keyPairs)
                Data.Add(item.Key, item.Value);
            IsSingleValue = false;
        }

        #region IEnumerable<KeyValuePair<string,string>> 成员

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
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

        [Dictionary]
        private Dictionary<string, string> Data { get; set; }

        public bool IsSingleValue { get; private set; }

        public string SingleNickName { get; private set; }

        public string SingleValue { get; private set; }

        private void Initialize(string nickName, string value)
        {
            Data = new Dictionary<string, string>();
            Data.Add(nickName, value);
            IsSingleValue = true;
            SingleNickName = nickName;
            SingleValue = value;
        }

        public string this[string nickName]
        {
            get
            {
                return ObjectUtil.TryGetValue(Data, nickName);
            }
        }

        public override string ToString()
        {
            var datas = from item in Data
                        select item.Key + "=" + item.Value;
            return string.Join("&", datas);
        }
    }
}
