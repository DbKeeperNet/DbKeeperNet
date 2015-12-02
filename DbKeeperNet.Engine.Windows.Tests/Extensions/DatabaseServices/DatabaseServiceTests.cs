using System;
using System.Data.Common;
using System.Globalization;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Windows.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        protected DatabaseServiceTests(string connectString)
        {
            Assert.That(connectString, Is.Not.Empty);

            _connectionString = connectString;

            if (IsRunningOnMono())
                _connectionString = ConnectionString + "_mono";
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        private readonly string _connectionString;

        protected T CreateConnectedDbService()
        {
            T service = new T();

            return (T)service.CloneForConnectionString(ConnectionString);
        }

        protected static void ExecuteSqlAndIgnoreException(IDatabaseService service, string sql, params object[] args)
        {

            try
            {
                string command = String.Format(CultureInfo.InvariantCulture, sql, args);

                service.ExecuteSql(command);
            }
            catch (DbException e)
            {
            //    Console.WriteLine("Ignored DbException: {0}", e);
            }
        }

        #region Private helper methods
        
        protected bool TestStoredProcedureExists(string procedure)
        {
            bool result;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.StoredProcedureExists(procedure);
            }
            return result;
        }
        #endregion
    }
}
