namespace DbKeeperNet.Engine.DefaultCheckers
{
    public class ExistsViewChecker : IDatabaseServiceTriggerChecker
    {
        public bool Exists(string name)
        {
            return true;
        }
    }
}