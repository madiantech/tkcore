using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class LevelTreeDefinition : IReadObjectCallBack
    {
        public const string ID_FIELD = "Value";
        public const string NAME_FIELD = "Name";
        private List<LevelItem> fItems;
        private int[] fLevel;

        internal LevelTreeDefinition()
        {
        }

        public LevelTreeDefinition(string idField, string nameField, IEnumerable<int> levels)
        {
            IdField = string.IsNullOrEmpty(idField) ? ID_FIELD : idField;
            NameField = string.IsNullOrEmpty(nameField) ? NAME_FIELD : nameField;
            CalLevel(levels);
        }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (Levels != null)
                CalLevel(Levels);
        }

        #endregion

        public int TotalCount { get; private set; }

        public int TotalLevel
        {
            get
            {
                return fItems.Count;
            }
        }

        [SimpleAttribute(DefaultValue = ID_FIELD)]
        public string IdField { get; private set; }

        [SimpleAttribute(DefaultValue = NAME_FIELD)]
        public string NameField { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Level")]
        private List<int> Levels { get; set; }

        private void CalLevel(IEnumerable<int> levels)
        {
            fItems = new List<LevelItem>();
            int start = 0;
            LevelItem current = null;
            foreach (int len in levels)
            {
                current = new LevelItem(start, len);
                fItems.Add(current);
                start = current.End;
            }
            if (current != null)
                TotalCount = current.End;

            fLevel = new int[TotalCount];
            int j = 0;
            for (int i = 0; i < fItems.Count; ++i)
            {
                var subItem = fItems[i];
                for (; j < subItem.End; ++j)
                    fLevel[j] = i;
            }
        }

        internal int GetLevel(string value)
        {
            int length = value.Length;
            return length == 0 ? 0 : fLevel[length - 1];
        }

        internal LevelItem GetLevelItem(int index)
        {
            return fItems[index];
        }
    }
}
