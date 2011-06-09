using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that procedure with given name doesn't exist.
    /// Condition reference name is <value>DbProcedureNotFound</value>.
    /// It has one parameter which should contain tested database
    /// procedure name.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Procedure testing_proc not found" Precondition="DbProcedureNotFound">
    ///   <Param>testing_proc</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </summary>
    public sealed class DbProcedureNotFound : IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbProcedureNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");
            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.StoredProcedureNameEmpty, Name));

            bool result = !context.DatabaseService.StoredProcedureExists(param[0].Value);

            return result;
        }

        #endregion
    }
}
