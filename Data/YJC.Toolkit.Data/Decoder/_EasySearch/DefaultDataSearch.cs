using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class DefaultDataSearch : IDataSearch
    {
        public static readonly IDataSearch SearchMethod = new DefaultDataSearch();

        private DefaultDataSearch()
        {
        }

        #region IDataSearch 成员

        public bool Search(EasySearch easySearch, SearchField searchType,
            string dataValue, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
                return true;

            switch (searchType)
            {
                case SearchField.DefaultValue:
                    return true;
                case SearchField.Value:
                    return dataValue.StartsWith(searchValue);
                case SearchField.Name:
                    return dataValue.IndexOf(searchValue) >= 0;
                case SearchField.Pinyin:
                    return dataValue.StartsWith(searchValue.ToUpper(ObjectUtil.SysCulture));
            }
            return false;
        }

        #endregion
    }
}
