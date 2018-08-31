[![Build Status](http://jenkinks-srv.northeurope.cloudapp.azure.com:8080/buildStatus/icon?job=DbKeeperNet/master)](http://jenkinks-srv.northeurope.cloudapp.azure.com:8080/job/DbKeeperNet/job/master/) [![Gitter char](https://badges.gitter.im/gitterHQ/gitter.png)](https://gitter.im/dbkeepernet/Lobby)

# DbKeeperNet

DbKeeperNet is a handy .Net Standard 2.0 component designed to offer users a simple, 
easy to use ADO.NET platform for distribution of relational database schema updates 
/ changes (database schema updater, RDBMS schema distribution tool).

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

```csharp
var serviceCollection = new ServiceCollection();
serviceCollection.UseDbKeeperNet(Configure);

ServiceProvider = serviceCollection.BuildServiceProvider(true);

DefaultScope = ServiceProvider.CreateScope();
```
