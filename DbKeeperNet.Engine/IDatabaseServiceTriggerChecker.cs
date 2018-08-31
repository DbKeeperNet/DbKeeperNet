namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceTriggerChecker
    {
        bool Exists(string name);
    }
}