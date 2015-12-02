using DbKeeperNet.Engine.Configuration;

namespace DbKeeperNet.Engine.Tests
{
    public class TestDbKeeperNetConfiguration : DbKeeperNetConfigurationX
    {
        public TestDbKeeperNetConfiguration()
        {
            DatabaseServiceMappings.Add(new DatabaseServiceMappingConfigurationElement
            {
                ConnectString = "mock",
                DatabaseService = "MockDriver"
            });
        }
    }

}