using System;
using DbKeeperNet.Engine.Resources;
using System.Globalization;

namespace DbKeeperNet.Engine.Extensions.Preconditions
{
    /// <summary>
    /// Condition verifies that index with given name for given table doesn't exist.
    /// </summary>
    /// <remarks>
    /// Condition reference name is <c>DbIndexNotFound</c>.
    /// It has two parameters which should contain tested database
    /// index name and table name.
    /// </remarks>
    /// <example>
    /// Following example shows how to reference this condition in the
    /// update script XML.
    /// <code>
    /// <![CDATA[
    /// <Precondition FriendlyName="Index UQ_test not found" Precondition="DbIndexNotFound">
    ///   <Param>UQ_test</Param>
    ///   <Param>table_test_index</Param>
    /// </Precondition>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class DbIndexNotFound: IPrecondition
    {
        #region IPrecondition Members

        public string Name
        {
            get { return @"DbIndexNotFound"; }
        }

        public bool CheckPrecondition(IUpdateContext context, PreconditionParamType[] param)
        {
            if (context == null)
                throw new ArgumentNullException(@"context");
            if ((param == null) || (param.Length != 2) || (String.IsNullOrEmpty(param[0].Value)) || (String.IsNullOrEmpty(param[1].Value)))
                throw new ArgumentNullException(@"param", String.Format(CultureInfo.CurrentCulture, PreconditionMessages.IndexNameOrTableEmpty, Name));

            bool result = !context.DatabaseService.IndexExists(param[0].Value, param[1].Value);

            return result;
        }

        #endregion
    }
}
