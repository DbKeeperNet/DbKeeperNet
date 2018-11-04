using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Pgsql.Tests
{
    [TestFixture]
    public class PgsqlDatabaseExtensionValidator : DatabaseExtensionValidatorBase
    {
        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UsePgsql(ConnectionStrings.TestDatabase)
                ;
            
            configurationBuilder.Services.AddLogging(c => { c.AddConsole(); });
        }
    }
}