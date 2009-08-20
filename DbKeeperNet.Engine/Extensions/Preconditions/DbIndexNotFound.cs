using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that primary key or
    /// index with given name doesn't exist.
    /// Condition reference name is <value>DbIndexNotFound</value>.
    /// </summary>
    class DbIndexNotFound: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbIndexNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, string[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");
            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0])))
                throw new ArgumentNullException(@"param", String.Format("Index or primary key name for condition {0} must be specified", Name));

            bool result = !context.DatabaseService.IndexExists(param[0]);

            return result;
        }

        #endregion
    }
}
