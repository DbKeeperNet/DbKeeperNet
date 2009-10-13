using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using DbKeeperNet.Engine;
using log4net.Config;
using log4net;

namespace DbKeeperNet.Log4NetSimpleDemo
{
    class Program
    {
        private static ILog _logger = LogManager.GetLogger("Log4NetSimpleDemo");

        static void Main(string[] args)
        {
            _logger.Info("------- Started --------");

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
                    _logger.InfoFormat("{0}: {1}", reader[0], reader[1]);
            }
            
            _logger.Info("------- Finished --------");

            Console.ReadKey();
        }
    }
}
