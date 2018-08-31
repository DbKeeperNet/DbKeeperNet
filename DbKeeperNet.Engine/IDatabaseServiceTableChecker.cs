namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceTableChecker
    {
        bool Exists(string name);
    }
}