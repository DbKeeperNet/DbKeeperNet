namespace DbKeeperNet.Engine
{
    public interface IDatabaseServicePrimaryKeyChecker
    {
        bool Exists(string name, string table);
    }
}