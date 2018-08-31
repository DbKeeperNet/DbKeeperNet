namespace DbKeeperNet.Engine.DefaultCheckers
{
    public class ExistsIndexChecker : IDatabaseServiceIndexChecker
    {
        public bool Exists(string foreignKeyName, string table)
        {
            return true;
        }
    }
}