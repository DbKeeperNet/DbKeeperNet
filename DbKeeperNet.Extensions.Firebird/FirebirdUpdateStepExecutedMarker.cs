using System;
using System.Data;
using System.Globalization;
using DbKeeperNet.Engine;
using FirebirdSql.Data.FirebirdClient;

namespace DbKeeperNet.Extensions.Firebird
{
    public class FirebirdUpdateStepExecutedMarker : IUpdateStepExecutedMarker
    {
        private readonly IDatabaseService<FbConnection> _databaseService;
        private readonly IDatabaseServiceTransactionProvider<FbTransaction> _transactionProvider;
        private FbCommand _assemblySelect;
        private FbCommand _assemblyInsert;
        private FbCommand _versionSelect;
        private FbCommand _versionInsert;
        private FbCommand _stepSelect;
        private FbCommand _stepInsert;

        public FirebirdUpdateStepExecutedMarker(IDatabaseService<FbConnection> databaseService, IDatabaseServiceTransactionProvider<FbTransaction> transactionProvider)
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
            _assemblySelect = new FbCommand
            {
                CommandText = @"select ""id"" from ""dbkeepernet_assembly"" where ""assembly"" = @assembly",
                CommandType = CommandType.Text
            };

            var assemblySelect = _assemblySelect.CreateParameter();
            assemblySelect.ParameterName = "@assembly";

            _assemblySelect.Parameters.Add(assemblySelect);


            _assemblyInsert = new FbCommand
            {
                CommandText = @"insert into ""dbkeepernet_assembly"" (""assembly"", ""created"") values(@assembly, current_timestamp) returning ""id""",
                CommandType = CommandType.Text
            };

            var assembly = _assemblySelect.CreateParameter();
            assembly.ParameterName = "@assembly";

            _assemblyInsert.Parameters.Add(assembly);
        }
        private void SetupVersionDbCommands()
        {

            _versionSelect = new FbCommand
            {
                CommandText = @"select ""id"" from ""dbkeepernet_version"" where ""dbkeepernet_assembly_id"" = @assemblyId and ""version"" = @version",
                CommandType = CommandType.Text
            };

            var assemblyIdSelect = _assemblySelect.CreateParameter();
            assemblyIdSelect.ParameterName = "@assemblyId";

            var versionSelect = _assemblySelect.CreateParameter();
            versionSelect.ParameterName = "@version";

            _versionSelect.Parameters.Add(assemblyIdSelect);
            _versionSelect.Parameters.Add(versionSelect);

            _versionInsert = new FbCommand
            {
                CommandText = @"insert into ""dbkeepernet_version"" (""dbkeepernet_assembly_id"", ""version"", ""created"") values(@assemblyId, @version, current_timestamp) returning ""id""",
                CommandType = CommandType.Text
            };


            var assemblyId = _assemblySelect.CreateParameter();
            assemblyId.ParameterName = "@assemblyId";

            var version = _assemblySelect.CreateParameter();
            version.ParameterName = "@version";

            _versionInsert.Parameters.Add(assemblyId);
            _versionInsert.Parameters.Add(version);

        }
        private void SetupStepDbCommands()
        {
            _stepSelect = new FbCommand
            {
                CommandText = @"select ""id"" from ""dbkeepernet_step"" where ""dbkeepernet_version_id"" = @versionId and ""step"" = @step",
                CommandType = CommandType.Text
            };

            var versionIdSelect = _assemblySelect.CreateParameter();
            versionIdSelect.ParameterName = "@versionId";

            var stepSelect = _assemblySelect.CreateParameter();
            stepSelect.ParameterName = "@step";

            _stepSelect.Parameters.Add(versionIdSelect);
            _stepSelect.Parameters.Add(stepSelect);

            _stepInsert = new FbCommand
            {
                CommandText = @"insert into ""dbkeepernet_step"" (""dbkeepernet_version_id"", ""step"", ""created"") values(@versionId, @step, current_timestamp)",
                CommandType = CommandType.Text
            };

            var versionId = _assemblySelect.CreateParameter();
            versionId.ParameterName = "@versionId";

            var step = _assemblySelect.CreateParameter();
            step.ParameterName = "@step";

            _stepInsert.Parameters.Add(versionId);
            _stepInsert.Parameters.Add(step);
        }
    }
}