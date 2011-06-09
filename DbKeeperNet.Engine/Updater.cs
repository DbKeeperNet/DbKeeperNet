using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Reflection;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

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

        private void ExecuteDatabaseSetupXml()
        {
            _context.Logger.TraceInformation(UpdaterMessages.DatabaseSetupCheck);

            Stream databaseSetup = _context.DatabaseService.DatabaseSetupXml;

            if (databaseSetup != null)
            {
                _context.Logger.TraceInformation(UpdaterMessages.DatabaseSetupToBeExecuted);
                ExecuteXmlInternal(databaseSetup);
                _context.Logger.TraceInformation(UpdaterMessages.DatabaseSetupFinished);
            }
            else
                _context.Logger.TraceInformation(UpdaterMessages.DatabaseSetupNotNecessary);
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

            _context.Logger.TraceInformation(UpdaterMessages.ExecutingUpdatesForAssembly, _context.CurrentAssemblyName);

            foreach (UpdateType update in updates.Update)
            {
                _context.CurrentVersion = update.Version;
                ProcessUpdate(update);
            }

            _context.Logger.TraceInformation(UpdaterMessages.ExecutingUpdatesForAssemblyFinished, _context.CurrentAssemblyName);
        }

        bool CheckStepPreconditions(IEnumerable<PreconditionType> preconditions)
        {
            _context.Logger.TraceInformation(UpdaterMessages.CheckingStepPreconditions, _context.CurrentStep);

            bool result = true;


            foreach (PreconditionType precondition in preconditions)
            {
                _context.Logger.TraceInformation(UpdaterMessages.CheckingStepPrecondition, precondition.Precondition, precondition.FriendlyName, DumpParams(precondition.Param));

                bool currentResult = false;

                try
                {
                    currentResult = _context.CheckPrecondition(precondition.Precondition, precondition.Param);

                    _context.Logger.TraceInformation(UpdaterMessages.CheckingPreconditionResult, precondition.Precondition, currentResult);
                }
                catch (NotSupportedException)
                {
                    _context.Logger.TraceWarning(UpdaterMessages.CheckingPreconditionNotSupported, precondition.Precondition, currentResult);
                }

                result &= currentResult;

                if (!result)
                    break;
            }

            _context.Logger.TraceInformation(UpdaterMessages.CheckingStepPreconditionsResult, result);

            return result;
        }
        void ExecuteStepSql(UpdateDbStepType step)
        {
            UpdateDbAlternativeStatementType usableStatement = null;
            UpdateDbAlternativeStatementType commonStatement = null;

            foreach (UpdateDbAlternativeStatementType statement in step.AlternativeStatement)
            {
                if (statement.DbType == "all")
                    commonStatement = statement;

                if (_context.DatabaseService.IsDbType(statement.DbType))
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
                _context.Logger.TraceWarning(UpdaterMessages.AlternativeSqlStatementNotFound, _context.DatabaseService.Name);
            }
        }

        void ExecuteStepCustom(CustomUpdateStepType step)
        {
            Type type = Type.GetType(step.Type);

            if (type == null)
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, UpdaterMessages.CustomStepTypeNotFound, step.Type));

            ICustomUpdateStep customStep = (ICustomUpdateStep)Activator.CreateInstance(type);
            customStep.ExecuteUpdate(_context, step.Param);
        }

        void ExecuteStepBody(UpdateStepBaseType step)
        {
            bool executed = false;
            UpdateDbStepType dbStep = step as UpdateDbStepType;

            if (dbStep != null)
            {
                ExecuteStepSql(dbStep);
                executed = true;
            }
            if (!executed)
            {
                CustomUpdateStepType customStep = step as CustomUpdateStepType;

                if (customStep != null)
                {
                    ExecuteStepCustom(customStep);
                    executed = true;
                }
            }
            
            if (!executed)
                throw new InvalidOperationException(UpdaterMessages.UnsupportedUpdateStepType);
        }
        void ExecuteStep(UpdateStepBaseType step)
        {
            try
            {
                _context.CurrentStep = step.Id;
                PreconditionType[] preconditions = step.Preconditions;

                if ((preconditions == null) || (preconditions.Length == 0))
                {
                    _context.Logger.TraceInformation(UpdaterMessages.UsingDefaultPreconditions);
                    preconditions = _context.DefaultPreconditions;
                }

                bool preconditionsResult = CheckStepPreconditions(preconditions);

                if (preconditionsResult)
                {
                    _context.Logger.TraceInformation(UpdaterMessages.StartingUpdateStep, step.Id, step.FriendlyName);
                    try
                    {
                        _context.DatabaseService.BeginTransaction();

                        ExecuteStepBody(step);
                        _context.Logger.TraceInformation(UpdaterMessages.FinishedUpdateStep, step.Id, step.FriendlyName);

                        if (step.MarkAsExecuted)
                        {
                            _context.DatabaseService.SetUpdateStepExecuted(_context.CurrentAssemblyName, _context.CurrentVersion, _context.CurrentStep);
                            _context.Logger.TraceInformation(UpdaterMessages.StepMarkedAsExecuted, step.Id, step.FriendlyName);
                        }

                        _context.DatabaseService.CommitTransaction();
                    }
                    catch
                    {
                        _context.Logger.TraceError(UpdaterMessages.StepExceptionRollback);
                        _context.DatabaseService.RollbackTransaction();
                        throw;
                    }
                }
                else
                    _context.Logger.TraceInformation(UpdaterMessages.StepSkipped, step.Id, step.FriendlyName);
            }
            finally
            {
                _context.CurrentStep = 0;
            }
        }

        void ProcessUpdate(UpdateType update)
        {
            _context.Logger.TraceInformation(UpdaterMessages.StartingVersion, update.Version, update.FriendlyName);

            foreach (UpdateStepBaseType step in update.UpdateStep)
                ExecuteStep(step);

            _context.Logger.TraceInformation(UpdaterMessages.FinishedVersion, update.Version, update.FriendlyName);
        }

        private static string DumpParams(IEnumerable<PreconditionParamType> param)
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
                _context.Logger.TraceError(UpdaterMessages.CaughtException, e.ToString());
                throw;
            }
            catch (Exception e)
            {
                _context.Logger.TraceError(UpdaterMessages.CaughtCommonException, e.ToString());
                throw new DbKeeperNetException(UpdaterMessages.CommonExceptionMessage, e);
            }
        }

        /// <summary>
        /// Executes all XML updates referenced in App.config file.
        /// </summary>
        /// <remarks>
        /// Updates are executed in two phases:
        /// <list>
        /// <item>Database setup for DbKeeperNet</item>
        /// <item>Each configured update in order defined in App.Config</item>
        /// </list>
        /// </remarks>
        /// <see cref="DbKeeperNetConfigurationSection"/>
        /// <exception cref="DbKeeperNetException"/>
        public void ExecuteXmlFromConfig()
        {
            try
            {
                ExecuteDatabaseSetupXml();

                foreach (UpdateScriptConfigurationElement e in _context.ConfigurationSection.UpdateScripts)
                {

                    ExecuteXmlInternal(_context.GetScriptFromStreamLocation(e.Provider, e.Location));
                }
            }
            catch (DbKeeperNetException)
            {
                throw;
            }
            catch (Exception e)
            {
                _context.Logger.TraceError(UpdaterMessages.CaughtCommonException, e.ToString());
                throw new DbKeeperNetException(UpdaterMessages.CommonExceptionMessage, e);
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
