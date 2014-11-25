An article describing a simple .NET library which simply keeps your database schema up-to-date.

# Introduction

Each project using database access solves how to distribute database schema and how to keep it up-to-date after upgrades. I was solving this problem multiple times, so I decided to write a common, easy to use, and freely available library. The result of this is the **DbKeeperNet** library which is pure ADO.NET framework (no dependency on Entity Framework).

This article will briefly show how to use **DbKeeperNet** library to fulfill this task. The library is designed as extensible and with planned support to any database engine.

# Supported Features

*   Very simple usage.
*   Database commands are kept in a simple, structured XML file.
*   Each upgrade step is executed in a separate transaction (if supported by the database service). In the case of failure, all further steps are prohibited.
*   Rich set of built-in preconditions used for evaluation whether update should or shouldn't be executed.
*   Support for unlimited and customizable list of database engines.
*   In single update, a script may be an alternative to SQL commands, for all database engine types if needed.
*   Support for custom preconditions.
*   Support for custom in-code upgrade steps (allows complex data transformations to be done in code instead of SQL).
*   DbKeeperNet provides deep logging of what is currently happening. Diagnostic output may be redirected through the standard .NET `System.Diagnostics.Trace` class or the `System.Diagnostics.TraceSource` class, or to a custom plug-in, allowing integration to an already existing application diagnostics framework.
*   XML update script structure is strictly defined by the XSD schema which can be used in any XML editor with auto-completion (intellisense).
*   Support for the Log4Net logging framework.
*   Support for MySQL Connect .NET.
*   Support for PostrgreSQL.
*   Support for SQLite.
*   Support for Oracle XE.
*   Support for Firebird.
*   Localizable log messages.
*   Support for customizable script sources (built-in are a disk file, embedded assembly).

# Background

There are two basic principles on how to get your application's database schema up-to-date:

*   Before each change, check directly in the database whether a change was already made or not (such as ask the database whether a table already exists or not).
*   Have a kind of database schema versioning table and record the current schema version.

DbKeeperNet supports both these principles; however, I suggest to use the second one.

DbKeeperNet's design for this second principle is in a unique identifier for each update step. The database service implementation simply keeps track of these already executed steps (concrete implementation is strongly dependent on the used database service). This allows you to very simply search the database and check which steps were already executed.

# Using DbKeeperNet

The code snippets below are taken from the _DbKeeperNet.SimpleDemo_ project which is part of the source control. If you want to directly execute the demo project, you need the SQL Server 2005 Express Edition installed, or you must change the connection string in _App.Config_.

For more complex scenarios, you can check the _DbKeeperNet.ComplexDemo_ project (there is an example of a custom step implementation, split XML scripts, etc.).

