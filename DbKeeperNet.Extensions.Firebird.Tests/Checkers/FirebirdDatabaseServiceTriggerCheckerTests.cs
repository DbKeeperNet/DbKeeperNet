using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Engine.Tests.Checkers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Extensions.Firebird.Tests.Checkers
{
    [TestFixture]
    public class FirebirdDatabaseServiceTriggerCheckerTests : TriggerCheckerTestBase
    {
        private const string TESTING_TABLE = @"firebird_testing_tr";

        protected override void Configure(IDbKeeperNetBuilder configurationBuilder)
        {
            configurationBuilder
                .UseFirebird(ConnectionStrings.TestDatabase)
                ;

            configurationBuilder.Services.AddLogging();
        }

        protected override void CreateDatabaseTrigger(string triggerName)
        {
            ExecuteSqlAndIgnoreException(@"create table ""{0}"" (id numeric(9,0) not null, id2 numeric(9,0))", TESTING_TABLE);
            ExecuteSqlAndIgnoreException(@"create trigger ""{0}"" for ""{1}"" after insert               
        		  as
                    begin
                    end", triggerName, TESTING_TABLE
            );
        }

        protected override void DropDatabaseTrigger(string triggerName)
        {
            ExecuteSqlAndIgnoreException(@"drop table ""{0}""", TESTING_TABLE);
        }

    }
}
