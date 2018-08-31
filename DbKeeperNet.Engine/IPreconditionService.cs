namespace DbKeeperNet.Engine
{
    public interface IPreconditionService
    {
        bool IsMet(UpdateStepContextWithPreconditions context);
    }
}