My favorite way to implement an upgrade script is by using an XML file stored as an embedded resource in an assembly. So, let's prepare a simple upgrade script with an alternative statement for two different database engines (you can find it in the _DbKeeperNet.Demo_ project as the file _DatabaseSetup.xml_):
```xml
<?xml version="1.0" encoding="utf-8" ?>
<upd:Updates xmlns:upd="http://code.google.com/p/dbkeepernet/Updates-1.0.xsd"
                    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
             xsi:schemaLocation=
        "http://code.google.com/p/dbkeepernet/Updates-1.0.xsd Updates-1.0.xsd"
                AssemblyName="DbKeeperNet.SimpleDemo">
  <!-- Default way how to check whether to execute update step or not -->
  <DefaultPreconditions>
    <!-- We will use step information saving strategy -->
    <Precondition FriendlyName="Update step executed" 
                Precondition="StepNotExecuted"/>
  </DefaultPreconditions>
  
  <Update Version="1.00">
    <UpdateStep xsi:type="upd:UpdateDbStepType" 
    FriendlyName="Create table DbKeeperNet_SimpleDemo" Id="1">
      <!-- DbType attribute may be omitted - it will result in default value all
           which means all database types -->
      <AlternativeStatement DbType="MsSql">
        <![CDATA[
          CREATE TABLE DbKeeperNet_SimpleDemo
          (
          id int identity(1, 1) not null,
          name nvarchar(32),
          constraint PK_DbKeeperNet_SimpleDemo primary key clustered (id)
          )
        ]]>
      </AlternativeStatement>
    </UpdateStep>
    <UpdateStep xsi:type="upd:UpdateDbStepType" 
    FriendlyName="Fill table DbKeeperNet_SimpleDemo" Id="2">
      <AlternativeStatement DbType="MsSql">
        <![CDATA[
          insert into DbKeeperNet_SimpleDemo(name) values('First value');
          insert into DbKeeperNet_SimpleDemo(name) values('Second value');
        ]]>
      </AlternativeStatement>
    </UpdateStep>
  </Update>
</upd:Updates>
```
Now, we will implement the necessary steps for the code execution:
```csharp
// Perform all configured database updates
using (UpdateContext context = new UpdateContext())
{
    context.LoadExtensions();
    context.InitializeDatabaseService("default");
 
    Updater updater = new Updater(context);
    updater.ExecuteXmlFromConfig();
}
// the above line is last required line for installation
// And now just print all inserted rows on console
// (just for demonstration purpose)
ConnectionStringSettings connectString = 
    ConfigurationManager.ConnectionStrings["default"];
 
using (SqlConnection connection = new SqlConnection(connectString.ConnectionString))
{
    connection.Open();
 
    SqlCommand cmd = connection.CreateCommand();
    cmd.CommandText = "select * from DbKeeperNet_SimpleDemo";
    SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
        Console.WriteLine("{0}: {1}", reader[0], reader[1]);
}
```
And finally, the setup configuration in the _App.config_ or _Web.Config_ file:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="dbkeeper.net" 
    type="DbKeeperNet.Engine.DbKeeperNetConfigurationSection,DbKeeperNet.Engine"/>
  </configSections>
  <dbkeeper.net loggingService="fx">
    <updateScripts>
      <add provider="asm" location="DbKeeperNet.SimpleDemo.DatabaseSetup.xml,DbKeeperNet.SimpleDemo" />
      <add provider="disk" location="c:\diskpath\DatabaseSetup.xml" />
    </updateScripts>
    <databaseServiceMappings>
      <add connectString="default" databaseService="MsSql" />
    </databaseServiceMappings>
  </dbkeeper.net>
  <connectionStrings>
    <add name="default" 
            connectionString="Data Source=.\SQLEXPRESS;
        AttachDbFilename='|DataDirectory|\DbKeeperNetSimpleDemo.mdf';
        Integrated Security=True;Connect Timeout=30;User Instance=True" 
            providerName="System.Data.SqlClient"/>
  </connectionStrings>
  <system.diagnostics>
    <!-- uncomment this for TraceSource class logger (fxts)-->
    <!--
    <sources>
      <source name="DbKeeperNet" switchName="DbKeeperNet">
        <listeners>
          <add name="file" />
        </listeners>
      </source>
    </sources>
    <switches>
      <add name="DbKeeperNet" value="Verbose"/>
    </switches>
    <sharedListeners>
      <add name="file" initializeData="dbkeepernetts.log" 
        type="System.Diagnostics.TextWriterTraceListener" />
    </sharedListeners>
    -->
    <trace autoflush="true">
      <!-- uncomment this for .NET Trace class logging (fx logger)-->
      <listeners>
        <add name="file" initializeData="dbkeepernet.log" 
        type="System.Diagnostics.TextWriterTraceListener" />
      </listeners>
    </trace>
  </system.diagnostics>
</configuration>
```
And that is all - all database changes are executed automatically, only in the case that they were not already executed.

# Writing Database Update Scripts

*   If you are using the _App.Config_ for the specification of executed XML scripts, all configured scripts are executed in the same order as they were defined in the configuration file. Also, the content of the XML file is processed exactly in the same order as it is written.
*   The `Assembly` attribute of the `Updates` element is in fact a namespace in which each `Version` and `Step` must be unique. If you would logically divide a single script into multiple files, you can use the same value in all the scripts.
*   The `Version` attribute of the `Update` element is intended to be used as a marker of database schema version. I suggest using a unique value for each distributed build changing the database schema (this value can be the same as the assembly version).
*   The `Step` attribute of the `UpdateStep` element should be unique inside each update version.
*   Never change the `AssemblyName`, `Version`, and `Step` steps after you deploy the application, unless you are absolutely sure what you are doing.&nbsp;

# Project location

If you have any questions, support requests, patches, your own extensions, you are looking for a binary package, documentation or if you are looking for the latest source files, the project is hosted at [http://github.com/DbKeeperNet/DbKeeperNet](http://github.com/DbKeeperNet/DbKeeperNet).

Alternatively you can reference DbKeeperNet as [Nuget packages](https://www.nuget.org/packages?q=dbkeepernet).

# Conclusion

This article shows only the basics from a set of supported functions. More information and examples of upgrade scripts can be find in the DbKeeperNet source files or in the unit tests.

# History

*   26th August, 2014: Update GitHub project reference
*   17th July, 2014: Project moved to GitHub
*   23rd September, 2012: Feature List updated, fixed App.Config example, update source package&nbsp;
*   4 June 2010: Feature list updated, new source package, updated examples according to new version.
*   15 November, 2009: Feature list updated, new source package.
*   4 September, 2009: Original article submitted.
