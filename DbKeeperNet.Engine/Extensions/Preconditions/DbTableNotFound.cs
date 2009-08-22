using System;
using System.Collections.Generic;
using System.Text;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that table with given name doesn't exist.
    /// Condition reference name is <value>DbTableNotFound</value>.
    /// It has one parameter which should contain tested database
    /// table name.
    /// <example>
    /// <![CDATA[
    /// <Precondition FriendlyName="Table testing_table not found" Precondition="DbTableNotFound">
    ///   <Param>testing_table</Param>
    /// </Precondition>
    /// ]]>
    /// </example>
    /// </summary>
    public sealed class DbTableNotFound : IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbTableNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");
            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format("Table name for condition {0} must be specified", Name));

            bool result = !context.DatabaseService.TableExists(param[0].Value);

            return result;
        }

        #endregion
    }
}
