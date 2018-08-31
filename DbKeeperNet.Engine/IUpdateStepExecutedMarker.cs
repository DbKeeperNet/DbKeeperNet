namespace DbKeeperNet.Engine
{
    public interface IUpdateStepExecutedMarker
    {
        void MarkAsExecuted(string assembly, string version, int stepNumber);
    }
}