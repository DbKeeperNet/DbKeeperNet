namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceStoredProcedureChecker
    {
        bool Exists(string name);
    }
}