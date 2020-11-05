namespace YJC.Toolkit.Data
{
    interface ILevelProvider
    {
        int GetLevel(LevelTreeDefinition def, string value);

        string GetParentId(LevelTreeDefinition def, int level, string value);

        string GetSqlLikeValue(LevelTreeDefinition def, int level, string value);

        string GetSqlExceptValue(LevelTreeDefinition def, int level, string value);
    }
}
