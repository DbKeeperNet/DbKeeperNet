using FirebirdSql.Data.FirebirdClient;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests
{
    [SetUpFixture]
    public class FirebirdSetupFixture
    {
        [OneTimeSetUp]
        public void CreateDatabase()
        {
            FbConnection.CreateDatabase(ConnectionStrings.TestDatabase);
        }
    }
}