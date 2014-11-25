# Introduction

It is a common task that as a part of database setup you need to seed some default user accounts and roles into the database as a part of setup.

[DbKeeperNet](http://www.codeproject.com/Articles/42091/DBKeeperNet-Keeps-Your-DB-Schema-Up-to-date) is an opensource .NET/C# framework which helps you manage database schema for your database performing various types of upgrade steps. DbKeeperNet is designed with support for various database types in mind. Currently it supports most common databases: _**MSSQL, SQLite, Firebird, MySQL**_, Oracle. Since the whole framework is extensible adding support of a new database type is just a simple task.

It is just matter of few minutes to get it set up in your project and have in-place infrastructure which will ensure proper upgrade path for your database schema on all installations for your application.

In its recent version [DbKeeperNet](http://www.codeproject.com/Articles/42091/DBKeeperNet-Keeps-Your-DB-Schema-Up-to-date)&nbsp;also support seeding of the ASP.NET membership and roles. With respect to overall design - this is supported on any database which implemented its membership providers and makes them available to the .NET infrastructure.

**Please keep in mind that any seeded account with pre-seeded password should change the seeded password ASAP.**

# Sample task

Let's consider the following set of operations you would perform in your database during the time as your application evolves (of course this can be mixed with any other database schema change like adding a table):

## Upgrade to version 1.00

*   If running on MSSQL setup the membership schema
*   Create role TestRole1
*   Create role TestRole2
*   Create user TestUser1 assigned to TestRole1 and TestRole2

## Upgrade to version 1.01

*   Create user TestUser2 assigned to TestRole1
*   Delete user TestUser1
*   Delete TestRole2

# Upgrade script

Part of this article is a console application demo project using the script below and referencing DbKeeperNet as a [Nuget package](https://www.nuget.org/packages/DbKeeperNet/):

*   The demo project is created for the MSSQL but it can be easily adopted for any other database just by changing setup in App.Config
    	*   The MSSQL specific step here is necessary since the DB schema creation script requires to be executed within a separated transaction and each of the steps needs to be commited
    *   For example MySQL providers seed the required schema on its own upon the first usage of membership providers.
*   It can be easily adopted and used in a web application - you simply have to plan for its initial execution.

[DbKeeperNet](http://www.codeproject.com/Articles/42091/DBKeeperNet-Keeps-Your-DB-Schema-Up-to-date) is using an XML validated against the XSD schema to define a database upgrade. Below you can find an example database upgrade script to achieve above steps (this is comming directly from the example):
```xml
<?xml version="1.0" encoding="utf-8"?>
<upd:Updates xmlns:upd="http://code.google.com/p/dbkeepernet/Updates-1.0.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" AssemblyName="DbKeeperNet.SimpleDemo" xsi:schemaLocation="http://code.google.com/p/dbkeepernet/Updates-1.0.xsd ../../DbKeeperNet.Engine/Resources/Updates-1.0.xsd">
  <!-- Default way how to check whether to execute update step or not -->
  <DefaultPreconditions>
    <!-- We will use step information saving strategy -->
    <Precondition FriendlyName="Update step executed" Precondition="StepNotExecuted"/>
  </DefaultPreconditions>
  <Update Version="1.00">
    <!--
    This update step is needed to properly inject MSSQL membership schema into the database.
    The challenge here is that the schema setup must be executed in separated transaction.
    
    For other membership providers this is usually handled automatically upon the first usage
    (like for MySql)
    -->
    <UpdateStep xsi:type="upd:CustomUpdateStepType" Id="1" Type="DbKeeperNet.Engine.CustomUpdateSteps.MsSqlStepWithoutExplicitTransaction, DbKeeperNet.Engine"  FriendlyName="Setting up database schema for membership and roles">
      <Preconditions>
        <Precondition FriendlyName="Update step executed" Precondition="StepNotExecuted"/>
        <Precondition FriendlyName="Database is MSSQL" Precondition="DbType">
          <Param>MSSQL</Param>
        </Precondition>
      </Preconditions>
      <Param>DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup</Param>
      <Param>DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup.MsSqlMembershipAndRolesSetup.sql</Param>
    </UpdateStep>
    <!-- Create some seeded roles -->
    <UpdateStep xsi:type="upd:AspNetRoleCreateUpdateStepType" FriendlyName="Create role TestRole1" Id="2" RoleName="TestRole1"/>
    <UpdateStep xsi:type="upd:AspNetRoleCreateUpdateStepType" Id="3" RoleName="TestRole2"/>
    <!-- Seed an account and associate it with roles -->
    <UpdateStep xsi:type="upd:AspNetAccountCreateUpdateStepType" Id="4" UserName="TestUser1" Mail="testuser1@domain.com" Password="SeededPassword">
      <Role>TestRole1</Role>
      <Role>TestRole2</Role>
    </UpdateStep>
  </Update>
  <Update Version="1.01">
    <UpdateStep xsi:type="upd:AspNetAccountCreateUpdateStepType" Id="1" UserName="TestUser2" Mail="testuser2@domain.com" Password="SeededPassword2">
      <Role>TestRole1</Role>
    </UpdateStep>
    <!-- Delete the seeded role -->
    <UpdateStep xsi:type="upd:AspNetRoleDeleteUpdateStepType" Id="2" RoleName="TestRole2"/>
    <!-- Delete one of the seeded accounts -->
    <UpdateStep xsi:type="upd:AspNetAccountDeleteUpdateStepType" Id="3" UserName="TestUser1"/>
  </Update>
</upd:Updates>
```

Now let's setup the C# portion which executes the script:
```csharp
const string connString = "default"; // MsSql connection   
using (UpdateContext context = new UpdateContext())
{
    context.LoadExtensions();
    context.InitializeDatabaseService(connString);

    Updater updater = new Updater(context);
    updater.ExecuteXmlFromConfig();
}
Console.WriteLine("Can login as TestUser2: " + Membership.Provider.ValidateUser("Testuser2", "SeededPassword2"));
Console.WriteLine("Can login as TestUser2: " + Membership.Provider.ValidateUser("testuser2", "InvalidPassword"));
Console.WriteLine("Is user testuser2 in role testrole1: " + Roles.Provider.IsUserInRole("testuser2", "testrole1"));
Console.WriteLine("Is user testuser2 in role testrole2: " + Roles.Provider.IsUserInRole("testuser2", "testrole2"));
```
And appropriate App.Config portion:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="dbkeeper.net" type="DbKeeperNet.Engine.DbKeeperNetConfigurationSection,DbKeeperNet.Engine"/>
  </configSections>
  <dbkeeper.net loggingService="fx">
    <updateScripts>
<!-- This is the location of the DB Upgrade script - we use an embedded resource -->
      <add provider="asm" location="DbKeeperNet.AspNetMembershipDemo.DatabaseSetup.xml,DbKeeperNet.AspNetMembershipDemo" />
    </updateScripts>
    <databaseServiceMappings>
      <add connectString="default" databaseService="MsSql" />
    </databaseServiceMappings>
  </dbkeeper.net>
  <connectionStrings>
    <!-- Change this to correct absolute path for the demo or to an actual database -->
    <add name="default" connectionString="Data Source=.\SQLEXPRESS;AttachDbFilename='C:\Users\voloda\MyRoot\Development\GIT\DbKeeperNet\DbKeeperNet\Demos\DbKeeperNet.AspNetMembershipDemo\bin\Debug\DbKeeperNetAspNetMembershipDemo.mdf';Integrated Security=True;Connect Timeout=30;User Instance=True;Initial catalog=DbKeeperNetAspNetMembershipDemo" providerName="System.Data.SqlClient"/>
  </connectionStrings>
<!-- Let's enable MSSQL membership providers -->
  <system.web>
    <membership defaultProvider="AspNetSqlMembershipProvider">
      <providers>
        <clear/>
        <add name="AspNetSqlMembershipProvider" type="System.Web.Security.SqlMembershipProvider, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" connectionStringName="default" enablePasswordRetrieval="false" enablePasswordReset="true" requiresQuestionAndAnswer="false" requiresUniqueEmail="false" passwordFormat="Hashed" maxInvalidPasswordAttempts="5" minRequiredPasswordLength="6" minRequiredNonalphanumericCharacters="0" passwordAttemptWindow="10" passwordStrengthRegularExpression="" applicationName="/"/>
      </providers>
    </membership>
    <roleManager enabled="true" defaultProvider="AspNetSqlRoleProvider">
      <providers>
        <clear/>
        <add connectionStringName="default" applicationName="/" name="AspNetSqlRoleProvider" type="System.Web.Security.SqlRoleProvider, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"/>
      </providers>
    </roleManager>
    <profile enabled="false">
      <providers>
        <clear/>
      </providers>
    </profile>
  </system.web>
</configuration>
```

# References

* [Demo project download](https://drive.google.com/file/d/0B27zqy23aL3BblVFNGJPdF9XVXM/view?usp=sharing)
* [DbKeeperNet](http://github.com/DbKeeperNet/DbKeeperNet) project (contains also some demo projects)
* DbKeeperNet [article](http://www.codeproject.com/Articles/42091/DBKeeperNet-Keeps-Your-DB-Schema-Up-to-date) on Codeproject
* Nuget packages
 * [https://www.nuget.org/packages/DbKeeperNet/](https://www.nuget.org/packages/DbKeeperNet/)
 * [https://www.nuget.org/packages/DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup/](https://www.nuget.org/packages/DbKeeperNet.Extensions.MsSqlMembershipAndRolesSetup/)

# History

*   Initial version
