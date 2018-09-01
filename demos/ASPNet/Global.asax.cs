using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DbKeeperNet.Engine;
using DbKeeperNet.Engine.Configuration;
using DbKeeperNet.Extensions.SQLite;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNet
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string ConnectionString = "Data Source=fullframeworkdemo.db3";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SetupDatabase();
        }

        private void SetupDatabase()
        {
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.UseDbKeeperNet(c =>
            {
                c
                    .UseSQLite(ConnectionString)
                    .AddEmbeddedResourceScript("ASPNet.DatabaseUpgrade.xml,ASPNet");
            });
            serviceCollection.AddLogging();

            var serviceProvider = serviceCollection.BuildServiceProvider(true);

            using (var scope = serviceProvider.CreateScope())
            {
                var upgrader = scope.ServiceProvider.GetService<IDatabaseUpdater>();
                upgrader.ExecuteUpgrade();
            }
        }
    }
}
