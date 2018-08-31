using System.Data.SqlClient;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SqlServer.Tests
{
    [SetUpFixture]
    public class SqlServerSetupFixture
    {
        [OneTimeSetUp]
        public void CreateDatabase()
        {
            using (var connection = new SqlConnection(ConnectionStrings.MasterDatabase))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "create database " + ConnectionStrings.TestDatabaseName;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
