namespace DbKeeperNet.Engine
{
    public class StaticConnectionStringProvider : IConnectionStringProvider
    {
        public StaticConnectionStringProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}