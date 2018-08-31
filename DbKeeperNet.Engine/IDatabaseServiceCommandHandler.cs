namespace DbKeeperNet.Engine
{
    public interface IDatabaseServiceCommandHandler
    {
        void Execute(string command);
    }
}