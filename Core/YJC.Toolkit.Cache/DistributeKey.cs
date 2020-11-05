using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public sealed class DistributeKey
    {
        private DistributeKey()
        {
        }

        public DistributeKey(string storeName, string key)
        {
            TkDebug.AssertArgumentNullOrEmpty(storeName, nameof(storeName), null);
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), null);

            StoreName = storeName;
            Key = key;
        }

        [SimpleAttribute]
        public string StoreName { get; }

        [SimpleAttribute]
        public string Key { get; }

        public override string ToString()
        {
            return this.WriteJson();
        }

        public static DistributeKey FromJson(string json)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, nameof(json), null);

            DistributeKey key = new DistributeKey();
            key.ReadJson(json);
            return key;
        }
    }
}