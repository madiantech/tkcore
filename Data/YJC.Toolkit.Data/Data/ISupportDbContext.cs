namespace YJC.Toolkit.Data
{
    public interface ISupportDbContext
    {
        DbContextConfig Default { get; }

        DbContextConfig GetContextConfig(string name);
    }
}
