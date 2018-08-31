namespace DbKeeperNet.Extensions.Pgsql.Tests
{
    public static class ConnectionStrings
    {
        public static readonly string CommonConnectionString = @"server=localhost;Integrated Security=true;Timeout=120";
        public static readonly string MasterDatabase = CommonConnectionString + ";database=postgres";
        public static readonly string TestDatabaseName = "testpgsql";
        public static readonly string TestDatabase = CommonConnectionString + ";database=" + TestDatabaseName;
        
    }
}