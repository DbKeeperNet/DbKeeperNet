using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests
{
    [SetUpFixture]
    public class PgsqlSetupFixture
    {
        [OneTimeSetUp]
        public void CreateDatabase()
        {
            using (var connection = new Npgsql.NpgsqlConnection(ConnectionStrings.MasterDatabase))
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