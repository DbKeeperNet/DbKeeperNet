namespace DbKeeperNet.Engine.DefaultCheckers
{
    public class ExistsTriggerChecker : IDatabaseServiceTriggerChecker
    {
        public bool Exists(string name)
        {
            return true;
        }
    }
}