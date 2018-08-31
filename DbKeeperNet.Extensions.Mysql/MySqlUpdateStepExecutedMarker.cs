using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using DbKeeperNet.Engine;
using MySql.Data.MySqlClient;

namespace DbKeeperNet.Extensions.Mysql
{
    public class MySqlUpdateStepExecutedMarker : IUpdateStepExecutedMarker
    {
        private readonly IDatabaseService<MySqlConnection> _databaseService;
        private readonly IDatabaseServiceTransactionProvider<MySqlTransaction> _transactionProvider;
        private MySqlCommand _assemblySelect;
        private MySqlCommand _assemblyInsert;
        private MySqlCommand _versionSelect;
        private MySqlCommand _versionInsert;
        private MySqlCommand _stepSelect;
        private MySqlCommand _stepInsert;

        public MySqlUpdateStepExecutedMarker(IDatabaseService<MySqlConnection> databaseService, IDatabaseServiceTransactionProvider<MySqlTransaction> transactionProvider)
        {
            _databaseService = databaseService;
            _transactionProvider = transactionProvider;
            SetupDbCommands();
        }

        public void MarkAsExecuted(string assemblyName, string version, int stepNumber)
        {
            var transaction = _transactionProvider.GetTransaction();

            _assemblyInsert.Transaction = transaction;
            _assemblySelect.Transaction = transaction;
            _versionInsert.Transaction = transaction;
            _versionSelect.Transaction = transaction;
            _stepInsert.Transaction = transaction;
            _stepSelect.Transaction = transaction;
            
            var connection = _databaseService.GetOpenConnection();
            if (connection == null)
            {
                throw new InvalidOperationException();
            }

            _assemblyInsert.Connection = connection;
            _assemblySelect.Connection = connection;
            _versionInsert.Connection = connection;
            _versionSelect.Connection = connection;
            _stepInsert.Connection = connection;
            _stepSelect.Connection = connection;

            _assemblySelect.Parameters[0].Value = assemblyName;
            int? assemblyId = (int?)_assemblySelect.ExecuteScalar();

            if (!assemblyId.HasValue)
            {
                _assemblyInsert.Parameters[0].Value = assemblyName;
                assemblyId = Convert.ToInt32(_assemblyInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _versionSelect.Parameters[0].Value = assemblyId.Value;
            _versionSelect.Parameters[1].Value = version;
            int? versionId = (int?)_versionSelect.ExecuteScalar();

            if (!versionId.HasValue)
            {
                _versionInsert.Parameters[0].Value = assemblyId.Value;
                _versionInsert.Parameters[1].Value = version;
                versionId = Convert.ToInt32(_versionInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _stepSelect.Parameters[0].Value = versionId.Value;
            _stepSelect.Parameters[1].Value = stepNumber;
            int? stepId = (int?)_stepSelect.ExecuteScalar();

            if (!stepId.HasValue)
            {
                _stepInsert.Parameters[0].Value = versionId.Value;
                _stepInsert.Parameters[1].Value = stepNumber;
                _stepInsert.ExecuteNonQuery();
            }
        }

        private void SetupDbCommands()
        {
            SetupAssemblyDbCommands();
            SetupVersionDbCommands();
            SetupStepDbCommands();
        }

        private void SetupAssemblyDbCommands()
        {
            if (_assemblySelect == null)
            {
                _assemblySelect = new MySqlCommand();
                _assemblySelect.CommandText = "select id from dbkeepernet_assembly where assembly = @assembly";
                _assemblySelect.CommandType = CommandType.Text;

                var assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = "@assembly";

                _assemblySelect.Parameters.Add(assembly);
            }
            if (_assemblyInsert == null)
            {
                _assemblyInsert = new MySqlCommand();
                _assemblyInsert.CommandText = "insert into dbkeepernet_assembly(assembly, created) values(@assembly, now()); select last_insert_id()";
                _assemblyInsert.CommandType = CommandType.Text;

                var assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = "@assembly";

                _assemblyInsert.Parameters.Add(assembly);
            }
        }
        private void SetupVersionDbCommands()
        {
            if (_versionSelect == null)
            {
                _versionSelect = new MySqlCommand();
                _versionSelect.CommandText = "select id from dbkeepernet_version where dbkeepernet_assembly_id = @assemblyId and version = @version";


                DbParameter assemblyId = _assemblySelect.CreateParameter();
                assemblyId.ParameterName = "@assemblyId";

                DbParameter version = _assemblySelect.CreateParameter();
                version.ParameterName = "@version";

                _versionSelect.Parameters.Add(assemblyId);
                _versionSelect.Parameters.Add(version);
            }
            if (_versionInsert == null)
            {
                _versionInsert = new MySqlCommand();
                _versionInsert.CommandText = "insert into dbkeepernet_version(dbkeepernet_assembly_id, version, created) values(@assemblyId, @version, now()); select last_insert_id() ";

                DbParameter assemblyId = _assemblySelect.CreateParameter();
                assemblyId.ParameterName = "@assemblyId";

                DbParameter version = _assemblySelect.CreateParameter();
                version.ParameterName = "@version";

                _versionInsert.Parameters.Add(assemblyId);
                _versionInsert.Parameters.Add(version);
            }
        }
        private void SetupStepDbCommands()
        {
            if (_stepSelect == null)
            {
                _stepSelect = new MySqlCommand();
                _stepSelect.CommandText = "select id from dbkeepernet_step where dbkeepernet_version_id = @versionId and step = @step";

                var versionId = _assemblySelect.CreateParameter();
                versionId.ParameterName = "@versionId";

                var step = _assemblySelect.CreateParameter();
                step.ParameterName = "@step";

                _stepSelect.Parameters.Add(versionId);
                _stepSelect.Parameters.Add(step);
            }
            if (_stepInsert == null)
            {
                _stepInsert = new MySqlCommand();
                _stepInsert.CommandText = "insert into dbkeepernet_step(dbkeepernet_version_id, step, created) values(@versionId, @step, now())";

                DbParameter versionId = _assemblySelect.CreateParameter();
                versionId.ParameterName = "@versionId";

                DbParameter step = _assemblySelect.CreateParameter();
                step.ParameterName = "@step";

                _stepInsert.Parameters.Add(versionId);
                _stepInsert.Parameters.Add(step);
            }
        }

    }
}