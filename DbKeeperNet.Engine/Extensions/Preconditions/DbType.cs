using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that current database service in current context
    /// supports DbType defined in the first parameter.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbType</c>.
    /// It has one parameter which should contain tested database
    /// type name as evaluated via <see cref="IDatabaseService.IsDbType"/>.
    /// 
    /// The intention of this service is to be used as a precondition
    /// for custom steps which may depend on database type.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Database type is MSSQL" Precondition="DbType">
    ///   <Param>mssql</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class DbType: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbType"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.DatabaseTypeEmpty, Name));

            return context.DatabaseService.IsDbType(param[0].Value);
        }

        #endregion
    }
}
