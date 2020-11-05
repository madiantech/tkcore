using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    class EasySearchContainer
    {
        private readonly EasySearch fEasySearch;
        private MultipleEasySearch fMultiEasySearch;

        public EasySearchContainer(EasySearch easySearch)
        {
            fEasySearch = easySearch;
        }

        public EasySearch GetEasySearch(ControlType ctrlType)
        {
            if (ctrlType == ControlType.MultipleEasySearch)
            {
                if (fMultiEasySearch == null)
                    fMultiEasySearch = new MultipleEasySearch(fEasySearch);
                return fMultiEasySearch;
            }
            else
                return fEasySearch;
        }
    }
}
