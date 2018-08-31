using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Mysql.Tests
{
    [SetUpFixture]
    public class MySqlSetupFixture
    {
        [OneTimeSetUp]
        public void CreateDatabase()
        {
            using (var connection = new MySqlConnection(ConnectionStrings.MasterDatabase))
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