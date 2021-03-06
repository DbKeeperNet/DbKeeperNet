﻿using System;
using System.Data.Common;
using System.Globalization;
using DbKeeperNet.Engine;
using Microsoft.Data.Sqlite;

namespace DbKeeperNet.Extensions.SQLite
{
    public class SQLiteUpdateStepExecutedMarker : IUpdateStepExecutedMarker
    {
        private readonly IDatabaseService<SqliteConnection> _databaseService;
        private readonly IDatabaseServiceTransactionProvider<SqliteTransaction> _transactionProvider;
        private SqliteCommand _assemblySelect;
        private SqliteCommand _assemblyInsert;
        private SqliteCommand _versionSelect;
        private SqliteCommand _versionInsert;
        private SqliteCommand _stepSelect;
        private SqliteCommand _stepInsert;

        public SQLiteUpdateStepExecutedMarker(IDatabaseService<SqliteConnection> databaseService, IDatabaseServiceTransactionProvider<SqliteTransaction> transactionProvider)
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
            long? assemblyId = (long?)_assemblySelect.ExecuteScalar();

            if (!assemblyId.HasValue)
            {
                _assemblyInsert.Parameters[0].Value = assemblyName;
                assemblyId = Convert.ToInt64(_assemblyInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _versionSelect.Parameters[0].Value = assemblyId.Value;
            _versionSelect.Parameters[1].Value = version;
            long? versionId = (long?)_versionSelect.ExecuteScalar();

            if (!versionId.HasValue)
            {
                _versionInsert.Parameters[0].Value = assemblyId.Value;
                _versionInsert.Parameters[1].Value = version;
                versionId = Convert.ToInt64(_versionInsert.ExecuteScalar(), CultureInfo.InvariantCulture);
            }

            _stepSelect.Parameters[0].Value = versionId.Value;
            _stepSelect.Parameters[1].Value = stepNumber;
            long? stepId = (long?)_stepSelect.ExecuteScalar();

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
                _assemblySelect = new SqliteCommand();
                _assemblySelect.CommandText = "select id from dbkeepernet_assembly where assembly = @assembly";

                var assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = "@assembly";

                _assemblySelect.Parameters.Add(assembly);
            }
            if (_assemblyInsert == null)
            {
                _assemblyInsert = new SqliteCommand();
                _assemblyInsert.CommandText = "insert into dbkeepernet_assembly(assembly, created) values(@assembly, CURRENT_TIMESTAMP); select last_insert_rowid()";

                var assembly = _assemblySelect.CreateParameter();
                assembly.ParameterName = "@assembly";

                _assemblyInsert.Parameters.Add(assembly);
            }
        }
        private void SetupVersionDbCommands()
        {
            if (_versionSelect == null)
            {
                _versionSelect = new SqliteCommand();
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
                _versionInsert = new SqliteCommand();
                _versionInsert.CommandText = "insert into dbkeepernet_version(dbkeepernet_assembly_id, version, created) values(@assemblyId, @version, CURRENT_TIMESTAMP); select last_insert_rowid()";

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
                _stepSelect = new SqliteCommand();
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
                _stepInsert = new SqliteCommand();
                _stepInsert.CommandText = "insert into dbkeepernet_step(dbkeepernet_version_id, step, created) values(@versionId, @step, CURRENT_TIMESTAMP); select last_insert_rowid()";

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