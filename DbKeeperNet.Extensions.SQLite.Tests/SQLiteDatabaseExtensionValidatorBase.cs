using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.SQLite.Tests
{
    [TestFixture]
    public class SQLiteDatabaseExtensionValidatorBase : DatabaseExtensionValidatorBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseSQLite("Data Source=endtoend.db3")
                ;

            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}