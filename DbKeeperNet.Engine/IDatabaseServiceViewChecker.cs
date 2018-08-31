namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceViewChecker
    {
        bool Exists(string name);
    }
}