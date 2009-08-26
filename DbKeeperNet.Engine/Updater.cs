using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Reflection;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Main class responsible for invocation of updates in XML
    /// definition file.
    /// </summary>
    public class Updater : IDisposable
    {
        IUpdateContext _context;

        /// <summary>
        /// Class construction. Requires initialized update context.
        /// </summary>
        /// <param name="context">Update context instance with all required information prepared</param>
        public Updater(IUpdateContext context)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            _context = context;
        }

        #region Private methods
        bool CheckStepPreconditions(PreconditionType[] preconditions)
        {
            _context.Logger.TraceInformation("Checking step '{0}' preconditions", _context.CurrentStep);

            bool result = true;


            foreach (PreconditionType precondition in preconditions)
            {
                _context.Logger.TraceInformation("Checking precondition: {0} [{1}] {{{2}}}", precondition.Precondition, precondition.FriendlyName, DumpParams(precondition.Param));

                bool currentResult = _context.CheckPrecondition(precondition.Precondition, precondition.Param);

                _context.Logger.TraceInformation("Checking precondition: {0} [result={1}]", precondition.Precondition, currentResult);

                result &= currentResult;

                if (!result)
                    break;
            }

            _context.Logger.TraceInformation("Conditions result to: {0}", result);

            return result;
        }
        void ExecuteStepSql(UpdateDbStepType step)
        {
            UpdateDbAlternativeStatementType usableStatement = null;
            UpdateDbAlternativeStatementType commonStatement = null;

            foreach (UpdateDbAlternativeStatementType statement in step.AlternativeStatement)
            {
                if (statement.Driver == "all")
                    commonStatement = statement;

                if (_context.DatabaseService.Name == statement.Driver)
                {
                    usableStatement = statement;
                    break;
                }
            }

            if (usableStatement == null)
                usableStatement = commonStatement;

            if (usableStatement != null)
            {
                _context.DatabaseService.ExecuteSql(usableStatement.Value);
            }
            else
            {
                _context.Logger.TraceWarning("No relevant SQL statement found for driver: {0}", _context.DatabaseService.Name);
            }
        }

        void ExecuteStepCustom(CustomUpdateStepType step)
        {
            Type type = Type.GetType(step.Type);

            if (type == null)
                throw new ArgumentException("Custom step type not found");

            ICustomUpdateStep customStep = (ICustomUpdateStep)Activator.CreateInstance(type);
            customStep.ExecuteUpdate(_context, step.Param);
        }

        void ExecuteStepBody(UpdateStepBaseType step)
        {
            if (step is UpdateDbStepType)
            {
                ExecuteStepSql((UpdateDbStepType)step);
            }
            else if (step is CustomUpdateStepType)
            {
                ExecuteStepCustom((CustomUpdateStepType)step);
            }
            else
                throw new InvalidOperationException("Unsupported update step type");
        }
        void ExecuteStep(UpdateStepBaseType step)
        {
            try
            {
                _context.CurrentStep = step.Id;
                PreconditionType[] preconditions = step.Preconditions;

                if ((preconditions == null) || (preconditions.Length == 0))
                {
                    _context.Logger.TraceInformation("No preconditions declared - using context defaults");
                    preconditions = _context.DefaultPreconditions;
                }

                bool preconditionsResult = CheckStepPreconditions(preconditions);

                if (preconditionsResult)
                {
                    _context.Logger.TraceInformation("Running step: {0} [{1}]", step.Id, step.FriendlyName);
                    ExecuteStepBody(step);
                    _context.Logger.TraceInformation("Finished step: {0} [{1}]", step.Id, step.FriendlyName);

                    if (step.MarkAsExecuted)
                    {
                        _context.DatabaseService.SetUpdateStepExecuted(_context.CurrentAssemblyName, _context.CurrentVersion, _context.CurrentStep);
                        _context.Logger.TraceInformation("Step executed information saved to database: {0} [{1}]", step.Id, step.FriendlyName);
                    }
                }
                else
                    _context.Logger.TraceInformation("Skipping step: {0} [{1}]", step.Id, step.FriendlyName);
            }
            finally
            {
                _context.CurrentStep = 0;
            }
        }

        void ProcessUpdate(UpdateType update)
        {
            _context.Logger.TraceInformation("Running version: {0} [{1}]", update.Version, update.FriendlyName);

            foreach (UpdateStepBaseType step in update.UpdateStep)
                ExecuteStep(step);

            _context.Logger.TraceInformation("Finished version with success: {0} [{1}]", update.Version, update.FriendlyName);
        }

        private string DumpParams(PreconditionParamType[] param)
        {
            StringBuilder builder = new StringBuilder();

            if (param != null)
            {
                bool first = true;

                foreach (PreconditionParamType p in param)
                {
                    if (!first)
                        builder.Append(',');

                    builder.Append(p.Value);

                    first = false;
                }
            }
            return builder.ToString();
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Thru this method is executed each XML batch. Single object
        /// instance may be used for invocation of multiple XML update
        /// definitions.
        /// </summary>
        /// <param name="inputXml">Input XML stream, which is during processing validated against the definition schema.</param>
        public void ExecuteXml(Stream inputXml)
        {
            if (inputXml == null)
                throw new ArgumentNullException(@"inputXml");

            try
            {
                ExecuteDatabaseSetupXml();
                ExecuteXmlInternal(inputXml);
            }
            catch (DbKeeperNetException e)
            {
                _context.Logger.TraceError("An exception caught during update: {0}", e.ToString());
                throw;
            }
            catch (Exception e)
            {
                _context.Logger.TraceError("A common exception caught during update, will rethrow: {0}", e.ToString());
                throw new DbKeeperNetException("An error occured during updates execution", e);
            }
        }

        private void ExecuteDatabaseSetupXml()
        {
            _context.Logger.TraceInformation("Going to check database setup");

            Stream databaseSetup = _context.DatabaseService.GetDatabaseSetupXml();

            if (databaseSetup != null)
            {
                _context.Logger.TraceInformation("Database setup script will be executed");
                ExecuteXmlInternal(databaseSetup);
                _context.Logger.TraceInformation("Database setup script finished");
            }
            else
                _context.Logger.TraceInformation("No database setup necessary");

        }

        private void ExecuteXmlInternal(Stream inputXml)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(@"http://code.google.com/p/dbkeepernet/Updates-1.0.xsd", XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(@"DbKeeperNet.Engine.Resources.Updates-1.0.xsd")));

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(schemaSet);
            settings.IgnoreWhitespace = true;
            settings.ValidationType = ValidationType.Schema;

            Updates updates;

            using (XmlReader xmlReader = XmlReader.Create(inputXml, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Updates));

                updates = (Updates)serializer.Deserialize(xmlReader);
            }

            _context.CurrentAssemblyName = updates.AssemblyName;

            if (updates.DefaultPreconditions != null)
                _context.DefaultPreconditions = updates.DefaultPreconditions;

            _context.Logger.TraceInformation("Executing updates for assembly: {0}", _context.CurrentAssemblyName);

            foreach (UpdateType update in updates.Update)
            {
                _context.CurrentVersion = update.Version;
                ProcessUpdate(update);
            }

            _context.Logger.TraceInformation("Updates for assembly executed successfully: {0}", _context.CurrentAssemblyName);
        }
        /// <summary>
        /// Executes all XML updates referenced in App.config file
        /// </summary>
        /// <see cref="UpdaterConfiguration"/>
        public void ExecuteXmlFromConfig()
        {
            ExecuteDatabaseSetupXml();

            foreach (AssemblyUpdateConfigurationElement e in DbKeeperNetConfigurationSection.Current.AssemblyUpdates)
            {
                _context.Logger.TraceInformation("Going to execute configurated update '{0}' in assembly '{1}'", e.ManifestResource, e.Assembly);
                Assembly assembly = Assembly.Load(e.Assembly);
                Stream updates = assembly.GetManifestResourceStream(e.ManifestResource);

                if (updates == null)
                {
                    _context.Logger.TraceError("Manifest resource '{0}' in assembly '{1}' not found", e.ManifestResource, e.Assembly);
                    throw new DbKeeperNetException(String.Format("Manifest resource '{0}' in assembly '{1}' not found", e.ManifestResource, e.Assembly));
                }

                ExecuteXmlInternal(updates);

                _context.Logger.TraceInformation("Successfully executed configured update '{0}' in assembly '{1}'", e.ManifestResource, e.Assembly);
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context = null;
                }
            }
        }
        #endregion
    }
}
