using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

namespace DbKeeperNet.ComplexDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // const string connString = "pgsql"; // MySql connection over .NET Connector
            // const string connString = "mysql"; // MySql connection over .NET Connector
            const string connString = "default"; // MsSql connection

            using (UpdateContext context = new UpdateContext())
            {
                context.LoadExtensions();
                context.InitializeDatabaseService(connString);

                Updater updater = new Updater(context);
                updater.ExecuteXmlFromConfig();
            }

            ConnectionStringSettings connectString = ConfigurationManager.ConnectionStrings[connString];

            using (DbConnection connection = DbProviderFactories.GetFactory(connectString.ProviderName).CreateConnection())
            {
                connection.ConnectionString = connectString.ConnectionString;
                connection.Open();

                DbCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from DbKeeperNet_SimpleDemo";
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    Console.WriteLine("{0}: {1}", reader[0], reader[1]);
            }

            Console.ReadKey();
        }
    }
}
