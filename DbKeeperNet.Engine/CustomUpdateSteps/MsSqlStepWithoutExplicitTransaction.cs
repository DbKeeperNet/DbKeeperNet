using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace DbKeeperNet.Engine.CustomUpdateSteps
{
    /// <summary>
    /// This custom steps executes the provided MSSQL script outside the explicit
    /// transaction for database upgrade
    /// </summary>
    /// <remarks>
    /// Use this steps very carefuly. It is for example useful when trying to setup
    /// the database for MSSQL Membership and Role provider.
    /// </remarks>
    /// <see cref="AspNetAccountCreateUpdateStepType"/>
    public class MsSqlStepWithoutExplicitTransaction : ICustomUpdateStep
    {
        /// <summary>
        /// Method executed as an action during installation.
        /// </summary>
        /// <param name="context">Current update context</param>
        /// <param name="param">Optional parameters (with optional name) which can be passed thru the installation XML</param>
        public void ExecuteUpdate(IUpdateContext context, CustomUpdateStepParamType[] param)
        {
            if (!context.DatabaseService.IsDbType("MSSQL")) throw new InvalidOperationException("Only Microsoft SQL database server is supported");
            if (param.Length != 2) throw new ArgumentException("Custom step should have exactly two parameters: name of the assembly with SQL script to be executed and name of the SQL script resource");

            var connectionString = context.DatabaseService.Connection.ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                using (var stream = Assembly.Load(param[0].Value).GetManifestResourceStream(param[1].Value))
                using (var script = new StreamReader(stream))
                {
                    var scriptText = script.ReadToEnd();
                    var sqlScriptSplitter = new MsSqlScriptSplitter();

                    foreach (var scriptPart in sqlScriptSplitter.SplitScript(scriptText))
                    {
                        context.Logger.TraceInformation(scriptPart);
                        command.CommandText = scriptPart;
                        command.Transaction = null;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }

}
