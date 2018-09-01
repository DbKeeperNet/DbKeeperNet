using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using DbKeeperNet.Engine.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DbKeeperNet.Engine.Tests
{
    public abstract class TestBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected IServiceScope DefaultScope { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbKeeperNet(Configure);
            
            ServiceProvider = serviceCollection.BuildServiceProvider(true);

            DefaultScope = ServiceProvider.CreateScope();


        }

        [TearDown]
        public virtual void Shutdown()
        {
            if (DefaultScope != null) DefaultScope.Dispose();
        }

        protected void ExecuteSqlAndIgnoreException(string sql, params object[] args)
        {
            ExecuteSqlAndIgnoreException(DefaultScope.ServiceProvider.GetService<IDatabaseService>(), sql, args);
        }

        protected static void ExecuteSqlAndIgnoreException(IDatabaseService service, string sql, params object[] args)
        {

            try
            {
                string command = String.Format(CultureInfo.InvariantCulture, sql, args);

                Console.WriteLine("Going to run {0}", command);
                var connection = service.GetOpenConnection();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (DbException e)
            {
                Console.WriteLine("Ignored DbException: {0}", e);
            }
        }

        protected T GetService<T>()
        {
            return DefaultScope.ServiceProvider.GetService<T>();
        }



        protected abstract void Configure(IDbKeeperNetBuilder configurationBuilder);
    }
}