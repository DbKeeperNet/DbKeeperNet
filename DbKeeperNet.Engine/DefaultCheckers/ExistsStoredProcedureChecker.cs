namespace DbKeeperNet.Engine.DefaultCheckers
{
    public class ExistsStoredProcedureChecker : IDatabaseServiceStoredProcedureChecker
    {
        public bool Exists(string name)
        {
            return true;
        }
    }
}