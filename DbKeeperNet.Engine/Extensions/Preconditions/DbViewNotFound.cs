using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that view with given name doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbViewNotFound</c>.
    /// It has one parameter which should contain tested database
    /// view name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="View testing_view not found" Precondition="DbViewNotFound">
    ///   <Param>testing_view</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class DbViewNotFound : IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbViewNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");

            if ((param == null) || (param.Length == 0) || (String.IsNullOrEmpty(param[0].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.ViewNameEmpty, Name));

            bool result = !context.DatabaseService.ViewExists(param[0].Value);

            return result;
        }

        #endregion
    }
}
