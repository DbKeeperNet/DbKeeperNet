using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that foreign key with specified
    /// name doesn't exist in database.
    /// Condition reference name is <value>DbForeignKeyNotFound</value>.
    /// </summary>
    class DbForeignKeyNotFound: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbForeignKeyNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");
            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format("Foreign key name for condition {0} must be specified", Name));

            bool result = !context.DatabaseService.ForeignKeyExists(param[0].Value);

            return result;
        }

        #endregion
    }
}
