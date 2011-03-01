using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace DbKeeperNet.Engine.Tests.Extensions.DatabaseServices
{
    public abstract class DatabaseServiceTests<T>
        where T: IDatabaseService, new()
    {
        protected T CreateConnectedDbService()
        {
            T service = new T();

            return (T) service.CloneForConnectionString(ConnectionString);
        }

        protected static void ExecuteSQLAndIgnoreException(IDatabaseService service, string sql)
        {
            try
            {
                service.ExecuteSql(sql);
            }
            catch (DbException)
            {
                //    Debug.WriteLine("Ignored DbException: ", e.ToString());
            }
        }

        #region Private helper methods
        protected bool TestForeignKeyExists(string key, string table)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.ForeignKeyExists(key, table);
            }
            return result;
        }
        protected bool TestIndexExists(string index, string table)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.IndexExists(index, table);
            }
            return result;
        }
        protected bool TestPKExists(string index, string table)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.PrimaryKeyExists(index, table);
            }
            return result;
        }
        protected bool TestTriggerExists(string trigger)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.TriggerExists(trigger);
            }
            return result;
        }

        protected bool TestTableExists(string table)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.TableExists(table);
            }
            return result;
        }
        protected bool TestStoredProcedureExists(string procedure)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.StoredProcedureExists(procedure);
            }
            return result;
        }

        protected bool TestViewExists(string view)
        {
            bool result = false;

            using (IDatabaseService connectedService = CreateConnectedDbService())
            {
                result = connectedService.ViewExists(view);
            }
            return result;
        }
        #endregion

        protected abstract string ConnectionString { get; }
    }
}
