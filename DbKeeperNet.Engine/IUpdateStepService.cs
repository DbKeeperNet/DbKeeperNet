namespace DbKeeperNet.Engine
{
    public interface IUpdateStepService
    {
        void Execute(UpdateStepContextWithPreconditions context);
    }
}