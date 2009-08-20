using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that view with given name doesn't exist.
    /// Condition reference name is <value>DbViewNotFound</value>.
    /// It has one parameter which should contain tested database
    /// view name.
    /// <example>
    /// <![CDATA[
    /// <Precondition FriendlyName="View testing_view not found" Precondition="DbViewNotFound">
    ///   <Param>testing_view</Param>
    /// </Precondition>
    /// ]]>
    /// </example>
    /// </summary>
    class DbViewNotFound: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbViewNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format("View name for condition {0} must be specified", Name));

            bool result = !context.DatabaseService.ViewExists(param[0].Value);

            return result;
        }

        #endregion
    }
}
