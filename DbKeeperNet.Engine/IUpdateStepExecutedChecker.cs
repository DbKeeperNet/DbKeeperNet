namespace DbKeeperNet.Engine
{
    public interface IUpdateStepExecutedChecker
    {
        bool IsExecuted(string assembly, string version, int stepNumber);
    }
}