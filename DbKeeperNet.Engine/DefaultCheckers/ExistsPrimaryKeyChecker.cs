namespace DbKeeperNet.Engine.DefaultCheckers
{
    public class ExistsPrimaryKeyChecker : IDatabaseServicePrimaryKeyChecker
    {
        public bool Exists(string name, string table)
        {
            return true;
        }
    }
}