using System;

namespace DbKeeperNet.Engine.Preconditions
{
    /// <summary>
    /// Condition verifies that current database service in current context
    /// supports DbType defined in the first parameter.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbType</c>.
    /// It has one parameter which should contain tested database
    /// type name as evaluated via <see cref="IDatabaseService.CanHandle"/> method.
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

    public class DbTypePrecondition : IPreconditionHandler
    {
        private readonly IDatabaseService _databaseService;
        
        public DbTypePrecondition(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public bool CanHandle(UpdateStepContextPrecondition context)
        {
            return context.Precondition.Precondition == @"DbType";
        }

        public bool IsMet(UpdateStepContextPrecondition context)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            var param = context.Precondition.Param;

            if ((param == null) || (param.Length == 0) || (string.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", "Database type for condition DbType must be specified");

            return _databaseService.CanHandle(param[0].Value);
        }
    }
}