[![Gitter char](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/dbkeepernet/Lobby)
![Build Status](http://jenkinks-srv.northeurope.cloudapp.azure.com:8080/buildStatus/icon?job=DbKeeperNet/master)

# DbKeeperNet

DbKeeperNet is a handy .Net Standard 2.0 (.NET 4.6.1+ and .NET Core 2.0+) component designed to offer users a simple, 
easy to use ADO.NET platform for distribution of relational database schema updates 
/ changes (database schema updater, RDBMS schema distribution tool).

It servers as an alternative for database migrations on projects where EF (Entity Framework) is not used.

You can use it freely in both commercial and non-commercial applications as you need.

# Support for different database engines:

* MSSQL 2000, MSSQL 2005, MSSQL 2008, MSSQL 2012 (including SQL EXPRESS).
* MySql Connector .NET support
* PostgreSQL (Npgsql connector)
* SQLite ADO.NET
* ~~Oracle Support (.NET built-in Oracle client or Oracle.ManagedDataAccess.Client)~~
* Firebird 2.5

# Other features:

* Allows .NET custom steps to be executed during the upgrade
* Database upgrade/migration described in a XML validated via a schema
* You have your database upgrade fully under your control
* Support for Asp.NET membership and role management
* Fully compatible with MONO 4.5, .NET 2.0+
* Easily extensible for new features

# Getting started

* First create an XML database upgrade script (see `demos\FullFrameworkConsoleApp\DatabaseUpgrade.xml` for an example)
  * In `demos` folder you can find additional examples for various usage scenarios
* Install necessary nuget packages (like `DbKeeperNet.Extensions.SQLite`)
* Register depedencies in dependency injection
* Resolve `IDatabaseUpdater` and call its `ExecuteUpgrade()` method

```csharp
const string connectionString = "Data Source=fullframeworkdemo.db3";

var serviceCollection = new ServiceCollection();
serviceCollection.AddDbKeeperNet(c =>
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
