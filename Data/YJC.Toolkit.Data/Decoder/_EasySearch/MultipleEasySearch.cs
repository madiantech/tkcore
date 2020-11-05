using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public sealed class MultipleEasySearch : EasySearch
    {
        private readonly EasySearch fEasySearch;
        public MultipleEasySearch(EasySearch easySearch)
        {
            TkDebug.AssertArgumentNull(easySearch, "easySearch", null);

            fEasySearch = easySearch;
        }

        public override IDecoderItem Decode(string code, params object[] args)
        {
            MultipleDecodeItem item = new MultipleDecodeItem(code, fEasySearch, args);
            return item;
        }

        public override IEnumerable<IDecoderItem> Search(SearchField field,
            string text, List<EasySearchRefField> refFields)
        {
            return fEasySearch.Search(field, text, refFields);
        }

        public override IDecoderItem[] SearchByName(string name, params object[] args)
        {
            return fEasySearch.SearchByName(name, args);
        }
    }
}
