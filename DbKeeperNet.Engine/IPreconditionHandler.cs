namespace DbKeeperNet.Engine
{
    public interface IPreconditionHandler
    {
        bool CanHandle(UpdateStepContextPrecondition context);

        bool IsMet(UpdateStepContextPrecondition context);
    }
}