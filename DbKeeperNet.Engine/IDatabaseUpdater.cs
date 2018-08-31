namespace DbKeeperNet.Engine
{
    public interface IDatabaseUpdater
    {
        void ExecuteUpgrade();
    }
}