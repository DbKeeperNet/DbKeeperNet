using System;
using System.Collections.Generic;
using System.Text;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that trigger with specified
    /// name and table doesn't exist in database.
    /// Condition reference name is <value>DbTriggerNotFound</value>.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Trigger TR_Update_Primary not found" Precondition="DbTriggerNotFound">
    ///   <Param>TR_Update_Primary</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </summary>
    public sealed class DbTriggerNotFound : IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbTriggerNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            if ((param == null) || (param.Length < 1) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.TriggerNameEmpty, Name));

            bool result = !context.DatabaseService.TriggerExists(param[0].Value);

            return result;
        }
        #endregion
    }
}
