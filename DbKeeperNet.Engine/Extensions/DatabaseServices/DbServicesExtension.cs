using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.DatabaseServices
{
    public sealed class DbServicesExtension : IExtension
    {
        #region IExtension Members

        public void Initialize(IUpdateContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.RegisterDatabaseService(new MsSqlDatabaseService());
            //  Those are not implemented yet
            //  context.RegisterDatabaseService(new MySqDbService());
            //  context.RegisterDatabaseService(new OracleDbService());
        }

        #endregion
    }
}
