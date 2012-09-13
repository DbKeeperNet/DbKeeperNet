using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that table with given name doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbTableNotFound</c>.
    /// It has one parameter which should contain tested database
    /// table name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Table testing_table not found" Precondition="DbTableNotFound">
    ///   <Param>testing_table</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
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
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.TableNameEmpty, Name));

            bool result = !context.DatabaseService.TableExists(param[0].Value);

            return result;
        }

        #endregion
    }
}
