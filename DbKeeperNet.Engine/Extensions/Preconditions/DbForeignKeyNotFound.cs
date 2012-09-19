using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that foreign key with specified
    /// name doesn't exist for table in database.
    /// </summary>
    /// <remarks>
    /// <para>Condition reference name is <c>DbForeignKeyNotFound</c>.
    /// It has two parameters which should contain tested database
    /// foreign key name and table name.
    /// </para>
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Foreign key FK_test not found" Precondition="DbForeignKeyNotFound">
    ///   <Param>FK_test</Param>
    ///   <Param>table_test_fk</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class DbForeignKeyNotFound: IPrecondition
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
            if ((param == null) || (param.Length != 2) || (String.IsNullOrEmpty(param[0].Value)) || (String.IsNullOrEmpty(param[1].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.ForeignKeyNameEmpty, Name));

            bool result = !context.DatabaseService.ForeignKeyExists(param[0].Value, param[1].Value);

            return result;
        }

        #endregion
    }
}
