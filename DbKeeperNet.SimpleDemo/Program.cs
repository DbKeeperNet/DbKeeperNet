using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine;

namespace DbKeeperNet.SimpleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UpdateContext context = new UpdateContext())
            {
                context.LoadExtensions();
                context.InitializeDatabaseService("default");

                Updater updater = new Updater(context);
                updater.ExecuteXmlFromConfig();
            }
        }
    }
}
