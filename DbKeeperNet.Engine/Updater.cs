using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using DbKeeperNet.Engine.Resources;

namespace DbKeeperNet.Engine
{
    /// <summary>
    /// Main class responsible for invocation of updates in XML
    /// definition file.
    /// </summary>
    /// <remarks>
    /// <example>
    /// Prepare App.Config file. Following example is for execution of one embeded resource script followed
    /// by a disk fil on MSSQL server.
    /// <code>
    /// <![CDATA[
    /// <?xml version="1.0" encoding="utf-8" ?>
    /// <configuration>
    /// <configSections>
    /// <section name="dbkeeper.net" 
    ///     type="DbKeeperNet.Engine.DbKeeperNetConfigurationSection,DbKeeperNet.Engine"/>
    ///   </configSections>
    ///   <dbkeeper.net loggingService="fx">
    ///     <updateScripts>
    ///       <add provider="asm" location="DbKeeperNet.SimpleDemo.DatabaseSetup.xml,DbKeeperNet.SimpleDemo" />
    ///       <add provider="disk" location="c:\diskpath\DatabaseSetup.xml" />
    ///     </updateScripts>
    ///     <databaseServiceMappings>
    ///       <add connectString="default" databaseService="MsSql" />
    ///     </databaseServiceMappings>
    ///   </dbkeeper.net>
    ///   <connectionStrings>
    ///     <add name="default" 
    ///             connectionString="Data Source=.\SQLEXPRESS;
    ///             AttachDbFilename='|DataDirectory|\DbKeeperNetSimpleDemo.mdf';
    ///             Integrated Security=True;Connect Timeout=30;User Instance=True" 
    ///             providerName="System.Data.SqlClient"/>
    ///   </connectionStrings>
    /// </configuration>
    /// ]]>
    /// </code>
    /// </example>
    /// 
    /// <example>
    /// Add the code which will run the scripts from App.Config (see <see cref="UpdateContext.InitializeDatabaseService(string)"/>):
    /// <code>
    /// using (UpdateContext context = new UpdateContext())
    /// {
    ///     context.LoadExtensions();
    ///     context.InitializeDatabaseService("default");
    ///  
    ///     Updater updater = new Updater(context);
    ///     updater.ExecuteXmlFromConfig();
    /// }
    /// </code>
    /// 
    /// <example>
    /// Alternatively connection in the context can be initialized
    /// manually (see <see cref="UpdateContext.InitializeDatabaseService(DbKeeperNet.Engine.IDatabaseService,bool)"/>):
    /// <code>
    /// using (IUpdateContext context = new UpdateContext())
    /// using (IDatabaseService databaseService = new MsSqlDatabaseService(CreateDatabaseConnection()))
    /// {
    ///    context.LoadExtensions();
    ///    context.InitializeDatabaseService(databaseService, false);
    ///
    ///    using (Updater updater = new Updater(context))
    ///    {
    ///       updater.ExecuteXmlFromConfig();
    ///    }
    /// }
    /// </code>
    /// </example>
    /// 
    /// </example>
    /// 
    /// <example>
    /// Prepare an XML script which can be executed:
    /// <code>
    /// <![CDATA[
    /// <?xml version="1.0" encoding="utf-8"?>
    /// <upd:Updates xmlns:upd="http://code.google.com/p/dbkeepernet/Updates-1.0.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" AssemblyName="DbKeeperNet.SimpleDemo" xsi:schemaLocation="http://code.google.com/p/dbkeepernet/Updates-1.0.xsd ../DbKeeperNet.Engine/Resources/Updates-1.0.xsd">
    /// 	<!-- Default way how to check whether to execute update step or not -->
    /// 	<DefaultPreconditions>
    /// 		<!-- We will use step information saving strategy -->
    /// 		<Precondition FriendlyName="Update step executed" Precondition="StepNotExecuted"/>
    /// 	</DefaultPreconditions>
    /// 	<Update Version="1.00">
    /// 		<UpdateStep xsi:type="upd:UpdateDbStepType" FriendlyName="Create table DbKeeperNet_SimpleDemo" Id="1">
    /// 			<!-- DbType attribute may be ommited - it will result in default value all
    ///            which means all database types -->
    /// 			<AlternativeStatement DbType="MsSql"><![CDATA[
    ///           CREATE TABLE DbKeeperNet_SimpleDemo
    ///           (
    ///           id int identity(1, 1) not null,
    ///           name nvarchar(32),
    ///           constraint PK_DbKeeperNet_SimpleDemo primary key clustered (id)
    ///           )
    ///         ]]&gt;</AlternativeStatement>
    /// 			<AlternativeStatement DbType="MySql"><![CDATA[
    ///           CREATE TABLE DbKeeperNet_SimpleDemo
    ///           (
    ///           id int not null auto_increment,
    ///           name nvarchar(32),
    ///           constraint PK_DbKeeperNet_SimpleDemo primary key (id)
    ///           )
    ///         ]]&gt;</AlternativeStatement>
    /// 			<AlternativeStatement DbType="PgSql"><![CDATA[
    ///           CREATE TABLE DbKeeperNet_SimpleDemo
    ///           (
    ///           id serial not null,
    ///           name varchar(32),
    ///           constraint PK_DbKeeperNet_SimpleDemo primary key (id)
    ///           )
    ///         ]]&gt;</AlternativeStatement>
    ///         <AlternativeStatement DbType="SQLite">
    ///         <![CDATA[
    ///         CREATE TABLE DbKeeperNet_SimpleDemo
    ///           (
    ///           id integer not null,
    ///           name text,
    ///           constraint PK_DbKeeperNet_SimpleDemo primary key (id)
    ///           )
    ///         ]]&gt;</AlternativeStatement>
    ///       <AlternativeStatement DbType="Oracle">
    ///         <![CDATA[
    ///         CREATE TABLE "DBKEEPERNET_SIMPLEDEMO"
    ///         (
    ///         "ID"         NUMBER(10,0) NOT NULL,
    ///         "NAME"       VARCHAR2(32),
    ///         CONSTRAINT "PK_DBKEEPERNET_SIMPLEDEMO" PRIMARY KEY ("ID")
    ///         )
    ///         ]]&gt;
    ///       </AlternativeStatement>
    /// 		</UpdateStep>
    ///     <UpdateStep xsi:type="upd:UpdateDbStepType" Id="2" FriendlyName="Create sequence generator DBKEEPERNET_SIMPLEDEMO_SEQ">
    ///       <AlternativeStatement DbType="Oracle">
    ///         <![CDATA[CREATE sequence "DBKEEPERNET_SIMPLEDEMO_SEQ" ]]&gt;
    ///       </AlternativeStatement>
    ///     </UpdateStep>
    ///     <UpdateStep xsi:type="upd:UpdateDbStepType" Id="3" FriendlyName="Create identity trigger BI_DBKEEPERNET_SIMPLEDEMO">
    ///       <AlternativeStatement DbType="Oracle">
    ///         <![CDATA[CREATE trigger "BI_DBKEEPERNET_SIMPLEDEMO" 
    /// 		      before insert on "DBKEEPERNET_SIMPLEDEMO"
    /// 		      for each row 
    /// 			    begin  
    /// 				    select "DBKEEPERNET_SIMPLEDEMO_SEQ".nextval into :NEW.ID from dual;
    /// 			    end;]]&gt;
    ///       </AlternativeStatement>
    ///     </UpdateStep>
    /// 		<UpdateStep xsi:type="upd:UpdateDbStepType" FriendlyName="Fill table DbKeeperNet_SimpleDemo" Id="4">
    /// 			<AlternativeStatement><![CDATA[
    ///           insert into DbKeeperNet_SimpleDemo(name) values('First value')
    ///         ]]&gt;</AlternativeStatement>
    /// 		</UpdateStep>
    ///     <UpdateStep xsi:type="upd:UpdateDbStepType" FriendlyName="Fill table DbKeeperNet_SimpleDemo" Id="5">
    ///       <AlternativeStatement>
    ///         <![CDATA[
    ///           insert into DbKeeperNet_SimpleDemo(name) values('Second value')
    ///         ]]&gt;
    ///       </AlternativeStatement>
    ///     </UpdateStep>
    /// 	</Update>
    /// </upd:Updates>
    /// ]]>
    /// </code>
    /// </example>
    /// </remarks>
    public class Updater : IDisposable
    {
        #region Private fields

        private IUpdateContext _context;
        
        #endregion
        
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void ExecuteXmlInternal(Stream inputXml)
        {
            XmlReaderSettings settings = new XmlReaderSettings();

            _context.IterateAllSchemas((schemaUri, schema) => settings.Schemas.Add(schemaUri, XmlReader.Create(new StringReader(schema))));
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

                        _context.GetUpdateStepHandlerFor(step).Handle(step, _context);

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
        /// <list type="number">
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
                _context = null;
            }
        }
        #endregion
    }
}
