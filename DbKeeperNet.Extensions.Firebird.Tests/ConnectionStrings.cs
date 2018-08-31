namespace DbKeeperNet.Extensions.Firebird.Tests
{
    public static class ConnectionStrings
    {
        public static readonly string CommonConnectionString = @"datasource=localhost;user=sysdba;password=masterkey";
        public static readonly string MasterDatabase = CommonConnectionString + ";database=";
        public static readonly string TestDatabaseName = "testfb";
        public static readonly string TestDatabase = CommonConnectionString + ";database=" + TestDatabaseName;

    }
}