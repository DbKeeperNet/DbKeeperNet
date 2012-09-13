using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that primary key with specified
    /// name and table doesn't exist in database.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbPrimaryKeyNotFound</c>.
    /// It has two parameters which should contain tested database
    /// primary key name and table name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Primary key PK_test not found" Precondition="DbPrimaryKeyNotFound">
    ///   <Param>PK_test</Param>
    ///   <Param>table_test_pk</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
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
            if ((param == null) || (param.Length != 2) || (String.IsNullOrEmpty(param[0].Value)) || (String.IsNullOrEmpty(param[1].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.PrimaryKeyNameOrTableEmpty, Name));

            bool result = !context.DatabaseService.PrimaryKeyExists(param[0].Value, param[1].Value);

            return result;
        }

        #endregion
    }
}
