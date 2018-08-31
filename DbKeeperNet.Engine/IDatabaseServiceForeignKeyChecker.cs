namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceForeignKeyChecker
    {
        bool Exists(string foreignKeyName, string table);
    }
}