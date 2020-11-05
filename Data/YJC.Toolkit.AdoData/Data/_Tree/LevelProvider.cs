namespace YJC.Toolkit.Data
{
    class LevelProvider : ILevelProvider
    {
        public static ILevelProvider Provider = new LevelProvider();

        private LevelProvider()
        {
        }

        #region ILevelProvider 成员

        public int GetLevel(LevelTreeDefinition def, string value)
        {
            return def.GetLevel(value);
        }

        public string GetParentId(LevelTreeDefinition def, int level, string value)
        {
            if (level == 0)
                return "#";
            else
            {
                LevelItem item = def.GetLevelItem(level - 1);
                return value.Substring(0, item.End);
            }
        }

        public string GetSqlLikeValue(LevelTreeDefinition def, int level, string value)
        {
            LevelItem item = def.GetLevelItem(level);
            if (level == 0)
                return string.Empty.PadRight(item.End, '_');
            else
            {
                if (string.IsNullOrEmpty(value))
                    value = string.Empty;
                else
                    value = value.Substring(0, item.Start);
                return value.PadRight(item.End, '_');
            }
        }

        public string GetSqlExceptValue(LevelTreeDefinition def, int level, string value)
        {
            if (level == 0)
                return string.Empty;
            else
            {
                LevelItem item = def.GetLevelItem(level - 1);
                if (string.IsNullOrEmpty(value))
                    value = string.Empty;
                else
                    value = value.Substring(0, item.End);
                return value;
            }
        }

        #endregion
    }
}
