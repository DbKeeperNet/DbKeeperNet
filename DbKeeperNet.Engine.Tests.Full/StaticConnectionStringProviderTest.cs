using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests.Full
{
    [TestFixture]
    public class StaticConnectionStringProviderTest
    {
        [Test]
        public void ConnectionStringShouldReturnExpectedValue()
        {
            const string connectionString = "server=.;database=test";

            var provider = new StaticConnectionStringProvider(connectionString);

            Assert.That(provider.ConnectionString, Is.EqualTo(connectionString));
        }
    }
}