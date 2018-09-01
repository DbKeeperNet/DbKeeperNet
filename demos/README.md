# Introduction

Each project using database access solves how to distribute database schema and how to keep it up-to-date after upgrades. I was solving this problem multiple times, so I decided to write a common, easy to use, and freely available library. The result of this is the **DbKeeperNet** library which is pure ADO.NET framework (no dependency on Entity Framework).

This article will briefly show how to use **DbKeeperNet** library to fulfill this task. The library is designed as extensible and with planned support to any database engine.

It is basically a simple alternative to Entity Framework database migrations for projects which do not use EF.

# Supported Features

* .NET Standard 2.0
  * Supports .NET Core 2.0+
  * Supports .NET Framework 4.6.1+
* Very simple usage.
* Database commands are kept in a simple, structured XML file.
* Each upgrade step is executed in a separate transaction (if supported by the database service). In the case of failure, all further steps are prohibited.
* Rich set of built-in preconditions used for evaluation whether update should or shouldn't be executed.
* Support for unlimited and customizable list of database engines.
* In single update, a script may be an alternative to SQL commands, for all database engine types if needed.
* Support for custom preconditions.
* Support for custom in-code upgrade steps (allows complex data transformations to be done in code instead of SQL).
* DbKeeperNet provides deep logging of what is currently happening. Diagnostic output may be redirected through the standard .NET `System.Diagnostics.Trace` class or the `System.Diagnostics.TraceSource` class, or to a custom plug-in, allowing integration to an already existing application diagnostics framework.
* XML update script structure is strictly defined by the XSD schema which can be used in any XML editor with auto-completion (intellisense).
* Support for MySQL Connect .NET.
* Support for PostrgreSQL.
* Support for SQLite.
* Support for Microsoft SQL server.
* Support for Firebird.

# Background

There are two basic principles on how to get your application's database schema up-to-date:

* Before each change, check directly in the database whether a change was already made or not (such as ask the database whether a table already exists or not).
* Have a kind of database schema versioning table and record the current schema version.

DbKeeperNet supports both these principles; however, I suggest to use the second one.

DbKeeperNet's design for this second principle is in a unique identifier for each update step. The database service implementation simply keeps track of these already executed steps (concrete implementation is strongly dependent on the used database service). This allows you to very simply search the database and check which steps were already executed.

# Using DbKeeperNet

The code snippets below are taken from the _CoreConsoleApp_ project which is part of the source control. If you want to directly execute 
the demo project it should work against the SQLite database. 

For other database types you need to adjust connection string and setup appropriate database engine using the correct extension reference:

* `DbKeeperNet.Extensions.Mysql`  and its `UseMysql()`
* `DbKeeperNet.Extensions.Firebird`  and its `UseFirebird()`
* `DbKeeperNet.Extensions.Pgsql`  and its `UsePgsql()`
* `DbKeeperNet.Extensions.SQLite`  and its `UseSQLite()`
* `DbKeeperNet.Extensions.SqlServer`  and its `UseSqlServer()`

For more complex scenarios, you can check the _ComplexDemo_ project (there is an example of a custom step implementation, split XML scripts, etc.).

My favorite way to implement an upgrade script is by using an XML file stored as an embedded resource in an assembly. So, let's prepare a simple upgrade script with an alternative statement for two different database engines (you can find it in the _CoreConsoleApp_ demo project as the file _DatabaseUpgrade.xml_ 
which contains also alternative database statements):

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
const string connectionString = "Data Source=fullframeworkdemo.db3";

var serviceCollection = new ServiceCollection();
serviceCollection.UseDbKeeperNet(c =>
{
    c
    .UseSQLite(connectionString)
    .AddEmbeddedResourceScript("FullFrameworkConsoleApp.DatabaseUpgrade.xml,FullFrameworkConsoleApp");
});
serviceCollection.AddLogging(c => { c.AddConsole(); });

var serviceProvider = serviceCollection.BuildServiceProvider(true);

using (var scope = serviceProvider.CreateScope())
{
    var upgrader = scope.ServiceProvider.GetService<IDatabaseUpdater>();
    upgrader.ExecuteUpgrade();
}

// the above line is last required line for installation
// And now just print all inserted rows on console
// (just for demonstration purpose)
using (var c = new SqliteConnection(connectionString))
{
    c.Open();

    DbCommand cmd = c.CreateCommand();
    cmd.CommandText = "select * from DbKeeperNet_SimpleDemo";
    DbDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
        Console.WriteLine("{0}: {1}", reader[0], reader[1]);
}
```

And that is all - all database changes are executed automatically, only in the case that they were not already executed.

# Changes from previous releases

The XML is backward compatible so you just need to update the way how the database upgrade is invoked. This will give you flexibility of new versions
of the .NET framework including .NET Core 2.1.

# Demo projects

* [ASP.NET Core demo](https://github.com/DbKeeperNet/DbKeeperNet/tree/master/demos/ASPNETCore)
* [ASP.NET MVC demo](https://github.com/DbKeeperNet/DbKeeperNet/tree/master/demos/ASPNet)
* [.NET Core Console Application demo](https://github.com/DbKeeperNet/DbKeeperNet/tree/master/demos/CoreConsoleApp)
* [.NET Framework 4.6.1 demo](https://github.com/DbKeeperNet/DbKeeperNet/tree/master/demos/FullFrameworkConsoleApp) 

# Writing Database Update Scripts

* All scripts are executed in the same order as they were registered
* The `Assembly` attribute of the `Updates` element is in fact a namespace in which each `Version` and `Step` must be unique. If you would logically divide a single script into multiple files, you can use the same value in all the scripts.
* The `Version` attribute of the `Update` element is intended to be used as a marker of database schema version. I suggest using a unique value for each distributed build changing the database schema (this value can be the same as the assembly version).
* The `Step` attribute of the `UpdateStep` element should be unique inside each update version.
* Never change the `AssemblyName`, `Version`, and `Step` steps after you deploy the application, unless you are absolutely sure what you are doing.&nbsp;

# Project references

* [http://github.com/DbKeeperNet/DbKeeperNet](http://github.com/DbKeeperNet/DbKeeperNet).
* [![Gitter char](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/dbkeepernet/Lobby)

# Conclusion

This article shows only the basics from a set of supported functions. More information and examples of upgrade scripts can be find in the DbKeeperNet source files or in the unit tests.
