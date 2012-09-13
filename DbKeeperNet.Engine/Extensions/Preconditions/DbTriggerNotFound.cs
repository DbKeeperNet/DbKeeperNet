using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that trigger with specified
    /// name and doesn't exist in database.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbTriggerNotFound</c>.
    /// It has one parameter which should contain tested database
    /// trigger name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Trigger TR_Update_Primary not found" Precondition="DbTriggerNotFound">
    ///   <Param>TR_Update_Primary</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
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
