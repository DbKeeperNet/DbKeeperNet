using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using DbKeeperNet.Engine.Resources;

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
    public class MsSqlStepWithoutExplicitTransaction : ICustomUpdateStep
    {
        /// <summary>
        /// Method executed as an action during installation.
        /// </summary>
        /// <param name="context">Current update context</param>
        /// <param name="param">Optional parameters (with optional name) which can be passed thru the installation XML</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void ExecuteUpdate(IUpdateContext context, CustomUpdateStepParamType[] param)
        {
            if (context == null || !context.DatabaseService.IsDbType(@"MSSQL")) throw new InvalidOperationException(MsSqlStepWithoutExplicitTransactionMessages.OnlyMsSqlSupported);
            if (param == null || param.Length != 2) throw new ArgumentException(MsSqlStepWithoutExplicitTransactionMessages.CustomStepHasInvalidParameters);

            var connectionString = context.DatabaseService.Connection.ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    var stream = Assembly.Load(new AssemblyName(param[0].Value)).GetManifestResourceStream(param[1].Value); // disposed as a part of stream reader below

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

}
