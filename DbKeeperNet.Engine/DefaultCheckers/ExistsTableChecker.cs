namespace DbKeeperNet.Engine.DefaultCheckers
{
    public class ExistsTableChecker : IDatabaseServiceTableChecker
    {
        public bool Exists(string name)
        {
            return true;
        }
    }
}