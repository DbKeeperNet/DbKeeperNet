using System;
using System.Data.Common;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Extensions.SQLite;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FullFrameworkConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Data Source=fullframeworkdemo.db3";

            var serviceCollection = new ServiceCollection();
            serviceCollection.UseDbKeeperNet(c =>
            {
                c
                .UseSQLite(connectionString)
                .AddEmbeddedResourceScript("FullFrameworkConsoleApp.DatabaseUpgrade.xml,FullFrameworkConsoleApp");
            });
            serviceCollection.AddLogging(c => { c.AddConsole(); });

            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            using (var scope = serviceProvider.CreateScope())
            {
                var upgrader = scope.ServiceProvider.GetService<IDatabaseUpdater>();
                upgrader.ExecuteUpgrade();
            }

            using (var c = new SqliteConnection(connectionString))
            {
                c.Open();

                DbCommand cmd = c.CreateCommand();
                cmd.CommandText = "select * from DbKeeperNet_SimpleDemo";
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    Console.WriteLine("{0}: {1}", reader[0], reader[1]);
            }

            Console.ReadLine();
        }
    }
}
