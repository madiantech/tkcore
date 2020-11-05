namespace YJC.Toolkit.Data
{
    class Level0Provider : ILevelProvider
    {
        public static ILevelProvider Provider = new Level0Provider();

        private Level0Provider()
        {
        }

        #region ILevelProvider 成员

        public int GetLevel(LevelTreeDefinition def, string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            return def.GetLevel(value.TrimEnd('0'));
        }

        public string GetParentId(LevelTreeDefinition def, int level, string value)
        {
            if (level == 0)
                return "#";
            else
            {
                LevelItem item = def.GetLevelItem(level - 1);
                return value.Substring(0, item.End).PadRight(def.TotalCount, '0');
            }
        }

        public string GetSqlLikeValue(LevelTreeDefinition def, int level, string value)
        {
            LevelItem item = def.GetLevelItem(level);
            if (level == 0)
                return string.Empty.PadRight(item.End, '_').PadRight(def.TotalCount, '0');
            else
            {
                if (string.IsNullOrEmpty(value))
                    value = string.Empty;
                else
                    value = value.Substring(0, item.Start);
                return value.PadRight(item.End, '_').PadRight(def.TotalCount, '0');
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
                //int len = def.TotalCount - item.End;
                return value.PadRight(def.TotalCount, '0');
            }
        }

        #endregion
    }
}
