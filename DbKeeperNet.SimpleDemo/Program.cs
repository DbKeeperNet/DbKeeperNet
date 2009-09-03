using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;

namespace DbKeeperNet.SimpleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UpdateContext context = new UpdateContext())
            {
                context.LoadExtensions();
                context.InitializeDatabaseService("default");

                Updater updater = new Updater(context);
                updater.ExecuteXmlFromConfig();
            }

            ConnectionStringSettings connectString = ConfigurationManager.ConnectionStrings["default"];

            using (SqlConnection connection = new SqlConnection(connectString.ConnectionString))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from DbKeeperNet_SimpleDemo";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    Console.WriteLine("{0}: {1}", reader[0], reader[1]);
            }

            Console.ReadKey();
        }
    }
}
