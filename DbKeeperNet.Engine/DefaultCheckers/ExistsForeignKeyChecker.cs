namespace DbKeeperNet.Engine.DefaultCheckers
{
    public class ExistsForeignKeyChecker : IDatabaseServiceForeignKeyChecker
    {
        public bool Exists(string foreignKeyName, string table)
        {
            return true;
        }
    }
}