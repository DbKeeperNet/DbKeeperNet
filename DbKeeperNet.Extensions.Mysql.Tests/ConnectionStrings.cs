namespace DbKeeperNet.Extensions.Mysql.Tests
{
    public static class ConnectionStrings
    {
        public static readonly string CommonConnectionString = @"datasource=localhost;user=root";
        public static readonly string MasterDatabase = CommonConnectionString + ";database=mysql";
        public static readonly string TestDatabaseName = "testfb";
        public static readonly string TestDatabase = CommonConnectionString + ";database=" + TestDatabaseName;

    }
}
