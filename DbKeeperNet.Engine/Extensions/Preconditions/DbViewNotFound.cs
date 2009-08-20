using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    class DbViewNotFound: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbViewNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, string[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0])))
                throw new ArgumentNullException(@"param", String.Format("View name for condition {0} must be specified", Name));

            bool result = !context.DatabaseService.ViewExists(param[0]);

            return result;
        }

        #endregion
    }
}
