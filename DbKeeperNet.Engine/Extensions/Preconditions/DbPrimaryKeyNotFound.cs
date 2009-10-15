using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that foreign primary with specified
    /// name and table doesn't exist in database.
    /// Condition reference name is <value>DbForeignKeyNotFound</value>.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Primary key PK_test not found" Precondition="DbPrimaryKeyNotFound">
    ///   <Param>PK_test</Param>
    ///   <Param>table_test_pk</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </summary>
    public sealed class DbPrimaryKeyNotFound: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbPrimaryKeyNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");
            if ((param == null) || (param.Length < 2) || (String.IsNullOrEmpty(param[0].Value)) || (String.IsNullOrEmpty(param[1].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.PrimaryKeyNameOrTableEmpty, Name));

            bool result = !context.DatabaseService.PrimaryKeyExists(param[0].Value, param[1].Value);

            return result;
        }

        #endregion
    }
}
