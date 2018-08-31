namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceIndexChecker
    {
        bool Exists(string name, string table);
    }
}