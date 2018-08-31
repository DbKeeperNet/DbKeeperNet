using System.Data;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    public abstract class DatabaseServiceTests<T> : TestBase
        where T : DbConnection
    {
        [Test]
        public void GenericGetOpenConnectionShouldReturnOpenedConnection()
        {
            var service = DefaultScope.ServiceProvider.GetService<IDatabaseService<T>>();

           Assert.That(service.GetOpenConnection().State, Is.EqualTo(ConnectionState.Open));
        }

        [Test]
        public void GetOpenConnectionShouldReturnOpenedConnection()
        {
            var service = DefaultScope.ServiceProvider.GetService<IDatabaseService<T>>();

            Assert.That(service.GetOpenConnection().State, Is.EqualTo(ConnectionState.Open));
        }
    }
}
